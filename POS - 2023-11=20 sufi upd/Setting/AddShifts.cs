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
    public partial class AddShifts : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddShifts(POSRestaurant.forms.MainForm frm)
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
                string q = "select * from Shifts where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();                    
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
                    MessageBox.Show("Please Enter Name of Shift");
                    return;
                }
                
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Shifts");
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
                    string q = "select * from Shifts where name='" + txtName.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Shift Name already exist");
                        return;
                    }

                    q = "insert into Shifts (id,name) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Shifts set name='" + txtName.Text.Trim().Replace("'", "''") + "'  where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Shifts");
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
                                                                                                                                                                                                               ug\POSRestaurant.Sale.CustomerId.resources
E:\POS\obj\Debug\POSRestaurant.Sale.Delivery.resources
E:\POS\obj\Debug\POSRestaurant.Sale.Modifier.resources
E:\POS\obj\Debug\POSRestaurant.Sale.test.resources
E:\POS\obj\Debug\POSRestaurant.Sale.VoidBill.resources
E:\POS\obj\Debug\POSRestaurant.Sale.DuplicaeBill.resources
E:\POS\obj\Debug\POSRestaurant.Sale.CustomerDisplay.resources
E:\POS\obj\Debug\POSRestaurant.Sale.Sale1.resources
E:\POS\obj\Debug\POSRestaurant.Setting.AddGroups.resources
E:\POS\obj\Debug\POSRestaurant.Setting.AddCategory.resources
E:\POS\obj\Debug\POSRestaurant.Setting.AddType.resources
E:\POS\obj\Debug\POSRestaurant.Setting.Addbrand.resources
E:\POS\ob