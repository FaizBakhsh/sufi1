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
    public partial class attachmenu : Form
    {
        public static string itemid = "";


        public attachmenu()
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
        public void getdata()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsdata = new DataSet();
                string q = "";
                if (checkBox2.Checked == false)
                {
                    q = "SELECT       dbo.Attachmenu1.id,dbo.ModifierFlavour.name AS size, dbo.MenuItem.Name,   dbo.Attachmenu1.UseRecipe,dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.attachmenuid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id   where dbo.Attachmenu1.menuitemid='" + comboBox2.SelectedValue + "'  and (type='MenuItem' or type is null)";
                }
                else
                {
                    q = "SELECT       dbo.Attachmenu1.id,dbo.ModifierFlavour.name AS size, dbo.MenuItem.Name,   dbo.Attachmenu1.UseRecipe,dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.attachmenuid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id   where dbo.Attachmenu1.menuitemid='" + cmbruntimemodfier.SelectedValue + "'  and type='RuntimeModifier'";
                }
                dsdata = objCore.funGetDataSet(q);
                dataGridView1.DataSource = dsdata.Tables[0];
                dataGridView1.Columns[0].Visible = false;

            }
            catch (Exception ex)
            {


            }

        }
        private void btnexit_Click(object sender, EventArgs e)
        {

        }

        public void filluom()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds3 = new DataSet();
                string q = "select * from MenuItem where MenuGroupId='" + comboBox1.SelectedValue + "' order by name";
                ds3 = objCore.funGetDataSet(q);

                DataRow dr = ds3.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds3.Tables[0].Rows.Add(dr);

                comboBox2.DataSource = ds3.Tables[0];
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "Name";

            }
            catch (Exception ex)
            {


            }
        }
        public void fillruntime()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds3 = new DataSet();
                string q = "select * from RuntimeModifier where menuitemid='" + comboBox2.SelectedValue + "' order by name";
                ds3 = objCore.funGetDataSet(q);               
                cmbruntimemodfier.DataSource = ds3.Tables[0];
                cmbruntimemodfier.ValueMember = "id";
                cmbruntimemodfier.DisplayMember = "Name";

            }
            catch (Exception ex)
            {


            }
        }
        public void fillflavour()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds3 = new DataSet();
                string q = "select * from ModifierFlavour where menuitemid='" + cmbattach.SelectedValue + "' order by name";
                ds3 = objCore.funGetDataSet(q);

                DataRow dr = ds3.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds3.Tables[0].Rows.Add(dr);

                cmbflavour.DataSource = ds3.Tables[0];
                cmbflavour.ValueMember = "id";
                cmbflavour.DisplayMember = "Name";

            }
            catch (Exception ex)
            {


            }
        }
        public void fillattachmenu()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds3 = new DataSet();
                string q = "select * from MenuItem order by name";
                ds3 = objCore.funGetDataSet(q);

                DataRow dr = ds3.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds3.Tables[0].Rows.Add(dr);

                cmbattach.DataSource = ds3.Tables[0];
                cmbattach.ValueMember = "id";
                cmbattach.DisplayMember = "Name";

            }
            catch (Exception ex)
            {


            }
        }


        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from MenuGroup";
            ds = objCore.funGetDataSet(q);
            DataRow dr = ds.Tables[0].NewRow();
            dr["Name"] = "Please Select";
            ds.Tables[0].Rows.Add(dr);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "Name";
            DataSet dskds = new DataSet();

            //comboBox1.Text = "Please Select";
            fillattachmenu();
            getdata();
            //if (editmode == 1)
            //{

            //    q = "SELECT     dbo.ModifierFlavour.name, dbo.ModifierFlavour.price, dbo.MenuItem.Code, dbo.MenuItem.Name AS Expr1, dbo.ModifierFlavour.MenuItemId FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuItem ON dbo.ModifierFlavour.MenuItemId = dbo.MenuItem.Id where dbo.ModifierFlavour.id='" + comboBox1.SelectedValue + "'";

            //    q = "SELECT     dbo.ModifierFlavour.name, dbo.ModifierFlavour.price, dbo.ModifierFlavour.MenuItemId, dbo.MenuGroup.Name AS Expr1 FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuGroup ON dbo.ModifierFlavour.MenuItemId = dbo.MenuGroup.Id where dbo.ModifierFlavour.id='" + comboBox1.SelectedValue + "'";
            //    ds = objCore.funGetDataSet(q);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        txtprice.Text = ds.Tables[0].Rows[0]["price"].ToString();
            //        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["ModifierName"].ToString();
            //        txtflavourname.Text = ds.Tables[0].Rows[0]["name"].ToString();
            //        itemid = ds.Tables[0].Rows[0]["id"].ToString();

            //        vButton2.Text = "Update";
            //    }
            //}
            try
            {
               string query = "ALTER TABLE [dbo].[Attachmenu1]  ADD PrintKitchen varchar(50) NULL ";
               objCore.executeQuery(query);
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
            //txtname.Text = string.Empty;

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public static string qryupdate = "";
        public void getinfo(string id)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsinfo = new DataSet();
            string q = "";
            if (checkBox2.Checked == true)
            {
                q = "SELECT       dbo.Attachmenu1.PrintKitchen,  dbo.Attachmenu1.id, dbo.Attachmenu1.userecipe, dbo.Attachmenu1.attachmenuid, dbo.Attachmenu1.attachFlavourid, dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status, MenuItem_1.MenuGroupId,                          dbo.RuntimeModifier.menuItemid FROM            dbo.MenuItem INNER JOIN                         dbo.Attachmenu1 ON dbo.MenuItem.Id = dbo.Attachmenu1.attachmenuid INNER JOIN                         dbo.RuntimeModifier ON dbo.Attachmenu1.menuitemid = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem AS MenuItem_1 ON dbo.RuntimeModifier.menuItemid = MenuItem_1.Id WHERE     dbo.Attachmenu1.id='" + id + "'";
            }
            else
            {

                q = "SELECT       dbo.Attachmenu1.PrintKitchen, dbo.Attachmenu1.id, dbo.Attachmenu1.menuitemid, dbo.Attachmenu1.userecipe,dbo.Attachmenu1.attachmenuid, dbo.Attachmenu1.attachFlavourid, dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status, dbo.MenuGroup.Id AS menugroupid FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Attachmenu1 ON dbo.MenuItem.Id = dbo.Attachmenu1.attachmenuid where dbo.Attachmenu1.id='" + id + "'";
            }
            dsinfo = objCore.funGetDataSet(q);
            if (dsinfo.Tables[0].Rows.Count > 0)
            {

                //  txtflavourname.Text = dsinfo.Tables[0].Rows[0]["name"].ToString();
                comboBox1.SelectedValue = dsinfo.Tables[0].Rows[0]["menugroupid"].ToString();
                comboBox2.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuItemId"].ToString();

                try
                {
                    fillflavour();
                    cmbflavour.SelectedValue = dsinfo.Tables[0].Rows[0]["attachFlavourid"].ToString();
                }
                catch (Exception ex)
                {
                    
                }

                cmbattach.SelectedValue = dsinfo.Tables[0].Rows[0]["attachmenuid"].ToString();

               
                if (dsinfo.Tables[0].Rows[0]["userecipe"].ToString() == "yes")
                {
                    chkrecipe.Checked = true;
                }
                //qryupdate = "delete from recipe where menuitemid='" + dsinfo.Tables[0].Rows[0]["MenuItemId"].ToString() + "' and attachmenuid='" + dsinfo.Tables[0].Rows[0]["attachmenuid"].ToString() + "'";

                comboBox4.Text = dsinfo.Tables[0].Rows[0]["status"].ToString();
                textBox1.Text = dsinfo.Tables[0].Rows[0]["quantity"].ToString();
                double gst = 0;
                try
                {
                    if (dsinfo.Tables[0].Rows[0]["PrintKitchen"].ToString() == "yes")
                    {
                        chkprintkitchen.Checked = true;
                    }
                }
                catch (Exception ex)
                {
                    
                }
                //DataSet dsq = new DataSet();
                //q = "select * from gst";
                //dsq = objCore.funGetDataSet(q);
                //if (dsq.Tables[0].Rows.Count > 0)
                //{
                //    string temp = dsq.Tables[0].Rows[0]["gst"].ToString();
                //    if (temp == "")
                //    {
                //        temp = "0";
                //    }
                //    gst = Convert.ToDouble(temp);

                //}
                //if (gst > 0)
                //{
                //    gst = gst / 100;

                //}

                //double tax = 0;
                //if (txtprice.Text == "")
                //{
                //    txtprice.Text = "0";
                //}
                //tax = Convert.ToDouble(txtprice.Text) * gst;
                //textBox2.Text = Math.Round(tax, 3).ToString();

                //textBox1.Text = Math.Round((Math.Round(tax, 3) + Convert.ToDouble(txtprice.Text)), 2).ToString();

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

        private void txtname_TextChanged(object sender, EventArgs e)
        {
            if (txtprice.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtprice.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                    return;
                }
            }
            updateprice1();
        }
        protected void save()
        {
            try
            {
                if (comboBox2.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Menu Item");
                    comboBox2.Focus();
                    return;
                }
                if (cmbattach.Text.Trim() == string.Empty || cmbattach.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Menu Item To Attach");
                    cmbattach.Focus();
                    return;
                }


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
                        MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                        return;
                    }
                }

                string userecipe = "",printkitchen="", flvid = "";

                try
                {
                    flvid = cmbflavour.SelectedValue.ToString();
                }
                catch (Exception ex)
                {
                   
                }

                if (chkrecipe.Checked == true)
                {
                    userecipe = "yes";
                }
                if (chkprintkitchen.Checked == true)
                {
                    printkitchen = "yes";
                }
                string type = "MenuItem", menuid = comboBox2.SelectedValue.ToString();
                if (checkBox2.Checked == true)
                {
                    type = "RuntimeModifier";
                    menuid = cmbruntimemodfier.SelectedValue.ToString();
                }
               
                if (vButton2.Text == "Add")
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int idd = 0;

                    ds = new DataSet();
                    string q = "select * from Attachmenu1 where MenuItemId ='" + comboBox2.SelectedValue + "' and attachmenuid='" + cmbattach.SelectedValue + "' and (type='MenuItem' or type is null)";
                    if (flvid != "")
                    {
                        q = "select * from Attachmenu1 where MenuItemId ='" + comboBox2.SelectedValue + "' and attachmenuid='" + cmbattach.SelectedValue + "' and attachFlavourid='" + flvid + "' and (type='MenuItem' or type is null)";
                    }
                    if (checkBox2.Checked == true)
                    {
                        q = "select * from Attachmenu1 where MenuItemId ='" + cmbruntimemodfier.SelectedValue + "' and attachmenuid='" + cmbattach.SelectedValue + "' and type='RuntimeModifier'";
                    }
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        MessageBox.Show("Record Name Already Exist for this Menu Item");
                        return;
                    }


                    q = "insert into Attachmenu1 (PrintKitchen,type,attachFlavourid,userecipe,menuitemid, attachmenuid, quantity, status) values('" + printkitchen + "','" + type + "','" + flvid + "','" + userecipe + "','" + menuid + "','" + cmbattach.SelectedValue + "','" + textBox1.Text.Trim().Replace("'", "''") + "','" + comboBox4.Text + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    
                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "update Attachmenu1 set PrintKitchen='" + printkitchen + "',type='" + type + "',userecipe='" + userecipe + "',attachmenuid='" + cmbattach.SelectedValue + "',attachFlavourid='" + flvid + "',MenuItemId='" + menuid + "', status='" + comboBox4.Text + "',  quantity='" + textBox1.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    objCore.executeQuery(qryupdate);
                    qryupdate = "";
                    
                    getdata();
                    MessageBox.Show("Record updated successfully");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            save();
        }
       
        private void vButton1_Click(object sender, EventArgs e)
        {

            //txtname.Text = string.Empty;
            txtprice.Text = string.Empty;
            //   txtflavourname.Text = string.Empty;
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
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {


                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (msg == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                        string q = "SELECT        id,  menuitemid, attachmenuid, quantity, status FROM            Attachmenu1 where id='" + id + "'";
                        DataSet dsinfo = new DataSet();
                        dsinfo = objCore.funGetDataSet(q);
                        if (dsinfo.Tables[0].Rows.Count > 0)
                        {
                           
                        }

                        q = "delete from Attachmenu1 where id='" + id + "'";
                        objCore.executeQuery(q);

                        getdata();

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton6_Click(object sender, EventArgs e)
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            filluom();
            fillruntime();
            getdata();
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            fillruntime();
            getdata();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        public void updateprice()
        {
            try
            {
                TextBox txt = textBox1 as TextBox;
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
                //if (checkBox1.Checked == false)
                //{
                //    richTextBox2.Text = txtprice.Text;
                //    textBox3.Text = "0";
                //    return;
                //}
                double gst = 0;
                DataSet ds = new DataSet();
                string q = "select * from gst";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0]["gst"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    gst = Convert.ToDouble(temp);

                }
                if (gst > 0)
                {
                    gst = gst + 100;
                    gst = gst / 100;
                    double val = Convert.ToDouble(txt.Text);
                    double val1 = val / gst;
                    txtprice.Text = Math.Round(val1, 2).ToString();
                    textBox2.Text = Math.Round((val - val1), 2).ToString();
                }
                else
                {
                    txtprice.Text = textBox1.Text;
                    textBox2.Text = "0";
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show("tax calculation Error! " + ex.Message);
            }
        }
        public void updateprice1()
        {
            try
            {
                TextBox txt = txtprice as TextBox;
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
                //if (checkBox1.Checked == false)
                //{
                //    richTextBox2.Text = txtprice.Text;
                //    textBox3.Text = "0";
                //    return;
                //}
                double gst = 0;
                DataSet ds = new DataSet();
                string q = "select * from gst";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0]["gst"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    gst = Convert.ToDouble(temp);

                }
                if (gst > 0)
                {
                    gst = gst;
                    gst = gst / 100;
                    double val = Convert.ToDouble(txt.Text);
                    double val1 = val * gst;
                    textBox2.Text = Math.Round(val1, 2).ToString();
                    textBox1.Text = Math.Round((val + val1), 2).ToString();
                }
                else
                {
                    textBox1.Text = txtprice.Text;
                    //textBox3.Text = "0";
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show("tax calculation Error! " + ex.Message);
            }
        }
        private void txtflavourname_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                txtprice.ReadOnly = true;
                textBox1.ReadOnly = false;
            }
            else
            {
                txtprice.ReadOnly = false;
                textBox1.ReadOnly = true;
            }
        }

        private void attachmenu_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                save();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void chkrecipe_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cmbattach_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillflavour();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                cmbruntimemodfier.Visible = true;
            }
            else
            {
                cmbruntimemodfier.Visible = false;
            }
            getdata();
        }

        private void cmbruntimemodfier_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }
    }
}
