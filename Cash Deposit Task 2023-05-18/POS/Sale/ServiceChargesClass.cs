using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Sale
{
   public class ServiceChargesClass
    {
        public int Id { get; set; }
        public string charges { get; set; }
        public string OrderType { get; set; }
        public string GstType { get; set; }
        public string Title { get; set; }
        
    }
}
