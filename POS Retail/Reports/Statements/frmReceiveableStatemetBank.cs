using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace POSRetail.Reports.Statements
{
    public partial class frmReceiveableStatemetBank : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmReceiveableStatemetBank()
        {
            InitializeComponent();
        }

        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet ds = new DataSet();
               string q = "select id,name from Customers";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All Customers";
                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "name";
                cmbsupplier.Text = "All Customers";
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRetail.Reports.Statements.rptreceiveablestatementbank rptDoc = new  rptreceiveablestatementbank();
                POSRetail.Reports.Statements.dspayablebank dsrpt = new  dspayablebank();
                //feereport ds = new feereport(); // .xsd file name

               
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders(); 
                getcompany();
                string company = "", phone = "", address = "", logo = "";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                }
                catch (Exception ex)
                {


                }
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    if (logo == "")
                    { }
                    else
                    {

                        dt.Rows.Add("", "", "", "", "", "", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("VoucherNo", typeof(string));
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("Debit", typeof(string));
                dtrpt.Columns.Add("Credit", typeof(string));
                dtrpt.Columns.Add("Balance", typeof(string));
                dtrpt.Columns.Add("CheckNo", typeof(string));
                dtrpt.Columns.Add("AccountName", typeof(string));
                dtrpt.Columns.Add("date1", typeof(string));
                dtrpt.Columns.Add("logo", typeof(Byte[]));

                DataSet ds = new DataSet();
                string q = "";
                if (cmbsupplier.Text == "All Customers")
                {
                    q = "SELECT     dbo.CustomerAccount.Balance AS CurrentBalance, dbo.CustomerAccount.Credit, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Description, dbo.CustomerAccount.VoucherNo,                       dbo.CustomerAccount.Date, dbo.Customers.Name AS accountname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CustomerAccount ON dbo.ChartofAccounts.Id = dbo.CustomerAccount.PayableAccountId INNER JOIN                      dbo.Customers ON dbo.CustomerAccount.CustomersId = dbo.Customers.Id where  (CustomerAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                       
                    //if (cmbtype.Text == "Bank")
                    //{
                    //    q = "SELECT     dbo.ChartofAccounts.Name AS accountname, dbo.CustomerAccount.Balance AS CurrentBalance, dbo.CustomerAccount.Credit, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Description,                       dbo.CustomerAccount.VoucherNo, dbo.CustomerAccount.Date FROM         dbo.ChartofAccounts INNER JOIN                     dbo.CustomerAccount ON dbo.ChartofAccounts.Id = dbo.CustomerAccount.PayableAccountId where  (CustomerAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //    //q = "SELECT     dbo.BankAccountReceiptCustomer.Date, dbo.BankAccountReceiptCustomer.Voucherno, dbo.BankAccountReceiptCustomer.CheckNo, dbo.BankAccountReceiptCustomer.CheckDate,                      dbo.BankAccountReceiptCustomer.Description, dbo.BankAccountReceiptCustomer.Debit, dbo.BankAccountReceiptCustomer.Credit, dbo.BankAccountReceiptCustomer.CurrentBalance,                       dbo.ChartofAccounts.Name AS accountname FROM         dbo.BankAccountReceiptCustomer INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.CustomerAccount ON dbo.BankAccountReceiptCustomer.Voucherno = dbo.CustomerAccount.VoucherNo where  (BankAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //}
                    //if (cmbtype.Text == "Cash")
                    //{

                    //    q = "SELECT     dbo.ChartofAccounts.Name AS accountname, dbo.CashAccountReceiptCustomer.Date, dbo.CashAccountReceiptCustomer.Voucherno, dbo.CashAccountReceiptCustomer.Description,                       dbo.CashAccountReceiptCustomer.Debit, dbo.CashAccountReceiptCustomer.Credit, dbo.CashAccountReceiptCustomer.CurrentBalance FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CashAccountReceiptCustomer ON dbo.ChartofAccounts.Id = dbo.CashAccountReceiptCustomer.ChartAccountId INNER JOIN                      dbo.CustomerAccount ON dbo.CashAccountReceiptCustomer.Voucherno = dbo.CustomerAccount.VoucherNo where  (CashAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //}
                    //q = "SELECT     SUM(dbo.Saledetails.TotalPrice) AS sum, COUNT(dbo.Saledetails.TotalPrice) AS count, dbo.Sale.Date, dbo.RawItem.ItemName as name FROM         dbo.Saledetails INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.Sale.Date, dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT     dbo.CustomerAccount.Balance AS CurrentBalance, dbo.CustomerAccount.Credit, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Description, dbo.CustomerAccount.VoucherNo,                       dbo.CustomerAccount.Date, dbo.Customers.Name AS accountname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CustomerAccount ON dbo.ChartofAccounts.Id = dbo.CustomerAccount.PayableAccountId INNER JOIN                      dbo.Customers ON dbo.CustomerAccount.CustomersId = dbo.Customers.Id where CustomerAccount.CustomersId='" + cmbsupplier.SelectedValue + "' and  (CustomerAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                   
                    //if (cmbtype.Text == "Bank")
                    //{
                    //    q = "SELECT     dbo.BankAccountReceiptCustomer.Date, dbo.BankAccountReceiptCustomer.Voucherno, dbo.BankAccountReceiptCustomer.CheckNo, dbo.BankAccountReceiptCustomer.CheckDate,                      dbo.BankAccountReceiptCustomer.Description, dbo.BankAccountReceiptCustomer.Debit, dbo.BankAccountReceiptCustomer.Credit, dbo.BankAccountReceiptCustomer.CurrentBalance,                       dbo.ChartofAccounts.Name AS accountname FROM         dbo.BankAccountReceiptCustomer INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.CustomerAccount ON dbo.BankAccountReceiptCustomer.Voucherno = dbo.CustomerAccount.VoucherNo where BankAccountReceiptCustomer.CustomerId='" + cmbsupplier.SelectedValue + "' and (BankAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                        
                    //}
                    //if (cmbtype.Text == "Cash")
                    //{
                       
                    //    q = "SELECT     dbo.ChartofAccounts.Name AS accountname, dbo.CashAccountReceiptCustomer.Date, dbo.CashAccountReceiptCustomer.Voucherno, dbo.CashAccountReceiptCustomer.Description,                       dbo.CashAccountReceiptCustomer.Debit, dbo.CashAccountReceiptCustomer.Credit, dbo.CashAccountReceiptCustomer.CurrentBalance FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CashAccountReceiptCustomer ON dbo.ChartofAccounts.Id = dbo.CashAccountReceiptCustomer.ChartAccountId INNER JOIN                      dbo.CustomerAccount ON dbo.CashAccountReceiptCustomer.Voucherno = dbo.CustomerAccount.VoucherNo where CashAccountReceiptCustomer.CustomerId='" + cmbsupplier.SelectedValue + "' and (CashAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //}
                }
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                ds = objCore.funGetDataSet(q);
                string debit = "";
                string credit = "", blnce="";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    debit = ds.Tables[0].Rows[i]["Debit"].ToString();
                    credit = ds.Tables[0].Rows[i]["Credit"].ToString();
                    blnce = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                   
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                  
                        


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbsupplier.Text == "")
            {
                MessageBox.Show("Please Select Supplier");
                return;
            }
            
            bindreport();
        }
    }
}
