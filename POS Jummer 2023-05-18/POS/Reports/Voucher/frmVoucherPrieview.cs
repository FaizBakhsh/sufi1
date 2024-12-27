using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Voucher
{
    public partial class frmVoucherPrieview : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string id = "";
        public string name = "";
        public string branch = "";
        public string type = "";
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
        DataSet dsbranch = new DataSet();
        public void getbranch()
        {
            dsbranch = objCore.funGetDataSet("select * from Branch where id='" + branch + "'");

        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();

                getcompany();
                getbranch();
                POSRestaurant.Reports.Voucher.rprVoucherPrieview rptDoc = new rprVoucherPrieview();
                POSRestaurant.Reports.Voucher.dsVoucherPreview dsrpt = new dsVoucherPreview();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                if (type == "jv")
                {
                    dt = getAllOrdersjv(branch);
                }
                if (type == "cpv")
                {
                    dt = getAllOrderscashpayment(branch);
                }
                if (type == "bpv")
                {
                    dt = getAllOrdersbankpayment(branch);
                }
                if (type == "crv")
                {
                    dt = getAllOrderscashreceipt(branch);
                }
                if (type == "brv")
                {
                    dt = getAllOrdersbankreceipt(branch);
                }
                if (type == "sjv")
                {
                    dt = getAllOrderssjv(branch);
                }
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
                    DataView dv = dt.DefaultView;
                    dv.Sort = "debit desc";
                    DataTable sortedDT = dv.ToTable();
                    dsrpt.Tables[0].Merge(sortedDT, true, MissingSchemaAction.Ignore);
                }
                
                string br = "";
                try
                {
                     br = dsbranch.Tables[0].Rows[0]["BranchName"].ToString();
                }
                catch (Exception ex)
                {                                        
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("branch", br);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public string getsupplier(string id)
        {
            string sup="";
            try
            {
                DataSet dssuplier = new DataSet();
                string q = "select name from Supplier where id='" + id + "'";
                dssuplier = objCore.funGetDataSet(q);
                if (dssuplier.Tables[0].Rows.Count > 0)
                {
                    sup = dssuplier.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                
               
            }
            return sup;
        }
        DataSet dsusers = new DataSet();
        public void getusers(string id)
        {
            try
            {
                dsusers = objCore.funGetDataSet("select * from Users where id=" + id);
            }
            catch (Exception ex)
            {


            }

        }
        public string getemployee(string id)
        {
            string sup = "";
            try
            {
                DataSet dssuplier = new DataSet();
                string q = "select name from Employees where id='" + id + "'";
                dssuplier = objCore.funGetDataSet(q);
                if (dssuplier.Tables[0].Rows.Count > 0)
                {
                    sup = dssuplier.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return sup;
        }
        public string getcust(string id)
        {
            string cus = "";
            try
            {
                DataSet dssuplier = new DataSet();
                string q = "select name from Customers where id='" + id + "'";
                dssuplier = objCore.funGetDataSet(q);
                if (dssuplier.Tables[0].Rows.Count > 0)
                {
                    cus = dssuplier.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return cus;
        }
        public DataTable getAllOrderscashpayment(string brid)
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("desc", typeof(string));
                dtrpt.Columns.Add("sign", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "", val = "";
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
                    ds = new DataSet();
                    //q = "select top 1 CurrentBalance from CashAccountPaymentSupplier where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";

                    q = "SELECT     dbo.CashAccountPaymentSupplier.Approveduserid, dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CashAccountPaymentSupplier.Description, dbo.CashAccountPaymentSupplier.Date, dbo.CashAccountPaymentSupplier.Debit, dbo.CashAccountPaymentSupplier.Credit,dbo.CashAccountPaymentSupplier.SupplierId FROM         dbo.CashAccountPaymentSupplier INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountPaymentSupplier.Voucherno='" + id + "' and dbo.CashAccountPaymentSupplier.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        string sup = getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = " (Supplier : " + sup + ")";
                        }
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(),null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);


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
                    q = "SELECT     dbo.SupplierAccount.Approveduserid,dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.SupplierAccount.Description, dbo.SupplierAccount.SupplierId, dbo.SupplierAccount.Date, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Credit FROM         dbo.SupplierAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.SupplierAccount.Voucherno='" + id + "' and dbo.SupplierAccount.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        string sup = getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = " (Supplier : " + sup + ")";
                        }
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(),null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);


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
                    q = "SELECT    dbo.EmployeesAccount.Approveduserid, dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.EmployeesAccount.Description, dbo.EmployeesAccount.EmployeeId, dbo.EmployeesAccount.Date, dbo.EmployeesAccount.Debit, dbo.EmployeesAccount.Credit FROM         dbo.EmployeesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.EmployeesAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.EmployeesAccount.Voucherno='" + id + "' and dbo.EmployeesAccount.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        string sup = getemployee(ds.Tables[0].Rows[i]["EmployeeId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = " (Employee : " + sup+")";
                        }
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);
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


            }
            return dtrpt;
        }
        public DataTable getAllOrdersbankpayment(string brid)
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("desc", typeof(string));
                dtrpt.Columns.Add("sign", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "", val = "";
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

                    q = "SELECT    dbo.BankAccountPaymentSupplier.Approveduserid,  dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.BankAccountPaymentSupplier.Description, dbo.BankAccountPaymentSupplier.Date, dbo.BankAccountPaymentSupplier.Debit, dbo.BankAccountPaymentSupplier.Credit,dbo.BankAccountPaymentSupplier.SupplierId FROM         dbo.BankAccountPaymentSupplier INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountPaymentSupplier.ChartAccountId = dbo.ChartofAccounts.Id where dbo.BankAccountPaymentSupplier.Voucherno='" + id + "' and dbo.BankAccountPaymentSupplier.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        string sup = getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = " (Supplier : " + sup+")";
                        }
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(),null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(),  dsusers.Tables[0].Rows[0]["Signature"]);


                        }


                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    q = "SELECT     dbo.SupplierAccount.Approveduserid, dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.SupplierAccount.Description, dbo.SupplierAccount.Date,dbo.SupplierAccount.SupplierId, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Credit FROM         dbo.SupplierAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.SupplierAccount.Voucherno='" + id + "' and dbo.SupplierAccount.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        string sup = getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = " (Supplier : " + sup + ")";
                        }
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);


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
                    q = "SELECT    dbo.EmployeesAccount.Approveduserid, dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.EmployeesAccount.Description, dbo.EmployeesAccount.EmployeeId, dbo.EmployeesAccount.Date, dbo.EmployeesAccount.Debit, dbo.EmployeesAccount.Credit FROM         dbo.EmployeesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.EmployeesAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.EmployeesAccount.Voucherno='" + id + "' and dbo.EmployeesAccount.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        string sup = getemployee(ds.Tables[0].Rows[i]["EmployeeId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = " (Employee : " + sup+")";
                        }
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(),null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);
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


            }
            return dtrpt;
        }
        public DataTable getAllOrderssjv(string brid)
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("desc", typeof(string));
                dtrpt.Columns.Add("sign", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "", val = "";
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
                    ds = new DataSet();

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.SalesAccount.Description, dbo.SalesAccount.Date,  dbo.SalesAccount.Debit, dbo.SalesAccount.Credit FROM         dbo.SalesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.SalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.SalesAccount.Voucherno like '" + id + "%' and dbo.SalesAccount.branchid='" + brid + "'";
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
                        string sup = "";
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.DiscountAccount.Description, dbo.DiscountAccount.Date,  dbo.DiscountAccount.Debit, dbo.DiscountAccount.Credit FROM         dbo.DiscountAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.DiscountAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.DiscountAccount.Voucherno like '" + id + "%' and dbo.DiscountAccount.branchid='" + brid + "'";
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
                        string sup = "";
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.GSTAccount.Description, dbo.GSTAccount.Date,  dbo.GSTAccount.Debit, dbo.GSTAccount.Credit FROM         dbo.GSTAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.GSTAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.GSTAccount.Voucherno like '" + id + "%' and dbo.GSTAccount.branchid='" + brid + "'";
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
                        string sup = "";
                        
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CashAccountReceiptCustomer.Description, dbo.CashAccountReceiptCustomer.Date,  dbo.CashAccountReceiptCustomer.Debit, dbo.CashAccountReceiptCustomer.Credit FROM         dbo.CashAccountReceiptCustomer INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountReceiptCustomer.Voucherno like '" + id + "%' and dbo.CashAccountReceiptCustomer.branchid='" + brid + "'";
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
                        string sup = "";

                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CashAccountSales.Description, dbo.CashAccountSales.Date,  dbo.CashAccountSales.Debit, dbo.CashAccountSales.Credit FROM         dbo.CashAccountSales INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountSales.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountSales.Voucherno like '" + id + "%' and dbo.CashAccountSales.branchid='" + brid + "'";
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
                        string sup = "";

                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.InventoryAccount.Description, dbo.InventoryAccount.Date,  dbo.InventoryAccount.Debit, dbo.InventoryAccount.Credit FROM         dbo.InventoryAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.InventoryAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.InventoryAccount.Voucherno like '" + id + "%' and dbo.InventoryAccount.branchid='" + brid + "'";
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
                        string sup = "";

                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CostSalesAccount.Description, dbo.CostSalesAccount.Date,  dbo.CostSalesAccount.Debit, dbo.CostSalesAccount.Credit FROM         dbo.CostSalesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.CostSalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CostSalesAccount.Voucherno like '" + id + "%' and dbo.CostSalesAccount.branchid='" + brid + "'";
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
                        string sup = "";

                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CustomerAccount.Description, dbo.CustomerAccount.CustomersId,dbo.CustomerAccount.Date, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Credit FROM         dbo.CustomerAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.CustomerAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.CustomerAccount.Voucherno like '" + id + "%' and dbo.CustomerAccount.branchid='" + brid + "'";
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
                        string sup = getcust(ds.Tables[0].Rows[i]["CustomersId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = "(Customer : " + sup+")";
                        }
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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


            }
            return dtrpt;
        }
        public DataTable getAllOrdersbankreceipt(string brid)
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("desc", typeof(string));
                dtrpt.Columns.Add("sign", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "", val = "";
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
                    ds = new DataSet();

                    //q = "select top 1 CurrentBalance from BankAccountReceiptCustomer where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc ";
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.BankAccountReceiptCustomer.Description, dbo.BankAccountReceiptCustomer.Date, dbo.BankAccountReceiptCustomer.CustomerId, dbo.BankAccountReceiptCustomer.Debit, dbo.BankAccountReceiptCustomer.Credit FROM         dbo.BankAccountReceiptCustomer INNER JOIN                      dbo.ChartofAccounts ON dbo.BankAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.BankAccountReceiptCustomer.Voucherno='" + id + "' and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
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
                        string sup = getcust(ds.Tables[0].Rows[i]["CustomerId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = "Customer : " + sup;
                        }
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CustomerAccount.Description, dbo.CustomerAccount.CustomersId,dbo.CustomerAccount.Date, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Credit FROM         dbo.CustomerAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.CustomerAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.CustomerAccount.Voucherno='" + id + "' and dbo.CustomerAccount.branchid='" + brid + "'";
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
                        string sup = getcust(ds.Tables[0].Rows[i]["CustomersId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = "Customer : " + sup;
                        }
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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


            }
            return dtrpt;
        }
        public DataTable getAllOrderscashreceipt(string brid)
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("desc", typeof(string));
                dtrpt.Columns.Add("sign", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "", val = "";
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
                    ds = new DataSet();
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CashAccountReceiptCustomer.Description, dbo.CashAccountReceiptCustomer.Date,dbo.CashAccountReceiptCustomer.CustomerId, dbo.CashAccountReceiptCustomer.Debit, dbo.CashAccountReceiptCustomer.Credit FROM         dbo.CashAccountReceiptCustomer INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountReceiptCustomer.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountReceiptCustomer.Voucherno='" + id + "' and dbo.CashAccountReceiptCustomer.branchid='" + brid + "'";
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
                        string sup = getcust(ds.Tables[0].Rows[i]["CustomerId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = "Customer : " + sup;
                        }
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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
                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.CustomerAccount.Description, dbo.CustomerAccount.Date,dbo.CustomerAccount.CustomersId, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Credit FROM         dbo.CustomerAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.CustomerAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.CustomerAccount.Voucherno='" + id + "' and dbo.CustomerAccount.branchid='" + brid + "'";
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
                        string sup = getcust(ds.Tables[0].Rows[i]["CustomersId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = "(Customer : " + sup+")";
                        }
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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


            }
            return dtrpt;
        }
        public DataTable getAllOrdersjv(string brid)
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("desc", typeof(string));
                dtrpt.Columns.Add("sign", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "", val = "";
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
                    ds = new DataSet();
                    q = "SELECT      dbo.JournalAccount.Approveduserid,dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.JournalAccount.Description, dbo.JournalAccount.Date, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.Voucherno='" + id + "' and dbo.JournalAccount.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], "", ds.Tables[0].Rows[i]["Description"].ToString(),null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], "", ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);


                        }
                        
                    }
                    ds = new DataSet();

                    q = "SELECT     dbo.SupplierAccount.Approveduserid,dbo.ChartofAccounts.AccountCode, dbo.SupplierAccount.Date, dbo.SupplierAccount.Description, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Credit, dbo.SupplierAccount.SupplierId,                       dbo.Supplier.Name FROM         dbo.SupplierAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.SupplierAccount.Voucherno='" + id + "' and dbo.SupplierAccount.branchid='" + brid + "'";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                        string usersid = "";
                        try
                        {
                            usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
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
                        string sup = getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = "Supplier : " + sup;
                        }
                        if (usersid == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], "", ds.Tables[0].Rows[i]["Description"].ToString(),null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], "", ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);


                        }

                    }
                    try
                    {
                        ds = new DataSet();
                        q = "SELECT     dbo.EmployeesAccount.Approveduserid,  dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.EmployeesAccount.Description, dbo.EmployeesAccount.EmployeeId, dbo.EmployeesAccount.Date, dbo.EmployeesAccount.Debit, dbo.EmployeesAccount.Credit FROM         dbo.EmployeesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.EmployeesAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.EmployeesAccount.Voucherno='" + id + "' and dbo.EmployeesAccount.branchid='" + brid + "'";
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                            string usersid = "";
                            try
                            {
                                usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                            }
                            catch (Exception ex)
                            {

                            }
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
                            string sup = getemployee(ds.Tables[0].Rows[i]["EmployeeId"].ToString());
                            if (sup.Trim() != "")
                            {
                                sup = "Employee : " + sup;
                            }
                            if (usersid == "")
                            {

                                dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(),null);
                            }
                            else
                            {


                                dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString() + sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString(), dsusers.Tables[0].Rows[0]["Signature"]);


                            }
                            //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

                        }

                    }
                    catch (Exception ex)
                    {


                    }
                    ds = new DataSet();


                    q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.CustomerAccount.Date, dbo.CustomerAccount.Description, dbo.CustomerAccount.Debit, dbo.CustomerAccount.Credit, dbo.CustomerAccount.CustomersId,                       dbo.Customers.Name FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CustomerAccount ON dbo.ChartofAccounts.Id = dbo.CustomerAccount.PayableAccountId INNER JOIN                      dbo.Customers ON dbo.CustomerAccount.CustomersId = dbo.Customers.Id where dbo.CustomerAccount.Voucherno='" + id + "' and dbo.CustomerAccount.branchid='" + brid + "'"; ;
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
                        string sup = getcust(ds.Tables[0].Rows[i]["CustomersId"].ToString());
                        if (sup.Trim() != "")
                        {
                            sup = "(Customer : " + sup + ")";
                        }
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, null, "", ds.Tables[0].Rows[i]["Description"].ToString(),null);
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString()+sup, debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], "", ds.Tables[0].Rows[i]["Description"].ToString(),null);


                        }

                    }
                }
                catch (Exception ex)
                {


                }

            }
            catch (Exception ex)
            {


            }
            return dtrpt;
        }
        //public DataTable getAllOrders(string brid)
        //{

        //    DataTable dtrpt = new DataTable();
        //    try
        //    {
        //        dtrpt.Columns.Add("date", typeof(string));
        //        dtrpt.Columns.Add("accountcode", typeof(string));
        //        dtrpt.Columns.Add("accountname", typeof(string));
        //        dtrpt.Columns.Add("debit", typeof(double));
        //        dtrpt.Columns.Add("credit", typeof(double));
        //        dtrpt.Columns.Add("voucherno", typeof(string));
        //        dtrpt.Columns.Add("vouchername", typeof(string));
        //        dtrpt.Columns.Add("logo", typeof(Byte[]));
        //        DataSet ds = new DataSet();
        //        string q = "", val="";
        //        getcompany();
        //        string logo = "";
        //        try
        //        {
        //            logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

        //        }
        //        catch (Exception ex)
        //        {


        //        }
                

               


                
        //        try
        //        {

        //            ds = new DataSet();
        //           // q = "select top 1 Balance as CurrentBalance from CashAccountPurchase where Date <= '" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
        //            q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.CashAccountPurchase.Date, dbo.CashAccountPurchase.Debit, dbo.CashAccountPurchase.Credit FROM         dbo.CashAccountPurchase INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountPurchase.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountPurchase.Voucherno='" + id + "' and dbo.CashAccountPurchase.branchid='" + brid + "'";
        //            ds = objCore.funGetDataSet(q);
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double debit = Convert.ToDouble(val);
        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double credit = Convert.ToDouble(val);
        //                if (logo == "")
        //                {

        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
        //                }
        //                else
        //                {


        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


        //                }
        //                // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

        //            }

        //        }
        //        catch (Exception ex)
        //        {


        //        }

                
        //        try
        //        {
        //            ds = new DataSet();
        //            //q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + cmbaccount.SelectedValue + "'";
        //            // q = "SELECT     sum(dbo.CashAccountSales.Debit) as Debit, sum(dbo.CashAccountSales.Credit) as Credit, sum(dbo.CashAccountSales.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CashAccountSales RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountSales.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CashAccountSales.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.ChartofAccounts.AccountCode";

        //            //q = "select top 1 Balance as CurrentBalance from CashAccountSales where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
        //            q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.CashAccountSales.Date, dbo.CashAccountSales.Debit, dbo.CashAccountSales.Credit FROM         dbo.CashAccountSales INNER JOIN                      dbo.ChartofAccounts ON dbo.CashAccountSales.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CashAccountSales.Voucherno='" + id + "' and dbo.CashAccountSales.branchid='" + brid + "'";
        //            ds = objCore.funGetDataSet(q);
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double debit = Convert.ToDouble(val);
        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double credit = Convert.ToDouble(val);
        //                if (logo == "")
        //                {

        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
        //                }
        //                else
        //                {


        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


        //                }
        //                //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //        try
        //        {
        //            ds = new DataSet();
        //            //q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
        //            //q = "select top 1     sum(dbo.CostSalesAccount.Debit) as Debit, sum(dbo.CostSalesAccount.Credit) as Credit, sum(dbo.CostSalesAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.CostSalesAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.CostSalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.CostSalesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.ChartofAccounts.AccountCode";

        //            //q = "select top 1 Balance as CurrentBalance from CostSalesAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
        //            q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.CostSalesAccount.Date, dbo.CostSalesAccount.Debit, dbo.CostSalesAccount.Credit FROM         dbo.CostSalesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.CostSalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.CostSalesAccount.Voucherno='" + id + "' and dbo.CostSalesAccount.branchid='" + brid + "'";
        //            ds = objCore.funGetDataSet(q);
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double debit = Convert.ToDouble(val);
        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double credit = Convert.ToDouble(val);
        //                if (logo == "")
        //                {

        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
        //                }
        //                else
        //                {


        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


        //                }
                        
        //                //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
                
        //        try
        //        {
        //            ds = new DataSet();
        //            // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";

        //            //q = "select top 1 Balance as CurrentBalance from DiscountAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
        //            q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.DiscountAccount.Date, dbo.DiscountAccount.Debit, dbo.DiscountAccount.Credit FROM         dbo.DiscountAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.DiscountAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.DiscountAccount.Voucherno='" + id + "' and dbo.DiscountAccount.branchid='" + brid + "'";
        //            ds = objCore.funGetDataSet(q);
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double debit = Convert.ToDouble(val);
        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double credit = Convert.ToDouble(val);
        //                if (logo == "")
        //                {

        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
        //                }
        //                else
        //                {


        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


        //                }
        //                //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //        try
        //        {
        //            ds = new DataSet();
        //            // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
        //            //q = "select top 1     sum(dbo.GSTAccount.Debit) as Debit, sum(dbo.GSTAccount.Credit) as Credit, sum(dbo.GSTAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.GSTAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.GSTAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.GSTAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.ChartofAccounts.AccountCode";

        //           // q = "select top 1 Balance as CurrentBalance from GSTAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
        //            q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.GSTAccount.Date, dbo.GSTAccount.Debit, dbo.GSTAccount.Credit FROM         dbo.GSTAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.GSTAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.GSTAccount.Voucherno='" + id + "' and dbo.GSTAccount.branchid='" + brid + "'";
        //            ds = objCore.funGetDataSet(q);
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double debit = Convert.ToDouble(val);
        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double credit = Convert.ToDouble(val);
        //                if (logo == "")
        //                {

        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
        //                }
        //                else
        //                {


        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


        //                }
                        
        //                // dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //        try
        //        {
        //            ds = new DataSet();
        //            //q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
        //            //q = "select top 1     sum(dbo.InventoryAccount.Debit) as Debit, sum(dbo.InventoryAccount.Credit) as Credit, sum(dbo.InventoryAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.InventoryAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.InventoryAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.ChartofAccounts.Id='" + id + "' and (dbo.InventoryAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.ChartofAccounts.AccountCode";

        //            //q = "select top 1 Balance as CurrentBalance from InventoryAccount where Date <='" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
        //            q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.InventoryAccount.Date, dbo.InventoryAccount.Debit, dbo.InventoryAccount.Credit FROM         dbo.InventoryAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.InventoryAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.InventoryAccount.Voucherno='" + id + "' and dbo.InventoryAccount.branchid='" + brid + "'";
        //            ds = objCore.funGetDataSet(q);
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double debit = Convert.ToDouble(val);
        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double credit = Convert.ToDouble(val);
        //                if (logo == "")
        //                {

        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
        //                }
        //                else
        //                {


        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


        //                }
        //                //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
                
        //        try
        //        {
        //            ds = new DataSet();
        //            // q = "select top 1    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + cmbaccount.select top 1edValue + "'";
        //            //q = "select top 1     sum(dbo.SalesAccount.Debit) as Debit, sum(dbo.SalesAccount.Credit) as Credit, sum(dbo.SalesAccount.Balance) As CurrentBalance,                       dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description AS title, dbo.ChartofAccounts.AccountCode FROM         dbo.SalesAccount RIGHT OUTER JOIN                      dbo.ChartofAccounts ON dbo.SalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where  dbo.ChartofAccounts.Id='" + id + "' and (dbo.SalesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "') GROUP BY dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.ChartofAccounts.AccountCode";

        //            //q = "select top 1 Balance as CurrentBalance from SalesAccount where Date <'" + dateTimePicker1.Text + "'  and ChartAccountId='" + id + "' order by id desc";
        //            q = "SELECT     dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.Name,dbo.ChartofAccounts.Description, dbo.SalesAccount.Date, dbo.SalesAccount.Debit, dbo.SalesAccount.Credit FROM         dbo.SalesAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.SalesAccount.ChartAccountId = dbo.ChartofAccounts.Id where dbo.SalesAccount.Voucherno='" + id + "' and dbo.SalesAccount.branchid='" + brid + "'";
        //            ds = objCore.funGetDataSet(q);
        //            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //            {

        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Debit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double debit = Convert.ToDouble(val);
        //                val = "";
        //                val = ds.Tables[0].Rows[i]["Credit"].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                double credit = Convert.ToDouble(val);
        //                if (logo == "")
        //                {

        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null);
        //                }
        //                else
        //                {


        //                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"]);


        //                }
        //                //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name);

        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
                

               
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //    return dtrpt;
        //}
    }
}
