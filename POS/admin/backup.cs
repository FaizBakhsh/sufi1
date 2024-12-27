using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRestaurant.admin
{
    public partial class backup : Form
    {
        POSRestaurant.classes.Clsdbcon objCore;
        public backup()
        {
            InitializeComponent();
            objCore = new classes.Clsdbcon();
        }
        public void bacupdishes()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM DISHES order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from DISHES where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Name,Price,Recordtype from DISHES')select '" + dr["Id"] + "','" + dr["Name"] + "','" + dr["Price"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupsalary()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM EMPLOYEESALARY order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from EMPLOYEESALARY where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Staffid,Month,Year,Date,Paidstatus,Recordtype from EMPLOYEESALARY')select '" + dr["Id"] + "','" + dr["Staffid"] + "','" + dr["Month"] + "','" + dr["Year"] + "','" + dr["Date"] + "','" + dr["Paidstatus"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupingredients()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM INGREDIENTS order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from INGREDIENTS where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Rid,Itemid,Usedquantity,Recordtype from INGREDIENTS')select '" + dr["Id"] + "','" + dr["Rid"] + "','" + dr["Itemid"] + "','" + dr["Usedquantity"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupitems()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM dbo.ITEM order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from ITEM where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Supid,Name,Measuringunit,Unitprice,Quantity,Totalprice,Date,Recordtype from ITEM')select '" + dr["Id"] + "','" + dr["Supid"] + "','" + dr["Name"] + "','" + dr["Measuringunit"] + "','" + dr["Unitprice"] + "','" + dr["Quantity"] + "','" + dr["Totalprice"] + "','" + dr["Date"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacuporders()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM dbo.ORDERS order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from ORDERS where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Rid,Userid,Quantity,Orderdate,Deliverydate,Status,Bill,Recordtype from ORDERS')select '" + dr["Id"] + "','" + dr["Rid"] + "','" + dr["Userid"] + "','" + dr["Quantity"] + "','" + dr["Orderdate"] + "','" + dr["Deliverydate"] + "','" + dr["Status"] + "','" + dr["Bill"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupstaf()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM dbo.STAFF order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from STAFF where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Name,Phone,Designation,Department,Salary,Address,City,Recordtype from STAFF')select '" + dr["Id"] + "','" + dr["Name"] + "','" + dr["Phone"] + "','" + dr["Designation"] + "','" + dr["Department"] + "','" + dr["Salary"] + "','" + dr["Address"] + "','" + dr["City"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupstafacnts()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM dbo.STAFFACCOUNTS order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from STAFFACCOUNTS where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Staffid,Username,Password,Recordtype from STAFFACCOUNTS')select '" + dr["Id"] + "','" + dr["Staffid"] + "','" + dr["Username"] + "','" + dr["Password"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupsuplier()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM dbo.SUPPLIER order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from SUPPLIER where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Name,Gender,Nicno,Category,Email,Phone,City,Address,Recordtype from SUPPLIER')select '" + dr["Id"] + "','" + dr["Name"] + "','" + dr["Gender"] + "','" + dr["Nicno"] + "','" + dr["Category"] + "','" + dr["Email"] + "','" + dr["Phone"] + "','" + dr["City"] + "','" + dr["Address"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupuserright()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM dbo.UserRights order by id asc");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from UserRights where Recordtype=''localServer''')");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,FormId,CanAdd,CanDelete,CanUpdate,CanView,Recordtype from UserRights')select '" + dr["Id"] + "','" + dr["FormId"] + "','" + dr["CanAdd"] + "','" + dr["CanDelete"] + "','" + dr["CanUpdate"] + "','" + dr["CanView"] + "','LocalServer'");

                }


                this.objCore.closeConnection();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult msg = MessageBox.Show("Are you sure you want to create backup this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msg == DialogResult.Yes)
            {
                try
                {
                   
                    DataSet ds = this.objCore.funGetDataSet("SELECT * FROM CUSTOMERS order by id asc");
                    DataTable dt = new DataTable();
                    dt = ds.Tables[0];
                    if (dt.Rows.Count>0)
                    {
                        this.objCore.executeQuery("delete OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from CUSTOMERS where Recordtype=''localServer''')");
                        foreach(DataRow dr in dt.Rows )
                        //while (drr.Read())
                        {
                            this.objCore.executeQuery("insert OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select Id,Name,Servedquantity,Bill,Discount,Netamount,Staffid,Dishid,Date,Recordtype from CUSTOMERS')select '" + dr["Id"] + "','" + dr["Name"] + "','" + dr["Servedquantity"] + "','" + dr["Bill"] + "','" + dr["Discount"] + "','" + dr["Netamount"] + "','" + dr["Staffid"] + "','" + dr["Dishid"] + "','" + dr["Date"] + "','LocalServer'");
 
                        }

                       
                        this.objCore.closeConnection();
                    }

                    bacupdishes();
                    bacupsalary();
                    bacupingredients();
                    bacupitems();
                    bacupstaf();
                    bacupstafacnts();
                    bacupsuplier();
                    bacupuserright();
                    bacuporders();
                    MessageBox.Show("Backup Created Successfully .", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult msg = MessageBox.Show("Are you sure you want to create backup this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msg == DialogResult.Yes)
            {
                try
                {
                    bacupdishes1();
                    bacupsalary1();
                    bacupingredients1();
                    bacupitems1();
                    bacupstaf1();
                    bacupstafacnts1();
                    bacupsuplier1();
                    bacupuserright1();
                    bacuporders1();
                    MessageBox.Show("Data Restored Successfully .", "", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void bacuporders1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.ORDERS where Recordtype=''localServer'' order by id asc')");        
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from ORDERS where Recordtype='localServer'");
               
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert into ORDERS(Id,Rid,Userid,Quantity,Orderdate,Deliverydate,Status,Bill,Recordtype) values( '" + dr["Id"] + "','" + dr["Rid"] + "','" + dr["Userid"] + "','" + dr["Quantity"] + "','" + dr["Orderdate"] + "','" + dr["Deliverydate"] + "','" + dr["Status"] + "','" + dr["Bill"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupuserright1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.UserRights where Recordtype=''localServer'' order by id asc')");        
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from UserRights where Recordtype='localServer'");
                
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert into UserRights (Id,FormId,CanAdd,CanDelete,CanUpdate,CanView,Recordtype) values('" + dr["Id"] + "','" + dr["FormId"] + "','" + dr["CanAdd"] + "','" + dr["CanDelete"] + "','" + dr["CanUpdate"] + "','" + dr["CanView"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupsuplier1()
        {

            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.SUPPLIER where Recordtype=''localServer'' order by id asc')");        
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from SUPPLIER where Recordtype='localServer'");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert into SUPPLIER (Id,Name,Gender,Nicno,Category,Email,Phone,City,Address,Recordtype) values('" + dr["Id"] + "','" + dr["Name"] + "','" + dr["Gender"] + "','" + dr["Nicno"] + "','" + dr["Category"] + "','" + dr["Email"] + "','" + dr["Phone"] + "','" + dr["City"] + "','" + dr["Address"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupstafacnts1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.STAFFACCOUNTS where Recordtype=''localServer'' order by id asc')");        
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from STAFFACCOUNTS where Recordtype='localServer'");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert into STAFFACCOUNTS (Id,Staffid,Username,Password,Recordtype) values( '" + dr["Id"] + "','" + dr["Staffid"] + "','" + dr["Username"] + "','" + dr["Password"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupstaf1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.STAFF where Recordtype=''localServer'' order by id asc')");        
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from STAFF where Recordtype='localServer'");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert into STAFF(Id,Name,Phone,Designation,Department,Salary,Address,City,Recordtype) values( '" + dr["Id"] + "','" + dr["Name"] + "','" + dr["Phone"] + "','" + dr["Designation"] + "','" + dr["Department"] + "','" + dr["Salary"] + "','" + dr["Address"] + "','" + dr["City"] + "','LocalServer')");


                }


                this.objCore.closeConnection();
            }
        }
        public void bacupitems1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.ITEM where Recordtype=''localServer'' order by id asc')");         
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from ITEM where Recordtype='localServer'");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert into ITEM( Id,Supid,Name,Measuringunit,Unitprice,Quantity,Totalprice,Date,Recordtype)values('" + dr["Id"] + "','" + dr["Supid"] + "','" + dr["Name"] + "','" + dr["Measuringunit"] + "','" + dr["Unitprice"] + "','" + dr["Quantity"] + "','" + dr["Totalprice"] + "','" + dr["Date"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupingredients1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.INGREDIENTS where Recordtype=''localServer'' order by id asc')");
          
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from INGREDIENTS where Recordtype='localServer'");
               
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert INGREDIENTS (Id,Rid,Itemid,Usedquantity,Recordtype)values( '" + dr["Id"] + "','" + dr["Rid"] + "','" + dr["Itemid"] + "','" + dr["Usedquantity"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupsalary1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.EMPLOYEESALARY where Recordtype=''localServer'' order by id asc')");
          
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                 this.objCore.executeQuery("delete from EMPLOYEESALARY where Recordtype='localServer'");
               
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert EMPLOYEESALARY (Id,Staffid,Month,Year,Date,Paidstatus,Recordtype) values ('" + dr["Id"] + "','" + dr["Staffid"] + "','" + dr["Month"] + "','" + dr["Year"] + "','" + dr["Date"] + "','" + dr["Paidstatus"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
        public void bacupdishes1()
        {
            DataSet ds = this.objCore.funGetDataSet("SELECT * FROM OPENQUERY([AFAQMALIK-PC\\SQLEXPRESS], 'select * from dbo.DISHES where Recordtype=''localServer'' order by id asc')");
            DataTable dt = new DataTable();
            dt = ds.Tables[0];
            if (dt.Rows.Count > 0)
            {
                this.objCore.executeQuery("delete from DISHES where Recordtype='localServer'");
                foreach (DataRow dr in dt.Rows)
                //while (drr.Read())
                {
                    this.objCore.executeQuery("insert into DISHES(Id,Name,Price,Recordtype)values( '" + dr["Id"] + "','" + dr["Name"] + "','" + dr["Price"] + "','LocalServer')");

                }


                this.objCore.closeConnection();
            }
        }
    }
}
