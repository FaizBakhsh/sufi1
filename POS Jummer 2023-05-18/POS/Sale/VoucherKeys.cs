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
    public partial class VoucherKeys : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private RestSale _frm1;
        public string cardno = "";
        public string saleid = "";
        public VoucherKeys(RestSale frm1)
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
            this.TopMost = true;
            getkeys();
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
        public void changtext(Button btn, string text, string color, string img, string fontsize, string fontcolor)
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
           string q = "SELECT     id, name, Amount FROM         VoucherKeys  where status='active'";
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
                        changtext(button, ds.Tables[0].Rows[i]["Name"].ToString() + " (" + ds.Tables[0].Rows[i]["Amount"].ToString() + ")", "", "", "12", "");
                        Addbutton(button);
                        //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }
                    
                }
            }
        }
        protected bool checkdis()
        {
            bool chk = false;
            try
            {
                string q = "select status from DiscountDetailsEnable";
                DataSet dschk = new DataSet();
                dschk = objCore.funGetDataSet(q);
                if (dschk.Tables[0].Rows.Count > 0)
                {
                    string temp = dschk.Tables[0].Rows[0][0].ToString();
                    if (temp.ToLower() == "enabled")
                    {
                        chk = true;
                    }
                }
            }
            catch (Exception ex)
            {
                
               
            }
            return chk;
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            vButton btn = (sender) as vButton;
            DataSet dschk = new DataSet();
            string q = "SELECT     id, name, Amount FROM         VoucherKeys where id='" + btn.Name + "'";
            dschk = objCore.funGetDataSet(q);
            if (dschk.Tables[0].Rows.Count > 0)
            {
                
                _frm1.disuser = getuser();
                _frm1.voucherid = btn.Name;
                _frm1.voucheramount =float.Parse(dschk.Tables[0].Rows[0]["Amount"].ToString());
                _frm1.Enabled = true;
                _frm1.vouchernarration = richTextBox1.Text.Replace("'", "''").Replace("&&", "n");
                _frm1.voucherkeys();
                // _frm1.TopMost = true;
                this.Close();

            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            //_frm1.TopMost = true;
           // _frm1.Enabled = true;
            this.Close();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " ";
        }

        private void vButton2_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
