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
    public partial class Discountdetails : Form
    {
        private RestSale _frm1;
        private TextBox focusedTextbox = null;
        public string dis = "", name = "", userid = "";
        public Discountdetails(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

                if (focusedTextbox != null)
                {

                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
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
                int index = focusedTextbox.SelectionStart;
                focusedTextbox.Text = focusedTextbox.Text.Remove(focusedTextbox.SelectionStart - 1, 1);
                focusedTextbox.Select(index - 1, 1);
                focusedTextbox.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter name");
                textBox1.Focus();
                return;
            }
            if (textBox2.Text == "")
            {
                MessageBox.Show("Please Enter Phone no");
                textBox2.Focus();
                return;
            }
            if (textBox3.Text == "")
            {
                MessageBox.Show("Please Enter name");
                textBox3.Focus();
                return;
            }
            if (textBox4.Text == "")
            {
                MessageBox.Show("Please Enter Reference");
                textBox4.Focus();
                return;
            }
            try
            {
                string q = "select * from DiscountDetails where saleid='" + saleid + "'";
                DataSet dss = new DataSet();
                dss = Objcore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    q = "update DiscountDetails set name='" + textBox1.Text + "',phone='" + textBox2.Text + "',staff='" + textBox3.Text + "',Reference='" + textBox4.Text + "' where saleid='" + saleid + "'";
                    Objcore.executeQuery(q);
                }
                else
                {
                    int id = 1;
                    q = "select max(id) as id from DiscountDetails ";
                    dss = new DataSet();
                    dss = Objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string temp = dss.Tables[0].Rows[0][0].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        id = Convert.ToInt32(temp);
                        id = id + 1;
                    }
                    q = "insert into DiscountDetails (id,name,phone,staff,Reference) values ('" + id + "','" + textBox1.Text + "','" + textBox2.Text + "','" + textBox3.Text + "','" + textBox4.Text + "')";
                    Objcore.executeQuery(q);
                    _frm1.discountdetailsid = id.ToString();
                }
            }
            catch (Exception ex)
            {


            }
            _frm1.discountkeys(dis, name,"0","");
            _frm1.updateorder("discount");
            _frm1.disuser = userid;
            this.Close();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }
        POSRestaurant.classes.Clsdbcon Objcore = new classes.Clsdbcon();
        public string saleid = "";
        private void Discountdetails_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            try
            {
                string q = "select * from DiscountDetails where saleid='" + saleid + "'";
                DataSet dss = new DataSet();
                dss = Objcore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    textBox1.Text = dss.Tables[0].Rows[0]["name"].ToString();
                    textBox2.Text = dss.Tables[0].Rows[0]["phone"].ToString();
                    textBox3.Text = dss.Tables[0].Rows[0]["staff"].ToString();
                    textBox4.Text = dss.Tables[0].Rows[0]["Reference"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
    }
}
