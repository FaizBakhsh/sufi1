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
    public partial class frmsuggested : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmsuggested()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
           
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


                POSRestaurant.Reports.SaleReports.rptsuggested rptDoc = new rptsuggested();
                POSRestaurant.Reports.SaleReports.dssuggeted dsrpt = new dssuggeted();
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
        public double getdiscount(string itmid)
        {
            double discount = 0;
            DataSet dsgst = new DataSet();
            // string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            string q = "SELECT     SUM(dbo.Sale.Discount) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";

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
        protected double getitemprice(string id,string type)
        {
            double price = 0;

            try
            {
                string q = "SELECT        dbo.MenuItem.Price,  dbo.MenuItem.proposedprice, dbo.GST.GST FROM            dbo.MenuItem CROSS JOIN                         dbo.GST  where dbo.menuitem.id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp2 = ds.Tables[0].Rows[0]["proposedprice"].ToString();
                    if (type == "current")
                    {
                        price = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["price"].ToString()), 2);// * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                    }
                    else
                    {
                        price = Math.Round(Convert.ToDouble(temp2), 2);
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return price;
        }
        protected double getmargin(string id, string type,string valuetype)
        {
            double price = 0;

            try
            {
                string q = "select * from menuitem where id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (valuetype == "margin")
                    {
                        string temp2 = ds.Tables[0].Rows[0]["currentmargin"].ToString();
                        if (temp2 == "")
                        {
                            temp2 = "0";
                        }
                        if (type == "current")
                        {
                            price = Math.Round(Convert.ToDouble(temp2), 2);// * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                        }
                        else
                        {
                            temp2 = ds.Tables[0].Rows[0]["ProposedMargin"].ToString();
                            if (temp2 == "")
                            {
                                temp2 = "0";
                            }
                            price = Math.Round(Convert.ToDouble(temp2), 2);
                        }
                    }
                    else
                    {
                        string temp2 = ds.Tables[0].Rows[0]["cost"].ToString();
                        if (temp2 == "")
                        {
                            temp2 = "0";
                        }
                        
                        {
                            price = Math.Round(Convert.ToDouble(temp2), 2);// * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return price;
        }
        protected double getitempricef(string id, string type)
        {
            double price = 0;

            try
            {
                string q = "SELECT        dbo.GST.GST, dbo.ModifierFlavour.price, dbo.ModifierFlavour.proposedprice, dbo.ModifierFlavour.ProposedMargin, dbo.ModifierFlavour.currentmargin FROM            dbo.ModifierFlavour CROSS JOIN                         dbo.GST  where dbo.ModifierFlavour.id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp2 = ds.Tables[0].Rows[0]["proposedprice"].ToString();
                    if (temp2 == "")
                    {
                        temp2 = "0";
                    }
                    if (type == "current")
                    {
                        price = Math.Round(Convert.ToDouble(ds.Tables[0].Rows[0]["price"].ToString()), 2);// * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                    }
                    else
                    {
                        price = Math.Round(Convert.ToDouble(temp2), 2);
                    }
                }
            }
            catch (Exception ex)
            {


            }
            return price;
        }
        protected double getmarginf(string id, string type,string valuetype)
        {
            double price = 0;

            try
            {
                string q = "SELECT        dbo.GST.GST, dbo.ModifierFlavour.price, dbo.ModifierFlavour.proposedprice, dbo.ModifierFlavour.ProposedMargin, dbo.ModifierFlavour.currentmargin FROM            dbo.ModifierFlavour CROSS JOIN                         dbo.GST  where dbo.ModifierFlavour.id=" + id;
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (valuetype == "margin")
                    {
                        string temp2 = ds.Tables[0].Rows[0]["currentmargin"].ToString();
                        if (temp2 == "")
                        {
                            temp2 = "0";
                        }
                        if (type == "current")
                        {
                            price = Math.Round(Convert.ToDouble(temp2), 2);// * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                        }
                        else
                        {
                            temp2 = ds.Tables[0].Rows[0]["ProposedMargin"].ToString();
                            if (temp2 == "")
                            {
                                temp2 = "0";
                            }
                            price = Math.Round(Convert.ToDouble(temp2), 2);
                        }
                    }
                    else
                    {
                        string temp2 = ds.Tables[0].Rows[0]["cost"].ToString();
                        if (temp2 == "")
                        {
                            temp2 = "0";
                        }

                        {
                            price = Math.Round(Convert.ToDouble(temp2), 2);// * ((100 + Convert.ToDouble(ds.Tables[0].Rows[0]["GST"].ToString())) / 100), 2);
                        }

                    }
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
                dtrpt.Columns.Add("Group", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Cost", typeof(double));
                dtrpt.Columns.Add("Marging", typeof(double));
                dtrpt.Columns.Add("Price", typeof(double));
                dtrpt.Columns.Add("Qty", typeof(double));
                dtrpt.Columns.Add("Gross", typeof(double));
                dtrpt.Columns.Add("Profit", typeof(string));
                dtrpt.Columns.Add("Margin2", typeof(double));
                dtrpt.Columns.Add("Price2", typeof(double));
                dtrpt.Columns.Add("Gross2", typeof(double));
                dtrpt.Columns.Add("Profit2", typeof(double));
                dtrpt.Columns.Add("Difference", typeof(double));               
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


                q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.MenuItem.Name , dbo.MenuItem.Id AS mid,                          dbo.ModifierFlavour.name AS Expr1,dbo.ModifierFlavour.id AS fid, dbo.MenuGroup.Name AS Expr2  FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY  dbo.MenuItem.Name, dbo.MenuItem.id, dbo.ModifierFlavour.name, dbo.Saledetails.comments,dbo.ModifierFlavour.id, dbo.MenuGroup.Name    order by dbo.MenuItem.Name  ";
                                                        
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double price = 0, price2 = 0, cmargin = 0, pmargin = 0, cost = 0;
                    cmargin = getmargin(ds.Tables[0].Rows[i]["mid"].ToString(), "current","margin");
                    pmargin = getmargin(ds.Tables[0].Rows[i]["mid"].ToString(), "", "margin");
                    cost = getmargin(ds.Tables[0].Rows[i]["mid"].ToString(), "", "cost");
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
                        price = getitemprice(ds.Tables[0].Rows[i]["mid"].ToString(),"current");
                        price2 = getitemprice(ds.Tables[0].Rows[i]["mid"].ToString(), "");
                    }
                    else
                    {
                        price = getitempricef(ds.Tables[0].Rows[i]["fid"].ToString(), "current");
                        price2 = getitempricef(ds.Tables[0].Rows[i]["fid"].ToString(), "");
                        cmargin = getmarginf(ds.Tables[0].Rows[i]["fid"].ToString(), "current", "margin");
                        pmargin = getmarginf(ds.Tables[0].Rows[i]["fid"].ToString(), "", "margin");
                        cost = getmarginf(ds.Tables[0].Rows[i]["fid"].ToString(), "", "cost");
                    }                  
                    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString());
                    
                    if (quantity == 0)
                    {
                        quantity = 1;
                    }

                    val = "";
                    double sum2 = 0, profit = 0, progit2 = 0, difference = 0;
                    sum2 = quantity * price2;
                    profit = cmargin * sum / 100;
                    progit2 = pmargin * sum2 / 100;
                    difference = progit2 - profit;
                    string sz = ds.Tables[0].Rows[i]["Expr1"].ToString();

                    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (logo == "")
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr2"].ToString(),sz+" "+ nm, cost,cmargin,price,quantity,sum,profit,pmargin,price2,sum2,progit2,difference,null);
                    }
                    else
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Expr2"].ToString(), sz + " " + nm, cost, cmargin, price, quantity, sum, profit, pmargin, price2, sum2, progit2, difference, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }
                             
                //ds = new DataSet();

                //q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.RuntimeModifier.name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'   and dbo.Saledetails.RunTimeModifierId > 0  GROUP BY dbo.RuntimeModifier.name";

                
                
                //ds = objCore.funGetDataSet(q);
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    string val = "";
                //    val = ds.Tables[0].Rows[i]["sum"].ToString();
                //    if (val == "")
                //    {
                //        val = "0";
                //    }
                //    double sum = Convert.ToDouble(val);
                //    val = "";

                //    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString());// getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                //    double discount = Convert.ToDouble(ds.Tables[0].Rows[i]["dis"].ToString()); //getdiscount(ds.Tables[0].Rows[i]["mid"].ToString()); //Convert.ToDouble(val);
                //    if (quantity == 0)
                //    {
                //        quantity = 1;
                //    }

                //    discount = Math.Round(discount, 2);
                //    val = "";
                //    double gst = Convert.ToDouble(ds.Tables[0].Rows[i]["gs"].ToString()); //getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                //    //gst = ((sum) * gst) / 100;
                //    sum = sum + gst;
                //    gst = Math.Round(gst, 2);
                //    double net = sum - gst;
                //    net = net - discount;
                //    net = Math.Round(net, 2);
                //    sum = Math.Round(sum, 2);

                //    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                //    if (sum > 0)
                //    {
                //        if (logo == "")
                //        {
                //            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, null, gst, net, discount, "R Modifier",0);
                //        }
                //        else
                //        {
                //            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, "R Modifier",0);
                //        }
                //    }
                //}


                //ds = new DataSet();

                //q = "SELECT        SUM(dbo.Saledetails.Price) AS sum, SUM(dbo.Saledetails.Quantity) AS count, SUM(dbo.Saledetails.Itemdiscount) AS dis, SUM(dbo.Saledetails.ItemGst) AS gs, dbo.Modifier.Name FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id  WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  and dbo.Saledetails.ModifierId >0   GROUP BY dbo.Modifier.name";
                    
                //ds = objCore.funGetDataSet(q);
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    string val = "";
                //    val = ds.Tables[0].Rows[i]["sum"].ToString();
                //    if (val == "")
                //    {
                //        val = "0";
                //    }
                //    double sum = Convert.ToDouble(val);
                //    val = "";

                //    double quantity = Convert.ToDouble(ds.Tables[0].Rows[i]["count"].ToString());// getquantity(ds.Tables[0].Rows[i]["mid"].ToString());
                //    double discount = Convert.ToDouble(ds.Tables[0].Rows[i]["dis"].ToString()); //getdiscount(ds.Tables[0].Rows[i]["mid"].ToString()); //Convert.ToDouble(val);
                //    if (quantity == 0)
                //    {
                //        quantity = 1;
                //    }

                //    discount = Math.Round(discount, 2);
                //    val = "";
                //    double gst = Convert.ToDouble(ds.Tables[0].Rows[i]["gs"].ToString()); //getgst(ds.Tables[0].Rows[i]["mid"].ToString());
                //    //gst = ((sum) * gst) / 100;
                //    sum = sum + gst;
                //    gst = Math.Round(gst, 2);
                //    double net = sum - gst;
                //    net = net - discount;
                //    net = Math.Round(net, 2);
                //    sum = Math.Round(sum, 2);

                //    string nm = ds.Tables[0].Rows[i]["Name"].ToString();
                //    if (sum > 0)
                //    {
                //        if (logo == "")
                //        {
                //            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, null, gst, net, discount, "Modifier",0);
                //        }
                //        else
                //        {
                //            dtrpt.Rows.Add(nm, quantity.ToString(), sum, comboBox1.Text, dscompany.Tables[0].Rows[0]["logo"], gst, net, discount, "Modifier",0);
                //        }
                //    }
                //}

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
