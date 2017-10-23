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
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Fornax.Net.Util.Security.Cryptography
{

    /// <summary>
    /// Implements a 64-bit CRC hash algorithm for a given polynomial.
    /// </summary>
    /// <remarks>
    /// For ISO 3309 compliant 64-bit CRC's use Crc64Iso.
    /// </remarks>
    public class CRC64 : HashAlgorithm
    {
        public const UInt64 DefaultSeed = 0x0;

        readonly UInt64[] table;

        readonly UInt64 seed;
        UInt64 hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="CRC64"/> class.
        /// </summary>
        /// <param name="polynomial">The polynomial.</param>
        public CRC64(UInt64 polynomial)
            : this(polynomial, DefaultSeed) {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CRC64"/> class.
        /// </summary>
        /// <param name="polynomial">The polynomial.</param>
        /// <param name="seed">The seed.</param>
        public CRC64(UInt64 polynomial, UInt64 seed) {
            table = InitializeTable(polynomial);
            this.seed = hash = seed;
        }

        /// <summary>
        /// Initializes an implementation of the <see cref="T:HashAlgorithm" /> class.
        /// </summary>
        public override void Initialize() {
            hash = seed;
        }

        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize) {
            hash = CalculateHash(hash, table, array, ibStart, cbSize);
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>
        /// The computed hash code.
        /// </returns>
        protected override byte[] HashFinal() {
            var hashBuffer = UInt64ToBigEndianBytes(hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        /// <summary>
        /// Gets the size, in bits, of the computed hash code.
        /// </summary>
        public override int HashSize { get { return 64; } }

        /// <summary>
        /// Calculates the hash.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="table">The table.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="start">The start.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        protected static UInt64 CalculateHash(UInt64 seed, UInt64[] table, IList<byte> buffer, int start, int size) {
            var hash = seed;
            for (var i = start; i < start + size; i++)
                unchecked {
                    hash = (hash >> 8) ^ table[(buffer[i] ^ hash) & 0xff];
                }
            return hash;
        }

        static byte[] UInt64ToBigEndianBytes(UInt64 value) {
            var result = BitConverter.GetBytes(value);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }

        static UInt64[] InitializeTable(UInt64 polynomial) {
            if (polynomial == CRC64Iso.Iso3309Polynomial && CRC64Iso.Table != null)
                return CRC64Iso.Table;

            var createTable = CreateTable(polynomial);

            if (polynomial == CRC64Iso.Iso3309Polynomial)
                CRC64Iso.Table = createTable;

            return createTable;
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        /// <param name="polynomial">The polynomial.</param>
        /// <returns></returns>
        protected static ulong[] CreateTable(ulong polynomial) {
            var createTable = new UInt64[256];
            for (var i = 0; i < 256; ++i) {
                var entry = (UInt64)i;
                for (var j = 0; j < 8; ++j)
                    if ((entry & 1) == 1)
                        entry = (entry >> 1) ^ polynomial;
                    else
                        entry = entry >> 1;
                createTable[i] = entry;
            }
            return createTable;
        }
    }

}
