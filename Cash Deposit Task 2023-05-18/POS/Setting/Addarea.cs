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
    public partial class Addarea : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public Addarea(POSRestaurant.forms.MainForm frm)
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
            
            txtName.Text = string.Empty;
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
          
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from branch";
            ds = objCore.funGetDataSet(q);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "branchName";

            if (editmode == 1)
            {
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from DeliveryArea where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                  
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["branchid"].ToString();
                    vButton2.Text = "Update";
                }
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of area");
                    return;
                }
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();

                if (editmode == 0)
                {
                   
                    ds = new DataSet();
                    string q = "select * from DeliveryArea where name='" + txtName.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Name already exist");
                        return;
                    }
                    q = "insert into DeliveryArea (Name,branchid) values('" + txtName.Text + "','" + comboBox1.SelectedValue + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update DeliveryArea set branchid='" + comboBox1.SelectedValue + "', Name='" + txtName.Text.Trim().Replace("'", "''") + "'  where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT        dbo.DeliveryArea.id, dbo.DeliveryArea.Name, dbo.Branch.BranchName FROM            dbo.Branch INNER JOIN                         dbo.DeliveryArea ON dbo.Branch.Id = dbo.DeliveryArea.Branchid");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           
            txtName.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
