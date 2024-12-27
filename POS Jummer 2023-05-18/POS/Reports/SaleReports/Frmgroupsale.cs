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
    public partial class Frmgroupsale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public Frmgroupsale()
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
                DataSet ds1 = new DataSet();
                string q = "select distinct Terminal from sale";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["Terminal"] = "All";
                ds1.Tables[0].Rows.Add(dr1);
                comboBox2.DataSource = ds1.Tables[0];
                comboBox2.ValueMember = "Terminal";
                comboBox2.DisplayMember = "Terminal";

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


                POSRestaurant.Reports.SaleReports.rptGroup rptDoc = new rptGroup();
                POSRestaurant.Reports.SaleReports.dsgroup dsrpt = new dsgroup();
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
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
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
                dtrpt.Columns.Add("Group", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Net Amount", typeof(double));
                dtrpt.Columns.Add("SubGroup", typeof(string));
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
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr2,dbo.MenuGroup.SubGroup , dbo.MenuItem.Name + '-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='"+cmbbranch.SelectedValue+"'   and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup   order by dbo.MenuItem.Name  ";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr2,dbo.MenuGroup.SubGroup, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup   order by dbo.MenuItem.Name  ";
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr2,dbo.MenuGroup.SubGroup, dbo.MenuItem.Name + '-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Sale.Terminal='" + comboBox2.Text + "'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup   order by dbo.MenuItem.Name  ";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr2,dbo.MenuGroup.SubGroup, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'  and dbo.Sale.Terminal='" + comboBox2.Text + "'  and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup   order by dbo.MenuItem.Name  ";
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
                    sum = Math.Round(sum, 2);
                    string sz = ds.Tables[0].Rows[i]["Expr1"].ToString();
                    if (sz.Length > 0)
                    {
                        sz = sz + " ";
                    }
                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr2"].ToString(), sz + nm, quantity.ToString(), sum, ds.Tables[0].Rows[i]["SubGroup"].ToString());
                }
                             
                ds = new DataSet();


                if (comboBox2.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1,dbo.MenuGroup.SubGroup, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1,dbo.MenuGroup.SubGroup, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup";
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1,dbo.MenuGroup.SubGroup, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.Terminal='" + comboBox2.Text + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1,dbo.MenuGroup.SubGroup, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Sale.Terminal='" + comboBox2.Text + "'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name,dbo.MenuGroup.SubGroup";
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
                   
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }

                   
                    sum = Math.Round(sum, 2);

                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (sum > 0)
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr1"].ToString(), "R Modifier" + nm, quantity.ToString(), sum, ds.Tables[0].Rows[i]["SubGroup"].ToString());
                    }
                }


                ds = new DataSet();


                if (comboBox2.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name";
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.ModifierId > 0  GROUP BY dbo.Modifier.Name";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name";
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.ModifierId > 0  dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'   GROUP BY dbo.Modifier.Name";
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.Terminal='" + comboBox2.Text + "'   and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name";
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.ModifierId > 0  GROUP BY dbo.Modifier.Name";
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuGroup.Name AS Expr1, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.branchid='" + cmbbranch.SelectedValue + "'  and Sale.BillStatus='paid'  and dbo.Sale.Terminal='" + comboBox2.Text + "'    and dbo.Saledetails.RunTimeModifierId > 0  and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RuntimeModifier.name, dbo.MenuGroup.Name";
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.ModifierId > 0  dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'   GROUP BY dbo.Modifier.Name";
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

                    if (quantity == 0)
                    {
                        quantity = 1;
                    }


                    sum = Math.Round(sum, 2);

                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (sum > 0)
                    {
                        dtrpt.Rows.Add("Modifier", nm, quantity.ToString(), sum, "Modifiers");
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
