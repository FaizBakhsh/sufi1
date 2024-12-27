using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.forms
{
    
    public partial class Connect : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public Connect()
        {
            InitializeComponent();
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
                    connString = "TrustServerCertificate=True;Password=" + this.txtServerPassword.Text.Trim() + ";Persist Security Info=True;User ID=" + this.txtServerLogin.Text.Trim() + ";Initial Catalog=" + this.txtDB.Text.Trim() + ";Data Source=" + this.txtServer.Text.Trim();
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
               // POSRetail.Properties.Settings.Default.ConnectionString = connString;
                //POSRestaurant.Properties.Settings.Default.Save();
                MessageBox.Show("Connection established Successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                POSRestaurant.Sale.RestSale frm = new Sale.RestSale();
                NewLogIn obj = new NewLogIn(frm);
                obj.Show();
                this.Close();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message+ "\nThe Information you entered is not correct or valid..\n Please Check The following:-\n (i) Server Name/IP Address\n (ii) Login\n (iii) Password\n (iv) Database");
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
    }
}
