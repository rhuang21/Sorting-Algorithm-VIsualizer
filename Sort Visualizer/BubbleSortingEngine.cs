using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Sort_Visualizer
{
    internal class BubbleSortingEngine : ISortingEngine
    {
        private int[] TheArray;
        private Graphics g;
        private int MaxValue;
        Brush WhitePaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        Brush BlackPaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        public BubbleSortingEngine(int[] TheArray_Clone, Graphics g_clone, int MaxValue_Clone) // constructor 
        {
            TheArray = TheArray_Clone;
            g = g_clone;
            MaxValue = MaxValue_Clone;
        }

        public void NextStep() // was DoSorting
        {
            for (int i = 0; i < TheArray.Count() - 1; i++)
            {
                if (TheArray[i] > TheArray[i + 1])
                {
                    Swap(i, i + 1);
                }
            }
        }

        private void Swap(int i, int v)
        {
            int temp = TheArray[i];
            TheArray[i] = TheArray[i+1];
            TheArray[i+1] = temp;

            // update display on the screen 

            DrawBar(i, TheArray[i]);
            DrawBar(v, TheArray[v]);
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
            for (int i = 0; i < TheArray.Count() - 1; i++) {
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
