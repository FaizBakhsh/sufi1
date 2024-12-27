using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Accounts
{
    public partial class frmBalaneSheet : Form
    {
        double ablnce = 0;
        double aopeningblnce = 0, aopeningdebit = 0, aopeningcredit = 0;
        double adebit = 0;
        double netprofit = 0, netprofitbf = 0;
       string min ="";// Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString()).ToShortDateString();
      //  string max = "";//Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString()).ToShortDateString();
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmBalaneSheet()
        {
            InitializeComponent();
        }

        private void frmaccounts_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    dateTimePicker1.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
            //    dateTimePicker1.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
            //}
            //catch (Exception ex)
            //{


            //}
            
            //bindreport();
            min = "2010-01-04";
            //max=dateTimePicker1
            DataSet ds = new System.Data.DataSet();
            string q = "SELECT     Id,  BranchName  FROM         Branch ";
            ds = new System.Data.DataSet();
            ds = objCore.funGetDataSet(q);
            DataRow dr = ds.Tables[0].NewRow();
            dr["BranchName"] = "All";
            ds.Tables[0].Rows.Add(dr);
            cmbbranchjv.DataSource = ds.Tables[0];
            cmbbranchjv.ValueMember = "id";
            cmbbranchjv.DisplayMember = "BranchName";
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


                POSRestaurant.Reports.Accounts.rptBalanceSheet rptDoc = new rptBalanceSheet();
                POSRestaurant.Reports.Accounts.dsBalancesheet dsrpt = new  dsBalancesheet();
                //feereport ds = new feereport(); // .xsd file name

                getcompany();
                string company = "", phone = "", address = "",logo="";
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
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders2();
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt);
                }
                else
                {
                    if (logo == "")
                    { }
                    else
                    {
                        dt.Rows.Add("", "", "", "", "", "", "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                        dsrpt.Tables[0].Merge(dt);
                    }
                }

                rptDoc.SetDataSource(dsrpt);                
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
                crystalReportViewer1.ReportSource = rptDoc;
                crystalReportViewer1.Zoom(50);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable dtrpt = new DataTable();
        public DataTable getAllOrders2()
        {
            dtrpt = new DataTable();
            dtrpt.Columns.Add("Type", typeof(string));
            dtrpt.Columns.Add("AccountType", typeof(string));
            dtrpt.Columns.Add("Name", typeof(string));
            dtrpt.Columns.Add("Amount", typeof(string));
            dtrpt.Columns.Add("Total", typeof(string));
            dtrpt.Columns.Add("grandTotal", typeof(string));
            dtrpt.Columns.Add("date", typeof(string));
            dtrpt.Columns.Add("logo", typeof(byte[]));
            ablnce = 0;
            aopeningblnce = 0; aopeningdebit = 0; aopeningcredit = 0;
            adebit = 0;
           
            string tp = "";
            getcompany();
            
            DataSet dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet("SELECT     id,  Name FROM         ChartofAccounts where AccountType='Fixed Assets'  order by id");
            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {
                string rem = "";
                if (dsaccounts.Tables[0].Rows.Count - 1 == i)
                {
                    rem = "0";
                }
                getAllOrders1(dsaccounts.Tables[0].Rows[i]["id"].ToString(), "Assets", "Fixed Assets", dsaccounts.Tables[0].Rows[i]["Name"].ToString(),rem);
            }
            dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet("SELECT     id,  Name FROM         ChartofAccounts where AccountType='Current Assets'  order by id");
            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {
                string rem = "";
                if (dsaccounts.Tables[0].Rows.Count - 1 == i)
                {
                    rem = "0";
                }
                getAllOrders1(dsaccounts.Tables[0].Rows[i]["id"].ToString(), "Assets", "Current Assets", dsaccounts.Tables[0].Rows[i]["Name"].ToString(), rem);
            }

            dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet("SELECT     id,  Name FROM         ChartofAccounts where AccountType='Equity and Capital'  order by id");
            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {
                string rem = "";
                if (dsaccounts.Tables[0].Rows.Count - 1 == i)
                {
                    rem = "0";
                }
                getAllOrders1(dsaccounts.Tables[0].Rows[i]["id"].ToString(), "Liabilities", "Equity and Capital", dsaccounts.Tables[0].Rows[i]["Name"].ToString(), rem);
            }
            dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet("SELECT     id,  Name FROM         ChartofAccounts where AccountType='Long Term Liabilities'  order by id");
            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {
                string rem = "";
                if (dsaccounts.Tables[0].Rows.Count - 1 == i)
                {
                    rem = "0";
                }
                getAllOrders1(dsaccounts.Tables[0].Rows[i]["id"].ToString(), "Liabilities", "Long Term Liabilities", dsaccounts.Tables[0].Rows[i]["Name"].ToString(), rem);
            }
            dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet("SELECT     id,  Name FROM         ChartofAccounts where AccountType='Current Liabilities'  order by id");
            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {
                string rem = "";
                if (dsaccounts.Tables[0].Rows.Count - 1 == i)
                {
                    rem = "0";
                }
                getAllOrders1(dsaccounts.Tables[0].Rows[i]["id"].ToString(), "Liabilities", "Current Liabilities", dsaccounts.Tables[0].Rows[i]["Name"].ToString(), rem);
            }
            return dtrpt;
        }
       
        public void getAllOrders1(string id , string type, string accounttype, string name, string count)
        {            
            try
            {
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }

                double blnce = 0;
                
                DataSet ds = new DataSet();
                DataSet dsopening = new DataSet();
                string q = "";
                ds = new DataSet();
                string title = "";
                string code = "";
                string val = "";
                double debit = 0, credit = 0, balance = 0;
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from BankAccountPaymentSupplier where Date <= '" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from BankAccountPaymentSupplier where Date <= '" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                       
                    }
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                   // q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.BankAccountReceiptCustomer RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' ";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'  and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                    }
                }
                catch (Exception ex)
                {


                }


                try
                {
                    ds = new DataSet();
                   
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountPaymentSupplier where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountPaymentSupplier where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {

                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                   // q = "SELECT     sum(dbo.CashAccountPurchase.Debit) as Debit, sum(dbo.CashAccountPurchase.Credit) as Credit, sum(dbo.CashAccountPurchase.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountPurchase RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountPurchase.ChartAccountId = dbo.ChartofAccounts.Id where  dbo.CashAccountPurchase.ChartAccountId='" + id + "' and (dbo.CashAccountPurchase.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountPurchase where Date <= '" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountPurchase where Date <= '" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' ";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountSales where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' ";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CashAccountSales where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CostSalesAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CostSalesAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CustomerAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' ";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from CustomerAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from BranchAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' ";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from BranchAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from DiscountAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from DiscountAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from GSTAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from GSTAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from InventoryAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from InventoryAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                  
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from JournalAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from JournalAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from SalesAccount where Date <'" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from SalesAccount where Date <'" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from SupplierAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' ";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from SupplierAccount where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
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
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from PettyCash where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "select SUM(Credit) AS credit, SUM(Debit) AS debit from PettyCash where Date <='" + dateTimePicker1.Text + "'  and PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {


                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        val = ds.Tables[0].Rows[i]["debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = (debit - credit).ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                    }

                }
                catch (Exception ex)
                {


                }

                string blnc = "";
                double total = 0, grtotal = 0, grtotal1 = 0;
                if (type == "Liabilities")
                {
                    if (Math.Round(blnce, 2).ToString().Contains("-"))
                    {
                        blnc = Math.Abs(Math.Round(blnce, 2)).ToString();// "(" + Math.Round(blnce, 2).ToString().Replace("-", "") + ")";
                    }
                    else
                    {
                        if (blnce == 0)
                        {
                            blnc = Math.Round(blnce, 2).ToString().Replace("-", "") ;//blnc = Math.Abs(Math.Round(blnce, 2)).ToString();
                        }
                        else
                        {
                            blnc = "(" + Math.Round(blnce, 2).ToString().Replace("-", "") + ")";//blnc = Math.Abs(Math.Round(blnce, 2)).ToString();
                        }
                    }
                }

                if (type == "Assets")
                {
                    if (Math.Round(blnce, 2).ToString().Contains("-"))
                    {
                        blnc = "(" + Math.Round(blnce, 2).ToString().Replace("-", "") + ")";//(Math.Round(blnce, 2)).ToString();//
                    }
                    else
                    {
                        blnc = blnc = (Math.Round(blnce, 2)).ToString();
                    }
                }
                

                if (dtrpt.Rows.Count >= 1)
                {
                    if (count == "0")
                    {
                        bool chk = false;
                        if (accounttype == "Equity and Capital")
                        {
                            getprofitloss();
                            getprofitlossbf();
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(type, accounttype, "Accumulate Profit/Loss ", netprofitbf, "", "", "as on " + dateTimePicker1.Text,null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(type, accounttype, "Accumulate Profit/Loss ", netprofitbf, "", "", "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(type, accounttype, "Current Profit/Loss ", netprofit, "", "", "as on " + dateTimePicker1.Text, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(type, accounttype, "Current Profit/Loss ", netprofit, "", "", "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(type, accounttype, name, blnc, "", "", "as on " + dateTimePicker1.Text, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(type, accounttype, name, blnc, "", "", "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                       
                        }
                        
                        for (int i = 0; i < dtrpt.Rows.Count; i++)
                        {
                            val = "0";
                            if (dtrpt.Rows[i]["AccountType"].ToString() == accounttype)
                            {
                                val = dtrpt.Rows[i]["Amount"].ToString();
                                if (val == string.Empty)
                                {
                                    val = "0";
                                }
                                if (val.Contains("("))
                                {
                                    val ="-"+ val.Replace("(", "").Replace(")", "");
                                }
                                total = total + Convert.ToDouble(val);

                                
                            }
                            if (dtrpt.Rows[i]["Type"].ToString() == "Assets")
                            {
                                val = dtrpt.Rows[i]["Amount"].ToString();
                                if (val == string.Empty)
                                {
                                    val = "0";
                                }
                                if (val.Contains("("))
                                {
                                    val = "-" + val.Replace("(", "").Replace(")", "");
                                }
                                grtotal = grtotal + Convert.ToDouble(val);
                                chk = true;
                                //string temp = "";
                                //if (Math.Round(grtotal, 2).ToString().Contains("-"))
                                //{
                                //    temp = "(" + Math.Round(grtotal, 2).ToString().Replace("-", "") + ")";
                                //}
                                //else
                                //{
                                //    temp = Math.Round(grtotal, 2).ToString();
                                
                                //}
                               // dtrpt.Rows.Add(type, accounttype, "Total " + accounttype, "","", grtotal);
                            }

                            if (dtrpt.Rows[i]["Type"].ToString() == "Liabilities")
                            {
                               
                               
                                val = dtrpt.Rows[i]["Amount"].ToString();
                                if (val == string.Empty)
                                {
                                    val = "0";
                                }
                                chk = true;
                                if (val.Contains("("))
                                {
                                    val = "-" + val.Replace("(", "").Replace(")", "");
                                }
                                grtotal1 = grtotal1 + Convert.ToDouble(val);
                                chk = true;
                               
                                //string temp = "";
                                //if (Math.Round(grtotal, 2).ToString().Contains("-"))
                                //{
                                //    temp = "(" + Math.Round(grtotal, 2).ToString().Replace("-", "") + ")";
                                //}
                                //else
                                //{
                                //    temp = Math.Round(grtotal, 2).ToString();
                                //}
                                //dtrpt.Rows.Add(type, accounttype, "Total " + accounttype, "", "", grtotal);
                            }


                        }

                        string temp = "";
                        if (Math.Round(total, 2).ToString().Contains("-"))
                        {
                            temp = "(" + Math.Round(total, 2).ToString().Replace("-", "") + ")";
                        }
                        else
                        {
                            temp = Math.Round(total, 2).ToString();

                        }
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(type, accounttype, "Total " + accounttype, "", temp, "", "as on " + dateTimePicker1.Text, null);
                        }
                        else
                        {

                            dtrpt.Rows.Add(type, accounttype, "Total " + accounttype, "", temp, "", "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                        }
                        

                        if ((chk == true && accounttype == "Current Assets"))
                        {
                            string temp1 = "";
                            if (Math.Round(grtotal, 2).ToString().Contains("-"))
                            {
                                temp1 = "(" + Math.Round(grtotal, 2).ToString().Replace("-", "") + ")";
                            }
                            else
                            {
                                temp1 = Math.Round(grtotal, 2).ToString();
                            }
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(type, accounttype, "Total " + type, "", "", temp1, "as on " + dateTimePicker1.Text, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(type, accounttype, "Total " + type, "", "", temp1, "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                                
                            }
                            
                            chk = false;
                        }
                        if ((chk == true && accounttype == "Current Liabilities") )
                        {
                            string temp1 = "";
                            if (Math.Round(grtotal1, 2).ToString().Contains("-"))
                            {
                                temp1 = "(" + Math.Round(grtotal1, 2).ToString().Replace("-", "") + ")";
                            }
                            else
                            {
                                temp1 = Math.Round(grtotal1, 2).ToString();
                            }
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(type, accounttype, "Total " + type, "", "", temp1, "as on " + dateTimePicker1.Text, null);
                            }
                            else
                            {

                                dtrpt.Rows.Add(type, accounttype, "Total " + type, "", "", temp1, "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                            
                            chk = false;
                        }
                        grtotal = 0;
                        
                        
                    }
                    else
                    {
                        grtotal = 0;
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(type, accounttype, name, blnc, "", "", "as on " + dateTimePicker1.Text, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(type, accounttype, name, blnc, "", "", "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                          
                        }
                        
                    }
                }
                else
                {
                    grtotal = 0;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(type, accounttype, name, blnc, "", "", "as on " + dateTimePicker1.Text, null);
                    }
                    else
                    {

                        dtrpt.Rows.Add(type, accounttype, name, blnc, "", "", "as on " + dateTimePicker1.Text, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    
                }
                
                
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

        private void crystalReportViewer1_Search(object source, CrystalDecisions.Windows.Forms.SearchEventArgs e)
        {

        }
        public void getprofitloss()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                
                double sales = 0, discount = 0, netsales = 0, costofsales = 0, grossprofit = 0, adminexp = 0, operexp = 0, financexp = 0,marketingexp=0;

                string val = "0";

                DataSet ds = new DataSet();
                string q = "";
                ds = new DataSet();
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Credit) as Sales,sum(debit) as Salesret FROM         SalesAccount where (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Credit) as Sales,sum(debit) as Salesret FROM         SalesAccount where (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Salesret"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }

                        double rest = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Sales"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        sales = Convert.ToDouble(val);
                        sales = sales - rest;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Discount,sum(credit) as Discountret FROM         DiscountAccount where (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Discount,sum(credit) as Discountret FROM         DiscountAccount where (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = (ds.Tables[0].Rows[i]["Discountret"].ToString());
                        if (val == "")
                        {
                            val = "0";
                        }
                        double rest = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Discount"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        discount = Convert.ToDouble(val);
                        discount = discount - rest;
                    }
                }
                catch (Exception ex)
                {


                }
                netsales = sales;// -discount;
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["costsalesrest"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double rest = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["costsales"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        costofsales = Convert.ToDouble(val);
                        costofsales = costofsales - rest;
                    }
                }
                catch (Exception ex)
                {


                }
                grossprofit = netsales - costofsales;
                DataSet dsexp = new System.Data.DataSet();
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                    }
                    else
                    {

                    }
                    q = "select id from ChartofAccounts where AccountType='Operating Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                    
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                    
                        }
                           ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpret"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            //double rest = Convert.ToDouble(ds.Tables[0].Rows[i]["opexpret"].ToString());
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            operexp = operexp + Convert.ToDouble(val);
                            operexp = operexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                     
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                     
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpret"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            //double rest = Convert.ToDouble(ds.Tables[0].Rows[i]["opexpret"].ToString());
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            operexp = operexp + Convert.ToDouble(val);
                            operexp = operexp - rest;
                        }
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses'";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            adminexp = adminexp + Convert.ToDouble(val);
                            adminexp = adminexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                     
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                     
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            adminexp = adminexp + Convert.ToDouble(val);
                            adminexp = adminexp - rest;
                        }

                    }
                    adminexp = adminexp + discount;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Financial Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') ";
                    
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                    
                        }
                          ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            financexp = financexp + Convert.ToDouble(val);
                            financexp = financexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            financexp = financexp + Convert.ToDouble(val);
                            financexp = financexp - rest;
                        }
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Marketing Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                     
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                     
                        }
                          ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            marketingexp = marketingexp + Convert.ToDouble(val);
                            marketingexp = marketingexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            marketingexp = marketingexp + Convert.ToDouble(val);
                            marketingexp = marketingexp - rest;
                        }
                    }
                }
                catch (Exception ex)
                {


                }

               
               
               
                double revenue = 0;
                dsexp = new System.Data.DataSet();
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Revenue' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                   
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                   

                        }
                            ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpret"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            //double rest = Convert.ToDouble(ds.Tables[0].Rows[i]["opexpret"].ToString());
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            revenue = revenue + Convert.ToDouble(val);
                            revenue = revenue - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + min + "' and '" + dateTimePicker1.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpret"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            //double rest = Convert.ToDouble(ds.Tables[0].Rows[i]["opexpret"].ToString());
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            revenue = revenue + Convert.ToDouble(val);
                            revenue = revenue - rest;
                        }
                    }
                }
                catch (Exception ex)
                {


                }
                double totalexp = financexp + operexp + adminexp + marketingexp;

                netprofit = (netsales ) - (costofsales + totalexp);
                netprofit = netprofit - revenue;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        public void getprofitlossbf()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                double sales = 0, discount = 0, netsales = 0, costofsales = 0, grossprofit = 0, adminexp = 0, operexp = 0, financexp = 0, marketingexp = 0;

                string val = "0";

                DataSet ds = new DataSet();
                string q = "";
                ds = new DataSet();
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Credit) as Sales,sum(debit) as Salesret FROM         SalesAccount where (Date <  '" + min + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Credit) as Sales,sum(debit) as Salesret FROM         SalesAccount where (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }

                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Salesret"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }

                        double rest = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Sales"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        sales = Convert.ToDouble(val);
                        sales = sales - rest;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Discount,sum(credit) as Discountret FROM         DiscountAccount where (Date <  '" + min + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Discount,sum(credit) as Discountret FROM         DiscountAccount where (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";

                    }
                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = (ds.Tables[0].Rows[i]["Discountret"].ToString());
                        if (val == "")
                        {
                            val = "0";
                        }
                        double rest = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["Discount"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        discount = Convert.ToDouble(val);
                        discount = discount - rest;
                    }
                }
                catch (Exception ex)
                {


                }
                netsales = sales;// -discount;
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date <  '" + min + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["costsalesrest"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        double rest = Convert.ToDouble(val);
                        val = "";
                        val = ds.Tables[0].Rows[i]["costsales"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        costofsales = Convert.ToDouble(val);
                        costofsales = costofsales - rest;
                    }
                }
                catch (Exception ex)
                {


                }
                grossprofit = netsales - costofsales;
                DataSet dsexp = new System.Data.DataSet();
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Operating Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpret"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            //double rest = Convert.ToDouble(ds.Tables[0].Rows[i]["opexpret"].ToString());
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            operexp = operexp + Convert.ToDouble(val);
                            operexp = operexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpret"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            //double rest = Convert.ToDouble(ds.Tables[0].Rows[i]["opexpret"].ToString());
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            operexp = operexp + Convert.ToDouble(val);
                            operexp = operexp - rest;
                        }
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                       
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                       
                        }
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            adminexp = adminexp + Convert.ToDouble(val);
                            adminexp = adminexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            adminexp = adminexp + Convert.ToDouble(val);
                            adminexp = adminexp - rest;
                        }

                    }
                    adminexp = adminexp + discount;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Financial Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            financexp = financexp + Convert.ToDouble(val);
                            financexp = financexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            financexp = financexp + Convert.ToDouble(val);
                            financexp = financexp - rest;
                        }
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Marketing Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            marketingexp = marketingexp + Convert.ToDouble(val);
                            marketingexp = marketingexp - rest;
                        }
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
                        }
                         ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexpres"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double rest = Convert.ToDouble(val);
                            val = "";
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            marketingexp = marketingexp + Convert.ToDouble(val);
                            marketingexp = marketingexp - rest;
                        }
                    }
                }
                catch (Exception ex)
                {


                }

              

               
               
                
               

                double totalexp = financexp + operexp + adminexp + marketingexp;

                netprofitbf = netsales - (costofsales + totalexp);


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            bindreport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Please Wait(Downloading Data)";
            button1.Enabled = false;
            bindreport();
            button1.Text = "Submit";
            button1.Enabled = true;
        }

    }
}
