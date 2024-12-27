using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Text;
using System.Windows.Forms;
using POSRetail.classes;
using System.Data.SqlClient;

namespace POSRetail.ControlPanel
{
    public partial class frmAddEditUser : Form
    {
        public string flag,ItemNo;
        public int myID;

        public frmAddEditUser()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.userId = 0;
            this.editMode = 0;

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
                DataSet dsrights = this.objCore.funGetDataSet("Select FormId,FormName From Forms");
                DataGridViewComboBoxColumn dgCmdRights = (DataGridViewComboBoxColumn)this.dgRights.Columns["FormId"];
                dgCmdRights.DataSource = dsrights.Tables[0];
                dgCmdRights.DisplayMember = dsrights.Tables[0].Columns[1].ToString();
                dgCmdRights.ValueMember = dsrights.Tables[0].Columns[0].ToString();

                if (this.editMode == 1)
                {
                    SqlDataReader dr = this.objCore.funGetDataReader("Select * from useraccount Where Id = " + this.userId);
                    if (dr.HasRows)
                    {
                        dr.Read();
                        this.txtUserId.Text = dr["UserId"].ToString();
                        this.txtUserName.Text = dr["UserName"].ToString();
                        if (Convert.ToBoolean(dr["Active"]))
                            this.rboActive.Checked = true;
                        else
                            this.rboInactive.Checked = true;
                        dr.Close();
                        this.objCore.closeConnection();
                        this.dsRights1.Clear();
                        this.daRights.SelectCommand.Connection.ConnectionString = this.objCore.getConnectionString();
                        this.daRights.SelectCommand.Parameters["@userId"].Value = this.userId;
                        this.daRights.Fill(this.dsRights1);
                        this.dsRights1.UserRights.IdColumn.DefaultValue = this.userId;
                        if (this.userId == 1)
                            this.plRights.Enabled = false;
                        this.cmdSave.Text = "Update";
                    }
                    else
                    {
                        dr.Close();
                        this.objCore.closeConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void SaveUser()
        {
            try
            {
                if (this.editMode == 0)
                {
                    if (!this.objCore.getUserRight(8, "CanAdd"))
                    {
                        MessageBox.Show(POSRetail.classes.UserMessages.canAdd, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                else if (this.editMode == 1)
                {
                    if (!this.objCore.getUserRight(8, "CanUpdate"))
                    {
                        MessageBox.Show(POSRetail.classes.UserMessages.canUpdate, "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                if (this.txtUserId.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("User Id is required.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (this.txtUserName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("User Name  is required.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (this.txtPassword.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Password is required.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (this.txtPassword.Text.Trim()!= this.txtCPassword.Text.Trim())
                {
                    MessageBox.Show("Password Must Match.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                SqlDataReader dr;
                if (this.editMode == 0)
                {
                    dr = this.objCore.funGetDataReader("Select Id from useraccount Where UserId = '" + this.txtUserId.Text.Trim() + "'");
                    if (dr.HasRows)
                    {
                        dr.Close();
                        this.objCore.closeConnection();
                        MessageBox.Show("The User Id already exists.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtUserId.Focus();
                        return;
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                }
                else if (this.editMode == 1)
                {
                    dr = this.objCore.funGetDataReader("Select Id from useraccount Where UserId = '" + this.txtUserId.Text.Trim() + "' And Id <> " + this.userId);
                    if (dr.HasRows)
                    {
                        dr.Close();
                        this.objCore.closeConnection();
                        MessageBox.Show("The User Id already exists.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.txtUserId.Focus();
                        return;
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                    if (this.userId == 1)
                        this.rboActive.Checked = true;
                }
                SqlConnection con = new SqlConnection();
                con.ConnectionString = this.objCore.getConnectionString();
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                SqlCommand com = new SqlCommand("", con, tran);
                string command = string.Empty;
           
                try
                {
                    if (this.editMode == 0)
                    {
                        command = "Insert into useraccount( UserId, UserName, Password, Active,UserGroupId) Values( '" + this.txtUserId.Text.Trim() + "','" + this.txtUserName.Text.Trim() + "','" + this.txtPassword.Text.Trim() + "','" + this.rboActive.Checked.ToString() + "',1 )";
                        com.CommandText = command;
                        com.ExecuteNonQuery();
                        command = "Select Max(Id) from useraccount";
                        com.CommandText = command;
                        dr = com.ExecuteReader();
                        if (dr.HasRows)
                        {
                            dr.Read();
                            this.userId = Convert.ToInt32(dr.GetValue(0));
                        }
                        dr.Close();
                        for (int i = 0; i < this.dgRights.Rows.Count; i++)
                        {
                            this.dgRights["IdRights", i].Value = this.userId;
                        }
                        this.bindingSource1.EndEdit();
                        this.daRights.SelectCommand.Connection = con;
                        this.daRights.SelectCommand.Transaction = tran;
                        this.daRights.InsertCommand.Connection = con;
                        this.daRights.InsertCommand.Transaction = tran;
                        this.daRights.UpdateCommand.Connection = con;
                        this.daRights.UpdateCommand.Transaction = tran;
                        this.daRights.DeleteCommand.Connection = con;
                        this.daRights.DeleteCommand.Transaction = tran;
                        this.daRights.Update(this.dsRights1);
                        tran.Commit();
                        this.editMode = 1;
                        this.dsRights1.UserRights.IdColumn.DefaultValue = this.userId;
                        this.cmdSave.Text = "Update";
                        MessageBox.Show("User Added successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (this.editMode == 1)
                    {
                        command = "Update useraccount set  UserId = '" + this.txtUserId.Text.Trim() + "',UserName ='" + this.txtUserName.Text.Trim() + "' ,Password ='" + this.txtPassword.Text.Trim() + "' ,Active = '" + this.rboActive.Checked.ToString() + "' where Id = " + this.userId;
                        com.CommandText = command;
                        com.ExecuteNonQuery();
                        this.daRights.SelectCommand.Connection = con;
                        this.daRights.SelectCommand.Transaction = tran;
                        this.daRights.InsertCommand.Connection = con;
                        this.daRights.InsertCommand.Transaction = tran;
                        this.daRights.UpdateCommand.Connection = con;
                        this.daRights.UpdateCommand.Transaction = tran;
                        this.daRights.DeleteCommand.Connection = con;
                        this.daRights.DeleteCommand.Transaction = tran;
                        this.daRights.Update(this.dsRights1);
                        tran.Commit();
                        MessageBox.Show("User updated successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmdSave_Click(object sender, EventArgs e)
        {
            this.SaveUser();
        }

      
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }
    }
}