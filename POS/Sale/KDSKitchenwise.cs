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
    public partial class KDSKitchenwise : Form
    {
        public string date = "";
        public KDSKitchenwise()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void KDSKitchenwise_Load(object sender, EventArgs e)
        {
            textBox1.Text = POSRestaurant.Properties.Settings.Default.fontsize;
            this.FormBorderStyle = FormBorderStyle.None;
            string q = "select * from kds where id>1";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 4; i > ds.Tables[0].Rows.Count; i--)
                {
                    tableLayoutPanel1.ColumnStyles[i-1].SizeType = SizeType.Absolute;
                    tableLayoutPanel1.ColumnStyles[i-1].Width = 0;
                }
            }
            try
            {
                q = "select top 1 * from dayend where daystatus='Open' order by id desc";
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    date = Convert.ToDateTime(ds.Tables[0].Rows[0]["date"].ToString()).ToString("yyyy-MM-dd");
                }
            }
            catch (Exception ex)
            {
                
            }
            fillgrid();
        }
        protected void fillgrid()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Color");
            dt.Columns.Add("Bill_No");
            dt.Columns.Add("Name");
            dt.Columns.Add("Qty");
            dt.Columns.Add("Time");
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("Id");
            dt1.Columns.Add("Color");
            dt1.Columns.Add("Bill_No");
            dt1.Columns.Add("Name");
            dt1.Columns.Add("Qty");
            dt1.Columns.Add("Time");
            DataTable dt2 = new DataTable();
            dt2.Columns.Add("Id");
            dt2.Columns.Add("Color");
            dt2.Columns.Add("Bill_No");
            dt2.Columns.Add("Name");
            dt2.Columns.Add("Qty");
            dt2.Columns.Add("Time");
            DataTable dt3 = new DataTable();
            dt3.Columns.Add("Id");
            dt3.Columns.Add("Color");
            dt3.Columns.Add("Bill_No");
            dt3.Columns.Add("Name");
            dt3.Columns.Add("Qty");
            dt3.Columns.Add("Time");

            DataTable dt4 = new DataTable();
            dt4.Columns.Add("Id");
            dt4.Columns.Add("Color");
            dt4.Columns.Add("Bill_No");
            dt4.Columns.Add("Name");
            dt4.Columns.Add("Qty");
            dt4.Columns.Add("Time");
            string q = "SELECT   ISNULL( dbo.Saledetails.Priority,'Normal') as Priority, dbo.sale.date, cast( dbo.Saledetails.id as varchar(100)) as id,cast( dbo.Saledetails.saleid as varchar(100)) as saleid,dbo.sale.Customer, cast(dbo.Saledetails.time as varchar(100)) as time, cast(dbo.Saledetails.kdsid as varchar(100)) as kdsid, cast(dbo.ModifierFlavour.name as varchar(100)) AS Flavour, cast(dbo.MenuItem.Name as varchar(100)) +' ('+dbo.Saledetails.comments+')' as Name, dbo.RuntimeModifier.name AS Runtime, dbo.Modifier.Name AS Modifier, cast(dbo.Saledetails.Quantity as varchar(100)) as Quantity, cast(dbo.MenuItem.Minutes as varchar(100)) as Minutes, cast(dbo.MenuItem.alarmtime as varchar(100)) as alarmtime FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id where saledetails.OrderStatusmain='Pending' order by dbo.Saledetails.time";
            DataSet dsdetails = new DataSet();
            dsdetails = objcore.funGetDataSet(q);
            List<SaleDetailsClass> menulist = new List<SaleDetailsClass>();
            try
            {
                menulist = new List<SaleDetailsClass>();
                IList<SaleDetailsClass> data = dsdetails.Tables[0].AsEnumerable().Select(row =>
                    new SaleDetailsClass
                    {
                        id = row.Field<string>("id"),
                        saleid = row.Field<string>("saleid"),
                        Customer = row.Field<string>("Customer"),
                        time = row.Field<string>("time"),
                        kdsid = row.Field<string>("kdsid"),
                        Flavour = row.Field<string>("Flavour"),
                        Name = row.Field<string>("Name"),
                        Runtime = row.Field<string>("Runtime"),
                        Modifier = row.Field<string>("Modifier"),
                        Quantity = row.Field<string>("Quantity"),
                        Minutes = row.Field<string>("Minutes"),
                        alarmtime = row.Field<string>("alarmtime"),
                        priority = row.Field<string>("Priority")



                    }).ToList();
                menulist = data.ToList();



            }
            catch (Exception ex)
            {


            }
            try
            {
                if (date == "")
                {
                    q = "select top 1 * from dayend where daystatus='Open' order by id desc";
                    DataSet ds1 = new DataSet();
                    ds1 = objcore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        date = Convert.ToDateTime(ds1.Tables[0].Rows[0]["date"].ToString()).ToString("yyyy-MM-dd");
                    }
                }

            }
            catch (Exception ex)
            {

            }
            q = "SELECT      cast( dbo.Saledetails.id as varchar(100)) as id,cast( dbo.Saledetails.saleid as varchar(100)) as saleid,dbo.sale.Customer, cast(dbo.Saledetails.completedtime as varchar(100)) as time, cast(dbo.Saledetails.kdsid as varchar(100)) as kdsid, cast(dbo.ModifierFlavour.name as varchar(100)) AS Flavour, cast(dbo.MenuItem.Name as varchar(100)) +' ('+dbo.Saledetails.comments+')' as Name, dbo.RuntimeModifier.name AS Runtime, dbo.Modifier.Name AS Modifier, cast(dbo.Saledetails.Quantity as varchar(100)) as Quantity, cast(dbo.MenuItem.Minutes as varchar(100)) as Minutes, cast(dbo.MenuItem.alarmtime as varchar(100)) as alarmtime FROM            dbo.Sale INNER JOIN                         dbo.Saledetails ON dbo.Sale.Id = dbo.Saledetails.saleid INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id where saledetails.OrderStatusmain='Completed' and sale.date='" + date + "' order by dbo.Saledetails.completedtime desc";
            dsdetails = new DataSet();
            dsdetails = objcore.funGetDataSet(q);
            List<SaleDetailsClass> menulistserved = new List<SaleDetailsClass>();
            try
            {
                menulistserved = new List<SaleDetailsClass>();
                IList<SaleDetailsClass> data = dsdetails.Tables[0].AsEnumerable().Select(row =>
                    new SaleDetailsClass
                    {
                        id = row.Field<string>("id"),
                        saleid = row.Field<string>("saleid"),
                        Customer = row.Field<string>("Customer"),
                        time = row.Field<string>("time"),
                        kdsid = row.Field<string>("kdsid"),
                        Flavour = row.Field<string>("Flavour"),
                        Name = row.Field<string>("Name"),
                        Runtime = row.Field<string>("Runtime"),
                        Modifier = row.Field<string>("Modifier"),
                        Quantity = row.Field<string>("Quantity"),
                        Minutes = row.Field<string>("Minutes"),
                        alarmtime = row.Field<string>("alarmtime")

                    }).ToList();
                menulistserved = data.ToList();



            }
            catch (Exception ex)
            {


            }
            q = "select * from kds where id>1";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                bool chk = false;
                string kdsid = ds.Tables[0].Rows[i]["id"].ToString();

                chk = true;
                if (chk == true)
                {
                    foreach (var item in menulist.Where(x => x.kdsid == kdsid))
                    {
                        int minutes = 15, alarm = 5;
                        string temp = "";
                        temp = item.Minutes.ToString();// dsdetails.Tables[0].Rows[j]["Minutes"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        try
                        {
                            minutes = Convert.ToInt32(temp);
                        }
                        catch (Exception ex)
                        {

                        }
                        temp = item.alarmtime.ToString();// dsdetails.Tables[0].Rows[j]["alarmtime"].ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        try
                        {
                            alarm = Convert.ToInt32(temp);
                        }
                        catch (Exception ex)
                        {

                        }
                        string color = "White";
                        DateTime ordertime = Convert.ToDateTime(item.time);
                        TimeSpan ts = DateTime.Now - ordertime;
                        string time = "";
                        try
                        {
                            time = string.Format("{0}:{1:00}", (int)ts.TotalMinutes, ts.Seconds);
                        }
                        catch (Exception ex)
                        {

                        }

                        double totalminutes = ts.TotalMinutes;
                        if (totalminutes > minutes)
                        {
                            if (totalminutes > (minutes + alarm))
                            {
                                color = "Red";
                            }
                            else
                            {
                                color = "Yellow";
                            }
                        }
                        if (item.priority == "High")
                        {
                            color = "Orange";
                        }
                        string name = item.Name.ToString();// dsdetails.Tables[0].Rows[j]["name"].ToString();

                        try
                        {
                            if (item.Flavour.ToString().Length > 0)
                            {
                                name = item.Flavour + "'" + name;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            if (item.Runtime.ToString().Length > 0)
                            {
                                name = "  " + item.Runtime;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        try
                        {
                            if (item.Modifier.ToString().Length > 0)
                            {
                                name = "  " + item.Modifier;
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        string flag = "";
                        try
                        {
                            if (menulist.Where(x => x.kdsid != kdsid && x.saleid == item.saleid).ToList().Count > 0)
                            {
                                flag = "*";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        if (i == 0)
                        {
                            dt.Rows.Add(item.id, color, flag + item.Customer + "(" + item.saleid + ")", name, item.Quantity, time);
                        }
                        if (i == 1)
                        {
                            dt1.Rows.Add(item.id, color, flag + item.Customer + "(" + item.saleid + ")", name, item.Quantity, time);
                        }
                        if (i == 2)
                        {
                            dt2.Rows.Add(item.id, color, flag + item.Customer + "(" + item.saleid + ")", name, item.Quantity, time);
                        }
                        if (i == 3)
                        {
                            dt3.Rows.Add(item.id, color, flag + item.Customer + "(" + item.saleid + ")", name, item.Quantity, time);
                        }
                    }
                }
                if (i == 0)
                {
                    label1.Text = ds.Tables[0].Rows[i]["name"].ToString();
                }
                if (i == 1)
                {
                    label2.Text = ds.Tables[0].Rows[i]["name"].ToString();
                }
                if (i == 2)
                {
                    label3.Text = ds.Tables[0].Rows[i]["name"].ToString();
                }
                if (i == 3)
                {
                    label4.Text = ds.Tables[0].Rows[i]["name"].ToString();
                }
                if (i == 0 & chk == true)
                {
                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[0].Width = 60;
                    dataGridView1.Columns[1].Width = 0;
                    dataGridView1.Columns[2].Width = 0;
                    dataGridView1.Columns[3].Width = 80;
                    dataGridView1.Columns[4].Width = 160;
                    //dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView1.Columns[5].Width = 50;
                    dataGridView1.Columns[6].Width = 70;
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                }
                if (i == 1 & chk == true)
                {
                    dataGridView2.DataSource = dt1;

                    dataGridView2.Columns[1].Width = 0;
                    dataGridView2.Columns[2].Width = 0;

                    dataGridView2.Columns[0].Width = 60;
                    dataGridView2.Columns[3].Width = 80;
                    dataGridView2.Columns[4].Width = 160;
                    //dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView2.Columns[5].Width = 50;
                    dataGridView2.Columns[6].Width = 70;
                    dataGridView2.Columns[1].Visible = false;
                    dataGridView2.Columns[2].Visible = false;
                }
                if (i == 2 & chk == true)
                {
                    dataGridView3.DataSource = dt2;

                   

                    dataGridView3.Columns[0].Width = 60;
                    dataGridView3.Columns[3].Width = 80;
                    dataGridView3.Columns[4].Width = 160;
                    //dataGridView3.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView3.Columns[5].Width = 50;
                    dataGridView3.Columns[6].Width = 70;
                    dataGridView3.Columns[1].Visible = false;
                    dataGridView3.Columns[2].Visible = false;
                }
                if (i == 3 & chk == true)
                {
                    

                    dataGridView4.DataSource = dt3;
                    

                    dataGridView4.Columns[0].Width = 60;
                    dataGridView4.Columns[3].Width = 80;
                    dataGridView4.Columns[4].Width = 160;
                    //dataGridView4.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    dataGridView4.Columns[5].Width = 50;
                    dataGridView4.Columns[6].Width = 70;
                    //dataGridView4.Columns[1].Visible = false;
                    //dataGridView4.Columns[2].Visible = false;
                }

            }
            try
            {
                foreach (var item in menulistserved)
                {

                    string color = "Green";
                    DateTime ordertime = Convert.ToDateTime(item.time);
                    TimeSpan ts = DateTime.Now - ordertime;
                    string time = "";
                    try
                    {
                        time = string.Format("{0}:{1:00}", (int)ts.TotalMinutes, ts.Seconds);
                    }
                    catch (Exception ex)
                    {

                    }

                    double totalminutes = ts.TotalMinutes;

                    string name = item.Name.ToString();

                    try
                    {
                        if (item.Flavour.ToString().Length > 0)
                        {
                            name = item.Flavour + "'" + name;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        if (item.Runtime.ToString().Length > 0)
                        {
                            name = "  " + item.Runtime;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    try
                    {
                        if (item.Modifier.ToString().Length > 0)
                        {
                            name = "  " + item.Modifier;
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    string flag = "";
                    try
                    {
                        if (menulist.Where(x => x.saleid == item.saleid).ToList().Count > 0)
                        {
                            flag = "*";
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                    dt4.Rows.Add(item.id, color, flag + item.Customer + "(" + item.saleid + ")", name, item.Quantity, time);

                }
                dataGridView5.DataSource = dt4;
                dataGridView5.Columns[2].Width = 80;
                dataGridView5.Columns[3].Width = 160;
                dataGridView5.Columns[4].Width = 50;
                dataGridView5.Columns[5].Width = 80;
                dataGridView5.Columns[0].Visible = false;
                dataGridView5.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {

            }
            float size = 12;

            try
            {
                size = float.Parse(POSRestaurant.Properties.Settings.Default.fontsize);
            }
            catch (Exception ex)
            {

            }
            try
            {
                dataGridView1.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView2.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView3.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView4.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView5.DefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);

                dataGridView1.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView2.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView3.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView4.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
                dataGridView5.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Times New Roman", size);
            }
            catch (Exception ex)
            {
                
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            fillgrid();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells["Color"].Value.ToString() == "Red")
                {
                    dr.DefaultCellStyle.BackColor = Color.Red;
                    dr.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                    if (dr.Cells["Color"].Value.ToString() == "Yellow")
                    {
                        dr.DefaultCellStyle.BackColor = Color.Yellow;
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                        if (dr.Cells["Color"].Value.ToString() == "Orange")
                        {
                            dr.DefaultCellStyle.BackColor = Color.Orange;
                            dr.DefaultCellStyle.ForeColor = Color.White;
                        }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = Color.White;
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }
            }
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                if (dr.Cells["Color"].Value.ToString() == "Red")
                {
                    dr.DefaultCellStyle.BackColor = Color.Red;
                    dr.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                    if (dr.Cells["Color"].Value.ToString() == "Yellow")
                    {
                        dr.DefaultCellStyle.BackColor = Color.Yellow;
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                        if (dr.Cells["Color"].Value.ToString() == "Orange")
                        {
                            dr.DefaultCellStyle.BackColor = Color.Orange;
                            dr.DefaultCellStyle.ForeColor = Color.White;
                        }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = Color.White;
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }
            }
        }

        private void dataGridView3_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView3.Rows)
            {
                if (dr.Cells["Color"].Value.ToString() == "Red")
                {
                    dr.DefaultCellStyle.BackColor = Color.Red;
                    dr.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                    if (dr.Cells["Color"].Value.ToString() == "Yellow")
                    {
                        dr.DefaultCellStyle.BackColor = Color.Yellow;
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }
                    else
                        if (dr.Cells["Color"].Value.ToString() == "Orange")
                        {
                            dr.DefaultCellStyle.BackColor = Color.Orange;
                            dr.DefaultCellStyle.ForeColor = Color.White;
                        }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = Color.White;
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }
            }
        }

        private void dataGridView4_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView4.Rows)
            {
                if (dr.Cells["Color"].Value.ToString() == "Red")
                {
                    dr.DefaultCellStyle.BackColor = Color.Red;
                    dr.DefaultCellStyle.ForeColor = Color.White;
                }
                else
                    if (dr.Cells["Color"].Value.ToString() == "Green")
                    {
                        dr.DefaultCellStyle.BackColor = Color.Green;
                        dr.DefaultCellStyle.ForeColor = Color.White;
                    }
                    else
                        if (dr.Cells["Color"].Value.ToString() == "Orange")
                        {
                            dr.DefaultCellStyle.BackColor = Color.Orange;
                            dr.DefaultCellStyle.ForeColor = Color.White;
                        }
                    else
                    {
                        dr.DefaultCellStyle.BackColor = Color.White;
                        dr.DefaultCellStyle.ForeColor = Color.Black;
                    }
            }
        }
        private void dataGridView5_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                //foreach (DataGridViewRow dr in dataGridView4.Rows)
                {
                    dataGridView5.DefaultCellStyle.BackColor = Color.Green;
                    dataGridView5.DefaultCellStyle.ForeColor = Color.White;
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                int index = senderGrid.CurrentCell.RowIndex;
                string id = senderGrid.Rows[index].Cells[1].Value.ToString();
                string q = "update Saledetails set OrderStatus='Completed',OrderStatusmain='Completed',NotificationStatus='Pending', completedtime='" + DateTime.Now + "' where id='" + id + "'";
                objcore.executeQuery(q);
                fillgrid();
            }
        }

        private void KDSKitchenwise_FormClosed(object sender, FormClosedEventArgs e)
        {
           Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fontSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Visible = true;
            button3.Visible = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                return;
            }
            POSRestaurant.Properties.Settings.Default.fontsize = textBox1.Text;
            POSRestaurant.Properties.Settings.Default.Save();
            textBox1.Visible = false;
            button3.Visible = false;
        }
    }
}
