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
    public partial class FrmTerminalSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmTerminalSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            ds = new DataSet();
            string q = "SELECT DISTINCT Terminal FROM         Sale";
            ds = objCore.funGetDataSet(q);
            DataRow dr = ds.Tables[0].NewRow();
            dr["Terminal"] = "All Terminals";
            ds.Tables[0].Rows.Add(dr);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "Terminal";
            comboBox1.DisplayMember = "Terminal";
            comboBox1.Text = "All Terminals";
            
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rprTerminalSale rptDoc = new rprTerminalSale();
                POSRestaurant.Reports.SaleReports.DsTerminalSale dsrpt = new DsTerminalSale();
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
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                else
                {
                    if (logo == "")
                    { }
                    else
                    {

                        dt.Rows.Add("", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);



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
                dtrpt.Columns.Add("Terminal", typeof(string));
                dtrpt.Columns.Add("Count", typeof(double));
                dtrpt.Columns.Add("Sum", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
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

                if (comboBox1.Text == "All Terminals")
                {
                    q = "SELECT      SUM(TotalBill -DiscountAmount) AS  sum, COUNT(NetBill) AS count, Date, dbo.Sale.Terminal FROM         Sale  WHERE     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and billstatus='Paid'  GROUP BY Date, Terminal";
                }
                else
                {
                    //q = "SELECT     SUM(dbo.Saledetails.Price) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.Sale.Date, dbo.Sale.Terminal FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='"+comboBox1.Text+"'  GROUP BY dbo.Sale.Date, dbo.Sale.Terminal";
                    q = "SELECT      SUM(TotalBill -DiscountAmount) AS  sum, COUNT(NetBill) AS count, Date, dbo.Sale.Terminal FROM         Sale  WHERE     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and billstatus='Paid' and dbo.Sale.terminal='" + comboBox1.Text + "'  GROUP BY Date, Terminal";
                }
                

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Terminal"].ToString(), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString()), 3), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["sum"].ToString()), 3), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Terminal"].ToString(), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString()), 3), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["sum"].ToString()), 3), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
