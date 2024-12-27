using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Net;
using System.IO;
using Newtonsoft.Json;
namespace POSRestaurant.forms
{
    public partial class Downloadtransferin : Form
    {
        public Downloadtransferin()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        string cs = "", url = "";
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='demand' ";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
                }
                else
                {
                    q = "select * from deliverytransfer where server='main'";
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tp = ds.Tables[0].Rows[0]["type"].ToString();
                        url = ds.Tables[0].Rows[0]["url"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {


            }
            if (tp == "")
            {
                tp = "sql";
            }
            return tp;
        }
        private void Downloadpurchase_Load(object sender, EventArgs e)
        {
            dtpurchase.Columns.Add("Date", typeof(string));
            dtpurchase.Columns.Add("Quantity", typeof(string));
            dtpurchase.Columns.Add("Remarks", typeof(string));
            dtpurchase.Columns.Add("Sourcebranchid", typeof(string));
            dtpurchase.Columns.Add("Download_Date", typeof(string));
            try
            {
                DataSet ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet("select * from SqlServerInfo where type='server'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string server = ds.Tables[0].Rows[0]["ServerName"].ToString();
                    string db = ds.Tables[0].Rows[0]["DbName"].ToString();
                    string user = ds.Tables[0].Rows[0]["UserName"].ToString();
                    string password = ds.Tables[0].Rows[0]["Password"].ToString();
                    cs = "Data Source=" + server + ";Initial Catalog=" + db + ";Persist Security Info=True;User ID=" + user + ";Password=" + password + "";
                }
            }
            catch (Exception ex)
            {


            }


            try
            {
                DataSet ds = new DataSet();


                SqlConnection connection = new SqlConnection(cs);
                SqlCommand com;


                string q = "select id,branchname from branch";
                ds = objCore.funGetDataSet(q);
                try
                {
                    
                    comboBox1.DataSource = ds.Tables[0];
                    comboBox1.ValueMember = "id";
                    comboBox1.DisplayMember = "branchname";
                    if (type() == "service")
                    {
                        getmenuservice();
                    }
                    else
                    {
                        getmenu();
                    }

                }
                catch (Exception ex)
                {

                    // MessageBox.Show(ex.Message);
                }


            }
            catch (Exception ex)
            {


            }
        }
        public DataTable dtpurchase = new DataTable();
        transfercls[] datat;
        protected void getmenuservice()
        {
            datat = new transfercls[10];
            try
            {
                string q = "SELECT   distinct      date ,sourcebranchid,count(id) as Quantity,datedownload FROM            InventoryTransfer where  sourcebranchid is not null  and TransferIn>0 group by date,datedownload,sourcebranchid order by date desc";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    datat = new transfercls[ds.Tables[0].Rows.Count];
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        datat[i] =
                    new transfercls()
                    {
                        Date = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("yyyy-MM-dd"),

                        TransferIn = ds.Tables[0].Rows[i]["Quantity"].ToString(),
                        sourcebranchid = ds.Tables[0].Rows[i]["sourcebranchid"].ToString(),
                        DateDownload = ds.Tables[0].Rows[i]["datedownload"].ToString()


                    };
                    }

                }
                else
                {
                    datat = new transfercls[1];
                    datat[0] =
                    new transfercls()
                    {
                        Date = ""

                    };
                }
            }
            catch (Exception ex)
            {

            }
            try
            {
                string uri = url + "/DeliveryServices/downloadtransfer.asmx/Getresponse?id=" + comboBox1.SelectedValue.ToString();
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<transfercls> res = (List<transfercls>)JsonConvert.DeserializeObject(result, typeof(List<transfercls>));
                    foreach (var item in res)
                    {
                        if (item.Date != "")
                        {

                            dtpurchase.Rows.Add(item.Date, item.TransferIn,item.Remarks,item.sourcebranchid);

                        }
                    }
                    dataGridView1.DataSource = dtpurchase;
                    int i = 0;
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        string dat = dr.Cells["date"].Value.ToString();
                        string dat2 = "";
                        int count = 0;
                        try
                        {
                            count = datat.Where(s => s.Date == dat).ToList().Count;
                            dat2 = datat.Where(s => s.Date == dat).ToList()[0].DateDownload;
                        }
                        catch (Exception ex)
                        {


                        }
                        if (count > 0)
                        {

                            dr.DefaultCellStyle.BackColor = Color.Green;
                            dtpurchase.Rows[i]["Download_Date"] = dat2;
                        }
                        i++;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected void getmenu()
        {
            string q = "";
            q = "SELECT        dbo.Purchase.Id, dbo.Supplier.id AS SupplierCode, dbo.Supplier.Name AS Supplier, dbo.Purchase.Date, dbo.Purchase.InvoiceNo, dbo.Purchase.TotalAmount FROM            dbo.Purchase INNER JOIN                         dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id where dbo.Purchase.BranchCode='" + comboBox1.SelectedValue + "' order by dbo.Purchase.Id desc";            
            DataSet dsmenu = new DataSet();
            SqlConnection connection = new SqlConnection(cs);
            SqlCommand com;
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Open();
                com = new SqlCommand(q, connection);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dsmenu);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            dataGridView1.DataSource = dsmenu.Tables[0];

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getmenu();
        }
        protected void downloadbyservice( string type)
        {
            try
            {
                button1.Text = "Please Wait";
                button1.Enabled = false;
                bool chkk = false;
               
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell chkchecking = row.Cells[0] as DataGridViewCheckBoxCell;

                    if (Convert.ToBoolean(chkchecking.Value) == true || type=="All")
                    {
                        string date = row.Cells["date"].Value.ToString();
                        string uri = url + "/DeliveryServices/downloadtransferdetails.asmx/Getresponse?id=" + comboBox1.SelectedValue + "&date=" + date;
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        string result = "";
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            result = readStream.ReadToEnd();
                        }
                        List<transfercls> res = (List<transfercls>)JsonConvert.DeserializeObject(result, typeof(List<transfercls>));

                        // q = "insert into InventoryTransfer (onlineid,Date, Itemid, TransferIn, TransferOut, branchid, Remarks, uploadstatus, sourcebranchid, status) values ( '" + row.Cells["onlineid"].Value.ToString() + "', '" + row.Cells["Date"].Value.ToString() + "', '" + row.Cells["TotalAmount"].Value.ToString() + "', '" + row.Cells["Date"].Value.ToString() + "', '" + row.Cells["BranchCode"].Value.ToString() + "', '" + row.Cells["StoreCode"].Value.ToString() + "', '" + row.Cells["InvoiceNo"].Value.ToString() + "', '" + row.Cells["status"].Value.ToString() + "')";
                        SqlConnection connection = new SqlConnection(objCore.getConnectionString());
                        SqlCommand com;
                        bool chk = false;
                        {
                            double amount = 0;
                            string q = "delete from InventoryTransfer where date='" + date + "'  and sourcebranchid is not null";
                            objCore.executeQuery(q);
                            foreach (var item in res)
                            {
                                if (item.Itemid != "")
                                {

                                    q = "insert into InventoryTransfer (datedownload,price,total, onlineid,Date, Itemid, TransferIn, TransferOut, branchid, Remarks,  sourcebranchid) values ( '" + DateTime.Now + "','" + item.price + "', '" + item.total + "', '" + item.onlineid + "', '" + item.Date + "', '" + item.Itemid + "', '" + item.TransferIn + "', '" + item.TransferOut + "','" + item.branchid + "','" + item.Remarks + "','" + item.sourcebranchid + "')";
                                    if (connection.State == ConnectionState.Open)
                                        connection.Close();
                                    connection.Open();
                                    com = new SqlCommand(q, connection);
                                    int reslt = com.ExecuteNonQuery();
                                    connection.Close();
                                    string temp = "";
                                    temp = item.total;
                                    if (temp == "")
                                    {
                                        temp = "0";
                                    }
                                    try
                                    {
                                        amount = amount + Convert.ToDouble(temp);
                                    }
                                    catch (Exception ex)
                                    {

                                    }
                                }

                            }
                            accounts(date, amount);

                        } 
                    }
                    

                }
                if (chkk == true)
                {
                    MessageBox.Show("Some of data were not downloaded");
                }
                else
                {
                    MessageBox.Show("data downloaded successfully");
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }
        string branchid = "";
        string q1 = "", q2 = "";
        public void accounts(string date, double amount)
        {
            
            string q = "";

            q = "delete from InventoryAccount where VoucherNo='SJVI-" + branchid + "-" + date + "'";
            objCore.executeQuery(q);
            q = "delete from BranchAccount where VoucherNo='SJVI-" + branchid + "-" + date + "'";
            objCore.executeQuery(q);
            DataSet ds = new DataSet();

            branchid = comboBox1.SelectedValue.ToString();

            inventoryaccount(date,amount);

            employeeaccount(amount.ToString(), "SJVI-" + branchid + "-" + date, "Cash", date);

            ExecuteSqlTransaction("", q1, q2, "");




        }
        private static void ExecuteSqlTransaction(string connectionString, string q1, string q2,  string message)
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
                    if (q1.Length > 0)
                    {
                        command.CommandText = q1;

                        command.ExecuteNonQuery();
                    }
                    if (q2.Length > 0)
                    {
                        command.CommandText = q2;
                        command.ExecuteNonQuery();
                    }
                    
                    // Attempt to commit the transaction.
                    transaction.Commit();
                    //MessageBox.Show(message);
                }
                catch (Exception ex)
                {
                  //  MessageBox.Show("  Message: {0}" + ex.Message);

                    // Attempt to roll back the transaction. 
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception ex2)
                    {
                      //  MessageBox.Show("Rollback Exception Type: {0}" + ex2.Message);
                    }
                }
            }
        }
        public void inventoryaccount(string date,double amount)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";

                {
                    string ChartAccountId = "";

                    double prc = amount;
                   

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
                   
                    q = "insert into InventoryAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance,branchid) values('" + iddd + "','" + date + "','" + ChartAccountId + "','SJVI-" + branchid + "-" + date + "','Inventory Received from head Office','" + Math.Round(Convert.ToDouble(prc), 2) + "','0','0','" + branchid + "')";
                    q1 = q;
                   

                }
            }
            catch (Exception ex)
            {


            }
        }

        public void employeeaccount(string amount, string saleid, string type, string date)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='Head Office Account'  and branchid='" + branchid + "'";
                dsacount = objCore.funGetDataSet(q);                        
                int id = 0;
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from BranchAccount");
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

                    q = "insert into BranchAccount (branchid,Id,Date,CustomersId,PayableAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + branchid + "','" + iddd + "','" + date + "','" + branchid + "','" + ChartAccountId + "','" + saleid + "','Inventory Received from Head Office','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    q2 = q;
                    //objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                button1.Text = "Please Wait";
                button1.Enabled = false;
                downloadbyservice("");
                button1.Enabled = true;
                button1.Text = "Download";
                return;
            }

            button1.Text = "Please Wait";
            button1.Enabled = false;
            bool chkk = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = row.Cells[0] as DataGridViewCheckBoxCell;

                if (Convert.ToBoolean(chk.Value) == true)
                {
                    chkk = true;
                    string id = row.Cells[1].Value.ToString();
                    string q = "";
                    q = "select * from Supplier where id='" + row.Cells["SupplierCode"].Value.ToString() + "'";
                    DataSet ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        DataSet dsgroup = new DataSet();
                        q = "select * from Supplier where id='" + row.Cells["SupplierCode"].Value.ToString() + "'";
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                            connection.Open();
                            com = new SqlCommand(q, connection);
                            SqlDataAdapter da = new SqlDataAdapter(com);
                            da.Fill(dsgroup);
                        }
                        catch (Exception ex)
                        {

                            // MessageBox.Show(ex.Message);
                        }

                        q = "insert into Supplier ( Id, Name, Code, CNICNo, City, Address, Phone,  payableaccountid) values ('" + dsgroup.Tables[0].Rows[0]["Id"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Name"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Code"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["CNICNo"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["City"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Address"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Phone"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["payableaccountid"].ToString() + "')";
                        objCore.executeQuery(q);
                    }
                    DataSet ds1 = new DataSet();
                    ds = new DataSet();
                    q = "select * from Purchase where id='" + row.Cells["id"].Value.ToString() + "'";
                    ds = new DataSet();
                    try
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {

                        // MessageBox.Show(ex.Message);
                    }


                    q = "select * from Purchase where id='" + row.Cells["id"].Value.ToString() + "'";
                    ds1 = new DataSet();
                    ds1 = objCore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        q = "update Purchase set SupplierId='" + ds.Tables[0].Rows[0]["SupplierId"].ToString() + "', TotalAmount='" + ds.Tables[0].Rows[0]["TotalAmount"].ToString() + "',Date='" + ds.Tables[0].Rows[0]["Date"].ToString() + "',InvoiceNo='" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString() + "',BranchCode='" + ds.Tables[0].Rows[0]["BranchCode"].ToString() + "',StoreCode='" + ds.Tables[0].Rows[0]["StoreCode"].ToString() + "' where id='" + ds.Tables[0].Rows[0]["id"].ToString() + "'";
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        q = "insert into Purchase (Id, SupplierId, TotalAmount, Date, BranchCode, StoreCode, InvoiceNo, Status) values ( '" + ds.Tables[0].Rows[0]["Id"].ToString() + "', '" + ds.Tables[0].Rows[0]["SupplierId"].ToString() + "', '" + ds.Tables[0].Rows[0]["TotalAmount"].ToString() + "', '" + ds.Tables[0].Rows[0]["Date"].ToString() + "', '" + ds.Tables[0].Rows[0]["BranchCode"].ToString() + "', '" + ds.Tables[0].Rows[0]["StoreCode"].ToString() + "', '" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString() + "', '" + ds.Tables[0].Rows[0]["status"].ToString() + "')";
                        objCore.executeQuery(q);
                    }

                    DataSet dsflavour = new DataSet();
                    q = "select * from PurchaseDetails where PurchaseId='" + row.Cells["id"].Value.ToString() + "'";
                    try
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(dsflavour);
                    }
                    catch (Exception ex)
                    {

                        // MessageBox.Show(ex.Message);
                    }
                    q = "delete from PurchaseDetails where PurchaseId='" + row.Cells["id"].Value.ToString() + "'";
                    objCore.executeQuery(q);
                    for (int j = 0; j < dsflavour.Tables[0].Rows.Count; j++)
                    {
                        q = "insert into PurchaseDetails ( Id, RawItemId, PurchaseId, Package, PackageItems, TotalItems, Price, PricePerItem, TotalAmount,  branchid, Description) values ( '" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["RawItemId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["PurchaseId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["Package"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["PackageItems"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["TotalItems"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["Price"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["PricePerItem"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["TotalAmount"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["branchid"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["Description"].ToString() + "')";
                        objCore.executeQuery(q);
                    }


                }

            }
            if (chkk == true)
            {
                MessageBox.Show("Data Downloaded successfully");
            }
            button1.Text = "Submit";
            button1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                button1.Text = "Please Wait";
                button1.Enabled = false;
                downloadbyservice("All");
                button1.Enabled = true;
                button1.Text = "Download";
                return;
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    string dat = dr.Cells["date"].Value.ToString();
                    int count = 0;
                    try
                    {
                        count = datat.Where(s => s.Date == dat).ToList().Count;
                    }
                    catch (Exception ex)
                    {


                    }
                    if (count > 0)
                    {
                        dr.DefaultCellStyle.BackColor = Color.Green;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            getmenuservice();
        }

        //private void button2_Click(object sender, EventArgs e)
        //{
        //    bool chkk = false;
        //    foreach (DataGridViewRow row in dataGridView1.Rows)
        //    {
        //        //DataGridViewCheckBoxCell chk = row.Cells[0] as DataGridViewCheckBoxCell;

        //        //if (Convert.ToBoolean(chk.Value) == true)
        //        {
        //            chkk = true;
        //            string id = row.Cells[1].Value.ToString();
        //            string q = "";
        //            q = "select * from menugroup where id='" + row.Cells["menugroupid"].Value.ToString() + "'";
        //            DataSet ds = new DataSet();
        //            ds = objCore.funGetDataSet(q);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {

        //            }
        //            else
        //            {
        //                SqlConnection connection = new SqlConnection(cs);
        //                SqlCommand com;
        //                DataSet dsgroup = new DataSet();
        //                q = "select * from menugroup where id='" + row.Cells["menugroupid"].Value.ToString() + "'";
        //                try
        //                {
        //                    if (connection.State == ConnectionState.Open)
        //                        connection.Close();
        //                    connection.Open();
        //                    com = new SqlCommand(q, connection);
        //                    SqlDataAdapter da = new SqlDataAdapter(com);
        //                    da.Fill(dsgroup);
        //                }
        //                catch (Exception ex)
        //                {

        //                    // MessageBox.Show(ex.Message);
        //                }

        //                q = "insert into menugroup ( Id, Name, ColorId, Description, Status, Image, FontColorId, FontSize, uploadstatus, branchid, type, role) values ('" + dsgroup.Tables[0].Rows[0]["Id"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Name"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["ColorId"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Description"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Status"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Image"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["FontColorId"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["FontSize"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["uploadstatus"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["branchid"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["type"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["role"].ToString() + "')";
        //                objCore.executeQuery(q);
        //            }
        //            q = "select * from menuitem where id='" + row.Cells["id"].Value.ToString() + "'";
        //            ds = new DataSet();
        //            ds = objCore.funGetDataSet(q);
        //            if (ds.Tables[0].Rows.Count > 0)
        //            {
        //                q = "update menuitem set Name='" + row.Cells["name"].Value.ToString() + "', FontSize='" + row.Cells["FontSize"].Value.ToString() + "',MenuGroupId='" + row.Cells["MenuGroupId"].Value.ToString() + "',ColorId='" + row.Cells["ColorId"].Value.ToString() + "',KDSId='" + row.Cells["KDSId"].Value.ToString() + "',FontColorId='" + row.Cells["FontColorId"].Value.ToString() + "', Price='" + row.Cells["name"].Value.ToString() + "', submenugroupid='" + row.Cells["name"].Value + "' where id='" + row.Cells["id"].Value.ToString() + "'";
        //                objCore.executeQuery(q);
        //            }
        //            else
        //            {
        //                q = "insert into menuitem (Id, Code, Name, MenuGroupId, BarCode, Price, Status, ColorId, KDSId,  FontColorId, FontSize, Minutes, alarmtime, minuteskdscolor, alarmkdscolor, submenugroupid) values ( '" + row.Cells["Id"].Value + "', '" + row.Cells["Code"].Value + "', '" + row.Cells["name"].Value + "', '" + row.Cells["MenuGroupId"].Value + "', '" + row.Cells["BarCode"].Value + "', '" + row.Cells["Price"].Value + "', '" + row.Cells["Status"].Value + "', '" + row.Cells["ColorId"].Value + "', '" + row.Cells["KDSId"].Value + "', '" + row.Cells["FontColorId"].Value + "', '" + row.Cells["FontSize"].Value + "', '" + row.Cells["Minutes"].Value + "', '" + row.Cells["alarmtime"].Value + "', '" + row.Cells["minuteskdscolor"].Value + "', '" + row.Cells["alarmkdscolor"].Value + "', '" + row.Cells["submenugroupid"].Value + "')";
        //                objCore.executeQuery(q);
        //            }

        //            DataSet dsflavour = new DataSet();
        //            q = "select * from ModifierFlavour where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
        //            try
        //            {
        //                SqlConnection connection = new SqlConnection(cs);
        //                SqlCommand com;
        //                if (connection.State == ConnectionState.Open)
        //                    connection.Close();
        //                connection.Open();
        //                com = new SqlCommand(q, connection);
        //                SqlDataAdapter da = new SqlDataAdapter(com);
        //                da.Fill(dsflavour);
        //            }
        //            catch (Exception ex)
        //            {

        //                // MessageBox.Show(ex.Message);
        //            }
        //            for (int j = 0; j < dsflavour.Tables[0].Rows.Count; j++)
        //            {
        //                q = "select * from ModifierFlavour where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
        //                ds = new DataSet();
        //                ds = objCore.funGetDataSet(q);
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    q = "update ModifierFlavour set Name='" + dsflavour.Tables[0].Rows[j]["name"].ToString() + "',MenuGroupId='" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "',MenuItemId='" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
        //                    objCore.executeQuery(q);
        //                }
        //                else
        //                {
        //                    q = "insert into ModifierFlavour (Id, MenuGroupId, MenuItemId, name, price, kdsid) values ( '" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["name"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "')";
        //                    objCore.executeQuery(q);
        //                }
        //            }

        //            DataSet dsRuntimeModifier = new DataSet();
        //            q = "select * from RuntimeModifier where menuItemid='" + row.Cells["id"].Value.ToString() + "'";
        //            try
        //            {
        //                SqlConnection connection = new SqlConnection(cs);
        //                SqlCommand com;
        //                if (connection.State == ConnectionState.Open)
        //                    connection.Close();
        //                connection.Open();
        //                com = new SqlCommand(q, connection);
        //                SqlDataAdapter da = new SqlDataAdapter(com);
        //                da.Fill(dsRuntimeModifier);
        //            }
        //            catch (Exception ex)
        //            {

        //                // MessageBox.Show(ex.Message);
        //            }
        //            for (int j = 0; j < dsRuntimeModifier.Tables[0].Rows.Count; j++)
        //            {
        //                q = "select * from RuntimeModifier where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
        //                ds = new DataSet();
        //                ds = objCore.funGetDataSet(q);
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    q = "update RuntimeModifier set type='" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "',Name='" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString() + "',Quantity='" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "',status='" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "',rawitemid='" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "',MenuItemId='" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
        //                    objCore.executeQuery(q);
        //                }
        //                else
        //                {
        //                    q = "insert into RuntimeModifier (type,id, name, menuItemid, price, Quantity, status,  kdsid,  rawitemid) values ( '" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "')";
        //                    objCore.executeQuery(q);
        //                }
        //            }

        //            DataSet dsreceipi = new DataSet();
        //            q = "select * from Recipe where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
        //            try
        //            {
        //                SqlConnection connection = new SqlConnection(cs);
        //                SqlCommand com;
        //                if (connection.State == ConnectionState.Open)
        //                    connection.Close();
        //                connection.Open();
        //                com = new SqlCommand(q, connection);
        //                SqlDataAdapter da = new SqlDataAdapter(com);
        //                da.Fill(dsreceipi);
        //            }
        //            catch (Exception ex)
        //            {

        //                // MessageBox.Show(ex.Message);
        //            }

        //            for (int j = 0; j < dsreceipi.Tables[0].Rows.Count; j++)
        //            {


        //                try
        //                {
        //                    DataSet dsrraw = new DataSet();
        //                    q = "select * from RawItem where id='" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
        //                    try
        //                    {
        //                        SqlConnection connection = new SqlConnection(cs);
        //                        SqlCommand com;
        //                        if (connection.State == ConnectionState.Open)
        //                            connection.Close();
        //                        connection.Open();
        //                        com = new SqlCommand(q, connection);
        //                        SqlDataAdapter da = new SqlDataAdapter(com);
        //                        da.Fill(dsrraw);
        //                    }
        //                    catch (Exception ex)
        //                    {

        //                        // MessageBox.Show(ex.Message);
        //                    }
        //                    for (int k = 0; k < dsrraw.Tables[0].Rows.Count; k++)
        //                    {
        //                        q = "select * from RawItem where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
        //                        ds = new DataSet();
        //                        ds = objCore.funGetDataSet(q);
        //                        if (ds.Tables[0].Rows.Count > 0)
        //                        {
        //                            q = "update RawItem set itemname='" + dsrraw.Tables[0].Rows[k]["itemname"].ToString() + "',GroupId='" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "',UOMId='" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "',price='" + dsrraw.Tables[0].Rows[k]["price"].ToString() + "' where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
        //                            objCore.executeQuery(q);
        //                        }
        //                        else
        //                        {
        //                            q = "insert into RawItem (Id, GroupId, CategoryId, TypeId, BrandId, UOMId, SizeId, ColorId,      ItemName, BarCode, Code, Price,    MinOrder) values ('" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["CategoryId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["TypeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BrandId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["SizeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ColorId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ItemName"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BarCode"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["Code"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["Price"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["MinOrder"].ToString() + "')";
        //                            objCore.executeQuery(q);
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {


        //                }


        //                q = "select * from Recipe where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
        //                ds = new DataSet();
        //                ds = objCore.funGetDataSet(q);
        //                if (ds.Tables[0].Rows.Count > 0)
        //                {
        //                    q = "update Recipe set type='" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "',MenuItemId='" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "',Quantity='" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "',modifierid='" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "', RawItemId='" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', UOMCId='" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "' where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
        //                    objCore.executeQuery(q);
        //                }
        //                else
        //                {
        //                    q = "insert into Recipe (type,Id, MenuItemId, RawItemId, UOMCId, Quantity,  modifierid) values ( '" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "')";
        //                    objCore.executeQuery(q);
        //                }
        //            }
        //        }

        //    }
        //    if (chkk == true)
        //    {
        //        MessageBox.Show("Data Downloaded successfully");
        //    }
        //}
    }
}
