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
    public partial class AddSource : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddSource(POSRestaurant.forms.MainForm frm)
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
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                string query = "ALTER TABLE [dbo].[ordersource]  ADD printinvoice varchar(50) NULL ";
                objCore.executeQueryint(query);

            }
            catch (Exception ex)
            {


            }
            if (editmode == 1)
            {
                
                DataSet ds = new DataSet();
                string q = "select * from ordersource where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    comboBox1.Text = ds.Tables[0].Rows[0]["status"].ToString();      
                    vButton2.Text = "Update";
                }
            }
        }
        protected void updateoldsource(string name)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            string q = "update Delivery set type='" + txtName.Text + "' where type='" + name + "'";
            objCore.executeQuery(q);
            
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
                if (comboBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    return;
                }
                if (txtamount.Text.Length > 0)
                {

                    if (txtamount.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Value. Only Numbers are allowed");
                            txtamount.Focus();
                            return;
                        }
                    }
                }
                else
                {
                    txtamount.Text = "0";
                }
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    
                    ds = new DataSet();
                    string q = "select * from ordersource where name='" + txtName.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Name already exist");
                        return;
                    }

                    q = "insert into ordersource (printinvoice,amount,status,name) values('" + cmbinvoice.Text + "','" + txtamount.Text + "','" + comboBox1.Text + "','" + txtName.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string oldname = "";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "select * from ordersource where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        oldname = ds.Tables[0].Rows[0]["Name"].ToString();
                       
                    }
                    q = "update ordersource set printinvoice='" + cmbinvoice.Text + "', amount='" + txtamount.Text + "',status='" + comboBox1.Text + "',name='" + txtName.Text.Trim().Replace("'", "''") + "'  where id='" + id + "'";
                    
                   int res= objCore.executeQueryint(q);
                   if (res > 0)
                   {
                       updateoldsource(oldname);
                   }
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from ordersource");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            
            txtName.Text = string.Empty;
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
                                                                                                                                                                                                               
