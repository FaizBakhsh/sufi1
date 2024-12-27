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
    public partial class CinemaCustomerDisplay : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public CinemaCustomerDisplay()
        {
            InitializeComponent();
        }

        private void CinemaCustomerDisplay_Load(object sender, EventArgs e)
        {
            try
            {
                Screen[] sc;
                sc = Screen.AllScreens;


                this.StartPosition = FormStartPosition.Manual;
                //   this.Location = new Point(sc[1].Bounds.Left, sc[1].Bounds.Top);
                this.Location = Screen.AllScreens[0].WorkingArea.Location;
                // If you intend the form to be maximized, change it to normal then maximized.
                this.WindowState = FormWindowState.Normal;
                this.WindowState = FormWindowState.Maximized;
            }
            catch (Exception ex)
            {

                MessageBox.Show("Customer Display not attached");
            }
        }
       
        public void calchk(string selectedmoviename, string selectedshow)
        {
            
           
            
            //Thread FirstThread = new Thread(() => checkseatstatus(tblA,selectedmoviename,selectedshow));
            //FirstThread.Start();
            //Thread FirstThread1 = new Thread(() => checkseatstatus(tblB, selectedmoviename, selectedshow));
            //FirstThread1.Start();
            //Thread FirstThread2 = new Thread(() => checkseatstatus(tblC, selectedmoviename, selectedshow));
            //FirstThread2.Start();
            //Thread FirstThread3 = new Thread(() => checkseatstatus(tblD, selectedmoviename, selectedshow));
            //FirstThread3.Start();
            //Thread FirstThread4 = new Thread(() => checkseatstatus(tblE, selectedmoviename, selectedshow));
            //FirstThread4.Start();
            //Thread FirstThread5 = new Thread(() => checkseatstatus(tblF, selectedmoviename, selectedshow));
            //FirstThread5.Start();
            //Thread FirstThread6 = new Thread(() => checkseatstatus(tblG, selectedmoviename, selectedshow));
            //FirstThread6.Start();
            //Thread FirstThread7 = new Thread(() => checkseatstatus(tblH, selectedmoviename, selectedshow));
            //FirstThread7.Start();
            //Thread FirstThread8 = new Thread(() => checkseatstatus(tblI, selectedmoviename, selectedshow));
            //FirstThread8.Start();
            //Thread FirstThread9 = new Thread(() => checkseatstatus(tblJ, selectedmoviename, selectedshow));
            //FirstThread9.Start();
            checkseatstatus(tblA, selectedmoviename, selectedshow);
            checkseatstatus(tblB, selectedmoviename, selectedshow);
            checkseatstatus(tblC, selectedmoviename, selectedshow);
            checkseatstatus(tblD, selectedmoviename, selectedshow);
            checkseatstatus(tblE, selectedmoviename, selectedshow);
            checkseatstatus(tblF, selectedmoviename, selectedshow);
            checkseatstatus(tblG, selectedmoviename, selectedshow);
            checkseatstatus(tblH, selectedmoviename, selectedshow);
            checkseatstatus(tblI, selectedmoviename, selectedshow);
            checkseatstatus(tblJ, selectedmoviename, selectedshow);
        }
        public void checkseatstatus(TableLayoutPanel tbl, string selectedmoviename, string selectedshow)
        {
            foreach (Control ctr in tbl.Controls)
            {
                if (ctr is vButton)
                {
                    string table = tbl.Name.ToString().Replace("tbl", "");
                    vButton abtn = ctr as vButton;
                    DataSet dschk = new DataSet();
                    string q = "SELECT     id, SeatNo, Status, Moviename, Showname FROM         MovieSeatReservation where Moviename='" + selectedmoviename + "' and Showname='" + selectedshow + "' and dbo.MovieSeatReservation.SeatNo='" + table + " " + abtn.Text.ToString() + "'";
                    dschk = objCore.funGetDataSet(q);
                    if (dschk.Tables[0].Rows.Count > 0)
                    {
                        if (dschk.Tables[0].Rows[0]["Status"].ToString() == "Reserved")
                        {
                            
                            //abtn.Invoke(new MethodInvoker(delegate
                            //{
                            //    // Executes the following code on the GUI thread.
                            //    abtn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                            //}));
                            if (abtn.Text == "")
                            { }
                            else
                            {
                                abtn.VIBlendTheme = VIBLEND_THEME.METROORANGE;
                            }
                            //abtn.Text = "changed";
                        }
                        else

                            if (dschk.Tables[0].Rows[0]["Status"].ToString() == "Booked")
                            {
                                //abtn.Invoke(new MethodInvoker(delegate
                                //{
                                //    // Executes the following code on the GUI thread.
                                //    abtn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                                //}));
                                if (abtn.Text == "")
                                { }
                                else
                                {
                                    abtn.VIBlendTheme = VIBLEND_THEME.METROBLUE;
                                }
                            }
                            else
                            {
                                //abtn.Invoke(new MethodInvoker(delegate
                                //{
                                //    // Executes the following code on the GUI thread.
                                //    abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                                //}));
                                if (abtn.Text == "")
                                { }
                                else
                                {
                                    abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                                }
                            }

                    }
                    else
                    {
                        //abtn.Invoke(new MethodInvoker(delegate
                        //{
                        //    // Executes the following code on the GUI thread.
                        //    abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                        //}));
                        if (abtn.Text == "")
                        { }
                        else
                        {
                            abtn.VIBlendTheme = VIBLEND_THEME.METROGREEN;
                        }
                    }


                }
            }

        }
    }
}
