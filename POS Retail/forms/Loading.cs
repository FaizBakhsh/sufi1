using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.forms
{
    public partial class Loading : Form
    {
        public Loading()
        {
            InitializeComponent();
        }

        private void Loading_Load(object sender, EventArgs e)
        {
            try
            {
               
                    POSRetail.Reports.Initilized rptDoc = new POSRetail.Reports.Initilized();

                   

                    rptDoc.PrintToPrinter(1, false, 0, 0);

            }
            catch (Exception ex)
            {
                
                
            }
            for (int i = 0; i <= 100; i++)
            {
                
            }
            login1 obj = new login1();
            obj.Show();
        }
    }
}
