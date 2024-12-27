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
    public partial class AddPoints : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddPoints(POSRestaurant.forms.MainForm frm)
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
            try
            {
                if (editmode == 1)
                {
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "select * from points where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtminsale.Text = ds.Tables[0].Rows[0]["MinSale"].ToString();
                        txtmaxsale.Text = ds.Tables[0].Rows[0]["MaxSale"].ToString();
                        txtpoint.Text = ds.Tables[0].Rows[0]["Points"].ToString();
                       
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
                if (txtminsale.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Minimaum Sale");
                    return;
                }
                if (txtmaxsale.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Maximum Sale");
                    return;
                }
                if (txtpoint.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter POints");
                    return;
                }

                if (Convert.ToInt32(txtmaxsale.Text.Trim()) <= Convert.ToInt32(txtminsale.Text.Trim()))
                {
                    MessageBox.Show("Maximum Sale Value is Less Than or Equal to Minimum Sale Value");
                    return;
                }
                
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from points");
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
                    string q = "select * from Points where MaxSale >='" + txtminsale.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Minimum Sale is Less Than Previous Maximum Sale");
                        return;
                    }

                    q = "insert into Points (id,MinSale,MaxSale,Points,Uploadstatus) values('" + id + "','" + txtminsale.Text.Trim().Replace("'", "''") + "','" + txtmaxsale.Text.Trim().Replace("'", "''") + "','" + txtpoint.Text.Trim().Replace("'", "''") + "','Pending')";
                    objCore.executeQuery(q);
                    //POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    _frm.getdata("select * from Points");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Points set MinSale='" + txtminsale.Text.Trim().Replace("'", "''") + "' ,MaxSale='" + txtmaxsale.Text.Trim().Replace("'", "''") + "' ,Points='" + txtpoint.Text.Trim().Replace("'", "''") + "'  where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    _frm.getdata("select * from Points");
                    MessageBox.Show("Record updated successfully");
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           
            txtminsale.Text = string.Empty;
            txtmaxsale.Text = string.Empty;
            txtpoint.Text = string.Empty;
           
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtminsale_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int Num;
                bool isNum = int.TryParse(txtminsale.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void txtmaxsale_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int Num;
                bool isNum = int.TryParse(txtmaxsale.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void txtpoint_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int Num;
                bool isNum = int.TryParse(txtpoint.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
