using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VIBlend.WinForms.Controls;
using VIBlend.WinForms.DataGridView.Filters;
using VIBlend.Utilities;
using System.Net;
using Newtonsoft.Json;
namespace POSRestaurant.RawItems
{
    public partial class PurChase_List : Form
    {
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        DataTable dt;
        public int editmode;
        Purchase _frm;
        public PurChase_List(Purchase frm)
        {
            InitializeComponent();
            _frm = frm;
        }

        private void PurChase_List_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "select * from supplier";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "All";

                ds.Tables[0].Rows.Add(dr);
                cmbsupplier.DataSource = ds.Tables[0];
                cmbsupplier.ValueMember = "id";
                cmbsupplier.DisplayMember = "Name";
                cmbsupplier.Text = "All";
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected void getdata()
        {
            try
            {
                string q = "";
                ds = new DataSet();
                if (cmbsupplier.Text == "All")
                {
                    q = "SELECT     dbo.Supplier.Name AS Supplier_Name, dbo.Purchase.Id AS SerialNo, dbo.Purchase.InvoiceNo, dbo.Purchase.Date, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.Purchase.TotalAmount, dbo.Purchase.Status, dbo.Purchase.BranchCode FROM dbo.Purchase INNER JOIN dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.Branch ON dbo.Purchase.BranchCode = dbo.Branch.Id INNER JOIN  dbo.Stores ON dbo.Purchase.StoreCode = dbo.Stores.Id where (dbo.Purchase.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.Status !='Cancel'";
                }
                else
                {
                    q = "SELECT     dbo.Supplier.Name AS Supplier_Name, dbo.Purchase.Id AS SerialNo, dbo.Purchase.InvoiceNo, dbo.Purchase.Date, dbo.Branch.BranchName, dbo.Stores.StoreName, dbo.Purchase.TotalAmount, dbo.Purchase.Status, dbo.Purchase.BranchCode FROM dbo.Purchase INNER JOIN dbo.Supplier ON dbo.Purchase.SupplierId = dbo.Supplier.Id INNER JOIN                      dbo.Branch ON dbo.Purchase.BranchCode = dbo.Branch.Id INNER JOIN  dbo.Stores ON dbo.Purchase.StoreCode = dbo.Stores.Id where (dbo.Purchase.Date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "') and dbo.Purchase.SupplierId='" + cmbsupplier.SelectedValue + "' and dbo.Purchase.Status !='Cancel'";

                }
                ds = objCore.funGetDataSet(q);

                dataGridView1.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                var BtnCell = (DataGridViewButtonCell)dr.Cells[0];
                BtnCell.Value = "Post";
                var BtnCell1 = (DataGridViewButtonCell)dr.Cells[1];
                BtnCell1.Value = "UnPost";
                var BtnCell2 = (DataGridViewButtonCell)dr.Cells[2];
                BtnCell2.Value = "Preview";
            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
           int indx = dataGridView1.CurrentCell.RowIndex;

           if (indx >= 0)
           {
               if (dataGridView1.Rows[indx].Cells[10].Value.ToString() != "Posted")
               {
                   string rawid = dataGridView1.Rows[indx].Cells[4].Value.ToString();
                   _frm.getdata(rawid);
                   this.Close();
               }
           }
        }
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='demand'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            if (tp == "")
            {
                tp = "sql";
            }
            return tp;
        }
        string url = "";
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0)
                {
                    DialogResult dr = MessageBox.Show("Are you sure to POST. It can not be reversed", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                        string branchid = dataGridView1.Rows[e.RowIndex].Cells["BranchCode"].Value.ToString();
                        if (url == "")
                        {
                            type();
                        }


                        string q = "";
                        DataSet ds = new DataSet();

                        bool chk = false;
                        try
                        {

                            string URI = url + "/Purchase.asmx/Getresponse";
                            string myparametrs = "";
                            q = "SELECT        Id, SupplierId, TotalAmount, Date, BranchCode, StoreCode, InvoiceNo, Status, UploadStatus, branchid, PONo, onlineid, PaymentStatus FROM            Purchase where id='" + id + "'";
                            ds = new DataSet();
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                URI = URI + "?id=" + id + "&InvoiceNo=" + ds.Tables[0].Rows[0]["InvoiceNo"].ToString() + "&BranchCode=" + ds.Tables[0].Rows[0]["BranchCode"].ToString() + "&Date=" + Convert.ToDateTime(ds.Tables[0].Rows[0]["Date"].ToString()).ToString("yyyy-MM-dd") + "&TotalAmount=" + ds.Tables[0].Rows[0]["TotalAmount"].ToString() + "&SupplierId=" + ds.Tables[0].Rows[0]["SupplierId"].ToString() + "&branchid=" + ds.Tables[0].Rows[0]["BranchCode"].ToString() + "&StoreCode=" + ds.Tables[0].Rows[0]["StoreCode"].ToString();
                                q = "SELECT   Id, RawItemId, PurchaseId, Package, PackageItems, TotalItems, Price, PricePerItem, TotalAmount, UploadStatus, branchid, Description,  Expiry FROM            PurchaseDetails where purchaseid="+id;
                                ds = new DataSet();
                                
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        if (myparametrs != "")
                                        {
                                            myparametrs = myparametrs + ",";
                                        }
                                        myparametrs = myparametrs + "{\"onlineid\":\"" + ds.Tables[0].Rows[i]["Id"].ToString() + "\",\"Description\":\"" + (ds.Tables[0].Rows[i]["Description"].ToString()) + "\",\"Expiry\":\"" + (ds.Tables[0].Rows[i]["Expiry"].ToString()) + "\",\"RawItemId\":\"" + (ds.Tables[0].Rows[i]["RawItemId"].ToString()) + "\",\"Package\":\"" + ds.Tables[0].Rows[i]["Package"].ToString() + "\",\"PackageItems\":\"" + ds.Tables[0].Rows[i]["PackageItems"].ToString().Replace("'", "") + "\",\"TotalItems\":\"" + ds.Tables[0].Rows[i]["TotalItems"].ToString().Replace("'", "") + "\",\"Price\":\"" + ds.Tables[0].Rows[i]["Price"].ToString().Replace("'", "") + "\",\"PricePerItem\":\"" + ds.Tables[0].Rows[i]["PricePerItem"].ToString() + "\",\"TotalAmount\":\"" + ds.Tables[0].Rows[i]["TotalAmount"].ToString() + "\"}";
                                    }
                                }
                                string msg = "";
                                myparametrs = "[" + myparametrs + "]";
                                using (WebClient wc = new WebClient())
                                {
                                    wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                                    //wc.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                                    string HtmlResult = wc.UploadString(URI, myparametrs);
                                    msg = HtmlResult;
                                    List<onlineidresponsecls> res = (List<onlineidresponsecls>)JsonConvert.DeserializeObject(HtmlResult, typeof(List<onlineidresponsecls>));

                                    //txt_postData.Text = HtmlResult;
                                    if (HtmlResult.ToString().ToLower().Contains("success"))
                                    {
                                        chk = true;
                                    }
                                    else
                                    {
                                        chk = false;
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            if (chk == true)
                            {
                                chk = false;
                                string URI = url + "/DeliveryServices/vendorsledger.asmx/Getresponse";
                                string myparametrs = "";
                                q = "select   Id, Date, SupplierId, PayableAccountId, VoucherNo, Description, Credit, Debit, Balance, branchid,invoiceno from SupplierAccount where voucherno='GRN-" + id + "-" + branchid + "'";
                                ds = new DataSet();
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                    {
                                        if (myparametrs != "")
                                        {
                                            myparametrs = myparametrs + ",";
                                        }
                                        myparametrs = myparametrs + "{\"table\":\"SupplierAccount\",\"onlineid\":\"" + ds.Tables[0].Rows[i]["Id"].ToString() + "\",\"Date\":\"" + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("yyyy-MM-dd") + "\",\"customerid\":\"" + ds.Tables[0].Rows[i]["SupplierId"].ToString() + "\",\"ChartAccountId\":\"" + ds.Tables[0].Rows[i]["PayableAccountId"].ToString() + "\",\"VoucherNo\":\"" + ds.Tables[0].Rows[i]["VoucherNo"].ToString().Replace("'", "") + "\",\"Description\":\"" + ds.Tables[0].Rows[i]["Description"].ToString().Replace("'", "") + "\",\"Credit\":\"" + ds.Tables[0].Rows[i]["Credit"].ToString().Replace("'", "") + "\",\"Debit\":\"" + ds.Tables[0].Rows[i]["Debit"].ToString() + "\",\"branchid\":\"" + ds.Tables[0].Rows[i]["branchid"].ToString() + "\",\"invoiceno\":\"" + ds.Tables[0].Rows[i]["invoiceno"].ToString() + "\"}";
                                    }
                                    q = "select   Id, Date, ChartAccountId, VoucherNo, Description, Credit, Debit, Balance, branchid from InventoryAccount where voucherno='GRN-" + id + "-" + branchid + "'";
                                    ds = new DataSet();
                                    ds = objCore.funGetDataSet(q);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                        {
                                            if (myparametrs != "")
                                            {
                                                myparametrs = myparametrs + ",";
                                            }
                                            myparametrs = myparametrs + "{\"table\":\"InventoryAccount\",\"onlineid\":\"" + ds.Tables[0].Rows[i]["Id"].ToString() + "\",\"Date\":\"" + Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"].ToString()).ToString("yyyy-MM-dd") + "\",\"ChartAccountId\":\"" + ds.Tables[0].Rows[i]["ChartAccountId"].ToString() + "\",\"VoucherNo\":\"" + ds.Tables[0].Rows[i]["VoucherNo"].ToString().Replace("'", "") + "\",\"Description\":\"" + ds.Tables[0].Rows[i]["Description"].ToString().Replace("'", "") + "\",\"Credit\":\"" + ds.Tables[0].Rows[i]["Credit"].ToString().Replace("'", "") + "\",\"Debit\":\"" + ds.Tables[0].Rows[i]["Debit"].ToString() + "\",\"branchid\":\"" + ds.Tables[0].Rows[i]["branchid"].ToString() + "\"}";
                                        }
                                    }
                                    string msg = "";
                                    myparametrs = "[" + myparametrs + "]";
                                    using (WebClient wc = new WebClient())
                                    {
                                        wc.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                                        //wc.Headers[HttpRequestHeader.ContentType] = "application/json; charset=utf-8";
                                        string HtmlResult = wc.UploadString(URI, myparametrs);
                                        msg = HtmlResult;
                                        List<onlineidresponsecls> res = (List<onlineidresponsecls>)JsonConvert.DeserializeObject(HtmlResult, typeof(List<onlineidresponsecls>));

                                        //txt_postData.Text = HtmlResult;
                                        if (HtmlResult.Contains("success"))
                                        {
                                            chk = true;
                                        }
                                        else
                                        {
                                            chk = false;
                                        }
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }

                        if (chk == true)
                        {
                            q = "Update purchase set status='Posted' where id='" + id + "'";
                            objCore.executeQuery(q);
                            MessageBox.Show("Posted Successfully");
                        }
                        else
                        {
                            MessageBox.Show("Posted Successfully");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
               
            }
            try
            {
                if (e.ColumnIndex == 1)
                {
                    string right = POSRestaurant.classes.Clsdbcon.authenticateEdit("Purchase Items", POSRestaurant.Properties.Settings.Default.UserId.ToString(), "update");
                    if (right == "no")
                    {
                        MessageBox.Show("You are not allowed to unpost");
                        return;
                    }

                    DialogResult msg = MessageBox.Show("Are you sure you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (msg == DialogResult.Yes)
                    {

                        string id = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                       string q = "Update purchase set status='Pending' where id='" + id + "'";
                        objCore.executeQuery(q);
                    }
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                if (e.ColumnIndex == 2)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    POSRestaurant.Reports.Inventory.frmInventoryPreview obj = new Reports.Inventory.frmInventoryPreview();
                    obj.id = id;
                    obj.Show();
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
