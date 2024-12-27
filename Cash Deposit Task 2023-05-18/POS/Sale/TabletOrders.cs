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
    public partial class TabletOrders : Form
    {
        RestSale _frm;
        public TabletOrders(RestSale frm )
        {
            InitializeComponent();
            _frm = frm;
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void getdata()
        {
            try
            {
                string q = "SELECT DISTINCT TableNo, SUM(price) AS TotalBillAmount FROM  TabletOrders WHERE (status = 'Pending') GROUP BY tableno";
                DataSet ds = objcore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void TabletOrders_Load(object sender, EventArgs e)
        {
            getdata();
        }
        bool chk = false;
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (chk == true)
            {
                return;
            }
            string tb = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            _frm.calltabletorder(tb);
            chk = true;
            this.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (chk == true)
            {
                return;
            }
            string tb = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            _frm.calltabletorder(tb);
            chk = true;
            this.Close();
        }
    }
}
