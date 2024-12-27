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
    public partial class Frmtargets : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public Frmtargets()
        {
            InitializeComponent();
        }
        
        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {


        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
       
        static string type = "";
        public void bindreportmenuitem()
        {
            type = "menu";

            POSRestaurant.Reports.SaleReports.rpttargets rptDoc = new Reports.SaleReports.rpttargets();
            POSRestaurant.Reports.SaleReports.dstargets ds = new Reports.SaleReports.dstargets();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            dt.TableName = "DataTable1";
            dt = getAllOrdersmenuitem();
            DataView dv = new DataView(dt);
            dv.Sort = "name";
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
                ds.DataTable1.Merge(dv.ToTable());
            }
            rptDoc.SetDataSource(ds);
            wastage();
            
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("phn", phone);
            rptDoc.SetParameterValue("addrs", address);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public double opening(string itemid)
        {
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "SELECT top 1 date, (remaining) as rem FROM     discard where Date <'" + date + "' and itemid='" + itemid + "' order by Date desc";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                date2 = dspurchase.Tables[0].Rows[0][0].ToString();
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                closing = Convert.ToDouble(val);
            }
            if (date2 == "")
            {
                date2 = date;
            }
            DateTime end = Convert.ToDateTime(date);
            DateTime start = Convert.ToDateTime(date2);
            TimeSpan ts = Convert.ToDateTime(date) - Convert.ToDateTime(date2);
            int days = ts.Days;
            if (days <= 1)
            {
                return closing;
            }
            start = start.AddDays(1);
            end = end.AddDays(-1);
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            dspurchase = new DataSet();
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                purchased = Convert.ToDouble(val);
            }

            val = "";
            purchased = Math.Round(purchased, 2);

            dspurchase = new DataSet();
            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                consumed = Convert.ToDouble(val);
            }
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferin = Convert.ToDouble(val);
            }
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferout = Convert.ToDouble(val);
            }


            val = "";
            double rate = 0;
            DataSet dscon = new DataSet();
            q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + itemid + "'";
            dscon = objcore.funGetDataSet(q);
            if (dscon.Tables[0].Rows.Count > 0)
            {
                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
            }
            if (rate > 0)
            {
                consumed = consumed / rate;
            }
            double qty = 0;
            qty = purchased - consumed;
            dspurchase = new DataSet();
            //q = "SELECT     SUM(variance) AS Expr1 FROM     Variance where Date <'" + date + "' and itemid='" + itemid + "'";
            //dspurchase = objcore.funGetDataSet(q);
            //if (dspurchase.Tables[0].Rows.Count > 0)
            //{
            //    val = dspurchase.Tables[0].Rows[0][0].ToString();
            //    if (val == "")
            //    {
            //        val = "0";
            //    }
            //    variance = Convert.ToDouble(val);
            //}
            ////if (variance > 0)
            //{
            //    qty = qty + (variance);
            //}
            dspurchase = new DataSet();
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                discard = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                staff = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][2].ToString();
                if (val == "")
                {
                    val = "0";
                }
                complt = Convert.ToDouble(val);
            }
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (closing + purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        double waste1 = 0, variance1 = 0, compwst1 = 0;
        protected void wastage()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, complete = 0, transferin = 0, transferout = 0, closing = 0;
            waste1 = 0; variance1 = 0; compwst1 = 0;
            string q = "select id, itemname from rawitem order by id";
            DataSet dsitem = new DataSet();
            dsitem = objCore.funGetDataSet(q);
            for (int i = 0; i < dsitem.Tables[0].Rows.Count; i++)
            {
                double prc = getprice(dsitem.Tables[0].Rows[i]["id"].ToString());
                q = "select sum(staff) as staff ,sum(Discard) as discard,sum(completewaste) as completewaste from discard where date between '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' and itemid='" + dsitem.Tables[0].Rows[i]["id"] + "'";
                DataSet dswaste = new DataSet();
                dswaste = objCore.funGetDataSet(q);
                if (dswaste.Tables[0].Rows.Count > 0)
                {


                    string temp1 = dswaste.Tables[0].Rows[0]["staff"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    waste1 = waste1 + (Convert.ToDouble(temp1) * prc);

                    
                    temp1 = dswaste.Tables[0].Rows[0]["completewaste"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    compwst1 = compwst1 + (Convert.ToDouble(temp1) * prc);
                }

            }
            
        }
        public double openingclosing(string itemid, string date, double closing)
        {
            string date2 = dateTimePicker2.Text;


            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "SELECT top 1 date, (remaining) as rem FROM     discard where Date <'" + date + "' and itemid='" + itemid + "' order by Date desc";

            DateTime end = Convert.ToDateTime(date2);
            DateTime start = Convert.ToDateTime(date);
            start = start.AddDays(1);
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            dspurchase = new DataSet();
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                purchased = Convert.ToDouble(val);
            }

            val = "";
            purchased = Math.Round(purchased, 2);

            dspurchase = new DataSet();
            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                consumed = Convert.ToDouble(val);
            }
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferin = Convert.ToDouble(val);
            }
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferout = Convert.ToDouble(val);
            }


            val = "";
            double rate = 0;
            DataSet dscon = new DataSet();
            q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + itemid + "'";
            dscon = objcore.funGetDataSet(q);
            if (dscon.Tables[0].Rows.Count > 0)
            {
                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
            }
            if (rate > 0)
            {
                consumed = consumed / rate;
            }
            double qty = 0;
            qty = purchased - consumed;
            dspurchase = new DataSet();
            //q = "SELECT     SUM(variance) AS Expr1 FROM     Variance where Date <'" + date + "' and itemid='" + itemid + "'";
            //dspurchase = objcore.funGetDataSet(q);
            //if (dspurchase.Tables[0].Rows.Count > 0)
            //{
            //    val = dspurchase.Tables[0].Rows[0][0].ToString();
            //    if (val == "")
            //    {
            //        val = "0";
            //    }
            //    variance = Convert.ToDouble(val);
            //}
            ////if (variance > 0)
            //{
            //    qty = qty + (variance);
            //}
            dspurchase = new DataSet();
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                discard = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                staff = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][2].ToString();
                if (val == "")
                {
                    val = "0";
                }
                complt = Convert.ToDouble(val);
            }
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        public DataTable getAllOrdersmenuitem()
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("Name", typeof(string));
                dtrptmenu.Columns.Add("Saletarget", typeof(double));
                dtrptmenu.Columns.Add("ActualSale", typeof(double));
                dtrptmenu.Columns.Add("SaleDiffrence", typeof(double));
                dtrptmenu.Columns.Add("CostPerc", typeof(double));
                dtrptmenu.Columns.Add("ActualCost", typeof(double));
                dtrptmenu.Columns.Add("Difference", typeof(double));
                dtrptmenu.Columns.Add("WastePerc", typeof(double));
                dtrptmenu.Columns.Add("ActualWaste", typeof(double));
                dtrptmenu.Columns.Add("WasteDifference", typeof(double));
                dtrptmenu.Columns.Add("logo", typeof(byte[]));

                DataSet ds = new DataSet();
                string q = "";
                double saletarget = 0, actualsale = 0, salediff = 0, costtarget = 0, actualcost = 0, waste = 0, actualwaste = 0;
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }

                string temp = "";
                q = "select sum(Saletarget) from SalesTargerts where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                DataSet dstarget = new DataSet();
                dstarget = objcore.funGetDataSet(q);
                if (dstarget.Tables[0].Rows.Count > 0)
                {
                    temp = dstarget.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                        
                    }
                    saletarget = Convert.ToDouble(temp);
                }
                q = "select sum(netbill + DiscountAmount) as amount from sale where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and billstatus='Paid'";
                dstarget = new DataSet();
                dstarget = objcore.funGetDataSet(q);
                if (dstarget.Tables[0].Rows.Count > 0)
                {
                    temp = dstarget.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                        
                    }
                    actualsale = Convert.ToDouble(temp);
                }
                salediff = actualsale - saletarget;

                q = "select avg(CostPerc) from SalesTargerts where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                dstarget = new DataSet();
                dstarget = objcore.funGetDataSet(q);
                if (dstarget.Tables[0].Rows.Count > 0)
                {
                    temp = dstarget.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                        
                    }
                    costtarget = Convert.ToDouble(temp);
                }
                actualcost = getcostmenu();
                actualcost = actualcost / actualsale * 100;
                q = "select avg(WastePerc) from SalesTargerts where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                dstarget = new DataSet();
                dstarget = objcore.funGetDataSet(q);
                if (dstarget.Tables[0].Rows.Count > 0)
                {
                    temp = dstarget.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                        
                    }
                    waste = Convert.ToDouble(temp);
                }
                wastage();
                actualwaste = compwst1 + waste1;
                actualwaste = actualwaste / actualsale * 100;

                ds=new DataSet();
                ds= objCore.funGetDataSet("select * from branch");


                dtrptmenu.Rows.Add(ds.Tables[0].Rows[0]["branchname"].ToString(), saletarget, actualsale, salediff, costtarget, actualcost, actualcost - costtarget, waste, actualwaste, actualwaste - waste, dscompany.Tables[0].Rows[0]["logo"]);
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
    

        private double getprice(string id)
        {

            double variance = 0, price = 0;
            string val = "";
            DataTable ds = new DataTable();
            DataSet dspurchase = new DataSet();
            string q = "SELECT     AVG(DISTINCT dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";
            q = "SELECT   MONTH(date),year(date), avg (dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE  ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'  group by MONTH(date), year(date)";
            dspurchase = objCore.funGetDataSet(q);
            for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
            {
                val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = price + Convert.ToDouble(val);
            }
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                price = price / dspurchase.Tables[0].Rows.Count;
            }

            if (price == 0)
            {

                q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE      ( date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and itemId = '" + id + "'   and (price > 0) GROUP BY MONTH(Date), YEAR(Date)";
                dspurchase = new DataSet();
                dspurchase = objCore.funGetDataSet(q);
                for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
                {
                    val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = price + Convert.ToDouble(val);
                }
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    price = price / dspurchase.Tables[0].Rows.Count;
                }
            }


            if (price == 0)
            {
                dspurchase = new DataSet();
                q = "SELECT     top 1 (dbo.PurchaseDetails.TotalAmount / dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";
                q = "SELECT  top 1 MONTH(date),year(date), avg (dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE  ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "'  group by MONTH(date), year(date)";

                dspurchase = objCore.funGetDataSet(q);
                for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
                {
                    val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = price + Convert.ToDouble(val);
                }
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    price = price / dspurchase.Tables[0].Rows.Count;
                }
            }
            if (price == 0)
            {
                try
                {
                    dspurchase = new DataSet();
                    q = "select price from rawitem where id='" + id + "'";
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
                catch (Exception ex)
                {


                }
            }
            return price;
        }

        public double getcost(string id, string type)
        {
            double cost = 0, totalqty = 0;

            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            if (type == "modifier")
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.Saledetails.ModifierId>0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
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
                    q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head FROM            Modifier where id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "'";

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
            }
            else
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid , dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "' and dbo.Saledetails.ModifierId=0 GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId";
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
                    string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if(rmodid=="")
                    {
                        rmodid="0";
                    }
                    string modid = dscons.Tables[0].Rows[i]["ModifierId"].ToString();
                    if(modid=="")
                    {
                        modid="0";
                    }

                    if (modid != "0")
                    {
                        q = "SELECT RawItemId, Quantity from Modifier where id='0' ";
                    }
                    else
                    {
                        if (rmodid != "0")
                        {
                            q = "SELECT RawItemId, Quantity from RuntimeModifier where id='" + rmodid + "' ";
                        }
                        else
                        {
                            if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                            }
                            else
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";
                            }
                        }
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
            }

            return cost;
        }
        public double getcostmenu()
        {
            double cost = 0, totalqty = 0;

            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
           
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')    AND (dbo.Sale.BillStatus = 'Paid')   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
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
                    q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head FROM            Modifier where id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "'";

                    q = "SELECT        dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.Modifier.uploadstatus, dbo.Modifier.branchid, dbo.Modifier.kdsid, dbo.Modifier.menugroupid, dbo.Modifier.Head,                          dbo.Type.TypeName FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId RIGHT OUTER JOIN                         dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId where dbo.Modifier.id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "'";

                    

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
            }
           
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')   and dbo.Saledetails.RunTimeModifierId >0  and dbo.Saledetails.ModifierId =0   AND (dbo.Sale.BillStatus = 'Paid')  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId";
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
                    q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "' ";
                    
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

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString() + "'";
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
            }
          
            {

                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and  dbo.Saledetails.ModifierId=0   and  dbo.Saledetails.runtimeModifierId=0    AND (dbo.Sale.BillStatus = 'Paid')    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                  
                
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }

                    string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if (rmodid == "")
                    {
                        rmodid = "0";
                    }

                    double qty = Convert.ToDouble(temp);

                    if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                    {
                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'";
                    }
                    else
                    {
                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";
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
            }

            return cost;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Enabled = false;
            vButton1.Text = "Please Wait";
            bindreportmenuitem();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }
        
        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            //CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
            ////String sObjectProperties = "Name: " + info.Name + "\nText: " + info.Text + "\nObject Type: " + info.ObjectType + "\nToolTip: " + info.ToolTip + "\nDataContext: " + info.DataContext + "\nGroup Name Path: " + info.GroupNamePath + "\nHyperlink: " + info.Hyperlink;
            //string name = info.Text;
            //if (type == "group")
            //{
            //    bindreportmenuitem(name);
            //}
            //else
            //{
            //    string size = "",id="";
            //    if (name.Substring(0, 2).Contains("'"))
            //    {
            //        size = name.Substring(0, 2);
            //        name = name.Substring(2);
            //    }
            //    string q = "select id from menuitem where name ='"+name+"'";
            //    DataSet ds = new DataSet();
            //    ds = objCore.funGetDataSet(q);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        id = ds.Tables[0].Rows[0][0].ToString();
            //    }
            //    if (size.Length > 0)
            //    {
            //        q = "select id from ModifierFlavour where MenuItemId ='" + id + "'";
            //        ds = new DataSet();
            //        ds = objCore.funGetDataSet(q);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            size = ds.Tables[0].Rows[0][0].ToString();
            //        }
            //    }
            //    bindreportcost(id, size,size+ name);
            //}
            //vButton2.Visible = true;
            //MessageBox.Show(sObjectProperties);
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
           
            vButton2.Visible = true;
        }
    }
}
