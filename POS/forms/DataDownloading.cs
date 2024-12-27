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
    public partial class DataDownloading : Form
    {
        BackgroundWorker bg2 = new BackgroundWorker();
        POSRestaurant.forms.BackendForm _frm;
        bool chk = false;
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        string cs = "";
        public DataDownloading(POSRestaurant.forms.BackendForm frm)
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

                SqlConnection connection = new SqlConnection(cs);
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {

                }
                for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                {
                    branchcode = ds.Tables[0].Rows[k]["id"].ToString();
                    SqlCommand com;
                    string qry = "";
                    // cs = "Persist Security Info=True;Integrated Security = true; User ID=DB_99F0DF_ideamedia_admin;Password=ideamobile Initial Catalog=LBGPOS;Data Source=SQL5009.Smarterasp.net";
                    DataSet dsinventry = new DataSet();
                    string q = "select * from Inventory where UploadStatus='Pending' and branchid='"+ branchcode +"'";
                    try
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(dsinventry);
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                    q = "select * from InventoryConsumed where UploadStatus='Pending' and branchid='" + ds.Tables[0].Rows[k]["id"].ToString() + "'";
                    DataSet dsinvconsumed = new DataSet();
                    try
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(dsinvconsumed);
                    }
                    catch (Exception ex)
                    {


                    }
                    DataSet dsgetinfi = new System.Data.DataSet();
                    DataSet dsdetails = new System.Data.DataSet();
                    q = "select * from sale where UploadStatus='Pending' and branchid='" + ds.Tables[0].Rows[k]["id"].ToString() + "'";
                    //dsgetinfi = objcore.funGetDataSet();
                    try
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(dsgetinfi);
                    }
                    catch (Exception ex)
                    {


                    }
                    if (dsgetinfi.Tables[0].Rows.Count > 0)
                    {
                        //qry = "sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'";
                        //if (connection.State == ConnectionState.Open)
                        //{
                        //    connection.Close();
                        //}
                        //connection.Open();
                        //com = new SqlCommand(qry, connection);
                        //com.ExecuteNonQuery();

                        for (int i = 0; i < dsgetinfi.Tables[0].Rows.Count; i++)
                        {
                           
                            qry = "delete from sale where id='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            objcore.executeQuery(q);
                            
                            qry = "delete from saledetails where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                            objcore.executeQuery(q);
                            try
                            {
                                
                                qry = "insert into sale (id,date,time,UserId,TotalBill,Discount,DiscountAmount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,branchid,GSTPerc,OrderStatus) values ('" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Date"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["time"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["UserId"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["TotalBill"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Discount"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["DiscountAmount"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["NetBill"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["BillType"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["OrderType"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["GST"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["BillStatus"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Terminal"].ToString().Replace("'", "''") + "','" + branchcode.Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["GSTPerc"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["OrderStatus"].ToString().Replace("'", "''") + "')";
                                objcore.executeQuery(q);
                                dsdetails = new System.Data.DataSet();
                                q = "select * from saledetails where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                                //dsdetails = objcore.funGetDataSet(q);
                                try
                                {
                                   
                                    if (connection.State == ConnectionState.Open)
                                        connection.Close();
                                    connection.Open();
                                    com = new SqlCommand(q, connection);
                                    SqlDataAdapter da = new SqlDataAdapter(com);
                                    da.Fill(dsdetails);
                                }
                                catch (Exception ex)
                                {


                                }
                                if (dsdetails.Tables[0].Rows.Count > 0)
                                {
                                    for (int j = 0; j < dsdetails.Tables[0].Rows.Count; j++)
                                    {
                                        try
                                        {
                                            qry = "insert into saledetails ( Id, saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus,Itemdiscount, ItemdiscountPerc, ItemGst, ItemGstPerc,branchid) values ('" + dsdetails.Tables[0].Rows[j]["id"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["saleid"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["MenuItemId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Flavourid"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ModifierId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["RunTimeModifierId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Price"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["BarnchCode"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Status"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["comments"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Orderstatus"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Itemdiscount"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ItemdiscountPerc"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ItemGst"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ItemGstPerc"].ToString() + "','" + branchcode.Replace("'", "''") + "')";
                                            objcore.executeQuery(qry);
                                        }
                                        catch (Exception ex)
                                        {
                                            chk = true;
                                        }
                                    }
                                    q = "update sale set uploadstatus='Uploaded' where id='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + branchcode + "'";
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                    connection.Open();
                                    com = new SqlCommand(q, connection);
                                    com.ExecuteNonQuery();                                    
                                    int percentage = (i + 1) * 100 / dsgetinfi.Tables[0].Rows.Count; ;
                                    bg2.ReportProgress(percentage);
                                }

                            }
                            catch (Exception ex)
                            {
                                chk = true;

                            }
                        }
                        qry = "";
                        if (dsinventry.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsinventry.Tables[0].Rows.Count; j++)
                            {
                                try
                                {
                                   
                                    qry = "delete from Inventory where RawItemId='" + dsinventry.Tables[0].Rows[j]["RawItemId"].ToString() + "' and branchid='" + branchcode + "'";
                                    objcore.executeQuery(qry);
                                    qry = "insert into Inventory (Id, RawItemId, Quantity, branchid) values ('" + dsinventry.Tables[0].Rows[j]["id"].ToString() + "','" + dsinventry.Tables[0].Rows[j]["RawItemId"].ToString() + "','" + dsinventry.Tables[0].Rows[j]["Quantity"].ToString() + "','" + branchcode + "')";
                                    objcore.executeQuery(qry);
                                    q = "update Inventory set uploadstatus='Uploaded' where id='" + dsinventry.Tables[0].Rows[j]["id"].ToString() + "' and branchid='" + branchcode + "'";
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                    connection.Open();
                                    com = new SqlCommand(q, connection);
                                    com.ExecuteNonQuery();                                    
                                }
                                catch (Exception ex)
                                {

                                    chk = true;
                                }
                            }


                        }
                        if (dsinvconsumed.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsinvconsumed.Tables[0].Rows.Count; j++)
                            {
                                try
                                {
                                    
                                    qry = "delete from InventoryConsumed where id='" + dsinvconsumed.Tables[0].Rows[j]["id"].ToString() + "' and branchid='" + branchcode + "'";
                                    objcore.executeQuery(qry);
                                    qry = "insert into InventoryConsumed (Id, RawItemId, QuantityConsumed, RemainingQuantity, Date, branchid) values ('" + dsinvconsumed.Tables[0].Rows[j]["id"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["RawItemId"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["QuantityConsumed"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["RemainingQuantity"].ToString() + "','" + dsinvconsumed.Tables[0].Rows[j]["Date"].ToString() + "','" + branchcode + "')";
                                    objcore.executeQuery(qry);
                                    q = "update InventoryConsumed set uploadstatus='Uploaded' where id='" + dsinvconsumed.Tables[0].Rows[j]["id"].ToString() + "' and branchid='" + branchcode + "'";
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Close();
                                    }
                                    connection.Open();
                                    com = new SqlCommand(q, connection);
                                    com.ExecuteNonQuery();                                    
                                }
                                catch (Exception ex)
                                {
                                    chk = true;
                                }
                            }


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
