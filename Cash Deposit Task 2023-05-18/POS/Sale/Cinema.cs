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
using System.IO;
using System.Threading;
namespace POSRestaurant.Sale
{
    public partial class Cinema : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        CinemaCustomerDisplay obcustomerdisplay;
        public Cinema()
        {
            InitializeComponent();
        }
        private void AddDisplayControls()
        {
           //// tblseats.Controls.Clear();

           // //Clear out the existing row and column styles
           // tblseats.ColumnStyles.Clear();
           // tblseats.RowStyles.Clear();
           // ds = new DataSet();
           // ds = objCore.funGetDataSet("select * from Tablelayout where tablename='seats'");
           // if (ds.Tables[0].Rows.Count > 0)
           // {
           //     tblseats.ColumnCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Columns"].ToString());
           //     tblseats.RowCount = Convert.ToInt32(ds.Tables[0].Rows[0]["Rows"].ToString());
           // }
           // else
           // {


           //     //Assign table no of rows and column
           //     tblseats.ColumnCount = 5;
           //     tblseats.RowCount = 4;

           // }
           // //Assign table no of rows and column

           // float cperc = 100 / tblseats.ColumnCount;
           // float rperc = 100 / tblseats.RowCount;

           // for (int i = 0; i < tblseats.ColumnCount; i++)
           // {
           //     tblseats.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cperc));

           //     for (int j = 0; j < tblseats.RowCount; j++)
           //     {
           //         if (i == 0)
           //         {
           //             //defining the size of cell
           //             tblseats.RowStyles.Add(new RowStyle(SizeType.Percent, rperc));
           //         }


           //     }
           // }
            // tableLayoutPanelmenugroup.HorizontalScroll.Enabled = false;
          

        }
        int tcolms = 0, trows = 0;
        public void changtext(Button btn, string text, string color, string img, string fontsize, string fontcolor)
        {

            try
            {
                btn.Text = text;
                btn.Text = text.Replace("&", "&&");
                // btn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                // string path = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                //string path = Application.StartupPath + "\\Resources\\ButtonIcons\\";
                btn.Font = new Font("", 12, FontStyle.Bold);
                if (fontcolor != string.Empty)
                {
                    btn.ForeColor = Color.FromArgb(Convert.ToInt32(fontcolor));
                }
                if (fontsize != string.Empty)
                {
                    btn.Font = new Font("", Convert.ToInt32(fontsize), FontStyle.Bold);
                }
                //if (img != string.Empty)
                //{
                //    btn.Image = Image.FromFile(path + img);
                //    btn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                //}
                //else
                //{
                //    btn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                //}
                if (color == string.Empty)
                {
                    btn.BackColor = Color.Transparent;
                }
                else
                {
                    btn.BackColor = Color.FromArgb(Convert.ToInt32(color));
                    //if (color.ToLower() == "-16777216")
                    //{
                    //    btn.ForeColor = Color.White;
                    //}
                    //if (color.ToLower() == "-1")
                    //{
                    //    btn.ForeColor = Color.Black;
                    //}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void getmovies()
        {

            objCore = new classes.Clsdbcon();
            ds = new DataSet();
            string q = "SELECT     id, name, status,img FROM         Movies where status='active' ";
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    // if (i == 0)
                    {
                        vButton button = new vButton();
                        button.VIBlendTheme = VIBLEND_THEME.NERO;// "BLACKPEARL";
                        button.Click += new EventHandler(vButton1_Click);
                        button.TextWrap = true;
                        // button.FlatStyle = FlatStyle.Standard;
                        byte[] MyData = new byte[0];


                        try
                        {
                            DataRow myRow;
                            myRow = ds.Tables[0].Rows[i];

                            MyData = (byte[])myRow["img"];

                            MemoryStream stream = new MemoryStream(MyData);
                            button.Image = Image.FromStream(stream);
                            button.ImageAlign = ContentAlignment.MiddleLeft;
                            button.TextImageRelation = TextImageRelation.ImageBeforeText;
                            button.TextAlign = ContentAlignment.MiddleRight;
                        }
                        catch (Exception ex)
                        {
                            
                           
                        }
                       
                        changtext(button, ds.Tables[0].Rows[i]["Name"].ToString(), "", "", "12", "");
                        Addbutton(button);
                        //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }
                    



                }
            }
        }
        private void Addbutton(vButton btn)
        {
            //// panel7.SuspendLayout();
            btn.Dock = DockStyle.Fill;


            tblmovies.Controls.Add(btn, tcolms, trows);
            tcolms++;
            //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            if (tcolms >= tblmovies.ColumnCount)
            {
                tcolms = 0;
                trows++;
            }
            // panel7.ResumeLayout(false);
        }
        public void checkseatstatus( TableLayoutPanel tbl )
        {
            foreach (Control ctr in tbl.Controls)
            {
                if (ctr is vButton)
                {
                    string table = tbl.Name.ToString().Replace("tbl","");
                    vButton abtn = ctr as vButton;
                    DataSet dschk = new DataSet();
                    
                    string q = "SELECT     id, SeatNo, Status, Moviename, Showname FROM         MovieSeatReservation WITH (NOLOCK) where Moviename='" + selectedmoviename + "' and Showname='" + selectedshow + "' and dbo.MovieSeatReservation.SeatNo='" + table + " " + abtn.Text.ToString() + "'";
                    if (table.ToLower()=="a")
                    {
                        dschk = objCore.funGetDataSet(q);
                        if (dschk.Tables[0].Rows.Count > 0)
                        {
                            if (dschk.Tables[0].Rows[0]["Status"].ToString() == "Reserved")
                            {
                                abtn.Invoke(new MethodInvoker(delegate
                                {
                                    // Executes the following code on the GUI thread.
                                    abtn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                                }));
                                //abtn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                            }
                            else

                                if (dschk.Tables[0].Rows[0]["Status"].ToString() == "Booked")
                                {
                                    abtn.Invoke(new MethodInvoker(delegate
                                    {
                                        // Executes the following code on the GUI thread.
                                        abtn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                                    }));
                                    //abtn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                                }
                                else
                                {
                                    abtn.Invoke(new MethodInvoker(delegate
                                    {
                                        // Executes the following code on the GUI thread.
                                        abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                                    }));
                                    //abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                                }

                        }
                        else
                        {
                            abtn.Invoke(new MethodInvoker(delegate
                            {
                                // Executes the following code on the GUI thread.
                                abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                            }));
                            //abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                        } 
                    }
                    
                    
                }
            }
            
        }
        public void checkseatstatus1(TableLayoutPanel tbl)
        {
            foreach (Control ctr in tbl.Controls)
            {
                if (ctr is vButton)
                {
                    string table = tbl.Name.ToString().Replace("tbl", "");
                    vButton abtn = ctr as vButton;
                   
                    DataSet dschk1 = new DataSet();
                   
                    string q = "SELECT     id, SeatNo, Status, Moviename, Showname FROM         MovieSeatReservation WITH (NOLOCK) where Moviename='" + selectedmoviename + "' and Showname='" + selectedshow + "' and dbo.MovieSeatReservation.SeatNo='" + table + " " + abtn.Text.ToString() + "'";
                   
                    if (table.ToLower() == "b")
                    {
                        dschk1 = objCore.funGetDataSet(q);
                        if (dschk1.Tables[0].Rows.Count > 0)
                        {
                            if (dschk1.Tables[0].Rows[0]["Status"].ToString() == "Reserved")
                            {
                                abtn.Invoke(new MethodInvoker(delegate
                                {
                                    // Executes the following code on the GUI thread.
                                    abtn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                                }));
                                //abtn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                            }
                            else

                                if (dschk1.Tables[0].Rows[0]["Status"].ToString() == "Booked")
                                {
                                    abtn.Invoke(new MethodInvoker(delegate
                                    {
                                        // Executes the following code on the GUI thread.
                                        abtn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                                    }));
                                    //abtn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                                }
                                else
                                {
                                    abtn.Invoke(new MethodInvoker(delegate
                                    {
                                        // Executes the following code on the GUI thread.
                                        abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                                    }));
                                    //abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                                }

                        }
                        else
                        {
                            abtn.Invoke(new MethodInvoker(delegate
                            {
                                // Executes the following code on the GUI thread.
                                abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                            }));
                            //abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                        }
                    }

                }
            }

        }
        public void getmoviesseats(string screenid)
        {
            tblseats.Controls.Clear();
            setastcolms = 0; seatstrows = 0;
            objCore = new classes.Clsdbcon();
            DataSet dsSeats = new DataSet();
            string q = "SELECT     *  FROM         MovieScreens where Id='" + screenid + "'";
            dsSeats = objCore.funGetDataSet(q);
            if (dsSeats.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < Convert.ToInt32(dsSeats.Tables[0].Rows[0]["Seats"].ToString()); i++)
                {
                    // if (i == 0)
                    {
                        vButton button = new vButton();
                        button.VIBlendTheme = VIBLEND_THEME.NERO;// "BLACKPEARL";
                        button.Click += new EventHandler(btnseats_Click);
                        button.TextWrap = true;
                        // button.FlatStyle = FlatStyle.Standard;
                        DataSet dschk = new DataSet();
                        q = "SELECT     id, SeatNo, Status, Moviename, Showname FROM         MovieSeatReservation where Moviename='" + selectedmoviename + "' and Showname='" + selectedshow + "' and dbo.MovieSeatReservation.SeatNo='" + (i + 1).ToString() + "'";
                        dschk = objCore.funGetDataSet(q);
                        if (dschk.Tables[0].Rows.Count > 0)
                        {
                            if (dschk.Tables[0].Rows[0]["Status"].ToString() == "Reserved")
                            {
                                button.VIBlendTheme = VIBLEND_THEME.ECOGREEN;
                            }
                        }
                        changtext(button, (i+1).ToString(), "", "", "12", "");
                        Addbuttonseats(button);
                        //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }




                }
            }
        }
        int setastcolms = 0, seatstrows=0;
        private void Addbuttonseats(vButton btn)
        {
            //// panel7.SuspendLayout();
            //btn.Dock = DockStyle.Fill;


            //tblseats.Controls.Add(btn, setastcolms, seatstrows);
            //setastcolms++;
            ////tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            //if (setastcolms >= tblseats.ColumnCount)
            //{
            //    setastcolms = 0;
            //    seatstrows++;
            //}
            // panel7.ResumeLayout(false);
        }
        string selectedmoviename = "",selectedshow="", selectedscreen="";
        public void getmoviesshows(string movie)
        {
            tblshows.Controls.Clear();
            showcolms = 0; showrows = 0;
            objCore = new classes.Clsdbcon();
            DataSet dsShows = new DataSet();
            string q = "SELECT     dbo.Movies.name, dbo.Movies.id, dbo.MovieScreens.ScreensName, dbo.MovieScreens.Id AS screenid, dbo.MovieShows.Name AS showname, dbo.MovieScreens.Seats,                       dbo.MovieAvailableShows.ShowId FROM         dbo.Movies INNER JOIN                      dbo.MovieAvailableShows ON dbo.Movies.id = dbo.MovieAvailableShows.MovieId INNER JOIN                      dbo.MovieScreens ON dbo.MovieAvailableShows.ScreenId = dbo.MovieScreens.Id INNER JOIN                      dbo.MovieShows ON dbo.MovieAvailableShows.ShowId = dbo.MovieShows.id where dbo.Movies.name='" + movie + "'";
            dsShows = objCore.funGetDataSet(q);
            if (dsShows.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsShows.Tables[0].Rows.Count; i++)
                {
                    // if (i == 0)
                    {
                        vButton button = new vButton();
                        button.VIBlendTheme = VIBLEND_THEME.NERO;// "BLACKPEARL";
                        button.Click += new EventHandler(vButton2_Click);
                        button.TextWrap = true;
                        // button.FlatStyle = FlatStyle.Standard;
                       
                        changtext(button, dsShows.Tables[0].Rows[i]["showname"].ToString(), "", "", "12", "");
                        Addbuttonshows(button);
                        //getsubmenuitem(ds.Tables[0].Rows[i]["id"].ToString());
                    }




                }
            }
        }
        int showcolms = 0, showrows = 0;
        private void Addbuttonshows(vButton btn)
        {
            //// panel7.SuspendLayout();
            btn.Dock = DockStyle.Fill;


            tblshows.Controls.Add(btn, showcolms, showrows);
            showcolms++;
            //tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
            if (setastcolms >= tblshows.ColumnCount)
            {
                showcolms = 0;
                showrows++;
            }
            // panel7.ResumeLayout(false);
        }
        DataTable dtgrid = new DataTable();
        private void Cinema_Load(object sender, EventArgs e)
        {
            //int vertScrollWidth = SystemInformation.VerticalScrollBarWidth;
            try
            {
                obcustomerdisplay = new CinemaCustomerDisplay();

                Screen[] sc;
                sc = Screen.AllScreens;
                obcustomerdisplay.Show();
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Customer Display not attached");
            }

            //tblseats.Padding = new Padding(0, 0, vertScrollWidth, 0);
            if (this.tblseats.HorizontalScroll.Visible)
            {
                int newWid =  SystemInformation.VerticalScrollBarWidth;
                //this.TableLayoutPanel1.Padding = new Padding(0, 0, newWid, 0);
                foreach (Control ctl in this.tblseats.Controls)
                {
                    ctl.Width = newWid;
                }
            }
            AddDisplayControls();
           // getmoviesseats();
            getmovies();
            dtgrid.Columns.Add("SeatNo", typeof(string));
            dtgrid.Columns.Add("Amount", typeof(double));
            dataGridView1.DataSource = dtgrid;
        }
        public void getshows(string name, string showname)
        {
            DataSet dsmovies = new DataSet();
            string q = "SELECT     dbo.Movies.name, dbo.Movies.id, dbo.MovieScreens.ScreensName,dbo.MovieScreens.Seats, dbo.MovieScreens.Id AS screenid, dbo.MovieShows.Name AS showname,dbo.MovieAvailableShows.price FROM         dbo.Movies INNER JOIN                      dbo.MovieAvailableShows ON dbo.Movies.id = dbo.MovieAvailableShows.MovieId INNER JOIN                      dbo.MovieScreens ON dbo.MovieAvailableShows.ScreenId = dbo.MovieScreens.Id INNER JOIN                      dbo.MovieShows ON dbo.MovieAvailableShows.ShowId = dbo.MovieShows.id where dbo.Movies.name='" + name + "' and dbo.MovieShows.Name='" + showname + "'";
            dsmovies = objCore.funGetDataSet(q);
            if (dsmovies.Tables[0].Rows.Count > 0)
            {
                //getmoviesseats(dsmovies.Tables[0].Rows[0]["screenid"].ToString());
                selectedscreen = dsmovies.Tables[0].Rows[0]["ScreensName"].ToString();
                string prc = dsmovies.Tables[0].Rows[0]["price"].ToString();
                if (prc == "")
                {
                    prc = "0";
                }
                price = Convert.ToDouble(prc);
            }
        }
        DataSet dscompany = new DataSet();
        public void getcompany()
        {
            try
            {

                dscompany = objCore.funGetDataSet("select * from CompanyInfo");
                byte[] MyData = new byte[0];


                DataRow myRow;
                myRow = dscompany.Tables[0].Rows[0];

                MyData = (byte[])myRow["logo"];

                MemoryStream stream = new MemoryStream(MyData);
                //With the code below, you are in fact converting the byte array of image
                //to the real image.
                //pictureBox1.Image = Image.FromStream(stream);
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
           
            foreach (Control ctr in tblmovies.Controls)
            {
                if (ctr is vButton)
                {
                    vButton abtn = ctr as vButton;
                    abtn.VIBlendTheme = VIBLEND_THEME.NERO;
                }
            }
            vButton btn=sender as vButton;
            getmoviesshows(btn.Text);
            selectedmoviename = btn.Text;
            btn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
            
        }
        private void vButton2_Click(object sender, EventArgs e)
        {
            foreach (Control ctr in tblshows.Controls)
            {
                if (ctr is vButton)
                {
                    vButton abtn = ctr as vButton;
                    abtn.VIBlendTheme = VIBLEND_THEME.NERO;
                }
            }
            
            vButton btn = sender as vButton;
            //btn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
            selectedshow = btn.Text;
            getshows(selectedmoviename, btn.Text);
            callchk();
            //checkseatstatus(tblA);
            //checkseatstatus(tblB);
            //checkseatstatus(tblC);
            //checkseatstatus(tblD);
            //checkseatstatus(tblE);
            //checkseatstatus(tblF);
            //checkseatstatus(tblG);
            //checkseatstatus(tblH);
            //checkseatstatus(tblI);
            //checkseatstatus(tblJ);
            obcustomerdisplay.calchk(selectedmoviename, selectedshow);
            

        }
        public void callchk()
        {
            //checkseatstatus(tblA);
            //checkseatstatus(tblB);
            //checkseatstatus(tblC);
            //checkseatstatus(tblD);
            //checkseatstatus(tblE);
            //checkseatstatus(tblF);
            //checkseatstatus(tblG);
            //checkseatstatus(tblH);
            //checkseatstatus(tblI);
            //checkseatstatus(tblJ);

            Thread FirstThread = new Thread(() => checkseatstatus(tblA));
            FirstThread.Start();
            Thread FirstThread1 = new Thread(() => checkseatstatus(tblB));
            FirstThread1.Start();
            //Thread FirstThread2 = new Thread(() => checkseatstatus(tblC));
            //FirstThread2.Start();
            //Thread FirstThread3 = new Thread(() => checkseatstatus(tblD));
            //FirstThread3.Start();
            //Thread FirstThread4 = new Thread(() => checkseatstatus(tblE));
            //FirstThread4.Start();
            //Thread FirstThread5 = new Thread(() => checkseatstatus(tblF));
            //FirstThread5.Start();
            //Thread FirstThread6 = new Thread(() => checkseatstatus(tblG));
            //FirstThread6.Start();
            //Thread FirstThread7 = new Thread(() => checkseatstatus(tblH));
            //FirstThread7.Start();
            //Thread FirstThread8 = new Thread(() => checkseatstatus(tblI));
            //FirstThread8.Start();
            //Thread FirstThread9 = new Thread(() => checkseatstatus(tblJ));
            //FirstThread9.Start();
        }
        private void btnseats_Click(object sender, EventArgs e)
        {
            vButton btn = sender as vButton;
            string q = "insert into MovieSeatReservation (SeatNo,Status,Moviename,Showname ) values('" + btn.Text + "','Reserved','" + selectedmoviename + "','" + selectedshow + "')";
            objCore.executeQuery(q);
            btn.VIBlendTheme = VIBLEND_THEME.ECOGREEN;
            //bindreport(btn.Text,selectedscreen);
        }
        public void bindreport(string ticketno, string screen, string price)
        {

            try
            {

                DataTable dt = new DataTable();


                POSRestaurant.Reports.Cinema.rptTicket rptDoc = new Reports.Cinema.rptTicket();
                POSRestaurant.Reports.Cinema.dsTicket dsrpt = new Reports.Cinema.dsTicket();
                //feereport ds = new feereport(); // .xsd file name

                getcompany();
                // Just set the name of data table
                dt.TableName = "Crystal Report";
                dt.Columns.Add("logo", typeof(byte[]));
                //dt = getAllOrders();
                string company = "", phone = "", address = "", logo = "";
                try
                {
                    company = dscompany.Tables[0].Rows[0]["Name"].ToString();
                    phone = dscompany.Tables[0].Rows[0]["Phone"].ToString();
                    address = dscompany.Tables[0].Rows[0]["Address"].ToString();
                    logo = dscompany.Tables[0].Rows[0]["logo"].ToString();
                }
                catch (Exception ex)
                {


                }
                dt.Rows.Add(dscompany.Tables[0].Rows[0]["logo"]);
                if (dt.Rows.Count > 0)
                {
                    dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                }
                //else
                //{
                //    if (logo == "")
                //    { }
                //    else
                //    {

                //        dt.Rows.Add("", "", "", "", dscompany.Tables[0].Rows[0]["logo"]);



                //        dsrpt.Tables[0].Merge(dt, true, MissingSchemaAction.Ignore);
                //    }
                //}


                rptDoc.SetDataSource(dsrpt);
                rptDoc.SetParameterValue("Comp", company);
                rptDoc.SetParameterValue("Addrs", phone);
                rptDoc.SetParameterValue("phn", address);
                rptDoc.SetParameterValue("ticketno", ticketno);
                rptDoc.SetParameterValue("movie", selectedmoviename);
                rptDoc.SetParameterValue("show", selectedshow);
                rptDoc.SetParameterValue("time", DateTime.Now.ToString());
                rptDoc.SetParameterValue("screen", screen);
                rptDoc.SetParameterValue("price", price);
                rptDoc.PrintToPrinter(1, false, 0, 0);
                //crystalReportViewer1.ReportSource = rptDoc;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void vButton6_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        int scrolright = 0,scrolup=0;
        private void vButton5_Click(object sender, EventArgs e)
        {

            scrolright = scrolright + 60;
            tblseats.AutoScrollPosition = new Point(scrolright, scrolup);
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    vButton btn = sender as vButton;
                    string q = "insert into MovieSeatReservation (SeatNo,Status,Moviename,Showname ) values('" + dr.Cells[0].Value.ToString() + "','Booked','" + selectedmoviename + "','" + selectedshow + "')";
                    objCore.executeQuery(q);
                    
                   // bindreport(dr.Cells[0].Value.ToString(), selectedscreen, "Price Rs:" + dr.Cells[1].Value.ToString());
                }
            }
            catch (Exception ex)
            {


            }
            MessageBox.Show("Seat Booked Successfully");
            dtgrid.Rows.Clear();
            callchk();
            //checkseatstatus(tblA);
            //checkseatstatus(tblB);
            //checkseatstatus(tblC);
            //checkseatstatus(tblD);
            //checkseatstatus(tblE);
            //checkseatstatus(tblF);
            //checkseatstatus(tblG);
            //checkseatstatus(tblH);
            //checkseatstatus(tblI);
            //checkseatstatus(tblJ);

            obcustomerdisplay.calchk(selectedmoviename, selectedshow);
           
            //if (scrolright == 0)
            //{
            //}
            //else
            //{
            //    scrolright = scrolright - 60;
            //}
            
            //tblseats.AutoScrollPosition = new Point(scrolright, scrolup);
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            scrolup = scrolup + 60;
            tblseats.AutoScrollPosition = new Point(scrolright, scrolup);
        }

        private void vButton2_Click_1(object sender, EventArgs e)
        {
            if (scrolup == 0)
            {
            }
            else
            {
                scrolup = scrolup - 60;
            }
            tblseats.AutoScrollPosition = new Point(scrolright, scrolup);
        }

        private void vButton7_Click(object sender, EventArgs e)
        {
           
        }
        double price = 0;
        private void vButton7_Click_1(object sender, EventArgs e)
        {
            vButton btn = (sender) as vButton;

            string name = btn.Parent.Name;
            name = name.Replace("tbl","");
            name = name + " " + btn.Text;
            dtgrid.Rows.Add(name, price);

        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow dr in dataGridView1.Rows)
                {
                    vButton btn = sender as vButton;
                    string q = "insert into MovieSeatReservation (SeatNo,Status,Moviename,Showname ) values('" + dr.Cells[0].Value.ToString() + "','Reserved','" + selectedmoviename + "','" + selectedshow + "')";
                    objCore.executeQuery(q);
                    btn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                    bindreport(dr.Cells[0].Value.ToString(), selectedscreen, "Price Rs:" + dr.Cells[1].Value.ToString());
                }
            }
            catch (Exception ex)
            {
                
                
            }
            dtgrid.Rows.Clear();
            callchk();
            //checkseatstatus(tblA);
            //checkseatstatus(tblB);
            //checkseatstatus(tblC);
            //checkseatstatus(tblD);
            //checkseatstatus(tblE);
            //checkseatstatus(tblF);
            //checkseatstatus(tblG);
            //checkseatstatus(tblH);
            //checkseatstatus(tblI);
            //checkseatstatus(tblJ);

            obcustomerdisplay.calchk( selectedmoviename, selectedshow);
            
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {

                {
                    string Id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    DataRow dgr = dtgrid.Rows[e.RowIndex];
                    if (dr.Cells[0].Value.ToString() == Id)
                    {
                        dgr.Delete();
                    }
                }
            }
        }
    }
}