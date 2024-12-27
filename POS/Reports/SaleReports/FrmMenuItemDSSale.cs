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
    public partial class FrmMenuItemDSSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmMenuItemDSSale()
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
                string q = "select id,name from menugroup ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbgroup.DataSource = ds.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "name";
                cmbgroup.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double getquantity(string itmid)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.DSSaledetails.Quantity) AS qty FROM         dbo.DSSale INNER JOIN                      dbo.DSSaledetails ON dbo.dssale.Id = dbo.DSSaledetails.saleid WHERE     (dbo.dssale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.DSSaledetails.MenuItemId = '" + itmid + "') AND (dbo.DSSaledetails.Flavourid = '0') AND (dbo.DSSaledetails.ModifierId = '0') AND                       (dbo.DSSaledetails.RunTimeModifierId = '0')";
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
        public double getprice(string itmid)
        {
            double price = 0;
            try
            {
                string q = "SELECT     (dbo.DSSaledetails.price)  FROM         dbo.DSSale INNER JOIN                      dbo.DSSaledetails ON dbo.dssale.Id = dbo.DSSaledetails.saleid WHERE    (dbo.DSSaledetails.MenuItemId = '" + itmid + "')";
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
                    price = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            return price;
        }
        public double getgst(string itmid)
        {
            double gst = 0;
            DataSet dsgst = new DataSet();
            string q = "SELECT     avg(dbo.dssale.GSTPerc) AS gst FROM         dbo.DSSale INNER JOIN                      dbo.DSSaledetails ON dbo.dssale.Id = dbo.DSSaledetails.saleid WHERE     (dbo.dssale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.DSSaledetails.MenuItemId = '" + itmid + "')";
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
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rprMenuItemSale rptDoc = new rprMenuItemSale();
                POSRestaurant.Reports.SaleReports.DsMenuItem dsrpt = new DsMenuItem();
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
        public double getdiscount(string itmid)
        {
            double discount = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.dssale.GSTPerc) AS gst FROM         dbo.DSSale INNER JOIN                      dbo.DSSaledetails ON dbo.dssale.Id = dbo.DSSaledetails.saleid WHERE     (dbo.dssale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.DSSaledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.dssale.Discount) AS qty FROM         dbo.DSSale INNER JOIN                      dbo.DSSaledetails ON dbo.dssale.Id = dbo.DSSaledetails.saleid WHERE     (dbo.dssale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.DSSaledetails.MenuItemId = '" + itmid + "') AND (dbo.DSSaledetails.Flavourid = '0') AND (dbo.DSSaledetails.ModifierId = '0') AND                       (dbo.DSSaledetails.RunTimeModifierId = '0')";

            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                discount = Convert.ToDouble(val);
            }
            return discount;
        }
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


                if (chktime.Checked == true)
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT     dbo.dssale.date,   SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid' and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.MenuItem.Name, dbo.dssale.date,dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid' and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '"+cmbtimefrom.Text+"' AND '"+cmbtimeto.Text+"')    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT     dbo.dssale.date,   SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid' and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name,dbo.dssale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name, dbo.dssale.date,dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid' and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id    order by dbo.MenuItem.Name  ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }

                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.MenuItem.Name  AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.DSSaledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId=0 and dbo.DSSaledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id   order by dbo.MenuItem.Name  ";
                                }
                            }
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
                    string date = "";
                    try
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(nm, quantity.ToString(), sum, date, null, gst, net, discount, sz,price);
                    }
                    else
                    {
                        dtrpt.Rows.Add(nm, quantity.ToString(), sum, date, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, sz, price);
                    }
                }
                             
                ds = new DataSet();

                if (chktime.Checked == true)
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')    GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }

                            }
                        }

                    }
                    else
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";
                                }

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.iddbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'    AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.RuntimeModifier.name";
                                }

                            }
                        }

                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.dssale.date,dbo.RuntimeModifier.name";
                                }

                            }
                        }

                    }
                    else
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";
                                }

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.RuntimeModifier.name";
                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.DSSaledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id    WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'   and dbo.DSSaledetails.RunTimeModifierId > 0  and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name";
                                }

                            }
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
                    string date = "";
                    try
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {

                    }
                    if (sum > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, date, null, gst, net, discount, "R Modifier",0);
                        }
                        else
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, date, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, "R Modifier",0);
                        }
                    }
                }


                ds = new DataSet();

                if (chktime.Checked == true)
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.dssale.date,dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        dbo.dssale.date,SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')   GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.Modifier.name";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'   AND (DATEPART(hh,  dbo.DSSaledetails.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "')  GROUP BY dbo.Modifier.name";
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (checkBox1.Checked == true)
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.dssale.date,dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        dbo.dssale.date,SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.dssale.date,dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT       dbo.dssale.date, SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT      dbo.dssale.date,  SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.dssale.date,dbo.Modifier.name";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0    and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";
                                }
                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All")
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0 and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";


                                }
                            }
                            else
                            {
                                if (textBox1.Text == "")
                                {

                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'  and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.Modifier.name";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSaledetails.Price) AS sum, SUM(dbo.DSSaledetails.Quantity) AS count, SUM(dbo.DSSaledetails.Itemdiscount) AS dis, SUM(dbo.DSSaledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.DSSaledetails INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.dssale.Id INNER JOIN                         dbo.Modifier ON dbo.DSSaledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.branchid='" + cmbbranch.SelectedValue + "' and DSSale.BillStatus='paid'  and dbo.DSSaledetails.ModifierId >0   and dbo.menugroup.id='" + cmbgroup.SelectedValue + "'   and dbo.dssale.shiftid='" + comboBox1.SelectedValue + "' and dbo.Modifier.name  like '%" + textBox1.Text + "%'  GROUP BY dbo.Modifier.name";
                                }
                            }
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
                    string date = "";
                    try
                    {
                        date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {

                    }
                    if (sum > 0)
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, date, null, gst, net, discount, "Modifier",0);
                        }
                        else
                        {
                            dtrpt.Rows.Add(nm, quantity.ToString(), sum, date, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, "Modifier",0);
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
