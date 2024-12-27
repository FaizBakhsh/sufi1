using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class BillRecallold : Form
    {
        private  RestSale _frm1;
        POSRestaurant.classes.Clsdbcon objCore ;
        DataSet ds ;
        public string date = "";
        public BillRecallold(RestSale frm1)
        {
            InitializeComponent();
            _frm1 = frm1;
            _frm1.Enabled = false;
            objCore = new classes.Clsdbcon();
        }
        //public AllowDiscount()
        //{
        //    InitializeComponent();
        //    this.editmode = 0;
        //    this.id = "";
            
        //}

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
            this.TopMost = false;
            getdata("");
        }
        protected string ridername(string id)
        {
            string name = "";
            try
            {
                string q = "select name from  ResturantStaff where id='" + id + "'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    name = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
               
            }
            return name;
        }
        protected string getcuid(string id, string type)
        {
            string cus = "";
            try
            {
                string q = "";
                if (type == "Take Away")
                {
                    q = "select CustomerId from TakeAway where saleid='" + id + "'";
                }
                if (type == "Dine In")
                {
                    q = "SELECT dbo.DinInTables.TableNo, dbo.ResturantStaff.Name FROM  dbo.DinInTables INNER JOIN                dbo.ResturantStaff ON dbo.DinInTables.WaiterId = dbo.ResturantStaff.Id where saleid='" + id + "'";
                }
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (type == "Take Away")
                    {
                        cus = ds.Tables[0].Rows[0][0].ToString(); ;
                    }
                    if (type == "Dine In")
                    {
                        cus = "Table No: " + ds.Tables[0].Rows[0][0].ToString() + " Waiter Name: " + ds.Tables[0].Rows[0][1].ToString();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return cus;
        }
        public void getdata(string word)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Bill No", typeof(string));       
            dt.Columns.Add("time", typeof(string));
            dt.Columns.Add("Net Bill", typeof(string));
            dt.Columns.Add("Order Type", typeof(string));
            dt.Columns.Add("Terminal", typeof(string));
            dt.Columns.Add("Order Name", typeof(string));
            dt.Columns.Add("Kitchen Print", typeof(string));
            dt.Columns.Add("Take Away ID", typeof(string));
             dt.Columns.Add("Table No", typeof(string));
             dt.Columns.Add("Guests", typeof(string));
             dt.Columns.Add("Waiter", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Address", typeof(string));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Rider", typeof(string));
            dt.Columns.Add("Dispatched Time", typeof(string));
            dt.Columns.Add("Delivered Time", typeof(string));
           
            try
            {
               
                DataSet ds9 = new DataSet();
                string q9 = "";
                if (_frm1.enabledordertype.Length > 0)
                {
                    if (word == "")
                    {
                        q9 = "SELECT        TOP (100) PERCENT dbo.Sale.Id AS Bill_No, dbo.Sale.OnlineId AS Online_BillNo, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.Sale.Customer, dbo.Sale.Terminal, dbo.Sale.TerminalOrder, dbo.Delivery.Name,                          dbo.Delivery.Phone, dbo.Delivery.Address, dbo.Delivery.RiderId, dbo.Delivery.Status, dbo.Delivery.Dispatchedtime, dbo.Delivery.deliveredtime FROM            dbo.Sale LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId WHERE   dbo.Sale.OrderType='" + _frm1.enabledordertype + "' and   (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "' and dbo.Sale.branchid='" + _frm1.branchid + "' ORDER BY Bill_No desc";
                        q9 = "SELECT        TOP (100) PERCENT dbo.Sale.Id AS Bill_No, dbo.Sale.OnlineId AS Online_BillNo, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.Sale.Customer, dbo.Sale.Terminal, dbo.Sale.TerminalOrder, dbo.Delivery.Name,                          dbo.Delivery.Phone, dbo.Delivery.Address, dbo.Delivery.RiderID, dbo.Delivery.Status, dbo.Delivery.DispatchedTime, dbo.Delivery.DeliveredTime, dbo.DinInTables.TableNo,dbo.DinInTables.Guests, dbo.ResturantStaff.Name AS Waiter,                           dbo.TakeAway.CustomerId AS TakeawayID FROM            dbo.ResturantStaff RIGHT OUTER JOIN                         dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId RIGHT OUTER JOIN                         dbo.Sale LEFT OUTER JOIN                         dbo.TakeAway ON dbo.Sale.Id = dbo.TakeAway.Saleid ON dbo.DinInTables.Saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId  WHERE   dbo.Sale.OrderType='" + _frm1.enabledordertype + "' and   (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "' and dbo.Sale.branchid='" + _frm1.branchid + "' ORDER BY Bill_No desc";
                    }
                    else
                    {
                        q9 = "SELECT        TOP (100) PERCENT dbo.Sale.Id AS Bill_No, dbo.Sale.OnlineId AS Online_BillNo, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.Sale.Customer, dbo.Sale.Terminal, dbo.Sale.TerminalOrder, dbo.Delivery.Name,                          dbo.Delivery.Phone, dbo.Delivery.Address, dbo.Delivery.RiderID, dbo.Delivery.Status, dbo.Delivery.DispatchedTime, dbo.Delivery.DeliveredTime, dbo.DinInTables.TableNo,dbo.DinInTables.Guests, dbo.ResturantStaff.Name AS Waiter,                           dbo.TakeAway.CustomerId AS TakeawayID FROM            dbo.ResturantStaff RIGHT OUTER JOIN                         dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId RIGHT OUTER JOIN                         dbo.Sale LEFT OUTER JOIN                         dbo.TakeAway ON dbo.Sale.Id = dbo.TakeAway.Saleid ON dbo.DinInTables.Saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId  WHERE   dbo.Sale.OrderType='" + _frm1.enabledordertype + "' and     (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and cast( dbo.Sale.id as varchar(max)) like '%" + word + "%' or  dbo.Sale.OrderType='" + _frm1.enabledordertype + "' and     (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.Delivery.Phone like '%" + word + "%' or dbo.Sale.OrderType='" + _frm1.enabledordertype + "' and     (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.Delivery.Name like '%" + word + "%'   or dbo.Sale.OrderType='" + _frm1.enabledordertype + "' and     (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.DinInTables.TableNo like '%" + word + "%'    or dbo.Sale.OrderType='" + _frm1.enabledordertype + "' and     (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.ResturantStaff.Name like '%" + word + "%'  ORDER BY Bill_No desc";
                    }
                }
                else
                {
                    if (word == "")
                    {
                        q9 = "SELECT        TOP (100) PERCENT dbo.Sale.Id AS Bill_No, dbo.Sale.OnlineId AS Online_BillNo, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.Sale.Customer, dbo.Sale.Terminal, dbo.Sale.TerminalOrder, dbo.Delivery.Name,                          dbo.Delivery.Phone, dbo.Delivery.Address, dbo.Delivery.RiderID, dbo.Delivery.Status, dbo.Delivery.DispatchedTime, dbo.Delivery.DeliveredTime, dbo.DinInTables.TableNo,dbo.DinInTables.Guests, dbo.ResturantStaff.Name AS Waiter,                           dbo.TakeAway.CustomerId AS TakeawayID FROM            dbo.ResturantStaff RIGHT OUTER JOIN                         dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId RIGHT OUTER JOIN                         dbo.Sale LEFT OUTER JOIN                         dbo.TakeAway ON dbo.Sale.Id = dbo.TakeAway.Saleid ON dbo.DinInTables.Saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId  WHERE     (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "' and dbo.Sale.branchid='" + _frm1.branchid + "' ORDER BY Bill_No desc";
                    }
                    else
                    {
                        q9 = "SELECT        TOP (100) PERCENT dbo.Sale.Id AS Bill_No, dbo.Sale.OnlineId AS Online_BillNo, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.Sale.Customer, dbo.Sale.Terminal, dbo.Sale.TerminalOrder, dbo.Delivery.Name,                          dbo.Delivery.Phone, dbo.Delivery.Address, dbo.Delivery.RiderId, dbo.Delivery.Status, dbo.Delivery.Dispatchedtime, dbo.Delivery.deliveredtime FROM            dbo.Sale LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId WHERE     (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.Sale.id like '%" + word + "%' ORDER BY Bill_No desc";
                        q9 = "SELECT        TOP (100) PERCENT dbo.Sale.Id AS Bill_No, dbo.Sale.OnlineId AS Online_BillNo, dbo.Sale.time, dbo.Sale.NetBill, dbo.Sale.OrderType, dbo.Sale.Customer, dbo.Sale.Terminal, dbo.Sale.TerminalOrder, dbo.Delivery.Name,                          dbo.Delivery.Phone, dbo.Delivery.Address, dbo.Delivery.RiderID, dbo.Delivery.Status, dbo.Delivery.DispatchedTime, dbo.Delivery.DeliveredTime, dbo.DinInTables.TableNo,dbo.DinInTables.Guests, dbo.ResturantStaff.Name AS Waiter,                           dbo.TakeAway.CustomerId AS TakeawayID FROM            dbo.ResturantStaff RIGHT OUTER JOIN                         dbo.DinInTables ON dbo.ResturantStaff.Id = dbo.DinInTables.WaiterId RIGHT OUTER JOIN                         dbo.Sale LEFT OUTER JOIN                         dbo.TakeAway ON dbo.Sale.Id = dbo.TakeAway.Saleid ON dbo.DinInTables.Saleid = dbo.Sale.Id LEFT OUTER JOIN                         dbo.Delivery ON dbo.Sale.Id = dbo.Delivery.SaleId  WHERE      (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and cast( dbo.Sale.id as varchar(max)) like '%" + word + "%' or    (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.Delivery.Phone like '%" + word + "%' or  (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.Delivery.Name like '%" + word + "%'  or    (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.DinInTables.TableNo like '%" + word + "%'    or    (dbo.Sale.BillStatus = 'Pending') and dbo.Sale.date='" + date + "'  and dbo.Sale.branchid='" + _frm1.branchid + "'  and dbo.ResturantStaff.Name like '%" + word + "%'  ORDER BY Bill_No desc";
          
                    }
                }
                ds9 = objCore.funGetDataSet(q9);
                for (int i = 0; i < ds9.Tables[0].Rows.Count; i++)
                {
                    string name = "";
                    if (ds9.Tables[0].Rows[i]["RiderId"].ToString().Length > 0)
                    {
                        name = ridername(ds9.Tables[0].Rows[i]["RiderId"].ToString());
                    }
                    string kot = "";// IsPrinterOk(ds9.Tables[0].Rows[i]["Bill_No"].ToString(), 0);
                  
                    dt.Rows.Add(ds9.Tables[0].Rows[i]["Bill_No"].ToString(), ds9.Tables[0].Rows[i]["time"].ToString(), ds9.Tables[0].Rows[i]["NetBill"].ToString(), ds9.Tables[0].Rows[i]["OrderType"].ToString(), ds9.Tables[0].Rows[i]["Terminal"].ToString(), 
                        ds9.Tables[0].Rows[i]["Customer"].ToString(), kot, ds9.Tables[0].Rows[i]["TakeawayID"].ToString(),ds9.Tables[0].Rows[i]["TableNo"].ToString(),ds9.Tables[0].Rows[i]["Guests"].ToString(),ds9.Tables[0].Rows[i]["Waiter"].ToString(), ds9.Tables[0].Rows[i]["Phone"].ToString(),ds9.Tables[0].Rows[i]["Address"].ToString(), ds9.Tables[0].Rows[i]["Status"].ToString(), name, ds9.Tables[0].Rows[i]["Dispatchedtime"].ToString(), ds9.Tables[0].Rows[i]["deliveredtime"].ToString());
                }
                                       
                bool chk = false;                
                dataGridView1.DataSource = dt;                
                //foreach (DataGridViewColumn column in dataGridView1.Columns)
                //{                    
                //    column.Width=150;//.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                //}

            }
            catch (Exception ex)
            {


            }
        }
        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
           
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        protected string  getdeliveryid(string id)
        {
            string did = "";
            DataSet dss = new DataSet();
            string q = "select id from delivery where saleid='"+id+"'";
            dss = objCore.funGetDataSet(q);
            if (dss.Tables[0].Rows.Count > 0)
            {
                did = dss.Tables[0].Rows[0][0].ToString();
            }
            return did;
        }
        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    //_frm1.cleargrid();
                    string saleid = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    string type = dataGridView1.Rows[indx].Cells[4].Value.ToString();
                    //_frm1.Islbldelivery = type;
                    //_frm1.deliveryid = getdeliveryid(saleid);
                   // _frm1.getorders("new");
                    _frm1.callrecalsale(saleid, "no");
                    _frm1.Enabled = true;
                   // _frm1.TopMost = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                
                
            }
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            _frm1.getorders("new");
            //_frm1.Islbldelivery = "Not Selected";
            _frm1.Enabled = true;
            //_frm1.TopMost = true;
            this.Close();

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void vButton1_Click_1(object sender, EventArgs e)
        {
            getdata(txtbill.Text);
        }
        private string IsPrinterOk(string name, int checkTimeInMillisec)
        {

            System.Collections.IList value = null;
            // do
            {
                //checkTimeInMillisec should be between 2000 and 5000
                System.Threading.Thread.Sleep(checkTimeInMillisec);
                // or use Timer with Threading.Monitor instead of thread sleep

                using (System.Management.ManagementObjectSearcher searcher = new System.Management.ManagementObjectSearcher("SELECT * FROM Win32_PrintJob WHERE Document like '%" + name + "%'"))
                {
                    value = null;

                    if (searcher.Get().Count == 0) // Number of pending document.

                        return "Success";  //return because we haven't got any pending document.
                    else
                    {
                        foreach (System.Management.ManagementObject printer in searcher.Get())
                        {
                            string nameww = printer["Document"].ToString();
                            value = printer.Properties.Cast<System.Management.PropertyData>().Where(p => p.Name.Equals("Status")).Select(p => p.Value).ToList();
                            break;
                        }
                    }
                }
            }
           
            //while (value.Contains("Printing") || value.Contains("UNKNOWN") || value.Contains("OK"));
            //if (value.Contains("Error"))
            //{

            //}
            return "Failed";
        }
        private void vButton2_Click_1(object sender, EventArgs e)
        {
            txtbill.Text = "";
            getdata("");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;

                if (t.Text == "." && txtbill.Text.Contains("."))
                {

                }
                else
                {                   
                    txtbill.Text = txtbill.Text + t.Text.Replace("&&", "&");                                        
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }
      
        private void button14_Click(object sender, EventArgs e)
        {
            //shiftkey();
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {

                int index = txtbill.SelectionStart;
                txtbill.Text = txtbill.Text.Remove(txtbill.SelectionStart - 1, 1);
                txtbill.Select(index - 1, 1);
                txtbill.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            txtbill.Text = txtbill.Text + " ";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            getdata(txtbill.Text);
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    string id = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Bill No"].Value.ToString();
                    string q = "insert into log (Name, Time, Description,userid) values ('Bill Print','" + DateTime.Now + "','" + id.ToString() + "','" + _frm1.userid + "')";
                    objCore.executeQuery(q);

                }
                catch (Exception ex)
                {

                }

                string name = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["Order Type"].Value.ToString();
                if (name=="Delivery")
                {
                    Dispatch obj = new Dispatch(this);
                    obj.saleid = dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value.ToString();
                    obj.Show();
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton18_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    //_frm1.cleargrid();
                    string saleid = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    
                    _frm1.callrecalsale(saleid, "cashout");
                    _frm1.Enabled = true;
                    
                    this.Close();
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
