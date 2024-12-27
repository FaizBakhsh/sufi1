
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class Frmitemlist : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public Frmitemlist()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,CategoryName from Category order by CategoryName";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["CategoryName"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbcategory.DataSource = dsi.Tables[0];
                cmbcategory.ValueMember = "id";
                cmbcategory.DisplayMember = "CategoryName";
                cmbcategory.Text = "All";

            }
            catch (Exception ex)
            {

               
            }
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,Name from KDS order by Name";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["Name"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbkitchen.DataSource = dsi.Tables[0];
                cmbkitchen.ValueMember = "id";
                cmbkitchen.DisplayMember = "Name";
                cmbkitchen.Text = "All";

            }
            catch (Exception ex)
            {

            }
            
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from Branch");

        }
       
        public void bindreport()
        {

            try
            {
                string company = "";
                try
                {
                    getcompany();
                    company = dscompany.Tables[0].Rows[0]["BranchName"].ToString();
                   
                }
                catch (Exception ex)
                {


                }
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptitemlist rptDoc = new  rptitemlist();
                POSRestaurant.Reports.SaleReports.dsitemlist dsrpt = new dsitemlist();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();

                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);

                rptDoc.SetDataSource(dsrpt);
                string of = "of \"" + cmbkitchen.Text + "\" Kitchen(s) and \"" + cmbcategory.Text + "\" Category(s)";
                rptDoc.SetParameterValue("branch", company);
                rptDoc.SetParameterValue("of", of);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
                  
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
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Price", typeof(string));
                dtrpt.Columns.Add("Min", typeof(double));
                dtrpt.Columns.Add("Max", typeof(double));
                dtrpt.Columns.Add("Difference", typeof(double));
                DataSet ds = new DataSet();
                string q = "";
                if (cmbkitchen.Text == "All")
                {
                    if (cmbcategory.Text == "All")
                    {
                        q = "SELECT        dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Id, dbo.RawItem.price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.status='Active' order by dbo.RawItem.ItemName";
               
                    }
                    else
                    {
                        q = "SELECT        dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Id, dbo.RawItem.price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.status='Active' and dbo.RawItem.categoryid='" + cmbcategory.SelectedValue + "' order by dbo.RawItem.ItemName";
               
                    }
                }
                else
                {
                    if (cmbcategory.Text == "All")
                    {
                        q = "SELECT      DISTINCT   dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Id, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.MenuItem INNER JOIN                         dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id where dbo.RawItem.status='Active' and dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";

                    }
                    else
                    {
                        q = "SELECT      DISTINCT   dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.RawItem.Id, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder FROM            dbo.MenuItem INNER JOIN                         dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id where dbo.RawItem.status='Active' and  dbo.RawItem.categoryid='" + cmbcategory.SelectedValue + "' and dbo.MenuItem.KDSId='" + cmbkitchen.SelectedValue + "' order by dbo.RawItem.ItemName";

                    }
                }
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string min = (ds.Tables[0].Rows[i]["MinOrder"].ToString());
                    if (min == "")
                    {
                        min = "0";
                    }
                    string maxorder = (ds.Tables[0].Rows[i]["maxorder"].ToString());
                    if (maxorder == "")
                    {
                        maxorder = "0";
                    }
                    double difference = Convert.ToDouble(maxorder) - Convert.ToDouble(min);
                    double price = getprice(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), ds.Tables[0].Rows[i]["Id"].ToString());
                    try
                    {
                        if (price == 0)
                        {
                            string temp = ds.Tables[0].Rows[i]["price"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            price = Convert.ToDouble(temp);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), price, min, maxorder, difference);
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
        protected string getprice(string id)
        {
            string prc = "0";
            if (id == "11")
            {
            }
            try
            {
                string q = "select id, Price, PricePerItem FROM            PurchaseDetails where RawItemId='" + id + "' order by id desc";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
                {
                    string temp = ds.Tables[0].Rows[i]["PricePerItem"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double price = Math.Round(Convert.ToDouble(temp), 3);
                    prc = prc + price.ToString() + ",";
                }
            }
            catch (Exception ex)
            {
                                
            }
            return prc;
        }
        private void crystalReportViewer1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void crystalReportViewer1_Click(object sender, EventArgs e)
        {
            
        }

        private void crystalReportViewer1_ClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
           
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            
        }
    }
}
