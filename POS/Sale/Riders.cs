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
    public partial class Riders : Form
    {
        Deliverystatus _frm;
        public Riders(Deliverystatus frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        public string id = "";
        private void vButton1_Click(object sender, EventArgs e)
        {
            _frm.getdata();
            this.Close();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void Riders_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            string brid = "";
            string q = "select * from branch";
            DataSet ds = new DataSet();
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                brid = ds.Tables[0].Rows[0]["id"].ToString();
            }
            try
            {
                string uri = "http://delivery.simplysufixprs.com/Riders.asmx/Getresponse?branchid=" + brid;
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<RidersClass> res = (List<RidersClass>)JsonConvert.DeserializeObject(result, typeof(List<RidersClass>));
                    comboBox1.DataSource = res;
                    comboBox1.ValueMember = "Id";
                    comboBox1.DisplayMember = "Name";
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }       
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please select Rider");
                return;
            }
            string q = "update sale set riderid='"+comboBox1.SelectedValue+"' where id='"+id+"'";
            objCore.executeQuery(q);
            MessageBox.Show("Successfully Added");
        }
    }
}
