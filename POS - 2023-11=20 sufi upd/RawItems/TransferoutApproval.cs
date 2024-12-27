using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class TransferoutApproval : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public TransferoutApproval()
        {
            InitializeComponent();
        }
        public void getdata()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0,tin=0,tout=0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
           
            ds.Columns.Add("Transfer Out", typeof(string));
            ds.Columns.Add("Price", typeof(string));
            ds.Columns.Add("Total", typeof(string));
            ds.Columns.Add("Remarks", typeof(string));
            ds.Columns.Add("Status", typeof(string));

            string q = "";
           
            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.InventoryTransferApproval.TransferOut FROM            dbo.InventoryTransferApproval INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferApproval.Itemid = dbo.RawItem.Id AND dbo.InventoryTransferApproval.Itemid = dbo.RawItem.Id AND dbo.InventoryTransferApproval.Itemid = dbo.RawItem.Id AND                          dbo.InventoryTransferApproval.Itemid = dbo.RawItem.Id AND dbo.InventoryTransferApproval.Itemid = dbo.RawItem.Id AND dbo.InventoryTransferApproval.Itemid = dbo.RawItem.Id AND                          dbo.InventoryTransferApproval.Itemid = dbo.RawItem.Id  where dbo.InventoryTransferApproval.Date ='" + date + "' and dbo.InventoryTransferApproval.branchid='" + comboBox1.SelectedValue + "'";
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                string val = "";
                double rem = 0;
                DataSet dspurchase = new DataSet();
                string remarks = "",status="";
                dspurchase = new DataSet();
                q = "SELECT     (TransferIn) AS tin,(TransferOut) AS tout,Remarks,Status FROM     InventoryTransferApproval where Date ='" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + comboBox1.SelectedValue + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    status = dspurchase.Tables[0].Rows[0]["Status"].ToString();
                    remarks = dspurchase.Tables[0].Rows[0]["remarks"].ToString();
                   
                    val = dspurchase.Tables[0].Rows[0][1].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = Convert.ToDouble(val);
                }
                double price1 = getprice(ds1.Tables[0].Rows[i]["id"].ToString());
                double amount = price1 * tout;
                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(),  tout,price1,amount,remarks,status);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            {
                if (gr.Cells["Status"].Value.ToString() == "Approved")
                {
                    
                    gr.DefaultCellStyle.BackColor = Color.Green;
                   
                }
            }
            
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }
        public double getprice(string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + dateTimePicker1.Text + "','" + dateTimePicker1.Text + "'," + id + ")";
            try
            {
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {
            }

            return cost;
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
           
        }
        
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            string q = "update InventoryTransferapproval set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "', status='Approved' where branchid='" + comboBox1.SelectedValue + "' and date='" + dateTimePicker1.Text + "'";
            int res = objcore.executeQueryint(q);
            if (res > 0)
            {
                MessageBox.Show("Record Saved Successfully");
                getdata();
            }
            else
            {
                MessageBox.Show("No data Exist or Failed to Approve");
            }

        }
        string url = "";
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='main'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            if (tp == "")
            {
                tp = "sql";
            }
            return tp;
        }
        private void vButton3_Click(object sender, EventArgs e)
        {

        }
   
       
        public void fillitems()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch where status='Active' order by BranchName";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];
               
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "BranchName";
                


            }
            catch (Exception ex)
            {


            }


        }
        
        private void Variance_Load(object sender, EventArgs e)
        {
            fillitems();
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmTransferoutapproval obj = new Reports.Inventory.frmTransferoutapproval();
            obj.date = dateTimePicker1.Text;
            obj.branchid = comboBox1.SelectedValue.ToString();
            obj.branchname = comboBox1.Text;
            obj.Show();
        }

        private void vButton3_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
