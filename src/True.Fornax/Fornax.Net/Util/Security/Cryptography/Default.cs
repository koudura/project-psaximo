/** MIT LICENSE
* 
*   Copyright (c) 2017 Koudura Ninci @True.Inc
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

using System.Security.Cryptography;

namespace Fornax.Net.Util.Security.Cryptography
{
    /// <summary>
    /// Fornax Default Hash Algorithm.
    /// </summary>
    class Default
    {
        /// <summary>
        /// Get's and Sets the FIPS compliancy of hash algorithm
        /// </summary>
        public static bool FIPSCompliant { get; set; }

        /// <summary>
        /// returns Fornax default <seealso cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        public static HashAlgorithm HashAlgorithm {
            get {
                if (FIPSCompliant) { return SHA1.Create(); }
                return MD5.Create();
            }
        }

        static Default() {
            FIPSCompliant = false;
        }
    }
}
