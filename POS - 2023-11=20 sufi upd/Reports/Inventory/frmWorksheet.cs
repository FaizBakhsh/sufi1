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
    public partial class frmWorksheet : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmWorksheet()
        {
            InitializeComponent();
        }
        public void fill()
        {
            
        }
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            
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


                POSRestaurant.Reports.Inventory.rptWorkSheet rptDoc = new rptWorkSheet();
                POSRestaurant.Reports.Inventory.dsworksheet dsrpt = new dsworksheet();
                //feereport ds = new feereport(); // .xsd file name

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

                        dt.Rows.Add("", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", "0", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period of " + dateTimePicker1.Text);


                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        double opening = 0, openingamount = 0;
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("ItemName", typeof(string));
                dtrpt.Columns.Add("OpeningStock", typeof(double));
                dtrpt.Columns.Add("OpeningAmount", typeof(double));
                dtrpt.Columns.Add("Received", typeof(double));
                dtrpt.Columns.Add("ReceivedAmount", typeof(double));
                dtrpt.Columns.Add("Consumed", typeof(double));
                dtrpt.Columns.Add("ConsumedAmount", typeof(double));
                dtrpt.Columns.Add("RawWaste", typeof(double));
                dtrpt.Columns.Add("RawWasteAmount", typeof(double));
                dtrpt.Columns.Add("CookWaste", typeof(double));
                dtrpt.Columns.Add("CookWasteAmount", typeof(double));
                dtrpt.Columns.Add("Variance", typeof(double));
                dtrpt.Columns.Add("VarianceAmount", typeof(double));
                dtrpt.Columns.Add("Closing", typeof(double));
                dtrpt.Columns.Add("ClosingAmount", typeof(double));
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
                string itemname = "";
                double  received = 0, receivedamount = 0, consumed = 0, consumedamount = 0, rawwaste = 0, rawwasteamount = 0, coockwaste = 0, coockwasteamount = 0, closing = 0, closingamount = 0, variance = 0, varianceamount = 0, unit = 0;
                DataSet ds = new DataSet();
                string q = "", date = "" ;
                DataSet dsrawitems = new DataSet();
                q = "SELECT     dbo.Inventory.RawItemId, dbo.Inventory.Quantity,RawItem.ItemName FROM         dbo.Inventory INNER JOIN                      dbo.RawItem ON dbo.Inventory.RawItemId = dbo.RawItem.Id  order by dbo.RawItem.ItemName";
                dsrawitems = objCore.funGetDataSet(q);
                for (int i = 0; i < dsrawitems.Tables[0].Rows.Count; i++)
                {
                    DataSet dsi = new DataSet();
                    string qry = "SELECT     SUM(dbo.PurchaseDetails.TotalAmount) / SUM(dbo.PurchaseDetails.TotalItems) AS Amount, COUNT(dbo.PurchaseDetails.RawItemId) AS count FROM         dbo.Purchase INNER JOIN                       dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId WHERE RawItemId='" + dsrawitems.Tables[0].Rows[i]["RawItemId"].ToString() + "' and   (dbo.Purchase.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                    dsi = objCore.funGetDataSet(qry);
                    if (dsi.Tables[0].Rows.Count > 0)
                    {
                        string val = dsi.Tables[0].Rows[0][0].ToString();
                        if (val == string.Empty)
                        {
                            val = "0";
                        }
                        receivedamount = Convert.ToDouble(val);
                        val = dsi.Tables[0].Rows[0][1].ToString();
                        if (val == string.Empty)
                        {
                            val = "0";
                        }
                        received = Convert.ToDouble(val);
                        unit = receivedamount / received;
                    }
                    q = "SELECT     TOP (1) Date,RemainingQuantity,QuantityConsumed FROM         InventoryConsumed WHERE     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId='" + dsrawitems.Tables[0].Rows[i]["RawItemId"].ToString() + "' ORDER BY Id DESC";
                    DataSet dsdate = new DataSet();
                    dsdate = objCore.funGetDataSet(q);
                    if (dsdate.Tables[0].Rows.Count > 0)
                    {
                        string val = dsdate.Tables[0].Rows[0]["RemainingQuantity"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        opening = Convert.ToDouble(val);
                        openingamount = opening * unit;
                        val = dsdate.Tables[0].Rows[0]["QuantityConsumed"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        consumed = Convert.ToDouble(val);
                        consumedamount = consumed * unit;
                        date = dsdate.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        string val = dsrawitems.Tables[0].Rows[0]["Quantity"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        opening = Convert.ToDouble(val);
                        consumed = 0;
                    }

                    q = "SELECT     id, itemid, quantity, type, Date, branchid FROM         Waste WHERE type='Raw Waste' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and itemid='" + dsrawitems.Tables[0].Rows[i]["RawItemId"].ToString() + "'";
                    dsdate = new DataSet();
                    dsdate = objCore.funGetDataSet(q);
                    if (dsdate.Tables[0].Rows.Count > 0)
                    {
                        string val = dsdate.Tables[0].Rows[0]["quantity"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        rawwaste = Convert.ToDouble(val);
                        rawwasteamount = rawwaste * unit;
                        
                    }
                    q = "SELECT     id, itemid, quantity, type, Date, branchid FROM         Waste WHERE type='Cook Waste' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and itemid='" + dsrawitems.Tables[0].Rows[i]["RawItemId"].ToString() + "'";
                    dsdate = new DataSet();
                    dsdate = objCore.funGetDataSet(q);
                    if (dsdate.Tables[0].Rows.Count > 0)
                    {
                        string val = dsdate.Tables[0].Rows[0]["quantity"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        coockwaste = Convert.ToDouble(val);
                        coockwasteamount = coockwaste * unit;

                    }
                    
                    q = "SELECT     id, itemid, date, quantity, physical, variance FROM         Variance WHERE (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and itemid='" + dsrawitems.Tables[0].Rows[i]["RawItemId"].ToString() + "'";
                    dsdate = new DataSet();
                    dsdate = objCore.funGetDataSet(q);
                    if (dsdate.Tables[0].Rows.Count > 0)
                    {
                        string val = dsdate.Tables[0].Rows[0]["variance"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        variance = Convert.ToDouble(val);
                        varianceamount = variance * unit;

                    }
                    closing = (opening + received) - (rawwaste + coockwaste) ;
                    //closing = closing - variance;
                    closingamount = (openingamount + receivedamount) - (rawwasteamount + coockwasteamount);
                    //closingamount = closingamount - varianceamount;
                    opening = opening + consumed;
                    openingamount = openingamount + closingamount;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(dsrawitems.Tables[0].Rows[i]["itemname"].ToString(), opening, openingamount, received, receivedamount, consumed, consumedamount, rawwaste, rawwasteamount, coockwaste, coockwasteamount, variance, varianceamount, closing, closingamount, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(dsrawitems.Tables[0].Rows[i]["itemname"].ToString(), opening, openingamount, received, receivedamount, consumed, consumedamount, rawwaste, rawwasteamount, coockwaste, coockwasteamount, variance, varianceamount, closing, closingamount, dscompany.Tables[0].Rows[0]["logo"]);
                  
                    }

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
