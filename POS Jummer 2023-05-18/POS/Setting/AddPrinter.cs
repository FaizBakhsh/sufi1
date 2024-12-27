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
    public partial class AddPrinter : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddPrinter( POSRestaurant.forms.MainForm frm1)
        {
            _frm = frm1;
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

        private void btnexit_Click(object sender, EventArgs e)
        {
          
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();

            if (editmode == 1)
            {
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
               string q = "select * from Printers where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cmbprinter.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    cmbtype.Text = ds.Tables[0].Rows[0]["type"].ToString();
                    
                    vButton2.Text = "Update";
                }
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbtype.Text.Trim() == "Please Select" || cmbtype.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select Printer Type");
                    cmbtype.Focus();
                    return;
                }
                if (cmbtype.Text == "Generic")
                {
                    if (cmbprinter.Text.Trim() == "Please Select" || cmbprinter.Text.Trim() == "")
                    {
                        MessageBox.Show("Please Select Printer Name");
                        cmbprinter.Focus();
                        return;
                    }
                }

                if (cmbtype.Text == "Receipt")
                {
                    if (cmbprinter.Text.Trim() == "Please Select" || cmbprinter.Text.Trim() == "")
                    {
                        MessageBox.Show("Please Select Printer Name");
                        cmbprinter.Focus();
                        return;
                    }
                }
                if (cmbtype.Text == "OPOS")
                {
                    if (textBox1.Text.Trim() == "")
                    {
                        MessageBox.Show("Please enter printer name");
                        textBox1.Focus();
                        return;
                    }
                }
                if (comboBox1.Text.Trim() == "Please Select" || comboBox1.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select Number of Prints");
                    comboBox1.Focus();
                    return;
                }
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string name = "";
                if (cmbtype.Text == "Generic" || cmbtype.Text == "Receipt" || cmbtype.Text == "Main KOT")
                {
                    name = cmbprinter.Text;
                }
                else
                {
                    name = textBox1.Text;
                }
                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Printers");
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
                    string q = "select * from Printers where  type='" + cmbtype.Text + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                         MessageBox.Show("Printer for "+cmbtype.Text+" already exist");
                            return;
                       
                    }
                    q = "insert into Printers (id,Name,type,prints) values('" + id + "','" + name + "','" + cmbtype.Text + "','" + comboBox1.Text + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                   
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Printers set prints='" + comboBox1.Text + "', Name='" + name + "' , type ='" + cmbtype.Text + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from Printers");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            cmbtype.Text = "Please Select";
            cmbprinter.Text = "Please Select";
            comboBox1.Text = "Please Select";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbtype.Text == "Generic" || cmbtype.Text == "Receipt" || cmbtype.Text == "Main KOT")
            {
                cmbprinter.Visible = true;
                textBox1.Visible = false;
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
                
            }
            else
            {
                cmbprinter.Visible = false;
                textBox1.Visible = true;
            }
        }

        private void cmbprinter_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
