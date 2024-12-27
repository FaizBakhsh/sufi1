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
    public partial class AttachMenuItem : Form
    {
        public static string itemid = "";
        public string umcid = "";
        public string mgid = "";
        public string mtid = "";
         POSRestaurant.forms.MainForm _frm;
         public AttachMenuItem(POSRestaurant.forms.MainForm frm)
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
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsdata = new DataSet();
                string q = "";
               // if (comboBox4.Visible == true)
                {
                    q = "SELECT dbo.AttachMenu.id, MenuGroup_1.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.ModifierFlavour.name AS Size, dbo.AttachMenu.Price, dbo.AttachMenu.Compulsory, dbo.AttachMenu.No FROM  dbo.AttachMenu INNER JOIN               dbo.MenuGroup AS MenuGroup_1 ON dbo.AttachMenu.Menugroupid = MenuGroup_1.Id INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id where   dbo.AttachMenu.Dealid='" + cmbdeal.SelectedValue + "'";
                    //q = "SELECT     dbo.Recipe.Id, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.RawItem.ItemName AS RawItem, dbo.Recipe.UOMCId AS UOM, dbo.Recipe.Quantity FROM         dbo.MenuItem INNER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId INNER JOIN                      dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id where dbo.Recipe.MenuItemId='" + cmbmenuitem.SelectedValue + "' and dbo.Recipe.modifierid='" + comboBox4.SelectedValue + "' and dbo.Recipe.modifierid='" + comboBox4.SelectedValue + "'";
                }
                //else
                //{
                //    q = "SELECT     dbo.AttachMenu.id, dbo.MenuGroup.Name AS Deal, MenuGroup_1.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem FROM         dbo.AttachMenu INNER JOIN                      dbo.MenuGroup ON dbo.AttachMenu.Groupid = dbo.MenuGroup.Id INNER JOIN                      dbo.MenuGroup AS MenuGroup_1 ON dbo.AttachMenu.Menugroupid = MenuGroup_1.Id INNER JOIN                      dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id where dbo.AttachMenu.Groupid='" + cmbdeal.SelectedValue + "'";
                //}
                dsdata = objCore.funGetDataSet(q);
                dataGridView1.DataSource = dsdata.Tables[0];
                dataGridView1.Columns[0].Visible = false;
               // dataGridView1.Columns[4].Visible = false;
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
        public void fillgroupmain()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsm = new DataSet();
                string q = "select * from Deals where status='active'";
                dsm = objCore.funGetDataSet(q);
                DataRow dr = dsm.Tables[0].NewRow();
                dr["Name"] = "Please Select";
                dsm.Tables[0].Rows.Add(dr);
                cmbdeal.DataSource = dsm.Tables[0];
                cmbdeal.ValueMember = "id";
                cmbdeal.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }
        public void fillgroup()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from MenuGroup";
                ds = objCore.funGetDataSet(q);

                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds.Tables[0].Rows.Add(dr);
                cmbgroup.DataSource = ds.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "Name";
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
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds1 = new DataSet();
                string q = "select * from MenuItem where MenuGroupId='" + cmbgroup.SelectedValue + "'";
                ds1 = objCore.funGetDataSet(q);

                DataRow dr = ds1.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds1.Tables[0].Rows.Add(dr);
                cmbmenuitem.DataSource = ds1.Tables[0];
                cmbmenuitem.ValueMember = "id";
                cmbmenuitem.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
                
                
            }
           
        }
        public void fillflavours()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds1 = new DataSet();
                string q = "select * from ModifierFlavour where MenuItemId='" + cmbmenuitem.SelectedValue + "'";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr = ds1.Tables[0].NewRow();
                dr["Name"] = "Please Select";
                ds1.Tables[0].Rows.Add(dr);
                comboBox4.DataSource = ds1.Tables[0];
                comboBox4.ValueMember = "id";
                comboBox4.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }

        }
        public void fillrawitem()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds2 = new DataSet();
                string q = "select * from RawItem";
                ds2 = objCore.funGetDataSet(q);

                DataRow dr = ds2.Tables[0].NewRow();
                dr["ItemName"] = "Please Select";

                ds2.Tables[0].Rows.Add(dr);
                cmbdeal.DataSource = ds2.Tables[0];
                cmbdeal.ValueMember = "id";
                cmbdeal.DisplayMember = "ItemName";
                cmbdeal.Text = "Please Select";
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
                string q = "select * from RawItem where ItemName='" + cmbdeal.Text + "'";
                ds3 = objCore.funGetDataSet(q);
                DataSet dsuom = new DataSet();
                dsuom = objCore.funGetDataSet("select * from UOMConversion where UOMId='" + ds3.Tables[0].Rows[0]["UOMId"].ToString() + "'");
                DataRow dr = dsuom.Tables[0].NewRow();
                dr["UOM"] = "Please Select";
                umcid = dsuom.Tables[0].Rows[0]["id"].ToString();
                dsuom.Tables[0].Rows.Add(dr);
                //txtuom.Text = dsuom.Tables[0].Rows[0]["uom"].ToString();
                
               // comboBox4.Text = "Please Select";

            }
            catch (Exception ex)
            {
                
               
            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            fillgroupmain();
            fillgroup();
            fillmenuitem();
            
           // filluom();
            fillflavours();

            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from AttachMenu where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    textBox2.Text = ds.Tables[0].Rows[0]["no"].ToString();
                    textBox1.Text = ds.Tables[0].Rows[0]["price"].ToString();
                    checkBox2.Checked =Convert.ToBoolean(ds.Tables[0].Rows[0]["Compulsory"].ToString());
                    cmbstatus.Text = ds.Tables[0].Rows[0]["status"].ToString();
                    cmbgroup.SelectedValue = ds.Tables[0].Rows[0]["Menugroupid"].ToString();
                    cmbmenuitem.SelectedValue = ds.Tables[0].Rows[0]["MenuItemId"].ToString();
                    cmbdeal.SelectedValue = ds.Tables[0].Rows[0]["Dealid"].ToString();
                    if (ds.Tables[0].Rows[0]["ModifierId"].ToString() == "" || ds.Tables[0].Rows[0]["ModifierId"].ToString() == "0")
                    {
                    }
                    else
                    {
                        checkBox1.Checked = true;
                        comboBox4.SelectedValue = ds.Tables[0].Rows[0]["ModifierId"].ToString();
                    }
                    vButton2.Text = "Update";
                }
            }
            getdata();
            //try
            //{
            //    if (editmode != 1)
            //    {
            //        cmbgroup.SelectedValue = mgid;
            //        cmbmenuitem.SelectedValue = mtid;
            //    }
            //}
            //catch (Exception ex)
            //{
                
                
            //}
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillmenuitem();
            fillflavours();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void getinfo(string idd)
        {
            id = idd;
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from AttachMenu where id='" + idd + "'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                //txtuom.Text = ds.Tables[0].Rows[0]["UOMCId"].ToString();
                //richTextBox1.Text = ds.Tables[0].Rows[0]["Quantity"].ToString();
                cmbgroup.SelectedValue = ds.Tables[0].Rows[0]["Menugroupid"].ToString();
                cmbmenuitem.SelectedValue = ds.Tables[0].Rows[0]["MenuItemId"].ToString();
                cmbdeal.SelectedValue = ds.Tables[0].Rows[0]["Dealid"].ToString();
                textBox1.Text = ds.Tables[0].Rows[0]["price"].ToString();
                textBox2.Text = ds.Tables[0].Rows[0]["no"].ToString();
                if (ds.Tables[0].Rows[0]["ModifierId"].ToString() == "" || ds.Tables[0].Rows[0]["ModifierId"].ToString() == "0")
                {

                }
                else
                {
                    checkBox1.Checked = true;
                    comboBox4.SelectedValue = ds.Tables[0].Rows[0]["ModifierId"].ToString();
                }
                vButton2.Text = "Update";
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
            fillflavours();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbdeal.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Deal");
                    return;
                }
                if (cmbgroup.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Group");
                    return;
                }
                if (cmbmenuitem.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Item");
                    cmbmenuitem.Focus();
                    return;
                }
                
                if (comboBox4.Text.Trim() == "Please Select" && comboBox4.Visible==true)
                {
                    MessageBox.Show("Please Select a valid Size");
                    comboBox4.Focus();
                    return;
                }
                if (cmbstatus.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select status");
                    cmbmenuitem.Focus();
                    return;
                }
                //if (textBox1.Text.Trim() == "")
                //{
                //    MessageBox.Show("Please enter price");
                //    textBox1.Focus();
                //    return;
                //}
                
                if (textBox1.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(textBox1.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid value. Only Nymbers are allowed");
                        return;
                    }
                }
                if (textBox2.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(textBox2.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid value. Only Nymbers are allowed");
                        return;
                    }
                }
                if (txtgroup.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtgroup.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid value. Only Nymbers are allowed");
                        return;
                    }
                }
                if (vButton2.Text == "Add")
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from AttachMenu");
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
                    if (comboBox4.Visible == false)
                    {
                        q = "select * from AttachMenu where Dealid='" + cmbdeal.SelectedValue+ "' and MenuItemId='" + cmbmenuitem.SelectedValue + "'";
                    }
                    else
                    {
                        q = "select * from AttachMenu where Dealid='" + cmbdeal.SelectedValue + "' and MenuItemId='" + cmbmenuitem.SelectedValue + "' and modifierid='" + comboBox4.SelectedValue + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("This Menu Item Already Exist for Selected Deal");
                        return;
                    }
                    if (comboBox4.Visible == true)
                    {
                        q = "insert into AttachMenu ( Menugroupitem,id,Dealid, MenuItemId,Menugroupid,  ModifierId,price,Compulsory,status,no) values('"+txtgroup.Text+"','" + id + "','" + cmbdeal.SelectedValue + "','" + cmbmenuitem.SelectedValue + "','" + cmbgroup.SelectedValue + "','" + comboBox4.SelectedValue + "','" + textBox1.Text + "','" + checkBox2.Checked + "','" + cmbstatus.Text + "','" + textBox2.Text + "')";

                    }
                    else
                    {
                        q = "insert into AttachMenu ( Menugroupitem,id,Dealid, MenuItemId,Menugroupid,price,Compulsory,status,no) values('" + txtgroup.Text + "','" + id + "','" + cmbdeal.SelectedValue + "','" + cmbmenuitem.SelectedValue + "','" + cmbgroup.SelectedValue + "','" + textBox1.Text + "','" + checkBox2.Checked + "','" + cmbstatus.Text + "','" + textBox2.Text + "')";
                    }
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                   
                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "";
                    if (comboBox4.Visible == true)
                    {
                        q = "update AttachMenu set Menugroupitem='" + txtgroup.Text + "', no='" + textBox2.Text + "', price='" + textBox1.Text + "',Compulsory='" + checkBox2.Checked + "', modifierid='" + comboBox4.SelectedValue + "', Dealid='" + cmbdeal.SelectedValue + "' , Menugroupid='" + cmbgroup.SelectedValue + "' , MenuItemId='" + cmbmenuitem.SelectedValue + "'  where id='" + id + "'";
                    }
                    else
                    {
                        q = "update AttachMenu set Menugroupitem='" + txtgroup.Text + "', no='" + textBox2.Text + "', price='" + textBox1.Text + "',Compulsory='" + checkBox2.Checked + "', Dealid='" + cmbdeal.SelectedValue + "' , Menugroupid='" + cmbgroup.SelectedValue + "' , MenuItemId='" + cmbmenuitem.SelectedValue + "'  where id='" + id + "'";
                    }
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    getdata();
                    MessageBox.Show("Record updated successfully");
                }
                string qq = "SELECT dbo.AttachMenu.id, dbo.Deals.name AS Deal, dbo.MenuItem.Name AS Menuitem, dbo.ModifierFlavour.name AS Size, dbo.AttachMenu.Price,dbo.AttachMenu.Compulsory ,dbo.AttachMenu.No, dbo.AttachMenu.status FROM  dbo.AttachMenu INNER JOIN               dbo.Deals ON dbo.AttachMenu.Dealid = dbo.Deals.id INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id";

                _frm.getdata(qq);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                  
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            //txtuom.Text = string.Empty;
            //richTextBox1.Text = string.Empty;
            cmbgroup.Text = "Please Select";
            cmbdeal.Text = "Please Select";
            textBox1.Text = "";
            textBox2.Text = "";
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
                        string q = "delete from AttachMenu where id='" + id + "'";
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
            if (checkBox1.Checked == true)
            {
                comboBox4.Visible = true;
            }
            else
            {
                comboBox4.Visible = false;
            }
            getdata();
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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

                    MessageBox.Show("Invalid value. Only Nymbers are allowed");
                    return;
                }
            }
        }

        private void cmbdeal_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {
                int Num;
                bool isNum = int.TryParse(txt.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid value. Only Nymbers are allowed");
                    return;
                }
            }
        }
    }
}
