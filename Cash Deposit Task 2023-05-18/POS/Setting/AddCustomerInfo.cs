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
    public partial class AddCustomerInfo : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddCustomerInfo(POSRestaurant.forms.MainForm frm)
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
        protected void fillaccount()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from ChartofAccounts where name like '%receivable%'";
                ds = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
            }
            catch (Exception ex)
            {

            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from Branch ";
            ds = objCore.funGetDataSet(q);
            comboBox2.DataSource = ds.Tables[0];
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "BranchName";
            fillaccount();
            try
            {
                if (editmode == 1)
                {
                   
                    ds = new DataSet();
                    q = "select * from Customers where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                        txtaddress.Text = ds.Tables[0].Rows[0]["address"].ToString();
                        txtphone.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                        txtcity.Text = ds.Tables[0].Rows[0]["city"].ToString();
                        txtcnic.Text = ds.Tables[0].Rows[0]["cnic"].ToString();
                        txtcompany.Text = ds.Tables[0].Rows[0]["company"].ToString();
                        txtmsr.Text = ds.Tables[0].Rows[0]["msr"].ToString();
                        vButton2.Text = "Update";
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["Chartaccountid"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Customer");
                    return;
                }
                //if (txtphone.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Please Enter Contact Nos of Customer");
                //    return;
                //}
                //if (txtcnic.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Please Enter CNIC No of Customer");
                //    return;
                //}
                
                //if (txtmsr.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Please Enter MSR No of Customer");
                //    return;
                //}
                //if (txtcity.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Please Enter City of Customer");
                //    return;
                //}
                
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Customers");
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
                    string q = "select * from Customers where Cnic='" + txtcnic.Text.Trim() + "' or Msr='"+txtmsr.Text.Trim()+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Customers CNIC/MSR no already exist");
                        return;
                    }

                    q = "insert into Customers (branchid,Chartaccountid,id,Name,Cnic,phone,Company,Msr,City,address,Uploadstatus) values('" + comboBox2.SelectedValue + "','" + comboBox1.SelectedValue + "','" + id + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtcnic.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtcompany.Text.Trim().Replace("'", "''") + "','" + txtmsr.Text.Trim().Replace("'", "''") + "','" + txtcity.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','Pending')";
                    objCore.executeQuery(q);
                    //POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    _frm.getdata("select * from Customers");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Customers set branchid='" + comboBox2.SelectedValue + "',Chartaccountid='" + comboBox1.SelectedValue + "',city='" + txtcity.Text.Trim().Replace("'", "''") + "' ,msr='" + txtmsr.Text.Trim().Replace("'", "''") + "' ,company='" + txtcompany.Text.Trim().Replace("'", "''") + "' ,cnic='" + txtcnic.Text.Trim().Replace("'", "''") + "' ,Name='" + txtname.Text.Trim().Replace("'", "''") + "' , address ='" + txtaddress.Text.Trim().Replace("'", "''") + "', phone ='" + txtphone.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    _frm.getdata("select * from Customers");
                    MessageBox.Show("Record updated successfully");
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtaddress.Text = string.Empty;
            txtname.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtcnic.Text = string.Empty;
            txtcity.Text = string.Empty;
            txtcompany.Text = string.Empty;
            txtmsr.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillaccount();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
