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
    public partial class Loyalitycard : Form
    {
        RestSale _frm;
        public Loyalitycard(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        protected void calfunc()
        {
            try
            {
                if (textBox1.Text.Trim().Length <= 0)
                {
                    return;
                }
                string q = "select * from LoyalityCards where cardno='" + textBox1.Text + "'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string disc = ds.Tables[0].Rows[0]["Discount"].ToString();
                    if (disc == "")
                    {
                        disc = "0";
                    }
                    float dis = float.Parse(disc);
                    _frm.cardid = ds.Tables[0].Rows[0]["id"].ToString();
                    _frm.discountkeys(dis.ToString(), "Loyality", "Loyality Card");
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    calfunc();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Loyalitycard_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void Loyalitycard_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.Focus();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            calfunc();
        }
    }
}
