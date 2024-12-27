using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class Purchase : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        
        public Purchase()
        {
            InitializeComponent();
            editmode = 0;
        }
        string purchasetype = "";
        public void getdata(string id)
        {
            try
            {
                DataSet dss = new DataSet();
                string q = "SELECT     dbo.PurchaseDetails.RawItemId AS ItemId, dbo.RawItem.ItemName AS Item, dbo.PurchaseDetails.Package AS Item_Per_Package, dbo.PurchaseDetails.PackageItems AS Total_Packages,                       dbo.PurchaseDetails.TotalItems AS Total_Items, dbo.PurchaseDetails.Price AS Price_Per_Package, dbo.PurchaseDetails.TotalAmount AS Total_Amount, dbo.PurchaseDetails.Description, dbo.PurchaseDetails.Expiry FROM         dbo.RawItem INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId INNER JOIN                      dbo.Purchase ON dbo.PurchaseDetails.PurchaseId = dbo.Purchase.Id where dbo.PurchaseDetails.PurchaseId='" + id + "' and dbo.Purchase.Status !='Cancel'";
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
                

               // dt.Rows.Add("", "", "", "", "", "Total Amount:", t.ToString());
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                purchasetype = "purchase";
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
                    dateTimePicker1.Text = dss.Tables[0].Rows[0]["date"].ToString();
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
        public void getdemanddata(string date,string brid,string sid)
        {
            try
            {
                DataSet dss = new DataSet();
                string q = "SELECT        dbo.Demand.id, dbo.Demand.Itemid, dbo.Demand.Date, dbo.Demand.Quantity, dbo.Demand.Status, dbo.Demand.branchid, dbo.RawItem.ItemName, dbo.RawItem.Price, ROUND(dbo.Demand.Quantity * dbo.RawItem.Price, 2)                          AS Total FROM            dbo.Demand INNER JOIN                         dbo.RawItem ON dbo.Demand.Itemid = dbo.RawItem.Id where dbo.Demand.date='" + date + "' and dbo.Demand.branchid= '" + brid + "'  and dbo.Demand.supplierid= '" + sid + "' and dbo.Demand.status='pending'";
                dss = objCore.funGetDataSet(q);
                try
                {
                    dt.Rows.Clear();
                }
                catch (Exception ex)
                {


                }
                for (int i = 0; i < dss.Tables[0].Rows.Count; i++)
                {                    
                    dt.Rows.Add(dss.Tables[0].Rows[i]["Itemid"].ToString(), dss.Tables[0].Rows[i]["ItemName"].ToString(), "1", dss.Tables[0].Rows[i]["Quantity"].ToString(), dss.Tables[0].Rows[i]["Quantity"].ToString(), dss.Tables[0].Rows[i]["Price"].ToString(), dss.Tables[0].Rows[i]["Total"].ToString(), "");
                    dateTimePicker1.Text = date;
                    cmbbranch.SelectedValue = brid;
                    
                }
                
                double t = 0;
                demanddate = date;
                demandbranch = brid;
                cmbsupplier.SelectedValue = sid;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                purchasetype = "demand";
                
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
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Expiry", typeof(string));
           
            string q = "select * from supplier";
            ds = objCore.funGetDataSet(q);
            DataRow dr = ds.Tables[0].NewRow();
            dr["Name"] = "Please Select";

            ds.Tables[0].Rows.Add(dr);
            cmbsupplier.DataSource = ds.Tables[0];
            cmbsupplier.ValueMember = "id";
            cmbsupplier.DisplayMember = "Name";
            cmbsupplier.Text = "Please Select";

            

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
            fillgroup();
            fillitems();
        }
        public void fillgroup()
        {
            try
            {
                string q = "select * from Groups";
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr2 = ds.Tables[0].NewRow();
                    dr2["GroupName"] = "All";
                    
                    ds.Tables[0].Rows.Add(dr2);
                    cmbgroup.DataSource = ds.Tables[0];
                    cmbgroup.ValueMember = "id";
                    cmbgroup.DisplayMember = "GroupName";
                    
                }
                
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }
        public void fillitems()
        {
            try
            {
                string q = "";
                if (cmbgroup.Text == "All")

                {
                    q = "select * from RawItem";
                }
                else
                {
                    q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.UOM.UOM FROM  dbo.RawItem INNER JOIN              dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where GroupId='" + cmbgroup.SelectedValue + "' order by dbo.RawItem.ItemName";
                }
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);

                DataTable dtitem = new DataTable();
                dtitem.Columns.Add("Id", typeof(string));
                dtitem.Columns.Add("ItemName", typeof(string));
                dtitem.Rows.Add("0", "Please Select");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    dtitem.Rows.Add(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["ItemName"].ToString() + " (" + ds.Tables[0].Rows[i]["UOM"].ToString() + ")");
                }

                cmbitem.DataSource = dtitem;
                cmbitem.ValueMember = "id";
                cmbitem.DisplayMember = "ItemName";
                cmbitem.Text = "Please Select";

            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
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
                float Num;
                bool isNum = float.TryParse(txtpackage.Text.ToString(), out Num); //c is your variable
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
                txtquantity.Text = (float.Parse(txtpackage.Text) * float.Parse(txttotalpackages.Text)).ToString();
                txttotalamount.Text = (float.Parse(txtamount.Text) * float.Parse(txttotalpackages.Text)).ToString();
                lblpriceperitem.Text =Math.Round( (float.Parse(txttotalamount.Text) / float.Parse(txtquantity.Text)),3).ToString();
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
                    float Num;
                    bool isNum = float.TryParse(txttotalpackages.Text.ToString(), out Num); //c is your variable
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
                    float Num;
                    bool isNum = float.TryParse(txtpackage.Text.ToString(), out Num); //c is your variable
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
                    float Num;
                    bool isNum = float.TryParse(txttotalpackages.Text.ToString(), out Num); //c is your variable
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


                        //if (vButton1.Text == "Update")
                        //{
                        //    try
                        //    {
                        //        if (dr[0].ToString() == cmbitem.SelectedValue.ToString())
                        //        {


                        //            dr.Delete();
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {
                                
                                
                        //    }
                        //}
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
                lblamount.Text = total.ToString("#,##0");


                dt.Rows.Add(cmbitem.SelectedValue.ToString(), cmbitem.Text, txtpackage.Text, txttotalpackages.Text, txtquantity.Text, txtamount.Text, txttotalamount.Text,txtdesc.Text.Trim().Replace("'",""),dateTimePicker2.Text);
                //dt.Rows.Add("", "", "", "", "", "Total Amount:", total.ToString());

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                
                vButton1.Text = "Add to List";
                cmbitem.Enabled = true;
                txttotalamount.Text = string.Empty;
                txtquantity.Text = string.Empty;
                txtpackage.Text = string.Empty;
                //txtamount.Text = string.Empty;
                txtdesc.Text = "";
                txttotalpackages.Text = string.Empty;
                
                lblpriceperitem.Text = "";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public void updateremaininginventory(string itemid, double quantity)
        {
            DataSet dsitems = new DataSet();
            string q = "select * from InventoryConsumed where RawItemId='" + itemid + "' and date='" + dateTimePicker1.Text + "'";
            dsitems = objCore.funGetDataSet(q);
            if (dsitems.Tables[0].Rows.Count > 0)
            {
                string val = dsitems.Tables[0].Rows[0]["QuantityConsumed"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                double consumed = Convert.ToDouble(val);
                val = dsitems.Tables[0].Rows[0]["RemainingQuantity"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                double remaining = Convert.ToDouble(val);               
                remaining = remaining + (quantity);
                q = "update InventoryConsumed set QuantityConsumed='" + consumed + "',RemainingQuantity='" + remaining + "' where id='" + dsitems.Tables[0].Rows[0]["id"].ToString() + "'";
                objCore.executeQuery(q);
            }
            else
            {
                DataSet dss = new DataSet();
                int id = 0;
                dss = objCore.funGetDataSet("select max(id) as id from InventoryConsumed");
                if (dss.Tables[0].Rows.Count > 0)
                {
                    string i = dss.Tables[0].Rows[0][0].ToString();
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
                dsitems = new DataSet();
                q = "select * from InventoryConsumed where RawItemId='" + itemid + "' order by id desc";
                dsitems = objCore.funGetDataSet(q);
                if (dsitems.Tables[0].Rows.Count > 0)
                {
                    string val = dsitems.Tables[0].Rows[0]["QuantityConsumed"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double consumed = Convert.ToDouble(val);
                    val = dsitems.Tables[0].Rows[0]["RemainingQuantity"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double remaining = Convert.ToDouble(val);

                    consumed = consumed + (quantity);
                    remaining = remaining + (quantity);
                    //q = "update InventoryConsumed set QuantityConsumed='" + consumed + "',RemainingQuantity='" + remaining + "' where id='" + dsitems.Tables[0].Rows[0]["id"].ToString() + "'";
                    //objcore.executeQuery(q);
                    string q1 = "insert into InventoryConsumed  (id,RawItemId,QuantityConsumed,RemainingQuantity,date) values('" + id + "','" + itemid + "','0','" + remaining + "','" + dateTimePicker1.Text + "')";
                    objCore.executeQuery(q1);
                }
                else
                {

                    string q1 = "insert into InventoryConsumed  (id,RawItemId,QuantityConsumed,RemainingQuantity,date) values('" + id + "','" + itemid + "','0','" + (quantity) + "','" + dateTimePicker1.Text + "')";
                    objCore.executeQuery(q1);
                }
            }
        }
        public void inventry(string id, double qnty)
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
                double records =Convert.ToDouble( ds.Tables[0].Rows[0]["Quantity"].ToString());
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
        public void inventryreverse(string id, double qnty)
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
                records = records - qnty;
                q = "update Inventory set Quantity='" + records + "' where RawItemId='" + id + "'";
                objCore.executeQuery(q);
            }
            ds = new DataSet();
            q = "select * from InventoryConsumed where RawItemId='" + id + "' order by id desc";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                double records = Convert.ToDouble(ds.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                records = records - qnty;
                string q1 = "insert into InventoryConsumed  (id,RawItemId,QuantityConsumed,RemainingQuantity,date) values('" + id + "','" + id + "','0','" + (records) + "','" + dateTimePicker1.Text + "')";
                objCore.executeQuery(q1);                
            }
            
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            
        }
        
        public void savedata()
        {
            if (cmbbranch.Text == "Please Select" || cmbbranch.Text == "")
            {
                MessageBox.Show("Please Select Branch");
                cmbbranch.Focus();
                return;
            }
            if (cmbstore.Text == "Please Select" || cmbstore.Text == "")
            {
                MessageBox.Show("Please Select Store");
                cmbstore.Focus();
                return;
            }
            if (cmbsupplier.Text == "Please Select" || cmbsupplier.Text == "")
            {
                MessageBox.Show("Please Select Supplier");
                cmbsupplier.Focus();
                return;
            }
            if (cmbpurchasetype.Text == "Please Select" || cmbpurchasetype.Text == "")
            {
                MessageBox.Show("Please Select Purchase Type");
                cmbpurchasetype.Focus();
                return;
            }
             SqlConnection connection;
             connection = new SqlConnection();
             connection.ConnectionString =objCore.getConnectionString();
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {

                    if (textBox1.Text == string.Empty)
                    {
                        ds = objCore.funGetDataSet("select id as id from Purchase where invoiceno='"+txtinvoice.Text+"'");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            MessageBox.Show("Invoice No already Exist");
                            return;
                        }
                        ds = new DataSet();
                        int id = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from Purchase");
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
                        double tamount = 0;// Convert.ToDouble(dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Cells["Total_Amount"].Value.ToString());
                        foreach (DataGridViewRow drr in dataGridView1.Rows)
                        {
                            string totalamount = drr.Cells["Total_Amount"].Value.ToString();
                            if (totalamount == "")
                            {
                                totalamount = "0";
                            }
                            tamount = tamount + Convert.ToDouble(totalamount);
                        }
                        string q = "insert into Purchase (PONo,id,SupplierId,TotalAmount,Date,BranchCode,StoreCode,InvoiceNo) values('" + txtpo.Text + "','" + id + "','" + cmbsupplier.SelectedValue + "','" + tamount + "','" + dateTimePicker1.Text.Trim() + "','" + cmbbranch.SelectedValue + "','" + cmbstore.SelectedValue + "','" + txtinvoice.Text.Trim().Replace("'", "''") + "')";
                        
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                            connection.Open();
                            SqlCommand com = new SqlCommand(q, connection);
                            int res = com.ExecuteNonQuery();
                            if (res == 0)
                            {
                                MessageBox.Show("Can not Save Data");
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }
                        objCore.executeQuery(q);
                        textBox1.Text = id.ToString();
                        pid = id.ToString();
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
                                string desc = dgr.Cells["Description"].Value.ToString();
                                double pricepitem = 0, totalp = 0, items = 0;
                                string expiry = dgr.Cells["Expiry"].Value.ToString();
                                if (totalamount == "")
                                {

                                }
                                else
                                {
                                    totalp = Convert.ToDouble(totalamount);
                                }
                                if (totalitems == "")
                                {

                                }
                                else
                                {
                                    items = Convert.ToDouble(totalitems);
                                }
                                try
                                {
                                    pricepitem = totalp / items;
                                }
                                catch (Exception ex)
                                {


                                }
                                int idd = 0;
                                ds = new DataSet();
                                ds = objCore.funGetDataSet("select max(id) as id from PurchaseDetails");
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
                                try
                                {
                                    q = "insert into PurchaseDetails (expiry,id,RawItemId,PurchaseId,Package,PackageItems,TotalItems,Price,TotalAmount,description,PricePerItem) values('" + expiry + "','" + idd + "','" + rawid + "','" + id + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + totalamount + "','" + desc + "','" + pricepitem + "')";
                                    string cs = POSRestaurant.Properties.Settings.Default.ConnectionString;
                                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(cs);
                                    string s = "";
                                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(q, con);
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    inventoryaccount(rawid, totalamount);
                                   
                                }
                                catch (Exception exx)
                                {
                                    q = "delete from purchase where id='" + id + "'";
                                    objCore.executeQuery(q);
                                    MessageBox.Show(exx.Message + "-" + q);

                                }
                                // objCore.executeQuery(q);
                                //inventry(rawid, Convert.ToInt32(totalitems));
                                //updateremaininginventory(rawid, Convert.ToInt32(totalitems));
                            }
                        }
                        if (purchasetype == "demand")
                        {
                            q = "update demand set status='Processed' where date='" + demanddate + "' and branchid='" + demandbranch + "' and supplierid='" + cmbsupplier.SelectedValue + "'";
                            objCore.executeQuery(q);
                        }
                        MessageBox.Show("Record Saved Successfully");
                        dt.Rows.Clear();
                        supplieraccount(tamount.ToString());
                        textBox1.Text = "";
                    }
                    else
                    {
                        double tamount = 0;// Convert.ToDouble(dataGridView1.Rows[(dataGridView1.Rows.Count - 1)].Cells["Total_Amount"].Value.ToString());
                        foreach (DataGridViewRow drr in dataGridView1.Rows)
                        {
                            string totalamount = drr.Cells["Total_Amount"].Value.ToString();
                            if (totalamount == "")
                            {
                                totalamount = "0";
                            }
                            tamount = tamount + Convert.ToDouble(totalamount);
                        }
                        string q = "update purchase set uploadstatus='Pending', SupplierId='" + cmbsupplier.SelectedValue + "',TotalAmount='" + tamount + "',Date='" + dateTimePicker1.Text + "',BranchCode='" + cmbbranch.SelectedValue + "',StoreCode='" + cmbstore.SelectedValue + "',InvoiceNo='" + txtinvoice.Text + "' where id='" + textBox1.Text + "'";

                        int res = objCore.executeQuery(q); ;
                        if (res == 0)
                        {
                            MessageBox.Show("Can not update Data");
                            return;
                        }

                        //try
                        //{
                        //    if (connection.State == ConnectionState.Open)
                        //        connection.Close();
                        //    connection.Open();
                        //    SqlCommand com = new SqlCommand(q, connection);
                        //    int res = com.ExecuteNonQuery();
                        //    if (res == 0)
                        //    {
                        //        MessageBox.Show("Can not update Data");
                        //        return;
                        //    }

                        //}
                        //catch (Exception ex)
                        //{
                        //    MessageBox.Show(ex.Message);
                        //    return;
                        //}
                        q = "delete from InventoryAccount where VoucherNo='GRN-" + textBox1.Text + "-" + cmbbranch.SelectedValue + "'";
                        objCore.executeQuery(q);
                        q = "delete from SupplierAccount where VoucherNo='GRN-" + textBox1.Text + "-" + cmbbranch.SelectedValue + "'";
                        objCore.executeQuery(q);
                        q = "delete from InventoryAccount where VoucherNo='GRN-" + textBox1.Text + "'";
                        objCore.executeQuery(q);
                        q = "delete from SupplierAccount where VoucherNo='GRN-" + textBox1.Text + "'";
                        objCore.executeQuery(q);
                        q = "delete from CashAccountPaymentSupplier where VoucherNo ='CPV-GRN-" + textBox1.Text + "'";
                        objCore.executeQuery(q);
                        q = "delete from SupplierAccount where VoucherNo ='CPV-GRN-" + textBox1.Text + "'";
                        objCore.executeQuery(q);
                        q = "delete from PurchaseDetails where PurchaseId='" + textBox1.Text + "'";
                        objCore.executeQuery(q);
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
                                string desc = dgr.Cells["Description"].Value.ToString();
                                string expiry = dgr.Cells["Expiry"].Value.ToString();
                                double pricepitem = 0, totalp = 0, items = 0;
                                if (totalamount == "")
                                {

                                }
                                else
                                {
                                    totalp = Convert.ToDouble(totalamount);
                                }
                                if (totalitems == "")
                                {

                                }
                                else
                                {
                                    items = Convert.ToDouble(totalitems);
                                }
                                try
                                {
                                    pricepitem = totalp / items;
                                }
                                catch (Exception ex)
                                {


                                }
                                int idd = 0;
                                ds = new DataSet();
                                ds = objCore.funGetDataSet("select max(id) as id from PurchaseDetails");
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
                                q = "insert into PurchaseDetails (expiry,id,RawItemId,PurchaseId,Package,PackageItems,TotalItems,Price,TotalAmount,description,PricePerItem) values('" + expiry + "','" + idd + "','" + rawid + "','" + textBox1.Text + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + totalamount + "','" + desc + "','" + pricepitem + "')";
                                objCore.executeQuery(q);
                                inventoryaccount(rawid, totalamount);
                            }
                        }
                        if (purchasetype == "demand")
                        {
                            q = "update demand set status='Processed' where date='" + demanddate + "' and branchid='" + demandbranch + "' and supplierid='" + cmbsupplier.SelectedValue + "'";
                            objCore.executeQuery(q);
                        }
                        MessageBox.Show("Record Updated Successfully");
                        dt.Rows.Clear();

                        supplieraccount(tamount.ToString());



                        textBox1.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }

        }

        public void inventoryaccount(string itmid, string amount)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";

                {
                    string ChartAccountId = "";
                    q = "select * from CashSalesAccountsList where AccountType='Inventory Account' and branchid='" + cmbbranch.SelectedValue + "'";
                    DataSet dsacountchk = new DataSet();
                    dsacountchk = objCore.funGetDataSet(q);
                    if (dsacountchk.Tables[0].Rows.Count > 0)
                    {
                        ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    string val = "";
                   
                    double prc = Convert.ToDouble(amount);

                    dsacount = new DataSet();
                    
                    {

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

                        q = "insert into InventoryAccount (Id,Date,ItemId,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + dateTimePicker1.Text + "','" + itmid + "','" + ChartAccountId + "','GRN-" + textBox1.Text + "-" + cmbbranch.SelectedValue + "','Purchase against Invoice No " + txtinvoice.Text.Replace("'", "") + "','" + Math.Round(Convert.ToDouble(prc), 2) + "','0','0','" + cmbbranch.SelectedValue + "')";
                        objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void supplieraccount(string amount)
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
                    q = "insert into SupplierAccount (invoiceno,Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + textBox1.Text + "-"+txtinvoice.Text+"','" + iddd + "','" + dateTimePicker1.Text + "','" + cmbsupplier.SelectedValue + "','" + PayableAccountId + "','GRN-" + textBox1.Text + "-"+cmbbranch.SelectedValue+"','Purchase against Invoice No " + txtinvoice.Text.Trim().Replace("'", "''") + "','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + cmbbranch.SelectedValue + "')";
                    objCore.executeQuery(q);
                    if (cmbpurchasetype.Text == "Cash")
                    {
                        paycashaccount(amount);
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void paycashaccount(string amount)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";

                {
                    string ChartAccountId = "";
                    q = "select * from CashSalesAccountsList where AccountType='Cash Purchase' and branchid='" + cmbbranch.SelectedValue + "'";
                    DataSet dsacountchk = new DataSet();
                    dsacountchk = objCore.funGetDataSet(q);
                    if (dsacountchk.Tables[0].Rows.Count > 0)
                    {
                        ChartAccountId = dsacountchk.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    }
                    string val = "";

                    double prc = Convert.ToDouble(amount);
                    int idd = 0;
                    DataSet dss = new DataSet();
                    dss = objCore.funGetDataSet("select max(id) as id from CashAccountPaymentSupplier");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string i = dss.Tables[0].Rows[0][0].ToString();
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
                    dsacount = new DataSet();

                    {
                        string q1 = "insert into CashAccountPaymentSupplier (id,Date,ChartAccountId,SupplierId,Voucherno,Description,Debit,Credit,branchid) values('" + idd + "','" + dateTimePicker1.Text + "','" + ChartAccountId + "','" + cmbsupplier.SelectedValue + "','CPV-GRN-" + textBox1.Text + "','Cash Paid against GRN-" + textBox1.Text.Trim() + "','0','" + prc + "','" + cmbbranch.SelectedValue + "')";
                        
                        dsacount = new DataSet();
                        q = "select payableaccountid from Supplier where id='" + cmbsupplier.SelectedValue + "' ";
                        dsacount = objCore.funGetDataSet(q);

                        string PayableAccountId = "";
                        try
                        {
                            if (dsacount.Tables[0].Rows.Count > 0)
                            {

                                PayableAccountId = dsacount.Tables[0].Rows[0][0].ToString();

                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        int iddd = 0;
                        DataSet ds = objCore.funGetDataSet("select max(id) as id from SupplierAccount");
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
                        string q2 = "insert into SupplierAccount (Id,Date,SupplierId,PayableAccountId,VoucherNo,Description,Debit,Credit,branchid) values('" + iddd + "','" + dateTimePicker1.Text + "','" + cmbsupplier.SelectedValue + "','" + PayableAccountId + "','CPV-GRN-" + textBox1.Text + "','Cash Paid against GRN-" + textBox1.Text.Trim() + "','" + Math.Round(Convert.ToDouble(prc), 2) + "','0','" + cmbbranch.SelectedValue + "')";
                        ExecuteSqlTransaction("", q1, q2, "Data Updated Successfully");
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        private static void ExecuteSqlTransaction(string connectionString, string q1, string q2, string message)
        {
            connectionString = POSRestaurant.Properties.Settings.Default.ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                SqlTransaction transaction;
                // Start a local transaction.
                transaction = connection.BeginTransaction("SampleTransaction");
                command.Connection = connection;
                command.Transaction = transaction;
                try
                {
                    command.CommandText = q1;
                    command.ExecuteNonQuery();
                    command.CommandText = q2;
                    command.ExecuteNonQuery();
                    // Attempt to commit the transaction.
                    transaction.Commit();
                    //MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("  Message: {0}" + ex.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show("Rollback Exception Type: {0}" + ex2.Message);
                    }
                }
            }
        }

        public string demanddate = "",demandbranch="";
        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillstore();
        }
        public static string pid = "";
        private void vButton2_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (txtexpected.Text.Trim().Length > 0)
                {
                    if (Convert.ToDouble(lblamount.Text) != Convert.ToDouble(txtexpected.Text))
                    {
                        MessageBox.Show("Invoice Expected Amount and Total Amount is not Equal");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            if (textBox1.Text.Length > 0)
            {
                string right = POSRestaurant.classes.Clsdbcon.authenticateEdit("Purchase Items", POSRestaurant.Properties.Settings.Default.UserId.ToString(), "update");
                if (right == "no")
                {
                    MessageBox.Show("You are not allowed to update");
                    return;
                }
            }
            savedata();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           // if(textBox1.Text)
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
            //    //dt.Rows.Add("", "", "", "", "", "Total Amount:", total.ToString());
            //}
            dataGridView1.Refresh();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            //DemandList obj = new DemandList(this);
            //obj.Show();
        }
        public void bindreport(string type)
        {
            try
            {

                if (textBox1.Text != string.Empty)
                {
                    //ReportDocument rptDoc = new ReportDocument();                    
                    POSRestaurant.Reports.Inventory.rprInventoryPurchase rptDoc = new Reports.Inventory.rprInventoryPurchase();
                    POSRestaurant.Reports.Inventory.rprInventoryInvoice rptDoc1 = new Reports.Inventory.rprInventoryInvoice();
                    POSRestaurant.Reports.Inventory.DsInventoryReceived dsrpt = new Reports.Inventory.DsInventoryReceived();
                    // .xsd file name
                    DataTable dt = new DataTable();
                    // Just set the name of data table
                    dt.TableName = "Crystal Report";
                    dt = getAllOrders();
                    if (dt.Rows.Count > 0)
                    {
                        dsrpt.Tables[0].Merge(dt);
                        if (type == "GRN")
                        {
                            rptDoc.SetDataSource(dsrpt);
                            rptDoc.PrintToPrinter(1, false, 0, 0);
                        }
                        if (type == "invoice")
                        {
                            rptDoc1.SetDataSource(dsrpt);
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
                dtrpt.Columns.Add("TotalAmount", typeof(double));
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
                dsinfo = objCore.funGetDataSet("SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseDetails.Package,                       dbo.PurchaseDetails.PackageItems, dbo.PurchaseDetails.TotalItems, dbo.PurchaseDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.Purchase.id='" + textBox1.Text + "'");
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
            int indx = dataGridView1.CurrentCell.RowIndex;

            if (indx >= 0)
            {
                string rawid = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                string pkg = dataGridView1.Rows[indx].Cells["Item_Per_Package"].Value.ToString();
                string pkgitems = dataGridView1.Rows[indx].Cells["Total_Packages"].Value.ToString();
                string totalitems = dataGridView1.Rows[indx].Cells["Total_Items"].Value.ToString();
                string price = dataGridView1.Rows[indx].Cells["Price_Per_Package"].Value.ToString();
                string totalamount = dataGridView1.Rows[indx].Cells["Total_Amount"].Value.ToString();
                txtdesc.Text = dataGridView1.Rows[indx].Cells["Description"].Value.ToString();
                cmbitem.SelectedValue= dataGridView1.Rows[indx].Cells[0].Value.ToString();
                txtpackage.Text = rawid;
                txtpackage.Text = pkg;
                txttotalpackages.Text = pkgitems;
                txtquantity.Text = totalitems;
                txtamount.Text = price;
                txttotalamount.Text = totalamount;
                vButton1.Text = "Update";
                cmbitem.Enabled = false;
                DataRow dtr = dt.Rows[indx];
                dtr.Delete();
                dt.AcceptChanges();
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
                POSRestaurant.Reports.Inventory.frmInventoryPreview obj = new Reports.Inventory.frmInventoryPreview();
                obj.id = textBox1.Text;
                obj.Show();
            }
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == string.Empty)
                { }
                else
                {
                    DialogResult rs = MessageBox.Show("Are You Sure to Cancel?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (rs == DialogResult.Yes)
                    {
                        //DataSet dsitems = new DataSet();
                        //string q = "select * from PurchaseDetails where PurchaseId='" + textBox1.Text + "'";
                        //dsitems = objCore.funGetDataSet(q);
                        //for (int i = 0; i < dsitems.Tables[0].Rows.Count; i++)
                        //{
                        //    inventryreverse(dsitems.Tables[0].Rows[i]["RawItemId"].ToString(), Convert.ToDouble(dsitems.Tables[0].Rows[i]["TotalItems"].ToString()));
                        //}

                        string q = "delete from PurchaseDetails  where PurchaseId='" + textBox1.Text + "'";
                        int res = objCore.executeQuery(q);
                        if (res > 0)
                        {
                           // objCore.executeQuery("update purchase set status='Cancel',uploadstatus='Pending' where id='" + textBox1.Text + "'");
                            objCore.executeQuery("delete from purchase  where id='" + textBox1.Text + "'");
                            objCore.executeQuery("delete from InventoryAccount  where voucherno='GRN-" + textBox1.Text + "'");
                            objCore.executeQuery("delete from SupplierAccount  where voucherno='GRN-" + textBox1.Text + "'");
                            q = "delete from CashAccountPaymentSupplier where VoucherNo ='CPV-GRN-" + textBox1.Text + "'";
                            objCore.executeQuery(q);
                            q = "delete from SupplierAccount where VoucherNo ='CPV-GRN-" + textBox1.Text + "'";
                            objCore.executeQuery(q);
                            MessageBox.Show("Purchase Deleted Successfully");
                            textBox1.Text = "";
                            dt.Rows.Clear();
                            dataGridView1.Rows.Clear();
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            int rowindx = dataGridView1.CurrentCell.RowIndex;
            DataRow dtr = dt.Rows[rowindx];
            dtr.Delete();
            dt.AcceptChanges();
        }
        public void getprice()
        {
            try
            {
                string price = "";
                DataSet ds = new DataSet();
                string q = "select top 1 price from rawitem where id='" + cmbitem.SelectedValue + "' order by id desc";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string temp = ds.Tables[0].Rows[0][0].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    price = temp;
                }
                if (price == "0")
                {
                    q = "select top 1 price from PurchaseDetails where RawItemId='" + cmbitem.SelectedValue + "' order by id desc";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string temp = ds.Tables[0].Rows[0][0].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        price = temp;
                    }
                }
                txtamount.Text = price;
            }
            catch (Exception wx)
            {
                
               
            }
        }
        private void cmbitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            getprice();
        }

        private void vButton11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            dt.Rows.Clear();
        }

        private void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillitems();
        }

        private void Purchase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                savedata();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void Purchase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("Are You Sure to Exit?", "There are unsaved rows", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void txtpo_KeyDown(object sender, KeyEventArgs e)
        
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {
                   
                    string q = "SELECT dbo.PurchaseOrderDetails.RawItemId AS ItemId,dbo.RawItem.ItemName  AS Item,dbo.PurchaseOrderDetails.Package AS Item_Per_Package, dbo.PurchaseOrderDetails.PackageItems  AS Total_Packages, dbo.PurchaseOrderDetails.TotalItems AS Total_Items, dbo.PurchaseOrderDetails.Price AS Price_Per_Package, dbo.PurchaseOrderDetails.TotalAmount AS Total_Amount, dbo.PurchaseOrderDetails.Description FROM            dbo.PurchaseOrder INNER JOIN                         dbo.Supplier ON dbo.PurchaseOrder.SupplierId = dbo.Supplier.Id INNER JOIN                         dbo.PurchaseOrderDetails ON dbo.PurchaseOrder.Id = dbo.PurchaseOrderDetails.PurchaseOrderId INNER JOIN                         dbo.RawItem ON dbo.PurchaseOrderDetails.RawItemId = dbo.RawItem.Id where dbo.PurchaseOrder.Id='" + txtpo.Text + "'";
                    DataSet ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    try
                    {
                        dt.Rows.Clear();
                    }
                    catch (Exception ex)
                    {


                    }

                    dt.Merge(ds.Tables[0], true, MissingSchemaAction.Ignore);
                    
                    dataGridView1.DataSource = null;
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].Visible = false;
                    q = "SELECT        dbo.Supplier.Name AS Supplier_Name, dbo.PurchaseOrder.Id AS PONo, dbo.PurchaseOrder.InvoiceNo, dbo.PurchaseOrder.Date, dbo.PurchaseOrderDetails.TotalItems, dbo.PurchaseOrderDetails.RawItemId,                          dbo.PurchaseOrderDetails.Package, dbo.PurchaseOrderDetails.PackageItems, dbo.PurchaseOrderDetails.Price, dbo.PurchaseOrderDetails.PricePerItem, dbo.PurchaseOrderDetails.TotalAmount, dbo.PurchaseOrder.SupplierId,                          dbo.RawItem.ItemName FROM            dbo.PurchaseOrder INNER JOIN                         dbo.Supplier ON dbo.PurchaseOrder.SupplierId = dbo.Supplier.Id INNER JOIN                         dbo.PurchaseOrderDetails ON dbo.PurchaseOrder.Id = dbo.PurchaseOrderDetails.PurchaseOrderId INNER JOIN                         dbo.RawItem ON dbo.PurchaseOrderDetails.RawItemId = dbo.RawItem.Id where dbo.PurchaseOrder.Id='" + txtpo.Text + "'";
                    DataSet ds1 = new DataSet();
                    ds1 = objCore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        dateTimePicker1.Text = ds1.Tables[0].Rows[0]["date"].ToString();
                        cmbsupplier.SelectedValue = ds1.Tables[0].Rows[0]["SupplierId"].ToString();

                    }
                    double total = 0;
                    try
                    {
                        foreach (DataRow dtr in dt.Rows)
                        {
                            try
                            {
                                                                
                                    total = total + Convert.ToDouble(dtr["Total_Amount"].ToString());
                                
                            }
                            catch (Exception ex)
                            {


                            }
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    
                    lblamount.Text = total.ToString("#,##0");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                dateTimePicker2.Visible = false;
            }
            else
            {
                dateTimePicker2.Visible = true;
            }
        }
    }
}
