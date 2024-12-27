using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Text;
using System.Windows.Forms;
using POSRetail.classes;
using System.Data.SqlClient;


namespace POSRetail.admin
{
    public partial class frmAddItemSubCategory : Form
    {
        public frmAddItemSubCategory()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.subCategoryId = 0;
            this.editMode = 0;
        }

        private void frmAddRentalCategory_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.editMode == 1)
                {
                    SqlDataReader dr = this.objCore.funGetDataReader("Select SubCategoryName from ItemSubCategory Where SubCategoryId = " + this.subCategoryId);
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
                    this.objCore.executeQuery("Insert Into ItemSubCategory (SubCategoryName) Values('" + this.txtCategoryName.Text.Trim() + "')");
                    MessageBox.Show("Record Successfully Saved...");
                }
                else if (this.editMode == 1)
                {
                    this.objCore.executeQuery("Update ItemSubCategory Set SubCategoryName = '" + this.txtCategoryName.Text.Trim() + "' Where SubCategoryId = " + this.subCategoryId);
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
