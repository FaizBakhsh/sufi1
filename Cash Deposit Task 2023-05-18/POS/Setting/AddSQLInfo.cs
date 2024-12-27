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
    public partial class AddSQLInfo : Form
    {
        POSRestaurant.forms.MainForm _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public AddSQLInfo(POSRestaurant.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
              
        }

       

        private void btnexit_Click(object sender, EventArgs e)
        {
          
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {

            if (editmode == 1)
            {
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                string q = "select * from SqlServerInfo where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["ServerName"].ToString();
                    txtdbname.Text = ds.Tables[0].Rows[0]["DbName"].ToString();
                    txtusername.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                    txtpassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                    vButton2.Text = "Update";
                }
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of SQl Server");
                    return;
                }
                if (txtdbname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Database");
                    return;
                }
                if (txtusername.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter User Name of Database");
                    return;
                }
                if (txtpassword.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Password of Database");
                    return;
                }
               

                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from SqlServerInfo");
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
                    string q = "select * from SqlServerInfo where ServerName='" + txtName.Text.Trim().Replace("'", "''") + "' and DbName='" + txtdbname.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Record Name already exist");
                        return;
                    }
                    q = "insert into SqlServerInfo (id,ServerName,DbName,UserName,Password) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtdbname.Text.Trim().Replace("'", "''") + "','" + txtusername.Text.Trim().Replace("'", "''") + "','" + txtpassword.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                   
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update SqlServerInfo set ServerName='" + txtName.Text.Trim().Replace("'", "''") + "', DbName='" + txtName.Text.Trim().Replace("'", "''") + "' , UserName ='" + txtusername.Text.Trim().Replace("'", "''") + "',password='" + txtpassword.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT  * from  SqlServerInfo  ");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtdbname.Text = string.Empty;
            txtusername.Text = string.Empty;
            txtpassword.Text = string.Empty;
           
            txtName.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
