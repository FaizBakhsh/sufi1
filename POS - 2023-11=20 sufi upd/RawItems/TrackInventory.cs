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
    public partial class TrackInventory : Form
    {
        public TrackInventory()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon ObjCore = new classes.Clsdbcon();
        protected void getdata(string code)
        {
            string q = "SELECT        dbo.Branch.BranchName AS Branch,dbo.InventoryTransfer.Date,dbo.RawItem.ItemName , dbo.UOM.UOM,  dbo.InventoryTransfer.TransferIn AS Quantity,dbo.PurchaseDetails.Expiry,  dbo.InventoryTransfer.barcode AS Code FROM            dbo.PurchaseDetails INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id RIGHT OUTER  JOIN                         dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.Branch ON dbo.InventoryTransfer.branchid = dbo.Branch.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id ON dbo.Purchase.InvoiceNo = dbo.InventoryTransfer.invoiceno AND dbo.PurchaseDetails.RawItemId = dbo.InventoryTransfer.Itemid where dbo.InventoryTransfer.barcode='" + code + "'";
            DataSet ds = new DataSet();
            ds = ObjCore.funGetDataSet(q);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void TrackInventory_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getdata(textBox1.Text);
            }
        }
    }
}
