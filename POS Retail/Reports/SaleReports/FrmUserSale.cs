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
    public partial class FrmUserSale : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public FrmUserSale()
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
            //ReportDocument rptDoc = new ReportDocument();
            POSRetail.Reports.SaleReports.rprDailySale rptDoc = new  SaleReports.rprDailySale();
            POSRetail.Reports.SaleReports.DsUserDaily ds = new SaleReports.DsUserDaily();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "Crystal Report";
            dt = getAllOrders();
            ds.Dt.Merge(dt);


           


           
           
            DataTable dtuser = new DataTable();
            dtuser.TableName = "Crystal Report User";
            dtuser = getAllOrdersuser();
            // Just set the name of data table


            ds.DataTable1.Merge(dtuser);

            DataTable dtmenu = new DataTable();
            dtmenu.TableName = "Crystal Report Menu";
            dtmenu = getAllOrdersmenu();
            // Just set the name of data table


            ds.MenuGroup.Merge(dtmenu);

            rptDoc.SetDataSource(ds);

            getcompany();
            string company = "", phone = "", address = "";
            try
            {
                company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                address = dscompany.Tables[0].Rows[0]["Address"].ToString();
            }
            catch (Exception ex)
            {


            }

            
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("Addrs", phone);
            rptDoc.SetParameterValue("phn", address);

            crystalReportViewer1.ReportSource = rptDoc;



        }
        public DataTable getAllOrdersmenu()
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("MenuGroup", typeof(string));
                dtrptmenu.Columns.Add("Count", typeof(string));
                dtrptmenu.Columns.Add("Sum", typeof(string));
                

                DataSet ds = new DataSet();
                string q = "";

                q = "SELECT     SUM(dbo.Saledetails.TotalPrice) AS sum, sum(dbo.Saledetails.Quantity) AS count,  dbo.RawItem.ItemName as name FROM         dbo.Saledetails INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY  dbo.RawItem.ItemName";
                
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
        public DataTable getAllOrdersuser()
        {

            DataTable dtrptuser = new DataTable();
            try
            {
                dtrptuser.Columns.Add("User", typeof(string));
                dtrptuser.Columns.Add("Count", typeof(string));
                dtrptuser.Columns.Add("Sum", typeof(string));


                DataSet ds = new DataSet();
                string q = "";


                q = "SELECT     SUM(dbo.Sale.NetBill) AS sum, COUNT(dbo.Sale.NetBill) AS count, dbo.Users.Name FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  GROUP BY dbo.Users.Name";



                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dtrptuser.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptuser;
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
            
            dat.Columns.Add("RefundNo", typeof(string));
            dat.Columns.Add("date", typeof(string));
            dat.Columns.Add("logo", typeof(Byte[]));

            double  gross=0,gst = 0, discount = 0, net = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0;
            string   Dlorders = "0", Torders = "0", Dorders = "0", RefundNo="0";
            
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
               
                string temp = "";
                string q = "";
                try
                {
                    ds = new DataSet();
                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        gross = Convert.ToDouble(ds.Tables[0].Rows[0]["gross"].ToString());
                        gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                        discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                        net = Convert.ToDouble(ds.Tables[0].Rows[0]["netsale"].ToString());
                        net = net - discount;
                        gross = gross + gst;
                    }
                    else
                    {
                        gross = 0;
                        gst = 0;
                        discount = 0;
                        net = 0;
                    }
                }
                catch (Exception ex)
                {
                    
                   
                }
                try
                {
                    ds = new DataSet();
                    q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Cash'";
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
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    ds = new DataSet();
                    q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Master Card'";
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
                }
                catch (Exception ex)
                {
                    
                   
                }
                try
                {
                    ds = new DataSet();
                    q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Credit Card'";
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
                }
                catch (Exception ex)
                {


                }
                
                
                
                try
                {
                    ds = new DataSet();
                    q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund'";
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
                

            }
            catch (Exception ex)
            {
                
            }
            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }
           
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                if (logo == "")
                {

                    dat.Rows.Add(gross, gst, discount, net, cash, credit, master, refund, RefundNo, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                }
                else
                {



                    dat.Rows.Add(gross, gst, discount, net, cash, credit, master, refund, RefundNo, "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
           

                    

                }

            }
             return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
