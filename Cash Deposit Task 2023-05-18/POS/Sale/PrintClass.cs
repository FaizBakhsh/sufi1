using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace POSRestaurant.Sale
{
   public class PrintClass
    {
       public static void Printt(string path, DataTable dtprint, string mop, string sid, string cardno, string ordertype, string total, string nettotal, string discount, string gst, string cash, string change, string printername, string deliveryinfo, int prints, string discountamount, string gstamount, string msg1, string msg2, string serice, string cashier, string date, string delivery, RestSale _frm, string qrcode, string pointsurl, string invoiceno, string billtype,string type)
       {
           if (dtprint.Rows.Count == 0)
           {
               return;
           }
           string name2 = _frm.name2;
           string provisionalbill = _frm.provisionalbill;
           POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
           IList<DiscountkeysClass> Discountkeys = null;
           try
           {
               DataSet dsindividual = new DataSet();
               string q = "select convert(varchar(100), id) as id,name from DiscountKeys";
               dsindividual = objcore.funGetDataSet(q);

               Discountkeys = dsindividual.Tables[0].AsEnumerable().Select(row =>
               new DiscountkeysClass
               {
                   id = row.Field<string>("id"),
                   name = row.Field<string>("name")


               }).ToList();
           }
           catch (Exception ex)
           {


           }
           
           IList<DiscountIndividualClass> DiscountIndividual = null;
           try
           {
               DataSet dsindividual = new DataSet();
               string q = "select id,discount,convert(varchar(100), DiscountPerc) as DiscountPerc,convert(varchar(100), MenuItemId) as MenuItemId,convert(varchar(100), Saleid) as Saleid,convert(varchar(100), Saledetailsid) as Saledetailsid,convert(varchar(100), Runtimemodifierid) as Runtimemodifierid,convert(varchar(100), flavourid) as flavourid  from DiscountIndividual where Saleid='" + sid + "'";
               dsindividual = objcore.funGetDataSet(q);

               DiscountIndividual = dsindividual.Tables[0].AsEnumerable().Select(row =>
               new DiscountIndividualClass
               {
                   DiscountPerc = row.Field<string>("DiscountPerc"),
                   discountAMount = row.Field<double>("discount"),
                   MenuItemId = row.Field<string>("MenuItemId"),
                   Saleid = row.Field<string>("Saleid"),
                   Saledetailsid = row.Field<string>("Saledetailsid"),
                   Runtimemodifierid = row.Field<string>("Runtimemodifierid"),
                   flavourid = row.Field<string>("flavourid")


               }).ToList();
           }
           catch (Exception ex)
           {


           }
           float servicecharhes = 0;
           string time = "";
           string servicegsttype = "", servicetitle = _frm.servicechargestitle;
           if (servicetitle.Trim() == "")
           {
               servicetitle = "Service Charges";
           }
           string servicetype = "";
           try
           {
               servicegsttype = _frm.servicegsttype;

               servicetype = _frm.servicetype;
               servicecharhes = _frm.servicecharhes;
           }
           catch (Exception ex)
           {

           }
           double servicecharges = 0, servicechargescash = 0, servicechargescard = 0;
           if (serice == "")
           {
               serice = "0";
           }
           try
           {
               servicecharges = Convert.ToDouble(serice);
           }
           catch (Exception ex)
           {

           }
           if (discountamount == "")
           {
               discountamount = "0";
           }
           string token = "", qrtitle = "", fbrcode = "", fbrurl = "", qrmenu = "";
           try
           {
               if (File.Exists("C:\\qr.jpg"))
               {
                   qrmenu = "C:\\qr.jpg";
               }
           }
           catch (Exception ex)
           {
               
              
           }

           string fullpath = "";
           try
           {

               byte[] code = null;

               try
               {
                   // qrcode =  "11222";
                   if (qrcode.Length > 0)
                   {
                       
                       fbrurl = "C:\\fbrlogo.jpg";
                       fbrcode = "Sales Tax QR Code";
                       Zen.Barcode.CodeQrBarcodeDraw qr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                       Image img = qr.Draw(qrcode, 30);
                       //pictureBox1.Image = img;
                       code = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));
                   }
               }
               catch (Exception ex)
               {

               }
               string printvisa = "yes", logoname = "";

               DataSet dscompany = new DataSet();
               if (_frm.multibranches == "Enabled")
               {
                   dscompany = objcore.funGetDataSet("select * from companyinfo where branchid='" + _frm.branchid + "'");
               }
               else
               {
                   dscompany = objcore.funGetDataSet("select * from companyinfo");
               }
               try
               {
                   if (dscompany.Tables[0].Rows.Count > 0)
                   {
                       printvisa = dscompany.Tables[0].Rows[0]["printvisa"].ToString();
                       logoname = dscompany.Tables[0].Rows[0]["logoname"].ToString();
                   }
               }
               catch (Exception ex)
               {
               }
               string gstvisa = _frm.gstpercvisa;
               if (printvisa == "")
               {
                   printvisa = "no";
               }
               if (gstvisa == "")
               {
                   gstvisa = "0";
               }
               if (Convert.ToDouble(gstvisa) == 0)
               {
                   printvisa = "no";
               }
               byte[] qrc = null;
               string discountid = "", discountname = "", discountnameind = "", discountamountind = "";
               try
               {
                   if (DiscountIndividual.Sum(x => x.discountAMount) > 0)
                   {
                       discountnameind = "Other Discounts";
                       discountamountind = DiscountIndividual.Sum(x => x.discountAMount).ToString();
                   }
               }
               catch (Exception ex)
               {

               }
               string posfee = "";
               string q = "select token,CONVERT(VARCHAR(10), CAST(time AS TIME), 0) as time,discountkeyid,posfee from sale where id=" + sid;
               try
               {
                   DataSet dstoken = new DataSet();
                   dstoken = objcore.funGetDataSet(q);
                   if (dstoken.Tables[0].Rows.Count > 0)
                   {
                       token = dstoken.Tables[0].Rows[0][0].ToString().Trim();
                       time = dstoken.Tables[0].Rows[0][1].ToString().Trim();
                       discountid = dstoken.Tables[0].Rows[0][2].ToString().Trim();
                       if (discountid.Length > 0)
                       {
                           discountname = Discountkeys.Where(x => x.id == discountid).ToList()[0].name;
                       }
                       posfee = dstoken.Tables[0].Rows[0]["posfee"].ToString().Trim();
                   }
               }
               catch (Exception ex)
               {

               }
               if (posfee == "0")
               {
                   posfee = "";
               }
               try
               {
                   if (token == "")
                   {
                       Random rnd = new Random();
                       token = "";
                       for (int i = 0; i < 9; i++)
                       {
                           token = token + rnd.Next(1, 9);
                       }
                       q = "update sale set token='" + token + "'  where id=" + sid;
                       objcore.executeQuery(q);
                   }
               }
               catch (Exception ex)
               {

               }
               try
               {
                   if (token.Length > 0)
                   {
                       qrtitle = "Sufi Rewards QR Code" + System.Environment.NewLine + "Download Sufi Rewards App from APP/Play Store" + System.Environment.NewLine + "Register yourself and scan through app" + System.Environment.NewLine + "Earn points and Redeem Products";
                       string url = pointsurl + "/verifypoints.asmx/Getresponse?id=" + sid + "&token=" + token;
                       Zen.Barcode.CodeQrBarcodeDraw qr = Zen.Barcode.BarcodeDrawFactory.CodeQr;
                       Image img = qr.Draw(url, 30);
                       //pictureBox1.Image = img;

                       qrc = (byte[])(new ImageConverter()).ConvertTo(img, typeof(byte[]));
                   }
               }
               catch (Exception ex)
               {

               }

               DataSet dsmenu = new DataSet();
               try
               {
                   if (CacheClass.Cache["menuitem"] != null)
                   {
                       dsmenu = (DataSet)CacheClass.Cache["menuitem"];

                   }
                   else
                   {
                       _frm.getmenuitemname();
                       dsmenu = (DataSet)CacheClass.Cache["menuitem"];
                   }
               }
               catch (Exception)
               {


               }
               double subtot = 0;
               DataTable dt = new DataTable();
               dt.Columns.Add("name", typeof(string));
               dt.Columns.Add("qty", typeof(double));
               dt.Columns.Add("Price", typeof(double));
               dt.Columns.Add("logo", typeof(byte[]));
               dt.Columns.Add("qr", typeof(byte[]));
               dt.Columns.Add("sprice", typeof(double));
               //DataSet dsd = new DataSet();
               string pricemethod = _frm.pricemethod;
               string showgross = _frm.ShowGrossPrice;
               //dsd = objcore.funGetDataSet(q);
               for (int i = 0; i < dtprint.Rows.Count; i++)
               {
                   string name22 = "";
                   if (name2 == "Enabled")
                   {
                       try
                       {
                           int flvrid = 0, runtimeid = 0, modifierid = 0, itemid = 0;
                           string tem = dtprint.Rows[i]["Id"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           itemid = Convert.ToInt32(tem);
                           tem = dtprint.Rows[i]["runtimeflavourid"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           runtimeid = Convert.ToInt32(tem);
                           tem = dtprint.Rows[i]["MdId"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           modifierid = Convert.ToInt32(tem);
                           tem = dtprint.Rows[i]["flavourid"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           flvrid = Convert.ToInt32(tem);
                           if (runtimeid > 0)
                           {
                               name22 = (_frm.runtimeclass.Where(x => x.Id == runtimeid).ToList().FirstOrDefault().Name2);

                           }
                           else if (modifierid > 0)
                           {
                               name22 = (_frm.modifierclass.Where(x => x.Id == modifierid).ToList().FirstOrDefault().Name2);

                           }
                           else if (flvrid > 0)
                           {


                               name22 = (_frm.flavourclass.Where(x => x.Id == flvrid).ToList().FirstOrDefault().Name2);
                               DataRow[] drr = dsmenu.Tables[0].Select("id=" + itemid);
                               name22 =name22+ drr[0]["Name2"].ToString();

                           }
                           else
                           {
                               DataRow[] drr = dsmenu.Tables[0].Select("id=" + itemid);
                               name22 = drr[0]["Name2"].ToString();

                           }
                       }
                       catch (Exception ex)
                       {
                           
                       }
                       name22 = "\n" + name22;
                   }
                   string tempp = dtprint.Rows[i]["Qty"].ToString();
                   if (tempp == "")
                   {
                       tempp = "1";
                   }
                   float qty = float.Parse(tempp);
                   double price = 0;
                   tempp = dtprint.Rows[i]["Price"].ToString();
                   if (tempp == "")
                   {
                       tempp = "0";
                   }
                   price = float.Parse(tempp);

                   if (showgross == "Enabled")
                   {
                       string temp = dtprint.Rows[i]["Amount"].ToString();
                       if (tempp == "")
                       {
                           tempp = "0";
                       }
                       price = float.Parse(temp);
                   }
                   if (pricemethod.ToLower() == "gross")
                   {
                       //price = dtprint.Rows[i]["Amount"].ToString();

                       try
                       {
                           int flvrid = 0, runtimeid = 0, modifierid = 0, itemid = 0;
                           string tem = dtprint.Rows[i]["Id"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           itemid = Convert.ToInt32(tem);
                           tem = dtprint.Rows[i]["runtimeflavourid"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           runtimeid = Convert.ToInt32(tem);
                           tem = dtprint.Rows[i]["MdId"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           modifierid = Convert.ToInt32(tem);
                           tem = dtprint.Rows[i]["flavourid"].ToString();
                           if (tem == "")
                           {
                               tem = "0";
                           }
                           flvrid = Convert.ToInt32(tem);
                           if (runtimeid > 0)
                           {
                               float prc = float.Parse(_frm.runtimeclass.Where(x => x.Id == runtimeid).ToList().FirstOrDefault().Price);
                               float g = float.Parse(gst) + 100;
                               g = g / 100;
                               prc = prc / g;
                               price = prc * qty;
                           }
                           else if (modifierid > 0)
                           {
                               float prc = float.Parse(_frm.modifierclass.Where(x => x.Id == modifierid).ToList().FirstOrDefault().Price);
                               float g = float.Parse(gst) + 100;
                               g = g / 100;
                               prc = prc / g;
                               price = prc * qty;
                           }
                           else if (flvrid > 0)
                           {
                               DataRow[] drr = dsmenu.Tables[0].Select("id=" + itemid);
                               string temp = drr[0]["GrossPrice"].ToString();
                               if (temp == "")
                               {
                                   temp = "0";
                               }
                               float prc = float.Parse(temp);

                               prc = prc + float.Parse(_frm.flavourclass.Where(x => x.Id == flvrid).ToList().FirstOrDefault().Price);
                               float g = float.Parse(gst) + 100;
                               g = g / 100;
                               prc = prc / g;

                               price = prc * qty;
                           }
                           else
                           {
                               DataRow[] drr = dsmenu.Tables[0].Select("id=" + itemid);
                               string temp = drr[0]["GrossPrice"].ToString();
                               if (temp == "")
                               {
                                   temp = "0";
                               }
                               float prc = float.Parse(temp);
                               float g = float.Parse(gst) + 100;
                               g = g / 100;
                               prc = prc / g;

                               price = prc * qty;
                           }
                       }
                       catch (Exception exxx)
                       {


                       }


                   }
                   price = Math.Round(price, 2);

                   double sprice = Math.Round(Convert.ToDouble(price) / Convert.ToDouble(qty), 2);

                   dt.Rows.Add(dtprint.Rows[i]["Item"].ToString()+name22, qty, price, code, qrc, sprice);
                   subtot = subtot + Convert.ToDouble(price);
               }
               subtot = Math.Round(subtot, 2);
               total = subtot.ToString();
               string tax = "", service = "", taxcard = "", servicecard = "", servicecash = "";
               string gsttitle = _frm.gsttitlevisa;
               if (_frm.applydiscount() == "before")
               {

                   servicechargescash = Math.Round((servicecharhes * (Convert.ToDouble(total))) / 100, 2);
                   servicechargescard = Math.Round((servicecharhes * (Convert.ToDouble(total))) / 100, 2);
               }
               else
               {
                   string tempdis = discountamount;
                   if (tempdis == "")
                   {
                       tempdis = "0";
                   }
                   servicechargescash = Math.Round((servicecharhes * (Convert.ToDouble(total) - Convert.ToDouble(tempdis))) / 100, 2);
                   servicechargescard = Math.Round((servicecharhes * (Convert.ToDouble(total) - Convert.ToDouble(tempdis))) / 100, 2);
               }

               if (servicegsttype == "" && servicetype == "")
               {
                   if (ordertype == "Take Away")
                   {
                       servicechargescash = 0;
                       servicechargescard = 0;
                   }
               }
               else
               {
                   if (servicetype.Length > 0)
                   {
                       if (ordertype == servicetype || servicetype.ToLower() == "all")
                       {
                           if (servicegsttype == "Cash" || servicegsttype.ToLower() == "all")
                           {
                           }
                           else
                           {
                               servicechargescash = 0;
                           }
                           if (servicegsttype == "Card" || servicegsttype.ToLower() == "all")
                           {
                           }
                           else
                           {
                               servicechargescard = 0;
                           }
                       }
                       else
                       {
                           servicechargescard = 0;
                           servicechargescash = 0;
                       }
                   }
               }
               if (servicecharges > 0)
               {

                   service = "\n" + servicetitle + ":  (" + servicecharhes + "%) " + servicecharges;
               }
               if (servicechargescash > 0)
               {

                   servicecash = "\n" + servicetitle + ":  (" + servicecharhes + "%) " + servicechargescash;
               }
               if (servicechargescard > 0)
               {

                   servicecard = "\n" + servicetitle + ":  (" + servicecharhes + "%) " + servicechargescard;
               }
               string title = _frm.gsttitle;
               if (title == "")
               {
                   title = "Sales Tax:      ";
               }
               if (mop.Length > 0)
               {

                   tax = title + " (" + gst + "%)   " + gstamount;
                   if (gst == "0")
                   {
                       tax = "";
                   }
               }
               else
               {
                   gstvisa = _frm.gstpercvisa;

                   if (gstvisa == "")
                   {
                       gstvisa = "0";
                   }
                   if (printvisa == "no")
                   {
                       if (_frm.checkedgsttype == "Card")
                       {
                          
                          // gst = _frm.gstpercvisa;
                           try
                           {
                               gstamount = Math.Round((subtot * Convert.ToDouble(gst) / 100), 2).ToString();
                           }
                           catch (Exception ex)
                           {
                               
                               
                           }
                       }
                       else
                       {
                           //gstamount = _frm.gstamountTotal.ToString();
                          // gst = _frm.gstperccash;
                           try
                           {
                               gstamount = Math.Round((subtot * Convert.ToDouble(gst) / 100), 2).ToString();
                           }
                           catch (Exception ex)
                           {


                           }
                       }
                       tax = title + " (" + gst + "%) " + gstamount;
                       if (gst == "0")
                       {
                           tax = "";
                       }
                   }
                   else
                   {
                       try
                       {
                           string gst1 = "0", gst2 = "0";
                           if (_frm.applydiscount() == "before")
                           {
                               gst1 = Math.Round((Convert.ToDouble(total) + servicechargescash) * Convert.ToDouble(_frm.gstperccash) / 100, 2).ToString();
                               gst2 = Math.Round((Convert.ToDouble(total) + servicechargescard) * Convert.ToDouble(_frm.gstpercvisa) / 100, 2).ToString();
                           }
                           else
                           {
                               string tempdis = discountamount;
                               if (tempdis == "")
                               {
                                   tempdis = "0";
                               }
                               gst1 = Math.Round((Convert.ToDouble(total) - Convert.ToDouble(tempdis)) * Convert.ToDouble(_frm.gstperccash) / 100, 2).ToString();
                               gst2 = Math.Round((Convert.ToDouble(total) - Convert.ToDouble(tempdis)) * Convert.ToDouble(_frm.gstpercvisa) / 100, 2).ToString();
                           }



                           if (title == "")
                           {
                               title = "Sales Tax:      ";
                           }
                           tax = title + "(" + _frm.gstperccash + "%) " + gst1;
                           //title = title + "                        " + "@" + _frm.gstperccash + "% :" + gst1.ToString();


                           taxcard = title + "(" + _frm.gstpercvisa + "%) " + gst2;
                       }
                       catch (Exception ex)
                       {

                       }
                   }
               }
              
               if (cash == "0")
               {
                   cash = "";
               }
               // dt = dtprint;
               LocalReport report = new LocalReport();

               fullpath = path.Remove(path.Length - 10) + @"\Sale\rpttest.rdlc";
               if (dscompany.Tables[0].Rows[0]["Name"].ToString().ToLower().Contains("sufi"))
               {
                   fullpath = path + @"\Sale\rpttest1.rdlc";
               }
               else
               {
                   if (printvisa == "no" || billtype == "Sale Slip" || billtype == "Duplicate Bill")
                   {

                       fullpath = path + @"\Sale\rpttest.rdlc";
                   }
                   else
                   {
                       fullpath = path + @"\Sale\rptcashvisa.rdlc";
                   }
               }
               if (qrmenu.Length > 0)
               {
                   fullpath = path + @"\Sale\rpttestmenu.rdlc";
               }

               string pathimage = "C:\\logo.jpg";
               if (_frm.multibranches == "Enabled")
               {
                   pathimage = "C:\\" + logoname;
               }
               string taxterminal = "";
               try
               {
                   taxterminal = _frm.taxTerminals.Where(x => x.Terminal.ToLower() == System.Environment.MachineName.ToString().ToLower()).ToList()[0].Terminal;
               }
               catch (Exception ex)
               {


               }

               string billtitle = billtype;
               string address = dscompany.Tables[0].Rows[0]["address"].ToString();
               string comp = dscompany.Tables[0].Rows[0]["Name"].ToString();
               string phone = dscompany.Tables[0].Rows[0]["phone"].ToString();
               if (provisionalbill == "Enabled" && taxterminal.Length == 0)
               {
                   fullpath = path + @"\Sale\rpttestprovisional.rdlc";
                   address = "";
                   billtitle = "Provisional Bill";
                   pathimage = "C:\\l.jpg";
                   comp = "";
                   phone = "";
               }


               report.ReportPath = fullpath;
               report.DataSources.Add(new ReportDataSource("dstest", dt));

               report.EnableExternalImages = true;
               report.EnableHyperlinks = true;
               if (invoiceno.Length > 0)
               {
                   sid = invoiceno;
               }

               //if (cash.Length > 0)
               //{
               //    billtitle = billtype;// "Sale Slip";
               //}
               //else
               //{

               //    billtitle = "Pre Sale Bill";
               //}

               Path.GetFullPath("C:logo.jpg");
               ReportParameter rurl = new ReportParameter("url", "");
               ReportParameter fbruri = new ReportParameter("fbrurl", "");
               ReportParameter rqrmenu = new ReportParameter("qrmenu", "");
               try
               {
                   fbruri = new ReportParameter("fbrurl", new Uri(fbrurl).AbsoluteUri);
               }
               catch (Exception ex)
               {

               }
               try
               {
                   rqrmenu = new ReportParameter("qrmenu", new Uri(qrmenu).AbsoluteUri);
               }
               catch (Exception ex)
               {

               }
               try
               {
                   rurl = new ReportParameter("url", new Uri(pathimage).AbsoluteUri);
               }
               catch (Exception ex)
               {

               }


               string param1 = "Cashier: " + cashier + "    MOP: " + mop + "\n" + "Date:" + date + " " + time + "   Order Type:" + ordertype + "\n" + "Terminal:" + System.Environment.MachineName;
               ReportParameter rcomp = new ReportParameter("comp", comp);
               ReportParameter rphone = new ReportParameter("phone", phone);
               ReportParameter raddress = new ReportParameter("address", address);
               ReportParameter rbillno = new ReportParameter("billno", sid);
               ReportParameter rcashier = new ReportParameter("cashier", param1);
               ReportParameter rmop = new ReportParameter("receipt", billtitle);
               ReportParameter rterminal = new ReportParameter("terminal", "Time:" + DateTime.Now.ToShortTimeString());
               ReportParameter rdate = new ReportParameter("date", date);
               ReportParameter rordertype = new ReportParameter("ordertype", "Order Type:" + ordertype);
               ReportParameter rDelivery = new ReportParameter("Delivery", deliveryinfo);
               ReportParameter rQrTitle = new ReportParameter("qrtitle", qrtitle);
               ReportParameter rFbrcode = new ReportParameter("fbrcode", fbrcode);


               string line = "\n-------------------------------------------------------";
               if (cash.Length == 0)
               {
                   line = "\n----------------------------------------";
               }
               string dis = "";
               if (discountamount == "0")
               {
                   discountamount = "";
               }
               if (discountamount.Length > 0)
               {
                   string tempdiscountamount = discountamount;
                   if (discountamountind.Length > 0)
                   {
                       tempdiscountamount = (Convert.ToDouble(discountamount) - Convert.ToDouble(discountamountind)).ToString();
                   }
                   if (cash.Length == 0)
                   {
                       dis = "\nDiscount: (" + discountname + "=>" + discount + "%)   " + tempdiscountamount;
                   }
                   else
                   {
                       dis = "\nDiscount: (" + discountname + "=>" + discount + "%)   " + tempdiscountamount;
                   }
                   if (discountamountind.Length > 0)
                   {
                       if (tempdiscountamount == "" || tempdiscountamount == "0")
                       {
                           dis = "\nOther Discounts:         " + discountamountind;
                       }
                       else
                       {
                           dis = dis + "\nOther Discounts:         " + discountamountind;
                       }
                   }

               }
               if (discountamount == "")
               {
                   discountamount = "0";
               }
               if (posfee.Length > 0)
               {
                   posfee = "\nDelv. Charges:           " + posfee;
               }
               string net = "", netusd = "", netcard = "", netcardusd = "";
               
               // "Amount Tendered:                                            " + nettotal;
               if (mop.Length > 0 || billtype == "Duplicate Bill" || billtype == "Sale Slip")
               {

                   net = "Net Total:                     " + Math.Round(Convert.ToDouble(nettotal), 2);
                 if (_frm.currencyrate > 1)
                 {
                     net = "Net Total:  " + _frm.currencyTitle + ":  " + Math.Round(Convert.ToDouble(nettotal), 2) + "\nNet Total:  PKR:  " + Math.Round(Convert.ToDouble(nettotal) * _frm.currencyrate, 2);
                 }
               }
               else
               {
                   if (printvisa == "no")
                   {
                       double nett = Math.Round(Math.Round(Convert.ToDouble(gstamount) + Convert.ToDouble(total) + Convert.ToDouble(servicechargescash) - Convert.ToDouble(discountamount), 2), 2);

                       net = "Net Total:                   " + Math.Round(Convert.ToDouble(nettotal), 2);
                       if (billtype == "Pre Sale Bill")
                       {
                           total = (Convert.ToDouble(total) + Convert.ToDouble(gstamount)).ToString();
                           gst = "0"; gstamount = "0";
                           tax = "";
                           nett = (Convert.ToDouble(total) + Convert.ToDouble(gstamount));

                       }
                      
                       if (_frm.currencyrate > 1)
                       {
                          // nett = Math.Round(Math.Round(Convert.ToDouble(gstamount) + Convert.ToDouble(total) + Convert.ToDouble(servicechargescash) - Convert.ToDouble(discountamount), 2), 2);

                           net = "Net Total:  " + _frm.currencyTitle + ":  " + Math.Round(Convert.ToDouble(nett), 2) + "\nNet Total:  PKR:  " + Math.Round(Convert.ToDouble(nett) * _frm.currencyrate, 2);
                       }
                      
                   }
                   else
                   {
                       try
                       {
                           string gst1 = "0", gst2 = "0";
                           if (_frm.applydiscount() == "before")
                           {
                               gst1 = Math.Round((Convert.ToDouble(total) + servicechargescash) * Convert.ToDouble(_frm.gstperccash) / 100, 2).ToString();
                               gst2 = Math.Round((Convert.ToDouble(total) + servicechargescard) * Convert.ToDouble(_frm.gstpercvisa) / 100, 2).ToString();

                           }
                           else
                           {
                               string tempdis = discountamount;
                               if (tempdis == "")
                               {
                                   tempdis = "0";
                               }
                               gst1 = Math.Round((Convert.ToDouble(total) - Convert.ToDouble(tempdis)) * Convert.ToDouble(_frm.gstperccash) / 100, 2).ToString();
                               gst2 = Math.Round((Convert.ToDouble(total) - Convert.ToDouble(tempdis)) * Convert.ToDouble(_frm.gstpercvisa) / 100, 2).ToString();
                           }
                           //string gst1 = Math.Round(Convert.ToDouble(total) * Convert.ToDouble(_frm.gstperccash) / 100, 2).ToString();
                           //string gst2 = Math.Round(Convert.ToDouble(total) * Convert.ToDouble(_frm.gstpercvisa) / 100, 2).ToString();
                           string titlet = "";
                           if (titlet == "")
                           {
                               titlet = "Net Total: ";
                           }
                           //net = titlet + Math.Round(Math.Round(Convert.ToDouble(gst1) + Convert.ToDouble(total) + Convert.ToDouble(servicechargescash) - Convert.ToDouble(discountamount), 2), 2);
                           double nett = Math.Round(Math.Round(Convert.ToDouble(gst1) + Convert.ToDouble(total) + Convert.ToDouble(servicechargescash) - Convert.ToDouble(discountamount), 2), 2);
                           double nettcard = Math.Round(Math.Round(Convert.ToDouble(gst2) + Convert.ToDouble(total) + Convert.ToDouble(servicechargescash) - Convert.ToDouble(discountamount), 2), 2);
                           try
                           {
                               nettcard = nettcard + float.Parse(posfee);
                               nett = nett + float.Parse(posfee);
                               

                           }
                           catch (Exception ex)
                           {

                           }
                           net = titlet + nett.ToString();
                           if (_frm.currencyrate > 1)
                           {
                               net = "Net:  " + _frm.currencyTitle + ":  " + nett + "\nNet:  PKR:  " + (nett * _frm.currencyrate);
                           }
                           netcard = titlet + nettcard;
                           if (_frm.currencyrate > 1)
                           {
                               netcard = "Net:  " + _frm.currencyTitle + ":  " + nettcard + "\nNet:  PKR:  " + (nettcard * _frm.currencyrate);
                           }
                           
                           
                       }
                       catch (Exception ex)
                       {

                       }
                   }

               }
               string charity = "";
               try
               {
                   if (billtype == "Duplicate Bill" || billtype == "Sale Slip")
                   {
                       double charityamount = 0;
                       q = "Select * from Charity";
                       DataSet dscharity = new DataSet();
                       dscharity = objcore.funGetDataSet(q);
                       if (dscharity.Tables[0].Rows.Count > 0)
                       {
                           charityamount = float.Parse(dscharity.Tables[0].Rows[0]["Perc"].ToString());
                           charity = dscharity.Tables[0].Rows[0]["Text"].ToString();
                           charityamount = Math.Round((float.Parse(nettotal) * charityamount / 100), 2);
                           charity = charity.Replace("{Amount}", charityamount.ToString());
                       }
                   }
               }
               catch (Exception ex)
               {

               }
               try
               {
                   cash = Math.Round(Convert.ToDouble(cash), 0).ToString();
               }
               catch (Exception ex)
               {
               }
               try
               {
                   change = Math.Round(Convert.ToDouble(change), 0).ToString();
               }
               catch (Exception ex)
               {
               }
               string cashgiven = "";
               if (provisionalbill == "Enabled" && taxterminal.Length== 0)
               {
               }
               else
               {
                   if (cash.Length > 0 && mop.ToLower().Contains("cash"))
                   {
                       cashgiven = line + "\n" + "Cash Received:                            " + cash + "\n";
                       cashgiven = cashgiven + "Change Given:                               " + change + "\n";
                   }
               }
               string space = "";
               if (cash.Length > 0 || billtype == "Duplicate Bill" || billtype == "Sale Slip")
               {
                   space = "                                                     ";
               }
               else
               {
                   if (printvisa == "no")
                   {
                   }
                   else
                   {
                       if (servicetype.ToLower().Trim() == "all" && servicegsttype.ToLower().Trim() == "all")
                       {

                       }
                       else
                       {
                           service = servicecash;
                       }
                   }
               }
               string subtotal = "Sub Total:   " + total + "\n" + tax + service + dis + posfee;

               string subtotalcard = "Sub Total:   " + total + "\n" + taxcard + servicecard + dis + posfee;
               if (showgross == "Enabled")
               {
                   subtotal = tax + service + dis + posfee;
                   subtotalcard = taxcard + servicecard + dis + posfee;
               }
               if (subtotal == "")
               {
                   subtotal = ".";

               }
               if (subtotalcard == "")
               {
                   subtotalcard = ".";

               }
               if (_frm.currencyrate > 1)
               {
                   subtotal=subtotal+"\n" + _frm.currencyTitle+" Conv. Rate:"+_frm.currencyrate;
                   subtotalcard = subtotalcard + "\n" + _frm.currencyTitle + " Conv. Rate:" + _frm.currencyrate;
               }
               string tender = net + cashgiven;
               string tendercard = netcard + cashgiven;
               ReportParameter rsubtotal = new ReportParameter("subtotal", subtotal);
               ReportParameter rsubtotalcard = new ReportParameter("subtotalcard", subtotalcard);
               ReportParameter rtender = new ReportParameter("tender", tender);
               ReportParameter rtendercard = new ReportParameter("tendercard", tendercard);
               ReportParameter rcharity = new ReportParameter("charity", charity);
               string footer = ""; 
               
              if (provisionalbill == "Enabled" && taxterminal.Length== 0)
              {
              }
              else
              {
                  try
                  {
                      footer = dscompany.Tables[0].Rows[0]["WellComeNote"].ToString() + "\nPrint Time:" + DateTime.Now.ToString();

                      footer = footer + "\n====================\n" + "Software Provided By Far Tech\nhttps://fartechpk.com\n====================";
                      if (qrcode.Length > 0)
                      {
                          footer = footer + "\n" + "Your Sale Tax Invoice Number is\n" + qrcode;
                      }
                  }
                  catch (Exception ex)
                  {

                  }
              }
               //else
               //{
               //    footer = "Print Time:" + DateTime.Now.ToString();
               //}
               ReportParameter rfooter = new ReportParameter("footer", footer);
               report.SetParameters(new ReportParameter[] { rurl, rcomp, rphone, raddress, rbillno, rcashier, rmop, rterminal, rdate, rordertype, rDelivery, rsubtotal, rfooter, rtender, rQrTitle, rFbrcode, rsubtotalcard, rtendercard, rcharity, fbruri,rqrmenu });
               var pageSettings = new PageSettings();
               pageSettings.PaperSize = report.GetDefaultPageSettings().PaperSize;
               pageSettings.Landscape = report.GetDefaultPageSettings().IsLandscape;
               pageSettings.Margins = report.GetDefaultPageSettings().Margins;

               if (type == "Preview")
               {
                   frmprintpreview obj = new frmprintpreview();
                   obj.rurl = rurl;
                   obj.dt = dt;
                   obj.report = report;
                   obj.rcomp = rcomp;
                   obj.rphone = rphone;
                   obj.raddress = raddress;
                   obj.rbillno = rbillno;
                   obj.rcashier = rcashier;
                   obj.rmop = rmop;
                   obj.rterminal = rterminal;
                   obj.rdate = rdate;
                   obj.rordertype = rordertype;
                   obj.rDelivery = rDelivery;
                   obj.rsubtotal = rsubtotal;
                   obj.rfooter = rfooter;
                   obj.rtender = rtender;
                   obj.rQrTitle = rQrTitle;
                   obj.rFbrcode = rFbrcode;
                   obj.rsubtotalcard = rsubtotalcard;
                   obj.rtendercard = rtendercard;
                   obj.rcharity = rcharity;
                   obj.fbruri = fbruri;
                   obj.rqrmenu = rqrmenu;
                   obj.Show();
               }
               else
               {
                   for (int i = 0; i < prints; i++)
                   {
                       Printr(report, pageSettings);
                   }
               }

           }
           catch (Exception ex)
           {

               MessageBox.Show(fullpath + "\n" + ex.Message + "\n" + ex.InnerException.Message + "\n" + ex.Message);
           }
       }
    
       public static void Printr(LocalReport report, PageSettings pageSettings)
       {
           try
           {
               string deviceInfo = @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>{pageSettings.PaperSize.Width * 100}in</PageWidth>
                <PageHeight>{pageSettings.PaperSize.Height * 100}in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>{pageSettings.Margins.Left * 100}in</MarginLeft>
                <MarginRight>{pageSettings.Margins.Right * 100}in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";

              

               Warning[] warnings;
               var streams = new List<Stream>();
               var currentPageIndex = 0;

               report.Render("Image", deviceInfo,
                   (name, fileNameExtension, encoding, mimeType, willSeek) =>
                   {
                       var stream = new MemoryStream();
                       streams.Add(stream);
                       return stream;
                   }, out warnings);

               foreach (Stream stream in streams)
                   stream.Position = 0;

               if (streams == null || streams.Count == 0)
                   throw new Exception("Error: no stream to print.");

               var printDocument = new PrintDocument();

               printDocument.DefaultPageSettings = pageSettings;

               if (!printDocument.PrinterSettings.IsValid)
                   throw new Exception("Error: cannot find the default printer.");
               else
               {
                   printDocument.PrintPage += (sender, e) =>
                   {
                       Metafile pageImage = new Metafile(streams[currentPageIndex]);
                       Rectangle adjustedRect = new Rectangle(
                           e.PageBounds.Left - (int)e.PageSettings.HardMarginX,
                           e.PageBounds.Top - (int)e.PageSettings.HardMarginY,
                           e.PageBounds.Width,
                           e.PageBounds.Height);
                       // e.Graphics.FillRectangle(Brushes.White, adjustedRect);
                       e.Graphics.DrawImage(pageImage, adjustedRect);
                       currentPageIndex++;
                       e.HasMorePages = (currentPageIndex < streams.Count);
                       //e.Graphics.DrawRectangle(Pens.Red, adjustedRect);
                   };
                   printDocument.EndPrint += (Sender, e) =>
                   {
                       if (streams != null)
                       {
                           foreach (Stream stream in streams)
                               stream.Close();
                           streams = null;
                       }
                   };
                   currentPageIndex = 0;
                   printDocument.Print();
               }
           }
           catch (Exception ex)
           {
           }
       }
    }
}
