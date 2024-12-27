using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class Addstaff : Form
    {
        Receiveables _frm;
        public Addstaff(Receiveables frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        private TextBox focusedTextbox = null;
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;


                textBox1.Text = textBox1.Text + t.Text.Replace("&&", "&");


            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                int index = textBox1.SelectionStart;
                textBox1.Text = textBox1.Text.Remove(textBox1.SelectionStart - 1, 1);
                textBox1.Select(index - 1, 1);
                textBox1.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                int id = 0;
                ds = objCore.funGetDataSet("select max(id) as id from ResturantStaff");
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
                string q = "select * from ResturantStaff where name='" + textBox1.Text.Trim() + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Name already exist");
                    return;
                }
                ds = new DataSet();

                q = "insert into ResturantStaff (id,Name) values('" + id + "','" + textBox1.Text.Trim().Replace("'", "''") + "')";
              int res= objCore.executeQuery(q);
              if (res == 255)
              {
                  MessageBox.Show("You are not Allowed for this Operation");
              }
              else if (res == 0)
              {
                  MessageBox.Show("Failed to Save Data");
              }
              else
              {
                  MessageBox.Show("Name Saved Successfully");
              }
                _frm.fill();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _frm.fill();
            this.Close();
        }

        private void button42_Click(object sender, EventArgs e)
        {
            textBox1.Text = textBox1.Text + " ";
        }
    }
}
