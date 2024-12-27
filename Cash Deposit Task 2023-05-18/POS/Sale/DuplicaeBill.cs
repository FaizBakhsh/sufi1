using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using OposPOSPrinter_CCO;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Reporting.WinForms;
namespace POSRestaurant.Sale
{
    public partial class DuplicaeBill : Form
    {
        private  RestSale _frm1;
        public static string user = "";
        public string date = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public DuplicaeBill(RestSale frm1)
           {
                InitializeComponent();
                _frm1 = frm1;
                //txtsearch.ForeColor = Color.LightGray;
                //txtsearch.Text = "Seach Sale by Invoice No";
                //this.txtsearch.Leave += new System.EventHandler(this.textBox1_Leave);
                //this.txtsearch.Enter += new System.EventHandler(this.textBox1_Enter);
           }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            //if (txtsearch.Text.Length == 0)
            //{
            //    txtsearch.Text = "Seach Sale by Invoice No";
            //    txtsearch.ForeColor = Color.LightGray;
            //}
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            //if (txtsearch.Text == "Seach Sale by Invoice No")
            //{
            //    txtsearch.Text = "";
            //    txtsearch.ForeColor = Color.Black;
            //}
        }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";

        //}

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         public void changtext(Button btn , string text)
        {
            btn.Text = text;
            btn.Text = text.Replace("&", "&&");
        }
         public void getdata()
         {
             try
             {
                 if (dateTimePicker1.Visible == true)
                 {
                     date = dateTimePicker1.Text;
                 }
                 //category
                 DataSet dsbill = new DataSet();
                 string q9 = "";
                 if (_frm1.multibranches == "Enabled")
                 {

                     if (txtsearch.Text == string.Empty)
                     {

                         q9 = "SELECT     dbo.Sale.Id AS Bill_No, dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.BillType, dbo.Sale.TotalBill, dbo.Sale.DiscountAmount, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.GSTPerc,                       dbo.Sale.Discount, dbo.Users.Name AS cashier, dbo.Sale.OrderType, dbo.Sale.servicecharges, dbo.Sale.Invoice  FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id where dbo.Sale.date='" + date + "' and dbo.Sale.BillStatus='Paid' and  dbo.Sale.branchid='" + _frm1.branchid + "' order by  dbo.Sale.id desc";

                     }
                     else
                     {

                         q9 = "SELECT     dbo.Sale.Id AS Bill_No , dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.BillType, dbo.Sale.TotalBill, dbo.Sale.DiscountAmount, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.GSTPerc,                       dbo.Sale.Discount, dbo.Users.Name AS cashier, dbo.Sale.OrderType, dbo.Sale.servicecharges, dbo.Sale.Invoice  FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id where cast( dbo.Sale.id as varchar(max))='" + txtsearch.Text.Trim() + "' and dbo.Sale.date='" + date + "'  and dbo.Sale.BillStatus='Paid' and  dbo.Sale.branchid='" + _frm1.branchid + "' order by  dbo.Sale.id desc";

                     }
                 }
                 else
                 {

                     if (txtsearch.Text == string.Empty)
                     {

                         q9 = "SELECT     dbo.Sale.Id AS Bill_No, dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.BillType, dbo.Sale.TotalBill, dbo.Sale.DiscountAmount, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.GSTPerc,                       dbo.Sale.Discount, dbo.Users.Name AS cashier, dbo.Sale.OrderType, dbo.Sale.servicecharges, dbo.Sale.Invoice  FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id where dbo.Sale.date='" + date + "' and dbo.Sale.BillStatus='Paid' order by  dbo.Sale.id desc";
                         q9 = "SELECT     dbo.Sale.Id AS Bill_No , dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.BillType, dbo.Sale.TotalBill, dbo.Sale.DiscountAmount, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.GSTPerc,                       dbo.Sale.Discount, dbo.Users.Name AS cashier, dbo.Sale.OrderType, dbo.Sale.servicecharges, dbo.Sale.Invoice, dbo.TakeAway.CustomerId AS TakeAwayID, dbo.DinInTables.TableNo, dbo.Delivery.Name, dbo.Delivery.Phone, dbo.Delivery.Address FROM            dbo.Sale INNER JOIN                         dbo.Users ON dbo.Sale.UserId = dbo.Users.Id LEFT OUTER JOIN                         dbo.TakeAway ON dbo.Sale.Id = dbo.TakeAway.Saleid LEFT OUTER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId  where dbo.Sale.date='" + date + "' and dbo.Sale.BillStatus='Paid' order by  dbo.Sale.id desc";
                     }
                     else
                     {

                         q9 = "SELECT     dbo.Sale.Id AS Bill_No , dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.BillType, dbo.Sale.TotalBill, dbo.Sale.DiscountAmount, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.GSTPerc,                       dbo.Sale.Discount, dbo.Users.Name AS cashier, dbo.Sale.OrderType, dbo.Sale.servicecharges, dbo.Sale.Invoice  FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id where (dbo.Sale.id='" + txtsearch.Text.Trim() + "' and dbo.Sale.date='" + date + "'  and dbo.Sale.BillStatus='Paid') or  (dbo.Sale.id='" + txtsearch.Text.Trim() + "' and dbo.Sale.date='" + date + "'  and dbo.Sale.BillStatus='Paid')   order by  dbo.Sale.id desc";
                         q9 = "SELECT     dbo.Sale.Id AS Bill_No , dbo.Sale.Date, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.BillType, dbo.Sale.TotalBill, dbo.Sale.DiscountAmount, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.GSTPerc,                       dbo.Sale.Discount, dbo.Users.Name AS cashier, dbo.Sale.OrderType, dbo.Sale.servicecharges, dbo.Sale.Invoice, dbo.TakeAway.CustomerId AS TakeAwayID, dbo.DinInTables.TableNo, dbo.Delivery.Name, dbo.Delivery.Phone, dbo.Delivery.Address FROM            dbo.Sale INNER JOIN                         dbo.Users ON dbo.Sale.UserId = dbo.Users.Id LEFT OUTER JOIN                         dbo.TakeAway ON dbo.Sale.Id = dbo.TakeAway.Saleid LEFT OUTER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId  where  (cast( dbo.Sale.id as varchar(max))='" + txtsearch.Text.Trim() + "' and dbo.Sale.date='" + date + "'  and dbo.Sale.BillStatus='Paid') or  (dbo.Delivery.Phone='" + txtsearch.Text.Trim() + "' and dbo.Sale.date='" + date + "'  and dbo.Sale.BillStatus='Paid')   or  (dbo.Delivery.Name like '%" + txtsearch.Text.Trim() + "%' and dbo.Sale.date='" + date + "'  and dbo.Sale.BillStatus='Paid')   order by  dbo.Sale.id desc";
                  
                     }
                 }
                 dsbill = objCore.funGetDataSet(q9);
                 dataGridView1.DataSource = dsbill.Tables[0];
                 dataGridView1.Columns[7].Visible = false;
                 dataGridView1.Columns[5].Visible = false;
                 dataGridView1.Columns[6].Visible = false;
                 dataGridView1.Columns[9].Visible = false;
                 //dataGridView1.Columns[10].Visible = false;
                 dataGridView1.Columns[11].Visible = false;
                 foreach (DataGridViewRow dr in dataGridView1.Rows)
                 {
                     dr.Height = 40;
                 }

             }
             catch (Exception ex)
             {


             }
         }
        private void AddGroups_Load(object sender, EventArgs e)
         {
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
                     //this.WindowState = FormWindowState.Maximized;

                 }

             }
             catch (Exception ex)
             {

             }
             dateTimePicker1.Visible = false;
           // this.TopMost = true;
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
               
                getdata();
                ds = new DataSet();
                string q = "SELECT   * from users where id='" + id + "' order by id desc ";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user = ds.Tables[0].Rows[0]["name"].ToString();
                }
                string authentication = objCore.authentication1("Old Duplicate Bills", ds.Tables[0].Rows[0]["id"].ToString());
                if (authentication == "yes")
                {
                    dateTimePicker1.Visible = true;
                    lbldate.Visible = true;
                }
                else
                {
                    dateTimePicker1.Visible = false;
                    lbldate.Visible = false;
                }
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        int gridindex = 0;
        DataTable dtr = new DataTable();
        public void filg2(string id)
        {
            
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsdetail = new DataSet();
            string q = "";// "SELECT      dbo.Saledetails.Id, dbo.Saledetails.saleid,dbo.Saledetails.ModifierId, dbo.MenuItem.Name as Item, dbo.Modifier.Name AS Modifier, dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM         dbo.Saledetails LEFT OUTER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                      dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc ";

           // q = "SELECT     dbo.Saledetails.Id, dbo.Saledetails.saleid,dbo.Saledetails.ModifierId, dbo.MenuItem.Name as Item , dbo.RawItem.ItemName AS Modifier , dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId RIGHT OUTER JOIN                      dbo.Saledetails LEFT OUTER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id ON dbo.Modifier.Id = dbo.Saledetails.ModifierId where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc ";
            q = "SELECT      dbo.Saledetails.Id,dbo.Saledetails.saleid, dbo.Saledetails.ModifierId, dbo.MenuItem.Name AS Item,dbo.Saledetails.comments AS Comments, dbo.RawItem.ItemName AS Modifier, dbo.Saledetails.Quantity, dbo.Saledetails.Price,                       dbo.Sale.OrderType,dbo.Sale.DiscountAmount, dbo.Saledetails.RunTimeModifierId FROM         dbo.MenuItem RIGHT OUTER JOIN                      dbo.Saledetails INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id ON dbo.MenuItem.Id = dbo.Saledetails.MenuItemId LEFT OUTER JOIN                      dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId ON dbo.Saledetails.ModifierId = dbo.Modifier.Id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc";
            q = "SELECT        dbo.Saledetails.Id, dbo.Saledetails.saleid, dbo.Saledetails.ModifierId, dbo.MenuItem.Name AS Item, dbo.Saledetails.comments AS Comments, dbo.Saledetails.Quantity, dbo.Saledetails.Price, dbo.Sale.OrderType,                          dbo.Sale.DiscountAmount, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS Rmodifier FROM            dbo.RuntimeModifier RIGHT OUTER JOIN                         dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id ON dbo.RuntimeModifier.id = dbo.Saledetails.RunTimeModifierId LEFT OUTER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id  where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc";
            q = "SELECT        dbo.Saledetails.Id, dbo.Saledetails.saleid, dbo.Saledetails.ModifierId, dbo.ModifierFlavour.name AS Size, dbo.MenuItem.Name AS Item, dbo.Saledetails.comments AS Comments, dbo.Saledetails.Quantity, dbo.Saledetails.Price, dbo.Saledetails.GrossPrice as Pricee,                          dbo.Sale.OrderType, dbo.Sale.DiscountAmount, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS Rmodifier,saledetails.runtimemodifierid,saledetails.flavourid,saledetails.menuitemid FROM            dbo.ModifierFlavour RIGHT OUTER JOIN                         dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id ON dbo.ModifierFlavour.Id = dbo.Saledetails.Flavourid LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id   where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc";
            dsdetail = objCore.funGetDataSet(q);
            try
            {
                dtr = dsdetail.Tables[0];
            }
            catch (Exception ex)
            {
                
                
            }
            if (dsdetail.Tables[0].Rows.Count > 0)
            {
                gridindex = dataGridView1.CurrentCell.RowIndex;
                dataGridView2.DataSource = dsdetail.Tables[0];
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                dataGridView2.Columns[2].Visible = false;
                if (_frm1.ShowGrossPrice == "Enabled")
                {
                    dataGridView2.Columns[7].Visible = false;
                }
                else
                {
                    dataGridView2.Columns[8].Visible = false;
                }
                dataGridView2.Columns[10].Visible = false;
                dataGridView2.Columns[14].Visible = false;
                dataGridView2.Columns[15].Visible = false;
                dataGridView2.Columns[16].Visible = false;

                foreach (DataGridViewRow dr in dataGridView2.Rows)
                {

                    try
                    {
                        dr.Height = 40;
                       // dr.Cells["Item"].Value=dr.Cells["Item"].Value.ToString().Replace("Open Item", "");
                        //string mid = dr.Cells["ModifierId"].Value.ToString();
                        //if (mid == null)
                        //{
                        //    mid = string.Empty;
                        //}
                        //if (mid == "0")
                        //{
                        //    mid = string.Empty;
                        //}
                        //if (mid != string.Empty)
                        //{
                        //    dr.Cells["Name"].Value = string.Empty;

                        //}
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                filg2(Id);
            }
            catch (Exception ex)
            {
                
               
            }
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
        public string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                    return printer;
            }
            return string.Empty;
        }
        public void bindreport(string mop, string sid ,string totalbil,string discount,string NetBill,string GST, string date,string time)
        {
            if (dataGridView2.Rows.Count > 0)
            {
                DataSet dsprint = new DataSet();
                dsprint = objCore.funGetDataSet("select * from Printers where type='Receipt'");

                if (dsprint.Tables[0].Rows.Count > 0)
                {
                    //ReportDocument rptDoc = new ReportDocument();
                    POSRestaurant.Reports.DuplicateCashReceipt rptDoc = new Reports.DuplicateCashReceipt();
                    POSRestaurant.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
                    //feereport ds = new feereport(); // .xsd file name
                    DataTable dt = new DataTable();

                    // Just set the name of data table
                    dt.TableName = "Crystal Report";
                    dt = getAllOrders(mop, sid, totalbil, discount, NetBill, GST, date, time);
                    dsrpt.Tables[0].Merge(dt,true,MissingSchemaAction.Ignore);


                    rptDoc.SetDataSource(dsrpt);
                    string printer = dsprint.Tables[0].Rows[0]["Name"].ToString();// GetDefaultPrinter();
                    rptDoc.PrintOptions.PrinterName = printer;
                   
                    rptDoc.PrintToPrinter(1, false, 0, 0);
                }
            }

        }
        public DataTable getAllOrders(string mp, string siid, string totalbil, string discount, string NetBill, string GST, string date, string time)
        {

            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("QTY", typeof(string));
            dtrpt.Columns.Add("Item", typeof(string));
            dtrpt.Columns.Add("Price", typeof(string));
            dtrpt.Columns.Add("Total", typeof(double));
            dtrpt.Columns.Add("Discount", typeof(double));
            dtrpt.Columns.Add("GST", typeof(double));
            dtrpt.Columns.Add("NetTotal", typeof(double));
            dtrpt.Columns.Add("Cashier", typeof(string));
            dtrpt.Columns.Add("CName", typeof(string));
            dtrpt.Columns.Add("CAddress", typeof(string));
            dtrpt.Columns.Add("CPhone", typeof(string));
            dtrpt.Columns.Add("MOP", typeof(string));
            dtrpt.Columns.Add("Invoice", typeof(string));
            dtrpt.Columns.Add("Date", typeof(string));
            dtrpt.Columns.Add("Time", typeof(string));
            dtrpt.Columns.Add("OrderType", typeof(string));
            dtrpt.Columns.Add("DiscountAmount", typeof(double));
            dtrpt.Columns.Add("logo", typeof(byte[]));
            dtrpt.Columns.Add("Amount", typeof(string));
            dtrpt.Columns.Add("Id", typeof(string));
            dtrpt.Columns.Add("runtimeflavourid", typeof(string));
            dtrpt.Columns.Add("MdId", typeof(string));
            dtrpt.Columns.Add("flavourid", typeof(string));
            string cname = "", caddress = "", cphone = "",logo="";
            DataSet dsinfo = new DataSet();
            dsinfo = objCore.funGetDataSet("select * from CompanyInfo");
            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                cname = dsinfo.Tables[0].Rows[0]["Name"].ToString();
                caddress = dsinfo.Tables[0].Rows[0]["Address"].ToString();
                cphone = dsinfo.Tables[0].Rows[0]["Phone"].ToString();
                logo = dsinfo.Tables[0].Rows[0]["logo"].ToString();
            }
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                try
                {
                    string modifierid = "", runtimemodifierid = "", flavourid = "", menuitemid = "";
                    if (dr.Cells["Id"].Value.ToString() != string.Empty)
                    {
                        modifierid = dr.Cells["modifierid"].Value.ToString();
                        runtimemodifierid = dr.Cells["runtimemodifierid"].Value.ToString();
                        flavourid = dr.Cells["flavourid"].Value.ToString();
                        menuitemid = dr.Cells["menuitemid"].Value.ToString();
                        string rmid = dr.Cells["Rmodifier"].Value.ToString();
                        if (rmid == null)
                        {
                            rmid = string.Empty;
                        }
                        string mid = dr.Cells["ModifierId"].Value.ToString();
                        if (mid == null)
                        {
                            mid = string.Empty;
                        }
                        if (mid == "0")
                        {
                            mid = string.Empty;
                        }
                        string amount = dr.Cells["DiscountAmount"].Value.ToString();
                        string price = "";
                        
                        if (rmid == "")
                        {
                            if (mid != string.Empty)
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add(dr.Cells["Quantity"].Value.ToString(), dr.Cells["Modifier"].Value.ToString(), Convert.ToDouble(dr.Cells["Price"].Value.ToString()), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, dr.Cells["OrderType"].Value.ToString(), Convert.ToDouble(amount), null, dr.Cells["Pricee"].Value.ToString(),menuitemid,runtimemodifierid,modifierid,flavourid);
                                }
                                else
                                {
                                    dtrpt.Rows.Add(dr.Cells["Quantity"].Value.ToString(), dr.Cells["Modifier"].Value.ToString(), Convert.ToDouble(dr.Cells["Price"].Value.ToString()), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, dr.Cells["OrderType"].Value.ToString(), Convert.ToDouble(amount), dsinfo.Tables[0].Rows[0]["logo"], dr.Cells["Pricee"].Value.ToString(), menuitemid, runtimemodifierid, modifierid, flavourid);

                                }
                            }
                            else
                            {
                                if (logo == "")
                                {

                                    dtrpt.Rows.Add((dr.Cells["Quantity"].Value.ToString()), dr.Cells["Size"].Value.ToString() + " " + dr.Cells["Item"].Value.ToString().Replace("Open Item", "") + " " + dr.Cells["Comments"].Value.ToString(), Convert.ToDouble(dr.Cells["Price"].Value.ToString()), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, dr.Cells["OrderType"].Value.ToString(), Convert.ToDouble(amount), null, dr.Cells["Pricee"].Value.ToString(), menuitemid, runtimemodifierid, modifierid, flavourid);
                                }
                                else
                                {
                                    dtrpt.Rows.Add((dr.Cells["Quantity"].Value.ToString()), dr.Cells["Size"].Value.ToString() + " " + dr.Cells["Item"].Value.ToString().Replace("Open Item", "") + " " + dr.Cells["Comments"].Value.ToString(), Convert.ToDouble(dr.Cells["Price"].Value.ToString()), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, dr.Cells["OrderType"].Value.ToString(), Convert.ToDouble(amount), dsinfo.Tables[0].Rows[0]["logo"], dr.Cells["Pricee"].Value.ToString(), menuitemid, runtimemodifierid, modifierid, flavourid);
                                }
                            }
                        }
                        else
                        {
                            if (logo == "")
                            {

                                dtrpt.Rows.Add(dr.Cells["Quantity"].Value.ToString(), dr.Cells["Rmodifier"].Value.ToString(), Convert.ToDouble(dr.Cells["Price"].Value.ToString()), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, dr.Cells["OrderType"].Value.ToString(), Convert.ToDouble(amount), null, dr.Cells["Pricee"].Value.ToString(), menuitemid, runtimemodifierid, modifierid, flavourid);
                            }
                            else
                            {
                                dtrpt.Rows.Add(dr.Cells["Quantity"].Value.ToString(), dr.Cells["Rmodifier"].Value.ToString(), Convert.ToDouble(dr.Cells["Price"].Value.ToString()), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, dr.Cells["OrderType"].Value.ToString(), Convert.ToDouble(amount), dsinfo.Tables[0].Rows[0]["logo"], dr.Cells["Pricee"].Value.ToString(), menuitemid, runtimemodifierid, modifierid, flavourid);

                            }
                        }

                    }
                }
                catch (Exception ex)
                {

                    //MessageBox.Show(ex.Message);
                }
            }

            return dtrpt;
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void PrintReceipt(DataTable dtref, string customer, string date, string cashier, double received, double change, string bill, string saletype, string total, string gst, string dis, string gstperc, string disperc, string net,string invoiceno)
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
                string time =Convert.ToDateTime(dtref.Rows[0]["Time"].ToString()).ToShortTimeString();
                PrintReceiptHeader(printer, name, addrs, bill, phone, date, cashier, saletype, customer,time,invoiceno);
                foreach (DataRow dr in dtref.Rows)
                {
                    try
                    {
                       // if (dr["Id"].ToString() != string.Empty)
                        {
                            string pc = dr["Price"].ToString();
                            string qnty = "";
                            qnty = dr["Qty"].ToString();
                            string tmp = qnty;
                            if (tmp == "")
                            {
                                tmp = "1";
                            }
                            int qty = Convert.ToInt32(tmp);
                            tmp = pc;
                            if (tmp == "")
                            {
                                tmp = "0";
                            }
                            double sprice = Convert.ToDouble(tmp);
                            double singleprice = 0;
                            singleprice = sprice / qty;
                            PrintLineItem(printer, dr["ItemName"].ToString(), qty, Convert.ToDouble(singleprice));
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }

                PrintReceiptFooter(printer, Convert.ToDouble(total), Convert.ToDouble(gst), Convert.ToDouble(dis), wellcom, received, change, Convert.ToDouble(disperc), Convert.ToDouble(gstperc));
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
            string offSetString ="        ";

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
            PrintTextLine(printer, String.Empty);
            PrintTextLine(printer, String.Empty);
            //Print 'advance and cut' escape command.
            printer.CutPaper(50);
          //  printer.PrintNormal(2, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, 112, 48, 55, 121 }));
            // PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
        }
        private void PrintLineItem(OPOSPOSPrinter printer, string itemCode, double quantity, double unitPrice)
        {
            int length = 16;// getlinelength("space", "receipt");
            try
            {
                string temp = "";
                string textprint = "";
                int indx = 0;
                if (itemCode.Length > length)
                {
                    string offSetString = new string(' ', length - indx);
                    string val = itemCode.Substring(0, length);
                    /*  //// indx = val.IndexOf(" ", 0); no required
                      ////temp = itemCode.Substring(0, indx);// +offSetString;*/
                    //PrintText(printer, TruncateAt(val, length+1));skipped for bixolon
                    textprint = val;
                }
                else
                {
                    string offSetString = new string(' ', length - itemCode.Length);
                    string val = itemCode + offSetString;
                    // PrintText(printer, TruncateAt(val, length)); skipped for bixolon
                    textprint = val;
                }
                textprint = textprint + quantity.ToString().PadLeft(3) + unitPrice.ToString().PadLeft(9) + (quantity * unitPrice).ToString().PadLeft(9);
                PrintTextLine(printer, (textprint));
                //PrintText(printer, TruncateAt(quantity.ToString().PadLeft(3), 9));
                //PrintText(printer, TruncateAt(unitPrice.ToString().PadLeft(9), 9));
                //PrintTextLine(printer, TruncateAt((quantity * unitPrice).ToString().PadLeft(9), 9));

                if (itemCode.Length > length)
                {
                    //temp = itemCode.Substring(indx + 1);
                    temp = itemCode.Substring(length);
                    PrintText(printer, TruncateAt(temp, length + 1));
                    PrintText(printer, TruncateAt("".PadLeft(3), 9));
                    PrintText(printer, TruncateAt("".PadLeft(9), 9));
                    PrintTextLine(printer, TruncateAt(("").ToString().PadLeft(9), 9));

                    textprint = temp + "".ToString().PadLeft(3) + "".ToString().PadLeft(9) + ("").ToString().PadLeft(9);
                }
            }
            catch (Exception ex)
            {


            }
        }

        //private void PrintLineItem(OPOSPOSPrinter printer, string itemCode, double quantity, double unitPrice)
        //{
        //    string temp = "";
        //    int indx = 0;
        //    if (itemCode.Length > 17)
        //    {
        //        string offSetString = new string(' ', 17 - indx);
        //        string val = itemCode.Substring(0, 17);
        //        // indx = val.IndexOf(" ", 0);
        //        //temp = itemCode.Substring(0, indx);// +offSetString;
        //        PrintText(printer, TruncateAt(val, 18));
        //    }
        //    else
        //    {
        //        string offSetString = new string(' ', 17 - itemCode.Length);
        //        string val = itemCode + offSetString;
        //        PrintText(printer, TruncateAt(val, 17));
        //    }
        //    PrintText(printer, TruncateAt(quantity.ToString().PadLeft(3), 9));
        //    PrintText(printer, TruncateAt(unitPrice.ToString().PadLeft(9), 9));
        //    PrintTextLine(printer, TruncateAt((quantity * unitPrice).ToString().PadLeft(9), 9));
        //    if (itemCode.Length > 17)
        //    {
        //        //temp = itemCode.Substring(indx + 1);
        //        temp = itemCode.Substring(17);
        //        PrintText(printer, TruncateAt(temp, 18));
        //        PrintText(printer, TruncateAt("".PadLeft(3), 9));
        //        PrintText(printer, TruncateAt("".PadLeft(9), 9));
        //        PrintTextLine(printer, TruncateAt(("").ToString().PadLeft(9), 9));
        //    }
        //}


        public string getselectordertype()
        {
            string type = "";
            try
            {
                string q = "select selecttype from selecttype";
                DataSet dstype = new DataSet();
                dstype = objCore.funGetDataSet(q);
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    type = dstype.Tables[0].Rows[0]["selecttype"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return type;
        }
        public string getordertype(string saleid)
        {
            string type = "";
            try
            {
                string q = "select OrderType from sale where id='" + saleid + "'";
                DataSet dstype = new DataSet();
                dstype = objCore.funGetDataSet(q);
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    type = dstype.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return type;
        }
        public string gettbleno(string id)
        {
            string tbl = "";

            try
            {
                DataSet dstbl = new DataSet();
                string q = "select TableNo from DinInTables where Saleid='" + id + "'";
                dstbl = objCore.funGetDataSet(q);
                if (dstbl.Tables[0].Rows.Count > 0)
                {
                    tbl = dstbl.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return tbl;
        }
        public string getcustomerid(string id)
        {
            string customerid = "";

            try
            {
                DataSet dstbl = new DataSet();
                string q = "select CustomerId from TakeAway where Saleid='" + id + "'";
                dstbl = objCore.funGetDataSet(q);
                if (dstbl.Tables[0].Rows.Count > 0)
                {
                    customerid = dstbl.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return customerid;
        }
        public string getdeliveryinfo(string id)
        {
            string info = "";
            DataSet dsinfo = new DataSet();
            string q = "select * from Delivery where SaleId='" + id + "'";
            dsinfo = objCore.funGetDataSet(q);
            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                info = "\nName : " + dsinfo.Tables[0].Rows[0]["Name"].ToString() + "\nPhone No : " + dsinfo.Tables[0].Rows[0]["Phone"].ToString() + "\nadrs:" + dsinfo.Tables[0].Rows[0]["Address"].ToString() + "\nNote : " + dsinfo.Tables[0].Rows[0]["Note"].ToString();
                try
                {
                    if (dsinfo.Tables[0].Rows[0]["Phone2"].ToString().Length > 0)
                    {
                        info = info + "\nAlternate Phone No: " + dsinfo.Tables[0].Rows[0]["Phone2"].ToString();
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    if (dsinfo.Tables[0].Rows[0]["PromisedTime"].ToString().Length > 0)
                    {
                        info = info + "\nPromised Time: " + dsinfo.Tables[0].Rows[0]["PromisedTime"].ToString();
                    }
                }
                catch (Exception ex)
                {


                }
            }
            return info;
        }
        private void PrintReceiptHeader(OPOSPOSPrinter printer, string companyName, string addressLine1, string billno, string taxNumber, string dateTime, string cashierName, string mop, string customer,string time,string invoiceno)
        {
            if (invoiceno.Length > 0)
            {
                billno = invoiceno;
            }
            
            //MemoryStream stream = new MemoryStream(MyData);
            //Image logo = Image.FromStream(stream);
            //Bitmap bmp = (Bitmap)Bitmap.FromStream(stream);
            //printer.PrintBarCode(2, "123456", 1, 20, 20, 20, 20);
            //printer.PrintMemoryBitmap(2, "C:\\logo\\logo1.bmp", 1, 400, 200);
            //printer.PrintBitmap(2, "C:\\logo\\logo1.bmp", 400, 200);
            string offSetString = new string(' ', printer.RecLineChars / 3-2);
            string offSetString1 ="            ";
            string Bold = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'b', (byte)'C' });
            string Bold1 = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' });
            string ESC = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27 });
            PrintTextLine(printer, offSetString + (Bold + companyName));
            PrintTextLine(printer, offSetString + "  " + Bold + addressLine1);
            PrintTextLine(printer, offSetString + "    " + Bold + taxNumber);
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            if (getselectordertype().ToLower() == "yes")
            {
                PrintTextLine(printer, String.Format("Bill No : {0}", billno));
            }
            else
            {
                PrintTextLine(printer, String.Format("Bill No : {0}", billno + "     Customer :" + customer));
            }
            PrintTextLine(printer, String.Format("CASHIER : {0}", cashierName + "        MOP : " + mop));
            PrintTextLine(printer, String.Format("DATE : {0}", Bold1 + Convert.ToDateTime(dateTime).ToShortDateString() + "   " + time));
            if (getselectordertype().ToLower() == "yes")
            {
                string type = getordertype(billno);
                string cusid = "";
                if (type == "Take Away")
                {
                    cusid = getcustomerid(billno);

                    cusid = "Customer Id: " + cusid;
                }
                if (type == "Dine In")
                {
                    string tblno = gettbleno(billno);
                    cusid = "Table No: " + tblno;
                }
                if (type == "Delivery")
                {
                    string tblno = "";// getdelivery(billno);
                    tblno = getdeliveryinfo(billno);
                    cusid = tblno;
                }
                PrintTextLine(printer, String.Format("Order Type : {0}", type + "  " + cusid));

            }
            PrintTextLine(printer, new string('-', printer.RecLineChars));
            PrintTextLine(printer, offSetString1 + (Bold + "Duplicate Sale Slip"));
            printer.PrintNormal(2, " " + Environment.NewLine);
            string text = Bold + "Item Name        QTY " + "Unit Price  " + "Total";
            PrintTextLine(printer, text);
            //PrintText(printer, Bold + "Item Name         ");
            //PrintText(printer, Bold + "QTY ");
            //PrintText(printer, Bold + "Unit Price  ");
            //PrintTextLine(printer, Bold + "Total");
            PrintTextLine(printer, new string('=', printer.RecLineChars));
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
        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        public void filllog(string billno)
        {
            try
            {
                string q = "insert into log (Name, Time, Description,userid) values ('Duplicate Bill','" + DateTime.Now + "','Bill No:(" + billno + ")','" + id + "')";
                objCore.executeQuery(q);
            }
            catch (Exception ex)
            {

            }
        }
        public void getqrcode(string sid)
        {
            string inv = "1";
            try
            {
                string q = "select (invoice) as invoice,FBRcode from sale where id='" + sid + "'";
                DataSet dsinvoice = new DataSet();
                dsinvoice = objCore.funGetDataSet(q);
                if (dsinvoice.Tables[0].Rows.Count > 0)
                {
                    qrcode = dsinvoice.Tables[0].Rows[0][1].ToString();


                }
            }
            catch (Exception ex)
            {


            }

        }
        string  qrcode = "";
        private void vButton2_Click(object sender, EventArgs e)
        {
           
            try
            {
                qrcode = "";
                //int indx = dataGridView1.CurrentCell.RowIndex;

                if (gridindex >= 0)
                {
                    string invoiceno = "";
                    
                   // DialogResult dr = MessageBox.Show("Are you sure to print duplicate bill?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                   // if (dr == DialogResult.Yes)
                    {
                       
                        string svschrgs = dataGridView1.Rows[gridindex].Cells["servicecharges"].Value.ToString();
                        string ordertype = dataGridView1.Rows[gridindex].Cells["OrderType"].Value.ToString();
                        string date =Convert.ToDateTime( dataGridView1.Rows[gridindex].Cells[1].Value.ToString()).ToString("yyyy-MM-dd");
                        string time = dataGridView1.Rows[gridindex].Cells[2].Value.ToString();
                        string saleid = dataGridView1.Rows[gridindex].Cells[0].Value.ToString();
                        if (_frm1.printinvoiceno.Trim() != "" && _frm1.printinvoiceno.Trim()!="no")
                        {
                            invoiceno = _frm1.getinvoicenopaid(saleid);
                        }
                       getqrcode(saleid);
                        string billtype = dataGridView1.Rows[gridindex].Cells[4].Value.ToString();
                        string totalbil = dataGridView1.Rows[gridindex].Cells[5].Value.ToString();
                        string discount = dataGridView1.Rows[gridindex].Cells[10].Value.ToString();
                        string discountamount = dataGridView1.Rows[gridindex].Cells[6].Value.ToString();
                        string NetBill = dataGridView1.Rows[gridindex].Cells[3].Value.ToString();
                        string GST = dataGridView1.Rows[gridindex].Cells[7].Value.ToString();
                        string GSTperc = dataGridView1.Rows[gridindex].Cells[9].Value.ToString();
                        string customer = dataGridView1.Rows[gridindex].Cells[8].Value.ToString();
                        string cashier = dataGridView1.Rows[gridindex].Cells[11].Value.ToString();
                        filllog(saleid);
                        if (printtype() == "opos")
                        {
                            DataTable dt = new DataTable();

                            // Just set the name of data table
                            dt.TableName = "Crystal Report";
                            dt = getAllOrders(billtype, saleid, totalbil, discount, NetBill, GST, date, time);
                            PrintReceipt(dt, customer, date, cashier, Convert.ToDouble(NetBill), 0, saleid, billtype, totalbil, GST, discountamount, GSTperc, discount, NetBill,invoiceno);
                        }
                        else if (printtype().ToLower() == "generic")
                        {
                            // for (int i = 0; i < totalprints(); i++)
                            {
                                Print(printername("generic"), saleid.ToString(), cashier, customer, billtype, "", dtr, "", "", totalbil, discountamount, GST, "", "", date.ToString(), invoiceno);

                            }
                        }
                        else if (printtype().ToLower() == "rdlc")
                        {
                            int print = 1;
                            string printername = "";
                            DataSet dsprint = new DataSet();
                            string q = "select * from Printers where type='Receipt'";
                            
                            getcompany();
                            string customermsg = dscompany.Tables[0].Rows[0]["CustomerMessage"].ToString();
                            string customermsg2 = dscompany.Tables[0].Rows[0]["CustomerMessage2"].ToString();
                           
                            DataTable dt = new DataTable();

                            // Just set the name of data table
                            dt.TableName = "Crystal Report";
                             dt = getAllOrders(billtype, saleid, totalbil, discount, NetBill, GST, date, time);
                            string info = "";// getdeliveryinfo(saleid.ToString());
                            if (ordertype == "Dine In")
                            {
                                string tblno = gettbleno(saleid.ToString());
                                info = "Table No: " + tblno;
                            }
                            else if (ordertype == "Take Away")
                            {
                                string cusid = getcustomerid(saleid.ToString());
                                info = "Customer Id: " + cusid;
                            }
                            else
                            {

                                info = getdeliveryinfo(saleid.ToString());
                            }
                            string url = _frm1.pointsurl;
                            if (url == "")
                            {
                                try
                                {
                                    string value;
                                    value = CacheClass.Cache["pointsurl"] as string;
                                    if (null == value)
                                    {

                                    }
                                    else
                                    {
                                        url = value;
                                    }
                                }
                                catch (Exception ex)
                                {


                                }
                            }
                            string path = Path.GetDirectoryName(Application.ExecutablePath);
                            PrintClass.Printt(path, dt, billtype, saleid.ToString(), "", ordertype, totalbil, NetBill, discount, GSTperc, "0", "0", printername, info, print, discountamount.ToString(), GST.ToString(), customermsg, customermsg2, svschrgs, cashier, date, "", _frm1, qrcode, url, invoiceno, "Duplicate Bill","");
                            try
                            {
                                q = "insert into log (Name, Time, Description,userid) values ('Duplicate Bill Print','" + DateTime.Now + "','" + saleid.ToString() + "','" + POSRestaurant.Properties.Settings.Default.UserId + "')";
                                objCore.executeQuery(q);
                                
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            bindreport(billtype, saleid.ToString(), totalbil, discount, NetBill, GST, date, time);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a Sale First from Sales List");
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        public string printername(string type)
        {
            string name = "";
            string temp = "";
            if (type.ToLower() == "tablet")
            {
                temp = "tablet";
            }
            else if (type == "kitchen")
            {
                temp = "kitchen";
            }
            else if (type == "generic")
            {
                temp = "generic";
            }
            else if (type == "kds")
            {
                temp = "kds";
            }
            else
            {
                temp = "opos";
            }
            string q = "";
            DataSet ds = new DataSet();
            try
            {
                try
                {
                    q = "select * from printers where type='" + temp + "' and terminal='" + System.Environment.MachineName + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        name = ds.Tables[0].Rows[0]["name"].ToString();
                    }
                }
                catch (Exception ex)
                {


                }
                if (name == "")
                {
                    q = "select * from printers where type='" + temp + "'";
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        name = ds.Tables[0].Rows[0]["name"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                ds.Dispose();
            }
            return name;
        }
        public void Print(string printerName, string sid, string cashier, string cusid, string mop, string delivery, DataTable dtr, string r, string c, string total, string dis, string gst, string type, string ordertype, string dat,string invoiceno)
        {
            //date = dat;
            POSRestaurant.Sale.NativeMethods.DOC_INFO_1 documentInfo;
            IntPtr printerHandle;
            byte[] managedData = null;
            string addrs = "";
            if (ordertype == "Delivery")
            {
                addrs = "Address:\n";
                //addrs = addrs + getaddress(sid);
            }
            managedData = GetDocument(sid, cashier, cusid, mop, addrs, dtr, r, c, total, dis, gst, type, ordertype,invoiceno);
            documentInfo = new POSRestaurant.Sale.NativeMethods.DOC_INFO_1();
            documentInfo.pDataType = "RAW";
            documentInfo.pDocName = "Receipt";
            printerHandle = new IntPtr(0);
            if (POSRestaurant.Sale.NativeMethods.OpenPrinter(printerName.Normalize(), out printerHandle, IntPtr.Zero))
            {
                if (POSRestaurant.Sale.NativeMethods.StartDocPrinter(printerHandle, 1, documentInfo))
                {
                    int bytesWritten;

                    IntPtr unmanagedData;

                    //managedData = document;
                    unmanagedData = Marshal.AllocCoTaskMem(managedData.Length);
                    Marshal.Copy(managedData, 0, unmanagedData, managedData.Length);

                    if (POSRestaurant.Sale.NativeMethods.StartPagePrinter(printerHandle))
                    {
                        POSRestaurant.Sale.NativeMethods.WritePrinter(
                            printerHandle,
                            unmanagedData,
                            managedData.Length,
                            out bytesWritten);
                        POSRestaurant.Sale.NativeMethods.EndPagePrinter(printerHandle);
                    }
                    else
                    {
                        throw new Win32Exception();
                    }

                    Marshal.FreeCoTaskMem(unmanagedData);

                    POSRestaurant.Sale.NativeMethods.EndDocPrinter(printerHandle);
                }
                else
                {
                    throw new Win32Exception();
                }

                POSRestaurant.Sale.NativeMethods.ClosePrinter(printerHandle);
            }
            else
            {
                throw new Win32Exception();
            }

        }
        public byte[] GetDocument(string sid, string cashier, string cusid, string mop, string delivery, DataTable dtr, string r, string c, string total, string dis, string gst, string type, string ordertype,string invoiceno)
        {
            var val = new MemoryStream();
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                // Reset the printer bws (NV images are not cleared)
                bw.Write(AsciiControlChars.Escape);
                bw.Write('@');
                // Render the logo
                //RenderLogo(bw);
                PrintReceipt(bw, sid, cashier, cusid, mop, delivery, dtr, r, c, total, dis, gst, ordertype,invoiceno);
                // Feed 3 vertical motion units and cut the paper with a 1 point cut
                bw.Write(AsciiControlChars.GroupSeparator);
                bw.Write('V');
                bw.Write((byte)66);
                bw.Write((byte)3);

                bw.Flush();
                val = new MemoryStream();
                return ms.ToArray();
                //val = ms.ToArray();
            }

        }
        public string gettime(string id)
        {
            string time = "";
            DataSet dstime = new DataSet();
            try
            {
                string q = "select time from sale where id='" + id + "'";

                dstime = objCore.funGetDataSet(q);
                if (dstime.Tables[0].Rows.Count > 0)
                {
                    time = Convert.ToDateTime(dstime.Tables[0].Rows[0][0].ToString()).ToString("HH:mm tt");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                dstime.Dispose();
            }
            return time;
        }
        private void PrintReceipt(BinaryWriter bw, string sid, string cashier, string cusid, string mop, string delivery, DataTable dtr, string r, string c, string total, string dis, string gst, string ordertype,string invoiceno)
        {
            getcompany();
            string customermsg = dscompany.Tables[0].Rows[0]["CustomerMessage"].ToString();
            string customermsg2 = dscompany.Tables[0].Rows[0]["CustomerMessage2"].ToString();
            string info = "Your Bill No is";
            string time = "";
            string spec = sid;
            if (invoiceno.Length > 0)
            {
                spec = invoiceno;
            }
            time = " " + gettime(sid);
            int length = getlinelength("name", "generic");
            string namee = dscompany.Tables[0].Rows[0]["Name"].ToString();
            for (int i = 0; i < length; i++)
            {
                namee = " " + namee;


            }
            length = getlinelength("billno", "generic");

            for (int i = 0; i < length; i++)
            {

                info = " " + info;
                spec = " " + spec;
            }
            //namee = namee.PadLeft(length);
            length = getlinelength("address", "generic");
            string addrs = dscompany.Tables[0].Rows[0]["Address"].ToString();
            for (int i = 0; i < length; i++)
            {
                addrs = " " + addrs;
            }
            //addrs = addrs.PadLeft(length);
            string phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
            length = getlinelength("phone", "generic");
            for (int i = 0; i < length; i++)
            {
                phone = " " + phone;
            }
            // phone = phone.PadLeft(length);
            length = getlinelength("title", "generic");

            string title = "Duplicate Slip";
            for (int i = 0; i < length; i++)
            {
                title = " " + title;
            }
            bw.LargeText(namee);
            bw.NormalFont(addrs);
            bw.NormalFont(phone);
            //bw.FeedLines(1);
            length = getlinelength("line", "generic");
            string print = "", space = "";
            for (int i = 0; i < length; i++)
            {
                print = print + "-";
            }
            length = getlinelength("space", "generic");
            for (int i = 0; i < length; i++)
            {
                space = space + " ";
            }
            string itmtitle = "Item Name";
            for (int i = itmtitle.Length; i < space.Length + 1; i++)
            {
                itmtitle += " ";
            }
            bw.NormalFont(title);
            bw.NormalFont(print);
            //bw.FeedLines(1);

            bw.NormalFont(info);
            bw.High("   " + spec.ToString());

            bw.High(customermsg);
            bw.High(customermsg2);
            bw.NormalFont(print);
            bw.NormalFont("Cashier: " + cashier);
            bw.NormalFont("MOP: " + mop + " ,    Date: " + Convert.ToDateTime(date).ToString("dd-MM-yyy") + time);
            // bw.NormalFont("Date: " +Convert.ToDateTime(date).ToString("dd-MM-yyy")+ DateTime.Now.ToString("HH:mm tt"));
            //if (cusid.Length > 0)
            //{
            //    cusid = ", " + cusid;
            //}
            bw.NormalFont("Order Type :" + ordertype);

            string cusid1 = "";
            if (ordertype == "Take Away")
            {
                cusid = getcustomerid(sid);
                cusid = "Customer Id: " + cusid;
            }
            if (ordertype == "Dine In")
            {
                string tblno = gettbleno(sid);
                cusid = "Table No:" + tblno;
            }
            if (ordertype == "Delivery")
            {
                string tblno = "";// getdelivery(billno);
                tblno = getdeliveryinfo(sid);
                cusid = tblno;
            }


            //bw.NormalFont(cusid);
            // bw.NormalFont("Order No: " + getorderno(sid));
            //bw.NormalFont(delivery);
            //bw.FeedLines(1);
            bw.NormalFont("Customer: " + cusid);
            bw.NormalFont(print);
            bw.NormalFont(itmtitle + " Qty   Price   Total");
            bw.NormalFont(print);
            foreach (DataRow dr in dtr.Rows)
            {
                try
                {
                    //if (dr["Id"].ToString() != string.Empty)
                    {
                        string pc = dr["Price"].ToString();
                        string qnty = "";
                        qnty = dr["Quantity"].ToString();
                        string tmp = qnty;
                        if (tmp == "")
                        {
                            tmp = "1";
                        }
                        float qty = float.Parse(tmp);
                        tmp = pc;
                        if (tmp == "")
                        {
                            tmp = "0";
                        }
                        double sprice = Convert.ToDouble(tmp);
                        double singleprice = 0;
                        int lnt = 0;
                        if (qty >= 1)
                        {
                            singleprice = sprice / qty;
                        }
                        else
                        {
                            singleprice = sprice;
                        }
                        string name = dr["Item"].ToString();
                        string subnm = "";
                        if (name.Length > space.Length)
                        {
                            subnm = name.Substring(space.Length);
                            name = name.Substring(0, space.Length);
                            for (int i = name.Length; i < space.Length + 1; i++)
                            {
                                name += " ";
                            }
                        }
                        else
                        {
                            lnt = name.Length;
                            for (int i = name.Length; i < space.Length + 1; i++)
                            {
                                name += " ";
                            }
                        }
                        string qtyy = qty.ToString();
                        lnt = qtyy.Length;
                        for (int i = qtyy.Length; i < 3; i++)
                        {
                            qtyy = " " + qtyy;
                        }

                        string spc = singleprice.ToString();
                        if (spc.Contains("."))
                        {
                            int strt = spc.IndexOf(".");
                            int count = spc.Substring(strt + 1).Length;
                            if (count < 2)
                            {
                                spc = spc + "0";
                            }
                        }
                        else
                        {
                            spc = spc + ".00";
                        }
                        lnt = spc.Length;
                        for (int i = spc.Length; i < 7; i++)
                        {
                            spc = " " + spc;
                        }
                        //spc = spc + "  ";
                        string spct = sprice.ToString();
                        if (spct.Contains("."))
                        {
                            int strt = spct.IndexOf(".");
                            int count = spct.Substring(strt + 1).Length;
                            if (count < 2)
                            {
                                spct = spct + "0";
                            }
                        }
                        else
                        {
                            spct = spct + ".00";
                        }
                        lnt = spct.Length;
                        for (int i = spct.Length; i < 7; i++)
                        {
                            spct = " " + spct;
                        }
                        bw.NormalFont(name + " " + qtyy.ToString() + " " + spc.ToString() + " " + spct.ToString());//"Garlic bread           2   100     200");
                        if (subnm.Length > 0)
                        {
                            bw.NormalFont(subnm);
                        }

                        string modifier = dr["MdId"].ToString();
                        if (modifier == "")
                        {
                            modifier = "0";
                        }
                        string rmodifier = dr["runtimeflavourid"].ToString();
                        if (rmodifier == "")
                        {
                            rmodifier = "0";
                        }
                        if (modifier == "0" && rmodifier == "0")
                        {
                            try
                            {
                                string q = "SELECT        dbo.MenuItem.KDSId, dbo.MenuItem.Name,dbo.MenuItem.kdsid, dbo.Attachmenu1.Quantity FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.attachmenuid = dbo.MenuItem.Id WHERE        (dbo.Attachmenu1.status = 'active') and dbo.Attachmenu1.menuitemid='" + dr["id"].ToString() + "'";
                                DataSet dsattach = new DataSet();
                                dsattach = objCore.funGetDataSet(q);
                                for (int j = 0; j < dsattach.Tables[0].Rows.Count; j++)
                                {
                                    name = dsattach.Tables[0].Rows[j]["Name"].ToString() + "(In Meal)";
                                    subnm = "";
                                    if (name.Length > space.Length)
                                    {
                                        subnm = name.Substring(space.Length);
                                        name = name.Substring(0, space.Length);
                                        for (int i = name.Length; i < space.Length + 1; i++)
                                        {
                                            name += " ";
                                        }
                                    }
                                    else
                                    {
                                        lnt = name.Length;
                                        for (int i = name.Length; i < space.Length + 1; i++)
                                        {
                                            name += " ";
                                        }
                                    }

                                    string attemp = dsattach.Tables[0].Rows[j]["Quantity"].ToString();
                                    if (attemp == "")
                                    {
                                        attemp = "0";
                                    }
                                    bw.NormalFont(name + "   " + (qty * Convert.ToInt32(attemp)).ToString());//"Garlic bread           2   100     200");
                                    if (subnm.Length > 0)
                                    {
                                        bw.NormalFont(subnm);
                                    }


                                }
                            }
                            catch (Exception e)
                            {


                            }
                        }


                    }
                }
                catch (Exception ex)
                {
                }
            }
            //bw.FeedLines(1);
            bw.NormalFont(print);

            for (int i = total.Length; i < 9; i++)
            {
                total = " " + total;
            }
            for (int i = dis.Length; i < 9; i++)
            {
                dis = " " + dis;
            }
            for (int i = gst.Length; i < 9; i++)
            {
                gst = " " + gst;
            }
            for (int i = r.Length; i < 9; i++)
            {
                r = " " + r;
            }
            for (int i = c.Length; i < 9; i++)
            {
                c = " " + c;
            }
            length = getlinelength("space footer", "generic");
            space = "";
            for (int i = 0; i < length; i++)
            {
                space = space + " ";
            }
            string tender = (Convert.ToDouble(total) + Convert.ToDouble(gst) - Convert.ToDouble(dis)).ToString();
            for (int i = tender.Length; i < 9; i++)
            {
                tender = " " + tender;
            }
            string type = "Sub Total:  ";
            for (int i = type.Length; i < space.Length + 1; i++)
            {
                type += " ";
            }
            bw.NormalFont(type + total);
            type = "Discount:  ";
            for (int i = type.Length; i < space.Length + 1; i++)
            {
                type += " ";
            }
            if (dis != "0")
            {
                bw.NormalFont(type + dis);
            }
            type = "GST     :  ";
            for (int i = type.Length; i < space.Length + 1; i++)
            {
                type += " ";
            }
            if (gst != "0")
            {
                bw.NormalFont(type + gst);
            }
            bw.NormalFont(print);
            type = "Amount Tendered:";
            for (int i = type.Length; i < space.Length + 1; i++)
            {
                type += " ";
            }
            bw.NormalFont(type + tender);
            bw.NormalFont(print);
            type = "Cash Given:  ";
            for (int i = type.Length; i < space.Length + 1; i++)
            {
                type += " ";
            }
            bw.NormalFont(type + r);
            type = "Change:  ";
            for (int i = type.Length; i < space.Length + 1; i++)
            {
                type += " ";
            }
            bw.NormalFont(type + c);
            bw.NormalFont(print);
            string note = dscompany.Tables[0].Rows[0]["WellComeNote"].ToString();
            string subnote = "";
            //if (note.Length > 41)
            //{
            //    subnote = note.Substring(41);
            //    note = note.Substring(0, 42);


            //}
            bw.NormalFont(note);
            bw.NormalFont(print);
            bw.NormalFont("Print Time :" + DateTime.Now.ToString("HH:mm tt"));
            bw.NormalFont(System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, 112, 48, 55, 121 }));
            // bw.NormalFont(subnote);
            // bw.Finish();

        }
        protected int getlinelength(string type, string p)
        {
            DataSet dsl = new DataSet();
            int length = 17;
            try
            {

                string q = "select * from linelength where type='" + type + "' and printr='" + p + "'";
                dsl = objCore.funGetDataSet(q);
                if (dsl.Tables[0].Rows.Count > 0)
                {
                    string temp = dsl.Tables[0].Rows[0]["length"].ToString();
                    if (temp == "")
                    {
                        temp = "17";
                    }
                    length = Convert.ToInt32(temp);
                }
                else
                {
                    length = 17;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsl.Dispose();
            }
            return length;
        }
        protected int getlinelengthgeneric(string type, string p)
        {
            DataSet dsl = new DataSet();
            int length = 17;
            try
            {

                string q = "select * from linelength where type='" + type + "' and printr='" + p + "'";
                dsl = objCore.funGetDataSet(q);
                if (dsl.Tables[0].Rows.Count > 0)
                {
                    string temp = dsl.Tables[0].Rows[0]["length"].ToString();
                    if (temp == "")
                    {
                        temp = "17";
                    }
                    length = Convert.ToInt32(temp);
                }
                else
                {
                    length = 17;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsl.Dispose();
            }
            return length;
        }
     
        private void DuplicaeBill_FormClosing(object sender, FormClosingEventArgs e)
        {
            _frm1.Enabled = true;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            //_frm1.TopMost = true;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

                if (t.Text == "." && txtsearch.Text.Contains("."))
                {

                }
                else
                {
                    txtsearch.Text = txtsearch.Text + t.Text.Replace("&&", "&");
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {

                int index = txtsearch.SelectionStart;
                txtsearch.Text = txtsearch.Text.Remove(txtsearch.SelectionStart - 1, 1);
                txtsearch.Select(index - 1, 1);
                txtsearch.Focus();
            }
            catch (Exception ex)
            {


            }
        }
        public void shiftkey()
        {
            if (button2.Text != "!")
            {
                button2.Text = "!";
                button3.Text = "@";
                button4.Text = "#";
                button5.Text = "$";
                button6.Text = "%";
                button7.Text = "^";
                button8.Text = "&&";
                button9.Text = "*";
                button10.Text = "(";
                button11.Text = ")";
                button12.Text = "Q";
                button16.Text = "W";
                button18.Text = "E";
                button20.Text = "R";
                button22.Text = "T";
                button21.Text = "Y";
                button19.Text = "U";
                button17.Text = "I";
                button15.Text = "O";
                button13.Text = "P";

                button23.Text = "A";
                button25.Text = "S";
                button27.Text = "D";
                button29.Text = "F";
                button31.Text = "G";
                button30.Text = "H";
                button28.Text = "J";
                button26.Text = "K";
                button24.Text = "L";

                button33.Text = "Z";
                button35.Text = "X";
                button37.Text = "C";
                button39.Text = "V";
                button41.Text = "B";
                button40.Text = "N";
                button38.Text = "M";
                // button36.Text = "o";


            }
            else
            {
                button2.Text = "1";
                button3.Text = "2";
                button4.Text = "3";
                button5.Text = "4";
                button6.Text = "5";
                button7.Text = "6";
                button8.Text = "7";
                button9.Text = "8";
                button10.Text = "9";
                button11.Text = "0";
                button12.Text = "q";
                button16.Text = "w";
                button18.Text = "e";
                button20.Text = "r";
                button22.Text = "t";
                button21.Text = "y";
                button19.Text = "u";
                button17.Text = "i";
                button15.Text = "o";
                button13.Text = "p";

                button23.Text = "a";
                button25.Text = "s";
                button27.Text = "d";
                button29.Text = "f";
                button31.Text = "g";
                button30.Text = "h";
                button28.Text = "j";
                button26.Text = "k";
                button24.Text = "l";

                button33.Text = "z";
                button35.Text = "x";
                button37.Text = "c";
                button39.Text = "v";
                button41.Text = "b";
                button40.Text = "n";
                button38.Text = "m";


            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txtsearch.Text = txtsearch.Text + " ";
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                qrcode = "";
                //int indx = dataGridView1.CurrentCell.RowIndex;

                if (gridindex >= 0)
                {
                    string invoiceno = "";

                    // DialogResult dr = MessageBox.Show("Are you sure to print duplicate bill?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    // if (dr == DialogResult.Yes)
                    {

                        string svschrgs = dataGridView1.Rows[gridindex].Cells["servicecharges"].Value.ToString();
                        string ordertype = dataGridView1.Rows[gridindex].Cells["OrderType"].Value.ToString();
                        string date = Convert.ToDateTime(dataGridView1.Rows[gridindex].Cells[1].Value.ToString()).ToString("yyyy-MM-dd");
                        string time = dataGridView1.Rows[gridindex].Cells[2].Value.ToString();
                        string saleid = dataGridView1.Rows[gridindex].Cells[0].Value.ToString();
                        if (_frm1.printinvoiceno.Trim() != "" && _frm1.printinvoiceno.Trim() != "no")
                        {
                            invoiceno = _frm1.getinvoicenopaid(saleid);
                        }
                        getqrcode(saleid);
                        string billtype = dataGridView1.Rows[gridindex].Cells[4].Value.ToString();
                        string totalbil = dataGridView1.Rows[gridindex].Cells[5].Value.ToString();
                        string discount = dataGridView1.Rows[gridindex].Cells[10].Value.ToString();
                        string discountamount = dataGridView1.Rows[gridindex].Cells[6].Value.ToString();
                        string NetBill = dataGridView1.Rows[gridindex].Cells[3].Value.ToString();
                        string GST = dataGridView1.Rows[gridindex].Cells[7].Value.ToString();
                        string GSTperc = dataGridView1.Rows[gridindex].Cells[9].Value.ToString();
                        string customer = dataGridView1.Rows[gridindex].Cells[8].Value.ToString();
                        string cashier = dataGridView1.Rows[gridindex].Cells[11].Value.ToString();
                        
                        
                        {
                            int print = 1;
                            string printername = "";
                            DataSet dsprint = new DataSet();
                            string q = "select * from Printers where type='Receipt'";

                            getcompany();
                            string customermsg = dscompany.Tables[0].Rows[0]["CustomerMessage"].ToString();
                            string customermsg2 = dscompany.Tables[0].Rows[0]["CustomerMessage2"].ToString();

                            DataTable dt = new DataTable();

                            // Just set the name of data table
                            dt.TableName = "Crystal Report";
                            dt = getAllOrders(billtype, saleid, totalbil, discount, NetBill, GST, date, time);
                            string info = "";// getdeliveryinfo(saleid.ToString());
                            if (ordertype == "Dine In")
                            {
                                string tblno = gettbleno(saleid.ToString());
                                info = "Table No: " + tblno;
                            }
                            else if (ordertype == "Take Away")
                            {
                                string cusid = getcustomerid(saleid.ToString());
                                info = "Customer Id: " + cusid;
                            }
                            else
                            {

                                info = getdeliveryinfo(saleid.ToString());
                            }
                            string url = _frm1.pointsurl;
                            if (url == "")
                            {
                                try
                                {
                                    string value;
                                    value = CacheClass.Cache["pointsurl"] as string;
                                    if (null == value)
                                    {

                                    }
                                    else
                                    {
                                        url = value;
                                    }
                                }
                                catch (Exception ex)
                                {


                                }
                            }
                            string path = Path.GetDirectoryName(Application.ExecutablePath);
                            PrintClass.Printt(path, dt, billtype, saleid.ToString(), "", ordertype, totalbil, NetBill, discount, GSTperc, "0", "0", printername, info, print, discountamount.ToString(), GST.ToString(), customermsg, customermsg2, svschrgs, cashier, date, "", _frm1, qrcode, url, invoiceno, "Print Preview", "Preview");

                        }
                       
                    }
                }
                else
                {
                    MessageBox.Show("Please Select a Sale First from Sales List");
                }
            }
            catch (Exception ex)
            {


            }
 
        }

        
       
    }
}
