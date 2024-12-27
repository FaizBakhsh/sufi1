﻿using System;
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
    public partial class ShiftEnd : Form
    {
        private EmployeeFunctions _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public string date = "",shiftid="";
        public ShiftEnd(EmployeeFunctions frm )
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
                    this.StartPosition = FormStartPosition.CenterScreen;
                    this.WindowState = FormWindowState.Normal;
                    //this.WindowState = FormWindowState.Maximized;

                }

            }
            catch (Exception ex)
            {

            }
            this.TopMost = true;
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
          
            if (vTextBox1.Text == "")
            {
                MessageBox.Show("Please Enter Physical Cash");
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
            //_frm.TopMost = false; 
            string id = "";
            string d = "";
            DataSet dsgst = new DataSet();
            _frm.shiftend(shiftid, "Shift End", date, vTextBox1.Text);
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
        private void vButton8_Click(object sender, EventArgs e)
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
                                                                                                                           