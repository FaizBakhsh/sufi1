using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.RawItems
{
    public partial class AddRawItems : Form
    {
        string fname = "";
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        string q = "";
        public static string itemid = "";
        public AddRawItems()
        {
            InitializeComponent();
            richTextBox4.ForeColor = Color.LightGray;
            richTextBox4.Text = "Seach Item by Keyword";
            this.richTextBox4.Leave += new System.EventHandler(this.textBox1_Leave);
            this.richTextBox4.Enter += new System.EventHandler(this.textBox1_Enter);
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (richTextBox4.Text.Length == 0)
            {
                richTextBox4.Text = "Seach Item by Keyword";
                richTextBox4.ForeColor = Color.LightGray;
            }
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (richTextBox4.Text == "Seach Item by Keyword")
            {
                richTextBox4.Text = "";
                richTextBox4.ForeColor = Color.Black;
            }
        }
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
        public void fillgroups()
        {
            try
            {
                DataTable dt = new DataTable();
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from groups";
                ds = objCore.funGetDataSet(q);
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
                dr["groupName"] = "Please Select";
                dr["id"] = "0";
                dt.Rows.InsertAt(dr, 0);
                comboBox1.DataSource = dt;
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "groupName";
                comboBox1.Text = "Please Select";
               
                
            }
            catch (Exception ex)
            {
                
                
            }
            

        }
        public void fillsalesaccounts()
        {
            try
            {
                string q = "select * from ChartofAccounts where AccountType='Revenue'";
               DataSet dsrev = objCore.funGetDataSet(q);
               DataRow dr = dsrev.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                dsrev.Tables[0].Rows.Add(dr);
                cmbrevenue.DataSource = dsrev.Tables[0];
                cmbrevenue.ValueMember = "id";
                cmbrevenue.DisplayMember = "Name";
                cmbrevenue.Text = "Please Select";


            }
            catch (Exception ex)
            {


            }


        }
        public void fillsalescost()
        {
            try
            {
                string q = "select * from ChartofAccounts where AccountType='Cost of Sales'";
                DataSet dscost = objCore.funGetDataSet(q);
                DataRow dr = dscost.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                dscost.Tables[0].Rows.Add(dr);
                cmbcostofsales.DataSource = dscost.Tables[0];
                cmbcostofsales.ValueMember = "id";
                cmbcostofsales.DisplayMember = "Name";
                cmbcostofsales.Text = "Please Select";


            }
            catch (Exception ex)
            {


            }


        }
        public void fillinventory()
        {
            try
            {
                string q = "select * from ChartofAccounts where AccountType='Current Assets'";
                DataSet dsinv = objCore.funGetDataSet(q);
                DataRow dr = dsinv.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                dsinv.Tables[0].Rows.Add(dr);
                cmbinventory.DataSource = dsinv.Tables[0];
                cmbinventory.ValueMember = "id";
                cmbinventory.DisplayMember = "Name";
                cmbinventory.Text = "Please Select";


            }
            catch (Exception ex)
            {


            }


        }
        public void fillcategory()
        {
            try
            {
                //category
                comboBox2.DataSource = null;
               DataSet ds1 = new DataSet();
               string q1 = "select * from Category where groupid='" + comboBox1.SelectedValue + "'";
                ds1 = objCore.funGetDataSet(q1);
                DataRow dr = ds1.Tables[0].NewRow();
                dr["CategoryName"] = "Please Select";

                ds1.Tables[0].Rows.Add(dr);
                comboBox2.DataSource = ds1.Tables[0];
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "CategoryName";
                if (comboBox1.Text == "Please Select")
                {
                    comboBox2.Text = "Please Select";
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void filltype()
        {
            try
            {
                //category
                DataSet ds2 = new DataSet();
               string  q2 = "select * from type where groupid='" + comboBox1.SelectedValue + "' and CategoryId='"+comboBox2.SelectedValue+"'";
                ds2 = objCore.funGetDataSet(q2);
                DataRow dr = ds2.Tables[0].NewRow();
                dr["TypeName"] = "Please Select";
                
                dr["id"] = "0";
                ds2.Tables[0].Rows.Add(dr);
                comboBox3.DataSource = ds2.Tables[0];
                comboBox3.ValueMember = "id";
                comboBox3.DisplayMember = "TypeName";
                
                if (comboBox2.Text == "Please Select")
                {
                    comboBox3.Text = "Please Select";
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void fillbrand()
        {
            try
            {
                //category
                DataSet ds3 = new DataSet();
               string q3 = "select * from brands";
                ds3 = objCore.funGetDataSet(q3);
                DataRow dr = ds3.Tables[0].NewRow();
                dr["BrandName"] = "Please Select";
                ds3.Tables[0].Rows.Add(dr);
                comboBox4.DataSource = ds3.Tables[0];
                comboBox4.ValueMember = "id";
                comboBox4.DisplayMember = "BrandName";
                comboBox4.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }
        }
        public void filluom()
        {
            try
            {
                //category
                DataSet ds4 = new DataSet();
              string   q4 = "select * from UOM";
                ds4 = objCore.funGetDataSet(q4);
                DataRow dr = ds4.Tables[0].NewRow();
                dr["UOM"] = "Please Select";
                ds4.Tables[0].Rows.Add(dr);
                comboBox5.DataSource = ds4.Tables[0];
                comboBox5.ValueMember = "id";
                comboBox5.DisplayMember = "UOM";
                comboBox5.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }
        }
        public void fillsize()
        {
            try
            {
                //category
                DataSet ds5 = new DataSet();
               string q5 = "select * from size";
                ds5 = objCore.funGetDataSet(q5);
                DataRow dr = ds5.Tables[0].NewRow();
                dr["SizeName"] = "Please Select";
                ds5.Tables[0].Rows.Add(dr);
                comboBox6.DataSource = ds5.Tables[0];
                comboBox6.ValueMember = "id";
                comboBox6.DisplayMember = "SizeName";
                comboBox6.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }
        }
        public void fillbranch()
        {
            try
            {
                //category
                DataSet ds6 = new DataSet();
               string q6 = "select * from Branch";
                ds6 = objCore.funGetDataSet(q6);
                DataRow dr = ds6.Tables[0].NewRow();
                dr["BranchName"] = "Please Select";
                ds6.Tables[0].Rows.Add(dr);
                comboBox7.DataSource = ds6.Tables[0];
                comboBox7.ValueMember = "id";
                comboBox7.DisplayMember = "BranchName";
                comboBox7.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }
        }
        public void fillstore()
        {
            try
            {
                //category
                DataSet ds7 = new DataSet();
              string  q7 = "select * from stores where BranchId='"+comboBox7.SelectedValue+"'";
                ds7 = objCore.funGetDataSet(q7);
                comboBox8.DataSource = ds7.Tables[0];
                DataRow dr = ds7.Tables[0].NewRow();
                dr["StoreName"] = "Please Select";
                ds7.Tables[0].Rows.Add(dr);
                comboBox8.ValueMember = "id";
                comboBox8.DisplayMember = "StoreName";
                if (comboBox7.Text == "Please Select")
                {
                    comboBox8.Text = "Please Select";
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void fillcolor()
        {
            try
            {
                //category
                DataSet ds8 = new DataSet();
               string q8 = "select * from Color";
                ds8 = objCore.funGetDataSet(q8);
                DataRow dr = ds8.Tables[0].NewRow();
                dr["Caption"] = "Please Select";
                ds8.Tables[0].Rows.Add(dr);
                comboBox9.DataSource = ds8.Tables[0];
                comboBox9.ValueMember = "id";
                comboBox9.DisplayMember = "Caption";
                comboBox9.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }
        }
        public void getdata()
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                if (richTextBox4.Text == string.Empty || richTextBox4.Text == "Seach Item by Keyword")
                {
                    //q9 = "SELECT    dbo.RawItem.Id,dbo.RawItem.ItemName, dbo.RawItem.Code,dbo.RawItem.BarCode,dbo.RawItem.Price, dbo.RawItem.Description, dbo.Size.SizeName, dbo.Type.TypeName, dbo.Stores.StoreName, dbo.Category.CategoryName, dbo.Brands.BrandName, dbo.Color.ColorName, dbo.Groups.GroupName, dbo.Branch.BranchName                        FROM         dbo.Groups INNER JOIN                      dbo.Brands INNER JOIN                      dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId ON dbo.Brands.Id = dbo.RawItem.BrandId INNER JOIN                      dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id ON dbo.Groups.Id = dbo.RawItem.GroupId INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id";
                    q9 = "SELECT     dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Code, dbo.RawItem.BarCode, dbo.RawItem.Price, dbo.RawItem.Description, dbo.Size.SizeName, dbo.Type.TypeName, dbo.Stores.StoreName,                       dbo.Category.CategoryName, dbo.Brands.BrandName, dbo.Color.caption as Color, dbo.Groups.GroupName, dbo.Branch.BranchName, ChartofAccounts_1.Name AS SalesAccount,                       dbo.ChartofAccounts.Name AS CostofSales, ChartofAccounts_2.Name AS InventoryAccount FROM         dbo.Groups INNER JOIN                      dbo.Brands INNER JOIN                      dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId ON dbo.Brands.Id = dbo.RawItem.BrandId INNER JOIN                      dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id ON dbo.Groups.Id = dbo.RawItem.GroupId INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id LEFT OUTER JOIN                      dbo.ChartofAccounts AS ChartofAccounts_2 ON dbo.RawItem.Inventoryid = ChartofAccounts_2.Id LEFT OUTER JOIN                      dbo.ChartofAccounts ON dbo.RawItem.Costofsalesid = dbo.ChartofAccounts.Id LEFT OUTER JOIN                      dbo.ChartofAccounts AS ChartofAccounts_1 ON dbo.RawItem.Revenueid = ChartofAccounts_1.Id";
                }
                else
                {
                    q9 = "SELECT     dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Code, dbo.RawItem.BarCode, dbo.RawItem.Price, dbo.RawItem.Description, dbo.Size.SizeName, dbo.Type.TypeName, dbo.Stores.StoreName,                       dbo.Category.CategoryName, dbo.Brands.BrandName, dbo.Color.caption as Color, dbo.Groups.GroupName, dbo.Branch.BranchName, ChartofAccounts_1.Name AS SalesAccount,                       dbo.ChartofAccounts.Name AS CostofSales, ChartofAccounts_2.Name AS InventoryAccount FROM         dbo.Groups INNER JOIN                      dbo.Brands INNER JOIN                      dbo.Branch INNER JOIN                      dbo.RawItem ON dbo.Branch.Id = dbo.RawItem.BranchId ON dbo.Brands.Id = dbo.RawItem.BrandId INNER JOIN                      dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id ON dbo.Groups.Id = dbo.RawItem.GroupId INNER JOIN                      dbo.Color ON dbo.RawItem.ColorId = dbo.Color.Id INNER JOIN                      dbo.Size ON dbo.RawItem.SizeId = dbo.Size.Id INNER JOIN                      dbo.Stores ON dbo.RawItem.StoreId = dbo.Stores.Id INNER JOIN                      dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id INNER JOIN                      dbo.UOM ON dbo.RawItem.UOMId = dbo.UOM.Id LEFT OUTER JOIN                      dbo.ChartofAccounts AS ChartofAccounts_2 ON dbo.RawItem.Inventoryid = ChartofAccounts_2.Id LEFT OUTER JOIN                      dbo.ChartofAccounts ON dbo.RawItem.Costofsalesid = dbo.ChartofAccounts.Id LEFT OUTER JOIN                      dbo.ChartofAccounts AS ChartofAccounts_1 ON dbo.RawItem.Revenueid = ChartofAccounts_1.Id where dbo.RawItem.ItemName like '%" + richTextBox4.Text + "%' or dbo.RawItem.BarCode like '%" + richTextBox4.Text + "%' or dbo.RawItem.Code like '%" + richTextBox4.Text + "%' or dbo.RawItem.Description like '%" + richTextBox4.Text + "%' or dbo.Size.SizeName like '%" + richTextBox4.Text + "%' or dbo.Type.TypeName like '%" + richTextBox4.Text + "%' or dbo.Stores.StoreName like '%" + richTextBox4.Text + "%' or dbo.Category.CategoryName like '%" + richTextBox4.Text + "%' or dbo.Brands.BrandName like '%" + richTextBox4.Text + "%' or dbo.Color.ColorName like '%" + richTextBox4.Text + "%' or dbo.Groups.GroupName like '%" + richTextBox4.Text + "%' or dbo.Branch.BranchName like '%" + richTextBox4.Text + "%'";
                }
                ds9 = objCore.funGetDataSet(q9);
                dataGridView1.DataSource = ds9.Tables[0];
                dataGridView1.Columns[0].Visible = false;
                
            }
            catch (Exception ex)
            {


            }
        }
        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void AddRawItems_Load(object sender, EventArgs e)
        {
            try
            {
                domainUpDown1.Text = "1";
            }
            catch (Exception ex)
            {
                
               
            }
            fillgroups();
            fillcategory();
            filltype();
            fillbrand();
            filluom();
            fillsize();
            fillbranch();
            fillstore();
            fillcolor();
            fillsalesaccounts();
            fillsalescost();
            fillinventory();
            getdata();
           //DataSet dsitems = new DataSet();
           //dsitems = objCore.funGetDataSet("select code from barcode");
           // AutoCompleteStringCollection MyCollection = new AutoCompleteStringCollection();
           // for (int i = 0; i < dsitems.Tables[0].Rows.Count; i++)
           // {
           //     MyCollection.Add(dsitems.Tables[0].Rows[i]["code"].ToString());//.GetString(0));
           // }
           // richTextBox2.AutoCompleteCustomSource = MyCollection;

           // richTextBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
           // richTextBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;


        }
       
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillcategory();
            filltype();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            filltype();
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            fillstore();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
        public void getinfo(string id)
        {
           DataSet dsinfo = new DataSet();
           string qry = "select * from rawitem where id='"+id+"'";
           dsinfo = objCore.funGetDataSet(qry);
           if (dsinfo.Tables[0].Rows.Count > 0)
           {
               txtName.Text = dsinfo.Tables[0].Rows[0]["ItemName"].ToString();
               richTextBox1.Text = dsinfo.Tables[0].Rows[0]["Code"].ToString();
               richTextBox2.Text = dsinfo.Tables[0].Rows[0]["barcode"].ToString();
               richTextBox3.Text = dsinfo.Tables[0].Rows[0]["Description"].ToString();
               try
               {
                   txtprice.Text = dsinfo.Tables[0].Rows[0]["price"].ToString();
               }
               catch (Exception ex)
               {
                   
                   
               }
               try
               {
                   comboBox1.SelectedValue = dsinfo.Tables[0].Rows[0]["GroupId"].ToString();
               }
               catch (Exception ex)
               {
                   
                  
               }
               try
               {
                   comboBox2.SelectedValue = dsinfo.Tables[0].Rows[0]["CategoryId"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                   
                   comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["TypeId"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                   
                   comboBox4.SelectedValue = dsinfo.Tables[0].Rows[0]["BrandId"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                 
                   comboBox5.SelectedValue = dsinfo.Tables[0].Rows[0]["UOMId"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                 
                   comboBox6.SelectedValue = dsinfo.Tables[0].Rows[0]["SizeId"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                   
                   comboBox7.SelectedValue = dsinfo.Tables[0].Rows[0]["BranchId"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                   comboBox8.SelectedValue = dsinfo.Tables[0].Rows[0]["StoreId"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                   comboBox9.SelectedValue = dsinfo.Tables[0].Rows[0]["ColorId"].ToString();
               }
               catch (Exception ex)
               {


               }

               try
               {
                   cmbrevenue.SelectedValue = dsinfo.Tables[0].Rows[0]["Revenueid"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                   cmbcostofsales.SelectedValue = dsinfo.Tables[0].Rows[0]["Costofsalesid"].ToString();
               }
               catch (Exception ex)
               {


               }
               try
               {
                   cmbinventory.SelectedValue = dsinfo.Tables[0].Rows[0]["Inventoryid"].ToString();
               }
               catch (Exception ex)
               {


               }
               
               string img = dsinfo.Tables[0].Rows[0]["img"].ToString();
               
               try
               {
                   pictureBox1.Image = null;
                   string path = Application.StartupPath + "\\Resources\\Products\\";
                   pictureBox1.Image = Image.FromFile(path + img);
                   pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
               }
               catch (Exception ex)
               {
                   
                   
               }
               vButton2.Text = "Update";
               itemid = id;
           }

        }
        private void button7_Click(object sender, EventArgs e)
        {
           
        }

        private void button8_Click(object sender, EventArgs e)
        {
           
        }

        private void button9_Click(object sender, EventArgs e)
        {
           
        }

        private void richTextBox2_Leave(object sender, EventArgs e)
        {
            //try
            //{
            //    ds = new DataSet();
            //    ds = objCore.funGetDataSet("select * from barcode where code='" + richTextBox2.Text.Trim() + "'");
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {

            //    }
            //    else
            //    {
            //        MessageBox.Show("invalid Barcode.");
            //    }
            //}
            //catch (Exception ex)
            //{
                
               
            //}
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox1.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Group Name");
                    return;
                }
                if (comboBox2.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Category");
                    return;
                }
                if (comboBox3.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Type");
                    return;
                }
                if (comboBox4.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Brand Name");
                    return;
                }
                if (comboBox5.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid UOM");
                    return;
                }
                if (comboBox6.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Size");
                    return;
                }
                if (comboBox7.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Branch Name");
                    return;
                }
                if (comboBox8.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Store Name");
                    return;
                }
                if (comboBox9.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Color");
                    return;
                }
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Item Name");
                    return;
                }
                if (txtprice.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Price");
                    return;
                }
                

                if (cmbrevenue.Text.Trim() == string.Empty || cmbrevenue.Text.Trim()=="Please Select")
                {
                    MessageBox.Show("Please Select Sales Account");
                    return;
                }

                if (cmbcostofsales.Text.Trim() == string.Empty || cmbcostofsales.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Cost of Sales Account");
                    return;
                }
                if (cmbinventory.Text.Trim() == string.Empty || cmbinventory.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Inventory Account");
                    return;
                }

                if (vButton2.Text == "Add")
                {
                    DataSet dsbarcode = new DataSet();
                    dsbarcode = objCore.funGetDataSet("select * from RawItem where barCode='" + richTextBox2.Text.Trim() + "'");
                    if (dsbarcode.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("BarCode already exist.");
                        return;
                    }
                    else
                    {

                    }
                    objCore = new classes.Clsdbcon();
                    int id = 0;
                    ds = new DataSet();
                    ds = objCore.funGetDataSet("select max(id) as id from RawItem");
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

                    q = "select * from RawItem where ItemName='" + txtName.Text.Trim() + "' and GroupId='" + comboBox1.SelectedValue + "' and CategoryId='" + comboBox2.SelectedValue + "' and TypeId='" + comboBox3.SelectedValue + "'and BrandId='" + comboBox4.SelectedValue + "'and UOMId='" + comboBox5.SelectedValue + "'and SizeId='" + comboBox6.SelectedValue + "'and BranchId='" + comboBox7.SelectedValue + "'and StoreId='" + comboBox8.SelectedValue + "'and ColorId='" + comboBox9.SelectedValue + "'";
                    DataSet dsdet = new DataSet();
                    dsdet = objCore.funGetDataSet(q);
                    if (dsdet.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Data already exist");
                        return;
                    }
                    q = "insert into RawItem (id,GroupId,CategoryId,TypeId,BrandId,UOMId,SizeId,BranchId,StoreId,ColorId,ItemName,Code,barCode,Description,img,price,Revenueid,Costofsalesid,Inventoryid) values('" + id + "','" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.SelectedValue + "','" + comboBox4.SelectedValue + "','" + comboBox5.SelectedValue + "','" + comboBox6.SelectedValue + "','" + comboBox7.SelectedValue + "','" + comboBox8.SelectedValue + "','" + comboBox9.SelectedValue + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + richTextBox3.Text.Trim().Replace("'", "''") + "','" + fname + "','" + txtprice.Text.Trim().Replace("'", "''") + "','" + cmbrevenue.SelectedValue + "','" + cmbcostofsales.SelectedValue + "','" + cmbinventory.SelectedValue + "')";
                    objCore.executeQuery(q);
                    getdata();
                    MessageBox.Show("Data Added Successfully");
                }
                if (vButton2.Text == "Update")
                {
                    if (fname == "")
                    {
                        objCore = new classes.Clsdbcon();
                        q = "update RawItem set Revenueid='" + cmbrevenue.SelectedValue + "',Costofsalesid='" + cmbcostofsales.SelectedValue + "',Inventoryid='" + cmbinventory.SelectedValue + "', price='" + txtprice.Text.Trim().Replace("'", "''") + "', ItemName='" + txtName.Text.Trim().Replace("'", "''") + "' , Code='" + richTextBox1.Text.Trim().Replace("'", "''") + "', barCode='" + richTextBox2.Text.Trim().Replace("'", "''") + "' ,Description='" + richTextBox3.Text.Trim().Replace("'", "''") + "' , GroupId='" + comboBox1.SelectedValue + "', CategoryId='" + comboBox2.SelectedValue + "' , TypeId='" + comboBox3.SelectedValue + "', BrandId='" + comboBox4.SelectedValue + "', UOMId='" + comboBox5.SelectedValue + "', SizeId='" + comboBox6.SelectedValue + "', BranchId='" + comboBox7.SelectedValue + "', StoreId='" + comboBox8.SelectedValue + "', ColorId='" + comboBox9.SelectedValue + "' where id='" + itemid + "'";
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        objCore = new classes.Clsdbcon();
                        q = "update RawItem set Revenueid='" + cmbrevenue.SelectedValue + "',Costofsalesid='" + cmbcostofsales.SelectedValue + "',Inventoryid='" + cmbinventory.SelectedValue + "', img='" + fname + "', ItemName='" + txtName.Text.Trim().Replace("'", "''") + "' , Code='" + richTextBox1.Text.Trim().Replace("'", "''") + "', barCode='" + richTextBox2.Text.Trim().Replace("'", "''") + "' ,Description='" + richTextBox3.Text.Trim().Replace("'", "''") + "' , GroupId='" + comboBox1.SelectedValue + "', CategoryId='" + comboBox2.SelectedValue + "' , TypeId='" + comboBox3.SelectedValue + "', BrandId='" + comboBox4.SelectedValue + "', UOMId='" + comboBox5.SelectedValue + "', SizeId='" + comboBox6.SelectedValue + "', BranchId='" + comboBox7.SelectedValue + "', StoreId='" + comboBox8.SelectedValue + "', ColorId='" + comboBox9.SelectedValue + "' where id='" + itemid + "'";
                        objCore.executeQuery(q);
                    }
                    getdata();
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            try
            {
                txtName.Text = string.Empty;
                richTextBox1.Text = string.Empty;
                richTextBox2.Text = string.Empty;
                richTextBox3.Text = string.Empty;
                comboBox1.Text = "Please Select";
                comboBox2.Text = "Please Select";
                comboBox3.Text = "Please Select";
                comboBox4.Text = "Please Select";
                comboBox5.Text = "Please Select";
                comboBox6.Text = "Please Select";
                comboBox7.Text = "Please Select";
                comboBox8.Text = "Please Select";
                comboBox9.Text = "Please Select";
                cmbinventory.Text = "Please Select";
                cmbrevenue.Text = "Please Select";
                cmbcostofsales.Text = "Please Select";
                txtprice.Text = "";
                vButton2.Text = "Add";
                pictureBox1.Image = null;
            }
            catch (Exception ex)
            {
                
               
            }
            
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton6_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    getinfo(id);

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {


                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (msg == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                        q = "delete from rawitem where id='" + id + "'";
                        objCore.executeQuery(q);
                        getdata();

                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            getdata();
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void vButton8_Click(object sender, EventArgs e)
        {
            DataSet dsbarcode1 = new DataSet();
            dsbarcode1 = objCore.funGetDataSet("select * from RawItem where barCode='" + richTextBox2.Text.Trim() + "'");
            if (dsbarcode1.Tables[0].Rows.Count > 0)
            {
                DialogResult dr = MessageBox.Show("BarCode Already Exist.Do You Want to Print BarCode Anyway?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    bindreport();
                }
                
            }
            else
            {
                bindreport();
            }
            
        }
        public void bindreport()
        {
            try
            {
                if (richTextBox2.Text!=string.Empty)
                {
                    DataSet dsprint = new DataSet();
                    dsprint = objCore.funGetDataSet("select * from Printers where type='BarCode'");

                    if (dsprint.Tables[0].Rows.Count > 0)
                    {
                        //ReportDocument rptDoc = new ReportDocument();
                        POSRetail.Reports.crpBarcode rptDoc = new Reports.crpBarcode();
                        POSRetail.Reports.dsBarcode1 dsrpt = new Reports.dsBarcode1();
                        //feereport ds = new feereport(); // .xsd file name
                        DataTable dt = new DataTable();

                        // Just set the name of data table
                        dt.TableName = "Crystal Report";
                        dt = getAllOrders();
                        dsrpt.Tables[0].Merge(dt, false, MissingSchemaAction.Ignore);


                        rptDoc.SetDataSource(dsrpt);
                        //rptDoc.DataDefinition.FormulaFields["PicPath"].Text = POSRetail.Properties.Resources.logo.ToString();// @"'C:\MyImage.jpg'";
                        //rptDoc.PrintOptions.PrinterName = "Posiflex PP6900 576 Partial Cut v3.01 v";
                        //rptDoc.PrintToPrinter(1, false, 0, 0);
                        int prnts = Convert.ToInt32(domainUpDown1.Text);
                        //rptDoc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, @"D:\ASD.rpt");
                        rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["name"].ToString();
                        rptDoc.PrintToPrinter(prnts, false, 0, 0);
                        

                    
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public DataTable getAllOrders()
        {

            DataTable dtrpt = new DataTable();
            dtrpt.Columns.Add("Company", typeof(string));
            dtrpt.Columns.Add("BarCode", typeof(string));
            dtrpt.Columns.Add("Product", typeof(string));
            dtrpt.Columns.Add("BarCodeShow", typeof(string));

            
            string cname = "", caddress = "", cphone = "";
            DataSet dsinfo = new DataSet();
            dsinfo = objCore.funGetDataSet("select * from CompanyInfo");
            if (dsinfo.Tables[0].Rows.Count > 0)
            {
                cname = dsinfo.Tables[0].Rows[0]["Name"].ToString();
                caddress = dsinfo.Tables[0].Rows[0]["Address"].ToString();
                cphone = dsinfo.Tables[0].Rows[0]["Phone"].ToString();
            }
            dtrpt.Rows.Add(cname, "*" + richTextBox2.Text.Trim() + "*", txtName.Text, richTextBox2.Text.Trim());

           

            return dtrpt;
        }
        private string _filterRegexPattern = "[^a-zA-Z0-9]"; // This would be "[^a-z0-9 ]" for this question.
        private int _stringMaxLength = 24;
        private void richTextBox2_TextChanged_1(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_filterRegexPattern))
            {
                var text = richTextBox2.Text;
                var newText =System.Text.RegularExpressions.Regex.Replace(richTextBox2.Text, _filterRegexPattern, "");

                if (newText.Length > _stringMaxLength)
                {
                    newText = newText.Substring(0, _stringMaxLength);
                }


                if (text.Length != newText.Length)
                {
                    var selectionStart = richTextBox2.SelectionStart - (text.Length - newText.Length);
                    richTextBox2.Text = newText;
                    richTextBox2.SelectionStart = selectionStart;
                }
            }
        }

       

        private void richTextBox2_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void label15_Click(object sender, EventArgs e)
        {

        }

        private void vButton7_Click(object sender, EventArgs e)
        {
            try
            {
                objCore = new classes.Clsdbcon();
                int iid = 0;
                ds = new DataSet();
                ds = objCore.funGetDataSet("select max(id) as id from RawItem");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0][0].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    iid = Convert.ToInt32(i) + 1;
                }
                else
                {
                    iid = 1;
                }
                Random rnd = new Random();
               int contd = rnd.Next(0, 999);
                OpenFileDialog opFile = new OpenFileDialog();
                opFile.Title = "Select a Image";
                opFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                string path = Application.StartupPath + "\\Resources\\Products\\"; //System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                // string path = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Resources\";
                string appPath = path; //System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Resources\Resources\";
                if (System.IO.Directory.Exists(appPath) == false)
                {
                    System.IO.Directory.CreateDirectory(appPath);
                }
                if (opFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string iName = opFile.SafeFileName;
                        string filepath = opFile.FileName;
                        iName = iid.ToString()+contd.ToString() + iName;
                        fname = iName;
                       System.IO.File.Copy(filepath, appPath + iName);

                       pictureBox1.Image = Image.FromFile(path + iName);
                       pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                        // picProduct.Image = new Bitmap(opFile.OpenFile());
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Unable to open file " + exp.Message);
                    }
                }
                else
                {
                    opFile.Dispose();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtprice.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtprice.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                
                
            }
            
        }
    }
}
