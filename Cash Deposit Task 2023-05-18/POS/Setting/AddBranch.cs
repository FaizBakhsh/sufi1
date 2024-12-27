using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class Addbranch : Form
    {
        POSRestaurant.forms.MainForm _frm;
        
        public Addbranch(POSRestaurant.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
                
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from ChartofAccounts";
                ds = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
            }
            catch (Exception ex)
            {

            }
            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["BranchName"].ToString();
                    txtdescription.Text = ds.Tables[0].Rows[0]["BranchCode"].ToString();
                    txtlocation.Text = ds.Tables[0].Rows[0]["Location"].ToString();
                    vButton2.Text = "Update";
                    try
                    {
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Branch");
                    return;
                }
                if (txtdescription.Text.Trim() == string.Empty)
                {
                    //MessageBox.Show("Please Enter Code of Branch");
                    //return;
                }
                if (txtlocation.Text.Trim() == string.Empty)
                {
                    //MessageBox.Show("Please Enter Location of Branch");
                    //return;
                }
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Branch");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        id = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        id = 1;
                    }
                    ds = new DataSet();
                    string q = "select * from Branch where BranchName='" + txtName.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Branch Name already exist");
                        return;
                    }

                    q = "insert into Branch ( id,BranchName,BranchCode,Location) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "','" + txtlocation.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string accountid = "0";
                    try
                    {
                        accountid = comboBox1.SelectedValue.ToString();
                    }
                    catch (Exception exxx)
                    {
                        
                        
                    }

                    string q = "update Branch set ChartaccountId='" + accountid + "' ,BranchName='" + txtName.Text.Trim().Replace("'", "''") + "' , BranchCode ='" + txtdescription.Text.Trim().Replace("'", "''") + "', Location ='" + txtlocation.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Branch");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtdescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtlocation.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
