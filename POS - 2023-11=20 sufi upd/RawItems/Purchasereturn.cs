using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using VIBlend.WinForms.DataGridView.Filters;
using VIBlend.Utilities;
using System.Net;
using Newtonsoft.Json;
using System.Data.SqlClient;
namespace POSRestaurant.RawItems
{
    public partial class Purchasereturn : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        Purchase _frm;
        public Purchasereturn()
        {
            InitializeComponent();
            
        }

        private void PurChase_List_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from branch ";
                ds = objCore.funGetDataSet(q);              
                cmbbranch.DataSource = ds.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchName";
              
            }
            catch (Exception ex)
            {
               
            }
            try
            {
                string q = "select * from supplier";
                ds = objCore.funGetDataSet(q);
               
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "Name";
                
            }
            catch (Exception ex)
            {

               
            }
        }
        protected void getinvoices()
        {
            try
            {
                string q = "";
                ds = new DataSet();


                q = "select id,cast(id as varchar(100))+'-'+InvoiceNo as invoiceno from purchase where  SupplierId='" + cmbsupplier.SelectedValue + "' and BranchCode='"+cmbbranch.SelectedValue+"' order by date desc";
                ds = objCore.funGetDataSet(q);

                cmbinvoiceno.DataSource = ds.Tables[0];
                cmbinvoiceno.ValueMember = "id";
                cmbinvoiceno.DisplayMember = "InvoiceNo";


            }
            catch (Exception ex)
            {

                
            }

        }
        protected void getdata()
        {
            try
            {
                string q = "";
                ds = new DataSet();


                q = "SELECT         dbo.PurchaseDetails.Id,dbo.Purchase.Date,dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.PricePerItem, dbo.PurchaseDetails.TotalAmount, dbo.PurchasereturnDetails.TotalItems AS ReturnQuantity,dbo.PurchaseDetails.RawItemId FROM            dbo.Purchase INNER JOIN                         dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                         dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id LEFT OUTER JOIN                         dbo.PurchasereturnDetails ON dbo.PurchaseDetails.Id = dbo.PurchasereturnDetails.PDID where purchase.id='" + cmbinvoiceno.SelectedValue + "'";
                ds = objCore.funGetDataSet(q);

                dataGridView1.DataSource = ds.Tables[0];
               
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = true;
                dataGridView1.Columns[2].ReadOnly = true;
                dataGridView1.Columns[3].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = true;
                dataGridView1.Columns[5].ReadOnly = true;
                dataGridView1.Columns[6].ReadOnly = true;
                dataGridView1.Columns[7].ReadOnly = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {

               
            }
            
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='demand'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            if (tp == "")
            {
                tp = "sql";
            }
            return tp;
        }
        string url = "";
    

        private void cmbsupplier_SelectedIndexChanged(object sender, EventArgs e)
        {
            getinvoices();
        }

        private void cmbinvoiceno_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }
        string invoiceno = "";
        public string inventoryaccount(string itmid, string amount, string invoice,string grn)
        {
            string q = "";
            try
            {
                DataSet dsacount = new DataSet();
               

                {
                    string ChartAccountId = "";
                    q = "select * from CashSalesAccountsList where AccountType='Inventory Account' and branchid='" + cmbbranch.SelectedValue + "'";
                    DataSet dsacountchk = new DataSet();
                    dsacountchk = objCore.funGetDataSet(q);
                    if (dsacountchk.Tables[0].Rows.Count > 0)
                    {
                        ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    string val = "";

                    double prc = Convert.ToDouble(amount);

                    dsacount = new DataSet();

                    {

                        int iddd = 0;
                        DataSet ds = objCore.funGetDataSet("select max(id) as id from InventoryAccount");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string i = ds.Tables[0].Rows[0][0].ToString();
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
                        val = "";
                        q = "delete from InventoryAccount where voucherno='GRN-R-" + cmbinvoiceno.SelectedValue + "-" + cmbbranch.SelectedValue + "-" + grn + "'";
                        objCore.executeQuery(q);
                        q = "insert into InventoryAccount (Id,Date,ItemId,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + dateTimePicker1.Text + "','" + itmid + "','" + ChartAccountId + "','GRN-R-" + cmbinvoiceno.SelectedValue + "-" + cmbbranch.SelectedValue +"-"+grn +"','Purchase Return against Invoice No " + invoice + "','0','" + Math.Round(Convert.ToDouble(prc), 2) + "','0','" + cmbbranch.SelectedValue + "')";
                       // objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return q;
        }
        public string supplieraccount(string amount, string invoiceno,string grn)
        {
            string q = "";
            try
            {

                DataSet dsacount = new DataSet();
                q = "select payableaccountid from Supplier where id='" + cmbsupplier.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
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
                    q = "delete from SupplierAccount where voucherno='GRN-R-" + cmbinvoiceno.SelectedValue + "-" + cmbbranch.SelectedValue + "-" + grn + "'";
                    objCore.executeQuery(q);
                    q = "insert into SupplierAccount (invoiceno,Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + cmbinvoiceno.Text + "','" + iddd + "','" + dateTimePicker1.Text + "','" + cmbsupplier.SelectedValue + "','" + PayableAccountId + "','GRN-R-" + cmbinvoiceno.SelectedValue + "-" + cmbbranch.SelectedValue +"-"+grn+ "','Purchase against Invoice No " + invoiceno + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','0','" + cmbbranch.SelectedValue + "')";
                   // objCore.executeQuery(q);
                    
                }
            }
            catch (Exception ex)
            {


            }
            return q;
        }
        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 7)
            {
                string temp = dataGridView1.Rows[e.RowIndex].Cells["TotalItems"].Value.ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                double total = Convert.ToDouble(temp);
                temp = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                double returnitem = Convert.ToDouble(temp);
                if (returnitem > total)
                {
                    MessageBox.Show("Can not return more than " + total + " items");
                    return;
                }

                double totalamount = Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["ReturnQuantity"].Value.ToString()) * Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["PricePerItem"].Value.ToString());
                string q = "select * from PurchasereturnDetails where pdid='"+dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString()+"'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    q = "update PurchasereturnDetails set PurchaseId='" + cmbinvoiceno.SelectedValue + "',totalamount='" + totalamount + "', TotalItems='" + dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString() + "', RawItemId='" + dataGridView1.Rows[e.RowIndex].Cells["RawItemId"].Value.ToString() + "' where id='" + ds.Tables[0].Rows[0]["id"].ToString() + "'";
                    objCore.executeQuery(q);
                }
                else
                {
                    ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from PurchasereturnDetails");
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
                    q = "insert into PurchasereturnDetails (PurchaseId,RawItemId,id,TotalItems,PricePerItem,TotalAmount,pdid,date) values('" + cmbinvoiceno.SelectedValue + "','" + dataGridView1.Rows[e.RowIndex].Cells["RawItemId"].Value.ToString() + "','" + id + "','" + dataGridView1.Rows[e.RowIndex].Cells["ReturnQuantity"].Value.ToString() + "','" + dataGridView1.Rows[e.RowIndex].Cells["PricePerItem"].Value.ToString() + "','" + totalamount + "','" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "','" + dateTimePicker1.Text + "')";
                    objCore.executeQuery(q);
                }
                q = "select * from purchase where id='" + cmbinvoiceno.SelectedValue + "'";
                DataSet dss = new DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    string q1 = inventoryaccount(dataGridView1.Rows[e.RowIndex].Cells["RawItemId"].Value.ToString(), totalamount.ToString(), dss.Tables[0].Rows[0]["InvoiceNo"].ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
                    string q2 = supplieraccount(totalamount.ToString(), dss.Tables[0].Rows[0]["InvoiceNo"].ToString(), dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());

                    ExecuteSqlTransaction("", q1, q2, "Data Added Successfully");
                }

            }
        }
        private static void ExecuteSqlTransaction(string connectionString, string q1, string q2, string message)
        {
            connectionString = POSRestaurant.Properties.Settings.Default.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;

                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");
                command.Connection = connection;
                command.Transaction = transaction;

                try
                {
                    command.CommandText = q1;
                    command.ExecuteNonQuery();
                    command.CommandText = q2;
                    command.ExecuteNonQuery();

                    // Attempt to commit the transaction.
                    transaction.Commit();
                  //  MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("  Message: {0}" + ex.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Rollback Exception Type: {0}" + ex2.Message);
                    }
                }
            }
        }
        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            getinvoices();
        }
    }
}
