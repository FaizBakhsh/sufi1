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
    public partial class frmReceiving : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string id = "";
        public string name = "";
        public string branch = "";
        public string type = "";
        public frmReceiving()
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
                POSRestaurant.Reports.Voucher.rptreceiving rptDoc = new rptreceiving();
                POSRestaurant.Reports.Voucher.rptpaidreceiving rptDoc2 = new rptpaidreceiving();
                POSRestaurant.Reports.Voucher.dsVoucherPreview dsrpt = new dsVoucherPreview();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                if (type == "bpv" || type == "cpv")
                {
                    dt = getAllOrdersbankpayment(id);
                }
               
                if (type == "crv")
                {
                    dt = getAllOrderscashreceipt(branch);
                }
                if (type == "brv")
                {
                    dt = getAllOrdersbankreceipt(branch);
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
                
                if (type == "bpv" || type == "cpv")
                {
                    rptDoc2.SetDataSource(dsrpt);
                    rptDoc2.SetParameterValue("Comp", company);
                    rptDoc2.SetParameterValue("phn", phone);
                    rptDoc2.SetParameterValue("Addrs", address);
                    rptDoc2.SetParameterValue("branch", br);
                    rptDoc2.SetParameterValue("balance", balance);
                    crystalReportViewer1.ReportSource = rptDoc2;
                }
                else
                {
                    rptDoc.SetDataSource(dsrpt);
                    rptDoc.SetParameterValue("Comp", company);
                    rptDoc.SetParameterValue("phn", phone);
                    rptDoc.SetParameterValue("Addrs", address);
                    rptDoc.SetParameterValue("branch", br);
                    crystalReportViewer1.ReportSource = rptDoc;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        double balance = 0;
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
        public string getAccouname(string id)
        {
            string sup = "";
            try
            {
                DataSet dssuplier = new DataSet();
                string q = "select name from ChartofAccounts where id='" + id + "'";
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
     
        public DataTable getAllOrdersbankpayment(string id)
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
                DataSet ds = new DataSet();
                string q = "", val = "",paidto="";
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
                    if (type == "bpv")
                    {
                        q = "select  * from BankAccountPaymentSupplier where Voucherno='" + id + "' order by id desc";
                    }
                    if (type == "cpv")
                    {
                        q = "select  * from CashAccountPaymentSupplier where Voucherno='" + id + "' order by id desc";
                    }
                    DataSet dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        string sid = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                        if (sid == "0")
                        {

                            DataSet dsbpvs = new System.Data.DataSet();
                            q = "select PayableAccountId from SupplierAccount where Voucherno='" + id + "'";
                            dsbpvs = objCore.funGetDataSet(q);
                            if (dsbpvs.Tables[0].Rows.Count > 0)
                            {
                                paidto = getAccouname((dsbpvs.Tables[0].Rows[0]["PayableAccountId"].ToString()));
                            }
                            else
                            {
                                string empid = "";
                                q = "select  EmployeeId from EmployeesAccount where Voucherno='" + id + "'";
                                dsbpvs = objCore.funGetDataSet(q);
                                if (dsbpvs.Tables[0].Rows.Count > 0)
                                {
                                    empid = dsbpvs.Tables[0].Rows[0]["EmployeeId"].ToString();
                                    paidto = getemployee((dsbpvs.Tables[0].Rows[0]["EmployeeId"].ToString()));
                                }
                                try
                                {
                                    try
                                    {
                                        q = "select (sum(debit)-sum(credit)) as balance  from EmployeesAccount where EmployeeId='" + empid + "' ";
                                        dsbpvs = objCore.funGetDataSet(q);
                                        if (dsbpvs.Tables[0].Rows.Count > 0)
                                        {
                                            string temp = dsbpvs.Tables[0].Rows[0]["balance"].ToString();
                                            if (temp == "")
                                            {
                                                temp = "0";
                                            }
                                            balance = Convert.ToDouble(temp);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        
                                    
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                  
                                }
                            }

                        }
                        else
                        {

                            string spid = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                            try
                            {
                                q = "select (sum(debit)-sum(credit)) as balance  from SupplierAccount where SupplierId='" + spid + "' ";
                                DataSet dsbpvs = new DataSet();
                                dsbpvs = objCore.funGetDataSet(q);
                                if (dsbpvs.Tables[0].Rows.Count > 0)
                                {
                                    string temp = dsbpvs.Tables[0].Rows[0]["balance"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "0";
                                    }
                                    balance = Convert.ToDouble(temp);
                                }
                            }
                            catch (Exception ex)
                            {


                            }
                                
                            paidto = getsupplier((spid));
                        }


                        string voucherno = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                        string amount = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                        string desc = dsacount.Tables[0].Rows[0]["Description"].ToString();

                        string chkno = "";
                        try
                        {
                            chkno = dsacount.Tables[0].Rows[0]["CheckNo"].ToString();
                        }
                        catch (Exception ex)
                        {
                            
                          
                        }

                        string date = dsacount.Tables[0].Rows[0]["date"].ToString();
                       
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(Convert.ToDateTime(date).ToShortDateString(), chkno, paidto, amount, "0", id, name, null, "", desc);
                        }
                        else
                        {
                            dtrpt.Rows.Add(Convert.ToDateTime(date).ToShortDateString(), chkno, paidto, amount, "0", id, name, dscompany.Tables[0].Rows[0]["logo"], "", desc);

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
                    q = "SELECT dbo.BankAccountReceiptCustomer.Description,dbo.BankAccountReceiptCustomer.CheckNo, dbo.BankAccountReceiptCustomer.Date, dbo.BankAccountReceiptCustomer.CustomerId, dbo.BankAccountReceiptCustomer.Debit,                dbo.BankAccountReceiptCustomer.Credit, dbo.Customers.Name FROM  dbo.BankAccountReceiptCustomer INNER JOIN               dbo.Customers ON dbo.BankAccountReceiptCustomer.CustomerId = dbo.Customers.Id where dbo.BankAccountReceiptCustomer.Voucherno='" + id + "' and dbo.BankAccountReceiptCustomer.branchid='" + brid + "'";
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

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["CheckNo"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["CheckNo"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


                        }
                        //dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["AccountCode"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit,id, name);

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
                    q = "SELECT dbo.CashAccountReceiptCustomer.Description, dbo.CashAccountReceiptCustomer.Date, dbo.CashAccountReceiptCustomer.CustomerId, dbo.CashAccountReceiptCustomer.Debit,                dbo.CashAccountReceiptCustomer.Credit, dbo.Customers.Name FROM  dbo.CashAccountReceiptCustomer INNER JOIN               dbo.Customers ON dbo.CashAccountReceiptCustomer.CustomerId = dbo.Customers.Id where dbo.CashAccountReceiptCustomer.Voucherno='" + id + "' and dbo.CashAccountReceiptCustomer.branchid='" + brid + "'";
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

                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), "", ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, null, sup, ds.Tables[0].Rows[i]["Description"].ToString());
                        }
                        else
                        {


                            dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), "", ds.Tables[0].Rows[i]["Name"].ToString(), debit, credit, id, name, dscompany.Tables[0].Rows[0]["logo"], sup, ds.Tables[0].Rows[i]["Description"].ToString());


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
       
    }
}
