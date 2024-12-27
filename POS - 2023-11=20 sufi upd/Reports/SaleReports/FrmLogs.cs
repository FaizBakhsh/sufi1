using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmLogs : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmLogs()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {                                  
            fillcus();
            
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        protected void fillcus()
        {
            try
            {
                ds = new DataSet();
                string q = "";
                q = "select distinct Name from log";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox3.DataSource = ds.Tables[0];
                comboBox3.ValueMember = "Name";
                comboBox3.DisplayMember = "Name";
                comboBox3.Text = "All";
            }
            catch (Exception ex)
            {

                
            }
            try
            {
                ds = new DataSet();
                string q = "";
                q = "select *  from users";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "Id";
                comboBox1.DisplayMember = "Name";
                comboBox1.Text = "All";
            }
            catch (Exception ex)
            {


            }
        }
        public void bindreport()
        {

            try
            {
                getcompany();
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptlog rptDoc = new rptlog();
                POSRestaurant.Reports.SaleReports.dslog dsrpt = new dslog();
                
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
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("address", address);
                rptDoc.SetParameterValue("phone", phone);
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {                
            }
                  
        }
     
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Time", typeof(DateTime));
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("User1", typeof(string));
                
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                DataSet ds = new DataSet();
                string q = "";
                if (comboBox1.Text == "All")
                {
                    if (comboBox3.Text == "All")
                    {
                        q = "SELECT        dbo.[Log].Name, dbo.[Log].Time, dbo.[Log].Description, dbo.Users.Name AS User1 FROM            dbo.[Log] INNER JOIN                         dbo.Users ON dbo.[Log].Userid = dbo.Users.Id where (CONVERT(DATE, dbo.[Log].Time)) between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                    }
                    else
                    {
                        q = "SELECT        dbo.[Log].Name, dbo.[Log].Time, dbo.[Log].Description, dbo.Users.Name AS User1 FROM            dbo.[Log] INNER JOIN                         dbo.Users ON dbo.[Log].Userid = dbo.Users.Id where (CONVERT(DATE, dbo.[Log].Time)) between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.[Log].name='" + comboBox3.Text + "'";
                    }
                }
                else
                {
                    if (comboBox3.Text == "All")
                    {
                        q = "SELECT        dbo.[Log].Name, dbo.[Log].Time, dbo.[Log].Description, dbo.Users.Name AS User1 FROM            dbo.[Log] INNER JOIN                         dbo.Users ON dbo.[Log].Userid = dbo.Users.Id where (CONVERT(DATE, dbo.[Log].Time)) between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.[Log].userid='" + comboBox1.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT        dbo.[Log].Name, dbo.[Log].Time, dbo.[Log].Description, dbo.Users.Name AS User1 FROM            dbo.[Log] INNER JOIN                         dbo.Users ON dbo.[Log].Userid = dbo.Users.Id where (CONVERT(DATE, dbo.[Log].Time)) between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.[Log].name='" + comboBox3.Text + "'  and dbo.[Log].userid='" + comboBox1.SelectedValue + "'";
                    }
                }
                ds = objCore.funGetDataSet(q);
                dtrpt.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillcus();
        }
    }
}
