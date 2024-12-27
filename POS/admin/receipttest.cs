using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.admin
{
    public partial class receipttest : Form
    {
        public DataTable ds = new  DataTable();
        public receipttest()
        {
            InitializeComponent();
        }

        private void receipttest_Load(object sender, EventArgs e)
        {
            POSRestaurant.Reports.CashReceipt rptDoc = new Reports.CashReceipt();
            POSRestaurant.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "Crystal Report";
           
            //dsrpt.Tables[0].Merge(dt,false,MissingSchemaAction.Ignore);
            dsrpt.Tables[0].Merge(ds);

            rptDoc.SetDataSource(dsrpt);
            crystalReportViewer1.ReportSource = rptDoc;
        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            POSRestaurant.Reports.CashReceipt rptDoc = new Reports.CashReceipt();
            POSRestaurant.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "Crystal Report";

            //dsrpt.Tables[0].Merge(dt,false,MissingSchemaAction.Ignore);
            dsrpt.Tables[0].Merge(ds);

            rptDoc.SetDataSource(dsrpt);
            crystalReportViewer1.ReportSource = rptDoc;
        }
    }
}
