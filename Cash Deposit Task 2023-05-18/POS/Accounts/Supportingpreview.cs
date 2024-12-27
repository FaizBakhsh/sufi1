using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Accounts
{
    public partial class Supportingpreview : Form
    {
        public string voucherno = "", table = "";
        public Supportingpreview()
        {
            InitializeComponent();
        }

        private void Supportingpreview_Load(object sender, EventArgs e)
        {
            try
            {
                
                
                    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
                    DataSet ds = new DataSet();
                    string q = "";

                    q = "select supporting from " + table + " where voucherno='" + voucherno + "'";
                    ds = objCore.funGetDataSet(q);
                    if (ds.Tables[0].Rows.Count > 0)
                    {


                        Byte[] data = new Byte[0];
                        data = (Byte[])(ds.Tables[0].Rows[0]["supporting"]);

                        MemoryStream mem = new MemoryStream(data);
                        pictureBox1.Image = Image.FromStream(mem);
                    }
                    else
                    {
                        if (table == "JournalAccount")
                        {
                            q = "select supporting from supplieraccount where voucherno='" + voucherno + "'";
                            ds = objCore.funGetDataSet(q);
                            if (ds.Tables[0].Rows.Count > 0)
                            {


                                Byte[] data = new Byte[0];
                                data = (Byte[])(ds.Tables[0].Rows[0]["supporting"]);

                                MemoryStream mem = new MemoryStream(data);
                                pictureBox1.Image = Image.FromStream(mem);
                            }
                            else
                            {
                                q = "select supporting from EmployeesAccount where voucherno='" + voucherno + "'";
                                ds = objCore.funGetDataSet(q);
                                if (ds.Tables[0].Rows.Count > 0)
                                {


                                    Byte[] data = new Byte[0];
                                    data = (Byte[])(ds.Tables[0].Rows[0]["supporting"]);

                                    MemoryStream mem = new MemoryStream(data);
                                    pictureBox1.Image = Image.FromStream(mem);
                                }
                            }
                        }
                    }
            }
            catch (Exception ex)
            {


            }
        }
    }
}
