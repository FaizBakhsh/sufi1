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
    public partial class frmReceivedInventory : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmReceivedInventory()
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
                    dsfill.Tables[0].Rows.Add(dr);
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
                ds.Tables[0].Rows.Add(dr);
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


                POSRetail.Reports.Inventory.rptReceivedInventory rptDoc = new rptReceivedInventory();
                POSRetail.Reports.Inventory.DsInvReceivedreport dsrpt = new DsInvReceivedreport();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                
                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
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
            try
            {
                dtrpt.Columns.Add("ItemCode", typeof(string));
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("Brand", typeof(string));
                dtrpt.Columns.Add("Color", typeof(string));
                dtrpt.Columns.Add("Size", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("Location", typeof(string));
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("Pdate", typeof(string));
                DataSet ds = new DataSet();
                string q = "";
                if (cmbbranch.Text == "All")
                {
                    if (textBox1.Text.Trim() == string.Empty)
                    {
                        q = "SELECT      dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption, dbo.Purchase.Date FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption, dbo.Purchase.Date ORDER BY dbo.Purchase.Date";

                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT      dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption, dbo.Purchase.Date FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE   dbo.Purchase.Id = '" + textBox1.Text.Trim().Replace("GRN-", "") + "' and  (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption, dbo.Purchase.Date ORDER BY dbo.Purchase.Date";
                        }
                        else
                        {
                            q = "SELECT      dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption, dbo.Purchase.Date FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE  (dbo.RawItem.ItemName like '%" + textBox1.Text.Trim() + "%' or dbo.Branch.BranchName like '%" + textBox1.Text.Trim() + "%' or  dbo.Stores.StoreName like '%" + textBox1.Text.Trim() + "%' or dbo.Brands.BrandName like '%" + textBox1.Text.Trim() + "%' or  dbo.Size.SizeName like '%" + textBox1.Text.Trim() + "%' or  dbo.Color.Caption like '%" + textBox1.Text.Trim() + "%') and  (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption, dbo.Purchase.Date ORDER BY dbo.Purchase.Date";
                        }
                    }
                    //q = "SELECT     dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption";
                }
               else
                {
                    if (textBox1.Text.Trim() == string.Empty)
                    {
                        q = "SELECT      dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption, dbo.Purchase.Date FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.RawItem.BranchId = '" + cmbbranch.SelectedValue + "') GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption, dbo.Purchase.Date ORDER BY dbo.Purchase.Date";
                  
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT      dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption, dbo.Purchase.Date FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE   dbo.Purchase.Id = '" + textBox1.Text.Trim().Replace("GRN-","") + "' and  (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.RawItem.BranchId = '" + cmbbranch.SelectedValue + "') AND (dbo.RawItem.storeId = '" + cmbstore.SelectedValue + "')  GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption, dbo.Purchase.Date ORDER BY dbo.Purchase.Date";
                        }
                        else
                        {
                            q = "SELECT      dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption, dbo.Purchase.Date FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE (dbo.RawItem.ItemName like '%" + textBox1.Text.Trim() + "%' or dbo.Branch.BranchName like '%" + textBox1.Text.Trim() + "%' or  dbo.Stores.StoreName like '%" + textBox1.Text.Trim() + "%' or dbo.Brands.BrandName like '%" + textBox1.Text.Trim() + "%' or  dbo.Size.SizeName like '%" + textBox1.Text.Trim() + "%' or  dbo.Color.Caption like '%" + textBox1.Text.Trim() + "%') and    (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.RawItem.BranchId = '" + cmbbranch.SelectedValue + "') AND (dbo.RawItem.storeId = '" + cmbstore.SelectedValue + "') GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption, dbo.Purchase.Date ORDER BY dbo.Purchase.Date";
                        }
                    }
                   // q = "SELECT      dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption, dbo.Purchase.Date FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.RawItem.BranchId = '" + cmbbranch.SelectedValue + "') GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption, dbo.Purchase.Date ORDER BY dbo.Purchase.Date";
                    //q = "SELECT     dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity,                       SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Brands.BrandName, dbo.Size.SizeName, dbo.Color.Caption FROM         dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (RawItem.StoreId = '" + cmbstore.SelectedValue + "') AND (RawItem.BranchId = '" + cmbbranch.SelectedValue + "') GROUP BY dbo.RawItem.Code, dbo.RawItem.Description, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.UOM.UOM, dbo.RawItem.ItemName, dbo.Brands.BrandName, dbo.Size.SizeName,                       dbo.Color.Caption";
         
                }

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Code"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["BrandName"].ToString(), ds.Tables[0].Rows[i]["Caption"].ToString(), ds.Tables[0].Rows[i]["SizeName"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["Amount"].ToString(), ds.Tables[0].Rows[i]["BranchName"].ToString() + ", " + ds.Tables[0].Rows[i]["StoreName"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text,ds.Tables[0].Rows[i]["Date"].ToString());
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
