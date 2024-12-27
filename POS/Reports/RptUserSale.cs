using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
namespace POSRestaurant.Reports
{
    public partial class RptUserSale : Form
    {
        private POSRestaurant.Sale.RestSale _frm1;
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public RptUserSale(POSRestaurant.Sale.RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            UserCashReport rptDoc = new UserCashReport();
          // rptUserCashReport rptDoc = new  rptUserCashReport();
            UserSale ds = new UserSale();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "Crystal Report";
            dt = getAllOrders();
            ds.Tables[0].Merge(dt);


            rptDoc.SetDataSource(ds);
            crystalReportViewer1.ReportSource = rptDoc;



        }
        public DataTable getAllOrders()
        {

            DataTable dat = new DataTable();
            dat.Columns.Add("GrossSale", typeof(double));
            dat.Columns.Add("GST", typeof(double));
            dat.Columns.Add("Discount", typeof(double));
            dat.Columns.Add("NetSale", typeof(double));
            dat.Columns.Add("CashSale", typeof(double));
            dat.Columns.Add("CreditSale", typeof(double));
            dat.Columns.Add("MasterSale", typeof(double));
            dat.Columns.Add("DinIn", typeof(double));
            dat.Columns.Add("TakeAway", typeof(double));
            dat.Columns.Add("Delivery", typeof(double));
            dat.Columns.Add("Refund", typeof(double));
            dat.Columns.Add("Void", typeof(double));
            dat.Columns.Add("CName", typeof(string));
            dat.Columns.Add("CAddress", typeof(string));
            dat.Columns.Add("CPhone", typeof(string));
            dat.Columns.Add("Date", typeof(string));
            dat.Columns.Add("Cashier", typeof(string));
            dat.Columns.Add("Dlorders", typeof(string));
            dat.Columns.Add("Torders", typeof(string));
            dat.Columns.Add("Dorders", typeof(string));
            dat.Columns.Add("RefundNo", typeof(string));
            dat.Columns.Add("carhope", typeof(double));
            dat.Columns.Add("carhopeorders", typeof(string));
            double  gst = 0, discount = 0, net = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0,carhope=0;
            string gross = "0", cname = "", caddress = "", cphone = "", cashier = cashiername, Dlorders = "0", Torders = "0", Dorders = "0", RefundNo="0",carhopeorders="0";
            
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                string q = "SELECT * from CompanyInfo ";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cname = (ds.Tables[0].Rows[0]["Name"].ToString());
                    caddress = (ds.Tables[0].Rows[0]["Address"].ToString());
                    cphone = (ds.Tables[0].Rows[0]["Phone"].ToString());
                   
                }
                string temp = "";
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale, SUM(Discount) AS discount FROM         Sale where userid='"+userid+"' and date='"+date+"'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    gross = (ds.Tables[0].Rows[0]["gross"].ToString());
                    gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                    discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                    net = Convert.ToDouble(ds.Tables[0].Rows[0]["netsale"].ToString());
                }
                else
                {
                    gross = "0";
                    gst = 0;
                    discount = 0;
                    net = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where userid='" + userid + "' and date='" + date + "' and BillType='Cash'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cash = Convert.ToDouble(temp);

                }
                else
                {
                    cash = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where userid='" + userid + "' and date='" + date + "' and BillType='Master Card'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    master = Convert.ToDouble(temp);
                   

                }
                else
                {
                    master = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where userid='" + userid + "' and date='" + date + "' and BillType='Credit Card'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                   // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    credit = Convert.ToDouble(temp);
                }
                else
                {
                    credit = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash , count(id) as count FROM         Sale where userid='" + userid + "' and date='" + date + "' and OrderType='Delivery'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    delivery = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    Dlorders = (temp);
                    //delivery = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    //Dlorders = ds.Tables[0].Rows[0]["count"].ToString();
                }
                else
                {
                    delivery = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where userid='" + userid + "' and date='" + date + "' and OrderType='Take Away'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                   // takeaway = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    takeaway = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    Torders = temp;
                }
                else
                {
                    takeaway = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where userid='" + userid + "' and date='" + date + "' and OrderType='Din In'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //dinin = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    dinin = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    Dorders = (temp);
                    //Dorders = ds.Tables[0].Rows[0]["count"].ToString();
                }
                else
                {
                    dinin = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where userid='" + userid + "' and date='" + date + "' and OrderType='Car Hope'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //dinin = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    carhope = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    carhopeorders = (temp);
                    //Dorders = ds.Tables[0].Rows[0]["count"].ToString();
                }
                else
                {
                    carhope = 0;
                }
                ds = new DataSet();
                q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where userid='" + userid + "' and date='" + date + "' and BillStatus='Refund'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    refund = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    
                    RefundNo = temp;
                }
                else
                {
                    refund = 0;
                }
                ds = new DataSet();
                q = "SELECT     COUNT(dbo.Saledetails.Id) AS count  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Saledetails.Status = 'Void') and dbo.Sale.userid='" + userid + "' and dbo.Sale.Date='" + date + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                   // voidsale = Convert.ToDouble(ds.Tables[0].Rows[0]["count"].ToString());
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    voidsale = Convert.ToDouble(temp);
                }
                else
                {
                    voidsale = 0;
                }

            }
            catch (Exception ex)
            {
                
            }
            
            dat.Rows.Add(gross,gst,discount,net,cash,credit,master,dinin,takeaway,delivery,refund,voidsale,cname,caddress,cphone,date,cashier,Dlorders,Torders,Dorders,RefundNo,carhope,carhopeorders);
            return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
            bindreport();
        }

        private void RptUserSale_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
