/***
* Copyright (c) 2017 Koudura Ninci @True.Inc
*
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in all
* copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
* SOFTWARE.
*
**/

using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Util.Collections;
using Fornax.Net.Util.IO.Readers;
using Fornax.Net.Util.Security.Cryptography;
using ProtoBuf;

namespace Fornax.Net.Document
{
    /// <summary>
    /// Representation of a File-System Document on fornax, this accepts files from a 
    /// file-system disk then processes it and converts it to a recognizable format 
    /// for fornax.
    /// </summary>
    [Serializable, ProtoContract]
    public class FSDocument : IDocument, java.io.Serializable.__Interface
    {
        private static FileFormat format;
        private readonly ulong docId;
        private TokenStream tokens;

        private static string _path;
        private static string _name;
        private Snippet _snippet;

        /// <summary>
        /// Gets the tokens of the text.
        /// </summary>
        /// <value>
        /// The tokens.
        /// </value>
        public TokenStream Tokens => tokens;

        #region ctors
        /// <summary>
        /// Initializes a new instance of the <see cref="FSDocument"/> class.
        /// </summary>
        /// <param name="doc">The document.</param>
        /// <param name="tokens">The tokens.</param>
        /// <exception cref="ArgumentNullException">tokens</exception>
        public FSDocument(FileInfo doc, TokenStream tokens, Extractor extractor = Extractor.Default) {
            Contract.Requires(doc != null && tokens != null);
            if (doc == null || tokens == null) throw new ArgumentNullException(nameof(tokens));

            this.tokens = tokens;
            format = GetFormat(doc);
            _path = doc.FullName;
            _name = Path.GetFileNameWithoutExtension(doc.FullName);
            docId = Adler32.Compute(doc.FullName);
            _snippet = CreateSnippet(this.tokens);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FSDocument"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="tokenizer">The tokenizer.</param>
        public FSDocument(string fileName, Tokenizer tokenizer, Extractor extractor = Extractor.Default)
            : this(new FileInfo(fileName) ?? throw new ArgumentNullException(nameof(fileName)), tokenizer) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FSDocument"/> class.
        /// </summary>
        /// <param name="file">The file to be parsed.</param>
        /// <param name="tokenizer">The tokenizer.</param>
        public FSDocument(FileInfo file, Tokenizer tokenizer, Extractor extractor = Extractor.Default)
            : this(file, GetContent(tokenizer, extractor, file.FullName).Result) {
        }

        #endregion


        #region props        
        /// <summary>
        /// Gets the link to the location of this document on the file-system.
        /// </summary>
        /// <value>
        /// The link to file;
        /// </value>
        [ProtoMember(1)]
        public string Link => _path;

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [ProtoMember(2)]
        public string Name => _name;

        /// <summary>
        /// Gets or sets a capture snippet for this document.
        /// </summary>
        /// <value>
        /// The capture.
        /// </value>
        [ProtoMember(4)]
        public Snippet Capture { get { return _snippet; } internal set { _snippet = value; } }

        /// <summary>
        /// Gets the fornax.net recognizable extension format of this document.
        /// </summary>
        /// <value>
        /// The format.
        /// </value>
        [ProtoMember(5)]
        public FileFormat Format => format;

        /// <summary>
        /// Gets the unique identifier for this document.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        [ProtoMember(6)]
        public ulong ID => docId;
        #endregion


        #region operations
        private Snippet CreateSnippet(TokenStream tokens) {
            return new Snippet(0, 100, tokens);
        }
        private static async Task<TokenStream> GetContent(Tokenizer tokenizer, Extractor ext, string path) {
            if (tokenizer == null) {
                throw new ArgumentNullException(nameof(tokenizer));
            }
            var tok = await getTokenizer(tokenizer, ext, path);
            return tok.GetTokens();
        }
        private static async Task<Tokenizer> getTokenizer(Tokenizer tokenizer, Extractor extractor, string path) {
            var txt = await GetText(extractor, path);
            return await GetTokenizer(tokenizer, string.Format("{0} {1}", txt, _name));
        }
        private static async Task<Tokenizer> GetTokenizer(Tokenizer tempTok, string text) {
            return await Task.Factory.StartNew(() => GetTok(tempTok, text));
        }
        private static Tokenizer GetTok(Tokenizer tempTok, string txt) {
            if (tempTok is CharTokenizer) {
                var chTok = tempTok as CharTokenizer;
                return (chTok.text == string.Empty) ? new CharTokenizer(txt) : new CharTokenizer(chTok.text);
            } else if (tempTok is BiasTokenizer) {
                var bTok = tempTok as BiasTokenizer;
                return (bTok.text == string.Empty) ? new BiasTokenizer(txt) : new BiasTokenizer(bTok.text);
            } else if (tempTok is NumericTokenizer) {
                var numTok = tempTok as NumericTokenizer;
                return (numTok.text == string.Empty) ? new NumericTokenizer(txt) : new NumericTokenizer(numTok.text);
            } else {
                var wsTok = tempTok as WhitespaceTokenizer;
                return (wsTok.text == string.Empty) ? new WhitespaceTokenizer(txt) : new WhitespaceTokenizer(wsTok.text);
            }
        }
        private static async Task<string> GetText(Extractor ext, string path) {
            FornaxReader reader = new FornaxReader(path);
            switch (ext) {
                case Extractor.Minimal:
                    return await GetState(reader, path);
                case Extractor.Buffered:
                    return await reader.BufferReadAsync();
                case Extractor.Default:
                    return await CompileTextTika(reader);
                default:
                    return await CompileText(reader);
            }
        }
        private static async Task<string> GetState(FornaxReader reader, string path) {
            if (FormatExt.Parse(Path.GetExtension(path)) == FileFormat.Xml) {
                return await reader.XmlReadAsync();
            }
            return await reader.StreamReadAsync();
        }
        private static async Task<string> CompileTextTika(FornaxReader reader) {
            var read = await reader.TikaReadAsync();
            var sbuilder = new StringBuilder();
            foreach (var str in read.Metadata) {
                sbuilder.AppendFormat("{0} : {1}\n", str.Key, str.Value);
            }
            sbuilder.Append("\n").Append(read.Text);
            return sbuilder.ToString();
        }
        private static async Task<string> CompileText(FornaxReader reader) {
            if (format.GetFornaxFormat() == FornaxFormat.Email) {
                var txt = await reader.ToxyEmailReadAsync();
                return (txt.Attributes + txt.Text);
            } else {
                return await CompileTextTika(reader);
            }
        }
        private static FileFormat GetFormat(FileInfo doc) {
            if (doc == null) {
                throw new ArgumentNullException(nameof(doc));
            }
            return FormatExt.Parse(doc.Extension);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return Collections.ToString(tokens);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is FSDocument) {
                return ((FSDocument)obj).docId == docId;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            return docId.GetHashCode();
        }
        #endregion
    }
}
