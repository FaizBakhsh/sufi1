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
    public partial class Frmreceipe : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public Frmreceipe()
        {
            InitializeComponent();
        }
        
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,name from menugroup where status='active' order by name";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["name"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbbranch.DataSource = dsi.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "name";
                cmbbranch.Text = "All";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            if (menuid.Length > 0)
            {
                bindreport();
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
                POSRestaurant.Reports.Inventory.rptreceipe rptDoc = new rptreceipe();
                POSRestaurant.Reports.Inventory.DataSet1 dsrpt = new DataSet1();
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
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }

                rptDoc.SetDataSource(dsrpt);
                crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public string date1 = "", date2 = "", menuid = "", flvid = "";
        //private double getprice(string id)
        //{

        //    double variance = 0, price = 0;
        //    string val = "";
        //    DataTable ds = new DataTable();
        //    DataSet dspurchase = new DataSet();
        //    string q = "SELECT     AVG(DISTINCT dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "') and RawItemId = '" + id + "'";

        //    if (price == 0)
        //    {
        //        q = "SELECT   MONTH(date),year(date), avg (dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE  ( dbo.Purchase.date between '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "') and RawItemId = '" + id + "'  group by MONTH(date), year(date)";
        //        // q = "SELECT   MONTH(date),year(date), avg (dbo.PurchaseDetails.PricePerItem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE  ( dbo.Purchase.date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and RawItemId = '" + id + "'  group by MONTH(date), year(date)";

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

        //        q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE      ( date between '" + DateTime.Now.ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "')  and itemId = '" + id + "'   and (price > 0) AND (dbo.InventoryTransfer.TransferIn > 0) GROUP BY MONTH(Date), YEAR(Date)";
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

        //        q = "SELECT    top 1    (price) AS Expr1 FROM            dbo.InventoryTransfer WHERE      ( date < '" + DateTime.Now.ToString("yyyy-MM-dd") + "')  and itemId = '" + id + "'   and (price > 0) AND (dbo.InventoryTransfer.TransferIn > 0) order by date desc";
        //        dspurchase = new DataSet();
        //        dspurchase = objCore.funGetDataSet(q);
        //        for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
        //        {
        //            val = dspurchase.Tables[0].Rows[i]["Expr1"].ToString();
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

        //        q = "SELECT  top 1 (dbo.PurchaseDetails.PricePerItem) AS Expr1, dbo.Purchase.Id FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE  ( dbo.Purchase.date < '" + DateTime.Now.ToString("yyyy-MM-dd") + "') and RawItemId = '" + id + "'   ORDER BY dbo.Purchase.Id DESC";

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
        public double getprice(string id)
        {
            string start; string end;
            start=DateTime.Now.ToString("yyyy-MM-dd");
            end = DateTime.Now.ToString("yyyy-MM-dd");
            if (menuid.Length > 0)
            {
                start = date1; end = date2;
            }
            double cost = 0;

            string q = "select  dbo.Getprice('" + start + "','" + end + "'," + id + ")";
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
                dtrpt.Columns.Add("Group", typeof(string));
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("Item", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(string));
                dtrpt.Columns.Add("Amount", typeof(double));
                dtrpt.Columns.Add("Total", typeof(double));
                dtrpt.Columns.Add("type", typeof(string));
                dtrpt.Columns.Add("Price", typeof(double));
                DataSet ds = new DataSet();
                string q = "", date = "";
                string id = "";
                bool chk = true;
                if (menuid.Length > 0)
                {
                   
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Price AS price2,dbo.ModifierFlavour.price as price1, dbo.MenuItem.Name AS menu, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Id, dbo.MenuItem.MenuGroupId, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id as flavourid FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where  dbo.MenuItem.id='" + menuid + "' ORDER BY menugroup, menu";
                    }
                }
                else
                {
                    if (textBox1.Text.Trim().Length > 0)
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Price AS price2,dbo.ModifierFlavour.price as price1, dbo.MenuItem.Name AS menu, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Id, dbo.MenuItem.MenuGroupId, dbo.ModifierFlavour.name , dbo.ModifierFlavour.id as flavourid FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where dbo.MenuItem.Name like '%" + textBox1.Text + "%' ORDER BY menugroup, menu";
                        }
                        else
                        {
                            q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Price AS price2,dbo.ModifierFlavour.price as price1, dbo.MenuItem.Name AS menu, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Id, dbo.MenuItem.MenuGroupId, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id as flavourid  FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where  dbo.MenuItem.Name like '%" + textBox1.Text + "%' and dbo.MenuGroup.id='" + cmbbranch.SelectedValue + "' ORDER BY menugroup, menu";

                        }
                    }
                    else
                    {
                        if (cmbbranch.Text == "All")
                        {
                            q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Price AS price2,dbo.ModifierFlavour.price as price1, dbo.MenuItem.Name AS menu, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Id, dbo.MenuItem.MenuGroupId, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id as flavourid  FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId  ORDER BY menugroup, menu";
                        }
                        else
                        {
                            q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Price AS price2,dbo.ModifierFlavour.price as price1, dbo.MenuItem.Name AS menu, dbo.MenuGroup.Name AS menugroup, dbo.MenuItem.Id, dbo.MenuItem.MenuGroupId, dbo.ModifierFlavour.name, dbo.ModifierFlavour.id as flavourid  FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where  dbo.MenuGroup.id='" + cmbbranch.SelectedValue + "' ORDER BY menugroup, menu";

                        }
                    }
                }
                DataSet dsmenuitms = new DataSet();
                dsmenuitms = objCore.funGetDataSet(q);
                string price = "0";
                string convs = "0";
                double amount = 0, qty = 0, menuprice=0;
                string qt = "0";
                for (int p = 0; p < dsmenuitms.Tables[0].Rows.Count; p++)
                {
                     string modifierflavourid=dsmenuitms.Tables[0].Rows[p]["flavourid"].ToString();
                    //if (cmbbranch.Text == "All")
                    //{
                    //    q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.ModifierFlavour.price as price1, dbo.MenuItem.price as price2, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.price, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                    //}
                    //else
                    //{
                    //    q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.ModifierFlavour.price as price1, dbo.MenuItem.price as price2,dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.price, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.MenuItem.menugroupid='" + cmbbranch.SelectedValue + "' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                    //}
                     if (modifierflavourid == "" || modifierflavourid == "0")
                     {
                         q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.ModifierFlavour.id as flavourid, dbo.ModifierFlavour.price as price1, dbo.MenuItem.price as price2,dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.price, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.MenuItem.id='" + dsmenuitms.Tables[0].Rows[p]["Id"] + "' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                     }
                     else
                     {
                         q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.ModifierFlavour.id as flavourid, dbo.ModifierFlavour.price as price1, dbo.MenuItem.price as price2,dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.price, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.Recipe.MenuItemId='" + dsmenuitms.Tables[0].Rows[p]["Id"] + "' and dbo.Recipe.modifierid='" + modifierflavourid + "'  ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                     }
                    
                   
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                       
                        if (ds.Tables[0].Rows[i]["menu"].ToString().Contains("Iftar Two"))
                        {
                            if (ds.Tables[0].Rows[i]["ItemName"].ToString().Contains("Tortil"))
                            {

                            }
                        }
                        if (id != ds.Tables[0].Rows[i]["MenuItemId"].ToString())
                        {
                            chk = true;
                        }
                        id = ds.Tables[0].Rows[i]["MenuItemId"].ToString();
                        price = getprice( ds.Tables[0].Rows[i]["id"].ToString()).ToString();
                        if (price == "")
                        {
                            price = "0";
                        }

                        convs = ds.Tables[0].Rows[i]["ConversionRate"].ToString();
                        if (convs == "")
                        {
                            convs = "1";
                        }
                        qt = ds.Tables[0].Rows[i]["Quantity"].ToString();
                        if (qt == "")
                        {
                            qt = "0";
                        }                      
                        try
                        {
                            amount = Convert.ToDouble(price);
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            qty = Convert.ToDouble(qt);
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
                        if (amount == 0)
                        {
                            amount = 1;
                        }
                        string temp = ds.Tables[0].Rows[i]["price1"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        menuprice = Convert.ToDouble(temp);
                        temp = ds.Tables[0].Rows[i]["price2"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        menuprice = menuprice + Convert.ToDouble(temp);
                        if (menuprice == 0)
                        {
                            menuprice = 1;
                        }
                        dtrpt.Rows.Add(dsmenuitms.Tables[0].Rows[p]["menugroup"].ToString(), ds.Tables[0].Rows[i]["name"].ToString() + "-" + ds.Tables[0].Rows[i]["menu"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), amount, qty * amount, ds.Tables[0].Rows[i]["type"].ToString(), menuprice);

                    }
                        //if (chk == true)
                        {
                            price = "0";
                            if (checkBox1.Checked==true)
                            {
                                chk = false;
                                q = "select distinct stage from RuntimeModifier  where menuItemid='" + dsmenuitms.Tables[0].Rows[p]["id"].ToString() + "'";
                                DataSet dsdet = new DataSet();
                                dsdet = objCore.funGetDataSet(q);

                                for (int k = 0; k < dsdet.Tables[0].Rows.Count; k++)
                                {
                                    q = "SELECT    top 1    dbo.RuntimeModifier.id,dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.rawitemid, dbo.RuntimeModifier.Type, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM FROM            dbo.RuntimeModifier INNER JOIN                         dbo.RawItem ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId  where dbo.RuntimeModifier.menuItemid='" + dsmenuitms.Tables[0].Rows[p]["id"].ToString() + "' and  dbo.RuntimeModifier.stage='" + dsdet.Tables[0].Rows[k]["stage"].ToString() + "' order by dbo.RuntimeModifier.Name asc";
                                    DataSet dsd = new DataSet();
                                    dsd = objCore.funGetDataSet(q);
                                    for (int j = 0; j < dsd.Tables[0].Rows.Count; j++)
                                    {

                                        id = dsmenuitms.Tables[0].Rows[p]["id"].ToString();
                                        price = getprice(dsd.Tables[0].Rows[j]["rawitemid"].ToString()).ToString();
                                        if (price == "")
                                        {
                                            price = "0";
                                        }
                                        qt = dsd.Tables[0].Rows[j]["Quantity"].ToString();
                                        if (qt == "")
                                        {
                                            qt = "0";
                                        }
                                        convs = dsd.Tables[0].Rows[j]["ConversionRate"].ToString();
                                        if (convs == "")
                                        {
                                            convs = "1";
                                        }
                                        amount = 0; qty = 0;
                                        try
                                        {
                                            amount = Convert.ToDouble(price);
                                        }
                                        catch (Exception ex)
                                        {


                                        }
                                        try
                                        {
                                            qty = Convert.ToDouble(qt);
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
                                        if (amount == 0)
                                        {
                                            amount = 1;
                                        }
                                        string temp = "";
                                        try
                                        {
                                            temp = dsmenuitms.Tables[0].Rows[p]["price1"].ToString();
                                            if (temp == "")
                                            {
                                                temp = "0";
                                            }
                                            menuprice = Convert.ToDouble(temp);
                                            temp = dsmenuitms.Tables[0].Rows[p]["price2"].ToString();
                                            if (temp == "")
                                            {
                                                temp = "0";
                                            }
                                            menuprice = menuprice + Convert.ToDouble(temp);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                        if (menuprice == 0)
                                        {
                                            menuprice = 1;
                                        }
                                        dtrpt.Rows.Add(dsmenuitms.Tables[0].Rows[p]["menugroup"].ToString(), dsmenuitms.Tables[0].Rows[p]["name"].ToString() + "-" + dsmenuitms.Tables[0].Rows[p]["menu"].ToString(), dsd.Tables[0].Rows[j]["ItemName"].ToString(), dsd.Tables[0].Rows[j]["UOM"].ToString(), dsd.Tables[0].Rows[j]["Quantity"].ToString(), amount, qty * amount, dsd.Tables[0].Rows[j]["type"].ToString(), menuprice);

                                        try
                                        {


                                            {
                                                string temp1 = dsd.Tables[0].Rows[j]["Quantity"].ToString();
                                                if (temp1 == "")
                                                {
                                                    temp1 = "0";
                                                }
                                                float subatachqty = float.Parse(temp1);
                                                if (subatachqty == 0)
                                                {
                                                    subatachqty = 1;
                                                }
                                                DataSet dsrecipie1 = new DataSet();
                                                //attachrecipie(dsatach.Tables[0].Rows[j]["attachmenuid"].ToString(), float.Parse(temp1));
                                                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid ,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.Type.TypeName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where  dbo.AttachRecipe.type='RuntimeModifier' and dbo.AttachRecipe.Menuitemid ='" + dsd.Tables[0].Rows[j]["id"].ToString() + "' ";
                                                dsrecipie1 = objCore.funGetDataSet(q);
                                                if (dsrecipie1.Tables[0].Rows.Count > 0)
                                                {
                                                    for (int l = 0; l < dsrecipie1.Tables[0].Rows.Count; l++)
                                                    {
                                                        string subname = dsrecipie1.Tables[0].Rows[l]["Name"].ToString();
                                                        if (subname.ToString().ToLower().Contains("whip"))
                                                        {

                                                        }
                                                        string rawitmid = dsrecipie1.Tables[0].Rows[l]["RawItemId"].ToString();
                                                        float qnty = float.Parse(dsrecipie1.Tables[0].Rows[l]["Qty"].ToString());
                                                        double convrate = double.Parse(dsrecipie1.Tables[0].Rows[l]["ConversionRate"].ToString());
                                                        double recipiqnty = double.Parse(dsrecipie1.Tables[0].Rows[l]["Quantity"].ToString());
                                                        double recipiattachqnty = double.Parse(dsrecipie1.Tables[0].Rows[l]["attachQty"].ToString());
                                                        double amounttodeduct = recipiqnty * subatachqty;
                                                        amounttodeduct = amounttodeduct * recipiattachqnty;
                                                        amounttodeduct = Math.Round(amounttodeduct, 3);
                                                        double rate = 0, rate1 = 0;
                                                        DataSet dscon = new DataSet();
                                                        q = "SELECT     dbo.RawItem.ItemName,dbo.RawItem.price, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + rawitmid + "'";
                                                        dscon = objCore.funGetDataSet(q);
                                                        if (dscon.Tables[0].Rows.Count > 0)
                                                        {
                                                            rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                                                        }
                                                        if (rate > 0)
                                                        {
                                                            rate1 = amounttodeduct / rate;
                                                        }
                                                        temp = dscon.Tables[0].Rows[0]["price"].ToString();
                                                        if (temp == "")
                                                        {
                                                            temp = "0";
                                                        }

                                                        double prc = getprice(rawitmid);
                                                        double amountt = prc * rate1;
                                                        amountt = Math.Round(amountt, 3);
                                                        if (amountt == 0)
                                                        {
                                                            amountt = 1;
                                                        }
                                                        try
                                                        {
                                                            temp = dsmenuitms.Tables[0].Rows[p]["price1"].ToString();
                                                            if (temp == "")
                                                            {
                                                                temp = "0";
                                                            }
                                                            menuprice = Convert.ToDouble(temp);
                                                            temp = dsmenuitms.Tables[0].Rows[p]["price2"].ToString();
                                                            if (temp == "")
                                                            {
                                                                temp = "0";
                                                            }
                                                            menuprice = menuprice + Convert.ToDouble(temp);
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                        }
                                                        if (menuprice == 0)
                                                        {
                                                            menuprice = 1;
                                                        }

                                                        dtrpt.Rows.Add(dsmenuitms.Tables[0].Rows[p]["menugroup"].ToString(), dsmenuitms.Tables[0].Rows[p]["name"].ToString() + "-" + dsmenuitms.Tables[0].Rows[p]["menu"].ToString(), "(" + subname + ")" + dscon.Tables[0].Rows[0]["ItemName"].ToString(), dscon.Tables[0].Rows[0]["UOM"].ToString(), amounttodeduct, prc / rate, amountt, "Both", menuprice);

                                                    }
                                                }
                                            }
                                        }
                                        catch (System.Exception ex)
                                        {
                                            MessageBox.Show(ex.Message);

                                        }

                                    }





                                } 
                            }
                            if (modifierflavourid == "" || modifierflavourid == "0")
                            {
                                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid ,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.Type.TypeName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where  dbo.AttachRecipe.type='MenuItem' and dbo.AttachRecipe.Menuitemid ='" + dsmenuitms.Tables[0].Rows[p]["id"].ToString() + "' ";

                            }
                            else
                            {
                                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid ,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.Type.TypeName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where  dbo.AttachRecipe.type='MenuItem' and dbo.AttachRecipe.Menuitemid ='" + dsmenuitms.Tables[0].Rows[p]["id"].ToString() + "' and dbo.AttachRecipe.FlavourId='" + modifierflavourid + "' ";

                            }
                           

                             DataSet dsrecipie = objCore.funGetDataSet(q);
                            if (dsrecipie.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsrecipie.Tables[0].Rows.Count; k++)
                                {
                                    string subname = dsrecipie.Tables[0].Rows[k]["Name"].ToString();
                                    if (subname.ToString().ToLower().Contains("whip"))
                                    {

                                    }
                                    string rawitmid = dsrecipie.Tables[0].Rows[k]["RawItemId"].ToString();
                                    float qnty = float.Parse(dsrecipie.Tables[0].Rows[k]["Qty"].ToString());
                                    double convrate = double.Parse(dsrecipie.Tables[0].Rows[k]["ConversionRate"].ToString());
                                    double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[k]["Quantity"].ToString());
                                    double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[k]["attachQty"].ToString());
                                    double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;                                
                                    amounttodeduct = amounttodeduct * recipiattachqnty;
                                    amounttodeduct = Math.Round(amounttodeduct, 3);
                                    double rate = 0, rate1 = 0;
                                    DataSet dscon = new DataSet();
                                    q = "SELECT     dbo.RawItem.ItemName,dbo.RawItem.price, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + rawitmid + "'";
                                    dscon = objCore.funGetDataSet(q);
                                    if (dscon.Tables[0].Rows.Count > 0)
                                    {
                                        rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                                    }
                                    if (rate > 0)
                                    {
                                        rate1 = amounttodeduct / rate;
                                    }
                                   string temp = dscon.Tables[0].Rows[0]["price"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "0";
                                    }

                                    double prc = getprice(rawitmid);
                                    double amountt = prc * rate1;
                                    amountt = Math.Round(amountt, 3);
                                    if (amountt == 0)
                                    {
                                        amountt = 1;
                                    }
                                    try
                                    {
                                        temp = dsmenuitms.Tables[0].Rows[p]["price1"].ToString();
                                        if (temp == "")
                                        {
                                            temp = "0";
                                        }
                                        menuprice = Convert.ToDouble(temp);
                                        temp = dsmenuitms.Tables[0].Rows[p]["price2"].ToString();
                                        if (temp == "")
                                        {
                                            temp = "0";
                                        }
                                        menuprice = menuprice + Convert.ToDouble(temp);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                    if (menuprice == 0)
                                    {
                                        menuprice = 1;
                                    }
                                    dtrpt.Rows.Add(dsmenuitms.Tables[0].Rows[p]["menugroup"].ToString(), dsmenuitms.Tables[0].Rows[p]["name"].ToString() + "-" + dsmenuitms.Tables[0].Rows[p]["menu"].ToString(), "(" + subname + ")" + dscon.Tables[0].Rows[0]["ItemName"].ToString(), dscon.Tables[0].Rows[0]["UOM"].ToString(), amounttodeduct, prc / rate, amountt, "Both", menuprice);
                                }
                            }

                            try
                            {
                                q = "SELECT        id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status FROM            Attachmenu1 where userecipe='yes' and  status='active' and menuitemid='" + dsmenuitms.Tables[0].Rows[p]["id"].ToString() + "' and type='MenuItem'";
                                DataSet dsatach = new DataSet();
                                dsatach = objCore.funGetDataSet(q);
                                for (int j = 0; j < dsatach.Tables[0].Rows.Count; j++)
                                {
                                    string temp1 = dsatach.Tables[0].Rows[j]["Quantity"].ToString();
                                    if (temp1 == "")
                                    {
                                        temp1 = "0";
                                    }
                                    float subatachqty = float.Parse(temp1);


                                    try
                                    {
                                        q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.ModifierFlavour.price as price1, dbo.MenuItem.price as price2,dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName,                                   dbo.RawItem.id,dbo.RawItem.price, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM FROM            dbo.Recipe INNER JOIN                                                               dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                                                            dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                                                            dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id                                    where dbo.MenuItem.id='" + dsatach.Tables[0].Rows[j]["attachmenuid"].ToString() + "' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                                        DataSet dsattach = new DataSet();
                                       
                                        dsattach = objCore.funGetDataSet(q);
                                        for (int i = 0; i < dsattach.Tables[0].Rows.Count; i++)
                                        {
                                            if (dsattach.Tables[0].Rows[i]["menu"].ToString().Contains("Iftar Two"))
                                            {
                                                if (dsattach.Tables[0].Rows[i]["ItemName"].ToString().Contains("Tortil"))
                                                {

                                                }
                                            }
                                            if (id != dsattach.Tables[0].Rows[i]["MenuItemId"].ToString())
                                            {
                                                chk = true;
                                            }
                                            id = dsattach.Tables[0].Rows[i]["MenuItemId"].ToString();
                                            price = getprice(dsattach.Tables[0].Rows[i]["id"].ToString()).ToString();
                                            if (price == "")
                                            {
                                                price = "0";
                                            }

                                            convs = dsattach.Tables[0].Rows[i]["ConversionRate"].ToString();
                                            if (convs == "")
                                            {
                                                convs = "1";
                                            }
                                            qt = dsattach.Tables[0].Rows[i]["Quantity"].ToString();
                                            if (qt == "")
                                            {
                                                qt = "0";
                                            }
                                            try
                                            {
                                                amount = Convert.ToDouble(price);
                                            }
                                            catch (Exception ex)
                                            {


                                            }
                                            try
                                            {
                                                qty = Convert.ToDouble(qt) * subatachqty;
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
                                            if (amount == 0)
                                            {
                                                amount = 1;
                                            }
                                            string temp = dsattach.Tables[0].Rows[i]["price1"].ToString();
                                            if (temp == "")
                                            {
                                                temp = "0";
                                            }

                                            dtrpt.Rows.Add(dsmenuitms.Tables[0].Rows[p]["menugroup"].ToString(), dsmenuitms.Tables[0].Rows[p]["name"].ToString() + "-" + dsmenuitms.Tables[0].Rows[p]["menu"].ToString(), "(" + dsattach.Tables[0].Rows[i]["menu"].ToString() + ")" + dsattach.Tables[0].Rows[i]["ItemName"].ToString(), dsattach.Tables[0].Rows[i]["UOM"].ToString(), qty, amount, qty * amount, dsattach.Tables[0].Rows[i]["type"].ToString(), menuprice);

                                        }
                                    }
                                    catch (Exception ee)
                                    {
                                        
                                    }



                                    //attachrecipie(dsatach.Tables[0].Rows[j]["attachmenuid"].ToString(), float.Parse(temp1));
                                   
                                    if (modifierflavourid == "" || modifierflavourid == "0")
                                    {
                                        q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid ,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.Type.TypeName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where  dbo.AttachRecipe.type='MenuItem' and dbo.AttachRecipe.Menuitemid ='" + dsatach.Tables[0].Rows[j]["attachmenuid"].ToString() + "' ";

                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid ,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.Type.TypeName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where  dbo.AttachRecipe.type='MenuItem' and dbo.AttachRecipe.Menuitemid ='" + dsmenuitms.Tables[0].Rows[p]["id"].ToString() + "' and dbo.AttachRecipe.FlavourId='" + modifierflavourid + "' ";
                                        q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid ,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.Type.TypeName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where  dbo.AttachRecipe.type='MenuItem' and dbo.AttachRecipe.Menuitemid ='" + dsatach.Tables[0].Rows[j]["id"].ToString() + "'  and dbo.AttachRecipe.FlavourId='" + modifierflavourid + "' ";

                                    }

                                    q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid ,                          dbo.SubRecipe.RawItemId, dbo.SubRecipe.Quantity, dbo.Type.TypeName FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId INNER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id  where  dbo.AttachRecipe.type='MenuItem' and dbo.AttachRecipe.Menuitemid ='" + dsatach.Tables[0].Rows[j]["attachmenuid"].ToString() + "' ";

                                    dsrecipie = objCore.funGetDataSet(q);
                                    if (dsrecipie.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dsrecipie.Tables[0].Rows.Count; k++)
                                        {
                                            string subname = dsrecipie.Tables[0].Rows[k]["Name"].ToString();
                                            if (subname.ToString().ToLower().Contains("whip"))
                                            {
                                            }
                                            string rawitmid = dsrecipie.Tables[0].Rows[k]["RawItemId"].ToString();
                                            float qnty = float.Parse(dsrecipie.Tables[0].Rows[k]["Qty"].ToString());
                                            double convrate = double.Parse(dsrecipie.Tables[0].Rows[k]["ConversionRate"].ToString());
                                            double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[k]["Quantity"].ToString());
                                            double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[k]["attachQty"].ToString());
                                            double amounttodeduct = recipiqnty * subatachqty;
                                            amounttodeduct = amounttodeduct * recipiattachqnty;
                                            amounttodeduct = Math.Round(amounttodeduct, 3);
                                            double rate = 0, rate1 = 0;
                                            DataSet dscon = new DataSet();
                                            q = "SELECT     dbo.RawItem.ItemName,dbo.RawItem.price, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + rawitmid + "'";
                                            dscon = objCore.funGetDataSet(q);
                                            if (dscon.Tables[0].Rows.Count > 0)
                                            {
                                                rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                                            }
                                            if (rate > 0)
                                            {
                                                rate1 = amounttodeduct / rate;
                                            }
                                           string temp = dscon.Tables[0].Rows[0]["price"].ToString();
                                            if (temp == "")
                                            {
                                                temp = "0";
                                            }

                                            double prc = getprice(rawitmid);
                                            double amountt = prc * rate1;
                                            amountt = Math.Round(amountt, 3);
                                            if (amountt == 0)
                                            {
                                                amountt = 1;
                                            }
                                            try
                                            {
                                                temp = dsmenuitms.Tables[0].Rows[p]["price1"].ToString();
                                                if (temp == "")
                                                {
                                                    temp = "0";
                                                }
                                                menuprice = Convert.ToDouble(temp);
                                                temp = dsmenuitms.Tables[0].Rows[p]["price2"].ToString();
                                                if (temp == "")
                                                {
                                                    temp = "0";
                                                }
                                                menuprice = menuprice + Convert.ToDouble(temp);
                                            }
                                            catch (Exception ex)
                                            {
                                                
                                            }
                                            if (menuprice == 0)
                                            {
                                                menuprice = 1;
                                            }
                                            dtrpt.Rows.Add(dsmenuitms.Tables[0].Rows[p]["menugroup"].ToString(),dsmenuitms.Tables[0].Rows[p]["name"].ToString() + "-" + dsmenuitms.Tables[0].Rows[p]["menu"].ToString(), "(" + subname + ")" + dscon.Tables[0].Rows[0]["ItemName"].ToString(), dscon.Tables[0].Rows[0]["UOM"].ToString(), amounttodeduct, prc / rate, amountt, "Both", menuprice);
                                        }
                                    }
                                }
                            }
                            catch (System.Exception ex)
                            {

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
