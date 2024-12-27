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
    public partial class manageitems : Form
    {
        POSRetail.classes.Clsdbcon objCore;
        public manageitems()
        {
            InitializeComponent();
            objCore=new classes.Clsdbcon();
        }
        void SearchQRY()
        {
            try
            {
                SqlDataReader dr;
                lvList.Items.Clear();

                dr = this.objCore.funGetDataReader("SELECT * FROM ITEM WHERE Name LIKE '%" + this.txtSearch.Text.Trim() + "%' Order By Name");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ListViewItem list = new ListViewItem(dr["Id"].ToString());
                        list.SubItems.Add(dr["Name"].ToString());
                        list.SubItems.Add(dr["Unitprice"].ToString());
                        list.SubItems.Add(dr["Quantity"].ToString());
                        list.SubItems.Add(dr["Measuringunit"].ToString());
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
        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            SearchQRY();
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            frmadditems obj = new frmadditems();
            obj.Show();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvList.SelectedItems.Count >= 1)
                {
                    int id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                    frmadditems ob = new frmadditems ();
                    ob.itemid = id;
                    ob.editMode = 1;
                    ob.ShowDialog();
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

        private void btn_Refresh_Click_1(object sender, EventArgs e)
        {
            SearchQRY();
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
                            this.objCore.executeQuery("DELETE FROM ITEM where Id = " + this.lvList.FocusedItem.Text.Trim());

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
                    MessageBox.Show("Please select an Item to delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            SearchQRY();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
