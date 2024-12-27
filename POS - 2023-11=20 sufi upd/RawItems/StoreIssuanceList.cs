using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using VIBlend.WinForms.DataGridView.Filters;
using VIBlend.Utilities;
namespace POSRestaurant.RawItems
{
    public partial class StoreIssuanceList : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        StoreTransfer _frm;
        public StoreIssuanceList(StoreTransfer frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void PurChase_List_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from kds";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "All";

                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "Name";
                cmbsupplier.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected void getdata()
        {
            try
            {
                string q = "";
                ds = new DataSet();
                if (cmbsupplier.Text == "All")
                {
                    q = "SELECT         dbo.InventoryTransferStore.Date,dbo.InventoryTransferStore.Invoiceno AS DemandNo, dbo.KDS.Name AS Kitchen, dbo.KDS.Id, SUM(dbo.InventoryTransferStore.Quantity) AS TotalQuantity FROM            dbo.KDS INNER JOIN                         dbo.InventoryTransferStore ON dbo.KDS.Id = dbo.InventoryTransferStore.RecvStoreId where (dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.InventoryTransferStore.Invoiceno, dbo.KDS.Name, dbo.KDS.Id, dbo.InventoryTransferStore.Date";
                }
                else
                {
                    q = "SELECT        dbo.InventoryTransferStore.Date, dbo.InventoryTransferStore.Invoiceno AS DemandNo, dbo.KDS.Name AS Kitchen, dbo.KDS.Id, SUM(dbo.InventoryTransferStore.Quantity) AS TotalQuantity FROM            dbo.KDS INNER JOIN                         dbo.InventoryTransferStore ON dbo.KDS.Id = dbo.InventoryTransferStore.RecvStoreId where  (dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RecvStoreId.RecvStoreId='" + cmbsupplier.SelectedValue + "'  GROUP BY dbo.InventoryTransferStore.Invoiceno, dbo.KDS.Name, dbo.KDS.Id, dbo.InventoryTransferStore.Date";
          
                }
                ds = objCore.funGetDataSet(q);

                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[4].Visible = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {

                var BtnCell2 = (DataGridViewButtonCell)dr.Cells[0];
                BtnCell2.Value = "Preview";
               
            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int indx = dataGridView1.CurrentCell.RowIndex;

            if (indx >= 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure to continue","",MessageBoxButtons.OKCancel,MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                //if (dataGridView1.Rows[indx].Cells["Status"].Value.ToString() != "Posted")
                {
                    string date = dataGridView1.Rows[indx].Cells["Date"].Value.ToString();
                    string invoiceno = dataGridView1.Rows[indx].Cells["DemandNo"].Value.ToString();
                    string kdsid = dataGridView1.Rows[indx].Cells["id"].Value.ToString();

                   _frm.getdata(date, invoiceno, kdsid);
                    this.Close();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.ColumnIndex == 0)
                {
                    string demandno = dataGridView1.Rows[e.RowIndex].Cells["DemandNo"].Value.ToString();
                    string kdsid = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    string kitchen = dataGridView1.Rows[e.RowIndex].Cells["kitchen"].Value.ToString();
                    string date = dataGridView1.Rows[e.RowIndex].Cells["date"].Value.ToString();
                    POSRestaurant.Reports.Inventory.frmStoreIssuancePreview obj = new Reports.Inventory.frmStoreIssuancePreview();
                    obj.date = date;
                    obj.demandno = demandno;
                    obj.kitchen = kitchen;
                    obj.kdsid = kdsid;
                    obj.ShowDialog();
                }
            }
            catch (Exception ex)
            {


            }
           
        }
    }
}
