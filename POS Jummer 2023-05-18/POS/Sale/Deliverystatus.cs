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
using VIBlend.WinForms.Controls;

namespace POSRestaurant.Sale
{
    public partial class Deliverystatus : Form
    {
        public Deliverystatus()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public string date = "", shiftid = "";
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void Transferbill_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            getdata();   
        }
        public void getdata()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "SELECT        TOP (100) PERCENT dbo.Sale.Id AS Bill_No,dbo.Sale.OnlineId, dbo.Sale.NetBill, dbo.BillType.cashoutime as DispatchedTime, dbo.Sale.DeliveredTime, dbo.Delivery.Name, dbo.Delivery.Phone, dbo.Delivery.Address, dbo.Sale.RiderId FROM            dbo.Sale INNER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId INNER JOIN                         dbo.BillType ON dbo.Sale.Id = dbo.BillType.saleid where dbo.Sale.date='" + date + "'  and dbo.Sale.Billstatus='Paid' and dbo.Sale.OrderType='Delivery' order by dbo.sale.id desc";
                ds = objcore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                //dataGridView1.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        
        }
       
        private void btnsmall_Click(object sender, EventArgs e)
        {
            try
            {
                string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
                string amount = dataGridView1.CurrentRow.Cells["NetBill"].Value.ToString();
                string q = "update sale set Deliverystatus='Delivered',DeliveredTime='"+DateTime.Now+"'  where id='" + id + "'";
                objcore.executeQuery(q);
              
                MessageBox.Show("Bill Updated Successfully");
                getdata();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void vButton1_Click(object sender, EventArgs e)
        {
           
         
            
        }

        

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public string userid = "";

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            vButton1.Text = "Please Wait";
            vButton1.Enabled = false;
            vButton3.Enabled = false;
            btnsmall.Enabled = false;
            string url = "https://www.simplysufixprs.com";
            //url = "http://localhost:21667/Web POS Delivery";
            try
            {
                foreach (DataGridViewRow row in dataGridView1.Rows)
                {
                    if (row.Cells["DeliveredTime"].Value.ToString() != "")
                    {
                        try
                        {
                            string uri = url + "/Deliverystatus.asmx/Getresponse?id=" + row.Cells["OnlineId"].Value.ToString() + "&rid=" + row.Cells["RiderId"].Value.ToString() + "&deliverytime=" + row.Cells["DeliveredTime"].Value.ToString() + "&dispatchtime=" + row.Cells["DispatchedTime"].Value.ToString();
                            HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                            Stream stream1 = response1.GetResponseStream();
                            using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                            {
                                // Load into XML document
                                string result = readStream.ReadToEnd();
                                if (result == "sucess")
                                {
                                    //MessageBox.Show("updated");
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            catch (Exception exx)
            {
                
               
            }
            vButton1.Text = "Synchronize";
            vButton1.Enabled = true;
            vButton3.Enabled = true;
            btnsmall.Enabled = true;
            MessageBox.Show("completed succeefully");
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            Riders obj = new Riders(this);
             string id = dataGridView1.CurrentRow.Cells[0].Value.ToString();
             obj.id = id;
             if (id.Length > 0)
             {
                 obj.Show();
             }
        }
       
    }
}
