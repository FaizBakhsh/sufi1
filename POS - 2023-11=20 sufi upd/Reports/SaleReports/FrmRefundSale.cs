using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmRefundSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmRefundSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                // ds1.Tables[0].Rows.Add(dr1);
                cmbbranch.DataSource = ds1.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";

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


                POSRestaurant.Reports.SaleReports.rprRefundSale rptDoc = new rprRefundSale();
                POSRestaurant.Reports.SaleReports.DsRefundSale dsrpt = new DsRefundSale();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
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
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn",phone );
                rptDoc.SetParameterValue("date", "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);


                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
                  
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }

        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("MenuItem", typeof(string));
                dtrpt.Columns.Add("Count", typeof(double));
                dtrpt.Columns.Add("Sum", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("GST", typeof(double));
                dtrpt.Columns.Add("Net", typeof(double));
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("InvoiceNo", typeof(string));               
                dtrpt.Columns.Add("Type", typeof(string));
                dtrpt.Columns.Add("Time", typeof(string));
                dtrpt.Columns.Add("BillTime", typeof(DateTime));
                dtrpt.Columns.Add("Reason", typeof(string));
                dtrpt.Columns.Add("Making", typeof(string));
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
                string q = "";
                if (comboBox1.Text == "All")
                {
                    if (chkdate.Checked == true)
                    {
                        if (chkinvoice.Checked == true)
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Sale.Date,dbo.Sale.time as BillTime,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.billstatus='Paid' GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Sale.Date,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus order by dbo.Sale.Date,dbo.MenuItem.Name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Sale.Date,dbo.Sale.time,dbo.Saledetailsrefund.time as BillTime,dbo.Saledetailsrefund.type,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.sale.billstatus='Paid'  GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Sale.Date,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus  order by dbo.Sale.Date,dbo.MenuItem.Name";
                        }
                    }
                    else
                    {

                        if (chkinvoice.Checked == true)
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time as BillTime,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.sale.billstatus='Paid'  GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus  order by dbo.MenuItem.Name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time as BillTime,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.sale.billstatus='Paid'   GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus  order by dbo.MenuItem.Name";

                        }
                    }
                }
                else
                {
                    if (chkdate.Checked == true)
                    {
                        if (chkinvoice.Checked == true)
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Sale.Date,dbo.Sale.time as BillTime,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.sale.billstatus='Paid'  and dbo.Saledetailsrefund.type='" + comboBox1.Text + "'  GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Sale.Date,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Sale.time ,dbo.Saledetailsrefund.type,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus order by dbo.Sale.Date,dbo.MenuItem.Name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Sale.Date,dbo.Sale.time as BillTime,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'    and dbo.sale.billstatus='Paid'  and dbo.Saledetailsrefund.type='" + comboBox1.Text + "'  GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Sale.Date,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus  order by dbo.Sale.Date,dbo.MenuItem.Name";

                        }
                    }
                    else
                    {

                        if (chkinvoice.Checked == true)
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Sale.time as BillTime,dbo.Saledetailsrefund.type,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.sale.billstatus='Paid'  and dbo.Saledetailsrefund.type='" + comboBox1.Text + "'  GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Sale.id,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time,dbo.Saledetailsrefund.Reason ,dbo.Saledetailsrefund.MakeStatus order by dbo.MenuItem.Name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetailsrefund.Price) AS sum, SUM(dbo.Saledetailsrefund.Quantity) AS count, SUM(dbo.Saledetailsrefund.Itemdiscount) AS dis, SUM(dbo.Saledetailsrefund.ItemGst) AS gs, dbo.MenuItem.Name,                          dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1,dbo.Saledetailsrefund.time,dbo.Sale.time as BillTime, dbo.Saledetailsrefund.type,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetailsrefund.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.sale.billstatus='Paid'  and dbo.Saledetailsrefund.type='" + comboBox1.Text + "'  GROUP BY  dbo.MenuItem.Name,dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Saledetailsrefund.time,dbo.Saledetailsrefund.type,dbo.Sale.time,dbo.Saledetailsrefund.Reason,dbo.Saledetailsrefund.MakeStatus  order by dbo.MenuItem.Name";

                        }
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double gst = Convert.ToDouble(ds.Tables[0].Rows[i]["gs"].ToString());
                    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString());// getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                    double discount = Convert.ToDouble(ds.Tables[0].Rows[i]["dis"].ToString()); 
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    string date = "", invoice = "";
                    try
                    {
                        invoice = ds.Tables[0].Rows[i]["id"].ToString();
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                    try
                    {
                        date =Convert.ToDateTime( ds.Tables[0].Rows[i]["date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {


                    }
                    sum = sum + gst;
                    double net = sum - gst;
                    net = net - discount;
                    sum = Math.Round(sum, 2);
                    net = Math.Round(net, 2);
                    string name = ds.Tables[0].Rows[i]["Expr1"].ToString();
                    if (name.Length > 0)
                    {
                        name = name + " ' " + ds.Tables[0].Rows[i]["Name"].ToString();
                    }
                    else
                    {
                        name = ds.Tables[0].Rows[i]["Name"].ToString();
                    }
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(name, quantity.ToString(), ds.Tables[0].Rows[i]["sum"].ToString(), null, gst, net, date, invoice, ds.Tables[0].Rows[i]["type"].ToString(), ds.Tables[0].Rows[i]["time"].ToString(), ds.Tables[0].Rows[i]["Billtime"].ToString(), ds.Tables[0].Rows[i]["Reason"].ToString(), ds.Tables[0].Rows[i]["MakeStatus"].ToString());
                    }
                    else
                    {
                        dtrpt.Rows.Add(name, quantity.ToString(), ds.Tables[0].Rows[i]["sum"].ToString(), dscompany.Tables[0].Rows[0]["logo"], gst, net, date, invoice, ds.Tables[0].Rows[i]["type"].ToString(), ds.Tables[0].Rows[i]["time"].ToString(), ds.Tables[0].Rows[i]["Billtime"].ToString(), ds.Tables[0].Rows[i]["Reason"].ToString(), ds.Tables[0].Rows[i]["MakeStatus"].ToString());
                    }
                    
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        public double getquantity(string itmid)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  AND (dbo.Saledetails.MenuItemId = '" + itmid + "') and      (dbo.Sale.BillStatus = 'Refund')  AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
                DataSet dsgetq = new DataSet();
                dsgetq = objCore.funGetDataSet(q);
                if (dsgetq.Tables[0].Rows.Count > 0)
                {
                    string val = "";
                    val = dsgetq.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    qty = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            return qty;
        }
        public double getgst(string itmid)
        {
            double gst = 0;
            DataSet dsgst = new DataSet();
            string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  AND (dbo.Saledetails.MenuItemId = '" + itmid + "') and      (dbo.Sale.BillStatus = 'Refund') ";
            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                gst = Convert.ToDouble(val);
            }
            return gst;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                comboBox1.SelectedValue = "All";
            }
            bindreport();
        }
    }
}
