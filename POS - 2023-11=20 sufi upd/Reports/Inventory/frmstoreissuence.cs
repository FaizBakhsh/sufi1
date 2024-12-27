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
    public partial class frmstoreissuence : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmstoreissuence()
        {
            InitializeComponent();
        }
        
        public void fillstore()
        {
            try
            {
                DataTable dt = new DataTable();
                objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Stores where branchid='" + cmbbranch.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                dt = ds.Tables[0];
                cmbstore1.DataSource = dt;
                cmbstore1.ValueMember = "id";
                cmbstore1.DisplayMember = "StoreName";

            }
            catch (Exception ex)
            {


            }
            try
            {
                DataTable dt = new DataTable();
                objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from KDS where id>0";
                ds = objCore.funGetDataSet(q);
                dt = ds.Tables[0];
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbstore2.DataSource = dt;
                cmbstore2.ValueMember = "id";
                cmbstore2.DisplayMember = "Name";
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
            getitems();
            try
            {
                ds = new DataSet();
                string q = "select * from branch";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                //dr["branchname"] = "All";
                //ds.Tables[0].Rows.Add(dr);
                cmbbranch.DataSource = ds.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";
                
                fillstore();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
            
        }
        protected void getitems()
        {
            try
            {
                DataSet dsq = new DataSet();
                string q = "select * from RawItem";
                if (cmbcategory.Text != "All")
                {
                    q = "select * from RawItem where categoryid='" + cmbcategory.SelectedValue + "'";
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
        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            fillstore();
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
                POSRestaurant.Reports.Inventory.rptStoreissuance rptDoc = new rptStoreissuance();
                POSRestaurant.Reports.Inventory.dsstoreissuance dsrpt = new dsstoreissuance();                
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
                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);                
                rptDoc.SetDataSource(dsrpt);                
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn",phone );
                rptDoc.SetParameterValue("store", "From " + cmbstore1.Text + " to " + cmbstore2.Text);
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public double getcostmenu1(string start, string end, string id)
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
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));               
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("price", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));              
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
                if (cmbcategory.Text == "All")
                {
                    if (cmbstore2.Text != "All")
                    {
                        if (checkBox1.Checked == false)
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      '' as Date,  SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT    '' as Date, SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "' and dbo.RawItem.Id='" + cmbitem.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM  order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "'  and dbo.InventoryTransferStore.Quantity>0  order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT     dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "' and dbo.RawItem.Id='" + cmbitem.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName ";

                            }
                        }
                    }
                    else
                    {
                        if (checkBox1.Checked == false)
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      '' as Date,  SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT    '' as Date, SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.RawItem.Id='" + cmbitem.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM  order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "'  and dbo.InventoryTransferStore.Quantity>0  order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT     dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "'  and dbo.RawItem.Id='" + cmbitem.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName ";

                            }
                        }
                    }
                }
                else
                {
                    if (cmbstore2.Text != "All")
                    {
                        if (checkBox1.Checked == false)
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      '' as Date,  SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT    '' as Date, SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "' and dbo.RawItem.Id='" + cmbitem.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM  order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "'  and dbo.InventoryTransferStore.Quantity>0 and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'   order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT     dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.RecvStoreId='" + cmbstore2.SelectedValue + "' and dbo.RawItem.Id='" + cmbitem.SelectedValue + "'  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName ";

                            }
                        }
                    }
                    else
                    {
                        if (checkBox1.Checked == false)
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      '' as Date,  SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0 and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'   GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM order by dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT    '' as Date, SUM(dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "' and dbo.RawItem.Id='" + cmbitem.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  GROUP BY dbo.RawItem.ItemName, dbo.RawItem.Id , dbo.UOM.UOM  order by dbo.RawItem.ItemName";

                            }
                        }
                        else
                        {
                            if (cmbitem.Text == "All")
                            {
                                q = "SELECT      dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "'  and dbo.InventoryTransferStore.Quantity>0  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'   order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName";
                            }
                            else
                            {
                                q = "SELECT     dbo.InventoryTransferStore.Date,  (dbo.InventoryTransferStore.Quantity) AS Quantity, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS ItemName, dbo.RawItem.Id  FROM            dbo.InventoryTransferStore INNER JOIN                         dbo.RawItem ON dbo.InventoryTransferStore.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransferStore.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.InventoryTransferStore.IssuingStoreId='" + cmbstore1.SelectedValue + "'  and dbo.RawItem.Id='" + cmbitem.SelectedValue + "' and dbo.InventoryTransferStore.Quantity>0  and dbo.RawItem.Categoryid='" + cmbcategory.SelectedValue + "'  order by dbo.InventoryTransferStore.Date, dbo.RawItem.ItemName ";

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
                        date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {
                       
                    }
                    double qty = 0;
                    double price = 0, Amount = 0;
                    try
                    {
                        string temp = ds.Tables[0].Rows[i]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        qty = Convert.ToDouble(temp);
                        price = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["Id"].ToString());
                        Amount = price * qty;
                        
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    if (qty > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(date, ds.Tables[0].Rows[i]["ItemName"].ToString(), qty, price, Amount, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(date, ds.Tables[0].Rows[i]["ItemName"].ToString(), qty, price, Amount, dscompany.Tables[0].Rows[0]["logo"]);
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            getitems();
        }
    }
}
