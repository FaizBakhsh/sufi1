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
    public partial class Production : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public Production()
        {
            InitializeComponent();
        }
        public void getdata()
        {
            string date = dateTimePicker1.Text;
            double quantity = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("Name", typeof(string));
            ds.Columns.Add("UOM", typeof(string));
            ds.Columns.Add("Quantity", typeof(string));
            ds.Columns.Add("Status", typeof(string));
            string q = "";
            if (textBox1.Text == "")
            {
                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id order by itemname ";
            }
            else
            {
                q = "select id,itemname,price,minorder,maxorder from rawitem where itemname like '%" + textBox1.Text + "%' order by itemname ";
                q = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.MinOrder, dbo.RawItem.maxorder, dbo.UOM.UOM FROM            dbo.RawItem INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id where dbo.RawItem.itemname like '%" + textBox1.Text + "%' order by itemname ";
         
            }
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                q = "select id from RecipeProduction where ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                DataSet dsrecipe = new DataSet();
                dsrecipe = objcore.funGetDataSet(q);
                if (dsrecipe.Tables[0].Rows.Count > 0)
                {
                    quantity = 0;
                    string val = "", status = "";

                    DataSet dspurchase = new DataSet();
                    string remarks = "";
                    dspurchase = new DataSet();
                   
                    q = "SELECT    Id, Date, ItemId, Quantity, Userid, status FROM            Production  where Date ='" + date + "' and ItemId='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and branchid='"+comboBox1.SelectedValue+"'";
                    dspurchase = objcore.funGetDataSet(q);
                    if (dspurchase.Tables[0].Rows.Count > 0)
                    {
                        status = dspurchase.Tables[0].Rows[0]["status"].ToString();
                        val = dspurchase.Tables[0].Rows[0]["Quantity"].ToString();
                        if (val == "")
                        {
                            val = "0";
                        }
                        quantity = Convert.ToDouble(val);


                    }

                    ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["Itemname"].ToString(), ds1.Tables[0].Rows[i]["UOM"].ToString(), quantity, status);
                }
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].ReadOnly = true;
            
            
        }

     
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

           
        }
        
        bool chk1 = false;
        private void vButton2_Click(object sender, EventArgs e)
        {
           
            bool chk = false;            
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                chk1 = false;
                
                {
                    DataSet dss = new DataSet();
                    
                    dss = new DataSet();
                    string q = "select * from Production where itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and branchid='"+comboBox1.SelectedValue+"'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        if (dss.Tables[0].Rows[0]["Status"].ToString() != "Posted")
                        {
                            q = "update Production set  branchid='" + comboBox1.SelectedValue + "', date='" + dateTimePicker1.Text + "',ItemId='" + dr.Cells["id"].Value.ToString() + "',Quantity='" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 3).ToString() + "',Userid='" + POSRestaurant.Properties.Settings.Default.UserId.ToString() + "',Status='Pending' where  itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "'  and branchid='" + comboBox1.SelectedValue + "' ";

                            // q = "update DemandSheet set userid='"+POSRestaurant.Properties.Settings.Default.UserId.ToString()+"', Quantity='" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 2).ToString() + "'  where   itemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' ";
                            int res = objcore.executeQueryint(q);
                            if (res > 0)
                            {
                                chk1 = true;
                            }
                        }
                    }
                    else
                    {
                        // q = "insert into DemandSheet (Date, ItemId, Quantity, Userid) values('" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 2).ToString() + "','"+POSRestaurant.Properties.Settings.Default.UserId.ToString()+"')";
                        q = "insert into Production(date,ItemId,Quantity,Userid,Status,branchid) values('" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 3).ToString() + "','" + POSRestaurant.Properties.Settings.Default.UserId.ToString() + "','Pending','" + comboBox1.SelectedValue + "')";
                        int res = objcore.executeQueryint(q);
                        if (res > 0)
                        {
                            chk = true;
                        }
                    }
                    //updateremaininginventory(dr.Cells["id"].Value.ToString(),variance);
                    
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
      
        private void Variance_Load(object sender, EventArgs e)
        {
            string q = "select id,branchname from branch where status='Active' ";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            try
            {

                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "branchname";


            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            
        }
        DataTable dt;
       
        private void vButton5_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton4_Click_1(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Data cant be reversed after Posting", "Do you Want to Proceed?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                return;
            }

            try
            {
                foreach (DataGridViewRow dgr in dataGridView1.Rows)
                {
                    if (dgr.Cells["Status"].Value.ToString() != "Posted")
                    {
                        string date = dateTimePicker1.Text;// dgr.Cells["date"].Value.ToString();
                        string itemid = dgr.Cells["Id"].Value.ToString();
                        string temp = dgr.Cells["Quantity"].Value.ToString();
                        if (temp == "")
                        {
                            temp = "0";
                        }
                        float quantity = float.Parse(temp);
                        
                        string q = "";
                        float rate = 1;
                        
                       
                        if (Convert.ToDouble(quantity) > 0)
                        {
                            attachrecipie(itemid, quantity);
                            q = "select * from RecipeProduction where itemid='" + itemid + "'";
                            DataSet ds = new DataSet();
                            ds = objcore.funGetDataSet(q);
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                string rawitmid = ds.Tables[0].Rows[i]["RawItemId"].ToString();
                                //try
                                //{
                                //    DataSet dscon1 = new DataSet();
                                //    q = "SELECT     dbo.RawItem.ItemName, dbo.UOMConversion.UOM, dbo.UOMConversion.ConversionRate FROM         dbo.RawItem INNER JOIN                       dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.RawItem.Id='" + rawitmid + "'";
                                //    dscon1 = objcore.funGetDataSet(q);
                                //    if (dscon1.Tables[0].Rows.Count > 0)
                                //    {
                                //        rate = float.Parse(dscon1.Tables[0].Rows[0]["ConversionRate"].ToString());
                                //    }
                                //}
                                //catch (Exception ex)
                                //{


                                //}

                                float recipeqty = float.Parse(ds.Tables[0].Rows[i]["quantity"].ToString());
                                if (rate > 1)
                                {
                                  //  recipeqty = recipeqty / rate;
                                }
                                float qty = quantity;
                                double finalqty = qty * recipeqty;
                                finalqty = Math.Round(finalqty, 3);

                                DataSet dsminus = new DataSet();
                                dsminus = objcore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + date + "'  and branchid='" + comboBox1.SelectedValue + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());

                                    q = "update InventoryConsumed set  QuantityConsumed='" + (inventryconsumedqty + finalqty) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "' ";
                                    objcore.executeQuery(q);
                                }
                                else
                                {
                                    DataSet dsc = new DataSet();
                                    int idcnsmd = 0;
                                    dsc = objcore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (dsc.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = dsc.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }

                                    q = "insert into InventoryConsumed (id,branchid,RawItemId,QuantityConsumed,Date) values('" + idcnsmd + "','" + comboBox1.SelectedValue + "','" + rawitmid + "','" + finalqty + "','" + date + "')";
                                    objcore.executeQuery(q);
                                }
                            }
                            q = "update Production set status='Posted' where itemid='" + itemid + "' and date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "'";
                            objcore.executeQuery(q);
                        }
                        
                    }
                }
                getdata();
            }
            catch (Exception ex)
            {

            }
        }
        public void attachrecipie(string itmid, float itmqnty)
        {
            DataSet dsrecipie = new DataSet();
            DataSet dsminus = new DataSet();
            try
            {

                string q = "";
                q = "";

                q = "SELECT        dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.AttachRecipe.Quantity  AS attachQty, dbo.SubRecipe.type, dbo.SubItems.Name, dbo.AttachRecipe.Menuitemid, dbo.SubRecipe.RawItemId,    dbo.SubRecipe.Quantity FROM            dbo.UOM INNER JOIN                         dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId INNER JOIN                         dbo.AttachRecipe INNER JOIN                         dbo.SubItems INNER JOIN                         dbo.SubRecipe ON dbo.SubItems.Id = dbo.SubRecipe.ItemId ON dbo.AttachRecipe.SubItemId = dbo.SubItems.Id ON dbo.RawItem.Id = dbo.SubRecipe.RawItemId where dbo.AttachRecipe.type='Production' and  dbo.AttachRecipe.Menuitemid='" + itmid + "' ";
                dsrecipie = objcore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        bool chk = true;
                        {
                            try
                            {
                                string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                                float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                                double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                                double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                                double recipiattachqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["attachQty"].ToString());
                                double amounttodeduct = recipiqnty * qnty;// (qnty / convrate) * recipiqnty;//recipiqnty
                                amounttodeduct = amounttodeduct * itmqnty;

                                amounttodeduct = amounttodeduct * recipiattachqnty;
                                amounttodeduct = Math.Round(amounttodeduct, 3);


                                dsminus = new DataSet();
                                double inventryqty = 0;
                                dsminus = objcore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");

                                dsminus = new DataSet();
                                dsminus = objcore.funGetDataSet("select * from InventoryConsumed where RawItemId='" + rawitmid + "' and Date='" + dateTimePicker1.Text + "' and branchid='" + comboBox1.SelectedValue + "'");
                                if (dsminus.Tables[0].Rows.Count > 0)
                                {
                                    double inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["QuantityConsumed"].ToString());
                                    double rem = double.Parse(dsminus.Tables[0].Rows[0]["RemainingQuantity"].ToString());
                                    if (inventryqty > 0)
                                    {
                                        rem = inventryqty;
                                    }
                                    q = "update InventoryConsumed set uploadstatus='Pending',RemainingQuantity='" + (rem - amounttodeduct) + "', QuantityConsumed='" + (inventryconsumedqty + amounttodeduct) + "' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                                    objcore.executeQuery(q);
                                }
                                else
                                {
                                    DataSet ds = new DataSet();
                                    int idcnsmd = 0;
                                    ds = objcore.funGetDataSet("select MAX(ID) as id from InventoryConsumed");
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        string ii = ds.Tables[0].Rows[0][0].ToString();
                                        if (ii == string.Empty)
                                        {
                                            ii = "0";
                                        }
                                        idcnsmd = Convert.ToInt32(ii) + 1;
                                    }
                                    else
                                    {
                                        idcnsmd = Convert.ToInt32("1");
                                    }
                                    //idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                                    q = "insert into InventoryConsumed (id,branchid,RawItemId,QuantityConsumed,RemainingQuantity,Date) values('" + idcnsmd + "','" + comboBox1.SelectedValue + "','" + rawitmid + "','" + amounttodeduct + "','" + (inventryqty - amounttodeduct) + "','" + dateTimePicker1.Text + "')";
                                    objcore.executeQuery(q);
                                }
                            }
                            catch (Exception exx)
                            {


                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
               
                dsrecipie.Dispose();
                dsminus.Dispose();
            }
        }
        private void vButton5_Click_1(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmDemandSheet obj = new Reports.Inventory.frmDemandSheet();
            obj.date = dateTimePicker1.Text;
            obj.branchid = comboBox1.SelectedValue.ToString();
            obj.Show();
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if (dr.Cells["Status"].Value.ToString() == "Posted")
                    {
                        dr.DefaultCellStyle.BackColor = Color.IndianRed;
                        dr.ReadOnly = true;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
