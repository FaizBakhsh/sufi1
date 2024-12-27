using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.admin
{
    public partial class frmOrderrpt : Form
    {
        public POSRestaurant.classes.Clsdbcon objCore;
        public frmOrderrpt()
        {
            InitializeComponent();
            this.objCore = new classes.Clsdbcon();
        }

        private void frmOrderrpt_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsCategory = this.objCore.funGetDataSet("Select Userid, Name from Vorders Order By Userid");
                DataRow newRow1 = dsCategory.Tables[0].NewRow();
                newRow1["Userid"] = -1;
                newRow1["Name"] = "All Clients";
                dsCategory.Tables[0].Rows.InsertAt(newRow1, dsCategory.Tables[0].Rows.Count);
                this.cmbCategory.DataSource = dsCategory.Tables[0];
                this.cmbCategory.DisplayMember = dsCategory.Tables[0].Columns[1].ToString();
                this.cmbCategory.ValueMember = dsCategory.Tables[0].Columns[0].ToString();
                this.cmbCategory.SelectedIndex = this.cmbCategory.Items.Count - 1;
                this.WindowState = FormWindowState.Normal;

            }
            catch (Exception ex)
            {

            }
        }

        public void bindreport()
        {
            //ReportDocument rptDoc = new ReportDocument();
            rptorder rptDoc = new rptorder();
            dsorder ds = new dsorder();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            // Just set the name of data table
            dt.TableName = "Crystal Report";
            dt = getAllOrders();
            ds.Tables[0].Merge(dt);


            rptDoc.SetDataSource(ds);
            rptDoc.PrintToPrinter(1, false, 0, 0);
            crystalReportViewer1.ReportSource = rptDoc;



        }
        public DataTable getAllOrders()
        {



            DataSet ds = new DataSet();
            SqlDataAdapter adapter = new SqlDataAdapter();
            try
            {
                if (cmbCategory.Text == "All Clients")
                {
                    ds = this.objCore.funGetDataSet("SELECT Name,Phone,Dishname,Price,Quantity,Bill,Orderdate,Status,Deliverydate FROM Vorders");//  
                }
                else
                {
                    ds = this.objCore.funGetDataSet("SELECT Name,Phone,Dishname,Price,Quantity,Bill,Orderdate,Status,Deliverydate FROM Vorders WHERE (Userid = '" + cmbCategory.SelectedValue + "' )");//  

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ds.Tables[0];
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
