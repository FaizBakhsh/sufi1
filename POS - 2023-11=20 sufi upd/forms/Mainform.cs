using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using POSRestaurant.classes;
using POSRestaurant.admin;
using System.Data.SqlClient;

using POSRestaurant.ControlPanel;

namespace POSRestaurant.forms
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

            POSRestaurant.classes.Clsdbcon objCore = new Clsdbcon();
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
            POSRestaurant.forms.login log = new login();
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
            POSRestaurant.admin.frmSupplierManagement obj = new frmSupplierManagement();
            obj.MdiParent = this;
            obj.Show();

        }
        private void supplierManagement_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            POSRestaurant.admin.frmSupplierManagement SupManagement = new POSRestaurant.admin.frmSupplierManagement();
            SupManagement.MdiParent = this.MdiParent;
            SupManagement.WindowState = FormWindowState.Maximized;
            SupManagement.Show();
        }

        private void addCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.admin.frmAddItemrecipe AddCategory = new POSRestaurant.admin.frmAddItemrecipe();
            AddCategory.Show();
        }

        private void manageCatergoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.admin.frmItemrecipemang ManageCatgory = new POSRestaurant.admin.frmItemrecipemang();
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
                POSRestaurant.classes.Clsdbcon objCore = new Clsdbcon();
                DataSet ds = new DataSet();
                if (label1.Text == "2.5 Recipe")
                {
                    DataTable dtgv = new DataTable();
                    DataColumn dcol1 = new DataColumn("id", typeof(string));
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
                    dataGridView1.Columns[5].ReadOnly = false;
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
                if (qry == "SELECT printer,type  FROM         printtype")
                {

                }
                else
                {
                    dataGridView1.Columns[0].Visible = false;
                }
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
            catch (Exception ex)
            {

            }
        }
        private void listBar1_ItemClicked(object sender, NETXP.Controls.Bars.ItemClickedEventArgs e)
        {
            try
            {
                string val = e.Item.Caption.ToString();
                try
                {
                    int strt = val.IndexOf(" ", 0);
                    val = val.Substring(strt + 1);
                }
                catch (Exception ex)
                {
                    
                    
                }
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
                    case 10:
                        label1.Text = "1.10 UOM Conversion";

                        q = "SELECT     dbo.UOMConversion.Id, dbo.UOM.UOM, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM AS Converted_UOM FROM         dbo.UOM INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId";
                        getdata(q);

                        break;
                    case 11:
                        label1.Text = "2.1 Menu Group";

                        q = "SELECT     dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.Color.ColorName, dbo.MenuGroup.type as KitchenPrint,dbo.MenuGroup.FontSize, dbo.MenuGroup.Status FROM         dbo.MenuGroup INNER JOIN                      dbo.Color ON dbo.MenuGroup.ColorId = dbo.Color.Id";
                        getdata(q);
                        break;
                    case 12:
                        label1.Text = "2.2 Menu Item";

                        //  q = "SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status, dbo.Color.Caption,                       dbo.KDS.Name AS KDS FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId LEFT OUTER JOIN                      dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id";
                        if (textBox1.Text == "")
                        {
                            q = "SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name,dbo.MenuItem.Name2, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price,dbo.MenuItem.GrossPrice,dbo.MenuItem.Price2,dbo.MenuItem.Price3, dbo.MenuItem.Status, dbo.MenuItem.Modifiercount, dbo.Color.ColorName,                       dbo.KDS.Name AS KDS, dbo.MenuItem.Minutes AS MakingTime, dbo.MenuItem.alarmtime FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId INNER JOIN                      dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id";
                        }
                        else
                        {
                            q = "SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name,dbo.MenuItem.Name2, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price,dbo.MenuItem.GrossPrice,dbo.MenuItem.Price2,dbo.MenuItem.Price3, dbo.MenuItem.Status, dbo.MenuItem.Modifiercount, dbo.Color.ColorName,                       dbo.KDS.Name AS KDS, dbo.MenuItem.Minutes AS MakingTime, dbo.MenuItem.alarmtime FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId INNER JOIN                      dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id where dbo.MenuItem.Name like '%" + textBox1.Text + "%' or dbo.MenuItem.price like '%" + textBox1.Text + "%'  or dbo.MenuItem.code like '%" + textBox1.Text + "%' or  dbo.MenuGroup.Name like '%" + textBox1.Text + "%'";
                     
                        }
                        getdata(q);
                        break;
                    case 13:
                        label1.Text = "2.3 Currency";

                        q = "SELECT   * from Currency";
                        getdata(q);
                        break;
                    case 14:
                        label1.Text = "2.4 MOP";

                        q = "SELECT   * from MOP";
                        getdata(q);
                        break;
                    case 15:
                        label1.Text = "2.5 Printer Setting";

                        q = "SELECT   * from Printers";
                        getdata(q);

                        break;
                    case 16:
                        label1.Text = "2.6 Recipe Modifier";
                        Setting.AddRuntimeModifier obj = new Setting.AddRuntimeModifier(this);
                        obj.Show();
                        //q = "SELECT   * from MenuItem";
                        //getdata(q);

                        break;
                    case 17:
                        label1.Text = "2.7 GST";

                        q = "SELECT   * from GST";
                        getdata(q);
                        break;
                    case 18:
                        label1.Text = "2.8 Device Setting";

                        q = "SELECT   * from  DeviceSetting";
                        getdata(q);
                        break;
                    case 19:
                        label1.Text = "2.9 Online DB Setting";

                        q = "SELECT   * from  SqlServerInfo";
                        getdata(q);
                        break;
                    case 20:
                        label1.Text = "2.10 Layout Setting";

                        q = "SELECT Id,tablename AS table_Name, Columns, Rows  FROM Tablelayout";
                        getdata(q);
                        break;
                    case 21:
                        label1.Text = "1.11 Supplier";

                        q = "SELECT * from supplier";
                        getdata(q);
                        break;
                    case 31:
                        label1.Text = "3.1 Company Info";

                        q = "SELECT * from CompanyInfo";
                        getdata(q);
                        break;
                    case 32:
                        label1.Text = "3.2 Expenses";

                        q = "SELECT        Id, Name, Amount, Date FROM            Expenses";
                        getdata(q);
                        break;
                    case 41:
                        label1.Text = "4.1 Users";

                        q = "select [Id],[Name],[FatherName],[Phone],[CNICNo],[Address],[Usertype],[UserName],[Designation],[uploadstatus],[branchid],[status],[kdsid],[terminal],[role],[ParkStatus],[discountlimit],[KDSType],[Signature] FROM Users";
                        getdata(q);
                        break;
                    case 42:
                        label1.Text = "4.2 Staff";

                        q = "SELECT * from ResturantStaff";
                        getdata(q);
                        break;
                    case 43:
                        label1.Text = "4.3 Employee Sale";

                        q = "SELECT        dbo.EmployeeRecvb.id, dbo.ResturantStaff.Name, dbo.EmployeeRecvb.amount, dbo.EmployeeRecvb.date FROM            dbo.ResturantStaff INNER JOIN                         dbo.EmployeeRecvb ON dbo.ResturantStaff.Id = dbo.EmployeeRecvb.empid order by  dbo.ResturantStaff.Name";
                        getdata(q);
                        break;
                    case 44:
                        label1.Text = "4.4 Employees";

                        q = "SELECT        * FROM            Employees";
                        getdata(q);
                        break;
                    case 45:
                        label1.Text = "4.5 Employees Salary";

                        q = "SELECT        dbo.EmployeesSalary.id, dbo.Employees.Name AS name, dbo.EmployeesSalary.Amount, dbo.EmployeesSalary.Date FROM            dbo.Employees INNER JOIN                         dbo.EmployeesSalary ON dbo.Employees.Id = dbo.EmployeesSalary.Empid";
                        getdata(q);
                        break;
                    case 46:
                       
                        label1.Text = "4.6 Attandance";

                        q = "SELECT        dbo.EmployeeAttandance.Id, dbo.Employees.Name AS name, dbo.EmployeeAttandance.Month, dbo.EmployeeAttandance.Days FROM            dbo.Employees INNER JOIN                         dbo.EmployeeAttandance ON dbo.Employees.Id = dbo.EmployeeAttandance.EmpID";
                        getdata(q);
                        break;
                        case 47:


                        label1.Text = "4.7 Employee Statement";
                        POSRestaurant.Reports.Statements.frmEmployeePayableStatemet objemp = new POSRestaurant.Reports.Statements.frmEmployeePayableStatemet();
                        objemp.Show();
                        
                        break;
                    case 211:
                        label1.Text = "2.11 KOT Setting";

                        q = "SELECT * from kds";
                        getdata(q);
                        break;
                    case 212:
                        label1.Text = "2.12 Customer Info";

                        q = "SELECT * from Customers";
                        getdata(q);
                        break;
                    case 213:
                        label1.Text = "2.13 Members Point Setting";

                        q = "SELECT * from Points";
                        getdata(q);
                        break;
                    case 214:
                        label1.Text = "2.14 Modifier";

                        //q = "SELECT   dbo.RuntimeModifier.id, dbo.RuntimeModifier.name AS ModifierName, dbo.RawItem.ItemName, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.price,                       dbo.RuntimeModifier.status, dbo.KDS.Name AS KDS FROM         dbo.RawItem INNER JOIN                      dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.Itemid LEFT OUTER JOIN                      dbo.KDS ON dbo.RuntimeModifier.kdsid = dbo.KDS.Id ORDER BY dbo.RuntimeModifier.id";
                        q = "SELECT   dbo.Modifier.Id,dbo.RawItem.ItemName, dbo.KDS.Name AS KDS,  dbo.Modifier.Price, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId LEFT OUTER JOIN                      dbo.KDS ON dbo.Modifier.kdsid = dbo.KDS.Id";
                        getdata(q);
                        break;
                    case 215:
                        label1.Text = "2.15 Discount Keys";

                        q = "SELECT     id, name, discount FROM         DiscountKeys  ";
                        getdata(q);
                        break;
                    case 216:
                        label1.Text = "2.16 Shifts";

                        q = "SELECT     id, name FROM         Shifts  ";
                        getdata(q);
                        break;
                    case 217:
                        label1.Text = "2.17 Service Charges";

                        q = "SELECT     id, charges FROM         SerivceCharges  ";
                        getdata(q);
                        break;
                    case 218:
                    label1.Text = "2.18 Banks";

                    q = "SELECT     id, Name FROM         banks";
                    getdata(q);
                    break;
                    case 219:
                    label1.Text = "2.19 Loyality Cards";

                    q = "SELECT     id, cardno, discount, date, name, phone FROM         LoyalityCards";
                    getdata(q);
                    break;


                    case 220:
                    label1.Text = "2.20 Print Type";

                    q = "SELECT printer,type  FROM         printtype";
                    getdata(q);
                    break;

                    case 221:
                    label1.Text = "2.21 Discount Compaign";

                    q = "SELECT Id,Name, Discount, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday,  Datefrom, DateTo, TimeFrom, TimeTo  FROM      DiscountCompaign";
                    getdata(q);
                    break;


                    case 51:
                    label1.Text = "5.1 Deal Head";

                    q = "SELECT id, Name, Status FROM  DealHeads";
                    getdata(q);
                    break;
                    //
                    case 52:
                    label1.Text = "5.2 Deals";

                    q = "SELECT dbo.Deals.id, dbo.DealHeads.name AS Head, dbo.Deals.Name,dbo.Deals.Price, dbo.Deals.Status FROM  dbo.DealHeads INNER JOIN               dbo.Deals ON dbo.DealHeads.id = dbo.Deals.headid";
                    getdata(q);
                    break;
                    case 53:
                    label1.Text = "5.3 Attach Menu";

                    q = "SELECT dbo.AttachMenu.id, dbo.Deals.name AS Deal, dbo.MenuItem.Name AS Menuitem, dbo.ModifierFlavour.name AS Size, dbo.AttachMenu.Price,dbo.AttachMenu.Compulsory ,dbo.AttachMenu.No, dbo.AttachMenu.status FROM  dbo.AttachMenu INNER JOIN               dbo.Deals ON dbo.AttachMenu.Dealid = dbo.Deals.id INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id";
                    getdata(q);
                    break;
                    case 112:
                    label1.Text = "1.12 Delivery Areas";

                    q = "SELECT        dbo.DeliveryArea.id, dbo.DeliveryArea.Name, dbo.Branch.BranchName FROM            dbo.Branch INNER JOIN                         dbo.DeliveryArea ON dbo.Branch.Id = dbo.DeliveryArea.Branchid";
                    getdata(q);
                    break;
                    case 222:
                    label1.Text = "2.22 Order Source";

                    q = "SELECT id, Name, Status FROM  ordersource";
                    getdata(q);
                    break;
                    case 223:
                    label1.Text = "2.23 Sub Items";

                    q = "SELECT id, Name, Status FROM  SubItems";
                    getdata(q);
                    break;
                    case 224:
                    label1.Text = "2.24 Vouchers";

                    q = "SELECT id, Name, Amount FROM  VoucherKeys";
                    getdata(q);
                    break;
                    case 225:
                    label1.Text = "2.25 Table Design";

                    q = "SELECT   Id, Floor, TableNo FROM            DineInTableDesign";
                    getdata(q);
                    break;
                    case 226:
                    label1.Text = "2.26 Attach Menu";
                        POSRestaurant.Setting.attachmenu obj1 = new Setting.attachmenu();
                         obj1.Show();
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
            ControlPanel.frmGlobalSettings settings = new POSRestaurant.ControlPanel.frmGlobalSettings();
            settings.ShowDialog();
        }


        private void addSupplierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.admin.frmSupplierAdd obj = new frmSupplierAdd();
            obj.MdiParent = this.MdiParent;
            obj.Show();

        }

        private void subCategoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRestaurant.admin.frmAddItemSubCategory AddCategory = new POSRestaurant.admin.frmAddItemSubCategory();
            AddCategory.Show();
        }


        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {

            POSRestaurant.ControlPanel.frmManageUsers Manageuser = new frmManageUsers();
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
            POSRestaurant.admin.frmItemSubCategory ManageCatgory = new POSRestaurant.admin.frmItemSubCategory();
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
        protected int colorcode(string type,string subtype)
        {
            int val = 0;
            try
            {
                string q = "select "+type+" from theme where type='Backend' and subtype='"+subtype+"'";
                DataSet dscolor = new DataSet();
                dscolor = objCore.funGetDataSet(q);
                if (dscolor.Tables[0].Rows.Count > 0)
                {
                    string temp = dscolor.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        if (subtype == "BackColor")
                        {
                            temp = "0";
                        }
                        else
                        {
                            temp = "1";
                        }
                    }
                    val = Convert.ToInt32(temp);
                }
                else
                {
                    string temp = "";
                    if (temp == "")
                    {
                        if (subtype == "BackColor")
                        {
                            temp = "1";
                        }
                        else
                        {
                            temp = "255";
                        }
                    }
                    val = Convert.ToInt32(temp);
                }
            }
            catch (Exception ex)
            {
                
                
            }

            return val;
        }
        public void addfunc(string tablename)
        {
            if (tablename == "group")
            {
                POSRestaurant.Setting.AddGroups obadd = new Setting.AddGroups(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor"));
                obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                obadd.Show();obadd.BackgroundImage = null;
            }
            if (tablename == "Category")
            {
                POSRestaurant.Setting.AddCategory obadd = new Setting.AddCategory(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Type")
            {
                POSRestaurant.Setting.AddType obadd = new Setting.AddType(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Brand")
            {
                POSRestaurant.Setting.Addbrand obadd = new Setting.Addbrand(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "UOM")
            {
                POSRestaurant.Setting.Adduom obadd = new Setting.Adduom(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Size")
            {
                POSRestaurant.Setting.Addsize obadd = new Setting.Addsize(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Color")
            {
                POSRestaurant.Setting.Addcolor obadd = new Setting.Addcolor(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Branch")
            {
                POSRestaurant.Setting.Addbranch obadd = new Setting.Addbranch(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Store")
            {
                POSRestaurant.Setting.AddStore obadd = new Setting.AddStore(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "UOMC")
            {
                POSRestaurant.Setting.Addcuom obadd = new Setting.Addcuom(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "MenuG")
            {
                POSRestaurant.Setting.AddMenuG obadd = new Setting.AddMenuG(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); 
                obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));

            }
            if (tablename == "MenuI")
            {
                //POSRetail.Setting.AddMenuItem obadd = new POSRetail.Setting.AddMenuItem(this;
                try
                {
                    POSRestaurant.Setting.AddMenuItem obadd = new Setting.AddMenuItem(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.Show(); obadd.BackgroundImage = null; obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));

                }
                catch (Exception ex)
                {
                    
                   
                }
            }
            if (tablename == "Currency")
            {
                POSRestaurant.Setting.AddCurrency obadd = new Setting.AddCurrency(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "MOP")
            {
                POSRestaurant.Setting.AddMOP obadd = new Setting.AddMOP(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "GST")
            {
                POSRestaurant.Setting.AddGST obadd = new Setting.AddGST(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Printer")
            {
                POSRestaurant.Setting.AddPrinter obadd = new Setting.AddPrinter(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "Device")
            {
                POSRestaurant.Setting.DevicesSetting obadd = new Setting.DevicesSetting(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "db")
            {
                POSRestaurant.Setting.AddSQLInfo obadd = new Setting.AddSQLInfo(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "layout")
            {
                POSRestaurant.Setting.AddLayout obadd = new Setting.AddLayout(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "supplier")
            {
                POSRestaurant.Setting.AddSupplier obadd = new Setting.AddSupplier(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "users")
            {
                POSRestaurant.Setting.AddUser obadd = new Setting.AddUser(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "cinfo")
            {
                POSRestaurant.Setting.AddCompanyInfo obadd = new Setting.AddCompanyInfo(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "kds")
            {
                POSRestaurant.Setting.AddKDS obadd = new Setting.AddKDS(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "customers")
            {
                POSRestaurant.Setting.AddCustomerInfo obadd = new Setting.AddCustomerInfo(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "points")
            {
                POSRestaurant.Setting.AddPoints obadd = new Setting.AddPoints(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;
               // obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "staff")
            {
                POSRestaurant.Setting.AddStaff obadd = new Setting.AddStaff(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "rmodifier")
            {
                POSRestaurant.Setting.AddModifier obadd = new  Setting.AddModifier(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "discountkeys")
            {
                POSRestaurant.Setting.AddDiscount obadd = new Setting.AddDiscount(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "shifts")
            {
                POSRestaurant.Setting.AddShifts obadd = new Setting.AddShifts(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "service")
            {
                POSRestaurant.Setting.Addservice obadd = new Setting.Addservice(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "banks")
            {
                POSRestaurant.Setting.Addbanks obadd = new Setting.Addbanks(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "emprecv")
            {
                POSRestaurant.Setting.receivableentry obadd = new Setting.receivableentry(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "cards")
            {
                POSRestaurant.Setting.addloyalitycard obadd = new Setting.addloyalitycard(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "printtype")
            {
                POSRestaurant.Setting.addprinttype obadd = new Setting.addprinttype(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;
                
                obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "attach")
            {
                POSRestaurant.Setting.AttachMenuItem obadd = new Setting.AttachMenuItem(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "extra")
            {
                //POSRestaurant.Setting.AddExtraFlavour obadd = new Setting.AddExtraFlavour(this);
                //obadd.MdiParent = this.MdiParent;
                //// objsl.WindowState = FormWindowState.Maximized;
                //obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "dealhead")
            {
                POSRestaurant.Setting.AddDealsHead obadd = new Setting.AddDealsHead(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "deal")
            {
                POSRestaurant.Setting.AddDeals obadd = new Setting.AddDeals(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "discountcompaign")
            {
                POSRestaurant.Setting.DiscountCompaign obadd = new Setting.DiscountCompaign(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "employees")
            {
                POSRestaurant.Setting.Addemployees obadd = new Setting.Addemployees(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }

            if (tablename == "employeessalary")
            {
                POSRestaurant.Setting.Addsalaryry obadd = new Setting.Addsalaryry(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "attandance")
            {
                POSRestaurant.Setting.AddAttandance obadd = new Setting.AddAttandance(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show(); obadd.BackgroundImage = null; obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "expenses")
            {
                POSRestaurant.Setting.Addexpenses obadd = new Setting.Addexpenses(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "area")
            {
                POSRestaurant.Setting.Addarea obadd = new Setting.Addarea(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show(); obadd.BackgroundImage = null; obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
            }
            if (tablename == "ordersource")
            {
                POSRestaurant.Setting.AddSource obadd = new Setting.AddSource(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show(); obadd.BackgroundImage = null;
            }
            if (tablename == "subitems")
            {
                POSRestaurant.Setting.AddSubItems obadd = new Setting.AddSubItems(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show(); obadd.BackgroundImage = null;
            }
            if (tablename == "vouchers")
            {
                POSRestaurant.Setting.AddVouchers obadd = new Setting.AddVouchers(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show(); obadd.BackgroundImage = null;
            }
            if (tablename == "tables")
            {
                POSRestaurant.Setting.AddTables obadd = new Setting.AddTables(this);
                obadd.MdiParent = this.MdiParent;
                // objsl.WindowState = FormWindowState.Maximized;
                obadd.editmode = 0;
                obadd.Show(); obadd.BackgroundImage = null;
            }
        }
        public void delfunc(string tablename, string id)
        {
            if (tablename == "group")
            {
                string q = "delete from groups where id='" + id + "'";
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
                q = "SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status, dbo.Color.ColorName,                       dbo.KDS.Name AS KDS, dbo.MenuItem.Minutes AS MakingTime, dbo.MenuItem.alarmtime FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId INNER JOIN                      dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id";
                
                getdata(q);
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
                getdata("SELECT * from supplier");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "users")
            {
                if (POSRestaurant.Properties.Settings.Default.UserId == id)
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
                getdata("SELECT * from CompanyInfo");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "kds")
            {

                string q = "delete from kds where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from kds");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "customers")
            {

                string q = "delete from Customers where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from Customers");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "points")
            {

                string q = "delete from points where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from points");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "staff")
            {

                string q = "delete from ResturantStaff where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT * from ResturantStaff");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "rmodifier")
            {

                string q = "delete from Modifier where id='" + id + "'";
                objCore.executeQuery(q);
                //q = "SELECT   dbo.RawItem.ItemName, dbo.KDS.Name AS KDS, dbo.Modifier.Id, dbo.Modifier.RawItemId, dbo.Modifier.Price, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId LEFT OUTER JOIN                      dbo.KDS ON dbo.Modifier.kdsid = dbo.KDS.Id";
                q = "SELECT   dbo.Modifier.Id,dbo.RawItem.ItemName, dbo.KDS.Name AS KDS,  dbo.Modifier.Price, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId LEFT OUTER JOIN                      dbo.KDS ON dbo.Modifier.kdsid = dbo.KDS.Id";                     
                getdata(q);
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "discountkeys")
            {

                string q = "delete from DiscountKeys where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     id, name, discount FROM         DiscountKeys");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "shifts")
            {

                string q = "delete from shifts where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     id, name FROM         shifts");
                //MessageBox.Show("Record Delected Successfully");
            }
            if (tablename == "service")
            {
                string q = "delete from SerivceCharges where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     id, name FROM         SerivceCharges");
            }
            if (tablename == "banks")
            {
                string q = "delete from banks where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT     id, name FROM         banks");
            }
            if (tablename == "emprecv")
            {
                string q = "delete from EmployeeRecvb where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT        dbo.EmployeeRecvb.id, dbo.ResturantStaff.Name, dbo.EmployeeRecvb.amount, dbo.EmployeeRecvb.date FROM            dbo.ResturantStaff INNER JOIN                         dbo.EmployeeRecvb ON dbo.ResturantStaff.Id = dbo.EmployeeRecvb.empid order by  dbo.ResturantStaff.Name");
            }
             if (tablename == "cards")
            {
                string q = "delete from LoyalityCards where id='" + id + "'";
                objCore.executeQuery(q);
                getdata("SELECT        id, cardno, discount, date, name, phone FROM            LoyalityCards");
         
             }
             if (tablename == "printtype")
             {
                 string q = "delete from printtype where printer='" + id + "'";
                 objCore.executeQuery(q);
                 getdata("SELECT printer,type  FROM         printtype");
             }
             if (tablename == "attach")
             {
                 string q = "delete from AttachMenu where id='" + id + "'";
                 objCore.executeQuery(q);
                 q = "SELECT dbo.AttachMenu.id, dbo.Deals.name AS Deal, dbo.MenuItem.Name AS Menuitem, dbo.ModifierFlavour.name AS Size, dbo.AttachMenu.Price,dbo.AttachMenu.Compulsory ,dbo.AttachMenu.No, dbo.AttachMenu.status FROM  dbo.AttachMenu INNER JOIN               dbo.Deals ON dbo.AttachMenu.Dealid = dbo.Deals.id INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id";
                 getdata(q);
             }
             if (tablename == "extra")
             {
                 string q = "delete from ModifierextraFlavour where id='" + id + "'";
                 objCore.executeQuery(q);

                 q = "SELECT     dbo.ModifierextraFlavour.Id, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.Name AS MenuItem, dbo.ModifierextraFlavour.name AS ExtraFlavour FROM         dbo.ModifierextraFlavour INNER JOIN                      dbo.MenuItem ON dbo.ModifierextraFlavour.MenuItemId = dbo.MenuItem.Id INNER JOIN                      dbo.MenuGroup ON dbo.ModifierextraFlavour.MenuGroupId = dbo.MenuGroup.Id";

                 getdata(q);
             }
             if (tablename == "dealhead")
             {
                 string q = "delete from DealHeads where id='" + id + "'";
                 objCore.executeQuery(q);

                 q = "SELECT id, name, status FROM  DealHeads";

                 getdata(q);
             }
             if (tablename == "deal")
             {
                 string q = "delete from Deals where id='" + id + "'";
                 objCore.executeQuery(q);

                 q = "SELECT dbo.Deals.id, dbo.DealHeads.name AS Head, dbo.Deals.Name,dbo.Deals.Price, dbo.Deals.Status FROM  dbo.DealHeads INNER JOIN               dbo.Deals ON dbo.DealHeads.id = dbo.Deals.headid";

                 getdata(q);
             }
             if (tablename == "discountcompaign")
             {
                 string q = "delete from DiscountCompaign where id='" + id + "'";
                 objCore.executeQuery(q);

                 q = "SELECT        Id, Name, Discount, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday, AllDay, Datefrom, DateTo, TimeFrom, TimeTo FROM            DiscountCompaign";

                 getdata(q);
             }
             if (tablename == "employees")
             {
                 string q = "delete from employees where id='" + id + "'";
                 objCore.executeQuery(q);

                 q = "SELECT        * FROM            employees";

                 getdata(q);
             }
            

             if (tablename == "employeessalary")
             {
                 string q = "delete from EmployeesSalary where id='" + id + "'";
                 objCore.executeQuery(q);

                 q = "SELECT         dbo.EmployeesSalary.id,dbo.Employees.Name  as name, dbo.EmployeesSalary.Amount, dbo.EmployeesSalary.Date FROM            dbo.Employees INNER JOIN                         dbo.EmployeesSalary ON dbo.Employees.EmpId = dbo.EmployeesSalary.Empid";

                 getdata(q);
             }
             if (tablename == "attandance")
             {
                 string q = "delete from EmployeeAttandance  where id='" + id + "'";
                 objCore.executeQuery(q);

                 q = "SELECT        dbo.EmployeeAttandance.Id, dbo.Employees.Name AS name, dbo.EmployeeAttandance.Month, dbo.EmployeeAttandance.Days FROM            dbo.Employees INNER JOIN                         dbo.EmployeeAttandance ON dbo.Employees.Id = dbo.EmployeeAttandance.EmpID";

                 getdata(q);
             }
             if (tablename == "expenses")
             {
                 string q = "delete from expenses where id='" + id + "'";
                 objCore.executeQuery(q);
                 q = "select Id, Name, Amount, Date  from expenses";
                 getdata(q);
             }
             if (tablename == "area")
             {
                 string q = "delete from DeliveryArea where id='" + id + "'";
                 objCore.executeQuery(q);
                 q = "SELECT        dbo.DeliveryArea.id, dbo.DeliveryArea.Name, dbo.Branch.BranchName FROM            dbo.Branch INNER JOIN                         dbo.DeliveryArea ON dbo.Branch.Id = dbo.DeliveryArea.Branchid";
                 getdata(q);
             }
             if (tablename == "ordersource")
             {
                 string q = "delete from ordersource where id='" + id + "'";
                 objCore.executeQuery(q);
                 q = "select Id, Name, Status  from ordersource";
                 getdata(q);
             }
             if (tablename == "subitems")
             {
                 string q = "delete from subitems where id='" + id + "'";
                 objCore.executeQuery(q);
                 q = "select Id, Name, Status  from subitems";
                 getdata(q);
             }
             if (tablename == "vouchers")
             {
                 string q = "delete from vouchers where id='" + id + "'";
                 objCore.executeQuery(q);
                 q = "select Id, Name, Amount  from VoucherKeys";
                 getdata(q);
             }
             if (tablename == "tables")
             {
                 string q = "delete from DineInTableDesign where id='" + id + "'";
                 objCore.executeQuery(q);
                 q = "SELECT  Id, Floor, TableNo FROM            DineInTableDesign";
                 getdata(q);
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
                    POSRestaurant.Setting.AddGroups obadd = new Setting.AddGroups(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Category")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddCategory obadd = new Setting.AddCategory(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Type")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddType obadd = new Setting.AddType(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Brand")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addbrand obadd = new Setting.Addbrand(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "UOM")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Adduom obadd = new Setting.Adduom(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Size")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addsize obadd = new Setting.Addsize(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Color")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addcolor obadd = new Setting.Addcolor(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Branch")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addbranch obadd = new Setting.Addbranch(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Store")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddStore obadd = new Setting.AddStore(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "UOMC")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addcuom obadd = new Setting.Addcuom(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "MenuG")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddMenuG obadd = new Setting.AddMenuG(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "MenuI")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddMenuItem obadd = new Setting.AddMenuItem(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Currency")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddCurrency obadd = new Setting.AddCurrency(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "MOP")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddMOP obadd = new Setting.AddMOP(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "GST")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddGST obadd = new Setting.AddGST(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Printer")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddPrinter obadd = new Setting.AddPrinter(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "Device")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.DevicesSetting obadd = new Setting.DevicesSetting(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "db")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddSQLInfo obadd = new Setting.AddSQLInfo(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "layout")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddLayout obadd = new Setting.AddLayout(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "supplier")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddSupplier obadd = new Setting.AddSupplier(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "users")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddUser obadd = new Setting.AddUser(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "cinfo")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();

                    POSRestaurant.Setting.AddCompanyInfo obadd = new Setting.AddCompanyInfo(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "kds")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddKDS obadd = new Setting.AddKDS(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "customers")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddCustomerInfo obadd = new Setting.AddCustomerInfo(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "points")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddPoints obadd = new Setting.AddPoints(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "staff")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddStaff obadd = new Setting.AddStaff(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "rmodifier")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddModifier obadd = new  Setting.AddModifier(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "discountkeys")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddDiscount obadd = new Setting.AddDiscount(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "shifts")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddShifts obadd = new Setting.AddShifts(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "service")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addservice obadd = new Setting.Addservice(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }

            if (tablename == "banks")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addbanks obadd = new Setting.Addbanks(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "emprecv")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.receivableentry obadd = new Setting.receivableentry(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "cards")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.addloyalitycard obadd = new Setting.addloyalitycard(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
             }
            if (tablename == "printtype")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[1].Value.ToString();
                    POSRestaurant.Setting.addprinttype obadd = new Setting.addprinttype(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "extra")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                   // POSRestaurant.Setting.AddExtraFlavour obadd = new Setting.AddExtraFlavour(this);
                    //obadd.MdiParent = this.MdiParent;
                    //// objsl.WindowState = FormWindowState.Maximized;
                    //obadd.editmode = 1;
                    //obadd.id = id;
                    //obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "dealhead")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddDealsHead obadd = new Setting.AddDealsHead(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "deal")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddDeals obadd = new Setting.AddDeals(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "discountcompaign")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.DiscountCompaign obadd = new Setting.DiscountCompaign(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "employees")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addemployees obadd = new Setting.Addemployees(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }

            if (tablename == "employeessalary")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addsalaryry obadd = new Setting.Addsalaryry(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "attandance")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddAttandance obadd = new Setting.AddAttandance(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show(); obadd.BackgroundImage = null; obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "expenses")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addexpenses obadd = new Setting.Addexpenses(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show();obadd.BackgroundImage = null;obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }
            }
            if (tablename == "area")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.Addarea obadd = new Setting.Addarea(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show(); obadd.BackgroundImage = null; obadd.BackColor = System.Drawing.Color.FromArgb(colorcode("red", "BackColor"), colorcode("green", "BackColor"), colorcode("blue", "BackColor")); obadd.ForeColor = System.Drawing.Color.FromArgb(colorcode("red", "ForeColor"), colorcode("green", "ForeColor"), colorcode("blue", "ForeColor"));
                }

            }
            if (tablename == "ordersource")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddSource obadd = new Setting.AddSource(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show(); obadd.BackgroundImage = null;
                }
            }
            if (tablename == "subitems")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddSubItems obadd = new Setting.AddSubItems(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show(); obadd.BackgroundImage = null;
                }
            }
            if (tablename == "vouchers")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddVouchers obadd = new Setting.AddVouchers(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show(); obadd.BackgroundImage = null;
                }
            }
            if (tablename == "tables")
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    POSRestaurant.Setting.AddTables obadd = new Setting.AddTables(this);
                    obadd.MdiParent = this.MdiParent;
                    // objsl.WindowState = FormWindowState.Maximized;
                    obadd.editmode = 1;
                    obadd.id = id;
                    obadd.Show(); obadd.BackgroundImage = null;
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
            if (label1.Text == "2.1 Menu Group")
            {
                addfunc("MenuG");
            }
            if (label1.Text == "2.2 Menu Item")
            {
                addfunc("MenuI");
            }
            if (label1.Text == "2.3 Currency")
            {
                addfunc("Currency");
            }
            if (label1.Text == "2.4 MOP")
            {
                addfunc("MOP");
            }
            if (label1.Text == "2.7 GST")
            {
                addfunc("GST");
            }
            if (label1.Text == "2.5 Printer Setting")
            {
                addfunc("Printer");
            }
            if (label1.Text == "2.8 Device Setting")
            {
                addfunc("Device");
            }
            if (label1.Text == "2.9 Online DB Setting")
            {
                addfunc("db");
            }
            if (label1.Text == "2.10 Layout Setting")
            {
                addfunc("layout");
            }
            if (label1.Text == "1.11 Supplier")
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
            if (label1.Text == "2.11 KOT Setting")
            {
                addfunc("kds");
            }
            if (label1.Text == "2.12 Customer Info")
            {
                addfunc("customers");
            }
            if (label1.Text == "2.13 Members Point Setting")
            {
                addfunc("points");
            }
            if (label1.Text == "4.2 Staff")
            {
                addfunc("staff");
            }
            if (label1.Text == "2.14 Modifier")
            {
                addfunc("rmodifier");
            }
            if (label1.Text == "2.15 Discount Keys")
            {
                addfunc("discountkeys");
            }
            if (label1.Text == "2.16 Shifts")
            {
                addfunc("shifts");
            }
            if (label1.Text == "2.17 Service Charges")
            {
                addfunc("service");
            }
            if (label1.Text == "2.18 Banks")
            {
                addfunc("banks");
            }
            if (label1.Text == "4.3 Employee Sale")
            {
                addfunc("emprecv");
            }
            if (label1.Text == "2.19 Loyality Cards")
            {
                addfunc("cards");
            }
            if (label1.Text == "2.20 Print Type")
            {
                addfunc("printtype");
            }
            if (label1.Text == "5.3 Attach Menu")
            {
                addfunc("attach");
            }
            if (label1.Text == "2.18 Extra Flavour")
            {
                addfunc("extra");
            }
            if (label1.Text == "5.1 Deal Head")
            {
                addfunc("dealhead");
            }
            if (label1.Text == "5.2 Deals")
            {
                addfunc("deal");
            }
            if (label1.Text == "2.21 Discount Compaign")
            {
                addfunc("discountcompaign");
            }
            if (label1.Text == "4.4 Employees")
            {
                addfunc("employees");
            }

            if (label1.Text == "4.5 Employees Salary")
            {
                addfunc("employeessalary");
            }
            if (label1.Text == "4.6 Attandance")
            {
                addfunc("attandance");
            }
            if (label1.Text == "3.2 Expenses")
            {
                addfunc("expenses");
            }
            if (label1.Text == "1.12 Delivery Areas")
            {
                addfunc("area");
            }
            if (label1.Text == "2.22 Order Source")
            {
                addfunc("ordersource");
            }
            if (label1.Text == "2.23 Sub Items")
            {
                addfunc("subitems");
            }
            if (label1.Text == "2.24 Vouchers")
            {
                addfunc("vouchers");
            }
            if (label1.Text == "2.25 Table Design")
            {
                addfunc("tables");
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
                    if (label1.Text == "2.1 Menu Group")
                    {
                        editfunc("MenuG");
                    }
                    if (label1.Text == "2.2 Menu Item")
                    {
                        editfunc("MenuI");
                    }
                    if (label1.Text == "2.3 Currency")
                    {
                        editfunc("Currency");
                    }
                    if (label1.Text == "2.4 MOP")
                    {
                        editfunc("MOP");
                    }
                    if (label1.Text == "2.7 GST")
                    {
                        editfunc("GST");
                    }
                    if (label1.Text == "2.5 Printer Setting")
                    {
                        editfunc("Printer");
                    }
                    if (label1.Text == "2.8 Device Setting")
                    {
                        editfunc("Device");
                    }
                    if (label1.Text == "2.9 Online DB Setting")
                    {
                        editfunc("db");
                    }
                    if (label1.Text == "2.10 Layout Setting")
                    {
                        editfunc("layout");
                    }
                    if (label1.Text == "1.11 Supplier")
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
                    if (label1.Text == "2.11 KOT Setting")
                    {
                        editfunc("kds");
                    }
                    if (label1.Text == "2.12 Customer Info")
                    {
                        editfunc("customers");
                    }
                    if (label1.Text == "2.13 Members Point Setting")
                    {
                        editfunc("points");
                    }
                    if (label1.Text == "4.2 Staff")
                    {
                        editfunc("staff");
                    }
                    if (label1.Text == "2.14 Modifier")
                    {
                        editfunc("rmodifier");
                    }
                    if (label1.Text == "2.15 Discount Keys")
                    {
                        editfunc("discountkeys");
                    }
                    if (label1.Text == "2.16 Shifts")
                    {
                        editfunc("shifts");
                    }
                    if (label1.Text == "2.17 Service Charges")
                    {
                        editfunc("service");
                    }
                    if (label1.Text == "2.18 Banks")
                    {
                        editfunc("banks");
                    }
                    if (label1.Text == "4.3 Employee Sale")
                    {
                        editfunc("emprecv");
                    }
                    if (label1.Text == "2.19 Loyality Cards")
                    {
                        editfunc("cards");
                    }
                    if (label1.Text == "2.20 Print Type")
                    {
                        editfunc("printtype");
                    }
                    if (label1.Text == "5.3 Attach Menu")
                    {
                        editfunc("attach");
                    }
                    if (label1.Text == "2.18 Extra Flavour")
                    {
                        editfunc("extra");
                    }
                    if (label1.Text == "5.1 Deal Head")
                    {
                        editfunc("dealhead");
                    }
                    if (label1.Text == "5.2 Deals")
                    {
                        editfunc("deal");
                    }

                    if (label1.Text == "2.21 Discount Compaign")
                    {
                        editfunc("discountcompaign");
                    }
                    if (label1.Text == "4.4 Employees")
                    {
                        editfunc("employees");
                    }
                    if (label1.Text == "4.5 Employees Salary")
                    {
                        editfunc("employeessalary");
                    }
                    if (label1.Text == "4.6 Attandance")
                    {
                        editfunc("attandance");
                    }
                    
                    if (label1.Text == "3.2 Expenses")
                    {
                        editfunc("expenses");
                    }
                    if (label1.Text == "1.12 Delivery Areas")
                    {
                        editfunc("area");
                    }
                    if (label1.Text == "2.22 Order Source")
                    {
                        editfunc("ordersource");
                    }
                    if (label1.Text == "2.23 Sub Items")
                    {
                        editfunc("subitems");
                    }
                    if (label1.Text == "2.24 Vouchers")
                    {
                        editfunc("vouchers");
                    }
                    if (label1.Text == "2.25 Table Design")
                    {
                        editfunc("tables");
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
                    if (label1.Text == "2.1 Menu Group")
                    {
                        delfunc("MenuG", id.ToString());
                    }
                    if (label1.Text == "2.2 Menu Item")
                    {
                        delfunc("MenuI", id.ToString());
                    }
                    if (label1.Text == "2.3 Currency")
                    {
                        delfunc("Currency", id.ToString());
                    }
                    if (label1.Text == "2.4 MOP")
                    {
                        delfunc("MOP", id.ToString());
                    }
                    if (label1.Text == "2.7 GST")
                    {
                        delfunc("GST", id.ToString());
                    }
                    if (label1.Text == "2.5 Printer Setting")
                    {
                        delfunc("Printer", id.ToString());

                    }
                    if (label1.Text == "2.8 Device Setting")
                    {
                        delfunc("Device", id.ToString());
                    }
                    if (label1.Text == "2.9 Online DB Setting")
                    {
                        delfunc("db", id.ToString());
                    }
                    if (label1.Text == "2.10 Layout Setting")
                    {
                        delfunc("layout", id.ToString());
                    }
                    if (label1.Text == "1.11 Supplier")
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
                    if (label1.Text == "2.11 KOT Setting")
                    {
                        delfunc("kds", id.ToString());
                    }
                    if (label1.Text == "2.12 Customer Info")
                    {
                        delfunc("customers", id.ToString());
                    }
                    if (label1.Text == "2.13 Members Point Setting")
                    {
                        delfunc("points", id.ToString());
                    }
                    if (label1.Text == "4.2 Staff")
                    {
                        delfunc("staff", id.ToString());
                    }
                    if (label1.Text == "2.14 Modifier")
                    {
                        delfunc("rmodifier", id.ToString());
                    }
                    if (label1.Text == "2.15 Discount Keys")
                    {
                        delfunc("discountkeys", id.ToString());
                    }
                    if (label1.Text == "2.16 Shifts")
                    {
                        delfunc("shifts", id.ToString());
                    }
                    if (label1.Text == "2.17 Service Charges")
                    {
                        delfunc("service", id.ToString());
                    }
                    if (label1.Text == "2.18 Banks")
                    {
                        delfunc("banks", id.ToString());
                    }
                    if (label1.Text == "4.3 Employee Sale")
                    {
                        delfunc("emprecv", id.ToString());
                    }
                    if (label1.Text == "2.19 Loyality Cards")
                    {
                        delfunc("cards", id.ToString());
                    }
                    if (label1.Text == "2.20 Print Type")
                    {
                        id = dataGridView1.Rows[indx].Cells[1].Value.ToString();
                        delfunc("printtype", id.ToString());
                    }
                    if (label1.Text == "5.3 Attach Menu")
                    {
                        delfunc("attach", id.ToString());
                    }

                    if (label1.Text == "2.18 Extra Flavour")
                    {
                        delfunc("extra", id.ToString());
                    }
                    if (label1.Text == "5.1 Deal Head")
                    {
                        delfunc("dealhead", id.ToString());
                    }
                    if (label1.Text == "5.2 Deals")
                    {
                        delfunc("deal", id.ToString());
                    }
                    if (label1.Text == "2.21 Discount Compaign")
                    {
                        delfunc("discountcompaign", id.ToString());                        
                    }
                    if (label1.Text == "4.4 Employees")
                    {
                        delfunc("employees",id.ToString());
                    }
                    if (label1.Text == "4.5 Employees Salary")
                    {
                        delfunc("employeessalary", id.ToString());
                    }
                    if (label1.Text == "4.6 Attandance")
                    {
                        delfunc("attandance", id.ToString());
                    }
                    
                    if (label1.Text == "3.2 Expenses")
                    {
                        delfunc("expenses", id.ToString());
                    }
                    if (label1.Text == "1.12 Delivery Areas")
                    {
                        delfunc("area", id.ToString());
                    }
                    if (label1.Text == "2.22 Order Source")
                    {
                        delfunc("ordersource", id.ToString());
                    }
                    if (label1.Text == "2.23 Sub Items")
                    {
                        delfunc("subitems", id.ToString());
                    }
                    if (label1.Text == "2.24 Vouchers")
                    {
                        delfunc("vouchers", id.ToString());
                    }

                    if (label1.Text == "2.25 Table Design")
                    {
                        delfunc("tables", id.ToString());
                    }
                }
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (label1.Text == "1.8 Branch")
                {
                    if (e.ColumnIndex == 2)
                    {
                        string q = "update Branch set BranchCode='" + dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                }
                if (label1.Text == "2.2 Menu Item")
                {
                    if (e.ColumnIndex == 6)
                    {
                        string q = "update menuitem set price='" + dataGridView1.Rows[e.RowIndex].Cells["Price"].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                       // objCore.executeQuery(q);
                        float price = float.Parse(dataGridView1.Rows[e.RowIndex].Cells["Price"].Value.ToString());
                        float grossprice = price;
                        try
                        {
                            q = "select gst from gst where type='Cash'";
                            float gst = 0;
                            DataSet ds = new DataSet();
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string temp = ds.Tables[0].Rows[0][0].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                gst = float.Parse(temp);
                            }
                            
                            grossprice = price * ((gst / 100) + 1);
                        }
                        catch (Exception ex)
                        {
                         
                        }

                        q = "update menuitem set price='" + dataGridView1.Rows[e.RowIndex].Cells["Price"].Value.ToString() + "',Grossprice='" + grossprice + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                        dataGridView1.Rows[e.RowIndex].Cells["GrossPrice"].Value = Math.Round(grossprice, 2);
                    }
                    if (e.ColumnIndex == 7)
                    {
                        string q = "select gst from gst where type='Cash'";
                        float gst = 0;
                        DataSet ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string temp = ds.Tables[0].Rows[0][0].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            gst = float.Parse(temp);
                        }
                        float price = float.Parse(dataGridView1.Rows[e.RowIndex].Cells["GrossPrice"].Value.ToString());
                        float netprice = price / ((gst / 100) + 1);
                        q = "update menuitem set price='" + Math.Round(netprice, 2) + "',Grossprice='" + price + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                        dataGridView1.Rows[e.RowIndex].Cells[6].Value = Math.Round(netprice, 2);
                    }
                    if (e.ColumnIndex == 1)
                    {
                        string q = "update menuitem set Name='" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                    if (e.ColumnIndex == 5)
                    {
                        string q = "update menuitem set barcode='" + dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                    if (e.ColumnIndex == 8)
                    {
                        string q = "update menuitem set Price2='" + dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                    if (e.ColumnIndex == 9)
                    {
                        string q = "update menuitem set Price3='" + dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                    if (e.ColumnIndex == 10)
                    {
                        string q = "update menuitem set Status='" + dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                    if (e.ColumnIndex == 11)
                    {
                        string q = "update menuitem set modifiercount='" + dataGridView1.Rows[e.RowIndex].Cells[11].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}

