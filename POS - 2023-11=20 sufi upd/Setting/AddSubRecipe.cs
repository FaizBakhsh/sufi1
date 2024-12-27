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
    public partial class AddSubRecipe : Form
    {
        public static string itemid = "";
        public string umcid = "";
        public string mgid = "";
        public string mtid = "";
         POSRestaurant.forms.MainForm _frm;
         public AddSubRecipe()
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
           
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
           
        }
        public void getdata()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsdata = new DataSet();
                string q = "";
                q = "SELECT        dbo.SubRecipe.Id, dbo.SubItems.Name, dbo.RawItem.ItemName AS RawItem, dbo.SubRecipe.Quantity, dbo.SubRecipe.type FROM            dbo.SubRecipe INNER JOIN                         dbo.SubItems ON dbo.SubRecipe.ItemId = dbo.SubItems.Id INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id where dbo.SubRecipe.itemid='" + cmbitem.SelectedValue + "'";
               
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
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from subitems where status='active' order by name";
                ds = objCore.funGetDataSet(q);

                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds.Tables[0].Rows.Add(dr);
                cmbitem.DataSource = ds.Tables[0];
                cmbitem.ValueMember = "id";
                cmbitem.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
       
        public void fillrawitem()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds2 = new DataSet();
                string q = "select * from RawItem  order by ItemName";
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
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds3 = new DataSet();
                string q = "select * from RawItem where ItemName='" + comboBox3.Text + "' order by ItemName";
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
          
            fillrawitem();
            filluom();
         

            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from SubRecipe where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbitem.SelectedValue = ds.Tables[0].Rows[0]["itemid"].ToString();
                    richTextBox1.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
                   // comboBox2.SelectedValue = ds.Tables[0].Rows[0]["MenuItemId"].ToString();
                    comboBox3.SelectedValue = ds.Tables[0].Rows[0]["RawItemId"].ToString();
                   
                    //DataSet dss = new DataSet();
                    //dss = objCore.funGetDataSet("select * from MenuItem where id='" + comboBox2.SelectedValue + "'");
                    //if (dss.Tables[0].Rows.Count > 0)
                    //{
                    //    
                    //}
                    vButton2.Text = "Update";
                    cmbtype.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                }
            }
            getdata();
            try
            {
                if (editmode != 1)
                {
                    cmbitem.SelectedValue = mgid;
                    //comboBox2.SelectedValue = mtid;
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
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
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsinfo = new DataSet();
                string qry = "select * from SubRecipe where id='" + id + "'";
                dsinfo = objCore.funGetDataSet(qry);
                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                    cmbitem.SelectedValue = dsinfo.Tables[0].Rows[0]["itemid"].ToString();
                   // comboBox2.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuItemId"].ToString();
                    comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["RawItemId"].ToString();
                    
                    richTextBox1.Text = dsinfo.Tables[0].Rows[0]["Quantity"].ToString();
                    //DataSet dss = new DataSet();
                    //dss = objCore.funGetDataSet("select * from MenuItem where id='" + comboBox2.SelectedValue + "'");
                    //if (dss.Tables[0].Rows.Count > 0)
                    //{
                    //    cmbitem.SelectedValue = dss.Tables[0].Rows[0]["MenuGroupId"].ToString();
                    //}
                    vButton2.Text = "Update";
                    itemid = id;
                    filluom();
                    cmbtype.SelectedValue = dsinfo.Tables[0].Rows[0]["type"].ToString();
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
            save();
        }
        protected void save()
        {
            try
            {
                if (cmbitem.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Group");
                    return;
                }

                if (txtuom.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("UOM Conversion not Defined. Please Define it First");
                    return;
                }
                if (cmbtype.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select type");
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
                    ds = objCore.funGetDataSet("select max(id) as id from SubRecipe");
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
                    string q = "";
                    q = "select * from SubRecipe where RawItemId='" + comboBox3.SelectedValue + "' and itemid='" + cmbitem.SelectedValue + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("This Raw Item Already Exist for Selected Item", "Do you Want to save Instead?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                    }
                    q = "insert into SubRecipe (type,id,RawItemId,itemid,Quantity) values('" + cmbtype.Text + "','" + id + "','" + comboBox3.SelectedValue + "','" + cmbitem.SelectedValue + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "')";

                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();

                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "";
                    q = "update SubRecipe set type='" + cmbtype.Text + "', itemid='" + cmbitem.SelectedValue + "', RawItemId='" + comboBox3.SelectedValue + "' , Quantity ='" + richTextBox1.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    getdata();
                    MessageBox.Show("Record updated successfully");
                }
                comboBox3.Focus();
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
                        string q = "delete from subRecipe where id='" + id + "'";
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

        private void vButton6_Click(object sender, EventArgs e)
        {
            AttachSubRecipe obj = new AttachSubRecipe();
            obj.Show();
        }

        private void AddSubRecipe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                save();
            }
            if (e.KeyCode == Keys.Escape)
            {
                cmbitem.Focus();
            }
        }
    }
}
