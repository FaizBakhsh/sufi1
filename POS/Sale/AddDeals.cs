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
    public partial class AddDeals : Form
    {
        private RestSale _frm;
        public AddDeals(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        DataSet ds = new DataSet();
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        public string size = "", sizeid = "", extraflavour = "", extraflavourchk = "", sizechk = "", extraflavourid = "", dealno = "";
        float sizeprice = 0;
        float totalitems = 0; int dealcount = 0;
        public DataTable dtmodify = new DataTable();
        private int getheight(string tb)
        {
            int h = 0;
            string q = "select top 1 height from " + tb;
            DataSet dsh = new DataSet();
            dsh = objcore.funGetDataSet(q);
            if (dsh.Tables[0].Rows.Count > 0)
            {
                string temp = dsh.Tables[0].Rows[0][0].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                h = Convert.ToInt32(temp);
            }
            return h;
        }
        private int getwidth(string tb)
        {
            int w = 0;
            string q = "select top 1 width from " + tb;
            DataSet dsh = new DataSet();
            dsh = objcore.funGetDataSet(q);
            if (dsh.Tables[0].Rows.Count > 0)
            {
                string temp = dsh.Tables[0].Rows[0][0].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                w = Convert.ToInt32(temp);
            }
            return w;
        }
        private int gettotalitems(string id)
        {
            int t = 0;
            string q = "select top 1 totalitems from Deals where id='" + id + "'";
            DataSet dsh = new DataSet();
            dsh = objcore.funGetDataSet(q);
            if (dsh.Tables[0].Rows.Count > 0)
            {
                string temp = dsh.Tables[0].Rows[0][0].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                t = Convert.ToInt32(temp);
            }
            if (dealcount > 0)
            {
                t = t * dealcount;
            }
            return t;
        }
        private void getheads()
        {
            ds = new DataSet();
            string q = "select * from DealHeads where status='active'";
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                vButton btn = new vButton();
                btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                btn.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                btn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                btn.Click += new EventHandler(btnhead_Click);
                btn.TextWrap = true;
                btn.Font = new Font("", 12, FontStyle.Bold);
                try
                {
                    btn.Width = getheight("DealHeads");
                    btn.Height = getwidth("DealHeads");
                }
                catch (Exception ex)
                {
                }
                flhead.Controls.Add(btn);
            }
        }
        string head = "";
        protected float itemdealcount()
        {
            float count = 0;
            if (dataGridView1.Rows.Count > 0 && totalitems > 0)
            {
                foreach (DataGridViewRow dgr in dataGridView1.Rows)
                {
                    if (dgr.Cells["topping"].Value.ToString() == "")
                    {
                        if (dgr.Cells["deal"].Value.ToString() == deal.ToString())
                        {
                            count = count + float.Parse(dgr.Cells["Quantity"].Value.ToString());
                        }
                    }
                }

            }
            return count;
        }
        public string dealidmain = "", dealnamemain = "";
        public void callheadclick(string id)
        {
            deal = 1;
            fldeals.Controls.Clear();
            flowLayoutPanel1.Controls.Clear();
            com = true;
            ds = new DataSet();
            string q = "SELECT dbo.Deals.name, dbo.Deals.headid, dbo.Deals.id, dbo.DealHeads.name AS head FROM  dbo.Deals INNER JOIN               dbo.DealHeads ON dbo.Deals.headid = dbo.DealHeads.id where dbo.Deals.id='" + id + "'";
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                string headid = ds.Tables[0].Rows[0]["headid"].ToString();
                head = ds.Tables[0].Rows[0]["head"].ToString();
                dealnamemain = ds.Tables[0].Rows[0]["name"].ToString();
                q = "select * from Deals where headid='" + headid + "' and status='active'";
                ds = objcore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    vButton btn = new vButton();
                    btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                    btn.Text = ds.Tables[0].Rows[i]["Name"].ToString();

                    btn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                    btn.Click += new EventHandler(btndeal_Click);
                    btn.TextWrap = true;
                    btn.Font = new Font("", 12, FontStyle.Bold);
                    try
                    {
                        btn.Width = getheight("Deals");
                        btn.Height = getwidth("Deals");
                    }
                    catch (Exception ex)
                    {
                    }
                    fldeals.Controls.Add(btn);
                }
            }

        }
        public void caldealclick(string id, string name)
        {
            buyoneprice = 0;
            insertdealname = false;
            dealcount = 0;
            if (dealno == "")
            {
                dealno = "0";
            }
            dealcount = Convert.ToInt32(dealno);
            if (dealcount == 0)
            {
                dealcount = 1;
            }
            dealno = "";
            //deal++;
            com = true;
            btnlarge.Visible = false; btnjumbo.Visible = false; btnregular.Visible = false; btnsmall.Visible = false;
            btnthick.Visible = false;
            btnthin.Visible = false;
            sizeid = ""; size = "";

            dealidd = id;
            dealname = name;
            DataSet dssize = new DataSet();
            string q = "SELECT distinct dbo.ModifierFlavour.name FROM  dbo.AttachMenu INNER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id where dbo.AttachMenu.Dealid='" + id + "'";
            dssize = objcore.funGetDataSet(q);
            if (dssize.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dssize.Tables[0].Rows.Count; i++)
                {
                    btnthick.Visible = true;
                    btnthin.Visible = true;
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "small")
                    {
                        btnsmall.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnsmall.Visible = true;
                    }
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "regular")
                    {
                        btnregular.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnregular.Visible = true;
                    }
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "large")
                    {
                        btnlarge.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnlarge.Visible = true;
                    }
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "jambo")
                    {
                        btnjumbo.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnjumbo.Visible = true;
                    }
                }
            }
            else
            {
                btnjumbo.Visible = false; btnlarge.Visible = false; btnregular.Visible = false; btnsmall.Visible = false;
                btnthick.Visible = false;
                btnthin.Visible = false;
                sizeid = ""; size = "";
            }

            totalitems = gettotalitems(id);
            getmenu(id);
        }
        private void btnhead_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (itemdealcount() > 0 && itemdealcount() < totalitems)
                {
                    MessageBox.Show("Please punch total " + totalitems.ToString() + " items in previuos deal ");
                    return;
                }
            }
            deal++;
            fldeals.Controls.Clear();
            flowLayoutPanel1.Controls.Clear();
            com = true;
            ds = new DataSet();
            vButton btn1 = sender as vButton;
            head = btn1.Text;
            string q = "select * from Deals where headid='" + btn1.Name + "' and status='active'";
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                vButton btn = new vButton();
                btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                btn.Text = ds.Tables[0].Rows[i]["Name"].ToString();

                btn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                btn.Click += new EventHandler(btndeal_Click);
                btn.TextWrap = true;
                btn.Font = new Font("", 12, FontStyle.Bold);
                try
                {
                    btn.Width = getheight("Deals");
                    btn.Height = getwidth("Deals");
                }
                catch (Exception ex)
                {
                }
                fldeals.Controls.Add(btn);
            }
        }
        string dealidd = "";
        private void btndeal_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                if (itemdealcount() > 0 && itemdealcount() < totalitems)
                {
                    MessageBox.Show("Please punch total " + totalitems.ToString() + " items in previuos deal ");
                    return;
                }
            }
            buyoneprice = 0;
            insertdealname = true;
            dealcount = 0;
            if (dealno == "")
            {
                dealno = "0";
            }
            dealcount = Convert.ToInt32(dealno);
            if (dealcount == 0)
            {
                dealcount = 1;
            }
            dealno = "";
            deal++;
            com = true;
            btnlarge.Visible = false; btnjumbo.Visible = false; btnregular.Visible = false; btnsmall.Visible = false;
            btnthick.Visible = false;
            btnthin.Visible = false;
            sizeid = ""; size = "";
            vButton btn1 = sender as vButton;
            dealidd = btn1.Name;
            dealname = btn1.Text;
            DataSet dssize = new DataSet();
            string q = "SELECT distinct dbo.ModifierFlavour.name FROM  dbo.AttachMenu INNER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id where dbo.AttachMenu.Dealid='" + btn1.Name + "'";
            dssize = objcore.funGetDataSet(q);
            if (dssize.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dssize.Tables[0].Rows.Count; i++)
                {
                    btnthick.Visible = true;
                    btnthin.Visible = true;
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "small")
                    {
                        btnsmall.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnsmall.Visible = true;
                    }
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "regular")
                    {
                        btnregular.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnregular.Visible = true;
                    }
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "large")
                    {
                        btnlarge.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnlarge.Visible = true;
                    }
                    if (dssize.Tables[0].Rows[i]["name"].ToString().ToLower() == "jambo")
                    {
                        btnjumbo.Text = dssize.Tables[0].Rows[i]["name"].ToString();
                        btnjumbo.Visible = true;
                    }
                }
            }
            else
            {
                btnjumbo.Visible = false; btnlarge.Visible = false; btnregular.Visible = false; btnsmall.Visible = false;
                btnthick.Visible = false;
                btnthin.Visible = false;
                sizeid = ""; size = "";
            }

            totalitems = gettotalitems(btn1.Name);
            getmenu(btn1.Name);
        }
        protected void adddeal(string id)
        {
            double price = 0;
            DataSet dsget = new DataSet();
            string q = "select * from Deals where id='" + id + "'";
            dsget = objcore.funGetDataSet(q);
            if (dsget.Tables[0].Rows.Count > 0)
            {
                string temp = dsget.Tables[0].Rows[0]["price"].ToString();
                if (temp == "")
                {
                    temp = "0";
                }
                price = Convert.ToDouble(temp) * dealcount;
                DataRow dr = dtmodify.NewRow();
                dr["id"] = "";
                dr["Name"] = dsget.Tables[0].Rows[0]["name"].ToString();
                dr["Quantity"] = dealcount;
                dr["Price"] = price;
                dr["mdid"] = id;
                dr["index"] = "";
                dr["deal"] = dsget.Tables[0].Rows[0]["name"].ToString();
                dr["flid"] = "";// dsget.Tables[0].Rows[i]["ModifierId"].ToString();
                dr["del"] = "yes";
                dr["topping"] = "";
                dr["extraflavourid"] = "";
                dr["menugrouid"] = "";
               
                dtmodify.Rows.InsertAt(dr, dataGridView1.Rows.Count);
                insertdealname = false;
            }
        }
        string dealid = "";
        double buyoneprice = 0;
        private void getmenu(string id)
        {
            flowLayoutPanel1.Controls.Clear();
            ds = new DataSet();
            int h = 0, w = 0;
            string q = "SELECT top 1 height, width from AttachMenu";
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                h = int.Parse(ds.Tables[0].Rows[0][0].ToString());
                w = int.Parse(ds.Tables[0].Rows[0][1].ToString());
            }
            dealid = id;
            q = "SELECT DISTINCT  dbo.MenuItem.Id, dbo.MenuItem.Name,dbo.AttachMenu.width,dbo.AttachMenu.height FROM  dbo.AttachMenu INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id where dbo.AttachMenu.Dealid='" + id + "' and dbo.AttachMenu.status='active' AND (dbo.AttachMenu.Compulsory = 'false')";
            ds = objcore.funGetDataSet(q);
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                vButton btn = new vButton();
                btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                btn.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                btn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                btn.Click += new EventHandler(btnmodify_Click);
                btn.TextWrap = true;
                btn.Font = new Font("", 12, FontStyle.Bold);
                try
                {
                    btn.Width = w;
                    btn.Height = h;
                }
                catch (Exception ex)
                {
                }
                flowLayoutPanel1.Controls.Add(btn);
            }
        }
        int deal = 0;
        private void AddModifier_Load(object sender, EventArgs e)
        {
            //deal = 0;
            getheads();

            dataGridView1.DataSource = dtmodify;
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
            dataGridView1.Columns[6].Visible = false;
            dataGridView1.Columns[7].Visible = false;
            dataGridView1.Columns[8].Visible = false;
            dataGridView1.Columns[9].Visible = false;
            dataGridView1.Columns[10].Visible = false;
            dataGridView1.Columns[11].Visible = false;
            if (dealidmain != "")
            {
                callheadclick(dealidmain);
                caldealclick(dealidmain, dealnamemain);
            }
        }

        private void AddModifier_Enter(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        protected double boneprice(string id, string siz)
        {
            double p = 0;
            string q = "select price from ModifierFlavour where menuitemid='" + id + "'  AND (name LIKE '%" + siz.Replace("'", "").Trim() + "%')";
            DataSet sprice = new DataSet();
            sprice = objcore.funGetDataSet(q);
            if (sprice.Tables[0].Rows.Count > 0)
            {
                string val = sprice.Tables[0].Rows[0][0].ToString();
                if (val == "")
                {
                    val = "0";
                }
                p = Convert.ToDouble(val);
            }
            return p;
        }
        bool com = true;
        string dealname = "";
        bool insertdealname = false;
        private void compulsory(string prc, double qnty)
        {
            string price = "";
            bool chk = true;
            int indx = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                indx = dataGridView1.CurrentCell.RowIndex;
            }

            //Create New Row
            DataSet dsget = new DataSet();
            string q = "SELECT dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.AttachMenu.Price, dbo.AttachMenu.id AS atid, dbo.AttachMenu.No, dbo.AttachMenu.ModifierId, dbo.ModifierFlavour.name AS flv FROM  dbo.AttachMenu INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id WHERE (dbo.AttachMenu.Dealid = '" + dealid + "') AND (dbo.AttachMenu.status = 'active') AND (dbo.AttachMenu.Compulsory = 'true')";
            dsget = objcore.funGetDataSet(q);
            for (int i = 0; i < dsget.Tables[0].Rows.Count; i++)
            {
                if (chk == true)
                {
                    price = dsget.Tables[0].Rows[i]["Price"].ToString();
                    if (price == "")
                    {
                        price = "0";
                    }

                    string temp = dsget.Tables[0].Rows[i]["No"].ToString();
                    if (temp == "")
                    {
                        temp = "1";
                    }
                    qnty = Convert.ToDouble(temp);
                    price = (Convert.ToDouble(prc) + Convert.ToDouble(price) * qnty).ToString();
                    chk = false;
                }
                else
                {
                    price = dsget.Tables[0].Rows[i]["Price"].ToString();
                    if (price == "")
                    {
                        price = "0";
                    }
                    string temp = dsget.Tables[0].Rows[i]["No"].ToString();
                    if (temp == "")
                    {
                        temp = "1";
                    }
                    qnty = Convert.ToDouble(temp);
                    price = (Convert.ToDouble(price) * qnty).ToString();
                }
                if (dsget.Tables[0].Rows[i]["flv"].ToString() != "")
                {
                    size = dsget.Tables[0].Rows[i]["flv"].ToString().Substring(0, 1) + "'";
                    sizeid = dsget.Tables[0].Rows[0]["ModifierId"].ToString();

                    if (size == "S'")
                    {
                        size = size + "PP '";
                    }
                    if (size == "R'")
                    {
                        size = "Reg '";
                    }
                    if (size == "L'")
                    {
                        size = "Lrg '";
                    }

                }
                DataRow dr = dtmodify.NewRow();
                dr["id"] = dsget.Tables[0].Rows[i]["id"].ToString();
                dr["Name"] = size + dsget.Tables[0].Rows[i]["Name"].ToString();
                dr["Quantity"] = qnty;
                dr["Price"] = price;
                dr["mdid"] = dsget.Tables[0].Rows[i]["atid"].ToString();
                dr["index"] = "";
                dr["deal"] = deal.ToString();
                dr["flid"] = sizeid;
                dr["del"] = "";
                dr["topping"] = "";
                dr["extraflavourid"] = "";
                dr["menugrouid"] = "";
                dtmodify.Rows.InsertAt(dr, dataGridView1.Rows.Count);

                com = false;
                try
                {
                    dataGridView1.ClearSelection();
                    dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                }
                catch (Exception ex)
                {


                }
            }
            dealno = "";

        }
        bool addsizeprice = true;
        private void btnmodify_Click(object sender, EventArgs e)
        {

            try
            {
                if (insertdealname == true)
                {
                    adddeal(dealidd);
                    addsizeprice = true;
                }
                string qty = "1";// 
                if (qty == "")
                {
                    qty = "1";
                }
                vButton bt = sender as vButton;
                double pqty = 1;
                if (dealno != "")
                {
                    pqty = Convert.ToDouble(dealno);
                }
                if (pqty == 0)
                {
                    pqty = 1;
                }
                dealno = "";
                string q = "";
                string price = "", size1 = "", sizeid1 = "",menugroupid="";
                string temsize = "";
                int indx = 0;
                double groupitems = 0;
                if (btnsmall.Visible == true || btnregular.Visible == true || btnlarge.Visible == true || btnjumbo.Visible == true)
                {
                    bool fl = true;
                    if (sizechk == "")
                    {
                        DataSet dschkflavr = new DataSet();
                        q = "select * from AttachMenu where Dealid='" + dealid + "' and MenuItemId='" + bt.Name + "'";
                        dschkflavr = objcore.funGetDataSet(q);
                        for (int k = 0; k < dschkflavr.Tables[0].Rows.Count; k++)
                        {
                            if (dschkflavr.Tables[0].Rows[k]["ModifierId"].ToString() != "")
                            {
                                fl = false;
                            }

                        }
                        if (fl == false)
                        {
                            MessageBox.Show("Please Select Size");
                            return;
                        }
                    }
                    else
                    {
                        DataSet dschkflavr = new DataSet();
                        q = "select * from AttachMenu where Dealid='" + dealid + "' and MenuItemId='" + bt.Name + "'";
                        dschkflavr = objcore.funGetDataSet(q);
                        for (int k = 0; k < dschkflavr.Tables[0].Rows.Count; k++)
                        {
                            if (dschkflavr.Tables[0].Rows[k]["ModifierId"].ToString() != "")
                            {
                                fl = false;
                            }

                        }

                    }

                    if (sizechk != "" && fl == false)
                    {
                        DataSet dsgetsize = new DataSet();
                        q = "SELECT dbo.ModifierFlavour.name, dbo.AttachMenu.Dealid, dbo.ModifierFlavour.Price, dbo.AttachMenu.ModifierId as id , dbo.AttachMenu.Menugroupid, dbo.AttachMenu.Menugroupitem FROM  dbo.AttachMenu INNER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id where dbo.AttachMenu.Dealid='" + dealid + "' and dbo.ModifierFlavour.name='" + sizechk + "' and dbo.AttachMenu.MenuItemId='" + bt.Name + "'";
                        dsgetsize = objcore.funGetDataSet(q);
                        if (dsgetsize.Tables[0].Rows.Count > 0)
                        {
                            menugroupid = dsgetsize.Tables[0].Rows[0]["Menugroupid"].ToString();
                            string tem = dsgetsize.Tables[0].Rows[0]["Menugroupitem"].ToString();
                            if (tem == "")
                            {
                                tem = "0";
                            }
                            groupitems = Convert.ToDouble(tem);
                            temsize = sizechk;
                            size1 = sizechk.Substring(0, 1) + "'";
                            sizeid1 = dsgetsize.Tables[0].Rows[0]["id"].ToString();

                            string val = "0";
                            if (val == "")
                            {
                                val = "0";
                            }

                            if (head.ToLower() == "buy 1 get 1 free" && addsizeprice == true)
                            {
                                val = dsgetsize.Tables[0].Rows[0]["price"].ToString();
                                if (val == "")
                                {
                                    val = "0";
                                }
                            }
                            if (size1 == "S'")
                            {
                                size1 = size1 + "PP '";
                            }
                            if (size1 == "R'")
                            {
                                size1 = "Reg '";
                            }
                            if (size1 == "L'")
                            {
                                size1 = "Lrg '";
                            }
                            if (size1 == "J'")
                            {
                                size1 = "Jmb '";
                            }
                            sizeprice = float.Parse(val);
                        }
                        else
                        {
                            MessageBox.Show("Invalid size for this Menu Item");
                            return;
                        }
                    }
                    else
                    {
                        q = "select * from attachmenu where Dealid='" + dealid + "'  and MenuItemId='" + bt.Name + "'";
                        DataSet dsgetsize = new DataSet();
                       
                        dsgetsize = objcore.funGetDataSet(q);
                        if (dsgetsize.Tables[0].Rows.Count > 0)
                        {
                            menugroupid = dsgetsize.Tables[0].Rows[0]["Menugroupid"].ToString();
                            string tem = dsgetsize.Tables[0].Rows[0]["Menugroupitem"].ToString();
                            if (tem == "")
                            {
                                tem = "0";
                            }
                            groupitems = Convert.ToDouble(tem);
                        }
                    }


                }
                else
                {
                    q = "select * from attachmenu where Dealid='" + dealid + "'  and MenuItemId='" + bt.Name + "'";
                    DataSet dsgetsize = new DataSet();

                    dsgetsize = objcore.funGetDataSet(q);
                    if (dsgetsize.Tables[0].Rows.Count > 0)
                    {
                        menugroupid = dsgetsize.Tables[0].Rows[0]["Menugroupid"].ToString();
                        string tem = dsgetsize.Tables[0].Rows[0]["Menugroupitem"].ToString();
                        if (tem == "")
                        {
                            tem = "0";
                        }
                        groupitems = Convert.ToDouble(tem);
                    }
                }
                //menu group items check
                if (dataGridView1.Rows.Count > 0 && groupitems > 0)
                {

                    float countg = 0;
                    foreach (DataGridViewRow dgr in dataGridView1.Rows)
                    {
                        if (dgr.Cells["deal"].Value.ToString() == deal.ToString())
                        {
                            if (dgr.Cells["topping"].Value.ToString() == "")
                            {
                                if (dgr.Cells["menugrouid"].Value.ToString() == menugroupid)
                                {
                                    countg = countg + float.Parse(dgr.Cells["Quantity"].Value.ToString());
                                }
                            }
                            //count++;
                        }
                    }
                    
                    countg = countg + float.Parse(pqty.ToString());


                    if (countg > groupitems)
                    {
                        MessageBox.Show("Only " + groupitems.ToString() + " total item(s) allowed in this Menu Group");
                        return;
                    }
                    
                }
                
                //menu group items check
                if (dataGridView1.Rows.Count > 0 && totalitems > 0)
                {

                    float count = 0;
                    foreach (DataGridViewRow dgr in dataGridView1.Rows)
                    {
                        if (dgr.Cells["deal"].Value.ToString() == deal.ToString())
                        {
                            if (dgr.Cells["topping"].Value.ToString() == "")
                            {
                                count = count + float.Parse(dgr.Cells["Quantity"].Value.ToString());
                            }
                            if (head.ToLower() == "buy 1 get 1 free")
                            {
                                if (dgr.Cells["topping"].Value.ToString() == "")
                                {
                                    string twmp = dgr.Cells["price"].Value.ToString();
                                    if (twmp == "")
                                    {
                                        twmp = "0";
                                    }
                                    if (twmp != "0")
                                    {
                                        buyoneprice = Convert.ToDouble(twmp);  //boneprice(dgr.Cells["id"].Value.ToString(), size1);
                                    }
                                }
                            }
                            //count++;
                        }
                    }
                    count = count + float.Parse(pqty.ToString());
                    if (head.ToLower() == "buy 1 get 1 free")
                    {
                        addsizeprice = false;
                        if (count < (totalitems / 2))
                        {
                            addsizeprice = true;
                        }
                    }
                    else
                    {
                        addsizeprice = false;
                    }
                    if (count > totalitems)
                    {
                        count = count - float.Parse(pqty.ToString());
                        pqty = (float.Parse(pqty.ToString()) / 2);
                        count = count + (float.Parse(pqty.ToString()));
                        if (count > totalitems)
                        {
                            MessageBox.Show("Only " + totalitems.ToString() + " total item(s) allowed in this deal");
                            dealno = "0";
                            return;
                        }
                    }
                }
                else
                {
                    float count = 0;
                    //qty = (Convert.ToDouble(qty) * pqty).ToString();
                    count = float.Parse((pqty).ToString());
                    if (count > totalitems)
                    {
                        MessageBox.Show("Only " + totalitems.ToString() + " total item(s) allowed in this deal");
                        dealno = "0";
                        return;
                    }
                }
                bool pricead = false;
                DataRow dr1 = dtmodify.NewRow(); //Create New Row
                DataSet dsget = new DataSet();
                price = "0";
                if (com == true)
                {
                    compulsory(price, pqty);
                }
                DataSet dsget1 = new DataSet();
                q = "SELECT dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.AttachMenu.Price, dbo.AttachMenu.id AS atid, dbo.AttachMenu.No, dbo.AttachMenu.ModifierId, dbo.ModifierFlavour.name AS flv FROM  dbo.AttachMenu INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id LEFT OUTER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id WHERE (dbo.AttachMenu.Dealid = '" + dealid + "') AND (dbo.AttachMenu.status = 'active') AND (dbo.AttachMenu.Compulsory = 'true')";
                dsget1 = objcore.funGetDataSet(q);
                if (dsget1.Tables[0].Rows.Count > 0)
                {
                }
                else
                {
                    pricead = true;
                }
                if (dataGridView1.Rows.Count > 0)
                {
                    indx = dataGridView1.CurrentCell.RowIndex;
                }
                dsget = new DataSet();
                if (size1 == "")
                {
                    q = "SELECT dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.AttachMenu.Price, dbo.AttachMenu.No,dbo.AttachMenu.ModifierId, dbo.AttachMenu.id as atid FROM  dbo.AttachMenu INNER JOIN               dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id WHERE dbo.AttachMenu.Dealid='" + dealid + "' and (dbo.AttachMenu.MenuItemId = '" + bt.Name + "') ";
                }
                else
                {
                    q = "SELECT dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.AttachMenu.Price, dbo.AttachMenu.No, dbo.AttachMenu.ModifierId, dbo.AttachMenu.id AS atid, dbo.ModifierFlavour.name AS Expr1 FROM  dbo.AttachMenu INNER JOIN              dbo.MenuItem ON dbo.AttachMenu.MenuItemId = dbo.MenuItem.Id INNER JOIN               dbo.ModifierFlavour ON dbo.AttachMenu.ModifierId = dbo.ModifierFlavour.Id WHERE dbo.AttachMenu.Dealid='" + dealid + "' and (dbo.AttachMenu.MenuItemId = '" + bt.Name + "') and dbo.AttachMenu.ModifierId='" + sizeid1 + "'";
                }
                dsget = objcore.funGetDataSet(q);
                if (dsget.Tables[0].Rows.Count > 0)
                {
                    float no = 0;
                    no = int.Parse(dsget.Tables[0].Rows[0]["No"].ToString());
                    if (dealcount > 0)
                    {
                        no = no * dealcount;
                    }
                    if (dataGridView1.Rows.Count > 0)
                    {
                        float tempno = 0;
                        foreach (DataGridViewRow dgr in dataGridView1.Rows)
                        {
                            if (dgr.Cells["deal"].Value.ToString() == deal.ToString())
                            {
                                if (size1 == "")
                                {
                                    if (dgr.Cells["id"].Value.ToString() == dsget.Tables[0].Rows[0]["id"].ToString())
                                    {
                                        if (dgr.Cells["topping"].Value.ToString() == "")
                                        {
                                            tempno = tempno + float.Parse(dgr.Cells["Quantity"].Value.ToString());
                                        }
                                        // tempno++;
                                    }
                                }
                                else
                                {
                                    int strt = dgr.Cells["Name"].Value.ToString().IndexOf("-", 0);
                                    string temp = dgr.Cells["Name"].Value.ToString().Substring(strt + 1);

                                    int end = temp.IndexOf("'", 0);
                                    temp = temp.Substring(0, end + 1);
                                    if (temp == "S'")
                                    {
                                        temp = temp + "PP '";
                                    }
                                    if (temp == size1)
                                    {
                                        if (dgr.Cells["topping"].Value.ToString() == "")
                                        {
                                            tempno = tempno + float.Parse(dgr.Cells["Quantity"].Value.ToString());
                                        }
                                        //tempno++;
                                    }
                                }
                            }
                        }
                        tempno = tempno + float.Parse(pqty.ToString());
                        if (tempno > no)
                        {
                            tempno = tempno - float.Parse(pqty.ToString());
                            pqty = pqty / 2;
                            tempno = tempno + float.Parse(pqty.ToString());
                            if (tempno > no)
                            {
                                if (size1 == "")
                                {
                                    MessageBox.Show("Only " + no.ToString() + "  " + bt.Text + " allowed");
                                }
                                else
                                {
                                    MessageBox.Show("Only " + no.ToString() + "  " + temsize + " allowed");
                                }
                                return;
                            }
                        }
                    }
                    if (pricead == true && com == true)
                    {
                        com = false;
                        string tmp = dsget.Tables[0].Rows[0]["Price"].ToString();
                        if (tmp == "")
                        {
                            tmp = "0";
                        }
                        if (dealcount > 0)
                        {
                            price = (Convert.ToDouble(price) * dealcount + Convert.ToDouble(tmp)).ToString();
                        }
                        else
                        {
                            price = (Convert.ToDouble(price) + Convert.ToDouble(tmp)).ToString();
                        }
                    }
                    else
                    {
                        price = dsget.Tables[0].Rows[0]["Price"].ToString();
                        if (dealcount > 0)
                        {
                            price = (Convert.ToDouble(price) * dealcount).ToString();
                        }
                        else
                        {
                            price = (Convert.ToDouble(price)).ToString();
                        }
                    }
                    if (price == "")
                    {
                        price = "0";
                    }
                    //qty=(Convert.ToDouble(qty)* pqty).ToString();
                    if (head.ToLower() == "buy 1 get 1 free")
                    {

                        if (buyoneprice > 0)
                        {
                            double tempprice = boneprice(dsget.Tables[0].Rows[0]["id"].ToString(), size1);
                            if (tempprice > buyoneprice)
                            {
                                MessageBox.Show("You can not add an item with price greater than previous item price");
                                return;
                            }
                        }
                    }
                    if (head.ToLower() == "buy 1 get 1 free")
                    {
                        float count = 0;
                        foreach (DataGridViewRow dgr in dataGridView1.Rows)
                        {
                            if (dgr.Cells["deal"].Value.ToString() == deal.ToString())
                            {
                                if (dgr.Cells["topping"].Value.ToString() == "")
                                {
                                    count = count + float.Parse(dgr.Cells["Quantity"].Value.ToString());
                                }
                            }
                        }

                        if ((sizeprice) > 0 && (pqty + count) > (totalitems))
                        {
                            pqty = ((totalitems) / 2);
                            pqty = pqty - count;
                            if (pqty < 0)
                            {
                                pqty = -1 * pqty;
                            }
                            if (pqty == 0)
                            {
                                pqty = 1;
                            }
                        }
                    }
                    double temquantity = pqty;
                    if (head.ToLower() == "buy 1 get 1 free")
                    {
                        double temppqty = 0, temprice = 0; ;
                        if ((Convert.ToDouble(price) + sizeprice) > 0 && pqty > 1)
                        {
                            foreach (DataGridViewRow dgr in dataGridView1.Rows)
                            {
                                if (dgr.Cells["deal"].Value.ToString() == deal.ToString())
                                {
                                    if (dgr.Cells["topping"].Value.ToString() == "")
                                    {
                                        if (dgr.Cells["Price"].Value.ToString() != "0")
                                        {
                                            temppqty = temppqty + float.Parse(dgr.Cells["Quantity"].Value.ToString());
                                            temprice = temprice + float.Parse(dgr.Cells["Price"].Value.ToString());
                                        }
                                    }

                                }
                            }

                            if ((temppqty + pqty) > (totalitems / 2))
                            {
                                temquantity = (totalitems / 2) - temppqty;
                            }
                        }
                    }
                    if (pqty < 1)
                    {
                        price = ((Convert.ToDouble(price) + sizeprice)).ToString();
                    }
                    else
                    {
                        price = ((Convert.ToDouble(price) + sizeprice) * temquantity).ToString();
                    }
                    if (extraflvr != "")
                    {
                        DataSet dsgetsize = new DataSet();
                        q = "select * from ModifierextraFlavour where name='" + extraflvr + "' and MenuItemId='" + dsget.Tables[0].Rows[0]["id"].ToString() + "'";
                        dsgetsize = objcore.funGetDataSet(q);
                        if (dsgetsize.Tables[0].Rows.Count > 0)
                        {
                            //extraflavour = extraflavourchk.Substring(0, 1) + "'";
                            extraflavourid = dsgetsize.Tables[0].Rows[0]["id"].ToString();

                            extraflvr = extraflvr + " ' ";

                        }
                        else
                        {
                            extraflavourid = "";
                            extraflvr = "";
                        }
                    }
                    dr1["id"] = dsget.Tables[0].Rows[0]["id"].ToString();
                    dr1["Name"] = extraflvr + size1 + dsget.Tables[0].Rows[0]["Name"].ToString();
                    dr1["Quantity"] = pqty;
                    dr1["Price"] = price;
                    dr1["mdid"] = dsget.Tables[0].Rows[0]["atid"].ToString();
                    dr1["index"] = "";
                    dr1["deal"] = deal.ToString();
                    dr1["flid"] = sizeid1;
                    dr1["del"] = "";
                    dr1["topping"] = "";
                    dr1["extraflavourid"] = extraflavourid;
                    dr1["menugrouid"] = menugroupid;
                    extraflavourid = "";
                    extraflvr = "";
                    dtmodify.Rows.InsertAt(dr1, dataGridView1.Rows.Count);
                    try
                    {
                        dataGridView1.ClearSelection();
                        dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                size = "";
                sizeid = "";
                sizeprice = 0;
                sizechk = "";
                extraflavourid = "";
                extraflavour = "";
                extraflavourchk = "";
                dealno = "";
                dataGridView1.CurrentCell = dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells[1];
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
                //int h = dataGridView1.CurrentCell.RowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void filldeal(string id, string name, string qty, string price, string deal, string flavour, int indx, string top)
        {
            DataRow dr = dtmodify.NewRow();
            dr[0] = id;
            dr[1] = name; dr[2] = qty; dr[3] = price; dr[4] = ""; dr[5] = "";
            dr[6] = deal; dr[7] = flavour; dr[8] = ""; dr[9] = top;
            dtmodify.Rows.InsertAt(dr, indx);
            // dtmodify.Rows.Add(id,name,qty,price,"","",deal,flavour,"");
            try
            {
                dataGridView1.ClearSelection();
                dataGridView1.Rows[dataGridView1.Rows.Count - 1].Selected = true;
            }
            catch (Exception ex)
            {

            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                int count = dtmodify.Rows.Count;
                try
                {
                    string deal = dataGridView1.Rows[indx].Cells["del"].Value.ToString();
                    string dealnotemp = dataGridView1.Rows[indx].Cells["deal"].Value.ToString();
                    string dealno = "";
                    try
                    {
                        dealno = dataGridView1.Rows[indx + 1].Cells["deal"].Value.ToString();
                    }
                    catch (Exception s)
                    {
                    }
                    string val = "";
                    if (deal == "yes")
                    {
                        insertdealname = true;
                        for (int i = count - 1; i >= 0; i--)
                        {

                            if (dataGridView1.Rows[i].Cells["deal"].Value.ToString() == dealno || dataGridView1.Rows[i].Cells["deal"].Value.ToString() == dealnotemp)
                            {
                                DataRow dgr = dtmodify.Rows[i];
                                dgr.Delete();
                            }
                        }
                    }
                    else
                    {
                        val = dataGridView1.Rows[indx].Cells["deal"].Value.ToString();
                        if (head.ToLower() == "buy 1 get 1 free")
                        {
                            if (dataGridView1.Rows[indx].Cells["Topping"].Value.ToString().ToLower() == "topping")
                            { }
                            else
                            {
                                string price = dataGridView1.Rows[indx].Cells["price"].Value.ToString();
                                if (price == "")
                                {
                                    price = "0";
                                }
                                if (price != "0")
                                {
                                    addsizeprice = true;

                                }
                            }
                        }
                        DataRow dgr = dtmodify.Rows[indx];
                        if (dataGridView1.Rows[indx].Cells["mdid"].Value.ToString() != "")
                        {
                            dgr.Delete();
                        }
                        else
                            if (dataGridView1.Rows[indx].Cells["Topping"].Value.ToString().ToLower() == "topping")
                            {
                                dgr.Delete();
                            }
                        //indx--;
                        if (indx > 0)
                        {
                            try
                            {
                                string nxt = "", previous = "";

                                try
                                {
                                    if (dataGridView1.Rows[indx].Cells["deal"].Value.ToString() == val)
                                    {
                                        nxt = "yes";
                                    }
                                }
                                catch (Exception ws)
                                {


                                }
                                try
                                {
                                    if (dataGridView1.Rows[indx - 1].Cells["deal"].Value.ToString() == val)
                                    {
                                        previous = "yes";
                                    }
                                }
                                catch (Exception ws)
                                {


                                }
                                if (nxt == "yes" || previous == "yes")
                                {
                                }
                                else
                                {
                                    {
                                        dgr = dtmodify.Rows[indx - 1];
                                        if (dataGridView1.Rows[indx - 1].Cells["mdid"].Value.ToString() != "")
                                        {
                                            dgr.Delete();
                                        }
                                        insertdealname = true;
                                    }
                                }
                            }
                            catch (Exception ed)
                            {


                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                    try
                    {
                        DataRow dgr = dtmodify.Rows[indx];
                        if (dataGridView1.Rows[indx].Cells["mdid"].Value.ToString() != "")
                        {
                            dgr.Delete();
                        }

                    }
                    catch (Exception ex1)
                    {


                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    //if (itemdealcount() > 0 && itemdealcount() < totalitems)
                    if (itemdealcount() < totalitems)
                    {
                        MessageBox.Show("Please punch total " + totalitems.ToString() + " items in previuos deal ");
                        return;
                    }
                    if (itemdealcount() > totalitems)
                    {
                        MessageBox.Show("Please punch total " + totalitems.ToString() + " items in previuos deal ");
                        return;
                    }
                }

                int indx = 0;
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    //if (dr.Cells["mdid"].Value.ToString() == "")
                    //{
                    //    indx = Convert.ToInt32(dr.Cells["index"].Value.ToString());
                    //}
                    //else
                    {
                        indx++;
                        _frm.fillgrideal(dr.Cells["id"].Value.ToString(), "", dr.Cells["Name"].Value.ToString(), dr.Cells["Price"].Value.ToString(), dr.Cells["Quantity"].Value.ToString(), "New", "", "", "", "", "", indx, dr.Cells["mdid"].Value.ToString(), dr.Cells["flid"].Value.ToString(), dr.Cells["extraflavourid"].Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            this.Close();
            //dt.Rows.Add(id, mid, q, itmname, price, saletyp, saledetailsid, flavrid, comnts, runtimflid, kdid);
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnsmall_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            sizechk = btn.Text;
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                DataGridView grid = dataGridView1 as DataGridView;
                //int indx = grid.CurrentCell.RowIndex;
                int index = grid.SelectedRows[0].Index;
                foreach (DataGridViewRow dgr in grid.SelectedRows)
                {
                    //string price = dgr.Cells["price"].Value.ToString();
                    //if (price == "")
                    //{
                    //    price = "0";
                    //}
                    //if (price != "0")
                    //{
                    //    addsizeprice = true;
                    //}
                    float q = float.Parse(dgr.Cells["deal"].Value.ToString());
                    float qty = float.Parse(dgr.Cells["Quantity"].Value.ToString());
                    qty = qty / 2;
                    dgr.Cells["Quantity"].Value = qty;
                }

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message);
            }
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            string val = btn.Text;
            dealno = dealno + val;

        }

        private void vButton15_Click(object sender, EventArgs e)
        {
            //if (dataGridView1.Rows.Count < 1)
            //{
            //    return;
            //}
            //try
            //{
            //    //ToppingDeal obj = new ToppingDeal(this);

            //    DataTable dtm = new DataTable();
            //    dtm.Columns.Add("id", typeof(string));
            //    dtm.Columns.Add("Name", typeof(string));
            //    dtm.Columns.Add("Quantity", typeof(string));
            //    dtm.Columns.Add("Price", typeof(string));
            //    dtm.Columns.Add("mdid", typeof(string));
            //    dtm.Columns.Add("index", typeof(string));
            //    int indx = dataGridView1.CurrentCell.RowIndex;
            //    //indx--;
            //    string Id = "";// dataGridView1.Rows[indx].Cells["id"].Value.ToString();
            //    string name = dataGridView1.Rows[indx].Cells["Name"].Value.ToString();
            //    string price = dataGridView1.Rows[indx].Cells["Price"].Value.ToString();
            //    string q = dataGridView1.Rows[indx].Cells["Quantity"].Value.ToString();
            //    dtm.Rows.Add(Id, name, q, price, "", indx.ToString());
            //    obj.dtmodify = dtm;
            //    obj.deall = dataGridView1.Rows[indx].Cells["deal"].Value.ToString();
            //    obj.Show();
            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
        }
        private void tableLayoutPanel8_Paint(object sender, PaintEventArgs e)
        {

        }
        string extraflvr = "";
        private void btnthin_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            extraflvr = btn.Text;
        }
    }
}
