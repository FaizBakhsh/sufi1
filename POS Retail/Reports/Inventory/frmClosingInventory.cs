using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
namespace POSRetail.Reports.Inventory
{
    public partial class frmClosingInventory : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmClosingInventory()
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
                    DataRow dr = dsfill.Tables[0].NewRow();
                    dr["storename"] = "All";
                    cmbstore.DataSource = dsfill.Tables[0];
                    cmbstore.ValueMember = "id";
                    cmbstore.DisplayMember = "storename";
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


                POSRetail.Reports.Inventory.rptClosingInventory rptDoc = new rptClosingInventory();
                POSRetail.Reports.Inventory.dsInventoryClosing dsrpt = new  dsInventoryClosing();
                //feereport ds = new feereport(); // .xsd file name
                getcompany();
                string company = "", phone = "", address = "",logo="";
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

                        dt.Rows.Add("", "", "", "", "0", "","", dscompany.Tables[0].Rows[0]["logo"]);


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
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("Location", typeof(string));
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));

                DataSet ds = new DataSet();
                string q = "";
                if (cmbbranch.Text == "All")
                {

                   // q = "SELECT     dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.Inventory.Quantity, dbo.Inventory.UploadStatus,                       dbo.Inventory.RawItemId, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.Brands.BrandName FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id RIGHT OUTER JOIN                      dbo.Inventory ON dbo.RawItem.Id = dbo.Inventory.RawItemId LEFT OUTER JOIN                      dbo.InventoryConsumed ON dbo.Inventory.RawItemId = dbo.InventoryConsumed.RawItemId WHERE     (InventoryConsumed.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' OR InventoryConsumed.Date IS NULL)";
                    if (textBox1.Text.Trim() == string.Empty)
                    {
                        q = "SELECT    sum( dbo.Inventory.Quantity) as Quantity, dbo.Inventory.UploadStatus, dbo.Inventory.RawItemId, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName,                       dbo.Brands.BrandName FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id RIGHT OUTER JOIN                      dbo.Inventory ON dbo.RawItem.Id = dbo.Inventory.RawItemId GROUP BY Inventory.UploadStatus, Inventory.RawItemId, RawItem.Code, RawItem.ItemName, Branch.BranchName, Stores.StoreName, Brands.BrandName";
                    }
                    else
                    {
                        q = "SELECT    sum( dbo.Inventory.Quantity) as Quantity, dbo.Inventory.UploadStatus, dbo.Inventory.RawItemId, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName,                       dbo.Brands.BrandName FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id RIGHT OUTER JOIN                      dbo.Inventory ON dbo.RawItem.Id = dbo.Inventory.RawItemId where dbo.RawItem.ItemName like '%" + textBox1.Text + "%' GROUP BY Inventory.UploadStatus, Inventory.RawItemId, RawItem.Code, RawItem.ItemName, Branch.BranchName, Stores.StoreName, Brands.BrandName  ";
               
                    }
                }
               else
                {
                    if (textBox1.Text.Trim() == string.Empty)
                    {
                        q = "SELECT    sum( dbo.Inventory.Quantity) as Quantity, dbo.Inventory.UploadStatus, dbo.Inventory.RawItemId, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName,                       dbo.Brands.BrandName FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id RIGHT OUTER JOIN                      dbo.Inventory ON dbo.RawItem.Id = dbo.Inventory.RawItemId  where   (RawItem.StoreId = '" + cmbstore.SelectedValue + "') AND (RawItem.BranchId = '" + cmbbranch.SelectedValue + "') GROUP BY Inventory.UploadStatus, Inventory.RawItemId, RawItem.Code, RawItem.ItemName, Branch.BranchName, Stores.StoreName, Brands.BrandName ";
                    }
                    else
                    {
                        q = "SELECT    sum( dbo.Inventory.Quantity) as Quantity, dbo.Inventory.UploadStatus, dbo.Inventory.RawItemId, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName,                       dbo.Brands.BrandName FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id RIGHT OUTER JOIN                      dbo.Inventory ON dbo.RawItem.Id = dbo.Inventory.RawItemId  where  dbo.RawItem.ItemName like'%" + textBox1.Text + "%' and (RawItem.StoreId = '" + cmbstore.SelectedValue + "') AND (RawItem.BranchId = '" + cmbbranch.SelectedValue + "') GROUP BY Inventory.UploadStatus, Inventory.RawItemId, RawItem.Code, RawItem.ItemName, Branch.BranchName, Stores.StoreName, Brands.BrandName ";
           
                    }
                   // q = "SELECT     dbo.Inventory.Quantity, dbo.Inventory.UploadStatus, dbo.Inventory.RawItemId, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName,                       dbo.Brands.BrandName FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id RIGHT OUTER JOIN                      dbo.Inventory ON dbo.RawItem.Id = dbo.Inventory.RawItemId where (RawItem.StoreId = '" + cmbstore.SelectedValue + "') AND (RawItem.BranchId = '" + cmbbranch.SelectedValue + "') ";
           
                }
                getcompany();
                string  logo = "";
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
                    double amount = 0;
                    DataSet dsi = new DataSet();
                    dsi = objCore.funGetDataSet("SELECT      SUM(TotalAmount) / SUM(TotalItems) AS Amount FROM         PurchaseDetails where RawItemId='" + ds.Tables[0].Rows[i]["RawItemId"].ToString() + "'");
                    if (dsi.Tables[0].Rows.Count > 0)
                    {
                        string val = dsi.Tables[0].Rows[0][0].ToString();
                        if (val == string.Empty)
                        {
                            val = "0";
                        }
                        amount = Convert.ToDouble(val);
                        amount = Math.Round(amount, 2);
                    }
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Code"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["BrandName"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), amount, ds.Tables[0].Rows[i]["BranchName"].ToString() + ", " + ds.Tables[0].Rows[i]["StoreName"].ToString(), "", null);

                    }
                    else
                    {

                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Code"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["BrandName"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), amount, ds.Tables[0].Rows[i]["BranchName"].ToString() + ", " + ds.Tables[0].Rows[i]["StoreName"].ToString(), "", dscompany.Tables[0].Rows[0]["logo"]);




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
