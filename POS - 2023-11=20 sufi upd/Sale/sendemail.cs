using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class sendemail : Form
    {
        ReportDocument cryRpt;
        public sendemail()
        {
            InitializeComponent();
        }
        public string date = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptdaily rptDoc = new Reports.SaleReports.rptdaily();
            POSRestaurant.Reports.SaleReports.DsUserDaily ds = new Reports.SaleReports.DsUserDaily();
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
            rptDoc.SetParameterValue("Addrs",address );
            rptDoc.SetParameterValue("phn",phone );
            rptDoc.SetParameterValue("report", "Sales Report");
            rptDoc.SetParameterValue("date", "as on  " + date);
            rptDoc.SetParameterValue("visa", visa);
            rptDoc.SetParameterValue("visaamounts", visaamounts);
            rptDoc.SetParameterValue("deliverysourcetitle", "");
            rptDoc.SetParameterValue("deliverysourcedata", "");
            rptDoc.SetParameterValue("takeawayorders", takawayorders);
            rptDoc.SetParameterValue("dineinorders", dineinorders);
            rptDoc.SetParameterValue("deliveryorders", deliveryorders);
            rptDoc.SetParameterValue("recvname", recvname);
            rptDoc.SetParameterValue("recvamount", recvamount);
            crystalReportViewer1.ReportSource = rptDoc;
            cryRpt = rptDoc;
        }
        string recvname = "", recvamount = "";
        DataSet dscompany = new DataSet();
        DataSet dsbranch = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");
            dsbranch = objCore.funGetDataSet("select * from Branch");
        }
        public string visa = "", visaamounts = "";
        string delievrysourcetitle = "", delievrysourcedata = "", takawayorders = "", dineinorders = "", deliveryorders = "";
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
            dat.Columns.Add("CashGST", typeof(double));
            dat.Columns.Add("CardGST", typeof(double));
            dat.Columns.Add("CashDiscount", typeof(double));
            dat.Columns.Add("CardDiscount", typeof(double));
            dat.Columns.Add("CashRecv", typeof(double));
            dat.Columns.Add("CardRecv", typeof(double));
            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }
            //double gross = 0, gst = 0, discount = 0, net = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
            //string Dlorders = "0", Torders = "0", Dorders = "0", RefundNo = "0", carhopeorders = "0";
            double gross = 0, cashgst = 0, cardgst = 0, gst = 0, cashdis = 0, carddis = 0, discount = 0, net = 0, service = 0, cashrecv = 0, cardrecv = 0, recv = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
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
                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount,sum(servicecharges) as serv FROM         Sale where  (Date = '" + date + "')  and billstatus='Paid'";
                    ds = objcore.funGetDataSet(q);
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
                        gross = gross + service;
                        DataSet dscash = new DataSet();
                        try
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date='" + date + "')  and billstatus='Paid' and GSTtype='Cash'";


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
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date='" + date + "')  and billstatus='Paid' and GSTtype='Card'";

                            dscash = new DataSet();
                            dscash = objcore.funGetDataSet(q);
                            if (dscash.Tables[0].Rows.Count > 0)
                            {
                                temp = dscash.Tables[0].Rows[0]["gst"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                cardgst = Convert.ToDouble(temp);
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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + date + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date = '"+ date +"') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";
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
                    q = "";// "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date = '"+ date +"') and BillType='Cash'";
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + date + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date = '"+ date +"') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";

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
                    //q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date = '"+ date +"') and BillType='Master Card'";
                    //q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '"+ date +"')  and dbo.BillType.type='Master Card'";
                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date = '" + date + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";

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
                    // q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and BillType='Credit Card'";
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + date + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date = '"+ date +"') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";

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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date='" + date + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                           
                    ds = objcore.funGetDataSet(q);
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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date= '" + date + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date= '" + date + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";

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
                    q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  (Date = '" + date + "') ";
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
                    q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where  (Date = '" + date + "') and BillStatus='Refund' ";
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
                    q = "SELECT     COUNT(dbo.Saledetails.Id) AS count  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Saledetails.Status = 'Void') and  (dbo.Sale.Date = '" + date + "')";
                    ds = objcore.funGetDataSet(q);
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
                string q = "SELECT     count(id) AS cash  FROM         Sale where  (Date = '" + date + "')";
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

            try
            {
                string q = "";
                ds = new DataSet();


                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date='" + date + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid'";

                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
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
                    takawayorders = temp;
                }
                else
                {
                    takeaway = 0;
                    takawayorders = "0";
                }


            }
            catch (Exception ex)
            {


            }
            try
            {
                string q = "";
                ds = new DataSet();

                
                        q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date='" + date + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                   

                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
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
                    dineinorders = temp;

                    try
                    {
                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where (dbo.Sale.Date='" + date + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                           
                        ds = new DataSet();
                        ds = objcore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            temp = ds.Tables[0].Rows[0]["count"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            dineinorders = dineinorders + "/" + temp;
                        }

                    }
                    catch (Exception ex)
                    {


                    }

                }
                else
                {
                    dinin = 0;
                    dineinorders = "0";
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                string q = "";
                ds = new DataSet();

                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date='" + date + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid'";
                   
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
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
                    deliveryorders = temp;
                }
                else
                {
                    delivery = 0;
                    deliveryorders = "0";
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
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, null, totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv, cashgst, cardgst, cashdis, carddis, cashrecv, cardrecv);

            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv, cashgst, cardgst, cashdis, carddis, cashrecv, cardrecv);
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale);
            }

            return dat;
        }
        public DataTable getAllOrdersmenu()
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("MenuGroup", typeof(string));
                dtrptmenu.Columns.Add("Count", typeof(string));
                dtrptmenu.Columns.Add("Sum", typeof(double));
                dtrptmenu.Columns.Add("CashSales", typeof(double));
                dtrptmenu.Columns.Add("CardSales", typeof(double));

                DataSet ds = new DataSet();
                string q = "";

                q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + date + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name";
                q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date='" + date + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                     


                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                    dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString(), getvalue(ds.Tables[0].Rows[i]["id"].ToString(), "Cash"), getvalue(ds.Tables[0].Rows[i]["id"].ToString(), "Card"));
               
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
        protected double getvalue(string id, string type)
        {
            double val = 0;
            DataSet ds = new DataSet();
            string q = "";

            try
            {


                q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date= '" + date + "') and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "' and sale.GSTtype like '" + type + "%' GROUP BY dbo.MenuGroup.Name";



                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0]["sum"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    val = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

            }
            return val;
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


                q = "SELECT     SUM(dbo.Sale.NetBill) AS sum, COUNT(dbo.Sale.NetBill) AS count,SUM(dbo.Sale.DiscountAmount) AS dis, dbo.Users.Name,dbo.Users.id FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id   WHERE     (Sale.Date = '" + date + "') and Sale.BillStatus='Paid'  GROUP BY dbo.Users.Name,dbo.Users.id ";
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
        public string userid = "";
        private void sendemail_Load(object sender, EventArgs e)
        {
            bindreport();
            try
            {

                string clientName = "", email = "", password = "", to = "", subject = "", body = "", cc1 = "", cc2 = "", cc3 = "", path = "";
                int port = 26;
                bool ssl = false;
                string qq = "select * from Mailsetting";
                DataSet dsmail = new DataSet();
                dsmail = objcore.funGetDataSet(qq);
                try
                {
                    if (dsmail.Tables[0].Rows.Count > 0)
                    {
                        path = dsmail.Tables[0].Rows[0]["path"].ToString();
                        try
                        {
                            ExportOptions CrExportOptions;
                            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                            CrDiskFileDestinationOptions.DiskFileName = path + ":\\Sales report " + date + ".pdf";
                            CrExportOptions = cryRpt.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            cryRpt.Export();
                        }
                        catch (Exception ex)
                        {
                            // MessageBox.Show(ex.ToString());
                        }
                        clientName = dsmail.Tables[0].Rows[0]["host"].ToString();
                        email = dsmail.Tables[0].Rows[0]["mailfrom"].ToString();
                        password = dsmail.Tables[0].Rows[0]["password"].ToString();
                        to = dsmail.Tables[0].Rows[0]["mailto"].ToString();
                        subject = dsmail.Tables[0].Rows[0]["head"].ToString();
                        body = dsmail.Tables[0].Rows[0]["mailto"].ToString();
                        port = Convert.ToInt32(dsmail.Tables[0].Rows[0]["port"].ToString());
                        cc1 = dsmail.Tables[0].Rows[0]["cc1"].ToString();
                        cc2 = dsmail.Tables[0].Rows[0]["cc2"].ToString();
                        cc3 = dsmail.Tables[0].Rows[0]["cc3"].ToString();
                        try
                        {
                            ssl = Convert.ToBoolean(dsmail.Tables[0].Rows[0]["ssl"].ToString());
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                catch (Exception ex)
                {


                }

                SmtpClient client = new SmtpClient(clientName, port);
                client.UseDefaultCredentials = false;
                client.EnableSsl = ssl;
                client.Credentials = new System.Net.NetworkCredential(email, password);
                MailMessage myMail = new MailMessage();
                MailAddress addTo = new MailAddress(to);
                myMail.IsBodyHtml = true;
                myMail.Subject = dsbranch.Tables[0].Rows[0]["BranchName"] + " " + date + " Sales Report";
                myMail.Body = "Sale Report of " + dsbranch.Tables[0].Rows[0]["BranchName"] + " of Date " + date + " is attached";
                Attachment at = new Attachment((path + "://Sales report " + date + ".pdf"));
                myMail.Attachments.Add(at);
                myMail.To.Add(addTo);
                if (cc1.Length > 0)
                {
                    MailAddress copy = new MailAddress(cc1);
                    myMail.CC.Add(copy);
                }
                if (cc2.Length > 0)
                {
                    MailAddress copy = new MailAddress(cc2);
                    myMail.CC.Add(copy);
                }
                if (cc3.Length > 0)
                {
                    MailAddress copy = new MailAddress(cc3);
                    myMail.CC.Add(copy);
                }
                //myMail.CC.Add();
                myMail.Priority = MailPriority.High;
                myMail.From = new MailAddress(email);
                client.Send(myMail);
                string q = "insert into log (Name, Time, Description,userid) values ('Day End Email','" + date + "','Email Sent at "+ DateTime.Now+"','" + userid + "')";
               objCore.executeQuery(q);
            }
            catch (Exception ex)
            {
                string q = "insert into log (Name, Time, Description,userid) values ('Day End Email','" + date + "','Erro Sending Email at " + DateTime.Now + "\n" + ex.InnerException + "','" + userid + "')";
                objCore.executeQuery(q);
            }
            this.Close();
        }
    }
}
