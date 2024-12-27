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
    public partial class DiscountSelection : Form
    {
        private RestSale _frm1;
        public string cardno = "", saleid = "";
        public float limit = 0;
        public DiscountSelection(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DiscountKeys obj = new DiscountKeys(_frm1);
            obj.cardno = cardno;
            obj.saleid = saleid;
            obj.limit = limit;
            obj.Show();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VoucherKeys obj = new VoucherKeys(_frm1);
            obj.cardno = cardno;
            obj.saleid = saleid;
            obj.Show();
            this.Close();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
            _frm1.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_frm1.saleid == 0)
            {
                PointsRedeem obj = new PointsRedeem(_frm1);
                obj.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Please Click on New Bill on Sale Screen");
            }
        }
    }
}
