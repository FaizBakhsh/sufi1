using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Windows.Forms;

namespace POSRestaurant.classes
{
    class clsVariables
    {

        #region User
        public static string username, fullname, usertype;
        #endregion

        #region CustomerPayment
        public static string CustPayID, FK_CustID;
        public static string FP, AccountName, CheckNo, AccountNo, BankName, Remarks, RCU,RMU;
        public static double Amount;
        public static bool Cleared;
        public static DateTime CustPayDate, DateDue, DateIssued, RC, RM;
        #endregion

        #region Items
        public static string RentalItems,RentalItemID;
        public static bool sStatus;
        public static DateTime sDateRented, sDueDate, sDateReturn;
        public static int sQuantity;
        public static double sTotalAmount,sRentPrice;
        #endregion

        #region Rental MaxHold
        public static int maxhold,LateFee,DefaultDays;
        #endregion

        #region EmailLogin
        public static string EmailUsername, EmailPassword,EmailProvider;
        public static int EmailPort;

        public static string ToEmail, CcEmail;

        public static bool LoginStat = false;
        #endregion

        public static string sMessageBox = "Inventory Management System ver. 1.0";
        public static string sUsername;
        public static string sUserFullname;
        public static string sUserLogin;
        public static string sUserType;
        public static string sServer;
        public static string sDatabase;
        public static string sDBUserID;
        public static string sDBPassword;
        public static string sCompanyName;
        public static string sContactName;
        public static string sCompanyAddress;
        public static string sPhoneNumber;
        public static string sFaxNumber;
        public static string sEmailAddress;
        public static string sWebAddress;
        public clsVariables()
        {

        }

       
    }
}
