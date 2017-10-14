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
using System.Runtime.Serialization;

namespace Fornax.Net.Util.Collections.Generic
{
    /// <summary>
    /// Exception class: LurchTableCorruptionException
    /// The LurchTable internal datastructure appears to be corrupted.
    /// </summary>

    [Serializable]
    [global::System.Diagnostics.DebuggerStepThrough]
    [global::System.Diagnostics.DebuggerNonUserCode]
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    [global::System.CodeDom.Compiler.GeneratedCode("CSharpTest.Net.Generators", "2.13.222.435")]
    public partial class LurchCorruptionException : Exception
    {

        /// <summary>
        /// Serialization constructor
        /// </summary>
        protected LurchCorruptionException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }

        /// <summary>
        /// Used to create this exception from an hresult and message bypassing the message formatting
        /// </summary>
        internal static Exception Create(int hResult, string message) {
            return new LurchCorruptionException((Exception)null, hResult, message);
        }
        /// <summary>
        /// Constructs the exception from an hresult and message bypassing the message formatting
        /// </summary>
        protected LurchCorruptionException(Exception innerException, int hResult, string message) : base(message, innerException) {
            base.HResult = hResult;
        }
        /// <summary>
        /// The LurchTable internal datastructure appears to be corrupted.
        /// </summary>
        public LurchCorruptionException()
            : this((Exception)null, -1, "The LurchTable internal datastructure appears to be corrupted.") {
        }
        /// <summary>
        /// The LurchTable internal datastructure appears to be corrupted.
        /// </summary>
        public LurchCorruptionException(Exception innerException)
            : this(innerException, -1, "The LurchTable internal datastructure appears to be corrupted.") {
        }
        /// <summary>
        /// if(condition == false) throws The LurchTable internal datastructure appears to be corrupted.
        /// </summary>
        public static void Assert(bool condition) {
            if (!condition) throw new LurchCorruptionException();
        }
    }
}

