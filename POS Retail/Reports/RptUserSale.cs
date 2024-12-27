using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
namespace POSRetail.Reports
{
    public partial class RptUserSale : Form
    {
        private POSRetail.Sale.Sale _frm1;
        public string date = "", userid = "", cashiername = "";
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public RptUserSale(POSRetail.Sale.Sale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
           UserCashReport rptDoc = new  UserCashReport();

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
           
            dat.Columns.Add("Refund", typeof(double));
            
            dat.Columns.Add("CName", typeof(string));
            dat.Columns.Add("CAddress", typeof(string));
            dat.Columns.Add("CPhone", typeof(string));
            dat.Columns.Add("Date", typeof(string));
            dat.Columns.Add("Cashier", typeof(string));
           
            dat.Columns.Add("RefundNo", typeof(string));
            dat.Columns.Add("logo", typeof(byte[]));

            double  gst = 0, discount = 0, net = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0;
            string gross = "0", cname = "", caddress = "", cphone = "", cashier = cashiername, Dlorders = "0", Torders = "0", Dorders = "0", RefundNo="0",logo="";
            
            DataSet ds = new DataSet();
            DataSet dsinfo = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                string q = "SELECT * from CompanyInfo ";
                dsinfo = objCore.funGetDataSet(q);
                try
                {
                    if (dsinfo.Tables[0].Rows.Count > 0)
                    {
                        cname = (dsinfo.Tables[0].Rows[0]["Name"].ToString());
                        caddress = (dsinfo.Tables[0].Rows[0]["Address"].ToString());
                        cphone = (dsinfo.Tables[0].Rows[0]["Phone"].ToString());
                        logo = (dsinfo.Tables[0].Rows[0]["logo"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    
                   
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
                

            }
            catch (Exception ex)
            {
                
            }
            if (logo == string.Empty)
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, refund, cname, caddress, cphone, date, cashier, RefundNo, null);
            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, refund, cname, caddress, cphone, date, cashier, RefundNo, dsinfo.Tables[0].Rows[0]["logo"]);
            }
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
