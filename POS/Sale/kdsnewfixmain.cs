using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class kdsnewfixmain : Form
    {
        public kdsnewfixmain()
        {
            InitializeComponent();
        }
        public string park = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void kdsnewfix_Load(object sender, EventArgs e)
        {
            //btnpark1.Visible = false;
            //btnpark2.Visible = false;
            //btnpark3.Visible = false;
            //btnpark4.Visible = false;
            //btnpark5.Visible = false;
            //btnpark6.Visible = false;
            //btnpark7.Visible = false;
            //btnpark8.Visible = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            this.KeyPreview = true;
            //DataTable dt = new DataTable();
            //dt.Columns.Add("test");
            //dt.Columns.Add("test2");
            //dt.Rows.Add("test            test            eed","sss");
            //dt.Rows.Add("test            test            eed", "sss");
            //dt.Rows.Add("test            test            eed", "sss");
            //listBox1.DataSource = dt;
            //listBox1.DisplayMember = "test";
           
            try
            {
                DataSet dsgst = new DataSet();
                dsgst = objCore.funGetDataSet("select top(1) * from DayEnd where daystatus='open' order by id desc");
                if (dsgst.Tables[0].Rows.Count > 0)
                {
                    date = dsgst.Tables[0].Rows[0]["Date"].ToString();

                }

            }
            catch (Exception ex)
            {

              //  MessageBox.Show(ex.Message);
            }
           
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
            fillpending("yes");
        }
        private void dataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                DataGridView dg = sender as DataGridView;
           
                dg.ClearSelection();
                dg.Dock = System.Windows.Forms.DockStyle.Fill;
                int width = dg.Width;
                dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dg.AllowUserToAddRows = false;
                dg.AllowUserToDeleteRows = false;
                dg.AllowUserToOrderColumns = false;
                dg.ReadOnly = true;
                DataGridViewColumn column = dg.Columns[0];
                int wid = width / 4;
                column.Width = (width) - (wid);
                dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //DataGridViewColumn column1 = dg.Columns[1];
                //column1.Width = (30);

                //dg.Columns[2].Visible = false;
                //dg.Columns[3].Visible = false;
                //dg.Columns[4].Visible = false;
                //dg.Columns[5].Visible = false;
                //dg.Columns[6].Visible = false;
                //dg.Columns[7].Visible = false;
                //dg.Columns[8].Visible = false;
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
                   // dr.Height = 50;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void bindgrid(DataGridView dg)
        {
            try
            {                
                dg.ClearSelection();
                dg.ColumnHeadersVisible = false;
                dg.RowHeadersVisible = false;
                dg.Dock = System.Windows.Forms.DockStyle.Fill;
                int width = dg.Width;
                dg.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dg.AllowUserToAddRows = false;
                dg.AllowUserToDeleteRows = false;
                dg.AllowUserToOrderColumns = false;
                dg.ReadOnly = true;
                DataGridViewColumn column = dg.Columns[0];
                int wid = width / 4;
                column.Width = (width) - (wid);
                dg.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                column.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
                //DataGridViewColumn column1 = dg.Columns[1];
                //column1.Width = (30);

                //dg.Columns[2].Visible = false;
                //dg.Columns[3].Visible = false;
                //dg.Columns[4].Visible = false;
                //dg.Columns[5].Visible = false;
                //dg.Columns[6].Visible = false;
                //dg.Columns[7].Visible = false;
                //dg.Columns[8].Visible = false;
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
                    // dr.Height = 50;
                }
            }
            catch (Exception ex)
            {

            }
        }
        public string date = "", kdsid = "", terminal = "";
        public int no = 0;
        DataTable dtrpt = new DataTable();
        DataSet dstable = new DataSet();
        public int old = 0, old1 = 0;
       
        string top = "8";
        protected void update(string id, string group)
        {
            string q = "SELECT dbo.Saledetails.Id, dbo.Sale.Terminal,dbo.MenuItem.kdsId,dbo.Saledetails.ParkStatus FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE (dbo.Saledetails.saleid = '" + id + "') and (dbo.Saledetails.kdsgroup = '" + group + "') ";

            DataTable dtgrid = new DataTable();

            DataSet dsd = new DataSet();
            dsd = objCore.funGetDataSet(q);

            for (int i = 0; i < dsd.Tables[0].Rows.Count; i++)
            {
                if (park == "yes")
                {
                    if (dsd.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                    {
                        q = "update Saledetails set OrderStatus='Completed',OrderStatusmain='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                        objCore.executeQuery(q);
                        q = "update sale set DeliveredTime='" + DateTime.Now + "' where id='" + id + "'";
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        q = "update Saledetails set ParkStatus='Parked' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                        objCore.executeQuery(q);
                        
                    }
                }
                else
                {
                    q = "update Saledetails set OrderStatus='Completed',OrderStatusmain='Completed' where id='" + dsd.Tables[0].Rows[i]["id"] + "'";
                    objCore.executeQuery(q);
                    q = "update sale set DeliveredTime='" + DateTime.Now + "' where id='" + id + "'";
                    objCore.executeQuery(q);

                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;

                //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                int indx = btn.Text.IndexOf(":");
                int indx2 = btn.Text.IndexOf("!");//12:45!
                int length = indx2 - (indx+2);
                string group = btn.Name;
                string id = btn.Text.Substring((indx + 1), length);
                string q = "";
                update(id, group);
                //btnpark1.Visible = false;
                //btnpark2.Visible = false;
                //btnpark3.Visible = false;
                //btnpark4.Visible = false;
                //btnpark5.Visible = false;
                //btnpark6.Visible = false;
                //btnpark7.Visible = false;
                //btnpark8.Visible = false;

                fillpending("yes");
            }
            catch (Exception ex)
            {
                
                
            }
            //fillcompleted();
        }
        private void button2_Click(object sender, EventArgs e)
        {

        }
        public void fillpending(string refresh)
        {

           
            DataSet ds = new DataSet();
            string q = "";
            if (terminal == "")
            {
                q = "SELECT DISTINCT TOP " + top + "  dbo.Sale.id as id, dbo.Sale.Customer, dbo.sale.time, dbo.Sale.terminal, dbo.Sale.TerminalOrder, dbo.Saledetails.kdsgroup, dbo.Saledetails.ParkStatus FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN              dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE ((dbo.Saledetails.OrderStatusmain = 'Pending')  AND (dbo.Saledetails.kdsgroup > 0) )  ORDER BY dbo.Saledetails.kdsgroup";
          
            }
            else
            {
                q = "SELECT DISTINCT TOP " + top + "  dbo.Sale.id as id, dbo.Sale.Customer, dbo.sale.time, dbo.Sale.terminal, dbo.Sale.TerminalOrder, dbo.Saledetails.kdsgroup, dbo.Saledetails.ParkStatus FROM  dbo.Saledetails INNER JOIN               dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id INNER JOIN              dbo.Sale ON dbo.Saledetails.saleid = dbo.Sale.Id WHERE ((dbo.Saledetails.OrderStatusmain = 'Pending')  AND (dbo.Saledetails.kdsgroup > 0) and dbo.Sale.terminal='" + terminal + "')  ORDER BY dbo.Saledetails.kdsgroup";
          
            }
              ds = objCore.funGetDataSet(q);
            //if (refresh == "yes")
            //{
            //    old = ds.Tables[0].Rows.Count;
            //    dstable = new DataSet();
            //}
            //else
            //{
            //    if (ds.Tables[0].Rows.Count != old)
            //    {
            //        old = ds.Tables[0].Rows.Count;
            //        dstable = new DataSet();
            //    }
            //    else
            //    {
            //        return;
            //    }
            //}
            if (ds.Tables[0].Rows.Count != old)
            {
                if (ds.Tables[0].Rows.Count > old)
                {
                    old = ds.Tables[0].Rows.Count;
                    Console.Beep(5000, 1000);
                }
                else
                {
                    old = ds.Tables[0].Rows.Count;
                }
                dstable = new DataSet();
            }
            dataGridView1.DataSource = null;
            button1.Text = "";
            button1.BackColor = Color.White;
            dataGridView2.DataSource = null;
            button2.Text = "";
            button2.BackColor = Color.White;
            dataGridView3.DataSource = null;
            button3.Text = "";
            button3.BackColor = Color.White;
            dataGridView4.DataSource = null;
            button4.Text = "";
            button4.BackColor = Color.White;
            dataGridView5.DataSource = null;
            button5.Text = "";
            button5.BackColor = Color.White;
            dataGridView6.DataSource = null;
            button6.Text = "";
            button6.BackColor = Color.White; 
            dataGridView7.DataSource = null;
            button7.Text = "";
            button7.BackColor = Color.White;
            dataGridView8.DataSource = null;
            button8.Text = "";
            button8.BackColor = Color.White;
           

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


            try
            {
                int rows = 0, colmns = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string addon = "";

                    q = "select id from saledetails where saleid='" + ds.Tables[0].Rows[i]["id"].ToString() + "' and kdsgroup<'" + ds.Tables[0].Rows[i]["kdsgroup"].ToString() + "' ";
                    DataSet dsaddon = new DataSet();
                    dsaddon = objCore.funGetDataSet(q);
                    if (dsaddon.Tables[0].Rows.Count > 0)
                    {
                        addon = "Add On-";
                    }
                    timediff = "";
                    try
                    {
                        DateTime time = Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString());
                        TimeSpan tm = DateTime.Now - time;
                        // MessageBox.Show(DateTime.Now.ToString());
                        //MessageBox.Show(time.ToString());
                        timediff = tm.ToString();
                        //MessageBox.Show(timediff);
                        string[] Split = timediff.ToString().Split('.');
                        timediff = Split[0];
                        //MessageBox.Show(timediff);
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }
                    if (i == 0)
                    {
                        try
                        {
                            Button btn = button1;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt = new DataTable();
                            dt.TableName = i.ToString();
                            dt = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            DataTable dt2 = new DataTable();
                            dt2 = dt.Copy();
                            if (dt.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            // dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            dataGridView1.DataSource = dt2;
                            dataGridView1.Columns[2].Visible = false;
                            dataGridView1.Columns[3].Visible = false;
                            dataGridView1.Columns[4].Visible = false;
                            dataGridView1.Columns[5].Visible = false;
                            dataGridView1.Columns[6].Visible = false;
                            dataGridView1.Columns[7].Visible = false;
                            dataGridView1.Columns[8].Visible = false;
                            bindgrid(dataGridView1);
                            btn.Text = addon + dt.Rows[0][8].ToString() +"-"+ds.Tables[0].Rows[i]["terminal"].ToString()+ "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView1.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView1.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView1.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView1.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView1.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView1.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }

                            //if (park == "yes")
                            //{
                            //    if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                            //    {
                            //        btnpark1.Text = "Parked";
                            //        btnpark1.BackColor = Color.Yellow;
                            //        btnpark1.ForeColor = Color.Black;

                            //    }
                            //    else
                            //    {
                            //        btnpark1.Text = "Pending";
                            //        btnpark1.BackColor = Color.White;
                            //        btnpark1.ForeColor = Color.Black;
                            //    }
                            //}
                            //else
                            //{
                            //    btnpark1.Visible = false;
                            //    if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                            //    {
                            //        btnpark1.Visible = true;
                            //        btnpark1.Text = "DT Order";
                            //        btnpark1.BackColor = Color.Yellow;
                            //        btnpark1.ForeColor = Color.Black;

                            //    }
                                
                            //}
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    if (i == 1)
                    {
                        try
                        {
                            Button btn = button2;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt1 = new DataTable();
                            dt1.TableName = i.ToString();
                            dt1 = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            if (dt1.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            // dataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            DataTable dt2 = new DataTable();
                            dt2 = dt1.Copy();
                            dataGridView2.DataSource = dt2;
                            dataGridView2.Columns[2].Visible = false;
                            dataGridView2.Columns[3].Visible = false;
                            dataGridView2.Columns[4].Visible = false;
                            dataGridView2.Columns[5].Visible = false;
                            dataGridView2.Columns[6].Visible = false;
                            dataGridView2.Columns[7].Visible = false;
                            dataGridView2.Columns[8].Visible = false;
                            bindgrid(dataGridView2);
                            btn.Text = addon + dt1.Rows[0][8].ToString() + "-" + ds.Tables[0].Rows[i]["terminal"].ToString() + "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView2.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView2.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView2.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView2.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView2.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView2.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView2.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView2.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }
                            
                            //if (park == "yes")
                            //{
                            //    if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                            //    {
                            //        btnpark2.Text = "Parked";
                            //        btnpark2.BackColor = Color.Yellow;
                            //        btnpark2.ForeColor = Color.Black;

                            //    }
                            //    else
                            //    {
                            //        btnpark2.Text = "Pending";
                            //        btnpark2.BackColor = Color.White;
                            //        btnpark2.ForeColor = Color.Black;
                            //    }
                            //}
                            //else
                            //{
                            //    btnpark2.Visible = false;
                            //    if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                            //    {
                            //        btnpark2.Visible = true;
                            //        btnpark2.Text = "DT Order";
                            //        btnpark2.BackColor = Color.Yellow;
                            //        btnpark2.ForeColor = Color.Black;

                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    if (i == 2)
                    {
                        try
                        {
                            Button btn = button3;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt = new DataTable();
                            dt.TableName = i.ToString();
                            dt = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            if (dt.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            // dataGridView3.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            DataTable dt2 = new DataTable();
                            dt2 = dt.Copy();
                            dataGridView3.DataSource = dt2;
                            dataGridView3.Columns[2].Visible = false;
                            dataGridView3.Columns[3].Visible = false;
                            dataGridView3.Columns[4].Visible = false;
                            dataGridView3.Columns[5].Visible = false;
                            dataGridView3.Columns[6].Visible = false;
                            dataGridView3.Columns[7].Visible = false;
                            dataGridView3.Columns[8].Visible = false;
                            bindgrid(dataGridView3);
                            btn.Text = addon + dt.Rows[0][8].ToString() + "-" + ds.Tables[0].Rows[i]["terminal"].ToString() + "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView3.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView3.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView3.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView3.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView3.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView3.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView3.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView3.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }
                            //if (park == "yes")
                            //{
                            //    if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                            //    {
                            //        btnpark3.Text = "Parked";
                            //        btnpark3.BackColor = Color.Yellow;
                            //        btnpark3.ForeColor = Color.Black;

                            //    }
                            //    else
                            //    {
                            //        btnpark3.Text = "Pending";
                            //        btnpark3.BackColor = Color.White;
                            //        btnpark3.ForeColor = Color.Black;
                            //    }
                            //}
                            //else
                            //{
                            //    btnpark3.Visible = false;
                            //    if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                            //    {
                            //        btnpark3.Visible = true;
                            //        btnpark3.Text = "DT Order";
                            //        btnpark3.BackColor = Color.Yellow;
                            //        btnpark3.ForeColor = Color.Black;

                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    if (i == 3)
                    {
                        try
                        {
                            Button btn = button4;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt = new DataTable();
                            dt.TableName = i.ToString();
                            dt = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            if (dt.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            // dataGridView4.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            DataTable dt2 = new DataTable();
                            dt2 = dt.Copy();
                            dataGridView4.DataSource = dt2;
                            dataGridView4.Columns[2].Visible = false;
                            dataGridView4.Columns[3].Visible = false;
                            dataGridView4.Columns[4].Visible = false;
                            dataGridView4.Columns[5].Visible = false;
                            dataGridView4.Columns[6].Visible = false;
                            dataGridView4.Columns[7].Visible = false;
                            dataGridView4.Columns[8].Visible = false;
                            bindgrid(dataGridView4);
                            btn.Text = addon + dt.Rows[0][8].ToString() + "-" + ds.Tables[0].Rows[i]["terminal"].ToString() + "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView4.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView4.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView4.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView4.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView4.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView4.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView4.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView4.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }
                            //if (park == "yes")
                            //{
                            //    if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                            //    {
                            //        btnpark4.Text = "Parked";
                            //        btnpark4.BackColor = Color.Yellow;
                            //        btnpark4.ForeColor = Color.Black;

                            //    }
                            //    else
                            //    {
                            //        btnpark4.Text = "Pending";
                            //        btnpark4.BackColor = Color.White;
                            //        btnpark4.ForeColor = Color.Black;
                            //    }
                            //}
                            //else
                            //{
                            //    btnpark4.Visible = false;
                            //    if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                            //    {
                            //        btnpark4.Visible = true;
                            //        btnpark4.Text = "DT Order";
                            //        btnpark4.BackColor = Color.Yellow;
                            //        btnpark4.ForeColor = Color.Black;

                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    if (i == 4)
                    {
                        try
                        {
                            Button btn = button7;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt = new DataTable();
                            dt.TableName = i.ToString();
                            dt = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            if (dt.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            // dataGridView7.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            DataTable dt2 = new DataTable();
                            dt2 = dt.Copy();
                            dataGridView7.DataSource = dt2;
                            dataGridView7.Columns[2].Visible = false;
                            dataGridView7.Columns[3].Visible = false;
                            dataGridView7.Columns[4].Visible = false;
                            dataGridView7.Columns[5].Visible = false;
                            dataGridView7.Columns[6].Visible = false;
                            dataGridView7.Columns[7].Visible = false;
                            dataGridView7.Columns[8].Visible = false;
                            bindgrid(dataGridView7);
                            btn.Text = addon + dt.Rows[0][8].ToString() + "-" + ds.Tables[0].Rows[i]["terminal"].ToString() + "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView7.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView7.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView7.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView7.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView7.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView7.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView7.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView7.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }
                            //if (park == "yes")
                            //{
                            //    if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                            //    {
                            //        btnpark5.Text = "Parked";
                            //        btnpark5.BackColor = Color.Yellow;
                            //        btnpark5.ForeColor = Color.Black;

                            //    }
                            //    else
                            //    {
                            //        btnpark5.Text = "Pending";
                            //        btnpark5.BackColor = Color.White;
                            //        btnpark5.ForeColor = Color.Black;
                            //    }
                            //}
                            //else
                            //{
                            //    btnpark5.Visible = false;
                            //    if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                            //    {
                            //        btnpark5.Visible = true;
                            //        btnpark5.Text = "DT Order";
                            //        btnpark5.BackColor = Color.Yellow;
                            //        btnpark5.ForeColor = Color.Black;

                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                    if (i == 5)
                    {

                        try
                        {
                            Button btn = button5;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt = new DataTable();
                            dt.TableName = i.ToString();
                            dt = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            if (dt.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            // dataGridView5.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            DataTable dt2 = new DataTable();
                            dt2 = dt.Copy();
                            dataGridView5.DataSource = dt2;
                            dataGridView5.Columns[2].Visible = false;
                            dataGridView5.Columns[3].Visible = false;
                            dataGridView5.Columns[4].Visible = false;
                            dataGridView5.Columns[5].Visible = false;
                            dataGridView5.Columns[6].Visible = false;
                            dataGridView5.Columns[7].Visible = false;
                            dataGridView5.Columns[8].Visible = false;
                            bindgrid(dataGridView5);
                            btn.Text = addon + dt.Rows[0][8].ToString() + "-" + ds.Tables[0].Rows[i]["terminal"].ToString() + "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView5.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView5.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView5.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView5.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView5.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView5.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView5.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView5.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }
                            //if (park == "yes")
                            //{
                            //    if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                            //    {
                            //        btnpark6.Text = "Parked";
                            //        btnpark6.BackColor = Color.Yellow;
                            //        btnpark6.ForeColor = Color.Black;

                            //    }
                            //    else
                            //    {
                            //        btnpark6.Text = "Pending";
                            //        btnpark6.BackColor = Color.White;
                            //        btnpark6.ForeColor = Color.Black;
                            //    }
                            //}
                            //else
                            //{
                            //    btnpark6.Visible = false;
                            //    if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                            //    {
                            //        btnpark6.Visible = true;
                            //        btnpark6.Text = "DT Order";
                            //        btnpark6.BackColor = Color.Yellow;
                            //        btnpark6.ForeColor = Color.Black;

                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {


                        }


                    }
                    if (i == 6)
                    {

                        try
                        {
                            Button btn = button6;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt = new DataTable();
                            dt.TableName = i.ToString();
                            dt = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            if (dt.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            //dataGridView6.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            DataTable dt2 = new DataTable();
                            dt2 = dt.Copy();
                            dataGridView6.DataSource = dt2;
                            dataGridView6.Columns[2].Visible = false;
                            dataGridView6.Columns[3].Visible = false;
                            dataGridView6.Columns[4].Visible = false;
                            dataGridView6.Columns[5].Visible = false;
                            dataGridView6.Columns[6].Visible = false;
                            dataGridView6.Columns[7].Visible = false;
                            dataGridView6.Columns[8].Visible = false;
                            bindgrid(dataGridView6);
                            btn.Text = addon + dt.Rows[0][8].ToString() + "-" + ds.Tables[0].Rows[i]["terminal"].ToString() + "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView6.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView6.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView6.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView6.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView6.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView6.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView6.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView6.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }

                            //if (park == "yes")
                            //{
                            //    if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                            //    {
                            //        btnpark7.Text = "Parked";
                            //        btnpark7.BackColor = Color.Yellow;
                            //        btnpark7.ForeColor = Color.Black;

                            //    }
                            //    else
                            //    {
                            //        btnpark7.Text = "Pending";
                            //        btnpark7.BackColor = Color.White;
                            //        btnpark7.ForeColor = Color.Black;
                            //    }
                            //}
                            //else
                            //{
                            //    btnpark7.Visible = false;
                            //    if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                            //    {
                            //        btnpark7.Visible = true;
                            //        btnpark7.Text = "DT Order";
                            //        btnpark7.BackColor = Color.Yellow;
                            //        btnpark7.ForeColor = Color.Black;

                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {


                        }
                    }
                    if (i == 7)
                    {
                        try
                        {
                            Button btn = button8;

                            btn.Font = new Font("", 14, FontStyle.Bold);
                            btn.Dock = System.Windows.Forms.DockStyle.Fill;
                            btn.Name = ds.Tables[0].Rows[i]["kdsgroup"].ToString();

                            DataTable dt = new DataTable();
                            dt.TableName = i.ToString();
                            dt = getdata(ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["kdsgroup"].ToString());
                            if (dt.Rows[0]["alarmtime"].ToString().Contains("-"))
                            {
                                btn.BackColor = Color.Red;
                            }
                            else
                            {
                                btn.BackColor = Color.White;
                            }
                            // dataGridView8.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(dataGridView_DataBindingComplete);
                            DataTable dt2 = new DataTable();
                            dt2 = dt.Copy();
                            dataGridView8.DataSource = dt2;
                            dataGridView8.Columns[2].Visible = false;
                            dataGridView8.Columns[3].Visible = false;
                            dataGridView8.Columns[4].Visible = false;
                            dataGridView8.Columns[5].Visible = false;
                            dataGridView8.Columns[6].Visible = false;
                            dataGridView8.Columns[7].Visible = false;
                            dataGridView8.Columns[8].Visible = false;
                            bindgrid(dataGridView8);
                            btn.Text = addon + dt.Rows[0][8].ToString() + "-" + ds.Tables[0].Rows[i]["terminal"].ToString() + "-Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! " + ds.Tables[0].Rows[i]["Customer"].ToString() + "-> " + timediff;
                            
                            if (park == "yes")
                            {
                                if (ds.Tables[0].Rows[i]["ParkStatus"].ToString().ToLower() == "parked")
                                {
                                    dataGridView8.DefaultCellStyle.BackColor = Color.White;
                                    dataGridView8.DefaultCellStyle.ForeColor = Color.Black;
                                    //btnpark8.Text = "Parked";
                                    //btnpark8.BackColor = Color.Yellow;
                                    //btnpark8.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView8.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView8.DefaultCellStyle.ForeColor = Color.White;
                                    //btnpark8.Text = "Pending";
                                    //btnpark8.BackColor = Color.White;
                                    //btnpark8.ForeColor = Color.Black;
                                }
                            }
                            else
                            {

                                if (ds.Tables[0].Rows[i]["terminal"].ToString().ToLower().Contains("dt"))
                                {
                                    dataGridView8.DefaultCellStyle.BackColor = Color.Yellow;
                                    dataGridView8.DefaultCellStyle.ForeColor = Color.Black;

                                }
                                else
                                {
                                    dataGridView8.DefaultCellStyle.BackColor = Color.Black;
                                    dataGridView8.DefaultCellStyle.ForeColor = Color.White;
                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }
                    }




                }
            }
            catch (Exception ex)
            {
                
               
            }

        }
        public DataTable getdata(string id,string group)
        {
            dtrpt.Clear();
            try
            {
                DataSet ds = new DataSet();
                string q = "";
                q = "SELECT     dbo.Saledetails.Orderstatus,dbo.sale.Terminal,dbo.Sale.id,dbo.Saledetails.time,dbo.MenuItem.Name,dbo.MenuItem.Minutes,dbo.MenuItem.alarmtime,dbo.MenuItem.minuteskdscolor,dbo.MenuItem.alarmkdscolor, dbo.Saledetails.id as sid,dbo.Saledetails.menuitemid,dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId,dbo.Saledetails.RunTimeModifierId, dbo.Saledetails.Quantity,dbo.Saledetails.comments, dbo.MenuItem.KDSId, dbo.Sale.OrderType FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id where  dbo.Sale.id='" + id + "' and  (dbo.Saledetails.OrderStatusmain = 'Pending')  and dbo.Saledetails.kdsgroup ='" + group + "'  order by dbo.Saledetails.id";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    int j = 1;
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        {

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
                                        dtrpt.Rows.Add("   " + dsmd.Tables[0].Rows[0]["name"].ToString() + ds.Tables[0].Rows[i]["comments"].ToString() + "(In Meal)", ds.Tables[0].Rows[i]["Quantity"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                    }
                                }
                                if (flavourid != string.Empty || mdid != string.Empty || runtime != string.Empty)
                                {
                                }
                                else
                                {
                                    dtrpt.Rows.Add(ds.Tables[0].Rows[i]["name"].ToString().Replace("Open Item", "") + "-" + ds.Tables[0].Rows[i]["comments"].ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                                    j++;
                                    try
                                    {
                                        q = "SELECT        dbo.MenuItem.KDSId, dbo.MenuItem.Name, dbo.Attachmenu1.Quantity FROM            dbo.Attachmenu1 INNER JOIN                         dbo.MenuItem ON dbo.Attachmenu1.attachmenuid = dbo.MenuItem.Id WHERE        (dbo.Attachmenu1.status = 'active') and dbo.Attachmenu1.menuitemid='" + ds.Tables[0].Rows[i]["menuitemid"].ToString() + "'";
                                        DataSet dsattach = new DataSet();
                                        dsattach = objCore.funGetDataSet(q);
                                        for (int m = 0; m < dsattach.Tables[0].Rows.Count; m++)
                                        {
                                            string temp = dsattach.Tables[0].Rows[m]["Quantity"].ToString();
                                            if (temp == "")
                                            {
                                                temp = "0";
                                            }
                                            float qty = float.Parse(temp);
                                            temp = ds.Tables[0].Rows[i]["Quantity"].ToString();
                                            if (temp == "")
                                            {
                                                temp = "0";
                                            }
                                            qty = qty * float.Parse(temp);
                                            dtrpt.Rows.Add("  " + dsattach.Tables[0].Rows[m]["name"].ToString() + "(In Meal)", qty, mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), ds.Tables[0].Rows[i]["id"].ToString(), ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());

                                        }
                                    }
                                    catch (Exception ex)
                                    {


                                    }
                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                
              
            }
            
            

            

            return dtrpt;
        }
        int columncount = 6; float fontt = 10;

        private void vButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            fillpending("yes");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            fillpending("yes");
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            kdsnewfixmainrecall obj = new kdsnewfixmainrecall();
            obj.no = no;
            obj.kdsid = kdsid;
            obj.date = date;
            obj.terminal = terminal;
            obj.Show();
        }

        private void kdsnewfix_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {               
                if (e.KeyValue == 49)
                {
                    Button btn = button1;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 50)
                {
                    Button btn = button2;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 51)
                {
                    Button btn = button3;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 52)
                {
                    Button btn = button4;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 54)
                {
                    Button btn = button7;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 55)
                {
                    Button btn = button5;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 56)
                {
                    Button btn = button6;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 57)
                {
                    Button btn = button8;

                    //"Order No:" + ds.Tables[0].Rows[i]["id"].ToString() + " ! Done";
                    int indx = btn.Text.IndexOf(":");
                    int indx2 = btn.Text.IndexOf("!");//12:45!
                    int length = indx2 - (indx + 1);
                    string group = btn.Name;
                    string id = btn.Text.Substring((indx + 1), length);
                    string q = "";
                    update(id, group);
                    fillpending("yes");
                }
                if (e.KeyValue == 53)
                {
                    kdsnewrecalfix obj = new kdsnewrecalfix();
                    obj.no = no;
                    obj.kdsid = kdsid;
                    obj.date = date;
                    obj.terminal = terminal;
                    obj.Show();
                }
            }
            catch (Exception ex) 
            {
                
            }
        }
    }
}
