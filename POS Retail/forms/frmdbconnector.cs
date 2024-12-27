using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POSRetail.classes;

namespace POSRetail.forms
{
    public partial class frmdbconnector : Form
    {
        public string connectionstring;

        public frmdbconnector()
        {
            InitializeComponent();
        }

        private void frmMySQLConnector_Load(object sender, EventArgs e)
        {

        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUID.Text == "" || txtServer.Text == "" || txtPassword.Text == "" || txtDatabase.Text == "")
                {
                    MessageBox.Show("Fill all the required fields", "MySQL Connector", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    Clsdbcon.Database = txtDatabase.Text;
                    Clsdbcon.Password = txtPassword.Text;
                    Clsdbcon.Server = txtServer.Text;
                    Clsdbcon.User = txtUID.Text;
                    Clsdbcon.con.Open();
                    Clsdbcon.con.Close();
                    MessageBox.Show("Test Connection Successfully!");
                }
            }
            catch
            {
                MessageBox.Show("Cannot Established Connection", "Connection Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
           
            Clsdbcon.Database = txtDatabase.Text;
            Clsdbcon.Password = txtPassword.Text;
            Clsdbcon.Server = txtServer.Text;
            Clsdbcon.User = txtUID.Text;
            Clsdbcon.con.Open();
            this.Dispose();
        }

        
    }
}
