using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.SaleReports
{
    public partial class FrmOrderWiseSale : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmOrderWiseSale()
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


                POSRetail.Reports.SaleReports.rprOrderWiseSale rptDoc = new rprOrderWiseSale();
                POSRetail.Reports.SaleReports.DsOrdeWiseSale dsrpt = new DsOrdeWiseSale();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();
                dsrpt.Tables[0].Merge(dt);


                rptDoc.SetDataSource(dsrpt);
                getcompany();
                string company = "", phone = "", address = "";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                }
                catch (Exception ex)
                {


                }
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
                

                DataSet ds = new DataSet();
                string q = "";
                if (comboBox1.Text == "All Users")
                {

                    q = "SELECT     SUM(NetBill) AS sum, COUNT(Id) AS count, OrderType FROM         Sale WHERE     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') GROUP BY OrderType";
                }
                else
                {

                    q = "SELECT     SUM(NetBill) AS sum, COUNT(Id) AS count, OrderType FROM         Sale WHERE     (Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and userid='" + comboBox1.SelectedValue + "' GROUP BY OrderType";
              
                }

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["OrderType"].ToString(), ds.Tables[0].Rows[i]["count"].ToString(), ds.Tables[0].Rows[i]["sum"].ToString());
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
