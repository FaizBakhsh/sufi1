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
    public partial class AddSupplier : Form
    {
        POSRetail.forms.MainForm _frm;
        public AddSupplier(POSRetail.forms.MainForm frm)
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

        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            try
            {
                
                string q = "select * from ChartofAccounts where AccountType='Current Liabilities'";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";
                comboBox1.Text = "Please Select";
            }
            catch (Exception ex)
            {
                
               
            }

            if (editmode == 1)
            {
               // POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                try
                {
                    ds = new DataSet();
                    string q = "select * from Supplier where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                        // txtfname.Text = ds.Tables[0].Rows[0]["FatherName"].ToString();
                        txtcode.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                        txtcnic.Text = ds.Tables[0].Rows[0]["CNICNo"].ToString();
                        txtcity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                        txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                        txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                        txtmobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                        txtarea.Text = ds.Tables[0].Rows[0]["Area"].ToString();
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["payableaccountid"].ToString();
                        vButton2.Text = "Update";
                    }
                }
                catch (Exception ex)
                {
                    
                   
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
                    MessageBox.Show("Please Enter Code of Supplier");
                    return;
                }
                if (txtcity.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter City Name of Supplier");
                    return;
                }
                if (txtcnic.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter CNIC No of Supplier");
                    return;
                }
                if (txtmobile.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Mobile No of Supplier");
                    return;
                }
                if (txtaddress.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Address of Supplier");
                    return;
                }
                if (comboBox1.Text.Trim() == string.Empty || comboBox1.Text.Trim() =="Please Select")
                {
                    MessageBox.Show("Please Select Payable Account");
                    return;
                }
                if (editmode == 0)
                {

                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                    string q = "select * from Supplier where Name='" + txtName.Text.Trim().Replace("'", "''") + "' and CNICNo='" + txtcnic.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Supplier Record already exist");
                        return;
                    }

                    q = "insert into Supplier (id,Name,Code,CNICNo,City,Address,Phone,Mobile,Area,Date,payableaccountid) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtcode.Text.Trim().Replace("'", "''") + "','" + txtcnic.Text.Trim().Replace("'", "''") + "','" + txtcity.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtmobile.Text.Trim().Replace("'", "''") + "','" + txtarea.Text.Trim().Replace("'", "''") + "','" + DateTime.Now + "','"+comboBox1.SelectedValue+"')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Supplier set payableaccountid='"+comboBox1.SelectedValue+"', Area='" + txtarea.Text.Trim().Replace("'", "''") + "', Mobile='" + txtmobile.Text.Trim().Replace("'", "''") + "', Phone='" + txtphone.Text.Trim().Replace("'", "''") + "', Address='" + txtaddress.Text.Trim().Replace("'", "''") + "', City='" + txtcity.Text.Trim().Replace("'", "''") + "', Name='" + txtName.Text.Trim().Replace("'", "''") + "' , Code ='" + txtcode.Text.Trim().Replace("'", "''") + "', CNICNo ='" + txtcnic.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT     dbo.Supplier.Id, dbo.Supplier.Name, dbo.Supplier.Code, dbo.Supplier.CNICNo, dbo.Supplier.City, dbo.Supplier.Address, dbo.Supplier.Phone, dbo.Supplier.Mobile,                       dbo.Supplier.Area, dbo.Supplier.Date, dbo.Supplier.payableaccountid, dbo.Supplier.uploadstatus, dbo.ChartofAccounts.Name AS PayableAccount FROM         dbo.Supplier LEFT OUTER JOIN                      dbo.ChartofAccounts ON dbo.Supplier.payableaccountid = dbo.ChartofAccounts.Id");
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
           
            txtcnic.Text = string.Empty;
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
    }
}
