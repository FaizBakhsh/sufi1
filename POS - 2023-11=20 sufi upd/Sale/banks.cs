﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class banks : Form
    {
        RestSale _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public banks(RestSale frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        public string paymenttype = "",gsttype="";
        private void banks_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            AddDisplayControls();
           DataSet ds = objCore.funGetDataSet("select * from Banks");
           if (ds.Tables[0].Rows.Count > 0)
           {
               for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
               {
                   Button btn = new Button();
                   btn.Text = ds.Tables[0].Rows[i]["name"].ToString();
                   btn.Name = ds.Tables[0].Rows[i]["id"].ToString();
                   btn.BackColor = Color.Gray;
                   btn.Font = new Font("", 12, FontStyle.Bold);
                   btn.ForeColor = Color.Yellow;
                   btn.Click += new EventHandler(button_Click);
                   Addbutton(btn);
               }
           }
        }
        int tcolms = 0;
        int trows = 0;
        public string saleid = "0", cashreceived = "0", remaining = "0", name = "", type = "", total = "0", phone = "";
        public string checkbilltype(string id)
        {
            string check = "";
            DataSet dss = new DataSet();
            try
            {
                string q = "select * from BillType where saleid='" + id + "'";
                dss = objCore.funGetDataSet(q);
                if (dss.Tables[0].Rows.Count > 0)
                {
                    check = "yes";
                }
            }
            catch (Exception ex)
            {
                check = "error";
            }
            finally
            {
                dss.Dispose();
            }
            return check;
        }
        protected void button_Click(object sender, EventArgs e)
        {
            Button bt = sender as Button;
            if (paymenttype == "partial")
            {
                _frm.billtype(saleid.ToString(), "Cash", total.ToString().Trim(), "0");
                _frm.billtype(saleid.ToString(), "Visa " + bt.Text, remaining.Trim().Replace("-", ""), "0");
                _frm.pay("Cash,Visa " + bt.Text, cashreceived, "0", name, type,phone,gsttype);
                if (checkbilltype(saleid.ToString()) != "yes")
                {
                    _frm.billtype(saleid.ToString(), "Cash", total.ToString().Trim(), "0");
                    _frm.billtype(saleid.ToString(), "Visa " + bt.Text, remaining.Trim().Replace("-", ""), "0");
                }
            }
            else
            {
                _frm.pay("Visa " + bt.Text, cashreceived, remaining, name, type, phone, gsttype);
                _frm.billtype(saleid.ToString(), "Visa " + bt.Text, total.ToString().Trim(), "0");
                if (checkbilltype(saleid.ToString()) != "yes")
                {
                    _frm.billtype(saleid.ToString(), "Visa " + bt.Text, total.ToString().Trim(), "0");
                }
            }
            this.Enabled = true;
           // _frm.TopMost = true;
            this.Close();
        }
        private void Addbutton(Button btn)
        {
            
            try
            {
                btn.Dock = DockStyle.Fill;
                tableLayoutPanel2.Controls.Add(btn, tcolms, trows);
                tcolms++;                
                if (tcolms >= tableLayoutPanel2.ColumnCount)
                {
                    tcolms = 0;
                    trows++;
                }
            }
            catch (Exception ex)
            {
            }
            
        }
        private void AddDisplayControls()
        {
            try
            {
                tableLayoutPanel2.Controls.Clear();
                //Clear out the existing row and column styles
                tableLayoutPanel2.ColumnStyles.Clear();
                tableLayoutPanel2.RowStyles.Clear();
                DataSet ds = new DataSet();
                int rowsize = 0;
                try
                {
                    ds = objCore.funGetDataSet("select * from Banks");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        tableLayoutPanel2.ColumnCount = Convert.ToInt32(ds.Tables[0].Rows.Count);                        
                    }
                    else
                    {
                        //Assign table no of rows and column
                        tableLayoutPanel2.ColumnCount = 2;
                        
                    }
                }
                catch (Exception ex)
                {
                }
                //Assign table no of rows and column            
                float cperc = 100 / tableLayoutPanel2.ColumnCount;
                float rperc = 100 / tableLayoutPanel2.RowCount;
                //tableLayoutPanelmenugroup.Height = Convert.ToInt32(rowsize * tableLayoutPanelmenugroup.RowCount);
                for (int i = 0; i < tableLayoutPanel2.ColumnCount; i++)
                {
                    tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));
                    for (int j = 0; j < tableLayoutPanel2.RowCount; j++)
                    {
                        if (i == 0)
                        {
                            //defining the size of cell
                            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
                        }
                    }
                }
                tableLayoutPanel2.HorizontalScroll.Enabled = false;
                
            }
            catch (Exception ex)
            {

            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //_frm.TopMost = true;
            this.Close();
        }
     
    }
}