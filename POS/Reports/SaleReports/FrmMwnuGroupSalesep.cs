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
    public partial class FrmMwnuGroupSalesep : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmMwnuGroupSalesep()
        {
            InitializeComponent();
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
        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            
            
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreportmenugroup()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptMenuGroupSale rptDoc = new Reports.SaleReports.rptMenuGroupSale();
            POSRestaurant.Reports.SaleReports.dsmenugroupreceipt ds = new Reports.SaleReports.dsmenugroupreceipt();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "DataTable1";
            dt = getAllOrdersmenugroup();
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
                ds.DataTable1.Merge(dt);
            }
            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("phn", "Menu Group Wise Sales Report");
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text +" to "+dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
        }
        public void bindreportmenuitem(string name)
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptMenuGroupSale rptDoc = new Reports.SaleReports.rptMenuGroupSale();
            POSRestaurant.Reports.SaleReports.dsmenugroupreceipt ds = new Reports.SaleReports.dsmenugroupreceipt();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "DataTable1";
            dt = getAllOrdersmenuitem(name);
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
                ds.DataTable1.Merge(dt);
            }
            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("phn", "Menu Item Wise Sales Report of '"+name+"'");
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
        }
        public DataTable getAllOrdersmenugroup()
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("name", typeof(string));
                dtrptmenu.Columns.Add("quantity", typeof(double));
                dtrptmenu.Columns.Add("amount", typeof(double));
                dtrptmenu.Columns.Add("logo", typeof(byte[]));
                dtrptmenu.Columns.Add("perc", typeof(string));
                dtrptmenu.Columns.Add("cost", typeof(double));
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
                //q = "SELECT     SUM(dbo.Saledetails.Price) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + date + "') GROUP BY dbo.MenuGroup.Name";
                q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid' and dbo.MenuGroup.type='"+comboBox1.Text+"'  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double perc = 0,totalsale=0;
                    DataSet dsperc = new DataSet();
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and   dbo.Sale.BillStatus='Paid' and dbo.MenuGroup.type='" + comboBox1.Text + "'";
                    dsperc = objCore.funGetDataSet(q);
                    if (dsperc.Tables[0].Rows.Count > 0)
                    {
                        string temp = dsperc.Tables[0].Rows[0]["price"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        totalsale = Convert.ToDouble(temp);
                    }
                    double cost = getcost(ds.Tables[0].Rows[i]["id"].ToString());
                    perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                    if (logo == "")
                    {
                        dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), null,perc.ToString()+" %",cost);
                    }
                    else
                    {
                        dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), dscompany.Tables[0].Rows[0]["logo"], perc.ToString() + " %",cost);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
        public DataTable getAllOrdersmenuitem(string name)
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("name", typeof(string));
                dtrptmenu.Columns.Add("quantity", typeof(double));
                dtrptmenu.Columns.Add("amount", typeof(double));
                dtrptmenu.Columns.Add("logo", typeof(byte[]));
                dtrptmenu.Columns.Add("perc", typeof(string));
                dtrptmenu.Columns.Add("cost", typeof(double));
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
                //q = "SELECT     SUM(dbo.Saledetails.Price) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + date + "') GROUP BY dbo.MenuGroup.Name";
               // q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid' and dbo.Saledetails.ModifierId='0' and dbo.Saledetails.RunTimeModifierId='0'  GROUP BY dbo.MenuGroup.Name";
                //q = "SELECT     SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuItem.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE dbo.MenuItem.Name='"+name+"' and (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId = '0') AND (dbo.Saledetails.RunTimeModifierId = '0') GROUP BY dbo.MenuItem.Name";
                q = "SELECT     SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuItem.Name, dbo.MenuItem.id,dbo.MenuGroup.id as mid, dbo.Saledetails.Flavourid,                      dbo.ModifierFlavour.name AS Expr1 FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                      dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE   dbo.MenuGroup.Name='" + name + "' and dbo.MenuGroup.type='" + comboBox1.Text + "' and  (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid')  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.id,dbo.MenuGroup.id,dbo.Saledetails.Flavourid   ORDER BY dbo.MenuItem.Name";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string size = ds.Tables[0].Rows[i]["Expr1"].ToString();
                    if (size.Length > 0)
                    {
                        size = size.Substring(0, 1)+" '";
                    }
                    double perc = 0, totalsale = 0;
                    DataSet dsperc = new DataSet();
                    //q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.MenuItem.MenuGroupId='" + ds.Tables[0].Rows[i]["mid"].ToString() + "' and dbo.Sale.BillStatus='Paid' and dbo.Saledetails.ModifierId='0' and dbo.Saledetails.RunTimeModifierId='0'  GROUP BY dbo.MenuGroup.Name";
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.MenuItem.MenuGroupId='" + ds.Tables[0].Rows[i]["mid"].ToString() + "' and dbo.MenuGroup.type='" + comboBox1.Text + "' and   dbo.Sale.BillStatus='Paid'";
                  
                    dsperc = objCore.funGetDataSet(q);
                    if (dsperc.Tables[0].Rows.Count > 0)
                    {
                        string temp = dsperc.Tables[0].Rows[0]["price"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        totalsale = Convert.ToDouble(temp);
                    }
                    double cost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString());
                    perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                    if (logo == "")
                    {
                        dtrptmenu.Rows.Add(size+ ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), null, perc.ToString() + " %", cost);
                    }
                    else
                    {
                        dtrptmenu.Rows.Add(size + ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), dscompany.Tables[0].Rows[0]["logo"], perc.ToString() + " %", cost);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
        private double getprice(string id)
        {

            double variance = 0, price = 0;
            string val = "";
            DataSet dspurchase = new DataSet();
            string q = "SELECT     AVG(dbo.PurchaseDetails.TotalAmount / dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";
            dspurchase = objCore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = Convert.ToDouble(val);
            }
            if (price == 0)
            {
                dspurchase = new DataSet();
                q = "SELECT     top 1 (dbo.PurchaseDetails.TotalAmount / dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";
                dspurchase = objCore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = Convert.ToDouble(val);
                }
            }
            return price;
        }
        public double getcost(string id)
        {
            double cost = 0,totalqty=0;

            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "' GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid";
            DataSet dscons = new DataSet();
            dscons = objCore.funGetDataSet(q);
            for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
            {
                string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                double qty = Convert.ToDouble(temp);
                if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                {
                    q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                }
                else
                {
                    q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";
                }
                DataSet dsrecipyqty = new DataSet();
                dsrecipyqty = objCore.funGetDataSet(q);
                for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                {
                    temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double recipeqty = Convert.ToDouble(temp);

                    double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                    totalqty = (recipeqty * qty);
                    double rate = 0;
                    DataSet dscon = new DataSet();
                    q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                    dscon = objCore.funGetDataSet(q);
                    if (dscon.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    if (rate > 0)
                    {
                        rate = totalqty / rate;
                    }
                    double amount = prc * rate;

                    amount = Math.Round(amount, 3);
                    cost = cost + amount;
                }
            }

            return cost;
        }
        public double getcostmenu(string id ,string mid)
        {
            double cost = 0, totalqty = 0;
           
            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='"+mid+"'  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid";
            DataSet dscons = new DataSet();
            dscons = objCore.funGetDataSet(q);
            for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
            {
                string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                double qty = Convert.ToDouble(temp);
                if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                {
                    q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                }
                else
                {
                    q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";
                }
                DataSet dsrecipyqty = new DataSet();
                dsrecipyqty = objCore.funGetDataSet(q);
                for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                {
                    temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double recipeqty = Convert.ToDouble(temp);

                    double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                    totalqty = (recipeqty * qty);
                    double rate = 0;
                    DataSet dscon = new DataSet();
                    q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                    dscon = objCore.funGetDataSet(q);
                    if (dscon.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    if (rate > 0)
                    {
                        rate = totalqty / rate;
                    }
                    double amount = prc * rate;

                    amount = Math.Round(amount, 3);
                    cost = cost + amount;
                }
            }

            return cost;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreportmenugroup();
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
            //String sObjectProperties = "Name: " + info.Name + "\nText: " + info.Text + "\nObject Type: " + info.ObjectType + "\nToolTip: " + info.ToolTip + "\nDataContext: " + info.DataContext + "\nGroup Name Path: " + info.GroupNamePath + "\nHyperlink: " + info.Hyperlink;
            string name = info.Text;
            bindreportmenuitem(name);
            vButton2.Visible = true;
            //MessageBox.Show(sObjectProperties);
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            bindreportmenugroup();
            vButton2.Visible = true;
        }
    }
}
