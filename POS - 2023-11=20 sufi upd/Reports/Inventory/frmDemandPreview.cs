using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Inventory
{
    public partial class frmDemandPreview : Form
    {
        public string id = "", date = "", kitchen = "", kdsid = "", demandno = "";
        public frmDemandPreview()
        {
            InitializeComponent();
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

                POSRestaurant.Reports.Inventory.rptdemandpreview rptDoc = new Reports.Inventory.rptdemandpreview();
                POSRestaurant.Reports.Inventory.dsPO dsrpt = new Reports.Inventory.dsPO();
                // .xsd file name
                DataTable dt = new DataTable();
                string company = "", phone = "", address = "", logo = "";
                try
                {
                    getcompany();
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                    date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {

                }
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    rptDoc.SetDataSource(dsrpt);
                    rptDoc.SetParameterValue("Comp", company);
                    rptDoc.SetParameterValue("Addrs", address);
                    rptDoc.SetParameterValue("phn", phone);                                
                    rptDoc.SetParameterValue("demandno", demandno);
                    date = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                    rptDoc.SetParameterValue("date", date);
                    rptDoc.SetParameterValue("kitchen", kitchen);
                    crystalReportViewer1.ReportSource = rptDoc;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("Unit", typeof(string));
                dtrpt.Columns.Add("Price", typeof(double));
                dtrpt.Columns.Add("Total", typeof(double));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {

                }
                string q = "SELECT        dbo.RawItem.ItemName, dbo.RawItem.id, dbo.UOM.UOM, dbo.StoreDemand.Quantity FROM            dbo.StoreDemand INNER JOIN                         dbo.RawItem ON dbo.StoreDemand.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        dbo.StoreDemand.Date='" + date + "' and dbo.StoreDemand.kdsid='" + kdsid + "' and dbo.StoreDemand.Invoiceno='" + demandno + "'";
                dsinfo = objCore.funGetDataSet(q);
                for (int i = 0; i < dsinfo.Tables[0].Rows.Count; i++)
                {
                    double closing = getclosing(dsinfo.Tables[0].Rows[i]["id"].ToString());
                    string temp = dsinfo.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    double required = 0;
                    if (qty > 0 && closing < qty)
                    {
                        required = qty - closing;

                    }
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["ItemName"].ToString(), dsinfo.Tables[0].Rows[i]["Quantity"].ToString(), null, dsinfo.Tables[0].Rows[i]["UOM"].ToString(), closing, required);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["ItemName"].ToString(), dsinfo.Tables[0].Rows[i]["Quantity"].ToString(), dscompany.Tables[0].Rows[0]["logo"], dsinfo.Tables[0].Rows[i]["UOM"].ToString(), closing, required);
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return dtrpt;
        }
        public double openingclosing(string itemid, string date, double closing)
        {

            string date2 = date;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            q = "SELECT top 1 date, (remaining) as rem FROM     closing where  Date <'" + date + "' and itemid='" + itemid + "' order by Date desc";

            DateTime end1 = Convert.ToDateTime(date2);
            DateTime start1 = Convert.ToDateTime(date);
            start1 = start1.AddDays(1);

            string start = start1.ToString("yyyy-MM-dd");
            string end = end1.ToString("yyyy-MM-dd");
            q = "";
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {

            }

            try
            {
                q = "";
                dspurchase = new DataSet();
                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";

                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = purchased + Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            val = ""; q = "";
            purchased = Math.Round(purchased, 2);
            try
            {
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
            }
            catch (Exception ex)
            {

            }

            dspurchase = new DataSet();

            q = "";
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {

            }
            q = "";
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {

            }

            q = "";
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
            q = "";
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {


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
        public double opening(string itemid)
        {
           

            string date2 = "";
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            q = "SELECT top 1 date, (remaining) as rem FROM     closing where Date <'" + date + "' and itemid='" + itemid + "'     order by Date desc";

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
            DateTime end1 = Convert.ToDateTime(date);
            DateTime start1 = Convert.ToDateTime(date2);
            TimeSpan ts = Convert.ToDateTime(date) - Convert.ToDateTime(date2);
            int days = ts.Days;
            if (days <= 1)
            {
                return closing;
            }
            start1 = start1.AddDays(1);
            end1 = end1.AddDays(-1);
            string start = start1.ToString("yyyy-MM-dd");
            string end = end1.ToString("yyyy-MM-dd");
            q = "";
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {


            } q = "";
            try
            {
                dspurchase = new DataSet();
                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";

                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = purchased + Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            val = "";
            purchased = Math.Round(purchased, 2);
            q = "";
            dspurchase = new DataSet();
            try
            {
                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";

            }
            catch (Exception ex)
            {

            }
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                consumed = Convert.ToDouble(val);
            } q = "";
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {

            }
            q = "";

            q = "";
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {

            }

            q = "";
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
            q = "";
            dspurchase = new DataSet();
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
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
            }
            catch (Exception ex)
            {

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


        double closingamount = 0, wastage = 0;
        public double getclosing(string id)
        {
            
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, minorder = 0, balance = 0, closing = 0;
            double qty = 0;
            DataTable ds = new DataTable();
            DataTable dtrpt = new DataTable();
            try
            {



                string q = "";
                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.Id='" + id + "' order by dbo.RawItem.ItemName";


                DataSet ds1 = new DataSet();
                ds1 = objcore.funGetDataSet(q);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    purchased = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; closing = 0;
                    double cmplt = 0;
                    if (ds1.Tables[0].Rows[i]["id"].ToString() == "224")
                    {

                    }
                    double openin = opening(ds1.Tables[0].Rows[i]["id"].ToString());
                    qty = openin;
                    string val = "";
                    double rem = 0;
                    minorder = 0; balance = 0;
                    string temp = ds1.Tables[0].Rows[i]["MinOrder"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    minorder = Convert.ToDouble(temp);
                    DataSet dspurchase = new DataSet();

                    try
                    {
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
                    }
                    catch (Exception ex)
                    {

                    }

                    val = "";
                    purchased = Math.Round(purchased, 2);
                    //qty = qty + purchased;



                    val = "";
                    purchased = Math.Round(purchased, 2);
                    qty = qty + purchased;

                    q = "";
                    dspurchase = new DataSet();
                    try
                    {
                        q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where   Date between '" + date + "' and '" + date + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

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
                    }
                    catch (Exception ex)
                    {

                    }
                    val = ""; q = "";
                    double rate = 0;
                    DataSet dscon = new DataSet();
                    q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    dscon = objcore.funGetDataSet(q);
                    if (dscon.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    consumed = consumed / rate;

                    qty = qty - consumed;
                    dspurchase = new DataSet();
                    q = "";
                    dspurchase = new DataSet();
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where Date between '" + date + "' and '" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                    try
                    {
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
                            cmplt = Convert.ToDouble(val);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    q = "";
                    if (rate > 0)
                    {
                        cmplt = cmplt / rate;
                    }
                    double tint = 0, tout = 0;
                    DataSet dsin = new DataSet();
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + date + "' and '" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                    try
                    {
                        dsin = objcore.funGetDataSet(q);
                        if (dsin.Tables[0].Rows.Count > 0)
                        {
                            val = dsin.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            tint = Convert.ToDouble(val);
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    q = "";
                    dsin = new DataSet();
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + date + "' and '" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                    try
                    {
                        dsin = objcore.funGetDataSet(q);
                        if (dsin.Tables[0].Rows.Count > 0)
                        {
                            val = dsin.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            tout = Convert.ToDouble(val);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    double ideal = 0;
                    // discard = discard * -1;
                    qty = qty - (discard * -1);
                    qty = qty - (staff);
                    qty = qty - (cmplt);
                    qty = qty + tint;
                    qty = qty - tout;
                    qty = Math.Round(qty, 2);
                    discard = 0;
                    string date2 = "";
                    string tempchk = "yes"; q = "";
                    q = "SELECT   top 1   remaining,date FROM     closing where Date between '" + date + "' and '" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' order by date desc";

                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        date2 = dspurchase.Tables[0].Rows[0]["date"].ToString();
                        val = dspurchase.Tables[0].Rows[0]["remaining"].ToString();
                        if (val == "")
                        {
                            tempchk = "no";
                            val = "0";
                        }
                        else
                        {
                            closing = Convert.ToDouble(val);
                            if (date2 == "")
                            {
                                date2 = date;
                            }
                            if (Convert.ToDateTime(date2) < Convert.ToDateTime(date))
                            {
                                closing = closing + openingclosing(ds1.Tables[0].Rows[i]["id"].ToString(), date2, closing);
                            }
                        }
                    }
                    else
                    {
                        tempchk = "no";
                    }


                    double actual = (openin + purchased + tint) - (staff + cmplt + tout);
                    double actual1 = (openin + purchased + tint) - (staff + cmplt + tout);
                    actual = actual - closing;
                    if (tempchk == "yes")
                    {

                        {
                            discard = consumed - actual;
                        }
                    }
                    else
                    {
                        closing = actual;
                        closing = closing - consumed;
                    }
                    ideal = actual1 - consumed;



                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return closing;
        }
        private void frmInventoryPreview_Load(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
