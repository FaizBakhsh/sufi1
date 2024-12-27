using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Speech.Synthesis;
namespace POSRestaurant.Sale
{
    public partial class MainKds : Form
    {
        string lbl1 = "",lbl2 = "",lbl3 = "",lbl4 = "",lbl5 = "",lbl6 = "",lbl7 = "",lbl8 = "",lbl9 = "",lbl10 = "";
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        DataTable dt;
        public MainKds()
        {
            InitializeComponent();
        }

        private void MainKds_Load(object sender, EventArgs e)
        {
            dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("Sr#", typeof(string));           
            dt.Columns.Add("ItemName", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            fill();
            getcompany();
        }
        public void fill()
        {
            bool chk = true;
            DataTable dt1 = new DataTable();
            dt = getdata(1);
            dt1.Merge(dt);
            dataGridView1.DataSource = dt1;
            dataGridView1.Columns[0].Visible = false;
            
            if (dt1.Rows.Count > 0)
            {
                vLabel1.Text = "Order No:" + dt1.Rows[0]["id"].ToString();
                
                if (lbl1 == "" || vLabel1.Text == lbl1)
                {
                    lbl1 = vLabel1.Text;
                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl1.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    
                    // Asynchronous
                   // synthesizer.SpeakAsync("Hello World");
                }
                lbl1 = vLabel1.Text;
                
            }
            else
            {
                vLabel1.Text = "";
            }
            DataTable dt2 = new DataTable();
            dt = getdata(2);
            dt2.Merge(dt);
            dataGridView2.DataSource = dt2;
            dataGridView2.Columns[0].Visible = false;
            if (dt2.Rows.Count > 0)
            {
                vLabel2.Text = "Order No:" + dt2.Rows[0]["id"].ToString();
                if (lbl2 == "" || vLabel2.Text == lbl2)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl2.Replace("Order No:", "") + " is Complete");
                        
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl2 = vLabel2.Text;
            }
            else
            {
                vLabel2.Text = "";
            }
            DataTable dt3 = new DataTable();
            dt = getdata(3);
            dt3.Merge(dt);
            dataGridView3.DataSource = dt3;
            dataGridView3.Columns[0].Visible = false;
            if (dt3.Rows.Count > 0)
            {
                vLabel3.Text = "Order No:" + dt3.Rows[0]["id"].ToString();
                if (lbl3 == "" || vLabel3.Text == lbl3)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {
 
                        }
                        synthesizer.Speak("Order Number " + lbl3.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl3 = vLabel3.Text;
            }
            else
            {
                vLabel3.Text = "";
            }
            DataTable dt4 = new DataTable();
            dt = getdata(4);
            dt4.Merge(dt);
            dataGridView4.DataSource = dt4;
            dataGridView4.Columns[0].Visible = false;
            if (dt4.Rows.Count > 0)
            {
                vLabel4.Text = "Order No:" + dt4.Rows[0]["id"].ToString();
                if (lbl4 == "" || vLabel4.Text == lbl4)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl4.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl4 = vLabel4.Text;
            }
            else
            {
                vLabel4.Text = "";
            }
            DataTable dt5 = new DataTable();
            dt = getdata(5);
            dt5.Merge(dt);
            dataGridView5.DataSource = dt5;
            dataGridView5.Columns[0].Visible = false;
            if (dt5.Rows.Count > 0)
            {
                vLabel5.Text = "Order No:" + dt5.Rows[0]["id"].ToString();
                if (lbl5 == "" || vLabel5.Text == lbl5)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl5.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl5 = vLabel5.Text;
            }
            else
            {
                vLabel5.Text = "";
            }

            DataTable dt6 = new DataTable();
            dt = getdata(6);
            dt6.Merge(dt);
            dataGridView6.DataSource = dt6;
            dataGridView6.Columns[0].Visible = false;
            if (dt6.Rows.Count > 0)
            {
                vLabel6.Text = "Order No:" + dt6.Rows[0]["id"].ToString();
                if (lbl6 == "" || vLabel6.Text == lbl6)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl6.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl6 = vLabel6.Text;
            }
            else
            {
                vLabel6.Text = "";
            }
            DataTable dt7 = new DataTable();
            dt = getdata(7);
            dt7.Merge(dt);
            dataGridView7.DataSource = dt7;
            dataGridView7.Columns[0].Visible = false;
            if (dt7.Rows.Count > 0)
            {
                vLabel7.Text = "Order No:" + dt7.Rows[0]["id"].ToString();
                if (lbl7 == "" || vLabel7.Text == lbl7)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl7.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl7 = vLabel7.Text;
            }
            else
            {
                vLabel7.Text = "";
            }
            DataTable dt8 = new DataTable();
            dt = getdata(8);
            dt8.Merge(dt);
            dataGridView8.DataSource = dt8;
            dataGridView8.Columns[0].Visible = false;
            if (dt8.Rows.Count > 0)
            {
                vLabel8.Text = "Order No:" + dt8.Rows[0]["id"].ToString();
                if (lbl8 == "" || vLabel8.Text == lbl8)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl8.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl8 = vLabel8.Text;
            }
            else
            {
                vLabel8.Text = "";
            }
            DataTable dt9 = new DataTable();
            dt = getdata(9);
            dt9.Merge(dt);
            dataGridView9.DataSource = dt9;
            dataGridView9.Columns[0].Visible = false;
            if (dt9.Rows.Count > 0)
            {
                vLabel9.Text = "Order No:" + dt9.Rows[0]["id"].ToString();
                if (lbl9 == "" || vLabel9.Text == lbl9)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl9.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl9 = vLabel9.Text;
            }
            else
            {
                vLabel9.Text = "";
            }
            DataTable dt10 = new DataTable();
            dt = getdata(10);
            dt10.Merge(dt);
            dataGridView10.DataSource = dt10;
            dataGridView10.Columns[0].Visible = false;
            if (dt10.Rows.Count > 0)
            {
                vLabel10.Text = "Order No:" + dt10.Rows[0]["id"].ToString();
                if (lbl10 == "" || vLabel10.Text == lbl10)
                {

                }
                else
                {
                    SpeechSynthesizer synthesizer = new SpeechSynthesizer();
                    synthesizer.Volume = 100;  // 0...100
                    synthesizer.Rate = -3;     // -10...10
                    synthesizer.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Teen);
                    // Synchronous
                    if (chk == true)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        for (int i = 0; i < 10; i++)
                        {

                        }
                        synthesizer.Speak("Order Number " + lbl10.Replace("Order No:", "") + " is Complete");
                    }
                    chk = false;
                    // Asynchronous
                    // synthesizer.SpeakAsync("Hello World");
                }
                lbl10 = vLabel10.Text;
            }
            else
            {
                vLabel10.Text = "";
            }
        }
        public DataTable getdata(int rowno)
        {
            //dtrpt = new DataTable();
            dt.Clear();
            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet dsgetid = new DataSet();
            string id = "";
            string qry = "";// "SELECT *  FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id asc) AS RowNum, * FROM VSaledetails where  ) sub WHERE RowNum = '" + rowno + "'";
            qry = "SELECT *  FROM (SELECT ROW_NUMBER() OVER (ORDER BY Id asc) AS RowNum, *  FROM VSaledetails ) sub";// WHERE  KDSId='" + kds + "' ";
            dsgetid = objCore.funGetDataSet(qry);
            if (dsgetid.Tables[0].Rows.Count > 0)
            {
                if (dsgetid.Tables[0].Rows.Count >= rowno + 1)
                {
                    id = dsgetid.Tables[0].Rows[rowno]["id"].ToString();
                }
                else
                {
                    return dt;
                }

            }
            else
            {
                return dt;
            }
            DataSet ds = new DataSet();
            string q = "SELECT     dbo.Saledetails.saleid, dbo.MenuItem.Name, dbo.Saledetails.Orderstatus AS Status FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id where dbo.Saledetails.saleid='"+id+"'";
            ds = objCore.funGetDataSet(q);
            //int i=1;
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++ )
            {

                dt.Rows.Add(ds.Tables[0].Rows[i]["saleid"].ToString(), (i + 1).ToString(), ds.Tables[0].Rows[i]["Name"].ToString(), ds.Tables[0].Rows[i]["Status"].ToString());
            }
            return dt;

        }

        public void bindgrid()
        {
            DataTable dtt = new DataTable();
            dtt.Merge(dt);
            string q = "SELECT     dbo.Saledetails.saleid, dbo.MenuItem.Name, dbo.Saledetails.Orderstatus FROM         dbo.Saledetails INNER JOIN                      dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id order by dbo.Saledetails.saleid";
            dataGridView1.DataSource = dtt;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            fill();
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            try
            {

                dscompany = objcore.funGetDataSet("select * from CompanyInfo");
                byte[] MyData = new byte[0];


                DataRow myRow;
                myRow = dscompany.Tables[0].Rows[0];

                MyData = (byte[])myRow["logo"];

               System.IO.MemoryStream stream = new System.IO.MemoryStream(MyData);
                //With the code below, you are in fact converting the byte array of image
                //to the real image.
                pictureBox1.Image = Image.FromStream(stream);
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Are you sure to logout?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void vLabel1_TextChanged(object sender, EventArgs e)
        {
           
        }
    }
}
