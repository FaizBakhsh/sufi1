using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class testprint : Form
    {
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;
       public DataTable dt;
        public testprint()
        {
            InitializeComponent();
        }
        public void bindreport()
        {
            try
            {
                // if (dtcopy.Rows.Count > 0)
                {

                    //if (dsprint.Tables[0].Rows.Count > 0)
                    {

                        POSRestaurant.Reports.CashReceipt rptDoc = new Reports.CashReceipt();
                        POSRestaurant.Reports.DsCashReceipt dsrpt = new Reports.DsCashReceipt();
                        dsrpt.Clear();

                        
                        dsrpt.Tables[0].Merge(dt);

                        rptDoc.SetDataSource(dsrpt);

                        
                        {
                            rptDoc.SetParameterValue("cardno", "");
                        }

                        ////if (txtcashrecvd.Text == "")
                        //{
                        //    cash = txtnettotal.Text;
                        //}
                        //else
                        //{
                        //    cash = txtcashrecvd.Text;
                        //}
                        //if (txtchange.Text == "")
                        //{
                        //    change = "0";
                        //}
                        //else
                        //{
                        //    change = txtchange.Text;
                        //}
                        
                        string msg = "";
                        try
                        {
                            msg = "";// dscompany.Tables[0].Rows[0]["WellComeNote"].ToString();
                        }
                        catch (Exception ex)
                        {

                        }
                        
                        {

                            rptDoc.SetParameterValue("table", "");
                        }

                        string title = "";// gsttitle;
                        if (title == "")
                        {
                            title = "Sales Tax";
                        }
                        rptDoc.SetParameterValue("gst", title);
                        rptDoc.SetParameterValue("gstamount", "12");

                        
                        rptDoc.SetParameterValue("discount", "0");
                        rptDoc.SetParameterValue("discountamount", "0");
                        rptDoc.SetParameterValue("subtotal", "0");
                        rptDoc.SetParameterValue("nettotal", "0");


                        rptDoc.SetParameterValue("cash", "0");
                        rptDoc.SetParameterValue("change", "0");
                        rptDoc.SetParameterValue("message", msg);
                        rptDoc.SetParameterValue("msg1", "0");
                        rptDoc.SetParameterValue("qrcodetext", "ssss");

                       
                        {
                            rptDoc.SetParameterValue("delivery", "");
                        }
                        crystalReportViewer1.ReportSource = rptDoc;
                        rptDoc.Dispose();
                        System.GC.Collect();
                        
                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
       
        private void testprint_Load(object sender, EventArgs e)
        {
            Printt();
           // bindreport();
        }
      
        public void Printt()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
                DataSet dscompany = new DataSet();
                dscompany = objcore.funGetDataSet("select * from companyinfo");

                DataTable dt = new DataTable();
                dt.Columns.Add("name", typeof(string));
                dt.Columns.Add("qty", typeof(double));
                dt.Columns.Add("Price", typeof(double));
                dt.Columns.Add("logo", typeof(byte[]));
                string q = "SELECT top 10       dbo.MenuItem.Name+' '+dbo.MenuItem.Name+' '+dbo.MenuItem.Name as name, dbo.Saledetails.Quantity AS qty, dbo.Saledetails.Price FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id";
                DataSet dsd = new DataSet();

                dsd = objcore.funGetDataSet(q);
                for (int i = 0; i < dsd.Tables[0].Rows.Count; i++)
                {
                    dt.Rows.Add(dsd.Tables[0].Rows[i][0].ToString(), dsd.Tables[0].Rows[i][1].ToString(), dsd.Tables[0].Rows[i][2].ToString(), dscompany.Tables[0].Rows[0]["logo"]);
                }
                dt = dsd.Tables[0];
                LocalReport report = new LocalReport();

                string path = Path.GetDirectoryName(Application.ExecutablePath);
                string fullpath = Path.GetDirectoryName(Application.ExecutablePath).Remove(path.Length - 10) + @"\Sale\rpttest.rdlc";
                report.ReportPath = fullpath;
                report.DataSources.Add(new ReportDataSource("dstest", dt));

                report.EnableExternalImages = true;
                report.EnableHyperlinks = true;

                string pathimage = Path.GetFullPath("D:ftechlogo.jpg");
                ReportParameter rurl = new ReportParameter("url", new Uri(pathimage).AbsoluteUri);

                report.SetParameters(new ReportParameter[] { rurl });
                var pageSettings = new PageSettings();
                pageSettings.PaperSize = report.GetDefaultPageSettings().PaperSize;
                pageSettings.Landscape = report.GetDefaultPageSettings().IsLandscape;
                pageSettings.Margins = report.GetDefaultPageSettings().Margins;
                Printr(report, pageSettings);
            }
            catch (Exception ex)
            {
                
               
            }
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void button1_Click(object sender, EventArgs e)
        {


            


            Printt();
            return;
            DataTable dt = new DataTable();
            string q = "SELECT top 15       dbo.MenuItem.Name, dbo.Saledetails.Quantity AS qty, dbo.Saledetails.Price FROM            dbo.Saledetails INNER JOIN                         dbo.MenuItem ON dbo.Saledetails.MenuItemId = dbo.MenuItem.Id";
            DataSet dsd = new DataSet();
            dsd = objcore.funGetDataSet(q);
            dt = dsd.Tables[0];
            LocalReport report = new LocalReport();
            string path = Path.GetDirectoryName(Application.ExecutablePath);
            string fullpath = Path.GetDirectoryName(Application.ExecutablePath).Remove(path.Length-10)+@"\Sale\rpttest.rdlc";
            report.ReportPath = fullpath;
            report.DataSources.Add(new ReportDataSource("dstest", dt));

            var pageSettings = new PageSettings();
            pageSettings.PaperSize = report.GetDefaultPageSettings().PaperSize;
            pageSettings.Landscape = report.GetDefaultPageSettings().IsLandscape;
            pageSettings.Margins = report.GetDefaultPageSettings().Margins;


            //report.SetParameters(parameters);
            PrintToPrinter(report,pageSettings);
        }
        public void Printr(LocalReport report, PageSettings pageSettings)
        {
            try
            {
                string deviceInfo = @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>{pageSettings.PaperSize.Width * 100}in</PageWidth>
                <PageHeight>{pageSettings.PaperSize.Height * 100}in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>{pageSettings.Margins.Left * 100}in</MarginLeft>
                <MarginRight>{pageSettings.Margins.Right * 100}in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";

                //            string deviceInfo = @"<DeviceInfo>
                //                <OutputFormat>EMF</OutputFormat>
                //                <PageWidth>3in</PageWidth>
                //               <PageHeight>3in</PageHeight>
                //                <MarginTop>0in</MarginTop>
                //                <MarginLeft>0in</MarginLeft>
                //                <MarginRight>0in</MarginRight>
                //                <MarginBottom>0in</MarginBottom>
                //            </DeviceInfo>";

                Warning[] warnings;
                var streams = new List<Stream>();
                var currentPageIndex = 0;

                report.Render("Image", deviceInfo,
                    (name, fileNameExtension, encoding, mimeType, willSeek) =>
                    {
                        var stream = new MemoryStream();
                        streams.Add(stream);
                        return stream;
                    }, out warnings);

                foreach (Stream stream in streams)
                    stream.Position = 0;

                if (streams == null || streams.Count == 0)
                    throw new Exception("Error: no stream to print.");

                var printDocument = new PrintDocument();

                printDocument.DefaultPageSettings = pageSettings;

                if (!printDocument.PrinterSettings.IsValid)
                    throw new Exception("Error: cannot find the default printer.");
                else
                {
                    printDocument.PrintPage += (sender, e) =>
                    {
                        Metafile pageImage = new Metafile(streams[currentPageIndex]);
                        Rectangle adjustedRect = new Rectangle(
                            e.PageBounds.Left - (int)e.PageSettings.HardMarginX,
                            e.PageBounds.Top - (int)e.PageSettings.HardMarginY,
                            e.PageBounds.Width,
                            e.PageBounds.Height);
                        // e.Graphics.FillRectangle(Brushes.White, adjustedRect);
                        e.Graphics.DrawImage(pageImage, adjustedRect);
                        currentPageIndex++;
                        e.HasMorePages = (currentPageIndex < streams.Count);
                        //e.Graphics.DrawRectangle(Pens.Red, adjustedRect);
                    };
                    printDocument.EndPrint += (Sender, e) =>
                    {
                        if (streams != null)
                        {
                            foreach (Stream stream in streams)
                                stream.Close();
                            streams = null;
                        }
                    };
                    currentPageIndex = 0;
                    printDocument.Print();
                }
            }
            catch (Exception ex)
            {
            }
        }
        public static void PrintToPrinter(LocalReport report, PageSettings pageSettings)
        {
            Export(report, pageSettings);

        }

        public static void Export(LocalReport report, PageSettings pageSettings, bool print = true)
        {
            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>3in</PageWidth>
               <PageHeight>3.3in</PageHeight>
                <MarginTop>0in</MarginTop>
                <MarginLeft>0in</MarginLeft>
                <MarginRight>0in</MarginRight>
                <MarginBottom>0in</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                Print(pageSettings);
            }
        }


        public static void Print( PageSettings pageSettings)
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();


            printDoc.DefaultPageSettings = pageSettings;
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }
    }
}
