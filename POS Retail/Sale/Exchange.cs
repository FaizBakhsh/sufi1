using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.Sale
{
    public partial class Exchange : Form
    {
        public DataTable dt = new DataTable();
        public DataTable dtnew = new DataTable();
        bool barcode = false;
        bool name = false;
        POSRetail.classes.Clsdbcon objcore = new classes.Clsdbcon();
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private Sale _frm1;
        public Exchange(Sale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
        }

        private void button1_Click(object sender, EventArgs e)
        {

            getbildetails();

        }
        string saleid = "";
        public void getbildetails()
        {
            try
            {
                double tamount = 0;
                DataSet dsinfo = new DataSet();
                string q = "SELECT    dbo.RawItem.id, dbo.RawItem.ItemName, dbo.Saledetails.Quantity, dbo.Saledetails.Price,dbo.Saledetails.saleid, dbo.Saledetails.TotalPrice, dbo.Color.Caption AS color, dbo.Size.SizeName AS Size FROM         dbo.Saledetails INNER JOIN                      dbo.RawItem ON dbo.Saledetails.ItemId = dbo.RawItem.Id INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id where dbo.Saledetails.saleid='" + textBox1.Text.Trim() + "'";
                dsinfo = objcore.funGetDataSet(q);
                for (int i = 0; i < dsinfo.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(dsinfo.Tables[0].Rows[i]["id"].ToString(), dsinfo.Tables[0].Rows[i]["ItemName"].ToString(), dsinfo.Tables[0].Rows[i]["color"].ToString(), dsinfo.Tables[0].Rows[i]["Size"].ToString(), dsinfo.Tables[0].Rows[i]["Quantity"].ToString(), dsinfo.Tables[0].Rows[i]["Price"].ToString(), dsinfo.Tables[0].Rows[i]["TotalPrice"].ToString());
                    string val = dsinfo.Tables[0].Rows[i]["TotalPrice"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    tamount = tamount + Convert.ToDouble(val);
                    saleid = dsinfo.Tables[0].Rows[i]["saleid"].ToString();
                }
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = dt;
                dataGridView1.Columns[0].Visible = false;
                label2.Text = "Total Amount= " + tamount.ToString();
                gettotal();
            }
            catch (Exception ex)
            {
                
                
            }
        }
        private void Exchange_Load(object sender, EventArgs e)
        {
            label5.Text = "0";
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            

            dt.Columns.Add("Color", typeof(string));
            dt.Columns.Add("Size", typeof(string));
            dt.Columns.Add("Qty", typeof(string));
            dt.Columns.Add("Price", typeof(string));
            dt.Columns.Add("TotalPrice", typeof(string));

            dtnew.Columns.Add("Id", typeof(string));
            dtnew.Columns.Add("Name", typeof(string));
           

            dtnew.Columns.Add("Color", typeof(string));
            dtnew.Columns.Add("Size", typeof(string));
            dtnew.Columns.Add("Qty", typeof(string));
            dtnew.Columns.Add("Price", typeof(string));
            dtnew.Columns.Add("TotalPrice", typeof(string));

           DataSet dsitems = new DataSet();
            dsitems = objcore.funGetDataSet("select ItemName from RawItem");
            AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
            for (int i = 0; i < dsitems.Tables[0].Rows.Count; i++)
            {
                MyCollection.Add(dsitems.Tables[0].Rows[i]["ItemName"].ToString());//.GetString(0));
            }
            textBox3.AutoCompleteCustomSource = MyCollection;

            textBox3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            textBox3.AutoCompleteSource = AutoCompleteSource.CustomSource;

        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            barcode = true;
            name = false;
        }
        public void getname()
        {
            try
            {
                if (name == false)
                {
                    return;
                }
                DataSet dsitem = new DataSet();

                dsitem = objcore.funGetDataSet("SELECT     dbo.Size.SizeName, dbo.Color.Caption, dbo.RawItem.* FROM         dbo.RawItem INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id where dbo.RawItem.ItemName='" + textBox3.Text.Trim() + "'");
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    name = false;
                    // if (barcode == false || code == false)
                    {
                        //   return;
                        //
                    }

                   // txtcode.Text = dsitem.Tables[0].Rows[0]["Code"].ToString();
                    textBox2.Text = dsitem.Tables[0].Rows[0]["BarCode"].ToString();
                    
                    //getpricetotal();

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void getbarcode()
        {
            textBox2.Text = textBox2.Text.Trim();
            if (barcode == false)
            {
                return;
            }
            try
            {
                DataSet dsitem = new DataSet();

                dsitem = objcore.funGetDataSet("SELECT     dbo.Size.SizeName, dbo.Color.Caption, dbo.RawItem.* FROM         dbo.RawItem INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id where dbo.RawItem.Barcode='" + textBox2.Text.Trim() + "'");
                if (dsitem.Tables[0].Rows.Count > 0)
                {
                    // barcode = false;
                    // if (name == false || code == false)
                    {
                        // return;
                        //
                    }
                    //if (code == true)
                    //{
                    //    
                    //}
                    //txtcode.Text = dsitem.Tables[0].Rows[0]["Code"].ToString();
                   // txtprice.Text = dsitem.Tables[0].Rows[0]["price"].ToString();
                    textBox3.Text = dsitem.Tables[0].Rows[0]["ItemName"].ToString();
                   // itemid = dsitem.Tables[0].Rows[0]["id"].ToString();
                   // size = dsitem.Tables[0].Rows[0]["SizeName"].ToString();
                   //// color = dsitem.Tables[0].Rows[0]["Caption"].ToString();
                    //txtitemquantity.Focus();
                   // getpricetotal();
                }
                    

                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                getbarcode();
            }
        }

        private void textBox3_Enter(object sender, EventArgs e)
        {
            barcode = false;
            name = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            getname();
        }
        public void clear()
        {
            textBox2.Text = "";
            textBox3.Text = "";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                string q = "SELECT    dbo.RawItem.id, dbo.RawItem.ItemName, dbo.Color.Caption AS color, dbo.Size.SizeName AS Size, dbo.RawItem.BarCode, dbo.RawItem.Price FROM         dbo.RawItem INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id where dbo.RawItem.BarCode='" + textBox2.Text + "'";
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dtnew.Rows.Add(ds.Tables[0].Rows[0]["id"].ToString(), ds.Tables[0].Rows[0]["ItemName"].ToString(), ds.Tables[0].Rows[0]["color"].ToString(), ds.Tables[0].Rows[0]["Size"].ToString(), "1", ds.Tables[0].Rows[0]["Price"].ToString(), ds.Tables[0].Rows[0]["Price"].ToString());
                }
                dataGridView2.DataSource = dtnew;
                dataGridView2.Refresh();
                dataGridView2.Columns[0].Visible = false;
                gettotal();
                clear();
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //  if (editsale == string.Empty)
                {
                    string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                   // string type = dataGridView1.Rows[e.RowIndex].Cells["SaleType"].Value.ToString();
                   //// if (type == "New")
                    {
                        DataRow dr = dt.Rows[e.RowIndex];
                        if (dr["id"].ToString() == Id)
                        {
                            dr.Delete();
                        }
                        dataGridView1.Refresh();
                        gettotal();
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //  if (editsale == string.Empty)
                {
                    string Id = dataGridView2.Rows[e.RowIndex].Cells[0].Value.ToString();
                    //string type = dataGridView2.Rows[e.RowIndex].Cells["SaleType"].Value.ToString();
                    //if (type == "New")
                    {
                        DataRow dr = dtnew.Rows[e.RowIndex];
                        if (dr["id"].ToString() == Id)
                        {
                            dr.Delete();
                        }
                        dataGridView2.Refresh();
                        gettotal();
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }
        public void gettotal()
        {
            try
            {
                double total = 0;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    string val = "";
                    val = dr.Cells["TotalPrice"].Value.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    total = total + Convert.ToDouble(val);
                }
                foreach (DataGridViewRow dr1 in dataGridView2.Rows)
                {
                    string val = "";
                    val = dr1.Cells["TotalPrice"].Value.ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    total = total + Convert.ToDouble(val);
                    
                }
                label5.Text = "Total Current Amount= " + total.ToString();
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        DataSet ds = new DataSet();
        public void cashaccount(string amount, string saleid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='Cash Account' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CashAccountSales");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CashAccountSales where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));


                    q = "insert into CashAccountSales (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','EX-" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void saleaccount(string amount, string saleid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='Sales Account' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from SalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from SalesAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into SalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','EX-" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void gstaccount(string amount, string saleid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "select * from CashSalesAccountsList where AccountType='GST Account' ";
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from GSTAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from GSTAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into GSTAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','EX-" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void discountaccount(string amount, string saleid)
        {
            try
            {

                //if (Convert.ToInt32(vall) > 0)
                {
                    DataSet dsacount = new DataSet();
                    string q = "select * from CashSalesAccountsList where AccountType='Discount Account' ";
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                        int iddd = 0;
                        ds = objCore.funGetDataSet("select max(id) as id from DiscountAccount");
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            string i = ds.Tables[0].Rows[0][0].ToString();
                            if (i == string.Empty)
                            {
                                i = "0";
                            }
                            iddd = Convert.ToInt32(i) + 1;
                        }
                        else
                        {
                            iddd = 1;
                        }
                        double balance = 0;
                        string val = "";
                        q = "select top 1 * from DiscountAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                        dsacount = new DataSet();
                        dsacount = objCore.funGetDataSet(q);
                        if (dsacount.Tables[0].Rows.Count > 0)
                        {
                            val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                        }
                        if (val == "")
                        {
                            val = "0";

                        }
                        balance = Convert.ToDouble(val);
                        double newbalance = (balance + Convert.ToDouble(amount));
                        newbalance = Math.Round(newbalance, 2);

                        q = "insert into DiscountAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','EX-" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                        objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void Costofsaleaccount(string amount, string saleid, string acountid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";// "SELECT     AVG(dbo.PurchaseDetails.PricePerItem) AS price, dbo.RawItem.Costofsalesid FROM         dbo.RawItem INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId WHERE     (dbo.PurchaseDetails.RawItemId = '" + itmid + "') GROUP BY dbo.RawItem.Costofsalesid ";
                //dsacount = objCore.funGetDataSet(q);
                //if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = acountid;
                    string val = "";
                    //val = dsacount.Tables[0].Rows[0]["price"].ToString();
                    //if (val == "")
                    //{
                    //    val = "0";
                    //}
                    //double prc = Convert.ToDouble(val) * Convert.ToInt32(amount);

                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CostSalesAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    val = "";
                    q = "select top 1 * from CostSalesAccount where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance - Convert.ToDouble(amount));
                    newbalance = Math.Round(newbalance, 2);


                    q = "insert into CostSalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','EX-" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void inventoryaccount(string amount, string saleid, string acountid, string itemid)
        {
            try
            {
                DataSet dsacount = new DataSet();
                string q = "";// "SELECT     AVG(dbo.PurchaseDetails.PricePerItem) AS price, dbo.RawItem.Inventoryid FROM         dbo.RawItem INNER JOIN                      dbo.PurchaseDetails ON dbo.RawItem.Id = dbo.PurchaseDetails.RawItemId WHERE     (dbo.PurchaseDetails.RawItemId = '" + itmid + "') GROUP BY dbo.RawItem.Inventoryid ";
                //dsacount = objCore.funGetDataSet(q);
                // if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string invntryid = acountid;
                    //amount = dsacount.Tables[0].Rows[0]["price"].ToString();

                    string val = "";
                    //val = dsacount.Tables[0].Rows[0]["price"].ToString();
                    //if (val == "")
                    //{
                    //    val = "0";
                    //}
                    //double prc = Convert.ToDouble(val) * Convert.ToInt32(amount);

                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from InventoryAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = 1;
                    }
                    double balance = 0;
                    val = "";
                    q = "select top 1 * from InventoryAccount where ChartAccountId='" + invntryid + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));

                    newbalance = Math.Round(newbalance, 2);

                    q = "insert into InventoryAccount (Id,Date,ItemId,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + itemid + "','" + invntryid + "','EX-" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to exchange ? you will not be able to reverse changings", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                int idd = 1;
                DataSet dsdayend = new DataSet();
                dsdayend = objCore.funGetDataSet("select max(id) as id from refund");
                if (dsdayend.Tables[0].Rows.Count > 0)
                {
                    string i = dsdayend.Tables[0].Rows[0][0].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    idd = Convert.ToInt32(i) + 1;

                }
                string q = "insert into exchange (id,saleid,reason) values('" + idd + "','" + saleid + "','')";
                objCore.executeQuery(q);
                q = "update sale set billstatus='Exchange' where id='" + saleid + "'";
                objCore.executeQuery(q);
                    
                DataSet dsacount = new DataSet();
                q = ""; string val = "";
                q = "select  * from CashAccountSales where VoucherNo='" + saleid + "'";
                dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    val = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    cashaccount(val, saleid);
                }

                q = "select  * from SalesAccount where VoucherNo='" + saleid + "'";
                dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    val = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    saleaccount(val, saleid);
                }

                q = "select  * from GSTAccount where VoucherNo='" + saleid + "'";
                dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    val = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    gstaccount(val, saleid);

                }

                q = "select  * from DiscountAccount where VoucherNo='" + saleid + "'";
                dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    val = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    discountaccount(val, saleid);

                }

                q = "select  * from CostSalesAccount where VoucherNo='" + saleid + "'";
                dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    val = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    Costofsaleaccount(val, saleid, dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString());

                }
                q = "select  * from InventoryAccount where VoucherNo='" + saleid + "'";
                dsacount = new DataSet();
                dsacount = objCore.funGetDataSet(q);
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    val = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                    if (val == "")
                    {
                        val = "0";
                    }
                    inventoryaccount(val, saleid, dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString(), dsacount.Tables[0].Rows[0]["ItemId"].ToString());

                }
                   
            }
            DataSet barcode = new DataSet();

            foreach (DataGridViewRow drr in dataGridView1.Rows)
            {
                barcode = new DataSet();
                //string q = "select barcode from rawitem where id='"+drr.Cells["id"].Value.ToString()+"'";
                //barcode = objcore.funGetDataSet(q);
                //if (barcode.Tables[0].Rows.Count > 0)
                //{
                //    _frm1.changequantity(drr.Cells["Qty"].Value.ToString());
                //    _frm1.getbarcode(barcode.Tables[0].Rows[0]["barcode"].ToString());
                //}
                _frm1.fillgrid(drr.Cells["id"].Value.ToString(), drr.Cells["totalprice"].Value.ToString(), drr.Cells["name"].Value.ToString(), drr.Cells["price"].Value.ToString(), drr.Cells["color"].Value.ToString(), drr.Cells["size"].Value.ToString(), drr.Cells["Qty"].Value.ToString(), "New", "");

                try
                {
                    //_frm1.obcustomerdisplay.fillgrid(drr.Cells["id"].Value.ToString(), drr.Cells["totalamount"].Value.ToString(), drr.Cells["name"].Value.ToString(), drr.Cells["price"].Value.ToString(), drr.Cells["color"].Value.ToString(), drr.Cells["size"].Value.ToString(), drr.Cells["Qty"].Value.ToString(), "New", "");
                    //obcustomerdisplay.fillgrid(itemid, txtitemtotal.Text, txtname.Text, txtprice.Text, color, size, txtitemquantity.Text, "New", "");

                }
                catch (Exception ex)
                {


                }
            }
            foreach (DataGridViewRow drr in dataGridView2.Rows)
            {
                barcode = new DataSet();
                string q = "select barcode from rawitem where id='"+drr.Cells["id"].Value.ToString()+"'";
                //barcode = objcore.funGetDataSet(q);
                //if (barcode.Tables[0].Rows.Count > 0)
                //{
                //    _frm1.changequantity(drr.Cells["Qty"].Value.ToString());
                //    _frm1.getbarcode(barcode.Tables[0].Rows[0]["barcode"].ToString());
                //}
                _frm1.fillgrid(drr.Cells["id"].Value.ToString(), drr.Cells["totalprice"].Value.ToString(), drr.Cells["name"].Value.ToString(), drr.Cells["price"].Value.ToString(), drr.Cells["color"].Value.ToString(), drr.Cells["size"].Value.ToString(), drr.Cells["Qty"].Value.ToString(), "New", "");

            }
            this.Close();
        }
        private TextBox focusedTextbox=null;
        private void textBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
                if (focusedTextbox.Name == "textBox2")
                {
                    barcode = true;
                    name = false;
                }
                if (focusedTextbox.Name == "textBox3")
                {
                    barcode = false;
                    name = true;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
        }

        private void button32_Click(object sender, EventArgs e)
        {
           
        }
        public void shiftkey()
        {
            if (vbutton2.Text != "!")
            {
                vbutton2.Text = "!";
                vbutton3.Text = "@";
                button4.Text = "#";
                button5.Text = "$";
                button6.Text = "%";
                button7.Text = "^";
                button8.Text = "&&";
                button9.Text = "*";
                button10.Text = "(";
                button11.Text = ")";
                button12.Text = "Q";
                button16.Text = "W";
                button18.Text = "E";
                button20.Text = "R";
                button22.Text = "T";
                button21.Text = "Y";
                button19.Text = "U";
                button17.Text = "I";
                button15.Text = "O";
                button13.Text = "P";

                button23.Text = "A";
                button25.Text = "S";
                button27.Text = "D";
                button29.Text = "F";
                button31.Text = "G";
                button30.Text = "H";
                button28.Text = "J";
                button26.Text = "K";
                button24.Text = "L";

                button33.Text = "Z";
                button35.Text = "X";
                button37.Text = "C";
                button39.Text = "V";
                button41.Text = "B";
                button40.Text = "N";
                button38.Text = "M";
                // button36.Text = "o";


            }
            else
            {
                vbutton2.Text = "1";
                vbutton3.Text = "2";
                button4.Text = "3";
                button5.Text = "4";
                button6.Text = "5";
                button7.Text = "6";
                button8.Text = "7";
                button9.Text = "8";
                button10.Text = "9";
                button11.Text = "0";
                button12.Text = "q";
                button16.Text = "w";
                button18.Text = "e";
                button20.Text = "r";
                button22.Text = "t";
                button21.Text = "y";
                button19.Text = "u";
                button17.Text = "i";
                button15.Text = "o";
                button13.Text = "p";

                button23.Text = "a";
                button25.Text = "s";
                button27.Text = "d";
                button29.Text = "f";
                button31.Text = "g";
                button30.Text = "h";
                button28.Text = "j";
                button26.Text = "k";
                button24.Text = "l";

                button33.Text = "z";
                button35.Text = "x";
                button37.Text = "c";
                button39.Text = "v";
                button41.Text = "b";
                button40.Text = "n";
                button38.Text = "m";


            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            shiftkey();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Button t = (sender) as Button;
            try
            {
                t = (sender) as Button;
                if (focusedTextbox != null)
                {

                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void button32_Click_1(object sender, EventArgs e)
        {
            try
            {
                int index = focusedTextbox.SelectionStart;
                focusedTextbox.Text = focusedTextbox.Text.Remove(focusedTextbox.SelectionStart - 1, 1);
                focusedTextbox.Select(index - 1, 1);
                focusedTextbox.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void button14_Click_1(object sender, EventArgs e)
        {
            shiftkey();
        }
    }
}
