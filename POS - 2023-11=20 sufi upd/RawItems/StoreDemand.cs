using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class StoreDemand : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public StoreDemand()
        {
            InitializeComponent();
        }
        public void getdata(string date,string invoice,string kdsid)
        {
            dateTimePicker1.Text = date;
            textBox2.Text = invoice;
            cmbkitchen.SelectedValue = kdsid;
            getdata("");
        }
        public void getdata(string search)
        {
            string date = dateTimePicker1.Text;
           
            double qty = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("Unit", typeof(string));          
            ds.Columns.Add("Quantity", typeof(string));
            ds.Columns.Add("Kitchen Stock", typeof(string));
            //ds.Columns.Add("Store Stock", typeof(string));
            ds.Columns.Add("Required", typeof(string));
            ds.Columns.Add("Unit Price", typeof(string));
            ds.Columns.Add("Total Amount", typeof(string));
            ds.Columns.Add("Status", typeof(string));
            string q = "";// "SELECT     dbo.RawItem.id,dbo.RawItem.ItemName, dbo.InventoryConsumed.RemainingQuantity FROM         dbo.RawItem INNER JOIN                      dbo.InventoryConsumed ON dbo.RawItem.Id = dbo.InventoryConsumed.RawItemId where dbo.InventoryConsumed.date='" + dateTimePicker1.Text + "'";
            if (cmbcategory.Text == "All")
            {
                if (textBox1.Text.Trim() == "")
                {
                    q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.itemname like '%" + textBox1.Text + "%'  order by dbo.RawItem.ItemName";
                }
            }
            else
            {
                if (textBox1.Text.Trim() == "")
                {
                    q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' order by dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  and dbo.RawItem.itemname like '%" + textBox1.Text + "%'  order by dbo.RawItem.ItemName";

                }
            }

            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                qty = 0;
                double total = 0,price=0;
                string sts = "Pending";
                q = "SELECT        id, Itemid, Date, Quantity, Status FROM            StoreDemand where itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and date='" + dateTimePicker1.Text + "'  and kdsid='" + cmbkitchen.SelectedValue + "' and invoiceno='" + textBox2.Text + "'";
                DataSet dsdmnd = new DataSet();
                dsdmnd = objcore.funGetDataSet(q);
                if (dsdmnd.Tables[0].Rows.Count > 0)
                {

                    sts = dsdmnd.Tables[0].Rows[0]["Status"].ToString();
                    string tmp = dsdmnd.Tables[0].Rows[0]["Quantity"].ToString();
                    if (tmp == "")
                    {
                        tmp = "0";
                    }
                    qty = Convert.ToDouble(tmp);

                }
                string tmp1 = ds1.Tables[0].Rows[i]["price"].ToString();
                if (tmp1 == "")
                {
                    tmp1 = "0";
                }
                price = Convert.ToDouble(tmp1);
                total = price * qty;
                Math.Round(total, 2);
                double closing = 0,closingkitchen=0;

                try
                {
                    //closing = getclosing(ds1.Tables[0].Rows[i]["id"].ToString());
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    closingkitchen = getclosingkitchen(ds1.Tables[0].Rows[i]["id"].ToString());
                }
                catch (Exception ex)
                {


                }
                double required = 0;
                if (qty > 0 && closing < qty)
                {
                    required = qty - closing;
                   
                }
                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), ds1.Tables[0].Rows[i]["uom"].ToString(), qty.ToString(),closingkitchen,  required, price, total, sts);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = false;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].ReadOnly = true;
            //dataGridView1.Columns[8].ReadOnly = true;
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            {
                if (gr.Cells["Status"].Value.ToString() == "Posted")
                {
                    gr.ReadOnly = true;
                    gr.DefaultCellStyle.BackColor = Color.Green;
                    gr.ReadOnly = true;
                }
            }
        }
        protected double getclosingkitchen(string id)
        {
            double qty = 0;
            double purchase = 0, consumed = 0;
            try
            {

                string q = "SELECT  SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.UOM.UOM FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE    dbo.InventoryConsumed.RawItemId='" + id + "' and (dbo.InventoryConsumed.Date <= '" + dateTimePicker1.Text + "') and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and (dbo.InventoryConsumed.QuantityConsumed > 0) GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM order by dbo.RawItem.ItemName";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataSet dspurchase = new DataSet();
                    string val = "";



                    val = ds.Tables[0].Rows[i]["QuantityConsumed"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    consumed = Convert.ToDouble(val);
                }

                DataSet dsin = new DataSet();


                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and   Date <= '" + dateTimePicker1.Text + "' and itemid='" + id + "'";

                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    string val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchase = Convert.ToDouble(val);
                }


            }
            catch (Exception ex)
            {

            }
            qty = purchase - consumed;
            return qty;
        }
        public double openingclosing(string itemid, string date, double closing)
        {

            string date2 = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            q = "SELECT top 1 date, (remaining) as rem FROM     closing where  Date <'" + date + "' and itemid='" + itemid + "' adn kdsid is NULL order by Date desc";

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
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid is null";
              
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
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            q = "SELECT top 1 date, (remaining) as rem FROM     closing where Date <'" + date + "' and itemid='" + itemid + "'  and kdsid is NULL   order by Date desc";

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
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid is NULL";
              
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
            string date = dateTimePicker1.Text;
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
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date ='" + dateTimePicker1.Text + "'  and dbo.PurchaseDetails.RawItemId='" + id + "'";
              
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
                    //try
                    //{
                    //    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                          
                    //    dspurchase = objcore.funGetDataSet(q);
                    //    if (dspurchase.Tables[0].Rows.Count > 0)
                    //    {
                    //        val = dspurchase.Tables[0].Rows[0][0].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        consumed = Convert.ToDouble(val);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
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
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                      
                    //try
                    //{
                    //    dspurchase = objcore.funGetDataSet(q);
                    //    if (dspurchase.Tables[0].Rows.Count > 0)
                    //    {
                    //        val = dspurchase.Tables[0].Rows[0][0].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        discard = Convert.ToDouble(val);

                    //        val = dspurchase.Tables[0].Rows[0][1].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        staff = Convert.ToDouble(val);
                    //        val = dspurchase.Tables[0].Rows[0][2].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        cmplt = Convert.ToDouble(val);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    //q = "";
                    //if (rate > 0)
                    //{
                    //    cmplt = cmplt / rate;
                    //}
                    double tint = 0, tout = 0;
                    DataSet dsin = new DataSet();
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                       
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
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                      
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
                    try
                    {
                        q = "";


                        dsin = new DataSet();


                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  Date <= '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                        dsin = objcore.funGetDataSet(q);
                        if (dsin.Tables[0].Rows.Count > 0)
                        {
                            val = dsin.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            tout = tout + Convert.ToDouble(val);
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
                    q = "SELECT   top 1   remaining,date FROM     closing where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and kdsid is NULL order by date desc";

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
                            if (Convert.ToDateTime(date2) < Convert.ToDateTime(dateTimePicker1.Text))
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



                    //double closingval = 0, purchaseval = 0, saleval = 0, discardval = 0, comptval = 0, staffval = 0;
                  
                    //balance = price * discard;
                    //wastage = wastage + ((staff + cmplt) * price);
                    //closingamount = closingamount + (price * closing);
                    //closingval = price * closing;
                    //double openingval = openin * price;
                    //purchaseval = purchased * price;
                    //saleval = consumed * price;
                    //discardval = price * discard;
                    //comptval = cmplt * price;
                    //staffval = staff * price;

                   
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return closing;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Text = "Fetching Data";
            vButton1.Enabled = false;
            getdata(textBox1.Text);
            vButton1.Text = "Search";
            vButton1.Enabled = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
            try
            {
                if (e.ColumnIndex == 3)
                {
                    if (textBox2.Text == "")
                    {
                        MessageBox.Show("Please Enter Demand No");
                        return;
                    }
                    string temp = dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    temp = dataGridView1.Rows[e.RowIndex].Cells["Unit Price"].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double price = Convert.ToDouble(temp);
                    double total = qty * price;
                    total = Math.Round(total, 2);
                    double closing=0, required = 0;
                    //temp = dataGridView1.Rows[e.RowIndex].Cells["Store Stock"].Value.ToString();
                    temp = "";
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    closing = Convert.ToDouble(temp);
                    if (closing < qty)
                    {
                        required = qty - closing;
                        dataGridView1.Rows[e.RowIndex].Cells["Required"].Value = required.ToString();
                    }

                    dataGridView1.Rows[e.RowIndex].Cells["Total Amount"].Value = total.ToString();
                    DataSet dss = new DataSet();
                    int id = 0;

                    string q = "select * from rawitem where id='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
                    DataSet dsitem = new DataSet();
                    dsitem = objcore.funGetDataSet(q);
                    if (dsitem.Tables[0].Rows.Count > 0)
                    {
                        string temp1 = dsitem.Tables[0].Rows[0]["maxorder"].ToString();
                        if (temp1 == "")
                        {
                            temp1 = "0";
                        }
                        if (Convert.ToDouble(temp1) > 0 && Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()) > Convert.ToDouble(temp1))
                        {
                            MessageBox.Show("Your Limit for this Item is  '" + temp1 + "' ");
                            dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value = "0";

                            dss = new DataSet();
                            q = "select * from StoreDemand where itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and kdsid='" + cmbkitchen.SelectedValue + "'  and invoiceno='" + textBox2.Text + "'";
                            dss = objcore.funGetDataSet(q);
                            if (dss.Tables[0].Rows.Count > 0)
                            {

                                q = "update StoreDemand set kdsid='" + cmbkitchen.SelectedValue + "', Quantity='0'  where   itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'  and invoiceno='" + textBox2.Text + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
                                objcore.executeQuery(q);
                            }

                            return;
                        }
                    }
                    dss = new DataSet();
                    q = "select * from StoreDemand where itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and kdsid='" + cmbkitchen.SelectedValue + "' and invoiceno='" + textBox2.Text + "'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {

                        q = "update StoreDemand set kdsid='" + cmbkitchen.SelectedValue + "', Quantity='" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()), 2).ToString() + "'  where   itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'  and invoiceno='" + textBox2.Text + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
                        objcore.executeQuery(q);                        
                    }
                    else
                    {
                        q = "insert into StoreDemand (invoiceno,kdsid,Itemid, Date, Quantity, Status) values('" + textBox2.Text + "','" + cmbkitchen.SelectedValue + "','" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString()), 2).ToString() + "','Pending')";
                        objcore.executeQuery(q);
                        
                    }


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }
        protected void getbranchid()
        {
            string bid = "";
            string q = "select id from Branch ";           
            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                bid = ds1.Tables[0].Rows[0][0].ToString();
            }
            branchid = bid;
            
        }
        string cs = "";
        bool chk1 = false;
        public static string branchid = "", url = "";
        
        private void vButton2_Click(object sender, EventArgs e)
        {
            string q = "update storedemand set status='Posted' where  kdsid='" + cmbkitchen.SelectedValue + "' and date='" + dateTimePicker1.Text + "' and invoiceno='" + textBox2.Text + "'";
            objcore.executeQuery(q);
            getdata(textBox1.Text);
        }
        private void showform()
        {
            try
            {
                //POSRestaurant.Reports.Inventory.frmDiscardemail obj = new Reports.Inventory.frmDiscardemail();
                //obj.date = dateTimePicker1.Text;
                //obj.Show();
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton3_Click(object sender, EventArgs e)
        {
            StoreDemandList obj = new StoreDemandList(this);
            obj.ShowDialog();
            //POSRestaurant.Reports.Inventory.frmStoreIssuancePreview obj = new Reports.Inventory.frmStoreIssuancePreview();
            //obj.kitchenid = cmbkitchen.SelectedValue.ToString();
            //obj.kitchen = cmbkitchen.Text;
            //obj.date = dateTimePicker1.Text;
            //obj.invoiceno = textBox2.Text;
            //obj.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FoodDiscard_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from KDS where id>0";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];
                cmbkitchen.DataSource = dt;
                cmbkitchen.ValueMember = "id";
                cmbkitchen.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Category";
                ds = objcore.funGetDataSet(q);
               
                DataRow dr = ds.Tables[0].NewRow();
                dr["CategoryName"] = "All";
                ds.Tables[0].Rows.Add(dr);
                dt = ds.Tables[0];
                cmbcategory.DataSource = dt;
                cmbcategory.ValueMember = "id";
                cmbcategory.DisplayMember = "CategoryName";
            }
            catch (Exception ex)
            {


            }

        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    
    }
}
