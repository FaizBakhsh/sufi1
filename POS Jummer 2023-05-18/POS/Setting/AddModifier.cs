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
    public partial class AddModifier : Form
    {
        public static string itemid = "";
        
         POSRestaurant.forms.MainForm _frm;
         public AddModifier(POSRestaurant.forms.MainForm frm)
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
                string q = "select Id,  Name, Price from Modifier";
                q = "SELECT     dbo.Modifier.Id, dbo.Modifier.Name, dbo.Modifier.Name2, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.RawItem.ItemName FROM         dbo.Modifier INNER JOIN                      dbo.RawItem ON dbo.Modifier.RawItemId = dbo.RawItem.Id";
                dsdata = objCore.funGetDataSet(q);
                dataGridView1.DataSource = dsdata.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                

                dataGridView1.DefaultCellStyle.BackColor = Color.White;
                dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
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
        //        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        //        DataSet ds3 = new DataSet();
        //        string q = "select * from RawItem where ItemName='" + comboBox3.Text + "'";
        //        ds3 = objCore.funGetDataSet(q);
        //        DataSet dsuom = new DataSet();
        //        dsuom = objCore.funGetDataSet("select * from UOMConversion where UOMId='" + ds3.Tables[0].Rows[0]["UOMId"].ToString() + "'");
        //        DataRow dr = dsuom.Tables[0].NewRow();
        //        dr["UOM"] = "Please Select";

        //        dsuom.Tables[0].Rows.Add(dr);
        //        txtprice.Text = dsuom.Tables[0].Rows[0]["uom"].ToString(); ;

        //        // comboBox4.Text = "Please Select";

        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
       
        private void AddGroups_Load(object sender, EventArgs e)
        {
            
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
            fillrawitem();
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
            //txtname.Text = string.Empty;
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void getinfo(string id)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsinfo = new DataSet();
            string qry = "select * from Modifier where id='" + id + "'";
            dsinfo = objCore.funGetDataSet(qry);
            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                
                txtname.Text = dsinfo.Tables[0].Rows[0]["name"].ToString();
                txtname2.Text = dsinfo.Tables[0].Rows[0]["name2"].ToString();
                txtprice.Text = dsinfo.Tables[0].Rows[0]["price"].ToString();
                textBox1.Text = dsinfo.Tables[0].Rows[0]["Quantity"].ToString();
                vButton2.Text = "Update";
                itemid = id;
                comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["RawItemId"].ToString();
                comboBox4.SelectedValue = dsinfo.Tables[0].Rows[0]["kdsid"].ToString();
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
           
        }
        protected void save()
        {
            try
            {

                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Modifier");
                    txtname.Focus();
                    return;
                }


                if (txtprice.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Price");
                    return;
                }
                if (comboBox3.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Raw Item");
                    comboBox3.Focus();
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
                        txtprice.Focus();
                        return;
                    }
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

                        MessageBox.Show("Invalid Quantity. Only Nymbers are allowed");
                        textBox1.Focus();
                        return;
                    }
                }

                if (comboBox4.Text.Trim() == "Please Select" || comboBox4.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select KDS");
                    comboBox4.Focus();
                    return;
                }
                string gst = "0";
                DataSet ds = objCore.funGetDataSet("select * from gst");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0]["gst"].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    gst = i;
                }
                double gross = Convert.ToDouble(txtprice.Text);

                float g = float.Parse(gst) + 100;
                g = g / 100;
                gross = gross / g;
                if (vButton2.Text == "Add")
                {

                    ds = new DataSet();
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
                    string q = "select * from Modifier where Name='" + txtname.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Data Already Exist");
                        return;

                    }
                    q = "insert into Modifier (Name2,grossprice,id,RawItemId,Name,price,kdsid,Quantity) values(N'" + txtname2.Text + "','" + gross + "','" + id + "','" + comboBox3.SelectedValue + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtprice.Text.Trim().Replace("'", "''") + "','" + comboBox4.SelectedValue + "','" + textBox1.Text + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();

                    //  getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string q = "update Modifier set Name2=N'" + txtname2.Text + "',grossprice='" + gross + "',RawItemId='" + comboBox3.SelectedValue + "',Quantity='" + textBox1.Text + "', kdsid='" + comboBox4.SelectedValue + "', price='" + txtprice.Text.Trim().Replace("'", "''") + "',  Name='" + txtname.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";

                    objCore.executeQuery(q);
                    // getdata();
                    MessageBox.Show("Record updated successfully");
                }
                getdata();
                //_frm.getdata("SELECT     dbo.Modifier.Id, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.KDS.Name AS kds FROM         dbo.Modifier LEFT OUTER JOIN                      dbo.KDS ON dbo.Modifier.kdsid = dbo.KDS.Id where dbo.Modifier.MenuItemId='" + comboBox3.SelectedValue + "'");
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

            txtname.Text = string.Empty;
            txtprice.Text = string.Empty;
            
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

        private void txtprice_TextChanged(object sender, EventArgs e)
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

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
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

                    MessageBox.Show("Invalid Quantity. Only Nymbers are allowed");
                    return;
                }
            }
        }

        private void AddModifier_KeyDown(object sender, KeyEventArgs e)
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
    }
}
