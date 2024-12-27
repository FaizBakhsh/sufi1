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
    public partial class frmGLAccounts : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmGLAccounts()
        {
            InitializeComponent();
        }

        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet ds = new DataSet();
               string q = "select id,name from ChartofAccounts";
                ds = objCore.funGetDataSet(q);
               
                cmbaccount.DataSource = ds.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "name";
                
                
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


                POSRetail.Reports.Statements.crplegeraccounts rptDoc = new  crplegeraccounts();
                POSRetail.Reports.Statements.dsledgeraccounts dsrpt = new dsledgeraccounts();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
               
                DataTable dt2 = dt.Clone();

                dt2.Columns["Date"].DataType = Type.GetType("System.DateTime");

                foreach (DataRow dr in dt.Rows)
                {
                    dt2.ImportRow(dr);
                }
                dt2.AcceptChanges();
                DataView dv = dt2.DefaultView;
                dt2.DefaultView.Sort = "Date";
                dt2 = dt2.DefaultView.ToTable();

                
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
                    dsrpt.Tables[0].Merge(dt2, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    if (logo == "")
                    { }
                    else
                    {

                        dt.Rows.Add("", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt2, true, MissingSchemaAction.Ignore);
                    }
                }
                
                dsrpt.Tables[0].DefaultView.Sort = "Date";
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
                dtrpt.Columns.Add("account", typeof(string));
                
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }

                string blnce = "";
                DataSet ds = new DataSet();
                string q = "";
                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountPaymentSupplier where ChartAccountId='"+cmbaccount.SelectedValue+"'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {
                        //string date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString();
                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }


                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["CurrentBalance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                   // dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }

                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                }

                ds = new DataSet();
                q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + cmbaccount.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    blnce = "";
                    blnce = ds.Tables[0].Rows[i]["Balance"].ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null);
                    }
                    else
                    {


                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
                    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
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
            if (cmbaccount.Text == "")
            {
                MessageBox.Show("Please Select Supplier");
                return;
            }
           
            bindreport();
        }
    }
}
