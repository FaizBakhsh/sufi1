using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using VIBlend.WinForms.Controls;
using VIBlend;
using VIBlend.Utilities;
namespace POSRetail.Sale
{
    public partial class Sale : Form
    {
        POSRetail.forms.KeyBoard key;
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
        string color = "", size = "", itemid = "";
        public static string date = "";
        int saleid ;
        public string editsale = "";
        public string discountamount = "0";
        CustomerDisplay obcustomerdisplay;// new CustomerDisplay();
        private TextBox focusedTextbox = null;
        string calculateqty = "";
        static DataSet dsitems = new DataSet();
        static DataSet dsitemCodes = new DataSet();
        bool name = true;
        bool code = true;
        bool barcode = true;
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
       // public string  Islbldelivery
       // {
            //get
            //{
               // return lblordertype.Text;
            //}
           // set
           // {
               // lblordertype.Text = value;
           // }
       // }
        public Sale()
        {
            InitializeComponent();
          
        }

        //private void button1_Click(object sender, EventArgs e)
        //{
        //    if (button1.Text != string.Empty)
        //    {
        //        callsubitem(button1);
        //    }
        //}
       
        //public void callsubitem(Button btn)
        //{
        //    objCore = new classes.Clsdbcon();
        //    ds = new DataSet();
        //    string q = "select id from MenuGroup where name='"+btn.Text.Replace("&&","&")+"'";
        //    ds = objCore.funGetDataSet(q);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        getsubmenuitem(ds.Tables[0].Rows[0]["id"].ToString());
        //    }
        //}
        //private void button26_Click(object sender, EventArgs e)
        //{
        //    if (button26.Text != string.Empty)
        //    {
        //        callfillgrid(button26);
        //    }
        //}
        public void changtext(Button btn , string text ,string color, string img,string fontsize,string fontcolor)
        {

            try
            {
                btn.Text = text;
                btn.Text = text.Replace("&", "&&");
                btn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
               // string path = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                string path = Application.StartupPath + "\\Resources\\ButtonIcons\\";
                 btn.Font = new Font("",12,FontStyle.Bold);
                if (fontcolor != string.Empty)
                {
                    btn.ForeColor = Color.FromArgb(Convert.ToInt32(fontcolor));
                }
                if (fontsize != string.Empty)
                {
                    btn.Font = new Font("", Convert.ToInt32(fontsize),FontStyle.Bold);
                }
                if (img != string.Empty)
                {
                    btn.Image = Image.FromFile(path + img);
                   btn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                }
                else
                {
                    btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                }
                if (color == string.Empty)
                {
                    btn.BackColor = Color.Transparent;
                }
                else
                {
                    btn.BackColor = Color.FromArgb(Convert.ToInt32(color));
                    //if (color.ToLower() == "-16777216")
                    //{
                    //    btn.ForeColor = Color.White;
                    //}
                    //if (color.ToLower() == "-1")
                    //{
                    //    btn.ForeColor = Color.Black;
                    //}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        //public void getmenuitem()
        //{
        //    AddDisplayControls();
        //    objCore = new classes.Clsdbcon();
        //    ds = new DataSet();
        //    string q = "SELECT     TOP (100) PERCENT dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.MenuGroup.Description, Color_1.ColorName, dbo.MenuGroup.Image, dbo.Color.ColorName AS Fontcolor,                       dbo.MenuGroup.FontSize FROM         dbo.MenuGroup LEFT OUTER JOIN                     dbo.Color ON dbo.MenuGroup.FontColorId = dbo.Color.Id LEFT OUTER JOIN                      dbo.Color AS Color_1 ON dbo.MenuGroup.ColorId = Color_1.Id WHERE     (dbo.MenuGroup.Status = 'Active') ORDER BY dbo.MenuGroup.Id";
        //    ds = objCore.funGetDataSet(q);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //           // if (i == 0)
        //            {
        //               Button button = new Button();
                      
        //                button.Click += new EventHandler(button_Click);
        //                button.FlatStyle = FlatStyle.Standard;

        //                changtext(button, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString(), ds.Tables[0].Rows[i]["Image"].ToString(),ds.Tables[0].Rows[i]["FontSize"].ToString(),ds.Tables[0].Rows[i]["Fontcolor"].ToString() );
        //                Addbutton(button);
        //                //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
        //            }
        //            #region 

        //            /*  if (i == 1)
        //            {
        //                changtext(button2, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());

        //            }
        //            if (i == 2)
        //            {
        //                changtext(button3, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 3)
        //            {
        //                changtext(button4, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 4)
        //            {
        //                changtext(button5, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 5)
        //            {
        //                changtext(button5, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 6)
        //            {
        //                changtext(button7, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 7)
        //            {
        //                changtext(button8, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 8)
        //            {
        //                changtext(button9, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 9)
        //            {
        //                changtext(button10, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 10)
        //            {
        //                changtext(button11, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 11)
        //            {
        //                changtext(button12, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 12)
        //            {
        //                changtext(button13, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 13)
        //            {
        //                changtext(button14, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 14)
        //            {
        //                changtext(button15, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 15)
        //            {
        //                changtext(button16, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 16)
        //            {
        //                changtext(button17, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 17)
        //            {
        //                changtext(button18, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 18)
        //            {
        //                changtext(button19, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }
        //            if (i == 19)
        //            {
        //                changtext(button20, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
        //            }*/
        //            #endregion

        //        }
        //    }
        //}
        //protected void button_Click(object sender, EventArgs e)
        //{
        //    Button button = sender as Button;
        //    button.FlatStyle = FlatStyle.Flat;
        //    button.FlatAppearance.BorderSize = 1;
        //    button.FlatAppearance.BorderColor = Color.Red;
        //    if (button.Text != string.Empty)
        //    {
        //        callsubitem(button);
        //    }
        //    foreach (Control c in tableLayoutPanelmenugroup.Controls)
        //    {

        //        if (c is Button)
        //        {
                    
        //            if (c.Text == button.Text)
        //            {

        //            }
        //            else
        //            {
                        
        //               ((Button)c).FlatStyle = FlatStyle.Standard;
        //                //button.FlatAppearance.BorderColor = Color.Transparent;
        //            }
        //        }
        //    }
        //    // identify which button was clicked and perform necessary actions
        //}
        protected void buttonmenu_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            if (button.Text != string.Empty)
            {
               // callfillgrid(button);
            }
            // identify which button was clicked and perform necessary actions
        }
        //private void AddDisplayControls()
        //{
        //    tableLayoutPanelmenugroup.Controls.Clear();
        //    //Clear out the existing row and column styles
        //    tableLayoutPanelmenugroup.ColumnStyles.Clear();
        //    tableLayoutPanelmenugroup.RowStyles.Clear();
        //    ds = new DataSet();
        //    ds = objCore.funGetDataSet("select * from Tablelayout where tablename='Menu Group'");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        tableLayoutPanelmenugroup.ColumnCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Columns"].ToString());
        //        tableLayoutPanelmenugroup.RowCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Rows"].ToString());
        //    }
        //    else
        //    {


        //        //Assign table no of rows and column
        //        tableLayoutPanelmenugroup.ColumnCount = 5;
        //        tableLayoutPanelmenugroup.RowCount = 4;

        //    }
        //    //Assign table no of rows and column
            
        //    float cperc = 100 / tableLayoutPanelmenugroup.ColumnCount;
        //    float rperc = 100 / tableLayoutPanelmenugroup.RowCount;

        //    for (int i = 0; i < tableLayoutPanelmenugroup.ColumnCount; i++)
        //    {
        //        tableLayoutPanelmenugroup.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));

        //        for (int j = 0; j < tableLayoutPanelmenugroup.RowCount; j++)
        //        {
        //            if (i == 0)
        //            {
        //                //defining the size of cell
        //                tableLayoutPanelmenugroup.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
        //            }

                  
        //        }
        //    }

        //  //  AddDisplayControlsmenu();

        //}
        //private void AddDisplayControlsmenu()
        //{
        //    tableLayoutPanelmenuitem.Controls.Clear();
        //    //Clear out the existing row and column styles
        //    tableLayoutPanelmenuitem.ColumnStyles.Clear();
        //    tableLayoutPanelmenuitem.RowStyles.Clear();
        //    ds = new DataSet();
        //    ds = objCore.funGetDataSet("select * from Tablelayout where tablename='Menu Items'");
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        tableLayoutPanelmenuitem.ColumnCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Columns"].ToString());
        //        tableLayoutPanelmenuitem.RowCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Rows"].ToString());
        //    }
        //    else
        //    {
                

        //        //Assign table no of rows and column
        //        tableLayoutPanelmenuitem.ColumnCount = 5;
        //        tableLayoutPanelmenuitem.RowCount = 4;
               
        //    }
        //    float cperc = 100 / tableLayoutPanelmenuitem.ColumnCount;
        //    float rperc = 100 / tableLayoutPanelmenuitem.RowCount;

        //    for (int i = 0; i < tableLayoutPanelmenuitem.ColumnCount; i++)
        //    {
        //        tableLayoutPanelmenuitem.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));

        //        for (int j = 0; j < tableLayoutPanelmenuitem.RowCount; j++)
        //        {
        //            if (i == 0)
        //            {
        //                //defining the size of cell
        //                tableLayoutPanelmenuitem.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
        //            }


        //        }
        //    }

        //}
        //int tcolms = 0;
        //int trows = 0;
        //private void Addbutton(Button btn)
        //{

        //    btn.Dock = DockStyle.Fill;


        //    tableLayoutPanelmenugroup.Controls.Add(btn, tcolms, trows);
        //    tcolms++;
        //    //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        //    if (tcolms >= tableLayoutPanelmenugroup.ColumnCount)
        //    {
        //        tcolms = 0;
        //        trows++;
        //    }
        //}
        int tcolms1 = 0;
        int trows1 = 0;
        //private void Addbuttonmenu(Button btn)
        //{

        //    btn.Dock = DockStyle.Fill;


        //    tableLayoutPanelmenuitem.Controls.Add(btn, tcolms1, trows1);
        //    tcolms1++;
        //    //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        //    if (tcolms >= tableLayoutPanelmenuitem.ColumnCount)
        //    {
        //        tcolms1 = 0;
        //        trows1++;
        //    }
        //}
        //public void getsubmenuitem(string id)
        //{
        //    tableLayoutPanelmenuitem.Controls.Clear();
            
        //    objCore = new classes.Clsdbcon();
        //   DataSet ds1 = new DataSet();
        //   string q1 = "";// "select * from MenuItem where MenuGroupId='" + id + "' and status='Active' order by id asc";
        //   q1 = "SELECT   dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status, dbo.Color.ColorName AS Fontcolor,                       dbo.MenuItem.MenuGroupId, dbo.MenuItem.Image, Color_1.ColorName, dbo.MenuItem.FontSize FROM         dbo.MenuItem LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.FontColorId = dbo.Color.Id LEFT OUTER JOIN                      dbo.Color AS Color_1 ON dbo.MenuItem.ColorId = Color_1.Id where dbo.MenuItem.MenuGroupId='" + id + "' and dbo.MenuItem.status='Active' order by dbo.MenuItem.id asc";
        //    ds1 = objCore.funGetDataSet(q1);
        //    if (ds1.Tables[0].Rows.Count > 0)
        //    {
        //        for (int j = 0; j < ds1.Tables[0].Rows.Count; j++)
        //        {
        //           // if (j == 0)
        //            {
        //                Button button1 = new Button();
        //                button1.Click += new EventHandler(buttonmenu_Click);
        //                changtext(button1, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString(), ds1.Tables[0].Rows[j]["Image"].ToString(), ds1.Tables[0].Rows[j]["FontSize"].ToString(), ds1.Tables[0].Rows[j]["Fontcolor"].ToString());
        //                tcolms1 = 0;
        //                trows1 = 0;
        //                Addbuttonmenu(button1);
        //            }
        //            #region
                   
        //            //if (j == 1)
        //            //{
        //            //    changtext(button22, ds1.Tables[0].Rows[j]["Name"].ToString() , ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 2)
        //            //{
        //            //    changtext(button23, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 3)
        //            //{
        //            //    changtext(button24, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 4)
        //            //{
        //            //    changtext(button25, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 5)
        //            //{
        //            //    changtext(button26, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 6)
        //            //{
        //            //    changtext(button27, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 7)
        //            //{
        //            //    changtext(button28, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 8)
        //            //{
        //            //    changtext(button29, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 9)
        //            //{
        //            //    changtext(button30, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 10)
        //            //{
        //            //    changtext(button31, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 11)
        //            //{
        //            //    changtext(button32, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 12)
        //            //{
        //            //    changtext(button33, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 13)
        //            //{
        //            //    changtext(button34, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 14)
        //            //{
        //            //    changtext(button35, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 15)
        //            //{
        //            //    changtext(button36, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 16)
        //            //{
        //            //    changtext(button37, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 17)
        //            //{
        //            //    changtext(button38, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 18)
        //            //{
        //            //    changtext(button39, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            //if (j == 19)
        //            //{
        //            //    changtext(button40, ds1.Tables[0].Rows[j]["Name"].ToString(), ds1.Tables[0].Rows[j]["ColorName"].ToString());
        //            //}
        //            #endregion
        //        }
        //    }
        //}
        private void Sale_Load(object sender, EventArgs e)
        {
            key = new forms.KeyBoard(this);
            if (editmode != "1")
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
                
            }
           // getmenuitem();
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
            lblgstt.Text = gst.ToString()+" %";
            dsgst = new DataSet();
            dsgst = objCore.funGetDataSet("select * from users where id='"+userid+"'");
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                cashr = dsgst.Tables[0].Rows[0]["name"].ToString();
                label3.Text = dsgst.Tables[0].Rows[0]["Name"].ToString();
            }
            try
            {
                dsitems = new DataSet();
                dsitems = objCore.funGetDataSet("select ItemName from RawItem");
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                for (int i = 0; i < dsitems.Tables[0].Rows.Count; i++)
                {
                    MyCollection.Add(dsitems.Tables[0].Rows[i]["ItemName"].ToString());//.GetString(0));
                }
                txtname.AutoCompleteCustomSource = MyCollection;

                txtname.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtname.AutoCompleteSource = AutoCompleteSource.CustomSource;

                dsitemCodes = new DataSet();
                dsitemCodes = objCore.funGetDataSet("select Code from RawItem");
                AutoCompleteStringCollection MyCollection1 = new AutoCompleteStringCollection();
                for (int i = 0; i < dsitemCodes.Tables[0].Rows.Count; i++)
                {
                    MyCollection1.Add(dsitemCodes.Tables[0].Rows[i]["Code"].ToString());//.GetString(0));
                }
                txtcode.AutoCompleteCustomSource = MyCollection1;

                txtcode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtcode.AutoCompleteSource = AutoCompleteSource.CustomSource;
                textBox4.Focus();
            }
            catch (Exception ex)
            {
                
                
            }
            //dsgst = new DataSet();
            //dsgst = objCore.funGetDataSet("select * from DeviceSetting where device='KOT'");
            //if (dsgst.Tables[0].Rows.Count > 0)
            //{

            //    string print = dsgst.Tables[0].Rows[0]["Status"].ToString();
            //    if (print == "Enabled")
            //    {
            //        vBtnkot.Enabled = true;
            //        vBtnkot.Text = "Print Kot";
            //    }
            //    else
            //    {
            //        vBtnkot.Enabled = false;
            //        vBtnkot.Text = "Enable Kot";
            //    }
            //}
            //else
            //{
            //    vBtnkot.Enabled = false;
            //    vBtnkot.Text = "Enable Kot";
            //}

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

                //MessageBox.Show(ex.Message);
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
        #region
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    if (button2.Text != string.Empty)
        //    {
        //        callsubitem(button2);
        //    }
        //}

        //private void button3_Click(object sender, EventArgs e)
        //{
        //    if (button3.Text != string.Empty)
        //    {
        //        callsubitem(button3);
        //    }
        //}

        //private void button4_Click(object sender, EventArgs e)
        //{
        //    if (button4.Text != string.Empty)
        //    {
        //        callsubitem(button4);
        //    }

        //}

        //private void button5_Click(object sender, EventArgs e)
        //{
        //    if (button5.Text != string.Empty)
        //    {
        //        callsubitem(button5);
        //    }
        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    if (button6.Text != string.Empty)
        //    {
        //        callsubitem(button6);
        //    }
        //}

        //private void button7_Click(object sender, EventArgs e)
        //{
        //    if (button7.Text != string.Empty)
        //    {
        //        callsubitem(button7);
        //    }
        //}

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    if (button8.Text != string.Empty)
        //    {
        //        callsubitem(button8);
        //    }
        //}

        //private void button9_Click(object sender, EventArgs e)
        //{
        //    if (button9.Text != string.Empty)
        //    {
        //        callsubitem(button9);
        //    }
        //}

        //private void button10_Click(object sender, EventArgs e)
        //{
        //    if (button10.Text != string.Empty)
        //    {
        //        callsubitem(button10);
        //    }
        //}

        //private void button11_Click(object sender, EventArgs e)
        //{
        //    if (button11.Text != string.Empty)
        //    {
        //        callsubitem(button11);
        //    }
        //}

        //private void button12_Click(object sender, EventArgs e)
        //{
        //    if (button12.Text != string.Empty)
        //    {
        //        callsubitem(button12);
        //    }
        //}

        //private void button13_Click(object sender, EventArgs e)
        //{
        //    if (button13.Text != string.Empty)
        //    {
        //        callsubitem(button13);
        //    }
        //}

        //private void button14_Click(object sender, EventArgs e)
        //{
        //    if (button14.Text != string.Empty)
        //    {
        //        callsubitem(button14);
        //    }
        //}

        //private void button15_Click(object sender, EventArgs e)
        //{
        //    if (button15.Text != string.Empty)
        //    {
        //        callsubitem(button15);
        //    }
        //}

        //private void button16_Click(object sender, EventArgs e)
        //{
        //    if (button16.Text != string.Empty)
        //    {
        //        callsubitem(button16);
        //    }
        //}

        //private void button17_Click(object sender, EventArgs e)
        //{
        //    if (button17.Text != string.Empty)
        //    {
        //        callsubitem(button17);
        //    }
        //}

        //private void button18_Click(object sender, EventArgs e)
        //{
        //    if (button18.Text != string.Empty)
        //    {
        //        callsubitem(button18);
        //    }
        //}

        //private void button19_Click(object sender, EventArgs e)
        //{
        //    if (button19.Text != string.Empty)
        //    {
        //        callsubitem(button19);
        //    }
        //}

        //private void button20_Click(object sender, EventArgs e)
        //{
        //    if (button20.Text != string.Empty)
        //    {
        //        callsubitem(button20);
        //    }
        //}
#endregion
        public void bindreportsample(string mop, string sid)
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
                        POSRetail.Reports.SampleCashReceipt rptDoc = new Reports.SampleCashReceipt();
                        POSRetail.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
                        //feereport ds = new feereport(); // .xsd file name
                        DataTable dt = new DataTable();

                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        dt = getAllOrders(mop, sid);
                        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);


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
                        
                        POSRetail.Reports.CashReceipt rptDoc = new Reports.CashReceipt();
                        //POSRetail.Reports.CrystalReport2 rptDoc = new Reports.CrystalReport2();
                        POSRetail.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
                        //feereport ds = new feereport(); // .xsd file name
                        DataTable dt = new DataTable();

                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        dt = getAllOrders(mop, sid);
                        dsrpt.Tables[0].Merge(dt,false,MissingSchemaAction.Ignore);
                       // CrystalDecisions.CrystalReports.Engine.Section s = (CrystalDecisions.CrystalReports.Engine.Section)rptDoc.ReportDefinition.Sections[3]; 
                        //s.Height = 1100;
 
                        
                        //rptDoc.Section4.Height = 11;
                        rptDoc.SetDataSource(dsrpt);
                        //rptDoc.DataDefinition.FormulaFields["PicPath"].Text = POSRetail.Properties.Resources.logo.ToString();// @"'C:\MyImage.jpg'";
                        //rptDoc.PrintOptions.PrinterName = "Posiflex PP6900 576 Partial Cut v3.01 v";
                        //rptDoc.PrintToPrinter(1, false, 0, 0);

                        rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["name"].ToString();
                        rptDoc.PrintToPrinter(1, false, 0, 0);
                        //POSRetail.Reports.crtest rptDoc1 = new Reports.crtest();
                        //rptDoc1.PrintToPrinter(1, false, 0, 0);
                        clear();
                    }
                }
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
        }
        public DataTable getAllOrders(string mp, string siid)
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

            string cname = "", caddress = "", cphone = "";
            DataSet dsinfo = new DataSet();
           string logo = "";
            string q = "select * from CompanyInfo";
            dsinfo = objCore.funGetDataSet(q);
            try
            {
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
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                try
                {
                    if (dr.Cells["Id"].Value.ToString() != string.Empty)
                    {
                        string pc = dr.Cells["Price"].Value.ToString();
                        string tpc = dr.Cells["TotalPrice"].Value.ToString();
                        //if (pc == string.Empty)
                        //{
                        //    pc = "0";
                        //}
                        if (logo == string.Empty)
                        {
                            dtrpt.Rows.Add((dr.Cells["Qty"].Value.ToString()), dr.Cells["name"].Value.ToString(), (pc), tpc, Convert.ToDouble(txttotal.Text.Trim()), Convert.ToDouble(txtdiscount.Text.Trim()), Convert.ToDouble(lblgst.Text.Trim()), Convert.ToDouble(txtnettotal.Text.Trim()), cashr, cname, caddress, cphone, mp, siid, "", "", Convert.ToDouble(discountamount), null);
                      
                        }
                        else
                        {
                            dtrpt.Rows.Add((dr.Cells["Qty"].Value.ToString()), dr.Cells["name"].Value.ToString(), (pc), tpc, Convert.ToDouble(txttotal.Text.Trim()), Convert.ToDouble(txtdiscount.Text.Trim()), Convert.ToDouble(lblgst.Text.Trim()), Convert.ToDouble(txtnettotal.Text.Trim()), cashr, cname, caddress, cphone, mp, siid, "", "", Convert.ToDouble(discountamount), dsinfo.Tables[0].Rows[0]["logo"]);
                        }
                       
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
                string saletype = "";
                saletype = dataGridView1.Rows[0].Cells["SaleType"].Value.ToString();
                bool chk = false;
                
               
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
                    string dsamount = txtdiscountamount.Text;
                    if (dsamount == string.Empty)
                    {
                        dsamount = "0";
                    }
                    string terminal = System.Environment.MachineName.ToString();
                    string q = "insert into sale (id,date,time,UserId,TotalBill,Discount,DiscountAmount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,uploadstatus) values ('" + id + "','" + date + "','" + DateTime.Now + "','" + userid + "','" + txttotal.Text + "','" + txtdiscount.Text + "','" + dsamount + "','" + txtnettotal.Text + "','" + billtype + "','','" + lblgst.Text + "','Paid','" + terminal + "','Pending')";
                    objCore.executeQuery(q);
                    DataSet dssale = new DataSet();
                    q = "select max(id) as id from sale where userid='" + userid + "'";
                    dssale = objCore.funGetDataSet(q);
                    if (dssale.Tables[0].Rows.Count > 0)
                    {
                        saleid = Convert.ToInt32(dssale.Tables[0].Rows[0][0].ToString());
                    }
                    cashaccount(txtnettotal.Text, saleid.ToString(),billtype);
                    saleaccount(txttotal.Text, saleid.ToString());
                    gstaccount(lblgst.Text, saleid.ToString());
                    discountaccount(txtdiscountamount.Text, saleid.ToString());
                }
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    try
                    {
                        string a = dr.Cells["Id"].Value.ToString();
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

                                string q = "insert into saledetails (id,saleid,ItemId,Quantity,Price,TotalPrice,status) values ('" + id + "','" + saleid + "','" + dr.Cells["Id"].Value.ToString() + "','" + dr.Cells["Qty"].Value.ToString() + "','" + dr.Cells["Price"].Value.ToString() + "','" + dr.Cells["TotalPrice"].Value.ToString() + "','Not Void')";
                                objCore.executeQuery(q);
                                chk = true;
                                Inventryupdate(dr.Cells["Id"].Value.ToString(), Convert.ToInt32(dr.Cells["Qty"].Value.ToString()));
                                Costofsaleaccount(dr.Cells["Qty"].Value.ToString(), saleid.ToString(), dr.Cells["Id"].Value.ToString());
                                inventoryaccount(dr.Cells["Id"].Value.ToString(), dr.Cells["Qty"].Value.ToString(), saleid.ToString());
                            }
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        
                        
                    }
                }
                txtdiscount.Enabled = false;
                textBox4.Focus();
                txtitemquantity.Text = "1";
                obcustomerdisplay.refresh();
                SaleMessage obj = new SaleMessage(this);
                if (txtcashrecvd.Text == string.Empty)
                {
                    txtcashrecvd.Text = "0";
                }
                if (txtchange.Text == string.Empty)
                {
                    txtchange.Text = "0";
                }
                try
                {
                    obj.Islbltotal = txtnettotal.Text;
                   
                }
                catch (Exception ex)
                {
                    
                    
                }
                try
                {
                    
                    obj.Islblreceived = txtcashrecvd.Text;
                    
                }
                catch (Exception ex)
                {


                }
                try
                {
                    
                    obj.Islblchange = txtchange.Text;
                }
                catch (Exception ex)
                {


                }
                try
                {
                    obj.Show();
                }
                catch (Exception ex)
                {
                    
                    
                }
                if (chk==true)
                {
                    //bindreportkitchen(saleid.ToString());
                }
                //MessageBox.Show("Sale Added Successfully");
             //   bindreport(billtype,saleid.ToString());
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void cashaccount(string amount, string saleid,string type)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";
                if (type == "Cash")
                {
                    q = "select * from CashSalesAccountsList where AccountType='Cash Account' ";
                }
                if (type == "Credit Card")
                {
                    q = "select * from CashSalesAccountsList where AccountType='Visa Account' ";
                }
                if (type == "Master Card")
                {
                    q = "select * from CashSalesAccountsList where AccountType='Master Account' ";
                }
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CashAccountSales");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CashAccountSales where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));



                    q = "insert into CashAccountSales (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        public void saleaccount(string amount, string saleid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='Sales Account' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from SalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from SalesAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into SalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        public void gstaccount(string amount, string saleid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='GST Account' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from GSTAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from GSTAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into GSTAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','" + saleid + "','','0','" + Math.Round( Convert.ToDouble( amount),2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        public void discountaccount(string amount, string saleid)
        {
            try
            {
                string vall = txtdiscountamount.Text;
                if (vall == "")
                {
                    vall = "0";
                }

                if (Convert.ToDouble(vall)>0)
                {
                    DataSet dsacount = new DataSet();
                    string q = "select * from CashSalesAccountsList where AccountType='Discount Account' ";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                        int iddd = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from DiscountAccount");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string i = ds.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            iddd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            iddd = 1;
                        }
                        double balance = 0;
                        string val = "";
                        q = "select top 1 * from DiscountAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                        dsacount = new DataSet();
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        balance = Convert.ToDouble(val);
                        double newbalance = (balance + Convert.ToDouble(amount));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into DiscountAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                        objCore.executeQuery(q);
                    } 
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void Costofsaleaccount(string amount, string saleid, string itmid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "SELECT     AVG(dbo.PurchaseDetails.PricePerItem) AS price, dbo.RawItem.Costofsalesid FROM         dbo.RawItem INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId WHERE     (dbo.PurchaseDetails.RawItemId = '" + itmid + "') GROUP BY dbo.RawItem.Costofsalesid ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["Costofsalesid"].ToString();
                    string val = "";
                    val = dsacount.Tables[0].Rows[0]["price"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                   double prc =Convert.ToDouble(val) * Convert.ToInt32(amount);

                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CostSalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    val = "";
                    q = "select top 1 * from CostSalesAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(prc));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into CostSalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','" + saleid + "','','" + Math.Round(Convert.ToDouble(prc), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void inventoryaccount(string itmid, string amount, string saleid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "SELECT     AVG(dbo.PurchaseDetails.PricePerItem) AS price, dbo.RawItem.Inventoryid FROM         dbo.RawItem INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId WHERE     (dbo.PurchaseDetails.RawItemId = '" + itmid + "') GROUP BY dbo.RawItem.Inventoryid ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string invntryid = dsacount.Tables[0].Rows[0]["Inventoryid"].ToString();
                    //amount = dsacount.Tables[0].Rows[0]["price"].ToString();

                    string val = "";
                    val = dsacount.Tables[0].Rows[0]["price"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double prc = Convert.ToDouble(val) * Convert.ToInt32(amount);

                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from InventoryAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    val = "";
                    q = "select top 1 * from InventoryAccount where ChartAccountId='" + invntryid + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(prc));

                    newbalance = Math.Round(newbalance, 2);

                    q = "insert into InventoryAccount (Id,Date,ItemId,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + itmid + "','" + invntryid + "','" + saleid + "','','0','" + Math.Round(Convert.ToDouble(prc), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        public void Inventryupdate(string itmid, int itmqnty)
        {
            try
            {
                //DataSet dsrecipie = new DataSet();
                //string q = "SELECT   * from rawitem where Id='" + itmid + "'";
                //dsrecipie = objCore.funGetDataSet(q);
                //if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    //for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = itmid;
                      //  int qnty = Convert.ToInt32(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                       // double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                      //  double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = 0;
                        amounttodeduct = itmqnty;
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            inventryqty = double.Parse(dsminus.Tables[0].Rows[0]["Quantity"].ToString());
                            objCore.executeQuery("update Inventory set Quantity='" + (inventryqty - amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'");
                        }
                        dsminus = new DataSet();
                        dsminus = objCore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='"+date+"'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                            objCore.executeQuery("update InventoryConsumed set RemainingQuantity='" + (inventryqty - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'");
                        }
                        else
                        {
                            ds = new DataSet();
                            int idcnsmd = 0;
                            ds = new DataSet();
                            ds = objCore.funGetDataSet("select max(id) as id from InventoryConsumed");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string val = ds.Tables[0].Rows[0][0].ToString();
                                if (val == string.Empty)
                                {
                                    val = "0";
                                }
                                idcnsmd = Convert.ToInt32(val) + 1;
                            }
                            else
                            {
                                idcnsmd = 1;
                            }
                            objCore.executeQuery("insert into InventoryConsumed (Id,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + idcnsmd + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + date + "')");
                   
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                
               
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
                string q = "SELECT     dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.Saledetails.Quantity AS qty, dbo.Color.Caption AS color, dbo.Size.SizeName AS size, dbo.Saledetails.Price, dbo.Saledetails.TotalPrice,                       dbo.Saledetails.Id AS SaleDeTailId FROM         dbo.Saledetails INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id LEFT OUTER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id LEFT OUTER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id where dbo.Saledetails.saleid='" + sid + "' order by dbo.Saledetails.id asc";
                dsrecalsale = objCore.funGetDataSet(q);
                if (dsrecalsale.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecalsale.Tables[0].Rows.Count; i++)
                    {
                        fillgrid(dsrecalsale.Tables[0].Rows[i]["id"].ToString(), dsrecalsale.Tables[0].Rows[i]["TotalPrice"].ToString(), dsrecalsale.Tables[0].Rows[i]["ItemName"].ToString(), dsrecalsale.Tables[0].Rows[i]["Price"].ToString(), dsrecalsale.Tables[0].Rows[i]["color"].ToString(), dsrecalsale.Tables[0].Rows[i]["size"].ToString(), dsrecalsale.Tables[0].Rows[i]["qty"].ToString(), "Old", dsrecalsale.Tables[0].Rows[i]["SaleDeTailId"].ToString());
                      
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void fillgrid(string id,string Totalprice , string itmname,string price,string clr, string siz, string q,string saletyp , string saledetailsid)
        {
            try
            {
                if (price == "0")
                {
                    price = "";
                }
                //if (mid != string.Empty)
                {
                    try
                    {
                      //  obcustomerdisplay.fillgrid(id, mid, itmname, price.ToString(), q.ToString());
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                }

                dt.Rows.Add(id, itmname, q, clr,siz,price, Totalprice,saletyp, saledetailsid);


               
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
        public void callfillgrid()
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
                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("please select an Item");
                    txtname.Focus();
                    return;
                }
                if (txtitemquantity.Text == string.Empty || txtitemquantity.Text == "0")
                {
                    MessageBox.Show("please Enter Quantity");
                    txtitemquantity.Focus();
                    return;
                }
                ds = new DataSet();
                ds = objCore.funGetDataSet("select top(1) * from DayEnd where DayStatus='Open' order by id desc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    date = ds.Tables[0].Rows[0]["Date"].ToString();

                }

                DataSet dschk = new DataSet();
                string qchk = "select * from rawitem where ItemName='" + txtname.Text + "' or Code='" + txtcode.Text + "' or BarCode='" + textBox4.Text + "' ";
                dschk = objCore.funGetDataSet(qchk);
                if (dschk.Tables[0].Rows.Count > 0)
                {

                }
                else
                {
                    return;
                }
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
               
                   
                    if (dgcolors.Rows.Count > 0)
                    {
                        DataGridViewSelectedCellCollection DGV = this.dgcolors.SelectedCells;
                        if (DGV.Count > 0)
                        {
                            color = DGV[0].Value.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Please Select Color");
                            return;
                        }
                    }
                    if (dgsize.Rows.Count > 0)
                    {
                        DataGridViewSelectedCellCollection DGV1 = this.dgsize.SelectedCells;
                        if (DGV1.Count > 0)
                        {
                            size = DGV1[0].Value.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Please Select Size");
                            return;
                        }
                    }
                    fillgrid(itemid,txtitemtotal.Text,txtname.Text,txtprice.Text,color,size,txtitemquantity.Text,"New", "");
                    
                    try
                    {
                        obcustomerdisplay.fillgrid(itemid, txtitemtotal.Text, txtname.Text, txtprice.Text, color, size, txtitemquantity.Text, "New", "");

                    }
                    catch (Exception ex)
                    {
                        
                       
                    }
                    quantity = 1;
                    calculateqty = string.Empty;
                   // flavour(ds.Tables[0].Rows[0]["id"].ToString());
                    txtname.Text = string.Empty;
                    txtcode.Text = string.Empty;
                    textBox4.Text = string.Empty;
                    txtitemquantity.Text = "1";
                    txtitemtotal.Text = string.Empty;
                    txtprice.Text = string.Empty;
                    textBox4.Focus();
                    name = true;
                    code = true;
                    barcode = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        public void modifier(string menuid)
        {
            DataSet dsmodifier = new DataSet();
            dsmodifier = objCore.funGetDataSet("select * from modifier where menuitemid='" + menuid + "'");
            if (dsmodifier.Tables[0].Rows.Count > 0)
            {
                POSRetail.Sale.Modifier objmd = new Modifier(this);
                objmd.id = menuid;
                objmd.Show();
            }
        }
        public void flavour(string menuid)
        {
            DataSet dsflavr = new DataSet();
            dsflavr = objCore.funGetDataSet("SELECT  * from ModifierFlavour where MenuItemId='" + menuid + "'");
            if (dsflavr.Tables[0].Rows.Count > 0)
            {

                ModifierFlaour obj = new ModifierFlaour(this);
                obj.menuitemid = menuid;
                //obj.id = dscallgrid.Tables[0].Rows[0]["id"].ToString();
                obj.Show();
            }
            else
            {
                modifier(menuid);
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
                discountamount = dscount.ToString();
                txtdiscountamount.Text = dscount.ToString();
                lblgst.Text = total.ToString();
                txtnettotal.Text = Math.Round((total + discountedtotal), 2).ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        #region
        //private void button21_Click(object sender, EventArgs e)
        //{
        //    if (button21.Text != string.Empty)
        //    {
        //        callfillgrid(button21);
        //    }
        //}

        //private void button22_Click(object sender, EventArgs e)
        //{
        //    if (button22.Text != string.Empty)
        //    {
        //        callfillgrid(button22);
        //    }
        //}

        //private void button23_Click(object sender, EventArgs e)
        //{
        //    if (button23.Text != string.Empty)
        //    {
        //        callfillgrid(button23);
        //    }

        //}

        //private void button24_Click(object sender, EventArgs e)
        //{
        //    if (button24.Text != string.Empty)
        //    {
        //        callfillgrid(button24);
        //    }
        //}

        //private void button25_Click(object sender, EventArgs e)
        //{
        //    if (button25.Text != string.Empty)
        //    {
        //        callfillgrid(button25);
        //    }
        //}

        //private void button27_Click(object sender, EventArgs e)
        //{
        //    if (button27.Text != string.Empty)
        //    {
        //        callfillgrid(button27);
        //    }
        //}

        //private void button28_Click(object sender, EventArgs e)
        //{
        //    if (button28.Text != string.Empty)
        //    {
        //        callfillgrid(button28);
        //    }
        //}

        //private void button29_Click(object sender, EventArgs e)
        //{
        //    if (button29.Text != string.Empty)
        //    {
        //        callfillgrid(button29);
        //    }
        //}

        //private void button30_Click(object sender, EventArgs e)
        //{
        //    if (button30.Text != string.Empty)
        //    {
        //        callfillgrid(button30);
        //    }
        //}

        //private void button31_Click(object sender, EventArgs e)
        //{
        //    if (button31.Text != string.Empty)
        //    {
        //        callfillgrid(button31);
        //    }
        //}

        //private void button32_Click(object sender, EventArgs e)
        //{
        //    if (button32.Text != string.Empty)
        //    {
        //        callfillgrid(button32);
        //    }
        //}

        //private void button33_Click(object sender, EventArgs e)
        //{
        //    if (button33.Text != string.Empty)
        //    {
        //        callfillgrid(button33);
        //    }
        //}

        //private void button34_Click(object sender, EventArgs e)
        //{
        //    if (button34.Text != string.Empty)
        //    {
        //        callfillgrid(button34);
        //    }
        //}

        //private void button35_Click(object sender, EventArgs e)
        //{
        //    if (button35.Text != string.Empty)
        //    {
        //        callfillgrid(button35);
        //    }
        //}

        //private void button36_Click(object sender, EventArgs e)
        //{
        //    if (button36.Text != string.Empty)
        //    {
        //        callfillgrid(button36);
        //    }
        //}

        //private void button40_Click(object sender, EventArgs e)
        //{
        //    if (button40.Text != string.Empty)
        //    {
        //        callfillgrid(button40);
        //    }
        //}

        //private void button39_Click(object sender, EventArgs e)
        //{
        //    if (button39.Text != string.Empty)
        //    {
        //        callfillgrid(button39);
        //    }
        //}

        //private void button38_Click(object sender, EventArgs e)
        //{
        //    if (button38.Text != string.Empty)
        //    {
        //        callfillgrid(button38);
        //    }
        //}

        //private void button37_Click(object sender, EventArgs e)
        //{
        //    if (button37.Text != string.Empty)
        //    {
        //        callfillgrid(button37);
        //    }
        //}
        #endregion
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
              //  if (editsale == string.Empty)
                {
                    string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    string type = dataGridView1.Rows[e.RowIndex].Cells["SaleType"].Value.ToString();
                    if (type == "New")
                    {
                        DataRow dr = dt.Rows[e.RowIndex];
                        if (dr["id"].ToString() == Id)
                        {
                            dr.Delete();
                        }
                        dataGridView1.Refresh();
                        gettotal();
                    }
                }
            }
            catch (Exception ex)
            {
                
               
            }

        }
        bool chk1 = false;
        private void button41_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button41.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button41.Text;
                    }
                    return;
                }


                {
                    calculateqty = calculateqty + "1";
                    quantity = Convert.ToInt32(calculateqty);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void button42_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button42.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button42.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "2";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //quantity = 2;
        }

        private void button43_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button43.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button43.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "3";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button44_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button44.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button44.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "4";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           // quantity = 4;
        }

        private void button45_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button45.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button45.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "5";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //quantity = 5;
        }

        private void button46_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button46.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button46.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "6";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //quantity = 6;
        }

        private void button47_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button47.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button47.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "7";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            // quantity = 7;
        }

        private void button48_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button48.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button48.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "8";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //quantity = 8;
        }

        private void button49_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button49.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button49.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "9";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //quantity = 9;
        }

        private void button50_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Text == "0")
                    {
                        focusedTextbox.Text = button50.Text;
                    }
                    else
                    {
                        focusedTextbox.Text = focusedTextbox.Text + button50.Text;
                    }
                    return;
                }
                calculateqty = calculateqty + "0";
                quantity = Convert.ToInt32(calculateqty);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //string qtemp = quantity.ToString() + "0";
            //quantity = Convert.ToInt32(qtemp) ;
        }

        private void txtdiscount_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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

            POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            ob.Show();
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            DuplicaeBill objd = new DuplicaeBill(this);
            objd.id = userid;
            objd.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            label4.Text = DateTime.Now.ToString();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            ob.editmode = "Discount";
            ob.Show();
        }

        private void panel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            AllowDiscount ob = new AllowDiscount(this);
            ob.id = userid;
            ob.editmode = "VoidBill";
            ob.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count <= 0)
                {
                    return;
                }

                //if (editsale == string.Empty && vBtnkot.Enabled == true)
                //{
                //    MessageBox.Show("Please Print KOT first");
                //    return;
                //}
                if (txtcashrecvd.Text == string.Empty || txtcashrecvd.Text == "0")
                {
                    txtcashrecvd.Text = txtnettotal.Text;
                    txtchange.Text = "0";
                }
                //if (vBtnkot.Enabled == false)
                //{
                //    if (lblordertype.Text == "Delivery")
                //    {
                //        sale("", lblordertype.Text);
                //        updatedelivery(saleid);
                //        recalsale(saleid.ToString());
                //    }
                //    if (lblordertype.Text == "Take Away")
                //    {
                //        sale("", lblordertype.Text);

                //        updatetakeaway(saleid);
                //        recalsale(saleid.ToString());
                //    }
                //    if (lblordertype.Text == "Din In")
                //    {
                //        sale("", lblordertype.Text);

                //        updateDinin(saleid);
                //        recalsale(saleid.ToString());
                //    }

                //}
                string saletype = "";
                saletype = dataGridView1.Rows[0].Cells["SaleType"].Value.ToString();
                if (saletype == "New")
                { }
                else
                {
                    return;
                }
                sale("Cash", "");
               // updatesales(editsale, "Cash");
               // recalsale(saleid.ToString());
                bindreport("Cash", saleid.ToString());
                itemid = "";
                //lblordertype.Text = "Not Selected";
                dt.Rows.Clear();
                dataGridView1.Refresh();

                editsale = string.Empty;
                clear();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            txttotal.Text = "0";
            txtnettotal.Text = "0";
           
            txtdiscount.Text = "0";
            txtdiscountamount.Text = "0";
            txtcashrecvd.Text = "0";
            txtchange.Text = "0";
            txttotal.Text = "0";
            textBox4.Text = "";
            txtitemquantity.Text = "1";
            txtname.Text = "";
            txtcode.Text = "";
            txtitemtotal.Text = "";
            txtprice.Text = "";

        }
        public void updatesales(string saleid, string billtype)
        {
            try
            {
                string q = "update sale set BillType='" + billtype + "', TotalBill='" + txttotal.Text.Trim() + "',Discount='" + txtdiscount.Text.Trim() + "',DiscountAmount='" + discountamount + "',NetBill='" + txtnettotal.Text.Trim() + "' ,BillStatus='Paid' where id='" + saleid + "'";
                objCore.executeQuery(q);
                q = "update TakeAway set status='Paid' where saleid='" + saleid + "'";
                objCore.executeQuery(q);
                q = "update Delivery set status='Paid' where saleid='" + saleid + "'";
                objCore.executeQuery(q);
                q = "update DinInTables set status='Paid' where saleid='" + saleid + "'";
                objCore.executeQuery(q);
                SaleMessage obj = new SaleMessage(this);
                if (txtcashrecvd.Text == string.Empty)
                {
                    txtcashrecvd.Text = "0";
                }
                if (txtchange.Text == string.Empty)
                {
                    txtchange.Text = "0";
                }
                obj.Islbltotal = txtnettotal.Text;
                obj.Islblreceived = txtcashrecvd.Text;
                obj.Islblchange = txtchange.Text;
                obj.Show();
                editsale = string.Empty;
                dt.Clear();
                dataGridView1.Refresh();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
           
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count <= 0)
                {
                    return;
                }

                //if (editsale == string.Empty && vBtnkot.Enabled == true)
                //{
                //    MessageBox.Show("Please Print KOT first");
                //    return;
                //}
                if (txtcashrecvd.Text == string.Empty || txtcashrecvd.Text == "0")
                {
                    txtcashrecvd.Text = txtnettotal.Text;
                    txtchange.Text = "0";
                }
                //if (vBtnkot.Enabled == false)
                //{
                //    if (lblordertype.Text == "Delivery")
                //    {
                //        sale("", lblordertype.Text);
                //        updatedelivery(saleid);
                //        recalsale(saleid.ToString());
                //    }
                //    if (lblordertype.Text == "Take Away")
                //    {
                //        sale("", lblordertype.Text);

                //        updatetakeaway(saleid);
                //        recalsale(saleid.ToString());
                //    }
                //    if (lblordertype.Text == "Din In")
                //    {
                //        sale("", lblordertype.Text);

                //        updateDinin(saleid);
                //        recalsale(saleid.ToString());
                //    }

                //}
                string saletype = "";
                saletype = dataGridView1.Rows[0].Cells["SaleType"].Value.ToString();
                if (saletype == "New")
                { }
                else
                {
                    return;
                }
                sale("Credit Card", "");
                // updatesales(editsale, "Cash");
                // recalsale(saleid.ToString());
                bindreport("Credit Card", saleid.ToString());
                itemid = "";
                //lblordertype.Text = "Not Selected";
                dt.Rows.Clear();
                dataGridView1.Refresh();

                editsale = string.Empty;

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void vButton3_Click(object sender, EventArgs e)
        {

            try
            {
                if (dataGridView1.Rows.Count <= 0)
                {
                    return;
                }

                //if (editsale == string.Empty && vBtnkot.Enabled == true)
                //{
                //    MessageBox.Show("Please Print KOT first");
                //    return;
                //}
                if (txtcashrecvd.Text == string.Empty || txtcashrecvd.Text == "0")
                {
                    txtcashrecvd.Text = txtnettotal.Text;
                    txtchange.Text = "0";
                }
                //if (vBtnkot.Enabled == false)
                //{
                //    if (lblordertype.Text == "Delivery")
                //    {
                //        sale("", lblordertype.Text);
                //        updatedelivery(saleid);
                //        recalsale(saleid.ToString());
                //    }
                //    if (lblordertype.Text == "Take Away")
                //    {
                //        sale("", lblordertype.Text);

                //        updatetakeaway(saleid);
                //        recalsale(saleid.ToString());
                //    }
                //    if (lblordertype.Text == "Din In")
                //    {
                //        sale("", lblordertype.Text);

                //        updateDinin(saleid);
                //        recalsale(saleid.ToString());
                //    }

                //}
                string saletype = "";
                saletype = dataGridView1.Rows[0].Cells["SaleType"].Value.ToString();
                if (saletype == "New")
                { }
                else
                {
                    return;
                }
                sale("Master Card", "");
                // updatesales(editsale, "Cash");
                // recalsale(saleid.ToString());
                bindreport("Master Card", saleid.ToString());
                itemid = "";
                //lblordertype.Text = "Not Selected";
                dt.Rows.Clear();
                dataGridView1.Refresh();

                editsale = string.Empty;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        
        private void vButton5_Click(object sender, EventArgs e)
        {
            AllowDiscount ob = new AllowDiscount(this);
            ob.id = userid;
            
            ob.editmode = "Duplicate";

            
            //this.Enabled = false;
            ob.Show();
        }
        public void voidall(string ssaleid)
        {
            try
            {
                string q = "update Saledetails set status='Void' where saleid='" + ssaleid + "'";
                objCore.executeQuery(q);
                recalsale(ssaleid);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //saleid = 0;
        }
        public void Waste(string ssaleid)
        {
            try
            {
                string q = "update Saledetails set status='Waste' where saleid='" + ssaleid + "'";
                objCore.executeQuery(q);
                recalsale(ssaleid);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            //saleid = 0;
        }
        public void voidone(string saleDetailid)
        {
            try
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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                        AllowDiscount ob = new AllowDiscount(this);
                        ob.id = userid;
                        ob.saleid = saleid.ToString();
                        ob.editmode = "VoidAll";
                        this.Enabled = false;
                        ob.Show();
                        
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

                        //if (lblordertype.Text == "Delivery")
                        //{
                        //    sale("", lblordertype.Text);
                        //    updatedelivery(saleid);
                        //    recalsale(saleid.ToString());
                        //}
                        //if (lblordertype.Text == "Take Away")
                        //{
                        //    sale("", lblordertype.Text);

                        //    updatetakeaway(saleid);
                        //    recalsale(saleid.ToString());
                        //}
                        //if (lblordertype.Text == "Din In")
                        //{
                        //    sale("", lblordertype.Text);

                        //    updateDinin(saleid);
                        //    recalsale(saleid.ToString());
                        //}

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
                //lblordertype.Text = "Take Away";
                POSRetail.Sale.CustomerId ob = new CustomerId(this);
                ob.id = userid;
                this.Enabled = false;
                ob.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        public void updatedelivery(int salid)
        {
            try
            {
                string q = "update delivery set Saleid='" + salid + "' where id='" + deliveryid + "'";
                objCore.executeQuery(q);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void updatetakeaway(int salid)
        {
            try
            {
                string q = "update TakeAway set Saleid='" + salid + "' where id='" + takeawayid + "'";
                objCore.executeQuery(q);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void updateDinin(int salid)
        {
            try
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
                string q = "insert into DinInTables (id,TableNo,Saleid,Date,time,Status) values('" + id + "','" + tableno + "','" + salid + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','Pending')";
                objCore.executeQuery(q);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void vButton12_Click(object sender, EventArgs e)
        {
            try
            {

                //if (lblordertype.Text != "not Selected")
                //{
                //    if (dataGridView1.Rows.Count > 0)
                //    {
                //        if (lblordertype.Text == "Delivery")
                //        {
                //            sale("", lblordertype.Text);
                //            updatedelivery(saleid);
                //        }
                //        if (lblordertype.Text == "Take Away")
                //        {
                //            sale("", lblordertype.Text);

                //            updatetakeaway(saleid);
                //        }
                //        if (lblordertype.Text == "Din In")
                //        {
                //            sale("", lblordertype.Text);

                //            updateDinin(saleid);
                //        }

                //        //lblordertype.Text = "Not Selected";
                //        dt.Rows.Clear();
                //        dataGridView1.Refresh();
                //        txtcashrecvd.Text = "0";
                //        txttotal.Text = "0";
                //        txtnettotal.Text = "0";
                //        txtchange.Text = "0";
                //        editsale = string.Empty;
                //        recalsale(saleid.ToString());
                //    }


                //}
                //else
                //{
                //    MessageBox.Show("Order Type Not Seleted");
                //}

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void bindreportkitchen(string sid)
        {
            try
            {
                //if (vBtnkot.Enabled==true)
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
                                //if (lblordertype.Text == "Din In")
                                {
                                    tb = tableno;
                                }
                               // dtrpt.Rows.Add((dr.Cells["Qty"].Value.ToString()), dr.Cells["Item"].Value.ToString(), cashr, cname, caddress, cphone, siid, date, DateTime.Now.ToShortTimeString(), lblordertype.Text,tb);

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
                        //if (lblordertype.Text == "Delivery")
                        //{
                        //    sale("", lblordertype.Text);
                        //    updatedelivery(saleid);
                        //    recalsale(saleid.ToString());
                        //}
                        //if (lblordertype.Text == "Take Away")
                        //{
                        //    sale("", lblordertype.Text);

                        //    updatetakeaway(saleid);
                        //    recalsale(saleid.ToString());
                        //}
                        //if (lblordertype.Text == "Din In")
                        //{
                        //    sale("", lblordertype.Text);

                        //    updateDinin(saleid);
                        //    recalsale(saleid.ToString());
                        //}

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
               // lblordertype.Text = "Delivery";
                POSRetail.Sale.Delivery ob = new Delivery(this);
                this.Enabled = false;
                ob.id = userid;
                ob.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            if (txtdiscount.Enabled == false)
            {
                POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
                ob.editmode = "Discount";
                //this.Enabled = false;
                ob.Show();
            }
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
                        //if (lblordertype.Text == "Delivery")
                        //{
                        //    sale("", lblordertype.Text);
                        //    updatedelivery(saleid);
                        //    recalsale(saleid.ToString());
                        //}
                        //if (lblordertype.Text == "Take Away")
                        //{
                        //    sale("", lblordertype.Text);

                        //    updatetakeaway(saleid);
                        //    recalsale(saleid.ToString());
                        //}
                        //if (lblordertype.Text == "Din In")
                        //{
                        //    sale("", lblordertype.Text);

                        //    updateDinin(saleid);
                        //    recalsale(saleid.ToString());
                        //}

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
                //lblordertype.Text = "Din In";
                POSRetail.Sale.TableOrder ob = new TableOrder(this);
                this.Enabled = false;
                ob.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton14_Click(object sender, EventArgs e)
        {
            POSRetail.Sale.BillRecall ob = new BillRecall(this);
            ob.id = userid;
            ob.date = date;
            //this.Enabled = false;
            ob.Show();
        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            //lblordertype.Text = "Din In";
            POSRetail.Sale.TableOrder ob = new TableOrder(this);

            ob.Show();
        }

        private void vButton17_Click(object sender, EventArgs e)
        {
            POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            ob.editmode = "CashDrawer";
            
            ob.Show();
            

        }

        private void vButton19_Click(object sender, EventArgs e)
        {
            try
            {
                key.TopMost = false;
                bool chk = false;
                DataSet dsdayend = new DataSet();
                dsdayend = objCore.funGetDataSet("select id from sale where billstatus='pending' and date='" + date + "' and userid='" + userid + "'");
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
                else
                {
                    key.TopMost = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                        AllowDiscount ob = new AllowDiscount(this);
                        ob.id = userid;
                        ob.saleid = sdetailid.ToString();
                        ob.editmode = "VoidOne";
                        this.Enabled = false;
                        ob.Show();

                    }
                }

            }
            catch (Exception ex)
            {


            }
        }
        public void dayend(string uid, string status)
        {
            try
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
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void vBtnday_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("Are You Sure to Continue ?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
                ob.editmode = vBtnday.Text;
                ob.id = userid;
                ob.Show();
            }
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            ob.editmode = "Refund";
            ob.id = userid;
           // this.Enabled = false;
            ob.Show();
        }

        private void vButton18_Click(object sender, EventArgs e)
        {
            POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            ob.editmode = "SaleReport";
            ob.id = userid;
            ob.cashrr = cashr;
            ob.datee = date;
           // this.Enabled = false;
            ob.Show();
            //POSRetail.Reports.RptUserSale obj = new Reports.RptUserSale();
            //obj.cashiername = cashr;
            //obj.date = date;
            //obj.userid = userid;
            //obj.Show();
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
                

                if (txtcashrecvd.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtcashrecvd.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        return;
                    }
                }
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

        private void txtcashrecvd_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }

        private void txtdiscount_Enter(object sender, EventArgs e)
        {
            focusedTextbox = (TextBox)sender;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (focusedTextbox.Text != null)
            {
                focusedTextbox.Text = focusedTextbox.Text.Substring(0, focusedTextbox.Text.Length - 1);
                if (focusedTextbox.Text == string.Empty)
                {
                    focusedTextbox.Text = "0";
                }
            }
        }

        private void button51_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            focusedTextbox = null;
            txtdiscount.Text = "0";
            txtcashrecvd.Text = "0";
        }

        private void vButton13_Click(object sender, EventArgs e)
        {
           bindreportsample("Cash", saleid.ToString());
            //bindreport("Cash", saleid.ToString());
        }

        private void vButton11_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.Rows.Count;

                if (indx > 0)
                {
                    string type = dataGridView1.Rows[0].Cells[5].Value.ToString();
                    if (type == "Old")
                    {
                        AllowDiscount ob = new AllowDiscount(this);
                        ob.id = userid;
                        ob.saleid = saleid.ToString();
                        ob.editmode = "waste";
                        this.Enabled = false;
                        ob.Show();
                       

                    }
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void tableLayoutPanelmenugroup_Paint(object sender, PaintEventArgs e)
        {

        }
        public DataSet Fliprows(DataSet my_DataSet)
        {
            DataSet dsflip = new DataSet();

            foreach (DataTable dt in my_DataSet.Tables)
            {
                DataTable table = new DataTable();

                for (int i = 0; i <= dt.Rows.Count; i++)
                { table.Columns.Add(Convert.ToString(i)); }

                DataRow r;
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    r = table.NewRow();
                    r[0] = dt.Columns[k].ToString();
                    for (int j = 1; j <= dt.Rows.Count; j++)
                    { r[j] = dt.Rows[j - 1][k]; }
                    table.Rows.Add(r);
                }
                dsflip.Tables.Add(table);
            }

            return dsflip;
        }
        public void removetext()
        {
            try
            {
                int index = focusedTextbox.SelectionStart;
                focusedTextbox.Text = focusedTextbox.Text.Remove(focusedTextbox.SelectionStart - 1, 1);
                focusedTextbox.Select(index - 1, 1);
                focusedTextbox.Focus();
            }
            catch (Exception ex)
            {


            }
        }
        public void filltextbox(string text)
        {
            if (focusedTextbox != null)
            {

                {
                    focusedTextbox.Text = focusedTextbox.Text + text;
                }
                return;
            }
        }
        private void textBox1_TextChanged_2(object sender, EventArgs e)
        {
            

            
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            //getbarcode();
        }
        public void getbarcode(string bcd)
        {
            textBox4.Text = textBox4.Text.Trim();
            if (brcode == false)
            {
                return;
            }
            try
            {
                DataSet dsitem = new DataSet();

                dsitem = objCore.funGetDataSet("SELECT     dbo.Size.SizeName, dbo.Color.Caption, dbo.RawItem.* FROM         dbo.RawItem INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id where dbo.RawItem.Barcode='" + bcd + "'");
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                   // barcode = false;
                   // if (name == false || code == false)
                    {
                       // return;
                        //
                    }
                    //if (code == true)
                    //{
                    //    
                    //}
                    txtcode.Text = dsitem.Tables[0].Rows[0]["Code"].ToString();
                    txtprice.Text = dsitem.Tables[0].Rows[0]["price"].ToString();
                    txtname.Text = dsitem.Tables[0].Rows[0]["ItemName"].ToString();
                    itemid = dsitem.Tables[0].Rows[0]["id"].ToString();
                    size = dsitem.Tables[0].Rows[0]["SizeName"].ToString();
                    color = dsitem.Tables[0].Rows[0]["Caption"].ToString();
                    //txtitemquantity.Focus();
                    getpricetotal();
                    try
                    {
                        if (checkBox1.Checked != true)
                        {

                            {
                                callfillgrid();
                            }
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }
        public void getpricetotal()
        {
            if (txtitemquantity.Text == string.Empty)
            { }
            else
            {
                int Num;
                bool isNum = int.TryParse(txtdiscount.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Quantity Value. Only Nymbers are allowed");
                    return;
                }
            }
            try
            {
                txtitemtotal.Text = (Convert.ToInt32(txtitemquantity.Text) * Convert.ToInt32(txtprice.Text)).ToString();
            }
            catch (Exception ex)
            {


            }
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            getpricetotal();
        }

        private void txttotal_TextChanged_1(object sender, EventArgs e)
        {
            gettotal();
        }

        private void txtitemquantity_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
                if (focusedTextbox.Name != "textBox4")
                {
                    if (key.Visible == true)
                    { }
                    else
                    {
                        //   key.MdiParent = this;
                       // 
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel15_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {

            callfillgrid();
        }

        private void txtitemquantity_KeyDown(object sender, KeyEventArgs e)
        {
            //textBox4.Text = textBox4.Text.Trim();
            if (e.KeyCode == Keys.Enter)
            {
                callfillgrid();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (focusedTextbox != null)
                {
                    if (focusedTextbox.Name == "txtitemquantity")
                    {
                        callfillgrid();
                    }
                    
                    return;
                }


               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void textBox4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtitemquantity.Focus();
            }
        }
        public void getcode()
        {
            try
            {
                if (cd == false )
                {
                    return;
                }
                DataSet dsitem = new DataSet();

                dsitem = objCore.funGetDataSet("SELECT     dbo.Size.SizeName, dbo.Color.Caption, dbo.RawItem.* FROM         dbo.RawItem INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id where dbo.RawItem.Code='" + txtcode.Text.Trim() + "'");
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                   
                    

                    txtname.Text = dsitem.Tables[0].Rows[0]["ItemName"].ToString();
                    txtprice.Text = dsitem.Tables[0].Rows[0]["price"].ToString();
                    textBox4.Text = dsitem.Tables[0].Rows[0]["BarCode"].ToString();
                    itemid = dsitem.Tables[0].Rows[0]["id"].ToString();
                    size = dsitem.Tables[0].Rows[0]["SizeName"].ToString();
                    color = dsitem.Tables[0].Rows[0]["Caption"].ToString();
                    getpricetotal();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void txtcode_TextChanged(object sender, EventArgs e)
        {
            getcode();
        }

        private void txtnettotal_TextChanged(object sender, EventArgs e)
        {

        }
        public static string temp="";
        private void textBox4_KeyDown_1(object sender, KeyEventArgs e)
        {
            //temp = temp + e.KeyData.ToString();
            if (e.KeyCode == Keys.Enter)
            {
                getbarcode(textBox4.Text.Trim());
            }
            //MessageBox.Show(+  e.KeyValue.ToString());
        }
        public void getname()
        {
            try
            {
                if (nm == false)
                {
                    return;
                }
                DataSet dsitem = new DataSet();

                dsitem = objCore.funGetDataSet("SELECT     dbo.Size.SizeName, dbo.Color.Caption, dbo.RawItem.* FROM         dbo.RawItem INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id where dbo.RawItem.ItemName='" + txtname.Text.Trim() + "'");
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    name = false;
                   // if (barcode == false || code == false)
                    {
                     //   return;
                        //
                    }

                    txtcode.Text = dsitem.Tables[0].Rows[0]["Code"].ToString();
                    textBox4.Text = dsitem.Tables[0].Rows[0]["BarCode"].ToString();
                    txtprice.Text = dsitem.Tables[0].Rows[0]["price"].ToString();

                    itemid = dsitem.Tables[0].Rows[0]["id"].ToString();
                    size = dsitem.Tables[0].Rows[0]["SizeName"].ToString();
                    color = dsitem.Tables[0].Rows[0]["Caption"].ToString();
                    getpricetotal();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void txtname_TextChanged(object sender, EventArgs e)
        {

            getname();
        }
        public bool brcode = false;
        public bool nm = false;
        public bool cd = false;
        public void changequantity(string quantity)
        {
            try
            {
                txtitemquantity.Text = quantity;
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void vButton6_Click_1(object sender, EventArgs e)
        {
            try
            {
                if (key.Visible == true)
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                
                
            }
            try
            {
                key = new forms.KeyBoard(this);
                key.Show();
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void vButton7_Click_1(object sender, EventArgs e)
        {
            dt.Rows.Clear();
            dataGridView1.DataSource = dt;
            clear();
        }

        private void lblgstt_Click(object sender, EventArgs e)
        {

        }

        private void txtname_Leave(object sender, EventArgs e)
        {
            getname();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            //textBox4.Text = textBox4.Text + e.KeyChar.ToString().Trim();
            getbarcode(textBox4.Text.Trim());
        }
        public void keyboradbarcode()
        {
            try
            {
                if (focusedTextbox.Name == "textBox4")
                {
                    getbarcode(textBox4.Text.Trim());
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void textBox4_Enter(object sender, EventArgs e)
        {
            focusedTextbox = textBox4; ;
            brcode = true;
            nm = false;
            cd = false;
        }

        private void txtname_Enter(object sender, EventArgs e)
        {
            focusedTextbox = txtname;
            brcode = false;
            cd = false;
            nm = true;
        }

        private void txtcode_Enter(object sender, EventArgs e)
        {
            focusedTextbox = txtcode;
            brcode = false;
            nm = false;
            cd = true;
        }

        private void vButton8_Click_1(object sender, EventArgs e)
        {
            POSRetail.Sale.AllowDiscount ob = new AllowDiscount(this);
            ob.editmode = "Exchange";
            ob.id = userid;
            // this.Enabled = false;
            ob.Show();
        }
    }
}
