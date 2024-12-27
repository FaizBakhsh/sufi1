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
using System.IO;
namespace POSRestaurant.Accounts
{
    public partial class Vouchers : Form
    {
        DataTable dt;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string acid = "";
        string min = "";
        string max = "";
        public Vouchers()
        {
            InitializeComponent();
            GetVoucherNo();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void getdata(string keyword, string show)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno, Description, Debit, Credit FROM         CashAccountPaymentSupplier where branchid='" + cmbbranch.SelectedValue + "' and (Date between '" + min + "' and '" + max + "')  order by id asc";
            }


            if (show == "yes")
            {
                Open obj = new Open(this);
                obj.q = q;
                obj.type = "cpv";
                obj.branch = cmbbranch.SelectedValue.ToString();
                obj.Show();
            }
            //ds = objCore.funGetDataSet(q);
            //dataGridView1.DataSource = ds.Tables[0];
            //dataGridView1.Columns[0].Visible = false;
        }
        public void getdatabank(string keyword, string show)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno,CheckNo, CheckDate, Description, Debit, Credit FROM         BankAccountPaymentSupplier where branchid='" + cmbbranchbpv.SelectedValue + "' and (Date between '" + min + "' and '" + max + "') order by id asc";
            }

            if (show == "yes")
            {
                Open obj = new Open(this);
                obj.q = q;
                obj.type = "bpv";
                obj.branch = cmbbranchbpv.SelectedValue.ToString();
                obj.Show();
            }
            //ds = objCore.funGetDataSet(q);
            //dataGridView4.DataSource = ds.Tables[0];
            //dataGridView4.Columns[0].Visible = false;
        }
        public void getdatareceipt(string keyword, string show)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno, Description, Debit, Credit FROM         CashAccountReceiptCustomer  where branchid='" + cmbbranchcrv.SelectedValue + "' and (Date between '" + min + "' and '" + max + "') order by id asc";
            }

            if (show == "yes")
            {
                Open obj = new Open(this);
                obj.q = q;
                obj.type = "crv";
                obj.branch = cmbbranchcrv.SelectedValue.ToString();
                obj.Show();
            }
            ds = objCore.funGetDataSet(q);
            //dataGridView2.DataSource = ds.Tables[0];
            //dataGridView2.Columns[0].Visible = false;
        }
        public void getdatareceiptbank(string keyword, string show)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno, CheckNo, CheckDate, Description, Debit, Credit FROM         BankAccountReceiptCustomer where branchid='" + cmbbranchbrv.SelectedValue + "' and (Date between '" + min + "' and '" + max + "')  order by id asc";
            }

            if (show == "yes")
            {
                Open obj = new Open(this);
                obj.q = q;
                obj.type = "brv";
                obj.branch = cmbbranchbrv.SelectedValue.ToString();
                obj.Show();
            }
            //ds = objCore.funGetDataSet(q);
            //dataGridView3.DataSource = ds.Tables[0];
            //dataGridView3.Columns[0].Visible = false;
        }
        public void getdatajournal(string keyword, string show)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     dbo.JournalAccount.Id, dbo.JournalAccount.Date, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit                        FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.branchid='" + cmbbranchjv.SelectedValue + "' and (dbo.JournalAccount.Date between '" + min + "' and '" + max + "') order by dbo.JournalAccount.Date,JournalAccount.VoucherNo ";
            }

            if (show == "yes")
            {
                Open obj = new Open(this);
                obj.q = q;
                obj.type = "jv";
                obj.branch = cmbbranchjv.SelectedValue.ToString();
                obj.Show();
            }
            ds = objCore.funGetDataSet(q);
            //dataGridView5.DataSource = ds.Tables[0];
            //dataGridView5.Columns[0].Visible = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            getdata("", "yes");
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void refereshbrv()
        {
            try
            {
                string q = "";
                if (rdcustomers.Checked == true)
                {
                    q = "SELECT     Id,  Name  FROM         Customers  order by Name";
                }
                if (rdglbrv.Checked == true)
                {
                    q = "SELECT     Id,  Name  FROM         ChartofAccounts where  status='Active' order by Name";
                }
                DataSet ds6 = new System.Data.DataSet();
                ds6 = objCore.funGetDataSet(q);


                cmbcustomers2.DataSource = ds6.Tables[0];
                cmbcustomers2.ValueMember = "id";
                cmbcustomers2.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }
        }
        public void refereshcrv()
        {
            try
            {
                string q = "";

                if (rdcustomers1.Checked == true)
                {
                    q = "SELECT     Id,  Name  FROM         Customers  order by Name";


                }
                if (rdglcrv.Checked == true)
                {
                    q = "SELECT     Id,  Name  FROM         ChartofAccounts where  status='Active' order by Name";
                }
                DataSet ds5 = new System.Data.DataSet();
                ds5 = objCore.funGetDataSet(q);

                cmbcustomers.DataSource = ds5.Tables[0];
                cmbcustomers.ValueMember = "id";
                cmbcustomers.DisplayMember = "Name";

            }
            catch (Exception ex)
            {


            }
        }
        public void refereshbpv()
        {
            try
            {
                DataSet dsbalance = new System.Data.DataSet();
                string q = "";
                if (rdemployeesbpv.Checked == true)
                {
                    q = "select * from Employees where branchid='" + cmbbranchbpv.SelectedValue + "' order by Name";

                }
                if (rdsupplier.Checked == true)
                {
                    q = "select * from supplier order by Name";
                    
                }
                if (rdgl.Checked == true)
                {
                    q = "SELECT     Id,  Name  FROM         ChartofAccounts where status='Active' order by Name";
                }
                DataSet dss1 = objCore.funGetDataSet(q);
                cmbsupplier2.DataSource = dss1.Tables[0];
                cmbsupplier2.ValueMember = "id";
                cmbsupplier2.DisplayMember = "Name";
                if (rdsupplier.Checked == true)
                {
                    getinvoices(cmbsupplier2.SelectedValue.ToString(), "BPV");
                  //double blnc=  getsuplierbalance(cmbsupplier2.SelectedValue.ToString(),cmbbranchbpv.SelectedValue.ToString());
                  //lblsuplierbalancebpv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
                if (rdemployeesbpv.Checked == true)
                {
                    double blnc = getemployeesbalance(cmbsupplier2.SelectedValue.ToString(), cmbbranchbpv.SelectedValue.ToString());
                    lblsuplierbalancebpv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
            }
            catch (Exception ex)
            {


            }
        }
        protected double getsuplierbalance(string id,string invoice)
        {
            double balance = 0;
            
            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         SupplierAccount where invoiceno='" + invoice + "' and SupplierId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp= dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return balance;
        }
        protected double getcustomerbalance(string id, string branchid)
        {
            double balance = 0;

            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         CustomerAccount where branchid='" + branchid + "' and CustomersId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp = dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return balance;
        }
        protected double getemployeesbalance(string id, string branchid)
        {
            double balance = 0;

            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         EmployeesAccount where branchid='" + branchid + "' and EmployeeId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp = dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return balance;
        }
        protected double getcashbalance(string id, string branchid)
        {
            double balance = 0;

            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         CashAccountPaymentSupplier where branchid='" + branchid + "' and ChartAccountId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp = dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         CashAccountReceiptCustomer where branchid='" + branchid + "' and ChartAccountId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp = dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = balance + Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         JournalAccount where branchid='" + branchid + "' and PayableAccountId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp = dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = balance + Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         BankAccountPaymentSupplier where branchid='" + branchid + "' and ChartAccountId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp = dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = balance + Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                string q = "SELECT     (sum(debit) - sum(credit)) as balance  FROM         BankAccountReceiptCustomer where branchid='" + branchid + "' and ChartAccountId='" + id + "'";

                DataSet dss1 = new System.Data.DataSet();
                dss1 = objCore.funGetDataSet(q);
                if (dss1.Tables[0].Rows.Count > 0)
                {
                    string temp = dss1.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    balance = balance + Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return balance;
        }
        protected void getbalances(string supplierid,string invoiceno,string type)
        {
            try
            {
               
                
                    double blnc = getsuplierbalance(supplierid.ToString(), invoiceno.ToString());
                    if (type == "CPV")
                    {
                        lblbalancecpvsupplier.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                    }
                    if (type == "BPV")
                    {
                        lblsuplierbalancebpv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                    }
            }
            catch (Exception ex)
            {


            }
        }
        protected void getinvoices(string supplierid,string type)
        {
            try
            {
                string q = "";
                cmbinvoicenocpv.DataSource = null;
                cmbinvoicebpv.DataSource = null;
                q = "select cast(id as varchar(100))+'-'+invoiceno as invoiceno from Purchase where paymentstatus='Pending' and SupplierId='" + supplierid + "'";
                q = "SELECT     (sum(debit) - sum(credit)) as balance ,invoiceno,SupplierId FROM         SupplierAccount  group by invoiceno ,SupplierId having  (sum(debit) - sum(credit))<0  and SupplierId='" + supplierid + "' ";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (type == "CPV")
                {
                    cmbinvoicenocpv.DataSource = dss.Tables[0];
                    cmbinvoicenocpv.ValueMember = "invoiceno";
                    cmbinvoicenocpv.DisplayMember = "invoiceno";
                    getbalances(supplierid, cmbinvoicenocpv.SelectedValue.ToString(),type);
                }
                if (type == "BPV")
                {
                    cmbinvoicebpv.DataSource = dss.Tables[0];
                    cmbinvoicebpv.ValueMember = "invoiceno";
                    cmbinvoicebpv.DisplayMember = "invoiceno";
                    getbalances(supplierid, cmbinvoicebpv.SelectedValue.ToString(),type);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void refereshcpv()
        {
            try
            {
                string q = "";
                if (rdemployees.Checked == true)
                {
                    q = "select * from Employees  order by Name";
                }
                if (rdsupplier1.Checked == true)
                {
                    q = "select * from supplier  order by Name";
                }
                if (rdglcpv.Checked == true)
                {
                    q = "SELECT     Id,  Name  FROM         ChartofAccounts where status='Active' order by Name";
                }
                DataSet dss = objCore.funGetDataSet(q);

                cmbsupplier.DataSource = dss.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "Name";
                if (rdsupplier1.Checked == true)
                {
                    getinvoices(cmbsupplier.SelectedValue.ToString(),"CPV");
                    //double blnc = getsuplierbalance(cmbsupplier.SelectedValue.ToString(), cmbbranch.SelectedValue.ToString());
                    //lblbalancecpvsupplier.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
                if (rdemployees.Checked == true)
                {
                    double blnc = getemployeesbalance(cmbsupplier.SelectedValue.ToString(), cmbbranch.SelectedValue.ToString());
                    lblbalancecpvsupplier.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void refreshaccounts()
        {
            string q = "";
            try
            {
                q = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets') and name like '%Cash%'  order by name";
                DataSet ds1 = new System.Data.DataSet();
                ds1 = objCore.funGetDataSet(q);

                cmbcashaccount.DataSource = ds1.Tables[0];
                cmbcashaccount.ValueMember = "id";
                cmbcashaccount.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }

            try
            {
                q = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets')  and name like '%Cash%'  order by name";
                DataSet ds2 = new System.Data.DataSet();
                ds2 = objCore.funGetDataSet(q);

                cmbcashaccount2.DataSource = ds2.Tables[0];
                cmbcashaccount2.ValueMember = "id";
                cmbcashaccount2.DisplayMember = "Name";

               
            }
            catch (Exception ex)
            {


            }

            try
            {
                q = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets')  and name like '%Bank%'   order by name";
                DataSet ds3 = new System.Data.DataSet();
                ds3 = objCore.funGetDataSet(q);

                cmbcashaccount3.DataSource = ds3.Tables[0];
                cmbcashaccount3.ValueMember = "id";
                cmbcashaccount3.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }

            try
            {
                q = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets')  and name like '%Bank%'   order by name";
                DataSet ds4 = new System.Data.DataSet();
                ds4 = objCore.funGetDataSet(q);

                cmbcashaccount4.DataSource = ds4.Tables[0];
                cmbcashaccount4.ValueMember = "id";
                cmbcashaccount4.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }


            refereshcrv();
            refereshbrv();
            refereshcpv();
            refereshbpv();
            refereshjvaccount();



        }
        public void refereshjvaccount()
        {
            try
            {
                string q = "";

                if (cmbtype.Text == "Employees")
                {
                    q = "SELECT     Id,  Name  FROM         Employees WHERE Branchid='" + cmbbranchjv.SelectedValue + "' order by name";
                }
                if (cmbtype.Text == "GL Accounts")
                {
                    q = "SELECT     Id,  Name  FROM         ChartofAccounts  order by name";
                }
                if (cmbtype.Text == "Payable Accounts")
                {
                    q = "SELECT     Id,  Name  FROM         Supplier  order by name";
                }
                if (cmbtype.Text == "Receiveable Accounts")
                {
                    q = "SELECT     Id,  Name  FROM         Customers  order by name";
                }
                DataSet ds6 = new System.Data.DataSet();
                ds6 = objCore.funGetDataSet(q);
                cmbaccount.DataSource = ds6.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "Name";

            }
            catch (Exception ex)
            {


            }
        }
        private void Vouchers_Load(object sender, EventArgs e)
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
            //try
            //{
            //    dateTimePicker3.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
            //    dateTimePicker3.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
            //}
            //catch (Exception ex)
            //{


            //}
            //try
            //{
            //    dateTimePicker4.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
            //    dateTimePicker4.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
            //}
            //catch (Exception ex)
            //{


            //}
            //try
            //{
            //    dateTimePicker5.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
            //    dateTimePicker5.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
            //}
            //catch (Exception ex)
            //{


            //}

            try
            {
                dt = new DataTable();
                dt.Columns.Add("id", typeof(string));
                dt.Columns.Add("Account Type", typeof(string));
                dt.Columns.Add("pid", typeof(string));
                dt.Columns.Add("Account Name", typeof(string));
                dt.Columns.Add("Voucher No", typeof(string));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("Debit", typeof(double));
                dt.Columns.Add("Credit", typeof(double));
                dt.Columns.Add("Customer/Supplier Name", typeof(string));
                dt.Columns.Add("BranchID", typeof(string));


                string q = "select * from supplier";

                DataSet ds = new System.Data.DataSet();
                q = "SELECT     Id,  BranchName  FROM         Branch order by BranchName";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);

                cmbbranch.DataSource = ds.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "BranchName";

                cmbbranchbpv.DataSource = ds.Tables[0];
                cmbbranchbpv.ValueMember = "id";
                cmbbranchbpv.DisplayMember = "BranchName";
                cmbbranchbrv.DataSource = ds.Tables[0];
                cmbbranchbrv.ValueMember = "id";
                cmbbranchbrv.DisplayMember = "BranchName";
                cmbbranchcrv.DataSource = ds.Tables[0];
                cmbbranchcrv.ValueMember = "id";
                cmbbranchcrv.DisplayMember = "BranchName";
                cmbbranchjv.DataSource = ds.Tables[0];
                cmbbranchjv.ValueMember = "id";
                cmbbranchjv.DisplayMember = "BranchName";



                refreshaccounts();


                getdata("", "");
                getdatabank("", "");
                getdatareceipt("", "");
                getdatareceiptbank("", "");
                getdatajournal("", "");
            }
            catch (Exception ex)
            {

                MessageBox.Show("form load error");
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
                    command.CommandText = q1;
                    command.ExecuteNonQuery();
                    command.CommandText = q2;
                    command.ExecuteNonQuery();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                    MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("  Message: {0}" + ex.Message);

                    // Attempt to roll back the transaction. 
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
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbsupplier.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Supplier");
                        return;
                    }
                    if (txtamount.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount.Focus();
                            return;
                        }
                    }
                    if (txtdesc.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button1.Text == "Save")
                {
                    if (txtvoucherno.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from CashAccountPaymentSupplier");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            idd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            idd = 1;
                        }
                        string q = "select top 1 * from CashAccountPaymentSupplier  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance - Convert.ToDouble(txtamount.Text));
                        newbalance = Math.Round(newbalance, 2);
                      //  if (rdsupplier1.Checked == true || rdglcpv.Checked == true)
                        if (rdsupplier1.Checked == true)
                        {

                            q = "insert into CashAccountPaymentSupplier (id,Date,ChartAccountId,SupplierId,Voucherno,Description,Debit,Credit,CurrentBalance,branchid) values('" + idd + "','" + dateTimePicker1.Text + "','" + cmbcashaccount.SelectedValue + "','" + cmbsupplier.SelectedValue + "','CPV-" + idd + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','0','" + txtamount.Text + "','" + newbalance + "','" + cmbbranch.SelectedValue + "')";
                        }

                        else if (rdglcpv.Checked == true || rdemployees.Checked == true)
                        {
                            q = "insert into CashAccountPaymentSupplier (id,Date,ChartAccountId,SupplierId,Voucherno,Description,Debit,Credit,CurrentBalance,branchid) values('" + idd + "','" + dateTimePicker1.Text + "','" + cmbcashaccount.SelectedValue + "','0','CPV-" + idd + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','0','" + txtamount.Text + "','" + newbalance + "','" + cmbbranch.SelectedValue + "')";
                        }
                        //objCore.executeQuery(q);
                        txtvoucherno.Text = "CPV-" + idd;
                        string q2 = supplieraccount(txtamount.Text, "");
                        ExecuteSqlTransaction("", q, q2, "Data Added Successfully");
                        savesupporting("CashAccountPaymentSupplier", txtvoucherno.Text);
                        //MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button1.Text == "Update")
                {

                    POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                    POSRestaurant.Properties.Settings.Default.Save();
                    string status = objCore.authenticate("update");
                    if (status == "no")
                    {
                        POSRestaurant.classes.Message obj = new classes.Message();
                        obj.Show();
                        return;
                    }


                    string q = "select  * from CashAccountPaymentSupplier where voucherno='" + txtvoucherno.Text + "' ";//where ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();
                                if (rdsupplier1.Checked == true)
                                {
                                    q = "update CashAccountPaymentSupplier set status='Pending', branchid='" + cmbbranch.SelectedValue + "', Date='" + dateTimePicker1.Text + "',ChartAccountId='" + cmbcashaccount.SelectedValue + "',SupplierId='" + cmbsupplier.SelectedValue + "', Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount.Text + "'  where id='" + acid + "'";

                                }
                                if (rdglcpv.Checked == true || rdemployees.Checked == true)
                                {
                                    q = "update CashAccountPaymentSupplier set status='Pending', branchid='" + cmbbranch.SelectedValue + "', Date='" + dateTimePicker1.Text + "',ChartAccountId='" + cmbcashaccount.SelectedValue + "',SupplierId='0', Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount.Text + "'  where id='" + acid + "'";

                                }
                                //updatecashaccountfirst(val);
                                //return;
                                chk = true;
                            }

                        }

                    }
                    string qry = "delete from EmployeesAccount where voucherno='" + txtvoucherno.Text + "' ";
                    objCore.executeQuery(qry);
                    qry = "delete from SupplierAccount where voucherno='" + txtvoucherno.Text + "' ";
                    objCore.executeQuery(qry);
                    string q2 = supplieraccount(txtamount.Text, txtvoucherno.Text);
                    ExecuteSqlTransaction("", q, q2, "Data Updated Successfully");
                    savesupporting("CashAccountPaymentSupplier", txtvoucherno.Text);
                    //MessageBox.Show("Data Updated Successfully");
                }
            }
            catch (Exception ex)
            {


            }

             clear();
            //getdata("", "");
        }
        public void savesupporting(string tabel,string voucherno)
        {
            try
            {
                string q = "update " + tabel + " set  supporting=@supporting  where voucherno='" + voucherno + "'";
                string c = objCore.getConnectionString();
                SqlConnection con = new SqlConnection(c);
                SqlCommand SqlCom = new SqlCommand(q, con);

                SqlCom.Parameters.Add(new SqlParameter("@supporting", (object)imageData));

                //Open connection and execute insert query.
                con.Open();
                SqlCom.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {

              //  MessageBox.Show("Failed to save supporting...\n"+ex.Message);
            }
        }
        public void updatecashaccountfirst(string val)
        {
            try
            {
                //string q = "select * from CashAccountPaymentSupplier where id < '" + val + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                //}
                //double balance = Convert.ToDouble(blnce);

                //double newbalance = (balance - Convert.ToDouble(txtamount.Text));
                //newbalance = Math.Round(newbalance, 2);
                string q = "";
                objCore = new classes.Clsdbcon();
                if (rdsupplier1.Checked == true)
                {
                    q = "update CashAccountPaymentSupplier set Date='" + dateTimePicker1.Text + "',ChartAccountId='" + cmbcashaccount.SelectedValue + "',SupplierId='" + cmbsupplier.SelectedValue + "', Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount.Text + "'  where id='" + acid + "'";

                }
                if (rdglcpv.Checked == true)
                {
                    q = "update CashAccountPaymentSupplier set Date='" + dateTimePicker1.Text + "',ChartAccountId='" + cmbcashaccount.SelectedValue + "',SupplierId='0', Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount.Text + "'  where id='" + acid + "'";

                }
                objCore.executeQuery(q);
                //updatecashaccount(val);

                ////new--------------------------------

                //string q = "";
                ////DataSet dss = new System.Data.DataSet();
                ////dss = objCore.funGetDataSet(q);
                ////string blnce = "0";
                ////if (dss.Tables[0].Rows.Count > 0)
                ////{
                ////    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                ////}
                ////double balance = Convert.ToDouble(blnce);

                ////double newbalance = (balance - Convert.ToDouble(txtamount.Text));
                ////newbalance = Math.Round(newbalance, 2);
                //objCore = new classes.Clsdbcon();
                //q = "update CashAccountPaymentSupplier set Date='" + dateTimePicker1.Text + "',ChartAccountId='" + cmbcashaccount.SelectedValue + "',SupplierId='" + cmbsupplier.SelectedValue + "', Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount.Text + "'  where id='" + val + "'";
                //objCore.executeQuery(q);
                //updatecashaccount2(cmbcashaccount.SelectedValue.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account first error");
            }
        }
        public void updatecashaccount2(string val)
        {
            try
            {
                string q = "select * from CashAccountPaymentSupplier where ChartAccountId ='" + val + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }

                        newbalance = Convert.ToDouble(blnce);
                    }
                    else
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }
                        newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit);

                    }
                    newbalance = Math.Round(newbalance, 2);
                    objCore = new classes.Clsdbcon();
                    q = "update CashAccountPaymentSupplier set     Debit='" + debit + "' ,credit='" + credit + "' , CurrentBalance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() + "'";
                    objCore.executeQuery(q);

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account  error");
            }
        }
        public void updatecashaccount(string val)
        {

            try
            {
                string q = "select * from CashAccountPaymentSupplier where id >='" + val + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account error");
            }
        }
        public void updatecashaccountremaining(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CashAccountPaymentSupplier set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatecashaccount(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletecashaccount(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account remaining error");
            }
        }
        public void deletecashaccountfirst(int id)
        {
            try
            {
                string q = "";// "select * from CashAccountPaymentSupplier where id <'" + id + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update CashAccountPaymentSupplier set  Credit='0' where voucherno='" + txtvoucherno.Text + "'";
                objCore.executeQuery(q);
                // deletecashaccount(id);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account first error");
            }
        }
        public void deletecashaccount(int id)
        {

            try
            {
                string q = "select * from CashAccountPaymentSupplier where id >='" + id + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account error");
            }
            getdata("", "");
        }


        public void updatecashaccountfirstbank(string val)
        {
            try
            {
                string q = "select * from BankAccountPaymentSupplier where id < '" + val + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id desc";

                objCore = new classes.Clsdbcon();
                if (rdgl.Checked == true)
                {
                    q = "update BankAccountPaymentSupplier set Date='" + dateTimePicker4.Text + "',ChartAccountId='" + cmbcashaccount4.SelectedValue + "',SupplierId='0', Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount4.Text + "' where id='" + val + "'";

                }
                if (rdgl.Checked == true)
                {
                    q = "update BankAccountPaymentSupplier set Date='" + dateTimePicker4.Text + "',ChartAccountId='" + cmbcashaccount4.SelectedValue + "',SupplierId='" + cmbsupplier2.SelectedValue + "', Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount4.Text + "' where id='" + val + "'";
                }
                objCore.executeQuery(q);
                //updatecashaccountbank(val);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash bank first  error");
            }
        }
        public void updatecashaccountbank(string val)
        {

            try
            {
                string q = "select * from BankAccountPaymentSupplier where id >='" + val + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account  error");
            }
        }
        public void updatecashaccountremainingbank(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update BankAccountPaymentSupplier set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatecashaccountbank(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletecashaccountbank(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account remaining error");
            }
        }
        public void deletecashaccountfirstbank(int id)
        {
            try
            {
                string q = "";// "select * from BankAccountPaymentSupplier where id <'" + id + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update BankAccountPaymentSupplier set  Credit='0' where voucherno='" + txtvoucherno4.Text + "'";
                objCore.executeQuery(q);
                //deletecashaccountbank(id);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete bank account first error");
            }
        }
        public void deletecashaccountbank(int id)
        {

            try
            {
                string q = "select * from BankAccountPaymentSupplier where id >='" + id + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete bank account error");
            }
            getdatabank("", "");
        }


        public string supplieraccount(string amount, string id)
        {
            string q = "";
            try
            {
                DataSet dsacount = new DataSet();
                q = "select payableaccountid from Supplier where id='" + cmbsupplier.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                //
                {
                    string PayableAccountId = "";
                    try
                    {
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            if (rdsupplier1.Checked == true)
                            {
                                PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    int iddd = 0;
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    if (rdemployees.Checked == true)
                    {
                        q = "select payableaccountid from Employees where id='" + cmbsupplier.SelectedValue + "' ";
                        dsacount = new System.Data.DataSet();
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                        }
                        dsa = objCore.funGetDataSet("select max(id) as id from EmployeesAccount");
                        if (dsa.Tables[0].Rows.Count > 0)
                        {
                            string i = dsa.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            iddd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            iddd = 1;
                        }
                    }

                   
                    double balance = 0;
                    string val = "";
                    string salarymonth = "";

                    salarymonth = dateTimePickeremployeecpv.Text;

                    double newbalance = 0;
                    newbalance = Math.Round(newbalance, 2);
                    if (rdemployees.Checked == true)
                    {
                        q = "insert into EmployeesAccount (SalaryMonth,Id,Date,EmployeeId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + salarymonth + "','" + iddd + "','" + dateTimePicker1.Text + "','" + cmbsupplier.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno.Text + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + cmbbranch.SelectedValue + "')";

                    }
                    if (rdsupplier1.Checked == true)
                    {
                        string invoiceno = "";

                        try
                        {
                            invoiceno = cmbinvoicenocpv.SelectedValue.ToString();
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        q = "insert into SupplierAccount (invoiceno,Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + invoiceno + "','" + iddd + "','" + dateTimePicker1.Text + "','" + cmbsupplier.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno.Text + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + cmbbranch.SelectedValue + "')";

                    }
                    if (rdglcpv.Checked == true)
                    {
                        q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + dateTimePicker1.Text + "','0','" + cmbsupplier.SelectedValue + "','" + txtvoucherno.Text + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + cmbbranch.SelectedValue + "')";

                    }
                    //if (button1.Text == "Save")
                    //{
                        

                    //    // objCore.executeQuery(q);
                    //}
                    //if (button1.Text == "Update")
                    //{
                    //    DataSet dsval = new System.Data.DataSet();
                    //    if (rdemployees.Checked == true)
                    //    {
                    //        dsval = new System.Data.DataSet();
                    //        q = "select id from EmployeesAccount where VoucherNo='" + txtvoucherno.Text + "'";
                    //        dsval = objCore.funGetDataSet(q);
                    //        if (dsval.Tables[0].Rows.Count > 0)
                    //        {
                    //            q = "update EmployeesAccount set EmployeeId='" + cmbsupplier.SelectedValue + "' ,Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',  PayableAccountId='" + PayableAccountId + "', branchid='" + cmbbranch.SelectedValue + "', Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "'  where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";

                    //        }
                    //    }
                    //    else
                    //    {
                    //        q = "select id from SupplierAccount where VoucherNo='" + txtvoucherno.Text + "'";
                    //        dsval = objCore.funGetDataSet(q);
                    //        if (dsval.Tables[0].Rows.Count > 0)
                    //        {

                    //            if (rdsupplier1.Checked == true)
                    //            {

                    //                q = "update SupplierAccount set SupplierId='" + cmbsupplier.SelectedValue + "' ,Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',  PayableAccountId='" + PayableAccountId + "', branchid='" + cmbbranch.SelectedValue + "', Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "'  where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";

                    //            }
                    //            if (rdglcpv.Checked == true)
                    //            {

                    //                q = "update SupplierAccount set SupplierId='0',  PayableAccountId='" + cmbsupplier.SelectedValue + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', branchid='" + cmbbranch.SelectedValue + "', Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "'  where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";

                    //            }
                    //            //updatesupplieraccountfirst(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                    //        }
                    //    }
                       

                    //    //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    //}

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("supplier account error");
            }
            return q;
        }
        public void updatesupplieraccountfirst(string val, string payaccount)
        {
            try
            {
                //string q = "select * from SupplierAccount where id < '" + val + "'  and SupplierId='" + cmbsupplier.SelectedValue + "' and PayableAccountId='" + payaccount + "' order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                //double balance = Convert.ToDouble(blnce);

                //double newbalance = (balance + Convert.ToDouble(txtamount.Text));
                //newbalance = Math.Round(newbalance, 2);
                string q = "";
                objCore = new classes.Clsdbcon();
                if (rdsupplier1.Checked == true)
                {

                    q = "update SupplierAccount set SupplierId='" + cmbsupplier.SelectedValue + "',  PayableAccountId='" + payaccount + "', branchid='" + cmbbranch.SelectedValue + "', Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "'  where id='" + val + "'";

                }
                if (rdglcpv.Checked == true)
                {

                    q = "update SupplierAccount set SupplierId='0',  PayableAccountId='" + cmbsupplier.SelectedValue + "', branchid='" + cmbbranch.SelectedValue + "', Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "'  where id='" + val + "'";

                }
                //objCore.executeQuery(q);
                // updatesupplieraccount(val, payaccount);

                //new-------

                //string q = "";

                //objCore = new classes.Clsdbcon();
                //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "' where id='" + val + "'";
                //objCore.executeQuery(q);
                //updatesupplieraccount2(payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier first account error");
            }
        }
        public void updatesupplieraccount2(string val)
        {
            try
            {
                string q = "select * from SupplierAccount where PayableAccountId ='" + val + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }

                        newbalance = Convert.ToDouble(blnce);
                    }
                    else
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }
                        newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit);

                    }
                    newbalance = Math.Round(newbalance, 2);
                    objCore = new classes.Clsdbcon();
                    q = "update SupplierAccount set     Debit='" + debit + "' ,credit='" + credit + "' , CurrentBalance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() + "'";
                    objCore.executeQuery(q);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account  error");
            }
        }
        public void updatesupplieraccount(string val, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + val + "' and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier account error");
            }
        }
        public void updatesupplieraccountremaining(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update SupplierAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatesupplieraccount(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deletesupplieraccount(count, payaccount);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier remaining account error");
            }
            return;
        }
        public void deletesupplieraccountfirst(int id, string payaccount)
        {
            try
            {
                string q = "";// "select * from SupplierAccount where id <'" + id + "' and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier.SelectedValue + "' order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update SupplierAccount set  Debit='0' where voucherno='" + txtvoucherno.Text + "'";
                objCore.executeQuery(q);
                //deletesupplieraccount(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier first account error");
            }
        }
        public void deletesupplieraccount(int id, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + id + "' and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier account error");
            }
            getdata("", "");
        }


        public string supplieraccountBank(string amount, string id)
        {
            string q = "";
            try
            {
                int iddd = 0;
                DataSet dsacount = new DataSet();
                q = "select payableaccountid from Supplier where id='" + cmbsupplier2.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                //
                {
                    string PayableAccountId = "";
                    try
                    {
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            if (rdsupplier.Checked == true)
                            {
                                PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    if (rdemployeesbpv.Checked == true)
                    {
                        q = "select payableaccountid from Employees where id='" + cmbsupplier2.SelectedValue + "' ";
                        dsacount = new System.Data.DataSet();
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                        }
                        dsa = objCore.funGetDataSet("select max(id) as id from EmployeesAccount");
                        if (dsa.Tables[0].Rows.Count > 0)
                        {
                            string i = dsa.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            iddd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            iddd = 1;
                        }
                    }
                  
                   
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from SupplierAccount where SupplierId='" + cmbsupplier2.SelectedValue + "' and PayableAccountId='" + PayableAccountId + "'  order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);
                    if (rdemployeesbpv.Checked == true)
                    {
                        q = "insert into EmployeesAccount (SalaryMonth,Id,Date,EmployeeId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + dateTimePickeremployeebpv.Text + "','" + iddd + "','" + dateTimePicker4.Text + "','" + cmbsupplier2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','Bank','" + cmbbranchbpv.SelectedValue + "')";

                    }
                    if (rdsupplier.Checked == true)
                    {
                        string invoiceno = "";

                        try
                        {
                            invoiceno = cmbinvoicebpv.SelectedValue.ToString();
                        }
                        catch (Exception ex)
                        {

                        }
                        q = "insert into SupplierAccount (invoiceno,Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + invoiceno + "','" + iddd + "','" + dateTimePicker4.Text + "','" + cmbsupplier2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','Bank','" + cmbbranchbpv.SelectedValue + "')";
                        //objCore.executeQuery(q);
                    }
                    if (rdgl.Checked == true)
                    {
                        q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + iddd + "','" + dateTimePicker4.Text + "','0','" + cmbsupplier2.SelectedValue + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','Bank','" + cmbbranchbpv.SelectedValue + "')";
                        // objCore.executeQuery(q);
                    }

                    //if (button21.Text == "Save")
                    //{
                       
                    //}
                    //if (button21.Text == "Update")
                    //{
                    //    DataSet dsval = new System.Data.DataSet();
                    //    q = "select id from SupplierAccount where VoucherNo='" + txtvoucherno4.Text + "'";
                    //    dsval = objCore.funGetDataSet(q);
                    //    if (dsval.Tables[0].Rows.Count > 0)
                    //    {
                    //        //updatesupplieraccountfirstbank(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                    //        if (rdemployeesbpv.Checked == true)
                    //        {
                    //            q = "update EmployeesAccount set EmployeeId='" + cmbsupplier2.SelectedValue + "',Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', PayableAccountId='" + PayableAccountId + "' , branchid='" + cmbbranchbpv.SelectedValue + "', Date='" + dateTimePicker4.Text + "', Debit='" + txtamount4.Text + "'  where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";
                    //        }
                    //        if (rdsupplier.Checked == true)
                    //        {
                    //            q = "update SupplierAccount set SupplierId='" + cmbsupplier2.SelectedValue + "',Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', PayableAccountId='" + PayableAccountId + "' , branchid='" + cmbbranchbpv.SelectedValue + "', Date='" + dateTimePicker4.Text + "', Debit='" + txtamount4.Text + "'  where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";
                    //        }
                    //        if (rdgl.Checked == true)
                    //        {
                    //            q = "update SupplierAccount set Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', PayableAccountId='" + cmbsupplier2.SelectedValue + "' , branchid='" + cmbbranchbpv.SelectedValue + "', Date='" + dateTimePicker4.Text + "', Debit='" + txtamount4.Text + "' where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";
                    //        }
                    //    }

                    //    //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    //}

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("supplier account bank error");
            }
            return q;
        }
        public void updatesupplieraccountfirstbank(string val, string payaccount)
        {
            try
            {
                //string q = "select * from SupplierAccount where id < '" + val + "'  and SupplierId='" + cmbsupplier2.SelectedValue + "'  and PayableAccountId='" + payaccount + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                //double balance = Convert.ToDouble(blnce);

                //double newbalance = (balance + Convert.ToDouble(txtamount4.Text));
                //newbalance = Math.Round(newbalance, 2);
                string q = "";
                objCore = new classes.Clsdbcon();
                if (rdsupplier.Checked == true)
                {
                    q = "update SupplierAccount set PayableAccountId='" + payaccount + "' , branchid='" + cmbbranchbpv.SelectedValue + "', Date='" + dateTimePicker4.Text + "', Debit='" + txtamount4.Text + "'  where id='" + val + "'";
                }
                if (rdgl.Checked == true)
                {
                    q = "update SupplierAccount set PayableAccountId='" + cmbsupplier2.SelectedValue + "' , branchid='" + cmbbranchbpv.SelectedValue + "', Date='" + dateTimePicker4.Text + "', Debit='" + txtamount4.Text + "' where id='" + val + "'";
                }
                objCore.executeQuery(q);
                //updatesupplieraccountbank(val, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier first bank account error");
            }
        }
        public void updatesupplieraccountbank(string val, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + val + "'   and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier2.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier account bank error");
            }
        }
        public void updatesupplieraccountremainingbank(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update SupplierAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatesupplieraccountbank(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deletesupplieraccountbank(count, payaccount);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier remaining account bank error");
            }
            return;
        }
        public void deletesupplieraccountfirstbank(int id, string payaccount)
        {
            try
            {
                string q = "";// "select * from SupplierAccount where id <'" + id + "' and    PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier2.SelectedValue + "' order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update SupplierAccount set  Debit='0'  where voucherno='" + txtvoucherno4.Text + "'";
                objCore.executeQuery(q);
                // deletesupplieraccountbank(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier first account bank error");
            }
        }
        public void deletesupplieraccountbank(int id, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + id + "' and  PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier2.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier account bank error");
            }
            getdata("", "");
        }


        public void updatecashaccountfirstcustomer(string val)
        {
            try
            {
                string q = "select * from CashAccountReceiptCustomer where id < '" + val + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance + Convert.ToDouble(txtamount2.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                if (rdglcrv.Checked == true)
                {
                    q = "update CashAccountReceiptCustomer set Date='" + dateTimePicker2.Text + "',ChartAccountId='" + cmbcustomers.SelectedValue + "',CustomerId='0', Description='" + txtdesc2.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount2.Text + "'  where id='" + val + "'";
                }
                if (rdcustomers1.Checked == true)
                {
                    q = "update CashAccountReceiptCustomer set Date='" + dateTimePicker2.Text + "',ChartAccountId='" + cmbcashaccount2.SelectedValue + "',CustomerId='" + cmbcustomers.SelectedValue + "', Description='" + txtdesc2.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount2.Text + "'  where id='" + val + "'";
                }
                objCore.executeQuery(q);
                //updatecashaccountcustomer(val);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account receipt first error");
            }
        }
        public void updatecashaccountcustomer(string val)
        {

            try
            {
                string q = "select * from CashAccountReceiptCustomer where id >='" + val + "' and ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account receipt error");
            }
        }
        public void updatecashaccountremainingcustomer(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CashAccountReceiptCustomer set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatecashaccountcustomer(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletecashaccountcustomer(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash receipt account receipt remaining error");
            }
        }

        public void deletecashaccountfirstcustomer(int id)
        {
            try
            {
                string q = ""; // "select * from CashAccountReceiptCustomer where id <'" + id + "' and ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update CashAccountReceiptCustomer set  Debit='0' where voucherno='" + txtvoucherno2.Text + "'";
                objCore.executeQuery(q);
                //deletecashaccountcustomer(id);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account receipt first error");
            }
        }
        public void deletecashaccountcustomer(int id)
        {

            try
            {
                string q = "select * from CashAccountReceiptCustomer where id >='" + id + "' and ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account receipt error");
            }
            getdatareceipt("", "");
        }



        public string Customeraccount(string amount, string id)
        {
            string q = "";
            try
            {
                DataSet dsacount = new DataSet();
                q = "select Chartaccountid from Customers where id='" + cmbcustomers.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                //if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = "";
                    if (rdglcrv.Checked == true)
                    {

                    }
                    if (rdcustomers1.Checked == true)
                    {
                        PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    }
                    int iddd = 0;
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from CustomerAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CustomerAccount where CustomersId='" + cmbcustomers.SelectedValue + "' and PayableAccountId='" + PayableAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    if (button12.Text == "Save")
                    {
                        if (rdglcrv.Checked == true)
                        {
                            q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType,branchid) values('" + iddd + "','" + dateTimePicker2.Text + "','0','" + cmbcustomers.SelectedValue + "','" + txtvoucherno2.Text + "','" + txtdesc2.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "','Cash','" + cmbbranchcrv.SelectedValue + "')";

                        }
                        if (rdcustomers1.Checked == true)
                        {
                            q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType,branchid) values('" + iddd + "','" + dateTimePicker2.Text + "','" + cmbcustomers.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno2.Text + "','" + txtdesc2.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "','Cash','" + cmbbranchcrv.SelectedValue + "')";

                        }
                        //objCore.executeQuery(q);
                    }
                    if (button12.Text == "Update")
                    {
                        DataSet dsval = new System.Data.DataSet();
                        q = "select id from CustomerAccount where VoucherNo='" + txtvoucherno2.Text + "'";
                        dsval = objCore.funGetDataSet(q);
                        if (dsval.Tables[0].Rows.Count > 0)
                        {
                            // updateCustomeraccountfirst(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                            if (rdglcrv.Checked == true)
                            {
                                q = "update CustomerAccount set Description='" + txtdesc2.Text.Trim().Replace("'", "''") + "', PayableAccountId='" + cmbcustomers.SelectedValue + "' , CustomersId='0', Date='" + dateTimePicker2.Text + "', Credit='" + txtamount2.Text + "',branchid='" + cmbbranchcrv.SelectedValue + "' where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";
                            }
                            if (rdcustomers1.Checked == true)
                            {
                                q = "update CustomerAccount set Description='" + txtdesc2.Text.Trim().Replace("'", "''") + "', PayableAccountId='" + PayableAccountId + "' , CustomersId='" + cmbcustomers.SelectedValue + "', Date='" + dateTimePicker2.Text + "', Credit='" + txtamount2.Text + "',branchid='" + cmbbranchcrv.SelectedValue + "' where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";
                            }
                        }

                        //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Customer account error");
            }
            return q;
        }
        public void updateCustomeraccountfirst(string val, string payaccount)
        {
            try
            {
                //string q = "select * from CustomerAccount where id < '" + val + "'  and CustomersId='" + cmbcustomers.SelectedValue + "' and PayableAccountId='" + payaccount + "' order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                //double balance = Convert.ToDouble(blnce);

                //double newbalance = (balance - Convert.ToDouble(txtamount2.Text));
                string q = "";

                objCore = new classes.Clsdbcon();
                if (rdglcrv.Checked == true)
                {
                    q = "update CustomerAccount set PayableAccountId='" + cmbcustomers.SelectedValue + "' , CustomersId='0', Date='" + dateTimePicker2.Text + "', Credit='" + txtamount2.Text + "',branchid='" + cmbbranchcrv.SelectedValue + "' where id='" + val + "'";
                }
                if (rdcustomers1.Checked == true)
                {
                    q = "update CustomerAccount set PayableAccountId='" + payaccount + "' , CustomersId='" + cmbcustomers.SelectedValue + "', Date='" + dateTimePicker2.Text + "', Credit='" + txtamount2.Text + "',branchid='" + cmbbranchcrv.SelectedValue + "' where id='" + val + "'";
                }
                objCore.executeQuery(q);
                //updateCustomeraccount(val, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer first account error");
            }
        }
        public void updateCustomeraccount(string val, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + val + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer account error");
            }
        }
        public void updateCustomeraccountremaining(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CustomerAccount set Balance='" + newbalance + "'  where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updateCustomeraccount(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deleteCustomeraccount(count, payaccount);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer remaining account error");
            }
            return;
        }
        public void deleteCustomeraccountfirst(int id, string payaccount)
        {
            try
            {
                string q = "";// "select * from CustomerAccount where id <'" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers.SelectedValue + "' order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update CustomerAccount set  Credit='0'  where voucherno='" + txtvoucherno2.Text + "'";
                objCore.executeQuery(q);
                //deleteCustomeraccount(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer first account error");
            }
        }
        public void deleteCustomeraccount(int id, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer account error");
            }
            getdatareceipt("", "");
        }

        public string Customeraccountbank(string amount, string id)
        {
            string q = "";
            try
            {
                DataSet dsacount = new DataSet();
                q = "select Chartaccountid from Customers where id='" + cmbcustomers2.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                // if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = "";
                    if (rdglbrv.Checked == true)
                    {

                    }
                    if (rdcustomers.Checked == true)
                    {
                        PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    }

                    int iddd = 0;
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from CustomerAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CustomerAccount where CustomersId='" + cmbcustomers2.SelectedValue + "' and PayableAccountId='" + PayableAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    if (button15.Text == "Save")
                    {
                        if (rdglbrv.Checked == true)
                        {
                            q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType, CheckNo, CheckDate,branchid) values('" + iddd + "','" + dateTimePicker3.Text + "','0','" + cmbcustomers2.SelectedValue + "','" + txtvoucherno3.Text + "','" + txtdesc3.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "','Bank','" + txtchechkno.Text.Trim().Replace("'", "''") + "','" + txtcheckdate.Text.Trim().Replace("'", "''") + "','" + cmbbranchbrv.SelectedValue + "')";

                        }
                        if (rdcustomers.Checked == true)
                        {
                            q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType, CheckNo, CheckDate,branchid) values('" + iddd + "','" + dateTimePicker3.Text + "','" + cmbcustomers2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno3.Text + "','" + txtdesc3.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "','Bank','" + txtchechkno.Text.Trim().Replace("'", "''") + "','" + txtcheckdate.Text.Trim().Replace("'", "''") + "','" + cmbbranchbrv.SelectedValue + "')";

                        }
                        //objCore.executeQuery(q);
                    }
                    if (button15.Text == "Update")
                    {
                        DataSet dsval = new System.Data.DataSet();
                        q = "select id from CustomerAccount where VoucherNo='" + txtvoucherno3.Text + "'";
                        dsval = objCore.funGetDataSet(q);
                        if (dsval.Tables[0].Rows.Count > 0)
                        {
                            updateCustomeraccountfirstbank(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                            if (rdglbrv.Checked == true)
                            {
                                q = "update CustomerAccount set Description='" + txtdesc3.Text.Trim().Replace("'", "''") + "', PayableAccountId='" + cmbcustomers2.SelectedValue + "', branchid='" + cmbbranchbrv.SelectedValue + "', Date='" + dateTimePicker3.Text + "', Credit='" + txtamount3.Text + "'  where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";

                            }
                            if (rdcustomers.Checked == true)
                            {
                                q = "update CustomerAccount set Description='" + txtdesc3.Text.Trim().Replace("'", "''") + "', PayableAccountId='" + PayableAccountId + "', CustomersId='" + cmbcustomers2.SelectedValue + "', branchid='" + cmbbranchbrv.SelectedValue + "', Date='" + dateTimePicker3.Text + "', Credit='" + txtamount3.Text + "'  where id='" + dsval.Tables[0].Rows[0][0].ToString() + "'";
                            }
                        }

                        //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Customer account bank error");
            }
            return q;
        }
        public void updateCustomeraccountfirstbank(string val, string payaccount)
        {
            try
            {
                //string q = "select * from CustomerAccount where id < '" + val + "'  and CustomersId='" + cmbcustomers2.SelectedValue + "' and PayableAccountId='" + payaccount + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                //double balance = Convert.ToDouble(blnce);

                //double newbalance = (balance - Convert.ToDouble(txtamount3.Text));
                //newbalance = Math.Round(newbalance, 2);
                string q = "";
                objCore = new classes.Clsdbcon();
                if (rdglbrv.Checked == true)
                {
                    q = "update CustomerAccount set PayableAccountId='" + cmbcustomers2.SelectedValue + "', branchid='" + cmbbranchbrv.SelectedValue + "', Date='" + dateTimePicker3.Text + "', Credit='" + txtamount3.Text + "'  where id='" + val + "'";

                }
                if (rdcustomers.Checked == true)
                {

                    q = "update CustomerAccount set PayableAccountId='" + payaccount + "', CustomersId='" + cmbcustomers2.SelectedValue + "', branchid='" + cmbbranchbrv.SelectedValue + "', Date='" + dateTimePicker3.Text + "', Credit='" + txtamount3.Text + "'  where id='" + val + "'";

                }
                objCore.executeQuery(q);
                //updateCustomeraccountbank(val, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer first account bank error");
            }
        }
        public void updateCustomeraccountbank(string val, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + val + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer account bank error");
            }
        }
        public void updateCustomeraccountremainingbank(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CustomerAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updateCustomeraccountbank(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deleteCustomeraccountbank(count, payaccount);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer remaining account error");
            }
            return;
        }
        public void deleteCustomeraccountfirstbank(int id, string payaccount)
        {
            try
            {
                string q = "";// "select * from CustomerAccount where id <'" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers2.SelectedValue + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update CustomerAccount set  Credit='0' where voucherno='" + txtvoucherno3.Text + "'";
                objCore.executeQuery(q);
                // deleteCustomeraccountbank(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer first account error");
            }
        }
        public void deleteCustomeraccountbank(int id, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer account error");
            }
            getdatareceipt("", "");
        }

        public void updateBankaccountfirstcustomer(string val)
        {
            try
            {
                //string q = "select * from BankAccountReceiptCustomer where id < '" + val + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                //}
                //double balance = Convert.ToDouble(blnce);

                //double newbalance = (balance + Convert.ToDouble(txtamount3.Text));
                //newbalance = Math.Round(newbalance, 2);
                string q = "";
                objCore = new classes.Clsdbcon();
                if (rdglbrv.Checked == true)
                {

                    q = "update BankAccountReceiptCustomer set branchid='" + cmbbranchbrv.SelectedValue + "',  CheckNo='" + txtchechkno.Text.Replace("'", "''") + "', CheckDate='" + txtcheckdate.Text.Replace("'", "''") + "', Date='" + dateTimePicker3.Text + "',ChartAccountId='" + cmbcashaccount3.SelectedValue + "', Description='" + txtdesc3.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount3.Text + "'  where id='" + val + "'";

                }
                if (rdcustomers.Checked == true)
                {

                    q = "update BankAccountReceiptCustomer set branchid='" + cmbbranchbrv.SelectedValue + "',  CheckNo='" + txtchechkno.Text.Replace("'", "''") + "', CheckDate='" + txtcheckdate.Text.Replace("'", "''") + "', Date='" + dateTimePicker3.Text + "',ChartAccountId='" + cmbcashaccount3.SelectedValue + "',CustomerId='" + cmbcustomers2.SelectedValue + "', Description='" + txtdesc3.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount3.Text + "'  where id='" + val + "'";

                }
                objCore.executeQuery(q);
                // updatebankaccountcustomer(val);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account receipt first error");
            }
        }
        public void updatebankaccountcustomer(string val)
        {

            try
            {
                string q = "select * from BankAccountReceiptCustomer where id >='" + val + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatebankaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account receipt error");
            }
        }
        public void updatebankaccountremainingcustomer(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update BankAccountReceiptCustomer set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatebankaccountcustomer(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletebankaccountcustomer(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank receipt account receipt remaining error");
            }
        }

        public void deletebankaccountfirstcustomer(int id)
        {
            try
            {
                string q = "";// "select * from BankAccountReceiptCustomer where id <'" + id + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id desc";
                //DataSet dss = new System.Data.DataSet();
                //dss = objCore.funGetDataSet(q);
                //string blnce = "0";
                //if (dss.Tables[0].Rows.Count > 0)
                //{
                //    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                //}
                objCore = new classes.Clsdbcon();
                q = "update BankAccountReceiptCustomer set  Debit='0' where voucherno='" + txtvoucherno3.Text + "'";
                objCore.executeQuery(q);
                // deletebankaccountcustomer(id);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete bank account receipt first error");
            }
        }
        public void deletebankaccountcustomer(int id)
        {

            try
            {
                string q = "select * from BankAccountReceiptCustomer where id >='" + id + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatebankaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account receipt error");
            }
            getdatareceipt("", "");
        }

        private void txtamount_TextChanged(object sender, EventArgs e)
        {
            if (txtamount.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Price Per Package. Only Nymbers are allowed");
                    txtamount.Focus();
                    return;
                }
            }
        }
        public void getinfo(string id, string branchid)
        {
            try
            {
                try
                {
                    cmbbranch.SelectedValue = branchid;
                }
                catch (Exception ex)
                {


                }
                string q = "select  * from CashAccountPaymentSupplier where Voucherno='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string sid = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                    if (sid == "0")
                    {
                        rdglcpv.Checked = true;
                        rdsupplier1.Checked = false;
                        rdemployees.Checked = false;
                        DataSet dsbpvs = new System.Data.DataSet();
                        q = "select PayableAccountId,invoiceno from SupplierAccount where Voucherno='" + id + "'";
                        dsbpvs = objCore.funGetDataSet(q);
                        if (dsbpvs.Tables[0].Rows.Count > 0)
                        {
                            cmbsupplier.SelectedValue = dsbpvs.Tables[0].Rows[0]["PayableAccountId"].ToString();

                        }
                        else
                        {
                            q = "select EmployeeId from EmployeesAccount where Voucherno='" + id + "'";
                            dsbpvs = objCore.funGetDataSet(q);
                            if (dsbpvs.Tables[0].Rows.Count > 0)
                            {
                                rdemployees.Checked = true;
                                rdglcpv.Checked = false;
                                rdsupplier1.Checked = false;
                                cmbsupplier.SelectedValue = dsbpvs.Tables[0].Rows[0]["EmployeeId"].ToString();
                            }
                        }


                    }
                    else
                    {
                        rdglcpv.Checked = false;
                        rdsupplier1.Checked = true;
                        cmbsupplier.SelectedValue = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                        DataSet dsbpvs = new System.Data.DataSet();
                        q = "select PayableAccountId,invoiceno from SupplierAccount where Voucherno='" + dsacount.Tables[0].Rows[0]["Voucherno"].ToString() + "'";
                        dsbpvs = objCore.funGetDataSet(q);
                        if (dsbpvs.Tables[0].Rows.Count > 0)
                        {

                            try
                            {
                                cmbinvoicenocpv.SelectedValue = dsbpvs.Tables[0].Rows[0]["invoiceno"].ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                    //cmbsupplier.SelectedValue = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                    cmbcashaccount.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount.Text = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                    txtdesc.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    dateTimePicker1.Text = dsacount.Tables[0].Rows[0]["date"].ToString();
                    button1.Text = "Update";
                    acid = dsacount.Tables[0].Rows[0]["id"].ToString();
                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dsacount.Tables[0].Rows[0]["supporting"]);
                        imageData = data;
                        System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                        pictureBox2.Image = Image.FromStream(mem);
                    }
                    catch (Exception ex)
                    {


                    }
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void getinfobank(string id,string branchid)
        {
            try
            {
                try
                {
                    cmbbranchbpv.SelectedValue = branchid;
                }
                catch (Exception ex)
                {


                }
                string q = "select  * from BankAccountPaymentSupplier where Voucherno='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string sid = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                    if (sid == "0")
                    {
                        rdgl.Checked = true;
                        rdsupplier.Checked = false;
                        DataSet dsbpvs = new System.Data.DataSet();
                        q = "select PayableAccountId,invoiceno from SupplierAccount where Voucherno='" + id + "'";
                        dsbpvs = objCore.funGetDataSet(q);
                        if (dsbpvs.Tables[0].Rows.Count > 0)
                        {
                            cmbsupplier2.SelectedValue = dsbpvs.Tables[0].Rows[0]["PayableAccountId"].ToString();
                            try
                            {
                                cmbinvoicebpv.SelectedValue = dsbpvs.Tables[0].Rows[0]["invoiceno"].ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            q = "select EmployeeId from EmployeesAccount where Voucherno='" + id + "'";
                            dsbpvs = objCore.funGetDataSet(q);
                            if (dsbpvs.Tables[0].Rows.Count > 0)
                            {
                                rdemployees.Checked = true;
                                rdglcpv.Checked = false;
                                rdsupplier1.Checked = false;
                                cmbsupplier.SelectedValue = dsbpvs.Tables[0].Rows[0]["EmployeeId"].ToString();
                            }
                        }

                    }
                    else
                    {
                        rdgl.Checked = false;
                        rdsupplier.Checked = true;
                        cmbsupplier2.SelectedValue = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                        DataSet dsbpvs = new System.Data.DataSet();
                        q = "select PayableAccountId,invoiceno from SupplierAccount where Voucherno='" + dsacount.Tables[0].Rows[0]["Voucherno"].ToString() + "'";
                        dsbpvs = objCore.funGetDataSet(q);
                        if (dsbpvs.Tables[0].Rows.Count > 0)
                        {
                           
                            try
                            {
                                cmbinvoicebpv.SelectedValue = dsbpvs.Tables[0].Rows[0]["invoiceno"].ToString();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }

                    

                    cmbcashaccount4.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno4.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount4.Text = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                    txtdesc4.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    button21.Text = "Update";
                    txtchechkno2.Text = dsacount.Tables[0].Rows[0]["CheckNo"].ToString();
                    txtcheckdate2.Text = dsacount.Tables[0].Rows[0]["CheckDate"].ToString();
                    dateTimePicker4.Text = dsacount.Tables[0].Rows[0]["date"].ToString();

                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dsacount.Tables[0].Rows[0]["supporting"]);
                        System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                        pictureBox1.Image = Image.FromStream(mem);
                        imageData = data;
                    }
                    catch (Exception ex)
                    {


                    }
                   
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void getinforeceipt(string id,string branchid)
        {
            try
            {
                try
                {
                    cmbbranchcrv.SelectedValue = branchid;
                }
                catch (Exception ex)
                {
                    
                    
                }
                string q = "select  * from CashAccountReceiptCustomer where Voucherno='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string sid = dsacount.Tables[0].Rows[0]["CustomerId"].ToString();
                    if (sid == "0")
                    {
                        rdglcrv.Checked = true;
                        rdcustomers1.Checked = false;
                        DataSet dsbpvs = new System.Data.DataSet();
                        q = "select PayableAccountId from CustomerAccount where Voucherno='" + id + "'";
                        dsbpvs = objCore.funGetDataSet(q);
                        if (dsbpvs.Tables[0].Rows.Count > 0)
                        {
                            cmbcustomers.SelectedValue = dsbpvs.Tables[0].Rows[0]["PayableAccountId"].ToString();
                        }
                    }
                    else
                    {
                        rdglcrv.Checked = false;
                        rdcustomers1.Checked = true;
                        cmbcustomers.SelectedValue = dsacount.Tables[0].Rows[0]["CustomerId"].ToString();
                    }
                    //cmbcustomers.SelectedValue = dsacount.Tables[0].Rows[0]["CustomerId"].ToString();
                    cmbcashaccount2.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno2.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount2.Text = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                    txtdesc2.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    button12.Text = "Update";
                    dateTimePicker2.Text = dsacount.Tables[0].Rows[0]["date"].ToString();
                    //acid = dsacount.Tables[0].Rows[0]["id"].ToString();
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void getinforeceiptbank(string id,string branchid)
        {
            try
            {
                try
                {
                    cmbbranchbrv.SelectedValue = branchid;
                }
                catch (Exception ex)
                {
                    
                }
                string q = "select  * from BankAccountReceiptCustomer where Voucherno='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string sid = dsacount.Tables[0].Rows[0]["CustomerId"].ToString();
                    if (sid == "0")
                    {
                        rdglbrv.Checked = true;
                        rdcustomers.Checked = false;
                        DataSet dsbpvs = new System.Data.DataSet();
                        q = "select PayableAccountId from CustomerAccount where Voucherno='" + id + "'";
                        dsbpvs = objCore.funGetDataSet(q);
                        if (dsbpvs.Tables[0].Rows.Count > 0)
                        {
                            cmbcustomers2.SelectedValue = dsbpvs.Tables[0].Rows[0]["PayableAccountId"].ToString();
                        }
                    }
                    else
                    {
                        rdglbrv.Checked = false;
                        rdcustomers.Checked = true;
                        cmbcustomers2.SelectedValue = dsacount.Tables[0].Rows[0]["CustomerId"].ToString();
                    }
                    cmbcashaccount3.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno3.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount3.Text = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                    txtdesc3.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    txtchechkno.Text = dsacount.Tables[0].Rows[0]["CheckNo"].ToString();
                    txtcheckdate.Text = dsacount.Tables[0].Rows[0]["CheckDate"].ToString();
                    button15.Text = "Update";
                    dateTimePicker3.Text = dsacount.Tables[0].Rows[0]["date"].ToString();
                    //acid = dsacount.Tables[0].Rows[0]["id"].ToString();
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        string jvdate = "";
        public void getinforejournaledit(string id)
        {
            try
            {
                int indx = dataGridView6.CurrentCell.RowIndex;
                if (indx >= 0)
                {

                    cmbtype.Text = dataGridView6.Rows[indx].Cells["Account Type"].Value.ToString();
                    txtvoucherno5.Text = dataGridView6.Rows[indx].Cells["voucher No"].Value.ToString();
                    txtdebit.Text = dataGridView6.Rows[indx].Cells["debit"].Value.ToString();
                    txtcredit.Text = dataGridView6.Rows[indx].Cells["credit"].Value.ToString();
                    txtdesc5.Text = dataGridView6.Rows[indx].Cells["description"].Value.ToString();
                    cmbaccount.SelectedValue = dataGridView6.Rows[indx].Cells["id"].Value.ToString();
                    dateTimePicker5.Text = jvdate;
                    cmbbranchjv.SelectedValue = jvbranchid;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void chartacounts()
        {
            DataSet dscharts = new System.Data.DataSet();
            string q = "select * from ChartofAccounts";
            dscharts = objCore.funGetDataSet(q);
            for (int i = 0; i < dscharts.Tables[0].Rows.Count; i++)
            {
                updatejournalaccountfirst(dscharts.Tables[0].Rows[i]["id"].ToString());
            }
        }
        public void updatejournalaccountfirst(string val)
        {
            try
            {
                string q = "select * from JournalAccount where PayableAccountId ='" + val + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }

                        newbalance = Convert.ToDouble(blnce);
                    }
                    else
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }
                        newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit);

                    }
                    newbalance = Math.Round(newbalance, 2);
                    objCore = new classes.Clsdbcon();
                    q = "update JournalAccount set     Debit='" + debit + "' ,credit='" + credit + "' , Balance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() + "'";
                    objCore.executeQuery(q);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("update journal  error");
            }
        }
        //public void updatejournalaccountfirst(string val, string acount, string debit, string credit, string date, string desc)
        //{
        //    try
        //    {
        //        string q = "select * from JournalAccount where id < '" + val + "' and PayableAccountId='" + acount + "' order by id desc";
        //        DataSet dss = new System.Data.DataSet();
        //        dss = objCore.funGetDataSet(q);
        //        string blnce = "0";
        //        if (dss.Tables[0].Rows.Count > 0)
        //        {
        //            blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
        //        }
        //        double balance = Convert.ToDouble(blnce);

        //        double newbalance = (balance + Convert.ToDouble(debit)) - Convert.ToDouble(credit);
        //        newbalance = Math.Round(newbalance, 2);
        //        objCore = new classes.Clsdbcon();
        //        q = "update JournalAccount set    Date='" + date + "', Description='" + desc + "', Debit='" + debit + "' ,credit='" + credit + "' , Balance='" + newbalance + "' where id='" + val + "'";
        //        objCore.executeQuery(q);
        //        updatejournalaccount(val, acount);
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show("update journal  first error");
        //    }
        //}
        public void updatejournalaccount(string val, string acount)
        {

            try
            {
                string q = "select * from JournalAccount where id >='" + val + "' and PayableAccountId='" + acount + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            string val2 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val2 == "")
                            {
                                val2 = "0";

                            }


                            updatejournalaccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", acount, val2);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update journal error");
            }
        }
        public void updatejournalaccountremaining(string val, string debit, string id, string functioncall, string acount, string credit)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(debit)) - Convert.ToDouble(credit);
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update JournalAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatejournalaccount(count.ToString(), acount);
                }
                if (functioncall == "delete")
                {
                    // deletebankaccountcustomer(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update journal remaining error");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dr == DialogResult.No)
            //    {
            //        return;
            //    }
            //    int indx = dataGridView1.CurrentCell.RowIndex;
            //    if (indx >= 0)
            //    {
            //        string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
            //        getinfo(id);

            //    }
            //}
            //catch (Exception ex)
            //{


            //}
        }
        public void clear()
        {
            try
            {

                txtvoucherno.Text = "";
                txtamount.Text = "";
                txtdesc.Text = "";
                button1.Text = "Save";
                acid = "";
                cmbsupplier.SelectedValue = "";
                cmbcashaccount.SelectedValue = "";
                pictureBox2.Image = null;
                imageData = null;
            }
            catch (Exception ex)
            {


            }
        }
        public void clear2()
        {
            try
            {

                txtvoucherno2.Text = "";
                txtamount2.Text = "";
                txtdesc2.Text = "";
                button12.Text = "Save";
                //acid = "";
                cmbcustomers.SelectedValue = "";
                cmbcashaccount2.SelectedValue = "";
            }
            catch (Exception ex)
            {


            }
        }
        public void clear3()
        {
            try
            {

                txtvoucherno3.Text = "";
                txtamount3.Text = "";
                txtdesc3.Text = "";
                button15.Text = "Save";
                //acid = "";
                txtchechkno.Text = "";
                cmbcustomers2.SelectedValue = "";
                cmbcashaccount3.SelectedValue = "";
            }
            catch (Exception ex)
            {


            }
        }
        public void clear4()
        {
            try
            {

                txtvoucherno4.Text = "";
                txtamount4.Text = "";
                txtdesc4.Text = "";
                button21.Text = "Save";
                //acid = "";
                txtchechkno2.Text = "";
                cmbsupplier2.SelectedValue = "";
                cmbcashaccount4.SelectedValue = "";
                pictureBox1.Image = null;
                imageData = null;
            }
            catch (Exception ex)
            {


            }
        }
        public void clear5()
        {
            try
            {

                txtvoucherno5.Text = "";
                txtdebit.Text = "";
                txtcredit.Text = "";
                txtdesc5.Text = "";
                //button29.Text = "Save";
                //acid = "";


            }
            catch (Exception ex)
            {


            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        protected void cpvledger(string start,string end,string supplierid,string suppliername,string branchid,string branchname)
        {
            return;
            try
            {
                DataTable dt = new DataTable();
                POSRestaurant.Reports.Statements.rptpayable rptDoc = new Reports.Statements.rptpayable();
                POSRestaurant.Reports.Statements.dspayablebank dsrpt = new Reports.Statements.dspayablebank();
                dt.TableName = "Crystal Report";
                dt = getAllOrderscpv(supplierid, start, end, branchid, suppliername);
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
               
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
               
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "For the period of " + Convert.ToDateTime(start).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(end).ToString("dd-MM-yyyy"));

                rptDoc.SetParameterValue("branch", branchname);
                rptDoc.SetParameterValue("statement", "Payable Statement");
            
                crystalReportViewer1.ReportSource = rptDoc;
                 crystalReportViewer2.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

            }
        }
        public double bfr(string csid,string start,string suplierid,string branchid)
        {
            double bf = 0;
            try
            {
                string q = "";
                if (cmbsupplier.Text == "All Suppliers")
                {

                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where  date <'" + start + "'  and SupplierId='" + csid + "'";

                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where SupplierId='" + suplierid + "' and branchid='" + branchid + "' and date <'" + start + "'  ";

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
        public DataTable getAllOrderscpv(string suplierid, string start, string end, string branchid, string suppliername)
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
                return dtrpt;
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


                bf = bfr("", start,suplierid,branchid);
                    
                   
                    if (logo == "")
                    {
                        //string date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString();
                        if (bf >= 0)
                        {
                            if (logo == "")
                            {

                                dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", suppliername, "For the period of " + start + " to " + end, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", suppliername, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }
                        else
                        {
                            bf = System.Math.Abs(bf);
                            if (logo == "")
                            {

                                dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", suppliername, "For the period of " + start + " to " + end, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", suppliername, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }
                    }
                    else
                    {
                        if (bf >= 0)
                        {
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", suppliername, "For the period of " + start + " to " + end, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", suppliername, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                            }

                        }
                        else
                        {
                            bf = System.Math.Abs(bf);
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", suppliername, "For the period of " + start + " to " + end, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", suppliername, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }


                    }

                    q = "SELECT     dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                       dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname FROM         dbo.SupplierAccount INNER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id where  SupplierAccount.SupplierId='" + suplierid + "' and (SupplierAccount.Date between '" + start + "' and '" + end + "') and SupplierAccount.branchid='" + branchid + "' order by dbo.SupplierAccount.Date,dbo.SupplierAccount.VoucherNo";
               
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

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + start + " to " + end, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        private void cmbsupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata("", "");
            if (rdsupplier1.Checked == true)
            {
                double blnc = getsuplierbalance(cmbsupplier.SelectedValue.ToString(), cmbbranch.SelectedValue.ToString());
                lblbalancecpvsupplier.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                getinvoices(cmbsupplier.SelectedValue.ToString(), "CPV");
            }
            else if (rdemployees.Checked == true)
            {
                double blnc = getemployeesbalance(cmbsupplier.SelectedValue.ToString(), cmbbranch.SelectedValue.ToString());
                lblbalancecpvsupplier.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
            }
            else
            {
                lblbalancecpvsupplier.Text = "";
            }
        }

        private void cmbcashaccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata("", "");
            try
            {
                double blnc = getcashbalance(cmbcashaccount.SelectedValue.ToString(), cmbbranch.SelectedValue.ToString());
                lblcpv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                POSRestaurant.Properties.Settings.Default.Save();
                string status = objCore.authenticate("delete");
                if (status == "no")
                {
                    POSRestaurant.classes.Message obj = new classes.Message();
                    obj.Show();
                    return;
                }
                if (txtvoucherno.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }

                DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                //int indx = dataGridView1.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno.Text;// dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    deletecashaccountfirst(1);
                    DataSet dsval = new System.Data.DataSet();
                    //string q = "select id,PayableAccountId from SupplierAccount where VoucherNo='" + txtvoucherno.Text + "'";
                    //dsval=objCore.funGetDataSet(q);
                    //if(dsval.Tables[0].Rows.Count>0)
                    {
                        deletesupplieraccountfirst(1, "");
                    }

                }
                MessageBox.Show("Deleted Successfully");
                clear();
            }
            catch (Exception ex)
            {


            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbcustomers.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Customer");
                        return;
                    }
                    if (txtamount2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount2.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount2.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount2.Focus();
                            return;
                        }
                    }
                    if (txtdesc2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button12.Text == "Save")
                {
                    if (txtvoucherno2.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from CashAccountReceiptCustomer");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            idd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            idd = 1;
                        }
                        string q = "select top 1 * from CashAccountReceiptCustomer where ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance + Convert.ToDouble(txtamount2.Text));
                        newbalance = Math.Round(newbalance, 2);
                        if (rdglcrv.Checked == true)
                        {

                            q = "insert into CashAccountReceiptCustomer (id,Date,ChartAccountId,CustomerId,Voucherno,Description,Debit,Credit,CurrentBalance,branchid) values('" + idd + "','" + dateTimePicker2.Text + "','" + cmbcashaccount2.SelectedValue + "','0','CRV-" + idd + "','" + txtdesc2.Text.Trim().Replace("'", "''") + "','" + txtamount2.Text + "','0','" + newbalance + "','" + cmbbranchcrv.SelectedValue + "')";

                        }
                        if (rdcustomers1.Checked == true)
                        {

                            q = "insert into CashAccountReceiptCustomer (id,Date,ChartAccountId,CustomerId,Voucherno,Description,Debit,Credit,CurrentBalance,branchid) values('" + idd + "','" + dateTimePicker2.Text + "','" + cmbcashaccount2.SelectedValue + "','" + cmbcustomers.SelectedValue + "','CRV-" + idd + "','" + txtdesc2.Text.Trim().Replace("'", "''") + "','" + txtamount2.Text + "','0','" + newbalance + "','" + cmbbranchcrv.SelectedValue + "')";

                        }
                        //objCore.executeQuery(q);
                        txtvoucherno2.Text = "CRV-" + idd;
                        string q2 = Customeraccount(txtamount2.Text, "");
                        ExecuteSqlTransaction("", q, q2, "Data Added Successfully");
                        //MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button12.Text == "Update")
                {
                    POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                    POSRestaurant.Properties.Settings.Default.Save();
                    string status = objCore.authenticate("update");
                    if (status == "no")
                    {
                        POSRestaurant.classes.Message obj = new classes.Message();
                        obj.Show();
                        return;
                    }
                    string q = "select  * from CashAccountReceiptCustomer where voucherno='" + txtvoucherno2.Text + "'";// where ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno2.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();
                                if (rdglcrv.Checked == true)
                                {
                                    q = "update CashAccountReceiptCustomer set  branchid='" + cmbbranchcrv.SelectedValue + "', Date='" + dateTimePicker2.Text + "',ChartAccountId='" + cmbcashaccount2.SelectedValue + "',CustomerId='0', Description='" + txtdesc2.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount2.Text + "'  where id='" + val + "'";
                                }
                                if (rdcustomers1.Checked == true)
                                {
                                    q = "update CashAccountReceiptCustomer set  branchid='" + cmbbranchcrv.SelectedValue + "', Date='" + dateTimePicker2.Text + "',ChartAccountId='" + cmbcashaccount2.SelectedValue + "',CustomerId='" + cmbcustomers.SelectedValue + "', Description='" + txtdesc2.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount2.Text + "'  where id='" + val + "'";
                                }
                                //return;
                                chk = true;
                            }

                        }
                    }
                    string q2 = Customeraccount(txtamount2.Text, txtvoucherno2.Text);
                    ExecuteSqlTransaction("", q, q2, "Data Updated Successfully");
                    //MessageBox.Show("Data Updated Successfully");
                }
            }
            catch (Exception ex)
            {
            }
              clear2();
            getdatareceipt("", "");
        }

        private void txtamount_TextChanged_1(object sender, EventArgs e)
        {
            if (txtamount.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount.Focus();
                    return;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            getdatareceipt("", "yes");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            clear2();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dr == DialogResult.No)
            //    {
            //        return;
            //    }
            //    int indx = dataGridView2.CurrentCell.RowIndex;
            //    if (indx >= 0)
            //    {
            //        string id = dataGridView2.Rows[indx].Cells[0].Value.ToString();
            //        getinforeceipt(id);

            //    }
            //}
            //catch (Exception ex)
            //{


            //}
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                POSRestaurant.Properties.Settings.Default.Save();
                string status = objCore.authenticate("delete");
                if (status == "no")
                {
                    POSRestaurant.classes.Message obj = new classes.Message();
                    obj.Show();
                    return;
                }
                if (txtvoucherno2.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                //int indx = dataGridView2.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = "";// dataGridView2.Rows[indx].Cells[0].Value.ToString();
                    deletecashaccountfirstcustomer(1);
                    DataSet dsval = new System.Data.DataSet();
                    //string q = "select id,PayableAccountId from CustomerAccount where VoucherNo='" + dataGridView2.Rows[indx].Cells[2].Value.ToString() + "'";
                    //dsval = objCore.funGetDataSet(q);
                    //if (dsval.Tables[0].Rows.Count > 0)
                    {
                        deleteCustomeraccountfirst(1, "");
                    }

                }
                MessageBox.Show("Deleted Successfully");
                clear2();
            }
            catch (Exception ex)
            {


            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdcustomers.Checked == true)
                {
                    double blnc = getcustomerbalance(cmbcustomers2.SelectedValue.ToString(), cmbbranchbrv.SelectedValue.ToString());
                    lblcustomerbrv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
                else
                {
                    lblcustomerbrv.Text = "";
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                double blnc = getcashbalance(cmbcashaccount3.SelectedValue.ToString(), cmbbranchbrv.SelectedValue.ToString());
                lblbrv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
            }
            catch (Exception ex)
            {


            }
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            DialogResult drl = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drl == DialogResult.No)
            {
                return;
            }
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount3.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbcustomers2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Customer");
                        return;
                    }
                    if (txtamount3.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount3.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount3.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount3.Focus();
                            return;
                        }
                    }
                    if (txtdesc3.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button15.Text == "Save")
                {
                    if (txtvoucherno3.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from BankAccountReceiptCustomer");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            idd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            idd = 1;
                        }
                        string q = "select top 1 * from BankAccountReceiptCustomer where ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance + Convert.ToDouble(txtamount3.Text));
                        newbalance = Math.Round(newbalance, 2);
                        if (rdglbrv.Checked == true)
                        {
                            q = "insert into BankAccountReceiptCustomer (id,Date,ChartAccountId,CustomerId,Voucherno,Description,Debit,Credit,CurrentBalance, CheckNo, CheckDate,branchid) values('" + idd + "','" + dateTimePicker3.Text + "','" + cmbcashaccount3.SelectedValue + "','0','BRV-" + idd + "','" + txtdesc3.Text.Trim().Replace("'", "''") + "','" + txtamount3.Text + "','0','" + newbalance + "','" + txtchechkno.Text.Trim().Replace("'", "''") + "','" + txtcheckdate.Text.Trim().Replace("'", "''") + "','" + cmbbranchbrv.SelectedValue + "')";
                        }
                        if (rdcustomers.Checked == true)
                        {
                            q = "insert into BankAccountReceiptCustomer (id,Date,ChartAccountId,CustomerId,Voucherno,Description,Debit,Credit,CurrentBalance, CheckNo, CheckDate,branchid) values('" + idd + "','" + dateTimePicker3.Text + "','" + cmbcashaccount3.SelectedValue + "','" + cmbcustomers2.SelectedValue + "','BRV-" + idd + "','" + txtdesc3.Text.Trim().Replace("'", "''") + "','" + txtamount3.Text + "','0','" + newbalance + "','" + txtchechkno.Text.Trim().Replace("'", "''") + "','" + txtcheckdate.Text.Trim().Replace("'", "''") + "','" + cmbbranchbrv.SelectedValue + "')";
                        }
                        //objCore.executeQuery(q);
                        txtvoucherno3.Text = "BRV-" + idd;
                        string q2 = Customeraccountbank(txtamount3.Text, "");
                        ExecuteSqlTransaction("", q, q2, "Data Added Successfully");
                        //MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button15.Text == "Update")
                {
                    POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                    POSRestaurant.Properties.Settings.Default.Save();
                    string status = objCore.authenticate("update");
                    if (status == "no")
                    {
                        POSRestaurant.classes.Message obj = new classes.Message();
                        obj.Show();
                        return;
                    }
                    string q = "select  * from BankAccountReceiptCustomer where voucherno='" + txtvoucherno3.Text + "'";// where ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno3.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();

                                //updateBankaccountfirstcustomer(val);
                                if (rdglbrv.Checked == true)
                                {

                                    q = "update BankAccountReceiptCustomer set branchid='" + cmbbranchbrv.SelectedValue + "',  CheckNo='" + txtchechkno.Text.Replace("'", "''") + "', CheckDate='" + txtcheckdate.Text.Replace("'", "''") + "', Date='" + dateTimePicker3.Text + "',ChartAccountId='" + cmbcashaccount3.SelectedValue + "', Description='" + txtdesc3.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount3.Text + "'  where id='" + val + "'";

                                }
                                if (rdcustomers.Checked == true)
                                {

                                    q = "update BankAccountReceiptCustomer set branchid='" + cmbbranchbrv.SelectedValue + "',  CheckNo='" + txtchechkno.Text.Replace("'", "''") + "', CheckDate='" + txtcheckdate.Text.Replace("'", "''") + "', Date='" + dateTimePicker3.Text + "',ChartAccountId='" + cmbcashaccount3.SelectedValue + "',CustomerId='" + cmbcustomers2.SelectedValue + "', Description='" + txtdesc3.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount3.Text + "'  where id='" + val + "'";

                                }
                                chk = true;
                            }

                        }

                    }


                    string q2 = Customeraccountbank(txtamount3.Text, txtvoucherno3.Text);
                    ExecuteSqlTransaction("", q, q2, "Data Updated Successfully");
                    // MessageBox.Show("Data Updated Successfully");
                }
            }
            catch (Exception ex)
            {


            }
            clear3();
            getdatareceiptbank("", "");

        }

        private void txtamount2_TextChanged(object sender, EventArgs e)
        {
            if (txtamount2.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount2.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount2.Focus();
                    return;
                }
            }
        }

        private void txtamount3_TextChanged(object sender, EventArgs e)
        {
            if (txtamount3.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount3.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount3.Focus();
                    return;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            clear3();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            getdatareceiptbank("", "yes");
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dr == DialogResult.No)
            //    {
            //        return;
            //    }
            //    int indx = dataGridView3.CurrentCell.RowIndex;
            //    if (indx >= 0)
            //    {
            //        string id = dataGridView3.Rows[indx].Cells[0].Value.ToString();
            //        getinforeceiptbank(id);

            //    }
            //}
            //catch (Exception ex)
            //{


            //}
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                POSRestaurant.Properties.Settings.Default.Save();
                string status = objCore.authenticate("delete");
                if (status == "no")
                {
                    POSRestaurant.classes.Message obj = new classes.Message();
                    obj.Show();
                    return;
                }
                if (txtvoucherno3.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                //int indx = dataGridView3.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = "";// dataGridView3.Rows[indx].Cells[0].Value.ToString();
                    deletebankaccountfirstcustomer(1);
                    //DataSet dsval = new System.Data.DataSet();
                    //string q = "select id,PayableAccountId from CustomerAccount where VoucherNo='" + dataGridView3.Rows[indx].Cells[2].Value.ToString() + "'";
                    //dsval = objCore.funGetDataSet(q);
                    //if (dsval.Tables[0].Rows.Count > 0)
                    {
                        deleteCustomeraccountfirstbank(1, "");
                    }

                    // getdatareceiptbank("","");
                }
                MessageBox.Show("Deleted Successfully");
                clear3();

            }
            catch (Exception ex)
            {


            }
        }

        private void txtamount4_TextChanged(object sender, EventArgs e)
        {
            if (txtamount4.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount4.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount4.Focus();
                    return;
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount4.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbsupplier2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Supplier");
                        return;
                    }
                    if (txtamount4.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount4.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount4.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount4.Focus();
                            return;
                        }
                    }
                    if (txtdesc4.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button21.Text == "Save")
                {
                    if (txtvoucherno4.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from BankAccountPaymentSupplier");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            idd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            idd = 1;
                        }
                        string q = "select top 1 * from BankAccountPaymentSupplier where ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance - Convert.ToDouble(txtamount4.Text));
                        newbalance = Math.Round(newbalance, 2);
                        if (rdsupplier.Checked == true)
                        {
                            q = "insert into BankAccountPaymentSupplier (id,Date,ChartAccountId,SupplierId,Voucherno,Description,Debit,Credit,CurrentBalance,CheckNo, CheckDate,branchid) values('" + idd + "','" + dateTimePicker4.Text + "','" + cmbcashaccount4.SelectedValue + "','" + cmbsupplier2.SelectedValue + "','BPV-" + idd + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','0','" + txtamount4.Text + "','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','" + cmbbranchbpv.SelectedValue + "')";
                        }
                        if (rdgl.Checked == true || rdemployeesbpv.Checked==true)
                        {
                            q = "insert into BankAccountPaymentSupplier (id,Date,ChartAccountId,SupplierId,Voucherno,Description,Debit,Credit,CurrentBalance,CheckNo, CheckDate,branchid) values('" + idd + "','" + dateTimePicker4.Text + "','" + cmbcashaccount4.SelectedValue + "','0','BPV-" + idd + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','0','" + txtamount4.Text + "','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','" + cmbbranchbpv.SelectedValue + "')";
                        }
                        //objCore.executeQuery(q);
                        txtvoucherno4.Text = "BPV-" + idd;
                        string q2 = supplieraccountBank(txtamount4.Text, "");
                        ExecuteSqlTransaction("", q, q2, "Data Added Successfully");
                        savesupporting("BankAccountPaymentSupplier", txtvoucherno4.Text);
                        //MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button21.Text == "Update")
                {
                    POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                    POSRestaurant.Properties.Settings.Default.Save();
                    string status = objCore.authenticate("update");
                    if (status == "no")
                    {
                        POSRestaurant.classes.Message obj = new classes.Message();
                        obj.Show();
                        return;
                    }
                    string q = "select  * from BankAccountPaymentSupplier where voucherno='" + txtvoucherno4.Text + "'";// where ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno4.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();
                                //if (val == "")
                                //{
                                //    val = "0";

                                //}
                                if (rdgl.Checked == true)
                                {
                                    q = "update BankAccountPaymentSupplier set status='Pending', branchid='" + cmbbranchbpv.SelectedValue + "', Date='" + dateTimePicker4.Text + "',ChartAccountId='" + cmbcashaccount4.SelectedValue + "',SupplierId='0', Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount4.Text + "' where id='" + val + "'";

                                }
                                if (rdsupplier.Checked == true || rdemployeesbpv.Checked == true)
                                {
                                    q = "update BankAccountPaymentSupplier set status='Pending', branchid='" + cmbbranchbpv.SelectedValue + "', Date='" + dateTimePicker4.Text + "',ChartAccountId='" + cmbcashaccount4.SelectedValue + "',SupplierId='" + cmbsupplier2.SelectedValue + "', Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount4.Text + "' where id='" + val + "'";
                                }
                                //updatecashaccountfirstbank(val);
                                //return;
                                chk = true;
                            }

                        }
                        string qry = "delete from EmployeesAccount where voucherno='" + txtvoucherno4.Text + "' ";
                        objCore.executeQuery(qry);
                        qry = "delete from SupplierAccount where voucherno='" + txtvoucherno4.Text + "' ";
                        objCore.executeQuery(qry);
                        string q2 = supplieraccountBank(txtamount4.Text, txtvoucherno4.Text);
                        ExecuteSqlTransaction("", q, q2, "Data Updated Successfully");
                        savesupporting("BankAccountPaymentSupplier", txtvoucherno4.Text);
                        // MessageBox.Show("Data Updated Successfully");
                    }



                }
            }
            catch (Exception ex)
            {


            }
            clear4();
            getdatabank("", "");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            clear4();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            getdatabank("", "yes");
        }

        private void button23_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dr == DialogResult.No)
            //    {
            //        return;
            //    }
            //    int indx = dataGridView4.CurrentCell.RowIndex;
            //    if (indx >= 0)
            //    {
            //        string id = dataGridView4.Rows[indx].Cells[0].Value.ToString();
            //        getinfobank(id);

            //    }
            //}
            //catch (Exception ex)
            //{


            //}
        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                POSRestaurant.Properties.Settings.Default.Save();
                string status = objCore.authenticate("delete");
                if (status == "no")
                {
                    POSRestaurant.classes.Message obj = new classes.Message();
                    obj.Show();
                    return;
                }
                if (txtvoucherno4.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                //int indx = dataGridView4.CurrentCell.RowIndex;
                // if (indx >= 0)
                {
                    //string id = dataGridView4.Rows[indx].Cells[0].Value.ToString();
                    deletecashaccountfirstbank(1);
                    DataSet dsval = new System.Data.DataSet();
                    //string q = "select id,PayableAccountId from SupplierAccount where VoucherNo='" + dataGridView4.Rows[indx].Cells[2].Value.ToString() + "'";
                    //dsval = objCore.funGetDataSet(q);
                    //if (dsval.Tables[0].Rows.Count > 0)
                    {
                        deletesupplieraccountfirstbank(1, "");
                    }

                }
                MessageBox.Show("Deleted Successfully");
                clear4();
            }
            catch (Exception ex)
            {


            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            //DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (dr == DialogResult.No)
            //{
            //    return;
            //}
            try
            {
                if (cmbaccount.Text == "")
                {
                    MessageBox.Show("Please Select Account");
                    return;
                }
                //if (txtvoucherno5.Text == "")
                //{
                //    MessageBox.Show("Please Enter Voucher No");
                //    return;
                //}
                if (txtdebit.Text == "")
                {
                    txtdebit.Text = "0";
                   // MessageBox.Show("Please Enter Debit Amount");
                   // return;
                }
                if (txtcredit.Text == "")
                {
                    txtcredit.Text = "0";
                   // MessageBox.Show("Please Enter Credit Amount");
                   // return;
                }
                if (txtdesc5.Text == "")
                {
                    MessageBox.Show("Please Enter Description");
                    return;
                }
                //foreach (DataGridViewRow dr in dataGridView6.Rows)
                //{
                //    string val = "";
                //    try
                //    {
                //        val = dr.Cells["Voucherno"].Value.ToString();
                //    }
                //    catch (Exception ex)
                //    {


                //    }
                //    if (val == txtvoucherno5.Text)
                //    {
                //        MessageBox.Show("Voucher no already exist");
                //        txtvoucherno5.Focus();
                //        return;
                //    }
                //}
                string vno = "";
                if (dt.Rows.Count > 0)
                {
                    vno = dt.Rows[0]["Voucher No"].ToString();
                    if (vno == txtvoucherno5.Text.Trim())
                    { }
                    else
                    {
                        MessageBox.Show("Voucher no do not match");
                        return;
                    }
                }
                DataSet dsacount = new System.Data.DataSet();
                string acount = "", acountname = "", pid = "";
                if (cmbtype.Text == "Payable Accounts")
                {
                    dsacount = objCore.funGetDataSet("SELECT     dbo.ChartofAccounts.Name, dbo.Supplier.payableaccountid FROM         dbo.ChartofAccounts INNER JOIN                      dbo.Supplier ON dbo.ChartofAccounts.Id = dbo.Supplier.payableaccountid where dbo.Supplier.id='" + cmbaccount.SelectedValue + "'");
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        acount = dsacount.Tables[0].Rows[0]["payableaccountid"].ToString();
                        acountname = dsacount.Tables[0].Rows[0]["Name"].ToString();
                        pid = cmbaccount.SelectedValue.ToString();
                    }
                    dt.Rows.Add(acount, cmbtype.Text, pid, acountname, txtvoucherno5.Text, txtdesc5.Text, txtdebit.Text, txtcredit.Text, cmbaccount.Text, cmbbranch.SelectedValue );
                }
                if (cmbtype.Text == "Employees")
                {
                    dsacount = objCore.funGetDataSet("SELECT     dbo.ChartofAccounts.Name, dbo.Employees.payableaccountid FROM         dbo.ChartofAccounts INNER JOIN                      dbo.Employees ON dbo.ChartofAccounts.Id = dbo.Employees.payableaccountid where dbo.Employees.id='" + cmbaccount.SelectedValue + "'");
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        acount = dsacount.Tables[0].Rows[0]["payableaccountid"].ToString();
                        acountname = dsacount.Tables[0].Rows[0]["Name"].ToString();
                        pid = cmbaccount.SelectedValue.ToString();
                    }
                    dt.Rows.Add(acount, cmbtype.Text, pid, acountname, txtvoucherno5.Text, txtdesc5.Text, txtdebit.Text, txtcredit.Text, cmbaccount.Text, cmbbranch.SelectedValue );
                }
                if (cmbtype.Text == "GL Accounts")
                {
                    acount = cmbaccount.SelectedValue.ToString();
                    acountname = cmbaccount.Text;
                    pid = "";
                    dt.Rows.Add(acount, cmbtype.Text, pid, acountname, txtvoucherno5.Text, txtdesc5.Text, txtdebit.Text, txtcredit.Text, "", cmbbranch.SelectedValue );
                }
                if (cmbtype.Text == "Receiveable Accounts")
                {
                    dsacount = objCore.funGetDataSet("SELECT     dbo.ChartofAccounts.Name, dbo.Customers.Chartaccountid FROM         dbo.ChartofAccounts INNER JOIN                      dbo.Customers ON dbo.ChartofAccounts.Id = dbo.Customers.Chartaccountid where dbo.Customers.id='" + cmbaccount.SelectedValue + "'");
                    //dsacount = objCore.funGetDataSet("select * from Customers where id='" + cmbaccount.SelectedValue + "'");
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        acount = dsacount.Tables[0].Rows[0]["Chartaccountid"].ToString();
                        acountname = dsacount.Tables[0].Rows[0]["Name"].ToString();
                        pid = cmbaccount.SelectedValue.ToString();
                    }
                    dt.Rows.Add(acount, cmbtype.Text, pid, acountname, txtvoucherno5.Text, txtdesc5.Text, txtdebit.Text, txtcredit.Text, cmbaccount.Text, cmbbranch.SelectedValue );
                }
                
                dataGridView6.DataSource = dt;
                dataGridView6.Columns[0].Visible = false;
                dataGridView6.Columns[1].Visible = false;
                dataGridView6.Columns[2].Visible = false;
                cmbaccount.Focus();
                txtcredit.Text = "0";
                txtdebit.Text = "0";
                try
                {
                    lbltotaljv.Text = "Total Debit:" + dt.AsEnumerable().Sum(x => x.Field<double>("Debit")) + "  Total Credit: " + dt.AsEnumerable().Sum(x => x.Field<double>("Credit"));
                }
                catch (Exception ex)
                {
                }

                //clear5();
            }
            catch (Exception ex)
            {


            }
        }

        private void txtdebit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtdebit.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtdebit.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        txtdebit.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void txtcredit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtcredit.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtcredit.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        txtcredit.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbaccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            //DataSet dscode = new System.Data.DataSet();
            //string q = "SELECT      AccountCode  FROM         ChartofAccounts where id='" + cmbaccount.SelectedValue + "'";
            //dscode = objCore.funGetDataSet(q);
            //if (dscode.Tables[0].Rows.Count > 0)
            //{
            //    txtcode.Text = dscode.Tables[0].Rows[0][0].ToString();
            //}
        }

        private void cmbaccount_SelectedValueChanged(object sender, EventArgs e)
        {

        }


        private void GetVoucherNo()
        {
            string Vouch="";
            DataSet dsvoucher = new System.Data.DataSet();
            string q = "SELECT     MAX(CONVERT(INT, id)+1) AS Expr1 FROM         jv";
            dsvoucher = objCore.funGetDataSet(q);
            if (dsvoucher.Tables[0].Rows.Count > 0)
            {
                Vouch = dsvoucher.Tables[0].Rows[0][0].ToString();
                if (Vouch == "")
                {
                    Vouch = "JV-1";
                }
                else
                {
                    Vouch = "JV-" + Vouch;
                }
            }
            else
            {
                Vouch = "JV-1";
            }

            txtvoucherno5.Text = Vouch;
        }

        private void button29_Click(object sender, EventArgs e)
        {
            DialogResult drl = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drl == DialogResult.No)
            {
                return;
            }
            try
            {
                double debit = 0;
                double credit = 0;
                if (dataGridView6.Rows.Count > 0)
                { }
                else
                {
                    return;
                }
                foreach (DataGridViewRow dr in dataGridView6.Rows)
                {
                    string val = "";
                    try
                    {
                        val = dr.Cells["Debit"].Value.ToString();
                    }
                    catch (Exception ex)
                    {


                    }
                    if (val == "")
                    {
                        val = "0";
                    }
                    double temp = Convert.ToDouble(val);
                    debit = debit +temp;
                    try
                    {
                        val = dr.Cells["Credit"].Value.ToString();
                    }
                    catch (Exception ex)
                    {


                    }
                    if (val == "")
                    {
                        val = "0";
                    }
                    credit = credit + Convert.ToDouble(val);
                }
                debit = Math.Round(debit, 3);
                credit = Math.Round(credit, 3);
                if (credit == debit)
                {
                    string voucher = "";


                    if (button29.Text == "Save")
                    {
                        DataSet dsvoucher = new System.Data.DataSet();
                        string q = "SELECT     MAX(CONVERT(INT, id)+1) AS Expr1 FROM         jv";
                        dsvoucher = objCore.funGetDataSet(q);
                        if (dsvoucher.Tables[0].Rows.Count > 0)
                        {
                            voucher = dsvoucher.Tables[0].Rows[0][0].ToString();
                            if (voucher == "")
                            {
                                voucher = "JV-1";
                            }
                            else
                            {
                                voucher="JV-"+voucher;
                            }
                        }
                        else
                        {
                            voucher = "JV-1";
                        }

                        foreach (DataGridViewRow dr in dataGridView6.Rows)
                        {
                            string table = "";
                            // if (txtvoucherno3.Text == string.Empty)
                            {
                                DataSet dsbarcode = new DataSet();

                                objCore = new classes.Clsdbcon();
                                int idd = 0;
                                DataSet dss = new DataSet();
                                dss = objCore.funGetDataSet("select max(id) as id from JournalAccount");
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    string i = dss.Tables[0].Rows[0][0].ToString();
                                    if (i == string.Empty)
                                    {
                                        i = "0";
                                    }
                                    idd = Convert.ToInt32(i) + 1;
                                }
                                else
                                {
                                    idd = 1;
                                }

                                double balance = 0;
                                double newbalance = 0;
                                string debt = dr.Cells["Debit"].Value.ToString();
                                string credt = dr.Cells["Credit"].Value.ToString();
                                if (debt == "")
                                {
                                    debt = "0";
                                }
                                if (credt == "")
                                {
                                    credt = "0";
                                }
                                newbalance = (balance + Convert.ToDouble(debt)) - Convert.ToDouble(credt); ;
                                newbalance = Math.Round(newbalance, 2);
                                if (dr.Cells["Account Type"].Value.ToString() == "GL Accounts")
                                {
                                    q = "insert into JournalAccount (id,Date,PayableAccountId,Voucherno,Description,Debit,Credit,Balance,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','" + dr.Cells["BranchID"].Value.ToString().Replace("'", "''") + "')";
                                    table = "JournalAccount";
                                }
                                if (dr.Cells["Account Type"].Value.ToString() == "Employees")
                                {
                                    dss = new DataSet();
                                    dss = objCore.funGetDataSet("select max(id) as id from EmployeesAccount");
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        string i = dss.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idd = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idd = 1;
                                    }
                                    //q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker4.Text + "','" + cmbsupplier2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','Bank','" + cmbbranchbpv.SelectedValue + "')";
                                    q = "insert into EmployeesAccount (Id,Date,EmployeeId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["pid"].Value.ToString() + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','','','','" + dr.Cells["BranchID"].Value.ToString().Replace("'", "''") + "')";
                                    table = "EmployeesAccount";
                                }
                                if (dr.Cells["Account Type"].Value.ToString() == "Payable Accounts")
                                {
                                    dss = new DataSet();
                                    dss = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        string i = dss.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idd = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idd = 1;
                                    }
                                    //q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker4.Text + "','" + cmbsupplier2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','Bank','" + cmbbranchbpv.SelectedValue + "')";
                                    q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["pid"].Value.ToString() + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','','','','" + dr.Cells["BranchID"].Value.ToString().Replace("'", "''") + "')";
                                    table = "SupplierAccount";
                                }
                                if (dr.Cells["Account Type"].Value.ToString() == "Receiveable Accounts")
                                {
                                    // q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType,branchid) values('" + iddd + "','" + dateTimePicker2.Text + "','0','" + cmbcustomers.SelectedValue + "','" + txtvoucherno2.Text + "','" + txtdesc2.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "','Cash','" + cmbbranchcrv.SelectedValue + "')";
                                    dss = new DataSet();
                                    dss = objCore.funGetDataSet("select max(id) as id from CustomerAccount");
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        string i = dss.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idd = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idd = 1;
                                    }
                                    q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["pid"].Value.ToString() + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','','" + dr.Cells["BranchID"].Value.ToString().Replace("'", "''") + "')";
                                    table = "SupplierAccount";
                                }
                                int res = objCore.executeQueryint(q);
                                if (res == 0)
                                {

                                }
                                q = "insert into jv (voucherno) values('"+voucher+"')";
                                objCore.executeQuery(q);
                                savesupporting(table, voucher);
                                try
                                {
                                    pictureBox3.Image = null;
                                    imageData = null;
                                }
                                catch (Exception ex)
                                {


                                }
                                //txtvoucherno3.Text = "BRV-" + idd;
                                //Customeraccountbank(txtamount3.Text, "");
                                // MessageBox.Show("Data Added Successfully");
                            }
                        }

                        MessageBox.Show("Data Added Successfully");

                    }
                    if (button29.Text == "Update")
                    {
                        POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                        POSRestaurant.Properties.Settings.Default.Save();
                        string status = objCore.authenticate("update");
                        if (status == "no")
                        {
                            POSRestaurant.classes.Message obj = new classes.Message();
                            obj.Show();
                            return;
                        }
                        voucher = dataGridView6.Rows[0].Cells["voucher no"].Value.ToString();
                        string q = "delete from JournalAccount where voucherno='" + dataGridView6.Rows[0].Cells["voucher no"].Value.ToString() + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                        objCore.executeQuery(q);
                        q = "delete from SupplierAccount where voucherno='" + dataGridView6.Rows[0].Cells["voucher no"].Value.ToString() + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                        objCore.executeQuery(q);
                        q = "delete from EmployeesAccount where voucherno='" + dataGridView6.Rows[0].Cells["voucher no"].Value.ToString() + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                        objCore.executeQuery(q);
                        q = "delete from CustomerAccount where voucherno='" + dataGridView6.Rows[0].Cells["voucher no"].Value.ToString() + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                        objCore.executeQuery(q);
                        foreach (DataGridViewRow dr in dataGridView6.Rows)
                        {
                            string table = "";
                            // if (txtvoucherno3.Text == string.Empty)
                            {
                                DataSet dsbarcode = new DataSet();

                                objCore = new classes.Clsdbcon();
                                int idd = 0;
                                DataSet dss = new DataSet();
                                dss = objCore.funGetDataSet("select max(id) as id from JournalAccount");
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    string i = dss.Tables[0].Rows[0][0].ToString();
                                    if (i == string.Empty)
                                    {
                                        i = "0";
                                    }
                                    idd = Convert.ToInt32(i) + 1;
                                }
                                else
                                {
                                    idd = 1;
                                }
                                q = "select top 1 * from JournalAccount where PayableAccountId='" + dr.Cells["id"].Value.ToString() + "'  order by id desc";
                                DataSet dsacount = new DataSet();
                                string val = "";
                                dsacount = objCore.funGetDataSet(q);
                                if (dsacount.Tables[0].Rows.Count > 0)
                                {
                                    val = dsacount.Tables[0].Rows[0]["Balance"].ToString();
                                }
                                if (val == "")
                                {
                                    val = "0";

                                }
                                double balance = Convert.ToDouble(val);

                                double newbalance = 0;
                                string debt = dr.Cells["Debit"].Value.ToString();
                                string credt = dr.Cells["Credit"].Value.ToString();
                                if (debt == "")
                                {
                                    debt = "0";
                                }
                                if (credt == "")
                                {
                                    credt = "0";
                                }
                                newbalance = (balance + Convert.ToDouble(debt)) - Convert.ToDouble(credt);
                                newbalance = Math.Round(newbalance, 2);
                                if (dr.Cells["Account Type"].Value.ToString() == "GL Accounts")
                                {
                                    objCore.executeQuery(q);
                                    q = "insert into JournalAccount (id,Date,PayableAccountId,Voucherno,Description,Debit,Credit,Balance,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','" + cmbbranchjv.SelectedValue + "')";
                                    table = "JournalAccount";
                                }
                                if (dr.Cells["Account Type"].Value.ToString() == "Employees")
                                {
                                    dss = new DataSet();
                                    dss = objCore.funGetDataSet("select max(id) as id from EmployeesAccount");
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        string i = dss.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idd = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idd = 1;
                                    }
                                    //q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker4.Text + "','" + cmbsupplier2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','Bank','" + cmbbranchbpv.SelectedValue + "')";
                                    q = "insert into EmployeesAccount (Id,Date,EmployeeId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["pid"].Value.ToString() + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','','','','" + cmbbranchjv.SelectedValue + "')";
                                    table = "EmployeesAccount";
                                }
                                if (dr.Cells["Account Type"].Value.ToString() == "Payable Accounts")
                                {
                                    objCore.executeQuery(q);
                                    dss = new DataSet();
                                    dss = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        string i = dss.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idd = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idd = 1;
                                    }
                                    //q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker4.Text + "','" + cmbsupplier2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'", "''") + "','Bank','" + cmbbranchbpv.SelectedValue + "')";
                                    q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["pid"].Value.ToString() + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','','','','" + cmbbranchjv.SelectedValue + "')";
                                    table = "SupplierAccount";
                                }
                                if (dr.Cells["Account Type"].Value.ToString() == "Receiveable Accounts")
                                {
                                    objCore.executeQuery(q);
                                    dss = new DataSet();
                                    dss = objCore.funGetDataSet("select max(id) as id from CustomerAccount");
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        string i = dss.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idd = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idd = 1;
                                    }
                                    q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType,branchid) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["pid"].Value.ToString() + "','" + dr.Cells["id"].Value.ToString() + "','" + voucher + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "','','" + cmbbranchjv.SelectedValue + "')";
                                    table = "CustomerAccount";
                                }
                                objCore.executeQuery(q);
                                savesupporting(table, voucher);
                                try
                                {
                                    pictureBox3.Image = null;
                                    imageData = null;
                                }
                                catch (Exception ex)
                                {


                                }
                                //txtvoucherno3.Text = "BRV-" + idd;
                                //Customeraccountbank(txtamount3.Text, "");
                                // MessageBox.Show("Data Added Successfully");
                            }
                        }
                        MessageBox.Show("Data Updated Successfully");
                    }
                    chartacounts();

                    dt.Clear();
                    dataGridView6.DataSource = dt;
                    button29.Text = "Save";
                    clear5();
                    getdatajournal("", "");

                }
                else
                {
                    MessageBox.Show("Credit and Debit is not equal");

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            getdatajournal("", "yes");
        }

        private void dataGridView6_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (button29.Text == "Update")
            {
                getinforejournaledit("");
            }
            //else
            {
                DataRow dtr = dt.Rows[e.RowIndex];

                dtr.Delete();
                dataGridView6.Refresh();
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                //int indx = dataGridView1.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno.Text;
                    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.branch = cmbbranch.SelectedValue.ToString();
                    obj.name = "Cash Payment Voucher";
                    obj.type = "cpv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno4.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                //int indx = dataGridView4.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno4.Text.Trim();
                    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.branch = cmbbranchbpv.SelectedValue.ToString();
                    obj.name = "Bank Payment Voucher";
                    obj.type = "bpv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno2.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                //int indx = dataGridView2.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno2.Text.Trim();
                    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.branch = cmbbranchcrv.SelectedValue.ToString();
                    obj.name = "Cash Receipt Voucher";
                    obj.type = "crv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno3.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                {
                    string id = txtvoucherno3.Text.Trim();
                    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.branch = cmbbranchbrv.SelectedValue.ToString();
                    obj.name = "Bank Receipt Voucher";
                    obj.type = "brv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView6.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
               // int indx = dataGridView6.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno5.Text.Trim();// dataGridView6.Rows[indx].Cells["Voucherno"].Value.ToString();
                    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.branch = cmbbranchjv.SelectedValue.ToString();
                    obj.name = "Journal Voucher";
                    obj.type = "jv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }
        string jvbranchid = "";
        public void getinfojournal(string voucher, string branchid)
        {
            try
            {
                try
                {
                    cmbbranchjv.SelectedValue = branchid;
                }
                catch (Exception ex)
                {
                    
                   
                }
                dt.Clear();
                DataSet dsgetjournal = new System.Data.DataSet();
                string q = "SELECT     dbo.JournalAccount.PayableAccountId,dbo.JournalAccount.id, dbo.JournalAccount.Date,dbo.JournalAccount.supporting, dbo.JournalAccount.branchid, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description , dbo.ChartofAccounts.AccountCode, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit,                       dbo.JournalAccount.Balance FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.VoucherNo='" + voucher + "' and dbo.JournalAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                dsgetjournal = objCore.funGetDataSet(q);
                for (int i = 0; i < dsgetjournal.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(dsgetjournal.Tables[0].Rows[i]["PayableAccountId"].ToString(), "GL Accounts", "", dsgetjournal.Tables[0].Rows[i]["Name"].ToString(), dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString(), dsgetjournal.Tables[0].Rows[i]["Description"].ToString(), dsgetjournal.Tables[0].Rows[i]["Debit"].ToString(), dsgetjournal.Tables[0].Rows[i]["Credit"].ToString(),"");
                    jvdate = dsgetjournal.Tables[0].Rows[i]["Date"].ToString();
                    jvbranchid = dsgetjournal.Tables[0].Rows[i]["branchid"].ToString();
                    txtvoucherno5.Text = dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString();

                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dsgetjournal.Tables[0].Rows[0]["supporting"]);
                        imageData = data;
                        System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                        pictureBox3.Image = Image.FromStream(mem);
                    }
                    catch (Exception ex)
                    {


                    }
                }
                dsgetjournal = new System.Data.DataSet();
                //q = "SELECT     dbo.JournalAccount.PayableAccountId,dbo.JournalAccount.id, dbo.JournalAccount.Date,dbo.JournalAccount.branchid, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description , dbo.ChartofAccounts.AccountCode, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit,                       dbo.JournalAccount.Balance FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.VoucherNo='" + voucher + "' and dbo.JournalAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                q = "SELECT     dbo.SupplierAccount.PayableAccountId, dbo.SupplierAccount.SupplierId, dbo.SupplierAccount.Date, dbo.SupplierAccount.branchid, dbo.SupplierAccount.supporting, dbo.ChartofAccounts.Name,                       dbo.SupplierAccount.VoucherNo, dbo.SupplierAccount.Description, dbo.ChartofAccounts.AccountCode, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Credit,                       dbo.SupplierAccount.Balance, dbo.Supplier.Name AS supname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.SupplierAccount ON dbo.ChartofAccounts.Id = dbo.SupplierAccount.PayableAccountId LEFT OUTER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.SupplierAccount.VoucherNo='" + voucher + "' and dbo.SupplierAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                dsgetjournal = objCore.funGetDataSet(q);
                for (int i = 0; i < dsgetjournal.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(dsgetjournal.Tables[0].Rows[i]["PayableAccountId"].ToString(), "Payable Accounts", dsgetjournal.Tables[0].Rows[i]["SupplierId"].ToString(), dsgetjournal.Tables[0].Rows[i]["Name"].ToString(), dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString(), dsgetjournal.Tables[0].Rows[i]["Description"].ToString(), dsgetjournal.Tables[0].Rows[i]["Debit"].ToString(), dsgetjournal.Tables[0].Rows[i]["Credit"].ToString(), dsgetjournal.Tables[0].Rows[i]["supname"].ToString());
                    jvdate = dsgetjournal.Tables[0].Rows[i]["Date"].ToString();
                    jvbranchid = dsgetjournal.Tables[0].Rows[i]["branchid"].ToString();
                    txtvoucherno5.Text = dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString();

                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dsgetjournal.Tables[0].Rows[0]["supporting"]);
                        imageData = data;
                        System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                        pictureBox3.Image = Image.FromStream(mem);
                    }
                    catch (Exception ex)
                    {


                    }
                }
                dsgetjournal = new System.Data.DataSet();
                //q = "SELECT     dbo.JournalAccount.PayableAccountId,dbo.JournalAccount.id, dbo.JournalAccount.Date,dbo.JournalAccount.branchid, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description , dbo.ChartofAccounts.AccountCode, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit,                       dbo.JournalAccount.Balance FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.VoucherNo='" + voucher + "' and dbo.JournalAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                q = "SELECT     dbo.EmployeesAccount.PayableAccountId, dbo.EmployeesAccount.EmployeeId, dbo.EmployeesAccount.Date, dbo.EmployeesAccount.branchid, dbo.EmployeesAccount.supporting, dbo.ChartofAccounts.Name,                       dbo.EmployeesAccount.VoucherNo, dbo.EmployeesAccount.Description, dbo.ChartofAccounts.AccountCode, dbo.EmployeesAccount.Debit, dbo.EmployeesAccount.Credit,                       dbo.EmployeesAccount.Balance, dbo.Employees.Name AS supname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.EmployeesAccount ON dbo.ChartofAccounts.Id = dbo.EmployeesAccount.PayableAccountId LEFT OUTER JOIN                      dbo.Employees ON dbo.EmployeesAccount.EmployeeId = dbo.Employees.Id where dbo.EmployeesAccount.VoucherNo='" + voucher + "' and dbo.EmployeesAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                dsgetjournal = objCore.funGetDataSet(q);
                for (int i = 0; i < dsgetjournal.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(dsgetjournal.Tables[0].Rows[i]["PayableAccountId"].ToString(), "Employees", dsgetjournal.Tables[0].Rows[i]["EmployeeId"].ToString(), dsgetjournal.Tables[0].Rows[i]["Name"].ToString(), dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString(), dsgetjournal.Tables[0].Rows[i]["Description"].ToString(), dsgetjournal.Tables[0].Rows[i]["Debit"].ToString(), dsgetjournal.Tables[0].Rows[i]["Credit"].ToString(), dsgetjournal.Tables[0].Rows[i]["supname"].ToString());
                    jvdate = dsgetjournal.Tables[0].Rows[i]["Date"].ToString();
                    jvbranchid = dsgetjournal.Tables[0].Rows[i]["branchid"].ToString();
                    txtvoucherno5.Text = dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString();

                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(dsgetjournal.Tables[0].Rows[0]["supporting"]);
                        imageData = data;
                        System.IO.MemoryStream mem = new System.IO.MemoryStream(data);
                        pictureBox3.Image = Image.FromStream(mem);
                    }
                    catch (Exception ex)
                    {


                    }
                }
                dsgetjournal = new System.Data.DataSet();
                //q = "SELECT     dbo.JournalAccount.PayableAccountId,dbo.JournalAccount.id, dbo.JournalAccount.Date,dbo.JournalAccount.branchid, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description , dbo.ChartofAccounts.AccountCode, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit,                       dbo.JournalAccount.Balance FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.VoucherNo='" + voucher + "' and dbo.JournalAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                q = "SELECT     dbo.ChartofAccounts.Name, dbo.ChartofAccounts.AccountCode, dbo.CustomerAccount.CustomersId, dbo.CustomerAccount.Date,                       dbo.CustomerAccount.PayableAccountId, dbo.CustomerAccount.VoucherNo, dbo.CustomerAccount.Description, dbo.CustomerAccount.Debit,                       dbo.CustomerAccount.Credit, dbo.CustomerAccount.Balance, dbo.CustomerAccount.branchid, dbo.Customers.Name AS cusname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.CustomerAccount ON dbo.ChartofAccounts.Id = dbo.CustomerAccount.PayableAccountId LEFT OUTER JOIN                      dbo.Customers ON dbo.CustomerAccount.CustomersId = dbo.Customers.Id where dbo.CustomerAccount.VoucherNo='" + voucher + "' and dbo.CustomerAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                dsgetjournal = objCore.funGetDataSet(q);
                for (int i = 0; i < dsgetjournal.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(dsgetjournal.Tables[0].Rows[i]["PayableAccountId"].ToString(), "Receiveable Accounts", dsgetjournal.Tables[0].Rows[i]["CustomersId"].ToString(), dsgetjournal.Tables[0].Rows[i]["Name"].ToString(), dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString(), dsgetjournal.Tables[0].Rows[i]["Description"].ToString(), dsgetjournal.Tables[0].Rows[i]["Debit"].ToString(), dsgetjournal.Tables[0].Rows[i]["Credit"].ToString(), dsgetjournal.Tables[0].Rows[i]["cusname"].ToString());
                    jvdate = dsgetjournal.Tables[0].Rows[i]["Date"].ToString();
                    jvbranchid = dsgetjournal.Tables[0].Rows[i]["branchid"].ToString();
                    txtvoucherno5.Text = dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString();
                   
                }
                button29.Text = "Update";
                dataGridView6.DataSource = dt;
                dataGridView6.Columns[0].Visible = false;
                dataGridView6.Columns[1].Visible = false;
                dataGridView6.Columns[2].Visible = false;
            }
            catch (Exception ex)
            {


            }
        }
        private void button26_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //    if (dr == DialogResult.No)
            //    {
            //        return;
            //    }
            //    int indx = dataGridView5.CurrentCell.RowIndex;
            //    if (indx >= 0)
            //    {
            //        string id = dataGridView5.Rows[indx].Cells[3].Value.ToString();
            //        getinfojournal(id);
            //        button29.Text = "Update";

            //    }
            //}
            //catch (Exception ex)
            //{


            //}
        }

        private void button35_Click(object sender, EventArgs e)
        {
            clear5();

        }

        private void button36_Click(object sender, EventArgs e)
        {
            dt.Clear();
            dataGridView6.DataSource = dt;
            button29.Text = "Save";
            try
            {
                pictureBox3.Image = null;
                imageData = null;
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public void deletejurnalcharts(string voucher)
        {
            try
            {
                DataSet dscharts = new System.Data.DataSet();
                string q = "select * from ChartofAccounts";
                dscharts = objCore.funGetDataSet(q);
                for (int i = 0; i < dscharts.Tables[0].Rows.Count; i++)
                {
                    updatejournal(dscharts.Tables[0].Rows[i]["id"].ToString(), voucher);
                }
            }
            catch (Exception ex)
            {


            }

        }
        public void updatejournal(string id, string voucher)
        {
            try
            {
                string q = "select * from JournalAccount where PayableAccountId ='" + id + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (dss.Tables[0].Rows[i]["voucherno"].ToString() == voucher)
                    {
                        if (i == 0)
                        {
                            debit = "0";
                            credit = "0";
                            blnce = "0";
                            if (debit == "")
                            {
                                debit = "0";
                            }
                            if (credit == "")
                            {
                                credit = "0";
                            }
                            if (blnce == "")
                            {
                                blnce = "0";
                            }

                            newbalance = Convert.ToDouble(blnce);
                        }
                        else
                        {
                            debit = "0";
                            credit = "0";
                            blnce = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            if (debit == "")
                            {
                                debit = "0";
                            }
                            if (credit == "")
                            {
                                credit = "0";
                            }
                            if (blnce == "")
                            {
                                blnce = "0";
                            }
                            newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit);

                        }
                        newbalance = Math.Round(newbalance, 2);
                        objCore = new classes.Clsdbcon();
                        q = "update JournalAccount set     Debit='" + debit + "' ,credit='" + credit + "' , Balance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() + "'";
                        objCore.executeQuery(q);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("delete journal  error");
            }
        }
        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.Properties.Settings.Default.formname = "Vouchers";
                POSRestaurant.Properties.Settings.Default.Save();
                string status = objCore.authenticate("delete");
                if (status == "no")
                {
                    POSRestaurant.classes.Message obj = new classes.Message();
                    obj.Show();
                    return;
                }
                if (dataGridView6.Rows.Count <= 0)
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }

                DialogResult dr = MessageBox.Show("Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
                int indx = 0;// dataGridView6.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string voucher = dataGridView6.Rows[indx].Cells["Voucher No"].Value.ToString();
                    string q = "update JournalAccount set     Debit='0' ,credit='0'  where Voucherno='" + voucher + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    objCore.executeQuery(q);
                    q = "update SupplierAccount set     Debit='0' ,credit='0'  where Voucherno='" + voucher + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    objCore.executeQuery(q);
                    q = "update CustomerAccount set     Debit='0' ,credit='0'  where Voucherno='" + voucher + "' and branchid='" + cmbbranchjv.SelectedValue + "'";
                    objCore.executeQuery(q);
                    //deletejurnalcharts(voucher);
                    //chartacounts();
                    //getdatajournal("", "");
                }
                MessageBox.Show("Deleted Successfully");
                clear5();
                dt.Clear();

            }
            catch (Exception ex)
            {


            }
        }

        private void cmbbranchjv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refreshaccounts();

           // getdatajournal("", "");
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refreshaccounts();
            //getdata("", "");

        }

        private void cmbbranchbpv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refreshaccounts();

           // getdatabank("", "");

        }

        private void cmbbranchcrv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refreshaccounts();

            //getdatareceipt("", "");

        }

        private void cmbbranchbrv_SelectedIndexChanged(object sender, EventArgs e)
        {
            //refreshaccounts();

           // getdatareceiptbank("", "");

        }

        private void button37_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdcustomers.Checked == true)
            {
                rdglbrv.Checked = false;
                refereshbrv();
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdglbrv.Checked == true)
            {
                rdcustomers.Checked = false;
                refereshbrv();
            }
        }

        private void rdsupplier_CheckedChanged(object sender, EventArgs e)
        {
            if (rdsupplier.Checked == true)
            {
                rdgl.Checked = false;
                rdemployeesbpv.Checked = false;
                cmbinvoicebpv.Visible = true;
                lblinvoicebpv.Visible = true;
                refereshbpv();
            }
            else
            {
                cmbinvoicebpv.Visible = false;
                lblinvoicebpv.Visible = false;
            }
        }

        private void rdgl_CheckedChanged(object sender, EventArgs e)
        {
            if (rdgl.Checked == true)
            {
                rdsupplier.Checked = false;
                rdemployeesbpv.Checked = false;
                refereshbpv();
            }
        }

        private void rdsupplier1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdsupplier1.Checked == true)
            {
                lblemployeecpv.Visible = false;
                rdglcpv.Checked = false;
                rdemployees.Checked = false;
                cmbinvoicenocpv.Visible = true;
                lblinvoice.Visible = true;
                refereshcpv();
            }
            else
            {
                cmbinvoicenocpv.Visible = false;
                lblinvoice.Visible = false;
            }
        }

        private void rdglcpv_CheckedChanged(object sender, EventArgs e)
        {
            if (rdglcpv.Checked == true)
            {
                lblemployeecpv.Visible = false;
                rdsupplier1.Checked = false;
                rdemployees.Checked = false;
                refereshcpv();
            }
        }

        private void rdcustomers1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdcustomers1.Checked == true)
            {
                rdglcrv.Checked = false;
                refereshcrv();
            }
        }

        private void rdglcrv_CheckedChanged(object sender, EventArgs e)
        {
            if (rdglcrv.Checked == true)
            {
                rdcustomers1.Checked = false;
                refereshcrv();
            }
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            refereshjvaccount();
        }

        private void cmbtype_SelectedValueChanged(object sender, EventArgs e)
        {
            refereshjvaccount();
        }

        private void cmbsupplier2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdsupplier.Checked == true)
                {
                    getinvoices(cmbsupplier2.SelectedValue.ToString(), "BPV");
                    double blnc = getsuplierbalance(cmbsupplier2.SelectedValue.ToString(), cmbbranchbpv.SelectedValue.ToString());
                    lblsuplierbalancebpv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
                else if (rdemployeesbpv.Checked == true)
                {
                    double blnc = getemployeesbalance(cmbsupplier2.SelectedValue.ToString(), cmbbranchbpv.SelectedValue.ToString());
                    lblsuplierbalancebpv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
                else
                {
                    lblsuplierbalancebpv.Text = "";
                }
            }
            catch (Exception ex)
            {
               
            }
        }

        private void button35_Click_1(object sender, EventArgs e)
        {
            clear5();
        }

        private void button36_Click_1(object sender, EventArgs e)
        {
            dt.Clear();
            dataGridView6.DataSource = dt;
            button29.Text = "Save";
        }

        private void button37_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbtype_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            refereshjvaccount();
        }

        private void cmbtype_SelectedValueChanged_1(object sender, EventArgs e)
        {
            refereshjvaccount();
        }

        private void cmbtype_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            refereshjvaccount();
        }

        private void txtdebit_Enter(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.SelectAll();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno2.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                //int indx = dataGridView2.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno2.Text.Trim();
                    POSRestaurant.Reports.Voucher.frmReceiving obj = new Reports.Voucher.frmReceiving();
                    obj.id = id;
                    obj.branch = cmbbranchcrv.SelectedValue.ToString();
                    obj.name = "Cash Receipt Voucher";
                    obj.type = "crv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno3.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                {
                    string id = txtvoucherno3.Text.Trim();
                    POSRestaurant.Reports.Voucher.frmReceiving obj = new Reports.Voucher.frmReceiving();
                    obj.id = id;
                    obj.branch = cmbbranchbrv.SelectedValue.ToString();
                    obj.name = "Bank Receipt Voucher";
                    obj.type = "brv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void rdemployees_CheckedChanged(object sender, EventArgs e)
        {
            if (rdemployees.Checked == true)
            {
                rdglcpv.Checked = false;
                rdsupplier1.Checked = false;
                dateTimePickeremployeecpv.Visible = true;
                lblemployeecpv.Visible = true;
                refereshcpv();
            }
            else
            {
                lblemployeecpv.Visible = false;
                dateTimePickeremployeecpv.Visible = false;
            }
        }

        private void rdemployeesbpv_CheckedChanged(object sender, EventArgs e)
        {
            if (rdemployeesbpv.Checked == true)
            {
                rdsupplier.Checked = false;
                rdgl.Checked = false;
                dateTimePickeremployeebpv.Visible = true;
                lblemployee.Visible = true;
                refereshbpv();
            }
            else
            {
                lblemployee.Visible = false;
                dateTimePickeremployeebpv.Visible = false;
            }
        }

        private void cmbcashaccount4_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                double blnc = getcashbalance(cmbcashaccount4.SelectedValue.ToString(), cmbbranchbpv.SelectedValue.ToString());
                lblbpv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbcashaccount2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                double blnc = getcashbalance(cmbcashaccount2.SelectedValue.ToString(), cmbbranchcrv.SelectedValue.ToString());
                lblcrv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbcustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdcustomers1.Checked == true)
                {
                    double blnc = getcustomerbalance(cmbcustomers.SelectedValue.ToString(), cmbbranchcrv.SelectedValue.ToString());
                    lblcustomercrv.Text = "Balance:  " + String.Format("{0:0.##}", blnc.ToString());
                }
                else
                {
                    lblcustomercrv.Text = "";
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (rdsupplier1.Checked == true)
            {
                cpvledger(dateTimePicker6.Text, dateTimePicker7.Text,cmbsupplier.SelectedValue.ToString(),cmbsupplier.Text,cmbbranch.SelectedValue.ToString(),cmbbranch.Text);
            }
            if (rdemployees.Checked == true)
            {
                cpvledgeremployees(dateTimePicker6.Text, dateTimePicker7.Text, cmbsupplier.SelectedValue.ToString(), cmbsupplier.Text, cmbbranch.SelectedValue.ToString(), cmbbranch.Text);
            }
        }

        protected void cpvledgeremployees(string start,string end,string employeeid,string employeename,string branchid,string branchname)
        {
            try
            {
               
                DataTable dt = new DataTable();
                POSRestaurant.Reports.Statements.rptpayable rptDoc = new Reports.Statements.rptpayable();
                POSRestaurant.Reports.Statements.dspayablebank dsrpt = new Reports.Statements.dspayablebank();
                dt.TableName = "Crystal Report";
                dt = getAllOrderscpvemployees(employeeid, start, end,employeename,branchid,branchname);
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

                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);

                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "For the period of " + Convert.ToDateTime(start).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(end).ToString("dd-MM-yyyy"));

                rptDoc.SetParameterValue("branch", branchname);
                rptDoc.SetParameterValue("statement", "Employees Statement");
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

            }
        }
        public double bfremployees(string csid,string start,string employeeid,string branchid)
        {
            double bf = 0;
            try
            {
                string q = "";
                
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         EmployeesAccount where EmployeeId='" + employeeid + "' and branchid='" + branchid + "' and date <'" + start + "'  ";

                
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
        public DataTable getAllOrderscpvemployees(string employeeid,string start,string end,string employeename,string branchid,string branchname)
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


                bf = bfremployees("",start, employeeid,branchid);


                if (logo == "")
                {
                    //string date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString();
                    if (bf >= 0)
                    {
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", employeename, "For the period of " + start + " to " + end, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", employeename, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                        }
                    }
                    else
                    {
                        bf = System.Math.Abs(bf);
                        if (logo == "")
                        {

                            dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", employeename, "For the period of " + start + " to " + end, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", employeename, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                        }
                    }
                }
                else
                {
                    if (bf >= 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", employeename, "For the period of " + start + " to " + end, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(start, "", "Balance B/F", bf, "0", bf, "", employeename, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                        }

                    }
                    else
                    {
                        bf = System.Math.Abs(bf);
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", employeename, "For the period of " + start + " to " + end, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(start, "", "Balance B/F", "0", bf, bf, "", cmbsupplier.Text, "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);
                        }
                    }


                }

                q = "SELECT     dbo.EmployeesAccount.Balance AS CurrentBalance, dbo.EmployeesAccount.Credit, dbo.EmployeesAccount.Debit, dbo.EmployeesAccount.Description, dbo.EmployeesAccount.VoucherNo,                       dbo.EmployeesAccount.Date, dbo.Employees.Name AS accountname FROM         dbo.EmployeesAccount INNER JOIN                      dbo.Employees ON dbo.EmployeesAccount.EmployeeId = dbo.Employees.Id INNER JOIN                      dbo.ChartofAccounts ON dbo.EmployeesAccount.PayableAccountId = dbo.ChartofAccounts.Id where  EmployeesAccount.EmployeeId='" + employeeid + "' and  (EmployeesAccount.Date between '" + start + "' and '" + end + "') and  dbo.EmployeesAccount.branchid='" + branchid + "' order by dbo.EmployeesAccount.Date,dbo.EmployeesAccount.VoucherNo";

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

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + start + " to " + end, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + start + " to " + end, dscompany.Tables[0].Rows[0]["logo"]);


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }

        private void button23_Click_1(object sender, EventArgs e)
        {
             if (rdsupplier.Checked == true)
            {
                cpvledger(dateTimePicker8.Text, dateTimePicker9.Text,cmbsupplier2.SelectedValue.ToString(),cmbsupplier2.Text,cmbbranchbpv.SelectedValue.ToString(),cmbbranchbpv.Text);
            }
            if (rdemployeesbpv.Checked == true)
            {
                cpvledgeremployees(dateTimePicker8.Text, dateTimePicker9.Text, cmbsupplier2.SelectedValue.ToString(), cmbsupplier2.Text, cmbbranchbpv.SelectedValue.ToString(), cmbbranchbpv.Text);
            }
        }
        public byte[] imageData = null;

        private void vButton1_Click(object sender, EventArgs e)
        {

        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                Image File;
                OpenFileDialog ofd = new OpenFileDialog();
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    imageData = null;
                    FileInfo fInfo = new FileInfo(ofd.FileName);
                    long numBytes = fInfo.Length;
                    FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fStream);
                    imageData = br.ReadBytes((int)numBytes);

                    File = Image.FromFile(ofd.FileName);
                    pictureBox2.Image = File;

                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
               
                Image File;
                OpenFileDialog ofd = new OpenFileDialog();
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    imageData = null;
                    FileInfo fInfo = new FileInfo(ofd.FileName);
                    long numBytes = fInfo.Length;
                    FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fStream);
                    imageData = br.ReadBytes((int)numBytes);

                    File = Image.FromFile(ofd.FileName);
                    pictureBox1.Image = File;

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {

                Image File;
                OpenFileDialog ofd = new OpenFileDialog();
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    imageData = null;
                    FileInfo fInfo = new FileInfo(ofd.FileName);
                    long numBytes = fInfo.Length;
                    FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                    BinaryReader br = new BinaryReader(fStream);
                    imageData = br.ReadBytes((int)numBytes);

                    File = Image.FromFile(ofd.FileName);
                    pictureBox3.Image = File;

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbinvoicenocpv_SelectedIndexChanged(object sender, EventArgs e)
        {

           
        }

        private void cmbinvoicebpv_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cmbinvoicebpv_TextChanged(object sender, EventArgs e)
        {
            try
            {
                getbalances(cmbsupplier2.SelectedValue.ToString(), cmbinvoicebpv.SelectedValue.ToString(), "BPV");
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbinvoicenocpv_TextChanged(object sender, EventArgs e)
        {
            try
            {
                getbalances(cmbsupplier.SelectedValue.ToString(), cmbinvoicenocpv.SelectedValue.ToString(), "CPV");
            }
            catch (Exception ex)
            {


            }
        }

        private void button26_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                //int indx = dataGridView2.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno.Text.Trim();
                    POSRestaurant.Reports.Voucher.frmReceiving obj = new Reports.Voucher.frmReceiving();
                    obj.id = id;
                    obj.branch = cmbbranchcrv.SelectedValue.ToString();
                    obj.name = "Cash Payment Voucher";
                    obj.type = "cpv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtvoucherno4.Text.Trim() == "")
                {
                    MessageBox.Show("Please Open Voucher First");
                    return;
                }
                //int indx = dataGridView2.CurrentCell.RowIndex;
                //if (indx >= 0)
                {
                    string id = txtvoucherno4.Text.Trim();
                    POSRestaurant.Reports.Voucher.frmReceiving obj = new Reports.Voucher.frmReceiving();
                    obj.id = id;
                    obj.branch = cmbbranchcrv.SelectedValue.ToString();
                    obj.name = "Bank Payment Voucher";
                    obj.type = "bpv";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }
        
    }
}
