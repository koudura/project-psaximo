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

namespace Fornax.Net.Util.Security
{
    /// <summary>
    /// Defines functions for all encoders and decoders.
    /// </summary>
    public interface IEncoder 
    {
        /// <summary>
        /// Encodes the specified bytes.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>an encoded <see cref="string"/> representation of <paramref name="bytes"/>.</returns>
        string Encode(byte[] bytes);

        /// <summary>
        /// Decodes the specified string.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>a decoded <see cref="byte[]"/> represenation of the string</returns>
        byte[]  Decode(string input);

    }
}
