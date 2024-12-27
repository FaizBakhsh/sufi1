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
    public partial class AddChartAccountsCodes : Form
    {
        POSRestaurant.forms.MainForm _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public AddChartAccountsCodes()
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
           
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
            DataSet dss = new DataSet();
            string q = "select * from Branch";
            dss = objCore.funGetDataSet(q);
            cmbBranch.DataSource = dss.Tables[0];
            cmbBranch.ValueMember = "id";
            cmbBranch.DisplayMember = "BranchName";
            
            getdata("SELECT     dbo.ChartofAccountsCodes.id, dbo.ChartofAccountsCodes.Name, dbo.ChartofAccountsCodes.Code, dbo.ChartofAccountsCodes.UploadStatus, dbo.Branch.BranchName FROM         dbo.ChartofAccountsCodes INNER JOIN                      dbo.Branch ON dbo.ChartofAccountsCodes.branchid = dbo.Branch.Id");
     
        }
        protected void edit()
        {

            {
                //POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from ChartofAccountsCodes where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbtype.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtlocation.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                    vButton2.Text = "Update";
                    cmbBranch.SelectedValue = ds.Tables[0].Rows[0]["branchid"].ToString();
                    editmode = 1;
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
                if (cmbtype.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Account Type");
                    return;
                }
                if (txtlocation.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Account Code");
                    return;
                }
                //if (cmbBranch.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Please Select Branch");
                //    return;
                //}
                
                if (editmode == 0)
                {                   
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from ChartofAccountsCodes");
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
                    string q = "select * from ChartofAccountsCodes where Name='" + cmbtype.Text.Trim() + "' ";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Account Name already exist");
                        return;
                    }

                    q = "insert into ChartofAccountsCodes (id,Name,Code) values('" + id + "','" + cmbtype.Text.Trim().Replace("'", "''") + "','" + txtlocation.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    //DataSet ds = new DataSet();
                    //string q = "select * from ChartofAccountsCodes where code='" + txtlocation.Text.Trim() + "'";
                    //ds =objCore.funGetDataSet(q);
                    //if (ds.Tables[0].Rows.Count > 0)
                    //{
                    //    MessageBox.Show("Account Code already exist");
                    //    return;
                    //}
                   string q = "update ChartofAccountsCodes set  Code='" + txtlocation.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                getdata("SELECT     dbo.ChartofAccountsCodes.id, dbo.ChartofAccountsCodes.Name, dbo.ChartofAccountsCodes.Code, dbo.ChartofAccountsCodes.UploadStatus FROM         dbo.ChartofAccountsCodes ");
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
            txtlocation.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["id"].Value.ToString();
            editmode = 1;
            edit();
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["id"].Value.ToString();
            DialogResult dr = MessageBox.Show("Are you Sure to delete ", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string q = "delete from ChartofAccountsCodes where id="+id;
                objCore.executeQuery(q);
                getdata("SELECT     dbo.ChartofAccountsCodes.id, dbo.ChartofAccountsCodes.Name, dbo.ChartofAccountsCodes.Code, dbo.ChartofAccountsCodes.UploadStatus FROM         dbo.ChartofAccountsCodes ");
                MessageBox.Show("Deleted Successfully");
            }
        }
    }
}
