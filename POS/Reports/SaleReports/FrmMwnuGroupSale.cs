﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmMwnuGroupSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmMwnuGroupSale()
        {
            InitializeComponent();
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
        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                //ds1.Tables[0].Rows.Add(dr1);
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
                DataSet ds1 = new DataSet();
                string q = "select distinct Terminal from sale ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["Terminal"] = "All Terminals";
                ds1.Tables[0].Rows.Add(dr1);
                comboBox1.DataSource = ds1.Tables[0];
                comboBox1.ValueMember = "Terminal";
                comboBox1.DisplayMember = "Terminal";

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
        public void bindreportmenugroup()
        {
            type = "group";
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptMenuGroupSale rptDoc = new Reports.SaleReports.rptMenuGroupSale();
            POSRestaurant.Reports.SaleReports.dsmenugroupreceipt ds = new Reports.SaleReports.dsmenugroupreceipt();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "DataTable1";
            dt = getAllOrdersmenugroup();
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
                ds.DataTable1.Merge(dt);
            }
            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("phn", "Menu Group Wise Sales Report");
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
        }
        static string type = "";
        public void bindreportmenuitem(string name)
        {
            type = "menu";
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptMenuGroupSale rptDoc = new Reports.SaleReports.rptMenuGroupSale();
            POSRestaurant.Reports.SaleReports.dsmenugroupreceipt ds = new Reports.SaleReports.dsmenugroupreceipt();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "DataTable1";
            dt = getAllOrdersmenuitem(name);
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
                ds.DataTable1.Merge(dt);
            }
            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("phn", "Menu Item Wise Sales Report of '" + name + "'");
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
        }
        public DataTable getAllOrdersmenugroup()
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("name", typeof(string));
                dtrptmenu.Columns.Add("quantity", typeof(double));
                dtrptmenu.Columns.Add("amount", typeof(double));
                dtrptmenu.Columns.Add("logo", typeof(byte[]));
                dtrptmenu.Columns.Add("perc", typeof(string));
                dtrptmenu.Columns.Add("cost", typeof(double));
                DataSet ds = new DataSet();
                string q = "";
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                double perc = 0, totalsale = 0, price = 0, runtimemsale = 0; ;

                DataSet dsperc = new DataSet();
                if (comboBox1.Text == "All Terminals")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and   dbo.Sale.BillStatus='Paid'";

                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and   dbo.Sale.BillStatus='Paid'  and   dbo.Sale.terminal='" +  comboBox1.Text + "'";
                }
                dsperc = objCore.funGetDataSet(q);
                if (dsperc.Tables[0].Rows.Count > 0)
                {
                    string temp = dsperc.Tables[0].Rows[0]["price"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    totalsale = Convert.ToDouble(temp);
                }


                //q = "SELECT     SUM(dbo.Saledetails.Price) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date = '" + date + "') GROUP BY dbo.MenuGroup.Name";
                if (comboBox1.Text == "All Terminals")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid'   and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid'  and dbo.Sale.terminal='"+comboBox1.Text+"'   and dbo.Saledetails.ModifierId=0 and dbo.Saledetails.RunTimeModifierId=0  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id";
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    perc = 0;  runtimemsale = 0; price = 0;
                    try
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid' AND (dbo.MenuGroup.Id = '" + ds.Tables[0].Rows[i]["id"] + "')  and  dbo.Saledetails.RunTimeModifierId>0  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id";
                      
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name, dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid' AND (dbo.MenuGroup.Id = '" + ds.Tables[0].Rows[i]["id"] + "')   and dbo.Sale.terminal='" + comboBox1.Text + "'   and  dbo.Saledetails.RunTimeModifierId>0  GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id";
                        }
                            DataSet dsrm = new DataSet();
                        dsrm = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsrm.Tables[0].Rows.Count; j++)
                        {
                            string temp = dsrm.Tables[0].Rows[0]["price"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            runtimemsale = Convert.ToDouble(temp);
                        }
                    }
                    catch (Exception ex)
                    {


                    }

                    string tempp = ds.Tables[0].Rows[i]["price"].ToString();
                    if (tempp == "")
                    {
                        tempp = "0";
                    }
                    price = runtimemsale + Convert.ToDouble(tempp);


                    
                    double cost = getcost(ds.Tables[0].Rows[i]["id"].ToString(), "");
                    perc = Math.Round(((price) / totalsale) * 100, 2);
                    if (logo == "")
                    {
                        dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), price, null, perc.ToString() + " %", cost);
                    }
                    else
                    {
                        dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), price, dscompany.Tables[0].Rows[0]["logo"], perc.ToString() + " %", cost);
                    }


                }
                ds = new DataSet();
                if (comboBox1.Text == "All Terminals")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid'   and dbo.Saledetails.ModifierId >0   ";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Paid'  and dbo.Sale.terminal='"+comboBox1.Text+"'   and dbo.Saledetails.ModifierId >0   ";

                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    //DataSet dsperc = new DataSet();
                    //q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and   dbo.Sale.BillStatus='Paid'   and dbo.Saledetails.ModifierId >0 and dbo.Saledetails.RunTimeModifierId=0";
                    //dsperc = objCore.funGetDataSet(q);
                    //if (dsperc.Tables[0].Rows.Count > 0)
                    //{
                    //    string temp = dsperc.Tables[0].Rows[0]["price"].ToString();
                    //    if (temp == "")
                    //    {
                    //        temp = "0";
                    //    }
                    //    totalsale = Convert.ToDouble(temp);
                    //}
                    double cost = getcost("", "modifier");
                    perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                    if (logo == "")
                    {
                        dtrptmenu.Rows.Add("Modifiers", Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), null, perc.ToString() + " %", cost);
                    }
                    else
                    {
                        dtrptmenu.Rows.Add("Modifiers", Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), dscompany.Tables[0].Rows[0]["logo"], perc.ToString() + " %", cost);
                    }


                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
        public DataTable getAllOrdersmenuitem(string name)
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("name", typeof(string));
                dtrptmenu.Columns.Add("quantity", typeof(double));
                dtrptmenu.Columns.Add("amount", typeof(double));
                dtrptmenu.Columns.Add("logo", typeof(byte[]));
                dtrptmenu.Columns.Add("perc", typeof(string));
                dtrptmenu.Columns.Add("cost", typeof(double));
                DataSet ds = new DataSet();
                string q = "";
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                if (name == "Modifiers")
                {
                    if (comboBox1.Text == "All Terminals")
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Head, dbo.Modifier.Name ,                          dbo.MenuGroup.Name AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuGroup ON dbo.Modifier.menugroupid = dbo.MenuGroup.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId > 0) AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.Price > 0) GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.MenuGroup.Name, dbo.Modifier.Head ORDER BY dbo.Modifier.Name";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Head, dbo.Modifier.Name ,                          dbo.MenuGroup.Name AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id INNER JOIN                         dbo.MenuGroup ON dbo.Modifier.menugroupid = dbo.MenuGroup.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.terminal='"+comboBox1.Text+"' AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId > 0) AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.Price > 0) GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.MenuGroup.Name, dbo.Modifier.Head ORDER BY dbo.Modifier.Name";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //string size = ds.Tables[0].Rows[i]["Expr1"].ToString();
                        //if (size.Length > 0)
                        //{
                        //    size = size.Substring(0, 1) + " '";
                        //}
                        double perc = 0, totalsale = 0;
                        DataSet dsperc = new DataSet();
                        //q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.MenuItem.MenuGroupId='" + ds.Tables[0].Rows[i]["mid"].ToString() + "' and dbo.Sale.BillStatus='Paid' and dbo.Saledetails.ModifierId='0' and dbo.Saledetails.RunTimeModifierId='0'  GROUP BY dbo.MenuGroup.Name";
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and    dbo.Sale.BillStatus='Paid'  and dbo.Saledetails.ModifierId>0 and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price>0";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.sale.terminal='" + comboBox1.Text + "' and    dbo.Sale.BillStatus='Paid'  and dbo.Saledetails.ModifierId>0 and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price>0";
                        }
                        dsperc = objCore.funGetDataSet(q);
                        if (dsperc.Tables[0].Rows.Count > 0)
                        {
                            string temp = dsperc.Tables[0].Rows[0]["price"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            totalsale = Convert.ToDouble(temp);
                        }
                        double cost = getcostmenu(ds.Tables[0].Rows[i]["ModifierId"].ToString(), "0", "modifier");
                        perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                        if (logo == "")
                        {
                            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Expr1"].ToString() + "-" + ds.Tables[0].Rows[i]["Head"].ToString() + "-" + ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), null, perc.ToString() + " %", cost);
                        }
                        else
                        {
                            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Expr1"].ToString() + "-" + ds.Tables[0].Rows[i]["Head"].ToString() + "-" + ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), dscompany.Tables[0].Rows[0]["logo"], perc.ToString() + " %", cost);
                        }
                    }


                    
                }
                else
                {
                    ds = new DataSet();
                    if (comboBox1.Text == "All Terminals")
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuItem.Name, dbo.MenuItem.id,dbo.MenuGroup.id as mid, dbo.Saledetails.Flavourid,                      dbo.ModifierFlavour.name AS Expr1 FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                      dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE   dbo.MenuGroup.Name='" + name + "' and  (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid') and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0   GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.id,dbo.MenuGroup.id,dbo.Saledetails.Flavourid   ORDER BY dbo.MenuItem.Name";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuItem.Name, dbo.MenuItem.id,dbo.MenuGroup.id as mid, dbo.Saledetails.Flavourid,                      dbo.ModifierFlavour.name AS Expr1 FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                      dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE   dbo.MenuGroup.Name='" + name + "' and  (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.terminal='"+comboBox1.Text+"' and  (dbo.Sale.BillStatus = 'Paid') and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0   GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.id,dbo.MenuGroup.id,dbo.Saledetails.Flavourid   ORDER BY dbo.MenuItem.Name";
                    }
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        string size = ds.Tables[0].Rows[i]["Expr1"].ToString();
                        if (size.Length > 0)
                        {
                            size = size.Substring(0, 1) + " '";
                        }
                        double perc = 0, totalsale = 0;
                        DataSet dsperc = new DataSet();
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  dbo.MenuItem.MenuGroupId='" + ds.Tables[0].Rows[i]["mid"].ToString() + "' and   dbo.Sale.BillStatus='Paid'  and dbo.Saledetails.ModifierId=0";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.sale.terminal='"+comboBox1.Text+"' and  dbo.MenuItem.MenuGroupId='" + ds.Tables[0].Rows[i]["mid"].ToString() + "' and   dbo.Sale.BillStatus='Paid'  and dbo.Saledetails.ModifierId=0";
                        }
                        dsperc = objCore.funGetDataSet(q);
                        if (dsperc.Tables[0].Rows.Count > 0)
                        {
                            string temp = dsperc.Tables[0].Rows[0]["price"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            totalsale = Convert.ToDouble(temp);
                        }
                        double cost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "");
                        perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                        if (logo == "")
                        {
                            dtrptmenu.Rows.Add(size + ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), null, perc.ToString() + " %", cost);
                        }
                        else
                        {
                            dtrptmenu.Rows.Add(size + ds.Tables[0].Rows[i]["Name"].ToString(), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()), dscompany.Tables[0].Rows[0]["logo"], perc.ToString() + " %", cost);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
        
        protected void getbranchtype()
        {
            try
            {
                string q = "select type from branch";
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
        }
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
        public static string branchtype = "";
        public double getcost(string id, string type)
        {
            double cost = 0, totalqty = 0;

            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            if (type == "modifier")
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.Saledetails.ModifierId>0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
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
            else
            {
                getbranchtype();
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid , dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "' and dbo.Saledetails.ModifierId=0 GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId";
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
                    string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if (rmodid == "")
                    {
                        rmodid = "0";
                    }
                    string modid = dscons.Tables[0].Rows[i]["ModifierId"].ToString();
                    if (modid == "")
                    {
                        modid = "0";
                    }

                    if (modid != "0")
                    {
                        q = "SELECT RawItemId, Quantity from Modifier where id='0' ";
                    }
                    else
                    {
                        if (rmodid != "0")
                        {
                            q = "SELECT RawItemId, Quantity from RuntimeModifier where id='" + rmodid + "' ";
                        }
                        else
                        {
                            if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'   and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                            }
                            else
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                            }
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
        public double getcostmenu(string id, string mid, string type)
        {
            double cost = 0, totalqty = 0;

            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            if (type == "modifier")
            {
                if (comboBox1.Text == "All Terminals")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.ModifierId ='" + id + "'    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.sale.terminal='"+comboBox1.Text+"' and  dbo.Saledetails.ModifierId ='" + id + "'    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
                }
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
                if (comboBox1.Text == "All Terminals")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.RunTimeModifierId ='" + id + "'  and dbo.Saledetails.ModifierId =0  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.sale.terminal='"+comboBox1.Text+"' and  dbo.Saledetails.RunTimeModifierId ='" + id + "'  and dbo.Saledetails.ModifierId =0  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId";
                }
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
                getbranchtype();
                if (comboBox1.Text == "All Terminals")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.sale.terminal='"+comboBox1.Text+"' and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                }
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
                            q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                        }
                        else
                        {
                            q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                        }
                    }
                    else
                    {
                        q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + rmodid + "'";

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
            bindreportmenugroup();
        }
        public void bindreportcost(string mid, string size, string name)
        {
            type = "menu";
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptcost rptDoc = new Reports.SaleReports.rptcost();
            POSRestaurant.Reports.SaleReports.dscost ds = new Reports.SaleReports.dscost();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "DataTable1";
            dt = getcostofmenu(mid, size);
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
                ds.DataTable1.Merge(dt);
            }
            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("phn", "Cost Report of '" + name + "'");
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
        }

        public DataTable getcostofmenu(string mid, string size)
        {
            DataTable dtrptmenu = new DataTable();

            dtrptmenu.Columns.Add("Name", typeof(string));
            dtrptmenu.Columns.Add("RQuantity", typeof(double));
            dtrptmenu.Columns.Add("UnitPrice", typeof(double));
            dtrptmenu.Columns.Add("Cost", typeof(double));
            dtrptmenu.Columns.Add("SQuantity", typeof(double));
            dtrptmenu.Columns.Add("Amount", typeof(double));
            dtrptmenu.Columns.Add("logo", typeof(byte[]));
            double cost = 0, totalqty = 0;
            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {
            }
            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";

            {
                if (size == "0")
                {
                    size = "";
                }
                if (size.Length > 0)
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid , dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.Saledetails.MenuItemId='" + mid + "' and dbo.Saledetails.Flavourid='" + size + "' GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.Saledetails.MenuItemId='" + mid + "' GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId";
                }
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string runtimeid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if (runtimeid == "")
                    {
                        runtimeid = "0";
                    }

                    string modifyid = dscons.Tables[0].Rows[i]["ModifierId"].ToString();
                    if (modifyid == "")
                    {
                        modifyid = "0";
                    }


                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);

                    if (modifyid == "0")
                    {
                        if (runtimeid == "0")
                        {
                            if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                            }
                            else
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                            }
                        }
                        else
                        {
                            q = "SELECT rawitemid , Quantity from RuntimeModifier where id='" + runtimeid + "' ";
                        }
                    }
                    else
                    {
                        q = "SELECT rawitemid , Quantity from Modifier where id='" + modifyid + "' ";
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

                        DataSet dspurchase = new DataSet();                                                                     

                        string name = "";
                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                            name = dscon.Tables[0].Rows[0]["ItemName"].ToString() + "(" + dscon.Tables[0].Rows[0]["UOM"].ToString() + ")";
                        }
                        //if (rate > 0)
                        //{
                        //    rate = totalqty / rate;
                        //}
                        //double amount = prc * rate;

                        //amount = Math.Round(amount, 3);
                        //cost = cost + amount;
                        cost = (prc / rate) * recipeqty;
                        if (name != "")
                        {
                            if (logo == "")
                            {
                                dtrptmenu.Rows.Add(name, recipeqty, prc / rate, cost, qty, qty * cost, null);
                            }
                            else
                            {
                                dtrptmenu.Rows.Add(name, recipeqty, prc / rate, cost, qty, qty * cost, dscompany.Tables[0].Rows[0]["logo"]);
                            }
                        }

                    }
                }

            }

            return dtrptmenu;
        }
        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
            //String sObjectProperties = "Name: " + info.Name + "\nText: " + info.Text + "\nObject Type: " + info.ObjectType + "\nToolTip: " + info.ToolTip + "\nDataContext: " + info.DataContext + "\nGroup Name Path: " + info.GroupNamePath + "\nHyperlink: " + info.Hyperlink;
            string name = info.Text;
            if (type == "group")
            {
                bindreportmenuitem(name);
            }
            else
            {
                string size = "", id = "";
                if (name.Substring(0, 2).Contains("'"))
                {
                    size = name.Substring(0, 2);
                    name = name.Substring(2);
                }
                string q = "select id from menuitem where name ='" + name + "'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    id = ds.Tables[0].Rows[0][0].ToString();
                }
                if (size.Length > 0)
                {
                    q = "select id from ModifierFlavour where MenuItemId ='" + id + "'";
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        size = ds.Tables[0].Rows[0][0].ToString();
                    }
                }
                bindreportcost(id, size, size + name);
            }
            vButton2.Visible = true;
            //MessageBox.Show(sObjectProperties);
        }

        private void vButton2_Click(object sender, EventArgs e)
        {

            bindreportmenugroup();
            vButton2.Visible = true;
        }
    }
}