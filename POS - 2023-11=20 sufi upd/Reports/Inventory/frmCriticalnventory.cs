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
    public partial class frmCriticalnventory : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmCriticalnventory()
        {
            InitializeComponent();
        }
       
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet dsi = new DataSet();
                string q = "select id,itemname from rawitem";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["itemname"] = "All";
                cmbbranch.DataSource = dsi.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "itemname";
                cmbbranch.Text = "All";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();
                POSRestaurant.Reports.Inventory.rptCritical rptDoc = new rptCritical();
                POSRestaurant.Reports.Inventory.dsdiscard dsrpt = new dsdiscard();
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
                dtrpt.Columns.Add("Discard", typeof(double));
                dtrpt.Columns.Add("Remaining", typeof(double));               
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
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     dbo.CriticalInventory.quantity AS quantity, dbo.CriticalInventory.date, dbo.RawItem.ItemName, dbo.CriticalInventory.Actual AS Actual, dbo.CriticalInventory.Remaining AS Remaining FROM         dbo.RawItem INNER JOIN                      dbo.CriticalInventory ON dbo.RawItem.Id = dbo.CriticalInventory.itemid  WHERE     (dbo.CriticalInventory.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') order by dbo.CriticalInventory.date, dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT     dbo.CriticalInventory.quantity AS quantity, dbo.CriticalInventory.date, dbo.RawItem.ItemName, dbo.CriticalInventory.Actual AS Actual, dbo.CriticalInventory.Remaining AS Remaining FROM         dbo.RawItem INNER JOIN                      dbo.CriticalInventory ON dbo.RawItem.Id = dbo.CriticalInventory.itemid  WHERE     (dbo.CriticalInventory.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.CriticalInventory.itemid='" + cmbbranch.SelectedValue + "' order by dbo.CriticalInventory.date, dbo.RawItem.ItemName";
             
                }
                DataSet dsdate = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["quantity"].ToString(), ds.Tables[0].Rows[i]["Actual"].ToString(), ds.Tables[0].Rows[i]["Remaining"].ToString(), null, Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()));
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["quantity"].ToString(), ds.Tables[0].Rows[i]["Actual"].ToString(), ds.Tables[0].Rows[i]["Remaining"].ToString(), dscompany.Tables[0].Rows[0]["logo"], Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()));
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
