using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.CashTransactions
{
    public partial class frmCashTransaction : Form
    {
        public frmCashTransaction()
        {
            InitializeComponent();
        }
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRestaurant.Reports.CashTransactions.rptCashTransaction1 rptDoc = new POSRestaurant.Reports.CashTransactions.rptCashTransaction1();
                POSRestaurant.Reports.CashTransactions.dsCashTransaction dsrpt = new  dsCashTransaction();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                getcompany();
                string company = "", phone = "", address = "", logo = "";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                }
                catch (Exception ex)
                {


                }
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    if (logo == "")
                    { }
                    else
                    {

                        dt.Rows.Add("", "0", "0", "", "0",  dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("cashin", typeof(double));
                dtrpt.Columns.Add("cashout", typeof(double));
                dtrpt.Columns.Add("description", typeof(string));
                dtrpt.Columns.Add("OpeningBalance", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));

                DataSet ds = new DataSet();
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                string q = "";
                q = "SELECT    sum( netbill) as netbill FROM         Sale  WHERE     Date= '" + dateTimePicker1.Text + "'";

                DataSet dscash = new DataSet();
                dscash = objCore.funGetDataSet(q);
                double opblnce = 0;
                if (dscash.Tables[0].Rows.Count > 0)
                {
                    string val = dscash.Tables[0].Rows[0]["netbill"].ToString();
                    if (val == string.Empty)
                    {
                        val = "0";

                    }
                    opblnce = Convert.ToDouble(val);
                }
                q = "SELECT     id, date, cashin, cashout, Description FROM         CashTransactions  WHERE     Date= '" + dateTimePicker1.Text + "'";

                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {

                        dtrpt.Rows.Add("as on "+dateTimePicker1.Text, ds.Tables[0].Rows[i]["cashin"].ToString(), ds.Tables[0].Rows[i]["cashout"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(),opblnce, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add("as on " + dateTimePicker1.Text, ds.Tables[0].Rows[i]["cashin"].ToString(), ds.Tables[0].Rows[i]["cashout"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), opblnce, dscompany.Tables[0].Rows[0]["logo"]);


                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
