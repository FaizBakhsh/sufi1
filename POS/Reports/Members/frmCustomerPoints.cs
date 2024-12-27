using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Members
{
    public partial class frmCustomerPoints : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmCustomerPoints()
        {
            InitializeComponent();
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void frmCustomerPoints_Load(object sender, EventArgs e)
        {

        }
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRestaurant.Reports.Members.rptPoints rptDoc = new  rptPoints();
                POSRestaurant.Reports.Members.dsPoints dsrpt = new  dsPoints();
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

                        dt.Rows.Add("", "", "", "",  dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }


                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period of "+dateTimePicker1.Text +" to "+ dateTimePicker2.Text);
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
                
                dtrpt.Columns.Add("Phone", typeof(string));
                dtrpt.Columns.Add("Points", typeof(string));
                dtrpt.Columns.Add("BillNo", typeof(string));
               
                dtrpt.Columns.Add("logo", typeof(byte[]));
                DataSet ds = new DataSet();
                string q = "";


               // q = "SELECT     Id, Name, Cnic, Phone, Company, Msr, City, Address, Uploadstatus FROM         Customers";
                q = "SELECT     Customers.Name,Customers.Phone,CustomerPoints.SaleId as BillNo, SUM(CustomerPoints.Points) AS Points FROM         Customers LEFT OUTER JOIN                      CustomerPoints ON Customers.Id = CustomerPoints.Customerid where dbo.CustomerPoints.SaleId like '%" + textBox1.Text + "%' GROUP BY Customers.Name, Customers.Phone,CustomerPoints.SaleId ";

                if (textBox1.Text.Trim() == string.Empty)
                {
                    q = "SELECT     dbo.Customers.Name, dbo.Customers.Phone, dbo.CustomerPoints.SaleId AS BillNo, SUM(dbo.CustomerPoints.Points) AS Points FROM         dbo.Sale INNER JOIN                      dbo.CustomerPoints ON dbo.Sale.Id = dbo.CustomerPoints.SaleId INNER JOIN                      dbo.Customers ON dbo.CustomerPoints.Customerid = dbo.Customers.Id where  (dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.Customers.Name, dbo.Customers.Phone, dbo.CustomerPoints.SaleId";
                }
                else
                {
                    q = "SELECT     dbo.Customers.Name, dbo.Customers.Phone, dbo.CustomerPoints.SaleId AS BillNo, SUM(dbo.CustomerPoints.Points) AS Points FROM         dbo.Sale INNER JOIN                      dbo.CustomerPoints ON dbo.Sale.Id = dbo.CustomerPoints.SaleId INNER JOIN                      dbo.Customers ON dbo.CustomerPoints.Customerid = dbo.Customers.Id where dbo.Customers.Name like '%" + textBox1.Text + "%' and (dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.Customers.Name, dbo.Customers.Phone, dbo.CustomerPoints.SaleId";
                }
                ds = objCore.funGetDataSet(q);
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Phone"].ToString(), ds.Tables[0].Rows[i]["Points"].ToString(), ds.Tables[0].Rows[i]["BillNo"].ToString(), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Phone"].ToString(), ds.Tables[0].Rows[i]["Points"].ToString(), ds.Tables[0].Rows[i]["BillNo"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }
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
    }
}
