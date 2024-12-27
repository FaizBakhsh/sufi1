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
    public partial class AddUser : Form
    {
        POSRetail.forms.MainForm _frm;
        public AddUser(POSRetail.forms.MainForm frm)
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
                cmbtype.Text = "Please Select";
                if (editmode == 1)
                {
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "select * from users where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                        txtcnic.Text = ds.Tables[0].Rows[0]["CNICNo"].ToString();
                        txtfathername.Text = ds.Tables[0].Rows[0]["FatherName"].ToString();

                        txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                        txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                        txtcard.Text = ds.Tables[0].Rows[0]["CardNo"].ToString();
                        txtusername.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                        txtpassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                        txtdesignation.Text = ds.Tables[0].Rows[0]["Designation"].ToString();
                        cmbtype.Text = ds.Tables[0].Rows[0]["Usertype"].ToString();
                        vButton2.Text = "Update";
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                if (txtcnic.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Cnic no");
                    return;
                }
                if (txtphone.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Phone No");
                    return;
                }
                if (txtusername.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Username");
                    return;
                }
                if (txtpassword.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Password");
                    return;
                }
                if (cmbtype.Text.Trim() == "Please Select" || cmbtype.Text.Trim()==string.Empty)
                {
                    MessageBox.Show("Please Select User type");
                    return;
                }
                if (editmode == 0)
                {

                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Users");
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
                    string q = "select * from Users where name='" + txtName.Text.Trim() + "' and CNICNo='"+txtcnic.Text+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("User already exist");
                        return;
                    }

                    q = "insert into Users (id,Name,FatherName,Phone,CNICNo,Address,Usertype,CardNo,UserName,Password,Designation,uploadstatus) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtfathername.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtcnic.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + cmbtype.Text.Trim().Replace("'", "''") + "','" + txtcard.Text.Trim().Replace("'", "''") + "','" + txtusername.Text.Trim().Replace("'", "''") + "','" + txtpassword.Text.Trim().Replace("'", "''") + "','" + txtdesignation.Text.Trim().Replace("'", "''") + "','Pending')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Users set Designation='" + txtdesignation.Text.Trim().Replace("'", "''") + "' ,Password='" + txtpassword.Text.Trim().Replace("'", "''") + "' ,UserName='" + txtusername.Text.Trim().Replace("'", "''") + "' ,CardNo='" + txtcard.Text.Trim().Replace("'", "''") + "' ,Usertype='" + cmbtype.Text.Trim().Replace("'", "''") + "' ,Address='" + txtaddress.Text.Trim().Replace("'", "''") + "' ,Phone='" + txtphone.Text.Trim().Replace("'", "''") + "' ,Name='" + txtName.Text.Trim().Replace("'", "''") + "' , CNICNo ='" + txtcnic.Text.Trim().Replace("'", "''") + "', FatherName ='" + txtfathername.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Users");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcnic.Text = string.Empty;
            txtName.Text = string.Empty;
            txtfathername.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtcard.Text = string.Empty;
            txtusername.Text = string.Empty;
            txtpassword.Text = string.Empty;
            txtdesignation.Text = string.Empty;
            cmbtype.SelectedText = "Please Select";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtcard_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtcard.Text = txtcard.Text + e.KeyChar.ToString().Trim();
        }

        private void txtcard_TextChanged(object sender, EventArgs e)
        {
            //txtcard.Text=txtcard.Text.Replace("\n", "");
            //txtcard.Text.Reverse();
        }
    }
}
