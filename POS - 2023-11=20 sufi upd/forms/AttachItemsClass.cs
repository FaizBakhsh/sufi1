using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.forms
{
   public class AttachItemsClass
   {
       public int Id { get; set; }
       public string itemid { get; set; }
       public string FlavourId { get; set; }
       public string SubItemId { get; set; }
       public string Quantity { get; set; }
       public string Type { get; set; }
    }
}
