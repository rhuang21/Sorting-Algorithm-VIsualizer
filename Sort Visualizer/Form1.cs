using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Sort_Visualizer
{
    // current version uses the same thread to run the application which locks up the program when it is running, sorting on seperate thread is needed 
    // optimizing the graphics portion is needed 
    public partial class btnReset : Form
    {
        int[] TheArray;
        Graphics g;
        BackgroundWorker bworker = null; // entity that runs the sorting algo on another thread 
        bool Paused = false;    // pause flag 
        public btnReset()
        {
            InitializeComponent();
            PopulateDropDown();
        }

        private void PopulateDropDown()  
        {
            // asks program to look through internal structure to find all classes that implement ISortingEngine interface 
            List<String> SortingList = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()) // get assemblies that are implementations of the isortingengine interface
                .Where(x => typeof(ISortingEngine).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)  // exclude interface itself and abstract classes 
                .Select(x => x.Name).ToList(); // names cast to a list and append to sortingList 
            SortingList.Sort(); 
            foreach (String entry in SortingList) {
                comboBox1.Items.Add(entry); // populate dropdown with names in alphabetical order 
            }
            comboBox1.SelectedIndex = 0;    // default to first item
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // if start is clicked before reset, there is an exception
            if (TheArray == null) button1_Click(null, null); // auto invoke reset button click is array is null, so before the reset is click 

            bworker = new BackgroundWorker(); // whenever we click sort, we init a new background worker to run algo on a new thread 
            bworker.WorkerSupportsCancellation = true;  // allows to cancel worker task 
            bworker.DoWork += new DoWorkEventHandler(bworker_DoWork); // add event handler with += to DoWork event instead of = (adding to a list of things when the event fires)
            bworker.RunWorkerAsync(argument: comboBox1.SelectedItem); // run backgroundworker and pass in selected algo 
        }

        private void btnPauseResume_Click(object sender, EventArgs e)
        {
            if (!Paused)
            {
                bworker.CancelAsync(); // send a signal to backgroundworker to cancel the thread. this doesnt happen immediately causes a time lag / race condition 
                Paused = true;  // flag 
            }
            else
            {
                if (bworker.IsBusy) return;  // this will prevent the race condition when the button is clicked but worker is busy 
                int NumberOfEntries = panel1.Width;
                int MaxValue = panel1.Height;
                Paused = false;
                for (int i = 0; i < NumberOfEntries; i++)
                {
                    // refresh display if it was disrupted before we move on 
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), i, 0, 1, MaxValue);
                    g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, MaxValue - TheArray[i], 1, MaxValue);
                }
                bworker.RunWorkerAsync(argument: comboBox1.SelectedItem); // run a new backgroundworker and picks up where last one left off with chosen algorithm 
            }
        }
        // reset button 
        private void button1_Click(object sender, EventArgs e)
        {
            g = panel1.CreateGraphics();
            int NumberOfEntries = panel1.Width;
            int MaxValue = panel1.Height;
            TheArray = new int[NumberOfEntries];
            g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), 0, 0, NumberOfEntries, MaxValue); // initialize color of background panel to black
            Random RandomNumGen = new Random(); // random number generator 
            for (int i = 0; i < NumberOfEntries; i++) {
                TheArray[i] = RandomNumGen.Next(0, MaxValue);  // populate the array with random num gen 
            }
            for (int i = 0;i < NumberOfEntries; i++)
            {
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.White), i, MaxValue - TheArray[i], 1, MaxValue); // fill in bars, each integer will be represented by 1 pixel wide rectangle
            }
        }

        #region Background Work

        public void bworker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)  // sender is the one that raised the event/ invoked this method, second is a block of arguments to it and customized by DoWorkEventArgs (type of event handler it is)
        {
            BackgroundWorker backgroundWorker = sender as BackgroundWorker; // explicitly cast sender as a background worker, makes code editing easier with right context 
            string SortEngineName = (string)e.Argument; // extract sort engine name from bworker.RunWorkerAsync(argument: comboBox1.SelectedItem) 
            Type type = Type.GetType("Sort_Visualizer." +  SortEngineName); // figure out actual type using reflection, prepend name space Sort_Visualizer for identification
            var constructor = type.GetConstructors(); // get constructor of the sorting engine (eg bubble sort engine) with type of type from above 
            try
            {
                ISortingEngine se = (ISortingEngine)constructor[0].Invoke(new object[] { TheArray, g, panel1.Height }); // make sorting engine object with the taken type and invoke a general constructor of the given type 
                // ISortingEngine se2 = new BubbleSortingEngine(TheArray, g, panel1.Height);   this is what it would look like if we only had one sort algo
                while (!se.isSorted() && (!backgroundWorker.CancellationPending)) // list is not sorted and user has not clicked cancel 
                {
                    se.NextStep();

                }
                if (se.isSorted())
                {
                    se.completeSort();
                }

            }catch (Exception ex) { 
                
            }
                                                                         
        }


        #endregion

        
    }
}
