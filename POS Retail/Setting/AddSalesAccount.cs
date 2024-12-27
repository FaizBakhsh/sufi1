using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Setting
{
    public partial class AddSalesAccount : Form
    {
        POSRetail.forms.MainForm _frm;
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public AddSalesAccount(POSRetail.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
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
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets')";

                }
                if (cmbtype.Text == "Sales Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'revenue')";
                }
                if (cmbtype.Text == "GST Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Liabilities')";
                }
                if (cmbtype.Text == "Discount Account")
                {
                    q8 = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Admin and Selling Expenses')";
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
                if (editmode == 1)
                {
                    //POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "select * from CashSalesAccountsList where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        cmbtype.Text = ds.Tables[0].Rows[0]["AccountType"].ToString();
                        cmbaccount.SelectedValue = ds.Tables[0].Rows[0]["ChartaccountId"].ToString();
                        vButton2.Text = "Update";
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
                if (cmbaccount.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Account");
                    return;
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
                    string q = "select * from CashSalesAccountsList where ChartaccountId='" + cmbaccount.SelectedValue + "' and AccountType='"+cmbtype.Text+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Account already exist");
                        return;
                    }

                    q = "insert into CashSalesAccountsList (id,ChartaccountId,AccountType) values('" + idd + "','" + cmbaccount.SelectedValue + "','" + cmbtype.Text + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {

                    string q = "update CashSalesAccountsList set ChartaccountId='" + cmbaccount.SelectedValue + "' where id='" + id + "'";
                    
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT     dbo.CashSalesAccountsList.Id, dbo.ChartofAccounts.Name, dbo.CashSalesAccountsList.AccountType, dbo.CashSalesAccountsList.UploadStatus FROM         dbo.CashSalesAccountsList INNER JOIN                      dbo.ChartofAccounts ON dbo.CashSalesAccountsList.ChartaccountId = dbo.ChartofAccounts.Id");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           
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
    }
}
