namespace POSRestaurant.forms
{
    partial class AddMovies
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
            this.vTextBox1 = new VIBlend.WinForms.Controls.vTextBox();
            this.vLabel1 = new VIBlend.WinForms.Controls.vLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.vLabel2 = new VIBlend.WinForms.Controls.vLabel();
            this.vButton2 = new VIBlend.WinForms.Controls.vButton();
            this.vButton3 = new VIBlend.WinForms.Controls.vButton();
            this.vButton4 = new VIBlend.WinForms.Controls.vButton();
            this.vButton5 = new VIBlend.WinForms.Controls.vButton();
            this.vButton6 = new VIBlend.WinForms.Controls.vButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // vTextBox1
            // 
            this.vTextBox1.BackColor = System.Drawing.Color.White;
            this.vTextBox1.BoundsOffset = new System.Drawing.Size(1, 1);
            this.vTextBox1.ControlBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(39)))), ((int)(((byte)(39)))));
            this.vTextBox1.DefaultText = "Empty...";
            this.vTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vTextBox1.Location = new System.Drawing.Point(109, 27);
            this.vTextBox1.MaxLength = 32767;
            this.vTextBox1.Name = "vTextBox1";
            this.vTextBox1.PasswordChar = '\0';
            this.vTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.vTextBox1.SelectionLength = 0;
            this.vTextBox1.SelectionStart = 0;
            this.vTextBox1.Size = new System.Drawing.Size(238, 28);
            this.vTextBox1.TabIndex = 0;
            this.vTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.vTextBox1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vLabel1
            // 
            this.vLabel1.BackColor = System.Drawing.Color.Transparent;
            this.vLabel1.DisplayStyle = VIBlend.WinForms.Controls.LabelItemStyle.TextOnly;
            this.vLabel1.Ellipsis = false;
            this.vLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vLabel1.ForeColor = System.Drawing.Color.White;
            this.vLabel1.ImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel1.Location = new System.Drawing.Point(12, 33);
            this.vLabel1.Multiline = true;
            this.vLabel1.Name = "vLabel1";
            this.vLabel1.Size = new System.Drawing.Size(91, 25);
            this.vLabel1.TabIndex = 1;
            this.vLabel1.Text = "Movie Name";
            this.vLabel1.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel1.UseMnemonics = true;
            this.vLabel1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton1.Location = new System.Drawing.Point(708, 25);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.Size = new System.Drawing.Size(100, 30);
            this.vButton1.TabIndex = 2;
            this.vButton1.Text = "Image";
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Active",
            "InActive"});
            this.comboBox1.Location = new System.Drawing.Point(456, 27);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(238, 28);
            this.comboBox1.TabIndex = 3;
            // 
            // vLabel2
            // 
            this.vLabel2.BackColor = System.Drawing.Color.Transparent;
            this.vLabel2.DisplayStyle = VIBlend.WinForms.Controls.LabelItemStyle.TextOnly;
            this.vLabel2.Ellipsis = false;
            this.vLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vLabel2.ForeColor = System.Drawing.Color.White;
            this.vLabel2.ImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel2.Location = new System.Drawing.Point(356, 32);
            this.vLabel2.Multiline = true;
            this.vLabel2.Name = "vLabel2";
            this.vLabel2.Size = new System.Drawing.Size(93, 25);
            this.vLabel2.TabIndex = 2;
            this.vLabel2.Text = "Movie Status";
            this.vLabel2.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel2.UseMnemonics = true;
            this.vLabel2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.VISTABLUE;
            // 
            // vButton2
            // 
            this.vButton2.AllowAnimations = true;
            this.vButton2.BackColor = System.Drawing.Color.Transparent;
            this.vButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton2.Location = new System.Drawing.Point(109, 84);
            this.vButton2.Name = "vButton2";
            this.vButton2.RoundedCornersMask = ((byte)(15));
            this.vButton2.Size = new System.Drawing.Size(86, 39);
            this.vButton2.TabIndex = 3;
            this.vButton2.Text = "Save";
            this.vButton2.UseVisualStyleBackColor = false;
            this.vButton2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton2.Click += new System.EventHandler(this.vButton2_Click);
            // 
            // vButton3
            // 
            this.vButton3.AllowAnimations = true;
            this.vButton3.BackColor = System.Drawing.Color.Transparent;
            this.vButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton3.Location = new System.Drawing.Point(201, 84);
            this.vButton3.Name = "vButton3";
            this.vButton3.RoundedCornersMask = ((byte)(15));
            this.vButton3.Size = new System.Drawing.Size(86, 39);
            this.vButton3.TabIndex = 4;
            this.vButton3.Text = "Clear";
            this.vButton3.UseVisualStyleBackColor = false;
            this.vButton3.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton3.Click += new System.EventHandler(this.vButton3_Click);
            // 
            // vButton4
            // 
            this.vButton4.AllowAnimations = true;
            this.vButton4.BackColor = System.Drawing.Color.Transparent;
            this.vButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton4.Location = new System.Drawing.Point(293, 84);
            this.vButton4.Name = "vButton4";
            this.vButton4.RoundedCornersMask = ((byte)(15));
            this.vButton4.Size = new System.Drawing.Size(86, 39);
            this.vButton4.TabIndex = 5;
            this.vButton4.Text = "Exit";
            this.vButton4.UseVisualStyleBackColor = false;
            this.vButton4.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            // 
            // vButton5
            // 
            this.vButton5.AllowAnimations = true;
            this.vButton5.BackColor = System.Drawing.Color.Transparent;
            this.vButton5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton5.Location = new System.Drawing.Point(765, 362);
            this.vButton5.Name = "vButton5";
            this.vButton5.RoundedCornersMask = ((byte)(15));
            this.vButton5.Size = new System.Drawing.Size(122, 39);
            this.vButton5.TabIndex = 6;
            this.vButton5.Text = "Delete Selected";
            this.vButton5.UseVisualStyleBackColor = false;
            this.vButton5.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton5.Click += new System.EventHandler(this.vButton5_Click);
            // 
            // vButton6
            // 
            this.vButton6.AllowAnimations = true;
            this.vButton6.BackColor = System.Drawing.Color.Transparent;
            this.vButton6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton6.Location = new System.Drawing.Point(637, 362);
            this.vButton6.Name = "vButton6";
            this.vButton6.RoundedCornersMask = ((byte)(15));
            this.vButton6.Size = new System.Drawing.Size(122, 39);
            this.vButton6.TabIndex = 5;
            this.vButton6.Text = "Edit Selected";
            this.vButton6.UseVisualStyleBackColor = false;
            this.vButton6.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton6.Click += new System.EventHandler(this.vButton6_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 135);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(953, 221);
            this.dataGridView1.TabIndex = 7;
            // 
            // AddMovies
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(977, 413);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.vButton5);
            this.Controls.Add(this.vButton4);
            this.Controls.Add(this.vButton6);
            this.Controls.Add(this.vButton3);
            this.Controls.Add(this.vButton2);
            this.Controls.Add(this.vLabel2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.vButton1);
            this.Controls.Add(this.vLabel1);
            this.Controls.Add(this.vTextBox1);
            this.Name = "AddMovies";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Movies";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.AddMovies_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private VIBlend.WinForms.Controls.vTextBox vTextBox1;
        private VIBlend.WinForms.Controls.vLabel vLabel1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private VIBlend.WinForms.Controls.vButton vButton1;
        private System.Windows.Forms.ComboBox comboBox1;
        private VIBlend.WinForms.Controls.vLabel vLabel2;
        private VIBlend.WinForms.Controls.vButton vButton2;
        private VIBlend.WinForms.Controls.vButton vButton3;
        private VIBlend.WinForms.Controls.vButton vButton4;
        private VIBlend.WinForms.Controls.vButton vButton5;
        private VIBlend.WinForms.Controls.vButton vButton6;
        private System.Windows.Forms.DataGridView dataGridView1;
    }
}