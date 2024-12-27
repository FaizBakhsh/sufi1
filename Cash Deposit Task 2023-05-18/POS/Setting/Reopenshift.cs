using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Setting
{
    public partial class Reopenshift : Form
    {
        public Reopenshift()
        {
            InitializeComponent();
        }

        private void Reopenshift_Load(object sender, EventArgs e)
        {
            getdate();
            getshift();
        }
        POSRestaurant.classes.Clsdbcon objcore = new classes.Clsdbcon();
        protected void getdate()
        {
            string q = "select top 1 * from dayend order by id desc";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                textBox1.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["date"].ToString()).ToString("yyyy-MM-dd");
            }
        }
        protected void getshift()
        {
            string q = "SELECT DISTINCT dbo.ShiftStart.shiftid, dbo.ShiftStart.Date, dbo.Shifts.Name, dbo.ShiftStart.Terminal, dbo.shiftcash.cashout FROM            dbo.ShiftStart INNER JOIN                         dbo.Shifts ON dbo.ShiftStart.shiftid = dbo.Shifts.Id INNER JOIN                         dbo.shiftcash ON dbo.ShiftStart.shiftid = dbo.shiftcash.shiftid AND dbo.ShiftStart.Date = dbo.shiftcash.date AND dbo.ShiftStart.Terminal = dbo.shiftcash.Terminal WHERE        (dbo.ShiftStart.Date = '" + textBox1.Text + "') AND (dbo.ShiftStart.status = 'Close')";
            DataSet ds = new DataSet();
            ds = objcore.funGetDataSet(q);
            //  if (ds.Tables[0].Rows.Count > 0)
            {
                dataGridView1.DataSource = ds.Tables[0];

            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                DialogResult dr = MessageBox.Show("Are you sure to ReOpen Selecred shift", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    string shiftid = dataGridView1.Rows[e.RowIndex].Cells["shiftid"].Value.ToString();
                    string terminal = dataGridView1.Rows[e.RowIndex].Cells["Terminal"].Value.ToString();
                    string q = "update ShiftStart set status='Open' where shiftid='" + shiftid + "' and Terminal='" + terminal + "' and date='" + textBox1.Text + "'";
                    objcore.executeQuery(q);
                }
                getshift();
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow data in dataGridView1.Rows)
                {
                    var BtnCell = (DataGridViewButtonCell)data.Cells[0];
                    BtnCell.Value = "ReOpen Shift";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 5)
            {
                try
                {

                    DialogResult dr = MessageBox.Show("Are you sure to ReOpen Selecred shift", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        string shiftid = dataGridView1.Rows[e.RowIndex].Cells["shiftid"].Value.ToString();
                        string terminal = dataGridView1.Rows[e.RowIndex].Cells["Terminal"].Value.ToString();
                        string q = "update shiftcash set cashout='" + dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString() + "' where shiftid='" + shiftid + "' and Terminal='" + terminal + "' and date='" + textBox1.Text + "'";
                        int res = objcore.executeQueryint(q);
                        if (res > 0)
                        {
                            MessageBox.Show("Physical Cash Updated");
                        }
                        else
                        {
                            MessageBox.Show("Error Updating Physical Cash");
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
