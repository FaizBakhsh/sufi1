using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Sale
{
    public partial class CustomerId : Form
    {
        private  Sale _frm1;
        POSRetail.classes.Clsdbcon objCore ;
        DataSet ds ;
        public CustomerId(Sale frm1)
           {
                InitializeComponent();
                _frm1 = frm1;
            }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";
            
        //}

        private void button1_Click(object sender, EventArgs e)
        {
          
               
                    
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
           
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton4_Click(object sender, EventArgs e)
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
            ds = objCore.funGetDataSet("select max(id) as id from TakeAway");
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
            ds = new DataSet();
            ds = objCore.funGetDataSet("select top(1) * from dayend where userid='"+id+"' order by id desc");
            if (ds.Tables[0].Rows.Count > 0)
            {
                date = ds.Tables[0].Rows[0]["Date"].ToString();
            }

            string q = "insert into TakeAway (id,CustomerId,Date,time,Saleid,Status) values ('" + idd + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + date + "','" + DateTime.Now.ToShortTimeString() + "','','Pending')";
            objCore.executeQuery(q);
            _frm1.takeawayid = idd.ToString();
            this.Close();
            _frm1.Enabled = true;
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
          //  _frm1.Islbldelivery = "Not Selected";
            _frm1.Enabled = true;
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
