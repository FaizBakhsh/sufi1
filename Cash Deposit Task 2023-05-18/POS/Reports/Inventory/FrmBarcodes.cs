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
    public partial class FrmBarcodes : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public string date = "", branchid = "", type = "";
        public FrmBarcodes()
        {
            InitializeComponent();
        }
       
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            bindreport();
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.Inventory.rptBarcodes rptDoc = new rptBarcodes();
                POSRestaurant.Reports.Inventory.dsBarcodes dsrpt = new dsBarcodes();
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
                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                
                rptDoc.SetDataSource(dsrpt);
                //rptDoc.SetParameterValue("Comp", company);
                //rptDoc.SetParameterValue("Address",address );
                //rptDoc.SetParameterValue("phone", phone);
                //rptDoc.SetParameterValue("date", date);
                //rptDoc.SetParameterValue("title", "Store Demand Sheet");

                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double getprice(string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + id + ")";
            try
            {
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {
            }

            return cost;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Branch", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Code", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Expiry", typeof(string));
                DataSet ds = new DataSet();
                string q = "";
                q = "SELECT        dbo.RawItem.ItemName as name, dbo.Branch.BranchName as Branch, dbo.InventoryTransfer.TransferIn as Quantity, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.barcode as Code, dbo.UOM.UOM FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.Branch ON dbo.InventoryTransfer.branchid = dbo.Branch.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransfer.TransferIn>0 and dbo.InventoryTransfer.date='" + date + "' and dbo.InventoryTransfer.branchid='" + branchid + "' order by dbo.RawItem.ItemName";
                q = "SELECT        dbo.RawItem.ItemName AS name, dbo.Branch.BranchName AS Branch, dbo.InventoryTransfer.TransferIn AS Quantity, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.barcode AS Code, dbo.UOM.UOM,dbo.PurchaseDetails.Expiry FROM            dbo.PurchaseDetails INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id RIGHT OUTER JOIN                         dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.Branch ON dbo.InventoryTransfer.branchid = dbo.Branch.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id ON dbo.Purchase.InvoiceNo = dbo.InventoryTransfer.invoiceno AND dbo.PurchaseDetails.RawItemId = dbo.InventoryTransfer.Itemid  where dbo.InventoryTransfer.TransferIn>0 and dbo.InventoryTransfer.date='" + date + "' and dbo.InventoryTransfer.branchid='" + branchid + "' order by dbo.RawItem.ItemName";
                DataSet dsdate = new DataSet();
                dsdate = objCore.funGetDataSet(q);
                dtrpt.Merge(dsdate.Tables[0], true, MissingSchemaAction.Ignore);

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
