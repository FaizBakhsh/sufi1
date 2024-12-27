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
       public static void Printt(string path, DataTable dtprint, string mop, string sid, string cardno, string ordertype, string total, string nettotal, string discount, string gst, string cash, string change, string printername, string deliveryinfo, int prints, string discountamount, string gstamount, string msg1, string msg2, string serice,string cashier,string date,string delivery,RestSale _frm,string qrcode,string pointsurl,string invoiceno,string billtype)
       {
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
           string token = "", qrtitle = "", fbrcode = "";
           string fullpath = "";
           try
           {
               
               byte[] code = null;

               try
               {
                  // qrcode =  "11222";
                   if (qrcode.Length > 0)
                   {
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
               string printvisa = "yes";
              
               DataSet dscompany = new DataSet();
               dscompany = objcore.funGetDataSet("select * from companyinfo");
               try
               {
                   if (dscompany.Tables[0].Rows.Count > 0)
                   {
                       printvisa = dscompany.Tables[0].Rows[0]["printvisa"].ToString();
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
               string discountid = "",discountname="",discountnameind="",discountamountind="";
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
               string q = "select token,CONVERT(VARCHAR(10), CAST(time AS TIME), 0) as time,discountkeyid from sale where id=" + sid;
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
                   }
               }
               catch (Exception ex)
               {
                  
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
              /* try
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
                   
               }*/
              
               DataTable dt = new DataTable();
               dt.Columns.Add("name", typeof(string));
               dt.Columns.Add("qty", typeof(double));
               dt.Columns.Add("Price", typeof(double));
               dt.Columns.Add("logo", typeof(byte[]));
               dt.Columns.Add("qr", typeof(byte[]));
               dt.Columns.Add("sprice", typeof(double));
               //DataSet dsd = new DataSet();

               if (billtype == "Pre Sale Bill")
               {
                   for (int i = 0; i < dtprint.Rows.Count; i++)
                   {
                       q = "select * from Saledetails where saleid=" + sid;
                       try
                       {
                           DataSet dsSaleDetail = new DataSet();
                           dsSaleDetail = objcore.funGetDataSet(q);
                           if (dsSaleDetail.Tables[0].Rows.Count > 0)
                           {
                               for (int k = 0; k < dsSaleDetail.Tables[0].Rows.Count; k++)
                               {
                                   //q = "select * from MenuItem where id=" + dsSaleDetail.Tables[0].Rows[k][2].ToString().Trim(); 
                                   //DataSet dsMenue = new DataSet();
                                  // dsMenue = objcore.funGetDataSet(q);

                                   string price = dsSaleDetail.Tables[0].Rows[k]["Price"].ToString();
                                   string Itemgst = dsSaleDetail.Tables[0].Rows[k]["ItemGst"].ToString();
                                   if (price == "")
                                   {
                                       price = "0";
                                   }
                                   string qty = dtprint.Rows[k]["Qty"].ToString();
                                   if (qty == "")
                                   {
                                       qty = "1";
                                   }
                                   //double sprice = Math.Round(Convert.ToDouble(price)+ Convert.ToDouble(Itemgst), 2);

                                   //dt.Rows.Add(dtprint.Rows[k]["Item"].ToString(), qty, sprice, code, qrc, sprice);
                                   double sprice = Math.Round(Convert.ToDouble(price) / Convert.ToDouble(qty), 2);

                                   dt.Rows.Add(dtprint.Rows[i]["Item"].ToString(), qty, price, code, qrc, sprice);
                               }
                               
                           }
                           break;
                          
                       }
                       catch (Exception ex)
                       {

                       }
                       
                   }
               
               }
               else
               {

                   //dsd = objcore.funGetDataSet(q);
                   for (int i = 0; i < dtprint.Rows.Count; i++)
                   {
                       string price = dtprint.Rows[i]["Price"].ToString();
                       if (price == "")
                       {
                           price = "0";
                       }
                       string qty = dtprint.Rows[i]["Qty"].ToString();
                       if (qty == "")
                       {
                           qty = "1";
                       }
                       double sprice = Math.Round(Convert.ToDouble(price) / Convert.ToDouble(qty), 2);

                       dt.Rows.Add(dtprint.Rows[i]["Item"].ToString(), qty, price, code, qrc, sprice);
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
                  // fullpath = path + @"\Sale\rpttest1.rdlc";
                   if (printvisa == "no" || billtype == "Sale Slip" || billtype == "Duplicate Bill")
                   {

                       fullpath = path + @"\Sale\rpttest.rdlc";
                   }
                   else
                   {
                       fullpath = path + @"\Sale\rptcashvisa.rdlc";
                   }
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
               report.ReportPath = fullpath;
               report.DataSources.Add(new ReportDataSource("dstest", dt));

               report.EnableExternalImages = true;
               report.EnableHyperlinks = true;
               if (invoiceno.Length > 0)
               {
                   sid = invoiceno;
               }
               string billtitle = billtype;


              





               //if (cash.Length > 0)
               //{
               //    billtitle = billtype;// "Sale Slip";
               //}
               //else
               //{
                   
               //    billtitle = "Pre Sale Bill";
               //}
               string pathimage = "C:\\logo.jpg"; 
               Path.GetFullPath("C:logo.jpg");
               ReportParameter rurl = new ReportParameter("url", new Uri(pathimage).AbsoluteUri);
               string param1 = "Cashier: " + cashier + "     MOP: " + mop + "\n" + "Date:" + date+ " "+time +"\n Order Type:" + ordertype + "\n" + "Terminal:" + System.Environment.MachineName;
               ReportParameter rcomp = new ReportParameter("comp", dscompany.Tables[0].Rows[0]["Name"].ToString());
               ReportParameter rphone = new ReportParameter("phone", dscompany.Tables[0].Rows[0]["phone"].ToString());
               ReportParameter raddress = new ReportParameter("address", dscompany.Tables[0].Rows[0]["address"].ToString());
               ReportParameter rbillno = new ReportParameter("billno", sid);
               ReportParameter rcashier = new ReportParameter("cashier", param1);
               ReportParameter rmop = new ReportParameter("receipt", billtitle);
               ReportParameter rterminal = new ReportParameter("terminal", "Time:" + DateTime.Now.ToShortTimeString());
               ReportParameter rdate = new ReportParameter("date", date);
               ReportParameter rordertype = new ReportParameter("ordertype", "Order Type:" + ordertype);
               ReportParameter rDelivery = new ReportParameter("Delivery", deliveryinfo);
               ReportParameter rQrTitle = new ReportParameter("qrtitle", qrtitle);
               ReportParameter rFbrcode = new ReportParameter("fbrcode", fbrcode);

             
               string tax = "", service = "", taxcard = "", servicecard = "", servicecash = "";
               string gsttitle = _frm.gsttitlevisa;
               if (_frm.applydiscount() == "before")
               {

                   servicechargescash = Math.Round((servicecharhes * (Convert.ToDouble(total))) / 100,2);
                   servicechargescard = Math.Round((servicecharhes * (Convert.ToDouble(total))) / 100, 2);
               }
               else
               {
                   string tempdis = discountamount;
                   if (tempdis == "")
                   {
                       tempdis = "0";
                   }
                   servicechargescash =Math.Round( (servicecharhes * (Convert.ToDouble(total) - Convert.ToDouble(tempdis))) / 100,2);
                   servicechargescard = Math.Round((servicecharhes * (Convert.ToDouble(total) - Convert.ToDouble(tempdis))) / 100,2);
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

                   service = "\n" + servicetitle + ":  (" + servicecharhes+"%) " + servicecharges;
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

                   tax = title+" (" + gst + "%)   " + gstamount;
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
                   if (printvisa=="no")
                   {

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
                               if(tempdis=="")
                               {
                                   tempdis="0";
                               }
                               gst1 = Math.Round((Convert.ToDouble(total) - Convert.ToDouble(tempdis)) * Convert.ToDouble(_frm.gstperccash) / 100, 2).ToString();
                               gst2 = Math.Round((Convert.ToDouble(total) - Convert.ToDouble(tempdis)) * Convert.ToDouble(_frm.gstpercvisa) / 100, 2).ToString();
                           }

                           
                          
                           if (title == "")
                           {
                               title = "Sales Tax:      ";
                           }
                           tax = title + "("+_frm.gstperccash + "%) " + gst1;
                           //title = title + "                        " + "@" + _frm.gstperccash + "% :" + gst1.ToString();


                           taxcard = title + "(" + _frm.gstpercvisa + "%) " + gst2;
                       }
                       catch (Exception ex)
                       {

                       }
                   }
               }
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
                           dis ="\nOther Discounts:         " + discountamountind;
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
               string net = "", netcard = "";// "Amount Tendered:                                     

               double DelvAmt = 0;
               DataSet dsDelv = new DataSet();



               try
               {
                   dsDelv = new DataSet();
                   string qs = "select deliveryAmt  from Sale where Id= " + sid;
                   dsDelv = objcore.funGetDataSet(qs);


                   if (dsDelv.Tables[0].Rows.Count > 0)
                   {
                       DelvAmt = Convert.ToDouble(dsDelv.Tables[0].Rows[0]["deliveryAmt"]);
                       if (DelvAmt > 0)
                       {
                           double Total = Convert.ToDouble(nettotal);
                           nettotal = (Total + DelvAmt).ToString();
                   
                            if (billtype != "Pre Sale Bill")
                           {
                               nettotal = (Total + DelvAmt).ToString();
                           }
                           else
                           {

                               if (DelvAmt > 0)
                               {
                                   double TTotal = Convert.ToDouble(total);
                                   double nTotal = Convert.ToDouble(nettotal);
                                   total = (TTotal + DelvAmt).ToString();
                                   net = (Convert.ToDouble(nettotal) + DelvAmt).ToString();
                               }

                           }
                       }
                   }
               }
               catch (Exception ex)
               {

               }

               if (mop.Length > 0 || billtype=="Duplicate Bill" || billtype=="Sale Slip")
               {

                   net = "Amount Tendered:                            " + Math.Round(Convert.ToDouble( nettotal),2);
               }
               else
               {
                   if (printvisa == "no")
                   {
                       net = "Amount Tendered:                        " + Math.Round(Convert.ToDouble(nettotal), 2);
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
                               titlet = "Amount Tendered: ";
                           }



                           net = titlet + Math.Round(Math.Round(Convert.ToDouble(gst1) + Convert.ToDouble(total) + Convert.ToDouble(servicechargescash) - Convert.ToDouble(discountamount), 2), 0);
                           netcard = titlet + Math.Round(Convert.ToDouble(gst2) + Convert.ToDouble(total) + Convert.ToDouble(servicechargescard) - Convert.ToDouble(discountamount), 2);

                       }
                       catch (Exception ex)
                       {

                       }
                   }
                   
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
               if (cash.Length > 0 && mop.ToLower().Contains("cash"))
               {
                   
                   cashgiven = line + "\n" + "Cash Received:                            " + cash + "\n";
                   cashgiven = cashgiven + "Change Given:                               " + change + "\n";

                   if (DelvAmt > 0)
                   {
                       //string Delv = "";
                       string Cash = "";
                       Cash = "\n" + "Delivery Charges:                            " + DelvAmt + "\n" + net;
                       net = Cash;
                   }
               }
               string space = "";
               if (cash.Length > 0 || billtype == "Duplicate Bill" || billtype == "Sale Slip")
               {
                   space = "                                                     ";
               }
               else
               {
                   service = servicecash;
               }

               string subtotal = "";

               string subtotalcard = "";
               string tender = "";
               string tendercard = "";
               int POSFee = 0;
               DataSet dsPosFee = new DataSet();
               string y = "SELECT Id, Name, Amount FROM POSFee";
               dsPosFee = objcore.funGetDataSet(y);
               if (dsPosFee.Tables[0].Rows.Count > 0)
               {
                   POSFee = Convert.ToInt32(dsPosFee.Tables[0].Rows[0][2]);

               }
               if (POSFee > 0)
               {
                   nettotal  = (Convert.ToDouble(POSFee) + Convert.ToDouble(nettotal)).ToString();
               }

              
               






               if (billtype == "Pre Sale Bill")
               {

                   if (DelvAmt > 0)
                   {
                       //string Delv = "";
                       string Cash = "";
                       Cash = "\n" + "Delivery Charges:                            " + DelvAmt + "\n" + net;
                       net = Cash;
                   }
                  // decimal GTotal = Convert.ToDecimal(total) + Convert.ToDecimal(gstamount);
                 //  decimal GTotal = Convert.ToDecimal(total) 
                   //total = GTotal.ToString();
                 //  gsttitle = "";
        //           subtotal = "Sub Total:   " + total + "\n\n"  + service + dis;
                    subtotal = "Sub Total:   " + total + "\n\n" + tax + service + dis;
                    subtotalcard = "Sub Total:   " + total + "\n\n"+ taxcard + servicecard + dis;
                    tender = net + cashgiven;
                    tendercard = netcard + cashgiven;
                    
                    
               }
               else {

                   if (POSFee > 0)
                   {
                       subtotal = "Sub Total:   " + total + "\n" + tax + service + dis + "\n POS Fees: "+ POSFee.ToString();
                       subtotalcard = "Sub Total:   " + total + "\n" + taxcard + servicecard + dis + "\n POS Fees: " + POSFee.ToString();
                       tender = net + cashgiven;
                       tendercard = netcard + cashgiven;
                      
                   }
                   else
                   {
                       subtotal = "Sub Total:   " + total + "\n\n" + tax + service + dis;
                       subtotalcard = "Sub Total:   " + total + "\n\n" + taxcard + servicecard + dis;
                       tender = net + cashgiven;
                       tendercard = netcard + cashgiven;
                   }

                  
               
               }
               //string subtotal = "Sub Total:   " +  total + "\n" + tax + service + dis;

              // string subtotalcard = "Sub Total:   " + total + "\n" + taxcard + servicecard + dis; 
              // string tender= net + cashgiven;
              // string tendercard = netcard + cashgiven;
               ReportParameter rsubtotal = new ReportParameter("subtotal", subtotal);
               ReportParameter rsubtotalcard = new ReportParameter("subtotalcard", subtotalcard);
               ReportParameter rtender = new ReportParameter("tender", tender);
               ReportParameter rtendercard = new ReportParameter("tendercard", tendercard);
               string footer = "";
              // if (billtype == "Duplicate Bill" || billtype == "Sale Slip")
               {
                   try
                   {
                       footer = dscompany.Tables[0].Rows[0]["WellComeNote"].ToString() + "\nPrint Time:" + DateTime.Now.ToString();
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
               report.SetParameters(new ReportParameter[] { rurl, rcomp, rphone, raddress, rbillno, rcashier, rmop, rterminal, rdate, rordertype, rDelivery, rsubtotal, rfooter, rtender, rQrTitle, rFbrcode, rsubtotalcard, rtendercard });
               var pageSettings = new PageSettings();
               pageSettings.PaperSize = report.GetDefaultPageSettings().PaperSize;
               pageSettings.Landscape = report.GetDefaultPageSettings().IsLandscape;
               pageSettings.Margins = report.GetDefaultPageSettings().Margins;

           

               for (int i = 0; i < prints; i++)
               {
                   Printr(report, pageSettings);
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

               //            string deviceInfo = @"<DeviceInfo>
               //                <OutputFormat>EMF</OutputFormat>
               //                <PageWidth>3in</PageWidth>
               //               <PageHeight>3in</PageHeight>
               //                <MarginTop>0in</MarginTop>
               //                <MarginLeft>0in</MarginLeft>
               //                <MarginRight>0in</MarginRight>
               //                <MarginBottom>0in</MarginBottom>
               //            </DeviceInfo>";

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
