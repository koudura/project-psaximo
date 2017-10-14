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
using System.Globalization;
using System.Threading;

namespace Fornax.Net.Util.System
{
    /// <summary>
    /// Allows switching the current thread to a new culture in a using block that will automatically 
    /// return the culture to its previous state upon completion.
    /// </summary>
    [Progress("CultureContext",true,Documented = true, Tested = true)]
    public sealed class CultureContext : IDisposable
    { 

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureContext"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public CultureContext(int culture)
          : this(new CultureInfo(culture), CultureInfo.CurrentUICulture) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureContext"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="uiCulture">The UI culture.</param>
        public CultureContext(int culture, int uiCulture)
            : this(new CultureInfo(culture), new CultureInfo(uiCulture)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureContext"/> class.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        public CultureContext(string cultureName)
            : this(new CultureInfo(cultureName), CultureInfo.CurrentUICulture) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureContext"/> class.
        /// </summary>
        /// <param name="cultureName">Name of the culture.</param>
        /// <param name="uiCultureName">Name of the UI culture.</param>
        public CultureContext(string cultureName, string uiCultureName)
            : this(new CultureInfo(cultureName), new CultureInfo(uiCultureName)) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureContext"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        public CultureContext(CultureInfo culture)
            : this(culture, CultureInfo.CurrentUICulture) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CultureContext"/> class.
        /// </summary>
        /// <param name="culture">The culture.</param>
        /// <param name="uiCulture">The UI culture.</param>
        /// <exception cref="ArgumentNullException">
        /// culture
        /// or
        /// uiCulture
        /// </exception>
        public CultureContext(CultureInfo culture, CultureInfo uiCulture) {
            /***
             *Setting both ui and current culture for this thread. 
             **/
            Thread.CurrentThread.CurrentCulture = culture ?? throw new ArgumentNullException("culture");
            Thread.CurrentThread.CurrentUICulture = uiCulture ?? throw new ArgumentNullException("uiCulture");

            currentThread = Thread.CurrentThread;

            /*** Record the current culture settings so they can be restored later.**/
            originalCulture = CultureInfo.CurrentCulture;
            originalUICulture = CultureInfo.CurrentUICulture;

        }

        private readonly Thread currentThread;
        private readonly CultureInfo originalCulture;
        private readonly CultureInfo originalUICulture;

        /// <summary>
        /// Gets the original culture.
        /// </summary>
        /// <value>
        /// The original culture.
        /// </value>
        public CultureInfo OriginalCulture => originalCulture;

        /// <summary>
        /// Gets the original UI culture.
        /// </summary>
        /// <value>
        /// The original UI culture.
        /// </value>
        public CultureInfo OriginalUICulture => originalUICulture;

        /// <summary>
        /// Restores the original culture.
        /// </summary>
        public void RestoreOriginalCulture() { 

            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUICulture;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// of <see cref="CultureContext" /> by <see cref="RestoreOriginalCulture" />.
        /// </summary>
        public void Dispose() {
            RestoreOriginalCulture();
        }
    }
}
