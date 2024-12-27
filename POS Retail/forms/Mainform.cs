using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POSRetail.classes;
using POSRetail.admin;
using System.Data.SqlClient;

using POSRetail.ControlPanel;

namespace POSRetail.forms
{
    public partial class MainForm : Form
    {

        public delegate void MsgHandler(string msg);
        MsgHandler modDbAction;
        public MainForm()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
        }
        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        
        void businessinfo()
        {
           
            POSRetail.classes.Clsdbcon objCore = new Clsdbcon();
            SqlDataReader dr = objCore.funGetDataReader("SELECT InfoId, CompanyName, PhoneNo, Email, Web, Address, Logo FROM  BUSINESINFO");
             if (dr.HasRows)
                {
                    dr.Read();
                   // Org_Name.Text = dr["CompanyName"].ToString();
                   // lblAddress.Text = dr["Address"].ToString();
                   // lblBusinessDesc.Text = dr["PhoneNo"].ToString();
                }
                    else
                    {
                 dr.Close();
                    }
                }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            this.WindowState = FormWindowState.Normal;
            if (dataGridView1.Rows.Count > 0)
            {
                vButton2.Visible = true;
                vButton3.Visible = true;
                vButton1.Visible = true;
                label1.Visible = true;
            }
            else
            {
                vButton2.Visible = false;
                vButton3.Visible = false;
                vButton1.Visible = false;
                label1.Visible = false;
            }
          ///  btnSale.FlatAppearance.BorderColor = Color.White;
           // btnpurchase.FlatAppearance.BorderColor = Color.White;
//btnAddcus.FlatAppearance.BorderColor = Color.White;
            //btnAddItem.FlatAppearance.BorderColor = Color.White;
         //   btnAddSupp.FlatAppearance.BorderColor = Color.White;
           // businessinfo();


        }

        private void mnuFrontDesk_Click(object sender, EventArgs e)
        {


        }


        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            showLogin();

            //CHECK THE RESTRICTIONS OF THE USER
            if (!clsUserSecurity.GetUser().IsAdmin)
            {
                //menuStrip.Visible = false;
                //toolBar.Visible = false;
            }
            //menuStrip.Visible = true;
           // toolBar.Visible = true;
        }

        private void showLogin()
        {
            POSRetail.forms.login1 log = new login1();
            log.ShowDialog();
            //if (!log.isValid) { Application.Exit(); }

            //statUser.Text = log.getUser;
        }


        private void newAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void userAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmSettings settings = new frmSettings();
            //settings.ShowDialog();
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            //frmAllCustomer customer = new frmAllCustomer();
            //customer.MdiParent = MainForm.ActiveForm;
            //customer.AddCustomer();
        }


        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult repz = MessageBox.Show("Are you sure you want to exit?",
            //   "Exit Application [MBL].......", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (repz == DialogResult.Yes)
            //{
            //    this.Dispose();
            //  //  Clsdbcon.con.Close();
            //    Application.Exit();
            //}
            //else
            //{ e.Cancel = true; }
        }

       
        private void addSupplier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            POSRetail.admin.frmSupplierManagement obj = new frmSupplierManagement();
            obj.MdiParent = this;
            obj.Show();

        }
        private void supplierManagement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            POSRetail.admin.frmSupplierManagement SupManagement = new POSRetail.admin.frmSupplierManagement();
            SupManagement.MdiParent = this.MdiParent;
            SupManagement.WindowState = FormWindowState.Maximized;
            SupManagement.Show();
        }
        
        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.admin.frmAddItemrecipe AddCategory = new POSRetail.admin.frmAddItemrecipe();
            AddCategory.Show();
        }

        private void manageCatergoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.admin.frmItemrecipemang ManageCatgory = new POSRetail.admin.frmItemrecipemang();
            ManageCatgory.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            DialogResult repz = MessageBox.Show("Are you sure you want to exit?",
              "Exit Application [MBL].......", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (repz == DialogResult.Yes)
            {
                this.Dispose();
                //Clsdbcon.con.Close();
                Application.Exit();
            }

        }

       
        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmItemrecipemang addItemcategory = new frmItemrecipemang();
            addItemcategory.ShowDialog();
        }
        public void getdata(string qry)
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new Clsdbcon();
                DataSet ds = new DataSet();
                if (label1.Text == "2.5 Recipe")
                {
                    DataTable dtgv = new DataTable();
                    DataColumn dcol1=new DataColumn("id",typeof(string));
                    DataColumn dcol2 = new DataColumn("Name", typeof(string));
                    DataColumn dcol3 = new DataColumn("UOM", typeof(string));
                    DataColumn dcol4 = new DataColumn("Quantity", typeof(string));
                    DataColumn dcol5 = new DataColumn("Cost", typeof(string));
                    dtgv.Columns.Add(dcol1);
                    dtgv.Columns.Add(dcol2);
                    dtgv.Columns.Add(dcol3);
                    dtgv.Columns.Add(dcol4);
                    dtgv.Columns.Add(dcol5);
                   // DataColumn dcol6 = new DataColumn("id", typeof(string));
                    ds = objCore.funGetDataSet("SELECT * from menuitem");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            dtgv.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), "", "", "");
                           // DataRowState dr = dtgv.NewRow(ds.Tables[0].Rows[i]["id"].ToString(),ds.Tables[0].Rows[i]["Name"].ToString(),"","","");
                            DataSet dsrawitm = new DataSet();
                            string qr = "SELECT     dbo.Recipe.Id, dbo.RawItem.ItemName, dbo.Recipe.Quantity, dbo.Recipe.UOM, dbo.Recipe.Cost FROM         dbo.Recipe INNER JOIN                      dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id where dbo.Recipe.MenuItemId='" + ds.Tables[0].Rows[i]["Id"].ToString() + "'";
                            dsrawitm = objCore.funGetDataSet(qr);
                            if (dsrawitm.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsrawitm.Tables[0].Rows.Count; j++)
                                {
                                    dtgv.Rows.Add("id", "Name", "UOM", "Quantity", "Cost");
                                    dtgv.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), dsrawitm.Tables[0].Rows[j]["ItemName"].ToString(), dsrawitm.Tables[0].Rows[j]["UOM"].ToString(), dsrawitm.Tables[0].Rows[j]["Quantity"].ToString(), dsrawitm.Tables[0].Rows[j]["cost"].ToString());
                                }
                            }
                            else
                            {
                                dtgv.Rows.Add("", "No Recipe of this Menu item Exist", "", "", "");

                          
                            }
                            dtgv.Rows.Add("", "", "", "", "");
                        }
                    }
                    dataGridView1.DataSource = dtgv;

                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.RowHeadersVisible = false;
                    //DataGridViewColumn column = dataGridView1.Columns[1];
                    //column.Width = 240;

                    // column.HeaderText = htext;
                    //DataGridViewColumn column1 = dataGridView1.Columns[2];
                    //column1.Width = 290;
                    if (dataGridView1.Rows.Count > 0)
                    {
                        vButton1.Visible = true;
                        vButton2.Visible = true;
                        vButton3.Visible = true;
                        label1.Visible = true;
                    }
                    dataGridView1.Refresh();
                    dataGridView1.Update();
                    return;
                }
                ds = objCore.funGetDataSet(qry);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //dataGridView1.DataSource = ds.Tables[0];
                }
                dataGridView1.DataSource = ds.Tables[0];
                
                dataGridView1.Columns[0].Visible = false;
                //DataGridViewColumn column = dataGridView1.Columns[1];
                //column.Width = 240;

                // column.HeaderText = htext;
                //DataGridViewColumn column1 = dataGridView1.Columns[2];
                //column1.Width = 290;
                if (dataGridView1.Rows.Count > 0)
                {
                    vButton1.Visible = true;
                    vButton2.Visible = true;
                    vButton3.Visible = true;
                    label1.Visible = true;
                }
                dataGridView1.Refresh();
                dataGridView1.Update();
            }
            catch(Exception ex)
            {
                
            }
        }

        private void listBar1_ItemClicked(object sender, NETXP.Controls.Bars.ItemClickedEventArgs e)
        {
            try
            {
                string val = e.Item.Caption.ToString();
                string authentication = objCore.authentication(val);
                if (authentication == "yes")
                {

                }
                else
                {
                    MessageBox.Show("You are not allowed to view this");
                    return;
                }
                switch (int.Parse(e.Item.Tag.ToString()))
                {
                    case 1:
                       
                        label1.Text = "1.1 Group";

                        

                        string q = "select * from Groups";
                        getdata(q);
                        break;
                    
                    case 2:
                        label1.Text = "1.2 Category";

                        q = "SELECT     dbo.Category.Id, dbo.Groups.GroupName, dbo.Category.CategoryName, dbo.Category.CategoryDiscription FROM         dbo.Groups INNER JOIN                      dbo.Category ON dbo.Groups.Id = dbo.Category.GroupId";
                        getdata(q);
                        
                        break;
                    case 3:
                        label1.Text = "1.3 Type";

                        q = "SELECT     dbo.Type.Id, dbo.Groups.GroupName, dbo.Category.CategoryName, dbo.Type.TypeName, dbo.Type.TypeDiscription FROM         dbo.Groups INNER JOIN                      dbo.Category ON dbo.Groups.Id = dbo.Category.GroupId INNER JOIN                      dbo.Type ON dbo.Groups.Id = dbo.Type.groupId AND dbo.Category.Id = dbo.Type.CategoryId";
                        getdata(q);

                        break;
                    case 4:
                        label1.Text = "1.4 Brand";

                        q = "SELECT * from Brands";
                        getdata(q);

                        break;
                    case 5:
                        label1.Text = "1.5 UOM";

                        q = "SELECT * from UOM";
                        getdata(q);

                        break;

                    case 6:
                        label1.Text = "1.6 Size";

                        q = "SELECT * from Size";
                        getdata(q);
                        
                        break;
                    case 7:
                        label1.Text = "1.7 Color";

                        q = "select Id,  Caption as Color_Name,ColorName as Color_Code,  UploadStatus from Color";
                        getdata(q);
                        
                        break;
                    case 8:
                       label1.Text = "1.8 Branch";

                       q = "SELECT * from Branch";
                        getdata(q);
                        break;
                    case 9:
                       label1.Text = "1.9 Store";

                       q = "SELECT     dbo.Stores.Id, dbo.Stores.StoreName, dbo.Stores.StoreCode, dbo.Branch.BranchName FROM         dbo.Branch INNER JOIN                      dbo.Stores ON dbo.Branch.Id = dbo.Stores.BranchId";
                        getdata(q);
                        break;
                    //case 10:
                    //    label1.Text = "1.10 UOM Conversion";

                    //   q = "SELECT     dbo.UOMConversion.Id, dbo.UOM.UOM, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM AS Converted_UOM FROM         dbo.UOM INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId";
                    //    getdata(q);
                        
                    //    break;
                    //case 11:
                    //    label1.Text = "2.1 Menu Group";

                    //    q = "SELECT     dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.Color.ColorName, dbo.MenuGroup.Description, dbo.MenuGroup.Status FROM         dbo.MenuGroup INNER JOIN                      dbo.Color ON dbo.MenuGroup.ColorId = dbo.Color.Id";
                    //    getdata(q);
                    //    break;
                    //case 12:
                    //    label1.Text = "2.2 Menu Item";

                    //    q = "SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status, dbo.Color.ColorName FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id";
                    //    getdata(q);
                    //    break;
                    case 13:
                        label1.Text = "2.1 Currency";

                        q = "SELECT   * from Currency";
                        getdata(q);
                        break;
                    case 14:
                         label1.Text = "2.2 MOP";

                        q = "SELECT   * from MOP";
                        getdata(q);
                        break;
                    case 15:
                        label1.Text = "2.3 Printer Setting";

                        q = "SELECT   * from Printers";
                        getdata(q);

                        break;
                    //case 16:
                    //    label1.Text = "2.4 Recipe Modifier";
                    //    Setting.AddModifier obj = new Setting.AddModifier(this);
                    //    obj.Show();
                    //    //q = "SELECT   * from MenuItem";
                    //    //getdata(q);
                       
                    //    break;
                    case 17:
                        label1.Text = "2.4 GST";

                        q = "SELECT   * from GST";
                        getdata(q);
                        break;
                    case 18:
                        label1.Text = "2.5 Device Setting";

                        q = "SELECT   * from  DeviceSetting";
                        getdata(q);
                        break;
                    case 19:
                        label1.Text = "2.6 Online DB Setting";

                        q = "SELECT   * from  SqlServerInfo";
                        getdata(q);
                        break;
                    //case 20:
                    //    label1.Text = "2.7 Layout Setting";

                    //    q = "SELECT Id,tablename AS table_Name, Columns, Rows  FROM Tablelayout";
                    //    getdata(q);
                    //    break;
                    case 21:
                        label1.Text = "1.10 Supplier";

                        q = "SELECT     dbo.Supplier.Id, dbo.Supplier.Name, dbo.Supplier.Code, dbo.Supplier.CNICNo, dbo.Supplier.City, dbo.Supplier.Address, dbo.Supplier.Phone, dbo.Supplier.Mobile,                       dbo.Supplier.Area, dbo.Supplier.Date, dbo.Supplier.payableaccountid, dbo.Supplier.uploadstatus, dbo.ChartofAccounts.Name AS PayableAccount FROM         dbo.Supplier LEFT OUTER JOIN                      dbo.ChartofAccounts ON dbo.Supplier.payableaccountid = dbo.ChartofAccounts.Id";
                        getdata(q);
                        break;
                    case 31:
                        label1.Text = "3.1 Company Info";

                        q = "select  Id, Name, Address, Phone, uploadstatus from CompanyInfo";
                        getdata(q);
                        break;
                    case 41:
                        label1.Text = "4.1 Users";

                        q = "SELECT * from Users";
                        getdata(q);
                        break;
                    case 277:
                        label1.Text = "2.7 Cash Sales Account";

                        q = "SELECT     dbo.CashSalesAccountsList.Id, dbo.ChartofAccounts.Name, dbo.CashSalesAccountsList.AccountType, dbo.CashSalesAccountsList.UploadStatus FROM         dbo.CashSalesAccountsList INNER JOIN                      dbo.ChartofAccounts ON dbo.CashSalesAccountsList.ChartaccountId = dbo.ChartofAccounts.Id";
                        getdata(q);
                        break;
                        case 51:
                        label1.Text = "5.1 Chart of Accounts Codes";

                        q = "SELECT * from ChartofAccountsCodes";
                        getdata(q);
                        break;
                        case 52:
                        label1.Text = "5.2 Customers";

                        q = "SELECT     dbo.Customers.Id, dbo.Customers.Name, dbo.Customers.Email, dbo.Customers.Phone, dbo.Customers.Mobile, dbo.Customers.City, dbo.Customers.Address, dbo.ChartofAccounts.Name AS Account FROM         dbo.ChartofAccounts INNER JOIN                      dbo.Customers ON dbo.ChartofAccounts.Id = dbo.Customers.Chartaccountid";
                        getdata(q);
                        break;
                    default:
                        ///  reportViewr.Visible = false;
                        break;
                }
            }
            catch
            { 
            }
        }

        

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ControlPanel.frmGlobalSettings settings = new POSRetail.ControlPanel.frmGlobalSettings();
            settings.ShowDialog();
        }

        
        private void addSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.admin.frmSupplierAdd obj = new frmSupplierAdd();
            obj.MdiParent = this.MdiParent;
            obj.Show();

        }

        private void subCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.admin.frmAddItemSubCategory AddCategory = new POSRetail.admin.frmAddItemSubCategory();
            AddCategory.Show();
        }

       
        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {

            POSRetail.ControlPanel.frmManageUsers Manageuser = new frmManageUsers();
            Manageuser.MdiParent = this.MdiParent;
            Manageuser.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            POSRetail.admin.frmItemSubCategory ManageCatgory = new POSRetail.admin.frmItemSubCategory();
            ManageCatgory.Show();
        }

        private void exitToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DialogResult repz = MessageBox.Show("Are you sure you want to exit?",
             "Exit Application [].......", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (repz == DialogResult.Yes)
            {
                this.Dispose();
                //Clsdbcon.con.Close();
                Application.Exit();
            }
        }

        private void btnAddcus_Click(object sender, EventArgs e)
        {
            backup objb = new backup();
            objb.Show();
        }

        private void btnAddSupp_Click(object sender, EventArgs e)
        {
            frmDishmanagement objds = new frmDishmanagement();
            objds.Show();
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }
        public void addfunc(string tablename)
        {
            if (tablename == "group")
            {
                POSRetail.Setting.AddGroups obadd = new  Setting.AddGroups(this);
                obadd.MdiParent = this.MdiParent;
               // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Category")
            {
                POSRetail.Setting.AddCategory obadd = new Setting.AddCategory(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Type")
            {
                POSRetail.Setting.AddType obadd = new Setting.AddType(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Brand")
            {
                POSRetail.Setting.Addbrand obadd = new Setting.Addbrand(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "UOM")
            {
                POSRetail.Setting.Adduom obadd = new Setting.Adduom(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Size")
            {
                POSRetail.Setting.Addsize obadd = new Setting.Addsize(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Color")
            {
                POSRetail.Setting.Addcolor obadd = new Setting.Addcolor(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Branch")
            {
                POSRetail.Setting.Addbranch obadd = new Setting.Addbranch(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Store")
            {
                POSRetail.Setting.AddStore obadd = new Setting.AddStore(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "UOMC")
            {
                POSRetail.Setting.Addcuom obadd = new Setting.Addcuom(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "MenuG")
            {
                POSRetail.Setting.AddMenuG obadd = new Setting.AddMenuG(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "MenuI")
            {
                POSRetail.Setting.AddMenuItem obadd = new Setting.AddMenuItem(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Currency")
            {
                POSRetail.Setting.AddCurrency obadd = new Setting.AddCurrency(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "MOP")
            {
                POSRetail.Setting.AddMOP obadd = new Setting.AddMOP(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "GST")
            {
                POSRetail.Setting.AddGST obadd = new Setting.AddGST(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Printer")
            {
                POSRetail.Setting.AddPrinter obadd = new Setting.AddPrinter(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "Device")
            {
                POSRetail.Setting.DevicesSetting obadd = new Setting.DevicesSetting(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "db")
            {
                POSRetail.Setting.AddSQLInfo obadd = new Setting.AddSQLInfo(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "layout")
            {
                POSRetail.Setting.AddLayout obadd = new Setting.AddLayout(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "supplier")
            {
                POSRetail.Setting.AddSupplier obadd = new Setting.AddSupplier(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "users")
            {
                POSRetail.Setting.AddUser obadd = new Setting.AddUser(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "cinfo")
            {
                POSRetail.Setting.AddCompanyInfo obadd = new Setting.AddCompanyInfo(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "chartaccounts")
            {
                POSRetail.Setting.AddChartAccountsCodes obadd = new Setting.AddChartAccountsCodes(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
            if (tablename == "cashsaleaccount")
            {
                POSRetail.Setting.AddSalesAccount obadd = new Setting.AddSalesAccount(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }

            if (tablename == "customers")
            {
                POSRetail.Setting.AddCustomer obadd = new Setting.AddCustomer(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();
            }
        }
        public void delfunc(string tablename, string id)
        {
            if (tablename == "group")
            {
                string q = "delete from groups where id='"+id+"'";
                objCore.executeQuery(q);
                getdata("select * from groups");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Category")
            {
                string q = "delete from Category where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.Category.Id, dbo.Groups.GroupName, dbo.Category.CategoryName, dbo.Category.CategoryDiscription FROM         dbo.Groups INNER JOIN                      dbo.Category ON dbo.Groups.Id = dbo.Category.GroupId");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Type")
            {
                string q = "delete from Type where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.Type.Id, dbo.Groups.GroupName, dbo.Category.CategoryName, dbo.Type.TypeName, dbo.Type.TypeDiscription FROM         dbo.Groups INNER JOIN                      dbo.Category ON dbo.Groups.Id = dbo.Category.GroupId INNER JOIN                      dbo.Type ON dbo.Groups.Id = dbo.Type.groupId AND dbo.Category.Id = dbo.Type.CategoryId");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Brand")
            {
                string q = "delete from Brands where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("select * from Brands");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "UOM")
            {
                string q = "delete from UOM where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("select * from UOM");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Size")
            {
                string q = "delete from Size where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("select * from Size");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Color")
            {
                string q = "delete from Color where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("select Id,  Caption as Color_Name,ColorName as Color_Code,  UploadStatus from Color");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Branch")
            {
                string q = "delete from Branch where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("select * from Branch");
                //MessageBox.Show("Record Delected Successfully");
            }
            
            if (tablename == "Store")
            {
                string q = "delete from Stores where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.Stores.Id, dbo.Stores.StoreName, dbo.Stores.StoreCode, dbo.Branch.BranchName FROM         dbo.Branch INNER JOIN                      dbo.Stores ON dbo.Branch.Id = dbo.Stores.BranchId");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "UOMC")
            {
                string q = "delete from UOMConversion where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.UOMConversion.Id, dbo.UOM.UOM, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM AS Converted_UOM FROM         dbo.UOM INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "MenuG")
            {
                string q = "delete from MenuGroup where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.Color.ColorName, dbo.MenuGroup.Description FROM         dbo.MenuGroup INNER JOIN                      dbo.Color ON dbo.MenuGroup.ColorId = dbo.Color.Id");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "MenuI")
            {
                string q = "delete from MenuItem where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.MenuItem.Id,dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Currency")
            {
                string q = "delete from Currency where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from Currency");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "MOP")
            {
                string q = "delete from MOP where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from MOP");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "GST")
            {
                string q = "delete from GST where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from GST");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Printer")
            {
                string q = "delete from Printers where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from Printers");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "Device")
            {
                string q = "delete from DeviceSetting where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from DeviceSetting");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "db")
            {
                string q = "delete from SqlServerInfo where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from SqlServerInfo");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "layout")
            {
                string q = "delete from Tablelayout where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT Id,tablename AS table_Name, Columns, Rows  FROM Tablelayout");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "supplier")
            {
                string q = "delete from supplier where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.Supplier.Id, dbo.Supplier.Name,  dbo.Supplier.Code, dbo.Supplier.CNICNo, dbo.Supplier.City, dbo.Supplier.Address, dbo.Supplier.Phone, dbo.Supplier.Mobile,                       dbo.Supplier.Area, dbo.Supplier.Date, dbo.Supplier.payableaccountid, dbo.Supplier.uploadstatus, dbo.ChartofAccounts.Name AS PayableAccount FROM         dbo.Supplier LEFT OUTER JOIN                      dbo.ChartofAccounts ON dbo.Supplier.payableaccountid = dbo.ChartofAccounts.Id");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "users")
            {
                if (POSRetail.Properties.Settings.Default.UserId == id)
                {
                    MessageBox.Show("You can not delete your own account");
                    return;
                }
                string q = "delete from users where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from users");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "cinfo")
            {

                string q = "delete from CompanyInfo where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("select  Id, Name, Address, Phone, uploadstatus from CompanyInfo");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "chartaccounts")
            {

                string q = "delete from ChartofAccountsCodes where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from ChartofAccountsCodes");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "cashsaleaccount")
            {

                string q = "delete from CashSalesAccountsList where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.CashSalesAccountsList.Id, dbo.ChartofAccounts.Name, dbo.CashSalesAccountsList.AccountType, dbo.CashSalesAccountsList.UploadStatus FROM         dbo.CashSalesAccountsList INNER JOIN                      dbo.ChartofAccounts ON dbo.CashSalesAccountsList.ChartaccountId = dbo.ChartofAccounts.Id");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "customers")
            {

                string q = "delete from customers where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     dbo.Customers.Id, dbo.Customers.Name, dbo.Customers.Email, dbo.Customers.Phone, dbo.Customers.Mobile, dbo.Customers.City, dbo.Customers.Address, dbo.ChartofAccounts.Name AS Account FROM         dbo.ChartofAccounts INNER JOIN                      dbo.Customers ON dbo.ChartofAccounts.Id = dbo.Customers.Chartaccountid");
                //MessageBox.Show("Record Delected Successfully");
            }
        }
        public void editfunc(string tablename)
        {
            if (tablename == "group")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddGroups obadd = new Setting.AddGroups(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Category")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddCategory obadd = new Setting.AddCategory(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Type")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddType obadd = new Setting.AddType(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Brand")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.Addbrand obadd = new Setting.Addbrand(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "UOM")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.Adduom obadd = new Setting.Adduom(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Size")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.Addsize obadd = new Setting.Addsize(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Color")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.Addcolor obadd = new Setting.Addcolor(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Branch")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.Addbranch obadd = new Setting.Addbranch(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Store")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddStore obadd = new Setting.AddStore(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "UOMC")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.Addcuom obadd = new Setting.Addcuom(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "MenuG")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddMenuG obadd = new Setting.AddMenuG(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "MenuI")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddMenuItem obadd = new Setting.AddMenuItem(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Currency")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddCurrency obadd = new Setting.AddCurrency(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "MOP")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddMOP obadd = new Setting.AddMOP(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "GST")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddGST obadd = new Setting.AddGST(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Printer")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddPrinter obadd = new Setting.AddPrinter(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "Device")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.DevicesSetting obadd = new Setting.DevicesSetting(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "db")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddSQLInfo obadd = new Setting.AddSQLInfo(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "layout")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddLayout obadd = new Setting.AddLayout(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "supplier")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddSupplier obadd = new Setting.AddSupplier(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "users")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddUser obadd = new Setting.AddUser(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "cinfo")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddCompanyInfo obadd = new Setting.AddCompanyInfo(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }

            if (tablename == "chartaccounts")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddChartAccountsCodes obadd = new Setting.AddChartAccountsCodes(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "cashsaleaccount")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddSalesAccount obadd = new Setting.AddSalesAccount(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
            if (tablename == "customers")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRetail.Setting.AddCustomer obadd = new Setting.AddCustomer(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();
                }
            }
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "1.1 Group")
            {
                addfunc("group");
            }
            if (label1.Text == "1.2 Category")
            {
                addfunc("Category");
            }
            if (label1.Text == "1.3 Type")
            {
                addfunc("Type");
            }
            if (label1.Text == "1.4 Brand")
            {
                addfunc("Brand");
            }
            if (label1.Text == "1.5 UOM")
            {
                addfunc("UOM");
            }
            if (label1.Text == "1.6 Size")
            {
                addfunc("Size");
            }
            if (label1.Text == "1.7 Color")
            {
                addfunc("Color");
            }
            if (label1.Text == "1.8 Branch")
            {
                addfunc("Branch");
            }
            if (label1.Text == "1.9 Store")
            {
                addfunc("Store");
            }
            if (label1.Text == "1.10 UOM Conversion")
            {
                addfunc("UOMC");
            }
            //if (label1.Text == "2.1 Menu Group")
            //{
            //    addfunc("MenuG");
            //}
            //if (label1.Text == "2.2 Menu Item")
            //{
            //    addfunc("MenuI");
            //}
            if (label1.Text == "2.1 Currency")
            {
                addfunc("Currency");
            }
            if (label1.Text == "2.2 MOP")
            {
                addfunc("MOP");
            }
            if (label1.Text == "2.4 GST")
            {
                addfunc("GST");
            }
            if (label1.Text == "2.3 Printer Setting")
            {
                addfunc("Printer");
            }
            if (label1.Text == "2.5 Device Setting")
            {
                addfunc("Device");
            }
            if (label1.Text == "2.6 Online DB Setting")
            {
                addfunc("db");
            }
            if (label1.Text == "2.7 Layout Setting")
            {
                addfunc("layout");
            }
            if (label1.Text == "1.10 Supplier")
            {
                addfunc("supplier");
            }
            if (label1.Text == "4.1 Users")
            {
                addfunc("users");
            }
            if (label1.Text == "3.1 Company Info")
            {
                addfunc("cinfo");
            }

            if (label1.Text == "5.1 Chart of Accounts Codes")
            {
                addfunc("chartaccounts");
            }
            if (label1.Text == "5.2 Customers")
            {
                addfunc("customers");
            }
            if (label1.Text == "2.7 Cash Sales Account")
            {
                addfunc("cashsaleaccount");
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {

            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    if (label1.Text == "1.1 Group")
                    {

                        editfunc("group");
                    }
                    if (label1.Text == "1.2 Category")
                    {
                        editfunc("Category");
                    }
                    if (label1.Text == "1.3 Type")
                    {
                        editfunc("Type");
                    }
                    if (label1.Text == "1.4 Brand")
                    {
                        editfunc("Brand");
                    }
                    if (label1.Text == "1.5 UOM")
                    {
                        editfunc("UOM");
                    }
                    if (label1.Text == "1.6 Size")
                    {
                        editfunc("Size");
                    }
                    if (label1.Text == "1.7 Color")
                    {
                        editfunc("Color");
                    }
                    if (label1.Text == "1.8 Branch")
                    {
                        editfunc("Branch");
                    }
                    if (label1.Text == "1.9 Store")
                    {
                        editfunc("Store");
                    }
                    if (label1.Text == "1.10 UOM Conversion")
                    {
                        editfunc("UOMC");
                    }
                    //if (label1.Text == "2.1 Menu Group")
                    //{
                    //    editfunc("MenuG");
                    //}
                    //if (label1.Text == "2.2 Menu Item")
                    //{
                    //    editfunc("MenuI");
                    //}
                    if (label1.Text == "2.1 Currency")
                    {
                        editfunc("Currency");
                    }
                    if (label1.Text == "2.2 MOP")
                    {
                        editfunc("MOP");
                    }
                    if (label1.Text == "2.4 GST")
                    {
                        editfunc("GST");
                    }
                    if (label1.Text == "2.3 Printer Setting")
                    {
                        editfunc("Printer");
                    }
                    if (label1.Text == "2.5 Device Setting")
                    {
                        editfunc("Device");
                    }
                    if (label1.Text == "2.6 Online DB Setting")
                    {
                        editfunc("db");
                    }
                    if (label1.Text == "2.7 Layout Setting")
                    {
                        editfunc("layout");
                    }
                    if (label1.Text == "1.10 Supplier")
                    {
                        editfunc("supplier");
                    }
                    if (label1.Text == "4.1 Users")
                    {
                        editfunc("users");
                    }
                    if (label1.Text == "3.1 Company Info")
                    {
                        editfunc("cinfo");
                    }
                    if (label1.Text == "5.1 Chart of Accounts Codes")
                    {
                        editfunc("chartaccounts");
                    }
                    if (label1.Text == "2.7 Cash Sales Account")
                    {
                        editfunc("cashsaleaccount");
                    }
                    if (label1.Text == "5.2 Customers")
                    {
                        editfunc("customers");
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msg == DialogResult.Yes)
            {

                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    if (label1.Text == "1.1 Group")
                    {

                        delfunc("group", id.ToString());
                    }
                    if (label1.Text == "1.2 Category")
                    {
                        delfunc("Category", id.ToString());
                    }

                    if (label1.Text == "1.3 Type")
                    {
                        delfunc("Type", id.ToString());
                    }
                    if (label1.Text == "1.4 Brand")
                    {
                        delfunc("Brand", id.ToString());
                    }
                    if (label1.Text == "1.5 UOM")
                    {
                        delfunc("UOM", id.ToString());
                    }
                    if (label1.Text == "1.6 Size")
                    {
                        delfunc("Size", id.ToString());
                    }
                    if (label1.Text == "1.7 Color")
                    {
                        delfunc("Color", id.ToString());
                    }
                    if (label1.Text == "1.8 Branch")
                    {
                        delfunc("Branch", id.ToString());
                    }
                    if (label1.Text == "1.9 Store")
                    {
                        delfunc("Store", id.ToString());
                    }
                    if (label1.Text == "1.10 UOM Conversion")
                    {
                        delfunc("UOMC", id.ToString());
                    }
                    //if (label1.Text == "2.1 Menu Group")
                    //{
                    //    delfunc("MenuG", id.ToString());
                    //}
                    //if (label1.Text == "2.2 Menu Item")
                    //{
                    //    delfunc("MenuI", id.ToString());
                    //}
                    if (label1.Text == "2.1 Currency")
                    {
                        delfunc("Currency", id.ToString());
                    }
                    if (label1.Text == "2.2 MOP")
                    {
                        delfunc("MOP", id.ToString());
                    }
                    if (label1.Text == "2.4 GST")
                    {
                        delfunc("GST", id.ToString());
                    }
                    if (label1.Text == "2.3 Printer Setting")
                    {
                        delfunc("Printer", id.ToString());
                        
                    }
                    if (label1.Text == "2.5 Device Setting")
                    {
                        delfunc("Device", id.ToString());
                    }
                    if (label1.Text == "2.6 Online DB Setting")
                    {
                        delfunc("db", id.ToString());
                    }
                    if (label1.Text == "2.7 Layout Setting")
                    {
                        delfunc("layout", id.ToString());
                    }
                    if (label1.Text == "1.10 Supplier")
                    {
                        delfunc("supplier", id.ToString());
                    }
                    if (label1.Text == "4.1 Users")
                    {
                        delfunc("users", id.ToString());
                    }
                    if (label1.Text == "3.1 Company Info")
                    {
                        delfunc("cinfo", id.ToString());
                    }
                    if (label1.Text == "5.1 Chart of Accounts Codes")
                    {
                        delfunc("chartaccounts", id.ToString());
                    }
                    if (label1.Text == "2.7 Cash Sales Account")
                    {
                        delfunc("cashsaleaccount", id.ToString());
                    }
                    if (label1.Text == "5.2 Customers")
                    {
                        delfunc("customers", id.ToString());
                    }
                }
            }
        }
    }
}

