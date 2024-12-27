using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class BankDepositsList : Form
    {
        public BankDepositsList()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void BankDepositsList_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string q = "select id,branchname from branch";
            ds = objCore.funGetDataSet(q);
            try
            {
                DataRow dr1 = ds.Tables[0].NewRow();
                dr1["branchname"] = "All";
                 ds.Tables[0].Rows.Add(dr1);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "branchname";
                
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
            getdata();
        }
    
        protected void getdata()
        {
          
            
            try
            {

                string q = "select Date, ActualAmount, DepositedAmount,Image,status  from BankDeposits where branchid='" + comboBox1.SelectedValue + "' order by date desc";
                q = "SELECT        TOP (100) PERCENT dbo.BankDeposits.Id, dbo.Branch.BranchName, dbo.BankDeposits.Date, dbo.BankDeposits.ActualAmount, dbo.BankDeposits.DepositedAmount, dbo.BankDeposits.Image, dbo.BankDeposits.Status FROM            dbo.BankDeposits INNER JOIN                         dbo.Branch ON dbo.BankDeposits.branchid = dbo.Branch.Id where dbo.BankDeposits.branchid='" + comboBox1.SelectedValue + "'  ORDER BY dbo.BankDeposits.Date DESC";
       
                if (comboBox1.Text == "All")
                {
                    q = "SELECT        TOP (100) PERCENT dbo.BankDeposits.Id, dbo.Branch.BranchName, dbo.BankDeposits.Date, dbo.BankDeposits.ActualAmount, dbo.BankDeposits.DepositedAmount, dbo.BankDeposits.Image, dbo.BankDeposits.Status FROM            dbo.BankDeposits INNER JOIN                         dbo.Branch ON dbo.BankDeposits.branchid = dbo.Branch.Id ORDER BY dbo.BankDeposits.Date DESC";
                }
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    textBox2.Text = ds.Tables[0].Rows[0][2].ToString();
                //    lblstatus.Text = ds.Tables[0].Rows[0]["status"].ToString();
                //    if (lblstatus.Text == "Posted")
                //    {
                //        lblstatus.ForeColor = Color.Green;
                //    }
                //    else
                //    {
                //        lblstatus.ForeColor = Color.Black;
                //    }
                //    imageData = new Byte[0];
                //    imageData = (Byte[])(ds.Tables[0].Rows[0]["Image"]);
                //    MemoryStream mem = new MemoryStream(imageData);
                //    pictureBox2.Image = Image.FromStream(mem);
                //}
                //else
                //{
                //    textBox2.Text = "";
                //    imageData = null;
                //    pictureBox2.Image = null;
                //}
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            BankDeposits obj = new BankDeposits();
            obj.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    double actual = 0, deposited = 0;

                    string temp = item.Cells["ActualAmount"].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    actual = Convert.ToDouble(temp);
                    temp = item.Cells["DepositedAmount"].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    deposited = Convert.ToDouble(temp);

                    if (deposited < actual)
                    {
                        item.DefaultCellStyle.BackColor = Color.Red;
                        item.DefaultCellStyle.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
            {
                Bankdepositslip obj = new Bankdepositslip();
                obj.id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                obj.ShowDialog();
            }
        }
    }
}
