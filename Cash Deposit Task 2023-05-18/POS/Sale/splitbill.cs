using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;

namespace POSRestaurant.Sale
{
    public partial class splitbill : Form
    {
        RestSale _frm;
        public splitbill(RestSale frm)
        {
            _frm = frm;
            InitializeComponent();
        }
        DataTable dtsplit = new DataTable();
        private void splitbill_Load(object sender, EventArgs e)
        {
            this.TopMost = false;
            getbills();
            dtsplit.Columns.Add("Id", typeof(string));
            dtsplit.Columns.Add("Name", typeof(string));
            dtsplit.Columns.Add("Quantity", typeof(string));
            dtsplit.Columns.Add("Amount", typeof(string));
            dataGridView2.DataSource = dtsplit;
        }
        public string date = "";
        protected void getbills()
        {
            string q = "select * from sale where date='" + date + "' and billstatus='Pending' order by id";
            DataSet dsbill = new DataSet();
            dsbill = objCore.funGetDataSet(q);
            if (dsbill.Tables[0].Rows.Count > 0)
            {
                int count = 12;
                if (dsbill.Tables[0].Rows.Count > 8)
                {
                    count = dsbill.Tables[0].Rows.Count;
                }
                AddDisplayControls(count);
            }
            for (int i = 0; i < dsbill.Tables[0].Rows.Count; i++)
            {
                vButton button1 = new vButton();
                button1.Name = dsbill.Tables[0].Rows[i]["id"].ToString();
                button1.Text = dsbill.Tables[0].Rows[i]["customer"].ToString();
                button1.Font = new Font("", Convert.ToInt32(20), FontStyle.Bold);
                button1.VIBlendTheme = VIBLEND_THEME.ULTRABLUE;
                button1.Click += new EventHandler(button_Click);
                Addbutton(button1);
            }
            dsbill.Dispose();
        }
        protected void fillgrid(string id)
        {
            dtsplit.Rows.Clear();
            dtsplit.AcceptChanges();
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Quantity", typeof(string));
            dt.Columns.Add("Amount", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            try
            {
                string q = "SELECT     dbo.Saledetails.id,   dbo.MenuItem.Name, dbo.ModifierFlavour.name AS flavour, dbo.Modifier.Name AS Modifier, dbo.RuntimeModifier.name AS Rmodifier, dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id where dbo.saledetails.saleid=" + id;
                DataSet dsdetails = new DataSet();
                dsdetails = objCore.funGetDataSet(q);
                for (int i = 0; i < dsdetails.Tables[0].Rows.Count; i++)
                {
                    string name = dsdetails.Tables[0].Rows[i]["Name"].ToString();
                    if (dsdetails.Tables[0].Rows[i]["flavour"].ToString().Trim() != "")
                    {
                        name = dsdetails.Tables[0].Rows[i]["flavour"].ToString().Trim() + " " + name;
                    }
                    if (dsdetails.Tables[0].Rows[i]["Rmodifier"].ToString().Trim() != "")
                    {
                        name = dsdetails.Tables[0].Rows[i]["Rmodifier"].ToString();
                    }
                    if (dsdetails.Tables[0].Rows[i]["Modifier"].ToString().Trim() != "")
                    {
                        name = dsdetails.Tables[0].Rows[i]["Modifier"].ToString();
                    }
                    string quantity = dsdetails.Tables[0].Rows[i]["Quantity"].ToString();
                    string Price = dsdetails.Tables[0].Rows[i]["Price"].ToString();
                    dt.Rows.Add(dsdetails.Tables[0].Rows[i]["id"].ToString(), name, quantity, Price,"Pending");

                }
            }
            catch (Exception ex)
            {
                
                
            }
            dataGridView1.DataSource = dt;
        }
        protected void button_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            fillgrid(btn.Name);
            saleid = btn.Name;
        }
        int tcolms = 0;
        int trows = 0;
        private void Addbutton(Button btn)
        {
            //// panel7.SuspendLayout();
            try
            {
                btn.Dock = DockStyle.Fill;


                tblbills.Controls.Add(btn, tcolms, trows);
                tcolms = 0;
                trows++;
            }
            catch (Exception ex)
            {


            }
            // panel7.ResumeLayout(false);
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void AddDisplayControls(int rows)
        {
            try
            {
                tblbills.Controls.Clear();
                //Clear out the existing row and column styles
                tblbills.ColumnStyles.Clear();
                tblbills.RowStyles.Clear();
                tblbills.ColumnCount = 1;
                tblbills.RowCount = rows;
              
                //Assign table no of rows and column            
                float cperc = 100 / tblbills.ColumnCount;
                float rperc = 100 / tblbills.RowCount;
                //tblbills.Height = Convert.ToInt32(rowsize * tblbills.RowCount);
                for (int i = 0; i < tblbills.ColumnCount; i++)
                {
                    tblbills.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));
                    for (int j = 0; j < tblbills.RowCount; j++)
                    {
                        if (i == 0)
                        {
                            //defining the size of cell
                            tblbills.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
                        }
                    }
                }
                tblbills.HorizontalScroll.Enabled = false;
                
            }
            catch (Exception ex)
            {

            }
            finally
            {
               
            }
        }

        private void btntablet_Click(object sender, EventArgs e)
        {
            _frm.callneworder();
            this.Close();
        }
        public static string saledetailid = "";
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            saledetailid = "";
            string status = "";
            string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
            status = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value.ToString();

            if (status == "Pending")
            {
                saledetailid = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
          
        }
        public string applydiscount()
        {
            string apply = "before";
            DataSet dsdis = new DataSet();
            try
            {
                string q = "select * from applydiscount ";

                dsdis = objCore.funGetDataSet(q);
                if (dsdis.Tables[0].Rows.Count > 0)
                {
                    apply = dsdis.Tables[0].Rows[0]["apply"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsdis.Dispose();
            }
            if (apply == "")
            {
                apply = "before";
            }
            return apply;
        }
        private void dataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
        public string saleid = "";
        protected void creatbill(string id)
        {
            string q = "";
            q = "select * from sale where id=" + id;
            DataSet dssale = new DataSet();
            dssale = objCore.funGetDataSet(q);

            double total = 0;
            foreach (DataGridViewRow dgr in dataGridView2.Rows)
            {
                string temp = "";
                temp = dgr.Cells["Amount"].Value.ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                total = total + Convert.ToDouble(temp);
            }


            double totalgst = 0; double discountedtotal = 0;
            try
            {
                double service = 0;
                double gst = Convert.ToDouble(dssale.Tables[0].Rows[0]["gstperc"].ToString());
                double dscount = Convert.ToDouble(dssale.Tables[0].Rows[0]["discount"].ToString());
                dscount = (dscount * total) / 100;
                dscount = Math.Round(dscount, 2);


                string ordertyppe = "";

                if (applydiscount() == "before")
                {
                    discountedtotal = total;// -dscount;
                    service = (0 * discountedtotal) / 100;
                    if (ordertyppe == "Take Away")
                    {
                        service = 0;
                    }
                    discountedtotal = discountedtotal + service;
                    totalgst = (gst * discountedtotal) / 100;
                    discountedtotal = discountedtotal - dscount;
                }
                else
                {
                    discountedtotal = total - dscount;
                    service = (0 * discountedtotal) / 100;
                    if (ordertyppe == "Take Away")
                    {
                        service = 0;
                    }
                    totalgst = (gst * discountedtotal) / 100;
                    discountedtotal = discountedtotal + service;
                }
                discountedtotal = discountedtotal;

            }

            catch (Exception ex)
            {


                throw;

            }


            totalgst = Math.Round(totalgst, 2);
            double newtotal = Math.Round(((totalgst + discountedtotal)), 2);

            if (dssale.Tables[0].Rows.Count > 0)
            {

                q = "insert into sale (branchid,discountid,date,time,UserId,TotalBill,Discount,NetBill,BillType,OrderType,GST,BillStatus,Terminal,GSTPerc,Shiftid,customer,servicecharges,TerminalOrder,invoice) values ('" + dssale.Tables[0].Rows[0]["branchid"].ToString() + "','" + dssale.Tables[0].Rows[0]["discountid"].ToString() + "','" + dssale.Tables[0].Rows[0]["date"].ToString() + "','" + dssale.Tables[0].Rows[0]["time"].ToString() + "','" + dssale.Tables[0].Rows[0]["UserId"].ToString() + "','" + total + "','" + dssale.Tables[0].Rows[0]["Discount"].ToString() + "','" + newtotal + "','" + dssale.Tables[0].Rows[0]["BillType"].ToString() + "','" + dssale.Tables[0].Rows[0]["OrderType"].ToString() + "','" + totalgst + "','" + dssale.Tables[0].Rows[0]["BillStatus"].ToString() + "','" + System.Environment.MachineName + "','" + dssale.Tables[0].Rows[0]["GSTPerc"].ToString() + "','" + dssale.Tables[0].Rows[0]["Shiftid"].ToString() + "','" + dssale.Tables[0].Rows[0]["customer"].ToString()+" I" + "','" + dssale.Tables[0].Rows[0]["servicecharges"].ToString() + "','" + System.Environment.MachineName + "','" + dssale.Tables[0].Rows[0]["invoice"].ToString() + "')";
                objCore.executeQuery(q);
                int sid = 0;
                DataSet dss = new DataSet();
                try
                {
                    string qry = "select max(id) as id from sale  where TerminalOrder='" + System.Environment.MachineName.ToString() + "'";
                    dss = objCore.funGetDataSet(qry);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string ii = dss.Tables[0].Rows[0][0].ToString();
                        if (ii == string.Empty)
                        {
                            ii = "0";
                        }
                        sid = Convert.ToInt32(ii);
                    }
                }
                catch (Exception ex)
                {


                }
                finally
                {
                    dss.Dispose();
                }

                foreach (DataGridViewRow dgr in dataGridView2.Rows)
                {
                    string idd = "";
                    idd = dgr.Cells["id"].Value.ToString();
                    q = "update saledetails set saleid='" + sid + "' where  id=" + idd + "";
                    objCore.executeQuery(q);
                    
                }
            }
        }
        private void dataGridView2_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (saledetailid != "")
                {
                    dtsplit.Rows.Add(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString(), dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value.ToString(), dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[2].Value.ToString(), dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[3].Value.ToString());
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[4].Value = "Splitted";
                    dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].ReadOnly = true;
                    saledetailid = "";
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            creatbill(saleid);
            saleid = "";
            fillgrid("");
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            splitbillItems obj = new splitbillItems(_frm);
            obj.date = date;
            obj.Show();
            this.Close();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            MergeBills obj = new MergeBills(_frm);
            obj.date = date;
            this.Close();
            obj.Show();
        }
    }
}
