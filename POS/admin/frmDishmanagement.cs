using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.admin
{
    public partial class frmDishmanagement : Form
    {
        public frmDishmanagement()
        {
            InitializeComponent();
            this.objCore = new classes.Clsdbcon();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void SearchQRY()
        {
            try
            {
                SqlDataReader dr;
                lvList.Items.Clear();

                dr = this.objCore.funGetDataReader("SELECT * FROM DISHES WHERE Name LIKE '%" + this.txtSearch.Text.Trim() + "%' Order By Name");
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        ListViewItem list = new ListViewItem(dr["Id"].ToString());
                        list.SubItems.Add(dr["Name"].ToString());
                        list.SubItems.Add(dr["Price"].ToString());
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
            frmAddDishes dsh = new frmAddDishes();
            dsh.Show();
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.lvList.SelectedItems.Count >= 1)
                {
                    int id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                    frmAddDishes dsh = new frmAddDishes();
                    dsh.id = id;
                    dsh.editmode = 1;
                    dsh.ShowDialog();
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
                            this.objCore.executeQuery("DELETE FROM DISHES where Id = " + this.lvList.FocusedItem.Text.Trim());

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
                    MessageBox.Show("Please select a Dish to delete.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (this.lvList.SelectedItems.Count >= 1)
                {
                    SqlDataReader dr1;
                    int id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                    //dr1 = this.objCore.funGetDataReader("SELECT * FROM DISHES WHERE Id = '" + id + "' ");
                    //if (dr1.HasRows)
                    {
                        //while (dr1.Read())
                        {
                           //int did =Convert.ToInt32 (dr1["Rid"].ToString());
                            frmItemrecipemang objir = new frmItemrecipemang();
                            objir.rid = id;

                            objir.ShowDialog();
                           
                        }

                        //dr1.Close();
                        //this.objCore.closeConnection();
                    }
                   
                }
                else
                {
                    MessageBox.Show("Please select a Dish to Check Recipe.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void frmDishmanagement_Load(object sender, EventArgs e)
        {

        }
    }
}
