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
    public partial class PurChaseOrderList : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        PurchaseOrder _frm;
        public PurChaseOrderList(PurchaseOrder frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void PurChase_List_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from supplier";
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
                    q = "SELECT        dbo.Supplier.Name AS Supplier_Name, dbo.PurchaseOrder.Id AS PONo, dbo.PurchaseOrder.InvoiceNo, dbo.PurchaseOrder.Date, dbo.PurchaseOrder.TotalAmount, dbo.PurchaseOrder.Status FROM            dbo.PurchaseOrder INNER JOIN                         dbo.Supplier ON dbo.PurchaseOrder.SupplierId = dbo.Supplier.Id where (dbo.PurchaseOrder.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                }
                else
                {
                    q = "SELECT        dbo.Supplier.Name AS Supplier_Name, dbo.PurchaseOrder.Id AS PONo, dbo.PurchaseOrder.InvoiceNo, dbo.PurchaseOrder.Date, dbo.PurchaseOrder.TotalAmount, dbo.PurchaseOrder.Status FROM            dbo.PurchaseOrder INNER JOIN                         dbo.Supplier ON dbo.PurchaseOrder.SupplierId = dbo.Supplier.Id where (dbo.PurchaseOrder.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseOrder.SupplierId='" + cmbsupplier.SelectedValue + "'";

                }
                ds = objCore.funGetDataSet(q);

                dataGridView1.DataSource = ds.Tables[0];
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
               //if (dataGridView1.Rows[indx].Cells[10].Value.ToString() != "Posted")
               {
                   string rawid = dataGridView1.Rows[indx].Cells[2].Value.ToString();
                 
                   _frm.getdata(rawid);
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
                    string invoice = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    string vendor = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string id = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string date = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    POSRestaurant.Reports.Inventory.frmPOPreview obj = new Reports.Inventory.frmPOPreview();
                    obj.id = id;
                    obj.date = date;
                    obj.invoiceno = invoice;
                    obj.vendor = vendor;
                    obj.ShowDialog();
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
