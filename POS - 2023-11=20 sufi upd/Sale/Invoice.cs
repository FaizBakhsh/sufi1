using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Sale
{
   public class Invoice
    {
            public string Invoicenumber { get; set; }
            public int POSID { get; set; }
            public string USIN { get; set; }
            public string RefUSIN { get; set; }
            public DateTime DateTime { get; set; }
            public string BuyerNTN { get; set; }
            public string BuyerCNIC { get; set; }
            public string BuyerName { get; set; }
            public string BuyerPhoneNumber { get; set; }
            public int PaymentMode { get; set; }
            public double TotalSaleValue { get; set; }
            public double TotalQuantity { get; set; }
            public double TotalBillAmount { get; set; }
            public double TotalTaxCharged { get; set; }
            public double Discount { get; set; }
            public double FurtherTax { get; set; }
            public int InvoiceType { get; set; }
            public List<InvoiceItems> Items { get; set; }


    }
}
