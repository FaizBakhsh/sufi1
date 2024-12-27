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
    public partial class Receiveables : Form
    {
        POSRestaurant.Sale.RestSale _frm;
        public Receiveables(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public void fill()
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "select id,name from Customers order by name";
                ds = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void Receiveables_Load(object sender, EventArgs e)
        {
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
                    this.StartPosition = FormStartPosition.CenterScreen;
                    this.WindowState = FormWindowState.Maximized;

                }

            }
            catch (Exception ex)
            {

            }
            this.TopMost = false;
            fill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Addstaff obj = new Addstaff(this);
            obj.Show();
        }
        public string id = "0", total = "0", name = "", type = "",phone="",gsttype="";
        private void vButton18_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please Select Customer");
                return;
            }

            string q = "delete from billtype where saleid='" + id + "'";
            objCore.executeQuery(q);
          
            _frm.pay("Receivable", total.ToString(), "0", name, type,phone,gsttype);
            _frm.billtype(id.ToString(), "Receivable", total.ToString().Trim(), comboBox1.SelectedValue.ToString());
           // _frm.TopMost = true;
            this.Close();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           // _frm.TopMost = true;
            this.Close();
        }
    }
}
