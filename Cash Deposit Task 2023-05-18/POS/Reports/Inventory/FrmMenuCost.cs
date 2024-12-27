using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.Inventory
{
    public partial class FrmMenuCost : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        string branchtype = "";
        public FrmMenuCost()
        {
            InitializeComponent();
            branchtype = getbranchtype();
        }
        string branchid = "";
        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,name from menugroup where status='active'";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["name"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbgroup.DataSource = dsi.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "name";
                cmbgroup.Text = "All";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            try
            {
                string q = "select * from branch";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchid = ds.Tables[0].Rows[0]["id"].ToString();
                }
            }
            catch (Exception ex)
            {
                
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
            
            POSRestaurant.Reports.Inventory.rptmenucost rptDoc = new rptmenucost();
            dsmenucost ds = new dsmenucost();
            //feereport ds = new feereport(); // .xsd file name
            DataTable dt = new DataTable();

            dt.TableName = "DataTable1";
            dt = getAllOrdersmenuitem();
           
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
                ds.DataTable1.Merge(dt);
            }
            rptDoc.SetDataSource(ds);
            //wastage();
            //rptDoc.SetParameterValue("waste", waste1);
            //rptDoc.SetParameterValue("variance", variance1);
            //rptDoc.SetParameterValue("compwaste", compwst1);
            //rptDoc.SetParameterValue("Comp", company);
            //rptDoc.SetParameterValue("phone", phone);
            //rptDoc.SetParameterValue("Address", address);
           
            crystalReportViewer1.ReportSource = rptDoc;
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        
        double waste1 = 0, variance1 = 0, compwst1 = 0;
        public double getcostprice(string id)
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
        public double attachMenucost(string itmid, string flvid, float itmqnty, string type, string recipetype)
        {
            double cost = 0;
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
                        string menuid = dsrecipie.Tables[0].Rows[i]["attachmenuid"].ToString();
                        string flid = dsrecipie.Tables[0].Rows[i]["attachFlavourid"].ToString();
                        if (flid == "")
                        {
                            flid = "0";
                        }
                        double qty = float.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        if (recipetype == "Packing")
                        {
                            cost = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), menuid, flid, "0", "0", branchtype, "Packing");
                        }
                        else
                        {
                            cost = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), menuid, flid, "0", "0", branchtype, "");
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
                    if (flavourid == "")
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
                    if (flavourid == "")
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
                dtrptmenu.Columns.Add("Group", typeof(string));
                dtrptmenu.Columns.Add("Name", typeof(string));
                dtrptmenu.Columns.Add("Price", typeof(double));
                dtrptmenu.Columns.Add("Cost", typeof(double));
                dtrptmenu.Columns.Add("GrossProfit", typeof(double));
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
                    q = "SELECT        dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.Id AS Flavourid, dbo.ModifierFlavour.price AS fprice, dbo.ModifierFlavour.name AS Flavour, dbo.MenuGroup.Name AS groupname FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where dbo.MenuItem.status='Active' order by dbo.MenuItem.name";
                }
                else
                {
                    q = "SELECT        dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Price, dbo.ModifierFlavour.Id AS Flavourid, dbo.ModifierFlavour.price AS fprice, dbo.ModifierFlavour.name AS Flavour, dbo.MenuGroup.Name AS groupname FROM            dbo.MenuItem INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where dbo.MenuItem.status='Active' and dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "' order by dbo.MenuItem.name";
                }
                command = new SqlCommand(q, connection);
                
                connection.Open();
                SqlDataReader dr = command.ExecuteReader();

                while (dr.Read())
                {
                    string name = dr["Name"].ToString();
                    if (dr["Name"].ToString().Contains("Green") )
                    {
                        string na = dr["Name"].ToString();
                    }

                    string size = dr["Flavour"].ToString();
                    if (size.Length > 0)
                    {
                        size = size + " '";
                    }
                    double perc = 0, totalsale = 0;

                    double cost = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), dr["id"].ToString(), dr["Flavourid"].ToString(), "0", "0", branchtype, "");
                    cost = cost * 1;
                    double pcost = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), dr["id"].ToString(), dr["Flavourid"].ToString(), "0", "0", branchtype, "Packing");
                    pcost = pcost * 1;
                    try
                    {
                        cost = cost + attachMenucost(dr["id"].ToString(), dr["Flavourid"].ToString(), 1, "MenuItem", "MenuItem");
                        pcost = pcost + attachMenucost(dr["id"].ToString(), dr["Flavourid"].ToString(), 1, "MenuItem", "Packing");
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        cost = cost + getsubitemcost(dr["id"].ToString(), dr["Flavourid"].ToString(), 1, "", "MenuItem");
                        pcost = pcost + getsubitemcost(dr["id"].ToString(), dr["Flavourid"].ToString(), 1, "Packing", "MenuItem");
                    }
                    catch (Exception ex)
                    {

                    }
                    string temp1 = dr["Price"].ToString();
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
                    
                   
                    
                    if (logo == "")
                    {
                        dtrptmenu.Rows.Add(dr["groupname"].ToString(), size + name, Math.Round(mprice + fprice, 3), cost + pcost, (Math.Round(mprice + fprice, 3) - (cost + pcost)));
                    }
                    else
                    {
                        dtrptmenu.Rows.Add(dr["groupname"].ToString(), size + name, Math.Round(mprice + fprice, 3), cost + pcost, (Math.Round(mprice + fprice, 3) - (cost + pcost)));
                    }
                }
                
                connection.Close();
                
              
               
                
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            return dtrptmenu;
        }
      
       
        protected string getbranchtype()
        {
            string branchtype = "";
            try
            {
                string q = "select type from branch where id='" + branchid + "'";
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
     
        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton1.Enabled = false;
            vButton1.Text = "Please Wait";
            bindreportmenuitem();
            vButton1.Text = "View";
            vButton1.Enabled = true;
        }
        
        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
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
           
        }
    }
}
