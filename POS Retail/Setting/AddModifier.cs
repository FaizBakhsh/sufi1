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
    public partial class AddModifier : Form
    {
        public static string itemid = "";
        
         POSRetail.forms.MainForm _frm;
         public AddModifier(POSRetail.forms.MainForm frm)
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
                string q = "SELECT     dbo.Modifier.Id, dbo.Modifier.Name, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.RawItem.ItemName AS RawItem, dbo.Modifier.Price FROM         dbo.Modifier INNER JOIN                      dbo.MenuGroup ON dbo.Modifier.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.MenuItem ON dbo.Modifier.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.RawItem ON dbo.Modifier.RawItemId = dbo.RawItem.Id where dbo.Modifier.MenuItemId='" + comboBox2.SelectedValue + "'";
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

        public void fillboxes()
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dss = new DataSet();
            string qq = "select * from Modifier where MenuItemId='" + comboBox2.SelectedValue + "'";
            dss = objCore.funGetDataSet(qq);
            if (dss.Tables[0].Rows.Count > 0)
            {
               // txtprice.Text = ds.Tables[0].Rows[0]["price"].ToString();
                txtname.Text = dss.Tables[0].Rows[0]["name"].ToString();
                //comboBox1.SelectedValue = ds.Tables[0].Rows[0]["MenuGroupId"].ToString();
                //comboBox2.SelectedValue = ds.Tables[0].Rows[0]["MenuItemId"].ToString();
                //comboBox3.SelectedValue = ds.Tables[0].Rows[0]["RawItemId"].ToString();


                
            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            fillgroup();
            fillmenuitem();
            fillrawitem();
            
            if (editmode == 1)
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Modifier where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtprice.Text = ds.Tables[0].Rows[0]["price"].ToString();
                    txtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["MenuGroupId"].ToString();
                    comboBox2.SelectedValue = ds.Tables[0].Rows[0]["MenuItemId"].ToString();
                    comboBox3.SelectedValue = ds.Tables[0].Rows[0]["RawItemId"].ToString();
                    
                  
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
            //txtname.Text = string.Empty;
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void getinfo(string id)
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsinfo = new DataSet();
            string qry = "select * from Modifier where id='" + id + "'";
            dsinfo = objCore.funGetDataSet(qry);
            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                comboBox1.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuGroupId"].ToString();
                comboBox2.SelectedValue = dsinfo.Tables[0].Rows[0]["MenuItemId"].ToString();
                comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["RawItemId"].ToString();
                txtname.Text = dsinfo.Tables[0].Rows[0]["name"].ToString();
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
            fillboxes();
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
                    MessageBox.Show("Please Select a valid Menu Group");
                    return;
                }
                if (comboBox2.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Item");
                    return;
                }
                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Modifier");
                    return;
                }
                if (comboBox3.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Raw Item");
                    return;
                }

                //if (txtprice.Text.Trim() == string.Empty)
                //{
                //    MessageBox.Show("Please Enter Price");
                //    return;
                //}

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
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Modifier");
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
                    string q = "select * from Modifier where MenuItemId='" + comboBox2.SelectedValue + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            if (ds.Tables[0].Rows[i]["name"].ToString() == txtname.Text.Trim() && ds.Tables[0].Rows[i]["RawItemId"].ToString() != comboBox3.SelectedValue.ToString())
                            {

                            }
                            else
                            {
                                MessageBox.Show("This Raw Item Already Exist for this Modifier");
                                return;
                            }
                        }
                    }
                    q = "insert into Modifier (id,MenuGroupId,RawItemId,MenuItemId,Name,price) values('" + id + "','" + comboBox1.SelectedValue + "','" + comboBox3.SelectedValue + "','" + comboBox2.SelectedValue + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtprice.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    
                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "update Modifier set RawItemId='" + comboBox3.SelectedValue + "' ,MenuItemId='" + comboBox3.SelectedValue + "' , MenuGroupId ='" + comboBox1.SelectedValue + "',  price='" + txtprice.Text.Trim().Replace("'", "''") + "',  Name='" + txtname.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    getdata();
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Modifier");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {

            txtname.Text = string.Empty;
            txtprice.Text = string.Empty;
            comboBox1.Text = "Please Select";
            comboBox3.Text = "Please Select";
            comboBox2.Text = "Please Select";
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
                        string q = "delete from Modifier where id='" + id + "'";
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
                    AddFlavour obj = new AddFlavour();
                    obj.id = id;
                    obj.Show();
                    this.Hide();

                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
