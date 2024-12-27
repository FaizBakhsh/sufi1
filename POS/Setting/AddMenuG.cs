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
    public partial class AddMenuG : Form
    {
       static string fname = "";
        POSRestaurant.forms.MainForm _frm;
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        DataSet ds = new DataSet();
        public AddMenuG(POSRestaurant.forms.MainForm frm)
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
            
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Color";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Caption"] = "Please Select";

                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Caption";


                try
                {
                    q = "select * from branch";
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);

                    
                    cmbbranch.DataSource = ds.Tables[0];
                    cmbbranch.ValueMember = "id";
                    cmbbranch.DisplayMember = "BranchName";

                }
                catch (Exception ex)
                {
                    
                   
                }


                DataSet dsc = new DataSet();
                q = "select * from Color";
                dsc = objCore.funGetDataSet(q);
                DataRow drc = dsc.Tables[0].NewRow();
                drc["Caption"] = "Please Select";

                dsc.Tables[0].Rows.Add(drc);
                comboBox2.DataSource = dsc.Tables[0];
                comboBox2.ValueMember = "id";
                comboBox2.DisplayMember = "Caption";
                comboBox1.Text = "Please Select";
                comboBox2.Text = "Please Select";
                cmbstatus.Text = "Please Select";
                comboBox3.Text = "5";

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


                if (editmode == 1)
                {
                    objCore = new classes.Clsdbcon();
                    ds = new DataSet();
                    q = "select * from MenuGroup where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                        txtdescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                        comboBox1.SelectedValue = ds.Tables[0].Rows[0]["ColorId"].ToString();
                        comboBox3.Text= ds.Tables[0].Rows[0]["FontSize"].ToString();
                        try
                        {
                            txtsub.Text = ds.Tables[0].Rows[0]["SubGroup"].ToString();
                            pictureBox1.Image = new Bitmap(imagespath + "/" + ds.Tables[0].Rows[0]["imageurl"].ToString());
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            comboBox2.SelectedValue = ds.Tables[0].Rows[0]["FontColorId"].ToString();
                        }
                        catch (Exception ex)
                        {
                            
                           
                        }
                        
                        try
                        {
                            cmbstatus.SelectedItem = ds.Tables[0].Rows[0]["status"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        try
                        {
                            comboBox4.SelectedItem = ds.Tables[0].Rows[0]["type"].ToString();
                        }
                        catch (Exception ex)
                        {


                        }
                        vButton2.Text = "Update";
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        protected void save()
        {
            try
            {

                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Menu Group");
                    return;
                }
                if (cmbstatus.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Activation Status");
                    return;
                }

                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(txtName.Text);
                string branchid = cmbbranch.SelectedValue.ToString();
                if (branchid == "")
                {
                    branchid = "0";
                }
                if (editmode == 0)
                {
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from MenuGroup");
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
                    string q = "select * from MenuGroup where Name='" + txtName.Text.Trim().Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Group Name already exist");
                        return;
                    }
                    
                    if (imageData == null)
                    {
                        q = "insert into MenuGroup (branchid, subgroup,type,id,Name,Description,ColorId,FontColorId,FontSize,Status) values('" + branchid + "','" + txtsub.Text + "','" + comboBox4.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "',N'" + txtdescription.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + comboBox2.SelectedValue.ToString().Replace("'", "''") + "','" + comboBox3.Text.ToString().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "')";
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        string strModified = "";
                        string fname = id + "mg" + extension;
                        try
                        {
                            strModified = Convert.ToBase64String(imageData, 0, imageData.Length);
                        }
                        catch (Exception ex)
                        {


                        }
                        q = "insert into MenuGroup (branchid,imageurl,subgroup,type,id,Name,Description,ColorId,Image,FontColorId,FontSize,Status) values('" + branchid + "','" + fname + "','" + txtsub.Text + "','" + comboBox4.Text + "','" + id + "',N'" + name.Trim().Replace("'", "''") + "',N'" + txtdescription.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "',@image,'" + comboBox2.SelectedValue.ToString().Replace("'", "''") + "','" + comboBox3.Text.ToString().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "')";
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

                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "";
                    if (imageData == null)
                    {
                        q = "update MenuGroup set branchid='" + branchid + "',subgroup='" + txtsub.Text + "',type='" + comboBox4.Text + "', FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "',Name=N'" + name.Trim().Replace("'", "''") + "' , Description =N'" + txtdescription.Text.Trim().Replace("'", "''") + "', ColorId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";
                        objCore = new classes.Clsdbcon();
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        string strModified = "";
                        string fname = id + "mg" + extension;
                        try
                        {
                            strModified = Convert.ToBase64String(imageData, 0, imageData.Length);
                        }
                        catch (Exception ex)
                        {


                        }
                        q = "update MenuGroup set branchid='" + branchid + "',subgroup='" + txtsub.Text + "',imageurl='" + fname + "',type='" + comboBox4.Text + "',FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "', Image=@image,  Name=N'" + name.Trim().Replace("'", "''") + "' , Description =N'" + txtdescription.Text.Trim().Replace("'", "''") + "', ColorId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";
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


                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT     dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.Color.ColorName, dbo.MenuGroup.type as KitchenPrint,dbo.MenuGroup.FontSize,  dbo.MenuGroup.Status, dbo.MenuGroup.SubGroup FROM         dbo.MenuGroup INNER JOIN                      dbo.Color ON dbo.MenuGroup.ColorId = dbo.Color.Id");
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
            txtdescription.Text = string.Empty;
            txtName.Text = string.Empty;
            comboBox1.Text = "Please Select";
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public byte[] imageData = null;
        string extension = "";
        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                if (DialogResult.OK == ofd.ShowDialog())
                {
                    FileInfo fInfo = new FileInfo(ofd.FileName);

                    extension = fInfo.Name;// Path.GetExtension(ofd.FileName);
                    //string fileSavePath = "";
                    //File.Copy(ofd.FileName, fileSavePath, true);

                    long numBytes = fInfo.Length;

                    FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);

                    BinaryReader br = new BinaryReader(fStream);

                    imageData = br.ReadBytes((int)numBytes);

                    pictureBox1.Image = new Bitmap(ofd.FileName); 

                }
            }
            catch (Exception ex)
            {
                
               
            }
        }

        private void AddMenuG_KeyDown(object sender, KeyEventArgs e)
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
    }
}
