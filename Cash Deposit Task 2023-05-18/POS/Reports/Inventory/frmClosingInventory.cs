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
    public partial class frmClosingInventory : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmClosingInventory()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                cmbstore.DataSource = null;
                cmbstore.Items.Clear();
                
                if (cmbbranch.Text == "All")
                {
                    cmbstore.Items.Add("All");
                }
                else
                {
                    DataSet dsfill = new DataSet();
                    string q = "select * from Stores where BranchId='" + cmbbranch.SelectedValue + "'";
                    dsfill = objCore.funGetDataSet(q);
                    //DataRow dr = dsfill.Tables[0].NewRow();
                   // dr["storename"] = "All";
                    cmbstore.DataSource = dsfill.Tables[0];
                    cmbstore.ValueMember = "id";
                    cmbstore.DisplayMember = "storename";
                   // cmbstore.Text = "All";
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from Category";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr2 = ds.Tables[0].NewRow();
                    dr2["CategoryName"] = "All";

                    ds.Tables[0].Rows.Add(dr2);
                    cmbgroupraw.DataSource = ds.Tables[0];
                    cmbgroupraw.ValueMember = "id";
                    cmbgroupraw.DisplayMember = "CategoryName";
                    cmbgroupraw.Text = "All";
                }

            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            try
            {
                ds = new DataSet();
                string q = "select * from branch";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["branchname"] = "All";
                cmbbranch.DataSource = ds.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";
                //cmbbranch.Text = "All";

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,name from KDS where id>0";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["name"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbkitchen.DataSource = dsi.Tables[0];
                cmbkitchen.ValueMember = "id";
                cmbkitchen.DisplayMember = "name";
                cmbkitchen.Text = "All";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill();
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();
                POSRestaurant.Reports.Inventory.rptClosingInventory rptDoc = new rptClosingInventory();
                POSRestaurant.Reports.Inventory.dsInventoryClosing dsrpt = new  dsInventoryClosing();
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

                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                
                
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "as on " + dateTimePicker1.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double getprice(string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + dateTimePicker1.Text + "','" + dateTimePicker1.Text + "'," + id + ")";
            try
            {
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {
            }

            return cost;
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public double opening(string itemid)
        {
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, purchasereturn = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q ="";
            if (cmbkitchen.Text == "All")
            {
                if (cmbbranch.Text == "All")
                {

                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where Date <'" + date + "' and itemid='" + itemid + "'  order by Date desc";

                }
                else
                {

                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid='" + cmbbranch.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "'  order by Date desc";

                }
            }
            else
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where   Date <'" + date + "' and itemid='" + itemid + "'    and kdsid='" + cmbkitchen.SelectedValue + "'   order by Date desc";
                }
                else
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where  branchid='" + cmbbranch.SelectedValue + "' and  Date <'" + date + "' and itemid='" + itemid + "'    and kdsid='" + cmbkitchen.SelectedValue + "'   order by Date desc";
                
                }
            }
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

            if (cmbkitchen.Text == "All")
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                }
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
            val = "";
            purchased = Math.Round(purchased, 2);

            if (cmbkitchen.Text == "All")
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "' and dbo.PurchasereturnDetails.RawItemId='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch.SelectedValue + "' and  dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "' and dbo.PurchasereturnDetails.RawItemId='" + itemid + "'";
                }
                dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchasereturn = Convert.ToDouble(val);
                }
            }
            val = "";
            purchasereturn = Math.Round(purchasereturn, 2);



            dspurchase = new DataSet();
            if (cmbkitchen.Text == "All")
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
                }
            }
            else
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "' and kdsid='"+cmbkitchen.SelectedValue+"'";
                }
                else
                {
                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
                }
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
            }
            DataSet dsin = new DataSet();
            if (cmbkitchen.Text == "All")
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
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
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
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

                dsin = new DataSet();
               
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    transferout = transferout + Convert.ToDouble(val);
                }


            }
            else
            {
                try
                {
                    dsin = new DataSet();
                    if (cmbbranch.Text == "All")
                    {
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    }
                    else
                    {
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    }
                    dsin = objCore.funGetDataSet(q);
                    if (dsin.Tables[0].Rows.Count > 0)
                    {
                        val = dsin.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        transferin = transferin + Convert.ToDouble(val);
                    }


                }
                catch (Exception ex)
                {

                }
                try
                {
                   
                    q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    dsin = objCore.funGetDataSet(q);
                    if (dsin.Tables[0].Rows.Count > 0)
                    {
                        val = dsin.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        transferout = transferout + Convert.ToDouble(val);
                    }
                }
                catch (Exception ex)
                {

                }
            }
            transferout = transferout + purchasereturn;
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
            
            dspurchase = new DataSet();
            if (cmbkitchen.Text == "All")
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
            }
            else
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid='"+cmbkitchen.SelectedValue+"'";
                }
                else
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
                }
            }
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
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("ItemCode", typeof(string));
                dtrpt.Columns.Add("Description", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("Location", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("Price", typeof(double));
                dtrpt.Columns.Add("MinOrder", typeof(double));
                dtrpt.Columns.Add("Balance", typeof(double));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                double purchased = 0, purchasereturn = 0, consumed = 0, variance = 0, price = 0, discard = 0, tint = 0, tout = 0, minorder = 0, max = 0, balance = 0;
                DataSet ds = new DataSet();
                string q = "", date = "" ;
                
                date = dateTimePicker1.Text;
                if (cmbkitchen.Text == "All")
                {
                    if (cmbgroupraw.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = " SELECT     dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM         dbo.RawItem INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)     order by dbo.RawItem.ItemName";
                        }
                        else
                        {
                            q = " SELECT     dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM         dbo.RawItem INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  dbo.RawItem.ItemName like '%" + textBox1.Text + "%'  order by dbo.RawItem.ItemName";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = " SELECT     dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM         dbo.RawItem INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "'  order by dbo.RawItem.ItemName";
                        }
                        else
                        {
                            q = " SELECT     dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM         dbo.RawItem INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%'  order by dbo.RawItem.ItemName";
                        }
                    }
                }
                else
                {
                    if (cmbgroupraw.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {

                            q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and      (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                        }
                        else
                        {
                            q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and      (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' order by dbo.RawItem.ItemName";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and       (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' order by dbo.RawItem.ItemName";
                        }
                        else
                        {
                            q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and      (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' order by dbo.RawItem.ItemName";
                        }
                    }
                }
                 string val = "";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    
                    consumed = 0;
                    double openin = opening(ds.Tables[0].Rows[i]["id"].ToString());
                    tint = 0; tout = 0; 
                    minorder = 0;balance = 0;
                    string temp = ds.Tables[0].Rows[i]["MinOrder"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    minorder = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[i]["maxorder"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    max = Convert.ToDouble(temp);

                    DataSet dspurchase = new DataSet();
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date ='" + date + "'  and dbo.PurchaseDetails.RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date ='" + date + "'  and dbo.Purchase.BranchCode ='" + cmbbranch.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        dspurchase = objCore.funGetDataSet(q);
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
                    else
                    {                        
                    }

                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where dbo.PurchasereturnDetails.date ='" + date + "'  and dbo.PurchaseDetails.RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where dbo.PurchasereturnDetails.date ='" + date + "'  and dbo.Purchase.BranchCode ='" + cmbbranch.SelectedValue + "'  and dbo.PurchasereturnDetails.RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        dspurchase = objCore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            val = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            purchasereturn = Convert.ToDouble(val);
                        }
                    }
                    else
                    {
                    }
                    try
                    {
                        dspurchase = new DataSet();
                        if (cmbkitchen.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date = '" + dateTimePicker1.Text + "' ) and dbo.Production.ItemId='" + ds.Tables[0].Rows[i]["rid"].ToString() + "' and dbo.Production.status='Posted'";
                            }
                            else
                            {

                                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date = '" + dateTimePicker1.Text + "' ) and dbo.Production.ItemId='" + ds.Tables[0].Rows[i]["rid"].ToString() + "'  and dbo.Production.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Production.status='Posted'";
                            }
                        }
                        else
                        {
                            q = "";
                        }
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
                    dspurchase = new DataSet();
                    price = getprice(ds.Tables[0].Rows[i]["id"].ToString());
                    double rate = 0;
                    DataSet dscon = new DataSet();
                    q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                    dscon = objCore.funGetDataSet(q);
                    if (dscon.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    dspurchase = new DataSet();
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date ='" + date + "' and RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid='" + cmbbranch.SelectedValue + "' and Date ='" + date + "' and RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        dspurchase = objCore.funGetDataSet(q);
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
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date ='" + date + "' and RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid='" + cmbbranch.SelectedValue + "' and Date ='" + date + "' and RawItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                        }
                        dspurchase = objCore.funGetDataSet(q);
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
                   val = "";
                   consumed = consumed / rate;                   
                   DataSet dstin = new DataSet();
                   if (cmbkitchen.Text == "All")
                   {
                       if (cmbbranch.Text == "All")
                       {
                           q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                       }
                       else
                       {
                           q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where branchid='" + cmbbranch.SelectedValue + "' and Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                       }
                       dstin = objCore.funGetDataSet(q);
                       if (dstin.Tables[0].Rows.Count > 0)
                       {
                           val = dstin.Tables[0].Rows[0][0].ToString();
                           if (val == "")
                           {
                               val = "0";
                           }
                           tint = Convert.ToDouble(val);
                       }
                       dstin = new DataSet();
                       if (cmbbranch.Text == "All")
                       {
                           q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                       }
                       else
                       {
                           q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where branchid='" + cmbbranch.SelectedValue + "' and  Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                       }
                       dstin = objCore.funGetDataSet(q);
                       if (dstin.Tables[0].Rows.Count > 0)
                       {
                           val = dstin.Tables[0].Rows[0][0].ToString();
                           if (val == "")
                           {
                               val = "0";
                           }
                           tout = Convert.ToDouble(val);
                       }

                       dstin = new DataSet();
                       
                       {
                           q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch.SelectedValue + "' and  Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                       }
                       dstin = objCore.funGetDataSet(q);
                       if (dstin.Tables[0].Rows.Count > 0)
                       {
                           val = dstin.Tables[0].Rows[0][0].ToString();
                           if (val == "")
                           {
                               val = "0";
                           }
                           tout = tout + Convert.ToDouble(val);
                       }
                   }
                   else
                   {
                       try
                       {
                           q = "";

                           {
                               dstin = new DataSet();
                               if (cmbbranch.Text == "All")
                               {
                                   q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date = '" + dateTimePicker1.Text + "'  and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                               }
                               else
                               {
                                   q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch.SelectedValue + "' and  Date = '" + dateTimePicker1.Text + "'  and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                               }
                               dstin = objCore.funGetDataSet(q);
                               if (dstin.Tables[0].Rows.Count > 0)
                               {
                                   val = dstin.Tables[0].Rows[0][0].ToString();
                                   if (val == "")
                                   {
                                       val = "0";
                                   }
                                   tint = tint + Convert.ToDouble(val);
                               }
                           }

                       }
                       catch (Exception ex)
                       {

                       }
                       try
                       {
                           dstin = new DataSet();
                           q = "";
                           if (cmbkitchen.Text == "All")
                           {
                               q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "' and Date = '" + dateTimePicker1.Text + "'  and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                           }
                           else
                           {
                               q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch.SelectedValue + "' and  Date = '" + dateTimePicker1.Text + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                           }

                           dstin = objCore.funGetDataSet(q);
                           if (dstin.Tables[0].Rows.Count > 0)

                               val = dstin.Tables[0].Rows[0][0].ToString();
                           if (val == "")
                           {
                               val = "0";
                           }
                           tout = tout + Convert.ToDouble(val);

                       }
                       catch (Exception ex)
                       {

                       }
                   }
                   tout = tout + purchasereturn;
                    double qty =0,waste=0,cwast=0;
                    qty = purchased - consumed;
                    dspurchase = new DataSet();
                   
                    {
                        qty = qty + (variance);
                    }
                    dspurchase = new DataSet();
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(discard) AS Expr1, SUM(staff) AS staff, SUM(completewaste) AS completewaste FROM     discard where Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(discard) AS Expr1, SUM(staff) AS staff, SUM(completewaste) AS completewaste FROM     discard where branchid='" + cmbbranch.SelectedValue + "' and  Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(discard) AS Expr1, SUM(staff) AS staff, SUM(completewaste) AS completewaste FROM     discard where Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(discard) AS Expr1, SUM(staff) AS staff, SUM(completewaste) AS completewaste FROM     discard where branchid='" + cmbbranch.SelectedValue + "' and  Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
                        }
                    }
                    dspurchase = objCore.funGetDataSet(q);
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
                        waste = Convert.ToDouble(val);
                        val = dspurchase.Tables[0].Rows[0][2].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        cwast = Convert.ToDouble(val);
                    }
                    if (rate > 0)
                    {
                        cwast = cwast / rate;
                    }
                    discard = discard * -1;
                    qty = 0;
                    qty = (purchased+  tint) - (discard+tout + waste + cwast+consumed);
                    qty = Math.Round(qty, 3);
                    double closing = 0;
                    string tempchk = "yes";
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT   top 1  remaining FROM     closing where Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' order by date desc";
                        }
                        else
                        {
                            q = "SELECT   top 1  remaining FROM     closing where branchid='" + cmbbranch.SelectedValue + "' and  Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' order by date desc";
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT   top 1  remaining FROM     closing where Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='"+cmbkitchen.SelectedValue+"' order by date desc";
                        }
                        else
                        {
                            q = "SELECT   top 1  remaining FROM     closing where branchid='" + cmbbranch.SelectedValue + "' and  Date ='" + date + "' and itemid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + cmbkitchen.SelectedValue + "'  order by date desc";
                        }
                    }
                    dspurchase = objCore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        val = dspurchase.Tables[0].Rows[0]["remaining"].ToString();
                        if (val == "")
                        {
                            tempchk = "no";
                            val = "0";
                        }
                        else
                        {

                            closing = Convert.ToDouble(val);
                        }
                    }
                    else
                    {
                        tempchk = "no";
                    }


                    double actual = (openin+purchased + tint) - (waste + cwast + tout);
                    actual = actual - closing;
                    if ( tempchk == "yes")
                    {
                        //if (consumed > 0)
                        {
                            discard = consumed - actual;
                        }
                    }
                    else
                    {
                        closing = actual;
                        closing = closing - consumed;
                    }


                    balance = closing - minorder;
                    double amount = price * closing;
                    if (checkBox1.Checked == false)
                    {
                        if (closing != 0)
                        {
                            if (logo == "")
                            {
                                dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), closing.ToString(), amount, "", null, price, minorder, max);
                            }
                            else
                            {
                                dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), closing.ToString(), amount, "", dscompany.Tables[0].Rows[0]["logo"], price, minorder, max);
                            }
                        }
                    }
                    else
                    {

                        if (logo == "")
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), closing.ToString(), amount, "", null, price, minorder, max);
                        }
                        else
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), closing.ToString(), amount, "", dscompany.Tables[0].Rows[0]["logo"], price, minorder, max);
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
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void InitializeComponent()
        {
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbkitchen = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbgroupraw = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbstore = new System.Windows.Forms.ComboBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbbranch = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.cmbkitchen);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.cmbgroupraw);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.textBox1);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cmbstore);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.vButton1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cmbbranch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1169, 146);
            this.panel2.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 95);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 20);
            this.label7.TabIndex = 41;
            this.label7.Text = "Kitchen";
            // 
            // cmbkitchen
            // 
            this.cmbkitchen.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbkitchen.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbkitchen.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbkitchen.FormattingEnabled = true;
            this.cmbkitchen.Location = new System.Drawing.Point(123, 92);
            this.cmbkitchen.Name = "cmbkitchen";
            this.cmbkitchen.Size = new System.Drawing.Size(247, 28);
            this.cmbkitchen.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(377, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 20);
            this.label8.TabIndex = 39;
            this.label8.Text = "Category";
            // 
            // cmbgroupraw
            // 
            this.cmbgroupraw.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbgroupraw.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbgroupraw.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbgroupraw.FormattingEnabled = true;
            this.cmbgroupraw.Location = new System.Drawing.Point(475, 54);
            this.cmbgroupraw.Name = "cmbgroupraw";
            this.cmbgroupraw.Size = new System.Drawing.Size(246, 28);
            this.cmbgroupraw.TabIndex = 38;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(377, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 20);
            this.label2.TabIndex = 14;
            this.label2.Text = "Keyword";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.textBox1.Location = new System.Drawing.Point(475, 18);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(246, 26);
            this.textBox1.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(727, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 20);
            this.label4.TabIndex = 12;
            this.label4.Text = "Store";
            this.label4.Visible = false;
            // 
            // cmbstore
            // 
            this.cmbstore.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbstore.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbstore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbstore.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbstore.FormattingEnabled = true;
            this.cmbstore.Location = new System.Drawing.Point(825, 12);
            this.cmbstore.Name = "cmbstore";
            this.cmbstore.Size = new System.Drawing.Size(246, 28);
            this.cmbstore.TabIndex = 11;
            this.cmbstore.Visible = false;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(124, 20);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(246, 20);
            this.dateTimePicker1.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Select Date";
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.Location = new System.Drawing.Point(475, 90);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.Size = new System.Drawing.Size(112, 33);
            this.vButton1.TabIndex = 2;
            this.vButton1.Text = "View";
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.ORANGEFRESH;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click_1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Branch";
            // 
            // cmbbranch
            // 
            this.cmbbranch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbbranch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbbranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbbranch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbbranch.FormattingEnabled = true;
            this.cmbbranch.Location = new System.Drawing.Point(124, 54);
            this.cmbbranch.Name = "cmbbranch";
            this.cmbbranch.Size = new System.Drawing.Size(246, 28);
            this.cmbbranch.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 146);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1169, 566);
            this.panel1.TabIndex = 4;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(1169, 566);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(381, 99);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(78, 17);
            this.checkBox1.TabIndex = 42;
            this.checkBox1.Text = "Show Zero";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // frmClosingInventory
            // 
            this.ClientSize = new System.Drawing.Size(1169, 712);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "frmClosingInventory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Closing Inventory";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmClosingInventory_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
