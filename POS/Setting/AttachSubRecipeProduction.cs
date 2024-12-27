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
    public partial class AttachSubRecipeProduction : Form
    {
        public static string itemid = "";
        public string umcid = "";
        public string mgid = "";
        public string mtid = "",type="";
         POSRestaurant.forms.MainForm _frm;
         public AttachSubRecipeProduction()
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
           
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
           
        }POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public void getdata()
        {
            string q = "";
            try
            {
                
                DataSet dsdata = new DataSet();
               

                q = "SELECT        dbo.AttachRecipe.Id, dbo.MenuItem.Name, dbo.SubItems.Name AS SubItem, dbo.AttachRecipe.Quantity FROM            dbo.AttachRecipe INNER JOIN                         dbo.MenuItem ON dbo.AttachRecipe.Menuitemid = dbo.MenuItem.Id INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id where dbo.AttachRecipe.Menuitemid='" + cmbitem.SelectedValue + "' and dbo.AttachRecipe.type='Production'";
                q = "SELECT        dbo.AttachRecipe.Id, dbo.RawItem.ItemName AS Name, dbo.SubItems.Name AS SubItem, dbo.AttachRecipe.Quantity FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.RawItem ON dbo.AttachRecipe.Menuitemid = dbo.RawItem.Id  where dbo.AttachRecipe.Menuitemid='" + cmbitem.SelectedValue + "' and dbo.AttachRecipe.type='Production'";
                dsdata = objCore.funGetDataSet(q);
                dataGridView1.DataSource = dsdata.Tables[0];
                dataGridView1.Columns[0].Visible = false;
              
            }
            catch (Exception ex)
            {
                
               
            }
            try
            {
                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where rawitem.id='" + cmbitem.SelectedValue + "' order by itemname ";
                DataSet dsitem = new DataSet();
                dsitem = objCore.funGetDataSet(q);
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    lbluom.Text = dsitem.Tables[0].Rows[0]["UOM"].ToString();
                }
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
        
       
        public void fillmenuitem()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds2 = new DataSet();
                string q = "SELECT DISTINCT dbo.RawItem.Id, dbo.RawItem.ItemName FROM            dbo.RecipeProduction INNER JOIN  dbo.RawItem ON dbo.RecipeProduction.ItemId = dbo.RawItem.Id  order by dbo.RawItem.ItemName";
                ds2 = objCore.funGetDataSet(q);

                DataRow dr = ds2.Tables[0].NewRow();
                dr["ItemName"] = "Please Select";

                ds2.Tables[0].Rows.Add(dr);
                cmbitem.DataSource = ds2.Tables[0];
                cmbitem.ValueMember = "id";
                cmbitem.DisplayMember = "ItemName";
                cmbitem.Text = "Please Select";
            }
            catch (Exception ex)
            {
                
               
            }

        }
        public void fillsubitem()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds2 = new DataSet();
                string q = "select * from subitems where  status='active'   order by name";
                ds2 = objCore.funGetDataSet(q);

                DataRow dr = ds2.Tables[0].NewRow();
                dr["name"] = "Please Select";

                ds2.Tables[0].Rows.Add(dr);
                comboBox3.DataSource = ds2.Tables[0];
                comboBox3.ValueMember = "id";
                comboBox3.DisplayMember = "name";
                comboBox3.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            fillmenuitem();
            fillsubitem();
           
           
         

            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "SELECT        dbo.AttachRecipe.Id, dbo.AttachRecipe.Menuitemid, dbo.AttachRecipe.SubItemId, dbo.AttachRecipe.Quantity, dbo.MenuItem.MenuGroupId  FROM            dbo.AttachRecipe INNER JOIN                         dbo.MenuItem ON dbo.AttachRecipe.Menuitemid = dbo.MenuItem.Id where dbo.AttachRecipe.id='" + id + "'  and dbo.AttachRecipe.type='Production'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    
                    cmbitem.SelectedValue = ds.Tables[0].Rows[0]["Menuitemid"].ToString();
                    richTextBox1.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();

                    comboBox3.SelectedValue = ds.Tables[0].Rows[0]["SubItemId"].ToString();
                   
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
            
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
           
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
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsinfo = new DataSet();
                string q = "SELECT        dbo.AttachRecipe.Id, dbo.AttachRecipe.Menuitemid, dbo.AttachRecipe.SubItemId, dbo.AttachRecipe.Quantity, dbo.MenuItem.MenuGroupId  FROM            dbo.AttachRecipe INNER JOIN                         dbo.MenuItem ON dbo.AttachRecipe.Menuitemid = dbo.MenuItem.Id where dbo.AttachRecipe.id='" + id + "'";
                dsinfo = objCore.funGetDataSet(q);
                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                   
                    cmbitem.SelectedValue = dsinfo.Tables[0].Rows[0]["Menuitemid"].ToString();
                    richTextBox1.Text = dsinfo.Tables[0].Rows[0]["Quantity"].ToString();

                    comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["SubItemId"].ToString();
                    vButton2.Text = "Update";
                    itemid = id;
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                if (cmbitem.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Group");
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

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from AttachRecipe");
                    
                    ds = new DataSet();
                    string q = "";
                    q = "select * from AttachRecipe where SubItemId='" + comboBox3.SelectedValue + "' and Menuitemid='" + cmbitem.SelectedValue + "' and type='Production'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("This Raw Item Already Exist for Selected Item", "Do you Want to save Instead?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                    }
                    q = "insert into AttachRecipe (SubItemId,Menuitemid,Quantity,Type) values('" + comboBox3.SelectedValue + "','" + cmbitem.SelectedValue + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "','Production')";

                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                   
                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "";
                    q = "update AttachRecipe set  Menuitemid='" + cmbitem.SelectedValue + "', SubItemId='" + comboBox3.SelectedValue + "' , Quantity ='" + richTextBox1.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                   
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    getdata();
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
           // txtuom.Text = string.Empty;
            richTextBox1.Text = string.Empty;
            //comboBox1.Text = "Please Select";
            //comboBox3.Text = "Please Select";
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
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msg == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                        string q = "delete from AttachRecipe where id='" + id + "'";
                        objCore.executeQuery(q);
                        getdata();

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
            getdata();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void cmbitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillmenuitem();
        }
    }
}
