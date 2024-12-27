using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace POSRestaurant.classes
{
    class pra
    {
        public static string getkey()
        {
            string keyy = "";
            using (var webClient = new System.Net.WebClient())
            {
                var getKeyUrl = "http://dev.bapps.pitb.gov.pk/RIMS/api/databaseupdate/formkey";
                var json = webClient.DownloadString(getKeyUrl);
                JObject o = JObject.Parse(json);
                //var results = JsonConvert.DeserializeObject<dynamic>(json);
                keyy = o["key"].ToString();                                
            }
            return keyy;
        }
        public void savedata()
        {
            string URI = "http://dev.bapps.pitb.gov.pk/RIMS/api/databaseupdate/formdata";
            string myParameters = "key=" + getkey() + "&data={\"pntn\":\"25251490\",\"branchcode\":\"BR25251490496\",\"invoice_number\":\"431-01\",\"invoice_date\": \"2015-08-04\",\"invoice_time\": \"02:41\",\"discount_percent\": \"2\",\"service_charges_percent\": \"5\",\"tax_percent\":\"16\",\"table_no\": \"1\",\"phone\": \"042456456\",\"customer_name\":\"Sani\",\"detail\":[{\"item_code\":\"34\",\"item\":\"Piza\", \"quantity\":\"1\",\"unit_price\":\"725\"}]}";

            using (WebClient wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                string HtmlResult = wc.UploadString(URI, myParameters);
                //txt_postData.Text = HtmlResult;
            }
   
        }
    }
    
}
