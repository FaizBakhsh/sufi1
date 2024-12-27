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
    public partial class AddRecipe : Form
    {
        public static string itemid = "";
        public string umcid = "";
         POSRetail.forms.MainForm _frm;
         public AddRecipe(POSRetail.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
           
        }
        public void getdata()
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsdata = new DataSet();
                string q = "SELECT     dbo.Recipe.Id, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.RawItem.ItemName AS RawItem, dbo.Recipe.UOMCId AS UOM, dbo.Recipe.Quantity FROM         dbo.MenuItem INNER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId INNER JOIN                      dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id where dbo.Recipe.MenuItemId='" + comboBox2.SelectedValue + "'";
                dsdata = objCore.funGetDataSet(q);
                dataGridView1.DataSource = dsdata.Tables[0];
                dataGridView1.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                
               
            }

        }
        private void btnclear_Click(object sender, EventArgs e)
        {
            
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
           
        }
        public void fillgroup()
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from MenuGroup";
                ds = objCore.funGetDataSet(q);

                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        public void fillmenuitem()
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds1 = new DataSet();
                string q = "select * from MenuItem where MenuGroupId='" + comboBox1.SelectedValue + "'";
                ds1 = objCore.funGetDataSet(q);

                DataRow dr = ds1.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds1.Tables[0].Rows.Add(dr);
                comboBox2.DataSource = ds1.Tables[0];
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                
                
            }
           
        }
        public void fillrawitem()
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds2 = new DataSet();
                string q = "select * from RawItem";
                ds2 = objCore.funGetDataSet(q);

                DataRow dr = ds2.Tables[0].NewRow();
                dr["ItemName"] = "Please Select";

                ds2.Tables[0].Rows.Add(dr);
                comboBox3.DataSource = ds2.Tables[0];
                comboBox3.ValueMember = "id";
                comboBox3.DisplayMember = "ItemName";
                comboBox3.Text = "Please Select";
            }
            catch (Exception ex)
            {
                
               
            }

        }
        public void filluom()
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds3 = new DataSet();
                string q = "select * from RawItem where ItemName='" + comboBox3.Text + "'";
                ds3 = objCore.funGetDataSet(q);
                DataSet dsuom = new DataSet();
                dsuom = objCore.funGetDataSet("select * from UOMConversion where UOMId='" + ds3.Tables[0].Rows[0]["UOMId"].ToString() + "'");
                DataRow dr = dsuom.Tables[0].NewRow();
                dr["UOM"] = "Please Select";
                umcid = dsuom.Tables[0].Rows[0]["id"].ToString();
                dsuom.Tables[0].Rows.Add(dr);
                txtuom.Text = dsuom.Tables[0].Rows[0]["uom"].ToString();
                
               // comboBox4.Text = "Please Select";

            }
            catch (Exception ex)
            {
                
               
            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            fillgroup();
            fillmenuitem();
            fillrawitem();
            filluom();
            if (editmode == 1)
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Recipe where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtuom.Text = ds.Tables[0].Rows[0]["UOMCId"].ToString();
                    richTextBox1.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
                    comboBox2.SelectedValue = ds.Tables[0].Rows[0]["MenuItemId"].ToString();
                    comboBox3.SelectedValue = ds.Tables[0].Rows[0]["RawItemId"].ToString();
                   DataSet dss = new DataSet();
                    dss = objCore.funGetDataSet("select * from MenuItem where id='"+comboBox2.SelectedValue+"'");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        comboBox1.SelectedValue = dss.Tables[0].Rows[0]["MenuGroupId"].ToString();
                    }
                    vButton2.Text = "Update";
                }
            }
            getdata();
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillmenuitem();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtuom.Text = string.Empty;
            filluom();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (richTextBox1.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(richTextBox1.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Quantity. Only Nymbers are allowed");
                    return;
                }
            }
        }
        public void getinfo(string id)
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsinfo = new DataSet();
            string qry = "select * from Recipe where id='" + id + "'";
            dsinfo = objCore.funGetDataSet(qry);
            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                comboBox2.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuItemId"].ToString();
                comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["RawItemId"].ToString();
                txtuom.Text = dsinfo.Tables[0].Rows[0]["UOMCId"].ToString();
                richTextBox1.Text = dsinfo.Tables[0].Rows[0]["Quantity"].ToString();
                DataSet dss = new DataSet();
                dss = objCore.funGetDataSet("select * from MenuItem where id='" + comboBox2.SelectedValue + "'");
                if (dss.Tables[0].Rows.Count > 0)
                {
                    comboBox1.SelectedValue = dss.Tables[0].Rows[0]["MenuGroupId"].ToString();
                }
                vButton2.Text = "Update";
                itemid = id;
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Group");
                    return;
                }
                if (comboBox2.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Item");
                    return;
                }
                if (comboBox2.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Raw Item");
                    return;
                }
                if (txtuom.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("UOM Conversion not Defined. Please Define it First");
                    return;
                }
                if (richTextBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Quantity");
                    return;
                }
                if (richTextBox1.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(richTextBox1.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Quantity. Only Nymbers are allowed");
                        return;
                    }
                }
                if (vButton2.Text == "Add")
                {

                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Recipe");
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
                    string q = "select * from Recipe where RawItemId='" + comboBox3.SelectedValue + "' and MenuItemId='" + comboBox2.SelectedValue + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("This Raw Item Already Exist for Selected Menu Item");
                        return;
                    }
                    q = "insert into Recipe (id,RawItemId,MenuItemId,UOMCId,Quantity) values('" + id + "','" + comboBox3.SelectedValue + "','" + comboBox2.SelectedValue + "','" + umcid + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                   
                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "update Recipe set RawItemId='" + comboBox3.SelectedValue + "' , UOMCId ='" + umcid + "', Quantity ='" + richTextBox1.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    getdata();
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Recipe");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                  
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtuom.Text = string.Empty;
            richTextBox1.Text = string.Empty;
            comboBox1.Text = "Please Select";
            comboBox3.Text = "Please Select";
            vButton2.Text = "Add";
            getdata();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    getinfo(id);

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {

                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (msg == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                        string q = "delete from Recipe where id='" + id + "'";
                        objCore.executeQuery(q);
                        getdata();

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
