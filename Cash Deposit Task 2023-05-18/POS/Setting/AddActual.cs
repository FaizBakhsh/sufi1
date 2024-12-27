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
    public partial class AddActual : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddActual()
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
        public void get(string idd)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from Actual where id='" + idd + "'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                txtcash.Text = ds.Tables[0].Rows[0]["cash"].ToString();
                txtcc.Text = ds.Tables[0].Rows[0]["CC"].ToString();
                vButton2.Text = "Update";
                id = idd;
                editmode = 1;
            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtcash.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Actual Cash");
                    return;
                }
                if (txtcc.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Actual CC");
                    return;
                }
                
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Actual");
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
                    string q = "select * from Actual where date='" + dateTimePicker1.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Amount for this date already exist");
                        get(ds.Tables[0].Rows[0]["id"].ToString());
                        return;
                    }

                    q = "insert into Actual (id,cash,CC, Date) values('" + id + "','" + txtcash.Text.Trim().Replace("'", "''") + "','" + txtcc.Text.Trim().Replace("'", "''") + "','" + dateTimePicker1.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Actual set cash='" + txtcash.Text.Trim().Replace("'", "''") + "' , CC ='" + txtcc.Text.Trim().Replace("'", "''") + "', Date ='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                    txtcash.Text = string.Empty;
                    txtcc.Text = string.Empty;
                    vButton2.Text = "Submit";
                    editmode = 0;
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcash.Text = string.Empty;
            txtcc.Text = string.Empty;
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
