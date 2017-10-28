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

namespace Fornax.Net.Util.IO
{
    /// <summary>
    /// Enum to manage delegate handlers for loading and preloading Dependencies 
    /// and external libraries. Manages eventHandlers Dynamic assembly runtime resolve.
    /// </summary>
    [Serializable]
    public enum FornaxDependency {
        /// <summary>
        /// The load on demand procedure notifies its event handler to load dependency library on demand to embedded memory.
        /// This Event Listens constantly at runtime for changes made to the default assembly external directory.
        /// then creates another event handler for each task by user.
        /// </summary>
        LoadOnDemand,
        /// <summary>
        /// The pre load procedure initializes a function that loads all specified dependency librariies at initialization time.
        /// This Loads library to embedded resources via memory stream, and clears embedded resources on close or after a specified time by user.
        /// </summary>
        PreLoad,
        /// <summary>
        /// The register on demand procedure works similar to <see cref="LoadOnDemand"/>, but byte[] representation of dependency library gets 
        /// gets regisered to the embeded resource. Persistence is thereby ensured. This Event When called once, erradicates the need for another call
        /// during a separate runtime, because previously loaded library as being registered into [Fornax.Net.Common.SandBox].
        /// </summary>
        RegisterOnDemand,
        /// <summary>
        /// The register procedure, much like the <seealso cref="PreLoad"/>. But byte[] representation get registered into fornax.net
        /// see <seealso cref="RegisterOnDemand"/> for more registration details.
        /// </summary>
        Register

    }
}