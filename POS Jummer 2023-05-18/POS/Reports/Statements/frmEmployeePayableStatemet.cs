using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
namespace POSRestaurant.Reports.Statements
{
    public partial class frmEmployeePayableStatemet : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmEmployeePayableStatemet()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Employees where branchid='" + cmbbranchjv.SelectedValue + "' and status='Active' order by name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All Employees";
                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "name";
                cmbsupplier.Text = "All Employees";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public double bfr(string csid)
        {
            double bf = 0;
            try
            {
                string q = "";
                if (cmbsupplier.Text == "All Employees")
                {

                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         EmployeesAccount where  date <'" + dateTimePicker1.Text + "'  and EmployeeId='" + csid + "'";

                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         EmployeesAccount where EmployeeId='" + cmbsupplier.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'  ";

                }
                double debitbf = 0, creditbf = 0;
                DataSet ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string val = ds.Tables[0].Rows[0]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[0]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                bf = debitbf - creditbf;
            }
            catch (Exception ex)
            {

                MessageBox.Show("bf error");
            }
            return bf;
        }
        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
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
            try
            {
                DataSet ds = new System.Data.DataSet();
                string q = "SELECT     Id,  BranchName  FROM         Branch ";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);

                cmbbranchjv.DataSource = ds.Tables[0];
                cmbbranchjv.ValueMember = "id";
                cmbbranchjv.DisplayMember = "BranchName";
            }
            catch (Exception ex)
            {


            }
            fill();
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


                POSRestaurant.Reports.Statements.rptpayable rptDoc = new rptpayable();
                POSRestaurant.Reports.Statements.dspayablebank dsrpt = new dspayablebank();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                dt.DefaultView.Sort = "Date asc";
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
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    if (logo == "")
                    { }
                    else
                    {

                        dt.Rows.Add(dateTimePicker1.Text, "", "", "0", "0", "", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "For the period of " + Convert.ToDateTime(dateTimePicker1.Text).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(dateTimePicker2.Text).ToString("dd-MM-yyyy"));

                rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
                rptDoc.SetParameterValue("statement", "Employees Statement");
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
                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("VoucherNo", typeof(string));
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("Debit", typeof(double));
                dtrpt.Columns.Add("Credit", typeof(double));
                dtrpt.Columns.Add("Balance", typeof(string));
                dtrpt.Columns.Add("CheckNo", typeof(string));
                dtrpt.Columns.Add("AccountName", typeof(string));
                dtrpt.Columns.Add("date1", typeof(string));
                dtrpt.Columns.Add("logo", typeof(Byte[]));

                DataSet ds = new DataSet();
                string q = "";

                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                double bf = 0;
                if (cmbsupplier.Text == "All Employees")
                {
                    q = "SELECT DISTINCT dbo.EmployeesAccount.EmployeeId, dbo.Employees.Name FROM         dbo.EmployeesAccount INNER JOIN                      dbo.Employees ON dbo.EmployeesAccount.EmployeeId = dbo.Employees.Id where   dbo.Employees.status='Active' and dbo.Employees.branchid='" + cmbbranchjv.SelectedValue + "'";
                    DataSet dscs = new System.Data.DataSet();
                    dscs = objCore.funGetDataSet(q);
                    for (int l = 0; l < dscs.Tables[0].Rows.Count; l++)
                    {
                        bf = 0;
                        bf = bfr(dscs.Tables[0].Rows[l][0].ToString());
                        string brf = "";
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
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", dscs.Tables[0].Rows[l][1].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                            }
                            else
                            {
                                bf = System.Math.Abs(bf);
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", dscs.Tables[0].Rows[l][1].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                            }
                        }
                        else
                        {
                            if (bf >= 0)
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", dscs.Tables[0].Rows[l][1].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);


                            }
                            else
                            {
                                bf = System.Math.Abs(bf);
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", dscs.Tables[0].Rows[l][1].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);

                            }


                        }
                    }
                }
                else
                {
                    bf = bfr("");
                    string brf = "";
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
                            if (logo == "")
                            {

                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }
                        else
                        {
                            bf = System.Math.Abs(bf);
                            if (logo == "")
                            {

                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }
                    }
                    else
                    {
                        if (bf >= 0)
                        {
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                            }

                        }
                        else
                        {
                            bf = System.Math.Abs(bf);
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }


                    }
                }
                if (cmbsupplier.Text == "All Employees")
                {
                    q = "SELECT     dbo.EmployeesAccount.Balance AS CurrentBalance, dbo.EmployeesAccount.Credit, dbo.EmployeesAccount.Debit, dbo.EmployeesAccount.Description, dbo.EmployeesAccount.VoucherNo,                       dbo.EmployeesAccount.Date, dbo.Employees.Name AS accountname FROM         dbo.EmployeesAccount INNER JOIN                      dbo.Employees ON dbo.EmployeesAccount.EmployeeId = dbo.Employees.Id INNER JOIN                      dbo.ChartofAccounts ON dbo.EmployeesAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.Employees.status='Active' and  (EmployeesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.EmployeesAccount.branchid='" + cmbbranchjv.SelectedValue + "' order by dbo.EmployeesAccount.Date,dbo.EmployeesAccount.VoucherNo";

                }
                else
                {
                    q = "SELECT     dbo.EmployeesAccount.Balance AS CurrentBalance, dbo.EmployeesAccount.Credit, dbo.EmployeesAccount.Debit, dbo.EmployeesAccount.Description, dbo.EmployeesAccount.VoucherNo,                       dbo.EmployeesAccount.Date, dbo.Employees.Name AS accountname FROM         dbo.EmployeesAccount INNER JOIN                      dbo.Employees ON dbo.EmployeesAccount.EmployeeId = dbo.Employees.Id INNER JOIN                      dbo.ChartofAccounts ON dbo.EmployeesAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.Employees.status='Active' and  EmployeesAccount.EmployeeId='" + cmbsupplier.SelectedValue + "' and  (EmployeesAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.EmployeesAccount.branchid='" + cmbbranchjv.SelectedValue + "' order by dbo.EmployeesAccount.Date,dbo.EmployeesAccount.VoucherNo";

                }
                double balance1 = 0, debit1 = 0, credit1 = 0;
                ds = objCore.funGetDataSet(q);
                string debit = "";
                balance1 = bf;
                string credit = "", blnce = "", val = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    debit = ds.Tables[0].Rows[i]["Debit"].ToString();
                    credit = ds.Tables[0].Rows[i]["Credit"].ToString();
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debit1 = Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    credit1 = Convert.ToDouble(val);

                    balance1 = balance1 + (debit1 - credit1);

                    blnce = balance1.ToString();
                    if (blnce.Contains("-"))
                    {
                        blnce = "(" + blnce.Replace("-", "") + ")";
                    }

                    if (logo == "")
                    {

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);

                    }
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
            if (cmbsupplier.Text == "")
            {
                MessageBox.Show("Please Select Employee");
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
            fill();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbbranchjv.Text == "All")
            {
                MessageBox.Show("Please Select Branch");
                return;
            }
            DialogResult dr = MessageBox.Show("Are you sure to generate salaries of " + Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM-yy"), "", MessageBoxButtons.YesNo);
            if (dr == DialogResult.No)
            {
                return;
            }
            button2.Text = "Please Wait";
            generatesalaries();
            button2.Text = "Generate Salraries";

        }
        protected void generatesalaries()
        {
            try
            {
                string q = "select * from employees where status='Active' and branchid='"+cmbbranchjv.SelectedValue+"'";
                DataSet ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);
                q = "select * from CashSalesAccountsList where AccountType='Salaries Expense Account'";
                DataSet dsacountchk = new DataSet();
                dsacountchk = objCore.funGetDataSet(q);
                string ChartAccountId = "";
                if (dsacountchk.Tables[0].Rows.Count > 0)
                {
                    ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var lastDayOfMonth = new DateTime(Convert.ToDateTime(dateTimePicker2.Text).Year, Convert.ToDateTime(dateTimePicker2.Text).Month, DateTime.DaysInMonth(Convert.ToDateTime(dateTimePicker2.Text).Year, Convert.ToDateTime(dateTimePicker2.Text).Month));
                    string empid = ds.Tables[0].Rows[i]["id"].ToString();
                    string empname = ds.Tables[0].Rows[i]["name"].ToString();
                    string PayableAccountId = ds.Tables[0].Rows[0]["payableaccountid"].ToString();
                    q = "select top 1 Amount from EmployeesSalary where Empid='" + empid + "' order by date desc";
                    DataSet dssalary = new System.Data.DataSet();
                    dssalary = objCore.funGetDataSet(q);
                    if (dssalary.Tables[0].Rows.Count > 0)
                    {
                        string month=Convert.ToDateTime(dateTimePicker2.Text).ToString("yyyy-MM");
                        q = "select Days from EmployeeAttandance where  Empid='" + empid + "' and month='" + month + "'";
                        DataSet dsattandance = new System.Data.DataSet();
                        dsattandance = objCore.funGetDataSet(q);
                        if (dsattandance.Tables[0].Rows.Count > 0)
                        {
                            string temp = "";
                            float days = 0;
                            try
                            {
                                temp = dsattandance.Tables[0].Rows[0]["Days"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }

                            days = float.Parse(temp);
                            string q1 = "", q2 = "";
                            double amount = 0;

                            temp = dssalary.Tables[0].Rows[0][0].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            amount = Convert.ToDouble(temp);
                            amount = (amount) / 30;
                            amount = Math.Round(amount * days, 3);
                            try
                            {
                                DataSet dsacount = new DataSet();
                                int iddd = 0;
                                dsacount = objCore.funGetDataSet("select max(id) as id from EmployeesAccount");
                                if (dsacount.Tables[0].Rows.Count > 0)
                                {
                                    string ii = dsacount.Tables[0].Rows[0][0].ToString();
                                    if (ii == string.Empty)
                                    {
                                        ii = "0";
                                    }
                                    iddd = Convert.ToInt32(ii) + 1;
                                }
                                else
                                {
                                    iddd = 1;
                                }
                                q1 = "insert into EmployeesAccount (Id,Date,EmployeeId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + lastDayOfMonth + "','" + empid + "','" + PayableAccountId + "','SPV-" + empid + "-" + Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM-yy") + "','" + days + " Days Salary of  " + Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM-yy") + " Generated','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + cmbbranchjv.SelectedValue + "')";
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                DataSet dsacount = new DataSet();
                                int iddd = 0;
                                dsacount = objCore.funGetDataSet("select max(id) as id from SalariesAccount");
                                if (dsacount.Tables[0].Rows.Count > 0)
                                {
                                    string ii = dsacount.Tables[0].Rows[0][0].ToString();
                                    if (ii == string.Empty)
                                    {
                                        ii = "0";
                                    }
                                    iddd = Convert.ToInt32(ii) + 1;
                                }
                                else
                                {
                                    iddd = 1;
                                }
                                q2 = "insert into SalariesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + lastDayOfMonth + "','" + ChartAccountId + "','SPV-" + empid + "-" + Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM-yy") + "','"+days +" Days Salary of  " + empname + " for " + Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM-yy") + " Generated','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','0','" + cmbbranchjv.SelectedValue + "')";
                            }
                            catch (Exception ex)
                            {
                            }
                            q = "delete from EmployeesAccount where voucherno='SPV-" + empid + "-" + Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM-yy") + "'";
                            objCore.executeQuery(q);
                            q = "delete from SalariesAccount where voucherno='SPV-" + empid + "-" + Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM-yy") + "'";
                            objCore.executeQuery(q);
                            ExecuteSqlTransaction("", q1, q2, "");
                        }
                    }
                }
                MessageBox.Show("Generated Successfully");
                bindreport();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private static void ExecuteSqlTransaction(string connectionString, string q1, string q2, string message)
        {
            connectionString = POSRestaurant.Properties.Settings.Default.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    if (q1.Length > 0)
                    {
                        command.CommandText = q1;
                        command.ExecuteNonQuery();
                    }
                    if (q2.Length > 0)
                    {
                        command.CommandText = q2;
                        command.ExecuteNonQuery();
                    }
                    // Attempt to commit the transaction.
                    transaction.Commit();
                    //MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("  Message: {0}" + ex.Message);
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Rollback Exception Type: {0}" + ex2.Message);
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmSalaryslip obj = new frmSalaryslip();
            obj.Show();
        }
    }
}
