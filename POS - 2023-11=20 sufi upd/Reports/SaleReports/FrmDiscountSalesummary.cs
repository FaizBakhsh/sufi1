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
    public partial class FrmDiscountSalesummary : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmDiscountSalesummary()
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
            try
            {
                ds = new DataSet();
                string q = "select name+'('+cast(discount as varchar(100))+')' as discount,id from DiscountKeys";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["discount"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "discount";
                comboBox1.Text = "All";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            try
            {
                ds = new DataSet();
                string q = "select Id, Name from Shifts";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox2.DataSource = ds.Tables[0];
                comboBox2.ValueMember = "Id";
                comboBox2.DisplayMember = "Name";
                comboBox2.Text = "All";
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
                getcompany();
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptDiscountSummary rptDoc = new  rptDiscountSummary();
                POSRestaurant.Reports.SaleReports.dsdiscountsummary dsrpt = new dsdiscountsummary();
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
                rptDoc.SetParameterValue("Addrs", address);
                rptDoc.SetParameterValue("phn", phone);
                rptDoc.SetParameterValue("date", "for the period " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        protected string getname(string id)
        {
            string name = "";

            try
            {
                DataSet dsitem = new DataSet();
                string q = "select name from DiscountKeys  where id='" + id + "'";
                dsitem = objCore.funGetDataSet(q);
                if (dsitem.Tables[0].Rows.Count > 0)
                {

                    name = dsitem.Tables[0].Rows[0][0].ToString();

                }
            }
            catch (Exception ex)
            {


            }

            return name;

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Amount", typeof(double));
                //dtrpt.Columns.Add("logo", typeof(byte[]));
                dtrpt.Columns.Add("Type", typeof(string));
                
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

                //if (comboBox2.Text == "All")
                //{
                //    if (comboBox1.Text == "All")
                //    {
                //        q = "SELECT        dbo.DiscountKeys.Name, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id WHERE        (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   GROUP BY dbo.DiscountKeys.name";
                //    }
                //    else
                //    {
                //        q = "SELECT        dbo.DiscountKeys.Name, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id WHERE     dbo.Sale.discountkeyid='" + comboBox1.SelectedValue + "'  and    (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   GROUP BY dbo.DiscountKeys.name";
                //    }
                //}
                //else
                //{
                //    if (comboBox1.Text == "All")
                //    {
                //        q = "SELECT        dbo.DiscountKeys.Name, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id WHERE        (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'    GROUP BY dbo.DiscountKeys.name";
                //    }
                //    else
                //    {
                //        q = "SELECT        dbo.DiscountKeys.Name, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountKeys ON dbo.Sale.discountkeyid = dbo.DiscountKeys.id WHERE     dbo.Sale.discountkeyid='" + comboBox1.SelectedValue + "'  and    (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'   GROUP BY dbo.DiscountKeys.name";
                //    }                    
                //}
                if (comboBox2.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        dbo.Sale.discountkeyid, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale  WHERE        (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.billstatus='Paid'   GROUP BY Sale.discountkeyid";
                    }
                    else
                    {
                        q = "SELECT        dbo.Sale.discountkeyid, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale  WHERE     dbo.Sale.discountkeyid='" + comboBox1.SelectedValue + "'  and    (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and sale.billstatus='Paid'  GROUP BY Sale.discountkeyid";
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        dbo.Sale.discountkeyid, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale  WHERE        (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and sale.billstatus='Paid'   Sale.discountkeyid";
                    }
                    else
                    {
                        q = "SELECT        dbo.Sale.discountkeyid, COUNT(dbo.Sale.discountkeyid) AS Quantity, SUM(dbo.Sale.DiscountAmount) AS Amount, 'Overall' AS Type FROM            dbo.Sale  WHERE     dbo.Sale.discountkeyid='" + comboBox1.SelectedValue + "'  and    (dbo.Sale.DiscountAmount > 0) and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid'  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'   GROUP BY Sale.discountkeyid";
                    }
                }
                ds = objCore.funGetDataSet(q);
                //dtrpt.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    string keyname = getname(ds.Tables[0].Rows[i]["discountkeyid"].ToString());
                    dtrpt.Rows.Add(keyname, ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["Amount"].ToString(), ds.Tables[0].Rows[i]["Type"].ToString());

                }

                if (comboBox2.Text == "All")
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        dbo.DiscountKeys.name, COUNT(dbo.DiscountKeys.id) AS Quantity, SUM(dbo.DiscountIndividual.discount) AS Amount, 'Individual' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id where (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.billstatus='Paid'   GROUP BY dbo.DiscountKeys.name";
                    }
                    else
                    {
                        q = "SELECT        dbo.DiscountKeys.name, COUNT(dbo.DiscountKeys.id) AS Quantity, SUM(dbo.DiscountIndividual.discount) AS Amount, 'Individual' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id where  dbo.DiscountKeys.id='" + comboBox1.SelectedValue + "'  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  and sale.billstatus='Paid'   and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')   GROUP BY dbo.DiscountKeys.name";
                        
                    }
                }
                else
                {
                    if (comboBox1.Text == "All")
                    {
                        q = "SELECT        dbo.DiscountKeys.name, COUNT(dbo.DiscountKeys.id) AS Quantity, SUM(dbo.DiscountIndividual.discount) AS Amount, 'Individual' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id where (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and sale.billstatus='Paid'  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'   GROUP BY dbo.DiscountKeys.name";
                    }
                    else
                    {
                        q = "SELECT        dbo.DiscountKeys.name, COUNT(dbo.DiscountKeys.id) AS Quantity, SUM(dbo.DiscountIndividual.discount) AS Amount, 'Individual' AS Type FROM            dbo.Sale INNER JOIN                         dbo.DiscountIndividual ON dbo.Sale.Id = dbo.DiscountIndividual.Saleid INNER JOIN                         dbo.DiscountKeys ON dbo.DiscountIndividual.DiscountPerc = dbo.DiscountKeys.id where  dbo.DiscountKeys.id='" + comboBox1.SelectedValue + "'  and   dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'   and sale.billstatus='Paid'   and   (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "')  and dbo.Sale.shiftid='" + comboBox2.SelectedValue + "'   GROUP BY dbo.DiscountKeys.name";

                    }
                }
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["name"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["Amount"].ToString(), ds.Tables[0].Rows[i]["Type"].ToString());

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

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
