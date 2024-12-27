using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Members
{
    public partial class frmMembers : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmMembers()
        {
            InitializeComponent();
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


                POSRestaurant.Reports.Members.rptMembers rptDoc = new rptMembers();
                POSRestaurant.Reports.Members.dsmembers dsrpt = new  dsmembers();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
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

                        dt.Rows.Add("", "", "", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



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
        
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Cnic", typeof(string));
                dtrpt.Columns.Add("Phone", typeof(string));
                dtrpt.Columns.Add("Company", typeof(string));
                dtrpt.Columns.Add("City", typeof(string));
                dtrpt.Columns.Add("Address", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                DataSet ds = new DataSet();
                string q = "";


                q = "SELECT     Id, Name, Cnic, Phone, Company, Msr, City, Address, Uploadstatus FROM         Customers";
                
                
                ds = objCore.funGetDataSet(q);
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Cnic"].ToString(), ds.Tables[0].Rows[i]["Phone"].ToString(), ds.Tables[0].Rows[i]["Company"].ToString(), ds.Tables[0].Rows[i]["City"].ToString(), ds.Tables[0].Rows[i]["Address"].ToString(),null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Cnic"].ToString(), ds.Tables[0].Rows[i]["Phone"].ToString(), ds.Tables[0].Rows[i]["Company"].ToString(), ds.Tables[0].Rows[i]["City"].ToString(), ds.Tables[0].Rows[i]["Address"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        private void frmMembers_Load(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
