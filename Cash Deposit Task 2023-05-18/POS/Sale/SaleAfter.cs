using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
namespace POSRestaurant.Sale
{
    public partial class SaleAfter : Form
    {
        public string total = "", name = "", type = "", phone = "", billtype = "",gsttype="",selecttype="";
        private RestSale _frm;
        public string advance = "0";
        public SaleAfter(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            txtcashreceived.Text = txtcashreceived.Text + btn.Text;
            txtcashreceived.Focus();
            txtcashreceived.SelectionStart = txtcashreceived.Text.Length;
            strt = txtcashreceived.SelectionStart;
        }
        private void AddDisplayControls()
        {
            try
            {

                tbltables.Controls.Clear();
                //Clear out the existing row and column styles
                tbltables.ColumnStyles.Clear();
                tbltables.RowStyles.Clear();
               DataSet ds = new DataSet();
                int rowsize = 0;
                try
                {
                    ds = Objcore.funGetDataSet("select * from Tablelayout where tablename='Tables'");
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
               // ds.Dispose();
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
               DataSet ds = new DataSet();
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
               // ds.Dispose();
            }
        }
        int tcolms = 0;
        int trows = 0;
        public static int strt = 0;
        private void txtcashreceived_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtcashreceived.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtcashreceived.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                        return;
                    }
                }
                double rem = Convert.ToDouble(txtcashreceived.Text) - Convert.ToDouble(txttotal.Text);
                txtremaining.Text = rem.ToString();
            }
            catch (Exception ex)
            {
                
            }
        }

        private void txtcashreceived_Enter(object sender, EventArgs e)
        {
            strt = txtcashreceived.SelectionStart;
        }

        private void vButton14_Click(object sender, EventArgs e)
        {
            if (txtcashreceived.Text.Contains("."))
            { }
            else
            {
                txtcashreceived.Text = txtcashreceived.Text + vButton14.Text;

            }
            txtcashreceived.Focus();
            txtcashreceived.SelectionStart = txtcashreceived.Text.Length;
            strt = txtcashreceived.SelectionStart;
        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            try
            {
                if (strt > 0)
                {
                    int index = txtcashreceived.SelectionStart;

                    txtcashreceived.Text = txtcashreceived.Text.Remove(strt - 1, 1);
                    // txtcashreceived.Select(index - 1, 1);
                    //txtcashreceived.Select();
                    strt = strt - 1;
                    txtcashreceived.Focus();
                    txtcashreceived.SelectionStart = txtcashreceived.Text.Length;
                    strt = txtcashreceived.SelectionStart;
                    //txtcashreceived.Focus(); 
                }
            }
            catch (Exception ex)
            {
            }
        }
        List<DineInTableModel> tablist = new List<DineInTableModel>();
        List<PendingBills> tablistbooked = new List<PendingBills>();
        private void SaleAfter_Load(object sender, EventArgs e)
        {
            if (selecttype == "never")
            {
                tableLayoutPanel7.Visible = true;
                tablist = _frm.tableslist;
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
            else
            {
                tableLayoutPanel7.Visible = false;
            }
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
            string status = "";

            try
            {
                status = POSRestaurant.Properties.Settings.Default.gstvariations.ToString();
            }
            catch (Exception ex)
            {
                
                
            }
            if (status != "Disabled")
            {
                if (billtype == "")
                {
                }
                else
                {
                    if (billtype == "card")
                    {
                        vButton1.Enabled = false;
                        vButton17.Enabled = false;
                    }
                    else
                    {
                        vButton2.Enabled = false;

                    }
                }
            }
            txtcashreceived.Focus();
            txttotal.Text = total;
            label1.Text = "Advance: " + advance;
            try
            {
                status = "";
                string q = "select * from  DeviceSetting where Device='DS'";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    status = ds.Tables[0].Rows[0]["Status"].ToString();
                }
                if (status == "Enabled")
                {
                    button1.Enabled = true;
                    button1.Visible = true;
                }
            }
            catch (Exception ex)
            {
                
            }
           //this.TopMost = true;
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
        private void table_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            textBox1.Text = btn.Text;

        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            
            if (billtype == "card")
            {
                DialogResult dr = MessageBox.Show("Selected GST Type is Card and You are Closing This Bill on Cash","Are You Sure to Proceed?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
            }
            if (txtcashreceived.Text == "")
            {
                txtcashreceived.Text = txttotal.Text;
            }
            if (Convert.ToDouble(txtcashreceived.Text) >= Convert.ToDouble(txttotal.Text))
            {

            }
            else
            {
                MessageBox.Show("Received amount is less than Total Bill");
                return;
            }
            this.Enabled = false;
            double t = 0;
            string temp = txttotal.Text;
            if (temp == "")
            {
                temp = "0";
            }
            t = Convert.ToDouble(temp);
            temp = advance;
            if (temp == "")
            {
                temp = "0";
            }
            t = t + Convert.ToDouble(temp);
            string q = "delete from billtype where saleid='" + id + "'";
            Objcore.executeQuery(q);
            if (tableLayoutPanel7.Visible == true)
            {
                if (textBox1.Text.Length > 0)
                {
                    // _frm.pay("Cash", "", "0", tbno, "Dine In", "", "");
                    _frm.pay("Cash", txtcashreceived.Text, txtremaining.Text, textBox1.Text, "Dine In", "", gsttype);
                  
                }
                else
                {
                    _frm.pay("Cash", txtcashreceived.Text, txtremaining.Text, name, "Take Away", "", gsttype);
                }
            }
            else
            {
                _frm.pay("Cash", txtcashreceived.Text, txtremaining.Text, name, type, phone, gsttype);
            }
            _frm.billtype(id.ToString(), "Cash", t.ToString().Trim(), "0");
            if (checkbilltype(id.ToString()) != "yes")
            {
                _frm.billtype(id.ToString(), "Cash", t.ToString().Trim(), "0");
            }
            
            this.Enabled = true;
            //_frm.TopMost = true;
            SaleMessage o = new SaleMessage(_frm);
            o.Islbltotal = txttotal.Text;
            o.Islblreceived = txtcashreceived.Text;
            o.Islblchange = txtremaining.Text;
            o.Show();
            this.Close();
        }
        public void getsaleid(string tbno)
        {
            int id = 0;
            DataSet ds = new DataSet();
            ds = Objcore.funGetDataSet("select max(id) as id from DinInTables");
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
            string date = _frm.date;

           
           string q = "insert into DinInTables (ResId,Guests,id,TableNo,Saleid,Date,time,Status,WaiterId) values('','0','" + id + "','" + tbno + "','" + saleid + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','Pending','')";
            Objcore.executeQuery(q);
            updateordertype("Dine In");



            this.Close();


        }
        string saleid = "";
        public void updateordertype(string type)
        {
            string q = "update sale set OrderType='" + type + "' where id='" + saleid + "'";
            Objcore.executeQuery(q);
        }
        POSRestaurant.classes.Clsdbcon Objcore = new classes.Clsdbcon();
        public string checkbilltype(string id)
        {
            string check = "";
            DataSet ds = new DataSet();
            try
            {
                string q = "select * from BillType where saleid='" + id + "'";
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    check = "yes";
                }
            }
            catch (Exception ex)
            {
                check = "error";
            }
            finally
            {
                ds.Dispose();
            }
            return check;
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            if (billtype == "cash")
            {
                DialogResult dr = MessageBox.Show("Selected GST Type is Cash and You are Closing This Bill on Card", "Are You Sure to Proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.No)
                {
                    return;
                }
            }

            if (txtcashreceived.Text == "")
            {
                txtcashreceived.Text = txttotal.Text;
            }
            if (Convert.ToDouble(txtcashreceived.Text) >= Convert.ToDouble(txttotal.Text))
            {

            }
            else
            {
                MessageBox.Show("Received amount is less than Total Bill");
                return;
            }
            this.Enabled = false;
            double t = 0;
            string temp = txttotal.Text;
            if (temp == "")
            {
                temp = "0";
            }
            t = Convert.ToDouble(temp);
            temp = advance;
            if (temp == "")
            {
                temp = "0";
            }
            t = t + Convert.ToDouble(temp);
            //_frm.billtype(id.ToString(), "Credit Card", t.ToString().Trim());
            //_frm.pay("Credit Card", txtcashreceived.Text, txtremaining.Text, name, type);
            
            //this.Enabled = true;
            //this.Close();
            string q = "delete from billtype where saleid='" + id + "'";
            Objcore.executeQuery(q);
            banks ob = new banks(_frm);
            ob.gsttype = gsttype;
            ob.saleid = id.ToString();
            ob.total = t.ToString();
            ob.cashreceived = txtcashreceived.Text;
            ob.remaining = txtremaining.Text;
            ob.name = name;
            ob.type = type;
            ob.phone = phone;
            ob.Show();
            this.Close();
        }
        public string id = "";
        private void vButton3_Click(object sender, EventArgs e)
        {
            if (txtcashreceived.Text == "")
            {
                MessageBox.Show("Please enter amount");
                return;
            }
            if (Convert.ToDouble(txtcashreceived.Text) >= Convert.ToDouble(txttotal.Text))
            {
                MessageBox.Show("Advance value can not be greater or equal than sale value");
                return;
            }
           
            _frm.payadvance(txtcashreceived.Text);
            //if (txtcashreceived.Text == "")
            //{
            //    txtcashreceived.Text = txttotal.Text;
            //}
            //if (Convert.ToDouble(txtcashreceived.Text) >= Convert.ToDouble(txttotal.Text))
            //{

            //}
            //else
            //{
            //    MessageBox.Show("Received amount is less than Total Bill");
            //    return;
            //}
            this.Enabled = false;
            //_frm.TopMost = true;
           // _frm.pay("Master Card", txtcashreceived.Text, txtremaining.Text,name);
            this.Close();
        }

        private void vButton16_Click(object sender, EventArgs e)
        {
            _frm.Islbldelivery = "";
            _frm.gettotal();
           // _frm.TopMost = true;
            this.Close();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void vButton17_Click(object sender, EventArgs e)
        {
            string q = "select distinct gst from gst";
            DataSet ds = new DataSet();
            ds = Objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 1)
            {
                MessageBox.Show("Partial Payment Can Not Be Processed Because of Differenet Tax Ratio for Cash and Card");
                return;
            }
            if (txtcashreceived.Text == "")
            {
                MessageBox.Show("Please Enter Cash Amount");
                return;
            }
            if (Convert.ToDouble(txtcashreceived.Text) < Convert.ToDouble(txttotal.Text))
            {

            }
            else
            {
                MessageBox.Show("Received amount Can Not Be Equal Or Greater than Total Bill in Partial Payment");
                return;
            }
            this.Enabled = false;
            double t = 0;
            string temp = txtcashreceived.Text;
            if (temp == "")
            {
                temp = "0";
            }
            t = Convert.ToDouble(temp);
            temp = advance;
            if (temp == "")
            {
                temp = "0";
            }
            t = t + Convert.ToDouble(temp);
            q = "delete from billtype where saleid='" + id + "'";
            Objcore.executeQuery(q);
            banks ob = new banks(_frm);
            ob.saleid = id.ToString();
            ob.total = t.ToString();
            ob.cashreceived = txtcashreceived.Text;
            ob.remaining = txtremaining.Text.Trim().Replace("-", "");
            ob.name = name;
            ob.type = type;
            ob.phone = phone;
            ob.paymenttype = "partial";
            ob.Show();
            this.Close();

            //_frm.billtype(id.ToString(), "Cash", t.ToString().Trim());
            //_frm.billtype(id.ToString(), "Credit Card", txtremaining.Text.Trim().Replace("-", ""));
            //_frm.pay("Cash,Credit Card", txtcashreceived.Text, "0", name, type);
            this.Enabled = true;
            this.Close();
        }

        private void txtremaining_Click(object sender, EventArgs e)
        {

        }

        private void vButton18_Click(object sender, EventArgs e)
        {
            Receiveables obj = new Receiveables(_frm);
            obj.id = id;
            obj.gsttype = gsttype;
            obj.total = txttotal.Text;
            obj.name = name;
            obj.type = type;
            obj.phone = phone;
            obj.Show();
            
            this.Close();
        }

        private void vButton20_Click(object sender, EventArgs e)
        {
            try
            {
                vButton btn = sender as vButton;
                string text = btn.Text;
                string rec = txtcashreceived.Text;
                if (rec == "")
                {
                    rec = "0";
                }
                double total = Convert.ToDouble(rec) + Convert.ToDouble(text);
                txtcashreceived.Text = total.ToString();
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton19_Click(object sender, EventArgs e)
        {
            txtcashreceived.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtcashreceived.Text == "")
            {
                txtcashreceived.Text = txttotal.Text;
            }
            DS obj = new DS(_frm);
            obj.id = id;
            obj.gsttype = gsttype;
            obj.total = txttotal.Text;
            obj.name = name;
            obj.type = type;
            obj.phone = phone;
            obj.Show();
            //_frm.saleDS("Cash", txtcashreceived.Text, txtremaining.Text, name, type, phone, gsttype);
            this.Close();

        }
    }
}
