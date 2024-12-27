using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmInvoiceSaleDS : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmInvoiceSaleDS()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select id,name from shifts ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbshift.DataSource = ds.Tables[0];
                cmbshift.ValueMember = "id";
                cmbshift.DisplayMember = "name";
                cmbshift.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            

        }
       
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptbilltype rptDoc = new rptbilltype();
                POSRestaurant.Reports.SaleReports.dsbilltype dsrpt = new dsbilltype();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
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


                rptDoc.SetDataSource(dsrpt);
                
                rptDoc.SetParameterValue("date", "for the period  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
      
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("BillNo", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("BillType", typeof(string));
                dtrpt.Columns.Add("Customer", typeof(string));
                dtrpt.Columns.Add("Orderterminal", typeof(DateTime));
                dtrpt.Columns.Add("Terminal", typeof(string));
                dtrpt.Columns.Add("Shift", typeof(string));
                dtrpt.Columns.Add("Onlineid", typeof(string));
                dtrpt.Columns.Add("Discount", typeof(double));
                dtrpt.Columns.Add("GST", typeof(double));
                dtrpt.Columns.Add("Total", typeof(double));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                DataSet ds = new DataSet();
                string q = "";


                if (cmbtype.Text == "All")
                {
                    if (cmbshift.Text == "All")
                    {
                        q = "SELECT        dbo.DSSale.Id,  dbo.DSSale.NetBill,dbo.DSSale.Billtype,dbo.DSSale.TotalBill,dbo.DSSale.GST, dbo.DSSale.DiscountAmount,  dbo.DSSale.Date, dbo.DSSale.Customer, dbo.DSSale.TerminalOrder, dbo.DSSale.Terminal, dbo.Shifts.Name, dbo.DSBillType.type, dbo.DSBillType.Amount, dbo.DSBillType.cashoutime FROM            dbo.DSSale INNER JOIN                         dbo.Shifts ON dbo.DSSale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.DSBillType ON dbo.DSSale.Id = dbo.DSBillType.saleid where dbo.DSSale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' order by dbo.DSSale.id";
                    }
                    else
                    {
                        q = "SELECT        dbo.DSSale.Id,dbo.DSSale.NetBill,dbo.DSSale.Billtype,dbo.DSSale.TotalBill,dbo.DSSale.GST, dbo.DSSale.DiscountAmount,   dbo.DSSale.Date, dbo.DSSale.Customer, dbo.DSSale.TerminalOrder, dbo.DSSale.Terminal, dbo.Shifts.Name, dbo.DSBillType.type, dbo.DSBillType.Amount, dbo.DSBillType.cashoutime FROM            dbo.DSSale INNER JOIN                         dbo.Shifts ON dbo.DSSale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.DSBillType ON dbo.DSSale.Id = dbo.DSBillType.saleid  where dbo.DSSale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' order by dbo.DSSale.id";
                    }
                }
                else
                {
                    if (cmbshift.Text == "All")
                    {
                        q = "SELECT        dbo.DSSale.Id,dbo.DSSale.NetBill,dbo.DSSale.Billtype,dbo.DSSale.TotalBill,dbo.DSSale.GST, dbo.DSSale.DiscountAmount,   dbo.DSSale.Date, dbo.DSSale.Customer, dbo.DSSale.TerminalOrder, dbo.DSSale.Terminal, dbo.Shifts.Name, dbo.DSBillType.type, dbo.DSBillType.Amount, dbo.DSBillType.cashoutime FROM            dbo.DSSale INNER JOIN                         dbo.Shifts ON dbo.DSSale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.DSBillType ON dbo.DSSale.Id = dbo.DSBillType.saleid  where dbo.DSSale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.DSSale.BillType like '%" + cmbtype.Text + "%' order by dbo.DSSale.id";
                    }
                    else
                    {
                        q = "SELECT        dbo.DSSale.Id,dbo.DSSale.NetBill,dbo.DSSale.Billtype, dbo.DSSale.TotalBill,dbo.DSSale.GST, dbo.DSSale.DiscountAmount,  dbo.DSSale.Date, dbo.DSSale.Customer, dbo.DSSale.TerminalOrder, dbo.DSSale.Terminal, dbo.Shifts.Name, dbo.DSBillType.type, dbo.DSBillType.Amount, dbo.DSBillType.cashoutime FROM            dbo.DSSale INNER JOIN                         dbo.Shifts ON dbo.DSSale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.DSBillType ON dbo.DSSale.Id = dbo.DSBillType.saleid  where dbo.DSSale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.DSSale.BillType like '%" + cmbtype.Text + "%'  order by dbo.DSSale.id";
                    }
                }
                
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double bill = 0, total = 0, gst = 0, dis = 0;
                    string val = "";
                    val = ds.Tables[0].Rows[i]["NetBill"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    total = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["DiscountAmount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    dis = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["gst"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    gst = Convert.ToDouble(val);
                    try
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), total, ds.Tables[0].Rows[i]["Billtype"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString(), Convert.ToDateTime(ds.Tables[0].Rows[i]["cashoutime"].ToString()), ds.Tables[0].Rows[i]["Terminal"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), "", dis, gst, ds.Tables[0].Rows[i]["TotalBill"].ToString());

                    }
                    catch (Exception wx)
                    {
                        
                       
                    }
                }                                                                                             
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
