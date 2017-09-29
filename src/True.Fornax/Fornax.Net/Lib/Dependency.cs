using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Fornax.Net.Util.System;
using DLL = Fornax.Net.Util.IO.FornaxAssembly;

namespace Fornax.Net.Lib
{
    [Progress("Dependency", false, Documented = false, Tested = false)]
    internal class Dependency
    {
        private Dependency() { }

        #region IKVM dependencies


        private const string IKVM_WINFORMS = "Fornax.Net.Lib.ikvm.IKVM.AWT.WinForms.dll";
        private const string IKVM_BEANS = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Beans.dll";
        private const string IKVM_CHARSETS = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Charsets.dll";
        private const string IKVM_CLDRDATA = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Cldrdata.dll";
        private const string IKVM_CORBA = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Corba.dll";
        private const string IKVM_CORE = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Core.dll";
        private const string IKVM_JDBC = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Jdbc.dll";
        private const string IKVM_LOCALEDATA = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Localedata.dll";
        private const string IKVM_MANAGEMENT = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Management.dll";
        private const string IKVM_MEDIA = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Media.dll";
        private const string IKVM_NAMING = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Naming.dll";
        private const string IKVM_MISC = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Misc.dll";
        private const string IKVM_NASHORN = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Nashorn.dll";
        private const string IKVM_REMOTING = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Remoting.dll";
        private const string IKVM_SECURITY = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Security.dll";
        private const string IKVM_SWING_AWT = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.SwingAWT.dll";
        private const string IKVM_TEXT = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Text.dll";
        private const string IKVM_TOOLS = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Tools.dll";
        private const string IKVM_UTIL = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.Util.dll";
        private const string IKVM_RUNTIME = "Fornax.Net.Lib.ikvm.IKVM.Runtime.dll";
        private const string IKVM_RUNTIME_JNI = "Fornax.Net.Lib.ikvm.IKVM.Runtime.JNI.dll";
        private const string IKVM_XML_API = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.XML.API.dll";
        private const string IKVM_XML_BIND = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.XML.Bind.dll";
        private const string IKVM_XML_CRYPTO = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.XML.Crypto.dll";
        private const string IKVM_XML_PARSE = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.XML.Parse.dll";
        private const string IKVM_XML_TRANSFORM = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.XML.Transform.dll";
        private const string IKVM_XML_WEBSERVICES = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.XML.WebServices.dll";
        private const string IKVM_XML_XPATH = "Fornax.Net.Lib.ikvm.IKVM.OpenJDK.XML.XPath.dll";

        internal static string[] Get_IKVM_AssemblyNames() {
            return new[] { IKVM_BEANS,IKVM_CHARSETS,IKVM_CLDRDATA,IKVM_CORBA,IKVM_CORE,
               IKVM_JDBC,IKVM_LOCALEDATA,IKVM_MANAGEMENT,IKVM_MEDIA,IKVM_MISC,IKVM_NAMING,
               IKVM_NASHORN,IKVM_REMOTING,IKVM_RUNTIME,IKVM_RUNTIME_JNI,IKVM_SECURITY,IKVM_SWING_AWT,
               IKVM_TEXT,IKVM_TOOLS,IKVM_UTIL,IKVM_WINFORMS,IKVM_XML_API,IKVM_XML_BIND,IKVM_XML_CRYPTO,
               IKVM_XML_PARSE,IKVM_XML_TRANSFORM,IKVM_XML_WEBSERVICES,IKVM_XML_XPATH
           };
        }

        internal static IEnumerable<AssemblyName> Get_IKVM_Names() {
            return DLL.GetNames(Get_IKVM_AssemblyNames());
        }

        internal static IEnumerable<Assembly> GetAllIKVM() {
            foreach (var dll in Get_IKVM_Names()) {
                yield return DLL.LoadAssembly(dll);
            }
        }
        #endregion

        #region Tika
        private const string TIKA = "Fornax.Net.Lib.tika.TikaOnDotNet.dll";
        private const string TIKA_TEXTEXTRACTION = "Fornax.Net.Lib.tika.TikaOnDotNet.TextExtraction.dll";
        private const string MICROSOFT_WIN32 = "Fornax.Net.Lib.Microsoft.Win32.Primitives.dll";

        internal static string[] Get_Tika_Names() => new[] { TIKA, TIKA_TEXTEXTRACTION, MICROSOFT_WIN32 };

        internal static IEnumerable<AssemblyName> GetTikaNames() {
            return DLL.GetNames(Get_Tika_Names());
        }


        internal static IEnumerable<Assembly> GetAllTika() {
            foreach (var dll in GetTikaNames()) {
                yield return DLL.LoadAssembly(dll);
            }
        }

        #endregion

        #region Toxy
        private const string DCSOFT_RTF = "Fornax.Net.Lib.toxy.DCSoft.RTF.dll";
        private const string DMACH_MAIL = "Fornax.Net.Lib.toxy.dmach.Mail.dll";
        private const string DOCUMENTFORMAT_OPENXML = "Fornax.Net.Lib.toxy.DocumentFormat.OpenXml.dll";
        private const string ITEXTSHARP = "Fornax.Net.Lib.toxy.itextsharp.dll";
        private const string LUMENWORKS_FRAMEWORK_IO = "Fornax.Net.Lib.toxy.LumenWorks.Framework.IO.dll";
        private const string MSG_READER = "Fornax.Net.Lib.toxy.MsgReader.dll";

        private const string NPOI = "Fornax.Net.Lib.toxy.NPOI.dll";
        private const string NPOI_OOXML = "Fornax.Net.Lib.toxy.NPOI.OOXML.dll";
        private const string NPOI_OPENXML4NET = "Fornax.Net.Lib.toxy.NPOI.OpenXml4Net.dll";
        private const string NPOI_OPENXMLFORMATS = "Fornax.Net.Lib.toxy.NPOI.OpenXmlFormats.dll";
        private const string NPOI_SCRATCHPAD_HWPF = "Fornax.Net.Lib.toxy.NPOI.ScratchPad.HWPF.dll";

        private const string POLICY_TAGLIB_SHARP = "Fornax.Net.Lib.toxy.policy.2.0.taglib-sharp.dll";
        private const string TAGLIB_SHARP = "Fornax.Net.Lib.toxy.taglib-sharp.dll";
        private const string THOUGHT_VCARDS = "Fornax.Net.Lib.toxy.Thought.vCards.dll";
        internal const string HTML_AGILITY_PACK = "Fornax.Net.Lib.toxy.HtmlAgilityPack.dll";

        private const string TOXY = "Fornax.Net.Lib.toxy.Toxy.dll";


        internal static string[] Get_Toxy_Names() {
            return new[] { DCSOFT_RTF, DMACH_MAIL,DOCUMENTFORMAT_OPENXML,ITEXTSHARP,LUMENWORKS_FRAMEWORK_IO,MSG_READER
                        ,NPOI,NPOI_OOXML,NPOI_OPENXML4NET,NPOI_OPENXMLFORMATS,NPOI_SCRATCHPAD_HWPF,POLICY_TAGLIB_SHARP,TAGLIB_SHARP,
                        THOUGHT_VCARDS,HTML_AGILITY_PACK,TOXY };
        }

        internal static IEnumerable<AssemblyName> GetToxyNames() {
            return DLL.GetNames(Get_Toxy_Names());
        }


        internal static IEnumerable<Assembly> GetAllToxy() {
            foreach (var dll in GetToxyNames()) {
                yield return DLL.LoadAssembly(dll);
            }
        }

        #endregion

        #region Zero Formatter
        private const string ZERO_FORMATTER = "Fornax.Net.Lib.zero.ZeroFormatter.dll";
        private const string ZERO_FORMATTER_INTERFACES = "Fornax.Net.Lib.zero.ZeroFormatter.Interfaces.dll";
        private const string ZERO_FORMATTER_ANALYZER = "Fornax.Net.Lib.zero.ZeroFormatterAnalyzer.dll";


        internal static string[] Get_Zero_Names() => new[] { ZERO_FORMATTER, ZERO_FORMATTER_ANALYZER, ZERO_FORMATTER_INTERFACES };

        #endregion

        #region Misc
        internal const string PDFBOX = "Fornax.Net.Lib.pdfbox.pdfbox-app-2.0.7.dll";
        internal const string SHARPZIPLIB = "Fornax.Net.Lib.ICSharpCode.SharpZipLib.dll";
        internal const string PROTOBUF = "Fornax.Net.Lib.protobuf-net.dll";
        internal const string LZ4NET = "Fornax.Net.Lib.Lz4Net.dll";
        internal const string LZ4 = "Fornax.Net.Lib.LZ4.dll";

        internal static string[] Get_MISC_Names() => new[] { PDFBOX, SHARPZIPLIB, PROTOBUF, LZ4NET, LZ4 };


        internal static IEnumerable<AssemblyName> GetMiscNames() {
            return DLL.GetNames(Get_MISC_Names());
        }

        #endregion


        static string[] all_deps = new[] { IKVM_BEANS,IKVM_CHARSETS,IKVM_CLDRDATA,IKVM_CORBA,IKVM_CORE,
               IKVM_JDBC,IKVM_LOCALEDATA,IKVM_MANAGEMENT,IKVM_MEDIA,IKVM_MISC,IKVM_NAMING,
               IKVM_NASHORN,IKVM_REMOTING,IKVM_RUNTIME,IKVM_RUNTIME_JNI,IKVM_SECURITY,IKVM_SWING_AWT,
               IKVM_TEXT,IKVM_TOOLS,IKVM_UTIL,IKVM_WINFORMS,IKVM_XML_API,IKVM_XML_BIND,IKVM_XML_CRYPTO,
               IKVM_XML_PARSE,IKVM_XML_TRANSFORM,IKVM_XML_WEBSERVICES,IKVM_XML_XPATH, TIKA, TIKA_TEXTEXTRACTION, MICROSOFT_WIN32,
               DCSOFT_RTF, DMACH_MAIL,DOCUMENTFORMAT_OPENXML,ITEXTSHARP,LUMENWORKS_FRAMEWORK_IO,MSG_READER,
               NPOI,NPOI_OOXML,NPOI_OPENXML4NET,NPOI_OPENXMLFORMATS,NPOI_SCRATCHPAD_HWPF,POLICY_TAGLIB_SHARP,TAGLIB_SHARP,
               THOUGHT_VCARDS,HTML_AGILITY_PACK,TOXY,ZERO_FORMATTER, ZERO_FORMATTER_ANALYZER, ZERO_FORMATTER_INTERFACES,
               PDFBOX, SHARPZIPLIB, PROTOBUF, LZ4NET,LZ4
       };

        public static string[] All_deps { get => all_deps; set => all_deps = value; }

        static string[] temp = new string[all_deps.Length];

        static int i = 0;
        internal static bool ResolveAll(string[] all) {
            i = 0;
            try {
                for (i = 0; i < all.Length; i++) {
                    temp[i] = all[i];
                    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
                }
                return true;
            } catch (Exception) { return false; }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            return DLL.LoadAssembly(temp[i]);
        }


        internal static bool IsIKVMLoaded { get; set; }
        internal static bool IsToxyLoaded { get; set; }
        internal static bool IsTikaLoaded { get; set; }
        internal static bool IsZeroLoaded { get; set; }
        internal static bool IsLZ4Loaded { get; set; }
        internal static bool IsHtmlPackLoaded { get; set; }
        internal static bool IsProtoLoaded { get; set; }
    }
}
