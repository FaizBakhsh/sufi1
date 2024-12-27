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
    public partial class CompDiscount : Form
    {
        private  RestSale _frm1;

        public CompDiscount(RestSale frm1)
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

        //public string Islbltotal
        //{
        //    get
        //    {
        //        return lbltotal.Text;
        //    }
        //    set
        //    {
        //        lbltotal.Text = value;
        //    }
        //}
        //public string Islblreceived
        //{
        //    get
        //    {
        //        return lblReceived.Text;
        //    }
        //    set
        //    {
        //        lblReceived.Text = value;
        //    }
        //}
        //public string Islblchange
        //{
        //    get
        //    {
        //        return lblchange.Text;
        //    }
        //    set
        //    {
        //        lblchange.Text = value;
        //    }
        //}

        private void AddGroups_Load(object sender, EventArgs e)
        {
            vButton2.Focus();
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (vTextBox1.Text == string.Empty || vTextBox1.Text == "0")
                {
                    MessageBox.Show("Please Enter Discount Value");
                    return;
                }
                if (vTextBox2.Text == string.Empty)
                {
                    MessageBox.Show("Please Provide a reason for Discount");
                    return;
                }
                _frm1.compdiscount(vTextBox1.Text);
                _frm1.Enabled = true;
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
                this.Close();
            }
        }

        private void vTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void vTextBox1_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (float.Parse(vTextBox1.Text.Trim()) > 100)
                {
                    vTextBox1.Text = "100";
                }
                if (vTextBox1.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(vTextBox1.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Discount Value. Only Nymbers are allowed");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        
    }
}
