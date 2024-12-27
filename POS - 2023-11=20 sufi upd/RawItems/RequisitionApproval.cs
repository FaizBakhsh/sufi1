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
    public partial class RequisitionApproval : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public RequisitionApproval()
        {
            InitializeComponent();
        }
       
        public void getdata()
        {
            string date = dateTimePicker1.Text;
           
            double qty = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("UOM", typeof(string));
            ds.Columns.Add("Quantity", typeof(string));          
            ds.Columns.Add("Expected Price", typeof(string));
            ds.Columns.Add("Total Amount", typeof(string));
            ds.Columns.Add("Remarks", typeof(string));
            ds.Columns.Add("Status", typeof(string));
            string q = "";
            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Requisition ON dbo.RawItem.Id = dbo.Requisition.ItemId where dbo.Requisition.branchid='" + cmbbranch.SelectedValue + "' order by dbo.RawItem.itemname";
              
            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                qty = 0;
                double total = 0,price=0;
                string sts = "Pending",remarks="";
                q = "SELECT  Id, ItemId, Quantity, Price, TotalAmount, Remarks, Status, branchid FROM            Requisition where itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and date='" + dateTimePicker1.Text + "'";
                DataSet dsdmnd = new DataSet();
                dsdmnd = objcore.funGetDataSet(q);
                if (dsdmnd.Tables[0].Rows.Count > 0)
                {
                    remarks = dsdmnd.Tables[0].Rows[0]["Remarks"].ToString();
                    sts = dsdmnd.Tables[0].Rows[0]["Status"].ToString();
                    string tmp = dsdmnd.Tables[0].Rows[0]["Quantity"].ToString();
                    if (tmp == "")
                    {
                        tmp = "0";
                    }
                    qty = Convert.ToDouble(tmp);
                    tmp = dsdmnd.Tables[0].Rows[0]["Price"].ToString();
                    if (tmp == "")
                    {
                        tmp = "0";
                    }
                    price = Convert.ToDouble(tmp);



                }
                if (price == 0)
                {
                    string tmp1 = ds1.Tables[0].Rows[i]["price"].ToString();
                    if (tmp1 == "")
                    {
                        tmp1 = "0";
                    }
                    price = Convert.ToDouble(tmp1);
                }
                total = price * qty;
               total= Math.Round(total, 2);
                

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), ds1.Tables[0].Rows[i]["uom"].ToString(),  qty.ToString(), price, total,remarks, sts);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = false;
            dataGridView1.Columns[4].ReadOnly = false;
            dataGridView1.Columns[5].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = false;
            dataGridView1.Columns[7].ReadOnly = true;
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            {
                if (gr.Cells["Status"].Value.ToString() == "Approved")
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
            getdata();
            vButton1.Text = "Search";
            vButton1.Enabled = true;
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            //try
            //{
            //    if (e.ColumnIndex == 3)
            //    {
            //        string temp = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            //        if (temp == "")
            //        {
            //            temp = "0";
            //        }
            //        double qty = Convert.ToDouble(temp);

                   


            //        temp = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            //        if (temp == "")
            //        {
            //            temp = "0";
            //        }
            //        double price = Convert.ToDouble(temp);
            //        double total = qty * price;
            //        total = Math.Round(total, 2);
            //        dataGridView1.Rows[e.RowIndex].Cells[5].Value = total.ToString();

            //        DataSet dss = new DataSet();
            //        int id = 0;

            //        string q = "select * from rawitem where id='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
            //        dss = new DataSet();
            //        q = "select * from Requisition where itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
            //        dss = objcore.funGetDataSet(q);
            //        if (dss.Tables[0].Rows.Count > 0)
            //        {

            //            q = "update Requisition set  Quantity='" + dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() + "'  where   itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
            //            objcore.executeQuery(q);
            //        }
            //        else
            //        {
            //            q = "insert into Requisition (ItemId, Quantity,Date, Price, TotalAmount, Remarks, Status) values('" + (dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString()).ToString() + "','" + dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Expected Price"].Value.ToString()), 2).ToString() + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Total Amount"].Value.ToString()), 2).ToString() + "','" + dataGridView1.Rows[e.RowIndex].Cells["Remarks"].Value.ToString() + "','Pending')";
            //            objcore.executeQuery(q);

            //        }


            //    }

            //}
            //catch (Exception ex)
            //{

            //  //  MessageBox.Show(ex.Message);
            //}
            //try
            //{
            //    if (e.ColumnIndex == 4)
            //    {
            //        string temp = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            //        if (temp == "")
            //        {
            //            temp = "0";
            //        }
            //        double qty = Convert.ToDouble(temp);




            //        temp = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            //        if (temp == "")
            //        {
            //            temp = "0";
            //        }
            //        double price = Convert.ToDouble(temp);
            //        double total = qty * price;
            //        total = Math.Round(total, 2);
            //        dataGridView1.Rows[e.RowIndex].Cells[5].Value = total.ToString();

            //        DataSet dss = new DataSet();
            //        int id = 0;

            //        string q = "select * from rawitem where id='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
            //        dss = new DataSet();
            //        q = "select * from Requisition where itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
            //        dss = objcore.funGetDataSet(q);
            //        if (dss.Tables[0].Rows.Count > 0)
            //        {

            //            q = "update Requisition set  Remarks='" + dataGridView1.Rows[e.RowIndex].Cells["Remarks"].Value.ToString() + "',Price='" + dataGridView1.Rows[e.RowIndex].Cells["Expected Price"].Value.ToString() + "',TotalAmount='" + dataGridView1.Rows[e.RowIndex].Cells["Total Amount"].Value.ToString() + "'  where   itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
            //            objcore.executeQuery(q);
            //        }
            //        else
            //        {
            //            q = "insert into Requisition (ItemId, Quantity,Date, Price, TotalAmount, Remarks, Status) values('" + (dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString()).ToString() + "','" + dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Expected Price"].Value.ToString()), 2).ToString() + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Total Amount"].Value.ToString()), 2).ToString() + "','" + dataGridView1.Rows[e.RowIndex].Cells["Remarks"].Value.ToString() + "','Pending')";
            //            objcore.executeQuery(q);

            //        }


            //    }

            //}
            //catch (Exception ex)
            //{

            //   // MessageBox.Show(ex.Message);
            //}
            //try
            //{
            //    if (e.ColumnIndex == 6)
            //    {
            //        string temp = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            //        if (temp == "")
            //        {
            //            temp = "0";
            //        }
            //        double qty = Convert.ToDouble(temp);




            //        temp = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            //        if (temp == "")
            //        {
            //            temp = "0";
            //        }
            //        double price = Convert.ToDouble(temp);
            //        double total = qty * price;
            //        total = Math.Round(total, 2);
            //        dataGridView1.Rows[e.RowIndex].Cells[5].Value = total.ToString();

            //        DataSet dss = new DataSet();
            //        int id = 0;

            //        string q = "select * from rawitem where id='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
            //        dss = new DataSet();
            //        q = "select * from Requisition where itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
            //        dss = objcore.funGetDataSet(q);
            //        if (dss.Tables[0].Rows.Count > 0)
            //        {

            //            q = "update Requisition set  Remarks='" + dataGridView1.Rows[e.RowIndex].Cells["Remarks"].Value.ToString() + "'  where   itemid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
            //            objcore.executeQuery(q);
            //        }
            //        else
            //        {
            //            q = "insert into Requisition (ItemId, Quantity,Date, Price, TotalAmount, Remarks, Status) values('" + (dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString()).ToString() + "','" + dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Expected Price"].Value.ToString()), 2).ToString() + "','" + Math.Round(Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Total Amount"].Value.ToString()), 2).ToString() + "','" + dataGridView1.Rows[e.RowIndex].Cells["Remarks"].Value.ToString() + "','Pending')";
            //            objcore.executeQuery(q);

            //        }


            //    }

            //}
            //catch (Exception ex)
            //{

            //    //MessageBox.Show(ex.Message);
            //}
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
        protected void uploadbyservice()
        {
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
                string URI = url + "/DeliveryServices/uploaddemand.asmx/Getresponse?type=req";
                string myparametrs = "";
                string q = "select   * from Requisition where status='Pending' and date='" + dateTimePicker1.Text + "' and Quantity>0";
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
                        myparametrs = myparametrs + "{\"Onlineid\":\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"itemid\":\"" + ds.Tables[0].Rows[i]["itemid"].ToString() + "\",\"date\":\"" + ds.Tables[0].Rows[i]["date"].ToString() + "\",\"Quantity\":\"" + ds.Tables[0].Rows[i]["Quantity"].ToString().Replace("'", "") + "\",\"Status\":\"" + ds.Tables[0].Rows[i]["Status"].ToString().Replace("'", "") + "\",\"branchid\":\"" + branchid + "\",\"Price\":\"" + ds.Tables[0].Rows[i]["Price"].ToString() + "\",\"Amount\":\"" + ds.Tables[0].Rows[i]["TotalAmount"].ToString() + "\",\"Remarks\":\"" + ds.Tables[0].Rows[i]["Remarks"].ToString() + "\"}";
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
                            q = "update Requisition set status='Posted' where id='" + ds.Tables[0].Rows[i]["id"] + "'";
                            objcore.executeQuery(q);
                        }
                        MessageBox.Show("Data uploaded successfully");
                        getdata();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show( ex.Message);
            }
        }
       
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='main'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
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
            string q = "update Requisition set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "', status='Approved' where date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "'";
            int res= objcore.executeQueryint(q);
            if (res > 0)
            {
                MessageBox.Show("Request Approved Successfully");
                getdata();
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
            POSRestaurant.Reports.Inventory.frmRequisitionPreview obj = new Reports.Inventory.frmRequisitionPreview();
            obj.branchid = cmbbranch.SelectedValue.ToString();
           
            obj.date = dateTimePicker1.Text.ToString();
            obj.Show();
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FoodDiscard_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            try
            {
                string q = "select * from Branch where status='Active'";
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

       
        
        
        private void Demand_Shown(object sender, EventArgs e)
        {


            type();
            
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
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

        private void vButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
