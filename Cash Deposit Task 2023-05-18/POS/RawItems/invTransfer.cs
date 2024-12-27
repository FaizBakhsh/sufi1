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
    public partial class invTransfer : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public invTransfer()
        {
            InitializeComponent();
        }
        protected void getdata()
        {
            string date = dateTimePicker1.Text;
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
            string q = "";
            if (textBox1.Text == "")
            {
                if (cmbcat.Text == "" || cmbcat.Text == "All")
                {
                    q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM         dbo.RawItem INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) ORDER BY dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM         dbo.RawItem INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.CategoryId='" + cmbcat.SelectedValue + "' and (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) ORDER BY dbo.RawItem.ItemName";
                }
            }
            else
            {

                string filter = cmbfilter.Text;

                if (cmbcat.Text == "" || cmbcat.Text == "All")
                {
                    if (filter == "Start With")
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and dbo.RawItem.itemname like '" + textBox1.Text + "%'  ORDER BY dbo.RawItem.ItemName";

                    }
                    else if (filter == "End With")
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and  dbo.RawItem.itemname like '%" + textBox1.Text + "'  ORDER BY dbo.RawItem.ItemName";

                    }
                    else if (filter == "Anywhere")
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and  dbo.RawItem.itemname like '%" + textBox1.Text + "%'  ORDER BY dbo.RawItem.ItemName";

                    }
                    else
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and  dbo.RawItem.itemname like '%" + textBox1.Text + "%'  ORDER BY dbo.RawItem.ItemName";

                    }
                }
                else
                {
                    if (filter == "Start With")
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where  dbo.RawItem.CategoryId='" + cmbcat.SelectedValue + "' and (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and dbo.RawItem.itemname like '" + textBox1.Text + "%'  ORDER BY dbo.RawItem.ItemName";

                    }
                    else if (filter == "End With")
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where  dbo.RawItem.CategoryId='" + cmbcat.SelectedValue + "' and  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and  dbo.RawItem.itemname like '%" + textBox1.Text + "'  ORDER BY dbo.RawItem.ItemName";

                    }
                    else if (filter == "Anywhere")
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where   dbo.RawItem.CategoryId='" + cmbcat.SelectedValue + "' and (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and  dbo.RawItem.itemname like '%" + textBox1.Text + "%'  ORDER BY dbo.RawItem.ItemName";

                    }
                    else
                    {
                        q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS itemname, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id  where  dbo.RawItem.CategoryId='" + cmbcat.SelectedValue + "' and  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL) and  dbo.RawItem.itemname like '%" + textBox1.Text + "%'  ORDER BY dbo.RawItem.ItemName";

                    }
                }
            }
            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
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
                string remarks = "", invoiceno = "";
                dspurchase = new DataSet();
                q = "SELECT     (TransferIn) AS tin,(TransferOut) AS tout,Remarks,price,status,invoiceno FROM     InventoryTransfer where Date ='" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + comboBox1.SelectedValue + "'  and sourcebranchid='" + comboBox2.SelectedValue + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
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

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), tin, price, Math.Round(tin * price, 2), remarks, status, ds1.Tables[0].Rows[i]["minorder"].ToString(), ds1.Tables[0].Rows[i]["maxorder"].ToString(), invoiceno);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            //dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[6].ReadOnly = true;
            dataGridView1.Columns[7].ReadOnly = true;
            dataGridView1.Columns[8].ReadOnly = true;
            foreach (DataGridViewRow dgr in dataGridView1.Rows)
            {

                if (dgr.Cells["Status"].Value.ToString() == "Posted")
                {
                    dgr.ReadOnly = true;
                }
            }
        }
        string demanddate = "", demandbranchid = "";
        public void getdata2(string brid, string date)
        {
            demanddate = date;
            demandbranchid = brid;
            try
            {
                comboBox1.SelectedValue = brid;
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

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), tin, ds1.Tables[0].Rows[i]["price"].ToString(), Math.Round(tin * price, 2), remarks, "", ds1.Tables[0].Rows[i]["minorder"].ToString(), ds1.Tables[0].Rows[i]["maxorder"].ToString(), "");
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
                if (e.ColumnIndex == 2 || e.ColumnIndex == 3)
                {
                    string qty = dataGridView1.Rows[e.RowIndex].Cells["Transfer In"].Value.ToString();
                    if (qty == "")
                    {
                        qty = "0";
                    }
                    string price = dataGridView1.Rows[e.RowIndex].Cells["price"].Value.ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    dataGridView1.Rows[e.RowIndex].Cells["Total"].Value = (Convert.ToDouble(price) * Convert.ToDouble(qty)).ToString();

                }
            }
            catch (Exception ex)
            {


            }
        }

        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == comboBox2.SelectedValue)
            {
                MessageBox.Show("Both Branches can not be same");
                return;
            }
            bool chk = false;
            string branchcode = "";
            string q = "select * from branch where id=" + comboBox1.SelectedValue;
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
                string barcode = branchcode + "-" + dr.Cells["id"].Value.ToString() + comboBox1.SelectedValue.ToString() + Convert.ToDateTime(dateTimePicker1.Text).ToString("dd") + Convert.ToDateTime(dateTimePicker1.Text).ToString("MM") + Convert.ToDateTime(dateTimePicker1.Text).ToString("yy");
                barcode = "*" + barcode + "*";
                {
                    DataSet dss = new DataSet();

                    dss = new DataSet();
                    q = "select * from InventoryTransfer where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid='" + comboBox2.SelectedValue + "'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        q = "update InventoryTransfer set DemandDate='" + demanddate + "',barcode='" + barcode + "',status='Pending', uploadstatus='Pending',InvoiceNo='" + dr.Cells["InvoiceNo"].Value.ToString().Replace("&", "n") + "',Remarks='" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "',TransferIn='" + Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2).ToString() + "',price ='" + Math.Round(Convert.ToDouble(dr.Cells["price"].Value.ToString()), 2).ToString() + "' ,total ='" + Math.Round(Convert.ToDouble(dr.Cells["total"].Value.ToString()), 2).ToString() + "'  where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid='" + comboBox2.SelectedValue + "'";
                        objcore.executeQuery(q);
                        chk1 = true;

                    }
                    else
                    {
                        if (Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2) > 0)
                        {
                            q = "insert into InventoryTransfer (DemandDate,barcode,InvoiceNo,status,Remarks,Date, Itemid, TransferIn, price, branchid,sourcebranchid,total) values('" + demanddate + "','" + barcode + "','" + dr.Cells["InvoiceNo"].Value.ToString().Replace("&", "n") + "','Pending','" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "','" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Transfer In"].Value.ToString()), 2).ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["price"].Value.ToString()), 2).ToString() + "','" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue + "','" + Math.Round(Convert.ToDouble(dr.Cells["total"].Value.ToString()), 2).ToString() + "')";
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
            obj.branchid = comboBox1.SelectedValue.ToString();
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

                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "BranchName";



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
                string q = "SELECT        dbo.Rights.Status, dbo.Forms.Forms, dbo.Rights.Userid FROM            dbo.Rights INNER JOIN                         dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where dbo.Forms.Forms='Demand Approval' and userid='" + POSRestaurant.Properties.Settings.Default.UserId + "'";
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
            DialogResult dr = MessageBox.Show("Are you sure to Post", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string q = "select * from InventoryTransfer where status='Pending' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid is not null";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    MessageBox.Show("Please Approve Demand First");
                    return;
                }
                try
                {
                    q = "select * from InventoryTransfer where  date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid is not null";
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
                q = "select * from InventoryTransfer where status='Approved' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid is not null";
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    q = "update InventoryTransfer set status='Posted' where date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid is not null";
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
        public void accounts(string date)
        {
            try
            {
                branchid = comboBox2.SelectedValue.ToString();
                double gross = 0, gst = 0, discount = 0, net = 0, cash = 0, credit = 0, master = 0, service = 0;
                string q = "";
                q = "delete from SalesAccount where VoucherNo='SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "'";
                objcore.executeQuery(q);
                q = "delete from DiscountAccount where VoucherNo='SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "'";
                objcore.executeQuery(q);

                q = "delete from GSTAccount where VoucherNo='SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "'";
                objcore.executeQuery(q);


                q = "delete from CostSalesAccount where VoucherNo='SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "'";
                objcore.executeQuery(q);
                q = "delete from InventoryAccount where VoucherNo='SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "'";
                objcore.executeQuery(q);
                q = "delete from BranchAccount where VoucherNo='SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "'";
                objcore.executeQuery(q);
                DataSet ds = new DataSet();



                inventoryaccount(date);

                q = "SELECT   SUM(isnull(total,'0')) AS total, SUM(isnull(total,'0')) AS netsale, SUM(isnull(GSTAmount,'0')) AS gst,SUM(isnull(DiscountAmount,'0')) AS discount  FROM         InventoryTransfer where  sourcebranchid is not null and branchid='" + comboBox1.SelectedValue + "' and date='" + date + "' and status='Posted'";
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    gross = Convert.ToDouble(ds.Tables[0].Rows[0]["total"].ToString());
                    gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                    discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                    net = Convert.ToDouble(ds.Tables[0].Rows[0]["netsale"].ToString());
                    net = net - discount;
                    //gross = gross + gst;
                    saleaccount(gross.ToString(), "SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue, date);
                    if (discount > 0)
                    {
                        discountaccount(discount.ToString(), "SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue, date);
                    }
                    if (gst > 0)
                    {
                        gstaccount(gst.ToString(), "SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue, date);
                    }

                    employeeaccount(net.ToString(), "SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue, "Cash", date, comboBox1.SelectedValue.ToString());

                    ExecuteSqlTransaction("", q1, q2, q3, q4, q5, q6, "");
                }
            }
            catch (Exception ex)
            {

            }



        }
        private static void ExecuteSqlTransaction(string connectionString, string q1, string q2, string q3, string q4, string q5, string q6, string message)
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
            string dat = "";
            q = "SELECT   top 1  price,date FROM     InventoryTransfer where Date <='" + date + "' and ItemId='" + id + "' and branchid='" + comboBox1.SelectedValue + "'  and sourcebranchid='" + comboBox2.SelectedValue + "' order by date desc";
            dspurchase = objCore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = Convert.ToDouble(val);
                try
                {
                    dat = Convert.ToDateTime(dspurchase.Tables[0].Rows[0][1].ToString()).ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {
                    
                }
            }

            dspurchase = new DataSet();

            try
            {
                q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date > '" + dat + "') and RawItemId = '" + id + "' order by dbo.Purchase.id desc";

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
            catch (Exception ex)
            {
               
            }


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
                    q = "SELECT   top 1  price FROM     InventoryTransfer where Date <='" + date + "' and ItemId='" + id + "' and branchid='" + comboBox1.SelectedValue + "'  and sourcebranchid='" + comboBox2.SelectedValue + "' order by date desc";
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
            price = Math.Round(price, 3);
            return price;
        }

        public void inventoryaccount(string date)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";

                {
                    string ChartAccountId = "";

                    double prc = 0;
                    string val = "";
                    q = "select * from InventoryTransfer where sourcebranchid is not null and date='" + date + "' and status='Posted' and branchid='" + comboBox1.SelectedValue + "'";
                    DataSet dsinv = new DataSet();
                    dsinv = objcore.funGetDataSet(q);
                    for (int i = 0; i < dsinv.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string itemid = dsinv.Tables[0].Rows[i]["Itemid"].ToString();
                            string temp = dsinv.Tables[0].Rows[i]["TransferIn"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            double qty = Convert.ToDouble(temp);
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
                                qty = qty / rate;
                            }
                            qty = Math.Round(qty, 3);
                            prc = prc + (qty * getprice(date, itemid));
                        }
                        catch (Exception ex)
                        {

                        }
                    }


                    dsacount = new DataSet();


                    q = "select * from CashSalesAccountsList where AccountType='Inventory Account'  and branchid='" + branchid + "'";
                    DataSet dsacountchk = new DataSet();
                    dsacountchk = objCore.funGetDataSet(q);
                    if (dsacountchk.Tables[0].Rows.Count > 0)
                    {
                        ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    int iddd = 0;
                    DataSet ds = objCore.funGetDataSet("select max(id) as id from InventoryAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    val = "";

                    q = "insert into InventoryAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + date + "','" + ChartAccountId + "','SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "','Inventory Transferred to Branch " + comboBox1.Text + "','0','" + Math.Round(Convert.ToDouble(prc), 2) + "','0','" + branchid + "')";
                    q1 = q;
                    // objCore.executeQuery(q);
                    ChartAccountId = "";
                    q = "select * from CashSalesAccountsList where AccountType='Cost of Sales Account'  and branchid='" + branchid + "'";
                    dsacountchk = new DataSet();
                    dsacountchk = objCore.funGetDataSet(q);
                    if (dsacountchk.Tables[0].Rows.Count > 0)
                    {
                        ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CostSalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }


                    q = "insert into CostSalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + date + "','" + ChartAccountId + "','SJVI-" + branchid + "-" + date + "-" + comboBox1.SelectedValue + "','Cost against Inventory Transferred to Branch " + comboBox1.Text + "','" + Math.Round(Convert.ToDouble(prc), 2) + "','0','0','" + branchid + "')";
                    q2 = q;

                    //  objCore.executeQuery(q);

                }
            }
            catch (Exception ex)
            {


            }
        }

        public void employeeaccount(string amount, string saleid, string type, string date, string ID)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();

                string q = "select * from Branch where ID='" + ID + "'";

                dsacount = objCore.funGetDataSet(q);
                int id = 0;
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from BranchAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";


                    double newbalance = (balance + Convert.ToDouble(amount));

                    q = "insert into BranchAccount (branchid,Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ID + "','" + ChartAccountId + "','" + saleid + "','Inventory Transferred to Branch " + comboBox1.Text + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    q3 = q;
                    //objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void saleaccount(string amount, string saleid, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='Sales Account'  and branchid='" + branchid + "'";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from SalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from SalesAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into SalesAccount (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','Inventory Transferred to Branch " + comboBox1.Text + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    q4 = q;
                    //objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void gstaccount(string amount, string saleid, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='GST Account' and branchid='" + branchid + "'";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from GSTAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from GSTAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into GSTAccount (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','Inventory Transferred to Branch " + comboBox1.Text + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    q5 = q;

                    //objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }

        public void discountaccount(string amount, string saleid, string date)
        {
            try
            {
                string vall = amount;
                if (vall == "")
                {
                    vall = "0";
                }
                DataSet ds = new DataSet();
                if (Convert.ToDouble(vall) > 0)
                {
                    DataSet dsacount = new DataSet();
                    string q = "select * from CashSalesAccountsList where AccountType='Discount Account' and branchid='" + branchid + "'";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                        int iddd = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from DiscountAccount");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string i = ds.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "1";
                            }
                            iddd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            iddd = Convert.ToInt32("1");
                        }
                        double balance = 0;
                        string val = "";
                        q = "select top 1 * from DiscountAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                        dsacount = new DataSet();
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        balance = Convert.ToDouble(val);
                        double newbalance = (balance + Convert.ToDouble(amount));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into DiscountAccount (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','Inventory Transferred to Branch " + comboBox1.Text + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                        q6 = q;

                        //objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        DataTable dt;

        private void vButton5_Click(object sender, EventArgs e)
        {
            DemandList2 obj = new DemandList2(this);
            obj.Show();
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
            string q = "update InventoryTransfer set status='Pending' where branchid='" + comboBox1.SelectedValue + "' and date='" + dateTimePicker1.Text + "'";
            objcore.executeQuery(q);
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmDeliveryChallanInvTransfer obj = new Reports.Inventory.frmDeliveryChallanInvTransfer();
            obj.branchid = comboBox1.SelectedValue.ToString();
            obj.sourcebranchid = comboBox2.SelectedValue.ToString();
            obj.date = dateTimePicker1.Text.ToString();
            obj.Show();
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.StockIssuanceInvoice obj = new Reports.Inventory.StockIssuanceInvoice();
            obj.branchid = comboBox1.SelectedValue.ToString();
            obj.sourcebranchid = comboBox2.SelectedValue.ToString();
            obj.date = dateTimePicker1.Text.ToString();
            obj.type = "royality";
            obj.Show();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
            {
               // getdata();
            }
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            //POSRestaurant.Reports.Inventory.FrmBarcodes obj = new Reports.Inventory.FrmBarcodes();
            POSRestaurant.RawItems.Barcodes obj = new Barcodes();
            obj.date = dateTimePicker1.Text;
            obj.branchid = comboBox1.SelectedValue.ToString();
            obj.Show();
        }

        private void btnapprove_Click(object sender, EventArgs e)
        {
            string q = "update InventoryTransfer set  status='Approved' where date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "'  and sourcebranchid is not null";
            int res = objcore.executeQueryint(q);
            if (res > 0)
            {
                MessageBox.Show("Demand Approved Successfully");
            }
            else
            {
                MessageBox.Show("Something went wrong. Please try again");
            }
            
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
