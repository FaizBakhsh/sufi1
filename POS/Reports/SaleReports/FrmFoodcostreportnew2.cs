using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmFoodcostreportnew2 : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        string branchtype = "";
        public FrmFoodcostreportnew2()
        {
            InitializeComponent();
            branchtype = getbranchtype();
        }
        public string start = "", end = "", branchid = "", reference = "";
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
                DataSet ds1 = new DataSet();
                string q = "select id,name from menugroup order by name";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["name"] = "All";
                ds1.Tables[0].Rows.Add(dr1);
                cmbgroup.DataSource = ds1.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "name";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            if (reference == "pnl")
            {
                dateTimePicker1.Text = start;
                dateTimePicker2.Text = end;
                if (branchid == "All")
                {
                    cmbbranch.SelectedItem = branchid;
                }
                else
                {
                    cmbbranch.SelectedValue = branchid;
                }
               
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterParent;
                bindreportmenuitem();
            }
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
       
        static string type = "";
        public void bindreportmenuitem()
        {
            type = "menu";
            
            POSRestaurant.Reports.SaleReports.rptfoodcost rptDoc = new Reports.SaleReports.rptfoodcost();
            POSRestaurant.Reports.SaleReports.dsfoodcost ds = new Reports.SaleReports.dsfoodcost();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            dt.TableName = "DataTable1";
            dt = getAllOrdersmenuitem();
            DataView dv = new DataView(dt);
            dv.Sort = "name, SaleValue";
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
                ds.DataTable1.Merge(dv.ToTable());
            }
            rptDoc.SetDataSource(ds);
            if (checkBox1.Checked == true)
            {
                wastage();
            }
            rptDoc.SetParameterValue("waste", waste1);
            rptDoc.SetParameterValue("variance", variance1);
            rptDoc.SetParameterValue("compwaste", compwst1);
            rptDoc.SetParameterValue("Comp", company);
            rptDoc.SetParameterValue("phone", phone);
            rptDoc.SetParameterValue("Address", address);
            rptDoc.SetParameterValue("date", "for the period of  " + dateTimePicker1.Text + " to " + dateTimePicker2.Text);
            crystalReportViewer1.ReportSource = rptDoc;
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public double opening(string itemid)
        {
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "SELECT top 1 date, (remaining) as rem FROM     discard where Date <'" + date + "' and itemid='" + itemid + "' order by Date desc";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                date2 = dspurchase.Tables[0].Rows[0][0].ToString();
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                closing = Convert.ToDouble(val);
            }
            if (date2 == "")
            {
                date2 = date;
            }
            DateTime end = Convert.ToDateTime(date);
            DateTime start = Convert.ToDateTime(date2);
            TimeSpan ts = Convert.ToDateTime(date) - Convert.ToDateTime(date2);
            int days = ts.Days;
            if (days <= 1)
            {
                return closing;
            }
            start = start.AddDays(1);
            end = end.AddDays(-1);
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            dspurchase = new DataSet();
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                purchased = Convert.ToDouble(val);
            }

            val = "";
            purchased = Math.Round(purchased, 2);

            dspurchase = new DataSet();
            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                consumed = Convert.ToDouble(val);
            }
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferin = Convert.ToDouble(val);
            }
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferout = Convert.ToDouble(val);
            }


            val = "";
            double rate = 0;
            DataSet dscon = new DataSet();
            q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + itemid + "'";
            dscon = objcore.funGetDataSet(q);
            if (dscon.Tables[0].Rows.Count > 0)
            {
                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
            }
            if (rate > 0)
            {
                consumed = consumed / rate;
            }
            double qty = 0;
            qty = purchased - consumed;
            dspurchase = new DataSet();
            //q = "SELECT     SUM(variance) AS Expr1 FROM     Variance where Date <'" + date + "' and itemid='" + itemid + "'";
            //dspurchase = objcore.funGetDataSet(q);
            //if (dspurchase.Tables[0].Rows.Count > 0)
            //{
            //    val = dspurchase.Tables[0].Rows[0][0].ToString();
            //    if (val == "")
            //    {
            //        val = "0";
            //    }
            //    variance = Convert.ToDouble(val);
            //}
            ////if (variance > 0)
            //{
            //    qty = qty + (variance);
            //}
            dspurchase = new DataSet();
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                discard = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                staff = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][2].ToString();
                if (val == "")
                {
                    val = "0";
                }
                complt = Convert.ToDouble(val);
            }
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (closing + purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        double waste1 = 0, variance1 = 0, compwst1 = 0;
        protected void wastage()
        {
            string date = dateTimePicker1.Text;

            try
            {
                waste1 = 0; variance1 = 0; compwst1 = 0;
                string q = "select id, itemname from rawitem where status is null or status='Active' order by id";
                DataSet dsitem = new DataSet();
                dsitem = objCore.funGetDataSet(q);
                for (int i = 0; i < dsitem.Tables[0].Rows.Count; i++)
                {
                    try
                    {
                        double prc = getprice(dsitem.Tables[0].Rows[i]["id"].ToString());
                        q = "select sum(staff) as staff ,sum(Discard) as discard,sum(completewaste) as completewaste from discard where date between '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "' and itemid='" + dsitem.Tables[0].Rows[i]["id"] + "'";
                        DataSet dswaste = new DataSet();
                        dswaste = objCore.funGetDataSet(q);
                        if (dswaste.Tables[0].Rows.Count > 0)
                        {


                            string temp1 = dswaste.Tables[0].Rows[0]["staff"].ToString();
                            if (temp1 == "")
                            {
                                temp1 = "0";
                            }
                            waste1 = waste1 + (Convert.ToDouble(temp1) * prc);

                            double rate = 0;
                            DataSet dscon = new DataSet();
                            q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsitem.Tables[0].Rows[i]["id"].ToString() + "'";
                            dscon = objcore.funGetDataSet(q);
                            if (dscon.Tables[0].Rows.Count > 0)
                            {
                                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                            }
                            temp1 = dswaste.Tables[0].Rows[0]["completewaste"].ToString();

                            if (temp1 == "")
                            {
                                temp1 = "0";
                            }
                            double val = 0;
                            if (rate > 0)
                            {
                                val = Convert.ToDouble(temp1) / rate;
                            }

                            compwst1 = compwst1 + (val * prc);

                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }
            }
            catch (Exception ex)
            {

            }

        }
        public double openingclosing(string itemid, string date, double closing)
        {
            string date2 = dateTimePicker2.Text;


            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "SELECT top 1 date, (remaining) as rem FROM     discard where Date <'" + date + "' and itemid='" + itemid + "' order by Date desc";

            DateTime end = Convert.ToDateTime(date2);
            DateTime start = Convert.ToDateTime(date);
            start = start.AddDays(1);
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";
            dspurchase = new DataSet();
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                purchased = Convert.ToDouble(val);
            }

            val = "";
            purchased = Math.Round(purchased, 2);

            dspurchase = new DataSet();
            q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                consumed = Convert.ToDouble(val);
            }
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferin = Convert.ToDouble(val);
            }
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dsin = objcore.funGetDataSet(q);
            if (dsin.Tables[0].Rows.Count > 0)
            {
                val = dsin.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                transferout = Convert.ToDouble(val);
            }


            val = "";
            double rate = 0;
            DataSet dscon = new DataSet();
            q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + itemid + "'";
            dscon = objcore.funGetDataSet(q);
            if (dscon.Tables[0].Rows.Count > 0)
            {
                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
            }
            if (rate > 0)
            {
                consumed = consumed / rate;
            }
            double qty = 0;
            qty = purchased - consumed;
            dspurchase = new DataSet();
            //q = "SELECT     SUM(variance) AS Expr1 FROM     Variance where Date <'" + date + "' and itemid='" + itemid + "'";
            //dspurchase = objcore.funGetDataSet(q);
            //if (dspurchase.Tables[0].Rows.Count > 0)
            //{
            //    val = dspurchase.Tables[0].Rows[0][0].ToString();
            //    if (val == "")
            //    {
            //        val = "0";
            //    }
            //    variance = Convert.ToDouble(val);
            //}
            ////if (variance > 0)
            //{
            //    qty = qty + (variance);
            //}
            dspurchase = new DataSet();
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                discard = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                staff = Convert.ToDouble(val);
                val = dspurchase.Tables[0].Rows[0][2].ToString();
                if (val == "")
                {
                    val = "0";
                }
                complt = Convert.ToDouble(val);
            }
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        public double attachMenucost(string itmid, string flvid, float itmqnty, string type,string recipetype)
        {
            double totalcost = 0;
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
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
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        double cost = 0;
                        string menuid = dsrecipie.Tables[0].Rows[i]["attachmenuid"].ToString();
                        string flid = dsrecipie.Tables[0].Rows[i]["attachFlavourid"].ToString();
                        if (flid == "")
                        {
                            flid = "0";
                        }
                        double qty = float.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        if (recipetype == "Packing")
                        {
                            //cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, menuid, flid, "0", "0", "Both", "Packing");
                            cost = fillcost(Convert.ToInt32(menuid), Convert.ToInt32(flid), 0, 0, branchtype, "Packing");
                   
                        }
                        else
                        {
                            //cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, menuid, flid, "0", "0", "Both", "");
                            cost = fillcost(Convert.ToInt32(menuid), Convert.ToInt32(flid), 0, 0, branchtype, "");
                        }
                       
                        cost = cost * qty;
                        

                        try
                        {
                            if (recipetype == "Packing")
                            {
                                cost = cost + getsubitemcost(menuid, flid, qty, "Packing", type);
                            }
                            else
                            {
                                cost = cost + getsubitemcost(menuid, flid, qty, "", type);
                            }
                           
                        }
                        catch (Exception ex)
                        {

                        }
                        totalcost = totalcost + cost;

                      
                    }
                }
                totalcost = totalcost * itmqnty;
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

            return totalcost;
        }
        List<RawItemClass> rawitemsClass = new List<RawItemClass>();
        List<PriceClass> pricesClass = new List<PriceClass>();
        List<RecipeClass> recipesClass = new List<RecipeClass>();
        List<RuntimemodifierClass> runtimeModifiers = new List<RuntimemodifierClass>();
        List<RuntimemodifierClass> Modifiers = new List<RuntimemodifierClass>();
        public void fillrecipes()
        {
            string q = "SELECT     Id, MenuItemId, RawItemId, UOMCId, Quantity, Cost, uploadstatus, branchid,cast( modifierid as varchar(100)) as modifierid, type, attachmenuid FROM            Recipe";
            DataSet dsservice = new DataSet();
            dsservice = objCore.funGetDataSet(q);
            try
            {

                IList<RecipeClass> data = dsservice.Tables[0].AsEnumerable().Select(row =>
                    new RecipeClass
                    {
                        MenuItemId = row.Field<int>("MenuItemId"),
                        RawItemId = row.Field<int>("RawItemId"),
                        ModifierId = row.Field<string>("modifierid"),
                        Quantity = row.Field<double>("Quantity"),
                        type = row.Field<string>("type")

                    }).ToList();
                recipesClass = data.ToList();

            }
            catch (Exception ex)
            {


            }
            q = "SELECT       id, name, menuItemid, price, Quantity, status, branchid, kdsid, uploadStatus, rawitemid, type, baseimage, stage, quantityallowed, Necessary, GrossPrice, imageurl, SubQty, kdsid2 FROM            RuntimeModifier";
            dsservice = new DataSet();
            dsservice = objCore.funGetDataSet(q);
            try
            {

                IList<RuntimemodifierClass> data = dsservice.Tables[0].AsEnumerable().Select(row =>
                    new RuntimemodifierClass
                    {
                        Id = row.Field<int>("id"),
                        RawItemId = row.Field<int>("RawItemId"),
                        Name = row.Field<string>("name"),
                        Quantity = row.Field<double>("Quantity")

                    }).ToList();
                runtimeModifiers = data.ToList();

            }
            catch (Exception ex)
            {


            }
            q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head, GrossPrice, imageurl, kdsid2 FROM            Modifier";
            dsservice = new DataSet();
            dsservice = objCore.funGetDataSet(q);
            try
            {

                IList<RuntimemodifierClass> data = dsservice.Tables[0].AsEnumerable().Select(row =>
                    new RuntimemodifierClass
                    {
                        Id = row.Field<int>("id"),
                        RawItemId = row.Field<int>("RawItemId"),
                        Name = row.Field<string>("name"),
                        Quantity = row.Field<double>("Quantity")

                    }).ToList();
                Modifiers = data.ToList();

            }
            catch (Exception ex)
            {


            }
        }
        public void fillRawItems()
        {
            string q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.Type.TypeName AS Type,cast( dbo.UOMConversion.ConversionRate as float) as ConversionRate FROM            dbo.RawItem INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id INNER JOIN                         dbo.UOMConversion ON dbo.RawItem.UOMId = dbo.UOMConversion.UOMId ";
            DataSet dsservice = new DataSet();
            dsservice = objCore.funGetDataSet(q);
            try
            {

                IList<RawItemClass> data = dsservice.Tables[0].AsEnumerable().Select(row =>
                    new RawItemClass
                    {
                        Id = row.Field<int>("Id"),
                        Name = row.Field<string>("ItemName"),
                        Type = row.Field<string>("Type"),
                        conversionrate = row.Field<double>("ConversionRate")

                    }).ToList();
                rawitemsClass = data.ToList();

            }
            catch (Exception ex)
            {


            }
        }
        public void fillRawItemPrices(int id)
        {
            int count = 0;
            try
            {
                count = pricesClass.Where(x => x.ItemId == id).ToList().Count;
            }
            catch (Exception ex)
            {
                
            }
            if (count == 0)
            {
                double price = getcostprice(id.ToString());
                pricesClass.Add(new PriceClass { ItemId = id, Price = price });
            }
           
        }
        protected double fillcost(int menuitemid,int flavourid,int runtimemodifier,int modifierid,string ordertype,string type)
        {
            double cost = 0;

            try
            {
                if (modifierid > 0)
                {
                    int itemid = Modifiers.Where(x => x.Id == modifierid).ToList()[0].RawItemId;
                    fillRawItemPrices(itemid);

                    double qty = Modifiers.Where(x => x.Id == modifierid).ToList()[0].Quantity;
                    double conversion = 1;
                    try
                    {
                        conversion = rawitemsClass.Where(x => x.Id == itemid).ToList()[0].conversionrate;
                    }
                    catch (Exception ex)
                    {

                    }
                    qty = qty / conversion;
                    double price = 0;
                    try
                    {
                        price = pricesClass.Where(x => x.ItemId == itemid).ToList()[0].Price;
                        cost = cost + (price * qty);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (runtimemodifier > 0)
                {
                    int itemid = runtimeModifiers.Where(x => x.Id == runtimemodifier).ToList()[0].RawItemId;
                    fillRawItemPrices(itemid);
                    double qty = runtimeModifiers.Where(x => x.Id == runtimemodifier).ToList()[0].Quantity;
                    double conversion = 1;
                    try
                    {
                        conversion = rawitemsClass.Where(x => x.Id == itemid).ToList()[0].conversionrate;
                    }
                    catch (Exception ex)
                    {

                    }
                    qty = qty / conversion;
                    double price = 0;
                    try
                    {
                        price = pricesClass.Where(x => x.ItemId == itemid).ToList()[0].Price;
                        cost = cost + (price * qty);
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else if (flavourid > 0)
                {
                    List<RecipeClass> recipefiltered = new List<RecipeClass>();

                    try
                    {
                        recipefiltered = recipesClass.Where(x => x.MenuItemId == menuitemid && x.ModifierId == flavourid.ToString()).ToList();



                    }
                    catch (Exception ex)
                    {

                    }
                    foreach (var item in recipefiltered)
                    {

                        double qty = item.Quantity;
                        double conversion = 1;
                        try
                        {
                            conversion = rawitemsClass.Where(x => x.Id == item.RawItemId).ToList()[0].conversionrate;
                        }
                        catch (Exception ex)
                        {

                        }
                        qty = qty / conversion;
                        double price = 0;
                        fillRawItemPrices(item.RawItemId);
                        try
                        {
                            if (type == "Packing")
                            {
                                if (rawitemsClass.Where(x => x.Id == item.RawItemId).ToList()[0].Type.ToLower() == "packing")
                                {
                                    try
                                    {
                                        price = pricesClass.Where(x => x.ItemId == item.RawItemId).ToList()[0].Price;
                                        cost = cost + (price * qty);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }
                            }
                            else
                            {
                                if (rawitemsClass.Where(x => x.Id == item.RawItemId).ToList()[0].Type.ToLower() != "packing")
                                {
                                    try
                                    {
                                        price = pricesClass.Where(x => x.ItemId == item.RawItemId).ToList()[0].Price;
                                        cost = cost + (price * qty);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }

                            }
                        }
                        catch (Exception w)
                        {
                            
                            
                        }


                    }
                }
                else
                {
                    List<RecipeClass> recipefiltered = new List<RecipeClass>();

                    try
                    {
                        recipefiltered = recipesClass.Where(x => x.MenuItemId == menuitemid).ToList();



                    }
                    catch (Exception ex)
                    {

                    }
                    foreach (var item in recipefiltered)
                    {

                        try
                        {
                            double qty = item.Quantity;
                            double conversion = 1;
                            try
                            {
                                conversion = rawitemsClass.Where(x => x.Id == item.RawItemId).ToList()[0].conversionrate;
                            }
                            catch (Exception ex)
                            {

                            }
                            qty = qty / conversion;
                            double price = 0;
                            fillRawItemPrices(item.RawItemId);
                            try
                            {
                                if (type == "Packing")
                                {
                                    if (rawitemsClass.Where(x => x.Id == item.RawItemId).ToList()[0].Type.ToLower() == "packing")
                                    {
                                        try
                                        {
                                            price = pricesClass.Where(x => x.ItemId == item.RawItemId).ToList()[0].Price;
                                            cost = cost + (price * qty);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                }
                                else
                                {
                                    if (rawitemsClass.Where(x => x.Id == item.RawItemId).ToList()[0].Type.ToLower() != "packing")
                                    {
                                        try
                                        {
                                            price = pricesClass.Where(x => x.ItemId == item.RawItemId).ToList()[0].Price;
                                            cost = cost + (price * qty);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }

                                }
                            }
                            catch (Exception ex)
                            {
                                
                            }
                        }
                        catch (Exception ex)
                        {

                        }


                    }
                }
            }
            catch (Exception ex)
            {
               
            }

            return cost;
        }
        public DataTable getAllOrdersmenuitem()
        {
            if (branchtype == "")
            {
               branchtype= getbranchtype();
            }
            string q = "";
            DataTable dtrptmenu = new DataTable();
            try
            {
                dtrptmenu.Columns.Add("group", typeof(string));
                dtrptmenu.Columns.Add("name", typeof(string));
                dtrptmenu.Columns.Add("price", typeof(double));
                dtrptmenu.Columns.Add("qty", typeof(double));
                dtrptmenu.Columns.Add("SaleValue", typeof(double));
                dtrptmenu.Columns.Add("FoodCost", typeof(double));
                dtrptmenu.Columns.Add("PackingCost", typeof(double));
                dtrptmenu.Columns.Add("GrossProfit", typeof(double));
                dtrptmenu.Columns.Add("ProfetPerc", typeof(double));
                dtrptmenu.Columns.Add("logo", typeof(byte[]));
                dtrptmenu.Columns.Add("CostPerc", typeof(double));
                dtrptmenu.Columns.Add("Target", typeof(double));
                DataSet ds = new DataSet();
                
                getcompany();
                string logo = "";
                try
                {
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();

                }
                catch (Exception ex)
                {
                }
                SqlConnection connection = new SqlConnection(POSRestaurant.Properties.Settings.Default.ConnectionString);
                SqlCommand command;
                if (cmbgroup.Text == "All")
                {
                    if (textBox1.Text.Length > 0)
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) and dbo.MenuItem.Name like '%"+textBox1.Text+"%' GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
             
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
             
                    }
                }
                else
                {
                    if (textBox1.Text.Length > 0)
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) and dbo.MenuItem.menugroupid='"+cmbgroup.SelectedValue+"' and dbo.MenuItem.Name like '%" + textBox1.Text + "%'  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
             
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0)  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "' AND dbo.MenuItem.Id >708 GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
             
                    }
                }
                  command = new SqlCommand(q, connection);
                  connection.Open();
                  SqlDataReader dr = command.ExecuteReader();
                //command = new SqlCommand("GetFoodCost", connection);
                //command.CommandType = System.Data.CommandType.StoredProcedure;
                //command.Parameters.Add(new SqlParameter("@date1", dateTimePicker1.Text));
                //command.Parameters.Add(new SqlParameter("@date2", dateTimePicker2.Text));
                //command.Parameters.Add(new SqlParameter("@branchid", cmbbranch.SelectedValue));
                //command.Parameters.Add(new SqlParameter("@type", "menu"));
               

                while (dr.Read())
                {
                    if (dr["Name"].ToString().ToLower().Contains("meal for 3") || dr["Name"].ToString().ToLower().Contains("zinger"))
                    {
                        string na = dr["Name"].ToString();
                    }

                    string size = dr["Flavour"].ToString();
                    if (size.Length > 0)
                    {
                        size = size + " '";
                    }
                    double perc = 0, totalsale = 0;

                    //double cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, dr["id"].ToString(), dr["Flavourid"].ToString(), dr["RunTimeModifierId"].ToString(), dr["ModifierId"].ToString(), branchtype, "");
                    double cost = fillcost(Convert.ToInt32(dr["id"].ToString()), Convert.ToInt32(dr["Flavourid"].ToString()), Convert.ToInt32(dr["RunTimeModifierId"].ToString()), Convert.ToInt32(dr["ModifierId"].ToString()), branchtype, "");
                   
                    cost = cost * Convert.ToDouble(dr["qty"].ToString());
                    double pcost = fillcost(Convert.ToInt32(dr["id"].ToString()), Convert.ToInt32(dr["Flavourid"].ToString()), Convert.ToInt32(dr["RunTimeModifierId"].ToString()), Convert.ToInt32(dr["ModifierId"].ToString()), branchtype, "Packing");
                    pcost = pcost * Convert.ToDouble(dr["qty"].ToString());
                    try
                    {
                        cost = cost + attachMenucost(dr["id"].ToString(), dr["Flavourid"].ToString(), float.Parse(dr["qty"].ToString()), "MenuItem", "MenuItem");
                        pcost = pcost + attachMenucost(dr["id"].ToString(), dr["Flavourid"].ToString(), float.Parse(dr["qty"].ToString()), "MenuItem", "Packing");
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        cost = cost + getsubitemcost(dr["id"].ToString(), dr["Flavourid"].ToString(), Convert.ToDouble(dr["qty"].ToString()), "", "MenuItem");
                        pcost = pcost + getsubitemcost(dr["id"].ToString(), dr["Flavourid"].ToString(), Convert.ToDouble(dr["qty"].ToString()), "Packing", "MenuItem");
                    }
                    catch (Exception ex)
                    {
                       
                    }
                    
                    q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') and dbo.saledetails.menuitemid='" + dr["Id"] + "' and dbo.saledetails.runtimemodifierid>0 GROUP BY  dbo.Saledetails.RunTimeModifierId";
                    DataSet dsr = new DataSet();
                    dsr = objcore.funGetDataSet(q);
                    for (int i = 0; i < dsr.Tables[0].Rows.Count;i++ )
                    {
                        //double costr = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, dr["id"].ToString(), "0", dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "0", branchtype, "");
                        double costr = fillcost(Convert.ToInt32(dr["id"].ToString()), 0, Convert.ToInt32(dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString()), 0, branchtype, "");
                   
                        costr = costr * Convert.ToDouble(dsr.Tables[0].Rows[i]["qty"].ToString());
                        //double pcostr = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, dr["id"].ToString(), "0", dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "0", branchtype, "Packing");
                        double pcostr = fillcost(Convert.ToInt32(dr["id"].ToString()), 0, Convert.ToInt32(dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString()), 0, branchtype, "Packing");
                   
                        pcostr = pcostr * Convert.ToDouble(dsr.Tables[0].Rows[i]["qty"].ToString());
                        cost = cost + costr;
                        pcost = pcost + pcostr;

                        try
                        {
                            cost = cost + attachMenucost(dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "", float.Parse(dsr.Tables[0].Rows[i]["qty"].ToString()), "RuntimeModifier", "RuntimeModifier");
                            pcost = pcost + attachMenucost(dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "", float.Parse(dsr.Tables[0].Rows[i]["qty"].ToString()), "RuntimeModifier", "Packing");
                        }
                        catch (Exception ex)
                        {

                        }

                        try
                        {
                            cost = cost + getsubitemcost(dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "", Convert.ToDouble(dsr.Tables[0].Rows[i]["qty"].ToString()), "", "RuntimeModifier");
                            pcost = pcost + getsubitemcost(dsr.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "", Convert.ToDouble(dsr.Tables[0].Rows[i]["qty"].ToString()), "Packing", "RuntimeModifier");
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                    string temp1 = dr["mprice"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double mprice = Convert.ToDouble(temp1);

                    temp1 = dr["fprice"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double fprice = Convert.ToDouble(temp1);
                    temp1 = dr["menuprice"].ToString();

                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                   // double fprice = Convert.ToDouble(temp1);
                    //fprice = fprice / Convert.ToDouble(dr["qty"].ToString());
                    temp1 = dr["target"].ToString();
                   // mprice = 0;
                    temp1 = dr["target"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double target = Convert.ToDouble(temp1);

                    if (mprice > 0)
                    {
                        target = target * mprice;
                    }
                    if (fprice > 0)
                    {
                        target = target * fprice;
                    }
                    TimeSpan ts = Convert.ToDateTime(dateTimePicker2.Text) - (Convert.ToDateTime(dateTimePicker1.Text));
                    try
                    {
                        int days = ts.Days;
                        days = days + 1;
                        target = target * days;

                    }
                    catch (Exception ex)
                    {

                        target = 0;
                    }
                    temp1 = dr["price"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double svalue = Convert.ToDouble(temp1);
                    string name = "";
                    //if (dr["RModifier"].ToString().ToString() != "")
                    //{
                    //    name =dr["Name"].ToString()+" "+ dr["RModifier"].ToString();
                    //}
                    //else if (dr["Modifier"].ToString().ToString() != "")
                    //{
                    //    name = dr["Name"].ToString() + " " + dr["Modifier"].ToString();
                    //}
                    //else
                    {
                        name = dr["Name"].ToString();
                    }
                    double svalue1 = svalue;
                    if (svalue1 == 0)
                    {
                        svalue1 = 1;

                    }
                    if (logo == "")
                    {
                        dtrptmenu.Rows.Add(dr["groupname"].ToString(), size + name, Math.Round(mprice + fprice, 3), Convert.ToDouble(dr["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue1) * 100, null, (((cost + pcost)) / svalue1) * 100, target);
                    }
                    else
                    {
                        dtrptmenu.Rows.Add(dr["groupname"].ToString(), size + name, Math.Round(mprice + fprice, 3), Convert.ToDouble(dr["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue1) * 100, dscompany.Tables[0].Rows[0]["logo"], (((cost + pcost)) / svalue1) * 100, target);
                    }
                }
                connection.Close();

                try
                {
                    q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.RuntimeModifier.name AS RModifier,                          dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Sale.Date BETWEEN @date1 AND @date2) AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = @branchid) AND (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.RuntimeModifier.name, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price";

                    if (cmbgroup.Text == "All")
                    {
                        if (textBox1.Text.Length > 0)
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) and dbo.MenuItem.Name like '%" + textBox1.Text + "%' GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.RuntimeModifier.name AS RModifier,                          dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker1.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.RuntimeModifier.name, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price";

                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.RuntimeModifier.name AS RModifier,                          dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker1.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.RuntimeModifier.name, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price";

                        }
                    }
                    else
                    {
                        if (textBox1.Text.Length > 0)
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "' and dbo.MenuItem.Name like '%" + textBox1.Text + "%'  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.RuntimeModifier.name AS RModifier,                          dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker1.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "')  and dbo.MenuItem.Name like '%" + textBox1.Text + "%'  AND (dbo.Menuitem.menugroupid = '" + cmbgroup.SelectedValue + "')  AND (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.RuntimeModifier.name, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price";

                        }
                        else
                        {
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0)  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                            q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.RuntimeModifier.name AS RModifier,                          dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker1.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (dbo.Menuitem.menugroupid = '" + cmbgroup.SelectedValue + "')  AND (dbo.Saledetails.RunTimeModifierId > 0) AND (dbo.Saledetails.ModifierId = 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.RuntimeModifier.name, dbo.Saledetails.RunTimeModifierId, dbo.RuntimeModifier.price";

                        }
                    }
                    command = new SqlCommand(q, connection);
                    connection.Open();

                    dr = command.ExecuteReader();


                    //command = new SqlCommand("GetFoodCost", connection);
                    //command.CommandType = System.Data.CommandType.StoredProcedure;
                    //command.Parameters.Add(new SqlParameter("@date1", dateTimePicker1.Text));
                    //command.Parameters.Add(new SqlParameter("@date2", dateTimePicker2.Text));
                    //command.Parameters.Add(new SqlParameter("@branchid", cmbbranch.SelectedValue));
                    //command.Parameters.Add(new SqlParameter("@type", "rmodifier"));
                    //connection.Open();
                    //dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        //string size = dr["Flavour"].ToString();
                        //if (size.Length > 0)
                        //{
                        //    size = size + " '";
                        //}
                        double perc = 0, totalsale = 0;

                        double cost = 0;// getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, dr["id"].ToString(), dr["Flavourid"].ToString(), dr["RunTimeModifierId"].ToString(), dr["ModifierId"].ToString(), branchtype, "");
                        // cost = cost * Convert.ToDouble(dr["qty"].ToString());
                        double pcost = 0;// getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, dr["id"].ToString(), dr["Flavourid"].ToString(), dr["RunTimeModifierId"].ToString(), dr["ModifierId"].ToString(), branchtype, "Packing");
                        //pcost = pcost * Convert.ToDouble(dr["qty"].ToString());


                        string temp1 = dr["mprice"].ToString();
                        if (temp1 == "")
                        {
                            temp1 = "0";
                        }
                        double mprice = Convert.ToDouble(temp1);

                        // temp1 = dr["fprice"].ToString();

                        temp1 = dr["menuprice"].ToString();

                        if (temp1 == "")
                        {
                            temp1 = "0";
                        }
                        double fprice = Convert.ToDouble(temp1);
                        fprice = fprice / Convert.ToDouble(dr["qty"].ToString());
                        temp1 = "";// dr["target"].ToString();
                        mprice = 0;
                        temp1 = "";// dr["target"].ToString();
                        if (temp1 == "")
                        {
                            temp1 = "0";
                        }
                        double target = Convert.ToDouble(temp1);

                        if (mprice > 0)
                        {
                            target = target * mprice;
                        }
                        if (fprice > 0)
                        {
                            target = target * fprice;
                        }
                        TimeSpan ts = Convert.ToDateTime(dateTimePicker2.Text) - (Convert.ToDateTime(dateTimePicker1.Text));
                        try
                        {
                            int days = ts.Days;
                            days = days + 1;
                            target = target * days;

                        }
                        catch (Exception ex)
                        {

                            target = 0;
                        }
                        temp1 = dr["price"].ToString();
                        if (temp1 == "")
                        {
                            temp1 = "0";
                        }
                        double svalue = Convert.ToDouble(temp1);
                        string name = "";

                        name = dr["RModifier"].ToString();


                        double svalue1 = svalue;
                        if (svalue1 == 0)
                        {
                            svalue1 = 1;

                        }
                        if (logo == "")
                        {
                            dtrptmenu.Rows.Add("Modifiers", name, Math.Round(mprice + fprice, 2), Convert.ToDouble(dr["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue1) * 100, null, (((cost + pcost)) / svalue1) * 100, target);
                        }
                        else
                        {
                            dtrptmenu.Rows.Add("Modifiers", name, Math.Round(mprice + fprice, 2), Convert.ToDouble(dr["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue1) * 100, dscompany.Tables[0].Rows[0]["logo"], (((cost + pcost)) / svalue1) * 100, target);
                        }
                    }
                }
                catch (Exception vv)
                {
                    
                }
                connection.Close();

                q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.Modifier.Name AS Modifier,                          dbo.Saledetails.ModifierId, dbo.Modifier.Price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.Date BETWEEN @date1 AND @date2) AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = @branchid) AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId > 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.Modifier.Name, dbo.Saledetails.ModifierId, dbo.Modifier.Price";
                if (cmbgroup.Text == "All")
                {
                    if (textBox1.Text.Length > 0)
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) and dbo.MenuItem.Name like '%" + textBox1.Text + "%' GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.Modifier.Name AS Modifier,                          dbo.Saledetails.ModifierId, dbo.Modifier.Price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and menuitem.name like '%" + textBox1.Text + "%' AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId > 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.Modifier.Name, dbo.Saledetails.ModifierId, dbo.Modifier.Price";
               
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.Modifier.Name AS Modifier,                          dbo.Saledetails.ModifierId, dbo.Modifier.Price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId > 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.Modifier.Name, dbo.Saledetails.ModifierId, dbo.Modifier.Price";
               
                    }
                }
                else
                {
                    if (textBox1.Text.Length > 0)
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0) and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "' and dbo.MenuItem.Name like '%" + textBox1.Text + "%'  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.Modifier.Name AS Modifier,                          dbo.Saledetails.ModifierId, dbo.Modifier.Price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and menuitem.name like '%" + textBox1.Text + "%' and menuitem.menugroupid='" + cmbgroup.SelectedValue + "' AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId > 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.Modifier.Name, dbo.Saledetails.ModifierId, dbo.Modifier.Price";
               
                    }
                    else
                    {
                        q = "SELECT        SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.MenuItem.Price AS mprice,                          dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name, dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid, dbo.ModifierFlavour.name AS Flavour,                          dbo.MenuGroup.Name AS Groupname, dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = " + cmbbranch.SelectedValue + ") AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId = 0)  and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "'  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price, dbo.MenuItem.Target,                          dbo.Saledetails.ModifierId, dbo.Saledetails.RunTimeModifierId";
                        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) AS menuprice, dbo.Modifier.Name AS Modifier,                          dbo.Saledetails.ModifierId, dbo.Modifier.Price AS mprice FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "') and menuitem.menugroupid='" + cmbgroup.SelectedValue + "'  AND (dbo.Saledetails.RunTimeModifierId = 0) AND (dbo.Saledetails.ModifierId > 0) AND                          (dbo.Saledetails.Price > 0) GROUP BY dbo.Modifier.Name, dbo.Saledetails.ModifierId, dbo.Modifier.Price";
               
                    }
                }
                
                command = new SqlCommand(q, connection);
                connection.Open();
               
                dr = command.ExecuteReader();
                
                //command = new SqlCommand("GetFoodCost", connection);
                //command.CommandType = System.Data.CommandType.StoredProcedure;
                //command.Parameters.Add(new SqlParameter("@date1", dateTimePicker1.Text));
                //command.Parameters.Add(new SqlParameter("@date2", dateTimePicker2.Text));
                //command.Parameters.Add(new SqlParameter("@branchid", cmbbranch.SelectedValue));
                //command.Parameters.Add(new SqlParameter("@type", "modifier"));
                //connection.Open();
                //dr = command.ExecuteReader();

                while (dr.Read())
                {
                   
                    double perc = 0, totalsale = 0;
                   
                   // double cost =  getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, "0", "0", "0", dr["ModifierId"].ToString(), branchtype, "");
                    double cost = fillcost(0, 0, 0, Convert.ToInt32(dr["ModifierId"].ToString()), branchtype, "");
                   
                    cost = cost * Convert.ToDouble(dr["qty"].ToString());
                    double pcost = 0;// getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, "0", "0", "0", dr["ModifierId"].ToString(), branchtype, "Packing");
                   // pcost = pcost * Convert.ToDouble(dr["qty"].ToString());


                    string temp1 = dr["mprice"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double mprice = Convert.ToDouble(temp1);

                   

                    temp1 = dr["menuprice"].ToString();

                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double fprice = Convert.ToDouble(temp1);
                    fprice = fprice / Convert.ToDouble(dr["qty"].ToString());
                    
                    mprice = 0;
                    temp1 = "";// dr["target"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double target = Convert.ToDouble(temp1);

                    if (mprice > 0)
                    {
                        target = target * mprice;
                    }
                    if (fprice > 0)
                    {
                        target = target * fprice;
                    }
                    TimeSpan ts = Convert.ToDateTime(dateTimePicker2.Text) - (Convert.ToDateTime(dateTimePicker1.Text));
                    try
                    {
                        int days = ts.Days;
                        days = days + 1;
                        target = target * days;

                    }
                    catch (Exception ex)
                    {

                        target = 0;
                    }
                    temp1 = dr["price"].ToString();
                    if (temp1 == "")
                    {
                        temp1 = "0";
                    }
                    double svalue = Convert.ToDouble(temp1);
                    string name = "";

                    name = dr["Modifier"].ToString();
                    
                    double svalue1 = svalue;
                    if (svalue1 == 0)
                    {
                        svalue1 = 1;

                    }
                    if (logo == "")
                    {
                        dtrptmenu.Rows.Add("Modifiers",  name, Math.Round(mprice + fprice, 2), Convert.ToDouble(dr["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue1) * 100, null, (((cost + pcost)) / svalue1) * 100, target);
                    }
                    else
                    {
                        dtrptmenu.Rows.Add("Modifiers",  name, Math.Round(mprice + fprice, 2), Convert.ToDouble(dr["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue1) * 100, dscompany.Tables[0].Rows[0]["logo"], (((cost + pcost)) / svalue1) * 100, target);
                    }
                }
                #region MyRegion
                //{
                //    if (cmbbranch.Text == "All")
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) as menuprice, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid')  and  (dbo.Sale.Customer != 'Delivery')  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price>0  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target";
                //    }
                //    else
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) as menuprice, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid')  and  (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "')  and  (dbo.Sale.Customer != 'Delivery')  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price>0  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target";
                //    }
                //    ds = objCore.funGetDataSet(q);
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        if (ds.Tables[0].Rows[i]["Name"].ToString().Contains("Crispy Chicken Burger") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("fish burger double") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("xprs xl burger"))
                //        {
                //            string na = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }

                //        string size = ds.Tables[0].Rows[i]["Expr1"].ToString();
                //        if (size.Length > 0)
                //        {
                //            size = size.Substring(0, 1) + " '";
                //        }
                //        double perc = 0, totalsale = 0;
                //        DataSet dsperc = new DataSet();
                //        double cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", "0", branchtype, "");
                //        cost = cost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                //        double pcost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", "0", branchtype, "Packing");
                //        pcost = pcost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());

                //        //double cost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "", "", "", "","yes");
                //        //double pcost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "", "Packing", "", "", "yes");
                //        ////perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                //        string temp1 = ds.Tables[0].Rows[i]["mprice"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double mprice = Convert.ToDouble(temp1);

                //        temp1 = ds.Tables[0].Rows[i]["fprice"].ToString();

                //        temp1 = ds.Tables[0].Rows[i]["menuprice"].ToString();

                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double fprice = Convert.ToDouble(temp1);
                //        fprice = fprice / Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                //        temp1 = ds.Tables[0].Rows[i]["target"].ToString();
                //        mprice = 0;
                //        temp1 = ds.Tables[0].Rows[i]["target"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double target = Convert.ToDouble(temp1);

                //        if (mprice > 0)
                //        {
                //            target = target * mprice;
                //        }
                //        if (fprice > 0)
                //        {
                //            target = target * fprice;
                //        }
                //        TimeSpan ts=Convert.ToDateTime(dateTimePicker2.Text)-(Convert.ToDateTime(dateTimePicker1.Text));
                //        try
                //        {
                //            int days=ts.Days;
                //            days = days + 1;
                //            target = target * days;

                //        }
                //        catch (Exception ex)
                //        {

                //            target = 0;
                //        }
                //        temp1 = ds.Tables[0].Rows[i]["price"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double svalue = Convert.ToDouble(temp1);

                //        if (logo == "")
                //        {
                //            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["groupname"].ToString(), size + " " + ds.Tables[0].Rows[i]["Name"].ToString(), Math.Round(mprice + fprice, 2), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue) * 100, null, (((cost + pcost)) / svalue) * 100,target);
                //        }
                //        else
                //        {
                //            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["groupname"].ToString(), size + " " + ds.Tables[0].Rows[i]["Name"].ToString(), Math.Round(mprice + fprice, 2), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue) * 100, dscompany.Tables[0].Rows[0]["logo"], (((cost + pcost)) / svalue) * 100, target);
                //        }
                //    }


                //    //Delivery

                //    ds = new DataSet();
                //    if (cmbbranch.Text == "All")
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) as menuprice, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid') and  (dbo.Sale.Customer = 'Delivery') and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price>0  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target";
                //    }
                //    else
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, SUM(dbo.Saledetails.Price) as menuprice, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid')  and  (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"') and  (dbo.Sale.Customer = 'Delivery') and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price>0  GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target";
                //    }
                //    ds = objCore.funGetDataSet(q);
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        if (ds.Tables[0].Rows[i]["Name"].ToString().Contains("Crispy Chicken Burger Meal") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("fish burger double") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("xprs xl burger"))
                //        {
                //            string na = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }
                //        string size = ds.Tables[0].Rows[i]["Expr1"].ToString();
                //        if (size.Length > 0)
                //        {
                //            size = size.Substring(0, 1) + " '";
                //        }
                //        double perc = 0, totalsale = 0;
                //        DataSet dsperc = new DataSet();

                //        double cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", "0", branchtype, "");
                //        cost = cost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                //        double pcost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", "0", branchtype, "Packing");
                //        pcost = pcost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());


                //        //double cost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "", "", "", "Delivery","yes");
                //        //double pcost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "", "Packing", "", "Delivery","yes");
                //        //perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                //        string temp1 = ds.Tables[0].Rows[i]["mprice"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double mprice = Convert.ToDouble(temp1);
                //        temp1 = ds.Tables[0].Rows[i]["fprice"].ToString();
                //        temp1 = ds.Tables[0].Rows[i]["menuprice"].ToString();

                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double fprice = Convert.ToDouble(temp1);
                //        fprice = fprice / Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                //        mprice = 0;

                //        temp1 = ds.Tables[0].Rows[i]["target"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double target = Convert.ToDouble(temp1);

                //        if (mprice > 0)
                //        {
                //            target = target * mprice;
                //        }
                //        if (fprice > 0)
                //        {
                //            target = target * fprice;
                //        }
                //        TimeSpan ts = Convert.ToDateTime(dateTimePicker2.Text) - (Convert.ToDateTime(dateTimePicker1.Text));
                //        try
                //        {
                //            int days = ts.Days;
                //            days = days + 1;
                //            target = target * days;

                //        }
                //        catch (Exception ex)
                //        {

                //            target = 0;
                //        }
                //        temp1 = ds.Tables[0].Rows[i]["price"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double svalue = Convert.ToDouble(temp1);

                //        if (logo == "")
                //        {
                //            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["groupname"].ToString(), size + " " + ds.Tables[0].Rows[i]["Name"].ToString()+"(D)", Math.Round(mprice + fprice, 2), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue) * 100, null, (((cost + pcost)) / svalue) * 100, target);
                //        }
                //        else
                //        {
                //            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["groupname"].ToString(), size + " " + ds.Tables[0].Rows[i]["Name"].ToString() + "(D)", Math.Round(mprice + fprice, 2), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), ((svalue - (cost + pcost)) / svalue) * 100, dscompany.Tables[0].Rows[0]["logo"], (((cost + pcost)) / svalue) * 100, target);
                //        }
                //    }
                //    ds = new DataSet();
                //    if (cmbbranch.Text == "All")
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid') and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price=0 GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target";
                //    }
                //    else
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.MenuItem.Name,dbo.MenuItem.Target, dbo.MenuItem.Id, dbo.MenuGroup.Id AS mid, dbo.Saledetails.Flavourid,                          dbo.ModifierFlavour.name AS Expr1, dbo.MenuGroup.Name AS Groupname FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id WHERE    (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and  (dbo.Sale.BillStatus = 'Paid')  and  (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"') and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.RunTimeModifierId=0 and dbo.Saledetails.Price=0 GROUP BY dbo.MenuItem.Name, dbo.ModifierFlavour.name, dbo.MenuItem.Id, dbo.MenuGroup.Id, dbo.Saledetails.Flavourid, dbo.MenuGroup.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.price,dbo.MenuItem.Target";

                //    }
                //       ds = objCore.funGetDataSet(q);
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        if (ds.Tables[0].Rows[i]["Name"].ToString().Contains("Crispy Chicken Burger Meal") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("fish burger double") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("xprs xl burger"))
                //        {
                //            string na = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }
                //        string size = ds.Tables[0].Rows[i]["Expr1"].ToString();
                //        if (size.Length > 0)
                //        {
                //            size = size.Substring(0, 1) + " '";
                //        }
                //        double perc = 0, totalsale = 0;
                //        DataSet dsperc = new DataSet();


                //        double cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", "0", branchtype, "");
                //        cost = cost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                //        double pcost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", "0", branchtype, "Packing");
                //        pcost = pcost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());


                //        //double cost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "", "", "0", "","no");
                //        //double pcost = getcostmenu(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "", "Packing", "0", "","no");
                //        ////perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                //        string temp1 = ds.Tables[0].Rows[i]["mprice"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double mprice = Convert.ToDouble(temp1);
                //        temp1 = ds.Tables[0].Rows[i]["fprice"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double fprice = Convert.ToDouble(temp1);

                //        temp1 = ds.Tables[0].Rows[i]["target"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double target = Convert.ToDouble(temp1);

                //        if (mprice > 0)
                //        {
                //            target = target * mprice;
                //        }
                //        if (fprice > 0)
                //        {
                //            target = target * fprice;
                //        }
                //        TimeSpan ts = Convert.ToDateTime(dateTimePicker2.Text) - (Convert.ToDateTime(dateTimePicker1.Text));
                //        try
                //        {
                //            int days = ts.Days;
                //            days = days + 1;
                //            target = target * days;

                //        }
                //        catch (Exception ex)
                //        {

                //            target = 0;
                //        }
                //        temp1 = ds.Tables[0].Rows[i]["price"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double svalue = Convert.ToDouble(temp1);
                //        double perc1 = (((Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()) * mprice) - (cost + pcost)) / (Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()) * mprice)) * 100;
                //        if (svalue == 0)
                //        {
                //            perc1 = 0;
                //        }
                //        double perc2 = (((cost + pcost)) / svalue) * 100;
                //        if (svalue == 0)
                //        {
                //            perc2 = 100;
                //        }

                //        if (logo == "")
                //        {
                //            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["groupname"].ToString(), size + " " + ds.Tables[0].Rows[i]["Name"].ToString()+"(Free)", Math.Round(mprice + fprice, 2), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), perc1, null, perc2, target);
                //        }
                //        else
                //        {
                //            dtrptmenu.Rows.Add(ds.Tables[0].Rows[i]["groupname"].ToString(), size + " " + ds.Tables[0].Rows[i]["Name"].ToString() + "(Free)", Math.Round(mprice + fprice, 2), Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), perc1, dscompany.Tables[0].Rows[0]["logo"], perc2, target);
                //        }
                //    }
                //}
                //{                    
                //    ds = new DataSet();
                //    if (cmbbranch.Text == "All")
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId > 0)  AND (dbo.Saledetails.Price > 0)  GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price ORDER BY dbo.Modifier.Name";
                //    }
                //    else
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"') AND (dbo.Saledetails.ModifierId > 0)  AND (dbo.Saledetails.Price > 0)  GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price ORDER BY dbo.Modifier.Name";
                //    }
                //    ds = objCore.funGetDataSet(q);
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        if (ds.Tables[0].Rows[i]["Name"].ToString().Contains("Crispy Chicken Burger Meal") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("fish burger double") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("xprs xl burger"))
                //        {
                //            string na = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }
                //        double perc = 0, totalsale = 0;
                //        DataSet dsperc = new DataSet();
                //        string temp1 = ds.Tables[0].Rows[i]["Expr1"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double mprice = Convert.ToDouble(temp1);

                //        double cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", ds.Tables[0].Rows[i]["ModifierId"].ToString(), branchtype, "");
                //        cost = cost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                //        double pcost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), "0", ds.Tables[0].Rows[i]["ModifierId"].ToString(), branchtype, "Packing");
                //        pcost = pcost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());


                //        //double cost = getcostmenu(ds.Tables[0].Rows[i]["ModifierId"].ToString(), "0", "modifier", "", "", "","no");
                //        //double pcost = getcostmenu(ds.Tables[0].Rows[i]["ModifierId"].ToString(), "0", "modifier", "Packing", "", "","no");
                //        temp1 = ds.Tables[0].Rows[i]["price"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double svalue = Convert.ToDouble(temp1);
                //        double perc1 = (((Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()) * mprice) - (cost + pcost)) / (Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()) * mprice)) * 100;
                //        if (svalue == 0)
                //        {
                //            perc1 = 0;
                //        }
                //        double perc2 = (((cost + pcost)) / svalue) * 100;
                //        if (svalue == 0)
                //        {
                //            perc2 = 100;
                //        }
                //        //perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                //        if (logo == "")
                //        {
                //            dtrptmenu.Rows.Add("Modifiers", ds.Tables[0].Rows[i]["Name"].ToString(), mprice, Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), perc1, null,perc2,0);
                //        }
                //        else
                //        {
                //            dtrptmenu.Rows.Add("Modifiers", ds.Tables[0].Rows[i]["Name"].ToString(), mprice, Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), perc1, dscompany.Tables[0].Rows[0]["logo"] ,perc2,0);
                //        }
                //    }

                //    ds = new DataSet();
                //    q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price AS Expr1 FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id WHERE        (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.ModifierId > 0)  AND (dbo.Saledetails.Price > 0)  GROUP BY dbo.Saledetails.ModifierId, dbo.Modifier.Name, dbo.Modifier.Price ORDER BY dbo.Modifier.Name";
                //    if (cmbbranch.Text == "All")
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.RuntimeModifier.name, dbo.RuntimeModifier.price AS Expr1,                          dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE       (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND     (dbo.Sale.BillStatus = 'Paid') AND (dbo.Saledetails.Price > 0) AND (dbo.Saledetails.RunTimeModifierId > 0) GROUP BY dbo.RuntimeModifier.name, dbo.RuntimeModifier.price, dbo.Saledetails.RunTimeModifierId";
                //    }
                //    else
                //    {
                //        q = "SELECT        TOP (100) PERCENT SUM(dbo.Saledetails.Price - dbo.Saledetails.Itemdiscount) AS price, SUM(dbo.Saledetails.Quantity) AS qty, dbo.RuntimeModifier.name, dbo.RuntimeModifier.price AS Expr1,                          dbo.Saledetails.RunTimeModifierId FROM            dbo.Saledetails INNER JOIN                         dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id WHERE       (dbo.Sale.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') AND     (dbo.Sale.BillStatus = 'Paid') AND  (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"') AND (dbo.Saledetails.Price > 0) AND (dbo.Saledetails.RunTimeModifierId > 0) GROUP BY dbo.RuntimeModifier.name, dbo.RuntimeModifier.price, dbo.Saledetails.RunTimeModifierId";
                //    }
                //    ds = objCore.funGetDataSet(q);
                //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //    {
                //        if (ds.Tables[0].Rows[i]["Name"].ToString().Contains("Crispy Chicken Burger Meal") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("fish burger double") || ds.Tables[0].Rows[i]["Name"].ToString().Contains("xprs xl burger"))
                //        {
                //            string na = ds.Tables[0].Rows[i]["Name"].ToString();
                //        }
                //        double perc = 0, totalsale = 0;
                //        DataSet dsperc = new DataSet();
                //        string temp1 = ds.Tables[0].Rows[i]["Expr1"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double mprice = Convert.ToDouble(temp1);

                //        double cost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(), ds.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "0", branchtype, "");
                //        cost = cost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());
                //        double pcost = getcostmenu1(dateTimePicker1.Text, dateTimePicker2.Text, ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Flavourid"].ToString(),  ds.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "0", branchtype, "Packing");
                //        pcost = pcost * Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString());




                //        //double cost = 0;// getcostmenu(ds.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "0", "rmodifier", "", "", "");
                //        //double pcost = 0;// getcostmenu(ds.Tables[0].Rows[i]["RunTimeModifierId"].ToString(), "0", "rmodifier", "Packing", "", "");
                //        temp1 = ds.Tables[0].Rows[i]["price"].ToString();
                //        if (temp1 == "")
                //        {
                //            temp1 = "0";
                //        }
                //        double svalue = Convert.ToDouble(temp1);
                //        double perc1 = (((Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()) * mprice) - (cost + pcost)) / (Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()) * mprice)) * 100;
                //        if (svalue == 0)
                //        {
                //            perc1 = 0;
                //        }
                //        double perc2 = (((cost + pcost)) / svalue) * 100;
                //        if (svalue == 0)
                //        {
                //            perc2 = 100;
                //        }
                //        //perc = Math.Round((Convert.ToDouble(ds.Tables[0].Rows[i]["price"].ToString()) / totalsale) * 100, 2);
                //        if (logo == "")
                //        {
                //            dtrptmenu.Rows.Add("Modifiers", ds.Tables[0].Rows[i]["Name"].ToString(), mprice, Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), perc1, null,perc2, 0);
                //        }
                //        else
                //        {
                //            dtrptmenu.Rows.Add("Modifiers", ds.Tables[0].Rows[i]["Name"].ToString(), mprice, Convert.ToDouble(ds.Tables[0].Rows[i]["qty"].ToString()), svalue, cost, pcost, svalue - (cost + pcost), perc1, dscompany.Tables[0].Rows[0]["logo"],perc2, 0);
                //        }
                //    }

                //} 
                #endregion
               
                
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
        public double getcostprice(string id)
        {
           
            double cost = 0;

            string q = "select  dbo.Getprice('" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "'," + id + ")";
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
        protected double getsubitemcost(string itemid, string flavourid, double qty, string type, string menutype)
        {
            double totalamount = 0;
            string q = "";
            try
            {
                if (type.ToLower() == "packing")
                {
                    if (flavourid == "" || flavourid == "0")
                    {
                        q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.type, dbo.SubItems.Name AS SubItem,                          dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName, dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate, dbo.Type.TypeName FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id where dbo.AttachRecipe.Type='" + menutype + "' and dbo.AttachRecipe.Menuitemid='" + itemid + "' AND (dbo.Type.TypeName = 'Packing')";
                    }
                    else
                    {
                        q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.type, dbo.SubItems.Name AS SubItem,                          dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName, dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate, dbo.Type.TypeName FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.AttachRecipe.Type='" + menutype + "' and dbo.AttachRecipe.Menuitemid='" + itemid + "' AND (dbo.Type.TypeName = 'Packing') and dbo.AttachRecipe.FlavourId='" + flavourid + "'";
                    }
                }
                else
                {
                    if (flavourid == "" || flavourid == "0")
                    {
                        q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.type, dbo.SubItems.Name AS SubItem,                          dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName, dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate, dbo.Type.TypeName FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id where dbo.AttachRecipe.Type='" + menutype + "' and dbo.AttachRecipe.Menuitemid='" + itemid + "'  AND (dbo.Type.TypeName != 'Packing')";
                    }
                    else
                    {
                        q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.type, dbo.SubItems.Name AS SubItem,                          dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName, dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate, dbo.Type.TypeName FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.AttachRecipe.Type='" + menutype + "' and dbo.AttachRecipe.Menuitemid='" + itemid + "' and dbo.AttachRecipe.FlavourId='" + flavourid + "'  AND (dbo.Type.TypeName != 'Packing')";
                    }
                }
                DataSet dssubitem = new DataSet();
                dssubitem = objCore.funGetDataSet(q);
                for (int h = 0; h < dssubitem.Tables[0].Rows.Count; h++)
                {
                    string price = getcostprice(dssubitem.Tables[0].Rows[h]["RawItemId"].ToString()).ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    string convs = dssubitem.Tables[0].Rows[h]["ConversionRate"].ToString();
                    if (convs == "")
                    {
                        convs = "1";
                    }
                    string qt = dssubitem.Tables[0].Rows[h]["Quantity"].ToString();
                    if (qt == "")
                    {
                        qt = "0";
                    }
                    double quantity = Convert.ToDouble(qt);
                    qt = dssubitem.Tables[0].Rows[h]["SubRecipeQuantity"].ToString();
                    if (qt == "")
                    {
                        qt = "0";
                    }
                    quantity = quantity * Convert.ToDouble(qt);

                    double amount = 0;
                    try
                    {
                        amount = Convert.ToDouble(price);
                    }
                    catch (Exception ex)
                    {


                    }
                    try
                    {
                        amount = amount / Convert.ToDouble(convs);
                    }
                    catch (Exception ex)
                    {


                    }
                    amount = amount * quantity;

                    amount = amount * qty;
                    totalamount = totalamount + amount;

                }
            }
            catch (Exception ex)
            {


            }
            return totalamount;
        }
        //private double getprice(string id)
        //{
        //    if (id == "69")
        //    {

        //    }
        //    double variance = 0, price = 0;
        //    string val = "";
        //    DataSet dspurchase = new DataSet();
        //    string q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";
        //    dspurchase = objCore.funGetDataSet(q);
        //    if (dspurchase.Tables[0].Rows.Count > 0)
        //    {
        //        val = dspurchase.Tables[0].Rows[0][0].ToString();
        //        if (val == "")
        //        {
        //            val = "0";
        //        }
        //        price = Convert.ToDouble(val);
        //    }
        //    if (price == 0)
        //    {
        //        dspurchase = new DataSet();
        //        q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";
        //        dspurchase = objCore.funGetDataSet(q);
        //        if (dspurchase.Tables[0].Rows.Count > 0)
        //        {
        //            val = dspurchase.Tables[0].Rows[0][0].ToString();
        //            if (val == "")
        //            {
        //                val = "0";
        //            }
        //            price = Convert.ToDouble(val);
        //        }
        //    }
        //    if (price == 0)
        //    {
        //        dspurchase = new DataSet();
        //        q = "select price from rawitem where id='"+id+"'";
        //        dspurchase = objCore.funGetDataSet(q);
        //        if (dspurchase.Tables[0].Rows.Count > 0)
        //        {
        //            try
        //            {
        //                val = dspurchase.Tables[0].Rows[0][0].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                price = Convert.ToDouble(val);
        //            }
        //            catch (Exception ez)
        //            {
                        
                       
        //            }
        //        }
        //    }
        //    return price;
        //}

        //private double getprice(string id)
        //{

        //    double variance = 0, price = 0;
        //    string val = "";
        //    DataTable ds = new DataTable();
        //    DataSet dspurchase = new DataSet();
        //    string q = "SELECT     AVG(DISTINCT dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";
        //    q = "SELECT   MONTH(date),year(date), avg (dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE  ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'  group by MONTH(date), year(date) order by MONTH(date) desc,year(date) desc";
        //    dspurchase = objCore.funGetDataSet(q);
        //    for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
        //    {
        //        val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
        //        if (val == "")
        //        {
        //            val = "0";
        //        }
        //        price = price + Convert.ToDouble(val);
        //    }
        //    if (dspurchase.Tables[0].Rows.Count > 0)
        //    {
        //        price = price / dspurchase.Tables[0].Rows.Count;
        //    }

        //    if (price == 0)
        //    {

        //        q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE      ( date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and itemId = '" + id + "'   and (price > 0) GROUP BY MONTH(Date), YEAR(Date) order by MONTH(date) desc,year(date) desc";
        //        dspurchase = new DataSet();
        //        dspurchase = objCore.funGetDataSet(q);
        //        for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
        //        {
        //            val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
        //            if (val == "")
        //            {
        //                val = "0";
        //            }
        //            price = price + Convert.ToDouble(val);
        //        }
        //        if (dspurchase.Tables[0].Rows.Count > 0)
        //        {
        //            price = price / dspurchase.Tables[0].Rows.Count;
        //        }
        //    }


        //    if (price == 0)
        //    {
        //        dspurchase = new DataSet();
        //        q = "SELECT     top 1 (dbo.PurchaseDetails.TotalAmount / dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";
        //        q = "SELECT  top 1 MONTH(date),year(date), avg (dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE  ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "'  group by MONTH(date), year(date) order by MONTH(date) desc,year(date) desc";

        //        dspurchase = objCore.funGetDataSet(q);
        //        for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
        //        {
        //            val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
        //            if (val == "")
        //            {
        //                val = "0";
        //            }
        //            price = price + Convert.ToDouble(val);
        //        }
        //        if (dspurchase.Tables[0].Rows.Count > 0)
        //        {
        //            price = price / dspurchase.Tables[0].Rows.Count;
        //        }
        //    }
        //    if (price == 0)
        //    {
        //        try
        //        {
        //            dspurchase = new DataSet();
        //            q = "select price from rawitem where id='" + id + "'";
        //            dspurchase = objCore.funGetDataSet(q);
        //            if (dspurchase.Tables[0].Rows.Count > 0)
        //            {
        //                val = dspurchase.Tables[0].Rows[0][0].ToString();
        //                if (val == "")
        //                {
        //                    val = "0";
        //                }
        //                price = Convert.ToDouble(val);
        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //    }
        //    return price;
        //}
        private double getprice(string id)
        {
            if (id == "69")
            {

            }
            double variance = 0, price = 0;
            string val = "";
            DataSet dspurchase = new DataSet();
            string q = "";

            q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.InventoryTransfer.ItemId = '" + id + "'  GROUP BY MONTH(Date), YEAR(Date)";

            dspurchase = objcore.funGetDataSet(q);
            for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
            {
                val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                price = price + Convert.ToDouble(val);
            }
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                price = price / dspurchase.Tables[0].Rows.Count;
            }
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT    top 1    (price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date <= '" + dateTimePicker1.Text + "' ) and dbo.InventoryTransfer.ItemId = '" + id + "'  order by date desc ";

                dspurchase = objcore.funGetDataSet(q);
                for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
                {
                    val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = price + Convert.ToDouble(val);
                }

            }
            if (price == 0)
            {
                dspurchase = new DataSet();

                q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'";

                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    price = Convert.ToDouble(val);
                }
                if (price == 0)
                {
                    dspurchase = new DataSet();

                    q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date < '" + dateTimePicker1.Text + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";

                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        val = dspurchase.Tables[0].Rows[0][0].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        price = Convert.ToDouble(val);
                    }
                }
                if (price == 0)
                {
                    dspurchase = new DataSet();
                    q = "select price from rawitem where id='" + id + "'";
                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            val = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            price = Convert.ToDouble(val);
                        }
                        catch (Exception ez)
                        {


                        }
                    }
                }
            }
            return price;
        }
        public double getcost(string id, string type)
        {
            double cost = 0, totalqty = 0;

            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            if (type == "modifier")
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.Saledetails.ModifierId>0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head FROM            Modifier where id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "'";

                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }

                }
            }
            else
            {
                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid , dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "' and dbo.Saledetails.ModifierId=0 GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid, dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.ModifierId";
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if(rmodid=="")
                    {
                        rmodid="0";
                    }
                    string modid = dscons.Tables[0].Rows[i]["ModifierId"].ToString();
                    if(modid=="")
                    {
                        modid="0";
                    }

                    if (modid != "0")
                    {
                        q = "SELECT RawItemId, Quantity from Modifier where id='0' ";
                    }
                    else
                    {
                        if (rmodid != "0")
                        {
                            q = "SELECT RawItemId, Quantity from RuntimeModifier where id='" + rmodid + "' ";
                        }
                        else
                        {
                            if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                            }
                            else
                            {
                                q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";
                            }
                        }
                    }
                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }
            }

            return cost;
        }
        protected string getbranchtype()
        {
            string branchtype = "";
            try
            {
                string q = "select type from branch where id='" + cmbbranch.SelectedValue + "'";
                DataSet dsb = new DataSet();
                dsb = objcore.funGetDataSet(q);
                if (dsb.Tables[0].Rows.Count > 0)
                {
                    branchtype = dsb.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            return branchtype;
        }
        public double getcostmenu1(string start,string end,string id,string flid, string rid,string mid,string btype,string packingtype)
        {
            double cost = 0;
            if (flid == "")
            {
                flid = "0";
            }
            string q = "select  dbo.getcost3(" + id + ",'" + start + "','" + end + "'," + flid + "," + rid + "," + mid + ",'" + btype + "','" + packingtype + "')";
            try
            {
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
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
        public double getcostmenu(string id, string mid, string type, string cat, string prcqty,string order,string chkrm)
        {
            double cost = 0, totalqty = 0;
           string bid = cmbbranch.Text;
            string branchtype = getbranchtype();
            string q = "";// "SELECT     dbo.Saledetails.Quantity AS qty, dbo.MenuGroup.Name, dbo.Recipe.Quantity, dbo.Recipe.RawItemId, dbo.Recipe.modifierid FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                      dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id LEFT OUTER JOIN                      dbo.Recipe ON dbo.MenuItem.Id = dbo.Recipe.MenuItemId AND dbo.Saledetails.Flavourid = dbo.Recipe.modifierid WHERE     (dbo.Sale.Date BETWEEN '" + dateTimePicker2.Text + "' AND '" + dateTimePicker2.Text + "') and dbo.MenuGroup.id='" + id + "'";
            if (type == "modifier")
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.ModifierId ='" + id + "'   AND (dbo.Sale.BillStatus = 'Paid')   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.ModifierId ='" + id + "'   AND (dbo.Sale.BillStatus = 'Paid')  AND (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"')   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.ModifierId";
               
                }
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    q = "SELECT        Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid, menugroupid, Head FROM            Modifier where id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "'";
                    if (cat.ToLower() == "packing")
                    {
                        q = "SELECT        dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.Modifier.uploadstatus, dbo.Modifier.branchid, dbo.Modifier.kdsid, dbo.Modifier.menugroupid, dbo.Modifier.Head,                          dbo.Type.TypeName FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId RIGHT OUTER JOIN                         dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId where dbo.Modifier.id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "' and dbo.Type.TypeName='Packing'";
                    }
                    else
                    {
                        q = "SELECT        dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Name, dbo.Modifier.Price, dbo.Modifier.Quantity, dbo.Modifier.uploadstatus, dbo.Modifier.branchid, dbo.Modifier.kdsid, dbo.Modifier.menugroupid, dbo.Modifier.Head,                          dbo.Type.TypeName FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId RIGHT OUTER JOIN                         dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId where dbo.Modifier.id='" + dscons.Tables[0].Rows[i]["ModifierId"].ToString() + "' and dbo.Type.TypeName!='Packing'";
                    
                    }

                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }
            }
            else if (type == "rmodifier")
            {
                if (cmbbranch.Text == "All")
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.RunTimeModifierId ='" + id + "'  and dbo.Saledetails.ModifierId =0   AND (dbo.Sale.BillStatus = 'Paid')  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId";
                }
                else
                {
                    q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "') and  dbo.Saledetails.RunTimeModifierId ='" + id + "'  and  dbo.Sale.branchid ='" + cmbbranch.SelectedValue + "'  and dbo.Saledetails.ModifierId =0   AND (dbo.Sale.BillStatus = 'Paid')  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.RunTimeModifierId";
                }
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    double qty = Convert.ToDouble(temp);
                    q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'";
                    if (cat.ToLower() == "packing")
                    {
                        q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'  and dbo.Type.TypeName='Packing'";
                    }
                    else
                    {
                        q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString() + "'  and dbo.Type.TypeName!='Packing'";
                    }
                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["rawitemid"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }
            }
            else
            {

                if (prcqty == "0")
                {
                    if (cmbbranch.Text == "All")
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price=0   AND (dbo.Sale.BillStatus = 'Paid')   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                    }
                    else
                    {
                        q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price=0   AND (dbo.Sale.BillStatus = 'Paid')   AND (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"')   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                    }
                }
                else
                {
                    if (order.ToLower() == "delivery")
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer='Delivery' and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price>0  AND (dbo.Sale.BillStatus = 'Paid')    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer='Delivery' and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price>0  AND (dbo.Sale.BillStatus = 'Paid')  AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "')    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer !='Delivery'  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price>0   AND (dbo.Sale.BillStatus = 'Paid')  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                        }
                        else
                        {
                            q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer !='Delivery'  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price>0   AND (dbo.Sale.BillStatus = 'Paid')  AND (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"')  GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                        }

                    }
                }
                
                DataSet dscons = new DataSet();
                dscons = objCore.funGetDataSet(q);
                for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                {
                    string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }

                    string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                    if (rmodid == "")
                    {
                        rmodid = "0";
                    }

                    double qty = Convert.ToDouble(temp);
                    if (rmodid == "0")
                    {
                        if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                        {
                           // q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                            if (cat.ToLower() == "packing")
                            {
                                if (bid == "All")
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName='Packing'";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName='Packing' and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                }
                            }
                            else
                            {
                                if (bid == "All")
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName!='Packing'";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                }
                            }
                        }
                        else
                        {
                            if (cat.ToLower() == "packing")
                            {
                                if (bid == "All")
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'  and dbo.Type.TypeName='Packing'";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'  and dbo.Type.TypeName='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                }
                            }
                            else
                            {
                                if (bid == "All")
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' and dbo.Type.TypeName!='Packing'";
                                }
                                else
                                {
                                    q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'   and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                }
                            }

                        }
                    }
                    else
                    {
                       // q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + rmodid + "'";

                        if (cat.ToLower() == "packing")
                        {
                            q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName='Packing'";
                        }
                        else
                        {
                            q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName!='Packing'";
                        }


                    }
                    DataSet dsrecipyqty = new DataSet();
                    dsrecipyqty = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                    {
                        temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        double recipeqty = Convert.ToDouble(temp);

                        double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                        totalqty = (recipeqty * qty);
                        double rate = 0;
                        DataSet dscon = new DataSet();
                        q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                        dscon = objCore.funGetDataSet(q);
                        if (dscon.Tables[0].Rows.Count > 0)
                        {
                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                        }
                        if (rate > 0)
                        {
                            rate = totalqty / rate;
                        }
                        double amount = prc * rate;

                        amount = Math.Round(amount, 3);
                        cost = cost + amount;
                    }
                }




                try
                {
                    if (prcqty != "0" && chkrm == "yes")
                    {


                        if (order.ToLower() == "delivery")
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer='Delivery' and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price=0  AND (dbo.Sale.BillStatus = 'Paid')  and dbo.Saledetails.RunTimeModifierId>0    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer='Delivery' and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price=0  AND (dbo.Sale.BillStatus = 'Paid')  AND (dbo.Sale.branchid = '" + cmbbranch.SelectedValue + "')  and dbo.Saledetails.RunTimeModifierId>0    GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                            }
                        }
                        else
                        {
                            if (cmbbranch.Text == "All")
                            {
                                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer !='Delivery'  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price=0  AND (dbo.Sale.BillStatus = 'Paid')  and dbo.Saledetails.RunTimeModifierId>0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                            }
                            else
                            {
                                q = "SELECT     SUM(dbo.Saledetails.Quantity) AS Quantity, dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id WHERE      (dbo.Sale.Date BETWEEN '" + dateTimePicker1.Text + "' AND '" + dateTimePicker2.Text + "')  and dbo.sale.customer !='Delivery'  and dbo.MenuItem.id='" + id + "' and dbo.Saledetails.Flavourid='" + mid + "'  and dbo.Saledetails.ModifierId=0  and dbo.Saledetails.price=0  AND (dbo.Sale.BillStatus = 'Paid')  AND (dbo.Sale.branchid = '"+cmbbranch.SelectedValue+"')  and dbo.Saledetails.RunTimeModifierId>0   GROUP BY dbo.Saledetails.MenuItemId, dbo.Saledetails.Flavourid,dbo.Saledetails.RunTimeModifierId";
                            }
                        }
                    }
                    else
                    {
                        q = "";
                    }

                    dscons = new DataSet();
                    dscons = objCore.funGetDataSet(q);
                    for (int i = 0; i < dscons.Tables[0].Rows.Count; i++)
                    {
                        string temp = dscons.Tables[0].Rows[i]["Quantity"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }

                        string rmodid = dscons.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                        if (rmodid == "")
                        {
                            rmodid = "0";
                        }

                        double qty = Convert.ToDouble(temp);
                        if (rmodid == "0")
                        {
                            if (dscons.Tables[0].Rows[i]["Flavourid"].ToString() == "0")
                            {
                                // q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' ";
                                if (cat.ToLower() == "packing")
                                {
                                    if (bid == "All")
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName='Packing'";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                    }
                                }
                                else
                                {
                                    if (bid == "All")
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and dbo.Type.TypeName!='Packing'";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                    }
                                }
                            }
                            else
                            {
                                // q = "SELECT RawItemId, Quantity from Recipe where MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "' and modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' ";

                                if (cat.ToLower() == "packing")
                                {
                                    if (bid == "All")
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'  and dbo.Type.TypeName='Packing'";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'    and dbo.Type.TypeName='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                    }
                                }
                                else
                                {
                                    if (bid == "All")
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "' and dbo.Type.TypeName!='Packing'";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.Recipe.RawItemId, dbo.Recipe.Quantity, dbo.Type.TypeName FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where dbo.Recipe.MenuItemId='" + dscons.Tables[0].Rows[i]["MenuItemId"].ToString() + "'  and dbo.Recipe.modifierid='" + dscons.Tables[0].Rows[i]["Flavourid"].ToString() + "'    and dbo.Type.TypeName!='Packing'  and  (dbo.Recipe.type='" + branchtype + "' or dbo.Recipe.type='Both')";
                                    }
                                }

                            }
                        }
                        else
                        {
                            // q = "SELECT   id, name, menuItemid,rawitemid, price, Quantity, status, branchid, kdsid, uploadStatus  FROM            RuntimeModifier where id='" + rmodid + "'";

                            if (cat.ToLower() == "packing")
                            {
                                q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName='Packing'";
                            }
                            else
                            {
                                q = "SELECT        dbo.RuntimeModifier.id, dbo.RuntimeModifier.name, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status,                          dbo.RuntimeModifier.branchid, dbo.RuntimeModifier.kdsid, dbo.RuntimeModifier.uploadStatus, dbo.Type.TypeName FROM            dbo.Type INNER JOIN                          dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid  where dbo.RuntimeModifier.id='" + rmodid + "'  and dbo.Type.TypeName!='Packing'";
                            }


                        }
                        DataSet dsrecipyqty = new DataSet();
                        dsrecipyqty = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsrecipyqty.Tables[0].Rows.Count; j++)
                        {
                            temp = dsrecipyqty.Tables[0].Rows[j]["Quantity"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            double recipeqty = Convert.ToDouble(temp);

                            double prc = getprice(dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString());

                            totalqty = (recipeqty * qty);
                            double rate = 0;
                            DataSet dscon = new DataSet();
                            q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + dsrecipyqty.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                            dscon = objCore.funGetDataSet(q);
                            if (dscon.Tables[0].Rows.Count > 0)
                            {
                                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                            }
                            if (rate > 0)
                            {
                                rate = totalqty / rate;
                            }
                            double amount = prc * rate;

                            amount = Math.Round(amount, 3);
                            cost = cost + amount;
                        }
                    }
                }
                catch (Exception ex)
                {


                }


            }

            return cost;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Enabled = false;
            vButton1.Text = "Please Wait";
            fillrecipes();
            fillRawItems();
            bindreportmenuitem();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }
        
        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            try
            {
                CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
                string name = info.Text;
                string flavour = "",flvid="";
                if (name.Contains(" '"))
                {
                    int indx = name.IndexOf(" '", 0);
                    flavour = name.Substring(0, indx);
                    name = name.Substring(indx + 2);
                }
                string q = "select id from menuitem where name='" + name + "'";
                
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string id = ds.Tables[0].Rows[0][0].ToString();

                    if (id.Length > 0)
                    {
                        try
                        {
                            q = "select id from ModifierFlavour where name='" + flavour + "' and menuitemid='" + id + "'";
                            DataSet dsf = new DataSet();
                            dsf = objcore.funGetDataSet(q);
                            if (dsf.Tables[0].Rows.Count > 0)
                            {
                                flvid = dsf.Tables[0].Rows[0][0].ToString();
                            }
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        POSRestaurant.Reports.Inventory.Frmreceipe obj = new Inventory.Frmreceipe();
                        obj.menuid = id;
                        obj.flvid = flvid;
                        obj.date1 = dateTimePicker1.Text;
                        obj.date2 = dateTimePicker2.Text;
                        obj.Show();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            //CrystalDecisions.Windows.Forms.ObjectInfo info = e.ObjectInfo;
            ////String sObjectProperties = "Name: " + info.Name + "\nText: " + info.Text + "\nObject Type: " + info.ObjectType + "\nToolTip: " + info.ToolTip + "\nDataContext: " + info.DataContext + "\nGroup Name Path: " + info.GroupNamePath + "\nHyperlink: " + info.Hyperlink;
            //string name = info.Text;
            //if (type == "group")
            //{
            //    bindreportmenuitem(name);
            //}
            //else
            //{
            //    string size = "",id="";
            //    if (name.Substring(0, 2).Contains("'"))
            //    {
            //        size = name.Substring(0, 2);
            //        name = name.Substring(2);
            //    }
            //    string q = "select id from menuitem where name ='"+name+"'";
            //    DataSet ds = new DataSet();
            //    ds = objCore.funGetDataSet(q);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        id = ds.Tables[0].Rows[0][0].ToString();
            //    }
            //    if (size.Length > 0)
            //    {
            //        q = "select id from ModifierFlavour where MenuItemId ='" + id + "'";
            //        ds = new DataSet();
            //        ds = objCore.funGetDataSet(q);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            size = ds.Tables[0].Rows[0][0].ToString();
            //        }
            //    }
            //    bindreportcost(id, size,size+ name);
            //}
            //vButton2.Visible = true;
            //MessageBox.Show(sObjectProperties);
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
           
            vButton2.Visible = true;
        }
    }
}
