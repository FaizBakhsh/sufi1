using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class AddRuntimeModifier : Form
    {
        public static string itemid = "";
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        POSRestaurant.forms.MainForm _frm;
         public AddRuntimeModifier(POSRestaurant.forms.MainForm frm)
        {
            InitializeComponent();
            this.editmode = 0;
            this.id = "";
            _frm = frm;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            
            
        }
        public void getdata()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsdata = new DataSet();
                string q = "SELECT        TOP (100) PERCENT dbo.RuntimeModifier.id, dbo.RuntimeModifier.name AS ModifierName,dbo.RuntimeModifier.name2, dbo.RawItem.ItemName, dbo.RuntimeModifier.price, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.status, dbo.RuntimeModifier.type, dbo.RuntimeModifier.Stage, dbo.RuntimeModifier.Quantityallowed FROM            dbo.RawItem RIGHT OUTER JOIN                         dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.rawitemid where dbo.RuntimeModifier.menuItemid='" + cmbmenuitem.SelectedValue + "' order by dbo.RuntimeModifier.Stage , dbo.RuntimeModifier.name";
                dsdata = objCore.funGetDataSet(q);
                dataGridView1.DataSource = dsdata.Tables[0];
                dataGridView1.Columns[0].Visible = false;

                pictureBox1.Image =new Bitmap(imagespath + "/" + dsdata.Tables[0].Rows[0]["imageurl"].ToString());
            }
            catch (Exception ex)
            {
                
               
            }

        }
        private void btnclear_Click(object sender, EventArgs e)
        {
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }
      
       
        public void fillrawitem()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds2 = new DataSet();
                string q = "select * from RawItem";
                ds2 = objCore.funGetDataSet(q);

                DataRow dr = ds2.Tables[0].NewRow();
                dr["ItemName"] = "Please Select";

                ds2.Tables[0].Rows.Add(dr);
                comboBox3.DataSource = ds2.Tables[0];
                comboBox3.ValueMember = "id";
                comboBox3.DisplayMember = "ItemName";
                comboBox3.Text = "Please Select";
            }
            catch (Exception ex)
            {
                
               
            }

        }
        public void fillmenuitem()
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet ds2 = new DataSet();
                string q = "select * from MenuItem";
                ds2 = objCore.funGetDataSet(q);

                DataRow dr = ds2.Tables[0].NewRow();
                dr["Name"] = "Please Select";

                ds2.Tables[0].Rows.Add(dr);
                cmbmenuitem.DataSource = ds2.Tables[0];
                cmbmenuitem.ValueMember = "id";
                cmbmenuitem.DisplayMember = "Name";
                cmbmenuitem.Text = "Please Select";
            }
            catch (Exception ex)
            {


            }

        }
        //public void filluom()
        //{
        //    try
        //    {
        //        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        //        DataSet ds3 = new DataSet();
        //        string q = "select * from RawItem where ItemName='" + comboBox3.Text + "'";
        //        ds3 = objCore.funGetDataSet(q);
        //        DataSet dsuom = new DataSet();
        //        dsuom = objCore.funGetDataSet("select * from UOMConversion where UOMId='" + ds3.Tables[0].Rows[0]["UOMId"].ToString() + "'");
        //        DataRow dr = dsuom.Tables[0].NewRow();
        //        dr["UOM"] = "Please Select";

        //        dsuom.Tables[0].Rows.Add(dr);
        //        txtprice.Text = dsuom.Tables[0].Rows[0]["uom"].ToString(); ;
                
        //       // comboBox4.Text = "Please Select";

        //    }
        //    catch (Exception ex)
        //    {
                
               
        //    }
        //}

        string imagespath = "";
        private void AddGroups_Load(object sender, EventArgs e)
        {
            fillmenuitem();
            fillrawitem();
            try
            {
               string q = "select * from deliverytransfer where type='images'";
               DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    imagespath = ds.Tables[0].Rows[0]["server"].ToString();

                }
            }
            catch (Exception ex)
            {


            }
            //if (editmode == 1)
            //{
               
            //    DataSet ds = new DataSet();
            //    string q = "select * from RuntimeModifier where id='" + id + "'";
            //    ds = objCore.funGetDataSet(q);
            //    if (ds.Tables[0].Rows.Count > 0)
            //    {
            //        txtprice.Text = ds.Tables[0].Rows[0]["price"].ToString();
            //        txtname.Text = ds.Tables[0].Rows[0]["name"].ToString();
            //        comboBox1.Text = ds.Tables[0].Rows[0]["Status"].ToString();
            //        cmbmenuitem.SelectedValue = ds.Tables[0].Rows[0]["menuItemid"].ToString();
            //        comboBox3.SelectedValue = ds.Tables[0].Rows[0]["ItemId"].ToString();
                    
                  
            //        vButton2.Text = "Update";
            //    }
            //}
            DataSet dskds = new DataSet();
            string qq = "select * from kds";
            dskds = objCore.funGetDataSet(qq);
            DataRow drck = dskds.Tables[0].NewRow();
            drck["name"] = "Please Select";

            dskds.Tables[0].Rows.Add(drck);
            comboBox4.DataSource = dskds.Tables[0];
            comboBox4.ValueMember = "id";
            comboBox4.DisplayMember = "name";
            comboBox4.Text = "Please Select";

            dskds = new DataSet();
            qq = "select * from kds";
            dskds = objCore.funGetDataSet(qq);
            DataRow drck1 = dskds.Tables[0].NewRow();
            drck1["name"] = "Please Select";

            dskds.Tables[0].Rows.Add(drck1);
            cmbkds2.DataSource = dskds.Tables[0];
            cmbkds2.ValueMember = "id";
            cmbkds2.DisplayMember = "name";
            cmbkds2.Text = "Please Select";

            getdata();
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            //txtname.Text = string.Empty;
            
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
           
        }
        public void getinfo(string id)
        {
            try
            {
                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                DataSet dsinfo = new DataSet();
                string qry = "select * from RuntimeModifier where id='" + id + "'";
                dsinfo = objCore.funGetDataSet(qry);
                if (dsinfo.Tables[0].Rows.Count > 0)
                {
                    comboBox1.SelectedItem = dsinfo.Tables[0].Rows[0]["Status"].ToString();
                    comboBox3.SelectedValue = dsinfo.Tables[0].Rows[0]["rawitemid"].ToString();
                    cmbmenuitem.SelectedValue = dsinfo.Tables[0].Rows[0]["menuItemid"].ToString();
                    txtname.Text = dsinfo.Tables[0].Rows[0]["name"].ToString();
                    txtprice.Text = dsinfo.Tables[0].Rows[0]["price"].ToString();
                    txtquantity.Text = dsinfo.Tables[0].Rows[0]["Quantity"].ToString();
                    try
                    {
                        comboBox4.SelectedValue = dsinfo.Tables[0].Rows[0]["kdsid"].ToString();
                    }
                    catch (Exception ex)
                    {


                    }
                    try
                    {
                        cmbkds2.SelectedValue = dsinfo.Tables[0].Rows[0]["kdsid2"].ToString();
                    }
                    catch (Exception ex)
                    {


                    }
                    vButton2.Text = "Update";
                    itemid = id;
                    cmbtype.SelectedItem = dsinfo.Tables[0].Rows[0]["type"].ToString();
                    cmbstage.SelectedItem = dsinfo.Tables[0].Rows[0]["stage"].ToString();
                    txtallowed.Text = dsinfo.Tables[0].Rows[0]["quantityallowed"].ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
          
           
        }

        private void txtname_TextChanged(object sender, EventArgs e)
        {
            if (txtprice.Text == string.Empty)
            { }
            else
            {
                float Num;
                bool isNum = float.TryParse(txtprice.Text.ToString(), out Num); //c is your variable
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
        protected void save()
        {
            try
            {
                if (cmbmenuitem.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Menu Item");
                    cmbmenuitem.Focus();
                    return;
                }
                if (txtname.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Modifier");
                    txtname.Focus();
                    return;
                }
                if (comboBox3.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select a valid Raw Item");
                    comboBox3.Focus();
                    return;
                }

                if (comboBox1.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    txtprice.Focus();
                    return;
                }
                if (txtquantity.Text.Trim() == string.Empty)
                {
                    MessageBox.Show("Please Enter Name of Quantity");
                    txtquantity.Focus();
                    return;
                }
                if (cmbtype.Text.Trim() == "")
                {
                    MessageBox.Show("Please Select Type");
                    cmbtype.Focus();
                    return;
                }
                if (txtprice.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtprice.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid Price. Only Nymbers are allowed");
                        txtprice.Focus();
                        return;
                    }
                }
                if (txtquantity.Text == string.Empty)
                { }
                else
                {
                    float Num;
                    bool isNum = float.TryParse(txtquantity.Text.ToString(), out Num); //c is your variable
                    if (isNum)
                    {

                    }
                    else
                    {

                        MessageBox.Show("Invalid value. Only Nymbers are allowed");
                        txtquantity.Focus();
                        return;
                    }
                }
                if (comboBox4.Text.Trim() == "Please Select")
                {
                    MessageBox.Show("Please Select KDS");
                    comboBox4.Focus();
                    return;
                }
                string strModified = "";

                try
                {
                    strModified = Convert.ToBase64String(imageData);
                }
                catch (Exception ex)
                {


                }
                string gst = "0";
                DataSet ds = objCore.funGetDataSet("select * from gst");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string i = ds.Tables[0].Rows[0]["gst"].ToString();
                    if (i == string.Empty)
                    {
                        i = "0";
                    }
                    gst = i;
                }
                double gross = Convert.ToDouble(txtprice.Text);

                //float g = float.Parse(gst) + 100;
                //g = g / 100;
                //gross = gross / g;

                double g = gross * float.Parse(gst) / 100;
                g = Math.Round(g, 2);
                gross = gross + g;
                string stage = "1", qtyallowed = "";
                qtyallowed = txtallowed.Text;
                stage = cmbstage.Text;
                if (stage == "")
                {
                    stage = "1";
                }
                if (vButton2.Text == "Add")
                {


                    ds = new DataSet();
                    int id = 0;
                    ds = objCore.funGetDataSet("select max(id) as id from RuntimeModifier");
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
                    string q = "select * from RuntimeModifier where Name='" + txtname.Text.Replace("'", "''") + "' and menuItemid='" + cmbmenuitem.SelectedValue + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                        MessageBox.Show("This Modifier Already Exist ");
                        return;
                    }
                    string fname = id + "rm" + extension;
                    q = "insert into RuntimeModifier (Name2,kdsid2,stage, quantityallowed,grossprice,imageurl,type,menuItemid,id,Name,price,status,rawitemid,Quantity,kdsid) values(N'" + txtname2.Text + "','" + cmbkds2.SelectedValue + "','" + stage + "','" + qtyallowed + "','" + gross + "','" + fname + "','" + cmbtype.Text + "','" + cmbmenuitem.SelectedValue + "','" + id + "','" + txtname.Text.Trim().Replace("'", "''") + "','" + txtprice.Text.Trim().Replace("'", "''") + "','" + comboBox1.Text + "','" + comboBox3.SelectedValue + "','" + txtquantity.Text.Trim().Replace("'", "''") + "','" + comboBox4.SelectedValue + "')";
                    objCore.executeQuery(q);
                    POSRestaurant.forms.MainForm obj = new forms.MainForm();
                    if (imageData != null)
                    {
                        try
                        {
                            System.IO.File.Delete(@imagespath + "\\" + fname);
                        }
                        catch (Exception ex)
                        {

                        }
                        pictureBox1.Image.Save(@imagespath + "\\" + fname);

                    }
                    getdata();
                    MessageBox.Show("Record saved successfully");
                }
                if (vButton2.Text == "Update")
                {
                    string fname = itemid + "rm" + extension;
                    string q = "";
                    if (imageData == null)
                    {
                        q = "update RuntimeModifier set Name2=N'" + txtname2.Text + "', kdsid2='" + cmbkds2.SelectedValue + "',  stage='" + stage + "', quantityallowed='" + qtyallowed + "', grossprice='" + gross + "',type='" + cmbtype.Text + "',menuItemid='" + cmbmenuitem.SelectedValue + "',kdsid='" + comboBox4.SelectedValue + "', Quantity='" + txtquantity.Text.Trim().Replace("'", "''") + "', rawitemid='" + comboBox3.SelectedValue + "' ,status='" + comboBox1.Text + "' ,   price='" + txtprice.Text.Trim().Replace("'", "''") + "',  Name='" + txtname.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                    }
                    else
                    {
                        q = "update RuntimeModifier set Name2=N'" + txtname2.Text + "',kdsid2='" + cmbkds2.SelectedValue + "', stage='" + stage + "', quantityallowed='" + qtyallowed + "', grossprice='" + gross + "',imageurl='" + fname + "',type='" + cmbtype.Text + "',menuItemid='" + cmbmenuitem.SelectedValue + "',kdsid='" + comboBox4.SelectedValue + "', Quantity='" + txtquantity.Text.Trim().Replace("'", "''") + "', rawitemid='" + comboBox3.SelectedValue + "' ,status='" + comboBox1.Text + "' ,   price='" + txtprice.Text.Trim().Replace("'", "''") + "',  Name='" + txtname.Text.Trim().Replace("'", "''") + "' where id='" + itemid + "'";
                        try
                        {
                            System.IO.File.Delete(@imagespath + "\\" + fname);
                        }
                        catch (Exception ex)
                        {

                        }
                        pictureBox1.Image.Save(@imagespath + "\\" + fname);
                    }

                    objCore.executeQuery(q);
                   
                    getdata();
                    MessageBox.Show("Record updated successfully");
                }
                _frm.getdata("SELECT   dbo.RuntimeModifier.id, dbo.RuntimeModifier.name AS ModifierName,dbo.RuntimeModifier.Name2, dbo.RawItem.ItemName, dbo.RuntimeModifier.Quantity, dbo.RuntimeModifier.price,                       dbo.RuntimeModifier.status,dbo.RuntimeModifier.Type, dbo.KDS.Name AS KDS,dbo.RuntimeModifier.Stage,dbo.RuntimeModifier.Quantityallowed     FROM         dbo.RawItem INNER JOIN                      dbo.RuntimeModifier ON dbo.RawItem.Id = dbo.RuntimeModifier.Itemid LEFT OUTER JOIN                      dbo.KDS ON dbo.RuntimeModifier.kdsid = dbo.KDS.Id ORDER BY dbo.RuntimeModifier.id");
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

            txtname.Text = string.Empty;
            txtprice.Text = string.Empty;
            
            comboBox3.Text = "Please Select";
            
            vButton2.Text = "Add";
            getdata();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void vButton5_Click(object sender, EventArgs e)
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

                POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                int indx = dataGridView1.CurrentCell.RowIndex;
                if (indx >= 0)
                {
                    DialogResult msg = MessageBox.Show("Are you sure you want to remove this?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (msg == DialogResult.Yes)
                    {
                        string id = dataGridView1.Rows[indx].Cells[0].Value.ToString();
                        string q = "delete from RuntimeModifier where id='" + id + "'";
                        objCore.executeQuery(q);
                        getdata();

                    }
                }
            }
            catch (Exception ex)
            {


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
                    AddFlavour obj = new AddFlavour();
                    obj.id = id;
                    obj.Show();
                    this.Hide();

                }
            }
            catch (Exception ex)
            {


            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbmenuitem_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();
        }
        public byte[] imageData = null;
        string extension = "";
        private void vButton6_Click_1(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (DialogResult.OK == ofd.ShowDialog())
            {
                FileInfo fInfo = new FileInfo(ofd.FileName);

                extension = Path.GetExtension(ofd.FileName);
                //string fileSavePath = "";
                //File.Copy(ofd.FileName, fileSavePath, true);

                long numBytes = fInfo.Length;

                FileStream fStream = new FileStream(ofd.FileName, FileMode.Open, FileAccess.Read);

                BinaryReader br = new BinaryReader(fStream);

                imageData = br.ReadBytes((int)numBytes);

                pictureBox1.Image = new Bitmap(ofd.FileName); 

            }
        }

        private void AddRuntimeModifier_KeyDown(object sender, KeyEventArgs e)
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

        private void vButton7_Click(object sender, EventArgs e)
        {
            AttachRuntimeModifierSubRecipe obj = new AttachRuntimeModifierSubRecipe();
            obj.Show();
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 3)
            {
                string q = "update RuntimeModifier set price='" + dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                objCore.executeQuery(q);
            }
            if (e.ColumnIndex == 1)
            {
                string q = "update RuntimeModifier set Name='" + dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString() + "' where id='" + dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() + "'";
                objCore.executeQuery(q);
            }
        }
    }
}
