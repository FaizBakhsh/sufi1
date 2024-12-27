using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace POSRetail.Setting
{
    public partial class AddPrinter : Form
    {
        POSRetail.forms.MainForm _frm;
        public AddPrinter( POSRetail.forms.MainForm frm1)
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
            cmbtype.Text = "Please Select";
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                if (cmbprinter.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Printer Name");
                    return;
                }
                if (cmbtype.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Printer Type");
                    return;
                }
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();

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
                    q = "insert into Printers (id,Name,type) values('" + id + "','" + cmbprinter.Text + "','" + cmbtype.Text + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                   
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update Printers set Name='" + cmbprinter.Text + "' , type ='" + cmbtype.Text + "' where id='" + id + "'";
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
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
