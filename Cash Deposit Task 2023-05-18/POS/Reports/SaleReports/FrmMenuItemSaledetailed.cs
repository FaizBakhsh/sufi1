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
    public partial class FrmMenuItemSaledetailed : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmMenuItemSaledetailed()
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
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                comboBox1.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                ds = new DataSet();
                string q = "select id,name from kds ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbkds.DataSource = ds.Tables[0];
                cmbkds.ValueMember = "id";
                cmbkds.DisplayMember = "name";
                cmbkds.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        //public double getquantity(string itmid)
        //{
        //    double qty = 0;
        //    try
        //    {
        //        string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
        //        DataSet dsgetq = new DataSet();
        //        dsgetq = objCore.funGetDataSet(q);
        //        if (dsgetq.Tables[0].Rows.Count > 0)
        //        {
        //            string val = "";
        //            val = dsgetq.Tables[0].Rows[0][0].ToString();
        //            if (val == "")
        //            {
        //                val = "0";
        //            }
        //            qty = Convert.ToDouble(val);
        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return qty;
        //}
        //public double getprice(string itmid)
        //{
        //    double price = 0;
        //    try
        //    {
        //        string q = "SELECT     (dbo.Saledetails.price)  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE    (dbo.Saledetails.MenuItemId = '" + itmid + "')";
        //        DataSet dsgetq = new DataSet();
        //        dsgetq = objCore.funGetDataSet(q);
        //        if (dsgetq.Tables[0].Rows.Count > 0)
        //        {
        //            string val = "";
        //            val = dsgetq.Tables[0].Rows[0][0].ToString();
        //            if (val == "")
        //            {
        //                val = "0";
        //            }
        //            price = Convert.ToDouble(val);
        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    return price;
        //}
        //public double getgst(string itmid)
        //{
        //    double gst = 0;
        //    DataSet dsgst = new DataSet();
        //    string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
        //    dsgst = objCore.funGetDataSet(q);
        //    if (dsgst.Tables[0].Rows.Count > 0)
        //    {
        //        string val = "";
        //        val = dsgst.Tables[0].Rows[0][0].ToString();
        //        if (val == "")
        //        {
        //            val = "0";
        //        }
        //        gst = Convert.ToDouble(val);
        //    }
        //    return gst;
        //}
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptmenuitemdetailed rptDoc = new rptmenuitemdetailed();
                POSRestaurant.Reports.SaleReports.dsmenuitemdetailed dsrpt = new dsmenuitemdetailed();
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

                        dt.Rows.Add("", "0", "0", "", dscompany.Tables[0].Rows[0]["logo"], "0", "0");
                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "for the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        //public double getdiscount(string itmid)
        //{
        //    double discount = 0;
        //    DataSet dsgst = new DataSet();
        //    // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
        //    string q = "SELECT     SUM(dbo.Sale.Discount) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";

        //    dsgst = objCore.funGetDataSet(q);
        //    if (dsgst.Tables[0].Rows.Count > 0)
        //    {
        //        string val = "";
        //        val = dsgst.Tables[0].Rows[0][0].ToString();
        //        if (val == "")
        //        {
        //            val = "0";
        //        }
        //        discount = Convert.ToDouble(val);
        //    }
        //    return discount;
        //}
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        protected double getitemprice(string id)
        {
            double price = 0;

            try
            {
                string q = "SELECT        dbo.MenuItem.Price, dbo.GST.GST FROM            dbo.MenuItem CROSS JOIN                         dbo.GST  where dbo.menuitem.id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    price = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["price"].ToString()) * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                }
            }
            catch (Exception ex)
            {


            }
            return price;
        }
        protected double getitempricef(string id)
        {
            double price = 0;

            try
            {
                string q = "SELECT        dbo.GST.GST, dbo.ModifierFlavour.price FROM            dbo.ModifierFlavour CROSS JOIN                         dbo.GST  where dbo.ModifierFlavour.id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    price = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["price"].ToString()) * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                }
            }
            catch (Exception ex)
            {


            }
            return price;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("MenuItem", typeof(string));
                dtrpt.Columns.Add("Count", typeof(double));
                dtrpt.Columns.Add("Sum", typeof(double));
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("GST", typeof(double));
                dtrpt.Columns.Add("Net", typeof(double));
                dtrpt.Columns.Add("dis", typeof(double));
                dtrpt.Columns.Add("size", typeof(string));
                dtrpt.Columns.Add("price", typeof(double));
                dtrpt.Columns.Add("group", typeof(string));
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

               
                if (cmbkds.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name + '-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                        }

                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                        }
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid' and dbo.MenuItem.kdsid='" + cmbkds.SelectedValue + "'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.MenuItem.kdsid='" + cmbkds.SelectedValue + "'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.kdsid='" + cmbkds.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                        }

                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.kdsid='" + cmbkds.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                        }
                    } 
                }
                
                
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double price = 0;
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    val = "";
                    val = ds.Tables[0].Rows[i]["fid"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    if (val == "0")
                    {
                        price = getitemprice(ds.Tables[0].Rows[i]["mid"].ToString());
                    }
                    else
                    {
                        price = getitempricef(ds.Tables[0].Rows[i]["fid"].ToString());
                    }                  
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
                    double net = sum - gst;
                    net = net - discount;
                    net = Math.Round(net, 2);
                    sum = Math.Round(sum, 2);
                    string sz = ds.Tables[0].Rows[i]["Expr1"].ToString();

                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, null, gst, net, discount, sz, price, nm);
                    }
                    else
                    {
                        dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, sz, price, nm);
                    }
                    string flid = ds.Tables[0].Rows[i]["fid"].ToString();

                    if (flid == "" || flid == "0")
                    {
                        q = "SELECT        dbo.MenuItem.Name, dbo.ModifierFlavour.name AS size,dbo.Attachmenu1.quantity FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.attachmenuid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id where dbo.Attachmenu1.menuitemid='" + ds.Tables[0].Rows[i]["mid"].ToString() + "'  and dbo.Attachmenu1.status='Active'";

                    }
                    else
                    {
                        q = "SELECT        dbo.MenuItem.Name, dbo.ModifierFlavour.name AS size,dbo.Attachmenu1.quantity FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.attachmenuid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id where dbo.Attachmenu1.menuitemid='" + ds.Tables[0].Rows[i]["mid"].ToString() + "' and dbo.Attachmenu1.Flavourid='" + ds.Tables[0].Rows[i]["fid"].ToString() + "'   and dbo.Attachmenu1.status='Active'";


                    }
                    DataSet dsattach = new DataSet();
                    dsattach = objCore.funGetDataSet(q);
                    for (int l = 0; l < dsattach.Tables[0].Rows.Count; l++)
                    {
                        string sz1 = dsattach.Tables[0].Rows[l]["size"].ToString();

                        string nm1 = dsattach.Tables[0].Rows[l]["Name"].ToString();
                        double atachqty = 0;
                        val = "";
                        val = dsattach.Tables[0].Rows[l]["quantity"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        atachqty = Convert.ToDouble(val);

                        if (logo == "")
                        {
                            dtrpt.Rows.Add(sz1 + " " + nm1 + "- (In Meal)" + "(" + sz + " " + nm + ")", (atachqty * quantity).ToString(), 0, comboBox1.Text, null, 0, 0, 0, "", 0, nm1+"-");
                        }
                        else
                        {
                            dtrpt.Rows.Add(sz1 + " " + nm1 + "- (In Meal)" + "(" + sz + " " + nm + ")", (atachqty * quantity).ToString(), 0, comboBox1.Text, dscompany.Tables[0].Rows[0]["logo"], 0, 0, 0, "", 0, nm1 + "-");
                        }
                    }

                }
                             
                ds = new DataSet();

                if (cmbkds.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  GROUP BY dbo.RuntimeModifier.name";

                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.RuntimeModifier.name";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";

                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";
                        }

                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0 and dbo.RuntimeModifier.kdsid='" + cmbkds.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.name";

                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.kdsid='" + cmbkds.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.RuntimeModifier.name";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.kdsid='" + cmbkds.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";

                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.kdsid='" + cmbkds.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";
                        }

                    } 
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
                    double net = sum - gst;
                    net = net - discount;
                    net = Math.Round(net, 2);
                    sum = Math.Round(sum, 2);

                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (sum > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, null, gst, net, discount, "R Modifier",0);
                        }
                        else
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, "R Modifier",0);
                        }
                    }
                }


                ds = new DataSet();

                if (cmbkds.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {

                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0   GROUP BY dbo.Modifier.name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0    and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";


                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {

                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0   and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.Modifier.name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0    and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";
                        }
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {

                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0 and dbo.Modifier.kdsid='" + cmbkds.SelectedValue + "'  GROUP BY dbo.Modifier.name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0   and dbo.Modifier.kdsid='" + cmbkds.SelectedValue + "'   and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";


                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {

                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0   and dbo.Modifier.kdsid='" + cmbkds.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.Modifier.name";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0   and dbo.Modifier.kdsid='" + cmbkds.SelectedValue + "'   and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";
                        }
                    } 
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
                    double net = sum - gst;
                    net = net - discount;
                    net = Math.Round(net, 2);
                    sum = Math.Round(sum, 2);

                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (sum > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, null, gst, net, discount, "Modifier",0);
                        }
                        else
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, "Modifier",0);
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
