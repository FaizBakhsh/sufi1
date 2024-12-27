
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
    public partial class FrmSalesTax : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmSalesTax()
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


                POSRestaurant.Reports.SaleReports.rptSalestax rptDoc = new rptSalestax();
                POSRestaurant.Reports.SaleReports.dsSalestax dsrpt = new dsSalestax();
                
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
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
               
                
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs",address );
                rptDoc.SetParameterValue("phn", phone);
               
                rptDoc.SetParameterValue("date", "for the period of "+dateTimePicker1.Text +" to "+dateTimePicker2.Text);
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
                dtrpt.Columns.Add("inv", typeof(string));
                dtrpt.Columns.Add("date", typeof(DateTime));
                dtrpt.Columns.Add("exsale", typeof(double));
                dtrpt.Columns.Add("tax", typeof(double));
                dtrpt.Columns.Add("incsal", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("items", typeof(string));
                                
                DataSet ds = new DataSet();
                string q = "";
                q = "SELECT SUM(TotalBill + GST - DiscountAmount) AS inc, SUM(TotalBill - DiscountAmount) AS exc, SUM(GST) AS tax, Date, Id FROM  dbo.Sale  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid' GROUP BY Date, Id";               
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["inc"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double incl = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["exc"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double exc = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["tax"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double tax = Convert.ToDouble(val);
                    string items = "";
                    DataSet dsitems = new DataSet();
                    q = "SELECT dbo.MenuItem.Name, dbo.Saledetails.saleid FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id where dbo.Saledetails.saleid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                    dsitems = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsitems.Tables[0].Rows.Count; j++)
                    {
                        if (items.Length > 0)
                        {
                            items = items + ",";
                        }
                        items = items + dsitems.Tables[0].Rows[j]["name"].ToString().Trim();
                       
                    }
                    exc = Math.Round(exc, 3);
                    incl = Math.Round(incl, 3);
                    tax = Math.Round(tax, 3);
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["date"].ToString(), exc, tax, incl, null,items);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["date"].ToString(), exc, tax, incl, dscompany.Tables[0].Rows[0]["logo"], items);
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
