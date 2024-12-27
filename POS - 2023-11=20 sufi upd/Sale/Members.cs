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
    public partial class Members : Form
    {
        private TextBox focusedTextbox = null;
        private  RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore=new classes.Clsdbcon() ;
        DataSet ds ;
        public Members(RestSale frm1)
           {
                InitializeComponent();
                _frm1 = frm1;
                _frm1.Enabled = false;
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
            DataSet ds = new DataSet();
            string q = "select * from customers where msr='" + txtmsr.Text.Trim() + "' or cnic='"+txtcnic.Text.Trim()+"'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                _frm1.memberid = ds.Tables[0].Rows[0]["id"].ToString();
                _frm1.memberinfo();
                this.Close();
            }
            else
            {
                MessageBox.Show("Inavlid Member Info");
                return;
            }
            
            
            
           
            
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
        public bool msr = true;
        public bool cnic = true;
        private void txtorderno_TextChanged(object sender, EventArgs e)
        {
            if (msr == true)
            {
                DataSet ds = new DataSet();
                string q = "select * from customers where msr='" + txtmsr.Text.Trim() + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cnic = false;
                    txtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtcompany.Text = ds.Tables[0].Rows[0]["company"].ToString();
                    txtphone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    txtcity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                    txtcnic.Text = ds.Tables[0].Rows[0]["cnic"].ToString();
                    cnic = true;
                    msr = true;
                }
                else
                {
                    cnic = false;
                    txtname.Text = string.Empty;
                    txtcompany.Text = string.Empty;
                    txtphone.Text = string.Empty;
                    txtcity.Text = string.Empty;
                    txtcnic.Text = string.Empty;
                    cnic = true;
                    msr = true;
                }
            }
        }

        private void txtcnic_TextChanged(object sender, EventArgs e)
        {
            if (cnic == true)
            {
                DataSet ds = new DataSet();
                string q = "select * from customers where cnic='" + txtcnic.Text.Trim() + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    msr = false;
                    txtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtcompany.Text = ds.Tables[0].Rows[0]["company"].ToString();
                    txtphone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    txtcity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                    txtmsr.Text = ds.Tables[0].Rows[0]["msr"].ToString();
                    cnic = true;
                    msr = true;
                }
                else
                {
                    msr = false;
                    txtname.Text = string.Empty;
                    txtcompany.Text = string.Empty;
                    txtphone.Text = string.Empty;
                    txtcity.Text = string.Empty;
                    txtcnic.Text = string.Empty;
                    cnic = true;
                    msr = true;
                }
            }
        }
    }
}
