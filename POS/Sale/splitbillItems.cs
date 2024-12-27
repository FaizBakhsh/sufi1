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
    public partial class splitbillItems : Form
    {
        RestSale _frm;
        public splitbillItems(RestSale frm)
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
            saleid = id;
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
            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = false;
            dataGridView1.Columns[3].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
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
           
            saleid = "";
            fillgrid("");
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                float qty = float.Parse(dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                if (qty > 0)
                {
                    string q = "select * from saledetails where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                    DataSet ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        float oldqty = float.Parse(ds.Tables[0].Rows[0]["quantity"].ToString());
                        float oldamount = float.Parse(ds.Tables[0].Rows[0]["Price"].ToString());
                        float oldgst = float.Parse(ds.Tables[0].Rows[0]["itemgst"].ToString());
                        float olddiscount = float.Parse(ds.Tables[0].Rows[0]["itemdiscount"].ToString());
                        if (qty >= oldqty)
                        {
                            MessageBox.Show("Quantity should not be greater then existing quantity");
                            return;
                        }
                        float newqty = oldqty - qty;
                        float singleprice =float.Parse( Math.Round(oldamount / oldqty).ToString());
                        double newadiscount = Math.Round(olddiscount * oldqty/100,2);
                        newadiscount = newadiscount * qty;
                        double prevdiscount = Math.Round(olddiscount * oldqty / 100, 2);
                        prevdiscount = prevdiscount * newqty;
                        double newgst = Math.Round(oldgst * oldqty / 100, 2);
                        newgst = newgst * qty;
                        double prevgst = Math.Round(oldgst * oldqty / 100);
                        prevgst = prevgst * newqty;
                        double newamount = Math.Round(singleprice * qty);
                        double oldprice = Math.Round(singleprice * newqty);

                        q = "insert into saledetails(saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, Quantity, Price, BarnchCode, Status, comments, Orderstatus, branchid, Itemdiscount, ItemdiscountPerc, ItemGst, ItemGstPerc, OrderStatusmain, atid, dealid, OnlineId, kdsgroup, time, kdsid, completedtime, pointscode, ParkStatus) SELECT   saleid, MenuItemId, Flavourid, ModifierId, RunTimeModifierId, " + qty + ", " + newamount + ", BarnchCode, Status, comments, Orderstatus, branchid, " + newadiscount + ", ItemdiscountPerc, " + newgst + ", ItemGstPerc, OrderStatusmain, atid, dealid, OnlineId, kdsgroup, time, kdsid, completedtime, pointscode, ParkStatus FROM            Saledetails where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                        q = "update saledetails set quantity='" + newqty + "',price='" + oldprice + "',Itemdiscount='" + prevdiscount + "',ItemGst='" + prevgst + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                        objCore.executeQuery(q);
                        fillgrid(saleid);
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

        }
    }
}
