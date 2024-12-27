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
    public partial class AddSalesAccount : Form
    {
        POSRestaurant.forms.MainForm _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public AddSalesAccount()
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
                if (cmbtype.Text == "Cash Account" || cmbtype.Text == "Master Account" || cmbtype.Text == "Visa Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets') ";

                }
                if (cmbtype.Text == "Sales Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'revenue') ";
                }
                if (cmbtype.Text == "GST Account" || cmbtype.Text == "WithHolding Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Liabilities')";
                }
                if (cmbtype.Text == "Discount Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Admin and Selling Expenses') or (AccountType = 'Operating Expenses') or (AccountType = 'Financial Expenses') ";
                }
                if (cmbtype.Text == "Cost of Sales Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Cost of Sales') ";
                }
                if (cmbtype.Text == "Inventory Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets') ";
                }
                if (cmbtype.Text == "Service Charges Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts ";
                }

                if (cmbtype.Text == "Head Office Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE    (AccountType = 'Current Liabilities') ";
                }
                if (cmbtype.Text == "Salaries Expense Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Admin and Selling Expenses') or (AccountType = 'Operating Expenses') or (AccountType = 'Financial Expenses') ";
            
                }
                if (cmbtype.Text == "Cash Purchase")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE    (AccountType = 'Current Assets') ";

                }
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
            try
            {
                DataSet dsb = new System.Data.DataSet();
                string q = "SELECT     Id,  Name  FROM         Banks ";
                dsb = new System.Data.DataSet();
                dsb = objCore.funGetDataSet(q);

                cmbbanks.DataSource = dsb.Tables[0];
                cmbbanks.ValueMember = "id";
                cmbbanks.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }
            getdata("SELECT     dbo.CashSalesAccountsList.Id, dbo.ChartofAccounts.Name, dbo.CashSalesAccountsList.AccountType, dbo.Branch.BranchName, dbo.CashSalesAccountsList.UploadStatus FROM         dbo.CashSalesAccountsList INNER JOIN                      dbo.ChartofAccounts ON dbo.CashSalesAccountsList.ChartaccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.Branch ON dbo.CashSalesAccountsList.branchid = dbo.Branch.Id");
       
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

                if (cmbbranchjv.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Branch");
                    return;
                }
                string bankid = "";
                if (cmbtype.Text == "Visa Account")
                {
                    bankid = cmbbanks.SelectedValue.ToString();
                }
                if (editmode == 0)
                {

                   
                    DataSet ds = new DataSet();
                    int idd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CashSalesAccountsList");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        idd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        idd = 1;
                    }
                    ds = new DataSet();
                    string q = "select * from CashSalesAccountsList where ChartaccountId='" + cmbaccount.SelectedValue + "' and AccountType='" + cmbtype.Text + "' and branchid='"+cmbbranchjv.SelectedValue+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("Account already exist", "Are youo sure to procees?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                    }

                    q = "insert into CashSalesAccountsList (bankid,id,ChartaccountId,AccountType,branchid) values('" + bankid + "','" + idd + "','" + cmbaccount.SelectedValue + "','" + cmbtype.Text + "','" + cmbbranchjv.SelectedValue + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {

                    string q = "update CashSalesAccountsList set bankid='" + bankid + "',ChartaccountId='" + cmbaccount.SelectedValue + "',branchid='" + cmbbranchjv.SelectedValue + "' where id='" + id + "'";
                    
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                getdata("SELECT     dbo.CashSalesAccountsList.Id, dbo.ChartofAccounts.Name, dbo.CashSalesAccountsList.AccountType, dbo.Branch.BranchName, dbo.CashSalesAccountsList.bankid FROM         dbo.CashSalesAccountsList INNER JOIN                      dbo.ChartofAccounts ON dbo.CashSalesAccountsList.ChartaccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.Branch ON dbo.CashSalesAccountsList.branchid = dbo.Branch.Id");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected void getdata(string q)
        {
            q = "SELECT     dbo.CashSalesAccountsList.Id, dbo.ChartofAccounts.Name, dbo.CashSalesAccountsList.AccountType, dbo.Branch.BranchName, dbo.CashSalesAccountsList.UploadStatus FROM         dbo.CashSalesAccountsList INNER JOIN                      dbo.ChartofAccounts ON dbo.CashSalesAccountsList.ChartaccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.Branch ON dbo.CashSalesAccountsList.branchid = dbo.Branch.Id where dbo.CashSalesAccountsList.branchid='"+cmbbranchjv.SelectedValue+"'";
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
            if (cmbtype.Text == "Visa Account")
            {
                lblbank.Visible = true;
                cmbbanks.Visible = true;
            }
            else
            {
                lblbank.Visible = false;
                cmbbanks.Visible = false;
            }
        }

        private void cmbbranchjv_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillaccounts();
            getdata("");
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
                    string q = "select * from CashSalesAccountsList where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cmbbranchjv.SelectedValue = ds.Tables[0].Rows[0]["branchid"].ToString();
                        cmbtype.Text = ds.Tables[0].Rows[0]["AccountType"].ToString();
                        cmbaccount.SelectedValue = ds.Tables[0].Rows[0]["ChartaccountId"].ToString();
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
                string q = "delete from CashSalesAccountsList where id=" + id;
                objCore.executeQuery(q);
                getdata("SELECT     dbo.CashSalesAccountsList.Id, dbo.ChartofAccounts.Name, dbo.CashSalesAccountsList.AccountType, dbo.Branch.BranchName, dbo.CashSalesAccountsList.UploadStatus FROM         dbo.CashSalesAccountsList INNER JOIN                      dbo.ChartofAccounts ON dbo.CashSalesAccountsList.ChartaccountId = dbo.ChartofAccounts.Id INNER JOIN                      dbo.Branch ON dbo.CashSalesAccountsList.branchid = dbo.Branch.Id");
                MessageBox.Show("Deleted Successfully");
            }
        }
    }
}
