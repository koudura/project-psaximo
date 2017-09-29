using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            if (culture == null)
                throw new ArgumentNullException("culture");
            if (uiCulture == null)
                throw new ArgumentNullException("uiCulture");

            this.currentThread = Thread.CurrentThread;

            /** Record the current culture settings so they can be restored later.**/
            this.originalCulture = CultureInfo.CurrentCulture;
            this.originalUICulture = CultureInfo.CurrentUICulture;

            /**
             *Setting both ui and current culture for this thread. 
             **/
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = uiCulture;

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
        public CultureInfo OriginalCulture {
            get { return this.originalCulture; }
        }

        /// <summary>
        /// Gets the original UI culture.
        /// </summary>
        /// <value>
        /// The original UI culture.
        /// </value>
        public CultureInfo OriginalUICulture {
            get { return this.originalUICulture; }
        }

        /// <summary>
        /// Restores the original culture.
        /// </summary>
        public void RestoreOriginalCulture() { 

            Thread.CurrentThread.CurrentCulture = originalCulture;
            Thread.CurrentThread.CurrentUICulture = originalUICulture;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources
        /// of <see cref="CultureContext"/> by <see cref="RestoreOriginalCulture"/>.
        /// </summary>
        public void Dispose() {
            RestoreOriginalCulture();
        }
    }
}
