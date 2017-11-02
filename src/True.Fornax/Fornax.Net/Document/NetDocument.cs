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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Fornax.Net.Analysis.Tokenization;
using Fornax.Net.Index;
using Fornax.Net.Util.Collections;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Readers;
using HtmlAgilityPack;
using ProtoBuf;

namespace Fornax.Net.Document
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Fornax.Net.Document.IDocument" />
    [Serializable, ProtoContract]
    public sealed class NetDocument : IDocument
    {
        HtmlDocument document;
        HtmlWeb web;
        FileInfo file;
        private string link;
        private IEnumerable<Uri> links;

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDocument"/> class.
        /// </summary>
        /// <param name="htmlfile">The htmlfile.</param>
        /// <exception cref="ArgumentNullException">htmlfile</exception>
        public NetDocument(FileInfo htmlfile) {
            Contract.Requires(htmlfile != null);

            file = htmlfile ?? throw new ArgumentNullException(nameof(htmlfile));
            if (htmlfile.Exists) {
                document.Load(htmlfile.FullName);
            } else {
                web = new HtmlWeb();
                document = web.Load(htmlfile.FullName);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDocument"/> class.
        /// </summary>
        /// <param name="htmlFile">The HTML file.</param>
        public NetDocument(FileWrapper htmlFile)
            : this(htmlFile.Parse().AsFile ?? throw new ArgumentNullException(nameof(htmlFile))) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetDocument"/> class.
        /// </summary>
        /// <param name="link">The link.</param>
        /// <exception cref="ArgumentNullException">link</exception>
        public NetDocument(Uri link) {
            Contract.Requires(link != null && link.IsWellFormedOriginalString());
            if (link == null || !link.IsWellFormedOriginalString()) throw new ArgumentNullException(nameof(link));

            new NetDocument(new FileInfo(link.AbsolutePath));
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public IEnumerable<Uri> Links => links = GetLinks();

        public Snippet Capture => throw new NotImplementedException();

        public FileFormat Format => throw new NotImplementedException();

        public ulong ID => throw new NotImplementedException();

        public string Link => throw new NotImplementedException();

        public string Name => throw new NotImplementedException();

        public TokenStream Tokens => throw new NotImplementedException();

        public TermVector Terms => throw new NotImplementedException();

        private IEnumerable<Uri> GetLinks() {
            var links = from lnk in document.DocumentNode.Descendants()
                        where lnk.Name == "a" && lnk.Attributes["href"] != null
                        where lnk.InnerText.Trim().Length > 0
                        select lnk.Attributes["href"].Value;
            foreach (var item in links) {
                if (Uri.IsWellFormedUriString(item, UriKind.RelativeOrAbsolute)) { yield return new Uri(item); }
            }
        }

        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <returns></returns>
        public async Task<(string Text, string Metadata)> GetContent() {
            IReader reader = new FornaxReader(file);
            var read = await reader.TikaReadAsync();
            return (read.Text, Collections.ToString(read.Metadata));
        }

    }
}
