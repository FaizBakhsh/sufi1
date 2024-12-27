using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OposCashDrawer_CCO;
using VIBlend.WinForms.Controls;
namespace POSRestaurant.Sale
{
    public partial class changebill : Form
    {
        RestSale _frm;
        public int sid = 0;
        public string name = "";
        public changebill(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            //_frm.TopMost = true;
        }

        private void vButton16_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select * from selecttype ";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["selecttype"].ToString() == "yes")
                    {
                        POSRestaurant.Sale.NewBillTypenew obj = new NewBillTypenew(_frm);
                        obj.saleid = sid.ToString();
                        obj.rename = "yes";
                        obj.Show();
                        this.Close();
                    }
                    else
                        if (ds.Tables[0].Rows[0]["selecttype"].ToString() == "never")
                        {

                        }
                        else
                        {
                            POSRestaurant.forms.NewOrder obj = new forms.NewOrder(_frm);
                            obj.saleid = sid;
                            obj.name = name;
                            obj.Show();
                            this.Close();
                        }

                }
            }
            catch (Exception ex)
            {

            }

           
        }
        public string userid = "";
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void vButton2_Click(object sender, EventArgs e)
        {
            Button b1 = sender as Button;
            vButton b = sender as vButton;
            string authentication = objcore.authentication1(b.Text,userid);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            _frm.reopenbill();
            //_frm.TopMost = true;
            this.Close();
        }

        private void changebill_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
        }
    }
}
