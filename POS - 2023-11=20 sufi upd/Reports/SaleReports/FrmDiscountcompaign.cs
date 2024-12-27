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
    public partial class FrmDiscountcompaign : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmDiscountcompaign()
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
                getcompany();
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptcompaign rptDoc = new rptcompaign();
                POSRestaurant.Reports.SaleReports.dscompaign dsrpt = new dscompaign();
                
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
                rptDoc.SetParameterValue("phone", phone);
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
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Perc", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("qty", typeof(double));
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
                string q = "SELECT        SUM(dbo.Sale.DiscountAmount) AS Expr1,COUNT(dbo.Sale.DiscountAmount) AS Expr2, dbo.DiscountCompaign.Name, dbo.DiscountCompaign.Discount FROM            dbo.Sale INNER JOIN                         dbo.DiscountCompaign ON dbo.Sale.discountid = dbo.DiscountCompaign.Id where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' GROUP BY dbo.DiscountCompaign.Name, dbo.DiscountCompaign.Discount";
                              
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                  
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Discount"].ToString(), ds.Tables[0].Rows[i]["Expr1"].ToString(), null, ds.Tables[0].Rows[i]["Expr2"].ToString());
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Discount"].ToString(), ds.Tables[0].Rows[i]["Expr1"].ToString(), dscompany.Tables[0].Rows[0]["logo"], ds.Tables[0].Rows[i]["Expr2"].ToString());
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
    }
}
