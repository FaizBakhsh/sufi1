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
    public partial class DevicesSetting : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public string id="";
        public DevicesSetting(POSRestaurant.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
           
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


            comboBox1.Text = "Please Select";
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "Please Select" || comboBox1.Text=="")
                {
                    MessageBox.Show("Please Select Device");
                    return;
                }
                //if (comboBox2.Text.Trim() == "")
                //{
                //    MessageBox.Show("Please Select Screen No");
                //    return;
                //}
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();

                //if (comboBox1.SelectedValue == "KOT")
                {
                    ds = new DataSet();
                    string q = "select * from DeviceSetting where Device='" + comboBox1.Text + "' and terminal='" + textBox1.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        q = "update DeviceSetting set terminal='" + textBox1.Text + "',Status='Enabled',no='" + comboBox2.Text + "'  where id='" + ds.Tables[0].Rows[0]["id"].ToString() + "'";
                        objCore = new classes.Clsdbcon();
                        objCore.executeQuery(q);
                        MessageBox.Show("Record updated successfully");
                    }
                    else
                    {
                        int id = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from DeviceSetting");
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

                        q = "insert into DeviceSetting (id,Device,no,Status,terminal) values('" + id + "','" + comboBox1.Text + "','" + comboBox2.Text + "','Enabled','"+textBox1.Text+"')";
                        objCore.executeQuery(q);
                        POSRestaurant.forms.MainForm obj = new forms.MainForm();
                        
                        MessageBox.Show("Record saved successfully");
                    }
                }

                _frm.getdata("select * from DeviceSetting");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Device");
                    return;
                }
                //if (comboBox1.SelectedValue == "Customer Display")
                {
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    ds = new DataSet();
                    string q = "select * from DeviceSetting where Device='" + comboBox1.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        q = "update DeviceSetting set terminal='"+textBox1.Text+"', Status='Disabled'  where id='" + ds.Tables[0].Rows[0]["id"].ToString() + "'";
                        objCore = new classes.Clsdbcon();
                        objCore.executeQuery(q);
                        MessageBox.Show("Record updated successfully");
                    }
                    else
                    {
                        int id = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from DeviceSetting");
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

                        q = "insert into DeviceSetting (id,Device,Status,terminal) values('" + id + "','" + comboBox1.Text + "','Disabled','"+textBox1.Text+"')";
                        objCore.executeQuery(q);
                        POSRestaurant.forms.MainForm obj = new forms.MainForm();

                        MessageBox.Show("Record saved successfully");
                    }
                    _frm.getdata("select * from DeviceSetting");
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
