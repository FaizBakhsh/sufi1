using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;

namespace POSRestaurant.Sale
{
    public partial class ShiftStart : Form
    {
        private EmployeeFunctions _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string date = "";
        public ShiftStart(EmployeeFunctions frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void ShiftStart_Load(object sender, EventArgs e)
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
                    //this.WindowState = FormWindowState.Maximized;

                }

            }
            catch (Exception ex)
            {

            }
            this.TopMost = true;
            try
            {
               DataSet ds = new DataSet();
               string q = "select id,name from Shifts";
                ds = objCore.funGetDataSet(q);
                //DataRow dr = ds.Tables[0].NewRow();
                //dr["name"] = "All Users";
                //ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            vTextBox1.Focus();
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please Select a Shift");
                return;
            }
            if (vTextBox1.Text == "")
            {
                MessageBox.Show("Please Enter Float Cash");
                return;
            }
            if (vTextBox1.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(vTextBox1.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    vTextBox1.Focus();
                    return;
                }
            }
            string id = "";
            string d = "";
            DataSet dsgst = new DataSet();
            dsgst = objCore.funGetDataSet("select  * from ShiftStart  where date='" + date + "' and shiftid='" + comboBox1.SelectedValue + "' and terminal='" + System.Environment.MachineName.ToString() + "'");
            if (dsgst.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("This shift is already started");
                return;
                d = dsgst.Tables[0].Rows[0]["date"].ToString();
                id = dsgst.Tables[0].Rows[0]["shiftid"].ToString();
            }
            
           _frm.shiftend(comboBox1.SelectedValue.ToString(), "Shift Start", date,vTextBox1.Text);
           _frm.TopMost = true;
            this.Close();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            _frm.TopMost = true;
            this.Close();
        }

        private void vTextBox1_TextChanged(object sender, EventArgs e)
        {
            if (vTextBox1.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(vTextBox1.Text.ToString(), out Num); //c is your variable
                if (isNum)
                {

                }
                else
                {

                    MessageBox.Show("Invalid Value. Only Nymbers are allowed");
                    vTextBox1.Focus();
                    return;
                }
            }
        }
        public static int strt = 0;
        private void vButton3_Click_1(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            vTextBox1.Text = vTextBox1.Text + btn.Text;
            vTextBox1.Focus();
            vTextBox1.SelectionStart = vTextBox1.Text.Length;
            strt = vTextBox1.SelectionStart;
        }

        private void vButton12_Click(object sender, EventArgs e)
        {
            try
            {
                if (strt > 0)
                {
                    int index = vTextBox1.SelectionStart;

                    vTextBox1.Text = vTextBox1.Text.Remove(strt - 1, 1);
                    // txtcashreceived.Select(index - 1, 1);
                    //txtcashreceived.Select();
                    strt = strt - 1;
                    vTextBox1.Focus();
                    vTextBox1.SelectionStart = vTextBox1.Text.Length;
                    strt = vTextBox1.SelectionStart;
                    //txtcashreceived.Focus(); 
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
