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
    public partial class frmDemandSheet : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public string date = "", branchid = "", type = "";
        public frmDemandSheet()
        {
            InitializeComponent();
        }
       
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            bindreport();
        }

        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();


                POSRestaurant.Reports.Inventory.rptdemandsheet rptDoc = new  rptdemandsheet();
                POSRestaurant.Reports.Inventory.dsdemandsheet dsrpt = new  dsdemandsheet();
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
                rptDoc.SetParameterValue("date", date);
                rptDoc.SetParameterValue("title", "Store Demand Sheet");

                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public double getprice(string id)
        {

            double cost = 0;
            string q = "select  dbo.Getprice('" + DateTime.Now.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "'," + id + ")";
            try
            {
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    cost = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {
            }

            return cost;
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Id", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Group", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Price", typeof(double));
                DataSet ds = new DataSet();
                string q = "";
                q = "select * from Production where date='" + date + "' and branchid='" + branchid + "' and Quantity>0";
                DataSet dsdate = new DataSet();
                dsdate = objCore.funGetDataSet(q);
                for (int i = 0; i < dsdate.Tables[0].Rows.Count; i++)
                {
                    string itemid = dsdate.Tables[0].Rows[i]["Id"].ToString();
                    double quantitydemand = 0;
                    string temp = dsdate.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    quantitydemand = Convert.ToDouble(temp);
                    
                    if (quantitydemand > 0)
                    {
                        q = "SELECT        dbo.RawItem.ItemName, dbo.RecipeProduction.Quantity, dbo.RecipeProduction.RawItemId FROM            dbo.RawItem INNER JOIN                         dbo.RecipeProduction ON dbo.RawItem.Id = dbo.RecipeProduction.RawItemId where dbo.RecipeProduction.ItemId='" + dsdate.Tables[0].Rows[i]["ItemId"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            string rawitemid = ds.Tables[0].Rows[j]["RawItemId"].ToString();
                            if (rawitemid == "64")
                            {

                            }
                            string rawitemName = ds.Tables[0].Rows[j]["ItemName"].ToString();
                            double qty = Convert.ToDouble(ds.Tables[0].Rows[j]["Quantity"].ToString());

                            string group = "", uom = "";
                            double rate = 1;
                            DataSet dscon = new DataSet();
                            q = "SELECT        dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate, dbo.Groups.GroupName FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.Groups ON dbo.RawItem.GroupId = dbo.Groups.Id where dbo.RawItem.Id='" + rawitemid + "'";
                           dscon = objCore.funGetDataSet(q);
                            if (dscon.Tables[0].Rows.Count > 0)
                            {
                                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                                group = dscon.Tables[0].Rows[0]["GroupName"].ToString();
                                uom = dscon.Tables[0].Rows[0]["UOM"].ToString();
                            }

                            qty = qty / rate;
                            qty = qty * quantitydemand;
                            

                            double existingqty = 0;
                            DataRow[] dr = dtrpt.Select("Id = '" + rawitemid + "'");
                            if (dr.Length > 0)
                            {
                                string avalue = dr[0]["Quantity"].ToString();
                                if (avalue == "")
                                {
                                    avalue = "0";

                                }
                                existingqty = Convert.ToDouble(avalue);
                                existingqty = existingqty + qty;
                                //existingqty = Math.Round(existingqty, 3);
                                foreach (DataRow dtr in dtrpt.Rows)
                                {
                                    if (dtr["Id"].ToString() == rawitemid)
                                    {
                                        dtr["Quantity"] = existingqty;
                                    }
                                }
                            }
                            else
                            {
                                double price = getprice(rawitemid);
                                dtrpt.Rows.Add(rawitemid, rawitemName , group, qty,uom ,price);
                            }
                        }

                        DataSet dsrecipie = new DataSet();
                        DataSet dsminus = new DataSet();
                        try
                        {

                          
                            q = "";

                            q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOM.UOM , dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='Production' and  dbo.AttachRecipe.Menuitemid='" + dsdate.Tables[0].Rows[i]["ItemId"].ToString() + "' ";
                            q = "SELECT        dbo.UOMConversion.Qty, dbo.RawItem.ItemName, dbo.UOMConversion.ConversionRate, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,                          dbo.SubRecipe.Quantity, dbo.Groups.GroupName, dbo.UOM.UOM FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Groups ON dbo.RawItem.GroupId = dbo.Groups.Id  where dbo.AttachRecipe.type='Production' and  dbo.AttachRecipe.Menuitemid='" + dsdate.Tables[0].Rows[i]["ItemId"].ToString() + "' ";
                            string group = "", uom = "";

                            dsrecipie = objCore.funGetDataSet(q);
                            if (dsrecipie.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsrecipie.Tables[0].Rows.Count; j++)
                                {
                                    bool chk = true;
                                    {
                                        try
                                        {
                                            string rawitemName = dsrecipie.Tables[0].Rows[j]["ItemName"].ToString();
                                            group = dsrecipie.Tables[0].Rows[j]["GroupName"].ToString();
                                            uom = dsrecipie.Tables[0].Rows[j]["UOM"].ToString();
                                            string rawitmid = dsrecipie.Tables[0].Rows[j]["RawItemId"].ToString();
                                            float qnty = float.Parse(dsrecipie.Tables[0].Rows[j]["Qty"].ToString());
                                            double convrate = double.Parse(dsrecipie.Tables[0].Rows[j]["ConversionRate"].ToString());
                                            double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[j]["Quantity"].ToString());
                                            double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[j]["attachQty"].ToString());
                                            double amounttodeduct = (qnty / convrate) * recipiqnty;// qnty* recipiqnty;// //;
                                            amounttodeduct = amounttodeduct * quantitydemand;

                                            amounttodeduct = amounttodeduct * recipiattachqnty;
                                           // amounttodeduct = amounttodeduct;
                                            amounttodeduct = Math.Round(amounttodeduct, 3);

                                            double existingqty = 0;
                                            DataRow[] dr = dtrpt.Select("Id = '" + rawitmid + "'");
                                            if (dr.Length > 0)
                                            {
                                                string avalue = dr[0]["Quantity"].ToString();
                                                if (avalue == "")
                                                {
                                                    avalue = "0";

                                                }
                                                existingqty = Convert.ToDouble(avalue);
                                                existingqty = existingqty + amounttodeduct;
                                                //existingqty = Math.Round(existingqty, 3);
                                                foreach (DataRow dtr in dtrpt.Rows)
                                                {
                                                    if (dtr["Id"].ToString() == rawitmid)
                                                    {
                                                        dtr["Quantity"] = existingqty;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                double price = getprice(rawitmid);
                                                dtrpt.Rows.Add(rawitmid, rawitemName , group, amounttodeduct,uom,price);
                                            }
                                        }
                                        catch (Exception exx)
                                        {


                                        }

                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        finally
                        {

                            dsrecipie.Dispose();
                            dsminus.Dispose();
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
            dtrpt.DefaultView.Sort = "Name ASC";
            dtrpt = dtrpt.DefaultView.ToTable();
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
