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
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsdata = new DataSet();
                string q = "SELECT    dbo.ModifierFlavour.id, dbo.ModifierFlavour.Name, dbo.ModifierFlavour.Price, dbo.MenuItem.Name AS MenuItem,dbo.MenuItem.Code FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuItem ON dbo.ModifierFlavour.MenuItemId = dbo.MenuItem.Id where dbo.ModifierFlavour.MenuItemId='" + comboBox1.SelectedValue + "'";
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
       
        //public void filluom()
        //{
        //    try
        //    {
        //        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        //        DataSet ds3 = new DataSet();
        //        string q = "select * from RawItem where ItemName='" + comboBox3.Text + "'";
        //        ds3 = objCore.funGetDataSet(q);
        //        DataSet dsuom = new DataSet();
        //        dsuom = objCore.funGetDataSet("select * from UOMConversion where UOMId='" + ds3.Tables[0].Rows[0]["UOMId"].ToString() + "'");
        //        DataRow dr = dsuom.Tables[0].NewRow();
        //        dr["UOM"] = "Please Select";

        //        dsuom.Tables[0].Rows.Add(dr);
        //        txtprice.Text = dsuom.Tables[0].Rows[0]["uom"].ToString(); ;
                
        //       // comboBox4.Text = "Please Select";

        //    }
        //    catch (Exception ex)
        //    {
                
               
        //    }
        //}

       
        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from Menuitem";
            ds = objCore.funGetDataSet(q);
            DataRow dr = ds.Tables[0].NewRow();
            dr["Name"] = "Please Select";

            ds.Tables[0].Rows.Add(dr);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "Name";

            //comboBox1.Text = "Please Select";
           
            getdata();
            if (editmode == 1)
            {

                q = "SELECT     dbo.ModifierFlavour.name, dbo.ModifierFlavour.price, dbo.MenuItem.Code, dbo.MenuItem.Name AS Expr1, dbo.ModifierFlavour.MenuItemId FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuItem ON dbo.ModifierFlavour.MenuItemId = dbo.MenuItem.Id where dbo.ModifierFlavour.id='" + comboBox1.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtprice.Text = ds.Tables[0].Rows[0]["price"].ToString();
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["ModifierName"].ToString();
                    txtflavourname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    itemid = ds.Tables[0].Rows[0]["id"].ToString();

                    vButton2.Text = "Update";
                }
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
        public void getinfo(string id)
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsinfo = new DataSet();
            string q = "SELECT     dbo.ModifierFlavour.name, dbo.ModifierFlavour.price, dbo.MenuItem.Code, dbo.MenuItem.Name AS Expr1, dbo.ModifierFlavour.MenuItemId FROM         dbo.ModifierFlavour INNER JOIN                      dbo.MenuItem ON dbo.ModifierFlavour.MenuItemId = dbo.MenuItem.Id where dbo.ModifierFlavour.id='" + id + "'";
            dsinfo = objCore.funGetDataSet(q);
            if (dsinfo.Tables[0].Rows.Count > 0)
            {

                txtflavourname.Text = dsinfo.Tables[0].Rows[0]["name"].ToString();
                comboBox1.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuItemId"].ToString();
                txtprice.Text = dsinfo.Tables[0].Rows[0]["price"].ToString();
                
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
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Menu item");
                    return;
                }
                if (txtflavourname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Flavour");
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
                if (vButton2.Text == "Add")
                {

                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                    string q = "select * from ModifierFlavour where MenuItemId ='" + comboBox1.SelectedValue + "' and name='" + txtflavourname.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        MessageBox.Show("Flavour Name Already Exist for this Menu Item");
                        return;
                           
                    }
                    q = "insert into ModifierFlavour (id,MenuItemId,Name,price) values('" + idd + "','" + comboBox1.SelectedValue + "','" + txtflavourname.Text.Trim().Replace("'", "''") + "','" + txtprice.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    
                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "update ModifierFlavour set   price='" + txtprice.Text.Trim().Replace("'", "''") + "',  Name='" + txtflavourname.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
            getdata();
        }
    }
}
