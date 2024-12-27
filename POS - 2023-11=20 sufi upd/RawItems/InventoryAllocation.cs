using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class InventoryAllocation : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public InventoryAllocation()
        {
            InitializeComponent();
        }
       
        public void getdata(string search)
        {
          
           
            double qty = 0;
            DataTable ds = new DataTable();
            ds.Columns.Add("Id", typeof(string));
            ds.Columns.Add("Branch", typeof(string));
            ds.Columns.Add("Allocation", typeof(string));
           
            string q = "";

            q = "select id,branchname from branch where type='Take Away' or type='Dine In' and status='Active' order by branchname";
              
            DataSet ds1 = new DataSet();
            ds1 = objcore.funGetDataSet(q);
            for (int i = 0; i < ds1.Tables[0].Rows.Count; i++)
            {
                qty = 0;
                double total = 0,price=0;
                string sts = "Pending";
                q = "SELECT        Id, branchid, Percentage FROM            InventoryAllocation where branchid='" + ds1.Tables[0].Rows[i]["id"].ToString() + "'";
                DataSet dsdmnd = new DataSet();
                dsdmnd = objcore.funGetDataSet(q);
                if (dsdmnd.Tables[0].Rows.Count > 0)
                {


                    string tmp = dsdmnd.Tables[0].Rows[0]["Percentage"].ToString();
                    if (tmp == "")
                    {
                        tmp = "0";
                    }
                    qty = Convert.ToDouble(tmp);

                }

                ds.Rows.Add(ds1.Tables[0].Rows[i]["id"].ToString(), ds1.Tables[0].Rows[i]["branchname"].ToString() , qty.ToString());
            }
            dataGridView1.DataSource = ds;
            dataGridView1.Columns[0].Visible = false;
          
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
          
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.ColumnIndex == 2)
                {



                    DataSet dss = new DataSet();
                    string q = "select * from InventoryAllocation where branchid='" + dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString() + "'";
                    dss = objcore.funGetDataSet(q);
                    if (dss.Tables[0].Rows.Count > 0)
                    {

                        q = "update InventoryAllocation set Percentage='" + (Convert.ToDouble(dataGridView1.Rows[e.RowIndex].Cells["Allocation"].Value.ToString())).ToString() + "'  where   id='" + dss.Tables[0].Rows[0]["id"].ToString() + "'";
                        objcore.executeQuery(q);
                    }
                    else
                    {
                        q = "insert into InventoryAllocation ( branchid, Percentage) values('" + ((dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString())).ToString() + "' ,'" + dataGridView1.Rows[e.RowIndex].Cells["Allocation"].Value.ToString() + "')";
                        objcore.executeQuery(q);

                    }


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }
       
        string cs = "";
        bool chk1 = false;
        public static string branchid = "", url = "";
       
     
        private void vButton3_Click(object sender, EventArgs e)
        {
           
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void FoodDiscard_Load(object sender, EventArgs e)
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            getdata("");
        }

    }
}
