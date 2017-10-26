﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fornax.Net.Util.IO;

namespace Fornax.Net.Index.Storage
{
    public class NETRepository : Repository
    {
        private FileInfo[] files;
        private FileFormat[] formats;
        private Configuration config;
        private FileWrapper[] files1;
        private string[] files2;
        private DirectoryInfo directory;

        public NETRepository(FileInfo[] files, FileFormat[] formats, Configuration config) {
            this.files = files;
            this.formats = formats;
            this.config = config;
        }

        public NETRepository(FileWrapper[] files1, FileFormat[] formats, Configuration config) {
            this.files1 = files1;
            this.formats = formats;
            this.config = config;
        }

        public NETRepository(string[] files2, FileFormat[] formats, Configuration config) {
            this.files2 = files2;
            this.formats = formats;
            this.config = config;
        }

        public NETRepository(DirectoryInfo directory, FileFormat[] formats, Configuration config) {
            this.directory = directory;
            this.formats = formats;
            this.config = config;
        }
    }
}
