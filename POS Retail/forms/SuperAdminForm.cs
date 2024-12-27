using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Net.NetworkInformation;

namespace POSRetail.forms
{
    public partial class SuperAdminForm : Form
    {
        POSRetail.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public string cons = "";
        public SuperAdminForm()
        {
            InitializeComponent();
        }

        private void cardFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm obj = new MainForm();
           // obj.MdiParent = this.MdiParent;
           // obj.WindowState = FormWindowState.Maximized;
            obj.Show();
        }
        public void callfillform()
        {

            fillforms("Production");
            fillforms("Items Sale Summary");
            fillforms("Bill Transfer");
            fillforms("Delivery Status");
            fillforms("Complimentry");
            fillforms("Void");
            fillforms("Targets");
            fillforms("Vouchers");
            fillforms("Update Sold Inventory");
            fillforms("ReOpen");
            fillforms("Reopen");
            fillforms("Sub Items");
            fillforms("Order Source");
            fillforms("Expenses");
            fillforms("Recipe vs Sale");
            fillforms("Delivery Area");
            fillforms("Complete Waste");
            fillforms("Demand");
            fillforms("Inventory Transfer");
            fillforms("Critical Inventory");
            fillforms("Download Purchase");
            fillforms("Demand");
            fillforms("Refund Sales Report");
            fillforms("Hourly Sales Report");
            fillforms("Detailed Sales Reports");
            fillforms("Invoice Wise Report");
            fillforms("Sales Tax Report");
            fillforms("Menu Prices");
            fillforms("Receipe Report");
            fillforms("Food Cost Report");
            fillforms("Discount Details Report");
            fillforms("Customer Sales Report");
            fillforms("Phone Nos Sales Report");
            fillforms("Item List");
            fillforms("Server's Sales Report");
            fillforms("Discount Compaign Report");
            fillforms("Group Wise Sales Report");
            fillforms("Profit n Loss Report");
            fillforms("Critical Inventory(Value)");
            fillforms("Critical Inventory(Quantitative)");
            



            fillforms("Complete Waste");
            fillforms("Demand");
            fillforms("Shift Wise Sales Report");
            fillforms("Daily Sales Report");
            fillforms("Salary Definition");
            fillforms("Setting");
            fillforms("Add Items");
            fillforms("Reports");            
            fillforms("Backup");
            fillforms("Purchase Items");
            fillforms("Purchase Return");
            fillforms("Chart of Accounts");
            fillforms("Chart of Account Report");
            fillforms("Vouchers");
            fillforms("GL Accounts");
            fillforms("Receiveable Statement");
            fillforms("Payable Statement");
            fillforms("Trial balance");
            fillforms("Profit \\ Loss");
            fillforms("Balance Sheet");
            fillforms("Brand Wise Sales Report");
            fillforms("Item Wise Sales Report");
            fillforms("Profit/Loss Report");
            fillforms("Cashier Wise Sales Report");
            fillforms("Terminal Wise Sales Report");
            fillforms("Payment Wise Sales Report");
            fillforms("Discount Sales Report");
            fillforms("Refund Sales Report");
            fillforms("Daily Sales Report");
            fillforms("Hourly Sales Report");
            fillforms("Inventory Received");
            fillforms("Closing Inventory");
            fillforms("Consumed Inventory");
            fillforms("Supplier Wise Purchase");
            fillforms("Brand Wise Purchase");
            fillforms("Item Wise Purchase");
            fillforms("Customers Report");
            fillforms("Accounts");
            fillforms("Group");
            fillforms("Category");
            fillforms("Type");
            fillforms("Brand");
            fillforms("UOM");
            fillforms("Size");
            fillforms("Color");
            fillforms("Branch");
            fillforms("Store");
            fillforms("UOM Conversion");
            fillforms("Supplier");
            fillforms("Menu Group");
            fillforms("Menu Item");
            fillforms("Currency");
            fillforms("MOP");
            fillforms("Printer Setting");
            fillforms("Recipe Modifier");
            fillforms("GST");
            fillforms("Device Setting");
            fillforms("Online DB Setting");
            fillforms("Layout Setting");
            fillforms("KOT Setting");
            fillforms("Customer Info");
            fillforms("Members Point Setting");
            fillforms("Company Info");
            fillforms("Users");
            fillforms("Cash Sales Account");
            fillforms("Chart of Accounts Codes");
            fillforms("Customers");
            fillforms("Discount");
            fillforms("VoidAll");
            fillforms("waste");
            fillforms("VoidOne");
            fillforms("Duplicate");
            fillforms("Day End");
            fillforms("Day Start");
            fillforms("Refund");
            fillforms("SaleReport");
            fillforms("CashDrawer");
            fillforms("Staff");
            fillforms("Cash Transaction");
            fillforms("Cash Transaction Report");
            fillforms("Members");
            fillforms("Member Points");
            fillforms("Menu Item Wise Sales Report");
            fillforms("Menu Group Wise Sales Report");
            fillforms("Shift Wise Sales Report");
            fillforms("Void Sales Report");
            fillforms("Inventory Waste Management");
            fillforms("Weekly Sales Report");
            fillforms("Monthly Sales Report");
            fillforms("Comparative Weekly Sales Report");
            fillforms("Comparative Monthly Sales Report");
            fillforms("Inventory Waste Report");
            fillforms("Add Raw Items");
            fillforms("Recall Bills");
            fillforms("Runtime Modifier Sales Report");
            fillforms("Flavour/Size Sales Report");
            fillforms("Modifier Sales Report");
            fillforms("Waste Management");
            fillforms("Modifier");
            fillforms("Inventory WorkSheet");
            fillforms("Inventory Variance");
            fillforms("Upload Sales");
            fillforms("Download Sales");
            fillforms("Discount Keys");
            fillforms("Shifts");
            fillforms("Variance");
            fillforms("Service Charges");
            fillforms("Banks");
            fillforms("Employee Sale");
            fillforms("Loyality Cards");
            fillforms("Print Type");
            fillforms("Deal Head");
            fillforms("Deals");
            fillforms("Attach Menu");
            fillforms("Discount Compaign");
            fillforms("Requisition(Demand)");
            fillforms("Employees");
            fillforms("Employees Salary");
            fillforms("Expenses");
            fillforms("Item Wise History");
            fillforms("Payables");
            fillforms("Receiveables");    
        }
        public void connecion(string con)
        {
            POSRetail.Properties.Settings.Default.ConnectionString= con;
            POSRetail.Properties.Settings.Default.Save();
            cons = con;
        }
        public void fillforms(string form)
        {
            try
            {
                DataSet dsufilforms = new System.Data.DataSet();
                string q = "select * from Forms where Forms='" + form + "' ";
                //dsufilforms = objcore.funGetDataSet(q);
                SqlConnection connection = new SqlConnection(cons);
                SqlCommand com = new SqlCommand(q, connection);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dsufilforms);
                if (dsufilforms.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    DataSet dss = new DataSet();
                    int idd = 0;
                    dss = objcore.funGetDataSet("select max(id) as id from Forms");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string ii = dss.Tables[0].Rows[0][0].ToString();
                        if (ii == string.Empty)
                        {
                            ii = "0";
                        }
                        idd = Convert.ToInt32(ii) + 1;
                    }
                    else
                    {
                        idd = 1;
                    }
                    q = "insert into Forms(Id,Forms) values('" + idd + "','" + form + "')";
                    objcore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }
        private void addRawItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RawItems.AddRawItems obj = new RawItems.AddRawItems();
            obj.Show();
        }

        private void BackendForm_Load(object sender, EventArgs e)
        {
            this.TopMost = false;
            callfillform();
            try
            {
                DataSet ds = new DataSet();
                string q = "select * from Users";
                ds = objcore.funGetDataSet(q);
               
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";
               
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
            getdata();
        }
        public void getdata()
        {
            try
            {
                string q = "select * from forms";
                DataSet dsfrms = new System.Data.DataSet();
                dsfrms = objcore.funGetDataSet(q);
                dataGridView1.DataSource = dsfrms.Tables[0];
                dataGridView1.Columns[1].Visible = false;
                userforms();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }
        public void userforms()
        {
            try
            {
                if (comboBox1.Text != string.Empty || comboBox1.Text != "Please Select")
                {
                    int i = 0;
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        string id = dr.Cells["id"].Value.ToString();

                        DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dr.Cells[0];
                       

                        DataSet dsuserforms = new System.Data.DataSet();
                        string q = "select * from rights where userid='" + comboBox1.SelectedValue + "' and formid='" + id + "'";
                        dsuserforms = objcore.funGetDataSet(q);
                        if (dsuserforms.Tables[0].Rows.Count > 0)
                        {
                            if (dsuserforms.Tables[0].Rows[0]["status"].ToString().ToLower() == "yes")
                            {
                                chk.Value = true;
                            }
                            else
                            {
                                chk.Value = false;
                            }
                        }
                        else
                        {
                            chk.Value = false;
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("User forms error");
            }
           
        }
        private void Button1_Click(object sender, EventArgs e)
        {

        }

        private void BackendForm_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult rd = MessageBox.Show("Are You Sure to Exit ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rd == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void menuGroupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmMwnuGroupSale obj = new POSRetail.Reports.SaleReports.FrmMwnuGroupSale();
            obj.Show();
        }

        private void menuItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmMenuItemSale obj = new POSRetail.Reports.SaleReports.FrmMenuItemSale();
            obj.Show();
        }

        private void cashierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmCashierSale obj = new POSRetail.Reports.SaleReports.FrmCashierSale();
            obj.Show();
        }

        private void discountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmDiscountSale obj = new POSRetail.Reports.SaleReports.FrmDiscountSale();
            obj.Show();
        }

        private void voidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmVoidSale obj = new POSRetail.Reports.SaleReports.FrmVoidSale();
            obj.Show();
        }

        private void refundToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmRefundSale obj = new POSRetail.Reports.SaleReports.FrmRefundSale();
            obj.Show();
        }

        private void hourlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmHourlySale obj = new POSRetail.Reports.SaleReports.FrmHourlySale();
            obj.Show();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmTerminalSale obj = new POSRetail.Reports.SaleReports.FrmTerminalSale();
            obj.Show();
        }

        private void orderWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmOrderWiseSale obj = new POSRetail.Reports.SaleReports.FrmOrderWiseSale();
            obj.Show();
        }

        private void paymentWiseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.SaleReports.FrmPaymentWiseSale obj = new POSRetail.Reports.SaleReports.FrmPaymentWiseSale();
            obj.Show();
        }

        private void dailyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.FrmUserSale obj = new POSRetail.Reports.FrmUserSale();
            obj.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {


        }

        private void uploadeSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void backUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            Reports obj = new Reports();
            
            obj.Show();
        }
        public static string GetMACAddress2()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    //IPInterfaceProperties properties = adapter.GetIPProperties(); Line is not required
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }
        public string getkey()
        {
            string val = POSRetail.Properties.Settings.Default.key.ToString();
            return val;
        }
        public string getdate()
        {
            string val = POSRetail.Properties.Settings.Default.KeyDate.ToString();
            return val;
        }
        public string getname()
        {
            string val = POSRetail.Properties.Settings.Default.name.ToString();
            return val;
        }
        public void register(string no,string name)
        {
            try
            {
                string mc = GetMACAddress2();
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                DataSet dsreg = new System.Data.DataSet();
                int id = 1;

                dsreg = objcore.funGetDataSet("select max(id) as id from reg");
                if (dsreg.Tables[0].Rows.Count > 0)
                {
                    string i = dsreg.Tables[0].Rows[0][0].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    id = Convert.ToInt32(i) + 1;

                }
                else
                {

                }
                objcore.executeQuery("insert into reg(id,RegNo,Date,UploadStatus)values('" + id + "','" + no + "','" + date + "','Pending')");
                //objcore.executeQuery("insert into reg(id,RegNo,Date,UploadStatus)values('" + id + "','" + mc + "','" + date + "','Pending')");
                POSRetail.Properties.Settings.Default.KeyDate = date;
                POSRetail.Properties.Settings.Default.key = no;
                POSRetail.Properties.Settings.Default.name = name;
                POSRetail.Properties.Settings.Default.Save();
               
                MessageBox.Show("Software Registered Successfully");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are You Sure to Register This Software","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Superadminpassword ob = new Superadminpassword(this);
                ob.Show();
            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            RawItems.AddRawItems obj = new RawItems.AddRawItems();
            obj.Show();
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            DialogResult rd = MessageBox.Show("Are You Sure to Exit ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (rd == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void vButton7_Click(object sender, EventArgs e)
        {            
        }
        private void vButton8_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.Inventory.frmClosingInventory obj = new POSRetail.Reports.Inventory.frmClosingInventory();
            obj.Show();
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.Inventory.frmConsumedInventory obj = new POSRetail.Reports.Inventory.frmConsumedInventory();
            obj.Show();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            POSRetail.Reports.Inventory.frmReceivedInventory obj = new POSRetail.Reports.Inventory.frmReceivedInventory();
            obj.Show();
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void vButton11_Click(object sender, EventArgs e)
        {
            Reports obj = new Reports();

            obj.Show();
        }

        private void vTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            userforms();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
           
            if (comboBox1.Text == string.Empty)
            {
                MessageBox.Show("Please Select User");
                return;
            }
            DialogResult dr = MessageBox.Show("Are you sure to apply changings ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                changeuserforms();
                userforms();
            }
        }
        public void changeuserforms()
        {
            try
            {
                if (comboBox1.Text != string.Empty || comboBox1.Text != "Please Select")
                {
                    int i = 0;
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        string id = dr.Cells["id"].Value.ToString();

                       // DataGridViewCheckBoxCell chk = (DataGridViewCheckBoxCell)dr.Cells[0];

                        string val = this.dataGridView1.Rows[i].Cells[0].Value.ToString();
                        //if (val.ToLower() == "true")
                        {
                            DataSet dsuserforms = new System.Data.DataSet();
                            string q = "select * from rights where userid='" + comboBox1.SelectedValue + "' and formid='" + id + "'";
                            dsuserforms = objcore.funGetDataSet(q);
                            if (dsuserforms.Tables[0].Rows.Count > 0)
                            {

                                if (val.ToLower() == "true")
                                {
                                    val = "yes";
                                }
                                else
                                {
                                    val = "no";
                                }
                                q = "update rights set status='" + val + "' where id='" + dsuserforms.Tables[0].Rows[0]["id"] + "'";
                                objcore.executeQuery(q);
                            }
                            else
                            {

                                if (val.ToLower() == "true")
                                {
                                    val = "yes";
                                }
                                else
                                {
                                    val = "No";
                                }
                                DataSet dss = new DataSet();
                                int idd = 0;
                                dss = objcore.funGetDataSet("select max(id) as id from rights");
                                if (dss.Tables[0].Rows.Count > 0)
                                {
                                    string ii = dss.Tables[0].Rows[0][0].ToString();
                                    if (ii == string.Empty)
                                    {
                                        ii = "0";
                                    }
                                    idd = Convert.ToInt32(ii) + 1;
                                }
                                else
                                {
                                    idd = 1;
                                }
                                q = "insert into rights(Id,formid,Userid, status) values('" + idd + "','" + id + "','" + comboBox1.SelectedValue + "','" + val + "')";
                                objcore.executeQuery(q);
                            }
                            
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("User forms error");
            }

        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex != -1)
            //{
            //    dataGridView1.Rows[e.RowIndex].Cells[0].Value = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string val=this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if(val=="True")
                {
                    val="true";
                }
                if(val=="False")
                {
                    val="false";
                }
                bool b =Convert.ToBoolean(val);
                this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !b;
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[0].Value = true;
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[0].Value = false;

            }
        }
        private double getprice(string id, string date)
        {
            if (id == "69")
            {

            }
            double variance = 0, price = 0;
            string val = "";
            DataSet dspurchase = new DataSet();
            string q = "";

            q = "SELECT        MONTH(Date) AS Expr2, YEAR(Date) AS Expr3, AVG(price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date between '" + date + "' and '" + date + "') and dbo.InventoryTransfer.ItemId = '" + id + "'  GROUP BY MONTH(Date), YEAR(Date)";

            dspurchase = objCore.funGetDataSet(q);
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

                q = "SELECT    top 1    (price) AS Expr1 FROM            dbo.InventoryTransfer WHERE        (dbo.InventoryTransfer.sourcebranchid IS NOT NULL) and ( dbo.InventoryTransfer.date <= '" + date + "' ) and dbo.InventoryTransfer.ItemId = '" + id + "'  order by date desc ";

                dspurchase = objCore.funGetDataSet(q);
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

                q = "SELECT     AVG(dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date between '" + date + "' and '" + date + "') and RawItemId = '" + id + "'";

                dspurchase = objCore.funGetDataSet(q);
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

                q = "SELECT     top 1 (dbo.PurchaseDetails.priceperitem) AS Expr1 FROM         dbo.PurchaseDetails INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id WHERE ( dbo.Purchase.date <= '" + date + "') and RawItemId = '" + id + "' order by dbo.Purchase.Id desc";

                dspurchase = objCore.funGetDataSet(q);
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
                dspurchase = objCore.funGetDataSet(q);
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

            return price;
        }
        public double getcostmenu1(string start, string end, string id)
        {
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

        public void inventoryaccount(string date)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";
                q = "delete from InventoryAccount where VoucherNo='SJV-P-" + date + "'";
                objcore.executeQuery(q);
                q = "delete from InventoryAccount where VoucherNo='SJV-F-" + date + "'";
                objcore.executeQuery(q);
                
                q = "delete from CostSalesAccount where VoucherNo='SJV-P-" + date + "'";
                objcore.executeQuery(q);
                q = "delete from CostSalesAccount where VoucherNo='SJV-F-" + date + "'";
                objcore.executeQuery(q);
                {
                    string ChartAccountId = "";

                    double prcpacking = 0, pricenonpacking = 0;
                    string val = "";
                    q = "select * from InventoryConsumed where date='" + date + "'";
                    q = "SELECT        dbo.InventoryConsumed.Id, dbo.InventoryConsumed.RawItemId, dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.InventoryConsumed.branchid,                          dbo.InventoryConsumed.uploadstatus, dbo.InventoryConsumed.OnlineId, dbo.InventoryConsumed.kdsid FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.InventoryConsumed ON dbo.RawItem.Id = dbo.InventoryConsumed.RawItemId  where dbo.InventoryConsumed.date='" + date + "' and dbo.Type.TypeName='Packing'";
                    DataSet dsinv = new DataSet();
                    dsinv = objcore.funGetDataSet(q);
                    for (int i = 0; i < dsinv.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string itemid = dsinv.Tables[0].Rows[i]["RawItemId"].ToString();
                            string temp = dsinv.Tables[0].Rows[i]["QuantityConsumed"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            double qty = Convert.ToDouble(temp);
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
                                qty = qty / rate;
                            }
                            qty = Math.Round(qty, 3);
                            prcpacking = prcpacking + (qty * getcostmenu1(date, date, itemid));
                        }
                        catch (Exception ex)
                        {

                        }
                    }


                    q = "SELECT        dbo.InventoryConsumed.Id, dbo.InventoryConsumed.RawItemId, dbo.InventoryConsumed.QuantityConsumed, dbo.InventoryConsumed.RemainingQuantity, dbo.InventoryConsumed.Date, dbo.InventoryConsumed.branchid,                          dbo.InventoryConsumed.uploadstatus, dbo.InventoryConsumed.OnlineId, dbo.InventoryConsumed.kdsid FROM            dbo.Type INNER JOIN                         dbo.RawItem ON dbo.Type.Id = dbo.RawItem.TypeId INNER JOIN                         dbo.InventoryConsumed ON dbo.RawItem.Id = dbo.InventoryConsumed.RawItemId  where dbo.InventoryConsumed.date='" + date + "' and dbo.Type.TypeName !='Packing'";
                    dsinv = new DataSet();
                    dsinv = objcore.funGetDataSet(q);
                    for (int i = 0; i < dsinv.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string itemid = dsinv.Tables[0].Rows[i]["RawItemId"].ToString();
                            string temp = dsinv.Tables[0].Rows[i]["QuantityConsumed"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            double qty = Convert.ToDouble(temp);
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
                                qty = qty / rate;
                            }
                            qty = Math.Round(qty, 3);
                            pricenonpacking = pricenonpacking + (qty * getcostmenu1(date, date, itemid));
                        }
                        catch (Exception ex)
                        {

                        }
                    }


                    if (prcpacking+pricenonpacking <= 0)
                    {
                        return;
                    }
                    dsacount = new DataSet();


                    q = "select * from CashSalesAccountsList where AccountType='Inventory Account'  and branchid='" + branchid + "'";
                    DataSet dsacountchk = new DataSet();
                    dsacountchk = objCore.funGetDataSet(q);
                    if (dsacountchk.Tables[0].Rows.Count > 0)
                    {
                        ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    int iddd = 0;
                    DataSet ds = objCore.funGetDataSet("select max(id) as id from InventoryAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    val = "";

                    q = "insert into InventoryAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + date + "','" + ChartAccountId + "','SJV-P-" + date + "','Packing Cost','0','" + Math.Round(Convert.ToDouble(prcpacking), 2) + "','0','" + branchid + "')";
                    objCore.executeQuery(q);
                    ds = objCore.funGetDataSet("select max(id) as id from InventoryAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    q = "insert into InventoryAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + date + "','" + ChartAccountId + "','SJV-F-" + date + "','Food Cost','0','" + Math.Round(Convert.ToDouble(pricenonpacking), 2) + "','0','" + branchid + "')";
                    objCore.executeQuery(q);


                    ChartAccountId = "";
                    q = "select * from CashSalesAccountsList where AccountType='Cost of Sales Account'  and branchid='" + branchid + "'";
                    dsacountchk = new DataSet();
                    dsacountchk = objCore.funGetDataSet(q);
                    if (dsacountchk.Tables[0].Rows.Count > 0)
                    {
                        ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CostSalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }


                    q = "insert into CostSalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + date + "','" + ChartAccountId + "','SJV-P-" + date + "','Packing Cost','" + Math.Round(Convert.ToDouble(prcpacking), 2) + "','0','0','" + branchid + "')";
                    objCore.executeQuery(q);
                    iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CostSalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    q = "insert into CostSalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + date + "','" + ChartAccountId + "','SJV-F-" + date + "','Food Cost','" + Math.Round(Convert.ToDouble(pricenonpacking), 2) + "','0','0','" + branchid + "')";
                    objCore.executeQuery(q);

                }
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton3_Click_1(object sender, EventArgs e)
        {
            string q = "UPDATE    Saledetails  SET              Itemdiscount = '0', ItemdiscountPerc = '0', ItemGst = '0', ItemGstPerc = '0'";
            objcore.executeQuery(q);
            DataSet ds = new System.Data.DataSet();
            q = "select * from sale where date >= '2015-05-24'";
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                float dis = 0;
                string val = ds.Tables[0].Rows[i]["discount"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                dis = float.Parse(val);
                if (dis > 0)
                {
                    int qty = 0;
                    double price = 0;
                    double discount = 0;
                    q = "select * from saledetails where saleid='"+ds.Tables[0].Rows[i]["id"].ToString()+"'";
                    DataSet dsdis = new System.Data.DataSet();
                    dsdis = objcore.funGetDataSet(q);
                    for (int j = 0; j < dsdis.Tables[0].Rows.Count; j++)
                    {
                        qty = Convert.ToInt32(dsdis.Tables[0].Rows[j]["Quantity"].ToString());
                        if (qty == 0)
                        {
                            qty = 1;
                        }
                        val = dsdis.Tables[0].Rows[j]["price"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        price = Convert.ToDouble(val);
                        double tempprice = price * qty;
                        if (tempprice > 0)
                        {
                            discount = (price * dis) / 100;
                            discount = Math.Round(discount, 2);
                            q = "update saledetails set Itemdiscount='" + discount + "' , ItemdiscountPerc='" + dis + "' where id='" + dsdis.Tables[0].Rows[j]["id"].ToString() + "'";
                            objcore.executeQuery(q);
                        }
                    }
                }
                float gst = float.Parse(ds.Tables[0].Rows[i]["GSTPerc"].ToString());
                if (gst > 0)
                {
                    int qty = 0;
                    double price = 0;
                    double gstval = 0;
                    double discount1 = 0;
                    q = "select * from saledetails where saleid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                    DataSet dsdis = new System.Data.DataSet();
                    dsdis = objcore.funGetDataSet(q);
                    for (int j = 0; j < dsdis.Tables[0].Rows.Count; j++)
                    {
                        gstval = 0;
                        discount1 = 0;
                        price = 0;
                        qty = Convert.ToInt32(dsdis.Tables[0].Rows[j]["Quantity"].ToString());
                        if (qty == 0)
                        {
                            qty = 1;
                        }
                        val = dsdis.Tables[0].Rows[j]["price"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        price = Convert.ToDouble(val);                                                
                       // price = price * qty;
                        double tempprice = price * qty;
                        if (tempprice > 0)
                        {
                            gstval = (price * gst) / 100;
                            gstval = Math.Round(gstval, 2);
                        }
                        q = "update saledetails set ItemGst='" + gstval + "' , ItemGstPerc='" + gst + "' where id='" + dsdis.Tables[0].Rows[j]["id"].ToString() + "'";
                        objcore.executeQuery(q);
                    }
                }
            }
            ds = new System.Data.DataSet();
            q = "select * from sale where date < '2015-05-24'";
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                float dis = 0;
                string val = ds.Tables[0].Rows[i]["discount"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                dis = float.Parse(val);
                if (dis > 0)
                {
                    int qty = 0;
                    double price = 0;
                    double discount = 0;
                    q = "select * from saledetails where saleid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                    DataSet dsdis = new System.Data.DataSet();
                    dsdis = objcore.funGetDataSet(q);
                    for (int j = 0; j < dsdis.Tables[0].Rows.Count; j++)
                    {
                        qty = Convert.ToInt32(dsdis.Tables[0].Rows[j]["Quantity"].ToString());
                        if (qty == 0)
                        {
                            qty = 1;
                        }
                        val = dsdis.Tables[0].Rows[j]["price"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        price = Convert.ToDouble(val);

                        double tempprice = price * qty;
                        if (tempprice > 0)
                        {
                            discount = (price * dis) / 100;
                            discount = Math.Round(discount,2);
                            q = "update saledetails set Itemdiscount='" + discount + "' , ItemdiscountPerc='" + dis + "' where id='" + dsdis.Tables[0].Rows[j]["id"].ToString() + "'";
                            objcore.executeQuery(q);
                        }
                    }
                }
                float gst = float.Parse(ds.Tables[0].Rows[i]["GSTPerc"].ToString());
                if (gst > 0)
                {
                    int qty = 0;
                    double price = 0;
                    double gstval = 0;
                    double discount1 = 0;
                    q = "select * from saledetails where saleid='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                    DataSet dsdis = new System.Data.DataSet();
                    dsdis = objcore.funGetDataSet(q);
                    for (int j = 0; j < dsdis.Tables[0].Rows.Count; j++)
                    {
                        gstval = 0;
                        discount1 = 0;
                        price = 0;
                        qty = Convert.ToInt32(dsdis.Tables[0].Rows[j]["Quantity"].ToString());
                        if (qty == 0)
                        {
                            qty = 1;
                        }
                        val = dsdis.Tables[0].Rows[j]["price"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        price = Convert.ToDouble(val);
                        if (dis>0)
                        {
                            discount1 = (price * dis) / 100; 
                        }
                        price = price - discount1;
                        // price = price * qty;
                        double tempprice = price * qty;
                        if (tempprice > 0)
                        {
                            gstval = (price * gst) / 100;
                            gstval = Math.Round(gstval, 2);
                        }
                        q = "update saledetails set ItemGst='" + gstval + "' , ItemGstPerc='" + gst + "' where id='" + dsdis.Tables[0].Rows[j]["id"].ToString() + "'";
                        objcore.executeQuery(q);

                    }
                }
            }
            MessageBox.Show("updated successfully");
        }
        string kitchenwiseconsumption = "";
        public void setinventory()
        {
            DataSet dsinv = new DataSet();
            dsinv = objCore.funGetDataSet("select * from DeviceSetting where device='Kitchen Wise Inventory'");
            if (dsinv.Tables[0].Rows.Count > 0)
            {
                string show = (dsinv.Tables[0].Rows[0]["Status"].ToString());
                try
                {
                    if (show == "Enabled")
                    {
                        kitchenwiseconsumption = "Enabled";

                    }
                    else
                    {
                        kitchenwiseconsumption = "Disabled";

                    }

                }
                catch (Exception ex)
                {

                }
            }
            DataSet ds1 = new System.Data.DataSet();
            string q = "update InventoryConsumed set QuantityConsumed=0,uploadstatus='Pending' where date > '" + dateTimePicker1.Text + "'";
            objcore.executeQuery(q);

            q = "select id,date,billtype from sale where date > '" + dateTimePicker1.Text + "' and billstatus='Paid' order by date";
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToDateTime(ds1.Tables[0].Rows[i]["date"].ToString()).ToShortDateString() == "2015-08-13" || Convert.ToDateTime(ds1.Tables[0].Rows[i]["date"].ToString()).ToShortDateString() == "2015-08-14")
                {
                }
                else
                {
                    q = "select *  from saledetails where saleid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    DataSet ds2 = new System.Data.DataSet();
                    ds2 = objcore.funGetDataSet(q);
                    for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                    {
                        if (ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == "0")
                        {
                            if (ds2.Tables[0].Rows[j]["ModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["ModifierId"].ToString() == "0")
                            {

                                {
                                    float qty = 0;
                                    string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "1";
                                    }
                                    string flv = ds2.Tables[0].Rows[j]["Flavourid"].ToString();
                                    if (flv == "0")
                                    {
                                        flv = "";
                                    }
                                    qty = float.Parse(temp);
                                    recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, ds1.Tables[0].Rows[i]["date"].ToString(), "", ds1.Tables[0].Rows[i]["billtype"].ToString(),"");
                                }
                            }

                            else
                            {
                                float qty = 0;
                                string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                if (temp == "")
                                {
                                    temp = "1";
                                }
                                qty = float.Parse(temp);
                                recipiemodifier(ds2.Tables[0].Rows[j]["ModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString());
                            }
                        }
                        else
                        {
                            float qty = 0;
                            string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                            if (temp == "")
                            {
                                temp = "1";
                            }
                            qty = float.Parse(temp);
                            recipiemodifierruntime(ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString(),"");
                        }
                    }
                }

            }
        }
        public void setinventorysingl()
        {
            DataSet ds1 = new System.Data.DataSet();
            string q = "delete from InventoryConsumed where date > '" + dateTimePicker1.Text + "' and RawItemId='3'";
            //objcore.executeQuery(q);

            q = "select id,date from sale where date > '" + dateTimePicker1.Text + "' order by date";
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                if (Convert.ToDateTime(ds1.Tables[0].Rows[i]["date"].ToString()).ToShortDateString() == "2015-08-13" || Convert.ToDateTime(ds1.Tables[0].Rows[i]["date"].ToString()).ToShortDateString() == "2015-08-14")
                {

                }
                else
                {
                    
                    q = "select *  from saledetails where saleid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    DataSet ds2 = new System.Data.DataSet();
                    ds2 = objcore.funGetDataSet(q);
                    for (int j = 0; j < ds2.Tables[0].Rows.Count; j++)
                    {

                        if (ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString() == "0")
                        {


                            if (ds2.Tables[0].Rows[j]["ModifierId"].ToString() == string.Empty || ds2.Tables[0].Rows[j]["ModifierId"].ToString() == "0")
                            {
                                
                                {
                                    float qty = 0;
                                    string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "1";
                                    }
                                    string flv = ds2.Tables[0].Rows[j]["Flavourid"].ToString();
                                    if (flv == "0")
                                    {
                                        flv = "";
                                    }
                                    qty = float.Parse(temp);
                                    recipie(ds2.Tables[0].Rows[j]["MenuItemId"].ToString(), qty, flv, ds1.Tables[0].Rows[i]["date"].ToString(),"3","","");
                                }
                               
                            }

                            else
                            {
                                float qty = 0;
                                string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                                if (temp == "")
                                {
                                    temp = "1";
                                }
                                qty = float.Parse(temp);
                               // recipiemodifier(ds2.Tables[0].Rows[j]["ModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString());

                            }
                        }
                        else
                        {
                            float qty = 0;
                            string temp = ds2.Tables[0].Rows[j]["Quantity"].ToString();
                            if (temp == "")
                            {
                                temp = "1";
                            }
                            qty = float.Parse(temp);
                            recipiemodifierruntime(ds2.Tables[0].Rows[j]["RunTimeModifierId"].ToString(), qty, ds1.Tables[0].Rows[i]["date"].ToString(),"3");
                        }
                    }
                }

            }
        }
        public void recipiemodifierruntime(string itmid, float itmqnty,string date,string rawid)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";// "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "'";
                // q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Modifier.MenuItemId, dbo.Modifier.RawItemId, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Modifier.Id='" + itmid + "'";
                q = "SELECT     dbo.RuntimeModifier.RawItemId AS RawItemId, dbo.RuntimeModifier.Quantity, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM FROM         dbo.RuntimeModifier INNER JOIN                      dbo.RawItem ON dbo.RuntimeModifier.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RuntimeModifier.Id='" + itmid + "'";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        if (rawid == "")
                        {
                            float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                            double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                            double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                            double amounttodeduct = (qnty / convrate) * recipiqnty;
                            amounttodeduct = amounttodeduct * itmqnty;
                            dsminus = new DataSet();
                            double inventryqty = 0;
                            dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                            if (dsminus.Tables[0].Rows.Count > 0)
                            {
                                inventryqty = double.Parse(dsminus.Tables[0].Rows[i]["Quantity"].ToString());
                                if (amounttodeduct.ToString().Contains("-"))
                                {
                                    amounttodeduct = Math.Abs(amounttodeduct);
                                }
                                q = "update Inventory set Quantity='" + (inventryqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                objCore.executeQuery(q);
                            }
                            dsminus = new DataSet();
                            dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                            if (dsminus.Tables[0].Rows.Count > 0)
                            {
                                double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                                q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                objCore.executeQuery(q);
                            }
                            else
                            {
                                ds = new DataSet();
                                int idcnsmd = 0;

                                ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string ii = ds.Tables[0].Rows[0][0].ToString();
                                    if (ii == string.Empty)
                                    {
                                        ii = "0";
                                    }
                                    idcnsmd = Convert.ToInt32(ii) + 1;
                                }
                                else
                                {
                                    idcnsmd = Convert.ToInt32("1");
                                }
                                // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                objCore.executeQuery(q);
                            }

                        }
                        else
                        {
                            if (rawitmid == rawid)
                            {
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double amounttodeduct = (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;
                                dsminus = new DataSet();
                                double inventryqty = 0;
                                dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    inventryqty = double.Parse(dsminus.Tables[0].Rows[i]["Quantity"].ToString());
                                    if (amounttodeduct.ToString().Contains("-"))
                                    {
                                        amounttodeduct = Math.Abs(amounttodeduct);
                                    }
                                    q = "update Inventory set Quantity='" + (inventryqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                dsminus = new DataSet();
                                dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                                    q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    ds = new DataSet();
                                    int idcnsmd = 0;

                                    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    objCore.executeQuery(q);
                                }
                            }
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
        }
        public static string branchtype = "";
        public static string branchid = "";
        protected void getbranchtype()
        {
            try
            {
                string q = "select id,type from branch";
                DataSet dsb = new DataSet();
                dsb = objCore.funGetDataSet(q);
                if (dsb.Tables[0].Rows.Count > 0)
                {
                    branchtype = dsb.Tables[0].Rows[0][1].ToString();
                    branchid = dsb.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void recipie(string itmid, float itmqnty, string flid, string date, string rawid,string branchtype, string recipetype)
        {
            if (recipetype != "Attachmenu")
            {
                attachMenurecipie(itmid, flid, itmqnty, date, branchtype);
            }
            try
            {
                attachrecipie(itmid, itmqnty, date);

                DataSet dsrecipie = new DataSet();
                string q = "";
                if (flid == "" || flid=="0")
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity,dbo.Recipe.type, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0')";
                }
                else
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.type, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
                }
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        if (branchtype == "")
                        {
                            getbranchtype();
                        }
                        string type = dsrecipie.Tables[0].Rows[i]["type"].ToString();
                        if (type == "" || type.ToLower() == "both")
                        {

                        }
                        else
                        {
                            if (type.ToLower() == "dine in" && branchtype.ToLower() == "take away")
                            {
                                chk = false;
                            }
                            else
                                if (type.ToLower() == "take away" && branchtype.ToLower() == "dine in")
                                {
                                    chk = false;
                                }
                        }
                        if (chk == true)
                        {


                            string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                            if (rawid == "")
                            {


                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);
                                DataSet dsminus = new DataSet();
                                double inventryqty = 0;

                                dsminus = new DataSet();
                                dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    if (inventryqty > 0)
                                    {
                                        rem = inventryqty;
                                    }
                                    q = "update InventoryConsumed set RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    objCore.executeQuery(q);
                                }

                            }
                            else
                            {
                                if (rawitmid == rawid)
                                {
                                    float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                    double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                    double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                    double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                    amounttodeduct = amounttodeduct * itmqnty;
                                    amounttodeduct = Math.Round(amounttodeduct, 3);
                                    DataSet dsminus = new DataSet();
                                    double inventryqty = 0;

                                    dsminus = new DataSet();
                                    dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                    if (dsminus.Tables[0].Rows.Count > 0)
                                    {
                                        double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                        double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                        if (inventryqty > 0)
                                        {
                                            rem = inventryqty;
                                        }
                                        q = "update InventoryConsumed set RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                        objCore.executeQuery(q);
                                    }
                                    else
                                    {
                                        ds = new DataSet();
                                        int idcnsmd = 0;
                                        ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            string ii = ds.Tables[0].Rows[0][0].ToString();
                                            if (ii == string.Empty)
                                            {
                                                ii = "0";
                                            }
                                            idcnsmd = Convert.ToInt32(ii) + 1;
                                        }
                                        else
                                        {
                                            idcnsmd = Convert.ToInt32("1");
                                        }
                                        // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                        q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                        objCore.executeQuery(q);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void attachMenurecipie(string itmid, string flvid, float itmqnty, string date, string branchtype)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                if (flvid == "" || flvid == "0")
                {
                    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where itemid='" + itmid + "' and status='Active' and userecipe='yes' ";
                }
                else
                {
                    q = "SELECT        TOP (200) id, menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status, userecipe FROM            Attachmenu1 where itemid='" + itmid + "' and Flavourid='" + flvid + "' and status='Active' and userecipe='yes' ";
                }
                //q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='MenuItem' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
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
                        float qty = float.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        recipie(menuid, qty, flid, date, "", branchtype, "Attachmenu");
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {

                dsrecipie.Dispose();
                dsminus.Dispose();
            }
        }
        public void attachrecipie(string itmid, float itmqnty, string date)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "";

                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='MenuItem' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        if (branchtype == "")
                        {
                            getbranchtype();
                        }
                        string type = dsrecipie.Tables[0].Rows[i]["type"].ToString();
                        if (type == "" || type.ToLower() == "both")
                        {

                        }
                        else
                        {
                            if (type.ToLower() == "dine in" && branchtype.ToLower() == "take away")
                            {
                                chk = false;
                            }
                            else
                                if (type.ToLower() == "take away" && branchtype.ToLower() == "dine in")
                                {
                                    chk = false;
                                }
                        }
                        if (chk == true)
                        {
                            try
                            {
                                string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["attachQty"].ToString());
                                double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                                amounttodeduct = amounttodeduct * itmqnty;

                                amounttodeduct = amounttodeduct * recipiattachqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);


                                dsminus = new DataSet();
                                double inventryqty = 0;
                                dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");

                                dsminus = new DataSet();
                                dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    if (inventryqty > 0)
                                    {
                                        rem = inventryqty;
                                    }
                                    q = "update InventoryConsumed set uploadstatus='Pending',RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    DataSet ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                                    objCore.executeQuery(q);
                                }
                            }
                            catch (Exception exx)
                            {


                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {

                dsrecipie.Dispose();
                dsminus.Dispose();
            }
        }
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new System.Data.DataSet();
        public void recipiemodifier(string itmid, float itmqnty,string date)
        {
            if (Convert.ToDateTime(date).ToShortDateString() == "2015-09-03" && itmid=="6" )
            {

            }
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";// "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "'";
                q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Modifier.RawItemId, dbo.Modifier.Quantity FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Modifier.Id='" + itmid + "'";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        float qnty = Convert.ToInt32(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        amounttodeduct = Math.Round(amounttodeduct, 3);
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        //dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                        //if (dsminus.Tables[0].Rows.Count > 0)
                        //{
                        //    inventryqty = double.Parse(dsminus.Tables[0].Rows[i]["Quantity"].ToString());
                        //    q = "update Inventory set Quantity='" + (inventryqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                        //    objCore.executeQuery(q);
                        //}
                        //else
                        //{
                        //    ds = new DataSet();
                        //    int idcnsmd = 0;
                        //    ds = objCore.funGetDataSet("select MAX(ID) as id from Inventory");
                        //    if (ds.Tables[0].Rows.Count > 0)
                        //    {
                        //        string ii = ds.Tables[0].Rows[0][0].ToString();
                        //        if (ii == string.Empty)
                        //        {
                        //            ii = "0";
                        //        }
                        //        idcnsmd = Convert.ToInt32(ii) + 1;
                        //    }
                        //    else
                        //    {
                        //        idcnsmd = Convert.ToInt32("1");
                        //    }
                        //    //amounttodeduct = Math.Abs(amounttodeduct);
                        //    // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                        //    q = "insert into Inventory ( Id, RawItemId, Quantity) values('" + idcnsmd + "','" + rawitmid + "','" + -amounttodeduct + "')";
                        //    objCore.executeQuery(q);
                        //}
                        dsminus = new DataSet();
                        dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[i]["QuantityConsumed"].ToString());
                            q = "update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {

                            ds = new DataSet();
                            int idcnsmd = 0;
                            ds = objCore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string ii = ds.Tables[0].Rows[0][0].ToString();
                                if (ii == string.Empty)
                                {
                                    ii = "0";
                                }
                                idcnsmd = Convert.ToInt32(ii) + 1;
                            }
                            else
                            {
                                idcnsmd = Convert.ToInt32("1");
                            }
                            //idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                            q = "insert into InventoryConsumed (branchid,Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')";
                            objCore.executeQuery(q);
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton5_Click_1(object sender, EventArgs e)
        {
            setinventorysingl();
            query obj = new query();
            obj.Show();
        }

        private void vButton6_Click_1(object sender, EventArgs e)
        {
            setinventory();
        }
        
        public void updatebilltype()
        {
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_1 where date='" + dateTimePicker1.Text + "'";
                dsbill = objcore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "insert into billtype (type,saleid,amount) values('" + type + "','" + id + "','" + amount + "')";
                    objcore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
            updatebilltype2();
        }
        public void updatebilltype4()
        {
            DataSet dsbill = new DataSet();
            try
            {
                string q = "SELECT        id, charges  FROM            SerivceCharges";
                dsbill = objCore.funGetDataSet(q);
                if (dsbill.Tables[0].Rows.Count > 0)
                {
                    string temp = dsbill.Tables[0].Rows[0]["charges"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    float charges = float.Parse(temp);
                    if (charges > 0)
                    {
                        q = "select * from sale where servicecharges > 0   and date >= '2018-11-04' order by id desc";
                        dsbill = new DataSet();
                        dsbill = objCore.funGetDataSet(q);
                        for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                        {
                            temp = "0";
                            string otype = dsbill.Tables[0].Rows[i]["OrderType"].ToString();
                            string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                            double servicecharges = 0, totalbill = 0, gst = 0, totalgst = 0, dscount = 0, discountedtotal = 0, service = 0, nettotal = 0;
                            temp = dsbill.Tables[0].Rows[i]["totalbill"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            totalbill = Convert.ToDouble(temp);
                            temp = dsbill.Tables[0].Rows[i]["gstperc"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            gst = Convert.ToDouble(temp);

                            temp = dsbill.Tables[0].Rows[i]["discount"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            dscount = Convert.ToDouble(temp);
                            dscount = (dscount * totalbill) / 100;
                            dscount = Math.Round(dscount, 2);

                            // if (otype == "Dine In")
                            {



                                if (applydiscount() == "before")
                                {
                                    discountedtotal = totalbill;// -dscount;
                                    service = (charges * discountedtotal) / 100;
                                    service = Math.Round(service, 2);
                                    if (otype == "Take Away")
                                    {
                                        service = 0;
                                    }
                                    discountedtotal = discountedtotal + service;
                                    totalgst = (gst * discountedtotal) / 100;
                                    totalgst = Math.Round(totalgst, 2);
                                    discountedtotal = discountedtotal - dscount;
                                }
                                else
                                {
                                    discountedtotal = totalbill - dscount;
                                    service = (charges * discountedtotal) / 100;
                                    service = Math.Round(service, 2);
                                    if (otype == "Take Away")
                                    {
                                        service = 0;
                                    }
                                    totalgst = (gst * discountedtotal) / 100;
                                    totalgst = Math.Round(totalgst, 2);
                                    discountedtotal = discountedtotal + service;
                                }
                                discountedtotal = Math.Round(discountedtotal, 2);

                                nettotal = Math.Round((totalgst + discountedtotal), 2);
                                q = "update sale set discountamount='" + dscount + "',servicecharges='" + service + "',NetBill='" + nettotal + "',gst='" + totalgst + "' where id='" + id + "'";
                                objCore.executeQuery(q);
                                q = "update billtype set amount='" + nettotal + "' where saleid='" + id + "'";
                                objCore.executeQuery(q);

                                q = "select * from saledetails where saleid='"+id+"'";
                                DataSet dsdet = new System.Data.DataSet();
                                dsdet = objcore.funGetDataSet(q);
                                for (int k = 0; k < dsdet.Tables[0].Rows.Count; k++)
                                {
                                    double price = 0, pgst = 0, itemdis = 0, scarges = 0;
                                    temp = dsdet.Tables[0].Rows[k]["price"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "0";
                                    }
                                    price = Convert.ToDouble(temp);

                                    temp = dsdet.Tables[0].Rows[k]["Itemdiscount"].ToString();
                                    if (temp == "")
                                    {
                                        temp = "0";
                                    }
                                    itemdis = Convert.ToDouble(temp);


                                    scarges = (price * charges) / 100;
                                    scarges = Math.Round(scarges, 2);
                                    if (applydiscount() == "before")
                                    {

                                        if (gst > 0 && price > 0)
                                        {
                                            pgst = ((price + scarges) * gst) / 100;
                                            pgst = Math.Round(pgst, 2);
                                        }
                                        else
                                        {
                                            pgst = 0;
                                        }
                                    }
                                    else
                                    {
                                        if (gst > 0 && price > 0)
                                        {
                                            pgst = ((price - itemdis) * gst) / 100;
                                            pgst = Math.Round(pgst, 2);
                                        }
                                        else
                                        {
                                            pgst = 0;
                                        }
                                    }

                                    q = "update saledetails set ItemGst='"+pgst+"' where id='"+dsdet.Tables[0].Rows[k]["id"]+"'";
                                    objcore.executeQuery(q);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        public string applydiscount()
        {
            string apply = "before";
            DataSet dsdis = new DataSet();
            try
            {
                string q = "select * from applydiscount ";

                dsdis = objCore.funGetDataSet(q);
                if (dsdis.Tables[0].Rows.Count > 0)
                {
                    apply = dsdis.Tables[0].Rows[0]["apply"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsdis.Dispose();
            }
            if (apply == "")
            {
                apply = "before";
            }
            return apply;
        }
       
        public void updatebilltype2()
        {
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_2 where date='" + dateTimePicker1.Text + "'";
                dsbill = objcore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    string id = dsbill.Tables[0].Rows[i]["id"].ToString();
                    string type = dsbill.Tables[0].Rows[i]["BillType"].ToString();
                    string amount = dsbill.Tables[0].Rows[i]["NetBill"].ToString();
                    q = "update billtype set amount='" + amount + "' where saleid='" + id + "'";
                    objcore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        protected void update()
        {
            string q = "select id, name, menuItemid from  RuntimeModifier where type='Take Away' and (name LIKE 'cola%')";
            DataSet ds = new System.Data.DataSet();
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                q = "update saledetails set RunTimeModifierId=483 where RunTimeModifierId='" + ds.Tables[0].Rows[i]["id"].ToString() + "'";
                objcore.executeQuery(q);
            }
        }
        private void vButton7_Click_1(object sender, EventArgs e)
        {
            update();
            query obj = new query();
            obj.Show();

        }
        public string getordertype(string id)
        {
            string type = "";
            DataSet dstype = new DataSet();
            try
            {
                string q = "select OrderType from sale where id='" + id + "'";

                dstype = objCore.funGetDataSet(q);
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    type = dstype.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //dstype.Dispose();
            }
            return type;
        }
        
        protected double getdiscountind(string sid)
        {
            double amount = 0;
            try
            {
                string q = "select sum(Discount) from DiscountIndividual where saleid='" + sid + "'";
                DataSet dsf = new DataSet();
                dsf = objCore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    amount = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return amount;
        }
        protected string type(string id)
        {
            string typ = "";
            try
            {
                DataSet dsdetail = new DataSet();
                string q = "select OrderType from sale where id='" + id + "' ";
                dsdetail = objCore.funGetDataSet(q);
                if (dsdetail.Tables[0].Rows.Count > 0)
                {
                    string temp = dsdetail.Tables[0].Rows[0]["OrderType"].ToString();
                    if (temp == "")
                    {
                        temp = "0";

                    }
                    typ = temp;
                }
            }
            catch (Exception ex)
            {


            }

            return typ;
        }
        protected double getdiscountinddetails(string sid)
        {
            double amount = 0;
            try
            {
                string q = "select sum(Discount) from DiscountIndividual where Saledetailsid='" + sid + "'";
                DataSet dsf = new DataSet();
                dsf = objCore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    amount = Convert.ToDouble(temp);
                }
            }
            catch (Exception ex)
            {


            }
            return amount;
        }
        public void updatebilltype3()
        {
           
            double servicecharhes = 0;
            DataSet dsgst = new DataSet();
            try
            {

                dsgst = objCore.funGetDataSet("select * from SerivceCharges");
                if (dsgst.Tables[0].Rows.Count > 0)
                {
                    servicecharhes = float.Parse(dsgst.Tables[0].Rows[0]["charges"].ToString());
                }
                else
                {
                    servicecharhes = 0;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsgst.Dispose();
            }
            DataSet dsbill = new DataSet();
            try
            {
                string q = "select * from View_3 where sum!=netbill";
                dsbill = objCore.funGetDataSet(q);
                for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
                {
                    double price = 0, gst = 0, dis = 0, net = 0, disp = 0;
                    string id = dsbill.Tables[0].Rows[i]["saleid"].ToString();
                    DataSet dsdetail = new DataSet();
                    q = "select * from saledetails where saleid='" + id + "'";
                    dsdetail = objCore.funGetDataSet(q);
                    for (int j = 0; j < dsdetail.Tables[0].Rows.Count; j++)
                    {
                        double discount = 0, gstt = 0, scarges = 0, price0 = 0;
                        try
                        {
                            string val = dsdetail.Tables[0].Rows[j]["price"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            price0 = Convert.ToDouble(val);
                            scarges = (price0 * servicecharhes) / 100;
                            scarges = Math.Round(scarges, 2);
                            val = dsdetail.Tables[0].Rows[j]["ItemdiscountPerc"].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            string ordertyppe = "";
                            ordertyppe = getordertype(id);

                            if (ordertyppe == "Take Away")
                            {
                                scarges = 0;
                            }
                            double dis0 = Convert.ToDouble(val);
                            if (dis0 > 0 && price0 > 0)
                            {
                                discount = (price0 * dis0) / 100;
                                discount = Math.Round(discount, 2);
                            }
                            val = "";
                            val = dsdetail.Tables[0].Rows[j]["ItemGstPerc"].ToString(); ;
                            if (val == "")
                            {
                                val = "0";
                            }
                            gstt = Convert.ToDouble(val);
                            if (applydiscount() == "before")
                            {

                                if (gstt > 0 && price0 > 0)
                                {
                                    gstt = ((price0 + scarges) * gstt) / 100;
                                    gstt = Math.Round(gstt, 2);
                                }
                                else
                                {
                                    gstt = 0;
                                }
                            }
                            else
                            {
                                if (gstt > 0 && price0 > 0)
                                {
                                    gstt = ((price0 - discount) * gstt) / 100;
                                    gstt = Math.Round(gstt, 2);
                                }
                                else
                                {
                                    gstt = 0;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                        double inddisdet = 0;
                        try
                        {
                            inddisdet = getdiscountinddetails(dsdetail.Tables[0].Rows[j]["id"].ToString());
                        }
                        catch (Exception)
                        {

                        }
                        if (inddisdet > 0)
                        {
                            q = "update saledetails set ItemGst='" + gstt + "',Itemdiscount='" + inddisdet + "' where id='" + dsdetail.Tables[0].Rows[j]["id"].ToString() + "'";
                        }
                        else
                        {
                            q = "update saledetails set ItemGst='" + gstt + "' where id='" + dsdetail.Tables[0].Rows[j]["id"].ToString() + "'";
                            
                        }
                        objcore.executeQuery(q);
                        string temp = dsdetail.Tables[0].Rows[j]["price"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }

                        price = price + Convert.ToDouble(temp);

                        gst = gst + Convert.ToDouble(gstt);
                        temp = dsdetail.Tables[0].Rows[j]["Itemdiscount"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }
                        if (inddisdet > 0)
                        {
                            dis = dis + inddisdet;
                        }
                        else
                        {
                            dis = dis + Convert.ToDouble(temp);
                        }
                        temp = dsdetail.Tables[0].Rows[j]["ItemdiscountPerc"].ToString();
                        if (temp == "")
                        {
                            temp = "0";

                        }
                        disp = Convert.ToDouble(temp);
                    }
                    double inddis = 0;
                    try
                    {
                        //inddis = getdiscountind(id);
                    }
                    catch (Exception)
                    {

                    }
                    double service = 0, serviceamount = 0;
                    if (type(id) == "Dine In")
                    {
                        dsdetail = new DataSet();
                        q = "select * from SerivceCharges ";
                        dsdetail = objCore.funGetDataSet(q);
                        if (dsdetail.Tables[0].Rows.Count > 0)
                        {
                            string temp = dsdetail.Tables[0].Rows[0]["charges"].ToString();
                            if (temp == "")
                            {
                                temp = "0";

                            }
                            service = Convert.ToDouble(temp);
                        }
                        try
                        {
                            if (applydiscount() == "before")
                            {
                                serviceamount = ((price) * service) / 100;
                            }
                            else
                            {
                                serviceamount = ((price - dis) * service) / 100;
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        serviceamount = Math.Round(serviceamount, 2);
                    }
                   // MessageBox.Show(((price + gst + serviceamount) - (dis)).ToString()+"-"+id.ToString());
                    q = "update sale set TotalBill='" + price + "',DiscountAmount='" + dis + "',GST='" + gst + "',netbill='" + ((price + gst + serviceamount) - (dis)) + "',discount='" + disp + "',servicecharges='" + serviceamount + "' where id='" + id + "'";
                    objCore.executeQuery(q);
                    q = "select * from billtype where saleid='"+id+"'";
                    dsbill = new System.Data.DataSet();
                    dsbill = objcore.funGetDataSet(q);
                    if (dsbill.Tables[0].Rows.Count == 1)
                    {
                        
                        q = "update billtype set amount='" + ((price + gst + serviceamount) - (dis)) + "' where saleid='" + id + "'";
                        objCore.executeQuery(q);
                        //MessageBox.Show(q);
                    }
                    if (dsbill.Tables[0].Rows.Count == 2)
                    {
                        double vsaamount = 0;
                        for (int l = 0; l < dsbill.Tables[0].Rows.Count; l++)
                        {
                            if (dsbill.Tables[0].Rows[l]["type"].ToString() == "Cash")
                            {

                            }
                            else
                            {
                                string temp = dsbill.Tables[0].Rows[l]["amount"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                vsaamount = Convert.ToDouble(temp);
                            }
                        }
                        q = "update billtype set amount='" + (((price + gst + serviceamount) - (dis + inddis))-vsaamount) + "' where saleid='" + id + "' and type='cash'";
                        objCore.executeQuery(q); 
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsbill.Dispose();
            }
        }
        private void vButton8_Click_1(object sender, EventArgs e)
        {
          
          //  objcore.addcolumns();
            //updatebilltype();
            //updatebilltype3();
        }

        private void vButton1_Click_2(object sender, EventArgs e)
        {
            
            string q = "select distinct terminal from sale where date>='"+dateTimePicker1.Text+"'";
            DataSet dster = new System.Data.DataSet();
            dster = objcore.funGetDataSet(q);
            for (int i = 0; i < dster.Tables[0].Rows.Count; i++)
            {
                double inc = 0;
                string ter = dster.Tables[0].Rows[i]["terminal"].ToString();
                q = "select id from sale where date>='"+dateTimePicker1.Text+"' and terminal='"+ter+"' order by id";
                ds = new System.Data.DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    inc = Convert.ToDouble(ds.Tables[0].Rows[0]["id"].ToString());
                    for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                    {
                        q = "update sale set invoice='" + inc + "' where id='" + ds.Tables[0].Rows[j]["id"].ToString() + "'";
                        objcore.executeQuery(q);
                        inc++;
                    }
                }
            }

        }
        public void saleaccount(string amount, string saleid, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='Sales Account'  and branchid='" + branchid + "'";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from SalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from SalesAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into SalesAccount (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void gstaccount(string amount, string saleid, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='GST Account' and branchid='" + branchid + "'";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from GSTAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from GSTAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into GSTAccount (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void discountaccount(string amount, string saleid, string date)
        {
            try
            {
                string vall = amount;
                if (vall == "")
                {
                    vall = "0";
                }
                DataSet ds = new DataSet();
                if (Convert.ToDouble(vall) > 0)
                {
                    DataSet dsacount = new DataSet();
                    string q = "select * from CashSalesAccountsList where AccountType='Discount Account' and branchid='" + branchid + "'";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                        int iddd = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from DiscountAccount");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string i = ds.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "1";
                            }
                            iddd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            iddd = Convert.ToInt32("1");
                        }
                        double balance = 0;
                        string val = "";
                        q = "select top 1 * from DiscountAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                        dsacount = new DataSet();
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        balance = Convert.ToDouble(val);
                        double newbalance = (balance + Convert.ToDouble(amount));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into DiscountAccount (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                        objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void serviceaccount(string amount, string saleid, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='Service Charges Account' and branchid='" + branchid + "'";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from GSTAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from GSTAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into GSTAccount (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','Bank/Service Charges','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        public void cashaccount(string amount, string saleid, string type, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "";
                //if (type == "Cash")
                //{
                //    q = "select * from CashSalesAccountsList where AccountType='Cash Account' and branchid='" + branchid + "'";
                //}
                //if (type == "Credit Card")
                //{
                //    q = "select * from CashSalesAccountsList where AccountType='Visa Account' and branchid='" + branchid + "'";
                //}
                //if (type == "Master Card")
                //{
                //    q = "select * from CashSalesAccountsList where AccountType='Master Account' and branchid='" + branchid + "'";
                //}
                
                //dsacount = objCore.funGetDataSet(q);

                if (type == "Cash")
                {
                    q = "select * from CashSalesAccountsList where AccountType='Cash Account' and branchid='" + branchid + "'";
                }
                else
                {
                    q = "select * from CashSalesAccountsList where AccountType='Visa Account' and bankid='" + type + "' and branchid='" + branchid + "'";

                }
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count == 0)
                {
                    q = "select * from CashSalesAccountsList where AccountType='Visa Account'  and branchid='" + branchid + "'";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                }
                
                int id = 0;
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    if (type != "Cash")
                    {
                        type = type + "-Bank";
                    }
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CashAccountSales");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CashAccountSales where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);
                    double newbalance = (balance + Convert.ToDouble(amount));

                    q = "insert into CashAccountSales (branchid,Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','" + type + " Sales','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {

            }
        }
        public string gettbleno(string id)
        {
            string tbl = "";
            //DataSet dstbl = new DataSet();
            try
            {

                string q = "select TableNo,guests,ResId from DinInTables where Saleid='" + id + "'";
                //q = "SELECT dbo.DinInTables.TableNo, dbo.ResturantStaff.Name FROM  dbo.DinInTables INNER JOIN               dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id  where dbo.DinInTables.Saleid='" + id + "'";
                // dstbl = objCore.funGetDataSet(q);
                SqlDataReader dr = objCore.funGetDataReader1(q);
                if (dr.Read())
                {
                    string resid = dr[2].ToString();
                    if (resid.Length > 0)
                    {
                        resid = " , Res ID: " + resid;
                    }
                    tbl = "  "+dr[0].ToString() + " , Guests: " + dr[1].ToString() + resid;
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {

            }
            return tbl;
        }

        public void employeeaccount(string amount, string voucherno, string type, string date, string ID,string customer,string saleid)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();

                string q = "select * from Customers where ID='" + ID + "'";
                dsacount = objCore.funGetDataSet(q);
                int id = 0;
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CustomerAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";


                    double newbalance = (balance + Convert.ToDouble(amount));

                    q = "insert into CustomerAccount (branchid,Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + ID + "','" + ChartAccountId + "','" + voucherno + "','Customer: " + customer + ", Bill #: " + saleid +gettbleno(saleid)+ "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        protected void backupdb()
        {
            string path = "C";
            string date = DateTime.Now.ToString("yyyy-MM-dd");
            string dateold = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            try
            {

                System.Data.SqlClient.SqlConnectionStringBuilder builder = new System.Data.SqlClient.SqlConnectionStringBuilder(objcore.getConnectionString());

                string server = builder.DataSource;
                string database = builder.InitialCatalog;
                string q = "EXECUTE master.dbo.xp_delete_file 0,N'" + path + ":\\" + dateold + "db.bak'";
                try
                {
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {

                }
                try
                {
                    q = "backup database " + database + " to disk ='" + path + ":\\" + date + "db.bak'";
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {


                }

                try
                {
                    q = "EXECUTE master.dbo.xp_delete_file 0,N'D:\\" + dateold + "db.bak'";
                    objcore.executeQuery(q);


                    q = "backup database " + database + " to disk ='D:\\" + date + "db.bak'";
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {

                }

                try
                {
                    q = "EXECUTE master.dbo.xp_delete_file 0,N'E:\\" + dateold + "db.bak'";
                    objcore.executeQuery(q);

                    q = "backup database " + database + " to disk ='E:\\" + date + "db.bak'";
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {

                }

                try
                {
                    q = "EXECUTE master.dbo.xp_delete_file 0,N'F:\\" + dateold + "db.bak'";
                    objcore.executeQuery(q);
                    q = "backup database " + database + " to disk ='F:\\" + date + "db.bak'";
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {


                }

                try
                {
                    q = "EXECUTE master.dbo.xp_delete_file 0,N'G:\\" + dateold + "db.bak'";
                    objcore.executeQuery(q);
                    q = "backup database " + database + " to disk ='G:\\" + date + "db.bak'";
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {

                }


                try
                {
                    q = "EXECUTE master.dbo.xp_delete_file 0,N'H:\\" + dateold + "db.bak'";
                    objcore.executeQuery(q);
                    q = "backup database " + database + " to disk ='H:\\" + date + "db.bak'";
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {


                }


                try
                {
                    q = "EXECUTE master.dbo.xp_delete_file 0,N'I:\\" + dateold + "db.bak'";
                    objcore.executeQuery(q);
                    q = "backup database " + database + " to disk ='I:\\" + date + "db.bak'";
                    objcore.executeQuery(q);
                }
                catch (Exception ex)
                {

                }
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton9_Click_1(object sender, EventArgs e)
        {
             DialogResult dr = MessageBox.Show("Are You Sure to Refresh Database","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
             if (dr == DialogResult.Yes)
             {
                 backupdb();
                 string q = "delete from saledetails";
                 objcore.executeQuery(q);

                 q = "delete from billtype";
                 objcore.executeQuery(q);
                 q = "delete from TakeAway";
                 objcore.executeQuery(q);
                 q = "delete from DinInTables";
                 objcore.executeQuery(q);
                 q = "delete from Delivery";
                 objcore.executeQuery(q);

                 q = "delete from sale";
                 objcore.executeQuery(q);

                 q = "delete from dayend";
                 objcore.executeQuery(q);
                 q = "delete from shiftcash";
                 objcore.executeQuery(q);
                 q = "delete from ShiftStart";
                 objcore.executeQuery(q);
                 q = "delete from Saledetailsrefund";
                 objcore.executeQuery(q);
                 q = "delete from DiscountIndividual";
                 objcore.executeQuery(q);
                 q = "delete from PurchaseDetails";
                 objcore.executeQuery(q);
                 q = "delete from Purchase";
                 objcore.executeQuery(q);
                 q = "delete from Ordercomplete";
                 objcore.executeQuery(q);
                 q = "delete from PurchaseDetails";
                 objcore.executeQuery(q);
                 q = "delete from IssueStock";
                 objcore.executeQuery(q);
                 q = "delete from IssueStockDetails";
                 objcore.executeQuery(q);
                 q = "delete from InventoryTransfer";
                 objcore.executeQuery(q);
                 q = "delete from InventoryConsumed";
                 objcore.executeQuery(q);
                 q = "delete from Expenses";
                 objcore.executeQuery(q);
                 q = "delete from EmployeesSalary";
                 objcore.executeQuery(q);
                 q = "delete from Employees";
                 objcore.executeQuery(q);
                 q = "delete from Discard";
                 objcore.executeQuery(q);
                 q = "delete from Demand";
                 objcore.executeQuery(q);
                 q = "DBCC CHECKIDENT ('Demand', RESEED, 1)";
                 objcore.executeQuery(q);
                 q = "DBCC CHECKIDENT ('saledetails', RESEED, 1)";
                 objcore.executeQuery(q);
                 q = "DBCC CHECKIDENT ('billtype', RESEED, 1)";
                 objcore.executeQuery(q);
                 q = "DBCC CHECKIDENT ('sale', RESEED, 1)";
                 objcore.executeQuery(q);
                 q = "DBCC CHECKIDENT ('InventoryTransfer', RESEED, 1)";
                 objcore.executeQuery(q);
                 q = "DBCC CHECKIDENT ('InventoryConsumed', RESEED, 1)";
                 objcore.executeQuery(q);
                 q = "delete from BankAccountPaymentSupplier";
                 objcore.executeQuery(q);
                 q = "delete from BankAccountReceiptCustomer";
                 objcore.executeQuery(q);
                 q = "delete from CashAccountPaymentSupplier";
                 objcore.executeQuery(q);
                 q = "delete from CashAccountPurchase";
                 objcore.executeQuery(q);
                 q = "delete from CashAccountReceiptCustomer";
                 objcore.executeQuery(q);
                 q = "delete from CashAccountSales";
                 objcore.executeQuery(q);
                 q = "delete from CostSalesAccount";
                 objcore.executeQuery(q);
                 q = "delete from InventoryAccount";
                 objcore.executeQuery(q);
                 q = "delete from CustomerAccount";
                 objcore.executeQuery(q);
                 q = "delete from DiscountAccount";
                 objcore.executeQuery(q);
                 q = "delete from GSTAccount";
                 objcore.executeQuery(q);
                 q = "delete from JournalAccount";
                 objcore.executeQuery(q);
                 q = "delete from SalesAccount";
                 objcore.executeQuery(q);
                 q = "delete from SupplierAccount";
                 objcore.executeQuery(q);
                 q = "delete from EmployeesAccount";
                 objcore.executeQuery(q);
                 q = "delete from SalariesAccount";
                 objcore.executeQuery(q);
                 q = "delete from DSSaleDetails";
                 objcore.executeQuery(q);
                 q = "delete from DSBillType";
                 objcore.executeQuery(q);
                 q = "delete from DSSale";
                 objcore.executeQuery(q);
                 q = "delete from Attachmenu1";
                 objcore.executeQuery(q);
                 q = "delete from errors";
                 objcore.executeQuery(q);
                 q = "delete from closing";
                 objcore.executeQuery(q);
                 q = "delete from FBR_Table";
                 objcore.executeQuery(q);
                 q = "delete from Log";
                 objcore.executeQuery(q); 
                 q = "delete from People";
                 objcore.executeQuery(q);
                 q = "delete from Points";
                 objcore.executeQuery(q);
             }
        }
        public void accounts(string date)
        {
            double gross = 0, gst = 0, discount = 0, net = 0, cash = 0, credit = 0, master = 0, service = 0;
            string q = "";
            q = "delete from SalesAccount where VoucherNo='SJV-" + date + "'";
            objcore.executeQuery(q);
            q = "delete from DiscountAccount where VoucherNo='SJV-" + date + "'";
            objcore.executeQuery(q);

            q = "delete from GSTAccount where VoucherNo='SJV-" + date + "'";
            objcore.executeQuery(q);
            q = "delete from InventoryAccount where VoucherNo='SJV-" + date + "'";
            objcore.executeQuery(q);
            q = "delete from CashAccountSales where VoucherNo='SJV-" + date + "'";
            objcore.executeQuery(q);
            q = "delete from CostSalesAccount where VoucherNo='SJV-" + date + "'";
            objcore.executeQuery(q);
            DataSet ds = new DataSet();
            q = "select * from Branch ";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                branchid = ds.Tables[0].Rows[0]["id"].ToString();
            }
            inventoryaccount(date);
            q = "SELECT   SUM(TotalBill) AS total, SUM(TotalBill) AS netsale, SUM(GST) AS gst,SUM(DiscountAmount) AS discount,SUM(servicecharges) AS servicecharges FROM         Sale where  (Date = '" + date + "')  and billstatus='Paid'";
            ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    service = Convert.ToDouble(ds.Tables[0].Rows[0]["servicecharges"].ToString());
                }
                catch (Exception ex)
                {


                }
                gross = Convert.ToDouble(ds.Tables[0].Rows[0]["total"].ToString());
                gst = Convert.ToDouble(ds.Tables[0].Rows[0]["gst"].ToString());
                discount = Convert.ToDouble(ds.Tables[0].Rows[0]["discount"].ToString());
                net = Convert.ToDouble(ds.Tables[0].Rows[0]["netsale"].ToString());
                net = net - discount;
                //gross = gross + gst;
                saleaccount(gross.ToString(), "SJV-" + date, date);
                if (discount > 0)
                {
                    discountaccount(discount.ToString(), "SJV-" + date, date);
                }
                if (service > 0)
                {
                    serviceaccount(service.ToString(), "SJV-" + date, date);
                }
                gstaccount(gst.ToString(), "SJV-" + date, date);
            }
            q = "SELECT   SUM(TotalBill) AS total, SUM(TotalBill) AS netsale, SUM(GST) AS gst,SUM(DiscountAmount) AS discount FROM         Sale where  (Date = '" + date + "') and BillType='Cash'  and billstatus='Paid'";
            q = "SELECT        SUM(dbo.Sale.TotalBill) AS total, SUM(dbo.Sale.NetBill) AS netsale, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.DiscountAmount) AS discount FROM            dbo.Sale INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where  (dbo.Sale.Date = '" + date + "') and dbo.BillType.Type ='Cash'  and dbo.Sale.billstatus='Paid'";

            ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string temp = ds.Tables[0].Rows[0]["total"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                gross = Convert.ToDouble(temp);
                temp = ds.Tables[0].Rows[0]["gst"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                gst = Convert.ToDouble(temp);
                temp = ds.Tables[0].Rows[0]["discount"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                discount = Convert.ToDouble(temp);
                temp = ds.Tables[0].Rows[0]["netsale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                net = Convert.ToDouble(temp);
                //net = net - discount;
                //net = net + gst;
                if (net > 0)
                {
                    cashaccount(net.ToString(), "SJV-" + date, "Cash", date);
                }
            }
            q = "SELECT        SUM(dbo.Sale.TotalBill) AS total, SUM(dbo.Sale.NetBill) AS netsale, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.DiscountAmount) AS discount, dbo.BillType.recvid, dbo.BillType.saleid,dbo.Sale.Customer  FROM            dbo.Sale INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where  (dbo.Sale.Date = '" + date + "') and dbo.BillType.Type ='Receivable'  and dbo.Sale.billstatus='Paid' GROUP BY dbo.BillType.recvid, dbo.BillType.saleid,dbo.Sale.Customer";
            ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string temp = ds.Tables[0].Rows[i]["total"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                gross = Convert.ToDouble(temp);
                temp = ds.Tables[0].Rows[i]["gst"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                gst = Convert.ToDouble(temp);
                temp = ds.Tables[0].Rows[i]["discount"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                discount = Convert.ToDouble(temp);
                temp = ds.Tables[0].Rows[i]["netsale"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                net = Convert.ToDouble(temp);
                //net = net - discount;
                //net = net + gst;
                if (net > 0)
                {
                    q = "delete from CustomerAccount where VoucherNo='SJV-" + ds.Tables[0].Rows[i]["saleid"].ToString() + "-" + date + "'";
                    objcore.executeQuery(q);
                    q = "delete from CustomerAccount where VoucherNo='SJV-" + date + "-" + ds.Tables[0].Rows[i]["saleid"].ToString() + "'";
                    objcore.executeQuery(q);
                    employeeaccount(net.ToString(), "SJV-" + date + "-" + ds.Tables[0].Rows[i]["saleid"].ToString(), "Cash", date, ds.Tables[0].Rows[i]["recvid"].ToString(), ds.Tables[0].Rows[i]["Customer"].ToString(), ds.Tables[0].Rows[i]["saleid"].ToString());
                }
            }

            q = "SELECT   SUM(TotalBill) AS total, SUM(TotalBill) AS netsale, SUM(GST) AS gst,SUM(DiscountAmount) AS discount FROM         Sale where  (Date = '" + date + "') and BillType='Credit Card'  and billstatus='Paid'";
            q = "select * from banks";
            DataSet dsbanks = new DataSet();
            dsbanks = objcore.funGetDataSet(q);
            for (int i = 0; i < dsbanks.Tables[0].Rows.Count; i++)
            {
                q = "SELECT        SUM(dbo.Sale.TotalBill) AS total, SUM(dbo.Sale.NetBill) AS netsale, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.DiscountAmount) AS discount FROM            dbo.Sale INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where  (dbo.Sale.Date = '" + date + "') and dbo.BillType.Type like '%Visa%'  and dbo.Sale.billstatus='Paid'";
                q = "SELECT        SUM(dbo.Sale.TotalBill) AS total, SUM(dbo.Sale.NetBill) AS netsale, SUM(dbo.Sale.GST) AS gst, SUM(dbo.Sale.DiscountAmount) AS discount FROM            dbo.Sale INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid  where  (dbo.Sale.Date = '" + date + "') and dbo.BillType.Type like '%" + dsbanks.Tables[0].Rows[i]["Name"].ToString() + "%'  and dbo.Sale.billstatus='Paid'";
              
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0]["total"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    gross = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["gst"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    gst = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["discount"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    discount = Convert.ToDouble(temp);
                    temp = ds.Tables[0].Rows[0]["netsale"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    net = Convert.ToDouble(temp);
                    //net = net - discount;
                    //net = net + gst;
                    if (net > 0)
                    {
                        cashaccount(net.ToString(), "SJV-" + date, dsbanks.Tables[0].Rows[i]["Id"].ToString(), date);
                    }


                }


            }
        }
    
        private void vButton10_Click_1(object sender, EventArgs e)
        {
            string q = "select distinct branchid ,date from sale where date>='" + dateTimePicker1.Text + "'  order by date";
            DataSet ds = new System.Data.DataSet();
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                branchid = ds.Tables[0].Rows[i]["branchid"].ToString();
                accounts(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd"));
                //inventoryaccount(Convert.ToDateTime(ds.Tables[0].Rows[i]["date"].ToString()).ToString("yyyy-MM-dd"));
            }
        }
    }
}
