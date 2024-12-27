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
namespace POSRestaurant.forms
{
    public partial class AddMovies : Form
    {
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();

        public AddMovies()
        {
            InitializeComponent();
        }
        string mid = "";
        private void vButton2_Click(object sender, EventArgs e)
        {
            if (vButton2.Text=="Save")
            {
                try
                {
                    DataSet ds = new DataSet();
                    string q = "select * from Movies where name='" + vTextBox1.Text.Trim().Replace("'", "''") + "'";
                    ds = objcore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        MessageBox.Show("Movie Name already Exist");
                        return;
                    }
                    ds = new DataSet();
                    int id = 0;
                    ds = objcore.funGetDataSet("select max(id) as id from Movies");
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
                    //q = "insert into Movies (name,img,status) values('"+vTextBox1.Text.Trim().Replace("'","''")+"','"+imageData+"','"+comboBox1.Text+"')";
                    //objcore.executeQuery(q);
                    string qry = "insert into Movies (id,name,img,status) values('" + id + "','" + vTextBox1.Text.Trim().Replace("'", "''") + "', @ImageData,'" + comboBox1.Text + "')";

                    string c = objcore.getConnectionString();
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
                    MessageBox.Show("Movie Added Successfully");
                    getdata();
                    imageData = null;
                }
                catch (Exception ex)
                {

                    MessageBox.Show(ex.Message);
                }
            }
            if (vButton2.Text == "Update")
            {
                string q = "";
                if (imageData == null)
                {
                    q = "update Movies set  Name='" + vTextBox1.Text.Trim().Replace("'", "''") + "' , status ='" + comboBox1.Text + "' where id='" + mid + "'";
                    objcore.executeQuery(q);
                }
                else
                {
                    q = "update Movies set img=@logo, Name='" + vTextBox1.Text.Trim().Replace("'", "''") + "' , status ='" + comboBox1.Text + "', where id='" + mid + "'";
                    string c = objcore.getConnectionString();
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
                getdata();
            }
        }
        public byte[] imageData = null;
        private void vButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {

               
                //Use FileInfo object to get file size.
                FileInfo fInfo = new FileInfo(ofd.FileName);
                long numBytes = fInfo.Length;

                //Open FileStream to read file
                FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);

                //Use BinaryReader to read file stream into byte array.
                BinaryReader br = new BinaryReader(fStream);

               
                imageData = br.ReadBytes((int)numBytes);
                // save();
            }
            
        }
        public void getdata()
        {
            DataSet dsgrid = new DataSet();
            dsgrid = objcore.funGetDataSet("SELECT     id, name, img, status FROM         Movies");
            dataGridView1.DataSource = dsgrid.Tables[0];
            dataGridView1.Columns[0].Visible = false;
        }
        private void AddMovies_Load(object sender, EventArgs e)
        {
            comboBox1.Text = "Active";
            getdata();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            vTextBox1.Text = "";
            imageData = null;
            vButton2.Text = "Save";
        }
        public void getinfo(string id)
        {
            DataSet dsgetinfo = new DataSet();
            dsgetinfo = objcore.funGetDataSet("select * from movies where id='"+id+"'");
            if (dsgetinfo.Tables[0].Rows.Count > 0)
            {
                vTextBox1.Text = dsgetinfo.Tables[0].Rows[0]["name"].ToString();
                comboBox1.Text = dsgetinfo.Tables[0].Rows[0]["status"].ToString();
                vButton2.Text = "Update";
            }
        }
        private void vButton6_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                    mid = id;
                    getinfo(id);

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void vButton5_Click(object sender, EventArgs e)
        {
            try
            {
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    DialogResult dr = MessageBox.Show("Are you sure to delete this?","",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                        string q = "delete from movies where id='"+id+"'";
                        objcore.executeQuery(q);
                        MessageBox.Show("Record Deleted Successfully");
                        getdata();
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
