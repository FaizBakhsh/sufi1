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
    public partial class frmissuence : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmissuence()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                DataSet dsq = new DataSet();
                string q = "select * from RawItem where status='Active' or status is null";
                if (cmbcat.Text != "All")
                {
                    q = "select * from RawItem where Categoryid='" + cmbcat.SelectedValue + "' and status='Active' or status is null";
                }
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
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
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
                string q = "select * from Category";
                dsq = objCore.funGetDataSet(q);
                DataRow drq = dsq.Tables[0].NewRow();
                drq["CategoryName"] = "All";
                dsq.Tables[0].Rows.Add(drq);
                cmbcat.DataSource = dsq.Tables[0];
                cmbcat.ValueMember = "id";
                cmbcat.DisplayMember = "CategoryName";
                cmbcat.Text = "All";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            fill();
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


                POSRestaurant.Reports.Inventory.rptissue rptDoc = new rptissue();
                POSRestaurant.Reports.Inventory.dsissue dsrpt = new dsissue();
                
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
                rptDoc.SetParameterValue("title", "Stocke Issuence Report");
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Address", address);
                rptDoc.SetParameterValue("phone",phone );
                if (cmbcat.Text == "All")
                {
                    rptDoc.SetParameterValue("sup", "of All Suppliers");
                }
                else
                {
                    rptDoc.SetParameterValue("sup", "of " + cmbcat.Text);
                }
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        protected string getbranch(string id)
        {

            try
            {
                string q = "SELECT  branchname from branch where id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    id = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
            return id;
        }
        protected string getuom(string id)
        {
            string uom = "";
            string q = "SELECT        dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.id=" + id;
            DataSet ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                uom = ds.Tables[0].Rows[0][0].ToString();
            }
            return uom;
        }
        protected string getcat(string id)
        {
            string cat = "";
            try
            {
                string q = "SELECT        CategoryName FROM            Category where id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cat = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
            return cat;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("sourcebranchid", typeof(string));
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("price", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("Category", typeof(string));
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
               
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbitem.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (cmbcat.Text == "All")
                                {
                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id  where  (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null  GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";                            
                                }
                            }
                            else
                            {
                                if (cmbcat.Text == "All")
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";
                             
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "' and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";                             
                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (cmbcat.Text == "All")
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";
                              
                                }
                                else
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";
                              
                                }
                            }
                            else
                            {
                                if (cmbcat.Text == "All")
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id  where    (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";
                              
                                }
                                else
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.Date, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id    where  (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "'  and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.InventoryTransfer.Date,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.InventoryTransfer.Date";
                              
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
                                if (cmbcat.Text == "All")
                                {
                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid,  AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id    where  (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null  GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid order by dbo.RawItem.ItemName";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid,  AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "' GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid  order by dbo.RawItem.ItemName";
                                }
                            }
                            else
                            {
                                if (cmbcat.Text == "All")
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid,  AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id    where  (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "' GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid  order by dbo.RawItem.ItemName";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName, dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "' and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid  order by dbo.RawItem.ItemName";
                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (cmbcat.Text == "All")
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid,  AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid  order by dbo.RawItem.ItemName";

                                }
                                else
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid,  AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid  order by dbo.RawItem.ItemName";

                                }
                            }
                            else
                            {
                                if (cmbcat.Text == "All")
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName, dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid, AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid  order by dbo.RawItem.ItemName";

                                }
                                else
                                {

                                    q = "SELECT        SUM(dbo.InventoryTransfer.TransferIn) AS qty, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.CategoryId, dbo.InventoryTransfer.sourcebranchid,  AVG(dbo.InventoryTransfer.price) AS price, SUM(dbo.InventoryTransfer.total) AS total FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id   where   (dbo.InventoryTransfer.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sourcebranchid is not null and  dbo.InventoryTransfer.Itemid='" + cmbitem.SelectedValue + "'  and dbo.InventoryTransfer.branchid='" + cmbbranch.SelectedValue + "'  and dbo.RawItem.supplierid='" + cmbcat.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName,dbo.RawItem.id ,dbo.RawItem.CategoryId,dbo.InventoryTransfer.sourcebranchid  order by dbo.RawItem.ItemName";

                                }
                            }
                        }
                    }
                }
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string date = "",invoice="",branchname="",category="";

                    branchname = getbranch(ds.Tables[0].Rows[i]["sourcebranchid"].ToString());
                    try
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {
                       
                    }
                    try
                    {
                        category = getcat(ds.Tables[0].Rows[i]["CategoryId"].ToString());
                    }
                    catch (Exception ex)
                    {

                    }
                    double qty = 0;
                    double price = 0;
                    try
                    {
                        string temp = ds.Tables[0].Rows[i]["qty"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        qty = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[i]["total"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double Amount = Convert.ToDouble(temp);
                        if (qty > 0)
                        {
                            price = Amount / qty;
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    if (qty > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(branchname, date, ds.Tables[0].Rows[i]["ItemName"].ToString(), getuom(ds.Tables[0].Rows[i]["id"].ToString()), ds.Tables[0].Rows[i]["qty"].ToString(), price, ds.Tables[0].Rows[i]["total"].ToString(), null,category);
                        }
                        else
                        {
                            dtrpt.Rows.Add(branchname, date, ds.Tables[0].Rows[i]["ItemName"].ToString(), getuom(ds.Tables[0].Rows[i]["id"].ToString()), ds.Tables[0].Rows[i]["qty"].ToString(), price, ds.Tables[0].Rows[i]["total"].ToString(), dscompany.Tables[0].Rows[0]["logo"], category);
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

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cmbcat_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill();
        }
    }
}
