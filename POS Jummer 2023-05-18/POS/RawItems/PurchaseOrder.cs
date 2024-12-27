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
    public partial class PurchaseOrder : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        
        public PurchaseOrder()
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
                string q = "SELECT     dbo.PurchaseOrderDetails.RawItemId AS ItemId, dbo.RawItem.ItemName AS Item, dbo.PurchaseOrderDetails.Package AS Item_Per_Package, dbo.PurchaseOrderDetails.PackageItems AS Total_Packages,                       dbo.PurchaseOrderDetails.TotalItems AS Total_Items, dbo.PurchaseOrderDetails.Price AS Price_Per_Package, dbo.PurchaseOrderDetails.TotalAmount AS Total_Amount, dbo.PurchaseOrderDetails.Description FROM         dbo.RawItem INNER JOIN                      dbo.PurchaseOrderDetails ON dbo.RawItem.Id = dbo.PurchaseOrderDetails.RawItemId INNER JOIN                      dbo.PurchaseOrder ON dbo.PurchaseOrderDetails.PurchaseOrderId = dbo.PurchaseOrder.Id where dbo.PurchaseOrderDetails.PurchaseOrderId='" + id + "' and dbo.PurchaseOrder.Status !='Cancel'";
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
                q = "select * from PurchaseOrder where id='" + id + "' and Status !='Cancel'";
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    cmbsupplier.SelectedValue = dss.Tables[0].Rows[0]["SupplierId"].ToString();
                    //cmbbranch.SelectedValue = dss.Tables[0].Rows[0]["BranchCode"].ToString();
                    //cmbstore.SelectedValue = dss.Tables[0].Rows[0]["StoreCode"].ToString();
                    textBox1.Text = dss.Tables[0].Rows[0]["Id"].ToString();
                    txtinvoice.Text = dss.Tables[0].Rows[0]["InvoiceNo"].ToString();
                    dateTimePicker1.Text = dss.Tables[0].Rows[0]["date"].ToString();
                }
                else
                {
                    try
                    {
                        cmbsupplier.Text = "Please Select";
                       
                        
                        textBox1.Text = "";
                        txtinvoice.Text = "";
                       
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
            dt.Columns.Add("Description", typeof(string));

            string q = "select * from Branch where status='Active'";
            ds = objCore.funGetDataSet(q);
            //DataRow dr = ds.Tables[0].NewRow();
            //dr["Name"] = "Please Select";

            //ds.Tables[0].Rows.Add(dr);
            cmbgroup.DataSource = ds.Tables[0];
            cmbgroup.ValueMember = "id";
            cmbgroup.DisplayMember = "branchname";
           // cmbsupplier.Text = "Please Select";


            ds = new DataSet();
            q = "select * from supplier";
            ds = objCore.funGetDataSet(q);
            //DataRow dr = ds.Tables[0].NewRow();
            //dr["Name"] = "Please Select";

            //ds.Tables[0].Rows.Add(dr);
            cmbsupplier.DataSource = ds.Tables[0];
            cmbsupplier.ValueMember = "id";
            cmbsupplier.DisplayMember = "Name";
            // cmbsupplier.Text = "Please Select";
           
            //fillgroup();
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
                
                {
                    q = "SELECT dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.UOM.UOM FROM  dbo.RawItem INNER JOIN              dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.status='Active' or dbo.RawItem.status is null order by dbo.RawItem.ItemName";
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


                double total = 0;
               
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

                dt.Rows.Add(cmbitem.SelectedValue.ToString(), cmbitem.Text, txtpackage.Text, txttotalpackages.Text, txtquantity.Text, txtamount.Text, txttotalamount.Text,"");
                //dt.Rows.Add("", "", "", "", "", "Total Amount:", total.ToString());

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                
                vButton1.Text = "Add to List";
                cmbitem.Enabled = true;
                txttotalamount.Text = string.Empty;
                txtquantity.Text = string.Empty;
                txtpackage.Text = string.Empty;
                
                txttotalpackages.Text = string.Empty;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
   
        private void vButton2_Click(object sender, EventArgs e)
        {
            
        }
        
        public void savedata()
        {
           
            if (cmbsupplier.Text == "Please Select" || cmbsupplier.Text == "")
            {
                MessageBox.Show("Please Select Supplier");
                cmbsupplier.Focus();
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
                        int id = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from PurchaseOrder");
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
                        string q = "insert into PurchaseOrder (branchid,status,id,SupplierId,TotalAmount,Date,InvoiceNo) values('"+cmbgroup.SelectedValue+"','Pending','" + id + "','" + cmbsupplier.SelectedValue + "','" + tamount + "','" + dateTimePicker1.Text.Trim() + "','" + txtinvoice.Text.Trim().Replace("'", "''") + "')";
                        
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
                                ds = objCore.funGetDataSet("select max(id) as id from PurchaseOrderDetails");
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
                                    q = "insert into PurchaseOrderDetails (id,RawItemId,PurchaseOrderId,Package,PackageItems,TotalItems,Price,TotalAmount,description,PricePerItem) values('" + idd + "','" + rawid + "','" + id + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + totalamount + "','" + desc + "','" + pricepitem + "')";
                                    string cs = POSRestaurant.Properties.Settings.Default.ConnectionString;
                                    System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(cs);
                                    string s = "";
                                    System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand(q, con);
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                   
                                   
                                }
                                catch (Exception exx)
                                {
                                    q = "delete from PurchaseOrder where id='" + id + "'";
                                    objCore.executeQuery(q);
                                    MessageBox.Show(exx.Message + "-" + q);

                                }
                                
                            }
                        }
                        
                        MessageBox.Show("Record Saved Successfully");
                        dt.Rows.Clear();
                      
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
                        string q = "update PurchaseOrder set status='pending',branchid='"+cmbgroup.SelectedValue+"', uploadstatus='Pending', SupplierId='" + cmbsupplier.SelectedValue + "',TotalAmount='" + tamount + "',Date='" + dateTimePicker1.Text + "',InvoiceNo='" + txtinvoice.Text + "' where id='" + textBox1.Text + "'";

                        

                        try
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                            connection.Open();
                            SqlCommand com = new SqlCommand(q, connection);
                            int res = com.ExecuteNonQuery();
                            if (res == 0)
                            {
                                MessageBox.Show("Can not update Data");
                                return;
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return;
                        }

                        q = "delete from PurchaseOrderDetails where PurchaseOrderId='" + textBox1.Text + "'";
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
                                ds = objCore.funGetDataSet("select max(id) as id from PurchaseOrderDetails");
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
                                q = "insert into PurchaseOrderDetails (id,RawItemId,PurchaseOrderId,Package,PackageItems,TotalItems,Price,TotalAmount,description,PricePerItem) values('" + idd + "','" + rawid + "','" + textBox1.Text + "','" + pkg + "','" + pkgitems + "','" + totalitems + "','" + price + "','" + totalamount + "','" + desc + "','" + pricepitem + "')";
                                objCore.executeQuery(q);
                                
                            }
                        }
                        
                        MessageBox.Show("Record Updated Successfully");
                        dt.Rows.Clear();

                        textBox1.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }

        }

      
        public string demanddate = "",demandbranch="";
        private void cmbbranch_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
        public static string pid = "";
        private void vButton2_Click_1(object sender, EventArgs e)
        {
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
                dsinfo = objCore.funGetDataSet("SELECT     dbo.Supplier.Name AS Supplier,  dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.Id AS serialNo, dbo.PurchaseOrderDetails.Package,                       dbo.PurchaseOrderDetails.PackageItems, dbo.PurchaseOrderDetails.TotalItems, dbo.PurchaseOrderDetails.Price,dbo.Purchase.TotalAmount,  dbo.PurchaseOrderDetails.TotalAmount AS PurchaseTotalAmount, dbo.RawItem.ItemName,                      dbo.UOM.UOM FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseOrderDetails ON dbo.Purchase.Id = dbo.PurchaseOrderDetails.PurchaseId INNER JOIN                      dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.RawItem ON dbo.PurchaseOrderDetails.RawItemId = dbo.RawItem.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.Purchase.id='" + textBox1.Text + "'");
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
                //txtdesc.Text = dataGridView1.Rows[indx].Cells["Description"].Value.ToString();
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
            PurChaseOrderList obj = new PurChaseOrderList(this);
            obj.Show();
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            //if (textBox1.Text != string.Empty)
            //{
            //    POSRestaurant.Reports.Inventory.frmInventoryPreview obj = new Reports.Inventory.frmInventoryPreview();
            //    obj.id = textBox1.Text;
            //    obj.Show();
            //}
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
                        
                        objCore.executeQuery("delete from PurchaseOrderDetails  where PurchaseId='" + textBox1.Text + "'");
                        objCore.executeQuery("update purchase set status='Cancel',uploadstatus='Pending' where id='" + textBox1.Text + "'");
                        //objCore.executeQuery("delete from purchase  where id='" + textBox1.Text + "'");
                        objCore.executeQuery("delete from InventoryAccount  where voucherno='GRN-" + textBox1.Text + "'");
                        objCore.executeQuery("delete from SupplierAccount  where voucherno='GRN-" + textBox1.Text + "'");
                        MessageBox.Show("Purchase Deleted Successfully");
                        textBox1.Text = "";
                        dt.Rows.Clear();
                        dataGridView1.Rows.Clear();

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
            double closing = 0;

            try
            {
                closing = getclosing(cmbitem.SelectedValue.ToString());
                lblqty.Text = "Closing Stock= " + closing.ToString();
            }
            catch (Exception ex)
            {


            }
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
                    q = "select top 1 price from PurchaseOrderDetails where RawItemId='" + cmbitem.SelectedValue + "' order by id desc";
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
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public double getclosing(string id)
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, minorder = 0, balance = 0, closing = 0;
            double qty = 0;
            DataTable ds = new DataTable();
            DataTable dtrpt = new DataTable();
            try
            {



                string q = "";
                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.UOM.UOM, dbo.RawItem.MinOrder FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.Id='" + id + "' order by dbo.RawItem.ItemName";


                DataSet ds1 = new DataSet();
                ds1 = objcore.funGetDataSet(q);
                for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
                {
                    purchased = 0; consumed = 0; variance = 0; price = 0; discard = 0; staff = 0; closing = 0;
                    double cmplt = 0;
                    if (ds1.Tables[0].Rows[i]["id"].ToString() == "224")
                    {

                    }
                    double openin = opening(ds1.Tables[0].Rows[i]["id"].ToString());
                    qty = openin;
                    string val = "";
                    double rem = 0;
                    minorder = 0; balance = 0;
                    string temp = ds1.Tables[0].Rows[i]["MinOrder"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    minorder = Convert.ToDouble(temp);
                    DataSet dspurchase = new DataSet();
                    q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date ='" + dateTimePicker1.Text + "'  and dbo.PurchaseDetails.RawItemId='" + id + "'";

                    try
                    {
                        dspurchase = objcore.funGetDataSet(q);
                        if (dspurchase.Tables[0].Rows.Count > 0)
                        {
                            val = dspurchase.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            purchased = Convert.ToDouble(val);
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    val = "";
                    purchased = Math.Round(purchased, 2);
                    //qty = qty + purchased;



                    val = "";
                    purchased = Math.Round(purchased, 2);
                    qty = qty + purchased;

                    q = "";
                    dspurchase = new DataSet();
                    //try
                    //{
                    //    q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where   Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                    //    dspurchase = objcore.funGetDataSet(q);
                    //    if (dspurchase.Tables[0].Rows.Count > 0)
                    //    {
                    //        val = dspurchase.Tables[0].Rows[0][0].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        consumed = Convert.ToDouble(val);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    val = ""; q = "";
                    double rate = 0;
                    DataSet dscon = new DataSet();
                    q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                    dscon = objcore.funGetDataSet(q);
                    if (dscon.Tables[0].Rows.Count > 0)
                    {
                        rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                    }
                    consumed = consumed / rate;

                    qty = qty - consumed;
                    dspurchase = new DataSet();
                    q = "";
                    dspurchase = new DataSet();
                    q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste FROM     discard where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                    //try
                    //{
                    //    dspurchase = objcore.funGetDataSet(q);
                    //    if (dspurchase.Tables[0].Rows.Count > 0)
                    //    {
                    //        val = dspurchase.Tables[0].Rows[0][0].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        discard = Convert.ToDouble(val);

                    //        val = dspurchase.Tables[0].Rows[0][1].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        staff = Convert.ToDouble(val);
                    //        val = dspurchase.Tables[0].Rows[0][2].ToString();
                    //        if (val == "")
                    //        {
                    //            val = "0";
                    //        }
                    //        cmplt = Convert.ToDouble(val);
                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    //q = "";
                    //if (rate > 0)
                    //{
                    //    cmplt = cmplt / rate;
                    //}
                    double tint = 0, tout = 0;
                    DataSet dsin = new DataSet();
                    q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                    try
                    {
                        dsin = objcore.funGetDataSet(q);
                        if (dsin.Tables[0].Rows.Count > 0)
                        {
                            val = dsin.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            tint = Convert.ToDouble(val);
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    q = "";
                    dsin = new DataSet();
                    q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                    try
                    {
                        dsin = objcore.funGetDataSet(q);
                        if (dsin.Tables[0].Rows.Count > 0)
                        {
                            val = dsin.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            tout = Convert.ToDouble(val);
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        q = "";


                        dsin = new DataSet();


                        q = "SELECT     SUM(Quantity ) AS Expr1 FROM     InventoryTransferStore where  Date <= '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";

                        dsin = objcore.funGetDataSet(q);
                        if (dsin.Tables[0].Rows.Count > 0)
                        {
                            val = dsin.Tables[0].Rows[0][0].ToString();
                            if (val == "")
                            {
                                val = "0";
                            }
                            tout = tout + Convert.ToDouble(val);
                        }


                    }
                    catch (Exception ex)
                    {

                    }
                    double ideal = 0;
                    // discard = discard * -1;
                    qty = qty - (discard * -1);
                    qty = qty - (staff);
                    qty = qty - (cmplt);
                    qty = qty + tint;
                    qty = qty - tout;
                    qty = Math.Round(qty, 2);
                    discard = 0;
                    string date2 = "";
                    string tempchk = "yes"; q = "";
                    q = "SELECT   top 1   remaining,date FROM     closing where Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker1.Text + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and kdsid is NULL order by date desc";

                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        date2 = dspurchase.Tables[0].Rows[0]["date"].ToString();
                        val = dspurchase.Tables[0].Rows[0]["remaining"].ToString();
                        if (val == "")
                        {
                            tempchk = "no";
                            val = "0";
                        }
                        else
                        {
                            closing = Convert.ToDouble(val);
                            if (date2 == "")
                            {
                                date2 = date;
                            }
                            if (Convert.ToDateTime(date2) < Convert.ToDateTime(dateTimePicker1.Text))
                            {
                                closing = closing + openingclosing(ds1.Tables[0].Rows[i]["id"].ToString(), date2, closing);
                            }
                        }
                    }
                    else
                    {
                        tempchk = "no";
                    }


                    double actual = (openin + purchased + tint) - (staff + cmplt + tout);
                    double actual1 = (openin + purchased + tint) - (staff + cmplt + tout);
                    actual = actual - closing;
                    if (tempchk == "yes")
                    {

                        {
                            discard = consumed - actual;
                        }
                    }
                    else
                    {
                        closing = actual;
                        closing = closing - consumed;
                    }
                    ideal = actual1 - consumed;



                    //double closingval = 0, purchaseval = 0, saleval = 0, discardval = 0, comptval = 0, staffval = 0;

                    //balance = price * discard;
                    //wastage = wastage + ((staff + cmplt) * price);
                    //closingamount = closingamount + (price * closing);
                    //closingval = price * closing;
                    //double openingval = openin * price;
                    //purchaseval = purchased * price;
                    //saleval = consumed * price;
                    //discardval = price * discard;
                    //comptval = cmplt * price;
                    //staffval = staff * price;


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return closing;
        }
        public double openingclosing(string itemid, string date, double closing)
        {

            string date2 = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            q = "SELECT top 1 date, (remaining) as rem FROM     closing where  Date <'" + date + "' and itemid='" + itemid + "' adn kdsid is NULL order by Date desc";

            DateTime end1 = Convert.ToDateTime(date2);
            DateTime start1 = Convert.ToDateTime(date);
            start1 = start1.AddDays(1);

            string start = start1.ToString("yyyy-MM-dd");
            string end = end1.ToString("yyyy-MM-dd");
            q = "";
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";

            try
            {
                dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }

            try
            {
                q = "";
                dspurchase = new DataSet();
                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";

                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = purchased + Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            val = ""; q = "";
            purchased = Math.Round(purchased, 2);
            try
            {
                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";

                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    consumed = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }

            dspurchase = new DataSet();

            q = "";
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    transferin = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }
            q = "";
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    transferout = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }

            q = "";
            val = "";
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
                consumed = consumed / rate;
            }
            double qty = 0;
            qty = purchased - consumed;
            dspurchase = new DataSet();
            //q = "SELECT     SUM(variance) AS Expr1 FROM     Variance where Date <'" + date + "' and itemid='" + itemid + "'";
            //dspurchase = objcore.funGetDataSet(q);
            //if (dspurchase.Tables[0].Rows.Count > 0)
            //{
            //    val = dspurchase.Tables[0].Rows[0][0].ToString();
            //    if (val == "")
            //    {
            //        val = "0";
            //    }
            //    variance = Convert.ToDouble(val);
            //}
            ////if (variance > 0)
            //{
            //    qty = qty + (variance);
            //}
            dspurchase = new DataSet();
            q = "";
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid is null";

            try
            {
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    discard = Convert.ToDouble(val);
                    val = dspurchase.Tables[0].Rows[0][1].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    staff = Convert.ToDouble(val);
                    val = dspurchase.Tables[0].Rows[0][2].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    complt = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        public double opening(string itemid)
        {
            string date = dateTimePicker1.Text;

            string date2 = "";
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0, staff = 0, transferin = 0, transferout = 0, complt = 0, closing = 0;
            double opening = 0;
            string val = "";
            double rem = 0;
            DataSet dspurchase = new DataSet();

            dspurchase = new DataSet();
            string q = "";
            q = "SELECT top 1 date, (remaining) as rem FROM     closing where Date <'" + date + "' and itemid='" + itemid + "'  and kdsid is NULL   order by Date desc";

            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                date2 = dspurchase.Tables[0].Rows[0][0].ToString();
                val = dspurchase.Tables[0].Rows[0][1].ToString();
                if (val == "")
                {
                    val = "0";
                }
                closing = Convert.ToDouble(val);
            }
            if (date2 == "")
            {
                date2 = date;
            }
            DateTime end1 = Convert.ToDateTime(date);
            DateTime start1 = Convert.ToDateTime(date2);
            TimeSpan ts = Convert.ToDateTime(date) - Convert.ToDateTime(date2);
            int days = ts.Days;
            if (days <= 1)
            {
                return closing;
            }
            start1 = start1.AddDays(1);
            end1 = end1.AddDays(-1);
            string start = start1.ToString("yyyy-MM-dd");
            string end = end1.ToString("yyyy-MM-dd");
            q = "";
            q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date between '" + start + "' and  '" + end + "' and dbo.PurchaseDetails.RawItemId='" + itemid + "'";

            try
            {
                dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            } q = "";
            try
            {
                dspurchase = new DataSet();
                q = "SELECT     SUM(dbo.Production.Quantity) AS Expr1 FROM         Production  where (dbo.Production.date between '" + start + "' and '" + end + "') and dbo.Production.ItemId='" + itemid + "' and dbo.Production.status='Posted'";

                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = purchased + Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {


            }
            val = "";
            purchased = Math.Round(purchased, 2);
            q = "";
            dspurchase = new DataSet();
            try
            {
                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date between '" + start + "' and  '" + end + "' and RawItemId='" + itemid + "'";

            }
            catch (Exception ex)
            {

            }
            dspurchase = objcore.funGetDataSet(q);
            if (dspurchase.Tables[0].Rows.Count > 0)
            {
                val = dspurchase.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                consumed = Convert.ToDouble(val);
            } q = "";
            DataSet dsin = new DataSet();
            q = "SELECT     SUM(TransferIn) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    transferin = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }
            q = "";

            q = "";
            dsin = new DataSet();
            q = "SELECT     SUM(TransferOut) AS Expr1 FROM     InventoryTransfer where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "'";

            try
            {
                dsin = objcore.funGetDataSet(q);
                if (dsin.Tables[0].Rows.Count > 0)
                {
                    val = dsin.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    transferout = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }

            q = "";
            val = "";
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
                consumed = consumed / rate;
            }
            double qty = 0;
            qty = purchased - consumed;
            dspurchase = new DataSet();
            //q = "SELECT     SUM(variance) AS Expr1 FROM     Variance where Date <'" + date + "' and itemid='" + itemid + "'";
            //dspurchase = objcore.funGetDataSet(q);
            //if (dspurchase.Tables[0].Rows.Count > 0)
            //{
            //    val = dspurchase.Tables[0].Rows[0][0].ToString();
            //    if (val == "")
            //    {
            //        val = "0";
            //    }
            //    variance = Convert.ToDouble(val);
            //}
            ////if (variance > 0)
            //{
            //    qty = qty + (variance);
            //}
            q = "";
            dspurchase = new DataSet();
            q = "SELECT     SUM(discard) AS Expr1,SUM(staff) AS staff,SUM(completewaste) AS completewaste,SUM(remaining) as rem FROM     discard where Date between '" + start + "' and  '" + end + "' and itemid='" + itemid + "' and kdsid is NULL";

            try
            {
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    discard = Convert.ToDouble(val);
                    val = dspurchase.Tables[0].Rows[0][1].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    staff = Convert.ToDouble(val);
                    val = dspurchase.Tables[0].Rows[0][2].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    complt = Convert.ToDouble(val);
                }
            }
            catch (Exception ex)
            {

            }
            if (rate > 0)
            {
                complt = complt / rate;
            }

            closing = (closing + purchased + transferin) - (staff + complt + transferout + consumed);
            //qty = (qty + transferin) - ((discard*-1) + staff + transferout + complt);
            closing = Math.Round(closing, 2);
            return closing;
        }
        private void vButton11_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            dt.Rows.Clear();
        }

        private void cmbgroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            //fillitems();
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
    }
}
