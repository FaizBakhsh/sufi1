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
    public partial class frmDiscard2 : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmDiscard2()
        {
            InitializeComponent();
        }
       
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet dsi = new DataSet();
                string q = "select id,itemname from rawitem";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["itemname"] = "All";
                cmbbranch.DataSource = dsi.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "itemname";
                cmbbranch.Text = "All";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();
                POSRestaurant.Reports.Inventory.rptDiscard rptDoc = new rptDiscard();
                POSRestaurant.Reports.Inventory.dsdiscard dsrpt = new dsdiscard();
                //feereport ds = new feereport(); // .xsd file name
                getcompany();
                dt = getAllOrders();
                // Just set the name of data table
                dt.TableName = "Crystal Report";
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
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", " For the period of " + dateTimePicker1.Text+" to "+dateTimePicker2.Text);


                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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

            DateTime end1 = Convert.ToDateTime(date2);
            DateTime start1 = Convert.ToDateTime(date);
            start1 = start1.AddDays(1);

          string start = start1.ToString("yyyy-MM-dd");
          string end = end1.ToString("yyyy-MM-dd"); 
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
            string q = "SELECT top 1 date, (remaining) as rem FROM     discard where Date <'" + date + "' and itemid='" + itemid + "'   and remaining is not null  order by Date desc";
            q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid ='" + cmbbranch.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "'   order by Date desc";
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

        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        double closingamount = 0, wastage = 0;
        public DataTable getAllOrders()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, minorder = 0, balance = 0, closing = 0;
            double qty = 0;
            DataTable ds = new DataTable();
            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Discard", typeof(double));
                dtrpt.Columns.Add("Remaining", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("date", typeof(DateTime));
                dtrpt.Columns.Add("waste", typeof(double));
                dtrpt.Columns.Add("CompleteWaste", typeof(double));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }

                string q = "";
                if (cmbbranch.Text == "All")
                {
                    // q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id where dbo.MenuItem.status='active' order by dbo.RawItem.ItemName";
                    q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id order by dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbbranch.SelectedValue + "' order by dbo.RawItem.ItemName";
                    q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.rawitem.id='"+cmbbranch.SelectedValue+"' order by dbo.RawItem.ItemName";
             
                }
                DataSet ds1 = new DataSet();
                ds1 = objcore.funGetDataSet(q);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    purchased = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; closing = 0;
                    double cmplt = 0;
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
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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
                    qty = qty + purchased;
                    dspurchase = new DataSet();
                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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
                    val = "";
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

                    dspurchase = new DataSet();
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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

                    if (rate > 0)
                    {
                        cmplt = cmplt / rate;
                    }
                    double tint = 0, tout = 0;
                    DataSet dsin = new DataSet();
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    dsin = objCore.funGetDataSet(q);
                    if (dsin.Tables[0].Rows.Count > 0)
                    {
                        val = dsin.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        tint = Convert.ToDouble(val);
                    }
                    dsin = new DataSet();
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    dsin = objCore.funGetDataSet(q);
                    if (dsin.Tables[0].Rows.Count > 0)
                    {
                        val = dsin.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        tout = Convert.ToDouble(val);
                    }


                    // discard = discard * -1;
                    qty = qty - (discard * -1);
                    qty = qty - (staff);
                    qty = qty - (cmplt);
                    qty = qty + tint;
                    qty = qty - tout;
                    qty = Math.Round(qty, 2);
                    discard = 0;
                    string date2 = "";
                    string tempchk = "yes";
                    q = "SELECT   top 1   remaining,date FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' order by date desc";
                    q = "SELECT     remaining,userid,date FROM     closing where branchid ='" + cmbbranch.SelectedValue + "' and Date ='" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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
                            if (Convert.ToDateTime(date2) < Convert.ToDateTime(dateTimePicker2.Text))
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
                    actual = actual - closing;
                    //if (dspurchase.Tables[0].Rows.Count > 0)
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
                    if (ds1.Tables[0].Rows[i]["id"].ToString() == "115")
                    {

                    }
                    double price2 = 0;
                    
                    double closingval = 0, purchaseval = 0, saleval = 0, discardval = 0, comptval = 0, staffval = 0;
                    price = getprice(ds1.Tables[0].Rows[i]["id"].ToString());
                    balance = price * discard;
                    wastage = wastage + ((staff + cmplt) * price);
                    closingamount = closingamount + (price2 * closing);
                    closingval = price2 * closing;
                    double openingval = openin * price;
                    purchaseval = purchased * price;
                    saleval = consumed * price;
                    discardval = price * discard;
                    comptval = cmplt * price;
                    staffval = staff * price;

                    double total =  Convert.ToDouble(cmplt) + Convert.ToDouble(staff);
                    total = total * price;
                    double totalvar = Convert.ToDouble(discard) ;
                    totalvar = totalvar * price;

                    getcompany();
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds1.Tables[0].Rows[i]["Itemname"].ToString() + "(" + ds1.Tables[0].Rows[i]["uom"].ToString() + ")", totalvar, discard, total, null, Convert.ToDateTime(dateTimePicker1.Text), staff, cmplt);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds1.Tables[0].Rows[i]["Itemname"].ToString() + "(" + ds1.Tables[0].Rows[i]["uom"].ToString() + ")", totalvar, discard, total, dscompany.Tables[0].Rows[0]["logo"], Convert.ToDateTime(dateTimePicker1.Text), staff, cmplt);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        DataSet dscompany = new DataSet();
        private double getprice(string id)
        {
            if (id == "69")
            {

            }
            double variance = 0, price = 0;
            string val = "";
            DataSet dspurchase = new DataSet();
            string q = "";

            q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryTransfer.ItemId = '" + id + "'  GROUP BY MONTH(Date), YEAR(Date)";

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
                dspurchase = new DataSet();

                q = "SELECT    top 1    (price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date <= '" + dateTimePicker1.Text + "' ) and dbo.InventoryTransfer.ItemId = '" + id + "'  order by date desc ";

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

            }
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";

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

                    q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";

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
                if (price == 0)
                {
                    dspurchase = new DataSet();
                    q = "select price from rawitem where id='" + id + "'";
                    dspurchase = objCore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            val = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            price = Convert.ToDouble(val);
                        }
                        catch (Exception ez)
                        {


                        }
                    }
                }
            }
            return price;
        }
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
