using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms;
using VIBlend.WinForms.Controls;
using VIBlend.WinForms.Controls.Properties;
using VIBlend;
using VIBlend.Utilities;
namespace POSRestaurant.Sale
{
    public partial class CashTransaction : Form
    {
        private RestSale _frm1;
        private vTextBox focustedtextbox=null;
        public CashTransaction(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            _frm1.Enabled = false;
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            vTextBox1.Text = "0";
            vTextBox2.Text = "0";
            vTextBox3.Text = "";
        }

        private void vTextBox1_Enter_1(object sender, EventArgs e)
        {
            focustedtextbox = (sender) as vTextBox;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

                if (focustedtextbox != null)
                {

                    {
                        focustedtextbox.Text = focustedtextbox.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                int index = focustedtextbox.SelectionStart;
                focustedtextbox.Text = focustedtextbox.Text.Remove(focustedtextbox.SelectionStart - 1, 1);
               // focustedtextbox.Select(index - 1, 1);
                focustedtextbox.Focus();
            }
            catch (Exception ex)
            {


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

        private void vButton3_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public string date = "";
        private void vButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (vTextBox1.Text.Trim() == string.Empty || vTextBox2.Text.Trim() == string.Empty || vTextBox2.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please fill All Fields");
                    return;
                }
                DataSet dss = new DataSet();
                int idd = 0;
                dss = objcore.funGetDataSet("select max(id) as id from CashTransactions");
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
                string q = "insert into CashTransactions(Id,date,cashin,cashout,Description) values('" + idd + "','" + date + "','" + vTextBox1.Text.Trim().Replace("'", "''") + "','" + vTextBox2.Text.Trim().Replace("'", "''") + "','" + vTextBox3.Text.Trim().Replace("'", "''") + "')";
                objcore.executeQuery(q);
                MessageBox.Show("Data Saved Successfully");
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void vTextBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (focustedtextbox.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(focustedtextbox.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }
        }
    }
}
