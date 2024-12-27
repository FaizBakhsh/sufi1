using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class Messages : Form
    {
        public Messages()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon Objcore = new classes.Clsdbcon();
        private void Messages_Load(object sender, EventArgs e)
        {

        }
        protected void binddata()
        {
            try
            {
                string q = "select * from messages";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from messages where type='" + comboBox1.Text + "'";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    q = "update messages set message='" + richTextBox1.Text + "' where type='" + comboBox1.Text + "'";
                    Objcore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                else
                {
                    q = "insert into  messages (type,message) values ('" + comboBox1.Text + "','" + richTextBox1.Text + "')";
                    Objcore.executeQuery(q);
                    MessageBox.Show("Record added successfully");
                }
            }
            catch (Exception ex)
            {


            }
            binddata();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from messages where type='" + comboBox1.Text + "'";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    richTextBox1.Text = ds.Tables[0].Rows[0]["message"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Promotion")
            {
                button3.Enabled = true;
            }
            else
            {
                button3.Enabled = false;
            }
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void button3_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;

            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 30;

            sendmessage();
            MessageBox.Show("Messages sent successfully");
            this.FormBorderStyle = FormBorderStyle.Fixed3D;
            this.Enabled = true;
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 0;
            progressBar1.Visible = false;
        }
        public void sendmessage()
        {
            try
            {
                string message = "";
                string Phon = "";
                string url = "";
                string q = "select * from messages where type='sale'";
                DataSet dsmsg = new DataSet();
                dsmsg = objCore.funGetDataSet(q);
                if (dsmsg.Tables[0].Rows.Count > 0)
                {
                    url = dsmsg.Tables[0].Rows[0]["url"].ToString();
                }
                message = richTextBox1.Text;
                bool chk = true;
                if (checkBox1.Checked == true)
                {
                    q = "select distinct customer,phone from sale where phone is not null";
                    dsmsg = new DataSet();
                    dsmsg = objCore.funGetDataSet(q);
                    if (dsmsg.Tables[0].Rows.Count > 0)
                    {
                        if (checkBox3.Checked == true)
                        {
                            for (int i = 0; i < dsmsg.Tables[0].Rows.Count; i++)
                            {
                                // message = message.Replace("{customer}", dsmsg.Tables[0].Rows[i]["Name"].ToString()).Replace("{bill}", "");
                                try
                                {
                                    if (chk == true)
                                    {
                                        Phon = dsmsg.Tables[0].Rows[i]["phone"].ToString();
                                        if (Phon.Substring(0, 1) == "0")
                                        {
                                            Phon = "92" + Phon.Substring(1);
                                        }
                                        else
                                        {
                                            Phon = "92" + Phon;
                                        }

                                      
                                        chk = false;
                                    }
                                    else
                                    {
                                        string Phontemp = dsmsg.Tables[0].Rows[i]["phone"].ToString();
                                        if (Phontemp.Substring(0, 1) == "0")
                                        {
                                            Phontemp = "92" + Phontemp.Substring(1);
                                        }
                                        else
                                        {
                                            Phontemp = "92" + Phontemp;
                                        }
                                        Phon = Phon + "," + Phontemp;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                   
                                }
                            }
                            if (Phon.Length > 0 && url.Length > 0)
                            {
                                try
                                {
                                    message = HttpUtility.UrlEncode(message);

                                    url = url.Replace("{phone}", Phon).Replace("{message}", message);

                                    HttpWebRequest request1 = WebRequest.Create(url) as HttpWebRequest;

                                    HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                                    Stream stream1 = response1.GetResponseStream();

                                }
                                catch (Exception ex)
                                {
                                    
                                    
                                }
                                // url = "http://sms.smsonthego.com/api/?username=GJCJT&password=Admin%23*02&receiver=923137807024&msgdata=testmessage";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dsmsg.Tables[0].Rows.Count; i++)
                            {
                                message = message.Replace("{customer}", dsmsg.Tables[0].Rows[i]["customer"].ToString()).Replace("{bill}", "");


                                try
                                {
                                    Phon = dsmsg.Tables[0].Rows[i]["phone"].ToString();
                                    if (Phon.Substring(0, 1) == "0")
                                    {
                                        Phon = "92" + Phon.Substring(1);
                                    }
                                    else
                                    {
                                        Phon = "92" + Phon;
                                    }                                    
                                    if (Phon.Length > 0 && url.Length > 0)
                                    {
                                        message = HttpUtility.UrlEncode(message);

                                        url = url.Replace("{phone}", Phon).Replace("{message}", message);

                                        HttpWebRequest request1 = WebRequest.Create(url) as HttpWebRequest;

                                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                                        Stream stream1 = response1.GetResponseStream();

                                        // url = "http://sms.smsonthego.com/api/?username=GJCJT&password=Admin%23*02&receiver=923137807024&msgdata=testmessage";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    
                                    
                                }
                            }

                        }
                    }
                }
                if (checkBox2.Checked == true)
                {
                    chk = true;
                    q = "select distinct Name,Phone from Customers1 where phone is not null";
                    dsmsg = new DataSet();
                    dsmsg = objCore.funGetDataSet(q);
                    if (dsmsg.Tables[0].Rows.Count > 0)
                    {
                        if (checkBox3.Checked == true)
                        {
                            for (int i = 0; i < dsmsg.Tables[0].Rows.Count; i++)
                            {
                                // message = message.Replace("{customer}", dsmsg.Tables[0].Rows[i]["Name"].ToString()).Replace("{bill}", "");
                                try
                                {
                                    if (chk == true)
                                    {

                                        Phon = dsmsg.Tables[0].Rows[i]["phone"].ToString();
                                        if (Phon.Substring(0, 1) == "0")
                                        {
                                            Phon = "92" + Phon.Substring(1);
                                        }
                                        else
                                        {
                                            Phon = "92" + Phon;
                                        }
                                        chk = false;
                                    }
                                    else
                                    {
                                       string Phontemp = dsmsg.Tables[0].Rows[i]["phone"].ToString();
                                       if (Phontemp.Substring(0, 1) == "0")
                                        {
                                            Phontemp = "92" + Phontemp.Substring(1);
                                        }
                                        else
                                        {
                                            Phontemp = "92" + Phontemp;
                                        }
                                       Phon = Phon + "," + Phontemp;
                                    }
                                }
                                catch (Exception ex)
                                {


                                }
                            }
                            if (Phon.Length > 0 && url.Length > 0)
                            {
                                try
                                {
                                    message = HttpUtility.UrlEncode(message);

                                    url = url.Replace("{phone}", Phon).Replace("{message}", message);

                                    HttpWebRequest request1 = WebRequest.Create(url) as HttpWebRequest;

                                    HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                                    Stream stream1 = response1.GetResponseStream();

                                }
                                catch (Exception ex)
                                {
                                    
                                    
                                }
                                // url = "http://sms.smsonthego.com/api/?username=GJCJT&password=Admin%23*02&receiver=923137807024&msgdata=testmessage";
                            }
                        }
                        else
                        {
                            for (int i = 0; i < dsmsg.Tables[0].Rows.Count; i++)
                            {
                                try
                                {
                                    message = message.Replace("{customer}", dsmsg.Tables[0].Rows[i]["Name"].ToString()).Replace("{bill}", "");

                                    string Phontemp = dsmsg.Tables[0].Rows[i]["phone"].ToString();
                                    if (Phontemp.Substring(0, 1) == "0")
                                    {
                                        Phontemp = "92" + Phontemp.Substring(1);
                                    }
                                    else
                                    {
                                        Phontemp = "92" + Phontemp;
                                    }
                                    Phon = Phontemp;

                                    //Phon = dsmsg.Tables[0].Rows[i]["phone"].ToString();
                                    //Phon = "92" + Phon.Substring(1);
                                    if (Phon.Length > 0 && url.Length > 0)
                                    {
                                        message = HttpUtility.UrlEncode(message);

                                        url = url.Replace("{phone}", Phon).Replace("{message}", message);

                                        HttpWebRequest request1 = WebRequest.Create(url) as HttpWebRequest;

                                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                                        Stream stream1 = response1.GetResponseStream();

                                        // url = "http://sms.smsonthego.com/api/?username=GJCJT&password=Admin%23*02&receiver=923137807024&msgdata=testmessage";
                                    }
                                }
                                catch (Exception ex)
                                {
                                   
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
