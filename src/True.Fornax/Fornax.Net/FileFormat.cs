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

namespace Fornax.Net
{

    /// <summary>
    /// File Formats Supported by Fornax.Net .
    /// </summary>
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
        /// The eml (electronic mail file)
        /// </summary>
        Eml,
        /// <summary>
        /// The MSG
        /// </summary>
        Msg,
        /// <summary>
        /// The PST
        /// </summary>
        Pst,
        /// <summary>
        /// The VCF
        /// </summary>
        Vcf,
        /// <summary>
        /// The ans
        /// </summary>
        Ans,
        /// <summary>
        /// The ASCII
        /// </summary>
        Ascii,
        /// <summary>
        /// The c
        /// </summary>
        C,
        /// <summary>
        /// The c-sharp source code file.
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
        /// The aspx
        /// </summary>
        Aspx,
        /// <summary>
        /// The MP3
        /// </summary>
        Mp3,
        /// <summary>
        /// The MP4
        /// </summary>
        Mp4,
        /// <summary>
        /// The JPEG
        /// </summary>
        Jpeg,
        /// <summary>
        /// The JPG
        /// </summary>
        Jpg,
        /// <summary>
        /// The PNG (portable network graphics) file.
        /// </summary>
        Png,
        /// <summary>
        /// The tiff 
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
        /// The XLSX (microsoft excel spread-shheet (2007+)) document/file.
        /// </summary>
        Xlsx,
        /// <summary>
        /// The PPT (microsoft power-point slides (97-2003)) document/file.
        /// </summary>
        Ppt,
        /// <summary>
        /// The PPTX (microsoft power-point slides (2007+)) document/file. 
        /// </summary>
        Pptx,
        /// <summary>
        /// The microsoft publisher document/file.
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
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        internal static string GetString(this FileFormat format) {
            return format.ToString().Trim().ToLower().Insert(0, ".");
        }



    }
}
