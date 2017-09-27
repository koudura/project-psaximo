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

namespace Fornax.Net.Util.System
{
    /// <summary>
    /// Fornax resource holder.
    /// </summary>
    internal sealed class Resource
    {
        private readonly Type type;
        private readonly string name;


        internal Resource(Type type, string name) {
            this.type = type;
            this.name = name;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (!(obj is Resource)) return false;
            var other = obj as Resource;
            return (this.type.Equals(other.type) && (this.name.Equals(other.name)));
        }

        /// <summary>
        /// Returns a hash code for this <see cref="Resource"/>
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() => (this.type.GetHashCode() ^ this.name.GetHashCode());

        /// <summary>
        /// Returns a <see cref="string" /> that represents this <see cref="Resource"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format($"{nameof(Resource)} ~[Type:{nameof(type)} = {type}, Name:{nameof(name)} = {name}].");
        }
    }
}
