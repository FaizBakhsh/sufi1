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
    public partial class Addsalaryry : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public Addsalaryry(POSRestaurant.forms.MainForm frm)
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
            txtamount.Text = string.Empty;
            
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
          
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select id as empid,name  from employees where status='Active' order by name";
            ds = objCore.funGetDataSet(q);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "empid";
            comboBox1.DisplayMember = "Name";

            if (editmode == 1)
            {
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from EmployeesSalary where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtamount.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["Empid"].ToString();
                    dateTimePicker1.Text= ds.Tables[0].Rows[0]["date"].ToString();
                    vButton2.Text = "Update";
                }
            }
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void vButton2_Click(object sender, EventArgs e)
        {
            string branchid = "0";
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                branchid = ds1.Tables[0].Rows[0]["id"].ToString();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                if (txtamount.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Salary");
                    return;
                }
                
               
                DataSet ds = new DataSet();

                if (editmode == 0)
                {
                    
                    ds = new DataSet();
                    string q = "select * from EmployeesSalary where date='" + dateTimePicker1.Text.Trim() + "' and Empid='"+comboBox1.SelectedValue+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Record already exist");
                        return;
                    }
                    q = "insert into EmployeesSalary (branchid,Empid, Amount, Date) values('" + branchid + "','" + comboBox1.SelectedValue + "','" + txtamount.Text.Trim().Replace("'", "''") + "','" + dateTimePicker1.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();                   
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update EmployeesSalary set branchid='" + branchid + "', Empid='" + comboBox1.SelectedValue + "', Amount='" + txtamount.Text.Trim().Replace("'", "''") + "' , Date ='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT       dbo.EmployeesSalary.id, dbo.Employees.Name as name, dbo.EmployeesSalary.Amount, dbo.EmployeesSalary.Date FROM            dbo.Employees INNER JOIN                         dbo.EmployeesSalary ON dbo.Employees.id = dbo.EmployeesSalary.Empid");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            
            txtamount.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
