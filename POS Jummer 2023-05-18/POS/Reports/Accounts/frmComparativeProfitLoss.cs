using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace POSRestaurant.Reports.Accounts
{
    public partial class frmComparativeProfitLoss : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmComparativeProfitLoss()
        {
            InitializeComponent();
        }

        private void frmaccounts_Load(object sender, EventArgs e)
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
           // bindreport();
           
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


                POSRestaurant.Reports.Accounts.rptComparativeProfitLoss1 rptDoc = new rptComparativeProfitLoss1();
                POSRestaurant.Reports.Accounts.dscomparativeprofitloss dsrpt = new dscomparativeprofitloss();
                //feereport ds = new feereport(); // .xsd file name
                getcompany();
                string company = "", phone = "", address = "",logo="";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
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
                        dt.Rows.Add("","", "0",  dscompany.Tables[0].Rows[0]["logo"]);
              
                        
                        dsrpt.Tables[0].Merge(dt);
                    }
                }


                rptDoc.SetDataSource(dsrpt);

                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period of "+dateTimePicker1.Text+" to"+dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable getAllOrders()
        {
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {

            }
            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("BranchName", typeof(string));
                dtrpt.Columns.Add("Heading", typeof(string));
                dtrpt.Columns.Add("value", typeof(double));
               
                dtrpt.Columns.Add("logo", typeof(byte[]));
                double sales = 0,discount=0,netsales=0,costofsales=0,grossprofit=0,adminexp=0,operexp=0,financexp=0,marketingexp=0;
                string q = "";
                string val = "0";
                DataSet dsbranch = new DataSet();
                q = "SELECT  Id, BranchName, BranchCode, Location, UploadStatus FROM         Branch";
                dsbranch = objCore.funGetDataSet(q);
                DataSet ds = new DataSet();
               
                ds = new DataSet();
                for (int z = 0; z < dsbranch.Tables[0].Rows.Count; z++)
                {
                    sales = 0; discount = 0; netsales = 0; costofsales = 0; grossprofit = 0; adminexp = 0; operexp = 0; financexp = 0; marketingexp = 0;
                    try
                    {
                        //q = "SELECT     sum(Credit) as Sales,sum(debit) as Salesret FROM         SalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  ";
                        q = "SELECT     SUM(dbo.SalesAccount.Credit) AS Sales, SUM(dbo.SalesAccount.Debit) AS Salesret, dbo.Branch.BranchName FROM         dbo.SalesAccount INNER JOIN                      dbo.Branch ON dbo.SalesAccount.branchid = dbo.Branch.Id where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.SalesAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";
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
                        // q = "SELECT     sum(Debit) as Discount,sum(credit) as Discountret FROM         DiscountAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                        q = "SELECT     SUM(dbo.DiscountAccount.Credit) AS Discountret, SUM(dbo.DiscountAccount.Debit) AS Discount, dbo.Branch.BranchName FROM         dbo.DiscountAccount INNER JOIN                      dbo.Branch ON dbo.DiscountAccount.branchid = dbo.Branch.Id where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DiscountAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            double rest = Convert.ToDouble(ds.Tables[0].Rows[i]["Discountret"].ToString());
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
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "A Net Sales", netsales, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "A Net Sales", netsales, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    try
                    {
                        //q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                        q = "SELECT     SUM(dbo.CostSalesAccount.Credit) AS costsalesrest, SUM(dbo.CostSalesAccount.Debit) AS costsales, dbo.Branch.BranchName FROM         dbo.CostSalesAccount INNER JOIN                      dbo.Branch ON dbo.CostSalesAccount.branchid = dbo.Branch.Id where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.CostSalesAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "B Cost of Sales", costofsales, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "B Cost of Sales", costofsales, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    grossprofit = netsales - costofsales;
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "C Gross Profit", grossprofit, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "C Gross Profit", grossprofit, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    DataSet dsexp = new System.Data.DataSet();
                    try
                    {
                        q = "select id from ChartofAccounts where AccountType='Operating Expenses' and branchid='"+ dsbranch.Tables[0].Rows[z]["id"].ToString()+"'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            // q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpret, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                        q = "select id from ChartofAccounts where AccountType='Operating Expenses' and branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            // q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            //q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpret, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";
                            q = "SELECT     SUM(dbo.PettyCash.Credit) AS opexpret, SUM(dbo.PettyCash.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.Branch INNER JOIN                      dbo.PettyCash ON dbo.Branch.Id = dbo.PettyCash.branchid where dbo.PettyCash.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PettyCash.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "D Operating Expenses", operexp, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "D Operating Expenses", operexp, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    try
                    {
                        q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses' and branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            //q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpres, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                        q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses' and branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            
                            //q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpres, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";
                            q = "SELECT     SUM(dbo.PettyCash.Credit) AS opexpret, SUM(dbo.PettyCash.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.Branch INNER JOIN                      dbo.PettyCash ON dbo.Branch.Id = dbo.PettyCash.branchid where dbo.PettyCash.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PettyCash.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                    adminexp = adminexp + discount;
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "E Admin and Selling Expenses", adminexp, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "E Admin and Selling Expenses", adminexp, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                   
                    try
                    {
                        q = "select id from ChartofAccounts where AccountType='Financial Expenses' and branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            
                           // q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpres, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";
                            q = "SELECT     SUM(dbo.PettyCash.Credit) AS opexpret, SUM(dbo.PettyCash.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.Branch INNER JOIN                      dbo.PettyCash ON dbo.Branch.Id = dbo.PettyCash.branchid where dbo.PettyCash.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PettyCash.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                        q = "select id from ChartofAccounts where AccountType='Financial Expenses' and branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            // q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpres, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "F Financial Expenses", financexp, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "F Financial Expenses", financexp, dscompany.Tables[0].Rows[0]["logo"]);
                    }

                    try
                    {
                        q = "select id from ChartofAccounts where AccountType='Marketing Expenses' and branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            // q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            //q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpres, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";
                            q = "SELECT     SUM(dbo.PettyCash.Credit) AS opexpret, SUM(dbo.PettyCash.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.Branch INNER JOIN                      dbo.PettyCash ON dbo.Branch.Id = dbo.PettyCash.branchid where dbo.PettyCash.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PettyCash.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                        q = "select id from ChartofAccounts where AccountType='Marketing Expenses' and branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "'";
                        dsexp = new System.Data.DataSet();
                        dsexp = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                        {
                            // q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            q = "SELECT     SUM(dbo.JournalAccount.Credit) AS opexpres, SUM(dbo.JournalAccount.Debit) AS opexp, dbo.Branch.BranchName FROM         dbo.JournalAccount INNER JOIN                      dbo.Branch ON dbo.JournalAccount.branchid = dbo.Branch.Id where dbo.JournalAccount.PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.JournalAccount.branchid='" + dsbranch.Tables[0].Rows[z]["id"].ToString() + "' GROUP BY dbo.Branch.BranchName";

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
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "Marketing Expenses", financexp, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "Marketing Expenses", financexp, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    double total = (operexp + adminexp + financexp + marketingexp);
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "N Total Expenses", total, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "N Total Expenses", total, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    
                    
                    double net = (grossprofit -Convert.ToDouble(total) );
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "O Net Profit \\ Loss", net, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsbranch.Tables[0].Rows[z]["BranchName"].ToString(), "O Net Profit \\ Loss", net, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
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
            button1.Text = "Please Wait(Downloading Data)";
            button1.Enabled = false;
            bindreport();
            button1.Text = "Submit";
            button1.Enabled = true;
        }

    }
}
