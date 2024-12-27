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
    public partial class Dispatch : Form
    {
        public string saleid = "";
        BillRecallold _frm;
        public Dispatch(BillRecallold frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        POSRestaurant.classes.Clsdbcon Objcore = new classes.Clsdbcon();
        private void Dispatch_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            try
            {

                string q = "select id,name from resturantstaff where usertype='Rider'";
                DataSet ds = new DataSet();
                ds = Objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    comboBox1.DataSource = ds.Tables[0];
                    comboBox1.ValueMember = "id";
                    comboBox1.DisplayMember = "name";
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "update delivery set status='Dispatched',dispatchedtime='"+DateTime.Now.ToShortTimeString()+"', riderid='" + comboBox1.SelectedValue + "' where saleid='" + saleid + "'";
                int res = Objcore.executeQueryint(q);
                if (res > 0)
                {
                    _frm.getdata("");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unable to assign Rider");
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string q = "update delivery set status='Delivered',deliveredtime='" + DateTime.Now.ToShortTimeString() + "' where saleid='" + saleid + "'";
                int res = Objcore.executeQueryint(q);
                if (res > 0)
                {
                    _frm.getdata("");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Unable to assign Rider");
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
