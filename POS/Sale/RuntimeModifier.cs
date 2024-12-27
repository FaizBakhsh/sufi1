using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using VIBlend.Utilities;
namespace POSRestaurant.Sale
{
    public partial class RuntimeModifier : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public string menuitemid = "";
        private RestSale _frm1;
        public int indx = 0;
        public RuntimeModifier(RestSale frm)
        {
            InitializeComponent();
            _frm1 = frm;
            _frm1.Enabled = false;
        }
        int tcolms = 1, trows=1;
        public void getdata(  )
        {
            DataSet ds = new DataSet();
            string q = "SELECT     id, name, Itemid, price FROM         RuntimeModifier where status='active'";
            ds = objcore.funGetDataSet(q);
            for( int i=0; i<ds.Tables[0].Rows.Count;i++ )
            {
            
                vButton button = new vButton();
                button.VIBlendTheme = VIBLEND_THEME.BLACKPEARL;// "BLACKPEARL";
                button.Click += new EventHandler(button_Click);
                button.Font = new Font("", 13, FontStyle.Bold);
                button.Dock = DockStyle.Fill;
                button.Text = ds.Tables[0].Rows[i]["name"].ToString().Replace("&", "&&");
                button.Name = ds.Tables[0].Rows[i]["id"].ToString().Replace("&", "&&");
                button.TextWrap = true;
                button.ForeColor = Color.White;
                tableLayoutPanel1.Controls.Add(button, tcolms, trows);
                tcolms++;
            //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                if (tcolms > 6)
                {
                    tcolms = 0;
                    trows++;
                }
            }

        }
        protected void button_Click(object sender, EventArgs e)
        {
            callgrid(sender as vButton);
        }
        public void callgrid(vButton btn)
        {
            try
            {
                if (btn.Text != string.Empty)
                {
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet dscallgrid = new DataSet();
                    dscallgrid = objCore.funGetDataSet("SELECT  * from RuntimeModifier where id='" + btn.Name + "' and name='" + btn.Text + "'");
                    if (dscallgrid.Tables[0].Rows.Count > 0)
                    {
                        _frm1.fillgridruntimemodifier(menuitemid, "", btn.Text, dscallgrid.Tables[0].Rows[0]["price"].ToString(), "", "New", "", "", "", dscallgrid.Tables[0].Rows[0]["id"].ToString(), dscallgrid.Tables[0].Rows[0]["kdsid"].ToString(), indx);                        
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void RuntimeModifier_Load(object sender, EventArgs e)
        {
            int y = Screen.PrimaryScreen.Bounds.Bottom - this.Height;
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Right - this.Width, y);
           // this.TopMost = true;
            getdata();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton1_Click_2(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }
    }
}
