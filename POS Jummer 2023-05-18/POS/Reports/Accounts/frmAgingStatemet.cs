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
    public partial class frmAgingStatemet : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmAgingStatemet()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Supplier  order by name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All Suppliers";
                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "name";
                cmbsupplier.Text = "All Suppliers";

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
                if (cmbbranchjv.Text == "All")
                {

                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where  date <'" + dateTimePicker1.Text + "'   and SupplierId='" + csid + "'";

                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where SupplierId='" + csid + "'  and branchid='" + cmbbranchjv.SelectedValue + "'  and date <'" + dateTimePicker1.Text + "'  ";

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
                cmbbranchjv.SelectedItem = "All";
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


                POSRestaurant.Reports.Accounts.rptAging rptDoc = new Accounts.rptAging();
                POSRestaurant.Reports.Accounts.dsAging dsrpt = new Accounts.dsAging();
                
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                
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
                rptDoc.SetParameterValue("date", "Date: " + Convert.ToDateTime(dateTimePicker1.Text).ToString("dd-MM-yyyy"));

               // rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
                rptDoc.SetParameterValue("statement", "Aging Report");
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public class supplieraccountClass
        {
            public int SupplierId { get; set; }
            public DateTime Date { get; set; }
            public int PayableAccountId { get; set; }
            public string VoucherNo { get; set; }
            public string CheckNo { get; set; }
            public string CheckDate { get; set; }
            public string Description { get; set; }
            public double Debit { get; set; }
            public double Credit { get; set; }
            public int branchid { get; set; }
            public string invoiceno { get; set; }
        }
        List<supplieraccountClass> ListSupplierAccount = new List<supplieraccountClass>();
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Limit", typeof(double));
                dtrpt.Columns.Add("InvoiceAmount", typeof(double));
                dtrpt.Columns.Add("PaidAmount", typeof(double));
                dtrpt.Columns.Add("Balance", typeof(double));
                dtrpt.Columns.Add("7", typeof(double));
                dtrpt.Columns.Add("15", typeof(double));
                dtrpt.Columns.Add("22", typeof(double));
                dtrpt.Columns.Add("29", typeof(double));
                dtrpt.Columns.Add("90", typeof(double));
                dtrpt.Columns.Add("91", typeof(double));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                dtrpt.Columns.Add("InvoiceNo", typeof(string));
                dtrpt.Columns.Add("Uncleared", typeof(double));
                dtrpt.Columns.Add("Balancewithchk", typeof(double));
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
                if (cmbsupplier.Text == "All Suppliers")
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT        SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) AS balance, dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id WHERE        (dbo.SupplierAccount.Date <= '" + dateTimePicker1.Text + "') GROUP BY dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId HAVING        (SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) > 0)";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) AS balance, dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id WHERE        (dbo.SupplierAccount.Date <= '" + dateTimePicker1.Text + "') and  (dbo.SupplierAccount.branchid = '" + cmbbranchjv.SelectedValue + "')  GROUP BY dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId HAVING        (SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) > 0)";

                    }
                }
                else
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT        SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) AS balance, dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id WHERE        (dbo.SupplierAccount.Date <= '" + dateTimePicker1.Text + "') and  (dbo.SupplierAccount.SupplierId = '" + cmbsupplier.SelectedValue + "')  GROUP BY dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId HAVING        (SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) > 0)";

                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) AS balance, dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id WHERE        (dbo.SupplierAccount.Date <= '" + dateTimePicker1.Text + "') and  (dbo.SupplierAccount.branchid = '" + cmbbranchjv.SelectedValue + "')  and  (dbo.SupplierAccount.SupplierId = '" + cmbsupplier.SelectedValue + "')  GROUP BY dbo.SupplierAccount.invoiceno, dbo.Supplier.Name,dbo.Supplier.CreditLimit, dbo.SupplierAccount.SupplierId HAVING        (SUM(dbo.SupplierAccount.Credit - dbo.SupplierAccount.Debit) > 0)";

                    }
                }
                DataSet dscs = new System.Data.DataSet();
                dscs = objCore.funGetDataSet(q);
                double seven = 0, fourteen = 0, twentyone = 0, twentyeight = 0, ninety = 0, ninetyplus = 0, invoiceamount = 0, paid = 0, balance = 0, uncleared = 0, balancewithchk = 0;
                double creditlimit = 0;
                for (int l = 0; l < dscs.Tables[0].Rows.Count; l++)
                {
                    ListSupplierAccount = new List<supplieraccountClass>();
                    try
                    {
                        q = "SELECT        TOP (200) Id, Date, SupplierId, PayableAccountId, VoucherNo, CheckNo,cast( CheckDate as varchar(100)) as CheckDate, Description, Debit, Credit, Balance, EntryType, branchid, Status, supporting, Approveduserid, uploadstatus, invoiceno FROM            SupplierAccount where invoiceno='" + dscs.Tables[0].Rows[l]["invoiceno"] + "' and SupplierId='" + dscs.Tables[0].Rows[l]["SupplierId"] + "' order by credit desc";
                        DataSet dsaccount = new System.Data.DataSet();
                        dsaccount = objCore.funGetDataSet(q);
                        try
                        {

                            IList<supplieraccountClass> data = dsaccount.Tables[0].AsEnumerable().Select(row =>
                                new supplieraccountClass
                                {
                                    SupplierId = row.Field<int>("SupplierId"),
                                    Date = row.Field<DateTime>("Date"),
                                    PayableAccountId = row.Field<int>("PayableAccountId"),
                                    VoucherNo = row.Field<string>("VoucherNo"),
                                    CheckNo = row.Field<string>("CheckNo"),
                                    CheckDate = row.Field<string>("CheckDate"),
                                    Description = row.Field<string>("Description"),
                                    Debit = row.Field<double>("Debit"),
                                    Credit = row.Field<double>("Credit"),
                                    branchid = row.Field<int>("branchid"),
                                    invoiceno = row.Field<string>("invoiceno")

                                }).ToList();
                            ListSupplierAccount = data.ToList();

                        }
                        catch (Exception ex)
                        {


                        }
                        DateTime invoicedate = DateTime.Now;
                        seven = 0; fourteen = 0; twentyone = 0; twentyeight = 0; ninety = 0; ninetyplus = 0; invoiceamount = 0; paid = 0; balance = 0;uncleared=0;
                       
                        string temp = dscs.Tables[0].Rows[l]["CreditLimit"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        creditlimit = Convert.ToDouble(temp);
                        invoiceamount = ListSupplierAccount.Sum(x => x.Credit);
                        invoicedate = ListSupplierAccount.Where(x => x.Credit > 0).ToList()[0].Date;
                        paid = ListSupplierAccount.Sum(x => x.Debit);
                        balance = invoiceamount - paid;

                        TimeSpan ts = Convert.ToDateTime(dateTimePicker1.Text) - invoicedate;
                        if (creditlimit == 0)
                        {
                            seven = balance;
                        }
                        else
                        {
                            if ((60 - ts.Days) <= 7)
                            {
                                seven = balance;
                            }
                            if ((60 - ts.Days) > 7 && (60 - ts.Days) <= 14)
                            {
                                fourteen = balance;
                            }
                            if ((60 - ts.Days) > 14 && (60 - ts.Days) <= 21)
                            {
                                twentyone = balance;
                            }
                            if ((60 - ts.Days) > 21 && (60 - ts.Days) <= 28)
                            {
                                twentyeight = balance;
                            }
                            if ((60 - ts.Days) > 28 && (60 - ts.Days) <= 90)
                            {
                                ninety = balance;
                            }
                            if ((60 - ts.Days) > 90)
                            {
                                ninetyplus = balance;
                            }
                        }
                        
                        
                        foreach (var item in ListSupplierAccount)
                        {

                            if (item.Debit > 0)
                            {
                                
                                if (item.VoucherNo.Contains("BPV"))
                                {
                                    uncleared = uncleared + getuncleared(item.VoucherNo);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    
                    balancewithchk = balance + uncleared;
                    dtrpt.Rows.Add(dscs.Tables[0].Rows[l]["Name"], creditlimit, invoiceamount, paid, balance, seven, fourteen, twentyone, twentyeight, ninety, ninetyplus, dscompany.Tables[0].Rows[0]["logo"], dscs.Tables[0].Rows[l]["invoiceno"],uncleared,balancewithchk);

                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        protected double getuncleared(string voucherno)
        {
            double val = 0;

            try
            {
                string q = "select ClearanceStatus,credit from BankAccountPaymentSupplier where voucherno='" + voucherno + "'";
                DataSet ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["ClearanceStatus"].ToString() == "Pending")
                    {
                        val = Convert.ToDouble(ds.Tables[0].Rows[0]["credit"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return val;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbsupplier.Text == "")
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
            fill();
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
    }
}
