using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.Sale
{
    public partial class NewBillType : Form
    {
        private TextBox focusedTextbox = null;
        private RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        string cs = POSRestaurant.Properties.Settings.Default.ConnectionString;
        DataSet ds;
        public string type = "", id = "",saleid="",total="0",name="",advance="0";

        public NewBillType(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
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
            _frm1.pay("Cash", "", "0", textBox1.Text, "", "", "");
            saleid = _frm1.getsaleid();
            q = "insert into DinInTables (id,TableNo,Saleid,Date,time,Status,WaiterId) values('" + id + "','" + tbno + "','" + saleid + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','Pending','" + comboBox1.SelectedValue + "')";
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
            if (textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Table no");
                return;
            }
            else
            {
                getsaleid(textBox1.Text);
                //_frm1.Islbldelivery = "Dine In";
                //_frm1.newbill();
            }
        }
        private void button34_Click(object sender, EventArgs e)
        {
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
                _frm1.pay("Cash", "", "0", txtname.Text, "","","");
                
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
                //Sale.SaleAfter obj = new Sale.SaleAfter(_frm1);
                //obj.total = _frm1.getnetvalue(); ;
                //obj.id = saleid.ToString();
                //obj.advance = advance;
                //obj.name = name;
                //obj.type = "Take Away";
                //obj.Show();
                
               // _frm1.newbill();
               
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public void updateordertype(string type)
        {
            string q = "update sale set OrderType='"+type+"' where id='"+saleid+"'";
            objCore.executeQuery(q);
        }
        public void shiftkey()
        {
            if (button2.Text != "!")
            {
                button2.Text = "!";
                button3.Text = "@";
                button4.Text = "#";
                button5.Text = "$";
                button6.Text = "%";
                button7.Text = "^";
                button8.Text = "&&";
                button9.Text = "*";
                button10.Text = "(";
                button11.Text = ")";
                button12.Text = "Q";
                button16.Text = "W";
                button18.Text = "E";
                button20.Text = "R";
                button22.Text = "T";
                button21.Text = "Y";
                button19.Text = "U";
                button17.Text = "I";
                button15.Text = "O";
                button13.Text = "P";

                button23.Text = "A";
                button25.Text = "S";
                button27.Text = "D";
                button29.Text = "F";
                button31.Text = "G";
                button30.Text = "H";
                button28.Text = "J";
                button26.Text = "K";
                button24.Text = "L";

                button33.Text = "Z";
                button35.Text = "X";
                button37.Text = "C";
                button39.Text = "V";
                button41.Text = "B";
                button40.Text = "N";
                button38.Text = "M";
                // button36.Text = "o";


            }
            else
            {
                button2.Text = "1";
                button3.Text = "2";
                button4.Text = "3";
                button5.Text = "4";
                button6.Text = "5";
                button7.Text = "6";
                button8.Text = "7";
                button9.Text = "8";
                button10.Text = "9";
                button11.Text = "0";
                button12.Text = "q";
                button16.Text = "w";
                button18.Text = "e";
                button20.Text = "r";
                button22.Text = "t";
                button21.Text = "y";
                button19.Text = "u";
                button17.Text = "i";
                button15.Text = "o";
                button13.Text = "p";

                button23.Text = "a";
                button25.Text = "s";
                button27.Text = "d";
                button29.Text = "f";
                button31.Text = "g";
                button30.Text = "h";
                button28.Text = "j";
                button26.Text = "k";
                button24.Text = "l";

                button33.Text = "z";
                button35.Text = "x";
                button37.Text = "c";
                button39.Text = "v";
                button41.Text = "b";
                button40.Text = "n";
                button38.Text = "m";


            }
        }
        private void NewBillType_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            txtname.Focus();
            focusedTextbox = txtname;
            button2.Text = "!";
            fillitems();
            shiftkey();
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
                    string q = "select id,name from ResturantStaff ";
                    SqlConnection con = new SqlConnection(cs);


                    try
                    {


                        if (con.State == ConnectionState.Open)
                        { con.Close(); }
                        con.Open();
                        SqlCommand com = new SqlCommand(q, con);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(ds);
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        con.Close();
                    }
                    // ds = objCore.funGetDataSet(q);

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

                throw;
            }
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
            shiftkey();
        }

        private void button46_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtphone.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Phone no");
                    txtphone.Focus();
                    return;
                }
                if (txtdeliveryname.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter name");
                    txtdeliveryname.Focus();
                    return;
                }
                if (txtaddress.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter address");
                    txtaddress.Focus();
                    return;
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
                _frm1.pay("Cash", "", "0", "Delivery", "",txtphone.Text,"");
                saleid = _frm1.getsaleid();
                q = "insert into Delivery (id,Name, Phone,Address,Note,Date,Saleid,Status) values ('" + idd + "','" + txtdeliveryname.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + txtnote.Text.Trim().Replace("'", "''") + "','" + date + "','"+saleid+"','Pending')";
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
                
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtphone_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataSet dsinfo = new DataSet();
                string q = "select * from Delivery where phone='" + txtphone.Text.Trim() + "'";
                dsinfo = objCore.funGetDataSet(q);
                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                    txtdeliveryname.Text = dsinfo.Tables[0].Rows[0]["Name"].ToString();
                    txtaddress.Text = dsinfo.Tables[0].Rows[0]["Address"].ToString();
                }
                else
                {
                    txtdeliveryname.Text = "";
                    txtaddress.Text = "";
                }
            }
            catch (Exception ex)
            {
                
               
            }
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
    }
}
