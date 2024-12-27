using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Accounts
{
    public partial class SubAccount : Form
    {
        public SubAccount()
        {
            InitializeComponent();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text == "Update")
            {

            }
            else
            {
                string q = "";
            }
        }
    }
}
