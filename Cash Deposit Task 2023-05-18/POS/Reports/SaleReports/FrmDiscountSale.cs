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
    public partial class FrmDiscountSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmDiscountSale()
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
                string q = "select name+'('+cast(discount as varchar(100))+')' as discount,id from DiscountKeys";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["discount"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "discount";
                comboBox1.Text = "All";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                ds = new DataSet();
                string q = "select Id, Name from Shifts";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox2.DataSource = ds.Tables[0];
                comboBox2.ValueMember = "Id";
                comboBox2.DisplayMember = "Name";
                comboBox2.Text = "All";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            fillcus();
            
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        protected void fillcus()
        {
            comboBox3.Text = "All";
            //try
            //{
            //    ds = new DataSet();
            //    string q = "";
            //    if (comboBox1.Text == "All")
            //    {
            //        q = "select distinct customer from sale where discount>0";
            //    }
            //    else
            //    {
            //        q = "select distinct customer from sale where discount='"+comboBox1.Text+"'";
            //    }
            //    ds = objCore.funGetDataSet(q);
            //    DataRow dr = ds.Tables[0].NewRow();
            //    dr["customer"] = "All";
            //    ds.Tables[0].Rows.Add(dr);
            //    comboBox3.DataSource = ds.Tables[0];
            //    comboBox3.ValueMember = "customer";
            //    comboBox3.DisplayMember = "customer";
            //    comboBox3.Text = "All";
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }
        public void bindreport()
        {

            try
            {
                getcompany();
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rprDiscountSale rptDoc = new  rprDiscountSale();
                POSRestaurant.Reports.SaleReports.DsDiscountsale dsrpt = new DsDiscountsale();
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
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
                  
        }
        public string getamount(string itmid,int qty, int perc)
        {
            double amount = 0;
            try
            {
                DataSet dsitem = new DataSet();
                string q = "SELECT     SUM(dbo.Saledetails.Price) AS price FROM         dbo.Saledetails INNER JOIN dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.Discount > 0) AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
                dsitem = objCore.funGetDataSet(q);
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    string val = "";
                    val = dsitem.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";

                    }
                    amount =  Convert.ToDouble(val);
                    amount = (amount * perc) / 100;
                }
            }
            catch (Exception ex)
            {
                
               
            }
            return amount.ToString();
        }
        protected string getname(string id)
        {
            string name = "";

            try
            {
                DataSet dsitem = new DataSet();
                string q = "select name from DiscountKeys  where id='" + id + "'";
                dsitem = objCore.funGetDataSet(q);
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    
                    name = dsitem.Tables[0].Rows[0][0].ToString();
                    
                }
            }
            catch (Exception ex)
            {


            }

            return name;

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
                dtrpt.Columns.Add("Perc", typeof(string));
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("discount", typeof(string));
                dtrpt.Columns.Add("Narration", typeof(string));
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


                if (comboBox2.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {

                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.Sale.DiscountNaration, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount, dbo.ModifierFlavour.name AS Expr1,                          dbo.DiscountKeys.name AS Expr2 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (Sale.branchid = '" + cmbbranch.SelectedValue + "')  AND (Sale.Discount > 0)  GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.DiscountKeys.name";
                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.Sale.DiscountNaration, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount,                          dbo.ModifierFlavour.name AS Expr1, dbo.Sale.discountkeyid FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE          (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (Sale.branchid = '" + cmbbranch.SelectedValue + "')  AND (Sale.Discount > 0) and sale.billstatus='Paid'  GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.Sale.discountkeyid";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.Sale.DiscountNaration,dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount, dbo.ModifierFlavour.name AS Expr1,                          dbo.DiscountKeys.name AS Expr2 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (Sale.discountkeyid = '" + comboBox1.SelectedValue + "')  GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.Sale.discountkeyid";
                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.Sale.DiscountNaration, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount,                          dbo.ModifierFlavour.name AS Expr1, dbo.Sale.discountkeyid FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE           (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (Sale.discountkeyid = '" + comboBox1.SelectedValue + "')  and sale.billstatus='Paid'  GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.Sale.discountkeyid";
                   
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {

                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.Sale.DiscountNaration,dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount, dbo.ModifierFlavour.name AS Expr1,                          dbo.DiscountKeys.name AS Expr2 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "') and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'  AND (Sale.Discount > 0) GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name, dbo.Sale.DiscountNaration,dbo.Sale.discountkeyid";
                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.Sale.DiscountNaration, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount,                          dbo.ModifierFlavour.name AS Expr1, dbo.Sale.discountkeyid FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "') and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'  AND (Sale.Discount > 0)  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name, dbo.Sale.DiscountNaration,dbo.Sale.discountkeyid";
                   
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer,dbo.Sale.DiscountNaration, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount, dbo.ModifierFlavour.name AS Expr1,                          dbo.DiscountKeys.name AS Expr2 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (Sale.branchid = '" + cmbbranch.SelectedValue + "') and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "' AND (Sale.discountkeyid = '" + comboBox1.SelectedValue + "')  GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.Sale.discountkeyid";
                        q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.Sale.DiscountNaration, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount,                          dbo.ModifierFlavour.name AS Expr1, dbo.Sale.discountkeyid FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (Sale.branchid = '" + cmbbranch.SelectedValue + "') and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "' AND (Sale.discountkeyid = '" + comboBox1.SelectedValue + "')  and sale.billstatus='Paid'   GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.Sale.discountkeyid";
                   
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string amount = ds.Tables[0].Rows[i]["dis"].ToString(); ;// getamount(ds.Tables[0].Rows[i]["id"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[i]["count"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["discount"].ToString()));
                    if (amount == "")
                    {
                        amount = "0";
                    }
                    string qty = ds.Tables[0].Rows[i]["count"].ToString();
                    if (qty == "")
                    {
                        qty = "0";
                    }
                    string keyname = getname(ds.Tables[0].Rows[i]["discountkeyid"].ToString());
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr1"].ToString() + " " + ds.Tables[0].Rows[i]["Name"].ToString(), qty, amount, null, ds.Tables[0].Rows[i]["discount"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString(), keyname, ds.Tables[0].Rows[i]["DiscountNaration"].ToString());
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr1"].ToString() + " " + ds.Tables[0].Rows[i]["Name"].ToString(), qty, amount, dscompany.Tables[0].Rows[0]["logo"], ds.Tables[0].Rows[i]["discount"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString(), keyname, ds.Tables[0].Rows[i]["DiscountNaration"].ToString());
                    }
                }
                if (comboBox3.Text == "All" || comboBox3.Text=="Individual")
                {
                    if (comboBox2.Text == "All")
                    {
                        if (comboBox1.Text == "All")
                        {

                            q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.MenuItem.Name,dbo.Sale.Customer,dbo.Sale.DiscountNaration, dbo.ModifierFlavour.name AS Expr1, dbo.DiscountKeys.name AS Expr2, dbo.DiscountKeys.discount FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id INNER JOIN                         dbo.Saledetails ON dbo.DiscountIndividual.Saledetailsid = dbo.Saledetails.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "')  and sale.billstatus='Paid'  GROUP BY dbo.Sale.Customer,dbo.DiscountKeys.name, dbo.MenuItem.Name, dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.DiscountKeys.discount";
                            //q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount, dbo.ModifierFlavour.name AS Expr1,                          dbo.DiscountKeys.name AS Expr2 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (Sale.Discount > 0)  AND (Sale.Customer = '" + comboBox3.Text + "') AND  (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0') GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name, dbo.DiscountKeys.name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.Sale.Customer, dbo.MenuItem.Name, dbo.MenuItem.Id, dbo.Sale.Discount AS discount, dbo.ModifierFlavour.name AS Expr1,                          dbo.DiscountKeys.name AS Expr2 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  AND (Sale.discountkeyid = '" + comboBox1.SelectedValue + "') AND (Sale.Customer = '" + comboBox3.Text + "')  AND  (dbo.Saledetails.ModifierId = '0') AND                        (dbo.Saledetails.RunTimeModifierId = '0') GROUP BY dbo.MenuItem.Name,dbo.Sale.Discount,dbo.MenuItem.id, Sale.Customer,dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.DiscountKeys.name";
                            q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.MenuItem.Name,dbo.Sale.Customer, dbo.ModifierFlavour.name AS Expr1, dbo.DiscountKeys.name AS Expr2, dbo.DiscountKeys.discount FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id INNER JOIN                         dbo.Saledetails ON dbo.DiscountIndividual.Saledetailsid = dbo.Saledetails.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "')  AND (dbo.DiscountKeys.id = '" + comboBox1.SelectedValue + "')  and sale.billstatus='Paid'  GROUP BY dbo.Sale.Customer,dbo.DiscountKeys.name, dbo.MenuItem.Name, dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.DiscountKeys.discount";
                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All")
                        {


                            q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.MenuItem.Name,dbo.Sale.Customer,dbo.Sale.DiscountNaration, dbo.ModifierFlavour.name AS Expr1, dbo.DiscountKeys.name AS Expr2, dbo.DiscountKeys.discount FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id INNER JOIN                         dbo.Saledetails ON dbo.DiscountIndividual.Saledetailsid = dbo.Saledetails.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "')  and sale.billstatus='Paid'  and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'   GROUP BY dbo.Sale.Customer,dbo.DiscountKeys.name, dbo.MenuItem.Name, dbo.ModifierFlavour.name,dbo.Sale.DiscountNaration, dbo.DiscountKeys.discount";

                        }
                        else
                        {

                            q = "SELECT        SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, dbo.MenuItem.Name,dbo.Sale.Customer,dbo.Sale.DiscountNaration, dbo.ModifierFlavour.name AS Expr1, dbo.DiscountKeys.name AS Expr2, dbo.DiscountKeys.discount FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id INNER JOIN                         dbo.Saledetails ON dbo.DiscountIndividual.Saledetailsid = dbo.Saledetails.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   AND (Sale.branchid = '" + cmbbranch.SelectedValue + "')  and sale.billstatus='Paid'  and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'  AND (dbo.DiscountKeys.id = '" + comboBox1.SelectedValue + "')   GROUP BY dbo.Sale.Customer,dbo.DiscountKeys.name, dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.Sale.DiscountNaration,dbo.DiscountKeys.discount";

                        }
                    }
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string amount = ds.Tables[0].Rows[i]["dis"].ToString(); ;// getamount(ds.Tables[0].Rows[i]["id"].ToString(), Convert.ToInt32(ds.Tables[0].Rows[i]["count"].ToString()), Convert.ToInt32(ds.Tables[0].Rows[i]["discount"].ToString()));
                        if (amount == "")
                        {
                            amount = "0";
                        }
                        string qty = ds.Tables[0].Rows[i]["count"].ToString();
                        if (qty == "")
                        {
                            qty = "0";
                        }
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr1"].ToString() + " " + ds.Tables[0].Rows[i]["Name"].ToString(), qty, amount, null, ds.Tables[0].Rows[i]["discount"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString(), ds.Tables[0].Rows[i]["Expr2"].ToString(), ds.Tables[0].Rows[i]["DiscountNaration"].ToString());
                        }
                        else
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr1"].ToString() + " " + ds.Tables[0].Rows[i]["Name"].ToString(), qty, amount, dscompany.Tables[0].Rows[0]["logo"], ds.Tables[0].Rows[i]["discount"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString(), ds.Tables[0].Rows[i]["Expr2"].ToString(), ds.Tables[0].Rows[i]["DiscountNaration"].ToString());
                        }
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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillcus();
        }
    }
}
