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
    public partial class FrmMenuItemSaleRecipe : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmMenuItemSaleRecipe()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                string q = "select id,name from shifts ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                comboBox1.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                ds = new DataSet();
                string q = "select id,name from kds ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbkds.DataSource = ds.Tables[0];
                cmbkds.ValueMember = "id";
                cmbkds.DisplayMember = "name";
                cmbkds.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double getquantity(string itmid)
        {
            double qty = 0;
            try
            {
                string q = "SELECT     SUM(dbo.Saledetails.Quantity) AS qty FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "') AND (dbo.Saledetails.Flavourid = '0') AND (dbo.Saledetails.ModifierId = '0') AND                       (dbo.Saledetails.RunTimeModifierId = '0')";
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
        public double getprice(string itmid)
        {
            double price = 0;
            try
            {
                string q = "SELECT     (dbo.Saledetails.price)  FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE    (dbo.Saledetails.MenuItemId = '" + itmid + "')";
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
                    price = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            return price;
        }
        public double getgst(string itmid)
        {
            double gst = 0;
            DataSet dsgst = new DataSet();
            string q = "SELECT     avg(dbo.Sale.GSTPerc) AS gst FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Saledetails.MenuItemId = '" + itmid + "')";
            dsgst = objCore.funGetDataSet(q);
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                string val = "";
                val = dsgst.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                gst = Convert.ToDouble(val);
            }
            return gst;
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.SaleReports.rptrecipesale rptDoc = new rptrecipesale();
                POSRestaurant.Reports.SaleReports.dsrecipesale dsrpt = new dsrecipesale();
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
        protected string getitemname(string id)
        {
            string name = "";
            try
            {
                string q = "select itemname from rawitem where id='" + id + "'";
                DataSet dss = new DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    name = dss.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
               
            }

            return name;
        }
        protected string getname(string id,string fid)
        {
            string name = "";
            try
            {
                string q = "";
                if (fid == "")
                {
                    q = "SELECT        dbo.MenuItem.Name, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.MenuItem LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where dbo.MenuItem.Id='" + id + "'";
                }
                else
                {
                    q = "SELECT        dbo.MenuItem.Name, dbo.ModifierFlavour.name AS Expr1 FROM            dbo.MenuItem LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where dbo.MenuItem.Id='" + id + "' and  dbo.ModifierFlavour.id='" + fid + "' ";             
                }
                DataSet dss = new DataSet();
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    name = dss.Tables[0].Rows[0]["Expr1"].ToString() + "-" + dss.Tables[0].Rows[0]["Name"].ToString();
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
                dtrpt.Columns.Add("RecipeQty", typeof(double));
                dtrpt.Columns.Add("ItemName", typeof(string));

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
                    q = "SELECT        SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                          dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                }
                else
                {
                    q = "SELECT        SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                          dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id where    (Sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and Sale.BillStatus='paid'  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string id = ds.Tables[0].Rows[i]["MenuItemId"].ToString();
                    string mid = ds.Tables[0].Rows[i]["ModifierId"].ToString();
                    string rid = ds.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    string flid = ds.Tables[0].Rows[i]["Flavourid"].ToString();
                    if (flid == "0")
                    {
                        flid = "";
                    }
                    string temp = ds.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);

                    if (rid == "" || rid == "0")
                    {
                        if (mid == "" || mid == "0")
                        {
                            try
                            {
                                if (flid == "" || flid == "0")
                                {

                                    q = "SELECT        Id, MenuItemId, RawItemId, UOMCId, Quantity, Cost, uploadstatus, branchid, modifierid, type FROM            Recipe where MenuItemId='" + id + "'";
                                }
                                else
                                {
                                    q = "SELECT        Id, MenuItemId, RawItemId, UOMCId, Quantity, Cost, uploadstatus, branchid, modifierid, type FROM            Recipe where MenuItemId='" + id + "' and modifierid='" + flid + "'";
                                }
                                DataSet dss = new DataSet();
                                dss = objCore.funGetDataSet(q);
                                for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                                {
                                    temp = dss.Tables[0].Rows[j]["Quantity"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "0";
                                    }
                                    double recipqty = Convert.ToDouble(temp);
                                    dtrpt.Rows.Add(getitemname(dss.Tables[0].Rows[j]["RawItemId"].ToString()), qty, recipqty, getname(id, flid));
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head FROM            Modifier where id='" + mid + "'";
                            DataSet dss = new DataSet();
                            dss = objCore.funGetDataSet(q);
                            for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                            {
                                temp = dss.Tables[0].Rows[j]["Quantity"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                double recipqty = Convert.ToDouble(temp);
                                dtrpt.Rows.Add(getitemname(dss.Tables[0].Rows[j]["RawItemId"].ToString()), qty, recipqty, getname(id, flid));
                            }
                        }
                    }
                    else
                    {
                        q = "SELECT        id, name, menuItemid, price, Quantity, status, branchid, kdsid, uploadStatus, rawitemid, type, baseimage FROM            RuntimeModifier where id='" + rid + "'";
                        DataSet dss = new DataSet();
                        dss = objCore.funGetDataSet(q);
                        for (int j = 0; j < dss.Tables[0].Rows.Count; j++)
                        {
                            temp = dss.Tables[0].Rows[j]["Quantity"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            double recipqty = Convert.ToDouble(temp);
                            dtrpt.Rows.Add(getitemname(dss.Tables[0].Rows[j]["rawitemid"].ToString()), qty, recipqty, getname(id, flid));
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
