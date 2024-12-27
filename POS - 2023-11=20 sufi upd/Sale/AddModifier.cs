using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using VIBlend;
using VIBlend.Utilities;
namespace POSRestaurant.Sale
{
    public partial class AddModifier : Form
    {
        private RestSale _frm;
        public AddModifier(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        DataSet ds = new DataSet();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

        public DataTable dtmodify = new DataTable();
        private void AddModifier_Load(object sender, EventArgs e)
        {
            //this.TopMost = true;
            int rcount = 0, ccount = 0;
            tblpanel.Controls.Clear();
            //Clear out the existing row and column styles
            tblpanel.ColumnStyles.Clear();
            tblpanel.RowStyles.Clear();
            try
            {
                string q = "select * from Tablelayout where tablename='modifier'";
                DataSet dsf = new DataSet();
                dsf = objcore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows[0]["Columns"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    ccount = Convert.ToInt32(temp);
                    temp = dsf.Tables[0].Rows[0]["rows"].ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    rcount = Convert.ToInt32(temp);
                }
                else
                {
                    rcount = 5;
                    ccount = 4;
                }
                tblpanel.RowCount = rcount;
                tblpanel.ColumnCount = ccount;
                float cperc = 100 / tblpanel.ColumnCount;
                float rperc = 100 / tblpanel.RowCount;
                for (int i = 0; i < tblpanel.ColumnCount; i++)
                {
                    tblpanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));

                    for (int j = 0; j < tblpanel.RowCount; j++)
                    {
                        if (i == 0)
                        {
                            //defining the size of cell
                            tblpanel.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
                        }
                    }
                }
            }
            catch (Exception ex)
            {                                
            }
            getdata("");
           
        }
        int menuitemcolumns = 0, menuitemrows = 0;
        int menuitemcount = 0;
        int menuitemsno = 0;
        string menugrpid = "";
        int topcount = 0;
        private void AddModifier_Enter(object sender, EventArgs e)
        {
           
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        protected string getpricemethod()
        {
            string type = "net";
            string q = "select * from pricesetting";
            DataSet dsprice = new DataSet();
            try
            {
                dsprice = objcore.funGetDataSet(q);
                if (dsprice.Tables[0].Rows.Count > 0)
                {
                    type = dsprice.Tables[0].Rows[0]["type"].ToString();
                    if (type == "")
                    {
                        type = "net";
                    }
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {

                dsprice.Dispose();
            }
            return type;
        }
        public string gs = "0";
        private void btnmodify_Click(object sender, EventArgs e)
        {
            double price = 0;
            int indx = dataGridView1.CurrentCell.RowIndex;
            vButton bt = sender as vButton;
            DataRow dr = dtmodify.NewRow(); //Create New Row
            DataSet dsget = new DataSet();
            string q = "select price,grossprice from Modifier where id='" + bt.Name + "'";
            dsget = objcore.funGetDataSet(q);
            if (dsget.Tables[0].Rows.Count > 0)
            {
                string temp = dsget.Tables[0].Rows[0][0].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                price = Convert.ToDouble(temp);
            }
            if (getpricemethod().ToLower() == "gross")
            {
                string temp = dsget.Tables[0].Rows[0]["GrossPrice"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                price = float.Parse(temp);

                float g = float.Parse(gs) + 100;
                g = g / 100;

                price = price / g;
            }
            price = Math.Round(price, 2);
            dr["id"] = dataGridView1.Rows[indx].Cells[0].Value.ToString();
            dr["Name"] = bt.Text;
            dr["Quantity"] = "1";
            dr["Price"] = price;
            dr["mdid"] = bt.Name;
            dr["index"] = "";
            dtmodify.Rows.InsertAt(dr,indx+1);
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            int indx = dataGridView1.CurrentCell.RowIndex;
            DataRow dgr = dtmodify.Rows[indx];
            if (dataGridView1.Rows[indx].Cells["mdid"].Value.ToString() != "")
            {
                dgr.Delete();
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = 0;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    if (dr.Cells["mdid"].Value.ToString() == "")
                    {
                        indx = Convert.ToInt32(dr.Cells["index"].Value.ToString());
                    }
                    else
                    {
                        indx++;
                        _frm.fillgridmodifier(dr.Cells["id"].Value.ToString(), dr.Cells["mdid"].Value.ToString(), dr.Cells["Name"].Value.ToString(), dr.Cells["Price"].Value.ToString(), dr.Cells["Quantity"].Value.ToString(), "New", "", "", "", "", "", indx);
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
            this.Close();
            //dt.Rows.Add(id, mid, q, itmname, price, saletyp, saledetailsid, flavrid, comnts, runtimflid, kdid);
        }
        int rows = 0, colmn = 0;
        public void getdata(string type)
        {
            topcount = (tblpanel.ColumnCount * tblpanel.RowCount);
            colmn = 0;
            rows = 0;
            tblpanel.Controls.Clear();
            try
            {
                string q = "";
                if (type == "scroll")
                {
                    
                }
                else
                {
                    q = "select * from Modifier";
                    menuitemsno = 0;
                    DataSet ds1 = new DataSet();
                    ds1 = objcore.funGetDataSet(q);
                    if (ds1.Tables[0].Rows.Count > 0)
                    {
                        menuitemcount = ds1.Tables[0].Rows.Count;
                    }
                }
                q = "Select top " + topcount + " * from (select Id, RawItemId, Name, Price, Quantity, uploadstatus, branchid, kdsid,ROW_NUMBER() OVER (ORDER BY id) as Sno from Modifier )  as t where Sno >" + menuitemsno + "";
           
                ds = objcore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    vButton btn = new vButton();
                    btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                    btn.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                    btn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                    btn.Click += new EventHandler(btnmodify_Click);
                    btn.TextWrap = true;
                    btn.Dock = DockStyle.Fill;
                    btn.Font = new Font("", 12, FontStyle.Bold);
                   
                    tblpanel.Controls.Add(btn, colmn, rows);
                    colmn++;
                    if (colmn >= tblpanel.ColumnCount)
                    {
                        colmn = 0;
                        rows++;
                    }
                }
                dataGridView1.DataSource = dtmodify;
                dataGridView1.Columns[0].Visible = false;
                dataGridView1.Columns[4].Visible = false;
                dataGridView1.Columns[5].Visible = false;
            }
            catch (Exception ex)
            {
                
               
            }
        }
        private void vButton4_Click(object sender, EventArgs e)
        {
            menuitemcolumns = tblpanel.ColumnCount;
            menuitemrows = tblpanel.RowCount;
            if (menuitemsno > 0)
            {
                menuitemcount = menuitemcount + menuitemcolumns;
                menuitemsno = menuitemsno - menuitemcolumns;
                getdata( "scroll");
            }
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            menuitemcolumns = tblpanel.ColumnCount;
            menuitemrows = tblpanel.RowCount;
            if (menuitemcount > (menuitemcolumns * menuitemrows))
            {
                menuitemcount = menuitemcount - menuitemcolumns;
                menuitemsno = menuitemsno + menuitemcolumns;
                getdata("scroll");
            }
        }
    }
}
