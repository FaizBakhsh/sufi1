using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using POSRetail;
namespace POSRestaurant.forms
{
    public partial class Form1 : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        int tcolms = 0;
        int trows = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //AddDisplayControls();
            //getmenuitem();
        }
        public void changtext(Button btn, string text, string color)
        {
            btn.Text = text;
            btn.Text = text.Replace("&", "&&");
            if (color == string.Empty)
            {
                btn.BackColor = Color.Black;
            }
            else
            {
                btn.BackColor = Color.FromArgb(Convert.ToInt32(color));
                if (color.ToLower() == "black")
                {
                    btn.ForeColor = Color.White;
                }
                if (color.ToLower() == "white")
                {
                    btn.ForeColor = Color.Black;
                }
            }
        }


        protected void button_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            // identify which button was clicked and perform necessary actions
        }
        int tlpRowCount = 0;
        private void Addbutton(Button btn)
        {

            btn.Dock = DockStyle.Fill;


            tableLayoutPanel1.Controls.Add(btn, tcolms, trows);
            tcolms++;
            //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            if (tcolms >= tableLayoutPanel1.ColumnCount)
            {
                tcolms = 0;
                trows++;
            }
        }
        private void AddDisplayControls()
        {
            tableLayoutPanel1.Controls.Clear();
            //Clear out the existing row and column styles
            tableLayoutPanel1.ColumnStyles.Clear();
            tableLayoutPanel1.RowStyles.Clear();

            //Assign table no of rows and column
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.RowCount = 4;
           
            for (int i = 0; i < tableLayoutPanel1.ColumnCount; i++)
            {
                tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));

                for (int j = 0; j < tableLayoutPanel1.RowCount; j++)
                {
                    if (i == 0)
                    {
                        //defining the size of cell
                        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent,16));
                    }
                   

                    Label lblBook = new Label();
                    lblBook.Text = "ddd";
                    //switch (i)
                    //{
                    //    case 0:
                    //        tableLayoutPanel1.Controls.Add(lblBook, i, j);
                    //        break;
                    //    case 1:
                    //        tableLayoutPanel1.Controls.Add(lblBook, i, j);
                    //        break;
                    //    case 2:
                    //        tableLayoutPanel1.Controls.Add(lblBook, i, j);
                    //        break;
                    //    case 3:
                    //        tableLayoutPanel1.Controls.Add(lblBook, i, j);
                    //        break;
                    //}
                }
            }
           
        }
        public void getmenuitem()
        {

          POSRestaurant.classes.Clsdbcon  objCore = new classes.Clsdbcon();
           DataSet ds = new DataSet();
            string q = "SELECT     dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.MenuGroup.Description, dbo.Color.ColorName FROM         dbo.MenuGroup LEFT OUTER JOIN                      dbo.Color ON dbo.MenuGroup.ColorId = dbo.Color.Id where dbo.MenuGroup.Status='Active'  order by dbo.MenuGroup.id asc";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                Point newLoc = new Point(5, 5);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   // if (i == 0)
                    {
                        Button button = new Button();
                        button.Click += new EventHandler(button_Click);
                        
                        changtext(button, ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["ColorName"].ToString());
                        
                        Addbutton(button);
                       // this.Controls.Add(button);
                        //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }
                }
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
          // string er= e.KeyChar.ToString();
        }
    }
}
