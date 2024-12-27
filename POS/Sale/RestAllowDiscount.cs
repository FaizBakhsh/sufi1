using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace POSRestaurant.Sale
{
    public partial class RestAllowDiscount : Form
    {
        private TextBox focusedTextbox = null;
        private  RestSale _frm1;
        public string saleid = "", menuitemid = "", name = "",shiftid="";
       
          public string cashrr;
          public string datee;
          public string useridd;

          public RestAllowDiscount(RestSale frm1)
          {
              InitializeComponent();
              _frm1 = frm1;
              
          }
       

        private void button1_Click(object sender, EventArgs e)
        {
          
               
                    
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
           
        }

        private void btnexit_Click(object sender, EventArgs e)
        {
            
        }

        private void AddGroups_Load(object sender, EventArgs e)
        {
            this.TopMost = true;
            txtcard.Focus();
            button2.Text = "!";
            shiftkey();
        }

        private void txtdescription_TextChanged(object sender, EventArgs e)
        {
            
        }
        public string reason = "";
        //private void vButton2_Click(object sender, EventArgs e)
        //{
        //    if (txtname.Text.Trim() == string.Empty)
        //    {
        //        MessageBox.Show("Please Enter User Name");
        //        return;
        //    }

        //    if (txtpassword.Text.Trim() == string.Empty)
        //    {
        //        MessageBox.Show("Please Enter Password");
        //        return;
        //    }

        //    POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
        //    DataSet ds = new DataSet();
        //    string q = "select * from Users where usertype='Admin' and UserName='" + txtname.Text + "' and password='" + txtpassword.Text.Trim() + "'";
        //    ds = objCore.funGetDataSet(q);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {

        //        string authentication = objCore.authentication1(editmode.ToString(), ds.Tables[0].Rows[0]["id"].ToString());
        //        if (authentication == "yes")
        //        {

        //        }
        //        else
        //        {
        //           // MessageBox.Show("You are not allowed to view this");
        //           // return;
        //        }
        //        if (editmode.ToString() == "Cash Transaction")
        //        {
        //            CashTransaction ob = new CashTransaction(_frm1);
        //            ob.date = id;
        //            ob.Show();
        //        }
        //        if (editmode.ToString() == "Complimentry Discount")
        //        {
        //            CompDiscount obj = new CompDiscount(_frm1);
        //            obj.Show();
                    
        //        }
        //        if (editmode.ToString() == "Discount")
        //        {
        //            _frm1.IsTextBoxEnabled = true;
        //            _frm1.Enabled = true;
        //        }
        //        if (editmode.ToString() == "VoidAll")
        //        {
        //            _frm1.voidall(saleid);
        //            _frm1.Enabled = true;
        //            //VoidBill objv = new VoidBill();
        //            //objv.id = id.ToString();
        //            //objv.Show();
        //        }
        //        if (editmode.ToString() == "waste")
        //        {
        //            _frm1.Waste(saleid);
        //            _frm1.Enabled = true;
        //            //VoidBill objv = new VoidBill();
        //            //objv.id = id.ToString();
        //            //objv.Show();
        //        }
        //        if (editmode.ToString() == "VoidOne")
        //        {
        //            _frm1.voidone(saleid);
        //            _frm1.Enabled = true;
        //            //VoidBill objv = new VoidBill();
        //            //objv.id = id.ToString();
        //            //objv.Show();
        //        }
        //        if (editmode.ToString() == "Duplicate")
        //        {
        //            DuplicaeBill ob = new DuplicaeBill(_frm1);
        //            ob.id = id;
        //            ob.Show();

        //        }
        //        if (editmode.ToString() == "Day End")
        //        {
        //            _frm1.dayend(id,editmode);
        //            _frm1.Enabled = true;
                    
        //        }
        //        if (editmode.ToString() == "Day Start")
        //        {
        //            _frm1.dayend(id, editmode);
        //            _frm1.Enabled = true;

        //        }
        //        if (editmode.ToString() == "Refund")
        //        {
        //            RefundBill ob = new RefundBill(_frm1);
        //            ob.id = id;
        //            ob.Show();

        //        }
        //        if (editmode.ToString() == "SaleReport")
        //        {
        //            POSRestaurant.Reports.RptUserSale obj = new Reports.RptUserSale(_frm1);
        //            obj.cashiername = cashrr;
        //            obj.date = datee;
        //            obj.userid = id;
        //            obj.Show();

        //        }
        //        if (editmode.ToString() == "CashDrawer")
        //        {
        //            DataSet dsprint = new DataSet();
        //            dsprint = objCore.funGetDataSet("select * from Printers where type='Receipt'");

        //            if (dsprint.Tables[0].Rows.Count > 0)
        //            {
        //                POSRestaurant.Reports.OpenDrawer rptDoc = new Reports.OpenDrawer();

        //                rptDoc.PrintOptions.PrinterName = dsprint.Tables[0].Rows[0]["name"].ToString();
                        
        //                rptDoc.PrintToPrinter(1, false, 0, 0);

        //            }
        //            _frm1.Enabled = true;
        //        }
        //        this.Close();
        //    }
        //    else
        //    {
        //        MessageBox.Show("invalid User Name or Password");
        //        return;

        //    }
            
        //}
        public void carlogin(string type)
        {
   

            POSRestaurant.classes.Clsdbcon objCore = new classes.Clsdbcon();
            DataSet ds = new DataSet();
            string q = "";
            if (type == "card")
            {
                q = "select * from Users where  CardNo='" + txtcard.Text.Trim() + "' and Usertype='admin' or CardNo='" + txtcard.Text.Trim() + "' and Usertype='Manager'";
            }
            else
            {
                q = "select * from Users where  password='" + txtcard.Text.Trim() + "' and Usertype='admin' or password='" + txtcard.Text.Trim() + "' and Usertype='Manager'";
            }
            ds = objCore.funGetDataSet(q);
            if (ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    //string authentication = objCore.authentication1(editmode.ToString(), ds.Tables[0].Rows[0]["id"].ToString());
                    //if (authentication == "yes")
                    //{

                    //}
                    //else
                    //{
                    //    MessageBox.Show("You are not allowed to view this");
                    //    return;
                    //}
                    if (editmode.ToString() == "service charges")
                    {
                        _frm1.cancelservicecharges();
                        _frm1.Enabled = true;

                    }
                    if (editmode.ToString() == "Complimentry Discount")
                    {
                        string authentication = objCore.authentication1("Complimentry", ds.Tables[0].Rows[0]["id"].ToString());
                        if (authentication == "yes")
                        {
                        }
                        else
                        {
                            MessageBox.Show("You are not allowed to view this");
                            return;
                        }
                        _frm1.calcomp();
                        _frm1.Enabled = true;
                        
                    }
                    if (editmode.ToString() == "changebill")
                    {
                        changebill obj = new changebill(_frm1);
                        try
                        {
                            obj.sid = Convert.ToInt32(id);
                            obj.userid = ds.Tables[0].Rows[0]["id"].ToString();
                        }
                        catch (Exception ex)
                        {
                           
                        }
                        obj.name = name;
                        obj.Show();

                    }
                    if (editmode.ToString() == "recall")
                    {
                        BillRecallold obj = new BillRecallold(_frm1);
                        obj.date = datee;
                        obj.Show();

                    }
                    if (editmode.ToString() == "Employee")
                    {
                        EmployeeFunctions obj = new EmployeeFunctions(_frm1);
                        obj.user = ds.Tables[0].Rows[0]["id"].ToString();
                        POSRestaurant.Properties.Settings.Default.UserId = ds.Tables[0].Rows[0]["id"].ToString();
                        POSRestaurant.Properties.Settings.Default.Save();
                        obj.date = datee;
                        obj.shiftid = shiftid;
                        obj.Show();
                        
                    }
                    if (editmode.ToString() == "Discount")
                    
                    {
                        string authentication = objCore.authentication1("Discount", ds.Tables[0].Rows[0]["id"].ToString());
                        if (authentication == "yes")
                        {

                        }
                        else
                        {
                            MessageBox.Show("You are not allowed to view this");
                            return;
                        }
                        DiscountSelection ob = new DiscountSelection(_frm1);
                        ob.cardno = txtcard.Text;
                        ob.saleid = saleid.ToString();
                        string limit = "0";
                        try
                        {
                            limit = ds.Tables[0].Rows[0]["discountlimit"].ToString();
                            if (limit == "")
                            {
                                limit = "0";
                            }
                            ob.limit = float.Parse(limit);
                        }
                        catch (Exception ex)
                        {
                        }
                       
                        //DiscountKeys obj = new DiscountKeys(_frm1);
                        //obj.cardno = txtcard.Text;
                        //obj.saleid = saleid.ToString();
                        ob.Show();
                        //_frm1.IsTextBoxEnabled = true;
                        //_frm1.Enabled = true;
                    }
                    if (editmode.ToString() == "disI")
                    {
                        _frm1.caldis(ds.Tables[0].Rows[0]["id"].ToString());
                        _frm1.Enabled = true;
                        //_frm1.IsTextBoxEnabled = true;
                        //_frm1.Enabled = true;
                    }
                    //if (editmode.ToString() == "VoidAll")
                    //{
                    //    _frm1.voidall(saleid);
                    //    _frm1.Enabled = true;
                    //    _frm1.TopMost = true;
                    //    //VoidBill objv = new VoidBill();
                    //    //objv.id = id.ToString();
                    //    //objv.Show();
                    //}
                    if (editmode.ToString() == "waste")
                    {
                        _frm1.Waste(saleid);
                        _frm1.Enabled = true;
                        _frm1.TopMost = true;
                        //VoidBill objv = new VoidBill();
                        //objv.id = id.ToString();
                        //objv.Show();
                    }
                    if (editmode.ToString() == "VoidOne")
                    {
                        _frm1.voidone(menuitemid, saleid);
                        _frm1.Enabled = true;
                        _frm1.TopMost = true;
                        //VoidBill objv = new VoidBill();
                        //objv.id = id.ToString();
                        //objv.Show();
                    }
                    if (editmode.ToString() == "void")
                    {
                        string authentication = objCore.authentication1("VoidOne", ds.Tables[0].Rows[0]["id"].ToString());
                        if (authentication == "yes")
                        {

                        }
                        else
                        {
                            MessageBox.Show("You are not allowed to view this");
                            return;
                        }
                        Reasons obj = new Reasons(_frm1);
                        obj.id = id;
                        
                        obj.Show();
                       
                        this.Close();
                        
                    }
                    if (editmode.ToString() == "voidall")
                    {
                        string authentication = objCore.authentication1("VoidOne", ds.Tables[0].Rows[0]["id"].ToString());
                        if (authentication == "yes")
                        {

                        }
                        else
                        {
                            MessageBox.Show("You are not allowed to view this");
                            return;
                        }
                        Reasons obj = new Reasons(_frm1);
                        obj.id = id;
                        obj.type = "voidall";
                        obj.Show();
                        this.Close();
                    }
                    if (editmode.ToString() == "Duplicate")
                    {
                        DuplicaeBill ob = new DuplicaeBill(_frm1);
                        ob.id = id;
                        ob.date = datee;
                        ob.Show();

                    }
                    if (editmode.ToString() == "Day End")
                    {
                        _frm1.dayend(id, editmode);
                        _frm1.Enabled = true;

                    }
                    if (editmode.ToString() == "Day Start")
                    {
                        _frm1.dayend(id, editmode);
                        _frm1.Enabled = true;

                    }
                    if (editmode.ToString() == "Refund")
                    {
                        RefundBill ob = new RefundBill(_frm1);
                        ob.id = id;
                        ob.date = datee;
                        ob.Show();

                    }
                    if (editmode.ToString() == "Update Previous")
                    {
                        _frm1.updatepreviousorder();
                        _frm1.Enabled = true;
                    }
                    if (editmode.ToString() == "SaleReport")
                    {
                        //POSRestaurant.Reports.RptUserSale obj = new Reports.RptUserSale(_frm1);
                        //obj.cashiername = cashrr;
                        //obj.date = datee;
                        //obj.userid = id;
                        //obj.Show();

                    }
                    if (editmode.ToString() == "CashDrawer")
                    {
                        DataSet dsprint = new DataSet();
                        // dsprint = objCore.funGetDataSet("select * from Printers where type='Receipt'");

                        //if (dsprint.Tables[0].Rows.Count > 0)
                        {
                            //POSRestaurant.Reports.OpenDrawer rptDoc = new Reports.OpenDrawer();

                            //rptDoc.PrintOptions.PrinterName = GetDefaultPrinter();

                            //rptDoc.PrintToPrinter(1, false, 0, 0);

                        }
                        _frm1.Enabled = true;
                    }
                    if (editmode.ToString() == "SaleModify")
                    {
                        IsValid = true;
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                   
                }
                _frm1.Enabled = true;
                
                this.Close();
            }
            else
            {
                MessageBox.Show("invalid Card");
                //return;

            }
        }
        //public string GetDefaultPrinter()
        //{
        //    PrinterSettings settings = new PrinterSettings();
        //    foreach (string printer in PrinterSettings.InstalledPrinters)
        //    {
        //        settings.PrinterName = printer;
        //        if (settings.IsDefaultPrinter)
        //            return printer;
        //    }
        //    return string.Empty;
        //}
        private void vButton1_Click(object sender, EventArgs e)
        {
            txtcard.Text = string.Empty;
            //txtname.Text = string.Empty;
            //txtpassword.Text = string.Empty;
        }

        private void txtcard_KeyPress(object sender, KeyPressEventArgs e)
        {
            //txtcard.Text =txtcard.Text+ e.KeyChar.ToString().Trim();
            //e.Handled = false;
        }

        private void txtcard_TextChanged(object sender, EventArgs e)
        {
            //carlogin();
        }

        private void vButton3_Click(object sender, EventArgs e)
        {

            _frm1.Enabled = true;
            //_frm1.TopMost = true;
            this.Close();
        }

        private void txtcard_Enter(object sender, EventArgs e)
        {
            try
            {
                focusedTextbox = (TextBox)sender;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Button t = (sender) as Button;
                
                if (focusedTextbox != null)
                {

                    {
                        focusedTextbox.Text = focusedTextbox.Text + t.Text.Replace("&&", "&");
                    }
                    return;
                }

            }
            catch (Exception ex)
            {

                // MessageBox.Show(ex.Message);
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {

            shiftkey();
        }
        public void shiftkey()
        {
            if (button2.Text != "!")
            {
                button2.Text = "!";
                button3.Text = "@";
                button4.Text = "#";
                button5.Text = "$";
                button6.Text = "%";
                button7.Text = "^";
                button8.Text = "&&";
                button9.Text = "*";
                button10.Text = "(";
                button11.Text = ")";
                button12.Text = "Q";
                button16.Text = "W";
                button18.Text = "E";
                button20.Text = "R";
                button22.Text = "T";
                button21.Text = "Y";
                button19.Text = "U";
                button17.Text = "I";
                button15.Text = "O";
                button13.Text = "P";

                button23.Text = "A";
                button25.Text = "S";
                button27.Text = "D";
                button29.Text = "F";
                button31.Text = "G";
                button30.Text = "H";
                button28.Text = "J";
                button26.Text = "K";
                button24.Text = "L";

                button33.Text = "Z";
                button35.Text = "X";
                button37.Text = "C";
                button39.Text = "V";
                button41.Text = "B";
                button40.Text = "N";
                button38.Text = "M";
                // button36.Text = "o";


            }
            else
            {
                button2.Text = "1";
                button3.Text = "2";
                button4.Text = "3";
                button5.Text = "4";
                button6.Text = "5";
                button7.Text = "6";
                button8.Text = "7";
                button9.Text = "8";
                button10.Text = "9";
                button11.Text = "0";
                button12.Text = "q";
                button16.Text = "w";
                button18.Text = "e";
                button20.Text = "r";
                button22.Text = "t";
                button21.Text = "y";
                button19.Text = "u";
                button17.Text = "i";
                button15.Text = "o";
                button13.Text = "p";

                button23.Text = "a";
                button25.Text = "s";
                button27.Text = "d";
                button29.Text = "f";
                button31.Text = "g";
                button30.Text = "h";
                button28.Text = "j";
                button26.Text = "k";
                button24.Text = "l";

                button33.Text = "z";
                button35.Text = "x";
                button37.Text = "c";
                button39.Text = "v";
                button41.Text = "b";
                button40.Text = "n";
                button38.Text = "m";


            }
        }

        private void button32_Click(object sender, EventArgs e)
        {
            try
            {
                int index = focusedTextbox.SelectionStart;
                focusedTextbox.Text = focusedTextbox.Text.Remove(focusedTextbox.SelectionStart - 1, 1);
                focusedTextbox.Select(index - 1, 1);
                focusedTextbox.Focus();
            }
            catch (Exception ex)
            {


            }
        }

        private void txtcard_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                carlogin("card");
            }
        }

        private void vButton2_Click(object sender, EventArgs e)
        {
            carlogin("");
        }
    }
}
