using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRetail.forms
{
    public partial class DataUploading : Form
    {
        BackgroundWorker bg2 = new BackgroundWorker();
        POSRetail.forms.BackendForm _frm;
        bool chk = false;
        POSRetail.classes.Clsdbcon objcore = new classes.Clsdbcon();
        string cs = "";
        public DataUploading(POSRetail.forms.BackendForm frm)
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
                ds = objcore.funGetDataSet("select * from Branch");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchcode = ds.Tables[0].Rows[0]["BranchName"].ToString();
                }

                SqlConnection connection = new SqlConnection(cs);
                try
                {
                    connection.Open();
                }
                catch (Exception ex)
                {


                }
                SqlCommand com;
                // cs = "Persist Security Info=True;Integrated Security = true; User ID=DB_99F0DF_ideamedia_admin;Password=ideamobile Initial Catalog=LBGPOS;Data Source=SQL5009.Smarterasp.net";
                DataSet dsgetinfi = new System.Data.DataSet();
                DataSet dsdetails = new System.Data.DataSet();
                dsgetinfi = objcore.funGetDataSet("select * from sale where UploadStatus='Pending'");
                if (dsgetinfi.Tables[0].Rows.Count > 0)
                {
                    //vProgressBar1.Minimum = 1;
                    //vProgressBar1.Maximum = dsgetinfi.Tables[0].Rows.Count;
                    //vProgressBar1.Step = 1;
                    for (int i = 0; i < dsgetinfi.Tables[0].Rows.Count; i++)
                    {
                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        string qry = "delete from sale where id='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchCode='" + branchcode + "'";
                        com = new SqlCommand(qry, connection);
                        com.ExecuteNonQuery();

                        if (connection.State == ConnectionState.Open)
                        {
                            connection.Close();
                        }
                        connection.Open();
                        qry = "delete from saledetails where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "' and branchCode='" + branchcode + "'";
                        com = new SqlCommand(qry, connection);
                        com.ExecuteNonQuery();

                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            qry = "insert into sale (id,date,time,UserId,TotalBill,Discount,DiscountAmount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,BranchCode) values ('" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Date"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["time"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["UserId"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["TotalBill"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Discount"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["DiscountAmount"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["NetBill"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["BillType"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["OrderType"].ToString().Replace("'", "''") + "','" + dsgetinfi.Tables[0].Rows[i]["GST"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["BillStatus"].ToString() + "','" + dsgetinfi.Tables[0].Rows[i]["Terminal"].ToString().Replace("'", "''") + "','" + branchcode.Replace("'", "''") + "')";
                            com = new SqlCommand(qry, connection);
                            com.ExecuteNonQuery();
                            dsdetails = new System.Data.DataSet();
                            string q = "select * from saledetails where saleid='" + dsgetinfi.Tables[0].Rows[i]["id"].ToString() + "'";
                            dsdetails = objcore.funGetDataSet(q);
                            if (dsdetails.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsdetails.Tables[0].Rows.Count; j++)
                                {
                                    try
                                    {
                                        qry = "insert into saledetails (id,saleid,MenuItemId,ModifierId,Quantity,Price,status,BranchCode) values ('" + dsdetails.Tables[0].Rows[j]["id"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["saleid"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["MenuItemId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["ModifierId"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["Price"].ToString() + "','" + dsdetails.Tables[0].Rows[j]["status"].ToString() + "','" + branchcode.Replace("'", "''") + "')";
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
                                
                                int percentage = (i + 1) * 100 / dsgetinfi.Tables[0].Rows.Count; ;
                                bg2.ReportProgress(percentage);
                            }
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
                DialogResult rs = MessageBox.Show("Some of Sales were not uploaded.", "", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                if (rs == DialogResult.Retry)
                {


                    bg2.RunWorkerAsync();
                }
                if (rs == DialogResult.Cancel)
                {
                    _frm.Enabled = true;
                    this.Close();
                }
            }
            else
            {
                _frm.Enabled = true;
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

            upload();
          
        }
    }
}
