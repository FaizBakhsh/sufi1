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
    public partial class AddStaff : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddStaff(POSRestaurant.forms.MainForm frm)
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
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();

            DataSet ds = new DataSet();
            string q = "select * from users where usertype='Cashier'";
            try
            {
                ds = objCore.funGetDataSet(q);

                cmbcashier.DataSource = ds.Tables[0];
                cmbcashier.ValueMember = "id";
                cmbcashier.DisplayMember = "Name";
            }
            catch (Exception ex)
            {

            }



            ds = new DataSet();
            q = "select * from ChartofAccounts";

            if (editmode == 1)
            {
                ds = new DataSet();
                q = "select * from ResturantStaff where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();

                    txtfathername.Text = ds.Tables[0].Rows[0]["FatherName"].ToString();

                    cmbtype.SelectedValue = ds.Tables[0].Rows[0]["Usertype"].ToString();
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
               
                 string cashierid = "0";
                 if (cmbcashier.Visible == true)
                 {
                     cashierid = cmbcashier.SelectedValue.ToString();
                 }
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from ResturantStaff");
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
                    string q = "select * from ResturantStaff where name='" + txtName.Text.Trim() + "' and FatherName='" + txtfathername.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("User  already exist");
                        return;
                    }
                    ds = new DataSet();
                   
                   
                    q = "insert into ResturantStaff (cashierid,id,Name,FatherName,Usertype) values('" + cashierid + "','" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtfathername.Text.Trim().Replace("'", "''") + "','" + cmbtype.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update ResturantStaff set cashierid='" + cashierid.Trim().Replace("'", "''") + "' ,Usertype='" + cmbtype.Text.Trim().Replace("'", "''") + "' ,Name='" + txtName.Text.Trim().Replace("'", "''") + "' ,  FatherName ='" + txtfathername.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from ResturantStaff");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           
            txtName.Text = string.Empty;
            txtfathername.Text = string.Empty;
           
            //btype.SelectedText = "Please Select";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtcard_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtcard_TextChanged(object sender, EventArgs e)
        {
            //txtcard.Text=txtcard.Text.Replace("\n", "");
            //txtcard.Text.Reverse();
        }

        private void cmbtype_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmbtype.Text == "Waiter")
            {
                cmbcashier.Visible = true;
                
                
            }
            else
            {
                cmbcashier.Visible = false;
                checkBox1.Visible = false;
            }
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                cmbcashier.Visible = true;
            }
            else
            {
                cmbcashier.Visible = false;
               
            }
        }
    }
}
