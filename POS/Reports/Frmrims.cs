using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports
{
    public partial class Frmrims : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public Frmrims()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            
            
        }
        
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.rptpra rptDoc = new rptpra();
                POSRestaurant.Reports.dspra dsrpt = new dspra();
                dt.TableName = "Crystal Report";
                dt = getAllOrders();               
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                rptDoc.SetDataSource(dsrpt);           
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
                dtrpt.Columns.Add("InvoiceNumber", typeof(string));
                dtrpt.Columns.Add("TableNumber", typeof(string));
                dtrpt.Columns.Add("InvoiceDate", typeof(DateTime));
                dtrpt.Columns.Add("CustomerName", typeof(string));
                dtrpt.Columns.Add("CustomerPhone", typeof(string));
                dtrpt.Columns.Add("DiscountAmount", typeof(double));
                dtrpt.Columns.Add("ServiceChargesAmount", typeof(double));
                dtrpt.Columns.Add("TaxAmount", typeof(double));
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
                q = "SELECT Id, Date, Customer, DiscountAmount, GST,servicecharges, NetBill FROM  Sale WHERE (BillStatus = 'Paid')  and date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' ORDER BY Id";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string dis = ds.Tables[0].Rows[i]["DiscountAmount"].ToString();
                    if (dis == "")
                    {
                        dis = "0";
                    }
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Id"].ToString(), "", ds.Tables[0].Rows[i]["Date"].ToString().ToString(), ds.Tables[0].Rows[i]["Customer"].ToString(), "", dis, ds.Tables[0].Rows[i]["servicecharges"].ToString(), ds.Tables[0].Rows[i]["gst"].ToString(), ds.Tables[0].Rows[i]["NetBill"].ToString());                                                       
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

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
