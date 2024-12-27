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
    public partial class frmadditems : Form
    {
        public POSRestaurant.classes.Clsdbcon objCore;
        public int editMode = 0;
        public int itemid = 0;
        public frmadditems()
        {
            InitializeComponent();
            objCore = new classes.Clsdbcon();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            try
            {
                if (this.txtFName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please enter item name.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFName.Focus();
                    return;
                }
                if (this.txtprice.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please enter price.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFName.Focus();
                    return;
                }
                if (this.txtquantity.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please enter quantity", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFName.Focus();
                    return;
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
                        int id = 0;
                        SqlDataReader sdr = objCore.funGetDataReader1("SELECT MAX(Id) AS MID FROM ITEM");
                        if (sdr.HasRows)
                        {
                            sdr.Read();
                            string idw = sdr[0].ToString();
                            if (idw == "")
                            {
                                id = 1;
                            }
                            else
                            {
                                id = Convert.ToInt32(sdr[0].ToString());
                                id = id + 1;
                            }
                        }
                        command = "Insert Into ITEM (Id,Supid,Name,Measuringunit,Unitprice,Quantity,Totalprice,Date,Recordtype) Values('" + id + "','" + cmb.SelectedValue + "','" + this.txtFName.Text.Trim() + "', '" + this.txtmunit.Text.Trim() + "', '" + this.txtprice.Text.Trim() + "', '" + this.txtquantity.Text.Trim() + "', '" + this.txttotalprice.Text.Trim() + "', '" + Convert.ToDateTime(DateTime.Now.ToShortDateString()) + "', 'LocalServer')";
                        com.CommandText = command;
                        com.ExecuteNonQuery();
                        tran.Commit();
                        MessageBox.Show("Item record saved sucessfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (this.editMode == 1)
                    {
                        command = "Update SUPPLIER Set Name = '" + this.txtFName.Text.Trim() + "', Supid = '" + this.cmb.SelectedValue + "', Measuringunit = '" + this.txtmunit.Text.Trim() + "', Unitprice = '" + this.txtprice.Text.Trim() + "', Quantity = '" + this.txtquantity.Text.Trim() + "', Totalprice = '" + this.txttotalprice.Text.Trim() + "', Date = '" + Convert.ToDateTime(DateTime.Now.ToShortDateString()) + "' Where Id = " + this.itemid;
                        com.CommandText = command;
                        com.ExecuteNonQuery();
                        tran.Commit();
                        con.Close();
                        MessageBox.Show("Item Record Updated sucessfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);


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

        private void frmadditems_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsCategory = this.objCore.funGetDataSet("Select Id, Name from SUPPLIER ");

                this.cmb.DataSource = dsCategory.Tables[0];
                this.cmb.DisplayMember = dsCategory.Tables[0].Columns[1].ToString();
                this.cmb.ValueMember = dsCategory.Tables[0].Columns[0].ToString();
            }
            catch  
            {
                
                
            }
            try
            {
                SqlDataReader dr = this.objCore.funGetDataReader("Select * from ITEM Where Id = '" + itemid + "' ");

                if (dr.HasRows)
                {
                    dr.Read();
                    txtFName.Text = dr["Name"].ToString();
                    txtmunit.Text = dr["Measuringunit"].ToString();
                    txtprice.Text = dr["Unitprice"].ToString();
                    txtquantity.Text = dr["Quantity"].ToString();
                   txttotalprice.Text = dr["Totalprice"].ToString();
                    // = dr["Date"].ToString();
                    
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void txtprice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txttotalprice.Text = (Convert.ToDouble(txtquantity.Text) * Convert.ToDouble(txtprice.Text)).ToString();
            }
            catch  
            {
                  
            }
        }

        private void txtquantity_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txttotalprice.Text = (Convert.ToDouble(txtquantity.Text) * Convert.ToDouble(txtprice.Text)).ToString();
            }
            catch  
            {
                
                
            }
     
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
