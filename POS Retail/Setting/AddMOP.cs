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
    public partial class AddMOP : Form
    {
         POSRetail.forms.MainForm _frm;
         public AddMOP(POSRetail.forms.MainForm frm)
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
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from MOP where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
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
                
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of MOP");
                    return;
                }
                
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                
                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from MOP");
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
                    string q = "select * from MOP where Name='" + txtName.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("MOP Name already exist");
                        return;
                    }
                    q = "insert into MOP (id,Name) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                  
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update MOP set Name='" + txtName.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from MOP");
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
