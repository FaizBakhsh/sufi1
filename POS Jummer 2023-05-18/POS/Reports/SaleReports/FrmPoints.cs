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
    public partial class FrmPoints : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmPoints()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
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
            

        }
        public double getquantity(string itmid)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
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
                string q = "SELECT     (dbo.Saledetails.price)  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE    (dbo.Saledetails.MenuItemId = '" + itmid + "')";
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
            string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
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


                POSRestaurant.Reports.SaleReports.rptpoints rptDoc = new rptpoints();
                POSRestaurant.Reports.SaleReports.dspoints dsrpt = new dspoints();
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
        protected double getitemprice(string id)
        {
            double price = 0;

            try
            {
                string q = "SELECT        dbo.MenuItem.GrossPrice FROM            dbo.MenuItem   where dbo.menuitem.id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0]["GrossPrice"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    price = Math.Round(Convert.ToDouble(temp), 2);
                        
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
                string q = "SELECT    dbo.ModifierFlavour.GrossPrice FROM            dbo.ModifierFlavour  where dbo.ModifierFlavour.id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0]["GrossPrice"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    price = Math.Round(Convert.ToDouble(temp), 2);
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
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Code", typeof(string));
               
                dtrpt.Columns.Add("Value", typeof(double));
                
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


                if (checkBox1.Checked == true)
                {


                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT       dbo.sale.date, dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name,dbo.sale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.Saledetails.pointscode,dbo.ModifierFlavour.id   HAVING        (LEN(dbo.Saledetails.pointscode) > 0) order by dbo.MenuItem.Name  ";
                        }
                        else
                        {
                            q = "SELECT       dbo.sale.date,dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'    and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name,dbo.sale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Saledetails.pointscode, dbo.Saledetails.comments,dbo.ModifierFlavour.id  HAVING        (LEN(dbo.Saledetails.pointscode) > 0) order by dbo.MenuItem.Name  ";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT       dbo.sale.date,dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE       (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'    and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name,dbo.sale.date, dbo.MenuItem.id, dbo.ModifierFlavour.name,dbo.Saledetails.pointscode, dbo.Saledetails.comments,dbo.ModifierFlavour.id  HAVING        (LEN(dbo.Saledetails.pointscode) > 0) order by dbo.MenuItem.Name  ";
                        }

                        else
                        {
                            q = "SELECT       dbo.sale.date,dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE      (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'    and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name, dbo.Saledetails.pointscode,dbo.sale.date,dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id  HAVING        (LEN(dbo.Saledetails.pointscode) > 0) order by dbo.MenuItem.Name  ";
                        }
                    }

                }
                else
                {


                    if (comboBox1.Text == "All")
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT       dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.Saledetails.pointscode,dbo.ModifierFlavour.id  HAVING        (LEN(dbo.Saledetails.pointscode) > 0)  order by dbo.MenuItem.Name  ";
                        }
                        else
                        {
                            q = "SELECT       dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE      (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'   GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.Saledetails.pointscode,dbo.ModifierFlavour.id  HAVING        (LEN(dbo.Saledetails.pointscode) > 0) order by dbo.MenuItem.Name  ";
                        }
                    }
                    else
                    {
                        if (textBox1.Text == "")
                        {
                            q = "SELECT       dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE      (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'   and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.Saledetails.pointscode,dbo.ModifierFlavour.id  HAVING        (LEN(dbo.Saledetails.pointscode) > 0) order by dbo.MenuItem.Name  ";
                        }

                        else
                        {
                            q = "SELECT       dbo.Saledetails.pointscode, SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name +'-'+ dbo.Saledetails.comments AS Name, dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE      (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + comboBox1.SelectedValue + "'    and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  and dbo.MenuItem.Name  like '%" + textBox1.Text + "%'    GROUP BY dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.pointscode,dbo.Saledetails.comments,dbo.ModifierFlavour.id  HAVING        (LEN(dbo.Saledetails.pointscode) > 0) order by dbo.MenuItem.Name  ";
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
                    dtrpt.Rows.Add(date, nm, quantity, ds.Tables[0].Rows[i]["pointscode"].ToString(), price * quantity);
                }
                 
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
