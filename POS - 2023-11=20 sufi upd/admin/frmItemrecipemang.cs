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
    public partial class frmItemrecipemang : Form
    {
        public int rid = 0;
        public frmItemrecipemang()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.activated = true;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmAddItemrecipe addItemcategory = new frmAddItemrecipe();
            addItemcategory.rid = rid;
            addItemcategory.ShowDialog();
        }

        private void frmRentalCategory_Load(object sender, EventArgs e)
        {
            try
            {
                fillListView();
            }
            catch (Exception ex)
            {
            }
        }

        public void fillListView()
        {
            try
            {
                lvList.Items.Clear();
                SqlDataReader dr = this.objCore.funGetDataReader("Select * from Vrecipe Where Rid = '" + rid + "'");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ListViewItem list = new ListViewItem(dr["Id"].ToString());
                        list.SubItems.Add(dr["Itemname"].ToString());
                        list.SubItems.Add(dr["Usedquantity"].ToString());
                        lvList.Items.AddRange(new ListViewItem[] { list });
                        int rCount = lvList.Items.Count;
                        if (rCount % 2 == 1)
                        {
                            list.BackColor = Color.WhiteSmoke;
                            list.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(234)))), ((int)(((byte)(189)))));
                        }
                        else
                        {
                            list.BackColor = Color.White;
                        }
                    }
                }
                dr.Close();
                this.objCore.closeConnection();
            }
            catch (Exception ex)
            {
            }
        }
       
        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            this.fillListView();
        }

        private void frmRentalCategory_FormClosed(object sender, FormClosedEventArgs e)
        {
      
        }

        public void DeleteQRY()
        {
            if (this.lvList.SelectedItems.Count >= 1)
            {
                this.activated = false;
                DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (msg == DialogResult.Yes)
                {
                    try
                    {
                        this.objCore.executeQuery("DELETE FROM INGREDIENTS where Id = '" + this.lvList.FocusedItem.Text.Trim() + "'");
                        this.activated = true;
                        MessageBox.Show("Record Successfully Deleted.", "  ", MessageBoxButtons.OK, MessageBoxIcon.Information);                        
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
            this.activated = true;
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            this.DeleteQRY();
        }
        string itemid;
       

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmItemCategory_Activated(object sender, EventArgs e)
        {
            if (this.activated)
                this.fillListView();
        }

        private void pbxEdit_Click(object sender, EventArgs e)
        {
            if (this.lvList.SelectedItems.Count >= 1)
            {
                int Id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                frmAddItemrecipe obj = new frmAddItemrecipe();
                obj.Id = Id;
                obj.rid = rid;
                obj.editMode = 1;
                obj.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a category to edit.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
