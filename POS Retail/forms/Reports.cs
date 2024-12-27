using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.forms
{
    public partial class Reports : Form
    {
        POSRetail.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public Reports()
        {
            InitializeComponent();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {

        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmMwnuGroupSale obj = new POSRetail.Reports.SaleReports.FrmMwnuGroupSale();
            obj.Show();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmMenuItemSale obj = new POSRetail.Reports.SaleReports.FrmMenuItemSale();
            obj.Show();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmOrderWiseSale obj = new POSRetail.Reports.SaleReports.FrmOrderWiseSale();
            obj.Show();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmCashierSale obj = new POSRetail.Reports.SaleReports.FrmCashierSale();
            obj.Show();
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmTerminalSale obj = new POSRetail.Reports.SaleReports.FrmTerminalSale();
            obj.Show();
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmPaymentWiseSale obj = new POSRetail.Reports.SaleReports.FrmPaymentWiseSale();
            obj.Show();
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmDiscountSale obj = new POSRetail.Reports.SaleReports.FrmDiscountSale();
            obj.Show();
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmVoidSale obj = new POSRetail.Reports.SaleReports.FrmVoidSale();
            obj.Show();
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmRefundSale obj = new POSRetail.Reports.SaleReports.FrmRefundSale();
            obj.Show();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.SaleReports.FrmHourlySale obj = new POSRetail.Reports.SaleReports.FrmHourlySale();
            obj.Show();
        }

        private void vButton11_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.FrmUserSale obj = new POSRetail.Reports.FrmUserSale();
            obj.Show();
        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.Inventory.frmReceivedInventory obj = new POSRetail.Reports.Inventory.frmReceivedInventory();
            obj.Show();
        }

        private void vButton13_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.Inventory.frmClosingInventory obj = new POSRetail.Reports.Inventory.frmClosingInventory();
            obj.Show();
        }

        private void vButton14_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.Inventory.frmConsumedInventory obj = new POSRetail.Reports.Inventory.frmConsumedInventory();
            obj.Show();
        }

        private void vButton8_Click_1(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.Accounts.frmProfitLoss obj = new  POSRetail.Reports.Accounts.frmProfitLoss();
            obj.Show();
        }

        private void vButton3_Click_1(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.Inventory.frmReceivedInventorySupplier obj = new POSRetail.Reports.Inventory.frmReceivedInventorySupplier();
            obj.Show();
        }

        private void vButton12_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.Inventory.frmReceivedInventoryBrand obj = new POSRetail.Reports.Inventory.frmReceivedInventoryBrand();
            obj.Show();
        }

        private void vButton16_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Reports.Inventory.frmInventoryItemWise obj = new POSRetail.Reports.Inventory.frmInventoryItemWise();
            obj.Show();
        }

        private void vButton19_Click(object sender, EventArgs e)
        {
            //Button b = sender as Button;
            //string authentication = objcore.authentication(b.Text);
            //if (authentication == "yes")
            //{

            //}
            //else
            //{
            //    MessageBox.Show("You are not allowed to view this");
            //    return;
            //}
            //POSRetail.Reports.Inventory.frmInventoryItemWise obj = new POSRetail.Reports.Inventory.frmInventoryItemWise();
            //obj.Show();
        }
    }
}
