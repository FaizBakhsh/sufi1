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
    public partial class Reports : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

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
            POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale obj = new POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale();
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
            POSRestaurant.Reports.SaleReports.FrmMenuItemSale obj = new POSRestaurant.Reports.SaleReports.FrmMenuItemSale();
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
               
                return;
            }
            POSRestaurant.Reports.SaleReports.DSReports obj = new POSRestaurant.Reports.SaleReports.DSReports();
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
            POSRestaurant.Reports.SaleReports.FrmCashierSale obj = new POSRestaurant.Reports.SaleReports.FrmCashierSale();
            obj.Show();
        }

        private void vButton5_Click(object sender, EventArgs e)
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
            POSRestaurant.Reports.SaleReports.Frmpnl3 obj = new POSRestaurant.Reports.SaleReports.Frmpnl3();
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
            POSRestaurant.Reports.SaleReports.FrmPaymentWiseSale obj = new POSRestaurant.Reports.SaleReports.FrmPaymentWiseSale();
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
            POSRestaurant.Reports.SaleReports.FrmDiscountSale obj = new POSRestaurant.Reports.SaleReports.FrmDiscountSale();
            obj.Show();
        }

        private void vButton8_Click(object sender, EventArgs e)
        {

            POSRestaurant.Reports.SaleReports.FrmComplimentorySale obj = new POSRestaurant.Reports.SaleReports.FrmComplimentorySale();
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
            POSRestaurant.Reports.SaleReports.FrmRefundSale obj = new POSRestaurant.Reports.SaleReports.FrmRefundSale();
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
            POSRestaurant.Reports.SaleReports.FrmHourlySale obj = new POSRestaurant.Reports.SaleReports.FrmHourlySale();
            obj.Show();
        }

        private void vButton11_Click(object sender, EventArgs e)
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
            POSRestaurant.Reports.SaleReports.FrmInvoiceSale obj = new POSRestaurant.Reports.SaleReports.FrmInvoiceSale();
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
            POSRestaurant.Reports.Inventory.frmReceivedInventory obj = new POSRestaurant.Reports.Inventory.frmReceivedInventory();
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
            POSRestaurant.Reports.Inventory.frmClosingInventory obj = new POSRestaurant.Reports.Inventory.frmClosingInventory();
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
            POSRestaurant.Reports.Inventory.frmConsumedInventory obj = new POSRestaurant.Reports.Inventory.frmConsumedInventory();
            obj.Show();
        }

        private void vButton12_Click(object sender, EventArgs e)
        {

        }

        private void vButton17_Click(object sender, EventArgs e)
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
            POSRestaurant.Reports.CashTransactions.frmCashTransaction obj = new  POSRestaurant.Reports.CashTransactions.frmCashTransaction();
            obj.Show();
        }

        private void vButton12_Click_1(object sender, EventArgs e)
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
            POSRestaurant.Reports.Members.frmMembers obj = new POSRestaurant.Reports.Members.frmMembers();
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
            POSRestaurant.Reports.Members.frmCustomerPoints obj = new  POSRestaurant.Reports.Members.frmCustomerPoints();
            obj.Show();
        }

        private void vButton18_Click(object sender, EventArgs e)
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
            POSRetail.Reports.waste.FrmWaste obj = new POSRetail.Reports.waste.FrmWaste();
           
            //POSRestaurant.Reports.Members.frmCustomerPoints obj = new POSRestaurant.Reports.Members.frmCustomerPoints();
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

            POSRestaurant.Reports.SaleReports.frmsuggested obj = new POSRestaurant.Reports.SaleReports.frmsuggested();
            obj.Show();
        }

        private void vButton20_Click(object sender, EventArgs e)
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

            POSRestaurant.Reports.frmSalesGroup obj = new POSRestaurant.Reports.frmSalesGroup();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton21_Click(object sender, EventArgs e)
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

            POSRestaurant.Reports.SaleReports.frmWeek obj = new POSRestaurant.Reports.SaleReports.frmWeek();
            obj.Show();
        }

        private void vButton22_Click(object sender, EventArgs e)
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

            POSRestaurant.Reports.SaleReports.FrmMenuItemSaleRecipe obj = new POSRestaurant.Reports.SaleReports.FrmMenuItemSaleRecipe();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void Reports_Load(object sender, EventArgs e)
        {
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, vTabControl1, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, vTabPage1, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, vTabPage2, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, vTabPage3, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, vTabPage4, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tableLayoutPanel1, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tableLayoutPanel2, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tableLayoutPanel3, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tableLayoutPanel4, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tableLayoutPanel5, new object[] { true });
            vTabControl1.TabPages.RemoveAt(2);
            vTabControl1.TabPages.RemoveAt(2);
            vTabControl1.TabPages.RemoveAt(2);
            
        }

        private void vButton23_Click(object sender, EventArgs e)
        {


            POSRestaurant.Reports.SaleReports.FrmMessagesSale obj = new POSRestaurant.Reports.SaleReports.FrmMessagesSale();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton24_Click(object sender, EventArgs e)
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

            POSRestaurant.Reports.SaleReports.Frmgroupsale obj = new POSRestaurant.Reports.SaleReports.Frmgroupsale();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton25_Click(object sender, EventArgs e)
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

            POSRestaurant.Reports.SaleReports.Frmpnl2 obj = new POSRestaurant.Reports.SaleReports.Frmpnl2();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton26_Click(object sender, EventArgs e)
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

            POSRestaurant.Reports.Inventory.frmWorksheet obj = new POSRestaurant.Reports.Inventory.frmWorksheet();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton27_Click(object sender, EventArgs e)
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

            POSRestaurant.Reports.Inventory.frmVariance obj = new POSRestaurant.Reports.Inventory.frmVariance();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton28_Click(object sender, EventArgs e)
        {
            //Button b = sender as Button;
            //string authentication = objcore.authentication(b.Text);
            //if (authentication == "yes")
            //{

            //}
            //else
            //{
            //    //MessageBox.Show("You are not allowed to view this");
            //    //return;
            //}
            POSRestaurant.Reports.SaleReports.FrmFoodcostreportnew1 obj = new POSRestaurant.Reports.SaleReports.FrmFoodcostreportnew1();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton29_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.Reports.frmSales obj = new POSRestaurant.Reports.frmSales();
            obj.Show();
        }

        private void vButton30_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.Reports.SaleReports.frmSalesConsolidated obj = new POSRestaurant.Reports.SaleReports.frmSalesConsolidated();
            obj.Show();
        }

        private void vButton31_Click(object sender, EventArgs e)
        {

            POSRestaurant.Reports.SaleReports.FrmMenuItemSaledetailed obj = new POSRestaurant.Reports.SaleReports.FrmMenuItemSaledetailed();
            obj.Show();
        }

        private void vButton1_Click_2(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale obj = new POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale();
            obj.Show();
        }

        private void vButton32_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.Reports.Inventory.frmDiscard2 obj = new POSRestaurant.Reports.Inventory.frmDiscard2();
            obj.Show();
        }

        private void vButton33_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.Reports.Inventory.frmfood obj = new POSRestaurant.Reports.Inventory.frmfood();
            obj.Show();
        }

        private void vButton34_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.Reports.SaleReports.FrmSalesTax obj = new POSRestaurant.Reports.SaleReports.FrmSalesTax();
            obj.Show();
        }

        private void vButton35_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.Reports.SaleReports.FrmDiscountSale100 obj = new POSRestaurant.Reports.SaleReports.FrmDiscountSale100();
            obj.Show();
        }
        private void vButton36_Click(object sender, EventArgs e)
        {

            POSRestaurant.Reports.SaleReports.Frmitemprices obj = new POSRestaurant.Reports.SaleReports.Frmitemprices();
            obj.Show();

        }

        private void vButton37_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Frmrims obj = new POSRestaurant.Reports.Frmrims();
            obj.Show();
        }

        private void vButton38_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.Frmreceipeold obj = new POSRestaurant.Reports.Inventory.Frmreceipeold();
            obj.Show();
        }

        private void vButton39_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmserevrsSale obj = new POSRestaurant.Reports.SaleReports.FrmserevrsSale();
            obj.Show();
        }

        private void vButton40_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmDiscountDetailsSale obj = new POSRestaurant.Reports.SaleReports.FrmDiscountDetailsSale();
            obj.Show();
        }

        private void vButton41_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmcustomerSale obj = new POSRestaurant.Reports.SaleReports.FrmcustomerSale();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton42_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.Frmitemlist obj = new POSRestaurant.Reports.SaleReports.Frmitemlist();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton43_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.FrmMenuCost obj = new POSRestaurant.Reports.Inventory.FrmMenuCost();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
            //POSRestaurant.Reports.SaleReports.FrmFoodcostreport2 obj2 = new POSRestaurant.Reports.SaleReports.FrmFoodcostreport2();
            ////POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            //obj2.Show();
        }

        private void vButton44_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmDiscountcompaign obj = new POSRestaurant.Reports.SaleReports.FrmDiscountcompaign();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton45_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.Frmpnl obj = new POSRestaurant.Reports.SaleReports.Frmpnl();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton46_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmfoodvalue obj = new POSRestaurant.Reports.Inventory.frmfoodvalue();
            obj.Show();
        }

        private void vButton47_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmissuence obj = new POSRestaurant.Reports.Inventory.frmissuence();
            obj.Show();
        }

        private void vButton48_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.Frmtargets obj = new POSRestaurant.Reports.SaleReports.Frmtargets();
            obj.Show();
        }

        private void vButton49_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmInvoiceDetailsSale obj = new POSRestaurant.Reports.SaleReports.FrmInvoiceDetailsSale();
            obj.Show();
        }

        private void vButton50_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.frmVoucherSales obj = new POSRestaurant.Reports.SaleReports.frmVoucherSales();
            obj.Show();
        }

        private void vButton51_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmPoints obj = new POSRestaurant.Reports.SaleReports.FrmPoints();
            //POSRestaurant.Reports.SaleReports.frmMonthly obj = new POSRestaurant.Reports.SaleReports.frmMonthly();
            obj.Show();
        }

        private void vButton52_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmDiscountSalesummary obj = new POSRestaurant.Reports.SaleReports.FrmDiscountSalesummary();
            obj.Show();
        }

        private void vButton53_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmLogs obj = new POSRestaurant.Reports.SaleReports.FrmLogs();
            obj.Show();
        }

        private void vButton54_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmreverserecipe obj = new POSRestaurant.Reports.Inventory.frmreverserecipe();
            obj.Show();
        }

        private void vButton55_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmProduction onj = new POSRestaurant.Reports.Inventory.frmProduction();
            onj.Show();
        }

        private void vButton56_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmFeedback onj = new POSRestaurant.Reports.SaleReports.FrmFeedback();
            onj.Show();
        }

        private void vButton57_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmTablesSale onj = new POSRestaurant.Reports.SaleReports.FrmTablesSale();
            onj.Show();
        }

        private void vButton58_Click(object sender, EventArgs e)
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
            POSRestaurant.Reports.Inventory.frmstoreissuence obj = new POSRestaurant.Reports.Inventory.frmstoreissuence();
            obj.Show();
        }
    }
}
