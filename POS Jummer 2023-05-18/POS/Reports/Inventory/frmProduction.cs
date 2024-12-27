using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Inventory
{
    public partial class frmProduction : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public frmProduction()
        {
            InitializeComponent();
        }
       
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            string q = "select id,branchname from branch where  status='Active'";
            DataSet ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            try
            {

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "branchname";


            }
            catch (Exception ex)
            {

            }
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.Inventory.rptProduction rptDoc = new rptProduction();
                POSRestaurant.Reports.Inventory.dsProduction dsrpt = new dsProduction();
                //feereport ds = new feereport(); // .xsd file name

                getcompany();
                dt = getAllOrders();
                // Just set the name of data table
                dt.TableName = "Crystal Report";
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
                rptDoc.SetParameterValue("Address",address );
                rptDoc.SetParameterValue("phone", phone);
                rptDoc.SetParameterValue("date", " For the period of " + dateTimePicker1.Text+" to "+dateTimePicker2.Text);


                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        string getuom(string id)
        {
            string val = "";

            try
            {
                string q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where rawitem.id='" + id + "' order by itemname ";
                DataSet dsitem = new DataSet();
                dsitem = objCore.funGetDataSet(q);
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    val = dsitem.Tables[0].Rows[0]["UOM"].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
            return val;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("name", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
               
               

                DataSet ds = new DataSet();
                string q = "", date = "" ;
                if (checkBox1.Checked == true)
                {
                    if (textBox1.Text == "")
                    {
                        q = "SELECT    FORMAT(dbo.Production.Date, 'dd-MM-yyyy') AS Date, dbo.RawItem.ItemName as name, dbo.RawItem.Id, SUM(dbo.Production.Quantity) AS Quantity FROM            dbo.Production INNER JOIN                         dbo.RawItem ON dbo.Production.ItemId = dbo.RawItem.Id where dbo.Production.status='Posted' and dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.Production.branchid='" + comboBox1.SelectedValue + "' GROUP BY dbo.Production.Date, dbo.RawItem.Id,dbo.RawItem.ItemName";
                    }
                    else
                    {
                        q = "SELECT        FORMAT(dbo.Production.Date, 'dd-MM-yyyy') AS Date, dbo.RawItem.ItemName as name,dbo.RawItem.Id, SUM(dbo.Production.Quantity) AS Quantity FROM            dbo.Production INNER JOIN                         dbo.RawItem ON dbo.Production.ItemId = dbo.RawItem.Id where dbo.Production.status='Posted' and dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.Production.branchid='" + comboBox1.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' GROUP BY dbo.Production.Date, dbo.RawItem.Id,dbo.RawItem.ItemName";

                    }
                }
                else
                {
                    if (textBox1.Text == "")
                    {
                        q = "SELECT        '' as Date, dbo.RawItem.ItemName as name,dbo.RawItem.Id, SUM(dbo.Production.Quantity) AS Quantity FROM            dbo.Production INNER JOIN                         dbo.RawItem ON dbo.Production.ItemId = dbo.RawItem.Id where dbo.Production.status='Posted' and dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.Production.branchid='" + comboBox1.SelectedValue + "' GROUP BY dbo.RawItem.Id,dbo.RawItem.ItemName";
                    }
                    else
                    {
                        q = "SELECT        '' as Date, dbo.RawItem.ItemName as name,dbo.RawItem.Id, SUM(dbo.Production.Quantity) AS Quantity FROM            dbo.Production INNER JOIN                         dbo.RawItem ON dbo.Production.ItemId = dbo.RawItem.Id where dbo.Production.status='Posted' and dbo.Production.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and dbo.Production.branchid='" + comboBox1.SelectedValue + "' and dbo.RawItem.ItemName like '%" + textBox1.Text + "%' GROUP BY  dbo.RawItem.Id,dbo.RawItem.ItemName";

                    }
                }
                DataSet dsdate = new DataSet();
               
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string uom = getuom(ds.Tables[0].Rows[i]["Id"].ToString());
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["Date"].ToString(), ds.Tables[0].Rows[i]["name"].ToString(), uom, ds.Tables[0].Rows[i]["Quantity"].ToString());  
                }
               


               // dtrpt.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
            }
            catch (Exception ex)
            {

               
            }
            return dtrpt;
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
