using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;

namespace POSRestaurant.Sale
{
    public partial class Transferbill : Form
    {
        public Transferbill()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public string date = "", shiftid = "";
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void Transferbill_Load(object sender, EventArgs e)
        {
            try
            {
                string no = POSRestaurant.Properties.Settings.Default.MainScreenLocation.ToString();
                if (Convert.ToInt32(no) > 0)
                {


                    Screen[] sc;
                    sc = Screen.AllScreens;
                    this.StartPosition = FormStartPosition.Manual;
                    int no1 = Convert.ToInt32(no);
                    this.Location = Screen.AllScreens[no1].WorkingArea.Location;
                    this.WindowState = FormWindowState.Normal;
                    this.StartPosition = FormStartPosition.CenterScreen;
                    this.WindowState = FormWindowState.Maximized;

                }

            }
            catch (Exception ex)
            {

            }
            this.TopMost = true;
            getdata();   
        }
        public void getdata()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id as Bill_No, Customer as Bill_Title,NetBill,BillType from sale where date='" + date + "'  and Billstatus='Paid' and (billtype like '%Visa%'  or  billtype='Cash') order by id desc";
                if (txtsearch.Text.Trim().Length > 0)
                {
                   q = "select id as Bill_No, Customer as Bill_Title,NetBill,BillType from sale where id='"+txtsearch.Text+"' and date='" + date + "'  and Billstatus='Paid' and (billtype like '%Visa%'  or  billtype='Cash') order by id desc";
              
                }
                ds = objcore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                //dataGridView1.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        
        }
        public void billtype(string sid, string type, string amount)
        {
            try
            {
                DataSet dss = new DataSet();
                int idd = 0;
                dss = objcore.funGetDataSet("select max(id) as id from BillType");
                if (dss.Tables[0].Rows.Count > 0)
                {
                    string ii = dss.Tables[0].Rows[0][0].ToString();
                    if (ii == string.Empty)
                    {
                        ii = "0";
                    }
                    idd = Convert.ToInt32(ii) + 1;
                }
                else
                {
                    idd = 1;
                }
                string q = "insert into BillType (SaleId,type,Amount,cashoutime) values('" + sid + "','" + type + "','" + amount + "','"+DateTime.Now+"')";
                objcore.executeQuery(q);
                q = "update sale set BillType='" + type + "' where id='" + sid + "'";
                objcore.executeQuery(q);
            }
            catch (Exception ex)
            {


            }
        }
        private void btnsmall_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string q = "select * from gst";
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 1)
            {
                MessageBox.Show("Because of Different Tax ratio on Cash and Card.Please Reopen Bill and Change Bill Type from there");
                return;
            }
            try
            {
                string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string amount = dataGridView1.CurrentRow.Cells["NetBill"].Value.ToString();
                q = "update sale set billtype='Cash',uploadstatusserver='Pending',uploadstatusbilltype='Pending',uploadstatusrefund='Pending',UploadStatus='Pending' where id='" + id + "'";
                objcore.executeQuery(q);
                q = "delete from billtype where saleid='" + id + "'";
                objcore.executeQuery(q);
                billtype(id, "Cash", amount);
                MessageBox.Show("Bill Transfered Successfully");
                getdata();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void updatebank(string type)
        {
            try
            {
                string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string q = "update sale set billtype='" + type + "',uploadstatusserver='Pending',uploadstatusbilltype='Pending',uploadstatusrefund='Pending',UploadStatus='Pending'  where id='" + id + "'";
                objcore.executeQuery(q);
                q = "delete from billtype where saleid='" + id + "'";
                objcore.executeQuery(q);
                string amount = dataGridView1.CurrentRow.Cells["NetBill"].Value.ToString();
                billtype(id, type, amount);
                MessageBox.Show("Bill Transfered Successfully");
                getdata();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string q = "select * from gst";
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 1)
            {
                MessageBox.Show("Because of Different Tax ratio on Cash and Card.Please Reopen Bill and Change Bill Type from there");
                return;
            }
         
            transferbanks obj = new transferbanks(this);
            obj.Show();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string q = "update sale set billtype='Master Card',uploadstatusserver='Pending',uploadstatusbilltype='Pending',uploadstatusrefund='Pending',UploadStatus='Pending'  where id='" + id + "'";
                objcore.executeQuery(q);
                q = "delete from billtype where saleid='" + id + "'";
                objcore.executeQuery(q);
                string amount = dataGridView1.CurrentRow.Cells["NetBill"].Value.ToString();
                billtype(id, "Master Card", amount);
                MessageBox.Show("Bill Transfered Successfully");
                getdata();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string userid = "";
        private void vButton2_Click_1(object sender, EventArgs e)
        {
            vButton b = sender as vButton;
            string authentication = objcore.authentication1(b.Text, userid);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
            string q = "update sale set billstatus='Pending' where id='" + id + "'";
            int res = objcore.executeQueryint(q);
            if (res > 0)
            {
                MessageBox.Show("Bill Reopened Successfully");
            }
            getdata();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            getdata();
        }
    }
}
