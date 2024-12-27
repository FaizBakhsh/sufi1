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
    public partial class FrmBarcodePrint : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public string date = "", branchid = "", type = "";
        public FrmBarcodePrint()
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


                POSRestaurant.Reports.Inventory.rptPrintBarcode rptDoc = new rptPrintBarcode();
                POSRestaurant.Reports.Inventory.dsBarcodePrint dsrpt = new dsBarcodePrint();
                //feereport ds = new feereport(); // .xsd file name

                getcompany();
                dt = getAllOrders();
                // Just set the name of data table
                dt.TableName = "Crystal Report";              
                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);                
                rptDoc.SetDataSource(dsrpt);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }

        }
        public List<POSRestaurant.RawItems.Barcodes.issuelist> issueLists = new List<RawItems.Barcodes.issuelist>();
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Barcode1", typeof(string));
                dtrpt.Columns.Add("Name1", typeof(string));
                dtrpt.Columns.Add("Expiry1", typeof(string));
                dtrpt.Columns.Add("Barcode2", typeof(string));
                dtrpt.Columns.Add("Name2", typeof(string));
                dtrpt.Columns.Add("Expiry2", typeof(string));
                dtrpt.Columns.Add("Barcode3", typeof(string));
                dtrpt.Columns.Add("Name3", typeof(string));
                dtrpt.Columns.Add("Expiry3", typeof(string));
                dtrpt.Columns.Add("Barcode4", typeof(string));
                dtrpt.Columns.Add("Name4", typeof(string));
                dtrpt.Columns.Add("Expiry4", typeof(string));
                dtrpt.Columns.Add("Barcode5", typeof(string));
                dtrpt.Columns.Add("Name5", typeof(string));
                dtrpt.Columns.Add("Expiry5", typeof(string));
                DataSet ds = new DataSet();
                string q = "";

                q = "SELECT         dbo.RawItem.Id,dbo.RawItem.LoosQTY,dbo.RawItem.ItemName AS name, dbo.Branch.BranchName AS Branch, dbo.InventoryTransfer.TransferIn AS Quantity, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.barcode AS Code, dbo.UOM.UOM,dbo.PurchaseDetails.Expiry FROM            dbo.PurchaseDetails INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id RIGHT OUTER JOIN                         dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.Branch ON dbo.InventoryTransfer.branchid = dbo.Branch.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id ON dbo.Purchase.InvoiceNo = dbo.InventoryTransfer.invoiceno AND dbo.PurchaseDetails.RawItemId = dbo.InventoryTransfer.Itemid  where dbo.InventoryTransfer.TransferIn>0 and dbo.InventoryTransfer.date='" + date + "' and dbo.InventoryTransfer.branchid='" + branchid + "' order by dbo.RawItem.ItemName";
                DataSet dsdate = new DataSet();
                dsdate = objCore.funGetDataSet(q);
                int k = 0;
                DataRow dr2 = dtrpt.NewRow();

                for (int i = 0; i < dsdate.Tables[0].Rows.Count; i++)
                {
                    int qty = 0, loosqty = 1, stickerqty = 0;
                    try
                    {
                        string temp = dsdate.Tables[0].Rows[i]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        qty = Convert.ToInt32(temp);
                        temp = dsdate.Tables[0].Rows[i]["LoosQTY"].ToString();
                        if (temp == "")
                        {
                            temp = "1";
                        }
                        loosqty = Convert.ToInt32(temp);
                        stickerqty = Convert.ToInt32(issueLists.Where(x => x.Id == dsdate.Tables[0].Rows[i]["Id"].ToString()).ToList()[0].Sticker);// qty / loosqty;

                    }
                    catch (Exception ex)
                    {

                    }
                    for (int j = 0; j < stickerqty; j++)
                    {
                        if (k == 0)
                        {
                            dr2 = dtrpt.NewRow();
                            dr2["Barcode1"] = dsdate.Tables[0].Rows[i]["Code"].ToString();
                            dr2["Name1"] = dsdate.Tables[0].Rows[i]["name"].ToString();
                            string expiry = dsdate.Tables[0].Rows[i]["Expiry"].ToString();
                            if (expiry.Length > 0)
                            {
                                expiry = "Expiry: " + expiry;
                            }
                            dr2["Expiry1"] = expiry;
                        }
                        if (k == 1)
                        {

                            dr2["Barcode2"] = dsdate.Tables[0].Rows[i]["Code"].ToString();
                            dr2["Name2"] = dsdate.Tables[0].Rows[i]["name"].ToString();
                            string expiry = dsdate.Tables[0].Rows[i]["Expiry"].ToString();
                            if (expiry.Length > 0)
                            {
                                expiry = "Expiry: " + expiry;
                            }
                            dr2["Expiry2"] = expiry;

                        }
                        if (k == 2)
                        {

                            dr2["Barcode3"] = dsdate.Tables[0].Rows[i]["Code"].ToString();
                            dr2["Name3"] = dsdate.Tables[0].Rows[i]["name"].ToString();
                            string expiry = dsdate.Tables[0].Rows[i]["Expiry"].ToString();
                            if (expiry.Length > 0)
                            {
                                expiry = "Expiry: " + expiry;
                            }
                            dr2["Expiry3"] = expiry;
                        }
                        if (k == 3)
                        {

                            dr2["Barcode4"] = dsdate.Tables[0].Rows[i]["Code"].ToString();
                            dr2["Name4"] = dsdate.Tables[0].Rows[i]["name"].ToString();
                            string expiry = dsdate.Tables[0].Rows[i]["Expiry"].ToString();
                            if (expiry.Length > 0)
                            {
                                expiry = "Expiry: " + expiry;
                            }
                            dr2["Expiry4"] = expiry;
                        }
                        if (k == 4)
                        {
                            dr2["Barcode5"] = dsdate.Tables[0].Rows[i]["Code"].ToString();
                            dr2["Name5"] = dsdate.Tables[0].Rows[i]["name"].ToString();
                            string expiry = dsdate.Tables[0].Rows[i]["Expiry"].ToString();
                            if (expiry.Length > 0)
                            {
                                expiry = "Expiry: " + expiry;
                            }
                            dr2["Expiry5"] = expiry;

                        }
                        if (i + 1 == stickerqty)
                        {
                            dtrpt.Rows.Add(dr2);

                        }
                        else

                            if (k == 4)
                            {
                                dtrpt.Rows.Add(dr2);
                                k = 0;
                            }
                            else
                            {
                                k++;
                            }
                    }


                }
                try
                {
                    int count = dr2.ItemArray.Length;
                    if (count > 0)
                    {
                        dtrpt.Rows.Add(dr2);
                    }
                }
                catch (Exception ex)
                {
                    
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
