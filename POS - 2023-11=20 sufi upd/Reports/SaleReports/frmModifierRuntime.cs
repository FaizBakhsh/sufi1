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
    public partial class frmModifierRuntime : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmModifierRuntime()
        {
            InitializeComponent();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
        DataSet dscompany = new DataSet();
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
        public double getdiscount(string itmid)
        {
            double discount = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.Discount) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";

            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                discount = Convert.ToDouble(val);
            }
            return discount;
        }
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptmodifier1 rptDoc = new rptmodifier1();
                POSRestaurant.Reports.SaleReports.dsmodifier dsrpt = new dsmodifier();
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

                        dt.Rows.Add("", "", "", "0","0", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }


                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("rpt", "Runtine Modifier Sales Report");
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
                dtrpt.Columns.Add("MenuGroup", typeof(string));
                dtrpt.Columns.Add("MenuItem", typeof(string));
                dtrpt.Columns.Add("Flavour", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));                
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("dis", typeof(double));
                dtrpt.Columns.Add("gst", typeof(double));
                dtrpt.Columns.Add("net", typeof(double)); 
                DataSet ds = new DataSet();
                string q = "";
                
                {
                    q = "SELECT     dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, SUM(dbo.Saledetails.Quantity) AS Quantity, SUM(dbo.Saledetails.Price) AS Amount, dbo.RuntimeModifier.name, dbo.MenuItem.id AS mid, dbo.Sale.Discount FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.MenuGroup.Name, dbo.MenuItem.Name,  dbo.RuntimeModifier.name, dbo.Saledetails.Price, dbo.MenuItem.id, dbo.Sale.Discount";

                }
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = ds.Tables[0].Rows[i]["Quantity"].ToString();
                    if (val == "" || val=="0")
                    {
                        val = "1";
                    }
                    double qty = Convert.ToDouble(val);
                    if (qty == 0)
                    {
                        qty = 1;
                    }
                    val = ds.Tables[0].Rows[i]["Amount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double amnt = Convert.ToDouble(val);
                    double gst = getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                    //val = "";
                    //val = ds.Tables[0].Rows[i]["discount"].ToString();
                    //if (val == "")
                    //{
                    //    val = "0";
                    //}
                    
                    double discount = getdiscount(ds.Tables[0].Rows[i]["mid"].ToString());//Convert.ToDouble(val);
                    discount = discount / qty;
                    discount = (amnt * discount) / 100;
                    discount = Math.Round(discount, 2);
                    gst = ((amnt) * gst) / 100;
                    double net = amnt + gst;
                    net = net - discount;
                    net = Math.Round(net, 2);    
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["MenuGroup"].ToString(), ds.Tables[0].Rows[i]["MenuItem"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), qty, amnt, null, discount, gst, net);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["MenuGroup"].ToString(), ds.Tables[0].Rows[i]["MenuItem"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), qty, amnt, dscompany.Tables[0].Rows[0]["logo"], discount, gst, net);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }

        private void frmModifier_Load(object sender, EventArgs e)
        {

        }
    }
}
