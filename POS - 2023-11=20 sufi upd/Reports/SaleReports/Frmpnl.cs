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
    public partial class Frmpnl : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public Frmpnl()
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
        }
       
        
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptpnl rptDoc = new rptpnl();
                POSRestaurant.Reports.SaleReports.dspnl dsrpt = new dspnl();
                
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

                TimeSpan ts = Convert.ToDateTime(dateTimePicker2.Text) - Convert.ToDateTime(dateTimePicker1.Text);
                double days = ts.TotalDays + 1;
                

                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
               
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn",phone );
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text+"("+days.ToString()+" days)");
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
        protected string getbranchtype(string id)
        {
            string branchtype = "";
            try
            {
                string q = "select type from branch where id='" + id + "'";
                DataSet dsb = new DataSet();
                dsb = objCore.funGetDataSet(q);
                if (dsb.Tables[0].Rows.Count > 0)
                {
                    branchtype = dsb.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return branchtype;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Group", typeof(string));
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("Percentage", typeof(double));
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
                double net=0;
                double grosmargin = 0;
                DataSet ds = new DataSet();
                string month1 = dateTimePicker1.Text.Substring(0, 2);
                string year1 = dateTimePicker1.Text.Substring(3);
                string month2 = dateTimePicker2.Text.Substring(0, 2);
                string year2 = dateTimePicker2.Text.Substring(3);
                string q = "";
                q = "select sum(TotalBill) as gross, sum(discountamount) as dis,sum(gst) as gst,avg(gstperc) as gstprc,avg(Discount) as Discount from sale where  date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and billstatus='Paid'";
               // q = "select sum(TotalBill) as gross, sum(discountamount) as dis,sum(gst) as gst,avg(gstperc) as gstprc,avg(Discount) as Discount from sale where  MONTH(date) BETWEEN MONTH(" + month1 + ") AND MONTH(" + month2 + ") and YEAR(date) BETWEEN YEAR("+year1+") AND YEAR("+year2+") and billstatus='Paid'";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["gross"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double gross = Convert.ToDouble(val);
                    gross = Math.Round(gross, 2);
                    val = "";
                    val = ds.Tables[0].Rows[i]["dis"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double discount = Convert.ToDouble(val);
                    discount = Math.Round(discount, 2);
                    val = "";
                    val = "";
                    val = ds.Tables[0].Rows[i]["gst"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double gst = Convert.ToDouble(val);

                    val = "";
                    val = ds.Tables[0].Rows[i]["Discount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double disprc = Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["gstprc"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double gstprc = Convert.ToDouble(val);
                    double netprc = 100 - (gstprc + disprc);

                    gst = Math.Round(gst, 2);
                    net = gross - (discount);
                    gross = gross + gst;
                    //gross = gross + discount;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("1. Sales", "Gross Sale", gross, 100, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("1. Sales", "Gross Sale", gross, 100, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                   
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("1. Sales", "Discounts", discount, disprc, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("1. Sales", "Discounts", discount, disprc, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                   
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("1. Sales", "GST", gst, gstprc, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("1. Sales", "GST", gst, gstprc, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    
                   
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("1. Sales", "Net Sales", net, netprc, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("1. Sales", "Net Sales", net, netprc, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    DataTable dtlog = new DataTable();
                    dtlog.Columns.Add("id", typeof(string));
                    dtlog.Columns.Add("amount", typeof(string));
                    dtlog.Columns.Add("type", typeof(string));
                    double cost = 0;// getcostmenu("", "");
                    double pcost = 0;// getcostmenu("", "Packing");
                    //cost = cost + getcostmenu("modifier", "");
                    //pcost = pcost + getcostmenu("modifier", "Packing");


                   // q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) as menuprice, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid')  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0   GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target";
                   //DataSet dscost = objCore.funGetDataSet(q);
                   //for (int k = 0; k < dscost.Tables[0].Rows.Count; k++)
                   // {

                   //     double cost1 = getcostmenu1(dateTimePicker1.Text, dateTimePicker1.Text, dscost.Tables[0].Rows[k]["id"].ToString(), dscost.Tables[0].Rows[k]["Flavourid"].ToString(), dscost.Tables[0].Rows[k]["RunTimeModifierId"].ToString(), dscost.Tables[0].Rows[k]["ModifierId"].ToString(), branchtype, "");
                   //     cost1 = cost1 * Convert.ToDouble(dscost.Tables[0].Rows[k]["qty"].ToString());
                 
                   //     double value = getcostmenu(dscost.Tables[0].Rows[k]["id"].ToString(), dscost.Tables[0].Rows[k]["Flavourid"].ToString(), "", "", "", "", "yes");
                   //     cost = cost + value;
                   //     double value2 = getcostmenu(dscost.Tables[0].Rows[k]["id"].ToString(), dscost.Tables[0].Rows[k]["Flavourid"].ToString(), "", "Packing", "", "", "yes");
                   //     pcost = pcost + value2;
                   //     dtlog.Rows.Add(dscost.Tables[0].Rows[k]["id"].ToString(), value, "");
                   //     dtlog.Rows.Add(dscost.Tables[0].Rows[k]["id"].ToString(), value2, "Packing");



                   // }
                   //dscost = new DataSet();
                   //q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId > 0)  AND (dbo.Saledetails.Price > 0)  GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price ORDER BY dbo.Modifier.Name";
                   //dscost = objCore.funGetDataSet(q);
                   //for (int k = 0; k < dscost.Tables[0].Rows.Count; k++)
                   //{
                      
                   //    double value = getcostmenu(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), "0", "modifier", "", "", "", "no");
                   //    cost = cost + value;
                   //    double value2 = getcostmenu(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), "0", "modifier", "Packing", "", "", "no");
                   //    pcost = pcost + value2;
                   //    dtlog.Rows.Add(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), value, "Modifier");
                   //    dtlog.Rows.Add(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), value2, "Modifier Packing");
                   //}
                    string branchtype = "";
                    if (cmbbranch.Text == "All")
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) as menuprice, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId,dbo.Saledetails.ModifierId,dbo.sale.branchid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid')  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0   GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target,dbo.Saledetails.RunTimeModifierId,dbo.sale.branchid, dbo.Saledetails.ModifierId";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) as menuprice, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId,dbo.Saledetails.ModifierId, dbo.sale.branchid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and  (dbo.Sale.BillStatus = 'Paid')  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0   GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target,dbo.Saledetails.RunTimeModifierId,dbo.sale.branchid, dbo.Saledetails.ModifierId";
                    }
                    DataSet dscost = objCore.funGetDataSet(q);
                    for (int k = 0; k < dscost.Tables[0].Rows.Count; k++)
                    {
                        if (branchtype == "")
                        {
                            branchtype = getbranchtype(dscost.Tables[0].Rows[k]["branchid"].ToString());
                        }
                        if (dscost.Tables[0].Rows[k]["id"].ToString() == "82")
                        {

                        }
                        double cost1 = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, dscost.Tables[0].Rows[k]["id"].ToString(), dscost.Tables[0].Rows[k]["Flavourid"].ToString(), dscost.Tables[0].Rows[k]["RunTimeModifierId"].ToString(), dscost.Tables[0].Rows[k]["ModifierId"].ToString(), branchtype, "");
                        cost1 = cost1 * Convert.ToDouble(dscost.Tables[0].Rows[k]["qty"].ToString());
                        double pcost1 = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, dscost.Tables[0].Rows[k]["id"].ToString(), dscost.Tables[0].Rows[k]["Flavourid"].ToString(), dscost.Tables[0].Rows[k]["RunTimeModifierId"].ToString(), dscost.Tables[0].Rows[k]["ModifierId"].ToString(), branchtype, "Packing");
                        pcost1 = pcost1 * Convert.ToDouble(dscost.Tables[0].Rows[k]["qty"].ToString());

                        cost = cost + cost1;
                        pcost = pcost + pcost1;


                        //double value=getcostmenu(dscost.Tables[0].Rows[k]["id"].ToString(), dscost.Tables[0].Rows[k]["Flavourid"].ToString(), "", "", "", "", "yes");
                        //cost = cost + value;
                        //double value2 =getcostmenu(dscost.Tables[0].Rows[k]["id"].ToString(), dscost.Tables[0].Rows[k]["Flavourid"].ToString(), "", "Packing", "", "", "yes");
                        //pcost = pcost + value2;
                        dtlog.Rows.Add(dscost.Tables[0].Rows[k]["id"].ToString(), cost1, "");
                        dtlog.Rows.Add(dscost.Tables[0].Rows[k]["id"].ToString(), pcost1, "Packing");

                    }
                    double mcost = 0, mpcost = 0;
                    dscost = new DataSet();
                    if (cmbbranch.Text == "All")
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId > 0)    GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price ORDER BY dbo.Modifier.Name";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId > 0)    GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price ORDER BY dbo.Modifier.Name";
                    }
                    dscost = objCore.funGetDataSet(q);
                    for (int k = 0; k < dscost.Tables[0].Rows.Count; k++)
                    {
                        double cost1 = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, "0", "0", "0", dscost.Tables[0].Rows[k]["ModifierId"].ToString(), branchtype, "");
                        cost1 = cost1 * Convert.ToDouble(dscost.Tables[0].Rows[k]["qty"].ToString());
                        double pcost1 = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, "0", "0", "0", dscost.Tables[0].Rows[k]["ModifierId"].ToString(), branchtype, "Packing");
                        pcost1 = pcost1 * Convert.ToDouble(dscost.Tables[0].Rows[k]["qty"].ToString());
                        mcost = cost1;
                        mpcost = pcost1;
                        cost = cost + cost1;
                        pcost = pcost + pcost1;


                        //double value =getcostmenu(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), "0", "modifier", "", "", "", "no");
                        //cost = cost + value;
                        //double value2 =getcostmenu(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), "0", "modifier", "Packing", "", "", "no");
                        //pcost = pcost + value2;
                        dtlog.Rows.Add(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), cost1, "Modifier");
                        dtlog.Rows.Add(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), pcost1, "Modifier Packing");
                    }

                    dscost = new DataSet();

                    if (cmbbranch.Text == "All")
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.RuntimeModifier.name, dbo.RuntimeModifier.price AS Expr1,                          dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE       (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND     (dbo.Sale.BillStatus = 'Paid')  AND (dbo.Saledetails.RunTimeModifierId > 0) GROUP BY dbo.RuntimeModifier.name, dbo.RuntimeModifier.price, dbo.Saledetails.RunTimeModifierId";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.RuntimeModifier.name, dbo.RuntimeModifier.price AS Expr1,                          dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE       (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND     (dbo.Sale.BillStatus = 'Paid') AND  (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "')  AND (dbo.Saledetails.RunTimeModifierId > 0) GROUP BY dbo.RuntimeModifier.name, dbo.RuntimeModifier.price, dbo.Saledetails.RunTimeModifierId";
                    }
                    dscost = objCore.funGetDataSet(q);
                    for (int k = 0; k < dscost.Tables[0].Rows.Count; k++)
                    {
                        double cost1 = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, "0", "0", dscost.Tables[0].Rows[k]["RunTimeModifierId"].ToString(), "0", branchtype, "");
                        cost1 = cost1 * Convert.ToDouble(dscost.Tables[0].Rows[k]["qty"].ToString());
                        double pcost1 = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, "0", "0", dscost.Tables[0].Rows[k]["RunTimeModifierId"].ToString(), "0", branchtype, "Packing");
                        pcost1 = pcost1 * Convert.ToDouble(dscost.Tables[0].Rows[k]["qty"].ToString());

                        mcost = mcost + cost1;
                        mpcost = mpcost + pcost1;
                        cost = cost + cost1;
                        pcost = pcost + pcost1;


                        //double value =getcostmenu(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), "0", "modifier", "", "", "", "no");
                        //cost = cost + value;
                        //double value2 =getcostmenu(dscost.Tables[0].Rows[k]["ModifierId"].ToString(), "0", "modifier", "Packing", "", "", "no");
                        //pcost = pcost + value2;
                        dtlog.Rows.Add(dscost.Tables[0].Rows[k]["RunTimeModifierId"].ToString(), cost1, "R Modifier");
                        dtlog.Rows.Add(dscost.Tables[0].Rows[k]["RunTimeModifierId"].ToString(), pcost1, "R Modifier Packing");
                    }

                    double costperc = (cost / (net)) * 100;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("2. Cost", "Food Cost", cost, costperc, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("2. Cost", "Food Cost", cost, costperc, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    double pcostperc = (pcost / (net)) * 100;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("2. Cost", "Paper Cost", pcost, pcostperc, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("2. Cost", "Paper Cost", pcost, pcostperc, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                    double tcostperc = ((cost + pcost) / (net)) * 100;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("2. Cost", "Total Cost", cost + pcost, tcostperc, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("2. Cost", "Total Cost", cost + pcost, tcostperc, dscompany.Tables[0].Rows[0]["logo"]);
                    }

                    grosmargin = net - (cost + pcost);
                    double grosmprc = (grosmargin / net) * 100;
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("2. Cost", "Gross Margin", grosmargin, grosmprc, null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("2. Cost", "Gross Margin", grosmargin, grosmprc, dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }
                double totalexp = 0, totalsalary = 0;

                ds = new DataSet();
                q = "";
                q = "With clientsum (Empid,amount) as (    SELECT     Empid, avg(amount) AS amount    FROM         EmployeesSalary   where date between '"+dateTimePicker1.Text+"' and  '"+dateTimePicker2.Text+"'   group by Empid) SELECT sum(amount) AS amount FROM  clientsum ";

                q = "select empid,name from employees";
                DataSet dsemp = new DataSet();
                dsemp = objCore.funGetDataSet(q);
                TimeSpan ts = Convert.ToDateTime(dateTimePicker2.Text) - Convert.ToDateTime(dateTimePicker1.Text);
                double days = ts.TotalDays + 1;
                double months = (Convert.ToDateTime(dateTimePicker1.Text).Year * 12 + Convert.ToDateTime(dateTimePicker1.Text).Month) - (Convert.ToDateTime(dateTimePicker2.Text).Year * 12 + Convert.ToDateTime(dateTimePicker2.Text).Month);
                months = months + 1;
                for (int j = 0 ; j <dsemp.Tables[0].Rows.Count; j++)
                {

                    q = "select Empid, (amount) AS amount    FROM         EmployeesSalary   where date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "'  and Empid='" + dsemp.Tables[0].Rows[j]["Empid"].ToString() + "' ";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string val = "";
                            val = ds.Tables[0].Rows[i]["Amount"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            if (val == "0")
                            {
                            }
                            double amount = Convert.ToDouble(val);
                            amount = Math.Round(amount, 2);
                            // amount = (amount * 12) / 365;
                            //amount = amount * days;
                           // amount = amount * months;
                            amount = Math.Round(amount, 2);
                            totalexp = totalexp + amount;
                            totalsalary = totalsalary + amount; 
                        }
                    }
                    else
                    {
                        //q = "select  top 1 (amount) AS amount    FROM         EmployeesSalary   where date < '" + dateTimePicker1.Text + "'  and Empid='" + dsemp.Tables[0].Rows[j]["Empid"].ToString() + "' order by date desc";
                        //DataSet ds2 = new DataSet();
                        //string val = "";
                        //ds2 = objCore.funGetDataSet(q);
                        //if (ds2.Tables[0].Rows.Count > 0)
                        //{
                        //    val = ds2.Tables[0].Rows[0]["Amount"].ToString();
                        //    if (val == "")
                        //    {
                        //        val = "0";
                        //    }
                        //    double amount = Convert.ToDouble(val);
                        //    amount = Math.Round(amount, 2);
                        //    //amount = (amount * 12) / 365;
                        //    //amount = amount * days;
                        //    amount = amount * months;
                        //    amount = Math.Round(amount, 2);
                        //    totalexp = totalexp + amount;
                        //    totalsalary = totalsalary + amount;
                        //}
                    }
                }
                double perc = (totalsalary / net) * 100;
                if (logo == "")
                {
                    dtrpt.Rows.Add("3. Expenses", "Salaries/Labour", totalsalary, perc, null);
                }
                else
                {
                    dtrpt.Rows.Add("3. Expenses", "Salaries/Labour", totalsalary, perc, dscompany.Tables[0].Rows[0]["logo"]);
                }
                ds = new DataSet();
                q = "";
                q = "select distinct name as name from Expenses";
                dsemp = new DataSet();
                dsemp = objCore.funGetDataSet(q);
               
                for (int j = 0; j < dsemp.Tables[0].Rows.Count; j++)
                {
                    q = "SELECT        Name, (Amount) AS Amount FROM            Expenses  where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  and Name='"+dsemp.Tables[0].Rows[j]["name"].ToString()+"'  ";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        double amount = 0;
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            string val = "";
                            val = ds.Tables[0].Rows[i]["Amount"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }

                            amount = amount + Convert.ToDouble(val);
                            amount = Math.Round(amount, 2);
                            //amount = (amount * 12) / 365;
                            //amount = amount * days;
                            //amount = amount * months;
                            amount = Math.Round(amount, 2);
                            totalexp = totalexp +Convert.ToDouble(val);
                        }


                        perc = (amount / net) * 100;
                        if (logo == "")
                        {
                            dtrpt.Rows.Add("3. Expenses", ds.Tables[0].Rows[0]["Name"].ToString(), amount, perc, null);
                        }
                        else
                        {
                            dtrpt.Rows.Add("3. Expenses", ds.Tables[0].Rows[0]["Name"].ToString(), amount, perc, dscompany.Tables[0].Rows[0]["logo"]);
                        }
                    }
                    else
                    {
                       
                    }
                }
                double totalprc = (totalexp / net) * 100;
                if (logo == "")
                {
                    dtrpt.Rows.Add("3. Expenses", "Total Expenses", totalexp, totalprc, null);
                }
                else
                {
                    dtrpt.Rows.Add("3. Expenses", "Total Expenses", totalexp, totalprc, dscompany.Tables[0].Rows[0]["logo"]);
                }
                double profit = grosmargin - totalexp;
                double profitprc = (profit / net) * 100;
                if (logo == "")
                {
                    dtrpt.Rows.Add("4. Profit n Loss", "Profit n Loss", profit, profitprc, null);
                }
                else
                {
                    dtrpt.Rows.Add("4. Profit n Loss", "Profit n Loss", profit, profitprc, dscompany.Tables[0].Rows[0]["logo"]);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        //private double getprice(string id)
        //{
        //    if (id == "69")
        //    {

        //    }
        //    double variance = 0, price = 0;
        //    string val = "";
        //    DataSet dspurchase = new DataSet();
        //    string q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";
        //    dspurchase = objCore.funGetDataSet(q);
        //    if (dspurchase.Tables[0].Rows.Count > 0)
        //    {
        //        val = dspurchase.Tables[0].Rows[0][0].ToString();
        //        if (val == "")
        //        {
        //            val = "0";
        //        }
        //        price = Convert.ToDouble(val);
        //    }
        //    if (price == 0)
        //    {
        //        dspurchase = new DataSet();
        //        q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";
        //        dspurchase = objCore.funGetDataSet(q);
        //        if (dspurchase.Tables[0].Rows.Count > 0)
        //        {
        //            val = dspurchase.Tables[0].Rows[0][0].ToString();
        //            if (val == "")
        //            {
        //                val = "0";
        //            }
        //            price = Convert.ToDouble(val);
        //        }
        //    }
        //    if (price == 0)
        //    {
        //        dspurchase = new DataSet();
        //        q = "select price from rawitem where id='" + id + "'";
        //        dspurchase = objCore.funGetDataSet(q);
        //        if (dspurchase.Tables[0].Rows.Count > 0)
        //        {
        //            try
        //            {
        //                val = dspurchase.Tables[0].Rows[0][0].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                price = Convert.ToDouble(val);
        //            }
        //            catch (Exception ez)
        //            {


        //            }
        //        }
        //    }
        //    return price;
        //}
        private double getprice(string id)
        {
            if (id == "69")
            {

            }
            double variance = 0, price = 0;
            string val = "";
            DataSet dspurchase = new DataSet();
            string q = "";

            q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryTransfer.ItemId = '" + id + "'  GROUP BY MONTH(Date), YEAR(Date)";

            dspurchase = objCore.funGetDataSet(q);
            for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
            {
                val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = price + Convert.ToDouble(val);
            }
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                price = price / dspurchase.Tables[0].Rows.Count;
            }
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT    top 1    (price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date <= '" + dateTimePicker1.Text + "' ) and dbo.InventoryTransfer.ItemId = '" + id + "'  order by date desc ";

                dspurchase = objCore.funGetDataSet(q);
                for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
                {
                    val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = price + Convert.ToDouble(val);
                }

            }
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";

                dspurchase = objCore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = Convert.ToDouble(val);
                }
                if (price == 0)
                {
                    dspurchase = new DataSet();

                    q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";

                    dspurchase = objCore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        val = dspurchase.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        price = Convert.ToDouble(val);
                    }
                }
                if (price == 0)
                {
                    dspurchase = new DataSet();
                    q = "select price from rawitem where id='" + id + "'";
                    dspurchase = objCore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            val = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            price = Convert.ToDouble(val);
                        }
                        catch (Exception ez)
                        {


                        }
                    }
                }
            }
            return price;
        }
        protected string getbranchtype()
        {
            string branchtype = "";
            try
            {
                string q = "select type from branch where id='" + cmbbranch.SelectedValue + "'";
                DataSet dsb = new DataSet();
                dsb = objCore.funGetDataSet(q);
                if (dsb.Tables[0].Rows.Count > 0)
                {
                    branchtype = dsb.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return branchtype;
        }
        public double getcostmenu1(string start, string end, string id, string flid, string rid, string mid, string btype, string packingtype)
        {
            double cost = 0;
            if (flid == "")
            {
                flid = "0";
            }
            string q = "select  dbo.getcost3(" + id + ",'" + start + "','" + end + "'," + flid + "," + rid + "," + mid + ",'" + btype + "','" + packingtype + "')";
            try
            {
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {


            }

            return cost;
        }
        public double getcostmenu(string id, string mid, string type, string cat, string prcqty, string order, string chkrm)
        {
            double cost = 0, totalqty = 0;
            string branchtype = getbranchtype();
            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            if (type == "modifier")
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE  dbo.Sale.billstatus='Paid'  and     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.ModifierId ='" + id + "'    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head FROM            Modifier where id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "'";
                    if (cat.ToLower() == "packing")
                    {
                        q = "SELECT        dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.Modifier.uploadstatus, dbo.Modifier.branchid, dbo.Modifier.kdsid, dbo.Modifier.menugroupid, dbo.Modifier.Head,                          dbo.Type.TypeName FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId RIGHT OUTER JOIN                         dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId where dbo.Modifier.id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "' and dbo.Type.TypeName='Packing'";
                    }
                    else
                    {
                        q = "SELECT        dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.Modifier.uploadstatus, dbo.Modifier.branchid, dbo.Modifier.kdsid, dbo.Modifier.menugroupid, dbo.Modifier.Head,                          dbo.Type.TypeName FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId RIGHT OUTER JOIN                         dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId where dbo.Modifier.id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "' and dbo.Type.TypeName!='Packing'";

                    }

                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }
            }
            else if (type == "rmodifier")
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.RunTimeModifierId ='" + id + "'  and dbo.Saledetails.ModifierId =0  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId";
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'";
                    if (cat.ToLower() == "packing")
                    {
                        q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'  and dbo.Type.TypeName='Packing'";
                    }
                    else
                    {
                        q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'  and dbo.Type.TypeName!='Packing'";
                    }
                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }
            }
            else
            {

                if (prcqty == "0")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price=0    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                }
                else
                {
                    if (order.ToLower() == "delivery")
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer='Delivery' and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price>0    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer !='Delivery'  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price>0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                    }
                }
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE   dbo.Sale.billstatus='Paid'  and (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')   and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";

                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }

                    string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if (rmodid == "")
                    {
                        rmodid = "0";
                    }

                    double qty = Convert.ToDouble(temp);
                    if (rmodid == "0")
                    {
                        if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                        {
                            // q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                            if (cat.ToLower() == "packing")
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";

                            }
                            else
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                            }
                        }
                        else
                        {
                            // q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";

                            if (cat.ToLower() == "packing")
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'  and dbo.Type.TypeName='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";

                            }
                            else
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                            }

                        }
                    }
                    else
                    {
                        // q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + rmodid + "'";

                        if (cat.ToLower() == "packing")
                        {
                            q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName='Packing'  ";
                        }
                        else
                        {
                            q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName!='Packing'  ";
                        }


                    }
                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);

                    DataTable dtprice = new DataTable();
                    dtprice.Columns.Add("id", typeof(string));
                    dtprice.Columns.Add("price", typeof(string));


                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());
                        dtprice.Rows.Add(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString(), prc);
                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }






            }

            return cost;
        }
        public double getcostmenu(string type, string cat)
        {
            double cost = 0, totalqty = 0;
            string branchtype = getbranchtype();
            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            if (type == "modifier")
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.ModifierId >0   and  dbo.Saledetails.price >0    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head FROM            Modifier where id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "'";
                    if (cat.ToLower() == "packing")
                    {
                        q = "SELECT        dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.Modifier.uploadstatus, dbo.Modifier.branchid, dbo.Modifier.kdsid, dbo.Modifier.menugroupid, dbo.Modifier.Head,                          dbo.Type.TypeName FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId RIGHT OUTER JOIN                         dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId where dbo.Modifier.id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "' and dbo.Type.TypeName='Packing'";
                    }
                    else
                    {
                        q = "SELECT        dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.Modifier.uploadstatus, dbo.Modifier.branchid, dbo.Modifier.kdsid, dbo.Modifier.menugroupid, dbo.Modifier.Head,                          dbo.Type.TypeName FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId RIGHT OUTER JOIN                         dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId where dbo.Modifier.id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "' and dbo.Type.TypeName!='Packing'";

                    }

                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }
            }
            else if (type == "rmodifier")
            {
                //q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.RunTimeModifierId >0  and dbo.Saledetails.ModifierId =0  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId";
                //DataSet dscons = new DataSet();
                //dscons = objCore.funGetDataSet(q);
                //for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                //{
                //    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                //    if (temp == "")
                //    {
                //        temp = "0";
                //    }
                //    double qty = Convert.ToDouble(temp);
                //    q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'";
                //    if (cat.ToLower() == "packing")
                //    {
                //        q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'  and dbo.Type.TypeName='Packing'";
                //    }
                //    else
                //    {
                //        q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'  and dbo.Type.TypeName!='Packing'";
                //    }
                //    DataSet dsrecipyqty = new DataSet();
                //    dsrecipyqty = objCore.funGetDataSet(q);
                //    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                //    {
                //        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                //        if (temp == "")
                //        {
                //            temp = "0";
                //        }
                //        double recipeqty = Convert.ToDouble(temp);

                //        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString());

                //        totalqty = (recipeqty * qty);
                //        double rate = 0;
                //        DataSet dscon = new DataSet();
                //        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString() + "'";
                //        dscon = objCore.funGetDataSet(q);
                //        if (dscon.Tables[0].Rows.Count > 0)
                //        {
                //            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                //        }
                //        if (rate > 0)
                //        {
                //            rate = totalqty / rate;
                //        }
                //        double amount = prc * rate;

                //        amount = Math.Round(amount, 3);
                //        cost = cost + amount;
                //    }
                //}
            }
            else
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')   and dbo.Saledetails.ModifierId=0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }

                    string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if (rmodid == "")
                    {
                        rmodid = "0";
                    }

                    double qty = Convert.ToDouble(temp);
                    if (rmodid == "0")
                    {
                        if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                        {
                            // q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                            if (cat.ToLower() == "packing")
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.RuntimeModifier.type='Both')";

                            }
                            else
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.RuntimeModifier.type='Both')";
                            }
                        }
                        else
                        {
                            // q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";

                            if (cat.ToLower() == "packing")
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'  and dbo.Type.TypeName='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.RuntimeModifier.type='Both')";

                            }
                            else
                            {
                                q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.RuntimeModifier.type='Both')";
                            }

                        }
                    }
                    else
                    {
                        // q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + rmodid + "'";

                        if (cat.ToLower() == "packing")
                        {
                            q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName='Packing'";
                        }
                        else
                        {
                            q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName!='Packing'";
                        }


                    }
                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }
            }

            return cost;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Text = "Please Wait";
            vButton1.Enabled = false;
            bindreport();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }
        
    }
}
