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
    public partial class PartialPayment : Form
    {
        private vTextBox focusedTextbox = null;
        private RestSale _frm1;
       public double total = 0;
        public PartialPayment(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            _frm1.Enabled = false;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (vTextBox1.Text == string.Empty)
                { }
                else
                {
                    double Num;
                    bool isNum = double.TryParse(vTextBox1.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        vTextBox1.Focus();
                        return;
                    }
                }
                if (vTextBox2.Text == string.Empty)
                { }
                else
                {
                    double Num;
                    bool isNum = double.TryParse(vTextBox2.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        vTextBox2.Focus();
                        return;
                    }
                }
                if (vTextBox3.Text == string.Empty)
                { }
                else
                {
                    double Num;
                    bool isNum = double.TryParse(vTextBox3.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        vTextBox3.Focus();
                        return;
                    }
                }
                string val = "", val1 = "", val2 = "";
                if (vTextBox1.Text == "")
                {
                    val = "0";
                }
                else
                {
                    val = vTextBox1.Text;
                }
                if (vTextBox2.Text == "")
                {
                    val1 = "0";
                }
                else
                {
                    val1 = vTextBox2.Text;
                }
                if (vTextBox3.Text == "")
                {
                    val2 = "0";
                }
                else
                {
                    val2 = vTextBox3.Text;
                }
                double billamount = 0;
                billamount = Convert.ToDouble(val) + Convert.ToDouble(val1) + Convert.ToDouble(val2);
                if (billamount != total)
                {
                    MessageBox.Show("Total bill is " + total.ToString() + " and you entered " + billamount.ToString());
                    return;
                }
               
                _frm1.partialsale();
                string saletype = "";
                if (vTextBox1.Text == string.Empty)
                { }
                else
                {
                    saletype = "Cash";
                    _frm1.calbilltype("Cash",vTextBox1.Text);
                }
                if (vTextBox2.Text == string.Empty)
                { }
                else
                {
                    saletype = saletype + "+ Credit Card";
                    _frm1.calbilltype("Credit Card", vTextBox2.Text);
                }
                if (vTextBox3.Text == string.Empty)
                { }
                else
                {
                    saletype = saletype + "+ Masret Card";
                    _frm1.calbilltype("Masret Card", vTextBox3.Text);
                }


                _frm1.partialreport(saletype, vTextBox4.Text);
                _frm1.savecardinfo(vTextBox4.Text);
                _frm1.partialclear();
                _frm1.Enabled = true;
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
       
        private void vTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void vTextBox1_TextChanged(object sender, EventArgs e)
        {


            vTextBox txt = (sender) as vTextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {
                double Num;
                bool isNum = double.TryParse(txt.Text.ToString(), out Num); //c is your variable
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

        private void vButton2_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }

        private void vTextBox1_Enter(object sender, EventArgs e)
        {

            focusedTextbox = sender as vTextBox;
        }

        private void vButton20_Click(object sender, EventArgs e)
        {
            try
            {
                vButton btn = sender as vButton;
                if (focusedTextbox != null)
                {
                    focusedTextbox.Text = focusedTextbox.Text + btn.Text;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton31_Click(object sender, EventArgs e)
        {
            vTextBox1.Text = ""; vTextBox2.Text = ""; vTextBox3.Text = "";
        }

        private void vButton32_Click(object sender, EventArgs e)
        {
            if (focusedTextbox.Text != null)
            {
                focusedTextbox.Text = focusedTextbox.Text.Substring(0, focusedTextbox.Text.Length - 1);
                if (focusedTextbox.Text == string.Empty)
                {
                   
                }
            }
        }

        private void vTextBox4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string val = vTextBox4.Text;
                    int strt = val.IndexOf(";");
                    int end = val.IndexOf("=", strt);
                    int length = (end - 1) - (strt);
                    val = val.Substring(strt + 1, length);
                    vTextBox4.Text = val;
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
    }
}
