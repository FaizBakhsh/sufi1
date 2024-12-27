using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports
{
    public partial class testrdlc : Form
    {
        public testrdlc()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void testrdlc_Load(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.DsMenuItem dsrpt = new  SaleReports.DsMenuItem();



            //dsrpt.Tables[0].Columns.Add("MenuItem", typeof(string));
            //dsrpt.Tables[0].Columns.Add("Count", typeof(double));
            //dsrpt.Tables[0].Columns.Add("Sum", typeof(double));
            //dsrpt.Tables[0].Columns.Add("Date", typeof(string));
            //dsrpt.Tables[0].Columns.Add("logo", typeof(byte[]));
            //dsrpt.Tables[0].Columns.Add("GST", typeof(double));
            //dsrpt.Tables[0].Columns.Add("Net", typeof(double));
            //dsrpt.Tables[0].Columns.Add("dis", typeof(double));
            //dsrpt.Tables[0].Columns.Add("size", typeof(string));
            //dsrpt.Tables[0].Columns.Add("price", typeof(double));
            getcompany();
            dsrpt.Tables[0].Rows.Add("", "0", "0", "", dscompany.Tables[0].Rows[0]["logo"], "0", "0");
            ReportDataSource datasource = new ReportDataSource("DsMenuItem", dsrpt.Tables[0]);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(datasource);
            reportViewer1.LocalReport.Refresh();


           
            //this.reportViewer1.RefreshReport();
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
    }
}
