using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class progressbar : Form
    {
        public progressbar()
        {
            InitializeComponent();
        } 
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public event EventHandler MyLongRunningTaskEvent;
        public string type = "";
        private void progressbar_Load(object sender, EventArgs e)
        {
            Thread thrd;
          
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;
            if (type == "Update Columns")
            {
                MyLongRunningTaskEvent += myLongRunningTaskIsDone;
                Thread _thread = new Thread(myLongRunningTask) { IsBackground = true };
                _thread.Start();
                //thrd = new Thread(() => objcore.addcolumns());
                //thrd.IsBackground = true;
                //thrd.Start();
            }
        }
        private void myLongRunningTask()
        {
            try
            {
                objcore.addcolumns();
            }
            finally
            {
               
            }
        }
        private void myLongRunningTaskIsDone(object sender, EventArgs arg)
        {
            MessageBox.Show("Updated");
        }
    }
}
