using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Text;
using System.Windows.Forms;
using POSRestaurant.classes;
using System.Data.SqlClient;

namespace POSRestaurant.ControlPanel
{
    public partial class frmManageUsers : Form
    {
        public string flag,ItemNo;
        public int myID;

        public frmManageUsers()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.UserId = "";
            this.editMode = 0;
            this.activated = false;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Title = "Select Picture";
            //openFileDialog1.Filter = "Image Files (JPEG,GIF,BMP)|*.jpg;*.jpeg;*.gif;*.bmp|JPEG Files(*.jpg;*.jpeg)|*.jpg;*.jpeg|GIF Files(*.gif)|*.gif|BMP Files(*.bmp)|*.bmp";
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    String myFileName = openFileDialog1.FileName;
            //    txtimgLink.Text = myFileName;
            //    picImage.Image = Image.FromFile(openFileDialog1.FileName);
            //}
            //openFileDialog1.Dispose();
        }

        private void frmAddMovie_Load(object sender, EventArgs e)
        {
            try
            {
                this.searchQRY(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdSave_Click(object sender, EventArgs e)
        {
          
        }

      
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            POSRestaurant.ControlPanel.frmAddEditUser User = new POSRestaurant.ControlPanel.frmAddEditUser();
            // saleitem.flag = "Save";
            User.ShowDialog(); 
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (this.lvList.SelectedItems.Count >= 1)
            {
                int itemId = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                frmAddEditUser obj = new frmAddEditUser();
                obj.userId = itemId;
                obj.editMode = 1;
                obj.MdiParent = this.MdiParent;
                obj.Show();
            }
            else
            {
                MessageBox.Show("Please select a user to edit.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (this.lvList.SelectedItems.Count >= 1)
            {
                try
                {
                    if (!this.objCore.getUserRight(8, "CanDelete"))
                    {
                        MessageBox.Show(POSRestaurant.classes.UserMessages.canDelete, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    this.activated = false;
                    DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "Muslim Book Land", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (msg == DialogResult.Yes)
                    {
                        int itemId = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                        SqlDataReader dr = this.objCore.funGetDataReader("Select ByDefault from useraccount Where Id = " + itemId);
                        if (dr.HasRows)
                        {
                            dr.Read();
                            if (Convert.ToBoolean(dr.GetValue(0)))
                            {
                                dr.Close();
                                this.objCore.closeConnection();
                                MessageBox.Show("Can not delete default user.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        }
                        dr.Close();
                        this.objCore.closeConnection();
                        this.objCore.executeQuery("DELETE FROM useraccount where Id = " + this.lvList.FocusedItem.Text.Trim());
                        this.activated = true;
                        MessageBox.Show("User Successfully Deleted.", "Muslim Book Land", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a user to delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            this.activated = true;
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            this.searchQRY();
        }

        private void toolStripLabel1_Click(object sender, EventArgs e)
        {

        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            this.activated = true;
            this.searchQRY();
        }

        void searchQRY()
        {
            try
            {
                this.lvList.Items.Clear();
                SqlDataReader dr;
                if (this.cboField.Text.Trim() == string.Empty)
                {
                    dr = this.objCore.funGetDataReader("SELECT * FROM vUsers Order by Id");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListViewItem list = new ListViewItem(dr["Id"].ToString());
                            list.SubItems.Add(dr["UserId"].ToString());
                            list.SubItems.Add(dr["UserName"].ToString());
                            list.SubItems.Add(dr["ActiveStatus"].ToString());
                            lvList.Items.AddRange(new ListViewItem[] { list });
                            list.ImageIndex = 0;
                        }
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                }
                else if (this.cboField.Text.Trim() == "User Name")
                {
                    dr = this.objCore.funGetDataReader("SELECT * FROM vUsers WHERE Lower(UserName) Like '%" + this.txtSearch.Text.Trim().ToLower() + "%'");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListViewItem list = new ListViewItem(dr["Id"].ToString());
                            list.SubItems.Add(dr["UserId"].ToString());
                            list.SubItems.Add(dr["UserName"].ToString());
                            list.SubItems.Add(dr["ActiveStatus"].ToString());
                            lvList.Items.AddRange(new ListViewItem[] { list });
                            list.ImageIndex = 0;
                        }
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                }
                else if (this.cboField.Text.Trim() == "User Id")
                {
                    dr = this.objCore.funGetDataReader("SELECT * FROM vUsers WHERE Lower(UserId) Like '%" + this.txtSearch.Text.Trim().ToLower() + "%'");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListViewItem list = new ListViewItem(dr["Id"].ToString());
                            list.SubItems.Add(dr["UserId"].ToString());
                            list.SubItems.Add(dr["UserName"].ToString());
                            list.SubItems.Add(dr["ActiveStatus"].ToString());
                            lvList.Items.AddRange(new ListViewItem[] { list });
                            list.ImageIndex = 0;
                        }
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                }              
            }
            catch (Exception ex)
            {
            }
        }

        private void frmManageUsers_Activated(object sender, EventArgs e)
        {
            if (this.activated)
            {
                this.searchQRY();
            }
        }
    }
}