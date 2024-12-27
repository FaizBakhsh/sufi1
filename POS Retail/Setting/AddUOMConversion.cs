using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Setting
{
    public partial class Addcuom : Form
    {
         POSRetail.forms.MainForm _frm;
         public Addcuom(POSRetail.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }
        
       

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
           
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "select * from uom";
            ds = objCore.funGetDataSet(q);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "uom";

            if (editmode == 1)
            {
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from UOMConversion where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Qty"].ToString();
                    txtdescription.Text = ds.Tables[0].Rows[0]["ConversionRate"].ToString();
                    txtcuom.Text = ds.Tables[0].Rows[0]["UOM"].ToString();
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["UOMId"].ToString();
                    vButton2.Text = "Update";
                }
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {

                if (comboBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select valid UOM");
                    return;
                }
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Quantity");
                    return;
                }
                if (txtdescription.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Converstion Rate");
                    return;
                }
                if (txtcuom.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Converted UOM");
                    return;
                }
                if (editmode == 0)
                {
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from UOMConversion");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        id = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        id = 1;
                    }
                    ds = new DataSet();
                    string q = "select * from UOMConversion where UOMId='" + comboBox1.SelectedValue + "' and uom='" + txtcuom.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Converted UOM already exist");
                        return;
                    }
                    q = "insert into UOMConversion (id,UOMId,Qty,ConversionRate,UOM) values('" + id + "','" + comboBox1.SelectedValue + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "','" + txtcuom.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update UOMConversion set UOMId='" + comboBox1.SelectedValue + "', Qty='" + txtName.Text.Trim().Replace("'", "''") + "' , ConversionRate ='" + txtdescription.Text.Trim().Replace("'", "''") + "', uom ='" + txtcuom.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT     dbo.UOMConversion.Id, dbo.UOM.UOM, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM AS Converted_UOM FROM         dbo.UOM INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtdescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtcuom.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
