using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using CrystalDecisions.CrystalReports.Engine;

namespace POSRestaurant
{
    class loadCompanyInfo
    {
        public void fillCompanyInfo(ReportClass rpt)
        {
            try
            {
                classes.Clsdbcon core = new POSRestaurant.classes.Clsdbcon();
                SqlDataAdapter da = new SqlDataAdapter("Select Top(1) CompanyName, PhoneNo, Email, Web, Address, Logo from BusinessInfo", core.getConnectionString());
                dsCompanyInfo dsCompanyInfo1 = new dsCompanyInfo();
                dsCompanyInfo1.EnforceConstraints = false;
                da.Fill(dsCompanyInfo1.BusinessInfo);
                dsCompanyInfo1.BusinessInfo.Rows[0]["CompanyName"] = classes.CompanyInfo.CompanyName;
                dsCompanyInfo1.BusinessInfo.Rows[0]["PhoneNo"] = classes.CompanyInfo.PhoneNo;
                dsCompanyInfo1.BusinessInfo.Rows[0]["Address"] = classes.CompanyInfo.Address;
                rpt.Subreports["srHeader"].SetDataSource(dsCompanyInfo1);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message, "Error", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }
        }
    }
}
