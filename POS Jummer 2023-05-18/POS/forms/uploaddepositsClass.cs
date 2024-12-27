using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.forms
{ 
    
   
   public class uploaddepositsClass
   {
       public string Onlineid { get; set; }
       public string date { get; set; }
       public string ActualAmount { get; set; }
       public string DepositedAmount { get; set; }
       public string branchid { get; set; }
       public byte[] Image { get; set; }
    }
}
