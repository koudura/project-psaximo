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
using System.Diagnostics.Contracts;
using System.Threading;

namespace Fornax.Net.Util.Threading
{
    /// <summary>
    /// Support class used to handle threads in fornax-safe way.
    /// </summary>
    /// <seealso cref="IRunnable" />
    public class FornaxThread : IRunnable
    {
        private Thread _thread;
        private Action taskAction;

        /// <summary>
        /// Gets or sets the name of the thread
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get { return _thread.Name; } set { _thread.Name = _thread.Name ?? value; } }

        /// <summary>
        /// Gets the current thread instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public Thread Instance { get { return _thread; } set { _thread = value; } }

        /// <summary>
        /// Gets or sets a value indicating the scheduling priority of a thread.
        /// </summary>
        /// <value>
        /// The priority.
        /// </value>
        public ThreadPriority Priority {
            get {
                try {
                    return _thread.Priority;
                } catch {
                    return ThreadPriority.Normal;
                }
            }
            set {
                try {
                    _thread.Priority = value;
                } catch { }
            }
        }

        /// <summary>
        /// Gets a value indicating the execution status of the current thread.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is alive; otherwise, <c>false</c>.
        /// </value>
        public bool IsAlive => _thread.IsAlive;

        /// <summary>
        /// Gets or sets a value indicating whether or not a thread is a background thread.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is background; otherwise, <c>false</c>.
        /// </value>
        public bool IsBackground { get { return _thread.IsBackground; } set { _thread.IsBackground = value;} }

        /// <summary>
        /// Gets the current state of the Thread.
        /// </summary>
        /// <value>
        /// The <see cref="ThreadState"/>
        /// </value>
        public ThreadState State => _thread.ThreadState;

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxThread"/> class.
        /// </summary>
        public FornaxThread() {
            _thread = new Thread(Run);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxThread" /> class.
        /// </summary>
        /// <param name="task">A ThreadStart delegate that references the methods to be invoked when this thread begins executing.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public FornaxThread(ThreadStart task) {
            Contract.Requires(task != null);
            _thread = new Thread(task) ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Initializes a new instance of the Thread class.
        /// </summary>
        /// <param name="name">The name of the thread.</param>
        public FornaxThread(string name) : this() {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the Thread class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="name">The name of the thread.</param>
        public FornaxThread(ThreadStart task, string name) : this(task) {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxThread"/> class.
        /// </summary>
        /// <param name="taskAction">The action delegate that is invoked when the thread begins executing.</param>
        /// <param name="name">The thread name.</param>
        public FornaxThread(Action taskAction, string name = null) {
            Contract.Requires(taskAction != null);
            this.taskAction = taskAction ?? throw new ArgumentNullException();
            _thread = new Thread(Run);
            Name = name ?? _thread.Name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FornaxThread"/> class.
        /// </summary>
        /// <param name="paramsTasks">The parameterized ThreadStart delegate that references the methods to be invoked when this thread begins executing.</param>
        public FornaxThread(ParameterizedThreadStart paramsTasks) {
            _thread = new Thread(paramsTasks);
        }

        /// <summary>
        /// This method has no functionality unless the method is overridden.
        /// method to contain the code to run on Thread.Start().
        /// </summary>
        public virtual void Run() {
            if (taskAction != null) {
                taskAction.Invoke();
            }
        }

        /// <summary>
        /// Causes the operating system to change the state of the current thread instance to ThreadState.Running.
        /// </summary>
        public void Start() {
            _thread.Start();
        }

        /// <summary>
        /// Interrupts a thread that is in the WaitSleepJoin thread state.
        /// </summary>
        public void Interrupt() {
            _thread.Interrupt();
        }

        /// <summary>
        /// Sets the current thread instance to Run in background or as main thread.
        /// </summary>
        /// <param name="isDaemon">if set to <c>true</c> run as background thread.</param>
        public void SetDaemon(bool isDaemon) {
            _thread.IsBackground = isDaemon;
        }

        /// <summary>
        /// Blocks the calling thread until a thread terminates
        /// </summary>
        public void Join() {
            _thread.Join();
        }

        /// <summary>
        /// Blocks the calling thread until a thread terminates or the specified time elapses.
        /// </summary>
        /// <param name="milliSeconds">Time of wait in milliseconds</param>
        public void Join(long milliSeconds) {
            _thread.Join(Convert.ToInt32(milliSeconds));
        }

        /// <summary>
        /// Blocks the calling thread until a thread terminates or the specified time elapses.
        /// </summary>
        /// <param name="milliSeconds">Time of wait in milliseconds.</param>
        /// <param name="nanoSeconds">Time of wait in nanoseconds.</param>
        public void Join(long milliSeconds, int nanoSeconds) {
            int totalTime = Convert.ToInt32(milliSeconds + (nanoSeconds * 0.000001));
            _thread.Join(totalTime);
        }

        /// <summary>
        ///  Resumes a thread that has been suspended.
        /// </summary>
        public void Resume() {
            Monitor.PulseAll(_thread);
        }

        /// <summary>
        /// Raises a ThreadAbortException in the thread on which it is invoked,
        /// to begin the process of terminating the thread. Calling this method
        /// usually terminates the thread
        /// </summary>
        public void Abort() {
            _thread.Abort();
        }

        /// <summary>
        /// Raises a ThreadAbortException in the thread on which it is invoked,
        /// to begin the process of terminating the thread while also providing
        /// exception information about the thread termination.
        /// Calling this method usually terminates the thread.
        /// </summary>
        /// <param name="stateInfo">An object that contains application-specific information, such as state, which can be used by the thread being aborted.</param>
        public void Abort(object stateInfo) {
            _thread.Abort(stateInfo);
        }

        /// <summary>
        /// Suspends the thread, if the thread is already suspended it has no effect
        /// </summary>
        public void Suspend() {
            Monitor.Wait(_thread);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this thread instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this thread instance.
        /// </returns>
        public override string ToString() {
            return "Thread[" + Name + "," + Priority.ToString() + "]";
        }

        /// <summary>
        /// Determines whether the specified <see cref="object" />, is equal to this thread instance.
        /// </summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="object" /> is equal to this thread instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj) {
            if (obj == null) return false;
            var threadable = obj as FornaxThread;
            if (obj is FornaxThread) return _thread.Equals(threadable._thread);
            return false;
        }

        /// <summary>
        /// Returns a hash code for this thread instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return _thread.GetHashCode();
        }

        [ThreadStatic]
        private static FornaxThread This = null;

        /// <summary>
        /// Same as <see cref="Current"/>. (implementation from java).
        /// </summary>
        /// <returns></returns>
        public static FornaxThread CurrentThread() {
            return Current();
        }

        /// <summary>
        /// Gets the currently running thread instance.
        /// </summary>
        /// <returns>The currently running thread.</returns>
        public static FornaxThread Current() {
            if (This == null) {
                This = new FornaxThread {
                    Instance = Thread.CurrentThread
                };
            }
            return This;
        }

        /// <summary>
        /// Implements the operator == for comparing Fornax threads.
        /// </summary>
        /// <param name="t1">Thread 1.</param>
        /// <param name="t2">Thread 2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator == (FornaxThread t1, object t2) {
            if (((object)t1) == null) return t2 == null;
            return t1.Equals(t2);
        }

        /// <summary>
        /// Implements the operator != for comparing Fornax threads.
        /// </summary>
        /// <param name="t1">Thread 1.</param>
        /// <param name="t2">Thread 2.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static bool operator != (FornaxThread t1, object t2) {
            return !(t1 == t2);
        }


    }
}