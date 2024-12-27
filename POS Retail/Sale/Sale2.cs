using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
namespace POSRetail.Sale
{
    public partial class Sale1 : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public static DataTable dt = new DataTable();
        public static int quantity = 1;
        public static float gst = 0;
        public string userid = "";
        public static string cashr = "";
        public static string saletype = "";
        public string editmode = "";
        public static bool disc = false;
        public string takeawayid = "";
        public string deliveryid = "";
        public string tableno = "";
        public static string date = "";
        int saleid ;
        public string editsale = "";
        public string discountamount = "0";
        CustomerDisplay obcustomerdisplay;// new CustomerDisplay();
        public bool IsTextBoxEnabled
        {
            get
            {
                return txtdiscount.Enabled;
            }
            set
            {
                txtdiscount.Enabled = value;
            }
        }
        public string  Islbldelivery
        {
            get
            {
                return lblordertype.Text;
            }
            set
            {
                lblordertype.Text = value;
            }
        }
        public Sale1()
        {
            InitializeComponent();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text != string.Empty)
            {
                callsubitem(button1);
            }
        }
        public void callsubitem(Button btn)
        {
            objCore = new classes.Clsdbcon();
            ds = new DataSet();
            string q = "select id from MenuGroup where name='"+btn.Text+"'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                getsubmenuitem(ds.Tables[0].Rows[0]["id"].ToString());
            }
        }
        private void button26_Click(object sender, EventArgs e)
        {
            if (button26.Text != string.Empty)
            {
                callfillgrid(button26);
            }
        }
        public void changtext(Button btn , string text ,string color)
        {
            btn.Text = text;
            btn.Text = text.Replace("&", "&&");
            if (color == string.Empty)
            {
                btn.BackColor = Color.Transparent;
            }
            else
            {
                btn.BackColor = Color.FromArgb(Convert.ToInt32(color));
                if (color.ToLower() == "black")
                {
                    btn.ForeColor = Color.White;
                }
                if (color.ToLower() == "white")
                {
                    btn.ForeColor = Color.Black;
                }
            }
        }
        public void getmenuitem()
        {
            objCore = new classes.Clsdbcon();
            ds = new DataSet();
            string q = "SELECT     dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.MenuGroup.Description, dbo.Color.ColorName FROM         dbo.MenuGroup LEFT OUTER JOIN                      dbo.Color ON dbo.MenuGroup.ColorId = dbo.Color.Id where dbo.MenuGroup.Status='Active'  order by dbo.MenuGroup.id asc";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        changtext(button1, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                        getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }
                    if (i == 1)
                    {
                        changtext(button2, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());

                    }
                    if (i == 2)
                    {
                        changtext(button3, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 3)
                    {
                        changtext(button4, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 4)
                    {
                        changtext(button5, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 5)
                    {
                        changtext(button5, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 6)
                    {
                        changtext(button7, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 7)
                    {
                        changtext(button8, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 8)
                    {
                        changtext(button9, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 9)
                    {
                        changtext(button10, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 10)
                    {
                        changtext(button11, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 11)
                    {
                        changtext(button12, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 12)
                    {
                        changtext(button13, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 13)
                    {
                        changtext(button14, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 14)
                    {
                        changtext(button15, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 15)
                    {
                        changtext(button16, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 16)
                    {
                        changtext(button17, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 17)
                    {
                        changtext(button18, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 18)
                    {
                        changtext(button19, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }
                    if (i == 19)
                    {
                        changtext(button20, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                    }

                }
            }
        }
        public void getsubmenuitem(string id)
        {
            objCore = new classes.Clsdbcon();
           DataSet ds1 = new DataSet();
           string q1 = "";// "select * from MenuItem where MenuGroupId='" + id + "' and status='Active' order by id asc";
           q1 = "SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status, dbo.Color.ColorName, dbo.MenuItem.MenuGroupId FROM         dbo.MenuItem LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id where dbo.MenuItem.MenuGroupId='" + id + "' and dbo.MenuItem.status='Active' order by dbo.MenuItem.id asc";
            ds1 = objCore.funGetDataSet(q1);
            if (ds1.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
                {
                    if (j == 0)
                    {
                        changtext(button21, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 1)
                    {
                        changtext(button22, ds1.Tables[0].Rows[j]["Name"].ToString() , ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 2)
                    {
                        changtext(button23, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 3)
                    {
                        changtext(button24, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 4)
                    {
                        changtext(button25, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 5)
                    {
                        changtext(button26, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 6)
                    {
                        changtext(button27, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 7)
                    {
                        changtext(button28, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 8)
                    {
                        changtext(button29, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 9)
                    {
                        changtext(button30, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 10)
                    {
                        changtext(button31, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 11)
                    {
                        changtext(button32, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 12)
                    {
                        changtext(button33, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 13)
                    {
                        changtext(button34, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 14)
                    {
                        changtext(button35, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 15)
                    {
                        changtext(button36, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 16)
                    {
                        changtext(button37, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 17)
                    {
                        changtext(button38, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 18)
                    {
                        changtext(button39, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }
                    if (j == 19)
                    {
                        changtext(button40, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
                    }

                }
            }
        }
        private void Sale_Load(object sender, EventArgs e)
        {
            
            if (editmode != "1")
            {
                dt.Columns.Add("Id", typeof(string));
                dt.Columns.Add("MdId", typeof(string));
                dt.Columns.Add("Qty", typeof(string));
                dt.Columns.Add("Item", typeof(string));
                dt.Columns.Add("Price", typeof(string));
                dt.Columns.Add("SaleType", typeof(string));
                dt.Columns.Add("SaleDetailid", typeof(string));
            }
            getmenuitem();
            DataSet dsgst = new DataSet();
            dsgst = objCore.funGetDataSet("select * from gst");
            if (dsgst.Tables[0].Rows.Count > 0)
            {
               // lblgst.Text = dsgst.Tables[0].Rows[0]["gst"].ToString()+" %";
                gst = float.Parse(dsgst.Tables[0].Rows[0]["gst"].ToString());
            }
            else
            {
                //lblgst.Text = "0 %";
                gst = 0;
            }
            dsgst = new DataSet();
            dsgst = objCore.funGetDataSet("select * from users where id='"+userid+"'");
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                cashr = dsgst.Tables[0].Rows[0]["name"].ToString();
                label3.Text = dsgst.Tables[0].Rows[0]["Name"].ToString();
            }
            dsgst = new DataSet();
            dsgst = objCore.funGetDataSet("select * from DeviceSetting where device='KOT'");
            if (dsgst.Tables[0].Rows.Count > 0)
            {

                string print = dsgst.Tables[0].Rows[0]["Status"].ToString();
                if (print == "Enabled")
                {
                    vBtnkot.Enabled = true;
                    vBtnkot.Text = "Print Kot";
                }
                else
                {
                    vBtnkot.Enabled = false;
                    vBtnkot.Text = "Enable Kot";
                }
            }
            else
            {
                vBtnkot.Enabled = false;
                vBtnkot.Text = "Enable Kot";
            }

            try
            {
                dsgst = new DataSet();
                dsgst = objCore.funGetDataSet("select * from DeviceSetting where device='Customer Display'");
                if (dsgst.Tables[0].Rows.Count > 0)
                {

                    string show = dsgst.Tables[0].Rows[0]["Status"].ToString();
                    if (show == "Enabled")
                    {
                        obcustomerdisplay = new CustomerDisplay();

                        Screen[] sc;
                        sc = Screen.AllScreens;


                        obcustomerdisplay.Show();
                    }

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            dsgst = new DataSet();
            dsgst = objCore.funGetDataSet("select top(1) * from DayEnd where userid='"+userid+"' order by id desc");
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                date = dsgst.Tables[0].Rows[0]["Date"].ToString();
                string daystatus = dsgst.Tables[0].Rows[0]["DayStatus"].ToString();
                if (daystatus == "Close")
                {
                    vBtnday.Text = "Day Start";
                }
                else
                {
                    vBtnday.Text = "Day End";
                }
            }
            else
            {
                vBtnday.Text = "Day Start";
            }
            lblterminal.Text = "Terminal:" + System.Environment.MachineName.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text != string.Empty)
            {
                callsubitem(button2);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (button3.Text != string.Empty)
            {
                callsubitem(button3);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (button4.Text != string.Empty)
            {
                callsubitem(button4);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (button5.Text != string.Empty)
            {
                callsubitem(button5);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (button6.Text != string.Empty)
            {
                callsubitem(button6);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (button7.Text != string.Empty)
            {
                callsubitem(button7);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (button8.Text != string.Empty)
            {
                callsubitem(button8);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (button9.Text != string.Empty)
            {
                callsubitem(button9);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (button10.Text != string.Empty)
            {
                callsubitem(button10);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (button11.Text != string.Empty)
            {
                callsubitem(button11);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (button12.Text != string.Empty)
            {
                callsubitem(button12);
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (button13.Text != string.Empty)
            {
                callsubitem(button13);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (button14.Text != string.Empty)
            {
                callsubitem(button14);
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (button15.Text != string.Empty)
            {
                callsubitem(button15);
            }
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (button16.Text != string.Empty)
            {
                callsubitem(button16);
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (button17.Text != string.Empty)
            {
                callsubitem(button17);
            }
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (button18.Text != string.Empty)
            {
                callsubitem(button18);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (button19.Text != string.Empty)
            {
                callsubitem(button19);
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (button20.Text != string.Empty)
            {
                callsubitem(button20);
            }
        }
        public void bindreport(string mop , string sid)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    DataSet dsprint = new DataSet();
                    dsprint = objCore.funGetDataSet("select * from Printers where type='Receipt'");

                    if (dsprint.Tables[0].Rows.Count > 0)
                    {
                        //ReportDocument rptDoc = new ReportDocument();
                        POSRetail.Reports.CashReceipt rptDoc = new Reports.CashReceipt();
                        POSRetail.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
                        //feereport ds = new feereport(); // .xsd file name
                        DataTable dt = new DataTable();

                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        dt = getAllOrders(mop, sid);
                        dsrpt.Tables[0].Merge(dt);


                        rptDoc.SetDataSource(dsrpt);
                        //rptDoc.DataDefinition.FormulaFields["PicPath"].Text = POSRetail.Properties.Resources.logo.ToString();// @"'C:\MyImage.jpg'";
                        //rptDoc.PrintOptions.PrinterName = "Posiflex PP6900 576 Partial Cut v3.01 v";
                        //rptDoc.PrintToPrinter(1, false, 0, 0);

                        rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["name"].ToString();
                        rptDoc.PrintToPrinter(1, false, 0, 0);

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public DataTable getAllOrders(string mp, string siid)
        {

            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("QTY", typeof(string));
            dtrpt.Columns.Add("ItemName", typeof(string));
            dtrpt.Columns.Add("Price", typeof(double));
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
            dtrpt.Columns.Add("OrderType", typeof(string));
            dtrpt.Columns.Add("DiscountAmount", typeof(double));
            string cname = "", caddress = "", cphone = "";
            DataSet dsinfo = new DataSet();
            dsinfo = objCore.funGetDataSet("select * from CompanyInfo");
            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                cname = dsinfo.Tables[0].Rows[0]["Name"].ToString();
                caddress = dsinfo.Tables[0].Rows[0]["Address"].ToString();
                cphone = dsinfo.Tables[0].Rows[0]["Phone"].ToString();
            }
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                try
                {
                    if (dr.Cells["Id"].Value.ToString() != string.Empty)
                    {
                        dtrpt.Rows.Add((dr.Cells["Qty"].Value.ToString()), dr.Cells["Item"].Value.ToString(), Convert.ToDouble(dr.Cells["Price"].Value.ToString()), Convert.ToDouble(txttotal.Text.Trim()), Convert.ToDouble(txtdiscount.Text.Trim()), Convert.ToDouble(lblgst.Text.Trim()), Convert.ToDouble(txtnettotal.Text.Trim()),cashr,cname,caddress,cphone,mp,siid,"","",lblordertype.Text,Convert.ToDouble(discountamount));
                        
                       
                    }
                }
                catch (Exception ex)
                {


                }
            }

            return dtrpt;
        }
        public void sale(string billtype , string ordertype )
        {
            try
            {
                bool chk = false;
                string saletype = "";
                if (editsale == string.Empty)
                {
                    saletype = "New";
                }
                else
                {
                    saletype = "Old";
                }
                if (txtdiscount.Enabled == true)
                {
                    disc = true;
                }
                int id = 0;
                if (saletype=="New")
                {
                   
                    ds = new DataSet();
                    ds = objCore.funGetDataSet("select max(id) as id from sale");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        id = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        id = 1;
                    }

                    ds = new DataSet();
                    ds = objCore.funGetDataSet("select top(1) * from dayend where userid='" + userid + "' order by id desc");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        date = ds.Tables[0].Rows[0]["Date"].ToString();
                    }
                    string terminal = System.Environment.MachineName.ToString();
                    string q = "insert into sale (id,date,time,UserId,TotalBill,Discount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,uploadstatus) values ('" + id + "','" + date + "','" + DateTime.Now + "','" + userid + "','" + txttotal.Text + "','" + txtdiscount.Text + "','" + txtnettotal.Text + "','" + billtype + "','" + ordertype + "','" + lblgst.Text + "','Pending','"+terminal+"','Pending')";
                    objCore.executeQuery(q);
                    DataSet dssale = new DataSet();
                    q = "select max(id) as id from sale where userid='" + userid + "'";
                    dssale = objCore.funGetDataSet(q);
                    if (dssale.Tables[0].Rows.Count > 0)
                    {
                        saleid = Convert.ToInt32(dssale.Tables[0].Rows[0][0].ToString());
                    } 
                }
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    try
                    {
                        if (dr.Cells["Id"].Value.ToString() != string.Empty)
                        {
                            if (dr.Cells["SaleType"].Value.ToString() == "Old")
                            {
                                saleid =Convert.ToInt32(editsale);
                            }
                            if (dr.Cells["SaleType"].Value.ToString() == "New")
                            {
                                id = 0;
                                ds = new DataSet();
                                ds = objCore.funGetDataSet("select max(id) as id from saledetails");
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    string i = ds.Tables[0].Rows[0][0].ToString();
                                    if (i == string.Empty)
                                    {
                                        i = "0";
                                    }
                                    id = Convert.ToInt32(i) + 1;
                                }
                                else
                                {
                                    id = 1;
                                }

                                string q = "insert into saledetails (id,saleid,MenuItemId,ModifierId,Quantity,Price,status) values ('" + id + "','" + saleid + "','" + dr.Cells["Id"].Value.ToString() + "','" + dr.Cells["MdId"].Value.ToString() + "','" + dr.Cells["Qty"].Value.ToString() + "','" + dr.Cells["Price"].Value.ToString() + "','Not Void')";
                                objCore.executeQuery(q);
                                chk = true;
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                }
                if (chk==true)
                {
                    bindreportkitchen(saleid.ToString());
                }
                //MessageBox.Show("Sale Added Successfully");
             //   bindreport(billtype,saleid.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void recalsale(string sid)
        {
            try
            {
                editsale = sid;
                dt.Clear();
                saleid = Convert.ToInt32(sid);
                DataSet dsrecalsale = new DataSet();
                string q = "SELECT     dbo.Saledetails.Id , dbo.Saledetails.MenuItemId, dbo.Saledetails.saleid, dbo.MenuItem.Name, dbo.Saledetails.ModifierId, dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id where dbo.Saledetails.saleid='" + sid + "' and dbo.Saledetails.Status ='Not Void' order by dbo.Saledetails.id asc";
                dsrecalsale = objCore.funGetDataSet(q);
                if (dsrecalsale.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecalsale.Tables[0].Rows.Count; i++)
                    {
                        string mid = dsrecalsale.Tables[0].Rows[i]["ModifierId"].ToString();
                        if (mid == "0")
                        {
                            mid = "";
                            fillgrid(dsrecalsale.Tables[0].Rows[i]["menuItemId"].ToString(), mid, dsrecalsale.Tables[0].Rows[i]["Name"].ToString(), dsrecalsale.Tables[0].Rows[i]["Price"].ToString(), dsrecalsale.Tables[0].Rows[i]["Quantity"].ToString(), "Old", dsrecalsale.Tables[0].Rows[i]["id"].ToString());

                        }
                        else
                        {
                            DataSet dscallgrid = new DataSet();
                            dscallgrid = objCore.funGetDataSet("SELECT     dbo.Modifier.Id, dbo.Modifier.Name AS ModifierName, dbo.Modifier.Price, dbo.RawItem.ItemName AS name FROM         dbo.Modifier INNER JOIN                     dbo.RawItem ON dbo.Modifier.RawItemId = dbo.RawItem.Id where dbo.Modifier.id='" + mid + "'");
                            if (dscallgrid.Tables[0].Rows.Count > 0)
                            {
                                fillgrid(dsrecalsale.Tables[0].Rows[i]["menuItemId"].ToString(), dscallgrid.Tables[0].Rows[0]["id"].ToString(), dscallgrid.Tables[0].Rows[0]["Name"].ToString(), dscallgrid.Tables[0].Rows[0]["price"].ToString(), "", "Old", dsrecalsale.Tables[0].Rows[i]["id"].ToString());

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void fillgrid(string id,string mid , string itmname,string price ,string q,string saletyp , string saledetailsid)
        {
            try
            {
                if (mid != string.Empty)
                {
                    try
                    {
                        //obcustomerdisplay.fillgrid(id, mid, itmname, price.ToString(), q.ToString());
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                }
                dt.Rows.Add(id, mid, q, itmname, price,saletyp,saledetailsid);
                dataGridView1.DataSource = dt;
                dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                try
                {
                    dataGridView1.Columns[0].Visible = false;
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[5].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    DataGridViewColumn column = dataGridView1.Columns[2];
                    column.Width = 40;
                    DataGridViewColumn column1 = dataGridView1.Columns[3];
                    column1.Width = 110;
                    DataGridViewColumn column2 = dataGridView1.Columns[4];
                    column2.Width = 75;
                }
                catch (Exception ex)
                {
                    
                   
                }

                dataGridView1.Columns[2].Resizable = DataGridViewTriState.False;
                dataGridView1.Columns[3].Resizable = DataGridViewTriState.False;
                dataGridView1.Columns[4].Resizable = DataGridViewTriState.False;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 30;
                }
                gettotal();
            }
            catch (Exception ex)
            {
                
                
            }
            
        }
        public void callfillgrid(Button name)
        {
            try
            {
                if (vBtnday.Text == "Day Start")
                {
                    MessageBox.Show("Please Start Day First");
                    return;
                }
                if (vBtnday.Text == "Sale Closed")
                {
                    MessageBox.Show("Day Sale Closed");
                    return;
                }
                ds = new DataSet();
                ds = objCore.funGetDataSet("select top(1) * from DayEnd where DayStatus='Open' order by id desc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    date = ds.Tables[0].Rows[0]["Date"].ToString();

                }
                if (lblordertype.Text == "Not Selected")
                {
                    MessageBox.Show("Please Select Order Type");
                    return;
                }

                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                string q = "select * from Menuitem where name='" + name.Text + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    float prc = float.Parse(ds.Tables[0].Rows[0]["price"].ToString());
                    prc = prc * quantity;
                    fillgrid(ds.Tables[0].Rows[0]["id"].ToString(), "", ds.Tables[0].Rows[0]["name"].ToString(), prc.ToString(), quantity.ToString(), "New", "");

                    try
                    {
                        //obcustomerdisplay.fillgrid(ds.Tables[0].Rows[0]["id"].ToString(), "", ds.Tables[0].Rows[0]["name"].ToString(), prc.ToString(), quantity.ToString());

                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                    quantity = 1;
                    DataSet dsmodifier = new DataSet();
                    dsmodifier = objCore.funGetDataSet("select * from modifier where menuitemid='" + ds.Tables[0].Rows[0]["id"].ToString() + "'");
                    if (dsmodifier.Tables[0].Rows.Count > 0)
                    {
                        //POSRetail.Sale.Modifier objmd = new Modifier(this);
                        //objmd.id = ds.Tables[0].Rows[0]["id"].ToString();
                        //objmd.Show();
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
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

                txttotal.Text = amout.ToString();
                double dscount = Convert.ToDouble(txtdiscount.Text.Trim());
                dscount = (dscount * amout) / 100;
                double discountedtotal = amout - dscount;
                double total = (gst * discountedtotal) / 100;
                discountamount = dscount.ToString();
                txtdiscountamount.Text = dscount.ToString();
                lblgst.Text = total.ToString();
                txtnettotal.Text = (total + discountedtotal).ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void button21_Click(object sender, EventArgs e)
        {
            if (button21.Text != string.Empty)
            {
                callfillgrid(button21);
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            if (button22.Text != string.Empty)
            {
                callfillgrid(button22);
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (button23.Text != string.Empty)
            {
                callfillgrid(button23);
            }

        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (button24.Text != string.Empty)
            {
                callfillgrid(button24);
            }
        }

        private void button25_Click(object sender, EventArgs e)
        {
            if (button25.Text != string.Empty)
            {
                callfillgrid(button25);
            }
        }

        private void button27_Click(object sender, EventArgs e)
        {
            if (button27.Text != string.Empty)
            {
                callfillgrid(button27);
            }
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (button28.Text != string.Empty)
            {
                callfillgrid(button28);
            }
        }

        private void button29_Click(object sender, EventArgs e)
        {
            if (button29.Text != string.Empty)
            {
                callfillgrid(button29);
            }
        }

        private void button30_Click(object sender, EventArgs e)
        {
            if (button30.Text != string.Empty)
            {
                callfillgrid(button30);
            }
        }

        private void button31_Click(object sender, EventArgs e)
        {
            if (button31.Text != string.Empty)
            {
                callfillgrid(button31);
            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            if (button32.Text != string.Empty)
            {
                callfillgrid(button32);
            }
        }

        private void button33_Click(object sender, EventArgs e)
        {
            if (button33.Text != string.Empty)
            {
                callfillgrid(button33);
            }
        }

        private void button34_Click(object sender, EventArgs e)
        {
            if (button34.Text != string.Empty)
            {
                callfillgrid(button34);
            }
        }

        private void button35_Click(object sender, EventArgs e)
        {
            if (button35.Text != string.Empty)
            {
                callfillgrid(button35);
            }
        }

        private void button36_Click(object sender, EventArgs e)
        {
            if (button36.Text != string.Empty)
            {
                callfillgrid(button36);
            }
        }

        private void button40_Click(object sender, EventArgs e)
        {
            if (button40.Text != string.Empty)
            {
                callfillgrid(button40);
            }
        }

        private void button39_Click(object sender, EventArgs e)
        {
            if (button39.Text != string.Empty)
            {
                callfillgrid(button39);
            }
        }

        private void button38_Click(object sender, EventArgs e)
        {
            if (button38.Text != string.Empty)
            {
                callfillgrid(button38);
            }
        }

        private void button37_Click(object sender, EventArgs e)
        {
            if (button37.Text != string.Empty)
            {
                callfillgrid(button37);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
              //  if (editsale == string.Empty)
                {
                    string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string type = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    if (type == "New")
                    {
                        DataRow dr = dt.Rows[e.RowIndex];
                        if (dr["id"].ToString() == Id)
                        {
                            dr.Delete();
                        }
                        dataGridView1.Refresh();
                    }
                }
            }
            catch (Exception ex)
            {
                
               
            }

        }

        private void button41_Click(object sender, EventArgs e)
        {
            quantity = 1;
        }

        private void button42_Click(object sender, EventArgs e)
        {
            quantity = 2;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            quantity = 3;
        }

        private void button44_Click(object sender, EventArgs e)
        {
            quantity = 4;
        }

        private void button45_Click(object sender, EventArgs e)
        {
            quantity = 5;
        }

        private void button46_Click(object sender, EventArgs e)
        {
            quantity = 6;
        }

        private void button47_Click(object sender, EventArgs e)
        {
            quantity = 7;
        }

        private void button48_Click(object sender, EventArgs e)
        {
            quantity = 8;
        }

        private void button49_Click(object sender, EventArgs e)
        {
            quantity = 9;
        }

        private void button50_Click(object sender, EventArgs e)
        {
            string qtemp = quantity.ToString() + "0";
            quantity = Convert.ToInt32(qtemp) ;
        }

        private void txtdiscount_TextChanged(object sender, EventArgs e)
        {
            if (txtdiscount.Text.Trim() == string.Empty)
            {
                txtdiscount.Text = "0";
            }
            if (Convert.ToInt32(txtdiscount.Text.Trim()) > 100)
            {
                txtdiscount.Text = "100";
            }
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
            gettotal();
            try
            {
                obcustomerdisplay.changtxtdscount(txtdiscount.Text);
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void txtcashrecvd_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtchange.Text = ((Convert.ToDouble(txtcashrecvd.Text.Trim()) - (Convert.ToDouble(txtnettotal.Text.Trim())))).ToString();
                obcustomerdisplay.changtext(txtcashrecvd.Text);
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }

        private void txtcashrecvd_Leave(object sender, EventArgs e)
        {
            try
            {
                if ((Convert.ToDouble(txtcashrecvd.Text.Trim()) < (Convert.ToDouble(txtnettotal.Text.Trim()))))
                {
                    MessageBox.Show("Received Cash Can Not be Less Than Net total");
                    txtcashrecvd.Focus();
                }
            }
            catch (Exception ex)
            {
                
              
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            sale("Cash", saletype);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            sale("Credit Card", saletype);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            sale("Master Card", saletype);
        }
        public void discount()
        {
            txtdiscount.Enabled = true;
        }

        private void button52_Click(object sender, EventArgs e)
        {

            //POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            //ob.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            //DuplicaeBill objd = new DuplicaeBill(this);
            //objd.id = userid;
            //objd.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            //POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            //ob.editmode = "Discount";
            //ob.Show();
        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            //AllowDiscount ob = new AllowDiscount(this);
            //ob.id = userid;
            //ob.editmode = "VoidBill";
            //ob.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                return;
            }

            if (editsale == string.Empty && vBtnkot.Enabled==true)
            {
                MessageBox.Show("Please Print KOT first");
                return;
            }
            if (txtcashrecvd.Text == string.Empty || txtcashrecvd.Text == "0")
            {
                txtcashrecvd.Text = txtnettotal.Text;
                txtchange.Text = "0";
            }
            if (vBtnkot.Enabled == false)
            {
                if (lblordertype.Text == "Delivery")
                {
                    sale("", lblordertype.Text);
                    updatedelivery(saleid);
                    recalsale(saleid.ToString());
                }
                if (lblordertype.Text == "Take Away")
                {
                    sale("", lblordertype.Text);

                    updatetakeaway(saleid);
                    recalsale(saleid.ToString());
                }
                if (lblordertype.Text == "Din In")
                {
                    sale("", lblordertype.Text);

                    updateDinin(saleid);
                    recalsale(saleid.ToString());
                }
                
            }
            
            updatesales(editsale,"Cash");
            recalsale(saleid.ToString());
            bindreport("Cash", saleid.ToString());
            lblordertype.Text = "Not Selected";
            dt.Rows.Clear();
            dataGridView1.Refresh();
           
            editsale = string.Empty;
        }
        public void updatesales(string saleid, string billtype)
        {
            string q = "update sale set BillType='" + billtype + "', TotalBill='" + txttotal.Text.Trim() + "',Discount='" + txtdiscount.Text.Trim() + "',DiscountAmount='"+discountamount+"',NetBill='" + txtnettotal.Text.Trim() + "' ,BillStatus='Paid' where id='" + saleid + "'";
            objCore.executeQuery(q);
            q = "update TakeAway set status='Paid' where saleid='" + saleid + "'";
            objCore.executeQuery(q);
            q = "update Delivery set status='Paid' where saleid='" + saleid + "'";
            objCore.executeQuery(q);
            q = "update DinInTables set status='Paid' where saleid='" + saleid + "'";
            objCore.executeQuery(q);
            //SaleMessage obj = new SaleMessage(this);
            if (txtcashrecvd.Text == string.Empty)
            {
                txtcashrecvd.Text = "0";
            }
            if (txtchange.Text == string.Empty)
            {
                txtchange.Text = "0";
            }
            //obj.Islbltotal = txtnettotal.Text;
            //obj.Islblreceived = txtcashrecvd.Text;
            //obj.Islblchange = txtchange.Text;
            //obj.Show();
            editsale = string.Empty;
            dt.Clear();
            dataGridView1.Refresh(); 
           
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                return;
            }

            if (editsale == string.Empty && vBtnkot.Enabled == true)
            {
                MessageBox.Show("Please Print KOT first");
                return;
            }
            if (txtcashrecvd.Text == string.Empty || txtcashrecvd.Text == "0")
            {
                try
                {
                    txtcashrecvd.Text = txtnettotal.Text;
                    txtchange.Text = "0";
                }
                catch (Exception ex)
                {
                    
                  
                }
            }
            if (vBtnkot.Enabled == false)
            {
                if (lblordertype.Text == "Delivery")
                {
                    sale("", lblordertype.Text);
                    updatedelivery(saleid);
                    recalsale(saleid.ToString());
                }
                if (lblordertype.Text == "Take Away")
                {
                    sale("", lblordertype.Text);

                    updatetakeaway(saleid);
                    recalsale(saleid.ToString());
                }
                if (lblordertype.Text == "Din In")
                {
                    sale("", lblordertype.Text);

                    updateDinin(saleid);
                   
                }

            }

            updatesales(editsale, "Credit Card"); 
            recalsale(saleid.ToString());
            bindreport("Credit Card", saleid.ToString());
            lblordertype.Text = "Not Selected";
            dt.Rows.Clear();
            dataGridView1.Refresh();
            
            editsale = string.Empty;
       
            
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 0)
            {
                return;
            }

            if (editsale == string.Empty && vBtnkot.Enabled == true)
            {
                MessageBox.Show("Please Print KOT first");
                return;
            }
            if (txtcashrecvd.Text == string.Empty || txtcashrecvd.Text == "0")
            {
                txtcashrecvd.Text = txtnettotal.Text;
                txtchange.Text = "0";
            }
            if (vBtnkot.Enabled == false)
            {
                if (lblordertype.Text == "Delivery")
                {
                    sale("", lblordertype.Text);
                    updatedelivery(saleid);
                    recalsale(saleid.ToString());
                }
                if (lblordertype.Text == "Take Away")
                {
                    sale("", lblordertype.Text);

                    updatetakeaway(saleid);
                    recalsale(saleid.ToString());
                }
                if (lblordertype.Text == "Din In")
                {
                    sale("", lblordertype.Text);

                    updateDinin(saleid);
                    recalsale(saleid.ToString());
                }

            }

            updatesales(editsale, "Master Card");
            recalsale(saleid.ToString());
            bindreport("Master Card", saleid.ToString());
            lblordertype.Text = "Not Selected";
            dt.Rows.Clear();
            dataGridView1.Refresh();
           
            editsale = string.Empty;
            
            
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            //DuplicaeBill objd = new DuplicaeBill(this);
            //objd.id = userid;
            //objd.Show();
        }
        public void voidall(string ssaleid)
        {
            string q = "update Saledetails set status='Void' where saleid='" + ssaleid + "'";
            objCore.executeQuery(q);
            recalsale(ssaleid);
            //saleid = 0;
        }
        public void voidone(string saleDetailid)
        {
            string q = "update Saledetails set status='Void' where id='" + saleDetailid + "'";
            objCore.executeQuery(q);
            DataSet dss = new DataSet();
            dss = objCore.funGetDataSet("select * from Saledetails where id='" + saleDetailid + "'");
            if (dss.Tables[0].Rows.Count > 0)
            {
                recalsale(dss.Tables[0].Rows[0]["saleid"].ToString());
            }
        }
        private void vButton6_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.Rows.Count;
               
                if (indx > 0)
                {
                    string type = dataGridView1.Rows[0].Cells[5].Value.ToString();
                    if (type == "Old")
                    {
                        //AllowDiscount ob = new AllowDiscount(this);
                        //ob.id = userid;
                        //ob.saleid = saleid.ToString();
                        //ob.editmode = "VoidAll";
                        //ob.Show();
                        
                    }
                }
                
            }
            catch (Exception ex)
            {


            }
           
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            if (vBtnday.Text == "Day Start")
            {
                MessageBox.Show("Please Start Day First");
                return;
            }
            if (dataGridView1.Rows.Count > 0)
            {
               // if (vBtnkot.Enabled == false)
                {
                    
                    if (lblordertype.Text == "Delivery")
                    {
                        sale("", lblordertype.Text);
                        updatedelivery(saleid);
                        recalsale(saleid.ToString());
                    }
                    if (lblordertype.Text == "Take Away")
                    {
                        sale("", lblordertype.Text);

                        updatetakeaway(saleid);
                        recalsale(saleid.ToString());
                    }
                    if (lblordertype.Text == "Din In")
                    {
                        sale("", lblordertype.Text);

                        updateDinin(saleid);
                        recalsale(saleid.ToString());
                    }

                }
               
            }
            txtcashrecvd.Text = "0";
            txttotal.Text = "0";
            txtnettotal.Text = "0";
            txtchange.Text = "0";
            txtdiscount.Text = "0";
            txtdiscountamount.Text = "0";
            dt.Clear();
            dataGridView1.Refresh();
            editsale = string.Empty;
            lblordertype.Text = "Take Away";
            //POSRetail.Sale.CustomerId ob = new CustomerId(this);
            //ob.id = userid;
            //ob.Show();
            
        }
        public void updatedelivery(int salid)
        {
            string q = "update delivery set Saleid='"+salid+"' where id='"+deliveryid+"'";
            objCore.executeQuery(q);
        }
        public void updatetakeaway(int salid)
        {
            string q = "update TakeAway set Saleid='" + salid + "' where id='" + takeawayid + "'";
            objCore.executeQuery(q);
        }
        public void updateDinin(int salid)
        {
            int id = 0;
            ds = new DataSet();
            ds = objCore.funGetDataSet("select max(id) as id from DinInTables");
            if (ds.Tables[0].Rows.Count > 0)
            {
                string i = ds.Tables[0].Rows[0][0].ToString();
                if (i == string.Empty)
                {
                    i = "0";
                }
                id = Convert.ToInt32(i) + 1;
            }
            else
            {
                id = 1;
            }
            
            ds = new DataSet();
            ds = objCore.funGetDataSet("select top(1) * from dayend where userid='" + userid + "' order by id desc");
            if (ds.Tables[0].Rows.Count > 0)
            {
                date = ds.Tables[0].Rows[0]["Date"].ToString();
            }
            string q = "insert into DinInTables (id,TableNo,Saleid,Date,time,Status) values('"+id+"','" + tableno + "','" + salid + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','Pending')";
            objCore.executeQuery(q);
        }
        private void vButton12_Click(object sender, EventArgs e)
        {
            if (lblordertype.Text != "not Selected")
            {
                if (dataGridView1.Rows.Count>0)
                {
                    if (lblordertype.Text == "Delivery")
                    {
                        sale("", lblordertype.Text);
                        updatedelivery(saleid);
                    }
                    if (lblordertype.Text == "Take Away")
                    {
                        sale("", lblordertype.Text);

                        updatetakeaway(saleid);
                    }
                    if (lblordertype.Text == "Din In")
                    {
                        sale("", lblordertype.Text);

                        updateDinin(saleid);
                    }

                    //lblordertype.Text = "Not Selected";
                    dt.Rows.Clear();
                    dataGridView1.Refresh();
                    txtcashrecvd.Text = "0";
                    txttotal.Text = "0";
                    txtnettotal.Text = "0";
                    txtchange.Text = "0";
                    editsale = string.Empty;
                    recalsale(saleid.ToString()); 
                }
                
                
            }
            else
            {
                MessageBox.Show("Order Type Not Seleted");
            }
            
        }
        public void bindreportkitchen(string sid)
        {
            try
            {
                if (vBtnkot.Enabled==true)
                {
                    if (dataGridView1.Rows.Count > 0)
                    {
                        DataSet dsprint = new DataSet();
                        dsprint = objCore.funGetDataSet("select * from Printers where type='Kitchen'");

                        if (dsprint.Tables[0].Rows.Count > 0)
                        {
                            //ReportDocument rptDoc = new ReportDocument();
                            POSRetail.Reports.Kotrpt rptDoc = new Reports.Kotrpt();
                            POSRetail.Reports.dskitchen dsrpt = new Reports.dskitchen();
                            // .xsd file name
                            DataTable dt = new DataTable();

                            // Just set the name of data table
                            dt.TableName = "Crystal Report";
                            dt = getAllOrderskitchen(sid);
                            if (dt.Rows.Count > 0)
                            {

                                dsrpt.Tables[0].Merge(dt);


                                rptDoc.SetDataSource(dsrpt);


                                rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["Name"].ToString();
                                rptDoc.PrintToPrinter(1, false, 0, 0);
                            }
                        }
                        else
                        {
                            //MessageBox.Show("Kitchen Printer Name is not Defined");
                        }
                    } 
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public DataTable getAllOrderskitchen(string siid)
        {

            DataTable dtrpt = new DataTable();
            try
            {
                dtrpt.Columns.Add("QTY", typeof(string));
                dtrpt.Columns.Add("ItemName", typeof(string));

                dtrpt.Columns.Add("Cashier", typeof(string));
                dtrpt.Columns.Add("CName", typeof(string));
                dtrpt.Columns.Add("CAddress", typeof(string));
                dtrpt.Columns.Add("CPhone", typeof(string));

                dtrpt.Columns.Add("Invoice", typeof(string));
                dtrpt.Columns.Add("Date", typeof(string));
                dtrpt.Columns.Add("Time", typeof(string));
                dtrpt.Columns.Add("type", typeof(string));
                dtrpt.Columns.Add("TableNo", typeof(string));
                string cname = "", caddress = "", cphone = "";
                DataSet dsinfo = new DataSet();
                dsinfo = objCore.funGetDataSet("select * from CompanyInfo");
                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                    cname = dsinfo.Tables[0].Rows[0]["Name"].ToString();
                    caddress = dsinfo.Tables[0].Rows[0]["Address"].ToString();
                    cphone = dsinfo.Tables[0].Rows[0]["Phone"].ToString();
                }
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    try
                    {
                        if (dr.Cells["Id"].Value.ToString() != string.Empty)
                        {
                            if (dr.Cells["SaleType"].Value.ToString() == "New")
                            {
                                string tb = "";
                                if (lblordertype.Text == "Din In")
                                {
                                    tb = tableno;
                                }
                                dtrpt.Rows.Add((dr.Cells["Qty"].Value.ToString()), dr.Cells["Item"].Value.ToString(), cashr, cname, caddress, cphone, siid, date, DateTime.Now.ToShortTimeString(), lblordertype.Text,tb);

                            }
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                }
            }
            catch (Exception ex)
            {
                
                MessageBox.Show(ex.Message);
            }

            return dtrpt;
        }
        private void vButton8_Click(object sender, EventArgs e)
        {
            try
            {
                if (vBtnday.Text == "Day Start")
                {
                    MessageBox.Show("Please Start Day First");
                    return;
                }
                if (dataGridView1.Rows.Count > 0)
                {

                    // if (vBtnkot.Enabled == false)
                    {
                        if (lblordertype.Text == "Delivery")
                        {
                            sale("", lblordertype.Text);
                            updatedelivery(saleid);
                            recalsale(saleid.ToString());
                        }
                        if (lblordertype.Text == "Take Away")
                        {
                            sale("", lblordertype.Text);

                            updatetakeaway(saleid);
                            recalsale(saleid.ToString());
                        }
                        if (lblordertype.Text == "Din In")
                        {
                            sale("", lblordertype.Text);

                            updateDinin(saleid);
                            recalsale(saleid.ToString());
                        }

                    }

                }
                txtcashrecvd.Text = "0";
                txttotal.Text = "0";
                txtnettotal.Text = "0";
                txtchange.Text = "0";
                txtdiscount.Text = "0";
                txtdiscountamount.Text = "0";
                dt.Clear();
                dataGridView1.Refresh();
                editsale = string.Empty;
                lblordertype.Text = "Delivery";
                //POSRetail.Sale.Delivery ob = new Delivery(this);
                //ob.id = userid;
                //ob.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            //POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            //ob.editmode = "Discount";
            //ob.Show();
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            try
            {
                if (vBtnday.Text == "Day Start")
                {
                    MessageBox.Show("Please Start Day First");
                    return;
                }
                if (dataGridView1.Rows.Count > 0)
                {

                    // if (vBtnkot.Enabled == false)
                    {
                        if (lblordertype.Text == "Delivery")
                        {
                            sale("", lblordertype.Text);
                            updatedelivery(saleid);
                            recalsale(saleid.ToString());
                        }
                        if (lblordertype.Text == "Take Away")
                        {
                            sale("", lblordertype.Text);

                            updatetakeaway(saleid);
                            recalsale(saleid.ToString());
                        }
                        if (lblordertype.Text == "Din In")
                        {
                            sale("", lblordertype.Text);

                            updateDinin(saleid);
                            recalsale(saleid.ToString());
                        }

                    }

                }
                txtcashrecvd.Text = "0";
                txttotal.Text = "0";
                txtnettotal.Text = "0";
                txtchange.Text = "0";
                txtdiscount.Text = "0";
                txtdiscountamount.Text = "0";
                dt.Clear();
                dataGridView1.Refresh();
                editsale = string.Empty;
                lblordertype.Text = "Din In";
                //POSRetail.Sale.TableOrder ob = new TableOrder(this);

                //ob.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton14_Click(object sender, EventArgs e)
        {
            //POSRetail.Sale.BillRecall ob = new BillRecall(this);
            //ob.id = userid;
            //ob.Show();
        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            //lblordertype.Text = "Din In";
            //POSRetail.Sale.TableOrder ob = new TableOrder(this);

            //ob.Show();
        }

        private void vButton17_Click(object sender, EventArgs e)
        {
            //POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            //ob.editmode = "CashDrawer";
            
            //ob.Show();
            

        }

        private void vButton19_Click(object sender, EventArgs e)
        {
            bool chk = false;
            DataSet dsdayend = new DataSet();
            dsdayend = objCore.funGetDataSet("select id from sale where billstatus='pending' and date='"+date+"' and userid='"+userid+"'");
            if (dsdayend.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsdayend.Tables[0].Rows.Count; i++)
                {
                    ds = new DataSet();
                    ds = objCore.funGetDataSet("select Status from saledetails where saleid='" + dsdayend.Tables[0].Rows[i]["id"].ToString() + "'");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int j = 0; j < ds.Tables[0].Rows.Count; j++)
                        {
                            if (ds.Tables[0].Rows[j]["Status"].ToString() == "Not Void")
                            {
                                chk = true;
                            }
                        }
                    }
                }
                if (chk == true)
                {
                    MessageBox.Show("There are Pending Bills. Please Clear Pending Orders Before Logout");
                    return;
                }
            }
             DialogResult msg = MessageBox.Show("Are you sure you want Logout?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

             if (msg == DialogResult.Yes)
             {
                 Application.Exit();
             }
        }

        private void vButton12_Click_1(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;

                if (indx >= 0)
                {
                    string type = dataGridView1.Rows[indx].Cells[5].Value.ToString();
                    string sdetailid = dataGridView1.Rows[indx].Cells[6].Value.ToString();
                    if (type == "Old")
                    {
                        //AllowDiscount ob = new AllowDiscount(this);
                        //ob.id = userid;
                        //ob.saleid = sdetailid.ToString();
                        //ob.editmode = "VoidOne";
                        //ob.Show();

                    }
                }

            }
            catch (Exception ex)
            {


            }
        }
        public void dayend(string uid, string status)
        {
            if (status == "Day Start")
            {

                int id = 1;
                DataSet dsdayend = new DataSet();
                dsdayend = objCore.funGetDataSet("select max(id) as id from dayend");
                if (dsdayend.Tables[0].Rows.Count > 0)
                {
                    string i = dsdayend.Tables[0].Rows[0][0].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    id = Convert.ToInt32(i) + 1;

                }
                else
                {

                }
                date = DateTime.Now.ToString("yyyy-MM-dd");
                string q = "Insert into dayend (Id,Date,DayStatus,UserId) values ('" + id + "','" + DateTime.Now.ToString("yyyy-MM-dd") + "','Open','" + uid + "')";
                objCore.executeQuery(q);
                MessageBox.Show("Day Started Successfully");
                vBtnday.Text = "Day End";
            }
            if (status == "Day End")
            {
                string q = "update dayend set DayStatus='Close' where userid='" + uid + "'";
                objCore.executeQuery(q);
                MessageBox.Show("Day Ended Successfully");
                vBtnday.Text = "Day Start";
            }
        }
        private void vBtnday_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are You Sure to Continue ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                //POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
                //ob.editmode = vBtnday.Text;
                //ob.id = userid;
                //ob.Show();
            }
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
        //    POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
        //    ob.editmode = "Refund";
        //    ob.id = userid;
        //    ob.Show();
        }

        private void vButton18_Click(object sender, EventArgs e)
        {
            //POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            //ob.editmode = "SaleReport";
            //ob.id = userid;
            //ob.cashrr = cashr;
            //ob.datee = date;
            //ob.Show();
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txttotal_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtchange.Text = ((Convert.ToDouble(txtcashrecvd.Text.Trim()) - (Convert.ToDouble(txtnettotal.Text.Trim())))).ToString();
                obcustomerdisplay.changtext(txtcashrecvd.Text);
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged_1(object sender, EventArgs e)
        {
            if (txtdiscount.Text.Trim() == string.Empty)
            {
                txtdiscount.Text = "0";
            }
            if (Convert.ToInt32(txtdiscount.Text.Trim()) > 100)
            {
                txtdiscount.Text = "100";
            }
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
            gettotal();
            try
            {
                obcustomerdisplay.changtxtdscount(txtdiscount.Text);
            }
            catch (Exception ex)
            {


            }
        }
    }
}
