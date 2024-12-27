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
    public partial class FrmComplimentorySale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmComplimentorySale()
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
          
            
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
       
        public void bindreport()
        {

            try
            {
                getcompany();
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptcomplimntry rptDoc = new rptcomplimntry();
                POSRestaurant.Reports.SaleReports.dscomplimentry dsrpt = new dscomplimentry();
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
                rptDoc.SetParameterValue("date", "for the period  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
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
                dtrpt.Columns.Add("InvoiceNo", typeof(string));
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("Narration", typeof(string));
                dtrpt.Columns.Add("Customer", typeof(string));
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
               
                string q = "";
                DataSet ds = new DataSet();
                if (textBox1.Text == "")
                {
                    if (comboBox2.Text == "All")
                    {
                        q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                    }
                    else
                    {
                        q = "SELECT       ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.shiftid='" + comboBox2.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Price, dbo.ModifierFlavour.price  order by dbo.Sale.Date";
                    }
                }
                else
                {
                    if (comboBox2.Text == "All")
                    {
                        q = "SELECT       ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    menuitem.name like '%" + textBox1.Text + "%' and   (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Price, dbo.ModifierFlavour.price  order by dbo.Sale.Date";
                    }
                    else
                    {
                        q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount,SUM(dbo.Saledetails.Quantity) AS Quantity,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    menuitem.name like '%" + textBox1.Text + "%' and      (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.shiftid='" + comboBox2.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.MenuItem.Price, dbo.ModifierFlavour.price  order by dbo.Sale.Date";
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string qty = ds.Tables[0].Rows[i]["Quantity"].ToString();
                    if (qty == "")
                    {
                        qty = "0";
                    }
                    string amount = ds.Tables[0].Rows[i]["amount"].ToString();
                    if (amount == "")
                    {
                        amount = "0";
                    }
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Id"].ToString(), Convert.ToDateTime(ds.Tables[0].Rows[i]["date"].ToString()).ToString("dd-MM-yyyy"), ds.Tables[0].Rows[i]["Expr1"].ToString() + " " + ds.Tables[0].Rows[i]["Name"].ToString(), qty, amount, ds.Tables[0].Rows[i]["ComplimentoryNarration"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString());

                }
                ds = new DataSet();
                if (textBox1.Text == "")
                {
                    if (comboBox2.Text == "All")
                    {
                        q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration,dbo.RuntimeModifier.Price  order by dbo.Sale.Date";
                    }
                    else
                    {
                        q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount,  SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.shiftid='" + comboBox2.SelectedValue + "' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration,dbo.RuntimeModifier.Price  order by dbo.Sale.Date";
                    }
                }
                else
                {
                    if (comboBox2.Text == "All")
                    {
                        q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount,  SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE    RuntimeModifier.name like '%" + textBox1.Text + "%' and   (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration,dbo.RuntimeModifier.Price  order by dbo.Sale.Date";
                    }
                    else
                    {
                        q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount,  SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE    RuntimeModifier.name like '%" + textBox1.Text + "%' and      (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.shiftid='" + comboBox2.SelectedValue + "' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.Saledetails.ComplimentoryNarration,dbo.RuntimeModifier.Price  order by dbo.Sale.Date";
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string qty = ds.Tables[0].Rows[i]["Quantity"].ToString();
                    if (qty == "")
                    {
                        qty = "0";
                    }
                    string amount = ds.Tables[0].Rows[i]["amount"].ToString();
                    if (amount == "")
                    {
                        amount = "0";
                    }
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Id"].ToString(), Convert.ToDateTime(ds.Tables[0].Rows[i]["date"].ToString()).ToString("dd-MM-yyyy"), ds.Tables[0].Rows[i]["Name"].ToString(), qty, amount, ds.Tables[0].Rows[i]["ComplimentoryNarration"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString());

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
           
        }
    }
}
