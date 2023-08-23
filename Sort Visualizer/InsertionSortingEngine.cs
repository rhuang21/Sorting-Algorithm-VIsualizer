using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort_Visualizer
{
    internal class InsertionSortingEngine : ISortingEngine
    {
        private int[] TheArray;
        private Graphics g;
        private int MaxValue;
        Brush WhitePaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        Brush BlackPaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        
        public InsertionSortingEngine(int[] TheArray_Clone, Graphics g_clone, int MaxValue_Clone) // constructor 
        {
            TheArray = TheArray_Clone;
            g = g_clone;
            MaxValue = MaxValue_Clone;
        }

        public void NextStep()
        {
            for (int i = 1; i < TheArray.Length; ++i)
            {
                Shift(TheArray, i);
            }
        }

        private void Shift(int[] TheArray, int location)
        {
            int key = TheArray[location];
            int j = location;

            while (j > 0 && TheArray[j-1] > key)
            {
                TheArray[j] = TheArray[j-1];
                DrawBar(j, TheArray[j-1]);
                j = j - 1;
            }
            TheArray[j] = key;
            DrawBar(j, key);  
        }

        public void DrawBar(int position, int height)
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
