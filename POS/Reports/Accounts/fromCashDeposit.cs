using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Runtime;
namespace POSRestaurant.Reports.Accounts
{
    public partial class frmCashDeposit : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public frmCashDeposit()
        {
            InitializeComponent();
        }

        private void frmPayableStatemetBank_Load(object sender, EventArgs e)
        {
            
            try
            {
                DataSet ds = new System.Data.DataSet();
                string q = "SELECT     Id,  BranchName  FROM         Branch ";
                ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["BranchName"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbbranchjv.DataSource = ds.Tables[0];
                cmbbranchjv.ValueMember = "id";
                cmbbranchjv.DisplayMember = "BranchName";
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
                //POSRestaurant.Reports.Statements.CrystalReport1 rptDoc = new CrystalReport1();
                POSRestaurant.Reports.Accounts.rptCashDeposit rptDoc = new rptCashDeposit();
                POSRestaurant.Reports.Accounts.dsCashDeposit dsrpt = new dsCashDeposit();
                //feereport ds = new feereport(); // .xsd file name
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllCashDeposit();           
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
                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                //dsrpt.Tables[0].DefaultView.Sort = "Date asc,VoucherNo asc";
                rptDoc.SetDataSource(dsrpt);
               // rptDoc.SetParameterValue("balance", totalin-totalout);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
               // rptDoc.SetParameterValue("branch", cmbbranchjv.Text);
                //rptDoc.SetParameterValue("title", "Cash Deposit");
                rptDoc.SetParameterValue("date", "For the period of " + Convert.ToDateTime(dateTimePicker1.Text).ToString("dd-MM-yyyy") + " to " + Convert.ToDateTime(dateTimePicker2.Text).ToString("dd-MM-yyyy"));
             
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        double bf = 0;
        public string getname(string id)
        {
            string name = "";
            DataSet dsname = new System.Data.DataSet();
            string q = "select name from ChartofAccounts where id='"+id+"'";
            dsname = objCore.funGetDataSet(q);
            if (dsname.Tables[0].Rows.Count > 0)
            {
                name = dsname.Tables[0].Rows[0][0].ToString();
            }
            return name;
        }
        double totalin = 0, totalout = 0;
        public DataTable getAllCashDeposit()
        {

            DataTable dat = new DataTable();
            dat.Columns.Add("Date", typeof(string));
            dat.Columns.Add("ActualAmount", typeof(double));
            dat.Columns.Add("DepositedAmount", typeof(double));
            dat.Columns.Add("OnlineId", typeof(int));
            dat.Columns.Add("BranchName", typeof(string));


            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }

            double ActualAmount = 0, DepositedAmount = 0;
            string OnlineId = "0";
            string Date = "0", BranchName = "0";

            DataSet ds = new DataSet();
         

            string temp = "";
            string q = "";
            try
            {
                ds = new DataSet();
                if (cmbbranchjv.Text == "All")
                {

                    q = "select convert(varchar, Date, 105) as Date ,ActualAmount,DepositedAmount,OnlineId,b.BranchName from BankDeposits d inner join Branch b on d.branchid=b.branchid where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') order by Date ";
                }
                else
                {
                    q = "select convert(varchar, Date, 105) as Date,ActualAmount,DepositedAmount,OnlineId,b.BranchName from BankDeposits d inner join Branch b on d.branchid=b.branchid where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and b.BranchName='" + cmbbranchjv.Text + "'" + " order by Date";
                }
                ds = objCore.funGetDataSet(q);
               // ds = db.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // Date = Convert.ToString(ds.Tables[0].Rows[0]["Date"].ToString());

                    // ActualAmount = Convert.ToDouble(ds.Tables[0].Rows[0]["ActualAmount"].ToString());
                    // DepositedAmount = Convert.ToDouble(ds.Tables[0].Rows[0]["DepositedAmount"].ToString());
                    // OnlineId = Convert.ToString(ds.Tables[0].Rows[0]["OnlineId"].ToString());
                    // BranchName = Convert.ToString(ds.Tables[0].Rows[0]["BranchName"].ToString());

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Date = Convert.ToString(dr["Date"].ToString());

                        ActualAmount = Convert.ToDouble(dr["ActualAmount"].ToString());
                        DepositedAmount = Convert.ToDouble(dr["DepositedAmount"].ToString());
                        OnlineId = Convert.ToString(ds.Tables[0].Rows[0]["OnlineId"].ToString());
                        BranchName = Convert.ToString(dr["BranchName"]);


                        if (logo == "")
                        {
                            //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, null, totlorder, avgsale);
                            dat.Rows.Add(Date, ActualAmount, DepositedAmount, OnlineId, BranchName);

                        }
                        else
                        {
                            dat.Rows.Add(Date, ActualAmount, DepositedAmount, OnlineId, BranchName);
                            //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale);
                        }
                    }


                }

                else
                {
                    ActualAmount = 0;
                    DepositedAmount = 0;
                    OnlineId = "0";
                    BranchName = "";
                    Date = "";
                }
            }
            catch (Exception ex)
            {


            }





            return dat;


            //dsshft = new DataSet();
            //dsshft = objcore.funGetDataSet("select * from users where id='" + user + "'");
            //if (dsshft.Tables[0].Rows.Count > 0)
            //{
            //    username = dsshft.Tables[0].Rows[0]["Name"].ToString();
            //}


        }
     
        private void button1_Click(object sender, EventArgs e)
        {
           
            System.GC.Collect();
            
            button1.Text = "Please Wait(Downloading Data)";
            button1.Enabled = false;
            bindreport();
            button1.Text = "Submit";
            button1.Enabled = true;
        }

        private void cmbbranchjv_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            POSRestaurant.Accounts.CashBookAccounts obj = new POSRestaurant.Accounts.CashBookAccounts();
            obj.Show();
        }
    }
}
