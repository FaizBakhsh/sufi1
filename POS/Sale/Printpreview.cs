using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class Printpreview : Form
    {
       public POSRestaurant.Reports.CashReceipt rptDoc = new Reports.CashReceipt();
                      
        public Printpreview()
        {
            InitializeComponent();
        }

        private void Printpreview_Load(object sender, EventArgs e)
        {
            crystalReportViewer1.ReportSource = rptDoc;
        }

    }
}
