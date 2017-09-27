/** 
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
***/

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Fornax.Net.Util.System
{
    public static partial class Extensions
    {
        private static ConcurrentDictionary<Resource, string> TempResourceCache = new ConcurrentDictionary<Resource, string>();

        /// <summary>
        /// Aggresively searches for a resource and, if found, returns an open <see cref="Stream"/>
        /// where it can be read.
        /// </summary>
        /// <param name="assembly">this assembly, <see cref="Assembly.GetExecutingAssembly"/> </param>
        /// <param name="type"> a type in the same namespace as the resource</param>
        /// <param name="name">the resource name to locate.</param>
        /// <returns>resource if found; otherwise, null</returns>
        public static Stream FindGetManifestResourceStream(this Assembly assembly, Type type, string name) {
            string resname = FindResource(assembly, type, name);
            if (string.IsNullOrEmpty(resname)) return null;
            return assembly.GetManifestResourceStream(resname);
        }

        /// <summary>
        /// Aggressively searches to find a resource based on a <see cref="Type"/> and resource name.
        /// </summary>
        /// <param name="assembly"> this assembly, <see cref="Assembly.GetExecutingAssembly"/></param>
        /// <param name="type"> a type in the same namespace as the resource</param>
        /// <param name="name">the resource name to locate.</param>
        /// <returns>resource if found; otherwise, null</returns>
        public static string FindResource(this Assembly assembly, Type type, string name) {
            string resname;
            Resource key = new Resource(type, name);
            if (!TempResourceCache.TryGetValue(key, out resname)) {
                string[] resnames = assembly.GetManifestResourceNames();
                resname = resnames.Where(x => x.Equals(name)).FirstOrDefault();

                ///If result is not-null , exact match, dont't search
                if (resname == null) {
                    string assemblyName = type.GetTypeInfo().Assembly.GetName().Name;
                    string namespaceName = type.GetTypeInfo().Namespace;

                    ///search by assembly + namespace
                    string resToFind = string.Concat(namespaceName, ".", name);
                    if (!TryFindResource(resnames, assemblyName, resToFind, name, out resname)) {
                        string found1 = resname;

                        ///search by namespace only
                        if (!TryFindResource(resnames, null, resToFind, name, out resname)) {
                            string found2 = resname;

                            ///search by assembly name only
                            resToFind = string.Concat(assembly, ".", name);
                            if (!TryFindResource(resnames, null, resToFind, name, out resname)) {
                                ///Take the first match of multiple, if there be any.
                                resname = found1 ?? found2 ?? resname;
                            }
                        }
                    }
                }
                TempResourceCache[key] = resname;
            }
            return resname;
        }

        private static bool TryFindResource(string[] resnames, string prefix, string resourceName, string exactResourceName, out string result) {
            if (!resnames.Contains(resourceName)) {
                string nameToFind = null;
                while (resourceName.Length > 0 && resourceName.Contains('.') && (!(string.IsNullOrEmpty(prefix)) || resourceName.Equals(exactResourceName))) {
                    nameToFind = string.IsNullOrEmpty(prefix)
                        ? resourceName
                        : string.Concat(prefix, ".", resourceName);
                    string[] matches = resnames.Where(x => x.EndsWith(nameToFind, StringComparison.Ordinal)).ToArray();
                    if (matches.Length == 1) {
                        result = matches[0]; // Exact match
                        return true;
                    } else if (matches.Length > 1) {
                        result = matches[0]; // First of many
                        return false;
                    }
                    resourceName = resourceName.Substring(resourceName.IndexOf('.') + 1);
                }
                result = null; // No match
                return false;
            }
            result = resourceName;
            return true;
        }
    }
}
