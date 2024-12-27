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
    public partial class AddExtraprice : Form
    {
        
        public AddExtraprice()
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
          
            txtName.Text = string.Empty;
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
          
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void AddGroups_Load(object sender, EventArgs e)
        {
            
            DataSet ds = new DataSet();
            string q = "select * from menuitem";
            ds = objCore.funGetDataSet(q);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "Name";

            if (editmode == 1)
            {
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from extraprice where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["price"].ToString();
                    
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["menuitemid"].ToString();
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
                    MessageBox.Show("Please Enter Price");
                    return;
                }
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();

                if (editmode == 0)
                {
                    string q = "select * from extraprice where menuitemid='" + comboBox1.SelectedValue + "' and flavourid='"+comboBox2.SelectedValue+"'";
                    ds = new DataSet();
                    if (comboBox2.Text == "")
                    {
                        q = "select * from extraprice where menuitemid='" + comboBox1.SelectedValue + "'";
                    }
                    
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Record already exist");
                        return;
                    }
                    string flv = "0";
                    try
                    {
                        flv = comboBox2.SelectedValue.ToString();
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }

                    q = "insert into extraprice (flavourid,menuitemid,type,amount) values('" + flv + "','" + comboBox1.SelectedValue + "','Delivery','" + txtName.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string flv = "0";
                    try
                    {
                        flv = comboBox2.SelectedValue.ToString();
                    }
                    catch (Exception ex)
                    {


                    }
                    string q = "update extraprice set flavourid='" + flv + "',menuitemid='" + comboBox1.SelectedValue + "', amount='" + txtName.Text.Trim().Replace("'", "''") + "'  where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                getdata("");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            editmode = 0;
            comboBox1.Enabled = true;
            txtName.Text = "";
        }
        protected void getdata(string text)
        {
            string q="";
            if (text != "")
            {
                q = "SELECT  dbo.ExtraPrice.id, dbo.MenuItem.Name, dbo.ExtraPrice.amount, dbo.ModifierFlavour.name AS Flavour FROM            dbo.ExtraPrice INNER JOIN                          dbo.MenuItem ON dbo.ExtraPrice.Menuitemid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.ExtraPrice.Flavourid = dbo.ModifierFlavour.Id  where  dbo.MenuItem.Name like '%" + textBox1.Text + "%'  order by dbo.ExtraPrice.menuitemid,dbo.MenuItem.Name";
            }
            else
            {
                q = "SELECT  dbo.ExtraPrice.id, dbo.MenuItem.Name, dbo.ExtraPrice.amount, dbo.ModifierFlavour.name AS Flavour FROM            dbo.ExtraPrice INNER JOIN                          dbo.MenuItem ON dbo.ExtraPrice.Menuitemid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.ExtraPrice.Flavourid = dbo.ModifierFlavour.Id   order by dbo.ExtraPrice.menuitemid,dbo.MenuItem.Name";
            }
            DataSet ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
           
            txtName.Text = string.Empty;
            getdata(textBox1.Text);
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string q = "select * from modifierflavour where menuitemid='"+comboBox1.SelectedValue+"'";
            ds = objCore.funGetDataSet(q);
            comboBox2.DataSource = ds.Tables[0];
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "Name";
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            getdata(textBox1.Text);
        }

        private void editSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string q = "select * from ExtraPrice where id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["Menuitemid"].ToString();
                    comboBox1.Enabled = false;
                    txtName.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                    
                    editmode = 1;
                    comboBox2.SelectedValue = ds.Tables[0].Rows[0]["flavourid"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void deleteSeletedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string q = "delete from ExtraPrice where id=" + id;
                objCore.executeQuery(q);
                getdata(textBox1.Text);
            }
            catch (Exception ex)
            {


            }
        }
    }
}
