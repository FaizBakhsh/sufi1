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
    public partial class VoucherApproval : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public VoucherApproval()
        {
            InitializeComponent();
        }

        private void VoucherApproval_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch order by BranchName";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];

                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "BranchName";



            }
            catch (Exception ex)
            {


            }
        }
        protected string payingto(string voucherno, string account)
        {
            string name = "";
            if (account == "JV")
            {
                string qr = "SELECT        dbo.JournalAccount.Id, dbo.JournalAccount.Date, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit, dbo.ChartofAccounts.Name,                          dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.AccountType FROM            dbo.JournalAccount INNER JOIN                         dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.Debit>0  and (dbo.JournalAccount.VoucherNo = '" + voucherno + "')  order by  Date,VoucherNo";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(qr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                }
            }

            return name;
        }
        public class Voucherapproval
        {
            public string Name { get; set; }
            public string Date { get; set; }
            public string Amount { get; set; }
            public string Voucherno { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
        }
        public string getsupplier(string id)
        {
            string sup = "";
            try
            {
                DataSet dssuplier = new DataSet();
                string q = "select name from Supplier where id='" + id + "'";
                dssuplier = objcore.funGetDataSet(q);
                if (dssuplier.Tables[0].Rows.Count > 0)
                {
                    sup = dssuplier.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return sup;
        }
        public string getemployee(string id)
        {
            string sup = "";
            try
            {
                DataSet dssuplier = new DataSet();
                string q = "select name from Employees where id='" + id + "'";
                dssuplier = objcore.funGetDataSet(q);
                if (dssuplier.Tables[0].Rows.Count > 0)
                {
                    sup = dssuplier.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return sup;
        }
        protected void getdata()
        {
            List<Voucherapproval> voucherapprovallist = new List<Voucherapproval>();
            DataTable dtdata = new DataTable();
            dtdata.Columns.Add("Name", typeof(string));
            dtdata.Columns.Add("Date", typeof(string));
            dtdata.Columns.Add("Amount", typeof(string));
            dtdata.Columns.Add("Voucherno", typeof(string));
            dtdata.Columns.Add("Description", typeof(string));
            
            string qr = "";

            try
            {
                qr = "SELECT        dbo.JournalAccount.Status,dbo.JournalAccount.Id, dbo.JournalAccount.Date, dbo.JournalAccount.VoucherNo, dbo.JournalAccount.Description, dbo.JournalAccount.Debit, dbo.JournalAccount.Credit, dbo.ChartofAccounts.Name,                          dbo.ChartofAccounts.AccountCode, dbo.ChartofAccounts.AccountType FROM            dbo.JournalAccount INNER JOIN                         dbo.ChartofAccounts ON dbo.JournalAccount.PayableAccountId = dbo.ChartofAccounts.Id where dbo.JournalAccount.branchId='" + comboBox1.SelectedValue + "' and dbo.JournalAccount.Credit>0 and dbo.ChartofAccounts.AccountType like '%Assets%' and (dbo.JournalAccount.Date = '" + dateTimePicker1.Text + "')  order by  dbo.JournalAccount.Date,dbo.JournalAccount.VoucherNo";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(qr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    voucherapprovallist.Add(new Voucherapproval { Name = ds.Tables[0].Rows[i]["Name"].ToString(), Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), Amount = ds.Tables[0].Rows[i]["Credit"].ToString(), Voucherno = ds.Tables[0].Rows[i]["VoucherNo"].ToString(), Description = ds.Tables[0].Rows[i]["Description"].ToString(), Status = ds.Tables[0].Rows[i]["Status"].ToString() });
                    //dtdata.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), ds.Tables[0].Rows[i]["Credit"].ToString(), ds.Tables[0].Rows[i]["VoucherNo"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString());
                }
                qr = "SELECT    Status, SupplierId, Id, Date, Voucherno, CheckNo, CheckDate, Description, Debit, Credit FROM         BankAccountPaymentSupplier where  branchId='" + comboBox1.SelectedValue + "' and status='Pending' and (Date = '" + dateTimePicker1.Text + "')  order by  Date,VoucherNo";
                ds = new DataSet();
                ds = objcore.funGetDataSet(qr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = "";
                   name= getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                    //dtdata.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), ds.Tables[0].Rows[i]["Credit"].ToString(), ds.Tables[0].Rows[i]["VoucherNo"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString());
                    voucherapprovallist.Add(new Voucherapproval { Name = name, Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), Amount = ds.Tables[0].Rows[i]["Credit"].ToString(), Voucherno = ds.Tables[0].Rows[i]["VoucherNo"].ToString(), Description = ds.Tables[0].Rows[i]["Description"].ToString(), Status = ds.Tables[0].Rows[i]["Status"].ToString() });
                  
                }
                qr = "SELECT     Status, SupplierId,Id, Date, Voucherno, Description, Debit, Credit FROM         CashAccountPaymentSupplier where branchId='" + comboBox1.SelectedValue + "' and   (Date = '" + dateTimePicker1.Text + "')  order by  Date,VoucherNo";
                ds = new DataSet();
                ds = objcore.funGetDataSet(qr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = "";
                    name = getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                    //dtdata.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), ds.Tables[0].Rows[i]["Credit"].ToString(), ds.Tables[0].Rows[i]["VoucherNo"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString());
                    voucherapprovallist.Add(new Voucherapproval { Name = name, Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), Amount = ds.Tables[0].Rows[i]["Credit"].ToString(), Voucherno = ds.Tables[0].Rows[i]["VoucherNo"].ToString(), Description = ds.Tables[0].Rows[i]["Description"].ToString(), Status = ds.Tables[0].Rows[i]["Status"].ToString() });
                  
                }

                qr = "SELECT    EmployeeId,Status, Id, Date, Voucherno, Description, Debit, Credit FROM         EmployeesAccount where branchId='" + comboBox1.SelectedValue + "' and  debit>0 and (Date = '" + dateTimePicker1.Text + "')  order by  Date,VoucherNo";
                ds = new DataSet();
                ds = objcore.funGetDataSet(qr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = "";
                    name = getemployee(ds.Tables[0].Rows[i]["EmployeeId"].ToString());
                    //dtdata.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), ds.Tables[0].Rows[i]["Credit"].ToString(), ds.Tables[0].Rows[i]["VoucherNo"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString());
                    int count = 0;
                    try
                    {
                        count = voucherapprovallist.Where(x => x.Voucherno == ds.Tables[0].Rows[i]["VoucherNo"].ToString()).ToList().Count;
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    if (count == 0)
                    {
                        voucherapprovallist.Add(new Voucherapproval { Name = name, Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), Amount = ds.Tables[0].Rows[i]["Debit"].ToString(), Voucherno = ds.Tables[0].Rows[i]["VoucherNo"].ToString(), Description = ds.Tables[0].Rows[i]["Description"].ToString(), Status = ds.Tables[0].Rows[i]["Status"].ToString() });
                    }
                }

                qr = "SELECT   SupplierId, Status, Id, Date, Voucherno, Description, Debit, Credit FROM         SupplierAccount where branchId='" + comboBox1.SelectedValue + "' and debit>0  and (Date = '" + dateTimePicker1.Text + "')  order by  Date,VoucherNo";
                ds = new DataSet();
                ds = objcore.funGetDataSet(qr);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string name = "";
                    name = getsupplier(ds.Tables[0].Rows[i]["SupplierId"].ToString());
                    //dtdata.Rows.Add(Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), ds.Tables[0].Rows[i]["Credit"].ToString(), ds.Tables[0].Rows[i]["VoucherNo"].ToString(), ds.Tables[0].Rows[i]["Description"].ToString());
                    int count = 0;
                    try
                    {
                        count = voucherapprovallist.Where(x => x.Voucherno == ds.Tables[0].Rows[i]["VoucherNo"].ToString()).ToList().Count;
                    }
                    catch (Exception ex)
                    {

                    }
                    if (count == 0)
                    {
                        voucherapprovallist.Add(new Voucherapproval { Name = name, Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy"), Amount = ds.Tables[0].Rows[i]["Debit"].ToString(), Voucherno = ds.Tables[0].Rows[i]["VoucherNo"].ToString(), Description = ds.Tables[0].Rows[i]["Description"].ToString(), Status = ds.Tables[0].Rows[i]["Status"].ToString() });
                    }
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            dataGridView1.DataSource = voucherapprovallist;
            //dataGridView1.Columns[1].Visible = false;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells["Status"].Value.ToString() == "Approved")
                {
                    dr.DefaultCellStyle.BackColor = Color.Green;
                    dr.DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 2)
                {
                    DialogResult dr = MessageBox.Show("Are you sure to Approve", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string voucherno = dataGridView1.Rows[e.RowIndex].Cells["voucherno"].Value.ToString();
                        string q = "Update SupplierAccount set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "' , status='Approved' where voucherno='" + voucherno + "'";
                        objcore.executeQuery(q);
                        q = "Update EmployeesAccount set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "' , status='Approved' where voucherno='" + voucherno + "'";
                        objcore.executeQuery(q);
                        q = "Update CashAccountPaymentSupplier set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "' ,status='Approved' where voucherno='" + voucherno + "'";
                        objcore.executeQuery(q);
                        q = "Update BankAccountPaymentSupplier set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "' ,status='Approved' where voucherno='" + voucherno + "'";
                        objcore.executeQuery(q);
                        q = "Update JournalAccount set Approveduserid='" + POSRestaurant.Properties.Settings.Default.UserId + "' ,status='Approved' where voucherno='" + voucherno + "'";
                        objcore.executeQuery(q);
                        getdata();
                    }
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                if (e.ColumnIndex == 0)
                {
                    string voucherno = dataGridView1.Rows[e.RowIndex].Cells["voucherno"].Value.ToString();
                    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
                    obj.id = voucherno;
                    obj.branch = comboBox1.SelectedValue.ToString();
                    if (voucherno.ToLower().Contains("jv"))
                    {
                        obj.name = "Journal Voucher";
                        obj.type = "jv";
                    }
                    if (voucherno.ToLower().Contains("cpv"))
                    {
                        obj.name = "Cash Payment Voucher";
                        obj.type = "cpv";
                    }
                    if (voucherno.ToLower().Contains("bpv"))
                    {
                        obj.name = "Bank Payment Voucher";
                        obj.type = "bpv";
                    }
                    obj.Show();
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                if (e.ColumnIndex == 1)
                {
                    string voucherno = dataGridView1.Rows[e.RowIndex].Cells["voucherno"].Value.ToString();
                    POSRestaurant.Accounts.Supportingpreview obj = new Supportingpreview();
                    obj.voucherno = voucherno;
                   
                    if (voucherno.ToLower().Contains("jv"))
                    {

                        obj.table = "JournalAccount";
                    }
                    if (voucherno.ToLower().Contains("cpv"))
                    {

                        obj.table = "CashAccountPaymentSupplier";
                    }
                    if (voucherno.ToLower().Contains("bpv"))
                    {

                        obj.table = "BankAccountPaymentSupplier";
                    }
                    obj.ShowDialog();
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {

        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
