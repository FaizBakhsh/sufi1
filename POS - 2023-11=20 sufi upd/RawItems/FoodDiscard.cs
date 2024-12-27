using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class FoodDiscard : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public FoodDiscard()
        {
            InitializeComponent();
        }
        public void getdata()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("Quantity", typeof(string));
            ds.Columns.Add("Discard", typeof(string));
            ds.Columns.Add("Remaining", typeof(string));

            string q = "";// "SELECT     dbo.RawItem.id,dbo.RawItem.ItemName, dbo.InventoryConsumed.RemainingQuantity FROM         dbo.RawItem INNER JOIN                      dbo.InventoryConsumed ON dbo.RawItem.Id = dbo.InventoryConsumed.RawItemId where dbo.InventoryConsumed.date='" + dateTimePicker1.Text + "'";
            q = "select id,itemname from rawitem";            
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                string val = "";
                double rem = 0;
                DataSet dspurchase = new DataSet();
                q = "SELECT     SUM(dbo.PurchaseDetails.TotalItems) AS Expr1 FROM         dbo.Purchase INNER JOIN                      dbo.PurchaseDetails ON dbo.Purchase.Id = dbo.PurchaseDetails.PurchaseId where dbo.Purchase.date <='" + date + "' and dbo.PurchaseDetails.RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    purchased = Convert.ToDouble(val);
                }
                dspurchase = new DataSet();
                q = "SELECT     SUM(QuantityConsumed) AS Expr1 FROM     InventoryConsumed where Date <='" + date + "' and RawItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    consumed = Convert.ToDouble(val);
                }
                val = "";
                double rate = 0;
                DataSet dscon = new DataSet();
                q = "SELECT     dbo.RawItem.ItemName, dbo.UOM.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                dscon = objcore.funGetDataSet(q);
                if (dscon.Tables[0].Rows.Count > 0)
                {
                    rate = Convert.ToDouble(dscon.Tables[0].Rows[0]["ConversionRate"].ToString());
                }
                consumed = consumed / rate;
                double qty = 0;
                qty = purchased - consumed;
                dspurchase = new DataSet();
                q = "SELECT     SUM(variance) AS Expr1 FROM     Variance where Date <='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    variance = Convert.ToDouble(val);
                }
                //if (variance > 0)
                {
                    qty = qty + (variance);
                }
                dspurchase = new DataSet();
                q = "SELECT     SUM(Discard) AS Expr1 FROM     Discard where Date <='" + date + "' and itemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    val = dspurchase.Tables[0].Rows[0][0].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    discard = Convert.ToDouble(val);
                }
                qty = qty - (discard);
                qty = Math.Round(qty, 2);
                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), qty.ToString(), "", "");
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;
            dataGridView1.Columns[4].ReadOnly = true;
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                try
                {
                    string val = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
                    if (val == "")
                    {
                        val = "0";

                    }
                    double physicalval = Convert.ToDouble(val);
                    val = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    if (val == "")
                    {
                        val = "0";

                    }
                    double remaining = Convert.ToDouble(val);
                    dataGridView1.Rows[e.RowIndex].Cells[4].Value = (remaining - physicalval).ToString();
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
           
        }
        public void updateremaininginventory(string itemid,double quantity)        
        {
            DataSet dsitems = new DataSet();
            string q = "select * from InventoryConsumed where RawItemId='"+itemid+"' and date='"+dateTimePicker1.Text+"'";
            dsitems = objcore.funGetDataSet(q);
            if (dsitems.Tables[0].Rows.Count > 0)
            {
                string val = dsitems.Tables[0].Rows[0]["QuantityConsumed"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                double consumed = Convert.ToDouble(val);
                val = dsitems.Tables[0].Rows[0]["RemainingQuantity"].ToString();
                if (val == "")
                {
                    val = "0";
                }
                double remaining = Convert.ToDouble(val);
                if (chk1 == false)
                {
                    consumed = consumed + (quantity);
                }
                remaining = remaining - (quantity);
                q = "update InventoryConsumed set RemainingQuantity='" + remaining + "' where id='" + dsitems.Tables[0].Rows[0]["id"].ToString() + "'";
                objcore.executeQuery(q);
                q = "update Inventory set Quantity='" + remaining + "' where RawItemId='" + itemid + "'";
                objcore.executeQuery(q);
            }
            else
            {
                DataSet dss = new DataSet();
                int id = 0;
                dss = objcore.funGetDataSet("select max(id) as id from InventoryConsumed");
                if (dss.Tables[0].Rows.Count > 0)
                {
                    string i = dss.Tables[0].Rows[0][0].ToString();
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
                dsitems = new DataSet();
                q = "select * from InventoryConsumed where RawItemId='" + itemid + "' order by id desc";
                dsitems = objcore.funGetDataSet(q);
                if (dsitems.Tables[0].Rows.Count > 0)
                {
                    string val = dsitems.Tables[0].Rows[0]["QuantityConsumed"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double consumed = Convert.ToDouble(val);
                    val = dsitems.Tables[0].Rows[0]["RemainingQuantity"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    double remaining = Convert.ToDouble(val);

                    consumed = consumed + (quantity);
                    remaining = remaining - (quantity);
                    //q = "update InventoryConsumed set QuantityConsumed='" + consumed + "',RemainingQuantity='" + remaining + "' where id='" + dsitems.Tables[0].Rows[0]["id"].ToString() + "'";
                    //objcore.executeQuery(q);
                    string q1 = "insert into InventoryConsumed  (id,RawItemId,QuantityConsumed,RemainingQuantity,date) values('" + id + "','" + itemid + "','0','" + remaining + "','" + dateTimePicker1.Text + "')";
                    objcore.executeQuery(q1);
                    q = "update Inventory set Quantity='" + remaining + "' where  RawItemId='" + itemid + "'";
                    objcore.executeQuery(q);
                }
                else
                {
                    dss = new DataSet();
                    int idd = 0;
                    dss = objcore.funGetDataSet("select max(id) as id from Inventory");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string i = dss.Tables[0].Rows[0][0].ToString();
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
                    string q1 = "insert into InventoryConsumed  (id,RawItemId,QuantityConsumed,RemainingQuantity,date) values('" + id + "','" + itemid + "','0','" + Math.Abs(quantity) + "','" + dateTimePicker1.Text + "')";
                    objcore.executeQuery(q1);
                    q1 = "insert into Inventory(id,RawItemId,Quantity) values('" + idd + "','" + itemid + "','" + Math.Abs(quantity) + "')";
                    objcore.executeQuery(q1);

                }
            }
        }
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
            bool chk = false;            
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                chk1 = false;
                if (dr.Cells[3].Value.ToString() != string.Empty && dr.Cells[3].Value.ToString() != "0")
                {
                    DataSet dss = new DataSet();
                    int id = 0;
                    dss = objcore.funGetDataSet("select max(id) as id from Discard");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string i = dss.Tables[0].Rows[0][0].ToString();
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
                    double variance = Math.Round(Convert.ToDouble(dr.Cells["Remaining"].Value.ToString()), 2);
                    //if (variance > 0)
                    //{
                    //    variance = -Math.Abs(variance);
                    //}
                    //else
                    //{
                    //    variance = Math.Abs(variance);
                    //}
                    dss = new DataSet();
                    string q = "select * from Discard where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        DialogResult dtr = MessageBox.Show("Discard Record of " + dr.Cells["ItemName"].Value.ToString() + " for this date already exist. Do you want to update it", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dtr == DialogResult.Yes)
                        {
                            double oldqty = Convert.ToDouble(dr.Cells["Quantity"].Value.ToString());
                            variance = oldqty - (Convert.ToDouble(dr.Cells["Discard"].Value.ToString()));
                            q = "update Discard set quantity='" + Math.Round(oldqty, 2).ToString() + "',Discard='" + Math.Round(Convert.ToDouble(dr.Cells["Discard"].Value.ToString()), 2).ToString() + "',Remaining ='" + variance.ToString() + "' where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'";
                            objcore.executeQuery(q);
                            chk1 = true;
                        }                        
                    }
                    else
                    {
                        q = "insert into Discard (id,itemid,date,quantity,Discard,Remaining) values('" + id + "','" + dr.Cells["id"].Value.ToString() + "','" + dateTimePicker1.Text + "','" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 2).ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Discard"].Value.ToString()), 2).ToString() + "','" + variance.ToString() + "')";
                        objcore.executeQuery(q);
                        chk = true;
                    }
                   // updateremaininginventory(dr.Cells["id"].Value.ToString(),variance);
                    
                }
            }
            if (chk == true)
            {
                MessageBox.Show("Record Saved Successfully");
                getdata();
            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FoodDiscard_Load(object sender, EventArgs e)
        {

        }
    }
}
