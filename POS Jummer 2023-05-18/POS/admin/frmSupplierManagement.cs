using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POSRestaurant.classes;
using System.Data.SqlClient;
using System.IO;


namespace POSRestaurant.admin
{
    public partial class frmSupplierManagement : Form
    {
        public string SearchItem,RentalIDs;

        public frmSupplierManagement()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.activated = false;
        }

        private void frmAllCustomer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Dispose();
            }
        }

        private void frmAllCustomer_Load(object sender, EventArgs e)
        {
            //this.SearchQRY();
            
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            POSRestaurant.admin.frmSupplierAdd CustAdd = new frmSupplierAdd();
            CustAdd.MdiParent = this.MdiParent;
            CustAdd.Show();
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            this.SearchQRY();
        }


       private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
       {
           if (this.lvList.SelectedItems.Count >= 1)
           {
               this.activated = false;
               DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

               if (msg == DialogResult.Yes)
               {
                   try
                   {
                       this.objCore.executeQuery("DELETE FROM SUPPLIER where Id = " + this.lvList.FocusedItem.Text.Trim());
                       this.activated = true;
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
               MessageBox.Show("Please select a supplier to delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           }
           this.activated = true;
       }

       private void btn_Edit_Click(object sender, EventArgs e)
       {
           if (this.lvList.SelectedItems.Count >= 1)
           {
               int id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
               frmSupplierAdd obj = new frmSupplierAdd();
               obj.supplierId = id;
               obj.editMode = 1;
               obj.ShowDialog();
           }
           else
           {
               MessageBox.Show("Please select a supplier to edit.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           }
       }

       private void btn_close_Click(object sender, EventArgs e)
       {
           this.Dispose();
       }

       private void lvList_SelectedIndexChanged(object sender, EventArgs e)
       {         
           SearchItem = lvList.FocusedItem.Text;
       }

       private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
       {

       }

       private void btnCancel_Click(object sender, EventArgs e)
       {
           this.Close();
       }

       void SearchQRY()
       {
           try
           {
               SqlDataReader dr;
               lvList.Items.Clear();
               if (this.cboField.Text.Trim() == string.Empty)
               {
                   dr = this.objCore.funGetDataReader("SELECT * FROM SUPPLIER Order By Name");
                  if (dr.HasRows)
                  {
                      while (dr.Read())
                      {
                          ListViewItem list = new ListViewItem(dr["Id"].ToString());
                          list.SubItems.Add(dr["Name"].ToString());
                          list.SubItems.Add(dr["Phone"].ToString());
                          list.SubItems.Add(dr["Category"].ToString());
                          list.SubItems.Add(dr["Address"].ToString());
                         
                          lvList.Items.AddRange(new ListViewItem[] { list });
                          list.ImageIndex = 1;
                      }
                  }
                  dr.Close();
                  this.objCore.closeConnection();
               }
               else if (this.cboField.Text.Trim() == "Name")
               {
                   dr = this.objCore.funGetDataReader("SELECT * FROM SUPPLIER WHERE Name Like '%" + this.txtSearch.Text.Trim() + "%' Order By Title");
                   if (dr.HasRows)
                   {
                       while (dr.Read())
                       {
                           ListViewItem list = new ListViewItem(dr["Id"].ToString());
                           list.SubItems.Add(dr["Name"].ToString());
                           list.SubItems.Add(dr["Phone"].ToString());
                           list.SubItems.Add(dr["Category"].ToString());
                           list.SubItems.Add(dr["Address"].ToString());
                           lvList.Items.AddRange(new ListViewItem[] { list });
                           list.ImageIndex = 1;
                       }
                   }
                   dr.Close();
                   this.objCore.closeConnection();
               }
               else if (this.cboField.Text.Trim() == "Contact Person")
               {
                   dr = this.objCore.funGetDataReader("SELECT * FROM SUPPLIER WHERE Phone Like '%" + this.txtSearch.Text.Trim() + "%' Order By Title");
                   if (dr.HasRows)
                   {
                       while (dr.Read())
                       {
                           ListViewItem list = new ListViewItem(dr["Id"].ToString());
                           list.SubItems.Add(dr["Name"].ToString());
                           list.SubItems.Add(dr["Phone"].ToString());
                           list.SubItems.Add(dr["Category"].ToString());
                           list.SubItems.Add(dr["Address"].ToString());
                           lvList.Items.AddRange(new ListViewItem[] { list });
                           list.ImageIndex = 1;
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

       private void cmdSearch_Click(object sender, EventArgs e)
       {
           this.activated = true;
           this.SearchQRY();
       }

       private void frmCustomerManagement_Activated(object sender, EventArgs e)
       {
           if (this.activated)
               this.SearchQRY();
       }   

    }
}
