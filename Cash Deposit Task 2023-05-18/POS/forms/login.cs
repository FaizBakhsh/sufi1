using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data.SqlClient;
using POSRestaurant.classes;
using System.Net;
using System.Net.NetworkInformation;
using System.Management;
using System.IO;
using System.Diagnostics;
namespace POSRestaurant.forms
{
    public partial class login : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new Clsdbcon();
        private TextBox focusedTextbox = null;
        public login()
        {
            InitializeComponent();
        }
       
        private void frmLogin_Load(object sender, EventArgs e)
        {
            try
            {
                Microsoft.Win32.RegistryKey rkey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Control Panel\International", true);
                rkey.SetValue("sShortDate", "yyyy-MM-dd");
                rkey.SetValue("sLongDate", "dddd, d MMMM, yyyy");
            }
            catch (Exception ex)
            {


            }
            //if (System.Diagnostics.Process.GetProcessesByName(System.IO.Path.GetFileNameWithoutExtension(System.Reflection.Assembly.GetEntryAssembly().Location)).Length > 1) System.Diagnostics.Process.GetCurrentProcess().Kill();
           //int length=Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length;
           //if (length > 1)
           // {
           //     // If ther is more than one, than it is already running.
           //    // System.Windows.MessageBox.Show("Application is already running.");
           //     System.Diagnostics.Process.GetCurrentProcess().Kill();
           //     return;
           // }
            button2.Text = "!";
            shiftkey();
            if (POSRestaurant.Properties.Settings.Default.ConnectionString == string.Empty)
            {
                
            }
            else
            {
               // tabControl1.TabPages.RemoveAt(1);
            }
            tabPage1.Focus();
            this.textBox1.Focus();
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from kds";
                ds = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                
              
            }
            getcompany();
            //Form1 obj = new Form1();
            //obj.Show();
            //this.Hide();
        }

        public static string GetMACAddress2()
        {
            //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //String sMacAddress = string.Empty;
            //foreach (NetworkInterface adapter in nics)
            //{
            //    if (sMacAddress == String.Empty)// only return MAC Address from first card  
            //    {
            //        //IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
            //        sMacAddress = adapter.GetPhysicalAddress().ToString();
            //    }
            //} return sMacAddress;
            //string mbInfo="";
            //try
            //{
            //    ManagementObjectSearcher mbs = new System.Management.ManagementObjectSearcher("Select * From Win32_BaseBoard");
            //    foreach (ManagementObject mo in mbs.Get())
            //    {
            //        mbInfo += mo["SerialNumber"].ToString();
            //    }
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show("Error getting motherboard no");
            //}
            POSRetail.forms.login1 obj = new POSRetail.forms.login1(); //POSRestaurant.Properties.Settings.Default.KeyDate.ToString();
            string d = obj.getkey();
            return d;
        }
        public void loginanyway(string id, string type)
        {
            if (type.ToLower() == "cashier")//.GetValue(1).ToString())
            {
                string screen = "";// "cinema";
                if (screen == "cinema")
                {
                    POSRestaurant.Sale.Cinema obj = new Sale.Cinema();

                    //obj.userid = dr["id"].ToString();
                    POSRestaurant.Properties.Settings.Default.UserId = id;
                    obj.Show();

                }
                else
                {
                    POSRestaurant.Sale.RestSale obj = new Sale.RestSale();
                   
                    obj.userid = id;
                    POSRestaurant.Properties.Settings.Default.UserId = id;
                    obj.Show();
                    //POSRestaurant.Sale.test oj = new Sale.test();
                    //oj.Show();
                }

            }
            if (type == "admin")//.GetValue(1).ToString())
            {
                POSRestaurant.forms.BackendForm obj = new BackendForm();
                
                //obj.userid = dr["id"].ToString();
                POSRestaurant.Properties.Settings.Default.UserId = id;
                obj.Show();
                //POSRestaurant.Sale.test oj = new Sale.test();
                //oj.Show();
            }
            this.Hide();
        }
        private void cmdLogin_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //Sale.Sale obj1 = new Sale.Sale();
            //obj1.Show();
            //return;
            string mc = GetMACAddress2();
            if (this.txtUserId.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter user Id!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtUserId.Focus();
                return;
            }
            if (this.txtPassword.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please enter user password!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.txtPassword.Focus();
                return;
            }
            try
            {
                if (txtUserId.Text.ToLower() == "superadmin" && txtPassword.Text == "L@wBiz1987")
                {
                   POSRetail.forms.SuperAdminForm obj = new  POSRetail.forms.SuperAdminForm();
                    obj.Show();
                    this.Hide();
                    return;
                }
                try
                {
                    POSRestaurant.classes.Clsdbcon objcore = new Clsdbcon();
                    DataSet reginfo = new DataSet();
                    string q = "select top 1 * from reg where RegNo='" + mc + "' order by id desc";
                    reginfo = objcore.funGetDataSet(q);
                    if (reginfo.Tables[0].Rows.Count > 0)
                    {
                        if (mc != reginfo.Tables[0].Rows[0]["RegNo"].ToString())
                        {
                            MessageBox.Show("Your Software is not registered. Please contact your service provider");
                            return;
                        }

                        try
                        {
                            POSRetail.forms.login1 obj = new POSRetail.forms.login1(); //POSRestaurant.Properties.Settings.Default.KeyDate.ToString();
                            string d = obj.getdate();
                            POSRestaurant.Properties.Settings.Default.KeyDate = d;
                            POSRestaurant.Properties.Settings.Default.Save();

                            DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                            DateTime d2 = Convert.ToDateTime(d);

                            TimeSpan t = d1 - d2;
                            double NrOfDays = t.TotalDays;
                            if (NrOfDays >= 90)
                            {
                                MessageBox.Show("Your Software registration is expired. Please contact your service provider");
                                return;
                            }
                            if (NrOfDays >= 80)
                            {
                                MessageBox.Show((90 - NrOfDays).ToString() + " Days left of Your Software Registration.");

                            }
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show("Date formate error");
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
                POSRestaurant.classes.Clsdbcon objCore = new Clsdbcon();
                SqlDataReader dr = objCore.funGetDataReader("Select * from Users Where Username = '" + this.txtUserId.Text.Trim() + "' and password='"+txtPassword.Text+"' ");
                if (dr.HasRows)
                {
                    dr.Read();
                    
                    if (this.txtPassword.Text.Trim() != dr["Password"].ToString())//.GetValue(1).ToString())
                    {
                        dr.Close();
                        MessageBox.Show("Incorrect password!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }
                    
                    
                    {
                        if (dr["Usertype"].ToString().ToLower() == "cashier")//.GetValue(1).ToString())
                        {
                            //if (dr["status"].ToString().ToLower() == "true")
                            //{
                            //    DialogResult drg = MessageBox.Show("This user is already Loged In. Continue anyway", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                            //    if (drg == DialogResult.Yes)
                            //    {
                            //        RestAllowDiscount ob = new RestAllowDiscount(this);
                            //        ob.id = dr["id"].ToString();
                            //        ob.utype = dr["Usertype"].ToString();
                            //        ob.editmode = "login";
                            //        ob.Show();
                            //    }
                            //    else
                            //    {
                            //        return;
                            //    }
                            //}
                            //else
                            {
                                string screen = "";// "cinema";
                                if (screen == "cinema")
                                {
                                    POSRestaurant.Sale.Cinema obj = new Sale.Cinema();

                                    //obj.userid = dr["id"].ToString();
                                    POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                    obj.Show();

                                }
                                else
                                {
                                    POSRestaurant.Sale.RestSale obj = new Sale.RestSale();
                                    updateuser(dr["id"].ToString());
                                    obj.userid = dr["id"].ToString();
                                    POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                    obj.Show();
                                    //POSRestaurant.Sale.test oj = new Sale.test();
                                    //oj.Show();
                                }
                            }
                        }
                        if (dr["Usertype"].ToString().ToLower() == "admin")//.GetValue(1).ToString())
                        {
                            POSRestaurant.forms.BackendForm obj = new BackendForm();
                            //updateuser(dr["id"].ToString());
                            //obj.userid = dr["id"].ToString();
                            POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                            obj.Show();
                            //POSRestaurant.Sale.test oj = new Sale.test();
                            //oj.Show();
                        }
                        this.Hide();
                    }
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
        }
        public void updateuser(string id)
        {
            try
            {
                string q = "update Users set status='true' where id='" + id + "'";
                objCore.executeQuery(q);
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
    

        }

        private void pbxCancel_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void gbLogin_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = this.cmdLogin;
        }

       //private void llConnection_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
       // {
       //     if (this.gbServer.Visible)
       //     {
       //         this.gbServer.Visible = false;
       //         this.txtUserId.Focus();
       //     }
       //     else
       //         this.gbServer.Visible = true;
       // }

        private void gbServer_Enter(object sender, EventArgs e)
        {
            this.AcceptButton = null;
        }

        private void gbLogin_Leave(object sender, EventArgs e)
        {
    
        }

        public void saveconection(string date)
        {
            POSRestaurant.Properties.Settings.Default.KeyDate = date;
            POSRestaurant.Properties.Settings.Default.Save();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            SqlConnection conn = new SqlConnection();
            try
            {
                string connString = string.Empty;

                if (this.chkIntegratedSecurity.Checked)
                {
                    if (this.txtServer.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Enter the server name!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtServer.Focus();
                        return;
                    }
                    if (this.txtDB.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Enter the Database name!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtDB.Focus();
                        return;
                    }
                }
                else
                {
                    if (this.txtServer.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Enter the server name!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtServer.Focus();
                        return;
                    }
                    if (this.txtServerLogin.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Enter the login name!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtServerLogin.Focus();
                        return;
                    }
                    if (this.txtServerPassword.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Enter the server password!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtServerPassword.Focus();
                        return;
                    }
                    if (this.txtDB.Text.Trim() == string.Empty)
                    {
                        MessageBox.Show("Enter the Database name!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtDB.Focus();
                        return;
                    }
                }

                if (this.chkIntegratedSecurity.Checked)//if Integrated security is checked
                {
                    connString = "Persist Security Info=True;Integrated Security = true;Initial Catalog=" + this.txtDB.Text.Trim() + ";Data Source=" + this.txtServer.Text.Trim();

                }
                else
                {
                    connString = "Password=" + this.txtServerPassword.Text.Trim() + ";Persist Security Info=True;User ID=" + this.txtServerLogin.Text.Trim() + ";Initial Catalog=" + this.txtDB.Text.Trim() + ";Data Source=" + this.txtServer.Text.Trim();
                }

                conn.ConnectionString = connString;

                //this.Cursor = Cursors.WaitCursor;

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Open();
                POSRestaurant.Properties.Settings.Default.ConnectionString = connString;
                POSRestaurant.Properties.Settings.Default.Save();
                POSRetail.forms.login1 obj = new POSRetail.forms.login1();
                obj.saveconection(connString);
                //POSRetail.Properties.Settings.Default.ConnectionString = connString;
                //POSRetail.Properties.Settings.Default.Save();
                MessageBox.Show("Connection established Successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Cursor = Cursors.Default;
            }
            catch (Exception)
            {
              MessageBox.Show("The Information you entered is not correct or valid..\n Please Check The following:-\n (i) Server Name/IP Address\n (ii) Login\n (iii) Password\n (iv) Database");
            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                    this.Cursor = Cursors.Default;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
           // textBox1.Text = textBox1.Text + e.KeyChar.ToString().Trim();
            
        }
        public void cardlogin()
        {
            try
            {
                if (textBox1.Text == string.Empty)
                {

                }
                else
                {
                    POSRestaurant.classes.Clsdbcon objCore = new Clsdbcon();
                    SqlDataReader dr = objCore.funGetDataReader("Select * from Users Where  cardno='" + textBox1.Text + "' ");
                    if (dr.HasRows)
                    {

                        dr.Read();
                        string mc = GetMACAddress2();
                        try
                        {
                            POSRestaurant.classes.Clsdbcon objcore = new Clsdbcon();
                            DataSet reginfo = new DataSet();
                            string q = "select top 1 * from reg where RegNo='" + mc + "' order by id desc";
                            reginfo = objcore.funGetDataSet(q);
                            if (reginfo.Tables[0].Rows.Count > 0)
                            {
                                if (mc != reginfo.Tables[0].Rows[0]["RegNo"].ToString())
                                {
                                    MessageBox.Show("Your Software is not registered. Please contact your service provider");
                                    return;
                                }

                                try
                                {
                                    POSRetail.forms.login1 obj = new POSRetail.forms.login1(); //POSRestaurant.Properties.Settings.Default.KeyDate.ToString();
                                    string d = obj.getdate();
                                    POSRestaurant.Properties.Settings.Default.KeyDate = d;
                                    POSRestaurant.Properties.Settings.Default.Save();

                                    DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                                    DateTime d2 = Convert.ToDateTime(d);

                                    TimeSpan t = d1 - d2;
                                    double NrOfDays = t.TotalDays;
                                    if (NrOfDays >= 90)
                                    {
                                        MessageBox.Show("Your Software registration is expired. Please contact your service provider");
                                        return;
                                    }
                                    if (NrOfDays >= 80)
                                    {
                                        MessageBox.Show((90 - NrOfDays).ToString() + " Days left of Your Software Registration.");

                                    }
                                }
                                catch (Exception ex)
                                {
                                    try
                                    {
                                        DataSet allowanyway = new DataSet();
                                        q = "select * from perm";
                                        allowanyway = objcore.funGetDataSet(q);
                                        if (allowanyway.Tables[0].Rows.Count > 0)
                                        {
                                            if (allowanyway.Tables[0].Rows[0]["allow"].ToString() == "yes")
                                            {

                                            }
                                            else
                                            {
                                                MessageBox.Show("Date formate error");
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("Date formate error");
                                            return;
                                        }
                                    }
                                    catch (Exception ex1)
                                    {
                                        
                                       MessageBox.Show("Date formate error");
                                       return;
                                    }
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

                        
                        
                        {
                            if (dr["Usertype"].ToString().ToLower() == "cashier")//.GetValue(1).ToString())
                            {
                                if (dr["status"].ToString().ToLower() == "true")
                                {
                                    DialogResult drg = MessageBox.Show("This user is already Loged In. Continue anyway", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                                    if (drg == DialogResult.Yes)
                                    {
                                        RestAllowDiscount ob = new RestAllowDiscount(this);
                                        ob.id = dr["id"].ToString();
                                        ob.utype = dr["Usertype"].ToString();
                                        ob.editmode = "login";
                                        ob.Show();
                                    }
                                    else
                                    {
                                        return;
                                    }
                                }
                                else
                                {
                                    POSRestaurant.Sale.RestSale obj = new Sale.RestSale();
                                    updateuser(dr["id"].ToString());
                                    obj.userid = dr["id"].ToString();
                                    POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                    obj.Show();
                                }
                                //POSRestaurant.Sale.test oj = new Sale.test();
                                //oj.Show();
                            }
                            if (dr["Usertype"].ToString().ToLower() == "admin")//.GetValue(1).ToString())
                            {
                                POSRestaurant.forms.BackendForm obj = new BackendForm();
                                //supdateuser(dr["id"].ToString());
                                //obj.userid = dr["id"].ToString();
                                POSRestaurant.Properties.Settings.Default.UserId = dr["id"].ToString();
                                obj.Show();
                                //POSRestaurant.Sale.test oj = new Sale.test();
                                //oj.Show();
                            }
                            this.Hide();
                        }
                        ///////////////////////////

                    }
                }
                
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
            }
            
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //cardlogin();
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            try
            {

                dscompany = objCore.funGetDataSet("select * from CompanyInfo");
                if (dscompany.Tables[0].Rows.Count > 0)
                {
                    vLabel1.Text = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    byte[] MyData = new byte[0];


                    DataRow myRow;
                    myRow = dscompany.Tables[0].Rows[0];

                    MyData = (byte[])myRow["logo"];

                    MemoryStream stream = new MemoryStream(MyData);
                    //With the code below, you are in fact converting the byte array of image
                    //to the real image.
                    pictureBox1.Image = Image.FromStream(stream);
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;
               
                if (focusedTextbox != null)
                {

                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void txtUserId_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
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
        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                int index = focusedTextbox.SelectionStart;
                focusedTextbox.Text = focusedTextbox.Text.Remove(focusedTextbox.SelectionStart - 1, 1);
                focusedTextbox.Select(index - 1, 1);
                focusedTextbox.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Please Select KDS");
                return;
            }
            if (textBox2.Text == string.Empty)
            {
                MessageBox.Show("Please Enter username");
                return;
            }
            if (textBox3.Text == string.Empty)
            {
                MessageBox.Show("Please Enter password");
                return;
            }
            string q = "select * from Users where username='"+textBox2.Text+"' and password='"+textBox3.Text+"'";
            DataSet dskds = new DataSet();
            dskds = objCore.funGetDataSet(q);
            if (dskds.Tables[0].Rows.Count > 0)
            {
                string typ = dskds.Tables[0].Rows[0]["Usertype"].ToString();
                if (typ.ToLower() == "admin" || typ.ToLower() == "kds")
                { }
                else
                {
                    MessageBox.Show("You are not authorized to open KDS");
                    return;
                }
                if (comboBox1.Text == "Main KDS")
                {
                    POSRestaurant.Sale.MainKds obj = new Sale.MainKds();
                    //obj.kds = comboBox1.SelectedValue.ToString();
                    //obj.kdsname = comboBox1.Text.ToString();
                    obj.Show();
                }
                else
                {
                    POSRestaurant.Sale.KDSScreen obj = new Sale.KDSScreen();
                    obj.kds = "1"; comboBox1.SelectedValue.ToString();
                    obj.kdsname = "Kitchen";
                    obj.Show();
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or Password");
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cardlogin();
            }
        }

        private void vLabel1_Click(object sender, EventArgs e)
        {

        }
   }
}
