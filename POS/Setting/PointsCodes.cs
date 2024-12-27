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
    public partial class PointsCodes : Form
    {
        public PointsCodes()
        {
            InitializeComponent();
        }
        public void fillgroup()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from MenuGroup where  status='Active' order by name";
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
        public void fillflavour()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds1 = new DataSet();
                string q = "select * from Modifierflavour where menuitemid='" + comboBox2.SelectedValue + "'";
                ds1 = objCore.funGetDataSet(q);

                //DataRow dr = ds1.Tables[0].NewRow();
                //dr["Name"] = "Please Select";

                //ds1.Tables[0].Rows.Add(dr);
                comboBox3.DataSource = ds1.Tables[0];
                comboBox3.ValueMember = "id";
                comboBox3.DisplayMember = "Name";
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    comboBox3.Visible = true;
                    lblflavour.Visible = true;
                }
                else
                {
                    comboBox3.Visible = false;
                    lblflavour.Visible = false;
                }
            }
            catch (Exception ex)
            {


            }

        }
        public void getdata()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsdata = new DataSet();
                string q = "";
                if (textBox1.Text == "")
                {
                    q = "SELECT        TOP (100) PERCENT dbo.Points.Id, dbo.MenuItem.Name, dbo.ModifierFlavour.name AS Size, dbo.Points.Code, dbo.Points.Time AS TimeUsed, dbo.Points.Status FROM            dbo.Points INNER JOIN                         dbo.MenuItem ON dbo.Points.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Points.Flavourid = dbo.ModifierFlavour.Id order by dbo.Points.Code,dbo.MenuItem.Name";
                }
                else
                {
                    q = "SELECT        TOP (100) PERCENT dbo.Points.Id, dbo.MenuItem.Name, dbo.ModifierFlavour.name AS Size, dbo.Points.Code, dbo.Points.Time AS TimeUsed, dbo.Points.Status FROM            dbo.Points INNER JOIN                         dbo.MenuItem ON dbo.Points.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Points.Flavourid = dbo.ModifierFlavour.Id where dbo.MenuItem.Name like '%" + textBox1.Text + "%' or  dbo.Points.Code like '%" + textBox1.Text + "%' order by dbo.Points.Code,dbo.MenuItem.Name";
                }
                dsdata = objCore.funGetDataSet(q);
                dataGridView1.DataSource = dsdata.Tables[0];
                dataGridView1.Columns[0].Visible = false;
               
            }
            catch (Exception ex)
            {


            }

        } 
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        protected void edit(string id)
        {
            try
            {
              
                DataSet ds = new DataSet();
                string q = "select * from Points where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    txtcode.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                    comboBox2.SelectedValue = ds.Tables[0].Rows[0]["MenuItemId"].ToString();


                    DataSet dss = new DataSet();
                    dss = objCore.funGetDataSet("select * from MenuItem where id='" + comboBox2.SelectedValue + "'");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        comboBox1.SelectedValue = dss.Tables[0].Rows[0]["MenuGroupId"].ToString();
                    }
                    button1.Text = "Update";

                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private void PointsCodes_Load(object sender, EventArgs e)
        {
            fillgroup();
            fillmenuitem();
            getdata();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void editSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            if (id.Length > 0)
            {
                edit(id);
                pointsid = id;
            }
        }

        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string status = "";
                string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                string q = "select * from points where id='" + id + "'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    status = ds.Tables[0].Rows[0]["Status"].ToString();
                    MessageBox.Show("This Code is already used and can not be deleted");
                    return;
                }
                else
                {
                    q = "delete from points where id='" + id + "'";
                    int res = objCore.executeQueryint(q);
                    if (res > 0)
                    {
                        MessageBox.Show("deleted successfully");
                    }
                    getdata();
                }
            }
            catch (Exception ex)
            {
                
            }
               
        }
        string pointsid = "";
        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox2.Text == "" || comboBox2.Text == "Please Select")
            {
                MessageBox.Show("Please Select Menu Item");
                comboBox2.Focus();
                return;
            }
            if (txtcode.Text == "" )
            {
                MessageBox.Show("Please Enter Code");
                txtcode.Focus();
                return;
            }
            string flid = "";
            try
            {
                if (comboBox3.Visible == true)
                {
                    flid = comboBox3.SelectedValue.ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
            if (button1.Text == "Submit")
            {
                string q = "insert into points (MenuItemId, Code,Flavourid) values ('" + comboBox2.SelectedValue + "','" + txtcode.Text.Trim() + "','"+flid+"')";
                int res = objCore.executeQueryint(q);
                if (res > 0)
                {
                    MessageBox.Show("Added successfully");
                }
                else
                {
                    MessageBox.Show("Error Saving Data");

                }
            }
            if (button1.Text == "Update")
            {
                string q = "update points set MenuItemId='" + comboBox2.SelectedValue + "', Code='" + txtcode.Text.Trim() + "' where id='" + pointsid + "'";
                int res = objCore.executeQueryint(q);
                if (res > 0)
                {
                    MessageBox.Show("Added successfully");
                    button1.Text = "Submit";
                    txtcode.Text = "";
                }
                else
                {
                    MessageBox.Show("Error Saving Data");

                }

            }
            getdata();
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillflavour();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillmenuitem();
        }
    }
}
