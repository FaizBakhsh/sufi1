using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRetail.admin
{
    public partial class frmAddstaff : Form
    {
        public int id;
        public int editmode;
        public frmAddstaff()
        {
            InitializeComponent();
            objCore = new classes.Clsdbcon();
            id = 0;
            editmode = 0;
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            if (txtname.Text == "")
            {
                MessageBox.Show("Please enter name of staff");
                return;
            }
            if(textBox4.Text =="")
            {
                MessageBox.Show("Please enter Salary");
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please enter Phone no");
                return;
            }
            if (richTextBox1.Text == "")
            {
                MessageBox.Show("Please enter Address");
                return;
            }

            try
            {
                SqlDataReader dr2 = this.objCore.funGetDataReader("Select * from STAFF Where Name = '" + this.txtname.Text.Trim()+"' AND Phone='"+textBox2.Text.Trim()+"'");

                if (dr2.HasRows)
                {
                    MessageBox.Show("Record already exist");
                    return;
                }
                if (editmode==0)
                {
                    SqlDataReader sdr = objCore.funGetDataReader1("SELECT MAX(Id) AS MID FROM DISHES");
                    if (sdr.HasRows)
                    {
                        sdr.Read();
                        string idw = sdr[0].ToString();
                        if (idw == "")
                        {
                            id = 1;
                        }
                        else
                        {
                            id = Convert.ToInt32(sdr[0].ToString());
                            id = id + 1;
                        }
                    }
                    objCore.executeQuery("INSERT INTO STAFF (Id,Name,Phone,Designation,Department,Salary,Address,City) VALUES('"+id+"','" + txtname.Text.Trim() + "','" + textBox2.Text.Trim() + "','" + cmbdes.Text + "','" + cmbdep.Text + "','" + textBox4.Text.Trim() + "','" + richTextBox1.Text.Trim() + "','" + textBox1.Text.Trim() + "')");
                    MessageBox.Show("Record Saved Successfully"); 
                }
                if (editmode == 1)
                {
                    objCore.executeQuery("UPDATE  STAFF SET Name='" + txtname.Text.Trim() + "',Phone='" + textBox2.Text.Trim() + "',Designation='" + cmbdes.Text + "',Department='" + cmbdep.Text + "',Salary='" + textBox4.Text.Trim() + "',Address='" + richTextBox1.Text.Trim() + "',City='" + textBox1.Text.Trim() + "' WHERE Id='"+id+"'");
                    MessageBox.Show("Record Saved Successfully");
                }
            }
            catch(Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
        }

        private void frmAddstaff_Load(object sender, EventArgs e)
        {

            try
            {
                SqlDataReader dr = this.objCore.funGetDataReader("Select * from STAFF Where Id = '" + id + "' ");

                if (dr.HasRows)
                {
                    dr.Read();
                    txtname.Text = dr["Name"].ToString();
                    textBox2.Text = dr["Phone"].ToString();
                    cmbdes.Text = dr["Designation"].ToString();
                    cmbdep.Text = dr["Department"].ToString();
                    textBox4.Text = dr["Salary"].ToString();
                    richTextBox1.Text = dr["Address"].ToString();
                    textBox1.Text = dr["City"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            txtname.Text = "";
            richTextBox1.Text = "";
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
