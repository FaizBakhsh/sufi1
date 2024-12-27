using Newtonsoft.Json;
using POSRestaurant.forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class TransferoutApprovalRequest : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public TransferoutApprovalRequest()
        {
            InitializeComponent();
        }
        public void getdata()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0,tin=0,tout=0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
           
            ds.Columns.Add("Transfer Out", typeof(string));
            ds.Columns.Add("Remarks", typeof(string));
            ds.Columns.Add("Status", typeof(string));

            string q = "";
            if (textBox1.Text == "")
            {
                q = "select id,itemname from rawitem order by itemname ";
            }
            else
            {
                q = "select id,itemname from rawitem where itemname like '%"+textBox1.Text+"%' order by itemname ";
            }
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                string val = "",status="";
                double rem = 0;
                tout = 0;
                DataSet dspurchase = new DataSet();
                string remarks = "";
                dspurchase = new DataSet();
                q = "SELECT     (TransferIn) AS tin,(TransferOut) AS tout,Remarks,status FROM     InventoryTransferApproval where Date ='" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and branchid='" + comboBox1.SelectedValue + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                { status = dspurchase.Tables[0].Rows[0]["status"].ToString();
                    remarks = dspurchase.Tables[0].Rows[0]["remarks"].ToString();
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tin = Convert.ToDouble(val);
                    val = dspurchase.Tables[0].Rows[0][1].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = Convert.ToDouble(val);

                }

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(),  tout,remarks,status);
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;

            foreach (DataGridViewRow item in dataGridView1.Rows)
            {
                if (item.Cells["Status"].Value.ToString().Trim() == "Posted")
                {
                    item.DefaultCellStyle.BackColor = Color.Green;
                }
                if (item.Cells["Status"].Value.ToString() == "Pending")
                {
                    item.DefaultCellStyle.BackColor = Color.Red;
                    item.DefaultCellStyle.ForeColor = Color.White;
                }
            }
            
            
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            
           
        }
        
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            bool chk = false;
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                chk1 = false;

                {
                    DataSet dss = new DataSet();

                    dss = new DataSet();
                    string q = "select * from InventoryTransferApproval where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        q = "update InventoryTransferApproval set Status='Pending',Remarks='" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "',TransferOut ='" + Math.Round(Convert.ToDouble(dr.Cells["Transfer Out"].Value.ToString()), 2).ToString() + "' where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "' and sourcebranchid is null";
                        objcore.executeQuery(q);
                        chk = true;
                    }
                    else
                    {
                        if (Math.Round(Convert.ToDouble(dr.Cells["Transfer Out"].Value.ToString()), 2) > 0)
                        {
                            q = "insert into InventoryTransferApproval (status,Remarks,Date, Itemid,  TransferOut, branchid) values('Pending','" + dr.Cells["Remarks"].Value.ToString().Replace("&", "n").Replace("'", "-") + "','" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Transfer Out"].Value.ToString()), 2).ToString() + "','" + comboBox1.SelectedValue + "')";
                            objcore.executeQuery(q);
                            chk = true;
                        }
                    }
                    //updateremaininginventory(dr.Cells["id"].Value.ToString(),variance);

                }
            }
            if (chk == true)
            {
                MessageBox.Show("Record Saved Successfully");
                
            }
        }
        string url = "";
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='main'";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
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
        private void vButton3_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                vButton3.Text = "Please Wait";
                vButton3.Enabled = false;
                uploadbyservice();
                vButton3.Enabled = true;
                vButton3.Text = "Post";
                return;
            }
        }
        protected void uploadbyservice()
        {
           string branchid = comboBox1.SelectedValue.ToString();
            if (url == "")
            {
                type();
            }
            try
            {
                bool chk = false;
                string URI = url + "/DeliveryServices/uploaddemand.asmx/Getresponse?type=trasferout";
                string myparametrs = "";
                string q = "select   * from InventoryTransferApproval where status='Pending' and date='" + dateTimePicker1.Text + "' and TransferOut>0";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (myparametrs != "")
                        {
                            myparametrs = myparametrs + ",";
                        }
                        myparametrs = myparametrs + "{\"Onlineid\":\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"itemid\":\"" + ds.Tables[0].Rows[i]["itemid"].ToString() + "\",\"date\":\"" + ds.Tables[0].Rows[i]["date"].ToString() + "\",\"Quantity\":\"" + ds.Tables[0].Rows[i]["TransferOut"].ToString().Replace("'", "") + "\",\"Status\":\"" + ds.Tables[0].Rows[i]["Status"].ToString().Replace("'", "") + "\",\"supplierid\":\"" + ds.Tables[0].Rows[i]["Remarks"].ToString().Replace("'", "") + "\",\"branchid\":\"" + branchid + "\"}";
                    }
                    string msg = "";
                    myparametrs = "[" + myparametrs + "]";
                        using (WebClient wc = new WebClient())
                        {
                            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                            //wc.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                            string HtmlResult = wc.UploadString(URI, myparametrs);
                            msg = HtmlResult;
                            //txt_postData.Text = HtmlResult;
                            if (HtmlResult.Contains("success"))
                            {
                                chk = true;
                            }
                            else
                            {
                                chk = false;
                            }
                        }
                    if (chk == false)
                    {
                        MessageBox.Show(msg);
                    }
                    else
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            q = "update InventoryTransferApproval set status='Posted' where id='" + ds.Tables[0].Rows[i]["id"] + "'";
                            objcore.executeQuery(q);
                        }
                        MessageBox.Show("Data uploaded successfully");
                        getdata();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
       
        public void fillitems()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Branch";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];
               
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "BranchName";
                


            }
            catch (Exception ex)
            {


            }


        }
     
        private void Variance_Load(object sender, EventArgs e)
        {
            fillitems();
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                vButton4.Text = "Please Wait";
                vButton4.Enabled = false;
                downloadbyservice("transferoutapproval");
                vButton4.Enabled = true;
                vButton4.Text = "Download Approval";
                return;
            }
        }
        protected void downloadbyservice(string type)
        {
            try
            {

                bool chkk = false;
                bool chk = false;
                //foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    //  DataGridViewCheckBoxCell chkchecking = row.Cells[0] as DataGridViewCheckBoxCell;


                    {
                        string date = dateTimePicker1.Text;
                        string uri = url + "/DeliveryServices/downloadtransferdetails.asmx/Getresponse?type=transferoutapproval&id=" + comboBox1.SelectedValue + "&date=" + date;
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
                        SqlConnection connection = new SqlConnection(objcore.getConnectionString());
                        SqlCommand com;
                       
                        {
                            double amount = 0;

                            foreach (var item in res)
                            {
                                if (item.Itemid != "")
                                {
                                    string q = "select * from  InventoryTransfer  where date='" + date + "'  and branchid='" + comboBox1.SelectedValue + "' and Itemid='" + item.Itemid + "' and sourcebranchid is null";
                                    DataSet dss = new DataSet();
                                    dss = objcore.funGetDataSet(q);
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        q = "update InventoryTransfer set datedownload='" + DateTime.Now + "', Date='" + item.Date + "', Itemid='" + item.Itemid + "',  TransferOut='" + item.TransferOut + "', branchid='" + item.branchid + "', Remarks='" + item.Remarks + "' where   date='" + date + "'  and branchid='" + comboBox1.SelectedValue + "' and Itemid='" + item.Itemid + "'  and sourcebranchid is null";

                                    }
                                    else
                                    {
                                        q = "insert into InventoryTransfer (datedownload,Date, Itemid,  TransferOut, branchid, Remarks) values ( '" + DateTime.Now + "', '" + item.Date + "', '" + item.Itemid + "', '" + item.TransferOut + "','" + item.branchid + "','" + item.Remarks + "')";
                                    }
                                    int resultt = objcore.executeQueryint(q);
                                    if (resultt > 0 && chk == false)
                                    {
                                        chk = true;
                                    }
                                }

                            }


                        }
                    }


                }
                if (chk == true)
                {
                    MessageBox.Show("data downloaded successfully");
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.Message);
            }
        }
        private void vButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (item.Cells["Status"].Value.ToString().Trim() == "Posted")
                    {
                        item.DefaultCellStyle.BackColor = Color.Green;
                    }
                    if (item.Cells["Status"].Value.ToString() == "Pending")
                    {
                        item.DefaultCellStyle.BackColor = Color.Red;
                        item.DefaultCellStyle.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}
