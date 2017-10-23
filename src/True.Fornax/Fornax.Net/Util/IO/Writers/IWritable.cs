using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Util.IO.Writers
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TObject">The type of the object.</typeparam>
    public interface IWritable<TObject> where TObject : new()
    {
        /// <summary>
        /// Reads the instance.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <returns></returns>
        TObject ReadInstance(string filename);
        /// <summary>
        /// Writes the instance.
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="obj">The object.</param>
        void WriteInstance(string filename, TObject obj);
        /// <summary>
        /// Writes to temporary.
        /// </summary>
        /// <param name="object">The object.</param>
        void WriteToTemp(TObject @object);
        /// <summary>
        /// Reads from temporary.
        /// </summary>
        /// <returns></returns>
        TObject ReadFromTemp();
    }
}
