using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.classes
{
   public class Class1
    {
       public static string list()
       {
           string path = Path.GetDirectoryName(Application.ExecutablePath);
           string file = path+@"\file.txt";
           string text = File.ReadAllText(file);
           return text;
       }

    }
}
