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
    public partial class Comments : Form
    {
        private  RestSale _frm1;

        public Comments(RestSale frm1)
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
            this.Close();
        }
         public void changtext(vButton btn , string text)
        {
            btn.Text = text;
            btn.Text = text.Replace("&", "&&");
        }
        private void AddGroups_Load(object sender, EventArgs e)
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

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        public void callgrid(vButton btn)
        {
            try
            {
                if (btn.Text != string.Empty)
                {
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet dscallgrid = new DataSet();
                    dscallgrid = objCore.funGetDataSet("SELECT     dbo.Modifier.Id, dbo.Modifier.Name AS ModifierName, dbo.Modifier.Price, dbo.Modifier.GrossPrice, dbo.RawItem.ItemName AS name FROM         dbo.Modifier INNER JOIN                     dbo.RawItem ON dbo.Modifier.RawItemId = dbo.RawItem.Id where dbo.Modifier.Name='" + btn.Text + "' and dbo.Modifier.Menuitemid='" + id + "'");
                    if (dscallgrid.Tables[0].Rows.Count > 0)
                    {
                        string coments = "";
                        if (richTextBox1.Text.Trim() != string.Empty)
                        {
                            coments = "(" + richTextBox1.Text.Trim() + ")";
                        }
                        string val = "";
                        val = dscallgrid.Tables[0].Rows[0]["Price"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double prc = Convert.ToDouble(val);
                        val = dscallgrid.Tables[0].Rows[0]["GrossPrice"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double grossprc = Convert.ToDouble(val);
                        if (qty == 0)
                        {
                            qty = 1;
                        }
                        prc = prc * qty;
                        grossprc = grossprc * qty;
                        _frm1.fillgrid(id, dscallgrid.Tables[0].Rows[0]["id"].ToString(), dscallgrid.Tables[0].Rows[0]["ModifierName"].ToString() + coments, prc.ToString(), qty.ToString(), "New", "", "", coments, "", "", "", "", "", "", grossprc.ToString());
                        
                    } 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public string menuitem = "";
        private void vButton4_Click(object sender, EventArgs e)
        {
           // _frm1.fillgrid(id, dscallgrid.Tables[0].Rows[0]["id"].ToString(), dscallgrid.Tables[0].Rows[0]["ModifierName"].ToString() + coments, prc.ToString(), qty.ToString(), "New", "", "", coments, "", "");
            _frm1.comments(richTextBox1.Text);
            _frm1.Enabled = true;
           // _frm1.TopMost = true;
            this.Close();
        }
        public int qty=0;
        private void button18_Click_1(object sender, EventArgs e)
        {
            try
            {
                vButton btn = sender as vButton;
                callgrid(btn);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        public void shiftkey()
        {
            if (button49.Text != "!")
            {
                button49.Text = "!";
                button51.Text = "@";
                button53.Text = "#";
                button55.Text = "$";
                button57.Text = "%";
                button56.Text = "^";
                button54.Text = "&&";
                button52.Text = "*";
                button50.Text = "(";
                button48.Text = ")";

                button21.Text = "Q";
                button34.Text = "W";
                button43.Text = "E";
                button45.Text = "R";
                button47.Text = "T";
                button46.Text = "Y";
                button44.Text = "U";
                button42.Text = "I";
                button22.Text = "O";
                button20.Text = "P";

                

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
                button49.Text = "1";
                button51.Text = "2";
                button53.Text = "3";
                button55.Text = "4";
                button57.Text = "5";
                button56.Text = "6";
                button54.Text = "7";
                button52.Text = "8";
                button50.Text = "9";
                button48.Text = "0";

                button21.Text = "q";
                button34.Text = "w";
                button43.Text = "e";
                button45.Text = "r";
                button47.Text = "t";
                button46.Text = "y";
                button44.Text = "u";
                button42.Text = "i";
                button22.Text = "o";
                button20.Text = "p";

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
        private void button49_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

               // if (richTextBox1 != null)
                {

                    {
                        richTextBox1.Text = richTextBox1.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
       
        }

        private void button19_Click(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {

                int index = richTextBox1.SelectionStart;
                richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart - 1, 1);
                richTextBox1.Select(index - 1, 1);
                richTextBox1.Focus();
            }
            catch (Exception ex)
            {


            }

        }

        private void button58_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " ";
        }

        private void button19_Click_1(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button32_Click_1(object sender, EventArgs e)
        {
            try
            {

                int index = richTextBox1.SelectionStart;
                richTextBox1.Text = richTextBox1.Text.Remove(richTextBox1.SelectionStart - 1, 1);
                richTextBox1.Select(index - 1, 1);
                richTextBox1.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            _frm1.TopMost = true;
            _frm1.Enabled = true;
            this.Close();
        }
    }
}
