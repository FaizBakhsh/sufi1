using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.RawItems
{
    public partial class Purchase : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        
        public Purchase()
        {
            InitializeComponent();
            editmode = 0;
        }
        public void getdata(string id)
        {
            try
            {
                DataSet dss = new DataSet();
                string q = "SELECT     dbo.PurchaseDetails.RawItemId AS ItemId, dbo.RawItem.ItemName AS Item, dbo.PurchaseDetails.Package AS Item_Per_Package, dbo.PurchaseDetails.PackageItems AS Total_Packages,                       dbo.PurchaseDetails.TotalItems AS Total_Items, dbo.PurchaseDetails.Price AS Price_Per_Package, dbo.PurchaseDetails.TotalAmount AS Total_Amount, dbo.PurchaseDetails.id AS pdid FROM         dbo.RawItem INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id where dbo.PurchaseDetails.PurchaseId='" + id + "' and dbo.Purchase.Status !='Cancel'";
                dss = objCore.funGetDataSet(q);
                try
                {
                    dt.Rows.Clear();
                }
                catch (Exception ex)
                {
                    
                  
                }
               
                dt.Merge(dss.Tables[0], true, MissingSchemaAction.Ignore);
                double t = 0;
                foreach (DataRow dtr in dt.Rows)
                {
                    try
                    {
                        if (dtr["Price_Per_Package"].ToString() == "Total Amount:")
                        {
                            dtr.Delete();


                        }
                        else
                        {
                            t = t + Convert.ToDouble(dtr["Total_Amount"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }



                dt.Rows.Add("", "", "", "", "", "Total Amount:", t.ToString(),"","");
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["type"].Visible = false;
                dataGridView1.Columns["pdid"].Visible = false;
                dss = new DataSet();
                q = "select * from Purchase where id='" + id + "' and Status !='Cancel'";
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    cmbsupplier.SelectedValue = dss.Tables[0].Rows[0]["SupplierId"].ToString();
                    cmbbranch.SelectedValue = dss.Tables[0].Rows[0]["BranchCode"].ToString();
                    cmbstore.SelectedValue = dss.Tables[0].Rows[0]["StoreCode"].ToString();
                    textBox1.Text = dss.Tables[0].Rows[0]["Id"].ToString();
                    txtinvoice.Text = dss.Tables[0].Rows[0]["InvoiceNo"].ToString();

                }
                else
                {
                    try
                    {
                        cmbsupplier.Text = "Please Select";
                        cmbbranch.SelectedValue = "Please Select";
                        
                        textBox1.Text = "";
                        txtinvoice.Text = "";
                        cmbstore.SelectedValue = "Please Select";
                    }
                    catch (Exception w)
                    {
                        
                      
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void Purchase_Load(object sender, EventArgs e)
        {
            
            dt = new DataTable();
                
            dt.Columns.Add("ItemId", typeof(string));
            dt.Columns.Add("Item", typeof(string));
            dt.Columns.Add("Item_Per_Package", typeof(string));
            dt.Columns.Add("Total_Packages", typeof(string));
            dt.Columns.Add("Total_Items", typeof(string));
            dt.Columns.Add("Price_Per_Package", typeof(string));
            dt.Columns.Add("Total_Amount", typeof(string));
            dt.Columns.Add("type", typeof(string));
            dt.Columns.Add("pdid", typeof(string));
            string q = "select * from supplier";
            ds = objCore.funGetDataSet(q);
            DataRow dr = ds.Tables[0].NewRow();
            dr["Name"] = "Please Select";

            ds.Tables[0].Rows.Add(dr);
            cmbsupplier.DataSource = ds.Tables[0];
            cmbsupplier.ValueMember = "id";
            cmbsupplier.DisplayMember = "Name";
            cmbsupplier.Text = "Please Select";

            q = "select * from RawItem";
            ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            DataRow dr1 = ds.Tables[0].NewRow();
            dr1["ItemName"] = "Please Select";

            ds.Tables[0].Rows.Add(dr1);
            cmbitem.DataSource = ds.Tables[0];
            cmbitem.ValueMember = "id";
            cmbitem.DisplayMember = "ItemName";
            cmbitem.Text = "Please Select";

            q = "select * from Branch";
            ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            DataRow dr2 = ds.Tables[0].NewRow();
            dr2["BranchName"] = "Please Select";

            ds.Tables[0].Rows.Add(dr2);
            cmbbranch.DataSource = ds.Tables[0];
            cmbbranch.ValueMember = "id";
            cmbbranch.DisplayMember = "BranchName";
            cmbbranch.Text = "Please Select";
            fillstore();
            fillcashacount();
            cmbpurchasetype.Text = "Credit";
            cashaccount();
            
        }
        public void fillcashacount()
        {
            POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            try
            {

                string q = "select * from ChartofAccounts where AccountType='Current Assets'";
               DataSet dsa = objCore.funGetDataSet(q);
               DataRow dr = dsa.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                dsa.Tables[0].Rows.Add(dr);
                cmbcashaccount.DataSource = dsa.Tables[0];
                cmbcashaccount.ValueMember = "id";
                cmbcashaccount.DisplayMember = "Name";
                cmbcashaccount.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }
        }
        public void fillstore()
        {
            try
            {
                string q = "select * from Stores where branchid='" + cmbbranch.SelectedValue + "'";
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr3 = ds.Tables[0].NewRow();
                    dr3["StoreName"] = "Please Select";

                    ds.Tables[0].Rows.Add(dr3);
                    cmbstore.DataSource = ds.Tables[0];
                    cmbstore.ValueMember = "id";
                    cmbstore.DisplayMember = "StoreName";
                    cmbstore.Text = "Please Select";
                }
                if (cmbbranch.Text == "Please Select")
                {
                    cmbstore.DataSource = null;
                    if (cmbstore.Items.Contains("Please Select"))
                    { }
                    else
                    {
                        cmbstore.Items.Add("Please Select");
                    }
                    cmbstore.Text = "Please Select";
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }
        private void textBox3_TextChanged(object sender, EventArgs e)
        {
           
            if (txtpackage.Text == string.Empty)
            { }
            else
            {
                int Num;
                bool isNum = int.TryParse(txtpackage.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    return;
                }
                calculate();
            }
        }

        private void txtamount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtamount.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        return;
                    }
                }
                calculate();
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public void calculate()
        {
            try
            {
                txtquantity.Text = (Convert.ToInt32(txtpackage.Text) * Convert.ToInt32(txttotalpackages.Text)).ToString();
                txttotalamount.Text = (Convert.ToInt32(txtamount.Text) * Convert.ToInt32(txttotalpackages.Text)).ToString();
            }
            catch (Exception ex)
            {
                
               
            }
        }
        private void txttotalpackages_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txttotalpackages.Text == string.Empty)
                { }
                else
                {
                    int Num;
                    bool isNum = int.TryParse(txttotalpackages.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        return;
                    }
                }
                calculate();
                
            }
            catch (Exception ex)
            {
                
           
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {

            try
            {
                if (cmbbranch.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Branch");
                    cmbbranch.Focus();
                    return;
                }
                if (cmbstore.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Store");
                    cmbstore.Focus();
                    return;
                }
                if (cmbsupplier.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Supplier");
                    cmbsupplier.Focus();
                    return;
                }
                if (cmbitem.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Raw Item");
                    cmbitem.Focus();
                    return;
                }
                if (txtpackage.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Items per Packages");
                    txtpackage.Focus();
                    return;
                }
                if (txttotalpackages.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter No of Packages");
                    txttotalpackages.Focus();
                    return;
                }
                if (txtamount.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Amount per Package");
                    txtamount.Focus();
                    return;
                }
                if (txtpackage.Text == string.Empty)
                { }
                else
                {
                    int Num;
                    bool isNum = int.TryParse(txtpackage.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Items per Package value. Only Nymbers are allowed");
                        txtpackage.Focus();
                        return;
                    }
                }
                if (txttotalpackages.Text == string.Empty)
                { }
                else
                {
                    int Num;
                    bool isNum = int.TryParse(txttotalpackages.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid No of Packages Value. Only Nymbers are allowed");
                        txttotalpackages.Focus();
                        return;
                    }
                }
                if (txtamount.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtamount.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Price Per Package. Only Nymbers are allowed");
                        txtamount.Focus();
                        return;
                    }
                }


                //foreach (DataGridViewRow dr in dataGridView1.Rows)
                //{
                //    if (vButton1.Text == "Update")
                //    { }
                //    else
                //    {
                //        //if (dr.Cells[0].Value.ToString() == cmbitem.SelectedValue.ToString())
                //        //{
                //        //    MessageBox.Show("Item Already Exist");
                //        //    return;
                //        //}
                //    }
                //}
                double total = 0;
                try
                {
                    foreach (DataRow dr in dt.Rows)
                    {


                        if (vButton1.Text == "Update")
                        {
                            try
                            {
                                if (dr[0].ToString() == cmbitem.SelectedValue.ToString())
                                {


                                    dr.Delete();
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
                try
                {
                    foreach (DataRow dtr in dt.Rows)
                    {
                        try
                        {
                            if (dtr["Price_Per_Package"].ToString() == "Total Amount:")
                            {


                                dtr.Delete();
                            }
                            else
                            {
                                total = total + Convert.ToDouble(dtr["Total_Amount"].ToString());
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
                total = total + Convert.ToDouble(txttotalamount.Text);

                dt.Rows.Add(cmbitem.SelectedValue.ToString(), cmbitem.Text, txtpackage.Text, txttotalpackages.Text, txtquantity.Text, txtamount.Text, txttotalamount.Text,"new","");
                dt.Rows.Add("", "", "", "", "", "Total Amount:", total.ToString(),"","");

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                vButton1.Text = "Add to List";
                cmbitem.Enabled = true;
                txttotalamount.Text = string.Empty;
                txtquantity.Text = string.Empty;
                txtpackage.Text = string.Empty;
                txtamount.Text = string.Empty;
                txttotalpackages.Text = string.Empty;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public void inventry(string id, double qnty,string type)
        {
            try
            {
                int iddd = 0;
                ds = objCore.funGetDataSet("select max(id) as id from Inventory");
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
                ds = new DataSet();
                string q = "select * from Inventory where RawItemId='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    double records = Convert.ToDouble(ds.Tables[0].Rows[0]["Quantity"].ToString());
                    records = records + qnty;
                    q = "update Inventory set Quantity='" + records + "' where RawItemId='" + id + "'";
                    objCore.executeQuery(q);
                }
                else
                {
                    q = "insert into Inventory (id,RawItemId,Quantity) values('" + iddd + "','" + id + "','" + qnty + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception exx)
            {

                MessageBox.Show("Inventory Error");
            }
            
        }
        public void inventoryaccount(string itmid, string amount, string type)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select Inventoryid from rawitem where id='" + itmid + "' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string invntryid = dsacount.Tables[0].Rows[0][0].ToString();
                    int iddd = 0;
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
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from InventoryAccount where ChartAccountId='" + invntryid + "' order by id desc";
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



                    q = "insert into InventoryAccount (Id,Date,ItemId,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + itmid + "','" + invntryid + "','GRN-" + textBox1.Text + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public void supplieraccount( string amount, string type)
        {
            try
            {

                DataSet dsacount = new DataSet();
                string q = "select payableaccountid from Supplier where id='" + cmbsupplier.SelectedValue + "' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
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
                    string val = "";
                    q = "select top 1 * from SupplierAccount where SupplierId='" + cmbsupplier.SelectedValue + "' and PayableAccountId='"+PayableAccountId+"' order by id desc";
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



                    q = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + cmbsupplier.SelectedValue + "','" + PayableAccountId + "','GRN-" + textBox1.Text + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public void cashaccount(string amount, string type)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select payableaccountid from Supplier where id='" + cmbsupplier.SelectedValue + "' ";
                // dsacount = objCore.funGetDataSet(q);
                // if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = cmbcashaccount.SelectedValue.ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CashAccountPurchase");
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
                    string val = "";
                    q = "select top 1 * from CashAccountPurchase where ChartAccountId='" + cmbcashaccount.SelectedValue + "' order by id desc";
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


                    q = "insert into CashAccountPurchase (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + cmbcashaccount.SelectedValue + "','GRN-" + textBox1.Text + "','" + txtdesc.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
               
            }
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            
        }
        public void savedata( string type)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {

                    if (textBox1.Text == string.Empty)
                    {
                        int id = 0;
                        string q = "";
                        if (type == "purchase")
                        {
                            q = "select max(id) as id from Purchase";
                        }
                        if (type == "return")
                        {
                            q = "select max(id) as id from Purchasereturn";
                        }
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string i = ds.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            id = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            id = 1;
                        }
                        ds = new DataSet();
                        double tamount = Convert.ToDouble(dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Cells["Total_Amount"].Value.ToString());
                        if (type == "purchase")
                        {
                            q = "insert into Purchase (id,SupplierId,TotalAmount,Date,BranchCode,StoreCode,InvoiceNo) values('" + id + "','" + cmbsupplier.SelectedValue + "','" + tamount + "','" + dateTimePicker1.Text.Trim() + "','" + cmbbranch.SelectedValue + "','" + cmbstore.SelectedValue + "','" + txtinvoice.Text.Trim().Replace("'", "''") + "')";
                        }
                        if (type == "return")
                        {
                            q = "insert into Purchasereturn (id,SupplierId,TotalAmount,Date,BranchCode,StoreCode,InvoiceNo) values('" + id + "','" + cmbsupplier.SelectedValue + "','" + tamount + "','" + dateTimePicker1.Text.Trim() + "','" + cmbbranch.SelectedValue + "','" + cmbstore.SelectedValue + "','" + txtinvoice.Text.Trim().Replace("'", "''") + "')";
                        }
                        objCore.executeQuery(q);
                        textBox1.Text =id.ToString();
                        foreach (DataGridViewRow dgr in dataGridView1.Rows)
                        {
                            if (dgr.Cells["Price_Per_Package"].Value.ToString() == "Total Amount:")
                            {
                            }
                            else
                            {
                                string rawid = dgr.Cells[0].Value.ToString();
                                string pkg = dgr.Cells["Item_Per_Package"].Value.ToString();
                                string pkgitems = dgr.Cells["Total_Packages"].Value.ToString();
                                string totalitems = dgr.Cells["Total_Items"].Value.ToString();
                                string price = dgr.Cells["Price_Per_Package"].Value.ToString();
                                string totalamount = dgr.Cells["Total_Amount"].Value.ToString();

                                int idd = 0;
                                ds = new DataSet();
                                if (type == "purchase")
                                {
                                    q = "select max(id) as id from PurchaseDetails";
                                }
                                if (type == "return")
                                {
                                    q = "select max(id) as id from PurchasereturnDetails";
                                }
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string i = ds.Tables[0].Rows[0][0].ToString();
                                    if (i == string.Empty)
                                    {
                                        i = "0";
                                    }
                                    idd = Convert.ToInt32(i) + 1;
                                }
                                else
                                {
                                    idd = 1;
                                }
                                double pitem = 0;
                                try
                                {
                                    pitem = (Convert.ToDouble(totalamount) / Convert.ToDouble(totalitems));
                                }
                                catch (Exception ex)
                                {

                                    MessageBox.Show(ex.Message);
                                }
                                if (type == "purchase")
                                {
                                    q = "insert into PurchaseDetails (id,RawItemId,PurchaseId,Package,PackageItems,TotalItems,Price,PricePerItem,TotalAmount) values('" + idd + "','" + rawid + "','" + id + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + pitem + "','" + totalamount + "')";
                                }
                                if (type == "return")
                                {
                                    q = "insert into PurchasereturnDetails (id,RawItemId,PurchasereturnId,Package,PackageItems,TotalItems,Price,PricePerItem,TotalAmount) values('" + idd + "','" + rawid + "','" + id + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + pitem + "','" + totalamount + "')";
                                }
                                objCore.executeQuery(q);
                                inventry(rawid, Convert.ToInt32(totalitems),type);
                                inventoryaccount(rawid, totalamount, type);
                                if (cmbpurchasetype.Text == "Credit")
                                {
                                    supplieraccount(totalamount, type);
                                }
                                if (cmbpurchasetype.Text == "Cash")
                                {
                                    cashaccount(totalamount, type);
                                }
                                
                                //dt.Clear();
                                //textBox1.Text = "";
                            }
                            
                        }
                        getdata(textBox1.Text);
                        MessageBox.Show("Record Saved Successfully");
                    }
                    else
                    {
                        foreach (DataGridViewRow dgr in dataGridView1.Rows)
                        {
                            if (dgr.Cells["Price_Per_Package"].Value.ToString() == "Total Amount:")
                            {
                            }
                            else
                            {
                                string rawid = dgr.Cells[0].Value.ToString();
                                string pkg = dgr.Cells["Item_Per_Package"].Value.ToString();
                                string pkgitems = dgr.Cells["Total_Packages"].Value.ToString();
                                string totalitems = dgr.Cells["Total_Items"].Value.ToString();
                                string price = dgr.Cells["Price_Per_Package"].Value.ToString();
                                string totalamount = dgr.Cells["Total_Amount"].Value.ToString();
                                string q = "";
                                if (dgr.Cells["type"].Value.ToString() == "")
                                {
                                    if (type == "purchase")
                                    {
                                        q = "select * from PurchaseDetails where RawItemId= '" + rawid + "' and PurchaseId='" + textBox1.Text + "'";
                                    }
                                    if (type == "return")
                                    {
                                        q = "select * from PurchasereturnDetails where RawItemId= '" + rawid + "' and PurchasereturnId='" + textBox1.Text + "'";
                                    }
                                    DataSet dspdetails = new DataSet();
                                    dspdetails = objCore.funGetDataSet(q);

                                    double ptotal = Convert.ToDouble(totalitems) - Convert.ToDouble(dspdetails.Tables[0].Rows[0]["TotalItems"].ToString());
                                    if (type == "purchase")
                                    {
                                        q = "update PurchaseDetails set Package='" + pkg + "',PackageItems='" + pkgitems + "',TotalItems='" + totalitems + "',Price='" + price + "',TotalAmount='" + totalamount + "' where  RawItemId= '" + rawid + "' and PurchaseId='" + textBox1.Text + "')";
                                    }
                                    if (type == "return")
                                    {
                                        q = "update PurchasereturnDetails set Package='" + pkg + "',PackageItems='" + pkgitems + "',TotalItems='" + totalitems + "',Price='" + price + "',TotalAmount='" + totalamount + "' where  RawItemId= '" + rawid + "' and PurchasereturnId='" + textBox1.Text + "')";
                                    }
                                    objCore.executeQuery(q);
                                    inventry(rawid, ptotal, type);
                                }
                                if (dgr.Cells["type"].Value.ToString() == "new")
                                {
                                    int idd = 0;
                                    ds = new DataSet();
                                    if (type == "purchase")
                                    {
                                        q = "select max(id) as id from PurchaseDetails";
                                    }
                                    if (type == "return")
                                    {
                                        q = "select max(id) as id from PurchasereturnDetails";
                                    }
                                    ds = objCore.funGetDataSet(q);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string i = ds.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idd = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idd = 1;
                                    }
                                    double pitem = 0;
                                    try
                                    {
                                        pitem = (Convert.ToDouble(totalamount) / Convert.ToDouble(totalitems));
                                    }
                                    catch (Exception ex)
                                    {

                                        MessageBox.Show(ex.Message);
                                    }
                                    if (type == "purchase")
                                    {
                                        q = "insert into PurchaseDetails (id,RawItemId,PurchaseId,Package,PackageItems,TotalItems,Price,PricePerItem,TotalAmount) values('" + idd + "','" + rawid + "','" + textBox1.Text + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + pitem + "','" + totalamount + "')";
                                    }
                                    if (type == "return")
                                    {
                                        q = "insert into PurchasereturnDetails (id,RawItemId,PurchasereturnId,Package,PackageItems,TotalItems,Price,PricePerItem,TotalAmount) values('" + idd + "','" + rawid + "','" + textBox1.Text + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + pitem + "','" + totalamount + "')";
                                    }
                                    objCore.executeQuery(q);
                                    inventry(rawid, Convert.ToInt32(totalitems), type);
                                    inventoryaccount(rawid, totalamount, type);
                                    if (cmbpurchasetype.Text == "Credit")
                                    {
                                        supplieraccount(totalamount, type);
                                    }
                                    if (cmbpurchasetype.Text == "Cash")
                                    {
                                        cashaccount(totalamount, type);
                                    }
                                }
                            }
                        }
                        MessageBox.Show("Record Updated Successfully");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillstore();
        }

        private void vButton2_Click_1(object sender, EventArgs e)
        {
            if (cmbcashaccount.Visible==true)
            {
                if (cmbcashaccount.Text == "" || cmbcashaccount.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Cash Account");
                } 
            }
            savedata("purchase");
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string type = dt.Rows[e.RowIndex]["type"].ToString();
                if (type == "")
                {
                    string id = dt.Rows[e.RowIndex]["pdid"].ToString();
                    string q = "delete from PurchaseDetails where  Id= '" + id + "' ";
                    objCore.executeQuery(q);
                    getdata(textBox1.Text);

                }
                //DataRow dtr = dt.Rows[e.RowIndex];

                //dtr.Delete();

                //double total = 0;

                //foreach (DataGridViewRow dr in dataGridView1.Rows)
                //{
                //    if (dr.Cells["Price_Per_Package"].Value.ToString() == "Total Amount:")
                //    {
                //        DataRow dtrr = dt.Rows[dr.Index];

                //        dtrr.Delete();
                //    }
                //    else
                //    {
                //        total = total + Convert.ToDouble(dr.Cells["Total_Amount"].Value.ToString());
                //    }
                //}
                //dataGridView1.Refresh();
                //if (dataGridView1.Rows.Count > 0)
                //{
                //    dt.Rows.Add("", "", "", "", "", "Total Amount:", total.ToString());
                //}
                //dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                
              
            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            bindreport("GRN");
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            dscompany = objCore.funGetDataSet("select * from CompanyInfo");

        }
        public void bindreport(string type)
        {
            try
            {

                if (textBox1.Text != string.Empty)
                {

                    //ReportDocument rptDoc = new ReportDocument();
                    
                    POSRetail.Reports.Inventory.rprInventoryPurchase rptDoc = new Reports.Inventory.rprInventoryPurchase();
                    POSRetail.Reports.Inventory.rprInventoryInvoice rptDoc1 = new Reports.Inventory.rprInventoryInvoice();
                    POSRetail.Reports.Inventory.DsInventoryReceived dsrpt = new Reports.Inventory.DsInventoryReceived();
                    // .xsd file name
                    DataTable dt = new DataTable();

                    // Just set the name of data table
                    dt.TableName = "Crystal Report";
                    dt = getAllOrders();
                    //if (dt.Rows.Count > 0)
                    {

                        getcompany();
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
                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        //dt = getAllOrders();
                        if (dt.Rows.Count > 0)
                        {
                            dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                        }
                        else
                        {
                            if (logo == "")
                            { }
                            else
                            {

                                dt.Rows.Add("", DateTime.Now, "", "", "", "0", "0", "0", "0", "0", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                                dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                            }
                        }
                        
                        if (type == "GRN")
                        {
                            rptDoc.SetDataSource(dsrpt);
                            rptDoc.SetParameterValue("Comp", company);
                            rptDoc.SetParameterValue("Addrs", phone);
                            rptDoc.SetParameterValue("phn", address);
                            //rptDoc.PrintOptions.PrinterName = "HP LaserJet Professional M1212nf MFP (Copy 3)";
                            rptDoc.PrintToPrinter(1, false, 0, 0);
                        }
                        if (type == "invoice")
                        {
                            rptDoc1.SetDataSource(dsrpt);
                            rptDoc1.SetParameterValue("Comp", company);
                            rptDoc1.SetParameterValue("Addrs", phone);
                            rptDoc1.SetParameterValue("phn", address);
                            rptDoc1.PrintToPrinter(1, false, 0, 0);
                        }

                        //rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["Name"].ToString();
                        
                    }

                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            DataSet dsinfo = new DataSet();
            try
            {
                dtrpt.Columns.Add("Supplier", typeof(string));
                dtrpt.Columns.Add("Date", typeof(DateTime));
                dtrpt.Columns.Add("InvoiceNo", typeof(string));
                dtrpt.Columns.Add("serialNo", typeof(string));
                dtrpt.Columns.Add("Package", typeof(string));

                dtrpt.Columns.Add("PackageItems", typeof(Int32));
                dtrpt.Columns.Add("TotalItems", typeof(double));
                dtrpt.Columns.Add("Price", typeof(double));

                dtrpt.Columns.Add("TotalAmount ", typeof(double));
                dtrpt.Columns.Add("PurchaseTotalAmount", typeof(double));
                dtrpt.Columns.Add("ItemName", typeof(string));
                dtrpt.Columns.Add("UOM", typeof(string));
                dtrpt.Columns.Add("pdate", typeof(string));
                dtrpt.Columns.Add("logo", typeof(byte[]));

                string q = "SELECT     dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems,                       dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.PurchaseDetails.TotalAmount, SUM(dbo.Purchase.TotalAmount) AS PurchaseTotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.Purchase.id='" + textBox1.Text + "' GROUP BY dbo.Supplier.Name, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id, dbo.PurchaseDetails.Package, dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems,                       dbo.PurchaseDetails.Price, dbo.Purchase.TotalAmount, dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.PurchaseDetails.TotalAmount";
               
                dsinfo = objCore.funGetDataSet(q);//"SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package,                       dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.Purchase.id='" + textBox1.Text + "'");

                dtrpt = dsinfo.Tables[0];
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            return dtrpt;// dsinfo.Tables[0];
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;

                if (indx >= 0)
                {
                    string rawid = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    string pkg = dataGridView1.Rows[indx].Cells["Item_Per_Package"].Value.ToString();
                    string pkgitems = dataGridView1.Rows[indx].Cells["Total_Packages"].Value.ToString();
                    string totalitems = dataGridView1.Rows[indx].Cells["Total_Items"].Value.ToString();
                    string price = dataGridView1.Rows[indx].Cells["Price_Per_Package"].Value.ToString();
                    string totalamount = dataGridView1.Rows[indx].Cells["Total_Amount"].Value.ToString();
                    cmbitem.SelectedValue = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    txtpackage.Text = rawid;
                    txtpackage.Text = pkg;
                    txttotalpackages.Text = pkgitems;
                    txtquantity.Text = totalitems;
                    txtamount.Text = price;
                    txttotalamount.Text = totalamount;
                    vButton1.Text = "Update";
                    cmbitem.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
            bindreport("invoice");
        }

        private void vButton4_Click_1(object sender, EventArgs e)
        {
            PurChase_List obj = new PurChase_List(this);
            obj.Show();
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != string.Empty)
            {
                POSRetail.Reports.Inventory.frmInventoryPreview obj = new Reports.Inventory.frmInventoryPreview();
                obj.id = textBox1.Text;
                obj.Show();
            }
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
            { }
            else
            {
                DialogResult rs = MessageBox.Show("Are You Sure to Cancel?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (rs == DialogResult.Yes)
                {
                    objCore.executeQuery("update purchase set status='Cancel' where id='"+textBox1.Text+"'");
                    MessageBox.Show("Cancelled Successfully");
                    textBox1.Text = "";
                    getdata(textBox1.Text);
                    
                }
            }
        }
        public void clear()
        {
            try
            {
                txtamount.Text = "";
                txtdesc.Text = "";
                txtinvoice.Text = "";
                txtpackage.Text = "";
                txtquantity.Text = "";
                txttotalamount.Text = "";
                txttotalpackages.Text = "";
                cmbbranch.Text = "";
                cmbsupplier.Text = "";
                cmbbranch.Text = "";
                cmbstore.Text = "";
                textBox1.Text = "";
                dt.Clear();
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void vButton9_Click(object sender, EventArgs e)
        {
            clear();
        }
        public void cashaccount()
        {
            if (cmbpurchasetype.Text == "Cash")
            {
                lblcashaccount.Visible = true;
                cmbcashaccount.Visible = true;
            }
            else
            {
                lblcashaccount.Visible = false;
                cmbcashaccount.Visible = false;
            }
        }
        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            cashaccount();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            if (cmbcashaccount.Visible == true)
            {
                if (cmbcashaccount.Text == "" || cmbcashaccount.Text == "Please Select")
                {
                    MessageBox.Show("Please Select Cash Account");
                }
            }
            savedata("return");
        }

        private void vButton10_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
