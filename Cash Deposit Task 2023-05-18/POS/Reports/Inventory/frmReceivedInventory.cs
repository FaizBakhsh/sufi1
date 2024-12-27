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
    public partial class frmReceivedInventory : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmReceivedInventory()
        {
            InitializeComponent();
        }
        public string purchaseid = "";
        public void fill()
        {
            try
            {
                cmbstore.DataSource = null;
                cmbstore.Items.Clear();

                if (cmbbranch.Text == "All")
                {
                    cmbstore.Items.Add("All");
                    cmbstore.Text = "All";
                }
                else
                {
                    DataSet dsfill = new DataSet();
                    string q = "select * from Stores where BranchId='" + cmbbranch.SelectedValue + "'";
                    dsfill = objCore.funGetDataSet(q);
                    DataRow dr = dsfill.Tables[0].NewRow();
                    dr["storename"] = "All";
                    cmbstore.DataSource = dsfill.Tables[0];
                    cmbstore.ValueMember = "id";
                    cmbstore.DisplayMember = "storename";
                    cmbstore.Text = "All";
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
                    cmbcategory.DataSource = ds.Tables[0];
                    cmbcategory.ValueMember = "id";
                    cmbcategory.DisplayMember = "CategoryName";
                    cmbcategory.Text = "All";
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
                ds.Tables[0].Rows.Add(dr);
                cmbbranch.DataSource = ds.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";
                cmbbranch.Text = "All";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                DataSet dsq = new DataSet();
                string q = "select * from Supplier";
                dsq = objCore.funGetDataSet(q);
                DataRow drq = dsq.Tables[0].NewRow();
                drq["name"] = "All";
                dsq.Tables[0].Rows.Add(drq);
                comboBox1.DataSource = dsq.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                comboBox1.Text = "All";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            getitems();
            if (purchaseid.Length > 0)
            {
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
                cmbbranch.Visible = false;
                cmbstore.Visible = false;
                comboBox1.Visible = false;
                cmbitem.Visible = false;
                checkBox1.Visible = false;
                checkBox2.Visible = false;
                vButton1.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;
                label6.Visible = false;
                bindreport();
            }
        }
        protected void getitems()
        {
            try
            {
                DataSet dsq = new DataSet();
                string q = "select * from RawItem";
                dsq = objCore.funGetDataSet(q);
                DataRow drq = dsq.Tables[0].NewRow();
                drq["ItemName"] = "All";
                dsq.Tables[0].Rows.Add(drq);
                cmbitem.DataSource = dsq.Tables[0];
                cmbitem.ValueMember = "id";
                cmbitem.DisplayMember = "ItemName";
                cmbitem.Text = "All";


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
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.Inventory.rptReceivedInventory rptDoc = new rptReceivedInventory();
                POSRestaurant.Reports.Inventory.dsin dsrpt = new dsin();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();

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
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }

                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                if (comboBox1.Text == "All")
                {
                    rptDoc.SetParameterValue("sup", "of All Suppliers");
                }
                else
                {
                    rptDoc.SetParameterValue("sup", "of " + comboBox1.Text);
                }
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("price", typeof(double));
                dtrpt.Columns.Add("Invoice", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }

                DataSet ds = new DataSet();
                string q = "";
                if (purchaseid.Length > 0)
                {
                    q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, (dbo.PurchaseDetails.TotalItems) AS quantity, (dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.id= '" + purchaseid + "')  ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";

                }
                else
                {
                    if (cmbcategory.Text == "All")
                    {
                        if (txtinvoice.Text.Trim().Length == 0)
                        {
                            if (checkBox2.Checked == false)
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (checkBox2.Checked == false)
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        if (txtinvoice.Text.Trim().Length == 0)
                        {
                            if (checkBox2.Checked == false)
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='"+cmbcategory.SelectedValue+"' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')   and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'   AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'   AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'   AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (checkBox2.Checked == false)
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%' and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%' and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%' and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "' and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%' and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (checkBox1.Checked == true)
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%' and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%' and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date, dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.Purchase.Date,dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbitem.Text == "All")
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (cmbbranch.Text == "All")
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id   WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "' and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'   and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                        else
                                        {
                                            if (comboBox1.Text == "All")
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "')  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                            else
                                            {
                                                q = "SELECT        TOP (100) PERCENT dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, SUM(dbo.PurchaseDetails.TotalItems) AS quantity, SUM(dbo.PurchaseDetails.TotalAmount) AS Amount, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                         dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id  WHERE     (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Purchase.BranchCode = '" + cmbbranch.SelectedValue + "') and dbo.Purchase.SupplierId='" + comboBox1.SelectedValue + "'  and dbo.PurchaseDetails.RawItemId='" + cmbitem.SelectedValue + "'  and dbo.Purchase.invoiceno like '%" + txtinvoice.Text + "%' or dbo.Purchase.id like '%" + txtinvoice.Text + "%'  and dbo.RawItem.CategoryId='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Purchase.Date,dbo.Purchase.id,dbo.Purchase.invoiceno ORDER BY dbo.RawItem.ItemName";
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string date = "",invoice="";
                    try
                    {
                        date = ds.Tables[0].Rows[i]["Date"].ToString();
                    }
                    catch (Exception ex)
                    {
                        date = dateTimePicker2.Text;
                    }
                    try
                    {
                        invoice = ds.Tables[0].Rows[i]["id"].ToString();
                    }
                    catch (Exception ex)
                    {
                        invoice = "";
                    }
                    try
                    {
                        if (ds.Tables[0].Rows[i]["invoiceno"].ToString().Length > 0)
                        {
                            invoice = invoice + "-(" + ds.Tables[0].Rows[i]["invoiceno"].ToString() + ")";
                        }
                    }
                    catch (Exception ex)
                    {
                       
                    }
                    double price = 0;
                    try
                    {
                        string temp = ds.Tables[0].Rows[i]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double qty = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[i]["Amount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double Amount = Convert.ToDouble(temp);
                        price = Amount / qty;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(date, ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["Amount"].ToString(), price, invoice, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(date, ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["Amount"].ToString(),price, invoice, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void cmbcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            getitems();
        }
    }
}
