using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using VIBlend.WinForms.Controls;
using Newtonsoft.Json;
namespace POSRestaurant.Sale
{
    public partial class NewBillTypenew : Form
    {
        private TextBox focusedTextbox = null;
        private RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        string cs = POSRestaurant.Properties.Settings.Default.ConnectionString;
        DataSet ds;
        public string type = "", id = "", saleid = "", total = "0", name = "", advance = "0", rename = "", date = "";

        public NewBillTypenew(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Customers order by name";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["name"] = "No Receivable";
                ds.Tables[0].Rows.Add(dr);
                cmbcustomers.DataSource = ds.Tables[0];
                cmbcustomers.ValueMember = "id";
                cmbcustomers.DisplayMember = "name";
                cmbcustomers.Text = "No Receivable";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void fillarea()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from DeliveryArea order by name";
                ds = objCore.funGetDataSet(q);
             
                cmbarea.DataSource = ds.Tables[0];
                cmbarea.ValueMember = "id";
                cmbarea.DisplayMember = "name";
               
            }
            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message);
            }
        }
        public void fillitems()
        {
            
            try
            {
                DataSet dsitems = new DataSet();
                dsitems = objCore.funGetDataSet("select Phone from Delivery");
                AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
                for (int i = 0; i < dsitems.Tables[0].Rows.Count; i++)
                {
                    MyCollection.Add(dsitems.Tables[0].Rows[i]["Phone"].ToString());//.GetString(0));
                }
                txtphone.AutoCompleteCustomSource = MyCollection;
                txtphone.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txtphone.AutoCompleteSource = AutoCompleteSource.CustomSource;
            }
            catch (Exception ex)
            {


            }
        }
        private void button42_Click(object sender, EventArgs e)
        {
            _frm1.cleartables();
            _frm1.Enabled = true;
            _frm1.Islbldelivery = "Not Selected";
          //  _frm1.refereshtabs();
           // _frm1.TopMost = true;
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 0)
            {
                txtname.Focus();
                focusedTextbox = txtname;
            }
            if (tabControl1.SelectedIndex == 1)
            {
                textBox1.Focus();
                focusedTextbox = textBox1;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            focusedTextbox = txt;
            strt = focusedTextbox.SelectionStart;
        }
        public void getsaleid(string tbno)
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
            string date = "";
            DataSet dsdt = new DataSet();
            string q = "select top(1) * from dayend order by id desc";
            SqlConnection con = new SqlConnection(cs);
            try
            {
                if (con.State == ConnectionState.Open)
                { con.Close(); }
                con.Open();
                SqlCommand com = new SqlCommand(q, con);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dsdt);
            }
            catch (Exception ex)
            {

            }
            finally
            {
                con.Close();
            }
            //objCore = new classes.Clsdbcon();
            //dsdt = objCore.funGetDataSet(q);
            try
            {
                if (dsdt.Tables[0].Rows.Count > 0)
                {
                    date = dsdt.Tables[0].Rows[0]["Date"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            _frm1.Islbldelivery = "Dine In";
            _frm1.waitername = comboBox1.Text;
            _frm1.posfee = 0;
            _frm1.pay("Cash", "", "0", tbno, "Dine In", "", "");
            saleid = _frm1.getsaleid();
            q = "insert into DinInTables (ResId,Guests,id,TableNo,Saleid,Date,time,Status,WaiterId) values('" + txtresid.Text + "','" + txtguest.Text + "','" + id + "','" + tbno + "','" + saleid + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','Pending','" + comboBox1.SelectedValue + "')";
            objCore.executeQuery(q);
            updateordertype("Dine In");
            _frm1.Enabled = true;
            //Sale.SaleAfter obj = new Sale.SaleAfter(_frm1);
            //obj.total = total;
            //obj.id = saleid.ToString();
            //obj.advance = advance;
            //obj.name = name;
            //obj.type = "Dine In";
            //obj.Show();
            
            this.Close();


        }
        private void btnsubmit_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text.Trim();
            if (textBox1.Text.Trim() == string.Empty)
            {
                //name = comboBox1.Text;


            }
            if (name.Trim() == string.Empty)
            {

                MessageBox.Show("Please Enter Table no");
                return;
            }


            if (rename == "yes")
            {
                try
                {
                    string q = "update sale set, posfee=0, ordertype='Dine In',customer='" + name + "' where id='" + saleid + "'";
                    objCore.executeQuery(q);
                    q = "update DinInTables set TableNo='" + name + "',WaiterId='" + comboBox1.SelectedValue + "',Guests='" + txtguest.Text + "',ResId='" + txtresid.Text + "' where saleid='" + saleid + "'";
                    objCore.executeQuery(q);
                    _frm1.getorders("new");
                    _frm1.Enabled = true;
                    this.Close();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            string p = "update sale set, posfee=0  where id='" + saleid + "'";
            objCore.executeQuery(p);
            getsaleid(name);
            //_frm1.Islbldelivery = "Dine In";
            //_frm1.newbill();

        }
        private void button34_Click(object sender, EventArgs e)
        {
            if (rename == "yes")
            {
                try
                {
                    string q = "";
                    string OrderType = getordertype(id);
                    if (OrderType != "Delivery")
                    {
                        q = "update sale set posfee='0'  where id='" + id + "'";
                        objCore.executeQuery(q);
                    }


                     q = "update sale set ordertype='Take Away',customer='" + txtname.Text + "' where id='" + saleid + "'";
                    objCore.executeQuery(q);
                    q = "update TakeAway set CustomerId='" + txtname.Text + "' where saleid='" + saleid + "'";
                    objCore.executeQuery(q);
                    _frm1.getorders("new");
                    _frm1.Enabled = true;
                    this.Close();
                    return;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            type = "takeaway";
            try
            {
                if (txtname.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Customer Id");
                    return;
                }
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                int idd = 0;
                ds = new DataSet();
                if (type == "takeaway")
                {
                    ds = objCore.funGetDataSet("select max(id) as id from TakeAway");
                }
                if (type == "carhope")
                {
                    ds = objCore.funGetDataSet("select max(id) as id from carhope");
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0][0].ToString();
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


                string date = "";
                DataSet dsdt = new DataSet();
                string q = "select top(1) * from dayend order by id desc";
                SqlConnection con = new SqlConnection(cs);
                try
                {
                    if (con.State == ConnectionState.Open)
                    { con.Close(); }
                    con.Open();
                    SqlCommand com = new SqlCommand(q, con);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dsdt);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
                //objCore = new classes.Clsdbcon();
                //dsdt = objCore.funGetDataSet(q);
                try
                {
                    if (dsdt.Tables[0].Rows.Count > 0)
                    {
                        date = dsdt.Tables[0].Rows[0]["Date"].ToString();
                    }
                }
                catch (Exception ex)
                {


                }
                _frm1.Islbldelivery = "Take Away";
                _frm1.gettotal();
                _frm1.pay("Cash", "", "0", txtname.Text, "Take Away", "", "");
                
                saleid = _frm1.getsaleid();
                if (type == "takeaway")
                {
                    q = "insert into TakeAway (id,CustomerId,Date,time,Saleid,Status) values ('" + idd + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','"+saleid+"','Pending')";
                    _frm1.takeawayid = idd.ToString();
                }
                if (type == "carhope")
                {
                    q = "insert into carhope (id,CustomerId,Date,Saleid,Status) values ('" + idd + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + date + "','','Pending')";
                    _frm1.carhopeid = idd.ToString();
                }
                objCore.executeQuery(q);

               // _frm1.settableno("Customer ID: " + txtname.Text.Trim().Replace("'", "''"));
                _frm1.Enabled = true;
                updateordertype("Take Away");
                _frm1.Islbldelivery = "";
                _frm1.gettotal();
                if (DirectCashoutTakeaway == "Enabled")
                {
                    _frm1.callcashout("takeAway");
                }
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public string getordertype(string id)
        {
            string type = "";
            DataSet dstype = new DataSet();
            try
            {
                string q = "select OrderType from sale where id='" + id + "'";

                dstype = objCore.funGetDataSet(q);
                if (dstype.Tables[0].Rows.Count > 0)
                {
                    type = dstype.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //dstype.Dispose();
            }
            return type;
        }
        public void updateordertype(string type)
        {
            string q = "update sale set OrderType='"+type+"' where id='"+saleid+"'";
            objCore.executeQuery(q);
        }
        List<DineInTableModel> tablist = new List<DineInTableModel>();
        List<PendingBills> tablistbooked = new List<PendingBills>();
        private void AddDisplayControls()
        {
            try
            {
                
                tbltables.Controls.Clear();
                //Clear out the existing row and column styles
                tbltables.ColumnStyles.Clear();
                tbltables.RowStyles.Clear();
                ds = new DataSet();
                int rowsize = 0;
                try
                {
                    ds = objCore.funGetDataSet("select * from Tablelayout where tablename='Tables'");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tbltables.ColumnCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Columns"].ToString());
                        tbltables.RowCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Rows"].ToString());
                        //rowsize = Convert.ToInt32(ds.Tables[0].Rows[0]["RowSize"].ToString());
                    }
                    else
                    {
                        //Assign table no of rows and column
                        tbltables.ColumnCount = 3;
                        tbltables.RowCount = 4;
                    }
                }
                catch (Exception ex)
                {

                }
                //Assign table no of rows and column            
                float cperc = 100 / tbltables.ColumnCount;
                float rperc = 100 / tbltables.RowCount;
                //tableLayoutPanelmenugroup.Height = Convert.ToInt32(rowsize * tableLayoutPanelmenugroup.RowCount);
                for (int i = 0; i < tbltables.ColumnCount; i++)
                {
                    tbltables.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));
                    for (int j = 0; j < tbltables.RowCount; j++)
                    {
                        if (i == 0)
                        {
                            //defining the size of cell
                            tbltables.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
                        }
                    }
                }
                tbltables.HorizontalScroll.Enabled = false;
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds.Dispose();
            }
        }
        private void AddDisplayControlsFloor(int count)
        {
            try
            {
                tblfloor.Controls.Clear();
               
                //Clear out the existing row and column styles
                tblfloor.ColumnStyles.Clear();
                tblfloor.RowStyles.Clear();
                ds = new DataSet();
                int rowsize = 0;
                try
                {
                    tblfloor.ColumnCount = count;
                    tblfloor.RowCount = 1;
                }
                catch (Exception ex)
                {

                }
                //Assign table no of rows and column            
                float cperc = 100 / tblfloor.ColumnCount;
                float rperc = 100 / tblfloor.RowCount;
                //tableLayoutPanelmenugroup.Height = Convert.ToInt32(rowsize * tableLayoutPanelmenugroup.RowCount);
                for (int i = 0; i < tblfloor.ColumnCount; i++)
                {
                    tblfloor.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));
                    for (int j = 0; j < tblfloor.RowCount; j++)
                    {
                        if (i == 0)
                        {
                            //defining the size of cell
                            tblfloor.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
                        }
                    }
                }
                tblfloor.HorizontalScroll.Enabled = false;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                ds.Dispose();
            }
        }
        int tcolms = 0;
        int trows = 0;
        private void Addbutton(Button btn)
        {
            //// panel7.SuspendLayout();
            try
            {
                btn.Dock = DockStyle.Fill;


                tblfloor.Controls.Add(btn, tcolms, trows);
                tcolms++;
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                if (tcolms >= tblfloor.ColumnCount)
                {
                    tcolms = 0;
                    trows++;
                }
            }
            catch (Exception ex)
            {


            }
            // panel7.ResumeLayout(false);
        }
        private void Addbutton1(Button btn)
        {
            //// panel7.SuspendLayout();
            try
            {
                btn.Dock = DockStyle.Fill;


                tbltables.Controls.Add(btn, tcolms, trows);
                tcolms++;
                //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
                if (tcolms >= tbltables.ColumnCount)
                {
                    tcolms = 0;
                    trows++;
                }
            }
            catch (Exception ex)
            {


            }
            // panel7.ResumeLayout(false);
        }
        string DirectCashoutTakeaway = "", DirectCashoutDelivery = "";
        public string multibranches = "", branchid = "";
        private void NewBillType_Load(object sender, EventArgs e)
        {
            fillarea();
            try
            {
                string no = POSRestaurant.Properties.Settings.Default.MainScreenLocation.ToString();
                if (Convert.ToInt32(no) > 0)
                {


                    Screen[] sc;
                    sc = Screen.AllScreens;
                    this.StartPosition = FormStartPosition.Manual;
                    int no1 = Convert.ToInt32(no);
                    this.Location = Screen.AllScreens[no1].WorkingArea.Location;
                    this.WindowState = FormWindowState.Normal;
                    this.WindowState = FormWindowState.Maximized;

                }

            }
            catch (Exception ex)
            {

            }
            if (POSRestaurant.Properties.Settings.Default.ordertype.ToLower() == "dinein")
            {

                tabControl1.SelectedIndex = 1;
            }
            DirectCashoutTakeaway = _frm1.DirectCashoutTakeaway; DirectCashoutDelivery = _frm1.DirectCashoutDelivery;
            if (DirectCashoutTakeaway == "" || DirectCashoutDelivery == "")
            {
                _frm1.getdirectcashoutstatus();
                DirectCashoutTakeaway = _frm1.DirectCashoutTakeaway;
                DirectCashoutDelivery = _frm1.DirectCashoutDelivery;
            }
            if (DirectCashoutDelivery == "Enabled")
            {
                lblreceivable.Visible = true;
                cmbcustomers.Visible = true;
                checkBox1.Visible = true;
            }
            else
            {
                lblreceivable.Visible = false;
                cmbcustomers.Visible = false;
                checkBox1.Visible = false;
            }
            try
            {
               DataSet dsdis = new DataSet();
                try
                {
                    string q = "select id as SaleId,Customer,OrderType from sale where billstatus='Pending' and date='"+date+"' ";
                    if (multibranches == "Enabled")
                    {
                        q = "select id as SaleId,Customer,OrderType from sale where billstatus='Pending' and date='" + date + "' and branchid='"+branchid+"' ";
                    }
                    dsdis = objCore.funGetDataSet(q);
                    if (dsdis.Tables[0].Rows.Count > 0)
                    {
                        string JSONString = string.Empty;
                        JSONString = JsonConvert.SerializeObject(dsdis.Tables[0]);
                        tablistbooked = (List<PendingBills>)JsonConvert.DeserializeObject(JSONString, typeof(List<PendingBills>));
                    }
                }
                catch (Exception ex)
                {


                }
                finally
                {
                    dsdis.Dispose();
                }
                tablist = _frm1.tableslist;
                if (tablist.Count > 0)
                {
                    textBox1.ReadOnly = true;
                }
                var tablistfiltered = tablist.Select(i => i.Floor).Distinct();
                try
                {
                    AddDisplayControlsFloor(tablistfiltered.ToList().Count);
                    AddDisplayControls();
                }
                catch (Exception ex)
                {
                    
                }
                foreach (var item in tablistfiltered)
                {
                    vButton btn = new vButton();
                    btn.Width = 150;
                    btn.Height = 70;
                    btn.Font = new Font("", Convert.ToInt32(8), FontStyle.Bold);
                   
                    btn.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.METROORANGE;
                    btn.Text = item;
                    btn.Click += new EventHandler(tablefloor_Click);
                    
                   // flowlaoutdinein.Controls.Add(btn);
                    Addbutton(btn);

                }
            }
            catch (Exception ex)
            {
                
            }
            this.TopMost = false;
            txtname.Focus();
            focusedTextbox = txtname;
            //button2.Text = "!";
            fillitems();
            fill();

            // shiftkey();
            try
            {
                //if (type == "takeaway")
                //{
                //    label2.Text = "Take Away Customer Id";
                //}
                //if (type == "carhope")
                //{
                //    label2.Text = "Carhope Customer Id";
                //}
                _frm1.Enabled = false;

            }
            catch (Exception ex)
            {


            }
            try
            {
                try
                {
                    ds = new DataSet();
                    string q = "select id,name from ResturantStaff where Usertype='Waiter' and cashierid='" + POSRestaurant.Properties.Settings.Default.UserId.ToString() + "'";

                    ds = objCore.funGetDataSet(q);
                   
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        ds = new DataSet();
                        q = "select id,name from ResturantStaff where Usertype='Waiter' order by name";

                        ds = objCore.funGetDataSet(q);

                    }
                    comboBox1.DataSource = ds.Tables[0];
                    comboBox1.ValueMember = "id";
                    comboBox1.DisplayMember = "name";

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
            }
            try
            {
                try
                {
                    DataSet ds1 = new DataSet();
                    string q = "select id,name from ordersource where status='active' ORDER BY name ";
                    SqlConnection con = new SqlConnection(cs);
                    try
                    {
                        if (con.State == ConnectionState.Open)
                        { con.Close(); }
                        con.Open();
                        SqlCommand com = new SqlCommand(q, con);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(ds1);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }
                    // ds = objCore.funGetDataSet(q);

                    cmbsource.DataSource = ds1.Tables[0];
                    cmbsource.ValueMember = "id";
                    cmbsource.DisplayMember = "name";

                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {


            }
            if (rename == "yes")
            {
                var list = tablistbooked.Where(x => x.SaleId == saleid).ToList();
                if (list.Count > 0)
                {
                    if (list[0].OrderType == "Take Away")
                    {
                        tabControl1.SelectedIndex = 0;
                        txtname.Text = list[0].Customer;
                        vButton46.Enabled = false;
                        vButton48.Enabled = false;

                        string q11 = "update sale set POSfee='" + 0 + "'  where id='" + saleid + "'";
                        objCore.executeQuery(q11);
                    }
                    if (list[0].OrderType == "Dine In")
                    {
                        tabControl1.SelectedIndex = 1;
                        vButton31.Enabled = false;
                        vButton48.Enabled = false;
                        try
                        {
                            DataSet ds = new DataSet();
                            string q = "select * from DinInTables where saleid='" + saleid + "'";
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                textBox1.Text = list[0].Customer;
                                txtguest.Text = ds.Tables[0].Rows[0]["Guests"].ToString();
                                comboBox1.SelectedValue = ds.Tables[0].Rows[0]["WaiterId"].ToString();
                            }
                            
                           
                        }
                        catch (Exception ex)
                        {
                            
                        }
                        string q11 = "update sale set POSfee='" + 0 + "'  where id='" + saleid + "'";
                        objCore.executeQuery(q11);
                    }
                    if (list[0].OrderType == "Delivery")
                    {
                        tabControl1.SelectedIndex = 2;
                        vButton46.Enabled = false;
                        vButton31.Enabled = false;
                        try
                        {
                            DataSet ds = new DataSet();
                            string q = "select * from delivery where saleid='" + saleid + "'";
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                                txtdeliveryname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                                txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                                txtnote.Text = ds.Tables[0].Rows[0]["Note"].ToString();
                                cmbsource.SelectedValue = ds.Tables[0].Rows[0]["type"].ToString();
                            }


                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
                
            }
        }
        private void tablefloor_Click(object sender, EventArgs e)
        {
            tcolms = 0;
            trows = 0;
            try
            {
                tbltables.Controls.Clear();
                //Clear out the existing row and column styles
                //tbltables.ColumnStyles.Clear();
                //tbltables.RowStyles.Clear();
              //  flowLayouttable.Controls.Clear();
                vButton btnrecv = sender as vButton;
                foreach (var item in tablist.Where(x => x.Floor == btnrecv.Text))
                {
                    vButton btn = new vButton();
                    btn.Width = 150;
                    btn.Height = 70;
                    btn.Font = new Font("", Convert.ToInt32(8), FontStyle.Bold);
                    try
                    {
                        var booked = tablistbooked.Where(x => x.Customer == item.TableNo && x.OrderType == "Dine In").ToList();
                        if (booked.Count > 0)
                        {
                            btn.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.METROGREEN;
                            btn.Enabled = false;
                        }
                        else
                        {
                            btn.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.METROBLUE;
                        }

                    }
                    catch (Exception ex)
                    {
                        btn.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.METROBLUE;
                    }
                    btn.Text = item.TableNo;
                    btn.Click += new EventHandler(table_Click);
                    //flowLayouttable.Controls.Add(btn);
                    Addbutton1(btn);
                    
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private void table_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            textBox1.Text = btn.Text;
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

                if (focusedTextbox != null)
                {

                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            //shiftkey();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            try
            {
                //if (txtphone.Text == string.Empty)
                //{
                //    MessageBox.Show("Please Enter Phone no");
                //    txtphone.Focus();
                //    return;
                //}
                if (txtdeliveryname.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter name");
                    txtdeliveryname.Focus();
                    return;
                }
                //if (txtaddress.Text == string.Empty)
                //{
                //    MessageBox.Show("Please Enter address");
                //    txtaddress.Focus();
                //    return;
                //}
                string phone = "", address = "";
                if (chkphone1.Checked == true)
                {
                    phone = txtphone.Text;
                }
                if (chkphone2.Checked == true)
                {
                    phone = txtphone2.Text;
                }
                if (chkphone3.Checked == true)
                {
                    phone = txtphone3.Text;
                }
                if (chkphone4.Checked == true)
                {
                    phone = txtphone4.Text;
                }

                if (chkaddress1.Checked == true)
                {
                    address = txtaddress.Text;
                }
                if (chkaddress2.Checked == true)
                {
                    address = txtaddress2.Text;
                }
                if (chkaddress3.Checked == true)
                {
                    address = txtaddress3.Text;
                }
                if (chkaddress4.Checked == true)
                {
                    address = txtaddress4.Text;
                }
                string q = "",phone2=txtalternative.Text;
                if (rename == "yes")
                {
                    try
                    {
                        q = "update sale set ordertype='Delivery',customer='D-" + txtdeliveryname.Text.Trim() + "' where id='" + saleid + "'";
                        objCore.executeQuery(q);
                        q = "update Delivery set PromisedTime ='" + txttime.Text + "',Name ='" + txtdeliveryname.Text + "',Phone='" + phone + "',Phone2='" + phone2 + "',Address='" + address + "',Note='" + txtnote.Text + "' ,type='" + cmbsource.Text + "' where saleid='" + saleid + "'";
                        objCore.executeQuery(q);
                        _frm1.getorders("new");
                        _frm1.Enabled = true;
                        this.Close();
                        return;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                int idd = 0;
                ds = new DataSet();
                ds = objCore.funGetDataSet("select max(id) as id from Delivery");               
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0][0].ToString();
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


                string date = "";
                DataSet dsdt = new DataSet();
                q = "select top(1) * from dayend order by id desc";
                SqlConnection con = new SqlConnection(cs);
                try
                {
                    if (con.State == ConnectionState.Open)
                    { con.Close(); }
                    con.Open();
                    SqlCommand com = new SqlCommand(q, con);
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    da.Fill(dsdt);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    con.Close();
                }
                //objCore = new classes.Clsdbcon();
                //dsdt = objCore.funGetDataSet(q);
                try
                {
                    if (dsdt.Tables[0].Rows.Count > 0)
                    {
                        date = dsdt.Tables[0].Rows[0]["Date"].ToString();
                    }
                }
                catch (Exception ex)
                {


                }
                _frm1.ordersource = cmbsource.Text;
                _frm1.pay("Cash", "", "0", "D-" + txtdeliveryname.Text.Trim().Replace("'", "''"), "Delivery", txtphone.Text,"");
                saleid = _frm1.getsaleid();


                q = "insert into Delivery (PromisedTime,phone2,type,id,Name, Phone,Address,Note,Date,Saleid,Status) values ('" + txttime.Text + "','" + phone2 + "','" + cmbsource.Text + "','" + idd + "','" + txtdeliveryname.Text.Trim().Replace("'", "''") + "','" + phone.Trim().Replace("'", "''") + "','" + address.Trim().Replace("'", "''") + "','" + txtnote.Text.Trim().Replace("'", "''") + "','" + date + "','" + saleid + "','Pending')";
                _frm1.deliveryid = idd.ToString();
                objCore.executeQuery(q);
                updateordertype("Delivery");
                _frm1.Enabled = true;
                //Sale.SaleAfter obj = new Sale.SaleAfter(_frm1);
                //obj.total = total;
                //obj.id = saleid.ToString();
                //obj.advance = advance;
                //obj.name = name;
                //obj.type = "Delivery";
                //obj.Show();
                //_frm1.Islbldelivery = "Delivery";
                //_frm1.newbill();

                if (DirectCashoutDelivery == "Enabled" && checkBox1.Checked==false)
                {
                    _frm1.callcashout("");

                    if (cmbcustomers.Text == "No Receivable")
                    {

                        _frm1.pay("Cash", _frm1.cashoutnettotal, "0", name, type, "", _frm1.cashoutgsttype);
                        _frm1.billtype(id.ToString(), "Cash", _frm1.cashoutnettotal.ToString().Trim(), "0");
                    }
                    else
                    {
                        _frm1.pay("Receivable", _frm1.cashoutnettotal.ToString(), "0", name, type, "", _frm1.cashoutgsttype);
                        _frm1.billtype(id.ToString(), "Receivable", _frm1.cashoutnettotal.ToString().Trim(), cmbcustomers.SelectedValue.ToString());
                    }
                }

                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public class phonelist{
            public string  Phone { get; set; }
        }
        List<phonelist> ListPhone = new List<phonelist>();
        protected void getphone2()
        {
            try
            {
                for (int j = 0; j < ListPhone.Count; j++)
                {
                    DataSet dsinfo = new DataSet();
                    string q = "select   * from Delivery where phone='" + txtphone.Text.Trim() + "' or phone2='" + txtphone.Text.Trim() + "' order by id desc";
                    dsinfo = objCore.funGetDataSet(q);
                    for (int i = 0; i < dsinfo.Tables[0].Rows.Count; i++)
                    {

                        if (ListPhone.Count == 4)
                        {
                            filltextboxes();
                            return;
                        }
                        string phone2 = dsinfo.Tables[0].Rows[0]["Phone2"].ToString();
                        if (phone2.Length > 0)
                        {
                            int res = 0;

                            try
                            {
                                res = ListPhone.Where(x => x.Phone == phone2).ToList().Count;
                            }
                            catch (Exception ex)
                            {


                            }
                            if (res == 0)
                            {
                                ListPhone.Add(new phonelist { Phone = phone2 });
                            }
                        }
                        string phone1 = dsinfo.Tables[0].Rows[0]["Phone"].ToString();
                        if (phone1.Length > 0)
                        {
                            int res = 0;

                            try
                            {
                                res = ListPhone.Where(x => x.Phone == phone1).ToList().Count;
                            }
                            catch (Exception ex)
                            {


                            }
                            if (res == 0)
                            {
                                ListPhone.Add(new phonelist { Phone = phone1 });
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                
            }

        }
        protected void filltextboxes()
        {
            try
            {
                for (int i = 0; i < ListPhone.Count; i++)
                {
                    string address = "";
                    string q = "select   distinct Address from Delivery where phone='" + ListPhone[i].Phone + "'";
                    DataSet dsaddress = new DataSet();
                    dsaddress = objCore.funGetDataSet(q);
                    if (dsaddress.Tables[0].Rows.Count > 0)
                    {

                        address = dsaddress.Tables[0].Rows[0]["Address"].ToString();

                    }
                    else
                    {

                    }
                    if (i == 1)
                    {
                        txtphone2.Text = ListPhone[i].Phone;
                        txtaddress2.Text = address;
                    }
                    if (i == 2)
                    {
                        txtphone3.Text = ListPhone[i].Phone;
                        txtaddress3.Text = address;
                    }
                    if (i == 3)
                    {
                        txtphone4.Text = ListPhone[i].Phone;
                        txtaddress4.Text = address;
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private void txtphone_Leave(object sender, EventArgs e)
        {
            try
            {

                DataSet dsinfo = new DataSet();
                string q = "select   * from Delivery where phone='" + txtphone.Text.Trim() + "' or phone2 ='" + txtphone.Text.Trim() + "' order by id desc";
                dsinfo = objCore.funGetDataSet(q);
                for (int i = 0; i < dsinfo.Tables[0].Rows.Count; i++)
                {
                    if (ListPhone.Count == 4)
                    {
                        filltextboxes();
                        return;
                    }
                    txtdeliveryname.Text = dsinfo.Tables[0].Rows[0]["Name"].ToString();
                    txtaddress.Text = dsinfo.Tables[0].Rows[0]["Address"].ToString();
                    ListPhone.Add(new phonelist { Phone = txtphone.Text.Trim() });
                    string phone1 = dsinfo.Tables[0].Rows[0]["Phone"].ToString();
                    if (phone1.Length > 0)
                    {
                        int res = 0;

                        try
                        {
                            res = ListPhone.Where(x => x.Phone == phone1).ToList().Count;
                        }
                        catch (Exception ex)
                        {


                        }
                        if (res == 0)
                        {
                            ListPhone.Add(new phonelist { Phone = phone1 });
                        }
                    }
                    string phone2 = dsinfo.Tables[0].Rows[0]["Phone2"].ToString();
                    if (phone2.Length > 0)
                    {
                        int res = 0;

                        try
                        {
                            res = ListPhone.Where(x => x.Phone == phone2).ToList().Count;
                        }
                        catch (Exception ex)
                        {


                        }
                        if (res == 0)
                        {
                            ListPhone.Add(new phonelist { Phone = phone2 });
                        }
                    }

                }
                getphone2();
                filltextboxes();
            }
            catch (Exception ex)
            {


            }

        }
        private void txtphone_TextChanged(object sender, EventArgs e)
        {
            }

        private void button45_Click(object sender, EventArgs e)
        {
            _frm1.cleartables();
            _frm1.Enabled = true;
            _frm1.Islbldelivery = "Not Selected";
            //_frm1.refereshtabs();
            //_frm1.TopMost = true;
            this.Close();
        }

        private void vButton32_Click(object sender, EventArgs e)
        {
            try
            {
                vButton t = (sender) as vButton;

                if (focusedTextbox != null && focusedTextbox.ReadOnly!=true)
                {

                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text.Replace("&&", "&");
                        focusedTextbox.SelectionStart = focusedTextbox.Text.Length;
                        strt = focusedTextbox.SelectionStart;
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void vButton29_Click(object sender, EventArgs e)
        {
            _frm1.cleartables();
            _frm1.Enabled = true;
            _frm1.Islbldelivery = "Not Selected";
            //  _frm1.refereshtabs();
            // _frm1.TopMost = true;
            this.Close();
        }

        private void vButton30_Click(object sender, EventArgs e)
        {
            focusedTextbox.Text = focusedTextbox.Text + " ";
            focusedTextbox.Focus();
            focusedTextbox.SelectionStart = focusedTextbox.Text.Length;
            strt = focusedTextbox.SelectionStart;
        }
        public static int strt = 0;
        private void vButton28_Click(object sender, EventArgs e)
        {
            try
            {
                if (strt > 0)
                {
                    int index = focusedTextbox.SelectionStart;

                    focusedTextbox.Text = focusedTextbox.Text.Remove(strt - 1, 1);
                    // txtcashreceived.Select(index - 1, 1);
                    //txtcashreceived.Select();
                    strt = strt - 1;
                    focusedTextbox.Focus();
                    focusedTextbox.SelectionStart = focusedTextbox.Text.Length;
                    strt = focusedTextbox.SelectionStart;
                    //txtcashreceived.Focus(); 
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbsource_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtguest_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = txtguest as TextBox;
                if (txt.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        txt.Text = "";
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                 txtguest.Text = "";
            }
        }

        private void chkaddress3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkaddress3.Checked == true)
            {
                if (txtaddress3.Text.Length == 0)
                {
                    chkaddress1.Checked = true;
                }
                else
                {
                    chkaddress1.Checked = false;
                    chkaddress2.Checked = false;
                    chkaddress4.Checked = false;
                }
            }
        }

        private void chkaddress2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkaddress2.Checked == true)
            {
                if (txtaddress2.Text.Length == 0)
                {
                    chkaddress1.Checked = true;
                }
                else
                {
                    chkaddress1.Checked = false;
                    chkaddress3.Checked = false;
                    chkaddress4.Checked = false;
                }
            }
        }

        private void chkaddress4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkaddress4.Checked == true)
            {
                if (txtaddress4.Text.Length == 0)
                {
                    chkaddress1.Checked = true;
                }
                else
                {
                    chkaddress1.Checked = false;
                    chkaddress3.Checked = false;
                    chkaddress2.Checked = false;
                }
            }
        }

        private void chkphone1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkphone1.Checked == true)
            {
                
                {
                    chkphone2.Checked = false;
                    chkphone3.Checked = false;
                    chkphone4.Checked = false;
                }
            }
        }

        private void chkaddress1_CheckedChanged(object sender, EventArgs e)
        {
            if (chkaddress1.Checked == true)
            {

                {
                    chkaddress4.Checked = false;
                    chkaddress3.Checked = false;
                    chkaddress2.Checked = false;
                }
            }
        }

        private void chkphone2_CheckedChanged(object sender, EventArgs e)
        {
            if (chkphone2.Checked == true)
            {
                if (txtphone2.Text.Length == 0)
                {
                    chkphone1.Checked = true;
                }
                else
                {
                    chkphone1.Checked = false;
                    chkphone3.Checked = false;
                    chkphone4.Checked = false;
                }
            }
        }

        private void chkphone3_CheckedChanged(object sender, EventArgs e)
        {
            if (chkphone3.Checked == true)
            {
                if (txtphone3.Text.Length == 0)
                {
                    chkphone1.Checked = true;
                }
                else
                {
                    chkphone1.Checked = false;
                    chkphone2.Checked = false;
                    chkphone4.Checked = false;
                }
            }
        }

        private void chkphone4_CheckedChanged(object sender, EventArgs e)
        {
            if (chkphone4.Checked == true)
            {
                if (txtphone4.Text.Length == 0)
                {
                    chkphone1.Checked = true;
                }
                else
                {
                    chkphone1.Checked = false;
                    chkphone3.Checked = false;
                    chkphone2.Checked = false;
                }
            }
        }

        private void vButton49_Click(object sender, EventArgs e)
        {
            try
            {
                ds = new DataSet();
                int idd = 0;
                ds = new DataSet();
                ds = objCore.funGetDataSet("select max(id) as id from Delivery");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0][0].ToString();
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
                string q = "insert into Delivery (phone2,type,id,Name, Phone,Address,Note,Date,Saleid,Status) values ('" + txtalternative.Text + "','" + cmbsource.Text + "','" + idd + "','" + txtdeliveryname.Text.Trim().Replace("'", "''") + "','" + txtphone.Text + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + txtnote.Text.Trim().Replace("'", "''") + "','" + date + "','','Pending')";
                int res = objCore.executeQueryint(q);
                if (res > 0)
                {
                    MessageBox.Show("Name and Address Saved Successfully");
                }
                else
                {
                    MessageBox.Show("Failed to save record");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

      
    }
}
