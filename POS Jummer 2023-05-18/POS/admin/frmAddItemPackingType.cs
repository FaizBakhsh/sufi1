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
    public partial class frmAddItemPackingType : Form
    {
        public frmAddItemPackingType()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.packingTypeId = 0;
            this.editMode = 0;
        }

        private void frmAddRentalCategory_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.editMode == 1)
                {
                    SqlDataReader dr = this.objCore.funGetDataReader("Select PackTypeName from PackingType Where PackTypeId = " + this.packingTypeId);
                    if (dr.HasRows)
                    {
                        dr.Read();
                        this.txtCategoryName.Text = dr.GetValue(0).ToString();
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
            this.SaveRentalCategory();
        }   

        void SaveRentalCategory()
        {
            try
            {
                if (this.editMode == 0)
                {
                    this.objCore.executeQuery("Insert Into PackingType (PackTypeName) Values('" + this.txtCategoryName.Text.Trim() + "')");
                    MessageBox.Show("Record Successfully Saved...");
                }
                else if (this.editMode == 1)
                {
                    this.objCore.executeQuery("Update PackingType Set PackTypeName = '" + this.txtCategoryName.Text.Trim() + "' Where PackTypeId = " + this.packingTypeId);
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
