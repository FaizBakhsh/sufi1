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
    public partial class AddCustomerInfo2 : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddCustomerInfo2(POSRestaurant.forms.MainForm frm)
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
                if (editmode == 1)
                {
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "select * from Customers1 where id='" + id + "'";
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
                        txtpoints.Text = ds.Tables[0].Rows[0]["points"].ToString();
                        vButton2.Text = "Update";
                        dateTimePicker1.Text = ds.Tables[0].Rows[0]["date"].ToString();
                        dateTimePicker2.Text = ds.Tables[0].Rows[0]["dob"].ToString();
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

                if (txtmsr.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter MSR No of Customer");
                    return;
                }
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
                    ds = objCore.funGetDataSet("select max(id) as id from Customers1");
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
                    string q = "select * from Customers1 where  Msr='"+txtmsr.Text.Trim()+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Customers MSR no already exist");
                        return;
                    }

                    q = "insert into Customers1 (date,Points,id,Name,Cnic,phone,Company,Msr,City,address,Uploadstatus) values('"+dateTimePicker1.Text+"','" + txtpoints.Text.Trim().Replace("'", "''") + "' ,'" + id + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtcnic.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtcompany.Text.Trim().Replace("'", "''") + "','" + txtmsr.Text.Trim().Replace("'", "''") + "','" + txtcity.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','Pending')";
                    objCore.executeQuery(q);
                    try
                    {
                        string points = txtpoints.Text;
                        if (points == "")
                        {
                            points = "0";
                        }
                        if (Convert.ToDouble(points) > 0)
                        {
                            q = "insert into CustomerPoints(Customerid,Points,saleid) values('" + id + "','" + txtpoints.Text + "','0')";
                            objCore.executeQuery(q);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                      
                    }
                    //POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    _frm.getdata("select * from Customers1");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Customers1 set date='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "' ,Points='" + txtpoints.Text.Trim().Replace("'", "''") + "' ,city='" + txtcity.Text.Trim().Replace("'", "''") + "' ,msr='" + txtmsr.Text.Trim().Replace("'", "''") + "' ,company='" + txtcompany.Text.Trim().Replace("'", "''") + "' ,cnic='" + txtcnic.Text.Trim().Replace("'", "''") + "' ,Name='" + txtname.Text.Trim().Replace("'", "''") + "' , address ='" + txtaddress.Text.Trim().Replace("'", "''") + "', phone ='" + txtphone.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    q = "delete from CustomerPoints where Customerid='"+id+"' and saleid='0'";
                    objCore.executeQuery(q);
                    try
                    {
                        string points = txtpoints.Text;
                        if (points == "")
                        {
                            points = "0";
                        }
                        if (Convert.ToDouble(points) > 0)
                        {
                            q = "insert into CustomerPoints(Customerid,Points,saleid) values('" + id + "','" + txtpoints.Text + "','0')";
                            objCore.executeQuery(q);
                        }
                    }
                    catch (Exception ex)
                    {


                    }


                    _frm.getdata("select * from Customers1");
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
    }
}
