using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class BillRecall : Form
    {
        DataSet ds = new DataSet();
        public BillRecall()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata("search");
        }
        public void getdata(string type)
        {
            try
            {
                ds = new DataSet();
                //dataGridView2.DataSource = ds.Tables[0];
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                if (txtsearch.Text != "")
                {
                    //q9 = "SELECT    Id as Bill_No, Date, time, UserId, TotalBill , DiscountAmount,  NetBill, BillType,  GST, BillStatus from sale where id='" + txtsearch.Text.Trim() + "'";
                    q9 = "SELECT        dbo.Sale.Id AS Bill_No, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.DiscountAmount AS Discount, dbo.Sale.OrderType, dbo.DinInTables.TableNo,dbo.DinInTables.Guests, dbo.ResturantStaff.Name AS Waiter_Name, dbo.Sale.Customer FROM            dbo.Sale LEFT OUTER JOIN                         dbo.ResturantStaff INNER JOIN                         dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId ON dbo.Sale.Id = dbo.DinInTables.Saleid where dbo.Sale.id='" + txtsearch.Text.Trim() + "' and (dbo.Sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid') and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "' order by  dbo.Sale.id asc";
               
                }
                else
                {
                   // q9 = "SELECT    Id as Bill_No, Date, time, UserId, TotalBill , DiscountAmount,  NetBill, BillType,  GST, BillStatus from sale order by id";
                    q9 = "SELECT        dbo.Sale.Id AS Bill_No, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.DiscountAmount AS Discount, dbo.Sale.OrderType, dbo.DinInTables.TableNo,dbo.DinInTables.Guests, dbo.ResturantStaff.Name AS Waiter_Name, dbo.Sale.Customer FROM            dbo.Sale LEFT OUTER JOIN                         dbo.ResturantStaff INNER JOIN                         dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId ON dbo.Sale.Id = dbo.DinInTables.Saleid where  (dbo.Sale.date between '" + dateTimePicker1.Text + "' and  '" + dateTimePicker2.Text + "') AND (dbo.Sale.BillStatus = 'Paid')  and dbo.Sale.branchid='" + cmbbranch.SelectedValue + "'  order by  dbo.Sale.id asc";
               
                }
                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
                //dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].Visible = false;
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
            //q = "SELECT     dbo.Saledetails.Id, dbo.Saledetails.saleid, dbo.RawItem.ItemName, dbo.Saledetails.Quantity, dbo.Saledetails.Price,dbo.Saledetails.TotalPrice FROM         dbo.Saledetails INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc";
            q = "SELECT        dbo.Saledetails.Id, dbo.Saledetails.saleid, dbo.ModifierFlavour.name AS Size, dbo.MenuItem.Name AS MenuItem, dbo.RuntimeModifier.name AS RModifier, dbo.Modifier.Name AS Modifier, dbo.Saledetails.Quantity,                          dbo.Saledetails.Price FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.Saledetails.Flavourid = dbo.ModifierFlavour.Id LEFT OUTER JOIN                         dbo.Modifier ON dbo.Saledetails.ModifierId = dbo.Modifier.Id LEFT OUTER JOIN                         dbo.RuntimeModifier ON dbo.Saledetails.RunTimeModifierId = dbo.RuntimeModifier.id where dbo.Saledetails.saleid='" + id + "' ORDER BY dbo.Saledetails.Id asc";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                dataGridView2.DataSource = ds.Tables[0];
                dataGridView2.Columns[0].Visible = false;
                dataGridView2.Columns[1].Visible = false;
                // dataGridView2.Columns[2].Visible = false;
                // dataGridView2.Columns[7].Visible = false;
                //foreach (DataGridViewRow dr in dataGridView2.Rows)
                //{

                //    try
                //    {
                //        dr.Height = 40;

                //    }
                //    catch (Exception ex)
                //    {


                //    }
                //}
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //try
            //{
            //    string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            //    filg2(Id);
            //}
            //catch (Exception ex)
            //{


            //}
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            getdata("all");
        }

        private void BillRecall_Load(object sender, EventArgs e)
        {
           
            try
            {
                DataSet ds1 = new DataSet();
                string q = "select id,branchname from branch ";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["branchname"] = "All";
              
                cmbbranch.DataSource = ds1.Tables[0];
                cmbbranch.ValueMember = "id";
                cmbbranch.DisplayMember = "branchname";

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            getdata("all");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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
