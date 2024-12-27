using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class AddUser : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddUser(POSRestaurant.forms.MainForm frm)
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
        protected void fillbranch()
        {
            DataSet ds = new DataSet();
            string q = "select * from branch where status='Active'";
            ds = objCore.funGetDataSet(q);
            DataRow dr = ds.Tables[0].NewRow();
            
            comboBox1.DataSource = ds.Tables[0];
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "BranchName";
           
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void AddGroups_Load(object sender, EventArgs e)
        {
          
            try
            {
                string query = "ALTER TABLE [dbo].[users]  ADD discountlimit varchar(50) NULL ";

               objCore.executeQuery(query);
            }
            catch (Exception ex)
            {


            }
            fillbranch();
            if (editmode == 1)
            {
               
                DataSet ds = new DataSet();
                string q = "select * from users where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["name"].ToString();
                    txtcnic.Text = ds.Tables[0].Rows[0]["CNICNo"].ToString();
                    txtfathername.Text = ds.Tables[0].Rows[0]["FatherName"].ToString();

                    txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                    txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                    txtcard.Text = ds.Tables[0].Rows[0]["CardNo"].ToString();
                    txtusername.Text = ds.Tables[0].Rows[0]["UserName"].ToString();
                    txtpassword.Text = ds.Tables[0].Rows[0]["Password"].ToString();
                    txtdesignation.Text = ds.Tables[0].Rows[0]["Designation"].ToString();
                    cmbtype.Text = ds.Tables[0].Rows[0]["Usertype"].ToString();
                    vButton2.Text = "Update";
                    try
                    {
                        Byte[] data = new Byte[0];
                        data = (Byte[])(ds.Tables[0].Rows[0]["Signature"]);
                        MemoryStream mem = new MemoryStream(data);
                        pictureBox2.Image = Image.FromStream(mem);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name");
                    return;
                }
                if (txtcnic.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Cnic no");
                    return;
                }
                if (txtphone.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Phone No");
                    return;
                }
                if (txtusername.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Username");
                    return;
                }
                if (txtpassword.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Password");
                    return;
                }
                if (cmbtype.Text.Trim() == "Please Select" || cmbtype.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select user type");
                    return;
                }
                string limit = txtlimit.Text.Trim();
                try
                {
                    if (limit == "")
                    {
                        limit = "0";
                    }
                }
                catch (Exception ex)
                {
                    
                }
                TextBox txt = txtlimit as TextBox;
                try
                {
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
                if (editmode == 0)
                {

                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from Users");
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
                    string q = "select * from Users where name='" + txtName.Text.Trim() + "' and CNICNo='"+txtcnic.Text+"'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("User already exist");
                        return;
                    }
                    if (imageData == null)
                    {
                        q = "insert into Users (branchid,discountlimit,id,Name,FatherName,Phone,CNICNo,Address,Usertype,CardNo,UserName,Password,Designation,uploadstatus) values('" + comboBox1.SelectedValue + "','" + limit + "','" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtfathername.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtcnic.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + cmbtype.Text.Trim().Replace("'", "''") + "','" + txtcard.Text.Trim().Replace("'", "''") + "','" + txtusername.Text.Trim().Replace("'", "''") + "','" + txtpassword.Text.Trim().Replace("'", "''") + "','" + txtdesignation.Text.Trim().Replace("'", "''") + "','Pending')";
                        objCore.executeQuery(q);
                    }
                    else
                    {

                        // string qry = "insert into Users (id,Name,phone,address,logo,WellComeNote,logoname) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtlocation.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "', @ImageData,'" + txtnote.Text.Trim().Replace("'", "''") + "','logo.bmp')";
                        string qry = "insert into Users (Signature,branchid,discountlimit,id,Name,FatherName,Phone,CNICNo,Address,Usertype,CardNo,UserName,Password,Designation,uploadstatus) values(@Signature,'" + comboBox1.SelectedValue + "','" + limit + "','" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtfathername.Text.Trim().Replace("'", "''") + "','" + txtphone.Text.Trim().Replace("'", "''") + "','" + txtcnic.Text.Trim().Replace("'", "''") + "','" + txtaddress.Text.Trim().Replace("'", "''") + "','" + cmbtype.Text.Trim().Replace("'", "''") + "','" + txtcard.Text.Trim().Replace("'", "''") + "','" + txtusername.Text.Trim().Replace("'", "''") + "','" + txtpassword.Text.Trim().Replace("'", "''") + "','" + txtdesignation.Text.Trim().Replace("'", "''") + "','Pending')";

                        string c = objCore.getConnectionString();
                        SqlConnection con = new SqlConnection(c);
                        SqlCommand SqlCom = new SqlCommand(qry, con);

                        SqlCom.Parameters.Add(new SqlParameter("@Signature", (object)imageData));

                        //Open connection and execute insert query.
                        con.Open();
                        int res = SqlCom.ExecuteNonQuery();
                        con.Close();
                        if (res > 0)
                        {
                            MessageBox.Show("Record saved successfully");
                        }
                    }
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                  
                }
                if (editmode == 1)
                {
                    string q = "";
                    if (imageData == null)
                    {
                        q = "update Users set branchid='" + comboBox1.SelectedValue + "',discountlimit='" + limit + "', Designation='" + txtdesignation.Text.Trim().Replace("'", "''") + "' ,Password='" + txtpassword.Text.Trim().Replace("'", "''") + "' ,UserName='" + txtusername.Text.Trim().Replace("'", "''") + "' ,CardNo='" + txtcard.Text.Trim().Replace("'", "''") + "' ,Usertype='" + cmbtype.Text.Trim().Replace("'", "''") + "' ,Address='" + txtaddress.Text.Trim().Replace("'", "''") + "' ,Phone='" + txtphone.Text.Trim().Replace("'", "''") + "' ,Name='" + txtName.Text.Trim().Replace("'", "''") + "' , CNICNo ='" + txtcnic.Text.Trim().Replace("'", "''") + "', FatherName ='" + txtfathername.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                        objCore.executeQuery(q);
                        MessageBox.Show("Record updated successfully");
                    }
                    else
                    {
                        q = "update Users set Signature=@Signature, branchid='" + comboBox1.SelectedValue + "',discountlimit='" + limit + "', Designation='" + txtdesignation.Text.Trim().Replace("'", "''") + "' ,Password='" + txtpassword.Text.Trim().Replace("'", "''") + "' ,UserName='" + txtusername.Text.Trim().Replace("'", "''") + "' ,CardNo='" + txtcard.Text.Trim().Replace("'", "''") + "' ,Usertype='" + cmbtype.Text.Trim().Replace("'", "''") + "' ,Address='" + txtaddress.Text.Trim().Replace("'", "''") + "' ,Phone='" + txtphone.Text.Trim().Replace("'", "''") + "' ,Name='" + txtName.Text.Trim().Replace("'", "''") + "' , CNICNo ='" + txtcnic.Text.Trim().Replace("'", "''") + "', FatherName ='" + txtfathername.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                        string c = objCore.getConnectionString();
                        SqlConnection con = new SqlConnection(c);
                        SqlCommand SqlCom = new SqlCommand(q, con);

                        SqlCom.Parameters.Add(new SqlParameter("@Signature", (object)imageData));

                        //Open connection and execute insert query.
                        con.Open();
                        int res = SqlCom.ExecuteNonQuery();
                        con.Close();
                        if (res > 0)
                        {
                            MessageBox.Show("Record saved successfully");
                        }
                    }
                   
                    
                }
                _frm.getdata("select * from Users");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcnic.Text = string.Empty;
            txtName.Text = string.Empty;
            txtfathername.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtcard.Text = string.Empty;
            txtusername.Text = string.Empty;
            txtpassword.Text = string.Empty;
            txtdesignation.Text = string.Empty;
            txtlimit.Text = string.Empty;
            cmbtype.SelectedText = "Please Select";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtcard_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtcard.Text = txtcard.Text + e.KeyChar.ToString().Trim();
        }

        private void txtcard_TextChanged(object sender, EventArgs e)
        {
            //txtcard.Text=txtcard.Text.Replace("\n", "");
            //txtcard.Text.Reverse();
        }

        private void txtlimit_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = txtlimit as TextBox;
            try
            {
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
        public byte[] imageData = null;
        private void vButton4_Click(object sender, EventArgs e)
        {
            Image File;
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                FileInfo fInfo = new FileInfo(ofd.FileName);
                long numBytes = fInfo.Length;
                FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fStream);
                imageData = br.ReadBytes((int)numBytes);

                File = Image.FromFile(ofd.FileName);
                pictureBox2.Image = File;

            }
        }
    }
}
