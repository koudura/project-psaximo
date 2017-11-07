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
using Fornax.Net.Util.IO;
using Fornax.Net.Util.Text;

namespace Fornax.Net
{

    /// <summary>
    /// File Formats Supported by Fornax.Net .
    /// </summary>
    [Serializable]
    public enum FileFormat
    {
        /// <summary>
        /// Dynamic Html document/file.
        /// </summary>
        DHtml,
        /// <summary>
        /// The FXML document/file.
        /// </summary>
        Fxml,
        /// <summary>
        /// The HTML documents/file.
        /// </summary>
        Html,
        /// <summary>
        /// The server parsed HTML document/file.
        /// </summary>
        SHtml,
        /// <summary>
        /// The xaml document/file.
        /// </summary>
        Xaml,
        /// <summary>
        /// The static HTML document/file.
        /// </summary>
        XHtml,
        /// <summary>
        /// The XML document/file.
        /// </summary>
        Xml,
        /// <summary>
        /// The CSV (comma separated values) document/file.
        /// </summary>
        Csv,
        /// <summary>
        /// The microsoft word-document/file (97-2003).
        /// </summary>
        Doc,
        /// <summary>
        /// The microsoft word-document/file (2003-2017).
        /// </summary>
        Docx,
        /// <summary>
        /// The epub comic book/document.
        /// </summary>
        Epub,
        /// <summary>
        /// The PDF (Portable document format) file.
        /// </summary>
        Pdf,
        /// <summary>
        /// The RTF (Rich text format) file.
        /// </summary>
        Rtf,
        /// <summary>
        /// The eml (electronic mail format) file.
        /// </summary>
        Eml,
        /// <summary>
        /// Microsoft Outlook <c>MSG</c> (Mail message format) file.
        /// </summary>
        Msg,
        /// <summary>
        /// The PST
        /// </summary>
        Pst,
        /// <summary>
        /// <c>VCF</c> (Variant call format) email/contact file.
        /// </summary>
        Vcf,
        /// <summary>
        /// <c>ANS</c> encoded plain-text file.
        /// </summary>
        Ans,
        /// <summary>
        /// <c>ASCII</c> encoded plain-text file.
        /// </summary>
        Ascii,
        /// <summary>
        /// c program source code file.
        /// </summary>
        C,
        /// <summary>
        /// c-sharp source code file.
        /// </summary>
        Cs,
        /// <summary>
        /// The java source code file.
        /// </summary>
        Java,
        /// <summary>
        /// The plain text file
        /// </summary>
        Txt,
        /// <summary>
        /// The gzip (gnu zip) compressed file/folder.
        /// </summary>
        Gzip,
        /// <summary>
        /// The rar (win-rar) compressed folder.
        /// </summary>
        Rar,
        /// <summary>
        /// The zip compressed folder.
        /// </summary>
        Zip,
        /// <summary>
        /// The ASP (raw active server pages)
        /// </summary>
        Asp,
        /// <summary>
        ///  microsoft aspx web server page.
        /// </summary>
        Aspx,
        /// <summary>
        ///  MP3 media file.
        /// </summary>
        Mp3,
        /// <summary>
        ///  MP4 media file.
        /// </summary>
        Mp4,
        /// <summary>
        ///  JPEG image file.
        /// </summary>
        Jpeg,
        /// <summary>
        ///  JPG image file.
        /// </summary>
        Jpg,
        /// <summary>
        /// PNG (portable network graphics) file.
        /// </summary>
        Png,
        /// <summary>
        ///  tiff web image format file. 
        /// </summary>
        Tiff,
        /// <summary>
        /// The xl document/file.
        /// </summary>
        Xl,
        /// <summary>
        /// The XLR document/file.
        /// </summary>
        Xlr,
        /// <summary>
        /// The XLS document/file.
        /// </summary>
        Xls,
        /// <summary>
        /// The XLSM document/file.
        /// </summary>
        Xlsm,
        /// <summary>
        /// The XLSX (Microsoft excel spread-shheet (2007+)) document/file.
        /// </summary>
        Xlsx,
        /// <summary>
        /// The PPT (Microsoft power-point slides (97-2003)) document/file.
        /// </summary>
        Ppt,
        /// <summary>
        /// The PPTX (Microsoft power-point slides (2007+)) document/file. 
        /// </summary>
        Pptx,
        /// <summary>
        /// Microsoft publisher document/file.
        /// </summary>
        Pub
    }

    /// <summary>
    /// Format extensions.
    /// </summary>
    public static class FormatExt
    {

        /// <summary>
        /// gets the <see cref="string" /> that represents the file extension of the 
        /// specified format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public static string GetString(this FileFormat format) {
            return format.ToString().Trim().ToLower().Insert(0, ".");
        }

        /// <summary>
        /// Gets the fornax format category <see cref="FornaxFormat"/>.
        /// </summary>
        /// <param name="format">The file format type.</param>
        /// <returns><see cref="FornaxFormat"/> that  <paramref name="format"/> belongs to.</returns>
        public static FornaxFormat GetFornaxFormat(this FileFormat format) {
            foreach (var item in ConfigFactory.FornaxFormatTable) {
                if (item.Value.Contains(format.GetString()))
                    return item.Key;
            }
            return FornaxFormat.Default;
        }

        /// <summary>
        /// Gets the <see cref="FileFormat"/> array of all the extension format in the 
        /// (<see cref="FornaxFormat"/>) <paramref name="fornaxFormat"/> category.
        /// </summary>
        /// <param name="fornaxFormat">The fornax format.</param>
        /// <returns>an array of all formats in <paramref name="fornaxFormat"/> category.</returns>
        public static FileFormat[] GetFormats(this FornaxFormat fornaxFormat) {
            var strs = ConfigFactory.FornaxFormatTable[fornaxFormat];
            FileFormat[] formats = new FileFormat[strs.Count];
            int i = 0;
            foreach (var item in strs) {
                formats[i] = Parse(item);
                i++;
            }
            return formats;
        }

        /// <summary>
        /// Parses the specified string format extension to recognizable fornax format. 
        /// </summary>
        /// <param name="form">The form.</param>
        /// <returns></returns>
        /// <exception cref="InvalidCastException"></exception>
        /// <exception cref="FornaxFormatException"></exception>
        public static FileFormat Parse(string form) {
            form = form.Trim().Remove(0, 1);
            if (Char.TryParse((form[0] + "").ToUpper(), out char res)) {
                form = form.ReplaceAt(0, res);
                return (FileFormat)Enum.Parse(typeof(FileFormat), form);
            }
            throw new InvalidCastException(); throw new FornaxFormatException();
        }
    }
}
