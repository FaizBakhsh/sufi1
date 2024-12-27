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
    public partial class FrmHourlySale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmHourlySale()
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
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rprHourlySale rptDoc = new rprHourlySale();
                POSRestaurant.Reports.SaleReports.DsHourlySale dsrpt = new DsHourlySale();
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
                else
                {
                    if (logo == "")
                    { }
                    else
                    {

                        dt.Rows.Add("", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "as on " + dateTimePicker1.Text );

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
                
                dtrpt.Columns.Add("Time", typeof(string));
                dtrpt.Columns.Add("Count", typeof(double));
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
                    q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '9') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='"+cmbbranch.SelectedValue+"'";

               
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("09:00 to 10:00",Convert.ToDouble(val),  null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("09:00 to 10:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                
                }
                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '10') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and branchid='" + cmbbranch.SelectedValue + "'";

                
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("10:00 to 11:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("10:00 to 11:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }
                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '11') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("11:00 to 12:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("11:00 to 12:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }
                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '12') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("12:00 to 13:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("12:00 to 13:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }
                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '13') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("13:00 to 14:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("13:00 to 14:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }
                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '14') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("14:00 to 15:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("14:00 to 15:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '15') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("15:00 to 16:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("15:00 to 16:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }
                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '16') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("16:00 to 17:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("16:00 to 17:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '17') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("17:00 to 18:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("17:00 to 18:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '18') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("18:00 to 19:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("18:00 to 19:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '19') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("19:00 to 20:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("19:00 to 20:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '20') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("20:00 to 21:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("20:00 to 21:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '21') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("21:00 to 22:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("21:00 to 22:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '22') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("22:00 to 23:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("22:00 to 23:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '23') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("23:00 to 00:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("23:00 to 00:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }
                string strt = Convert.ToDateTime(dateTimePicker1.Text).AddDays(1).ToString("yyyy-MM-dd");
                string end = Convert.ToDateTime(dateTimePicker1.Text).AddDays(1).ToString("yyyy-MM-dd");

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '00') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("00:00 to 01:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("00:00 to 01:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '1') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("01:00 to 02:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("01:00 to 02:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '2') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";


                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("02:00 to 03:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("02:00 to 03:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '3') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("03:00 to 04:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("03:00 to 04:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }
                
                    q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '4') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("04:00 to 05:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("04:00 to 05:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '5') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("05:00 to 06:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("05:00 to 06:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '6') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

               

                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("06:00 to 07:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("06:00 to 07:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }

                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '7') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";

                
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    if (logo == "")
                    {
                        dtrpt.Rows.Add("07:00 to 08:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("07:00 to 08:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
                    }
                }

                q = "SELECT     SUM(TotalBill - DiscountAmount + GST) AS sum FROM         Sale WHERE billstatus='Paid' and  (DATEPART(hh, time) = '8') and  (date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and branchid='" + cmbbranch.SelectedValue + "'";
                
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    val = ds.Tables[0].Rows[i]["sum"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }

                    if (logo == "")
                    {
                        dtrpt.Rows.Add("08:00 to 09:00", Convert.ToDouble(val), null);
                    }
                    else
                    {
                        dtrpt.Rows.Add("08:00 to 09:00", Convert.ToDouble(val), dscompany.Tables[0].Rows[0]["logo"]);
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
