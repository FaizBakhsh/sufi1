using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class DownloadMenu : Form
    {
        public DownloadMenu()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
       static string cs = "", url = "",imagesurl="",imagespath="",branchid="";
        protected string type()
        {
            string tp = "";
            try
            {
                string q = "select * from deliverytransfer where server='main'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    tp = ds.Tables[0].Rows[0]["type"].ToString();
                    url = ds.Tables[0].Rows[0]["url"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                string q = "select * from branch";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchid = ds.Tables[0].Rows[0]["id"].ToString();
                }
            }
            catch (Exception ex)
            {


            }
            if (tp == "")
            {
                tp = "sql";
            }

            try
            {
                string q = "select * from deliverytransfer where type='images'";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    imagespath = ds.Tables[0].Rows[0]["server"].ToString();
                    imagesurl = ds.Tables[0].Rows[0]["url"].ToString();
                }
            }
            catch (Exception ex)
            {


            }


            return tp;
        }
        private void DownloadMenu_Load(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new System.Data.DataSet();
                ds = objCore.funGetDataSet("select * from SqlServerInfo where type='server'");
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string server = ds.Tables[0].Rows[0]["ServerName"].ToString();
                    string db = ds.Tables[0].Rows[0]["DbName"].ToString();
                    string user = ds.Tables[0].Rows[0]["UserName"].ToString();
                    string password = ds.Tables[0].Rows[0]["Password"].ToString();
                    cs = "Data Source=" + server + ";Initial Catalog=" + db + ";Persist Security Info=True;User ID=" + user + ";Password=" + password + "";
                }
            }
            catch (Exception ex)
            {


            }

            if (type() == "service")
            {
                getmenugroupservice();
            }
            else
            {
                try
                {
                    DataSet ds = new DataSet();
                    SqlConnection connection = new SqlConnection(cs);
                    SqlCommand com;
                    string q = "select id,name from menugroup where status='active'";
                    try
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(ds);
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["name"] = "All";
                        dr["id"] = "0";
                        ds.Tables[0].Rows.Add(dr);
                        comboBox1.DataSource = ds.Tables[0];
                        comboBox1.ValueMember = "id";
                        comboBox1.DisplayMember = "name";
                        comboBox1.Text = "All";
                        getmenu();

                    }
                    catch (Exception ex)
                    {

                        // MessageBox.Show(ex.Message);
                    }


                }
                catch (Exception ex)
                {


                }
            }
        }
        public DataTable dtgroup = new DataTable();
        protected void getmenugroupservice()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("Name", typeof(string));

            try
            {
                string uri = url + "/menugroup.asmx/Getresponse?type=download";
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;

                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<menugroupcls> res = (List<menugroupcls>)JsonConvert.DeserializeObject(result, typeof(List<menugroupcls>));
                    foreach (var item in res)
                    {
                        dt.Rows.Add(item.id, item.name);
                    }
                    DataRow dr = dt.NewRow();
                    dr["name"] = "All";
                    dr["id"] = "0";
                    dt.Rows.Add(dr);
                    comboBox1.DataSource = dt;
                    comboBox1.ValueMember = "id";
                    comboBox1.DisplayMember = "name";
                   
                    getmenuservice();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        protected void getmenuservice()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("menugroupid", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("price", typeof(string));
            string id = comboBox1.SelectedValue.ToString();
            if (comboBox1.Text == "All")
            {
                id = "All";
            }
            try
            {
                string uri = url + "/menuitem.asmx/Getresponse?id=" +id+"&branchid="+branchid;
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    try
                    {
                        string result = readStream.ReadToEnd();
                        List<menuitemcls> res = (List<menuitemcls>)JsonConvert.DeserializeObject(result, typeof(List<menuitemcls>));
                        foreach (var item in res)
                        {
                            dt.Rows.Add(item.id, item.menugroupid, item.name, item.price);
                        }
                    }
                    catch (Exception ex)
                    {
                        
                       
                    }                  
                    dataGridView1.DataSource = dt;
                                      
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void SaveImage(string url)
        {
            try
            {
                url = imagesurl + url;
                WebClient webClient = new WebClient();
               string path = @imagespath; // Create a folder named 'Images' in your root directory
                string fileName = Path.GetFileName(url);

                try
                {
                    System.IO.File.Delete(path + "\\" + fileName);
                }
                catch (Exception ex)
                {
                    
                }


                webClient.DownloadFile(url, path + "\\" + fileName);

            }
            catch (Exception ex)
            {
                
            }
        }
        protected void getmenu()
        {
            string q = "";
            if (comboBox1.Text == "All")
            {
                q = "select * from menuitem where status='active' order by menugroupid,name";
            }
            else
            {
                q = "select * from menuitem where menugroupid='" + comboBox1.SelectedValue + "' and status='active' order by name";
            }
            DataSet dsmenu = new DataSet();
            SqlConnection connection = new SqlConnection(cs);
            SqlCommand com;            
            try
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                connection.Open();
                com = new SqlCommand(q, connection);
                SqlDataAdapter da = new SqlDataAdapter(com);
                da.Fill(dsmenu);                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

            dataGridView1.DataSource = dsmenu.Tables[0];

        }
        protected void downloadbyservice(string type)
        {

            bool chkk = false;

            
            try
            {
                string uri = url + "/DeliveryServices/downloadcolor.asmx/Getresponse";
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<ColorClass> res = (List<ColorClass>)JsonConvert.DeserializeObject(result, typeof(List<ColorClass>));
                    foreach (var item in res)
                    {
                        string q = "select * from Color where id='" + item.Id + "'";
                        DataSet ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update Color set ColorName='" + item.Name + "', Caption='" + item.Caption + "' where id='" + item.Id + "'";
                            objCore.executeQuery(q);

                        }
                        else
                        {
                            q = "insert into color (Id,ColorName,Caption) values ('" + item.Id + "','" + item.Name + "','" + item.Caption + "')";
                            objCore.executeQuery(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }

            try
            {
                string uri = url + "/DeliveryServices/downloadgroup.asmx/Getresponse";
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<GroupClass> res = (List<GroupClass>)JsonConvert.DeserializeObject(result, typeof(List<GroupClass>));
                    foreach (var item in res)
                    {
                        string q = "select * from Groups where id='" + item.Id + "'";
                        DataSet ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update Groups set GroupName='" + item.Name + "' where id='" + item.Id + "'";
                            objCore.executeQuery(q);

                        }
                        else
                        {
                            q = "insert into Groups (Id,GroupName) values ('" + item.Id + "','" + item.Name + "')";
                            objCore.executeQuery(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }

            try
            {
                string uri = url + "/DeliveryServices/downloadcategory.asmx/Getresponse";
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<CategoryClass> res = (List<CategoryClass>)JsonConvert.DeserializeObject(result, typeof(List<CategoryClass>));
                    foreach (var item in res)
                    {
                        string q = "select * from Category where id='" + item.Id + "'";
                        DataSet ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update Category set CategoryName='" + item.Name + "', GroupId='" + item.Groupid + "' where id='" + item.Id + "'";
                            objCore.executeQuery(q);

                        }
                        else
                        {
                            q = "insert into Category (Id,CategoryName,GroupId) values ('" + item.Id + "','" + item.Name + "','" + item.Groupid + "')";
                            objCore.executeQuery(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                string uri = url + "/DeliveryServices/downloadtype.asmx/Getresponse";
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<TypeClass> res = (List<TypeClass>)JsonConvert.DeserializeObject(result, typeof(List<TypeClass>));
                    foreach (var item in res)
                    {
                        string q = "select * from Type where id='" + item.Id + "'";
                        DataSet ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update Type set Type='" + item.Name + "', GroupId='" + item.Groupid + "', CategoryId='" + item.Catid + "' where id='" + item.Id + "'";
                            objCore.executeQuery(q);

                        }
                        else
                        {
                            q = "insert into Type (Id, groupId, CategoryId, TypeName) values ('" + item.Id + "','" + item.Groupid + "','" + item.Catid + "','" + item.Name + "')";
                            objCore.executeQuery(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
            try
            {
                string uri = url + "/DeliveryServices/downloadbrannd.asmx/Getresponse";
                HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                Stream stream1 = response1.GetResponseStream();
                using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                {
                    // Load into XML document
                    string result = readStream.ReadToEnd();
                    List<BrandClass> res = (List<BrandClass>)JsonConvert.DeserializeObject(result, typeof(List<BrandClass>));
                    foreach (var item in res)
                    {
                        string q = "select * from Brands where id='" + item.Id + "'";
                        DataSet ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update Brands set BrandName='" + item.Name + "' where id='" + item.Id + "'";
                            objCore.executeQuery(q);

                        }
                        else
                        {
                            q = "insert into Brands  (Id,BrandName) values ('" + item.Id + "','" + item.Name + "')";
                            objCore.executeQuery(q);
                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }

            try
            {
                try
                {
                    string query = "ALTER TABLE [dbo].[RawItem]  ADD critical varchar(100) NULL ";
                    objCore.executeQuery(query);
                    query = "ALTER TABLE [dbo].[RawItem]  ADD Status varchar(100) NULL ";
                    objCore.executeQuery(query);
                    query = "ALTER TABLE RawItem DROP CONSTRAINT FK_RawItem_Brands; ";
                    objCore.executeQuery(query);
                    query = "ALTER TABLE RawItem DROP CONSTRAINT FK_RawItem_Color; ";
                    objCore.executeQuery(query);
                    query = "ALTER TABLE RawItem DROP CONSTRAINT FK_RawItem_Size;";
                    objCore.executeQuery(query);
                }
                catch (Exception ex)
                {


                }
                DataSet dsrraw = new DataSet();


                try
                {
                    string uri = url + "/DeliveryServices/downloadrawitem.asmx/Getresponse";
                    HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                    HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                    Stream stream1 = response1.GetResponseStream();
                    using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                    {
                        // Load into XML document
                        string result = readStream.ReadToEnd();
                        List<downloadrawitemcls> res = (List<downloadrawitemcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadrawitemcls>));
                        foreach (var item in res)
                        {
                            
                            string q = "select * from RawItem where id='" + item.Id + "'";
                            DataSet ds = new DataSet();
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                q = "update RawItem set Price='" + item.Price + "',status='" + item.Status + "',Supplierid='" + item.Supplierid + "',itemname='" + item.ItemName + "',CategoryId='" + item.CategoryId + "',TypeId='" + item.TypeId + "',GroupId='" + item.GroupId + "',UOMId='" + item.UOMId + "' where id='" + item.Id + "'";
                                objCore.executeQuery(q);
                            }
                            else
                            {
                                q = "insert into RawItem (status,Supplierid,Id, GroupId, CategoryId, TypeId, BrandId, UOMId, SizeId, ColorId,      ItemName,   Price,    MinOrder) values ('" + item.Status + "','" + item.Supplierid + "','" + item.Id + "','" + item.GroupId + "','" + item.CategoryId + "','" + item.TypeId + "','" + item.BrandId + "','" + item.UOMId + "','" + item.SizeId + "','" + item.ColorId + "','" + item.ItemName.Replace("'", "''") + "','" + item.Price + "','" + item.MinOrder + "')";
                                objCore.executeQuery(q);
                                DataSet dss = new DataSet();
                                int idc = 0;
                                dss = objCore.funGetDataSet("select id as id from closing where itemid='" + item.Id + "'");
                                if (dss.Tables[0].Rows.Count > 0)
                                {

                                }
                                else
                                {
                                    dss = new DataSet();
                                    dss = objCore.funGetDataSet("select max(id) as id from closing");
                                    if (dss.Tables[0].Rows.Count > 0)
                                    {
                                        string i = dss.Tables[0].Rows[0][0].ToString();
                                        if (i == string.Empty)
                                        {
                                            i = "0";
                                        }
                                        idc = Convert.ToInt32(i) + 1;
                                    }
                                    else
                                    {
                                        idc = 1;
                                    }
                                    if (branchid == "")
                                    {
                                        getbranchid();
                                    }
                                    q = "insert into closing (branchid,id,itemid,date,Remaining,Remarks,onlineid) values('" + branchid + "','" + idc + "','" + item.Id + "','" + DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd") + "','0','',0)";
                                    objCore.executeQuery(q);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {


                }

                
            }
            catch (Exception ex)
            {


            }


            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = row.Cells[0] as DataGridViewCheckBoxCell;

                if (Convert.ToBoolean(chk.Value) == true || type=="All")
                {
                    chkk = true;
                    string id = row.Cells[1].Value.ToString();
                    string q = "";
                    q = "select * from menugroup where id='" + row.Cells["menugroupid"].Value.ToString() + "'";
                    DataSet ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            string uri = url + "/DeliveryServices/downloadmenugroup.asmx/Getresponse?id=" + row.Cells["menugroupid"].Value.ToString();
                            HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                            Stream stream1 = response1.GetResponseStream();
                            using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                            {
                                // Load into XML document
                                string result = readStream.ReadToEnd();
                                List<downloadmenugroupcls> res = (List<downloadmenugroupcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadmenugroupcls>));
                                foreach (var item in res)
                                {
                                    q = "update menugroup set ColorId='" + item.ColorId + "', FontColorId='" + item.FontColorId + "', imageurl='" + item.image + "', Name='" + item.Name + "', Status='" + item.Status + "',   FontSize='" + item.FontSize + "' where id='" + item.Id + "'";
                                    objCore.executeQuery(q);
                                    if (item.image.Length > 0)
                                    {
                                        SaveImage("/images/menu/" + item.image);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                           // MessageBox.Show(ex.Message);
                        }          
                    }
                    else
                    {
                        try
                        {
                            string uri = url + "/DeliveryServices/downloadmenugroup.asmx/Getresponse?id=" + row.Cells["menugroupid"].Value.ToString();
                            HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                            Stream stream1 = response1.GetResponseStream();
                            using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                            {
                                // Load into XML document
                                string result = readStream.ReadToEnd();
                                List<downloadmenugroupcls> res = (List<downloadmenugroupcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadmenugroupcls>));
                                foreach (var item in res)
                                {
                                    q = "insert into menugroup (imageurl,Id, Name, ColorId, Description, Status,  FontColorId, FontSize,  type, role) values ('" + item.image + "','" + item.Id + "','" + item.Name.Replace("'","''") + "','" + item.ColorId + "','" + item.Description + "','" + item.Status + "','" + item.FontColorId + "','" + item.FontSize + "','" + item.type + "','" + item.role + "')";
                                    objCore.executeQuery(q);
                                    if (item.image.Length > 0)
                                    {
                                        SaveImage("/images/menu/" + item.image);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                          //  MessageBox.Show(ex.Message);
                        }                        
                    }
                    q = "select * from menuitem where id='" + row.Cells["id"].Value.ToString() + "'";
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        try
                        {
                            string uri = url + "/DeliveryServices/downloadmenuitem.asmx/Getresponse?id=" + row.Cells["id"].Value.ToString();
                            HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                            Stream stream1 = response1.GetResponseStream();
                            using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                            {
                                // Load into XML document
                                string result = readStream.ReadToEnd();
                                List<downloadmenuitemcls> res = (List<downloadmenuitemcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadmenuitemcls>));
                                foreach (var item in res)
                                {
                                    q = "update menuitem set grossprice='" + item.Pricegross + "',imageurl='" + item.image + "',startdate='" + item.startdate + "',enddate='" + item.enddate + "',status='" + item.Status + "', Name='" + item.Name.Replace("'", "''") + "',modifiercount='" + item.modifiercount + "',Target='" + item.Target + "', FontSize='" + item.FontSize + "',MenuGroupId='" + item.MenuGroupId + "',ColorId='" + item.ColorId + "',KDSId='" + item.KDSId + "',KDSId2='" + item.KDSId2 + "',FontColorId='" + item.FontColorId + "', Price='" + item.Price + "',Price2='" + item.Price2 + "', submenugroupid='" + item.submenugroupid + "' where id='" + row.Cells["id"].Value.ToString() + "'";
                                    objCore.executeQuery(q);
                                    if (item.image.Length > 0)
                                    {
                                        SaveImage("/images/menu/" + item.image);
                                    }

                                }


                            }
                        }
                        catch (Exception ex)
                        {

                            //  MessageBox.Show(ex.Message);
                        }


                    }
                    else
                    {
                        try
                        {
                            string uri = url + "/DeliveryServices/downloadmenuitem.asmx/Getresponse?id=" + row.Cells["id"].Value.ToString();
                            HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                            Stream stream1 = response1.GetResponseStream();
                            using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                            {
                                // Load into XML document
                                string result = readStream.ReadToEnd();
                                List<downloadmenuitemcls> res = (List<downloadmenuitemcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadmenuitemcls>));
                                foreach (var item in res)
                                {

                                    q = "insert into menuitem (OptionalModifier, Price3,KDSId2,Price2,grossprice,imageurl,startdate,enddate,Id,  Name, MenuGroupId, Price, Status, ColorId, KDSId,  FontColorId, FontSize, submenugroupid,modifiercount,Target ) values ( '" + item.OptionalModifier + "','" + item.Price3 + "','" + item.KDSId2 + "','" + item.Price2 + "','" + item.Pricegross + "', '" + item.image + "','" + item.startdate + "','" + item.enddate + "','" + item.Id + "', '" + item.Name.Replace("'", "''") + "', '" + item.MenuGroupId + "', '" + item.Price + "', '" + item.Status + "', '" + item.ColorId + "', '" + item.KDSId + "', '" + item.FontColorId + "', '" + item.FontSize + "',  '" + item.submenugroupid + "','" + item.modifiercount + "','" + item.Target + "')";
                                    objCore.executeQuery(q);
                                    if (item.image.Length > 0)
                                    {
                                        SaveImage("/images/menu/" + item.image);
                                    }
                                }


                            }
                        }
                        catch (Exception ex)
                        {

                            // MessageBox.Show(ex.Message);
                        }

                    }
                    try
                    {
                        string uri = url + "/DeliveryServices/downloadsubitems.asmx/Getresponse";
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<SubItemsClass> res = (List<SubItemsClass>)JsonConvert.DeserializeObject(result, typeof(List<SubItemsClass>));
                            foreach (var item in res)
                            {
                                q = "select * from SubItems where id='" + item.Id + "'";
                                ds = new DataSet();
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    q = "update SubItems set Name='" + item.name + "', Status='" + item.Status + "' where id='" + item.Id + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                 
                                    q = "SET IDENTITY_INSERT SubItems ON; insert into SubItems (id,Name,Status) values ('" + item.Id + "','" + item.name + "','" + item.Status + "') SET IDENTITY_INSERT SubItems OFF;";
                                    objCore.executeQuery(q);
                                   
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        
                    }
                    try
                    {
                        string uri = url + "/DeliveryServices/downloadsubrecipe.asmx/Getresponse";
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<SubRecipeClass> res = (List<SubRecipeClass>)JsonConvert.DeserializeObject(result, typeof(List<SubRecipeClass>));
                            foreach (var item in res)
                            {
                                q = "select * from SubRecipe where id='" + item.Id + "'";
                                ds = new DataSet();
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    q = "update SubRecipe set ItemId='" + item.ItemId + "', RawItemId='" + item.RawItemId + "', Quantity='" + item.Quantity + "', type='" + item.type + "' where id='" + item.Id + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {

                                    q = "insert into SubRecipe ( Id, ItemId, RawItemId, Quantity,  type) values ('" + item.Id + "','" + item.ItemId + "','" + item.RawItemId + "','" + item.Quantity + "','" + item.type + "') ";
                                    objCore.executeQuery(q);

                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {


                    }
                    try
                    {
                        string uri = url + "/DeliveryServices/downloadattachrecipe.asmx/Getresponse";
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            q = "delete from AttachRecipe ";
                            objCore.executeQuery(q);
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<AttachItemsClass> res = (List<AttachItemsClass>)JsonConvert.DeserializeObject(result, typeof(List<AttachItemsClass>));
                            foreach (var item in res)
                            {
                                q = "select * from AttachRecipe where id='" + item.Id + "'";
                                ds = new DataSet();
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    q = "update AttachRecipe set Menuitemid='" + item.itemid + "', FlavourId='" + item.FlavourId + "', SubItemId='" + item.SubItemId + "', Quantity='" + item.Quantity + "', Type='" + item.Type + "' where id='" + item.Id + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                   
                                    q = "SET IDENTITY_INSERT AttachRecipe ON; insert into AttachRecipe (Id, Menuitemid, FlavourId, SubItemId, Quantity, Type) values ('" + item.Id + "','" + item.itemid + "','" + item.FlavourId + "','" + item.SubItemId + "','" + item.Quantity + "','" + item.Type + "') SET IDENTITY_INSERT AttachRecipe OFF;";
                                   int r= objCore.executeQueryint(q);
                                   if (r == 0)
                                   {

                                   }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {


                    }     
                   


                    try
                    {
                        string uri = url + "/DeliveryServices/downloaddeliveryprices.asmx/Getresponse?id=" + row.Cells["id"].Value.ToString();
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<downloaddeliverypricescls> res = (List<downloaddeliverypricescls>)JsonConvert.DeserializeObject(result, typeof(List<downloaddeliverypricescls>));
                            foreach (var item in res)
                            {
                                string fid = item.Flavourid.ToString();
                                if (fid == "" || fid == "0")
                                {
                                    q = "select * from ExtraPrice where Menuitemid='" + item.Menuitemid + "'";
                                    ds = new DataSet();
                                    ds = objCore.funGetDataSet(q);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        q = "update ExtraPrice set  amount='" + item.amount + "',type='" + item.Type + "' where id='" + item.Id + "'";
                                        objCore.executeQuery(q);
                                    }
                                    else
                                    {
                                        q = "insert into extraprice (Type, amount, Menuitemid ) values('Delivery','" + item.amount + "','" + item.Menuitemid + "')";
                                        objCore.executeQuery(q);
                                    }
                                }
                                else
                                {
                                    q = "select * from ExtraPrice where Menuitemid='" + item.Menuitemid + "' and Flavourid='"+item.Flavourid+"'";
                                    ds = new DataSet();
                                    ds = objCore.funGetDataSet(q);
                                    if (ds.Tables[0].Rows.Count > 0)
                                    {
                                        q = "update ExtraPrice set  Flavourid='" + item.Flavourid + "',amount='" + item.amount + "',type='" + item.Type + "' where id='" + item.Id + "'";
                                        objCore.executeQuery(q);
                                    }
                                    else
                                    {
                                        q = "insert into extraprice (Type, amount, Menuitemid,Flavourid ) values('Delivery','" + item.amount + "','" + item.Menuitemid + "','" + item.Flavourid + "')";
                                        objCore.executeQuery(q);
                                    }
                                }
                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                       // MessageBox.Show(ex.Message);
                    }


                    DataSet dsflavour = new DataSet();
                    DataTable dtf = new DataTable();
                    dtf.Columns.Add("Id", typeof(string));
                    dtf.Columns.Add("MenuGroupId", typeof(string));
                    dtf.Columns.Add("MenuItemId", typeof(string));
                    dtf.Columns.Add("name", typeof(string));
                    dtf.Columns.Add("price", typeof(string));
                    dtf.Columns.Add("kdsid", typeof(string));
                    dtf.Columns.Add("pricegross", typeof(string));
                    dtf.Columns.Add("status", typeof(string));
                    dsflavour.Tables.Add(dtf);
                    try
                    {
                        string uri = url + "/DeliveryServices/downloadflavour.asmx/Getresponse?id=" + row.Cells["id"].Value.ToString();
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<downloadflavourcls> res = (List<downloadflavourcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadflavourcls>));
                            foreach (var item in res)
                            {
                                dsflavour.Tables[0].Rows.Add(item.Id, item.MenuGroupId, item.MenuItemId, item.name, item.price, item.kdsid,item.Pricegross,item.status);                                
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                       // MessageBox.Show(ex.Message);
                    }
                    for (int j = 0; j < dsflavour.Tables[0].Rows.Count; j++)
                    {
                        q = "select * from ModifierFlavour where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update ModifierFlavour set status='" + dsflavour.Tables[0].Rows[j]["status"].ToString() + "',grossprice='" + dsflavour.Tables[0].Rows[j]["pricegross"].ToString() + "',Name='" + dsflavour.Tables[0].Rows[j]["name"].ToString().Replace("'", "''") + "',MenuGroupId='" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "',MenuItemId='" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                            
                        }
                        else
                        {
                            q = "insert into ModifierFlavour (status,grossprice,Id, MenuGroupId, MenuItemId, name, price, kdsid) values ( '" + dsflavour.Tables[0].Rows[j]["status"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["pricegross"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["name"].ToString().Replace("'", "''") + "', '" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "')";
                            objCore.executeQuery(q);
                        }
                    }
                    DataSet dsattach = new DataSet();
                    DataTable dtr = new DataTable();
                    dtr.Columns.Add("Id", typeof(string));
                    dtr.Columns.Add("menuitemid", typeof(string));
                    dtr.Columns.Add("Flavourid", typeof(string));
                    dtr.Columns.Add("attachmenuid", typeof(string));
                    dtr.Columns.Add("attachFlavourid", typeof(string));
                    dtr.Columns.Add("Quantity", typeof(string));
                    dtr.Columns.Add("status", typeof(string));
                    dtr.Columns.Add("Type", typeof(string));
                    dtr.Columns.Add("userecipe", typeof(string));
                    
                    dsattach.Tables.Add(dtr);
                    try
                    {
                        string uri = url + "/DeliveryServices/downloadattach.asmx/Getresponse?type=MenuItem&id=" + row.Cells["id"].Value.ToString();
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<downloadattachcls> res = (List<downloadattachcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadattachcls>));
                            foreach (var item in res)
                            {
                                dsattach.Tables[0].Rows.Add(item.Id, item.menuitemid, item.Flavourid, item.attachmenuid, item.attachFlavourid, item.Quantity, item.status, item.Type, item.userecipe.ToString());
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show(ex.Message);
                    }
                    q = "delete from Attachmenu1 where menuItemid='" + row.Cells["id"].Value.ToString() + "' and (type='MenuItem' or type='' or type is null) ";
                    objCore.executeQuery(q);
                    for (int j = 0; j < dsattach.Tables[0].Rows.Count; j++)
                    {

                        try
                        {
                            if (dsattach.Tables[0].Rows[j]["menuitemid"].ToString().Trim().Length > 0 && dsattach.Tables[0].Rows[j]["menuitemid"].ToString()!="0")
                            {
                                q = "insert into Attachmenu1 (Type, userecipe,menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status) values ( '" + dsattach.Tables[0].Rows[j]["Type"].ToString() + "','" + dsattach.Tables[0].Rows[j]["userecipe"].ToString() + "','" + dsattach.Tables[0].Rows[j]["menuitemid"].ToString() + "','" + dsattach.Tables[0].Rows[j]["Flavourid"].ToString() + "', '" + dsattach.Tables[0].Rows[j]["attachmenuid"].ToString() + "', '" + dsattach.Tables[0].Rows[j]["attachFlavourid"].ToString() + "', '" + dsattach.Tables[0].Rows[j]["Quantity"].ToString() + "', '" + dsattach.Tables[0].Rows[j]["status"].ToString() + "')";
                                objCore.executeQuery(q);
                            }
                        }
                        catch (Exception ex)
                        {


                        }

                    }
                    DataSet dsRuntimeModifier = new DataSet();                   
                    dtr = new DataTable();
                    dtr.Columns.Add("Id", typeof(string));
                    dtr.Columns.Add("name", typeof(string));
                    dtr.Columns.Add("menuItemid", typeof(string));
                    dtr.Columns.Add("price", typeof(string));
                    dtr.Columns.Add("Quantity", typeof(string));
                    dtr.Columns.Add("status", typeof(string));
                    dtr.Columns.Add("kdsid", typeof(string));
                    dtr.Columns.Add("rawitemid", typeof(string));
                    dtr.Columns.Add("type", typeof(string));
                    dtr.Columns.Add("image", typeof(string));
                    dtr.Columns.Add("pricegross", typeof(string));
                    dtr.Columns.Add("kdsid2", typeof(string));
                    dtr.Columns.Add("Necessary", typeof(string));
                    dtr.Columns.Add("stage", typeof(string));
                    dtr.Columns.Add("quantityallowed", typeof(string));
                    dsRuntimeModifier.Tables.Add(dtr);
                    try
                    {
                        string uri = url + "/DeliveryServices/downloadruntime.asmx/Getresponse?id=" + row.Cells["id"].Value.ToString();
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<downloadruntimecls> res = (List<downloadruntimecls>)JsonConvert.DeserializeObject(result, typeof(List<downloadruntimecls>));
                            foreach (var item in res)
                            {
                                dsRuntimeModifier.Tables[0].Rows.Add(item.Id, item.name, item.menuItemid, item.price, item.Quantity, item.status, item.kdsid, item.rawitemid, item.type, item.image, item.Pricegross, item.kdsid2, item.Necessary, item.stage, item.quantityallowed);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                    }                    
                    q = "delete from RuntimeModifier where menuItemid='" + row.Cells["id"].Value.ToString() + "'";
                    objCore.executeQuery(q);
                    for (int j = 0; j < dsRuntimeModifier.Tables[0].Rows.Count; j++)
                    {

                        //try
                        //{
                        //    DataSet dsrraw = new DataSet();


                        //    dtr = new DataTable();
                        //    dtr.Columns.Add("Id", typeof(string));
                        //    dtr.Columns.Add("GroupId", typeof(string));
                        //    dtr.Columns.Add("CategoryId", typeof(string));
                        //    dtr.Columns.Add("TypeId", typeof(string));
                        //    dtr.Columns.Add("BrandId", typeof(string));
                        //    dtr.Columns.Add("UOMId", typeof(string));
                        //    dtr.Columns.Add("SizeId", typeof(string));
                        //    dtr.Columns.Add("ColorId", typeof(string));
                        //    dtr.Columns.Add("ItemName", typeof(string));
                        //    dtr.Columns.Add("Price", typeof(string));
                        //    dtr.Columns.Add("MinOrder", typeof(string));

                        //    dsrraw.Tables.Add(dtr);
                        //    try
                        //    {
                        //        string uri = url + "/DeliveryServices/downloadrawitem.asmx/Getresponse?id=" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString();
                        //        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        //        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        //        Stream stream1 = response1.GetResponseStream();
                        //        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        //        {
                        //            // Load into XML document
                        //            string result = readStream.ReadToEnd();
                        //            List<downloadrawitemcls> res = (List<downloadrawitemcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadrawitemcls>));
                        //            foreach (var item in res)
                        //            {
                        //                dsrraw.Tables[0].Rows.Add(item.Id, item.GroupId, item.CategoryId, item.TypeId, item.BrandId, item.UOMId, item.SizeId, item.ColorId, item.ItemName, item.Price, item.MinOrder);
                        //            }
                        //        }
                        //    }
                        //    catch (Exception ex)
                        //    {


                        //    }

                        //    for (int k = 0; k < dsrraw.Tables[0].Rows.Count; k++)
                        //    {
                        //        q = "select * from RawItem where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
                        //        ds = new DataSet();
                        //        ds = objCore.funGetDataSet(q);
                        //        if (ds.Tables[0].Rows.Count > 0)
                        //        {
                        //            q = "update RawItem set itemname='" + dsrraw.Tables[0].Rows[k]["itemname"].ToString().Replace("'", "''") + "',CategoryId='" + dsrraw.Tables[0].Rows[k]["CategoryId"].ToString() + "',TypeId='" + dsrraw.Tables[0].Rows[k]["TypeId"].ToString() + "',GroupId='" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "',UOMId='" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "',price='" + dsrraw.Tables[0].Rows[k]["price"].ToString() + "' where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
                        //            objCore.executeQuery(q);
                        //        }
                        //        else
                        //        {
                        //            q = "insert into RawItem (Id, GroupId, CategoryId, TypeId, BrandId, UOMId, SizeId, ColorId,      ItemName,   Price,    MinOrder) values ('" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["CategoryId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["TypeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BrandId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["SizeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ColorId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ItemName"].ToString().Replace("'", "''") + "','" + dsrraw.Tables[0].Rows[k]["Price"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["MinOrder"].ToString() + "')";
                        //            objCore.executeQuery(q);
                        //            DataSet dss = new DataSet();
                        //            int idc = 0;
                        //            dss = objCore.funGetDataSet("select max(id) as id from closing");
                        //            if (dss.Tables[0].Rows.Count > 0)
                        //            {
                        //                string i = dss.Tables[0].Rows[0][0].ToString();
                        //                if (i == string.Empty)
                        //                {
                        //                    i = "0";
                        //                }
                        //                idc = Convert.ToInt32(i) + 1;
                        //            }
                        //            else
                        //            {
                        //                idc = 1;
                        //            }
                        //            if (branchid == "")
                        //            {
                        //                getbranchid();
                        //            }
                        //            q = "insert into closing (branchid,id,itemid,date,Remaining,Remarks,onlineid) values('" + branchid + "','" + idc + "','" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "','" + DateTime.Now.AddYears(-2).ToString("yyyy-MM-dd") + "','0','',0)";
                        //            objCore.executeQuery(q);
                        //        }
                        //    }
                        //}
                        //catch (Exception ex)
                        //{


                        //}


                        q = "select * from RuntimeModifier where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update RuntimeModifier set stage='" + dsRuntimeModifier.Tables[0].Rows[j]["stage"].ToString() + "',quantityallowed='" + dsRuntimeModifier.Tables[0].Rows[j]["quantityallowed"].ToString() + "',Necessary='" + dsRuntimeModifier.Tables[0].Rows[j]["Necessary"].ToString() + "',kdsid2='" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid2"].ToString() + "',grossprice='" + dsRuntimeModifier.Tables[0].Rows[j]["pricegross"].ToString() + "',imageurl='" + dsRuntimeModifier.Tables[0].Rows[j]["image"].ToString() + "',type='" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "',Name='" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString().Replace("'", "''") + "',Quantity='" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "',status='" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "',rawitemid='" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "',MenuItemId='" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                            SaveImage("/images/menu/" + dsRuntimeModifier.Tables[0].Rows[j]["image"].ToString());
                            if (dsRuntimeModifier.Tables[0].Rows[j]["image"].ToString().Length > 0)
                            {
                                SaveImage("/images/menu/" + dsRuntimeModifier.Tables[0].Rows[j]["image"].ToString());
                            }
                        }
                        else
                        {
                            q = "insert into RuntimeModifier (stage,quantityallowed,Necessary,kdsid2,grossprice,imageurl,type,id, name, menuItemid, price, Quantity, status,  kdsid,rawitemid) values ('" + dsRuntimeModifier.Tables[0].Rows[j]["stage"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["quantityallowed"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["Necessary"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid2"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["pricegross"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["image"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString().Replace("'", "''") + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "')";
                            objCore.executeQuery(q);
                            if (dsRuntimeModifier.Tables[0].Rows[j]["image"].ToString().Length > 0)
                            {
                                SaveImage("/images/menu/" + dsRuntimeModifier.Tables[0].Rows[j]["image"].ToString());
                            }
                        }

                        dsattach = new DataSet();
                        dtr = new DataTable();
                        dtr.Columns.Add("Id", typeof(string));
                        dtr.Columns.Add("menuitemid", typeof(string));
                        dtr.Columns.Add("Flavourid", typeof(string));
                        dtr.Columns.Add("attachmenuid", typeof(string));
                        dtr.Columns.Add("attachFlavourid", typeof(string));
                        dtr.Columns.Add("Quantity", typeof(string));
                        dtr.Columns.Add("status", typeof(string));
                        dtr.Columns.Add("Type", typeof(string));
                        dtr.Columns.Add("userecipe", typeof(string));

                        dsattach.Tables.Add(dtr);
                        try
                        {
                            string uri = url + "/DeliveryServices/downloadattach.asmx/Getresponse?type=RuntimeModifier&id=" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString();
                            HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                            HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                            Stream stream1 = response1.GetResponseStream();
                            using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                            {
                                // Load into XML document
                                string result = readStream.ReadToEnd();
                                List<downloadattachcls> res = (List<downloadattachcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadattachcls>));
                                foreach (var item in res)
                                {
                                    dsattach.Tables[0].Rows.Add(item.Id, item.menuitemid, item.Flavourid, item.attachmenuid, item.attachFlavourid, item.Quantity, item.status, item.Type, item.userecipe);
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                           // MessageBox.Show(ex.Message);
                        }
                        q = "delete from Attachmenu1 where menuItemid='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "' and type='RuntimeModifier'";
                        objCore.executeQuery(q);
                        for (int k = 0; k < dsattach.Tables[0].Rows.Count; k++)
                        {

                            try
                            {
                                if (dsattach.Tables[0].Rows[k]["menuitemid"].ToString().Trim().Length > 0 && dsattach.Tables[0].Rows[k]["menuitemid"].ToString() != "0")
                                {
                                    q = "insert into Attachmenu1 (Type, userecipe,menuitemid, Flavourid, attachmenuid, attachFlavourid, Quantity, status) values ( '" + dsattach.Tables[0].Rows[k]["Type"].ToString() + "','" + dsattach.Tables[0].Rows[k]["userecipe"].ToString() + "','" + dsattach.Tables[0].Rows[k]["menuitemid"].ToString() + "','" + dsattach.Tables[0].Rows[k]["Flavourid"].ToString() + "', '" + dsattach.Tables[0].Rows[k]["attachmenuid"].ToString() + "', '" + dsattach.Tables[0].Rows[k]["attachFlavourid"].ToString() + "', '" + dsattach.Tables[0].Rows[k]["Quantity"].ToString() + "', '" + dsattach.Tables[0].Rows[k]["status"].ToString() + "')";
                                    objCore.executeQuery(q);
                                }
                            }
                            catch (Exception ex)
                            {


                            }

                        }


                    }

                    DataSet dsreceipi = new DataSet();
                    dtr = new DataTable();
                    dtr.Columns.Add("Id", typeof(string));
                    dtr.Columns.Add("MenuItemId", typeof(string));
                    dtr.Columns.Add("RawItemId", typeof(string));
                    dtr.Columns.Add("UOMCId", typeof(string));
                    dtr.Columns.Add("Quantity", typeof(string));
                    dtr.Columns.Add("modifierid", typeof(string));
                    dtr.Columns.Add("type", typeof(string));
                    dsreceipi.Tables.Add(dtr);
                    try
                    {
                        string uri = url + "/DeliveryServices/downloadrecipe.asmx/Getresponse?id=" + row.Cells["id"].Value.ToString();
                        HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                        HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                        Stream stream1 = response1.GetResponseStream();
                        using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                        {
                            // Load into XML document
                            string result = readStream.ReadToEnd();
                            List<downloadrecipecls> res = (List<downloadrecipecls>)JsonConvert.DeserializeObject(result, typeof(List<downloadrecipecls>));
                            foreach (var item in res)
                            {
                                dsreceipi.Tables[0].Rows.Add(item.Id, item.MenuItemId, item.RawItemId, item.UOMCId, item.Quantity, item.modifierid, item.type);
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        //MessageBox.Show(ex.Message);
                    }
                    q = "delete from  Recipe where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
                    objCore.executeQuery(q);
                    for (int j = 0; j < dsreceipi.Tables[0].Rows.Count; j++)
                    {

                        try
                        {
                            DataSet dsrraw = new DataSet();


                            dtr = new DataTable();
                            dtr.Columns.Add("Id", typeof(string));
                            dtr.Columns.Add("GroupId", typeof(string));
                            dtr.Columns.Add("CategoryId", typeof(string));
                            dtr.Columns.Add("TypeId", typeof(string));
                            dtr.Columns.Add("BrandId", typeof(string));
                            dtr.Columns.Add("UOMId", typeof(string));
                            dtr.Columns.Add("SizeId", typeof(string));
                            dtr.Columns.Add("ColorId", typeof(string));
                            dtr.Columns.Add("ItemName", typeof(string));
                            dtr.Columns.Add("Price", typeof(string));
                            dtr.Columns.Add("MinOrder", typeof(string));
                            dsrraw.Tables.Add(dtr);
                            //try
                            //{
                            //    string uri = url + "/DeliveryServices/downloadrawitem.asmx/Getresponse?id=" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString();
                            //    HttpWebRequest request1 = WebRequest.Create(uri) as HttpWebRequest;
                            //    HttpWebResponse response1 = request1.GetResponse() as HttpWebResponse;
                            //    Stream stream1 = response1.GetResponseStream();
                            //    using (StreamReader readStream = new StreamReader(stream1, Encoding.UTF8))
                            //    {
                            //        // Load into XML document
                            //        string result = readStream.ReadToEnd();
                            //        List<downloadrawitemcls> res = (List<downloadrawitemcls>)JsonConvert.DeserializeObject(result, typeof(List<downloadrawitemcls>));
                            //        foreach (var item in res)
                            //        {
                            //            dsrraw.Tables[0].Rows.Add(item.Id, item.GroupId, item.CategoryId, item.TypeId, item.BrandId, item.UOMId, item.SizeId, item.ColorId, item.ItemName, item.Price, item.MinOrder);
                            //        }
                            //    }
                            //}
                            //catch (Exception ex)
                            //{


                            //}

                            //    for (int k = 0; k < dsrraw.Tables[0].Rows.Count; k++)
                            //    {
                            //        q = "select * from RawItem where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
                            //        ds = new DataSet();
                            //        ds = objCore.funGetDataSet(q);
                            //        if (ds.Tables[0].Rows.Count > 0)
                            //        {
                            //            q = "update RawItem set itemname='" + dsrraw.Tables[0].Rows[k]["itemname"].ToString() + "',CategoryId='" + dsrraw.Tables[0].Rows[k]["CategoryId"].ToString() + "',TypeId='" + dsrraw.Tables[0].Rows[k]["TypeId"].ToString() + "',GroupId='" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "',UOMId='" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "',price='" + dsrraw.Tables[0].Rows[k]["price"].ToString() + "' where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
                            //            objCore.executeQuery(q);
                            //        }
                            //        else
                            //        {
                            //            q = "insert into RawItem (Id, GroupId, CategoryId, TypeId, BrandId, UOMId, SizeId, ColorId,      ItemName,   Price,    MinOrder) values ('" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["CategoryId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["TypeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BrandId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["SizeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ColorId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ItemName"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["Price"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["MinOrder"].ToString() + "')";
                            //            objCore.executeQuery(q);
                            //            DataSet dss = new DataSet();
                            //            int idc = 0;
                            //            dss = objCore.funGetDataSet("select max(id) as id from closing");
                            //            if (dss.Tables[0].Rows.Count > 0)
                            //            {
                            //                string i = dss.Tables[0].Rows[0][0].ToString();
                            //                if (i == string.Empty)
                            //                {
                            //                    i = "0";
                            //                }
                            //                idc = Convert.ToInt32(i) + 1;
                            //            }
                            //            else
                            //            {
                            //                idc = 1;
                            //            }
                            //            if (branchid == "")
                            //            {
                            //                getbranchid();
                            //            }
                            //            q = "insert into closing (branchid,id,itemid,date,Remaining,Remarks,onlineid) values('" + branchid + "','" + idc + "','" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "','" + DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd") + "','0','',0)";
                            //            objCore.executeQuery(q);
                            //        }
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {


                        }

                      
                        q = "select * from Recipe where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update Recipe set type='" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "',MenuItemId='" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "',Quantity='" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "',modifierid='" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "', RawItemId='" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', UOMCId='" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "' where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            q = "insert into Recipe (type,Id, MenuItemId, RawItemId, UOMCId, Quantity,  modifierid) values ( '" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "')";
                            objCore.executeQuery(q);
                        }
                    }

                }

            }
            if (chkk == true)
            {
                MessageBox.Show("Data Downloaded successfully");
            }
            
        }
        protected void getbranchid()
        {
            try
            {
                string q = "select id from branch";
                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    branchid = ds.Tables[0].Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {


            }
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                getmenuservice();
            }
            else
            {
                getmenu();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                button1.Text = "Please Wait";
                button1.Enabled = false;
                button2.Enabled = false;
                downloadbyservice("");
                button1.Text = "Download Selected Menu";
                button1.Enabled = true;
                button2.Enabled = true;
                return;
            }
            bool chkk = false;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DataGridViewCheckBoxCell chk = row.Cells[0] as DataGridViewCheckBoxCell;

                if (Convert.ToBoolean(chk.Value) == true)
                {
                    chkk = true;
                    string id = row.Cells[1].Value.ToString();
                    string q = "";
                    q = "select * from menugroup where id='" + row.Cells["menugroupid"].Value.ToString() + "'";
                    DataSet ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {

                    }
                    else
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        DataSet dsgroup = new DataSet();
                        q = "select * from menugroup where id='" + row.Cells["menugroupid"].Value.ToString() + "'";
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                            connection.Open();
                            com = new SqlCommand(q, connection);
                            SqlDataAdapter da = new SqlDataAdapter(com);
                            da.Fill(dsgroup);
                        }
                        catch (Exception ex)
                        {

                            // MessageBox.Show(ex.Message);
                        }

                        q = "insert into menugroup ( Id, Name, ColorId, Description, Status, Image, FontColorId, FontSize, uploadstatus, branchid, type, role) values ('" + dsgroup.Tables[0].Rows[0]["Id"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["name"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["ColorId"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Description"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Status"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Image"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["FontColorId"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["FontSize"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["uploadstatus"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["branchid"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["type"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["role"].ToString() + "')";
                        objCore.executeQuery(q);
                    }
                    q = "select * from menuitem where id='" + row.Cells["id"].Value.ToString() + "'";
                    ds = new DataSet();
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        q = "update menuitem set Status='" + row.Cells["Status"].Value.ToString() + "',Name='" + row.Cells["name"].Value.ToString() + "', FontSize='" + row.Cells["FontSize"].Value.ToString() + "',MenuGroupId='" + row.Cells["MenuGroupId"].Value.ToString() + "',ColorId='" + row.Cells["ColorId"].Value.ToString() + "',KDSId='" + row.Cells["KDSId"].Value.ToString() + "',FontColorId='" + row.Cells["FontColorId"].Value.ToString() + "', Price='" + row.Cells["name"].Value.ToString() + "', submenugroupid='" + row.Cells["name"].Value + "' where id='" + row.Cells["id"].Value.ToString() + "'";
                        objCore.executeQuery(q);
                    }
                    else
                    {
                        q = "insert into menuitem (Id, Code, Name, MenuGroupId, BarCode, Price, Status, ColorId, KDSId,  FontColorId, FontSize, Minutes, alarmtime, minuteskdscolor, alarmkdscolor, submenugroupid) values ( '" + row.Cells["Id"].Value + "', '" + row.Cells["Code"].Value + "', '" + row.Cells["name"].Value + "', '" + row.Cells["MenuGroupId"].Value + "', '" + row.Cells["BarCode"].Value + "', '" + row.Cells["Price"].Value + "', '" + row.Cells["Status"].Value + "', '" + row.Cells["ColorId"].Value + "', '" + row.Cells["KDSId"].Value + "', '" + row.Cells["FontColorId"].Value + "', '" + row.Cells["FontSize"].Value + "', '" + row.Cells["Minutes"].Value + "', '" + row.Cells["alarmtime"].Value + "', '" + row.Cells["minuteskdscolor"].Value + "', '" + row.Cells["alarmkdscolor"].Value + "', '" + row.Cells["submenugroupid"].Value + "')";
                        objCore.executeQuery(q);
                    }

                    DataSet dsflavour = new DataSet();
                    q = "select * from ModifierFlavour where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
                    try
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(dsflavour);
                    }
                    catch (Exception ex)
                    {

                        // MessageBox.Show(ex.Message);
                    }
                    
                    for (int j = 0; j < dsflavour.Tables[0].Rows.Count; j++)
                    {
                        q = "select * from ModifierFlavour where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update ModifierFlavour set Name='" + dsflavour.Tables[0].Rows[j]["name"].ToString() + "',MenuGroupId='" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "',MenuItemId='" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            q = "insert into ModifierFlavour (Id, MenuGroupId, MenuItemId, name, price, kdsid) values ( '" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["name"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "')";
                            objCore.executeQuery(q);
                        }
                    }


                    DataSet dsRuntimeModifier = new DataSet();
                    q = "select * from RuntimeModifier where menuItemid='" + row.Cells["id"].Value.ToString() + "'";
                    try
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(dsRuntimeModifier);
                    }
                    catch (Exception ex)
                    {

                        // MessageBox.Show(ex.Message);
                    }
                    q = "delete from RuntimeModifier where menuItemid='" + row.Cells["id"].Value.ToString() + "'";
                    objCore.executeQuery(q);
                    for (int j = 0; j < dsRuntimeModifier.Tables[0].Rows.Count; j++)
                    {
                        q = "select * from RuntimeModifier where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update RuntimeModifier set type='" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "',Name='" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString() + "',Quantity='" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "',status='" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "',rawitemid='" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "',MenuItemId='" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            q = "insert into RuntimeModifier (type,id, name, menuItemid, price, Quantity, status,  kdsid,  rawitemid) values ( '" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "')";
                            objCore.executeQuery(q);
                        }
                    }
                    DataSet dsreceipi = new DataSet();
                    q = "select * from Recipe where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
                    try
                    {
                        SqlConnection connection = new SqlConnection(cs);
                        SqlCommand com;
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        connection.Open();
                        com = new SqlCommand(q, connection);
                        SqlDataAdapter da = new SqlDataAdapter(com);
                        da.Fill(dsreceipi);
                    }
                    catch (Exception ex)
                    {

                        // MessageBox.Show(ex.Message);
                    }
                    q = "delete from  Recipe where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
                    objCore.executeQuery(q);
                    for (int j = 0; j < dsreceipi.Tables[0].Rows.Count; j++)
                    {

                        try
                        {
                            DataSet dsrraw = new DataSet();
                            q = "select * from RawItem where id='" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
                            try
                            {
                                SqlConnection connection = new SqlConnection(cs);
                                SqlCommand com;
                                if (connection.State == ConnectionState.Open)
                                    connection.Close();
                                connection.Open();
                                com = new SqlCommand(q, connection);
                                SqlDataAdapter da = new SqlDataAdapter(com);
                                da.Fill(dsrraw);
                            }
                            catch (Exception ex)
                            {

                                // MessageBox.Show(ex.Message);
                            }
                            for (int k = 0; k < dsrraw.Tables[0].Rows.Count; k++)
                            {
                                q = "select * from RawItem where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
                                ds = new DataSet();
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    q = "update RawItem set itemname='" + dsrraw.Tables[0].Rows[k]["itemname"].ToString() + "',GroupId='" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "',UOMId='" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "' where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
                                    objCore.executeQuery(q);
                                }
                                else
                                {
                                    q = "insert into RawItem (Id, GroupId, CategoryId, TypeId, BrandId, UOMId, SizeId, ColorId,      ItemName, BarCode, Code, Price,    MinOrder) values ('" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["CategoryId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["TypeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BrandId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["SizeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ColorId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ItemName"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BarCode"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["Code"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["Price"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["MinOrder"].ToString() + "')";
                                    objCore.executeQuery(q);
                                }
                            }
                        }
                        catch (Exception ex)
                        {


                        }


                        q = "select * from Recipe where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
                        ds = new DataSet();
                        ds = objCore.funGetDataSet(q);
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            q = "update Recipe set type='" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "',MenuItemId='" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "',Quantity='" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "',modifierid='" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "', RawItemId='" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', UOMCId='" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "' where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
                            objCore.executeQuery(q);
                        }
                        else
                        {
                            q = "insert into Recipe (type,Id, MenuItemId, RawItemId, UOMCId, Quantity,  modifierid) values ( '" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "')";
                            objCore.executeQuery(q);
                        }
                    }

                }

            }
            if (chkk == true)
            {
                MessageBox.Show("Data Downloaded successfully");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (type() == "service")
            {
                button2.Text = "Please Wait";
                button2.Enabled = false;
                button1.Enabled = false;
                downloadbyservice("All");
                button2.Text = "Download All";
                button1.Enabled = true;
                button2.Enabled = true;
                return;
            }


            //bool chkk = false;
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    //DataGridViewCheckBoxCell chk = row.Cells[0] as DataGridViewCheckBoxCell;

            //    //if (Convert.ToBoolean(chk.Value) == true)
            //    {
            //        chkk = true;
            //        string id = row.Cells[1].Value.ToString();
            //        string q = "";
            //        q = "select * from menugroup where id='" + row.Cells["menugroupid"].Value.ToString() + "'";
            //        DataSet ds = new DataSet();
            //        ds = objCore.funGetDataSet(q);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {

            //        }
            //        else
            //        {
            //            SqlConnection connection = new SqlConnection(cs);
            //            SqlCommand com;
            //            DataSet dsgroup = new DataSet();
            //            q = "select * from menugroup where id='" + row.Cells["menugroupid"].Value.ToString() + "'";
            //            try
            //            {
            //                if (connection.State == ConnectionState.Open)
            //                    connection.Close();
            //                connection.Open();
            //                com = new SqlCommand(q, connection);
            //                SqlDataAdapter da = new SqlDataAdapter(com);
            //                da.Fill(dsgroup);
            //            }
            //            catch (Exception ex)
            //            {

            //                // MessageBox.Show(ex.Message);
            //            }

            //            q = "insert into menugroup ( Id, Name, ColorId, Description, Status, Image, FontColorId, FontSize, uploadstatus, branchid, type, role) values ('" + dsgroup.Tables[0].Rows[0]["Id"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["name"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["ColorId"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Description"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Status"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["Image"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["FontColorId"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["FontSize"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["uploadstatus"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["branchid"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["type"].ToString() + "','" + dsgroup.Tables[0].Rows[0]["role"].ToString() + "')";
            //            objCore.executeQuery(q);
            //        }
            //        q = "select * from menuitem where id='" + row.Cells["id"].Value.ToString() + "'";
            //        ds = new DataSet();
            //        ds = objCore.funGetDataSet(q);
            //        if (ds.Tables[0].Rows.Count > 0)
            //        {
            //            q = "update menuitem set Name='" + row.Cells["name"].Value.ToString() + "', FontSize='" + row.Cells["FontSize"].Value.ToString() + "',MenuGroupId='" + row.Cells["MenuGroupId"].Value.ToString() + "',ColorId='" + row.Cells["ColorId"].Value.ToString() + "',KDSId='" + row.Cells["KDSId"].Value.ToString() + "',FontColorId='" + row.Cells["FontColorId"].Value.ToString() + "', Price='" + row.Cells["name"].Value.ToString() + "', submenugroupid='" + row.Cells["name"].Value + "' where id='" + row.Cells["id"].Value.ToString() + "'";
            //            objCore.executeQuery(q);
            //        }
            //        else
            //        {
            //            q = "insert into menuitem (Id, Code, Name, MenuGroupId, BarCode, Price, Status, ColorId, KDSId,  FontColorId, FontSize, Minutes, alarmtime, minuteskdscolor, alarmkdscolor, submenugroupid) values ( '" + row.Cells["Id"].Value + "', '" + row.Cells["Code"].Value + "', '" + row.Cells["name"].Value + "', '" + row.Cells["MenuGroupId"].Value + "', '" + row.Cells["BarCode"].Value + "', '" + row.Cells["Price"].Value + "', '" + row.Cells["Status"].Value + "', '" + row.Cells["ColorId"].Value + "', '" + row.Cells["KDSId"].Value + "', '" + row.Cells["FontColorId"].Value + "', '" + row.Cells["FontSize"].Value + "', '" + row.Cells["Minutes"].Value + "', '" + row.Cells["alarmtime"].Value + "', '" + row.Cells["minuteskdscolor"].Value + "', '" + row.Cells["alarmkdscolor"].Value + "', '" + row.Cells["submenugroupid"].Value + "')";
            //            objCore.executeQuery(q);
            //        }

            //        DataSet dsflavour = new DataSet();
            //        q = "select * from ModifierFlavour where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
            //        try
            //        {
            //            SqlConnection connection = new SqlConnection(cs);
            //            SqlCommand com;
            //            if (connection.State == ConnectionState.Open)
            //                connection.Close();
            //            connection.Open();
            //            com = new SqlCommand(q, connection);
            //            SqlDataAdapter da = new SqlDataAdapter(com);
            //            da.Fill(dsflavour);
            //        }
            //        catch (Exception ex)
            //        {

            //            // MessageBox.Show(ex.Message);
            //        }
            //        for (int j = 0; j < dsflavour.Tables[0].Rows.Count; j++)
            //        {
            //            q = "select * from ModifierFlavour where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
            //            ds = new DataSet();
            //            ds = objCore.funGetDataSet(q);
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                q = "update ModifierFlavour set Name='" + dsflavour.Tables[0].Rows[j]["name"].ToString() + "',MenuGroupId='" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "',MenuItemId='" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "'";
            //                objCore.executeQuery(q);
            //            }
            //            else
            //            {
            //                q = "insert into ModifierFlavour (Id, MenuGroupId, MenuItemId, name, price, kdsid) values ( '" + dsflavour.Tables[0].Rows[j]["id"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuGroupId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["name"].ToString() + "', '" + dsflavour.Tables[0].Rows[j]["price"].ToString() + "','" + dsflavour.Tables[0].Rows[j]["kdsid"].ToString() + "')";
            //                objCore.executeQuery(q);
            //            }
            //        }

            //        DataSet dsRuntimeModifier = new DataSet();
            //        q = "select * from RuntimeModifier where menuItemid='" + row.Cells["id"].Value.ToString() + "'";
            //        try
            //        {
            //            SqlConnection connection = new SqlConnection(cs);
            //            SqlCommand com;
            //            if (connection.State == ConnectionState.Open)
            //                connection.Close();
            //            connection.Open();
            //            com = new SqlCommand(q, connection);
            //            SqlDataAdapter da = new SqlDataAdapter(com);
            //            da.Fill(dsRuntimeModifier);
            //        }
            //        catch (Exception ex)
            //        {

            //            // MessageBox.Show(ex.Message);
            //        }
            //        for (int j = 0; j < dsRuntimeModifier.Tables[0].Rows.Count; j++)
            //        {
            //            q = "select * from RuntimeModifier where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
            //            ds = new DataSet();
            //            ds = objCore.funGetDataSet(q);
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                q = "update RuntimeModifier set type='" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "',Name='" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString() + "',Quantity='" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "',status='" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "',rawitemid='" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "',MenuItemId='" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', Price='" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', kdsid='" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "' where id='" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "'";
            //                objCore.executeQuery(q);
            //            }
            //            else
            //            {
            //                q = "insert into RuntimeModifier (type,id, name, menuItemid, price, Quantity, status,  kdsid,  rawitemid) values ( '" + dsRuntimeModifier.Tables[0].Rows[j]["type"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["id"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["name"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["price"].ToString() + "', '" + dsRuntimeModifier.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["status"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["kdsid"].ToString() + "','" + dsRuntimeModifier.Tables[0].Rows[j]["rawitemid"].ToString() + "')";
            //                objCore.executeQuery(q);
            //            }
            //        }

            //        DataSet dsreceipi = new DataSet();
            //        q = "select * from Recipe where MenuItemId='" + row.Cells["id"].Value.ToString() + "'";
            //        try
            //        {
            //            SqlConnection connection = new SqlConnection(cs);
            //            SqlCommand com;
            //            if (connection.State == ConnectionState.Open)
            //                connection.Close();
            //            connection.Open();
            //            com = new SqlCommand(q, connection);
            //            SqlDataAdapter da = new SqlDataAdapter(com);
            //            da.Fill(dsreceipi);
            //        }
            //        catch (Exception ex)
            //        {

            //            // MessageBox.Show(ex.Message);
            //        }

            //        for (int j = 0; j < dsreceipi.Tables[0].Rows.Count; j++)
            //        {


            //            try
            //            {
            //                DataSet dsrraw = new DataSet();
            //                q = "select * from RawItem where id='" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "'";
            //                try
            //                {
            //                    SqlConnection connection = new SqlConnection(cs);
            //                    SqlCommand com;
            //                    if (connection.State == ConnectionState.Open)
            //                        connection.Close();
            //                    connection.Open();
            //                    com = new SqlCommand(q, connection);
            //                    SqlDataAdapter da = new SqlDataAdapter(com);
            //                    da.Fill(dsrraw);
            //                }
            //                catch (Exception ex)
            //                {

            //                    // MessageBox.Show(ex.Message);
            //                }
            //                for (int k = 0; k < dsrraw.Tables[0].Rows.Count; k++)
            //                {
            //                    q = "select * from RawItem where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
            //                    ds = new DataSet();
            //                    ds = objCore.funGetDataSet(q);
            //                    if (ds.Tables[0].Rows.Count > 0)
            //                    {
            //                        q = "update RawItem set itemname='" + dsrraw.Tables[0].Rows[k]["itemname"].ToString() + "',GroupId='" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "',UOMId='" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "',price='" + dsrraw.Tables[0].Rows[k]["price"].ToString() + "' where id='" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "'";
            //                        objCore.executeQuery(q);
            //                    }
            //                    else
            //                    {
            //                        q = "insert into RawItem (Id, GroupId, CategoryId, TypeId, BrandId, UOMId, SizeId, ColorId,      ItemName, BarCode, Code, Price,    MinOrder) values ('" + dsrraw.Tables[0].Rows[k]["id"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["GroupId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["CategoryId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["TypeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BrandId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["UOMId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["SizeId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ColorId"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["ItemName"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["BarCode"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["Code"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["Price"].ToString() + "','" + dsrraw.Tables[0].Rows[k]["MinOrder"].ToString() + "')";
            //                        objCore.executeQuery(q);
            //                    }
            //                }
            //            }
            //            catch (Exception ex)
            //            {
                            
                            
            //            }


            //            q = "select * from Recipe where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
            //            ds = new DataSet();
            //            ds = objCore.funGetDataSet(q);
            //            if (ds.Tables[0].Rows.Count > 0)
            //            {
            //                q = "update Recipe set type='" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "',MenuItemId='" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "',Quantity='" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "',modifierid='" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "', RawItemId='" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', UOMCId='" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "' where id='" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "'";
            //                objCore.executeQuery(q);
            //            }
            //            else
            //            {
            //                q = "insert into Recipe (type,Id, MenuItemId, RawItemId, UOMCId, Quantity,  modifierid) values ( '" + dsreceipi.Tables[0].Rows[j]["type"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["id"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["MenuItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["RawItemId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["UOMCId"].ToString() + "', '" + dsreceipi.Tables[0].Rows[j]["Quantity"].ToString() + "','" + dsreceipi.Tables[0].Rows[j]["modifierid"].ToString() + "')";
            //                objCore.executeQuery(q);
            //            }
            //        }
            //    }

            //}
            //if (chkk == true)
            //{
            //    MessageBox.Show("Data Downloaded successfully");
            //}
        }
    }
}
