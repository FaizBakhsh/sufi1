using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports
{
    public partial class frmpictest : Form
    {
        public frmpictest()
        {
            InitializeComponent();
        }

        private void frmpictest_Load(object sender, EventArgs e)
        {
            bindreport();
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRetail.Reports.crpPic rptDoc = new crpPic();
                POSRetail.Reports.dspictest dsrpt = new dspictest();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();

                dsrpt.Tables[0].Merge(dt);
                //dsrpt.Tables[0].Merge(dt); ;
                rptDoc.SetDataSource(dsrpt);
                
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("pic", typeof(byte[]));

              

                string q = "";
               
                {
                    q = "SELECT Name,logo from CompanyInfo";

                }
                double tamount = 0;
                dsinfo = objCore.funGetDataSet(q);
                for (int j = 0; j < dsinfo.Tables[0].Rows.Count; j++)
                {


                    dtrpt.Rows.Add(dsinfo.Tables[0].Rows[j]["name"].ToString(), dsinfo.Tables[0].Rows[j]["logo"]);
                }
                //dtrpt = dsinfo.Tables[0];

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return dtrpt;// dsinfo.Tables[0];
        }
    }
}
