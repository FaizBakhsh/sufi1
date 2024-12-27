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
    public partial class frmPOPreview : Form
    {
        public string id = "", date = "", vendor = "", invoiceno = "";
        public frmPOPreview()
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

                POSRestaurant.Reports.Inventory.rptPO rptDoc = new Reports.Inventory.rptPO();
                POSRestaurant.Reports.Inventory.dsPO dsrpt = new Reports.Inventory.dsPO();
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
                    date = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
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

                    rptDoc.SetDataSource(dsrpt);
                    rptDoc.SetParameterValue("Comp", company);
                    rptDoc.SetParameterValue("Addrs", address);
                    rptDoc.SetParameterValue("phn", phone);
                    rptDoc.SetParameterValue("pono", id);
                    rptDoc.SetParameterValue("pono", id);
                    rptDoc.SetParameterValue("demandno", invoiceno);
                    rptDoc.SetParameterValue("date", date);
                    rptDoc.SetParameterValue("supplier", vendor);
                    crystalReportViewer1.ReportSource = rptDoc;

                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        DataSet dsusers = new DataSet();
        public void getusers(string id)
        {
            try
            {
                dsusers = objCore.funGetDataSet("select * from Users where id=" + id);
            }
            catch (Exception ex)
            {


            }

        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("Unit", typeof(string));
                dtrpt.Columns.Add("Price", typeof(double));
                dtrpt.Columns.Add("Total", typeof(double));
                dtrpt.Columns.Add("Sign", typeof(byte[]));
                dtrpt.Columns.Add("Vendor", typeof(string));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                string q = "SELECT        dbo.RawItem.LoosQTY, dbo.RawItem.PackingName, dbo.Supplier.Name AS Supplier_Name, dbo.PurchaseOrder.Id AS PONo, dbo.PurchaseOrder.InvoiceNo, dbo.PurchaseOrder.Date, dbo.PurchaseOrder.Approveduserid,                          dbo.RawItem.ItemName AS Name, dbo.UOM.UOM, dbo.PurchaseOrderDetails.TotalItems, dbo.PurchaseOrderDetails.PricePerItem, dbo.PurchaseOrderDetails.TotalAmount FROM            dbo.PurchaseOrder INNER JOIN                         dbo.PurchaseOrderDetails ON dbo.PurchaseOrder.Id = dbo.PurchaseOrderDetails.PurchaseOrderId INNER JOIN                         dbo.RawItem ON dbo.PurchaseOrderDetails.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id LEFT OUTER JOIN                         dbo.Supplier ON dbo.RawItem.Supplierid = dbo.Supplier.Id WHERE        (dbo.PurchaseOrder.id = '" + id + "')";
                dsinfo = objCore.funGetDataSet(q);
                for (int i = 0; i < dsinfo.Tables[0].Rows.Count; i++)
                {
                    getusers(dsinfo.Tables[0].Rows[i]["Approveduserid"].ToString());
                    string usersid = "";
                    try
                    {
                        usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                    }
                    catch (Exception ex)
                    {

                    }
                    string price = dsinfo.Tables[0].Rows[i]["PricePerItem"].ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    string total = dsinfo.Tables[0].Rows[i]["TotalAmount"].ToString();
                    if (total == "")
                    {
                        total = "0";
                    }
                   string temp = dsinfo.Tables[0].Rows[i]["LoosQTY"].ToString();
                    if (temp == "")
                    {
                        temp = "1";
                    }
                    double loosqty = Convert.ToDouble(temp);
                    
                    temp = dsinfo.Tables[0].Rows[i]["TotalItems"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                   
                    string uom=dsinfo.Tables[0].Rows[i]["UOM"].ToString();
                    if (loosqty > 1)
                    {
                        qty = qty / loosqty;
                        uom = dsinfo.Tables[0].Rows[i]["PackingName"].ToString();
                    }

                    qty = Math.Round(qty, 0);
                    if (usersid == "")
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["Name"].ToString(), qty, dscompany.Tables[0].Rows[0]["logo"], uom, price, total, null, dsinfo.Tables[0].Rows[i]["Supplier_Name"].ToString());
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["Name"].ToString(), qty, dscompany.Tables[0].Rows[0]["logo"], uom, price, total, dsusers.Tables[0].Rows[0]["Signature"], dsinfo.Tables[0].Rows[i]["Supplier_Name"].ToString());
                    }

                }
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
