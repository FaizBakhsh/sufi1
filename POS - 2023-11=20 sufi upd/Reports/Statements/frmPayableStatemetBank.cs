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
    public partial class frmPayableStatemetBank : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmPayableStatemetBank()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Supplier ";
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
        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
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


                POSRetail.Reports.Statements.rptpayablestatementbank rptDoc = new rptpayablestatementbank();
                POSRetail.Reports.Statements.dspayablebank dsrpt = new  dspayablebank();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
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

                        dt.Rows.Add("", "", "", "", "", "", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
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
                dtrpt.Columns.Add("CheckNo", typeof(string));
                dtrpt.Columns.Add("AccountName", typeof(string));
                dtrpt.Columns.Add("date1", typeof(string));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                getcompany();
                DataSet ds = new DataSet();
                string q = "";
                double bf = 0; // bfr();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                if (cmbsupplier.Text == "All Suppliers")
                {
                    q = "";// "SELECT DISTINCT dbo.CustomerAccount.CustomersId, dbo.Customers.Name  FROM         dbo.Customers INNER JOIN                      dbo.CustomerAccount ON dbo.Customers.Id = dbo.CustomerAccount.CustomersId where dbo.Customers.branchid='" + cmbbranchjv.SelectedValue + "'";
                    q = "SELECT DISTINCT dbo.SupplierAccount.SupplierId, dbo.Supplier.Name FROM         dbo.SupplierAccount INNER JOIN                      dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.Supplier.branchid='" + cmbbranchjv.SelectedValue + "'";
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
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", dscs.Tables[0].Rows[l][1].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);


                            }
                            else
                            {
                                bf = System.Math.Abs(bf);
                                dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", dscs.Tables[0].Rows[l][1].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);

                            }


                        }
                    }
                    //q = "SELECT     dbo.SupplierAccount.Balance AS CurrentBalance, dbo.SupplierAccount.Credit, dbo.SupplierAccount.Debit, dbo.SupplierAccount.Description, dbo.SupplierAccount.VoucherNo,                       dbo.SupplierAccount.Date, dbo.Supplier.Name AS accountname FROM         dbo.ChartofAccounts INNER JOIN                      dbo.SupplierAccount ON dbo.ChartofAccounts.Id = dbo.SupplierAccount.PayableAccountId INNER JOIN                     dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where  (dbo.SupplierAccount.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') order by dbo.SupplierAccount.Date,dbo.SupplierAccount.VoucherNo";
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
                            dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                        }
                        else
                        {
                            bf = System.Math.Abs(bf);
                            dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                        }
                    }
                    else
                    {
                        if (bf >= 0)
                        {
                            dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", bf, "0", brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);


                        }
                        else
                        {
                            bf = System.Math.Abs(bf);
                            dtrpt.Rows.Add(dateTimePicker1.Text, "", "Balance B/F", "0", bf, brf, "", cmbsupplier.Text, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);

                        }


                    }

                }
                
                                                               

                double balance1 = 0, debit1 = 0, credit1 = 0;
                balance1 = bf;
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string debit = "";
                    string credit = "";
                    string blnce = "",val="";

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
        public double bfr(string id)
        {
            double bf = 0;
            try
            {
                string q = "";
                if (cmbsupplier.Text == "All Suppliers")
                {

                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where  date <'" + dateTimePicker1.Text + "' and SupplierId='" + id + "' ";

                }
                else
                {
                    q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where SupplierId='" + cmbsupplier.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'  ";

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
        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbsupplier.Text == "")
            {
                MessageBox.Show("Please Select Supplier");
                return;
            }

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
    }
}
