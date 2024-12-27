using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class PointsRedeem : Form
    {
        RestSale _frm;
        public PointsRedeem(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    string q = "select * from LoyalityCards where cardno='" + textBox1.Text + "'";
                    DataSet ds = new DataSet();
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string disc = ds.Tables[0].Rows[0]["Discount"].ToString();
                        if (disc == "")
                        {
                            disc = "0";
                        }
                        float dis = float.Parse(disc);
                        _frm.cardid = ds.Tables[0].Rows[0]["id"].ToString();
                        _frm.discountkeys(dis.ToString(),"Loyality");
                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Loyalitycard_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
        }

        private void Loyalitycard_KeyDown(object sender, KeyEventArgs e)
        {
            textBox1.Focus();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            vButton3.Text = "Please Wait";
            string q = "select * from deliverytransfer where type='service' and server='main'";
            string url = "";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                vButton3.Enabled = false;
                progressBar1.MarqueeAnimationSpeed = 30;
                progressBar1.Style = ProgressBarStyle.Marquee;
                url = ds.Tables[0].Rows[0]["url"].ToString();
                try
                {
                    string uri = url + "/pointscodeverification.asmx/Getresponse?Code=" + textBox1.Text;
                    HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                    HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                    Stream stream1 = response1.GetResponseStream();
                    using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                    {
                        // Load into XML document
                        string result = readStream.ReadToEnd();
                        List<Pointscode> res = (List<Pointscode>)JsonConvert.DeserializeObject(result, typeof(List<Pointscode>));
                        if (res[0].Status.ToLower() == "pending")
                        {
                            float prc = 0, sizeprice = 0;
                            string menuid = "", fid = "", name = "", size = "", kdsid = "";
                            menuid = res[0].MenuItemId.ToString();
                            fid = res[0].FlavouridId;
                            try
                            {
                                q = "select * from menuitem where id='" + menuid + "'";
                                DataSet dsmenu = new DataSet();
                                dsmenu = objcore.funGetDataSet(q);
                                if (dsmenu.Tables[0].Rows.Count > 0)
                                {
                                    name = dsmenu.Tables[0].Rows[0]["Name"].ToString();
                                    kdsid = dsmenu.Tables[0].Rows[0]["kdsid"].ToString();
                                    prc = float.Parse(dsmenu.Tables[0].Rows[0]["price"].ToString());

                                    if (_frm.pricemethod.ToLower() == "gross")
                                    {
                                        string temp = dsmenu.Tables[0].Rows[0]["GrossPrice"].ToString();
                                        if (temp == "")
                                        {
                                            temp = "0";
                                        }
                                        prc = float.Parse(temp);

                                        float g = float.Parse(_frm.gstperc) + 100;
                                        g = g / 100;

                                        prc = prc / g;
                                    }

                                }
                                q = "select * from ModifierFlavour where id='" + fid + "'";
                                DataSet dsflavour = new DataSet();
                                dsflavour = objcore.funGetDataSet(q);
                                if (dsflavour.Tables[0].Rows.Count > 0)
                                {
                                    sizeprice = float.Parse(dsflavour.Tables[0].Rows[0]["price"].ToString());
                                    if (_frm.pricemethod.ToLower() == "gross")
                                    {
                                        string temp = dsflavour.Tables[0].Rows[0]["GrossPrice"].ToString();
                                        if (temp == "")
                                        {
                                            temp = "0";
                                        }
                                        sizeprice = float.Parse(temp);

                                        float g = float.Parse(_frm.gstperc) + 100;
                                        g = g / 100;

                                        sizeprice = sizeprice / g;
                                    }
                                    size = dsflavour.Tables[0].Rows[0]["Name"].ToString();
                                    if (size.Length > 0)
                                    {
                                        size = size + " '";
                                    }
                                }
                            }
                            catch (Exception ex)
                            {

                            }
                            prc = prc + sizeprice;
                            _frm.fillgrid(menuid, "", size + name, "0", "1", "New", "", fid, "", "", kdsid, "", "", "", textBox1.Text);
                            _frm.callrmodifierpoints(menuid);
                            _frm.redeemcode = textBox1.Text;
                            uri = url + "/updatepoints.asmx/Getresponse?date=" + _frm.date + "&branchid=" + _frm.branchid + "&Code=" + textBox1.Text;
                            HttpWebRequest request = WebRequest.Create(uri) as HttpWebRequest;

                            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                            Stream stream = response.GetResponseStream();
                            using (StreamReader readStream1 = new StreamReader(stream, Encoding.UTF8))
                            {
                                // Load into XML document
                                string result1 = readStream.ReadToEnd();
                                List<Pointscode> res1 = (List<Pointscode>)JsonConvert.DeserializeObject(result1, typeof(List<Pointscode>));

                            }
                            this.Close();

                        }
                        else if (res[0].Status.ToLower() == "used")
                        {
                            vButton3.Enabled = true;
                            progressBar1.Visible = false;
                            MessageBox.Show("Code is used already");
                        }
                        else
                        {
                            vButton3.Enabled = true;
                            progressBar1.Visible = false;
                            MessageBox.Show("Invalid Code");
                        }

                    }
                }
                catch (Exception ex)
                {
                    vButton3.Enabled = true;
                    MessageBox.Show("Error reading code. " + ex.Message);
                }
            }
            vButton3.Text = "Redeem";
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Button btn=sender as Button;
            textBox1.Text = textBox1.Text + btn.Text;
        }

        private void button32_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
