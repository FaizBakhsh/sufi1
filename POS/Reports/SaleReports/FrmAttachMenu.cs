
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports.SaleReports
{
    public partial class FrmAttachMenu : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public FrmAttachMenu()
        {
            InitializeComponent();
        }

        private void FrmMwnuGroupSale_Load(object sender, EventArgs e)
        {
            try
            {
              DataSet  ds = new DataSet();
                string q = "select id,name from menugroup ";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "All";
                ds.Tables[0].Rows.Add(dr);
                cmbgroup.DataSource = ds.Tables[0];
                cmbgroup.ValueMember = "id";
                cmbgroup.DisplayMember = "name";
                cmbgroup.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
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


                POSRestaurant.Reports.SaleReports.rptAttachmenu rptDoc = new rptAttachmenu();
                POSRestaurant.Reports.SaleReports.dsAttachmenu dsrpt = new  dsAttachmenu();
                //feereport ds = new feereport(); // .xsd file name


                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt = getAllOrders();

                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);

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
            List<string> menulist = new List<string>();

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("Name", typeof(string));
                dtrpt.Columns.Add("AttachedName", typeof(string));
                dtrpt.Columns.Add("Quantity", typeof(double));
                dtrpt.Columns.Add("Type", typeof(string));
                dtrpt.Columns.Add("Modifier", typeof(string));
                DataSet ds = new DataSet();
                string q = "";
                if (cmbgroup.Text == "All")
                {
                    if (textBox1.Text == "")
                    {
                        q = "SELECT        dbo.Attachmenu1.id, dbo.MenuItem.Price,  dbo.MenuItem.Id as menuid, dbo.MenuItem.Name AS Deal, dbo.ModifierFlavour.name AS size, MenuItem_1.Name, dbo.Attachmenu1.userecipe, dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem AS MenuItem_1 ON dbo.Attachmenu1.attachmenuid = MenuItem_1.Id INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.menuitemid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Attachmenu1.Type = 'MenuItem') OR                         (dbo.Attachmenu1.Type IS NULL)";

                    }
                    else
                    {
                        q = "SELECT        dbo.Attachmenu1.id, dbo.MenuItem.Price,  dbo.MenuItem.Id as menuid,dbo.MenuItem.Name AS Deal, dbo.ModifierFlavour.name AS size, MenuItem_1.Name, dbo.Attachmenu1.userecipe, dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem AS MenuItem_1 ON dbo.Attachmenu1.attachmenuid = MenuItem_1.Id INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.menuitemid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id WHERE   ( dbo.MenuItem.Name like '%" + textBox1.Text + "%'  and  ((dbo.Attachmenu1.Type = 'MenuItem') OR                         (dbo.Attachmenu1.Type IS NULL))) or  ( dbo.MenuItem_1.Name like '%" + textBox1.Text + "%'  and  ((dbo.Attachmenu1.Type = 'MenuItem') OR                         (dbo.Attachmenu1.Type IS NULL))) ";
                    }
                }
                else
                {
                    if (textBox1.Text == "")
                    {
                        q = "SELECT        dbo.Attachmenu1.id, dbo.MenuItem.Price,  dbo.MenuItem.Id as menuid,dbo.MenuItem.Name AS Deal, dbo.ModifierFlavour.name AS size, MenuItem_1.Name, dbo.Attachmenu1.userecipe, dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem AS MenuItem_1 ON dbo.Attachmenu1.attachmenuid = MenuItem_1.Id INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.menuitemid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id WHERE    ( dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "') and  ( (dbo.Attachmenu1.Type = 'MenuItem') OR                         (dbo.Attachmenu1.Type IS NULL))";

                    }
                    else
                    {
                        q = "SELECT        dbo.Attachmenu1.id, dbo.MenuItem.Price,  dbo.MenuItem.Id as menuid,dbo.MenuItem.Name AS Deal, dbo.ModifierFlavour.name AS size, MenuItem_1.Name, dbo.Attachmenu1.userecipe, dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem AS MenuItem_1 ON dbo.Attachmenu1.attachmenuid = MenuItem_1.Id INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.menuitemid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id WHERE   ( ( dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "') and  dbo.MenuItem.Name like '%" + textBox1.Text + "%'  and  ((dbo.Attachmenu1.Type = 'MenuItem') OR                         (dbo.Attachmenu1.Type IS NULL))) or  (  ( dbo.MenuItem.menugroupid='" + cmbgroup.SelectedValue + "') and dbo.MenuItem_1.Name like '%" + textBox1.Text + "%'  and  ((dbo.Attachmenu1.Type = 'MenuItem') OR                         (dbo.Attachmenu1.Type IS NULL)) )";
                    }
                }
                ds = objCore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string val = "";


                  

                    string menuid = ds.Tables[0].Rows[i]["menuid"].ToString();
                    string price = ds.Tables[0].Rows[i]["Price"].ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    string name = "";
                    string deal = ds.Tables[0].Rows[i]["Deal"].ToString();
                    string flavour = ds.Tables[0].Rows[i]["size"].ToString();
                    string menu = ds.Tables[0].Rows[i]["Name"].ToString();
                    if (flavour.Trim().Length > 0)
                    {
                        name = flavour + "' " + menu;
                    }
                    else
                    {
                        name = menu;
                    }

                    dtrpt.Rows.Add(deal + " (" + price + ")", name, ds.Tables[0].Rows[i]["Quantity"].ToString(), "Menu Item","");

                    try
                    {
                        bool chk = menulist.Any(x => x == menuid);
                        if (chk == false)
                        {
                            menulist.Add(menuid);
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }
                
                {
                    
                    {




                        q = "SELECT        dbo.Attachmenu1.id, dbo.ModifierFlavour.name AS size, MenuItem_1.Name, dbo.MenuItem.Price, dbo.Attachmenu1.userecipe, dbo.Attachmenu1.Quantity, dbo.Attachmenu1.status, dbo.RuntimeModifier.name AS Rname,                          dbo.RuntimeModifier.menuItemid, dbo.MenuItem.Name AS MenuName FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem AS MenuItem_1 ON dbo.Attachmenu1.attachmenuid = MenuItem_1.Id INNER JOIN                         dbo.RuntimeModifier ON dbo.Attachmenu1.menuitemid = dbo.RuntimeModifier.id INNER JOIN                         dbo.MenuItem ON dbo.RuntimeModifier.menuItemid = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Attachmenu1.attachFlavourid = dbo.ModifierFlavour.Id WHERE        (dbo.Attachmenu1.Type = 'RuntimeModifier')";
                        DataSet dsitems = new DataSet();
                        dsitems = objCore.funGetDataSet(q);
                        for (int j = 0; j < dsitems.Tables[0].Rows.Count; j++)
                        {

                            string runtimename = dsitems.Tables[0].Rows[j]["Rname"].ToString();
                            string deal = dsitems.Tables[0].Rows[j]["MenuName"].ToString();
                            string price = dsitems.Tables[0].Rows[j]["Price"].ToString();
                            if (price == "")
                            {
                                price = "0";
                            }
                            string name = "";

                            string flavour = dsitems.Tables[0].Rows[j]["size"].ToString();
                            string menu = dsitems.Tables[0].Rows[j]["Name"].ToString();
                            if (flavour.Trim().Length > 0)
                            {
                                name = flavour + "' " + menu;
                            }
                            else
                            {
                                name = menu;
                            }

                            dtrpt.Rows.Add(deal + " (" + price + ")", name, dsitems.Tables[0].Rows[j]["Quantity"].ToString(), "Runtime Modifier",runtimename);

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
        private void vButton1_Click(object sender, EventArgs e)
        {
            bindreport();
        }

        private void crystalReportViewer1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void crystalReportViewer1_Click(object sender, EventArgs e)
        {
            
        }

        private void crystalReportViewer1_ClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
           
        }

        private void crystalReportViewer1_DoubleClickPage(object sender, CrystalDecisions.Windows.Forms.PageMouseEventArgs e)
        {
            
        }
    }
}
