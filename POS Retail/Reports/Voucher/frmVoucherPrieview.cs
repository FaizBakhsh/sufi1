using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.Voucher
{
    public partial class frmVoucherPrieview : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string id = "";
        public string name = "";
        public frmVoucherPrieview()
        {
            InitializeComponent();
        }

        private void frmVoucherPrieview_Load(object sender, EventArgs e)
        {
            bindreport();
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

                getcompany();
                POSRetail.Reports.Voucher.rprVoucherPrieview rptDoc = new rprVoucherPrieview();
                POSRetail.Reports.Voucher.dsVoucherPreview dsrpt = new dsVoucherPreview();
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
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("accountcode", typeof(string));
                dtrpt.Columns.Add("accountname", typeof(string));
                dtrpt.Columns.Add("debit", typeof(double));
                dtrpt.Columns.Add("credit", typeof(double));
                dtrpt.Columns.Add("voucherno", typeof(string));
                dtrpt.Columns.Add("vouchername", typeof(string));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "", val="";
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                try
                {
                    
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.BankAccountPaymentSupplier.Date, dbo.BankAccountPaymentSupplier.Debit, dbo.BankAccountPaymentSupplier.Credit FROM         dbo.BankAccountPaymentSupplier INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where dbo.BankAccountPaymentSupplier.Voucherno='"+id+"'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val=ds.Tables[0].Rows[i]["Debit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        double debit=Convert.ToDouble(val);
                        val="";
                        val=ds.Tables[0].Rows[i]["Credit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        double credit=Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {
                            

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    
                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.BankAccountReceiptCustomer.Date, dbo.BankAccountReceiptCustomer.Debit, dbo.BankAccountReceiptCustomer.Credit FROM         dbo.BankAccountReceiptCustomer INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.BankAccountReceiptCustomer.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit,id, name);

                    }
                }
                catch (Exception ex)
                {


                }


                try
                {
                    ds = new DataSet();
                    //q = "select top 1 CurrentBalance from CashAccountPaymentSupplier where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";

                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.CashAccountPaymentSupplier.Date, dbo.CashAccountPaymentSupplier.Debit, dbo.CashAccountPaymentSupplier.Credit FROM         dbo.CashAccountPaymentSupplier INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountPaymentSupplier.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {

                    ds = new DataSet();
                   // q = "select top 1 Balance as CurrentBalance from CashAccountPurchase where Date <= '" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.CashAccountPurchase.Date, dbo.CashAccountPurchase.Debit, dbo.CashAccountPurchase.Credit FROM         dbo.CashAccountPurchase INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountPurchase.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountPurchase.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }

                }
                catch (Exception ex)
                {


                }

                try
                {
                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    //q = "SELECT     sum(dbo.CashAccountReceiptCustomer.Debit) as Debit, sum(dbo.CashAccountReceiptCustomer.Credit) as Credit, sum(dbo.CashAccountReceiptCustomer.CurrentBalance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountReceiptCustomer RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CashAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                   // q = "select top 1 CurrentBalance from CashAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.CashAccountReceiptCustomer.Date, dbo.CashAccountReceiptCustomer.Debit, dbo.CashAccountReceiptCustomer.Credit FROM         dbo.CashAccountReceiptCustomer INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountReceiptCustomer.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    // q = "SELECT     sum(dbo.CashAccountSales.Debit) as Debit, sum(dbo.CashAccountSales.Credit) as Credit, sum(dbo.CashAccountSales.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountSales RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountSales.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CashAccountSales.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    //q = "select top 1 Balance as CurrentBalance from CashAccountSales where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.CashAccountSales.Date, dbo.CashAccountSales.Debit, dbo.CashAccountSales.Credit FROM         dbo.CashAccountSales INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountSales.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountSales.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
                    //q = "select top 1     sum(dbo.CostSalesAccount.Debit) as Debit, sum(dbo.CostSalesAccount.Credit) as Credit, sum(dbo.CostSalesAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CostSalesAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CostSalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CostSalesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    //q = "select top 1 Balance as CurrentBalance from CostSalesAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.CostSalesAccount.Date, dbo.CostSalesAccount.Debit, dbo.CostSalesAccount.Credit FROM         dbo.CostSalesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.CostSalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CostSalesAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //  q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + cmbaccount.select top 1edValue + "'";
                    // q = "select top 1     sum(dbo.CustomerAccount.Debit) as Debit, sum(dbo.CustomerAccount.Credit) as Credit, sum(dbo.CustomerAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CustomerAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CustomerAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and  (dbo.CustomerAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    //q = "select top 1 Balance as CurrentBalance from CustomerAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.CustomerAccount.Date, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Credit FROM         dbo.CustomerAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.CustomerAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.CustomerAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);

                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";

                    //q = "select top 1 Balance as CurrentBalance from DiscountAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.DiscountAccount.Date, dbo.DiscountAccount.Debit, dbo.DiscountAccount.Credit FROM         dbo.DiscountAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.DiscountAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.DiscountAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
                    //q = "select top 1     sum(dbo.GSTAccount.Debit) as Debit, sum(dbo.GSTAccount.Credit) as Credit, sum(dbo.GSTAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.GSTAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.GSTAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.GSTAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                   // q = "select top 1 Balance as CurrentBalance from GSTAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.GSTAccount.Date, dbo.GSTAccount.Debit, dbo.GSTAccount.Credit FROM         dbo.GSTAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.GSTAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.GSTAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
                    //q = "select top 1     sum(dbo.InventoryAccount.Debit) as Debit, sum(dbo.InventoryAccount.Credit) as Credit, sum(dbo.InventoryAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.InventoryAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.InventoryAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.InventoryAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    //q = "select top 1 Balance as CurrentBalance from InventoryAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.InventoryAccount.Date, dbo.InventoryAccount.Debit, dbo.InventoryAccount.Credit FROM         dbo.InventoryAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.InventoryAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.InventoryAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + cmbaccount.select top 1edValue + "'";
                    // q = "select top 1     sum(dbo.JournalAccount.Debit) as Debit, sum(dbo.JournalAccount.Credit) as Credit, sum(dbo.JournalAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.JournalAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.JournalAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    //q = "select top 1 Balance as CurrentBalance from JournalAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.JournalAccount.Date, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
                    //q = "select top 1     sum(dbo.SalesAccount.Debit) as Debit, sum(dbo.SalesAccount.Credit) as Credit, sum(dbo.SalesAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.SalesAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.SalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where  dbo.ChartofAccounts.Id='" + id + "' and (dbo.SalesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    //q = "select top 1 Balance as CurrentBalance from SalesAccount where Date <'" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.SalesAccount.Date, dbo.SalesAccount.Debit, dbo.SalesAccount.Credit FROM         dbo.SalesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.SalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.SalesAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + cmbaccount.select top 1edValue + "'";
                    // q = "select top 1     sum(dbo.SupplierAccount.Debit) as Debit, sum(dbo.SupplierAccount.Credit) as Credit, sum(dbo.SupplierAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.SupplierAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    //q = "select top 1 Balance as CurrentBalance from SupplierAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' order by id desc";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name, dbo.SupplierAccount.Date, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Credit FROM         dbo.SupplierAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.SupplierAccount.Voucherno='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double debit = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double credit = Convert.ToDouble(val);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                    }

                }
                catch (Exception ex)
                {


                }

               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
    }
}
