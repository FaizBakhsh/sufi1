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
    public partial class DS : Form
    {
        POSRestaurant.Sale.RestSale _frm;
        public DS(RestSale frm)
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
                string q = "select id,name from ResturantStaff where Usertype='Staff'  order by name";
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
            //this.TopMost = true;
            fill();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Addstaff obj = new Addstaff(this);
            //obj.Show();
        }
        public string id = "0", total = "0", name = "", type = "",phone="",gsttype="",saleid="";
        private void vButton18_Click(object sender, EventArgs e)
        {
         
            _frm.saleDS("Cash", total.ToString(), "0", name, type, phone, gsttype,comboBox1.SelectedValue.ToString(),id);
            this.Close();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
           // _frm.TopMost = true;
            this.Close();
        }
    }
}
