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
    public partial class StoreDemandList : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        StoreDemand _frm;
        public StoreDemandList(StoreDemand frm)
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
                    q = "SELECT        dbo.StoreDemand.Date, dbo.StoreDemand.Invoiceno as DemandNo, dbo.KDS.Name AS Kitchen,dbo.KDS.id, SUM(dbo.StoreDemand.Quantity) AS TotalQuantity,dbo.StoreDemand.Status FROM            dbo.StoreDemand INNER JOIN                         dbo.KDS ON dbo.StoreDemand.kdsid = dbo.KDS.Id  where (dbo.StoreDemand.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.StoreDemand.Date, dbo.StoreDemand.Invoiceno, dbo.KDS.Name, dbo.StoreDemand.Status,dbo.KDS.id ";
                }
                else
                {
                    q = "SELECT        dbo.StoreDemand.Date, dbo.StoreDemand.Invoiceno  as DemandNo, dbo.KDS.Name AS Kitchen,dbo.KDS.id, SUM(dbo.StoreDemand.Quantity) AS TotalQuantity,dbo.StoreDemand.Status FROM            dbo.StoreDemand INNER JOIN                         dbo.KDS ON dbo.StoreDemand.kdsid = dbo.KDS.Id  where (dbo.StoreDemand.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.StoreDemand.kdsid='" + cmbsupplier.SelectedValue + "'  GROUP BY dbo.StoreDemand.Date, dbo.StoreDemand.Invoiceno, dbo.KDS.Name , dbo.StoreDemand.Status,dbo.KDS.id";

                }
                ds = objCore.funGetDataSet(q);

                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[5].Visible = false;
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
                    POSRestaurant.Reports.Inventory.frmDemandPreview obj = new Reports.Inventory.frmDemandPreview();
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
