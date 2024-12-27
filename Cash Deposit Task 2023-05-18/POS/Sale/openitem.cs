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
    public partial class openitem : Form
    {
        private TextBox focusedTextbox = null;
        public string id = "0", kdid = "0";
        RestSale _frm;
        public openitem(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            _frm.TopMost = true;
            this.Close();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            if (txtname.Text == "")
            {
                MessageBox.Show("Please Enter Name");
                txtname.Focus();
                return;
            }
            if (txtprice.Text == "")
            {
                MessageBox.Show("Please Enter Price");
                txtprice.Focus();
                return;
            }
            
            if (txtprice.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtprice.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }

            _frm.fillgrid(id, "", "", txtprice.Text, "1", "New", "", "", txtname.Text, "", kdid, "", "", "", "", txtprice.Text);
            _frm.TopMost = true;
            this.Close();
        }

        private void txtprice_TextChanged(object sender, EventArgs e)
        {
            if (txtprice.Text.Trim() == string.Empty)
            {
                
            }

            if (txtprice.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtprice.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtprice.Focus();
                    return;
                }
            }
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

        private void txtprice_Enter(object sender, EventArgs e)
        {
            focusedTextbox = sender as TextBox;
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

        private void openitem_Load(object sender, EventArgs e)
        {
            try
            {
                string no = POSRestaurant.Properties.Settings.Default.MainScreenLocation.ToString();
                if (Convert.ToInt32(no) > 0)
                {


                    Screen[] sc;
                    sc = Screen.AllScreens;
                    this.StartPosition = FormStartPosition.Manual;
                    int no1 = Convert.ToInt32(no);
                    this.Location = Screen.AllScreens[no1].WorkingArea.Location;
                    this.WindowState = FormWindowState.Normal;
                    this.StartPosition = FormStartPosition.CenterScreen;
                    //this.WindowState = FormWindowState.Maximized;

                }

            }
            catch (Exception ex)
            {

            }
            this.TopMost = true;
        }
    }
}
