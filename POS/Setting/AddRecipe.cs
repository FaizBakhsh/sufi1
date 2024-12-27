using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class AddRecipe : Form
    {
        public static string itemid = "";
        public string umcid = "";
        public string mgid = "";
        public string mtid = "";
         POSRestaurant.forms.MainForm _frm;
         public AddRecipe(POSRestaurant.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }
         POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void button1_Click(object sender, EventArgs e)
        {
            
           
        }
        public void getdata()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("RawItem", typeof(string));
            dt.Columns.Add("Quantity", typeof(string));
            dt.Columns.Add("Price", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            dt.Columns.Add("Type", typeof(string));
            try
            {
                double totalamount = 0;
                DataSet dsdata = new DataSet();
                string q = "";
                if (comboBox4.Visible == true)
                {
                    q = "SELECT        dbo.Recipe.Id, dbo.RawItem.ItemName +'('+ dbo.UOMConversion.UOM+')' AS RawItem, dbo.Recipe.UOMCId, dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.UOMConversion.ConversionRate FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + comboBox2.SelectedValue + "' and dbo.Recipe.modifierid='" + comboBox4.SelectedValue + "'";
                }
                else
                {
                    q = "SELECT        dbo.Recipe.Id, dbo.RawItem.ItemName +'('+ dbo.UOMConversion.UOM+')' AS RawItem, dbo.Recipe.UOMCId, dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.UOMConversion.ConversionRate FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + comboBox2.SelectedValue + "' ";
                }
                dsdata = objCore.funGetDataSet(q);
                for (int i = 0; i < dsdata.Tables[0].Rows.Count; i++)
                {
                    double price = 0, qty = 0, amount = 0, convrate = 0;
                    string temp = dsdata.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    qty = float.Parse(temp);
                    temp = dsdata.Tables[0].Rows[i]["ConversionRate"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    convrate = float.Parse(temp);
                    price = Math.Round(getprice(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), dsdata.Tables[0].Rows[i]["RawItemId"].ToString()), 4);
                    amount = Math.Round((price * qty), 4);
                    amount = Math.Round((amount / convrate), 4);
                    totalamount = totalamount + amount;
                   
                    dt.Rows.Add(dsdata.Tables[0].Rows[i]["Id"].ToString(), dsdata.Tables[0].Rows[i]["RawItem"].ToString(), dsdata.Tables[0].Rows[i]["Quantity"].ToString(), price, amount, dsdata.Tables[0].Rows[i]["type"].ToString());
                }
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                label7.Text ="Total Amount:  "+ totalamount.ToString("N", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
  
            }

        }
        public double getprice(string start, string end, string id)
        {

            double cost = 0;

            string q = "select  dbo.Getprice('" + start + "','" + end + "'," + id + ")";
            try
            {
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {
            }

            return cost;
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
                string q = "select * from MenuGroup order by name";
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
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds1 = new DataSet();
                string q = "select * from MenuItem where MenuGroupId='" + comboBox1.SelectedValue + "' and status='Active' order by name";
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
        public void fillflavours()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds1 = new DataSet();
                string q = "select * from ModifierFlavour where MenuItemId='" + comboBox2.SelectedValue + "'";
                ds1 = objCore.funGetDataSet(q);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    checkBox1.Checked = true;
                }
                else
                {
                    checkBox1.Checked = false;
                }
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
            fillmenuitem();
            fillrawitem();
            filluom();
            fillflavours();

            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                    dss = objCore.funGetDataSet("select * from MenuItem where id='" + comboBox2.SelectedValue + "'");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        comboBox1.SelectedValue = dss.Tables[0].Rows[0]["MenuGroupId"].ToString();
                    }
                    vButton2.Text = "Update";
                    cmbtype.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                }
            }
            getdata();
            try
            {
                if (editmode != 1)
                {
                    comboBox1.SelectedValue = mgid;
                    comboBox2.SelectedValue = mtid;
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
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
            fillflavours();
        }
        protected void save()
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
                if (comboBox4.Text.Trim() == "Please Select" && comboBox4.Visible == true)
                {
                    MessageBox.Show("Please Select a valid Size");
                    comboBox4.Focus();
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
                    string q = "";
                    if (comboBox4.Visible == false)
                    {
                        q = "select * from Recipe where RawItemId='" + comboBox3.SelectedValue + "' and MenuItemId='" + comboBox2.SelectedValue + "'";
                    }
                    else
                    {
                        q = "select * from Recipe where RawItemId='" + comboBox3.SelectedValue + "' and MenuItemId='" + comboBox2.SelectedValue + "' and modifierid='" + comboBox4.SelectedValue + "'";
                    }
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        DialogResult dr = MessageBox.Show("This Raw Item Already Exist for Selected Menu Item\\Size", "Do you Want to save Instead?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dr == DialogResult.No)
                        {
                            return;
                        }
                    }
                    if (comboBox4.Visible == true)
                    {
                        q = "insert into Recipe (type,id,RawItemId,MenuItemId,UOMCId,Quantity,modifierid) values('" + cmbtype.Text + "','" + id + "','" + comboBox3.SelectedValue + "','" + comboBox2.SelectedValue + "','" + umcid + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox4.SelectedValue + "')";

                    }
                    else
                    {
                        q = "insert into Recipe (type,id,RawItemId,MenuItemId,UOMCId,Quantity,modifierid) values('" + cmbtype.Text + "','" + id + "','" + comboBox3.SelectedValue + "','" + comboBox2.SelectedValue + "','" + umcid + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "','')";

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
                        q = "update Recipe set type='" + cmbtype.Text + "',modifierid='" + comboBox4.SelectedValue + "', RawItemId='" + comboBox3.SelectedValue + "' , UOMCId ='" + umcid + "', Quantity ='" + richTextBox1.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                    }
                    else
                    {
                        q = "update Recipe set type='" + cmbtype.Text + "', RawItemId='" + comboBox3.SelectedValue + "' , UOMCId ='" + umcid + "', Quantity ='" + richTextBox1.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                    }
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    getdata();
                    MessageBox.Show("Record updated successfully");
                }
                comboBox3.Focus();
                _frm.getdata("select * from Recipe");
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

        private void vButton6_Click(object sender, EventArgs e)
        {
            AttachSubRecipe obj = new AttachSubRecipe();
            obj.Show();
        }

        private void AddRecipe_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                save();
            }
            if (e.KeyCode == Keys.Escape)
            {
                comboBox1.Focus();
            }
        }
    }
}
