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

namespace Fornax.Net.Util.Collections.Generic
{
    /// <summary>
    /// Signified the type of change operation carried out the observable list,
    /// <seealso cref="ObservableList{T}"/>.
    /// </summary>
    public enum ListOperation
    {
        /// <summary>
        /// An item Was Added to the <see cref="ObservableList{T}"/>.
        /// </summary>
        Add,
        /// <summary>
        /// An item was removed at a specified index from <see cref="ObservableList{T}"/>.
        /// </summary>
        RemoveAt,
        /// <summary>
        /// The first occurence of an item was removed from <see cref="ObservableList{T}"/>.
        /// </summary>
        Remove,
        /// <summary>
        /// An item was Inserted into <see cref="ObservableList{T}"/>.
        /// </summary>
        Insert,
        /// <summary>
        /// The <see cref="ObservableList{T}"/> was cleared.
        /// This resetsa all index and Item to default values. 
        /// </summary>
        Clear,
        /// <summary>
        /// An Item was set in the <see cref="ObservableList{T}"/>. 
        /// </summary>
        Set
    }

    /// <summary>
    /// Defines if and how items added to a LurchTable are linked together, this defines
    /// the value returned from Peek/Dequeue as the oldest entry of the specified operation.
    /// </summary>
    public enum LurchOrder
    {
        /// <summary> No linking </summary>
        None,
        /// <summary> Linked in insertion order </summary>
        Insertion,
        /// <summary> Linked by most recently inserted or updated </summary>
        Modified,
        /// <summary> Linked by most recently inserted, updated, or fetched </summary>
        Access,
    }

}
