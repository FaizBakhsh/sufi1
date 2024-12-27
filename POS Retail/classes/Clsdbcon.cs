using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
//using POSRetail.dbsources;

namespace POSRetail.classes
{
    public class Clsdbcon
    {
        public static SqlConnection con;
        public static string Database;
        public static string Password;
        public static string Server;
        public static string User;
        public static string constring = "";
        private SqlConnection connection;
        public static POSRetail.forms.frmdbconnector Connector = new POSRetail.forms.frmdbconnector();

        public Clsdbcon()
        {
            this.connection = new SqlConnection();
            this.connection.ConnectionString = this.getConnectionString();
        }

        public string getConnectionString()
        {
            return POSRetail.Properties.Settings.Default.ConnectionString;
        }
        public SqlDataReader funGetDataReader1( string tbl)
        {
            SqlDataReader dr;
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(tbl, this.connection);
                dr = com.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public SqlDataReader funGetDataReader(string query)
        {
            SqlDataReader dr;
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(query, this.connection);
                dr = com.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        public string authentication(string form )
        {
            DataSet ds = new DataSet();
            string val = "";
            try
            {
                
               string user = POSRetail.Properties.Settings.Default.UserId.ToString();
               string q = "SELECT     dbo.Rights.Status, dbo.Forms.Forms, dbo.Rights.Userid FROM         dbo.Rights INNER JOIN                      dbo.Forms ON dbo.Rights.formid = dbo.Forms.Id where  dbo.Rights.Userid ='" + user + "' and dbo.Forms.Forms='"+form+"'";
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(q, this.connection);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    val = ds.Tables[0].Rows[0]["Status"].ToString(); ;
                }
                else
                {
                    val = "no";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.connection.Close();
            }
            return val;
        }
        public DataSet funGetDataSet(string query)
        {
            DataSet ds = new DataSet();
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(query, this.connection);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(ds);
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                this.connection.Close();
            }
            return ds;
        }
        public void executeQuery(string query)
        {
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
                this.connection.Open();
                SqlCommand com = new SqlCommand(query, this.connection);
               int rows= com.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                this.connection.Close();
            }
        }
        public string getUserId()
        {
            return POSRetail.Properties.Settings.Default.UserId;
        }

        public bool getUserRight(int formId, string colName)
        {
            bool right = false;
            try
            {
                SqlDataReader dr = this.funGetDataReader("Select " + colName + " from UserRights Where Id = (Select UserId from Stafflogin Where UserId = '" + this.getUserId() + "') And FormId = " + formId);
                if (dr.HasRows)
                {
                    dr.Read();
                    
                }
                right = true;// Convert.ToBoolean(dr.GetValue(0));
                dr.Close();
                this.closeConnection();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return right;
        }

        public void closeConnection()
        {
            try
            {
                if (this.connection.State == ConnectionState.Open)
                    this.connection.Close();
            }
            catch (Exception ex)
            {
            }
        }
        public static Clsdbcon GetDB()
        {
            if (clsado == null)
            {
                clsado = new Clsdbcon();
                return clsado;
            }

            return clsado;
        }
        public static Clsdbcon clsado;
        //public dsRecord dtset = new dsRecord();
        //public POSRetail.dbsources.dsRecordTableAdapters.stockposTableAdapter brgyAdapter = new POSRetail.dbsources.dsRecordTableAdapters.stockposTableAdapter();
        ////public Monitoring.Database.VincentDBTableAdapters.Purok_tblTableAdapter purokAdapter = new Monitoring.Database.VincentDBTableAdapters.Purok_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.HouseHold_tblTableAdapter houseAdapter = new Monitoring.Database.VincentDBTableAdapters.HouseHold_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.Residents_tblTableAdapter residentAdapter = new Monitoring.Database.VincentDBTableAdapters.Residents_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.Vehicles_tblTableAdapter vehicleAdapter = new Monitoring.Database.VincentDBTableAdapters.Vehicles_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.StaffTableAdapter staffAdapter = new Monitoring.Database.VincentDBTableAdapters.StaffTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.TableAdapterManager ad = new Monitoring.Database.VincentDBTableAdapters.TableAdapterManager();

        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByPurokTableAdapter showResByPurok = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByPurokTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByHH_IDTableAdapter showResByHHID = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByHH_IDTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByHHTableAdapter showByHH = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByHHTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.ShowResidentByYearTableAdapter showByYear = new Monitoring.Database.VincentDBTableAdapters.ShowResidentByYearTableAdapter();

        //public Monitoring.Database.VincentDBTableAdapters.GetAgeBracketTableAdapter agebracket = new Monitoring.Database.VincentDBTableAdapters.GetAgeBracketTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.GetEmploymentByPurokTableAdapter employmentadapter = new Monitoring.Database.VincentDBTableAdapters.GetEmploymentByPurokTableAdapter();

        //public Monitoring.Database.VincentDBTableAdapters.Appliances_tblTableAdapter appliancesadapter = new Monitoring.Database.VincentDBTableAdapters.Appliances_tblTableAdapter();
        //public Monitoring.Database.VincentDBTableAdapters.Age_tblTableAdapter age = new Monitoring.Database.VincentDBTableAdapters.Age_tblTableAdapter();


        //THIS ARE THE DATASET THAT CAN BE USED WITHIN THIS MODULE;
	}
}
