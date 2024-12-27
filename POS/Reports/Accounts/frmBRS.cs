using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
namespace POSRestaurant.Reports.Accounts
{
    public partial class frmBRS : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmBRS()
        {
            InitializeComponent();
        }
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Supplier  order by name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All Suppliers";
                ds.Tables[0].Rows.Add(dr);
                cmbaccount.DataSource = ds.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "name";
                cmbaccount.Text = "All Suppliers";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    
        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
            
            try
            {
                DataSet ds = new System.Data.DataSet();
                string q = "select id,name from ChartofAccounts where name like '%bank%' and accounttype='Current Assets'";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);
                
                cmbaccount.DataSource = ds.Tables[0];
                cmbaccount.ValueMember = "id";
                cmbaccount.DisplayMember = "name";
              
            }
            catch (Exception ex)
            {


            }
            
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport()
        {

            try
            {

                DataTable dt = new DataTable();


                POSRestaurant.Reports.Accounts.rptBRS rptDoc = new rptBRS();
                POSRestaurant.Reports.Accounts.dsbrd dsrpt = new dsbrd();
                
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
               
                getcompany();
                string company = "", phone = "", address = "", logo = "";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                }
                catch (Exception ex)
                {


                }
                
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("bank", cmbaccount.Text);
                string enddate = "";
                try
                {
                    enddate = Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM dd, yyyy");
                }
                catch (Exception ex)
                {


                }
                rptDoc.SetParameterValue("date", "As at " + enddate);

               
              
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {

                dtrpt.Columns.Add("Head", typeof(string));
                dtrpt.Columns.Add("Particulars", typeof(string));
                dtrpt.Columns.Add("ChequeNO", typeof(string));
                dtrpt.Columns.Add("ChequeDate", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("Balance", typeof(double));
                dtrpt.Columns.Add("total", typeof(double));
                dtrpt.Columns.Add("logo", typeof(Byte[]));

                DataSet ds = new DataSet();
                string q = "";
                double total = 0,bankbalance=0;
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                double bf = 0;
                q = "";
                DataSet dscs = new System.Data.DataSet();
                string enddate = "";
                try
                {
                    enddate = Convert.ToDateTime(dateTimePicker2.Text).ToString("MMM dd, yyyy");
                }
                catch (Exception ex)
                {


                }
                
                bf = getBF(cmbaccount.SelectedValue.ToString());

                dtrpt.Rows.Add("1. Balance as per ledger as on " + enddate, "", "", "", 0, bf, 0, dscompany.Tables[0].Rows[0]["logo"]);
                total = total + bf;
                q = "select credit,CheckDate, Description,CheckNo   from BankAccountPaymentSupplier where date <= '" + dateTimePicker2.Text + "' and ChartAccountId='" + cmbaccount.SelectedValue + "' and ClearanceStatus='Pending'";

                dscs = objCore.funGetDataSet(q);
                for (int i = 0; i < dscs.Tables[0].Rows.Count; i++)
                {
                    string chkdate = dscs.Tables[0].Rows[i]["CheckDate"].ToString();
                    try
                    {
                        chkdate = Convert.ToDateTime(chkdate).ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    double credit=Convert.ToDouble(dscs.Tables[0].Rows[i]["credit"].ToString());
                    total = total + credit;
                    //if (i == 0)
                    //{
                    //    credit = credit + bf;
                    //}
                    dtrpt.Rows.Add("2. ADD: Unpresented cheque/Error: ", dscs.Tables[0].Rows[i]["Description"].ToString(), dscs.Tables[0].Rows[i]["CheckNo"].ToString(), chkdate, dscs.Tables[0].Rows[i]["credit"].ToString(), 0, credit, dscompany.Tables[0].Rows[0]["logo"]);
                }

                q = "select  AccountId, Date, Debit, Credit, Description   from BankReconciliation where Debit>0 and  date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and AccountId='" + cmbaccount.SelectedValue + "' ";
                dscs = new System.Data.DataSet();
                dscs = objCore.funGetDataSet(q);
                for (int i = 0; i < dscs.Tables[0].Rows.Count; i++)
                {
                    if (dscs.Tables[0].Rows[i]["Description"].ToString() == "Balance")
                    {
                        //bankbalance = Convert.ToDouble(dscs.Tables[0].Rows[i]["Debit"].ToString());
                    }
                    else
                    {
                        total = total + Convert.ToDouble(dscs.Tables[0].Rows[i]["Debit"].ToString());
                        dtrpt.Rows.Add("2. ADD: Unpresented cheque/Error: ", dscs.Tables[0].Rows[i]["Description"].ToString(), "", "", dscs.Tables[0].Rows[i]["Debit"].ToString(), 0, dscs.Tables[0].Rows[i]["Debit"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }

              double  totalcredit = 0;

              q = "SELECT        dbo.BankAccountReceiptCustomer.debit,dbo.BankAccountReceiptCustomer.CheckDate, dbo.BankAccountReceiptCustomer.Description,dbo.BankAccountReceiptCustomer.CheckNo, dbo.Customers.Name FROM            dbo.BankAccountReceiptCustomer INNER JOIN                         dbo.Customers ON dbo.BankAccountReceiptCustomer.CustomerId = dbo.Customers.Id  where BankAccountReceiptCustomer.date <= '" + dateTimePicker2.Text + "' and BankAccountReceiptCustomer.ChartAccountId='" + cmbaccount.SelectedValue + "' and BankAccountReceiptCustomer.ClearanceStatus='Pending'";
                dscs = objCore.funGetDataSet(q);
                for (int i = 0; i < dscs.Tables[0].Rows.Count; i++)
                {
                    string chkdate = dscs.Tables[0].Rows[i]["CheckDate"].ToString();
                    try
                    {
                        chkdate = Convert.ToDateTime(chkdate).ToString("yyyy-MM-dd");
                    }
                    catch (Exception ex)
                    {


                    }
                    totalcredit = totalcredit + Convert.ToDouble(dscs.Tables[0].Rows[i]["debit"].ToString());
                    dtrpt.Rows.Add("3. LESS: ", "Uncredited payment by bank,Customer " + dscs.Tables[0].Rows[i]["Name"].ToString(), dscs.Tables[0].Rows[i]["CheckNo"].ToString(), chkdate, dscs.Tables[0].Rows[i]["debit"].ToString(), 0, dscs.Tables[0].Rows[i]["debit"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                }

                q = "select  AccountId, Date, Debit, Credit, Description   from BankReconciliation where credit>0 and  date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and AccountId='" + cmbaccount.SelectedValue + "' ";
                dscs = new System.Data.DataSet();
                dscs = objCore.funGetDataSet(q);
                for (int i = 0; i < dscs.Tables[0].Rows.Count; i++)
                {
                    if (dscs.Tables[0].Rows[i]["Description"].ToString() == "Balance")
                    {
                        //bankbalance = Convert.ToDouble(dscs.Tables[0].Rows[i]["Debit"].ToString());
                    }
                    else
                    {
                        totalcredit = totalcredit + Convert.ToDouble(dscs.Tables[0].Rows[i]["credit"].ToString());
                        dtrpt.Rows.Add("3. LESS: ", dscs.Tables[0].Rows[i]["Description"].ToString(), "", "", dscs.Tables[0].Rows[i]["credit"].ToString(), 0, dscs.Tables[0].Rows[i]["credit"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }
                try
                {
                    q = "select  AccountId, Date, balance, Credit, Description   from BankReconciliation where balance>0 and  date =  '" + dateTimePicker2.Text + "' and AccountId='" + cmbaccount.SelectedValue + "' and Description='Balance'";
                    dscs = new System.Data.DataSet();
                    dscs = objCore.funGetDataSet(q);
                    for (int i = 0; i < dscs.Tables[0].Rows.Count; i++)
                    {
                        bankbalance = Convert.ToDouble(dscs.Tables[0].Rows[i]["balance"].ToString());

                    }
                }
                catch (Exception ex)
                {
                    
                }
                dtrpt.Rows.Add("4. Adjusted balance as per ledger as on " + enddate+" (A)", "", "", "", 0, total-totalcredit, 0, dscompany.Tables[0].Rows[0]["logo"]);
                dtrpt.Rows.Add("5. Balance as per bank statement as on " + enddate + " (B)", "", "", "", 0, bankbalance, 0, dscompany.Tables[0].Rows[0]["logo"]);

                dtrpt.Rows.Add("6. Difference (C=A-B) " + enddate + " (B)", "", "", "", 0, (total - totalcredit) - bankbalance, 0, dscompany.Tables[0].Rows[0]["logo"]);

            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        public double getBF(string id)
        {
           double bf = 0;
            string q = "";
            //q = "select  AccountType from ChartofAccounts where id='" + cmbaccount.SelectedValue + "'";
            DataSet ds = new DataSet();
            //ds = objCore.funGetDataSet(q);
            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    string type = ds.Tables[0].Rows[0][0].ToString();
            //    if (type == "Cost of Sales" || type == "Revenue" || type == "Operating Expenses" || type == "Admin and Selling Expenses" || type == "Financial Expenses" || type == "Marketing Expenses")
            //    {
            //       // return;
            //    }
            //}
            try
            {

                string blnce = "", val = "0";
                double balancebf = 0, debitbf = 0, creditbf = 0;


                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalariesAccount where ChartAccountId='" + id + "' and date <= '" + dateTimePicker2.Text + "' ";
               

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);



                }

                ds.Dispose();
                ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountPaymentSupplier where ChartAccountId='" + id + "'  and date <= '" + dateTimePicker2.Text + "' ";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);



                }

                ds.Dispose();
                ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BankAccountReceiptCustomer where ChartAccountId='" + id + "'  and  date <= '" + dateTimePicker2.Text + "'";
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPaymentSupplier where ChartAccountId='" + id + "'  and  date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountPurchase where ChartAccountId='" + id + "'   and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountReceiptCustomer where ChartAccountId='" + id + "'  and  date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }
                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CashAccountSales where ChartAccountId='" + id + "' and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CostSalesAccount where ChartAccountId='" + id + "' and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose();
                ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         CustomerAccount where PayableAccountId='" + id + "'  and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose();
                ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         BranchAccount where PayableAccountId='" + id + "'  and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose();
                ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         DiscountAccount where ChartAccountId='" + id + "'  and date <= '" + dateTimePicker2.Text + "'";
               

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         GSTAccount where ChartAccountId='" + id + "' and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         InventoryAccount where ChartAccountId='" + id + "' and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         JournalAccount where PayableAccountId='" + id + "'  and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + id + "'  and date <= '" + dateTimePicker2.Text + "'";
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();

                q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SupplierAccount where PayableAccountId='" + id + "'  and date <= '" + dateTimePicker2.Text + "'";

                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    debitbf = debitbf + Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    creditbf = creditbf + Convert.ToDouble(val);
                }

                ds.Dispose(); ds = new DataSet();
                // q = "SELECT    sum( Debit) as Debit,sum( Credit) as Credit FROM         SalesAccount where ChartAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
                //q = "SELECT     Id, Date, PayableAccountId, VoucherNo, Description, Debit, Credit, Balance, branchid FROM         PettyCash where PayableAccountId='" + id + "' and branchid='" + cmbbranchjv.SelectedValue + "' and date <'" + dateTimePicker1.Text + "'";
                //ds = objCore.funGetDataSet(q);
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    val = ds.Tables[0].Rows[i]["Debit"].ToString();
                //    if (val == "")
                //    {
                //        val = "0";
                //    }
                //    debitbf = debitbf + Convert.ToDouble(val);

                //    val = ds.Tables[0].Rows[i]["Credit"].ToString();
                //    if (val == "")
                //    {
                //        val = "0";
                //    }
                //    creditbf = creditbf + Convert.ToDouble(val);
                //}
                bf = debitbf - creditbf;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return bf;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (cmbaccount.Text == "")
            {
                MessageBox.Show("Please Select Supplier");
                return;
            }

            System.GC.Collect();
            button1.Text = "Please Wait(Downloading Data)";
            button1.Enabled = false;
            bindreport();
            button1.Text = "Submit";
            button1.Enabled = true;
        }

        private void cmbbranchjv_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill();
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;

            //string name = info.Text;
            //if (name.Contains("CPV"))
            //{
            //    string id = name;
            //    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
            //    obj.id = id;
            //    obj.branch = cmbbranchjv.SelectedValue.ToString();
            //    obj.name = "Cash Payment Voucher";
            //    obj.type = "cpv";
            //    obj.Show();
            //}
            //if (name.Contains("BPV"))
            //{
            //    string id = name;
            //    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
            //    obj.id = id;
            //    obj.branch = cmbbranchjv.SelectedValue.ToString();
            //    obj.name = "Bank Payment Voucher";
            //    obj.type = "bpv";
            //    obj.Show();
            //}

            //if (name.Contains("JV-") && !name.Contains("S"))
            //{
            //    string id = name;
            //    POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
            //    obj.id = id;
            //    obj.branch = cmbbranchjv.SelectedValue.ToString();
            //    obj.name = "Journal Voucher";
            //    obj.type = "jv";
            //    obj.Show();
            //}
            //if (name.Contains("GRN"))
            //{
            //    string id = name;

            //    id = id.Substring(4);

            //    POSRestaurant.Reports.Inventory.frmReceivedInventory obj = new Inventory.frmReceivedInventory();
            //    obj.purchaseid = id;
            //    //POSRestaurant.Reports.Voucher.frmVoucherPrieview obj = new Reports.Voucher.frmVoucherPrieview();
            //    //obj.id = id;
            //    //obj.branch = cmbbranchjv.SelectedValue.ToString();
            //    //obj.name = "Sales Journal Voucher";
            //    //obj.type = "sjv";
            //    obj.Show();
            //}

        }
    }
}
