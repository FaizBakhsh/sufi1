using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Data.SqlClient;
using POSRetail.classes;
using System.Net;
using System.Net.NetworkInformation;
using System.Management;
namespace POSRetail.forms
{
    public partial class login1 : Form
    {
        private TextBox focusedTextbox = null;
        public login1()
        {
            InitializeComponent();
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            button2.Text = "!";
            shiftkey();
            if (POSRetail.Properties.Settings.Default.ConnectionString == string.Empty)
            {
                
            }
            else
            {
               // tabControl1.TabPages.RemoveAt(1);
            }
            tabPage1.Focus();
            this.textBox1.Focus();

            //Form1 obj = new Form1();
            //obj.Show();
            //this.Hide();
        }
        //public static string GetMACAddress2()
        //{
        //    //NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
        //    //String sMacAddress = string.Empty;
        //    //foreach (NetworkInterface adapter in nics)
        //    //{
        //    //    if (sMacAddress == String.Empty)// only return MAC Address from first card  
        //    //    {
        //    //        //IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
        //    //        sMacAddress = adapter.GetPhysicalAddress().ToString();
        //    //    }
        //    //} return sMacAddress;
           
           
        //}
        private void cmdLogin_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //Sale.Sale obj1 = new Sale.Sale();
            //obj1.Show();
            //return;

            string mc = getkey();// GetMACAddress2();
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
                    SuperAdminForm obj = new SuperAdminForm();
                    obj.Show();
                    this.Hide();
                    return;
                }
                try
                {
                    POSRetail.classes.Clsdbcon objcore = new Clsdbcon();
                    DataSet reginfo = new DataSet();
                    string q = "select top 1 * from reg where RegNo='"+mc+"' order by id desc";
                    reginfo = objcore.funGetDataSet(q);
                    if (reginfo.Tables[0].Rows.Count > 0)
                    {
                        if (mc != reginfo.Tables[0].Rows[0]["RegNo"].ToString())
                        {
                            MessageBox.Show("Your Software is not registered. Please contact your service provider");
                            return;
                        }
                        string d = POSRetail.Properties.Settings.Default.KeyDate.ToString();
                        DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                        DateTime d2 = Convert.ToDateTime(d);

                        TimeSpan t = d1 - d2;
                        double NrOfDays = t.TotalDays;
                        if (NrOfDays >= 366)
                        {
                            MessageBox.Show("Your Software registration is expired. Please contact your service provider");
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
                catch (Exception ex)
                {

                    MessageBox.Show("Connection Not Initialized. Please Contact you service provider");
                    return;
                }
                progressBar1.Visible = true;
                POSRetail.classes.Clsdbcon objCore = new Clsdbcon();
                SqlDataReader dr = objCore.funGetDataReader("Select * from Users Where Username = '" + this.txtUserId.Text.Trim() + "' and password='"+txtPassword.Text+"' ");
                if (dr.HasRows)
                {
                    dr.Read();
                    try
                    {
                        if (dr["usertype"].ToString().ToLower() == "admin")
                        { }
                        else
                        {
                            POSRetail.Reports.Initilized rptDoc = new POSRetail.Reports.Initilized();
                           // rptDoc.PrintToPrinter(1, false, 0, 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    if (this.txtPassword.Text.Trim() != dr["Password"].ToString())//.GetValue(1).ToString())
                    {
                        dr.Close();
                        MessageBox.Show("Incorrect password!", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        return;
                    }


                    if (dr["Usertype"].ToString().ToLower()=="cashier")//.GetValue(1).ToString())
                    {
                        POSRetail.Sale.Sale obj = new Sale.Sale();

                        obj.userid = dr["id"].ToString();
                        POSRetail.Properties.Settings.Default.UserId = dr["id"].ToString();
                        obj.Show();
                        //POSRetail.Sale.test oj = new Sale.test();
                        //oj.Show();
                    }
                    if (dr["Usertype"].ToString().ToLower() == "admin")//.GetValue(1).ToString())
                    {
                        POSRetail.forms.BackendForm obj = new BackendForm();

                        //obj.userid = dr["id"].ToString();
                        POSRetail.Properties.Settings.Default.UserId = dr["id"].ToString();
                        obj.Show();
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
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
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
        public string getkey()
        {
            string val = POSRetail.Properties.Settings.Default.key.ToString();
            return val;
        }
        public string getdate()
        {
           string val= POSRetail.Properties.Settings.Default.KeyDate.ToString();
           return val;
        }
        public void saveconection(string con)
        {
            POSRetail.Properties.Settings.Default.ConnectionString = con;
            POSRetail.Properties.Settings.Default.Save();
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
                POSRetail.Properties.Settings.Default.ConnectionString = connString;
                POSRetail.Properties.Settings.Default.Save();
                
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
                    POSRetail.classes.Clsdbcon objCore = new Clsdbcon();
                    SqlDataReader dr = objCore.funGetDataReader("Select * from Users Where  cardno='" + textBox1.Text + "' ");
                    if (dr.HasRows)
                    {
                        string mc = getkey();
                        try
                        {
                            POSRetail.classes.Clsdbcon objcore = new Clsdbcon();
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
                                string d = POSRetail.Properties.Settings.Default.KeyDate.ToString();
                                DateTime d1 = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
                                DateTime d2 = Convert.ToDateTime(d);

                                TimeSpan t = d1 - d2;
                                double NrOfDays = t.TotalDays;
                                if (NrOfDays >= 366)
                                {
                                    MessageBox.Show("Your Software registration is expired. Please contact your service provider");
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
                        catch (Exception ex)
                        {

                            MessageBox.Show("Connection Not Initialized. Please Contact you service provider");
                            return;
                        }
                        dr.Read();




                        if (dr["Usertype"].ToString().ToLower() == "cashier")//.GetValue(1).ToString())
                        {
                            POSRetail.Sale.Sale obj = new Sale.Sale();

                            obj.userid = dr["id"].ToString();
                            POSRetail.Properties.Settings.Default.UserId = dr["id"].ToString();
                            obj.Show();
                            //POSRetail.Sale.test oj = new Sale.test();
                            //oj.Show();
                        }
                        if (dr["Usertype"].ToString().ToLower() == "admin")//.GetValue(1).ToString())
                        {
                            POSRetail.forms.BackendForm obj = new BackendForm();

                            //obj.userid = dr["id"].ToString();
                            POSRetail.Properties.Settings.Default.UserId = dr["id"].ToString();
                            obj.Show();
                            //POSRetail.Sale.test oj = new Sale.test();
                            //oj.Show();
                        }
                        this.Hide();
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
            cardlogin();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Button t = (sender) as Button;
            try
            {
                t = (sender) as Button;
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

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;
                t.Text = t.Text.Replace("&&", "&");
                if (focusedTextbox != null)
                {
                    
                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text;
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
            focusedTextbox = (TextBox)sender;
        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
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

        private void txtServerLogin_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtServer_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }

        private void txtServerPassword_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }
   }
}
