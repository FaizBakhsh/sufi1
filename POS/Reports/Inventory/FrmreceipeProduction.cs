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
    public partial class FrmreceipeProduction : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmreceipeProduction()
        {
            InitializeComponent();
        }
        public string date1 = "", date2 = "", menuid = "";
        private void frmClosingInventory_Load(object sender, EventArgs e)
        {
          
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
                string id = "";
                bool chk = true;


                if (textBox1.Text.Trim() == "")
                {

                    q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.price, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.MenuItem.status='Active' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                    q = "SELECT        dbo.RawItem.ItemName AS Name, dbo.RawItem.Id, RawItem_1.ItemName, dbo.UOMConversion.UOM, dbo.RecipeProduction.Quantity, dbo.RecipeProduction.RawItemId, dbo.UOMConversion.ConversionRate FROM            dbo.RecipeProduction INNER JOIN                         dbo.RawItem ON dbo.RecipeProduction.ItemId = dbo.RawItem.Id INNER JOIN                         dbo.RawItem AS RawItem_1 ON dbo.RecipeProduction.RawItemId = RawItem_1.Id INNER JOIN                         dbo.UOMConversion ON RawItem_1.UOMId = dbo.UOMConversion.UOMId";
                }
                else
                {

                    q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName,dbo.RawItem.id,dbo.RawItem.price, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.MenuItem.status='Active' and dbo.MenuItem.Name like '" + textBox1.Text + "' or  dbo.MenuItem.status='Active' and dbo.RawItem.ItemName  like '" + textBox1.Text + "'  ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                    q = "SELECT        dbo.RawItem.ItemName AS Name, dbo.RawItem.Id, RawItem_1.ItemName, dbo.UOMConversion.UOM, dbo.RecipeProduction.Quantity, dbo.RecipeProduction.RawItemId, dbo.UOMConversion.ConversionRate FROM            dbo.RecipeProduction INNER JOIN                         dbo.RawItem ON dbo.RecipeProduction.ItemId = dbo.RawItem.Id INNER JOIN                         dbo.RawItem AS RawItem_1 ON dbo.RecipeProduction.RawItemId = RawItem_1.Id INNER JOIN                         dbo.UOMConversion ON RawItem_1.UOMId = dbo.UOMConversion.UOMId where  dbo.RawItem.ItemName  like '%" + textBox1.Text + "%' ";

                }

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    double mprice = 0;
                    string temp = "0";

                    temp = "0";

                    mprice = mprice + Convert.ToDouble(temp);
                    mprice = Math.Round(mprice, 2);
                    if (id != ds.Tables[0].Rows[i]["Id"].ToString())
                    {
                        chk = true;
                    }
                    string price = getcostmenu1(DateTime.Now.ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"), ds.Tables[0].Rows[i]["RawItemId"].ToString()).ToString();
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
                        qty = qty / Convert.ToDouble(convs);
                    }
                    catch (Exception ex)
                    {


                    }
                    qty = Math.Round(qty, 4);
                    string name = ds.Tables[0].Rows[i]["name"].ToString();

                    dtrpt.Rows.Add("", name, ds.Tables[0].Rows[i]["ItemName"].ToString(), ds.Tables[0].Rows[i]["UOM"].ToString(), qty, amount, qty * amount, "");
                    if (chk == true)
                    {
                        chk = false;
                        try
                        {
                            
                                q = "SELECT        dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.type AS SubRecipetype, dbo.UOMConversion.UOM, dbo.AttachRecipe.FlavourId, dbo.AttachRecipe.Quantity, dbo.AttachRecipe.Type, dbo.SubItems.Name, dbo.SubRecipe.Quantity AS SubRecipeQuantity, dbo.RawItem.ItemName,                          dbo.SubRecipe.RawItemId, dbo.UOMConversion.ConversionRate FROM            dbo.AttachRecipe INNER JOIN                         dbo.SubItems ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId INNER JOIN                         dbo.RawItem ON dbo.SubRecipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.AttachRecipe.Type='Production' and dbo.AttachRecipe.Menuitemid='" + ds.Tables[0].Rows[i]["Id"].ToString() + "'";
                           
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
                                    quantity = quantity / Convert.ToDouble(convs);
                                }
                                catch (Exception ex)
                                {


                                }

                                string subname = dssubitem.Tables[0].Rows[h]["name"].ToString();
                               
                  
                                dtrpt.Rows.Add("", name, "(" + subname + ")" + dssubitem.Tables[0].Rows[h]["ItemName"].ToString(), dssubitem.Tables[0].Rows[h]["UOM"].ToString(), quantity, amount, quantity * amount, dssubitem.Tables[0].Rows[h]["SubRecipetype"].ToString());

                            }
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    id = ds.Tables[0].Rows[i]["id"].ToString();
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
            menuid = "";
            bindreport();
        }
    }
}
