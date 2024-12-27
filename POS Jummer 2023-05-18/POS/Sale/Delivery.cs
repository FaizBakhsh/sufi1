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
    public partial class Delivery : Form
    {
        private TextBox focusedTextbox = null;
        private  RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore ;
        DataSet ds ;
        public Delivery(RestSale frm1)
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
            if (txtorderno.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Order No");
                txtorderno.Focus();
                return;
            }
            if (txtname.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Name");
                txtname.Focus();
                return;
            }
            if (txtphone.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Phone No");
                txtphone.Focus();
                return;
            }
            if (txtaddress.Text == string.Empty)
            {
                MessageBox.Show("Please Enter Address");
                txtaddress.Focus();
                return;
            }
            
            objCore = new classes.Clsdbcon();
            ds = new DataSet();
            int idd = 0;
            ds = new DataSet();
            ds = objCore.funGetDataSet("select max(id) as id from Delivery");
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
            ds = new DataSet();
            ds = objCore.funGetDataSet("select top(1) * from dayend where userid='" + id + "' order by id desc");
            if (ds.Tables[0].Rows.Count > 0)
            {
                date = ds.Tables[0].Rows[0]["Date"].ToString();
            }
            string q = "insert into Delivery (id,OrderNo,Name,Phone,PostalCode,Address,Note,SaleId,Status,date) values ('" + idd + "','" + txtorderno.Text.Replace("'", "''") + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtpostalcode.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + txtnote.Text.Trim().Replace("'", "''") + "','','Pending','" + date + "')";
            objCore.executeQuery(q);
            _frm1.deliveryid = idd.ToString();
            _frm1.Enabled = true;
            this.Close();
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            //_frm1.Islbldelivery = "Not Selected";
            _frm1.Enabled = true;
            this.Close();

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

        private void button14_Click(object sender, EventArgs e)
        {
            shiftkey();
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
        private void txtorderno_Enter(object sender, EventArgs e)
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

        private void txtaddress_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            focusedTextbox.Text = focusedTextbox.Text + " ";
        }
    }
}
