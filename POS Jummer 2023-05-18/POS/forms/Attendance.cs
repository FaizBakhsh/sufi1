using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;

namespace POSRestaurant.forms
{
    public partial class Attendance : Form
    {
        FilterInfoCollection filterinfocollection;
        VideoCaptureDevice videodevice;
        public Attendance()
        {
            InitializeComponent();
        }

        private void Attendance_Load(object sender, EventArgs e)
        {
            filterinfocollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo filterinfo in filterinfocollection)
            {
                cmbcamera.Items.Add(filterinfo.Name);
            }
            cmbcamera.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            videodevice=new VideoCaptureDevice(filterinfocollection[cmbcamera.SelectedIndex].MonikerString);
            videodevice.NewFrame += CaptureDevice_NewFrame;
            videodevice.Start();
            timer1.Start();
        }
        protected void CaptureDevice_NewFrame(object sender, AForge.Video.NewFrameEventArgs e)
        {
            pictureBox1.Image = (Bitmap)e.Frame.Clone();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (pictureBox1.Image != null)
                {
                    //BarcodeReader reader = new BarcodeReader();
                    //Result result = reader.Decode((Bitmap)pictureBox1.Image);
                    //if (result != null)
                    //{
                    //    textBox1.Text = result.ToString();
                    //    if (videodevice.IsRunning)
                    //    {
                    //        timer1.Stop();
                    //        videodevice.Stop();
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void Attendance_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videodevice.IsRunning)
            {
                timer1.Stop();
                videodevice.Stop();
            }
        }
    }
}
