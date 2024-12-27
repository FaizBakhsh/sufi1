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
    public partial class frmCashBook : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmCashBook()
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
                //DataRow dr = ds.Tables[0].NewRow();
                //dr["BranchName"] = "All";
                //ds.Tables[0].Rows.Add(dr);
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
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
                rptDoc.SetParameterValue("title", "Cash Book");
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
                qr = "SELECT        dbo.ChartofAccounts.Id, dbo.ChartofAccounts.Name, dbo.CashBook.Type FROM            dbo.ChartofAccounts INNER JOIN                         dbo.CashBook ON dbo.ChartofAccounts.Id = dbo.CashBook.AccountId  where dbo.ChartofAccounts.branchid='" + cmbbranchjv.SelectedValue + "' order by dbo.CashBook.Type";

                dsaccounts = objCore.funGetDataSet(qr);
                if (checkBox1.Checked == true)
                {
                    for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
                    {
                        bf = 0;
                        string type = dsaccounts.Tables[0].Rows[k]["Type"].ToString();
                        string acid = dsaccounts.Tables[0].Rows[k]["id"].ToString();
                        string account = dsaccounts.Tables[0].Rows[k]["Name"].ToString();

                        if (type.ToLower() == "cash in")
                        {

                            bf = getBF(acid);
                            totalinopening = totalinopening + bf;
                        }
                        if (type.ToLower() == "cash out")
                        {
                            bf = getBF(acid);
                            totaloutopening = totaloutopening + bf;
                        }
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
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("", "Opening", totaloutopening, null, "Cash Out");
                        totalout = totalout + totaloutopening;
                    }
                    else
                    {
                        dtrpt.Rows.Add("Opening", totaloutopening, dscompany.Tables[0].Rows[0]["logo"], "Cash Out");
                        totalout = totalout + totaloutopening;
                    }
                }
                for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
                {
                    string type = dsaccounts.Tables[0].Rows[k]["Type"].ToString();
                    string acid = dsaccounts.Tables[0].Rows[k]["id"].ToString();
                    string nm = dsaccounts.Tables[0].Rows[k]["name"].ToString();

                   
                    //bf = getBF(acid);
                    //string brf = "";
                    //if (bf.ToString().Contains("-"))
                    //{
                    //    brf = "(" + bf.ToString().Replace("-", "") + ")";
                    //}
                    //else
                    //{
                    //    brf = bf.ToString();
                    //}

                    //if (logo == "")
                    //{
                    //    dtrpt.Rows.Add("Opening", bf, null, type);
                    //}
                    //else
                    //{
                    //    dtrpt.Rows.Add("Opening", bf, dscompany.Tables[0].Rows[0]["logo"], type);

                    //}

                    string blnce = "", val = "0";
                    double balance = 0, debit = 0, credit = 0;
                    DataSet ds = new DataSet();
                    string q = "";
                    balance = bf;
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {

                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";

                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }


                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit FROM         EmployeesAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit FROM         EmployeesAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";

                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit FROM         SalariesAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit FROM         SalariesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";

                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                       
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }


                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        // dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    //ds = new DataSet();
                    //if (cmbbranchjv.Text == "All")
                    //{
                    //    q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         EmployeesAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    //}
                    //else
                    //{
                    //    q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         EmployeesAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    //}

                    //ds = objCore.funGetDataSet(q);
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    //    if (val == "")
                    //    {
                    //        val = "0";
                    //    }
                    //    if (chksum.Checked == true)
                    //    {
                    //        debit = debit + Convert.ToDouble(val);
                    //        if (type == "Cash In")
                    //        {
                    //            totalin = totalin + Convert.ToDouble(val);
                    //        }
                    //        if (type == "Cash Out")
                    //        {
                    //            totalout = totalout + Convert.ToDouble(val);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        debit = Convert.ToDouble(val);
                    //        if (type == "Cash In")
                    //        {
                    //            totalin = totalin + debit;
                    //        }
                    //        if (type == "Cash Out")
                    //        {
                    //            totalout = totalout + debit;
                    //        }
                    //    }
                    //    if (chksum.Checked == false)
                    //    {
                    //        if (debit != 0)
                    //        {
                    //            if (logo == "")
                    //            {

                    //                dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                    //            }
                    //            else
                    //            {
                    //                dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                    //            }
                    //        }
                    //    }
                    //    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    //}

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT        dbo.InventoryAccount.Date, dbo.InventoryAccount.VoucherNo, dbo.InventoryAccount.Description, dbo.InventoryAccount.Debit, dbo.InventoryAccount.Credit, dbo.InventoryAccount.Balance, dbo.RawItem.ItemName FROM            dbo.InventoryAccount INNER JOIN                         dbo.RawItem ON dbo.InventoryAccount.ItemId = dbo.RawItem.Id where dbo.InventoryAccount.ChartAccountId='" + acid + "'  and dbo.InventoryAccount.date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryAccount.voucherno like '%grn%'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT        dbo.InventoryAccount.Date, dbo.InventoryAccount.VoucherNo, dbo.InventoryAccount.Description, dbo.InventoryAccount.Debit, dbo.InventoryAccount.Credit, dbo.InventoryAccount.Balance, dbo.RawItem.ItemName FROM            dbo.InventoryAccount INNER JOIN                         dbo.RawItem ON dbo.InventoryAccount.ItemId = dbo.RawItem.Id where dbo.InventoryAccount.ChartAccountId='" + acid + "'  and dbo.InventoryAccount.branchid='" + cmbbranchjv.SelectedValue + "'  and dbo.InventoryAccount.date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryAccount.voucherno like '%grn%'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text);
                    }

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    }

                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        val = ds.Tables[0].Rows[i]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        if (chksum.Checked == true)
                        {
                            debit = debit + Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + Convert.ToDouble(val);
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + Convert.ToDouble(val);
                            }
                        }
                        else
                        {
                            debit = Convert.ToDouble(val);
                            if (type == "Cash In")
                            {
                                totalin = totalin + debit;
                            }
                            if (type == "Cash Out")
                            {
                                totalout = totalout + debit;
                            }
                        }
                        if (chksum.Checked == false)
                        {
                            if (debit != 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, null, type);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Description"].ToString(), nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                                }
                            }
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                    }
                    if (chksum.Checked == true)
                    {
                        if (debit != 0)
                        {
                            if (logo == "")
                            {

                                dtrpt.Rows.Add("", nm, debit, null, type);
                            }
                            else
                            {
                                dtrpt.Rows.Add("", nm, debit, dscompany.Tables[0].Rows[0]["logo"], type);

                            }
                        }
                    }
                    ds = new DataSet();
                    //if (cmbbranchjv.Text == "All")
                    //{
                    //    q = "SELECT     Id, Date, PayableAccountId, VoucherNo, Description, Debit, Credit, Balance, branchid FROM         PettyCash where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    //}
                    //else
                    //{
                    //    q = "SELECT     Id, Date, PayableAccountId, VoucherNo, Description, Debit, Credit, Balance, branchid FROM         PettyCash where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and debit>0 ORDER BY  Date, VoucherNo";
                    //}

                    //ds = objCore.funGetDataSet(q);
                    //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    //{
                    //    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    //    if (val == "")
                    //    {
                    //        val = "0";
                    //    }
                    //    debit = Convert.ToDouble(val);

                    //    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    //    if (val == "")
                    //    {
                    //        val = "0";
                    //    }
                    //    credit = Convert.ToDouble(val);

                    //    balance = balance + (debit);
                    //    blnce = "";
                    //    blnce = balance.ToString();

                    //    //if (blnce.Contains("-"))
                    //    //{
                    //    //    blnce = "(" + blnce.Replace("-", "") + ")";
                    //    //}
                    //    if (logo == "")
                    //    {

                    //        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, null, nm);
                    //    }
                    //    else
                    //    {
                    //        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"],nm);
                    //    }
                    //    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["Debit"].ToString(), ds.Tables[0].Rows[i]["Credit"].ToString(), blnce, cmbaccount.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                    //} 
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountPaymentSupplier where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'  ";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountReceiptCustomer where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and  date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and  date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPurchase where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "'  and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and  date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountSales where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CostSalesAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CustomerAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         DiscountAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         GSTAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         InventoryAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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
            POSRestaurant.Accounts.CashBookAccounts obj = new POSRestaurant.Accounts.CashBookAccounts();
            obj.Show();
        }

        private void crystalReportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
