namespace POSRestaurant.Reports.Statements
{
    partial class frmGLAccounts
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbsubaccount = new System.Windows.Forms.ComboBox();
            this.chkcredit = new System.Windows.Forms.CheckBox();
            this.chkdebit = new System.Windows.Forms.CheckBox();
            this.chkempty = new System.Windows.Forms.CheckBox();
            this.chkgroup = new System.Windows.Forms.CheckBox();
            this.chksum = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbsubhead = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbhead = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbbranchjv = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbaccount = new System.Windows.Forms.ComboBox();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.cmbsubaccount);
            this.panel2.Controls.Add(this.chkcredit);
            this.panel2.Controls.Add(this.chkdebit);
            this.panel2.Controls.Add(this.chkempty);
            this.panel2.Controls.Add(this.chkgroup);
            this.panel2.Controls.Add(this.chksum);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.cmbsubhead);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cmbhead);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.cmbbranchjv);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.dateTimePicker2);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cmbaccount);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1214, 149);
            this.panel2.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(529, 49);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(87, 20);
            this.label7.TabIndex = 93;
            this.label7.Text = "2nd Head";
            // 
            // cmbsubaccount
            // 
            this.cmbsubaccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbsubaccount.FormattingEnabled = true;
            this.cmbsubaccount.Location = new System.Drawing.Point(622, 47);
            this.cmbsubaccount.Name = "cmbsubaccount";
            this.cmbsubaccount.Size = new System.Drawing.Size(187, 24);
            this.cmbsubaccount.TabIndex = 5;
            this.cmbsubaccount.SelectedIndexChanged += new System.EventHandler(this.cmbsubaccount_SelectedIndexChanged);
            this.cmbsubaccount.SelectedValueChanged += new System.EventHandler(this.cmbsubaccount_SelectedValueChanged);
            // 
            // chkcredit
            // 
            this.chkcredit.AutoSize = true;
            this.chkcredit.Location = new System.Drawing.Point(650, 125);
            this.chkcredit.Name = "chkcredit";
            this.chkcredit.Size = new System.Drawing.Size(77, 17);
            this.chkcredit.TabIndex = 12;
            this.chkcredit.Text = "Only Credit";
            this.chkcredit.UseVisualStyleBackColor = true;
            this.chkcredit.CheckedChanged += new System.EventHandler(this.chkcredit_CheckedChanged);
            // 
            // chkdebit
            // 
            this.chkdebit.AutoSize = true;
            this.chkdebit.Location = new System.Drawing.Point(531, 125);
            this.chkdebit.Name = "chkdebit";
            this.chkdebit.Size = new System.Drawing.Size(75, 17);
            this.chkdebit.TabIndex = 11;
            this.chkdebit.Text = "Only Debit";
            this.chkdebit.UseVisualStyleBackColor = true;
            this.chkdebit.CheckedChanged += new System.EventHandler(this.chkdebit_CheckedChanged);
            // 
            // chkempty
            // 
            this.chkempty.AutoSize = true;
            this.chkempty.Location = new System.Drawing.Point(650, 103);
            this.chkempty.Name = "chkempty";
            this.chkempty.Size = new System.Drawing.Size(155, 17);
            this.chkempty.TabIndex = 10;
            this.chkempty.Text = "Exclude Empty Transaction";
            this.chkempty.UseVisualStyleBackColor = true;
            // 
            // chkgroup
            // 
            this.chkgroup.AutoSize = true;
            this.chkgroup.Checked = true;
            this.chkgroup.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkgroup.Location = new System.Drawing.Point(531, 82);
            this.chkgroup.Name = "chkgroup";
            this.chkgroup.Size = new System.Drawing.Size(113, 17);
            this.chkgroup.TabIndex = 7;
            this.chkgroup.Text = "Group By Account";
            this.chkgroup.UseVisualStyleBackColor = true;
            this.chkgroup.CheckedChanged += new System.EventHandler(this.chkgroup_CheckedChanged);
            // 
            // chksum
            // 
            this.chksum.AutoSize = true;
            this.chksum.Location = new System.Drawing.Point(531, 103);
            this.chksum.Name = "chksum";
            this.chksum.Size = new System.Drawing.Size(71, 17);
            this.chksum.TabIndex = 9;
            this.chksum.Text = "Only Sum";
            this.chksum.UseVisualStyleBackColor = true;
            this.chksum.CheckedChanged += new System.EventHandler(this.chksum_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(650, 82);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(104, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Include Opening";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(273, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(89, 20);
            this.label6.TabIndex = 85;
            this.label6.Text = "Sub Head";
            // 
            // cmbsubhead
            // 
            this.cmbsubhead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbsubhead.FormattingEnabled = true;
            this.cmbsubhead.Location = new System.Drawing.Point(364, 49);
            this.cmbsubhead.Name = "cmbsubhead";
            this.cmbsubhead.Size = new System.Drawing.Size(159, 24);
            this.cmbsubhead.TabIndex = 4;
            this.cmbsubhead.SelectedIndexChanged += new System.EventHandler(this.cmbsubhead_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(12, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 20);
            this.label5.TabIndex = 83;
            this.label5.Text = "Head";
            // 
            // cmbhead
            // 
            this.cmbhead.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbhead.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbhead.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbhead.FormattingEnabled = true;
            this.cmbhead.Items.AddRange(new object[] {
            "All",
            "Assets",
            "Liabilities",
            "Equity and Capital",
            "Revenue",
            "Cost of Sales",
            "Expenses"});
            this.cmbhead.Location = new System.Drawing.Point(105, 49);
            this.cmbhead.Name = "cmbhead";
            this.cmbhead.Size = new System.Drawing.Size(159, 24);
            this.cmbhead.TabIndex = 3;
            this.cmbhead.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(529, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 20);
            this.label4.TabIndex = 81;
            this.label4.Text = "Branch";
            // 
            // cmbbranchjv
            // 
            this.cmbbranchjv.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbbranchjv.FormattingEnabled = true;
            this.cmbbranchjv.Location = new System.Drawing.Point(622, 13);
            this.cmbbranchjv.Name = "cmbbranchjv";
            this.cmbbranchjv.Size = new System.Drawing.Size(187, 24);
            this.cmbbranchjv.TabIndex = 2;
            this.cmbbranchjv.SelectedIndexChanged += new System.EventHandler(this.cmbbranchjv_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(815, 13);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(144, 80);
            this.button1.TabIndex = 13;
            this.button1.Text = "Submit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(364, 17);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(159, 20);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(105, 17);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(159, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(273, 18);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Account";
            // 
            // cmbaccount
            // 
            this.cmbaccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbaccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbaccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbaccount.FormattingEnabled = true;
            this.cmbaccount.Location = new System.Drawing.Point(105, 114);
            this.cmbaccount.Name = "cmbaccount";
            this.cmbaccount.Size = new System.Drawing.Size(418, 24);
            this.cmbaccount.TabIndex = 6;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 149);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(1214, 688);
            this.crystalReportViewer1.TabIndex = 2;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            this.crystalReportViewer1.DoubleClickPage += new CrystalDecisions.Windows.Forms.PageMouseEventHandler(this.crystalReportViewer1_DoubleClickPage);
            // 
            // frmGLAccounts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1214, 837);
            this.Controls.Add(this.crystalReportViewer1);
            this.Controls.Add(this.panel2);
            this.Name = "frmGLAccounts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GL Accounts";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmPayableStatemetBank_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbaccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbbranchjv;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbhead;
        private System.Windows.Forms.ComboBox cmbsubhead;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox chksum;
        private System.Windows.Forms.CheckBox chkgroup;
        private System.Windows.Forms.CheckBox chkempty;
        private System.Windows.Forms.CheckBox chkcredit;
        private System.Windows.Forms.CheckBox chkdebit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbsubaccount;
    }
}