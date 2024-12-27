using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.Globalization;
using System.Data.SqlClient;
using POSRetail.classes;

namespace POSRetail.admin
{
    public partial class frmSupplierAdd : Form
    {
        SqlCommand cmd = new SqlCommand();
        int myID;

        public string mFormState;

        public frmSupplierAdd()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.supplierId = 0;
            this.editMode = 0;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            this.SaveQRY();
        }

        void SaveQRY()
        {
            try
            {
                if (this.txtFName.Text.Trim() == string.Empty )
                {
                    MessageBox.Show("Please enter supplier name.","", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFName.Focus();
                    return;
                }
                if (this.txtphone.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please enter supplier Phone No.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtFName.Focus();
                    return;
                }
                if (this.txtAddress.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please enter supplier Address.", "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        SqlDataReader sdr = objCore.funGetDataReader1("SELECT MAX(Id) AS MID FROM DISHES");
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
                        command = "Insert Into SUPPLIER (Id,Name,Gender,Nicno,Category,Email,Phone,City,Address) Values('"+id+"','" + this.txtFName.Text.Trim() + "','" + cmbgender.Text.Trim() + "', '" + this.txtnic.Text.Trim() + "', '" + this.txtcatgry.Text.Trim() + "', '" + this.txtmail.Text.Trim() + "', '" + this.txtphone.Text.Trim() + "', '" + this.textcity.Text.Trim() + "', '"+txtAddress.Text.Trim()+"')";
                        com.CommandText = command;
                        com.ExecuteNonQuery();
                        tran.Commit();
                        MessageBox.Show("Supplier saved sucessfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (this.editMode == 1)
                    {
                        command = "Update SUPPLIER Set Name = '" + this.txtFName.Text.Trim() + "', Gender = '" + this.cmbgender.Text.Trim() + "', Nicno = '" + this.txtnic.Text.Trim() + "', Category = '" + this.txtcatgry.Text.Trim() + "', Email = '" + this.txtmail.Text.Trim() + "', Phone = '" + this.txtphone.Text.Trim() + "', City = '" + this.textcity.Text.Trim() + "',Address='" + txtAddress.Text.Trim() + "' Where Id = " + this.supplierId;
                        com.CommandText = command;
                        com.ExecuteNonQuery();                       
                        tran.Commit();
                        con.Close();
                        MessageBox.Show("Supplier Record Updated sucessfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                       
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
     
        private void frmCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.editMode == 1)
                {
                    SqlDataReader dr;
                    dr = this.objCore.funGetDataReader("Select * from SUPPLIER Where Id = " + this.supplierId);
                    if (dr.HasRows)
                    {
                        dr.Read();
                        this.txtFName.Text = dr["Name"].ToString();
                        this.cmbgender.Text = dr["Gender"].ToString();
                        this.txtnic.Text = dr["Nicno"].ToString();
                        this.txtcatgry.Text = dr["Category"].ToString();
                        this.txtmail.Text = dr["Email"].ToString();
                        this.txtphone.Text = dr["Phone"].ToString();
                        this.txtAddress.Text = dr["Address"].ToString();
                        this.textcity.Text = dr["City"].ToString();
                        this.btnSave.Text = "Update";
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

