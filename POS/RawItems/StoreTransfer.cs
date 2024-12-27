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
    public partial class StoreTransfer : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public StoreTransfer()
        {
            InitializeComponent();
        }
        public void getdata(string date, string invoice, string kdsid)
        {
            dateTimePicker1.Text = date;
            txtinvoice.Text = invoice;
            cmbstore2.SelectedValue = kdsid;
            getdata();
        }
        public void getdata()
        {
            string date = dateTimePicker1.Text;
           
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("Quantity", typeof(string));
            if (txtinvoice.Text == "")
            {
                MessageBox.Show("Please Enter Invoice No");
                return;
            }
            string q = "";
            if (textBox1.Text.Trim() == "")
            {
                q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id ORDER BY dbo.RawItem.ItemName";
            }
            else
            {
                if (cmbfilter.Text == "")
                {
                    q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.itemname like '%" + textBox1.Text + "%'  order by dbo.RawItem.itemname";
                }
                else
                {
                    string filter = cmbfilter.Text;
                    if (filter == "Start With")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.itemname like '" + textBox1.Text + "%'  order by dbo.RawItem.itemname";

                    }
                    else if (filter == "End With")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.itemname like '%" + textBox1.Text + "'  order by dbo.RawItem.itemname";

                    }
                    else if (filter == "Anywhere")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.itemname like '%" + textBox1.Text + "%'  order by dbo.RawItem.itemname";

                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.itemname like '%" + textBox1.Text + "%'  order by dbo.RawItem.itemname";

                    }
                }
            }
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                double qty = 0;
                string val = "";
                double rem = 0;
                DataSet dspurchase = new DataSet();
                string remarks = "";
                dspurchase = new DataSet();
                q = "SELECT     Quantity FROM     InventoryTransferStore where Date ='" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + comboBox1.SelectedValue + "' and IssuingStoreId='" + cmbstore1.SelectedValue + "' and RecvStoreId='" + cmbstore2.SelectedValue + "' and InvoiceNo='" + txtinvoice.Text + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {                   
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    qty = Convert.ToDouble(val);                    
                }
                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), qty);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {
                try
                {
                    string temp = dgr.Cells["Quantity"].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    if (Convert.ToDouble(temp) > 0)
                    {
                        dgr.DefaultCellStyle.BackColor = Color.Green;
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
            
        }
        public void opendemand(string date, string kdsid, string InvoiceNo)
        {
            dateTimePicker1.Text = date;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("Quantity", typeof(string));

            string q = "";
            q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname, dbo.StoreDemand.Quantity, dbo.StoreDemand.kdsid FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.StoreDemand ON dbo.RawItem.Id = dbo.StoreDemand.Itemid where dbo.StoreDemand.InvoiceNo='" + InvoiceNo + "' and dbo.StoreDemand.date='" + Convert.ToDateTime(date).ToString("yyyy-MM-dd") + "' and dbo.StoreDemand.kdsid='" + kdsid + "'  ORDER BY dbo.RawItem.ItemName";
            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                txtinvoice.Text = InvoiceNo;
                cmbstore2.SelectedValue = ds1.Tables[0].Rows[i]["kdsid"].ToString();
                double qty = 0;
                string val = "";

                val = ds1.Tables[0].Rows[i]["Quantity"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                qty = Convert.ToDouble(val);
                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), qty);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Text = "Wait Please";
            vButton1.Enabled = false;
            getdata();
            vButton1.Text = "Search";
            vButton1.Enabled = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
           
        }
        
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            //if (cmbstore1.SelectedValue.ToString() == cmbstore2.SelectedValue.ToString())
            //{
            //    MessageBox.Show("Both Stores can not be same");
            //    return;
            //}
            bool chk = false;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                chk1 = false;

                {
                    DataSet dss = new DataSet();

                    try
                    {
                        dss = new DataSet();
                        string q = "select * from InventoryTransferStore where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "'  and IssuingStoreId='" + cmbstore1.SelectedValue + "' and RecvStoreId='" + cmbstore2.SelectedValue + "' and InvoiceNo='" + txtinvoice.Text + "'";
                        dss = objcore.funGetDataSet(q);
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            q = "update InventoryTransferStore set uploadstatus='Pending',Quantity='" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 2).ToString() + "'  where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and IssuingStoreId='" + cmbstore1.SelectedValue + "'  and RecvStoreId='" + cmbstore2.SelectedValue + "'  and InvoiceNo='" + txtinvoice.Text + "'";
                            objcore.executeQuery(q);
                            chk = true;
                        }
                        else
                        {
                            q = "insert into InventoryTransferStore (InvoiceNo,Date, Itemid, Quantity, IssuingStoreId,RecvStoreId, branchid) values('" + txtinvoice.Text + "','" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 2).ToString() + "','" + cmbstore1.SelectedValue + "','" + cmbstore2.SelectedValue + "','" + comboBox1.SelectedValue + "')";
                            objcore.executeQuery(q);
                            chk = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    //updateremaininginventory(dr.Cells["id"].Value.ToString(),variance);

                }
            }
            if (chk == true)
            {
                string q = "update storeDemand set status='Processed' where invoiceno='" + txtinvoice.Text + "' and date='" + dateTimePicker1.Text + "' and kdsid='" + cmbstore2.SelectedValue + "'";
                objcore.executeQuery(q);
                MessageBox.Show("Record Saved Successfully");
                getdata();
            }
        }
        private void vButton3_Click(object sender, EventArgs e)
        {
            DemandList obj = new DemandList(this);
            obj.ShowDialog();
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
        public void fillstore()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Stores where branchid='"+comboBox1.SelectedValue+"'";
                ds = objcore.funGetDataSet(q);              
                dt = ds.Tables[0];
                cmbstore1.DataSource = dt;
                cmbstore1.ValueMember = "id";
                cmbstore1.DisplayMember = "StoreName";

            }
            catch (Exception ex)
            {


            }
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from KDS where id>0";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];
                cmbstore2.DataSource = dt;
                cmbstore2.ValueMember = "id";
                cmbstore2.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }

        }
        private void Variance_Load(object sender, EventArgs e)
        {
            fillitems();
            fillstore();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillstore();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            StoreIssuanceList obj = new StoreIssuanceList(this);
            obj.ShowDialog();
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
          //  getdata();
        }
    }
}
