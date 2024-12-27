using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class Barcodes : Form
    {
        public string date = "", branchid = "";
        public Barcodes()
        {
            InitializeComponent();
        }

        private void Barcodes_Load(object sender, EventArgs e)
        {
            getdata();
        }
        DataTable dtrpt = new DataTable();
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public class issuelist
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string UOM { get; set; }
            public double Quantity { get; set; }
            public string Barcode { get; set; }
            public double Sticker { get; set; }
        }
        List<issuelist> issueList = new List<issuelist>();
        public void getdata()
        {
            //dtrpt.Columns.Add("Id", typeof(string));
            //dtrpt.Columns.Add("Name", typeof(string));
            //dtrpt.Columns.Add("UOM", typeof(string));
            //dtrpt.Columns.Add("Quantity", typeof(string));
            //dtrpt.Columns.Add("Barcode", typeof(string));
            //dtrpt.Columns.Add("Sticker Quantity", typeof(string));

            string q = "SELECT         dbo.RawItem.Id,dbo.RawItem.LoosQTY,dbo.RawItem.ItemName AS name, dbo.Branch.BranchName AS Branch, dbo.InventoryTransfer.TransferIn AS Quantity, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.barcode AS Code, dbo.UOM.UOM,dbo.PurchaseDetails.Expiry FROM            dbo.PurchaseDetails INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id RIGHT OUTER JOIN                         dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.Branch ON dbo.InventoryTransfer.branchid = dbo.Branch.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id ON dbo.Purchase.InvoiceNo = dbo.InventoryTransfer.invoiceno AND dbo.PurchaseDetails.RawItemId = dbo.InventoryTransfer.Itemid  where dbo.InventoryTransfer.TransferIn>0 and dbo.InventoryTransfer.date='" + date + "' and dbo.InventoryTransfer.branchid='" + branchid + "' and dbo.RawItem.PrintBarcode='Yes' order by dbo.RawItem.ItemName";
            DataSet dsdate = new DataSet();
            dsdate = objCore.funGetDataSet(q);
            int k = 0;
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
                    stickerqty = qty / loosqty;
                    issueList.Add(new issuelist { Id = dsdate.Tables[0].Rows[i]["Id"].ToString(),Name= dsdate.Tables[0].Rows[i]["name"].ToString(),UOM= dsdate.Tables[0].Rows[i]["UOM"].ToString(),Quantity= qty,Barcode= dsdate.Tables[0].Rows[i]["Code"].ToString(),Sticker= stickerqty });
                   // dtrpt.Rows.Add(dsdate.Tables[0].Rows[i]["Id"].ToString(), dsdate.Tables[0].Rows[i]["name"].ToString(), dsdate.Tables[0].Rows[i]["UOM"].ToString(), qty, dsdate.Tables[0].Rows[i]["Code"].ToString(), stickerqty);

                }
                catch (Exception ex)
                {

                }
            }
            dataGridView1.DataSource = issueList;
            dataGridView1.Columns[0].Visible = false;
           
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.FrmBarcodePrint obj = new Reports.Inventory.FrmBarcodePrint();
            obj.date = date;
            obj.branchid = branchid;
            obj.issueLists = issueList;
            obj.Show();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                issueList[e.RowIndex].Sticker = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            }
        }
    }
}
