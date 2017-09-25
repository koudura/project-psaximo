/**
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

using System;
using System.Reflection;

namespace Fornax.Net
{
    /// <summary>
    /// A simple attribute for monitoring progress
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage( (AttributeTargets.Class | AttributeTargets.Interface) ,Inherited = false, AllowMultiple = true)]
    internal sealed class Progress : Attribute
    {
        private readonly bool IsComplete;
        private readonly string classname;

        /// <summary>
        /// Initializes a new instance of the <see cref="Progress"/> attribute.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="complete">if set to <c>true</c> [complete].</param>
        public Progress(string name ,bool complete) {
            this.classname = name;
            this.IsComplete = complete;

            CommitProgress();
        }

        /// <summary>
        /// Gets a value indicating whether the specified class holding this instance of attribute <see cref="Progress"/>
        /// is completed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the class is completed; otherwise, <c>false</c>.
        /// </value>
        public bool IsCompleted => this.IsComplete;

        /// <summary>
        /// Gets the name of the class.
        /// </summary>
        /// <value>
        /// The name of the class.
        /// </value>
        public string ClassName => this.classname;

        /// <summary>
        /// Gets or sets a value indicating whether the class holding this <see cref="Progress"/> is tested.
        /// </summary>
        /// <value>
        ///   <c>true</c> if tested; otherwise, <c>false</c>.
        /// </value>
        public bool Tested { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the class holding this <see cref="Progress"/> is documented.
        /// i.e Is the class completely documented
        /// </summary>
        /// <value>
        ///   <c>true</c> if documented; otherwise, <c>false</c>.
        /// </value>
        public bool Documented { get; set;}

        /// <summary>
        /// Commits and logs the progress  of all classes holding 
        /// this attribute to a file in root directory.
        /// </summary>
        private static void CommitProgress() {

        }
    }
}
