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
    public partial class Addemployees : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public Addemployees(POSRestaurant.forms.MainForm frm)
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

        protected void fillaccount()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from ChartofAccounts where name like '%salary%' and AccountType='Current Liabilities' ";
                ds = objCore.funGetDataSet(q);
                cmbaccount.DataSource = ds.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "name";
            }
            catch (Exception ex)
            {

            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from Branch ";
            ds = objCore.funGetDataSet(q);
            cmbbranch.DataSource = ds.Tables[0];
            cmbbranch.ValueMember = "id";
            cmbbranch.DisplayMember = "BranchName";
            fillaccount();
            if (editmode == 1)
            {
                ds = new DataSet();
                q = "select * from employees where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = ds.Tables[0].Rows[0]["Name"].ToString();

                    txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtcode.Text = ds.Tables[0].Rows[0]["EmpId"].ToString();
                    dateTimePicker1.Text = ds.Tables[0].Rows[0]["JoiningDate"].ToString();

                    txtdesignation.Text = ds.Tables[0].Rows[0]["Designation"].ToString();
                    try
                    {
                        cmbaccount.SelectedValue = ds.Tables[0].Rows[0]["payableaccountid"].ToString();
                    }
                    catch (Exception ex)
                    {
                       
                    }
                    try
                    {
                        cmbaccount.SelectedValue = ds.Tables[0].Rows[0]["status"].ToString();
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
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void vButton2_Click(object sender, EventArgs e)
        {
            
           
            try
            {
                if (txtcode.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Cnic no");
                    return;
                }
                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name");
                    return;
                }
                
                if (txtphone.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Phone No");
                    return;
                }
                if (cmbstatus.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    return;
                }
                
               
                if (editmode == 0)
                {

                  
                    DataSet ds = new DataSet();
                    
                    ds = new DataSet();
                    string q = "select * from employees where name='" + txtname.Text.Trim() + "' and empid='" + txtcode.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("employee already exist");
                        return;
                    }

                    q = "insert into employees (branchid,status,payableaccountid,EmpId, Name, Phone, Designation, JoiningDate) values('" + cmbbranch.SelectedValue + "','" + cmbstatus.Text + "','" + cmbaccount.SelectedValue + "','" + txtcode.Text + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtdesignation.Text.Trim().Replace("'", "''") + "','" + dateTimePicker1.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update employees set status='" + cmbstatus.Text + "',branchid='" + cmbbranch.SelectedValue + "',payableaccountid='" + cmbaccount.SelectedValue + "', JoiningDate='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "',EmpId='" + txtcode.Text.Trim().Replace("'", "''") + "',Designation='" + txtdesignation.Text.Trim().Replace("'", "''") + "' ,Phone='" + txtphone.Text.Trim().Replace("'", "''") + "' ,Name='" + txtname.Text.Trim().Replace("'", "''") + "'  where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from employees");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcode.Text = string.Empty;
            txtname.Text = string.Empty;
         
            txtphone.Text = string.Empty;
           
            
            txtdesignation.Text = string.Empty;
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

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  fillaccount();
        }
    }
}
