using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Globalization;

namespace POSRestaurant.Setting
{
    public partial class AddMenuItem : Form
    {
        static string fname = "";
         POSRestaurant.forms.MainForm _frm;
         POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
         DataSet ds = new DataSet();
         public AddMenuItem(POSRestaurant.forms.MainForm frm)
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
             this.Close();
         }
        static string imagespath = "";
        private void AddGroups_Load(object sender, EventArgs e)
        {
            string q = "";
            try
            {
                q = "select * from deliverytransfer where type='images'";
                ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    imagespath = ds.Tables[0].Rows[0]["server"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                try
                {
                    DataSet dsbranch = new DataSet();
                    q = "select * from Branch where type='Dine In' or type='Take Away'";
                    dsbranch = objCore.funGetDataSet(q);

                    checkedListBox1.DataSource = dsbranch.Tables[0];
                    checkedListBox1.DisplayMember = "Branchname";

                }
                catch (Exception ex)
                {

                }
                DataSet ds = new DataSet();
                q = "select * from Menugroup";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Name";
                comboBox1.Text = "Please Select";

                DataSet ds1 = new DataSet();
                q = "select * from Color";
                ds1 = objCore.funGetDataSet(q);
                DataRow dr1 = ds1.Tables[0].NewRow();
                dr1["Caption"] = "Please Select";

                ds1.Tables[0].Rows.Add(dr1);
                cmbcolor.DataSource = ds1.Tables[0];
                cmbcolor.ValueMember = "id";
                cmbcolor.DisplayMember = "Caption";
                cmbcolor.Text = "Please Select";

                DataSet dsc = new DataSet();
                q = "select * from Color";
                dsc = objCore.funGetDataSet(q);
                DataRow drc = dsc.Tables[0].NewRow();
                drc["Caption"] = "Please Select";

                dsc.Tables[0].Rows.Add(drc);
                comboBox2.DataSource = dsc.Tables[0];
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "Caption";
                comboBox2.Text = "Please Select";
                comboBox3.Text = "Please Select";
                cmbstatus.Text = "Please Select";

                DataSet dskds = new DataSet();
                q = "select * from kds";
                dskds = objCore.funGetDataSet(q);
                DataRow drck = dskds.Tables[0].NewRow();
                drck["name"] = "Please Select";

                dskds.Tables[0].Rows.Add(drck);
                comboBox4.DataSource = dskds.Tables[0];
                comboBox4.ValueMember = "id";
                comboBox4.DisplayMember = "name";

                DataSet dskds2 = new DataSet();
                q = "select * from kds";
                dskds2 = objCore.funGetDataSet(q);
                DataRow drck2 = dskds2.Tables[0].NewRow();
                drck2["name"] = "Please Select";

                dskds2.Tables[0].Rows.Add(drck2);
                cmbkds2.DataSource = dskds2.Tables[0];
                cmbkds2.ValueMember = "id";
                cmbkds2.DisplayMember = "name";

                //comboBox4.Text = "Please Select";
                if (editmode == 1)
                {
                    objCore = new classes.Clsdbcon();
                    ds = new DataSet();
                    q = "select * from Menuitem where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                       
                        txtcode.Text = ds.Tables[0].Rows[0]["code"].ToString();
                        richTextBox1.Text = ds.Tables[0].Rows[0]["BarCode"].ToString();
                        richTextBox2.Text = ds.Tables[0].Rows[0]["Price"].ToString();
                        txttarget.Text = ds.Tables[0].Rows[0]["target"].ToString();
                        txtmodifier.Text = ds.Tables[0].Rows[0]["modifiercount"].ToString();

                        txtproposedmargin.Text = ds.Tables[0].Rows[0]["ProposedMargin"].ToString();
                        txtproposedprice.Text = ds.Tables[0].Rows[0]["proposedprice"].ToString();
                        txtcurentmargin.Text = ds.Tables[0].Rows[0]["currentmargin"].ToString();

                        try
                        {
                            pictureBox1.Image = new Bitmap(imagespath + "/" + ds.Tables[0].Rows[0]["imageurl"].ToString());
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            double gst = 0;
                            DataSet dsq = new DataSet();
                            q = "select * from gst";
                            dsq = objCore.funGetDataSet(q);
                            if (dsq.Tables[0].Rows.Count > 0)
                            {
                                string temp = dsq.Tables[0].Rows[0]["gst"].ToString();
                                if (temp == "")
                                {
                                    temp = "0";
                                }
                                gst = Convert.ToDouble(temp);

                            }
                            if (gst > 0)
                            {
                                gst = gst / 100;

                            }

                            double tax = 0;
                            if (richTextBox2.Text == "")
                            {
                                richTextBox2.Text = "0";
                            }
                            tax = Convert.ToDouble(richTextBox2.Text) * gst;
                            textBox3.Text = Math.Round(tax, 3).ToString();

                            txtprice.Text = Math.Round((Math.Round(tax, 3) + Convert.ToDouble(richTextBox2.Text)), 2).ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            txttime.Text = ds.Tables[0].Rows[0]["Minutes"].ToString();
                            txtalarmtime.Text = ds.Tables[0].Rows[0]["alarmtime"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            dateTimePicker1.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["startdate"].ToString()).ToString("yyyy-MM-dd");
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {

                            dateTimePicker2.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["enddate"].ToString()).ToString("yyyy-MM-dd");
                        }
                        catch (Exception ex)
                        {


                        }

                        textBox1.Text = ds.Tables[0].Rows[0]["minuteskdscolor"].ToString();
                        textBox2.Text = ds.Tables[0].Rows[0]["alarmkdscolor"].ToString();
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["MenuGroupId"].ToString();
                        try
                        {
                            comboBox2.SelectedValue = ds.Tables[0].Rows[0]["FontColorId"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            comboBox4.SelectedValue = ds.Tables[0].Rows[0]["KDSId"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            cmbkds2.SelectedValue = ds.Tables[0].Rows[0]["KDSId2"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        comboBox3.Text = ds.Tables[0].Rows[0]["FontSize"].ToString();
                        try
                        {
                            cmbcolor.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            cmbstatus.Text = ds.Tables[0].Rows[0]["status"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        vButton2.Text = "Update";


                        string[] array = new string[checkedListBox1.Items.Count]; 
                        ds = new DataSet();
                        q = "SELECT        dbo.MenuItemBranch.Id, dbo.MenuItemBranch.Branchid, dbo.MenuItemBranch.MenuItemId, dbo.MenuItemBranch.Status, dbo.Branch.BranchName FROM            dbo.MenuItemBranch INNER JOIN dbo.Branch ON dbo.MenuItemBranch.Branchid = dbo.Branch.Id where dbo.MenuItemBranch.MenuItemId='" + id + "'";
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            int j = 0;
                            foreach (object itemChecked in checkedListBox1.Items)
                            {
                                DataRowView castedItem = itemChecked as DataRowView;
                                string branch = castedItem["BranchName"].ToString();
                                try
                                {
                                    DataTable Result = ds.Tables[0].Select("BranchName = '" + branch + "'").CopyToDataTable();
                                    string temp = Result.Rows[0]["status"].ToString();
                                    if (temp.ToLower() == "yes")
                                    {
                                        array[j] = "yes";
                                        
                                    }
                                    else
                                    {
                                        array[j] = "no";
                                    }
                                }
                                catch (Exception ex)
                                {
                                    array[j] = "no";
                                }
                                j++;
                            }
                            j = 0;
                            foreach (string i in array)
                            {
                                if (i == "yes")
                                {
                                    checkedListBox1.SetItemChecked(j, true);
                                }
                                else
                                {
                                    checkedListBox1.SetItemChecked(j, false);
                                }
                                j++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            checkBox1.Checked = true;
        }

         private void txtdescription_TextChanged(object sender, EventArgs e)
         {

         }

         private void richTextBox2_TextChanged(object sender, EventArgs e)
         {
             if (richTextBox2.Text == string.Empty)
             { }
             else
             {
                 float Num;
                 bool isNum = float.TryParse(richTextBox2.Text.ToString(), out Num); //c is your variable
                 if (isNum)
                 {

                 }
                 else
                 {

                     MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                     return;
                 }
             }
             //foreach (char c in richTextBox2.Text)
             //{
             //    int Num;
             //    bool isNum = int.TryParse(c.ToString(), out Num); //c is your variable
             //    if (isNum)
             //    {

             //    }
             //}
         }

         private void button2_Click(object sender, EventArgs e)
         {

         }
         protected void updatebranches(string id)
         {
             int j = 0;
             foreach (object itemChecked in checkedListBox1.Items)
             {
                 
                 DataRowView castedItem = itemChecked as DataRowView;
                 string branch = castedItem["BranchName"].ToString();
                 CheckBox castedchek = itemChecked as CheckBox;
                 string status = checkedListBox1.GetItemCheckState(j).ToString();;
                 if (status == "Checked")
                 {
                     status = "yes";
                 }
                 else
                 {
                     status = "no";
                 }
                 string q = "SELECT        * from branch where BranchName='" + branch + "'";
                 DataSet ds1 = new DataSet();
                 ds1 = objCore.funGetDataSet(q);
                 if (ds1.Tables[0].Rows.Count > 0)
                 {
                     q = "SELECT        dbo.MenuItemBranch.Id, dbo.MenuItemBranch.Branchid, dbo.MenuItemBranch.MenuItemId, dbo.MenuItemBranch.Status, dbo.Branch.BranchName FROM            dbo.MenuItemBranch INNER JOIN                         dbo.Branch ON dbo.MenuItemBranch.Branchid = dbo.Branch.Id where dbo.Branch.BranchName='" + branch + "' and dbo.MenuItemBranch.MenuItemId='" + id + "'";
                     DataSet ds = new DataSet();
                     ds = objCore.funGetDataSet(q);
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         q = "update MenuItemBranch set status='" + status + "' where id=" + ds.Tables[0].Rows[0]["id"].ToString() + "";
                         objCore.executeQuery(q);
                     }
                     else
                     {
                         if (status == "yes")
                         {
                             q = "insert into MenuItemBranch (Branchid, MenuItemId, Status) values('" + ds1.Tables[0].Rows[0]["id"].ToString() + "','" + id + "','" + status + "')";
                             objCore.executeQuery(q);
                         }
                     }
                 }
                 j++;
             }
         }
         protected void save()
         {
             try
             {
                 if (txtName.Text.Trim() == string.Empty)
                 {
                     MessageBox.Show("Please Enter Name of Menu Item");
                     txtName.Focus();
                     return;
                 }
                 if (comboBox1.Text.Trim() == "Please Select")
                 {
                     MessageBox.Show("Please Select a valid Menu Group");
                     txtName.Focus();
                     return;
                 }
                 if (txtcode.Text.Trim() == string.Empty)
                 {
                     MessageBox.Show("Please Enter Code of Menu Item");
                     comboBox1.Focus();
                     return;
                 }
                 if (richTextBox2.Text.Trim() == string.Empty)
                 {
                     MessageBox.Show("Please Enter Price of Menu Item");
                     txtprice.Focus();
                     return;
                 }
                 if (cmbcolor.Text.Trim() == "Please Select")
                 {
                     MessageBox.Show("Please Select a valid Color");
                     cmbcolor.Focus();
                     return;
                 }
                 if (comboBox2.Text.Trim() == "Please Select")
                 {
                     MessageBox.Show("Please Select a valid Font Color");
                     comboBox2.Focus();
                     return;
                 }
                 if (cmbstatus.Text.Trim() == "Please Select")
                 {
                     MessageBox.Show("Please Select Activation Status ");
                     cmbstatus.Focus();
                     return;
                 }
                 if (comboBox4.Text.Trim() == "Please Select")
                 {
                     MessageBox.Show("Please Select KDS ");
                     comboBox4.Focus();
                     return;
                 }
                 //if (txttime.Text.Trim() == string.Empty)
                 //{
                 //    MessageBox.Show("Please Enter Making Time of Menu Item");
                 //    txttime.Focus();
                 //    return;
                 //}
                 //if (txtalarmtime.Text.Trim() == string.Empty)
                 //{
                 //    MessageBox.Show("Please Enter Alarm Time of Menu Item");
                 //    txtalarmtime.Focus();
                 //    return;
                 //}
                 //if (textBox1.Text.Trim() == string.Empty)
                 //{
                 //    MessageBox.Show("Please Enter Color for Making of Menu Item");

                 //    textBox1.Focus();
                 //    return;
                 //}
                 //if (textBox2.Text.Trim() == string.Empty)
                 //{
                 //    MessageBox.Show("Please Enter Color for Alarm of Menu Item");
                 //    textBox2.Focus();
                 //    return;
                 //}
                 float Num;
                 bool isNum = float.TryParse(richTextBox2.Text.ToString(), out Num); //c is your variable
                 if (isNum)
                 {

                 }
                 else
                 {

                     MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                     return;
                 }
                 if (txtprice2.Text.Length > 0)
                 {
                     isNum = float.TryParse(txtprice2.Text.ToString(), out Num); //c is your variable
                     if (isNum)
                     {

                     }
                     else
                     {

                         MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                         return;
                     }
                 }
                 if (txtprice3.Text.Length > 0)
                 {
                     isNum = float.TryParse(txtprice3.Text.ToString(), out Num); //c is your variable
                     if (isNum)
                     {

                     }
                     else
                     {

                         MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                         return;
                     }
                 }
                 string kdsid2 = "";
                 try
                 {
                     if (cmbkds2.Text == "Please Select")
                     {
                         kdsid2 = "0";
                     }
                     else
                     {
                         kdsid2 = cmbkds2.SelectedValue.ToString();
                     }
                 }
                 catch (Exception ex)
                 {
                     
                 }
                 string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtName.Text); 
                 if (editmode == 0)
                 {
                     // POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                     DataSet ds = new DataSet();
                     int id = 0;
                     ds = objCore.funGetDataSet("select max(id) as id from MenuItem");
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
                     string q = "select * from MenuItem where Name='" + txtName.Text.Trim().Replace("'", "''") + "' and  MenuGroupId='" + comboBox1.SelectedValue + "'";
                     ds = objCore.funGetDataSet(q);
                     if (ds.Tables[0].Rows.Count > 0)
                     {
                         MessageBox.Show("Menu Item Name already exist");
                         return;
                     }
                     
                     if (imageData == null)
                     {
                         if (checkBox2.Checked == true & checkBox3.Checked == true)
                         {
                             q = "insert into MenuItem (code,OptionalModifier,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,startdate,enddate,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'"+txtcode.Text+"','" + cmboptional.Text + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "','" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";
                         }
                         else if (checkBox2.Checked == true)
                         {
                             q = "insert into MenuItem (code,OptionalModifier,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,startdate,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'" + txtcode.Text + "','" + cmboptional.Text + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + dateTimePicker1.Text + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "','" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";

                         }
                         else if (checkBox3.Checked == true)
                         {
                             q = "insert into MenuItem (code,OptionalModifier,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,enddate,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'" + txtcode.Text + "','" + cmboptional.Text + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + dateTimePicker2.Text + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "','" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";

                         }
                         else
                         {
                             q = "insert into MenuItem (code,OptionalModifier,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'" + txtcode.Text + "','" + cmboptional.Text + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "','" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";

                         }
                         objCore.executeQuery(q);
                     }
                     else
                     {
                         string fname = id + "m" + extension;
                         string strModified = "";

                         //try
                         //{
                         //    strModified = Convert.ToBase64String(imageData, 0, imageData.Length);
                         //}
                         //catch (Exception ex)
                         //{


                         //}
                         if (checkBox2.Checked == true & checkBox3.Checked == true)
                         {
                             q = "insert into MenuItem (code,OptionalModifier,price2,price3,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,startdate,enddate,imageurl,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,Image,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'" + txtcode.Text + "','" + cmboptional.Text + "','" + txtprice2.Text.Trim() + "','" + txtprice3.Text.Trim() + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + dateTimePicker1.Text + "','" + dateTimePicker2.Text + "','" + fname + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "',@image,'" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";
                         }
                         else if (checkBox2.Checked == true)
                         {
                             q = "insert into MenuItem (code,OptionalModifier,price2,price3,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,startdate,imageurl,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,Image,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'" + txtcode.Text + "','" + cmboptional.Text + "','" + txtprice2.Text.Trim() + "','" + txtprice3.Text.Trim() + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + dateTimePicker1.Text + "','" + fname + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "',@image,'" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";

                         }
                         else if (checkBox3.Checked == true)
                         {
                             q = "insert into MenuItem (code,OptionalModifier,price2,price3,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,enddate,imageurl,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,Image,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'" + txtcode.Text + "','" + cmboptional.Text + "','" + txtprice2.Text.Trim() + "','" + txtprice3.Text.Trim() + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + dateTimePicker2.Text + "','" + fname + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "',@image,'" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";

                         }
                         else
                         {
                             q = "insert into MenuItem (code,OptionalModifier,price2,price3,kdsid2,ManualQty,grossprice,currentmargin,proposedprice,ProposedMargin,imageurl,modifiercount, Target,id,Name,Code,BarCode,MenuGroupId,Price,status,Image,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor,KDSId) values(N'" + txtcode.Text + "','" + cmboptional.Text + "','" + txtprice2.Text.Trim() + "','" + txtprice3.Text.Trim() + "','" + kdsid2 + "','" + cmbmanual.Text + "','" + txtprice.Text + "','" + txtcurentmargin.Text + "','" + txtproposedprice.Text + "','" + txtproposedmargin.Text + "','" + fname + "','" + txtmodifier.Text + "','" + txttarget.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "','','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "',@image,'" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "','" + comboBox4.SelectedValue + "')";

                         }
                         string c = objCore.getConnectionString();
                         SqlConnection con = new SqlConnection(c);
                         SqlCommand SqlCom = new SqlCommand(q, con);

                         SqlCom.Parameters.Add(new SqlParameter("@image", (object)imageData));

                         con.Open();
                         SqlCom.ExecuteNonQuery();
                         con.Close();
                       
                         try
                         {
                             System.IO.File.Delete(@imagespath + "\\" + fname);
                         }
                         catch (Exception ex)
                         {

                         }
                         pictureBox1.Image.Save(@imagespath + "\\" + id + "m" + extension);


                     }
                     q = "select max(id) as id from menuitem";
                     DataSet dsmenu = new DataSet();
                     dsmenu = objCore.funGetDataSet(q);
                     if (dsmenu.Tables[0].Rows.Count > 0)
                     {
                         updatebranches(dsmenu.Tables[0].Rows[0][0].ToString());
                     }
                     
                     POSRestaurant.forms.MainForm obj = new forms.MainForm();
                     MessageBox.Show("Record saved successfully");
                 }
                 if (editmode == 1)
                 {
                     string q = "";
                     if (imageData == null)
                     {
                         if (checkBox2.Checked == true & checkBox3.Checked == true)
                         {
                             q = "update MenuItem set code=N'" + txtcode.Text + "', OptionalModifier='" + cmboptional.Text.Trim() + "', price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "', kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', startdate='" + dateTimePicker1.Text + "',enddate='" + dateTimePicker2.Text + "',modifiercount='" + txtmodifier.Text + "', Target='" + txttarget.Text + "' , KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "',  FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "', ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";
                         }
                         else if (checkBox2.Checked == true)
                         {
                             q = "update MenuItem set code=N'" + txtcode.Text + "',OptionalModifier='" + cmboptional.Text.Trim() + "',price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "',kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', startdate='" + dateTimePicker1.Text + "',modifiercount='" + txtmodifier.Text + "', Target='" + txttarget.Text + "' , KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "',  FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "', ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                         }
                         else if (checkBox3.Checked == true)
                         {
                             q = "update MenuItem set code=N'" + txtcode.Text + "',OptionalModifier='" + cmboptional.Text.Trim() + "',price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "',kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', enddate='" + dateTimePicker2.Text + "',modifiercount='" + txtmodifier.Text + "', Target='" + txttarget.Text + "' , KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "',  FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "', ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                         }
                         else
                         {
                             q = "update MenuItem set  code=N'" + txtcode.Text + "',OptionalModifier='" + cmboptional.Text.Trim() + "',price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "',kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', Target='" + txttarget.Text + "' ,modifiercount='" + txtmodifier.Text + "', KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "',  FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "', ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                         }
                         objCore.executeQuery(q);
                     }
                     else
                     {
                         string fname = id + "m" + extension;
                         string strModified = "";

                         //try
                         //{
                         //    strModified = Convert.ToBase64String(imageData);
                         //}
                         //catch (Exception ex)
                         //{


                         //}

                         if (checkBox2.Checked == true & checkBox3.Checked == true)
                         {
                             q = "update MenuItem set code=N'" + txtcode.Text + "',OptionalModifier='" + cmboptional.Text.Trim() + "',price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "',kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', startdate='" + dateTimePicker1.Text + "',enddate='" + dateTimePicker2.Text + "', imageurl='" + fname + "', modifiercount='" + txtmodifier.Text + "', Target='" + txttarget.Text + "',KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "', FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "',Image=@image, ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";
                         }
                         else if (checkBox2.Checked == true)
                         {
                             q = "update MenuItem set code=N'" + txtcode.Text + "',OptionalModifier='" + cmboptional.Text.Trim() + "',price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "',kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', startdate='" + dateTimePicker1.Text + "',imageurl='" + fname + "', modifiercount='" + txtmodifier.Text + "', Target='" + txttarget.Text + "',KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "', FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "',Image=@image, ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                         }
                         else if (checkBox3.Checked == true)
                         {
                             q = "update MenuItem set code=N'" + txtcode.Text + "',OptionalModifier='" + cmboptional.Text.Trim() + "',price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "',kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', enddate='" + dateTimePicker2.Text + "',imageurl='" + fname + "', modifiercount='" + txtmodifier.Text + "', Target='" + txttarget.Text + "',KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "', FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "',Image=@image, ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                         }
                         else
                         {
                             q = "update MenuItem set code=N'" + txtcode.Text + "',OptionalModifier='" + cmboptional.Text.Trim() + "',price2='" + txtprice2.Text.Trim() + "',price3='" + txtprice3.Text.Trim() + "',kdsid2='" + kdsid2 + "',ManualQty='" + cmbmanual.Text + "',grossprice='" + txtprice.Text + "',currentmargin='" + txtcurentmargin.Text + "',proposedprice='" + txtproposedprice.Text + "',ProposedMargin='" + txtproposedmargin.Text + "', imageurl='" + fname + "', modifiercount='" + txtmodifier.Text + "', Target='" + txttarget.Text + "',KDSId='" + comboBox4.SelectedValue + "', minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "', FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "',Image=@image, ColorId='" + cmbcolor.SelectedValue + "', Name=N'" + name.Trim().Replace("'", "''") + "' , Code ='', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                         }
                         string c = objCore.getConnectionString();
                         SqlConnection con = new SqlConnection(c);
                         SqlCommand SqlCom = new SqlCommand(q, con);

                         SqlCom.Parameters.Add(new SqlParameter("@image", (object)imageData));

                         con.Open();
                         SqlCom.ExecuteNonQuery();
                         con.Close();
                         
                         try
                         {
                             System.IO.File.Delete(@imagespath + "\\" + fname);
                         }
                         catch (Exception ex)
                         {

                         }

                         pictureBox1.Image.Save(@imagespath + "\\" + fname);
                     }

                     updatebranches(id);
                     MessageBox.Show("Record updated successfully");
                 }
                 string qq = "SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name,dbo.MenuItem.code,  dbo.MenuGroup.Name AS MenuGroup,  dbo.MenuItem.Price,dbo.MenuItem.Price2,dbo.MenuItem.Price3, dbo.MenuItem.Status,                        dbo.KDS.Name AS KDS, dbo.MenuItem.Minutes AS MakingTime, dbo.MenuItem.alarmtime,dbo.MenuItem.OptionalModifier FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId INNER JOIN                      dbo.KDS ON dbo.MenuItem.KDSId = dbo.KDS.Id LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id";
                 //  _frm.getdata("SELECT     dbo.MenuItem.Id,dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId");
                 _frm.getdata(qq);

             }
             catch (Exception ex)
             {

                 MessageBox.Show(ex.Message);
             }
         }
         private void vButton2_Click(object sender, EventArgs e)
         {
             save();
         }
        
         private void vButton1_Click(object sender, EventArgs e)
         {
             txtcode.Text = string.Empty;
             txtName.Text = string.Empty;
             richTextBox1.Text = string.Empty;
             richTextBox2.Text = string.Empty;
             comboBox1.Text = "Please Select";
             cmbcolor.Text = "Please Select";
             editmode = 0;
             vButton2.Text = "Submit";
         }

         private void vButton3_Click(object sender, EventArgs e)
         {
             AddRecipe obj = new AddRecipe(_frm);
             obj.Show();
         }

         private void vButton4_Click(object sender, EventArgs e)
         {
             this.Close();
         }

         private void vButton5_Click(object sender, EventArgs e)
         {
             
         }

         private void vButton6_Click(object sender, EventArgs e)
         {
             try
             {


                 AddFlavour obj = new AddFlavour();

                 obj.Show();
                 this.Hide();


             }
             catch (Exception ex)
             {


             }
         }

         private void textBox1_TextChanged(object sender, EventArgs e)
         {
             
         }

         private void textBox1_Click(object sender, EventArgs e)
         {
             
         }

         private void richTextBox2_TextChanged_1(object sender, EventArgs e)
         {
             TextBox txt = sender as TextBox;
             if (txt.Text == string.Empty)
             { }
             else
             {

                 float Num;
                 bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                 if (isNum)
                 {

                 }
                 else
                 {

                     MessageBox.Show("Invalid value. Only Nymbers are allowed");
                     return;
                 }
             }
             if (checkBox1.Checked == false)
             {
                 updateprice1();
             }
         }

         private void AddMenuItem_Click(object sender, EventArgs e)
         {
             //try
             //{
             //    TextBox txt = sender as TextBox;
             //    DialogResult result = colorDialog1.ShowDialog();
             //    // See if user pressed ok.
             //    if (result == DialogResult.OK)
             //    {
             //        // Set form background to the selected color.
             //        //textBox1.Text = colorDialog1.Color.ToArgb().ToString(); ;
             //        txt.Text = colorDialog1.Color.Name.ToString();
             //    }
             //}
             //catch (Exception ex)
             //{
                 
                
             //}
         }
         private void AddMenuItemcolor_Click(object sender, EventArgs e)
         {
             try
             {
                 TextBox txt = sender as TextBox;
                 DialogResult result = colorDialog1.ShowDialog();
                 // See if user pressed ok.
                 if (result == DialogResult.OK)
                 {
                     // Set form background to the selected color.
                     //textBox1.Text = colorDialog1.Color.ToArgb().ToString(); ;
                     txt.Text = colorDialog1.Color.Name.ToString();
                 }
             }
             catch (Exception ex)
             {


             }
         }
         public byte[] imageData = null;
         string extension = "";
         string picturename = "";
         private void vButton5_Click_1(object sender, EventArgs e)
         {
             OpenFileDialog ofd = new OpenFileDialog();
             if (DialogResult.OK == ofd.ShowDialog())
             {
                 FileInfo fInfo = new FileInfo(ofd.FileName);

                 extension = fInfo.Name;// ofd.FileName;// Path.GetExtension(ofd.FileName);
                 //string fileSavePath = "";
                 //File.Copy(ofd.FileName, fileSavePath, true);

                 long numBytes = fInfo.Length;

                 FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);

                 BinaryReader br = new BinaryReader(fStream);

                 imageData = br.ReadBytes((int)numBytes);

                 pictureBox1.Image = new Bitmap(ofd.FileName); 
                 

             }
         }

         private void textBox1_TextChanged_1(object sender, EventArgs e)
         {

         }

         private void txtprice_TextChanged(object sender, EventArgs e)
         {
             updateprice();
         }
         public void updateprice()
         {
             try
             {
                 TextBox txt = txtprice as TextBox;
                 if (txt.Text == string.Empty)
                 { }
                 else
                 {
                     float Num;
                     bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                     if (isNum)
                     {

                     }
                     else
                     {

                         MessageBox.Show("Invalid value. Only Nymbers are allowed");
                         return;
                     }
                 }
                 //if (checkBox1.Checked == false)
                 //{
                 //    richTextBox2.Text = txtprice.Text;
                 //    textBox3.Text = "0";
                 //    return;
                 //}
                 double gst = 0;
                 DataSet ds = new DataSet();
                 string q = "select * from gst";
                 ds = objCore.funGetDataSet(q);
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     string temp = ds.Tables[0].Rows[0]["gst"].ToString();
                     if (temp == "")
                     {
                         temp = "0";
                     }
                     gst = Convert.ToDouble(temp);

                 }
                 if (gst > 0)
                 {
                     gst = gst + 100;
                     gst = gst / 100;
                     double val = Convert.ToDouble(txt.Text);
                     double val1 = val / gst;
                     richTextBox2.Text = Math.Round(val1, 2).ToString();
                     textBox3.Text = Math.Round((val - val1), 2).ToString();
                 }
                 else
                 {
                     richTextBox2.Text = txtprice.Text;
                     textBox3.Text = "0";
                 }

             }
             catch (Exception ex)
             {

                // MessageBox.Show("tax calculation Error! " + ex.Message);
             }
         }
         public void updateprice1()
         {
             try
             {
                 TextBox txt = richTextBox2 as TextBox;
                 if (txt.Text == string.Empty)
                 { }
                 else
                 {
                     float Num;
                     bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                     if (isNum)
                     {

                     }
                     else
                     {

                         MessageBox.Show("Invalid value. Only Nymbers are allowed");
                         return;
                     }
                 }
                 //if (checkBox1.Checked == false)
                 //{
                 //    richTextBox2.Text = txtprice.Text;
                 //    textBox3.Text = "0";
                 //    return;
                 //}
                 double gst = 0;
                 DataSet ds = new DataSet();
                 string q = "select * from gst";
                 ds = objCore.funGetDataSet(q);
                 if (ds.Tables[0].Rows.Count > 0)
                 {
                     string temp = ds.Tables[0].Rows[0]["gst"].ToString();
                     if (temp == "")
                     {
                         temp = "0";
                     }
                     gst = Convert.ToDouble(temp);

                 }
                 if (gst > 0)
                 {
                     gst = gst ;
                     gst = gst / 100;
                     double val = Convert.ToDouble(txt.Text);
                     double val1 = val * gst;
                     textBox3.Text = Math.Round(val1, 2).ToString();
                     txtprice.Text = Math.Round((val + val1), 2).ToString();
                 }
                 else
                 {
                     txtprice.Text = richTextBox2.Text;
                     //textBox3.Text = "0";
                 }

             }
             catch (Exception ex)
             {

                 // MessageBox.Show("tax calculation Error! " + ex.Message);
             }
         }
         private void label9_Click(object sender, EventArgs e)
         {

         }
         private void textBox3_TextChanged(object sender, EventArgs e)
         {

         }

         private void checkBox1_CheckedChanged(object sender, EventArgs e)
         {
             if (checkBox1.Checked == true)
             {
                 txtprice.ReadOnly = false;
                 richTextBox2.ReadOnly = true;
                 updateprice();
             }
             else
             {
                 richTextBox2.ReadOnly = false;
                 txtprice.ReadOnly = true;
                 updateprice1();
             }
             
         }

         private void vButton7_Click(object sender, EventArgs e)
         {
             AddExtraprice obj = new AddExtraprice();
             obj.Show();
         }

         private void textBox4_TextChanged(object sender, EventArgs e)
         {
             try
             {
                 TextBox txt = sender as TextBox;
                 if (txt.Text == string.Empty)
                 { }
                 else
                 {
                     float Num;
                     bool isNum = float.TryParse(txt.Text.ToString(), out Num); //c is your variable
                     if (isNum)
                     {

                     }
                     else
                     {

                         MessageBox.Show("Invalid value. Only Nymbers are allowed");
                         return;
                     }
                 }
             }
             catch (Exception ex)
             {
                 
                 
             }
         }

         private void AddMenuItem_KeyDown(object sender, KeyEventArgs e)
         {
             if (e.Control && e.KeyCode == Keys.S)
             {
                 save();
             }
             if (e.KeyCode == Keys.Escape)
             {
                 this.Close();
             }
         }

         private void txtprice2_TextChanged(object sender, EventArgs e)
         {
             if (txtprice2.Text.Length > 0)
             {
                 float Num;
                bool isNum = float.TryParse(txtprice2.Text.ToString(), out Num); //c is your variable
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

         private void txtprice3_TextChanged(object sender, EventArgs e)
         {
             if (txtprice3.Text.Length > 0)
             {
                 float Num;
                 bool isNum = float.TryParse(txtprice3.Text.ToString(), out Num); //c is your variable
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
    }
}
