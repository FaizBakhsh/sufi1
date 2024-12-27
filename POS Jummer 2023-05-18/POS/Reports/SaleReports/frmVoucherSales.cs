using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Net.Mail;
namespace POSRestaurant.Reports.SaleReports
{
    public partial class frmVoucherSales : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public frmVoucherSales()
        {
            InitializeComponent();

        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptvouchers rptDoc = new SaleReports.rptvouchers();
            POSRestaurant.Reports.SaleReports.dsvouchers ds = new SaleReports.dsvouchers();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

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
                ds.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
            }
            
           

            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("phn", phone);
           
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
            cryRpt = rptDoc;
        }
     
       
        DataSet dscompany = new DataSet();
        DataSet dsbranch = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");
            dsbranch = objCore.funGetDataSet("select * from Branch");
        }
        public DataTable getAllOrders()
        {

            DataTable dat = new DataTable();
            dat.Columns.Add("Date", typeof(DateTime));
            dat.Columns.Add("BillNo", typeof(string));
            dat.Columns.Add("VoucherName", typeof(string));
            dat.Columns.Add("Amount", typeof(double));
            dat.Columns.Add("Narration", typeof(string));
            
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
            SqlDataAdapter adapter = new SqlDataAdapter();
            

                string temp = "";
                string q = "";
                try
                {
                    q = "SELECT        dbo.Sale.Date, dbo.VoucherTrack.Saleid AS BillNo, dbo.VoucherKeys.Name + '(' + CONVERT(varchar(10), dbo.VoucherKeys.Amount) + ')' AS VoucherName, dbo.VoucherTrack.Amount,Sale.VoucherNaration as Narration FROM            dbo.Sale INNER JOIN                         dbo.VoucherTrack ON dbo.Sale.Id = dbo.VoucherTrack.Saleid INNER JOIN                         dbo.VoucherKeys ON dbo.VoucherTrack.VoucherId = dbo.VoucherKeys.Id where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.billstatus='paid' ";
                    ds = objcore.funGetDataSet(q);
                    dat.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                }
                catch (Exception ex)
                {
                    
                   
                }

            return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                // ds1.Tables[0].Rows.Add(dr1);
                cmbbranch.DataSource = ds1.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
      
       
    
      
   
     
       
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Enabled = false;
            vButton1.Text = "Please Wait";
           
            bindreport();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }
   
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        ReportDocument cryRpt;
        private void vButton2_Click(object sender, EventArgs e)
        {

           
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
    }
}
