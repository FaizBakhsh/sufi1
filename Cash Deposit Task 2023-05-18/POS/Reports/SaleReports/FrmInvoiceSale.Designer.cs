namespace POSRestaurant.Reports.SaleReports
{
    partial class FrmInvoiceSale
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.chkaddress = new System.Windows.Forms.CheckBox();
            this.chkphone = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbordertype = new System.Windows.Forms.ComboBox();
            this.lblfloors = new System.Windows.Forms.Label();
            this.cmbfloors = new System.Windows.Forms.ComboBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbbranch = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbtype = new System.Windows.Forms.ComboBox();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbshift = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbtimeto = new System.Windows.Forms.ComboBox();
            this.cmbtimefrom = new System.Windows.Forms.ComboBox();
            this.chktime = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.crystalReportViewer1);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(924, 542);
            this.panel1.TabIndex = 0;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 206);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(924, 336);
            this.crystalReportViewer1.TabIndex = 1;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.DoubleClickPage += new CrystalDecisions.Windows.Forms.PageMouseEventHandler(this.crystalReportViewer1_DoubleClickPage);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.chktime);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cmbtimeto);
            this.panel2.Controls.Add(this.cmbtimefrom);
            this.panel2.Controls.Add(this.chkaddress);
            this.panel2.Controls.Add(this.chkphone);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.cmbordertype);
            this.panel2.Controls.Add(this.lblfloors);
            this.panel2.Controls.Add(this.cmbfloors);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cmbbranch);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cmbtype);
            this.panel2.Controls.Add(this.dateTimePicker2);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.vButton1);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cmbshift);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(924, 206);
            this.panel2.TabIndex = 0;
            // 
            // chkaddress
            // 
            this.chkaddress.AutoSize = true;
            this.chkaddress.Location = new System.Drawing.Point(615, 119);
            this.chkaddress.Name = "chkaddress";
            this.chkaddress.Size = new System.Drawing.Size(135, 17);
            this.chkaddress.TabIndex = 9;
            this.chkaddress.Text = "Show Delivery Address";
            this.chkaddress.UseVisualStyleBackColor = true;
            // 
            // chkphone
            // 
            this.chkphone.AutoSize = true;
            this.chkphone.Location = new System.Drawing.Point(484, 119);
            this.chkphone.Name = "chkphone";
            this.chkphone.Size = new System.Drawing.Size(128, 17);
            this.chkphone.TabIndex = 8;
            this.chkphone.Text = "Show Delivery Phone";
            this.chkphone.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(12, 83);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(97, 20);
            this.label7.TabIndex = 32;
            this.label7.Text = "Order Type";
            // 
            // cmbordertype
            // 
            this.cmbordertype.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbordertype.FormattingEnabled = true;
            this.cmbordertype.Items.AddRange(new object[] {
            "All",
            "Take Away",
            "Dine In",
            "Delivery"});
            this.cmbordertype.Location = new System.Drawing.Point(111, 80);
            this.cmbordertype.Margin = new System.Windows.Forms.Padding(2);
            this.cmbordertype.Name = "cmbordertype";
            this.cmbordertype.Size = new System.Drawing.Size(246, 28);
            this.cmbordertype.TabIndex = 4;
            this.cmbordertype.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            this.cmbordertype.TextChanged += new System.EventHandler(this.comboBox2_TextChanged);
            // 
            // lblfloors
            // 
            this.lblfloors.AutoSize = true;
            this.lblfloors.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblfloors.Location = new System.Drawing.Point(374, 83);
            this.lblfloors.Name = "lblfloors";
            this.lblfloors.Size = new System.Drawing.Size(59, 20);
            this.lblfloors.TabIndex = 30;
            this.lblfloors.Text = "Floors";
            this.lblfloors.Visible = false;
            // 
            // cmbfloors
            // 
            this.cmbfloors.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbfloors.FormattingEnabled = true;
            this.cmbfloors.Location = new System.Drawing.Point(484, 80);
            this.cmbfloors.Margin = new System.Windows.Forms.Padding(2);
            this.cmbfloors.Name = "cmbfloors";
            this.cmbfloors.Size = new System.Drawing.Size(246, 28);
            this.cmbfloors.TabIndex = 5;
            this.cmbfloors.Visible = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(378, 119);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(89, 17);
            this.checkBox1.TabIndex = 7;
            this.checkBox1.Text = "Only Tax Bills";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(12, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 20);
            this.label6.TabIndex = 27;
            this.label6.Text = "Branch";
            // 
            // cmbbranch
            // 
            this.cmbbranch.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.cmbbranch.FormattingEnabled = true;
            this.cmbbranch.Location = new System.Drawing.Point(111, 112);
            this.cmbbranch.Margin = new System.Windows.Forms.Padding(2);
            this.cmbbranch.Name = "cmbbranch";
            this.cmbbranch.Size = new System.Drawing.Size(246, 28);
            this.cmbbranch.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(374, 48);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 20);
            this.label5.TabIndex = 14;
            this.label5.Text = "Type";
            // 
            // cmbtype
            // 
            this.cmbtype.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbtype.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbtype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtype.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbtype.FormattingEnabled = true;
            this.cmbtype.Items.AddRange(new object[] {
            "All",
            "Visa",
            "Cash"});
            this.cmbtype.Location = new System.Drawing.Point(484, 45);
            this.cmbtype.Name = "cmbtype";
            this.cmbtype.Size = new System.Drawing.Size(246, 28);
            this.cmbtype.TabIndex = 3;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(484, 18);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(246, 20);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(111, 18);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(246, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(374, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "End Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Start Date";
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.Location = new System.Drawing.Point(484, 142);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.Size = new System.Drawing.Size(118, 38);
            this.vButton1.TabIndex = 13;
            this.vButton1.Text = "View";
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.ORANGEFRESH;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Shift";
            // 
            // cmbshift
            // 
            this.cmbshift.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbshift.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbshift.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbshift.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbshift.FormattingEnabled = true;
            this.cmbshift.Location = new System.Drawing.Point(111, 45);
            this.cmbshift.Name = "cmbshift";
            this.cmbshift.Size = new System.Drawing.Size(246, 28);
            this.cmbshift.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(209, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 20);
            this.label8.TabIndex = 38;
            this.label8.Text = "To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(12, 156);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 20);
            this.label4.TabIndex = 37;
            this.label4.Text = "From";
            // 
            // cmbtimeto
            // 
            this.cmbtimeto.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbtimeto.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbtimeto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtimeto.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbtimeto.FormattingEnabled = true;
            this.cmbtimeto.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "00"});
            this.cmbtimeto.Location = new System.Drawing.Point(290, 152);
            this.cmbtimeto.Name = "cmbtimeto";
            this.cmbtimeto.Size = new System.Drawing.Size(67, 28);
            this.cmbtimeto.TabIndex = 11;
            // 
            // cmbtimefrom
            // 
            this.cmbtimefrom.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbtimefrom.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbtimefrom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbtimefrom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbtimefrom.FormattingEnabled = true;
            this.cmbtimefrom.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "00"});
            this.cmbtimefrom.Location = new System.Drawing.Point(111, 152);
            this.cmbtimefrom.Name = "cmbtimefrom";
            this.cmbtimefrom.Size = new System.Drawing.Size(67, 28);
            this.cmbtimefrom.TabIndex = 10;
            // 
            // chktime
            // 
            this.chktime.AutoSize = true;
            this.chktime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chktime.Location = new System.Drawing.Point(378, 155);
            this.chktime.Name = "chktime";
            this.chktime.Size = new System.Drawing.Size(99, 17);
            this.chktime.TabIndex = 12;
            this.chktime.Text = "Include Time";
            this.chktime.UseVisualStyleBackColor = true;
            // 
            // FrmInvoiceSale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(924, 542);
            this.Controls.Add(this.panel1);
            this.Name = "FrmInvoiceSale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Invoice Type Sale Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMwnuGroupSale_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbshift;
        private VIBlend.WinForms.Controls.vButton vButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbtype;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbbranch;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbordertype;
        private System.Windows.Forms.Label lblfloors;
        private System.Windows.Forms.ComboBox cmbfloors;
        private System.Windows.Forms.CheckBox chkaddress;
        private System.Windows.Forms.CheckBox chkphone;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbtimeto;
        private System.Windows.Forms.ComboBox cmbtimefrom;
        private System.Windows.Forms.CheckBox chktime;
    }
}