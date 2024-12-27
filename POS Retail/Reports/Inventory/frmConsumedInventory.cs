using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.Inventory
{
    public partial class frmConsumedInventory : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmConsumedInventory()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                cmbstore.DataSource = null;
                cmbstore.Items.Clear();
                
                if (cmbbranch.Text == "All")
                {
                    cmbstore.Items.Add("All");
                }
                else
                {
                    DataSet dsfill = new DataSet();
                    string q = "select * from Stores where BranchId='" + cmbbranch.SelectedValue + "'";
                    dsfill = objCore.funGetDataSet(q);
                    DataRow drr = dsfill.Tables[0].NewRow();
                    drr["Storename"] = "All";
                    cmbstore.DataSource = dsfill.Tables[0];
                    cmbstore.ValueMember = "id";
                    cmbstore.DisplayMember = "Storename";
                    cmbstore.Text = "All";
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select * from branch";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["branchname"] = "All";
                cmbbranch.DataSource = ds.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";
                cmbbranch.Text = "All";

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill();
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


                POSRetail.Reports.Inventory.rptComsumedinventory rptDoc = new rptComsumedinventory();
                POSRetail.Reports.Inventory.DsInventoryCosumption dsrpt = new  DsInventoryCosumption();
                //feereport ds = new feereport(); // .xsd file name

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

                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
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

                        dt.Rows.Add("", "", "", "", "", "", "" , "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);


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
                dtrpt.Columns.Add("ItemCode", typeof(string));
                dtrpt.Columns.Add("ItemDescription", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Consumed", typeof(string));
                dtrpt.Columns.Add("Remaining", typeof(string));
                dtrpt.Columns.Add("Location", typeof(string));
                dtrpt.Columns.Add("date1", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));


                DataSet ds = new DataSet();
                string q = "";
                if (cmbbranch.Text == "All")
                {
                    if (textBox1.Text.Trim()==string.Empty)
                    {
                        q = "SELECT     dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName,                       dbo.Stores.StoreName, dbo.Brands.BrandName FROM         dbo.InventoryConsumed INNER JOIN                      dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Branch ON dbo.RawItem.BranchId = dbo.Branch.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id WHERE     (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT     dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName,                       dbo.Stores.StoreName, dbo.Brands.BrandName FROM         dbo.InventoryConsumed INNER JOIN                      dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Branch ON dbo.RawItem.BranchId = dbo.Branch.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id WHERE   dbo.RawItem.ItemName like '%" + textBox1.Text + "%' and  (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                  
                    }
                }
               else
                {
                    if (textBox1.Text.Trim() == string.Empty)
                    {
                        q = "SELECT     dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName,                       dbo.Stores.StoreName, dbo.Brands.BrandName FROM         dbo.InventoryConsumed INNER JOIN                      dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Branch ON dbo.RawItem.BranchId = dbo.Branch.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id WHERE     (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.StoreId ='" + cmbstore.SelectedValue + "' and dbo.RawItem.BranchId='" + cmbbranch.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT     dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName,                       dbo.Stores.StoreName, dbo.Brands.BrandName FROM         dbo.InventoryConsumed INNER JOIN                      dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Branch ON dbo.RawItem.BranchId = dbo.Branch.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id WHERE   dbo.RawItem.ItemName like '%" + textBox1.Text + "%' and  (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.StoreId ='" + cmbstore.SelectedValue + "' and dbo.RawItem.BranchId='" + cmbbranch.SelectedValue + "'";
            
                    }
                    //q = "SELECT     dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName,                       dbo.Stores.StoreName, dbo.Brands.BrandName FROM         dbo.InventoryConsumed INNER JOIN                      dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Branch ON dbo.RawItem.BranchId = dbo.Branch.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id WHERE     (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.StoreId ='" + cmbstore.SelectedValue + "' and dbo.RawItem.BranchId='" + cmbbranch.SelectedValue + "'";
            
                }
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Code"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["BrandName"].ToString(), ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["QuantityConsumed"].ToString(), ds.Tables[0].Rows[i]["RemainingQuantity"].ToString(), ds.Tables[0].Rows[i]["BranchName"].ToString() + "," + ds.Tables[0].Rows[i]["StoreName"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);

                    }
                    else
                    {

                        
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Code"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["BrandName"].ToString(), ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["QuantityConsumed"].ToString(), ds.Tables[0].Rows[i]["RemainingQuantity"].ToString(), ds.Tables[0].Rows[i]["BranchName"].ToString() + "," + ds.Tables[0].Rows[i]["StoreName"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);



                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
