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
    public partial class ManualQty : Form
    {
        private TextBox focusedTextbox = null;
        private  RestSale _frm1;
        public string saleid = "", menuitemid = "", name = "",shiftid="";
        Button _btn;
          public string cashrr;
          public string datee;
          public string useridd;

          public ManualQty(RestSale frm1,Button btn)
          {
              InitializeComponent();
              _frm1 = frm1;
              _btn = btn;
          }
       

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
            this.TopMost = true;
            txtcard.Focus();
           
            
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        public string reason = "";
      
        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcard.Text = string.Empty;
            //txtname.Text = string.Empty;
            //txtpassword.Text = string.Empty;
        }

        private void txtcard_KeyPress(object sender, KeyPressEventArgs e)
        {
            //txtcard.Text =txtcard.Text+ e.KeyChar.ToString().Trim();
            //e.Handled = false;
        }

        private void txtcard_TextChanged(object sender, EventArgs e)
        {
            //carlogin();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {

            _frm1.Enabled = true;
            //_frm1.TopMost = true;
            this.Close();
        }

        private void txtcard_Enter(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;



                if (t.Text == ".")
                {
                    if (txtcard.Text.Length == 0)
                    {
                        return;
                    }
                    if (txtcard.Text.Contains("."))
                    {
                        return;
                    }
                }
                else
                {
                    float.Parse(t.Text);
                }
                txtcard.Text = txtcard.Text + t.Text.Replace("&&", "&");



            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

           
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

        private void txtcard_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                _frm1.callfillgrid(_btn, float.Parse(txtcard.Text));
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Invalid Quantity",ex.Message);
            }
        }
    }
}
