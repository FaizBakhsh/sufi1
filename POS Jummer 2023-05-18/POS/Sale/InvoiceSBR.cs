using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Sale
{
   public class InvoiceSBR
   {
       public int posId { get; set; }
       public string name { get; set; }
       public string ntn { get; set; }

       public string invoiceDateTime { get; set; }
       public string invoiceType { get; set; }
       public string invoiceId   { get; set; }
       public double rateValue { get; set; }
       public double saleValue { get; set; }
       public double taxAmount { get; set; }
       public string consumerName { get; set; }
       public string consumerNTN { get; set; }
       public string address { get; set; }
       public string tariffCode { get; set; }
       public string extraInf { get; set; }
       public string TransType { get; set; }
      
    }
}
