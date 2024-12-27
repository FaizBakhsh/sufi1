using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using VIBlend;
using VIBlend.Utilities.Properties;
using VIBlend.WinForms.Controls.Properties;
using VIBlend.Utilities;
namespace POSRestaurant.Sale
{
    public partial class Reasons : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private RestSale _frm1;
        public string cardno = "",type="";
        public Reasons(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }
        public string getuser()
        {
            string userid = "";
            DataSet dsuser = new DataSet();
            string q = "select id from users where cardno='"+cardno+"'";
            dsuser = objCore.funGetDataSet(q);
            if (dsuser.Tables[0].Rows.Count > 0)
            {
                userid = dsuser.Tables[0].Rows[0][0].ToString();
            }
            return userid;
        }
        private void DiscountKeys_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            getkeys();
        }
        public void changtext(Button btn, string text)
        {

            try
            {
                btn.Text = text;
                btn.Text = text.Replace("&", "&&");
                btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                // string path = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                //string path = Application.StartupPath + "\\Resources\\ButtonIcons\\";
                btn.Font = new Font("", 12, FontStyle.Bold);
                btn.ForeColor = Color.White;                                               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        int tcolms = 0, trows=0;
        private void Addbutton(Button btn)
        {
            //// panel7.SuspendLayout();
            btn.Dock = DockStyle.Fill;


            tableLayoutPanel1.Controls.Add(btn, tcolms, trows);
            tcolms++;
            //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            if (tcolms >= tableLayoutPanel1.ColumnCount)
            {
                tcolms = 0;
                trows++;
            }
            // panel7.ResumeLayout(false);
        }
        public void getkeys()
        {
           
            objCore = new classes.Clsdbcon();
           DataSet ds = new DataSet();
           string q = "SELECT     id, Title FROM         Reasons";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // if (i == 0)
                    {
                        vButton button = new vButton();
                        button.VIBlendTheme = VIBLEND_THEME.NERO;// "BLACKPEARL";
                        button.Click += new EventHandler(vButton2_Click);
                        button.TextWrap = true;
                        button.Name = ds.Tables[0].Rows[i]["id"].ToString();
                        changtext(button, ds.Tables[0].Rows[i]["Title"].ToString());
                        Addbutton(button);
                        //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }
                    
                }
            }
        }
        public string id = "";
        private void vButton2_Click(object sender, EventArgs e)
        {
            vButton btn = (sender) as vButton;
            VoidStatus obj = new VoidStatus(_frm1);
            obj.type = type;
            obj.id = id;
            obj.reason = btn.Text;
            obj.Show();
           
            //vButton btn = (sender) as vButton;
            //if (type == "voidall")
            //{
            //    _frm1.voidAllitems(id, btn.Text);
            //}
            //else
            //{
            //    _frm1.callvoiditems(btn.Text);
            //    //_frm1.voiditems(id, btn.Text);
            //    _frm1.Enabled = true;
            //}
            this.Close();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           
            _frm1.Enabled = true;
            this.Close();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;



                {
                    richTextBox1.Text = richTextBox1.Text + t.Text.Replace("&&", "&");
                }



            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }
        private void vButton2_Click_1(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " ";
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            
            VoidStatus obj = new VoidStatus(_frm1);
            obj.type = type;
            obj.id = id;
            obj.reason = richTextBox1.Text;
            obj.Show();
            this.Close();
        }
    }
}
