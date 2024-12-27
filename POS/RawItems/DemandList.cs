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
    public partial class DemandList : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        StoreTransfer _frm;
        public DemandList(StoreTransfer frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void PurChase_List_Load(object sender, EventArgs e)
        {
            try
            {

                string q = "select * from branch";
                ds = objCore.funGetDataSet(q);


             

                cmbbranch.DataSource = ds.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchName";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                DataTable dt = new DataTable();
                objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from KDS where id>0";
                ds = objCore.funGetDataSet(q);
                dt = ds.Tables[0];
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }

        }
        protected void getdata()
        {
            try
            {
                string q = "";
                ds = new DataSet();
                q = "SELECT        Date,InvoiceNo, Status, COUNT(*) AS TotalQuantity FROM            dbo.storeDemand where kdsid='" + comboBox1.SelectedValue + "' and date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and (status='Posted' or status='Processed' ) GROUP BY  Date, InvoiceNo,Status ";
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
                string status = dataGridView1.Rows[indx].Cells[3].Value.ToString();
                if (status == "Processed")
                {
                    MessageBox.Show("This demand is already processed");
                    return;
                }
                string date = dataGridView1.Rows[indx].Cells[1].Value.ToString();
                string InvoiceNo = dataGridView1.Rows[indx].Cells[2].Value.ToString();
                _frm.opendemand(date, comboBox1.SelectedValue.ToString(), InvoiceNo);
                this.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    string demandno = dataGridView1.Rows[e.RowIndex].Cells["InvoiceNo"].Value.ToString();
                    string kdsid = comboBox1.SelectedValue.ToString();
                    string kitchen = comboBox1.Text.ToString();
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
        }
    }
}
