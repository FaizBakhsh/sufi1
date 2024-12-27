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
    public partial class Table : Form
    {
        private RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore =new classes.Clsdbcon();
        DataSet ds ;
        public Table(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }
       
        public void getsaleid(string tbno)
        {
            DataSet dsgetsalid = new DataSet();
            dsgetsalid = objCore.funGetDataSet("select * from DinInTables where Tableno='" + tbno + "' and status='Pending'");
            if (dsgetsalid.Tables[0].Rows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("Table is already reserved. Are you sure to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    _frm1.recalsale(dsgetsalid.Tables[0].Rows[0]["saleid"].ToString(),"no");
                    _frm1.settableno("Table no: " + tbno);
                    this.Close();
                }
                return;

            }
            else
            {
                _frm1.tableno = tbno;
                _frm1.settableno("Table no: "+tbno);
                _frm1.waiterid = comboBox1.SelectedValue.ToString();
                _frm1.Enabled = true;
                this.Close();
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {

            //if (comboBox1.Text.Trim() == string.Empty)
            //{
            //    MessageBox.Show("Please Select Waiter Name");
            //    return;
            //}
            if (textBox1.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please Enter Table no");
                return;
            }
            else
            {
                getsaleid(textBox1.Text);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _frm1.cleartables();
            _frm1.Enabled = true;
            this.Close();
        }
        string cs = POSRestaurant.Properties.Settings.Default.ConnectionString.ToString();
        private void Table_Load(object sender, EventArgs e)
        {
            _frm1.Enabled = false;
            this.TopMost = true;
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
        private TextBox focusedTextbox = null;
        private void button2_Click_1(object sender, EventArgs e)
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

        private void textBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                TextBox t = (sender) as TextBox;
                focusedTextbox = t;
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void button32_Click(object sender, EventArgs e)
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
        private void button14_Click(object sender, EventArgs e)
        {
            shiftkey();
        }
    }
}
