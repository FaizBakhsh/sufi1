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
    public partial class AddChartAccountsCodes : Form
    {
        POSRetail.forms.MainForm _frm;
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public AddChartAccountsCodes(POSRetail.forms.MainForm frm)
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
            if (editmode == 1)
            {
                //POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from ChartofAccountsCodes where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbtype.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtlocation.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                    vButton2.Text = "Update";
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
                    string q = "select * from ChartofAccountsCodes where Name='" + cmbtype.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Account Name already exist");
                        return;
                    }

                    q = "insert into ChartofAccountsCodes (id,Name,Code) values('" + id + "','" + cmbtype.Text.Trim().Replace("'", "''") + "','" + txtlocation.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    DataSet ds = new DataSet();
                    string q = "select * from ChartofAccountsCodes where code='" + txtlocation.Text.Trim() + "'";
                    ds =objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Account Code already exist");
                        return;
                    }
                    q = "update ChartofAccountsCodes set Code='" + txtlocation.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from ChartofAccountsCodes");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           
            txtlocation.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
