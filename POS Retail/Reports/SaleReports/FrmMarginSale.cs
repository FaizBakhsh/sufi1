using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.SaleReports
{
    public partial class FrmMarginSale : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmMarginSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select id,ItemName from RawItem";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["ItemName"] = "All Items";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "ItemName";
                comboBox1.Text = "All Items";
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


                POSRetail.Reports.SaleReports.RptMargin rptDoc = new RptMargin();
                POSRetail.Reports.SaleReports.DsMargin dsrpt = new DsMargin();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                dsrpt.Tables[0].Merge(dt);


                rptDoc.SetDataSource(dsrpt);

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
                dtrpt.Columns.Add("Item", typeof(string));
                dtrpt.Columns.Add("Purchse", typeof(string));
                dtrpt.Columns.Add("Sale", typeof(string));
                dtrpt.Columns.Add("Profit", typeof(string));
                dtrpt.Columns.Add("SDate", typeof(string));
                dtrpt.Columns.Add("EDate", typeof(string));
                DataSet ds = new DataSet();
                string q = "";
                if (comboBox1.Text == "All Items")
                {
                    //q = "SELECT     SUM(dbo.Saledetails.TotalPrice) AS sum, COUNT(dbo.Saledetails.TotalPrice) AS count, dbo.Sale.Date, dbo.RawItem.ItemName FROM         dbo.Saledetails INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.Sale.Date, dbo.RawItem.ItemName";
                    q = "SELECT     AVG(dbo.PurchaseDetails.PricePerItem) AS purchase, dbo.PurchaseDetails.RawItemId, AVG(dbo.Saledetails.Price) AS sale, dbo.RawItem.ItemName FROM         dbo.Saledetails INNER JOIN                      dbo.PurchaseDetails ON dbo.Saledetails.ItemId = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.PurchaseDetails.RawItemId, dbo.RawItem.ItemName";
                }
                else
                {
                   // q = "SELECT     SUM(dbo.Saledetails.TotalPrice) AS sum, COUNT(dbo.Saledetails.TotalPrice) AS count, dbo.Sale.Date, dbo.RawItem.ItemName as name FROM         dbo.Saledetails INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.userid='" + comboBox1.SelectedValue + "' GROUP BY dbo.Sale.Date, dbo.RawItem.ItemName";
                    q = "SELECT     AVG(dbo.PurchaseDetails.PricePerItem) AS purchase, dbo.PurchaseDetails.RawItemId, AVG(dbo.Saledetails.Price) AS sale, dbo.RawItem.ItemName FROM         dbo.Saledetails INNER JOIN                      dbo.PurchaseDetails ON dbo.Saledetails.ItemId = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.PurchaseDetails.RawItemId='" + comboBox1.SelectedValue + "' GROUP BY dbo.PurchaseDetails.RawItemId, dbo.RawItem.ItemName";

                }

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double purchase = Convert.ToDouble(ds.Tables[0].Rows[i]["purchase"].ToString());
                    double sale = Convert.ToDouble(ds.Tables[0].Rows[i]["sale"].ToString());
                    purchase = Math.Round(purchase, 2);
                    sale = Math.Round(sale, 2);
                    double margin = sale - purchase;
                    margin = Math.Round(margin, 2);
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), purchase.ToString(), sale.ToString(), margin.ToString(), dateTimePicker1.Text, dateTimePicker2.Text);
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
