using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data.SqlClient;
using POSRetail.classes;

namespace POSRetail.forms
{
    public partial class KeyBoard : Form
    {
        private POSRetail.Sale.Sale _frm1;
        private TextBox focusedTextbox = null;
        public KeyBoard(POSRetail.Sale.Sale frm1)
        {
            InitializeComponent();
             _frm1 = frm1;
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            button2.Text = "!";
            shiftkey();
            if (POSRetail.Properties.Settings.Default.ConnectionString == string.Empty)
            {
                
            }
            else
            {
               // tabControl1.TabPages.RemoveAt(1);
            }
           

            //Form1 obj = new Form1();
            //obj.Show();
            //this.Hide();
        }

       

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
    

        }

        private void pbxCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void gbLogin_Enter(object sender, EventArgs e)
        {
           
        }

       //private void llConnection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       // {
       //     if (this.gbServer.Visible)
       //     {
       //         this.gbServer.Visible = false;
       //         this.txtUserId.Focus();
       //     }
       //     else
       //         this.gbServer.Visible = true;
       // }

        private void gbServer_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void gbLogin_Leave(object sender, EventArgs e)
        {
    
        }

      

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           // textBox1.Text = textBox1.Text + e.KeyChar.ToString().Trim();
            
        }
       
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button t = (sender) as Button;
            try
            {
                t = (sender) as Button;
                _frm1.filltextbox(t.Text.Replace("&&", "&"));

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;
                try
                {
                    t = (sender) as Button;
                    _frm1.filltextbox(t.Text);

                }
                catch (Exception ex)
                {

                    // MessageBox.Show(ex.Message);
                }
                
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
        }

        private void txtUserId_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }

        private void txtPassword_Enter(object sender, EventArgs e)
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
        private void button14_Click(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                _frm1.removetext();
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void txtServerLogin_Enter(object sender, EventArgs e)
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

        private void txtServer_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }

        private void txtServerPassword_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           // if (focusedTextbox.Name == "textBox4")
            {
                _frm1.keyboradbarcode();
            }
        }
   }
}
