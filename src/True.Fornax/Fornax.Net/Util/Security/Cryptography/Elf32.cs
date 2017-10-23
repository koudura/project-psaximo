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
    /// Implements a 32-bit ELF hash algorithm compatible with ELF binary format.
    /// </summary>
    public sealed class Elf32 : HashAlgorithm
    {
        UInt32 hash;

        /// <summary>
        /// Initializes a new instance of the <see cref="Elf32"/> class.
        /// </summary>
        public Elf32() {
            hash = 0;
        }

        /// <summary>
        /// Initializes an implementation of the <see cref="T:System.Security.Cryptography.HashAlgorithm" /> class.
        /// </summary>
        public override void Initialize() {
            hash = 0;
        }

        /// <summary>
        /// When overridden in a derived class, routes data written to the object into the hash algorithm for computing the hash.
        /// </summary>
        /// <param name="array">The input to compute the hash code for.</param>
        /// <param name="ibStart">The offset into the byte array from which to begin using data.</param>
        /// <param name="cbSize">The number of bytes in the byte array to use as data.</param>
        protected override void HashCore(byte[] array, int ibStart, int cbSize) {
            hash = CalculateHash(hash, array, ibStart, cbSize);
        }

        /// <summary>
        /// When overridden in a derived class, finalizes the hash computation after the last data is processed by the cryptographic stream object.
        /// </summary>
        /// <returns>
        /// The computed hash code.
        /// </returns>
        protected override byte[] HashFinal() {
            var hashBuffer = UInt32ToBigEndianBytes(hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        /// <summary>
        /// Gets the size, in bits, of the computed hash code.
        /// </summary>
        public override int HashSize { get { return 32; } }

        /// <summary>
        /// Computes the elf-32 hash code for the specified buffer using default polynomial and seed.
        /// </summary>
        /// <param name="buffer">The buffer.</param>
        /// <returns>a 32-bit integer hash code.</returns>
        public static UInt32 Compute(byte[] buffer) {
            return CalculateHash(0, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Computes the elf-32 hash code for the specified buffer using the provided 
        /// polynomial and seed.
        /// </summary>
        /// <param name="polynomial">The polynomial.</param>
        /// <param name="seed">The seed.</param>
        /// <param name="buffer">The buffer.</param>
        /// <returns>a 32-bit integer hash code.</returns>
        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer) {
            return CalculateHash(seed, buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Calculates the hash code.
        /// </summary>
        /// <param name="seed">The seed.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="start">The start.</param>
        /// <param name="size">The size.</param>
        /// <returns></returns>
        static UInt32 CalculateHash(UInt32 seed, IList<byte> buffer, int start, int size) {
            var hash = seed;
            for (var i = start; i < start + size; i++) {
                hash = (hash << 4) + buffer[i];
                var work = hash & 0xf0000000u;
                hash ^= work >> 24;
                hash &= ~work;
            }
            return hash;
        }

        static byte[] UInt32ToBigEndianBytes(UInt32 uint32) {
            var result = BitConverter.GetBytes(uint32);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);

            return result;
        }
    }
}

