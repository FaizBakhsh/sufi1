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

namespace POSRestaurant.forms
{
    public partial class License : Form
    {
        public License()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "Wait Please";
            try
            {
                string uri =txturl.Text+ "/API/keys.asmx/Getresponse?key="+textBox1.Text;
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<KeysClass> res = (List<KeysClass>)JsonConvert.DeserializeObject(result, typeof(List<KeysClass>));
                    if (res.Count > 0)
                    {
                        if (res[0].Status.ToLower() == "used")
                        {
                            MessageBox.Show("Key already used");
                        }
                        else
                        {
                            int days = res[0].Duration;
                            DateTime date = res[0].date;
                            POSRestaurant.Properties.Settings.Default.KeyDate = date.ToString("yyyy-MM-dd");
                            POSRestaurant.Properties.Settings.Default.key = textBox1.Text;
                            POSRestaurant.Properties.Settings.Default.keybaseurl = txturl.Text;
                            
                            POSRestaurant.Properties.Settings.Default.Save();

                            MessageBox.Show("Software Registered Successfully");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Key");
                    }
                }
            }
            catch (Exception ex)
            {


            }
            button1.Text = "Submit";
        }
        string baseurl = "";
        private void License_Load(object sender, EventArgs e)
        {
            baseurl = POSRestaurant.Properties.Settings.Default.keybaseurl.ToString();
            if (baseurl.Length > 0)
            {
                txturl.Text = baseurl;
            }
            else
            {
                txturl.Text = "http://20.122.125.171:9090";
            }
            
        }
    }
}
