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
    public partial class Waste : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string itid = "";
        public static double editqty = 0;
        public Waste()
        {
            InitializeComponent();
        }
        public void fillitems()
        {
            try
            {
                DataTable dt = new DataTable();
                objCore = new classes.Clsdbcon();
               DataSet ds = new DataSet();
               string q = "select * from Rawitem";
                ds = objCore.funGetDataSet(q);
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
                dr["ItemName"] = "Please Select";
                dt.Rows.InsertAt(dr, 0);
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "ItemName";
                comboBox1.Text = "Please Select";


            }
            catch (Exception ex)
            {


            }


        }
        public void fillgrid()
        {
            try
            {
               
                objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "SELECT      dbo.Waste.id, dbo.RawItem.ItemName, dbo.Waste.quantity AS WasteQuantity, dbo.UOMConversion.UOM, dbo.Waste.type AS WasteType, dbo.Waste.Date AS WasteDate FROM         dbo.RawItem INNER JOIN                      dbo.Waste ON dbo.RawItem.Id = dbo.Waste.itemid INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id INNER JOIN                      dbo.UOMConversion ON dbo.UOM.Id = dbo.UOMConversion.UOMId ORDER BY dbo.Waste.id DESC";
                ds = objCore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].Visible = false;

            }
            catch (Exception ex)
            {


            }


        }
        private void button1_Click(object sender, EventArgs e)
        {

        }
        public void recipie(string itmid, double itmqnty)
        {
            try
            {
                DataSet dsrecipie = new DataSet();
                string q = "SELECT     dbo.Recipe.RawItemId, dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.Recipe.MenuItemId, dbo.Recipe.Quantity FROM dbo.Recipe INNER JOIN                      dbo.UOMConversion ON dbo.Recipe.UOMCId = dbo.UOMConversion.Id where dbo.Recipe.MenuItemId='" + itmid + "'";
                q = "SELECT     dbo.UOMConversion.Qty, dbo.UOMConversion.ConversionRate, dbo.UOMConversion.UOM, dbo.RawItem.Id AS RawItemId FROM         dbo.UOMConversion INNER JOIN                      dbo.UOM ON dbo.UOMConversion.UOMId = dbo.UOM.Id INNER JOIN                      dbo.RawItem ON dbo.UOM.Id = dbo.RawItem.UOMId WHERE     (dbo.RawItem.Id = '" + itmid + "')";
                dsrecipie = objCore.funGetDataSet(q);
                if (dsrecipie.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsrecipie.Tables[0].Rows.Count; i++)
                    {
                        if (vButton1.Text == "Add")
                        {
                            //itmqnty = editqty - itmqnty;
                        }
                        else
                        {
                            //if (editqty > itmqnty)
                            //{
                            //    itmqnty =editqty - itmqnty ;
                            //}
                            //else
                            {
                                itmqnty = itmqnty - editqty;
                            }
                        }

                        string rawitmid = dsrecipie.Tables[0].Rows[i]["RawItemId"].ToString();
                        int qnty = Convert.ToInt32(dsrecipie.Tables[0].Rows[i]["Qty"].ToString());
                        double convrate = double.Parse(dsrecipie.Tables[0].Rows[i]["ConversionRate"].ToString());
                        double recipiqnty = itmqnty;
                        double amounttodeduct = (qnty / convrate) * recipiqnty;
                        //editqty = amounttodeduct * editqty;
                        amounttodeduct = amounttodeduct * itmqnty;
                        
                        DataSet dsminus = new DataSet();
                        double inventryqty = 0;
                        double newqty = 0;
                        dsminus = objCore.funGetDataSet("select * from Inventory where RawItemId='" + rawitmid + "'");
                        if (dsminus.Tables[0].Rows.Count > 0)
                        {
                            inventryqty = double.Parse(dsminus.Tables[0].Rows[i]["Quantity"].ToString());
                            if (itmqnty < 0)
                            {
                                newqty = inventryqty + amounttodeduct;
                            }
                            else
                            {
                                newqty = inventryqty - amounttodeduct;
                            }
                            objCore.executeQuery("update Inventory set Quantity='" + (newqty) + "' where id='" + dsminus.Tables[0].Rows[i]["id"].ToString() + "'");
                        }
                        string date = dateTimePicker1.Text;
                        if (vButton1.Text == "Add")
                        {
                            

                            DataSet ds = new DataSet();
                            int idcnsmd = 0;
                            ds = new DataSet();
                            ds = objCore.funGetDataSet("select max(id) as id from Waste");
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                string val = ds.Tables[0].Rows[0][0].ToString();
                                if (val == string.Empty)
                                {
                                    val = "0";
                                }
                                idcnsmd = Convert.ToInt32(val) + 1;
                            }
                            else
                            {
                                idcnsmd = 1;
                            }
                            objCore.executeQuery("insert into Waste (Id,itemid,quantity,type,Date) values('" + idcnsmd + "','" + rawitmid + "','" + textBox1.Text + "','" + comboBox2.Text + "','" + date + "')");

                        }
                        else
                        {
                            objCore.executeQuery("update Waste set quantity='" + textBox1.Text + "',type='" + comboBox2.Text + "',Date='" + date + "' where id='"+itid+"'");

                        }

                    }
                    
                }
                textBox1.Text = "";
                vButton1.Text = "Add";
            }
            catch (Exception ex)
            {


            }
        }

        private void Waste_Load(object sender, EventArgs e)
        {
            fillitems();
            fillgrid();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Please Select" || comboBox1.Text == "")
            {
                MessageBox.Show("Please Select Item");
                return;
            }
            if (comboBox2.Text == "Please Select" || comboBox2.Text == "")
            {
                MessageBox.Show("Please Select Waste Type");
                return;
            }
            if (textBox1.Text == "")
            {
                MessageBox.Show("Please Enter Quantity in Minimum Units");
                return;
            }
            recipie(comboBox1.SelectedValue.ToString(), float.Parse(textBox1.Text));
            fillgrid();
            
        }
        public void getdata(string id)
        {
            try
            {
                DataSet dsq = new DataSet();
                string q = "select * from waste where id='" + id + "'";
                dsq = objCore.funGetDataSet(q);
                if (dsq.Tables[0].Rows.Count > 0)
                {
                    itid = id;
                    comboBox1.SelectedValue = dsq.Tables[0].Rows[0]["itemid"].ToString();
                    comboBox2.Text = dsq.Tables[0].Rows[0]["type"].ToString();
                    textBox1.Text = dsq.Tables[0].Rows[0]["quantity"].ToString();
                    editqty = double.Parse(dsq.Tables[0].Rows[0]["quantity"].ToString());
                    dateTimePicker1.Text = dsq.Tables[0].Rows[0]["Date"].ToString();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                getdata(id);
                vButton1.Text = "Update";
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            vButton1.Text = "Add";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
