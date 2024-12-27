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
    public partial class Addexpenses : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public Addexpenses(POSRestaurant.forms.MainForm frm)
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
            string q = "SELECT        Id, Name, Amount, Date, branchid FROM            Expenses";
           
            if (editmode == 1)
            {
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "SELECT        Id, Name, Amount, Date, branchid FROM            Expenses where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtamount.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                    textBox1.Text = ds.Tables[0].Rows[0]["name"].ToString();
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
                if (textBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Expense Name");
                    return;
                }
                if (txtamount.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Amount");
                    return;
                }
              
                DataSet ds = new DataSet();

                if (editmode == 0)
                {
                    
                    ds = new DataSet();
                    string q = "select * from Expenses where date='" + dateTimePicker1.Text.Trim() + "' and name='" + textBox1.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Record already exist");
                        return;
                    }
                    q = "insert into Expenses (branchid,Name, Amount, Date) values('" + branchid + "','" + textBox1.Text + "','" + txtamount.Text.Trim().Replace("'", "''") + "','" + dateTimePicker1.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Expenses set branchid='" + branchid + "', Name='" + textBox1.Text + "', Amount='" + txtamount.Text.Trim().Replace("'", "''") + "' , Date ='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT        Id, Name, Amount, Date FROM            Expenses");
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
