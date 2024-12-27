using System;
using System.Collections.Generic;
using System.Text;

namespace POSRestaurant.classes
{
    public class clsUserSecurity
    {
        static clsUserSecurity sec;
        private bool isadmin=false;
        private int userid=0;

        private clsUserSecurity()
        {
        }

        static public clsUserSecurity GetUser()
        {
            if (sec == null)
            {
                sec = new clsUserSecurity();
            }

            return sec;
        }

        public bool IsAdmin
        {
            get { return isadmin; }
            set { isadmin = value; }
        }

        public int UserID
        {
            get { return userid; }
            set { userid = value; }
        }
    }
}
