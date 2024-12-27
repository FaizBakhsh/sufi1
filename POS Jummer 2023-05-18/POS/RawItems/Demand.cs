using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class Demand : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public Demand()
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
            string q = "";

            q = "SELECT top 1 date, (remaining) as rem FROM     closing where  Date <'" + date + "' and itemid='" + itemid + "'  and kdsid is null and storeid is null order by Date desc";



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


            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where    dbo.Purchase.date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";

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

            q = "";
            dspurchase = new DataSet();

            try
            {


                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where  (dbo.Production.date  between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "') and dbo.Production.ItemId='" + itemid + "'    and dbo.Production.status='Posted'";



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
            q = "";
            val = "";
            purchased = Math.Round(purchased, 2);
            try
            {

                dspurchase = new DataSet();



                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where   Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and RawItemId='" + itemid + "'";





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


            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
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
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
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



            q = "";



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

            dspurchase = new DataSet(); q = "";


            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'  and kdsid is null";


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
        public double getclosingdata(string itemid)
        {
            double closing1 = 0;
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, complete = 0, transferin = 0,purchasereturn=0, transferout = 0, closing = 0;
            double qty = 0;
            
            string q = "";
            q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.id = '" + itemid + "'  order by dbo.RawItem.itemname";
            

            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                purchased = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; complete = 0; transferin = 0; purchasereturn=0; transferout = 0; closing = 0;
                double openin = 0;
                openin = opening(ds1.Tables[0].Rows[i]["id"].ToString());
                qty = openin;
                string val = "";
                double rem = 0;
                DataSet dspurchase = new DataSet();

                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date ='" + date + "' and  dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";


                //q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date ='" + date + "' and  dbo.Purchase.branchcode ='" + cmbbranch.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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
                dspurchase = new DataSet();
                try
                {
                   
                    {
                        q = "SELECT        SUM(dbo.PurchasereturnDetails.TotalItems) AS Expr1 FROM            dbo.Purchase INNER JOIN                         dbo.PurchasereturnDetails ON dbo.Purchase.Id = dbo.PurchasereturnDetails.PurchaseId where dbo.PurchasereturnDetails.date ='" + date + "' and dbo.PurchasereturnDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                        //q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date ='" + date + "' and  dbo.Purchase.branchcode ='" + cmbbranch.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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
                }
                catch (Exception ex)
                {


                }
                val = "";
                q = "";
                dspurchase = new DataSet();



                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date = '" + dateTimePicker1.Text + "') and dbo.Production.ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'    and dbo.Production.status='Posted'";

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
                purchased = Math.Round(purchased, 3);

                qty = qty + purchased;
                dspurchase = new DataSet(); q = "";
                try
                {




                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where  Date ='" + date + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'   ";



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
                consumed = Math.Round(consumed, 3);
                DataSet dsin = new DataSet(); q = "";

                q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where  Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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
                q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where  Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
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



                q = "";




                
                transferin = Math.Round(transferin, 3);
                transferout = transferout + purchasereturn;
                transferout = Math.Round(transferout, 3);
                val = "";
                double rate = 0;
                DataSet dscon = new DataSet();
                q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                dscon = objcore.funGetDataSet(q);
                if (dscon.Tables[0].Rows.Count > 0)
                {
                    rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                }
                if (rate > 0)
                {
                    consumed = consumed / rate;
                }
                consumed = Math.Round(consumed, 3);
                //qty = qty - consumed;
                dspurchase = new DataSet();
                string remarks = "";
                dspurchase = new DataSet(); q = "";
                try
                {

                    q = "SELECT     (discard) AS Expr1,(staff) AS staff ,(completewaste) AS completewaste,remarks,remaining FROM     discard where  Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' ";

                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        remarks = dspurchase.Tables[0].Rows[0]["remarks"].ToString();
                        val = dspurchase.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        discard = Convert.ToDouble(val);
                        discard = Math.Round(discard, 3);
                        val = dspurchase.Tables[0].Rows[0][1].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        staff = Convert.ToDouble(val);
                        staff = Math.Round(staff, 3);
                        val = dspurchase.Tables[0].Rows[0][2].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        complete = Convert.ToDouble(val);
                        complete = Math.Round(complete, 3);
                    }
                    if (rate > 0)
                    {
                        complete = complete / rate;
                    }
                }
                catch (Exception ex)
                {

                }
                complete = Math.Round(complete, 3);
                string user = "";
                string tempchk = "yes"; q = "";


                q = "SELECT        dbo.Closing.Remaining, dbo.Users.Name FROM            dbo.Closing LEFT OUTER JOIN                         dbo.Users ON dbo.Closing.Userid = dbo.Users.Id  where  dbo.Closing.Date ='" + date + "' and dbo.Closing.itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";




                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    user = dspurchase.Tables[0].Rows[0]["Name"].ToString();
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
                //discard = discard * -1;
                double actual = (openin + purchased + transferin) - (staff + complete + transferout);
                actual = Math.Round(actual, 3);
                actual = actual - closing;
                actual = Math.Round(actual, 3);
                if (tempchk == "yes")

                //if (dspurchase.Tables[0].Rows.Count > 0)
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
                discard = Math.Round(discard, 3);
                //qty = (qty + transferin) - ((discard *-1) + staff + complete + transferout);
                qty = Math.Round(qty, 2);
                closing1 = closing;
            }
            return closing1;
        }
        public void getdata(string search)
        {
            string date = dateTimePicker1.Text;
           
            double qty = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("Closing", typeof(string));
            ds.Columns.Add("Demand", typeof(string));          
            ds.Columns.Add("Unit Price", typeof(string));
            ds.Columns.Add("Total Amount", typeof(string));
            ds.Columns.Add("Status", typeof(string));
            string q = "";// "SELECT     dbo.RawItem.id,dbo.RawItem.ItemName, dbo.InventoryConsumed.RemainingQuantity FROM         dbo.RawItem INNER JOIN                      dbo.InventoryConsumed ON dbo.RawItem.Id = dbo.InventoryConsumed.RawItemId where dbo.InventoryConsumed.date='" + dateTimePicker1.Text + "'";
            if (cmbcat.Text == "All Category")
            {
                if (textBox1.Text.Trim() == "")
                {
                    q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.status='Active' or dbo.RawItem.status is null order by dbo.RawItem.itemname";
                }
                else
                {
                    q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where dbo.RawItem.itemname like '%" + textBox1.Text + "%' and  dbo.RawItem.status='Active' or dbo.RawItem.itemname like '%" + textBox1.Text + "%'  and dbo.RawItem.status is null order by dbo.RawItem.itemname";
                }
            }
            else
            {
                if (textBox1.Text.Trim() == "")
                {
                    q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where RawItem.CategoryId='" + cmbcat.SelectedValue + "'  and  dbo.RawItem.status='Active' or RawItem.CategoryId='" + cmbcat.SelectedValue + "'  and dbo.RawItem.status is null order by dbo.RawItem.itemname";
                }
                else
                {
                    q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where RawItem.CategoryId='" + cmbcat.SelectedValue + "' and  dbo.RawItem.itemname like '%" + textBox1.Text + "%' and  dbo.RawItem.status='Active' or RawItem.CategoryId='" + cmbcat.SelectedValue + "' and  dbo.RawItem.itemname like '%" + textBox1.Text + "%' and  dbo.RawItem.status is null order by dbo.RawItem.itemname";
                }
            }
            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                qty = 0;
                double total = 0,price=0;
                string sts = "Pending";
                q = "SELECT        id, Itemid, Date, Quantity, Status FROM            Demand where itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and date='" + dateTimePicker1.Text + "'";
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
                double closing = getclosingdata(ds1.Tables[0].Rows[i]["id"].ToString());

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString() + "(" + ds1.Tables[0].Rows[i]["uom"].ToString() + ")", closing,  qty.ToString(), price, total, sts);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = false;
            dataGridView1.Columns[4].ReadOnly = true;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            {
                if (gr.Cells["Status"].Value.ToString() == "Uploaded")
                {
                    gr.ReadOnly = true;
                    gr.DefaultCellStyle.BackColor = Color.Green;
                    gr.ReadOnly = true;
                }
            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Text = "Wait Please";
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
                    string temp = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);

                    temp = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double closing = Convert.ToDouble(temp);


                    temp = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double price = Convert.ToDouble(temp);
                    double total = qty * price;
                    total = Math.Round(total, 2);
                    dataGridView1.Rows[e.RowIndex].Cells[5].Value = total.ToString();

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
                        if (Convert.ToDouble(temp1) > 0 && Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Demand"].Value.ToString()) + closing > Convert.ToDouble(temp1))
                        {
                            MessageBox.Show("Your Limit for this Item is  '" + temp1 + "' ");
                            dataGridView1.Rows[e.RowIndex].Cells["Demand"].Value = "0";

                            dss = new DataSet();
                            q = "select * from Demand where itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                            dss = objcore.funGetDataSet(q);
                            if (dss.Tables[0].Rows.Count > 0)
                            {

                                q = "update Demand set supplierid='" + cmbcat.SelectedValue + "', Quantity='0'  where   itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                                objcore.executeQuery(q);
                            }

                            return;
                        }
                    }
                    double closingw = 0;
                    if (chkclosing == true)
                    {
                        closingw = getclosing(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                        if (closingw < Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Demand"].Value.ToString()), 2))
                        {
                            MessageBox.Show("This Stock is not Available at Warehouse. You can Maximum Demand  " + closingw.ToString() + "  Items");
                            return;
                        }
                    }
                    dss = new DataSet();
                    q = "select * from Demand where itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {

                        q = "update Demand set supplierid='" + cmbcat.SelectedValue + "', Quantity='" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Demand"].Value.ToString()), 2).ToString() + "', Closing='" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Closing"].Value.ToString()), 2).ToString() + "'  where   itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                        objcore.executeQuery(q);
                    }
                    else
                    {
                        q = "insert into Demand (Closing,supplierid,Itemid, Date, Quantity, Status) values('" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Closing"].Value.ToString()), 2).ToString() + "' ,'" + cmbcat.SelectedValue + "','" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Demand"].Value.ToString()), 2).ToString() + "','Pending')";
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
            string q = "select id from Branch where status='Active'";           
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
        protected void uploadbyservice()
        {
            branchid = cmbbranch.SelectedValue.ToString();
            if (branchid == "")
            {
                getbranchid();
            }
            if (url == "")
            {
                type();
            }
            try
            {
                bool chk = false;
                string URI = url + "/DeliveryServices/uploaddemand.asmx/Getresponse";
                string myparametrs = "";
                string q = "select   * from demand where status='Pending' and date='" + dateTimePicker1.Text + "' and Quantity>0";
               DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (myparametrs != "")
                        {
                            myparametrs = myparametrs + ",";
                        }
                        myparametrs = myparametrs + "{\"Onlineid\":\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"itemid\":\"" + ds.Tables[0].Rows[i]["itemid"].ToString() + "\",\"date\":\"" + ds.Tables[0].Rows[i]["date"].ToString() + "\",\"Quantity\":\"" + ds.Tables[0].Rows[i]["Quantity"].ToString().Replace("'", "") + "\",\"Status\":\"" + ds.Tables[0].Rows[i]["Status"].ToString().Replace("'", "") + "\",\"branchid\":\"" + branchid + "\",\"supplierid\":\"" + ds.Tables[0].Rows[i]["supplierid"].ToString() + "\",\"closing\":\"" + ds.Tables[0].Rows[i]["closing"].ToString() + "\"}";
                    }
                    string msg = "";
                    myparametrs = "[" + myparametrs + "]";
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        //wc.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                        string HtmlResult = wc.UploadString(URI, myparametrs);
                        msg = HtmlResult;
                        //txt_postData.Text = HtmlResult;
                        if (HtmlResult.Contains("success"))
                        {
                            chk = true;
                        }
                        else
                        {
                            chk = false;
                        }
                    }
                    if (chk == false)
                    {
                         MessageBox.Show(msg);
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            q = "update demand set status='Uploaded' where id='" + ds.Tables[0].Rows[i]["id"] + "'";
                            objcore.executeQuery(q);
                        }
                        MessageBox.Show("Data uploaded successfully");
                        getdata(textBox1.Text);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Inventory Transfer" + ex.Message);
            }
        }
       
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='demand'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
                }
                else
                {
                    q = "select * from deliverytransfer where server='main'";
                   ds = new DataSet();
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tp = ds.Tables[0].Rows[0]["type"].ToString();
                        url = ds.Tables[0].Rows[0]["url"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {


            }
            if (tp == "")
            {
                tp = "sql";
            }
            return tp;
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                vButton2.Text = "Please Wait";
                vButton2.Enabled = false;
                uploadbyservice();
                vButton2.Enabled = true;
                vButton2.Text = "Post Demand";
                return;
            }
            bool chk = true;
            DialogResult drr = MessageBox.Show("Are you sure to Post", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (drr == DialogResult.No)
            {
                return;
            }
            try
            {
                DataSet ds = new System.Data.DataSet();
                ds = objcore.funGetDataSet("select * from SqlServerInfo where type='server'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string server = ds.Tables[0].Rows[0]["ServerName"].ToString();
                    string db = ds.Tables[0].Rows[0]["DbName"].ToString();
                    string user = ds.Tables[0].Rows[0]["UserName"].ToString();
                    string password = ds.Tables[0].Rows[0]["Password"].ToString();
                    cs = "Data Source=" + server + ";Initial Catalog=" + db + ";Persist Security Info=True;User ID=" + user + ";Password=" + password + "";
                }
            }
            catch (Exception ex)
            {


            }
            if (branchid == "")
            {
                getbranchid();
            }
          
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                string q ="";
                q = "select id from demand where  itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                DataSet dsd=new DataSet();
                dsd=objcore.funGetDataSet(q);
                if(dsd.Tables[0].Rows.Count>0)
                {
                    q="delete from demand where branchid='" + branchid + "' and itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                    SqlConnection connection = new SqlConnection(cs);
                    SqlCommand com;
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    connection.Open();
                    com = new SqlCommand(q, connection);
                    com.ExecuteNonQuery();
                    connection.Close();
                }
                if (Convert.ToDouble(dr.Cells["Demand"].Value.ToString()) > 0)
                {
                    q = "insert into Demand (Itemid, Date, Quantity, Status,branchid) values('" + dr.Cells["id"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dr.Cells["Demand"].Value.ToString()), 2).ToString() + "','Pending','" + branchid + "')";

                    try
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        int res = com.ExecuteNonQuery();
                        connection.Close();
                        if (res == 1)
                        {
                            chk = false;
                            q = "update demand set status='Uploaded' where  itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                            objcore.executeQuery(q);
                        }
                    }
                    catch (Exception ex)
                    {

                        // MessageBox.Show(ex.Message);
                    }
                }
            }
            if (chk == false)
            {
                MessageBox.Show("Record saved successfully");
                getdata(textBox1.Text);
            }
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
           
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FoodDiscard_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            try
            {
                string q = "select * from Category";
                DataSet ds = objcore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["CategoryName"] = "All Category";
                ds.Tables[0].Rows.Add(dr);
                cmbcat.DataSource = ds.Tables[0];

                cmbcat.ValueMember = "id";
                cmbcat.DisplayMember = "CategoryName";
                cmbcat.SelectedItem = "All Category";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                string q = "select * from branch where status='Active'";
                DataSet ds = objcore.funGetDataSet(q);
              
                cmbbranch.DataSource = ds.Tables[0];

                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "BranchName";
           
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            bool chk = false;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                chk1 = false;


                DataSet dss = new DataSet();
                int id = 0;

               

                dss = new DataSet();
                string q = "select * from Demand where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                dss = objcore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {

                    q = "update Demand set supplierid='"+cmbcat.SelectedValue+"', Quantity='" + Math.Round(Convert.ToDouble(dr.Cells["Demand"].Value.ToString()), 2).ToString() + "'  where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                    objcore.executeQuery(q);
                    chk1 = true;
                    chk = true;

                }
                else
                {
                    q = "insert into Demand (supplierid,Itemid, Date, Quantity, Status) values('" + cmbcat.SelectedValue + "','" + dr.Cells["id"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dr.Cells["Demand"].Value.ToString()), 2).ToString() + "','Pending')";
                    objcore.executeQuery(q);
                    chk = true;
                }
            }
            if (chk == true)
            {
                //try
                //{
                //    string qq = "select * from Mailsetting";
                //    DataSet dsmail = new DataSet();
                //    dsmail = objcore.funGetDataSet(qq);
                //    if (dsmail.Tables[0].Rows.Count > 0)
                //    {
                //        if (dsmail.Tables[0].Rows[0]["status"].ToString() == "Enabled")
                //        {
                //            //Thread sf = new Thread(new ThreadStart(showform));
                //            //sf.Start();
                //            showform();
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{


                //}
                MessageBox.Show("Record Saved Successfully");
                getdata(textBox1.Text);
            }
        }
        protected double getclosing(string id)
        {
            double closing = 0;
            try
            {
                closingclass = new List<ClosingClass>();
                string uri = url + "/closingwarehouse.asmx/Getresponse?id=" + id + "&end=" + DateTime.Now.ToString("yyyy-MM-dd");
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    closingclass = (List<ClosingClass>)JsonConvert.DeserializeObject(result, typeof(List<ClosingClass>));
                    closing = closingclass[0].Quantity;

                }
            }
            catch (Exception ex)
            {


            }
            return closing;           
        }
        bool chkclosing = false;
        List<ClosingClass> closingclass = new List<ClosingClass>();
        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            progressBar1.BeginInvoke(
                 new Action(() =>
                 {
                     progressBar1.Visible = true;
                     progressBar1.Style = ProgressBarStyle.Marquee;
                     progressBar1.MarqueeAnimationSpeed = 30;


                     type();
                     this.Enabled = false;


                     try
                     {
                         string uri = url + "/closingwarehouse.asmx/Getresponse?end=" + DateTime.Now.ToString("yyyy-MM-dd");
                         HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                         HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                         Stream stream1 = response1.GetResponseStream();
                         using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                         {
                             // Load into XML document
                             string result = readStream.ReadToEnd();
                             closingclass = (List<ClosingClass>)JsonConvert.DeserializeObject(result, typeof(List<ClosingClass>));


                         }
                     }
                     catch (Exception ex)
                     {


                     }
                     
                 }
             ));
            
        }
        private BackgroundWorker worker = new BackgroundWorker();
        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressBar1.BeginInvoke(
                   new Action(() =>
                   {
                       this.Enabled = true;
                       progressBar1.Visible = false;
                       progressBar1.Style = ProgressBarStyle.Marquee;
                       progressBar1.MarqueeAnimationSpeed = 0;
                   }
               ));
            
        }
        private void Demand_Shown(object sender, EventArgs e)
        {
           
           
            string q = "select * from DeviceSetting where Device='Check Closing Before Demand'";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
               

                closingclass = new List<ClosingClass>();
                string status = ds.Tables[0].Rows[0]["Status"].ToString();
                if (status == "Enabled")
                {
                    chkclosing = true;
                    type();
                    //worker.WorkerSupportsCancellation = true;
                    //worker.DoWork += new DoWorkEventHandler(worker_DoWork);
                    //worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);

                    //worker.RunWorkerAsync();

                }
            }
            
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            {
                if (gr.Cells["Status"].Value.ToString() == "Uploaded")
                {
                    gr.ReadOnly = true;
                    gr.DefaultCellStyle.BackColor = Color.Green;
                    gr.ReadOnly = true;
                }
            }
        }
    }
}
