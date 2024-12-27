using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Sale
{
   public class InvoiceItems
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string PCTCode { get; set; }
        public double Quantity { get; set; }
        public float TaxRate { get; set; }
        public double SaleValue { get; set; }
        public double Discount { get; set; }
        public double FurtherTax { get; set; }
        public double TaxCharged { get; set; }
        public double TotalAmount { get; set; }
        public int InvoiceType { get; set; }
        public string RefUSIN { get; set; }
    }
}
