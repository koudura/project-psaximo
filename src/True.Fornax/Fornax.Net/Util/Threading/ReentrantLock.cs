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

using System.Threading;

namespace Fornax.Net.Util.Threading
{
    /// <summary>
    /// A support class that imitates the java reentrant lock object.
    /// </summary>
    public class ReentrantLock
    {
        private readonly object _lock = new object();

        private int queueLength = 0;

        /// <summary>
        /// Acquires an exclusive reetrant lock on the specified. 
        /// </summary>
        public void Lock() {

            Interlocked.Increment(ref queueLength);
            Monitor.Enter(_lock);
            Interlocked.Decrement(ref queueLength);
        }

        /// <summary>
        /// Releases an exclusive reetrant lock on the specified. 
        /// </summary>
        public void Unlock() {
            Monitor.Exit(_lock);
        }

        /// <summary>
        /// Attempts to acquire an exclusive reentrant lock on a specified object.
        /// </summary>
        /// <returns></returns>
        public bool TryLock() {
            Interlocked.Increment(ref queueLength);
            var success = Monitor.TryEnter(_lock);
            Interlocked.Decrement(ref queueLength);

            return success;
        }

        /// <summary>
        /// Gets the length of the queue.
        /// </summary>
        /// <value>
        /// The length of the queue.
        /// </value>
        public int QueueLength {
            get {
                int est = queueLength;
                return est <= 0 ? 0 : est;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has queued threads.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has queued threads; otherwise, <c>false</c>.
        /// </value>
        public bool HasQueuedThreads => queueLength > 0;
    }
}
