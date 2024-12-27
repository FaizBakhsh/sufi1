using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.forms
{
    public partial class Bankdepositslip : Form
    {
        public Bankdepositslip()
        {
            InitializeComponent();
        }  POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
       public string id = "";
        private void Bankdepositslip_Load(object sender, EventArgs e)
        {
            try
            {
                string q = "SELECT        dbo.BankDeposits.Date, dbo.BankDeposits.ActualAmount, dbo.BankDeposits.DepositedAmount, dbo.BankDeposits.Image, dbo.BankDeposits.Status FROM            dbo.BankDeposits where dbo.BankDeposits.id='" + id + "'";


                DataSet ds = new DataSet();
                ds = objCore.funGetDataSet(q);
                if (ds.Tables[0].Rows.Count > 0)
                {

                    Byte[] imageData = new Byte[0];
                    imageData = (Byte[])(ds.Tables[0].Rows[0]["Image"]);
                    MemoryStream mem = new MemoryStream(imageData);
                    pictureBox1.Image = Image.FromStream(mem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
