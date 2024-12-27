using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Inventory
{
    public partial class frmInventoryPreview : Form
    {
        public string id = "";
        public frmInventoryPreview()
        {
            InitializeComponent();
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

               
                    //ReportDocument rptDoc = new ReportDocument();
                    POSRestaurant.Reports.Inventory.rprInventoryPurchase rptDoc = new Reports.Inventory.rprInventoryPurchase();
                    POSRestaurant.Reports.Inventory.DsInventoryReceived dsrpt = new Reports.Inventory.DsInventoryReceived();
                    // .xsd file name
                    DataTable dt = new DataTable();
                    string company = "", phone = "", address = "", logo = "";
                    try
                    {
                        getcompany();
                        company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                        phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                        address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                        logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                       
                    }
                    catch (Exception ex)
                    {

                    }
                    // Just set the name of data table
                    dt.TableName = "Crystal Report";
                    dt = getAllOrders();
                    if (dt.Rows.Count > 0)
                    {

                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);

                        string po = "";
                        try
                        {
                            string q = "select pono from purchase where id='" + id + "'";
                            DataSet dss = new DataSet();
                            dss = objCore.funGetDataSet(q);
                            if (dss.Tables[0].Rows.Count > 0)
                            {
                                po = dss.Tables[0].Rows[0][0].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        rptDoc.SetDataSource(dsrpt);
                        rptDoc.SetParameterValue("po", po);
                        rptDoc.SetParameterValue("Comp", company);
                        rptDoc.SetParameterValue("Addrs", address);
                        rptDoc.SetParameterValue("phn", phone);
                       
                        crystalReportViewer1.ReportSource = rptDoc;

                        //rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["Name"].ToString();
                       // rptDoc.PrintToPrinter(1, false, 0, 0);
                    }

               

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();

        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("Supplier", typeof(string));
                

                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("InvoiceNo", typeof(string));
                dtrpt.Columns.Add("serialNo", typeof(string));
                dtrpt.Columns.Add("Package", typeof(double));

                dtrpt.Columns.Add("PackageItems", typeof(double));
                dtrpt.Columns.Add("TotalItems", typeof(double));
                dtrpt.Columns.Add("Price", typeof(double));
                
                dtrpt.Columns.Add("TotalAmount ", typeof(double));
                dtrpt.Columns.Add("PurchaseTotalAmount", typeof(double));
                dtrpt.Columns.Add("ItemName", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dsinfo = objCore.funGetDataSet("SELECT        dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems,                          dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.CompanyInfo.logo FROM            dbo.Purchase INNER JOIN                         dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                         dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                         dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id CROSS JOIN                         dbo.CompanyInfo where dbo.Purchase.id='" + id + "'");

                dtrpt = dsinfo.Tables[0];

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return dtrpt;// dsinfo.Tables[0];
        }
        private void frmInventoryPreview_Load(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
