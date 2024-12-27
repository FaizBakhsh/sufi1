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
    public partial class FrmShiftSaleDaterange : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public FrmShiftSaleDaterange()
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
            //else
            //{
            //    if (logo == "")
            //    { }
            //    else
            //    {
            //        dt.Rows.Add("", "", "", dscompany.Tables[0].Rows[0]["logo"]);
            //        ds.Dt1.Merge(dt, true, MissingSchemaAction.Ignore);
            //    }
            //}
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
            rptDoc.SetParameterValue("Addrs",address );
            rptDoc.SetParameterValue("phn", phone);
            rptDoc.SetParameterValue("visa", visa);
            rptDoc.SetParameterValue("visaamounts", visaamounts);
            rptDoc.SetParameterValue("date", "as of  " + dateTimePicker2.Text);
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
                q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.MenuGroup.Name";
                
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


                q = "SELECT     SUM(dbo.Sale.NetBill) AS sum, COUNT(dbo.Sale.NetBill) AS count,SUM(dbo.Sale.DiscountAmount) AS dis, dbo.Users.Name,dbo.Users.id FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id   WHERE     (dbo.Sale.Date between '" + dateTimePicker2.Text + "' and '" + dateTimePicker1.Text + "') and Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'  GROUP BY dbo.Users.Name,dbo.Users.id ";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double refnd = 0, disc = 0;
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
                    total = Math.Round(total, 2);
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
            double gross = 0, gst = 0, discount = 0, net = 0, cash = 0, credit = 0, service = 0, recv = 0, master = 0, dinin = 0, calculatedcash = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
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
                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM    sale where     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + comboBox1.SelectedValue + "' and billstatus='Paid'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        gross = Convert.ToDouble(ds.Tables[0].Rows[0]["gross"].ToString());
                        gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                        discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker2.Text + "' and '" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Cash' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.BillStatus='Paid'";
                   
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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Master Card' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker2.Text + "' and '" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.BillStatus='Paid'";
                   
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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.BillType.type";
                    
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker2.Text + "' and '" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Sale.BillStatus='Paid'";
                   
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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
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
                    q = "SELECT   sum(cashin) as cashin,sum(cashout)  as cashout FROM  shiftcash where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + comboBox1.SelectedValue + "'";
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
                    q = "SELECT     SUM(NetBill) AS cash , count(id) as count FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and OrderType='Delivery'";
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
                    q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and OrderType='Take Away' and BillStatus='Paid'";
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
                    q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and OrderType='Dine In' and BillStatus='Paid'";
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
                    q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and OrderType='Car Hope' and BillStatus='Paid'";
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
                    q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' and shiftid='" + comboBox1.SelectedValue + "'";

                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
                   
                   
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
                        gross = gross + refund;
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
                    q = "SELECT     COUNT(dbo.Saledetails.Id) AS count  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Saledetails.Status = 'Void') and  (dbo.Sale.Date between '" + dateTimePicker2.Text + "' and '" + dateTimePicker1.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'";
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

            }
            catch (Exception ex)
            {

            }

            double totlorder = 0;// (Convert.ToDouble(Torders) + Convert.ToDouble(Dorders) + Convert.ToDouble(Dlorders));
            try
            {
                ds = new DataSet();
                string q = "SELECT     count(id) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Paid' and shiftid='" + comboBox1.SelectedValue + "'";
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
            net = Math.Round(net, 2);
            if (logo == "")
            {
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, null, totlorder, avgsale);
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, comboBox1.Text, Torders, Dorders, RefundNo, null, totlorder, avgsale, carhope, carhopeorders,calculatedcash, drawerfloat, bankingtotal, declared, over, total,service,recv);

            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, comboBox1.Text, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv);
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
            
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            visa = ""; visaamounts = "";
            bindreport();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
