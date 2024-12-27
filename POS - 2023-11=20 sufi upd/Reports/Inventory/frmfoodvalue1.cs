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
    public partial class frmfoodvalue1 : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmfoodvalue1()
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
                string q = "select id,itemname from rawitem order by itemname ";
                if (cmbgroupraw.Text != "All")
                {
                    q = "select id,itemname from rawitem where CategoryId='" + cmbgroupraw.SelectedValue + "'  order by itemname ";
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
                rptDoc.SetParameterValue("title", "Critical Inventory Report(Quantitative)");
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double openingclosing(string itemid,string date,double closing)
        {
            
            string date2 = dateTimePicker2.Text;           
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0;
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

            string start = start1.ToString("yyyy-MM-dd");
            string end = end1.ToString("yyyy-MM-dd");
            q = "";
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                } 
            }
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
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";
                    }
                    else
                    {

                        q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "'  and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted'";
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
                if (cmbbranch1.Text == "All")
                {
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'   ";
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'  ";
                    
                        }
                        else
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'  ";
                        }
                    }
                }
                else
                {
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'   ";
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
                        }
                        else
                        {

                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
                        }
                    }
                }
                if (cmbstore.Text != "All")
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
                    consumed = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }

            dspurchase = new DataSet();

            q = "";
            DataSet dsin = new DataSet();
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                } 
            }
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
            try
            {
                q = "";
                if (cmbkitchen.Text == "All" || cmbstore.Text != "All")
                {
                }
                else
                {
                    dsin = new DataSet();
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    }
                    else
                    {
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
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

            }
            catch (Exception ex)
            {

            } q = "";
            dsin = new DataSet();
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                } 
            }
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
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {

                }
                else
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                }
            }
            try
            {
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
            try
            {
                q = "";
                dsin = new DataSet();
                if (cmbstore.Text == "All")
                {
                    if (cmbkitchen.Text == "All")
                    {
                       // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
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
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where   IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                  
                        }
                        else
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and  IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                        }
                    }
                    
                }
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
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
                }
                else
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
                }
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and isnull(kdsid,'') != ''";
                    }
                    else
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'   and isnull(kdsid,'') != ''";
                    }
                }
                else
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
                    }
                }
            }
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

            closing = ( purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        public double opening(string itemid)
        {
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0, purchasereturn=0;
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
            string start = start1.ToString("yyyy-MM-dd");
            string end = end1.ToString("yyyy-MM-dd");
            q = "";
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                } 
            }
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
            dspurchase = new DataSet();
            
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                    q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "') and dbo.PurchasereturnDetails.RawItemId='" + itemid + "'";
                    
                }
                else
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and  dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                    q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where  dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and   (dbo.PurchasereturnDetails.date between '" + start + "' and  '" + end + "') and dbo.PurchasereturnDetails.RawItemId='" + itemid + "'";
                    
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
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchasereturn = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }
            q = "";
            try
            {
                dspurchase = new DataSet();
                if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";
                    }
                    else
                    {

                        q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "'  and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted'";
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
                if (cmbbranch1.Text == "All")
                {
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'   ";
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "' ";
             
                        }
                        else
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "' ";
                        }
                    }
                }
                else
                {
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "' ";
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where   branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
               
                        }
                        else
                        {
                            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
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
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    consumed = Convert.ToDouble(val);
                }
            }
            catch (Exception ee)
            {
                
            } 
            q = "";
            DataSet dsin = new DataSet();
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                } 
            }
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
            try
            {
                if (cmbkitchen.Text == "All" || cmbstore.Text!="All")
                {
                }
                else
                {
                    dsin = new DataSet();
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    }
                    else
                    {
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
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

            }
            catch (Exception ex)
            {

            }
            q = "";
            dsin = new DataSet();
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
                } 
            }
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
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {

                }
                else
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                }
            }
            try
            {
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
            try
            {
                q = "";
                dsin = new DataSet();
                if (cmbstore.Text == "All")
                {
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
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
                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "' and Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                 
                        }
                        else
                        {

                            q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                        }
                    }
                    
                }
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
            transferout = transferout + purchasereturn;
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
            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
            {
                if (cmbbranch1.Text == "All")
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
                }
                else
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid is null ";
                }
            }
            else
            {
                if (checkBox1.Checked == true)
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and isnull(kdsid,'') != '' ";
                    }
                    else
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and isnull(kdsid,'') != ''";
                    }
                }
                else
                {
                    if (cmbbranch1.Text == "All")
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
                    }
                }
            }
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

            closing = (closing + purchased + transferin) - (staff + complt + transferout+consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
      
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        double closingamount = 0, wastage = 0;
        public DataTable getAllOrders()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0,purchasereturn=0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, minorder = 0, balance = 0, closing = 0;
            double qty = 0;
            DataTable ds = new DataTable();
            DataTable dtrpt = new DataTable();
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
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbgroup.SelectedValue + "'  order by dbo.RawItem.ItemName";
                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";
                        }
                    }
                    else
                    {
                        if (cmbitem.Text == "All")
                        {
                            if (cmbgroup.Text == "All")
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbgroup.SelectedValue + "' and dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "'  order by dbo.RawItem.ItemName";
                        
                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";

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
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbgroup.SelectedValue + "' and dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "'  order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";
                        }
                    }
                    else
                    {
                        if (cmbitem.Text == "All")
                        {
                            if (cmbgroup.Text == "All")
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE     dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and     (dbo.MenuItem.Status = 'active') and  dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  order by dbo.RawItem.ItemName";

                            }
                            else
                            {
                                q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.MinOrder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE     dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and     (dbo.MenuItem.Status = 'active')  and  dbo.MenuItem.MenuGroupId='" + cmbgroup.SelectedValue + "' and dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";
                                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where  dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where  dbo.RawItem.Id='" + cmbitem.SelectedValue + "' order by dbo.RawItem.ItemName";

                        }
                    }
                }
                DataSet ds1 = new DataSet();
                ds1 = objcore.funGetDataSet(q);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    purchased = 0; consumed = 0; variance = 0; price = 0; purchasereturn = 0; discard = 0; staff = 0; closing = 0;
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
                    if (cmbkitchen.Text == "All" && checkBox1.Checked==false)
                    {
                        if (cmbstore.Text == "All")
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where (dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "'";
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

                    dspurchase = new DataSet();
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbstore.Text == "All")
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchasereturnDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchasereturnDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchasereturnDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where (dbo.PurchasereturnDetails.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchasereturnDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and dbo.Purchase.BranchCode='" + cmbbranch1.SelectedValue + "' and dbo.Purchase.storeCode='" + cmbstore.SelectedValue + "'";
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
                            val = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            purchasereturn = Convert.ToDouble(val);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    val = "";
                    purchased = Math.Round(purchased, 2);
                    //qty = qty + purchased;

                    try
                    {
                        dspurchase = new DataSet();
                        if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Production.ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and dbo.Production.status='Posted'";
                            }
                            else
                            {

                                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Production.ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and dbo.Production.branchid='" + cmbbranch1.SelectedValue + "'  and dbo.Production.status='Posted'";
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

                    val = "";
                    purchased = Math.Round(purchased, 2);
                    qty = qty + purchased;

                    q = "";
                    dspurchase = new DataSet();
                    try
                    {
                        if (cmbbranch1.Text == "All" )
                        {
                            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  ";
                            }
                            else
                            {
                                if (checkBox1.Checked == true)
                                {
                                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    
                                }
                                else
                                {
                                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                                }
                            }
                        }
                        else
                        {
                            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                            {
                                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'   ";
                            }
                            else
                            {
                                if (checkBox1.Checked == true)
                                {
                                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                                
                                }
                                else
                                {
                                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where kdsid='" + cmbkitchen.SelectedValue + "' and  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                                }
                            }
                        }
                        if (cmbstore.Text != "All")
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
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid is null ";
                        }
                        else
                        {
                            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid is null ";
                        }
                        
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and  isnull(kdsid,'') != ''";
                            }
                            else
                            {
                                q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and  isnull(kdsid,'') != ''";
                            }
                        }
                        else
                        {
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where  branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid='" + cmbkitchen.SelectedValue + "'";
                            }
                        }
                    }
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
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where branchid='" + cmbbranch1.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                    }
                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    try
                    {
                        q = "";
                        if (cmbkitchen.Text == "All" || cmbstore.Text!="All")
                        {
                        }
                        else
                        {
                            dsin = new DataSet();
                            if (cmbbranch1.Text == "All")
                            {
                                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                            }
                            dsin = objCore.funGetDataSet(q);
                            if (dsin.Tables[0].Rows.Count > 0)
                            {
                                val = dsin.Tables[0].Rows[0][0].ToString();
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

                    } q = "";
                    dsin = new DataSet();
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        } 
                    }
                    try
                    {
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
                    }
                    catch (Exception ex)
                    {
                       
                    }

                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                           
                        }
                        else
                        {
                            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        }
                    }
                    try
                    {
                        dsin = objCore.funGetDataSet(q);
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
                    try
                    {
                        dsin = new DataSet();
                        q = "";
                        if (cmbstore.Text == "All")
                        {
                            if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                            {
                                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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
                                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                            }
                            else
                            {
                               // q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and IssuingStoreId='" + cmbstore.SelectedValue + "' and branchid='" + cmbbranch1.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                            }

                        }

                        dsin = objCore.funGetDataSet(q);
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
                    tout = tout + purchasereturn;
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
                    if (cmbkitchen.Text == "All" && checkBox1.Checked == false)
                    {
                        if (cmbbranch1.Text == "All")
                        {
                            q = "SELECT   top 1   remaining,date FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' order by date desc";
                            q = "SELECT   top 1   remaining,date FROM     closing where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid is null  order by date desc";

                        }
                        else
                        {
                            q = "SELECT   top 1   remaining,date FROM     discard where branchid='" + cmbbranch1.SelectedValue + "' and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' order by date desc";
                            q = "SELECT   top 1   remaining,date FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid is null  order by date desc";

                        }
                    }
                    else
                    {
                        if (checkBox1.Checked == true)
                        {
                            q = "SELECT   top 1   remaining,date FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and   isnull(kdsid,'') != '' order by date desc";
                     
                        }
                        else
                        {
                            q = "SELECT   top 1   remaining,date FROM     closing where branchid='" + cmbbranch1.SelectedValue + "' and   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + cmbkitchen.SelectedValue + "' order by date desc";
                        }
                    }
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
                    price = getprice(ds1.Tables[0].Rows[i]["id"].ToString());
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
                    openin = openin * price;
                    purchased = purchased * price;
                    consumed = consumed * price;
                    discard = discard * price;
                    staff = staff * price;
                    cmplt = cmplt * price;
                    closing = closing * price;
                    tout = tout * price;
                    

                    getcompany();
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds1.Tables[0].Rows[i]["Itemname"].ToString() + "(" + ds1.Tables[0].Rows[i]["uom"].ToString() + ")", openin.ToString(), purchased.ToString(), consumed.ToString(), discard.ToString(), staff, cmplt, tint, tout, closing, minorder, balance, null, price, openingval, closingval, purchaseval, saleval, discardval, staffval, comptval,ideal);
                    }

                    else
                    {
                        dtrpt.Rows.Add(ds1.Tables[0].Rows[i]["Itemname"].ToString() + "(" + ds1.Tables[0].Rows[i]["uom"].ToString() + ")", openin.ToString(), purchased.ToString(), consumed.ToString(), discard.ToString(), staff, cmplt, tint, tout, closing, minorder, balance, dscompany.Tables[0].Rows[0]["logo"], price, openingval, closingval, purchaseval, saleval, discardval, staffval, comptval,ideal);

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
