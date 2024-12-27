using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POSRetail.Accounts
{
    public partial class ChartofAccounts : Form
    {
        POSRetail.classes.Clsdbcon objcore = new classes.Clsdbcon();
       static string id = "";
        public ChartofAccounts()
        {
            InitializeComponent();
        }
        public void getdata()
        {
            try
            {
                objcore = new classes.Clsdbcon();
                DataSet ds = new System.Data.DataSet();
                string q = "select * from ChartofAccounts";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //treeView1.Nodes.Clear();
                    treeView1.Nodes[0].Nodes[0].Nodes.Clear();
                    treeView1.Nodes[0].Nodes[1].Nodes.Clear();
                    treeView1.Nodes[1].Nodes[1].Nodes.Clear();
                    treeView1.Nodes[1].Nodes[0].Nodes.Clear();
                    treeView1.Nodes[2].Nodes.Clear();
                    treeView1.Nodes[3].Nodes.Clear();
                    treeView1.Nodes[4].Nodes.Clear();
                    treeView1.Nodes[5].Nodes[0].Nodes.Clear();
                    treeView1.Nodes[5].Nodes[1].Nodes.Clear();
                    treeView1.Nodes[5].Nodes[2].Nodes.Clear();
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string type = dr["AccountType"].ToString();
                    if (type == "Fixed Assets")
                    {

                        treeView1.Nodes[0].Nodes[0].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Current Assets")
                    {
                       treeView1.Nodes[0].Nodes[1].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Current Liabilities")
                    {
                        treeView1.Nodes[1].Nodes[1].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Long Term Liabilities")
                    {

                        treeView1.Nodes[1].Nodes[0].Nodes.Add(dr["Name"].ToString());
                    }

                    if (type == "Equity and Capital")
                    {

                        treeView1.Nodes[2].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Revenue")
                    {

                        treeView1.Nodes[3].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Cost of Sales")
                    {

                        treeView1.Nodes[4].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Operating Expenses")
                    {

                        treeView1.Nodes[5].Nodes[0].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Admin and Selling Expenses")
                    {

                        treeView1.Nodes[5].Nodes[1].Nodes.Add(dr["Name"].ToString());
                    }
                    if (type == "Financial Expenses")
                    {

                        treeView1.Nodes[5].Nodes[2].Nodes.Add(dr["Name"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        public void retrieve(string name)
        {
            try
            {
                DataSet ds = new System.Data.DataSet();
                string q = "select * from ChartofAccounts where Name='" + name + "'";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    txtcode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString();

                    txtdesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                    cmbtype.Text = ds.Tables[0].Rows[0]["AccountType"].ToString();
                    cmbstatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                    id = ds.Tables[0].Rows[0]["id"].ToString();
                    int count = textBox1.Text.Length;
                    if (count > 0)
                    {
                        txtcode.Text = txtcode.Text.Substring(count);
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
            
        }
        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
           string val= e.Node.Text.ToString();
           retrieve(val);

        }

        private void ChartofAccounts_Load(object sender, EventArgs e)
        {
            getdata();
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            string val = treeView1.SelectedNode.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbtype.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Type");
                    return;
                }
                if (txtname.Text == string.Empty)
                {
                    MessageBox.Show("Please Eneter Name");
                    return;
                }
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("Account Code for \""+cmbtype.Text+"\" is not defined");
                    return;
                }
                if (txtcode.Text == string.Empty)
                {
                    MessageBox.Show("Please Eneter Account Code");
                    return;
                }
                if (txtdesc.Text == string.Empty)
                {
                    MessageBox.Show("Please Eneter Description");
                    return;
                }
                
                if (cmbstatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Activation Status");
                    return;
                }
                DataSet ds = new System.Data.DataSet();
                int idd = 0;
                ds = objcore.funGetDataSet("select max(id) as id from ChartofAccounts");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0][0].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    idd = Convert.ToInt32(i) + 1;
                }
                else
                {
                    idd = 1;
                }
                ds = new System.Data.DataSet();
                string q = "select * from ChartofAccounts where Name='" + txtname.Text.Trim() + "' or AccountCode='"+textBox1.Text.Trim() + txtcode.Text.Trim() + "'";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Account Name or Code Already Exist");
                    return;
                }
                objcore.executeQuery("insert into ChartofAccounts (id,AccountType,Name,AccountCode,Description,Status) values ('" + idd + "','" + cmbtype.Text + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + textBox1.Text + txtcode.Text.Trim().Replace("'", "''") + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text + "')");
                MessageBox.Show("Data Saved Successfully");
                getdata();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            try
            {
                txtname.Text = string.Empty;
                txtcode.Text = string.Empty;
                txtdesc.Text = string.Empty;
                textBox1.Text = string.Empty;
                cmbtype.Text = string.Empty;
                cmbstatus.Text = string.Empty;
            }
            catch (Exception ex)
            {


            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbtype.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Type");
                    return;
                }
                if (txtname.Text == string.Empty)
                {
                    MessageBox.Show("Please Eneter Name");
                    return;
                }
                if (textBox1.Text == string.Empty)
                {
                    MessageBox.Show("Account Code for " + cmbtype.Text + " is not defined");
                    return;
                }
                if (txtcode.Text == string.Empty)
                {
                    MessageBox.Show("Please Eneter Account Code");
                    return;
                }
                if (txtdesc.Text == string.Empty)
                {
                    MessageBox.Show("Please Eneter Description");
                    return;
                }
                
                if (cmbstatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Activation Status");
                    return;
                }
                DataSet ds = new System.Data.DataSet();
                //string q = "select * from ChartofAccounts where AccountCode='" + txtcode.Text + "'";
                //ds = objcore.funGetDataSet(q);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    MessageBox.Show("Account Name or Code Already Exist");
                //    return;
                //}
                objcore.executeQuery("update ChartofAccounts set AccountType='"+cmbtype.Text+"', Name='" + txtname.Text + "', AccountCode='" + textBox1.Text + txtcode.Text + "',Description='" + txtdesc.Text + "',Status='" + cmbstatus.Text + "' where id='" + id + "'");
                MessageBox.Show("Data Updated Successfully");
                getdata();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbtype_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new System.Data.DataSet();
                string q = "select * from ChartofAccountsCodes where Name='" + cmbtype.Text + "' ";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    textBox1.Text = ds.Tables[0].Rows[0]["Code"].ToString();

                }
                else
                {
                    textBox1.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string q = "DELETE FROM ChartofAccounts WHERE     (Id = '"+id+"')";
           // objcore.executeQuery(q);
            SqlConnection connection;

            try
            {
                string cs = objcore.getConnectionString();
                connection = new SqlConnection(cs);

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Open();
                SqlCommand com = new SqlCommand(q, connection);
                int rows = com.ExecuteNonQuery();
                connection.Close();
                if (rows > 0)
                {
                    MessageBox.Show("Deleted successfully");
                }
                else
                {
                    MessageBox.Show("No Record Deleted");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("You can not delete this");
            }
            finally
            {
                //connection.Close();
            }
            getdata();
            clear();
        }
    }
}
