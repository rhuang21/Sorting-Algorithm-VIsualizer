using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sort_Visualizer
{
    internal interface ISortingEngine
    {
        void NextStep(); // everytime its called it will take one step towards sorting the output
        bool isSorted(); // flag 
        void ReDraw(); // pause and resume and refresh graphics object 

        void completeSort();
    }
}
