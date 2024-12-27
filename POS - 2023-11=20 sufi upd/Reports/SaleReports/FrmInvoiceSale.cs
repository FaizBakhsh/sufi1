using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmInvoiceSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmInvoiceSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            cmbordertype.SelectedItem = "All";
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
                DataSet ds1 = new DataSet();
                string q = "select DISTINCT Floor from DineInTableDesign ORDER BY Floor ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["Floor"] = "All";
                ds1.Tables[0].Rows.Add(dr1);
                cmbfloors.DataSource = ds1.Tables[0];
                cmbfloors.ValueMember = "Floor";
                cmbfloors.DisplayMember = "Floor";
                cmbfloors.Text = "All";
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
                cmbshift.DataSource = ds.Tables[0];
                cmbshift.ValueMember = "id";
                cmbshift.DisplayMember = "name";
                cmbshift.Text = "All";
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


                POSRestaurant.Reports.SaleReports.rptbilltype rptDoc = new rptbilltype();
                POSRestaurant.Reports.SaleReports.dsbilltype dsrpt = new dsbilltype();
                
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
                
                rptDoc.SetParameterValue("date", "for the period  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
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
                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("BillNo", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("BillType", typeof(string));
                dtrpt.Columns.Add("Customer", typeof(string));
                dtrpt.Columns.Add("Orderterminal", typeof(string));
                dtrpt.Columns.Add("Terminal", typeof(string));
                dtrpt.Columns.Add("Shift", typeof(string));
                dtrpt.Columns.Add("Onlineid", typeof(string));
                dtrpt.Columns.Add("Discount", typeof(double));
                dtrpt.Columns.Add("GST", typeof(double));
                dtrpt.Columns.Add("Total", typeof(double));
                dtrpt.Columns.Add("fbrcode", typeof(string));
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
                    if (cmbordertype.Text == "All")
                    {
                        if (checkBox1.Checked == true)
                        {
                            if (cmbtype.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and LEN(FBRcode) > 0  order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0  order by dbo.sale.id";
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and LEN(FBRcode) > 0  order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0   and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  order by dbo.sale.id";
                                }
                            }
                        }
                        else
                        {
                            if (cmbtype.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' order by dbo.sale.id";
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode, dbo.Sale.time,dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode, dbo.Sale.time,dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  order by dbo.sale.id";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbfloors.Visible == false)
                        {
                            if (checkBox1.Checked == true)
                            {
                                if (cmbtype.Text == "All")
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                                else
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'   order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0   and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                            }
                            else
                            {
                                if (cmbtype.Text == "All")
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.OrderType='" + cmbordertype.Text + "'   order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                                else
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                if (cmbtype.Text == "All")
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                    }
                                }
                                else
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'   order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0   and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                    }
                                }
                            }
                            else
                            {
                                if (cmbfloors.Text == "All")
                                {
                                    if (cmbtype.Text == "All")
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.OrderType='" + cmbordertype.Text + "'   order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                    else
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbtype.Text == "All")
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.OrderType='" + cmbordertype.Text + "' AND (dbo.DineInTableDesign.Floor = '" + cmbfloors.Text + "')  order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                    else
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where  dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'  AND (DATEPART(hh,  dbo.Sale.time) BETWEEN '" + cmbtimefrom.Text + "' AND '" + cmbtimeto.Text + "') and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (cmbordertype.Text == "All")
                    {
                        if (checkBox1.Checked == true)
                        {
                            if (cmbtype.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and LEN(FBRcode) > 0  order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0  order by dbo.sale.id";
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and LEN(FBRcode) > 0  order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0   and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  order by dbo.sale.id";
                                }
                            }
                        }
                        else
                        {
                            if (cmbtype.Text == "All")
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'   order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' order by dbo.sale.id";
                                }
                            }
                            else
                            {
                                if (cmbshift.Text == "All")
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' order by dbo.sale.id";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  order by dbo.sale.id";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (cmbfloors.Visible == false)
                        {
                            if (checkBox1.Checked == true)
                            {
                                if (cmbtype.Text == "All")
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                                else
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'   order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0   and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                            }
                            else
                            {
                                if (cmbtype.Text == "All")
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id,dbo.Sale.onlineid, dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.OrderType='" + cmbordertype.Text + "'   order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                                else
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id,dbo.Sale.onlineid,  dbo.Sale.Date,dbo.Sale.TotalBill,dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount, dbo.Sale.DiscountAmount, dbo.BillType.cashoutime FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (checkBox1.Checked == true)
                            {
                                if (cmbtype.Text == "All")
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "' and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                    }
                                }
                                else
                                {
                                    if (cmbshift.Text == "All")
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and LEN(FBRcode) > 0  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'   order by dbo.sale.id";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and LEN(FBRcode) > 0   and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                    }
                                }
                            }
                            else
                            {
                                if (cmbfloors.Text == "All")
                                {
                                    if (cmbtype.Text == "All")
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.OrderType='" + cmbordertype.Text + "'   order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                    else
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                }
                                else
                                {
                                    if (cmbtype.Text == "All")
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.OrderType='" + cmbordertype.Text + "' AND (dbo.DineInTableDesign.Floor = '" + cmbfloors.Text + "')  order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "' and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                    else
                                    {
                                        if (cmbshift.Text == "All")
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time,dbo.Sale.OrderType,  dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo   where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%' and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                        }
                                        else
                                        {
                                            q = "SELECT        dbo.Sale.FBRcode,dbo.Sale.time, dbo.Sale.OrderType, dbo.Sale.Id, dbo.Sale.OnlineId, dbo.Sale.Date, dbo.Sale.TotalBill, dbo.Sale.GST, dbo.Sale.Customer, dbo.Sale.TerminalOrder, dbo.Sale.Terminal, dbo.Shifts.Name, dbo.BillType.type, dbo.BillType.Amount,                          dbo.Sale.DiscountAmount, dbo.BillType.cashoutime, dbo.DineInTableDesign.Floor FROM            dbo.Sale INNER JOIN                         dbo.Shifts ON dbo.Sale.Shiftid = dbo.Shifts.Id INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid INNER JOIN                         dbo.DineInTableDesign ON dbo.Sale.Customer = dbo.DineInTableDesign.TableNo  where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.sale.branchid='" + cmbbranch.SelectedValue + "'  and dbo.sale.shiftid='" + cmbshift.SelectedValue + "'  and dbo.Sale.BillType like '%" + cmbtype.Text + "%'  and dbo.sale.OrderType='" + cmbordertype.Text + "'  and dbo.DineInTableDesign.Floor='" + cmbfloors.Text + "'  order by dbo.sale.id";
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double bill = 0, total = 0, gst = 0, dis = 0;
                    string val = "";
                    val = ds.Tables[0].Rows[i]["Amount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    total = Convert.ToDouble(val);

                    val = ds.Tables[0].Rows[i]["DiscountAmount"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    dis = Convert.ToDouble(val);
                    val = ds.Tables[0].Rows[i]["gst"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    gst = Convert.ToDouble(val);
                    string customer=ds.Tables[0].Rows[i]["Customer"].ToString();
                    string tempcus = gettbleno(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                    if(tempcus.Length>0)
                    {
                        customer=tempcus;
                    }
                    string time = "";
                    try
                    {
                        time = ds.Tables[0].Rows[i]["cashoutime"].ToString();
                        if (chktime.Checked == true)
                        {
                            time = ds.Tables[0].Rows[i]["time"].ToString();
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }

                    try
                    {
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["date"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), total, ds.Tables[0].Rows[i]["type"].ToString(), customer, time, ds.Tables[0].Rows[i]["Terminal"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), ds.Tables[0].Rows[i]["Onlineid"].ToString(), dis, gst, ds.Tables[0].Rows[i]["TotalBill"].ToString(), ds.Tables[0].Rows[i]["FBRcode"].ToString());
                    }
                    catch (Exception wx)
                    {
                        
                       
                    }
                }                                                                                             
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return dtrpt;
        }
        public string gettbleno(string id,string ordertype)
        {
            string tbl = "";
            //DataSet dstbl = new DataSet();
            try
            {
                if (ordertype == "Dine In")
                {
                    string q = "select TableNo,guests,ResId from DinInTables where Saleid='" + id + "'";
                    //q = "SELECT dbo.DinInTables.TableNo, dbo.ResturantStaff.Name FROM  dbo.DinInTables INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where dbo.DinInTables.Saleid='" + id + "'";
                    // dstbl = objCore.funGetDataSet(q);
                    SqlDataReader dr = objCore.funGetDataReader1(q);
                    if (dr.Read())
                    {
                        string resid = dr[2].ToString();
                        if (resid.Length > 0)
                        {
                            resid = " , Res ID: " + resid;
                        }
                        tbl = "TBL:" + dr[0].ToString() + " , Guests: " + dr[1].ToString() + resid;
                    }
                }
                if (ordertype == "Delivery")
                {
                    string q = "select name,address,phone from delivery where Saleid='" + id + "'";
                    //q = "SELECT dbo.DinInTables.TableNo, dbo.ResturantStaff.Name FROM  dbo.DinInTables INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where dbo.DinInTables.Saleid='" + id + "'";
                    // dstbl = objCore.funGetDataSet(q);
                    SqlDataReader dr = objCore.funGetDataReader1(q);
                    if (dr.Read())
                    {
                        string name = "", phone = "", address = "";
                        name = dr[0].ToString();
                        if (chkphone.Checked==true)
                        {
                            phone = "\nPhone=" + dr[2].ToString();
                           
                        }
                        if (chkaddress.Checked == true)
                        {
                            phone = "\nAddress=" + dr[1].ToString();

                        }
                        tbl = name + phone + address;
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {

            }
            return tbl;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (cmbordertype.Text == "Dine In")
            {
                lblfloors.Visible = true;
                cmbfloors.Visible = true;
            }
            else
            {
                lblfloors.Visible = false;
                cmbfloors.Visible = false;
            }
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            try
            {
                CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
                int id = Convert.ToInt32(info.Text);
                FrmInvoiceDetailsSale obj = new FrmInvoiceDetailsSale();
                obj.saleid = id.ToString();
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
               
            }

        }
    }
}
