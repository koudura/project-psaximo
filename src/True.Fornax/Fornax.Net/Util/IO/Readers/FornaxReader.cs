using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

using Fornax.Net.Util.Linq;
using TikaOnDotNet.TextExtraction;
using Toxy;

namespace Fornax.Net.Util.IO.Readers
{

    /// <summary>
    /// FornaxReader for Extraction of Text-Content from files.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public class FornaxReader : IReader
    {
        private FileInfo file;
        private FileFormat format;
        private FornaxFormat category;
        private static bool isSafeLoad;

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader" /> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <exception cref="ArgumentException">message - filename</exception>
        public FornaxReader(string filename)
            : this(new FileInfo(filename ?? throw new ArgumentNullException(nameof(filename)))) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public FornaxReader(FileInfo file) {
            this.file = file ?? throw new ArgumentNullException(nameof(file));
            this.format = FormatExt.Parse(file.Extension);
            this.category = format.GetFornaxFormat();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxReader" /> class.
        /// </summary>
        /// <param name="fileWrapper">The file wrapper.</param>
        /// <exception cref="FornaxFileException"></exception>
        public FornaxReader(FileWrapper fileWrapper)
            : this(fileWrapper.Parse().AsFile ?? throw new FornaxFileException()) {
        }


        #region Toxy
        /// <summary>
        /// Uses Toxy Dom Reader to extract content from DOM file.
        /// i.e. @ <see cref="FornaxFormat.Dom" />.
        /// </summary>
        /// <returns>
        /// content details of the Dom-object file.
        /// </returns>
        /// <exception cref="FornaxFormatException">category</exception>
        public (string Name, string Text, string InnerText, string NodeString, Dictionary<string, string> Attributes) ToxyDomRead() {
            if (this.category != FornaxFormat.Dom)
                throw new FornaxFormatException($"{nameof(category)} is not a Dom file.");
            IDomParser parser = ParserFactory.CreateDom(new ParserContext(this.file.FullName));
            var dom = parser.Parse().Root; var attribs = dom.Attributes.ToDictionary(Name, Value);
            return (dom.Name, dom.Text, dom.InnerText, dom.NodeString, attribs);
        }

        /// <summary>
        /// Uses Toxy Slide Reader to extract content from Slide file.
        /// i.e. @ <see cref="FornaxFormat.Slide" />
        /// </summary>
        /// <returns>
        /// content details of the slide-show file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public (string Note, string Text) ToxySlideRead() {
            if (this.category != FornaxFormat.Slide)
                throw new FornaxFormatException($"{nameof(this.file)} is not a slideshow file.");
            var text = new StringBuilder(); var note = new StringBuilder();
            var content = ParserFactory.CreateSlideshow(new ParserContext(this.file.FullName)).Parse();
            var slides = content.Slides;
            foreach (var item in slides) {
                note.AppendLine(item.Note);
                text.AppendLine(item.Texts.Concat());
            }
            return (note.ToString(), text.ToString());
        }

        /// <summary>
        /// Uses Toxy Text Reader to extract content from text and plain files.
        /// i.e. @ <see cref="FornaxFormat.Plain" /> and <see cref="FornaxFormat.Text" />
        /// </summary>
        /// <returns>
        /// content details of the plain-text file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public string ToxyTextRead() {
            if (this.category != FornaxFormat.Text || this.category != FornaxFormat.Plain)
                throw new FornaxFormatException($"{nameof(this.file)} is neither a plain-text nor a raw-text file.");
            var content = ParserFactory.CreateText(new ParserContext(this.file.FullName)).Parse();
            return content;
        }

        /// <summary>
        /// Uses Toxy Email Reader to extract content from email files.
        /// i.e. <see cref="FornaxFormat.Email"/>.
        /// </summary>
        /// <returns>content details of the email file</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public (string Attributes, string Text) ToxyEmailRead() {
            if (this.category != FornaxFormat.Email)
                throw new FornaxFormatException($"{nameof(this.file)} is not an Email file");
            if (this.format == FileFormat.Vcf) {
                return ToxyVCardRead();
            }
            var details = new StringBuilder();
            var content = ParserFactory.CreateEmail(new ParserContext(this.file.FullName)).Parse();
            details.AppendLine(content.Attachments.Concat()).AppendLine(content.Bcc.Concat()).AppendLine(content.Cc.Concat());
            details.AppendLine(content.From).AppendLine(content.To.Concat()).AppendLine(content.Subject);
            return (details.ToString(), content.TextBody);
        }


        /// <summary>
        /// Uses Toxy V-card reader to extract content from vcf files. 
        /// </summary>
        /// <returns>content details of the vcf file.</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public (string Attributes, string Text) ToxyVCardRead() {
            if (this.format != FileFormat.Vcf)
                throw new FornaxFormatException($"{nameof(file)} is not a Vcard file.");
            var details = new StringBuilder(); var texts = new StringBuilder();
            var card = ParserFactory.CreateVCard(new ParserContext(file.FullName)).Parse();
            foreach (var item in card.Cards) {
                details.AppendLine(item.Title);
                details.AppendFormat("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n",
                    ((item.Name == null) ? "" : item.Name.FullName),
                    ((item.NickName == null) ? "" : item.NickName.FullName),
                    ((item.Birthday == null) ? "" : item.Birthday.ToLongDateString()),
                    item.Gender.ToString(),
                    item.GEO,
                    item.Orgnization,
                    item.ProductID);
                details.AppendFormat("{0}\n{1}\n{2}\n{3}", item.TimeZone, item.Label, item.Class, item.UID);
                texts.AppendLine(item.Categories.Concat()).AppendLine(item.Photos.Concat()).AppendLine(item.Sources.Concat());
                foreach (var add in item.Addresses) {
                    texts.AppendFormat("{0}\n{1}\n{2}\n{3}\n{4}\n", add.City, add.Country, add.PostalCode, add.Region, add.Street);
                }
                foreach (var cnt in item.Contacts) {
                    texts.AppendFormat("{0}\n{1}\n{2}", cnt.Name, cnt.Note, cnt.Value);
                }
            }
            return (details.ToString(), texts.ToString());
        }
        #endregion

        #region misc 
        /// <summary>
        /// Reads an Xml file.
        /// </summary>
        /// <returns>Text content of the specified xml</returns>
        public string XmlRead() {
            if (format != FileFormat.Xml)
                throw new FornaxFormatException();
            StringBuilder builder = new StringBuilder();
            using (var stream = new FileStream(this.file.FullName, FileMode.Open, FileAccess.Read)) {
                using (XmlTextReader reader = new XmlTextReader(stream)) {
                    while (reader.Read()) {
                        switch (reader.NodeType) {
                            case XmlNodeType.Text:
                                builder.AppendLine(reader.ReadString());
                                break;
                        }
                    }
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// Implementation of the BufferedReader .net, may be used to extract types of
        /// <see cref="FornaxFormat.Dom"/> , <see cref="FornaxFormat.Plain"/>, <see cref="FornaxFormat.Web"/>.
        /// NOTE: Only the Raw-Text content is returned.
        /// </summary>
        /// <returns>String Text content of the specified file.</returns>
        public string BufferRead() {
            var bufferedReader = new BufferedReaderWrapper(this.file);
            return bufferedReader.Text;
        }

        /// <summary>
        /// Use Tika Extractor to Read file.
        /// </summary>
        /// <exception cref="FornaxException">category</exception>
        public (string ContentType, string Text, IDictionary<string, string> Metadata) TikaRead() {
            var res = new TextExtractor().Extract(this.file.FullName);
            return (res.ContentType, res.Text, res.Metadata);
        }

        private string Name(ToxyAttribute attribute) => attribute.Name;

        private string Value(ToxyAttribute attribute) => attribute.Value;
        #endregion

        #region async      


        #region toxy-async

        /// <summary>
        /// Uses Toxy Dom Reader to extract content from DOM file asynchronously.
        /// i.e. @ <see cref="FornaxFormat.Dom" />.
        /// </summary>
        /// <returns>
        /// content details of the Dom-object file.
        /// </returns>
        /// <exception cref="FornaxFormatException">category</exception>
        public async Task<(string Name, string Text, string InnerText, string NodeString, Dictionary<string, string> Attributes)> ToxyDomReadAsync() {
            return await Task.Factory.StartNew(ToxyDomRead);
        }

        /// <summary>
        /// Uses Toxy Slide Reader to extract content from Slide file asynchronously.
        /// i.e. @ <see cref="FornaxFormat.Slide" />
        /// </summary>
        /// <returns>
        /// content details of the slide-show file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public async Task<(string Note, string Text)> ToxySlideReadAsync() {
            return await Task.Factory.StartNew(ToxySlideRead);
        }

        /// <summary>
        /// Uses Toxy Text Reader to extract content from text and plain files asynchronously.
        /// i.e. @ <see cref="FornaxFormat.Plain" /> and <see cref="FornaxFormat.Text" />
        /// </summary>
        /// <returns>
        /// content details of the plain-text file.
        /// </returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public async Task<string> ToxyTextReadAsync() {
            return await Task.Factory.StartNew(ToxyTextRead);
        }

        /// <summary>
        /// Uses Toxy Email Reader to extract content from email files asynchronously.
        /// i.e. <see cref="FornaxFormat.Email"/>.
        /// </summary>
        /// <returns>content details of the email file</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public async Task<(string Attributes, string Text)> ToxyEmailReadAsync() {
            return await Task.Factory.StartNew(ToxyEmailRead);
        }

        /// <summary>
        /// Uses Toxy V-card reader to extract content from vcf files asynchronously.
        /// </summary>
        /// <returns>content details of the vcf file.</returns>
        /// <exception cref="FornaxFormatException">file</exception>
        public async Task<(string Attributes, string Text)> ToxyVCardReadAsync() {
            return await Task.Factory.StartNew(ToxyVCardRead);
        }
        #endregion


        /// <summary>
        /// Reads an Xml file asynchronously.
        /// </summary>
        /// <returns>Text content of the specified xml</returns>
        public async Task<string> XmlReadAsync() {
            if (format != FileFormat.Xml)
                throw new FornaxFormatException();

            //StringBuilder builder = new StringBuilder();
            //using (var stream = new FileStream(this.file.FullName, FileMode.Open, FileAccess.Read)) {
            //    using (XmlTextReader reader = new XmlTextReader(stream)) {
            //        var r = reader.ReadAsync();
            //        while (r.Result) {
            //            switch (reader.NodeType) {
            //                case XmlNodeType.Text:
            //                    builder.AppendLine(await reader.GetValueAsync());
            //                    break;
            //            }
            //        }
            //    }
            //}
            //return builder.ToString();
            return await Task.Factory.StartNew(XmlRead);
        }

        /// <summary>
        /// Use Tika Extractor to Read file asynchronously.
        /// </summary>
        public async Task<(string ContentType, string Text, IDictionary<string, string> Metadata)> TikaReadAsync() {
            return await Task.Factory.StartNew(TikaRead);
        }

        /// <summary>
        /// Asynchronized implementation of the BufferedReader .net, may be used to extract types of
        /// <see cref="FornaxFormat.Dom"/> , <see cref="FornaxFormat.Plain"/>, <see cref="FornaxFormat.Web"/>.
        /// NOTE: Only the Raw-Text content is returned.
        /// </summary>
        /// <returns>String Text content of the specified file.</returns>
        public async Task<string> BufferReadAsync() {
            return await Task.Factory.StartNew(BufferRead);
        }

        #endregion

        static FornaxReader() {
            if (FornaxAssembly.TryResolveTika() && FornaxAssembly.TryResolveToxy()) {
                isSafeLoad = true;
            }
        }

    }
}