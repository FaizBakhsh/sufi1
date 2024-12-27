using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Reports.SaleReports
{
   public class RecipeClass
    {
        public int MenuItemId { get; set; }
        public int RawItemId { get; set; }
        public string ModifierId { get; set; }
        public double Quantity { get; set; }
        public string type { get; set; }

    }
}
