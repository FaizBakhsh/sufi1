
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
    public partial class FrmKDS : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmKDS()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            
            
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


                POSRestaurant.Reports.SaleReports.rptKDS rptDoc = new rptKDS();
                POSRestaurant.Reports.SaleReports.dsKDS dsrpt = new dsKDS();
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
                rptDoc.SetParameterValue("date", "from " + dateTimePicker1.Text+" to "+dateTimePicker2.Text);
               
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
                dtrpt.Columns.Add("Size", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Modifier", typeof(string));
                dtrpt.Columns.Add("rModifier", typeof(string));

                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("OrderTime", typeof(DateTime));
                dtrpt.Columns.Add("CompletedTime", typeof(DateTime));
                dtrpt.Columns.Add("Minutes", typeof(int));
                DataSet ds = new DataSet();
                string q = "";
                
                    if (textBox1.Text == "")
                    {
                        q = "SELECT        dbo.ModifierFlavour.name AS size, dbo.MenuItem.Name, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS RModifier, dbo.Saledetails.Quantity, dbo.Saledetails.time as OrderTime, dbo.Saledetails.completedtime,DATEDIFF(MINUTE,dbo.Saledetails.time , dbo.Saledetails.completedtime) as Minutes FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' order by dbo.menuitem.name";
                    }
                    else
                    {
                        q = "SELECT        dbo.ModifierFlavour.name AS size, dbo.MenuItem.Name, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS RModifier, dbo.Saledetails.Quantity, dbo.Saledetails.time as OrderTime,  dbo.Saledetails.completedtime,DATEDIFF(MINUTE,dbo.Saledetails.time , dbo.Saledetails.completedtime) as Minutes FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id where dbo.sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.menuitem.like '%" + textBox1.Text + "%' order by dbo.menuitem.name";
                    }
               
                ds = objCore.funGetDataSet(q);
                dtrpt.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{

                //    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["size"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Modifier"].ToString(), ds.Tables[0].Rows[i]["RModifier"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["time"].ToString(), ds.Tables[0].Rows[i]["completedtime"].ToString());

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

        private void crystalReportViewer1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void crystalReportViewer1_Click(object sender, EventArgs e)
        {
            
        }

        private void crystalReportViewer1_ClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
           
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            
        }
    }
}
