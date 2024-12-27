using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace POSRetail.admin
{
    public partial class Salarymanagement : Form
    {
        public POSRetail.classes.Clsdbcon objCore;
        public Salarymanagement()
        {
            InitializeComponent();
            objCore = new classes.Clsdbcon();
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            calculate();
            SearchQRY();
        }
      
        public void calculate()
        {
            try
            {
                SqlDataReader dr1;

                {
                    SqlDataReader dr2;
                    dr2 = this.objCore.funGetDataReader("SELECT * FROM STAFF");
                    
                    if (dr2.HasRows)
                    {
                        DataSet ds = this.objCore.funGetDataSet("SELECT * FROM STAFF");
                   
                        //while (dr2.Read())
                        DataTable dt = new DataTable();
                        dt = ds.Tables[0];
                        foreach(DataRow dtr in dt.Rows )
                        {
                            string stfid = dtr["Id"].ToString();// dr2["Id"].ToString();
                           
                            dr1 = this.objCore.funGetDataReader("SELECT * FROM EMPLOYEESALARY WHERE Staffid='"+stfid+"' ORDER BY Id DESC");
                            if (dr1.HasRows)
                            {
                                //while (dr1.Read())
                                dr1.Read();
                                {
                                    int cmnth = DateTime.Now.Month;
                                    int cyear = DateTime.Now.Year;
                                    int pmnth = (Convert.ToInt32(dr1["Month"].ToString()));
                                    int pyear = (Convert.ToInt32(dr1["Year"].ToString()));
                                    if ((cmnth + cyear) > (pmnth + pyear))
                                    {
                                        int id = 0;
                                        SqlDataReader sdr = objCore.funGetDataReader1("SELECT MAX(Id) AS MID FROM EMPLOYEESALARY");
                                        if (sdr.HasRows)
                                        {
                                            sdr.Read();
                                            string idw = sdr[0].ToString();
                                            if (idw == "")
                                            {
                                                id = 1;
                                            }
                                            else
                                            {
                                                id = Convert.ToInt32(sdr[0].ToString());
                                                id = id + 1;
                                            }
                                        }
                                        objCore.executeQuery("INSERT INTO EMPLOYEESALARY (Id,Staffid,Month,Year,Paidstatus) VALUES('" + id + "','" + stfid + "','" + DateTime.Now.Month + "','" + DateTime.Now.Year + "','Not Paid')");
                                    }
                                }

                            }
                            else
                            {
                                int id = 0;
                                SqlDataReader sdr = objCore.funGetDataReader1("SELECT MAX(Id) AS MID FROM EMPLOYEESALARY");
                                if (sdr.HasRows)
                                {
                                    sdr.Read();
                                    string idw = sdr[0].ToString();
                                    if (idw == "")
                                    {
                                        id = 1;
                                    }
                                    else
                                    {
                                        id = Convert.ToInt32(sdr[0].ToString());
                                        id = id + 1;
                                    }

                                }
                                objCore.executeQuery("INSERT INTO EMPLOYEESALARY (Id,Staffid,Month,Year,Paidstatus) VALUES('"+id+"','" + stfid + "','" + DateTime.Now.Month + "','" + DateTime.Now.Year + "','Not Paid')");

                            }
                        }
                    }
                    
                    //else
                    //{
                    //    SqlDataReader dr2;
                    //    dr2 = this.objCore.funGetDataReader("SELECT * FROM STAFF");
                    //    if (dr2.HasRows)
                    //    {
                    //        while (dr2.Read())
                    //        {
                    //            int id = 0;
                    //            SqlDataReader sdr = objCore.funGetDataReader1("SELECT MAX(Id) AS MID FROM EMPLOYEESALARY");
                    //            if (sdr.HasRows)
                    //            {
                    //                sdr.Read();
                    //                string idw = sdr[0].ToString();
                    //                if (idw == "")
                    //                {
                    //                    id = 0;
                    //                }
                    //                else
                    //                {
                    //                    id = Convert.ToInt32(sdr[0].ToString());
                    //                }
                                    
                    //            }
                    //            objCore.executeQuery("INSERT INTO EMPLOYEESALARY (Staffid,Month,Year,Paidstatus) VALUES('"+dr2["Id"]+"','"+DateTime.Now.Month+"','"+DateTime.Now.Year+"','Not Paid')");

                    //        }
                    //    }

                    //}
                    //dr1.Close();
                    //this.objCore.closeConnection();
                }


            }
            catch (Exception ex)
            {
            }
        }
        void SearchQRY()
        {
            try
            {
                SqlDataReader dr;
                lvList.Items.Clear();
                
                {
                    dr = this.objCore.funGetDataReader("SELECT * FROM vsalary WHERE Name Like '%" + this.txtSearch.Text.Trim() + "%' Order By Id DESC");
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            ListViewItem list = new ListViewItem(dr["Id"].ToString());
                            list.SubItems.Add(dr["Name"].ToString());
                            list.SubItems.Add(dr["Designation"].ToString());
                            list.SubItems.Add(dr["Salary"].ToString());
                            list.SubItems.Add(dr["Paidstatus"].ToString());
                            list.SubItems.Add(dr["Date"].ToString());
                            string mnth = dr["Month"].ToString();
                            string yer = dr["Year"].ToString();
                            string dat = mnth + "-" + yer;
                            list.SubItems.Add(dat);
                            lvList.Items.AddRange(new ListViewItem[] { list });
                            list.ImageIndex = 1;
                        }
                    }
                    dr.Close();
                    this.objCore.closeConnection();
                }
                
                
            }
            catch (Exception ex)
            {
            }
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            if (this.lvList.SelectedItems.Count >= 1)
            {
                int id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                objCore.executeQuery("UPDATE EMPLOYEESALARY SET Paidstatus='Paid', Date='"+DateTime.Now.ToShortDateString()+"' WHERE Id='"+id+"'");
                SearchQRY();
            }
            else
            {
                MessageBox.Show("Please select a Staff to pay.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            SearchQRY();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (this.lvList.SelectedItems.Count >= 1)
            {
                string d="";
                int id = Convert.ToInt32(this.lvList.FocusedItem.Text.Trim());
                objCore.executeQuery("UPDATE EMPLOYEESALARY SET Paidstatus='Not Paid', Date='"+d+"' WHERE Id='" + id + "'");
                SearchQRY();
            }
            else
            {
                MessageBox.Show("Please select a Staff to pay.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
