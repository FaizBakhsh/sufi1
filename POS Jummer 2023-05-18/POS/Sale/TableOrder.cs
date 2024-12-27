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
    public partial class TableOrder : Form
    {
        private  RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore ;
        DataSet ds ;
        public TableOrder(RestSale frm1)
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
            objCore = new classes.Clsdbcon();
            ds = new DataSet();
            string q = "select * from DinInTables where status='Pending'";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    string tableno = ds.Tables[0].Rows[i]["TableNo"].ToString();
                    if (tableno == "1")
                    {
                        vButton1.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label1.BackColor = Color.Green;
                    }
                    if (tableno == "2")
                    {
                        vButton2.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label2.BackColor = Color.Green;
                    }
                    if (tableno == "3")
                    {
                        vButton3.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label3.BackColor = Color.Green;
                    }
                    if (tableno == "4")
                    {
                        vButton4.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label4.BackColor = Color.Green;
                    }
                    if (tableno == "5")
                    {
                        vButton5.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label5.BackColor = Color.Green;
                    }
                    if (tableno == "6")
                    {
                        vButton6.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label6.BackColor = Color.Green;
                    }
                    if (tableno == "7")
                    {
                        vButton7.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label7.BackColor = Color.Green;
                    }
                    if (tableno == "8")
                    {
                        vButton8.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label8.BackColor = Color.Green;
                    }
                    if (tableno == "9")
                    {
                        vButton9.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label9.BackColor = Color.Green;
                    }
                    if (tableno == "10")
                    {
                        vButton10.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label10.BackColor = Color.Green;
                    }
                    if (tableno == "11")
                    {
                        vButton11.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label11.BackColor = Color.Green;
                    }
                    if (tableno == "12")
                    {
                        vButton12.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label12.BackColor = Color.Green;
                    }
                    if (tableno == "13")
                    {
                        vButton13.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label13.BackColor = Color.Green;
                    }
                    if (tableno == "14")
                    {
                        vButton14.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label14.BackColor = Color.Green;
                    }
                    if (tableno == "15")
                    {
                        vButton15.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label15.BackColor = Color.Green;
                    }
                    
                    if (tableno == "16")
                    {
                        vButton16.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label16.BackColor = Color.Green;
                    }
                    if (tableno == "17")
                    {
                        vButton17.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label17.BackColor = Color.Green;
                    }
                    if (tableno == "18")
                    {
                        vButton18.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label18.BackColor = Color.Green;
                    }
                    if (tableno == "19")
                    {
                        vButton19.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label19.BackColor = Color.Green;
                    }
                    if (tableno == "20")
                    {
                        vButton20.Image = ((System.Drawing.Image)(Properties.Resources.tablefill));
                        label20.BackColor = Color.Green;
                    }
                }
            }
            
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
            objCore = new classes.Clsdbcon();
            ds = new DataSet();
            int id = 0;
            ds = new DataSet();
            ds = objCore.funGetDataSet("select max(id) as id from TakeAway");
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

           // string q = "insert into TakeAway (id,CustomerId,Date,time,Saleid,Status) values ('" + id + "','" + txtname.Text.Trim() + "','" + DateTime.Now.ToShortDateString() + "','" + DateTime.Now.ToShortTimeString() + "','','Pending')";
          //  objCore.executeQuery(q);
            _frm1.takeawayid = id.ToString();
            this.Close();
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            //_frm1.Islbldelivery = "Not Selected";
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void getsaleid(string tbno)
        {
            DataSet dsgetsalid = new DataSet();
            dsgetsalid = objCore.funGetDataSet("select * from DinInTables where Tableno='"+tbno+"' and status='Pending'");
            if (dsgetsalid.Tables[0].Rows.Count > 0)
            {
                _frm1.recalsale(dsgetsalid.Tables[0].Rows[0]["saleid"].ToString(),"no");
                //_frm1.Enabled = true;
            }
        }
        private void vButton1_Click_1(object sender, EventArgs e)
        {
            _frm1.tableno = "1";
            if (label1.BackColor == Color.Green)
            {
                getsaleid("1");
                   
            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton2_Click_1(object sender, EventArgs e)
        {
            
            if (label2.BackColor == Color.Green)
            {
                getsaleid("2");

            }
            _frm1.Enabled = true;
            _frm1.tableno = "2";
            this.Close();
        }

        private void vButton3_Click_1(object sender, EventArgs e)
        {
            _frm1.tableno = "3";
            if (label3.BackColor == Color.Green)
            {
                getsaleid("3");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton4_Click_1(object sender, EventArgs e)
        {
            _frm1.tableno = "4";
            if (label4.BackColor == Color.Green)
            {
                getsaleid("4");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "5";
            if (label5.BackColor == Color.Green)
            {
                getsaleid("5");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "6";
            if (label6.BackColor == Color.Green)
            {
                getsaleid("6");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "7";
            if (label7.BackColor == Color.Green)
            {
                getsaleid("7");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "8";
            if (label8.BackColor == Color.Green)
            {
                getsaleid("8");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton9_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "9";
            if (label9.BackColor == Color.Green)
            {
                getsaleid("9");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton10_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "10";
            if (label10.BackColor == Color.Green)
            {
                getsaleid("10");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton11_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "11";
            if (label11.BackColor == Color.Green)
            {
                getsaleid("11");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton12_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "12";
            if (label12.BackColor == Color.Green)
            {
                getsaleid("12");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton13_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "13";
            if (label13.BackColor == Color.Green)
            {
                getsaleid("13");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton14_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "14";
            if (label14.BackColor == Color.Green)
            {
                getsaleid("14");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "15";
            if (label15.BackColor == Color.Green)
            {
                getsaleid("15");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton16_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "16";
            if (label16.BackColor == Color.Green)
            {
                getsaleid("16");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton17_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "17";
            if (label17.BackColor == Color.Green)
            {
                getsaleid("17");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton18_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "18";
            if (label18.BackColor == Color.Green)
            {
                getsaleid("18");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton19_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "19";
            if (label19.BackColor == Color.Green)
            {
                getsaleid("19");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton20_Click(object sender, EventArgs e)
        {
            _frm1.tableno = "20";
            if (label20.BackColor == Color.Green)
            {
                getsaleid("20");

            }
            _frm1.Enabled = true;
            this.Close();
        }

        private void vButton21_Click(object sender, EventArgs e)
        {
            _frm1.Enabled = true;
            this.Close();
        }
    }
}
