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
    public partial class frmManagestaff : Form
    {
        public POSRetail.classes.Clsdbcon objCore;
        public frmManagestaff()
        {
            InitializeComponent();
            objCore = new classes.Clsdbcon();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            SearchQRY();
        }
        void SearchQRY()
        {
            try
            {
                SqlDataReader dr;
                lvList.Items.Clear();

                dr = this.objCore.funGetDataReader("SELECT * FROM STAFF WHERE Name LIKE '%" + this.txtSearch.Text.Trim() + "%' OR Phone LIKE '%" + this.txtSearch.Text.Trim() + "%' OR Department LIKE '%" + this.txtSearch.Text.Trim() + "%' Order By Name");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ListViewItem list = new ListViewItem(dr["Id"].ToString());
                        list.SubItems.Add(dr["Name"].ToString());
                        list.SubItems.Add(dr["Phone"].ToString());
                        list.SubItems.Add(dr["Designation"].ToString());
                        list.SubItems.Add(dr["Department"].ToString());
                        list.SubItems.Add(dr["Salary"].ToString());
                        list.SubItems.Add(dr["Address"].ToString());
                        lvList.Items.AddRange(new ListViewItem[] { list });
                        list.ImageIndex = 1;
                    }

                    dr.Close();
                    this.objCore.closeConnection();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            SearchQRY();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            frmAddstaff objstf = new frmAddstaff();
            objstf.Show();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvList.SelectedItems.Count >= 1)
                {
                    int id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                    frmAddstaff objsh = new frmAddstaff();
                    objsh.id = id;
                    objsh.editmode = 1;
                    objsh.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Please select a Dish to edit.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvList.SelectedItems.Count >= 1)
                {

                    DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (msg == DialogResult.Yes)
                    {
                        try
                        {
                            this.objCore.executeQuery("DELETE FROM STAFF where Id = " + this.lvList.FocusedItem.Text.Trim());

                            MessageBox.Show("Record Successfully Deleted.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {

                    }
                }
                else
                {
                    MessageBox.Show("Please select an account to delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
