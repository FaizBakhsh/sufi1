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
    public partial class StockEstimation : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public StockEstimation()
        {
            InitializeComponent();
        }
        public void getdata(string date, string invoice)
        {
            dateTimePicker1.Text = date;
            txtorderno.Text = invoice;
            
            getdata();
        }
        public void getdata()
        {
            string date = dateTimePicker1.Text;
            double purchased = 0, consumed = 0, variance = 0, price = 0, discard = 0,tin=0,tout=0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("SizeId", typeof(string));
            ds.Columns.Add("Size", typeof(string));
            ds.Columns.Add("ItemName", typeof(string));
            ds.Columns.Add("Quantity", typeof(string));
            ds.Columns.Add("Status", typeof(string));   
            string q = "";
            if (comboBox1.SelectedText == "All")
            {
                if (textBox1.Text == "")
                {
                    q = "SELECT        dbo.MenuItem.Id, dbo.ModifierFlavour.name AS Size,dbo.ModifierFlavour.id AS flid, dbo.MenuItem.Name FROM            dbo.MenuItem LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where dbo.MenuItem.status='Active' order by dbo.MenuItem.MenugroupId, dbo.MenuItem.name ";
                }
                else
                {
                    q = "SELECT        dbo.MenuItem.Id, dbo.ModifierFlavour.name AS Size, dbo.ModifierFlavour.id AS flid,dbo.MenuItem.Name FROM            dbo.MenuItem LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where  dbo.MenuItem.status='Active' and dbo.MenuItem.name like '%" + textBox1.Text + "%' order by dbo.MenuItem.MenugroupId, dbo.MenuItem.name ";
                }
            }
            else
            {
                if (textBox1.Text == "")
                {
                    q = "SELECT        dbo.MenuItem.Id, dbo.ModifierFlavour.name AS Size,dbo.ModifierFlavour.id AS flid, dbo.MenuItem.Name FROM            dbo.MenuItem LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where  dbo.MenuItem.status='Active' and  dbo.MenuItem.menugroupid='" + comboBox1.SelectedValue + "' order by dbo.MenuItem.MenugroupId, dbo.MenuItem.name ";
                }
                else
                {
                    q = "SELECT        dbo.MenuItem.Id, dbo.ModifierFlavour.name AS Size, dbo.ModifierFlavour.id AS flid,dbo.MenuItem.Name FROM            dbo.MenuItem LEFT OUTER JOIN                         dbo.ModifierFlavour ON dbo.MenuItem.Id = dbo.ModifierFlavour.MenuItemId where  dbo.MenuItem.status='Active' and dbo.MenuItem.menugroupid='" + comboBox1.SelectedValue + "' and dbo.MenuItem.name like '%" + textBox1.Text + "%' order by dbo.MenuItem.MenugroupId, dbo.MenuItem.name ";
                }

            }
            DataSet ds1 = new DataSet(); 
            ds1= objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                string status = "Pending";
                string val = "";
                double rem = 0;
                DataSet dspurchase = new DataSet();
                
                dspurchase = new DataSet();
                if (ds1.Tables[0].Rows[i]["Size"].ToString() == "")
                {
                    q = "SELECT     Menuitemid, Quantity, Date,status FROM     StockEstimation where Date ='" + date + "' and Menuitemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and OrderNo='"+txtorderno.Text+"'";
                }
                else
                {
                    q = "SELECT     Menuitemid, Quantity, Date,status FROM     StockEstimation where Date ='" + date + "' and Menuitemid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "' and Flavouid='" + ds1.Tables[0].Rows[i]["flid"].ToString() + "'  and OrderNo='" + txtorderno.Text + "'";
                }
                dspurchase = objcore.funGetDataSet(q);
                if (dspurchase.Tables[0].Rows.Count > 0)
                {
                    status = dspurchase.Tables[0].Rows[0]["Status"].ToString();
                    val = dspurchase.Tables[0].Rows[0]["Quantity"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tout = Convert.ToDouble(val);
                }

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["flid"].ToString(), ds1.Tables[0].Rows[i]["Size"].ToString(), ds1.Tables[0].Rows[i]["name"].ToString(), tout, status);
                tout = 0;
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[1].Visible = false;
            foreach (DataGridViewRow gr in dataGridView1.Rows)
            {
                if (gr.Cells["Status"].Value.ToString() == "Posted")
                {
                    gr.ReadOnly = true;
                    gr.DefaultCellStyle.BackColor = Color.Red; 
                }
            }
           
            
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
                    string q = "select * from StockEstimation where Menuitemid='" + dr.Cells["id"].Value.ToString() + "' and date='" + dateTimePicker1.Text + "' and Flavouid='" + dr.Cells["SizeId"].Value.ToString() + "' ";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        
                        {

                            q = "update StockEstimation set Quantity='" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 2).ToString() + "' where   id='" + dss.Tables[0].Rows[0]["id"].ToString() + "'";
                            objcore.executeQuery(q);
                            chk1 = true;
                        }                        
                    }
                    else
                    {
                        string qty = dr.Cells["Quantity"].Value.ToString();
                        if (qty == "")
                        {
                            qty = "0";
                        }
                        if (float.Parse(qty) > 0)
                        {
                            q = "insert into StockEstimation (Flavouid,Status,Date, Menuitemid, Quantity) values('" + dr.Cells["SizeId"].Value.ToString() + "','Pending','" + dateTimePicker1.Text + "','" + dr.Cells["id"].Value.ToString() + "','" + Math.Round(Convert.ToDouble(dr.Cells["Quantity"].Value.ToString()), 2).ToString() + "')";
                            objcore.executeQuery(q);
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
            StockEstimationList obj = new StockEstimationList(this);
            obj.Show();
        }
        public void fillitems()
        {
            try
            {
                DataTable dt = new DataTable();
                objcore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from menugroup order by name";
                ds = objcore.funGetDataSet(q);
                dt = ds.Tables[0];
               
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                
            }
            catch (Exception ex)
            {


            }


        }
        private void Variance_Load(object sender, EventArgs e)
        {
            fillitems();
        }
        protected void getbranchid()
        {
            try
            {
                string q = "select id from branch";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchid = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
        }
        string branchid = "";
        public void recipie(string itmid, float itmqnty, string flid)
        {
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "";
                if (flid == "")
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid IS NULL) or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='0') or dbo.Recipe.MenuItemId='" + itmid + "' and  (dbo.Recipe.modifierid ='')";
                }
                else
                {
                    q = "SELECT        dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity, dbo.Recipe.modifierid FROM            dbo.Recipe INNER JOIN                         dbo.RawItem ON dbo.Recipe.RawItemId = dbo.RawItem.Id INNER JOIN                         dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                         dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId where dbo.Recipe.MenuItemId='" + itmid + "' and (dbo.Recipe.modifierid ='" + flid + "')";
                }

                if (branchid == "")
                {
                    getbranchid();
                }
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        float qnty = float.Parse(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = double.Parse(dsrecipie.Tables[0].Rows[i]["Quantity"].ToString());
                        double amounttodeduct = recipiqnty;// (qnty / convrate) * recipiqnty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        amounttodeduct = Math.Round(amounttodeduct, 3);
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        
                        dsminus = new DataSet();
                        dsminus = objCore.funGetDataSet("select * from Discard where itemid='" + rawitmid + "' and Date='" + dateTimePicker1.Text + "'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            double inventryconsumedqty = 0;
                            try
                            {
                                inventryconsumedqty = double.Parse(dsminus.Tables[0].Rows[0]["completewaste"].ToString());
                            }
                            catch (Exception ex)
                            {
                                
                               
                            }

                            q = "update Discard set completewaste='" + (inventryconsumedqty + amounttodeduct) + "',uploadstatus='Pending' where id='" + dsminus.Tables[0].Rows[0]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                           DataSet ds = new DataSet();
                            int idcnsmd = 0;
                            ds = objCore.funGetDataSet("select MAX(ID) as id from Discard");
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
                            // idcnsmd = Convert.ToInt32(branchid + idcnsmd.ToString());
                            q = "insert into Discard (branchid,id, itemid, date, quantity, Discard, completewaste, staff) values('" + branchid + "','" + idcnsmd + "','" + rawitmid + "','" + dateTimePicker1.Text + "','0','0','" + amounttodeduct + "','0')";
                            objCore.executeQuery(q);
                        }

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton4_Click(object sender, EventArgs e)
        {
            POSRestaurant.Reports.Inventory.frmStockEstimationPreview obj = new Reports.Inventory.frmStockEstimationPreview();
            obj.date = dateTimePicker1.Text;
            obj.orderno = txtorderno.Text;
            obj.Show();
            //DialogResult dr = MessageBox.Show("Value can not be reversed after Posting","Are you Sure ?",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            //if (dr == DialogResult.Yes)
            //{
            //    foreach (DataGridViewRow gr in dataGridView1.Rows)
            //    {
            //        if (gr.Cells["Status"].Value.ToString() == "Pending")
            //        {
            //            string id = gr.Cells["id"].Value.ToString();
                       
                        
            //            string flv = gr.Cells["SizeId"].Value.ToString();
            //            string qty = gr.Cells["Quantity"].Value.ToString();
            //            if (qty == "")
            //            {
            //                qty = "0";
            //            }
            //            if (float.Parse(qty) > 0)
            //            {
            //                recipie(id, float.Parse(qty), flv);
            //                string q = "";
            //                if (flv == "")
            //                {
            //                    q = "update StockEstimation set status='Posted' where  Menuitemid='" + id + "' and  Date='" + dateTimePicker1.Text + "'";
            //                }
            //                else
            //                {
            //                    q = "update StockEstimation set status='Posted' where Flavouid='" + flv + "'and Menuitemid='" + id + "' and  Date='" + dateTimePicker1.Text + "'";
            //                }
            //                objcore.executeQuery(q);
            //            }
            //        }
            //    }
            //    getdata();
            //}
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
