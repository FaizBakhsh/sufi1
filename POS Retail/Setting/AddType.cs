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
    public partial class AddType : Form
    {
        POSRetail.forms.MainForm _frm;
        public AddType(POSRetail.forms.MainForm frm)
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
        public void change()
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from groups";
                ds = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "groupName";
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from Category Where groupid='" + comboBox1.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                comboBox2.DataSource = ds.Tables[0];
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "CategoryName";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                change();
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "";
                if (editmode == 1)
                {
                    objCore = new classes.Clsdbcon();
                    ds = new DataSet();
                    q = "select * from Type where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = ds.Tables[0].Rows[0]["TypeName"].ToString();
                        txtdescription.Text = ds.Tables[0].Rows[0]["TypeDiscription"].ToString();
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["groupId"].ToString();
                        comboBox2.SelectedValue = ds.Tables[0].Rows[0]["CategoryId"].ToString();
                        vButton2.Text = "Update";
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "";

                q = "select * from Category Where groupid='" + comboBox1.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    comboBox2.DataSource = ds.Tables[0];
                    comboBox2.ValueMember = "id";
                    comboBox2.DisplayMember = "CategoryName";
                }
                else
                {
                    comboBox2.DataSource = null;
                }
            }
            catch (Exception ex)
            {
                
               
            }

        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {

                if (comboBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please select a valid Group");
                    comboBox1.Focus();
                    return;
                }
                if (comboBox2.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please select a valid Category");
                    comboBox2.Focus();
                    return;
                }
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Type");
                    txtName.Focus();
                    return;
                }
                if (editmode == 0)
                {
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Type");
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
                    string q = "select * from Type where TypeName='" + txtName.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Type Name Name already exist");
                        return;
                    }
                    q = "insert into Type (id,GroupId,Categoryid,TypeName,TypeDiscription) values('" + id + "','" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                   
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Type set GroupId='" + comboBox1.SelectedValue + "',Categoryid='" + comboBox2.SelectedValue + "', TypeName='" + txtName.Text.Trim().Replace("'", "''") + "' , TypeDiscription ='" + txtdescription.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT     dbo.Type.Id, dbo.Groups.GroupName, dbo.Category.CategoryName, dbo.Type.TypeName, dbo.Type.TypeDiscription FROM         dbo.Groups INNER JOIN                      dbo.Category ON dbo.Groups.Id = dbo.Category.GroupId INNER JOIN                      dbo.Type ON dbo.Groups.Id = dbo.Type.groupId AND dbo.Category.Id = dbo.Type.CategoryId");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtdescription.Text = string.Empty;
            txtName.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
