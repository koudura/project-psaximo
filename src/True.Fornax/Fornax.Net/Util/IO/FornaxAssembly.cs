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
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Dependency = Fornax.Net.Lib.Dependency;

namespace Fornax.Net.Util.IO
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Reflection.Assembly" />
    /// <seealso cref="System.IDisposable" />
    [Progress("FornaxAssembly", false, Documented = false, Tested = false)]
    public sealed class FornaxAssembly : Assembly, IDisposable
    {
        private static IDictionary<string, Assembly> resolvedAssemblies;
        private Assembly assembly;
        private IList<Assembly> assemblies;
        private static Assembly hiddenAssembly;
        private static string name;

        /// <summary>
        /// Gets the display name of the assembly.
        /// </summary>
        public override string FullName => assembly.FullName;

        /// <summary>
        /// Gets the name of the Assembly.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name => name;

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxAssembly"/> class.
        /// </summary>
        /// <param name="_assembly">The assembly.</param>
        /// <exception cref="FornaxAssemblyException">_assembly</exception>
        public FornaxAssembly(Assembly _assembly) {
            Contract.Requires(_assembly != null);
            this.assembly = _assembly ?? throw new FornaxAssemblyException(nameof(_assembly));

            name = _assembly.GetName().FullName;
            hiddenAssembly = this.assembly;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxAssembly"/> class.
        /// </summary>
        /// <param name="_assemblies">The assemblies.</param>
        /// <exception cref="FornaxAssemblyException">_assemblies</exception>
        public FornaxAssembly(IEnumerable<Assembly> _assemblies) {
            Contract.Requires(assemblies != null);
            if (_assemblies == null) throw new FornaxAssemblyException(nameof(_assemblies));
            this.assemblies = _assemblies.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxAssembly"/> class.
        /// </summary>
        /// <param name="assemblyBuffer">The assembly buffer.</param>
        public FornaxAssembly(byte[] assemblyBuffer) : this(Assembly.Load(assemblyBuffer)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxAssembly"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="FornaxAssemblyException"></exception>
        public FornaxAssembly(FileInfo file) {
            Contract.Requires(file != null);
            if (!file.Exists || file == null) throw new FornaxAssemblyException();
            name = file.Name;
            new FornaxAssembly(LoadFrom(file.FullName));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxAssembly"/> class.
        /// </summary>
        /// <param name="filename">The filename.</param>
        public FornaxAssembly(AssemblyName filename) : this(File.ReadAllBytes(filename.FullName)) {
            name = filename.FullName;
        }

        private static bool Resolve() {
            try { AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve; return true; } catch (Exception) { return false; }
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args) {
            return LoadAssembly(name);
        }

        /// <summary>
        /// Tries the resolve all.
        /// </summary>
        /// <param name="_assemblies">The assemblies.</param>
        /// <returns></returns>
        public static bool TryResolveAll(Assembly[] _assemblies) {
            try {
                foreach (var item in _assemblies) {
                    if (!TryResolve(item)) return false;
                }
                return true;
            } catch (Exception) {
                return false;
            }
        }

        /// <summary>
        /// Tries the resolve.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <returns></returns>
        public static bool TryResolve(Assembly assembly) {
            hiddenAssembly = assembly;
            return Resolve();
        }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        /// <exception cref="FornaxAssemblyException">name</exception>
        public static Assembly LoadAssembly(AssemblyName name) {
            Contract.Requires(name != null);
            if (name == null) throw new FornaxAssemblyException(nameof(name));
            return LoadAssembly(name.FullName);
        }

        /// <summary>
        /// Gets the assembly.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static Assembly LoadAssembly(string name) {
            if (string.IsNullOrEmpty(name)) {
                throw new ArgumentException("invalid libarary - ", nameof(name));
            }

            if (!resolvedAssemblies.ContainsKey(name)) {
                using (var stream = GetExecutingAssembly().GetManifestResourceStream(name)) {
                    byte[] temp = new byte[stream.Length];
                    stream.Read(temp, 0, temp.Length);
                    var ass = Load(temp);
                    resolvedAssemblies.Add(name, ass);
                    return ass;
                }
            } else return resolvedAssemblies[name];
        }

        /// <summary>
        /// Loads the specified temporary.
        /// </summary>
        /// <param name="temp">The temporary.</param>
        /// <returns></returns>
        public new static Assembly Load(byte[] temp) {
            try {
                return Assembly.Load(temp);
            } catch { return null; }
        }

        /// <summary>
        /// Loads all.
        /// </summary>
        /// <param name="temp">The temporary.</param>
        /// <returns></returns>
        /// <exception cref="FornaxAssemblyException"></exception>
        public static Assembly[] LoadAll(IEnumerable<byte[]> temp) {
            if (temp == null) throw new FornaxAssemblyException();
            byte[][] barr = temp.ToArray();
            Assembly[] assemblies = new Assembly[barr.Length];
            for (int i = 0; i < assemblies.Length; i++) {
                assemblies[i] = Load(barr[i]);
            }
            return assemblies;
        }

        /// <summary>
        /// Gets the names.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns></returns>
        public static IEnumerable<AssemblyName> GetNames(string[] assemblies) {
            var assembly_names = from dll in assemblies
                                 where (!string.IsNullOrEmpty(dll))
                                 let dll_name = new AssemblyName(dll)
                                 select dll_name;
            foreach (var item in assembly_names) {
                yield return item;
            }
        }

        internal static bool TryResolveAllDependencies() {
            return Dependency.ResolveAll(Dependency.All_deps);
        }

        internal static bool TryResolveIKVMs() {
            return Dependency.ResolveAll(Dependency.Get_IKVM_AssemblyNames());
        }

        internal static bool TryResolveToxy() {
            return Dependency.ResolveAll(Dependency.Get_Toxy_Names());
        }

        internal static bool TryResolveTika() {
            if (Dependency.ResolveAll(Dependency.Get_IKVM_AssemblyNames())) {
                return Dependency.ResolveAll(Dependency.Get_Tika_Names());
            }
            return false;
        }

        internal static bool TryResolveMisc() {
            return Dependency.ResolveAll(Dependency.Get_MISC_Names());
        }

        internal static bool TryResolveLZ4() {
            return Dependency.ResolveAll(new[] { Dependency.LZ4, Dependency.LZ4NET });
        }

        internal static bool TryResolveProtoBuf() {
            return Dependency.ResolveAll(new[] { Dependency.PROTOBUF });
        }

        internal static bool TryResolveZero() {
            return Dependency.ResolveAll(Dependency.Get_Zero_Names());
        }

        internal static bool TryResolvePDFBox() {
            return Dependency.ResolveAll(new[] { Dependency.PDFBOX});
        }

        internal static bool TryResolveAllSerializers() {
            bool proto = TryResolveProtoBuf();
            bool zero = TryResolveZero();

            return proto && zero;
        }

        internal static bool TryResolveAnySerializer() {
            return TryResolveProtoBuf() || TryResolveZero();
        }

        internal static bool TryResolveHtmlPack() {
            return Dependency.ResolveAll(new[] { Dependency.HTML_AGILITY_PACK });
        }

        #region override
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() { }

        /// <summary>
        /// Tries the resolve.
        /// </summary>
        /// <returns></returns>
        public bool TryResolve() {
            return TryResolve(this.assembly);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// </summary>
        /// <param name="o">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object o) {
            return assembly.Equals(o);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return assembly.GetHashCode();
        }

        /// <summary>
        /// Gets an <see cref="T:System.Reflection.AssemblyName" /> for this assembly.
        /// </summary>
        /// <returns>
        /// An object that contains the fully parsed display name for this assembly.
        /// </returns>
        public override AssemblyName GetName() {
            return assembly.GetName();
        }

        /// <summary>
        /// Gets the names.
        /// </summary>
        /// <returns></returns>
        public AssemblyName[] GetNames() {
            var assembly_names = from dll in this.assemblies
                                 where (!string.IsNullOrEmpty(dll.FullName))
                                 select dll.GetName();
            return assembly_names.ToArray();
        }

        /// <summary>
        /// Gets the types defined in this assembly.
        /// </summary>
        /// <returns>
        /// An array that contains all the types that are defined in this assembly.
        /// </returns>
        public override Type[] GetTypes() {
            return assembly.GetTypes();
        }

        #endregion

        /// <summary>
        /// Initializes the <see cref="FornaxAssembly"/> class.
        /// </summary>
        static FornaxAssembly() {
            resolvedAssemblies = new Dictionary<string, Assembly>();
        }

    }
}
