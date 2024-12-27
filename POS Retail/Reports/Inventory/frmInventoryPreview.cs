using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.Inventory
{
    public partial class frmInventoryPreview : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                    POSRetail.Reports.Inventory.rprInventoryPurchase rptDoc = new Reports.Inventory.rprInventoryPurchase();
                    POSRetail.Reports.Inventory.DsInventoryReceived dsrpt = new Reports.Inventory.DsInventoryReceived();
                    // .xsd file name
                    DataTable dt = new DataTable();

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
                    // Just set the name of data table
                    dt.TableName = "Crystal Report";
                    dt = getAllOrders();
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

                            dt.Rows.Add("", DateTime.Now, "", "", "", "0", "0", "0", "0", "0", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                            dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                        }
                    }
                    rptDoc.SetDataSource(dsrpt);

                    rptDoc.SetParameterValue("Comp", company);
                    rptDoc.SetParameterValue("Addrs", phone);
                    rptDoc.SetParameterValue("phn", address);
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
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("Supplier", typeof(string));
                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("InvoiceNo", typeof(string));
                dtrpt.Columns.Add("serialNo", typeof(string));
                dtrpt.Columns.Add("Package", typeof(string));

                dtrpt.Columns.Add("PackageItems", typeof(Int32));
                dtrpt.Columns.Add("TotalItems", typeof(double));
                dtrpt.Columns.Add("Price", typeof(double));

                dtrpt.Columns.Add("TotalAmount ", typeof(double));
                dtrpt.Columns.Add("PurchaseTotalAmount", typeof(double));
                dtrpt.Columns.Add("ItemName", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("pdate", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));

                POSRetail.classes.Clsdbcon objCore=new classes.Clsdbcon();
                string q = "SELECT     dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems,                       dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.PurchaseDetails.TotalAmount, SUM(dbo.Purchase.TotalAmount) AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.Purchase.id='" + id + "' GROUP BY dbo.Supplier.Name, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems,                       dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.PurchaseDetails.TotalAmount";
                dsinfo = objCore.funGetDataSet(q);//"SELECT     dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems,                       dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM,                       dbo.CompanyInfo.logo FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id CROSS JOIN                      dbo.CompanyInfo where dbo.Purchase.id='" + id + "'");

                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                for (int j = 0; j < dsinfo.Tables[0].Rows.Count; j++)
                {
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[j]["Supplier"].ToString(), dsinfo.Tables[0].Rows[j]["Date"].ToString(), dsinfo.Tables[0].Rows[j]["InvoiceNo"].ToString(), dsinfo.Tables[0].Rows[j]["serialNo"].ToString(), dsinfo.Tables[0].Rows[j]["Package"].ToString(), dsinfo.Tables[0].Rows[j]["PackageItems"].ToString(), dsinfo.Tables[0].Rows[j]["TotalItems"].ToString(), dsinfo.Tables[0].Rows[j]["Price"].ToString(), dsinfo.Tables[0].Rows[j]["TotalAmount"].ToString(), dsinfo.Tables[0].Rows[j]["PurchaseTotalAmount"].ToString(), dsinfo.Tables[0].Rows[j]["ItemName"].ToString(), dsinfo.Tables[0].Rows[j]["UOM"].ToString(), "", null);

                    }
                    else
                    {

                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[j]["Supplier"].ToString(), dsinfo.Tables[0].Rows[j]["Date"].ToString(), dsinfo.Tables[0].Rows[j]["InvoiceNo"].ToString(), dsinfo.Tables[0].Rows[j]["serialNo"].ToString(), dsinfo.Tables[0].Rows[j]["Package"].ToString(), dsinfo.Tables[0].Rows[j]["PackageItems"].ToString(), dsinfo.Tables[0].Rows[j]["TotalItems"].ToString(), dsinfo.Tables[0].Rows[j]["Price"].ToString(), dsinfo.Tables[0].Rows[j]["TotalAmount"].ToString(), dsinfo.Tables[0].Rows[j]["PurchaseTotalAmount"].ToString(), dsinfo.Tables[0].Rows[j]["ItemName"].ToString(), dsinfo.Tables[0].Rows[j]["UOM"].ToString(), "", dscompany.Tables[0].Rows[0]["logo"]);




                    }
                }
                //dtrpt = dsinfo.Tables[0];

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
