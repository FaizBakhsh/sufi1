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
    public partial class frmRequisitionPreview : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string sid = "", branchid = "", sourcebranchid = "", date = "";
        public frmRequisitionPreview()
        {
            InitializeComponent();
        }

        private void frmDeliveryChallanInvTransfer_Load(object sender, EventArgs e)
        {
            bindreport(sid);
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo ");
            dsbranch = objCore.funGetDataSet("select * from branch where id=" + branchid);
        }
        DataSet dsbranch = new DataSet();
       
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
                            company = dsbranch.Tables[0].Rows[0]["branchName"].ToString();
                            phone = "";
                            address = dsbranch.Tables[0].Rows[0]["Location"].ToString();
                            
                            logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        POSRestaurant.Reports.Inventory.rptRequisition rptDoc = new rptRequisition();
                        //POSRetail.Reports.CrystalReport2 rptDoc = new Reports.CrystalReport2();
                        POSRestaurant.Reports.Inventory.dsrequisition dsrpt = new dsrequisition();
                        //feereport ds = new feereport(); // .xsd file name
                        DataTable dt = new DataTable();

                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        dt = getAllOrders(sid);
                        dsrpt.Tables[0].Merge(dt, false, MissingSchemaAction.Ignore);

                        rptDoc.SetDataSource(dsrpt);
                        DataSet dsc = new DataSet();
                        
                        
                      
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
            dtrpt.Columns.Add("Cat", typeof(string));
            dtrpt.Columns.Add("Name", typeof(string));
            dtrpt.Columns.Add("UOM", typeof(string));
            dtrpt.Columns.Add("Quantity", typeof(double));
            dtrpt.Columns.Add("Price", typeof(double));
            dtrpt.Columns.Add("TotalAmount", typeof(double));
            dtrpt.Columns.Add("Sign", typeof(byte[]));
            dtrpt.Columns.Add("logo", typeof(byte[]));
          
            
            DataSet ds = new DataSet();
            string q = "";


            q = "SELECT        dbo.RawItem.Id, dbo.RawItem.LoosQTY, dbo.RawItem.PackingName, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.Requisition.Approveduserid, dbo.Requisition.Quantity, dbo.Requisition.Price, dbo.Requisition.TotalAmount,                          dbo.Requisition.branchid, dbo.Category.CategoryName FROM            dbo.Requisition INNER JOIN                         dbo.RawItem ON dbo.Requisition.ItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id where dbo.Requisition.branchid='" + branchid + "' and dbo.Requisition.date='" + date + "' and dbo.Requisition.Quantity>0 and dbo.Requisition.status='Approved'";
            ds = objCore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                
                getusers(ds.Tables[0].Rows[i]["Approveduserid"].ToString());
                string usersid = "";
                try
                {
                    usersid = dsusers.Tables[0].Rows[0]["Signature"].ToString();
                }
                catch (Exception ex)
                {

                }
               
                string cat = getcat(ds.Tables[0].Rows[i]["Id"].ToString());
                string uom = "";
                try
                {
                    uom = ds.Tables[0].Rows[i]["UOM"].ToString();
                }
                catch (Exception ex)
                {


                }
                string temp = ds.Tables[0].Rows[i]["Quantity"].ToString();
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
                if (loosqty > 1)
                {
                    qty = qty / loosqty;
                    uom = ds.Tables[0].Rows[i]["PackingName"].ToString();
                }
                
                qty = Math.Round(qty, 0);
                if (usersid == "")
                {
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["CategoryName"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), uom, qty, ds.Tables[0].Rows[i]["price"].ToString(), ds.Tables[0].Rows[i]["TotalAmount"].ToString(), null, dscompany.Tables[0].Rows[0]["logo"]);
                }
                else
                {
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["CategoryName"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), uom, qty, ds.Tables[0].Rows[i]["price"].ToString(), ds.Tables[0].Rows[i]["TotalAmount"].ToString(), dscompany.Tables[0].Rows[0]["logo"], dsusers.Tables[0].Rows[0]["Signature"]);
                }
            }

            return dtrpt;
        }
    }
}
