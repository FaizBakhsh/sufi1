using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls.Properties;
using VIBlend.WinForms.Controls;
using VIBlend.WinForms;
using System.IO;
namespace POSRestaurant.Sale
{
    public partial class KDSScreen : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
       public string kds = "";
       public string kdsname = "";
        DataTable dtrpt = new DataTable();
        public KDSScreen()
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
        private void KDSScreen_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            label1.Text = kdsname;
            dtrpt = new DataTable();
            dtrpt.Columns.Add("Sr#", typeof(string));
            dtrpt.Columns.Add("QTY", typeof(string));            
            dtrpt.Columns.Add("ItemName", typeof(string));
            dtrpt.Columns.Add("time", typeof(string));
            dtrpt.Columns.Add("alarmtime", typeof(string));
            dtrpt.Columns.Add("timecolor", typeof(string));
            dtrpt.Columns.Add("alarmcolor", typeof(string));
            dtrpt.Columns.Add("id", typeof(string));
            dtrpt.Columns.Add("sid", typeof(string));
            dtrpt.Columns.Add("type", typeof(string));
            bind();
            getcompany();
          
        }
        public void bind()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = getdata(0);
                DataTable dtt = new DataTable();
                dtt.Merge(dt);
                dataGridView1.DataSource = dtt;
                if (dtt.Rows.Count > 0)
                {
                    label4.Text = "Order No:" + dtt.Rows[0]["id"].ToString();
                }
                else
                {
                    label4.Text = "";
                }
                dataGridView1.Columns[3].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[8].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                //DataGridViewColumn column1 = dataGridView1.Columns[0];
                //column1.Width = 30;
                //DataGridViewColumn column2 = dataGridView1.Columns[1];
                //column2.Width = 30;
                //DataGridViewColumn column3 = dataGridView1.Columns[2];
                //column3.Width = 250;
                //DataGridViewColumn column4 = dataGridView1.Columns[3];
                //column4.Width = 90;

                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 50;
                    if (dr.Cells["type"].Value.ToString() == "Din In")
                    {
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
                    }
                    if (dr.Cells["type"].Value.ToString() == "Take Away")
                    {
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                    }
                    if (dr.Cells["type"].Value.ToString() == "Delivery")
                    {
                        dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Brown;
                    }
                    if (dr.Cells["time"].Value.ToString().Contains("-"))
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromName(dr.Cells["timecolor"].Value.ToString()); //Color.Red;
                    }
                    if (dr.Cells["alarmtime"].Value.ToString().Contains("-"))
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromName(dr.Cells["alarmcolor"].Value.ToString()); //Color.Red;
                    }
                }
                DataTable dt1 = new DataTable();
                dt = getdata(1);
                dt1.Merge(dt);
                dataGridView2.DataSource = dt1;
                if (dt1.Rows.Count > 0)
                {
                    label2.Text = "Order No:" + dt1.Rows[0]["id"].ToString();
                }
                else
                {
                    label2.Text = "";
                }
                dataGridView2.Columns[3].Visible = false;
                dataGridView2.Columns[4].Visible = false;
                dataGridView2.Columns[5].Visible = false;
                dataGridView2.Columns[6].Visible = false;
                dataGridView2.Columns[7].Visible = false;
                dataGridView2.Columns[8].Visible = false;
                dataGridView2.Columns[9].Visible = false;
                //DataGridViewColumn column11 = dataGridView2.Columns[0];
                //column11.Width = 30;
                //DataGridViewColumn column21 = dataGridView2.Columns[1];
                //column21.Width = 30;
                //DataGridViewColumn column31 = dataGridView2.Columns[2];
                //column31.Width = 250;
                //DataGridViewColumn column41 = dataGridView2.Columns[3];
                //column41.Width = 90;

                foreach (DataGridViewRow dr in dataGridView2.Rows)
                {
                    dr.Height = 50;
                    if (dr.Cells["type"].Value.ToString() == "Din In")
                    {
                        dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
                    }
                    if (dr.Cells["type"].Value.ToString() == "Take Away")
                    {
                        dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                    }
                    if (dr.Cells["type"].Value.ToString() == "Delivery")
                    {
                        dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
                    }
                    if (dr.Cells["time"].Value.ToString().Contains("-"))
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromName(dr.Cells["timecolor"].Value.ToString()); //Color.Red;
                    }
                    if (dr.Cells["alarmtime"].Value.ToString().Contains("-"))
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromName(dr.Cells["alarmcolor"].Value.ToString()); //Color.Red;
                    }
                }
                DataTable dt2 = new DataTable();
                dt = getdata(2);
                dt2.Merge(dt);
                dataGridView3.DataSource = dt2;
                if (dt2.Rows.Count > 0)
                {
                    label3.Text ="Order No:"+ dt2.Rows[0]["id"].ToString();
                }
                else
                {
                    label3.Text = "";
                }
                
                dataGridView3.Columns[3].Visible = false;
                dataGridView3.Columns[4].Visible = false;
                dataGridView3.Columns[5].Visible = false;
                dataGridView3.Columns[6].Visible = false;
                dataGridView3.Columns[7].Visible = false;
                dataGridView3.Columns[8].Visible = false;
                dataGridView3.Columns[9].Visible = false;
                //DataGridViewColumn column12 = dataGridView3.Columns[0];
                //column12.Width = 30;
                //DataGridViewColumn column22 = dataGridView3.Columns[1];
                //column22.Width = 30;
                //DataGridViewColumn column32 = dataGridView3.Columns[2];
                //column32.Width = 250;
                //DataGridViewColumn column42 = dataGridView3.Columns[3];
                //column42.Width = 90;

                foreach (DataGridViewRow dr in dataGridView3.Rows)
                {
                    dr.Height = 50;
                    if (dr.Cells["type"].Value.ToString() == "Din In")
                    {
                        dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.Blue;
                    }
                    if (dr.Cells["type"].Value.ToString() == "Take Away")
                    {
                        dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.Green;
                    }
                    if (dr.Cells["type"].Value.ToString() == "Delivery")
                    {
                        dataGridView3.ColumnHeadersDefaultCellStyle.BackColor = Color.Yellow;
                    }
                    if (dr.Cells["time"].Value.ToString().Contains("-"))
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromName(dr.Cells["timecolor"].Value.ToString()); //Color.Red;
                    }
                    if (dr.Cells["alarmtime"].Value.ToString().Contains("-"))
                    {
                        dr.DefaultCellStyle.BackColor = Color.FromName(dr.Cells["alarmcolor"].Value.ToString()); //Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {

               // MessageBox.Show(ex.Message);
            }
        }
       
        public DataTable getdata(int rowno)
        {
            //dtrpt = new DataTable();
            dtrpt.Clear();
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsgetid = new DataSet();
            string id = "";
            string qry = "";//"SELECT *  FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id asc) AS RowNum, * FROM VSaledetails where  ) sub WHERE RowNum = '"+rowno+"'";
            qry = "SELECT *  FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id asc) AS RowNum, *  FROM VSaledetails ) sub WHERE  KDSId='"+kds+"' ";
            dsgetid = objCore.funGetDataSet(qry);
            if (dsgetid.Tables[0].Rows.Count > 0)
            {
                if (dsgetid.Tables[0].Rows.Count >= rowno + 1)
                {
                    id = dsgetid.Tables[0].Rows[rowno]["id"].ToString();
                }
                else
                {
                    return dtrpt;
                }
                
            }
            else
            {
                return dtrpt;
            }
            DataSet ds = new DataSet();
            string q = "SELECT     dbo.Sale.time,dbo.MenuItem.Name,dbo.MenuItem.Minutes,dbo.MenuItem.alarmtime,dbo.MenuItem.minuteskdscolor,dbo.MenuItem.alarmkdscolor, dbo.Saledetails.id as sid,dbo.Saledetails.Flavourid, dbo.Saledetails.ModifierId, dbo.Saledetails.Quantity,dbo.Saledetails.comments, dbo.MenuItem.KDSId, dbo.Sale.OrderType FROM         dbo.Sale INNER JOIN                      dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id where dbo.MenuItem.KDSId='" + kds + "' and  dbo.Sale.id='" + id + "' and dbo.Saledetails.Orderstatus='pending' order by dbo.Saledetails.id";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                int j = 1;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    float mnts = 0, alarmmnts = 0;
                    string mts = "", alarmmts = "" ;
                    
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

                        // mnts = span.Minutes;
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
                        TimeSpan span = (Convert.ToDateTime(ds.Tables[0].Rows[i]["time"].ToString()) - DateTime.Now.AddMinutes(-(alarmmnts+mnts)));

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
                            dtrpt.Rows.Add("", "", dsflr.Tables[0].Rows[0]["name"].ToString() + ds.Tables[0].Rows[i]["comments"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), id, ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                        }

                    }
                    string mdid = ds.Tables[0].Rows[i]["ModifierId"].ToString();
                    if (mdid == "0")
                    {
                        mdid = string.Empty;
                    }
                    if (mdid != string.Empty )
                    {
                        DataSet dsmd = new DataSet();
                        q = "SELECT     Name FROM         Modifier where id='" + mdid + "'";
                        dsmd = objCore.funGetDataSet(q);
                        if (dsmd.Tables[0].Rows.Count > 0)
                        {
                            dtrpt.Rows.Add("", "", dsmd.Tables[0].Rows[0]["name"].ToString() + ds.Tables[0].Rows[i]["comments"].ToString(), "", "", ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), id, ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                        }

                    }
                    if (flavourid != string.Empty || mdid != string.Empty)
                    {
                    }
                    else
                    {
                        dtrpt.Rows.Add(j.ToString(), ds.Tables[0].Rows[i]["Quantity"].ToString(), ds.Tables[0].Rows[i]["name"].ToString() + ds.Tables[0].Rows[i]["comments"].ToString(), mts.ToString(), alarmmts, ds.Tables[0].Rows[i]["minuteskdscolor"].ToString(), ds.Tables[0].Rows[i]["alarmkdscolor"].ToString(), id, ds.Tables[0].Rows[i]["sid"].ToString(), ds.Tables[0].Rows[i]["OrderType"].ToString());
                        j++;
                    }
                }
            }
            return dtrpt;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            vButton btn =(sender) as vButton;
            if (btn.Name == "vButton3")
            {
                try
                {
                    foreach (DataGridViewRow dr in dataGridView1.Rows)
                    {
                        string val = dr.Cells["sid"].Value.ToString();
                        objCore.executeQuery("UPDATE    Saledetails SET              Orderstatus = 'Completed' where id='" + val + "'");
                    }
                }
                catch (Exception ex)
                {
                    
                    
                }
                
            }
            if (btn.Name == "vButton1")
            {
                try
                {
                    foreach (DataGridViewRow dr in dataGridView2.Rows)
                    {
                        string val = dr.Cells["sid"].Value.ToString();
                        objCore.executeQuery("UPDATE    Saledetails SET              Orderstatus = 'Completed' where id='" + val + "'");
                    }
                }
                catch (Exception ex)
                {
                    
                   
                }

            }
            if (btn.Name == "vButton2")
            {
                try
                {
                    foreach (DataGridViewRow dr in dataGridView3.Rows)
                    {
                        string val = dr.Cells["sid"].Value.ToString();
                        objCore.executeQuery("UPDATE    Saledetails SET              Orderstatus = 'Completed' where id='" + val + "'");
                    }
                }
                catch (Exception ex)
                {
                    
                   
                }

            }
            
            bind();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            bind();
        }

        private void KDSScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)// && e.KeyCode == Keys.N)
            {
                MessageBox.Show(e.KeyCode.ToString());
            }
        }

        private void KDSScreen_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.Control)// && e.KeyCode == Keys.N)
            {
                MessageBox.Show(e.KeyChar.ToString());
            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_Scroll(object sender, ScrollEventArgs e)
        {
           
           
        }
    }
}
