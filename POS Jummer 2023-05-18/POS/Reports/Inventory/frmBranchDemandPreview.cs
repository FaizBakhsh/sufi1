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
    public partial class frmBranchDemandPreview : Form
    {
        public string id = "", date = "", restaurant = "", branchid = "", demandno = "";
        public frmBranchDemandPreview()
        {
            InitializeComponent();
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

                POSRestaurant.Reports.Inventory.rptDemandBranch rptDoc = new Reports.Inventory.rptDemandBranch();
                POSRestaurant.Reports.Inventory.dsBranchDemand dsrpt = new Reports.Inventory.dsBranchDemand();
                // .xsd file name
                DataTable dt = new DataTable();
                string company = "", phone = "", address = "", logo = "";
                try
                {
                    getcompany();
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                    date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                }
                catch (Exception ex)
                {

                }
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    rptDoc.SetDataSource(dsrpt);
                    rptDoc.SetParameterValue("name", company);
                    rptDoc.SetParameterValue("restaurant", "Restaurant: " + restaurant);
                  
                    date = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                    rptDoc.SetParameterValue("date","Date:"+ date);
                   
                    crystalReportViewer1.ReportSource = rptDoc;
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double getprice(string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + date + "','" + date + "'," + id + ")";
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
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("Category", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Price", typeof(double));
                dtrpt.Columns.Add("Closing", typeof(double));
                dtrpt.Columns.Add("ClosingValue", typeof(double));
                dtrpt.Columns.Add("Demand", typeof(double));
                dtrpt.Columns.Add("DemandValue", typeof(double));
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
                string q = "";
                q = "SELECT        dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.Demand.Quantity, dbo.Demand.Closing, dbo.Demand.Itemid, dbo.Category.CategoryName FROM            dbo.Demand INNER JOIN                         dbo.RawItem ON dbo.Demand.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id WHERE        dbo.Demand.Date='" + date + "' and dbo.Demand.branchid='" + branchid + "'";
                dsinfo = objCore.funGetDataSet(q);
                for (int i = 0; i < dsinfo.Tables[0].Rows.Count; i++)
                {
                    double closing = 0;
                    string temp = dsinfo.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    temp = dsinfo.Tables[0].Rows[i]["Closing"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    closing = Convert.ToDouble(temp);
                    double price = getprice(dsinfo.Tables[0].Rows[i]["Itemid"].ToString());
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["CategoryName"].ToString(), dsinfo.Tables[0].Rows[i]["ItemName"].ToString(), dsinfo.Tables[0].Rows[i]["UOM"].ToString(), price, closing, closing * price, qty, qty * price, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["CategoryName"].ToString(), dsinfo.Tables[0].Rows[i]["ItemName"].ToString(), dsinfo.Tables[0].Rows[i]["UOM"].ToString(), price, closing, closing * price, qty, qty * price, dscompany.Tables[0].Rows[0]["logo"]);          
                    }

                }
              
            }
            catch (Exception ex)
            {

            }

            return dtrpt;
        }
       
       
        private void frmInventoryPreview_Load(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
