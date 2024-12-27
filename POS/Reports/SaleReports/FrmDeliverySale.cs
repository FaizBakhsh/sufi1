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
    public partial class FrmDeliverySale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmDeliverySale()
        {
            InitializeComponent();
        }
        public string saleid = "";
        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            
           
        }
       
        
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptDelivery rptDoc = new rptDelivery();
                POSRestaurant.Reports.SaleReports.dsdelivery dsrpt = new dsdelivery();
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
                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
               
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
      
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Phone", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                
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


                q = "SELECT        SUM(dbo.Sale.NetBill) AS Amount, dbo.Delivery.Name, dbo.Delivery.Phone FROM            dbo.Delivery INNER JOIN                         dbo.Sale ON dbo.Delivery.SaleId = dbo.Sale.Id where sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' GROUP BY dbo.Delivery.Name, dbo.Delivery.Phone";
                
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Phone"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Amount"].ToString());                 
                }

                
               

                

            }
            catch (Exception ex)
            {

               
            }
            return dtrpt;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
