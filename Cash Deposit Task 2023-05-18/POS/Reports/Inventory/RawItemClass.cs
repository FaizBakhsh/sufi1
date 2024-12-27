using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.Reports.Inventory
{
    public class RawItemClass
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Price { get; set; }
        public string UOM { get; set; }
        public string MinOrder { get; set; }
        public string Cat { get; set; }


    }

    public struct RawItemStruct
    {
        public int Id ;
        public string ItemName ;
        public string Price ;
        public string UOM ;
        public string MinOrder;
        public string Cat;
        public RawItemStruct(int Id1, string ItemName1, string Price1, string UOM1, string MinOrder1, string Cat1)
        {

            this.Id = Id1;
            this.ItemName = ItemName1;
            this.Price = Price1;
            this.UOM = UOM1;
            this.MinOrder = MinOrder1;
            this.Cat = Cat1;
        }
    }


}
