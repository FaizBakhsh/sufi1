using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class Backendnew : Form
    {
        public Backendnew()
        {
            InitializeComponent();

            string q = "select top 1 BranchName,type from Branch where status='Active' and Branchcode = '1001' "; 
            DataSet dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                //lblBranchName.Text = Convert.ToString(dspurchase.Tables[0].Rows[0][0]);

                lblBranchName.Text = Convert.ToString(dspurchase.Tables[0].Rows[0][0]);
                //dateTimePicker1.MinDate = date;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

            DialogResult rd = MessageBox.Show("Are You Sure to Exit ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rd == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        public void fillforms(string form)
        {
            try
            {
                string q = ""; 

                if (formslist.Where(x=>x.name==form).ToList().Count > 0)
                {

                }
                else
                {
                    DataSet dss = new DataSet();
                    int idd = 0;
                    dss = objcore.funGetDataSet("select id as id from Forms where forms='" + form + "'");
                    if (dss.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        dss = new DataSet();
                        dss = objcore.funGetDataSet("select max(id) as id from Forms");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string ii = dss.Tables[0].Rows[0][0].ToString();
                            if (ii == string.Empty)
                            {
                                ii = "0";
                            }
                            idd = Convert.ToInt32(ii) + 1;
                        }
                        else
                        {
                            idd = 1;
                        }
                        q = "insert into Forms(Id,Forms) values('" + idd + "','" + form + "')";
                        objcore.executeQuery(q);
                        getforms();
                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void treeView1_Click(object sender, EventArgs e)
        {

        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

            string authentication = "no"; objcore.authentication(e.Node.Text);
            try
            {
                authentication = Rightslist.Where(x => x.Forms == e.Node.Text && x.Userid.ToString() == POSRestaurant.Properties.Settings.Default.UserId.ToString()).ToList().FirstOrDefault().Status;
            }
            catch (Exception ex)
            {
                
            }
            if (authentication == "yes")
            {

            }
            else
            {
                if (e.Node.Text == "Inventory" || e.Node.Text == "Reports" || e.Node.Text == "Update Database" || e.Node.Text == "Reopen Shift")
                {
                    if (e.Node.Text == "Update Database")
                    {
                        //progressbar myForm = new progressbar();
                        //myForm.MdiParent = this;
                        //try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                        //// //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                        //// myForm.Dock = DockStyle.Fill;
                        //myForm.StartPosition = FormStartPosition.CenterScreen;
                        //pnlmain.Controls.Add(myForm);
                        //myForm.type = "Update Columns";
                        //myForm.Show();
                        objcore.addcolumns();
                        MessageBox.Show("Updated");
                        
                    }
                    
                }
                else if (e.Node.Text == "Reports .")
                {
                    return;
                }
                else
                {
                    fillforms(e.Node.Text);
                    MessageBox.Show("You are not allowed to view this");
                    return;
                }
            }
            
            if (e.Node.Text == "Ds Server Sales Report")
            {


                POSRestaurant.Reports.SaleReports.FrmserevrsDSSale myForm = new POSRestaurant.Reports.SaleReports.FrmserevrsDSSale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }

            if (e.Node.Text == "Voucher Approval")
            {


                POSRestaurant.Accounts.VoucherApproval myForm = new POSRestaurant.Accounts.VoucherApproval();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Bank Reconciliation")
            {


                POSRestaurant.Accounts.BankReconciliation myForm = new POSRestaurant.Accounts.BankReconciliation();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Bank Reconciliation Report")
            {


                POSRestaurant.Reports.Accounts.frmBRS myForm = new POSRestaurant.Reports.Accounts.frmBRS();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            
            if (e.Node.Text == "Raw Waste")
            {
                POSRestaurant.RawItems.Rawwaste myForm = new RawItems.Rawwaste();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Raw Wastage")
            {
                POSRestaurant.Reports.Inventory.frmWastage myForm = new POSRestaurant.Reports.Inventory.frmWastage();
                myForm.MdiParent = this;
                try { myForm.TopLevel = true; }
                catch (Exception ex) { } myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Inventory Closing")
            {
                POSRestaurant.RawItems.CriticalInventoryClosingonly myForm = new RawItems.CriticalInventoryClosingonly();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }


            if (e.Node.Text == "Sales Report Till")
            {


                POSRestaurant.Reports.frmSalesTill myForm = new POSRestaurant.Reports.frmSalesTill();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Attach Menu Report")
            {


                POSRestaurant.Reports.SaleReports.FrmAttachMenu myForm = new POSRestaurant.Reports.SaleReports.FrmAttachMenu();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Close Shift")
            {


                POSRestaurant.Setting.Closeshift myForm = new Setting.Closeshift();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            
            if (e.Node.Text == "Purchase Order Approval")
            {


                POSRestaurant.RawItems.PurchaseOrderApproval myForm = new RawItems.PurchaseOrderApproval();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
             if (e.Node.Text == "Issue Stock Approval")
            {


                POSRestaurant.RawItems.invTransferApproval myForm = new RawItems.invTransferApproval();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Requisition Request")
            {


                POSRestaurant.RawItems.RequisitionRequest myForm = new RawItems.RequisitionRequest();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Requisition Approval")
            {
                POSRestaurant.RawItems.RequisitionApproval myForm = new RawItems.RequisitionApproval();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Transfer Out Approval Request")
            {


                POSRestaurant.RawItems.TransferoutApprovalRequest myForm = new RawItems.TransferoutApprovalRequest();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Transfer Out Approval")
            {


                POSRestaurant.RawItems.TransferoutApproval myForm = new RawItems.TransferoutApproval();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
             if (e.Node.Text == "Delivery Report")
            {


                POSRestaurant.Reports.SaleReports.FrmDeliverySale myForm = new POSRestaurant.Reports.SaleReports.FrmDeliverySale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            
            if (e.Node.Text == "Track Inventory")
            {


                POSRestaurant.RawItems.TrackInventory myForm = new  RawItems.TrackInventory();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Ds Item Wise Sales Report")
            {


                POSRestaurant.Reports.SaleReports.FrmMenuItemDSSale myForm = new POSRestaurant.Reports.SaleReports.FrmMenuItemDSSale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Bank Deposits")
            {


                POSRestaurant.forms.BankDepositsList myForm = new POSRestaurant.forms.BankDepositsList();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }

            if (e.Node.Text == "Inventory Allocation Report")
            {


                POSRestaurant.Reports.Inventory.frmAllocation myForm = new POSRestaurant.Reports.Inventory.frmAllocation();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
             if (e.Node.Text == "Inventory Allocation")
            {


                POSRestaurant.RawItems.InventoryAllocation myForm = new  RawItems.InventoryAllocation();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Ds Sales Report")
            {


                POSRestaurant.Reports.SaleReports.frmSalesDS myForm = new POSRestaurant.Reports.SaleReports.frmSalesDS();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "KDS Report")
            {


                POSRestaurant.Reports.SaleReports.FrmKDS myForm = new POSRestaurant.Reports.SaleReports.FrmKDS();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Purchase Order")
            {


                POSRestaurant.RawItems.PurchaseOrder myForm = new RawItems.PurchaseOrder();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Vendor List")
            {


                POSRestaurant.Reports.Inventory.frmVendorlist myForm = new POSRestaurant.Reports.Inventory.frmVendorlist();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Stock Estimation")
            {


                POSRestaurant.RawItems.StockEstimation myForm = new RawItems.StockEstimation();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Reopen Shift")
            {


                POSRestaurant.Setting.Reopenshift myForm = new POSRestaurant.Setting.Reopenshift();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;
                myForm.StartPosition = FormStartPosition.CenterScreen;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            //pnlmain.Controls.Clear();
            if (e.Node.Text == "Old Interface")
            {
                POSRestaurant.Properties.Settings.Default.view = "old";
                POSRestaurant.Properties.Settings.Default.Save();
                POSRestaurant.forms.BackendForm obj = new BackendForm();
                obj.Show();
                this.Close();
            }
            if (e.Node.Text == "New Interface")
            {
                POSRestaurant.Properties.Settings.Default.view = "new";
                POSRestaurant.Properties.Settings.Default.Save();
                POSRestaurant.forms.Backendnew obj = new Backendnew();
                obj.Show();
                this.Close();
            }
            if (e.Node.Text == "Update Sold Inventory")
            {
                UpdateInventory myForm = new UpdateInventory();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                // myForm.Dock = DockStyle.Fill;

                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Targets")
            {
                POSRestaurant.forms.Targets2 myForm = new POSRestaurant.forms.Targets2();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Points Codes")
            {
                POSRestaurant.Setting.PointsCodes myForm = new POSRestaurant.Setting.PointsCodes();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Recall Bills")
            {
                POSRestaurant.forms.BillRecall myForm = new POSRestaurant.forms.BillRecall();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Chart Account Codes")
            {
                POSRestaurant.Accounts.AddChartAccountsCodes myForm = new POSRestaurant.Accounts.AddChartAccountsCodes();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Chart of Accounts")
            {
                POSRestaurant.Accounts.ChartofAccounts myForm = new POSRestaurant.Accounts.ChartofAccounts();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Assign Accounts")
            {
                POSRestaurant.Accounts.AddSalesAccount myForm = new POSRestaurant.Accounts.AddSalesAccount();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Vouchers")
            {
                POSRestaurant.Accounts.Vouchers myForm = new POSRestaurant.Accounts.Vouchers();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Chart of Account Report")
            {
                POSRestaurant.Reports.Accounts.frmaccounts myForm = new POSRestaurant.Reports.Accounts.frmaccounts();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            
            if (e.Node.Text == "GL Accounts")
            {
                POSRestaurant.Reports.Statements.frmGLAccounts myForm = new POSRestaurant.Reports.Statements.frmGLAccounts();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Receivable Statement")
            {
                POSRestaurant.Reports.Statements.frmReceiveableStatemet myForm = new POSRestaurant.Reports.Statements.frmReceiveableStatemet();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            

            if (e.Node.Text == "Payable Statement")
            {
                POSRestaurant.Reports.Statements.frmPayableStatemet myForm = new POSRestaurant.Reports.Statements.frmPayableStatemet();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Aging Report")
            {
                POSRestaurant.Reports.Accounts.frmAgingStatemet myForm = new POSRestaurant.Reports.Accounts.frmAgingStatemet();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Trial Balance")
            {
                POSRestaurant.Reports.Accounts.frmTrialBalane2 myForm = new POSRestaurant.Reports.Accounts.frmTrialBalane2();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Profit\\Loss")
            {
                POSRestaurant.Reports.Accounts.frmProfitLoss myForm = new POSRestaurant.Reports.Accounts.frmProfitLoss();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Balance Sheet")
            {
                POSRestaurant.Reports.Accounts.frmBalaneSheet myForm = new POSRestaurant.Reports.Accounts.frmBalaneSheet();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Receivables")
            {
                POSRestaurant.Reports.Statements.frmReceiveableStatemetsum myForm = new POSRestaurant.Reports.Statements.frmReceiveableStatemetsum();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Payables")
            {
                POSRestaurant.Reports.Statements.frmpayableableStatemetsum myForm = new POSRestaurant.Reports.Statements.frmpayableableStatemetsum();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Day Book")
            {
                POSRestaurant.Reports.Accounts.frmDayBook myForm = new POSRestaurant.Reports.Accounts.frmDayBook();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Download Accounts")
            {
                POSRestaurant.Accounts.DownloadAccounts myForm = new POSRestaurant.Accounts.DownloadAccounts();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Download Users")
            {
                POSRestaurant.forms.DownloadUsers myForm = new POSRestaurant.forms.DownloadUsers();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Employee Statement")
            {
                POSRestaurant.Reports.Statements.frmEmployeePayableStatemet myForm = new POSRestaurant.Reports.Statements.frmEmployeePayableStatemet();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Rights")
            {
                
                POSRestaurant.Setting.Rights myForm = new POSRestaurant.Setting.Rights();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
               
            }
            if (e.Node.Text == "Stock Issuance")
            {
                POSRestaurant.Reports.Inventory.frmissuence myForm = new POSRestaurant.Reports.Inventory.frmissuence();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Food Variance")
            {
                POSRestaurant.Reports.Inventory.frmDiscard2 myForm = new POSRestaurant.Reports.Inventory.frmDiscard2();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Stock Issuance Report")
            {
                POSRestaurant.Reports.Inventory.frmissuence myForm = new POSRestaurant.Reports.Inventory.frmissuence();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Store Issuance Report")
            {
                POSRestaurant.Reports.Inventory.frmstoreissuence myForm = new POSRestaurant.Reports.Inventory.frmstoreissuence();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Ds Group Wise Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmgroupDSsale myForm = new POSRestaurant.Reports.SaleReports.FrmgroupDSsale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Ds Invoice Wise Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmInvoiceSaleDS myForm = new POSRestaurant.Reports.SaleReports.FrmInvoiceSaleDS();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Menu Item Wise Detailed Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmMenuItemSaledetailed myForm = new POSRestaurant.Reports.SaleReports.FrmMenuItemSaledetailed();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Log Report")
            {
                POSRestaurant.Reports.SaleReports.FrmLogs myForm = new POSRestaurant.Reports.SaleReports.FrmLogs();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Payment Wise Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmPaymentWiseSale myForm = new POSRestaurant.Reports.SaleReports.FrmPaymentWiseSale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Daily Sales Report(Group)")
            {
                POSRestaurant.Reports.frmSalesGroup myForm = new POSRestaurant.Reports.frmSalesGroup();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Price Analysis Report")
            {
                POSRestaurant.Reports.SaleReports.frmsuggested myForm = new POSRestaurant.Reports.SaleReports.frmsuggested();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Targets Report")
            {
                POSRestaurant.Reports.SaleReports.Frmtargets myForm = new POSRestaurant.Reports.SaleReports.Frmtargets();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Invoice Detailed Report")
            {
                POSRestaurant.Reports.SaleReports.FrmInvoiceDetailsSale myForm = new POSRestaurant.Reports.SaleReports.FrmInvoiceDetailsSale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Voucher Sales Report")
            {
                POSRestaurant.Reports.SaleReports.frmVoucherSales myForm = new POSRestaurant.Reports.SaleReports.frmVoucherSales();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Complimentory Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmComplimentorySale myForm = new POSRestaurant.Reports.SaleReports.FrmComplimentorySale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Discount Summary Report")
            {
                POSRestaurant.Reports.SaleReports.FrmDiscountSalesummary myForm = new POSRestaurant.Reports.SaleReports.FrmDiscountSalesummary();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Menu Price vs Cost")
            {
                POSRestaurant.Reports.Inventory.FrmMenuCost myForm = new POSRestaurant.Reports.Inventory.FrmMenuCost();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Points Redeem Report")
            {
                POSRestaurant.Reports.SaleReports.FrmPoints myForm = new POSRestaurant.Reports.SaleReports.FrmPoints();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Reverse Recipe Report")
            {
                POSRestaurant.Reports.Inventory.frmreverserecipe myForm = new POSRestaurant.Reports.Inventory.frmreverserecipe();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Feedback Report")
            {
                POSRestaurant.Reports.SaleReports.FrmFeedback myForm = new POSRestaurant.Reports.SaleReports.FrmFeedback();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }

            if (e.Node.Text == "Tables Report")
            {
                POSRestaurant.Reports.SaleReports.FrmTablesSale myForm = new POSRestaurant.Reports.SaleReports.FrmTablesSale();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Production Recipe")
            {
                POSRestaurant.Setting.AddRecipeProduction myForm = new POSRestaurant.Setting.AddRecipeProduction();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Production Recipe Report")
            {
                POSRestaurant.Reports.Inventory.FrmreceipeProduction myForm = new POSRestaurant.Reports.Inventory.FrmreceipeProduction();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Production")
            {
                POSRestaurant.RawItems.Production myForm = new POSRestaurant.RawItems.Production();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }

            if (e.Node.Text == "Production PercentagWise")
            {
                POSRestaurant.RawItems.ProductionPercentageWise myForm = new POSRestaurant.RawItems.ProductionPercentageWise();
                myForm.MdiParent = this;
                try { myForm.TopLevel = true; }
                catch (Exception ex) { } myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }

            if (e.Node.Text == "Production Recipe")
            {
                POSRestaurant.Setting.AddRecipeProduction myForm = new POSRestaurant.Setting.AddRecipeProduction();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Production Report")
            {
                POSRestaurant.Reports.Inventory.frmProduction myForm = new POSRestaurant.Reports.Inventory.frmProduction();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Issue Stock")
            {
                POSRestaurant.RawItems.invTransfer myForm = new POSRestaurant.RawItems.invTransfer();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Kitchen Demand")
            {
                POSRestaurant.RawItems.StoreDemand myForm = new POSRestaurant.RawItems.StoreDemand();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Kitchen Issuance")
            {
                POSRestaurant.RawItems.StoreTransfer myForm = new POSRestaurant.RawItems.StoreTransfer();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Download Menu")
            {
                DownloadMenu myForm = new DownloadMenu();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Download Discounts")
            {
                DownloadDiscounts myForm = new DownloadDiscounts();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Download Issuance")
            {
                Downloadtransferin myForm = new Downloadtransferin();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Download Accounts")
            {
          
                POSRestaurant.Accounts.DownloadAccounts myForm = new Accounts.DownloadAccounts();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Settings")
            {
                MainForm myForm = new MainForm();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Add Raw Items")
            {
                RawItems.AddRawItems myForm = new RawItems.AddRawItems();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}
                myForm.Text = e.Node.Text;
                myForm.AutoScroll = true;
               // myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Demand")
            {
                RawItems.Demand myForm = new RawItems.Demand();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Purchase Return")
            {
                RawItems.Purchasereturn myForm = new RawItems.Purchasereturn();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Purchase Items")
            {
                RawItems.Purchase myForm = new RawItems.Purchase();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            


            if (e.Node.Text == "Download Purchase")
            {
                forms.Downloadpurchase myForm = new forms.Downloadpurchase();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Critical Inventory")
            {
                RawItems.CriticalInventory myForm = new RawItems.CriticalInventory();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Inventory Transfer")
            {
                RawItems.Transfer myForm = new RawItems.Transfer();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Complete Waste")
            {
                RawItems.Completewaste myForm = new RawItems.Completewaste();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            
            
            if (e.Node.Text == "Menu Item Wise Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmMenuItemSale myForm = new POSRestaurant.Reports.SaleReports.FrmMenuItemSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Shift Wise Sales Report")
            {
                POSRestaurant.Reports.FrmShiftSale myForm = new POSRestaurant.Reports.FrmShiftSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Refund Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmRefundSale myForm = new POSRestaurant.Reports.SaleReports.FrmRefundSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
              //  //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Discount Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmDiscountSale myForm = new POSRestaurant.Reports.SaleReports.FrmDiscountSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
              //  //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Hourly Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmHourlySale myForm = new POSRestaurant.Reports.SaleReports.FrmHourlySale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Detailed Sales Reports")
            {
                POSRestaurant.Reports.SaleReports.frmSalesConsolidated myForm = new POSRestaurant.Reports.SaleReports.frmSalesConsolidated();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Invoice Wise Report")
            {
                POSRestaurant.Reports.SaleReports.FrmInvoiceSale myForm = new POSRestaurant.Reports.SaleReports.FrmInvoiceSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
               // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Sales Report")
            {
                POSRestaurant.Reports.frmSales myForm = new POSRestaurant.Reports.frmSales();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                ////myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Menu Group Wise Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale myForm = new POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                ////myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Sales Tax Report")
            {
                POSRestaurant.Reports.SaleReports.FrmSalesTax myForm = new POSRestaurant.Reports.SaleReports.FrmSalesTax();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                ////myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Discount Report 100")
            {
                POSRestaurant.Reports.SaleReports.FrmDiscountSale100 myForm = new POSRestaurant.Reports.SaleReports.FrmDiscountSale100();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                ////myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Menu Prices")
            {
                POSRestaurant.Reports.SaleReports.Frmitemprices myForm = new POSRestaurant.Reports.SaleReports.Frmitemprices();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                ////myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Receipe Report")
            {
                POSRestaurant.Reports.Inventory.Frmreceipe myForm = new POSRestaurant.Reports.Inventory.Frmreceipe();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                ////myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Food Cost Report")
            {
               //POSRestaurant.Reports.SaleReports.FrmFoodcostreportnew1 myForm = new POSRestaurant.Reports.SaleReports.FrmFoodcostreportnew1();
                POSRestaurant.Reports.SaleReports.FrmFoodcostreportnew2 myForm = new POSRestaurant.Reports.SaleReports.FrmFoodcostreportnew2();
              
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Discount Details Report")
            {
                POSRestaurant.Reports.SaleReports.FrmDiscountDetailsSale myForm = new POSRestaurant.Reports.SaleReports.FrmDiscountDetailsSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Customer Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmcustomerSale myForm = new POSRestaurant.Reports.SaleReports.FrmcustomerSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Phone Nos Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmMessagesSale myForm = new POSRestaurant.Reports.SaleReports.FrmMessagesSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Item List")
            {
                POSRestaurant.Reports.SaleReports.Frmitemlist myForm = new POSRestaurant.Reports.SaleReports.Frmitemlist();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Server Sales Report")
            {
                POSRestaurant.Reports.SaleReports.FrmserevrsSale myForm = new POSRestaurant.Reports.SaleReports.FrmserevrsSale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Discount Compaign Report")
            {
                POSRestaurant.Reports.SaleReports.FrmDiscountcompaign myForm = new POSRestaurant.Reports.SaleReports.FrmDiscountcompaign();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Group Wise Sales Report")
            {
                POSRestaurant.Reports.SaleReports.Frmgroupsale myForm = new POSRestaurant.Reports.SaleReports.Frmgroupsale();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Profit n Loss Report")
            {
                POSRestaurant.Reports.SaleReports.Frmpnl myForm = new POSRestaurant.Reports.SaleReports.Frmpnl();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Inventory Received")
            {
                POSRestaurant.Reports.Inventory.frmReceivedInventory myForm = new POSRestaurant.Reports.Inventory.frmReceivedInventory();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Consumed Inventory")
            {
                POSRestaurant.Reports.Inventory.frmConsumedInventory myForm = new POSRestaurant.Reports.Inventory.frmConsumedInventory();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Closing Inventory")
            {
                POSRestaurant.Reports.Inventory.frmClosingInventory myForm = new POSRestaurant.Reports.Inventory.frmClosingInventory();
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Critical Inventory(Quantitative)")
            {
                //POSRestaurant.Reports.Inventory.frmfood myForm = new POSRestaurant.Reports.Inventory.frmfood();
                //myForm.MdiParent = this;
                // try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                ////myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                //myForm.Dock = DockStyle.Fill;
                //pnlmain.Controls.Add(myForm);
                //myForm.Show();


                POSRestaurant.Reports.Inventory.frmfoodnew myForm1 = new POSRestaurant.Reports.Inventory.frmfoodnew();
                myForm1.MdiParent = this;
                myForm1.TopLevel = false; myForm1.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm1.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm1);
                myForm1.Show();
            }
            

            if (e.Node.Text == "Critical Inventory(Value)")
            {
                POSRestaurant.Reports.Inventory.frmfoodnew myForm = new POSRestaurant.Reports.Inventory.frmfoodnew();
                myForm.type = "value";
                myForm.MdiParent = this;
                 try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {}myForm.Text = e.Node.Text;
                myForm.Text = e.Node.Text;
                //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Recipe vs Sale")
            {
                POSRestaurant.Reports.SaleReports.FrmMenuItemSaleRecipe myForm = new POSRestaurant.Reports.SaleReports.FrmMenuItemSaleRecipe();
                myForm.MdiParent = this;
                try                {                    myForm.TopLevel = true;                }                catch (Exception ex)                {} myForm.Text = e.Node.Text;
                myForm.Text = e.Node.Text;
                myForm.AutoScroll = true; myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
            if (e.Node.Text == "Cash Deposit")
            {
                POSRestaurant.Reports.Accounts.frmCashDeposit myForm = new POSRestaurant.Reports.Accounts.frmCashDeposit();
                myForm.MdiParent = this;
                try { myForm.TopLevel = true; }
                catch (Exception ex) { } myForm.Text = e.Node.Text;
                // //myForm.AutoScroll = true;myForm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
                myForm.Dock = DockStyle.Fill;
                pnlmain.Controls.Add(myForm);
                myForm.Show();
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        List<FormsClass> formslist = new List<FormsClass>();
        List<RightsClass> Rightslist = new List<RightsClass>();
        protected void getforms()
        {
            try
            {
                string q = "select * from Forms order by Forms";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                formslist = new List<FormsClass>();
                IList<FormsClass> data = ds.Tables[0].AsEnumerable().Select(row =>
                    new FormsClass
                    {

                        name = row.Field<string>("Forms")

                    }).ToList();
                formslist = data.ToList();



            }
            catch (Exception ex)
            {


            }
            
           
        }
        protected void getrights()
        {
            try
            {
                string q = "SELECT     dbo.Rights.Status, dbo.Forms.Forms, dbo.Rights.Userid FROM         dbo.Rights INNER JOIN                      dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                Rightslist = new List<RightsClass>();
                IList<RightsClass> data = ds.Tables[0].AsEnumerable().Select(row =>
                    new RightsClass
                    {

                        Forms = row.Field<string>("Forms"),
                        Status = row.Field<string>("Status"),
                        Userid = row.Field<int>("Userid")

                    }).ToList();
                Rightslist = data.ToList();



            }
            catch (Exception ex)
            {


            }
        }
        private void Backendnew_Load(object sender, EventArgs e)
        {
            try
            {
                fillforms("Old Duplicate Bills");
                //lblversion.Text = "V. " + Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
            catch (Exception ex)
            {

            }
            try
            {
                SqlDataReader sdr = null;
                sdr = objcore.funGetDataReader1("select * from DeviceSetting where device='Main Screen Location'");
                if (sdr.Read())
                {
                    string show = (sdr["Status"].ToString());
                    try
                    {
                        if (show == "Enabled")
                        {
                            Screen[] sc;
                            sc = Screen.AllScreens;
                            this.StartPosition = FormStartPosition.Manual;

                            this.Location = Screen.AllScreens[Convert.ToInt32(sdr["no"].ToString())].WorkingArea.Location;
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }

            }
            catch (Exception ex)
            {

            }
            DataTable dt = new DataTable();
            dt.Columns.Add("Name", typeof(string));
            getforms();
            getrights();
           
            foreach (TreeNode ndparent in treeView1.Nodes)
            {
                string authenticate = "";

                try
                {
                    try
                    {
                        authenticate = Rightslist.Where(x => x.Forms == ndparent.Text && x.Userid.ToString() == POSRestaurant.Properties.Settings.Default.UserId).ToList().FirstOrDefault().Status;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (authenticate.ToLower() == "yes")
                    {
                        foreach (TreeNode nd in ndparent.Nodes)
                        {
                            authenticate = "";
                            try
                            {
                                if (nd.Text == "Others" || nd.Text == "Configurations" || nd.Text == "Update Database" || nd.Text == "Old Interface" || nd.Text == "Reopen Shift")
                                {
                                }
                                else
                                {
                                    try
                                    {
                                        authenticate = Rightslist.Where(x => x.Forms == nd.Text && x.Userid.ToString() == POSRestaurant.Properties.Settings.Default.UserId).ToList().FirstOrDefault().Status;
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if (authenticate.ToLower() == "yes")
                                    {
                                        foreach (TreeNode nd2 in nd.Nodes)
                                        {
                                            authenticate = "";
                                            try
                                            {
                                                
                                                {
                                                    try
                                                    {
                                                        authenticate = Rightslist.Where(x => x.Forms == nd2.Text && x.Userid.ToString() == POSRestaurant.Properties.Settings.Default.UserId).ToList().FirstOrDefault().Status;
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                    if (authenticate.ToLower() == "yes")
                                                    {
                                                    }
                                                    else
                                                    {
                                                        fillforms(nd2.Text);
                                                        dt.Rows.Add(nd2.Text);

                                                    }
                                                }
                                            }
                                            catch (Exception ex)
                                            {

                                            }
                                        }
                                    }
                                    else
                                    {
                                        fillforms(nd.Text);
                                        dt.Rows.Add(nd.Text);

                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    else
                    {
                        fillforms(ndparent.Text);
                        dt.Rows.Add(ndparent.Text);
                        
                    }
                }
                catch (Exception ex)
                {
                    
                }
               
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                try
                {
                    TreeNode[] nodes = treeView1.Nodes.Find(dt.Rows[i][0].ToString(), true);
                    if (nodes.Length > 0)
                    {
                        treeView1.Nodes.Remove(nodes[0]);
                       
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node.Text == "Reports .")
            {
                string authentication = objcore.authentication(e.Node.Text);
                if (authentication == "yes")
                {

                }
                else
                {
                    e.Cancel = true;
                    
                }
            }
        }

        private void Backendnew_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void lblversion_Click(object sender, EventArgs e)
        {

        }

        private void lblBranchName_Click(object sender, EventArgs e)
        {

        }
    }
}
