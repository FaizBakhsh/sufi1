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
namespace POSRestaurant.Reports.SaleReports
{
    public partial class frmSalesDS : Form
    {
        public string date = "", userid = "", cashiername = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public frmSalesDS()
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
            rptDoc.SetParameterValue("report", "DSSales Report");
            deliverysource();
            rptDoc.SetParameterValue("deliverysourcetitle", delievrysourcetitle);
            rptDoc.SetParameterValue("deliverysourcedata", delievrysourcedata);
            rptDoc.SetParameterValue("takeawayorders", takawayorders);
            rptDoc.SetParameterValue("dineinorders", dineinorders);
            rptDoc.SetParameterValue("deliveryorders", deliveryorders);
            rptDoc.SetParameterValue("recvname", "");
            rptDoc.SetParameterValue("recvamount", "");
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
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'  GROUP BY dbo.Delivery.type ";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.DSSale.terminal='" + comboBox1.Text + "'  GROUP BY dbo.Delivery.type ";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' GROUP BY dbo.Delivery.type ";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.DSSale.terminal='" + comboBox1.Text + "'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  GROUP BY dbo.Delivery.type ";

                        }
                    }
                }
                else
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.DSSale.terminal='" + comboBox1.Text + "'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.DSSale.NetBill) AS amount, dbo.Delivery.type FROM            dbo.DSSale INNER JOIN                          dbo.Delivery ON dbo.DSSale.Id = dbo.Delivery.saleid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and dbo.DSSale.terminal='" + comboBox1.Text + "'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  GROUP BY dbo.Delivery.type ORDER BY dbo.Delivery.type";

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
                            delievrysourcetitle = delievrysourcetitle + " ,";
                        }
                        delievrysourcetitle = delievrysourcetitle + dssource.Tables[0].Rows[i]["type"].ToString();

                        if (delievrysourcedata.Length > 0)
                        {
                            delievrysourcedata = delievrysourcedata + " ,";
                        }
                        string temp = dssource.Tables[0].Rows[i]["amount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        delievrysourcedata = delievrysourcedata + Math.Round(Convert.ToDouble(temp), 2).ToString();

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

                if (cmbservers.Text == "All")
                {
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (DSSale.branchid='" + cmbbranch.SelectedValue + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.branchid='" + cmbbranch.SelectedValue + "')  and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.id  FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (DSSale.branchid='" + cmbbranch.SelectedValue + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name , dbo.MenuGroup.id FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.branchid='" + cmbbranch.SelectedValue + "')  and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

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

                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='"+cmbservers.SelectedValue+"' and   (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (DSSale.branchid='" + cmbbranch.SelectedValue + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.branchid='" + cmbbranch.SelectedValue + "')  and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and    (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (DSSale.branchid='" + cmbbranch.SelectedValue + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";
                            }
                            else
                            {
                                q = "SELECT        SUM(dbo.DSSaledetails.Price - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name, dbo.MenuGroup.Id FROM            dbo.DSSaledetails INNER JOIN                         dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid WHERE  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.branchid='" + cmbbranch.SelectedValue + "')  and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') and DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.MenuGroup.Name, dbo.MenuGroup.id ";

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

        protected double getvalue(string id,string type)
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

                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.BillStatus = 'Paid') and menugroup.id='" + id + "' and DSSale.GSTtype like '" + type + "%' GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') and menugroup.id='" + id + "' and DSSale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (DSSale.branchid='" + cmbbranch.SelectedValue + "') and (DSSale.BillStatus = 'Paid') and menugroup.id='" + id + "' and DSSale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.branchid='" + cmbbranch.SelectedValue + "')  and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') and menugroup.id='" + id + "'  and DSSale.GSTtype like '" + type + "%'  GROUP BY dbo.MenuGroup.Name";

                        }
                    }
                }
                else
                {
                    if (cmbbranch.Text == "All")
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.BillStatus = 'Paid')  and (DSSale.shiftid = '"+cmbshift.SelectedValue+"') and menugroup.id='" + id + "' and DSSale.GSTtype like '" + type + "%' GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.terminal='" + comboBox1.Text + "')   and (DSSale.shiftid = '" + cmbshift.SelectedValue + "') and (DSSale.BillStatus = 'Paid') and menugroup.id='" + id + "' and DSSale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";

                        }
                    }
                    else
                    {
                        if (comboBox1.Text == "All Terminals")
                        {

                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and (DSSale.branchid='" + cmbbranch.SelectedValue + "')  and (DSSale.shiftid = '" + cmbshift.SelectedValue + "')  and (DSSale.BillStatus = 'Paid') and menugroup.id='" + id + "' and DSSale.GSTtype like '" + type + "%'   GROUP BY dbo.MenuGroup.Name";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSaledetails.Price  - dbo.DSSaledetails.Itemdiscount) AS sum, COUNT(dbo.DSSaledetails.Price) AS count, dbo.MenuGroup.Name FROM         dbo.DSSaledetails INNER JOIN                      dbo.MenuItem ON dbo.DSSaledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.DSSale ON dbo.DSSaledetails.saleid = dbo.DSSale.Id WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and (DSSale.branchid='" + cmbbranch.SelectedValue + "')   and (DSSale.shiftid = '" + cmbshift.SelectedValue + "')  and (DSSale.terminal='" + comboBox1.Text + "') and (DSSale.BillStatus = 'Paid') and menugroup.id='" + id + "'  and DSSale.GSTtype like '" + type + "%'  GROUP BY dbo.MenuGroup.Name";

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


                q = "SELECT     SUM(dbo.DSSale.NetBill) AS sum, COUNT(dbo.DSSale.NetBill) AS count,SUM(dbo.DSSale.DiscountAmount) AS dis, dbo.Users.Name,dbo.Users.id FROM         dbo.DSSale INNER JOIN                      dbo.Users ON dbo.DSSale.UserId = dbo.Users.Id   WHERE     (DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and DSSale.BillStatus='Paid'  GROUP BY dbo.Users.Name,dbo.Users.id ";
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double refnd = 0, disc = 0;
                    DataSet dsref = new DataSet();
                    //q = "SELECT     sum( NetBill) FROM         DSSale where UserId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and BillStatus='Refund'";
                    //q = "SELECT     SUM(NetBill) AS cash, count(id) as count  FROM         DSSale where  (Date = '" + dateTimePicker2.Text + "')   and dbo.DSSale.UserId='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and BillStatus='Refund'";
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
            dat.Columns.Add("averageSale", typeof(double));
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
            getcompany();
            string logo = "";
            try
            {
                logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

            }
            catch (Exception ex)
            {


            }
            double gross = 0, cashgst = 0, cardgst = 0, gst = 0, cashdis = 0, carddis = 0, discount = 0, net = 0, service = 0,cashrecv=0,cardrecv=0, recv = 0, cash = 0, credit = 0, master = 0, dinin = 0, takeaway = 0, delivery = 0, refund = 0, voidSale = 0, carhope = 0, calculatedcash = 0, drawerfloat = 0, bankingtotal = 0, declared = 0, over = 0, total = 0;
            string Dlorders = "0", Torders = "0", Dorders = "0", RefundNo = "0", carhopeorders = "0";

            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {

                string temp = "";
                string q = "";
                try
                {
                    ds = new DataSet();

                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";

                                }
                                else
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid'";

                                }
                                else
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                                else
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                                else
                                {
                                    q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

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
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid'";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid'";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "'  and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                                else
                                {
                                    q = "SELECT        SUM(dbo.DSSale.TotalBill) AS gross, SUM(dbo.DSSale.GST) AS gst, SUM(dbo.DSSale.TotalBill) AS netSale, SUM(dbo.DSSale.servicecharges) AS serv, SUM(dbo.DSSale.DiscountAmount) AS discount FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                    

                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        
                        discount = discount + Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                        gross = Convert.ToDouble(ds.Tables[0].Rows[0]["gross"].ToString());
                        gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                       
                        net = Convert.ToDouble(ds.Tables[0].Rows[0]["netSale"].ToString());
                        net = net - discount;
                        gross = gross + gst;
                        net = Math.Round(net, 2);
                        service = Convert.ToDouble(ds.Tables[0].Rows[0]["serv"].ToString());
                        gross = gross + service;

                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

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
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Cash' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Cash' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

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
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

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
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid'";

                                    }
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'  and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                }
                                else
                                {
                                    if (comboBox1.Text == "All Terminals")
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and billstatus='Paid' and GSTtype='Card' and shiftid='" + cmbshift.SelectedValue + "'";

                                    }
                                    else
                                    {
                                        q = "SELECT     SUM(TotalBill) AS gross, SUM(GST) AS gst, SUM(TotalBill) AS netSale,sum(servicecharges) as serv, SUM(DiscountAmount) AS discount FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and terminal='" + comboBox1.Text + "'  and GSTtype='Card' and billstatus='Paid' and shiftid='" + cmbshift.SelectedValue + "'";

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

                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

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
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.BillStatus='Paid'";
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.BillStatus='Paid'";
                            
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";
                            
                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                    
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                        }
                    }
                    
                    //q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.BillType='Cash' and dbo.DSSale.BillStatus='Paid'";
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
                    q = "";// "SELECT     SUM(NetBill) AS cash  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillType='Cash'";
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash' and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type='Cash' and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type='Cash' and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSBillType.type='Cash'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type='Cash' and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

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

                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.BillType='Master Card' and dbo.DSSale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.BillType='Master Card'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.BillType='Master Card' and dbo.DSSale.BillStatus='Paid'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.BillType='Master Card'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.BillType='Master Card' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='"+cmbshift.SelectedValue+"'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.BillType='Master Card'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {


                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.BillType='Master Card' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.BillType='Master Card'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

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
                    // q = "SELECT     SUM(NetBill) AS cash  FROM         DSSale where  (Date = '" + dateTimePicker2.Text + "') and BillType='Credit Card'";

                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";

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

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='"+cmbservers.SelectedValue+"' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' GROUP BY dbo.DSBillType.type";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash, dbo.DSBillType.type  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSBillType.type like '%Visa%'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' GROUP BY dbo.DSBillType.type";

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

                    if (cmbservers.Text == "All")
                    {
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

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

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='"+cmbservers.SelectedValue+"' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' ";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

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
                        recv = Convert.ToDouble(temp);
                    }
                    else
                    {
                        recv = 0;
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
                        if (cmbshift.Text == "All")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Cash'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Cash'";

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

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='"+cmbservers.SelectedValue+"' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Cash'";

                                }
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Cash'";

                                }
                            }
                            else
                            {
                                if (comboBox1.Text == "All Terminals")
                                {

                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Cash'";
                                }
                                else
                                {
                                    q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id  INNER JOIN dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Cash'";

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

                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Card'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Card'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Card'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.GSTtype='Card'";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.DSSale.GSTtype='Card'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Card'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {

                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Card'";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.DSBillType.Amount) AS cash  FROM         dbo.DSBillType INNER JOIN dbo.DSSale ON dbo.DSBillType.saleid = dbo.DSSale.Id where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.DSBillType.type ='Receivable'  and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' and dbo.DSSale.GSTtype='Card'";

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
                        declared = 0;// Convert.ToDouble(temp);
                        calculatedcash = 0;// calculatedcash + drawerfloat;
                        total = 0;// calculatedcash - drawerfloat;
                        over = 0;// declared - calculatedcash;
                        bankingtotal = 0;// declared - drawerfloat;
                    }

                }
                catch (Exception ex)
                {


                }
                try
                {
                    ds = new DataSet();
                    if (comboBox1.Text == "All Terminals")
                    {

                        q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' ";
                    }
                    else
                    {
                        q = "SELECT     SUM(TotalBill + GST - DiscountAmount) AS cash, count(id) as count  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and BillStatus='Refund' and terminal='" + comboBox1.Text + "'";

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

                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.terminal='" + comboBox1.Text + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' ";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.terminal='" + comboBox1.Text + "'";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Refund' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";

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


                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.terminal='" + comboBox1.Text + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' ";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.terminal='" + comboBox1.Text + "'";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' ";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' ";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' ";
                            }
                            else
                            {
                                q = "SELECT SUM(dbo.DSSaledetailsrefund.Price + dbo.DSSaledetailsrefund.ItemGst - dbo.DSSaledetailsrefund.ItemdiscountPerc) AS Expr1 FROM  dbo.DSSaledetailsrefund INNER JOIN               dbo.DSSale ON dbo.DSSaledetailsrefund.saleid = dbo.DSSale.Id where  dbo.DSSaledetailsrefund.type='Void' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.terminal='" + comboBox1.Text + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "' ";

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
                        voidSale = Convert.ToDouble(temp);
                        

                    }
                    else
                    {
                        voidSale = 0;
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

                if (cmbservers.Text == "All")
                {
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                            }
                            else
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and shiftid='" + cmbshift.SelectedValue + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     count(id) AS cash  FROM         DSSale where  (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";

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
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')";
                            }
                            else
                            {
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and branchid='" + cmbbranch.SelectedValue + "'";

                            }
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and   (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and shiftid='" + cmbshift.SelectedValue + "'";

                            }
                        }
                        else
                        {
                            if (comboBox1.Text == "All Terminals")
                            {
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";
                            }
                            else
                            {
                                q = "SELECT     count(DSSale.id) AS cash  FROM         DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and terminal='" + comboBox1.Text + "' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and shiftid='" + cmbshift.SelectedValue + "'";

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
            double avgSale = 0;
            try
            {
                if (gross == 0)
                {
                }
                else
                {
                    avgSale = (net) / totlorder;
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

                if (cmbservers.Text == "All")
                {
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                        }
                    }
                }
                else
                {
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale INNER JOIN  dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and  (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Take Away' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
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

                if (cmbservers.Text == "All")
                {
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                        }
                    }
                }
                else
                {
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT        SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash, COUNT(dbo.DSSale.Id) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash, COUNT(dbo.DSSale.Id) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT        SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash, COUNT(dbo.DSSale.Id) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash, COUNT(dbo.DSSale.Id) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
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
                        if (cmbservers.Text == "All")
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid  where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid  where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid  where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid  where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' ";
                                }
                            }
                        }
                        else
                        {
                            if (cmbshift.Text == "All")
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid   where dbo.DinInTables.WaiterId='"+cmbservers.SelectedValue+"' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'";
                                }
                                else
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid  where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                                }
                            }
                            else
                            {
                                if (cmbbranch.Text == "All")
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid  where dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                                }
                                else
                                {
                                    q = "SELECT        SUM(CAST(COALESCE(guests, 0) AS int)) AS count FROM            dbo.DSSale INNER JOIN                         dbo.DinInTables ON dbo.DSSale.Id = dbo.DinInTables.Dsid  where  dbo.DinInTables.WaiterId='" + cmbservers.SelectedValue + "' and (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Dine In' and dbo.DSSale.BillStatus='Paid'  and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' ";
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
                if (cmbservers.Text=="All")
                {
                    if (cmbshift.Text == "All")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Delivery' and dbo.DSSale.BillStatus='Paid'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Delivery' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "'";
                        }

                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash ,count(dbo.DSSale.id) as count FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Delivery' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.DSSale.TotalBill + dbo.DSSale.GST - dbo.DSSale.DiscountAmount) AS cash,count(dbo.DSSale.id) as count  FROM  DSSale where (dbo.DSSale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.DSSale.OrderType='Delivery' and dbo.DSSale.BillStatus='Paid' and dbo.DSSale.branchid='" + cmbbranch.SelectedValue + "' and dbo.DSSale.shiftid='" + cmbshift.SelectedValue + "'";
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
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidDSSale, Dlorders, Torders, Dorders, RefundNo, null, totlorder, avgDSSale);
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidSale, sht, username, Dorders, RefundNo, null, totlorder, avgSale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv, cashgst, cardgst, cashdis, carddis, cashrecv, cardrecv, avgdinein, avgdineintable);

            }
            else
            {
                dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidSale, sht, username, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgSale, carhope, carhopeorders, calculatedcash, drawerfloat, bankingtotal, declared, over, total, service, recv, cashgst, cardgst, cashdis, carddis, cashrecv, cardrecv, avgdinein, avgdineintable);
                //dat.Rows.Add(gross, gst, discount, net, cash, credit, master, dinin, takeaway, delivery, refund, voidDSSale, Dlorders, Torders, Dorders, RefundNo, dscompany.Tables[0].Rows[0]["logo"], totlorder, avgDSSale);
            }

            return dat;
        }
        private void RptUserSale_Load(object sender, EventArgs e)
        {
            try
            {
               DataSet ds = new DataSet();
                string q = "select id,name from ResturantStaff ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
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
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
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
                ds.Tables[0].Rows.Add(dr);
                cmbshift.DataSource = ds.Tables[0];
                cmbshift.ValueMember = "id";
                cmbshift.DisplayMember = "name";

            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
        }
        protected void fillterminal()
        {
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select distinct Terminal from DSSale where branchid='"+cmbbranch.SelectedValue+"'";
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
                    q = "update DSSale set uploadstatusserver='Pending',uploadstatusbilltype='Pending' ,UploadStatus='Pending' where id='" + id + "'";
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
                    q = "update DSSale set uploadstatusserver='Pending',uploadstatusbilltype='Pending' ,UploadStatus='Pending'  where id='" + id + "'";
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
                string q = "select sum(Discount) from DiscountIndividual where DSSaledetailsid='" + sid + "'";
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
                string q = "select OrderType from DSSale where id='" + id + "'";

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
                string q = "select OrderType from DSSale where id='" + id + "' ";
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
            //updatebilltype3();
            //updatebilltype31();
            updatebilltype();
            visa = ""; visaamounts = "";
            bindreport();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }
        //public void updatebilltype4()
        //{
        //    DataSet dsbill = new DataSet();
        //    try
        //    {
        //        string q = "SELECT        id, charges  FROM            SerivceCharges";
        //        dsbill = objCore.funGetDataSet(q);
        //        if (dsbill.Tables[0].Rows.Count > 0)
        //        {
        //            string temp = dsbill.Tables[0].Rows[0]["charges"].ToString();
        //            if (temp == "")
        //            {
        //                temp = "0";
        //            }
        //            float charges = float.Parse(temp);
        //            if (charges > 0)
        //            {
        //                q = "select * from DSSale where  billtype='Dine In'  and date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "' order by id desc";
        //                dsbill = new DataSet();
        //                dsbill = objCore.funGetDataSet(q);
        //                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
        //                {
        //                    temp = "0";
        //                    string otype = dsbill.Tables[0].Rows[i]["OrderType"].ToString();
        //                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
        //                    double servicecharges = 0, totalbill = 0, gst = 0, totalgst = 0, dscount = 0, discountedtotal = 0, service = 0, nettotal = 0;
        //                    temp = dsbill.Tables[0].Rows[i]["totalbill"].ToString();
        //                    if (temp == "")
        //                    {
        //                        temp = "0";
        //                    }
        //                    totalbill = Convert.ToDouble(temp);
        //                    temp = dsbill.Tables[0].Rows[i]["gstperc"].ToString();
        //                    if (temp == "")
        //                    {
        //                        temp = "0";
        //                    }
        //                    gst = Convert.ToDouble(temp);

        //                    temp = dsbill.Tables[0].Rows[i]["discount"].ToString();
        //                    if (temp == "")
        //                    {
        //                        temp = "0";
        //                    }
        //                    dscount = Convert.ToDouble(temp);
        //                    dscount = (dscount * totalbill) / 100;
        //                    dscount = Math.Round(dscount, 2);

        //                    // if (otype == "Dine In")
        //                    {



        //                        if (applydiscount() == "before")
        //                        {
        //                            discountedtotal = totalbill;// -dscount;
        //                            service = (charges * discountedtotal) / 100;
        //                            service = Math.Round(service, 2);
        //                            if (otype == "Take Away")
        //                            {
        //                                service = 0;
        //                            }
        //                            discountedtotal = discountedtotal + service;
        //                            totalgst = (gst * discountedtotal) / 100;
        //                            totalgst = Math.Round(totalgst, 2);
        //                            discountedtotal = discountedtotal - dscount;
        //                        }
        //                        else
        //                        {
        //                            discountedtotal = totalbill - dscount;
        //                            service = (charges * discountedtotal) / 100;
        //                            service = Math.Round(service, 2);
        //                            if (otype == "Take Away")
        //                            {
        //                                service = 0;
        //                            }
        //                            totalgst = (gst * discountedtotal) / 100;
        //                            totalgst = Math.Round(totalgst, 2);
        //                            discountedtotal = discountedtotal + service;
        //                        }
        //                        discountedtotal = Math.Round(discountedtotal, 2);

        //                        nettotal = Math.Round((totalgst + discountedtotal), 2);
        //                        q = "update DSSale set discountamount='" + dscount + "',servicecharges='" + service + "',NetBill='" + nettotal + "',gst='" + totalgst + "' where id='" + id + "'";
        //                        objCore.executeQuery(q);
        //                        q = "update billtype set amount='" + nettotal + "' where saleid='" + id + "'";
        //                        objCore.executeQuery(q);
        //                    }

        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //    finally
        //    {
        //        dsbill.Dispose();
        //    }
        //}
        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        ReportDocument cryRpt;
        private void vButton2_Click(object sender, EventArgs e)
        {
            return;
            try
            {

                string clientName = "", email = "", password = "", to = "", subject = "", body = "", cc1 = "", cc2 = "", cc3 = "", path = "";
                int port = 26;
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
                            ExportOptions CrExportOptions;
                            DiskFileDestinationOptions CrDiskFileDestinationOptions = new DiskFileDestinationOptions();
                            PdfRtfWordFormatOptions CrFormatTypeOptions = new PdfRtfWordFormatOptions();
                            CrDiskFileDestinationOptions.DiskFileName = path.ToString() + ":\\Sales report " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " .pdf";
                            CrExportOptions = cryRpt.ExportOptions;
                            {
                                CrExportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                                CrExportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                                CrExportOptions.DestinationOptions = CrDiskFileDestinationOptions;
                                CrExportOptions.FormatOptions = CrFormatTypeOptions;
                            }
                            cryRpt.Export();
                        }
                        catch (Exception ex)
                        {
                            // MessageBox.Show(ex.ToString());
                        }
                        clientName = dsmail.Tables[0].Rows[0]["host"].ToString();
                        email = dsmail.Tables[0].Rows[0]["mailfrom"].ToString();
                        password = dsmail.Tables[0].Rows[0]["password"].ToString();
                        to = dsmail.Tables[0].Rows[0]["mailto"].ToString();
                        subject = dsmail.Tables[0].Rows[0]["head"].ToString();
                        body = dsmail.Tables[0].Rows[0]["mailto"].ToString();
                        port = Convert.ToInt32(dsmail.Tables[0].Rows[0]["port"].ToString());
                        cc1 = dsmail.Tables[0].Rows[0]["cc1"].ToString();
                        cc2 = dsmail.Tables[0].Rows[0]["cc2"].ToString();
                        cc3 = dsmail.Tables[0].Rows[0]["cc3"].ToString();
                    }
                }
                catch (Exception ex)
                {


                }

                SmtpClient client = new SmtpClient(clientName, port);
                client.UseDefaultCredentials = false;
                client.EnableSsl = false;
                client.Credentials = new System.Net.NetworkCredential(email, password);
                MailMessage myMail = new MailMessage();
                MailAddress addTo = new MailAddress(to);
                myMail.IsBodyHtml = true;
                myMail.Subject = dsbranch.Tables[0].Rows[0]["BranchName"] + " " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " DSSales Report";
                myMail.Body = "DSSale Report of " + dsbranch.Tables[0].Rows[0]["BranchName"] + " from " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " is attached";
                Attachment at = new Attachment((path + ":\\DSSales report " + dateTimePicker1.Text + " to " + dateTimePicker2.Text + " .pdf"));
                myMail.Attachments.Add(at);
                myMail.To.Add(addTo);
                if (cc1.Length > 0)
                {
                    MailAddress copy = new MailAddress(cc1);
                    myMail.CC.Add(copy);
                }
                if (cc2.Length > 0)
                {
                    MailAddress copy = new MailAddress(cc2);
                    myMail.CC.Add(copy);
                }
                if (cc3.Length > 0)
                {
                    MailAddress copy = new MailAddress(cc3);
                    myMail.CC.Add(copy);
                }

                myMail.Priority = MailPriority.High;
                myMail.From = new MailAddress(email);
                client.Send(myMail);

            }
            catch (Exception exx)
            {
                MessageBox.Show(exx.InnerException.ToString());
            }
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillterminal();
        }
    }
}
