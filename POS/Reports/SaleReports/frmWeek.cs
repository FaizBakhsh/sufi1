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
    public partial class frmWeek : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmWeek()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                //ds = new DataSet();
                //string q = "select id,name from users where usertype='cashier'";
                //ds = objCore.funGetDataSet(q);
                //DataRow dr = ds.Tables[0].NewRow();
                //dr["name"] = "All Users";
                //ds.Tables[0].Rows.Add(dr);
                //comboBox1.DataSource = ds.Tables[0];
                //comboBox1.ValueMember = "id";
                //comboBox1.DisplayMember = "name";
                //comboBox1.Text = "All Users";
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


                POSRestaurant.Reports.SaleReports.rptcomparativeweekly rptDoc = new rptcomparativeweekly();
                POSRestaurant.Reports.SaleReports.dscomparativeweekly dsrpt = new dscomparativeweekly();
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

                        dt.Rows.Add("", "", "", "", "", "", "", "","", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "Before the Date "+ dateTimePicker2.Text);

                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
                  
        }
        public double getquantity(string itmid, DateTime sdate, DateTime enddate)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date <= '" + sdate + "') and (dbo.Sale.Date >= '" + enddate + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
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
        public double getgst(string itmid, DateTime sdate,DateTime enddate)
        {
            double gst = 0;
            DataSet dsgst = new DataSet();
            string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date <= '" + sdate + "') and (dbo.Sale.Date >= '" + enddate + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
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
                dtrpt.Columns.Add("MenuGroup", typeof(string));
                dtrpt.Columns.Add("MenuItem", typeof(string));
               
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("day", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
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
                DateTime strtdate = Convert.ToDateTime(dateTimePicker2.Text);

                DateTime enddate = strtdate.AddDays(-7);
               
                   // q = "SELECT     SUM(dbo.Saledetails.Price) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.Sale.Date FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.MenuGroup.Name, dbo.Sale.Date";
              
                q = "SELECT    dbo.MenuItem.Id, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Name AS menuitem FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId ORDER BY menugroup";

                ds = objCore.funGetDataSet(q);

                for (int j = 0; j < 7; j++)
                {
                    string day = "";
                    if (j == 0)
                    {
                        day = "Monday";
                    }
                    if (j == 1)
                    {
                        day = "Tuesday";
                    }
                    if (j == 2)
                    {
                        day = "Wednesday";
                    }
                    if (j == 3)
                    {
                        day = "Thursday";
                    }
                    if (j == 4)
                    {
                        day = "Friday";
                    }
                    if (j == 5)
                    {
                        day = "Saturday";
                    }
                    if (j == 6)
                    {
                        day = "Sunday";
                    }
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                       

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price) AS amount, COUNT(dbo.Saledetails.Price) AS quantity, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Name AS menuitem,dbo.Sale.Date, dbo.MenuItem.id AS mid, dbo.Sale.Discount FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (dbo.Sale.Date <= '" + strtdate + "') and (dbo.Sale.Date >= '" + enddate + "') and dbo.MenuItem.id='" + ds.Tables[0].Rows[i]["id"] + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuItem.Name,dbo.Sale.Date, dbo.MenuItem.id, dbo.Sale.Discount";
                            DataSet dsitems = new DataSet();
                            dsitems = objCore.funGetDataSet(q);
                            if (dsitems.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsitems.Tables[0].Rows.Count; k++)
                                {
                                    DateTime dte = Convert.ToDateTime(dsitems.Tables[0].Rows[k]["Date"].ToString());
                                    string saleday = dte.DayOfWeek.ToString();
                                    string val = "";
                                    //val = dsitems.Tables[0].Rows[k]["quantity"].ToString();
                                    //if (val == "")
                                    //{
                                    //    val = "0";
                                    //}
                                   // double quantity = Convert.ToDouble(val);
                                    val = "";
                                    val = dsitems.Tables[0].Rows[k]["amount"].ToString();
                                    if (val == "")
                                    {
                                        val = "0";
                                    }
                                    double amount = Convert.ToDouble(val);
                                    double quantity = getquantity(dsitems.Tables[0].Rows[k]["mid"].ToString(), strtdate, enddate);
                                    double gst = getgst(dsitems.Tables[0].Rows[k]["mid"].ToString(), strtdate, enddate);
                                    val = "";
                                    val = dsitems.Tables[0].Rows[k]["discount"].ToString();
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
                                    amount = net;
                                    if (saleday == day)
                                    {
                                        if (logo == "")
                                        {
                                            dtrpt.Rows.Add(dsitems.Tables[0].Rows[k]["menugroup"].ToString(), dsitems.Tables[0].Rows[k]["menuitem"].ToString(), amount, day, null);
                                        }
                                        else
                                        {
                                            dtrpt.Rows.Add(dsitems.Tables[0].Rows[k]["menugroup"].ToString(), dsitems.Tables[0].Rows[k]["menuitem"].ToString(), amount, day, dscompany.Tables[0].Rows[0]["logo"]);
                                        }
                                    }
                                    else
                                    {
                                        if (logo == "")
                                        {
                                            dtrpt.Rows.Add(dsitems.Tables[0].Rows[k]["menugroup"].ToString(), dsitems.Tables[0].Rows[k]["menuitem"].ToString(), "0", day, null);
                                        }
                                        else
                                        {
                                            dtrpt.Rows.Add(dsitems.Tables[0].Rows[k]["menugroup"].ToString(), dsitems.Tables[0].Rows[k]["menuitem"].ToString(), "0", day, dscompany.Tables[0].Rows[0]["logo"]);
                                        }
                                    }
                                } 
                            }

                            else
                            {
                                if (logo == "")
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menugroup"].ToString(), ds.Tables[0].Rows[i]["menuitem"].ToString(), "0", day, null);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menugroup"].ToString(), ds.Tables[0].Rows[i]["menuitem"].ToString(), "0", day, dscompany.Tables[0].Rows[0]["logo"]);
                                }
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
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
