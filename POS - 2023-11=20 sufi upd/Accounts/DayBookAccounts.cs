using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Accounts
{
    public partial class DayBookAccounts : Form
    {
        POSRestaurant.forms.MainForm _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public DayBookAccounts()
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
           
        }
        public void fillaccounts()
        {
            try
            {
                //category
                DataSet ds8 = new DataSet();
                string q8 = "";
                q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE   AccountType='Current Assets'";
               
                ds8 = objCore.funGetDataSet(q8);

                cmbaccount.DataSource = ds8.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "Name";
                
            }
            catch (Exception ex)
            {


            }
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
            fillaccounts();
            try
            {
                DataSet dsb = new System.Data.DataSet();
                string q = "SELECT     Id,  BranchName  FROM         Branch ";
                dsb = new System.Data.DataSet();
                dsb = objCore.funGetDataSet(q);

                cmbbranchjv.DataSource = dsb.Tables[0];
                cmbbranchjv.ValueMember = "id";
                cmbbranchjv.DisplayMember = "BranchName";
            }
            catch (Exception ex)
            {
                
                
            }
            getdata("SELECT     dbo.DayBook.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type, dbo.Branch.BranchName FROM         dbo.DayBook INNER JOIN                      dbo.ChartofAccounts ON dbo.DayBook.AccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.Branch ON dbo.DayBook.branchid = dbo.Branch.Id");
          
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbaccount.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Account");
                    return;
                }

                //if (cmbbranchjv.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Please Select Branch");
                //    return;
                //}
                if (editmode == 0)
                {

                   
                    DataSet ds = new DataSet();
                    int idd = 0;
                    
                    ds = new DataSet();
                    string q = "select * from DayBook where AccountId='" + cmbaccount.SelectedValue + "' and Type='" + cmbtype.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Account already exist");
                        return;
                    }

                    q = "insert into DayBook (branchid,AccountId, Type) values('" + cmbbranchjv.SelectedValue + "','" + cmbaccount.SelectedValue + "','" + cmbtype.Text + "')";
                    int res = objCore.executeQuery(q);
                    if (res == 255)
                    {
                        MessageBox.Show("You are not Allowed for this Operation");
                    }
                    else if (res == 0)
                    {
                        MessageBox.Show("Failed to Save Data");
                    }
                    else
                    {MessageBox.Show("Record saved successfully");
                       
                    }
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    
                }
                if (editmode == 1)
                {

                    string q = "update DayBook set branchid='" + cmbbranchjv.SelectedValue + "',AccountId='" + cmbaccount.SelectedValue + "',Type='" + cmbtype.Text + "' where id='" + id + "'";

                    int res = objCore.executeQuery(q);
                    if (res == 255)
                    {
                        MessageBox.Show("You are not Allowed for this Operation");
                    }
                    else if (res == 0)
                    {
                        MessageBox.Show("Failed to Save Data");
                    }
                    else
                    {MessageBox.Show("Record updated successfully");

                    }
                    
                }
                getdata("SELECT     dbo.DayBook.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM         dbo.DayBook INNER JOIN                      dbo.ChartofAccounts ON dbo.DayBook.AccountId = dbo.ChartofAccounts.Id");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected void getdata(string q)
        {
            DataSet ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton2.Text = "Submit";
            editmode = 0;
            //txtlocation.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbtype_SelectedValueChanged(object sender, EventArgs e)
        {
            fillaccounts();
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbbranchjv_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillaccounts();
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void editSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["id"].Value.ToString();
            editmode = 1;
            edit();
        }
        protected void edit()
        {
            try
            {
                if (editmode == 1)
                {
                    //POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "select * from DayBook where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                       
                        cmbtype.Text = ds.Tables[0].Rows[0]["type"].ToString();
                        cmbaccount.SelectedValue = ds.Tables[0].Rows[0]["Accountid"].ToString();
                        vButton2.Text = "Update";
                        editmode = 1;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["id"].Value.ToString();
            DialogResult dr = MessageBox.Show("Are you Sure to delete ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string q = "delete from DayBook where id=" + id;
                objCore.executeQuery(q);
                getdata("SELECT     dbo.DayBook.Id, dbo.ChartofAccounts.Name, dbo.DayBook.Type FROM         dbo.DayBook INNER JOIN                      dbo.ChartofAccounts ON dbo.DayBook.AccountId = dbo.ChartofAccounts.Id");
                MessageBox.Show("Deleted Successfully");
            }
        }
    }
}
