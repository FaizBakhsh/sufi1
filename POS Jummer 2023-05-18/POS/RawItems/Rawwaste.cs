﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class Rawwaste : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public Rawwaste()
        {
            InitializeComponent();
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
            string q = "SELECT top 1 date, (remaining) as rem FROM     discard where Date <'" + date + "' and itemid='" + itemid + "'  and remaining is not null order by Date desc";
            if (cmbkitchen.Text == "All")
            {
                if (cmbstore.Text == "All")
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid ='" + cmbbranch.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "'  and kdsid is null and storeid is null order by Date desc";

                }
                else
                {
                    q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid ='" + cmbbranch.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "'  and kdsid is null and storeid='" + cmbstore.SelectedValue + "'  order by Date desc";

                }
            }
            else
            {
                q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid ='" + cmbbranch.SelectedValue + "' and Date <'" + date + "' and itemid='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "'  order by Date desc";
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
            q = "";
            if (cmbkitchen.Text == "All")
            {
                if (cmbstore.Text == "All")
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where  dbo.purchase.branchcode ='" + cmbbranch.SelectedValue + "' and  dbo.Purchase.date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                }
                else
                {
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.purchase.StoreCode ='" + cmbstore.SelectedValue + "' and dbo.purchase.branchcode ='" + cmbbranch.SelectedValue + "' and  dbo.Purchase.date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
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
            } q = "";
            dspurchase = new DataSet();
            if (cmbkitchen.Text == "All")
            {
                try
                {
                    if (cmbbranch.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";
                    }
                    else
                    {
                        if (cmbstore.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where  (dbo.Production.date  between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "') and dbo.Production.ItemId='" + itemid + "'  and dbo.Production.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Production.status='Posted'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where dbo.Production.storeid='" + cmbstore.SelectedValue + "' and (dbo.Production.date  between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "') and dbo.Production.ItemId='" + itemid + "'  and dbo.Production.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Production.status='Posted'";
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
            } q = "";
            val = "";
            purchased = Math.Round(purchased, 2);
            try
            {

                dspurchase = new DataSet();
                if (cmbkitchen.Text == "All")
                {
                    if (cmbstore.Text == "All")
                    {
                         q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid ='" + cmbbranch.SelectedValue + "' and  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and RawItemId='" + itemid + "' and kdsid is null";
                    }
                    else
                    {
                        //q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid ='" + cmbbranch.SelectedValue + "' and  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and RawItemId='" + itemid + "'";
                    }

                }
                else
                {
                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid ='" + cmbbranch.SelectedValue + "' and  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and RawItemId='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "'";

                }
                // MessageBox.Show(q);
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    consumed = Convert.ToDouble(val);
                    //   MessageBox.Show(consumed.ToString());
                }
            }
            catch (Exception ex) 
            {
                
            }
            
            q = "";
            DataSet dsin = new DataSet();
            if (cmbkitchen.Text == "All")
            {
                if (cmbstore.Text == "All")
                {
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where branchid ='" + cmbbranch.SelectedValue + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
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
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where branchid ='" + cmbbranch.SelectedValue + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
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
                else
                {
                    q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where IssuingStoreId='" + cmbstore.SelectedValue + "' and Date  between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
                    dsin = objcore.funGetDataSet(q);
                    if (dsin.Tables[0].Rows.Count > 0)
                    {
                        val = dsin.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        transferout =transferout+ Convert.ToDouble(val);
                    }
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
                        dsin = objcore.funGetDataSet(q);
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
            }
            q = "";
            if (cmbkitchen.Text == "All")
            {
                if (cmbstore.Text=="All")
                {
                    if (cmbbranch.Text == "All")
                    {

                    }
                    else
                    {
                        q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where sourcebranchid='" + cmbbranch.SelectedValue + "' and  Date between '" + start + "' and '" + end + "' and itemid='" + itemid + "'";
                    } 
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
                    transferout = transferout + Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            } q = "";
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
            dspurchase = new DataSet(); q = "";
            if (cmbkitchen.Text == "All")
            {
                if (cmbstore.Text == "All")
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where branchid ='" + cmbbranch.SelectedValue + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'  and kdsid is null";
                }
                else
                {
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where storeid='"+cmbstore.SelectedValue+"' and branchid ='" + cmbbranch.SelectedValue + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'  and kdsid is null";
                }
            }
            else
            {
                q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where branchid ='" + cmbbranch.SelectedValue + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "' and kdsid='" + cmbkitchen.SelectedValue + "'";

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

            //MessageBox.Show(closing.ToString() + "-p-" + purchased.ToString() + "trin-" + transferin.ToString() + "staff-" + staff + "complete-" + complt + "trout-" + transferout + "consumed-" + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        public void getdata(string search)
        {
            try
            {
                string userid = POSRestaurant.Properties.Settings.Default.UserId.ToString();
                string qq = "insert into log (Name, Time, Description,userid) values ('Closing','" + DateTime.Now + "','Closing Page Opened','" + userid + "')";
                objcore.executeQuery(qq);

            }
            catch (Exception ex)
            {

            }

            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, complete = 0, transferin = 0, transferout = 0, closing = 0;
            double qty = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            
            ds.Columns.Add("Waste/Staff Consumption", typeof(string));
          
            ds.Columns.Add("Remarks", typeof(string));
            ds.Columns.Add("Closing_By_User", typeof(string));
            string q = "";// "SELECT     dbo.RawItem.id,dbo.RawItem.ItemName, dbo.InventoryConsumed.RemainingQuantity FROM         dbo.RawItem INNER JOIN                      dbo.InventoryConsumed ON dbo.RawItem.Id = dbo.InventoryConsumed.RawItemId where dbo.InventoryConsumed.date='" + dateTimePicker1.Text + "'";
            if (textBox1.Text.Trim() == "")
            {
                q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.status ='Active' or dbo.RawItem.status is null ORDER BY dbo.RawItem.ItemName";
            }
            else
            {
                q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.itemname like '%" + textBox1.Text + "%' and dbo.RawItem.status ='Active' or dbo.RawItem.itemname like '%" + textBox1.Text + "%' and  dbo.RawItem.status is null order by dbo.RawItem.itemname";
            }

            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                purchased = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; complete = 0; transferin = 0; transferout = 0; closing = 0;
                double openin = 0;
                openin = opening(ds1.Tables[0].Rows[i]["id"].ToString());
                qty = openin;
                string val = "";
                double rem = 0;
                DataSet dspurchase = new DataSet();
                string user = "";
                //qty = qty - consumed;
                dspurchase = new DataSet();
                string remarks = "";
                dspurchase = new DataSet(); q = "";
                try
                {
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbstore.Text == "All")
                        {
                            q = "SELECT     (discard) AS Expr1,(staff) AS staff ,(completewaste) AS completewaste,remarks,userid FROM     discard where branchid ='" + cmbbranch.SelectedValue + "' and Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid is null ";
                        }
                        else
                        {
                            q = "SELECT     (discard) AS Expr1,(staff) AS staff ,(completewaste) AS completewaste,remarks,userid FROM     discard where storeid='" + cmbstore.SelectedValue + "' and branchid ='" + cmbbranch.SelectedValue + "' and Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and kdsid is null ";
                        }
                    }
                    else
                    {
                        q = "SELECT     (discard) AS Expr1,(staff) AS staff ,(completewaste) AS completewaste,remarks,userid FROM     discard where branchid ='" + cmbbranch.SelectedValue + "' and Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                    }
                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        remarks = dspurchase.Tables[0].Rows[0]["remarks"].ToString();
                        user = getuser(dspurchase.Tables[0].Rows[0]["userid"].ToString());
                        val = dspurchase.Tables[0].Rows[0][1].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        staff = Convert.ToDouble(val);
                        staff = Math.Round(staff, 3);
                        
                    }
                    
                }
                catch (Exception ex)
                {
                    
                }
                complete = Math.Round(complete, 3);
               
                string tempchk = "yes"; q = "";
              

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), staff.ToString(),  remarks, user);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
            
        }
        protected string getuser(string id)
        {
            string name = "";

            try
            {
                string q = "select name from users where id='" + id + "'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    name = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
            }

            return name;

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata(textBox1.Text);
        }
        bool chkclosingflag = false;
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {


            if (e.ColumnIndex == 2)
            {
                try
                {
                    string val = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    if (val == "")
                    {
                        val = "0";

                    }

                    double staff = Convert.ToDouble(val);




                    savedata(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString(), dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString(), staff.ToString());
                    //discardval = discardval * -1;
                    ////  double remaining = Convert.ToDouble(val);
                    //dataGridView1.Rows[e.RowIndex].Cells[10].Value = Math.Round(((opening + purchaes+tin) - consumed) - (discardval + staff + complete+tout), 2).ToString();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }


        }
    
        protected void savedata(string itemid, string remarks,string staff)
        {
            chk1 = false;
            string userid = POSRestaurant.Properties.Settings.Default.UserId;
            {
                DataSet dss = new DataSet();
                int id = 0;
                dss = objcore.funGetDataSet("select max(id) as id from Discard");
                if (dss.Tables[0].Rows.Count > 0)
                {
                    string i = dss.Tables[0].Rows[0][0].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    id = Convert.ToInt32(i) + 1;
                }
                else
                {
                    id = 1;
                }


                if (branchid == "")
                {
                    branchid = cmbbranch.SelectedValue.ToString();
                }
                dss = new DataSet();
                string q = "select * from Discard where itemid='" + itemid + "' and date='" + dateTimePicker1.Text + "'";
                if (cmbkitchen.Text != "All")
                {
                    q = "select * from Discard where itemid='" + itemid + "' and date='" + dateTimePicker1.Text + "' and kdsid='" + cmbkitchen.SelectedValue + "'";
                }
                else
                {
                    if (cmbstore.Text == "All")
                    {
                        q = "select * from Discard where itemid='" + itemid + "' and date='" + dateTimePicker1.Text + "' and kdsid is null";
                    }
                    else
                    {
                        q = "select * from Discard where itemid='" + itemid + "' and date='" + dateTimePicker1.Text + "' and kdsid is null and storeid='" + cmbstore.SelectedValue + "'";
                    }
                }
                dss = objcore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbstore.Text == "All")
                        {
                            q = "update Discard set  userid='" + userid + "',  branchid='" + branchid + "',  uploadstatus='Pending',Remarks='" + remarks.Replace("&", "n").Replace("'", "-") + "',staff='" + Math.Round(Convert.ToDouble(staff), 2).ToString() + "' where   itemid='" + itemid + "' and date='" + dateTimePicker1.Text + "'";
                        }
                        else
                        {
                            q = "update Discard set userid='" + userid + "',  branchid='" + branchid + "',  uploadstatus='Pending',Remarks='" + remarks.Replace("&", "n").Replace("'", "-") + "',staff='" + Math.Round(Convert.ToDouble(staff), 2).ToString() + "' where   itemid='" + itemid + "' and storeid='" + cmbstore.SelectedValue + "' and date='" + dateTimePicker1.Text + "'";
                 
                        }
                    }
                    else
                    {

                        q = "update Discard set userid='" + userid + "',  kdsid='" + cmbkitchen.SelectedValue + "',branchid='" + branchid + "',  uploadstatus='Pending',Remarks='" + remarks.Replace("&", "n").Replace("'", "-") + "',staff='" + Math.Round(Convert.ToDouble(staff), 2).ToString() + "' where   itemid='" + itemid + "' and date='" + dateTimePicker1.Text + "' and kdsid='" + cmbkitchen.SelectedValue + "'";

                    }
                    objcore.executeQuery(q);
                }
                else
                {
                    if (cmbkitchen.Text == "All")
                    {
                        if (cmbstore.Text == "All")
                        {
                            q = "insert into Discard (userid,branchid,id,itemid,date,quantity,Discard,Remaining,staff,Remarks) values('" + userid + "','" + branchid + "','" + id + "','" + itemid + "','" + dateTimePicker1.Text + "','0','0','0','" + Math.Round(Convert.ToDouble(staff), 2).ToString() + "','" + remarks.Replace("&", "n").Replace("'", "-") + "')";
                        }
                        else
                        {
                            q = "insert into Discard (userid,storeid,branchid,id,itemid,date,quantity,Discard,Remaining,staff,Remarks) values('" + userid + "','" + cmbstore.SelectedValue + "','" + branchid + "','" + id + "','" + itemid + "','" + dateTimePicker1.Text + "','0','0','0','" + Math.Round(Convert.ToDouble(staff), 2).ToString() + "','" + remarks.Replace("&", "n").Replace("'", "-") + "')";
                        }
                        
                    }
                    else
                    {
                        q = "insert into Discard (userid,kdsid,branchid,id,itemid,date,quantity,Discard,Remaining,staff,Remarks) values('" + userid + "','" + cmbkitchen.SelectedValue + "','" + branchid + "','" + id + "','" + itemid + "','" + dateTimePicker1.Text + "','0','0','0','" + Math.Round(Convert.ToDouble(staff), 2).ToString() + "','" + remarks.Replace("&", "n").Replace("'", "-") + "')";
                  
                    }
                    objcore.executeQuery(q);

                }


            }
        }
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            bool chk = false;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                chk1 = false;
                if (dr.Cells[5].Value.ToString() != string.Empty)
                {
                    DataSet dss = new DataSet();
                    int id = 0;
                    dss = objcore.funGetDataSet("select max(id) as id from Discard");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string i = dss.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        id = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        id = 1;
                    }
                    double variance = (Convert.ToDouble(dr.Cells["Closing"].Value.ToString()));

                    if (branchid == "")
                    {
                        getbranchid();
                    }
                    dss = new DataSet();
                    string q = "select * from Discard where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        double oldqty = Convert.ToDouble(dss.Tables[0].Rows[0]["Discard"].ToString());
                        variance = oldqty - (Convert.ToDouble(dr.Cells["Variance"].Value.ToString()));
                        q = "update Discard set  branchid='" + branchid + "',  uploadstatus='Pending',Remarks='" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "',Discard='" + Math.Round(Convert.ToDouble(dr.Cells["Variance"].Value.ToString()), 2).ToString() + "',staff='" + Math.Round(Convert.ToDouble(dr.Cells["Waste/Staff Consumption"].Value.ToString()), 2).ToString() + "',Remaining ='" + dr.Cells["Closing"].Value.ToString() + "' where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                        objcore.executeQuery(q);
                        chk1 = true;
                        chk = true;
                    }
                    else
                    {
                        q = "insert into Discard (branchid,id,itemid,date,quantity,Discard,Remaining,staff,Remarks) values('" + branchid + "','" + id + "','" + dr.Cells["id"].Value.ToString() + "','" + dateTimePicker1.Text + "','0','" + Math.Round(Convert.ToDouble(dr.Cells["Variance"].Value.ToString()), 2).ToString() + "','" + dr.Cells["Closing"].Value.ToString() + "','" + dr.Cells["Waste/Staff Consumption"].Value.ToString() + "','" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "')";
                        objcore.executeQuery(q);
                        chk = true;
                    }
                }
            }
            if (chk == true)
            {
                try
                {
                    string qq = "select * from Mailsetting";
                    DataSet dsmail = new DataSet();
                    dsmail = objcore.funGetDataSet(qq);
                    if (dsmail.Tables[0].Rows.Count > 0)
                    {
                        if (dsmail.Tables[0].Rows[0]["status"].ToString() == "Enabled")
                        {
                            //Thread sf = new Thread(new ThreadStart(showform));
                            //sf.Start();
                            showform();
                        }
                    }
                }
                catch (Exception ex)
                {


                }
                MessageBox.Show("Record Saved Successfully");
                getdata(textBox1.Text);
            }
        }
        protected void getbranchid()
        {
            try
            {
                string q = "select id from branch where id='" + cmbbranch.SelectedValue + "'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchid = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
        }
        string branchid = "";
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
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void fillitems()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];
                cmbbranch.DataSource = dt;
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "BranchName";

                fillstores();

            }
            catch (Exception ex)
            {


            }


        }
        public void fillstores()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Stores where BranchId='" + cmbbranch.SelectedValue + "'";
                ds = objcore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["StoreName"] = "All";
                ds.Tables[0].Rows.Add(dr);
                dt = ds.Tables[0];
               
                cmbstore.DataSource = dt;
                cmbstore.ValueMember = "id";
                cmbstore.DisplayMember = "StoreName";
                cmbstore.Text = "All";
                cmbkitchen.SelectedValue = "0";

            }
            catch (Exception ex)
            {


            }


        }
        protected void fillkds()
        {
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,name from KDS where id>0";
                dsi = objcore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["name"] = "All";
                dr["id"] = "0";
                dsi.Tables[0].Rows.Add(dr);
                cmbkitchen.DataSource = dsi.Tables[0];
                cmbkitchen.ValueMember = "id";
                cmbkitchen.DisplayMember = "name";
                cmbkitchen.SelectedValue = "0";

            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
        }
        string username = "";
        private void FoodDiscard_Load(object sender, EventArgs e)
        {
            fillkds();
            fillitems();
            try
            {
                string userid = POSRestaurant.Properties.Settings.Default.UserId.ToString();
                string q = "select name from users where id='" + userid + "'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    username = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }

        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 10)
            {
                chkclosingflag = false;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillstores();
        }

        private void cmbstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbstore.Text != "All")
                {
                    cmbkitchen.SelectedValue = "0";
                    cmbkitchen.Enabled = false;
                }
                else
                {
                    cmbkitchen.Enabled = true;
                }
            }
            catch (Exception ew)
            {
                
            }
        }
    }
}