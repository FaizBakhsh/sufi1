using POSRestaurant.Sale;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace POSRestaurant.classes
{
    class printgeneric
    {
       public static string date = "";
        public static string Print(string printerName, string sid, string cashier, string cusid, string mop, string delivery, DataTable dtr, string r, string c, string total, string dis, string gst, string type, string ordertype, string dat)
        { 
            date = dat;
            POSRestaurant.Sale.NativeMethods.DOC_INFO_1 documentInfo;
            IntPtr printerHandle;
            byte[] managedData = null;
            string addrs = "";
            if (ordertype == "Delivery")
            {
                addrs = "Address:\n";
                //addrs = addrs + getaddress(sid);
            }
           // managedData = GetDocument(sid, cashier, cusid, mop, addrs, dtr, r, c, total, dis, gst, type, ordertype);
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
            return "";
        }
        public byte[] GetDocument(string sid, string cashier, string cusid, string mop, string delivery, DataTable dtr, string r, string c, string total, string dis, string gst, string type, string ordertype)
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
                //if (type == "kot")
                //{
                //    PrintReceiptkitchen(bw, sid, cashier, cusid, mop, delivery, dtr, r, c, total, dis, gst);
                //}
                //else
                {
                    PrintReceipt(bw, sid, cashier, cusid, mop, delivery, dtr, r, c, total, dis, gst, ordertype);
                }
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
        POSRestaurant.classes.Clsdbcon objCore = new Clsdbcon();
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public string gettime(string id)
        {
            string time = "";

            try
            {
                string q = "select time from sale where id='" + id + "'";
                DataSet dstime = new DataSet();
                dstime = objCore.funGetDataSet(q);
                if (dstime.Tables[0].Rows.Count > 0)
                {
                    time = Convert.ToDateTime(dstime.Tables[0].Rows[0][0].ToString()).ToString("HH:mm tt");
                }
            }
            catch (Exception ex)
            {
                
            }
            return time;
        }
        public int getlinelength(string type, string p)
        {
            int length = 0;
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
                        temp = "37";
                    }
                    length = Convert.ToInt32(temp);
                }
                else
                {
                    length = 37;
                }
            }
            catch (Exception ex)
            {
                
            }
            return length;
        }
        private void PrintReceipt(BinaryWriter bw, string sid, string cashier, string cusid, string mop, string delivery, DataTable dtr, string r, string c, string total, string dis, string gst, string ordertype)
        {
            getcompany();
            string time = "";
            time = " " + gettime(sid);
            int length = getlinelength("name", "receipt");
            string namee = dscompany.Tables[0].Rows[0]["Name"].ToString();
            namee = namee.PadLeft(length);
            length = getlinelength("address", "receipt");
            string addrs = dscompany.Tables[0].Rows[0]["Address"].ToString();
            addrs = addrs.PadLeft(length);
            string phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
            length = getlinelength("phone", "receipt");
            phone = phone.PadLeft(length);
            length = getlinelength("title", "receipt");
            string title = "Sale Slip".PadLeft(length);
            bw.LargeText(namee);
            bw.LargeText(addrs);
            bw.NormalFont(phone);
            //bw.FeedLines(1);
            length = getlinelength("line", "receipt");
            string print = "", space = "";
            for (int i = 0; i < length; i++)
            {
                print = print + "-";
            }
            length = getlinelength("space", "receipt");
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
            bw.NormalFont("Bill #: " + sid.ToString() + "    Cashier: " + cashier);
            bw.NormalFont("MOP: " + mop + " ,    Date: " + Convert.ToDateTime(date).ToString("dd-MM-yyy") + time);
            // bw.NormalFont("Date: " +Convert.ToDateTime(date).ToString("dd-MM-yyy")+ DateTime.Now.ToString("HH:mm tt"));
            //if (cusid.Length > 0)
            //{
            //    cusid = ", " + cusid;
            //}
            bw.NormalFont("Order Type :" + ordertype);
            bw.NormalFont(cusid);
           // bw.NormalFont("Order No: " + getorderno(sid));
            bw.NormalFont(delivery);
            //bw.FeedLines(1);
            bw.NormalFont(print);
            bw.NormalFont(itmtitle + "Qty  Price  Total");
            bw.NormalFont(print);
            foreach (DataRow dr in dtr.Rows)
            {
                try
                {
                    //if (dr["Id"].ToString() != string.Empty)
                    {
                        string pc = dr["Price"].ToString();
                        string qnty = "";
                        qnty = dr["Qty"].ToString();
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
                        bw.NormalFont(name + qtyy.ToString() + spc.ToString() + spct.ToString());//"Garlic bread           2   100     200");
                        if (subnm.Length > 0)
                        {
                            bw.NormalFont(subnm);
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
            bw.NormalFont(type + dis);
            type = "GST     :  ";
            for (int i = type.Length; i < space.Length + 1; i++)
            {
                type += " ";
            }
            bw.NormalFont(type + gst);
            bw.NormalFont(print);
            type = "Amount Tendered:  ";
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
      
    }
}
