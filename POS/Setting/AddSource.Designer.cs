namespace POSRestaurant.Setting
{
    partial class AddSource
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
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.vButton3 = new VIBlend.WinForms.Controls.vButton();
            this.vButton2 = new VIBlend.WinForms.Controls.vButton();
            this.txtName = new VIBlend.WinForms.Controls.vTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.txtamount = new VIBlend.WinForms.Controls.vTextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbinvoice = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 15.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(65, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name ";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.vButton1);
            this.panel1.Controls.Add(this.vButton3);
            this.panel1.Controls.Add(this.vButton2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 228);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(662, 57);
            this.panel1.TabIndex = 4;
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton1.Location = new System.Drawing.Point(410, 7);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.RoundedCornersRadius = 8;
            this.vButton1.Size = new System.Drawing.Size(106, 41);
            this.vButton1.TabIndex = 6;
            this.vButton1.Text = "Clear";
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.OFFICE2003BLUE;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // vButton3
            // 
            this.vButton3.AllowAnimations = true;
            this.vButton3.BackColor = System.Drawing.Color.Transparent;
            this.vButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton3.Location = new System.Drawing.Point(539, 7);
            this.vButton3.Name = "vButton3";
            this.vButton3.RoundedCornersMask = ((byte)(15));
            this.vButton3.RoundedCornersRadius = 8;
            this.vButton3.Size = new System.Drawing.Size(106, 41);
            this.vButton3.TabIndex = 7;
            this.vButton3.Text = "Exit";
            this.vButton3.UseVisualStyleBackColor = false;
            this.vButton3.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.OFFICE2003BLUE;
            this.vButton3.Click += new System.EventHandler(this.vButton3_Click);
            // 
            // vButton2
            // 
            this.vButton2.AllowAnimations = true;
            this.vButton2.BackColor = System.Drawing.Color.Transparent;
            this.vButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton2.Location = new System.Drawing.Point(287, 7);
            this.vButton2.Name = "vButton2";
            this.vButton2.RoundedCornersMask = ((byte)(15));
            this.vButton2.RoundedCornersRadius = 8;
            this.vButton2.Size = new System.Drawing.Size(106, 41);
            this.vButton2.TabIndex = 5;
            this.vButton2.Text = "Submit";
            this.vButton2.UseVisualStyleBackColor = false;
            this.vButton2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.OFFICE2003BLUE;
            this.vButton2.Click += new System.EventHandler(this.vButton2_Click);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.BoundsOffset = new System.Drawing.Size(1, 1);
            this.txtName.ControlBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.txtName.DefaultText = "Empty...";
            this.txtName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(266, 44);
            this.txtName.MaxLength = 32767;
            this.txtName.Name = "txtName";
            this.txtName.PasswordChar = '\0';
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtName.SelectionLength = 0;
            this.txtName.SelectionStart = 0;
            this.txtName.Size = new System.Drawing.Size(288, 25);
            this.txtName.TabIndex = 1;
            this.txtName.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtName.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 15.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(65, 160);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 24);
            this.label2.TabIndex = 8;
            this.label2.Text = "Status";
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Active",
            "Inactive"});
            this.comboBox1.Location = new System.Drawing.Point(266, 160);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(288, 24);
            this.comboBox1.TabIndex = 3;
            // 
            // txtamount
            // 
            this.txtamount.BackColor = System.Drawing.Color.White;
            this.txtamount.BoundsOffset = new System.Drawing.Size(1, 1);
            this.txtamount.ControlBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.txtamount.DefaultText = "Empty...";
            this.txtamount.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtamount.Location = new System.Drawing.Point(266, 86);
            this.txtamount.MaxLength = 32767;
            this.txtamount.Name = "txtamount";
            this.txtamount.PasswordChar = '\0';
            this.txtamount.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtamount.SelectionLength = 0;
            this.txtamount.SelectionStart = 0;
            this.txtamount.Size = new System.Drawing.Size(288, 25);
            this.txtamount.TabIndex = 2;
            this.txtamount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.txtamount.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 15.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(65, 87);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(198, 24);
            this.label3.TabIndex = 8;
            this.label3.Text = "Price Increament(%)";
            // 
            // cmbinvoice
            // 
            this.cmbinvoice.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbinvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold);
            this.cmbinvoice.FormattingEnabled = true;
            this.cmbinvoice.Items.AddRange(new object[] {
            "yes",
            "no"});
            this.cmbinvoice.Location = new System.Drawing.Point(266, 122);
            this.cmbinvoice.Name = "cmbinvoice";
            this.cmbinvoice.Size = new System.Drawing.Size(288, 24);
            this.cmbinvoice.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 15.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(65, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(125, 24);
            this.label4.TabIndex = 10;
            this.label4.Text = "Print Invoice";
            // 
            // AddSource
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MediumOrchid;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(662, 285);
            this.Controls.Add(this.cmbinvoice);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtamount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "AddSource";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Groups";
            this.Load += new System.EventHandler(this.AddGroups_Load);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        public int editmode;
        public string id;
        private VIBlend.WinForms.Controls.vButton vButton2;
        private VIBlend.WinForms.Controls.vButton vButton1;
        private VIBlend.WinForms.Controls.vButton vButton3;
        private VIBlend.WinForms.Controls.vTextBox txtName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private VIBlend.WinForms.Controls.vTextBox txtamount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbinvoice;
        private System.Windows.Forms.Label label4;
    }
}                                                                                                                                                                                                                                                                          min.frmDishmanagement.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmSupplierAdd.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmSupplierManagement.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmAddItemPackingType.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmAddItemSubCategory.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmItemSubCategory.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmAddItemrecipe.resources
E:\timmys\POS\obj\Debug\POSRestaurant.ControlPanel.frmManageUsers.resources
E:\timmys\POS\obj\Debug\POSRestaurant.ControlPanel.frmAddEditUser.resources
E:\timmys\POS\obj\Debug\POSRestaurant.ControlPanel.frmGlobalSettings.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmItemrecipemang.resources
E:\timmys\POS\obj\Debug\POSRestaurant.controls.BarCodeCtrl.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.AddMovies.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.AddMovieShows.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.BackendForm.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.BillRecall.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.DataBackUp.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.DataSetting.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.DataUploading.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.Form1.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.frmdbconnector.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.login.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.MainForm.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.Reports.resources
E:\timmys\POS\obj\Debug\POSRestaurant.forms.RestAllowDiscount.resources
E:\timmys\POS\obj\Debug\POSRestaurant.Properties.Resources.resources
E:\timmys\POS\obj\Debug\POSRestaurant.RawItems.AddRawItems.resources
E:\timmys\POS\obj\Debug\POSRestaurant.RawItems.PurChase_List.resources
E:\timmys\POS\obj\Debug\POSRestaurant.RawItems.Purchase.resources
E:\t