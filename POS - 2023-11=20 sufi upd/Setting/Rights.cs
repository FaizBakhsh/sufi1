using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class Rights : Form
    {
        public Rights()
        {
            InitializeComponent();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_2(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public void fillforms(string form)
        {
            try
            {
                DataSet dsufilforms = new System.Data.DataSet();
                string q = "select * from Forms where Forms='" + form.Replace("'","''") + "' ";
                //dsufilforms = objcore.funGetDataSet(q);
                dsufilforms = objcore.funGetDataSet(q);
               
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

                MessageBox.Show(ex.Message);
            }
        }
        public void callfillform()
        {

            fillforms("Reports .");
            fillforms("DS Group Wise Sales Report");
            fillforms("Points Codes");
            fillforms("Items Sale Summary");
            fillforms("Bill Transfer");
            fillforms("Delivery Status");
            fillforms("Table Design");
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
            fillforms("Production");
            fillforms("Issue Stock");
            fillforms("Employees Statement");
            fillforms("Store Issuance");
            fillforms("Dashboard");
            fillforms("Store Demand");
            fillforms("Issue Stock Approval");
            fillforms("Attandance");
            fillforms("Demand Approval");
            fillforms("Show MenuItem Price 3inch Report");
            

        }
        private void Rights_Load(object sender, EventArgs e)
        {
            try
            {
               string query = "ALTER TABLE [dbo].[Rights]  ADD InsertStatus varchar(500) NULL ";
               objcore.executeQuery(query);
                query = "ALTER TABLE [dbo].[Rights]  ADD UpdateStatus varchar(500) NULL ";
                objcore.executeQuery(query);
                query = "ALTER TABLE [dbo].[Rights]  ADD DeleteStaus varchar(500) NULL ";
                objcore.executeQuery(query);

            }
            catch (Exception ex)
            {


            }
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
                dataGridView1.Columns[4].Visible = false;
                userforms();
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                        DataGridViewCheckBoxCell chkinsert = (DataGridViewCheckBoxCell)dr.Cells[1];
                        DataGridViewCheckBoxCell chkedit = (DataGridViewCheckBoxCell)dr.Cells[2];
                        DataGridViewCheckBoxCell chkdelete = (DataGridViewCheckBoxCell)dr.Cells[3];

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
                            if (dsuserforms.Tables[0].Rows[0]["InsertStatus"].ToString().ToLower() == "yes")
                            {
                                chkinsert.Value = true;
                            }
                            else
                            {
                                chkinsert.Value = false;
                            }
                            if (dsuserforms.Tables[0].Rows[0]["UpdateStatus"].ToString().ToLower() == "yes")
                            {
                                chkedit.Value = true;
                            }
                            else
                            {
                                chkedit.Value = false;
                            }
                            if (dsuserforms.Tables[0].Rows[0]["DeleteStaus"].ToString().ToLower() == "yes")
                            {
                                chkdelete.Value = true;
                            }
                            else
                            {
                                chkdelete.Value = false;
                            }
                        }
                        else
                        {
                            chk.Value = false;
                            chkinsert.Value = false;
                            chkedit.Value = false;
                            chkdelete.Value = false;
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
                        string valinsert = this.dataGridView1.Rows[i].Cells[1].Value.ToString();
                        string valedit = this.dataGridView1.Rows[i].Cells[2].Value.ToString();
                        string valdelete = this.dataGridView1.Rows[i].Cells[3].Value.ToString();
                        if (dataGridView1.Rows[i].Cells[5].Value.ToString().ToLower() == "group")
                        {

                        }
                        if (valinsert.ToLower() == "true")
                        {
                            valinsert = "yes";
                        }
                        else
                        {
                            valinsert = "no";
                        }
                        if (valedit.ToLower() == "true")
                        {
                            valedit = "yes";
                        }
                        else
                        {
                            valedit = "no";
                        }
                        if (valdelete.ToLower() == "true")
                        {
                            valdelete = "yes";
                        }
                        else
                        {
                            valdelete = "no";
                        }

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
                                q = "update rights set status='" + val + "',InsertStatus='" + valinsert + "',UpdateStatus='" + valedit + "',DeleteStaus='" + valdelete + "' where id='" + dsuserforms.Tables[0].Rows[0]["id"] + "'";
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
                                q = "insert into rights(Id,formid,Userid, status,InsertStatus,UpdateStatus,DeleteStaus) values('" + idd + "','" + id + "','" + comboBox1.SelectedValue + "','" + val + "','" + valinsert + "','" + valedit + "','" + valdelete + "')";
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[0].Value = true;
                dr.Cells[1].Value = true;
                dr.Cells[2].Value = true;
                dr.Cells[3].Value = true;
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[0].Value = false;
                dr.Cells[1].Value = false;
                dr.Cells[2].Value = false;
                dr.Cells[3].Value = false;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            userforms();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string val = this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                if (val == "True")
                {
                    val = "true";
                }
                if (val == "False")
                {
                    val = "false";
                }
                bool b = Convert.ToBoolean(val);
                this.dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !b;
                
            }
            catch (Exception ex)
            {


            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[1].Value = true;

            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[1].Value = false;

            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[2].Value = true;

            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[2].Value = false;

            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[3].Value = true;

            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                dr.Cells[3].Value = false;

            }
        }
    }
}
