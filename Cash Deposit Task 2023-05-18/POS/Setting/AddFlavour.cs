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
    public partial class AddFlavour : Form
    {
        public static string itemid = "";
        
        
         public AddFlavour()
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
                string q = "";// "SELECT     dbo.ModifierFlavour.Id, dbo.MenuGroup.Name AS MenuGroup, dbo.ModifierFlavour.name, dbo.ModifierFlavour.price FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuGroup ON dbo.ModifierFlavour.MenuItemId = dbo.MenuGroup.Id where dbo.ModifierFlavour.MenuItemId='" + comboBox2.SelectedValue + "'";
                q = "SELECT     dbo.ModifierFlavour.Id, dbo.ModifierFlavour.name,dbo.ModifierFlavour.name2, dbo.ModifierFlavour.price, dbo.ModifierFlavour.Status FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuGroup ON dbo.ModifierFlavour.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.MenuItem ON dbo.ModifierFlavour.MenuItemId = dbo.MenuItem.Id WHERE     (dbo.ModifierFlavour.MenuItemId = '" + comboBox2.SelectedValue + "')";
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
                string q = "select * from MenuItem where MenuGroupId='" + comboBox1.SelectedValue + "' and status='Active'";
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
            string qq = "select * from kds";
            dskds = objCore.funGetDataSet(qq);
            DataRow drck = dskds.Tables[0].NewRow();
            drck["name"] = "Please Select";

            dskds.Tables[0].Rows.Add(drck);
            comboBox4.DataSource = dskds.Tables[0];
            comboBox4.ValueMember = "id";
            comboBox4.DisplayMember = "name";
            comboBox4.Text = "Please Select";
            //comboBox1.Text = "Please Select";
           
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
        public void getinfo(string id)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsinfo = new DataSet();
            string q = "";
            q = "SELECT     dbo.ModifierFlavour.name, dbo.ModifierFlavour.name2, dbo.ModifierFlavour.price, dbo.ModifierFlavour.MenuGroupId, dbo.ModifierFlavour.MenuItemId, dbo.ModifierFlavour.status, dbo.ModifierFlavour.kdsid, dbo.MenuGroup.Name AS Expr1 FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuGroup ON dbo.ModifierFlavour.MenuGroupId  = dbo.MenuGroup.Id  where dbo.ModifierFlavour.id='" + id + "'";
            dsinfo = objCore.funGetDataSet(q);
            if (dsinfo.Tables[0].Rows.Count > 0)
            {

                txtflavourname.Text = dsinfo.Tables[0].Rows[0]["name"].ToString();
                txtname2.Text = dsinfo.Tables[0].Rows[0]["name2"].ToString();
                comboBox1.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuGroupId"].ToString();
                comboBox2.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuItemId"].ToString();
                txtprice.Text = dsinfo.Tables[0].Rows[0]["price"].ToString();

                try
                {
                    txtsuggestedmargin.Text = dsinfo.Tables[0].Rows[0]["ProposedMargin"].ToString();
                    txtcurrentmargin.Text = dsinfo.Tables[0].Rows[0]["currentmargin"].ToString();
                    txtsuggestedprice.Text = dsinfo.Tables[0].Rows[0]["proposedprice"].ToString();
                    txtcost.Text = dsinfo.Tables[0].Rows[0]["cost"].ToString();
                }
                catch (Exception ex)
                {
                    
                    
                }
                double gst = 0;
                DataSet dsq = new DataSet();
                q = "select * from gst";
                dsq = objCore.funGetDataSet(q);
                if (dsq.Tables[0].Rows.Count > 0)
                {
                    string temp = dsq.Tables[0].Rows[0]["gst"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    gst = Convert.ToDouble(temp);

                }
                if (gst > 0)
                {
                    gst = gst / 100;

                }

                double tax = 0;
                if (txtprice.Text == "")
                {
                    txtprice.Text = "0";
                }
                tax = Convert.ToDouble(txtprice.Text) * gst;
                textBox2.Text = Math.Round(tax, 3).ToString();

                textBox1.Text = Math.Round((Math.Round(tax, 3) + Convert.ToDouble(txtprice.Text)), 2).ToString();
                try
                {
                    comboBox4.SelectedValue = dsinfo.Tables[0].Rows[0]["kdsid"].ToString();
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    cmbstatus.SelectedItem = dsinfo.Tables[0].Rows[0]["status"].ToString();
                }
                catch (Exception ex)
                {


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
                if (comboBox1.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Menu Group");
                    return;
                }
                if (txtflavourname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Size/Flavour");
                    return;
                }


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

                if (comboBox4.Text.Trim() == "Please Select" || comboBox4.Text == "")
                {
                    MessageBox.Show("Please Select KDS");
                    return;
                }
                if (cmbstatus.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select Status");
                    cmbstatus.Focus();
                    return;
                }
                if (vButton2.Text == "Add")
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int idd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from ModifierFlavour");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        idd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        idd = 1;
                    }
                    ds = new DataSet();
                    string q = "select * from ModifierFlavour where MenuItemId ='" + comboBox2.SelectedValue + "' and name='" + txtflavourname.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        MessageBox.Show("Flavour/SIze Name Already Exist for this Menu Item");
                        return;

                    }
                    q = "insert into ModifierFlavour (Name2,status,grossprice,currentmargin,ProposedMargin,proposedprice,cost,id,MenuGroupId,MenuItemId,Name,price,kdsid) values(N'" + txtname2.Text + "','" + cmbstatus.Text + "','" + textBox1.Text + "','" + txtcurrentmargin.Text + "','" + txtsuggestedmargin.Text + "','" + txtsuggestedprice.Text + "','" + txtcost.Text + "','" + idd + "','" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue + "','" + txtflavourname.Text.Trim().Replace("'", "''") + "','" + txtprice.Text.Trim().Replace("'", "''") + "','" + comboBox4.SelectedValue + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();

                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "update ModifierFlavour set Name2=N'" + txtname2.Text + "', status='" + cmbstatus.Text + "',grossprice='" + textBox1.Text + "',currentmargin='" + txtcurrentmargin.Text + "',ProposedMargin='" + txtsuggestedmargin.Text + "',proposedprice='" + txtsuggestedprice.Text + "',cost='" + txtcost.Text + "', MenuGroupId='" + comboBox1.SelectedValue + "',MenuItemId='" + comboBox2.SelectedValue + "', kdsid='" + comboBox4.SelectedValue + "',  price='" + txtprice.Text.Trim().Replace("'", "''") + "',  Name='" + txtflavourname.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
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
        private void vButton2_Click(object sender, EventArgs e)
        {
            save();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {

            //txtname.Text = string.Empty;
            txtprice.Text = string.Empty;
            txtflavourname.Text = string.Empty;
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
                        string q = "delete from ModifierFlavour where id='" + id + "'";
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
            getdata();
        }

        private void comboBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            getdata();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            try
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
                updateprice();
                //double gst = 0;
                //DataSet ds = new DataSet();
                //string q = "select * from gst";
                //ds = objCore.funGetDataSet(q);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    string temp = ds.Tables[0].Rows[0]["gst"].ToString();
                //    if (temp == "")
                //    {
                //        temp = "0";
                //    }
                //    gst = Convert.ToDouble(temp);

                //}
                //if (gst > 0)
                //{
                //    gst = gst + 100;
                //    gst = gst / 100;
                //}
                //double val = Convert.ToDouble(txt.Text);
                //double val1 = 0;
                //if (gst > 0)
                //{
                //    val1 = val / gst;
                //}
                //else
                //{
                //    val1 = val;
                //}
                //txtprice.Text = Math.Round(val1, 2).ToString();
                //textBox2.Text = Math.Round((val - val1), 2).ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show("tax calculation Error! " + ex.Message);
            }
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

        private void AddFlavour_KeyDown(object sender, KeyEventArgs e)
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

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                string q = "update Modifierflavour set price='" + dataGridView1.Rows[e.RowIndex].Cells["price"].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                objCore.executeQuery(q);
            }
            if (e.ColumnIndex == 1)
            {
                string q = "update Modifierflavour set Name='" + dataGridView1.Rows[e.RowIndex].Cells["Name"].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                objCore.executeQuery(q);
            }
        }
    }
}
