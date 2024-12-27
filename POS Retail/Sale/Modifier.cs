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
    public partial class Modifier : Form
    {
        private  Sale _frm1;

        public Modifier(Sale frm1)
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
          
               
                
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
         public void changtext(Button btn , string text)
        {
            btn.Text = text;
            btn.Text = text.Replace("&", "&&");
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "SELECT     dbo.Modifier.Id, dbo.Modifier.Name AS ModifierName, dbo.Modifier.Price, dbo.RawItem.ItemName AS name FROM         dbo.Modifier INNER JOIN                      dbo.RawItem ON dbo.Modifier.RawItemId = dbo.RawItem.Id where dbo.Modifier.menuitemid='" + id + "' ";
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
        public void callgrid(Button btn)
        {
            try
            {
                if (btn.Text != string.Empty)
                {
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet dscallgrid = new DataSet();
                    dscallgrid = objCore.funGetDataSet("SELECT     dbo.Modifier.Id, dbo.Modifier.Name AS ModifierName, dbo.Modifier.Price, dbo.RawItem.ItemName AS name FROM         dbo.Modifier INNER JOIN                     dbo.RawItem ON dbo.Modifier.RawItemId = dbo.RawItem.Id where dbo.RawItem.ItemName='" + btn.Text + "' and dbo.Modifier.Menuitemid='" + id + "'");
                    if (dscallgrid.Tables[0].Rows.Count > 0)
                    {
                        string coments = "";
                        if (richTextBox1.Text.Trim() != string.Empty)
                        {
                            coments = "(" + richTextBox1.Text.Trim() + ")";
                        }
                        //_frm1.fillgrid(id, dscallgrid.Tables[0].Rows[0]["id"].ToString(), dscallgrid.Tables[0].Rows[0]["Name"].ToString()+coments, dscallgrid.Tables[0].Rows[0]["price"].ToString(), "", "New","");
                        
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
            this.Close();
        }
    }
}
