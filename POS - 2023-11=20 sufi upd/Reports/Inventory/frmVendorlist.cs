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
    public partial class frmVendorlist : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmVendorlist()
        {
            InitializeComponent();
        }
        public void fill()
        {
            
        }
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            
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


                POSRestaurant.Reports.Inventory.rptVendors rptDoc = new rptVendors();
                POSRestaurant.Reports.Inventory.dsVendors dsrpt = new dsVendors();
                
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
                //rptDoc.SetParameterValue("title", "Stocke Issuence Report");
                //rptDoc.SetParameterValue("Comp", company);
                //rptDoc.SetParameterValue("Address", address);
                //rptDoc.SetParameterValue("phone",phone );
                
               
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
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Phone", typeof(string));
                dtrpt.Columns.Add("Address", typeof(string));
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

                q = "SELECT        dbo.Supplier.id, dbo.Supplier.Name, dbo.Supplier.Phone, dbo.Supplier.Address FROM            dbo.Supplier  order by dbo.Supplier.Name";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Phone"].ToString(), ds.Tables[0].Rows[i]["Address"].ToString(), getcat(ds.Tables[0].Rows[i]["id"].ToString()));
                

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        protected string getcat(string id)
        {
            string cat = "";

            try
            {
                string q = "SELECT DISTINCT dbo.Category.CategoryName FROM            dbo.RawItem INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id where dbo.RawItem.Supplierid='" + id + "'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (cat.Length > 0)
                    {
                        cat = cat + " ," + ds.Tables[0].Rows[i]["CategoryName"].ToString();
                    }
                    else
                    {
                        cat = ds.Tables[0].Rows[i]["CategoryName"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            return cat;

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
