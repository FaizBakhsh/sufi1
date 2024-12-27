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
    public partial class frmTransferoutapproval : Form
    {
        public string id = "", date = "", vendor = "", invoiceno = "",branchid="",branchname="";
        public frmTransferoutapproval()
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

                POSRestaurant.Reports.Inventory.rptTransferOutApproval rptDoc = new Reports.Inventory.rptTransferOutApproval();
                POSRestaurant.Reports.Inventory.dsPO dsrpt = new Reports.Inventory.dsPO();
                // .xsd file name
                DataTable dt = new DataTable();
                string company = "", phone = "", address = "", logo = "";
                
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                if (dt.Rows.Count > 0)
                {
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

                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);

                    rptDoc.SetDataSource(dsrpt);
                    rptDoc.SetParameterValue("comp", branchname);
                    rptDoc.SetParameterValue("Addrs", "");
                    rptDoc.SetParameterValue("phn", "");
                   
                    rptDoc.SetParameterValue("date", date);
                   
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

                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.LoosQTY, dbo.RawItem.PackingName, dbo.RawItem.ItemName AS Name, dbo.UOM.UOM, dbo.InventoryTransferApproval.Approveduserid, dbo.InventoryTransferApproval.TransferOut, dbo.InventoryTransferApproval.price,                          dbo.InventoryTransferApproval.total FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.InventoryTransferApproval ON dbo.RawItem.Id = dbo.InventoryTransferApproval.Itemid where dbo.InventoryTransferApproval.branchid='" + branchid + "' and dbo.InventoryTransferApproval.date='" + date + "'";
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
                   
                   string temp = dsinfo.Tables[0].Rows[i]["LoosQTY"].ToString();
                    if (temp == "")
                    {
                        temp = "1";
                    }
                    double loosqty = Convert.ToDouble(temp);

                    temp = dsinfo.Tables[0].Rows[i]["TransferOut"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    double price1 = getprice(dsinfo.Tables[0].Rows[i]["id"].ToString());
                    double amount = 0;

                    try
                    {
                        amount = price1 * qty;
                    }
                    catch (Exception ex)
                    {


                    }
                    string uom=dsinfo.Tables[0].Rows[i]["UOM"].ToString();
                    if (loosqty > 1)
                    {
                        qty = qty / loosqty;
                        uom = dsinfo.Tables[0].Rows[i]["PackingName"].ToString();
                    }

                    qty = Math.Round(qty, 0);

                    


                    if (usersid == "")
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["Name"].ToString(), qty, dscompany.Tables[0].Rows[0]["logo"], uom, price1, amount, null, "");
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[i]["Name"].ToString(), qty, dscompany.Tables[0].Rows[0]["logo"], uom, price1, amount, dsusers.Tables[0].Rows[0]["Signature"], "");
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return dtrpt;// dsinfo.Tables[0];
        }
        public double getprice(string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + date + "','" + date + "'," + id + ")";
            try
            {
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {
            }

            return cost;
        }
        private void frmInventoryPreview_Load(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
