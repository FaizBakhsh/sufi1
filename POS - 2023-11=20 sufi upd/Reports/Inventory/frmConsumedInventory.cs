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
    public partial class frmConsumedInventory : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmConsumedInventory()
        {
            InitializeComponent();
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
                cmbbranch.Text = "All";


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
            if (reference == "pnl")
            {
                dateTimePicker1.Text = start;
                dateTimePicker2.Text = end;
                if (branchid == "All")
                {
                    cmbbranch.SelectedItem = branchid;
                }
                else
                {
                    cmbbranch.SelectedValue = branchid;
                }
               
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterParent;
                bindreport();
            }
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public string start = "", end = "", branchid = "", reference = "";
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.Inventory.rptComsumedinventory rptDoc = new rptComsumedinventory();
                POSRestaurant.Reports.Inventory.DsInventoryCosumption dsrpt = new DsInventoryCosumption();
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
                rptDoc.SetParameterValue("date", "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);


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
                dtrpt.Columns.Add("ItemCode", typeof(string));
                dtrpt.Columns.Add("ItemDescription", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Consumed", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("price", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("purchased", typeof(double));
                dtrpt.Columns.Add("Type", typeof(string));
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

                if (cmbgroupraw.Text == "All")
                {
                    if (cmbkitchen.Text == "All")
                    {
                        if (textBox1.Text.Trim() == "")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and   (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  and dbo.RawItem.ItemName like '%" + textBox1.Text + "%'  GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                    }
                    else
                    {
                        if (textBox1.Text.Trim() == "")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and (dbo.InventoryConsumed.QuantityConsumed > 0) GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and (dbo.InventoryConsumed.QuantityConsumed > 0) GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and   (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' and (dbo.InventoryConsumed.QuantityConsumed > 0) GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' and (dbo.InventoryConsumed.QuantityConsumed > 0)  GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                    }
                }
                else
                {
                    if (cmbkitchen.Text == "All")
                    {
                        if (textBox1.Text.Trim() == "")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and   dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and  (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and   dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and     (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and   dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  and dbo.RawItem.ItemName like '%" + textBox1.Text + "%'  GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                    }
                    else
                    {
                        if (textBox1.Text.Trim() == "")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE   (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and  dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and (dbo.InventoryConsumed.QuantityConsumed > 0) GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and (dbo.InventoryConsumed.QuantityConsumed > 0) GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and    dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' and (dbo.InventoryConsumed.QuantityConsumed > 0) GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT SUM(dbo.InventoryConsumed.QuantityConsumed) AS QuantityConsumed, dbo.RawItem.Id AS rid, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM,    dbo.Type.TypeName FROM            dbo.InventoryConsumed INNER JOIN                         dbo.RawItem ON dbo.InventoryConsumed.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                        dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id WHERE  (dbo.RawItem.Status='Active' or dbo.RawItem.Status is NULL)   and   dbo.RawItem.CategoryId='" + cmbgroupraw.SelectedValue + "' and    (dbo.InventoryConsumed.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryConsumed.branchid='" + cmbbranch.SelectedValue + "'  and dbo.InventoryConsumed.kdsid='" + cmbkitchen.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' and (dbo.InventoryConsumed.QuantityConsumed > 0)  GROUP BY dbo.RawItem.Id, dbo.RawItem.Code, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.UOM.UOM, dbo.Type.TypeName order by dbo.RawItem.ItemName";

                            }
                        }
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DataSet dspurchase = new DataSet();
                    string val = "";
                    double purchase = 0, consumed = 0, consumedamount = 0;
                    double qtycons=0;
                    double variance = 0, price = 0;

                    double rate = 0;
                    DataSet dscon1 = new DataSet();
                    q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + ds.Tables[0].Rows[i]["rid"].ToString() + "'";
                    dscon1 = objCore.funGetDataSet(q);
                    if (dscon1.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon1.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    


                    //try
                    //{
                    //    q = "SELECT        SUM(QuantityConsumed) AS Expr1, Date  FROM            dbo.InventoryConsumed where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + ds.Tables[0].Rows[i]["rid"].ToString() + "' GROUP BY Date";
                    //    DataSet dscon = new DataSet();
                    //    dscon = objCore.funGetDataSet(q);
                    //    foreach (DataRow item in dscon.Tables[0].Rows)
                    //    {
                    //        string tempp = item["Expr1"].ToString();
                    //        if (tempp == "")
                    //        {
                    //            tempp = "0";
                    //        }
                    //        qtycons = Convert.ToDouble(tempp);
                    //        qtycons = qtycons / rate;

                    //        price = getprice(item["Date"].ToString(), item["Date"].ToString(), ds.Tables[0].Rows[i]["rid"].ToString());

                    //        consumedamount = consumedamount + (qtycons * price);
                    //        consumed = consumed + (qtycons);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                        
                    //}
                    price = getprice(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["rid"].ToString());
                    val = ds.Tables[0].Rows[i]["QuantityConsumed"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    consumed = Convert.ToDouble(val);
                    consumed = consumed / rate;
                    consumedamount = (consumed * price);

                   
                   
                    double prchased = 0;
                    if (cmbkitchen.Text == "All")
                    {
                        dspurchase = new DataSet();
                        q = "SELECT     sum (dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + ds.Tables[0].Rows[i]["rid"].ToString() + "'";
                        dspurchase = objCore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            string temp = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            prchased = Convert.ToDouble(temp);
                        }
                    }
                    else
                    {
                        try
                        {

                            DataSet dsin = new DataSet();
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds.Tables[0].Rows[i]["rid"].ToString() + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where RecvStoreId='" + cmbkitchen.SelectedValue + "' and branchid='" + cmbbranch.SelectedValue + "' and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and itemid='" + ds.Tables[0].Rows[i]["rid"].ToString() + "'";
                            }
                            dsin = objCore.funGetDataSet(q);
                            if (dsin.Tables[0].Rows.Count > 0)
                            {
                                val = dsin.Tables[0].Rows[0][0].ToString();
                                if (val == "")
                                {
                                    val = "0";
                                }
                                prchased = Convert.ToDouble(val);
                            }


                        }
                        catch (Exception ex)
                        {

                        }
                    }



                    

                   //double qty = Math.Round(qty, 3);
                   //double amount = Math.Round(amount, 3);
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), "", consumed, consumedamount, price, null, prchased, ds.Tables[0].Rows[i]["TypeName"].ToString());
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), "", consumed, consumedamount, price, dscompany.Tables[0].Rows[0]["logo"], prchased, ds.Tables[0].Rows[i]["TypeName"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        public double getprice(string start, string end, string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + start + "','" + end + "'," + id + ")";
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
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
