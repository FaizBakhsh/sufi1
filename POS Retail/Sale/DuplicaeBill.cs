using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Sale
{
    public partial class DuplicaeBill : Form
    {
        private  Sale _frm1;
        public static string user = "";
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public DuplicaeBill(Sale frm1)
           {
                InitializeComponent();
                _frm1 = frm1;
                txtsearch.ForeColor = Color.LightGray;
                txtsearch.Text = "Seach Sale by Invoice No";
                this.txtsearch.Leave += new System.EventHandler(this.textBox1_Leave);
                this.txtsearch.Enter += new System.EventHandler(this.textBox1_Enter);
           }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (txtsearch.Text.Length == 0)
            {
                txtsearch.Text = "Seach Sale by Invoice No";
                txtsearch.ForeColor = Color.LightGray;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (txtsearch.Text == "Seach Sale by Invoice No")
            {
                txtsearch.Text = "";
                txtsearch.ForeColor = Color.Black;
            }
        }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";

        //}

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
         public void changtext(Button btn , string text)
        {
            btn.Text = text;
            btn.Text = text.Replace("&", "&&");
        }
         public void getdata()
         {
             try
             {
                 //category
                 DataSet ds9 = new DataSet();
                 string q9 = "";
                 if (txtsearch.Text == string.Empty || txtsearch.Text == "Seach Sale by Invoice No")
                 {
                     q9 = "SELECT   Id as Bill_No, Date, time, UserId, TotalBill, Discount, NetBill, BillType,  GST, BillStatus,DiscountAmount  from sale where userid='" + id + "' order by id desc";

                 }
                 else
                 {
                     q9 = "SELECT    Id as Bill_No, Date, time, UserId, TotalBill, Discount, NetBill, BillType,  GST, BillStatus,DiscountAmount from sale where id='" + txtsearch.Text.Trim() + "' and userid='" + id + "'";

                 }
                 ds9 = objCore.funGetDataSet(q9);
                 dataGridView1.DataSource = ds9.Tables[0];
                 //dataGridView1.Columns[0].Visible = false;
                 dataGridView1.Columns[3].Visible = false;
                 foreach (DataGridViewRow dr in dataGridView1.Rows)
                 {
                     dr.Height = 40;
                 }

             }
             catch (Exception ex)
             {


             }
         }
        private void AddGroups_Load(object sender, EventArgs e)
        {

            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                //string q = "SELECT   * from sale where userid='" + id + "' order by id desc ";
                //ds = objCore.funGetDataSet(q);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    dataGridView1.DataSource = ds.Tables[0];
                //   // dataGridView1.Columns[0].Visible = false;
                //    dataGridView1.Columns[3].Visible = false;
                //    foreach (DataGridViewRow dr in dataGridView1.Rows)
                //    {
                //        dr.Height = 40;
                //    }
                //}
                getdata();
                ds = new DataSet();
                string q = "SELECT   * from users where id='" + id + "' order by id desc ";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    user = ds.Tables[0].Rows[0]["name"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        public void filg2(string id)
        {
            
            
            DataSet ds = new DataSet();
            string q = "";// "SELECT      dbo.Saledetails.Id, dbo.Saledetails.saleid,dbo.Saledetails.ModifierId, dbo.MenuItem.Name as Item, dbo.Modifier.Name AS Modifier, dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM         dbo.Saledetails LEFT OUTER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                      dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc ";

           // q = "SELECT     dbo.Saledetails.Id, dbo.Saledetails.saleid,dbo.Saledetails.ModifierId, dbo.MenuItem.Name as Item , dbo.RawItem.ItemName AS Modifier , dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId RIGHT OUTER JOIN                      dbo.Saledetails LEFT OUTER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id ON dbo.Modifier.Id = dbo.Saledetails.ModifierId where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc ";
            q = "SELECT     dbo.Saledetails.Id, dbo.Saledetails.saleid, dbo.RawItem.ItemName, dbo.Saledetails.Quantity, dbo.Saledetails.Price,dbo.Saledetails.TotalPrice FROM         dbo.Saledetails INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataGridView2.DataSource = ds.Tables[0];
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
               // dataGridView2.Columns[2].Visible = false;
               // dataGridView2.Columns[7].Visible = false;
                foreach (DataGridViewRow dr in dataGridView2.Rows)
                {

                    try
                    {
                        dr.Height = 40;
                        //string mid = dr.Cells["ModifierId"].Value.ToString();
                        //if (mid == null)
                        //{
                        //    mid = string.Empty;
                        //}
                        //if (mid == "0")
                        //{
                        //    mid = string.Empty;
                        //}
                        //if (mid != string.Empty)
                        //{
                        //    dr.Cells["Name"].Value = string.Empty;

                        //}
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                }
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                filg2(Id);
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public void bindreport(string mop, string sid ,string totalbil,string discount,string NetBill,string GST, string date,string time,string damountt)
        {
            try
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    DataSet dsprint = new DataSet();
                    dsprint = objCore.funGetDataSet("select * from Printers where type='Receipt'");

                    if (dsprint.Tables[0].Rows.Count > 0)
                    {
                        //ReportDocument rptDoc = new ReportDocument();
                        POSRetail.Reports.DuplicateCashReceipt rptDoc = new Reports.DuplicateCashReceipt();
                        POSRetail.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
                        //feereport ds = new feereport(); // .xsd file name
                        DataTable dt = new DataTable();

                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        dt = getAllOrders(mop, sid, totalbil, discount, NetBill, GST, date, time, damountt);
                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);


                        rptDoc.SetDataSource(dsrpt);
                        rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["name"].ToString();
                        //rptDoc.PrintToPrinter(1, false, 0, 0);

                        //rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["Name"].ToString();
                        rptDoc.PrintToPrinter(1, false, 0, 0);
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable getAllOrders(string mp, string siid, string totalbil, string discount, string NetBill, string GST, string date, string time, string damount)
        {

            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("QTY", typeof(string));
            dtrpt.Columns.Add("ItemName", typeof(string));
            dtrpt.Columns.Add("UnitPrice", typeof(string));
            dtrpt.Columns.Add("Totalrice", typeof(string));
            dtrpt.Columns.Add("Total", typeof(double));
            dtrpt.Columns.Add("Discount", typeof(double));
            dtrpt.Columns.Add("GST", typeof(double));
            dtrpt.Columns.Add("NetTotal", typeof(double));
            dtrpt.Columns.Add("Cashier", typeof(string));
            dtrpt.Columns.Add("CName", typeof(string));
            dtrpt.Columns.Add("CAddress", typeof(string));
            dtrpt.Columns.Add("CPhone", typeof(string));
            dtrpt.Columns.Add("MOP", typeof(string));
            dtrpt.Columns.Add("Invoice", typeof(string));
            dtrpt.Columns.Add("Date", typeof(string));
            dtrpt.Columns.Add("Time", typeof(string));            
            dtrpt.Columns.Add("DiscountAmount", typeof(double));
            dtrpt.Columns.Add("logo", typeof(byte[]));

            string cname = "", caddress = "", cphone = "",logo="";
            DataSet dsinfo = new DataSet();
            try
            {
                dsinfo = objCore.funGetDataSet("select * from CompanyInfo");
                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                    cname = dsinfo.Tables[0].Rows[0]["Name"].ToString();
                    caddress = dsinfo.Tables[0].Rows[0]["Address"].ToString();
                    cphone = dsinfo.Tables[0].Rows[0]["Phone"].ToString();
                    logo = dsinfo.Tables[0].Rows[0]["logo"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                
            }
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                try
                {
                    if (dr.Cells["Id"].Value.ToString() != string.Empty)
                    {

                        string amount = damount;
                        string pc = dr.Cells["Price"].Value.ToString();
                        string tpc = dr.Cells["TotalPrice"].Value.ToString();
                        if (pc == string.Empty)
                        {
                            pc = "0";
                        }
                        if (tpc == string.Empty)
                        {
                            tpc = "0";
                        }
                        if (logo == string.Empty)
                        {
                            dtrpt.Rows.Add((dr.Cells["Quantity"].Value.ToString()), dr.Cells["ItemName"].Value.ToString(), Convert.ToDouble(pc), Convert.ToDouble(tpc), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, Convert.ToDouble(amount),null);
                   
                        }
                        else
                        {
                            dtrpt.Rows.Add((dr.Cells["Quantity"].Value.ToString()), dr.Cells["ItemName"].Value.ToString(), Convert.ToDouble(pc), Convert.ToDouble(tpc), Convert.ToDouble(totalbil), Convert.ToDouble(discount), Convert.ToDouble(GST), Convert.ToDouble(NetBill), user, cname, caddress, cphone, mp, siid, date, time, Convert.ToDouble(amount), dsinfo.Tables[0].Rows[0]["logo"]);
                        }
                       

                    }
                }
                catch (Exception ex)
                {


                }
            }

            return dtrpt;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button9_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string date = dataGridView1.Rows[indx].Cells["Date"].Value.ToString();
                    string time = dataGridView1.Rows[indx].Cells["time"].Value.ToString();
                    string saleid = dataGridView1.Rows[indx].Cells["Bill_No"].Value.ToString();
                    string billtype = dataGridView1.Rows[indx].Cells["BillType"].Value.ToString();
                    string totalbil = dataGridView1.Rows[indx].Cells["TotalBill"].Value.ToString();
                    string discount = dataGridView1.Rows[indx].Cells["Discount"].Value.ToString();
                    string NetBill = dataGridView1.Rows[indx].Cells["NetBill"].Value.ToString();
                    string GST = dataGridView1.Rows[indx].Cells["GST"].Value.ToString();
                    string dam = dataGridView1.Rows[indx].Cells["DiscountAmount"].Value.ToString();
                    bindreport(billtype, saleid.ToString(), totalbil, discount, NetBill, GST, date, time,dam);
                    
                }
                else
                {
                    MessageBox.Show("Please Select a Sale First from Sales List");
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void DuplicaeBill_FormClosing(object sender, FormClosingEventArgs e)
        {
            _frm1.Enabled = true;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }
       
    }
}
