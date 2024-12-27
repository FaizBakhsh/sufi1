using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Speech.Synthesis;

using OposPOSPrinter_CCO;
using ArabicPrint;
//using Microsoft.PointOfService;
//using OposPOSPrinter_1_8_Lib;
using System.IO;
using System.Net.Mail;
using CrystalDecisions.Shared;
using System.Net;
namespace POSRestaurant.forms
{
    public partial class BackendForm : Form
    {
        //CashDrawer myCashDrawer;
        //PosExplorer explorer;

        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        //private void PrintReceipt()
        //{
           
        //    PosPrinter printer = GetReceiptPrinter();

        //    try
        //    {
        //        ConnectToPrinter(printer);
        //        PrintReceiptHeader(printer, "ABCDEF Pte. Ltd.", "123 My Street, My City,", "My State, My Country", "012-3456789", DateTime.Now, "ABCDEF");
        //        PrintLineItem(printer, "Item 1", 10, 99.99);
        //        PrintLineItem(printer, "Item 2", 101, 0.00);
        //        PrintLineItem(printer, "Item 3", 9, 0.1);
        //        PrintLineItem(printer, "Item 4", 1000, 1);

        //        PrintReceiptFooter(printer, 1, 0.1, 0.1, "THANK YOU FOR CHOOSING ABC Ptr. Ltd.");
        //    }
        //    finally
        //    {
        //        DisconnectFromPrinter(printer);
        //    }
        //}

        //private void DisconnectFromPrinter(PosPrinter printer)
        //{
        //    printer.Release();
        //    printer.Close();
        //}

        //private void ConnectToPrinter(PosPrinter printer)
        //{
        //    printer.Open();
        //    printer.Claim(10000);
        //    printer.DeviceEnabled = true;
        //}

        //private PosPrinter GetReceiptPrinter()
        //{
        //    PosExplorer posExplorer = new PosExplorer(this);
        //    DeviceInfo receiptPrinterDevice = posExplorer.GetDevice(DeviceType.PosPrinter, "pp88"); //May need to change this if you don't use a logicial name or use a different one.
        //    return (PosPrinter)posExplorer.CreateInstance(receiptPrinterDevice);
        //}

        //private void PrintReceiptFooter(PosPrinter printer, int subTotal, double tax, double discount, string footerText)
        //{
        //    string offSetString = new string(' ', printer.RecLineChars / 2);

        //    PrintTextLine(printer, new string('-', (printer.RecLineChars / 3) * 2));
        //    PrintTextLine(printer, offSetString + String.Format("SUB-TOTAL     {0}", subTotal.ToString("#0.00")));
        //    PrintTextLine(printer, offSetString + String.Format("TAX           {0}", tax.ToString("#0.00")));
        //    PrintTextLine(printer, offSetString + String.Format("DISCOUNT      {0}", discount.ToString("#0.00")));
        //    PrintTextLine(printer, offSetString + new string('-', (printer.RecLineChars / 3)));
        //    PrintTextLine(printer, offSetString + String.Format("TOTAL         {0}", (subTotal - (tax + discount)).ToString("#0.00")));
        //    PrintTextLine(printer, offSetString + new string('-', (printer.RecLineChars / 3)));
        //    PrintTextLine(printer, String.Empty);

        //    //Embed 'center' alignment tag on front of string below to have it printed in the center of the receipt.
        //    PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' }) + footerText);

        //    //Added in these blank lines because RecLinesToCut seems to be wrong on my printer and
        //    //these extra blank lines ensure the cut is after the footer ends.
        //    PrintTextLine(printer, String.Empty);
        //    PrintTextLine(printer, String.Empty);
        //    PrintTextLine(printer, String.Empty);
        //    PrintTextLine(printer, String.Empty);
        //    PrintTextLine(printer, String.Empty);

        //    //Print 'advance and cut' escape command.
        //    PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
        //}

        //private void PrintLineItem(PosPrinter printer, string itemCode, int quantity, double unitPrice)
        //{
        //    PrintText(printer, TruncateAt(itemCode.PadRight(9), 9));
        //    PrintText(printer, TruncateAt(quantity.ToString("#0.00").PadLeft(9), 9));
        //    PrintText(printer, TruncateAt(unitPrice.ToString("#0.00").PadLeft(10), 10));
        //    PrintTextLine(printer, TruncateAt((quantity * unitPrice).ToString("#0.00").PadLeft(10), 10));
        //}

        //private void PrintReceiptHeader(PosPrinter printer, string companyName, string addressLine1, string addressLine2, string taxNumber, DateTime dateTime, string cashierName)
        //{
        //    PrintTextLine(printer, companyName);
        //    PrintTextLine(printer, addressLine1);
        //    PrintTextLine(printer, addressLine2);
        //    PrintTextLine(printer, taxNumber);
        //    PrintTextLine(printer, new string('-', printer.RecLineChars / 2));
        //    PrintTextLine(printer, String.Format("DATE : {0}", dateTime.ToShortDateString()));
        //    PrintTextLine(printer, String.Format("CASHIER : {0}", cashierName));
        //    PrintTextLine(printer, String.Empty);
        //    PrintText(printer, "item      ");
        //    PrintText(printer, "qty       ");
        //    PrintText(printer, "Unit Price ");
        //    PrintTextLine(printer, "Total      ");
        //    PrintTextLine(printer, new string('=', printer.RecLineChars));
        //    PrintTextLine(printer, String.Empty);

        //}

        //private void PrintText(PosPrinter printer, string text)
        //{
        //    if (text.Length <= printer.RecLineChars)
        //        printer.PrintNormal(PrinterStation.Receipt, text); //Print text
        //    else if (text.Length > printer.RecLineChars)
        //        printer.PrintNormal(PrinterStation.Receipt, TruncateAt(text, printer.RecLineChars)); //Print exactly as many characters as the printer allows, truncating the rest.
        //}

        //private void PrintTextLine(PosPrinter printer, string text)
        //{
        //   // printer.PrintNormal(PrinterStation.Receipt, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, 112, 48, 55, 121 }));
        //    if (text.Length < printer.RecLineChars)
        //        printer.PrintNormal(PrinterStation.Receipt, text + Environment.NewLine); //Print text, then a new line character.
        //    else if (text.Length > printer.RecLineChars)
        //        printer.PrintNormal(PrinterStation.Receipt, TruncateAt(text, printer.RecLineChars)); //Print exactly as many characters as the printer allows, truncating the rest, no new line character (printer will probably auto-feed for us)
        //    else if (text.Length == printer.RecLineChars)
        //        printer.PrintNormal(PrinterStation.Receipt, text + Environment.NewLine); //Print text, no new line character, printer will probably auto-feed for us.
        //}

        //private string TruncateAt(string text, int maxWidth)
        //{
        //    string retVal = text;
        //    if (text.Length > maxWidth)
        //        retVal = text.Substring(0, maxWidth);

        //    return retVal;
        //}
        public BackendForm()
        {
            InitializeComponent();              
        }
        //public void OpenCashDrawer()
        //{
        //    explorer = new PosExplorer(this);
        //    DeviceInfo ObjDevicesInfo = explorer.GetDevice("CashDrawer", "CR Demo");
        //    myCashDrawer =explorer.CreateInstance(ObjDevicesInfo) as CashDrawer;        
        //    myCashDrawer.Open();
        //    myCashDrawer.Claim(1000);
        //    myCashDrawer.DeviceEnabled = true;
        //    myCashDrawer.OpenDrawer();
        //    myCashDrawer.DeviceEnabled = false;
        //    myCashDrawer.Release();
        //    myCashDrawer.Close();
        //}
        private void cardFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            

            MainForm obj = new MainForm();
           // obj.MdiParent = this.MdiParent;
           // obj.WindowState = FormWindowState.Maximized;
            obj.Show();
        }

        private void addRawItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawItems.AddRawItems obj = new RawItems.AddRawItems();
            obj.Show();
        }

        private void BackendForm_Load(object sender, EventArgs e)
        {
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tabControl1, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, panel7, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tabPage1, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tabPage2, new object[] { true });
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tableLayoutPanel1, new object[] { true });
            SqlDataReader sdr = null;
            try
            {
                sdr = objcore.funGetDataReader1("select * from DeviceSetting where device='Main Screen Location'");
                if (sdr.Read())
                {
                    string show = (sdr["Status"].ToString());
                    try
                    {
                        if (show == "Enabled")
                        {
                            Screen[] sc;
                            sc = Screen.AllScreens;
                            this.StartPosition = FormStartPosition.Manual;

                            this.Location = Screen.AllScreens[Convert.ToInt32(sdr["no"].ToString())].WorkingArea.Location;
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }

            }
            catch (Exception ex)
            {

            }
            //try
            //{
            //   DataSet dsgst = new DataSet();
            //    dsgst = objcore.funGetDataSet("select top(1) * from DayEnd  order by id desc");
            //    if (dsgst.Tables[0].Rows.Count > 0)
            //    {
            //      string  date = dsgst.Tables[0].Rows[0]["Date"].ToString();
            //        DateTime datetemp = Convert.ToDateTime(date);
            //        date = datetemp.ToShortDateString();
                   
            //        string daystatus = dsgst.Tables[0].Rows[0]["DayStatus"].ToString();
            //        if (daystatus == "Close")
            //        {
            //            btnday.Text = "Day Start";
            //        }
            //        else
            //        {
            //            btnday.Text = "Day End";
            //        }
            //    }
            //    else
            //    {
            //        btnday.Text = "Day Start";
            //    }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }

        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void BackendForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult rd = MessageBox.Show("Are You Sure to Exit ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rd == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void menuGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale obj = new POSRestaurant.Reports.SaleReports.FrmMwnuGroupSale();
            obj.Show();
        }

        private void menuItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmMenuItemSale obj = new POSRestaurant.Reports.SaleReports.FrmMenuItemSale();
            obj.Show();
        }

        private void cashierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmCashierSale obj = new POSRestaurant.Reports.SaleReports.FrmCashierSale();
            obj.Show();
        }

        private void discountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmDiscountSale obj = new POSRestaurant.Reports.SaleReports.FrmDiscountSale();
            obj.Show();
        }

        private void voidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmVoidSale obj = new POSRestaurant.Reports.SaleReports.FrmVoidSale();
            obj.Show();
        }

        private void refundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmRefundSale obj = new POSRestaurant.Reports.SaleReports.FrmRefundSale();
            obj.Show();
        }

        private void hourlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmHourlySale obj = new POSRestaurant.Reports.SaleReports.FrmHourlySale();
            obj.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmTerminalSale obj = new POSRestaurant.Reports.SaleReports.FrmTerminalSale();
            obj.Show();
        }

        private void orderWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmOrderWiseSale obj = new POSRestaurant.Reports.SaleReports.FrmOrderWiseSale();
            obj.Show();
        }

        private void paymentWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.SaleReports.FrmPaymentWiseSale obj = new POSRestaurant.Reports.SaleReports.FrmPaymentWiseSale();
            obj.Show();
        }

        private void dailyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.FrmUserSale obj = new POSRestaurant.Reports.FrmUserSale();
            obj.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           


           
              
            


        }

        private void uploadeSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataUploading obj = new DataUploading(this);
            obj.Show();
            this.Enabled = false;
        }

        private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataBackUp obj = new DataBackUp(this);
            obj.Show();
            this.Enabled = false;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            Reports obj = new Reports();
            
            obj.Show();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            MainForm obj = new MainForm();
            // obj.MdiParent = this.MdiParent;
            // obj.WindowState = FormWindowState.Maximized;
            obj.Show();
        }
       
        private void vButton3_Click(object sender, EventArgs e)
        {
            //var client = new WebClient();
            //Uri address = new Uri("http://www.posiflexpakistan.com/uploads/video/26.mp4");
            //client.DownloadFileAsync(address, @"d:\26.mp4");


            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            RawItems.AddRawItems obj = new RawItems.AddRawItems();
            obj.Show();
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            DataUploading obj = new DataUploading(this);
            obj.Show();
            this.Enabled = false;
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
           
            //Button b = sender as Button;
            //string authentication = objcore.authentication(b.Text);
            //if (authentication == "yes")
            //{

            //}
            //else
            //{
            //    MessageBox.Show("You are not allowed to view this");
            //    return;
            //}
            DownloadDiscounts obj = new DownloadDiscounts();
            obj.Show();
           // this.Enabled = false;
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            DialogResult rd = MessageBox.Show("Are You Sure to Exit ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rd == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            //POSRestaurant.RawItems.Purchase obj = new  POSRestaurant.RawItems.Purchase();
            POSRestaurant.RawItems.Purchase obj = new RawItems.Purchase();
            obj.Show();
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmClosingInventory obj = new POSRestaurant.Reports.Inventory.frmClosingInventory();
            obj.Show();
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmConsumedInventory obj = new POSRestaurant.Reports.Inventory.frmConsumedInventory();
            obj.Show();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmReceivedInventory obj = new POSRestaurant.Reports.Inventory.frmReceivedInventory();
            obj.Show();
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void vButton11_Click(object sender, EventArgs e)
        {
            //POSRestaurant.Reports.testrdlc obj = new POSRestaurant.Reports.testrdlc();
            //obj.Show();
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            Reports obj = new Reports();

            obj.Show();
        }

        private void vButton1_Click_2(object sender, EventArgs e)
        {
            
            

            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Setting.Rights obj = new  Setting.Rights();
            
            obj.Show();
        }

        private void BackendForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            {
               // MessageBox.Show(e.KeyChar.ToString());
            }
        }

        private void vButton8_Click_1(object sender, EventArgs e)
        {
            //Button b = sender as Button;
            //string authentication = objcore.authentication(b.Text);
            //if (authentication == "yes")
            //{

            //}
            //else
            //{
            //    MessageBox.Show("You are not allowed to view this");
            //    return;
            //}
            POSRestaurant.forms.Downloadtransferin obj = new Downloadtransferin();
            

            obj.Show();
        }
        public void dayend(string uid, string status)
        {
            try
            {
                if (status == "Day Start")
                {

                    int id = 1;
                    DataSet dsdayend = new DataSet();
                    dsdayend = objcore.funGetDataSet("select max(id) as id from dayend");
                    if (dsdayend.Tables[0].Rows.Count > 0)
                    {
                        string i = dsdayend.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        id = Convert.ToInt32(i) + 1;

                    }
                    else
                    {

                    }
                   string date = DateTime.Now.ToString("yyyy-MM-dd");
                    string q = "Insert into dayend (Id,Date,DayStatus,UserId) values ('" + id + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','Open','" + uid + "')";
                    objcore.executeQuery(q);
                    MessageBox.Show("Day Started Successfully");
                    btnday.Text = "Day End";
                }
                if (status == "Day End")
                {
                    string q = "update dayend set DayStatus='Close' ";
                    objcore.executeQuery(q);
                    MessageBox.Show("Day Ended Successfully");
                    //Application.Exit();
                    btnday.Text = "Day Start";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void vButton9_Click_1(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            UpdateInventory obj = new UpdateInventory();

            obj.Show();
            
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            //System.Media.SoundPlayer player = new System.Media.SoundPlayer(@"C:\media\1.wav");
            //player.Play();
            
        }

        private void vButton9_Click_2(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
               // MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.forms.AddMovies obj = new  AddMovies();


            obj.Show();
        }

        private void vButton10_Click_1(object sender, EventArgs e)
        {

        }
        //public void printer()
        //{
        //    PosPrinter _oposPrinter;
        //    DeviceInfo _device;
        //    string LDN;

        //    explorer = new PosExplorer();
        //    _device = explorer.GetDevice(DeviceType.PosPrinter, LDN);
        //    _oposPrinter = (PosPrinter)explorer.CreateInstance(_device);
        //    _oposPrinter.Open();
        //    _oposPrinter.Claim(10000);
        //    _oposPrinter.DeviceEnabled = true;
        //    // normal print
        //    _oposPrinter.PrintNormal(PrinterStation.Receipt, "");
        //    // pulse the cash drawer pin  pulseLength-> 1 = 100ms, 2 = 200ms, pin-> 0 = pin2, 1 = pin5
        //    _oposPrinter.PrintNormal(PrinterStation.Receipt, "(char)16 + (char)20 + (char)1 + (char)pin + (char)pulseLength");

        //    // cut the paper
        //    _oposPrinter.PrintNormal(PrinterStation.Receipt, "(char)29 + (char)86 + (char)66");

        //    // print stored bitmap
        //    _oposPrinter.PrintNormal(PrinterStation.Receipt, "(char)29 + (char)47 + (char)0");
        //}
        public string printername()
        {
            string name = "";

            DataSet ds = new DataSet();
            string q = "select * from printers where type='opos'";
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                name = ds.Tables[0].Rows[0]["name"].ToString();
            }
            return name;
        }
        public void setinventory()
        {
            DataSet ds1 = new System.Data.DataSet();
            string q = "select id,date from sale where date = '2015-09-03' order by date";
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToDateTime(ds1.Tables[0].Rows[i]["date"].ToString()).ToShortDateString() == "2015-08-13" || Convert.ToDateTime(ds1.Tables[0].Rows[i]["date"].ToString()).ToShortDateString() == "2015-08-14")
                {
                }
                else
                {
                    q = "select *  from saledetails where saleid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and (ModifierId = '6')";
                    DataSet ds2 = new System.Data.DataSet();
                    ds2 = objcore.funGetDataSet(q);
                    for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                    {
                        if (ds2.Tables[0].Rows[j]["ModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["ModifierId"].ToString() == "0")
                        {
                            int qty = 0;
                            string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                            if (temp == "")
                            {
                                temp = "1";
                            }
                            string flv = ds2.Tables[0].Rows[j]["Flavourid"].ToString();
                            if (flv == "0")
                            {
                                flv = "";
                            }
                            qty = Convert.ToInt32(temp);
                            recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, ds1.Tables[0].Rows[i]["date"].ToString());
                        }
                        else
                        {
                            int qty = 0;
                            string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                            if (temp == "")
                            {
                                temp = "1";
                            }
                            qty = Convert.ToInt32(temp);
                            recipiemodifier(ds2.Tables[0].Rows[j]["ModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString());

                        }
                    }
                }

            }
        }
        public void recipie(string itmid, int itmqnty, string flid, string date)
        {
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";
                if (flid == "")
                {
                    q = "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.modifierid FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='')";
                }
                else
                {
                    q = "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.modifierid FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
                }
                dsrecipie = objcore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        int qnty = Convert.ToInt32(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        amounttodeduct = Math.Round(amounttodeduct, 3);
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        //dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                        //if (dsminus.Tables[0].Rows.Count > 0)
                        //{
                        //    inventryqty = double.Parse(dsminus.Tables[0].Rows[0]["Quantity"].ToString());
                        //    q = "update Inventory set Quantity='" + (inventryqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                        //    objCore.executeQuery(q);
                        //}
                        //else
                        //{
                        //    ds = new DataSet();
                        //    int idcnsmd = 0;
                        //    ds = objCore.funGetDataSet("select MAX(ID) as id from Inventory");
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        string ii = ds.Tables[0].Rows[0][0].ToString();
                        //        if (ii == string.Empty)
                        //        {
                        //            ii = "0";
                        //        }
                        //        idcnsmd = Convert.ToInt32(ii) + 1;
                        //    }
                        //    else
                        //    {
                        //        idcnsmd = Convert.ToInt32("1");
                        //    }
                        //    //amounttodeduct = Math.Abs(amounttodeduct);
                        //    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                        //    q = "insert into Inventory ( Id, RawItemId, Quantity) values('" + idcnsmd + "','" + rawitmid + "','" + -amounttodeduct + "')";
                        //    objCore.executeQuery(q);
                        //}
                        dsminus = new DataSet();
                        dsminus = objcore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                            double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                            if (inventryqty > 0)
                            {
                                rem = inventryqty;
                            }
                            q = "update InventoryConsumed set RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                            objcore.executeQuery(q);
                        }
                        else
                        {
                            ds = new DataSet();
                            int idcnsmd = 0;
                            ds = objcore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string ii = ds.Tables[0].Rows[0][0].ToString();
                                if (ii == string.Empty)
                                {
                                    ii = "0";
                                }
                                idcnsmd = Convert.ToInt32(ii) + 1;
                            }
                            else
                            {
                                idcnsmd = Convert.ToInt32("1");
                            }
                            // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                            q = "insert into InventoryConsumed (Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                            objcore.executeQuery(q);
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        
        DataSet ds = new System.Data.DataSet();
        public void recipiemodifier(string itmid, int itmqnty, string date)
        {
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";// "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "'";
                q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Modifier.RawItemId, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Modifier.Id='" + itmid + "'";
                dsrecipie = objcore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        int qnty = Convert.ToInt32(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        amounttodeduct = Math.Round(amounttodeduct, 3);
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        //dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                        //if (dsminus.Tables[0].Rows.Count > 0)
                        //{
                        //    inventryqty = double.Parse(dsminus.Tables[0].Rows[i]["Quantity"].ToString());
                        //    q = "update Inventory set Quantity='" + (inventryqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                        //    objCore.executeQuery(q);
                        //}
                        //else
                        //{
                        //    ds = new DataSet();
                        //    int idcnsmd = 0;
                        //    ds = objCore.funGetDataSet("select MAX(ID) as id from Inventory");
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        string ii = ds.Tables[0].Rows[0][0].ToString();
                        //        if (ii == string.Empty)
                        //        {
                        //            ii = "0";
                        //        }
                        //        idcnsmd = Convert.ToInt32(ii) + 1;
                        //    }
                        //    else
                        //    {
                        //        idcnsmd = Convert.ToInt32("1");
                        //    }
                        //    //amounttodeduct = Math.Abs(amounttodeduct);
                        //    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                        //    q = "insert into Inventory ( Id, RawItemId, Quantity) values('" + idcnsmd + "','" + rawitmid + "','" + -amounttodeduct + "')";
                        //    objCore.executeQuery(q);
                        //}
                        dsminus = new DataSet();
                        dsminus = objcore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                            q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                            objcore.executeQuery(q);
                        }
                        else
                        {

                            ds = new DataSet();
                            int idcnsmd = 0;
                            ds = objcore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string ii = ds.Tables[0].Rows[0][0].ToString();
                                if (ii == string.Empty)
                                {
                                    ii = "0";
                                }
                                idcnsmd = Convert.ToInt32(ii) + 1;
                            }
                            else
                            {
                                idcnsmd = Convert.ToInt32("1");
                            }
                            //idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                            q = "insert into InventoryConsumed (Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                            objcore.executeQuery(q);
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        protected void backupdb()
        {
            string path = "D";
            try
            {
                try
                {
                    string qq = "select * from Mailsetting";
                    DataSet dsmail = new DataSet();
                    dsmail = objcore.funGetDataSet(qq);

                    if (dsmail.Tables[0].Rows.Count > 0)
                    {
                        path = dsmail.Tables[0].Rows[0]["path"].ToString();
                    }
                }
                catch (Exception ex)
                {


                }
                string file = @"" + path + ":\\db.bak";
                System.IO.File.Delete(file);

            }
            catch (Exception ex)
            {


            }
            try
            {
                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(objcore.getConnectionString());

                string server = builder.DataSource;
                string database = builder.InitialCatalog;
                string q = "backup database "+database+" to disk ='" + path + ":\\db.bak'";
                objcore.executeQuery(q);
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton9_Click_3(object sender, EventArgs e)
        {
           // backupdb();
            
            //setinventory();
            //cashdrawertest oj = new cashdrawertest();
            //oj.Show();
            //PrintReceipt();
           // OpenCashDrawer();
            //OPOSPOSPrinter obj = new OPOSPOSPrinter();
            //obj.Open(printername());
            ////'Get the exclusive control right for the opened device.
            ////'Then the device is disable from other application.
            ////'(Notice:When using an old CO, use the Claim.)
            //obj.ClaimDevice(1000);
            ////'Enable the device.
            //obj.DeviceEnabled = true;
            //if (obj.DeviceEnabled == true)
            //{ 
            
            //}
            //else
            //{
            //    MessageBox.Show(obj.DeviceEnabled.ToString());
            //}
            //obj.CharacterSet = 864;
            //string text = "first print\n";
            ////text = ArabicPrint.Encoding.encode(text);
            ////text = ArabicPrint.ReverseString.reverseStringLeftToRight(text);
            ////obj.PrintNormal(1, text);
            //obj.PrintNormal(2, text);
            //obj.CutPaper(50);
            //obj.PrintNormal(1, text);
            //obj.CutPaper(50);
            ////text = "first print";
            ////text = ArabicPrint.Encoding.encode(text);
            ////text = ArabicPrint.ReverseString.reverseStringLeftToRight(text);
            ////obj.PrintNormal(1, text);
            ////obj.PrintNormal(2, text);
            //obj.ReleaseDevice();
            //obj.Close();
            //OPOSCashDrawer ob=new OPOSCashDrawer();
            //OPOSCashDrawer ob = new OPOSCashDrawer();
            //ob.Open("crr");
            //ob.ClaimDevice(1000);
            //ob.DeviceEnabled = true;
            //ob.OpenDrawer();

           
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.forms.BillRecall obj1 = new BillRecall();
            obj1.Show();
        }

        private void vButton10_Click_2(object sender, EventArgs e)
        {
            //Button b = sender as Button;
            //string authentication = objcore.authentication(b.Text);
            //if (authentication == "yes")
            //{
            //}
            //else
            //{
            //    MessageBox.Show("You are not allowed to view this");
            //    return;
            //}
            POSRestaurant.Setting.attachmenu obj = new Setting.attachmenu();
            obj.Show();
        }

        private void vButton12_Click(object sender, EventArgs e)
        {
            //Button b = sender as Button;
            //string authentication = objcore.authentication(b.Text);
            //if (authentication == "yes")
            //{
            //}
            //else
            //{
            //    MessageBox.Show("You are not allowed to view this");
            //    return;
            //}
            POSRestaurant.forms.DownloadMenu obj = new DownloadMenu();
            //POSRestaurant.forms.Downloadtransferin obj = new Downloadtransferin();
            obj.Show();
        }

        private void vButton13_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {
            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.RawItems.Demand obj = new RawItems.Demand();
            obj.Show();
        }
        public void sendmail()
        {
            //try
            //{
            //    ExportOptions CrExportOptions;
            //    DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //    PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
            //    CrDiskFileDestinationOptions.DiskFileName = "c:\\csharp.net-informations.pdf";
            //    CrExportOptions = cryRpt.ExportOptions;
            //    {
            //        CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //        CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //        CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
            //        CrExportOptions.FormatOptions = CrFormatTypeOptions;
            //    }
            //    cryRpt.Export();
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            try
            {
                string clientName, email, password;
                clientName = "www.ftech.com.pk";
                email = "younas@ftech.com.pk";
                password = "Admin@123";
                SmtpClient client = new SmtpClient(clientName, 25);
                client.UseDefaultCredentials = false;
                client.EnableSsl = false;
                client.Credentials = new System.Net.NetworkCredential(email, password);
                MailMessage myMail = new MailMessage();
                MailAddress addTo = new MailAddress("younas0313@gmail.com");
                myMail.IsBodyHtml = true;
                myMail.Subject = "This is a test email";
                myMail.Body = "This is a test email body";
                Attachment at = new Attachment(("C://new.csv"));
                myMail.Attachments.Add(at);
                myMail.To.Add(addTo);
                myMail.Priority = MailPriority.High;
                myMail.From = new MailAddress(email);
                client.Send(myMail);
                
            }
            catch
            {
               
            }
        }           
        private void vButton14_Click(object sender, EventArgs e)
        {
            //sendmail();
            //POSRestaurant.Sale.sendemail ob = new Sale.sendemail();
            //ob.date = "2016-10-13";
            //ob.Show();
            //return;
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {
            }
            else
            {
                //MessageBox.Show("You are not allowed to view this");
                //return;
            }
            POSRestaurant.RawItems.CriticalInventory obj = new RawItems.CriticalInventory();
            obj.Show();
        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.RawItems.StoreTransfer obj = new RawItems.StoreTransfer();
            obj.Show();
        }
        protected string getdate()
        {
            string dat = "";
            string q = "select date  from prainfo ";
            DataSet dster = new System.Data.DataSet();
            dster = objcore.funGetDataSet(q);
            if (dster.Tables[0].Rows.Count > 0)
            {
                dat = dster.Tables[0].Rows[0][0].ToString();
            }
            return dat;
        }
        private void vButton16_Click(object sender, EventArgs e)
        {

            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {
            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.RawItems.StoreDemand obj = new RawItems.StoreDemand();
            obj.Show();
        }

        private void vButton17_Click(object sender, EventArgs e)
        {
            POSRestaurant.RawItems.Transfer obj = new  RawItems.Transfer();
            obj.Show();
        }

        private void vButton18_Click(object sender, EventArgs e)
        {
            POSRestaurant.RawItems.Completewaste obj = new RawItems.Completewaste();
            obj.Show();
        }

        private void vButton19_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {
            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.RawItems.invTransfer obj = new RawItems.invTransfer();
            obj.Show();
        }

        private void vButton26_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Accounts.ChartofAccounts obj = new Accounts.ChartofAccounts();
            obj.Show();
        }

        private void vButton27_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Accounts.frmaccounts obj = new POSRestaurant.Reports.Accounts.frmaccounts();
            obj.Show();
        }

        private void vButton28_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Accounts.Vouchers obj = new Accounts.Vouchers();
            obj.Show();
        }

        private void vButton25_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Statements.frmGLAccounts obj = new POSRestaurant.Reports.Statements.frmGLAccounts();
            obj.Show();
        }

        private void vButton24_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            //POSRestaurant.Reports.Statements.frmReceiveableStatemetBank obj = new POSRestaurant.Reports.Statements.frmReceiveableStatemetBank();
            POSRestaurant.Reports.Statements.frmReceiveableStatemet obj = new POSRestaurant.Reports.Statements.frmReceiveableStatemet();
            obj.Show();
        }

        private void vButton23_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            //POSRestaurant.Reports.Statements.frmPayableStatemetBank obj = new POSRestaurant.Reports.Statements.frmPayableStatemetBank();
            POSRestaurant.Reports.Statements.frmPayableStatemet obj = new POSRestaurant.Reports.Statements.frmPayableStatemet();
            obj.Show();
        }

        private void vButton22_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Accounts.frmTrialBalane2 obj = new POSRestaurant.Reports.Accounts.frmTrialBalane2();
            obj.Show();
        }

        private void vButton21_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Accounts.frmProfitLoss obj = new POSRestaurant.Reports.Accounts.frmProfitLoss();
            obj.Show();
        }

        private void vButton20_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Accounts.frmBalaneSheet obj = new POSRestaurant.Reports.Accounts.frmBalaneSheet();
            obj.Show();
        }

        private void vButton30_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Accounts.frmComparativeProfitLoss obj = new POSRestaurant.Reports.Accounts.frmComparativeProfitLoss();
            obj.Show();
        }

        private void vButton41_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Statements.frmReceiveableStatemetsum obj = new POSRestaurant.Reports.Statements.frmReceiveableStatemetsum();
            obj.Show();
        }

        private void vButton42_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Reports.Statements.frmpayableableStatemetsum obj = new POSRestaurant.Reports.Statements.frmpayableableStatemetsum();
            obj.Show();
        }

        private void vButton32_Click(object sender, EventArgs e)
        {
            POSRestaurant.Accounts.AddChartAccountsCodes obj = new Accounts.AddChartAccountsCodes();
            obj.Show();
        }

        private void vButton33_Click(object sender, EventArgs e)
        {
            POSRestaurant.Accounts.AddSalesAccount obj = new Accounts.AddSalesAccount();
            obj.Show();
        }

        private void vButton36_Click(object sender, EventArgs e)
        {
            POSRestaurant.Setting.Reopenshift obj = new  Setting.Reopenshift();
            obj.Show();
        }

        private void vButton37_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.pra obj = new POSRestaurant.Reports.pra();
            obj.Show();
        }

        private void vButton38_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.forms.Targets2 obj = new Targets2();
            obj.Show();
        }

        private void vButton40_Click(object sender, EventArgs e)
        {
            vButton40.Text = "Pleas Wait";
            objcore.addcolumns();
            MessageBox.Show("Updated");
            vButton40.Text = "Update Database Columns";
        }

        private void vButton43_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            POSRestaurant.Setting.PointsCodes obj = new Setting.PointsCodes();
            obj.Show();
        }

        private void vButton44_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Accounts.frmDayBook obj = new POSRestaurant.Reports.Accounts.frmDayBook();
            obj.Show();
        }

        private void vButton45_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            Production obj = new  Production();
            obj.Show();
        }

        private void vButton46_Click(object sender, EventArgs e)
        {
            POSRestaurant.Accounts.DownloadAccounts obj = new Accounts.DownloadAccounts();
           
            obj.Show();
        }

        private void vButton47_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string authentication = objcore.authentication(b.Text);
            if (authentication == "yes")
            {

            }
            else
            {
                MessageBox.Show("You are not allowed to view this");
                return;
            }
            //POSRestaurant.Reports.Statements.frmReceiveableStatemetBank obj = new POSRestaurant.Reports.Statements.frmReceiveableStatemetBank();
            POSRestaurant.Reports.Statements.frmEmployeePayableStatemet obj = new POSRestaurant.Reports.Statements.frmEmployeePayableStatemet();
            obj.Show();
        }

        private void vButton48_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Accounts.frmCashBook obj = new POSRestaurant.Reports.Accounts.frmCashBook();
            obj.Show();
        }

        private void vButton48_Click_1(object sender, EventArgs e)
        {
            POSRestaurant.Properties.Settings.Default.view = "new";
            POSRestaurant.Properties.Settings.Default.Save();
            
            this.Close();
        }
    }
}
