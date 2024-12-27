using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class receivableentry : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public receivableentry(POSRestaurant.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }
       
        

        private void button1_Click(object sender, EventArgs e)
        {
           
               
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
           
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from ResturantStaff";
                ds = objCore.funGetDataSet(q);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "name";

                if (editmode == 1)
                {
                    objCore = new classes.Clsdbcon();
                    ds = new DataSet();
                    q = "select * from EmployeeRecvb where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtamount.Text = ds.Tables[0].Rows[0]["amount"].ToString();
                        dateTimePicker1.Text =Convert.ToDateTime( ds.Tables[0].Rows[0]["date"].ToString()).ToString("yyyy-MM-dd");
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["empid"].ToString();
                        vButton2.Text = "Update";
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try 
            {
                 if (comboBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select a valid Employee");
                    comboBox1.Focus();
                    return;
                }
                if (txtamount.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Amount");
                    txtamount.Focus();
                    return;
                }
                if (dateTimePicker1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Date");
                    dateTimePicker1.Focus();
                    return;
                }
                if (editmode == 0)
                {
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from EmployeeRecvb");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        id = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        id = 1;
                    }
                    ds = new DataSet();
                    string q = "select * from EmployeeRecvb where empid='" + comboBox1.SelectedValue + "' and date='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Data already exist");
                        return;
                    }
                    q = "insert into EmployeeRecvb (id, empid, amount, date) values('" + id + "','" + comboBox1.SelectedValue + "','" + txtamount.Text.Trim().Replace("'", "''") + "','" + dateTimePicker1.Text.Trim().Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    cashaccount(txtamount.Text.Trim(), "SJVR-" + id, dateTimePicker1.Text);
                    employeeaccount(txtamount.Text.Trim(), "SJVR-" + id, dateTimePicker1.Text,comboBox1.SelectedValue.ToString());
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                   
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "update EmployeeRecvb set empid='" + comboBox1.SelectedValue + "', amount='" + txtamount.Text.Trim().Replace("'", "''") + "' , date ='" + dateTimePicker1.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    q = "delete from EmployeeAccount where voucherno='SJVR-"+id+"'";
                    objCore.executeQuery(q);
                    q = "delete from CashAccountSales where voucherno='SJVR-" + id + "'";
                    objCore.executeQuery(q);
                    cashaccount(txtamount.Text.Trim(), "SJVR-" + id, dateTimePicker1.Text);
                    employeeaccount(txtamount.Text.Trim(), "SJVR-" + id, dateTimePicker1.Text, comboBox1.SelectedValue.ToString());
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT        dbo.EmployeeRecvb.id, dbo.ResturantStaff.Name, dbo.EmployeeRecvb.amount, dbo.EmployeeRecvb.date FROM            dbo.ResturantStaff INNER JOIN                         dbo.EmployeeRecvb ON dbo.ResturantStaff.Id = dbo.EmployeeRecvb.empid order by  dbo.ResturantStaff.Name");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void employeeaccount(string amount, string saleid, string date, string ID)
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();

                string q = "select * from ResturantStaff where ID='" + ID + "'";
                dsacount = objCore.funGetDataSet(q);
                int id = 0;
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from EmployeeAccount");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";


                    double newbalance = (balance + Convert.ToDouble(amount));

                    q = "insert into EmployeeAccount (Id,Date,EmployeeId,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + date + "','" + ID + "','" + ChartAccountId + "','" + saleid + "','Less Cash','" + Math.Round(Convert.ToDouble(amount), 2) + "','0','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void cashaccount(string amount, string saleid, string date)
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                DataSet dsacount = new DataSet();
                string q = "";
                q = "select * from CashSalesAccountsList where AccountType='Cash Account'";                        
                dsacount = objCore.funGetDataSet(q);
                int id = 0;
                if (dsacount.Tables[0].Rows.Count > 0)
                {
                    string ChartAccountId = dsacount.Tables[0].Rows[0]["ChartaccountId"].ToString();
                    int iddd = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CashAccountSales");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string i = ds.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "1";
                        }
                        iddd = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        iddd = Convert.ToInt32("1");
                    }
                    double balance = 0;
                    string val = "";
                    q = "select top 1 * from CashAccountSales where ChartAccountId='" + ChartAccountId + "' order by id desc";
                    dsacount = new DataSet();
                    dsacount = objCore.funGetDataSet(q);
                    if (dsacount.Tables[0].Rows.Count > 0)
                    {
                        val = dsacount.Tables[0].Rows[0]["balance"].ToString();
                    }
                    if (val == "")
                    {
                        val = "0";

                    }
                    balance = Convert.ToDouble(val);

                    double newbalance = (balance + Convert.ToDouble(amount));
                    q = "insert into CashAccountSales (Id,Date,ChartAccountId,VoucherNo,Description,Debit,Credit,Balance) values('" + iddd + "','" + date + "','" + ChartAccountId + "','" + saleid + "','Less Cash','0','" + Math.Round(Convert.ToDouble(amount), 2) + "','" + newbalance + "')";
                    objCore.executeQuery(q);
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {           
            txtamount.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
