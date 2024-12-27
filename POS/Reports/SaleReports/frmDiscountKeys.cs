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
namespace POSRestaurant.Reports
{
    public partial class frmDiscountKeys : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmDiscountKeys()
        {
            InitializeComponent();
           
        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptDiscountKeys rptDoc = new SaleReports.rptDiscountKeys();
            POSRestaurant.Reports.SaleReports.dsDiscountkeys ds = new SaleReports.dsDiscountkeys();
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
            //rptDoc.SetParameterValue("Comp", company);
            //rptDoc.SetParameterValue("Addrs", phone);
            //rptDoc.SetParameterValue("phn", address);
            //rptDoc.SetParameterValue("report", "Sales Report");
            //rptDoc.SetParameterValue("date", dateTimePicker1.Text +" to "+ dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;

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
            dat.Columns.Add("Date", typeof(DateTime));
            dat.Columns.Add("Name", typeof(string));
            dat.Columns.Add("Discount", typeof(double));
            dat.Columns.Add("amount", typeof(double));

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
                ds = new DataSet();
                q = "SELECT     SUM(dbo.Sale.DiscountAmount) AS amount, dbo.Sale.Discount, dbo.Sale.Date, dbo.Users.Name FROM         dbo.Users INNER JOIN                      dbo.DiscountTrack ON dbo.Users.Id = dbo.DiscountTrack.userid RIGHT OUTER JOIN                      dbo.Sale ON dbo.DiscountTrack.saleid = dbo.Sale.Id WHERE    (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and  (dbo.Sale.Discount > '0') GROUP BY dbo.Sale.Discount, dbo.Sale.Date, dbo.Users.Name  order by dbo.Sale.Date, dbo.Sale.Discount";
                ds = objcore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    dat.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToShortDateString(), ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["Discount"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["amount"].ToString()));

                }
                
            }
            catch (Exception ex)
            {


            }

            return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Text = "Please Wait";
            vButton1.Enabled = false;
            bindreport();
            vButton1.Enabled = true;
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

            DataSet ds = new DataSet();
            string q = "select * from printtype";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                type = ds.Tables[0].Rows[0]["type"].ToString();
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
                double total = 0;
               
                
                
                DataTable dt = new DataTable();
                // Just set the name of data table
                dt.TableName = "Crystal Report";

                dt = getAllOrders();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["sum"].ToString()));
                    PrintLineItem(printer, dt.Rows[i]["date"].ToString(), dt.Rows[i]["Name"].ToString(), Convert.ToDouble(dt.Rows[i]["Discount"].ToString()), Math.Round(Convert.ToDouble(dt.Rows[i]["Amount"].ToString()), 2));
                    //total = total + Convert.ToDouble(ds.Tables[0].Rows[i]["sum"].ToString());
                }

                PrintTextLine(printer, new string('=', printer.RecLineChars));
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

        private void PrintLineItem(OPOSPOSPrinter printer, string date, string name, double quantity, double amount)
        {
            PrintText(printer, TruncateAt(Convert.ToDateTime(date).ToShortDateString(), 10));
            PrintText(printer, TruncateAt(name.ToString().PadLeft(7), 20));
            PrintText(printer, TruncateAt(quantity.ToString().PadLeft(7), 20));
            PrintTextLine(printer, TruncateAt((amount).ToString().PadLeft(16), 20));
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
            
            PrintTextLine(printer, offSetString + (Bold + "Discount Keys Report"));
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            PrintTextLine(printer,("for the period of "+dateTimePicker1.Text +" to "+dateTimePicker2.Text));
            
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            //printer.PrintNormal(2, " " + Environment.NewLine);
            PrintText(printer, Bold + "Date        ");
            PrintText(printer, Bold + "Name     ");
            PrintText(printer, Bold + "Dis(%)       ");
            PrintTextLine(printer, Bold + "Amount");
            PrintTextLine(printer, new string('=', printer.RecLineChars));

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

        private void vButton2_Click(object sender, EventArgs e)
        {
            string type = printtype();
            if (type == "opos")
            {
                PrintReceipt("", dateTimePicker2.Text, "", "day");
            }
        }
    }
}
                                       mys\POS\obj\Debug\POSRestaurant.Setting.AddKDS.resources
E:\timmys\POS\obj\Debug\POSRestaurant.Setting.AddCustomerInfo.resources
E:\timmys\POS\obj\Debug\POSRestaurant.Setting.AddPoints.resources
E:\timmys\POS\obj\Debug\POSRestaurant.Setting.AddStaff.resources
E:\timmys\POS\obj\Debug\POSRestaurant.Setting.AddRuntimeModifier.resources
E:\timmys\POS\obj\Debug\POSRestaurant.Setting.AddDiscount.resources
E:\timmys\POS\obj\Debug\POSRestaurant.Setting.DevicesSetting.resources
E:\timmys\POS\obj\Debug\POSRestaurant.test.resources
E:\timmys\POS\obj\Debug\POSRestaurant.csproj.GenerateResource.Cache
E:\timmys\POS\obj\Debug\MBLFront.exe.manifest
E:\timmys\POS\obj\Debug\MBLFront.application
E:\timmys\POS\obj\Debug\MBLFront.exe
E:\timmys\POS\obj\Debug\MBLFront.pdb
E:\timmys scroll\POS\bin\Debug\MBLFront.exe.config
E:\timmys scroll\POS\obj\Debug\Interop.VBA.dll
E:\timmys scroll\POS\obj\Debug\POSRestaurant.csproj.ResolveComReference.cache
E:\timmys scroll\POS\obj\Debug\MBLFront.exe
E:\timmys scroll\POS\obj\Debug\MBLFront.pdb
E:\timmys scroll\POS\bin\Debug\MBLFront.exe.manifest
E:\timmys scroll\POS\bin\Debug\MBLFront.application
E:\timmys scroll\POS\bin\Debug\MBLFront.exe
E:\timmys scroll\POS\bin\Debug\MBLFront.pdb
E:\timmys scroll\POS\bin\Debug\POSRetail.exe
E:\timmys scroll\POS\bin\Debug\POSRetail.pdb
E:\timmys scroll\POS\bin\Debug\Interop.VBA.dll
E:\timmys scroll\POS\obj\Debug\POSRestaurant.csprojResolveAssemblyReference.cache
E:\timmys scroll\POS\obj\Debug\POSRestaurant.Accounts.ChartofAccount.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.backup.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmadditems.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmAddstaff.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmManagestaff.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmOrderrpt.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmStockrpt.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.manageitems.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.receipttest.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.Salarymanagement.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmAddDishes.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmDishmanagement.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmSupplierAdd.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmSupplierManagement.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmAddItemPackingType.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmAddItemSubCategory.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmItemSubCategory.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmAddItemrecipe.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.ControlPanel.frmManageUsers.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.ControlPanel.frmAddEditUser.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.ControlPanel.frmGlobalSettings.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.admin.frmItemrecipemang.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.controls.BarCodeCtrl.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.forms.AddMovies.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.forms.AddMovieShows.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.forms.BackendForm.resources
E:\timmys scroll\POS\obj\Debug\POSRestaurant.forms.BillRecall.resources
E:\timmys scroll\POS\o