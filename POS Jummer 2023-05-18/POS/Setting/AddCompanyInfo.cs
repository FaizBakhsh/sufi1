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

using System.Drawing.Imaging;


using Zen.Barcode;

namespace POSRestaurant.Setting
{
    public partial class AddCompanyInfo : Form
    {
        POSRestaurant.forms.MainForm _frm;
        public AddCompanyInfo(POSRestaurant.forms.MainForm frm)
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
            this.BackgroundImage = null;
            try
            {
                if (editmode == 1)
                {
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "select * from CompanyInfo where id='" + id + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                        txtdescription.Text = ds.Tables[0].Rows[0]["address"].ToString();
                        txtlocation.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                        txtnote.Text = ds.Tables[0].Rows[0]["WellComeNote"].ToString();
                        txtlogoname.Text = ds.Tables[0].Rows[0]["logoname"].ToString();
                        vButton2.Text = "Update";
                        string baseDir = System.AppDomain.CurrentDomain.BaseDirectory + "\\" + "logo.bmp";
                        pictureBox1.Image = Image.FromFile(@baseDir);

                        Byte[] data = new Byte[0];
                        data = (Byte[])(ds.Tables[0].Rows[0]["logo"]);
                    
                        MemoryStream mem = new MemoryStream(data);
                        pictureBox2.Image = Image.FromStream(mem);
                    }
                }
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
            try
            {
                if (txtName.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Company");
                    return;
                }
                if (txtdescription.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Contact Nos of Company");
                    return;
                }
                if (txtlocation.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Address of Company");
                    return;
                }
                if (txtnote.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Bill Note");
                    return;
                }
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                if (editmode == 0)
                {

                   
                    DataSet ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from CompanyInfo");
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
                    string q = "select * from CompanyInfo where name='" + txtName.Text.Trim() + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Company Name already exist");
                        return;
                    }

                    string qry = "insert into CompanyInfo (id,Name,phone,address,logo,WellComeNote,logoname) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtlocation.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "', @ImageData,'" + txtnote.Text.Trim().Replace("'", "''") + "','logo.bmp')";

                    string c = objCore.getConnectionString();
                    SqlConnection con = new SqlConnection(c);
                    SqlCommand SqlCom = new SqlCommand(qry, con);
                    
                    SqlCom.Parameters.Add(new SqlParameter("@ImageData", (object)imageData));

                    con.Open();
                    SqlCom.ExecuteNonQuery();
                    con.Close();
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "";
                    if (imageData == null)
                    {
                        q = "update CompanyInfo set logoname='logo.bmp',WellComeNote='" + txtnote.Text.Trim().Replace("'", "''") + "',  Name='" + txtName.Text.Trim().Replace("'", "''") + "' , address ='" + txtdescription.Text.Trim().Replace("'", "''") + "', phone ='" + txtlocation.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        q = "update CompanyInfo set logoname='logo.bmp',WellComeNote='" + txtnote.Text.Trim().Replace("'", "''") + "', logo=@logo, Name='" + txtName.Text.Trim().Replace("'", "''") + "' , address ='" + txtdescription.Text.Trim().Replace("'", "''") + "', phone ='" + txtlocation.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                        string c = objCore.getConnectionString();
                        SqlConnection con = new SqlConnection(c);
                        SqlCommand SqlCom = new SqlCommand(q, con);
                        
                        SqlCom.Parameters.Add(new SqlParameter("@logo", (object)imageData));

                        //Open connection and execute insert query.
                        con.Open();
                        SqlCom.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select * from CompanyInfo");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void vButton1_Click(object sender, EventArgs e)
        {
            //var bitmap = new Bitmap(pictureBox1.Image);
            //var lumianceSsource = new BitmapLuminanceSource(bitmap);
            //var binBitmap = new BinaryBitmap(new HybridBinarizer(lumianceSsource));

            //MultiFormatReader reader = new MultiFormatReader();
            //Result result = null;

            //try
            //{
            //    result = reader.decode(binBitmap);
            //}
            //catch (Exception err)
            //{
            //    // Handle the exceptions, in a way that fits to your application.
            //}


             //GenerateMyQCCode("123");
            txtdescription.Text = string.Empty;
            txtName.Text = string.Empty;
            txtlocation.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void vButton5_Click(object sender, EventArgs e)
        {
            string baseDir = System.AppDomain.CurrentDomain.BaseDirectory;
              
            Image File;
            OpenFileDialog f = new OpenFileDialog();
            f.Filter = "Image files (*.bmp) | *.bmp; ";

            if (f.ShowDialog() == DialogResult.OK)
            {
                File = Image.FromFile(f.FileName);
                pictureBox1.Image = File;
                pictureBox1.Image.Save(baseDir + "\\" + "logo.bmp");
            }
        }
    }
}
