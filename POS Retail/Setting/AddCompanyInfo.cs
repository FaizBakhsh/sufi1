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
namespace POSRetail.Setting
{
    public partial class AddCompanyInfo : Form
    {
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRetail.forms.MainForm _frm;
        public AddCompanyInfo(POSRetail.forms.MainForm frm)
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
            if (editmode == 1)
            {
                POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds = new DataSet();
                string q = "select * from CompanyInfo where id='" + id + "'";
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtName.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                    txtdescription.Text = ds.Tables[0].Rows[0]["address"].ToString();
                    txtlocation.Text = ds.Tables[0].Rows[0]["phone"].ToString();
                    vButton2.Text = "Update";
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
                if (imageData == null)
                {
                    MessageBox.Show("Please Select Logo");
                    return;
                }
                if (editmode == 0)
                {

                    POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
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
                    string qry = "insert into CompanyInfo (id,Name,phone,address,logo) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtlocation.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "', @ImageData)";

                    string c = objCore.getConnectionString();
                    SqlConnection con = new SqlConnection(c);
                    SqlCommand SqlCom = new SqlCommand(qry, con);
                    //We are passing Original Image Path and 
                    //Image byte data as SQL parameters.
                    //SqlCom.Parameters.Add(new SqlParameter("@OriginalPath",(object)txtImagePath.Text));
                    SqlCom.Parameters.Add(new SqlParameter("@ImageData", (object)imageData));

                    //Open connection and execute insert query.
                    con.Open();
                    SqlCom.ExecuteNonQuery();
                    con.Close();

                    //q = "insert into CompanyInfo (id,Name,phone,address,logo) values('" + id + "','" + txtName.Text.Trim().Replace("'", "''") + "','" + txtlocation.Text.Trim().Replace("'", "''") + "','" + txtdescription.Text.Trim().Replace("'", "''") + "','Convert(VARBINARY(MAX),"+ imageData+")')";
                    //objCore.executeQuery(q);
                    POSRetail.forms.MainForm obj = new forms.MainForm();
                    // obj.getdata("select * from Groups", "Group Name");
                    MessageBox.Show("Record saved successfully");
                }
                if (editmode == 1)
                {
                    string q = "";
                    if (imageData == null)
                    {
                        q = "update CompanyInfo set  Name='" + txtName.Text.Trim().Replace("'", "''") + "' , address ='" + txtdescription.Text.Trim().Replace("'", "''") + "', phone ='" + txtlocation.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";

                    }
                    else
                    {
                        q = "update CompanyInfo set logo=@logo, Name='" + txtName.Text.Trim().Replace("'", "''") + "' , address ='" + txtdescription.Text.Trim().Replace("'", "''") + "', phone ='" + txtlocation.Text.Trim().Replace("'", "''") + "' where id='" + id + "'";
                        string c = objCore.getConnectionString();
                        SqlConnection con = new SqlConnection(c);
                        SqlCommand SqlCom = new SqlCommand(q, con);
                        //We are passing Original Image Path and 
                        //Image byte data as SQL parameters.
                        //SqlCom.Parameters.Add(new SqlParameter("@OriginalPath",(object)txtImagePath.Text));
                        SqlCom.Parameters.Add(new SqlParameter("@logo", (object)imageData));

                        //Open connection and execute insert query.
                        con.Open();
                        SqlCom.ExecuteNonQuery();
                        con.Close();
                    }
                     
                   // objCore.executeQuery(q);
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("select  Id, Name, Address, Phone, uploadstatus from CompanyInfo");
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
            txtlocation.Text = string.Empty;
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void save()
        {
            string qry = "insert into CompanyInfo (id,logo) values('5', @ImageData)";
            
           string c= objCore.getConnectionString();
           SqlConnection con = new SqlConnection(c);
           SqlCommand SqlCom = new SqlCommand(qry, con);
            //We are passing Original Image Path and 
            //Image byte data as SQL parameters.
            //SqlCom.Parameters.Add(new SqlParameter("@OriginalPath",(object)txtImagePath.Text));
            SqlCom.Parameters.Add(new SqlParameter("@ImageData", (object)imageData));

            //Open connection and execute insert query.
            con.Open();
            SqlCom.ExecuteNonQuery();
            con.Close();

            //Close form and return to list or images.
            //this.Close();
        }
        public byte[] imageData = null;
        private void vButton4_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
               
                //FileInfo fileInfo = new FileInfo(ofd.FileName);
                //long imageFileLength = fileInfo.Length;
                //FileStream FS = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);

                //imageData = new byte[FS.Length];
                //FS.Read(imageData, 0, Convert.ToInt32(FS.Length));

               // BinaryReader br = new BinaryReader(fs);
               // imageData = br.ReadBytes((int)imageFileLength);

                

                //Use FileInfo object to get file size.
                FileInfo fInfo = new FileInfo(ofd.FileName);
                long numBytes = fInfo.Length;

                //Open FileStream to read file
                FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);

                //Use BinaryReader to read file stream into byte array.
                BinaryReader br = new BinaryReader(fStream);

                //When you use BinaryReader, you need to supply number of bytes 
                //to read from file.
                //In this case we want to read entire file. 
                //So supplying total number of bytes.
                imageData = br.ReadBytes((int)numBytes);
               // save();
            }
        }
    }
}
