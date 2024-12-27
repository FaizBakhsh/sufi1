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
    public partial class DiscountKeys : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private RestSale _frm1;
        public string cardno = "";
        public string saleid = "", userid = "";
        public float limit = 0;
        public DiscountKeys(RestSale frm1)
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
            getkeys();
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
           string q = "SELECT     id, name, discount FROM         DiscountKeys  where status='active'";
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
                        changtext(button, ds.Tables[0].Rows[i]["Name"].ToString() + " (" + ds.Tables[0].Rows[i]["discount"].ToString() + " %)", "", "", "12", "");
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
            try
            {
               string query = "ALTER TABLE [dbo].[DiscountKeys]  ADD limit varchar(100) NULL ";
               objCore.executeQuery(query);

            }
            catch (Exception ex)
            {


            }
            vButton btn = (sender) as vButton;
            DataSet dschk = new DataSet();
            string q = "SELECT     id, name, discount,limit FROM         DiscountKeys where id='"+btn.Name+"'";
            dschk = objCore.funGetDataSet(q);
            if (dschk.Tables[0].Rows.Count > 0)
            {
                string discountlimit = dschk.Tables[0].Rows[0]["limit"].ToString().Trim();
                if (discountlimit.Length > 0 && _frm1.saleid==0)
                {
                    MessageBox.Show("Please Tender Bill Before Applying Discount");
                    return;
                }
                if (checkdis() == true)
                {
                    Discountdetails ob = new Discountdetails(_frm1);
                    ob.dis = dschk.Tables[0].Rows[0]["discount"].ToString();
                    ob.name = dschk.Tables[0].Rows[0]["name"].ToString();
                    ob.userid = getuser();
                    ob.Show();
                    this.Close();
                }
                else
                {
                    if (limit > 0)
                    {
                        if (limit < Convert.ToDouble(dschk.Tables[0].Rows[0]["discount"].ToString()))
                        {
                            MessageBox.Show("Your limit of applying discount is upto "+limit.ToString()+" %");
                            return;
                        }
                    }
                    _frm1.discountid = btn.Name;
                    _frm1.discountkeys(dschk.Tables[0].Rows[0]["discount"].ToString(), dschk.Tables[0].Rows[0]["name"].ToString(), richTextBox1.Text.Replace("&", "&&").Replace("'", "''"));
                    _frm1.updateorder("discount");
                    _frm1.disuser = getuser();                    
                    _frm1.Enabled = true;
                    // _frm1.TopMost = true;
                    this.Close();
                }
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

        private void button14_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " ";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " ";
        }
    }
}
