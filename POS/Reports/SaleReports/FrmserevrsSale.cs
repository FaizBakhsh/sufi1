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
    public partial class FrmserevrsSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmserevrsSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select id,name from ResturantStaff ";
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
                string q = "select id,name from menugroup order by name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox2.DataSource = ds.Tables[0];
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "name";
                comboBox2.Text = "All";
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


                POSRestaurant.Reports.SaleReports.rptwaiterorders rptDoc = new rptwaiterorders();
                POSRestaurant.Reports.SaleReports.dswaitersorders dsrpt = new dswaitersorders();
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

                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phone", phone);
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
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("date", typeof(DateTime));
                dtrpt.Columns.Add("item", typeof(string));
                dtrpt.Columns.Add("qty", typeof(double));
                dtrpt.Columns.Add("amount", typeof(double));
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
                DataSet ds = new DataSet();
                string q = "";
                if (comboBox2.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";
                        }
                        else
                        {
                            q = "SELECT dbo.Sale.DATE,dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name FROM  dbo.Saledetails INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where  dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid' GROUP BY  dbo.MenuItem.Name, dbo.ResturantStaff.Name,dbo.Sale.DATE";
                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where    dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";

                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT dbo.Sale.DATE,dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name FROM  dbo.Saledetails INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "' and Sale.BillStatus='paid' GROUP BY  dbo.MenuItem.Name, dbo.ResturantStaff.Name,dbo.Sale.DATE";

                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "'  and  Sale.BillStatus='paid' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";

                        }

                        else
                        {
                            q = "SELECT dbo.Sale.DATE,dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name FROM  dbo.Saledetails INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where   dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "'   and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid' GROUP BY  dbo.MenuItem.Name, dbo.ResturantStaff.Name,dbo.Sale.DATE";
                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where     dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "'  and  Sale.BillStatus='paid' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";

                        }
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid' and dbo.menuitem.menugroupid='"+comboBox2.SelectedValue+"' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";
                        }
                        else
                        {
                            q = "SELECT dbo.Sale.DATE,dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name FROM  dbo.Saledetails INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where  dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid' GROUP BY  dbo.MenuItem.Name, dbo.ResturantStaff.Name,dbo.Sale.DATE";
                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where    dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.menuitem.menugroupid='" + comboBox2.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";

                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT dbo.Sale.DATE,dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name FROM  dbo.Saledetails INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "' and Sale.BillStatus='paid' GROUP BY  dbo.MenuItem.Name, dbo.ResturantStaff.Name,dbo.Sale.DATE";

                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "'  and  Sale.BillStatus='paid' and dbo.menuitem.menugroupid='" + comboBox2.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";

                        }

                        else
                        {
                            q = "SELECT dbo.Sale.DATE,dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name FROM  dbo.Saledetails INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where   dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "'   and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid' GROUP BY  dbo.MenuItem.Name, dbo.ResturantStaff.Name,dbo.Sale.DATE";
                            q = "SELECT        dbo.Sale.Date, dbo.MenuItem.Name AS item, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS price, dbo.ResturantStaff.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier,                          dbo.RuntimeModifier.name AS runtime FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid INNER JOIN                         dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  where     dbo.MenuItem.Name  like '%" + textBox1.Text + "%'  and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.ResturantStaff.id='" + comboBox1.SelectedValue + "'  and  Sale.BillStatus='paid' and dbo.menuitem.menugroupid='" + comboBox2.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.ResturantStaff.Name, dbo.Sale.Date, dbo.ModifierFlavour.name, dbo.Modifier.Name, dbo.RuntimeModifier.name";

                        }
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["price"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    val = "";
                    val = ds.Tables[0].Rows[i]["runtime"].ToString();
                    if (val == "")
                    {
                        val = ds.Tables[0].Rows[i]["Modifier"].ToString();
                        if (val == "")
                        {
                            val = ds.Tables[0].Rows[i]["flavour"].ToString() + " " + ds.Tables[0].Rows[i]["item"].ToString();
                        }
                    }
                    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                    val = val.Trim();

                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["date"].ToString(), val, quantity.ToString(), sum, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["date"].ToString(), val, quantity.ToString(), sum, dscompany.Tables[0].Rows[0]["logo"]);
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
