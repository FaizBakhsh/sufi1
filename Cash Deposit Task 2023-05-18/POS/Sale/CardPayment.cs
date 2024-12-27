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
    public partial class CardPayment : Form
    {
        private vTextBox focusedTextbox = null;
        private RestSale _frm1;
        public string type = "";
       public double total = 0;
       public CardPayment(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            _frm1.Enabled = false;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
       
        private void vTextBox1_Click(object sender, EventArgs e)
        {

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
                    if (vTextBox4.Text != string.Empty)
                    {
                        try
                        {
                            if (type == "Credit Card")
                            {
                                _frm1.creditcardsale(vTextBox4.Text);
                            }
                            if (type == "Master Card")
                            {
                                _frm1.mastercardsale(vTextBox4.Text);
                            }


                            _frm1.savecardinfo(vTextBox4.Text);
                            _frm1.Enabled = true;
                            this.Close();
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }
    }
}
