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
    public partial class DownloadDiscounts : Form
    {
        public DownloadDiscounts()
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
            dtpurchase.Columns.Add("Id", typeof(string));
            dtpurchase.Columns.Add("Name", typeof(string));
            dtpurchase.Columns.Add("Discount", typeof(string));
            dtpurchase.Columns.Add("Status", typeof(string));
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
        protected void getmenuservice()
        {
            try
            {
                dtpurchase.Rows.Clear();
                dtpurchase.AcceptChanges();
                string uri = url + "/DeliveryServices/downloaddiscounts.asmx/Getresponse?id=" + comboBox1.Text.ToString();
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<discountcls> res = (List<discountcls>)JsonConvert.DeserializeObject(result, typeof(List<discountcls>));
                    foreach (var item in res)
                    {
                        if (item.id != "")
                        {

                            dtpurchase.Rows.Add(item.id, item.name,item.discount,item.Status);

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
                        if (comboBox1.Text == "Discounts")
                        {
                            q = "delete from DiscountKeys where id='" + row.Cells["Id"].Value.ToString() + "'";
                            objCore.executeQuery(q);
                            q = "insert into DiscountKeys (id, name, discount, status) values ( '" + row.Cells["Id"].Value.ToString() + "', '" + row.Cells["name"].Value.ToString() + "', '" + row.Cells["discount"].Value.ToString() + "', '" + row.Cells["Status"].Value.ToString() + "')";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            q = "delete from VoucherKeys where id='" + row.Cells["Id"].Value.ToString() + "'";
                            objCore.executeQuery(q);
                            q = "insert into VoucherKeys (id, name, amount, status) values ( '" + row.Cells["Id"].Value.ToString() + "', '" + row.Cells["name"].Value.ToString() + "', '" + row.Cells["discount"].Value.ToString() + "', '" + row.Cells["Status"].Value.ToString() + "')";
                            objCore.executeQuery(q);
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
