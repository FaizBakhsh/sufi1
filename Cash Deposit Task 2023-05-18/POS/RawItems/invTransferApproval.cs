using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class invTransferApproval : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public invTransferApproval()
        {
            InitializeComponent();
        }
        string sourceid = "";
        protected void getdata()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0,tin=0,tout=0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("UOM", typeof(string));
            ds.Columns.Add("Quantity", typeof(string));         
            ds.Columns.Add("Price", typeof(string));
            ds.Columns.Add("Total", typeof(string));
            ds.Columns.Add("Status", typeof(string));
            ds.Columns.Add("Date1", typeof(string));
            ds.Columns.Add("Quantity1", typeof(string));
            ds.Columns.Add("Amount1", typeof(string));
            ds.Columns.Add("Date2", typeof(string));
            ds.Columns.Add("Quantity2", typeof(string));
            ds.Columns.Add("Amount2", typeof(string));
            ds.Columns.Add("Date3", typeof(string));
            ds.Columns.Add("Quantity3", typeof(string));
            ds.Columns.Add("Amount3", typeof(string));
            
            string q = "";
            q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM         dbo.RawItem INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where branchid='"+cmbbranch.SelectedValue+"' and ORDER BY dbo.RawItem.ItemName";
            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName ,dbo.UOM.UOM, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.InventoryTransfer ON dbo.RawItem.Id = dbo.InventoryTransfer.Itemid  where dbo.InventoryTransfer.date='" + dateTimePicker1.Text + "' and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "' and dbo.InventoryTransfer.TransferIn>0 and dbo.InventoryTransfer.sourcebranchid is not null ORDER BY dbo.RawItem.ItemName";
            
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                price = 0;
                tin = 0;
                string val = "", status = "";
                double rem = 0;
                val = ds1.Tables[0].Rows[i]["price"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = getprice(ds1.Tables[0].Rows[i]["id"].ToString(), dateTimePicker1.Text); ;
                DataSet dspurchase = new DataSet();
                string remarks = "", invoiceno = "",approveuserid="";
                dspurchase = new DataSet();
                q = "SELECT     (TransferIn) AS tin,(TransferOut) AS tout,Remarks,price,status,invoiceno,sourcebranchid,Approveduserid FROM     InventoryTransfer where TransferIn>0 and Date ='" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and sourcebranchid is not null and branchid='" + cmbbranch.SelectedValue + "' ";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    approveuserid = dspurchase.Tables[0].Rows[0]["Approveduserid"].ToString();
                    sourceid = dspurchase.Tables[0].Rows[0]["sourcebranchid"].ToString();
                    invoiceno = dspurchase.Tables[0].Rows[0]["invoiceno"].ToString();
                    remarks = dspurchase.Tables[0].Rows[0]["remarks"].ToString();
                    status = dspurchase.Tables[0].Rows[0]["status"].ToString();
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tin = Convert.ToDouble(val);
                    val = dspurchase.Tables[0].Rows[0][1].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = Convert.ToDouble(val);

                    val = dspurchase.Tables[0].Rows[0]["price"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    if (val != "0")
                    {
                        price = Convert.ToDouble(val);
                    }

                }
                double qty1 = 0, qty2 = 0, qty3 = 0, price3 = 0, price1 = 0, price2 = 0;
                string date1 = "", date2 = "", date3 = "";
                try
                {
                    q = "SELECT  top 3   (TransferIn) AS tin,(TransferOut) AS tout,Remarks,price,status,date FROM     InventoryTransfer where TransferIn>0 and  Date <'" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'  and sourcebranchid is not null and branchid='" + cmbbranch.SelectedValue + "' order by date desc";
                  DataSet  dspurchase1 = new DataSet();
                  dspurchase1 = objcore.funGetDataSet(q);

                  for (int ii = 0; ii < dspurchase1.Tables[0].Rows.Count; ii++)
                    {
                        if (ii == 0)
                        {
                            date1 = Convert.ToDateTime(dspurchase1.Tables[0].Rows[ii]["date"].ToString()).ToString("dd-MMM-yyyy");
                            val = dspurchase1.Tables[0].Rows[ii]["tin"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            qty1 = Convert.ToDouble(val);

                            val = dspurchase1.Tables[0].Rows[ii]["price"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            if (val != "0")
                            {
                                price1 = Convert.ToDouble(val);
                            }


                        }
                        if (ii == 1)
                        {
                            date2 = Convert.ToDateTime(dspurchase1.Tables[0].Rows[ii]["date"].ToString()).ToString("dd-MMM-yyyy");
                            val = dspurchase1.Tables[0].Rows[ii]["tin"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            qty2 = Convert.ToDouble(val);

                            val = dspurchase1.Tables[0].Rows[ii]["price"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            if (val != "0")
                            {
                                price2 = Convert.ToDouble(val);
                            }


                        }
                        if (ii == 2)
                        {
                            date3 = Convert.ToDateTime(dspurchase1.Tables[0].Rows[ii]["date"].ToString()).ToString("dd-MMM-yyyy");
                            val = dspurchase1.Tables[0].Rows[ii]["tin"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            qty3 = Convert.ToDouble(val);

                            val = dspurchase1.Tables[0].Rows[ii]["price"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            if (val != "0")
                            {
                                price3 = Convert.ToDouble(val);
                            }


                        }
                    }
                }
                catch (Exception ex)
                {

                }
                string status1 = "Pending";
                if (approveuserid.Length > 0)
                {
                    status1 = "Approved";
                }
                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), ds1.Tables[0].Rows[i]["UOM"].ToString(), tin, price, Math.Round(tin * price, 2),status1, date1, qty1, Math.Round(price1 * qty1, 2), date2, qty2, Math.Round(price2 * qty2, 2), date3, qty3, Math.Round(price3 * qty3, 2));
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            //dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[3].ReadOnly = false;
            dataGridView1.Columns[4].ReadOnly = true;

            
           
            dataGridView1.Columns[7].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[7].DefaultCellStyle.ForeColor = Color.Black;

            dataGridView1.Columns[8].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[8].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[9].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[9].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[10].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[10].DefaultCellStyle.ForeColor = Color.Black;

            dataGridView1.Columns[11].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[11].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[12].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[12].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[13].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[13].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[14].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[14].DefaultCellStyle.ForeColor = Color.Black;
            dataGridView1.Columns[15].DefaultCellStyle.BackColor = Color.Yellow;
            dataGridView1.Columns[15].DefaultCellStyle.ForeColor = Color.Black;

            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {

                if (dgr.Cells["Status"].Value.ToString() == "Approved")
                {
                    dataGridView1.Columns[1].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView1.Columns[1].DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns[2].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView1.Columns[2].DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns[3].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView1.Columns[3].DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns[4].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView1.Columns[4].DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns[5].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView1.Columns[5].DefaultCellStyle.ForeColor = Color.White;
                    dataGridView1.Columns[6].DefaultCellStyle.BackColor = Color.Green;
                    dataGridView1.Columns[6].DefaultCellStyle.ForeColor = Color.White;

                }
            }
        }
        string demanddate = "", demandbranchid = "";
        public void getdata2(string brid,string date)
        {
            demanddate = date;
            demandbranchid = brid;
            try
            {
                cmbbranch.SelectedValue = brid;
            }
            catch (Exception ex)
            {
                
            }
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, tin = 0, tout = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("Transfer In", typeof(string));
            ds.Columns.Add("Price", typeof(string));
            ds.Columns.Add("Total", typeof(string));
            ds.Columns.Add("Remarks", typeof(string));
            ds.Columns.Add("Status", typeof(string));
            ds.Columns.Add("Min", typeof(string));
            ds.Columns.Add("Max", typeof(string));
            ds.Columns.Add("InvoiceNo", typeof(string));
            dateTimePicker1.Text = date;
            string q = "";
            q = "SELECT        dbo.RawItem.id,dbo.RawItem.minorder,dbo.RawItem.maxorder, dbo.Demand.Itemid, dbo.Demand.Date, dbo.Demand.Quantity, dbo.Demand.Status, dbo.Demand.branchid, dbo.RawItem.ItemName, dbo.RawItem.Price, ROUND(dbo.Demand.Quantity * dbo.RawItem.Price, 2)                          AS Total FROM            dbo.Demand INNER JOIN                         dbo.RawItem ON dbo.Demand.Itemid = dbo.RawItem.Id where dbo.Demand.date='" + date + "' and dbo.Demand.branchid= '" + brid + "'   and dbo.Demand.status='pending'";
               
            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                price = 0;
                tin = 0;
                string val = "";
                double rem = 0;
                val = ds1.Tables[0].Rows[i]["price"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = Convert.ToDouble(val);
               
                string remarks = "";

                val = ds1.Tables[0].Rows[i]["Quantity"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                tin = Convert.ToDouble(val);

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), tin, ds1.Tables[0].Rows[i]["price"].ToString(), Math.Round(tin * price, 2), remarks, "", ds1.Tables[0].Rows[i]["minorder"].ToString(), ds1.Tables[0].Rows[i]["maxorder"].ToString(),"");
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            //dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].ReadOnly = true;
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

            try
            {
                if (e.ColumnIndex == 3)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString();
                    string qty = dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
                    if (qty == "")
                    {
                        qty = "0";
                    }
                    string price = dataGridView1.Rows[e.RowIndex].Cells["price"].Value.ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    dataGridView1.Rows[e.RowIndex].Cells["Total"].Value =Math.Round( (Convert.ToDouble(price) * Convert.ToDouble(qty)),3).ToString();
                    string q = "";
                    q = "update InventoryTransfer set TransferIn='" + qty + "',total='" + dataGridView1.Rows[e.RowIndex].Cells["Total"].Value + "'  where   Date ='" + dateTimePicker1.Text + "' and ItemId='" + id + "' and branchid='" + cmbbranch.SelectedValue + "' and sourcebranchid is not null";

                    objcore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            if (cmbbranch.SelectedValue == comboBox2.SelectedValue)
            {
                MessageBox.Show("Both Branches can not be same");
                return;
            }
            bool chk = false;
            string branchcode = "";
            string q = "select * from branch where id=" + cmbbranch.SelectedValue;
            DataSet dsbranch = new DataSet();
            dsbranch = objcore.funGetDataSet(q);
            if (dsbranch.Tables[0].Rows.Count > 0)
            {
                branchcode = dsbranch.Tables[0].Rows[0]["BranchCode"].ToString();
            }
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
              //  chk1 = false;
                Random rnd = new Random();
               // int code = rnd.Next(10, 99);
                string barcode =branchcode+"-"+  dr.Cells["id"].Value.ToString() + cmbbranch.SelectedValue.ToString() + Convert.ToDateTime(dateTimePicker1.Text).ToString("dd") + Convert.ToDateTime(dateTimePicker1.Text).ToString("MM") + Convert.ToDateTime(dateTimePicker1.Text).ToString("yy");
                barcode = "*" + barcode + "*";
                {
                    DataSet dss = new DataSet();
                    
                    dss = new DataSet();
                    q = "select * from InventoryTransfer where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and sourcebranchid='"+comboBox2.SelectedValue+"'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        q = "update InventoryTransfer set DemandDate='" + demanddate + "',barcode='" + barcode + "',status='Pending', uploadstatus='Pending',InvoiceNo='" + dr.Cells["InvoiceNo"].Value.ToString().Replace("&", "n") + "',Remarks='" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "',TransferIn='" + Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2).ToString() + "',price ='" + Math.Round(Convert.ToDouble(dr.Cells["price"].Value.ToString()), 2).ToString() + "' ,total ='" + Math.Round(Convert.ToDouble(dr.Cells["total"].Value.ToString()), 2).ToString() + "'  where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and sourcebranchid='" + comboBox2.SelectedValue + "'";
                        objcore.executeQuery(q);
                        chk1 = true;

                    }
                    else
                    {
                        if (Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2) > 0)
                        {
                            q = "insert into InventoryTransfer (DemandDate,barcode,InvoiceNo,status,Remarks,Date, Itemid, TransferIn, price, branchid,sourcebranchid,total) values('" + demanddate + "','" + barcode + "','" + dr.Cells["InvoiceNo"].Value.ToString().Replace("&", "n") + "','Pending','" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "','" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2).ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["price"].Value.ToString()), 2).ToString() + "','" + cmbbranch.SelectedValue + "','" + comboBox2.SelectedValue + "','" + Math.Round(Convert.ToDouble(dr.Cells["total"].Value.ToString()), 2).ToString() + "')";
                            objcore.executeQuery(q);
                            chk = true;
                        }
                    }
                    //updateremaininginventory(dr.Cells["id"].Value.ToString(),variance);
                    
                }
            }
            if (chk == true)
            {
               
                MessageBox.Show("Record Saved Successfully");
                //getdata();
            }
           
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.StockIssuanceInvoice obj = new Reports.Inventory.StockIssuanceInvoice();
            obj.branchid = cmbbranch.SelectedValue.ToString();
            obj.sourcebranchid = comboBox2.SelectedValue.ToString();
            obj.date = dateTimePicker1.Text.ToString();
            obj.Show();
        }
        public void fillitems()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch  where  status='active'";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];
               
                cmbbranch.DataSource = dt;
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "BranchName";
                


            }
            catch (Exception ex)
            {


            }


        }
        public void fillitems2()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch where branchname like '%head%'  and status='Active'  or  branchname like '%ware%' and status='Active' ";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];

                comboBox2.DataSource = dt;
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "BranchName";



            }
            catch (Exception ex)
            {


            }


        }
        private void Variance_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from Category";
                DataSet ds = objcore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["CategoryName"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbcat.DataSource = ds.Tables[0];

                cmbcat.ValueMember = "id";
                cmbcat.DisplayMember = "CategoryName";
                cmbcat.SelectedItem = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            fillitems();
            fillitems2();
            try
            {
                string q = "SELECT        dbo.Rights.Status, dbo.Forms.Forms, dbo.Rights.Userid FROM            dbo.Rights INNER JOIN                         dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where dbo.Forms.Forms='Issue Stock Approval' and userid='"+POSRestaurant.Properties.Settings.Default.UserId+"'";
                DataSet dss = new DataSet();
                dss = objcore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    string status = dss.Tables[0].Rows[0][0].ToString();
                    if (status == "yes")
                    {
                        btnapprove.Enabled = true;

                    }
                    else
                    {
                        btnapprove.Enabled = false;
                    }
                }
                else
                {
                    btnapprove.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show( "Are you sure to Post","", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string q = "select * from InventoryTransfer where status='Pending' and date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and sourcebranchid is not null";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    MessageBox.Show("Please Approve Demand First");
                    return;
                }
                try
                {
                    q = "select * from InventoryTransfer where  date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and sourcebranchid is not null";
                    ds = new DataSet();
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        demanddate = ds.Tables[0].Rows[0]["demanddate"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    
                }
                q = "select * from InventoryTransfer where status='Approved' and date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and sourcebranchid is not null";
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    q = "update InventoryTransfer set status='Posted' where date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and sourcebranchid is not null";
                    objcore.executeQuery(q);

                    q1 = "update demand set status='Processed' where date='" + demanddate + "' and branchid='" + demandbranchid + "'";
                    objcore.executeQuery(q1);


                    MessageBox.Show("Posted successfully");
                }
                else
                {
                    MessageBox.Show("Nothing to Post");
                }
               // accounts(dateTimePicker1.Text);
               
            }
        }
        string branchid = "";
        string q1 = "", q2 = "", q3 = "", q4 = "", q5 = "", q6 = "";
     
        private static void ExecuteSqlTransaction(string connectionString, string q1, string q2,string q3,string q4,string q5,string q6, string message)
        {
            connectionString = POSRestaurant.Properties.Settings.Default.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    if (q1.Length > 0)
                    {
                        command.CommandText = q1;

                        command.ExecuteNonQuery();
                    }
                    if (q2.Length > 0)
                    {
                        command.CommandText = q2;
                        command.ExecuteNonQuery();
                    }
                    if (q3.Length > 0)
                    {
                        command.CommandText = q3;
                        command.ExecuteNonQuery();
                    }
                    if (q4.Length > 0)
                    {
                        command.CommandText = q4;
                        command.ExecuteNonQuery();
                    }
                    if (q5.Length > 0)
                    {
                        command.CommandText = q5;
                        command.ExecuteNonQuery();
                    }
                    if (q6.Length > 0)
                    {
                        command.CommandText = q6;
                        command.ExecuteNonQuery();
                    }
                    // Attempt to commit the transaction.
                    transaction.Commit();
                    //MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("  Message: {0}" + ex.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Rollback Exception Type: {0}" + ex2.Message);
                    }
                }
            }
        }
        private double getprice(string id, string date)
        {
            if (id == "69")
            {

            }
            double variance = 0, price = 0;
            string val = "";
            DataSet dspurchase = new DataSet();
            string q = "";

            //q = "SELECT   top 1  price FROM     InventoryTransfer where Date <='" + date + "' and ItemId='" + id + "' and branchid='" + comboBox1.SelectedValue + "'  and sourcebranchid='" + comboBox2.SelectedValue + "' order by date desc";
            //dspurchase = objCore.funGetDataSet(q);
            //if (dspurchase.Tables[0].Rows.Count > 0)
            //{
            //    val = dspurchase.Tables[0].Rows[0][0].ToString();
            //    if (val == "")
            //    {
            //        val = "0";
            //    }
            //    price = Convert.ToDouble(val);
            //}
            
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + date + "' and '" + date + "') and RawItemId = '" + id + "'";

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

                    q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date <= '" + date + "') and RawItemId = '" + id + "' order by dbo.Purchase.date desc";

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
       
        
        DataTable dt;
       
        private void vButton5_Click(object sender, EventArgs e)
        {
            
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
            string q = "update InventoryTransfer set status='Pending' where branchid='"+cmbbranch.SelectedValue+"' and date='"+dateTimePicker1.Text+"'";
            objcore.executeQuery(q);
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            //string q = "update InventoryTransfer set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "' where date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "'  and sourcebranchid is not null";
            //int res = objcore.executeQueryint(q);
           
            POSRestaurant.Reports.Inventory.frmDeliveryChallanInvTransfer obj= new Reports.Inventory.frmDeliveryChallanInvTransfer();
            obj.branchid = cmbbranch.SelectedValue.ToString();
            obj.sourcebranchid = "";
            obj.date = dateTimePicker1.Text.ToString();
            obj.Show();
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.StockIssuanceInvoice obj = new Reports.Inventory.StockIssuanceInvoice();
            obj.branchid = cmbbranch.SelectedValue.ToString();
            obj.sourcebranchid = comboBox2.SelectedValue.ToString();
            obj.date = dateTimePicker1.Text.ToString();
            obj.type = "royality";
            obj.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
                getdata();
            }
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            //POSRestaurant.Reports.Inventory.FrmBarcodes obj = new Reports.Inventory.FrmBarcodes();
            POSRestaurant.Reports.Inventory.FrmBarcodePrint obj = new Reports.Inventory.FrmBarcodePrint();
            obj.date = dateTimePicker1.Text;
            obj.branchid = cmbbranch.SelectedValue.ToString();
            obj.Show();
        }

        private void btnapprove_Click(object sender, EventArgs e)
        {
            string q = "update InventoryTransfer set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "' where date='" + dateTimePicker1.Text + "' and branchid='" + cmbbranch.SelectedValue + "'  and sourcebranchid is not null";
          int res=  objcore.executeQueryint(q);
          if (res > 0)
          {
              MessageBox.Show("Demand Approved Successfully");
          }
          else
          {
              MessageBox.Show("Something went wrong. Please try again");
          }
          getdata();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
