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
using CrystalDecisions.CrystalReports.Engine;
namespace POSRestaurant.Reports.Statements
{
    public partial class frmGLAccounts : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmGLAccounts()
        {
            InitializeComponent();
        }
        public string start = "", end = "", branchid = "",accountid="", reference = "";
        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
            try
            {
                fillsubheads();
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
            cmbhead.SelectedItem = "All";
            fillacount();
            if (reference == "pnl")
            {
                dateTimePicker1.Text = start;
                dateTimePicker2.Text = end;
                if (branchid == "All")
                {
                    cmbbranchjv.SelectedItem = branchid;
                }
                else
                {
                    cmbbranchjv.SelectedValue = branchid;
                }
                cmbaccount.SelectedValue = accountid;

                
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;
                bindreport();
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
                ReportDocument  rptDoc= null;
                DataTable dt = new DataTable();
                //POSRestaurant.Reports.Statements.CrystalReport1 rptDoc = new CrystalReport1();
                if (chkgroup.Checked == false)
                {
                     rptDoc = new crpledgeraccountplain();
                   
                }
                else
                {
                     rptDoc = new crplegeraccounts1();
                }
                POSRestaurant.Reports.Statements.dsledgeraccounts dsrpt = new dsledgeraccounts();
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
                if (cmbaccount.Text == "Cash-POS")
                { }
                else
                {
                    DataView dv = dt2.DefaultView;
                    dt2.DefaultView.Sort = "Date asc,VoucherNo asc";
                }
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
                        dt.Rows.Add("", "", "", "0", "0", "", "", dscompany.Tables[0].Rows[0]["logo"]);
                        dsrpt.Tables[0].Merge(dt2, true, MissingSchemaAction.Ignore);
                    }
                }

                //dsrpt.Tables[0].DefaultView.Sort = "Date asc,VoucherNo asc";
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
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
            string q = "select name from ChartofAccounts where id='" + id + "'";
            dsname = objCore.funGetDataSet(q);
            if (dsname.Tables[0].Rows.Count > 0)
            {
                name = dsname.Tables[0].Rows[0][0].ToString();
            }
            return name;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("VoucherNo", typeof(string));
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("Debit", typeof(double));
                dtrpt.Columns.Add("Credit", typeof(double));
                dtrpt.Columns.Add("Balance", typeof(string));
                dtrpt.Columns.Add("account", typeof(string));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("Opening", typeof(double));
                getcompany();
                DataSet dsaccounts = new System.Data.DataSet();
                string qr = "";
                if (cmbaccount.Text.ToLower() == "all")
                {
                    if (cmbhead.Text.ToLower() == "all")
                    {
                        qr = "select id,name from ChartofAccounts where status='active'  order by name";
                    }
                    else
                    {
                        if (cmbsubhead.Text == "All")
                        {
                            if (cmbsubaccount.Text == "All")
                            {
                                qr = "select id,name from ChartofAccounts where status='active'  and AccountType like '%" + cmbhead.Text + "%' order by name";
                            }
                            else
                            {
                                qr = "select id,name from ChartofAccounts where status='active'  and SubAccount='" + cmbsubaccount.Text + "' and AccountType like '%" + cmbhead.Text + "%' order by name";
                            }
                        }
                        else
                        {
                            if (cmbsubaccount.Text == "")
                            {
                                qr = "select id,name from ChartofAccounts where status='active'  and AccountType = '" + cmbsubhead.Text + "' order by name";
                            }
                            else
                            {
                                qr = "select id,name from ChartofAccounts where status='active'  and SubAccount='" + cmbsubaccount.Text + "'  and AccountType = '" + cmbsubhead.Text + "' order by name";
                            }
                            
                        }

                    }

                }
                else
                {
                    qr = "select id,name from ChartofAccounts where id='" + cmbaccount.SelectedValue + "' ";
                }
                dsaccounts = objCore.funGetDataSet(qr);
                for (int k = 0; k < dsaccounts.Tables[0].Rows.Count; k++)
                {
                    bf = 0;
                    string acid = dsaccounts.Tables[0].Rows[k][0].ToString();
                    if (checkBox1.Checked == true)
                    {
                        bf = getBF(acid);
                    }
                    string nm = dsaccounts.Tables[0].Rows[k]["name"].ToString();
                    string logo = "";
                    try
                    {
                        logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                    }
                    catch (Exception ex)
                    {


                    }
                    string brf = "";
                    if (chkgroup.Checked == true)
                    {
                        if (bf.ToString().Contains("-"))
                        {
                            brf = "(" + bf.ToString().Replace("-", "") + ")";
                        }
                        else
                        {
                            brf = bf.ToString();
                        }

                        if (logo == "")
                        {
                            //string date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString();
                            if (bf >= 0)
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, cmbaccount.Text, null, nm);
                            }
                            else
                            {
                                bf = System.Math.Abs(bf);
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, cmbaccount.Text, null, nm);
                            }
                        }
                        else
                        {
                            if (bf >= 0)
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf.ToString(), cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                            }
                            else
                            {
                                bf = System.Math.Abs(bf);
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf.ToString(), cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                            }
                        }
                    }
                    string blnce = "", val = "0";
                    double balance = 0, debit = 0, credit = 0, totaldebit = 0, totalcredit = 0;
                    DataSet ds = new DataSet();
                    string q = "";
                    balance = bf;
                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {

                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance,ChartAccountId FROM         BankAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                    }
                                }

                            }
                            else
                            {
                                if (logo == "")
                                {
                                    //string date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString();
                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                }
                            }
                            
                        }
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";

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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {

                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                    }
                                }

                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                }
                            }
                           
                            //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                        }
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                    }
                                }

                            }
                            else
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                }
                            }                            
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountPurchase where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                   }
                               }
                           }
                           else
                           {
                               if (logo == "")
                               {
                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {
                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                               }
                           }                                                    
                       }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }


                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {

                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                   }
                               }
                           }
                           else
                           {
                               if (logo == "")
                               {
                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {
                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                               }
                           }
                           
                       }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CashAccountSales where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                   }
                               }

                           }
                           else
                           {
                               if (logo == "")
                               {

                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {
                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                               }
                           }
                           string time = DateTime.Now.ToLongTimeString();
                           
                       }
                        // dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CostSalesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                    }
                                }
                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                }
                            }
                            
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         CustomerAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                   }
                               }

                           }
                           else
                           {
                               if (logo == "")
                               {

                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {

                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                               }

                           }
                           
                       }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         BranchAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         BranchAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {

                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {


                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                   }
                               }

                           }
                           else
                           {
                               if (logo == "")
                               {

                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {


                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                               }
                           }
                           
                       }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         DiscountAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {

                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {


                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                    }
                                }

                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {


                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                }
                            }
                            
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         SalariesAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         SalariesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {

                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {


                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                    }
                                }

                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {


                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                }
                            }
                            
                        }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose();

                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         GSTAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {

                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {
                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                   }
                               }
                               
                           }
                           else
                           {
                               if (logo == "")
                               {

                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {


                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                               }
                           }
                          
                       }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         InventoryAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {

                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {
                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);
                                    }
                                }
                                
                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {


                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                }

                            }
                            
                        }

                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         JournalAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {

                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {


                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                   }
                               }
                               
                           }
                           else
                           {
                               if (logo == "")
                               {

                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {


                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                               }
                           }
                           
                       }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + acid + "'  and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         SalesAccount where ChartAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                       blnce = "";
                       if (chksum.Checked == true)
                       {
                           totaldebit = totaldebit + debit;
                           totalcredit = totalcredit + credit;
                       }
                       else
                       {
                           if (chkempty.Checked == true)
                           {
                               if ((debit + credit) > 0)
                               {
                                   if (logo == "")
                                   {

                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                   }
                                   else
                                   {


                                       dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                   }
                               }
                               
                           }
                           else
                           {
                               if (logo == "")
                               {

                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                               }
                               else
                               {


                                   dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                               }
                           }
                           
                       }
                        //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text);
                    }

                    ds.Dispose(); 
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit, Balance FROM         SupplierAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {

                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {


                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                    }
                                }
                                else
                                {

                                }
                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {


                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                }
                            }
                            
                        }
                    }

                    ds.Dispose();
                    ds = new DataSet();
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit FROM         EmployeesAccount where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    }
                    else
                    {
                        q = "SELECT    Date,branchid,  Voucherno,  Description, Debit, Credit FROM         EmployeesAccount where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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
                        if (chkcredit.Checked == true)
                        {
                            debit = 0;
                        }
                        if (chkdebit.Checked == true)
                        {
                            credit = 0;
                        }
                        blnce = "";
                        if (chksum.Checked == true)
                        {
                            totaldebit = totaldebit + debit;
                            totalcredit = totalcredit + credit;
                        }
                        else
                        {
                            if (chkempty.Checked == true)
                            {
                                if ((debit + credit) > 0)
                                {
                                    if (logo == "")
                                    {

                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(),branchname(ds.Tables[0].Rows[i]["branchid"].ToString())+ ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text, null, nm);
                                    }
                                    else
                                    {


                                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), branchname(ds.Tables[0].Rows[i]["branchid"].ToString()) + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                    }
                                }
                                
                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text, null, nm);
                                }
                                else
                                {


                                    dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm);

                                }
                            }
                            

                        }
                    }

                    ds.Dispose();
                    if (checkBox1.Checked == false)
                    {
                        bf = 0;
                    }
                    if (chksum.Checked == true)
                    {
                        if (chkempty.Checked == true)
                        {
                            if ((totaldebit + totalcredit) > 0)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(Convert.ToDateTime(dateTimePicker2.Text), "", "Total Transactions", totaldebit, totalcredit, blnce, cmbaccount.Text, null, nm,bf);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(Convert.ToDateTime(dateTimePicker2.Text).ToShortDateString(), "", "Total Transactions", totaldebit, totalcredit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm, bf);
                                }
                            }
                        }
                        else
                        {
                            if (logo == "")
                            {

                                dtrpt.Rows.Add(Convert.ToDateTime(dateTimePicker2.Text), "", "Total Transactions", totaldebit, totalcredit, blnce, cmbaccount.Text, null, nm, bf);
                            }
                            else
                            {


                                dtrpt.Rows.Add(Convert.ToDateTime(dateTimePicker2.Text).ToShortDateString(), "", "Total Transactions", totaldebit, totalcredit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"], nm, bf);

                            }
                        }
                    }

                    ds = new DataSet();
                    //if (cmbbranchjv.Text == "All")
                    //{
                    //    q = "SELECT     Id, Date, PayableAccountId, VoucherNo, Description, Debit, Credit, Balance, branchid FROM         PettyCash where PayableAccountId='" + acid + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
                    //}
                    //else
                    //{
                    //    q = "SELECT     Id, Date, PayableAccountId, VoucherNo, Description, Debit, Credit, Balance, branchid FROM         PettyCash where PayableAccountId='" + acid + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date between'" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY  Date, VoucherNo";
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

                    //    balance = balance + (debit - credit);
                    //    blnce = "";
                    //    blnce = balance.ToString();

                    //    //if (blnce.Contains("-"))
                    //    //{
                    //    //    blnce = "(" + blnce.Replace("-", "") + ")";
                    //    //}
                    //    if (logo == "")
                    //    {

                    //        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text, null, nm);
                    //    }
                    //    else
                    //    {
                    //        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text, dscompany.Tables[0].Rows[0]["logo"],nm);
                    //    }
                    //    //dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit,credit, blnce, cmbaccount.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                    //} 
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


                ds.Dispose(); ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalariesAccount where ChartAccountId='" + id + "' and date <'" + dateTimePicker1.Text + "'  ";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalariesAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'  ";
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

                ds.Dispose();
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

                ds.Dispose();
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

                ds.Dispose(); ds = new DataSet();
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

                ds.Dispose(); ds = new DataSet();
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
                ds.Dispose(); ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "'  and  date <'" + dateTimePicker1.Text + "'";
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
                ds.Dispose(); ds = new DataSet();
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

                ds.Dispose(); ds = new DataSet();
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

                ds.Dispose();
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

                ds.Dispose();
                ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BranchAccount where PayableAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BranchAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
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

                ds.Dispose();
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

                ds.Dispose(); ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         GSTAccount where ChartAccountId='" + id + "' and date <'" + dateTimePicker1.Text + "'";
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

                ds.Dispose(); ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         InventoryAccount where ChartAccountId='" + id + "' and date <'" + dateTimePicker1.Text + "'";
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

                ds.Dispose(); ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and  date <'" + dateTimePicker1.Text + "'";
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

                ds.Dispose(); ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
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

                ds.Dispose(); ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where PayableAccountId='" + id + "'  and date <'" + dateTimePicker1.Text + "'";
              
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

                ds.Dispose(); ds = new DataSet();
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
            if (cmbaccount.Text == "")
            {
                MessageBox.Show("Please Select Supplier");
                return;
            }

            System.GC.Collect();

            button1.Text = "Please Wait(Downloading Data)";
            button1.Enabled = false;
            bindreport();
            button1.Text = "Submit";
            button1.Enabled = true;
        }

        private void cmbbranchjv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fillacount();
        }
        protected void fillsubheads()
        {
            cmbsubhead.Items.Clear();
            if (cmbhead.Text == "All")
            {
                cmbsubhead.Items.Add("All");
            }
            else
            {

                if (cmbhead.Text == "Assets")
                {
                    cmbsubhead.Items.Add("All");
                    cmbsubhead.Items.Add("Fixed Assets");
                    cmbsubhead.Items.Add("Current Assets");
                }
                if (cmbhead.Text == "Liabilities")
                {
                    cmbsubhead.Items.Add("All");
                    cmbsubhead.Items.Add("Long Term Liabilities");
                    cmbsubhead.Items.Add("Current Liabilities");
                }
                if (cmbhead.Text == "Equity and Capital")
                {
                    cmbsubhead.Items.Add("All");
                }
                if (cmbhead.Text == "Cost of Sales")
                {
                    cmbsubhead.Items.Add("All");
                }
                if (cmbhead.Text == "Revenue")
                {
                    cmbsubhead.Items.Add("All");
                }
                if (cmbhead.Text == "Expenses")
                {
                    cmbsubhead.Items.Add("All");
                    cmbsubhead.Items.Add("Operating Expenses");
                    cmbsubhead.Items.Add("Admin and Selling Expenses");
                    cmbsubhead.Items.Add("Financial Expenses");
                }

            }
            cmbsubhead.SelectedItem = "All";
        }
        protected void getsubaccounts()
        {
            try
            {
                string q = "select distinct SubAccount from ChartofAccounts where  ISNULL(LEN(SubAccount), '') > 1233";
                if (cmbhead.Text == "All")
                {

                }
                else
                {
                    if (cmbsubhead.Text == "All")
                    {
                        q = "select distinct SubAccount from ChartofAccounts where AccountType like '%" + cmbhead.Text + "%' and ISNULL(LEN(SubAccount), '') > 0";
                    }
                    else
                    {
                        q = "select distinct SubAccount from ChartofAccounts where AccountType = '" + cmbsubhead.Text + "' and ISNULL(LEN(SubAccount), '') > 0";
                    }
                }
                DataSet ds = objCore.funGetDataSet(q);
                DataRow dr1 = ds.Tables[0].NewRow();
                dr1["SubAccount"] = "All";
                ds.Tables[0].Rows.Add(dr1);

                cmbsubaccount.DataSource = ds.Tables[0];
                cmbsubaccount.ValueMember = "SubAccount";
                cmbsubaccount.DisplayMember = "SubAccount";
                cmbsubaccount.SelectedValue = "All";
            }
            catch (Exception ex)
            {


            }
        }
        public void fillacount()
        {

            try
            {
                DataSet ds1 = new DataSet();
                string q = "";
                if (cmbhead.Text == "All")
                {
                    q = "select id,name from ChartofAccounts where status='active'  order by name";
                }
                else
                {
                    if (cmbsubhead.Text == "All")
                    {
                        if (cmbsubaccount.Text == "All")
                        {
                            q = "select id,name from ChartofAccounts where status='active'  and AccountType like '%" + cmbhead.Text + "%' order by name";
                        }
                        else
                        {
                            q = "select id,name from ChartofAccounts where status='active'  and SubAccount='"+cmbsubaccount.Text+"' and AccountType like '%" + cmbhead.Text + "%' order by name";
                        }
                    }
                    else
                    {
                        if (cmbsubaccount.Text == "All")
                        {
                            q = "select id,name from ChartofAccounts where status='active'  and AccountType = '" + cmbsubhead.Text + "' order by name";
                        }
                        else
                        {
                            q = "select id,name from ChartofAccounts where status='active'  and SubAccount='" + cmbsubaccount.Text + "'  and AccountType = '" + cmbsubhead.Text + "' order by name";
                        }
                        
                    }
                }

                ds1 = objCore.funGetDataSet(q);
                DataRow dr = ds1.Tables[0].NewRow();
                dr["name"] = "All";
                ds1.Tables[0].Rows.Add(dr);
                cmbaccount.DataSource = ds1.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "name";
                cmbaccount.SelectedItem = "All";

            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillsubheads();

        }

        private void cmbsubhead_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            getsubaccounts(); 
            fillacount();
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
           
            string name = info.Text;
            if (name.Contains("CPV"))
            {
                string id = name;
                POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                obj.id = id;
                obj.branch = cmbbranchjv.SelectedValue.ToString();
                obj.name = "Cash Payment Voucher";
                obj.type = "cpv";
                obj.Show();
            }
            if (name.Contains("BPV"))
            {
                string id = name;
                POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                obj.id = id;
                obj.branch = cmbbranchjv.SelectedValue.ToString();
                obj.name = "Bank Payment Voucher";
                obj.type = "bpv";
                obj.Show();
            }
            if (name.Contains("CRV"))
            {
                string id = name;
                POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                obj.id = id;
                obj.branch = cmbbranchjv.SelectedValue.ToString();
                obj.name = "Cash Receipt Voucher";
                obj.type = "crv";
                obj.Show();
            }
            if (name.Contains("BRV"))
            {
                string id = name;
                POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                obj.id = id;
                obj.branch = cmbbranchjv.SelectedValue.ToString();
                obj.name = "Bank Receipt Voucher";
                obj.type = "brv";
                obj.Show();
            }
            if (name.Contains("JV-") && !name.Contains("S"))
            {
                string id = name;
                POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                obj.id = id;
                obj.branch = cmbbranchjv.SelectedValue.ToString();
                obj.name = "Journal Voucher";
                obj.type = "jv";
                obj.Show();
            }
            if (name.Contains("SJV"))
            {
                string id = name;
                int indx = 0;
                try
                {
                    indx = id.IndexOf("-", 13);
                }
                catch (Exception ex)
                {
                    
                }
                if (indx > 0)
                {
                    id = id.Substring(indx+1);
                }
                POSRestaurant.Reports.SaleReports.FrmcustomerSale obj = new SaleReports.FrmcustomerSale();
                obj.saleid = id;
                //POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                //obj.id = id;
                //obj.branch = cmbbranchjv.SelectedValue.ToString();
                //obj.name = "Sales Journal Voucher";
                //obj.type = "sjv";
                obj.Show();
            }
            if (name.Contains("GRN"))
            {
                string id = name;

                id = id.Substring(4);

                POSRestaurant.Reports.Inventory.frmReceivedInventory obj = new Inventory.frmReceivedInventory();
                obj.purchaseid = id;
                //POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                //obj.id = id;
                //obj.branch = cmbbranchjv.SelectedValue.ToString();
                //obj.name = "Sales Journal Voucher";
                //obj.type = "sjv";
                obj.Show();
            }

        }

        private void chkgroup_CheckedChanged(object sender, EventArgs e)
        {
            if (chkgroup.Checked == false)
            {
                chksum.Checked = true;
            }
        }

        private void chksum_CheckedChanged(object sender, EventArgs e)
        {
            if (chkgroup.Checked == false)
            {
                chksum.Checked = true;
            }
        }

        private void chkdebit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkdebit.Checked == true)
            {
                chkcredit.Checked = false;
            }
            else
            {
                chkcredit.Checked = true;
            }
        }

        private void chkcredit_CheckedChanged(object sender, EventArgs e)
        {
            if (chkcredit.Checked == true)
            {
                chkdebit.Checked = false;
            }
            else
            {
                chkdebit.Checked = true;
            }
        }

        private void cmbsubaccount_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cmbsubaccount_SelectedValueChanged(object sender, EventArgs e)
        {
            fillacount();
        }
    }
}
