using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

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
        public static TObject Read<TObject>(string fileName) where TObject : new() {
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
        public static TObject Read<TObject>(FileInfo file) where TObject : new() {
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
        public static TObject Read<TObject>(Stream stream) where TObject : new() {
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
        public static void Write<TObject>(TObject @object, string fileName) where TObject : new() {
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
        public static void Write<TObject>(TObject @object, FileInfo file) where TObject : new() {
            Contract.Requires(file != null);
            if (file == null) {
                throw new ArgumentNullException(nameof(file));
            }
            lock (file) {
                using (var stream = new FileStream(file.FullName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite)) {
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
        public static void Write<TObject>(TObject @object, Stream stream) where TObject : new() {
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

        public static async Task<TObject> ReadAsync<TObject>(string fileName) where TObject : new() {
            return await Task.Factory.StartNew(() => Read<TObject>(fileName));
        }

        public static async Task<TObject> ReadAsync<TObject>(FileInfo file) where TObject : new() {
            return await Task.Factory.StartNew(() => Read<TObject>(file));
        }

        public static void BufferWrite() { }
        public static void ZeroWrite() { }

        internal static bool IsSafeLoad { get; private set; }
        static FornaxWriter() {
            if (FornaxAssembly.TryResolveAllSerializers()) IsSafeLoad = true;
        }
    }
}
