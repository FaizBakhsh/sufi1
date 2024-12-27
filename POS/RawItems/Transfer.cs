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
    public partial class Transfer : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public Transfer()
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
            ds.Columns.Add("Transfer In", typeof(string));
            ds.Columns.Add("Transfer Out", typeof(string));
            ds.Columns.Add("Remarks", typeof(string));


            string q = "";
            if (textBox1.Text == "")
            {
                q = "select id,itemname from rawitem order by itemname ";
            }
            else
            {
                q = "select id,itemname from rawitem where itemname like '%"+textBox1.Text+"%' order by itemname ";
            }
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                string val = "";
                double rem = 0;
                DataSet dspurchase = new DataSet();
                string remarks = "";
                dspurchase = new DataSet();
                q = "SELECT     (TransferIn) AS tin,(TransferOut) AS tout,Remarks FROM     InventoryTransfer where Date ='" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and branchid='"+comboBox1.SelectedValue+"' and sourcebranchid is null";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    remarks = dspurchase.Tables[0].Rows[0]["remarks"].ToString();
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tin = Convert.ToDouble(val);
                    val = dspurchase.Tables[0].Rows[0][1].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = Convert.ToDouble(val);
                }

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), tin, tout,remarks);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            try
            {
                q = "select status from DeviceSetting where device='Transfer Out Approval'";
                DataSet dsapr = new DataSet();
                dsapr = objcore.funGetDataSet(q);
                if (dsapr.Tables[0].Rows.Count > 0)
                {
                    string status = dsapr.Tables[0].Rows[0][0].ToString();
                    if (status == "Enabled")
                    {
                        dataGridView1.Columns[3].ReadOnly = true;
                        btntransferout.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
           
        }
        
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            bool chk = false;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                chk1 = false;

                {
                    DataSet dss = new DataSet();

                    dss = new DataSet();
                    string q = "select * from InventoryTransfer where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid is null";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        q = "update InventoryTransfer set uploadstatus='Pending',Remarks='" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "',TransferIn='" + Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2).ToString() + "',TransferOut ='" + Math.Round(Convert.ToDouble(dr.Cells["Transfer Out"].Value.ToString()), 2).ToString() + "' where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid is null";
                        objcore.executeQuery(q);
                        chk = true;
                    }
                    else
                    {
                        q = "insert into InventoryTransfer (Remarks,Date, Itemid, TransferIn, TransferOut, branchid) values('" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "','" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2).ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Transfer Out"].Value.ToString()), 2).ToString() + "','" + comboBox1.SelectedValue + "')";
                        objcore.executeQuery(q);
                        chk = true;
                    }
                    //updateremaininginventory(dr.Cells["id"].Value.ToString(),variance);

                }
            }
            if (chk == true)
            {
                MessageBox.Show("Record Saved Successfully");
                getdata();
            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void fillitems()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch";
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
    }
}
