using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Threading;
using System.Data.SqlClient;

namespace POSRestaurant.Reports.Accounts
{
    public partial class frmTrialBalane2 : Form
    {
        double ablnce = 0;
        double aopeningblnce = 0, aopeningdebit = 0, aopeningcredit = 0;
        double adebit = 0;
        double acredit = 0;
        //string min = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString()).ToShortDateString();
        //string max = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString()).ToShortDateString();
       public static string min ="";// Convert.ToDateTime(dateTimePicker2.Text).ToShortDateString();
       public static string max = "";// Convert.ToDateTime(dateTimePicker2.Text).ToShortDateString();
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmTrialBalane2()
        {
            InitializeComponent();
        }
        private void frmaccounts_Load(object sender, EventArgs e)
        {
            //try
            //{
            //    dateTimePicker2.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
            //    dateTimePicker2.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
            //}
            //catch (Exception ex)
            //{


            //}
            //bindreport();
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
               DateTime From = Convert.ToDateTime(dateTimePicker1.Text);
               DateTime To = Convert.ToDateTime(dateTimePicker2.Text);
                DataTable dt = new DataTable();
                POSRestaurant.Reports.Accounts.rptTrialBalance2 rptDoc = new rptTrialBalance2();
                POSRestaurant.Reports.Accounts.dstrialbalance2 dsrpt = new dstrialbalance2();
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
                dt = getAllTrialBalance(From, To);
                //dt = getAllOrders();
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
                        dtrpt.Rows.Add("", "", "0", "0", "0", "0", "0", "0", dscompany.Tables[0].Rows[0]["logo"],"");                        
                        dsrpt.Tables[0].Merge(dt);
                    }
                }               
                rptDoc.SetDataSource(dsrpt);                
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period  "+dateTimePicker1.Text+" to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public DataTable dtrpt = new DataTable();

        public DataTable getAllTrialBalance(DateTime From, DateTime To)
        {
            string head, accounttype, subaccount, titl = "";
            double openingdebit, openingcredit, debit, credit, blnce = 0;
            double ClosingDebit, ClosingCredit = 0;
            int IsDebit = 0;
            string logo = "";
            
            dtrpt = new DataTable();
            dtrpt.Columns.Add("Head", typeof(string));
            dtrpt.Columns.Add("SubHead", typeof(string));
            dtrpt.Columns.Add("SubAccount", typeof(string));
            dtrpt.Columns.Add("Title", typeof(string));
            dtrpt.Columns.Add("OpeningDebit", typeof(double));
            dtrpt.Columns.Add("OpeningCredit", typeof(double));
            dtrpt.Columns.Add("Debit", typeof(double));
            dtrpt.Columns.Add("Credit", typeof(double));
            dtrpt.Columns.Add("ClosingDebit", typeof(double));
            dtrpt.Columns.Add("ClosingCredit", typeof(double));
            dtrpt.Columns.Add("logo", typeof(byte[]));
            dtrpt.Columns.Add("branch", typeof(string));

            DataSet dsaccounts = new System.Data.DataSet();

            double acprft = getprofitlossbf();
            if (acprft < 0)
            {
                if (logo == "")
                {
                    dtrpt.Rows.Add("", "", "", "Accumulate Profit/Loss", 0, 0, 0, 0, acprft, 0, null, cmbbranchjv.Text);
                }
                else
                {
                    dtrpt.Rows.Add("", "", "", "Accumulate Profit/Loss", 0, 0, 0, 0, acprft, 0, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                }
            }
            else
            {
                if (logo == "")
                {
                    dtrpt.Rows.Add("", "", "", "Accumulate Profit/Loss", 0, 0, 0, 0, 0, acprft, null, cmbbranchjv.Text);
                }
                else
                {
                    dtrpt.Rows.Add("", "", "", "Accumulate Profit/Loss", 0, 0, 0, 0, 0, acprft, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                }
            }
            //return dtrpt;
                   

                


            dsaccounts = GeTrialBalanceByDate(From, To);

            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {

                DataRow row = dsaccounts.Tables[0].Rows[i];
                 head = row.Field<string>("Head");
                 accounttype = row.Field<string>("accounttype");
                 subaccount = row.Field<string>("subaccount");
                 titl = row.Field<string>("Name");

                 openingdebit = row.Field<double>("OpeningDebit");
                 openingcredit = row.Field<double>("OpeningCredit");
                 debit = row.Field<double>("debit");
                 credit = row.Field<double>("credit");
               
                 

                 IsDebit = row.Field<int>("IsDebit");

                 if (openingdebit > 0 & openingcredit > 0)
                 {
                     if (IsDebit == 1)
                     {
                         double netOpeningDebit = openingdebit - openingcredit;
                         if (netOpeningDebit > 0)
                         {

                             openingdebit = netOpeningDebit;
                             openingcredit = 0;

                         }
                         else
                         {

                             openingcredit = netOpeningDebit;
                             openingdebit = 0;
                         }


                     }
                     else {

                         double netOpeningCredit =openingcredit- openingdebit ;
                         if (netOpeningCredit > 0)
                         {

                            
                             openingcredit = netOpeningCredit;
                             openingdebit = 0;

                         }
                         else
                         {

                             openingdebit = netOpeningCredit;
                             openingcredit = 0;

                         }
                     
                     }
                 
                 
                 }

                

                 ClosingDebit = openingdebit + debit;
                 ClosingCredit = openingcredit + credit;

                 if (ClosingDebit > 0 & ClosingCredit > 0)
                 {

                     if (IsDebit == 1)
                     {
                         double netClosingDebit = ClosingDebit - ClosingCredit;
                         if (netClosingDebit > 0)
                         {

                             ClosingDebit = netClosingDebit;
                             ClosingCredit = 0;

                         }
                         else
                         {

                             ClosingCredit = netClosingDebit;
                             ClosingDebit = 0;
                         }


                     }
                     else {
                         double netClosingCredit = ClosingCredit - ClosingDebit;
                         if (netClosingCredit > 0)
                         {

                             ClosingCredit = netClosingCredit;
                             ClosingDebit = 0;

                         }
                         else
                         {

                             ClosingDebit = netClosingCredit;
                             ClosingCredit = 0;
                         }
                     
                     }
                 
                 }




                 if (ClosingDebit != 0 || ClosingCredit != 0)
                 {
                     if (logo == "")
                     {
                         dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, ClosingDebit, ClosingCredit, null, cmbbranchjv.Text);
                     }
                     else
                     {
                         dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, ClosingDebit, ClosingCredit, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                     }
                 }
            }


           
            return dtrpt;
        
        }

        public DataSet GeTrialBalanceByDate(DateTime fromDate, DateTime toDate)
        {
            // Connection string to your SQL Server database
            string connectionString = "Data Source=YourServer;Initial Catalog=YourDatabase;Integrated Security=True";
           
            connectionString= POSRestaurant.Properties.Settings.Default.ConnectionString;
        
            // Create a DataSet to hold the retrieved data
            DataSet dataSet = new DataSet();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // Open the connection
                    connection.Open();

                    // Create a SqlCommand object to execute the stored procedure
                    using (SqlCommand command = new SqlCommand("usp_GetTrialBalance", connection))
                    {
                        // Specify that the command is a stored procedure
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        command.Parameters.AddWithValue("@FromDate", fromDate);
                        command.Parameters.AddWithValue("@ToDate", toDate);

                        // Create a SqlDataAdapter to fill the DataSet with the results of the stored procedure
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            // Fill the DataSet
                            adapter.Fill(dataSet);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
                Console.WriteLine("Error: " + ex.Message);
            }

            return dataSet;
        }
        public DataTable getAllOrders()
        {
            dtrpt = new DataTable();
            dtrpt.Columns.Add("Head", typeof(string));
            dtrpt.Columns.Add("SubHead", typeof(string));
            dtrpt.Columns.Add("SubAccount", typeof(string));
            dtrpt.Columns.Add("Title", typeof(string));
            dtrpt.Columns.Add("OpeningDebit", typeof(double));
            dtrpt.Columns.Add("OpeningCredit", typeof(double));
            dtrpt.Columns.Add("Debit", typeof(double));
            dtrpt.Columns.Add("Credit", typeof(double));
            dtrpt.Columns.Add("ClosingDebit", typeof(double));
            dtrpt.Columns.Add("ClosingCredit", typeof(double));
            dtrpt.Columns.Add("logo", typeof(byte[]));
            dtrpt.Columns.Add("branch", typeof(string));
            ablnce = 0;
            aopeningblnce = 0; aopeningdebit = 0; aopeningcredit = 0;
            adebit = 0;
            acredit = 0;

            DataSet dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet("SELECT     *  FROM         ChartofAccounts order by AccountType");
           // dsaccounts = objCore.funGetDataSet("SELECT     *  FROM         ChartofAccounts where id=60 order by AccountType");

            for (int i = 0; i < dsaccounts.Tables[0].Rows.Count; i++)
            {
                //string brid=cmbbranchjv.SelectedValue.ToString();
                //Thread workerOne = new Thread(() => getAllOrders1(dsaccounts.Tables[0].Rows[i][0].ToString(),brid ));
                //workerOne.Start();

                getAllOrders1(dsaccounts.Tables[0].Rows[i]["id"].ToString(), cmbbranchjv.SelectedValue.ToString(), dsaccounts.Tables[0].Rows[i]["AccountCode"].ToString(), dsaccounts.Tables[0].Rows[i]["Name"].ToString(), dsaccounts.Tables[0].Rows[i]["AccountType"].ToString(), dsaccounts.Tables[0].Rows[i]["SubAccount"].ToString(), Convert.ToBoolean(dsaccounts.Tables[0].Rows[i]["IsDebit"]));
            }
            getcompany();
            string company = "", phone = "", address = "", logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
            }
            catch (Exception ex)
            {
            }
            double acprft = getprofitlossbf();
            if (acprft < 0)
            {
                if (logo == "")
                {
                    dtrpt.Rows.Add("","","", "Accumulate Profit/Loss", 0, 0, 0, 0, acprft, 0, null, cmbbranchjv.Text);
                }
                else
                {
                    dtrpt.Rows.Add("", "", "", "Accumulate Profit/Loss", 0, 0, 0, 0, acprft, 0, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                }
            }
            else
            {
                if (logo == "")
                {
                    dtrpt.Rows.Add("", "", "", "Accumulate Profit/Loss", 0, 0, 0, 0, 0, acprft, null, cmbbranchjv.Text);
                }
                else
                {
                    dtrpt.Rows.Add("", "", "", "Accumulate Profit/Loss", 0, 0, 0, 0, 0, acprft, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                }
            }
            return dtrpt;
        }
        string mtitle = "";
        string mcode = "";
        public void getAllOrders1(string id,string brid,string cd, string titl,string accounttype,string subaccount, bool IsDebit)
        {           
            try
            {
                string head = "";
                if (accounttype.ToLower().Contains("assets"))
                {
                    head = "Assets";
                }
                if (accounttype.ToLower().Contains("liabilities"))
                {
                    head = "Liabilities";
                }
                if (accounttype.ToLower().Contains("equity"))
                {
                    accounttype = "";
                    head = "Equity and Capital";
                }
                if (accounttype.ToLower().Contains("expenses"))
                {
                    head = "Expenses";
                }
                if (accounttype.ToLower().Contains("revenue"))
                {
                    accounttype = "";
                    head = "Revenue";
                }
                if (accounttype.ToLower().Contains("cost of sales"))
                {
                    accounttype = "";
                    head = "Cost of Sales";
                }         
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
                string val = "0";
                mtitle = "";
               //double bf= getBF(id,brid);
                double bf = 0;

                title = mtitle;
                code = mcode;
                min = dateTimePicker1.Text;
                //DateTime From = Convert.ToDateTime(dateTimePicker1.Text);
                //DateTime To = Convert.ToDateTime(dateTimePicker2.Text);

                DateTime From = Convert.ToDateTime("2023-07-01");
                DateTime To = Convert.ToDateTime(dateTimePicker1.Text).AddDays(-1);
                if (bf > 0)
                {
                    debit = bf;
                }
                if (bf < 0)
                {
                    bf = Math.Abs(bf);
                    credit = bf;
                }
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + min + "' and '" + dateTimePicker2.Text + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + min + "' and '" + dateTimePicker2.Text + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "' and dbo.BankAccountPaymentSupplier.branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {                                                
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {
                    
                    
                }

                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + From  + "' and '" + To + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + From + "' and '" + To + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "' and dbo.BankAccountPaymentSupplier.branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                   DataSet ds1 = new DataSet();
                   if (cmbbranchjv.Text == "All")
                   { q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                   }
                   else
                   {
                       q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + min + "' and '" + dateTimePicker2.Text + "') and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
                   }
                    ds1 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {                       
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);

                        //blnce = blnce + (debit - credit);

                        //title = ds1.Tables[0].Rows[i]["title"].ToString();
                        //code = ds1.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    DataSet ds1 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + From + "' and '" + To + "') and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
                    }
                    ds1 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit  = openingcredit + Convert.ToDouble(val);

                        //blnce = blnce + (debit - credit);

                        //title = ds1.Tables[0].Rows[i]["title"].ToString();
                        //code = ds1.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    DataSet ds1 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + min + "' and '" + dateTimePicker2.Text + "') and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
                    }
                    ds1 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);

                        //blnce = blnce + (debit - credit);

                        //title = ds1.Tables[0].Rows[i]["title"].ToString();
                        //code = ds1.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                   DataSet  ds2 = new DataSet();
                   if (cmbbranchjv.Text == "All")
                   {
                       q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                   }
                   else
                   {
                       q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                   }
                    ds2 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {
                       
                        val = "0";
                        //val = ds2.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        blnce = blnce + Convert.ToDouble(val);
                        //title = ds2.Tables[0].Rows[i]["title"].ToString();
                        //code = ds2.Tables[0].Rows[i]["AccountCode"].ToString();
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                       
                    }
                }
                catch (Exception ex)
                {


                }


                try
                {
                    DataSet ds2 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds2 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds2.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        //title = ds2.Tables[0].Rows[i]["title"].ToString();
                        //code = ds2.Tables[0].Rows[i]["AccountCode"].ToString();
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);

                    }
                }
                catch (Exception ex)
                {


                }

                try
                {

                   DataSet ds3 = new DataSet();
                   if (cmbbranchjv.Text == "All")
                   {
                       q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                   }
                   else
                   {
                       q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                   }
                    
                    ds3 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {                        
                        val = "0";
                        //val = ds3.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        //title = ds3.Tables[0].Rows[i]["title"].ToString();
                        //code = ds3.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }

                }
                catch (Exception ex)
                {


                }

                try
                {

                    DataSet ds3 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }

                    ds3 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        //val = ds3.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds3.Tables[0].Rows[i]["title"].ToString();
                        //code = ds3.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                       
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                       
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                      
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                       
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                       // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                       
                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '"  + From  + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + min + "' and '" + dateTimePicker2.Text + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                    }


                }
                catch (Exception ex)
                {


                }


                if (debit == 0 && credit==0)
                {
                    
                }
                else
                {
                    openingblnce = openingdebit - openingcredit;
                    //blnce = openingblnce + (debit - credit);
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
                    //openingblnce = 5000;
                    string logo = "";
                    try
                    {
                        logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                    }
                    catch (Exception ex)
                    {

                    }

                    


                    if (blnce >= 0)
                    {
                        if (openingblnce > 0)
                        {
                            openingdebit = openingblnce;
                            debit -= openingdebit;
                            blnce -= openingdebit;
                        }
                        if (openingblnce < 0)
                        {
                            openingcredit = Math.Abs(openingblnce);
                            credit -= openingcredit;
                            blnce -= openingcredit;
                        }

                        if (debit < 0)
                        {
                            debit = Math.Abs(debit);
                            credit += debit;
                            debit = 0;
                        
                        }
                        double OB = GetOpeningBalance(id, brid,cd, titl, accounttype,subaccount);

                        if (OB > 0)
                        {
                            openingdebit = OB;
                            openingcredit = 0;
                        }
                        else
                        {
                            openingcredit = OB;
                            openingdebit = 0;

                        }
                        if (IsDebit)
                        {
                            if (credit > 0)
                            {
                                debit = credit;
                                credit = 0;
                                if (openingcredit > 0)
                                {
                                    openingdebit = openingcredit;
                                    openingcredit = 0;

                                }

                            }


                        }
                        else
                        {
                            if (debit > 0)
                            {
                                credit = debit;
                                debit = 0;
                                if (openingdebit > 0)
                                {
                                    openingcredit = openingdebit;
                                    openingdebit = 0;

                                }
                            }


                        }
                        //if (openingblnce >= 0)
                        {
                            
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, blnce, 0, null, cmbbranchjv.Text);
                            }
                            else
                            {
                                dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, blnce, 0, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                            }
                        }
                        //else
                        //{
                        //    openingblnce = Math.Abs(openingblnce);
                        //    if (logo == "")
                        //    {
                        //        dtrpt.Rows.Add(code, title, 0, openingblnce, debit, credit, blnce, 0, null);
                        //    }
                        //    else
                        //    {
                        //        dtrpt.Rows.Add(code, title, 0, openingblnce, debit, credit, blnce, 0, dscompany.Tables[0].Rows[0]["logo"]);
                        //    }
                        //}
                    }
                    else
                    {
                        blnce = Math.Abs(blnce);
                        if (openingblnce > 0)
                        {
                            openingdebit = openingblnce;
                            openingblnce = openingdebit;
                            debit -= openingdebit;
                        }
                        if (openingblnce < 0)
                        {
                           openingcredit = Math.Abs(openingblnce);
                           openingblnce = openingcredit;
                           credit -= openingcredit;
                        }
                       
                        double OB = GetOpeningBalance(id, brid, cd, titl, accounttype, subaccount);

                       

                        //if (OB > 0)
                        //{
                        //    openingdebit= OB;
                        //    openingcredit = 0;
                        //}
                        //else
                        //{
                         //   openingcredit= OB;
                         //   openingdebit = 0;


                        //}


                        if (IsDebit)
                        {
                            openingdebit = Math.Abs(OB);
                            openingcredit = 0;
                        }
                        else
                        {
                            openingcredit = Math.Abs(OB); ;
                            openingdebit = 0;


                        }



                        if (IsDebit)
                        {
                            if (credit > 0)
                            {
                                debit = credit;
                                credit = 0;
                                if (openingcredit > 0)
                                {
                                    openingdebit = openingcredit;
                                    openingcredit = 0;

                                }

                            }
                           
                          
                        }
                        else {
                            if (debit > 0)
                            {
                                credit = debit;
                                debit = 0;
                                if (openingdebit > 0)
                                {
                                    openingcredit = openingdebit;
                                    openingdebit = 0;

                                }
                            }
                            else {

                                credit = Math.Abs(credit);
                                debit = 0;
                                openingcredit = Math.Abs(openingcredit);
                            
                            
                            }
                        
                        
                        }
                        //if (openingblnce >= 0)
                        {
                           // openingblnce = Math.Abs(openingblnce);
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, 0, blnce, null, cmbbranchjv.Text);
                            }
                            else
                            {
                                dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, 0, blnce, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                            }
                        }
                        
                    }                                                        
                    
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }

        public double GetOpeningBalance(string id, string brid, string cd, string titl, string accounttype, string subaccount)
        {
            double OpeningBal = 0;
            try
            {
                string head = "";
                if (accounttype.ToLower().Contains("assets"))
                {
                    head = "Assets";
                }
                if (accounttype.ToLower().Contains("liabilities"))
                {
                    head = "Liabilities";
                }
                if (accounttype.ToLower().Contains("equity"))
                {
                    accounttype = "";
                    head = "Equity and Capital";
                }
                if (accounttype.ToLower().Contains("expenses"))
                {
                    head = "Expenses";
                }
                if (accounttype.ToLower().Contains("revenue"))
                {
                    accounttype = "";
                    head = "Revenue";
                }
                if (accounttype.ToLower().Contains("cost of sales"))
                {
                    accounttype = "";
                    head = "Cost of Sales";
                }
                double blnce = 0;
                double openingblnce = 0, openingdebit = 0, openingcredit = 0;
                double debit = 0;
                double credit = 0;
                DataSet ds = new DataSet();
                DataSet dsopening = new DataSet();
                string q = "";
                ds = new DataSet();
                string title = "";
                string code = "";
                string val = "0";
                mtitle = "";
                //double bf= getBF(id,brid);
                double bf = 0;

                title = mtitle;
                code = mcode;
                min = Convert.ToDateTime(dateTimePicker1.Text).AddMonths(-1).ToString();
                //DateTime From = Convert.ToDateTime(dateTimePicker1.Text);
                //DateTime To = Convert.ToDateTime(dateTimePicker2.Text);
               
                DateTime From = Convert.ToDateTime("2023-07-01");
              // DateTime To = Convert.ToDateTime(dateTimePicker1.Text).AddDays(-2);
                DateTime FromPeriod = Convert.ToDateTime(dateTimePicker1.Text).AddMonths(-1);
                DateTime ToPeriod = Convert.ToDateTime(dateTimePicker1.Text).AddDays(-1);
                DateTime To = FromPeriod.AddDays(-1);


                if (bf > 0)
                {
                    debit = bf;
                }
                if (bf < 0)
                {
                    bf = Math.Abs(bf);
                    credit = bf;
                }
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + FromPeriod + "' and '" + ToPeriod + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + FromPeriod + "' and '" + ToPeriod + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "' and dbo.BankAccountPaymentSupplier.branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + From + "' and '" + To + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "'";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountPaymentSupplier.Debit) as Debit, sum(dbo.BankAccountPaymentSupplier.Credit) as Credit, sum(dbo.BankAccountPaymentSupplier.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountPaymentSupplier where (dbo.BankAccountPaymentSupplier.Date between '" + From + "' and '" + To + "') and dbo.BankAccountPaymentSupplier.ChartAccountId='" + id + "' and dbo.BankAccountPaymentSupplier.branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    DataSet ds1 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + FromPeriod + "' and '" + ToPeriod + "') and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
                    }
                    ds1 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);

                        //blnce = blnce + (debit - credit);

                        //title = ds1.Tables[0].Rows[i]["title"].ToString();
                        //code = ds1.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    DataSet ds1 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + From + "' and '" + To + "') and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
                    }
                    ds1 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);

                        //blnce = blnce + (debit - credit);

                        //title = ds1.Tables[0].Rows[i]["title"].ToString();
                        //code = ds1.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    DataSet ds1 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(dbo.BankAccountReceiptCustomer.Debit) as Debit, sum(dbo.BankAccountReceiptCustomer.Credit) as Credit, sum(dbo.BankAccountReceiptCustomer.CurrentBalance) As CurrentBalance FROM         dbo.BankAccountReceiptCustomer where dbo.BankAccountReceiptCustomer.ChartAccountId='" + id + "' and (dbo.BankAccountReceiptCustomer.Date between '" + FromPeriod + "' and '" + ToPeriod + "') and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
                    }
                    ds1 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds1.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);

                        //blnce = blnce + (debit - credit);

                        //title = ds1.Tables[0].Rows[i]["title"].ToString();
                        //code = ds1.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    DataSet ds2 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds2 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds2.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        blnce = blnce + Convert.ToDouble(val);
                        //title = ds2.Tables[0].Rows[i]["title"].ToString();
                        //code = ds2.Tables[0].Rows[i]["AccountCode"].ToString();
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);

                    }
                }
                catch (Exception ex)
                {


                }


                try
                {
                    DataSet ds2 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds2 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds2.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        //title = ds2.Tables[0].Rows[i]["title"].ToString();
                        //code = ds2.Tables[0].Rows[i]["AccountCode"].ToString();
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds2.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);

                    }
                }
                catch (Exception ex)
                {


                }

                try
                {

                    DataSet ds3 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }

                    ds3 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        //val = ds3.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        blnce = blnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = debit + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = credit + Convert.ToDouble(val);
                        //title = ds3.Tables[0].Rows[i]["title"].ToString();
                        //code = ds3.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
                    }

                }
                catch (Exception ex)
                {


                }

                try
                {

                    DataSet ds3 = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountPurchase where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }

                    ds3 = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds3.Tables[0].Rows.Count; i++)
                    {
                        val = "0";
                        //val = ds3.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds3.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds3.Tables[0].Rows[i]["title"].ToString();
                        //code = ds3.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CashAccountSales where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CostSalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         CustomerAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         BranchAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod +"') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         DiscountAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         GSTAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         InventoryAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         JournalAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "')";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SalesAccount where ChartAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                        // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["title"].ToString(), openingblnce, ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce);
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         SupplierAccount where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + FromPeriod + "' and '" + ToPeriod + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
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
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
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
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') ";
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Debit, sum(Credit) as Credit  FROM         PettyCash where PayableAccountId='" + id + "' and (Date between '" + From + "' and '" + To + "') and branchid='" + brid + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        val = "0";
                        //val = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        //if (val == "")
                        //{
                        //    val = "0";
                        //}
                        openingblnce = openingblnce + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingdebit = openingdebit + Convert.ToDouble(val);
                        val = "0";
                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        openingcredit = openingcredit + Convert.ToDouble(val);
                        //title = ds.Tables[0].Rows[i]["title"].ToString();
                        //code = ds.Tables[0].Rows[i]["AccountCode"].ToString();
                    }


                }
                catch (Exception ex)
                {


                }


                if (debit == 0 && credit == 0)
                {

                }
                else
                {
                    openingblnce = openingdebit - openingcredit;
                    //blnce = openingblnce + (debit - credit);
                    blnce = openingblnce + (debit - credit);


                    string blnco = "";
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
                    //openingblnce = 5000;
                    string logo = "";
                    try
                    {
                        logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                    }
                    catch (Exception ex)
                    {

                    }




                    if (blnce >= 0)
                    {
                        if (openingblnce > 0)
                        {
                            openingdebit = openingblnce;
                            debit -= openingdebit;
                            blnce -= openingdebit;
                        }
                        if (openingblnce < 0)
                        {
                            openingcredit = Math.Abs(openingblnce);
                            credit -= openingcredit;
                            blnce -= openingcredit;
                        }

                        if (debit < 0)
                        {
                            debit = Math.Abs(debit);
                            credit += debit;
                            debit = 0;

                        }

                        OpeningBal = blnce;

                        //if (openingblnce >= 0)
                       // {

                         //   if (logo == "")
                        //    {
                        //        dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingblnce, 0, debit, credit, blnce, 0, null, cmbbranchjv.Text);
                        //    }
                        //    else
                        //    {
                         //       dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingblnce, 0, debit, credit, blnce, 0, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                         //   }
                       // }
                        //else
                        //{
                        //    openingblnce = Math.Abs(openingblnce);
                        //    if (logo == "")
                        //    {
                        //        dtrpt.Rows.Add(code, title, 0, openingblnce, debit, credit, blnce, 0, null);
                        //    }
                        //    else
                        //    {
                        //        dtrpt.Rows.Add(code, title, 0, openingblnce, debit, credit, blnce, 0, dscompany.Tables[0].Rows[0]["logo"]);
                        //    }
                        //}
                    }
                    else
                    {
                        blnce = Math.Abs(blnce);
                        if (openingblnce > 0)
                        {
                            openingdebit = openingblnce;
                            openingblnce = openingdebit;
                            debit -= openingdebit;
                        }
                        if (openingblnce < 0)
                        {
                            openingcredit = Math.Abs(openingblnce);
                            openingblnce = openingcredit;
                            credit -= openingcredit;
                        }
                       
                        OpeningBal = blnce;


                        //if (openingblnce >= 0)
                       // {
                            // openingblnce = Math.Abs(openingblnce);
                       //     if (logo == "")
                       //     {
                       //         dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, 0, blnce, null, cmbbranchjv.Text);
                       //     }
                       //     else
                       //     {
                       //         dtrpt.Rows.Add(head, accounttype, subaccount, titl, openingdebit, openingcredit, debit, credit, 0, blnce, dscompany.Tables[0].Rows[0]["logo"], cmbbranchjv.Text);
                        //    }
                       // }

                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return OpeningBal;

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
            System.GC.Collect();
            button1.Text = "Please Wait(Downloading Data)";
            button1.Enabled = false;
            bindreport();
            button1.Text = "Submit";
            button1.Enabled = true;
        }
       // double bf = 0;
        public double getprofitlossbf()
        {
            double netprofitbf = 0;
            min = dateTimePicker1.Text;
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
                        q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date <  '" + min + "') ";
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
                   
                    {
                        q = "select id from ChartofAccounts where AccountType='Operating Expenses' ";
                    }
                    
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
                    
                    {
                        q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses'";
                    }
                    
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "')";
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
                   
                    {
                        q = "select id from ChartofAccounts where AccountType='Financial Expenses' ";
                    }
                   
                    
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "')";
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
                   
                    {
                        q = "select id from ChartofAccounts where AccountType='Marketing Expenses'";
                    }
                    
                    
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "')";
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "')";
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

                try
                {
                   
                    {
                        q = "select id from ChartofAccounts where AccountType='Operating Expenses' ";
                    }
                    
                    
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
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
                    
                    {
                        q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses'";
                    }
                    
                    
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') ";
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
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

                }
                catch (Exception ex)
                {


                }
                try
                {
                   
                    {
                        q = "select id from ChartofAccounts where AccountType='Financial Expenses'";
                    }
                    
                    
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
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
                   
                    {
                        q = "select id from ChartofAccounts where AccountType='Marketing Expenses'";
                    }
                    
                    
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "')";
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date <  '" + min + "') and branchid='" + cmbbranchjv.SelectedValue + "'"; 
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
            return netprofitbf;
        }
        public double getBF( string account ,string brid)
        {
            min = dateTimePicker1.Text;
            double bf = 0;
            string q = "";
            account = "12";
            q = "select  AccountType,name,AccountCode from ChartofAccounts where id='" + account + "'";
            DataSet ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string type = ds.Tables[0].Rows[0][0].ToString();
                mtitle = ds.Tables[0].Rows[0][1].ToString();
                mcode = ds.Tables[0].Rows[0][2].ToString();
                if (mtitle == "Inventory Stock")
                {

                }
                if (type == "Cost of Sales" || type == "Revenue" || type == "Operating Expenses" || type == "Admin and Selling Expenses" || type == "Financial Expenses" || type == "Marketing Expenses")
                {
                    return bf;
                }
                //if ( type == "Operating Expenses" || type == "Admin and Selling Expenses" || type == "Financial Expenses" || type == "Marketing Expenses")
                //{
                //    return bf;
                //}
            }
            try
            {
                string blnce = "", val = "0";
                double balancebf = 0, debitbf = 0, creditbf = 0;
                ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountPaymentSupplier where ChartAccountId='" + account + "'  and date <'" + min + "'  ";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountPaymentSupplier where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'  ";
                }
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountReceiptCustomer where ChartAccountId='" + account + "'  and  date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountReceiptCustomer where ChartAccountId='" + account + "' and branchid='" + brid + "' and  date <'" + min + "'";
                }
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPaymentSupplier where ChartAccountId='" + account + "'  and  date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPaymentSupplier where ChartAccountId='" + account + "' and branchid='" + brid + "' and  date <'" + min + "'";
                }
                ds = new DataSet();
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPurchase where ChartAccountId='" + account + "'   and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPurchase where ChartAccountId='" + account + "' and branchid='" + brid + "'  and date <'" + min + "'";
                }
                ds = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountReceiptCustomer where ChartAccountId='" + account + "'  and  date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountReceiptCustomer where ChartAccountId='" + account + "' and branchid='" + brid + "' and  date <'" + min + "'";
                }
                ds = new DataSet();
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountSales where ChartAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountSales where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CostSalesAccount where ChartAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CostSalesAccount where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CustomerAccount where PayableAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CustomerAccount where PayableAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BranchAccount where PayableAccountId='" + account + "' and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BranchAccount where PayableAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         DiscountAccount where ChartAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         DiscountAccount where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         GSTAccount where ChartAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         GSTAccount where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         InventoryAccount where ChartAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         InventoryAccount where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where PayableAccountId='" + account + "'  and date <'" + min + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where PayableAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                }
                ds = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                if (cmbbranchjv.Text == "All")
                {
                }
                else
                {
                }
                ds = new DataSet();
                // q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + account + "' and branchid='" + brid + "' and date <'" + dateTimePicker1.Text + "'";
                //q = "SELECT     Id, Date, PayableAccountId, VoucherNo, Description, Debit, Credit, Balance, branchid FROM         PettyCash where PayableAccountId='" + account + "' and branchid='" + brid + "' and date <'" + min + "'";
                //ds = objCore.funGetDataSet(q);
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                //    if (val == "")
                //    {
                //        val = "0";
                //    }
                //    debitbf = debitbf + Convert.ToDouble(val);

                //    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                //    if (val == "")
                //    {
                //        val = "0";
                //    }
                //    creditbf = creditbf + Convert.ToDouble(val);
                //}
                bf = debitbf - creditbf;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return bf;
        }
    }
}
