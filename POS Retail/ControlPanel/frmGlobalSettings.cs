using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
 
using System.Text;
using System.Windows.Forms;
using POSRetail.classes;
using System.Data.SqlClient;

namespace POSRetail.ControlPanel
{
    public partial class frmGlobalSettings : Form
    {
        public string flag,ItemNo;
        public int myID;

        public frmGlobalSettings()
        {
            InitializeComponent();
            this.objCore = new Clsdbcon();
            this.itemId = 0;
            this.editMode = 0;
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            //openFileDialog1.Title = "Select Picture";
            //openFileDialog1.Filter = "Image Files (JPEG,GIF,BMP)|*.jpg;*.jpeg;*.gif;*.bmp|JPEG Files(*.jpg;*.jpeg)|*.jpg;*.jpeg|GIF Files(*.gif)|*.gif|BMP Files(*.bmp)|*.bmp";
            //if (openFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    String myFileName = openFileDialog1.FileName;
            //    txtimgLink.Text = myFileName;
            //    picImage.Image = Image.FromFile(openFileDialog1.FileName);
            //}
            //openFileDialog1.Dispose();
        }

        private void frmAddMovie_Load(object sender, EventArgs e)
        {
            try
            {
                SqlDataReader dr = this.objCore.funGetDataReader("Select * from  BUSINESINFO  Where InfoId = 1");
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr["Logo"] != DBNull.Value)
                    {
                        System.IO.MemoryStream photoStream = new System.IO.MemoryStream((byte[])dr["Logo"]);
                        this.pBxPhoto.Image = Image.FromStream(photoStream, true, true);
                    }
                }
                dr.Close();
                this.objCore.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    
        void SaveItems()
        {
            try
            {
                bool photoExists = false;
                System.IO.MemoryStream photoStream = new System.IO.MemoryStream();
                if (this.pBxPhoto.Image != null)
                {
                    photoExists = true;
                    this.pBxPhoto.Image.Save(photoStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
                 SqlConnection con = new SqlConnection();
                con.ConnectionString = this.objCore.getConnectionString();
                con.Open();
                SqlTransaction tran = con.BeginTransaction();
                SqlCommand com = new SqlCommand("", con, tran);
                string command = string.Empty;
                try
                {
                    command = "Update BUSINESINFO Set Logo = @photo Where InfoId = 1";
                    com.CommandText = command;
                    com.Parameters.Add("@photo", SqlDbType.Image);
                    if (photoExists)
                    {
                        com.Parameters["@photo"].Value = photoStream.ToArray();
                    }
                    else
                    {
                        com.Parameters["@photo"].Value = DBNull.Value;
                    }
                    com.ExecuteNonQuery();
                    com.Parameters.Clear();
                    tran.Commit();
                    MessageBox.Show("Settings updated successfully.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void cmdSave_Click(object sender, EventArgs e)
        {
            this.SaveItems();
        }

      
        private void cmdCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void tabPage1_Click(object sender, EventArgs e)
        {

        }      

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.pBxPhoto.Image = null;
        }

        private void cmdBrowsePic_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog opDlg = new OpenFileDialog();
                DialogResult dlg = opDlg.ShowDialog(this);
                if (dlg == DialogResult.Cancel)
                    return;
                string imagePath = opDlg.FileName;

                System.IO.FileInfo flInfo = new System.IO.FileInfo(imagePath);
                string photoType = flInfo.Extension;
                if (photoType == ".jpeg" || photoType == ".jpg" || photoType == ".png" || photoType == ".gif")
                {
                    int photoSize = Convert.ToInt32(flInfo.Length);

                    //Following code checks if the Photo size is greater than 16K Bytes
                    //if (photoSize > 65536)
                    //{
                    //    MessageBox.Show("Can not accept a photo greater than 64K bytes", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    return;
                    //}
                    Size imgSize = new Size(this.pBxPhoto.Width, this.pBxPhoto.Height);
                    System.Drawing.Bitmap bmpPhoto = new Bitmap(Image.FromFile(imagePath), imgSize);
                    this.pBxPhoto.Image = bmpPhoto;
                }
                else
                {
                    MessageBox.Show("Please load image of type JPEG, GIF OR PNG", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        
    }
}