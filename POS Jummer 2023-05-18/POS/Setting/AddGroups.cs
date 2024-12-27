﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class AddGroups : Form
    {
         POSRestaurant.forms.MainForm _frm;
         public AddGroups(POSRestaurant.forms.MainForm frm)
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
            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from groups where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["groupName"].ToString();
                    txtdescription.Text = ds.Tables[0].Rows[0]["description"].ToString();
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
                    MessageBox.Show("Please Enter Name of Group");
                    return;
                }
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
               
                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from groups");
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
                    string q = "select * from groups where GroupName='" + txtName.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Group Name already exist");
                        return;
                    }
                    q = "insert into Groups (id,GroupName,Description) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Groups set GroupName='" + txtName.Text.Trim().Replace("'", "''") + "' , Description ='" + txtdescription.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Groups");
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
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
