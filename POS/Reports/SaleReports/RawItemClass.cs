using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Reports.SaleReports
{
   public class RawItemClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double conversionrate { get; set; }
    }
}
