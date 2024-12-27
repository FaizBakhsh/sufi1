using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.Sale
{
    public partial class CustomerId : Form
    {
        private TextBox focusedTextbox = null;
        private  RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore ;
        DataSet ds ;
        public string type = "";
        public CustomerId(RestSale frm1)
           {
                InitializeComponent();
                _frm1 = frm1;
                
            }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";
            
        //}
        string cs = POSRestaurant.Properties.Settings.Default.ConnectionString.ToString();
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
            button2.Text = "!";
            shiftkey();
            if (type == "takeaway")
            {
                label2.Text = "Take Away Customer Id";
            }
            if (type == "carhope")
            {
                label2.Text = "Carhope Customer Id";
            }
            _frm1.Enabled = false;
            this.TopMost = true;
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
                if (txtname.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Customer Id");
                    return;
                }
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                int idd = 0;
                ds = new DataSet();
                if (type == "takeaway")
                {
                    ds = objCore.funGetDataSet("select max(id) as id from TakeAway");
                }
                if (type == "carhope")
                {
                    ds = objCore.funGetDataSet("select max(id) as id from carhope");
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0][0].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    idd = Convert.ToInt32(i) + 1;
                }
                else
                {
                    idd = 1;
                }

                
                string date = "";
                DataSet dsdt = new DataSet();
                string q = "select top(1) * from dayend order by id desc";
                SqlConnection con = new SqlConnection(cs);
                try
                {
                    if (con.State == ConnectionState.Open)
                    { con.Close(); }
                    con.Open();
                    SqlCommand com = new SqlCommand(q, con);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dsdt);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
                //objCore = new classes.Clsdbcon();
                //dsdt = objCore.funGetDataSet(q);
                try
                {
                    if (dsdt.Tables[0].Rows.Count > 0)
                    {
                        date = dsdt.Tables[0].Rows[0]["Date"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    
                   
                }


                if (type == "takeaway")
                {
                    q = "insert into TakeAway (id,CustomerId,Date,time,Saleid,Status) values ('" + idd + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','','Pending')";
                    _frm1.takeawayid = idd.ToString();
                }
                if (type == "carhope")
                {
                    q = "insert into carhope (id,CustomerId,Date,Saleid,Status) values ('" + idd + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + date + "','','Pending')";
                    _frm1.carhopeid = idd.ToString();
                }
                objCore.executeQuery(q);

                _frm1.settableno("Customer ID: " + txtname.Text.Trim().Replace("'", "''"));
                _frm1.Enabled = true;
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            //_frm1.Islbldelivery = "Not Selected";
            _frm1.Enabled = true;
            _frm1.settableno("");
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;
                
                if (focusedTextbox != null)
                {

                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void txtname_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                
                int index = focusedTextbox.SelectionStart;
                focusedTextbox.Text = focusedTextbox.Text.Remove(focusedTextbox.SelectionStart - 1, 1);
                focusedTextbox.Select(index - 1, 1);
                focusedTextbox.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            focusedTextbox.Text = focusedTextbox.Text + " ";
        }
    }
}
