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
    public partial class BillRecall : Form
    {
        private  RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore ;
        DataSet ds ;
        public BillRecall(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            _frm1.Enabled = false;
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
               // q9 = "SELECT     Id as Bill_No, Date, time, NetBill, OrderType   FROM         Sale where userid='" + id + "' and BillStatus='Pending' order by id desc";
                //q9 = "SELECT     dbo.Sale.Id as Bill_No, dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.DinInTables.TableNo, dbo.ResturantStaff.Name as Waiter_Name FROM         dbo.ResturantStaff INNER JOIN                      dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId RIGHT OUTER JOIN                      dbo.Sale ON dbo.DinInTables.Saleid = dbo.Sale.Id where  dbo.Sale.BillStatus='Pending' order by dbo.Sale.id asc";
                q9 = "SELECT     dbo.Sale.Id AS Bill_No, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.DinInTables.TableNo, dbo.ResturantStaff.Name AS Waiter_Name,                       dbo.TakeAway.CustomerId FROM         dbo.TakeAway RIGHT OUTER JOIN                      dbo.Sale ON dbo.TakeAway.Saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.ResturantStaff INNER JOIN                      dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.BillStatus='Pending' order by  dbo.Sale.id asc";
               
                ds9 = objCore.funGetDataSet(q9);
                DataTable dt = new DataTable();
                dt = ds9.Tables[0];
               
                // dataGridView1.Columns[0].Visible = false;
                // dataGridView1.Columns[3].Visible = false;
                bool chk = false;
                foreach (DataRow dr in dt.Rows)
                {
                    //dr.Height = 40;
                    //string iddd = dr[0].ToString();
                    //ds = new DataSet();
                    //ds = objCore.funGetDataSet("select Status from saledetails where saleid='" + iddd + "'");
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    //    {
                    //        if (ds.Tables[0].Rows[j]["Status"].ToString() == "Not Void")
                    //        {
                    //            chk = true;
                    //        }
                    //    }
                    //}
                    //if (chk == true)
                    //{
                    //    chk = false;
                    //}
                    //else
                    //{

                    //    dr.Delete();
                    //}


                }
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
                    string type = dataGridView1.Rows[indx].Cells[4].Value.ToString();
                    //_frm1.Islbldelivery = type;
                    _frm1.recalsale(saleid,"no");
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
                //q9 = "SELECT     Id as Bill_No, Date, time, NetBill, OrderType   FROM         Sale where id='" + txtbill.Text.Trim() + "' and userid='" + id + "' and BillStatus='Pending'";
               // q9 = "SELECT     dbo.Sale.Id as Bill_No, dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.DinInTables.TableNo, dbo.ResturantStaff.Name as Waiter_Name FROM         dbo.ResturantStaff INNER JOIN                      dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId RIGHT OUTER JOIN                      dbo.Sale ON dbo.DinInTables.Saleid = dbo.Sale.Id where dbo.Sale.id='" + txtbill.Text.Trim() + "' and dbo.Sale.BillStatus='Pending' order by  dbo.Sale.id asc";
                q9 = "SELECT    dbo.Sale.Id AS Bill_No, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.DinInTables.TableNo, dbo.ResturantStaff.Name AS Waiter_Name,                       dbo.TakeAway.CustomerId FROM         dbo.TakeAway RIGHT OUTER JOIN                      dbo.Sale ON dbo.TakeAway.Saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.ResturantStaff INNER JOIN                      dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.id='" + txtbill.Text.Trim() + "' and dbo.Sale.BillStatus='Pending' order by  dbo.Sale.id asc";
               
                
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

                if (t.Text == "." && txtbill.Text.Contains("."))
                {

                }
                else
                {                   
                    txtbill.Text = txtbill.Text + t.Text.Replace("&&", "&");                                        
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }
        public void shiftkey()
        {
            if (button2.Text != "!")
            {
                button2.Text = "!";
                button3.Text = "@";
                button4.Text = "#";
                button5.Text = "$";
                button6.Text = "%";
                button7.Text = "^";
                button8.Text = "&&";
                button9.Text = "*";
                button10.Text = "(";
                button11.Text = ")";
                button12.Text = "Q";
                button16.Text = "W";
                button18.Text = "E";
                button20.Text = "R";
                button22.Text = "T";
                button21.Text = "Y";
                button19.Text = "U";
                button17.Text = "I";
                button15.Text = "O";
                button13.Text = "P";

                button23.Text = "A";
                button25.Text = "S";
                button27.Text = "D";
                button29.Text = "F";
                button31.Text = "G";
                button30.Text = "H";
                button28.Text = "J";
                button26.Text = "K";
                button24.Text = "L";

                button33.Text = "Z";
                button35.Text = "X";
                button37.Text = "C";
                button39.Text = "V";
                button41.Text = "B";
                button40.Text = "N";
                button38.Text = "M";
                // button36.Text = "o";


            }
            else
            {
                button2.Text = "1";
                button3.Text = "2";
                button4.Text = "3";
                button5.Text = "4";
                button6.Text = "5";
                button7.Text = "6";
                button8.Text = "7";
                button9.Text = "8";
                button10.Text = "9";
                button11.Text = "0";
                button12.Text = "q";
                button16.Text = "w";
                button18.Text = "e";
                button20.Text = "r";
                button22.Text = "t";
                button21.Text = "y";
                button19.Text = "u";
                button17.Text = "i";
                button15.Text = "o";
                button13.Text = "p";

                button23.Text = "a";
                button25.Text = "s";
                button27.Text = "d";
                button29.Text = "f";
                button31.Text = "g";
                button30.Text = "h";
                button28.Text = "j";
                button26.Text = "k";
                button24.Text = "l";

                button33.Text = "z";
                button35.Text = "x";
                button37.Text = "c";
                button39.Text = "v";
                button41.Text = "b";
                button40.Text = "n";
                button38.Text = "m";


            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {

                int index = txtbill.SelectionStart;
                txtbill.Text = txtbill.Text.Remove(txtbill.SelectionStart - 1, 1);
                txtbill.Select(index - 1, 1);
                txtbill.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txtbill.Text = txtbill.Text + " ";
        }
    }
}
