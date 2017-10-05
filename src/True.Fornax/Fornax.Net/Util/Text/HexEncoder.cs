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

namespace Fornax.Net.Util.Text
{
    /// <summary>
    /// Hexadecimal Encoder for Hex-<see cref="string"/> formats.
    /// </summary>
    /// <seealso cref="IEncoder" />
    public class HexEncoder : IEncoder
    {

        private int Hex(char a) {
            if (a >= '0' && a <= '9')
                return a - '0';

            if (a >= 'a' && a <= 'f')
                return a - 'a' + 10;

            if (a >= 'A' && a <= 'F')
                return a - 'A' + 10;

            throw new ArgumentOutOfRangeException("a", String.Format("Character {0} is not hexadecimal", a));
        }

        /// <summary>
        /// Decodes the specified string input to its <c>byte[]</c>.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException">input - Must be an even number of hex digits</exception>
        public virtual byte[] Decode(string input) {
            if (input.Length % 2 == 1)
                throw new ArgumentOutOfRangeException("input", "Must be an even number of hex digits");

            var output = new byte[input.Length / 2];
            var textIndex = 0;
            for (var outputIndex = 0; outputIndex < output.Length; outputIndex++) {
                var b = (byte)((Hex(input[textIndex++]) << 4) + Hex(input[textIndex++]));
                output[outputIndex] = b;
            }
            return output;
        }

        /// <summary>
        /// Encodes the specified bytes into its relative Default Base format.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>an encoded byte as string</returns>
        public virtual string Encode(byte[] bytes) {
            var output = new char[bytes.Length * 2];
            var outputIndex = 0;
            for (var byteIndex = 0; byteIndex < bytes.Length; byteIndex++) {
                var hex = bytes[byteIndex].ToString("X2");
                output[outputIndex++] = hex[0];
                output[outputIndex++] = hex[1];
            }
            return new string(output);
        }


    }
}
