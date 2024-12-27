using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POSRestaurant.Accounts
{
    public partial class ChartofAccounts : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
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
                string q = "select * from ChartofAccounts ";
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
                    treeView1.Nodes[5].Nodes[3].Nodes.Clear();
                }
                else
                {
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
                    treeView1.Nodes[5].Nodes[3].Nodes.Clear();
                }
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string type = dr["AccountType"].ToString();
                    string subaccount = dr["SubAccount"].ToString();
                    if (type == "Fixed Assets")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0;
                            int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[0].Nodes[0].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;
                                    
                                }
                                i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[0].Nodes[0].Nodes.Add(subaccount); index = i;
                            }
                            
                            treeView1.Nodes[0].Nodes[0].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[0].Nodes[0].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Current Assets")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0;
                            int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[0].Nodes[1].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                }
                                i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[0].Nodes[1].Nodes.Add(subaccount);
                                index = i;
                            }
                            
                            treeView1.Nodes[0].Nodes[1].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[0].Nodes[1].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Current Liabilities")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[1].Nodes[1].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;
                                }
                                i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[1].Nodes[1].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[1].Nodes[1].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[1].Nodes[1].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Long Term Liabilities")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[1].Nodes[0].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;
                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[1].Nodes[0].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[1].Nodes[0].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {

                            treeView1.Nodes[1].Nodes[0].Nodes.Add(dr["Name"].ToString());
                        }
                    }

                    if (type == "Equity and Capital")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[2].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[2].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[2].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[2].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Revenue")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[3].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[3].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[3].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[3].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Cost of Sales")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[4].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[4].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[4].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[4].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Operating Expenses")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[5].Nodes[0].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[5].Nodes[0].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[5].Nodes[0].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[5].Nodes[0].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Admin and Selling Expenses")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[5].Nodes[1].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[5].Nodes[1].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[5].Nodes[1].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[5].Nodes[1].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Financial Expenses")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[5].Nodes[2].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[5].Nodes[2].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[5].Nodes[2].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[5].Nodes[2].Nodes.Add(dr["Name"].ToString());
                        }
                    }
                    if (type == "Marketing Expenses")
                    {
                        if (subaccount.Trim().Length > 0)
                        {
                            bool chkfind = false;
                            int index = 0; int i = 0;
                            foreach (TreeNode node in treeView1.Nodes[5].Nodes[3].Nodes)
                            {
                                if (node.Text.ToUpper().Contains(subaccount.Trim().ToUpper()))
                                {
                                    treeView1.SelectedNode = node;
                                    chkfind = true;
                                    index = node.Index;


                                } i++;
                            }
                            if (chkfind == false)
                            {
                                treeView1.Nodes[5].Nodes[3].Nodes.Add(subaccount); index = i;
                            }

                            treeView1.Nodes[5].Nodes[3].Nodes[index].Nodes.Add(dr["Name"].ToString());
                        }
                        else
                        {
                            treeView1.Nodes[5].Nodes[3].Nodes.Add(dr["Name"].ToString());
                        }
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
                string q = "select * from ChartofAccounts where Name='" + name + "' ";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    txtcode.Text = ds.Tables[0].Rows[0]["AccountCode"].ToString();
                    txtdesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                    cmbtype.Text = ds.Tables[0].Rows[0]["AccountType"].ToString();
                    cmbstatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                    id = ds.Tables[0].Rows[0]["id"].ToString();
                    //int count = textBox1.Text.Length;
                    //if (count > 0)
                    //{
                    //    txtcode.Text = txtcode.Text.Substring(count);
                    //}
                    string subaccount = ds.Tables[0].Rows[0]["SubAccount"].ToString();
                    cmbsubaccount.SelectedValue = subaccount;
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
            try
            {
                //string q = "select * from Branch";
                //DataSet ds = objcore.funGetDataSet(q);
                //DataRow dr = ds.Tables[0].NewRow();

                //cmbbranch1.DataSource = ds.Tables[0];
                //cmbbranch1.ValueMember = "id";
                //cmbbranch1.DisplayMember = "BranchName";
                //cmbstatus.SelectedItem = "Active";
            }
            catch (Exception ex)
            {
                
               
            }
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
                    cmbtype.Focus();
                    return;
                }
                if (txtname.Text == string.Empty)
                {
                    MessageBox.Show("Please Eneter Name");
                    txtname.Focus();
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
                    txtcode.Focus();
                    return;
                }
                //if (txtdesc.Text == string.Empty)
                //{
                //    MessageBox.Show("Please Eneter Description");
                //    return;
                //}
                //if (cmbbranch.Text == string.Empty)
                //{
                //    MessageBox.Show("Please Select Branch");
                //    cmbbranch.Focus();
                //    return;
                //}
                if (cmbstatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Activation Status");
                    cmbstatus.Focus();
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
                string q = "select * from ChartofAccounts where  ( Name='" + txtname.Text.Trim() + "' or AccountCode='" + textBox1.Text.Trim() + txtcode.Text.Trim() + "')";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Account Name or Code Already Exist");
                    return;
                }
                objcore.executeQuery("insert into ChartofAccounts (SubAccount,id,AccountType,Name,AccountCode,Description,Status) values ('" + cmbsubaccount.Text + "','" + idd + "','" + cmbtype.Text + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtcode.Text.Trim().Replace("'", "''") + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text + "')");
                MessageBox.Show("Data Saved Successfully");
                getsubaccounts();
                getdata();

                txtname.Text = "";
                getcode();
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
                //if (cmbbranch1.Text == string.Empty)
                //{
                //    MessageBox.Show("Please Select Branch");
                //    return;
                //}
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
                objcore.executeQuery("update ChartofAccounts set SubAccount='"+cmbsubaccount.Text+"', AccountType='" + cmbtype.Text + "', Name='" + txtname.Text + "', AccountCode='" + txtcode.Text + "',Description='" + txtdesc.Text + "',Status='" + cmbstatus.Text + "' where id='" + id + "'");
                MessageBox.Show("Data Updated Successfully");
                getsubaccounts();
                getdata();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected void getsubaccounts()
        {
            try
            {
                string q = "select distinct SubAccount from ChartofAccounts where AccountType='" + cmbtype.Text + "' and ISNULL(LEN(SubAccount), '') > 0";
                DataSet ds = objcore.funGetDataSet(q);
               

                cmbsubaccount.DataSource = ds.Tables[0];
                cmbsubaccount.ValueMember = "SubAccount";
                cmbsubaccount.DisplayMember = "SubAccount";
            }
            catch (Exception ex)
            {


            }
        }
        protected void getcode()
        {           
                string code = "001";
                try
                {
                    DataSet ds = new DataSet();
                    string q = "select * from ChartofAccounts where Name='" + txtname.Text + "' ";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        q = "select max(CAST(AccountCode as int)+1) as code from ChartofAccounts where AccountType='" + cmbtype.Text + "'";
                        ds = objcore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            code = "00" + ds.Tables[0].Rows[0][0].ToString();
                            
                        }
                        txtcode.Text = code;
                    }
                }
                catch (Exception ex)
                {

                }
               
            

        }
        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            getcode();
            
            getsubaccounts();
        }

        private void cmbtype_SelectedValueChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet dsx = new System.Data.DataSet();
                string q = "select * from ChartofAccountsCodes where Name='" + cmbtype.Text + "' ";
                dsx = objcore.funGetDataSet(q);
                if (dsx.Tables[0].Rows.Count > 0)
                {
                    textBox1.Text = dsx.Tables[0].Rows[0]["Code"].ToString();

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
            DialogResult dr = MessageBox.Show("Are you sure to delete?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }
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

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void button5_Click(object sender, EventArgs e)
        {

        }
    }
}
