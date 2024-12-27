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
namespace POSRestaurant.Accounts
{
    public partial class DownloadAccounts : Form
    {
        public DownloadAccounts()
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

                    }
                    else
                    {
                       
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
            dtpurchase.Columns.Add("Id", typeof(string));
            dtpurchase.Columns.Add("AccountType", typeof(string));
            dtpurchase.Columns.Add("Name", typeof(string));
            dtpurchase.Columns.Add("AccountCode", typeof(string));
            dtpurchase.Columns.Add("Description", typeof(string));
            dtpurchase.Columns.Add("Status", typeof(string));
            dtpurchase.Columns.Add("Branchid", typeof(string));
            type();
            getmenuservice();

           
        }
        public DataTable dtpurchase = new DataTable();
        protected void getmenuservice()
        {
            try
            {
                dtpurchase.Rows.Clear();
                dtpurchase.AcceptChanges();
                string uri = url + "/accountsdownload.asmx/Getresponse?branchid=" + comboBox1.SelectedValue.ToString();
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<accountsdownloadcls> res = (List<accountsdownloadcls>)JsonConvert.DeserializeObject(result, typeof(List<accountsdownloadcls>));
                    foreach (var item in res)
                    {
                        if (item.Id != "")
                        {

                            dtpurchase.Rows.Add(item.Id, item.AccountType, item.Name, item.AccountCode, item.Description, item.Status, item.Branchid);

                        }
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
        protected void downloadbyservice(string type)
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
                    bool val = false;
                    try
                    {
                        val = Convert.ToBoolean(chkchecking.Value);
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                    
                    if (type == "All")
                    {
                        q = "select * from ChartofAccounts where id='" + row.Cells["Id"].Value.ToString() + "'";
                    }
                    else if (val == true)
                    {
                        q = "select * from ChartofAccounts where id='" + row.Cells["Id"].Value.ToString() + "'";
                    }

                    try
                    {
                        if (q.Length > 0)
                        {
                            DataSet ds = new DataSet();
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                q = "update ChartofAccounts set AccountType='" + row.Cells["AccountType"].Value.ToString() + "', Name='" + row.Cells["Name"].Value.ToString() + "', AccountCode='" + row.Cells["AccountCode"].Value.ToString() + "', Description='" + row.Cells["Description"].Value.ToString() + "', Status='" + row.Cells["Status"].Value.ToString() + "',   Branchid='" + row.Cells["Branchid"].Value.ToString() + "' where id='" + row.Cells["Id"].Value.ToString() + "'";
                                int res = objCore.executeQueryint(q);
                                if (chkk == false && res == 0)
                                {
                                    chkk = true;
                                }
                            }
                            else
                            {
                                q = "insert into ChartofAccounts (Id, AccountType, Name, AccountCode, Description, Status, Branchid) values('" + row.Cells["Id"].Value.ToString() + "','" + row.Cells["AccountType"].Value.ToString() + "', '" + row.Cells["Name"].Value.ToString() + "','" + row.Cells["AccountCode"].Value.ToString() + "','" + row.Cells["Description"].Value.ToString() + "','" + row.Cells["Status"].Value.ToString() + "','" + row.Cells["Branchid"].Value.ToString() + "')";
                                int res = objCore.executeQueryint(q);
                                if (chkk == false && res == 0)
                                {
                                    chkk = true;
                                }
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                       
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
                downloadbyservice("");
                button1.Enabled = true;
                button1.Text = "Download";
                return;
            }

           
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            getmenuservice();
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

       
    }
}
