using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.forms
{
    public partial class RecallBills : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public RecallBills()
        {
            InitializeComponent();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {

        }
        public void getdata()
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                
                {
                    q9 = "SELECT    Id as Bill_No, Date, time, UserId, TotalBill , DiscountAmount,  NetBill, BillType,  GST, BillStatus from sale where id='" + txtsearch.Text.Trim() + "'";

                }
                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
                //dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[3].Visible = false;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 40;
                }

            }
            catch (Exception ex)
            {


            }
        }
        public void filg2(string id)
        {

            
            string q = "";// "SELECT      dbo.Saledetails.Id, dbo.Saledetails.saleid,dbo.Saledetails.ModifierId, dbo.MenuItem.Name as Item, dbo.Modifier.Name AS Modifier, dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM         dbo.Saledetails LEFT OUTER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                      dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc ";

            // q = "SELECT     dbo.Saledetails.Id, dbo.Saledetails.saleid,dbo.Saledetails.ModifierId, dbo.MenuItem.Name as Item , dbo.RawItem.ItemName AS Modifier , dbo.Saledetails.Quantity, dbo.Saledetails.Price FROM         dbo.RawItem INNER JOIN                      dbo.Modifier ON dbo.RawItem.Id = dbo.Modifier.RawItemId RIGHT OUTER JOIN                      dbo.Saledetails LEFT OUTER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id ON dbo.Modifier.Id = dbo.Saledetails.ModifierId where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc ";
            q = "SELECT     dbo.Saledetails.Id, dbo.Saledetails.saleid, dbo.RawItem.ItemName, dbo.Saledetails.Quantity, dbo.Saledetails.Price,dbo.Saledetails.TotalPrice FROM         dbo.Saledetails INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataGridView2.DataSource = ds.Tables[0];
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                // dataGridView2.Columns[2].Visible = false;
                // dataGridView2.Columns[7].Visible = false;
                foreach (DataGridViewRow dr in dataGridView2.Rows)
                {

                    try
                    {
                        dr.Height = 40;
                        
                    }
                    catch (Exception ex)
                    {


                    }
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                filg2(Id);
            }
            catch (Exception ex)
            {


            }
        }
    }
}
