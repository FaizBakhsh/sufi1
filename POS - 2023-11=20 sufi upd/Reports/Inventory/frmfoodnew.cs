using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Inventory
{
    public partial class frmfoodnew : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmfoodnew()
        {
            InitializeComponent();
        }
        public void fillstore()
        {
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,StoreName from Stores where BranchId='" + cmbbranch1.SelectedValue + "'";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["StoreName"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbstore.DataSource = dsi.Tables[0];
                cmbstore.ValueMember = "id";
                cmbstore.DisplayMember = "StoreName";
                cmbstore.Text = "All";

            }
            catch (Exception ex)
            {

            }
        }  public string type = "";
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
               DataSet dsi = new DataSet();
                string q = "select id,name from menugroup where status='active'";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["name"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbgroup.DataSource = dsi.Tables[0];
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
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                ds1.Tables[0].Rows.Add(dr1);
                cmbbranch1.DataSource = ds1.Tables[0];
                cmbbranch1.ValueMember = "id";
                cmbbranch1.DisplayMember = "branchname";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            fillstore();
            getitems();
        }
        protected void getitems()
        {
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,itemname from rawitem where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)    order by itemname ";
                if (cmbgroupraw.Text != "All")
                {
                    q = "select id,itemname from rawitem where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  CategoryId='" + cmbgroupraw.SelectedValue + "'  order by itemname ";
                }
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["itemname"] = "All";
                ds1.Tables[0].Rows.Add(dr1);
                cmbitem.DataSource = ds1.Tables[0];
                cmbitem.ValueMember = "id";
                cmbitem.DisplayMember = "itemname";

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
                POSRestaurant.Reports.Inventory.rptfood rptDoc = new rptfood();
                POSRestaurant.Reports.Inventory.dsfood dsrpt = new dsfood();
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
                rptDoc.SetParameterValue("address", phone);
                rptDoc.SetParameterValue("phone", address);
                rptDoc.SetParameterValue("date", " For the period of " + dateTimePicker1.Text+" to "+dateTimePicker2.Text);
                rptDoc.SetParameterValue("closing", closingamount);
                rptDoc.SetParameterValue("waste", wastage);
                if (type == "value")
                {
                    rptDoc.SetParameterValue("title", "Critical Inventory Report(Value)");
                }
                else
                {
                    rptDoc.SetParameterValue("title", "Critical Inventory Report(Quantitative)");
                }
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }

        }
    
        public double openingclosing(string itemid,string date,double closing)
        {
            
            string date2 = dateTimePicker2.Text;           
            double purchased = 0,purchasreturn=0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            if (cmbkitchen.Text == "All" && checkBox1.Checked==false)
            {
                if (cmbbranch1.Text == "All")
                {

                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where  Date <'" + date + "' and itemid='" + itemid + "'  and kdsid is null  order by Date desc";

                }
                else
                {

                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "'  and kdsid is null  order by Date desc";

                }
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "' and   isnull(kdsid,'') != '' order by Date desc";
                }
                else
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "' order by Date desc";
                }
            }

            DateTime end1 = Convert.ToDateTime(date2);
            DateTime start1 = Convert.ToDateTime(date);
            start1 = start1.AddDays(1);

            DateTime start =Convert.ToDateTime( start1.ToString("yyyy-MM-dd"));
            DateTime end = Convert.ToDateTime(end1.ToString("yyyy-MM-dd"));
            q = "";

            try
            {
                purchased = purchaseopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
                
            }
            catch (Exception ex)
            {

            }
            try
            {
                purchasreturn = purchasereturnopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);

            }
            catch (Exception ex)
            {

            }

            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            //    } 
            //}
            //try
            //{
            //    dspurchase = new DataSet();
            //    dspurchase = objcore.funGetDataSet(q);
            //    if (dspurchase.Tables[0].Rows.Count > 0)
            //    {
            //        val = dspurchase.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        purchased = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}
            try
            {
                purchased =purchased+ Productionopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);

            }
            catch (Exception ex)
            {

            }
            //try
            //{
            //    q = "";
            //    dspurchase = new DataSet();
            //    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //    {
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";
            //        }
            //        else
            //        {

            //            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "'  and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted'";
            //        } 
            //    }
            //    dspurchase = objcore.funGetDataSet(q);
            //    if (dspurchase.Tables[0].Rows.Count > 0)
            //    {
            //        val = dspurchase.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        purchased = purchased + Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{


            //}

            try
            {
                consumed = InventoryConsumedopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);

            }
            catch (Exception ex)
            {

            }


            //val = ""; q = "";
            //purchased = Math.Round(purchased, 2);
            //try
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //        {
            //            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'   ";
            //        }
            //        else
            //        {
            //            if (checkBox1.Checked == true)
            //            {
            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'  ";
                    
            //            }
            //            else
            //            {
            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'  ";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //        {
            //            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'   ";
            //        }
            //        else
            //        {
            //            if (checkBox1.Checked == true)
            //            {
            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
            //            }
            //            else
            //            {

            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
            //            }
            //        }
            //    }
            //    if (cmbstore.Text != "All")
            //    {
            //        q = "";
            //    }
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
            try
            {
                transferin =InventoryTransferInopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);

            }
            catch (Exception ex)
            {

            }
            //dspurchase = new DataSet();

            //q = "";
            DataSet dsin = new DataSet();
            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    } 
            //}
            //try
            //{
            //    dsin = objcore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferin = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}

            try
            {
                transferin =transferin+InventoryTransferStoreInopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);

            }
            catch (Exception ex)
            {

            }
            //try
            //{
            //    q = "";
            //    if (cmbkitchen.Text == "All" || cmbstore.Text != "All")
            //    {
            //    }
            //    else
            //    {
            //        dsin = new DataSet();
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        else
            //        {
            //            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        dsin = objCore.funGetDataSet(q);
            //        if (dsin.Tables[0].Rows.Count > 0)
            //        {
            //            val = dsin.Tables[0].Rows[0][0].ToString();
            //            if (val == "")
            //            {
            //                val = "0";
            //            }
            //            transferin = transferin + Convert.ToDouble(val);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //} q = "";

            try
            {
                transferout =InventoryTransferOutnopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);

            }
            catch (Exception ex)
            {

            }
            transferout = transferout + purchasreturn;
            //dsin = new DataSet();
            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    } 
            //}
            //try
            //{
            //    dsin = objcore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferout = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}

            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {

            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //    }
            //}
            //try
            //{
            //    dsin = objCore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferout = transferout + Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                transferout = transferout + InventoryTransferStoreOutopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);

            }
            catch (Exception ex)
            {

            }
            //try
            //{
            //    q = "";
            //    dsin = new DataSet();
            //    if (cmbstore.Text == "All")
            //    {
            //        if (cmbkitchen.Text == "All")
            //        {
            //           // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        else
            //        {
            //           // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  RecvStoreId='" + cmbkitchen.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //    }
            //    else
            //    {

            //        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //        {
            //            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        else
            //        {
            //            if (checkBox1.Checked == true)
            //            {
            //                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where   IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                  
            //            }
            //            else
            //            {
            //                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and  IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //            }
            //        }
                    
            //    }
            //    dsin = objCore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferout = transferout + Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            q = "";
            val = "";
            double rate = 0;
            try
            {
                double result;
                if (Conversiondict.TryGetValue(Convert.ToInt32(itemid), out result))
                {
                    rate = result;
                }
                //rate = Conversionlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity;

            }
            catch (Exception ex)
            {


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
           


            try
            {
                discard = discardopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);


            }
            catch (Exception ex)
            {

            }
            try
            {
                complt = completeopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);


            }
            catch (Exception ex)
            {

            }
            try
            {
                staff = staffopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);


            }
            catch (Exception ex)
            {

            }
            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
            //    }
            //}
            //else
            //{
            //    if (checkBox1.Checked == true)
            //    {
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and isnull(kdsid,'') != ''";
            //        }
            //        else
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'   and isnull(kdsid,'') != ''";
            //        }
            //    }
            //    else
            //    {
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
            //        }
            //        else
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
            //        }
            //    }
            //}
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
            //        complt = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{
                
                
            //}
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = ( purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        public double opening(string itemid)
        {
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, purchasreturn = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where Date <'" + date + "' and itemid='" + itemid + "'   and kdsid is null    order by Date desc";

                }
                else
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where  branchid='" + cmbbranch1.SelectedValue + "' and  Date <'" + date + "' and itemid='" + itemid + "'   and kdsid is null    order by Date desc";

                }
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT top 1 date, (remaining) as rem FROM     closing where   Date <'" + date + "' and itemid='" + itemid + "'    and isnull(kdsid,'') != ''   order by Date desc";
                    }
                    else
                    {
                        q = "SELECT top 1 date, (remaining) as rem FROM     closing where  branchid='" + cmbbranch1.SelectedValue + "' and  Date <'" + date + "' and itemid='" + itemid + "'    and isnull(kdsid,'') != ''  order by Date desc";

                    }
                }
                else
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT top 1 date, (remaining) as rem FROM     closing where   Date <'" + date + "' and itemid='" + itemid + "'    and kdsid='" + cmbkitchen.SelectedValue + "'   order by Date desc";
                    }
                    else
                    {
                        q = "SELECT top 1 date, (remaining) as rem FROM     closing where  branchid='" + cmbbranch1.SelectedValue + "' and  Date <'" + date + "' and itemid='" + itemid + "'    and kdsid='" + cmbkitchen.SelectedValue + "'   order by Date desc";

                    }
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
            DateTime start = Convert.ToDateTime(start1.ToString("yyyy-MM-dd"));
            DateTime end = Convert.ToDateTime(end1.ToString("yyyy-MM-dd"));
            q = "";

            try
            {
                purchased = purchaseopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date.Date >= start.Date && x.Date.Date <= end.Date).Sum(xx => xx.quantity);
                //purchased = purchaseopeningdict.Find(e => e.ItemId.ToString() == itemid.ToString() && e.Date >= start && e.Date <= end).quantity;
            }
            catch (Exception ex)
            {
              
            }
            try
            {
                purchasreturn = purchasereturnopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date.Date >= start.Date && x.Date.Date <= end.Date).Sum(xx => xx.quantity);
                //purchased = purchaseopeningdict.Find(e => e.ItemId.ToString() == itemid.ToString() && e.Date >= start && e.Date <= end).quantity;
            }
            catch (Exception ex)
            {

            }

            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            //    } 
            //}
            //try
            //{
            //    dspurchase = new DataSet();
            //    dspurchase = objcore.funGetDataSet(q);
            //    if (dspurchase.Tables[0].Rows.Count > 0)
            //    {
            //        val = dspurchase.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        purchased = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{


            //} 
            try
            {
                purchased = purchased + Productionopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
               // purchased = purchased + Productionopeningdict.Find(e => e.ItemId.ToString() == itemid.ToString() && e.Date >= start && e.Date <= end).quantity;
            }
            catch (Exception ex)
            {
                
              
            }
            q = "";
            //try
            //{
            //    dspurchase = new DataSet();
            //    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //    {
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";
            //        }
            //        else
            //        {

            //            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "'  and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted'";
            //        } 
            //    }
            //    dspurchase = objcore.funGetDataSet(q);
            //    if (dspurchase.Tables[0].Rows.Count > 0)
            //    {
            //        val = dspurchase.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        purchased = purchased + Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{


            //}

            try
            {
                consumed = InventoryConsumedopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date.Date >= start.Date && x.Date.Date <= end.Date).Sum(xx => xx.quantity);
               
            }
            catch (Exception ex)
            {

            }


            //val = "";
            //purchased = Math.Round(purchased, 2);
            //q = "";
            //dspurchase = new DataSet();
            //try
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //        {
            //            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'   ";
            //        }
            //        else
            //        {
            //            if (checkBox1.Checked == true)
            //            {
            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "' ";
             
            //            }
            //            else
            //            {
            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "' ";
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //        {
            //            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "' ";
            //        }
            //        else
            //        {
            //            if (checkBox1.Checked == true)
            //            {
            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where   branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
               
            //            }
            //            else
            //            {
            //                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
            //            }
            //        }
            //    }
            //    if (cmbstore.Text != "All")
            //    {
            //        q = "";
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}
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
            //        consumed = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ee)
            //{
                
            //} 

            try
            {
                transferin = InventoryTransferInopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
              
            }
            catch (Exception ex)
            {

            }

            //q = "";
            DataSet dsin = new DataSet();
            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    } 
            //}
            //try
            //{
            //    dsin = objcore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferin = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}
            q = "";
            try
            {
                transferin = transferin + InventoryTransferStoreInopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
                
            }
            catch (Exception ex)
            {

            }
            //try
            //{
            //    if (cmbkitchen.Text == "All" || cmbstore.Text!="All")
            //    {
            //    }
            //    else
            //    {
            //        dsin = new DataSet();
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        else
            //        {
            //            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        dsin = objCore.funGetDataSet(q);
            //        if (dsin.Tables[0].Rows.Count > 0)
            //        {
            //            val = dsin.Tables[0].Rows[0][0].ToString();
            //            if (val == "")
            //            {
            //                val = "0";
            //            }
            //            transferin = transferin + Convert.ToDouble(val);
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{

            //}
            q = "";
            try
            {
                transferout = InventoryTransferOutnopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
             
            }
            catch (Exception ex)
            {

            }
            transferout = purchasreturn + transferout;
            //dsin = new DataSet();
            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            //    } 
            //}
            //try
            //{
            //    dsin = objcore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferout = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}
            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {

            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //    }
            //}
            //try
            //{
            //    dsin = objCore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferout = transferout + Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                transferout = transferout + InventoryTransferStoreInopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
              
            }
            catch (Exception ex)
            {

            }

            //try
            //{
            //    q = "";
            //    dsin = new DataSet();
            //    if (cmbstore.Text == "All")
            //    {
            //        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //        {
            //            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        else
            //        {
            //           // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  RecvStoreId='" + cmbkitchen.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //    }
            //    else
            //    {

            //        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //        {
            //            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //        }
            //        else
            //        {
            //            if (checkBox1.Checked == true)
            //            {
            //                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                 
            //            }
            //            else
            //            {

            //                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
            //            }
            //        }
                    
            //    }
            //    dsin = objCore.funGetDataSet(q);
            //    if (dsin.Tables[0].Rows.Count > 0)
            //    {
            //        val = dsin.Tables[0].Rows[0][0].ToString();
            //        if (val == "")
            //        {
            //            val = "0";
            //        }
            //        transferout = transferout + Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            q = "";
            val = "";
            double rate = 0;
            try
            {
                double result;
                if (Conversiondict.TryGetValue(Convert.ToInt32(itemid), out result))
                {
                    rate = result;
                }
                //rate = Conversionlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity;

            }
            catch (Exception ex)
            {


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


            try
            {
                discard = discardopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
              
               
            }
            catch (Exception ex)
            {

            }
            try
            {
                complt = completeopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
              
               
            }
            catch (Exception ex)
            {

            }
            try
            {
                staff = staffopeningdict.Where(x => x.ItemId.ToString() == itemid.ToString() && x.Date >= start && x.Date <= end).Sum(xx => xx.quantity);
              
                
            }
            catch (Exception ex)
            {

            }

            //q = "";
            //dspurchase = new DataSet();
            //if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            //{
            //    if (cmbbranch1.Text == "All")
            //    {
            //        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
            //    }
            //    else
            //    {
            //        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
            //    }
            //}
            //else
            //{
            //    if (checkBox1.Checked == true)
            //    {
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and isnull(kdsid,'') != '' ";
            //        }
            //        else
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and isnull(kdsid,'') != ''";
            //        }
            //    }
            //    else
            //    {
            //        if (cmbbranch1.Text == "All")
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
            //        }
            //        else
            //        {
            //            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
            //        }
            //    }
            //}
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
            //        complt = Convert.ToDouble(val);
            //    }
            //}
            //catch (Exception ex)
            //{
                
            //}
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (closing + purchased + transferin) - (staff + complt + transferout+consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }

        #region
        public void fillopening()
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


            string start = Convert.ToDateTime(dateTimePicker1.Text).AddMonths(-2).ToString("yyyy-MM-dd");
            string end = Convert.ToDateTime(dateTimePicker2.Text).AddDays(0).ToString("yyyy-MM-dd");
            q = "";
            if (cmbitem.Text == "All")
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                    }
                }
            }
            else
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='"+cmbitem.SelectedValue+"' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                    }
                }
            }
            try
            {
                dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    purchaseopeningdict = new List<OpeningCriticalInventoryClass>();
                    IList<OpeningCriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                               new OpeningCriticalInventoryClass
                               {
                                   ItemId = row.Field<int>("RawItemId"),
                                   quantity = row.Field<double>("Expr1"),
                                   Date = row.Field<DateTime>("Date")
                                   

                               }).ToList();
                    purchaseopeningdict = data.ToList();
                }
            }
            catch (Exception ex)
            {


            } 
            
            q = "";

            if (cmbitem.Text == "All")
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        //q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                        q = "SELECT        SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date FROM            dbo.Purchase INNER JOIN                         dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "' group by dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date";
                     
                    }
                    else
                    {
                       // q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                        q = "SELECT        SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date FROM            dbo.Purchase INNER JOIN                         dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "' group by dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date";
                   
                    }
                }
            }
            else
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                       // q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                        q = "SELECT        SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date FROM            dbo.Purchase INNER JOIN                         dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "'  and dbo.PurchasereturnDetails.RawItemId='" + cmbitem.SelectedValue + "'  group by dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date";
                     

                    }
                    else
                    {
                       // q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' group by dbo.PurchaseDetails.RawItemId,dbo.Purchase.Date";
                        q = "SELECT        SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date FROM            dbo.Purchase INNER JOIN                         dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where   dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "'  and dbo.PurchasereturnDetails.RawItemId='" + cmbitem.SelectedValue + "'  group by dbo.PurchasereturnDetails.RawItemId,dbo.PurchasereturnDetails.Date";
                     
                    }
                }
            }
            try
            {
                dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {

                    purchasereturnopeningdict = new List<OpeningCriticalInventoryClass>();
                    IList<OpeningCriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                               new OpeningCriticalInventoryClass
                               {
                                   ItemId = row.Field<int>("RawItemId"),
                                   quantity = row.Field<double>("Expr1"),
                                   Date = row.Field<DateTime>("Date")


                               }).ToList();
                    purchasereturnopeningdict = data.ToList();
                }
            }
            catch (Exception ex)
            {


            }

            q = "";
            try
            {
                dspurchase = new DataSet();
                if (cmbitem.Text == "All")
                {
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1,dbo.Production.ItemId,date FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "')  and dbo.Production.status='Posted' group by dbo.Production.ItemId,date";
                        }
                        else
                        {

                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1,dbo.Production.ItemId,date FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted' group by dbo.Production.ItemId,date";
                        }
                    }
                }
                else
                {
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1,dbo.Production.ItemId,date FROM         Production  where ItemId='"+cmbitem.SelectedValue+"' and (dbo.Production.date between '" + start + "' and '" + end + "')  and dbo.Production.status='Posted' group by dbo.Production.ItemId,date";
                        }
                        else
                        {

                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1,dbo.Production.ItemId,date FROM         Production  where ItemId='" + cmbitem.SelectedValue + "' and  (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted' group by dbo.Production.ItemId,date";
                        }
                    }
                }
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        Productionopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("ItemId"),
                                       quantity = row.Field<double>("Expr1"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        Productionopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {
                       
                    }
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
                if (cmbitem.Text == "All")
                {
                    if (cmbbranch1.Text == "All")
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "'  group by RawItemId,date  ";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where  Date between '" + start + "' and  '" + end + "'  group by RawItemId,date";

                            }
                            else
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' group by RawItemId,date ";
                            }
                        }
                    }
                    else
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' group by RawItemId,date ";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where   branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' group by RawItemId,date";

                            }
                            else
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where  kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' group by RawItemId,date";
                            }
                        }
                    }
                }
                else
                {
                    if (cmbbranch1.Text == "All")
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where RawItemId='"+cmbitem.SelectedValue+"' and Date between '" + start + "' and  '" + end + "'  group by RawItemId,date  ";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where RawItemId='" + cmbitem.SelectedValue + "' and   Date between '" + start + "' and  '" + end + "'  group by RawItemId,date";

                            }
                            else
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where RawItemId='" + cmbitem.SelectedValue + "' and  kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' group by RawItemId,date ";
                            }
                        }
                    }
                    else
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where RawItemId='" + cmbitem.SelectedValue + "' and   branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' group by RawItemId,date ";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where  RawItemId='" + cmbitem.SelectedValue + "' and   branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' group by RawItemId,date";

                            }
                            else
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId,date FROM     InventoryConsumed where  RawItemId='" + cmbitem.SelectedValue + "' and  kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' group by RawItemId,date";
                            }
                        }
                    }
                }
                if (cmbstore.Text != "All")
                {
                    q = "";
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    try
                    {
                        InventoryConsumedopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("RawItemId"),
                                       quantity = row.Field<double>("Expr1"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        InventoryConsumedopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ee)
            {

            }
            q = "";
            DataSet dsin = new DataSet();
            if (cmbitem.Text == "All")
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(TransferIn) AS Expr1,itemid,date FROM     InventoryTransfer where TransferIn>0 and  Date between '" + start + "' and  '" + end + "' group by  itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(TransferIn) AS Expr1,itemid,date FROM     InventoryTransfer where TransferIn>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' group by  itemid,date";
                    }
                }
            }
            else
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(TransferIn) AS Expr1,itemid,date FROM     InventoryTransfer where itemid='"+cmbitem.SelectedValue+"' and TransferIn>0 and  Date between '" + start + "' and  '" + end + "' group by  itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(TransferIn) AS Expr1,itemid,date FROM     InventoryTransfer where itemid='" + cmbitem.SelectedValue + "' and  TransferIn>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' group by  itemid,date";
                    }
                } 
            }
            try
            {
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        InventoryTransferInopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dsin.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("itemid"),
                                       quantity = row.Field<double>("Expr1"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        InventoryTransferInopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            q = "";
            try
            {
                if (cmbkitchen.Text == "All" || cmbstore.Text != "All")
                {
                }
                else
                {
                    dsin = new DataSet();
                    if (cmbitem.Text == "All")
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date FROM     InventoryTransferStore where Quantity>0 and  RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date FROM     InventoryTransferStore where Quantity>0 and  RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                    }
                    else
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date FROM     InventoryTransferStore where itemid='"+cmbitem.SelectedValue+"' and Quantity>0 and  RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date FROM     InventoryTransferStore where  itemid='" + cmbitem.SelectedValue + "' and Quantity>0 and  RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                    }
                    dsin = objCore.funGetDataSet(q);
                    if (dsin.Tables[0].Rows.Count > 0)
                    {
                        try
                        {

                            InventoryTransferStoreInopeningdict = new List<OpeningCriticalInventoryClass>();
                            IList<OpeningCriticalInventoryClass> data = dsin.Tables[0].AsEnumerable().Select(row =>
                                       new OpeningCriticalInventoryClass
                                       {
                                           ItemId = row.Field<int>("itemid"),
                                           quantity = row.Field<double>("Expr1"),
                                           Date = row.Field<DateTime>("Date")


                                       }).ToList();
                            InventoryTransferStoreInopeningdict = data.ToList();
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }
            q = "";
            dsin = new DataSet();
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbitem.Text == "All")
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(TransferOut) AS Expr1,itemid,date  FROM     InventoryTransfer where TransferOut>0 and  Date between '" + start + "' and  '" + end + "' group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(TransferOut) AS Expr1,itemid,date  FROM     InventoryTransfer where TransferOut>0 and branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' group by itemid,date";
                    }
                }
                else
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(TransferOut) AS Expr1,itemid,date  FROM     InventoryTransfer where itemid='"+cmbitem.SelectedValue+"' and TransferOut>0 and  Date between '" + start + "' and  '" + end + "' group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(TransferOut) AS Expr1,itemid,date  FROM     InventoryTransfer where  itemid='" + cmbitem.SelectedValue + "' and TransferOut>0 and branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' group by itemid,date";
                    }
                }

            }
            try
            {
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        InventoryTransferOutnopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dsin.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("itemid"),
                                       quantity = row.Field<double>("Expr1"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        InventoryTransferOutnopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            q = "";
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {

                }
                else
                {
                    if (cmbitem.Text == "All")
                    {
                        q = "SELECT     SUM(TransferIn) AS Expr1,itemid,date  FROM     InventoryTransfer where TransferIn>0 and sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";

                    }
                    else
                    {
                        q = "SELECT     SUM(TransferIn) AS Expr1,itemid,date  FROM     InventoryTransfer where itemid='"+cmbitem.SelectedValue+"' and TransferIn>0 and sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";
               
                    }
                }
            }
            try
            {
                dsin = objCore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        InventoryTransferOutnopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dsin.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("itemid"),
                                       quantity = row.Field<double>("Expr1"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        InventoryTransferOutnopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                q = "";
                dsin = new DataSet();
                if (cmbitem.Text == "All")
                {
                    if (cmbstore.Text == "All")
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where Quantity>0 and Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                        else
                        {
                            // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  RecvStoreId='" + cmbkitchen.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                        }
                    }
                    else
                    {

                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where  Quantity>0 and IssuingStoreId='" + cmbstore.SelectedValue + "' and Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where  Quantity>0 and  IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";

                            }
                            else
                            {

                                q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where  Quantity>0 and RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";
                            }
                        }

                    }
                }
                else
                {

                    if (cmbstore.Text == "All")
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where itemid='"+cmbitem.SelectedValue+"' and Quantity>0 and Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                        else
                        {
                            // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  RecvStoreId='" + cmbkitchen.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                        }
                    }
                    else
                    {

                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where itemid='" + cmbitem.SelectedValue + "' and   Quantity>0 and IssuingStoreId='" + cmbstore.SelectedValue + "' and Date between '" + start + "' and '" + end + "' group by itemid,date";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where itemid='" + cmbitem.SelectedValue + "' and   Quantity>0 and  IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";

                            }
                            else
                            {

                                q = "SELECT     SUM(Quantity ) AS Expr1,itemid,date  FROM     InventoryTransferStore where  itemid='" + cmbitem.SelectedValue + "' and  Quantity>0 and RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' group by itemid,date";
                            }
                        }

                    }
                }
                dsin = objCore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        InventoryTransferStoreOutopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dsin.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("itemid"),
                                       quantity = row.Field<double>("Expr1"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        InventoryTransferStoreOutopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            q = "";
            val = "";
            double rate = 0;
            DataSet dscon = new DataSet();
            //q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + itemid + "'";
            //dscon = objcore.funGetDataSet(q);
            //if (dscon.Tables[0].Rows.Count > 0)
            //{
            //    rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
            //}
            //if (rate > 0)
            //{
            //    consumed = consumed / rate;
            //}
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
            if (cmbitem.Text == "All")
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT    SUM(completewaste) AS completewaste,itemid,date FROM     discard where completewaste>0 and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where completewaste>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where completewaste>0 and  Date between '" + start + "' and  '" + end + "'  and isnull(kdsid,'') != '' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where completewaste>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and isnull(kdsid,'') != '' group by itemid,date";
                        }
                    }
                    else
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date FROM     discard where completewaste>0 and  Date between '" + start + "' and  '" + end + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where completewaste>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                    }
                }
            }
            else
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT    SUM(completewaste) AS completewaste,itemid,date FROM     discard where itemid='"+cmbitem.SelectedValue+"' and completewaste>0 and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  completewaste>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  completewaste>0 and  Date between '" + start + "' and  '" + end + "'  and isnull(kdsid,'') != '' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  completewaste>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and isnull(kdsid,'') != '' group by itemid,date";
                        }
                    }
                    else
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  completewaste>0 and  Date between '" + start + "' and  '" + end + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(completewaste) AS completewaste,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  completewaste>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                    }
                } 
            }
            try
            {
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        completeopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("itemid"),
                                       quantity = row.Field<double>("completewaste"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        completeopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            dspurchase = new DataSet();
            if (cmbitem.Text == "All")
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where discard>0 and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where  discard>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where discard>0 and  Date between '" + start + "' and  '" + end + "'  and isnull(kdsid,'') != '' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where  discard>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and isnull(kdsid,'') != '' group by itemid,date";
                        }
                    }
                    else
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where discard>0 and  Date between '" + start + "' and  '" + end + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where discard>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                    }
                }
            }
            else
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where itemid='"+cmbitem.SelectedValue+"' and discard>0 and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where  itemid='" + cmbitem.SelectedValue + "' and  discard>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where  itemid='" + cmbitem.SelectedValue + "' and discard>0 and  Date between '" + start + "' and  '" + end + "'  and isnull(kdsid,'') != '' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where  itemid='" + cmbitem.SelectedValue + "' and  discard>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and isnull(kdsid,'') != '' group by itemid,date";
                        }
                    }
                    else
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where  itemid='" + cmbitem.SelectedValue + "' and discard>0 and  Date between '" + start + "' and  '" + end + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(discard) AS Expr1,itemid,date FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  discard>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                    }
                }
            }
            try
            {
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    try
                    {

                        discardopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("itemid"),
                                       quantity = row.Field<double>("Expr1"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        discardopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }
            dspurchase = new DataSet();
            if (cmbitem.Text == "All")
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT    SUM(staff) AS staff,itemid,date FROM     discard where staff>0 and  Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT    SUM(staff) AS staff,itemid,date FROM     discard where  staff>0 and branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where staff>0 and Date between '" + start + "' and  '" + end + "'  and isnull(kdsid,'') != '' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where staff>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and isnull(kdsid,'') != '' group by itemid,date";
                        }
                    }
                    else
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where staff>0 and Date between '" + start + "' and  '" + end + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where  staff>0 and branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                    }
                }
            }
            else
            {
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT    SUM(staff) AS staff,itemid,date FROM     discard where itemid='"+cmbitem.SelectedValue+"' and staff>0 and  Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                    else
                    {
                        q = "SELECT    SUM(staff) AS staff,itemid,date FROM     discard where  itemid='" + cmbitem.SelectedValue + "' and  staff>0 and branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid is null group by itemid,date";
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  staff>0 and Date between '" + start + "' and  '" + end + "'  and isnull(kdsid,'') != '' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  staff>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and isnull(kdsid,'') != '' group by itemid,date";
                        }
                    }
                    else
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and  staff>0 and Date between '" + start + "' and  '" + end + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                        else
                        {
                            q = "SELECT     SUM(staff) AS staff,itemid,date  FROM     discard where itemid='" + cmbitem.SelectedValue + "' and   staff>0 and branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid,date";
                        }
                    }
                } 
            }
            try
            {
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {

                    try
                    {

                        staffopeningdict = new List<OpeningCriticalInventoryClass>();
                        IList<OpeningCriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                   new OpeningCriticalInventoryClass
                                   {
                                       ItemId = row.Field<int>("itemid"),
                                       quantity = row.Field<double>("staff"),
                                       Date = row.Field<DateTime>("Date")


                                   }).ToList();
                        staffopeningdict = data.ToList();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        #endregion
        
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        double closingamount = 0, wastage = 0;
        

      
        Dictionary<int, double> purchasedict = new Dictionary<int, double>();
        Dictionary<int, double> purchasereturndict = new Dictionary<int, double>();
        Dictionary<int, double> Productiondict = new Dictionary<int, double>();
        Dictionary<int, double> InventoryConsumeddict = new Dictionary<int, double>();
        Dictionary<int, double> discarddict = new Dictionary<int, double>();
        Dictionary<int, double> staffdict = new Dictionary<int, double>();
        Dictionary<int, double> completedict = new Dictionary<int, double>();
        Dictionary<int, double> nventoryTransferIndict = new Dictionary<int, double>();
        Dictionary<int, double> InventoryTransferStoreIndict = new Dictionary<int, double>();
        Dictionary<int, double> InventoryTransferStoreOutdict = new Dictionary<int, double>();
        Dictionary<int, double> InventoryTransferOutndict = new Dictionary<int, double>();
        Dictionary<int, double> InventoryTransferIndict = new Dictionary<int, double>();
        Dictionary<int, double> Conversiondict = new Dictionary<int, double>();

        List<OpeningCriticalInventoryClass> purchaseopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> purchasereturnopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> Productionopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> InventoryConsumedopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> discardopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> staffopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> completeopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> nventoryTransferInopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> InventoryTransferStoreInopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> InventoryTransferStoreOutopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> InventoryTransferOutnopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> InventoryTransferInopeningdict = new List<OpeningCriticalInventoryClass>();
        List<OpeningCriticalInventoryClass> Conversionopeningdict = new List<OpeningCriticalInventoryClass>();



        List<RawItemClass> rawItemlist = new List<RawItemClass>();
        RawItemStruct[] rawItems=new RawItemStruct[1]; 
        List<CriticalInventoryClass> purchaseslist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClass> purchasereturnslist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClass> Productionlist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClass> InventoryConsumedlist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClassDiscard> discardlist = new List<CriticalInventoryClassDiscard>();
        List<CriticalInventoryClass> InventoryTransferInlist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClass> InventoryTransferStoreInlist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClass> InventoryTransferStoreOutlist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClass> InventoryTransferOutlist = new List<CriticalInventoryClass>();
        List<CriticalInventoryClass> Conversionlist = new List<CriticalInventoryClass>();
        public void filtable(int start, int count, string query)
        {
            string query1 = query;
            string date = dateTimePicker1.Text;
            double purchased = 0, purchasereturn = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, minorder = 0, balance = 0, closing = 0;
            double qty = 0;
            //List<RawItemClass> rawItemlistFiltered = new List<RawItemClass>();
            //rawItemlistFiltered = rawItemlist.GetRange(start, count);
           for (int i=0;i< rawItemlist.Count; i++)         
            {

                int itemid = rawItemlist[i].Id; string name = rawItemlist[i].ItemName.ToString() + "(" + rawItemlist[i].UOM + ")";
                string category = rawItemlist[i].Cat.ToString();
                if (itemid.ToString() == "88")
                {

                }
                query = query1.Replace("{itemid}", itemid.ToString());

                purchased = 0; purchasereturn = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; closing = 0;
                double cmplt = 0;

                double openin = 0;

                try
                {
                    openin = opening(itemid.ToString());
                }
                catch (Exception ex)
                {
                    
                }
                //qty = openin;

                double rem = 0;
                minorder = 0; balance = 0;
                string temp = "0";
                try
                {
                    temp = rawItemlist[i].MinOrder.ToString();
                }
                catch (Exception ex)
                {
                    
                    
                }
                if (temp == "")
                {
                    temp = "0";
                }
                minorder = Convert.ToDouble(temp);

                try
                {
                    
                    double result;
                    if (purchasedict.TryGetValue(itemid,out result))
                    {
                        purchased = result;
                    }
                    else
                    {                       
                    }


                    //purchased = purchaseslist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity;
                }
                catch (Exception ex)
                {

                }

                try
                {
                    double result;
                    if (Productiondict.TryGetValue(itemid, out result))
                    {
                        purchased = purchased + result;
                    }
                    else
                    {
                    }
                    //purchased = purchased + Productionlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity;
                }
                catch (Exception ex)
                {

                }

               string val = "";
                purchased = Math.Round(purchased, 2);
                //qty = qty + purchased;


                try
                {
                    double result;
                    if (purchasereturndict.TryGetValue(itemid, out result))
                    {
                        purchasereturn =  result;
                    }
                    else
                    {
                    }
                    //purchased = purchased + Productionlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity;
                }
                catch (Exception ex)
                {

                }


                val = ""; string q = "";
                double rate = 0;
                DataSet dscon = new DataSet();
                try
                {
                    double result;
                    if (InventoryConsumeddict.TryGetValue(itemid, out result))
                    {
                        consumed = result;
                    }
                    else
                    {
                    }
                   // consumed = InventoryConsumedlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity;
                   
                }
                catch (Exception ex)
                {


                }

                try
                {
                    double result;
                    if (Conversiondict.TryGetValue(itemid, out result))
                    {
                        rate = result;
                    }
                    //rate = Conversionlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity;
                   
                }
                catch (Exception ex)
                {


                }
                if (rate > 0)
                {
                    consumed = consumed / rate;
                }
                // qty = qty - consumed;


                try
                {


                    {
                        try
                        {
                            val = "";
                            double result;
                            if (discarddict.TryGetValue(itemid, out result))
                            {
                                discard = result;
                            }
                            //val = discardlist.Find(e => e.ItemId.ToString() == itemid.ToString()).discard;
                            
                            //if (val == "")
                            //{
                            //    val = "0";
                            //}
                            //discard = Convert.ToDouble(val);
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                           // val = "";
                           // val = discardlist.Find(e => e.ItemId.ToString() == itemid.ToString()).staff;
                           //// val = discardlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().staff.ToString();
                           // if (val == "")
                           // {
                           //     val = "0";
                           // }
                           // staff = Convert.ToDouble(val);


                            double result;
                            if (staffdict.TryGetValue(itemid, out result))
                            {
                                staff = result;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            //val = "";
                            //val = discardlist.Find(e => e.ItemId.ToString() == itemid.ToString()).completewaste;
                            ////val = discardlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().completewaste.ToString();
                            //if (val == "")
                            //{
                            //    val = "0";
                            //}
                            //cmplt = Convert.ToDouble(val);

                            double result;
                            if (completedict.TryGetValue(itemid, out result))
                            {
                                cmplt = result;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
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




                try
                {
                    val = "";
                    double result;
                    if (InventoryTransferIndict.TryGetValue(itemid, out result))
                    {
                        val = result.ToString();
                    }
                    //val = InventoryTransferInlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity.ToString();
                    //val = InventoryTransferInlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tint = Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    val = "";
                    double result;
                    if (InventoryTransferStoreIndict.TryGetValue(itemid, out result))
                    {
                        val = result.ToString();
                    }
                    //val = InventoryTransferInlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity.ToString();
                    //val = InventoryTransferInlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tint =tint+ Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    //val = InventoryTransferInlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity.ToString();
                    ////val = InventoryTransferStoreInlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    //if (val == "")
                    //{
                    //    val = "0";
                    //}
                    //tint = tint + Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }



                try
                {
                    val = "";
                    double result;
                    if (InventoryTransferOutndict.TryGetValue(itemid, out result))
                    {
                        val = result.ToString();
                    }
                   // val = InventoryTransferOutlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity.ToString();
                   // val = InventoryTransferOutlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    val = "";
                    double result;
                    if (InventoryTransferStoreOutdict.TryGetValue(itemid, out result))
                    {
                        val = result.ToString();
                    }
                    // val = InventoryTransferOutlist.Find(e => e.ItemId.ToString() == itemid.ToString()).quantity.ToString();
                    // val = InventoryTransferOutlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout =tout+ Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    //val = InventoryTransferStoreOutlist.Where(x => x.ItemId.ToString() == itemid.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    //if (val == "")
                    //{
                    //    val = "0";
                    //}
                    //tout = tout + Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                tout = tout + purchasereturn;
                double ideal = 0;
                // discard = discard * -1;
                //qty = qty - (discard * -1);
                //qty = qty - (staff);
                //qty = qty - (cmplt);
                //qty = qty + tint;
                //qty = qty - tout;
                //qty = Math.Round(qty, 2);
                discard = 0;
                string date2 = "";
                string tempchk = "yes"; q = "";
                q = query;
                DataSet dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                try
                {
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
                                closing = closing + openingclosing(itemid.ToString(), date2, closing);
                            }
                        }
                    }
                    else
                    {
                        tempchk = "no";
                    }
                }
                catch (Exception ex)
                {
                    
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



                double closingval = 0, purchaseval = 0, saleval = 0, discardval = 0, comptval = 0, staffval = 0;
                price = getprice(itemid.ToString());
                balance = price * discard;
                wastage = wastage + ((staff + cmplt) * price);
                closingamount = closingamount + (price * closing);
                closingval = price * closing;
                double openingval = openin * price;
                purchaseval = purchased * price;
                saleval = consumed * price;
                discardval = price * discard;
                comptval = cmplt * price;
                staffval = staff * price;
                //=========================
                if (type == "value")
                {
                    openin = openin * price;
                    purchased = purchased * price;
                    consumed = consumed * price;
                    discard = discard * price;
                    staff = staff * price;
                    cmplt = cmplt * price;
                    closing = closing * price;
                    tint = tint * price;
                    tout = tout * price;
                    ideal = ideal * price;
                }
                getcompany();
                string logo = "";
                try
                {
                   // logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                if (logo == "")
                {
                    dtrpt.Rows.Add(name, openin.ToString(), purchased.ToString(), consumed.ToString(), discard.ToString(), staff, cmplt, tint, tout, closing, minorder, balance, null, price, openingval, closingval, purchaseval, saleval, discardval, staffval, comptval, ideal, category);
                }

                else
                {
                    dtrpt.Rows.Add(name, openin.ToString(), purchased.ToString(), consumed.ToString(), discard.ToString(), staff, cmplt, tint, tout, closing, minorder, balance, dscompany.Tables[0].Rows[0]["logo"], price, openingval, closingval, purchaseval, saleval, discardval, staffval, comptval, ideal, category);

                }
            }
           // fillreport();
        }
        public void filtable2(int start, int count, string query)
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, purchasreturn = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, minorder = 0, balance = 0, closing = 0;
            double qty = 0;
            List<RawItemClass> rawItemlistFiltered = new List<RawItemClass>();
            rawItemlistFiltered = rawItemlist.GetRange(start, count);
            foreach (var item in rawItemlistFiltered)
            {
                query = query.Replace("{itemid}", item.Id.ToString());

                purchased = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; closing = 0;
                double cmplt = 0;

                double openin = 0;// opening(item.Id.ToString());
                //qty = openin;

                double rem = 0;
                minorder = 0; balance = 0;
                string temp = item.MinOrder.ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                minorder = Convert.ToDouble(temp);

                try
                {
                    purchased = purchaseslist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity;
                }
                catch (Exception ex)
                {

                }

                try
                {
                    purchased = purchased + Productionlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity;
                }
                catch (Exception ex)
                {

                }

                string val = "";
                purchased = Math.Round(purchased, 2);
                //qty = qty + purchased;

                try
                {
                    purchasreturn = purchasereturnslist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity;
                }
                catch (Exception ex)
                {

                }



                val = ""; string q = "";
                double rate = 0;
                DataSet dscon = new DataSet();
                try
                {
                    consumed = InventoryConsumedlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity;
                }
                catch (Exception ex)
                {


                }

                try
                {
                    rate = Conversionlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity;
                }
                catch (Exception ex)
                {


                }
                if (rate > 0)
                {
                    consumed = consumed / rate;
                }
                // qty = qty - consumed;


                try
                {


                    {
                        try
                        {
                            val = discardlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().discard.ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            discard = Convert.ToDouble(val);
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                            val = discardlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().staff.ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            staff = Convert.ToDouble(val);
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            val = discardlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().completewaste.ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            cmplt = Convert.ToDouble(val);
                        }
                        catch (Exception ex)
                        {

                        }
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




                try
                {
                    val = InventoryTransferInlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tint = Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    val = InventoryTransferStoreInlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tint = tint + Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }



                try
                {
                    val = InventoryTransferOutlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    val = InventoryTransferStoreOutlist.Where(x => x.ItemId.ToString() == item.Id.ToString()).ToList().FirstOrDefault().quantity.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = tout + Convert.ToDouble(val);
                }
                catch (Exception ex)
                {

                }
                tout = tout + purchasreturn;
                double ideal = 0;
                // discard = discard * -1;
                //qty = qty - (discard * -1);
                //qty = qty - (staff);
                //qty = qty - (cmplt);
                //qty = qty + tint;
                //qty = qty - tout;
                //qty = Math.Round(qty, 2);
                discard = 0;
                string date2 = "";
                string tempchk = "yes"; q = "";
                q = query;
                DataSet dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                try
                {
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
                            //if (date2 == "")
                            //{
                            //    date2 = date;
                            //}
                            //if (Convert.ToDateTime(date2) < Convert.ToDateTime(dateTimePicker2.Text))
                            //{
                            //    closing = closing + openingclosing(ds1.Tables[0].Rows[i]["id"].ToString(), date2, closing);
                            //}
                        }
                    }
                    else
                    {
                        tempchk = "no";
                    }
                }
                catch (Exception ex)
                {
                   
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



                double closingval = 0, purchaseval = 0, saleval = 0, discardval = 0, comptval = 0, staffval = 0;
                price = getprice(item.Id.ToString());
                balance = price * discard;
                wastage = wastage + ((staff + cmplt) * price);
                closingamount = closingamount + (price * closing);
                closingval = price * closing;
                double openingval = openin * price;
                purchaseval = purchased * price;
                saleval = consumed * price;
                discardval = price * discard;
                comptval = cmplt * price;
                staffval = staff * price;
                //=========================
                if (type == "value")
                {
                    openin = openin * price;
                    purchased = purchased * price;
                    consumed = consumed * price;
                    discard = discard * price;
                    staff = staff * price;
                    cmplt = cmplt * price;
                    closing = closing * price;
                    tint = tint * price;
                    tout = tout * price;
                    ideal = ideal * price;
                }
                getcompany();
                string logo = "";
                try
                {
                   // logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                if (logo == "")
                {
                    dtrpt.Rows.Add(item.ItemName.ToString() + "(" + item.UOM + ")", openin.ToString(), purchased.ToString(), consumed.ToString(), discard.ToString(), staff, cmplt, tint, tout, closing, minorder, balance, null, price, openingval, closingval, purchaseval, saleval, discardval, staffval, comptval, ideal);
                }

                else
                {
                    dtrpt.Rows.Add(item.ItemName + "(" + item.UOM + ")", openin.ToString(), purchased.ToString(), consumed.ToString(), discard.ToString(), staff, cmplt, tint, tout, closing, minorder, balance, dscompany.Tables[0].Rows[0]["logo"], price, openingval, closingval, purchaseval, saleval, discardval, staffval, comptval, ideal);

                }
            }
           
        }
        DataTable dtrpt = new DataTable();
        public DataTable getAllOrders()
        {
           
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, minorder = 0, balance = 0, closing = 0;
            double qty = 0;
            DataTable ds = new DataTable();
            dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("Opening", typeof(double));
                dtrpt.Columns.Add("Purchase", typeof(double));
                dtrpt.Columns.Add("sale", typeof(double));
                dtrpt.Columns.Add("Discard", typeof(double));
                dtrpt.Columns.Add("Staff", typeof(double));
                dtrpt.Columns.Add("Complete", typeof(double));
                dtrpt.Columns.Add("TransferIn", typeof(double));
                dtrpt.Columns.Add("TransferOut", typeof(double));
                dtrpt.Columns.Add("Closing", typeof(double));
                dtrpt.Columns.Add("MinOrder", typeof(double));
                dtrpt.Columns.Add("Balance", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("price", typeof(double));
                dtrpt.Columns.Add("Openingval", typeof(double));
                dtrpt.Columns.Add("Closingval", typeof(double));
                dtrpt.Columns.Add("Purchaseval", typeof(double));
                dtrpt.Columns.Add("saleval", typeof(double));
                dtrpt.Columns.Add("Discardval", typeof(double));
                dtrpt.Columns.Add("Staffval", typeof(double));
                dtrpt.Columns.Add("Completeval", typeof(double));
                dtrpt.Columns.Add("Ideal", typeof(double));
                dtrpt.Columns.Add("Cat", typeof(string));
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
                if (cmbgroupraw.Text == "All")
                {
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbitem.Text == "All")
                        {
                            if (cmbgroup.Text == "All")
                            {
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, cast( dbo.RawItem.Price as varchar(200)) as Price, dbo.UOM.UOM, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  where dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName,cast( dbo.RawItem.Price as varchar(200)) as Price, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbgroup.SelectedValue + "'  order by dbo.RawItem.ItemName";
                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, cast( dbo.RawItem.Price as varchar(200)) as Price, dbo.UOM.UOM, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  where dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";
                        }
                    }
                    else
                    {
                        if (cmbitem.Text == "All")
                        {
                            
                            if (cmbgroup.Text == "All")
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName,cast( dbo.RawItem.Price as varchar(200)) as Price, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  WHERE    (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and     (dbo.MenuItem.Status = 'active') and   dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "'  order by dbo.RawItem.ItemName";                             
                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName,cast( dbo.RawItem.Price as varchar(200)) as Price, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  WHERE    (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and     (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "' and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "'    order by dbo.RawItem.ItemName";
                               
                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, cast( dbo.RawItem.Price as varchar(200)) as Price, dbo.UOM.UOM, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  where  dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";
                           
                        }
                    }
                }
                else
                {
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbitem.Text == "All")
                        {
                            if (cmbgroup.Text == "All")
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.Category.CategoryName, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  WHERE        (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName,cast( dbo.RawItem.Price as varchar(200)) as Price, dbo.UOM.UOM, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id   where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "'  order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName,cast( dbo.RawItem.Price as varchar(200)) as Price, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and      (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbgroup.SelectedValue + "' and dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "'  order by dbo.RawItem.ItemName";
                                //q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "'  order by dbo.RawItem.ItemName";
                               
                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, cast( dbo.RawItem.Price as varchar(200)) as Price, dbo.UOM.UOM, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  where dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";
                        }
                    }
                    else
                    {
                        if (cmbitem.Text == "All")
                        {
                            if (cmbgroup.Text == "All")
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName,cast( dbo.RawItem.Price as varchar(200)) as Price, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and     (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                               // q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "'  order by dbo.RawItem.ItemName";

                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName,cast( dbo.RawItem.Price as varchar(200)) as Price, cast( dbo.RawItem.MinOrder as varchar(200)) as MinOrder, dbo.Category.CategoryName, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and     (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbgroup.SelectedValue + "' and dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                               // q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where  dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, CAST(dbo.RawItem.Price AS varchar(200)) AS Price, dbo.UOM.UOM, CAST(dbo.RawItem.MinOrder AS varchar(200)) AS MinOrder, dbo.Category.CategoryName FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id where  dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";

                        }
                    }
                }
                fillopening();
                DataSet ds1 = new DataSet();
                ds1 = objcore.funGetDataSet(q);
                
                try
                {
                   
                    
                    {
                        try
                        {

                            IList<RawItemClass> data = ds1.Tables[0].AsEnumerable().Select(row =>
                                new RawItemClass
                                {
                                    Id = row.Field<int>("Id"),
                                    ItemName = row.Field<string>("ItemName"),
                                    Price = row.Field<string>("Price"),
                                    UOM = row.Field<string>("UOM"),
                                    MinOrder = row.Field<string>("MinOrder"),
                                    Cat = row.Field<string>("CategoryName")

                                }).ToList();
                            rawItemlist = data.ToList();
                            //rawItems = new RawItemStruct[rawItemlist.Count];
                            //for (int i = 0; i < rawItemlist.Count; i++)
                            //{
                            //    rawItems[i] = new RawItemStruct(rawItemlist[i].Id, rawItemlist[i].ItemName, rawItemlist[i].Price, rawItemlist[i].UOM, rawItemlist[i].MinOrder);
                            //}

                        }
                        catch (Exception ex)
                        {


                        }

                    }
                }
                catch (Exception ex)
                {

                }


               // var CoreCount = System.Environment.ProcessorCount / 2;
             
               

                DataSet dspurchase = new DataSet();
                string val = "";
                try
                {
                   
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbstore.Text == "All")
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') group by dbo.PurchaseDetails.RawItemId";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' group by dbo.PurchaseDetails.RawItemId";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "' group by dbo.PurchaseDetails.RawItemId";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1,dbo.PurchaseDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "' group by dbo.PurchaseDetails.RawItemId";
                            }
                        }
                    }
                    else
                    {
                        q = "";
                    }
                   

                    try
                    {
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                           

                            try
                            {
                                purchasedict = new  Dictionary<int,double>();
                                IDictionary<int,double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("RawItemId"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                purchasedict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {
                    
                }

                dspurchase = new DataSet();
                val = "";
                try
                {

                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbstore.Text == "All")
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') group by dbo.PurchasereturnDetails.RawItemId";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' group by dbo.PurchasereturnDetails.RawItemId";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "' group by dbo.PurchasereturnDetails.RawItemId";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1,dbo.PurchasereturnDetails.RawItemId FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "' group by dbo.PurchasereturnDetails.RawItemId";
                            }
                        }
                    }
                    else
                    {
                        q = "";
                    }


                    try
                    {
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {


                            try
                            {
                               
                                purchasereturndict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("RawItemId"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                purchasereturndict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }


                try
                {

                    q = "SELECT     dbo.RawItem.Id, dbo.UOM.UOM, cast( dbo.UOMConversion.ConversionRate as float) as ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ";
                  


                    try
                    {
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    Conversionlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("Id"),
                            //            quantity = row.Field<double>("ConversionRate")

                            //        }).ToList();
                            //    Conversionlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                Conversiondict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("Id"),
                                        quantity = row.Field<double>("ConversionRate")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                Conversiondict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                try
                {
                    q = "";
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1,ItemId FROM         Production  where (dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Production.status='Posted' group by ItemId";
                        }
                        else
                        {

                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1,ItemId FROM         Production  where (dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted' group by ItemId";
                        }
                    }
                    else
                    {
                        q = "";
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    Productionlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("ItemId"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    Productionlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                Productiondict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("ItemId"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                Productiondict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                try
                {
                    q = "";
                    if (cmbbranch1.Text == "All")
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId FROM     InventoryConsumed where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by RawItemId  ";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId FROM     InventoryConsumed where  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by RawItemId";

                            }
                            else
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by RawItemId";
                            }
                        }
                    }
                    else
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by RawItemId  ";
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by RawItemId";

                            }
                            else
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1,RawItemId FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  group by RawItemId";
                            }
                        }
                    }
                    if (cmbstore.Text != "All")
                    {
                        q = "";
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    InventoryConsumedlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("RawItemId"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    InventoryConsumedlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                InventoryConsumeddict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("RawItemId"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                InventoryConsumeddict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                //try
                //{
                //    q = "";
                //    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                //    {
                //        if (cmbbranch1.Text == "All")
                //        {
                //            q = "SELECT      cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and  kdsid is null group by itemid";
                //        }
                //        else
                //        {
                //            q = "SELECT      cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and kdsid is null group by itemid";
                //        }

                //    }
                //    else
                //    {
                //        if (checkBox1.Checked == true)
                //        {
                //            if (cmbbranch1.Text == "All")
                //            {
                //                q = "SELECT      cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                //            }
                //            else
                //            {
                //                q = "SELECT      cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                //            }
                //        }
                //        else
                //        {
                //            if (cmbbranch1.Text == "All")
                //            {
                //                q = "SELECT    cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                //            }
                //            else
                //            {
                //                q = "SELECT     cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                //            }
                //        }
                //    }


                //    try
                //    {
                //        dspurchase = new DataSet();
                //        dspurchase = objcore.funGetDataSet(q);
                //        if (dspurchase.Tables[0].Rows.Count > 0)
                //        {
                //            try
                //            {
                //                discardlist = new List<CriticalInventoryClassDiscard>();
                //                IList<CriticalInventoryClassDiscard> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                //                    new CriticalInventoryClassDiscard
                //                    {
                //                        ItemId = row.Field<int>("itemid"),
                //                        discard = row.Field<string>("Expr1"),
                //                        staff = row.Field<string>("staff"),
                //                        completewaste = row.Field<string>("completewaste")

                //                    }).ToList();
                //                discardlist = data.ToList();

                //            }
                //            catch (Exception ex)
                //            {


                //            }
                            
                //        }
                //    }
                //    catch (Exception ex)
                //    {

                //    }

                //}
                //catch (Exception ex)
                //{

                //}


                try
                {
                    q = "";
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT      cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and  kdsid is null group by itemid";
                        }
                        else
                        {
                            q = "SELECT      cast(SUM(discard) as varchar(200)) AS Expr1,cast( SUM(staff) as varchar(200)) AS staff,cast( SUM(completewaste) as varchar(200)) AS completewaste,itemid FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and kdsid is null group by itemid";
                        }

                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT      SUM(discard)  AS discard,itemid FROM     discard where discard>0 and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                            }
                            else
                            {
                                q = "SELECT      SUM(discard)  AS discard,itemid FROM     discard where discard>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT    SUM(discard)  AS discard,itemid FROM     discard where discard>0 and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                            }
                            else
                            {
                                q = "SELECT     SUM(discard)  AS discard,itemid FROM     discard where discard>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                            }
                        }
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                discarddict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("discard")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                discarddict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }

                try
                {
                    q = "";
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(staff)  AS staff,itemid FROM     discard where staff>0 and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and  kdsid is null group by itemid";
                        }
                        else
                        {
                            q = "SELECT      SUM(staff)  AS staff,itemid  FROM     discard where  staff>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and kdsid is null group by itemid";
                        }

                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT      SUM(staff)  AS staff,itemid  FROM     discard where staff>0 and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                            }
                            else
                            {
                                q = "SELECT      SUM(staff)  AS staff,itemid  FROM     discard where staff>0 and    branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT    SUM(staff)  AS staff,itemid  FROM     discard where staff>0 and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                            }
                            else
                            {
                                q = "SELECT     SUM(staff)  AS staff,itemid  FROM     discard where staff>0 and    branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                            }
                        }
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                staffdict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("staff")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                staffdict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }

                try
                {
                    q = "";
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT       SUM(completewaste)  AS completewaste,itemid FROM     discard where completewaste>0 and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and  kdsid is null group by itemid";
                        }
                        else
                        {
                            q = "SELECT       SUM(completewaste)  AS completewaste,itemid FROM     discard where completewaste>0 and branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and kdsid is null group by itemid";
                        }

                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT       SUM(completewaste) AS completewaste,itemid FROM     discard where  completewaste>0 and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                            }
                            else
                            {
                                q = "SELECT       SUM(completewaste) AS completewaste,itemid FROM     discard where  completewaste>0 and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and  isnull(kdsid,'') != '' group by itemid";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(completewaste) AS completewaste,itemid FROM     discard where completewaste>0 and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                            }
                            else
                            {
                                q = "SELECT      SUM(completewaste) AS completewaste,itemid FROM     discard where completewaste>0 and   branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and kdsid='" + cmbkitchen.SelectedValue + "' group by itemid";
                            }
                        }
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                               completedict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("completewaste")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                completedict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                try
                {

                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(NULLIF(TransferIn,'0')) AS Expr1,itemid FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and TransferIn>0 group by itemid";
                        }
                        else
                        {
                            q = "SELECT     SUM(NULLIF(TransferIn,'0')) AS Expr1,itemid FROM     InventoryTransfer where branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and TransferIn>0 group by itemid";
                        }
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    InventoryTransferInlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("itemid"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    InventoryTransferInlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                InventoryTransferIndict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                InventoryTransferIndict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                try
                {
                    q = "";
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(NULLIF(TransferOut,'0')) AS Expr1,itemid FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and TransferOut>0 group by itemid";
                        }
                        else
                        {
                            q = "SELECT     SUM(NULLIF(TransferOut,'0')) AS Expr1,itemid FROM     InventoryTransfer where branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and TransferOut>0 group by itemid";
                        }
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    InventoryTransferOutlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("itemid"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    InventoryTransferOutlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                InventoryTransferOutndict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                InventoryTransferOutndict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }

                try
                {
                    q = "";
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {

                        }
                        else
                        {
                            q = "SELECT     SUM(NULLIF(TransferIn,'0')) AS Expr1,itemid FROM     InventoryTransfer where sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and TransferIn>0 group by itemid";
                        }
                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    InventoryTransferOutlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("itemid"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    InventoryTransferOutlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}

                            try
                            {
                                InventoryTransferOutndict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                InventoryTransferOutndict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }

                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }


                try
                {
                    q = "";
                    if (cmbstore.Text == "All")
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and quantity>0 group by itemid";
                        }
                        else
                        {
                            // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  RecvStoreId='" + cmbkitchen.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                    }
                    else
                    {

                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "'  and quantity>0  and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by itemid";
                        }
                        else
                        {
                            // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }

                    }


                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    InventoryTransferStoreInlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("itemid"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    InventoryTransferStoreInlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                //InventoryTransferStoreIndict = new Dictionary<int, double>();
                                //IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                //    new CriticalInventoryClass
                                //    {
                                //        ItemId = row.Field<int>("itemid"),
                                //        quantity = row.Field<double>("Expr1")

                                //    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                //InventoryTransferStoreIndict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                q = "";
                try
                {
                    if (cmbkitchen.Text == "All" || cmbstore.Text != "All")
                    {
                    }
                    else
                    {

                        if (cmbitem.Text == "All")
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore  where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and quantity>0  group by itemid";
                            }
                            else
                            {
                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "'  and quantity>0  and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by itemid ";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore  where itemid='"+cmbitem.SelectedValue+"' and RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and quantity>0  group by itemid";
                            }
                            else
                            {
                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where itemid='" + cmbitem.SelectedValue + "' and  RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "'  and quantity>0  and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by itemid ";
                            }
                        }

                    }




                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    InventoryTransferStoreInlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("itemid"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    InventoryTransferStoreInlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                InventoryTransferStoreIndict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                InventoryTransferStoreIndict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                q = "";

                try
                {

                    if (cmbstore.Text == "All")
                    {
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            if (cmbitem.Text == "All")
                            {

                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and quantity>0  group by itemid";
                            }
                            else
                            {
                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where  itemid='" + cmbitem.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and quantity>0  group by itemid";
                            }
                        }

                        else
                        {
                            // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  RecvStoreId='" + cmbkitchen.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                    }
                    else
                    {

                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "'  and quantity>0  and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by itemid";
                            }
                            else
                            {
                                q = "SELECT     SUM(NULLIF(Quantity,'0') ) AS Expr1,itemid FROM     InventoryTransferStore where itemid='" + cmbitem.SelectedValue + "' and   IssuingStoreId='" + cmbstore.SelectedValue + "'  and quantity>0  and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' group by itemid";
                            }
                        }
                        else
                        {
                            // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }

                    }



                    try
                    {
                        dspurchase = new DataSet();
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            //try
                            //{
                            //    InventoryTransferStoreOutlist = new List<CriticalInventoryClass>();
                            //    IList<CriticalInventoryClass> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                            //        new CriticalInventoryClass
                            //        {
                            //            ItemId = row.Field<int>("itemid"),
                            //            quantity = row.Field<double>("Expr1")

                            //        }).ToList();
                            //    InventoryTransferStoreOutlist = data.ToList();

                            //}
                            //catch (Exception ex)
                            //{


                            //}
                            try
                            {
                                InventoryTransferStoreOutdict = new Dictionary<int, double>();
                                IDictionary<int, double> data = dspurchase.Tables[0].AsEnumerable().Select(row =>
                                    new CriticalInventoryClass
                                    {
                                        ItemId = row.Field<int>("itemid"),
                                        quantity = row.Field<double>("Expr1")

                                    }).ToDictionary(keySelector: m => m.ItemId, elementSelector: m => m.quantity);
                                InventoryTransferStoreOutdict = data.ToDictionary(keySelector: m => m.Key, elementSelector: m => m.Value);

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
                catch (Exception ex)
                {

                }
                q = "";
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT   top 1   remaining,date,itemid FROM     closing where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='{itemid}'  and kdsid is null  order by date desc";

                    }
                    else
                    {
                        q = "SELECT   top 1   remaining,date,itemid FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='{itemid}'  and kdsid is null  order by date desc";

                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        q = "SELECT   top 1   remaining,date,itemid FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='{itemid}' and   isnull(kdsid,'') != '' order by date desc";

                    }
                    else
                    {
                        q = "SELECT   top 1   remaining,date,itemid FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='{itemid}' and kdsid='" + cmbkitchen.SelectedValue + "' order by date desc";
                    }
                }

               

                filtable(0, 0, q);
                //threadfill = new Thread(() => filtable(0, 100, q));
                //threadfill.IsBackground = true;
                //threadfill.Start();

                //threadfill2 = new Thread(() => filtable2(100, 79, q));
                //threadfill2.IsBackground = true;
                //threadfill2.Start();

                //threadfill3 = new Thread(() => filtable(200, 100));
                //threadfill3.IsBackground = true;
                //threadfill3.Start();

                //for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        System.Threading.Thread threadfill;
        System.Threading.Thread threadfill2;
        System.Threading.Thread threadfill3;
        DataSet dscompany = new DataSet();
        public double getprice(string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "'," + id + ")";
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
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Text = "Please Wait";
            vButton1.Enabled = false;
            bindreport();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            try
            {
                CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;

                string name = info.Text;
                int indx = name.IndexOf("(");
                name = name.Substring(0, indx);
                string q = "select id from rawitem where itemname='" + name + "'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string id = ds.Tables[0].Rows[0][0].ToString();
                    if (id.Length > 0)
                    {
                        POSRestaurant.Reports.Inventory.frmreverserecipe obj = new POSRestaurant.Reports.Inventory.frmreverserecipe();
                        obj.id = id;
                        obj.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void cmbbranch1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillstore();
        }

        private void cmbgroupraw_SelectedIndexChanged(object sender, EventArgs e)
        {
            getitems();
        }
    }
}
