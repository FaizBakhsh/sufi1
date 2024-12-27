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
    public partial class StockEstimationList : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        StockEstimation _frm;
        public StockEstimationList(StockEstimation frm)
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
                q = "SELECT        Date, OrderNo, SUM(Quantity) AS TotalQuantity FROM            dbo.StockEstimation   where (dbo.StockEstimation.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY Date, OrderNo ";
              
                ds = objCore.funGetDataSet(q);

                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {

                var BtnCell1 = (DataGridViewButtonCell)dr.Cells[0];
                BtnCell1.Value = "Preview";
                var BtnCell2 = (DataGridViewButtonCell)dr.Cells[1];
                BtnCell2.Value = "Post";
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
                    string OrderNo = dataGridView1.Rows[indx].Cells["OrderNo"].Value.ToString();


                    _frm.getdata(date, OrderNo);
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
                    string orderno = dataGridView1.Rows[e.RowIndex].Cells["OrderNo"].Value.ToString();
                   
                 
                    string date = dataGridView1.Rows[e.RowIndex].Cells["date"].Value.ToString();
                    POSRestaurant.Reports.Inventory.frmStockEstimationPreview obj = new Reports.Inventory.frmStockEstimationPreview();
                    obj.date = date;
                    obj.orderno = orderno;
                    obj.ShowDialog();
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                if (e.ColumnIndex == 1)
                {
                    string demandno = dataGridView1.Rows[e.RowIndex].Cells["DemandNo"].Value.ToString();
                    string kdsid = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    string kitchen = dataGridView1.Rows[e.RowIndex].Cells["kitchen"].Value.ToString();
                    string date = dataGridView1.Rows[e.RowIndex].Cells["date"].Value.ToString();
                    string q = "update storedemand set status='Posted' where  kdsid='" + kdsid + "' and date='" + date + "' and invoiceno='" + demandno + "'";
                    objCore.executeQuery(q);
                    MessageBox.Show("Demand POsted Successfully");
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
