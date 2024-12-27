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
    public partial class Addcolor : Form
    {
        POSRetail.forms.MainForm _frm;
        public Addcolor(POSRetail.forms.MainForm frm)
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
            txtdescription.Text = string.Empty;
            
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
           
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            if (editmode == 1)
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from color where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    richTextBox1.Text = ds.Tables[0].Rows[0]["Caption"].ToString();
                    txtdescription.Text = ds.Tables[0].Rows[0]["ColorName"].ToString();
                    vButton2.Text = "Update";
                }
            }
            richTextBox1.Focus();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Color Name");
                    return;
                }
                if (txtdescription.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Choose Color");
                    return;
                }
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
               
                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Color");
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
                    string q = "select * from color where ColorName='" + txtdescription.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Color Name already exist");
                        return;
                    }
                    q = "insert into Color (id,ColorName,Caption) values('" + id + "','" + txtdescription.Text.Trim().Replace("'", "''") + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update color set ColorName='" + txtdescription.Text.Trim().Replace("'", "''") + "', Caption='" + richTextBox1.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select Id,  Caption as Color_Name,ColorName as Color_Code,  UploadStatus from Color");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
               
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtdescription.Text = string.Empty;
            richTextBox1.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtdescription_Click(object sender, EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                txtdescription.Text = colorDialog1.Color.ToArgb().ToString(); ;
                richTextBox1.Text = colorDialog1.Color.Name.ToString();
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
