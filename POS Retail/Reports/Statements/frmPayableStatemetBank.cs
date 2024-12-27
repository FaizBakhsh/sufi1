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
    public partial class frmPayableStatemetBank : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmPayableStatemetBank()
        {
            InitializeComponent();
        }

        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet ds = new DataSet();
               string q = "select id,name from Supplier";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All Suppliers";
                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "name";
                cmbsupplier.Text = "All Suppliers";
                
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


                POSRetail.Reports.Statements.rptpayablestatementbank rptDoc = new rptpayablestatementbank();
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
                if (cmbsupplier.Text == "All Suppliers")
                {
                    q = "SELECT     dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                       dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.SupplierAccount ON dbo.ChartofAccounts.Id = dbo.SupplierAccount.PayableAccountId INNER JOIN                     dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where  (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //if (cmbtype.Text == "Bank")
                    //{
                    //    q = "SELECT     dbo.ChartofAccounts.Name AS accountname, dbo.SupplierAccount.VoucherNo, dbo.SupplierAccount.CheckNo, dbo.SupplierAccount.Description, dbo.SupplierAccount.Debit,                       dbo.SupplierAccount.Credit, dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Date FROM         dbo.BankAccountPaymentSupplier INNER JOIN                      dbo.SupplierAccount ON dbo.BankAccountPaymentSupplier.Voucherno = dbo.SupplierAccount.VoucherNo INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where  (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //}
                    //if (cmbtype.Text == "Cash")
                    //{
                    //    q = "SELECT     dbo.ChartofAccounts.Name AS accountname, dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description,                       dbo.SupplierAccount.VoucherNo, dbo.SupplierAccount.Date FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CashAccountPaymentSupplier ON dbo.ChartofAccounts.Id = dbo.CashAccountPaymentSupplier.ChartAccountId INNER JOIN                      dbo.SupplierAccount ON dbo.CashAccountPaymentSupplier.Voucherno = dbo.SupplierAccount.VoucherNo where  (SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //}
                    //q = "SELECT     SUM(dbo.Saledetails.TotalPrice) AS sum, COUNT(dbo.Saledetails.TotalPrice) AS count, dbo.Sale.Date, dbo.RawItem.ItemName as name FROM         dbo.Saledetails INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.Sale.Date, dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT     dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                       dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.SupplierAccount ON dbo.ChartofAccounts.Id = dbo.SupplierAccount.PayableAccountId INNER JOIN                     dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.SupplierAccount.SupplierId='" + cmbsupplier.SelectedValue + "' and  (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                  
                    //if (cmbtype.Text == "Bank")
                    //{

                    //    //q = "SELECT     dbo.BankAccountPaymentSupplier.Date, dbo.BankAccountPaymentSupplier.Voucherno, dbo.BankAccountPaymentSupplier.CheckNo, dbo.BankAccountPaymentSupplier.Description,                      dbo.BankAccountPaymentSupplier.Debit, dbo.BankAccountPaymentSupplier.Credit, dbo.BankAccountPaymentSupplier.CurrentBalance, dbo.ChartofAccounts.Name AS accountname FROM         dbo.BankAccountPaymentSupplier INNER JOIN                      dbo.SupplierAccount ON dbo.BankAccountPaymentSupplier.Voucherno = dbo.SupplierAccount.VoucherNo INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where dbo.BankAccountPaymentSupplier.SupplierId='" + cmbsupplier.SelectedValue + "' and (BankAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //    q = "SELECT     dbo.ChartofAccounts.Name AS accountname, dbo.SupplierAccount.VoucherNo, dbo.SupplierAccount.CheckNo, dbo.SupplierAccount.Description, dbo.SupplierAccount.Debit,                       dbo.SupplierAccount.Credit, dbo.SupplierAccount.Balance AS currentbalance, dbo.SupplierAccount.Date FROM         dbo.BankAccountPaymentSupplier INNER JOIN                      dbo.SupplierAccount ON dbo.BankAccountPaymentSupplier.Voucherno = dbo.SupplierAccount.VoucherNo INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where dbo.SupplierAccount.SupplierId='" + cmbsupplier.SelectedValue + "' and  (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //}
                    //if (cmbtype.Text == "Cash")
                    //{
                    //    //q = "SELECT     dbo.CashAccountPaymentSupplier.Date, dbo.CashAccountPaymentSupplier.Voucherno, dbo.CashAccountPaymentSupplier.Description, dbo.CashAccountPaymentSupplier.Debit,                       dbo.CashAccountPaymentSupplier.Credit, dbo.CashAccountPaymentSupplier.CurrentBalance, dbo.ChartofAccounts.Name AS accountname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CashAccountPaymentSupplier ON dbo.ChartofAccounts.Id = dbo.CashAccountPaymentSupplier.ChartAccountId INNER JOIN                      dbo.SupplierAccount ON dbo.CashAccountPaymentSupplier.Voucherno = dbo.SupplierAccount.VoucherNo where dbo.CashAccountPaymentSupplier.SupplierId='" + cmbsupplier.SelectedValue + "' and  (CashAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    //    q = "SELECT     dbo.ChartofAccounts.Name AS accountname, dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description,                       dbo.SupplierAccount.VoucherNo, dbo.SupplierAccount.Date FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CashAccountPaymentSupplier ON dbo.ChartofAccounts.Id = dbo.CashAccountPaymentSupplier.ChartAccountId INNER JOIN                      dbo.SupplierAccount ON dbo.CashAccountPaymentSupplier.Voucherno = dbo.SupplierAccount.VoucherNo where dbo.SupplierAccount.SupplierId='" + cmbsupplier.SelectedValue + "' and  (SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
              
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
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string debit = "";
                    string credit = "";
                    string blnce = "";
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
