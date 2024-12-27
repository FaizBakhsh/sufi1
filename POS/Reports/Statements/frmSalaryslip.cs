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
    public partial class frmSalaryslip : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmSalaryslip()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Employees where branchid='" + cmbbranchjv.SelectedValue + "' order by name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "name";
                cmbsupplier.Text = "All";
          

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
        DataSet dsemployee = new DataSet();
        public void getemployee()
        {
            dsemployee = objCore.funGetDataSet("select * from Employees where id=" + cmbsupplier.SelectedValue);

        }
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRestaurant.Reports.Statements.rptsalaryslip rptDoc = new rptsalaryslip();
                POSRestaurant.Reports.Statements.dssalaryslip dsrpt = new dssalaryslip();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                getemployee();
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
                rptDoc.SetParameterValue("Address", address);
                rptDoc.SetParameterValue("phone", phone);
                rptDoc.SetParameterValue("Month","Salary of : "+ Convert.ToDateTime( dateTimePicker1.Text+"-01").ToString("MMMM, yyyy"));

                rptDoc.SetParameterValue("EmpName", "");
                rptDoc.SetParameterValue("EmpCode", "");
                rptDoc.SetParameterValue("Desig", "");
                
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
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Title", typeof(string));
                dtrpt.Columns.Add("Desc", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                dtrpt.Columns.Add("Date", typeof(string));
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
                string sdate = dateTimePicker1.Text + "-01";
                string enddate = dateTimePicker1.Text + "-" + DateTime.DaysInMonth(Convert.ToDateTime(dateTimePicker1.Text).Year, Convert.ToDateTime(dateTimePicker1.Text).Month).ToString();
                ds = new System.Data.DataSet();
                if (cmbsupplier.Text == "All")
                {
                    q = "select id,name from Employees where status='Active'";
                }
                else
                {

                    q = "select id,name from Employees where id='" + cmbsupplier.SelectedValue + "'";
                }
                DataSet dsemployees = new System.Data.DataSet();
                dsemployees = objCore.funGetDataSet(q);
                foreach (DataRow dr in dsemployees.Tables[0].Rows)
                {
                    double totalpaid = 0,remaining=0;
                    string empid=dr["id"].ToString();
                    string name = dr["name"].ToString();
                    q = "select * from EmployeesAccount where  SalaryMonth='" + Convert.ToDateTime(sdate).ToString("MMMM yyyy") + "' and EmployeesAccount.EmployeeId='" + empid + "' and  (EmployeesAccount.Date between '" + sdate + "' and '" + Convert.ToDateTime(enddate).AddDays(-1) + "') and  dbo.EmployeesAccount.branchid='" + cmbbranchjv.SelectedValue + "' order by dbo.EmployeesAccount.Date";
                    double debit1 = 0, credit1 = 0;
                    ds = objCore.funGetDataSet(q);
                    string val = "";
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

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
                        if (debit1 > 0)
                        {
                            totalpaid = totalpaid + debit1;
                            dtrpt.Rows.Add(name, "2. Advances", ds.Tables[0].Rows[i]["Description"].ToString(), debit1, dscompany.Tables[0].Rows[0]["logo"], Convert.ToDateTime(ds.Tables[0].Rows[i]["date"].ToString()).ToString("dd-MM-yyyy"));
                        }
                    }
                    ds = new System.Data.DataSet();
                    q = "select * from EmployeesAccount  where  EmployeesAccount.EmployeeId='" + empid + "' and  (EmployeesAccount.Date between '" + enddate + "' and '" + enddate + "') and  dbo.EmployeesAccount.branchid='" + cmbbranchjv.SelectedValue + "' order by dbo.EmployeesAccount.Date";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

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


                        if (credit1 > 0)
                        {
                            remaining = remaining + credit1;
                            dtrpt.Rows.Add(name, "1. Earnings", ds.Tables[0].Rows[i]["Description"].ToString(), credit1, dscompany.Tables[0].Rows[0]["logo"], Convert.ToDateTime(ds.Tables[0].Rows[i]["date"].ToString()).ToString("dd-MM-yyyy"));
                        }
                    }
                    ds = new System.Data.DataSet();
                    q = "select * from EmployeesAccount where  SalaryMonth='" + Convert.ToDateTime(sdate).ToString("MMMM yyyy") + "' and  EmployeesAccount.EmployeeId='" + empid + "' and  (EmployeesAccount.Date> '" + enddate + "' ) and  dbo.EmployeesAccount.branchid='" + cmbbranchjv.SelectedValue + "' order by dbo.EmployeesAccount.Date";
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

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


                        if (debit1 > 0)
                        {
                            totalpaid = totalpaid + debit1;
                            dtrpt.Rows.Add(name, "3. Payments", ds.Tables[0].Rows[i]["Description"].ToString(), debit1, dscompany.Tables[0].Rows[0]["logo"],Convert.ToDateTime( ds.Tables[0].Rows[i]["date"].ToString()).ToString("dd-MM-yyyy"));
                        }
                    }
                    if (totalpaid > 0)
                    {
                        dtrpt.Rows.Add(name, "4. Total Paid", "", totalpaid, dscompany.Tables[0].Rows[0]["logo"],"");
                    }
                    if ((remaining- totalpaid) != 0)
                    {
                        dtrpt.Rows.Add(name, "5. Outstanding/Advance", "", remaining - totalpaid, dscompany.Tables[0].Rows[0]["logo"], "");
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
           

        }
      
    }
}
