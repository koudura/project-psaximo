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
 * Copyright (c) Damien Guard.  All rights reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
 * Originally published at http://damieng.com/blog/2007/11/19/calculating-crc-64-in-c-and-net
 *  
 ***/

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

        public Elf32() {
            hash = 0;
        }

        public override void Initialize() {
            hash = 0;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize) {
            hash = CalculateHash(hash, array, ibStart, cbSize);
        }

        protected override byte[] HashFinal() {
            var hashBuffer = UInt32ToBigEndianBytes(hash);
            HashValue = hashBuffer;
            return hashBuffer;
        }

        public override int HashSize { get { return 32; } }

        public static UInt32 Compute(byte[] buffer) {
            return CalculateHash(0, buffer, 0, buffer.Length);
        }

        public static UInt32 Compute(UInt32 polynomial, UInt32 seed, byte[] buffer) {
            return CalculateHash(seed, buffer, 0, buffer.Length);
        }

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

