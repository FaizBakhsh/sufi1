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
    public partial class RefundBill : Form
    {
        private  Sale _frm1;
        string date = "";
        POSRetail.classes.Clsdbcon objCore ;
        DataSet ds ;
        public RefundBill(Sale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            objCore = new classes.Clsdbcon();
        }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";
            
        //}

        private void button1_Click(object sender, EventArgs e)
        {
          
               
                    
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            getdata();
        }
        public void getdata()
        {
            try
            {
                //category
                
                ds = new DataSet();
                ds = objCore.funGetDataSet("select top(1) * from dayend where userid='" + id + "' order by id desc");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    date = ds.Tables[0].Rows[0]["Date"].ToString();
                }
                DataSet ds9 = new DataSet();
                string q9 = "";
                q9 = "SELECT     Id as BillNo, Date, time, NetBill   FROM         Sale where userid='" + id + "' and BillStatus='Paid' and date='"+date+"' order by id desc";

                
                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
                // dataGridView1.Columns[0].Visible = false;
                // dataGridView1.Columns[3].Visible = false;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 40;


                }
                

            }
            catch (Exception ex)
            {


            }
        }
        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text == string.Empty)
            {
                MessageBox.Show("Please Provide a Reason of Refund");
                return;
            }
            DialogResult rs = MessageBox.Show("Are You Sure to Refund this Bill","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (rs==DialogResult.Yes)
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                   
                    string saleid = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    string type = dataGridView1.Rows[indx].Cells[4].Value.ToString();

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
                    objCore.executeQuery("insert into refund (id,saleid,reason) values('" + idd + "','" + saleid + "','" + richTextBox1.Text.Replace("'", "''") + "')");
                    objCore.executeQuery("update sale set billstatus='Refund' where id='" + saleid + "'");
                    
                    
                    DataSet dsacount=new DataSet();
                    string q="",val="";
                    q = "select  * from CashAccountSales where VoucherNo='" + saleid + "'";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        cashaccount(val,saleid);
                    }

                    q = "select  * from SalesAccount where VoucherNo='" + saleid + "'";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        saleaccount(val,saleid);
                    }

                    q = "select  * from GSTAccount where VoucherNo='" + saleid + "'";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        gstaccount(val, saleid);
                        
                    }

                    q = "select  * from DiscountAccount where VoucherNo='" + saleid + "'";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        discountaccount(val, saleid);

                    }

                    q = "select  * from CostSalesAccount where VoucherNo='" + saleid + "'";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["Debit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        Costofsaleaccount(val, saleid, dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString());

                    }
                    q = "select  * from InventoryAccount where VoucherNo='" + saleid + "'";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["Credit"].ToString();
                        if(val=="")
                        {
                            val="0";
                        }
                        inventoryaccount(val, saleid, dsacount.Tables[0].Rows[0]["ChartAccountId"].ToString(), dsacount.Tables[0].Rows[0]["ItemId"].ToString());

                    }
                   
                    

                    getdata();
                    richTextBox1.Text = string.Empty;
                    txtbill.Text = string.Empty;
                    MessageBox.Show("Bill Refund Operation Completed Successfully");
                }
                else
                {
                    MessageBox.Show("Please Select a bill to Refund");
                } 
            }
            
        }
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


                    q = "insert into CashAccountSales (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','CN-" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
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


                    q = "insert into SalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','CN-" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
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


                    q = "insert into GSTAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','CN-" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
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

                        q = "insert into DiscountAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','CN-" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
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


                    q = "insert into CostSalesAccount (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + ChartAccountId + "','CN-" + saleid + "','','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
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

                    q = "insert into InventoryAccount (Id,Date,ItemId,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + DateTime.Now + "','" + itemid + "','" + invntryid + "','CN-" + saleid + "','','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton3_Click(object sender, EventArgs e)
        {
            //_frm1.Islbldelivery = "Not Selected";
            _frm1.Enabled = true;
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                q9 = "SELECT     Id as InvoiceNo, Date, time, NetBill   FROM         Sale where id='" + txtbill.Text.Trim() + "' and userid='" + id + "' and BillStatus='Paid' and date='" + date + "'";
                
                
                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
               // dataGridView1.Columns[0].Visible = false;
               // dataGridView1.Columns[3].Visible = false;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Height = 40;
                }

            }
            catch (Exception ex)
            {


            }
        }

        private void vButton2_Click_1(object sender, EventArgs e)
        {
            getdata();
        }
        private TextBox focusedTextbox=null;
        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
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

        private void button32_Click(object sender, EventArgs e)
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
        public void shiftkey()
        {
            if (button2.Text != "!")
            {
                button2.Text = "!";
                button3.Text = "@";
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
                button2.Text = "1";
                button3.Text = "2";
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

        private void richTextBox1_Enter_1(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
    }
}
