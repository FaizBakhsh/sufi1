using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace POSRestaurant.Setting
{
    public partial class AddDeals : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddDeals(POSRestaurant.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
                
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }
        public void fillhead()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds1 = new DataSet();
                string q = "select * from DealHeads";
                ds1 = objCore.funGetDataSet(q);

                DataRow dr = ds1.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds1.Tables[0].Rows.Add(dr);
                cmbhead.DataSource = ds1.Tables[0];
                cmbhead.ValueMember = "id";
                cmbhead.DisplayMember = "Name";
            }
            catch (Exception ex)
            {


            }

        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            fillhead();
            //String pkInstalledPrinters;
            //for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            //{
            //    if (i == 0)
            //    {
            //        cmbprinter.Items.Add("Please Select");
            //    }
            //    pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
            //    cmbprinter.Items.Add(pkInstalledPrinters);
            //}
            //cmbprinter.Text = "Please Select";
            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Deals where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    vTextBox1.Text = ds.Tables[0].Rows[0]["price"].ToString();
                    comboBox1.Text = ds.Tables[0].Rows[0]["status"].ToString();
                    cmbhead.SelectedValue = ds.Tables[0].Rows[0]["headid"].ToString();
                   
                    vButton2.Text = "Update";
                }
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbhead.Text == "Please Select")
                {
                    MessageBox.Show("Please select Head");
                    cmbhead.Focus();
                    return;
                }
                if (txtname.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Deal");
                    txtname.Focus();
                    return;
                }
                if (vTextBox1.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Price ");
                    vTextBox1.Focus();
                    return;
                }
                if (comboBox1.Text == string.Empty)
                {
                    MessageBox.Show("Please select status");
                    comboBox1.Focus();
                    return;
                }
                if (vTextBox1.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(vTextBox1.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid value. Only Nymbers are allowed");
                        return;
                    }
                }
                if (vTextBox2.Text == string.Empty)
                { }
                else
                {
                    int Num;
                    bool isNum = int.TryParse(vTextBox2.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid value. Only Nymbers are allowed");
                        return;
                    }
                }
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Deals");
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
                    string q = "select * from Deals where Name='" + txtname.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Deals Name already exist");
                        return;
                    }

                    q = "insert into Deals (id,Name,status,headid,price,totalitems) values('" + id + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + comboBox1.Text.Trim().Replace("'", "''") + "','" + cmbhead.SelectedValue + "','" + vTextBox1.Text + "','" + vTextBox2.Text + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Deals set totalitems='"+vTextBox2.Text+"', price='" + vTextBox1.Text + "', headid='" + cmbhead.SelectedValue + "', status='" + comboBox1.Text.Trim().Replace("'", "''") + "',name='" + txtname.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT dbo.Deals.id, dbo.DealHeads.name AS Head, dbo.Deals.Name,dbo.Deals.Price, dbo.Deals.Status FROM  dbo.DealHeads INNER JOIN               dbo.Deals ON dbo.DealHeads.id = dbo.Deals.headid");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            //cmbprinter.Text = "Please Select";
            txtname.Text = "";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void vTextBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void vTextBox1_TextChanged(object sender, EventArgs e)
        {

            if (vTextBox1.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(vTextBox1.Text.ToString(), out Num); //c is your variable
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

        private void vTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (vTextBox2.Text == string.Empty)
            { }
            else
            {
                int Num;
                bool isNum = int.TryParse(vTextBox2.Text.ToString(), out Num); //c is your variable
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
