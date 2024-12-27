using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Reports.Inventory
{
   public class CriticalInventoryClass
    {
        public int ItemId { get; set; }
        public double  quantity { get; set; }
        
    }
   public class OpeningCriticalInventoryClass
   {
       public int ItemId { get; set; }
       public double quantity { get; set; }
       public DateTime Date { get; set; }
   }
}
