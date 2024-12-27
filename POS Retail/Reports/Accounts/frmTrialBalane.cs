using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace POSRetail.Reports.Accounts
{
    public partial class frmTrialBalane : Form
    {
        double ablnce = 0;
        double aopeningblnce = 0, aopeningdebit = 0, aopeningcredit = 0;
        double adebit = 0;
        double acredit = 0;
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmTrialBalane()
        {
            InitializeComponent();
        }

        private void frmaccounts_Load(object sender, EventArgs e)
        {
            //bindreport();
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


                POSRetail.Reports.Accounts.rptTrialBalance rptDoc = new rptTrialBalance();
                POSRetail.Reports.Accounts.dstrialbalance dsrpt = new dstrialbalance();
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
                dt = getAllOrders();
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
                        
                        dtrpt.Rows.Add("", "", "", "", "", "", "", "", "0", "", "", "", "", "0", "0", "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                        

                        dsrpt.Tables[0].Merge(dt);
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
        public DataTable dtrpt = new DataTable();
        public DataTable getAllOrders()
        {
            dtrpt = new DataTable();
            dtrpt.Columns.Add("Code", typeof(string));
            dtrpt.Columns.Add("Title", typeof(string));
            dtrpt.Columns.Add("OpeningBalance", typeof(string));
            dtrpt.Columns.Add("Debit", typeof(string));
            dtrpt.Columns.Add("Credit", typeof(string));
            dtrpt.Columns.Add("ClosingBalance", typeof(double));
            dtrpt.Columns.Add("OpeningDebit", typeof(string));
            dtrpt.Columns.Add("OpeningCredit", typeof(string));

            dtrpt.Columns.Add("aOpeningBalance", typeof(string));
            dtrpt.Columns.Add("aDebit", typeof(string));
            dtrpt.Columns.Add("aCredit", typeof(string));
            dtrpt.Columns.Add("aClosingBalance", typeof(string));
            dtrpt.Columns.Add("aOpeningDebit", typeof(string));
            dtrpt.Columns.Add("aOpeningCredit", typeof(string));
            dtrpt.Columns.Add("ClosingBalancec", typeof(double));
            dtrpt.Columns.Add("aClosingBalancec", typeof(string));
            dtrpt.Columns.Add("date", typeof(string));
            dtrpt.Columns.Add("logo", typeof(byte[]));
            ablnce = 0;
            aopeningblnce = 0; aopeningdebit = 0; aopeningcredit = 0;
            adebit = 0;
            acredit = 0;

            DataSet dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet("SELECT     Id FROM         ChartofAccounts ");
            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {
                getAllOrders1(dsaccounts.Tables[0].Rows[i][0].ToString());
            }
            return dtrpt;
        }
        public void getAllOrders1(string id)
        {

           
            try
            {
                


                double blnce = 0;
                double openingblnce = 0,openingdebit=0,openingcredit=0;
                double debit = 0;
                double credit = 0;
                DataSet ds = new DataSet();
                DataSet dsopening = new DataSet();
                string q = "";
                ds = new DataSet();
                string title = "";
                string code = "";

                try
                {
                    q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.BankAccountPaymentSupplier RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where (dbo.BankAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.ChartofAccounts.Id='" + id + "' GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    CurrentBalance,credit,debit FROM         BankAccountPaymentSupplier  WHERE   ChartAccountId='" + id + "' and  (Date < '" + dateTimePicker1.Text + "') and ChartAccountId='" + id + "' ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {
                            
                            val=dsopening.Tables[0].Rows[0][0].ToString();
                            if(val=="")
                            {
                                val="0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);
                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);


                        //val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                       // blnce = blnce + (debit - credit) ;


                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.BankAccountReceiptCustomer RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    CurrentBalance,credit,debit FROM         BankAccountReceiptCustomer  WHERE  ChartAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        //val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        //blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);

                        //blnce = blnce + (debit - credit);

                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }


                try
                {
                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.CashAccountPaymentSupplier.Debit) as Debit, sum(dbo.CashAccountPaymentSupplier.Credit) as Credit, sum(dbo.CashAccountPaymentSupplier.CurrentBalance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountPaymentSupplier RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CashAccountPaymentSupplier.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    CurrentBalance,credit,debit FROM         CashAccountPaymentSupplier  WHERE  ChartAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);
                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {

                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.CashAccountPurchase.Debit) as Debit, sum(dbo.CashAccountPurchase.Credit) as Credit, sum(dbo.CashAccountPurchase.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountPurchase RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountPurchase.ChartAccountId = dbo.ChartofAccounts.Id where  dbo.CashAccountPurchase.ChartAccountId='" + id + "' and (dbo.CashAccountPurchase.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         CashAccountPurchase  WHERE  ChartAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);
                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }

                }
                catch (Exception ex)
                {


                }

                try
                {
                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.CashAccountReceiptCustomer.Debit) as Debit, sum(dbo.CashAccountReceiptCustomer.Credit) as Credit, sum(dbo.CashAccountReceiptCustomer.CurrentBalance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountReceiptCustomer RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CashAccountReceiptCustomer.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    CurrentBalance,credit,debit FROM         CashAccountReceiptCustomer  WHERE   ChartAccountId='" + id + "' and  (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.CashAccountSales.Debit) as Debit, sum(dbo.CashAccountSales.Credit) as Credit, sum(dbo.CashAccountSales.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountSales RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountSales.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CashAccountSales.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         CashAccountSales  WHERE ChartAccountId='" + id + "' and    (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.CostSalesAccount.Debit) as Debit, sum(dbo.CostSalesAccount.Credit) as Credit, sum(dbo.CostSalesAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CostSalesAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CostSalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CostSalesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         CostSalesAccount  WHERE ChartAccountId='" + id + "' and    (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);


                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //  q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.CustomerAccount.Debit) as Debit, sum(dbo.CustomerAccount.Credit) as Credit, sum(dbo.CustomerAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CustomerAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CustomerAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and  (dbo.CustomerAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         CustomerAccount  WHERE  PayableAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);


                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";

                    q = "SELECT     sum(dbo.DiscountAccount.Debit) as Debit, sum(dbo.DiscountAccount.Credit) as Credit, sum(dbo.DiscountAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.DiscountAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.DiscountAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.DiscountAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         DiscountAccount  WHERE  ChartAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);


                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.GSTAccount.Debit) as Debit, sum(dbo.GSTAccount.Credit) as Credit, sum(dbo.GSTAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.GSTAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.GSTAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.GSTAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         GSTAccount  WHERE   ChartAccountId='" + id + "' and  (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);


                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.InventoryAccount.Debit) as Debit, sum(dbo.InventoryAccount.Credit) as Credit, sum(dbo.InventoryAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.InventoryAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.InventoryAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.InventoryAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         InventoryAccount  WHERE ChartAccountId='" + id + "' and    (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.JournalAccount.Debit) as Debit, sum(dbo.JournalAccount.Credit) as Credit, sum(dbo.JournalAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.JournalAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.JournalAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         JournalAccount  WHERE  PayableAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);


                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.SalesAccount.Debit) as Debit, sum(dbo.SalesAccount.Credit) as Credit, sum(dbo.SalesAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.SalesAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.SalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where  dbo.ChartofAccounts.Id='" + id + "' and (dbo.SalesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         SalesAccount  WHERE  ChartAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);


                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + cmbaccount.SelectedValue + "'";
                    q = "SELECT     sum(dbo.SupplierAccount.Debit) as Debit, sum(dbo.SupplierAccount.Credit) as Credit, sum(dbo.SupplierAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.SupplierAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode";

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        q = "SELECT top 1    Balance,credit,debit FROM         SupplierAccount  WHERE  PayableAccountId='" + id + "' and   (Date < '" + dateTimePicker1.Text + "') ORDER BY Date DESC";
                        dsopening = new System.Data.DataSet();
                        dsopening = objCore.funGetDataSet(q);
                        string val = "0";
                        if (dsopening.Tables[0].Rows.Count > 0)
                        {

                            val = dsopening.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingblnce = openingblnce + Convert.ToDouble(val);


                            val = dsopening.Tables[0].Rows[0][1].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingcredit = openingcredit + Convert.ToDouble(val);

                            val = dsopening.Tables[0].Rows[0][2].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            openingdebit = openingdebit + Convert.ToDouble(val);
                        }
                        val = "0";
                        val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        title = ds.Tables[0].Rows[i]["title"].ToString();
                        code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                    }


                }
                catch (Exception ex)
                {


                }
                if (title == "")
                {
                    
                }
                else
                {
                    blnce = openingblnce + (debit - credit);
                    string  blnco = "";
                    double blnc = 0;
                    
                    blnco = openingblnce.ToString();
                    //if (blnce.ToString().Contains("-"))
                    //{
                    //    blnc = "(" + blnce.ToString().Replace("-", "") + ")";
                    //}
                    if (openingblnce.ToString().Contains("-"))
                    {
                        blnco = "(" + openingblnce.ToString().Replace("-", "") + ")";
                    }
                    adebit = adebit + debit;
                    acredit = acredit + credit;
                    ablnce = ablnce + blnce;
                    aopeningdebit = aopeningdebit + openingdebit;
                    aopeningcredit = aopeningcredit + openingcredit;
                    aopeningblnce = aopeningblnce + openingblnce;
                    blnc = Math.Abs(blnce);

                    string logo = "";
                    try
                    {
                        logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                    }
                    catch (Exception ex)
                    {

                    }
                    

                    if (aopeningblnce >= 0)
                    {
                        if (blnce >= 0)
                        {
                            if (ablnce >= 0)
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", "0", "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", "0", "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                                }
                            }
                            else
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", "0", ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", "0", ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                       
                                    
                                }
                            }
                        }
                        else
                        {
                            if (ablnce >= 0)
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", blnc, "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                                }
                                else
                                {


                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", blnc, "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);

                                }
                            }
                            else
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", blnc, ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", blnc, ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                        

                                    

                                }
                            }
                           // dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", blnc);
                        }
                    }
                    if (aopeningblnce < 0)
                    {
                        if (blnce >= 0)
                        {
                            if (ablnce >= 0)
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", "0", "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                                }
                                else
                                {
                                    

                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", "0", "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);


                                }
                                
                            }
                            else
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", "0", ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                               
                                }
                                else
                                {


                                    
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", "0", ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);

                                }
                                
                            }
                           // dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", "0");
                        }
                        else
                        {
                            if (ablnce >= 0)
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", blnc, "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);

                                }
                                else
                                {

                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", blnc, "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);

                                    

                                }
                                
                            }
                            else
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", blnc, ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);

                                }
                                else
                                {

                                    

                                    dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, "", aopeningblnce, "0", blnc, ablnce, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);

                                }
                                
                            }

                           // dtrpt.Rows.Add(code, title, blnco, debit, credit, "0", openingdebit.ToString(), openingcredit.ToString(), "0", adebit, acredit, ablnce, aopeningblnce, "0", blnc);
                        }
                    }
                   // dtrpt.Rows.Add(code, title, blnco, debit, credit, blnc, openingdebit.ToString(), openingcredit.ToString(), aopeningblnce, adebit, acredit, ablnce, aopeningdebit, aopeningcredit);
                
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

        private void crystalReportViewer1_ReportRefresh(object source, CrystalDecisions.Windows.Forms.ViewerEventArgs e)
        {
            bindreport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

    }
}
