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
    public partial class DiscountIndividual : Form
    {
        int count = 0;
        string[] tabsarray = new string[20];
        RestSale _frm;
        public int indx = 0;
        public string saleid = "", userid = "";
        public DiscountIndividual(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        public void changtext(Button btn, string text, string color, string img, string fontsize, string fontcolor)
        {

            try
            {
                btn.Text = text;
                btn.Text = text.Replace("&", "&&");
                btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                // string path = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                //string path = Application.StartupPath + "\\Resources\\ButtonIcons\\";
                btn.Font = new Font("", 12, FontStyle.Bold);
                btn.ForeColor = Color.White;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void Addbutton(Button btn)
        {
           
            btn.Width = 100;
            btn.Height = 100;

            flowLayoutPanel1.Controls.Add(btn);
            
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public void getkeys()
        {

            objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "SELECT     id, name, discount FROM         DiscountKeys";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // if (i == 0)
                    {
                        Button button = new Button();

                        button.Click += new EventHandler(vButton18_Click);

                        button.Name = ds.Tables[0].Rows[i]["id"].ToString();
                        changtext(button, ds.Tables[0].Rows[i]["Name"].ToString() + " (" + ds.Tables[0].Rows[i]["discount"].ToString() + " %)", "", "", "12", "");
                        Addbutton(button);
                        //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }

                }
            }
        }
       public DataTable dt = new DataTable();
        private void complimentry_Load(object sender, EventArgs e)
       {
           try
           {
               string no = POSRestaurant.Properties.Settings.Default.MainScreenLocation.ToString();
               if (Convert.ToInt32(no) > 0)
               {


                   Screen[] sc;
                   sc = Screen.AllScreens;
                   this.StartPosition = FormStartPosition.Manual;
                   int no1 = Convert.ToInt32(no);
                   this.Location = Screen.AllScreens[no1].WorkingArea.Location;
                   this.WindowState = FormWindowState.Normal;
                   this.StartPosition = FormStartPosition.CenterScreen;
                   this.WindowState = FormWindowState.Maximized;

               }

           }
           catch (Exception ex)
           {

           }
            //this.TopMost = true;
            try
            {
                dataGridView1.DataSource = dt;
                try
                {
                    dataGridView1.Columns[1].Visible = false;
                    dataGridView1.Columns[2].Visible = false;
                    dataGridView1.Columns[6].Visible = false;
                    dataGridView1.Columns[7].Visible = false;
                    dataGridView1.Columns[8].Visible = false;
                    dataGridView1.Columns[9].Visible = false;
                    dataGridView1.Columns[11].Visible = false;
                    dataGridView1.Columns[10].Visible = false;
                }
                catch (Exception ex)
                {


                }
                dataGridView1.MultiSelect = true;


                tabsarray = new string[dt.Rows.Count - 1];
            }
            catch (Exception ex)
            {
                               
            }
            getkeys();
        }

        private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            //if (dataGridView1.IsCurrentCellDirty)
            //{
            //    dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            //}
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].Selected = true;

            //}
           // dataGridView1.Rows[e.RowIndex].Selected = !dataGridView1.Rows[e.RowIndex].Selected;
        }
        public string applydiscount()
        {
            string apply = "before";
            
            
            DataSet dsdis = new DataSet();
            try
            {
                string q = "select * from applydiscount ";

                dsdis = objCore.funGetDataSet(q);
                if (dsdis.Tables[0].Rows.Count > 0)
                {
                    apply = dsdis.Tables[0].Rows[0]["apply"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            finally
            {
                dsdis.Dispose();
            }
            if (apply == "")
            {
                apply = "before";
            }
            return apply;
        }
        private void vButton18_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    try
                    {
                        DataGridViewCheckBoxCell chk = dataGridView1.Rows[i].Cells[0] as DataGridViewCheckBoxCell;
                        if (Convert.ToBoolean(chk.Value) == true)
                        {
                            double price = 0,dis=0; 
                            string temp= dt.Rows[i]["Price"].ToString();
                            if (temp == "")
                            {
                                temp = "0";
                            }
                            price = Convert.ToDouble(temp);

                            string q = "SELECT     id, name, discount FROM         DiscountKeys where id='" + btn.Name + "'";
                            DataSet ds = new DataSet();

                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                temp = ds.Tables[0].Rows[0]["discount"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                            }

                            if (applydiscount() == "before")
                            {

                            }
                            else
                            {

                            }

                            //temp=btn.Name;
                            dis = Convert.ToDouble(temp);
                            price = price * (dis / 100);
                            q = "delete from DiscountIndividual where Saledetailsid='" + dt.Rows[i]["SaleDetailid"].ToString() + "' ";
                            objCore.executeQuery(q);
                            discount(btn.Name.ToString(),price.ToString(), saleid, dt.Rows[i]["SaleDetailid"].ToString(), dt.Rows[i]["Id"].ToString(), dt.Rows[i]["flavourid"].ToString(), dt.Rows[i]["runtimeflavourid"].ToString());
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
               // _frm.updatecomplmntry(indx, dt,"");
                //_frm.TopMost = true;
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }
        protected void discount(string disperc, string dis, string saleid, string sdid, string mid, string flid, string rmid)
        {
            string q = "insert into DiscountIndividual (DiscountPerc,Discount, MenuItemId, Saleid, Userid, Date, Saledetailsid,Runtimemodifierid,flavourid)values('" + disperc + "','" + dis + "','" + mid + "','" + saleid + "','" + userid + "','" + DateTime.Now + "','" + sdid + "','" + rmid + "','" + flid + "')";
            objCore.executeQuery(q);
            q = "";
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            //_frm.TopMost = true;
            this.Close();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Cells[0].Value = true;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    dr.Cells[0].Value = false;
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
