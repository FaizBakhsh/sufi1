using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.forms
{
    public partial class DataBackUp : Form
    {
        BackgroundWorker bg2 = new BackgroundWorker();
        POSRestaurant.forms.BackendForm _frm;
        bool chk = false;
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        string cs = "";
        string type = "";
        public DataBackUp(POSRestaurant.forms.BackendForm frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void DataUploading_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new System.Data.DataSet();
                ds = objcore.funGetDataSet("select * from SqlServerInfo");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string server = ds.Tables[0].Rows[0]["ServerName"].ToString();
                    string db = ds.Tables[0].Rows[0]["DbName"].ToString();
                    string user = ds.Tables[0].Rows[0]["UserName"].ToString();
                    string password = ds.Tables[0].Rows[0]["Password"].ToString();
                    cs = "Data Source=" + server + ";Initial Catalog=" + db + ";Persist Security Info=True;User ID=" + user + ";Password=" + password + "";
                }
                label1.Text = "";
                label2.Text = "";
            }
            catch (Exception ex)
            {


            }
        }
        private void myBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                int count = 0;
                int percntcount = 0;
                string branchcode = "";
                DataSet ds = new System.Data.DataSet();
                ds = objcore.funGetDataSet("select * from Branch");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchcode = ds.Tables[0].Rows[0]["id"].ToString();
                }


                SqlConnection connection = new SqlConnection(cs);

                SqlCommand com;
                // cs = "Persist Security Info=True;Integrated Security = true; User ID=DB_99F0DF_ideamedia_admin;Password=ideamobile Initial Catalog=LBGPOS;Data Source=SQL5009.Smarterasp.net";
                DataSet dsbarcode = new DataSet();
                string q = "";
                if (type == "Full")
                {
                    q = "select * from Barcode";
                }
                else
                {
                    q = "select * from Barcode where uploadstatus ='Pending'";
                }
                dsbarcode = objcore.funGetDataSet(q);
                if (dsbarcode.Tables[0].Rows.Count > 0)
                {
                    count = count + dsbarcode.Tables[0].Rows.Count;
                }
                DataSet dsBranch = new DataSet();

                if (type == "Full")
                {
                    q = "select * from Branch";

                }
                else
                {
                    q = "select * from Branch where uploadstatus ='Pending'";
                }
                dsBranch = objcore.funGetDataSet(q);
                if (dsBranch.Tables[0].Rows.Count > 0)
                {
                    count = count + dsBranch.Tables[0].Rows.Count;
                }
                DataSet dsBilltype = new DataSet();

                if (type == "Full")
                {
                    q = "select * from BillType";

                }
                else
                {
                    q = "select * from BillType where uploadstatus ='Pending'";
                }
                dsBilltype = objcore.funGetDataSet(q);
                if (dsBilltype.Tables[0].Rows.Count > 0)
                {
                    count = count + dsBilltype.Tables[0].Rows.Count;
                }

                DataSet dsBrands = new DataSet();

                if (type == "Full")
                {
                    q = "select * from Brands";

                }
                else
                {
                    q = "select * from Brands where uploadstatus ='Pending'";
                }
                dsBrands = objcore.funGetDataSet(q);
                if (dsBrands.Tables[0].Rows.Count > 0)
                {
                    count = count + dsBrands.Tables[0].Rows.Count;
                }


                DataSet dsCategory = new DataSet();

                if (type == "Full")
                {
                    q = "select * from Category";

                }
                else
                {
                    q = "select * from Category where uploadstatus ='Pending'";
                }
                dsCategory = objcore.funGetDataSet(q);

                if (dsCategory.Tables[0].Rows.Count > 0)
                {
                    count = count + dsCategory.Tables[0].Rows.Count;
                }


                DataSet dsColor = new DataSet();

                if (type == "Full")
                {
                    q = "select * from Color";

                }
                else
                {
                    q = "select * from Color where uploadstatus ='Pending'";
                }
                dsColor = objcore.funGetDataSet(q);
                if (dsColor.Tables[0].Rows.Count > 0)
                {
                    count = count + dsColor.Tables[0].Rows.Count;
                }

                DataSet dsCompanyInfo = new DataSet();
                if (type == "Full")
                {
                    q = "select * from CompanyInfo";

                }
                else
                {
                    q = "select * from CompanyInfo where uploadstatus ='Pending'";
                }
                dsCompanyInfo = objcore.funGetDataSet(q);
                if (dsCompanyInfo.Tables[0].Rows.Count > 0)
                {
                    count = count + dsCompanyInfo.Tables[0].Rows.Count;
                }

                DataSet dsCurrency = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Currency";

                }
                else
                {
                    q = "select * from Currency where uploadstatus ='Pending'";
                }
                dsCurrency = objcore.funGetDataSet(q);
                if (dsCurrency.Tables[0].Rows.Count > 0)
                {
                    count = count + dsCurrency.Tables[0].Rows.Count;
                }
                DataSet dsCutomers = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Customers";

                }
                else
                {
                    q = "select * from Customers where uploadstatus ='Pending'";
                }
                dsCutomers = objcore.funGetDataSet(q);
                if (dsCutomers.Tables[0].Rows.Count > 0)
                {
                    count = count + dsCutomers.Tables[0].Rows.Count;
                }
                DataSet dsDayEnd = new DataSet();
                if (type == "Full")
                {
                    q = "select * from DayEnd";

                }
                else
                {
                    q = "select * from DayEnd where uploadstatus ='Pending'";
                }
                dsDayEnd = objcore.funGetDataSet(q);
                if (dsDayEnd.Tables[0].Rows.Count > 0)
                {
                    count = count + dsDayEnd.Tables[0].Rows.Count;
                }

                DataSet dsDelivery = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Delivery";

                }
                else
                {
                    q = "select * from Delivery where uploadstatus ='Pending'";
                }
                dsDelivery = objcore.funGetDataSet(q);
                if (dsDelivery.Tables[0].Rows.Count > 0)
                {
                    count = count + dsDelivery.Tables[0].Rows.Count;
                }

                DataSet dsDeviceSetting = new DataSet();
                if (type == "Full")
                {
                    q = "select * from DeviceSetting";
                }
                else
                {
                    q = "select * from DeviceSetting where uploadstatus ='Pending'";
                }
                dsDeviceSetting = objcore.funGetDataSet(q);
                if (dsDeviceSetting.Tables[0].Rows.Count > 0)
                {
                    count = count + dsDeviceSetting.Tables[0].Rows.Count;
                }
                DataSet dsDinInTables = new DataSet();
                if (type == "Full")
                {
                    q = "select * from DinInTables";

                }
                else
                {
                    q = "select * from DinInTables where uploadstatus ='Pending'";
                }
                dsDinInTables = objcore.funGetDataSet(q);
                if (dsDinInTables.Tables[0].Rows.Count > 0)
                {
                    count = count + dsDinInTables.Tables[0].Rows.Count;
                }
                DataSet dsDiscountKeys = new DataSet();
                if (type == "Full")
                {
                    q = "select * from DiscountKeys";
                }
                else
                {
                    q = "select * from DiscountKeys where uploadstatus ='Pending'";
                }
                dsDiscountKeys = objcore.funGetDataSet(q);
                if (dsDiscountKeys.Tables[0].Rows.Count > 0)
                {
                    count = count + dsDiscountKeys.Tables[0].Rows.Count;
                }
                DataSet dsGroups = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Groups";

                }
                else
                {
                    q = "select * from Groups where uploadstatus ='Pending'";
                }
                dsGroups = objcore.funGetDataSet(q);
                if (dsGroups.Tables[0].Rows.Count > 0)
                {
                    count = count + dsGroups.Tables[0].Rows.Count;
                }

                DataSet dsGST = new DataSet();
                if (type == "Full")
                {
                    q = "select * from GST";

                }
                else
                {
                    q = "select * from GST where uploadstatus ='Pending'";
                }
                dsGST = objcore.funGetDataSet(q);
                if (dsGST.Tables[0].Rows.Count > 0)
                {
                    count = count + dsGST.Tables[0].Rows.Count;
                }
                DataSet dsInventory = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Inventory";

                }
                else
                {
                    q = "select * from Inventory where uploadstatus ='Pending'";
                }
                dsInventory = objcore.funGetDataSet(q);
                if (dsInventory.Tables[0].Rows.Count > 0)
                {
                    count = count + dsInventory.Tables[0].Rows.Count;
                }
                DataSet dskds = new DataSet();
                if (type == "Full")
                {
                    q = "select * from KDS";

                }
                else
                {
                    q = "select * from KDS where uploadstatus ='Pending'";
                }
                dskds = objcore.funGetDataSet(q);
                if (dskds.Tables[0].Rows.Count > 0)
                {
                    count = count + dskds.Tables[0].Rows.Count;
                }
                DataSet dsMenuGroup = new DataSet();
                if (type == "Full")
                {
                    q = "select * from MenuGroup";

                }
                else
                {
                    q = "select * from MenuGroup where uploadstatus ='Pending'";
                }
                dsMenuGroup = objcore.funGetDataSet(q);
                if (dsMenuGroup.Tables[0].Rows.Count > 0)
                {
                    count = count + dsMenuGroup.Tables[0].Rows.Count;
                }
                DataSet dsMenuItem = new DataSet();
                if (type == "Full")
                {
                    q = "select * from MenuItem";
                }
                else
                {
                    q = "select * from MenuItem where uploadstatus ='Pending'";
                }
                dsMenuItem = objcore.funGetDataSet(q);
                if (dsMenuItem.Tables[0].Rows.Count > 0)
                {
                    count = count + dsMenuItem.Tables[0].Rows.Count;
                }
                DataSet dsModifier = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Modifier";
                }
                else
                {
                    q = "select * from Modifier where uploadstatus ='Pending'";
                }
                dsModifier = objcore.funGetDataSet(q);
                if (dsModifier.Tables[0].Rows.Count > 0)
                {
                    count = count + dsModifier.Tables[0].Rows.Count;
                }
                DataSet dsModifierflavour = new DataSet();
                if (type == "Full")
                {
                    q = "select * from ModifierFlavour";
                }
                else
                {
                    q = "select * from ModifierFlavour where uploadstatus ='Pending'";
                }
                dsModifierflavour = objcore.funGetDataSet(q);
                if (dsModifierflavour.Tables[0].Rows.Count > 0)
                {
                    count = count + dsModifierflavour.Tables[0].Rows.Count;
                }

                DataSet dsMOP = new DataSet();
                if (type == "Full")
                {
                    q = "select * from MOP";
                }
                else
                {
                    q = "select * from MOP where uploadstatus ='Pending'";
                }
                dsMOP = objcore.funGetDataSet(q);
                if (dsMOP.Tables[0].Rows.Count > 0)
                {
                    count = count + dsMOP.Tables[0].Rows.Count;
                }

                DataSet dsPrinters = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Printers";
                }
                else
                {
                    q = "select * from Printers where uploadstatus ='Pending'";
                }
                dsPrinters = objcore.funGetDataSet(q);
                if (dsPrinters.Tables[0].Rows.Count > 0)
                {
                    count = count + dsPrinters.Tables[0].Rows.Count;
                }
                DataSet dspurchase = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Purchase";
                }
                else
                {
                    q = "select * from Purchase where uploadstatus ='Pending'";
                }
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    count = count + dspurchase.Tables[0].Rows.Count;
                }
                DataSet dspurchasedetails = new DataSet();
                if (type == "Full")
                {
                    q = "select * from PurchaseDetails";
                }
                else
                {
                    q = "select * from PurchaseDetails where uploadstatus ='Pending'";
                }
                dspurchasedetails = objcore.funGetDataSet(q);
                if (dspurchasedetails.Tables[0].Rows.Count > 0)
                {
                    count = count + dspurchasedetails.Tables[0].Rows.Count;
                }
                DataSet dspurchasereturn = new DataSet();
                if (type == "Full")
                {
                    q = "select * from PurchaseReturn";
                }
                else
                {
                    q = "select * from PurchaseReturn where uploadstatus ='Pending'";
                }
                dspurchasereturn = objcore.funGetDataSet(q);
                if (dspurchasereturn.Tables[0].Rows.Count > 0)
                {
                    count = count + dspurchasereturn.Tables[0].Rows.Count;
                }
                DataSet dspurchasereturndetails = new DataSet();
                if (type == "Full")
                {
                    q = "select * from PurchasereturnDetails";
                }
                else
                {
                    q = "select * from PurchasereturnDetails where uploadstatus ='Pending'";
                }
                dspurchasereturndetails = objcore.funGetDataSet(q);
                if (dspurchasereturndetails.Tables[0].Rows.Count > 0)
                {
                    count = count + dspurchasereturndetails.Tables[0].Rows.Count;
                }
                DataSet dsRawItem = new DataSet();
                if (type == "Full")
                {
                    q = "select * from RawItem";
                }
                else
                {
                    q = "select * from RawItem where uploadstatus ='Pending'";
                }
                dsRawItem = objcore.funGetDataSet(q);
                if (dsRawItem.Tables[0].Rows.Count > 0)
                {
                    count = count + dsRawItem.Tables[0].Rows.Count;
                }

                DataSet dsRecipe = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Recipe";

                }
                else
                {
                    q = "select * from Recipe where uploadstatus ='Pending'";
                }
                dsRecipe = objcore.funGetDataSet(q);
                if (dsRecipe.Tables[0].Rows.Count > 0)
                {
                    count = count + dsRecipe.Tables[0].Rows.Count;
                }

                DataSet dsRefund = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Refund";

                }
                else
                {
                    q = "select * from Refund where uploadstatus ='Pending'";
                }
                dsRefund = objcore.funGetDataSet(q);
                if (dsRefund.Tables[0].Rows.Count > 0)
                {
                    count = count + dsRefund.Tables[0].Rows.Count;
                }
                DataSet dsruntimemodofier = new DataSet();
                if (type == "Full")
                {
                    q = "select * from RuntimeModifier";

                }
                else
                {
                    q = "select * from RuntimeModifier where uploadstatus ='Pending'";
                }
                dsruntimemodofier = objcore.funGetDataSet(q);
                if (dsruntimemodofier.Tables[0].Rows.Count > 0)
                {
                    count = count + dsruntimemodofier.Tables[0].Rows.Count;
                }
                DataSet dsSize = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Size";

                }
                else
                {
                    q = "select * from Size where uploadstatus ='Pending'";
                }
                dsSize = objcore.funGetDataSet(q);
                if (dsSize.Tables[0].Rows.Count > 0)
                {
                    count = count + dsSize.Tables[0].Rows.Count;
                }

                DataSet dsSqlServerInfo = new DataSet();
                if (type == "Full")
                {
                    q = "select * from SqlServerInfo";

                }
                else
                {
                    q = "select * from SqlServerInfo where uploadstatus ='Pending'";
                }
                dsSqlServerInfo = objcore.funGetDataSet(q);
                if (dsSqlServerInfo.Tables[0].Rows.Count > 0)
                {
                    count = count + dsSqlServerInfo.Tables[0].Rows.Count;
                }

                DataSet dsStores = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Stores";

                }
                else
                {
                    q = "select * from Stores where uploadstatus ='Pending'";
                }
                dsStores = objcore.funGetDataSet(q);
                if (dsStores.Tables[0].Rows.Count > 0)
                {
                    count = count + dsStores.Tables[0].Rows.Count;
                }
                DataSet dsSupplier = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Supplier";

                }
                else
                {
                    q = "select * from Supplier where uploadstatus ='Pending'";
                }
                dsSupplier = objcore.funGetDataSet(q);
                if (dsSupplier.Tables[0].Rows.Count > 0)
                {
                    count = count + dsSupplier.Tables[0].Rows.Count;
                }
                DataSet dsTabletorders = new DataSet();
                if (type == "Full")
                {
                    q = "select * from TabletOrders";

                }
                else
                {
                    q = "select * from TabletOrders where uploadstatus ='Pending'";
                }
                dsTabletorders = objcore.funGetDataSet(q);
                if (dsTabletorders.Tables[0].Rows.Count > 0)
                {
                    count = count + dsTabletorders.Tables[0].Rows.Count;
                }
                DataSet dsTablelayout = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Tablelayout";
                }
                else
                {
                    q = "select * from Tablelayout where uploadstatus ='Pending'";
                }
                dsTablelayout = objcore.funGetDataSet(q);
                if (dsTablelayout.Tables[0].Rows.Count > 0)
                {
                    count = count + dsTablelayout.Tables[0].Rows.Count;
                }
                DataSet dsTakeAway = new DataSet();
                if (type == "Full")
                {
                    q = "select * from TakeAway";

                }
                else
                {
                    q = "select * from TakeAway where uploadstatus ='Pending'";
                }
                dsTakeAway = objcore.funGetDataSet(q);
                if (dsTakeAway.Tables[0].Rows.Count > 0)
                {
                    count = count + dsTakeAway.Tables[0].Rows.Count;
                }

                DataSet dsType = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Type";

                }
                else
                {
                    q = "select * from Type where uploadstatus ='Pending'";
                }
                dsType = objcore.funGetDataSet(q);
                if (dsType.Tables[0].Rows.Count > 0)
                {
                    count = count + dsType.Tables[0].Rows.Count;
                }

                DataSet dsUOM = new DataSet();
                if (type == "Full")
                {
                    q = "select * from UOM";

                }
                else
                {
                    q = "select * from UOM where uploadstatus ='Pending'";
                }
                dsUOM = objcore.funGetDataSet(q);
                if (dsUOM.Tables[0].Rows.Count > 0)
                {
                    count = count + dsUOM.Tables[0].Rows.Count;
                }

                DataSet dsUOMConversion = new DataSet();
                if (type == "Full")
                {
                    q = "select * from UOMConversion";

                }
                else
                {
                    q = "select * from UOMConversion where uploadstatus ='Pending'";
                }
                dsUOMConversion = objcore.funGetDataSet(q);
                if (dsUOMConversion.Tables[0].Rows.Count > 0)
                {
                    count = count + dsUOMConversion.Tables[0].Rows.Count;
                }

                DataSet dsUsers = new DataSet();
                if (type == "Full")
                {
                    q = "select * from Users";

                }
                else
                {
                    q = "select * from Users where uploadstatus ='Pending'";
                }
                dsUsers = objcore.funGetDataSet(q);
                if (dsUsers.Tables[0].Rows.Count > 0)
                {
                    count = count + dsUsers.Tables[0].Rows.Count;
                }

                DataSet dsVoidBills = new DataSet();
                if (type == "Full")
                {
                    q = "select * from VoidBills";

                }
                else
                {
                    q = "select * from VoidBills where uploadstatus ='Pending'";
                }
                dsVoidBills = objcore.funGetDataSet(q);
                if (dsVoidBills.Tables[0].Rows.Count > 0)
                {
                    count = count + dsVoidBills.Tables[0].Rows.Count;
                }
                DataSet dsusercard = new DataSet();
                if (type == "Full")
                {
                    q = "select * from VoidBills";

                }
                else
                {
                    q = "select * from VoidBills where uploadstatus ='Pending'";
                }
                dsusercard = objcore.funGetDataSet(q);
                if (dsusercard.Tables[0].Rows.Count > 0)
                {
                    count = count + dsusercard.Tables[0].Rows.Count;
                }
                string qry = "";
                qry = "sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Open();
                com = new SqlCommand(qry, connection);
                com.ExecuteNonQuery();
                
                if (dsbarcode.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsbarcode.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Barcode where id='" + dsbarcode.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into barcode (id,Code,branchid) values('" + dsbarcode.Tables[0].Rows[i]["id"].ToString() + "','" + dsbarcode.Tables[0].Rows[i]["Code"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update barcode set uploadstatus='Uploaded' where id='" + dsbarcode.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsBranch.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsBranch.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Branch where id='" + dsBranch.Tables[0].Rows[i]["id"].ToString() + "' ";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Branch (id,BranchName,BranchCode,Location) values('" + dsBranch.Tables[0].Rows[i]["id"].ToString() + "','" + dsBranch.Tables[0].Rows[i]["BranchName"].ToString() + "','" + dsBranch.Tables[0].Rows[i]["BranchCode"].ToString() + "','" + dsBranch.Tables[0].Rows[i]["Location"].ToString() + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Branch set uploadstatus='Uploaded' where id='" + dsBranch.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsBrands.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsBrands.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Brands where id='" + dsBrands.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Brands (id,BrandName,Description,branchid) values('" + dsBrands.Tables[0].Rows[i]["id"].ToString() + "','" + dsBrands.Tables[0].Rows[i]["BrandName"].ToString() + "','" + dsBrands.Tables[0].Rows[i]["Description"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Brands set uploadstatus='Uploaded' where id='" + dsBrands.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsCategory.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsCategory.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Category where id='" + dsCategory.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Category (id,GroupId,CategoryName,CategoryDiscription,branchid) values('" + dsCategory.Tables[0].Rows[i]["id"].ToString() + "','" + dsCategory.Tables[0].Rows[i]["GroupId"].ToString() + "','" + dsCategory.Tables[0].Rows[i]["CategoryName"].ToString() + "','" + dsCategory.Tables[0].Rows[i]["CategoryDiscription"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Category set uploadstatus='Uploaded' where id='" + dsCategory.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsColor.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsColor.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Color where id='" + dsColor.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Color (id,ColorName,branchid,Caption) values('" + dsColor.Tables[0].Rows[i]["id"].ToString() + "','" + dsColor.Tables[0].Rows[i]["ColorName"].ToString() + "','" + branchcode + "','" + dsColor.Tables[0].Rows[i]["Caption"].ToString() + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Color set uploadstatus='Uploaded' where id='" + dsColor.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                //if (dsColor.Tables[0].Rows.Count > 0)
                //{


                //    for (int i = 0; i < dsColor.Tables[0].Rows.Count; i++)
                //    {
                //        try
                //        {
                //            if (connection.State == ConnectionState.Open)
                //            {
                //                connection.Close();
                //            }
                //            connection.Open();
                //            qry = "delete from Color where id='" + dsColor.Tables[0].Rows[i]["id"].ToString() + "' and branchCode='" + branchcode + "'";
                //            com = new SqlCommand(qry, connection);
                //            com.ExecuteNonQuery();
                //            qry = "insert into Color (id,ColorName,BranchCode) values('" + dsColor.Tables[0].Rows[i]["id"].ToString() + "','" + dsColor.Tables[0].Rows[i]["ColorName"].ToString() + "','" + branchcode + "')";
                //            if (connection.State == ConnectionState.Open)
                //            {
                //                connection.Close();
                //            }
                //            connection.Open();
                //            com = new SqlCommand(qry, connection);
                //            com.ExecuteNonQuery();
                //            objcore.executeQuery("update Color set uploadstatus='Uploaded' where id='" + dsColor.Tables[0].Rows[i]["id"].ToString() + "'");
                //            percntcount = percntcount + 1;
                //            int percentage = (percntcount) * 100 / count;
                //            bg2.ReportProgress(percentage);
                //        }
                //        catch (Exception ex)
                //        {

                //            chk = true;
                //        }
                //    }
                //}
                if (dsCompanyInfo.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsCompanyInfo.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from CompanyInfo where id='" + dsCompanyInfo.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into CompanyInfo (id,Name,Address,WellComeNote,Phone,branchid) values ('" + dsCompanyInfo.Tables[0].Rows[i]["id"].ToString() + "','" + dsCompanyInfo.Tables[0].Rows[i]["Name"].ToString() + "','" + dsCompanyInfo.Tables[0].Rows[i]["Address"].ToString() + "','" + dsCompanyInfo.Tables[0].Rows[i]["WellComeNote"].ToString() + "','" + dsCompanyInfo.Tables[0].Rows[i]["Phone"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update CompanyInfo set uploadstatus='Uploaded' where id='" + dsCompanyInfo.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }
                if (dsCurrency.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsCurrency.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Currency where id='" + dsCurrency.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Currency (id,Name,branchid) values('" + dsCurrency.Tables[0].Rows[i]["id"].ToString() + "','" + dsCurrency.Tables[0].Rows[i]["Name"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Currency set uploadstatus='Uploaded' where id='" + dsCurrency.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsDayEnd.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsDayEnd.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from DayEnd where id='" + dsDayEnd.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into DayEnd (id,Date,DayStatus,UserId,branchid) values('" + dsDayEnd.Tables[0].Rows[i]["id"].ToString() + "','" + dsDayEnd.Tables[0].Rows[i]["Date"].ToString() + "','" + dsDayEnd.Tables[0].Rows[i]["DayStatus"].ToString() + "','" + dsDayEnd.Tables[0].Rows[i]["UserId"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update DayEnd set uploadstatus='Uploaded' where id='" + dsDayEnd.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsDelivery.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsDelivery.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Delivery where id='" + dsDelivery.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Delivery (id,OrderNo,Name,Phone,PostalCode,Address,Note,SaleId,Status,Date,branchid) values('" + dsDelivery.Tables[0].Rows[i]["id"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["OrderNo"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["Name"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["Phone"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["PostalCode"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["Address"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["Note"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["SaleId"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["Status"].ToString() + "','" + dsDelivery.Tables[0].Rows[i]["Date"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Delivery set uploadstatus='Uploaded' where id='" + dsDelivery.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsDeviceSetting.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsDeviceSetting.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from DeviceSetting where id='" + dsDeviceSetting.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into DeviceSetting (id,Device,Status,branchid) values('" + dsDeviceSetting.Tables[0].Rows[i]["id"].ToString() + "','" + dsDeviceSetting.Tables[0].Rows[i]["Device"].ToString() + "','" + dsDeviceSetting.Tables[0].Rows[i]["Status"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update DeviceSetting set uploadstatus='Uploaded' where id='" + dsDeviceSetting.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsDinInTables.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsDinInTables.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from DinInTables where id='" + dsDinInTables.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into DinInTables (id,TableNo,Saleid,WaiterId,Date,time,Status,branchid) values('" + dsDinInTables.Tables[0].Rows[i]["id"].ToString() + "','" + dsDinInTables.Tables[0].Rows[i]["TableNo"].ToString() + "','" + dsDinInTables.Tables[0].Rows[i]["Saleid"] + "','" + dsDinInTables.Tables[0].Rows[i]["WaiterId"] + "','" + dsDinInTables.Tables[0].Rows[i]["Date"].ToString() + "','" + dsDinInTables.Tables[0].Rows[i]["time"].ToString() + "','" + dsDinInTables.Tables[0].Rows[i]["Status"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update DinInTables set uploadstatus='Uploaded' where id='" + dsDinInTables.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsGroups.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsGroups.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Groups where id='" + dsGroups.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Groups (id,GroupName,Description,branchid) values('" + dsGroups.Tables[0].Rows[i]["id"].ToString() + "','" + dsGroups.Tables[0].Rows[i]["GroupName"].ToString() + "','" + dsGroups.Tables[0].Rows[i]["Description"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Groups set uploadstatus='Uploaded' where id='" + dsGroups.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsGST.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsGST.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from GST where id='" + dsGST.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into GST (id,GST,branchid) values('" + dsGST.Tables[0].Rows[i]["id"].ToString() + "','" + dsGST.Tables[0].Rows[i]["GST"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update GST set uploadstatus='Uploaded' where id='" + dsGST.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsMenuGroup.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsMenuGroup.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from MenuGroup where id='" + dsMenuGroup.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into MenuGroup (id,Name,ColorId,Description,Status,Image,FontColorId,FontSize,branchid) values('" + dsMenuGroup.Tables[0].Rows[i]["id"].ToString() + "','" + dsMenuGroup.Tables[0].Rows[i]["Name"].ToString() + "','" + dsMenuGroup.Tables[0].Rows[i]["ColorId"] + "','" + dsMenuGroup.Tables[0].Rows[i]["Description"].ToString() + "','" + dsMenuGroup.Tables[0].Rows[i]["Status"].ToString() + "','" + dsMenuGroup.Tables[0].Rows[i]["Image"] + "','" + dsMenuGroup.Tables[0].Rows[i]["FontColorId"] + "','" + dsMenuGroup.Tables[0].Rows[i]["FontSize"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update MenuGroup set uploadstatus='Uploaded' where id='" + dsMenuGroup.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsMenuItem.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsMenuItem.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from MenuItem where id='" + dsMenuItem.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into MenuItem (id,Code,Name,MenuGroupId,BarCode,Price,Status,ColorId,KDSId,Image,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,branchid) values('" + dsMenuItem.Tables[0].Rows[i]["id"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["Code"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["Name"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["MenuGroupId"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["BarCode"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["Price"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["Status"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["ColorId"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["KDSId"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["Image"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["FontColorId"] + "','" + dsMenuItem.Tables[0].Rows[i]["FontSize"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["Minutes"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["alarmtime"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["minuteskdscolor"].ToString() + "','" + dsMenuItem.Tables[0].Rows[i]["alarmkdscolor"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update MenuItem set uploadstatus='Uploaded' where id='" + dsMenuItem.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsModifier.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsModifier.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Modifier where id='" + dsModifier.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Modifier (id,MenuGroupId,MenuItemId,RawItemId,Name,Price,Quantity,kdsid,branchid) values('" + dsModifier.Tables[0].Rows[i]["id"].ToString() + "','" + dsModifier.Tables[0].Rows[i]["MenuGroupId"].ToString() + "','" + dsModifier.Tables[0].Rows[i]["MenuItemId"].ToString() + "','" + dsModifier.Tables[0].Rows[i]["RawItemId"].ToString() + "','" + dsModifier.Tables[0].Rows[i]["Name"].ToString() + "','" + dsModifier.Tables[0].Rows[i]["Price"].ToString() + "','" + dsModifier.Tables[0].Rows[i]["Quantity"].ToString() + "','" + dsModifier.Tables[0].Rows[i]["kdsid"] + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Modifier set uploadstatus='Uploaded' where id='" + dsModifier.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsMOP.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsMOP.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from MOP where id='" + dsMOP.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into MOP (id,Name,branchid) values('" + dsMOP.Tables[0].Rows[i]["id"].ToString() + "','" + dsMOP.Tables[0].Rows[i]["Name"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update MOP set uploadstatus='Uploaded' where id='" + dsMOP.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsPrinters.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsPrinters.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Printers where id='" + dsPrinters.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Printers (id,Name,type,prints,branchid) values('" + dsPrinters.Tables[0].Rows[i]["id"].ToString() + "','" + dsPrinters.Tables[0].Rows[i]["Name"].ToString() + "','" + dsPrinters.Tables[0].Rows[i]["type"].ToString() + "','" + dsPrinters.Tables[0].Rows[i]["prints"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Printers set uploadstatus='Uploaded' where id='" + dsPrinters.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsRawItem.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsRawItem.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from RawItem where id='" + dsRawItem.Tables[0].Rows[i]["id"].ToString() + "' ";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into RawItem (id,GroupId,CategoryId,TypeId,BrandId,UOMId,SizeId,ColorId,BranchId,StoreId,Revenueid,Costofsalesid,Inventoryid,ItemName,BarCode,Code,Price,Img,Description) values('" + dsRawItem.Tables[0].Rows[i]["id"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["GroupId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["CategoryId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["TypeId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["BrandId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["UOMId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["SizeId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["ColorId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["BranchId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["StoreId"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["Revenueid"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["Costofsalesid"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["Inventoryid"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["ItemName"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["BarCode"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["Code"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["Price"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["Img"].ToString() + "','" + dsRawItem.Tables[0].Rows[i]["Description"].ToString() + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update RawItem set uploadstatus='Uploaded' where id='" + dsRawItem.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsRecipe.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsRecipe.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Recipe where id='" + dsRecipe.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Recipe (id,MenuItemId,RawItemId,UOMCId,Quantity,Cost,branchid,modifierid) values('" + dsRecipe.Tables[0].Rows[i]["id"].ToString() + "','" + dsRecipe.Tables[0].Rows[i]["MenuItemId"].ToString() + "','" + dsRecipe.Tables[0].Rows[i]["RawItemId"].ToString() + "','" + dsRecipe.Tables[0].Rows[i]["UOMCId"].ToString() + "','" + dsRecipe.Tables[0].Rows[i]["Quantity"].ToString() + "','" + dsRecipe.Tables[0].Rows[i]["Cost"].ToString() + "','" + branchcode + "','" + dsRecipe.Tables[0].Rows[i]["modifierid"].ToString() + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Recipe set uploadstatus='Uploaded' where id='" + dsRecipe.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsRefund.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsRefund.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Refund where id='" + dsRefund.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Refund (id,SaleId,Reason,branchid) values('" + dsRefund.Tables[0].Rows[i]["id"].ToString() + "','" + dsRefund.Tables[0].Rows[i]["SaleId"].ToString() + "','" + dsRefund.Tables[0].Rows[i]["Reason"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Refund set uploadstatus='Uploaded' where id='" + dsRefund.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsSize.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsSize.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Size where id='" + dsSize.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Size (id,SizeCode,SizeName,branchid) values('" + dsSize.Tables[0].Rows[i]["id"].ToString() + "','" + dsSize.Tables[0].Rows[i]["SizeCode"].ToString() + "','" + dsSize.Tables[0].Rows[i]["SizeName"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Size set uploadstatus='Uploaded' where id='" + dsSize.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsSqlServerInfo.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsSqlServerInfo.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from SqlServerInfo where id='" + dsSqlServerInfo.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into SqlServerInfo (id,ServerName,DbName,UserName,Password,branchid) values('" + dsSqlServerInfo.Tables[0].Rows[i]["id"].ToString() + "','" + dsSqlServerInfo.Tables[0].Rows[i]["ServerName"].ToString() + "','" + dsSqlServerInfo.Tables[0].Rows[i]["DbName"].ToString() + "','" + dsSqlServerInfo.Tables[0].Rows[i]["UserName"].ToString() + "','" + dsSqlServerInfo.Tables[0].Rows[i]["Password"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update SqlServerInfo set uploadstatus='Uploaded' where id='" + dsSqlServerInfo.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsStores.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsStores.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Stores where id='" + dsStores.Tables[0].Rows[i]["id"].ToString() + "' ";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Stores (id,StoreName,StoreCode,BranchId) values('" + dsStores.Tables[0].Rows[i]["id"].ToString() + "','" + dsStores.Tables[0].Rows[i]["StoreName"].ToString() + "','" + dsStores.Tables[0].Rows[i]["StoreCode"].ToString() + "','" + dsStores.Tables[0].Rows[i]["BranchId"].ToString() + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Stores set uploadstatus='Uploaded' where id='" + dsStores.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsTakeAway.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsTakeAway.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from TakeAway where id='" + dsTakeAway.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into TakeAway (id,CustomerId,Date,time,Saleid,Status,branchid) values('" + dsTakeAway.Tables[0].Rows[i]["id"].ToString() + "','" + dsTakeAway.Tables[0].Rows[i]["CustomerId"].ToString() + "','" + dsTakeAway.Tables[0].Rows[i]["Date"].ToString() + "','" + dsTakeAway.Tables[0].Rows[i]["time"].ToString() + "','" + dsTakeAway.Tables[0].Rows[i]["Saleid"].ToString() + "','" + dsTakeAway.Tables[0].Rows[i]["Status"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update TakeAway set uploadstatus='Uploaded' where id='" + dsTakeAway.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsType.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsType.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Type where id='" + dsType.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Type (id,groupId,CategoryId,TypeName,TypeDiscription,branchid) values('" + dsType.Tables[0].Rows[i]["id"].ToString() + "','" + dsType.Tables[0].Rows[i]["groupId"].ToString() + "','" + dsType.Tables[0].Rows[i]["CategoryId"].ToString() + "','" + dsType.Tables[0].Rows[i]["TypeName"].ToString() + "','" + dsType.Tables[0].Rows[i]["TypeDiscription"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Type set uploadstatus='Uploaded' where id='" + dsType.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsUOM.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsUOM.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from UOM where id='" + dsUOM.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into UOM (id,UOM,Description,branchid) values('" + dsUOM.Tables[0].Rows[i]["id"].ToString() + "','" + dsUOM.Tables[0].Rows[i]["UOM"].ToString() + "','" + dsUOM.Tables[0].Rows[i]["Description"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update UOM set uploadstatus='Uploaded' where id='" + dsUOM.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsUOMConversion.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsUOMConversion.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from UOMConversion where id='" + dsUOMConversion.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into UOMConversion (id,UOMId,Qty,ConversionRate,UOM,branchid) values('" + dsUOMConversion.Tables[0].Rows[i]["id"].ToString() + "','" + dsUOMConversion.Tables[0].Rows[i]["UOMId"].ToString() + "','" + dsUOMConversion.Tables[0].Rows[i]["Qty"].ToString() + "','" + dsUOMConversion.Tables[0].Rows[i]["ConversionRate"].ToString() + "','" + dsUOMConversion.Tables[0].Rows[i]["UOM"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update UOMConversion set uploadstatus='Uploaded' where id='" + dsUOMConversion.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsUsers.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsUsers.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Users where id='" + dsUsers.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Users (id,Name,FatherName,Phone,CNICNo,Address,Usertype,CardNo,UserName,Password,Designation,branchid,status) values('" + dsUsers.Tables[0].Rows[i]["id"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["Name"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["FatherName"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["Phone"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["CNICNo"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["Address"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["Usertype"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["CardNo"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["UserName"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["Password"].ToString() + "','" + dsUsers.Tables[0].Rows[i]["Designation"].ToString() + "','" + branchcode + "','" + dsUsers.Tables[0].Rows[i]["status"].ToString() + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Users set uploadstatus='Uploaded' where id='" + dsUsers.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsVoidBills.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsVoidBills.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from VoidBills where id='" + dsVoidBills.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into VoidBills (id,Saleid,Status,branchid) values('" + dsVoidBills.Tables[0].Rows[i]["id"].ToString() + "','" + dsVoidBills.Tables[0].Rows[i]["Saleid"].ToString() + "','" + dsVoidBills.Tables[0].Rows[i]["Status"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update VoidBills set uploadstatus='Uploaded' where id='" + dsVoidBills.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }

                if (dsTablelayout.Tables[0].Rows.Count > 0)
                {


                    for (int i = 0; i < dsTablelayout.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Tablelayout where id='" + dsTablelayout.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Tablelayout (id,Columns,Rows,tablename,RowSize,branchid) values('" + dsTablelayout.Tables[0].Rows[i]["id"].ToString() + "','" + dsTablelayout.Tables[0].Rows[i]["Columns"].ToString() + "','" + dsTablelayout.Tables[0].Rows[i]["Rows"].ToString() + "','" + dsTablelayout.Tables[0].Rows[i]["tablename"].ToString() + "','" + dsTablelayout.Tables[0].Rows[i]["RowSize"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Tablelayout set uploadstatus='Uploaded' where id='" + dsTablelayout.Tables[0].Rows[i]["id"].ToString() + "'");
                            percntcount = percntcount + 1;
                            int percentage = (percntcount) * 100 / count;
                            bg2.ReportProgress(percentage);
                        }
                        catch (Exception ex)
                        {

                            chk = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                chk = true;
            }

        }
        void bg2_completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (chk == true)
            {
                DialogResult rs = MessageBox.Show("Some of Data were not uploaded.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (rs == DialogResult.Retry)
                {

                    type = "Partial";
                    bg2.RunWorkerAsync();
                }
                if (rs == DialogResult.Cancel)
                {
                    label1.Text = "Data Uploaded";
                    //MessageBox.Show("Backup Created Successfully");

                    _frm.Enabled = true;
                }
            }
            else
            {

                MessageBox.Show("Backup Created Successfully");
                vProgressBar1.Value = 0;
                label2.Text = "";
                vButton1.Enabled = true;
                vButton2.Enabled = true;
                vButton3.Enabled = true;
                _frm.Enabled = true;
            }
        }
        void bg2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            vProgressBar1.Value = e.ProgressPercentage;
            if (e.ProgressPercentage > 100)
            {

            }
            else
            {

                label2.Text = e.ProgressPercentage.ToString() + " %";
            }
        }
        public void upload()
        {
            DataSet ds = new System.Data.DataSet();
            ds = objcore.funGetDataSet("select * from Branch");
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["BranchName"].ToString() == string.Empty)
                {
                    MessageBox.Show("Please Define Branch Name First");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Please Define Branch Information First");
                return;
            }
            bool verifyconection = true;
            SqlConnection connection = new SqlConnection(cs);
            try
            {
                label1.Text = "Checking Data Connection";
                connection.Open();
            }
            catch (Exception ex)
            {

                verifyconection = false;
            }

            if (verifyconection == false)
            {
                DialogResult rs = MessageBox.Show("Inavlid data Connection or Internet Connection is not Avaiable.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (rs == DialogResult.Retry)
                {
                    upload();
                    return;
                }
                if (rs == DialogResult.Cancel)
                {
                    _frm.Enabled = true;
                    this.Close();
                }
            }
            else
            {
                label1.Text = "Uploading Data";
                bg2.DoWork += new DoWorkEventHandler(myBGWorker_DoWork);
                bg2.ProgressChanged += new ProgressChangedEventHandler(bg2_ProgressChanged);
                bg2.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bg2_completed);
                bg2.WorkerReportsProgress = true;
                bg2.WorkerSupportsCancellation = true;
                bg2.RunWorkerAsync();
            }





        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            //upload();
        }

        private void DataUploading_Shown(object sender, EventArgs e)
        {



        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            label1.Text = "Preparing for Upload";
            type = "Full";
            vLabel2.Visible = true;
            upload();
            vButton1.Enabled = false;
            vButton2.Enabled = false;
            vButton3.Enabled = false;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            _frm.Enabled = true;
            this.Close();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            label1.Text = "Preparing for Upload";
            type = "Partial";
            vLabel2.Visible = true;
            upload();
            vButton1.Enabled = false;
            vButton2.Enabled = false;
            vButton3.Enabled = false;
        }

        private void vProgressBar1_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}
