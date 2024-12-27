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
    public partial class frmStoreIssuancePreview : Form
    {
        public string date = "", kitchen = "", demandno = "", kdsid = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmStoreIssuancePreview()
        {
            InitializeComponent();
        }
        public void fill()
        {
            
        }
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            bindreport();
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fill();
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.Inventory.rptstoreissuancepreview rptDoc = new rptstoreissuancepreview();
                POSRestaurant.Reports.Inventory.dsStoreissuancepreview dsrpt = new dsStoreissuancepreview();
                //feereport ds = new feereport(); // .xsd file name
                date = Convert.ToDateTime(date).ToString("yyyy-MM-dd");
                getcompany();
                dt = getAllOrders();
                // Just set the name of data table
                dt.TableName = "Crystal Report";
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
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Address", phone);
                rptDoc.SetParameterValue("phone", address);
                date = Convert.ToDateTime(date).ToString("dd-MM-yyyy");
                rptDoc.SetParameterValue("date",date);
                rptDoc.SetParameterValue("kitchen", kitchen);
                rptDoc.SetParameterValue("invoice", demandno);
                rptDoc.SetParameterValue("title", "Store Issuance");
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

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
        public DataTable getAllOrders()
        {
            getcompany();

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Unit", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Price", typeof(double));
                dtrpt.Columns.Add("Total", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                DataSet ds = new DataSet();
                string q = "";
                DataSet dsrawitems = new DataSet();

                DataSet dsi = new DataSet();

                q = "SELECT        dbo.RawItem.ItemName,dbo.RawItem.id, dbo.UOM.UOM, dbo.InventoryTransferStore.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.InventoryTransferStore ON dbo.RawItem.Id = dbo.InventoryTransferStore.Itemid WHERE        (dbo.InventoryTransferStore.Date = '" + date + "') AND (dbo.InventoryTransferStore.Invoiceno = '" + demandno + "') AND (dbo.InventoryTransferStore.RecvStoreId = '" + kdsid + "') and dbo.InventoryTransferStore.Quantity>0";
                dsi = objCore.funGetDataSet(q);
                for (int i = 0; i < dsi.Tables[0].Rows.Count; i++)
                {
                    string temp = dsi.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    double price = getprice(dsi.Tables[0].Rows[i]["id"].ToString());

                    dtrpt.Rows.Add(dsi.Tables[0].Rows[i]["ItemName"].ToString(), dsi.Tables[0].Rows[i]["UOM"].ToString(), qty, price, price * qty, dscompany.Tables[0].Rows[0]["logo"]);
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
