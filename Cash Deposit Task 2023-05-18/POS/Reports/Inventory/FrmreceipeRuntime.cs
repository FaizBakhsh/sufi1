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
    public partial class FrmreceipeRuntime : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmreceipeRuntime()
        {
            InitializeComponent();
        }
        public string date1 = "", date2 = "", menuid = "";
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
                cmbgroup.DataSource = dsi.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "name";
                cmbgroup.Text = "All";

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
        public double getcostmenu1(string start, string end, string id)
        {
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
                DataSet ds = new DataSet();
                string q = "", date = "";
                string id = "", flid = "";
                bool chk = true;
                if (menuid.Length > 0)
                {

                    q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')  and dbo.RuntimeModifier.id='" + menuid + "' ORDER BY menu";

                }
                else
                {
                    if (textBox1.Text.Trim() == "")
                    {
                        if (cmbgroup.Text == "All")
                        {
                            if (cmbmenu.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active') ORDER BY menu";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')   and dbo.RuntimeModifier.menuitemid='" + cmbmenu.SelectedValue + "' ORDER BY menu";

                            }
                        }
                        else
                        {
                            if (cmbmenu.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')   and dbo.menuitem.menugroupid='" + cmbgroup.SelectedValue + "'  ORDER BY menu";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')     and dbo.RuntimeModifier.menuitemid='" + cmbmenu.SelectedValue + "' ORDER BY menu";

                            }
                        }
                    }
                    else
                    {

                        if (cmbgroup.Text == "All")
                        {
                            if (cmbmenu.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')  and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  ORDER BY menu";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')   and dbo.RuntimeModifier.menuitemid='" + cmbmenu.SelectedValue + "' and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  ORDER BY menu";

                            }
                        }
                        else
                        {
                            if (cmbmenu.Text == "All")
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')   and dbo.menuitem.menugroupid='" + cmbgroup.SelectedValue + "' and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'   ORDER BY menu";
                            }
                            else
                            {
                                q = "SELECT        TOP (100) PERCENT dbo.MenuItem.Name AS menu, dbo.RuntimeModifier.name,dbo.RuntimeModifier.Id as RId, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.UOMConversion.UOM,                          dbo.RuntimeModifier.price AS mprice, dbo.RuntimeModifier.menuItemid, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.type FROM            dbo.RuntimeModifier INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ON dbo.RuntimeModifier.rawitemid = dbo.RawItem.Id WHERE        (dbo.MenuItem.Status = 'Active')     and dbo.RuntimeModifier.menuitemid='" + cmbmenu.SelectedValue + "' and dbo.RuntimeModifier.name  like '%" + textBox1.Text + "%'  ORDER BY menu";

                            }
                        }
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double mprice = 0;
                    string temp = "0";
                    temp = ds.Tables[0].Rows[i]["mprice"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    mprice = Convert.ToDouble(temp);
                    temp = "0";

                    mprice = Math.Round(mprice, 2);
                    if ("101" == ds.Tables[0].Rows[i]["id"].ToString())
                    {

                    }
                    string price = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), ds.Tables[0].Rows[i]["id"].ToString()).ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    string convs = ds.Tables[0].Rows[i]["ConversionRate"].ToString();
                    if (convs == "")
                    {
                        convs = "1";
                    }
                    string qt = ds.Tables[0].Rows[i]["Quantity"].ToString();
                    if (qt == "")
                    {
                        qt = "0";
                    }
                    double amount = 0, qty = 0;
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

                    string name = ds.Tables[0].Rows[i]["name"].ToString();


                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menu"].ToString(), name + "(" + mprice + ")", ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), amount, qty * amount, ds.Tables[0].Rows[i]["type"].ToString());

                    try
                    {
                        string cid = "", cflid = "";
                        bool chkatachsub = true;
                        q = "SELECT        dbo.MenuItem.Name, dbo.RawItem.ItemName, dbo.UOMConversion.ConversionRate, dbo.Recipe.RawItemId, dbo.Attachmenu1.Quantity AS attachqty, dbo.Recipe.Quantity, dbo.UOMConversion.UOM,                          dbo.Attachmenu1.attachmenuid, dbo.Attachmenu1.attachFlavourid, dbo.Attachmenu1.menuitemid FROM            dbo.Recipe INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId RIGHT OUTER JOIN                         dbo.Attachmenu1 ON dbo.Recipe.modifierid = dbo.Attachmenu1.attachFlavourid AND dbo.MenuItem.Id = dbo.Attachmenu1.attachmenuid WHERE        (dbo.Attachmenu1.userecipe = 'yes') AND dbo.AttachRecipe.Type='RuntimeModifier' and dbo.AttachRecipe.Menuitemid='" + ds.Tables[0].Rows[i]["RId"].ToString() + "'";
                      
                        DataSet dssubitem = new DataSet();
                        dssubitem = objCore.funGetDataSet(q);
                        for (int h = 0; h < dssubitem.Tables[0].Rows.Count; h++)
                        {
                            string attachmenuname = dssubitem.Tables[0].Rows[h]["Name"].ToString();
                            if (cid != dssubitem.Tables[0].Rows[h]["attachmenuid"].ToString() || cflid != dssubitem.Tables[0].Rows[h]["attachFlavourid"].ToString())
                            {
                                cid = dssubitem.Tables[0].Rows[h]["attachmenuid"].ToString();
                                cflid = dssubitem.Tables[0].Rows[h]["attachFlavourid"].ToString();
                                chkatachsub = true;
                            }

                            price = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), dssubitem.Tables[0].Rows[h]["RawItemId"].ToString()).ToString();
                            if (price == "")
                            {
                                price = "0";
                            }
                            convs = dssubitem.Tables[0].Rows[h]["ConversionRate"].ToString();
                            if (convs == "")
                            {
                                convs = "1";
                            }
                            qt = dssubitem.Tables[0].Rows[h]["Quantity"].ToString();
                            if (qt == "")
                            {
                                qt = "0";
                            }
                            double quantity = Convert.ToDouble(qt);
                            qt = dssubitem.Tables[0].Rows[h]["attachqty"].ToString();
                            if (qt == "")
                            {
                                qt = "0";
                            }
                            quantity = quantity * Convert.ToDouble(qt);
                            quantity = Math.Round(quantity, 3);
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
                                amount = amount / Convert.ToDouble(convs);
                            }
                            catch (Exception ex)
                            {


                            }

                            string subname = dssubitem.Tables[0].Rows[h]["name"].ToString();
                            try
                            {
                                subname = dssubitem.Tables[0].Rows[h]["Flavour"].ToString() + " " + subname;
                            }
                            catch (Exception ex)
                            {

                            }
                            if (dssubitem.Tables[0].Rows[h]["ItemName"].ToString().Length > 0)
                            {
                                dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menu"].ToString(), name + "(" + mprice + ")", "(" + subname + ")" + dssubitem.Tables[0].Rows[h]["ItemName"].ToString(), dssubitem.Tables[0].Rows[h]["UOM"].ToString(), quantity, amount, quantity * amount, "Both");
                            }
                           
                            try
                            {
                                if (chkatachsub == true)
                                {
                                    chkatachsub = false;
                                    if (ds.Tables[0].Rows[i]["modifierid"].ToString() == "")
                                    {
                                        q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.Type, dbo.SubItems.Name, dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName,                          dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.AttachRecipe.Type='MenuItem' and dbo.AttachRecipe.Menuitemid='" + dssubitem.Tables[0].Rows[h]["attachmenuid"].ToString() + "'";
                                    }
                                    else
                                    {
                                        q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.Type, dbo.SubItems.Name, dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName,                          dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId  where dbo.AttachRecipe.Type='MenuItem' and dbo.AttachRecipe.Menuitemid='" + dssubitem.Tables[0].Rows[h]["attachmenuid"].ToString() + "' and dbo.AttachRecipe.FlavourId='" + dssubitem.Tables[0].Rows[h]["attachFlavourid"].ToString() + "'";
                                    }
                                    DataSet dssubitem1 = new DataSet();
                                    dssubitem1 = objCore.funGetDataSet(q);
                                    for (int g = 0; g < dssubitem1.Tables[0].Rows.Count; g++)
                                    {
                                        price = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), dssubitem1.Tables[0].Rows[g]["RawItemId"].ToString()).ToString();
                                        if (price == "")
                                        {
                                            price = "0";
                                        }
                                        convs = dssubitem1.Tables[0].Rows[g]["ConversionRate"].ToString();
                                        if (convs == "")
                                        {
                                            convs = "1";
                                        }
                                        qt = dssubitem1.Tables[0].Rows[g]["Quantity"].ToString();
                                        if (qt == "")
                                        {
                                            qt = "0";
                                        }
                                        quantity = Convert.ToDouble(qt);
                                        qt = dssubitem1.Tables[0].Rows[g]["SubRecipeQuantity"].ToString();
                                        if (qt == "")
                                        {
                                            qt = "0";
                                        }
                                        quantity = quantity * Convert.ToDouble(qt);
                                        quantity = Math.Round(quantity, 3);
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
                                            amount = amount / Convert.ToDouble(convs);
                                        }
                                        catch (Exception ex)
                                        {


                                        }

                                        subname = attachmenuname + "-" + dssubitem1.Tables[0].Rows[g]["name"].ToString();
                                        if (dssubitem1.Tables[0].Rows[g]["ItemName"].ToString().Length > 0)
                                        {
                                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menugroup"].ToString(), name + "(" + mprice + ")", "(" + subname + ")" + dssubitem1.Tables[0].Rows[g]["ItemName"].ToString(), dssubitem1.Tables[0].Rows[g]["UOM"].ToString(), quantity, amount, quantity * amount, dssubitem1.Tables[0].Rows[g]["SubRecipetype"].ToString());
                                            
                                        }

                                    }
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

                    try
                    {

                        q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.Type, dbo.SubItems.Name, dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName,                          dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.AttachRecipe.Type='RuntimeModifier' and dbo.AttachRecipe.Menuitemid='" + ds.Tables[0].Rows[i]["RId"].ToString() + "'";

                        DataSet dssubitem = new DataSet();
                        dssubitem = objCore.funGetDataSet(q);
                        for (int h = 0; h < dssubitem.Tables[0].Rows.Count; h++)
                        {
                            price = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), dssubitem.Tables[0].Rows[h]["RawItemId"].ToString()).ToString();
                            if (price == "")
                            {
                                price = "0";
                            }
                            convs = dssubitem.Tables[0].Rows[h]["ConversionRate"].ToString();
                            if (convs == "")
                            {
                                convs = "1";
                            }
                            qt = dssubitem.Tables[0].Rows[h]["Quantity"].ToString();
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
                            quantity = Math.Round(quantity, 4);
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
                                amount = amount / Convert.ToDouble(convs);
                            }
                            catch (Exception ex)
                            {


                            }

                            string subname = dssubitem.Tables[0].Rows[h]["name"].ToString();

                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["menu"].ToString(), name + "(" + mprice + ")", "(" + subname + ")" + dssubitem.Tables[0].Rows[h]["ItemName"].ToString(), dssubitem.Tables[0].Rows[h]["UOM"].ToString(), quantity, amount, quantity * amount, dssubitem.Tables[0].Rows[h]["SubRecipetype"].ToString());

                        }
                    }
                    catch (Exception ex)
                    {


                    }



                }
                dtrpt.DefaultView.Sort = "Item";
                dtrpt = dtrpt.DefaultView.ToTable();
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
            menuid = "";
            bindreport();
        }

        private void cmbmenu_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,name from menuitem where status='active' and menugroupid='" + cmbgroup.SelectedValue + "' order by name";
                if (cmbgroup.Text == "All")
                {
                    q = "select id,name from menuitem where status='active'";
                }
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["name"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbmenu.DataSource = dsi.Tables[0];
                cmbmenu.ValueMember = "id";
                cmbmenu.DisplayMember = "name";
                cmbmenu.Text = "All";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
