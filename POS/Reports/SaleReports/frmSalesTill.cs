using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OposPOSPrinter_CCO;
using System.Data.SqlClient;
using POSRestaurant.Sale;
using System.Runtime.InteropServices;
using System.IO;
using POSRestaurant.Reports.SaleReports;
namespace POSRestaurant.Reports
{
    public partial class frmSalesTill : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmSalesTill()
        {
            InitializeComponent();

        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
           // POSRestaurant.Reports.SaleReports.rptShiftSalepos rptDoc = new SaleReports.rptShiftSalepos();

            POSRestaurant.Reports.SaleReports.rptdaily rptDoc = new SaleReports.rptdaily();
         
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
            rptDoc.SetParameterValue("Addrs", phone);
            rptDoc.SetParameterValue("phn", address);
            rptDoc.SetParameterValue("visa", visa);
            rptDoc.SetParameterValue("visaamounts", visaamounts);
            rptDoc.SetParameterValue("report", "Sales Report");
            deliverysource();
            rptDoc.SetParameterValue("deliverysourcetitle", delievrysourcetitle);
            rptDoc.SetParameterValue("deliverysourcedata", delievrysourcedata);
            if (takawayorders == "")
            {
                takawayorders = "0"; 
            }
            rptDoc.SetParameterValue("takeawayorders", takawayorders);
            if (dineinorders == "")
            {
                dineinorders = "0";
            }
            rptDoc.SetParameterValue("dineinorders", dineinorders);
            if (deliveryorders == "")
            {
                deliveryorders = "0";
            }
            rptDoc.SetParameterValue("deliveryorders", deliveryorders);
            rptDoc.SetParameterValue("date", dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            rptDoc.SetParameterValue("recvname", "");
            rptDoc.SetParameterValue("recvamount", "");
            crystalReportViewer1.ReportSource = rptDoc;

        }
        public string visa = "", visaamounts = "";
        string delievrysourcetitle = "", delievrysourcedata = "";
        protected void deliverysource()
        {
            string val = "";
            string q = "";

            try
            {
                q = "SELECT        SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'  GROUP BY dbo.Delivery.type ";
                   
                DataSet dssource = new DataSet();
                dssource = objcore.funGetDataSet(q);
                if (dssource.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dssource.Tables[0].Rows.Count; i++)
                    {


                        if (delievrysourcetitle.Length > 0)
                        {
                            delievrysourcetitle = delievrysourcetitle + " ,";
                        }
                        delievrysourcetitle = delievrysourcetitle + dssource.Tables[0].Rows[i]["type"].ToString();

                        if (delievrysourcedata.Length > 0)
                        {
                            delievrysourcedata = delievrysourcedata + " ,";
                        }
                        string temp = dssource.Tables[0].Rows[i]["amount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        delievrysourcedata = delievrysourcedata + Math.Round(Convert.ToDouble(temp), 2).ToString();

                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
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
                if (comboBox1.Text == "All")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.Billstatus='Paid' GROUP BY dbo.MenuGroup.Name";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE   sale.userid='' and  (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.Billstatus='Paid' GROUP BY dbo.MenuGroup.Name";
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


                q = "SELECT     SUM(dbo.Sale.NetBill) AS sum, COUNT(dbo.Sale.NetBill) AS count,SUM(dbo.Sale.DiscountAmount) AS dis, dbo.Users.Name,dbo.Users.id FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='Paid'  GROUP BY dbo.Users.Name,dbo.Users.id ";
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
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        string  takawayorders = "", dineinorders = "", deliveryorders = "";
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
            dat.Columns.Add("cashgst", typeof(double));
            dat.Columns.Add("cardgst", typeof(double));
            dat.Columns.Add("CashDiscount", typeof(double));
            dat.Columns.Add("CardDiscount", typeof(double));
            dat.Columns.Add("CashRecv", typeof(double));
            dat.Columns.Add("CardRecv", typeof(double));
            dat.Columns.Add("avgdinein", typeof(double));
            dat.Columns.Add("avgdineintable", typeof(double));
            dat.Columns.Add("DlvCharges", typeof(double));
            dat.Columns.Add("Comp", typeof(double));
            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }
            double gross = 0, cashgst = 0, visagst = 0, DlvCharges = 0, gst = 0, complimtry=0, cashdis = 0, carddis = 0, discount = 0, net = 0, cashrecv = 0, cardrecv = 0, cash = 0, credit = 0, master = 0, recvble = 0, service = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
            string Dlorders = "0", Torders = "0", Dorders = "0", RefundNo = "0", carhopeorders = "0";
            complimtry = getcomplimentary();
            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {

                string temp = "";
                string q = "";
                try
                {
                    ds = new DataSet();
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst,SUM(servicecharges) AS service, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount, SUM(DeliveryAmt) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst,SUM(servicecharges) AS service, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount, SUM(DeliveryAmt) AS DlvCharges FROM         Sale where userid='" + comboBox1.SelectedValue + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";
                    }
                        ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                       
                       
                        //try
                        //{
                        //    string q1 = "SELECT        SUM(dbo.DiscountIndividual.Discount) AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid  where  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and dbo.Sale.billstatus='Paid'";
                        //    DataSet dsdiscount = new DataSet();
                        //    dsdiscount = objCore.funGetDataSet(q1);
                        //    if (dsdiscount.Tables[0].Rows.Count > 0)
                        //    {
                        //        temp = dsdiscount.Tables[0].Rows[0]["Expr1"].ToString();
                        //        if (temp == "")
                        //        {
                        //            temp = "0";
                        //        }
                        //        discount = Convert.ToDouble(temp);
                        //        temp = "";
                        //    }
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                        gross = Convert.ToDouble(ds.Tables[0].Rows[0]["gross"].ToString());
                        gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                        discount =discount+ Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                        net = Convert.ToDouble(ds.Tables[0].Rows[0]["netsale"].ToString());
                        try
                        {
                            DlvCharges = Convert.ToDouble(ds.Tables[0].Rows[0]["DlvCharges"].ToString());
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        service = Convert.ToDouble(ds.Tables[0].Rows[0]["service"].ToString());
                        net = net - discount;
                        gross = gross + gst + service + DlvCharges+complimtry;
                        net = Math.Round(net, 2);
                        try
                        {
                            if (comboBox1.Text == "All")
                            {
                                q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";
                            }
                            else
                            {
                                q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  userid='" + comboBox1.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";
                           
                            }
                            DataSet dscash = new DataSet();
                            dscash = objcore.funGetDataSet(q);
                            if (dscash.Tables[0].Rows.Count > 0)
                            {
                                temp = dscash.Tables[0].Rows[0]["gst"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                cashgst = Convert.ToDouble(temp);
                                temp = dscash.Tables[0].Rows[0]["discount"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                cashdis = Convert.ToDouble(temp);
                            }
                            if (comboBox1.Text == "All")
                            {
                                q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst,SUM(servicecharges) AS service, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";
                            }
                            else
                            {
                                q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst,SUM(servicecharges) AS service, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount FROM         Sale where userid='"+comboBox1.SelectedValue+"' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";
                           
                            }
                            dscash = new DataSet();
                            dscash = objcore.funGetDataSet(q);
                            if (dscash.Tables[0].Rows.Count > 0)
                            {
                                temp = dscash.Tables[0].Rows[0]["gst"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                visagst = Convert.ToDouble(temp);
                                temp = dscash.Tables[0].Rows[0]["discount"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                carddis = Convert.ToDouble(temp);
                            }
                        }
                        catch (Exception ex)
                        {
                            
                           
                        }

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

                    // q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  sale.userid='" + comboBox1.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                 
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cash = Convert.ToDouble(temp);
                        //calculatedcash = cash;
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
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, count(dbo.BillType.id) as count  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                        //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, count(dbo.BillType.id) as count  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  sale.userid='" + comboBox1.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                     
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        //cash = Convert.ToDouble(temp);
                        calculatedcash = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[0]["count"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cashp = temp;
                        //cash = cash - discount;
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Master Card'  and dbo.Sale.BillStatus='Paid'";
                        //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {

                    }
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
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                        //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where sale.userid='" + comboBox1.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                     
                    }
                    ds = objcore.funGetDataSet(q);
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
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Receivable'  and dbo.Sale.BillStatus='Paid'";
                        //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  sale.userid='" + comboBox1.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Receivable'  and dbo.Sale.BillStatus='Paid'";
                   
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        recvble = Convert.ToDouble(temp);
                    }
                    else
                    {
                        recvble = 0;
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    ds = new DataSet();
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Receivable'  and dbo.Sale.BillStatus='Paid'  and dbo.Sale.GSTtype='Cash'";
                        //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  sale.userid='" + comboBox1.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Receivable'  and dbo.Sale.BillStatus='Paid'  and dbo.Sale.GSTtype='Cash'";
                   
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cashrecv = Convert.ToDouble(temp);
                    }
                    else
                    {
                        cashrecv = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Receivable'  and dbo.Sale.BillStatus='Paid'  and dbo.Sale.GSTtype='Card'";
                        //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where sale.userid='"+comboBox1.SelectedValue+"' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Receivable'  and dbo.Sale.BillStatus='Paid'  and dbo.Sale.GSTtype='Card'";
                    
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cardrecv = Convert.ToDouble(temp);
                    }
                    else
                    {
                        cardrecv = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();

                    q = "SELECT  SUM(cashin) AS cashin,SUM(cashout) AS cashout  FROM  shiftcash where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        drawerfloat = Convert.ToDouble(ds.Tables[0].Rows[0]["cashin"].ToString());
                        declared = Convert.ToDouble(ds.Tables[0].Rows[0]["cashout"].ToString());
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
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' ";
                    }
                    else
                    {
                        q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where userid='"+comboBox1.SelectedValue+"' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' ";
                    
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        refund = Convert.ToDouble(temp);
                        gross = gross + refund;
                        // net = net - refund;
                        temp = ds.Tables[0].Rows[0]["count"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        //gross = gross - refund;
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
                try
                {
                    ds = new DataSet();
                    q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' ";
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Refund' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where sale.userid='" + comboBox1.SelectedValue + "' and  dbo.Saledetailsrefund.type='Refund' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                    
                    }
                    ds = objcore.funGetDataSet(q);
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
                        // net = net - refund;

                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Void' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                    }
                    else
                    {
                        q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where sale.userid='"+comboBox1.SelectedValue+"' and dbo.Saledetailsrefund.type='Void' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                 
                    }
                    ds = objcore.funGetDataSet(q);
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
            double avgdinein = 0, avgdineintable = 0;
            try
            {
                ds = new DataSet();
                string q = "";
                if (comboBox1.Text == "All")
                {
                    q = "SELECT    sum(NetBill) as cash, count(id) AS count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and OrderType='Dine In'   AND (BillStatus = 'Paid')";
                }
                else
                {
                    q = "SELECT    sum(NetBill) as cash, count(id) AS count  FROM         Sale where  userid='" + comboBox1.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and OrderType='Dine In'   AND (BillStatus = 'Paid')";
              
                }
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
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
                    dineinp = (temp);
                    dineinorders = temp;
                    try
                    {
                        if (comboBox1.Text == "All")
                        {
                            q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                        }
                        else
                        {
                            q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where  sale.userid='" + comboBox1.SelectedValue + "' and   (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                    
                        }
                        ds = new DataSet();
                        ds = objcore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            temp = ds.Tables[0].Rows[0]["count"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            avgdineintable = dinin / Convert.ToDouble(dineinorders);
                            dineinorders = dineinorders + "/" + temp;
                            avgdinein = dinin / Convert.ToDouble(temp);
                        }

                    }
                    catch (Exception ex)
                    {


                    }

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
                string q = "";
                if (comboBox1.Text == "All")
                {
                    q = "SELECT    sum(NetBill) as cash, count(id) AS count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and OrderType='Take Away'   AND (BillStatus = 'Paid')";
                }
                else
                {
                    q = "SELECT    sum(NetBill) as cash, count(id) AS count  FROM         Sale where userid='" + comboBox1.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and OrderType='Take Away'   AND (BillStatus = 'Paid')";
               
                }
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
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
                    takeawayp = (temp);
                    takawayorders = temp;
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
                string q = "";
                if (comboBox1.Text == "All")
                {
                    q = "SELECT    sum(NetBill) as cash, count(id) AS count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and OrderType='Delivery'   AND (BillStatus = 'Paid')";
                }

                else
                {
                    q = "SELECT    sum(NetBill) as cash, count(id) AS count  FROM         Sale where userid='" + comboBox1.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and OrderType='Delivery'   AND (BillStatus = 'Paid')";
               
                }
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
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
                    deliveryp = (temp);
                    deliveryorders = temp;
                }
                else
                {
                    delivery = 0;
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
                if (comboBox1.Text == "All")
                {
                    q = "SELECT     count(id) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and billstatus='Paid'";
                }
                else
                {
                    q = "SELECT     count(id) AS cash  FROM         Sale where userid='"+comboBox1.SelectedValue+"' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and billstatus='Paid'";
           
                }
                ds = objcore.funGetDataSet(q);
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
            dsshft = objcore.funGetDataSet("select * from Shifts where id='2'");
            if (dsshft.Tables[0].Rows.Count > 0)
            {
                // sht = dsshft.Tables[0].Rows[0]["Name"].ToString();
            }
            //dsshft = new DataSet();
            //dsshft = objcore.funGetDataSet("select * from users where id='" + user + "'");
            //if (dsshft.Tables[0].Rows.Count > 0)
            //{
            //    username = dsshft.Tables[0].Rows[0]["Name"].ToString();
            //}
            net = Math.Round(net, 2);
            if (logo == "")
            {
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, null, totlorder, avgsale);
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, deliveryp, takeawayp, dineinp, RefundNo, null, totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recvble, cashgst, visagst, cashdis, carddis, cashrecv, cardrecv, avgdinein, avgdineintable, DlvCharges,complimtry);

            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, deliveryp, takeawayp, dineinp, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recvble, cashgst, visagst, cashdis, carddis, cashrecv, cardrecv, avgdinein, avgdineintable, DlvCharges, complimtry);
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale);
            }

            return dat;
        }

        protected double getcomplimentary()
        {
            double val = 0;
            try
            {


                //q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0)  and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price";
                string q = "";
                q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                
                    
                        if (comboBox1.Text == "All")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                    
               


                DataSet dsdis = new DataSet();
                dsdis = new DataSet();
                dsdis = objcore.funGetDataSet(q);
                IList<complimentoryClass> data = dsdis.Tables[0].AsEnumerable().Select(row =>
                 new complimentoryClass
                 {
                     Amount = row.Field<double>("amount"),
                     Quantity = row.Field<double>("Quantity")


                 }).ToList();
                
                    
                        if (comboBox1.Text == "All")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.billstatus='Paid' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                    
                
                

                DataSet dsdis1 = new DataSet();
                dsdis1 = new DataSet();
                dsdis1 = objcore.funGetDataSet(q);
                IList<complimentoryClass> data1 = dsdis1.Tables[0].AsEnumerable().Select(row =>
                 new complimentoryClass
                 {
                     Amount = row.Field<double>("amount"),
                     Quantity = row.Field<double>("Quantity")


                 }).ToList();

                double qty = 0, amount = 0;

                qty = data.Sum(s => s.Quantity);
                amount = data.Sum(s => s.Amount);

                //qty = qty + data1.Sum(s => s.Quantity);
                //amount = amount + data1.Sum(s => s.Amount);


                val = Math.Round(Convert.ToDouble(amount), 3);

            }
            catch (Exception exc)
            {


            }

            return val;
        }


        protected void getusers()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "SELECT DISTINCT dbo.Users.Id, dbo.Users.Name FROM            dbo.Users INNER JOIN                         dbo.Sale ON dbo.Users.Id = dbo.Sale.UserId where sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' order by dbo.Users.Name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                dr["id"] = "0";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                comboBox1.Text = "All";
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
            getusers();
            try
            {
                string no = POSRestaurant.Properties.Settings.Default.MainScreenLocation.ToString();
                if (Convert.ToInt32(no) > 0)
                {


                    Screen[] sc;
                    sc = Screen.AllScreens;
                    this.StartPosition = FormStartPosition.Manual;
                    int no1 = Convert.ToInt32(no);
                    this.Location = Screen.AllScreens[no1].WorkingArea.Location;
                    this.WindowState = FormWindowState.Normal;
                    this.StartPosition = FormStartPosition.CenterScreen;
                    this.WindowState = FormWindowState.Maximized;

                }

            }
            catch (Exception ex)
            {

            }
            //this.TopMost = true;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Text = "Please Wait";
            vButton1.Enabled = false;
            vButton2.Enabled = false;
            updatebilltype();
            bindreport();
            vButton1.Enabled = true;
            vButton2.Enabled = true;
            vButton1.Text = "View";
        }
        public string printername()
        {
            string name = "";

            DataSet ds = new DataSet();
            string q = "select * from printers where type='opos'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                name = ds.Tables[0].Rows[0]["name"].ToString();
            }
            return name;
        }
        public string printtype()
        {
            string type = "";

            DataSet ds1 = new DataSet();
            try
            {
                string q = "select * from printtype where terminal='" + System.Environment.MachineName + "' and printer is null or terminal='" + System.Environment.MachineName + "' and printer='receipt'";
                ds1 = objCore.funGetDataSet(q);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    type = ds1.Tables[0].Rows[0]["type"].ToString();
                }
                if (type == "")
                {
                    ds1 = new DataSet();
                    q = "select * from printtype where  printer is null or printer='receipt'";
                    ds1 = objCore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        type = ds1.Tables[0].Rows[0]["type"].ToString().ToLower();
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                ds1.Dispose();
            }
            return type;
        }
        private void PrintReceipt(string cashier, string date, string shift, string type)
        {
            OPOSPOSPrinter printer = new OPOSPOSPrinter(); ;

            try
            {
                string pname = printername();
                printer.Open(pname);
                printer.ClaimDevice(10000);
                printer.DeviceEnabled = true;
                getcompany();
                string name = dscompany.Tables[0].Rows[0]["Name"].ToString();
                string addrs = dscompany.Tables[0].Rows[0]["Address"].ToString();
                string phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                string wellcom = dscompany.Tables[0].Rows[0]["WellComeNote"].ToString();
                string sht = "", username = "";
                DataSet dsshft = new DataSet();
                dsshft = objCore.funGetDataSet("select * from Shifts where id='" + shift + "'");
                if (dsshft.Tables[0].Rows.Count > 0)
                {
                    //sht = dsshft.Tables[0].Rows[0]["Name"].ToString();
                }
                dsshft = new DataSet();
                //dsshft = objCore.funGetDataSet("select * from users where id='" + user + "'");
                //if (dsshft.Tables[0].Rows.Count > 0)
                //{
                //    username = dsshft.Tables[0].Rows[0]["Name"].ToString();
                //}
                if (type == "day")
                {
                    sht = "";
                }
                PrintReceiptHeader(printer, name, addrs, sht, phone, date, username, type);
                DataSet ds = new DataSet();
                string q = "";
                double total = 0, totalcashh = 0, totalvisa = 0;
                if (type == "shift")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + date + "') and Sale.shiftid='" + shift + "' and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.Id ";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.Id ";

                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double cashh = 0, visaa = 0;
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.MenuGroup.Id='" + ds.Tables[0].Rows[i]["Id"].ToString() + "'  and dbo.sale.billtype like 'Cash%' and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name ";
                    DataSet dscash = new DataSet();
                    dscash = objcore.funGetDataSet(q);
                    if (dscash.Tables[0].Rows.Count > 0)
                    {
                        string temp = dscash.Tables[0].Rows[0]["sum"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cashh = Convert.ToDouble(temp);
                        cashh = Math.Round(cashh, 2);
                        totalcashh = totalcashh + cashh;
                    }
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.MenuGroup.Id='" + ds.Tables[0].Rows[i]["Id"].ToString() + "'  and dbo.sale.billtype like 'Visa%'  and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name ";
                    dscash = new DataSet();
                    dscash = objcore.funGetDataSet(q);
                    if (dscash.Tables[0].Rows.Count > 0)
                    {
                        string temp = dscash.Tables[0].Rows[0]["sum"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        visaa = Convert.ToDouble(temp);
                        visaa = Math.Round(visaa, 2);
                        totalvisa = totalvisa + visaa;
                    }
                    string showcardvisa = "yes";
                    try
                    {
                        showcardvisa = dscompany.Tables[0].Rows[0]["showcardvisa"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (showcardvisa == "no")
                    {
                        cashh = 0; visaa = 0;
                        totalcashh = 0; totalvisa = 0;
                    }
                    PrintLineItem(printer, ds.Tables[0].Rows[i]["Name"].ToString(), cashh, visaa, Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["sum"].ToString()), 2));
                    total = total + Convert.ToDouble(ds.Tables[0].Rows[i]["sum"].ToString());
                }
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintLineItem(printer, "Total", totalcashh,totalvisa, Math.Round(total, 2));

                string offSetString = new string(' ', printer.RecLineChars / 3);
                string Bold = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'b', (byte)'C' });
                PrintTextLine(printer, new string('=', (printer.RecLineChars)));
                PrintTextLine(printer, offSetString + " " + (Bold + "Sales Summary"));
                PrintTextLine(printer, new string('-', printer.RecLineChars));
                DataTable dt = new DataTable();
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                if (type == "shift")
                {
                    // dt = getAllOrders();
                }
                else
                {
                    dt = getAllOrders();
                }
                double net = 0, gst = 0, cashdis = 0, carddis = 0, dis = 0, refund = 0, gross = 0, cashgst = 0, cardgst = 0, avgdinein = 0, avgdineintable = 0;
                double cash = 0, credit = 0, master = 0, cashrecv=0, cardrecv=0, recvble = 0, service = 0, checks = 0, totalchks = 0, delivery = 0, visa = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, totalcash = 0;

                if (dt.Rows.Count > 0)
                {
                    net = Convert.ToDouble(dt.Rows[0]["NetSale"].ToString());
                    gst = Convert.ToDouble(dt.Rows[0]["GST"].ToString());
                    cashgst = Convert.ToDouble(dt.Rows[0]["cashgst"].ToString());
                    cardgst = Convert.ToDouble(dt.Rows[0]["cardgst"].ToString());
                    dis = Convert.ToDouble(dt.Rows[0]["Discount"].ToString());
                    refund = Convert.ToDouble(dt.Rows[0]["Refund"].ToString());
                    gross = Convert.ToDouble(dt.Rows[0]["GrossSale"].ToString());
                    cash = Convert.ToDouble(dt.Rows[0]["CashSale"].ToString());
                    visa = Convert.ToDouble(dt.Rows[0]["CreditSale"].ToString());
                    master = Convert.ToDouble(dt.Rows[0]["MasterSale"].ToString());
                    calculatedcash = Convert.ToDouble(dt.Rows[0]["calculatedcash"].ToString());
                    drawerfloat = Convert.ToDouble(dt.Rows[0]["float"].ToString());
                    totalcash = Convert.ToDouble(dt.Rows[0]["total"].ToString());
                    declared = Convert.ToDouble(dt.Rows[0]["declared"].ToString());
                    over = Convert.ToDouble(dt.Rows[0]["over"].ToString());
                    bankingtotal = Convert.ToDouble(dt.Rows[0]["bankingtotal"].ToString());
                    totalchks = Convert.ToDouble(dt.Rows[0]["totalorders"].ToString());
                    checks = Convert.ToDouble(dt.Rows[0]["averagesale"].ToString());
                    service = Convert.ToDouble(dt.Rows[0]["servicecharges"].ToString());
                    recvble = Convert.ToDouble(dt.Rows[0]["receivables"].ToString());
                    cashdis = Convert.ToDouble(dt.Rows[0]["CashDiscount"].ToString());
                    carddis = Convert.ToDouble(dt.Rows[0]["CardDiscount"].ToString());
                    cashrecv = Convert.ToDouble(dt.Rows[0]["cashrecv"].ToString());
                    cardrecv = Convert.ToDouble(dt.Rows[0]["cardrecv"].ToString());
                    avgdinein = Convert.ToDouble(dt.Rows[0]["avgdinein"].ToString());
                    avgdineintable = Convert.ToDouble(dt.Rows[0]["avgdineintable"].ToString());
                    
                }
                string qq1 = "";
                try
                {
                    if (type == "shift")
                    {
                        //qq1 = "SELECT ROUND(SUM(NetBill), 2) AS Expr1, id FROM  dbo.Sale WHERE (BillType ='Credit Card') AND (BillStatus = 'Paid') and date='" + date + "' and shiftid='" + shift + "' GROUP BY id";
                        qq1 = "SELECT  dbo.BillType.Amount AS cash, dbo.BillType.type, dbo.BillType.saleid FROM  dbo.BillType INNER JOIN               dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id WHERE (dbo.BillType.type LIKE '%Visa%') AND (dbo.sale.BillStatus = 'Paid') and dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and dbo.sale.shiftid='" + shift + "' ORDER BY dbo.BillType.type";
                    }
                    else
                    {
                        //qq1 = "SELECT ROUND(SUM(NetBill), 2) AS Expr1, id FROM  dbo.Sale WHERE (BillType ='Credit Card') AND (BillStatus = 'Paid') and date='" + date + "' GROUP BY id";
                        qq1 = "SELECT  dbo.BillType.Amount AS cash, dbo.BillType.type, dbo.BillType.saleid FROM  dbo.BillType INNER JOIN               dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id WHERE (dbo.BillType.type LIKE '%Visa%') AND (dbo.sale.BillStatus = 'Paid') and dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  ORDER BY dbo.BillType.type";

                    }

                    DataSet dsdiss = new DataSet();
                    dsdiss = objcore.funGetDataSet(qq1);
                    if (dsdiss.Tables[0].Rows.Count > 0)
                    {
                        PrintTextLine(printer, offSetString + " " + (Bold + "Visa Invoices"));
                        PrintTextLine(printer, new string('-', printer.RecLineChars));

                        for (int i = 0; i < dsdiss.Tables[0].Rows.Count; i++)
                        {
                            try
                            {
                                PrintLineItem(printer, "   " + dsdiss.Tables[0].Rows[i]["type"].ToString() + "- Bill #  " + dsdiss.Tables[0].Rows[i]["saleid"].ToString() + ": ",0, 0, Convert.ToDouble(dsdiss.Tables[0].Rows[i]["cash"].ToString()));
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        PrintTextLine(printer, new string('-', printer.RecLineChars));
                    }
                }
                catch (Exception e)
                {


                }

                PrintLineItem(printer, "Net Sale:",0, 0, Math.Round(net, 2));
                PrintLineItem(printer, "GST :", 0,0, Math.Round(gst, 2));
                PrintLineItem(printer, "   GST on Cash:", 0, 0, Math.Round(cashgst, 2));
                PrintLineItem(printer, "   GST on Card:", 0, 0, Math.Round(cardgst, 2));
                PrintLineItem(printer, "Srvc Chrgs :", 0,0, Math.Round(service, 2));
                PrintLineItem(printer, "Discount :",0, 0, Math.Round(dis, 2));
                PrintLineItem(printer, "   Discount on Cash:", 0, 0, Math.Round(cashdis, 2));
                PrintLineItem(printer, "   Discount on Card:", 0, 0, Math.Round(carddis, 2));
                try
                {
                    string qq = "";
                    //if (type == "shift")
                    //{
                    //    qq = "SELECT ROUND(SUM(Discountamount), 2) AS Expr1, Discount FROM  dbo.Sale WHERE (Discount > 0) AND (BillStatus = 'Paid') and date='" + date + "' and shiftid='" + shift + "' GROUP BY Discount";
                    //}
                    //else
                    {
                        qq = "SELECT ROUND(SUM(Discountamount), 2) AS Expr1, Discount FROM  dbo.Sale WHERE (Discount > 0) AND (BillStatus = 'Paid') and date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' GROUP BY Discount";

                    }

                    DataSet dsdis = new DataSet();
                    dsdis = objcore.funGetDataSet(qq);
                    for (int i = 0; i < dsdis.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            PrintLineItem(printer, "              " + dsdis.Tables[0].Rows[i]["Discount"].ToString() + " % ", 0,0, Convert.ToDouble(dsdis.Tables[0].Rows[i]["Expr1"].ToString()));
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    //if (type == "shift")
                    //{
                    //    qq = "SELECT ROUND(SUM(Discountamount), 2) AS Expr1, Discount FROM  dbo.Sale1 WHERE  date='" + date + "' AND (BillStatus = 'Paid') and shiftid='" + shift + "' GROUP BY Discount";
                    //}
                    //else
                    {
                        qq = "SELECT ROUND(SUM(Discountamount), 2) AS Expr1, Discount FROM  dbo.Sale1 WHERE  date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' AND (BillStatus = 'Paid') GROUP BY Discount";

                    }
                    dsdis = new DataSet();
                    dsdis = objcore.funGetDataSet(qq);
                    for (int i = 0; i < dsdis.Tables[0].Rows.Count; i++)
                    {

                        try
                        {
                            PrintLineItem(printer, "              100 % ", 0,0, Convert.ToDouble(dsdis.Tables[0].Rows[i]["Expr1"].ToString()));

                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {

                }
                try
                {
                    try
                    {
                        
                        {
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0)  and (sale.date between  '" + dateTimePicker1 + "' and '" + dateTimePicker2 + "')  AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price";

                        }
                        DataSet dsdis = new DataSet();
                        dsdis = new DataSet();
                        dsdis = objCore.funGetDataSet(q);
                        IList<complimentoryClass> data = dsdis.Tables[0].AsEnumerable().Select(row =>
                         new complimentoryClass
                         {
                             Amount = row.Field<double>("amount"),
                             Quantity = row.Field<double>("Quantity")


                         }).ToList();


                        q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between  '" + dateTimePicker1 + "' and '" + dateTimePicker2 + "')  GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer, dbo.RuntimeModifier.Price";

                        
                        DataSet dsdis1 = new DataSet();
                        dsdis1 = new DataSet();
                        dsdis1 = objCore.funGetDataSet(q);
                        IList<complimentoryClass> data1 = dsdis1.Tables[0].AsEnumerable().Select(row =>
                         new complimentoryClass
                         {
                             Amount = row.Field<double>("amount"),
                             Quantity = row.Field<double>("Quantity")


                         }).ToList();

                        double qty = 0, amount = 0;

                        qty = data.Sum(s => s.Quantity);
                        amount = data.Sum(s => s.Amount);

                        qty = qty + data1.Sum(s => s.Quantity);
                        amount = amount + data1.Sum(s => s.Amount);

                        PrintLineItem(printer, "     Complimentary " + "( " + qty + " )", 0, 0, Convert.ToDouble(amount));

                    }
                    catch (Exception exc)
                    {


                    }
                }
                catch (Exception ex)
                {


                }

                PrintLineItem(printer, "Refund :", 0,0, Math.Round(refund, 2));
                PrintLineItem(printer, "Gross Sale :", 0,0, Math.Round(gross, 2));

                PrintTextLine(printer, new string('=', (printer.RecLineChars)));
                PrintTextLine(printer, offSetString + "   " + (Bold + "Void Items"));
                PrintTextLine(printer, new string('-', printer.RecLineChars));
                try
                {
                    string qq = "";
                    //if (type == "shift")
                    //{

                    //    qq = "SELECT SUM(dbo.Saledetailsrefund.Quantity) AS Expr1, SUM(dbo.Saledetailsrefund.Price) AS Expr2, dbo.MenuItem.Name , dbo.Saledetailsrefund.reason FROM  dbo.Saledetailsrefund INNER JOIN               dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id  WHERE  dbo.sale.date='" + date + "' and dbo.sale.shiftid='" + shift + "' GROUP BY dbo.MenuItem.Name, dbo.Saledetailsrefund.reason";
                    //}
                    //else
                    {
                        qq = "SELECT SUM(dbo.Saledetailsrefund.Quantity) AS Expr1, SUM(dbo.Saledetailsrefund.Price) AS Expr2, dbo.MenuItem.Name, dbo.Saledetailsrefund.reason FROM  dbo.Saledetailsrefund INNER JOIN               dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id  WHERE  dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   GROUP BY dbo.MenuItem.Name, dbo.Saledetailsrefund.reason";


                    }
                    DataSet dsdis = new DataSet();
                    dsdis = objcore.funGetDataSet(qq);
                    for (int i = 0; i < dsdis.Tables[0].Rows.Count; i++)
                    {
                        string namme = dsdis.Tables[0].Rows[i]["Name"].ToString();
                        if (namme.Length > 23)
                        {
                            namme = namme.Substring(0, 23);
                        }
                        try
                        {
                            PrintLineItem(printer, namme + "(" + dsdis.Tables[0].Rows[i]["Expr1"].ToString() + ")", 0,0, Convert.ToDouble(dsdis.Tables[0].Rows[i]["Expr2"].ToString()));
                            PrintLineItem(printer, dsdis.Tables[0].Rows[i]["reason"].ToString(), 0,0, 0);

                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                catch (Exception)
                {


                }

                PrintTextLine(printer, new string('=', (printer.RecLineChars)));
                PrintTextLine(printer, offSetString + "   " + (Bold + "Payments"));
                PrintTextLine(printer, new string('-', printer.RecLineChars));
                PrintLineItem(printer, "Cash Sale:", 0,0, Math.Round(cash, 2));
                try
                {
                    //if (type == "shift")
                    //{
                    //    q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + date + "') and dbo.BillType.type like '%Visa%'  and dbo.sale.Terminal='" + System.Environment.MachineName.ToString() + "'  and dbo.Sale.shiftid='" + shift + "' GROUP BY dbo.BillType.type";
                    //}
                    //else
                    {
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash , dbo.BillType.type FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + date + "') and dbo.BillType.type like 'Visa%'  between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' GROUP BY dbo.BillType.type";
                    }
                    DataSet dsvisa = new DataSet();
                    dsvisa = objCore.funGetDataSet(q);
                    for (int i = 0; i < dsvisa.Tables[0].Rows.Count; i++)
                    {
                        string temp = dsvisa.Tables[0].Rows[i]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double amount = Convert.ToDouble(temp);
                        PrintLineItem(printer, dsvisa.Tables[0].Rows[i]["type"].ToString() + ":", 0,0, Math.Round(amount, 2));
                    }
                }
                catch (Exception ex)
                {


                }
                 PrintLineItem(printer, "Visa Sale:", 0,0, Math.Round(visa, 2));
               // PrintLineItem(printer, "Master Sale:", 0, Math.Round(master, 2));
                PrintLineItem(printer, "Receivables:", 0,0, Math.Round(recvble, 2));
                PrintLineItem(printer, "   Cash Receivables:", 0, 0, Math.Round(cashrecv, 2));
                PrintLineItem(printer, "   Card Receivables:", 0, 0, Math.Round(cardrecv, 2));
                PrintTextLine(printer, new string('=', (printer.RecLineChars)));
                PrintTextLine(printer, offSetString + "   " + (Bold + "Check OUt"));
                PrintTextLine(printer, new string('-', printer.RecLineChars));

                PrintLineItem(printer, "Calculated Cash :", 0,0, Math.Round(calculatedcash, 2));
                PrintLineItem(printer, "Drawer Float :", 0,0, Math.Round(drawerfloat, 2));
                PrintLineItem(printer, "Total :", 0,0, Math.Round(totalcash, 2));
                PrintLineItem(printer, "Declared Cash:", 0, 0, Math.Round(declared, 2));
                PrintLineItem(printer, "Over/Short:", 0, 0, Math.Round(over, 2));
                PrintLineItem(printer, "Banking Total:", 0, 0, Math.Round(bankingtotal, 2));
                PrintTextLine(printer, offSetString + "      " + (Bold + "Audit"));
                PrintTextLine(printer, new string('-', printer.RecLineChars));
                PrintLineItem(printer, "Total Checks:", 0, 0, Math.Round(totalchks, 2));
                PrintLineItem(printer, "Average Check:", 0, 0, Math.Round(checks, 2));
                PrintTextLine(printer, new string('=', (printer.RecLineChars)));
                PrintTextLine(printer, new string('.', (printer.RecLineChars)));
                PrintTextLine(printer, new string('.', (printer.RecLineChars)));
                PrintTextLine(printer, new string('.', (printer.RecLineChars)));
                PrintTextLine(printer, new string('.', (printer.RecLineChars)));
                printer.CutPaper(50);
                //PrintReceiptFooter(printer, Convert.ToDouble(0), Convert.ToDouble(0), Convert.ToDouble(0), wellcom,0, 0, Convert.ToDouble(0), Convert.ToDouble(0));
            }
            finally
            {
                DisconnectFromPrinter(printer);
            }
        }
        private void DisconnectFromPrinter(OPOSPOSPrinter printer)
        {
            printer.ReleaseDevice();
            printer.Close();
        }
        private void ConnectToPrinter(OPOSPOSPrinter printer)
        {
            try
            {
                //printer.Open();
                //printer.Claim(10000);
                //printer.DeviceEnabled = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Printer not connected");
            }
        }
        private void PrintReceiptFooter(OPOSPOSPrinter printer, double subTotal, double tax, double discount, string footerText, double received, double change, double disp, double gstp)
        {
            string offSetString = new string(' ', printer.RecLineChars / 2);

            PrintTextLine(printer, new string('-', (printer.RecLineChars)));
            PrintTextLine(printer, String.Format("SUB-TOTAL                     {0}", subTotal.ToString("#0.00")));
            PrintTextLine(printer, String.Format("GST                           {0}", tax.ToString("#0.00") + "(" + gstp + "%)"));
            PrintTextLine(printer, String.Format("DISCOUNT                       {0}", discount.ToString("#0.00") + "(" + disp + "%)"));
            PrintTextLine(printer, new string('-', (printer.RecLineChars)));
            PrintTextLine(printer, String.Format("Amount Tendered               {0}", ((subTotal + tax) - (discount)).ToString("#0.00")));
            PrintTextLine(printer, new string('-', (printer.RecLineChars)));
            PrintTextLine(printer, String.Format("Cash Given                    {0}", (received).ToString("#0.00")));
            PrintTextLine(printer, String.Format("Change Given                  {0}", (change).ToString("#0.00")));
            PrintTextLine(printer, new string('-', (printer.RecLineChars)));
            PrintTextLine(printer, String.Empty);

            //Embed 'center' alignment tag on front of string below to have it printed in the center of the receipt.
            int length = footerText.Length;
            int strt = 0;

            if (footerText.Contains("\n"))
            {
                PrintTextLine(printer, footerText.Replace("endline", ""));
            }
            else
            {
                int indx = footerText.IndexOf("endline", strt);
                for (int i = 0; i < length; i++)
                {
                    indx = footerText.IndexOf("endline", strt);
                    if (indx > 0)
                    {
                        string txt = footerText.Substring(strt, indx);

                        footerText = footerText.Substring(indx + 7);
                        length = footerText.Length;
                        PrintTextLine(printer, txt);
                        i = 0;
                    }
                    else
                    {
                        PrintTextLine(printer, footerText);
                        length = 0;
                    }
                }
            }
            //Added in these blank lines because RecLinesToCut seems to be wrong on my printer and
            //these extra blank lines ensure the cut is after the footer ends.
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            //Print 'advance and cut' escape command.
            printer.CutPaper(50);
            // printer.PrintNormal(2, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, 112, 48, 55, 121 }));
            // PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
        }

        private void PrintLineItem(OPOSPOSPrinter printer, string itemCode, double pricecash,double pricevisa, double Price)
        {

            {
                //temp = itemCode.Substring(indx + 1);
                int length = 6;
                int textlength = 15;
                string cash = pricecash.ToString();
                string visa = pricevisa.ToString();
                string total = Price.ToString(), total1 = "";
                if (cash == "0")
                {
                    cash = "";
                }
                else
                {
                    cash = cash ;
                }
                if (visa == "0")
                {
                    visa = "";
                }
                else
                {
                    visa = visa ;
                }
                if (cash.Length < length)
                {
                    cash = cash.PadRight(length);
                }
                if (visa.Length < length)
                {
                    visa = visa.PadRight(length );
                }
                if (total.Length < length)
                {
                    total = total.PadRight(length);
                }
                total1 = cash + visa + total.ToString();
                string text = itemCode + total1.ToString().PadLeft(39 - itemCode.Length);
                if (itemCode == "   GST on Cash:" || itemCode == "   GST on Card:")
                {
                    text = itemCode + total.ToString().PadLeft(34 - itemCode.Length);
                }
                if (itemCode.Contains("Visa") && itemCode != "Visa Sale:")
                {
                    text = itemCode + total.ToString().PadLeft(34 - itemCode.Length);
                }
                PrintTextLine(printer, text);
                //PrintText(printer, TruncateAt(itemCode, 28));
                //PrintTextLine(printer, TruncateAt(Price.ToString().PadLeft(40 - itemCode.Length), 45));

            }
        }
        private void PrintLineItemshift(OPOSPOSPrinter printer, string title, string value)
        {

            {
                //temp = itemCode.Substring(indx + 1);

                PrintText(printer, TruncateAt(title, 28));
                PrintTextLine(printer, TruncateAt(value.ToString().PadLeft(40 - title.Length), 45));
            }
        }
        private void PrintReceiptHeader(OPOSPOSPrinter printer, string companyName, string addressLine1, string shift, string taxNumber, string dateTime, string cashierName, string type)
        {
            string offSetString = new string(' ', printer.RecLineChars / 3);
            string Bold = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'b', (byte)'C' });
            string Bold1 = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' });
            string ESC = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27 });
            PrintTextLine(printer, offSetString + (Bold + companyName));
            PrintTextLine(printer, offSetString + "  " + Bold + addressLine1);
            //PrintTextLine(printer, offSetString + "    " + Bold + taxNumber);
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            if (type == "shift")
            {
                PrintTextLine(printer, offSetString + (Bold + "Cash Drawer Checkout"));
            }
            else
            {
                PrintTextLine(printer, offSetString + (Bold + "Day End Report"));
            }
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            PrintTextLine(printer, String.Format("DATE : {0}", Bold1 + Convert.ToDateTime(dateTime).ToShortDateString() + "   " + DateTime.Now.ToShortTimeString()));
            PrintTextLine(printer, String.Format("Shift : {0}", shift));
            PrintTextLine(printer, String.Format("Name : {0}", cashierName));

            PrintTextLine(printer, new string('-', printer.RecLineChars));
            //printer.PrintNormal(2, " " + Environment.NewLine);
            PrintTextLine(printer, offSetString + "       " + (Bold + "Sales"));
            PrintTextLine(printer, Bold + "Name                 Cash   Card   Total");
            PrintTextLine(printer, new string('-', printer.RecLineChars));

            //PrintTextLine(printer, String.Empty);

        }
        private void PrintText(OPOSPOSPrinter printer, string text)
        {
            if (text.Length <= printer.RecLineChars)
                printer.PrintNormal(2, text); //Print text
            else if (text.Length > printer.RecLineChars)
                printer.PrintNormal(2, TruncateAt(text, printer.RecLineChars)); //Print exactly as many characters as the printer allows, truncating the rest.
        }
        private void PrintTextLine(OPOSPOSPrinter printer, string text)
        {
            // printer.PrintNormal(PrinterStation.Receipt, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, 112, 48, 55, 121 }));
            if (text.Length < printer.RecLineChars || text.Contains("\n"))
                printer.PrintNormal(2, text + Environment.NewLine); //Print text, then a new line character.
            else if (text.Length > printer.RecLineChars)
                printer.PrintNormal(2, TruncateAt(text, printer.RecLineChars)); //Print exactly as many characters as the printer allows, truncating the rest, no new line character (printer will probably auto-feed for us)
            else if (text.Length == printer.RecLineChars)
                printer.PrintNormal(2, text + Environment.NewLine); //Print text, no new line character, printer will probably auto-feed for us.
        }
        private string TruncateAt(string text, int maxWidth)
        {
            string retVal = text;
            if (text.Length > maxWidth)
                retVal = text.Substring(0, maxWidth);

            return retVal;
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void updatebilltype()
        {
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_1 where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                dsbill = objcore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "insert into billtype (type,saleid,amount) values('" + type + "','" + id + "','" + amount + "')";
                    objcore.executeQuery(q);
                    q = "update sale set uploadstatusserver='Pending',uploadstatusbilltype='Pending' where id='" + id + "'";
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
                string q = "select * from View_2 where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                dsbill = objcore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "update billtype set amount='" + amount + "' where saleid='" + id + "'";
                    objcore.executeQuery(q);
                    q = "update sale set uploadstatusserver='Pending',uploadstatusbilltype='Pending' where id='" + id + "'";
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
            return;

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
                        objcore.executeQuery(q);
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
                    dsbill = objcore.funGetDataSet(q);
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
                        //q = "update sale set TotalBill='" + price + "',DiscountAmount='" + dis + "',GST='" + gst + "',netbill='" + ((price + gst) - (dis)) + "' where id='" + id + "'";
                        //objCore.executeQuery(q);
                        //q = "update sale set netbill=netbill+servicecharges+posfee where id='" + id + "'";
                        //objCore.executeQuery(q);
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
        private void vButton2_Click(object sender, EventArgs e)
        {
           
           
            updatebilltype();
            updatebilltype31();
            string type = printtype();
            if (type == "opos")
            {
                vButton2.Text = "Please Wait";
                vButton1.Enabled = false;
                vButton2.Enabled = false;
                PrintReceipt("", dateTimePicker2.Text, "", "day");
                vButton2.Text = "Print";
                vButton1.Enabled = true;
                vButton2.Enabled = true;

            }
            else if (printtype().ToLower() == "generic")
            {
                Printshiftend(printername("generic"), "", dateTimePicker1.Text, "", "day");
            }
            else
            {
                Printshiftend(printername("generic"), "", dateTimePicker1.Text, "", "day");
            }
        }
        protected void Printshiftend(string printerName, string name, string date, string shift, string type)
        {

            NativeMethods.DOC_INFO_1 documentInfo;
            IntPtr printerHandle;
            byte[] managedData = null;
            managedData = GetDocumentshiftend(name, date, shift, type);
            documentInfo = new NativeMethods.DOC_INFO_1();
            documentInfo.pDataType = "RAW";
            documentInfo.pDocName = "Receipt";
            printerHandle = new IntPtr(0);
            if (NativeMethods.OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
            {
                if (NativeMethods.StartDocPrinter(printerHandle, 1, documentInfo))
                {
                    int bytesWritten;
                    IntPtr unmanagedData;
                    //managedData = document;
                    unmanagedData = Marshal.AllocCoTaskMem(managedData.Length);
                    Marshal.Copy(managedData, 0, unmanagedData, managedData.Length);
                    if (NativeMethods.StartPagePrinter(printerHandle))
                    {
                        NativeMethods.WritePrinter(
                            printerHandle,
                            unmanagedData,
                            managedData.Length,
                            out bytesWritten);
                        NativeMethods.EndPagePrinter(printerHandle);
                    }
                    else
                    {
                        throw new Win32Exception();
                    }

                    Marshal.FreeCoTaskMem(unmanagedData);

                    NativeMethods.EndDocPrinter(printerHandle);
                }
                else
                {
                    throw new Win32Exception();
                }

                NativeMethods.ClosePrinter(printerHandle);
            }
            else
            {
                throw new Win32Exception();
            }

        }
        public byte[] GetDocumentshiftend(string name, string date, string shift, string type)
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Reset the printer bws (NV images are not cleared)
                bw.Write(AsciiControlChars.Escape);
                bw.Write('@');
                // Render the logo
                //RenderLogo(bw);
                PrintReceiptshiftend(bw, name, date, shift, type);
                // Feed 3 vertical motion units and cut the paper with a 1 point cut
                bw.Write(AsciiControlChars.GroupSeparator);
                bw.Write('V');
                bw.Write((byte)66);
                bw.Write((byte)3);

                bw.Flush();

                return ms.ToArray();
            }
        }
        protected int getlinelength(string type, string p)
        {
            int length = 0;
            DataSet dsl = new DataSet();
            string q = "select * from linelength where type='" + type + "' and printr='" + p + "'";
            dsl = objCore.funGetDataSet(q);
            if (dsl.Tables[0].Rows.Count > 0)
            {
                string temp = dsl.Tables[0].Rows[0]["length"].ToString();
                if (temp == "")
                {
                    temp = "20";
                }
                length = Convert.ToInt32(temp);
            }
            else
            {
                length = 37;
            }
            return length;
        }
        protected string  printitems()
        {
            string print = "Disabled";
            try
            {
                DataSet dsl = new DataSet();
                string q = "select * from DeviceSetting where  Device='Print Menu Item'";
                dsl = objCore.funGetDataSet(q);
                if (dsl.Tables[0].Rows.Count > 0)
                {
                    print = dsl.Tables[0].Rows[0]["Status"].ToString();
                    if (print == "")
                    {
                        print = "Disabled";
                    }

                }
            }
            catch (Exception ex)
            {
                
                
            }
           
            return print;
        }
        protected string printsubgroup()
        {
            string print = "Disabled";
            try
            {
                DataSet dsl = new DataSet();
                string q = "select * from DeviceSetting where  Device='Print Sub Group'";
                dsl = objCore.funGetDataSet(q);
                if (dsl.Tables[0].Rows.Count > 0)
                {
                    print = dsl.Tables[0].Rows[0]["Status"].ToString();
                    if (print == "")
                    {
                        print = "Disabled";
                    }

                }
            }
            catch (Exception ex)
            {


            }

            return print;
        }
        protected string printcat(string type)
        {
            string print = "Enabled";
            try
            {
                DataSet dsl = new DataSet();
                string q = "select * from DeviceSetting where  Device='" + type + "'";
                dsl = objCore.funGetDataSet(q);
                if (dsl.Tables[0].Rows.Count > 0)
                {
                    print = dsl.Tables[0].Rows[0]["Status"].ToString();
                    if (print == "")
                    {
                        print = "Disabled";
                    }

                }
            }
            catch (Exception ex)
            {


            }

            return print;
        }
        string branchid = "";
        public double opening(string itemid,string kitchenid)
        {
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "SELECT top 1 date, (remaining) as rem FROM     discard where Date <'" + date + "' and itemid='" + itemid + "'  and remaining is not null order by Date desc";
            if (kitchenid == "All")
            {
                q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid ='" + branchid + "' and Date <'" + date + "' and itemid='" + itemid + "'   order by Date desc";
            }
            else
            {
                q = "SELECT top 1 date, (remaining) as rem FROM     closing where branchid ='" + branchid + "' and Date <'" + date + "' and itemid='" + itemid + "' and kdsid='" + kitchenid + "'  order by Date desc";
            }
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                date2 = dspurchase.Tables[0].Rows[0][0].ToString();
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                closing = Convert.ToDouble(val);
            }
            if (date2 == "")
            {
                date2 = date;
            }
            DateTime end = Convert.ToDateTime(date);
            DateTime start = Convert.ToDateTime(date2);
            TimeSpan ts = Convert.ToDateTime(date) - Convert.ToDateTime(date2);
            int days = ts.Days;
            if (days <= 1)
            {
                return closing;
            }
            start = start.AddDays(1);
            end = end.AddDays(-1);
            if (kitchenid == "All")
            {
                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.purchase.branchcode ='" + branchid + "' and  dbo.Purchase.date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
                dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = Convert.ToDouble(val);
                }
            }
            dspurchase = new DataSet();
            if (kitchenid == "All")
            {
                try
                {
                    if (branchid == "All")
                    {
                        q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";
                    }
                    else
                    {

                        q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "') and dbo.Production.ItemId='" + itemid + "'  and dbo.Production.branchid='" + branchid + "'  and dbo.Production.status='Posted'";
                    }
                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        val = dspurchase.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        purchased = purchased + Convert.ToDouble(val);
                    }
                }
                catch (Exception ex)
                {


                }
            }
            val = "";
            purchased = Math.Round(purchased, 2);

            dspurchase = new DataSet();
            if (kitchenid == "All")
            {
                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid ='" + branchid + "' and  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and RawItemId='" + itemid + "'";
            }
            else
            {
                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid ='" + branchid + "' and  Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and RawItemId='" + itemid + "' and kdsid='" + kitchenid + "'";

            }
            // MessageBox.Show(q);
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                consumed = Convert.ToDouble(val);
                //   MessageBox.Show(consumed.ToString());
            }
            DataSet dsin = new DataSet();
            if (kitchenid == "All")
            {
                q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where branchid ='" + branchid + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    transferin = Convert.ToDouble(val);
                }
                dsin = new DataSet();
                q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where branchid ='" + branchid + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    transferout = Convert.ToDouble(val);
                }

            }
            val = "";
            double rate = 0;
            DataSet dscon = new DataSet();
            q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + itemid + "'";
            dscon = objcore.funGetDataSet(q);
            if (dscon.Tables[0].Rows.Count > 0)
            {
                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
            }
            if (rate > 0)
            {
                consumed = consumed / rate;
            }
            double qty = 0;
            qty = purchased - consumed;
            dspurchase = new DataSet();
            
            dspurchase = new DataSet();
            if (kitchenid == "All")
            {
                q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where branchid ='" + branchid + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "'";
            }
            else
            {
                q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where branchid ='" + branchid + "' and Date between '" + start.ToString("yyyy-MM-dd") + "' and  '" + end.ToString("yyyy-MM-dd") + "' and itemid='" + itemid + "' and kdsid='" + kitchenid + "'";

            }
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                discard = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                staff = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][2].ToString();
                if (val == "")
                {
                    val = "0";
                }
                complt = Convert.ToDouble(val);
            }
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (closing + purchased + transferin) - (staff + complt + transferout + consumed);

            //MessageBox.Show(closing.ToString() + "-p-" + purchased.ToString() + "trin-" + transferin.ToString() + "staff-" + staff + "complete-" + complt + "trout-" + transferout + "consumed-" + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        public double getclosing(string search,string kitchenid)
        {
            double closingvalue = 0;
            try
            {
                string date = dateTimePicker1.Text;
                double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, complete = 0, transferin = 0, transferout = 0, closing = 0;
                double qty = 0;
                DataTable ds = new DataTable();
                ds.Columns.Add("Id", typeof(string));
                ds.Columns.Add("ItemName", typeof(string));
                ds.Columns.Add("Opening", typeof(string));
                ds.Columns.Add("Purchased/Produced", typeof(string));
                ds.Columns.Add("Consumed", typeof(string));
                ds.Columns.Add("Variance", typeof(string));
                ds.Columns.Add("Waste/Staff Consumption", typeof(string));
                ds.Columns.Add("CompleteWaste", typeof(string));
                ds.Columns.Add("Transfer In", typeof(string));
                ds.Columns.Add("Transfer Out", typeof(string));
                ds.Columns.Add("Closing", typeof(string));
                ds.Columns.Add("Remarks", typeof(string));
                ds.Columns.Add("Closing_By_User", typeof(string));
                string q = "";
                q = "SELECT        TOP (100) PERCENT dbo.RawItem.Id, dbo.RawItem.ItemName + '(' + dbo.UOM.UOM + ')' AS Itemname FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.id = '" + search + "'  order by dbo.RawItem.itemname";

                DataSet ds1 = new DataSet();
                ds1 = objcore.funGetDataSet(q);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    purchased = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; complete = 0; transferin = 0; transferout = 0; closing = 0;
                    double openin = 0;
                    openin = opening(ds1.Tables[0].Rows[i]["id"].ToString(), kitchenid);
                    qty = openin;
                    string val = "";
                    double rem = 0;
                    DataSet dspurchase = new DataSet();

                    val = "";

                    dspurchase = new DataSet();

                    qty = qty + purchased;
                    dspurchase = new DataSet();

                    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where branchid ='" + branchid + "' and Date ='" + date + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + kitchenid + "'";

                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        val = dspurchase.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        consumed = Convert.ToDouble(val);
                    }
                    consumed = Math.Round(consumed, 3);
                    DataSet dsin = new DataSet();

                    val = "";
                    double rate = 0;
                    DataSet dscon = new DataSet();
                    q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    dscon = objcore.funGetDataSet(q);
                    if (dscon.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    if (rate > 0)
                    {
                        consumed = consumed / rate;
                    }
                    consumed = Math.Round(consumed, 3);
                    //qty = qty - consumed;
                    dspurchase = new DataSet();
                    string remarks = "";
                    dspurchase = new DataSet();


                    q = "SELECT     (discard) AS Expr1,(staff) AS staff ,(completewaste) AS completewaste,remarks,remaining FROM     discard where branchid ='" + branchid + "' and Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and kdsid='" + kitchenid + "'";

                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        remarks = dspurchase.Tables[0].Rows[0]["remarks"].ToString();
                        val = dspurchase.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        discard = Convert.ToDouble(val);
                        discard = Math.Round(discard, 3);
                        val = dspurchase.Tables[0].Rows[0][1].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        staff = Convert.ToDouble(val);
                        staff = Math.Round(staff, 3);
                        val = dspurchase.Tables[0].Rows[0][2].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        complete = Convert.ToDouble(val);
                        complete = Math.Round(complete, 3);
                    }
                    if (rate > 0)
                    {
                        complete = complete / rate;
                    }
                    complete = Math.Round(complete, 3);
                    string user = "";
                    string tempchk = "yes";

                    q = "SELECT     remaining,userid FROM     closing where branchid ='" + branchid + "' and Date ='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    if (kitchenid == "All")
                    {
                        q = "SELECT        dbo.Closing.Remaining, dbo.Users.Name FROM            dbo.Closing LEFT OUTER JOIN                         dbo.Users ON dbo.Closing.Userid = dbo.Users.Id  where dbo.Closing.branchid ='" + branchid + "' and dbo.Closing.Date ='" + date + "' and dbo.Closing.itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    }
                    else
                    {
                        q = "SELECT        dbo.Closing.Remaining, dbo.Users.Name FROM            dbo.Closing LEFT OUTER JOIN                         dbo.Users ON dbo.Closing.Userid = dbo.Users.Id  where dbo.Closing.branchid ='" + branchid + "' and dbo.Closing.Date ='" + date + "' and dbo.Closing.itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and dbo.Closing.kdsid='" + kitchenid + "'";
                    }
                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        user = dspurchase.Tables[0].Rows[0]["Name"].ToString();
                        val = dspurchase.Tables[0].Rows[0]["remaining"].ToString();
                        if (val == "")
                        {
                            tempchk = "no";
                            val = "0";
                        }
                        else
                        {
                            closing = Convert.ToDouble(val);
                        }
                    }
                    else
                    {
                        tempchk = "no";
                    }
                    //discard = discard * -1;
                    double actual = (openin + purchased + transferin) - (staff + complete + transferout);
                    actual = Math.Round(actual, 3);
                    actual = actual - closing;
                    actual = Math.Round(actual, 3);
                    if (tempchk == "yes")

                    //if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        //if (consumed > 0)
                        {
                            discard = consumed - actual;
                        }
                    }
                    else
                    {
                        closing = actual;
                        closing = closing - consumed;
                    }
                    discard = Math.Round(discard, 3);

                    qty = Math.Round(qty, 2);
                    closingvalue = closing;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return closingvalue;
        }
        string cashp = "0", deliveryp = "0", dineinp = "0", cts = "", openp = "0", takeawayp = "0", visap = "0", masterp = "0", ctsp = "0", refundp = "0";
        private void PrintReceiptshiftend(BinaryWriter bw, string cashier, string date, string shift, string type)
        {
            //int length = getlinelength("line", "receipt");
            //string print = "", space = "", print2 = "";
            //for (int i = 0; i < length; i++)
            //{
            //    print = print + "-";
            //    print2 = print2 + "=";
            //}
            //length = getlinelength("space", "receipt");
            //for (int i = 0; i < length; i++)
            //{
            //    space = space + " ";
            //}
            //DataSet dsshft = new DataSet();
            //string temp = "", shiftname = "";
            //dsshft = objCore.funGetDataSet("select * from Shifts where id='" + shift + "'");
            //if (dsshft.Tables[0].Rows.Count > 0)
            //{
            //    shiftname = dsshft.Tables[0].Rows[0]["Name"].ToString();
            //}
            //string offSetString = new string(' ', 20);
            //getcompany();
            //length = getlinelength("name", "receipt");
            //string namee = dscompany.Tables[0].Rows[0]["Name"].ToString();
            //namee = namee.PadLeft(length);
            //length = getlinelength("address", "receipt");
            //string addrs = dscompany.Tables[0].Rows[0]["Address"].ToString();
            //addrs = addrs.PadLeft(length);

            //length = getlinelength("comp", "receipt");
            //string comp = dscompany.Tables[0].Rows[0]["Company"].ToString();
            //comp = comp.PadLeft(length);

            //string phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
            //length = getlinelength("phone", "receipt");
            //phone = phone.PadLeft(length);


            //bw.NormalFont(comp);
            //bw.NormalFont(namee);
            //bw.NormalFont(addrs);
            //bw.NormalFont(phone);

            //length = getlinelength("line", "receipt");


            //bw.NormalFont(print);
            DataSet dsshft = new DataSet();
            string temp = "", shiftname = "";
            dsshft = objCore.funGetDataSet("select * from Shifts where id='" + shift + "'");
            if (dsshft.Tables[0].Rows.Count > 0)
            {
                shiftname = dsshft.Tables[0].Rows[0]["Name"].ToString();
            }
            try
            {
                dsshft = new DataSet();
                dsshft = objCore.funGetDataSet("select * from users where id="+comboBox1.SelectedValue);
                if (dsshft.Tables[0].Rows.Count > 0)
                {
                    cashier = dsshft.Tables[0].Rows[0]["name"].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
            try
            {
                dsshft = new DataSet();
                dsshft = objCore.funGetDataSet("select * from Branch");
                if (dsshft.Tables[0].Rows.Count > 0)
                {
                    branchid = dsshft.Tables[0].Rows[0]["id"].ToString();
                }
            }
            catch (Exception ex)
            {

            }

            string offSetString = new string(' ', 20);
            getcompany();



            int length = getlinelength("name", "receipt");
            string namee = dscompany.Tables[0].Rows[0]["Name"].ToString();
            string tempp = "";
            tempp = tempp.PadLeft(length);
            namee = tempp + namee;
            tempp = "";
            length = getlinelength("address", "receipt");
            string addrs = dscompany.Tables[0].Rows[0]["Address"].ToString();
            tempp = tempp.PadLeft(length);
            addrs = tempp + addrs;
            tempp = "";
            //length = getlinelength("comp", "receipt");
            //string comp = dscompany.Tables[0].Rows[0]["Company"].ToString();
            //tempp = tempp.PadLeft(length);
            //comp = tempp + comp;

            string phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
            length = getlinelength("phone", "receipt");
            tempp = tempp.PadLeft(length);
            phone = tempp + phone;


           // bw.NormalFont(comp);
            bw.NormalFont(namee);
            bw.NormalFont(addrs);
            bw.NormalFont(phone);

            length = getlinelength("line", "receipt");
            string print = "", space = "", print2 = "";
            for (int i = 0; i < length; i++)
            {
                print = print + "-";
                print2 = print2 + "=";
            }
            length = getlinelength("space", "kot");
            for (int i = 0; i < length; i++)
            {
                space = space + " ";
            }
            bw.NormalFont(print);
            if (type == "shift")
            {
                bw.LargeText("Cash Drawer Checkout");
                bw.NormalFont(print);
                bw.NormalFont("DATE :  " + Convert.ToDateTime(date).ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                bw.NormalFont("Name :  " + cashier);
                bw.NormalFont("Shift : " + shiftname + "  Terminal : " + System.Environment.MachineName);


            }
            else
            {
                bw.LargeText("  Daily Sales Report");
                bw.NormalFont(print);
                bw.NormalFont("DATE :  " + Convert.ToDateTime(date).ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
                //bw.NormalFont("Shift : " + shift + "  Terminal : " + System.Environment.MachineName);
                bw.NormalFont("Name :  " + cashier);
            }
            bw.NormalFont(print);
            bw.LargeText("          Sales");
            bw.NormalFont(print);
            DataSet ds = new DataSet();
            string q = "";
            double total = 0;
            string nm = "", sum = "",summ="";
            if (printcat("Show MenuGroup on Sales Report") == "Enabled")
            {
                if (printsubgroup() == "Enabled")
                {
                    if (type == "shift")
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.SubGroup as Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + date + "') and Sale.shiftid='" + shift + "' and (Sale.BillStatus = 'Paid') and Saledetails.Price > 0  GROUP BY dbo.MenuGroup.SubGroup ";
                    }
                    else
                    {
                        if (comboBox1.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.SubGroup as Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.BillStatus = 'Paid') and Saledetails.Price > 0  GROUP BY dbo.MenuGroup.SubGroup ";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.SubGroup as Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE  sale.userid='" + comboBox1.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.BillStatus = 'Paid') and Saledetails.Price > 0  GROUP BY dbo.MenuGroup.SubGroup ";

                        }
                    }
                }
                else
                {
                    if (type == "shift")
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + date + "') and Sale.shiftid='" + shift + "' and (Sale.BillStatus = 'Paid') and Saledetails.Price > 0  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                    }
                    else
                    {
                        if (comboBox1.Text == "All")
                        {

                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.BillStatus = 'Paid') and Saledetails.Price > 0  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    sale.userid='" + comboBox1.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.BillStatus = 'Paid') and Saledetails.Price > 0  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                      
                        }
                    }
                }

                try
                {
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string name = ds.Tables[0].Rows[i]["Name"].ToString();
                        for (int j = name.Length; j < space.Length; j++)
                        {
                            name += " ";
                        }
                        temp = ds.Tables[0].Rows[i]["sum"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                        if (!sum.Contains("."))
                        {
                            sum = sum + ".0";
                        }
                        for (int j = sum.Length; j < 12; j++)
                        {
                            sum = " " + sum;
                        }
                        bw.NormalFont(name + sum);
                        total = total + Convert.ToDouble(temp);
                    }

                    bw.NormalFont(print);
                    nm = "Total";
                    for (int j = nm.Length; j < space.Length; j++)
                    {
                        nm += " ";
                    }
                    summ = (Math.Round(total, 3)).ToString();
                    if (!summ.Contains("."))
                    {
                        summ = summ + ".0";
                    }
                    for (int j = summ.Length; j < 12; j++)
                    {
                        summ = " " + summ;
                    }
                    bw.NormalFont(nm + summ);
                    bw.NormalFont(print2);
                }
                catch (Exception exx)
                {
                    
                }
            }
            double totalqty = 0;
            if (printitems() == "Enabled")
            {
                total = 0;
                bw.LargeText(" Items Wise Sales");
                bw.NormalFont(print);
                if (comboBox1.Text == "All")
                {
                    q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                }
                else
                {
                    q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE   sale.userid='"+comboBox1.SelectedValue+"' and  (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                }
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string flavour = ds.Tables[0].Rows[i]["Expr1"].ToString();

                    string name = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (flavour.Length > 0)
                    {
                        name = flavour + " " + name;
                    }
                    for (int j = name.Length ; j < space.Length; j++)
                    {
                        name += " ";
                    }
                    temp = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                    if (!sum.Contains("."))
                    {
                        sum = sum + ".0";
                    }
                    for (int j = sum.Length; j < 6; j++)
                    {
                        sum = " " + sum;
                    }
                    total = total + Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[i]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    string qty = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                    if (!qty.Contains("."))
                    {
                        qty = qty + ".0";
                    }
                    for (int j = qty.Length; j < 6; j++)
                    {
                        qty = " " + qty;
                    }
                    totalqty = totalqty + Convert.ToDouble(temp);
                    bw.NormalFont(name + qty + " ," + sum);

                    
                }
                if (comboBox1.Text == "All")
                {
                    q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0 and dbo.Saledetails.price>0  GROUP BY dbo.RuntimeModifier.name";
                }
                else
                {
                    q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE   sale.userid='"+comboBox1.SelectedValue+"' and  (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0 and dbo.Saledetails.price>0  GROUP BY dbo.RuntimeModifier.name";
                }
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = ds.Tables[0].Rows[i]["Name"].ToString();
                    for (int j = name.Length ; j < space.Length; j++)
                    {
                        name += " ";
                    }
                    temp = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                    if (!sum.Contains("."))
                    {
                        sum = sum + ".0";
                    }
                    for (int j = sum.Length; j < 6; j++)
                    {
                        sum = " " + sum;
                    }
                    total = total + Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[i]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    string qty = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                    if (!qty.Contains("."))
                    {
                        qty = qty + ".0";
                    }
                    for (int j = qty.Length; j < 6; j++)
                    {
                        qty = " " + qty;
                    }
                    bw.NormalFont(name + qty + " ," + sum);
                    totalqty = totalqty + Convert.ToDouble(temp);
                    
                }
                if (comboBox1.Text == "All")
                {
                    q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0 and dbo.Saledetails.price>0  GROUP BY dbo.Modifier.name";
                }
                else
                {
                    q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE  sale.userid='"+comboBox1.SelectedValue+"' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0 and dbo.Saledetails.price>0  GROUP BY dbo.Modifier.name";
                }
                    ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = ds.Tables[0].Rows[i]["Name"].ToString();
                    for (int j = name.Length ; j < space.Length; j++)
                    {
                        name += " ";
                    }
                    temp = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                    if (!sum.Contains("."))
                    {
                        sum = sum + ".0";
                    }
                    for (int j = sum.Length; j < 6; j++)
                    {
                        sum = " " + sum;
                    }
                    total = total + Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[i]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    string qty = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                    if (!qty.Contains("."))
                    {
                        qty = qty + ".0";
                    }
                    for (int j = qty.Length; j < 6; j++)
                    {
                        qty = " " + qty;
                    }
                    totalqty = totalqty + Convert.ToDouble(temp);
                    bw.NormalFont(name + qty + " ," + sum);
                }
                bw.NormalFont(print);
                nm = "Total";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                summ = (Math.Round(total, 3)).ToString();
                if (!summ.Contains("."))
                {
                    summ = summ + ".0";
                }
                for (int j = summ.Length; j < 6; j++)
                {
                    summ = " " + summ;
                }
                string tqty = "";
                try
                {
                    tqty = (Math.Round(totalqty, 3)).ToString();
                    if (!tqty.Contains("."))
                    {
                        tqty = tqty + ".0";
                    }
                    for (int j = tqty.Length; j < 6; j++)
                    {
                        tqty = " " + tqty;
                    }
                }
                catch (Exception ex)
                {


                }
                bw.NormalFont(nm + tqty + " ," + summ);

                bw.NormalFont(print2);
            }
            try
            {
                q = "select id,itemname from rawitem where critical='yes'";
                DataSet dsitem = new DataSet();
                dsitem = objcore.funGetDataSet(q);

                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    bw.LargeText(" Sold Inventory");
                    bw.NormalFont(print);
                    //bw.NormalFont(print);
                    for (int ii = 0; ii < dsitem.Tables[0].Rows.Count; ii++)
                    {
                        total = 0;

                        q = "select sum(QuantityConsumed) as QuantityConsumed from InventoryConsumed where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and RawItemId='" + dsitem.Tables[0].Rows[ii]["id"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string name = dsitem.Tables[0].Rows[ii]["itemname"].ToString();
                            for (int j = name.Length; j < space.Length; j++)
                            {
                                name += " ";
                            }
                            temp = ds.Tables[0].Rows[i]["QuantityConsumed"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                            if (!sum.Contains("."))
                            {
                                sum = sum + ".0";
                            }
                            for (int j = sum.Length; j < 6; j++)
                            {
                                sum = " " + sum;
                            }

                            bw.NormalFont(name + ": " + sum);


                        }
                        bw.NormalFont(print);
                    }

                    bw.LargeText(" Closing Inventory");
                    bw.NormalFont(print);


                    q = "select * from kds where id>0";
                    DataSet dskds = new DataSet();
                    dskds = objcore.funGetDataSet(q);
                    for (int i = 0; i < dskds.Tables[0].Rows.Count; i++)
                    {
                        
                        q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Description, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id WHERE        (dbo.MenuItem.Status = 'active') and (dbo.RawItem.critical = 'yes') and  dbo.MenuItem.KDSId='" + dskds.Tables[0].Rows[i]["id"].ToString() + "' order by dbo.RawItem.ItemName";
                        DataSet dsd = new DataSet();
                        dsd = objcore.funGetDataSet(q);
                        if (dsd.Tables[0].Rows.Count > 0)
                        {
                            bw.High(dskds.Tables[0].Rows[i]["Name"].ToString());
                            bw.NormalFont(print);
                        }
                        for (int i1 = 0; i1 < dsd.Tables[0].Rows.Count; i1++)
                        {
                            double value = getclosing(dsd.Tables[0].Rows[i1]["id"].ToString(), dskds.Tables[0].Rows[i]["id"].ToString());
                            string name = dsd.Tables[0].Rows[i1]["itemname"].ToString();
                            for (int j = name.Length; j < space.Length; j++)
                            {
                                name += " ";
                            }
                            temp = value.ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
                            if (!sum.Contains("."))
                            {
                                sum = sum + ".0";
                            }
                            for (int j = sum.Length; j < 6; j++)
                            {
                                sum = " " + sum;
                            }

                            bw.NormalFont(name + ": " + sum);
                        }

                    }
                    bw.NormalFont(print2);




                }
            }
            catch (Exception ex)
            {


            }
             
           
            DataTable dt = new DataTable();
            // Just set the name of data table
            dt.TableName = "Crystal Report";
            dt = getAllOrders(); 
            //if (type == "shift")
            //{
            //    dt = getAllOrders();
            //}
            //else
            //{
            //   // dt = getAllOrders1(date);
            //}
            double net = 0, gst = 0, cashdis = 0, DlvCharges = 0, carddis = 0, cashrecv = 0, cardrecv = 0, dis = 0, refund = 0, voidsale = 0, gross = 0, cashgst = 0, cardgst = 0, avgdinein = 0, avgdineintable = 0;
            double cash = 0, credit = 0, master = 0, checks = 0, totalchks = 0, delivery = 0,service=0, membership = 0, visa = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, totalcash = 0;
            if (dt.Rows.Count > 0)
            {
                try
                {
                    temp = dt.Rows[0]["avgdinein"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    avgdinein = Convert.ToDouble(temp);
                    temp = dt.Rows[0]["avgdineintable"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    avgdineintable = Convert.ToDouble(temp);
                }
                catch (Exception ex)
                {
                   
                }


                temp = dt.Rows[0]["NetSale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }

                net = Convert.ToDouble(temp);
                temp = dt.Rows[0]["GST"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                gst = Convert.ToDouble(temp);

                temp = dt.Rows[0]["DlvCharges"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                DlvCharges = Convert.ToDouble(temp);



                temp = dt.Rows[0]["cardgst"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                cardgst = Convert.ToDouble(temp);

                temp = dt.Rows[0]["cashgst"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                cashgst = Convert.ToDouble(temp);

                temp = dt.Rows[0]["cashrecv"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                cashrecv = Convert.ToDouble(temp);
                temp = dt.Rows[0]["cardrecv"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                cardrecv = Convert.ToDouble(temp);

                temp = dt.Rows[0]["Discount"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                dis = Convert.ToDouble(temp);
                temp = dt.Rows[0]["CashDiscount"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                cashdis = Convert.ToDouble(temp);
                temp = dt.Rows[0]["CardDiscount"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                carddis = Convert.ToDouble(temp);

                temp = dt.Rows[0]["Refund"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                refund = Convert.ToDouble(temp);
                temp = dt.Rows[0]["void"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                voidsale = Convert.ToDouble(temp);
                temp = dt.Rows[0]["GrossSale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                gross = Convert.ToDouble(temp);
                temp = dt.Rows[0]["CashSale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                cash = Convert.ToDouble(temp);
                temp = dt.Rows[0]["CreditSale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                visa = Convert.ToDouble(temp);
                temp = dt.Rows[0]["MasterSale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                master = Convert.ToDouble(temp);
                // temp = dt.Rows[0]["membership"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                membership = Convert.ToDouble(temp);

                temp = dt.Rows[0]["calculatedcash"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                calculatedcash = Convert.ToDouble(temp);
                temp = dt.Rows[0]["float"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                drawerfloat = Convert.ToDouble(temp);
                temp = dt.Rows[0]["total"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                totalcash = Convert.ToDouble(temp);
                temp = dt.Rows[0]["declared"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                declared = Convert.ToDouble(temp);
                temp = dt.Rows[0]["over"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                over = Convert.ToDouble(temp);
                temp = dt.Rows[0]["bankingtotal"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                bankingtotal = Convert.ToDouble(temp);
                temp = dt.Rows[0]["totalorders"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                totalchks = Convert.ToDouble(temp);
                temp = dt.Rows[0]["averagesale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                checks = Convert.ToDouble(temp);
            }
            if (printcat("Show Sales Summary on Sales Report") == "Enabled")
            {
                bw.LargeText("     Sales Summary");
                bw.NormalFont(print);
                temp = Math.Round(net, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Net Sale :   " + temp);
                temp = Math.Round(gst, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("GST :        " + temp);

                temp = Math.Round(cashgst, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("    Cash GST :    " + temp);

                temp = Math.Round(cardgst, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("    Card GST :    " + temp);
                temp = Math.Round(DlvCharges, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Dlv Charges :    " + temp);


                double salgst = gst + net + DlvCharges;
                salgst = Math.Round(salgst, 3);


                nm = "Net Sale + GST + Dlv Charges:";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }

                temp = Math.Round(salgst, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);


                temp = dt.Rows[0]["servicecharges"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                service = Convert.ToDouble(temp);
                temp = Math.Round(service, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 6; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Service Charges :  " + temp);


                temp = Math.Round(dis, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Discount :   " + temp);
                temp = Math.Round(cashdis, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("    Cash Discount :    " + temp);
                temp = Math.Round(carddis, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("    Card Discount :    " + temp);

                try
                {
                    string qq = "";
                    //if (type == "shift")
                    //{
                    //    qq = "SELECT ROUND(SUM(Discountamount), 2) AS Expr1, Discount FROM  dbo.Sale WHERE (Discount > 0) AND (BillStatus = 'Paid') and date='" + date + "' and shiftid='" + shift + "' GROUP BY Discount";
                    //}
                    //else
                    {
                        qq = "SELECT ROUND(SUM(Discountamount), 2) AS Expr1, Discount FROM  dbo.Sale WHERE (Discount > 0) AND (BillStatus = 'Paid') and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY Discount";

                    }

                    DataSet dsdis = new DataSet();
                    dsdis = objcore.funGetDataSet(qq);
                    for (int i = 0; i < dsdis.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            //PrintLineItem(printer, "              " + dsdis.Tables[0].Rows[i]["Discount"].ToString() + " % ", 0, 0, Convert.ToDouble(dsdis.Tables[0].Rows[i]["Expr1"].ToString()));
                            nm = "     " + dsdis.Tables[0].Rows[i]["Discount"].ToString() + " % ";
                            for (int j = nm.Length; j < space.Length; j++)
                            {
                                nm += " ";
                            }
                            temp = Math.Round(Convert.ToDouble(dsdis.Tables[0].Rows[i]["Expr1"].ToString()), 3).ToString();
                            if (!temp.Contains("."))
                            {
                                temp = temp + ".00";
                            }
                            for (int j = temp.Length; j < 12; j++)
                            {
                                temp = " " + temp;
                            }
                            bw.NormalFont(nm + temp);
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                catch (Exception ex)
                {

                }
                try
                {
                    //if (type == "shift")
                    //{

                    //    q = "SELECT SUM(CONVERT(float, dbo.MenuItem.Price)) + SUM(CONVERT(float, dbo.ModifierFlavour.price)) AS Expr1, COUNT(dbo.Saledetails.Quantity) AS Expr2 FROM  dbo.Sale INNER JOIN               dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE dbo.Saledetails.price='0' AND (dbo.Sale.Discount < 100)  and dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' AND (dbo.Sale.BillStatus = 'Paid') and dbo.Sale.shiftid='" + shift + "' ";

                    //}
                    //else
                    //{

                    // q = "SELECT        (SUM(CONVERT(float, dbo.MenuItem.Price)) + SUM(CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS Expr1, SUM(dbo.Saledetails.Quantity) AS Expr2 FROM  dbo.Sale INNER JOIN               dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE dbo.Saledetails.price='0' AND (dbo.Sale.Discount < 100)  and dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' AND (dbo.Sale.BillStatus = 'Paid')";

                    // }
                    try
                    {
                        //q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0)  and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price";
                        q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                        DataSet dsdis = new DataSet();
                        dsdis = new DataSet();
                        dsdis = objcore.funGetDataSet(q);
                        IList<complimentoryClass> data = dsdis.Tables[0].AsEnumerable().Select(row =>
                         new complimentoryClass
                         {
                             Amount = row.Field<double>("amount"),
                             Quantity = row.Field<double>("Quantity")


                         }).ToList();
                        q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";
                        DataSet dsdis1 = new DataSet();
                        dsdis1 = new DataSet();
                        dsdis1 = objcore.funGetDataSet(q);
                        IList<complimentoryClass> data1 = dsdis1.Tables[0].AsEnumerable().Select(row =>
                         new complimentoryClass
                         {
                             Amount = row.Field<double>("amount"),
                             Quantity = row.Field<double>("Quantity")


                         }).ToList();

                        double qty = 0, amount = 0;

                        qty = data.Sum(s => s.Quantity);
                        amount = data.Sum(s => s.Amount);

                        //qty = qty + data1.Sum(s => s.Quantity);
                        //amount = amount + data1.Sum(s => s.Amount);

                        nm = "     Complimentary " + "( " + qty + " )";
                        for (int j = nm.Length; j < space.Length; j++)
                        {
                            nm += " ";
                        }
                        temp = Math.Round(Convert.ToDouble(amount), 3).ToString();
                        if (!temp.Contains("."))
                        {
                            temp = temp + ".00";
                        }
                        for (int j = temp.Length; j < 12; j++)
                        {
                            temp = " " + temp;
                        }
                        bw.NormalFont(nm + temp);
                        gross = gross + amount;
                    }
                    catch (Exception exc)
                    {


                    }

                    //for (int i = 0; i < dsdis.Tables[0].Rows.Count; i++)
                    //{

                    //    try
                    //    {
                    //        //PrintLineItem(printer, "     Complimentary " + "( " + dsdis.Tables[0].Rows[i]["Expr2"].ToString() + " )", 0, 0, Convert.ToDouble(dsdis.Tables[0].Rows[i]["Expr1"].ToString()));
                    //        nm = "     Complimentary " + "( " + dsdis.Tables[0].Rows[i]["Expr2"].ToString() + " )";
                    //        for (int j = nm.Length; j < space.Length; j++)
                    //        {
                    //            nm += " ";
                    //        }
                    //        temp =  Math.Round(Convert.ToDouble(dsdis.Tables[0].Rows[i]["Expr1"].ToString()), 3).ToString();
                    //        if (!temp.Contains("."))
                    //        {
                    //            temp = temp + ".00";
                    //        }
                    //        for (int j = temp.Length; j < 12; j++)
                    //        {
                    //            temp = " " + temp;
                    //        }
                    //        bw.NormalFont(nm + temp);

                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }
                    //}
                }
                catch (Exception ex)
                {


                }
                nm = "Void :";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                temp = Math.Round(voidsale, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);


                temp = Math.Round(refund, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Refund :  (" + refundp + ")" + temp);
                temp = Math.Round(gross, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Gross Sale : " + temp);

                bw.NormalFont(print2);
            }
            if (printcat("Show Payments on Sales Report") == "Enabled")
            {
                bw.LargeText("        Payments");
                bw.NormalFont(print);
                double totalpayment = cash;
                nm = "Cash Sale :   (" + cashp + ")";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                temp = Math.Round(cash, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);
                double totalcard = 0, totalcardqty = 0;
                DataSet dsBanks = new DataSet();
                dsBanks = objcore.funGetDataSet("Select * from banks");
                for (int i = 0; i < dsBanks.Tables[0].Rows.Count; i++)
                {

                    if (type == "shift")
                    {
                        q = "SELECT     SUM(dbo.Sale.NetBill) AS cash ,count(*) as p  FROM         Sale where (Date  between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Visa " + dsBanks.Tables[0].Rows[i]["Name"] + "' and shiftid='" + shift + "' and BillStatus='Paid' and Terminal='" + System.Environment.MachineName + "'";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.Sale.NetBill) AS cash ,count(*) as p  FROM         Sale where (Date  between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Visa " + dsBanks.Tables[0].Rows[i]["Name"] + "'  and BillStatus='Paid'";

                    }
                    DataSet dsbanks1 = new DataSet();
                    dsbanks1 = objcore.funGetDataSet(q);
                    nm = "" + dsBanks.Tables[0].Rows[i]["Name"] + " :   (" + dsbanks1.Tables[0].Rows[0]["p"] + ")";
                    for (int j = nm.Length; j < space.Length; j++)
                    {
                        nm += " ";
                    }
                    if (dsbanks1.Tables[0].Rows[0]["cash"] == System.DBNull.Value)
                    {
                        temp = "0";
                    }
                    else
                    {
                        temp = Math.Round(Convert.ToDouble(dsbanks1.Tables[0].Rows[0]["cash"]), 3).ToString();
                    }
                    if (!temp.Contains("."))
                    {
                        temp = temp + ".00";
                    }
                    for (int j = temp.Length; j < 12; j++)
                    {
                        temp = " " + temp;
                    }
                    bw.NormalFont(nm + temp);
                    try
                    {
                        totalcard = totalcard + Convert.ToDouble(dsbanks1.Tables[0].Rows[0]["cash"]);
                        totalcardqty = totalcardqty + Convert.ToDouble(dsbanks1.Tables[0].Rows[0]["p"]);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                totalpayment = totalpayment + totalcard;

                if (type == "shift")
                {
                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash ,count(*) as p  FROM         Sale where (Date  between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Receivables' and shiftid='" + shift + "' and BillStatus='Paid' and Terminal='" + System.Environment.MachineName + "'";

                    q = "SELECT        SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash, COUNT(*) AS p, dbo.Customers.Name FROM            dbo.Sale INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date  between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + shift + "'  and dbo.Sale.BillType='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.Terminal='" + System.Environment.MachineName + "' GROUP BY dbo.Customers.Name";

                }
                else
                {
                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash ,count(*) as p  FROM         Sale where (Date  between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Receivable'  and BillStatus='Paid' and Terminal='" + System.Environment.MachineName + "'";
                    q = "SELECT        SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash, COUNT(*) AS p, dbo.Customers.Name FROM            dbo.Sale INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date  between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Receivable'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.Customers.Name";

                }
                double totalrecv = 0, totalqtyrecv = 0;
                DataSet receivables = new DataSet();
                receivables = objcore.funGetDataSet(q);
                for (int h = 0; h < receivables.Tables[0].Rows.Count; h++)
                {
                    try
                    {
                        temp = receivables.Tables[0].Rows[h]["p"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        totalqtyrecv = totalqtyrecv + Convert.ToDouble(temp);
                        temp = receivables.Tables[0].Rows[h]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        totalrecv = totalrecv + Convert.ToDouble(temp);
                    }
                    catch (Exception ex)
                    {

                    }
                }

                nm = "Receivables :   (" + totalqtyrecv + ")";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }

                totalpayment = totalpayment + Math.Round(totalrecv, 3);
                temp = totalrecv.ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);


                for (int h = 0; h < receivables.Tables[0].Rows.Count; h++)
                {
                    try
                    {
                        nm = "   " + receivables.Tables[0].Rows[h]["Name"].ToString();

                        temp = receivables.Tables[0].Rows[h]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        totalrecv = Convert.ToDouble(temp);

                        for (int j = nm.Length; j < space.Length - 4; j++)
                        {
                            nm += " ";
                        }
                        if (!temp.Contains("."))
                        {
                            temp = temp + ".00";
                        }
                        for (int j = temp.Length; j < 8; j++)
                        {
                            temp = " " + temp;
                        }
                        bw.NormalFont(nm + temp);
                    }
                    catch (Exception ex)
                    {

                    }
                }



                temp = Math.Round(cashrecv, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("    Cash Receivables :  " + temp);
                temp = Math.Round(cardrecv, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("    Card Receivables :  " + temp);

                totalpayment = Math.Round(totalpayment, 3);


                nm = "Total Payments :";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }

                temp = Math.Round(totalpayment, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);

                
                bw.NormalFont(print2);
            }
            if (printcat("Show Checkout on Sales Report") == "Enabled")
            {
                bw.LargeText("        Check Out");
                bw.NormalFont(print);

                //nm = "Open Bills :  (" + openp + ")";
                //for (int j = nm.Length; j < space.Length; j++)
                //{
                //    nm += " ";
                //}
                //temp = Math.Round(openchks(date), 3).ToString();
                //if (!temp.Contains("."))
                //{
                //    temp = temp + ".00";
                //}
                //for (int j = temp.Length; j < 12; j++)
                //{
                //    temp = " " + temp;
                //}
                //bw.NormalFont(nm + temp);

                nm = "Calculated Cash :";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                temp = Math.Round(calculatedcash, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);

                nm = "Drawer Float :";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                temp = Math.Round(drawerfloat, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);

                nm = "Total :";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                temp = Math.Round(totalcash, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);

                nm = "Declared Cash :";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                temp = Math.Round(declared, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);

                nm = "Over/Short :";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                temp = Math.Round(over, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".00";
                }
                for (int j = temp.Length; j < 12; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont(nm + temp);

                //nm = "Banking Total :";
                //for (int j = nm.Length; j < space.Length; j++)
                //{
                //    nm += " ";
                //}
                //temp = Math.Round(bankingtotal, 3).ToString();
                //if (!temp.Contains("."))
                //{
                //    temp = temp + ".00";
                //}
                //for (int j = temp.Length; j < 12; j++)
                //{
                //    temp = " " + temp;
                //}
                //bw.NormalFont(nm + temp);
                bw.NormalFont(print2);
            }
            if (printcat("Show Audit on Sales Report") == "Enabled")
            {
                bw.LargeText("         Audit");
                bw.NormalFont(print);
                DataSet dsordrs = new DataSet();
                string dineid = dt.Rows[0]["DinIn"].ToString();
                string tk = dt.Rows[0]["TakeAway"].ToString();
                string del = dt.Rows[0]["delivery"].ToString();
                string tor = dt.Rows[0]["Torders"].ToString();
                string dlor = dt.Rows[0]["Dlorders"].ToString();
                string dor = dt.Rows[0]["Dorders"].ToString();

                temp = dineid.ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Dine In :     (" + dor + ")" + temp);

                temp = dineinorders.ToString();

                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("  Orders/Guests :   " + temp);


                temp = avgdinein.ToString();

                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("  Avg Per Guest :   " + temp);

                temp = avgdineintable.ToString();

                for (int j = temp.Length; j < space.Length - 8; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("  Avg Per Table :   " + temp);




                temp = tk.ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 4; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Take Away :   (" + tor + ")" + temp);

                temp = del.ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 4; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Delivery :    (" + dlor + ")" + temp);
                temp = Math.Round(totalchks, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 4; j++)
                {
                    temp = " " + temp;
                }
                temp = Math.Round(totalchks, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 4; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Total Checks :   " + temp);
                temp = Math.Round(checks, 3).ToString();
                if (!temp.Contains("."))
                {
                    temp = temp + ".0";
                }
                for (int j = temp.Length; j < space.Length - 4; j++)
                {
                    temp = " " + temp;
                }
                bw.NormalFont("Average Check :  " + temp);
                bw.NormalFont(print2);
            }
            bw.FeedLines(2);
        }
        public string printername(string type)
        {
            string name = "";

            DataSet ds = new DataSet();
            string q = "select * from printers where type='" + type + "'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                name = ds.Tables[0].Rows[0]["name"].ToString();
            }
            return name;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            getusers();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            getusers();
        }
    }
}
