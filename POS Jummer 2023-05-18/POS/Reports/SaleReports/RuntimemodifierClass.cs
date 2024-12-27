using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Reports.SaleReports
{
   public class RuntimemodifierClass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RawItemId { get; set; }
        public double Quantity { get; set; }
        
    }
}
