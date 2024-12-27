using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace POSRestaurant
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
       {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            POSRestaurant.Sale.RestSale obj = new Sale.RestSale();
            Application.Run(new forms.NewLogIn(obj));
            //Application.Run(new forms.Attendance());
        }
    }
}
