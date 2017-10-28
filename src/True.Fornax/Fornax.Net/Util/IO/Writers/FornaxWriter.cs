using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using io = java.io;
using ProtoBuf;

namespace Fornax.Net.Util.IO.Writers
{
    /// <summary>
    /// 
    /// </summary>
    public static class FornaxWriter
    {
        static TextWriter err = Console.Error;

        /// <summary>
        /// Reads an object representation from the specified file.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">fileName</exception>
        public static TObject Read<TObject>(string fileName) where TObject : class {
            Contract.Requires(fileName != null);
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            return Read<TObject>(new FileInfo(fileName));
        }

        /// <summary>
        /// Reads the specified file.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">file</exception>
        public static TObject Read<TObject>(FileInfo file) where TObject : class {
            Contract.Requires(file != null && file.Exists);
            if (file == null || !file.Exists) {
                throw new ArgumentNullException(nameof(file));
            }
            lock (file) {
                using (var filer = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    return Read<TObject>(filer);
                }
            }
        }

        /// <summary>
        /// Reads the specified stream.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">stream</exception>
        public static TObject Read<TObject>(Stream stream) where TObject : class {
            Contract.Requires(stream != null);
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            TObject data = default(TObject);
            try {
                IFormatter formatter = new BinaryFormatter();
                data = (TObject)formatter.Deserialize(stream);
                stream.Close();
            } catch (Exception ex) when (ex is IOException | ex is SerializationException) {
                return data;
            }
            return data;
        }

        /// <summary>
        /// Writes the specified object to the specified file.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <exception cref="ArgumentNullException">fileName</exception>
        public static void Write<TObject>(TObject @object, string fileName) where TObject : class {
            Contract.Requires(fileName != null);
            if (fileName == null) throw new ArgumentNullException(nameof(fileName));
            Write(@object, new FileInfo(fileName));
        }

        /// <summary>
        /// Writes the specified object to file.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public static void Write<TObject>(TObject @object, FileInfo file) where TObject : class {
            Contract.Requires(file != null);
            if (file == null) {
                throw new ArgumentNullException(nameof(file));
            }
            lock (file) {
                using (var stream = new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite)) {
                    Write(@object, stream);
                }
            }
        }

        /// <summary>
        /// Writes the specified object via the specified open filestream to an external file on disk.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="object">The object to be written to file.</param>
        /// <param name="stream">The open File-Stream.</param>
        /// <exception cref="ArgumentNullException">stream</exception>
        public static void Write<TObject>(TObject @object, Stream stream) where TObject : class {
            Contract.Requires(stream != null);
            if (stream == null) throw new ArgumentNullException(nameof(stream));

            try {
                IFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, @object);
                stream.Close();
            } catch (Exception ex) when (ex is IOException | ex is SerializationException) {
                err.WriteLine(Constants.ErrorMessage(ex, stream, @object));
            }
        }

        /// <summary>
        /// Reads the specified file asynchronously.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="fileName"></param>
        /// <returns>Object representation of file content.</returns>
        public static async Task<TObject> ReadAsync<TObject>(string fileName) where TObject : class {
            return await Task.Factory.StartNew(() => Read<TObject>(fileName));
        }

        /// <summary>
        /// Reads the specified file asynchronously.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<TObject> ReadAsync<TObject>(FileInfo file) where TObject : class {
            return await Task.Factory.StartNew(() => Read<TObject>(file));
        }

        /// <summary>
        /// Asynchronously writes the specified object via the specified open filestream to an external file on disk.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="object"></param>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static async Task WriteAsync<TObject>(TObject @object, Stream stream) where TObject : class {
             await Task.Factory.StartNew(() => Write(@object, stream));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="object"></param>
        /// <param name="file"></param>
        public static void ProtoWrite<TObject>(TObject @object, string file) where TObject : class {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="object"></param>
        /// <param name="file"></param>
        public static void ProtoWrite<TObject>(TObject @object, FileInfo file) where TObject : class {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="object"></param>
        /// <param name="stream"></param>
        public static void ProtoWrite<TObject>(TObject @object, Stream stream) where TObject : class {
            Contract.Requires(stream != null);
            if (stream == null) throw new ArgumentNullException(nameof(stream));
        }

        /// <summary>
        /// Uses jdk's java.io.Outputstream to write object data to file.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="object">object of reference type to be written to file.</param>
        /// <param name="file">file to write object to.</param>
        /// <param name="append"> append object data to file.</param>
        public static void BufferWrite<TObject>(TObject @object, FileWrapper file, bool append) where TObject : class {
            var tempinfo = file.Parse().AsFile;
            Contract.Requires(file != null && tempinfo != null);
            if (file != null || tempinfo != null) throw new ArgumentNullException(nameof(file));

            using (io.FileOutputStream stream = new io.FileOutputStream(file.File, append)) {
                using(io.ObjectOutputStream oos = new io.ObjectOutputStream(stream)) {
                    oos.writeObject(@object);
                    oos.close();
                }
            }
        }

        /// <summary>
        /// Uses jdk's java.io.Outputstream to write object data to file.
        /// </summary>
        /// <typeparam name="TObject"></typeparam>
        /// <param name="object">object of reference type to be written to file.</param>
        /// <param name="file">file to write object to.</param>
        /// <param name="append"> append object data to file.</param>
        public static void BufferWrite<TObject>(TObject @object, FileInfo file, bool append) where TObject : class {
            Contract.Requires(file != null);
            if (file != null) throw new ArgumentNullException(nameof(file));
            BufferWrite(@object, new FileWrapper(file.FullName),append);
        }


        public static async Task BufferWriteAsync<TObject>(TObject @object, FileInfo file, bool append) where TObject : class {
            await Task.Factory.StartNew(() => BufferWrite(@object, file, append));
        }

        public static TObject BufferRead<TObject>(FileInfo file) where TObject : class {
            Contract.Requires(file != null);
            if (file == null) throw new ArgumentNullException(nameof(file));
            return BufferRead<TObject>(new FileWrapper(file.FullName));
        }

        public static TObject BufferRead<TObject>(FileWrapper fileWrapper) where TObject : class {
            var tempinfo = fileWrapper.Parse().AsFile;
            Contract.Requires(fileWrapper != null && tempinfo != null);
            if (fileWrapper != null || tempinfo != null || !tempinfo.Exists) throw new ArgumentNullException(nameof(fileWrapper));

            TObject obj = null;
            using(io.FileInputStream fis = new io.FileInputStream(fileWrapper.File)) {
                using (io.ObjectInputStream ois = new io.ObjectInputStream(fis)) {
                    obj = (TObject)ois.readObject();
                    ois.close();
                }
            }
            return obj;
        }

        public static void ZeroWrite() { }
        internal static bool IsSafeLoad { get; private set; }
        static FornaxWriter() {
            if (FornaxAssembly.TryResolveAllSerializers()) IsSafeLoad = true;
        }
    }
}
