using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Sale
{
    public partial class VoidBill : Form
    {
        private  Sale _frm1;
        public static string user = "";
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public VoidBill()
           {
                InitializeComponent();
                
                richTextBox4.ForeColor = Color.LightGray;
                richTextBox4.Text = "Seach Sale by Invoice No";
                this.richTextBox4.Leave += new System.EventHandler(this.textBox1_Leave);
                this.richTextBox4.Enter += new System.EventHandler(this.textBox1_Enter);
           }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (richTextBox4.Text.Length == 0)
            {
                richTextBox4.Text = "Seach Sale by Invoice No";
                richTextBox4.ForeColor = Color.LightGray;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (richTextBox4.Text == "Seach Sale by Invoice No")
            {
                richTextBox4.Text = "";
                richTextBox4.ForeColor = Color.Black;
            }
        }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";

        //}

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         public void changtext(Button btn , string text)
        {
            btn.Text = text;
            btn.Text = text.Replace("&", "&&");
        }
         public void getdata()
         {
             try
             {
                 //category
                 DataSet ds9 = new DataSet();
                 string q9 = "";
                 if (richTextBox4.Text == string.Empty || richTextBox4.Text == "Seach Sale by Invoice No")
                 {
                     
                 }
                 else
                 {
                     filg2(richTextBox4.Text.Trim());
                 }
                 

             }
             catch (Exception ex)
             {


             }
         }
        private void AddGroups_Load(object sender, EventArgs e)
        {

            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "SELECT   * from sale where userid='" + id + "' order by id desc ";
                //ds = objCore.funGetDataSet(q);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    dataGridView1.DataSource = ds.Tables[0];
                //    dataGridView1.Columns[0].Visible = false;
                //    dataGridView1.Columns[3].Visible = false;
                //    foreach (DataGridViewRow dr in dataGridView1.Rows)
                //    {
                //        dr.Height = 40;
                //    }
                //}
                ds = new DataSet();
                q = "SELECT   * from users where id='" + id + "' order by id desc ";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user = ds.Tables[0].Rows[0]["name"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        public void filg2(string idd)
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "SELECT   * from sale where userid='" + id + "' and id='"+idd+"' order by id desc ";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 40;
                }
            }
            
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               // string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
               // filg2(Id);
            }
            catch (Exception ex)
            {
                
               
            }
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox4.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Invoice No. in Search Box ");
                    return;
                }
                DialogResult msg = MessageBox.Show("Are you sure you want to void this bill?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (msg == DialogResult.Yes)
                {
                    DataSet ds = new DataSet();
                    string qq = "SELECT   * from sale where userid='" + id + "' and id='" + richTextBox4.Text.Trim() + "' order by id desc ";
                    ds = objCore.funGetDataSet(qq);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid Invoice No");
                        return;
                    }
                    string sid = richTextBox4.Text.Trim();
                    int idd = 0;
                    ds = new DataSet();
                    ds = objCore.funGetDataSet("select max(id) as id from VoidBills");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        idd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        idd = 1;
                    }
                    string q = "select * from VoidBills where Saleid='" + sid + "' ";
                    DataSet dsdet = new DataSet();
                    dsdet = objCore.funGetDataSet(q);
                    if (dsdet.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("This Bill is Already in Void List");
                        return;
                    }
                    q = "insert into VoidBills (id,Saleid,status) values('" + idd + "','" + sid + "','Yes')";
                    objCore.executeQuery(q);

                    MessageBox.Show("Bill Added to Void List Successfully");

                }

            }
            catch (Exception ex)
            {


            }
        }
       
    }
}
