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
    public partial class FrmMessagesSale : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmMessagesSale()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
           
            
        }
        
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptmessagesale rptDoc = new rptmessagesale();
                POSRestaurant.Reports.SaleReports.dsmessagesalexsd dsrpt = new dsmessagesalexsd();
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
                rptDoc.SetParameterValue("Addrs",address );
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("totaltrans", getquantity());
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
        public double getquantity()
        {
            double qty = 0;
            try
            {
                string q = "SELECT     count(*) AS qty FROM         Sale  WHERE     (Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') ";
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
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Customer", typeof(string));
                dtrpt.Columns.Add("phone", typeof(string));
                dtrpt.Columns.Add("Messages", typeof(double));                
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
                if (textBox1.Text == "")
                {
                    q = "SELECT        COUNT(*) AS Expr1, Customer, phone FROM            dbo.Sale WHERE  date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and   (phone IS NOT NULL) GROUP BY Customer, phone";
                }
                else
                {
                    q = "SELECT        COUNT(*) AS Expr1, Customer, phone FROM            dbo.Sale WHERE    date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'   and       (phone IS NOT NULL) where Customer like '%" + textBox1.Text + "%' GROUP BY Customer, phone";
                }               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";
                    if (ds.Tables[0].Rows[i]["phone"].ToString().Trim() != "")
                    {
                        if (logo == "")
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Customer"].ToString(), ds.Tables[0].Rows[i]["phone"].ToString(), ds.Tables[0].Rows[i]["Expr1"].ToString(), null);
                        }
                        else
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Customer"].ToString(), ds.Tables[0].Rows[i]["phone"].ToString(), ds.Tables[0].Rows[i]["Expr1"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                        }
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
