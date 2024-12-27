using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Accounts
{
    public partial class Open : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string q = "",type="",branch="";
        private Vouchers _frm;
        public Open(Vouchers frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void Open_Load(object sender, EventArgs e)
        {
            try
            {
                dateTimePicker1.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
                dateTimePicker1.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());

            }
            catch (Exception ex)
            {
            }
            try
            {
                dateTimePicker2.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
                dateTimePicker2.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
            }
            catch (Exception ex)
            {


            }
            //MessageBox.Show(q);
            ////DataSet ds = new System.Data.DataSet();           
            ////ds = objCore.funGetDataSet(q);
            ////dataGridView1.DataSource = ds.Tables[0];
            ////dataGridView1.Columns[0].Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells["Voucherno"].Value.ToString();
                     string branchid = dataGridView1.Rows[indx].Cells["branchid"].Value.ToString();
                    if (type == "jv")
                    {
                        _frm.getinfojournal(id, branchid);
                    }
                    if (type == "brv")
                    {
                        _frm.getinforeceiptbank(id,branchid);
                    }

                    if (type == "crv")
                    {
                        _frm.getinforeceipt(id,branchid);
                    }

                    if (type == "bpv")
                    {
                        _frm.getinfobank(id,branchid);
                    }
                    if (type == "cpv")
                    {
                        _frm.getinfo(id,branchid);
                    
                    }
                    this.Close();
                    //button29.Text = "Update";

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            string qr = "";

            if (type == "jv")
            {
                if (vTextBox1.Text.Trim() != string.Empty)
                {
                    qr = "SELECT     dbo.JournalAccount.Id, dbo.JournalAccount.Date, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit,dbo.JournalAccount.branchId                        FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where  (dbo.JournalAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.VoucherNo like'%" + vTextBox1.Text + "%' order by dbo.JournalAccount.Date,JournalAccount.VoucherNo ";
                }
                else
                {
                    qr = "SELECT     dbo.JournalAccount.Id, dbo.JournalAccount.Date, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit,dbo.JournalAccount.branchId                        FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where  (dbo.JournalAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') order by dbo.JournalAccount.Date,JournalAccount.VoucherNo ";

                }

            }
            if (type == "brv")
            {
                if (vTextBox1.Text.Trim() != string.Empty)
                {
                    qr = "SELECT     Id, Date, Voucherno, CheckNo, CheckDate, Description, Debit, Credit,branchId FROM         BankAccountReceiptCustomer where  (dbo.BankAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.BankAccountReceiptCustomer.VoucherNo like'%" + vTextBox1.Text + "%' order by dbo.BankAccountReceiptCustomer.Date,BankAccountReceiptCustomer.VoucherNo";
                }
                else
                {
                    qr = "SELECT     Id, Date, Voucherno, CheckNo, CheckDate, Description, Debit, Credit,branchId FROM         BankAccountReceiptCustomer where  (dbo.BankAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  order by dbo.BankAccountReceiptCustomer.Date,BankAccountReceiptCustomer.VoucherNo";

                }
            }
           
            if (type == "crv" )
            {
                if (vTextBox1.Text.Trim() != string.Empty)
                {
                    qr = "SELECT     Id, Date, Voucherno, Description, Debit, Credit,branchId FROM         CashAccountReceiptCustomer  where (dbo.CashAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and dbo.CashAccountReceiptCustomer.VoucherNo like'%" + vTextBox1.Text + "%' order by dbo.CashAccountReceiptCustomer.Date,CashAccountReceiptCustomer.VoucherNo";
                }
                else
                {
                    qr = "SELECT     Id, Date, Voucherno, Description, Debit, Credit,branchId FROM         CashAccountReceiptCustomer  where  (dbo.CashAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  order by dbo.CashAccountReceiptCustomer.Date,CashAccountReceiptCustomer.VoucherNo";

                }
            }
            
            if (type == "bpv" )
            {
                if (vTextBox1.Text.Trim() != string.Empty)
                {
                    qr = "SELECT     Id, Date, Voucherno,CheckNo, CheckDate, Description, Debit, Credit,branchId FROM         BankAccountPaymentSupplier where  (dbo.BankAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.BankAccountPaymentSupplier.VoucherNo like'%" + vTextBox1.Text + "%' order by dbo.BankAccountPaymentSupplier.Date,BankAccountPaymentSupplier.VoucherNo";
                }
                else
                {
                    qr = "SELECT     Id, Date, Voucherno,CheckNo, CheckDate, Description, Debit, Credit,branchId FROM         BankAccountPaymentSupplier where  (dbo.BankAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  order by dbo.BankAccountPaymentSupplier.Date,BankAccountPaymentSupplier.VoucherNo";

                }
            }
            
            if (type == "cpv")
            {
                if (vTextBox1.Text.Trim() != string.Empty)
                {
                    qr = "SELECT     Id, Date, Voucherno, Description, Debit, Credit,branchId FROM         CashAccountPaymentSupplier where  (dbo.CashAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.CashAccountPaymentSupplier.VoucherNo like'%" + vTextBox1.Text + "%' order by dbo.CashAccountPaymentSupplier.Date,CashAccountPaymentSupplier.VoucherNo";
                }
                else
                {
                    qr = "SELECT     Id, Date,Voucherno, Description, Debit, Credit, branchId FROM         CashAccountPaymentSupplier where  (dbo.CashAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  order by dbo.CashAccountPaymentSupplier.Date,CashAccountPaymentSupplier.VoucherNo";

                }
            }
            
            DataSet ds = new System.Data.DataSet();
            ds = objCore.funGetDataSet(qr);
            if (type == "jv")
            {

                try
                {
                    qr = " SELECT         dbo.CustomerAccount.Id, dbo.CustomerAccount.Date, dbo.CustomerAccount.VoucherNo, dbo.CustomerAccount.Description, dbo.CustomerAccount.Debit,                          dbo.CustomerAccount.Credit,dbo.CustomerAccount.branchId FROM            dbo.ChartofAccounts INNER JOIN                         dbo.CustomerAccount ON dbo.ChartofAccounts.Id = dbo.CustomerAccount.PayableAccountId WHERE    (dbo.CustomerAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and  (dbo.CustomerAccount.VoucherNo LIKE '%jv-%') order by dbo.CustomerAccount.Date,CustomerAccount.VoucherNo ";
                    DataSet ds1 = new DataSet();
                    ds1 = objCore.funGetDataSet(qr);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[0].Rows.Add(ds1.Tables[0].Rows[i]["Id"], ds1.Tables[0].Rows[i]["Date"], "", ds1.Tables[0].Rows[i]["VoucherNo"], ds1.Tables[0].Rows[i]["Description"], ds1.Tables[0].Rows[i]["Debit"], ds1.Tables[0].Rows[i]["Credit"], ds1.Tables[0].Rows[i]["branchid"]);
                    }
                }
                catch (Exception ex)
                {
                    
                }
                try
                {
                    qr = " SELECT         dbo.SupplierAccount.Id, dbo.SupplierAccount.Date, dbo.SupplierAccount.VoucherNo, dbo.SupplierAccount.Description, dbo.SupplierAccount.Debit,                          dbo.SupplierAccount.Credit,dbo.CustomerAccount.branchId FROM            dbo.ChartofAccounts INNER JOIN                         dbo.SupplierAccount ON dbo.ChartofAccounts.Id = dbo.SupplierAccount.PayableAccountId WHERE   (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and  (dbo.SupplierAccount.VoucherNo LIKE '%jv-%') order by dbo.SupplierAccount.Date,SupplierAccount.VoucherNo ";
                    DataSet ds2 = new DataSet();
                    ds2 = objCore.funGetDataSet(qr);

                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                        ds.Tables[0].Rows.Add(ds2.Tables[0].Rows[i]["Id"], ds2.Tables[0].Rows[i]["Date"], "", ds2.Tables[0].Rows[i]["VoucherNo"], ds2.Tables[0].Rows[i]["Description"], ds2.Tables[0].Rows[i]["Debit"], ds2.Tables[0].Rows[i]["Credit"], ds2.Tables[0].Rows[i]["branchid"]);
                    }
                }
                catch (Exception ex)
                {
                  
                }
            }
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[2].Visible = false;
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
             var senderGrid = (DataGridView)sender;

             //if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
             if (e.ColumnIndex==0  && e.RowIndex >= 0)
             {
                 try
                 {
                     string vouchertype = "";
                     int indx = dataGridView1.CurrentCell.RowIndex;
                     if (indx >= 0)
                     {
                         string id = dataGridView1.Rows[indx].Cells["Voucherno"].Value.ToString();
                         branch = dataGridView1.Rows[indx].Cells["BranchID"].Value.ToString();
                         if (type == "jv")
                         {
                             vouchertype = "Journal Voucher";
                         }
                         if (type == "brv")
                         {
                             vouchertype = "Bank Receipt Voucher";
                         }

                         if (type == "crv")
                         {
                             vouchertype = "Cash Receipt Voucher";
                         }

                         if (type == "bpv")
                         {
                             vouchertype = "Bank Payment Voucher";
                         }
                         if (type == "cpv")
                         {
                             vouchertype = "Cash Payment Voucher";

                         }
                        
                         POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                         obj.id = id;
                         obj.branch = branch;
                         obj.name = vouchertype;
                         obj.type = type;
                         obj.Show();

                     }
                 }
                 catch (Exception ex)
                 {


                 }
             }
             if (e.ColumnIndex == 1 && e.RowIndex >= 0)
             {
                 try
                 {
                     string vouchertype = "";
                     int indx = dataGridView1.CurrentCell.RowIndex;
                     if (indx >= 0)
                     {
                         string id = dataGridView1.Rows[indx].Cells["Voucherno"].Value.ToString();
                         if (type == "jv")
                         {
                             vouchertype = "Journal Voucher";
                         }
                         if (type == "brv")
                         {
                             vouchertype = "Bank Receipt Voucher";
                         }

                         if (type == "crv")
                         {
                             vouchertype = "Cash Receipt Voucher";
                         }

                         if (type == "bpv")
                         {
                             vouchertype = "Bank Payment Voucher";
                         }
                         if (type == "cpv")
                         {
                             vouchertype = "Cash Payment Voucher";

                         }

                         //POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                         //obj.id = id;
                         //obj.branch = branch;
                         //obj.name = vouchertype;
                         //obj.type = type;
                         //obj.Show();

                         
                         POSRestaurant.Reports.Voucher.frmReceiving obj = new Reports.Voucher.frmReceiving();
                         obj.id = id;
                         obj.branch = branch;
                         obj.name = vouchertype;
                         obj.type = type;
                         obj.Show();
                     }
                 }
                 catch (Exception ex)
                 {


                 }
             }
        }
    }
}
