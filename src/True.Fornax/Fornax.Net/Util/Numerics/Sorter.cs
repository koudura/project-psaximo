using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fornax.Net.Util.Numerics
{
   sealed class Sorter<T>
    {
        T[] array;

        IComparer<T> comparer;

        internal Sorter(T[] array, IComparer<T> comparer) {
            this.array = array;
            this.comparer = comparer;
        }

        internal void IntroSort(int f, int b) {
            if (b - f > 31) {
                int depth_limit = (int)Math.Floor(2.5 * Math.Log(b - f, 2));

                IntroSort(f, b, depth_limit);
            } else
                InsertionSort(f, b);
        }

        private void IntroSort(int f, int b, int depth_limit) {
            const int size_threshold = 24;

            if (depth_limit-- == 0)
                HeapSort(f, b);
            else if (b - f <= size_threshold)
                InsertionSort(f, b);
            else {
                int p = Partition(f, b);

                IntroSort(f, p, depth_limit);
                IntroSort(p, b, depth_limit);
            }
        }

        private int Compare(T i1, T i2) { return comparer.Compare(i1, i2); }

        private int Partition(int f, int b) {
            int bot = f, mid = (b + f) / 2, top = b - 1;
            T abot = array[bot], amid = array[mid], atop = array[top];

            if (Compare(abot, amid) < 0) {
                if (Compare(atop, abot) < 0) {
                    array[top] = amid; amid = array[mid] = abot; array[bot] = atop;
                } else if (Compare(atop, amid) < 0) { //abot<=atop<amid
                    array[top] = amid; amid = array[mid] = atop;
                }
            } else { //else abot<amid<=atop

                if (Compare(amid, atop) > 0) {   //atop<amid<=abot 
                    array[bot] = atop; array[top] = abot;
                } else if (Compare(abot, atop) > 0) {   //amid<=atop<abot
                    array[bot] = amid; amid = array[mid] = atop; array[top] = abot;
                } else {    //amid<=abot<=atop
                    array[bot] = amid; amid = array[mid] = abot;
                }
            }

            int i = bot, j = top;

            while (true) {
                while (Compare(array[++i], amid) < 0) ;

                while (Compare(amid, array[--j]) < 0) ;

                if (i < j) {
                    T tmp = array[i]; array[i] = array[j]; array[j] = tmp;
                } else
                    return i;
            }
        }

        internal void InsertionSort(int f, int b) {
            for (int j = f + 1; j < b; j++) {
                T key = array[j], other;
                int i = j - 1;

                if (comparer.Compare(other = array[i], key) > 0) {
                    array[j] = other;
                    while (i > f && comparer.Compare(other = array[i - 1], key) > 0) { array[i--] = other; }

                    array[i] = key;
                }
            }
        }

        internal void HeapSort(int f, int b) {
            for (int i = (b + f) / 2; i >= f; i--) Heapify(f, b, i);

            for (int i = b - 1; i > f; i--) {
                T tmp = array[f]; array[f] = array[i]; array[i] = tmp;
                Heapify(f, i, f);
            }
        }


        private void Heapify(int f, int b, int i) {
            T pv = array[i], lv, rv, max = pv;
            int j = i, maxpt = j;

            while (true) {
                int l = 2 * j - f + 1, r = l + 1;

                if (l < b && Compare(lv = array[l], max) > 0) { maxpt = l; max = lv; }

                if (r < b && Compare(rv = array[r], max) > 0) { maxpt = r; max = rv; }

                if (maxpt == j)
                    break;

                array[j] = max;
                max = pv;
                j = maxpt;
            }

            if (j > i)
                array[j] = pv;
        }
    }


}

