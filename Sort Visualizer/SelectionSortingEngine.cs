using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort_Visualizer
{
    internal class SelectionSortingEngine : ISortingEngine
    {

        private int[] TheArray;
        private Graphics g;
        private int MaxValue;
        // Brush RedPaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
        Brush WhitePaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
        Brush BlackPaintBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);

        public SelectionSortingEngine(int[] TheArray_Clone, Graphics g_clone, int MaxValue_Clone) // constructor 
        {
            TheArray = TheArray_Clone;
            g = g_clone;
            MaxValue = MaxValue_Clone;
        }

        public bool isSorted()
        {
            for (int i = 0; i < TheArray.Count() - 1; i++)
            {
                if (TheArray[i] > TheArray[i + 1]) return false;
            }
            return true;
        }

        public void NextStep()
        {
            for (int i = 0; i < TheArray.Count(); i++)
            {
                int min = FindMin(TheArray, i);
                int temp = TheArray[i];
                TheArray[i] = TheArray[min];
                DrawBar(i, TheArray[min]);
                System.Threading.Thread.Sleep(1);
                TheArray[min] = temp;
                DrawBar(min, temp);
                System.Threading.Thread.Sleep(1);
            }
        }

        private int FindMin(int[] theArray, int a)
        {
            int index = a;
            int value = theArray[a];
            for (int i = a + 1; i < theArray.Count(); i++)
            {
                if (theArray[a] < value)
                {
                    index = i;
                    value = theArray[i];
                }

            }
            return index;
        }

        private void DrawBar(int position, int height)
        {
            g.FillRectangle(BlackPaintBrush, position, 0, 1, MaxValue);
            // g.FillRectangle(RedPaintBrush, position, MaxValue - TheArray[position], 1, MaxValue);
            g.FillRectangle(WhitePaintBrush, position, MaxValue - TheArray[position], 1, MaxValue);
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
