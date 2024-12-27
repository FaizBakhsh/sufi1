using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace POSRetail.Setting
{
    public partial class AddMenuG : Form
    {
       static string fname = "";
        POSRetail.forms.MainForm _frm;
        public AddMenuG(POSRetail.forms.MainForm frm)
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

        private void AddGroups_Load(object sender, EventArgs e)
        {
            try
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from Color";
                ds = objCore.funGetDataSet(q);
                DataRow dr = ds.Tables[0].NewRow();
                dr["Caption"] = "Please Select";

                ds.Tables[0].Rows.Add(dr);
                comboBox1.DataSource = ds.Tables[0];
                comboBox1.ValueMember = "id";
                comboBox1.DisplayMember = "Caption";

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
                        comboBox3.SelectedValue = ds.Tables[0].Rows[0]["FontSize"].ToString();
                        try
                        {
                            comboBox2.SelectedValue = ds.Tables[0].Rows[0]["FontColorId"].ToString();
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

        private void vButton2_Click(object sender, EventArgs e)
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
                    MessageBox.Show("Please Select Activation Status ");
                    return;
                }
                
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                
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
                    if (fname == string.Empty)
                    { }
                    else
                    {
                       
                    }
                    q = "insert into MenuGroup (id,Name,Description,ColorId,Image,FontColorId,FontSize,Status) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + fname.Replace("'", "''") + "','" + comboBox2.SelectedValue.ToString().Replace("'", "''") + "','" + comboBox3.SelectedValue.ToString().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "";
                    if (fname == string.Empty)
                    {
                        q = "update MenuGroup set FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "',Name='" + txtName.Text.Trim().Replace("'", "''") + "' , Description ='" + txtdescription.Text.Trim().Replace("'", "''") + "', ColorId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                    }
                    else
                    {
                        q = "update MenuGroup set FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "', Image='" + fname.Replace("'", "''") + "',  Name='" + txtName.Text.Trim().Replace("'", "''") + "' , Description ='" + txtdescription.Text.Trim().Replace("'", "''") + "', ColorId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";

                    }

                    objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT     dbo.MenuGroup.Id, dbo.MenuGroup.Name, dbo.Color.ColorName, dbo.MenuGroup.Description, dbo.MenuGroup.Status FROM         dbo.MenuGroup INNER JOIN                      dbo.Color ON dbo.MenuGroup.ColorId = dbo.Color.Id");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
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

        private void vButton4_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opFile = new OpenFileDialog();
                opFile.Title = "Select a Image";
                opFile.Filter = "jpg files (*.jpg)|*.jpg|All files (*.*)|*.*";
                string path = Application.StartupPath + "\\Resources\\ButtonIcons\\"; //System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                // string path = System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Resources\";
                string appPath = path; //System.IO.Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName + @"\Resources\Resources\";
                if (Directory.Exists(appPath) == false)
                {
                    Directory.CreateDirectory(appPath);
                }
                if (opFile.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string iName = opFile.SafeFileName;
                        string filepath = opFile.FileName;
                        fname = iName;
                        File.Copy(filepath, appPath + iName);
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
    }
}
