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
    public partial class frmSalesConsolidated : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmSalesConsolidated()
        {
            InitializeComponent();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

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
        public double getdiscount(string itmid)
        {
            double discount = 0,qty=0;
            DataSet dsgst = new DataSet();
           // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.DiscountAmount) AS qty  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
            //string q = "SELECT     SUM(dbo.Sale.DiscountAmount) AS qty  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')  AND (dbo.Sale.Discount > 0) ";
            //q = "SELECT     SUM(dbo.Sale.DiscountAmount) / SUM(dbo.Sale.TotalBill) * 100 AS ex FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')  AND (dbo.Sale.Discount > 0) ";
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
                if (discount > 0)
                {
                }
            }
            return discount;
        }
        public double getgst()
        {
            double gst = 0, qty = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.gst) AS qty  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
            //string q = "SELECT     SUM(dbo.Sale.DiscountAmount) AS qty  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')  AND (dbo.Sale.Discount > 0) ";
            //q = "SELECT     SUM(dbo.Sale.DiscountAmount) / SUM(dbo.Sale.TotalBill) * 100 AS ex FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')  AND (dbo.Sale.Discount > 0) ";
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
        public double getdiscountruntime(string itmid , string id)
        {
            double discount = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.Discount) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '"+id+"')";

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
        public double getdiscountmodifierflavour(string itmid, string id)
        {
            double discount = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.Discount) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '"+id+"') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";

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
        public double getdiscountmodifier(string itmid, string id)
        {
            double discount = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.Discount) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '"+id+"') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";

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
        public double getprice(string itmid)
        {
            double price = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT  Price from  MenuItem where id='"+itmid+"'";

            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = Convert.ToDouble(val);
            }
            return price;
        }
        public double getpriceruntime(string itmid)
        {
            double price = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT  Price from  RuntimeModifier where id='" + itmid + "'";

            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = Convert.ToDouble(val);
            }
            return price;
        }
        public double getpriceflavour(string itmid)
        {
            double price = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT  Price from  ModifierFlavour where id='" + itmid + "'";

            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = Convert.ToDouble(val);
            }
            return price;
        }
        public double getpricemodifier(string itmid)
        {
            double price = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT  Price from  Modifier where id='" + itmid + "'";

            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = Convert.ToDouble(val);
            }
            return price;
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
        public double getquantityruntime(string itmid,string id)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '"+id+"')";
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
        public double getquantitymodifier(string itmid , string id)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '"+id+"') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
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
        public double getquantityflavr(string itmid,string id)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '"+id+"') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
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
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptSaleConsolidated rptDoc = new rptSaleConsolidated();
                POSRestaurant.Reports.SaleReports.dsSales dsrpt = new dsSales();
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

                        dt.Rows.Add("", "", "","","", "0","0", dscompany.Tables[0].Rows[0]["logo"],"0","0","0");



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }

                //double discount = getdiscount("");
                //double gst = getgst("");
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                //rptDoc.SetParameterValue("dis", discount);
                //rptDoc.SetParameterValue("gst", gst);
                //rptDoc.SetParameterValue("rpt", "Modifier Flavour Sales Report");
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
                dtrpt.Columns.Add("Modifier", typeof(string));
                dtrpt.Columns.Add("RuntimeModifier", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));                
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("dis", typeof(double));
                dtrpt.Columns.Add("gst", typeof(double));
                dtrpt.Columns.Add("net", typeof(double));
                dtrpt.Columns.Add("BillNo", typeof(string));
                dtrpt.Columns.Add("User", typeof(string));
                dtrpt.Columns.Add("Date", typeof(string));
                DataSet ds = new DataSet();
                string q = "";

                if (cmbgroup.Text == "All")
                {
                    if (checkBox1.Checked == true)
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.ModifierFlavour.name, dbo.Modifier.Name AS modifier, dbo.RuntimeModifier.name AS RuntimeModifier,                          dbo.ModifierFlavour.Id AS modifld, SUM(dbo.Saledetails.Itemdiscount) AS discount, SUM(dbo.Saledetails.ItemGst) AS gst, SUM(dbo.Saledetails.Quantity) AS Quantity, SUM(dbo.Saledetails.Price) AS Amount,                          dbo.MenuItem.Id AS mid, dbo.Modifier.Id AS modiifierid, dbo.RuntimeModifier.id AS modirid, dbo.Users.Name AS Username, dbo.Sale.Id, dbo.Sale.date FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Users ON dbo.Sale.UserId = dbo.Users.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.MenuGroup.Name, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.Id, dbo.Modifier.Name, dbo.RuntimeModifier.name, dbo.Modifier.Id,                       dbo.RuntimeModifier.id, dbo.Users.Name, dbo.Sale.Id, dbo.Sale.date order by dbo.MenuItem.Name,dbo.RuntimeModifier.name";
                    }
                    else
                    {
                        q = "SELECT     dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.ModifierFlavour.name, dbo.Modifier.Name AS modifier, dbo.RuntimeModifier.name AS RuntimeModifier,                       dbo.ModifierFlavour.Id AS modifld, SUM(dbo.Saledetails.Itemdiscount) AS discount, SUM(dbo.Saledetails.ItemGst) AS gst, SUM(dbo.Saledetails.Quantity) AS Quantity, SUM(dbo.Saledetails.Price) AS Amount, dbo.MenuItem.Id AS mid, dbo.Modifier.Id AS modiifierid,                       dbo.RuntimeModifier.id AS modirid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                      dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                     dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.MenuGroup.Name, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.Id, dbo.Modifier.Name, dbo.RuntimeModifier.name, dbo.Modifier.Id,                       dbo.RuntimeModifier.id order by dbo.MenuItem.Name,dbo.RuntimeModifier.name";

                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.ModifierFlavour.name, dbo.Modifier.Name AS modifier, dbo.RuntimeModifier.name AS RuntimeModifier,                          dbo.ModifierFlavour.Id AS modifld, SUM(dbo.Saledetails.Itemdiscount) AS discount, SUM(dbo.Saledetails.ItemGst) AS gst, SUM(dbo.Saledetails.Quantity) AS Quantity, SUM(dbo.Saledetails.Price) AS Amount,                          dbo.MenuItem.Id AS mid, dbo.Modifier.Id AS modiifierid, dbo.RuntimeModifier.id AS modirid, dbo.Users.Name AS Username, dbo.Sale.Id, dbo.Sale.date FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Users ON dbo.Sale.UserId = dbo.Users.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillStatus='Paid' and dbo.MenuGroup.id='"+cmbgroup.SelectedValue+"' GROUP BY dbo.MenuGroup.Name, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.Id, dbo.Modifier.Name, dbo.RuntimeModifier.name, dbo.Modifier.Id,                       dbo.RuntimeModifier.id, dbo.Users.Name, dbo.Sale.Id, dbo.Sale.date order by dbo.MenuItem.Name,dbo.RuntimeModifier.name";
                    }
                    else
                    {
                        q = "SELECT     dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.ModifierFlavour.name, dbo.Modifier.Name AS modifier, dbo.RuntimeModifier.name AS RuntimeModifier,                       dbo.ModifierFlavour.Id AS modifld, SUM(dbo.Saledetails.Itemdiscount) AS discount, SUM(dbo.Saledetails.ItemGst) AS gst, SUM(dbo.Saledetails.Quantity) AS Quantity, SUM(dbo.Saledetails.Price) AS Amount, dbo.MenuItem.Id AS mid, dbo.Modifier.Id AS modiifierid,                       dbo.RuntimeModifier.id AS modirid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                      dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                     dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillStatus='Paid' and dbo.MenuGroup.id='" + cmbgroup.SelectedValue + "'  GROUP BY dbo.MenuGroup.Name, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.Id, dbo.Modifier.Name, dbo.RuntimeModifier.name, dbo.Modifier.Id,                       dbo.RuntimeModifier.id order by dbo.MenuItem.Name,dbo.RuntimeModifier.name";

                    }
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
                string itmid = "";
                double discountprice = 0;// getprice(ds.Tables[0].Rows[i]["mid"].ToString());
                double discountqty = 0, discountperc = 0;
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val =  ds.Tables[0].Rows[i]["Quantity"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double qty =Convert.ToDouble(val);

                    if (qty == 0)
                    {
                        qty = 1;
                    }
                    double amnt = 0;// Convert.ToDouble(val);
                    string id = ds.Tables[0].Rows[i]["mid"].ToString();
                    //double gst = getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                    val = ds.Tables[0].Rows[i]["gst"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double gst = Convert.ToDouble(val);
                    gst = Math.Round(gst, 2);
                    double price = 0;
                    double discount = 0;

                    val = ds.Tables[0].Rows[i]["discount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    discount = Convert.ToDouble(val);
                    discount = Math.Round(discount, 2);

                    val = ds.Tables[0].Rows[i]["Amount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = Convert.ToDouble(val);
                    price = Math.Round(price, 2);

                    //if (ds.Tables[0].Rows[i]["modirid"].ToString() != "")
                    //{
                    //    //qty = getquantityruntime(ds.Tables[0].Rows[i]["mid"].ToString(), ds.Tables[0].Rows[i]["modirid"].ToString());
                    //    price = getpriceruntime(ds.Tables[0].Rows[i]["modirid"].ToString());
                    //  //  discount = getdiscountruntime(ds.Tables[0].Rows[i]["mid"].ToString(), ds.Tables[0].Rows[i]["modirid"].ToString());
                    //}
                    //else

                    //if (ds.Tables[0].Rows[i]["modiifierid"].ToString() != "")
                    //{
                    //    //qty = getquantitymodifier(ds.Tables[0].Rows[i]["mid"].ToString(), ds.Tables[0].Rows[i]["modiifierid"].ToString());
                    //    price = getpricemodifier(ds.Tables[0].Rows[i]["modiifierid"].ToString());
                    //   // discount = getdiscountmodifier(ds.Tables[0].Rows[i]["mid"].ToString(), ds.Tables[0].Rows[i]["modiifierid"].ToString());
                    //}
                    //else
                    //if (ds.Tables[0].Rows[i]["modifld"].ToString() != "")
                    //{
                    //    //qty = getquantityflavr(ds.Tables[0].Rows[i]["mid"].ToString(), ds.Tables[0].Rows[i]["modifld"].ToString());
                    //    price = getpriceflavour(ds.Tables[0].Rows[i]["modifld"].ToString());
                    //   // discount = getdiscountmodifierflavour(ds.Tables[0].Rows[i]["mid"].ToString(), ds.Tables[0].Rows[i]["modifld"].ToString());
                    //}
                    //else
                    //{
                    //    //qty = getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                    //    price = getprice(ds.Tables[0].Rows[i]["mid"].ToString());
                    //    //discount = getdiscount(ds.Tables[0].Rows[i]["mid"].ToString());
                    //}
                    //if (qty == 0)
                    //{
                    //    qty = 1;
                    //}
                   
                    

                    //if (i == 0 || itmid != ds.Tables[0].Rows[i]["mid"].ToString())
                    //{
                    //    itmid = ds.Tables[0].Rows[i]["mid"].ToString();
                    //    discountprice = getprice(ds.Tables[0].Rows[i]["mid"].ToString());
                    //    val = ds.Tables[0].Rows[i]["Quantity"].ToString();
                    //    if (val == "")
                    //    {
                    //        val = "0";
                    //    }
                    //    discountqty = Convert.ToDouble(val);
                    //    discountperc = getdiscount(ds.Tables[0].Rows[i]["mid"].ToString());
                    //    double discountlessamount = discountqty * discountprice;
                    //    if (discountlessamount > 0)
                    //    {
                    //        discountperc = (discountperc / discountlessamount) * 100;
                    //    }
                    //    else
                    //    {
                    //        discountperc = 0;
                    //    }
                    //}
                   
                    amnt = price;
                    //discount = getdiscount(ds.Tables[0].Rows[i]["mid"].ToString());
                    //Convert.ToDouble(val);
                    //if (discount > 0)
                    //{
                    //    discount = discount / qty;
                    //    discount = (amnt * discount) / 100;
                    //    discount = Math.Round(discount, 2);
                    //}
                   // discount = discount / qty;
                   // discount = (amnt * discount) / 100; 
                    //discount = discountperc;  
                    //if (Convert.ToDateTime(dateTimePicker1.Text) >= Convert.ToDateTime("2015-04-24"))
                    //{
                    //    gst = ((amnt) * gst) / 100;
                    //}
                    //if (Convert.ToDateTime(dateTimePicker1.Text) < Convert.ToDateTime("2015-04-24"))
                    //{
                    //    gst = ((amnt -discount) * gst) / 100;
                    //}
                    string user = "", date = "", billno = "";
                    try
                    {
                        user = ds.Tables[0].Rows[i]["Username"].ToString();
                        billno = ds.Tables[0].Rows[i]["Id"].ToString();
                        date = Convert.ToDateTime(ds.Tables[0].Rows[i]["date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {
                        
                      
                    }
                    double net = amnt + gst;
                    net = net - discount;
                    net = Math.Round(net, 2);   
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["MenuGroup"].ToString(), ds.Tables[0].Rows[i]["MenuItem"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), ds.Tables[0].Rows[i]["modifier"].ToString(), ds.Tables[0].Rows[i]["RuntimeModifier"].ToString(), qty, amnt, null, discount, gst, net,billno,user,date);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["MenuGroup"].ToString(), ds.Tables[0].Rows[i]["MenuItem"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), ds.Tables[0].Rows[i]["modifier"].ToString(), ds.Tables[0].Rows[i]["RuntimeModifier"].ToString(), qty, amnt, dscompany.Tables[0].Rows[0]["logo"], discount, gst, net, billno, user, date);
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
            try
            {
                ds = new DataSet();
                string q = "select id,name from menugroup ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbgroup.DataSource = ds.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "name";
                cmbgroup.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                // ds1.Tables[0].Rows.Add(dr1);
                cmbbranch.DataSource = ds1.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void crystalReportViewer1_DoubleClick(object sender, EventArgs e)
        {
          
        }

        private void crystalReportViewer1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
          
        }

        private void crystalReportViewer1_ClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
           // CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
           //String sObjectProperties = "Name: " + info.Name +                        "\nText: " + info.Text +                        "\nObject Type: " + info.ObjectType +                        "\nToolTip: " + info.ToolTip +                        "\nDataContext: " + info.DataContext +                        "\nGroup Name Path: " + info.GroupNamePath +                        "\nHyperlink: " + info.Hyperlink;
           //MessageBox.Show(sObjectProperties);
        }
    }
}
