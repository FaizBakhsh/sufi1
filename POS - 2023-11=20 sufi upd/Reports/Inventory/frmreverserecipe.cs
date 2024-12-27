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
    public partial class frmreverserecipe : Form
    {
        public string id = "";
        public frmreverserecipe()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        private void frmreverserecipe_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet dsi = new DataSet();
                string q = "select id,Itemname from Rawitem order by Itemname";
                dsi = objCore.funGetDataSet(q);
                DataRow dr = dsi.Tables[0].NewRow();
                dr["Itemname"] = "All";
                dsi.Tables[0].Rows.Add(dr);
                cmbgroup.DataSource = dsi.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "Itemname";
                cmbgroup.Text = "All";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            if (id.Length > 0)
            {
                vButton1.Text = "Please Wait";
                vButton1.Enabled = false;
                bindreport();
                vButton1.Text = "View";
                vButton1.Enabled = true;
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            id = "";
            bindreport();
        }
        public void bindreport()
        {

            try
            {
                DataTable dt = new DataTable();
                POSRestaurant.Reports.Inventory.rptreverserecipe rptDoc = new  rptreverserecipe();
                POSRestaurant.Reports.Inventory.dsreverserecipe dsrpt = new dsreverserecipe();
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
        public DataTable getAllOrders()
        {
            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("MenuItem", typeof(string));
                dtrpt.Columns.Add("Size", typeof(string));
                dtrpt.Columns.Add("Unit", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Type", typeof(string));
                dtrpt.Columns.Add("Kitchen", typeof(string));
                DataSet ds = new DataSet();
                string q = "", date = "";             
                bool chk = true;
                if (id.Length > 0)
                {
                    q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.Recipe.Quantity, dbo.Recipe.type,                          dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.KDS.Name AS KDS FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.Recipe.rawitemid='" + id + "' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";                  
                }
                else
                {
                    if (cmbgroup.Text == "All")
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.Recipe.Quantity, dbo.Recipe.type,                          dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.KDS.Name AS KDS FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id   ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                    }
                    else
                    {
                        q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.Recipe.Quantity, dbo.Recipe.type,                          dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.KDS.Name AS KDS FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.Recipe.rawitemid='" + cmbgroup.SelectedValue + "' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                    }
                }

                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    
                    string name = ds.Tables[0].Rows[i]["name"].ToString();
                    if (name.Length > 0)
                    {
                        name = name + "-";
                    }
                    name = name + ds.Tables[0].Rows[i]["menu"].ToString();
                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["ItemName"].ToString(), name, "", ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["type"].ToString(), "(" + ds.Tables[0].Rows[i]["kds"].ToString() + ")");

                }

                try
                {
                    if (id.Length > 0)
                    {
                        //q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.Recipe.Quantity, dbo.Recipe.type,                          dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.KDS.Name AS KDS FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.Recipe.rawitemid='" + id + "' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                    }
                    else
                    {
                        if (cmbgroup.Text == "All")
                        {
                            q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.Recipe.Quantity, dbo.Recipe.type,                          dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.KDS.Name AS KDS FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id   ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                            q = "SELECT        dbo.RawItem.ItemName, RawItem_1.ItemName AS RawItemName, dbo.UOMConversion.UOM, dbo.RecipeProduction.Quantity FROM            dbo.RawItem AS RawItem_1 INNER JOIN                         dbo.RecipeProduction INNER JOIN                         dbo.RawItem ON dbo.RecipeProduction.ItemId = dbo.RawItem.Id ON RawItem_1.Id = dbo.RecipeProduction.RawItemId INNER JOIN                         dbo.UOMConversion INNER JOIN                         dbo.UOM ON dbo.UOMConversion.UOMId = dbo.UOM.Id ON RawItem_1.UOMId = dbo.UOM.Id";
                        }
                        else
                        {
                            q = "SELECT        TOP (100) PERCENT dbo.ModifierFlavour.name, dbo.MenuItem.Name AS menu, dbo.UOMConversion.ConversionRate, dbo.RawItem.ItemName, dbo.RawItem.Id, dbo.RawItem.Price, dbo.Recipe.Quantity, dbo.Recipe.type,                          dbo.Recipe.MenuItemId, dbo.MenuGroup.Name AS menugroup, dbo.UOMConversion.UOM, dbo.MenuItem.Price AS mprice, dbo.ModifierFlavour.price AS fprice, dbo.KDS.Name AS KDS FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.MenuItem ON dbo.Recipe.MenuItemId = dbo.MenuItem.Id INNER JOIN                         dbo.MenuGroup ON dbo.MenuItem.MenuGroupId = dbo.MenuGroup.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Recipe.modifierid = dbo.ModifierFlavour.Id where dbo.Recipe.rawitemid='" + cmbgroup.SelectedValue + "' ORDER BY menugroup, dbo.Recipe.MenuItemId, menu, dbo.ModifierFlavour.name";
                            q = "SELECT        dbo.RawItem.ItemName, RawItem_1.ItemName AS RawItemName, dbo.UOMConversion.UOM, dbo.RecipeProduction.Quantity FROM            dbo.RawItem AS RawItem_1 INNER JOIN                         dbo.RecipeProduction INNER JOIN                         dbo.RawItem ON dbo.RecipeProduction.ItemId = dbo.RawItem.Id ON RawItem_1.Id = dbo.RecipeProduction.RawItemId INNER JOIN                         dbo.UOMConversion INNER JOIN                         dbo.UOM ON dbo.UOMConversion.UOMId = dbo.UOM.Id ON RawItem_1.UOMId = dbo.UOM.Id  where dbo.RecipeProduction .rawitemid='" + cmbgroup.SelectedValue + "' ";

                        }
                    }
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        string name = ds.Tables[0].Rows[i]["ItemName"].ToString();


                        dtrpt.Rows.Add(ds.Tables[0].Rows[i]["RawItemName"].ToString(), name, "", ds.Tables[0].Rows[i]["UOM"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "Production");

                    }
                }
                catch (Exception ex)
                {
                    
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
    }
}
