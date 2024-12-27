using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls.Properties;
using VIBlend.WinForms.Controls;
using VIBlend.Utilities.Properties;
namespace POSRestaurant.Sale
{
    public partial class ModifierFlaour : Form
    {
        private  RestSale _frm1;
        public string menuitemid = "";
        public ModifierFlaour(RestSale frm1)
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

        private void button1_Click(object sender, EventArgs e)
        {
          
               
                
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "";//"select * from Users where usertype='Admin' and UserName='"+txtname.Text+"' and password='"+txtpassword.Text.Trim()+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        _frm1.IsTextBoxEnabled= true;
                       // _frm1.Controls["txtdiscount"].Enabled = true;
                        //Sale form = (Sale)Application.OpenForms["Sale"];
                        
                        //Sale obj = new Sale();
                        //obj.txtdiscount.Enabled=true;//.discount();
                        //obj.editmode = "1";
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("invalid User Name or Password");
                        return;
 
                    }
                    
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
         public int quantity = 0;
        private void AddGroups_Load(object sender, EventArgs e)
        {
            _frm1.Enabled = false;
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "SELECT  * from ModifierFlavour where MenuItemId='" + menuitemid + "' ";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        if (j == 0)
                        {
                            changtext(button1, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 1)
                        {
                            changtext(button2, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 2)
                        {
                            changtext(button3, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 3)
                        {
                            changtext(button4, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 4)
                        {
                            changtext(button5, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 5)
                        {
                            changtext(button6, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 6)
                        {
                            changtext(button7, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 7)
                        {
                            changtext(button8, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 8)
                        {
                            changtext(button9, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 9)
                        {
                            changtext(button10, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 10)
                        {
                            changtext(button11, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 11)
                        {
                            changtext(button12, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 12)
                        {
                            changtext(button13, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 13)
                        {
                            changtext(button14, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 14)
                        {
                            changtext(button15, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 15)
                        {
                            changtext(button16, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 16)
                        {
                            changtext(button17, ds.Tables[0].Rows[j]["Name"].ToString());
                        }
                        if (j == 17)
                        {
                            changtext(button18, ds.Tables[0].Rows[j]["Name"].ToString());
                        }

                    }
                }
            }
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
                    dscallgrid = objCore.funGetDataSet("SELECT  * from ModifierFlavour where MenuItemId='" + menuitemid + "' and name='"+btn.Text+"'");
                    if (dscallgrid.Tables[0].Rows.Count > 0)
                    {
                        string coments = "";
                        string val = dscallgrid.Tables[0].Rows[0]["price"].ToString();
                        if (val == "")

                        {
                            val = "0";
                        }
                        double prce = Convert.ToDouble(val);
                        val = dscallgrid.Tables[0].Rows[0]["grossprice"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double grossprce = Convert.ToDouble(val);
                        if (quantity == 0)
                        {
                            quantity = 1;
                        }
                        prce = prce * quantity;
                        try
                        {
                            grossprce = grossprce * quantity;
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        _frm1.fillgrid(menuitemid, "", dscallgrid.Tables[0].Rows[0]["Name"].ToString() + coments, prce.ToString(), quantity.ToString(), "New", "", dscallgrid.Tables[0].Rows[0]["id"].ToString(), "", "", dscallgrid.Tables[0].Rows[0]["kdsid"].ToString(), "", "", "", "",grossprce.ToString());
                        _frm1.Enabled = true;
                        this.Close();
                        _frm1.modifier(menuitemid, quantity);
                    } 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            callgrid(button1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            callgrid(button2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            callgrid(button3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            callgrid(button4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            callgrid(button5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            callgrid(button6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            callgrid(button7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            callgrid(button8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            callgrid(button9);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            callgrid(button10);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            callgrid(button11);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            callgrid(button12);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            callgrid(button13);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            callgrid(button14);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            callgrid(button15);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            callgrid(button16);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            callgrid(button17);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            callgrid(button18);
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
           
        }

        private void button1_Click_2(object sender, EventArgs e)
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
    }
}
