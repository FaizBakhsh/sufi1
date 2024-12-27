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
    public partial class AddSupplier : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddSupplier(POSRestaurant.forms.MainForm frm)
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
        protected void fillaccount()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from ChartofAccounts where name like '%payable%'";
                ds = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
            }
            catch (Exception ex)
            {

            }
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from Branch ";
            //ds = objCore.funGetDataSet(q);
            //comboBox2.DataSource = ds.Tables[0];
            //comboBox2.ValueMember = "id";
            //comboBox2.DisplayMember = "BranchName";
            fillaccount();
            
            if (editmode == 1)
            {
                
                ds = new DataSet();
                q = "select * from Supplier where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    // txtfname.Text = ds.Tables[0].Rows[0]["FatherName"].ToString();
                    txtcode.Text = ds.Tables[0].Rows[0]["Code"].ToString();

                    txtcity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                    txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtmobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                    txtarea.Text = ds.Tables[0].Rows[0]["Area"].ToString();
                    vButton2.Text = "Update";
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["payableaccountid"].ToString();

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
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Supplier");
                    return;
                }
                
               
                if (txtcode.Text.Trim() == string.Empty)
                {
                   // MessageBox.Show("Please Enter Code of Supplier");
                    //return;
                }
                if (txtcity.Text.Trim() == string.Empty)
                {
                   // MessageBox.Show("Please Enter City Name of Supplier");
                   // return;
                }
                
                if (txtmobile.Text.Trim() == string.Empty)
                {
                   // MessageBox.Show("Please Enter Mobile No of Supplier");
                   // return;
                }
                if (txtaddress.Text.Trim() == string.Empty)
                {
                   // MessageBox.Show("Please Enter Address of Supplier");
                   // return;
                }
                string account = "";// comboBox1.SelectedValue.ToString();
                try
                {
                    account = comboBox1.SelectedValue.ToString();
                }
                catch (Exception ex)
                {
                    
                    
                }
                TextBox txt = txtcreditlimit as TextBox;
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
                if (account == "")
                {
                    account = "0";
                }
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Supplier");
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
                    string q = "select * from Supplier where Name='" + txtName.Text.Trim().Replace("'", "''") + "' ";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Supplier Record already exist");
                        return;
                    }

                    q = "insert into Supplier (CreditLimit,payableaccountid,id,Name,Code,CNICNo,City,Address,Phone,Mobile,Area,Date) values('" + txtcreditlimit.Text + "','" + account + "','" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtcode.Text.Trim().Replace("'", "''") + "','','','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtmobile.Text.Trim().Replace("'", "''") + "','" + txtarea.Text.Trim().Replace("'", "''") + "','" + DateTime.Now + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Supplier set CreditLimit='" + txtcreditlimit.Text.Trim().Replace("'", "''") + "',payableaccountid='" + account.Trim().Replace("'", "''") + "',Area='" + txtarea.Text.Trim().Replace("'", "''") + "', Mobile='" + txtmobile.Text.Trim().Replace("'", "''") + "', Phone='" + txtphone.Text.Trim().Replace("'", "''") + "', Address='" + txtaddress.Text.Trim().Replace("'", "''") + "', City='" + txtcity.Text.Trim().Replace("'", "''") + "', Name='" + txtName.Text.Trim().Replace("'", "''") + "' , Code ='" + txtcode.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Supplier");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcode.Text = string.Empty;
            txtName.Text = string.Empty;
           
            
            txtcity.Text = string.Empty;
            txtmobile.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtarea.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            fillaccount();
        }

        private void txtcreditlimit_TextChanged(object sender, EventArgs e)
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
    }
}
