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
    public partial class FrmInvoiceDetailsSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public string saleid = "";
        public FrmInvoiceDetailsSale()
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
            try
            {
                ds = new DataSet();
                string q = "select id,name from shifts ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbshift.DataSource = ds.Tables[0];
                cmbshift.ValueMember = "id";
                cmbshift.DisplayMember = "name";
                cmbshift.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            if (saleid.Length > 0)
            {
                panel2.Visible = false;
                bindreport();
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;
            }

        }
       
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptinvoices rptDoc = new rptinvoices();
                POSRestaurant.Reports.SaleReports.dsinvoice dsrpt = new dsinvoice();
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
                
                rptDoc.SetParameterValue("date", "for the period  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                rptDoc.SetParameterValue("branch", dscompany.Tables[0].Rows[0]["address"]);
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

                dtrpt.Columns.Add("InvoiceNo", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("GST", typeof(double));
                dtrpt.Columns.Add("Discount", typeof(double));
                dtrpt.Columns.Add("Net", typeof(double));
                dtrpt.Columns.Add("date", typeof(DateTime));
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


                if (saleid.Length > 0)
                {
                    q = "SELECT        dbo.Sale.Id, dbo.Sale.Date, dbo.Shifts.Name AS shift, dbo.ModifierFlavour.name AS Flavour, dbo.MenuItem.Name, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS RModifier, dbo.Saledetails.Quantity,                          dbo.Saledetails.Price, dbo.Saledetails.Itemdiscount, dbo.Saledetails.ItemGst FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id where dbo.sale.id = '" + saleid + "'";
                 
                }
                else
                {
                    if (cmbshift.Text == "All")
                    {
                        q = "SELECT        dbo.Sale.Id, dbo.Sale.Date, dbo.Shifts.Name AS shift, dbo.ModifierFlavour.name AS Flavour, dbo.MenuItem.Name, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS RModifier, dbo.Saledetails.Quantity,                          dbo.Saledetails.Price, dbo.Saledetails.Itemdiscount, dbo.Saledetails.ItemGst FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   order by dbo.sale.id";
                    }
                    else
                    {
                        q = "SELECT        dbo.Sale.Id, dbo.Sale.Date, dbo.Shifts.Name AS shift, dbo.ModifierFlavour.name AS Flavour, dbo.MenuItem.Name, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS RModifier, dbo.Saledetails.Quantity,                          dbo.Saledetails.Price, dbo.Saledetails.Itemdiscount, dbo.Saledetails.ItemGst FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' order by dbo.sale.id";
                    }
                }

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double amount = 0, total = 0, gst = 0, dis = 0, qty = 0;
                    string val = "";
                    val = ds.Tables[0].Rows[i]["Itemdiscount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    dis = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["Quantity"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    qty = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["ItemGst"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    gst = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["Price"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    amount = Convert.ToDouble(val);

                    total = (amount + gst) - dis;
                    string name = "";
                    name = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (ds.Tables[0].Rows[i]["RModifier"].ToString() != "")
                    {
                        name = "  " + ds.Tables[0].Rows[i]["RModifier"].ToString();
                    }
                    else
                        if (ds.Tables[0].Rows[i]["Modifier"].ToString() != "")
                        {
                            name = "  " + ds.Tables[0].Rows[i]["Modifier"].ToString();
                        }
                        else
                            if (ds.Tables[0].Rows[i]["Flavour"].ToString() != "")
                            {
                                name = ds.Tables[0].Rows[i]["Flavour"].ToString() + " " + name; ;
                            }
                    try
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), name, qty, amount, gst, dis, total, ds.Tables[0].Rows[i]["date"].ToString());

                    }
                    catch (Exception wx)
                    {


                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
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
