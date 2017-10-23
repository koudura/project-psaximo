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
*   Copyright (c) Damien Guard.  All rights reserved.
* Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
* You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
* Originally published at http://damieng.com/blog/2007/11/19/calculating-crc-64-in-c-and-net  
* 
**/

using System;

namespace Fornax.Net.Util.Security.Cryptography
{
    /// <summary>
    /// Implements a 64-bit CRC hash algorithm for a given polynomial.
    /// </summary>
    /// <remarks>
    /// For ISO 3309 compliant 64-bit CRC's use Crc64Iso.
    /// </remarks>
    public sealed class CRC64Iso : CRC64
    {
        internal static UInt64[] Table;

        /// <summary>
        /// The iso3309 polynomial
        /// </summary>
        public const UInt64 Iso3309Polynomial = 0xD800000000000000;

        /// <summary>
        /// Initializes a new instance of the <see cref="CRC64Iso"/> class.
        /// </summary>
        public CRC64Iso()
            : base(Iso3309Polynomial) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CRC64Iso"/> class.
        /// </summary>
        /// <param name="seed">The seed.</param>
        public CRC64Iso(UInt64 seed)
            : base(Iso3309Polynomial, seed) {
        }

        /// <summary>
        /// Computes the specified buffer.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static UInt64 Compute(byte[] buffer) {
            return Compute(DefaultSeed, buffer);
        }

        /// <summary>
        /// Computes the hashcode for the specified buffer using the specified seed.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns></returns>
        public static UInt64 Compute(UInt64 seed, byte[] buffer) {
            if (Table == null)
                Table = CreateTable(Iso3309Polynomial);

            return CalculateHash(seed, Table, buffer, 0, buffer.Length);
        }
    }


}
