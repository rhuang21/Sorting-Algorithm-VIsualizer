using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort_Visualizer
{
    internal class MergeSortingEngine : ISortingEngine
    {
        private int[] TheArray;
        private Graphics g;
        private int MaxValue;
        Brush WhitePaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        Brush BlackPaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        private int CurrentPointer = 0;
        public MergeSortingEngine(int[] TheArray_Clone, Graphics g_clone, int MaxValue_Clone) // constructor 
        {
            TheArray = TheArray_Clone;
            g = g_clone;
            MaxValue = MaxValue_Clone;
        }

        public void NextStep() // was DoSorting
        {
            mergeSort(TheArray, 0, TheArray.Count() - 1);
        }

        private void mergeSort(int[] theArray, int start, int end)
        {
            if (start < end)
            {
                int middle = (start + end) / 2;
                mergeSort(theArray, start, middle);
                mergeSort(TheArray, middle + 1, end);

                Merge(TheArray, start, middle, end);
            }
        }

        private void Merge(int[] theArray, int start, int middle, int end)
        {
            int[] leftArray = new int[middle - start + 1];
            int[] rightArray = new int[end - middle];

            Array.Copy(theArray, start, leftArray, 0, middle - start + 1);
            Array.Copy(theArray, middle + 1, rightArray, 0, end - middle);

            int i = 0;
            int j = 0;
            for (int k = start; k < end + 1; k++)
            {
                if (i == leftArray.Length)
                {
                    theArray[k] = rightArray[j];
                    DrawBar(k, theArray[k]);
                    System.Threading.Thread.Sleep(1);
                    j++;
                }
                else if (j == rightArray.Length)
                {
                    theArray[k] = leftArray[i];
                    DrawBar(k, theArray[k]);
                    System.Threading.Thread.Sleep(1);
                    i++;
                }
                else if (leftArray[i] <= rightArray[j])
                {
                    theArray[k] = leftArray[i];
                    DrawBar(k, theArray[k]);
                    System.Threading.Thread.Sleep(1);
                    i++;
                }
                else
                {
                    theArray[k] = rightArray[j];
                    DrawBar(k, theArray[k]);
                    System.Threading.Thread.Sleep(1);
                    j++;
                }
            }
        }

        // sends the value all the way to the back and shift the whole array to the left 
        private void Rotate(int currentPointer)
        {
            int temp = TheArray[currentPointer];
            int arrayEnd = TheArray.Count() - 1;

            for (int i = currentPointer; i < arrayEnd; i++)
            {
                TheArray[i] = TheArray[i + 1];
                DrawBar(i, TheArray[i]);
            }

            TheArray[arrayEnd] = temp; 
            DrawBar(arrayEnd, TheArray[arrayEnd]);
        }

        private void DrawBar(int position, int height)
        {
            // remove old values from display
            g.FillRectangle(BlackPaintBrush, position, 0, 1, MaxValue);
            // display new values after swapping 
            g.FillRectangle(WhitePaintBrush, position, MaxValue - TheArray[position], 1, MaxValue);
        }

        public bool isSorted()  // public so it can be checked by the caller 
        {
            for (int i = 0; i < TheArray.Count() - 1; i++)
            {
                if (TheArray[i] > TheArray[i + 1]) return false;
            }
            return true;

        }

        public void ReDraw()
        {
            for (int i = 0; i < (TheArray.Count() - 1); i++)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, MaxValue - TheArray[i], 1, MaxValue);
            }
        }

        public void completeSort()
        {
            for (int i = 0; i < (TheArray.Count() - 1); i++)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Green), i, MaxValue - TheArray[i], 1, MaxValue);
                System.Threading.Thread.Sleep(1);
            }
        }
    }
}
