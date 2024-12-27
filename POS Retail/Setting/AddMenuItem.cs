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
    public partial class AddMenuItem : Form
    {
        static string fname = "";
         POSRetail.forms.MainForm _frm;
         public AddMenuItem(POSRetail.forms.MainForm frm)
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
                string q = "select * from Menugroup";
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
                if (editmode == 1)
                {
                    objCore = new classes.Clsdbcon();
                    ds = new DataSet();
                    q = "select * from Menuitem where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                        txtcode.Text = ds.Tables[0].Rows[0]["Code"].ToString();
                        richTextBox1.Text = ds.Tables[0].Rows[0]["BarCode"].ToString();
                        richTextBox2.Text = ds.Tables[0].Rows[0]["Price"].ToString();
                        try
                        {
                            txttime.Text = ds.Tables[0].Rows[0]["Minutes"].ToString();
                            txtalarmtime.Text = ds.Tables[0].Rows[0]["alarmtime"].ToString();
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
                        comboBox3.SelectedValue = ds.Tables[0].Rows[0]["FontSize"].ToString();
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

        private void vButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Menu Item");
                    return;
                }
                if (comboBox1.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Group");
                    return;
                }
                if (txtcode.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Code of Menu Item");
                    return;
                }
                if (richTextBox2.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Price of Menu Item");
                    return;
                }
                if (cmbcolor.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Color");
                    return;
                }
                if (comboBox2.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Font Color");
                    return;
                }
                if (cmbstatus.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select Activation Status ");
                    return;
                }
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
                if (editmode == 0)
                {
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                    string q = "select * from MenuItem where Name='" + txtName.Text.Trim().Replace("'", "''") + "' or  Code='" + txtcode.Text.Replace("'", "''") + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Item Name/Code already exist");
                        return;
                    }
                    q = "insert into MenuItem (id,Name,Code,BarCode,MenuGroupId,Price,status,Image,ColorId,FontColorId,FontSize,Minutes,alarmtime,minuteskdscolor,alarmkdscolor) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtcode.Text.Trim().Replace("'", "''") + "','" + richTextBox1.Text.Trim().Replace("'", "''") + "','" + comboBox1.SelectedValue + "','" + richTextBox2.Text.Trim().Replace("'", "''") + "','" + cmbstatus.Text.Replace("'", "''") + "','" + fname.Replace("'", "''") + "','" + cmbcolor.SelectedValue + "','" + comboBox2.SelectedValue + "','" + comboBox3.Text + "','" + txttime.Text + "','" + txtalarmtime.Text + "','" + textBox1.Text + "','" + textBox2.Text + "')";
                    objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();

                   // obj.getdata("SELECT     dbo.MenuItem.Id,dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId");
                  
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "";
                    if (fname == "")
                    {
                        q = "update MenuItem set minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='"+textBox2.Text+"', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "',  FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "', ColorId='" + cmbcolor.SelectedValue + "', Name='" + txtName.Text.Trim().Replace("'", "''") + "' , Code ='" + txtcode.Text.Trim().Replace("'", "''") + "', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";
                    }
                    else
                    {
                        q = "update MenuItem set minuteskdscolor='" + textBox1.Text + "',alarmkdscolor='" + textBox2.Text + "', Minutes='" + txttime.Text + "',alarmtime='" + txtalarmtime.Text + "', FontColorId='" + comboBox2.SelectedValue + "',FontSize='" + comboBox3.Text + "',Image='" + fname + "', ColorId='" + cmbcolor.SelectedValue + "', Name='" + txtName.Text.Trim().Replace("'", "''") + "' , Code ='" + txtcode.Text.Trim().Replace("'", "''") + "', BarCode ='" + richTextBox1.Text.Trim().Replace("'", "''") + "',price ='" + richTextBox2.Text.Trim().Replace("'", "''") + "', MenuGroupId ='" + comboBox1.SelectedValue + "', Status='" + cmbstatus.Text + "' where id='" + id + "'";
                
                    }
                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                string qq="SELECT     dbo.MenuItem.Id, dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status, dbo.Color.ColorName FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId LEFT OUTER JOIN                      dbo.Color ON dbo.MenuItem.ColorId = dbo.Color.Id";
              //  _frm.getdata("SELECT     dbo.MenuItem.Id,dbo.MenuItem.Name, dbo.MenuItem.Code, dbo.MenuGroup.Name AS MenuGroup, dbo.MenuItem.BarCode, dbo.MenuItem.Price, dbo.MenuItem.Status FROM         dbo.MenuGroup INNER JOIN                      dbo.MenuItem ON dbo.MenuGroup.Id = dbo.MenuItem.MenuGroupId");
                _frm.getdata(qq);
      
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcode.Text = string.Empty;
            txtName.Text = string.Empty;
            richTextBox1.Text = string.Empty;
            richTextBox2.Text = string.Empty;
            comboBox1.Text = "Please Select";
            cmbcolor.Text = "Please Select";
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

        private void textBox1_Click(object sender, EventArgs e)
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
    }
}
