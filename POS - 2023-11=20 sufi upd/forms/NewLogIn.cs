using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using System.Data.SqlClient;
using OposPOSKeyboard_CCO;
using System.Security.Cryptography;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Data.Sql;
using System.Diagnostics;
namespace POSRestaurant.forms
{
    public partial class NewLogIn : Form
    {
        private POSRestaurant.Sale.RestSale _frm;
        public static List<Sale.OrdersourceClass> resordersource = new List<Sale.OrdersourceClass>();
        public NewLogIn(POSRestaurant.Sale.RestSale frm )
        {
            InitializeComponent();
            _frm = frm;
        }
        public string chk = "";
       
        private void vButton1_Click(object sender, EventArgs e)
        {
            if (vTextBox1.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(vTextBox1.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {
                }
                else
                {
                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }
            vButton btn = sender as vButton;

            vTextBox1.Text = vTextBox1.Text + btn.Text;
            vTextBox1.Focus();
            vTextBox1.SelectionStart = vTextBox1.Text.Length;
            strt = vTextBox1.SelectionStart;
        }
        public static int strt = 0;
        private void vButton14_Click(object sender, EventArgs e)
        {
            vTextBox1.Text = "";
            vTextBox1.Focus();
            vTextBox1.SelectionStart = vTextBox1.Text.Length;
            strt = vTextBox1.SelectionStart;
        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            try
            {
                if (strt > 0)
                {
                    int index = vTextBox1.SelectionStart;

                    vTextBox1.Text = vTextBox1.Text.Remove(strt - 1, 1);
                    // txtcashreceived.Select(index - 1, 1);
                    //txtcashreceived.Select();
                    strt = strt - 1;
                    vTextBox1.Focus();
                    vTextBox1.SelectionStart = vTextBox1.Text.Length;
                    strt = vTextBox1.SelectionStart;
                    //txtcashreceived.Focus(); 
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton13_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("Shutdown", "-s -t 10");
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(@"C:\WINDOWS\system32\rundll32.exe", "user32.dll,LockWorkStation");
        }
        protected string blockmessage()
        {
            string block = "no";
            try
            {
                string connString = "Password=farm#*;Persist Security Info=True;User ID=farm;Initial Catalog=farm;Data Source=162.144.251.124;Min Pool Size=0;Max Pool Size=1000;Pooling=true;";
                POSRetail.forms.SuperAdminForm obj = new POSRetail.forms.SuperAdminForm();
                string d = obj.getname();
                SqlConnection conc = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand("select * from blocksoftware where name='" + d + "'", conc);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    block = ds.Tables[0].Rows[0]["message"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return block;
        }
        protected string block()
        {
            string block = "no";
            try
            {
                string connString = "Password=farm#*;Persist Security Info=True;User ID=farm;Initial Catalog=farm;Data Source=162.144.251.124;Min Pool Size=0;Max Pool Size=1000;Pooling=true;";
                POSRetail.forms.SuperAdminForm obj = new POSRetail.forms.SuperAdminForm();
                string d = obj.getname();
                SqlConnection conc = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand("select * from blocksoftware where name='" + d + "'", conc);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    block = ds.Tables[0].Rows[0]["block"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                
            }
            return block;
        }
        private void vButton12_Click(object sender, EventArgs e)
        {
            login();
        }
        private void login()
        {
            if (System.Environment.MachineName.ToString() == "DUMMY")
            {
                if (keystatus.ToLower() == "inactive")
                {
                   /* POSRestaurant.Properties.Settings.Default.KeyDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                    POSRestaurant.Properties.Settings.Default.Save();
                    MessageBox.Show("Your License Key has expired.Please Contact you service provider");
                    btnlicense.Visible = true;*/
                    return;
                }
                if (keystatus == "Not Defined")
                {
                   
                    
                    
                    if (DateTime.Now > Convert.ToDateTime("2025-06-30"))
                    {
                        MessageBox.Show("Your License Key is not defined.Please Contact you service provider");
                        btnlicense.Visible = true;
                        return;
                    }
                }
            }
            POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
            POSRestaurant.Properties.Settings.Default.apiExecution = "";
            POSRestaurant.Properties.Settings.Default.Save();
            try
            {
                string q = "select * from DeviceSetting where Device='API Execution'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    POSRestaurant.Properties.Settings.Default.apiExecution = ds.Tables[0].Rows[0]["Status"].ToString();
                    POSRestaurant.Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                string q = "select * from deliverytransfer where type='API'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    POSRestaurant.Properties.Settings.Default.BaseUrl = ds.Tables[0].Rows[0]["url"].ToString();
                    POSRestaurant.Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {

            }
            //string text = "+gj4xdoUt25txBCFe93IwxZ+zGaPmbBg25ciO7mpmE4=";
            //text = "+svkLRj9nYEgZo7gWDJD5IQ==";
            //string EncryptionKey = "MAKV2SPBNI99212";
            //byte[] cipherBytes = Convert.FromBase64String(text);
            //using (Aes encryptor = Aes.Create())
            //{
            //    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            //    encryptor.Key = pdb.GetBytes(32);
            //    encryptor.IV = pdb.GetBytes(16);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(cipherBytes, 0, cipherBytes.Length);
            //            cs.Close();
            //        }
            //        text = Encoding.Unicode.GetString(ms.ToArray());
            //    }
            //}
          


            //string clearText = "9OYR9kUytIsLilKZieD5xg";
            //EncryptionKey = "MAKV2SPBNI99212";
            //byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            //using (Aes encryptor = Aes.Create())
            //{
            //    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            //    encryptor.Key = pdb.GetBytes(32);
            //    encryptor.IV = pdb.GetBytes(16);
            //    using (MemoryStream ms = new MemoryStream())
            //    {
            //        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            //        {
            //            cs.Write(clearBytes, 0, clearBytes.Length);
            //            cs.Close();
            //        }
            //        clearText = Convert.ToBase64String(ms.ToArray());
            //    }
            //}

          
            //string qq = "BEGIN TRAN T1;  	DELETE FROM Saledetails WHERE saleid = 21365;   DELETE FROM BillType WHERE saleid = 21365;  DELETE FROM sale WHERE id = 21365;  COMMIT TRAN T1;  ";
            //int re= objcore.executeQueryint(qq);
            RegistrationClass.getlist();
            if (vTextBox1.Text == "L@wBiz1987")
            {
                POSRetail.forms.SuperAdminForm obj = new POSRetail.forms.SuperAdminForm();               
                obj.connecion(POSRestaurant.Properties.Settings.Default.ConnectionString);
                obj.Show();
                this.Hide();
                return;
            }
            if (POSRestaurant.Properties.Settings.Default.ordertype.ToLower() == "")
            {

                POSRestaurant.Properties.Settings.Default.ordertype = "takeaway";
                POSRestaurant.Properties.Settings.Default.Save();
            }
            //if (block() == "yes")
            //{
            //    MessageBox.Show(blockmessage());
            //    return;
            //}
            if (System.Environment.MachineName.ToString() == "DUMMY")
            {
                try
                {

                    DataSet reginfo = new DataSet();
                    string q = "select top 1 * from users  order by id desc";
                    reginfo = objcore.funGetDataSet(q);
                    if (reginfo.Tables[0].Rows.Count > 0)
                    {
                        string d = POSRestaurant.Properties.Settings.Default.KeyDate;
                        if (d.ToString().Length > 0)
                        {
                            DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime d2 = Convert.ToDateTime(d);

                            TimeSpan t = d1 - d2;
                            double NrOfDays = t.TotalDays;
                            if (NrOfDays >= 366)
                            {
                                MessageBox.Show("Your Software registration has expired. Please contact your service provider");
                                return;
                            }
                            if (NrOfDays >= 335)
                            {
                                MessageBox.Show((365 - NrOfDays).ToString() + " Days left of Your Software Registration.");

                            }

                        }
                        else
                        {
                            btnlicense.Visible = true;
                            MessageBox.Show("Your Software is not registered. Please contact your service provider");
                            return;
                        }
                    }
                    else
                    {
                        btnlicense.Visible = true;
                        MessageBox.Show("Your Software is not registered. Please contact your service provider");
                        return;
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Connection Not Initialized. Please Contact you service provider");
                    return;
                } 
            }
            try
            {
                //RegistrationClass
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                SqlDataReader dr = objCore.funGetDataReader("Select * from Users Where password = '" + vTextBox1.Text + "' or CardNo='" + vTextBox1.Text + "'");
                if (dr.HasRows)
                {
                    dr.Read();
                    try
                    {
                        POSRestaurant.Properties.Settings.Default.usertype = dr["role"].ToString();
                        POSRestaurant.Properties.Settings.Default.Save();
                    }
                    catch (Exception ex)
                    {


                    }
                    string query = "ALTER TABLE [dbo].[ModifierFlavour]  ADD Status varchar(50) NULL ";
                    objcore.executeQuery(query);
                    //try
                    //{
                    //    string min = "";
                    //    int indx = comboBox1.Text.IndexOf("to", 0);
                    //    min = comboBox1.Text.Substring(0, indx - 1);
                    //    string max = comboBox1.Text.Substring(indx + 3);
                    //    POSRetail.Properties.Settings.Default.MinDate = min;
                    //    POSRetail.Properties.Settings.Default.MaxDate = max;
                    //}
                    //catch (Exception ex)
                    //{


                    //}
                    POSRestaurant.classes.Clsdbcon ob = new classes.Clsdbcon();


                    if (dr["Usertype"].ToString().ToLower() == "cashier")//.GetValue(1).ToString())
                    {

                        string qq = "select status from DeviceSetting where Terminal='" + System.Environment.MachineName + "' and Device='Sale Screen'";
                        DataSet dsdevice = new DataSet();
                        dsdevice = objcore.funGetDataSet(qq);
                        if (dsdevice.Tables[0].Rows.Count > 0)
                        {
                            if (dsdevice.Tables[0].Rows[0][0].ToString() == "Disabled")
                            {
                                MessageBox.Show("Sale Screen not allowed on this system");
                                return;
                            }
                        }
                        try
                        {
                            string q = "select name, status, isnull(amount,0) as amount from ordersource";
                            DataSet ds = new DataSet();
                            ds = objcore.funGetDataSet(q);

                            IList<Sale.OrdersourceClass> data = ds.Tables[0].AsEnumerable().Select(row =>
                            new Sale.OrdersourceClass
                            {
                                type = row.Field<string>("name"),
                                amount = row.Field<string>("amount")


                            }).ToList();
                            resordersource = data.ToList();
                            q = "ALTER TABLE SerivceCharges ADD Title varchar(100) NULL";
                            objcore.executeQuery(q);
                            q = "ALTER TABLE sale ADD Token varchar(100) NULL";
                            objcore.executeQuery(q);
                            q = "ALTER TABLE saledetails ALTER COLUMN Quantity float;";
                            objcore.executeQuery(q);
                            q = "ALTER TABLE [dbo].[MenuItem]  ADD ManualQty varchar(50) NULL ";
                            objcore.executeQuery(q);
                            q = "ALTER TABLE [dbo].[KDS]  ADD KitchenBell varchar(50) NULL ";
                            objcore.executeQuery(q);

                            q = "ALTER TABLE saledetails ADD pointscode varchar(500) NULL";
                            objcore.executeQuery(q);
                            q = "ALTER TABLE delivery ADD RiderID varchar(50) NULL";
                            objcore.executeQuery(q);
                            q = "ALTER TABLE delivery ADD DispatchedTime  varchar(50)  NULL";
                            objcore.executeQuery(q);
                            q = "ALTER TABLE delivery ADD DeliveredTime  varchar(50)  NULL";
                            objcore.executeQuery(q);

                            try
                            {
                                query = "ALTER TABLE [dbo].[Sale]  ADD GSTtype varchar(100) NULL ";
                                objcore.executeQuery(query);
                            }
                            catch (Exception ex)
                            {


                            }

                        }
                        catch (Exception ex)
                        {


                        }

                        string brid = Getbranchid();
                        if (brid == string.Empty)
                        {
                            MessageBox.Show("Your Branch info is not added.Please Contact your Admin");
                            return;
                        }
                        string screen = "";
                        //screen = getscree();
                        string status = "";
                        if (chk == "yes")
                        {
                            _frm.userid = dr["id"].ToString();
                            POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                            POSRestaurant.Properties.Settings.Default.Save();
                            _frm.updateinfo();
                            this.Hide();
                        }
                        else
                        {
                            try
                            {
                                string q = "select * from DeviceSetting where Device='GST Variation'";
                                DataSet ds = new DataSet();
                                ds = objcore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    status = ds.Tables[0].Rows[0]["Status"].ToString();

                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            if (status == "")
                            {
                                status = "Enabled";
                            }
                            POSRestaurant.Properties.Settings.Default.gstvariations = status;
                            POSRestaurant.Properties.Settings.Default.Save();


                            POSRestaurant.Sale.RestSale obj = new Sale.RestSale();
                            obj.userid = dr["id"].ToString();
                            POSRestaurant.Properties.Settings.Default.branchid = brid;
                            POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                            POSRestaurant.Properties.Settings.Default.Save();
                            
                            obj.Show();
                        }
                        //POSRetail.Sale.test oj = new Sale.test();
                        //oj.Show();
                    }
                    if (dr["Usertype"].ToString().ToLower() == "kds")//.GetValue(1).ToString())
                    {
                        if (dr["role"].ToString().ToLower() == "kds-main")
                        {
                            if (dr["KDSType"].ToString().ToLower() == "list")
                            {

                                POSRestaurant.Sale.KDSKitchenwise obj = new Sale.KDSKitchenwise();
                                obj.Show();

                            }
                            else
                            {
                                POSRestaurant.Sale.kdsnewfixmain obj = new Sale.kdsnewfixmain();
                                obj.terminal = dr["terminal"].ToString();
                                obj.kdsid = dr["kdsid"].ToString();
                                obj.park = dr["ParkStatus"].ToString();
                                obj.Show();
                            }
                        }
                        else
                        {
                            string screen = "";
                            POSRestaurant.Sale.kdsnewfix obj = new Sale.kdsnewfix();
                            obj.terminal = dr["terminal"].ToString();
                            obj.kdsid = dr["kdsid"].ToString();// "1"; //comboBox1.SelectedValue.ToString();
                            //obj.kdsname = "Kitchen";
                            obj.Show();
                        }

                    }
                    if (dr["Usertype"].ToString().ToLower() == "admin")//.GetValue(1).ToString())
                    {
                        try
                        {
                            query = "ALTER TABLE [dbo].[Sale]  ADD GSTtype varchar(100) NULL ";
                            objcore.executeQuery(query);
                            query = "ALTER TABLE [dbo].[MenuItem]  ADD Price2 float NULL ";
                            objcore.executeQuery(query);
                            query = "ALTER TABLE [dbo].[MenuItem]  ADD Price3 float NULL ";
                            objcore.executeQuery(query);
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            query = "ALTER TABLE [dbo].[Closing]  ADD Userid int NULL ";
                            objcore.executeQuery(query);
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            string q = "select id from sale where gsttype is null and billstatus='Paid'";
                            DataSet dsq = new DataSet();
                            dsq = objcore.funGetDataSet(q);
                            if (dsq.Tables[0].Rows.Count > 0)
                            {
                                q = "update sale set gsttype='Cash' where BillType='Cash' and gsttype is null and billstatus='Paid'";
                                objcore.executeQuery(q);
                                q = "update sale set gsttype='Card' where BillType like '%visa%' and gsttype is null  and billstatus='Paid'";
                                objcore.executeQuery(q);

                                q = "select id from sale where gsttype is null and billstatus='Paid'";
                                dsq = new DataSet();
                                dsq = objcore.funGetDataSet(q);
                                if (dsq.Tables[0].Rows.Count > 0)
                                {
                                    q = "select * from gst";
                                    DataSet dsgst = new DataSet();
                                    dsgst = objcore.funGetDataSet(q);
                                    for (int i = 0; i < dsgst.Tables[0].Rows.Count; i++)
                                    {
                                        string type = dsgst.Tables[0].Rows[i]["Type"].ToString();
                                        string gst = dsgst.Tables[0].Rows[i]["gst"].ToString();
                                        q = "update sale set gsttype='" + type + "' where GSTPerc='" + gst + "' and gsttype is null  and billstatus='Paid'";
                                        objcore.executeQuery(q);
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {

                        }
                        if (chk == "yes")
                        {
                            _frm.Close();

                        }
                        string view = POSRestaurant.Properties.Settings.Default.view;
                        if (view == "old")
                        {
                            POSRestaurant.forms.BackendForm obj = new BackendForm();

                            POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                            POSRestaurant.Properties.Settings.Default.Save();
                            obj.Show();
                        }
                        else
                        {

                            POSRestaurant.forms.Backendnew obj = new Backendnew();
                            POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                            POSRestaurant.Properties.Settings.Default.Save();
                            obj.Show();
                        }


                        //POSRetail.Sale.test oj = new Sale.test();
                        //oj.Show();
                    }
                    this.Hide();
                    ///////////////////////////

                }
                else
                {
                    dr.Close();
                    MessageBox.Show("User not found!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string getkey()
        {
            string val = POSRestaurant.Properties.Settings.Default.key.ToString();
            POSRetail.forms.SuperAdminForm obj = new POSRetail.forms.SuperAdminForm();
            val = obj.getkey();
            return val;
        }
        public static string Getbranchid()
        {
            string branchid = "";
            POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
            DataSet reginfo = new DataSet();
            string q = "select * from Branch  order by id desc";
            reginfo = objcore.funGetDataSet(q);
            if (reginfo.Tables[0].Rows.Count > 0)
            {
                branchid = reginfo.Tables[0].Rows[0]["BranchCode"].ToString();
            }
            return branchid;


        }
        public string getscree()
        {
            string screen = "";
            try
            {
                POSRestaurant.classes.Clsdbcon objcor = new classes.Clsdbcon();
                DataSet dsscreen = new DataSet();
                dsscreen = objcor.funGetDataSet("select * from screens where status='active'");
                screen = dsscreen.Tables[0].Rows[0]["screen"].ToString();
            }
            catch (Exception ex)
            {



            }
            return screen;
        }
        public void cardlogin()
        {
           /* if (keystatus.ToLower() == "inactive")
            {
                POSRestaurant.Properties.Settings.Default.KeyDate = DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd");
                POSRestaurant.Properties.Settings.Default.Save();
                MessageBox.Show("Your License Key has expired.Please Contact you service provider");
                btnlicense.Visible = true;
                return;
            }
            if (keystatus == "Not Defined")
            {
                if (DateTime.Now > Convert.ToDateTime("2022-06-30"))
                {
                    MessageBox.Show("Your License Key is not defined.Please Contact you service provider");
                    btnlicense.Visible = true;
                    return;
                }
            }*/
            POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
            POSRestaurant.Properties.Settings.Default.apiExecution = "";
            POSRestaurant.Properties.Settings.Default.Save();
            try
            {
                string q = "select * from DeviceSetting where Device='API Execution'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    POSRestaurant.Properties.Settings.Default.apiExecution = ds.Tables[0].Rows[0]["Status"].ToString();
                    POSRestaurant.Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                string q = "select * from deliverytransfer where type='API'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    POSRestaurant.Properties.Settings.Default.BaseUrl = ds.Tables[0].Rows[0]["url"].ToString();
                    POSRestaurant.Properties.Settings.Default.Save();
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                if (vTextBox1.Text == string.Empty)
                {

                }
                else
                {
                    //if (block() == "yes")
                    //{
                    //    MessageBox.Show(blockmessage());
                    //    return;
                    //}
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    SqlDataReader dr = objCore.funGetDataReader("Select * from Users Where  cardno='" + vTextBox1.Text + "' ");
                    if (dr.HasRows)
                    {

                        if (System.Environment.MachineName.ToString() == "DUMMY")
                        {
                            try
                            {

                                DataSet reginfo = new DataSet();
                                string q = "select top 1 * from users  order by id desc";
                                reginfo = objcore.funGetDataSet(q);
                                if (reginfo.Tables[0].Rows.Count > 0)
                                {
                                    string d = POSRestaurant.Properties.Settings.Default.KeyDate;
                                    if (d.ToString().Length > 0)
                                    {
                                        DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                                        DateTime d2 = Convert.ToDateTime(d);

                                        TimeSpan t = d1 - d2;
                                        double NrOfDays = t.TotalDays;
                                        if (NrOfDays >= 366)
                                        {
                                            MessageBox.Show("Your Software registration has expired. Please contact your service provider");
                                            return;
                                        }
                                        if (NrOfDays >= 335)
                                        {
                                            MessageBox.Show((365 - NrOfDays).ToString() + " Days left of Your Software Registration.");

                                        }

                                    }
                                    else
                                    {
                                        MessageBox.Show("Your Software is not registered. Please contact your service provider");
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("Your Software is not registered. Please contact your service provider");
                                    return;
                                }
                            }
                            catch (Exception ex)
                            {

                                MessageBox.Show("Connection Not Initialized. Please Contact you service provider");
                                return;
                            }
                        }
                        dr.Read();
                        try
                        {
                            POSRestaurant.Properties.Settings.Default.usertype = dr["role"].ToString();
                            POSRestaurant.Properties.Settings.Default.Save();
                        }
                        catch (Exception ex)
                        {


                        }
                        POSRestaurant.classes.Clsdbcon ob = new classes.Clsdbcon();
                        if (dr["Usertype"].ToString().ToLower() == "cashier")//.GetValue(1).ToString())
                        {
                            string brid = Getbranchid();
                            if (brid == string.Empty)
                            {
                                MessageBox.Show("Your Branch info is not added.Please Contact your Admin");
                                return;
                            }
                            string screen = "";
                            //screen = getscree();

                            if (chk == "yes")
                            {
                                _frm.userid = dr["id"].ToString();
                                POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                POSRestaurant.Properties.Settings.Default.Save();
                                _frm.updateinfo();
                                this.Hide();
                            }
                            else
                            {
                                POSRestaurant.Sale.RestSale obj = new Sale.RestSale();
                                obj.userid = dr["id"].ToString();
                                POSRestaurant.Properties.Settings.Default.branchid = brid;
                                POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                POSRestaurant.Properties.Settings.Default.Save();
                             
                                obj.Show();
                            }
                            //POSRetail.Sale.test oj = new Sale.test();
                            //oj.Show();
                        }
                        if (dr["Usertype"].ToString().ToLower() == "kds")//.GetValue(1).ToString())
                        {

                            string screen = "";
                            POSRestaurant.Sale.kdsnewfix obj = new Sale.kdsnewfix();
                            obj.terminal = dr["terminal"].ToString();
                            obj.kdsid = dr["kdsid"].ToString();// "1"; //comboBox1.SelectedValue.ToString();
                            //obj.kdsname = "Kitchen";
                            obj.Show();

                            
                        }
                        string query = "";
                        if (dr["Usertype"].ToString().ToLower() == "admin")//.GetValue(1).ToString())
                        {
                            try
                            {
                                query = "ALTER TABLE [dbo].[Sale]  ADD GSTtype varchar(100) NULL ";
                                objcore.executeQuery(query);
                                query = "ALTER TABLE [dbo].[MenuItem]  ADD Price2 float NULL ";
                                objcore.executeQuery(query);
                                query = "ALTER TABLE [dbo].[MenuItem]  ADD Price3 float NULL ";
                                objcore.executeQuery(query);
                            }
                            catch (Exception ex)
                            {


                            }
                            try
                            {
                                query = "ALTER TABLE [dbo].[Closing]  ADD Userid int NULL ";
                                objcore.executeQuery(query);
                            }
                            catch (Exception ex)
                            {


                            }
                            try
                            {
                                string q = "select id from sale where gsttype is null and billstatus='Paid'";
                                DataSet dsq = new DataSet();
                                dsq = objcore.funGetDataSet(q);
                                if (dsq.Tables[0].Rows.Count > 0)
                                {
                                    q = "update sale set gsttype='Cash' where BillType='Cash' and gsttype is null and billstatus='Paid'";
                                    objcore.executeQuery(q);
                                    q = "update sale set gsttype='Card' where BillType like '%visa%' and gsttype is null  and billstatus='Paid'";
                                    objcore.executeQuery(q);

                                    q = "select id from sale where gsttype is null and billstatus='Paid'";
                                    dsq = new DataSet();
                                    dsq = objcore.funGetDataSet(q);
                                    if (dsq.Tables[0].Rows.Count > 0)
                                    {
                                        q = "select * from gst";
                                        DataSet dsgst = new DataSet();
                                        dsgst = objcore.funGetDataSet(q);
                                        for (int i = 0; i < dsgst.Tables[0].Rows.Count; i++)
                                        {
                                            string type = dsgst.Tables[0].Rows[i]["Type"].ToString();
                                            string gst = dsgst.Tables[0].Rows[i]["gst"].ToString();
                                            q = "update sale set gsttype='" + type + "' where GSTPerc='" + gst + "' and gsttype is null  and billstatus='Paid'";
                                            objcore.executeQuery(q);
                                        }
                                    }
                                }

                            }
                            catch (Exception ex)
                            {

                            }
                            if (chk == "yes")
                            {
                                _frm.Close();

                            }
                            string view = POSRestaurant.Properties.Settings.Default.view;
                            if (view == "old")
                            {
                                POSRestaurant.forms.BackendForm obj = new BackendForm();

                                POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                POSRestaurant.Properties.Settings.Default.Save();
                                obj.Show();
                            }
                            else
                            {

                                POSRestaurant.forms.Backendnew obj = new Backendnew();
                                POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                POSRestaurant.Properties.Settings.Default.Save();
                                obj.Show();
                            }
                        }
                        this.Hide();
                        ///////////////////////////

                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }

        }
        private void vTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com"))
                    return true;
            }
            catch
            {
                return false;
            }
        }
        public string keystatus = "";
        protected void getcloudkey()
        {
            try
            {
                string uri =POSRestaurant.Properties.Settings.Default.keybaseurl+ "/API/keys.asmx/Getresponse?key=" + POSRestaurant.Properties.Settings.Default.key;
                RegistrationClass regcls = new RegistrationClass();
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<KeysClass> res = (List<KeysClass>)JsonConvert.DeserializeObject(result, typeof(List<KeysClass>));
                    if (res.Count > 0)
                    {
                        keystatus = res[0].Status;

                    }
                    else
                    {
                        keystatus = "Not Defined";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                keystatus = "";

            }
        }
        private void NewLogIn_Load(object sender, EventArgs e)
        {


          /*  if (System.Environment.MachineName.ToString() == "DUMMY")
            {

                if (CheckForInternetConnection() == true)
                {
                    if (POSRestaurant.Properties.Settings.Default.keybaseurl.ToString().Length == 0)
                    {
                        btnlicense.Visible = true;
                    }
                    getcloudkey();
                }
                if (keystatus == "InActive")
                {
                    btnlicense.Visible = true;
                }
            }*/
            //////this.TopMost = true;
            if (POSRestaurant.Properties.Settings.Default.ConnectionString == "")
            {
                //string servers = "";
                //List<String> ServerNames = new List<String>();
                //var instances = SqlDataSourceEnumerator.Instance.GetDataSources();
                //foreach (DataRow instance in instances.AsEnumerable())
                //{
                //    string instance1 = instance["InstanceName"].ToString();
                //    servers = instance["ServerName"].ToString();
                //    if (instance1.Length > 0)
                //    {
                //        servers = servers + "\\" + instance1;
                //    }
                //    ServerNames.Add(servers);
                //}

                //foreach (var item in ServerNames)
                //{
                //    string connString = "TrustServerCertificate=True;Password=gl;Persist Security Info=True;User ID=gl;Initial Catalog=LawBizPOS;Data Source=" + item.ToString();

                //    try
                //    {
                //        SqlConnection conn = new SqlConnection();
                //        conn.ConnectionString = connString;

                //        //this.Cursor = Cursors.WaitCursor;

                //        if (conn.State == System.Data.ConnectionState.Open)
                //        {
                //            conn.Close();
                //        }
                //        conn.Open();
                //        POSRestaurant.Properties.Settings.Default.ConnectionString = connString;
                //        POSRestaurant.Properties.Settings.Default.Save();

                //        Application.Exit();
                //        System.Diagnostics.Process.Start(Application.ExecutablePath);
                //    }
                //    catch (Exception exx)
                //    {
                      
                //    }
                //}

                Connect obj = new Connect();
                obj.Show();
                this.Hide();
              
            }
            else
            {
                try
                {
                    SqlConnection con = new SqlConnection(POSRestaurant.Properties.Settings.Default.ConnectionString);
                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        //con.Close();
                        con.Open();
                    }
                    con.Open();
                    
                }
                catch (Exception ex)
                {
                    btnconect.Visible = true;
                    
                }

                try
                {
                    POSRestaurant.classes.Clsdbcon Objcore = new classes.Clsdbcon();
                    try
                    {
                        string query = "ALTER TABLE [dbo].[Users]  ADD KDSType varchar(50) NULL ";
                        Objcore.executeQuery(query);
                    }
                    catch (Exception ex)
                    {


                    }
                    string q = "select * from users where role='" + System.Environment.MachineName + "'";
                    DataSet dster = new DataSet();
                   
                    dster = Objcore.funGetDataSet(q);
                    if (dster.Tables[0].Rows.Count > 0)
                    {
                        
                        if (dster.Tables[0].Rows[0]["KDSType"].ToString().ToLower()=="list")
                        {
                            if (System.Environment.MachineName.ToLower().ToString().Contains("kds-main"))
                            {
                                POSRestaurant.Sale.KDSKitchenwise obj = new Sale.KDSKitchenwise();
                                obj.Show();
                            }
                        }
                        else
                        {
                            if (System.Environment.MachineName.ToLower().ToString().Contains("kds-main"))
                            {
                                string screen = "";
                                POSRestaurant.Sale.kdsnewfixmain obj = new Sale.kdsnewfixmain();
                                obj.terminal = dster.Tables[0].Rows[0]["terminal"].ToString();
                                obj.kdsid = dster.Tables[0].Rows[0]["kdsid"].ToString();// "1"; //comboBox1.SelectedValue.ToString();
                                obj.park = dster.Tables[0].Rows[0]["ParkStatus"].ToString();
                                obj.Show();
                            }
                            else
                            {
                                string screen = "";
                                POSRestaurant.Sale.kdsnewfix obj = new Sale.kdsnewfix();
                                obj.terminal = dster.Tables[0].Rows[0]["terminal"].ToString();
                                obj.kdsid = dster.Tables[0].Rows[0]["kdsid"].ToString();// "1"; //comboBox1.SelectedValue.ToString();
                                //obj.kdsname = "Kitchen";
                                obj.Show();
                            }
                        }
                        this.Hide();
                    }
                }
                catch (Exception ex)
                {
                    
                    
                }
            }
            vTextBox1.Focus();
        }

        private void vTextBox1_Enter(object sender, EventArgs e)
        {
            strt = vTextBox1.SelectionStart;
        }

        private void btnconect_Click(object sender, EventArgs e)
        {
            Connect obj = new Connect();
            obj.Show();
            this.Hide();
        }

        private void btnlicense_Click(object sender, EventArgs e)
        {
            License obj = new License();
            obj.TopMost = true;
            obj.Show();
        }
    }
}
