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
    public partial class frmItemSubCategory : Form
    {
        public frmItemSubCategory()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.activated = true;
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            frmAddItemSubCategory addItemcategory = new frmAddItemSubCategory();
            addItemcategory.ShowDialog();
        }

        private void frmRentalCategory_Load(object sender, EventArgs e)
        {
            try
            {
                //SMSLibX.SMSModem smd = new SMSLibX.SMSModem();
                //smd.OpenComm(SMSLibX.GSMModemTypeConstants.gsmModemNokia, 3, null, SMSLibX.SMSModemNotificationConstants.smsNotifyAll, 100, false);
                //SMSLibX.SMSSubmit sub = new SMSLibX.SMSSubmit();
                //sub.Alphabet = SMSLibX.GSMAlphabetConstants.gsmAlphabetUnicode;
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
                SqlDataReader dr = this.objCore.funGetDataReader("Select * from ItemSubCategory Where ByDefault = '0'");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ListViewItem list = new ListViewItem(dr["SubCategoryId"].ToString());
                        list.SubItems.Add(dr["SubCategoryName"].ToString());
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
                DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "Muslim Book Land", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (msg == DialogResult.Yes)
                {
                    try
                    {
                        this.objCore.executeQuery("DELETE FROM ItemSubCategory where SubCategoryId = '" + this.lvList.FocusedItem.Text.Trim() + "'");
                        this.activated = true;
                        MessageBox.Show("Record Successfully Deleted.", "Muslim Book Land", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                MessageBox.Show("Please select a sub category to delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                int subCategoryId = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                frmAddItemSubCategory obj = new frmAddItemSubCategory();
                obj.subCategoryId = subCategoryId;
                obj.editMode = 1;
                obj.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a sub category to edit.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
