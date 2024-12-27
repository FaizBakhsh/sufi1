using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace POSRetail.Reports.Accounts
{
    public partial class frmaccounts : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmaccounts()
        {
            InitializeComponent();
        }

        private void frmaccounts_Load(object sender, EventArgs e)
        {
            bindreport();
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRetail.Reports.Accounts.rptAccounts rptDoc = new rptAccounts();
                POSRetail.Reports.Accounts.dsaccounts dsrpt = new dsaccounts();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                dsrpt.Tables[0].Merge(dt);


                rptDoc.SetDataSource(dsrpt);
                getcompany();
                string company = "", phone = "", address = "";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                }
                catch (Exception ex)
                {


                }
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
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("AccountType", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("AccountCode", typeof(string));
                dtrpt.Columns.Add("Status", typeof(string));
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));

                getcompany();
                string  logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                   
                }
                catch(Exception ex )
                {

                }
                DataSet ds = new DataSet();
                string q = "";
                ds = new DataSet();
                q = "SELECT     AccountType, Name, AccountCode, Description, Status FROM         ChartofAccounts order by AccountType";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountType"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Status"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(),null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountType"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Status"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }

               


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }

        private void crystalReportViewer1_Search(object source, CrystalDecisions.Windows.Forms.SearchEventArgs e)
        {

        }

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            bindreport();
        }

    }
}
