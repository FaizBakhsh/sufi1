using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime;
namespace POSRestaurant.Reports.Accounts
{
    public partial class frmDayBook : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmDayBook()
        {
            InitializeComponent();
        }

        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
            
            try
            {
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
            catch (Exception ex)
            {
                               
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
                //POSRestaurant.Reports.Statements.CrystalReport1 rptDoc = new CrystalReport1();
                POSRestaurant.Reports.Accounts.rptcashbook rptDoc = new rptcashbook();
                POSRestaurant.Reports.Accounts.dscashbook dsrpt = new dscashbook();
                //feereport ds = new feereport(); // .xsd file name
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();           
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
                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                //dsrpt.Tables[0].DefaultView.Sort = "Date asc,VoucherNo asc";
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("balance", totalin-totalout);
                rptDoc.SetParameterValue("Comp", company);
                if (rdbank.Checked == true)
                {
                    rptDoc.SetParameterValue("title", "Bank Book");
                }
                if (rdcash.Checked == true)
                {
                    rptDoc.SetParameterValue("title", "Cash Book");
                }
                if (rdday.Checked == true)
                {
                    rptDoc.SetParameterValue("title", "Day Book");
                }
               
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
                rptDoc.SetParameterValue("date", "For the period of " + Convert.ToDateTime(dateTimePicker1.Text).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(dateTimePicker2.Text).ToString("dd-MM-yyyy"));
             
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        double bf = 0;
        public string getname(string id)
        {
            string name = "";
            DataSet dsname = new System.Data.DataSet();
            string q = "select name from ChartofAccounts where id='"+id+"'";
            dsname = objCore.funGetDataSet(q);
            if (dsname.Tables[0].Rows.Count > 0)
            {
                name = dsname.Tables[0].Rows[0][0].ToString();
            }
            return name;
        }
        double totalin = 0, totalout = 0;
        public DataTable getAllOrders()
        {
            getcompany();
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
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Account", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                dtrpt.Columns.Add("Group", typeof(string));
                getcompany();
                totalin = 0; totalout = 0; double totalinopening = 0, totaloutopening = 0;
                DataSet dsaccounts = new System.Data.DataSet();
                string qr = "";
                if (cmbbranchjv.Text == "All")
                {
                    if (rdcash.Checked == true)
                    {
                        qr = "SELECT        dbo.ChartofAccounts.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM            dbo.ChartofAccounts INNER JOIN                         dbo.DayBook ON dbo.ChartofAccounts.Id = dbo.DayBook.AccountId  where dbo.DayBook.Type='Cash' order by dbo.DayBook.Type";
                    }
                    if (rdbank.Checked == true)
                    {
                        qr = "SELECT        dbo.ChartofAccounts.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM            dbo.ChartofAccounts INNER JOIN                         dbo.DayBook ON dbo.ChartofAccounts.Id = dbo.DayBook.AccountId  where dbo.DayBook.Type='Bank'  order by dbo.DayBook.Type";
                    }
                    if (rdday.Checked == true)
                    {
                        qr = "SELECT        dbo.ChartofAccounts.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM            dbo.ChartofAccounts INNER JOIN                         dbo.DayBook ON dbo.ChartofAccounts.Id = dbo.DayBook.AccountId  order by dbo.DayBook.Type";
                    }
                }
                else
                {
                    if (rdcash.Checked == true)
                    {
                        qr = "SELECT        dbo.ChartofAccounts.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM            dbo.ChartofAccounts INNER JOIN                         dbo.DayBook ON dbo.ChartofAccounts.Id = dbo.DayBook.AccountId  where dbo.DayBook.Type='Cash' order by dbo.DayBook.Type";
                    }
                    if (rdbank.Checked == true)
                    {
                        qr = "SELECT        dbo.ChartofAccounts.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM            dbo.ChartofAccounts INNER JOIN                         dbo.DayBook ON dbo.ChartofAccounts.Id = dbo.DayBook.AccountId  where dbo.DayBook.Type='Bank' order by dbo.DayBook.Type";
                    }
                    if (rdday.Checked == true)
                    {
                        qr = "SELECT        dbo.ChartofAccounts.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM            dbo.ChartofAccounts INNER JOIN                         dbo.DayBook ON dbo.ChartofAccounts.Id = dbo.DayBook.AccountId   order by dbo.DayBook.Type";
                    }
                }
                dsaccounts = objCore.funGetDataSet(qr);
                if (checkBox1.Checked == true)
                {
                    for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
                    {
                        bf = 0;
                        string type = dsaccounts.Tables[0].Rows[k]["Type"].ToString();
                        string acid = dsaccounts.Tables[0].Rows[k]["id"].ToString();
                        string account = dsaccounts.Tables[0].Rows[k]["Name"].ToString();
                        bf = getBF(acid);
                        totalinopening = totalinopening + bf;
                        //if (type.ToLower() == "cash in")
                        //{

                           
                        //}
                        //if (type.ToLower() == "cash out")
                        //{
                        //    bf = getBF(acid);
                        //    totaloutopening = totaloutopening + bf;
                        //}
                    }
                }

                if (checkBox1.Checked == true)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("","Opening", totalinopening, null, "Cash In");
                        totalin = totalin + totalinopening;
                    }
                    else
                    {
                        dtrpt.Rows.Add("", "Opening", totalinopening, dscompany.Tables[0].Rows[0]["logo"], "Cash In");
                        totalin = totalin + totalinopening;
                    }
                    //if (logo == "")
                    //{
                    //    dtrpt.Rows.Add("", "Opening", totaloutopening, null, "Cash Out");
                    //    totalout = totalout + totaloutopening;
                    //}
                    //else
                    //{
                    //    dtrpt.Rows.Add("Opening", totaloutopening, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");
                    //    totalout = totalout + totaloutopening;
                    //}
                }
                for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
                {
                    string type = dsaccounts.Tables[0].Rows[k]["Type"].ToString();
                    string acid = dsaccounts.Tables[0].Rows[k]["id"].ToString();
                    string nm = dsaccounts.Tables[0].Rows[k]["name"].ToString();

                    string blnce = "", val = "0", val1 = "0";
                    double balance = 0, debit = 0, credit = 0;
                    DataSet ds = new DataSet();
                    string q = "";
                    balance = bf;
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {

                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId,branchid FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId,branchid FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,branchid FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,branchid FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";

                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }


                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit,branchid FROM         EmployeesAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit,branchid FROM         EmployeesAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";

                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit,branchid FROM         SalariesAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit,branchid FROM         SalariesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";

                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                       
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,branchid FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,branchid FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }


                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,branchid FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,branchid FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CashAccountSales where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CashAccountSales where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        // dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CostSalesAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CostSalesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CustomerAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         CustomerAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        
                    }

                    
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         DiscountAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         DiscountAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         GSTAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         GSTAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT        dbo.InventoryAccount.Date, dbo.InventoryAccount.VoucherNo, dbo.InventoryAccount.Description, dbo.InventoryAccount.Debit, dbo.InventoryAccount.Credit, dbo.InventoryAccount.branchid, dbo.RawItem.ItemName FROM            dbo.InventoryAccount INNER JOIN                         dbo.RawItem ON dbo.InventoryAccount.ItemId = dbo.RawItem.Id where dbo.InventoryAccount.ChartAccountId='" + acid + "'  and dbo.InventoryAccount.date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryAccount.voucherno like '%grn%'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT        dbo.InventoryAccount.Date, dbo.InventoryAccount.VoucherNo, dbo.InventoryAccount.Description, dbo.InventoryAccount.Debit, dbo.InventoryAccount.Credit, dbo.InventoryAccount.branchid, dbo.RawItem.ItemName FROM            dbo.InventoryAccount INNER JOIN                         dbo.RawItem ON dbo.InventoryAccount.ItemId = dbo.RawItem.Id where dbo.InventoryAccount.ChartAccountId='" + acid + "'  and dbo.InventoryAccount.branchid='" + cmbbranchjv.SelectedValue + "'  and dbo.InventoryAccount.date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryAccount.voucherno like '%grn%'   ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         JournalAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         JournalAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         SalesAccount where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         SalesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         SupplierAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance,branchid FROM         SupplierAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        val1 = ds.Tables[0].Rows[i]["Credit"].ToString();
                        if (val1 == "")
                        {
                            val1 = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            credit = credit + Convert.ToDouble(val1);

                            totalin = totalin + Convert.ToDouble(val);

                            totalout = totalout + Convert.ToDouble(val1);

                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            credit = Convert.ToDouble(val1);

                            totalin = totalin + debit;
                            totalout = totalout + credit;

                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, null, "Cash In");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                                }
                            }
                            if (credit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString())+ ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, null, "Cash Out");
                                }
                                else
                                {
                                    dtrpt.Rows.Add(branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString() + "(Dated: " + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy") + ")", ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                    }
                    if (chksum.Checked == true)
                    {
                        if (debit != 0)
                        {
                            if (logo == "")
                            {

                                dtrpt.Rows.Add("", nm, debit, null, "Cash In");
                            }
                            else
                            {
                                dtrpt.Rows.Add("", nm, debit, dscompany.Tables[0].Rows[0]["logo"], "Cash In");

                            }
                        }
                        if (credit != 0)
                        {
                            if (logo == "")
                            {

                                dtrpt.Rows.Add("", nm, credit, null, "Cash Out");
                            }
                            else
                            {
                                dtrpt.Rows.Add("", nm, credit, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");

                            }
                        }
                    }
                    ds = new DataSet();
                    
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        public string branchname(string branchid)
        {
            string name = "";

            try
            {
                string q = "select BranchName from Branch where id=" + branchid;
                DataSet ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    name = ds.Tables[0].Rows[0][0].ToString();
                    if (name.Length > 0)
                    {
                        name = "(" + name + ")";
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return name;
        }
        public double getBF(string id)
        {
            bf = 0;
            string q = "";
            //q = "select  AccountType from ChartofAccounts where id='" + cmbaccount.SelectedValue + "'";
            DataSet ds = new DataSet();
            //ds = objCore.funGetDataSet(q);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    string type = ds.Tables[0].Rows[0][0].ToString();
            //    if (type == "Cost of Sales" || type == "Revenue" || type == "Operating Expenses" || type == "Admin and Selling Expenses" || type == "Financial Expenses" || type == "Marketing Expenses")
            //    {
            //       // return;
            //    }
            //}
            try
            {

                string blnce = "", val = "0";
                double balancebf = 0, debitbf = 0, creditbf = 0;


                ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountPaymentSupplier where ChartAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'  ";
               
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountPaymentSupplier where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'  ";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountReceiptCustomer where ChartAccountId='" + id + "'  and  date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountReceiptCustomer where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and  date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "'  and  date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and  date <'" + dateTimePicker1.Text + "'";
              
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPurchase where ChartAccountId='" + id + "'   and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPurchase where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'  and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and   date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and  date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountSales where ChartAccountId='" + id + "' and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountSales where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CostSalesAccount where ChartAccountId='" + id + "' and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CostSalesAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CustomerAccount where PayableAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CustomerAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         DiscountAccount where ChartAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         DiscountAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         GSTAccount where ChartAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         GSTAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         InventoryAccount where ChartAccountId='" + id + "' and  date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         InventoryAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + id + "' and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where PayableAccountId='" + id + "' and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                // q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
                //q = "SELECT     Id, Date, PayableAccountId, VoucherNo, Description, Debit, Credit, Balance, branchid FROM         PettyCash where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
        private void button1_Click(object sender, EventArgs e)
        {
           
            System.GC.Collect();
            
            button1.Text = "Please Wait(Downloading Data)";
            button1.Enabled = false;
            bindreport();
            button1.Text = "Submit";
            button1.Enabled = true;
        }

        private void cmbbranchjv_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            POSRestaurant.Accounts.DayBookAccounts obj = new POSRestaurant.Accounts.DayBookAccounts();
            obj.Show();
        }

        private void rdcash_CheckedChanged(object sender, EventArgs e)
        {
            if (rdcash.Checked == true)
            {
                rdbank.Checked = false;
                rdday.Checked = false;
            }
        }

        private void rdbank_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbank.Checked == true)
            {
                rdcash.Checked = false;
                rdday.Checked = false;
            }
        }

        private void rdday_CheckedChanged(object sender, EventArgs e)
        {
            if (rdday.Checked == true)
            {
                //checkBox1.Checked = false;
                rdcash.Checked = false;
                rdbank.Checked = false;
            }
        }
    }
}
