using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;

namespace POSRestaurant.Sale
{
    public partial class expenses : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string date = "";
        public expenses()
        {
            InitializeComponent();
        }

        private void expenses_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Supplier";
                ds = objCore.funGetDataSet(q);
                //DataRow dr = ds.Tables[0].NewRow();
                //dr["name"] = "All Users";
                //ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            fillgrid();
        }
        public string shiftid = "";
        protected void fillgrid()
        {
            try
            {
                string q = "SELECT        dbo.CashierExpenses.id, dbo.Supplier.Name, dbo.CashierExpenses.Amount, dbo.CashierExpenses.Date, dbo.CashierExpenses.Terminal FROM            dbo.CashierExpenses INNER JOIN                         dbo.Supplier ON dbo.CashierExpenses.SupplierId = dbo.Supplier.Id where dbo.CashierExpenses.date='" + date + "' and shiftid='"+shiftid+"'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[1].Visible = false;

                q = "select sum(amount) as amount from CashierExpenses where date='" + date + "' and shiftid='"+shiftid+"' and terminal='"+System.Environment.MachineName+"'";
                DataSet dsd = new DataSet();
                dsd = objCore.funGetDataSet(q);
                if (dsd.Tables[0].Rows.Count > 0)
                {
                    lbltotal.Text = "Total Expense added: " + dsd.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public static int strt = 0;
        private void vButton3_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            txtamount.Text = txtamount.Text + btn.Text;
            txtamount.Focus();
            txtamount.SelectionStart = txtamount.Text.Length;
            strt = txtamount.SelectionStart;
        }

        private void vButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (strt > 0)
                {
                    int index = txtamount.SelectionStart;

                    txtamount.Text = txtamount.Text.Remove(strt - 1, 1);
                    // txtcashreceived.Select(index - 1, 1);
                    //txtcashreceived.Select();
                    strt = strt - 1;
                    txtamount.Focus();
                    txtamount.SelectionStart = txtamount.Text.Length;
                    strt = txtamount.SelectionStart;
                    //txtcashreceived.Focus(); 
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtamount.Text = "";
            strt = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtamount.Text == "" )
            {
                MessageBox.Show("Please enetr amount");
                return;
            }
            try
            {
                TextBox txt = txtamount as TextBox;
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
            catch (Exception ex)
            {


            }
            try
            {
                if (comboBox1.SelectedValue.ToString() == "" || comboBox1.SelectedValue.ToString() == "0")
                {

                    MessageBox.Show("Please select valid vendor");
                    return;
                }
                string q = "insert into CashierExpenses(SupplierId, Amount, Date,terminal,shiftid) values('" + comboBox1.SelectedValue + "','" + txtamount.Text + "','" + date + "','" + System.Environment.MachineName + "','" + shiftid + "')";
                objCore.executeQuery(q);
                fillgrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //MessageBox.Show("Added Succefully");
        }

        private void txtamount_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderGrid = (DataGridView)sender;

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    //TODO - Button Clicked - Execute Code Here
                    DialogResult dr = MessageBox.Show("are you sure to delete", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        string q = "delete from CashierExpenses where id=" + id;
                        objCore.executeQuery(q);
                        fillgrid();
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
    }
}
