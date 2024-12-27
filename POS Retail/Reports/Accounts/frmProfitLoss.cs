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
    public partial class frmProfitLoss : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmProfitLoss()
        {
            InitializeComponent();
        }

        private void frmaccounts_Load(object sender, EventArgs e)
        {
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


                POSRetail.Reports.Accounts.rptProfitLoss rptDoc = new rptProfitLoss();
                POSRetail.Reports.Accounts.dsprofitloss dsrpt = new dsprofitloss();
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
                        dt.Rows.Add("", "", "", "", "", "", "", "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
              
                        
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
                double sales = 0,discount=0,netsales=0,costofsales=0,grossprofit=0,adminexp=0,operexp=0,financexp=0;
                
                string val = "0";
              
                DataSet ds = new DataSet();
                string q = "";
                ds = new DataSet();
                try
                {
                    q = "SELECT     sum(Credit) as Sales FROM         SalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Sales"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        sales = Convert.ToDouble(val);

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    q = "SELECT     sum(Debit) as Discount FROM         DiscountAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Discount"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        discount = Convert.ToDouble(val);

                    }
                }
                catch (Exception ex)
                {


                }
                netsales = sales - discount;
                try
                {
                    q = "SELECT     sum(Debit) as costsales FROM         CostSalesAccount where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["costsales"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        costofsales = Convert.ToDouble(val);

                    }
                }
                catch (Exception ex)
                {


                }
                grossprofit = netsales - costofsales;
                DataSet dsexp = new System.Data.DataSet();
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Operating Expenses'";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        q = "SELECT     sum(Debit) as opexp FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            operexp = operexp + Convert.ToDouble(val);

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
                        q = "SELECT     sum(Debit) as opexp FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            adminexp = adminexp + Convert.ToDouble(val);

                        }
                    }

                }
                catch (Exception ex)
                {


                }
                try
                {
                    q = "select id from ChartofAccounts where AccountType='Financial Expenses'";
                    dsexp = new System.Data.DataSet();
                    dsexp = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsexp.Tables[0].Rows.Count; j++)
                    {
                        q = "SELECT     sum(Debit) as opexp FROM         JournalAccount where PayableAccountId='" + dsexp.Tables[0].Rows[j][0].ToString() + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            val = ds.Tables[0].Rows[i]["opexp"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            financexp = financexp + Convert.ToDouble(val);

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
                    dtrpt.Rows.Add(Math.Round(sales, 2), Math.Round(discount, 2), Math.Round(netsales, 2), Math.Round(costofsales, 2), Math.Round(grossprofit, 2), Math.Round(operexp, 2), Math.Round(adminexp, 2), Math.Round(financexp, 2), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text,null);
                }
                else
                {
                    dtrpt.Rows.Add(Math.Round(sales, 2), Math.Round(discount, 2), Math.Round(netsales, 2), Math.Round(costofsales, 2), Math.Round(grossprofit, 2), Math.Round(operexp, 2), Math.Round(adminexp, 2), Math.Round(financexp, 2), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
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
            bindreport();
        }

    }
}
