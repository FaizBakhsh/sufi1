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
namespace POSRestaurant.Reports
{
    public partial class frmSalesItemTill : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmSalesItemTill()
        {
            InitializeComponent();

        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptShiftSalepos rptDoc = new SaleReports.rptShiftSalepos();
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
           
            // Just set the name of data table
            ds.DataTable1.Merge(dtuser);
            DataTable dtmenu = new DataTable();
            dtmenu.TableName = "Crystal Report Menu";
           
            // Just set the name of data table
            ds.MenuGroup.Merge(dtmenu);
            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("Addrs", phone);
            rptDoc.SetParameterValue("phn", address);
            rptDoc.SetParameterValue("report", "Sales Report");
            //rptDoc.SetParameterValue("date", dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            //crystalReportViewer1.ReportSource = rptDoc;

        }
        
        DataSet dscompany = new DataSet();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
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
            dat.Columns.Add("cashgst", typeof(double));
            dat.Columns.Add("cardgst", typeof(double));
            


            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }
            double gross = 0,cashgst=0,visagst=0, gst = 0, discount = 0, net = 0, cash = 0, credit = 0, master = 0, recvble = 0, service = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
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
                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst,SUM(servicecharges) AS service, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount FROM         Sale where  (Date = '" + dateTimePicker1.Text + "' )  and billstatus='Paid'";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //try
                        //{
                        //    string q1 = "SELECT        SUM(dbo.DiscountIndividual.Discount) AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid  where  (dbo.Sale.Date ='" + dateTimePicker1.Text + "')   and dbo.Sale.billstatus='Paid'";
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
                        service = Convert.ToDouble(ds.Tables[0].Rows[0]["service"].ToString());
                        net = net - discount;
                        gross = gross + gst + service;
                        net = Math.Round(net, 2);
                        try
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst,SUM(servicecharges) AS service, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount FROM         Sale where  (Date = '" + dateTimePicker1.Text + "' )  and billstatus='Paid' and billtype like 'Cash%'";
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
                            }
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst,SUM(servicecharges) AS service, SUM(TotalBill) AS netsale, SUM(DiscountAmount) AS discount FROM         Sale where  (Date = '" + dateTimePicker1.Text + "' )  and billstatus='Paid' and billtype like 'Visa%'";
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

                    // q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker1.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";

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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker1.Text + "' ) and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";

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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.BillType.type='Master Card'  and dbo.Sale.BillStatus='Paid'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";

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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";

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
                    q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.BillType.type='Receivable'  and dbo.Sale.BillStatus='Paid'";
                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date ='" + dateTimePicker1.Text + "') and dbo.Sale.BillType='Credit Card' and dbo.Sale.BillStatus='Paid'";

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
                    q = "SELECT  SUM(cashin) AS cashin,SUM(cashout) AS cashout  FROM  shiftcash where  (Date ='" + dateTimePicker1.Text + "')";
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
                    q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where  (Date ='" + dateTimePicker1.Text + "') and BillStatus='Refund' ";
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
                    q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where  (Date ='" + dateTimePicker1.Text + "') and BillStatus='Refund' ";
                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Refund' and (dbo.Sale.Date ='" + dateTimePicker1.Text + "') ";
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
                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Void' and (dbo.Sale.Date ='" + dateTimePicker1.Text + "') ";
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
            double totlorder = 0;// (Convert.ToDouble(Torders) + Convert.ToDouble(Dorders) + Convert.ToDouble(Dlorders));
            try
            {
                ds = new DataSet();
                string q = "SELECT     count(id) AS cash  FROM         Sale where  (Date ='" + dateTimePicker1.Text + "')";
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
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, null, totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recvble,cashgst,visagst);

            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recvble, cashgst, visagst);
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale);
            }

            return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
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
               

                string offSetString = new string(' ', printer.RecLineChars / 3);
                string Bold = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'b', (byte)'C' });
                PrintTextLine(printer, new string('=', (printer.RecLineChars)));
               
                DataTable dt = new DataTable();
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                
                double net = 0, gst = 0, dis = 0, refund = 0, gross = 0, cashgst = 0, cardgst = 0;
                double cash = 0, credit = 0, master = 0, recvble = 0, service = 0, checks = 0, totalchks = 0, delivery = 0, visa = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, totalcash = 0;

                
                string qq1 = "";
                
                

                
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
                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash , dbo.BillType.type FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + date + "') and dbo.BillType.type like 'Visa%'  ='" + dateTimePicker1.Text + "' GROUP BY dbo.BillType.type";
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
            PrintTextLine(printer, offSetString + (Bold + "Items Sale Summary"));
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            PrintTextLine(printer, String.Format("DATE : {0}", Bold1 + Convert.ToDateTime(dateTime).ToShortDateString() + "   " + DateTime.Now.ToShortTimeString()));
           

            PrintTextLine(printer, new string('-', printer.RecLineChars));
            //printer.PrintNormal(2, " " + Environment.NewLine);
           

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
                string q = "select * from View_1 where date ='" + dateTimePicker1.Text + "'";
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
                string q = "select * from View_2 where date ='" + dateTimePicker1.Text + "'";
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
        private void vButton2_Click(object sender, EventArgs e)
        {
           
           
           
            string type = printtype();
            if (type == "opos")
            {
                vButton2.Text = "Please Wait";
                vButton1.Enabled = false;
                vButton2.Enabled = false;
                PrintReceipt("", dateTimePicker1.Text, "", "day");
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
                    temp = "37";
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
        string cashp = "0", deliveryp = "0", dineinp = "0", cts = "", openp = "0", takeawayp = "0", visap = "0", masterp = "0", ctsp = "0", refundp = "0";
        private void PrintReceiptshiftend(BinaryWriter bw, string cashier, string date, string shift, string type)
        {
            
            DataSet dsshft = new DataSet();
            string temp = "", shiftname = "";
            dsshft = objCore.funGetDataSet("select * from Shifts where id='" + shift + "'");
            if (dsshft.Tables[0].Rows.Count > 0)
            {
                shiftname = dsshft.Tables[0].Rows[0]["Name"].ToString();
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

            bw.LargeText("Items Sales Summary");
            bw.NormalFont(print);
            bw.NormalFont("DATE :  " + Convert.ToDateTime(date).ToShortDateString() + " " + DateTime.Now.ToShortTimeString());
            bw.NormalFont(print);
            DataSet ds = new DataSet();
            string q = "";
            double total = 0;
            double totalqty = 0;
            {
                string showprice = "yes";
                try
                {
                    q = "SELECT     dbo.Rights.Status, dbo.Forms.Forms, dbo.Rights.Userid FROM         dbo.Rights INNER JOIN                      dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where dbo.Rights.Userid='"+userid+"' and dbo.Forms.Forms='Show MenuItem Price 3inch Report'";
                    DataSet ds1 = new DataSet();
                    ds1 = objcore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        showprice = ds1.Tables[0].Rows[0]["status"].ToString();
                    }
                }
                catch (Exception ex)
                {
                    
                }
                total = 0;
                bw.LargeText(" Items Wise Sales");
                bw.NormalFont(print);
                q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date ='" + dateTimePicker1.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
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
                    for (int j = name.Length; j < space.Length; j++)
                    {
                        name += " ";
                    }
                    temp = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    string sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
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
                    if (showprice == "yes")
                    {
                        bw.NormalFont(name + qty + " ," + sum);
                    }
                    else
                    {
                        bw.NormalFont(name + " ," + qty.ToString());
                    }

                }
                q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date ='" + dateTimePicker1.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0 and dbo.Saledetails.price>0  GROUP BY dbo.RuntimeModifier.name";
                ds = new DataSet();
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
                    string sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
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
                    if (showprice == "yes")
                    {
                        bw.NormalFont(name + qty + " ," + sum);
                    }
                    else
                    {
                        bw.NormalFont(name + " ," + qty);
                    }
                    totalqty = totalqty + Convert.ToDouble(temp);

                }

                q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (Sale.Date ='" + dateTimePicker1.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0 and dbo.Saledetails.price>0  GROUP BY dbo.Modifier.name";
                ds = new DataSet();
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
                    string sum = (Math.Round(Convert.ToDouble(temp), 3)).ToString();
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
                    if (showprice == "yes")
                    {
                        bw.NormalFont(name + qty + " ," + sum);
                    }
                    else
                    {
                        bw.NormalFont(name + " ," + qty);
                    }
                }
                bw.NormalFont(print);
                string nm = "Total";
                for (int j = nm.Length; j < space.Length; j++)
                {
                    nm += " ";
                }
                string summ = (Math.Round(total, 3)).ToString();
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
                if (showprice == "yes")
                {
                    bw.NormalFont(nm + tqty + " ," + summ);
                }
                else
                {
                    bw.NormalFont(nm + " ," + tqty);
                }
                bw.NormalFont(print2);
            }

            bw.NormalFont(print2);
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
    }
}
