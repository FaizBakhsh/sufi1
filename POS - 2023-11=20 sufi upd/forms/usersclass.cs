using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace POSRestaurant.forms
{

  public  class usersclass
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Usertype { get; set; }
        public string CardNo { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int branchid { get; set; }
        public string kdsid { get; set; }
        public string role { get; set; }
        public string terminal { get; set; }
        public string discountlimit { get; set; }
        public string KDSType { get; set; }
    }
}
