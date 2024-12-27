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
    public partial class frmStockEstimationPreview : Form
    {
        public string id = "", date = "", kitchen = "", kdsid = "", orderno = "";
        DataTable dtrpt = new DataTable();
        public frmStockEstimationPreview()
        {
            InitializeComponent();
            dtrpt.Columns.Add("Id", typeof(string));
            dtrpt.Columns.Add("Name", typeof(string));
            dtrpt.Columns.Add("Group", typeof(string));
            dtrpt.Columns.Add("Quantity", typeof(double));
            dtrpt.Columns.Add("UOM", typeof(string));
            dtrpt.Columns.Add("Price", typeof(double));
            
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
                POSRestaurant.Reports.Inventory.rptdemandsheet rptDoc = new rptdemandsheet();
                POSRestaurant.Reports.Inventory.dsdemandsheet dsrpt = new dsdemandsheet();
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
                rptDoc.SetParameterValue("Address", address);
                rptDoc.SetParameterValue("phone", phone);
                rptDoc.SetParameterValue("date",Convert.ToDateTime( date).ToString("dd-MM-yyyy"));
                rptDoc.SetParameterValue("title", "Stock Estimation Sheet");
                
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public DataTable getAllOrders()
        {

           
            DataSet dsinfo = new DataSet();
            try
            {
                
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                string q = "SELECT        id, Flavouid, Menuitemid, Quantity, Date FROM            dbo.StockEstimation WHERE        dbo.StockEstimation.Date='" + date + "' and Quantity>0 and orderno='" + orderno + "'";
                dsinfo = objCore.funGetDataSet(q);
                for (int i = 0; i < dsinfo.Tables[0].Rows.Count; i++)
                {
                  
                    string temp = dsinfo.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    float qty = float.Parse(temp);
                    string menuid = dsinfo.Tables[0].Rows[i]["Menuitemid"].ToString(), flvrid = dsinfo.Tables[0].Rows[i]["Flavouid"].ToString();
                    //if (flvrid == "")
                    //{
                    //    q = "SELECT        TOP (200) Id, MenuItemId, RawItemId, UOMCId, Quantity, Cost, uploadstatus, branchid, modifierid, type, attachmenuid, rowno FROM            Recipe where MenuItemId='"+menuid+"'";
                    //}
                    //else
                    //{
                    //    q = "SELECT        TOP (200) Id, MenuItemId, RawItemId, UOMCId, Quantity, Cost, uploadstatus, branchid, modifierid, type, attachmenuid, rowno FROM            Recipe where MenuItemId='" + menuid + "' and modifierid='"+flvrid+"'";
                    //}
                    //DataSet dsrec = new DataSet();
                    //dsrec = objcore.funGetDataSet(q);
                    //for (int j = 0; j < dsrec.Tables[0].Rows.Count; j++)
                    {
                        recipie(menuid, qty, flvrid, date, "", "", "");
                    }
                   

                }
            }
            catch (Exception ex)
            {

            }

            return dtrpt;
        }
        public void recipie(string itmid, float itmqnty, string flid, string date, string rawid, string branchtype, string recipetype)
        {
            if (recipetype != "Attachmenu")
            {
                attachMenurecipie(itmid, flid, itmqnty, date, branchtype);
            }
            attachrecipie(itmid, itmqnty, date);
            DataSet ds = new DataSet();
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";
                if (flid == "" || flid == "0")
                {
                    q = "SELECT        dbo.RawItem.ItemName, dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.Recipe.modifierid,                          dbo.MenuItem.KDSId, dbo.Groups.GroupName FROM            dbo.Recipe INNER JOIN                          dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Groups ON dbo.RawItem.GroupId = dbo.Groups.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='')";
                }
                else
                {
                    q = "SELECT        dbo.RawItem.ItemName, dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.Recipe.modifierid,                          dbo.MenuItem.KDSId, dbo.Groups.GroupName FROM            dbo.Recipe INNER JOIN                          dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Groups ON dbo.RawItem.GroupId = dbo.Groups.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
                }
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        if (branchtype == "")
                        {
                            // getbranchtype();
                        }
                        if (dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString() == "180")
                        {
                        }
                        string type = dsrecipie.Tables[0].Rows[i]["type"].ToString();
                        if (type == "" || type.ToLower() == "both")
                        {

                        }
                        else
                        {
                            if (type.ToLower() == "dine in" && branchtype.ToLower() == "take away")
                            {
                                chk = false;
                            }
                            else
                                if (type.ToLower() == "take away" && branchtype.ToLower() == "dine in")
                                {
                                    chk = false;
                                }
                        }
                        if (chk == true)
                        {

                            string kdsid = dsrecipie.Tables[0].Rows[i]["kdsid"].ToString();
                            string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                            string uom = dsrecipie.Tables[0].Rows[i]["uom"].ToString();
                            string rawitemName = dsrecipie.Tables[0].Rows[i]["Itemname"].ToString();
                            string group = dsrecipie.Tables[0].Rows[i]["GroupName"].ToString();
                            float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                            double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                            double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                            double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                            amounttodeduct = amounttodeduct * itmqnty;
                            amounttodeduct = amounttodeduct / convrate;
                            amounttodeduct = Math.Round(amounttodeduct, 3);
                            DataSet dsminus = new DataSet();
                            double inventryqty = 0;
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
                                dtrpt.Rows.Add(rawitmid, rawitemName, group, amounttodeduct, uom, price);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void attachMenurecipie(string itmid, string flvid, float itmqnty, string date, string branchtype)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                if (flvid == "" || flvid == "0")
                {
                    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and status='Active' and userecipe='yes' ";
                }
                else
                {
                    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and Flavourid='" + flvid + "' and status='Active' and userecipe='yes' ";
                }
                //q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='MenuItem' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string menuid = dsrecipie.Tables[0].Rows[i]["attachmenuid"].ToString();
                        string flid = dsrecipie.Tables[0].Rows[i]["attachFlavourid"].ToString();
                        if (flid == "")
                        {
                            flid = "0";
                        }
                        float qty = float.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        recipie(menuid, qty, flid, date, "", branchtype, "Attachmenu");
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
        public void attachrecipie(string itmid, float itmqnty, string date)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "";

                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='MenuItem' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.MenuItem.KDSId, dbo.Groups.GroupName, dbo.RawItem.ItemName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.MenuItem ON dbo.AttachRecipe.Menuitemid = dbo.MenuItem.Id INNER JOIN                         dbo.Groups ON dbo.RawItem.GroupId = dbo.Groups.Id WHERE        (dbo.AttachRecipe.type = 'MenuItem') and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";

                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        
                        string type = dsrecipie.Tables[0].Rows[i]["type"].ToString();
                       
                       
                        {
                            try
                            {
                                string kdsid = dsrecipie.Tables[0].Rows[i]["KDSId"].ToString();
                                string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                                string uom = dsrecipie.Tables[0].Rows[i]["uom"].ToString();
                                string rawitemName = dsrecipie.Tables[0].Rows[i]["Itemname"].ToString();
                                string group = dsrecipie.Tables[0].Rows[i]["GroupName"].ToString();
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["attachQty"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;

                                amounttodeduct = amounttodeduct * recipiattachqnty;
                                amounttodeduct = amounttodeduct / convrate;
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
                                    dtrpt.Rows.Add(rawitmid, rawitemName, group, amounttodeduct, uom, price);
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
        private void frmInventoryPreview_Load(object sender, EventArgs e)
        {
            bindreport();
        }
    }
}
