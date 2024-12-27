using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Inventory
{
    public partial class frmVariance : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmVariance()
        {
            InitializeComponent();
        }
       
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
           
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.Inventory.rptVariance rptDoc = new rptVariance();
                POSRestaurant.Reports.Inventory.dsVariance dsrpt = new dsVariance();
                //feereport ds = new feereport(); // .xsd file name

                getcompany();
                dt = getAllOrders();
                // Just set the name of data table
                dt.TableName = "Crystal Report";
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

                        dt.Rows.Add("0", "0", "0", "0", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", " For the period of " + dateTimePicker1.Text+" to "+dateTimePicker2.Text);


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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("PhysicalQuantity", typeof(double));
                dtrpt.Columns.Add("Variance", typeof(double));               
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("date", typeof(DateTime));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }

                DataSet ds = new DataSet();
                string q = "", date = "" ;
                q = "SELECT     dbo.RawItem.ItemName,dbo.Variance.Date, SUM(dbo.Variance.quantity) AS Closing, SUM(dbo.Variance.physical) AS physical, SUM(dbo.Variance.variance) AS variance FROM         dbo.RawItem INNER JOIN                      dbo.Variance ON dbo.RawItem.Id = dbo.Variance.itemid WHERE     (dbo.Variance.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.ItemName,dbo.Variance.Date ";
                DataSet dsdate = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Closing"].ToString(), ds.Tables[0].Rows[i]["physical"].ToString(), ds.Tables[0].Rows[i]["variance"].ToString(), null, Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()));
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Closing"].ToString(), ds.Tables[0].Rows[i]["physical"].ToString(), ds.Tables[0].Rows[i]["variance"].ToString(), dscompany.Tables[0].Rows[0]["logo"], Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()));
                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
