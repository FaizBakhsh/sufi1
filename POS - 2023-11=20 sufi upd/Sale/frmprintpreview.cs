using Microsoft.Reporting.WinForms;
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
    public partial class frmprintpreview : Form
    {
        public frmprintpreview()
        {
            InitializeComponent();
        }
       public LocalReport report;public DataTable dt ;public ReportParameter rurl; public ReportParameter rcomp; public ReportParameter rphone; public ReportParameter raddress; public ReportParameter rbillno; public ReportParameter rcashier; public ReportParameter rmop; public ReportParameter rterminal; public ReportParameter rdate

            ; public ReportParameter rordertype; public ReportParameter rDelivery; public ReportParameter rsubtotal; public ReportParameter rfooter; public ReportParameter rtender; public ReportParameter rQrTitle; public ReportParameter rFbrcode; public ReportParameter rsubtotalcard; public ReportParameter rtendercard; public ReportParameter rcharity; public ReportParameter fbruri; public ReportParameter rqrmenu;

        public void fillreport(LocalReport report, DataTable dt, ReportParameter rurl, ReportParameter rcomp, ReportParameter rphone, ReportParameter raddress, ReportParameter rbillno, ReportParameter rcashier, ReportParameter rmop, ReportParameter rterminal, ReportParameter rdate

            , ReportParameter rordertype, ReportParameter rDelivery, ReportParameter rsubtotal, ReportParameter rfooter, ReportParameter rtender, ReportParameter rQrTitle, ReportParameter rFbrcode, ReportParameter rsubtotalcard, ReportParameter rtendercard, ReportParameter rcharity, ReportParameter fbruri)
        {
            reportViewer1.LocalReport.ReportPath = report.ReportPath; ;
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("dstest", dt));
            this.reportViewer1.LocalReport.EnableExternalImages = true;
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rurl, rcomp, rphone, raddress, rbillno, rcashier, rmop, rterminal, rdate, rordertype, rDelivery, rsubtotal, rfooter, rtender, rQrTitle, rFbrcode, rsubtotalcard, rtendercard, rcharity, fbruri,rqrmenu });
            
            
            
            this.reportViewer1.RefreshReport();
        }
        private void frmprintpreview_Load(object sender, EventArgs e)
        {
            fillreport(report, dt, rurl, rcomp, rphone,  raddress,  rbillno,  rcashier,  rmop,  rterminal,  rdate,  rordertype,  rDelivery,  rsubtotal,  rfooter,  rtender,  rQrTitle,  rFbrcode,  rsubtotalcard,  rtendercard,  rcharity,  fbruri);
            this.reportViewer1.RefreshReport();
         
        }
    }
}
