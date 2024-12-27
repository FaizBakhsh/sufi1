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
    public partial class DiscountCompaign : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public DiscountCompaign(POSRestaurant.forms.MainForm frm)
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
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,Name from menugroup ";
                ds1 = objCore.funGetDataSet(q);
                
                cmbgroup.DataSource = ds1.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "Name";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            if (editmode == 1)
            {
               
                DataSet ds = new DataSet();
                string q = "select * from DiscountCompaign where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtperc.Text = ds.Tables[0].Rows[0]["discount"].ToString();
                    txtstartdate.Text = ds.Tables[0].Rows[0]["Datefrom"].ToString();
                    txtenddate.Text = ds.Tables[0].Rows[0]["DateTo"].ToString();
                    txtstarttime.Text = ds.Tables[0].Rows[0]["TimeFrom"].ToString();
                    txtendtime.Text = ds.Tables[0].Rows[0]["TimeTo"].ToString();

                    try
                    {
                        chkMonday.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["monday"]);
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    try
                    {
                        chkTuesday.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["tuesday"]);
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    try
                    {
                        chkWednesday.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["wednesday"]);
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    try
                    {
                        chkthursday.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["thursday"]);
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                    try
                    {
                        chkFriday.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["friday"]);
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    try
                    {
                        chkSaturday.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["saturday"]);
                    }
                    catch (Exception ex)
                    {
                                                
                    }
                    try
                    {
                        chkSunday.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["sunday"]);
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                    try
                    {
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
                    }
                    catch (Exception ex)
                    {
                                                
                    }
                    try
                    {
                        cmbtype.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        cmbgroup.SelectedValue = ds.Tables[0].Rows[0]["groupid"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }    
                    vButton2.Text = "Update";
                }
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        public void gst()
        {

            
        }
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter name");
                    return;
                }
                if (txtperc.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Discount Percentage");
                    return;
                }
                if (txtperc.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtperc.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Numbers are allowed");
                        return;
                    }
                }
                if (cmbtype.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please select Type");
                    return;
                }
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "";
                if (editmode == 0)
                {

                    ds = new DataSet();
                    q = "select * from DiscountCompaign where name='" + txtname.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Discount Name already exist");
                        return;
                    }
                    if (checkBox1.Checked == true)
                    {
                        q = "insert into DiscountCompaign (groupid,type,Name, Discount, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, AllDay, Datefrom, DateTo, TimeFrom, Status) values('" + cmbgroup.SelectedValue + "','" + cmbtype.Text + "','" + txtname.Text + "','" + txtperc.Text.Trim().Replace("'", "''") + "','" + chkMonday.Checked + "','" + chkTuesday.Checked + "','" + chkWednesday.Checked + "','" + chkthursday.Checked + "','" + chkFriday.Checked + "','" + chkSaturday.Checked + "','" + chkSunday.Checked + "','','" + txtstartdate.Text + "','" + txtenddate.Text + "','" + txtstarttime.Text + "','" + comboBox1.Text + "')";
              
                    }
                    else
                    {
                        q = "insert into DiscountCompaign (groupid,type,Name, Discount, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, AllDay, Datefrom, DateTo, TimeFrom, TimeTo,Status) values('" + cmbgroup.SelectedValue + "','" + cmbtype.Text + "','" + txtname.Text + "','" + txtperc.Text.Trim().Replace("'", "''") + "','" + chkMonday.Checked + "','" + chkTuesday.Checked + "','" + chkWednesday.Checked + "','" + chkthursday.Checked + "','" + chkFriday.Checked + "','" + chkSaturday.Checked + "','" + chkSunday.Checked + "','','" + txtstartdate.Text + "','" + txtenddate.Text + "','" + txtstarttime.Text + "','" + txtendtime.Text + "','" + comboBox1.Text + "')";
                    }
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();

                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    if (checkBox1.Checked == true)
                    {
                        q = "update DiscountCompaign set groupid='" + cmbgroup.SelectedValue + "',type='" + cmbtype.Text.Trim().Replace("'", "''") + "',Name='" + txtname.Text.Trim().Replace("'", "''") + "', Discount='" + txtperc.Text.Trim().Replace("'", "''") + "', Monday='" + chkMonday.Checked + "', Tuesday='" + chkTuesday.Checked + "', Wednesday='" + chkWednesday.Checked + "', Thursday='" + chkthursday.Checked + "', Friday='" + chkFriday.Checked + "', Saturday='" + chkSaturday.Checked + "', Sunday='" + chkSunday.Checked + "',  Datefrom='" + txtstartdate.Text + "', DateTo='" + txtenddate.Text + "', TimeFrom='" + txtstarttime.Text + "', TimeTo=null,Status='" + comboBox1.Text + "' where id='" + id + "'";
                    }
                    else
                    {
                        q = "update DiscountCompaign set groupid='" + cmbgroup.SelectedValue+ "',type='" + cmbtype.Text.Trim().Replace("'", "''") + "',Name='" + txtname.Text.Trim().Replace("'", "''") + "', Discount='" + txtperc.Text.Trim().Replace("'", "''") + "', Monday='" + chkMonday.Checked + "', Tuesday='" + chkTuesday.Checked + "', Wednesday='" + chkWednesday.Checked + "', Thursday='" + chkthursday.Checked + "', Friday='" + chkFriday.Checked + "', Saturday='" + chkSaturday.Checked + "', Sunday='" + chkSunday.Checked + "',  Datefrom='" + txtstartdate.Text + "', DateTo='" + txtenddate.Text + "', TimeFrom='" + txtstarttime.Text + "', TimeTo='" + txtendtime.Text + "',Status='" + comboBox1.Text + "' where id='" + id + "'";
                    }
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from DiscountCompaign");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtname.Text = string.Empty;
            txtperc.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtperc_TextChanged(object sender, EventArgs e)
        {
            if (txtperc.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtperc.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Numbers are allowed");
                    return;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtendtime.Enabled = false;
            }
            else
            {
                txtendtime.Enabled = true;
            }
        }
    }
}
