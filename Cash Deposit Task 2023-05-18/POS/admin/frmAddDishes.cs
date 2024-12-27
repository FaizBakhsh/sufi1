using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.admin
{
    public partial class frmAddDishes : Form
    {
        public frmAddDishes()
        {
            InitializeComponent();
            this.objcore = new classes.Clsdbcon();
            this.editmode = 0;
            this.id = 0;
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            maskedTextBox1.Text = "0";
            textsname.Text = "";
        }
        public void save()
        
        {
            if (textsname.Text == "")
            {
                MessageBox.Show("Please Enter Name of Dish");
                return;
            }
            if (maskedTextBox1.Text == "")
            {
                MessageBox.Show("Please Enter Price of Dish");
                return;
            }
            if (this.editmode == 0)
            {
                int id = 0;
                SqlDataReader sdr = objcore.funGetDataReader1("SELECT MAX(Id) AS MID FROM DISHES");
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
                this.objcore.executeQuery("INSERT INTO DISHES (Id,Name,Price) VALUES('"+id+"','"+textsname.Text.Trim()+"','"+maskedTextBox1.Text.Trim()+"')");
                MessageBox.Show("Record Saved Successfully");
            }
            if (this.editmode == 1)
            {
                this.objcore.executeQuery("UPDATE DISHES SET Name='" + textsname.Text.Trim() + "',Price='" + maskedTextBox1.Text.Trim() + "' WHERE Id='"+this.id+"'");

                MessageBox.Show("Record Updated Successfully");
            }
        }
        private void cmdSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (this.editmode == 0)
                {
                    if (!this.objcore.getUserRight(2, "CanAdd"))
                    {
                        MessageBox.Show(POSRestaurant.classes.UserMessages.canAdd, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else if (this.editmode== 1)
                {
                    if (!this.objcore.getUserRight(2, "CanUpdate"))
                    {
                        MessageBox.Show(POSRestaurant.classes.UserMessages.canUpdate, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                this.save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmAddDishes_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.editmode == 1)
                {
                    SqlDataReader dr = this.objcore.funGetDataReader("Select * from DISHES Where Id = " + this.id);
                    if (dr.HasRows)
                    {
                        dr.Read();

                        this.textsname.Text = dr["Name"].ToString();
                        this.maskedTextBox1.Text = dr["Price"].ToString();
                        this.cmdSave.Text = "Update";
                        
                    }
                    dr.Close();
                    this.objcore.closeConnection();
                }
            }
            catch
            {


            }
        }
    }
}
