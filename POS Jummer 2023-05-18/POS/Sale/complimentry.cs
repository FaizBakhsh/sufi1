using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class complimentry : Form
    {
        int count = 0;
        string[] tabsarray = new string[20];
        RestSale _frm;
        public int indx = 0;
        public complimentry(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }
       public DataTable dt = new DataTable();
        private void complimentry_Load(object sender, EventArgs e)
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
            this.TopMost = false;
            try
            {
                dataGridView1.DataSource = dt;
                try
                {
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;
                    dataGridView1.Columns[9].Visible = false;
                    dataGridView1.Columns[11].Visible = false;
                    dataGridView1.Columns[10].Visible = false;
                }
                catch (Exception ex)
                {


                }
                dataGridView1.MultiSelect = true;


                tabsarray = new string[dt.Rows.Count - 1];
            }
            catch (Exception ex)
            {
                               
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;



                {
                    richTextBox1.Text = richTextBox1.Text + t.Text.Replace("&&", "&");
                }



            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //if (dataGridView1.IsCurrentCellDirty)
            //{
            //    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //}
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Selected = true;

            //}
           // dataGridView1.Rows[e.RowIndex].Selected = !dataGridView1.Rows[e.RowIndex].Selected;
        }

        private void vButton18_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        DataGridViewCheckBoxCell chk = dataGridView1.Rows[i].Cells[0] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chk.Value) == true)
                        {
                            dt.Rows[i]["Price"] = "0";
                            dt.Rows[i]["Amount"] = "0";
                            
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                _frm.updatecomplmntry(indx, dt, richTextBox1.Text.Replace("&", "&&").Replace("'", "''"));
                //_frm.TopMost = true;
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            //_frm.TopMost = true;
            this.Close();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    float qty = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
            //    if (qty > 0)
            //    {
            //        string q = "select * from saledetails where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
            //        DataSet ds = new DataSet();
            //        ds = objCore.funGetDataSet(q);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            float oldqty = float.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
            //            float oldamount = float.Parse(ds.Tables[0].Rows[0]["Price"].ToString());
            //            float oldgst = float.Parse(ds.Tables[0].Rows[0]["itemgst"].ToString());
            //            float olddiscount = float.Parse(ds.Tables[0].Rows[0]["itemdiscount"].ToString());
            //            if (qty >= oldqty)
            //            {
            //                MessageBox.Show("Quantity should not be greater then existing quantity");
            //                return;
            //            }
            //            float newqty = oldqty - qty;
            //            float singleprice = float.Parse(Math.Round(oldamount / oldqty).ToString());
            //            double newadiscount = Math.Round(olddiscount * oldqty / 100, 2);
            //            newadiscount = newadiscount * qty;
            //            double prevdiscount = Math.Round(olddiscount * oldqty / 100, 2);
            //            prevdiscount = prevdiscount * newqty;
            //            double newgst = Math.Round(oldgst * oldqty / 100, 2);
            //            newgst = newgst * qty;
            //            double prevgst = Math.Round(oldgst * oldqty / 100);
            //            prevgst = prevgst * newqty;
            //            double newamount = Math.Round(singleprice * qty);
            //            double oldprice = Math.Round(singleprice * newqty);

            //            q = "insert into saledetails(saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus, branchid, Itemdiscount, ItemdiscountPerc, ItemGst, ItemGstPerc, OrderStatusmain, atid, dealid, OnlineId, kdsgroup, time, kdsid, completedtime, pointscode, ParkStatus) SELECT   saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, " + qty + ", " + newamount + ", BarnchCode, Status, comments, Orderstatus, branchid, " + newadiscount + ", ItemdiscountPerc, " + newgst + ", ItemGstPerc, OrderStatusmain, atid, dealid, OnlineId, kdsgroup, time, kdsid, completedtime, pointscode, ParkStatus FROM            Saledetails where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
            //            objCore.executeQuery(q);
            //            q = "update saledetails set quantity='" + newqty + "',price='" + oldprice + "',Itemdiscount='" + prevdiscount + "',ItemGst='" + prevgst + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
            //            objCore.executeQuery(q);
                        
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
        }

        private void button32_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " ";
        }
    }
}
