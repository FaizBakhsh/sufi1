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
    public partial class SaleMessage : Form
    {
        private  RestSale _frm1;

        public SaleMessage(RestSale frm1)
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

        public string Islbltotal
        {
            get
            {
                return lbltotal.Text;
            }
            set
            {
                lbltotal.Text = value;
            }
        }
        public string Islblreceived
        {
            get
            {
                return lblReceived.Text;
            }
            set
            {
                lblReceived.Text = value;
            }
        }
        public string Islblchange
        {
            get
            {
                return lblchange.Text;
            }
            set
            {
                lblchange.Text = value;
            }
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            vButton2.Focus();
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        
    }
}
