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
    public partial class FrmcustomerSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmcustomerSale()
        {
            InitializeComponent();
        }
        public string saleid = "";
        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select id,name from customers order by name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                comboBox1.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            if (saleid.Length > 0)
            {
                dateTimePicker1.Visible = false;
                dateTimePicker2.Visible = false;
                comboBox1.Visible = false;
                vButton1.Visible = false;
                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                
                bindreport();
            }
        }
       
        
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptCustomersale rptDoc = new rptCustomersale();
                POSRestaurant.Reports.SaleReports.dscustomersale dsrpt = new dscustomersale();
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
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                rptDoc.SetParameterValue("report", "Customer Wise Sales Report");
                
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
                dtrpt.Columns.Add("billno", typeof(string));
                dtrpt.Columns.Add("Item", typeof(string));
                dtrpt.Columns.Add("qty", typeof(double));
                dtrpt.Columns.Add("dis", typeof(double));
                dtrpt.Columns.Add("gst", typeof(double));
                dtrpt.Columns.Add("amount", typeof(double));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("date", typeof(DateTime));
                dtrpt.Columns.Add("customer", typeof(string));
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
                if (saleid == "")
                {
                    if (comboBox1.Text == "All")
                    {

                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Id,dbo.Sale.date, dbo.Saledetails.ItemGst AS gs,                          dbo.MenuItem.Name + '  ' + dbo.Saledetails.comments AS name, dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1, dbo.Saledetails.comments, dbo.Sale.Customer FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.Sale.Customer = dbo.Customers.Name LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    order by dbo.MenuItem.Name  ";
                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Id, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs,                          dbo.MenuItem.Name + '  ' + dbo.Saledetails.comments AS name, dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1, dbo.Saledetails.comments, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and     (dbo.Sale.BillStatus = 'paid')   and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    order by dbo.MenuItem.Name  ";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Id,dbo.Sale.date, dbo.Saledetails.ItemGst AS gs,                          dbo.MenuItem.Name + '  ' + dbo.Saledetails.comments AS name, dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1, dbo.Saledetails.comments, dbo.Sale.Customer FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.Sale.Customer = dbo.Customers.Name LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.customer='" + comboBox1.Text + "' and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    order by dbo.MenuItem.Name  ";
                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Id, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs,                          dbo.MenuItem.Name + '  ' + dbo.Saledetails.comments AS name, dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1, dbo.Saledetails.comments, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.BillType.recvid ='" + comboBox1.SelectedValue + "'  and     (dbo.Sale.BillStatus = 'paid')   and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    order by dbo.MenuItem.Name  ";

                    }
                }
                else
                {
                    q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Id, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs,                          dbo.MenuItem.Name + '  ' + dbo.Saledetails.comments AS name, dbo.MenuItem.Id AS mid, dbo.ModifierFlavour.name AS Expr1, dbo.Saledetails.comments, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (Sale.id='" + saleid + "')   and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    order by dbo.MenuItem.Name  ";
                 
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    val = "";
                    //val = ds.Tables[0].Rows[i]["discount"].ToString();
                    //if (val == "")
                    //{
                    //    val = "0";
                    //}                    
                    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString());// getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                    double discount = Convert.ToDouble(ds.Tables[0].Rows[i]["dis"].ToString()); //getdiscount(ds.Tables[0].Rows[i]["mid"].ToString()); //Convert.ToDouble(val);
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }
                    
                    discount = Math.Round(discount, 2);
                    val = "";
                    double gst = Convert.ToDouble(ds.Tables[0].Rows[i]["gs"].ToString()); //getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                    //gst = ((sum) * gst) / 100;
                    sum = sum + gst;
                    gst = Math.Round(gst, 2);
                    double net = sum;// -gst;
                    net = net - discount;
                    net = Math.Round(net, 2);
                    sum = Math.Round(sum, 2);
                    string sz = ds.Tables[0].Rows[i]["Expr1"].ToString();
                   
                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();

                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), sz+" "+nm, quantity, discount, gst, net, null, ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["customer"].ToString());
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), sz + " " + nm, quantity, discount, gst, net, dscompany.Tables[0].Rows[0]["logo"], ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["customer"].ToString());
                    }                    
                }

                ds = new DataSet();

                if (saleid == "")
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs, dbo.Saledetails.comments, dbo.Sale.Id,                          dbo.Modifier.Name, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id WHERE       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId>0 and dbo.Saledetails.RunTimeModifierId=0 ";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs, dbo.Saledetails.comments, dbo.Sale.Id,                          dbo.Modifier.Name, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id WHERE       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId>0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.BillType.recvid ='" + comboBox1.SelectedValue + "' ";

                    }
                }
                else
                {
                    q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs, dbo.Saledetails.comments, dbo.Sale.Id,                          dbo.Modifier.Name, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id WHERE       (Sale.id='"+saleid+"')  and dbo.Saledetails.ModifierId>0 and dbo.Saledetails.RunTimeModifierId=0 ";
             
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    val = "";
                                     
                    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString());// getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                    double discount = Convert.ToDouble(ds.Tables[0].Rows[i]["dis"].ToString()); //getdiscount(ds.Tables[0].Rows[i]["mid"].ToString()); //Convert.ToDouble(val);
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }

                    discount = Math.Round(discount, 2);
                    val = "";
                    double gst = Convert.ToDouble(ds.Tables[0].Rows[i]["gs"].ToString()); //getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                    //gst = ((sum) * gst) / 100;
                    sum = sum + gst;
                    gst = Math.Round(gst, 2);
                    double net = sum;// -gst;
                    net = net - discount;
                    net = Math.Round(net, 2);
                    sum = Math.Round(sum, 2);
                   
                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (sum > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), nm, quantity, discount, gst, net, null, ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["customer"].ToString());
                        }
                        else
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), nm, quantity, discount, gst, net, dscompany.Tables[0].Rows[0]["logo"], ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["customer"].ToString());
                        }  
                    }
                }


                ds = new DataSet();

                if (saleid == "")
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs, dbo.Saledetails.comments, dbo.Sale.Id,                          dbo.RuntimeModifier.name, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                        dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id WHERE    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and      (dbo.Sale.BillStatus = 'paid') AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.RunTimeModifierId > 0)";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs, dbo.Saledetails.comments, dbo.Sale.Id,                          dbo.RuntimeModifier.name, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                        dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id WHERE    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and      (dbo.Sale.BillStatus = 'paid') AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.RunTimeModifierId > 0)  and dbo.BillType.recvid ='" + comboBox1.SelectedValue + "' ";

                    }
                }
                else
                {
                    q = "SELECT        TOP (100) PERCENT dbo.Saledetails.Price AS sum, dbo.Saledetails.Quantity AS count, dbo.Saledetails.Itemdiscount AS dis, dbo.Sale.Date, dbo.Saledetails.ItemGst AS gs, dbo.Saledetails.comments, dbo.Sale.Id,                          dbo.RuntimeModifier.name, dbo.Customers.Name AS Customer FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                        dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id WHERE    (Sale.id= '" + saleid + "') and (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.RunTimeModifierId > 0)";
                
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    val = "";

                    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString());// getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                    double discount = Convert.ToDouble(ds.Tables[0].Rows[i]["dis"].ToString()); //getdiscount(ds.Tables[0].Rows[i]["mid"].ToString()); //Convert.ToDouble(val);
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }

                    discount = Math.Round(discount, 2);
                    val = "";
                    double gst = Convert.ToDouble(ds.Tables[0].Rows[i]["gs"].ToString()); //getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                    //gst = ((sum) * gst) / 100;
                    sum = sum + gst;
                    gst = Math.Round(gst, 2);
                    double net = sum;// -gst;
                    net = net - discount;
                    net = Math.Round(net, 2);
                    sum = Math.Round(sum, 2);

                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (sum > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), nm, quantity, discount, gst, net, null, ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["customer"].ToString());
                        }
                        else
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), nm, quantity, discount, gst, net, dscompany.Tables[0].Rows[0]["logo"], ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["customer"].ToString());
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
    }
}
