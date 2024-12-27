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
    public partial class DemandList2 : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        invTransfer _frm;
        public DemandList2(invTransfer frm)
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
                string q = "select * from supplier";
                DataSet ds1 = new DataSet();
                ds1 = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds1.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";

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
                q = "SELECT     top 7   Date, Status, COUNT(*) AS TotalQuantity FROM            dbo.Demand where  branchid='" + cmbbranch.SelectedValue + "'  and date between '"+dateTimePicker1.Text+"' and '"+dateTimePicker2.Text+"' GROUP BY  Date, Status order by date desc";            
                ds = objCore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    var BtnCell2 = (DataGridViewButtonCell)dr.Cells[0];
                    BtnCell2.Value = "Preview";
                }
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
                string date = dataGridView1.Rows[indx].Cells[1].Value.ToString();
                _frm.getdata2(cmbbranch.SelectedValue.ToString(), date);
               
                this.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                   
                    {
                        string date = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        string branchid = cmbbranch.SelectedValue.ToString();
                       POSRestaurant.Reports.Inventory.frmBranchDemandPreview obj = new  Reports.Inventory.frmBranchDemandPreview();
                       obj.date = date;
                       obj.restaurant = cmbbranch.Text;
                       obj.branchid = branchid;
                       obj.Show();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
