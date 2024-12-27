using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POSRetail.forms
{
    public partial class query : Form
    {
        public query()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            POSRetail.classes.Clsdbcon objcore = new classes.Clsdbcon();
            string cs = POSRetail.Properties.Settings.Default.ConnectionString;
            SqlConnection connection = new SqlConnection(cs);
            
            try
            {
                
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                    connection.Open();
                    SqlCommand com = new SqlCommand(richTextBox1.Text, connection);
                int rows = com.ExecuteNonQuery();
                if (rows == 1)
                {
                    MessageBox.Show("Query Executed");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            
        }
    }
}
