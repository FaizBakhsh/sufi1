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
    public partial class CustomerDisplay : Form
    {
        public static DataTable dt = new DataTable();
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public static float gst = 0;
        private Sale _frm1;
        public CustomerDisplay()
           {
                InitializeComponent();
                
            }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";
            
        //}
        public void fillgrid(string id, string Totalprice, string itmname, string price, string clr, string siz, string q, string saletyp, string saledetailsid)
        {
            try
            {
                dt.Rows.Add(id, itmname, q, clr, siz, price, Totalprice, saletyp, saledetailsid);



                dataGridView1.DataSource = dt;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                try
                {
                    dataGridView1.Columns[0].Visible = false;

                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;

                }
                catch (Exception ex)
                {


                }

               
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 30;
                }
                gettotal();
            }
            catch (Exception ex)
            {
                
                throw;
            }

        }
        //public void gettotal()
        //{
        //    if (txtdiscount.Text == string.Empty)
        //    { }
        //    else
        //    {
        //        float Num;
        //        bool isNum = float.TryParse(txtdiscount.Text.ToString(), out Num); //c is your variable
        //        if (isNum)
        //        {

        //        }
        //        else
        //        {

        //            MessageBox.Show("Invalid Discount Value. Only Nymbers are allowed");
        //            return;
        //        }
        //    }
        //    double amout = 0;
        //    DataGridViewCellStyle RedCellStyle = null;
        //    RedCellStyle = new DataGridViewCellStyle();
        //    RedCellStyle.ForeColor = Color.RoyalBlue;
        //    DataGridViewCellStyle GreenCellStyle = null;
        //    GreenCellStyle = new DataGridViewCellStyle();
        //    GreenCellStyle.ForeColor = Color.Green;
        //    foreach (DataGridViewRow gr in dataGridView1.Rows)
        //    {

        //        string gcell = string.Empty;
        //        try
        //        {
        //            gcell = gr.Cells["price"].Value.ToString();
        //            string mdval = gr.Cells["Mdid"].Value.ToString();
        //            if (mdval != string.Empty)
        //            {
        //                gr.DefaultCellStyle = RedCellStyle;
        //            }
        //        }
        //        catch (Exception ex)
        //        {


        //        }
        //        if (gcell == string.Empty)
        //        {

        //        }
        //        else
        //        {
        //            amout = amout + Convert.ToDouble(gcell);
        //        }
        //    }
        //    if (txtdiscount.Text.Trim() == string.Empty)
        //    {
        //        txtdiscount.Text = "0";
        //    }

        //    txttotal.Text = amout.ToString();
        //    double dscount = Convert.ToDouble(txtdiscount.Text.Trim());
        //    dscount = (dscount * amout) / 100;
        //    dscount = Math.Round(dscount, 2);
        //    double discountedtotal = amout - dscount;
        //    double total = (gst * discountedtotal) / 100;
        //    total = Math.Round(total, 2);
        //    lblgst.Text = total.ToString();
        //    txtnettotal.Text = Math.Round((total + discountedtotal),2).ToString();

        //}

        public void gettotal()
        {
            try
            {
                if (txtdiscount.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtdiscount.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Discount Value. Only Nymbers are allowed");
                        return;
                    }
                }
                double amout = 0;
                DataGridViewCellStyle RedCellStyle = null;
                RedCellStyle = new DataGridViewCellStyle();
                RedCellStyle.ForeColor = Color.RoyalBlue;
                DataGridViewCellStyle GreenCellStyle = null;
                GreenCellStyle = new DataGridViewCellStyle();
                GreenCellStyle.ForeColor = Color.Green;
                foreach (DataGridViewRow gr in dataGridView1.Rows)
                {

                    string gcell = string.Empty;
                    try
                    {
                        gcell = gr.Cells["totalprice"].Value.ToString();

                    }
                    catch (Exception ex)
                    {


                    }
                    if (gcell == string.Empty)
                    {

                    }
                    else
                    {
                        amout = amout + Convert.ToDouble(gcell);
                    }
                }
                if (txtdiscount.Text.Trim() == string.Empty)
                {
                    txtdiscount.Text = "0";
                }

                txttotal.Text = amout.ToString();
                double dscount = Convert.ToDouble(txtdiscount.Text.Trim());
                dscount = (dscount * amout) / 100;
                dscount = Math.Round(dscount, 2);

                double total = (gst * amout) / 100;
                double discountedtotal = amout - dscount;
                //total = total - discountedtotal;
                total = Math.Round(total, 2);
               // discountamount = dscount.ToString();
               // txtdiscountamount.Text = dscount.ToString();
                lblgst.Text = total.ToString();
                txtnettotal.Text = Math.Round((total + discountedtotal), 2).ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
          
               
                
                    
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void changtxtdscount(string text)
        {
            txtdiscount.Text = text;
            gettotal();
            //btn.Text = text.Replace("&", "&&");
        }
         public void changtext( string text)
        {
            txtcashrecvd.Text = text;
            richTextBox1.Text = ((Convert.ToDouble(txtcashrecvd.Text.Trim()) - (Convert.ToDouble(txtnettotal.Text.Trim())))).ToString();
            //btn.Text = text.Replace("&", "&&");
        }
         public void refresh()
         {
             dt.Clear();
             dataGridView1.DataSource = dt;
            
             txttotal.Text = "0";
             txtcashrecvd.Text = "0";
             txtdiscount.Text = "0";
             txtnettotal.Text = "0";
             richTextBox1.Text = "0";
         }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("Name", typeof(string));
                dt.Columns.Add("Qty", typeof(string));

                dt.Columns.Add("Color", typeof(string));
                dt.Columns.Add("Size", typeof(string));
                dt.Columns.Add("Price", typeof(string));
                dt.Columns.Add("TotalPrice", typeof(string));
                dt.Columns.Add("SaleType", typeof(string));
                dt.Columns.Add("SaleDeTailId", typeof(string));
                DataSet dsgst = new DataSet();
                dsgst = objCore.funGetDataSet("select * from gst");
                if (dsgst.Tables[0].Rows.Count > 0)
                {
                    lblgst.Text = dsgst.Tables[0].Rows[0]["gst"].ToString() + " %";
                    gst = float.Parse(dsgst.Tables[0].Rows[0]["gst"].ToString());
                }
                else
                {
                    lblgst.Text = "0 %";
                    gst = 0;
                }

                Screen[] sc;
                sc = Screen.AllScreens;


                this.StartPosition = FormStartPosition.Manual;
                //   this.Location = new Point(sc[1].Bounds.Left, sc[1].Bounds.Top);
                this.Location = Screen.AllScreens[1].WorkingArea.Location;
                // If you intend the form to be maximized, change it to normal then maximized.
                this.WindowState = FormWindowState.Normal;
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {
                
               
            }
           
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        public void callgrid(Button btn)
        {
            try
            {
                if (btn.Text != string.Empty)
                {
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet dscallgrid = new DataSet();
                    dscallgrid = objCore.funGetDataSet("SELECT     dbo.Modifier.Id, dbo.Modifier.Name AS ModifierName, dbo.Modifier.Price, dbo.RawItem.ItemName AS name FROM         dbo.Modifier INNER JOIN                     dbo.RawItem ON dbo.Modifier.RawItemId = dbo.RawItem.Id where dbo.RawItem.ItemName='" + btn.Text + "' and dbo.Modifier.Menuitemid='" + id + "'");
                    if (dscallgrid.Tables[0].Rows.Count > 0)
                    {
                        
                    } 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtdiscount_TextChanged(object sender, EventArgs e)
        {

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {

        }
      
    }
}
