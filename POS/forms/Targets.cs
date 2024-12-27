using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class Targets : Form
    {
        public Targets()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

        protected void getdata()
        {
            string q = "";
            if (textBox2.Text == "")
            {
                q = "SELECT        Id, Date, Saletarget, CostPerc, WastePerc, Branchid FROM            SalesTargerts where branchid= " + comboBox1.SelectedValue;
            }
            else
            {
                q = "SELECT        Id, Date, Saletarget, CostPerc, WastePerc, Branchid FROM            SalesTargerts where date like '%" + textBox2.Text + "%' or  Saletarget like '%" + textBox2.Text + "%' or  CostPerc like '%" + textBox2.Text + "%' or  WastePerc like '%" + textBox2.Text + "%' or  branchid= " + comboBox1.SelectedValue;
            }
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
        }
        protected void getinfo(string id)
        {
            string q = "";
            try
            {
                q = "SELECT        Id, Date, Saletarget, CostPerc, WastePerc, Branchid FROM            SalesTargerts where id='" + id + "'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dateTimePicker1.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["date"]).ToString("yyyy-MM-dd");
                    txtcost.Text = ds.Tables[0].Rows[0]["CostPerc"].ToString();
                    txtsale.Text = ds.Tables[0].Rows[0]["Saletarget"].ToString();
                    txtwast.Text = ds.Tables[0].Rows[0]["WastePerc"].ToString();
                    comboBox1.SelectedValue = ds.Tables[0].Rows[0]["Branchid"].ToString();
                    button1.Text = "Update";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void Targets_Load(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string q = "select * from branch";
            ds = objcore.funGetDataSet(q);
           
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "branchName";
           
            getdata();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {

                float Num;
                bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid value. Only Nymbers are allowed");
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please select branch");
                comboBox1.Focus();
                return;
            }
            if (txtsale.Text == "")
            {
                MessageBox.Show("Please Enter Sale Target");
                txtsale.Focus();
                return;
            }
            if (txtcost.Text == "")
            {
                MessageBox.Show("Please Enter Cost Target");
                txtcost.Focus();
                return;
            }
            if (txtwast.Text == "")
            {
                MessageBox.Show("Please Enter Waste Target");
                txtwast.Focus();
                return;
            }
            TextBox txt = txtsale as TextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {

                float Num;
                bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid value. Only Nymbers are allowed");
                    return;
                }
            }
            txt = txtcost as TextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {

                float Num;
                bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid value. Only Nymbers are allowed");
                    return;
                }
            }
            txt = txtwast as TextBox;
            if (txt.Text == string.Empty)
            { }
            else
            {

                float Num;
                bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid value. Only Nymbers are allowed");
                    return;
                }
            }

            string q = "";
            if (button1.Text == "Submit")
            {
                q = "select * from SalesTargerts where branchid='" + comboBox1.SelectedValue + "' and date='" + dateTimePicker1.Text + "'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    MessageBox.Show("Data already exist");
                    return;
                }
                q = "insert into SalesTargerts ( Date, Saletarget, CostPerc, WastePerc, Branchid) values ('" + dateTimePicker1.Text + "','" + txtsale.Text + "','" + txtcost.Text + "','" + txtwast.Text + "','" + comboBox1.SelectedValue + "')";
                objcore.executeQuery(q);
            }
            if (button1.Text == "Update")
            {
                q = "update SalesTargerts set   Date='" + dateTimePicker1.Text + "', Saletarget='" + txtsale.Text + "', CostPerc='" + txtcost.Text + "', WastePerc='" + txtwast.Text + "', Branchid= '" + comboBox1.SelectedValue + "' where id='" + tid + "'";
                objcore.executeQuery(q);
                txtcost.Text = "";
                txtsale.Text = "";
                txtwast.Text = "";
                button1.Text = "Submit";
            }
            getdata();
        }

        private void editSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tid = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Id"].Value.ToString();
            getinfo(tid);           
        }
        public static string tid = "";
        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string  id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Id"].Value.ToString();

            DialogResult dr= MessageBox.Show("Are you Sure to Delete?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                string q = "delete from SalesTargerts where id='"+id+"'";
                objcore.executeQuery(q);
                getdata();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
