using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VIBlend.Utilities;
using VIBlend.WinForms.Controls;
namespace POSRestaurant.forms
{
    public partial class NewOrder : Form
    {
        public string total = "";
        private POSRestaurant.Sale.RestSale _frm;
        vTextBox focusedtextbox;
        public NewOrder(POSRestaurant.Sale.RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            focusedtextbox.Text = focusedtextbox.Text + btn.Text;
            focusedtextbox.Focus();
            focusedtextbox.SelectionStart = focusedtextbox.Text.Length;
            strt = focusedtextbox.SelectionStart;
        }

        private void vButton30_Click(object sender, EventArgs e)
        {
            focusedtextbox.Text = focusedtextbox.Text + " ";
            focusedtextbox.Focus();
            focusedtextbox.SelectionStart = focusedtextbox.Text.Length;
            strt = focusedtextbox.SelectionStart;
        }

        private void vButton29_Click(object sender, EventArgs e)
        {
           // _frm.TopMost = true;
            this.Close();
        }

        private void vButton27_Click(object sender, EventArgs e)
        {
            focusedtextbox.Text = "";
        }
        public static int strt = 0;
        private void vButton28_Click(object sender, EventArgs e)
        {
            try
            {
                if (strt > 0)
                {
                    int index = focusedtextbox.SelectionStart;

                    focusedtextbox.Text = focusedtextbox.Text.Remove(strt - 1, 1);
                    // txtcashreceived.Select(index - 1, 1);
                    //txtcashreceived.Select();
                    strt = strt - 1;
                    focusedtextbox.Focus();
                    focusedtextbox.SelectionStart = focusedtextbox.Text.Length;
                    strt = focusedtextbox.SelectionStart;
                    //txtcashreceived.Focus(); 
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vTextBox1_Enter(object sender, EventArgs e)
        {
            vTextBox1.Focus();
            focusedtextbox = vTextBox1;
            strt = focusedtextbox.SelectionStart;
        }
       public int saleid = 0;
       public string name = "", phone = "";
       POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void vButton31_Click(object sender, EventArgs e)
        {
            if (vTextBox1.Text != "")
            {
                if (txtphone.Text.Replace("Customer Phone", "").Length > 0)
                {
                    if (txtphone.Text.Length != 11)
                    {
                        MessageBox.Show("Invalid Phone number. Phone number must be like  03137807024");
                        return;
                    }
                    try
                    {
                        bool val = Regex.IsMatch(txtphone.Text, @"^\d+$");

                        if (val == false)
                        {
                            MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                            return;
                        }

                    }
                    catch (Exception ex)
                    {


                    }
                }
                if (saleid > 0)
                {
                    string q = "update sale set customer='" + vTextBox1.Text.Replace("Customer Name", "") + "',phone='" + txtphone.Text.Replace("Customer Phone", "") + "' where id='" + saleid + "'";
                    POSRestaurant.classes.Clsdbcon obj = new classes.Clsdbcon();
                    obj.executeQuery(q);
                    _frm.cahangetext(vTextBox1.Text);
                }
                else
                {
                    this.Enabled = false;
                    _frm.pay("Cash", "", "0", vTextBox1.Text.Replace("Customer Name", ""), "", txtphone.Text.Replace("Customer Phone", ""),"");
                }
                _frm.Enabled = true;
                // this.Close();
                //Sale.SaleAfter obj = new Sale.SaleAfter(_frm);
                //obj.total = total;
                //obj.name = vTextBox1.Text;
                //obj.Show();
                //_frm.neworder(vTextBox1.Text);
                //_frm.TopMost = true;
                this.Close();

            }
            //if (vTextBox1.Text != "")
            //{
            //    if (saleid > 0)
            //    {
            //        string q = "update sale set customer='"+vTextBox1.Text+"' where id='"+saleid+"'";
            //        POSRestaurant.classes.Clsdbcon obj = new classes.Clsdbcon();
            //        obj.executeQuery(q);
            //        _frm.cahangetext(vTextBox1.Text);
            //    }
            //    else
            //    {
            //        this.Enabled = false;
            //        _frm.pay("Cash", "", "0", vTextBox1.Text,"");
            //    }
            //    _frm.Enabled = true;
            //   // this.Close();
            //    //Sale.SaleAfter obj = new Sale.SaleAfter(_frm);
            //    //obj.total = total;
            //    //obj.name = vTextBox1.Text;
            //    //obj.Show();
            //    //_frm.neworder(vTextBox1.Text);
            //    //_frm.TopMost = true;
            //    this.Close();
                
            //}
        }

        private void NewOrder_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from sale where id='" + saleid + "'";
                DataSet dsf = new DataSet();
                try
                {

                    dsf = objcore.funGetDataSet(q);
                    if (dsf.Tables[0].Rows.Count > 0)
                    {
                        phone = dsf.Tables[0].Rows[0]["phone"].ToString();

                    }
                }
                catch (Exception ex)
                {


                }
            }
            catch (Exception ex)
            {
                
                
            }
            vTextBox1.Text = name;
            txtphone.Text = phone;
            focusedtextbox = txtphone;
            int rcount = 0, ccount = 0;
            try
            {
               string q = "select * from customers";
               DataSet dsf = new DataSet();
                dsf = objcore.funGetDataSet(q);
                if (dsf.Tables[0].Rows.Count > 0)
                {
                    string temp = dsf.Tables[0].Rows.Count.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    ccount = Convert.ToInt32(temp);
                    temp = "1";
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    rcount = Convert.ToInt32(temp);
                }
                else
                {
                    rcount = 1;
                    ccount = 10;
                }
                if (ccount > 10)
                {
                    rcount = 2;
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
           

            ////this.TopMost = true;
        }
        int rows = 0, colmn = 0;
        public void getdata(string type)
        {

            colmn = 0;
            rows = 0;
            tblpanel.Controls.Clear();
            try
            {
                string q = "select * from customers";

                DataSet ds = objcore.funGetDataSet(q);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    vButton btn = new vButton();
                    btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                    btn.Text = ds.Tables[0].Rows[i]["Name"].ToString();
                    btn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
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

            }
            catch (Exception ex)
            {


            }
        }
        private void btnmodify_Click(object sender, EventArgs e)
        {
            try
            {
                vButton btn = sender as vButton;
                vTextBox1.Text = btn.Text;
            }
            catch (Exception ex)
            {


            }
            
        }
        private void txtphone_Enter(object sender, EventArgs e)
        {
            focusedtextbox = txtphone;
            strt = txtphone.SelectionStart;
        }
    }
}
