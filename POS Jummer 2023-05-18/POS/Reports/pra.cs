using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Reports
{
    public partial class pra : Form
    {
        public pra()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

        private void pra_Load(object sender, EventArgs e)
        {
            string q = "select * from prainfo";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string date = Convert.ToDateTime(ds.Tables[0].Rows[0]["date"]).ToString("yyyy-MM-dd");
                lblname.Text="NTN:  "+ds.Tables[0].Rows[0]["ntn"].ToString()+"   , Branch Code:  "+ds.Tables[0].Rows[0]["branchcode"].ToString();
                q = "select count(*) from sale where gst>0 and billstatus='Paid' and date>'"+date+"'";
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lbltotal.Text = ds.Tables[0].Rows[0][0].ToString();
                }
                q = "select count(*) from sale where gst>0 and billstatus='Paid' and date>'" + date + "' and uploadstatus='Processed'";
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lbluploaded.Text = ds.Tables[0].Rows[0][0].ToString();
                }
                q = "select count(*) from sale where gst>0 and billstatus='Paid' and date>'" + date + "' and uploadstatus='Pending'";
                ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    lblpending.Text = ds.Tables[0].Rows[0][0].ToString();
                }
            }
        }
    }
}
