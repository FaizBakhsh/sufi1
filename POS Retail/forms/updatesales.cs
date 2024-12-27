using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRetail.forms
{
    public partial class updatesales : Form
    {
        public updatesales()
        {
            InitializeComponent();
        }
        POSRetail.classes.Clsdbcon objCore = new classes.Clsdbcon();
        private void button1_Click(object sender, EventArgs e)
        {
            string q = "";
            if (chksales.Checked == true)
            {
                q = "update sale set uploadstatusserver='Pending' where date between '"+dateTimePicker1.Text+"' and '"+dateTimePicker2.Text+"'";
                objCore.executeQuery(q);
            }
            if (chkinv.Checked == true)
            {
                q = "update InventoryConsumed set uploadstatus='Pending' where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                objCore.executeQuery(q);
            }
            if (chkclosing.Checked == true)
            {
                q = "update discard set uploadstatus='Pending' where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                objCore.executeQuery(q);
            }
            if (chktransfer.Checked == true)
            {
                q = "update InventoryTransfer set uploadstatus='Pending' where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                objCore.executeQuery(q);
            }
            if (chkexpenses.Checked == true)
            {
                q = "update EmployeesSalary set uploadstatus='Pending' where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                objCore.executeQuery(q);
                q = "update Expenses set uploadstatus='Pending' where date between '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                objCore.executeQuery(q);
            }
        }
    }
}
