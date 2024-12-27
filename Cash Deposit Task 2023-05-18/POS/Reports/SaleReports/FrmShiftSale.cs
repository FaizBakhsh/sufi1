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
    public partial class FrmShiftSale : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public FrmShiftSale()
        {
            InitializeComponent();
           
        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptShiftSale rptDoc = new SaleReports.rptShiftSale();
            POSRestaurant.Reports.SaleReports.DsUserDaily ds = new SaleReports.DsUserDaily();
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
                ds.Dt1.Merge(dt, true, MissingSchemaAction.Ignore);
            }
            else
            {
                if (logo == "")
                { }
                else
                {
                    dt.Rows.Add("", "", "", dscompany.Tables[0].Rows[0]["logo"]);
                    ds.Dt1.Merge(dt, true, MissingSchemaAction.Ignore);
                }
            }                                            
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
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("phn", phone);
            rptDoc.SetParameterValue("visa", visa);
            rptDoc.SetParameterValue("visaamounts", visaamounts);
            rptDoc.SetParameterValue("date", dateTimePicker2.Text);
            rptDoc.SetParameterValue("report", "Shift Wise Sales Report");
            crystalReportViewer1.ReportSource = rptDoc;
        }
        public string visa = "", visaamounts = "";
        public DataTable getAllOrdersmenu()
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("MenuGroup", typeof(string));
                dtrptmenu.Columns.Add("Count", typeof(string));
                dtrptmenu.Columns.Add("Sum", typeof(double));
                

                DataSet ds = new DataSet();
                string q = "";
                if (comboBox2.Text == "All Terminals")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.MenuGroup.Name";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and sale.terminal='"+comboBox2.Text+"' and shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.MenuGroup.Name";
                }
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


                q = "SELECT     SUM(dbo.Sale.NetBill) AS sum, COUNT(dbo.Sale.NetBill) AS count,SUM(dbo.Sale.DiscountAmount) AS dis, dbo.Users.Name,dbo.Users.id FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id   WHERE     (Sale.Date = '" + dateTimePicker2.Text + "') and Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'  GROUP BY dbo.Users.Name,dbo.Users.id ";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double refnd = 0,disc=0;
                    DataSet dsref = new DataSet();
                    //q = "SELECT     sum( NetBill) FROM         Sale where UserId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and BillStatus='Refund'";
                    //q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "')   and dbo.Sale.UserId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and BillStatus='Refund'";
                    //dsref = objCore.funGetDataSet(q);
                    //if (dsref.Tables[0].Rows.Count > 0)
                    //{
                    //    string temp = dsref.Tables[0].Rows[0]["cash"].ToString();
                    //    if (temp == "")
                    //    {
                    //        temp = "0";
                    //    }
                    //    refnd = Convert.ToDouble(temp);
                    //}
                    string val = ds.Tables[0].Rows[i]["dis"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    disc = Convert.ToDouble(val);
                    val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    double total = 0;
                    try
                    {
                        total = sum - refnd;
                        //total = total - disc;
                    }
                    catch (Exception ex)
                    {

                    }
                    total = Math.Round(total,2);
                    dtrptuser.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), total.ToString());
                    //dtrptuser.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptuser;
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

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
            dat.Columns.Add("Dlorders", typeof(string));
            dat.Columns.Add("Torders", typeof(string));
            dat.Columns.Add("Dorders", typeof(string));
            dat.Columns.Add("RefundNo", typeof(string));
            dat.Columns.Add("logo", typeof(byte[]));
            dat.Columns.Add("totalorders", typeof(double));
            dat.Columns.Add("averagesale", typeof(double));
            dat.Columns.Add("CarHope", typeof(double));
            dat.Columns.Add("CarHopeorders", typeof(string));
            dat.Columns.Add("calculatedcash", typeof(double));
            dat.Columns.Add("float", typeof(double));
            dat.Columns.Add("bankingtotal", typeof(double));
            dat.Columns.Add("declared", typeof(double));
            dat.Columns.Add("over", typeof(double));
            dat.Columns.Add("total", typeof(double));
            dat.Columns.Add("servicecharges", typeof(double));
            dat.Columns.Add("receivables", typeof(double));
            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }
            double gross = 0, gst = 0, discount = 0, net = 0, cash = 0, credit = 0, service = 0, recv = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
            string Dlorders = "0", Torders = "0", Dorders = "0", RefundNo = "0", carhopeorders = "0";

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {

                string temp = "";
                string q = "";
                try
                {
                    ds = new DataSet();
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and shiftid='" + comboBox1.SelectedValue + "' and billstatus='Paid'";
                        try
                        {
                            string q1 = "SELECT        SUM(dbo.DiscountIndividual.Discount) AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid  where  (dbo.Sale.Date = '" + dateTimePicker2.Text + "')  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'    and dbo.Sale.billstatus='Paid'";
                            DataSet dsdiscount = new DataSet();
                            dsdiscount = objCore.funGetDataSet(q1);
                            if (dsdiscount.Tables[0].Rows.Count > 0)
                            {
                                temp = dsdiscount.Tables[0].Rows[0]["Expr1"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                discount = Convert.ToDouble(temp);
                                temp = "";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    else
                    {
                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and terminal='"+comboBox2.Text+"' and shiftid='" + comboBox1.SelectedValue + "' and billstatus='Paid'";
                        try
                        {
                            string q1 = "SELECT        SUM(dbo.DiscountIndividual.Discount) AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid  where  (dbo.Sale.Date = '" + dateTimePicker2.Text + "')  and dbo.Sale.terminal='" + comboBox2.Text + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'    and dbo.Sale.billstatus='Paid'";
                            DataSet dsdiscount = new DataSet();
                            dsdiscount = objCore.funGetDataSet(q1);
                            if (dsdiscount.Tables[0].Rows.Count > 0)
                            {
                                temp = dsdiscount.Tables[0].Rows[0]["Expr1"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                discount = Convert.ToDouble(temp);
                                temp = "";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        gross = Convert.ToDouble(ds.Tables[0].Rows[0]["gross"].ToString());
                        gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                        discount =discount+ Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                        net = Convert.ToDouble(ds.Tables[0].Rows[0]["netsale"].ToString());
                        net = net - discount;
                        gross = gross + gst;
                        net = Math.Round(net, 2);
                        service = Convert.ToDouble(ds.Tables[0].Rows[0]["serv"].ToString());
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
                    q = "";// "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Cash'";
                    if (comboBox2.Text == "All Terminals")
                    {

                        q = "SELECT    SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT    SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and shiftid='" + comboBox1.SelectedValue + "' and terminal='" + comboBox2.Text + "' and dbo.Sale.BillStatus='Paid'";
               
                    }
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Cash' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.BillStatus='Paid'";
                   
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cash = Convert.ToDouble(temp);
                        calculatedcash = cash;
                        //cash = cash - discount;
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
                    //q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Master Card'";
                    if (comboBox2.Text == "All Terminals")
                    {

                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.BillType.type='Master Card'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.BillType.type='Master Card'";
                    }
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.BillStatus='Paid'";
                   
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
                    // q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and BillType='Credit Card'";
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.BillType.type";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.terminal='" + comboBox2.Text + "' GROUP BY dbo.BillType.type";
                    
                    }
                        //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.BillStatus='Paid'";
                   
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            temp = ds.Tables[0].Rows[i]["cash"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            credit = credit + Convert.ToDouble(temp);
                            if (visa.Length > 0)
                            {
                                visa = visa + " , ";
                                visaamounts = visaamounts + " , ";
                            }
                            visa = visa + ds.Tables[0].Rows[i]["type"].ToString();
                            visaamounts = visaamounts + Math.Round(Convert.ToDouble(temp), 2);
                        }
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
                    // q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and BillType='Credit Card'";
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable' and dbo.Sale.terminal='"+comboBox2.Text+"' and dbo.Sale.BillStatus='Paid'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
                  
                    }
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";

                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        recv = Convert.ToDouble(temp);
                    }
                    else
                    {
                        recv = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT   sum(cashin) as cashin,sum(cashout)  as cashout FROM  shiftcash where  (Date = '" + dateTimePicker2.Text + "') and shiftid='" + comboBox1.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT   sum(cashin) as cashin,sum(cashout)  as cashout FROM  shiftcash where  (Date = '" + dateTimePicker2.Text + "') and terminal='" + comboBox2.Text + "' and shiftid='" + comboBox1.SelectedValue + "'";
                 
                    }
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["cashin"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        drawerfloat = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[0]["cashout"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        declared = Convert.ToDouble(temp);
                        calculatedcash = calculatedcash + drawerfloat;
                        total = calculatedcash - drawerfloat;
                        over = declared - calculatedcash;
                        bankingtotal = declared - drawerfloat;
                        
                    }

                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox2.Text == "All Terminals")
                    {

                        q = "SELECT     SUM(NetBill) AS cash , count(id) as count FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and OrderType='Delivery'";
                    }
                    else
                    {
                        q = "SELECT     SUM(NetBill) AS cash , count(id) as count FROM         Sale where terminal='"+comboBox2.Text+"' and (Date = '" + dateTimePicker2.Text + "') and OrderType='Delivery'";
                    
                    }

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
                        delivery = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        Dlorders = ds.Tables[0].Rows[0]["count"].ToString();
                    }
                    else
                    {
                        delivery = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and OrderType='Take Away' and BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and terminal='"+comboBox2.Text+"' and OrderType='Take Away' and BillStatus='Paid'";
                    
                    }
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
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and OrderType='Din In' and BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and terminal='"+comboBox2.Text+"' and OrderType='Din In' and BillStatus='Paid'";
                   
                    }
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
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and OrderType='Car Hope' and BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where terminal='"+comboBox2.Text+"' (Date = '" + dateTimePicker2.Text + "') and OrderType='Car Hope' and BillStatus='Paid'";
                    }
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
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    

                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Refund' and (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date ='" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox2.Text + "' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";

                    }
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["Expr1"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        refund = Convert.ToDouble(temp);
                        //gross = gross + refund;
                        
                    }
                    else
                    {
                        refund = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox2.Text == "All Terminals")
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Void' and (dbo.Sale.Date = '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
                    }
                    else
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date ='" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox2.Text + "' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";

                    }
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // voidsale = Convert.ToDouble(ds.Tables[0].Rows[0]["count"].ToString());
                        temp = ds.Tables[0].Rows[0]["Expr1"].ToString();
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

            }
            catch (Exception ex)
            {

            }
            double totlorder = 0;// (Convert.ToDouble(Torders) + Convert.ToDouble(Dorders) + Convert.ToDouble(Dlorders));
            try
            {
                ds = new DataSet();
                string q = "";
                if (comboBox2.Text == "All Terminals")
                {
                    q = "SELECT     count(id) AS cash  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and BillStatus='Paid' and shiftid='" + comboBox1.SelectedValue + "'";
                }
                else
                {
                    q = "SELECT     count(id) AS cash  FROM         Sale where  terminal='"+comboBox2.Text+"' and (Date = '" + dateTimePicker2.Text + "') and BillStatus='Paid' and shiftid='" + comboBox1.SelectedValue + "'";
                
                }
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
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
                    avgsale = (net) / totlorder;
                }
            }
            catch (Exception ex)
            {


            }
            string sht = "", username = "";
            DataSet dsshft = new DataSet();
            dsshft = objCore.funGetDataSet("select * from Shifts where id='" + comboBox1.SelectedValue + "'");
            if (dsshft.Tables[0].Rows.Count > 0)
            {
                sht = dsshft.Tables[0].Rows[0]["Name"].ToString();
            }
            dsshft = new DataSet();
            //dsshft = objcore.funGetDataSet("select * from users where id='" + user + "'");
            //if (dsshft.Tables[0].Rows.Count > 0)
            //{
            //    username = dsshft.Tables[0].Rows[0]["Name"].ToString();
            //}
            net = Math.Round(net, 2);
            if (logo == "")
            {
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, null, totlorder, avgsale);
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, null, totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total,service,recv);

            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv);
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale);
            }


            return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet ds = new DataSet();
                string q = "select id,name from shifts ";
                ds = objCore.funGetDataSet(q);
                //DataRow dr = ds.Tables[0].NewRow();
                //dr["name"] = "All Shifts";
                //ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select distinct Terminal from sale ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["Terminal"] = "All Terminals";
                ds1.Tables[0].Rows.Add(dr1);
                comboBox2.DataSource = ds1.Tables[0];
                comboBox2.ValueMember = "Terminal";
                comboBox2.DisplayMember = "Terminal";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            } 
        }
        public void updatebilltype()
        {
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_1 where date = '" + dateTimePicker2.Text + "'";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "insert into billtype (type,saleid,amount) values('" + type + "','" + id + "','" + amount + "')";
                    objCore.executeQuery(q);
                    q = "update sale set uploadstatusserver='Pending' where id='" + id + "'";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
            updatebilltype2();
        }
        public void updatebilltype2()
        {

            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_2 where date  = '" + dateTimePicker2.Text + "'";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "update billtype set amount='" + amount + "' where saleid='" + id + "'";
                    objCore.executeQuery(q);
                    q = "update sale set uploadstatusserver='Pending' where id='" + id + "'";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        protected string type(string id)
        {
            string typ = "";
            try
            {
                DataSet dsdetail = new DataSet();
                string q = "select OrderType from sale where id='" + id + "' ";
                dsdetail = objCore.funGetDataSet(q);
                if (dsdetail.Tables[0].Rows.Count > 0)
                {
                    string temp = dsdetail.Tables[0].Rows[0]["OrderType"].ToString();
                    if (temp == "")
                    {
                        temp = "0";

                    }
                    typ = temp;
                }
            }
            catch (Exception ex)
            {


            }

            return typ;
        }
        protected double getdiscountinddetails(string sid)
        {
            double amount = 0;
            try
            {
                string q = "select sum(Discount) from DiscountIndividual where Saledetailsid='" + sid + "'";
                DataSet dsf = new DataSet();
                dsf = objCore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    amount = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return amount;
        }
        public string getordertype(string id)
        {
            string type = "";
            DataSet dstype = new DataSet();
            try
            {
                string q = "select OrderType from sale where id='" + id + "'";

                dstype = objCore.funGetDataSet(q);
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    type = dstype.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //dstype.Dispose();
            }
            return type;
        }
        public void updatebilltype3()
        {

            double servicecharhes = 0;
            DataSet dsgst = new DataSet();
            try
            {

                dsgst = objCore.funGetDataSet("select * from SerivceCharges");
                if (dsgst.Tables[0].Rows.Count > 0)
                {
                    servicecharhes = float.Parse(dsgst.Tables[0].Rows[0]["charges"].ToString());
                }
                else
                {
                    servicecharhes = 0;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsgst.Dispose();
            }
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_3 where sum!=netbill";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    double price = 0, gst = 0, dis = 0, net = 0, disp = 0;
                    string id = dsbill.Tables[0].Rows[i]["saleid"].ToString();
                    DataSet dsdetail = new DataSet();
                    q = "select * from saledetails where saleid='" + id + "'";
                    dsdetail = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsdetail.Tables[0].Rows.Count; j++)
                    {
                        double discount = 0, gstt = 0, scarges = 0, price0 = 0;
                        try
                        {
                            string val = dsdetail.Tables[0].Rows[j]["price"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            price0 = Convert.ToDouble(val);
                            scarges = (price0 * servicecharhes) / 100;
                            scarges = Math.Round(scarges, 2);
                            val = dsdetail.Tables[0].Rows[j]["ItemdiscountPerc"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            string ordertyppe = "";
                            ordertyppe = getordertype(id);

                            if (ordertyppe == "Take Away")
                            {
                                scarges = 0;
                            }
                            double dis0 = Convert.ToDouble(val);
                            if (dis0 > 0 && price0 > 0)
                            {
                                discount = (price0 * dis0) / 100;
                                discount = Math.Round(discount, 2);
                            }
                            val = "";
                            val = dsdetail.Tables[0].Rows[j]["ItemGstPerc"].ToString(); ;
                            if (val == "")
                            {
                                val = "0";
                            }
                            gstt = Convert.ToDouble(val);
                            if (applydiscount() == "before")
                            {

                                if (gstt > 0 && price0 > 0)
                                {
                                    gstt = ((price0 + scarges) * gstt) / 100;
                                    gstt = Math.Round(gstt, 2);
                                }
                                else
                                {
                                    gstt = 0;
                                }
                            }
                            else
                            {
                                if (gstt > 0 && price0 > 0)
                                {
                                    gstt = ((price0 - discount) * gstt) / 100;
                                    gstt = Math.Round(gstt, 2);
                                }
                                else
                                {
                                    gstt = 0;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        double inddisdet = 0;
                        try
                        {
                            inddisdet = getdiscountinddetails(dsdetail.Tables[0].Rows[j]["id"].ToString());
                        }
                        catch (Exception)
                        {

                        }
                        if (inddisdet > 0)
                        {
                            q = "update saledetails set ItemGst='" + gstt + "',Itemdiscount='" + inddisdet + "' where id='" + dsdetail.Tables[0].Rows[j]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "update saledetails set ItemGst='" + gstt + "' where id='" + dsdetail.Tables[0].Rows[j]["id"].ToString() + "'";

                        }
                        objCore.executeQuery(q);
                        string temp = dsdetail.Tables[0].Rows[j]["price"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }

                        price = price + Convert.ToDouble(temp);

                        gst = gst + Convert.ToDouble(gstt);
                        temp = dsdetail.Tables[0].Rows[j]["Itemdiscount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }
                        if (inddisdet > 0)
                        {
                            dis = dis + inddisdet;
                        }
                        else
                        {
                            dis = dis + Convert.ToDouble(temp);
                        }
                        temp = dsdetail.Tables[0].Rows[j]["ItemdiscountPerc"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }
                        disp = Convert.ToDouble(temp);
                    }
                    double inddis = 0;
                    try
                    {
                        //inddis = getdiscountind(id);
                    }
                    catch (Exception)
                    {

                    }
                    double service = 0, serviceamount = 0;
                    if (type(id) == "Dine In")
                    {
                        dsdetail = new DataSet();
                        q = "select * from SerivceCharges ";
                        dsdetail = objCore.funGetDataSet(q);
                        if (dsdetail.Tables[0].Rows.Count > 0)
                        {
                            string temp = dsdetail.Tables[0].Rows[0]["charges"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            service = Convert.ToDouble(temp);
                        }
                        try
                        {
                            if (applydiscount() == "before")
                            {
                                serviceamount = ((price) * service) / 100;
                            }
                            else
                            {
                                serviceamount = ((price - dis) * service) / 100;
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        serviceamount = Math.Round(serviceamount, 2);
                    }
                    // MessageBox.Show(((price + gst + serviceamount) - (dis)).ToString()+"-"+id.ToString());
                    q = "update sale set TotalBill='" + price + "',DiscountAmount='" + dis + "',GST='" + gst + "',netbill='" + ((price + gst + serviceamount) - (dis)) + "',discount='" + disp + "',servicecharges='" + serviceamount + "' where id='" + id + "'";
                    objCore.executeQuery(q);
                    q = "select * from billtype where saleid='" + id + "'";
                    dsbill = new System.Data.DataSet();
                    dsbill = objCore.funGetDataSet(q);
                    if (dsbill.Tables[0].Rows.Count == 1)
                    {

                        q = "update billtype set amount='" + ((price + gst + serviceamount) - (dis)) + "' where saleid='" + id + "'";
                        objCore.executeQuery(q);
                        //MessageBox.Show(q);
                    }
                    if (dsbill.Tables[0].Rows.Count == 2)
                    {
                        double vsaamount = 0;
                        for (int l = 0; l < dsbill.Tables[0].Rows.Count; l++)
                        {
                            if (dsbill.Tables[0].Rows[l]["type"].ToString() == "Cash")
                            {

                            }
                            else
                            {
                                string temp = dsbill.Tables[0].Rows[l]["amount"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                vsaamount = Convert.ToDouble(temp);
                            }
                        }
                        q = "update billtype set amount='" + (((price + gst + serviceamount) - (dis + inddis)) - vsaamount) + "' where saleid='" + id + "' and type='cash'";
                        objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        protected double getdiscountind(string sid)
        {
            double amount = 0;
            try
            {
                string q = "select sum(Discount) from DiscountIndividual where saleid='" + sid + "'";
                DataSet dsf = new DataSet();
                dsf = objCore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    amount = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return amount;
        }
        public string applydiscount()
        {
            string apply = "before";
            DataSet dsdis = new DataSet();
            try
            {
                string q = "select * from applydiscount ";

                dsdis = objCore.funGetDataSet(q);
                if (dsdis.Tables[0].Rows.Count > 0)
                {
                    apply = dsdis.Tables[0].Rows[0]["apply"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsdis.Dispose();
            }
            if (apply == "")
            {
                apply = "before";
            }
            return apply;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
          
            //updatebilltype3();
            updatebilltype();
            updatebilltype31();
            visa = ""; visaamounts = "";
            bindreport();
        }
        public void updatebilltype31()
        {

            double servicecharhes = 0;
            DataSet dsgst = new DataSet();

            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_3 where (sum IS NULL) AND (netbill > 0)";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    double price = 0, gst = 0, dis = 0, net = 0, disp = 0;
                    string id = dsbill.Tables[0].Rows[i]["saleid"].ToString();
                    DataSet dsdetail = new DataSet();
                    q = "select * from saledetails where saleid='" + id + "'";
                    dsdetail = objCore.funGetDataSet(q);
                    if (dsdetail.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        q = "update sale set TotalBill='0',DiscountAmount='0',GST='0',netbill='0',discount='0',servicecharges='0',UploadStatus='Pending',uploadstatusbilltype='Pending',uploadstatusrefund='pending',uploadstatusserver='Pending' where id='" + id + "'";
                        objCore.executeQuery(q);
                        q = "update billtype set amount='0' where saleid='" + id + "'";
                        objCore.executeQuery(q);
                    }


                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
            dsbill = new DataSet();
            try
            {
                string q = "select * from View_3 where (sum!=netbill)";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    double price = 0, gst = 0, dis = 0, net = 0, disp = 0;
                    string id = dsbill.Tables[0].Rows[i]["saleid"].ToString();
                    DataSet dsdetail = new DataSet();
                    q = "select * from saledetails where saleid='" + id + "'";
                    dsdetail = objCore.funGetDataSet(q);
                    if (dsdetail.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsdetail.Tables[0].Rows.Count; j++)
                        {

                            string temp = dsdetail.Tables[0].Rows[j]["Itemdiscount"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            dis = dis + Convert.ToDouble(temp);
                            temp = dsdetail.Tables[0].Rows[j]["Itemgst"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            gst = gst + Convert.ToDouble(temp);
                            temp = dsdetail.Tables[0].Rows[j]["price"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            price = price + Convert.ToDouble(temp);
                        }
                        q = "update sale set TotalBill='" + price + "',DiscountAmount='" + dis + "',GST='" + gst + "',netbill='" + ((price + gst) - (dis)) + "' where id='" + id + "'";
                        objCore.executeQuery(q);
                        q = "update sale set netbill=netbill+servicecharges where id='" + id + "'";
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        q = "update sale set TotalBill='0',DiscountAmount='0',GST='0',netbill='0',discount='0',servicecharges='0',UploadStatus='Pending',uploadstatusbilltype='Pending',uploadstatusrefund='pending',uploadstatusserver='Pending' where id='" + id + "'";
                        objCore.executeQuery(q);
                        q = "update billtype set amount='0' where saleid='" + id + "'";
                        objCore.executeQuery(q);
                    }


                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        public void updatebilltype4()
        {
            DataSet dsbill = new DataSet();
            try
            {
                string q = "SELECT        id, charges  FROM            SerivceCharges";
                dsbill = objCore.funGetDataSet(q);
                if (dsbill.Tables[0].Rows.Count > 0)
                {
                    string temp = dsbill.Tables[0].Rows[0]["charges"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    float charges = float.Parse(temp);
                    if (charges > 0)
                    {
                        q = "select * from sale where servicecharges > 0   and date >= '2018-11-04' order by id desc";
                        dsbill = new DataSet();
                        dsbill = objCore.funGetDataSet(q);
                        for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                        {
                            temp = "0";
                            string otype = dsbill.Tables[0].Rows[i]["OrderType"].ToString();
                            string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                            double servicecharges = 0, totalbill = 0, gst = 0, totalgst = 0, dscount = 0, discountedtotal = 0, service = 0, nettotal = 0;
                            temp = dsbill.Tables[0].Rows[i]["totalbill"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            totalbill = Convert.ToDouble(temp);
                            temp = dsbill.Tables[0].Rows[i]["gstperc"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            gst = Convert.ToDouble(temp);

                            temp = dsbill.Tables[0].Rows[i]["discount"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            dscount = Convert.ToDouble(temp);
                            dscount = (dscount * totalbill) / 100;
                            dscount = Math.Round(dscount, 2);

                            // if (otype == "Dine In")
                            {



                                if (applydiscount() == "before")
                                {
                                    discountedtotal = totalbill;// -dscount;
                                    service = (charges * discountedtotal) / 100;
                                    service = Math.Round(service, 2);
                                    if (otype == "Take Away")
                                    {
                                        service = 0;
                                    }
                                    discountedtotal = discountedtotal + service;
                                    totalgst = (gst * discountedtotal) / 100;
                                    totalgst = Math.Round(totalgst, 2);
                                    discountedtotal = discountedtotal - dscount;
                                }
                                else
                                {
                                    discountedtotal = totalbill - dscount;
                                    service = (charges * discountedtotal) / 100;
                                    service = Math.Round(service, 2);
                                    if (otype == "Take Away")
                                    {
                                        service = 0;
                                    }
                                    totalgst = (gst * discountedtotal) / 100;
                                    totalgst = Math.Round(totalgst, 2);
                                    discountedtotal = discountedtotal + service;
                                }
                                discountedtotal = Math.Round(discountedtotal, 2);

                                nettotal = Math.Round((totalgst + discountedtotal), 2);
                                q = "update sale set discountamount='" + dscount + "',servicecharges='" + service + "',NetBill='" + nettotal + "',gst='" + totalgst + "' where id='" + id + "'";
                                objCore.executeQuery(q);
                                q = "update billtype set amount='" + nettotal + "' where saleid='" + id + "'";
                                objCore.executeQuery(q);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
