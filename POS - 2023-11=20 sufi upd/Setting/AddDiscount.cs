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
    public partial class AddDiscount : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddDiscount(POSRestaurant.forms.MainForm frm)
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
            this.Close();
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from DiscountKeys where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtprc.Text = ds.Tables[0].Rows[0]["discount"].ToString();
                    textBox1.Text = ds.Tables[0].Rows[0]["limit"].ToString();
                    vButton2.Text = "Update";
                    comboBox1.Text = ds.Tables[0].Rows[0]["status"].ToString();
                }
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Discount Key");
                    txtname.Focus();
                    return;
                }
                if (txtprc.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Discount Percentage Value");
                    txtprc.Focus();
                    return;
                }
                
                if (txtprc.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtprc.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid value. Only Numbers are allowed");
                        txtprc.Focus();
                        return;
                    }
                }
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();

                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from DiscountKeys");
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
                    string q = "select * from DiscountKeys where name='" + txtname.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Discount Name already exist");
                        return;
                    }
                    q = "insert into DiscountKeys (limit,id,name,discount,status) values('" + textBox1.Text + "','" + id + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtprc.Text.Trim().Replace("'", "''") + "','" + comboBox1.Text.Trim().Replace("'", "''") + "' )";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update DiscountKeys set limit='"+textBox1.Text.Trim()+"', status='" + comboBox1.Text.Trim().Replace("'", "''") + "' ,name='" + txtname.Text.Trim().Replace("'", "''") + "' , discount ='" + txtprc.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from DiscountKeys");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtprc.Text = string.Empty;
            txtname.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid value. Only Numbers are allowed");
                    return;
                }
            }
        }
    }
}
