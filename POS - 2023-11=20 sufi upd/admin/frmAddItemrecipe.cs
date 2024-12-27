using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Text;
using System.Windows.Forms;
using POSRestaurant.classes;
using System.Data.SqlClient;


namespace POSRestaurant.admin
{
    public partial class frmAddItemrecipe : Form
    {
        public int rid = 0;
        public frmAddItemrecipe()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.Id = 0;
            this.editMode = 0;
        }

        private void frmAddRentalCategory_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsCategory = this.objCore.funGetDataSet("Select Id, Name from ITEM Order By Name");

                this.comboBox1.DataSource = dsCategory.Tables[0];
                this.comboBox1.DisplayMember = dsCategory.Tables[0].Columns[1].ToString();
                this.comboBox1.ValueMember = dsCategory.Tables[0].Columns[0].ToString();

                if (this.editMode == 1)
                {
                    SqlDataReader dr = this.objCore.funGetDataReader("Select * from INGREDIENTS Where Id = " + this.Id);
                    if (dr.HasRows)
                    {
                        dr.Read();
                        this.comboBox1.SelectedValue = dr.GetValue(2).ToString();
                        this.cmdSave.Text = "Update";
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
            this.Save();
        }   

        void Save()
        {
            try
            {
                if (this.editMode == 0)
                {
                     int id = 0;
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
                    this.objCore.executeQuery("Insert Into INGREDIENTS (Id,Rid,Itemid,Usedquantity) Values('"+id+"','"+rid+"','" + this.comboBox1.SelectedValue + "','"+textBox1.Text.Trim()+"')");
                    MessageBox.Show("Record Successfully Saved...");
                }
                else if (this.editMode == 1)
                {
                    this.objCore.executeQuery("Update INGREDIENTS Set Rid ='" + rid + "', Itemid='" + this.comboBox1.SelectedValue + "',Usedquantity='" + textBox1.Text.Trim() + "' Where id = " + this.Id);
                    MessageBox.Show("Record Successfully updated...");
                }
               this.Close();
            }
            catch (Exception e)
            { 
                MessageBox.Show(e.Message, "Error in Saving", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
        }
    }
}
