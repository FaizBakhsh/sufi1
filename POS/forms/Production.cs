using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class Production : Form
    {
        public Production()
        {
            InitializeComponent();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           POSRestaurant.Setting.AddRecipeProduction obj = new  Setting.AddRecipeProduction();

            obj.Show();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            POSRestaurant.RawItems.Production onj = new RawItems.Production();

            onj.Show();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmProduction onj = new POSRestaurant.Reports.Inventory.frmProduction();
            onj.Show();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            POSRestaurant.RawItems.Production onj = new RawItems.Production();
            
            onj.Show();

        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.FrmreceipeProduction onj = new POSRestaurant.Reports.Inventory.FrmreceipeProduction();
            onj.Show();
        }
    }
}
