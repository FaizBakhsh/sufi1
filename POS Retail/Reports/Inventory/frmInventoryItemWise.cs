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
    public partial class frmInventoryItemWise : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmInventoryItemWise()
        {
            InitializeComponent();
        }
     
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select * from RawItem";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["ItemName"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "ItemName";
                cmbsupplier.Text = "All";

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
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
                DataTable dt = new DataTable();


                POSRetail.Reports.Inventory.rptItemWiseinventory rptDoc = new rptItemWiseinventory();
                POSRetail.Reports.Inventory.dsIteminventory dsrpt = new dsIteminventory();
                //feereport ds = new feereport(); // .xsd file name

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

                        dt.Rows.Add("", "", "", "", "", "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);
                        


                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
               
                //dsrpt.Tables[0].Merge(dt); ;
                rptDoc.SetDataSource(dsrpt);
              
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs",address );
                rptDoc.SetParameterValue("phn", phone);
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
                dtrpt.Columns.Add("ItemName", typeof(string));
                dtrpt.Columns.Add("Caption", typeof(string));

                dtrpt.Columns.Add("SizeName", typeof(string));
                dtrpt.Columns.Add("GroupName", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(string));
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
               
                string q = "";
                if (cmbsupplier.Text == "All")
                {
                    
                        //q = "SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package,                       dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where   (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  ";
                    q = "SELECT     dbo.RawItem.ItemName, dbo.Color.Caption, dbo.Size.SizeName, dbo.Groups.GroupName, SUM(dbo.Inventory.Quantity) AS Quantity FROM         dbo.Inventory INNER JOIN                      dbo.RawItem ON dbo.Inventory.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Groups ON dbo.RawItem.GroupId = dbo.Groups.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id where   (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.ItemName, dbo.Color.Caption, dbo.Size.SizeName, dbo.Groups.GroupName ";
                    
                }
                else
                {
                    q = "SELECT     dbo.RawItem.ItemName, dbo.Color.Caption, dbo.Size.SizeName, dbo.Groups.GroupName, SUM(dbo.Inventory.Quantity) AS Quantity FROM         dbo.Inventory INNER JOIN                      dbo.RawItem ON dbo.Inventory.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Groups ON dbo.RawItem.GroupId = dbo.Groups.Id INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id where   (dbo.Purchase.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.RawItem.id='" + cmbsupplier.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.Color.Caption, dbo.Size.SizeName, dbo.Groups.GroupName ";
                    
                }
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                double tamount = 0;
                dsinfo = objCore.funGetDataSet(q);
                for (int j = 0; j < dsinfo.Tables[0].Rows.Count; j++)
                {

                    if (logo == "")
                    {
                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[j]["ItemName"].ToString(), dsinfo.Tables[0].Rows[j]["Caption"].ToString(), dsinfo.Tables[0].Rows[j]["SizeName"].ToString(), dsinfo.Tables[0].Rows[j]["GroupName"].ToString(), dsinfo.Tables[0].Rows[j]["Quantity"].ToString(), "for he period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);

                    }
                    else
                    {


                        

                        dtrpt.Rows.Add(dsinfo.Tables[0].Rows[j]["ItemName"].ToString(), dsinfo.Tables[0].Rows[j]["Caption"].ToString(), dsinfo.Tables[0].Rows[j]["SizeName"].ToString(), dsinfo.Tables[0].Rows[j]["GroupName"].ToString(), dsinfo.Tables[0].Rows[j]["Quantity"].ToString(), "for he period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);

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

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
