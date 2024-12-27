using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using VIBlend.WinForms.Controls.Properties;
using VIBlend;
using VIBlend.Utilities;
using System.IO;
namespace POSRestaurant.Sale
{
    public partial class KDSNEWupdated : Form
    {
        public string date = "",kdsid="",terminal="";
        public int no = 0;
        DataTable dtrpt = new DataTable();
        DataSet dstable = new DataSet();
        public int old = 0, old1 = 0;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public KDSNEWupdated()
        {
            InitializeComponent();
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
        public void addcolumn()
        {
            try
            {
                string q = "ALTER TABLE dbo.sale ADD Deliverystatus VARCHAR(20) NULL ;  ";
                objCore.executeQuery(q);
                q = "ALTER TABLE dbo.sale  ADD CONSTRAINT Deliverystatus_def   DEFAULT 'Pending' FOR Deliverystatus ;";
                objCore.executeQuery(q);
                q = "ALTER TABLE dbo.saledetails ADD OrderStatusmain VARCHAR(20) NULL ;  ";
                objCore.executeQuery(q);
                q = "ALTER TABLE dbo.saledetails  ADD CONSTRAINT OrderStatusmain_def   DEFAULT 'Pending' FOR OrderStatusmain ;";
                objCore.executeQuery(q);
            }
            catch (Exception ex)
            {


            }
        }
        int top = 12;
        int showrows = 0;
        public void fillpending(string refresh)
        {
            showrows = 0;
            try
            {
                top = columncount * 2;
            }
            catch (Exception ex)
            {
               
            }
            DataSet ds = new DataSet();
            string q = "select * from sale where OrderStatus='Pending' and date='" + date + "' ";
            
            if (terminal.Trim() != "")
            {
                q = "SELECT DISTINCT TOP " + top + "  dbo.Saledetails.saleid as id, dbo.Sale.Customer, dbo.Sale.time, dbo.Sale.terminal, dbo.Sale.TerminalOrder FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN              dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE ((dbo.Saledetails.OrderStatusmain = 'Pending') and dbo.Sale.Terminal='" + terminal + "') or ((dbo.Saledetails.Orderstatus = 'Pending') and dbo.MenuItem.KDSId = '" + kdsid + "') ORDER BY dbo.Saledetails.saleid";
                q = "SELECT DISTINCT TOP " + top + "  dbo.Sale.id as id, dbo.Sale.Customer, dbo.Sale.time, dbo.Sale.terminal, dbo.Sale.TerminalOrder FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN              dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE ((dbo.Saledetails.OrderStatusmain = 'Pending') and dbo.Sale.Terminal='" + terminal + "') or ((dbo.Saledetails.Orderstatus = 'Pending') and dbo.MenuItem.KDSId = '" + kdsid + "') ORDER BY dbo.Sale.id";

            }
            else
            {
                q = "SELECT DISTINCT TOP " + top + "  dbo.Saledetails.saleid as id, dbo.Sale.Customer, dbo.Sale.time, dbo.Sale.terminal, dbo.Sale.TerminalOrder FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN              dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE (dbo.Saledetails.Orderstatus = 'Pending') AND (dbo.MenuItem.KDSId = '" + kdsid + "') ORDER BY dbo.Saledetails.saleid";

            }
             ds = objCore.funGetDataSet(q);
            if (refresh == "yes")
            {
                old = ds.Tables[0].Rows.Count;
                dstable = new DataSet();
            }
            else
            {
                if (ds.Tables[0].Rows.Count != old)
                {
                    old = ds.Tables[0].Rows.Count;
                    dstable = new DataSet();
                }
                else
                {
                    return;
                }
            }
            tblmain.Controls.Clear();
            tblmain.ColumnStyles.Clear();
            tblmain.RowStyles.Clear();
            tblmain.SuspendLayout();
            tblmain.HorizontalScroll.Value = scroll;
            tblmain.PerformLayout();
            tblmain.ResumeLayout();
            string timediff = "";
            
            //tblmain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            int k = 0;
            if (ds.Tables[0].Rows.Count < 4)
            {
                k = 4;
            }
            else
            {
                k = ds.Tables[0].Rows.Count;
            }
            tblmain.ColumnCount = columncount;
            tblmain.RowCount = 2;
            tblmain.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            tblmain.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
            for (int i = 0; i < k; i++)
            {
                tblmain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            }
            tblmain.Location = new Point(0, 0);
            int rows = 0, colmns = 0;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                timediff = "";
                try
                {
                    DateTime time = Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString());
                    TimeSpan tm = DateTime.Now - time;
                    timediff = tm.ToString();
                    string[] Split = timediff.ToString().Split('.');
                    timediff = Split[0];
                }
                catch (Exception ex)
                {
                    
                }
                //tblmain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));               
                TableLayoutPanel tbl = new TableLayoutPanel();
                typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tbl, new object[] { true });
                tbl.Dock = System.Windows.Forms.DockStyle.Fill;
                tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 30));
                tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 70));
                vButton btn = new vButton();
                btn.Click += new EventHandler(button2_Click);
                btn.Font = new Font("", 14, FontStyle.Bold);
                btn.Dock = System.Windows.Forms.DockStyle.Fill;
                
                btn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                DataGridView dg = new DataGridView();
                int width = dg.Width;
                dg.Width = tbl.Width;
                dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
               // dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dg.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                //dg.DataBindingComplete += new EventHandler(dataGridView_DataBindingComplete);
                using (Font font = new Font(dg.DefaultCellStyle.Font.FontFamily, fontt, FontStyle.Regular))
                {
                    dg.RowsDefaultCellStyle.Font = font;
                }
                DataTable dt = new DataTable();
                dt.TableName = i.ToString();
                dt = getdata(ds.Tables[0].Rows[i]["id"].ToString());
                //if (ds.Tables[0].Rows[i]["OrderStatus"].ToString() == "Completed")
                //{
                //    dg.BackgroundColor = Color.Green;
                //}
                //else
                //{
                //    dg.BackgroundColor = Color.Yellow;
                //}
                //DataTable dt2 = new DataTable();
                //dt2 = dt.Copy();
                dstable.Tables.Add(dt.Copy());
                dg.DataSource = dstable.Tables[i];
                dg.Dock = System.Windows.Forms.DockStyle.Fill;
                int g =dg.Columns.Count;                
                dg.BackgroundColor = Color.White;
                dg.Dock = System.Windows.Forms.DockStyle.Fill;
                btn.Text = ds.Tables[0].Rows[i]["TerminalOrder"].ToString() + "  Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + ": " + timediff;
                btn.TextWrap = true;
                tbl.Controls.Add(btn, 0, 0);
                tbl.Controls.Add(dg, 0, 1);
                if (colmns > columncount)
                {
                    colmns = 0;
                    showrows = 1;
                    //tblmain.Controls.Add(tbl, colmns, 1);
                    
                }
                else
                {
                    //tblmain.Controls.Add(tbl, colmns, 0);
                    
                }

                tblmain.Controls.Add(tbl, colmns, showrows);
                colmns++;
                typeof(vButton).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, btn, new object[] { true });
            }
            
        }
        int columncount = 6; float fontt = 10;
        //public void fillcompleted()
        //{
        //    DataSet ds = new DataSet();
        //    string q = "select * from sale where OrderStatus='Completed' and date='" + date + "'";
        //    ds = objCore.funGetDataSet(q);
        //    if (ds.Tables[0].Rows.Count != old1)
        //    {
        //        old1 = ds.Tables[0].Rows.Count;
        //    }
        //    else
        //    {
        //        return;
        //    }
        //    tblmain2.Controls.Clear();
        //    tblmain2.ColumnStyles.Clear();
        //    tblmain2.RowStyles.Clear();            
        //    tblmain2.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        //    int k = 0;
        //    if (ds.Tables[0].Rows.Count < 4)
        //    {
        //        k = 4;
        //    }
        //    else
        //    {
        //        k = ds.Tables[0].Rows.Count;
        //    }
        //    tblmain2.ColumnCount = k;
        //    tblmain2.RowCount = 1;
        //    for (int i = 0; i < k; i++)
        //    {
        //        tblmain2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300));
        //    }
        //    tblmain2.Location = new Point(0, 0);
        //    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //    {
        //        //tblmain.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 50));               
        //        TableLayoutPanel tbl = new TableLayoutPanel();
        //        typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tbl, new object[] { true });
        //        tbl.Dock = System.Windows.Forms.DockStyle.Fill;
        //        tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 20));
        //        tbl.RowStyles.Add(new RowStyle(SizeType.Percent, 80));
        //        vButton btn = new vButton();
                
        //        btn.Font = new Font("", 14, FontStyle.Bold);
        //        btn.Dock = System.Windows.Forms.DockStyle.Fill;
        //        btn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                
        //        DataGridView dg = new DataGridView();
        //       //dg.ScrollBars=DataGridView
        //       // dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        //        dg.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView1_DataBindingComplete);
        //        using (Font font = new Font(dg.DefaultCellStyle.Font.FontFamily, 14, FontStyle.Bold))
        //        {
        //            dg.RowsDefaultCellStyle.Font = font;
        //        }
        //        DataTable dt = new DataTable();
        //        dt.TableName = i.ToString();
        //        dt = getdata(ds.Tables[0].Rows[i]["id"].ToString());
        //        dg.BackgroundColor = Color.Green;
        //        //if (ds.Tables[0].Rows[i]["OrderStatus"].ToString() == "Completed")
        //        //{
        //        //    dg.BackgroundColor = Color.Green;
        //        //}
        //        //else
        //        //{
        //        //    dg.BackgroundColor = Color.Yellow;
        //        //}
        //        dstable.Tables.Add(dt.Copy());
        //        dg.DataSource = dstable.Tables[i];
        //        //dg.Columns[2].Visible = false;
        //        //dg.Columns[3].Visible = false;
        //        //dg.Columns[4].Visible = false;
        //        //dg.Columns[5].Visible = false;
        //        //dg.Columns[6].Visible = false;
        //        //dg.Columns[7].Visible = false;
        //        //dg.Columns[8].Visible = false;
        //        dg.BackgroundColor = Color.White;
        //        dg.Dock = System.Windows.Forms.DockStyle.Fill;
        //        btn.Text = "Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " "+ ds.Tables[0].Rows[i]["Customer"].ToString();
        //        tbl.Controls.Add(btn, 0, 0);
        //        tbl.Controls.Add(dg, 0, 1);
        //        tblmain2.Controls.Add(tbl, i, 0);                                 
        //        //tblmain.Width = 300 * ds.Tables[0].Rows.Count;
        //    }
        //}
        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridView dg = sender as DataGridView;
                dg.ForeColor = Color.Blue;
                dg.ClearSelection();
                dg.Dock = System.Windows.Forms.DockStyle.Fill;
                int width = dg.Width;
                dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dg.AllowUserToAddRows = false;
                dg.AllowUserToDeleteRows = false;
                dg.AllowUserToOrderColumns = false;
                dg.ReadOnly = true;
                DataGridViewColumn column = dg.Columns[0];
                int wid = width / 2;
                column.Width = (width / 2) - (wid / 3);
                dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //DataGridViewColumn column1 = dg.Columns[1];
                //column1.Width = (30);
                if (dg.Rows[0].Cells["alarmtime"].Value.ToString().Contains("-"))
                {
                    dg.BackgroundColor = Color.Red;
                }
                else
                {
                    dg.BackgroundColor = Color.Yellow;
                }
                dg.Columns[2].Visible = false;
                dg.Columns[3].Visible = false;
                dg.Columns[4].Visible = false;
                dg.Columns[5].Visible = false;
                dg.Columns[6].Visible = false;
                dg.Columns[7].Visible = false;
                dg.Columns[8].Visible = false;
                foreach (DataGridViewRow dr in dg.Rows)
                {
                    double prc = 0;
                    string temp = dr.Cells["QTY"].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    if (Convert.ToDouble(temp) < 0)
                    {
                        dr.DefaultCellStyle.BackColor = Color.Gray;
                    }
                    dr.Height = 50;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            DataGridView dg = sender as DataGridView;
            int width = dg.Width;
            DataGridViewColumn column = dg.Columns[0];
            column.Width = width - (width / 5);
            DataGridViewColumn column1 = dg.Columns[1];
            column1.Width = (width / 5);
            //if (dg.Rows[0].Cells["alarmtime"].Value.ToString().Contains("-"))
            {
                dg.BackgroundColor = Color.Green;
            }
            dg.Columns[2].Visible = false;
            dg.Columns[3].Visible = false;
            dg.Columns[4].Visible = false;
            dg.Columns[5].Visible = false;
            dg.Columns[6].Visible = false;
            dg.Columns[7].Visible = false;
            dg.Columns[8].Visible = false;
            foreach (DataGridViewRow dr in dg.Rows)
            {
                double prc = 0;
                string temp = dr.Cells["QTY"].Value.ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                if (Convert.ToDouble(temp) < 0)
                {
                    dr.DefaultCellStyle.BackColor = Color.Gray;
                }
                dr.Height = 40;
            }
        }
        private void KDSNEW_Load(object sender, EventArgs e)
        {
          
            try
            {
                comboBox1.Text = "4";
                comboBox2.Text = "12";
            }
            catch (Exception ex)
            {
                
            }
            try
            {
               // comboBox1.Text = "4";
                comboBox2.Text = "12";
            }
            catch (Exception ex)
            {

            }
            try
            {
                columncount = Convert.ToInt32(comboBox1.Text);
            }
            catch (Exception ex)
            {
                
            }
            try
            {
                fontt = float.Parse(comboBox2.Text);
            }
            catch (Exception ex)
            {

            }
            addcolumn();
            try
            {
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

                //MessageBox.Show(ex.Message);
            }
            getcompany();
            typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tblmain, new object[] { true });
            //typeof(Panel).InvokeMember("DoubleBuffered", System.Reflection.BindingFlags.SetProperty | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic, null, tblmain2, new object[] { true });
            try
            {
               DataSet dsgst = new DataSet();
                dsgst = objCore.funGetDataSet("select top(1) * from DayEnd  order by id desc");
                if (dsgst.Tables[0].Rows.Count > 0)
                {
                    date = dsgst.Tables[0].Rows[0]["Date"].ToString();
                    
                }
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            tblmain.Height = (tableLayoutPanel1.Height) - 100;
            tblmain.Width = tableLayoutPanel1.Width;
            //tblmain2.Height = (tableLayoutPanel1.Height / 2) - 30;
            //tblmain2.Width = tableLayoutPanel1.Width;
            tblmain.Location = new Point(0, 0);
            //tblmain2.Location = new Point(0, 0);
            dtrpt = new DataTable();
            dtrpt.Columns.Add("ItemName", typeof(string));
            dtrpt.Columns.Add("QTY", typeof(string));            
            dtrpt.Columns.Add("time", typeof(string));
            dtrpt.Columns.Add("alarmtime", typeof(string));
            dtrpt.Columns.Add("timecolor", typeof(string));
            dtrpt.Columns.Add("alarmcolor", typeof(string));
            dtrpt.Columns.Add("id", typeof(string));
            dtrpt.Columns.Add("sid", typeof(string));
            dtrpt.Columns.Add("type", typeof(string));
            tblmain.Controls.Clear();
            //Clear out the existing row and column styles
            fillpending("yes");
            //fillcompleted();
            
        }
        public DataTable getdata(string id)
        {
            dtrpt.Clear();
            DataSet ds = new DataSet();
            string q = "";
            if (terminal.Trim() != "")
            {
                q = "SELECT     dbo.Saledetails.Orderstatus,dbo.sale.Terminal,dbo.Sale.id,dbo.Sale.time,dbo.MenuItem.Name,dbo.MenuItem.Minutes,dbo.MenuItem.alarmtime,dbo.MenuItem.minuteskdscolor,dbo.MenuItem.alarmkdscolor, dbo.Saledetails.id as sid,dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId,dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.Quantity,dbo.Saledetails.comments, dbo.MenuItem.KDSId, dbo.Sale.OrderType FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id where  dbo.Sale.id='" + id + "' and  (dbo.Saledetails.OrderStatusmain = 'Pending')  order by dbo.Saledetails.id";
            
            }
            else
            {
                q = "SELECT     dbo.Saledetails.Orderstatus,dbo.sale.Terminal,dbo.Sale.id,dbo.Sale.time,dbo.MenuItem.Name,dbo.MenuItem.Minutes,dbo.MenuItem.alarmtime,dbo.MenuItem.minuteskdscolor,dbo.MenuItem.alarmkdscolor, dbo.Saledetails.id as sid,dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId,dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.Quantity,dbo.Saledetails.comments, dbo.MenuItem.KDSId, dbo.Sale.OrderType FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id where  dbo.Sale.id='" + id + "' and  (dbo.Saledetails.Orderstatus = 'Pending') and dbo.MenuItem.KDSId='" + kdsid + "' order by dbo.Saledetails.id";            
            }            
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {

                int j = 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (terminal == "")
                    {
                        float mnts = 0, alarmmnts = 0;
                        string mts = "", alarmmts = "";
                        try
                        {
                            try
                            {
                                mnts = float.Parse(ds.Tables[0].Rows[i]["Minutes"].ToString());
                            }
                            catch (Exception ex)
                            {
                            }
                            TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-mnts));
                            mts = span.ToString();
                            string[] Split = mts.ToString().Split('.');
                            mts = Split[0];
                        }
                        catch (Exception ex)
                        {
                        }
                        try
                        {
                            try
                            {
                                alarmmnts = float.Parse(ds.Tables[0].Rows[i]["alarmtime"].ToString());
                            }
                            catch (Exception ex)
                            {
                            }
                            TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-(alarmmnts + mnts)));
                            // mnts = span.Minutes;
                            alarmmts = span.ToString();
                            string[] Split = alarmmts.ToString().Split('.');
                            alarmmts = Split[0];
                        }
                        catch (Exception ex)
                        {
                        }
                        string runtime = ds.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                        if (runtime == "0")
                        {
                            runtime = string.Empty;
                        }
                        string flavourid = ds.Tables[0].Rows[i]["Flavourid"].ToString();
                        if (flavourid == "0")
                        {
                            flavourid = string.Empty;
                        }
                        if (flavourid != string.Empty)
                        {
                            DataSet dsflr = new DataSet();
                            q = "SELECT     Id, MenuItemId, name, price FROM  ModifierFlavour where id='" + flavourid + "'";
                            dsflr = objCore.funGetDataSet(q);
                            if (dsflr.Tables[0].Rows.Count > 0)
                            {
                                dtrpt.Rows.Add(dsflr.Tables[0].Rows[0]["name"].ToString().Substring(0, 1) + "'" + ds.Tables[0].Rows[i]["name"].ToString() + "-" + ds.Tables[0].Rows[i]["comments"].ToString() , ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                            }
                        }
                        string mdid = ds.Tables[0].Rows[i]["ModifierId"].ToString();
                        if (mdid == "0")
                        {
                            mdid = string.Empty;
                        }
                        if (runtime == string.Empty)
                        {
                            if (mdid != string.Empty)
                            {
                                DataSet dsmd = new DataSet();
                                q = "SELECT     Name FROM         Modifier where id='" + mdid + "'";
                                dsmd = objCore.funGetDataSet(q);
                                if (dsmd.Tables[0].Rows.Count > 0)
                                {
                                    dtrpt.Rows.Add("  "+dsmd.Tables[0].Rows[0]["name"].ToString() + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                }
                            }
                        }
                        else
                        {
                            DataSet dsmd = new DataSet();
                            q = "SELECT     Name FROM         RuntimeModifier where id='" + runtime + "'";
                            dsmd = objCore.funGetDataSet(q);
                            if (dsmd.Tables[0].Rows.Count > 0)
                            {
                                dtrpt.Rows.Add("   " + dsmd.Tables[0].Rows[0]["name"].ToString() + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                            }
                        }
                        if (flavourid != string.Empty || mdid != string.Empty || runtime != string.Empty)
                        {
                        }
                        else
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["name"].ToString().Replace("Open Item", "") + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                            j++;
                        }
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[i]["Terminal"].ToString().ToLower() == terminal.ToLower() || ds.Tables[0].Rows[i]["KDSId"].ToString() == kdsid)
                        {
                            float mnts = 0, alarmmnts = 0;
                            string mts = "", alarmmts = "";
                            try
                            {
                                try
                                {
                                    mnts = float.Parse(ds.Tables[0].Rows[i]["Minutes"].ToString());
                                }
                                catch (Exception ex)
                                {
                                }
                                TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-mnts));
                                mts = span.ToString();
                                string[] Split = mts.ToString().Split('.');
                                mts = Split[0];
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                try
                                {
                                    alarmmnts = float.Parse(ds.Tables[0].Rows[i]["alarmtime"].ToString());
                                }
                                catch (Exception ex)
                                {
                                }
                                TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-(alarmmnts + mnts)));
                                // mnts = span.Minutes;
                                alarmmts = span.ToString();
                                string[] Split = alarmmts.ToString().Split('.');
                                alarmmts = Split[0];
                            }
                            catch (Exception ex)
                            {

                            }
                            string runtime = ds.Tables[0].Rows[i]["RunTimeModifierId"].ToString();
                            if (runtime == "0")
                            {
                                runtime = string.Empty;
                            }
                            string flavourid = ds.Tables[0].Rows[i]["Flavourid"].ToString();
                            if (flavourid == "0")
                            {
                                flavourid = string.Empty;
                            }
                            if (flavourid != string.Empty)
                            {
                                DataSet dsflr = new DataSet();
                                q = "SELECT     Id, MenuItemId, name, price FROM  ModifierFlavour where id='" + flavourid + "'";
                                dsflr = objCore.funGetDataSet(q);
                                if (dsflr.Tables[0].Rows.Count > 0)
                                {
                                    dtrpt.Rows.Add(dsflr.Tables[0].Rows[0]["name"].ToString().Substring(0, 1) + "'" + ds.Tables[0].Rows[i]["name"].ToString() + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                }
                            }
                            string mdid = ds.Tables[0].Rows[i]["ModifierId"].ToString();
                            if (mdid == "0")
                            {
                                mdid = string.Empty;
                            }
                            if (runtime == string.Empty)
                            {
                                if (mdid != string.Empty)
                                {
                                    DataSet dsmd = new DataSet();
                                    q = "SELECT     Name FROM         Modifier where id='" + mdid + "'";
                                    dsmd = objCore.funGetDataSet(q);
                                    if (dsmd.Tables[0].Rows.Count > 0)
                                    {
                                        dtrpt.Rows.Add(dsmd.Tables[0].Rows[0]["name"].ToString() + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                    }
                                }
                            }
                            else
                            {
                                DataSet dsmd = new DataSet();
                                q = "SELECT     Name FROM         RuntimeModifier where id='" + runtime + "'";
                                dsmd = objCore.funGetDataSet(q);
                                if (dsmd.Tables[0].Rows.Count > 0)
                                {
                                    dtrpt.Rows.Add("   " + dsmd.Tables[0].Rows[0]["name"].ToString() + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                }
                            }
                            if (flavourid != string.Empty || mdid != string.Empty || runtime != string.Empty)
                            {
                            }
                            else
                            {
                                dtrpt.Rows.Add(ds.Tables[0].Rows[i]["name"].ToString().Replace("Open Item", "") + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                j++;
                            }
                        }
                    }
                }
            }


            ds = new DataSet();
            q = "SELECT     dbo.Saledetailsrefund.Orderstatus,dbo.sale.Terminal,dbo.Sale.id,dbo.Sale.time,dbo.MenuItem.Name,dbo.MenuItem.Minutes,dbo.MenuItem.alarmtime,dbo.MenuItem.minuteskdscolor,dbo.MenuItem.alarmkdscolor, dbo.Saledetailsrefund.id as sid,dbo.Saledetailsrefund.Flavourid, dbo.Saledetailsrefund.ModifierId, dbo.Saledetailsrefund.Quantity,dbo.Saledetailsrefund.comments, dbo.MenuItem.KDSId, dbo.Sale.OrderType FROM         dbo.Sale INNER JOIN                      dbo.Saledetailsrefund ON dbo.Sale.Id = dbo.Saledetailsrefund.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetailsrefund.MenuItemId = dbo.MenuItem.Id where  dbo.Sale.id='" + id + "'   order by dbo.Saledetailsrefund.id";            

            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {

                int j = 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (terminal == "")
                    {
                        float mnts = 0, alarmmnts = 0;
                        string mts = "", alarmmts = "";
                        try
                        {
                            try
                            {
                                mnts = float.Parse(ds.Tables[0].Rows[i]["Minutes"].ToString());
                            }
                            catch (Exception ex)
                            {
                            }
                            TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-mnts));
                            mts = span.ToString();
                            string[] Split = mts.ToString().Split('.');
                            mts = Split[0];
                        }
                        catch (Exception ex)
                        {
                        }
                        try
                        {
                            try
                            {
                                alarmmnts = float.Parse(ds.Tables[0].Rows[i]["alarmtime"].ToString());
                            }
                            catch (Exception ex)
                            {
                            }
                            TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-(alarmmnts + mnts)));
                            // mnts = span.Minutes;
                            alarmmts = span.ToString();
                            string[] Split = alarmmts.ToString().Split('.');
                            alarmmts = Split[0];
                        }
                        catch (Exception ex)
                        {
                        }
                        string flavourid = ds.Tables[0].Rows[i]["Flavourid"].ToString();
                        if (flavourid == "0")
                        {
                            flavourid = string.Empty;
                        }
                        if (flavourid != string.Empty)
                        {
                            DataSet dsflr = new DataSet();
                            q = "SELECT     Id, MenuItemId, name, price FROM  ModifierFlavour where id='" + flavourid + "'";
                            dsflr = objCore.funGetDataSet(q);
                            if (dsflr.Tables[0].Rows.Count > 0)
                            {
                                dtrpt.Rows.Add(dsflr.Tables[0].Rows[0]["name"].ToString().Substring(0, 1) + "'" + ds.Tables[0].Rows[i]["name"].ToString()+"-"+ds.Tables[0].Rows[i]["comments"].ToString(), "-"+ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                            }
                        }
                        string mdid = ds.Tables[0].Rows[i]["ModifierId"].ToString();
                        if (mdid == "0")
                        {
                            mdid = string.Empty;
                        }
                        if (mdid != string.Empty)
                        {
                            DataSet dsmd = new DataSet();
                            q = "SELECT     Name FROM         Modifier where id='" + mdid + "'";
                            dsmd = objCore.funGetDataSet(q);
                            if (dsmd.Tables[0].Rows.Count > 0)
                            {
                                dtrpt.Rows.Add(dsmd.Tables[0].Rows[0]["name"].ToString() + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), "-" + ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                            }
                        }
                        if (flavourid != string.Empty || mdid != string.Empty)
                        {
                        }
                        else
                        {
                            dtrpt.Rows.Add(ds.Tables[0].Rows[i]["name"].ToString().Replace("Open Item", "") + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), "-" + ds.Tables[0].Rows[i]["Quantity"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                            j++;
                        }
                    }
                    else
                    {
                        if (ds.Tables[0].Rows[i]["Terminal"].ToString().ToLower() == terminal.ToLower() || ds.Tables[0].Rows[i]["KDSId"].ToString() == kdsid)
                        {
                            float mnts = 0, alarmmnts = 0;
                            string mts = "", alarmmts = "";
                            try
                            {
                                try
                                {
                                    mnts = float.Parse(ds.Tables[0].Rows[i]["Minutes"].ToString());
                                }
                                catch (Exception ex)
                                {
                                }
                                TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-mnts));
                                mts = span.ToString();
                                string[] Split = mts.ToString().Split('.');
                                mts = Split[0];
                            }
                            catch (Exception ex)
                            {
                            }
                            try
                            {
                                try
                                {
                                    alarmmnts = float.Parse(ds.Tables[0].Rows[i]["alarmtime"].ToString());
                                }
                                catch (Exception ex)
                                {
                                }
                                TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-(alarmmnts + mnts)));
                                // mnts = span.Minutes;
                                alarmmts = span.ToString();
                                string[] Split = alarmmts.ToString().Split('.');
                                alarmmts = Split[0];
                            }
                            catch (Exception ex)
                            {
                            }
                            string flavourid = ds.Tables[0].Rows[i]["Flavourid"].ToString();
                            if (flavourid == "0")
                            {
                                flavourid = string.Empty;
                            }
                            if (flavourid != string.Empty)
                            {
                                DataSet dsflr = new DataSet();
                                q = "SELECT     Id, MenuItemId, name, price FROM  ModifierFlavour where id='" + flavourid + "'";
                                dsflr = objCore.funGetDataSet(q);
                                if (dsflr.Tables[0].Rows.Count > 0)
                                {
                                    dtrpt.Rows.Add(dsflr.Tables[0].Rows[0]["name"].ToString().Substring(0, 1) + "'" + ds.Tables[0].Rows[i]["name"].ToString()+"-" + ds.Tables[0].Rows[i]["comments"].ToString(), "-" + ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                }
                            }
                            string mdid = ds.Tables[0].Rows[i]["ModifierId"].ToString();
                            if (mdid == "0")
                            {
                                mdid = string.Empty;
                            }
                            if (mdid != string.Empty)
                            {
                                DataSet dsmd = new DataSet();
                                q = "SELECT     Name FROM         Modifier where id='" + mdid + "'";
                                dsmd = objCore.funGetDataSet(q);
                                if (dsmd.Tables[0].Rows.Count > 0)
                                {
                                    dtrpt.Rows.Add(dsmd.Tables[0].Rows[0]["name"].ToString() + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), "-" + ds.Tables[0].Rows[i]["Quantity"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                }
                            }
                            if (flavourid != string.Empty || mdid != string.Empty)
                            {
                            }
                            else
                            {
                                dtrpt.Rows.Add(ds.Tables[0].Rows[i]["name"].ToString().Replace("Open Item", "") + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), "-" + ds.Tables[0].Rows[i]["Quantity"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                j++;
                            }
                        }
                    }
                }
            }

            return dtrpt;
        }
        string[,] array;
        public void setlocation()
        {

            //int x = 3, y = 3;
            //int xval = 0, yval = 0;

            //array = new string[1, 2];
            //var loc = tableLayoutPanel1.PointToScreen(Point.Empty);
            //x = loc.X;
            //y = loc.Y;
            //array[0, 0] = "0";// x.ToString();
            //array[0, 1] = "0";// y.ToString();
            //int i = 0;

        }
        private void vHScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            int x = 0, y = 0, i = 0;
            //var loc = tblmain.PointToScreen(Point.Empty);
            //x = loc.X;
            //y = loc.Y;
            //x = Convert.ToInt32(array[0, 0]);
            //y = Convert.ToInt32(array[0, 1]);
            //x = 0;// x / 4;
            //int val = vHScrollBar1.Value;
            //x = x - val;
            //tblmain.Location = new Point(x, y);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
            int indx = btn.Text.IndexOf("!");
            indx = indx - 10;
            string id = btn.Name;// btn.Text.Substring(9, indx);
            string q = "";
            q = "SELECT dbo.Saledetails.Id, dbo.Sale.Terminal,dbo.MenuItem.kdsId FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE (dbo.Saledetails.saleid = '" + id + "') ";
           
            
            //if (kdsid == "1")
            //{
            //    q = "SELECT dbo.Saledetails.Id FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id WHERE (dbo.Saledetails.saleid = '" + id + "') ";
           
            //}
            //else
            //{
            //    q = "SELECT dbo.Saledetails.Id FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id WHERE (dbo.Saledetails.saleid = '" + id + "') AND (dbo.MenuItem.KDSId = '" + kdsid + "') ";
           
            //}

            DataTable dtgrid = new DataTable();

             DataSet dsd = new DataSet();
            dsd = objCore.funGetDataSet(q);

            for (int i = 0; i < dsd.Tables[0].Rows.Count; i++)
            {
                if (terminal == "")
                {
                    if (kdsid == dsd.Tables[0].Rows[i]["kdsid"].ToString().ToLower())
                    {
                        q = "update Saledetails set OrderStatus='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                        objCore.executeQuery(q);
                    }
                }
                else
                {
                    if (terminal.ToLower() == dsd.Tables[0].Rows[i]["Terminal"].ToString().ToLower())
                    {
                        if (kdsid == dsd.Tables[0].Rows[i]["kdsid"].ToString().ToLower())
                        {
                            q = "update Saledetails set OrderStatus='Completed',OrderStatusmain='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            q = "update Saledetails set OrderStatusmain='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                            objCore.executeQuery(q);
                        }
                    }
                    else
                    {
                        if (kdsid == dsd.Tables[0].Rows[i]["kdsid"].ToString().ToLower())
                        {
                            q = "update Saledetails set OrderStatus='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                            objCore.executeQuery(q);
                        }
                    }
                }
                //if (kdsid == "2")
                //{
                //    q = "update Saledetails set OrderStatus='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                //    objCore.executeQuery(q);
                //}
                //if (kdsid == "1")
                //{
                //    q = "update Saledetails set OrderStatus='Completed',OrderStatusmain='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                //    objCore.executeQuery(q);
                //}
            }
            fillpending("yes");
            //fillcompleted();
        }
        private void vHScrollBar1_Click(object sender, EventArgs e)
        {

        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            POSRestaurant.Sale.RestSale frm = new RestSale();
            forms.NewLogIn obj = new forms.NewLogIn(frm);
            obj.Show();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            fillpending("");
            //fillcompleted();
        }
        int scroll = 0;
        private void tblmain_Scroll(object sender, ScrollEventArgs e)
        {
            scroll = e.NewValue;           
        }

        private void tblmain_ControlAdded(object sender, ControlEventArgs e)
        {
            //tblmain.SuspendLayout();
            //tblmain.HorizontalScroll.Value = scroll;
            //tblmain.PerformLayout();
            //tblmain.ResumeLayout();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            KDSRecall obj = new KDSRecall();
            obj.no = no;
            obj.kdsid = kdsid;
            obj.date = date;
            obj.terminal = terminal;
            obj.Show();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            fillpending("yes");
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            fillpending("yes");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
        }

        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                columncount = Convert.ToInt32(comboBox1.Text);
            }
            catch (Exception ex)
            {

            }
            try
            {
                fontt = Convert.ToInt32(comboBox2.Text);
            }
            catch (Exception ex)
            {

            }
            fillpending("yes");
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                columncount = Convert.ToInt32(comboBox1.Text);
            }
            catch (Exception ex)
            {

            }
            try
            {
                fontt = Convert.ToInt32(comboBox2.Text);
            }
            catch (Exception ex)
            {

            }
            fillpending("yes");
        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
