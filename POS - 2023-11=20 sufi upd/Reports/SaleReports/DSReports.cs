using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class DSReports : Form
    {
        public DSReports()
        {
            InitializeComponent();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmgroupDSsale obj = new POSRestaurant.Reports.SaleReports.FrmgroupDSsale();
            obj.Show();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmInvoiceSaleDS obj = new POSRestaurant.Reports.SaleReports.FrmInvoiceSaleDS();
            obj.Show();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmMenuItemDSSale obj = new POSRestaurant.Reports.SaleReports.FrmMenuItemDSSale();
            obj.Show();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.frmSalesDS obj = new POSRestaurant.Reports.SaleReports.frmSalesDS();
            obj.Show();
        }
    }
}
