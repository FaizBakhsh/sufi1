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
    public partial class MergeBills : Form
    {
        public MergeBills(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        public string date = "";
        RestSale _frm;
        POSRestaurant.classes.Clsdbcon ObjCore = new classes.Clsdbcon();
        protected void getdata()
        {
            try
            {
                string q = "select id,Customer as Name,BillType,Ordertype,NetBill from sale where billstatus='Pending' and date='" + date + "' order by id";
                DataSet ds = new DataSet();
                ds = ObjCore.funGetDataSet(q);
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[1].Visible = false;
            }
            catch (Exception ex)
            {
                
            }
        }
        private void MergeBills_Load(object sender, EventArgs e)
        {
            getdata();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            string id = "";
            DialogResult drr = MessageBox.Show("Are you sure to Merge Selected Bills? You Wont be able to Reverse This","",MessageBoxButtons.YesNo);
            if (drr == DialogResult.No)
            {
                return;
            }
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                try
                {
                    DataGridViewCheckBoxCell chk = dr.Cells[0] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(chk.Value) == true)
                    {

                        if (id == "")
                        {
                            id = dr.Cells[1].Value.ToString();
                        }
                        else
                        {
                            string saleid = dr.Cells[1].Value.ToString();
                            string q = "update saledetails set saleid=" + id + " where saleid=" + saleid;
                            ObjCore.executeQuery(q);
                            q = "update sale set billstatus='Merged' where id=" + saleid;
                            ObjCore.executeQuery(q);

                        }

                    }
                }
                catch (Exception ex)
                {

                }

            }
            
           
            _frm.callneworder(); 
            this.Close();
        }
    }
}
