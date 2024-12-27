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
    public partial class frmReceivedInventoryBrand : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmReceivedInventoryBrand()
        {
            InitializeComponent();
        }
     
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select * from brands";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["BrandName"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbbrand.DataSource = ds.Tables[0];
                cmbbrand.ValueMember = "id";
                cmbbrand.DisplayMember = "BrandName";
                cmbbrand.Text = "All";

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
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


                POSRetail.Reports.Inventory.rprInventoryPurchaseBrand rptDoc = new rprInventoryPurchaseBrand();
                POSRetail.Reports.Inventory.DsInventoryReceived dsrpt = new DsInventoryReceived();
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

                        dt.Rows.Add("", DateTime.Now, "", "", "", "0", "0", "0", "0", "0", "", "",  "for he period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
               
                
                //dsrpt.Tables[0].Merge(dt); ;
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
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("Supplier", typeof(string));
                

                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("InvoiceNo", typeof(string));
                dtrpt.Columns.Add("serialNo", typeof(string));
                dtrpt.Columns.Add("Package", typeof(string));

                dtrpt.Columns.Add("PackageItems", typeof(Int32));
                dtrpt.Columns.Add("TotalItems", typeof(double));
                dtrpt.Columns.Add("Price", typeof(double));

                dtrpt.Columns.Add("TotalAmount ", typeof(double));
                dtrpt.Columns.Add("PurchaseTotalAmount", typeof(double));
                dtrpt.Columns.Add("ItemName", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("pdate", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                string q = "";
                if (cmbbrand.Text == "All")
                {
                    if (textBox1.Text == string.Empty)
                    {
                       // q = "SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package,                       dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where   (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  ";

                        q = "SELECT     dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems,                       dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM,                       dbo.Supplier.Id, dbo.Brands.BrandName FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id where   (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                       // q = "SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package,                       dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where  (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and ( dbo.Purchase.Id = '" + textBox1.Text.Trim().Replace("GRN-", "") + "') ";
                        q = "SELECT     dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems,                       dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM,                       dbo.Supplier.Id, dbo.Brands.BrandName FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id where   (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and ( dbo.Purchase.Id = '" + textBox1.Text.Trim().Replace("GRN-", "") + "')";
                  
                    }
                }
                else
                {
                    if (textBox1.Text == string.Empty)
                    {
                        q = "SELECT     dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems,                       dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM,                       dbo.Supplier.Id, dbo.Brands.BrandName FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id where dbo.RawItem.BrandId='" + cmbbrand.SelectedValue + "' and  (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                
                //        q = "SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package,                       dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where  dbo.Supplier.Id='" + cmbbrand.SelectedValue + "' and (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  ";
                    }
                    else
                    {
                        q = "SELECT     dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems,                       dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM,                       dbo.Supplier.Id, dbo.Brands.BrandName FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.Brands ON dbo.RawItem.BrandId = dbo.Brands.Id where dbo.RawItem.BrandId='" + cmbbrand.SelectedValue + "' and  (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and ( dbo.Purchase.Id = '" + textBox1.Text.Trim().Replace("GRN-", "") + "')";
                
                        //q = "SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package,                       dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where  dbo.Supplier.Id='" + cmbbrand.SelectedValue + "' and (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and ( dbo.Purchase.Id = '" + textBox1.Text.Trim().Replace("GRN-", "") + "') ";
                    }
                }
                double tamount = 0;
                dsinfo = objCore.funGetDataSet(q);
                for (int j = 0; j < dsinfo.Tables[0].Rows.Count; j++)
                {
                   
                    DateTime dte = DateTime.Now;
                    string date = dsinfo.Tables[0].Rows[j]["Date"].ToString();

                    try
                    {
                        tamount = tamount + Convert.ToDouble(dsinfo.Tables[0].Rows[j]["PurchaseTotalAmount"].ToString());

                    }
                    catch (Exception ex)
                    {
                        
                        
                    }

                    try
                    {
                        dte = Convert.ToDateTime(date);
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    string grn = "";
                    if (textBox1.Text.Trim() == "")
                    {
                        grn = "All";
                    }
                    else
                    {
                        grn = dsinfo.Tables[0].Rows[j]["serialNo"].ToString();
                    }
                    string brn = "";
                    if (cmbbrand.Text.Trim() == "All")
                    {
                        brn = "All";
                    }
                    else
                    {
                        brn = dsinfo.Tables[0].Rows[j]["BrandName"].ToString();
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
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(brn,  dte, dsinfo.Tables[0].Rows[j]["InvoiceNo"].ToString(), grn, dsinfo.Tables[0].Rows[j]["Package"].ToString(), dsinfo.Tables[0].Rows[j]["PackageItems"].ToString(), dsinfo.Tables[0].Rows[j]["TotalItems"].ToString(), dsinfo.Tables[0].Rows[j]["Price"].ToString(), dsinfo.Tables[0].Rows[j]["TotalAmount"].ToString(), tamount, dsinfo.Tables[0].Rows[j]["ItemName"].ToString(), dsinfo.Tables[0].Rows[j]["UOM"].ToString(), "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);

                    }
                    else
                    {

                        dtrpt.Rows.Add(brn,  dte, dsinfo.Tables[0].Rows[j]["InvoiceNo"].ToString(), grn, dsinfo.Tables[0].Rows[j]["Package"].ToString(), dsinfo.Tables[0].Rows[j]["PackageItems"].ToString(), dsinfo.Tables[0].Rows[j]["TotalItems"].ToString(), dsinfo.Tables[0].Rows[j]["Price"].ToString(), dsinfo.Tables[0].Rows[j]["TotalAmount"].ToString(), tamount, dsinfo.Tables[0].Rows[j]["ItemName"].ToString(), dsinfo.Tables[0].Rows[j]["UOM"].ToString(), "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);


                        

                    }
                    
                }
                //dtrpt = dsinfo.Tables[0];

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return dtrpt;// dsinfo.Tables[0];
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
