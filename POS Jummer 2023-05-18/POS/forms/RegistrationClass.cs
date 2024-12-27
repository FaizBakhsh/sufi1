using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Device.Location;
using System.Data;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Net.Http;
using System.Windows;
namespace POSRestaurant.forms
{
   public class RegistrationClass
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string WellComeNote { get; set; }
        public string info { get; set; }
        public string info2 { get; set; }
        public RegistrationClass()
        {
          
       
        }
       
        public static void getlist()
        {
            try
            {
                string info = "";
                string info2 = "";
                POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
                try
                {
                    GeoCoordinateWatcher watcher;
                    watcher = new GeoCoordinateWatcher();
                    var coordinate = watcher.Position.Location;

                    info2 = coordinate.Longitude.ToString();
                    info = coordinate.Latitude.ToString();
                }
                catch (Exception ex)
                {


                }
                string q = "select * from companyinfo";
                DataSet ds = new DataSet();
                ds = objcore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    List<RegistrationClass> list = new List<RegistrationClass>();
                    list.Add(new RegistrationClass
                    {
                        Name = ds.Tables[0].Rows[0]["Name"].ToString(),
                        Address = ds.Tables[0].Rows[0]["Name"].ToString(),
                        Phone = ds.Tables[0].Rows[0]["Phone"].ToString(),
                        WellComeNote = ds.Tables[0].Rows[0]["WellComeNote"].ToString(),
                        info = info,
                        info2 = info2
                    });



                    try
                    {
                        bool chk = false;
                        string URI = classes.Class1.list();

                        string msg = "";
                        var myparametrs = JsonConvert.SerializeObject(list).ToList();
                        using (WebClient wc = new WebClient())
                        {
                            wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                            //wc.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                            string HtmlResult = wc.UploadString(URI, myparametrs.ToString());
                            msg = HtmlResult;
                            //txt_postData.Text = HtmlResult;

                        }


                    }
                    catch (Exception ex)
                    {

                    }
                }

            }
            catch (Exception ex)
            {
                
                
            }
        }
    }
}
