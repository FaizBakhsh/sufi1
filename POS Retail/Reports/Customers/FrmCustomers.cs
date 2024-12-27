using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.Customers
{
    public partial class FrmCustomers : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmCustomers()
        {
            InitializeComponent();
        }

        private void FrmCustomers_Load(object sender, EventArgs e)
        {
           
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


                POSRetail.Reports.Customers.rptCustomers rptDoc = new  rptCustomers();
                POSRetail.Reports.Customers.dscustomers dsrpt = new  dscustomers();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();

                dsrpt.Tables[0].Merge(dt);
                //dsrpt.Tables[0].Merge(dt); ;
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
            DataSet dsinfo = new DataSet();
            try
            {
                getcompany();
                string  logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                    
                }
                catch (Exception ex)
                {


                }
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Email", typeof(string));

                dtrpt.Columns.Add("Phone", typeof(string));
                dtrpt.Columns.Add("Mobile", typeof(string));
                dtrpt.Columns.Add("city", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                string q = "";
                q = "SELECT      dbo.CustomerInfo.Name, dbo.CustomerInfo.Email, dbo.CustomerInfo.Phone, dbo.CustomerInfo.Mobile, dbo.CustomerInfo.City, dbo.CompanyInfo.logo FROM         dbo.CustomerInfo CROSS JOIN                      dbo.CompanyInfo";

                dsinfo = objCore.funGetDataSet(q);
                
                dtrpt = dsinfo.Tables[0];

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return dtrpt;// dsinfo.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
