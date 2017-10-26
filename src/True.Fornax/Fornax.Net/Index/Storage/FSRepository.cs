using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Util.IO;
using FSDocument = Fornax.Net.Document.FSDocument;
using Cst = Fornax.Net.Util.Constants;
using Fornax.Net.Util.IO.Readers;

namespace Fornax.Net.Index.Storage
{
    [Serializable]
    public sealed class FSRepository : Repository {

        [NonSerialized]
        private IEnumerable<FileInfo> files;
        private static string dir = DateTime.Now.ToFileTime().ToString() + "[repo].4dat";
        private static FileInfo repofile = new FileInfo(Cst.BaseDirectory.FullName + dir);

        private Configuration config;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="files"></param>
        /// <param name="formats"></param>
        /// <param name="config"></param>
        public FSRepository(FileInfo[] files, FileFormat[] formats, Configuration config) {
            this.files = GetFiles(files, formats);
            this.config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filewrappers"></param>
        /// <param name="formats"></param>
        /// <param name="config"></param>
        public FSRepository(FileWrapper[] filewrappers, FileFormat[] formats, Configuration config) {
            files = GetFiles(filewrappers, formats);
            this.config = config;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileNames"></param>
        /// <param name="formats"></param>
        /// <param name="config"></param>
        public FSRepository(string[] fileNames, FileFormat[] formats, Configuration config) {
            files = GetFiles(fileNames, formats);
            this.config = config;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <param name="formats"></param>
        /// <param name="config"></param>
        public FSRepository(DirectoryInfo directory, FileFormat[] formats, Configuration config) {
            files = GetFiles(directory, formats);
            this.config = config;
        }




        private static IEnumerable<FileInfo> GetFiles(DirectoryInfo directory, FileFormat[] formats) {
            var files = from ext in formats
                        let sext = ext.GetString()
                        from t in directory.EnumerateFiles(("*" + sext), SearchOption.AllDirectories)
                        select t;
            foreach (var file in files) yield return file;

        }

        private IEnumerable<FileInfo> GetFiles(FileInfo[] files, FileFormat[] formats) {
            var fls = from ext in formats
                      let sext = ext.GetString()
                      from file in files
                      where file.Extension == sext
                      select file;
            foreach (var file in fls) yield return file;
        }


        private IEnumerable<FileInfo> GetFiles(FileWrapper[] filewrappers, FileFormat[] formats) {

            IList<FileInfo> file_s = new List<FileInfo>();
            foreach (var wrapper in filewrappers) {
                var pass = wrapper.Parse();
                var file = pass.AsFile; var dir = pass.AsDirectory;
                if (file == null) {
                    foreach (var item in formats) {
                        var fs = item.GetString();
                        foreach (var f in dir.EnumerateFiles(("*" + fs), SearchOption.AllDirectories)) {
                            file_s.Add(f);
                        }
                    }
                } else if (dir == null) {
                    foreach (var item in formats) {
                        var fs = item.GetString();
                        if (file.Extension == fs)
                            file_s.Add(file);
                    }
                }
            }
            return file_s;
        }

        private IEnumerable<FileInfo> GetFiles(string[] fileNames, FileFormat[] formats) {
            IList<FileInfo> files = new List<FileInfo>();
            foreach (var filename in fileNames) {
                var file = new FileInfo(filename);
                if (file.Exists) files.Add(file);
            }
            return GetFiles(files.ToArray(), formats);
        }

        //public IEnumerable<FSDocument> GetDocuments(FornaxReader reader, Extractor extractorOption) {

        //}
    }
}
