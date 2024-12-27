using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class UpdateInventory : Form
    {
        public UpdateInventory()
        {
            InitializeComponent();
        }

        private void UpdateInventory_Load(object sender, EventArgs e)
        {

        }
        public void recipie(string itmid, float itmqnty, string flid, string date, string rawid, string branchtype, string recipetype)
        {
            if (itmid == "1" || itmid == "2" || itmid == "3" )
            {

            }
            if (recipetype != "Attachmenu")
            {
                attachMenurecipie(itmid, flid, itmqnty,date,branchtype,"MenuItem");
            }
            attachrecipie(itmid, itmqnty, date, flid);
            DataSet ds = new DataSet();
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";
                if (flid == "" || flid == "0")
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0')";
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.Recipe.modifierid,                          dbo.MenuItem.KDSId FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='')";
             
                }
                else
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.Recipe.modifierid,                          dbo.MenuItem.KDSId FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
            
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
                        if (dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString() == "362")
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
                            if (rawid == "")
                            {


                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);
                                DataSet dsminus = new DataSet();
                                double inventryqty = 0;

                                dsminus = new DataSet();
                                if (kitchenwiseconsumption == "Enabled")
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where kdsid='" + kdsid + "' and RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                else
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where  RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    //double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    //if (inventryqty > 0)
                                    //{
                                    //    rem = inventryqty;
                                    //}
                                    q = "update InventoryConsumed set  QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    if (kitchenwiseconsumption == "Enabled")
                                    {
                                        q = "insert into InventoryConsumed (kdsid,branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }
                                    else
                                    {
                                        q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }


                                    objCore.executeQuery(q);
                                }

                            }
                            else
                            {
                                if (rawitmid == rawid)
                                {
                                    float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                    double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                    double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                    double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                    amounttodeduct = amounttodeduct * itmqnty;
                                    amounttodeduct = Math.Round(amounttodeduct, 3);
                                    DataSet dsminus = new DataSet();
                                    double inventryqty = 0;

                                    dsminus = new DataSet();
                                    if (kitchenwiseconsumption == "Enabled")
                                    {
                                        dsminus = objCore.funGetDataSet("select * from InventoryConsumed where kdsid='" + kdsid + "' and RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                    }
                                    else
                                    {
                                        dsminus = objCore.funGetDataSet("select * from InventoryConsumed where  RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                    }
                                    if (dsminus.Tables[0].Rows.Count > 0)
                                    {
                                        double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                        //double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                        //if (inventryqty > 0)
                                        //{
                                        //    rem = inventryqty;
                                        //}
                                        q = "update InventoryConsumed set  QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                        objCore.executeQuery(q);
                                    }
                                    else
                                    {
                                        ds = new DataSet();
                                        int idcnsmd = 0;
                                        ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            string ii = ds.Tables[0].Rows[0][0].ToString();
                                            if (ii == string.Empty)
                                            {
                                                ii = "0";
                                            }
                                            idcnsmd = Convert.ToInt32(ii) + 1;
                                        }
                                        else
                                        {
                                            idcnsmd = Convert.ToInt32("1");
                                        }
                                        // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                        if (kitchenwiseconsumption == "Enabled")
                                        {
                                            q = "insert into InventoryConsumed (kdsid,branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                        }
                                        else
                                        {
                                            q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                        }
                                        objCore.executeQuery(q);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void attachMenurecipie(string itmid, string flvid, float itmqnty,string date,string branchtype,string type)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                //if (flvid == "" || flvid == "0")
                //{
                //    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and status='Active' and userecipe='yes' ";
                //}
                //else
                //{
                //    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and Flavourid='" + flvid + "' and status='Active' and userecipe='yes' ";
                //}

                if (type == "RuntimeModifier")
                {
                    if (flvid == "" || flvid == "0")
                    {
                        q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and status='Active' and userecipe='yes' and type='RuntimeModifier'";
                    }
                    else
                    {
                        q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and Flavourid='" + flvid + "' and status='Active' and userecipe='yes' and type='RuntimeModifier'";
                    }
                }
                else
                {
                    if (flvid == "" || flvid == "0")
                    {
                        q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and status='Active' and userecipe='yes'  and (type='MenuItem' or type is null)";
                    }
                    else
                    {
                        q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where menuitemid='" + itmid + "' and Flavourid='" + flvid + "' and status='Active' and userecipe='yes' and (type='MenuItem' or type is null)";
                    }
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
                        recipie(menuid, qty, flid,date,"",branchtype, "Attachmenu");
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
        public void attachrecipie(string itmid, float itmqnty,string date,string flavourid)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "";

                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='MenuItem' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";

                if (flavourid == "" || flavourid == "0")
                {
                    q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.MenuItem.KDSId FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.MenuItem ON dbo.AttachRecipe.Menuitemid = dbo.MenuItem.Id WHERE        (dbo.AttachRecipe.type = 'MenuItem') and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                }
                else
                {
                    q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.MenuItem.KDSId FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.MenuItem ON dbo.AttachRecipe.Menuitemid = dbo.MenuItem.Id WHERE        (dbo.AttachRecipe.type = 'MenuItem') and  dbo.AttachRecipe.Menuitemid='" + itmid + "' AND (dbo.AttachRecipe.FlavourId = '"+flavourid+"') ";
                }
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        if (branchtype == "")
                        {
                            getbranchtype();
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
                            try
                            {
                                string kdsid = dsrecipie.Tables[0].Rows[i]["KDSId"].ToString();
                                string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["attachQty"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;

                                amounttodeduct = amounttodeduct * recipiattachqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);


                                dsminus = new DataSet();
                                double inventryqty = 0;
                               
                                dsminus = new DataSet();
                                if (kitchenwiseconsumption == "Enabled")
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where KDSId='" + kdsid + "' and  RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                else
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where   RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    //double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    //if (inventryqty > 0)
                                    //{
                                    //    rem = inventryqty;
                                    //}
                                    q = "update InventoryConsumed set uploadstatus='Pending', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                   DataSet ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    if (kitchenwiseconsumption == "Enabled")
                                    {
                                        q = "insert into InventoryConsumed (kdsid,branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }
                                    else
                                    {
                                        q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }
                                    objCore.executeQuery(q);
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
        public void recipiemodifierruntime(string itmid, float itmqnty, string date, string rawid, string recipetype)
        {
            if (recipetype != "Attachmenu")
            {
                attachMenurecipie(itmid, "", itmqnty,date,"", "RuntimeModifier");
            }
            attachrecipieruntime(itmid, itmqnty, date);
            DataSet ds = new DataSet();
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";// "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "'";
                // q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Modifier.MenuItemId, dbo.Modifier.RawItemId, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Modifier.Id='" + itmid + "'";
                q = "SELECT     dbo.RuntimeModifier.RawItemId AS RawItemId, dbo.RuntimeModifier.Quantity, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM,dbo.RuntimeModifier.kdsid FROM         dbo.RuntimeModifier INNER JOIN                      dbo.RawItem ON dbo.RuntimeModifier.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RuntimeModifier.Id='" + itmid + "'";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string kdsid = dsrecipie.Tables[0].Rows[i]["kdsid"].ToString();
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        if (rawid == "")
                        {
                            float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                            double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                            double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                            double amounttodeduct = recipiqnty;//(qnty / convrate) * 
                            amounttodeduct = amounttodeduct * itmqnty;
                            dsminus = new DataSet();
                            double inventryqty = 0;
                            dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                            if (dsminus.Tables[0].Rows.Count > 0)
                            {
                                inventryqty = double.Parse(dsminus.Tables[0].Rows[i]["Quantity"].ToString());
                                if (amounttodeduct.ToString().Contains("-"))
                                {
                                    amounttodeduct = Math.Abs(amounttodeduct);
                                }
                                q = "update Inventory set Quantity='" + (inventryqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                objCore.executeQuery(q);
                            }
                            dsminus = new DataSet();
                            if (kitchenwiseconsumption == "Enabled")
                            {
                                dsminus = objCore.funGetDataSet("select * from InventoryConsumed where kdsid='" + kdsid + "' and RawItemId='" + rawitmid + "' and Date='" + date + "'");
                            }
                            else
                            {
                                dsminus = objCore.funGetDataSet("select * from InventoryConsumed where  RawItemId='" + rawitmid + "' and Date='" + date + "'");
                            }
                            if (dsminus.Tables[0].Rows.Count > 0)
                            {
                                double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                                q = "update InventoryConsumed set  QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                objCore.executeQuery(q);
                            }
                            else
                            {
                                ds = new DataSet();
                                int idcnsmd = 0;

                                ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string ii = ds.Tables[0].Rows[0][0].ToString();
                                    if (ii == string.Empty)
                                    {
                                        ii = "0";
                                    }
                                    idcnsmd = Convert.ToInt32(ii) + 1;
                                }
                                else
                                {
                                    idcnsmd = Convert.ToInt32("1");
                                }
                                // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                if (kitchenwiseconsumption == "Enabled")
                                {
                                    q = "insert into InventoryConsumed (kdsid,branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                }
                                else
                                {
                                    q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                }
                                objCore.executeQuery(q);
                            }

                        }
                        else
                        {
                            if (rawitmid == rawid)
                            {
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double amounttodeduct = (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;
                                dsminus = new DataSet();
                                double inventryqty = 0;
                               
                                dsminus = new DataSet();
                                if (kitchenwiseconsumption == "Enabled")
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where kdsid='" + kdsid + "' and RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                else
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where  RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                                    q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    ds = new DataSet();
                                    int idcnsmd = 0;

                                    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    if (kitchenwiseconsumption == "Enabled")
                                    {
                                        q = "insert into InventoryConsumed (kdsid,branchidId,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }
                                    else
                                    {
                                        q = "insert into InventoryConsumed (branchidId,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }
                                    objCore.executeQuery(q);
                                }
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
                ds.Dispose();
                dsrecipie.Dispose();
                dsminus.Dispose();
            }
        }
        public void attachrecipieruntime(string itmid, float itmqnty, string date)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "";

                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='RuntimeModifier' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.RuntimeModifier.kdsid FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.RuntimeModifier ON dbo.AttachRecipe.Menuitemid = dbo.RuntimeModifier.id WHERE        (dbo.AttachRecipe.type = 'RuntimeModifier') and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
              
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        if (branchtype == "")
                        {
                            getbranchtype();
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
                            try
                            {
                                string kdsid = dsrecipie.Tables[0].Rows[i]["kdsid"].ToString();
                                string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["attachQty"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;

                                amounttodeduct = amounttodeduct * recipiattachqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);


                                dsminus = new DataSet();
                                double inventryqty = 0;
                               
                                dsminus = new DataSet();
                                if (kitchenwiseconsumption == "Enabled")
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where kdsid='" + kdsid + "' and RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                else
                                {
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where  RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                }
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    if (inventryqty > 0)
                                    {
                                        rem = inventryqty;
                                    }
                                    q = "update InventoryConsumed set uploadstatus='Pending',RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    DataSet ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    if (kitchenwiseconsumption == "Enabled")
                                    {
                                        q = "insert into InventoryConsumed (kdsid,branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }
                                    else
                                    {
                                        q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    }
                                    objCore.executeQuery(q);
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
        public static string branchtype = "";
        public static string branchid = "";
        protected void getbranchtype()
        {
            try
            {
                string q = "select id,type from branch";
                DataSet dsb = new DataSet();
                dsb = objCore.funGetDataSet(q);
                if (dsb.Tables[0].Rows.Count > 0)
                {
                    branchtype = dsb.Tables[0].Rows[0][1].ToString();
                    branchid = dsb.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void recipiemodifier(string itmid, float itmqnty, string date)
        {
            DataSet ds = new DataSet();
            if (Convert.ToDateTime(date).ToShortDateString() == "2015-09-03" && itmid == "6")
            {

            }
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";// "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "'";
                q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Modifier.RawItemId, dbo.Modifier.Quantity, dbo.Modifier.kdsid FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Modifier.Id='" + itmid + "'";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        string kdsid = dsrecipie.Tables[0].Rows[i]["kdsid"].ToString();
                        float qnty = Convert.ToInt32(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        amounttodeduct = Math.Round(amounttodeduct, 3);
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        
                        dsminus = new DataSet();
                        if (kitchenwiseconsumption == "Enabled")
                        {
                            dsminus = objCore.funGetDataSet("select * from InventoryConsumed where kdsid='" + kdsid + "' and RawItemId='" + rawitmid + "' and Date='" + date + "'");
                      
                        }
                        else
                        {
                            dsminus = objCore.funGetDataSet("select * from InventoryConsumed where  RawItemId='" + rawitmid + "' and Date='" + date + "'");
                      

                        }
                         if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                            q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {

                            ds = new DataSet();
                            int idcnsmd = 0;
                            ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string ii = ds.Tables[0].Rows[0][0].ToString();
                                if (ii == string.Empty)
                                {
                                    ii = "0";
                                }
                                idcnsmd = Convert.ToInt32(ii) + 1;
                            }
                            else
                            {
                                idcnsmd = Convert.ToInt32("1");
                            }
                            //idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                            if (kitchenwiseconsumption == "Enabled")
                            {
                                q = "insert into InventoryConsumed (kdsid,branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                            }
                            else
                            {
                                q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                            }
                            objCore.executeQuery(q);
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public void recipie(string itmid, float itmqnty, string flid, string date)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";
                if (flid == "" || flid=="0")
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='')";
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.modifierid, dbo.MenuItem.KDSId FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='')";
                }
                else
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.modifierid, dbo.MenuItem.KDSId FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
                }

                if (branchid == "")
                {
                    getbranchtype();
                }
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        amounttodeduct = Math.Round(amounttodeduct, 3);
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        string kdsid = dsrecipie.Tables[0].Rows[i]["KDSId"].ToString();
                        dsminus = new DataSet();
                        if (kitchenwiseconsumption == "Enabled")
                        {
                            dsminus = objCore.funGetDataSet("select * from Discard where itemid='" + rawitmid + "' and Date='" + dateTimePicker1.Text + "' and kdsid='"+kdsid+"'");
                        }
                        else
                        {
                            dsminus = objCore.funGetDataSet("select * from Discard where itemid='" + rawitmid + "' and Date='" + dateTimePicker1.Text + "'");
                        }
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = 0;
                            try
                            {
                                inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["completewaste"].ToString());
                            }
                            catch (Exception ex)
                            {


                            }

                            q = "update Discard set completewaste='" + (inventryconsumedqty + amounttodeduct) + "',uploadstatus='Pending'  where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            DataSet ds = new DataSet();
                            int idcnsmd = 0;
                            ds = objCore.funGetDataSet("select MAX(ID) as id from Discard");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string ii = ds.Tables[0].Rows[0][0].ToString();
                                if (ii == string.Empty)
                                {
                                    ii = "0";
                                }
                                idcnsmd = Convert.ToInt32(ii) + 1;
                            }
                            else
                            {
                                idcnsmd = Convert.ToInt32("1");
                            }
                            // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                            if (kitchenwiseconsumption == "Enabled")
                            {
                                q = "insert into Discard (kdsid,branchid,id, itemid, date, quantity, Discard, completewaste, staff) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + date + "','0','0','" + amounttodeduct + "','0')";
                         
                            }
                            else
                            {
                                q = "insert into Discard (branchid,id, itemid, date, quantity, Discard, completewaste, staff) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + date + "','0','0','" + amounttodeduct + "','0')";
                            }
                            objCore.executeQuery(q);
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        string kitchenwiseconsumption = "";
        public void setinventory()
        {
            
            DataSet dsinv = new DataSet();
            dsinv = objCore.funGetDataSet("select * from DeviceSetting where device='Kitchen Wise Inventory'");
            if (dsinv.Tables[0].Rows.Count>0)
            {
                string show = (dsinv.Tables[0].Rows[0]["Status"].ToString());
                try
                {
                    if (show == "Enabled")
                    {
                        kitchenwiseconsumption = "Enabled";
                        
                    }
                    else
                    {
                        kitchenwiseconsumption = "Disabled";
                       
                    }

                }
                catch (Exception ex)
                {

                }
            }
            DataSet ds1 = new System.Data.DataSet();
            string q = "update InventoryConsumed set QuantityConsumed=0,uploadstatus='Pending'  where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
            objcore.executeQuery(q);

           
            
            //try
            //{
                
            //    {

            //        {

            //            q = "SELECT        dbo.sale.OrderType,dbo.Saledetailsrefund.MenuItemId, dbo.Saledetailsrefund.Flavourid, dbo.Saledetailsrefund.ModifierId, dbo.Saledetailsrefund.RunTimeModifierId FROM            dbo.Saledetailsrefund INNER JOIN                         dbo.Sale ON dbo.Saledetailsrefund.saleid = dbo.Sale.Id  where dbo.Saledetailsrefund.MakeStatus='After'  and  sale.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
            //            DataSet ds2 = new System.Data.DataSet();
            //            ds2 = objcore.funGetDataSet(q);
            //            for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
            //            {

            //                if (ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == "0")
            //                {


            //                    if (ds2.Tables[0].Rows[j]["ModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["ModifierId"].ToString() == "0")
            //                    {

            //                        {
            //                            float qty = 0;
            //                            string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
            //                            if (temp == "")
            //                            {
            //                                temp = "1";
            //                            }
            //                            string flv = ds2.Tables[0].Rows[j]["Flavourid"].ToString();
            //                            if (flv == "0")
            //                            {
            //                                flv = "";
            //                            }
            //                            qty = float.Parse(temp);
            //                            //recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, ds2.Tables[0].Rows[j]["date"].ToString(), "", ds2.Tables[0].Rows[j]["OrderType"].ToString(), "");
            //                            recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, Convert.ToDateTime(ds2.Tables[0].Rows[j]["date"].ToString()).ToString("yyyy-MM-dd"));
          
            //                        }

            //                    }

            //                    else
            //                    {
            //                        float qty = 0;
            //                        string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
            //                        if (temp == "")
            //                        {
            //                            temp = "1";
            //                        }
            //                        qty = float.Parse(temp);
            //                        recipiemodifier(ds2.Tables[0].Rows[j]["ModifierId"].ToString(), qty, ds2.Tables[0].Rows[j]["date"].ToString());
            //                        recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, Convert.ToDateTime(ds2.Tables[0].Rows[j]["date"].ToString()).ToString("yyyy-MM-dd"));
          

            //                    }
            //                }
            //                else
            //                {
            //                    float qty = 0;
            //                    string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
            //                    if (temp == "")
            //                    {
            //                        temp = "1";
            //                    }
            //                    qty = float.Parse(temp);
            //                    recipiemodifierruntime(ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString(), qty, ds2.Tables[0].Rows[j]["date"].ToString(), "", "");
            //                }
            //            }
            //        }

            //    }
            //}
            //catch (Exception ex)
            //{

            //}

            try
            {
                q = "select id,date,OrderType from sale where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and billstatus='Paid' order by date";
                ds1 = objcore.funGetDataSet(q);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    {

                        q = "select *  from saledetails where saleid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        DataSet ds2 = new System.Data.DataSet();
                        ds2 = objcore.funGetDataSet(q);
                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {

                            if (ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == "0")
                            {


                                if (ds2.Tables[0].Rows[j]["ModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["ModifierId"].ToString() == "0")
                                {

                                    {
                                        float qty = 0;
                                        string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                        if (temp == "")
                                        {
                                            temp = "1";
                                        }
                                        string flv = ds2.Tables[0].Rows[j]["Flavourid"].ToString();
                                        if (flv == "0")
                                        {
                                            flv = "";
                                        }
                                        qty = float.Parse(temp);
                                        recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, ds1.Tables[0].Rows[i]["date"].ToString(), "", ds1.Tables[0].Rows[i]["OrderType"].ToString(), "");
                                    }

                                }

                                else
                                {
                                    float qty = 0;
                                    string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "1";
                                    }
                                    qty = float.Parse(temp);
                                    recipiemodifier(ds2.Tables[0].Rows[j]["ModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString());

                                }
                            }
                            else
                            {
                                float qty = 0;
                                string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                if (temp == "")
                                {
                                    temp = "1";
                                }
                                qty = float.Parse(temp);
                                recipiemodifierruntime(ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString(), "","");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                
            }

            try
            {
                q = "select id,date,OrderType from DSSale where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and billstatus='Paid' order by date";
                ds1 = objcore.funGetDataSet(q);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {

                    {
                        q = "select *  from DSsaledetails where saleid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                        DataSet ds2 = new System.Data.DataSet();
                        ds2 = objcore.funGetDataSet(q);
                        for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                        {
                            if (ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == "0")
                            {
                                if (ds2.Tables[0].Rows[j]["ModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["ModifierId"].ToString() == "0")
                                {

                                    {
                                        float qty = 0;
                                        string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                        if (temp == "")
                                        {
                                            temp = "1";
                                        }
                                        string flv = ds2.Tables[0].Rows[j]["Flavourid"].ToString();
                                        if (flv == "0")
                                        {
                                            flv = "";
                                        }
                                        qty = float.Parse(temp);
                                        recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, ds1.Tables[0].Rows[i]["date"].ToString(), "", ds1.Tables[0].Rows[i]["OrderType"].ToString(), "");
                                    }

                                }

                                else
                                {
                                    float qty = 0;
                                    string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "1";
                                    }
                                    qty = float.Parse(temp);
                                    recipiemodifier(ds2.Tables[0].Rows[j]["ModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString());

                                }
                            }
                            else
                            {
                                float qty = 0;
                                string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                if (temp == "")
                                {
                                    temp = "1";
                                }
                                qty = float.Parse(temp);
                                recipiemodifierruntime(ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString(), "","");
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void voidattachMenurecipie(string itmid, string flvid, float itmqnty, string date, string branchtype,string type)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                if (flvid == "" || flvid == "0")
                {
                    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where Type='" + type + "' and  menuitemid='" + itmid + "' and status='Active' and userecipe='yes' ";
                }
                else
                {
                    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where Type='" + type + "' and  menuitemid='" + itmid + "' and Flavourid='" + flvid + "' and status='Active' and userecipe='yes' ";
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
                       // voidrecipie(menuid, qty, flid, branchtype, "Attachmenu");
                        recipie(menuid, qty, flid, Convert.ToDateTime(date).ToString("yyyy-MM-dd"));
          
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
        public void voidattachrecipieruntime(string itmid, float itmqnty,string date)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "";

                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.RuntimeModifier.kdsid FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.RuntimeModifier ON dbo.AttachRecipe.Menuitemid = dbo.RuntimeModifier.id WHERE        (dbo.AttachRecipe.type = 'RuntimeModifier') and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        if (branchtype == "")
                        {
                            getbranchtype();
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
                            try
                            {
                                string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                                string kdsid = dsrecipie.Tables[0].Rows[i]["kdsid"].ToString();
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["attachQty"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;

                                amounttodeduct = amounttodeduct * recipiattachqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);


                                dsminus = new DataSet();
                                double inventryqty = 0;

                                dsminus = new DataSet();
                                //if (kitchenwiseconsumtion == "Enabled")
                                //{
                                //    dsminus = objCore.funGetDataSet("select * from Discard where itemid='" + rawitmid + "' and Date='" + date + "' and kdsid='" + kdsid + "'");
                                //}
                                //else
                                {
                                    dsminus = objCore.funGetDataSet("select * from Discard where itemid='" + rawitmid + "' and Date='" + date + "'");
                                }
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = 0;
                                    try
                                    {
                                        inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["completewaste"].ToString());
                                    }
                                    catch (Exception ex)
                                    {


                                    }

                                    q = "update Discard set completewaste='" + (inventryconsumedqty + amounttodeduct) + "',uploadstatus='Pending' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    DataSet ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objCore.funGetDataSet("select MAX(ID) as id from Discard");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    //if (kitchenwiseconsumtion == "Enabled")
                                    //{
                                    //    q = "insert into Discard (kdsid,branchid,id, itemid, date, quantity, Discard, completewaste, staff) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + date + "','0','0','" + amounttodeduct + "','0')";
                                    //}
                                    //else
                                    {
                                        q = "insert into Discard (branchid,id, itemid, date, quantity, Discard, completewaste, staff) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + date + "','0','0','" + amounttodeduct + "','0')";
                                    }

                                    objCore.executeQuery(q);
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
        public void voidrecipiemodifierruntime(string itmid, float itmqnty, string recipetype,string date)
        {
            if (recipetype != "Attachmenu")
            {
                voidattachMenurecipie(itmid, "", itmqnty,date, "both", "RuntimeModifier");
            }

            voidattachrecipieruntime(itmid, itmqnty,date);

            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "SELECT     dbo.RuntimeModifier.RawItemId AS RawItemId, dbo.RuntimeModifier.Quantity, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM,dbo.RuntimeModifier.kdsid FROM         dbo.RuntimeModifier INNER JOIN                      dbo.RawItem ON dbo.RuntimeModifier.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RuntimeModifier.Id='" + itmid + "'";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        string kdsid = dsrecipie.Tables[0].Rows[i]["kdsid"].ToString();
                        float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        dsminus = new DataSet();
                        double inventryqty = 0;

                        dsminus = new DataSet();
                        //if (kitchenwiseconsumtion == "Enabled")
                        //{
                        //    dsminus = objCore.funGetDataSet("select * from Discard where itemid='" + rawitmid + "' and Date='" + date + "' and kdsid='" + kdsid + "'");
                        //}
                        //else
                        {
                            dsminus = objCore.funGetDataSet("select * from Discard where itemid='" + rawitmid + "' and Date='" + date + "'");
                        }
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = 0;
                            try
                            {
                                inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["completewaste"].ToString());
                            }
                            catch (Exception ex)
                            {


                            }

                            q = "update Discard set completewaste='" + (inventryconsumedqty + amounttodeduct) + "',uploadstatus='Pending' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            DataSet ds = new DataSet();
                            int idcnsmd = 0;
                            ds = objCore.funGetDataSet("select MAX(ID) as id from Discard");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string ii = ds.Tables[0].Rows[0][0].ToString();
                                if (ii == string.Empty)
                                {
                                    ii = "0";
                                }
                                idcnsmd = Convert.ToInt32(ii) + 1;
                            }
                            else
                            {
                                idcnsmd = Convert.ToInt32("1");
                            }
                            // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                            //if (kitchenwiseconsumtion == "Enabled")
                            //{
                            //    q = "insert into Discard (kdsid,branchid,id, itemid, date, quantity, Discard, completewaste, staff) values('" + kdsid + "','" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + date + "','0','0','" + amounttodeduct + "','0')";
                            //}
                            //else
                            {
                                q = "insert into Discard (branchid,id, itemid, date, quantity, Discard, completewaste, staff) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + date + "','0','0','" + amounttodeduct + "','0')";
                            }

                            objCore.executeQuery(q);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
               // ds.Dispose();
                dsrecipie.Dispose();
                dsminus.Dispose();
            }
        }
        private void completewaste()
        {

            try
            {
                string q = "update Discard set completewaste=0,uploadstatus='Pending'  where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' and completewaste>0";
                objcore.executeQuery(q);
                q = "SELECT       sale.date, dbo.Saledetailsrefund.MenuItemId, dbo.Saledetailsrefund.Flavourid, dbo.Saledetailsrefund.ModifierId, dbo.Saledetailsrefund.RunTimeModifierId, dbo.Saledetailsrefund.Quantity, dbo.Saledetailsrefund.type FROM            dbo.Sale INNER JOIN                         dbo.Saledetailsrefund ON dbo.Sale.Id = dbo.Saledetailsrefund.saleid where sale.date between '"+dateTimePicker1.Text+"' and '"+dateTimePicker2.Text+"'  and dbo.Saledetailsrefund.MakeStatus='After'";
                DataSet dsrevs = new DataSet();
                dsrevs = objcore.funGetDataSet(q);
                for (int i = 0; i < dsrevs.Tables[0].Rows.Count; i++)
                {
                    if (dsrevs.Tables[0].Rows[0]["Flavourid"].ToString() == "0")
                    {
                        if (dsrevs.Tables[0].Rows[0]["ModifierId"].ToString() == "0")
                        {
                            if (dsrevs.Tables[0].Rows[0]["RunTimeModifierId"].ToString() != "0")
                            {
                                voidrecipiemodifierruntime(dsrevs.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), 1, "", Convert.ToDateTime(dsrevs.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                //voidrecipie(dsrevs.Tables[0].Rows[0]["MenuItemId"].ToString(), 1, dsrevs.Tables[0].Rows[0]["Flavourid"].ToString(), "", "");
                                try
                                {
                                    q = "select * from CompleteWaste where Flavouid='" + dsrevs.Tables[0].Rows[i]["Flavourid"].ToString() + "' and Menuitemid='" + dsrevs.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and date='" + Convert.ToDateTime(dsrevs.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd") + "' and Type='After Making'";
                                    DataSet dss = new DataSet();
                                    dss = objcore.funGetDataSet(q);
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                    }
                                    else
                                    {
                                        q = "insert into CompleteWaste (Type,Flavouid, Menuitemid, Quantity, Date, Status) values('After Making','" + dsrevs.Tables[0].Rows[i]["Flavourid"].ToString() + "','" + dsrevs.Tables[0].Rows[i]["MenuItemId"].ToString() + "','" + dsrevs.Tables[0].Rows[i]["Quantity"].ToString() + "','" + Convert.ToDateTime(dsrevs.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd") + "','Posted')";
                                        objCore.executeQuery(q);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                        else
                        {
                            if (dsrevs.Tables[0].Rows[0]["RunTimeModifierId"].ToString() != "0")
                            {
                                voidrecipiemodifierruntime(dsrevs.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), 1, "", Convert.ToDateTime(dsrevs.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd"));
                            }
                            else
                            {
                                //recipiemodifier(dsrevs.Tables[0].Rows[0]["ModifierId"].ToString(), -1);
                            }
                        }
                    }
                    else
                    {

                        //voidrecipie(dsrevs.Tables[0].Rows[0]["MenuItemId"].ToString(), Convert.ToInt32(dsrevs.Tables[0].Rows[0]["Quantity"].ToString()) * 1, dsrevs.Tables[0].Rows[0]["Flavourid"].ToString(), "", "");
                        try
                        {
                            q = "select * from CompleteWaste where Flavouid='" + dsrevs.Tables[0].Rows[i]["Flavourid"].ToString() + "' and Menuitemid='" + dsrevs.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and date='" + Convert.ToDateTime(dsrevs.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd") + "' and Type='After Making'";
                            DataSet dss = new DataSet();
                            dss = objcore.funGetDataSet(q);
                            if (dss.Tables[0].Rows.Count > 0)
                            {
                            }
                            else
                            {
                                q = "insert into CompleteWaste (Type,Flavouid, Menuitemid, Quantity, Date, Status) values('After Making','" + dsrevs.Tables[0].Rows[i]["Flavourid"].ToString() + "','" + dsrevs.Tables[0].Rows[i]["MenuItemId"].ToString() + "','" + dsrevs.Tables[0].Rows[i]["Quantity"].ToString() + "','" + Convert.ToDateTime(dsrevs.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd") + "','Posted')";
                                objCore.executeQuery(q);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                q = "select * from CompleteWaste  where status='Posted'  and  Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                DataSet dswaste = new DataSet();
                dswaste = objcore.funGetDataSet(q);
                for (int i = 0; i < dswaste.Tables[0].Rows.Count; i++)
                {
                    recipie(dswaste.Tables[0].Rows[i]["Menuitemid"].ToString(), float.Parse(dswaste.Tables[0].Rows[i]["Quantity"].ToString()), dswaste.Tables[0].Rows[i]["Flavouid"].ToString(), Convert.ToDateTime(dswaste.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd"));
                }
            }
            catch (Exception ex)
            {
                
            }


            
        }
        private void productionrecipe()
        {
           
            try
            {
                string branchidd = "";
                string q = "update Production set status='Pending' where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                objcore.executeQuery(q);


                 q = "SELECT    Id, Date, ItemId, Quantity, Userid, Status, branchid, storeid, uploadstatus FROM            Production where date between '"+dateTimePicker1.Text+"' and '"+dateTimePicker2.Text+"'";
                DataSet dsproduction = new DataSet();
                dsproduction = objcore.funGetDataSet(q);
                foreach (DataRow dgr in dsproduction.Tables[0].Rows)
                {
                    if (dgr["Status"].ToString() != "Posted")
                    {
                        string date = Convert.ToDateTime( dgr["date"].ToString()).ToString("yyyy-MM-dd");
                        branchidd = dgr["branchid"].ToString();
                        string itemid = dgr["ItemId"].ToString();
                        string temp = dgr["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        float quantity = float.Parse(temp);

                      
                        float rate = 1;
                        //try
                        //{
                        //    DataSet dscon1 = new DataSet();
                        //    q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + itemid + "'";
                        //    dscon1 = objcore.funGetDataSet(q);
                        //    if (dscon1.Tables[0].Rows.Count > 0)
                        //    {
                        //        rate = float.Parse(dscon1.Tables[0].Rows[0]["ConversionRate"].ToString());
                        //    }
                        //}
                        //catch (Exception ex)
                        //{


                        //}
                        //if (rate > 1)
                        //{
                        //  //  quantity = quantity * rate;
                        //}
                      
                        if (Convert.ToDouble(quantity) > 0)
                        {
                            Productionattachrecipie(itemid, quantity, branchidd, date);
                            q = "select * from RecipeProduction where itemid='" + itemid + "'";
                            DataSet ds = new DataSet();
                            ds = objcore.funGetDataSet(q);
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                
                                string rawitmid = ds.Tables[0].Rows[i]["RawItemId"].ToString();

                                float recipeqty = float.Parse(ds.Tables[0].Rows[i]["quantity"].ToString());
                               
                                //try
                                //{
                                //    DataSet dscon1 = new DataSet();
                                //    q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + rawitmid + "'";
                                //    dscon1 = objcore.funGetDataSet(q);
                                //    if (dscon1.Tables[0].Rows.Count > 0)
                                //    {
                                //        rate = float.Parse(dscon1.Tables[0].Rows[0]["ConversionRate"].ToString());
                                //    }
                                //}
                                //catch (Exception ex)
                                //{


                                //}
                                
                                //if (rate > 1)
                                //{
                                //    recipeqty = recipeqty / rate;
                                //}
                                float qty = quantity;
                                double finalqty = qty * recipeqty;
                                finalqty = Math.Round(finalqty, 3);

                                DataSet dsminus = new DataSet();
                                dsminus = objcore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());

                                    q = "update InventoryConsumed set  QuantityConsumed='" + (inventryconsumedqty + finalqty) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "' ";
                                    objcore.executeQuery(q);
                                }
                                else
                                {
                                    DataSet dsc = new DataSet();
                                    int idcnsmd = 0;
                                    dsc = objcore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (dsc.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = dsc.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }

                                    q = "insert into InventoryConsumed (id,branchid,RawItemId,QuantityConsumed,Date) values('" + idcnsmd + "','" + branchidd + "','" + rawitmid + "','" + finalqty + "','" + date + "')";
                                    objcore.executeQuery(q);
                                }
                            }
                            q = "update Production set status='Posted' where  itemid='" + itemid + "' and date='" + date + "' and branchid='" + branchidd + "'";
                            objcore.executeQuery(q);
                        }

                    }
                }
                //q = "update Production set status='Posted' where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                //objcore.executeQuery(q);
            }
            catch (Exception ex)
            {

            }
        }
        public void Productionattachrecipie(string itmid, float itmqnty,string branchidd,string date)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "";

                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='Production' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                dsrecipie = objcore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        {
                            try
                            {
                                string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["attachQty"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;

                                amounttodeduct = amounttodeduct * recipiattachqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);


                                dsminus = new DataSet();
                                double inventryqty = 0;
                                dsminus = objcore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");

                                dsminus = new DataSet();
                                dsminus = objcore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "' and branchid='" + branchidd + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    if (inventryqty > 0)
                                    {
                                        rem = inventryqty;
                                    }
                                    q = "update InventoryConsumed set uploadstatus='Pending',RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objcore.executeQuery(q);
                                }
                                else
                                {
                                    DataSet ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objcore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    //idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    q = "insert into InventoryConsumed (id,branchid,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + idcnsmd + "','" + branchidd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    objcore.executeQuery(q);
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
        private void button1_Click(object sender, EventArgs e)
        {
            
            getbranchtype();
            button1.Enabled = false;
            button2.Enabled = false;
          //  completewaste();
            setinventory();
            productionrecipe();
            MessageBox.Show("Updated Successfully");
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getbranchtype();
            button1.Enabled = false;
            button2.Enabled = false;
            completewaste();
            MessageBox.Show("Updated Successfully");
            button1.Enabled = true;
            button2.Enabled = true;
        }
    }
}
