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
    public partial class DataUploading : Form
    {
        BackgroundWorker bg2 = new BackgroundWorker();
        POSRestaurant.forms.BackendForm _frm;
        bool chk = false;
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        string cs = "";
        public DataUploading(POSRestaurant.forms.BackendForm frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void DataUploading_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new System.Data.DataSet();
                ds = objcore.funGetDataSet("select * from SqlServerInfo  where type='server'");
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
        }
        private void myBGWorker_DoWork(object sender, DoWorkEventArgs e)
        {

            try
            {
                string branchcode = "";
                DataSet ds = new System.Data.DataSet();
                ds = objcore.funGetDataSet("select * from Branch order by id desc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchcode = ds.Tables[0].Rows[0]["id"].ToString();
                }
                int count = 0;
                int percntcount = 0;
                SqlConnection connection = new SqlConnection(cs);
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {


                }
                SqlCommand com;
                string qry = "";
                // cs = "Persist Security Info=True;Integrated Security = true; User ID=DB_99F0DF_ideamedia_admin;Password=ideamobile Initial Catalog=LBGPOS;Data Source=SQL5009.Smarterasp.net";
                DataSet dsinventry = new DataSet();
                string q = "";
                //dsinventry = objcore.funGetDataSet(q);
                q = "select * from InventoryConsumed where UploadStatus='Pending'";
                DataSet dsinvconsumed = new DataSet();
                dsinvconsumed = objcore.funGetDataSet(q);

                if (dsinvconsumed.Tables[0].Rows.Count > 0)
                {
                    count = count + dsinvconsumed.Tables[0].Rows.Count;
                }

                q = "select * from discard where UploadStatus='Pending'";
                DataSet dsdiscard = new DataSet();
                dsdiscard = objcore.funGetDataSet(q);
                if (dsdiscard.Tables[0].Rows.Count > 0)
                {
                    count = count + dsdiscard.Tables[0].Rows.Count;
                }

                q = "select * from purchase where UploadStatus='Pending'";
                DataSet dspurchase = new DataSet();
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    count = count + dspurchase.Tables[0].Rows.Count;
                }


                DataSet dsgetinfi = new System.Data.DataSet();
                DataSet dsdetails = new System.Data.DataSet();
                dsgetinfi = objcore.funGetDataSet("select * from sale where UploadStatus='Pending'");
                if (dsgetinfi.Tables[0].Rows.Count > 0)
                {

                    count = count + dsgetinfi.Tables[0].Rows.Count;
                    qry = "sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    com = new SqlCommand(qry, connection);
                    com.ExecuteNonQuery();

                    for (int i = 0; i < dsgetinfi.Tables[0].Rows.Count; i++)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        qry = "delete from saledetails where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                        com = new SqlCommand(qry, connection);
                        com.ExecuteNonQuery();

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        qry = "delete from BillType where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                        com = new SqlCommand(qry, connection);
                        com.ExecuteNonQuery();



                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        qry = "delete from sale where id='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                        com = new SqlCommand(qry, connection);
                        com.ExecuteNonQuery();

                        

                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "insert into sale (onlineid,date,time,UserId,TotalBill,Discount,DiscountAmount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,branchid,GSTPerc,OrderStatus) values ('" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Date"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["time"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["UserId"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["TotalBill"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Discount"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["DiscountAmount"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["NetBill"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["BillType"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["OrderType"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["GST"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["BillStatus"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Terminal"].ToString().Replace("'", "''") + "','" + branchcode.Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["GSTPerc"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["OrderStatus"].ToString().Replace("'", "''") + "')";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();

                            DataSet dsbilltype = new DataSet();
                            dsbilltype = new System.Data.DataSet();
                            q = "select * from BillType where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "'";
                            dsbilltype = objcore.funGetDataSet(q);
                            if (dsbilltype.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsbilltype.Tables[0].Rows.Count; j++)
                                {
                                    try
                                    {
                                        if (connection.State == ConnectionState.Open)
                                        {
                                            connection.Close();
                                        }
                                        connection.Open();
                                        qry = "insert into BillType (id, type, saleid, Amount,recvid,branchid) values ('" + dsbilltype.Tables[0].Rows[j]["id"].ToString() + "','" + dsbilltype.Tables[0].Rows[j]["type"].ToString() + "','" + dsbilltype.Tables[0].Rows[j]["saleid"].ToString() + "','" + dsbilltype.Tables[0].Rows[j]["Amount"].ToString() + "','" + dsbilltype.Tables[0].Rows[j]["recvid"].ToString() + "','" + branchcode.Replace("'", "''") + "')";
                                        com = new SqlCommand(qry, connection);
                                        com.ExecuteNonQuery();

                                    }
                                    catch (Exception ex)
                                    {

                                        chk = true;
                                    }
                                }

                            }

                            dsdetails = new System.Data.DataSet();
                            q = "select * from saledetails where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "'";
                            dsdetails = objcore.funGetDataSet(q);
                            if (dsdetails.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsdetails.Tables[0].Rows.Count; j++)
                                {
                                    try
                                    {
                                        qry = "insert into saledetails ( Id, saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus,Itemdiscount, ItemdiscountPerc, ItemGst, ItemGstPerc,branchid) values ('" + dsdetails.Tables[0].Rows[j]["id"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["saleid"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["MenuItemId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Flavourid"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ModifierId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["RunTimeModifierId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Price"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["BarnchCode"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Status"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["comments"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Orderstatus"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Itemdiscount"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ItemdiscountPerc"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ItemGst"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ItemGstPerc"].ToString() + "','" + branchcode.Replace("'", "''") + "')";
                                        if (connection.State == ConnectionState.Open)
                                        {
                                            connection.Close();
                                        }
                                        connection.Open();
                                        com = new SqlCommand(qry, connection);
                                        com.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {

                                        chk = true;
                                    }
                                }
                                objcore.executeQuery("update sale set uploadstatus='Uploaded' where id='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "'");


                                //int percentage = (i + 1) * 100 / dsgetinfi.Tables[0].Rows.Count; ;
                                //bg2.ReportProgress(percentage);
                            }

                        }
                        catch (Exception ex)
                        {
                            chk = true;

                        }

                        percntcount = percntcount + 1;
                        int percentage = (percntcount) * 100 / count;
                        bg2.ReportProgress(percentage);


                    }
                }

                if (dspurchase.Tables[0].Rows.Count > 0)
                {                    
                    qry = "sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    com = new SqlCommand(qry, connection);
                    com.ExecuteNonQuery();

                    for (int i = 0; i < dspurchase.Tables[0].Rows.Count; i++)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        qry = "delete from PurchaseDetails where PurchaseId='" + dspurchase.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                        com = new SqlCommand(qry, connection);
                        com.ExecuteNonQuery();
                       
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        qry = "delete from Purchase where id='" + dspurchase.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                        com = new SqlCommand(qry, connection);
                        com.ExecuteNonQuery();



                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "insert into Purchase ( Id, SupplierId, TotalAmount, Date, BranchCode, StoreCode, InvoiceNo, Status, branchid) values ('" + dspurchase.Tables[0].Rows[i]["id"].ToString() + "','" + dspurchase.Tables[0].Rows[i]["SupplierId"].ToString() + "','" + dspurchase.Tables[0].Rows[i]["TotalAmount"].ToString() + "','" + dspurchase.Tables[0].Rows[i]["Date"].ToString() + "','" + dspurchase.Tables[0].Rows[i]["BranchCode"].ToString() + "','" + dspurchase.Tables[0].Rows[i]["StoreCode"].ToString() + "','" + dspurchase.Tables[0].Rows[i]["InvoiceNo"].ToString() + "','" + dspurchase.Tables[0].Rows[i]["Status"].ToString() + "','" + branchcode.Replace("'", "''") + "')";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();

                            

                           DataSet  dspurchasedetails = new System.Data.DataSet();
                           q = "select * from PurchaseDetails where PurchaseId='" + dspurchase.Tables[0].Rows[i]["id"].ToString() + "'";
                            dspurchasedetails = objcore.funGetDataSet(q);
                            if (dspurchasedetails.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dspurchasedetails.Tables[0].Rows.Count; j++)
                                {
                                    try
                                    {
                                        qry = "insert into PurchaseDetails ( Id, RawItemId, PurchaseId, Package, PackageItems, TotalItems, Price, PricePerItem, TotalAmount, Description,branchid) values ('" + dspurchasedetails.Tables[0].Rows[j]["id"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["RawItemId"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["PurchaseId"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["Package"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["PackageItems"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["TotalItems"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["Price"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["PricePerItem"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["TotalAmount"].ToString() + "','" + dspurchasedetails.Tables[0].Rows[j]["Description"].ToString() + "','" + branchcode.Replace("'", "''") + "')";
                                        if (connection.State == ConnectionState.Open)
                                        {
                                            connection.Close();
                                        }
                                        connection.Open();
                                        com = new SqlCommand(qry, connection);
                                        com.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {

                                        chk = true;
                                    }
                                }
                                objcore.executeQuery("update Purchase set uploadstatus='Uploaded' where id='" + dspurchase.Tables[0].Rows[i]["id"].ToString() + "'");


                                //int percentage = (i + 1) * 100 / dsgetinfi.Tables[0].Rows.Count; ;
                                //bg2.ReportProgress(percentage);
                            }

                        }
                        catch (Exception ex)
                        {
                            chk = true;

                        }

                        percntcount = percntcount + 1;
                        int percentage = (percntcount) * 100 / count;
                        bg2.ReportProgress(percentage);


                    }
                }
                qry = "";
                
                if (dsinvconsumed.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsinvconsumed.Tables[0].Rows.Count; j++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from InventoryConsumed where id='" + dsinvconsumed.Tables[0].Rows[j]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into InventoryConsumed (Id, RawItemId, QuantityConsumed, RemainingQuantity, Date, branchid) values ('" + dsinvconsumed.Tables[0].Rows[j]["id"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["RawItemId"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["QuantityConsumed"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["RemainingQuantity"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["Date"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update InventoryConsumed set uploadstatus='Uploaded' where id='" + dsinvconsumed.Tables[0].Rows[j]["id"].ToString() + "'");
                        }
                        catch (Exception ex)
                        {
                            chk = true;
                        }

                        percntcount = percntcount + 1;
                        int percentage = (percntcount) * 100 / count;
                        bg2.ReportProgress(percentage);

                    }
                }

                if (dsdiscard.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsdiscard.Tables[0].Rows.Count; j++)
                    {
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "delete from Discard where id='" + dsdiscard.Tables[0].Rows[j]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            qry = "insert into Discard (id, itemid, date, quantity, Discard, Remaining, staff, completewaste) values ('" + dsdiscard.Tables[0].Rows[j]["id"].ToString() + "','" + dsdiscard.Tables[0].Rows[j]["itemid"].ToString() + "','" + dsdiscard.Tables[0].Rows[j]["date"].ToString() + "','" + dsdiscard.Tables[0].Rows[j]["quantity"].ToString() + "','" + dsdiscard.Tables[0].Rows[j]["Discard"].ToString() + "','" + dsdiscard.Tables[0].Rows[j]["Remaining"].ToString() + "','" + dsdiscard.Tables[0].Rows[j]["staff"].ToString() + "','" + dsdiscard.Tables[0].Rows[j]["completewaste"].ToString() + "','" + branchcode + "')";
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            objcore.executeQuery("update Discard set uploadstatus='Uploaded' where id='" + dsdiscard.Tables[0].Rows[j]["id"].ToString() + "'");
                        }
                        catch (Exception ex)
                        {
                            chk = true;
                        }

                        percntcount = percntcount + 1;
                        int percentage = (percntcount) * 100 / count;
                        bg2.ReportProgress(percentage);

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
                DialogResult rs = MessageBox.Show("Some of Sales were not uploaded.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (rs == DialogResult.Retry)
                {


                    bg2.RunWorkerAsync();
                }
                if (rs == DialogResult.Cancel)
                {
                    _frm.Enabled = true;
                    vButton2.Enabled = false;
                    vButton3.Enabled = false;
                    this.Close();
                }
            }
            else
            {
                _frm.Enabled = true;
                vButton2.Enabled = false;
                vButton3.Enabled = false;
                this.Close();
            }
        }
        void bg2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
           vProgressBar1.Value = e.ProgressPercentage;
        }
        public void upload()
        {

            bool verifyconection = true;
            SqlConnection connection = new SqlConnection(cs);
            try
            {
               
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

        private void vButton3_Click(object sender, EventArgs e)
        {
            _frm.Enabled = true;
            this.Close();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            vButton2.Enabled = false;
            vButton3.Enabled = false;
            upload();
        }
    }
}
