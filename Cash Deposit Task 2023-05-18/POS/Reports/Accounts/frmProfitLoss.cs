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
    public partial class frmProfitLoss : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmProfitLoss()
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
            //try
            //{
            //    dateTimePicker2.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
            //    dateTimePicker2.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
            //}
            //catch (Exception ex)
            //{


            //}
           // bindreport();
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

                //POSRestaurant.Reports.Accounts.rptProfitLoss rptDoc = new rptProfitLoss();
                POSRestaurant.Reports.Accounts.rptProfitlossfinal rptDoc = new rptProfitlossfinal();
                POSRestaurant.Reports.Accounts.dsprofitloss dsrpt = new dsprofitloss();
                //feereport ds = new feereport(); // .xsd file name
                getcompany();
                string company = "", phone = "", address = "", logo = "";
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
                dt = getAllOrdersfinal();// getAllOrders();


                dsrpt.Tables[1].Merge(dt);

                rptDoc.SetDataSource(dsrpt);

                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs",address );
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("fc", foodcostperc);
                rptDoc.SetParameterValue("pc", foodcostperc);
                //rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
                rptDoc.SetParameterValue("of","of "+ cmbbranchjv.Text+" Branch(s)");
                rptDoc.SetParameterValue("date", "from " + Convert.ToDateTime(dateTimePicker1.Text).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(dateTimePicker2.Text).ToString("dd-MM-yyyy"));
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double getprice(string start, string end, string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + start + "','" + end + "'," + id + ")";
            try
            {
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {
            }

            return cost;
        }
        double foodcostperc = 0, papercostperc = 0;
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Sales", typeof(double));
                dtrpt.Columns.Add("Discount", typeof(double));
                dtrpt.Columns.Add("netsales", typeof(double));
                dtrpt.Columns.Add("costofsale", typeof(double));
                dtrpt.Columns.Add("grossprofit", typeof(double));
                dtrpt.Columns.Add("Operatingexpense", typeof(double));
                dtrpt.Columns.Add("AdminSellingExpense", typeof(double));
                dtrpt.Columns.Add("FinancialExpense", typeof(double));
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("Marketing Expenses", typeof(double));

              
                double sales = 0,discount=0,netsales=0,costofsales=0,grossprofit=0,adminexp=0,operexp=0,financexp=0,marketingexp=0;                
                string val = "0";              
                DataSet ds = new DataSet();
                string q = "";
                ds = new DataSet();
                try
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT     sum(Credit) as Sales,sum(debit) as Salesret FROM         SalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  ";
                  
                    }
                    else
                    {
                        q = "SELECT     sum(Credit) as Sales,sum(debit) as Salesret FROM         SalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranchjv.SelectedValue + "'";
                  
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
                        q = "SELECT     sum(Debit) as Discount,sum(credit) as Discountret FROM         DiscountAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                  
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as Discount,sum(credit) as Discountret FROM         DiscountAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                  
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
                        q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                  
                    }
                    else
                    {
                        q = "SELECT     sum(Debit) as costsales,sum(credit) as costsalesrest FROM         CostSalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                  
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                       
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                       
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                       
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpret FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                       
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
                    if (cmbbranchjv.Text == "All")
                    {
                    }
                    else
                    {

                    }
                    q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
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
                    if (cmbbranchjv.Text == "All")
                    {
                    }
                    else
                    {

                    }
                    q = "select id from ChartofAccounts where AccountType='Admin and Selling Expenses'";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
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
                    if (cmbbranchjv.Text == "All")
                    {
                    }
                    else
                    {

                    }
                    q = "select id from ChartofAccounts where AccountType='Financial Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                     
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                     
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
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
                    if (cmbbranchjv.Text == "All")
                    {
                    }
                    else
                    {

                    }
                    q = "select id from ChartofAccounts where AccountType='Financial Expenses' ";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
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
                    
                    
                        q = "select id from ChartofAccounts where AccountType='Marketing Expenses' and branchid='" + cmbbranchjv.SelectedValue + "'";
                   
                    
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
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
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                      
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         SupplierAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
                      
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
                    if (cmbbranchjv.Text == "All")
                    {
                    }
                    else
                    {

                    }
                    q = "select id from ChartofAccounts where AccountType='Marketing Expenses' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                        }
                        else
                        {
                            q = "SELECT     sum(Debit) as opexp,sum(credit) as opexpres FROM         PettyCash where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranchjv.SelectedValue + "'";
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
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {

                }
                if (logo == "")
                {
                    dtrpt.Rows.Add(Math.Round(sales, 2), Math.Round(discount, 2), Math.Round(netsales, 2), Math.Round(costofsales, 2), Math.Round(grossprofit, 2), Math.Round(operexp, 2), Math.Round(adminexp, 2), Math.Round(financexp, 2), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null, Math.Round(marketingexp, 2));
                }
                else
                {
                    dtrpt.Rows.Add(Math.Round(sales, 2), Math.Round(discount, 2), Math.Round(netsales, 2), Math.Round(costofsales, 2), Math.Round(grossprofit, 2), Math.Round(operexp, 2), Math.Round(adminexp, 2), Math.Round(financexp, 2), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"], Math.Round(marketingexp, 2));
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        public DataTable getAllOrdersfinal()
        {
            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("Title", typeof(string));
            dtrpt.Columns.Add("SubTitle", typeof(string));
            dtrpt.Columns.Add("Name", typeof(string));
            dtrpt.Columns.Add("Value", typeof(double));
            dtrpt.Columns.Add("logo", typeof(Byte[]));
            DataSet dsaccounts = new System.Data.DataSet();
            string qr = "";
            double grossprofit = 0;
            getcompany();
            foodcostperc = 0; papercostperc = 0;
            double gst = 0;
            qr = "select id,name from ChartofAccounts where status='active'  and AccountType = 'Current Liabilities' and name like '%GST%' order by name";
            dsaccounts = objCore.funGetDataSet(qr);
            for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
            {
                string acid = dsaccounts.Tables[0].Rows[k][0].ToString();
                string name = dsaccounts.Tables[0].Rows[k][1].ToString();
                double balance = getvalue(acid) * -1;

                if (balance != 0)
                {
                    //dtrpt.Rows.Add("2. Taxes", "", name, balance, dscompany.Tables[0].Rows[0]["logo"]);
                   // grossprofit = grossprofit - balance;
                    gst = gst + balance;
                }
            }


            qr = "select id,name from ChartofAccounts where status='active'  and AccountType = 'Revenue' order by name";
            dsaccounts = objCore.funGetDataSet(qr);
            for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
            {
                string acid = dsaccounts.Tables[0].Rows[k][0].ToString();
                string name = dsaccounts.Tables[0].Rows[k][1].ToString();
                double balance = getvalue(acid) * -1;

                if (balance != 0)
                {
                    dtrpt.Rows.Add("1. Income","", name, balance, dscompany.Tables[0].Rows[0]["logo"]);
                    grossprofit = grossprofit + balance;
                }
            }
            if (gst > 0)
            {
                dtrpt.Rows.Add("1. Income", "", "Taxes", gst, dscompany.Tables[0].Rows[0]["logo"]);
            }
            double packingcost = 0, foodcost = 0;
            qr = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE     (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='"+cmbbranchjv.SelectedValue+"' GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
            dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet(qr);
            for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
            {
                try
                {
                    double rate = 1;
                    string q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsaccounts.Tables[0].Rows[k]["rid"].ToString() + "'";
                    DataSet dscon1 = new System.Data.DataSet();
                    dscon1 = objCore.funGetDataSet(q);
                    if (dscon1.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon1.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    if (rate > 0)
                    {

                    }

                    double price = getprice(dateTimePicker1.Text, dateTimePicker2.Text, dsaccounts.Tables[0].Rows[k]["rid"].ToString());
                    string val = dsaccounts.Tables[0].Rows[k]["QuantityConsumed"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double consumed = Convert.ToDouble(val);
                    if (rate > 0)
                    {
                        consumed = consumed / rate;
                    }
                    if (dsaccounts.Tables[0].Rows[k]["TypeName"].ToString() == "Packing")
                    {
                        packingcost = packingcost + (consumed * price);
                    }
                    else
                    {
                        foodcost = foodcost + (consumed * price);
                    }
                }
                catch (Exception ex)
                {
                    
                }

            }
            try
            {
                foodcostperc = Math.Round((foodcost / grossprofit * 100), 2);
                papercostperc = Math.Round((packingcost / grossprofit * 100), 2);
            }
            catch (Exception ex)
            {
                
              
            }
            dtrpt.Rows.Add("2. Cost of Sales", "", "Packing Cost", packingcost, dscompany.Tables[0].Rows[0]["logo"]);
            grossprofit = grossprofit - packingcost;
            dtrpt.Rows.Add("2. Cost of Sales", "", "Food Cost", foodcost, dscompany.Tables[0].Rows[0]["logo"]);
            grossprofit = grossprofit - foodcost;


            //qr = "select id,name from ChartofAccounts where status='active'  and AccountType like '%Cost of Sales%' order by name";
            //dsaccounts = new System.Data.DataSet();
            //dsaccounts = objCore.funGetDataSet(qr);
            //for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
            //{
            //    string acid = dsaccounts.Tables[0].Rows[k][0].ToString();
            //    string name = dsaccounts.Tables[0].Rows[k][1].ToString();
            //    costtype = "packing";
            //    double balance = getvalue(acid);
            //    //if (dsaccounts.Tables[0].Rows.Count > 1)
            //    //{
            //    //    if (balance != 0)
            //    //    {
            //    //        dtrpt.Rows.Add("2. Cost of Sales", "", "Packing Cost", balance, dscompany.Tables[0].Rows[0]["logo"]);
            //    //    }
            //    //}
            //    //else
            //    {
            //        if (balance != 0)
            //        {
                       
            //            dtrpt.Rows.Add("2. Cost of Sales", "", "Packing Cost", balance, dscompany.Tables[0].Rows[0]["logo"]);
            //            grossprofit = grossprofit - balance;
            //        }
            //        balance = 0;
            //        costtype = "food";
            //        balance = getvalue(acid);
            //        if (balance != 0)
            //        {
            //            dtrpt.Rows.Add("2. Cost of Sales", "", "Food Cost", balance, dscompany.Tables[0].Rows[0]["logo"]);
            //            grossprofit = grossprofit - balance;
            //        }
            //    }
            //}

            dtrpt.Rows.Add("3. Gross Profit", "", "", grossprofit, dscompany.Tables[0].Rows[0]["logo"]);


            qr = "select id,name,AccountType from ChartofAccounts where status='active'  and AccountType like '%Expenses%' order by name";
            dsaccounts = new System.Data.DataSet();
            dsaccounts = objCore.funGetDataSet(qr);
            for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
            {
                string acid = dsaccounts.Tables[0].Rows[k][0].ToString();
                string name = dsaccounts.Tables[0].Rows[k][1].ToString();
                string type = dsaccounts.Tables[0].Rows[k][2].ToString();
                double balance = getvalue(acid);

                if (balance != 0)
                {

                    dtrpt.Rows.Add("4. Expenses", type, name, balance, dscompany.Tables[0].Rows[0]["logo"]);
                }
            }
            double profitloss=0,income=0,expese=0;
            try
            {
                income = dtrpt.AsEnumerable().Where(x => x.Field<string>("Title") == "1. Income").Sum(row => row.Field<double>("Value"));
                expese = dtrpt.AsEnumerable().Where(x => x.Field<string>("Title") == "2. Cost of Sales").Sum(row => row.Field<double>("Value"));
                expese = expese + dtrpt.AsEnumerable().Where(x => x.Field<string>("Title") == "4. Expenses").Sum(row => row.Field<double>("Value"));
            }
            catch (Exception ex)
            {

            }
            profitloss = income - expese;

            dtrpt.Rows.Add("4. Profit/Loss","", "", profitloss, dscompany.Tables[0].Rows[0]["logo"]);

            return dtrpt;
        }
        string costtype = "";
        public double getvalue(string acid)
        {

            double value = 0;
            try
            {
                
                    
                    string blnce = "", val = "0";
                    double balance = 0, debit = 0, credit = 0, totaldebit = 0, totalcredit = 0;
                    DataSet ds = new DataSet();
                    string q = "";
                 
                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {

                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";

                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }


                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        // dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (costtype == "packing")
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where Description like '%Packing Cost%' and  ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                        }
                        else
                        {
                            q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where Description like '%Packing Cost%' and   ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                        }
                    }
                    else if (costtype == "food")
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where Description like '%Food Cost%' and   ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                        }
                        else
                        {
                            q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where Description like '%Food Cost%' and   ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                        }
                    }
                    else
                    {
                        if (cmbbranchjv.Text == "All")
                        {
                            q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where   ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                        }
                        else
                        {
                            q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where   ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                        }
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);

                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                    }

                    ds.Dispose();
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                       
                       
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         BranchAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         BranchAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                       
                       
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalariesAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalariesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        

                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                      
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                       
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        
                    }

                    ds.Dispose();
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit FROM         EmployeesAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit FROM         EmployeesAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debit = Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        credit = Convert.ToDouble(val);
                        
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                       
                    }

                    ds.Dispose();
                    

                    ds = new DataSet();


                    value = totaldebit - totalcredit;

               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return value;
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

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
            string name = info.Text;
            
            //String sObjectProperties = "Name: " + info.Name + "\nText: " + info.Text + "\nObject Type: " + info.ObjectType + "\nToolTip: " + info.ToolTip + "\nDataContext: " + info.DataContext + "\nGroup Name Path: " + info.GroupNamePath + "\nHyperlink: " + info.Hyperlink;
            string group = info.GroupNamePath;///Title[4. Expenses]/SubTitle[Admin and Selling Expenses]
            
            if (group.Contains("1. Income"))
            {
                POSRestaurant.Reports.frmSales obj = new frmSales();
                obj.start = dateTimePicker1.Text;
                obj.end = dateTimePicker2.Text;
                if (cmbbranchjv.Text == "All")
                {
                    obj.branchid = "All";
                }
                else
                {
                    obj.branchid = cmbbranchjv.SelectedValue.ToString();
                }
                obj.reference = "pnl";
                obj.ShowDialog();
            }
            if (group.Contains("2. Cost of Sales"))
            {
                POSRestaurant.Reports.Inventory.frmConsumedInventory obj = new POSRestaurant.Reports.Inventory.frmConsumedInventory();
                obj.start = dateTimePicker1.Text;
                obj.end = dateTimePicker2.Text;
                if (cmbbranchjv.Text == "All")
                {
                    obj.branchid = "All";
                }
                else
                {
                    obj.branchid = cmbbranchjv.SelectedValue.ToString();
                }
                obj.reference = "pnl";
                obj.ShowDialog();
            }

            if (group.Contains("4. Expenses"))
            {
                POSRestaurant.Reports.Statements.frmGLAccounts obj = new POSRestaurant.Reports.Statements.frmGLAccounts();
                obj.start = dateTimePicker1.Text;
                obj.end = dateTimePicker2.Text;
                if (cmbbranchjv.Text == "All")
                {
                    obj.branchid = "All";
                }
                else
                {
                    obj.branchid = cmbbranchjv.SelectedValue.ToString();
                }
                obj.reference = "pnl";
                string accountid = "";
                try
                {
                    DataSet ds1 = new DataSet();
                    string q = "select id from ChartofAccounts where Name='" + name + "' and AccountType like '%Expenses%'";
                    ds1 = objCore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        accountid = ds1.Tables[0].Rows[0][0].ToString();

                    }

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }

                if (accountid.Length > 0)
                {
                    obj.accountid = accountid;
                    obj.ShowDialog();
                }
            }
           
        }

    }
}
