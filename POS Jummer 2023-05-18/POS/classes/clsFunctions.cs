using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace POSRestaurant.classes
{
    class clsFunctions
    {
        public static string username,fullname,usertype;

        #region fillCombo
        public static void fillCombo(ComboBox cmb, string sSQL)
        {
            SqlCommand Com = new SqlCommand(sSQL, Clsdbcon.con);
            SqlDataReader reader = Com.ExecuteReader();
            cmb.Items.Clear();
            while (reader.Read())
            {
                cmb.Items.AddRange(new object[] { reader[0].ToString() });
            }
            reader.Close();
        }
        #endregion

        public clsFunctions()
        { 
        
        }

    }
}
