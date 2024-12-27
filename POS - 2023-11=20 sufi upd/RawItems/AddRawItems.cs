using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.RawItems
{
    public partial class AddRawItems : Form
    { 
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
        public void fillvendor()
        {
            try
            {
                DataTable dt = new DataTable();
                objCore = new classes.Clsdbcon();
                ds = new DataSet();
                q = "select * from supplier";
                ds = objCore.funGetDataSet(q);
                dt = ds.Tables[0];
                DataRow dr = dt.NewRow();
                dr["Name"] = "Please Select";
                dt.Rows.InsertAt(dr, 0);
                drpvendor.DataSource = dt;
                drpvendor.ValueMember = "id";
                drpvendor.DisplayMember = "Name";
                drpvendor.Text = "Please Select";
               
                
            }
            catch (Exception ex)
            {
                
                
            }
            

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
        public void fillcategory()
        {
            try
            {
                //category
                comboBox2.DataSource = null;
               DataSet ds1 = new DataSet();
               string q1 = "select * from Category ";
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
               string  q2 = "select * from type";
                ds2 = objCore.funGetDataSet(q2);
                DataRow dr = ds2.Tables[0].NewRow();
                dr["TypeName"] = "Please Select";

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
        //public void fillbrand()
        //{
        //    try
        //    {
        //        //category
        //        DataSet ds3 = new DataSet();
        //       string q3 = "select * from brands";
        //        ds3 = objCore.funGetDataSet(q3);
        //        DataRow dr = ds3.Tables[0].NewRow();
        //        dr["BrandName"] = "Please Select";
        //        ds3.Tables[0].Rows.Add(dr);
        //        comboBox4.DataSource = ds3.Tables[0];
        //        comboBox4.ValueMember = "id";
        //        comboBox4.DisplayMember = "BrandName";
        //        comboBox4.Text = "Please Select";
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
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
            //try
            //{
            //    //category
            //    DataSet ds5 = new DataSet();
            //   string q5 = "select * from size";
            //    ds5 = objCore.funGetDataSet(q5);
            //    DataRow dr = ds5.Tables[0].NewRow();
            //    dr["SizeName"] = "Please Select";
            //    ds5.Tables[0].Rows.Add(dr);
            //    cmbstatus.DataSource = ds5.Tables[0];
            //    cmbstatus.ValueMember = "id";
            //    cmbstatus.DisplayMember = "SizeName";
            //    cmbstatus.Text = "Please Select";
            //}
            //catch (Exception ex)
            //{


            //}
        }
        //public void fillbranch()
        //{
        //    try
        //    {
        //        //category
        //        DataSet ds6 = new DataSet();
        //       string q6 = "select * from Branch";
        //        ds6 = objCore.funGetDataSet(q6);
        //        DataRow dr = ds6.Tables[0].NewRow();
        //        dr["BranchName"] = "Please Select";
        //        ds6.Tables[0].Rows.Add(dr);
        //        comboBox7.DataSource = ds6.Tables[0];
        //        comboBox7.ValueMember = "id";
        //        comboBox7.DisplayMember = "BranchName";
        //        comboBox7.Text = "Please Select";
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
        //public void fillstore()
        //{
        //    try
        //    {
        //        //category
        //        DataSet ds7 = new DataSet();
        //      string  q7 = "select * from stores where BranchId='"+comboBox7.SelectedValue+"'";
        //        ds7 = objCore.funGetDataSet(q7);
        //        comboBox8.DataSource = ds7.Tables[0];
        //        DataRow dr = ds7.Tables[0].NewRow();
        //        dr["StoreName"] = "Please Select";
        //        ds7.Tables[0].Rows.Add(dr);
        //        comboBox8.ValueMember = "id";
        //        comboBox8.DisplayMember = "StoreName";
        //        if (comboBox7.Text == "Please Select")
        //        {
        //            comboBox8.Text = "Please Select";
        //        }
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
        //public void fillcolor()
        //{
        //    try
        //    {
        //        //category
        //        DataSet ds8 = new DataSet();
        //       string q8 = "select * from Color";
        //        ds8 = objCore.funGetDataSet(q8);
        //        DataRow dr = ds8.Tables[0].NewRow();
        //        dr["Caption"] = "Please Select";
        //        ds8.Tables[0].Rows.Add(dr);
        //        comboBox9.DataSource = ds8.Tables[0];
        //        comboBox9.ValueMember = "id";
        //        comboBox9.DisplayMember = "Caption";
        //        comboBox9.Text = "Please Select";
        //    }
        //    catch (Exception ex)
        //    {


        //    }
        //}
        public void getdata()
        {
            try
            {
                //category
                DataSet ds9 = new DataSet();
                string q9 = "";
                if (richTextBox4.Text == string.Empty || richTextBox4.Text == "Seach Item by Keyword")
                {
                    q9 = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.BarCode, dbo.UOM.UOM, dbo.Type.TypeName, dbo.Category.CategoryName, dbo.Groups.GroupName, dbo.RawItem.Status, dbo.RawItem.PrintBarcode FROM            dbo.UOM RIGHT OUTER JOIN                         dbo.Groups RIGHT OUTER JOIN                         dbo.RawItem ON dbo.Groups.Id = dbo.RawItem.GroupId LEFT OUTER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id ON dbo.UOM.Id = dbo.RawItem.UOMId LEFT OUTER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id order by dbo.RawItem.ItemName";
                }
                else
                {
                    q9 = "SELECT        dbo.RawItem.Id, dbo.RawItem.ItemName, dbo.RawItem.Price, dbo.RawItem.BarCode, dbo.UOM.UOM, dbo.Type.TypeName, dbo.Category.CategoryName, dbo.Groups.GroupName, dbo.RawItem.Status, dbo.RawItem.PrintBarcode FROM            dbo.UOM RIGHT OUTER JOIN                         dbo.Groups RIGHT OUTER JOIN                         dbo.RawItem ON dbo.Groups.Id = dbo.RawItem.GroupId LEFT OUTER JOIN                         dbo.Type ON dbo.RawItem.TypeId = dbo.Type.Id ON dbo.UOM.Id = dbo.RawItem.UOMId LEFT OUTER JOIN                         dbo.Category ON dbo.RawItem.CategoryId = dbo.Category.Id  where dbo.RawItem.ItemName like '%" + richTextBox4.Text + "%' or dbo.RawItem.BarCode like '%" + richTextBox4.Text + "%' or dbo.RawItem.Code like '%" + richTextBox4.Text + "%' or dbo.RawItem.Description like '%" + richTextBox4.Text + "%'  or dbo.Type.TypeName like '%" + richTextBox4.Text + "%'  or dbo.Category.CategoryName like '%" + richTextBox4.Text + "%'  or dbo.Groups.GroupName like '%" + richTextBox4.Text + "%' order by dbo.RawItem.ItemName";
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
               string query = "ALTER TABLE [dbo].[RawItem]  ADD critical varchar(100) NULL ";
               objCore.executeQuery(query);
               query = "ALTER TABLE RawItem DROP CONSTRAINT FK_RawItem_Brands; ";
               objCore.executeQuery(query);
               query = "ALTER TABLE RawItem DROP CONSTRAINT FK_RawItem_Color; ";
               objCore.executeQuery(query);
               query = "ALTER TABLE RawItem DROP CONSTRAINT FK_RawItem_Size;";
               objCore.executeQuery(query);
            }
            catch (Exception ex)
            {


            }
            fillvendor();
            //this.FormBorderStyle = FormBorderStyle.None;
            fillgroups();
            fillcategory();
            filltype();
            //fillbrand();
            filluom();
            fillsize();
           // fillbranch();
           // fillstore();
          //  fillcolor();
            getdata();
           

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
           // fillstore();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }
        public void getinfo(string id)
        {
            try
            {
                DataSet dsinfo = new DataSet();
                string qry = "select * from rawitem where id='" + id + "'";
                dsinfo = objCore.funGetDataSet(qry);
                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                    vButton2.Text = "Update";
                    itemid = id;
                    txtName.Text = dsinfo.Tables[0].Rows[0]["ItemName"].ToString();
                    richTextBox1.Text = dsinfo.Tables[0].Rows[0]["BarCode"].ToString();
                    richTextBox2.Text = dsinfo.Tables[0].Rows[0]["price"].ToString();

                    comboBox1.SelectedValue = dsinfo.Tables[0].Rows[0]["GroupId"].ToString();
                    comboBox2.SelectedValue = dsinfo.Tables[0].Rows[0]["CategoryId"].ToString();
                    comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["TypeId"].ToString();
                   // comboBox4.SelectedValue = dsinfo.Tables[0].Rows[0]["BrandId"].ToString();
                    comboBox5.SelectedValue = dsinfo.Tables[0].Rows[0]["UOMId"].ToString();
                   
                  //  comboBox7.SelectedValue = dsinfo.Tables[0].Rows[0]["BranchId"].ToString();
                   // comboBox8.SelectedValue = dsinfo.Tables[0].Rows[0]["StoreId"].ToString();
                   // comboBox9.SelectedValue = dsinfo.Tables[0].Rows[0]["ColorId"].ToString();
                    vButton2.Text = "Update";

                    txtmin.Text = dsinfo.Tables[0].Rows[0]["MinOrder"].ToString();
                    txtmax.Text = dsinfo.Tables[0].Rows[0]["maxorder"].ToString();
                    
                    string critical = dsinfo.Tables[0].Rows[0]["critical"].ToString();
                    if (critical == "yes")
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                    drpvendor.SelectedValue = dsinfo.Tables[0].Rows[0]["supplierid"].ToString();
                    cmbstatus.SelectedItem = dsinfo.Tables[0].Rows[0]["Status"].ToString();
                    lblloose.Text = dsinfo.Tables[0].Rows[0]["LoosQTY"].ToString();
                    txtpackingname.Text = dsinfo.Tables[0].Rows[0]["Packingname"].ToString();
                    cmbprintcodes.SelectedItem = dsinfo.Tables[0].Rows[0]["PrintBarcode"].ToString();
                }
            }
            catch (Exception ex)
            {
                
                
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
            save();
        }
        protected void save()
        {
            string branchid = "";
            try
            {
                DataSet dsbranch = new DataSet();
                q = "select id from Branch";
                dsbranch = objCore.funGetDataSet(q);
                if (dsbranch.Tables[0].Rows.Count > 0)
                {
                    branchid = dsbranch.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                
            }
            string critical = "";
            try
            {
                if (checkBox1.Checked == true)
                {
                    critical = "yes";
                }
                else
                {
                    critical = "no";
                }
            }
            catch (Exception ex)
            {
                
               
            }
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
                
                if (comboBox5.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid UOM");
                    return;
                }
                if (cmbstatus.Text == "")
                {
                    MessageBox.Show("Please Select Status");
                    return;
                }
                
               
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Item name");
                    return;
                }
                if (drpvendor.Text == "Please Select")
                {
                    MessageBox.Show("Please Select a valid endor");
                    return;
                }
                //DataSet dsbarcode = new DataSet();
                //dsbarcode = objCore.funGetDataSet("select * from barcode where code='" + richTextBox2.Text.Trim() + "'");
                //if (ds.Tables[0].Rows.Count > 0)
                //{

                //}
                //else
                //{
                //    MessageBox.Show("invalid Barcode.");
                //    return;
                //}
                if (vButton2.Text == "Add")
                {
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

                    q = "select * from RawItem where ItemName='" + txtName.Text.Trim() + "' ";
                    DataSet dsdet = new DataSet();
                    dsdet = objCore.funGetDataSet(q);
                    if (dsdet.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Data already exist");
                        return;
                    }
                    string vid = drpvendor.SelectedValue.ToString();
                    if (vid == "")
                    {
                        vid = "0";
                    }
                    q = "insert into RawItem (PrintBarcode,LoosQTY,PackingName,status,critical,Supplierid,MinOrder,maxorder,id,GroupId,CategoryId,TypeId,UOMId,ItemName,BarCode,price,Description) values('" + cmbprintcodes.Text + "','" + lblloose.Text + "','" + txtpackingname.Text + "','" + cmbstatus.Text + "','" + critical + "','" + vid + "','" + txtmin.Text + "','" + txtmax.Text.Trim().Replace("'", "''") + "' ,'" + id + "','" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.SelectedValue + "','" + comboBox5.SelectedValue + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','')";
                    objCore.executeQuery(q);
                    DataSet dss = new DataSet();
                    int idc = 0;
                    dss = objCore.funGetDataSet("select max(id) as id from closing");
                    if (dss.Tables[0].Rows.Count > 0)
                    {
                        string i = dss.Tables[0].Rows[0][0].ToString();
                        if (i == string.Empty)
                        {
                            i = "0";
                        }
                        idc = Convert.ToInt32(i) + 1;
                    }
                    else
                    {
                        idc = 1;
                    }

                    q = "insert into closing (branchid,id,itemid,date,Remaining,Remarks,onlineid) values('" + branchid + "','" + idc + "','" + id + "','" + DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd") + "','0','',0)";
                    objCore.executeQuery(q);
                    getdata();
                    MessageBox.Show("Data Added Successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string vid = drpvendor.SelectedValue.ToString();
                    if (vid == "")
                    {
                        vid = "0";
                    }
                    objCore = new classes.Clsdbcon();
                    q = "update RawItem set PrintBarcode='"+cmbprintcodes.Text+"', PackingName='" + txtpackingname.Text + "',LoosQTY='" + lblloose.Text + "',status='" + cmbstatus.Text + "',critical='" + critical + "',Supplierid='" + vid + "',maxorder='" + txtmax.Text.Trim().Replace("'", "''") + "' ,MinOrder='" + txtmin.Text.Trim().Replace("'", "''") + "' ,ItemName='" + txtName.Text.Trim().Replace("'", "''") + "' , BarCode='" + richTextBox1.Text.Trim().Replace("'", "''") + "', price='" + richTextBox2.Text.Trim().Replace("'", "''") + "' ,Description='' , GroupId='" + comboBox1.SelectedValue + "', CategoryId='" + comboBox2.SelectedValue + "' , TypeId='" + comboBox3.SelectedValue + "', UOMId='" + comboBox5.SelectedValue + "'  where id='" + itemid + "'";
                    objCore.executeQuery(q);
                    getdata();
                    vButton2.Text = "Add";
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void vButton1_Click(object sender, EventArgs e)
        {
            clear();
        }
        protected void clear()
        {
            txtName.Text = string.Empty;
            richTextBox1.Text = string.Empty;
            richTextBox2.Text = string.Empty;
            comboBox1.Text = "Please Select";
            comboBox2.Text = "Please Select";
            comboBox3.Text = "Please Select";
            lblloose.Text = "";
            txtpackingname.Text = "";
            comboBox5.Text = "Please Select";
          //  cmbstatus.Text = "Please Select";
           // comboBox7.Text = "Please Select";
           // comboBox8.Text = "Please Select";
           // comboBox9.Text = "Please Select";
            vButton2.Text = "Add";
            txtmax.Text = "";
            txtmin.Text = "";
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddRawItems_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)      
            {
                save();
            }
            if (e.Control && e.KeyCode == Keys.C)
            {
                clear();
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2)
            {
                try
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    double price = 0;
                    string temp = "0";
                    temp = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    if (temp == "")
                    {
                        temp = "0";
                    }
                    price = Convert.ToDouble(temp);
                    q = "update rawitem set price='" + price + "' where id='" + id + "'";
                    objCore.executeQueryint(q);
                   // getdata();
                }
                catch (Exception ex)
                {
                    
                    
                }
               
            }
            if (e.ColumnIndex == 8)
            {
                try
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                   
                    string temp = "";
                    temp = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();

                    q = "update rawitem set Status='" + temp + "' where id='" + id + "'";
                    objCore.executeQueryint(q);
                    // getdata();
                }
                catch (Exception ex)
                {


                }

            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
