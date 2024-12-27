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
    public partial class SaleMessage : Form
    {
        private TextBox focusedTextbox = null;
        private  Sale _frm1;

        public SaleMessage(Sale frm1)
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

        public string Islbltotal
        {
            get
            {
                return lbltotal.Text;
            }
            set
            {
                lbltotal.Text = value;
            }
        }
        public string Islblreceived
        {
            get
            {
                return lblReceived.Text;
            }
            set
            {
                lblReceived.Text = value;
            }
        }
        public string Islblchange
        {
            get
            {
                return lblchange.Text;
            }
            set
            {
                lblchange.Text = value;
            }
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            textBox1.Focus();

        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();


                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CustomerInfo");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        id = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        id = 1;
                    }
                    ds = new DataSet();
                    string q = "select * from CustomerInfo where Mobile='" + textBox3.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Mobile No already exist");
                       
                    }
                    else
                    {
                        q = "insert into CustomerInfo (id,Name,Email,Mobile,City,uploadstatus) values('" + id + "','" + textBox1.Text.Trim().Replace("'", "''") + "','" + textBox2.Text.Trim().Replace("'", "''") + "','" + textBox3.Text.Trim().Replace("'", "''") + "','" + textBox4.Text.Trim().Replace("'", "''") + "','Pending')";
                        objCore.executeQuery(q);
                    }
                }
            }
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
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
        private void button2_Click(object sender, EventArgs e)
        {
            Button t = (sender) as Button;
            try
            {
                t = (sender) as Button;
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

        
    }
}
