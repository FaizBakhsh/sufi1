using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class addprinttype : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public addprinttype(POSRestaurant.forms.MainForm frm)
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
        protected void getdata()
        {
            try
            {

                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "";


                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from printtype where printer='" + comboBox1.Text + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    comboBox2.Text = ds.Tables[0].Rows[0]["type"].ToString();
                    vButton2.Text = "Update";
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {

                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "";


                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from printtype where printer='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    comboBox1.Text = ds.Tables[0].Rows[0]["printer"].ToString();
                    comboBox2.Text = ds.Tables[0].Rows[0]["type"].ToString();
                    textBox1.Text = ds.Tables[0].Rows[0]["terminal"].ToString();
                    vButton2.Text = "Update";
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {

                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();



                ds = new DataSet();
                string q = "select * from printtype where printer='" + comboBox1.Text + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    q = "update printtype set type='" + comboBox2.Text + "',terminal='"+textBox1.Text+"' where printer='" + comboBox1.Text + "'";
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                else
                {
                    if (comboBox1.Text == "receipt")
                    {
                        q = "delete from  printtype  where printer is null";
                        objCore.executeQuery(q);
                    }
                    q = "delete from  printtype  where printer='" + comboBox1.Text + "'";
                    objCore.executeQuery(q);
                    q = "insert into printtype (printer,type,terminal) values('" + comboBox1.Text + "','" + comboBox2.Text + "','" + textBox1.Text + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    MessageBox.Show("Record saved successfully");
                }

                getdata();
                _frm.getdata("SELECT printer,type,terminal  FROM         printtype");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton2.Text = "Submit";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtrows_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtcols_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            getdata();
        }
    }
}
