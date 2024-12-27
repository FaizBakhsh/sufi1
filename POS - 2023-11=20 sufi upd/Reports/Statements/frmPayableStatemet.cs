using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace POSRestaurant.Reports.Statements
{
    public partial class frmPayableStatemet : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmPayableStatemet()
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


                POSRestaurant.Reports.Statements.rptpayable rptDoc = new rptpayable();
                POSRestaurant.Reports.Statements.dspayablebank dsrpt = new dspayablebank();
                
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
                rptDoc.SetParameterValue("statement", "Payable Statement");
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
                if (cmbsupplier.Text == "All Suppliers")
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT DISTINCT dbo.SupplierAccount.SupplierId, dbo.Supplier.Name FROM         dbo.SupplierAccount INNER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id ";
                    }
                    else
                    {
                        q = "SELECT DISTINCT dbo.SupplierAccount.SupplierId, dbo.Supplier.Name FROM         dbo.SupplierAccount INNER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.SupplierAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                }
                else
                {
                    if (cmbbranchjv.Text == "All")
                    {
                        q = "SELECT DISTINCT dbo.SupplierAccount.SupplierId, dbo.Supplier.Name FROM         dbo.SupplierAccount INNER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.SupplierAccount.SupplierId='" + cmbsupplier.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT DISTINCT dbo.SupplierAccount.SupplierId, dbo.Supplier.Name FROM         dbo.SupplierAccount INNER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.SupplierAccount.SupplierId='" + cmbsupplier.SelectedValue + "' and dbo.SupplierAccount.branchid='" + cmbbranchjv.SelectedValue + "'";
                    }
                }
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


                if (cmbbranchjv.Text == "All")
                {
                    if (cmbsupplier.Text == "All Suppliers")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.SupplierAccount.branchid, dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                          dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname, dbo.Branch.BranchName FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id INNER JOIN                         dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id INNER JOIN                         dbo.Branch ON dbo.SupplierAccount.branchid = dbo.Branch.Id where  (SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  order by dbo.SupplierAccount.Date,dbo.SupplierAccount.Id";

                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.SupplierAccount.branchid, dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                          dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname, dbo.Branch.BranchName FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id INNER JOIN                         dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id INNER JOIN                         dbo.Branch ON dbo.SupplierAccount.branchid = dbo.Branch.Id where  SupplierAccount.SupplierId='" + cmbsupplier.SelectedValue + "' and (SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') order by dbo.SupplierAccount.Date,dbo.SupplierAccount.Id";
                    }
                }
                else
                {
                    if (cmbsupplier.Text == "All Suppliers")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.SupplierAccount.branchid, dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                          dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname, dbo.Branch.BranchName FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id INNER JOIN                         dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id INNER JOIN                         dbo.Branch ON dbo.SupplierAccount.branchid = dbo.Branch.Id where  (SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.SupplierAccount.branchid='" + cmbbranchjv.SelectedValue + "' order by dbo.SupplierAccount.Date,dbo.SupplierAccount.Id";

                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.SupplierAccount.branchid, dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                          dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname, dbo.Branch.BranchName FROM            dbo.SupplierAccount INNER JOIN                         dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id INNER JOIN                         dbo.ChartofAccounts ON dbo.SupplierAccount.PayableAccountId = dbo.ChartofAccounts.Id INNER JOIN                         dbo.Branch ON dbo.SupplierAccount.branchid = dbo.Branch.Id where  SupplierAccount.SupplierId='" + cmbsupplier.SelectedValue + "' and (SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and  dbo.SupplierAccount.branchid='" + cmbbranchjv.SelectedValue + "'  order by dbo.SupplierAccount.Date,dbo.SupplierAccount.Id";
                    }
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

                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), "("+ds.Tables[0].Rows[i]["BranchName"].ToString()+")" + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Voucherno"].ToString(), "(" + ds.Tables[0].Rows[i]["BranchName"].ToString() + ")" + ds.Tables[0].Rows[i]["Description"].ToString(), debit, credit, blnce, "", ds.Tables[0].Rows[i]["accountname"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);




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
