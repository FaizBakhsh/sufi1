using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Forms;
using System.Management;
namespace POSRetail.forms
{
    public partial class Superadminpassword : Form
    {
        SuperAdminForm _frm;
        public string type = "";
        string mbInfo = String.Empty;
        public Superadminpassword(SuperAdminForm frm)
        {
            InitializeComponent();
            _frm = frm;
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    ManagementClass oMClass = new ManagementClass("Win32_NetworkAdapterConfiguration");
            //    ManagementObjectCollection colMObj = oMClass.GetInstances();
            //    foreach (ManagementObject objMO in colMObj)
            //    {
            //        mbInfo += objMO["MacAddress"].ToString();
            //    }
            //}
            //catch (Exception ex)
            //{


            //}
            if (textBox1.Text == "L@wBiz1987")
            {
                if (textBox2.Text == "")
                {
                    MessageBox.Show("Please Enter Key");
                    return;
                }
                _frm.register(textBox2.Text, textBox3.Text);
                _frm.TopMost = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid password");
            }
        }

        private void Superadminpassword_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            
            //    Console.WriteLine(objMO["MacAddress"].ToString());

            ////Get motherboard's serial number 
            //ManagementObjectSearcher mbs = new ManagementObjectSearcher("Select * From Win32_BaseBoard");
            //foreach (ManagementObject mo in mbs.Get())
            //{
            //    mbInfo += mo["SerialNumber"].ToString();
            //}
        
          
        }
    }
}
