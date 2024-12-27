using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace POSRetail.Accounts
{
    public partial class Vouchers : Form
    {
        DataTable dt;
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string acid = "";
        public Vouchers()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void getdata(string keyword)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno, Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier order by id asc";
            }
            else
            {
                q = "SELECT     Id, Date, Voucherno, Description, Debit, Credit, CurrentBalance FROM         CashAccountPaymentSupplier where Voucherno like '%" + txtkeyword.Text + "%'  order by id asc";
            }
            ds = objCore.funGetDataSet(q);
            dataGridView1.DataSource = ds.Tables[0];
            dataGridView1.Columns[0].Visible = false;
        }
        public void getdatabank(string keyword)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno,CheckNo, CheckDate, Description, Debit, Credit, CurrentBalance FROM         BankAccountPaymentSupplier  order by id asc";
            }
            else
            {
                q = "SELECT     Id, Date, Voucherno,CheckNo, CheckDate, Description, Debit, Credit, CurrentBalance FROM         BankAccountPaymentSupplier where Voucherno like '%" + txtkeyword4.Text + "%' or CheckNo like '%" + txtkeyword4.Text + "%'  order by id asc";
            }
            ds = objCore.funGetDataSet(q);
            dataGridView4.DataSource = ds.Tables[0];
            dataGridView4.Columns[0].Visible = false;
        }
        public void getdatareceipt(string keyword)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno, Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer  order by id asc";
            }
            else
            {
                q = "SELECT     Id, Date, Voucherno, Description, Debit, Credit, CurrentBalance FROM         CashAccountReceiptCustomer where Voucherno like '%" + txtkeyword3.Text + "%'  order by id asc";
            }
            ds = objCore.funGetDataSet(q);
            dataGridView2.DataSource = ds.Tables[0];
            dataGridView2.Columns[0].Visible = false;
        }
        public void getdatareceiptbank(string keyword)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     Id, Date, Voucherno, CheckNo, CheckDate, Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer  order by id asc";
            }
            else
            {
                q = "SELECT     Id, Date, Voucherno, CheckNo, CheckDate, Description, Debit, Credit, CurrentBalance FROM         BankAccountReceiptCustomer where Voucherno like '%" + txtkeyword3.Text + "%' or CheckNo like '%" + txtkeyword3.Text + "%' order by id asc";
            }
            ds = objCore.funGetDataSet(q);
            dataGridView3.DataSource = ds.Tables[0];
            dataGridView3.Columns[0].Visible = false;
        }
        public void getdatajournal(string keyword)
        {
            DataSet ds = new System.Data.DataSet();
            string q = "";
            if (keyword.Trim() == string.Empty)
            {
                q = "SELECT     dbo.JournalAccount.Id, dbo.JournalAccount.Date, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit , dbo.JournalAccount.Balance                       FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id  order by dbo.JournalAccount.Date,JournalAccount.VoucherNo";
            }
            else
            {
                q = "SELECT     dbo.JournalAccount.Id, dbo.JournalAccount.Date, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit  , dbo.JournalAccount.Balance                      FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.Voucherno like '%" + txtkeyword.Text + "%' or dbo.ChartofAccounts.Name like '%" + txtkeyword.Text + "%' order by dbo.JournalAccount.Date,JournalAccount.VoucherNo";
            }
            ds = objCore.funGetDataSet(q);
            dataGridView5.DataSource = ds.Tables[0];
            dataGridView5.Columns[0].Visible = false;
        }
        private void button4_Click(object sender, EventArgs e)
        {
            getdata(txtkeyword.Text.Trim());
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Vouchers_Load(object sender, EventArgs e)
        {
            try
            {
                dt = new DataTable();
                dt.Columns.Add("id", typeof(string));
                dt.Columns.Add("AccountName", typeof(string));
                dt.Columns.Add("AccountNo", typeof(string));
                dt.Columns.Add("Voucherno", typeof(string));
                dt.Columns.Add("Description", typeof(string));
                dt.Columns.Add("Debit", typeof(string));
                dt.Columns.Add("Credit", typeof(string));
               

                string q = "select * from supplier";
                DataSet ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();

                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "Name";

                cmbsupplier2.DataSource = ds.Tables[0];
                cmbsupplier2.ValueMember = "id";
                cmbsupplier2.DisplayMember = "Name";

                q = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets')";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);

                cmbcashaccount.DataSource = ds.Tables[0];
                cmbcashaccount.ValueMember = "id";
                cmbcashaccount.DisplayMember = "Name";
               
                q = "SELECT     Id,  Name  FROM         ChartofAccounts WHERE     (AccountType = 'Current Assets')";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);

                cmbcashaccount2.DataSource = ds.Tables[0];
                cmbcashaccount2.ValueMember = "id";
                cmbcashaccount2.DisplayMember = "Name";

                cmbcashaccount3.DataSource = ds.Tables[0];
                cmbcashaccount3.ValueMember = "id";
                cmbcashaccount3.DisplayMember = "Name";

                cmbcashaccount4.DataSource = ds.Tables[0];
                cmbcashaccount4.ValueMember = "id";
                cmbcashaccount4.DisplayMember = "Name";

                q = "SELECT     Id,  Name  FROM         Customers ";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);

                cmbcustomers.DataSource = ds.Tables[0];
                cmbcustomers.ValueMember = "id";
                cmbcustomers.DisplayMember = "Name";

                cmbcustomers2.DataSource = ds.Tables[0];
                cmbcustomers2.ValueMember = "id";
                cmbcustomers2.DisplayMember = "Name";

                q = "SELECT     Id,  Name  FROM         ChartofAccounts ";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);

                cmbaccount.DataSource = ds.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "Name";
                
                DataSet dscode = new System.Data.DataSet();
                q = "SELECT      AccountCode  FROM         ChartofAccounts where id='" + cmbaccount.SelectedValue + "'";
                dscode = objCore.funGetDataSet(q);
                if (dscode.Tables[0].Rows.Count > 0)
                {
                    txtcode.Text = dscode.Tables[0].Rows[0][0].ToString();
                }
                
                getdata("");
                getdatabank("");
                getdatareceipt("");
                getdatareceiptbank("");
                getdatajournal("");
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("form load error");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbsupplier.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Supplier");
                        return;
                    }
                    if (txtamount.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount.Focus();
                            return;
                        }
                    }
                    if (txtdesc.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button1.Text == "Add")
                {
                    if (txtvoucherno.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from CashAccountPaymentSupplier");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
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
                        string q = "select top 1 * from CashAccountPaymentSupplier  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance - Convert.ToDouble(txtamount.Text));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into CashAccountPaymentSupplier (id,Date,ChartAccountId,SupplierId,Voucherno,Description,Debit,Credit,CurrentBalance) values('" + idd + "','" + dateTimePicker1.Text + "','" + cmbcashaccount.SelectedValue + "','" + cmbsupplier.SelectedValue + "','CPV-" + idd + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','0','" + txtamount.Text + "','" + newbalance + "')";
                        objCore.executeQuery(q);
                        txtvoucherno.Text = "CPV-" + idd;
                        supplieraccount(txtamount.Text, "");
                        MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button1.Text == "Update")
                {
                    string q = "select  * from CashAccountPaymentSupplier where ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();
                               
                                updatecashaccountfirst(val);
                                //return;
                                chk = true;
                            }
                            
                        }

                    }


                    supplieraccount(txtamount.Text, txtvoucherno.Text);
                }
            }
            catch (Exception ex)
            {
                
                
            }
            getdata("");
        }
        public void updatecashaccountfirst(string val)
        {
            try
            {
                string q = "select * from CashAccountPaymentSupplier where id < '" + val + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance - Convert.ToDouble(txtamount.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update CashAccountPaymentSupplier set Date='" + dateTimePicker1.Text + "',ChartAccountId='" + cmbcashaccount.SelectedValue + "',SupplierId='" + cmbsupplier.SelectedValue + "', Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount.Text + "' , CurrentBalance='" + newbalance + "' where id='" + acid + "'";
                objCore.executeQuery(q);
                updatecashaccount(val);

                ////new--------------------------------

                //string q = "";
                ////DataSet dss = new System.Data.DataSet();
                ////dss = objCore.funGetDataSet(q);
                ////string blnce = "0";
                ////if (dss.Tables[0].Rows.Count > 0)
                ////{
                ////    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                ////}
                ////double balance = Convert.ToDouble(blnce);

                ////double newbalance = (balance - Convert.ToDouble(txtamount.Text));
                ////newbalance = Math.Round(newbalance, 2);
                //objCore = new classes.Clsdbcon();
                //q = "update CashAccountPaymentSupplier set Date='" + dateTimePicker1.Text + "',ChartAccountId='" + cmbcashaccount.SelectedValue + "',SupplierId='" + cmbsupplier.SelectedValue + "', Description='" + txtdesc.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount.Text + "'  where id='" + val + "'";
                //objCore.executeQuery(q);
                //updatecashaccount2(cmbcashaccount.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                
              MessageBox.Show("update cash account first error");
            }
        }
        public void updatecashaccount2(string val)
        {
            try
            {
                string q = "select * from CashAccountPaymentSupplier where ChartAccountId ='" + val + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i]["CurrentBalance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }

                        newbalance = Convert.ToDouble(blnce);
                    }
                    else
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }
                        newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit);

                    }
                    newbalance = Math.Round(newbalance, 2);
                    objCore = new classes.Clsdbcon();
                    q = "update CashAccountPaymentSupplier set     Debit='" + debit + "' ,credit='" + credit + "' , CurrentBalance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() + "'";
                    objCore.executeQuery(q);

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account  error");
            }
        }
        public void updatecashaccount(string val)
        {

            try
            {
                string q = "select * from CashAccountPaymentSupplier where id >='" + val + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("update cash account error");
            }
        }
        public void updatecashaccountremaining(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CashAccountPaymentSupplier set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatecashaccount(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletecashaccount(count);
                }
                return;
            }
            catch (Exception ex) 
            {
                
                MessageBox.Show("update cash account remaining error");
            }
        }
        public void deletecashaccountfirst(int id)
        {
            try
            {
                string q = "select * from CashAccountPaymentSupplier where id <'" + id + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update CashAccountPaymentSupplier set  Credit='0',CurrentBalance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deletecashaccount(id);
            }
            catch (Exception ex)
            {
                
               MessageBox.Show("delete cash account first error");
            }
        }
        public void deletecashaccount(int id)
        {

            try
            {
                string q = "select * from CashAccountPaymentSupplier where id >='" + id + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show("delete cash account error");
            }
            getdata("");
        }


        public void updatecashaccountfirstbank(string val)
        {
            try
            {
                string q = "select * from BankAccountPaymentSupplier where id < '" + val + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance - Convert.ToDouble(txtamount4.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update BankAccountPaymentSupplier set Date='" + dateTimePicker4.Text + "',ChartAccountId='" + cmbcashaccount4.SelectedValue + "',SupplierId='" + cmbsupplier2.SelectedValue + "', Description='" + txtdesc4.Text.Trim().Replace("'", "''") + "', Credit='" + txtamount4.Text + "' , CurrentBalance='" + newbalance + "' where id='" + val + "'";
                objCore.executeQuery(q);
                updatecashaccountbank(val);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash bank first  error");
            }
        }
        public void updatecashaccountbank(string val)
        {

            try
            {
                string q = "select * from BankAccountPaymentSupplier where id >='" + val + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account  error");
            }
        }
        public void updatecashaccountremainingbank(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update BankAccountPaymentSupplier set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatecashaccountbank(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletecashaccountbank(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account remaining error");
            }
        }
        public void deletecashaccountfirstbank(int id)
        {
            try
            {
                string q = "select * from BankAccountPaymentSupplier where id <'" + id + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update BankAccountPaymentSupplier set  Credit='0',CurrentBalance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deletecashaccountbank(id);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete bank account first error");
            }
        }
        public void deletecashaccountbank(int id)
        {

            try
            {
                string q = "select * from BankAccountPaymentSupplier where id >='" + id + "' and ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete bank account error");
            }
            getdatabank("");
        }


        public void supplieraccount(string amount , string id)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select payableaccountid from Supplier where id='" + cmbsupplier.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    int iddd = 0;
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from SupplierAccount where SupplierId='" + cmbsupplier.SelectedValue + "' and PayableAccountId='" + PayableAccountId + "'  order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    if (button1.Text == "Add")
                    {
                        q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + dateTimePicker1.Text + "','" + cmbsupplier.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno.Text + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                        objCore.executeQuery(q);
                    }
                    if (button1.Text == "Update")
                    {
                        DataSet dsval = new System.Data.DataSet();
                        q = "select id from SupplierAccount where VoucherNo='" + txtvoucherno.Text + "'";
                        dsval = objCore.funGetDataSet(q);
                        if (dsval.Tables[0].Rows.Count > 0)
                        {
                            updatesupplieraccountfirst(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                        }

                        //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    }

                }
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show("supplier account error");
            }
        }
        public void updatesupplieraccountfirst(string val, string payaccount)
        {
            try
            {
                string q = "select * from SupplierAccount where id < '" + val + "'  and SupplierId='" + cmbsupplier.SelectedValue + "' and PayableAccountId='" + payaccount + "' order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance + Convert.ToDouble(txtamount.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "' , Balance='" + newbalance + "' where id='" + val + "'";
                objCore.executeQuery(q);
                updatesupplieraccount(val, payaccount);

                //new-------

                //string q = "";
               
                //objCore = new classes.Clsdbcon();
                //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "', Debit='" + txtamount.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "' where id='" + val + "'";
                //objCore.executeQuery(q);
                //updatesupplieraccount2(payaccount);
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("update supplier first account error");
            }
        }
        public void updatesupplieraccount2(string val)
        {
            try
            {
                string q = "select * from SupplierAccount where PayableAccountId ='" + val + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }

                        newbalance = Convert.ToDouble(blnce);
                    }
                    else
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }
                        newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit);

                    }
                    newbalance = Math.Round(newbalance, 2);
                    objCore = new classes.Clsdbcon();
                    q = "update SupplierAccount set     Debit='" + debit + "' ,credit='" + credit + "' , CurrentBalance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() + "'";
                    objCore.executeQuery(q);
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account  error");
            }
        }
        public void updatesupplieraccount(string val, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + val + "' and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show("update supplier account error");
            }
        }
        public void updatesupplieraccountremaining(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update SupplierAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatesupplieraccount(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deletesupplieraccount(count, payaccount);
                }
            }
            catch (Exception ex)
            {
                
                 MessageBox.Show("update supplier remaining account error");
            }
            return;
        }
        public void deletesupplieraccountfirst(int id, string payaccount)
        {
            try
            {
                string q = "select * from SupplierAccount where id <'" + id + "' and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier.SelectedValue + "' order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update SupplierAccount set  Debit='0',Balance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deletesupplieraccount(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier first account error");
            }
        }
        public void deletesupplieraccount(int id, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + id + "' and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier account error");
            }
            getdata("");
        }


        public void supplieraccountBank(string amount, string id)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select payableaccountid from Supplier where id='" + cmbsupplier2.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    int iddd = 0;
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from SupplierAccount where SupplierId='" + cmbsupplier2.SelectedValue + "' and PayableAccountId='" + PayableAccountId + "'  order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    if (button21.Text == "Add")
                    {
                        q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,CheckNo, CheckDate,EntryType) values('" + iddd + "','" + dateTimePicker4.Text + "','" + cmbsupplier2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno4.Text + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "','" + txtchechkno2.Text.Replace("'", "''") + "','" + txtcheckdate2.Text.Trim().Replace("'","''") + "','Bank')";
                        objCore.executeQuery(q);
                    }
                    if (button21.Text == "Update")
                    {
                        DataSet dsval = new System.Data.DataSet();
                        q = "select id from SupplierAccount where VoucherNo='" + txtvoucherno4.Text + "'";
                        dsval = objCore.funGetDataSet(q);
                        if (dsval.Tables[0].Rows.Count > 0)
                        {
                            updatesupplieraccountfirstbank(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                        }

                        //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("supplier account bank error");
            }
        }
        public void updatesupplieraccountfirstbank(string val, string payaccount)
        {
            try
            {
                string q = "select * from SupplierAccount where id < '" + val + "'  and SupplierId='" + cmbsupplier2.SelectedValue + "'  and PayableAccountId='" + payaccount + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance + Convert.ToDouble(txtamount4.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update SupplierAccount set Date='" + dateTimePicker4.Text + "', Debit='" + txtamount4.Text + "' , Balance='" + newbalance + "' where id='" + val + "'";
                objCore.executeQuery(q);
                updatesupplieraccountbank(val, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier first bank account error");
            }
        }
        public void updatesupplieraccountbank(string val, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + val + "'   and PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier2.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier account bank error");
            }
        }
        public void updatesupplieraccountremainingbank(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update SupplierAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatesupplieraccountbank(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deletesupplieraccountbank(count, payaccount);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update supplier remaining account bank error");
            }
            return;
        }
        public void deletesupplieraccountfirstbank(int id, string payaccount)
        {
            try
            {
                string q = "select * from SupplierAccount where id <'" + id + "' and    PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier2.SelectedValue + "' order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update SupplierAccount set  Debit='0',Balance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deletesupplieraccountbank(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier first account bank error");
            }
        }
        public void deletesupplieraccountbank(int id, string payaccount)
        {

            try
            {
                string q = "select * from SupplierAccount where id >='" + id + "' and  PayableAccountId='" + payaccount + "' and SupplierId='" + cmbsupplier2.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatesupplieraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete supplier account bank error");
            }
            getdata("");
        }


        public void updatecashaccountfirstcustomer(string val)
        {
            try
            {
                string q = "select * from CashAccountReceiptCustomer where id < '" + val + "' and ChartAccountId='" + cmbcashaccount.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance + Convert.ToDouble(txtamount2.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update CashAccountReceiptCustomer set Date='" + dateTimePicker2.Text + "',ChartAccountId='" + cmbcashaccount2.SelectedValue + "',CustomerId='" + cmbcustomers.SelectedValue + "', Description='" + txtdesc2.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount2.Text + "' , CurrentBalance='" + newbalance + "' where id='" + val + "'";
                objCore.executeQuery(q);
                updatecashaccountcustomer(val);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account receipt first error");
            }
        }
        public void updatecashaccountcustomer(string val)
        {

            try
            {
                string q = "select * from CashAccountReceiptCustomer where id >='" + val + "' and ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash account receipt error");
            }
        }
        public void updatecashaccountremainingcustomer(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CashAccountReceiptCustomer set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatecashaccountcustomer(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletecashaccountcustomer(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update cash receipt account receipt remaining error");
            }
        }

        public void deletecashaccountfirstcustomer(int id)
        {
            try
            {
                string q = "select * from CashAccountReceiptCustomer where id <'" + id + "' and ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update CashAccountReceiptCustomer set  Debit='0',CurrentBalance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deletecashaccountcustomer(id);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account receipt first error");
            }
        }
        public void deletecashaccountcustomer(int id)
        {

            try
            {
                string q = "select * from CashAccountReceiptCustomer where id >='" + id + "' and ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatecashaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account receipt error");
            }
            getdatareceipt("");
        }



        public void Customeraccount(string amount, string id)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select Chartaccountid from Customers where id='" + cmbcustomers.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    int iddd = 0;
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from CustomerAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CustomerAccount where CustomersId='" + cmbcustomers.SelectedValue + "' and PayableAccountId='" + PayableAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    if (button12.Text == "Add")
                    {
                        q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType) values('" + iddd + "','" + dateTimePicker2.Text + "','" + cmbcustomers.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno2.Text + "','" + txtdesc2.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "','Cash')";
                        objCore.executeQuery(q);
                    }
                    if (button12.Text == "Update")
                    {
                        DataSet dsval = new System.Data.DataSet();
                        q = "select id from CustomerAccount where VoucherNo='" + txtvoucherno2.Text + "'";
                        dsval = objCore.funGetDataSet(q);
                        if (dsval.Tables[0].Rows.Count > 0)
                        {
                            updateCustomeraccountfirst(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                        }

                        //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Customer account error");
            }
        }
        public void updateCustomeraccountfirst(string val, string payaccount)
        {
            try
            {
                string q = "select * from CustomerAccount where id < '" + val + "'  and CustomersId='" + cmbcustomers.SelectedValue + "' and PayableAccountId='" + payaccount + "' order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance - Convert.ToDouble(txtamount2.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update CustomerAccount set Date='" + dateTimePicker2.Text + "', Credit='" + txtamount2.Text + "' , Balance='" + newbalance + "' where id='" + val + "'";
                objCore.executeQuery(q);
                updateCustomeraccount(val, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer first account error");
            }
        }
        public void updateCustomeraccount(string val, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + val + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer account error");
            }
        }
        public void updateCustomeraccountremaining(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CustomerAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updateCustomeraccount(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deleteCustomeraccount(count, payaccount);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer remaining account error");
            }
            return;
        }
        public void deleteCustomeraccountfirst(int id, string payaccount)
        {
            try
            {
                string q = "select * from CustomerAccount where id <'" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers.SelectedValue + "' order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update CustomerAccount set  Credit='0',Balance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deleteCustomeraccount(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer first account error");
            }
        }
        public void deleteCustomeraccount(int id, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers.SelectedValue + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer account error");
            }
            getdatareceipt("");
        }

        public void Customeraccountbank(string amount, string id)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select Chartaccountid from Customers where id='" + cmbcustomers2.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    int iddd = 0;
                    DataSet dsa = objCore.funGetDataSet("select max(id) as id from CustomerAccount");
                    if (dsa.Tables[0].Rows.Count > 0)
                    {
                        string i = dsa.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CustomerAccount where CustomersId='" + cmbcustomers2.SelectedValue + "' and PayableAccountId='" + PayableAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    if (button15.Text == "Add")
                    {
                        q = "insert into CustomerAccount (Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,EntryType, CheckNo, CheckDate) values('" + iddd + "','" + dateTimePicker3.Text + "','" + cmbcustomers2.SelectedValue + "','" + PayableAccountId + "','" + txtvoucherno3.Text + "','" + txtdesc3.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "','Bank','" + txtchechkno.Text.Trim().Replace("'", "''") + "','" + txtcheckdate.Text.Trim().Replace("'", "''") + "')";
                        objCore.executeQuery(q);
                    }
                    if (button15.Text == "Update")
                    {
                        DataSet dsval = new System.Data.DataSet();
                        q = "select id from CustomerAccount where VoucherNo='" + txtvoucherno3.Text + "'";
                        dsval = objCore.funGetDataSet(q);
                        if (dsval.Tables[0].Rows.Count > 0)
                        {
                            updateCustomeraccountfirstbank(dsval.Tables[0].Rows[0][0].ToString(), PayableAccountId);
                        }

                        //q = "update SupplierAccount set Date='" + dateTimePicker1.Text + "',SupplierId='" + cmbsupplier.SelectedValue + "',PayableAccountId='" + PayableAccountId + "',VoucherNo='CPV-" + txtvoucherno.Text + "',Description='" + txtdesc.Text.Trim().Replace("'", "''") + "',Debit='" + Math.Round(Convert.ToDouble(amount), 2) + "',Balance='" + newbalance + "' where VoucherNo='" + id + "'";
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Customer account bank error");
            }
        }
        public void updateCustomeraccountfirstbank(string val, string payaccount)
        {
            try
            {
                string q = "select * from CustomerAccount where id < '" + val + "'  and CustomersId='" + cmbcustomers2.SelectedValue + "' and PayableAccountId='" + payaccount + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance - Convert.ToDouble(txtamount3.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update CustomerAccount set Date='" + dateTimePicker3.Text + "', Credit='" + txtamount3.Text + "' , Balance='" + newbalance + "' where id='" + val + "'";
                objCore.executeQuery(q);
                updateCustomeraccountbank(val, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer first account bank error");
            }
        }
        public void updateCustomeraccountbank(string val, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + val + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update", payaccount);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer account bank error");
            }
        }
        public void updateCustomeraccountremainingbank(string val, string amount, string id, string functioncall, string payaccount)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance - Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update CustomerAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updateCustomeraccountbank(count.ToString(), payaccount);
                }
                if (functioncall == "delete")
                {
                    deleteCustomeraccountbank(count, payaccount);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update Customer remaining account error");
            }
            return;
        }
        public void deleteCustomeraccountfirstbank(int id, string payaccount)
        {
            try
            {
                string q = "select * from CustomerAccount where id <'" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers2.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update CustomerAccount set  Credit='0',Balance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deleteCustomeraccountbank(id, payaccount);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer first account error");
            }
        }
        public void deleteCustomeraccountbank(int id, string payaccount)
        {

            try
            {
                string q = "select * from CustomerAccount where id >='" + id + "' and PayableAccountId='" + payaccount + "' and CustomersId='" + cmbcustomers2.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updateCustomeraccountremainingbank(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete", payaccount);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete Customer account error");
            }
            getdatareceipt("");
        }

        public void updateBankaccountfirstcustomer(string val)
        {
            try
            {
                string q = "select * from BankAccountReceiptCustomer where id < '" + val + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                double balance = Convert.ToDouble(blnce);

                double newbalance = (balance + Convert.ToDouble(txtamount3.Text));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                q = "update BankAccountReceiptCustomer set  CheckNo='" + txtchechkno.Text.Replace("'", "''") + "', CheckDate='" + txtcheckdate.Text.Replace("'", "''") + "', Date='" + dateTimePicker3.Text + "',ChartAccountId='" + cmbcashaccount3.SelectedValue + "',CustomerId='" + cmbcustomers2.SelectedValue + "', Description='" + txtdesc3.Text.Trim().Replace("'", "''") + "', Debit='" + txtamount3.Text + "' , CurrentBalance='" + newbalance + "' where id='" + val + "'";
                objCore.executeQuery(q);
                updatebankaccountcustomer(val);
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account receipt first error");
            }
        }
        public void updatebankaccountcustomer(string val)
        {

            try
            {
                string q = "select * from BankAccountReceiptCustomer where id >='" + val + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatebankaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update");
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank account receipt error");
            }
        }
        public void updatebankaccountremainingcustomer(string val, string amount, string id, string functioncall)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(amount));
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update BankAccountReceiptCustomer set CurrentBalance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatebankaccountcustomer(count.ToString());
                }
                if (functioncall == "delete")
                {
                    deletebankaccountcustomer(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update bank receipt account receipt remaining error");
            }
        }

        public void deletebankaccountfirstcustomer(int id)
        {
            try
            {
                string q = "select * from BankAccountReceiptCustomer where id <'" + id + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id desc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                if (dss.Tables[0].Rows.Count > 0)
                {
                    blnce = dss.Tables[0].Rows[0]["CurrentBalance"].ToString();
                }
                objCore = new classes.Clsdbcon();
                q = "update BankAccountReceiptCustomer set  Debit='0',CurrentBalance='" + blnce + "' where id='" + id + "'";
                objCore.executeQuery(q);
                deletebankaccountcustomer(id);
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete bank account receipt first error");
            }
        }
        public void deletebankaccountcustomer(int id)
        {

            try
            {
                string q = "select * from BankAccountReceiptCustomer where id >='" + id + "' and ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            string val = dss.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            updatebankaccountremainingcustomer(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "delete");
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("delete cash account receipt error");
            }
            getdatareceipt("");
        }
        
        private void txtamount_TextChanged(object sender, EventArgs e)
        {
            if (txtamount.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Price Per Package. Only Nymbers are allowed");
                    txtamount.Focus();
                    return;
                }
            }
        }
        public void getinfo(string id)
        {
            try
            {
                string q = "select  * from CashAccountPaymentSupplier where id='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    cmbsupplier.SelectedValue = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                    cmbcashaccount.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount.Text = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                    txtdesc.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    button1.Text = "Update";
                    acid = dsacount.Tables[0].Rows[0]["id"].ToString();
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void getinfobank(string id)
        {
            try
            {
                string q = "select  * from BankAccountPaymentSupplier where id='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    cmbsupplier2.SelectedValue = dsacount.Tables[0].Rows[0]["SupplierId"].ToString();
                    cmbcashaccount4.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno4.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount4.Text = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                    txtdesc4.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    button21.Text = "Update";
                    txtchechkno2.Text = dsacount.Tables[0].Rows[0]["CheckNo"].ToString();
                    txtcheckdate2.Text = dsacount.Tables[0].Rows[0]["CheckDate"].ToString();
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void getinforeceipt(string id)
        {
            try
            {
                string q = "select  * from CashAccountReceiptCustomer where id='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    cmbcustomers.SelectedValue = dsacount.Tables[0].Rows[0]["CustomerId"].ToString();
                    cmbcashaccount2.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno2.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount2.Text = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                    txtdesc2.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    button12.Text = "Update";
                    //acid = dsacount.Tables[0].Rows[0]["id"].ToString();
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void getinforeceiptbank(string id)
        {
            try
            {
                string q = "select  * from BankAccountReceiptCustomer where id='" + id + "' order by id desc";
                DataSet dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    cmbcustomers2.SelectedValue = dsacount.Tables[0].Rows[0]["CustomerId"].ToString();
                    cmbcashaccount3.SelectedValue = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                    txtvoucherno3.Text = dsacount.Tables[0].Rows[0]["Voucherno"].ToString();
                    txtamount3.Text = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                    txtdesc3.Text = dsacount.Tables[0].Rows[0]["Description"].ToString();
                    txtchechkno.Text = dsacount.Tables[0].Rows[0]["CheckNo"].ToString();
                    txtcheckdate.Text = dsacount.Tables[0].Rows[0]["CheckDate"].ToString();
                    button15.Text = "Update";
                    //acid = dsacount.Tables[0].Rows[0]["id"].ToString();
                    // cmbcashaccount.Text = dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }

        public void getinforejournaledit(string id)
        {
            try
            {
                int indx = dataGridView6.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    txtvoucherno5.Text = dataGridView6.Rows[indx].Cells["voucherno"].Value.ToString();
                    txtdebit.Text = dataGridView6.Rows[indx].Cells["debit"].Value.ToString();
                    txtcredit.Text = dataGridView6.Rows[indx].Cells["credit"].Value.ToString();
                    txtdesc5.Text = dataGridView6.Rows[indx].Cells["description"].Value.ToString();
                    cmbaccount.SelectedValue = dataGridView6.Rows[indx].Cells["id"].Value.ToString();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show("Get Info error");
            }
        }
        public void chartacounts()
        {
            DataSet dscharts = new System.Data.DataSet();
            string q="select * from ChartofAccounts";
            dscharts = objCore.funGetDataSet(q);
            for (int i = 0; i < dscharts.Tables[0].Rows.Count; i++)
            {
                updatejournalaccountfirst(dscharts.Tables[0].Rows[i]["id"].ToString());
            }
        }
        public void updatejournalaccountfirst(string val)
        {
            try
            {
                string q = "select * from JournalAccount where PayableAccountId ='" + val + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }
                       
                        newbalance = Convert.ToDouble(blnce);
                    }
                    else
                    {
                        debit = dss.Tables[0].Rows[i]["debit"].ToString();
                        credit = dss.Tables[0].Rows[i]["credit"].ToString();
                        blnce = dss.Tables[0].Rows[i-1]["Balance"].ToString();
                        if (debit == "")
                        {
                            debit = "0";
                        }
                        if (credit == "")
                        {
                            credit = "0";
                        }
                        if (blnce == "")
                        {
                            blnce = "0";
                        }
                        newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit); 
                        
                    }
                    newbalance = Math.Round(newbalance, 2);
                    objCore = new classes.Clsdbcon();
                    q = "update JournalAccount set     Debit='" + debit + "' ,credit='" + credit + "' , Balance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() +"'";
                    objCore.executeQuery(q);
                }
                              
            }
            catch (Exception ex)
            {

                MessageBox.Show("update journal  error");
            }
        }
        //public void updatejournalaccountfirst(string val, string acount, string debit, string credit, string date, string desc)
        //{
        //    try
        //    {
        //        string q = "select * from JournalAccount where id < '" + val + "' and PayableAccountId='" + acount + "' order by id desc";
        //        DataSet dss = new System.Data.DataSet();
        //        dss = objCore.funGetDataSet(q);
        //        string blnce = "0";
        //        if (dss.Tables[0].Rows.Count > 0)
        //        {
        //            blnce = dss.Tables[0].Rows[0]["Balance"].ToString();
        //        }
        //        double balance = Convert.ToDouble(blnce);

        //        double newbalance = (balance + Convert.ToDouble(debit)) - Convert.ToDouble(credit);
        //        newbalance = Math.Round(newbalance, 2);
        //        objCore = new classes.Clsdbcon();
        //        q = "update JournalAccount set    Date='" + date + "', Description='" + desc + "', Debit='" + debit + "' ,credit='" + credit + "' , Balance='" + newbalance + "' where id='" + val + "'";
        //        objCore.executeQuery(q);
        //        updatejournalaccount(val, acount);
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show("update journal  first error");
        //    }
        //}
        public void updatejournalaccount(string val, string acount)
        {

            try
            {
                string q = "select * from JournalAccount where id >='" + val + "' and PayableAccountId='" + acount + "'  order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                    {
                        if (i > 0 && i < 2)
                        {
                            val = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            string val1 = dss.Tables[0].Rows[i]["Debit"].ToString();
                            if (val == "")
                            {
                                val = "0";

                            }
                            if (val1 == "")
                            {
                                val1 = "0";

                            }

                            string val2 = dss.Tables[0].Rows[i]["Credit"].ToString();
                            if (val2 == "")
                            {
                                val2 = "0";

                            }
                            

                            updatejournalaccountremaining(val, val1, dss.Tables[0].Rows[i]["id"].ToString(), "update",acount,val2);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("update journal error");
            }
        }
        public void updatejournalaccountremaining(string val, string debit, string id, string functioncall,string acount,string credit)
        {
            try
            {
                double balance = Convert.ToDouble(val);

                double newbalance = (balance + Convert.ToDouble(debit)) - Convert.ToDouble(credit);
                newbalance = Math.Round(newbalance, 2);
                objCore = new classes.Clsdbcon();
                string q = "update JournalAccount set Balance='" + newbalance + "' where id='" + id + "'";
                objCore.executeQuery(q);
                int count = Convert.ToInt32(id);
                if (functioncall == "update")
                {
                    updatejournalaccount(count.ToString(),acount);
                }
                if (functioncall == "delete")
                {
                   // deletebankaccountcustomer(count);
                }
                return;
            }
            catch (Exception ex)
            {

                MessageBox.Show("update journal remaining error");
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    getinfo(id);

                }
            }
            catch (Exception ex)
            {


            }
        }
        public void clear()
        {
            try
            {

                txtvoucherno.Text = "";
                txtamount.Text = "";
                txtdesc.Text = "";
                button1.Text = "Add";
                acid = "";
                cmbsupplier.SelectedValue = "";
                cmbcashaccount.SelectedValue = "";
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public void clear2()
        {
            try
            {

                txtvoucherno2.Text = "";
                txtamount2.Text = "";
                txtdesc2.Text = "";
                button12.Text = "Add";
                //acid = "";
                cmbcustomers.SelectedValue = "";
                cmbcashaccount2.SelectedValue = "";
            }
            catch (Exception ex)
            {


            }
        }
        public void clear3()
        {
            try
            {

                txtvoucherno3.Text = "";
                txtamount3.Text = "";
                txtdesc3.Text = "";
                button15.Text = "Add";
                //acid = "";
                txtchechkno.Text = "";
                cmbcustomers2.SelectedValue = "";
                cmbcashaccount3.SelectedValue = "";
            }
            catch (Exception ex)
            {


            }
        }
        public void clear4()
        {
            try
            {

                txtvoucherno4.Text = "";
                txtamount4.Text = "";
                txtdesc4.Text = "";
                button21.Text = "Add";
                //acid = "";
                txtchechkno2.Text = "";
                cmbsupplier2.SelectedValue = "";
                cmbcashaccount4.SelectedValue = "";
            }
            catch (Exception ex)
            {


            }
        }
        public void clear5()
        {
            try
            {

                txtvoucherno5.Text = "";
                txtdebit.Text = "";
                txtcredit.Text = "";
                txtdesc5.Text = "";
                //button29.Text = "Save";
                //acid = "";
               
               
            }
            catch (Exception ex)
            {


            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void cmbsupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata("");
        }

        private void cmbcashaccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata("");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    deletecashaccountfirst(Convert.ToInt32(id));
                    DataSet dsval=new System.Data.DataSet();
                    string q = "select id,PayableAccountId from SupplierAccount where VoucherNo='" + dataGridView1.Rows[indx].Cells[2].Value.ToString() + "'";
                    dsval=objCore.funGetDataSet(q);
                    if(dsval.Tables[0].Rows.Count>0)
                    {
                        deletesupplieraccountfirst(Convert.ToInt32(dsval.Tables[0].Rows[0]["id"].ToString()), dsval.Tables[0].Rows[0]["PayableAccountId"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbcustomers.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Customer");
                        return;
                    }
                    if (txtamount2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount2.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount2.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount2.Focus();
                            return;
                        }
                    }
                    if (txtdesc2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button12.Text == "Add")
                {
                    if (txtvoucherno2.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from CashAccountReceiptCustomer");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
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
                        string q = "select top 1 * from CashAccountReceiptCustomer where ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance + Convert.ToDouble(txtamount2.Text));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into CashAccountReceiptCustomer (id,Date,ChartAccountId,CustomerId,Voucherno,Description,Debit,Credit,CurrentBalance) values('" + idd + "','" + dateTimePicker2.Text + "','" + cmbcashaccount2.SelectedValue + "','" + cmbcustomers.SelectedValue + "','CRV-" + idd + "','" + txtdesc2.Text.Trim().Replace("'", "''") + "','" + txtamount2.Text + "','0','" + newbalance + "')";
                        objCore.executeQuery(q);
                        txtvoucherno2.Text = "CRV-" + idd;
                        Customeraccount(txtamount2.Text, "");
                        MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button12.Text == "Update")
                {
                    string q = "select  * from CashAccountReceiptCustomer where ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno2.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();
                                //if (val == "")
                                //{
                                //    val = "0";

                                //}
                                updatecashaccountfirstcustomer(val);
                                //return;
                                chk = true;
                            }
                            //if (chk == true)
                            //{
                            //    val = dsacount.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            //    string val1 = dsacount.Tables[0].Rows[i]["Credit"].ToString();
                            //    if (val == "")
                            //    {
                            //        val = "0";

                            //    }
                            //    if (val1 == "")
                            //    {
                            //        val1 = "0";

                            //    }
                            //    updatecashaccountremaining(val, val1, dsacount.Tables[0].Rows[i]["id"].ToString());
                            //}
                        }

                    }

                    Customeraccount(txtamount2.Text, txtvoucherno2.Text);
                   
                }
            }
            catch (Exception ex)
            {


            }

            getdatareceipt("");
        }

        private void txtamount_TextChanged_1(object sender, EventArgs e)
        {
            if (txtamount.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount.Focus();
                    return;
                }
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            getdatareceipt(txtkeyword3.Text.Trim());
        }

        private void button11_Click(object sender, EventArgs e)
        {
            clear2();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView2.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView2.Rows[indx].Cells[0].Value.ToString();
                    getinforeceipt(id);

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView2.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView2.Rows[indx].Cells[0].Value.ToString();
                    deletecashaccountfirstcustomer(Convert.ToInt32(id));
                    DataSet dsval = new System.Data.DataSet();
                    string q = "select id,PayableAccountId from CustomerAccount where VoucherNo='" + dataGridView2.Rows[indx].Cells[2].Value.ToString() + "'";
                    dsval = objCore.funGetDataSet(q);
                    if (dsval.Tables[0].Rows.Count > 0)
                    {
                        deleteCustomeraccountfirst(Convert.ToInt32(dsval.Tables[0].Rows[0]["id"].ToString()), dsval.Tables[0].Rows[0]["PayableAccountId"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label18_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void button15_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount3.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbcustomers2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Customer");
                        return;
                    }
                    if (txtamount3.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount3.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount3.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount3.Focus();
                            return;
                        }
                    }
                    if (txtdesc3.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button15.Text == "Add")
                {
                    if (txtvoucherno3.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from BankAccountReceiptCustomer");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
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
                        string q = "select top 1 * from BankAccountReceiptCustomer where ChartAccountId='" + cmbcashaccount3.SelectedValue + "'  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance + Convert.ToDouble(txtamount3.Text));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into BankAccountReceiptCustomer (id,Date,ChartAccountId,CustomerId,Voucherno,Description,Debit,Credit,CurrentBalance, CheckNo, CheckDate) values('" + idd + "','" + dateTimePicker3.Text + "','" + cmbcashaccount3.SelectedValue + "','" + cmbcustomers2.SelectedValue + "','BRV-" + idd + "','" + txtdesc3.Text.Trim().Replace("'", "''") + "','" + txtamount3.Text + "','0','" + newbalance + "','" + txtchechkno.Text.Trim().Replace("'", "''") + "','" + txtcheckdate.Text.Trim().Replace("'","''") + "')";
                        objCore.executeQuery(q);
                        txtvoucherno3.Text = "BRV-" + idd;
                        Customeraccountbank(txtamount3.Text, "");
                        MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button15.Text == "Update")
                {
                    string q = "select  * from BankAccountReceiptCustomer where ChartAccountId='" + cmbcashaccount2.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno3.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();
                                
                                updateBankaccountfirstcustomer(val);
                                //return;
                                chk = true;
                            }
                            
                        }

                    }

                    Customeraccountbank(txtamount3.Text, txtvoucherno3.Text);

                }
            }
            catch (Exception ex)
            {


            }

            getdatareceiptbank("");
        }

        private void txtamount2_TextChanged(object sender, EventArgs e)
        {
            if (txtamount2.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount2.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount2.Focus();
                    return;
                }
            }
        }

        private void txtamount3_TextChanged(object sender, EventArgs e)
        {
            if (txtamount3.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount3.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount3.Focus();
                    return;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            clear3();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            getdatareceiptbank(txtkeyword3.Text);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView3.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView3.Rows[indx].Cells[0].Value.ToString();
                    getinforeceiptbank(id);

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView3.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView3.Rows[indx].Cells[0].Value.ToString();
                    deletebankaccountfirstcustomer(Convert.ToInt32(id));
                    DataSet dsval = new System.Data.DataSet();
                    string q = "select id,PayableAccountId from CustomerAccount where VoucherNo='" + dataGridView3.Rows[indx].Cells[2].Value.ToString() + "'";
                    dsval = objCore.funGetDataSet(q);
                    if (dsval.Tables[0].Rows.Count > 0)
                    {
                        deleteCustomeraccountfirstbank(Convert.ToInt32(dsval.Tables[0].Rows[0]["id"].ToString()), dsval.Tables[0].Rows[0]["PayableAccountId"].ToString());
                    }
                    
                    getdatareceiptbank("");
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void txtamount4_TextChanged(object sender, EventArgs e)
        {
            if (txtamount4.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtamount4.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    txtamount4.Focus();
                    return;
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtvoucherno.Text == string.Empty)
                {
                    if (cmbcashaccount4.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Cash Account");
                        return;
                    }
                    if (cmbsupplier2.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Supplier");
                        return;
                    }
                    if (txtamount4.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enetr Amount");
                        return;
                    }
                    if (txtamount4.Text == string.Empty)
                    { }
                    else
                    {
                        float Num;
                        bool isNum = float.TryParse(txtamount4.Text.ToString(), out Num); //c is your variable
                        if (isNum)
                        {

                        }
                        else
                        {

                            MessageBox.Show("Invalid Amount. Only Nymbers are allowed");
                            txtamount4.Focus();
                            return;
                        }
                    }
                    if (txtdesc4.Text == string.Empty)
                    {
                        MessageBox.Show("Please Enter Description");
                        return;
                    }
                }

                if (button21.Text == "Add")
                {
                    if (txtvoucherno4.Text == string.Empty)
                    {
                        DataSet dsbarcode = new DataSet();

                        objCore = new classes.Clsdbcon();
                        int idd = 0;
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet("select max(id) as id from BankAccountPaymentSupplier");
                        if (dss.Tables[0].Rows.Count > 0)
                        {
                            string i = dss.Tables[0].Rows[0][0].ToString();
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
                        string q = "select top 1 * from BankAccountPaymentSupplier where ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id desc";
                        DataSet dsacount = new DataSet();
                        string val = "";
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["CurrentBalance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        double balance = Convert.ToDouble(val);

                        double newbalance = (balance - Convert.ToDouble(txtamount4.Text));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into BankAccountPaymentSupplier (id,Date,ChartAccountId,SupplierId,Voucherno,Description,Debit,Credit,CurrentBalance,CheckNo, CheckDate) values('" + idd + "','" + dateTimePicker4.Text + "','" + cmbcashaccount4.SelectedValue + "','" + cmbsupplier2.SelectedValue + "','BPV-" + idd + "','" + txtdesc4.Text.Trim().Replace("'", "''") + "','0','" + txtamount4.Text + "','" + newbalance + "','" + txtchechkno2.Text.Replace("'","''") + "','" + txtcheckdate2.Text.Trim().Replace("'","''") + "')";
                        objCore.executeQuery(q);
                        txtvoucherno4.Text = "BPV-" + idd;
                        supplieraccountBank(txtamount4.Text, "");
                        MessageBox.Show("Data Added Successfully");
                    }
                }
                if (button21.Text == "Update")
                {
                    string q = "select  * from BankAccountPaymentSupplier where ChartAccountId='" + cmbcashaccount4.SelectedValue + "'  order by id";
                    DataSet dsacount = new DataSet();
                    string val = "";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        bool chk = false;
                        for (int i = 0; i < dsacount.Tables[0].Rows.Count; i++)
                        {
                            if (dsacount.Tables[0].Rows[i]["Voucherno"].ToString() == txtvoucherno4.Text)
                            {
                                val = dsacount.Tables[0].Rows[i]["id"].ToString();
                                //if (val == "")
                                //{
                                //    val = "0";

                                //}
                                updatecashaccountfirstbank(val);
                                //return;
                                chk = true;
                            }
                            //if (chk == true)
                            //{
                            //    val = dsacount.Tables[0].Rows[i - 1]["CurrentBalance"].ToString();
                            //    string val1 = dsacount.Tables[0].Rows[i]["Credit"].ToString();
                            //    if (val == "")
                            //    {
                            //        val = "0";

                            //    }
                            //    if (val1 == "")
                            //    {
                            //        val1 = "0";

                            //    }
                            //    updatecashaccountremaining(val, val1, dsacount.Tables[0].Rows[i]["id"].ToString());
                            //}
                        }
                        
                        supplieraccountBank(txtamount4.Text, txtvoucherno4.Text);

                    }


                    
                }
            }
            catch (Exception ex)
            {


            }
            getdatabank("");
        }

        private void button20_Click(object sender, EventArgs e)
        {
            clear4();
        }

        private void button19_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            getdatabank(txtkeyword4.Text);
        }

        private void button23_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView4.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView4.Rows[indx].Cells[0].Value.ToString();
                    getinfobank(id);

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView4.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView4.Rows[indx].Cells[0].Value.ToString();
                    deletecashaccountfirstbank(Convert.ToInt32(id));
                    DataSet dsval = new System.Data.DataSet();
                    string q = "select id,PayableAccountId from SupplierAccount where VoucherNo='" + dataGridView4.Rows[indx].Cells[2].Value.ToString() + "'";
                    dsval = objCore.funGetDataSet(q);
                    if (dsval.Tables[0].Rows.Count > 0)
                    {
                        deletesupplieraccountfirstbank(Convert.ToInt32(dsval.Tables[0].Rows[0]["id"].ToString()), dsval.Tables[0].Rows[0]["PayableAccountId"].ToString());
                    }

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbaccount.Text == "")
                {
                    MessageBox.Show("Please Select Account");
                    return;
                }
                if (txtvoucherno5.Text == "")
                {
                    MessageBox.Show("Please Enter Voucher No");
                    return;
                }
                if (txtdebit.Text == "")
                {
                    MessageBox.Show("Please Enter Debit Amount");
                    return;
                }
                if (txtcredit.Text == "")
                {
                    MessageBox.Show("Please Enter Debit Amount");
                    return;
                }
                if (txtdesc5.Text == "")
                {
                    MessageBox.Show("Please Enter Description");
                    return;
                }
                //foreach (DataGridViewRow dr in dataGridView6.Rows)
                //{
                //    string val = "";
                //    try
                //    {
                //        val = dr.Cells["Voucherno"].Value.ToString();
                //    }
                //    catch (Exception ex)
                //    {


                //    }
                //    if (val == txtvoucherno5.Text)
                //    {
                //        MessageBox.Show("Voucher no already exist");
                //        txtvoucherno5.Focus();
                //        return;
                //    }
                //}
                string vno = "";
                if (dt.Rows.Count > 0)
                {
                    vno = dt.Rows[0]["Voucherno"].ToString();
                    if (vno == txtvoucherno5.Text.Trim())
                    { }
                    else
                    {
                        MessageBox.Show("Voucher no do not match");
                        return;
                    }
                }
                dt.Rows.Add(cmbaccount.SelectedValue, cmbaccount.Text, txtcode.Text, txtvoucherno5.Text, txtdesc5.Text, txtdebit.Text, txtcredit.Text);
                dataGridView6.DataSource = dt;
                dataGridView6.Columns[0].Visible = false;
                //clear5();
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void txtdebit_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                
                
            }
        }

        private void txtcredit_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {


            }
        }

        private void cmbaccount_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet dscode = new System.Data.DataSet();
            string q = "SELECT      AccountCode  FROM         ChartofAccounts where id='" + cmbaccount.SelectedValue + "'";
            dscode = objCore.funGetDataSet(q);
            if (dscode.Tables[0].Rows.Count > 0)
            {
                txtcode.Text = dscode.Tables[0].Rows[0][0].ToString();
            }
        }

        private void cmbaccount_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void button29_Click(object sender, EventArgs e)
        {
            try
            {
                double debit = 0;
                double credit = 0;
                if (dataGridView6.Rows.Count > 0)
                { }
                else
                {
                    return;
                }
                foreach (DataGridViewRow dr in dataGridView6.Rows)
                {
                    string val = "";
                    try
                    {
                        val = dr.Cells["Debit"].Value.ToString();
                    }
                    catch (Exception ex)
                    {


                    }
                    if (val == "")
                    {
                        val = "0";
                    }
                    debit = debit + Convert.ToDouble(val);
                    try
                    {
                        val = dr.Cells["Credit"].Value.ToString();
                    }
                    catch (Exception ex)
                    {


                    }
                    if (val == "")
                    {
                        val = "0";
                    }
                    credit = credit + Convert.ToDouble(val);
                }
                if (credit == debit)
                {
                    if (button29.Text == "Add")
                    {
                        foreach (DataGridViewRow dr in dataGridView6.Rows)
                        {

                            // if (txtvoucherno3.Text == string.Empty)
                            {
                                DataSet dsbarcode = new DataSet();

                                objCore = new classes.Clsdbcon();
                                int idd = 0;
                                DataSet dss = new DataSet();
                                dss = objCore.funGetDataSet("select max(id) as id from JournalAccount");
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    string i = dss.Tables[0].Rows[0][0].ToString();
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
                                string q = "select top 1 * from JournalAccount where PayableAccountId='" + dr.Cells["id"].Value.ToString() + "'  order by id desc";
                                DataSet dsacount = new DataSet();
                                string val = "";
                                dsacount = objCore.funGetDataSet(q);
                                if (dsacount.Tables[0].Rows.Count > 0)
                                {
                                    val = dsacount.Tables[0].Rows[0]["Balance"].ToString();
                                }
                                if (val == "")
                                {
                                    val = "0";

                                }
                                double balance = Convert.ToDouble(val);

                                double newbalance = 0;
                                string debt = dr.Cells["Debit"].Value.ToString();
                                string credt = dr.Cells["Credit"].Value.ToString();
                                if (debt == "")
                                {
                                    debt = "0";
                                }
                                if (credt == "")
                                {
                                    credt = "0";
                                }
                                newbalance = (balance + Convert.ToDouble(debt)) - Convert.ToDouble(credt); ;
                                newbalance = Math.Round(newbalance, 2);


                                q = "insert into JournalAccount (id,Date,PayableAccountId,Voucherno,Description,Debit,Credit,Balance) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["id"].Value.ToString() + "','" + dr.Cells["Voucherno"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString().Replace("'", "''") + "','" + newbalance + "')";
                                objCore.executeQuery(q);
                                //txtvoucherno3.Text = "BRV-" + idd;
                                //Customeraccountbank(txtamount3.Text, "");
                                // MessageBox.Show("Data Added Successfully");
                            }
                        }

                        MessageBox.Show("Data Added Successfully");
                        
                    }
                    if (button29.Text == "Update")
                    {
                        string q = "delete from JournalAccount where voucherno='" + dataGridView6.Rows[0].Cells["voucherno"].Value.ToString() + "'";
                        objCore.executeQuery(q);
                        foreach (DataGridViewRow dr in dataGridView6.Rows)
                        {

                            // if (txtvoucherno3.Text == string.Empty)
                            {
                                DataSet dsbarcode = new DataSet();

                                objCore = new classes.Clsdbcon();
                                int idd = 0;
                                DataSet dss = new DataSet();
                                dss = objCore.funGetDataSet("select max(id) as id from JournalAccount");
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    string i = dss.Tables[0].Rows[0][0].ToString();
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
                                q = "select top 1 * from JournalAccount where PayableAccountId='" + dr.Cells["id"].Value.ToString() + "'  order by id desc";
                                DataSet dsacount = new DataSet();
                                string val = "";
                                dsacount = objCore.funGetDataSet(q);
                                if (dsacount.Tables[0].Rows.Count > 0)
                                {
                                    val = dsacount.Tables[0].Rows[0]["Balance"].ToString();
                                }
                                if (val == "")
                                {
                                    val = "0";

                                }
                                double balance = Convert.ToDouble(val);

                                double newbalance = 0;
                                string debt = dr.Cells["Debit"].Value.ToString();
                                string credt = dr.Cells["Credit"].Value.ToString();
                                if (debt == "")
                                {
                                    debt = "0";
                                }
                                if (credt == "")
                                {
                                    credt = "0";
                                }
                                newbalance = (balance + Convert.ToDouble(debt)) - Convert.ToDouble(credt); ;
                                newbalance = Math.Round(newbalance, 2);


                                q = "insert into JournalAccount (id,Date,PayableAccountId,Voucherno,Description,Debit,Credit,Balance) values('" + idd + "','" + dateTimePicker5.Text.Replace("'", "''") + "','" + dr.Cells["id"].Value.ToString() + "','" + dr.Cells["Voucherno"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Description"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Debit"].Value.ToString().Replace("'", "''") + "','" + dr.Cells["Credit"].Value.ToString() + "','" + newbalance + "')";
                                objCore.executeQuery(q);
                                //txtvoucherno3.Text = "BRV-" + idd;
                                //Customeraccountbank(txtamount3.Text, "");
                                // MessageBox.Show("Data Added Successfully");
                            }
                        }
                        chartacounts();
                        MessageBox.Show("Data Updated Successfully");

                    }
                    getdatajournal("");

                }
                else
                {
                    MessageBox.Show("Credit and Debit is not equal");

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            getdatajournal(txtkeyword5.Text);
        }

        private void dataGridView6_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (button29.Text == "Update")
            {
                getinforejournaledit("");
            }
            //else
            {
                DataRow dtr = dt.Rows[e.RowIndex];

                dtr.Delete();
                dataGridView6.Refresh();
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[2].Value.ToString();
                    POSRetail.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.name = "Cash Payment Voucher";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView4.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView4.Rows[indx].Cells[2].Value.ToString();
                    POSRetail.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.name = "Bank Payment Voucher";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView2.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView2.Rows[indx].Cells[2].Value.ToString();
                    POSRetail.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.name = "Cash Receipt Voucher";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView3.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView3.Rows[indx].Cells[2].Value.ToString();
                    POSRetail.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.name = "Bank Receipt Voucher";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView5.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView5.Rows[indx].Cells[3].Value.ToString();
                    POSRetail.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = id;
                    obj.name = "Journal Voucher";
                    obj.Show();

                }
            }
            catch (Exception ex)
            {


            }
        }
        public void getinfojournal(string voucher)
        {
            try
            {
                
                dt.Clear();
                DataSet dsgetjournal = new System.Data.DataSet();
                string q = "SELECT     dbo.JournalAccount.PayableAccountId,dbo.JournalAccount.id, dbo.JournalAccount.Date, dbo.ChartofAccounts.Name, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description , dbo.ChartofAccounts.AccountCode, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit,                       dbo.JournalAccount.Balance FROM         dbo.JournalAccount INNER JOIN                      dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.VoucherNo='" + voucher + "'";
                dsgetjournal = objCore.funGetDataSet(q);
                for (int i = 0; i < dsgetjournal.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(dsgetjournal.Tables[0].Rows[i]["PayableAccountId"].ToString(), dsgetjournal.Tables[0].Rows[i]["Name"].ToString(), dsgetjournal.Tables[0].Rows[i]["AccountCode"].ToString(), dsgetjournal.Tables[0].Rows[i]["VoucherNo"].ToString(), dsgetjournal.Tables[0].Rows[i]["Description"].ToString(), dsgetjournal.Tables[0].Rows[i]["Debit"].ToString(), dsgetjournal.Tables[0].Rows[i]["Credit"].ToString());

                }
                dataGridView6.DataSource = dt;
                dataGridView6.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                
               
            }
        }
        private void button26_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView5.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView5.Rows[indx].Cells[3].Value.ToString();
                    getinfojournal(id);
                    button29.Text = "Update";

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            clear5();
            
        }

        private void button36_Click(object sender, EventArgs e)
        {
            dt.Clear();
            dataGridView6.DataSource = dt;
            button29.Text = "Add";
        }
        public void deletejurnalcharts(string voucher)
        {
            try
            {
                DataSet dscharts = new System.Data.DataSet();
                string q = "select * from ChartofAccounts";
                dscharts = objCore.funGetDataSet(q);
                for (int i = 0; i < dscharts.Tables[0].Rows.Count; i++)
                {
                    updatejournal(dscharts.Tables[0].Rows[i]["id"].ToString(), voucher);
                }
            }
            catch (Exception ex)
            {
                
              
            }

        }
        public void updatejournal(string id, string voucher)
        {
            try
            {
                string q = "select * from JournalAccount where PayableAccountId ='" + id + "' order by id asc";
                DataSet dss = new System.Data.DataSet();
                dss = objCore.funGetDataSet(q);
                string blnce = "0";
                string debit = "", credit = "";
                double newbalance = 0;
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {
                    if (dss.Tables[0].Rows[i]["voucherno"].ToString()==voucher)
                    {
                        if (i == 0)
                        {
                            debit = "0";
                            credit = "0";
                            blnce = "0";
                            if (debit == "")
                            {
                                debit = "0";
                            }
                            if (credit == "")
                            {
                                credit = "0";
                            }
                            if (blnce == "")
                            {
                                blnce = "0";
                            }

                            newbalance = Convert.ToDouble(blnce);
                        }
                        else
                        {
                            debit = "0";
                            credit = "0";
                            blnce = dss.Tables[0].Rows[i - 1]["Balance"].ToString();
                            if (debit == "")
                            {
                                debit = "0";
                            }
                            if (credit == "")
                            {
                                credit = "0";
                            }
                            if (blnce == "")
                            {
                                blnce = "0";
                            }
                            newbalance = (Convert.ToDouble(blnce) + Convert.ToDouble(debit)) - Convert.ToDouble(credit);

                        }
                        newbalance = Math.Round(newbalance, 2);
                        objCore = new classes.Clsdbcon();
                        q = "update JournalAccount set     Debit='" + debit + "' ,credit='" + credit + "' , Balance='" + newbalance + "' where id='" + dss.Tables[0].Rows[i]["id"].ToString() + "'";
                        objCore.executeQuery(q); 
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("delete journal  error");
            }
        }
        private void button25_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView5.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string voucher = dataGridView5.Rows[indx].Cells[3].Value.ToString();

                    deletejurnalcharts(voucher);
                    chartacounts();
                    getdatajournal("");

                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
