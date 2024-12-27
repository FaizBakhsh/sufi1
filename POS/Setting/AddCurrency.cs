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
    public partial class AddCurrency : Form
    {
         POSRestaurant.forms.MainForm _frm;
         public AddCurrency(POSRestaurant.forms.MainForm frm)
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
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Currency where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    txtrate.Text = ds.Tables[0].Rows[0]["Rate"].ToString();
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
                    MessageBox.Show("Please Enter Name of Currency");
                    return;
                }
                
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                 if (editmode == 0)
                 {
                     int id = 0;
                     ds = objCore.funGetDataSet("select max(id) as id from Currency");
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
                     string q = "select * from Currency where Name='" + txtName.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Currency Name already exist");
                        return;
                    }
                    q = "insert into Currency (id,Name,Rate) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtrate.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Currency set Name='" + txtName.Text.Trim().Replace("'", "''") + "',Rate='" + txtrate.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Currency");
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
