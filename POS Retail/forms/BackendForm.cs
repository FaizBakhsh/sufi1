using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
namespace POSRetail.forms
{
    public partial class BackendForm : Form
    {
        public string accountstab = "";
        POSRetail.classes.Clsdbcon objcore = new classes.Clsdbcon();

        public BackendForm()
        {
            InitializeComponent();
        }

        private void cardFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm obj = new MainForm();
           // obj.MdiParent = this.MdiParent;
           // obj.WindowState = FormWindowState.Maximized;
            obj.Show();
        }

        private void addRawItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawItems.AddRawItems obj = new RawItems.AddRawItems();
            obj.Show();
        }

        private void BackendForm_Load(object sender, EventArgs e)
        {
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, vTabControl1, new object[] { true });
       
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, Accounts, new object[] { true });

            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tableLayoutPanel2, new object[] { true });
            if (accountstab == "remove")
            {
               
               // vTabControl1.TabPages.RemoveAt(0);
                 Accounts.Focus();
                vButton19.Visible = true;
            }
            else
            {
                // 
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void BackendForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult rd = MessageBox.Show("Are You Sure to Exit ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rd == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void menuGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmMwnuGroupSale obj = new POSRetail.Reports.SaleReports.FrmMwnuGroupSale();
            obj.Show();
        }

        private void menuItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmMenuItemSale obj = new POSRetail.Reports.SaleReports.FrmMenuItemSale();
            obj.Show();
        }

        private void cashierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmCashierSale obj = new POSRetail.Reports.SaleReports.FrmCashierSale();
            obj.Show();
        }

        private void discountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmDiscountSale obj = new POSRetail.Reports.SaleReports.FrmDiscountSale();
            obj.Show();
        }

        private void voidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmVoidSale obj = new POSRetail.Reports.SaleReports.FrmVoidSale();
            obj.Show();
        }

        private void refundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmRefundSale obj = new POSRetail.Reports.SaleReports.FrmRefundSale();
            obj.Show();
        }

        private void hourlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmHourlySale obj = new POSRetail.Reports.SaleReports.FrmHourlySale();
            obj.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmTerminalSale obj = new POSRetail.Reports.SaleReports.FrmTerminalSale();
            obj.Show();
        }

        private void orderWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmOrderWiseSale obj = new POSRetail.Reports.SaleReports.FrmOrderWiseSale();
            obj.Show();
        }

        private void paymentWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmPaymentWiseSale obj = new POSRetail.Reports.SaleReports.FrmPaymentWiseSale();
            obj.Show();
        }

        private void dailyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.FrmUserSale obj = new POSRetail.Reports.FrmUserSale();
            obj.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           


           
              
            


        }

        private void uploadeSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataUploading obj = new DataUploading(this);
            obj.Show();
            this.Enabled = false;
        }

        private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataBackUp obj = new DataBackUp(this);
            obj.Show();
            this.Enabled = false;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            Reports obj = new Reports();
            
            obj.Show();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
           string authentication= objcore.authentication(b.Text);
           if (authentication == "yes")
           {

           }
           else
           {
               MessageBox.Show("You are not allowed to view this");
               return;
           }
            MainForm obj = new MainForm();
            // obj.MdiParent = this.MdiParent;
            // obj.WindowState = FormWindowState.Maximized;
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
            RawItems.AddRawItems obj = new RawItems.AddRawItems();
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
            DataUploading obj = new DataUploading(this);
            obj.Show();
            this.Enabled = false;
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
            DataBackUp obj = new DataBackUp(this);
            obj.Show();
            this.Enabled = false;
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            DialogResult rd = MessageBox.Show("Are You Sure to Exit ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rd == DialogResult.Yes)
            {
                Application.Exit();
            }
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
            POSRetail.RawItems.Purchase obj = new RawItems.Purchase();
            obj.Show();
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.Inventory.frmClosingInventory obj = new POSRetail.Reports.Inventory.frmClosingInventory();
            obj.Show();
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.Inventory.frmConsumedInventory obj = new POSRetail.Reports.Inventory.frmConsumedInventory();
            obj.Show();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {

            POSRetail.Reports.Inventory.frmReceivedInventory obj = new POSRetail.Reports.Inventory.frmReceivedInventory();
            obj.Show();
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
           
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
            Reports obj = new Reports();

            obj.Show();
        }

        private void vButton10_Click_1(object sender, EventArgs e)
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
            POSRetail.Accounts.ChartofAccounts obj = new Accounts.ChartofAccounts();
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
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRetail.Accounts.Vouchers obj = new Accounts.Vouchers();
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
            POSRetail.Reports.Statements.frmPayableStatemetBank obj = new POSRetail.Reports.Statements.frmPayableStatemetBank();
            obj.Show();
        }

        private void vButton9_Click_1(object sender, EventArgs e)
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
            POSRetail.Reports.Statements.frmReceiveableStatemetBank obj = new POSRetail.Reports.Statements.frmReceiveableStatemetBank();
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
            POSRetail.Reports.Statements.frmGLAccounts obj = new POSRetail.Reports.Statements.frmGLAccounts();
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
            POSRetail.Reports.Accounts.frmaccounts obj = new  POSRetail.Reports.Accounts.frmaccounts();
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
            POSRetail.RawItems.PurchaseReturn obj = new RawItems.PurchaseReturn();
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
            POSRetail.Reports.Accounts.frmTrialBalane obj = new POSRetail.Reports.Accounts.frmTrialBalane();
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
            POSRetail.Reports.Accounts.frmBalaneSheet obj = new POSRetail.Reports.Accounts.frmBalaneSheet();
            obj.Show();
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
            POSRetail.Reports.Accounts.frmProfitLoss obj = new POSRetail.Reports.Accounts.frmProfitLoss();
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
            POSRetail.Reports.Customers.FrmCustomers obj = new POSRetail.Reports.Customers.FrmCustomers();
            obj.Show();
            //POSRetail.Reports.frmpictest obj = new  POSRetail.Reports.frmpictest();
            //obj.Show();
        }

        private void Accounts_Paint(object sender, PaintEventArgs e)
        {

        }

        private void vButton19_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
