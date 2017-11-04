// ***********************************************************************
// Assembly         : Fornax.Net
// Author           : Habuto Koudura
// Created          : 10-26-2017
//
// Last Modified By : Habuto Koudura
// Last Modified On : 10-30-2017
// ***********************************************************************
// <copyright file="FornaxWriter.cs" company="Microsoft">
//     Copyright © Microsoft 2017
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using io = java.io;
using ProtoBuf;

/// <summary>
/// The Writers namespace.
/// </summary>
namespace Fornax.Net.Util.IO.Writers
{
    /// <summary>
    /// Class FornaxWriter.
    /// </summary>
    public static class FornaxWriter
    {
        /// <summary>
        /// The error
        /// </summary>
        static TextWriter err = Console.Error;

        /// <summary>
        /// Reads an object representation from the specified file.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>TObject.</returns>
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
        /// <returns>TObject.</returns>
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
        /// <returns>TObject.</returns>
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
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Object representation of file content.</returns>
        public static async Task<TObject> ReadAsync<TObject>(string fileName) where TObject : class {
            return await Task.Factory.StartNew(() => Read<TObject>(fileName));
        }

        /// <summary>
        /// Asynchronously writes the specified object via the specified open filestream to an external file on disk.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="stream">The stream.</param>
        /// <returns>Task.</returns>
        public static async Task WriteAsync<TObject>(TObject @object, Stream stream) where TObject : class {
             await Task.Factory.StartNew(() => Write(@object, stream));
        }

        /// <summary>
        /// write as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="file">The file.</param>
        /// <returns>Task.</returns>
        public static async Task WriteAsync<TObject>(TObject @object, FileInfo file)  where TObject : class{
            await Task.Factory.StartNew(() => Write(@object, file));
        }

        /// <summary>
        /// write as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>Task.</returns>
        public static async Task WriteAsync<TObject>(TObject @object, string fileName) where TObject : class {
            await Task.Factory.StartNew(() => Write(@object, fileName));
        }

        /// <summary>
        /// Protoes the write.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="file">The file.</param>
        public static void ProtoWrite<TObject>(TObject @object, string file) where TObject : class {

        }

        /// <summary>
        /// Protoes the write.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="file">The file.</param>
        public static void ProtoWrite<TObject>(TObject @object, FileInfo file) where TObject : class {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Protoes the write.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="stream">The stream.</param>
        /// <exception cref="ArgumentNullException">stream</exception>
        public static void ProtoWrite<TObject>(TObject @object, Stream stream) where TObject : class {
            Contract.Requires(stream != null);
            if (stream == null) throw new ArgumentNullException(nameof(stream));
            throw new NotImplementedException();
        }

        /// <summary>
        /// Uses jdk's java.io.Outputstream to write object data to file.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">object of reference type to be written to file.</param>
        /// <param name="file">file to write object to.</param>
        /// <param name="append">append object data to file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
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
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">object of reference type to be written to file.</param>
        /// <param name="file">file to write object to.</param>
        /// <param name="append">append object data to file.</param>
        /// <exception cref="ArgumentNullException">file</exception>
        public static void BufferWrite<TObject>(TObject @object, FileInfo file, bool append) where TObject : class {
            Contract.Requires(file != null);
            if (file != null) throw new ArgumentNullException(nameof(file));
            BufferWrite(@object, new FileWrapper(file.FullName),append);
        }


        /// <summary>
        /// buffer write as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="object">The object.</param>
        /// <param name="file">The file.</param>
        /// <param name="append">if set to <c>true</c> [append].</param>
        /// <returns>Task.</returns>
        public static async Task BufferWriteAsync<TObject>(TObject @object, FileInfo file, bool append) where TObject : class {
            await Task.Factory.StartNew(() => BufferWrite(@object, file, append));
        }

        /// <summary>
        /// Buffers the read.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="file">The file.</param>
        /// <returns>TObject.</returns>
        /// <exception cref="ArgumentNullException">file</exception>
        public static TObject BufferRead<TObject>(FileInfo file) where TObject : class {
            Contract.Requires(file != null);
            if (file == null) throw new ArgumentNullException(nameof(file));
            return BufferRead<TObject>(new FileWrapper(file.FullName));
        }

        /// <summary>
        /// Buffers the read.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="fileWrapper">The file wrapper.</param>
        /// <returns>TObject.</returns>
        /// <exception cref="ArgumentNullException">fileWrapper</exception>
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

        /// <summary>
        /// buffer read as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TObject">The type of the t object.</typeparam>
        /// <param name="file">The file.</param>
        /// <returns>Task&lt;TObject&gt;.</returns>
        public static async Task<TObject> BufferReadAsync<TObject>(FileInfo file) where TObject : class 
        {
            return await Task.Factory.StartNew(() => BufferRead<TObject>(file));
        }

        /// <summary>
        /// Zeroes the write.
        /// </summary>
        public static void ZeroWrite() { }
        /// <summary>
        /// Gets a value indicating whether this instance is safe load.
        /// </summary>
        /// <value><c>true</c> if this instance is safe load; otherwise, <c>false</c>.</value>
        internal static bool IsSafeLoad { get; private set; }
        /// <summary>
        /// Initializes static members of the <see cref="FornaxWriter" /> class.
        /// </summary>
        static FornaxWriter() {
            if (FornaxAssembly.TryResolveAllSerializers()) IsSafeLoad = true;
        }
    }
}
