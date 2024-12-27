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
    public partial class frmpayableableStatemetsum : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmpayableableStatemetsum()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Supplier where branchid='"+cmbbranchjv.SelectedValue+"' order by name";
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
                dateTimePicker1.MinDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MinDate.ToString());
                dateTimePicker1.MaxDate = Convert.ToDateTime(POSRestaurant.Properties.Settings.Default.MaxDate.ToString());
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
                cmbbranchjv.Text = "All";
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
                POSRestaurant.Reports.Statements.rptrecvble rptDoc = new rptrecvble();
                POSRestaurant.Reports.Statements.dsrecvables dsrpt = new dsrecvables();
                //feereport ds = new feereport(); // .xsd file name               
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                dt.DefaultView.Sort = "name asc";
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
                        //dt.Rows.Add(dateTimePicker1.Text, "",  "0", dscompany.Tables[0].Rows[0]["logo"]);
                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "as on " + dateTimePicker1.Text);
                rptDoc.SetParameterValue("branch","of "+ cmbbranchjv.Text);
                rptDoc.SetParameterValue("report", "Payables");
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("amount", typeof(double));
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
                if (cmbbranchjv.Text == "All")
                {
                    if (cmbsupplier.Text == "All Suppliers")
                    {

                        q = "SELECT SUM(dbo.SupplierAccount.Debit) AS Debit, SUM(dbo.SupplierAccount.Credit) AS Credit, dbo.Supplier.Name  FROM  dbo.SupplierAccount INNER JOIN               dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where  dbo.SupplierAccount.date <'" + dateTimePicker1.Text + "' GROUP BY dbo.Supplier.Name  order by dbo.Supplier.Name";

                    }
                    else
                    {
                        q = "SELECT SUM(dbo.SupplierAccount.Debit) AS Debit, SUM(dbo.SupplierAccount.Credit) AS Credit, dbo.Supplier.Name  FROM  dbo.SupplierAccount INNER JOIN               dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.Supplier.Id='" + cmbsupplier.SelectedValue + "'  dbo.SupplierAccount.date <'" + dateTimePicker1.Text + "'  GROUP BY dbo.Supplier.Name  order by dbo.Supplier.Name";

                    }
                }
                else
                {
                    if (cmbsupplier.Text == "All Suppliers")
                    {

                        q = "SELECT SUM(dbo.SupplierAccount.Debit) AS Debit, SUM(dbo.SupplierAccount.Credit) AS Credit, dbo.Supplier.Name  FROM  dbo.SupplierAccount INNER JOIN               dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where  dbo.SupplierAccount.date <'" + dateTimePicker1.Text + "' and dbo.Supplier.branchid='"+cmbbranchjv.SelectedValue+"' GROUP BY dbo.Supplier.Name  order by dbo.Supplier.Name";

                    }
                    else
                    {
                        q = "SELECT SUM(dbo.SupplierAccount.Debit) AS Debit, SUM(dbo.SupplierAccount.Credit) AS Credit, dbo.Supplier.Name  FROM  dbo.SupplierAccount INNER JOIN               dbo.Supplier ON dbo.SupplierAccount.SupplierId = dbo.Supplier.Id where dbo.Supplier.Id='" + cmbsupplier.SelectedValue + "'  dbo.SupplierAccount.date <'" + dateTimePicker1.Text + "'  GROUP BY dbo.Supplier.Name  order by dbo.Supplier.Name";

                    } 
                }
                double debitbf = 0, creditbf = 0;
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int l = 0; l < ds.Tables[0].Rows.Count; l++)
                    {
                        string val = ds.Tables[0].Rows[l]["Debit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        debitbf =  Convert.ToDouble(val);

                        val = ds.Tables[0].Rows[l]["Credit"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        creditbf =  Convert.ToDouble(val);

                        bf =creditbf - debitbf ;
                        if (bf > 0)
                        {
                            if (logo == "")
                            {

                                dtrpt.Rows.Add(ds.Tables[0].Rows[l]["name"].ToString(), bf, null);
                            }
                            else
                            {
                                dtrpt.Rows.Add(ds.Tables[0].Rows[l]["name"].ToString(), bf, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }
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
    }
}
