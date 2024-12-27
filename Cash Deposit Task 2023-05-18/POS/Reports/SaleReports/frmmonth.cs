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
    public partial class frmmonth : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmmonth()
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


                POSRestaurant.Reports.SaleReports.rptmonth rptDoc = new rptmonth();
                POSRestaurant.Reports.SaleReports.dsmonth dsrpt = new dsmonth();
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

                        dt.Rows.Add("", "", "", "", "", "", "", "", "", "", "", "", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
               

                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
                  
        }
        public double getquantity(string itmid,string month)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (Month(dbo.Sale.Date) = '" + month + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
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
            string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (YEAR(dbo.Sale.Date) = '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
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
        public double getdiscount(string itmid ,string month)
        {
            double discount = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.Discount) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (Month(dbo.Sale.Date) = '" + month + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";

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
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("MenuGroup", typeof(string));
                dtrpt.Columns.Add("MenuItem", typeof(string));
                dtrpt.Columns.Add("jan", typeof(string));
                dtrpt.Columns.Add("feb", typeof(string));
                dtrpt.Columns.Add("march", typeof(string));
                dtrpt.Columns.Add("april", typeof(string));
                dtrpt.Columns.Add("may", typeof(string));
                dtrpt.Columns.Add("june", typeof(string));
                dtrpt.Columns.Add("july", typeof(string));
                dtrpt.Columns.Add("aug", typeof(string));
                dtrpt.Columns.Add("sept", typeof(string));
                dtrpt.Columns.Add("oct", typeof(string));
                dtrpt.Columns.Add("nov", typeof(string));
                dtrpt.Columns.Add("dec", typeof(string));
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



                q = "SELECT      distinct dbo.Saledetails.MenuItemId,dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Name AS menuitem  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Year(Sale.Date) = '" + dateTimePicker2.Text + "') ORDER BY menugroup";
                string j = "0", f = "0", m = "0", a = "0", ma = "0", ju = "0", jul = "0", au = "0", s = "0", o = "0", n = "0", d = "0";                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    q = "SELECT DISTINCT MONTH(Sale.Date) AS month FROM         Saledetails INNER JOIN                      MenuItem ON Saledetails.MenuItemId = MenuItem.Id INNER JOIN                      MenuGroup ON MenuItem.MenuGroupId = MenuGroup.Id INNER JOIN                      Sale ON Saledetails.saleid = Sale.Id WHERE     (YEAR(Sale.Date) = '" + dateTimePicker2.Text + "') ORDER BY month";
                    DataSet dsmonth = new DataSet();
                    dsmonth = objCore.funGetDataSet(q);
                    for (int k = 0; k < dsmonth.Tables[0].Rows.Count; k++)
                    {
                        q = "SELECT      SUM(dbo.Saledetails.Price) AS amount, COUNT(dbo.Saledetails.Price) AS quantity, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Name AS menuitem, dbo.MenuItem.id AS mid,                       month(dbo.Sale.Date) AS month FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (MONTH(Sale.Date) = '" + dsmonth.Tables[0].Rows[k]["month"].ToString() + "') and dbo.Saledetails.MenuItemId='" + ds.Tables[0].Rows[i]["MenuItemId"].ToString() + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuItem.Name, dbo.MenuItem.id, month(dbo.Sale.Date) ORDER BY month";
                        DataSet dsitems = new DataSet();
                        dsitems = objCore.funGetDataSet(q);
                        if (dsitems.Tables[0].Rows.Count > 0)
                        {
                            string day = dsmonth.Tables[0].Rows[k]["month"].ToString();
                            string val = "";
                            val = dsitems.Tables[0].Rows[0]["quantity"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double quantity = getquantity(ds.Tables[0].Rows[i]["MenuItemId"].ToString(), day);
                            val = "";
                            val = dsitems.Tables[0].Rows[0]["amount"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            double amount = Convert.ToDouble(val);
                            double gst = getgst(dsitems.Tables[0].Rows[0]["mid"].ToString());
                            double discount = getdiscount(dsitems.Tables[0].Rows[0]["mid"].ToString(), day);//Convert.ToDouble(val);
                            discount = discount / quantity;
                            discount = (amount * discount) / 100;
                            gst = ((amount) * gst) / 100;

                            double net = amount + gst;
                            net = net - discount;
                            net = Math.Round(net, 2);
                            amount = net;
                            if (day == "1")
                            {
                                j = amount.ToString();
                            }
                            if (day == "2")
                            {
                                f = amount.ToString();
                            }
                            if (day == "3")
                            {
                                m = amount.ToString();
                            }
                            if (day == "4")
                            {
                                a = amount.ToString();
                            }
                            if (day == "5")
                            {
                                ma = amount.ToString();
                            }
                            if (day == "6")
                            {
                                ju = amount.ToString();
                            }
                            if (day == "7")
                            {
                                jul = amount.ToString();
                            }
                            if (day == "8")
                            {
                                au = amount.ToString();
                            }
                            if (day == "9")
                            {
                                s = amount.ToString();
                            }
                            if (day == "10")
                            {
                                o = amount.ToString();
                            }
                            if (day == "11")
                            {
                                n = amount.ToString();
                            }
                            if (day == "12")
                            {
                                d = amount.ToString();
                            }
                        }
                    }
                    //DateTime dte = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString());
                    
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menugroup"].ToString(), ds.Tables[0].Rows[i]["menuitem"].ToString(), j, f, m, a, ma, ju, jul, au, s, o, n, d, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menugroup"].ToString(), ds.Tables[0].Rows[i]["menuitem"].ToString(), j, f, m, a, ma, ju, jul, au, s, o, n, d, dscompany.Tables[0].Rows[0]["logo"]);
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
