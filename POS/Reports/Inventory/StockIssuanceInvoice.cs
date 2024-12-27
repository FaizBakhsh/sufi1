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
    public partial class StockIssuanceInvoice : Form
    {
        public StockIssuanceInvoice()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string sid = "", branchid = "", sourcebranchid = "", date = "",type="";
        private void StockIssuanceInvoice_Load(object sender, EventArgs e)
        {

            bindreport(sid);
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport(string sid)
        {
            try
            {

                {
                    DataSet dsprint = new DataSet();

                    getcompany();

                    {
                        string company = "", phone = "", address = "", logo = "", ntn = "", stn = "";
                        try
                        {
                            company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                            phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                            address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                            ntn = dscompany.Tables[0].Rows[0]["ntn"].ToString();
                            stn = dscompany.Tables[0].Rows[0]["stn"].ToString();
                            logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        POSRestaurant.Reports.Inventory.rptdeliverychallanInvTransfer rptDoc = new rptdeliverychallanInvTransfer();
                        //POSRetail.Reports.CrystalReport2 rptDoc = new Reports.CrystalReport2();
                        POSRestaurant.Reports.Inventory.dstransferReceiving dsrpt = new dstransferReceiving();
                        //feereport ds = new feereport(); // .xsd file name
                        DataTable dt = new DataTable();

                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        dt = getAllOrders(sid);
                        dsrpt.Tables[0].Merge(dt, false, MissingSchemaAction.Ignore);

                        rptDoc.SetDataSource(dsrpt);
                        DataSet dsc = new DataSet();
                        string q = "select * from Branch where id='" + branchid + "'";
                        dsc = objCore.funGetDataSet(q);
                        if (dsc.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                rptDoc.SetParameterValue("caddress", dsc.Tables[0].Rows[0]["Location"].ToString());
                                rptDoc.SetParameterValue("customer", "To: " + dsc.Tables[0].Rows[0]["BranchName"].ToString());

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                        else
                        {
                            rptDoc.SetParameterValue("caddress", "");

                            rptDoc.SetParameterValue("customer", "");
                        }
                        if (type == "royality")
                        {
                            rptDoc.SetParameterValue("title", "Royalty Invoice");
                        }
                        else
                        {
                            rptDoc.SetParameterValue("title", "Store Invoice");
                        }
                        q = "select * from Branch where id='" + sourcebranchid + "'";
                        dsc = new DataSet();
                        dsc = objCore.funGetDataSet(q);
                        if (dsc.Tables[0].Rows.Count > 0)
                        {
                            try
                            {

                                rptDoc.SetParameterValue("sentby", "From: " + dsc.Tables[0].Rows[0]["BranchName"].ToString());

                            }
                            catch (Exception ex)
                            {


                            }
                        }
                        else
                        {
                            rptDoc.SetParameterValue("sentby", "");

                        }
                        rptDoc.SetParameterValue("Comp", company);
                        rptDoc.SetParameterValue("phn", phone);
                        rptDoc.SetParameterValue("Addrs", address);


                        crystalReportViewer1.ReportSource = rptDoc;
                        rptDoc.SetParameterValue("date", date);

                    }
                }
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }
        protected string getcat(string id)
        {
            string cat = "";
            try
            {
                string q = "SELECT        CategoryName FROM            Category where id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    cat = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return cat;
        }
        public DataTable getAllOrders(string siid)
        {
            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("Name", typeof(string));
            dtrpt.Columns.Add("UOM", typeof(string));
            dtrpt.Columns.Add("Quantity", typeof(double));
            dtrpt.Columns.Add("Price", typeof(double));
            dtrpt.Columns.Add("Amount", typeof(double));
            dtrpt.Columns.Add("logo", typeof(byte[]));
            dtrpt.Columns.Add("Category", typeof(string));
            DataSet ds = new DataSet();
            string q = "";
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
            }
            catch (Exception ex)
            {

            }
            if (type == "royality")
            {
                q = "SELECT       dbo.RawItem.Id, dbo.RawItem.LoosQTY,   dbo.RawItem.ItemName  AS ItemName, dbo.InventoryTransfer.TransferIn,dbo.InventoryTransfer.price,dbo.InventoryTransfer.total, dbo.InventoryTransfer.sourcebranchid FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransfer.branchid='" + branchid + "' and dbo.InventoryTransfer.sourcebranchid='" + sourcebranchid + "' and dbo.InventoryTransfer.date='" + date + "' and dbo.InventoryTransfer.TransferIn>0 and dbo.RawItem.ItemName like '%royalty%' and dbo.InventoryTransfer.status='Posted'";

            }
            else
            {
                q = "SELECT       dbo.RawItem.Id, dbo.RawItem.LoosQTY,dbo.RawItem.PackingName,dbo.RawItem.ItemName ,dbo.UOM.UOM , dbo.InventoryTransfer.TransferIn,dbo.InventoryTransfer.price,dbo.InventoryTransfer.total, dbo.InventoryTransfer.sourcebranchid FROM            dbo.InventoryTransfer INNER JOIN                         dbo.RawItem ON dbo.InventoryTransfer.Itemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.InventoryTransfer.branchid='" + branchid + "' and dbo.InventoryTransfer.date='" + date + "' and dbo.InventoryTransfer.TransferIn>0 and dbo.RawItem.ItemName not like '%royalty%'  and dbo.InventoryTransfer.status='Posted'";
            }
            ds = objCore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string cat = getcat(ds.Tables[0].Rows[i]["Id"].ToString());
                string uom = "";
                try
                {
                    uom = ds.Tables[0].Rows[i]["UOM"].ToString();
                }
                catch (Exception ex)
                {
                    
                  
                }
                string temp = ds.Tables[0].Rows[i]["TransferIn"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                double qty = Convert.ToDouble(temp);
                temp = ds.Tables[0].Rows[i]["LoosQTY"].ToString();
                if (temp == "")
                {
                    temp = "1";
                }
                double loosqty = Convert.ToDouble(temp);
                try
                {
                    if (loosqty > 1)
                    {
                        uom = ds.Tables[0].Rows[i]["PackingName"].ToString();
                        qty = qty / loosqty;
                    }
                }
                catch (Exception ex)
                {
                    
                }
                
                qty = Math.Round(qty, 2);
                if (logo == "")
                {
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(),uom, qty, ds.Tables[0].Rows[i]["price"].ToString(), ds.Tables[0].Rows[i]["total"].ToString(), null,cat);
                }
                else
                {
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(),uom, qty, ds.Tables[0].Rows[i]["price"].ToString(), ds.Tables[0].Rows[i]["total"].ToString(), dscompany.Tables[0].Rows[0]["logo"],cat);
                }
            }

            return dtrpt;
        }
    }
}
