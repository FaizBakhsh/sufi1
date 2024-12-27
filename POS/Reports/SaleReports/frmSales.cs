using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.SqlClient;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.Net.Mail;
using POSRestaurant.Reports.SaleReports;
using System.IO;
using System.Net;
namespace POSRestaurant.Reports
{
    public partial class frmSales : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public frmSales()
        {
            InitializeComponent();

        }
        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            POSRestaurant.Reports.SaleReports.rptdaily rptDoc = new SaleReports.rptdaily();
            POSRestaurant.Reports.SaleReports.DsUserDaily ds = new SaleReports.DsUserDaily();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

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
                ds.Dt1.Merge(dt, true, MissingSchemaAction.Ignore);
            }
            else
            {
                if (logo == "")
                { }
                else
                {
                    dt.Rows.Add("", "", "", dscompany.Tables[0].Rows[0]["logo"]);
                    ds.Dt1.Merge(dt, true, MissingSchemaAction.Ignore);
                }
            }
            DataTable dtuser = new DataTable();
            dtuser.TableName = "Crystal Report User";
            dtuser = getAllOrdersuser();
            // Just set the name of data table


            ds.DataTable1.Merge(dtuser);

            DataTable dtmenu = new DataTable();
            dtmenu.TableName = "Crystal Report Menu";
            dtmenu = getAllOrdersmenu();
            // Just set the name of data table


            ds.MenuGroup.Merge(dtmenu);

            rptDoc.SetDataSource(ds);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("Addrs", address);
            rptDoc.SetParameterValue("phn", phone);
            rptDoc.SetParameterValue("visa", visa);
            rptDoc.SetParameterValue("visaamounts", visaamounts);
            rptDoc.SetParameterValue("report", "Sales Report");
            delievrysourcetitle = "";
            delievrysourcedata = "";
            deliverysource();
            rptDoc.SetParameterValue("deliverysourcetitle", delievrysourcetitle);


            rptDoc.SetParameterValue("deliverysourcedata", delievrysourcedata);
            if (takawayorders == "")
            {
                takawayorders = "0";
            }
            if (deliveryorders == "")
            {
                deliveryorders = "0";
            }
            rptDoc.SetParameterValue("takeawayorders", takawayorders);
            rptDoc.SetParameterValue("dineinorders", dineinorders);
            rptDoc.SetParameterValue("deliveryorders", deliveryorders);
            rptDoc.SetParameterValue("recvname", recvname);
            rptDoc.SetParameterValue("recvamount", recvamount);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
            cryRpt = rptDoc;
        }

        string delievrysourcetitle = "", delievrysourcedata = "", takawayorders = "", dineinorders = "", deliveryorders = "";
        protected void deliverysource()
        {
            string val = "";
            string q = "";

            try
            {
                if (cmbshift.Text == "All")
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT      count(dbo.Sale.Id) as count,  SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'  GROUP BY dbo.Delivery.type ";
                        }
                        else
                        {
                            q = "SELECT       count(dbo.Sale.Id) as count, SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.sale.terminal='" + comboBox1.Text + "'  GROUP BY dbo.Delivery.type ";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT       count(dbo.Sale.Id) as count, SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' GROUP BY dbo.Delivery.type ";
                        }
                        else
                        {
                            q = "SELECT       count(dbo.Sale.Id) as count, SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.sale.terminal='" + comboBox1.Text + "'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.Delivery.type ";

                        }
                    }
                }
                else
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT       count(dbo.Sale.Id) as count, SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";
                        }
                        else
                        {
                            q = "SELECT      count(dbo.Sale.Id) as count,  SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.sale.terminal='" + comboBox1.Text + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT       count(dbo.Sale.Id) as count, SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";
                        }
                        else
                        {
                            q = "SELECT      count(dbo.Sale.Id) as count,  SUM(dbo.Sale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.Sale INNER JOIN                          dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.sale.terminal='" + comboBox1.Text + "'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";

                        }
                    }
                }
                DataSet dssource = new DataSet();
                dssource = objcore.funGetDataSet(q);
                if (dssource.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dssource.Tables[0].Rows.Count; i++)
                    {
                        if (delievrysourcetitle.Length > 0)
                        {
                            delievrysourcetitle = delievrysourcetitle + " \n";
                        }
                        delievrysourcetitle = delievrysourcetitle + dssource.Tables[0].Rows[i]["type"].ToString() + "      (" + dssource.Tables[0].Rows[i]["count"].ToString() + ")";

                        if (delievrysourcedata.Length > 0)
                        {
                            delievrysourcedata = delievrysourcedata + " \n";
                        }
                        string temp = dssource.Tables[0].Rows[i]["amount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        delievrysourcedata = delievrysourcedata + Math.Round(Convert.ToDouble(temp), 2).ToString("N");

                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
        public string visa = "", visaamounts = "";
        public DataTable getAllOrdersmenu()
        {

            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("MenuGroup", typeof(string));
                dtrptmenu.Columns.Add("Count", typeof(string));
                dtrptmenu.Columns.Add("Sum", typeof(double));
                dtrptmenu.Columns.Add("CashSales", typeof(double));
                dtrptmenu.Columns.Add("CardSales", typeof(double));


                DataSet ds = new DataSet();
                string q = "";

                if (cmbcashier.Text == "All")
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }

                    }
                    else
                    {

                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }

                    }
                }
                else
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    Sale.UserId='" + cmbcashier.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    Sale.UserId='" + cmbcashier.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    Sale.UserId='" + cmbcashier.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE   Sale.UserId='" + cmbcashier.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    Sale.UserId='" + cmbcashier.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    Sale.UserId='" + cmbcashier.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE    Sale.UserId='" + cmbcashier.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }

                    }
                    else
                    {

                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid WHERE  Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                                }
                            }
                        }

                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString(), getvalue(ds.Tables[0].Rows[i]["id"].ToString(), "Cash"), getvalue(ds.Tables[0].Rows[i]["id"].ToString(), "Card"));
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }

        protected double getvalue(string id, string type)
        {
            double val = 0;
            DataSet ds = new DataSet();
            string q = "";

            try
            {
                if (cmbshift.Text == "All")
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "' and sale.GSTtype like '" + type + "%' GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "' and sale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "') and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "' and sale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "'  and sale.GSTtype like '" + type + "%'  GROUP BY dbo.MenuGroup.Name";

                        }
                    }
                }
                else
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.BillStatus = 'Paid')  and (Sale.shiftid = '" + cmbshift.SelectedValue + "') and menugroup.id='" + id + "' and sale.GSTtype like '" + type + "%' GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.terminal='" + comboBox1.Text + "')   and (Sale.shiftid = '" + cmbshift.SelectedValue + "') and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "' and sale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (Sale.branchid='" + cmbbranch.SelectedValue + "')  and (Sale.shiftid = '" + cmbshift.SelectedValue + "')  and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "' and sale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Price  - dbo.Saledetails.Itemdiscount) AS sum, COUNT(dbo.Saledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (Sale.branchid='" + cmbbranch.SelectedValue + "')   and (Sale.shiftid = '" + cmbshift.SelectedValue + "')  and (Sale.terminal='" + comboBox1.Text + "') and (Sale.BillStatus = 'Paid') and menugroup.id='" + id + "'  and sale.GSTtype like '" + type + "%'  GROUP BY dbo.MenuGroup.Name";

                        }
                    }
                }

                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0]["sum"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    val = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {

            }
            return val;
        }
        public DataTable getAllOrdersuser()
        {

            DataTable dtrptuser = new DataTable();
            try
            {
                dtrptuser.Columns.Add("User", typeof(string));
                dtrptuser.Columns.Add("Count", typeof(string));
                dtrptuser.Columns.Add("Sum", typeof(string));


                DataSet ds = new DataSet();
                string q = "";


                q = "SELECT     SUM(dbo.Sale.NetBill) AS sum, COUNT(dbo.Sale.NetBill) AS count,SUM(dbo.Sale.DiscountAmount) AS dis, dbo.Users.Name,dbo.Users.id FROM         dbo.Sale INNER JOIN                      dbo.Users ON dbo.Sale.UserId = dbo.Users.Id   WHERE     (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='Paid'  GROUP BY dbo.Users.Name,dbo.Users.id ";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double refnd = 0, disc = 0;
                    DataSet dsref = new DataSet();
                    //q = "SELECT     sum( NetBill) FROM         Sale where UserId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and BillStatus='Refund'";
                    //q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "')   and dbo.Sale.UserId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and BillStatus='Refund'";
                    //dsref = objCore.funGetDataSet(q);
                    //if (dsref.Tables[0].Rows.Count > 0)
                    //{
                    //    string temp = dsref.Tables[0].Rows[0]["cash"].ToString();
                    //    if (temp == "")
                    //    {
                    //        temp = "0";
                    //    }
                    //    refnd = Convert.ToDouble(temp);
                    //}
                    string val = ds.Tables[0].Rows[i]["dis"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    disc = Convert.ToDouble(val);
                    val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double sum = Convert.ToDouble(val);
                    double total = 0;
                    try
                    {
                        total = sum - refnd;
                        //total = total - disc;
                    }
                    catch (Exception ex)
                    {

                    }
                    total = Math.Round(total, 2);
                    dtrptuser.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), total.ToString());
                    //dtrptuser.Rows.Add(ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString());
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrptuser;
        }
        DataSet dscompany = new DataSet();
        DataSet dsbranch = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");
            dsbranch = objCore.funGetDataSet("select * from Branch");
        }
        protected double getcomplimentary()
        {
            double val = 0;
            try
            {


                //q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0)  and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price";
                string q = "";
                q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                if (cmbshift.Text == "All")
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.terminal='" + comboBox1.Text + "'   and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                    }
                }
                else
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid'  and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.branchid='" + cmbbranch.SelectedValue + "' and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";

                        }
                    }
                }


                DataSet dsdis = new DataSet();
                dsdis = new DataSet();
                dsdis = objcore.funGetDataSet(q);
                IList<complimentoryClass> data = dsdis.Tables[0].AsEnumerable().Select(row =>
                 new complimentoryClass
                 {
                     Amount = row.Field<double>("amount"),
                     Quantity = row.Field<double>("Quantity")


                 }).ToList();
                if (cmbshift.Text == "All")
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.billstatus='Paid' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.billstatus='Paid' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.terminal='" + comboBox1.Text + "'   and sale.billstatus='Paid' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.terminal='" + comboBox1.Text + "'   and sale.billstatus='Paid' GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                    }
                }
                else
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid'  and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.branchid='" + cmbbranch.SelectedValue + "' and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.branchid='" + cmbbranch.SelectedValue + "' and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, SUM(POSFee) AS POSFee FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";
                            q = "SELECT        ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) * SUM(dbo.Saledetails.Quantity) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.MenuItem.Name, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND ((CONVERT(float, dbo.MenuItem.Price)) + (CONVERT(float, ISNULL(dbo.ModifierFlavour.price, '0')))) > 0 and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')   and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuItem.Name, dbo.Sale.Id, dbo.Sale.Date, dbo.ModifierFlavour.name,dbo.Sale.customer, dbo.MenuItem.Price, dbo.ModifierFlavour.price order by dbo.Sale.Date";
                            q = "SELECT       ((CONVERT(float, dbo.RuntimeModifier.Price))  * SUM(dbo.Saledetails.Quantity)) AS amount, SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Sale.Id,dbo.Sale.customer, dbo.Sale.Date, dbo.RuntimeModifier.name FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND (dbo.Saledetails.Price = 0) AND (dbo.RuntimeModifier.price > 0) and (sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "')    and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.RuntimeModifier.Name, dbo.Sale.Id, dbo.Sale.Date,dbo.Sale.customer,dbo.RuntimeModifier.Price";

                        }
                    }
                }

                DataSet dsdis1 = new DataSet();
                dsdis1 = new DataSet();
                dsdis1 = objcore.funGetDataSet(q);
                IList<complimentoryClass> data1 = dsdis1.Tables[0].AsEnumerable().Select(row =>
                 new complimentoryClass
                 {
                     Amount = row.Field<double>("amount"),
                     Quantity = row.Field<double>("Quantity")


                 }).ToList();

                double qty = 0, amount = 0;

                qty = data.Sum(s => s.Quantity);
                amount = data.Sum(s => s.Amount);

                //qty = qty + data1.Sum(s => s.Quantity);
                //amount = amount + data1.Sum(s => s.Amount);


                val = Math.Round(Convert.ToDouble(amount), 3);

            }
            catch (Exception exc)
            {


            }

            return val;
        }
        string recvname = "", recvamount = "";
        public DataTable getAllOrders()
        {

            DataTable dat = new DataTable();
            dat.Columns.Add("GrossSale", typeof(double));
            dat.Columns.Add("GST", typeof(double));
            dat.Columns.Add("Discount", typeof(double));
            dat.Columns.Add("NetSale", typeof(double));
            dat.Columns.Add("CashSale", typeof(double));
            dat.Columns.Add("CreditSale", typeof(double));
            dat.Columns.Add("MasterSale", typeof(double));
            dat.Columns.Add("DinIn", typeof(double));
            dat.Columns.Add("TakeAway", typeof(double));
            dat.Columns.Add("Delivery", typeof(double));
            dat.Columns.Add("Refund", typeof(double));
            dat.Columns.Add("Void", typeof(double));
            dat.Columns.Add("Dlorders", typeof(string));
            dat.Columns.Add("Torders", typeof(string));
            dat.Columns.Add("Dorders", typeof(string));
            dat.Columns.Add("RefundNo", typeof(string));
            dat.Columns.Add("logo", typeof(byte[]));
            dat.Columns.Add("totalorders", typeof(double));
            dat.Columns.Add("averagesale", typeof(double));
            dat.Columns.Add("CarHope", typeof(double));
            dat.Columns.Add("CarHopeorders", typeof(string));
            dat.Columns.Add("calculatedcash", typeof(double));
            dat.Columns.Add("float", typeof(double));
            dat.Columns.Add("bankingtotal", typeof(double));
            dat.Columns.Add("declared", typeof(double));
            dat.Columns.Add("over", typeof(double));
            dat.Columns.Add("total", typeof(double));
            dat.Columns.Add("servicecharges", typeof(double));
            dat.Columns.Add("receivables", typeof(double));
            dat.Columns.Add("CashGST", typeof(double));
            dat.Columns.Add("CardGST", typeof(double));
            dat.Columns.Add("CashDiscount", typeof(double));
            dat.Columns.Add("CardDiscount", typeof(double));
            dat.Columns.Add("CashRecv", typeof(double));
            dat.Columns.Add("CardRecv", typeof(double));
            dat.Columns.Add("avgdinein", typeof(double));
            dat.Columns.Add("avgdineintable", typeof(double));
            dat.Columns.Add("POSFee", typeof(double));
            dat.Columns.Add("Comp", typeof(double));
            dat.Columns.Add("DlvCharges", typeof(double));
            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }
            double gross = 0, cashgst = 0, posfee = 0, complimtry = 0, cardgst = 0, gst = 0, cashdis = 0, carddis = 0,DlvCharges=0, discount = 0, net = 0, service = 0, cashrecv = 0, cardrecv = 0, recv = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidsale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
            string Dlorders = "0", Torders = "0", Dorders = "0", RefundNo = "0", carhopeorders = "0" ;

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {

                string temp = "";
                string q = "";
                try
                {
                    ds = new DataSet();

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount,sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and   (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and   dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "' and sale.billstatus='Paid'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sale.terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(dbo.Sale.TotalBill) AS gross, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.TotalBill) AS netsale, SUM(dbo.Sale.servicecharges) AS serv, SUM(dbo.Sale.DiscountAmount) AS discount, sum(ISNULL(DeliveryAmt,0)) AS DlvCharges FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and sale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and sale.billstatus='Paid' and sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                    }

                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        complimtry = getcomplimentary();
                        discount = discount + Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                        gross = Convert.ToDouble(ds.Tables[0].Rows[0]["gross"].ToString());
                        gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                        try
                        {
                            DlvCharges = Convert.ToDouble(ds.Tables[0].Rows[0]["DlvCharges"].ToString());
                        }
                        catch (Exception ex)
                        {

                        }
                        net = Convert.ToDouble(ds.Tables[0].Rows[0]["netsale"].ToString());
                        net = net - discount;
                        gross = gross + gst + DlvCharges;
                        net = Math.Round(net, 2);
                        service = Convert.ToDouble(ds.Tables[0].Rows[0]["serv"].ToString());
                        gross = gross + service;
                        gross = gross + complimtry;
                        if (cmbcashier.Text == "All")
                        {
                            if (cmbservers.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbservers.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where UserId='" + cmbcashier.SelectedValue + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.terminal='" + comboBox1.Text + "'  and dbo.Sale.GSTtype='Cash' and dbo.Sale.billstatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                        }
                        DataSet dscash = new DataSet();
                        dscash = objcore.funGetDataSet(q);
                        if (dscash.Tables[0].Rows.Count > 0)
                        {
                            temp = dscash.Tables[0].Rows[0]["gst"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            cashgst = Convert.ToDouble(temp);
                            temp = dscash.Tables[0].Rows[0]["discount"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            cashdis = Convert.ToDouble(temp);
                        }
                        if (cmbcashier.Text == "All")
                        {
                            if (cmbservers.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbservers.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where UserId='" + cmbcashier.SelectedValue + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where   UserId='" + cmbcashier.SelectedValue + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale where  UserId='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid'";

                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                    else
                                    {
                                        if (comboBox1.Text == "All Terminals")
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                        else
                                        {
                                            q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netsale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where   sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                        }
                                    }
                                }
                            }
                        }
                        dscash = new DataSet();
                        dscash = objcore.funGetDataSet(q);
                        if (dscash.Tables[0].Rows.Count > 0)
                        {
                            temp = dscash.Tables[0].Rows[0]["gst"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            cardgst = Convert.ToDouble(temp);
                            temp = dscash.Tables[0].Rows[0]["discount"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            carddis = Convert.ToDouble(temp);
                        }
                    }
                    else
                    {
                        gross = 0;
                        gst = 0;
                        discount = 0;
                        net = 0;
                    }
                }
                catch (Exception ex)
                {


                }

                try
                {
                    ds = new DataSet();

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where   dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                    }

                    //q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Cash' and dbo.Sale.BillStatus='Paid'";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cash = Convert.ToDouble(temp);
                        //calculatedcash = cash;
                        //cash = cash - discount;
                    }
                    else
                    {
                        cash = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    q = "";// "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Cash'";
                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {


                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type='Cash' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                            }
                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        //cash = Convert.ToDouble(temp);
                        calculatedcash = Convert.ToDouble(temp);
                        //cash = cash - discount;
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {


                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.BillType='Master Card' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType='Master Card'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        master = Convert.ToDouble(temp);
                    }
                    else
                    {
                        master = 0;
                    }
                }
                catch (Exception ex)
                {
                }
                try
                {
                    ds = new DataSet();
                    // q = "SELECT     SUM(NetBill) AS cash  FROM         Sale where  (Date = '" + dateTimePicker2.Text + "') and BillType='Credit Card'";

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                        }
                    }
                    else
                    {

                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and   (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount)AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount)as cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and   dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash, dbo.BillType.type  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type like '%Visa%'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.BillType.type";

                                    }
                                }
                            }
                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            temp = ds.Tables[0].Rows[i]["cash"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            credit = credit + Convert.ToDouble(temp);
                            if (visa.Length > 0)
                            {
                                visa = visa + " , ";
                                visaamounts = visaamounts + " , ";
                            }

                            visa = visa + ds.Tables[0].Rows[i]["type"].ToString();
                            visaamounts = visaamounts + Math.Round(Convert.ToDouble(temp), 2);
                        }
                    }
                    else
                    {
                        credit = 0;
                    }
                }
                catch (Exception ex)
                {

                    credit = 0;
                }
                try
                {
                    ds = new DataSet();

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'   and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.terminal='" + comboBox1.Text + "'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        //q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'   and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.terminal='" + comboBox1.Text + "'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  GROUP BY dbo.Customers.Name";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        //q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.Customers.Name";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                        // q = "SELECT        SUM(dbo.BillType.Amount) AS cash, dbo.Customers.Name FROM            dbo.BillType INNER JOIN                         dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id INNER JOIN                         dbo.Customers ON dbo.BillType.recvid = dbo.Customers.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   GROUP BY dbo.Customers.Name";

                                    }
                                }
                            }
                        }
                    }
                    recv = 0;
                    ds = objcore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        temp = ds.Tables[0].Rows[i]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        recv = recv + Convert.ToDouble(temp);
                        try
                        {
                            if (recvamount.Length > 0)
                            {
                                recvamount = recvamount + "," + temp;
                            }
                            else
                            {
                                recvamount = temp;
                            }
                            if (recvname.Length > 0)
                            {
                                recvname = recvname + "," + ds.Tables[0].Rows[i]["Name"].ToString();
                            }
                            else
                            {
                                recvname = ds.Tables[0].Rows[i]["Name"].ToString();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                }
                catch (Exception ex)
                {


                }


                try
                {
                    ds = new DataSet();

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                            q = "";
                        }
                    }
                    else
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id  INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Cash'";

                                    }
                                }
                            }
                            q = "";
                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cashrecv = Convert.ToDouble(temp);
                    }
                    else
                    {
                        cashrecv = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {

                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where  dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Card'";
                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.BillType.type ='Receivable'  and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.BillStatus='Paid' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.Sale.GSTtype='Card'";

                                    }
                                }
                            }
                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        cardrecv = Convert.ToDouble(temp);
                    }
                    else
                    {
                        cardrecv = 0;
                    }
                }
                catch (Exception ex)
                {


                }


                try
                {
                    ds = new DataSet();
                    if (cmbcashier.Text == "All")
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                //if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                                    }
                                    else
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  terminal='" + comboBox1.Text + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";

                                    }
                                }
                            }
                            else
                            {
                                //if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  terminal='" + comboBox1.Text + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }

                            }
                        }
                    }
                    else
                    {
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                //if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                                    }
                                    else
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  terminal='" + comboBox1.Text + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";

                                    }
                                }
                            }
                            else
                            {
                                //if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT   SUM(cashin) AS cashin,SUM(cashout) AS cashout FROM  shiftcash where  terminal='" + comboBox1.Text + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }

                            }
                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        temp = ds.Tables[0].Rows[0]["cashin"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        drawerfloat = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[0]["cashout"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        declared = Convert.ToDouble(temp);
                        //calculatedcash = calculatedcash + drawerfloat;
                        total = calculatedcash + drawerfloat;
                        over = declared - total;
                        bankingtotal = declared - drawerfloat;
                    }

                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (cmbservers.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' ";
                        }
                        else
                        {
                            q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         Sale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' and terminal='" + comboBox1.Text + "'";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash, count(dbo.Sale.id) as count FROM         dbo.Sale   INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Refund' ";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash, count(dbo.Sale.id) as count FROM         dbo.Sale   INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.BillStatus='Refund' and dbo.Sale.terminal='" + comboBox1.Text + "'";

                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["cash"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        refund = Convert.ToDouble(temp);
                        gross = gross + refund;
                        // net = net - refund;
                        temp = ds.Tables[0].Rows[0]["count"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        //gross = gross - refund;
                        RefundNo = temp;
                    }
                    else
                    {
                        refund = 0;
                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();

                    if (cmbcashier.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }

                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and   dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and   dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and   dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and   dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Refund' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }

                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["Expr1"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        refund = Convert.ToDouble(temp);
                        // gross = gross + refund;
                        // net = net - refund;

                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();


                    if (cmbcashier.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";

                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT SUM(dbo.Saledetailsrefund.Price + dbo.Saledetailsrefund.ItemGst - dbo.Saledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.Saledetailsrefund INNER JOIN               dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id where   dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.Saledetailsrefund.type='Void' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "' ";

                                }
                            }
                        }
                    }
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                        temp = ds.Tables[0].Rows[0]["Expr1"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        voidsale = Convert.ToDouble(temp);


                    }
                    else
                    {
                        voidsale = 0;
                    }
                }
                catch (Exception ex)
                {


                }

            }
            catch (Exception ex)
            {

            }
            double totlorder = 0;// (Convert.ToDouble(Torders) + Convert.ToDouble(Dorders) + Convert.ToDouble(Dlorders));
            try
            {
                ds = new DataSet();
                string q = "";

                if (cmbcashier.Text == "All")
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where billstatus='Paid' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";

                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where   billstatus='Paid' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and branchid='" + cmbbranch.SelectedValue + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where sale.billstatus='Paid' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where sale.billstatus='Paid' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where sale.billstatus='Paid' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where sale.billstatus='Paid' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where sale.billstatus='Paid' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where sale.billstatus='Paid' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                }
                else
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and userid='" + cmbcashier.SelectedValue + "' and (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  userid='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  userid='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  userid='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and branchid='" + cmbbranch.SelectedValue + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  userid='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  userid='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  userid='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(id) AS cash  FROM         Sale where  billstatus='Paid' and  userid='" + cmbcashier.SelectedValue + "' and  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and  dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where sale.billstatus='Paid' and  dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     count(Sale.id) AS cash  FROM         Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  sale.billstatus='Paid' and dbo.Sale.userid='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.terminal='" + comboBox1.Text + "' and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.Sale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                }
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //refund = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    totlorder = Convert.ToDouble(temp);
                }
                else
                {
                    totlorder = 0;
                }
            }
            catch (Exception ex)
            {


            }
            double avgsale = 0;
            try
            {
                if (gross == 0)
                {
                }
                else
                {
                    avgsale = (net) / totlorder;
                }
            }
            catch (Exception ex)
            {


            }
            string sht = "", username = "";
            DataSet dsshft = new DataSet();
            dsshft = objcore.funGetDataSet("select * from Shifts where id='2'");
            if (dsshft.Tables[0].Rows.Count > 0)
            {
                // sht = dsshft.Tables[0].Rows[0]["Name"].ToString();
            }

            try
            {
                string q = "";
                ds = new DataSet();

                if (cmbcashier.Text == "All")
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                }
                else
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN  dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Take Away' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                }
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    takeaway = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    takawayorders = temp;
                }
                else
                {
                    takeaway = 0;
                    takawayorders = "0";
                }


            }
            catch (Exception ex)
            {


            }
            double avgdinein = 0, avgdineintable = 0;
            try
            {
                string q = "";
                ds = new DataSet();

                if (cmbcashier.Text == "All")
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                }
                else
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                    else
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale INNER JOIN dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "' and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                }
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    dinin = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    dineinorders = temp;

                    try
                    {
                        if (cmbcashier.Text == "All")
                        {
                            if (cmbservers.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                    }
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid   where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (cmbservers.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                    }
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid   where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                                    }
                                }
                                else
                                {
                                    if (cmbbranch.Text == "All")
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.Sale INNER JOIN                         dbo.DinInTables ON dbo.Sale.Id = dbo.DinInTables.Saleid  where  dbo.Sale.UserId='" + cmbcashier.SelectedValue + "'  and  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Dine In' and dbo.Sale.BillStatus='Paid'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' ";
                                    }
                                }
                            }
                        }
                        ds = new DataSet();
                        ds = objcore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {

                            temp = ds.Tables[0].Rows[0]["count"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            avgdineintable = dinin / Convert.ToDouble(dineinorders);
                            dineinorders = dineinorders + "/" + temp;
                            avgdinein = dinin / Convert.ToDouble(temp);

                        }

                    }
                    catch (Exception ex)
                    {


                    }

                }
                else
                {
                    dinin = 0;
                    dineinorders = "0";
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                string q = "";
                ds = new DataSet();
                if (cmbcashier.Text == "All")
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }

                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash ,count(dbo.sale.id) as count FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                }
                else
                {
                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'";
                            }

                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash ,count(dbo.sale.id) as count FROM  Sale where dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and  (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Sale.TotalBill + dbo.Sale.GST - dbo.Sale.DiscountAmount) AS cash,count(dbo.sale.id) as count  FROM  Sale where  dbo.Sale.Userid='" + cmbcashier.SelectedValue + "' and (dbo.Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Sale.OrderType='Delivery' and dbo.Sale.BillStatus='Paid' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                        }
                    }
                }
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    // credit = Convert.ToDouble(ds.Tables[0].Rows[0]["cash"].ToString());
                    string temp = ds.Tables[0].Rows[0]["cash"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    delivery = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["count"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    deliveryorders = temp;
                }
                else
                {
                    delivery = 0;
                    deliveryorders = "0";
                }
            }
            catch (Exception ex)
            {


            }
            net = Math.Round(net, 2);
            if (logo == "")
            {
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, null, totlorder, avgsale);
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, null, totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv, cashgst, cardgst, cashdis, carddis, cashrecv, cardrecv, avgdinein, avgdineintable, posfee, complimtry,DlvCharges);

            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, sht, username, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv, cashgst, cardgst, cashdis, carddis, cashrecv, cardrecv, avgdinein, avgdineintable, posfee, complimtry,DlvCharges);
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidsale, Dlorders, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgsale);
            }

            return dat;
        }
        public string start = "", end = "", branchid = "", reference = "";
        private void RptUserSale_Load(object sender, EventArgs e)
        {

            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from ResturantStaff ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                dr["id"] = "0";
                ds.Tables[0].Rows.Add(dr);
                cmbservers.DataSource = ds.Tables[0];
                cmbservers.ValueMember = "id";
                cmbservers.DisplayMember = "name";
                cmbservers.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from users where Usertype='Cashier' ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                dr["id"] = "0";
                ds.Tables[0].Rows.Add(dr);
                cmbcashier.DataSource = ds.Tables[0];
                cmbcashier.ValueMember = "id";
                cmbcashier.DisplayMember = "name";
                cmbcashier.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
                dr1["id"] = "0";
                ds1.Tables[0].Rows.Add(dr1);
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
                DataSet ds = new DataSet();
                string q = "select id,name from shifts ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                dr["id"] = "0";
                ds.Tables[0].Rows.Add(dr);
                cmbshift.DataSource = ds.Tables[0];
                cmbshift.ValueMember = "id";
                cmbshift.DisplayMember = "name";

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
            if (reference == "pnl")
            {
                dateTimePicker1.Text = start;
                dateTimePicker2.Text = end;
                if (branchid == "All")
                {
                    cmbbranch.SelectedItem = branchid;
                }
                else
                {
                    cmbbranch.SelectedValue = branchid;
                }
                cmbshift.SelectedValue = "0";

                cmbservers.SelectedValue = "0";
                comboBox1.SelectedValue = "All Terminals";
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;
                bindreport();
            }
        }
        protected void fillterminal()
        {
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select distinct Terminal from sale where branchid='" + cmbbranch.SelectedValue + "'";
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
        public void updatebilltype()
        {
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_1 where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                dsbill = objcore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "insert into billtype (type,saleid,amount) values('" + type + "','" + id + "','" + amount + "')";
                    objcore.executeQuery(q);
                    q = "update sale set uploadstatusserver='Pending',uploadstatusbilltype='Pending' ,UploadStatus='Pending' where id='" + id + "'";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
            updatebilltype2();
        }
        public void updatebilltype2()
        {



            DataSet dsbill = new DataSet();
            try
            {

                string q = "select * from View_2 where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                dsbill = objcore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "update billtype set amount='" + amount + "' where saleid='" + id + "'";
                    objcore.executeQuery(q);
                    q = "update sale set uploadstatusserver='Pending',uploadstatusbilltype='Pending' ,UploadStatus='Pending'  where id='" + id + "'";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        protected double getdiscountinddetails(string sid)
        {
            double amount = 0;
            try
            {
                string q = "select sum(Discount) from DiscountIndividual where Saledetailsid='" + sid + "'";
                DataSet dsf = new DataSet();
                dsf = objCore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    amount = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return amount;
        }
        public string getordertype(string id)
        {
            string type = "";
            DataSet dstype = new DataSet();
            try
            {
                string q = "select OrderType from sale where id='" + id + "'";

                dstype = objCore.funGetDataSet(q);
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    type = dstype.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //dstype.Dispose();
            }
            return type;
        }
        public void updatebilltype3()
        {
            return;
            double servicecharhes = 0;
            DataSet dsgst = new DataSet();
            try
            {

                dsgst = objCore.funGetDataSet("select * from SerivceCharges");
                if (dsgst.Tables[0].Rows.Count > 0)
                {
                    servicecharhes = float.Parse(dsgst.Tables[0].Rows[0]["charges"].ToString());
                }
                else
                {
                    servicecharhes = 0;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsgst.Dispose();
            }
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_3 where (sum IS NULL) AND (netbill > 0)  or sum!=netbill AND (netbill > 0)";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    double price = 0, gst = 0, dis = 0, net = 0, disp = 0;
                    string id = dsbill.Tables[0].Rows[i]["saleid"].ToString();
                    DataSet dsdetail = new DataSet();
                    q = "select * from saledetails where saleid='" + id + "'";
                    dsdetail = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsdetail.Tables[0].Rows.Count; j++)
                    {
                        double discount = 0, gstt = 0, scarges = 0, price0 = 0;
                        try
                        {
                            string val = dsdetail.Tables[0].Rows[j]["price"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            price0 = Convert.ToDouble(val);
                            scarges = (price0 * servicecharhes) / 100;
                            scarges = Math.Round(scarges, 2);
                            val = dsdetail.Tables[0].Rows[j]["ItemdiscountPerc"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            string ordertyppe = "";
                            ordertyppe = getordertype(id);

                            if (ordertyppe == "Take Away")
                            {
                                scarges = 0;
                            }
                            double dis0 = Convert.ToDouble(val);
                            if (dis0 > 0 && price0 > 0)
                            {
                                discount = (price0 * dis0) / 100;
                                discount = Math.Round(discount, 2);
                            }
                            val = "";
                            val = dsdetail.Tables[0].Rows[j]["ItemGstPerc"].ToString(); ;
                            if (val == "")
                            {
                                val = "0";
                            }
                            gstt = Convert.ToDouble(val);
                            if (applydiscount() == "before")
                            {

                                if (gstt > 0 && price0 > 0)
                                {
                                    gstt = ((price0 + scarges) * gstt) / 100;
                                    gstt = Math.Round(gstt, 2);
                                }
                                else
                                {
                                    gstt = 0;
                                }
                            }
                            else
                            {
                                if (gstt > 0 && price0 > 0)
                                {
                                    gstt = ((price0 - discount) * gstt) / 100;
                                    gstt = Math.Round(gstt, 2);
                                }
                                else
                                {
                                    gstt = 0;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        double inddisdet = 0;
                        //try
                        //{
                        //    inddisdet = getdiscountinddetails(dsdetail.Tables[0].Rows[j]["id"].ToString());
                        //}
                        //catch (Exception)
                        //{

                        //}
                        //if (inddisdet > 0)
                        //{
                        //    q = "update saledetails set ItemGst='" + gstt + "',Itemdiscount='" + inddisdet + "' where id='" + dsdetail.Tables[0].Rows[j]["id"].ToString() + "'";
                        //}
                        //else
                        //{
                        //    q = "update saledetails set ItemGst='" + gstt + "' where id='" + dsdetail.Tables[0].Rows[j]["id"].ToString() + "'";

                        //}
                        objcore.executeQuery(q);
                        string temp = dsdetail.Tables[0].Rows[j]["price"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }

                        price = price + Convert.ToDouble(temp);

                        gst = gst + Convert.ToDouble(gstt);
                        temp = dsdetail.Tables[0].Rows[j]["Itemdiscount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }
                        if (inddisdet > 0)
                        {
                            dis = dis + inddisdet;
                        }
                        else
                        {
                            dis = dis + Convert.ToDouble(temp);
                        }
                        temp = dsdetail.Tables[0].Rows[j]["ItemdiscountPerc"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }
                        disp = Convert.ToDouble(temp);
                    }
                    double inddis = 0;
                    try
                    {
                        //inddis = getdiscountind(id);
                    }
                    catch (Exception)
                    {

                    }
                    double service = 0, serviceamount = 0;
                    if (type(id) == "Dine In")
                    {
                        dsdetail = new DataSet();
                        q = "select * from SerivceCharges ";
                        dsdetail = objCore.funGetDataSet(q);
                        if (dsdetail.Tables[0].Rows.Count > 0)
                        {
                            string temp = dsdetail.Tables[0].Rows[0]["charges"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            service = Convert.ToDouble(temp);
                        }
                        try
                        {
                            if (applydiscount() == "before")
                            {
                                serviceamount = ((price) * service) / 100;
                            }
                            else
                            {
                                serviceamount = ((price - dis) * service) / 100;
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        serviceamount = Math.Round(serviceamount, 2);
                    }
                    // MessageBox.Show(((price + gst + serviceamount) - (dis)).ToString()+"-"+id.ToString());
                    q = "update sale set TotalBill='" + price + "',DiscountAmount='" + dis + "',GST='" + gst + "',netbill='" + ((price + gst + serviceamount) - (dis)) + "',discount='" + disp + "',servicecharges='" + serviceamount + "' where id='" + id + "'";
                    objCore.executeQuery(q);
                    q = "select * from billtype where saleid='" + id + "'";
                    dsbill = new System.Data.DataSet();
                    dsbill = objcore.funGetDataSet(q);
                    if (dsbill.Tables[0].Rows.Count == 1)
                    {

                        q = "update billtype set amount='" + ((price + gst + serviceamount) - (dis)) + "' where saleid='" + id + "'";
                        objCore.executeQuery(q);
                        //MessageBox.Show(q);
                    }
                    if (dsbill.Tables[0].Rows.Count == 2)
                    {
                        double vsaamount = 0;
                        for (int l = 0; l < dsbill.Tables[0].Rows.Count; l++)
                        {
                            if (dsbill.Tables[0].Rows[l]["type"].ToString() == "Cash")
                            {

                            }
                            else
                            {
                                string temp = dsbill.Tables[0].Rows[l]["amount"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                vsaamount = Convert.ToDouble(temp);
                            }
                        }
                        q = "update billtype set amount='" + (((price + gst + serviceamount) - (dis + inddis)) - vsaamount) + "' where saleid='" + id + "' and type='cash'";
                        objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        public void updatebilltype31()
        {
            
            double servicecharhes = 0;
            DataSet dsgst = new DataSet();

            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_3 where (sum IS NULL) AND (netbill > 0)";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    double price = 0, gst = 0, dis = 0, net = 0, disp = 0;
                    string id = dsbill.Tables[0].Rows[i]["saleid"].ToString();
                    DataSet dsdetail = new DataSet();
                    q = "select * from saledetails where saleid='" + id + "'";
                    dsdetail = objCore.funGetDataSet(q);
                    if (dsdetail.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        q = "update sale set TotalBill='0',DiscountAmount='0',GST='0',netbill='0',discount='0',servicecharges='0',UploadStatus='Pending',uploadstatusbilltype='Pending',uploadstatusrefund='pending',uploadstatusserver='Pending' where id='" + id + "'";
                        objCore.executeQuery(q);
                        q = "update billtype set amount='0' where saleid='" + id + "'";
                        objCore.executeQuery(q);
                    }


                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
            dsbill = new DataSet();
            try
            {
                string q = "select * from View_3 where (sum!=netbill)";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    double price = 0, gst = 0, dis = 0, net = 0, disp = 0;
                    string id = dsbill.Tables[0].Rows[i]["saleid"].ToString();
                    DataSet dsdetail = new DataSet();
                    q = "select * from saledetails where saleid='" + id + "'";
                    dsdetail = objCore.funGetDataSet(q);
                    if (dsdetail.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < dsdetail.Tables[0].Rows.Count; j++)
                        {

                            string temp = dsdetail.Tables[0].Rows[j]["Itemdiscount"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            dis = dis + Convert.ToDouble(temp);
                            temp = dsdetail.Tables[0].Rows[j]["Itemgst"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            gst = gst + Convert.ToDouble(temp);
                            temp = dsdetail.Tables[0].Rows[j]["price"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            price = price + Convert.ToDouble(temp);
                        }
                        //q = "update sale set TotalBill='" + price + "',DiscountAmount='" + dis + "',GST='" + gst + "',netbill='" + ((price + gst) - (dis)) + "' where id='" + id + "'";
                        //objCore.executeQuery(q);
                        //q = "update sale set netbill=netbill+servicecharges+posfee where id='" + id + "'";
                        //objCore.executeQuery(q);
                    }
                    else
                    {
                        q = "update sale set TotalBill='0',DiscountAmount='0',GST='0',netbill='0',discount='0',servicecharges='0',UploadStatus='Pending',uploadstatusbilltype='Pending',uploadstatusrefund='pending',uploadstatusserver='Pending' where id='" + id + "'";
                        objCore.executeQuery(q);
                        q = "update billtype set amount='0' where saleid='" + id + "'";
                        objCore.executeQuery(q);
                    }


                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        protected double getdiscountind(string sid)
        {
            double amount = 0;
            try
            {
                string q = "select sum(Discount) from DiscountIndividual where saleid='" + sid + "'";
                DataSet dsf = new DataSet();
                dsf = objCore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    amount = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return amount;
        }
        protected string type(string id)
        {
            string typ = "";
            try
            {
                DataSet dsdetail = new DataSet();
                string q = "select OrderType from sale where id='" + id + "' ";
                dsdetail = objCore.funGetDataSet(q);
                if (dsdetail.Tables[0].Rows.Count > 0)
                {
                    string temp = dsdetail.Tables[0].Rows[0]["OrderType"].ToString();
                    if (temp == "")
                    {
                        temp = "0";

                    }
                    typ = temp;
                }
            }
            catch (Exception ex)
            {


            }

            return typ;
        }
        public string applydiscount()
        {
            string apply = "before";
            DataSet dsdis = new DataSet();
            try
            {
                string q = "select * from applydiscount ";

                dsdis = objCore.funGetDataSet(q);
                if (dsdis.Tables[0].Rows.Count > 0)
                {
                    apply = dsdis.Tables[0].Rows[0]["apply"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsdis.Dispose();
            }
            if (apply == "")
            {
                apply = "before";
            }
            return apply;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Enabled = false;
            vButton1.Text = "Please Wait";
            // off update bill and bill type 2024-07-08
            //updatebilltype31();
           // updatebilltype();
            visa = ""; visaamounts = "";
            bindreport();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }
      
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        ReportDocument cryRpt;
        private void vButton2_Click(object sender, EventArgs e)
        {

            try
            {

                string clientName = "", email = "", password = "", to = "", subject = "", body = "", cc1 = "", cc2 = "", cc3 = "", path = "";
                int port = 26;
                bool ssl = false;
                string qq = "select * from Mailsetting";
                DataSet dsmail = new DataSet();
                dsmail = objcore.funGetDataSet(qq);
                try
                {
                    if (dsmail.Tables[0].Rows.Count > 0)
                    {
                        path = dsmail.Tables[0].Rows[0]["path"].ToString();
                        if (path == "")
                        {
                            path = "C";
                        }
                        try
                        {
                            POSRestaurant.Reports.SaleReports.rptdaily rptDoc = new SaleReports.rptdaily();
                            POSRestaurant.Reports.SaleReports.DsUserDaily ds = new SaleReports.DsUserDaily();
                            //feereport ds = new feereport(); // .xsd file name
                            DataTable dt = new DataTable();

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
                                ds.Dt1.Merge(dt, true, MissingSchemaAction.Ignore);
                            }
                            else
                            {
                                if (logo == "")
                                { }
                                else
                                {
                                    dt.Rows.Add("", "", "", dscompany.Tables[0].Rows[0]["logo"]);
                                    ds.Dt1.Merge(dt, true, MissingSchemaAction.Ignore);
                                }
                            }
                            DataTable dtuser = new DataTable();
                            dtuser.TableName = "Crystal Report User";
                            dtuser = getAllOrdersuser();
                            // Just set the name of data table


                            ds.DataTable1.Merge(dtuser);

                            DataTable dtmenu = new DataTable();
                            dtmenu.TableName = "Crystal Report Menu";
                            dtmenu = getAllOrdersmenu();
                            // Just set the name of data table


                            ds.MenuGroup.Merge(dtmenu);

                            rptDoc.SetDataSource(ds);
                            rptDoc.SetParameterValue("Comp", company);
                            rptDoc.SetParameterValue("Addrs", address);
                            rptDoc.SetParameterValue("phn", phone);
                            rptDoc.SetParameterValue("visa", visa);
                            rptDoc.SetParameterValue("visaamounts", visaamounts);
                            rptDoc.SetParameterValue("report", "Sales Report");
                            delievrysourcetitle = "";
                            delievrysourcedata = "";
                            deliverysource();
                            rptDoc.SetParameterValue("deliverysourcetitle", delievrysourcetitle);


                            rptDoc.SetParameterValue("deliverysourcedata", delievrysourcedata);
                            if (takawayorders == "")
                            {
                                takawayorders = "0";
                            }
                            if (deliveryorders == "")
                            {
                                deliveryorders = "0";
                            }





                            rptDoc.SetParameterValue("takeawayorders", takawayorders);
                            rptDoc.SetParameterValue("dineinorders", dineinorders);
                            rptDoc.SetParameterValue("deliveryorders", deliveryorders);
                            rptDoc.SetParameterValue("recvname", recvname);
                            rptDoc.SetParameterValue("recvamount", recvamount);
                            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                            rptDoc.SetDataSource(dt);
                            
                            crystalReportViewer1.ReportSource = rptDoc;
                            cryRpt = rptDoc;

                            //ExportOptions CrExportOptions;
                            //DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                            //PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                            //CrDiskFileDestinationOptions.DiskFileName = path.ToString() + ":\\Sales report " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " .pdf";
                            //CrExportOptions = cryRpt.ExportOptions;
                            //{
                            //    CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                            //    CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                            //    CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                            //    CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            //}
                            //cryRpt.Export();

                            //ReportDocument report1 = new ReportDocument();
                            //report1.Load("YourReportFile.rpt"); // Replace with your report file path

                            // Create a MemoryStream to store the PDF content
                            MemoryStream pdfStream = new MemoryStream();

                            cryRpt.ExportToStream(ExportFormatType.PortableDocFormat).CopyTo(pdfStream);

                            

                            clientName = dsmail.Tables[0].Rows[0]["host"].ToString();
                            email = dsmail.Tables[0].Rows[0]["mailfrom"].ToString();
                            password = dsmail.Tables[0].Rows[0]["password"].ToString();
                            to = dsmail.Tables[0].Rows[0]["mailto"].ToString();
                            subject = dsmail.Tables[0].Rows[0]["head"].ToString();
                            body = dsmail.Tables[0].Rows[0]["mailto"].ToString();
                            port = Convert.ToInt32(dsmail.Tables[0].Rows[0]["port"].ToString());
                            //cc1 = dsmail.Tables[0].Rows[0]["cc1"].ToString();
                            //cc2 = dsmail.Tables[0].Rows[0]["cc2"].ToString();
                            //cc3 = dsmail.Tables[0].Rows[0]["cc3"].ToString();
                            try
                            {
                                ssl = Convert.ToBoolean(dsmail.Tables[0].Rows[0]["ssl"].ToString());
                            }
                            catch (Exception ex)
                            {

                            }
                            // Export the report directly to the MemoryStream
                          

                            // Send email with attachment
                            if (pdfStream.Length > 0)
                            { SendEmail(pdfStream, "SaleReport.pdf", email, to, password, port); }
                            else
                            {
                                MessageBox.Show("PDF is Blank");
                            }
                            
                        }
                        catch (Exception ex)
                        {
                            // MessageBox.Show(ex.ToString());
                        }
                        
                    }
                }
                catch (Exception ex)
                {


                }

                //SmtpClient client = new SmtpClient(clientName, port);
                //client.UseDefaultCredentials = false;
                //client.EnableSsl = ssl;
                //client.Credentials = new System.Net.NetworkCredential(email, password);
                //MailMessage myMail = new MailMessage();
                //MailAddress addTo = new MailAddress(to);
                //myMail.IsBodyHtml = true;
                //myMail.Subject = dsbranch.Tables[0].Rows[0]["BranchName"] + " " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " Sales Report";
                //myMail.Body = "Sale Report of " + dsbranch.Tables[0].Rows[0]["BranchName"] + " from " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " is attached";
                //Attachment at = new Attachment((path + ":\\Sales report " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " .pdf"));
                //myMail.Attachments.Add(at);
                //myMail.To.Add(addTo);
                //if (cc1.Length > 0)
                //{
                //    MailAddress copy = new MailAddress(cc1);
                //    myMail.CC.Add(copy);
                //}
                //if (cc2.Length > 0)
                //{
                //    MailAddress copy = new MailAddress(cc2);
                //    myMail.CC.Add(copy);
                //}
                //if (cc3.Length > 0)
                //{
                //    MailAddress copy = new MailAddress(cc3);
                //    myMail.CC.Add(copy);
                //}

                //myMail.Priority = MailPriority.High;
                //myMail.From = new MailAddress(email);
               
              
               
                //client.Send(myMail);


               

            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.InnerException.ToString());
            }
        }


        private void SendEmail(MemoryStream attachmentStream, string attachmentFileName, string FromEmail, string ToEmail, string Password, int Port)
        {

            try
        {
        // Create an email message
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress(FromEmail); // Replace with sender email
        ToEmail = "syedpk786@gmail.com";
        mail.To.Add(ToEmail); // Replace with recipient email
        mail.Subject = "Sale Report PDF";
        mail.Body = "Please find attached the Sale Report in PDF.";

        // Attach the PDF content from the MemoryStream
        Attachment attachment = new Attachment(attachmentStream, attachmentFileName);
        mail.Attachments.Add(attachment);

        // Configure SMTP settings
       // SmtpClient smtpServer = new SmtpClient("mail.ftech.com.pk");
        SmtpClient smtpServer = new SmtpClient();
        smtpServer.Host = "smtp.gmail.com";
       // smtpServer.Host = "mail.ftech.com.pk";

        //smtpServer.Port = Port;
        System.Net.NetworkCredential ntcd = new NetworkCredential();
        ntcd.UserName = "greencables15@gmail.com";
        ntcd.Password = "unav heev ybyy tuws";
        smtpServer.Credentials = ntcd;
        smtpServer.EnableSsl = true;
        smtpServer.Port = 587;
        smtpServer.Send(mail);
        //smtpServer.Credentials = new NetworkCredential(FromEmail, Password);
                
        //smtpServer.Credentials = new NetworkCredential(FromEmail, Password);
     
        //smtpServer.EnableSsl = false;
        //smtpServer.Timeout = 120000;
        //smtpServer.UseDefaultCredentials = false;
        //smtpServer.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
      
        //smtpServer.Send(mail);

        // Close the MemoryStream
        attachmentStream.Close();
        }
        catch (Exception ex)
        {
       // MessageBox.Show($"Error sending email: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        }
       
        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillterminal();
        }
    }
}
