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
namespace POSRetail.RawItems
{
    public partial class PurChase_List : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        Purchase _frm;
        public PurChase_List(Purchase frm)
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
                    q = "SELECT     dbo.Supplier.Name AS Supplier_Name, dbo.Purchase.Id AS SerialNo, dbo.Purchase.InvoiceNo, dbo.Purchase.Date, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.Purchase.TotalAmount FROM dbo.Purchase INNER JOIN dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.Branch ON dbo.Purchase.BranchCode = dbo.Branch.Id INNER JOIN  dbo.Stores ON dbo.Purchase.StoreCode = dbo.Stores.Id where (dbo.Purchase.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.Status !='Cancel'";
                }
                else
                {
                    q = "SELECT     dbo.Supplier.Name AS Supplier_Name, dbo.Purchase.Id AS SerialNo, dbo.Purchase.InvoiceNo, dbo.Purchase.Date, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.Purchase.TotalAmount FROM dbo.Purchase INNER JOIN dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.Branch ON dbo.Purchase.BranchCode = dbo.Branch.Id INNER JOIN  dbo.Stores ON dbo.Purchase.StoreCode = dbo.Stores.Id where (dbo.Purchase.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + cmbsupplier.SelectedValue + "' and dbo.Purchase.Status !='Cancel'";

                }
                ds = objCore.funGetDataSet(q);

                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
               string rawid = dataGridView1.Rows[indx].Cells[1].Value.ToString();
               _frm.getdata(rawid);
               this.Close();
           }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
