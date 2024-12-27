using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Sale
{
    public partial class BillRecall : Form
    {
        private  Sale _frm1;
        POSRetail.classes.Clsdbcon objCore ;
        public string date = "";
        DataSet ds ;
        public BillRecall(Sale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            objCore = new classes.Clsdbcon();
        }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";
            
        //}

        private void button1_Click(object sender, EventArgs e)
        {
          
               
                    
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            getdata();
        }
        public void getdata()
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                q9 = "SELECT     Id as Bill_No, Date, time, NetBill,BillType,Terminal   FROM         Sale where userid='" + id + "' and date='" + date + "' order by id desc";

                
                ds9 = objCore.funGetDataSet(q9);
                DataTable dt = new DataTable();
                dt = ds9.Tables[0];
               
                // dataGridView1.Columns[0].Visible = false;
                // dataGridView1.Columns[3].Visible = false;
                bool chk = false;
                //foreach (DataRow dr in dt.Rows)
                //{
                //    //dr.Height = 40;
                //    string iddd = dr[0].ToString();
                //    ds = new DataSet();
                //    ds = objCore.funGetDataSet("select Status from saledetails where saleid='" + iddd + "'");
                //    if (ds.Tables[0].Rows.Count > 0)
                //    {
                //        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                //        {
                //            if (ds.Tables[0].Rows[j]["Status"].ToString() == "Not Void")
                //            {
                //                chk = true;
                //            }
                //        }
                //    }
                //    if (chk == true)
                //    {
                //        chk = false;
                //    }
                //    else
                //    {

                //        dr.Delete();
                //    }


                //}
                dataGridView1.DataSource = dt;
                
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    
                    column.Width=150;//.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                }

            }
            catch (Exception ex)
            {


            }
        }
        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string saleid = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    //string type = dataGridView1.Rows[indx].Cells[4].Value.ToString();
                   // _frm1.Islbldelivery = type;
                    _frm1.recalsale(saleid);
                    _frm1.Enabled = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                
                
            }
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            //_frm1.Islbldelivery = "Not Selected";
            _frm1.Enabled = true;
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                q9 = "SELECT     Id as Bill_No, Date, time, NetBill,BillType,Terminal   FROM         Sale where id='" + txtbill.Text.Trim() + "' and userid='" + id + "' and date='"+date+"'";
                
                
                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
               // dataGridView1.Columns[0].Visible = false;
               // dataGridView1.Columns[3].Visible = false;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 40;
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void vButton2_Click_1(object sender, EventArgs e)
        {
            getdata();
        }
    }
}
