using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Reports.waste
{
    public partial class FrmWaste : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmWaste()
        {
            InitializeComponent();
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
                getcompany();
                DataTable dt = new DataTable();


                POSRetail.Reports.waste.rptWaste rptDoc = new  rptWaste();
                POSRetail.Reports.waste.dsWaste dsrpt = new  dsWaste();
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

                        dt.Rows.Add("", "", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                    }
                }
                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("date", "For the period  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
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
                dtrpt.Columns.Add("ItemName", typeof(string));
                dtrpt.Columns.Add("WasteQuantity", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("WasteType", typeof(string));
                dtrpt.Columns.Add("WasteDate", typeof(string));
                dtrpt.Columns.Add("logo", typeof(Byte[]));
                DataSet ds = new DataSet();
                string q = "";
                
                {
                    q = "SELECT      dbo.Waste.id, dbo.RawItem.ItemName, dbo.Waste.quantity AS WasteQuantity, dbo.UOMConversion.UOM, dbo.Waste.type AS WasteType, dbo.Waste.Date AS WasteDate FROM         dbo.RawItem INNER JOIN                      dbo.Waste ON dbo.RawItem.Id = dbo.Waste.itemid INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ORDER BY dbo.Waste.id DESC";

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

                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["WasteQuantity"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["WasteType"].ToString(), ds.Tables[0].Rows[i]["WasteDate"].ToString(), null);
                    }
                    else
                    {



                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["WasteQuantity"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["WasteType"].ToString(), ds.Tables[0].Rows[i]["WasteDate"].ToString(), dscompany.Tables[0].Rows[0]["logo"]);


                    }
                    //DateTime dte = Convert.ToDateTime(ds.Tables[0].Rows[i]["WasteDate"].ToString());
                    //string day = dte.DayOfWeek.ToString();

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
