using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class AddMovieShows : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        public AddMovieShows()
        {
            InitializeComponent();
        }
        public void getdata()
        {
            string q = "SELECT      dbo.MovieScreens.ScreensName, dbo.MovieShows.Name AS show, dbo.MovieShows.Timing FROM         dbo.Movies INNER JOIN                     dbo.MovieAvailableShows ON dbo.Movies.id = dbo.MovieAvailableShows.MovieId INNER JOIN                      dbo.MovieScreens ON dbo.MovieAvailableShows.ScreenId = dbo.MovieScreens.Id INNER JOIN                      dbo.MovieShows ON dbo.MovieAvailableShows.ShowId = dbo.MovieShows.id where dbo.MovieAvailableShows.MovieId='"+comboBox1.SelectedValue+"'";
            DataSet dsgetdata = new DataSet();
            dsgetdata = objCore.funGetDataSet(q);
            dataGridView1.DataSource = dsgetdata.Tables[0];
        }
        private void AddMovieShows_Load(object sender, EventArgs e)
        {
           
            DataSet ds = new DataSet();
            string q = "select * from Movies";
            ds = objCore.funGetDataSet(q);
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "Name";
            DataSet dsscreen = new DataSet();
            q = "select * from MovieScreens";
            dsscreen = objCore.funGetDataSet(q);
            comboBox2.DataSource = dsscreen.Tables[0];
            comboBox2.ValueMember = "id";
            comboBox2.DisplayMember = "ScreensName";
           
            DataSet dsshow = new DataSet();
            q = "select * from MovieShows";
            dsshow = objCore.funGetDataSet(q);
            comboBox3.DataSource = dsshow.Tables[0];
            comboBox3.ValueMember = "id";
            comboBox3.DisplayMember = "Name";
            getdata();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            DataSet dschk = new DataSet();
            string q = "select * from MovieAvailableShows where MovieId='" + comboBox1.SelectedValue + "' and ShowId='" + comboBox3.SelectedValue + "' and ScreenId='" + comboBox2.SelectedValue + "'";
            dschk = objCore.funGetDataSet(q);
            if (dschk.Tables[0].Rows.Count > 0)
            {
                MessageBox.Show("Record already exist");
                return;
            }
            int id = 0;
           DataSet dss = objCore.funGetDataSet("select max(id) as id from MovieAvailableShows");
           if (dss.Tables[0].Rows.Count > 0)
            {
                string i = dss.Tables[0].Rows[0][0].ToString();
                if (i == string.Empty)
                {
                    i = "0";
                }
                id = Convert.ToInt32(i) + 1;
            }
            else
            {
                id = 1;
            }
           q = "insert into MovieAvailableShows (id,MovieId,ShowId,ScreenId) values('"+id+"','" + comboBox1.SelectedValue + "','" + comboBox3.SelectedValue + "','" + comboBox2.SelectedValue + "')";
           objCore.executeQuery(q);
           MessageBox.Show("Record Added Successfully");
           getdata();
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
