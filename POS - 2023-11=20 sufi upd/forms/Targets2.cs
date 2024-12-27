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
    public partial class Targets2 : Form
    {
        public Targets2()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

        protected void getdata()
        {
            try
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("Saletarget", typeof(string));
                dt.Columns.Add("CostPerc", typeof(string));
                dt.Columns.Add("WastePerc", typeof(string));


                int year = Convert.ToInt32(dtyear.Text);
                int month = Convert.ToInt32(dtmonth.Text);


                DateTime date = new DateTime(year, month, 1);
                string q = "";
                do
                {



                    q = "SELECT        Id, Date, Saletarget, CostPerc, WastePerc FROM            SalesTargerts where date='" + date + "' and branchid= " + comboBox1.SelectedValue;


                    DataSet ds1 = new DataSet();
                    ds1 = objcore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        q = "insert into SalesTargerts ( Date, Saletarget, CostPerc, WastePerc, Branchid)values('" + date + "','0','0','0','" + comboBox1.SelectedValue + "')";
                        int res = objcore.executeQueryint(q);
                    }
                    date = date.AddDays(1);
                }
                while (date.Month == month);





                q = "SELECT        Id, Date, Saletarget, CostPerc, WastePerc FROM            SalesTargerts where MONTH(date) = '" + dtmonth.Text + "' AND YEAR(date) = '" + dtyear.Text + "' and  branchid= " + comboBox1.SelectedValue + " order by date";

                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    dt.Rows.Add(
                //}
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[1].Visible = false;
                dataGridView1.Columns[2].Visible = false;
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
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
                    dtyear.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["date"]).ToString("yyyy-MM-dd");
                    
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
            
            

            //string q = "";
            //if (button1.Text == "Submit")
            //{
            //    q = "select * from SalesTargerts where branchid='" + comboBox1.SelectedValue + "' and date='" + dtyear.Text + "'";
            //    DataSet ds = new DataSet();
            //    ds = objcore.funGetDataSet(q);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        MessageBox.Show("Data already exist");
            //        return;
            //    }
            //    q = "insert into SalesTargerts ( Date, Saletarget, CostPerc, WastePerc, Branchid) values ('" + dtyear.Text + "','" + txtsale.Text + "','" + txtcost.Text + "','" + txtwast.Text + "','" + comboBox1.SelectedValue + "')";
            //    objcore.executeQuery(q);
            //}
            //if (button1.Text == "Update")
            //{
            //    q = "update SalesTargerts set   Date='" + dtyear.Text + "', Saletarget='" + txtsale.Text + "', CostPerc='" + txtcost.Text + "', WastePerc='" + txtwast.Text + "', Branchid= '" + comboBox1.SelectedValue + "' where id='" + tid + "'";
            //    objcore.executeQuery(q);
            //    txtcost.Text = "";
            //    txtsale.Text = "";
            //    txtwast.Text = "";
            //    button1.Text = "Submit";
            //}
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

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Cells[0].Value = Convert.ToDateTime(dr.Cells[2].Value.ToString()).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                if (id == "")
                {
                    string date = "", Saletarget = "", CostPerc = "", WastePerc = "";
                    try
                    {
                        date = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        Saletarget = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        CostPerc = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        WastePerc = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (Saletarget == "")
                    {
                        Saletarget = "0";
                    }
                    if (CostPerc == "")
                    {
                        CostPerc = "0";
                    }
                    if (WastePerc == "")
                    {
                        WastePerc = "0";
                    }
                    string q = "insert into SalesTargerts ( Date, Saletarget, CostPerc, WastePerc, Branchid)values('" + date + "','" + Saletarget + "','" + CostPerc + "','" + WastePerc + "','" + comboBox1.SelectedValue + "')";
                   int res= objcore.executeQueryint(q);
                   if (res == 1)
                   {
                       DataSet ds = new DataSet();
                       q = "select max(id) as id from SalesTargerts";
                       ds = objcore.funGetDataSet(q);
                       if (ds.Tables[0].Rows.Count > 0)
                       {
                           dataGridView1.Rows[e.RowIndex].Cells[1].Value=ds.Tables[0].Rows[0][0].ToString();
                       }
                   }
                }
                else
                {
                    string date = "", Saletarget = "", CostPerc = "", WastePerc = "";
                    try
                    {
                        date = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        Saletarget = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        CostPerc = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        WastePerc = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (Saletarget == "")
                    {
                        Saletarget = "0";
                    }
                    if (CostPerc == "")
                    {
                        CostPerc = "0";
                    }
                    if (WastePerc == "")
                    {
                        WastePerc = "0";
                    }
                    string q = "update SalesTargerts set Date='" + date + "', Saletarget='" + Saletarget + "', CostPerc='" + CostPerc + "', WastePerc='" + WastePerc + "', Branchid='" + comboBox1.SelectedValue + "' where id='" + id + "'";
                    objcore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }
    }
}
