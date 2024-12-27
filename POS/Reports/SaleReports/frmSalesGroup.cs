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
    public partial class frmSalesGroup : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public frmSalesGroup()
        {
            InitializeComponent();
           
        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptSalesGroup rptDoc = new SaleReports.rptSalesGroup();
            POSRestaurant.Reports.SaleReports.dssalesgroup ds = new SaleReports.dssalesgroup();
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
                ds.DataTable1.Merge(dt, true, MissingSchemaAction.Ignore);
            }
            else
            {
               
            }                                            
            

            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("Addrs", phone);
            rptDoc.SetParameterValue("phn", address);
            //rptDoc.SetParameterValue("report", "Sales Report");
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;

        }
        
      
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public DataTable getAllOrders()
        {

            DataTable dat = new DataTable();
            dat.Columns.Add("Date", typeof(DateTime));
            dat.Columns.Add("Hot", typeof(double));
            dat.Columns.Add("Cold", typeof(double));
            dat.Columns.Add("Food", typeof(double));
            dat.Columns.Add("Extras", typeof(double));
            dat.Columns.Add("Beans", typeof(double));
            dat.Columns.Add("Fruit", typeof(double));
            dat.Columns.Add("Merchendise", typeof(double));
            dat.Columns.Add("SalesDiscount", typeof(double));
            dat.Columns.Add("Discount", typeof(double));
            dat.Columns.Add("POSSales", typeof(double));
            dat.Columns.Add("NetSales", typeof(double));
            dat.Columns.Add("Tax", typeof(double));
            dat.Columns.Add("POS", typeof(double));
            dat.Columns.Add("POSCC", typeof(double));
            dat.Columns.Add("ActualCC", typeof(double));
            dat.Columns.Add("POSCash", typeof(double));
            dat.Columns.Add("ActualCash", typeof(double));
            dat.Columns.Add("Over", typeof(double));
            dat.Columns.Add("TotalTrans", typeof(double));
            dat.Columns.Add("Avg", typeof(double));
            dat.Columns.Add("logo", typeof(byte[]));

            DataSet ds = new DataSet();
            DataSet dsdate = new DataSet();
            string q = "";
            q = "select distinct date from sale where (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') order by date";
            dsdate = objcore.funGetDataSet(q);

            for (int k = 0; k < dsdate.Tables[0].Rows.Count; k++)
            {
                ds = new DataSet();

                double hot = 0, cold = 0, food = 0, extra = 0, beans = 0, fruit = 0, merch = 0;
                q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date ='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string temp1 = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }

                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "Hot Drinks")
                    {
                        hot = Convert.ToDouble(temp1);
                    }
                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "Cold Drinks")
                    {
                        cold = Convert.ToDouble(temp1);
                    }
                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "Food")
                    {
                        food = Convert.ToDouble(temp1);
                    }
                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "Fruit Smoothie N Chillers")
                    {
                        fruit = Convert.ToDouble(temp1);
                    }
                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "Beans")
                    {
                        beans = Convert.ToDouble(temp1);
                    }
                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "Merchendise")
                    {
                        merch = Convert.ToDouble(temp1);
                    }
                    if (ds.Tables[0].Rows[i]["Name"].ToString() == "Extras")
                    {
                        extra = Convert.ToDouble(temp1);
                    }
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
                double gross = 0, gst = 0, discount = 0, possales = 0, pos = 0, poscc = 0, actualcc = 0, actualcash = 0, net = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0, avg = 0;
                string Dlorders = "0", Torders = "0", Dorders = "0", RefundNo = "0", carhopeorders = "0";

                ds = new DataSet();
                SqlDataAdapter adapter = new SqlDataAdapter();


                string temp = "";
                q = "";
                try
                {
                    ds = new DataSet();
                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount FROM         Sale where (date='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "')  and billstatus='Paid'";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["gross"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        gross = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[0]["gst"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        gst = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[0]["discount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        discount = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[0]["netsale"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        net = Convert.ToDouble(temp);

                        gross = gross + gst;
                        net = Math.Round(net, 2);
                        possales = net;
                        net = net - discount;
                        pos = net + gst;

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
                    q = "";// "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "') and BillType='Cash'";
                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.date='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";
                    ds = objcore.funGetDataSet(q);
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
                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.date='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";
                    ds = objcore.funGetDataSet(q);
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
                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.date='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";
                    ds = objcore.funGetDataSet(q);
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
                    q = "SELECT     *  FROM  Actual where (date='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "')";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["CC"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        
                        actualcc = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        actualcash = Convert.ToDouble(temp);
                    }
                    else
                    {
                        credit = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                poscc = credit + master;                                
                over = actualcash - cash;
                double totlorder = 0;
                try
                {
                    ds = new DataSet();
                    q = "SELECT     count(id) AS cash  FROM         Sale where  (date='" + dsdate.Tables[0].Rows[k]["date"].ToString() + "') and BillStatus='Paid'";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        totlorder = Convert.ToDouble(temp);

                    }
                    else
                    {
                        totlorder = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                double avgsale = 0;
                try
                {
                    if (gross == 0)
                    {
                    }
                    else
                    {
                        avgsale = (gross) / totlorder;
                    }
                }
                catch (Exception ex)
                {


                }
                string sht = "", username = "";
                DataSet dsshft = new DataSet();

                net = Math.Round(net, 2);

                if (logo == "")
                {

                    dat.Rows.Add(dsdate.Tables[0].Rows[k]["date"].ToString(), hot, cold, food, extra, beans, fruit, merch, discount, discount, possales, net, gst, pos, poscc, actualcc, cash, actualcash, over, totlorder, avgsale, null);

                }
                else
                {
                    dat.Rows.Add(dsdate.Tables[0].Rows[k]["date"].ToString(), hot, cold, food, extra, beans, fruit, merch, discount, discount, possales, net, gst, pos, poscc, actualcc, cash, actualcash, over, totlorder, avgsale, dscompany.Tables[0].Rows[0]["logo"]);

                }

            }
            return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            //TextBox txt = textBox1;
            //if (txt.Text == string.Empty)
            //{ }
            //else
            //{
            //    float Num;
            //    bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
            //    if (isNum)
            //    {

            //    }
            //    else
            //    {

            //        MessageBox.Show("Invalid value. Only Nymbers are allowed");
            //        textBox1.Focus();
            //        return;
            //    }
            //}
            //txt = textBox2;
            //if (txt.Text == string.Empty)
            //{ }
            //else
            //{
            //    float Num;
            //    bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
            //    if (isNum)
            //    {

            //    }
            //    else
            //    {

            //        MessageBox.Show("Invalid value. Only Nymbers are allowed");
            //        textBox2.Focus();
            //        return;
            //    }
            //}
            bindreport();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid value. Only Nymbers are allowed");
                    return;
                }
            }
        }
    }
}
