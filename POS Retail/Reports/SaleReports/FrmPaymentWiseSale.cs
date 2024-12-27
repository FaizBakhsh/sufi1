﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.SaleReports
{
    public partial class FrmPaymentWiseSale : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmPaymentWiseSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select id,name from users where usertype='cashier'";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All Users";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                comboBox1.Text = "All Users";
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


                POSRetail.Reports.SaleReports.rprPaymentWiseSale rptDoc = new rprPaymentWiseSale();
                POSRetail.Reports.SaleReports.DsPayementWiseSale dsrpt = new DsPayementWiseSale();
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

                        dt.Rows.Add("", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
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
                dtrpt.Columns.Add("Type", typeof(string));
                dtrpt.Columns.Add("Count", typeof(string));
                dtrpt.Columns.Add("Sum", typeof(string));
                dtrpt.Columns.Add("date", typeof(string));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "";
                if (comboBox1.Text == "All Users")
                {

                    q = "SELECT     SUM(NetBill) AS sum, COUNT(Id) AS count, BillType FROM         Sale WHERE     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and billstatus='Paid' GROUP BY BillType";
                }
                else
                {

                    q = "SELECT     SUM(NetBill) AS sum, COUNT(Id) AS count, BillType FROM         Sale WHERE     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and billstatus='Paid' and userid='" + comboBox1.SelectedValue + "' GROUP BY BillType";
              
                }

                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {


                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (logo == "")
                    {

                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["BillType"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, null);
                    }
                    else
                    {


                        
                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["BillType"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString(), "For the period of " + dateTimePicker1.Text + " to " + dateTimePicker2.Text, dscompany.Tables[0].Rows[0]["logo"]);



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
