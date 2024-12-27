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
    public partial class AddLayout : Form
    {
        POSRetail.forms.MainForm _frm;
        public AddLayout(POSRetail.forms.MainForm frm)
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
            txtcols.Text = string.Empty;
            txtrows.Text = string.Empty;
            comboBox1.Text = "Please Select";
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
          
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                comboBox1.Text = "Please Select";
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "";

                if (editmode == 1)
                {
                    objCore = new classes.Clsdbcon();
                    ds = new DataSet();
                    q = "select * from Tablelayout where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtrows.Text = ds.Tables[0].Rows[0]["Rows"].ToString();
                        txtcols.Text = ds.Tables[0].Rows[0]["Columns"].ToString();
                        comboBox1.Text = ds.Tables[0].Rows[0]["tablename"].ToString();
                        vButton2.Text = "Update";
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
       
        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtrows.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter No of Rows");
                    return;
                }
                if (txtcols.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter No of Columns");
                    return;
                }
                if (txtrows.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtrows.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {
                        txtrows.Focus();
                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        return;
                    }
                }
                if (txtcols.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtcols.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        txtcols.Focus();
                        return;
                    }
                }
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();

                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Tablelayout");
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
                    string q = "select * from Tablelayout where tablename='" + comboBox1.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Data already exist");
                        return;
                    }
                    q = "insert into Tablelayout (id,Rows,Columns,tablename) values('" + id + "','" + txtrows.Text.Trim().Replace("'", "''") + "','" + txtcols.Text.Trim().Replace("'", "''") + "','" + comboBox1.Text + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Tablelayout set Rows='" + txtrows.Text.Trim().Replace("'", "''") + "' , Columns ='" + txtcols.Text.Trim().Replace("'", "''") + "',tablename='" + comboBox1.Text + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT Id,tablename AS table_Name, Columns, Rows  FROM         Tablelayout");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcols.Text = string.Empty;
            txtrows.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtrows_TextChanged(object sender, EventArgs e)
        {
            if (txtrows.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtrows.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }
        }

        private void txtcols_TextChanged(object sender, EventArgs e)
        {
            if (txtcols.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtcols.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
            }
        }
    }
}
