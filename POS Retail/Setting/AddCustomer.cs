using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Setting
{
    public partial class AddCustomer : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        POSRetail.forms.MainForm _frm;
        public AddCustomer(POSRetail.forms.MainForm frm)
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
                DataSet ds = new DataSet();
                string q = "select id,name from ChartofAccounts where AccountType='Current Assets'";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "Please Select";
                ds.Tables[0].Rows.Add(dr);
                cmbaccount.DataSource = ds.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "name";
                cmbaccount.Text = "Please Select";
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
            if (editmode == 1)
            {

                string q = "select * from Customers where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtcity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    txtemail.Text = ds.Tables[0].Rows[0]["Email"].ToString();

                    txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtmobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();

                    cmbaccount.SelectedValue = ds.Tables[0].Rows[0]["Chartaccountid"].ToString();
                    vButton2.Text = "Update";
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
                    MessageBox.Show("Please Enter Name");
                    return;
                }
                if (txtcity.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter City");
                    return;
                }
                if (txtphone.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Phone No");
                    return;
                }
                if (txtmobile.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Mobile No");
                    return;
                }
                if (txtaddress.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Address");
                    return;
                }
                if (cmbaccount.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Account");
                    return;
                }
                if (editmode == 0)
                {

                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                    string q = "select * from Customers where name='" + txtName.Text.Trim() + "' and Mobile='" + txtmobile.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Customers already exist");
                        return;
                    }

                    q = "insert into Customers (id,Name,Email,Phone,City,Address,Chartaccountid,Mobile,uploadstatus) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtemail.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtcity.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + cmbaccount.SelectedValue.ToString().Trim().Replace("'", "''") + "','" + txtmobile.Text.Trim().Replace("'", "''") + "','Pending')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Customers set Mobile='" + txtmobile.Text.Trim().Replace("'", "''") + "' ,Chartaccountid='" + cmbaccount.SelectedValue + "' ,Address='" + txtaddress.Text.Trim().Replace("'", "''") + "' ,Phone='" + txtphone.Text.Trim().Replace("'", "''") + "' ,Name='" + txtName.Text.Trim().Replace("'", "''") + "' , City ='" + txtcity.Text.Trim().Replace("'", "''") + "', Email ='" + txtemail.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT     dbo.Customers.Id, dbo.Customers.Name, dbo.Customers.Email, dbo.Customers.Phone, dbo.Customers.Mobile, dbo.Customers.City, dbo.Customers.Address, dbo.ChartofAccounts.Name AS Account FROM         dbo.ChartofAccounts INNER JOIN                      dbo.Customers ON dbo.ChartofAccounts.Id = dbo.Customers.Chartaccountid");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcity.Text = string.Empty;
            txtName.Text = string.Empty;
            txtemail.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtmobile.Text = string.Empty;
            
            cmbaccount.SelectedText = "Please Select";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtcard_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtmobile.Text = txtmobile.Text + e.KeyChar.ToString().Trim();
        }

        private void txtcard_TextChanged(object sender, EventArgs e)
        {
            //txtcard.Text=txtcard.Text.Replace("\n", "");
            //txtcard.Text.Reverse();
        }
    }
}
