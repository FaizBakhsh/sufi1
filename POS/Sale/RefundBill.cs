using OposPOSPrinter_CCO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class RefundBill : Form
    {
        private  RestSale _frm1;
       public string date = "";
        POSRestaurant.classes.Clsdbcon objCore ;
        DataSet ds ;
        public RefundBill(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            objCore = new classes.Clsdbcon();
        }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";
            
        //}

        private void button1_Click(object sender, EventArgs e)
        {
          
               
                    
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
           // getdata();
        }
        public void getdata()
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                q9 = "";// "SELECT     Id as BillNo, Date, time, NetBill   FROM         Sale where id='" + txtbill.Text.Trim() + "'  and BillStatus='Paid' and date='" + date + "'";
                q9 = "SELECT     TOP (100) PERCENT dbo.Saledetails.id,dbo.MenuItem.Name, dbo.ModifierFlavour.name AS Size, dbo.Saledetails.Quantity, dbo.Saledetails.Price,dbo.Saledetails.ItemdiscountPerc, dbo.Saledetails.ItemGstPerc FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                      dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId AND dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id where dbo.Saledetails.saleid='" + txtbill.Text.Trim() + "' ORDER BY dbo.MenuItem.Name";

                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                try
                {
                    string dis = dataGridView1.Rows[0].Cells["5"].Value.ToString();
                    if (dis == "")
                    {
                        dis = "0";
                    }
                    txtdiscount.Text = dis;
                }
                catch (Exception ex)
                {


                }
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 40;
                }
                gettotal();
            }
            catch (Exception ex)
            {


            }
        }
        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {

        }
        private void vButton41_Click(object sender, EventArgs e)
        {
            string saleid = txtbill.Text;
                
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("Please Provide a Reason of Refund");
                return;
            }
            DialogResult rs = MessageBox.Show("Are You Sure to Refund Selected items", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {
                 bool chk = true;
                 int i = 0;
                 foreach (DataGridViewRow gr in dataGridView1.Rows)
                 {
                     var isChecked = dataGridView1[0, i].Value as bool? ?? false;
                     i++;
                     if (isChecked == true)
                     {
                         chk = false;
                         string sid = string.Empty;
                         try
                         {
                             sid = gr.Cells["id"].Value.ToString();

                         }
                         catch (Exception ex)
                         {


                         }
                         string q = "INSERT INTO Saledetailsrefund (Id, saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus, branchid, Itemdiscount,                       ItemdiscountPerc, ItemGst, ItemGstPerc) SELECT     Id, saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus, branchid, Itemdiscount,                       ItemdiscountPerc, ItemGst, ItemGstPerc FROM         Saledetails WHERE     (id = '"+sid+"')";
                         objCore.executeQuery(q);
                         q = "update Saledetailsrefund set reason='" + richTextBox1.Text + "' where id='" + sid + "'";
                         objCore.executeQuery(q);
                         q = "delete from Saledetails where id='"+sid+"'";
                         objCore.executeQuery(q);
                     }
                 }
               // int indx = dataGridView1.CurrentCell.RowIndex;
                 if (chk == false)
                 {
                     
                     //string type = dataGridView1.Rows[indx].Cells[4].Value.ToString();
                     int idd = 1;
                     DataSet dsdayend = new DataSet();
                     dsdayend = objCore.funGetDataSet("select max(id) as id from refund");
                     if (dsdayend.Tables[0].Rows.Count > 0)
                     {
                         string ii = dsdayend.Tables[0].Rows[0][0].ToString();
                         if (ii == string.Empty)
                         {
                             ii = "0";
                         }
                         idd = Convert.ToInt32(ii) + 1;

                     }
                     objCore.executeQuery("insert into refund (id,saleid,reason) values('" + idd + "','" + saleid + "','" + richTextBox1.Text.Replace("'", "''") + "')");
                     objCore.executeQuery("update sale set TotalBill='" + txttotal.Text + "',DiscountAmount='" + txtdiscountamount.Text + "',GST='" + txtgst.Text + "',NetBill='" + txtnet.Text + "' where id='" + saleid + "'");
                     objCore.executeQuery("update billtype set amount='" + txtnet.Text + "' where saleid='" + saleid + "'");
                     if (txtnet.Text == "0")
                     {
                         objCore.executeQuery("update sale set Billstatus='Paid' where id='" + saleid + "'");
                   
                     }
                     richTextBox1.Text = string.Empty;
                     txtbill.Text = string.Empty;
                     getdata();

                     DataTable tbl = dataGridView1.DataSource as DataTable;
                     //PrintReceipt(tbl, "", date, "", Convert.ToDouble("0") + 0, Convert.ToDouble("0"), saleid.ToString(), "", txttotal.Text, txtgst.Text, txtdiscountamount.Text, "0", txtdiscount.Text, txtnet.ToString(), "partial");
               
                         
                         
                     // _frm1.recalsale(saleid, "no");
                     //_frm1.cleargrid();
                     MessageBox.Show("Selected Items Refunded Successfully");
                 }
                 else
                 {
                     MessageBox.Show("Please Select an Item to Refund");
                 }
            }

        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            //_frm1.Islbldelivery = "Not Selected";
            _frm1.Enabled = true;
            //_frm1.TopMost = true;
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public double servicecharhes = 0;
        public void gettotal()
        {
            try
            {

                double amout = 0;
                DataGridViewCellStyle RedCellStyle = null;
                RedCellStyle = new DataGridViewCellStyle();
                RedCellStyle.ForeColor = Color.RoyalBlue;
                DataGridViewCellStyle GreenCellStyle = null;
                GreenCellStyle = new DataGridViewCellStyle();
                GreenCellStyle.ForeColor = Color.Green;
                {
                    int i = 0;
                    bool chk = true;
                    foreach (DataGridViewRow gr in dataGridView1.Rows)
                    {
                        var isChecked = dataGridView1[0, i].Value as bool? ?? false;
                        i++;
                        if (isChecked == false)
                        {
                            chk = false;
                            string gcell = string.Empty;
                            try
                            {
                                gcell = gr.Cells["price"].Value.ToString();

                            }
                            catch (Exception ex)
                            {


                            }
                            if (gcell == string.Empty)
                            {

                            }
                            else
                            {
                                amout = amout + Convert.ToDouble(gcell);
                            }
                            double service = 0;
                            if (txtdiscount.Text.Trim() == string.Empty)
                            {
                                txtdiscount.Text = "0";
                            }
                            double gst = 0;
                            DataSet dsgst = new DataSet();
                            dsgst = objCore.funGetDataSet("select * from gst");
                            if (dsgst.Tables[0].Rows.Count > 0)
                            {
                                gst = float.Parse(dsgst.Tables[0].Rows[0]["gst"].ToString());

                            }
                            else
                            {
                                gst = 0;
                            }
                            amout = Math.Round(amout, 2);
                            txttotal.Text = amout.ToString();
                            double dscount = Convert.ToDouble(txtdiscount.Text.Trim());
                            dscount = (dscount * amout) / 100;
                            // dscount = Math.Round(dscount, 2);
                            double discountedtotal = amout;// -dscount;

                            service = (servicecharhes * discountedtotal) / 100;
                            txtservice.Text = service.ToString();
                            double totalgst = (gst * discountedtotal) / 100;

                            discountedtotal = amout - dscount;
                            discountedtotal = discountedtotal + service;
                            // discountedtotal = amout;// -dscount;
                            totalgst = Math.Round(totalgst, 2);
                            string discountamount = dscount.ToString();
                            txtdiscountamount.Text = dscount.ToString();
                            txtgst.Text = totalgst.ToString();
                            txtnet.Text = Math.Round((totalgst + discountedtotal), 2).ToString();
                        }

                    }
                    if (i > 0 && chk == true)
                    {
                        txttotal.Text = "0";
                        txtdiscount.Text = "0";
                        txtgst.Text = "0";
                        txtdiscountamount.Text = "0";
                        txtnet.Text = "0";
                       
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void vButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                q9 = "";// "SELECT     Id as BillNo, Date, time, NetBill   FROM         Sale where id='" + txtbill.Text.Trim() + "'  and BillStatus='Paid' and date='" + date + "'";
                q9 = "SELECT     TOP (100) PERCENT dbo.Saledetails.id,dbo.MenuItem.Name, dbo.ModifierFlavour.name AS Size, dbo.Saledetails.Quantity, dbo.Saledetails.Price,dbo.Saledetails.Itemdiscount, dbo.Saledetails.ItemGst,dbo.Saledetails.ItemdiscountPerc FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                      dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId AND dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id where dbo.Saledetails.saleid='" + txtbill.Text.Trim() + "' ORDER BY dbo.MenuItem.Name";
                
                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                try
                {
                    string dis = dataGridView1.Rows[0].Cells["8"].Value.ToString();
                    if (dis == "")
                    {
                        dis = "0";
                    }
                    txtdiscount.Text = dis;
                }
                catch (Exception ex)
                {
                    
                    
                }
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 40;
                }
                gettotal();
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton2_Click_1(object sender, EventArgs e)
        {
            getdata();
        }
        private void PrintReceipt(DataTable dtref, string customer, string date, string cashier, double received, double change, string bill, string saletype, string totalt, string gstt, string dist, string gstperc, string disperc, string net, string ordertype)
        {
            OPOSPOSPrinter printer = new OPOSPOSPrinter(); ;
            try
            {
                string pname = printername(ordertype);
                printer.Open(pname);
                printer.ClaimDevice(2000);
                printer.DeviceEnabled = true;
                double total = 0, gst = 0, dis = 0, nettotal = 0;
                if (printer.DeviceEnabled == true)
                {
                    getcompany();
                    string name = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    string addrs = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    string phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    string wellcom = dscompany.Tables[0].Rows[0]["WellComeNote"].ToString();
                    PrintReceiptHeader(printer, name, addrs, bill, phone, date, cashier, saletype, customer, ordertype);
                   
                    int i = 0;
                    bool chk = false;
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        try
                        {
                            if (ordertype == "full")
                            {
                                
                                {
                                    string pc = dr.Cells["Price"].Value.ToString();
                                    string qnty = "";
                                    qnty = dr.Cells["Quantity"].Value.ToString();
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
                                    total = total + sprice;

                                    tmp = dr.Cells["ItemGst"].Value.ToString();
                                    if (tmp == "")
                                    {
                                        tmp = "1";
                                    }
                                    gst = gst + Convert.ToDouble(tmp);
                                    tmp = dr.Cells["Itemdiscount"].Value.ToString();
                                    if (tmp == "")
                                    {
                                        tmp = "1";
                                    }
                                    dis = dis + Convert.ToDouble(tmp);
                                    double singleprice = 0;
                                    singleprice = sprice / qty;
                                    string size = dr.Cells["size"].Value.ToString();
                                    if (size.Length > 0)
                                    {
                                        size = size.Substring(0, 1)+"'";
                                    }
                                    string nm =  size + dr.Cells["name"].Value.ToString();
                                    PrintLineItem(printer, nm, qty, Convert.ToDouble(singleprice));
                                }
                            }
                            else
                            {
                                var isChecked = dataGridView1[0, i].Value as bool? ?? false;
                                i++;
                                if (isChecked == true)
                                {
                                    chk = false;
                                   
                                    {
                                        string pc = dr.Cells["Price"].Value.ToString();
                                        string qnty = "";
                                        qnty = dr.Cells["Quantity"].Value.ToString();
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
                                        total = total + sprice;
                                        double singleprice = 0;
                                        tmp = dr.Cells["ItemGst"].Value.ToString();
                                        if (tmp == "")
                                        {
                                            tmp = "1";
                                        }
                                        gst = gst + Convert.ToDouble(tmp);
                                        tmp = dr.Cells["Itemdiscount"].Value.ToString();
                                        if (tmp == "")
                                        {
                                            tmp = "1";
                                        }
                                        dis = dis + Convert.ToDouble(tmp);
                                        singleprice = sprice / qty;
                                        string size = dr.Cells["size"].Value.ToString();
                                        if (size.Length > 0)
                                        {
                                            size = size.Substring(0, 1) + "'";
                                        }
                                        string nm = size + dr.Cells["name"].Value.ToString();
                                        PrintLineItem(printer, nm, qty, Convert.ToDouble(singleprice));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    nettotal = total + gst;
                    nettotal = nettotal - dis;
                    PrintReceiptFooter(printer, Convert.ToDouble(total), Convert.ToDouble(gst), Convert.ToDouble(dis), wellcom, received, change, Convert.ToDouble(disperc), Convert.ToDouble(gstperc), saletype, Convert.ToDouble(nettotal));

                }
            }
            finally
            {
                DisconnectFromPrinter(printer);
            }
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
            else
            {
                temp = "opos";
            }
            DataSet ds = new DataSet();
            string q = "select * from printers where type='" + temp + "'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                name = ds.Tables[0].Rows[0]["name"].ToString();
            }
            return name;
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        
        private void DisconnectFromPrinter(OPOSPOSPrinter printer)
        {
            try
            {
                printer.ReleaseDevice();
                printer.Close();
            }
            catch (Exception ex)
            {


            }
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
        public string getnetval = "0";
        public string getnetvalue()
        {
            string val = getnetval;
            return val;
        }

        private void PrintReceiptFooter(OPOSPOSPrinter printer, double subTotal, double tax, double discount, string footerText, double received, double change, double disp, double gstp, string type, double net)
        {
            try
            {
                string offSetString = new string(' ', printer.RecLineChars / 2);
                double servc = 0;
                if (servicecharhes > 0)
                {
                    try
                    {
                        servc = Convert.ToDouble(txtservice.Text);
                    }
                    catch (Exception ex)
                    {


                    }
                }
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, String.Format("SUB-TOTAL                     {0}", subTotal.ToString("#0.00")));
               // if (gstp > 0)
                {
                    PrintTextLine(printer, String.Format("GST                           {0}", tax.ToString("#0.00") + "(" + gstp + "%)"));
                }
                if (servicecharhes > 0)
                {
                    PrintTextLine(printer, String.Format("Service Charges               {0}", servc.ToString("#0.00") + "(" + servicecharhes + "%)"));
                }
                if (discount > 0)
                {
                    PrintTextLine(printer, String.Format("DISCOUNT                       {0}", discount.ToString("#0.00") + "(" + disp + "%)"));
                }
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                PrintTextLine(printer, String.Format("Amount Tendered               {0}", (net).ToString("#0.00")));
                PrintTextLine(printer, new string('-', (printer.RecLineChars)));
                //PrintTextLine(printer, String.Format("Cash Given                    {0}", (received).ToString("#0.00")));
                //PrintTextLine(printer, String.Format("Change Given                  {0}", (change).ToString("#0.00")));
                
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                PrintTextLine(printer, String.Empty);
                //Print 'advance and cut' escape command.
                printer.CutPaper(50);
               
            }
            catch (Exception ex)
            {


            }
            // PrintTextLine(printer, System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'1', (byte)'0', (byte)'0', (byte)'P', (byte)'f', (byte)'P' }));
        }
        protected int getlinelength(string type, string p)
        {
            int length = 17;
            try
            {
                DataSet dsl = new DataSet();
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
            return length;
        }
        //private void PrintLineItem(OPOSPOSPrinter printer, string itemCode, double quantity, double unitPrice)
        //{
        //    int length = getlinelength("space", "receipt");
        //    try
        //    {
        //        string temp = "";
        //        int indx = 0;
        //        if (itemCode.Length > length)
        //        {
        //            string offSetString = new string(' ', length - indx);
        //            string val = itemCode.Substring(0, length);
        //            // indx = val.IndexOf(" ", 0);
        //            //temp = itemCode.Substring(0, indx);// +offSetString;
        //            PrintText(printer, TruncateAt(val, length + 1));
        //        }
        //        else
        //        {
        //            string offSetString = new string(' ', length - itemCode.Length);
        //            string val = itemCode + offSetString;
        //            PrintText(printer, TruncateAt(val, length));
        //        }
        //        PrintText(printer, TruncateAt(quantity.ToString().PadLeft(3), 9));
        //        PrintText(printer, TruncateAt(unitPrice.ToString().PadLeft(9), 9));
        //        PrintTextLine(printer, TruncateAt((quantity * unitPrice).ToString().PadLeft(9), 9));
        //        if (itemCode.Length > length)
        //        {
        //            //temp = itemCode.Substring(indx + 1);
        //            temp = itemCode.Substring(length);
        //            PrintText(printer, TruncateAt(temp, length + 1));
        //            PrintText(printer, TruncateAt("".PadLeft(3), 9));
        //            PrintText(printer, TruncateAt("".PadLeft(9), 9));
        //            PrintTextLine(printer, TruncateAt(("").ToString().PadLeft(9), 9));
        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
        private void PrintLineItem(OPOSPOSPrinter printer, string itemCode, double quantity, double unitPrice)
        {
            int length = getlinelength("space", "receipt");
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


        public Bitmap BytesToBitmap(byte[] byteArray)
        {

            return (Bitmap)Image.FromStream(new MemoryStream(byteArray));



        }

        public byte[] ImageToByteArray(Image img)
        {

            using (MemoryStream ms = new MemoryStream())
            {

                img.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

                return ms.ToArray();

            }

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
            }
            return info;
        }
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
        private void PrintReceiptHeader(OPOSPOSPrinter printer, string companyName, string addressLine1, string billno, string taxNumber, string dateTime, string cashierName, string mop, string customer, string type)
        {


            try
            {
                int width = Convert.ToInt32(dscompany.Tables[0].Rows[0]["width"].ToString());
                string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
                printer.SetBitmap(1, 2, baseDir + "\\" + dscompany.Tables[0].Rows[0]["logoname"].ToString(), width, 2);
            }
            catch (Exception ex)
            {

            }
            string ESCp = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27 });
            int length = getlinelength("logo", "receipt");
            if (length == 17)
            {
                length = 10;
            }
            string spacelogo = "";
            for (int i = 0; i < length; i++)
            {
                spacelogo = spacelogo + " ";
            }
            try
            {
                printer.PrintNormal(2, spacelogo + ESCp + "|1B");
                printer.PrintNormal(2, " " + Environment.NewLine);
            }
            catch (Exception ex)
            {


            }
            string space = "";


            try
            {
                length = getlinelength("name", "receipt");
                if (length == 17)
                {
                    length = 1;
                }
                for (int i = 0; i < length; i++)
                {
                    companyName = " " + companyName;
                }
                length = getlinelength("address", "receipt");
                if (length == 17)
                {
                    length = 5;
                }
                for (int i = 0; i < length; i++)
                {
                    addressLine1 = " " + addressLine1;
                }
                length = getlinelength("phone", "receipt");

                if (length == 17)
                {
                    length = 5;
                }
                for (int i = 0; i < length; i++)
                {
                    taxNumber = " " + taxNumber;
                }
                string offSetString = new string(' ', printer.RecLineChars / 3 - 2);
                string Bold = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'b', (byte)'C' });
                string Bold1 = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27, (byte)'|', (byte)'c', (byte)'A' });
                string ESC = System.Text.ASCIIEncoding.ASCII.GetString(new byte[] { 27 });
                if (mop != "kitchen")
                {
                    if (companyName.Contains("Gloria"))
                    {
                        PrintTextLine(printer, offSetString + (Bold + companyName));
                        PrintTextLine(printer, offSetString + Bold + addressLine1);
                        PrintTextLine(printer, offSetString + Bold + taxNumber);
                    }
                    else
                    {
                        PrintTextLine(printer, offSetString + (Bold + companyName));
                        PrintTextLine(printer, Bold + addressLine1);
                        PrintTextLine(printer, Bold + taxNumber);
                    }
                    PrintTextLine(printer, new string('-', printer.RecLineChars));
                }
                if (getselectordertype().ToLower() == "yes")
                {
                    PrintTextLine(printer, String.Format("Bill No : {0}", billno));
                }
                else
                {
                    PrintTextLine(printer, String.Format("Bill No : {0}", billno + "       Customer :" + customer));
                }
                if (mop != "kitchen")
                {
                   // PrintTextLine(printer, String.Format("CASHIER : {0}", cashierName + "  MOP :" + mop));
                }
                string date = dateTime;
                try
                {
                    date = Convert.ToDateTime(dateTime).ToString("dd-MM-yyyy");
                }
                catch (Exception ex)
                {


                }
                PrintTextLine(printer, String.Format("DATE : {0}", Bold1 + date + " " + DateTime.Now.ToShortTimeString()));

                if (getselectordertype().ToLower() == "yes")
                {
                    string cusid = "";
                    if (type == "Take Away")
                    {
                        cusid = getcustomerid(billno);
                        cusid = "Customer Id: " + cusid;
                    }
                    if (type == "Dine In")
                    {
                        string tblno = gettbleno(billno);
                        cusid = "Table No:" + tblno;
                    }
                    if (type == "Delivery")
                    {
                        string tblno = "";// getdelivery(billno);
                        tblno = getdeliveryinfo(billno);
                        cusid = tblno;
                    }
                   // PrintTextLine(printer, String.Format("Order Type: {0}", type + " " + cusid));

                }

                PrintTextLine(printer, new string('-', printer.RecLineChars));
                PrintTextLine(printer, offSetString + "    " + (Bold + "Refund Slip"));
                printer.PrintNormal(2, " " + Environment.NewLine);
                length = getlinelength("space", "receipt");
                length = length - 9;

                for (int i = 0; i < length; i++)
                {
                    space = space + " ";
                }
                string text = Bold + "Item Name" + space + "QTY " + "Unit Price  " + "Total";
                PrintTextLine(printer, text);
                //PrintText(printer, Bold + "Item Name" + space);
                //PrintText(printer, Bold + "QTY ");
                //PrintText(printer, Bold + "Unit Price  ");
                //PrintTextLine(printer, Bold + "Total");
                PrintTextLine(printer, new string('=', printer.RecLineChars));
            }
            catch (Exception ex)
            {


            }
            //PrintTextLine(printer, String.Empty);

        }
        public string gettbleno(string id)
        {
            string tbl = "";

            try
            {
                DataSet dstbl = new DataSet();
                string q = "select TableNo from DinInTables where Saleid='" + id + "'";
                q = "SELECT dbo.DinInTables.TableNo, dbo.ResturantStaff.Name FROM  dbo.DinInTables INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where dbo.DinInTables.Saleid='" + id + "'";
                dstbl = objCore.funGetDataSet(q);
                if (dstbl.Tables[0].Rows.Count > 0)
                {
                    tbl = dstbl.Tables[0].Rows[0][0].ToString() + "(" + dstbl.Tables[0].Rows[0][1].ToString() + ")";
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
            try
            {
                if (text.Length < printer.RecLineChars || text.Contains("\n"))
                    printer.PrintNormal(2, text + Environment.NewLine); //Print text, then a new line character.
                else if (text.Length > printer.RecLineChars)
                    printer.PrintNormal(2, TruncateAt(text, printer.RecLineChars)); //Print exactly as many characters as the printer allows, truncating the rest, no new line character (printer will probably auto-feed for us)
                else if (text.Length == printer.RecLineChars)
                    printer.PrintNormal(2, text + Environment.NewLine); //Print text, no new line character, printer will probably auto-feed for us.
            }
            catch (Exception ex)
            {


            }
        }

        private string TruncateAt(string text, int maxWidth)
        {
            string retVal = text;
            if (text.Length > maxWidth)
                retVal = text.Substring(0, maxWidth);

            return retVal;
        }
        public void shiftkey()
        {
            if (button49.Text != "!")
            {
                button49.Text = "!";
                button51.Text = "@";
                button53.Text = "#";
                button55.Text = "$";
                button57.Text = "%";
                button56.Text = "^";
                button54.Text = "&&";
                button52.Text = "*";
                button50.Text = "(";
                button48.Text = ")";
                button21.Text = "Q";
                button34.Text = "W";
                button43.Text = "E";
                button45.Text = "R";
                button47.Text = "T";
                button46.Text = "Y";
                button44.Text = "U";
                button42.Text = "I";
                button22.Text = "O";
                button20.Text = "P";
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
                button49.Text = "1";
                button51.Text = "2";
                button53.Text = "3";
                button55.Text = "4";
                button57.Text = "5";
                button56.Text = "6";
                button54.Text = "7";
                button52.Text = "8";
                button50.Text = "9";
                button48.Text = "0";

                button21.Text = "q";
                button34.Text = "w";
                button43.Text = "e";
                button45.Text = "r";
                button47.Text = "t";
                button46.Text = "y";
                button44.Text = "u";
                button42.Text = "i";
                button22.Text = "o";
                button20.Text = "p";

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
        private void button49_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

                // if (richTextBox1 != null)
                {

                    {
                        richTextBox1.Text = richTextBox1.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button58_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text + " ";
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //gettotal();
        }

        private void dataGridView1_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            gettotal();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
            gettotal();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void vButton2_Click_2(object sender, EventArgs e)
        {
            string saleid = txtbill.Text;
                  
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("Please Provide a Reason of Refund");
                return;
            }
            DialogResult rs = MessageBox.Show("Are You Sure to Refund Selected items", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rs == DialogResult.Yes)
            {               
                // int indx = dataGridView1.CurrentCell.RowIndex;
                 if (dataGridView1.Rows.Count > 0)
                 {
                     
                     //string type = dataGridView1.Rows[indx].Cells[4].Value.ToString();
                     int idd = 1;
                     DataSet dsdayend = new DataSet();
                     dsdayend = objCore.funGetDataSet("select max(id) as id from refund");
                     if (dsdayend.Tables[0].Rows.Count > 0)
                     {
                         string ii = dsdayend.Tables[0].Rows[0][0].ToString();
                         if (ii == string.Empty)
                         {
                             ii = "0";
                         }
                         idd = Convert.ToInt32(ii) + 1;
                     }
                     objCore.executeQuery("insert into refund (id,saleid,reason) values('" + idd + "','" + saleid + "','" + richTextBox1.Text.Replace("'", "''") + "')");
                     DataSet dsref = new DataSet();
                     string q = "select id from saledetails where saleid='"+saleid+"'";
                     dsref = objCore.funGetDataSet(q);
                     for (int i = 0; i < dsref.Tables[0].Rows.Count; i++)
                     {
                         string sid = dsref.Tables[0].Rows[i]["id"].ToString();
                         q = "INSERT INTO Saledetailsrefund (Id, saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus, branchid, Itemdiscount,                       ItemdiscountPerc, ItemGst, ItemGstPerc) SELECT     Id, saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus, branchid, Itemdiscount,                       ItemdiscountPerc, ItemGst, ItemGstPerc FROM         Saledetails WHERE     (id = '" + sid + "')";
                         objCore.executeQuery(q);
                         q = "update Saledetailsrefund set reason='" + richTextBox1.Text + "' where id='" + sid + "'";
                         objCore.executeQuery(q);
                         q = "delete from Saledetails where id='" + sid + "'";
                         objCore.executeQuery(q);
                     }
                     objCore.executeQuery("update sale set BillStatus='Refund' where id='" + saleid + "'");
                     objCore.executeQuery("update sale set TotalBill='0',DiscountAmount='0',GST='0',NetBill='0',servicecharges='0' where id='" + saleid + "'");
                     objCore.executeQuery("update billtype set amount='0' where saleid='" + saleid + "'");
                     richTextBox1.Text = string.Empty;
                     txtbill.Text = string.Empty;
                     DataTable tbl = dataGridView1.DataSource as DataTable;
                    // PrintReceipt(tbl, "", date, "", Convert.ToDouble("0") + 0, Convert.ToDouble("0"), saleid.ToString(), "", txttotal.Text, txtgst.Text, txtdiscountamount.Text, "0", txtdiscount.Text, txtnet.ToString(), "full");
              
                     getdata();
                     // _frm1.recalsale(saleid, "no");
                     //_frm1.cleargrid();
                     MessageBox.Show("Selected Bill Refunded Successfully");
                 }
                 else
                 {
                     MessageBox.Show("Please Select a Bill to Refund");
                 }
            }

        }
    }
}
