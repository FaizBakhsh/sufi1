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
    public partial class frmWastage : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmWastage()
        {
            InitializeComponent();
        }
       
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet dsi = new DataSet();
                string q = "select id,itemname from rawitem";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["itemname"] = "All";
                cbItem.DataSource = dsi.Tables[0];
                cbItem.ValueMember = "id";
                cbItem.DisplayMember = "itemname";
                cbItem.Text = "All";


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            
            }

            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                ds1.Tables[0].Rows.Add(dr1);
                cmbranch.DataSource = ds1.Tables[0];
                cmbranch.ValueMember = "id";
                cmbranch.DisplayMember = "branchname";

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
                POSRestaurant.Reports.Inventory.rptDiscard rptDoc = new rptDiscard();
                POSRestaurant.Reports.Inventory.dsdiscard dsrpt = new dsdiscard();
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
                
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", " For the period of " + dateTimePicker1.Text+" to "+dateTimePicker2.Text);


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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Discard", typeof(double));
                dtrpt.Columns.Add("Remaining", typeof(double));               
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("date", typeof(DateTime));
                dtrpt.Columns.Add("waste", typeof(double));
                dtrpt.Columns.Add("CompleteWaste", typeof(double));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }

                DataSet ds = new DataSet();
                string q = "", date = "" ;
                if (cbItem.Text == "All")
                {
                    q = "SELECT TOP (100) PERCENT SUM(dbo.Discard.Discard) AS Discard,SUM(dbo.Discard.staff) AS waste,SUM(dbo.Discard.completewaste) AS completewaste, dbo.RawItem.ItemName, dbo.RawItem.id FROM  dbo.RawItem INNER JOIN               dbo.Discard ON dbo.RawItem.Id = dbo.Discard.itemid   WHERE     (dbo.Discard.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY dbo.RawItem.ItemName, dbo.RawItem.id order by dbo.RawItem.ItemName";
                }
                else
                {
                    q = "SELECT TOP (100) PERCENT SUM(dbo.Discard.Discard) AS Discard,SUM(dbo.Discard.staff) AS waste,SUM(dbo.Discard.completewaste) AS completewaste, dbo.RawItem.ItemName, dbo.RawItem.id FROM  dbo.RawItem INNER JOIN               dbo.Discard ON dbo.RawItem.Id = dbo.Discard.itemid   WHERE     (dbo.Discard.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Discard.itemid='" + cbItem.SelectedValue + "' GROUP BY dbo.RawItem.ItemName, dbo.RawItem.id order by dbo.RawItem.ItemName";
                    //q = "SELECT     dbo.Discard.quantity AS quantity, dbo.Discard.date, dbo.RawItem.ItemName, dbo.Discard.Discard AS Discard, dbo.Discard.Remaining AS Remaining FROM         dbo.RawItem INNER JOIN                      dbo.Discard ON dbo.RawItem.Id = dbo.Discard.itemid  WHERE     (dbo.Discard.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Discard.itemid='" + cmbbranch.SelectedValue + "' order by dbo.Discard.date, dbo.RawItem.ItemName";
             
                }
                DataSet dsdate = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double price = getprice(ds.Tables[0].Rows[i]["id"].ToString());// *(Convert.ToDouble(ds.Tables[0].Rows[i]["Discard"].ToString()));
                    string dis = ds.Tables[0].Rows[i]["Discard"].ToString();
                    if (dis == "")
                    {
                        dis = "0";
                    }
                    string cwst = ds.Tables[0].Rows[i]["completewaste"].ToString();
                    if (cwst == "")
                    {
                        cwst = "0";
                    }
                    string wst = ds.Tables[0].Rows[i]["waste"].ToString();
                    if (wst == "")
                    {
                        wst = "0";
                    }
                    double total = Convert.ToDouble(dis) + Convert.ToDouble(cwst) + Convert.ToDouble(wst);
                    total = total * price;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), total, ds.Tables[0].Rows[i]["Discard"].ToString(), 0, null, Convert.ToDateTime(dateTimePicker1.Text), wst, cwst);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), total, ds.Tables[0].Rows[i]["Discard"].ToString(), 0, dscompany.Tables[0].Rows[0]["logo"], Convert.ToDateTime(dateTimePicker1.Text), wst, cwst);
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
        private double getprice(string id)
        {
            if (id == "69")
            {

            }
            double variance = 0, price = 0;
            string val = "";
            DataSet dspurchase = new DataSet();
            string q = "";

            q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryTransfer.ItemId = '" + id + "'  GROUP BY MONTH(Date), YEAR(Date)";

            dspurchase = objCore.funGetDataSet(q);
            for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
            {
                val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = price + Convert.ToDouble(val);
            }
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                price = price / dspurchase.Tables[0].Rows.Count;
            }
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT    top 1    (price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date <= '" + dateTimePicker1.Text + "' ) and dbo.InventoryTransfer.ItemId = '" + id + "'  order by date desc ";

                dspurchase = objCore.funGetDataSet(q);
                for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
                {
                    val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = price + Convert.ToDouble(val);
                }

            }
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";

                dspurchase = objCore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = Convert.ToDouble(val);
                }
                if (price == 0)
                {
                    dspurchase = new DataSet();

                    q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";

                    dspurchase = objCore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        val = dspurchase.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        price = Convert.ToDouble(val);
                    }
                }
                if (price == 0)
                {
                    dspurchase = new DataSet();
                    q = "select price from rawitem where id='" + id + "'";
                    dspurchase = objCore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            val = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            price = Convert.ToDouble(val);
                        }
                        catch (Exception ez)
                        {


                        }
                    }
                }
            }
            return price;
        }
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
