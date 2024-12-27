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
    public partial class FrmPaymentWiseSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmPaymentWiseSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                // ds1.Tables[0].Rows.Add(dr1);
                cmbbranch.DataSource = ds1.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";

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


                POSRestaurant.Reports.SaleReports.rprPaymentWiseSale rptDoc = new rprPaymentWiseSale();
                POSRestaurant.Reports.SaleReports.DsPayementWiseSale dsrpt = new DsPayementWiseSale();
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
                // if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                //else
                //{
                //    if (logo == "")
                //    { }
                //    else
                //    {

                //        dt.Rows.Add("", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                //        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                //    }
                //}
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
                dtrpt.Columns.Add("Type", typeof(string));
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

                q = "SELECT        dbo.Sale.BillType, SUM(dbo.BillType.Amount) AS cash, COUNT(dbo.Sale.Id) AS count FROM            dbo.Sale INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid WHERE      (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"') AND (dbo.Sale.BillStatus = 'Paid') GROUP BY dbo.Sale.BillType";




                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["BillType"].ToString(), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString()), 3), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["cash"].ToString()), 3), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["BillType"].ToString(), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString()), 3), Math.Round(Convert.ToDouble(ds.Tables[0].Rows[i]["cash"].ToString()), 3), dscompany.Tables[0].Rows[0]["logo"]);
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
