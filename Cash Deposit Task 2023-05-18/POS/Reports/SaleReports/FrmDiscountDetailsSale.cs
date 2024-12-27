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
    public partial class FrmDiscountDetailsSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmDiscountDetailsSale()
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


                POSRestaurant.Reports.SaleReports.rptdiscountdetails rptDoc = new rptdiscountdetails();
                POSRestaurant.Reports.SaleReports.dsdiscountdetails dsrpt = new dsdiscountdetails();
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
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
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
                dtrpt.Columns.Add("date", typeof(DateTime));
                dtrpt.Columns.Add("time", typeof(DateTime));
                dtrpt.Columns.Add("billno", typeof(string));
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("phone", typeof(string));
                dtrpt.Columns.Add("staff", typeof(string));
                dtrpt.Columns.Add("amount", typeof(double));
                dtrpt.Columns.Add("Reference", typeof(string));
                dtrpt.Columns.Add("TotalBill", typeof(double));
               
                DataSet ds = new DataSet();
                string q = "SELECT        dbo.DiscountDetails.Reference,dbo.DiscountDetails.name, dbo.DiscountDetails.phone, dbo.DiscountDetails.staff, dbo.DiscountDetails.saleid, dbo.Sale.Date, dbo.Sale.time, dbo.Sale.DiscountAmount, dbo.Sale.NetBill FROM            dbo.DiscountDetails INNER JOIN                         dbo.Sale ON dbo.DiscountDetails.saleid = dbo.Sale.Id where dbo.sale.discount>0  and sale.billstatus='Paid'  order by dbo.sale.id";
                          
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["time"].ToString(), ds.Tables[0].Rows[i]["saleid"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), ds.Tables[0].Rows[i]["phone"].ToString(), ds.Tables[0].Rows[i]["staff"].ToString(), ds.Tables[0].Rows[i]["DiscountAmount"].ToString(), ds.Tables[0].Rows[i]["Reference"].ToString(), ds.Tables[0].Rows[i]["NetBill"].ToString());                   
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

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            try
            {
                CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
                string id = info.Text;
                if (info.Name == "billno1")
                {
                    id = Convert.ToInt32(id).ToString();
                    if (id.Length > 0)
                    {
                        FrmInvoiceDetailsSale obj = new FrmInvoiceDetailsSale();
                        obj.saleid = id;
                        obj.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
