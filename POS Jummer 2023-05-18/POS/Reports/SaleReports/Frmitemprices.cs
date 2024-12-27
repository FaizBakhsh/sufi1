
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
    public partial class Frmitemprices : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public Frmitemprices()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            
            
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


                POSRestaurant.Reports.rptitemprices rptDoc = new rptitemprices();
                POSRestaurant.Reports.dsitemprices dsrpt = new  dsitemprices();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    
                }

                rptDoc.SetDataSource(dsrpt);
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
                dtrpt.Columns.Add("group", typeof(string));
                dtrpt.Columns.Add("Code", typeof(string));
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("Size", typeof(string));
               
                dtrpt.Columns.Add("Price", typeof(string));
                dtrpt.Columns.Add("gst", typeof(string));
                dtrpt.Columns.Add("total", typeof(string));
                dtrpt.Columns.Add("Start", typeof(string));
                dtrpt.Columns.Add("End", typeof(string));
                dtrpt.Columns.Add("Kitchen", typeof(string));
                DataSet ds = new DataSet();
                string q = "";
                if (checkBox1.Checked == true)
                {
                    if (textBox1.Text == "")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.MenuGroup.Name, dbo.MenuItem.Id, dbo.MenuItem.Name AS Expr1, dbo.MenuItem.Code, dbo.MenuItem.Price, dbo.MenuItem.startdate, dbo.MenuItem.enddate, dbo.KDS.Name AS Kitchen FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id where dbo.menuitem.status='Active' ORDER BY dbo.MenuGroup.name";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.MenuGroup.Name, dbo.MenuItem.Id, dbo.MenuItem.Name AS Expr1, dbo.MenuItem.Code, dbo.MenuItem.Price, dbo.MenuItem.startdate, dbo.MenuItem.enddate, dbo.KDS.Name AS Kitchen FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id  where dbo.MenuItem.name like '%" + textBox1.Text + "%' and  dbo.menuitem.status='Active' ORDER BY dbo.MenuGroup.name";
                    }
                }
                else
                {
                    if (textBox1.Text == "")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.MenuGroup.Name, dbo.MenuItem.Id, dbo.MenuItem.Name AS Expr1, dbo.MenuItem.Code, dbo.MenuItem.Price, dbo.MenuItem.startdate, dbo.MenuItem.enddate, dbo.KDS.Name AS Kitchen FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id  ORDER BY dbo.MenuGroup.name";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.MenuGroup.Name, dbo.MenuItem.Id, dbo.MenuItem.Name AS Expr1, dbo.MenuItem.Code, dbo.MenuItem.Price, dbo.MenuItem.startdate, dbo.MenuItem.enddate, dbo.KDS.Name AS Kitchen FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id  where dbo.MenuItem.name like '%" + textBox1.Text + "%'  ORDER BY dbo.MenuGroup.name";
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";

                    string price = "", start = "", end = "";
                    try
                    {
                        start = Convert.ToDateTime(ds.Tables[0].Rows[i]["startdate"].ToString()).ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                    try
                    {
                        end = Convert.ToDateTime(ds.Tables[0].Rows[i]["enddate"].ToString()).ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {


                    }
                    double gst = 0, total = 0;
                     
                    string size = "";
                    price = ds.Tables[0].Rows[i]["Price"].ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    gst = ((Convert.ToDouble(price) * .16));
                    total = gst + Convert.ToDouble(price);
                    
                    string name =  ds.Tables[0].Rows[i]["Expr1"].ToString();
                    q = "select * from ModifierFlavour where MenuItemId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                    DataSet dsss = new DataSet();
                    dsss = objCore.funGetDataSet(q);
                    if (dsss.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsss.Tables[0].Rows.Count; j++)
                        {
                            size = dsss.Tables[0].Rows[j]["name"].ToString();
                            price = dsss.Tables[0].Rows[j]["Price"].ToString();
                            if (price == "")
                            {
                                price = "0";
                            }
                            gst = ((Convert.ToDouble(price) * .16));
                           
                            total = gst + Convert.ToDouble(price);
                            gst = Math.Round(gst,2);
                            total = Math.Round(total, 2);
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Code"].ToString(), name, size, price, gst.ToString(), total.ToString(), start, end, ds.Tables[0].Rows[i]["Kitchen"].ToString());
                        }
                    }
                    else
                    {
                        gst = Math.Round(gst, 2);
                        total = Math.Round(total, 2);
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Code"].ToString(), name, size, price, gst, total, start, end, ds.Tables[0].Rows[i]["Kitchen"].ToString());
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
