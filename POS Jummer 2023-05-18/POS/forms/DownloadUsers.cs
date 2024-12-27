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
    public partial class DownloadUsers : Form
    {
        public DownloadUsers()
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
                string q = "select * from deliverytransfer where server='main' ";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
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
            DataSet ds = new DataSet();
            string q = "select id,branchname from branch";
            ds = objCore.funGetDataSet(q);
            try
            {

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "branchname";
               

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
            dtpurchase.Columns.Add("Id", typeof(string));
            dtpurchase.Columns.Add("Name", typeof(string));
            dtpurchase.Columns.Add("Usertype", typeof(string));
            dtpurchase.Columns.Add("Username", typeof(string));
            //try
            //{
            //    DataSet ds = new System.Data.DataSet();
            //    ds = objCore.funGetDataSet("select * from SqlServerInfo where type='server'");
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        string server = ds.Tables[0].Rows[0]["ServerName"].ToString();
            //        string db = ds.Tables[0].Rows[0]["DbName"].ToString();
            //        string user = ds.Tables[0].Rows[0]["UserName"].ToString();
            //        string password = ds.Tables[0].Rows[0]["Password"].ToString();
            //        cs = "Data Source=" + server + ";Initial Catalog=" + db + ";Persist Security Info=True;User ID=" + user + ";Password=" + password + "";
            //    }
            //}
            //catch (Exception ex)
            //{


            //}
            type();
            getmenuservice();

            //try
            //{
            //    DataSet ds = new DataSet();


            //    SqlConnection connection = new SqlConnection(cs);
            //    SqlCommand com;


            //    string q = "select id,branchname from branch";
            //    ds = objCore.funGetDataSet(q);
            //    try
            //    {
                    
            //        comboBox1.DataSource = ds.Tables[0];
            //        comboBox1.ValueMember = "id";
            //        comboBox1.DisplayMember = "branchname";
            //        if (type() == "service")
            //        {
                        
            //        }
            //        else
            //        {
            //            getmenu();
            //        }

            //    }
            //    catch (Exception ex)
            //    {

            //        // MessageBox.Show(ex.Message);
            //    }


            //}
            //catch (Exception ex)
            //{


            //}
        }
        public DataTable dtpurchase = new DataTable();
        List<usersclass> resUsers = new List<usersclass>();
        protected void getmenuservice()
        {
            try
            {
                dtpurchase.Rows.Clear();
                dtpurchase.AcceptChanges();
                string uri = url + "/DeliveryServices/downloadusers.asmx/Getresponse?branchid="+comboBox1.SelectedValue;
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    resUsers = (List<usersclass>)JsonConvert.DeserializeObject(result, typeof(List<usersclass>));
                    foreach (var item in resUsers)
                    {

                        dtpurchase.Rows.Add(item.Id, item.Name, item.Usertype, item.UserName);


                    }
                    dataGridView1.DataSource = dtpurchase;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
      
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        protected void downloadbyservice()
        {
            try
            {
                button1.Text = "Please Wait";
                button1.Enabled = false;
                bool chkk = false;
               
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    DataGridViewCheckBoxCell chkchecking = row.Cells[0] as DataGridViewCheckBoxCell;
                    string q = "";
                    if (Convert.ToBoolean(chkchecking.Value) == true)
                    {

                        List<usersclass> resfiltered = new List<usersclass>();
                        resfiltered = resUsers.Where(x => x.Id.ToString() == row.Cells["Id"].Value.ToString()).ToList();
                            q = "select * from users where id='" + row.Cells["Id"].Value.ToString() + "'";
                            DataSet ds = new DataSet();
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                q = "update users set  Name='" + resfiltered[0].Name + "', Usertype='" + resfiltered[0].Usertype + "', CardNo='" + resfiltered[0].CardNo + "', UserName='" + resfiltered[0].UserName + "', Password='" + resfiltered[0].Password + "'," +
                                    "branchid='" + resfiltered[0].branchid + "',  kdsid='" + resfiltered[0].kdsid + "', terminal='" + resfiltered[0].terminal + "', role='" + resfiltered[0].role + "',  discountlimit='" + resfiltered[0].discountlimit + "', KDSType='" + resfiltered[0].KDSType + "' where id='" + row.Cells["Id"].Value.ToString() + "'";
                                
                            }
                            else
                            {
                                q = "insert into users (Id, Name, Usertype, CardNo, UserName, Password,  branchid,  kdsid, terminal, role,  discountlimit, KDSType) values" +
                                    "( '" + resfiltered[0].Id + "', '" + resfiltered[0].Name + "', '" + resfiltered[0].Usertype + "', '" + resfiltered[0].CardNo + "', '" + resfiltered[0].UserName + "', '" + resfiltered[0].Password + "',"+
                                    "'" + resfiltered[0].branchid + "', '" + resfiltered[0].kdsid + "', '" + resfiltered[0].terminal + "', '" + resfiltered[0].role + "', '" + resfiltered[0].discountlimit + "', '" + resfiltered[0].KDSType + "')";
                                
                            }
                          int res=  objCore.executeQueryint(q);
                          if (res == 0)
                          {
                              chkk = true;
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
        private void button1_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                button1.Text = "Please Wait";
                button1.Enabled = false;
                downloadbyservice();
                button1.Enabled = true;
                button1.Text = "Download";
                return;
            }

           
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            getmenuservice();
        }

       
    }
}
