using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
namespace POSRestaurant.Reports.SaleReports
{
    public partial class frmMonthly : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmMonthly()
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
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptWeekly rptDoc = new  rptWeekly();
                POSRestaurant.Reports.SaleReports.DsWeekly dsrpt = new  DsWeekly();
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

                        dt.Rows.Add("", "", "", "0","0","0","0", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);

                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
                  
        }
        public double getquantity(string itmid)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
                DataSet dsgetq = new DataSet();
                dsgetq = objCore.funGetDataSet(q);
                if (dsgetq.Tables[0].Rows.Count > 0)
                {
                    string val = "";
                    val = dsgetq.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    qty = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            return qty;
        }
        public double getgst(string itmid)
        {
            double gst = 0;
            DataSet dsgst = new DataSet();
            string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                gst = Convert.ToDouble(val);
            }
            return gst;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("day", typeof(string));
                dtrpt.Columns.Add("menugroup", typeof(string));
                dtrpt.Columns.Add("menuitem", typeof(string));
                dtrpt.Columns.Add("quantity", typeof(double));
                dtrpt.Columns.Add("amount", typeof(double));
                
                dtrpt.Columns.Add("GST", typeof(double));
                dtrpt.Columns.Add("Net", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("dis", typeof(double));
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
                   // q = "SELECT     SUM(dbo.Saledetails.Price) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.Sale.Date FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.MenuGroup.Name, dbo.Sale.Date";
                    q = "SELECT     SUM(dbo.Saledetails.Price) AS amount, COUNT(dbo.Saledetails.Price) AS quantity, dbo.MenuGroup.Name AS menugroup, dbo.Sale.Date, dbo.MenuItem.Name AS menuitem, dbo.MenuItem.id AS mid, dbo.Sale.Discount FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.MenuGroup.Name, dbo.Sale.Date, dbo.MenuItem.Name, dbo.MenuItem.id, dbo.Sale.Discount";
                }
                else
                {
                   // q = "SELECT     SUM(dbo.Saledetails.Price) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.Sale.Date FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.userid='"+comboBox1.SelectedValue+"' GROUP BY dbo.MenuGroup.Name, dbo.Sale.Date";
                    q = "SELECT     SUM(dbo.Saledetails.Price) AS amount, COUNT(dbo.Saledetails.Price) AS quantity, dbo.MenuGroup.Name AS menugroup, dbo.Sale.Date, dbo.MenuItem.Name AS menuitem, dbo.MenuItem.id AS mid, dbo.Sale.Discount FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.userid='" + comboBox1.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.Sale.Date, dbo.MenuItem.Name, dbo.MenuItem.id, dbo.Sale.Discount";
            
                }

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DateTime dte = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    string day = dte.ToString("MMMM", CultureInfo.InvariantCulture);
                    string val = "";
                    
                    val = "";
                    val = ds.Tables[0].Rows[i]["amount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double amount = Convert.ToDouble(val);
                    double quantity = getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                    double gst = getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                    val = "";
                    val = ds.Tables[0].Rows[i]["discount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double discount = Convert.ToDouble(val);
                    discount = (amount * discount) / 100;
                    gst = ((amount - discount) * gst) / 100;
                    double net = amount + gst;
                    net = net - discount;
                    net = Math.Round(net, 2);
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(day, ds.Tables[0].Rows[i]["menugroup"].ToString(), ds.Tables[0].Rows[i]["menuitem"].ToString(), quantity,amount,gst,net, null,discount);
                    }
                    else
                    {
                        dtrpt.Rows.Add(day, ds.Tables[0].Rows[i]["menugroup"].ToString(), ds.Tables[0].Rows[i]["menuitem"].ToString(), quantity, amount, gst, net, dscompany.Tables[0].Rows[0]["logo"], discount);
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
