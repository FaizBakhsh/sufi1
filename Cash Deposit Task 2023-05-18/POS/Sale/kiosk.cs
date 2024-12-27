using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class kiosk : Form
    {
        public kiosk()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

        public string date = "";
        private void kiosk_Load(object sender, EventArgs e)
        {
            bind(); 
        }
        protected void bind()
        {
            dataGridView1.DataSource = null;
            string q = "select Id as Bill_No,NetBill from sale where billstatus='Kiosk Pending' and date='" + date + "'";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            dataGridView1.DataSource = ds.Tables[0];
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void recipie(string itmid, float itmqnty, string flid, string date, string rawid)
        {
            DataSet ds = new DataSet();
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";
                if (flid == "" || flid == "0")
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0')";
                }
                else
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
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
                                dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    if (inventryqty > 0)
                                    {
                                        rem = inventryqty;
                                    }
                                    q = "update InventoryConsumed set RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty - amounttodeduct) + "',uploadstatus='Pending' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                //else
                                //{
                                //    ds = new DataSet();
                                //    int idcnsmd = 0;
                                //    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                //    if (ds.Tables[0].Rows.Count > 0)
                                //    {
                                //        string ii = ds.Tables[0].Rows[0][0].ToString();
                                //        if (ii == string.Empty)
                                //        {
                                //            ii = "0";
                                //        }
                                //        idcnsmd = Convert.ToInt32(ii) + 1;
                                //    }
                                //    else
                                //    {
                                //        idcnsmd = Convert.ToInt32("1");
                                //    }
                                //    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                //    q = "insert into InventoryConsumed (Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                //    objCore.executeQuery(q);
                                //}

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
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                    if (dsminus.Tables[0].Rows.Count > 0)
                                    {
                                        double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                        double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                        if (inventryqty > 0)
                                        {
                                            rem = inventryqty;
                                        }
                                        q = "update InventoryConsumed set RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty - amounttodeduct) + "',uploadstatus='Pending' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                        objCore.executeQuery(q);
                                    }
                                    else
                                    {
                                        
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
        public void recipiemodifierruntime(string itmid, float itmqnty, string date, string rawid)
        {
            DataSet ds = new DataSet();
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";// "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "'";
                // q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Modifier.MenuItemId, dbo.Modifier.RawItemId, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Modifier.Id='" + itmid + "'";
                q = "SELECT     dbo.RuntimeModifier.RawItemId AS RawItemId, dbo.RuntimeModifier.Quantity, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM FROM         dbo.RuntimeModifier INNER JOIN                      dbo.RawItem ON dbo.RuntimeModifier.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RuntimeModifier.Id='" + itmid + "'";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        if (rawid == "")
                        {
                            float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                            double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                            double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                            double amounttodeduct = (qnty / convrate) * recipiqnty;
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
                            dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                            if (dsminus.Tables[0].Rows.Count > 0)
                            {
                                double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                                q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty - amounttodeduct) + "',uploadstatus='Pending' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                objCore.executeQuery(q);
                            }
                            else
                            {
                                
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
                                dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                                    q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty - amounttodeduct) + "',uploadstatus='Pending' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                   
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
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public static string branchtype = "";
        protected void getbranchtype()
        {
            try
            {
                string q = "select type from branch";
                DataSet dsb = new DataSet();
                dsb = objCore.funGetDataSet(q);
                if (dsb.Tables[0].Rows.Count > 0)
                {
                    branchtype = dsb.Tables[0].Rows[0][0].ToString();
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
                q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Modifier.RawItemId, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Modifier.Id='" + itmid + "'";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        float qnty = Convert.ToInt32(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        amounttodeduct = Math.Round(amounttodeduct, 3);
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;

                        dsminus = new DataSet();
                        dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                            q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {

                            
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void setinventory(string saleid)
        {
            DataSet ds1 = new System.Data.DataSet();
            string q = "";// "delete from InventoryConsumed where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";


            q = "select id,date from sale where id = '" + saleid + "' order by date";
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
                                    recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, ds1.Tables[0].Rows[i]["date"].ToString(), "");
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
                            recipiemodifierruntime(ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString(), "");
                        }
                    }
                }

            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var senderGrid = (DataGridView)sender;

                if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
                {
                    string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString();
                    if (e.ColumnIndex == 0)
                    {
                        string q = "update sale set billstatus='Pending' where id='" + id + "'";
                        objcore.executeQuery(q);
                        q = "update Saledetails set Orderstatus='Pending',OrderStatusmain='Pending' where saleid='" + id + "'";
                        objcore.executeQuery(q);
                        bind(); 
                    }
                    if (e.ColumnIndex == 1)
                    {
                        string q = "update sale set billstatus='Cancelled' where id='" + id + "'";
                        objcore.executeQuery(q);
                        q = "update Saledetails set Orderstatus='Cancelled',OrderStatusmain='Cancelled' where saleid='" + id + "'";
                        objcore.executeQuery(q);
                      //  setinventory(id);
                        bind(); 
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
