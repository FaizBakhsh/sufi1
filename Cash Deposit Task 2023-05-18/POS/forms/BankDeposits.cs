using Newtonsoft.Json;
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

namespace POSRestaurant.forms
{
    public partial class BankDeposits : Form
    {
        public BankDeposits()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon Objcore = new classes.Clsdbcon();
        protected void getdata()
        {
            lblstatus.Text = "";
            try
            {
                string q = "select sum(amount) as amount from billtype";
                q = "SELECT     SUM(dbo.BillType.Amount) AS cash  FROM         dbo.BillType INNER JOIN dbo.Sale ON dbo.BillType.saleid = dbo.Sale.Id where (dbo.Sale.Date = '" + dateTimePicker1.Text + "') and dbo.BillType.type='Cash'  and dbo.Sale.BillStatus='Paid'";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtactual.Text = ds.Tables[0].Rows[0][0].ToString();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            try
            {
                string q = "select Date, ActualAmount, DepositedAmount,Image,status  from BankDeposits where date='" + dateTimePicker1.Text + "'";

                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    textBox2.Text = ds.Tables[0].Rows[0][2].ToString();
                    lblstatus.Text = ds.Tables[0].Rows[0]["status"].ToString();
                    if (lblstatus.Text == "Posted")
                    {
                        lblstatus.ForeColor = Color.Green;
                    }
                    else
                    {
                        lblstatus.ForeColor = Color.Black;
                    }
                    imageData = new Byte[0];
                    imageData = (Byte[])(ds.Tables[0].Rows[0]["Image"]);
                    MemoryStream mem = new MemoryStream(imageData);
                    pictureBox2.Image = Image.FromStream(mem);
                }
                else
                {
                    textBox2.Text = "";
                    imageData = null;
                    pictureBox2.Image = null;
                }
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);

            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            getdata();
        }
        public byte[] imageData = null;
        private void vButton4_Click(object sender, EventArgs e)
        {
            Image File;
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                FileInfo fInfo = new FileInfo(ofd.FileName);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                imageData = br.ReadBytes((int)numBytes);

                File = Image.FromFile(ofd.FileName);
                pictureBox2.Image = File;

            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            getbranchid();
            try
            {
                if (txtactual.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Actual Sale Value can not be left Empty");
                    return;
                }
                if (textBox2.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Deposited Amount");
                    return;
                }

                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();

                {


                    DataSet ds = new DataSet();
                    int id = 0;

                    ds = new DataSet();
                    string q = "select * from BankDeposits where date='" + dateTimePicker1.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (imageData == null)
                        {
                            q = "update BankDeposits set status='Pending',  Date='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "' , ActualAmount ='" + txtactual.Text.Trim().Replace("'", "''") + "', DepositedAmount ='" + textBox2.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                            objCore.executeQueryint(q);
                        }
                        else
                        {
                            q = "update BankDeposits set status='Pending', Image=@logo, Date='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "' , ActualAmount ='" + txtactual.Text.Trim().Replace("'", "''") + "', DepositedAmount ='" + textBox2.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                            string c = objCore.getConnectionString();
                            SqlConnection con = new SqlConnection(c);
                            SqlCommand SqlCom = new SqlCommand(q, con);

                            SqlCom.Parameters.Add(new SqlParameter("@logo", (object)imageData));

                            //Open connection and execute insert query.
                            con.Open();
                            SqlCom.ExecuteNonQuery();
                            con.Close();
                        }
                        MessageBox.Show("Record updated successfully");
                    }
                    else
                    {
                        string qry = "insert into BankDeposits (branchid,status,Date, ActualAmount, DepositedAmount, Image) values('"+branchid+"','Pending','" + dateTimePicker1.Text.Trim().Replace("'", "''") + "','" + txtactual.Text.Trim().Replace("'", "''") + "','" + textBox2.Text.Trim().Replace("'", "''") + "', @ImageData)";

                        string c = objCore.getConnectionString();
                        SqlConnection con = new SqlConnection(c);
                        SqlCommand SqlCom = new SqlCommand(qry, con);

                        SqlCom.Parameters.Add(new SqlParameter("@ImageData", (object)imageData));

                        con.Open();
                        SqlCom.ExecuteNonQuery();
                        con.Close();
                        POSRestaurant.forms.MainForm obj = new forms.MainForm();

                        MessageBox.Show("Record saved successfully");
                    }
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void BankDeposits_Load(object sender, EventArgs e)
        {
            getdata();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            imageData = null;
            pictureBox2.Image = null;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            uploadbyservice();
        }
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='main'";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
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
        protected void getbranchid()
        {
            string bid = "";
            string q = "select id from Branch ";
            DataSet ds1 = new DataSet();
            ds1 = Objcore.funGetDataSet(q);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                bid = ds1.Tables[0].Rows[0][0].ToString();
            }
            branchid = bid;

        }
        public static string branchid = "", url = "";
        protected void uploadbyservice()
        {
            if (branchid == "")
            {
                getbranchid();
            }
            if (url == "")
            {
                type();
            }
            try
            {
                bool chk = false;
                string URI = url + "/DeliveryServices/uploadDeposit.asmx/Getresponse";
                string myparametrs = "";
                List<uploaddepositsClass> res = new List<uploaddepositsClass>();
                string q = "select   * from BankDeposits where status='Pending' and date='" + dateTimePicker1.Text + "' ";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(ds.Tables[0].Rows[i]["image"]);
                        res.Add(new uploaddepositsClass
                        {
                            Onlineid = ds.Tables[0].Rows[i]["id"].ToString(),
                            date = ds.Tables[0].Rows[i]["date"].ToString(),
                            ActualAmount = ds.Tables[0].Rows[i]["ActualAmount"].ToString(),
                            DepositedAmount = ds.Tables[0].Rows[i]["DepositedAmount"].ToString(),
                            Image = data,
                            branchid = branchid
                        });
                        
                        //myparametrs = myparametrs + "{\"Onlineid\":\"" + ds.Tables[0].Rows[i]["id"].ToString() + "\",\"date\":\"" + ds.Tables[0].Rows[i]["date"].ToString() + "\",\"ActualAmount\":\"" + ds.Tables[0].Rows[i]["ActualAmount"].ToString().Replace("'", "") + "\",\"DepositedAmount\":\"" + ds.Tables[0].Rows[i]["DepositedAmount"].ToString().Replace("'", "") + "\",\"Image\":\"" + ds.Tables[0].Rows[i]["Image"] + "\",\"branchid\":\"" + branchid + "\"}";
                    }
                    string msg = "";
                   
      
                    var myparametrs1 = JsonConvert.SerializeObject(res);
                  
                    using (WebClient wc = new WebClient())
                    {
                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                        //wc.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                        string HtmlResult = wc.UploadString(URI, myparametrs1);
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
                            q = "update BankDeposits set status='Posted' where id='" + ds.Tables[0].Rows[i]["id"] + "'";
                            Objcore.executeQuery(q);
                            lblstatus.Text = "Posted";
                            lblstatus.ForeColor = Color.Green;
                        }
                        MessageBox.Show("Data uploaded successfully");
                        
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Inventory Transfer" + ex.Message);
            }
        }
        private void vButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
