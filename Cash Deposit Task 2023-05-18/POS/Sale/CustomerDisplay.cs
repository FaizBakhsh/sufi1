using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AxWMPLib;
namespace POSRestaurant.Sale
{
    public partial class CustomerDisplay : Form
    {
        public DataTable dt = new DataTable();
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public int no = 0;
        DataSet ds = new DataSet();
        public static float gst = 0;
        public float servicecharhes = 0;
        public string servicetype = "", servicegsttype = "";
        private RestSale _frm1;
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
        public string applytax()
        {
            string tax = "yes";
            try
            {
                string q = "select * from discountkeys where discount='" + txtdiscount.Text + "'";
                DataSet dsdis = new DataSet();
                dsdis = objCore.funGetDataSet(q);
                if (dsdis.Tables[0].Rows.Count > 0)
                {
                    tax = dsdis.Tables[0].Rows[0]["tax"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            if (tax == "")
            {
                tax = "yes";
            }
            return tax;
        }
        public void changeqty(int indx, int qty, string price)
        {
            try
            {
                dataGridView1.Rows[indx].Cells[2].Value = qty;
                dataGridView1.Rows[indx].Cells[4].Value = price;
               // gettotal();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        public void deleteitems(string Id, string mid, string rmId, string flid, int RowIndex)
        {
            //if (type == "New")
            try
            {
                {
                    int i = 0, j = 0;

                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        try
                        {
                            DataRow dgr = dt.Rows[i];
                            if (dr.Cells["id"].Value.ToString() == Id)
                            {
                                dgr.Delete();
                            }
                            i++;

                        }
                        catch (Exception ex)
                        {


                        }
                    }

                    dataGridView1.Refresh();
                   // gettotal();
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
      
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            try
            {

                dscompany = objCore.funGetDataSet("select * from CompanyInfo");
                byte[] MyData = new byte[0];


                DataRow myRow;
                myRow = dscompany.Tables[0].Rows[0];

                MyData = (byte[])myRow["logo"];

                MemoryStream stream = new MemoryStream(MyData);
                //With the code below, you are in fact converting the byte array of image
                //to the real image.
                pictureBox1.Image = Image.FromStream(stream);
            }
            catch (Exception ex)
            {
                
               
            }
        }
        public void addpoints(string id, string salid)
        {
            try
            {
                string cpoints = lblcurrentpoints.Text.Replace("Current Points:", "");
                if (cpoints == string.Empty || cpoints=="0")
                {
                    return;
                }
                DataSet ds = new DataSet();
                string q = "select * from Customers where  id='"+id+"' ";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataSet dss = new DataSet();
                    int idd = 0;
                    dss = objCore.funGetDataSet("select max(id) as id from CustomerPoints");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string i = dss.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        idd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        idd = 1;
                    }
                    q = "insert into CustomerPoints (Id,Customerid,SaleId,Points,uploadstatus) values('"+idd+"','"+id+"','"+salid+"','"+cpoints+"','Pending')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void points()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select * from points where  (MaxSale >= '" + txtnettotal.Text + "') AND (MinSale <= '" + txtnettotal.Text + "') ";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblcurrentpoints.Text = "Current Points:" + ds.Tables[0].Rows[0]["points"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        public void cinfo(string id)
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "SELECT     Customers.Id, Customers.Name, SUM(CustomerPoints.Points) AS Points FROM         Customers LEFT OUTER JOIN                      CustomerPoints ON Customers.Id = CustomerPoints.Customerid where dbo.Customers.id='" + id + "' GROUP BY Customers.Name, Customers.Id ";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblcurrentpoints.Visible = true;
                    lblname.Text = "WELLCOME :    " + ds.Tables[0].Rows[0]["name"].ToString();
                    string val = ds.Tables[0].Rows[0]["Points"].ToString();
                    if (val == string.Empty)
                    {
                        val = "0";
                    }
                    lblpoints.Text = "Points :" + val;
                }
                else
                {
                    lblcurrentpoints.Visible = false;
                    lblname.Text = "";
                    lblpoints.Text = "";
                    lblcurrentpoints.Text = "";
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        public void clear()
        {
            txtcashrecvd.Text = "";
            txttotal.Text = "";
            txtnettotal.Text = "";
            dt.Clear();
            dataGridView1.DataSource = dt;
            richTextBox1.Text = "";
            txtdiscount.Text = "";
            lblgst.Text = "";
        }
        public void fillgrid(string id, string mid, string itmname, string price, string q, string disc, string disamount, string SaleDetailid)
        {
            if (price == "0")
            {
                price = "";
            }
            dt.Rows.Add(id, mid, q, itmname, price, SaleDetailid);
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            this.dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            
            //foreach (DataGridViewRow dr in dataGridView1.Rows)
            //{
            //    dr.Height = 40;
            //}
            txtdiscount.Text = disc;
            gettotal(disamount);
            points();
        }
        public void filltotal(string total,string discount,string gst,string servc,string nettotal,string POSFEE)
        {
            try
            {
                txttotal.Text = total;
                txtdiscount.Text = discount;
                lblgst.Text = gst;
                lblservice.Text = servc;
                lblposfee.Text = POSFEE;
                txtnettotal.Text = nettotal;
            }
            catch (Exception ex)
            {
                
            }
        }
        public void gettotal(string damount)
        {
            return;
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
            double amout = 0, amountindvidualdiscount = 0;
            DataGridViewCellStyle RedCellStyle = null;
            RedCellStyle = new DataGridViewCellStyle();
            RedCellStyle.ForeColor = Color.RoyalBlue;
            DataGridViewCellStyle GreenCellStyle = null;
            GreenCellStyle = new DataGridViewCellStyle();
            GreenCellStyle.ForeColor = Color.Green;
            IList<DiscountIndividualClass> DiscountIndividual = null;
            try
            {
                DataSet dsindividual = new DataSet();
                string q = "select id,convert(varchar(100), DiscountPerc) as DiscountPerc,convert(varchar(100), MenuItemId) as MenuItemId,convert(varchar(100), Saleid) as Saleid,convert(varchar(100), Saledetailsid) as Saledetailsid,convert(varchar(100), Runtimemodifierid) as Runtimemodifierid,convert(varchar(100), flavourid) as flavourid  from DiscountIndividual where Saleid='" + _frm1.saleid + "'";
                dsindividual = objCore.funGetDataSet(q);

                DiscountIndividual = dsindividual.Tables[0].AsEnumerable().Select(row =>
                new DiscountIndividualClass
                {
                    DiscountPerc = row.Field<string>("DiscountPerc"),
                    MenuItemId = row.Field<string>("MenuItemId"),
                    Saleid = row.Field<string>("Saleid"),
                    Saledetailsid = row.Field<string>("Saledetailsid"),
                    Runtimemodifierid = row.Field<string>("Runtimemodifierid"),
                    flavourid = row.Field<string>("flavourid")


                }).ToList();
            }
            catch (Exception ex)
            {


            }
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            {

                string gcell = string.Empty;
                try
                {
                    string sdid = "";
                    try
                    {
                        sdid = DiscountIndividual.Where(x => x.Saledetailsid == gr.Cells["SaleDetailid"].Value.ToString()).Select(x => x.Saledetailsid).FirstOrDefault();
                    }
                    catch (Exception ex)
                    {

                    }
                    if (gr.Cells["SaleDetailid"].Value.ToString() == sdid)
                    {

                    }
                    else
                    {
                        gcell = gr.Cells["price"].Value.ToString();

                        if (gcell.Trim() == string.Empty)
                        {
                        }
                        else
                        {
                            amountindvidualdiscount = amountindvidualdiscount + Convert.ToDouble(gcell);
                        }
                    }
                    gcell = gr.Cells["price"].Value.ToString();
                    string mdval = gr.Cells["Mdid"].Value.ToString();
                    if (mdval != string.Empty)
                    {
                        gr.DefaultCellStyle = RedCellStyle;
                    }
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
            double service = 0;
           // textBox1.Text = amout.ToString();
            double dscount = Convert.ToDouble(txtdiscount.Text.Trim());
            dscount = (dscount * amountindvidualdiscount) / 100;
            dscount = Math.Round(dscount - Convert.ToDouble(damount), 2);
            double discountedtotal = 0;
            double total = 0;
            if (applydiscount() == "before")
            {
                discountedtotal = amout;// -dscount;
                service = (servicecharhes * discountedtotal) / 100;
                
                //discountedtotal = discountedtotal;
                total = (gst * discountedtotal) / 100;
                discountedtotal = amout - dscount;
            }
            else
            {
                discountedtotal = amout - dscount;
                service = (servicecharhes * discountedtotal) / 100;
                total = (gst * discountedtotal) / 100;
            }
            if (servicegsttype == "" && servicetype == "")
            {
                if (newordertype == "Take Away")
                {
                    service = 0;
                }
            }
            else
            {
                if (servicetype.Length > 0)
                {
                    if (newordertype == servicetype || servicetype.ToLower() == "all")
                    {
                        if ((servicegsttype == "Cash" && _frm1.rdcash.Checked == true) || (servicegsttype == "Card" && _frm1.rdcard.Checked == true) || (servicegsttype.ToLower() == "all"))
                        {
                        }
                        else
                        {
                            service = 0;
                        }
                    }
                    else
                    {
                        service = 0;
                    }
                }
            }
            lblservice.Text = Math.Round(service, 2).ToString(); 
            total = Math.Round(total, 2);
            txttotal.Text = (amout - dscount).ToString();
           // discountedtotal = amout - dscount;
            lblgst.Text = total.ToString();
            string tax = applytax();
            if (tax == "no")
            {
                total = 0;
                lblgst.Text = "0";
            }
            discountedtotal = discountedtotal + service;
           
            double newtotal = Math.Round((total + discountedtotal), 2);// -Convert.ToDouble(damount);
            txtnettotal.Text = newtotal.ToString();

        }
        public string newordertype = "";
        public string applydiscount()
        {
            string apply = "before";
            try
            {
                string q = "select * from applydiscount ";
                DataSet dsdis = new DataSet();
                dsdis = objCore.funGetDataSet(q);
                if (dsdis.Tables[0].Rows.Count > 0)
                {
                    apply = dsdis.Tables[0].Rows[0]["apply"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            if (apply == "")
            {
                apply = "before";
            }
            return apply;
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
            //gettotal();
            //btn.Text = text.Replace("&", "&&");
        }
         public void changtext( string text)
        {
            txtcashrecvd.Text = text;
            richTextBox1.Text = ((Convert.ToDouble(txtcashrecvd.Text.Trim()) - (Convert.ToDouble(txtnettotal.Text.Trim())))).ToString();
            //btn.Text = text.Replace("&", "&&");
        }
        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.URL = "C:\\sample.mp4";


            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
            try
            {
                getcompany();
                lblname.Text = "";
                lblcurrentpoints.Text = "";
                lblcurrentpoints.Visible = false;
                lblpoints.Text = "";
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("MdId", typeof(string));
                dt.Columns.Add("Qty", typeof(string));
                dt.Columns.Add("Item", typeof(string));
                dt.Columns.Add("Price", typeof(string));
                dt.Columns.Add("SaleDetailid", typeof(string));
                
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
                this.Location = Screen.AllScreens[no].WorkingArea.Location;
                // If you intend the form to be maximized, change it to normal then maximized.
                this.WindowState = FormWindowState.Normal;
                this.WindowState = FormWindowState.Maximized;
                
            }
            catch (Exception ex)
            {
                string q = "insert into errors  (query,date) values('Customer Display " + ex.InnerException.ToString().Replace("'", "") + "','" + DateTime.Now.ToString() + "')";
                objCore.executeQuery(q);
               // MessageBox.Show("Customer Display Screen is not attached");
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
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
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

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            try
            {
                if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
            }
            catch (Exception ex)
            {
                
            }
        }
      
    }
}
