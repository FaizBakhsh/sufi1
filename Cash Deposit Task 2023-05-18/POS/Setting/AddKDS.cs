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
    public partial class AddKDS : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddKDS(POSRestaurant.forms.MainForm frm)
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
            String pkInstalledPrinters;
            for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            {
                if (i == 0)
                {
                    cmbprinter.Items.Add("Please Select");
                }
                pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
                cmbprinter.Items.Add(pkInstalledPrinters);
            }
            cmbprinter.Text = "Please Select";
            if (editmode == 1)
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from kds where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    //cmbprinter.Text = ds.Tables[0].Rows[0]["Printer"].ToString();
                   
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
                if (txtname.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of KOT");
                    txtname.Focus();
                    return;
                }
                
                
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from kds");
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
                    string q = "select * from kds where Name='" + txtname.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("KDS Name already exist");
                        return;
                    }
                    string bell="Disabled";
                    if(checkBox1.Checked==true)
                    {
                        bell="Enabled";
                    }
                    q = "insert into kds (id,Name,Printer,KitchenBell) values('" + id + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + cmbprinter.Text + "','" + bell + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string bell = "Disabled";
                    if (checkBox1.Checked == true)
                    {
                        bell = "Enabled";
                    }
                    string q = "update kds set name='" + txtname.Text.Trim().Replace("'", "''") + "',Printer='" + cmbprinter.Text + "',KitchenBell='" + bell + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from kds");
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
    }
}
