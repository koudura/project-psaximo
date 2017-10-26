
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Fornax.Net.Util.Collections;
using Fornax.Net.Util.IO;
using Fornax.Net.Util.IO.Readers;
using HtmlAgilityPack;

namespace Fornax.Net.Document
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Fornax.Net.Document.IDocument" />
    [Serializable]
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
            file = htmlfile ?? throw new ArgumentNullException(nameof(htmlfile));
            if (htmlfile.Exists) {
                document.Load(htmlfile.FullName);
            }else {
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
            Contract.Requires(link.IsWellFormedOriginalString());
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
        public async Task<(string Text,string Metadata)> GetContent() {
            IReader reader = new FornaxReader(file);
            var read = await reader.TikaReadAsync();
            return (read.Text, Collections.ToString(read.Metadata));
        }

    }
}
