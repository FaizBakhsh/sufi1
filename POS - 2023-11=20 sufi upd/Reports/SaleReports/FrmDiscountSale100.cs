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
    public partial class FrmDiscountSale100 : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmDiscountSale100()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select id,name from users where usertype='cashier'";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All Users";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                comboBox1.Text = "All Users";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                getcompany();
                DataTable dt = new DataTable();
                POSRestaurant.Reports.SaleReports.rptdiscount100 rptDoc = new rptdiscount100();
                POSRestaurant.Reports.SaleReports.dsdiscount100 dsrpt = new dsdiscount100();                                
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
               
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Company", company);
                rptDoc.SetParameterValue("Address", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
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
                dtrpt.Columns.Add("date", typeof(DateTime));
                dtrpt.Columns.Add("item", typeof(string));
                dtrpt.Columns.Add("price", typeof(double));
                dtrpt.Columns.Add("invoice", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("qty", typeof(double));
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
                if (comboBox1.Text == "All Users")
                {

                    q = "SELECT     SUM(dbo.Saledetails1.Quantity) AS count,  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.Sale1.date ,dbo.Sale1.id as invoice ,SUM(dbo.Saledetails1.Price) AS price FROM         dbo.Saledetails1 INNER JOIN                      dbo.MenuItem ON dbo.Saledetails1.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.Sale1 ON dbo.Saledetails1.saleid = dbo.Sale1.Id WHERE     (Sale1.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   GROUP BY dbo.MenuItem.Name,dbo.Sale1.Discount,dbo.MenuItem.id, dbo.Sale1.date ,dbo.Sale1.id";
                }
                else
                {                  
                    q = "SELECT     SUM(dbo.Saledetails1.Quantity) AS count, dbo.MenuItem.Name,dbo.MenuItem.id, dbo.Sale1.date ,dbo.Sale1.id as invoice ,SUM(dbo.Saledetails1.Price) AS price  FROM         dbo.Saledetails1 INNER JOIN                      dbo.MenuItem ON dbo.Saledetails1.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.Sale1 ON dbo.Saledetails1.saleid = dbo.Sale1.Id WHERE     (Sale1.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale1.userid='" + comboBox1.SelectedValue + "'  GROUP BY dbo.MenuItem.Name,dbo.Sale1.Discount,dbo.MenuItem.id, dbo.Sale1.date ,dbo.Sale1.id";            
                }
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["price"].ToString(), ds.Tables[0].Rows[i]["invoice"].ToString(), null, ds.Tables[0].Rows[i]["count"].ToString());
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["price"].ToString(), ds.Tables[0].Rows[i]["invoice"].ToString(), dscompany.Tables[0].Rows[0]["logo"], ds.Tables[0].Rows[i]["count"].ToString());
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
