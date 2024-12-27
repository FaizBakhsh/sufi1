using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Accounts
{
    public partial class BankReconciliation : Form
    {
        public BankReconciliation()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon Objcore = new classes.Clsdbcon();
        private void BankReconciliation_Load(object sender, EventArgs e)
        {
            fillbank();
            getdatabrs();
            getdata(textBox1.Text);
        }
        protected void fillbank()
        {
            try
            {
                string q = "select id,name from ChartofAccounts where name like '%bank%' and accounttype='Current Assets'";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";
            }
            catch (Exception ex)
            {
            
            }
        }
        public void getdata(string keyword)
        {
            try
            {
                string q = "";
                if (radioButton1.Checked == true)
                {
                    if (keyword.Length > 0)
                    {
                        q = "SELECT         Id, Date,  Voucherno, CheckNo, CheckDate, Description, Debit, Credit FROM            BankAccountPaymentSupplier where (ClearanceStatus='Pending' and Voucherno like '%" + keyword + "%') or (ClearanceStatus='Pending' and CheckNo like '%" + keyword + "%') or (ClearanceStatus='Pending' and Description like '%" + keyword + "%')";
                    }
                    else
                    {
                        q = "SELECT         Id, Date,  Voucherno, CheckNo, CheckDate, Description, Debit, Credit FROM            BankAccountPaymentSupplier where ClearanceStatus='Pending'";
                    }
                }
                else
                {
                    if (keyword.Length > 0)
                    {
                        q = "SELECT         Id, Date,  Voucherno, CheckNo, CheckDate, Description, Debit, Credit FROM            BankAccountReceiptCustomer where (ClearanceStatus='Pending' and Voucherno like '%" + keyword + "%') or (ClearanceStatus='Pending' and CheckNo like '%" + keyword + "%') or (ClearanceStatus='Pending' and Description like '%" + keyword + "%')";
                    }
                    else
                    {
                        q = "SELECT         Id, Date,  Voucherno, CheckNo, CheckDate, Description, Debit, Credit FROM            BankAccountReceiptCustomer where ClearanceStatus='Pending'";
                    }
                }
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {
                
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            getdata(textBox1.Text);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    DialogResult dr = MessageBox.Show("Are you sure to Proceed. It can not be reversed", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                        string voucherno = dataGridView1.Rows[e.RowIndex].Cells["Voucherno"].Value.ToString();
                        if (voucherno.ToLower().Contains("bpv"))
                        {
                            string q = "update BankAccountPaymentSupplier set ClearanceStatus='Cleared' where id='" + id + "'";
                            Objcore.executeQuery(q);
                        }
                        if (voucherno.ToLower().Contains("brv"))
                        {
                            string q = "update BankAccountReceiptCustomer set ClearanceStatus='Cleared' where id='" + id + "'";
                            Objcore.executeQuery(q);
                        }
                        MessageBox.Show("Updated successfully");
                        getdata(textBox1.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked == true)
            {
                radioButton1.Checked = false;
            }
            else
            {
                radioButton1.Checked = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                radioButton2.Checked = false;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtcredit.Text == "" && txtdebit.Text == "")
            {
                MessageBox.Show("Please Enter Debit or Credit");
                return;
            }
            if (txtdebit.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtdebit.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtdebit.Focus();
                    return;
                }
            }
            if (txtcredit.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtcredit.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtcredit.Focus();
                    return;
                }
            }
            if (txtbalance.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtbalance.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtbalance.Focus();
                    return;
                }
            }
            try
            {
                if (button1.Text == "Submit")
                {
                    string q = "";
                    if (txtbalance.Text.Length > 0)
                    {
                        q = "insert into BankReconciliation (Accountid,Date, Balance, Credit,Debit, Description) values ('" + comboBox1.SelectedValue + "','" + dateTimePicker1.Text + "','" + txtbalance.Text + "','0','0','Balance')";
               
                    }
                    else
                    {
                        q = "insert into BankReconciliation (Accountid,Date, Debit, Credit, Description) values ('" + comboBox1.SelectedValue + "','" + dateTimePicker1.Text + "','" + txtdebit.Text + "','" + txtcredit.Text + "','" + richTextBox1.Text.Replace("'", "''") + "')";
                    }
                        
                    int res = Objcore.executeQueryint(q);
                    if (res > 0)
                    {
                        MessageBox.Show("Data Saved Successfully");
                        getdatabrs();
                    }
                    else
                    {
                        MessageBox.Show("Error Saving Data");
                    }
                }
                if (button1.Text == "Update")
                {
                    string q = "";
                    if (txtbalance.Text.Length > 0)
                    {
                        q = "update BankReconciliation set Accountid='" + comboBox1.SelectedValue + "',Date='" + dateTimePicker1.Text + "', Balance='" + txtbalance.Text + "',  Description='Balance' where id=" + brsid;
                 
                    }
                    else
                    {
                        q = "update BankReconciliation set Accountid='" + comboBox1.SelectedValue + "',Date='" + dateTimePicker1.Text + "', Debit='" + txtdebit.Text + "', Credit='" + txtcredit.Text + "', Description='" + richTextBox1.Text.Replace("'", "''") + "' where id=" + brsid;
                 
                    }
                      int res = Objcore.executeQueryint(q);
                    if (res > 0)
                    {
                        MessageBox.Show("Data Updated Successfully");
                        getdatabrs();
                        button1.Text = "Submit";
                        txtcredit.Text = "";
                        txtdebit.Text = "";
                        richTextBox1.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Error Updating Data");
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        string brsid = "";
        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdatabrs();
        }
        public void getdatabrs()
        {
            try
            {
                string q = "";
                q = "SELECT   Id, Date, Debit, Credit, Description FROM            BankReconciliation where AccountId='"+comboBox1.SelectedValue+"' order by date desc";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                dataGridView2.DataSource = ds.Tables[0];
                dataGridView2.Columns[2].Visible = false;
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                try
                {
                    string id = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string q = "SELECT   Id, Date, Debit, Credit, Description FROM            BankReconciliation where AccountId='" + comboBox1.SelectedValue + "' order by date desc";
                    DataSet ds = new DataSet();
                    ds = Objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        dateTimePicker1.Text = ds.Tables[0].Rows[0]["date"].ToString();
                        richTextBox1.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                        txtcredit.Text = ds.Tables[0].Rows[0]["Credit"].ToString();
                        txtdebit.Text = ds.Tables[0].Rows[0]["Debit"].ToString();
                        if (ds.Tables[0].Rows[0]["Description"].ToString() == "Balance")
                        {
                            txtcredit.Text = "";
                            txtdebit.Text = "";
                            txtbalance.Text = ds.Tables[0].Rows[0]["Debit"].ToString();
                        }
                        button1.Text = "Update";
                        brsid = id;
                    }
                    getdatabrs();
                }
                catch (Exception ex)
                {
                    
               
                }
            }
            if (e.ColumnIndex == 1)
            {
                DialogResult dr = MessageBox.Show("Are You Sure to Delete?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string id = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    string q = "delete FROM            BankReconciliation where id='" + id + "' ";
                    Objcore.executeQuery(q);
                    getdatabrs();
                }
                
            }
        }

        private void txtdebit_TextChanged(object sender, EventArgs e)
        {
            if (txtdebit.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtdebit.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtdebit.Focus();
                    return;
                }
            }
        }

        private void txtcredit_TextChanged(object sender, EventArgs e)
        {
            if (txtcredit.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtcredit.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtcredit.Focus();
                    return;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Accounts.frmBRS obj = new Reports.Accounts.frmBRS();
            obj.Show();
        }
    }
}
