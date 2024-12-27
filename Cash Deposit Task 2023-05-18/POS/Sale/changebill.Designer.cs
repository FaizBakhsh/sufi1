namespace POSRestaurant.Sale
{
    partial class changebill
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
            this.vButton16 = new VIBlend.WinForms.Controls.vButton();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.vButton2 = new VIBlend.WinForms.Controls.vButton();
            this.SuspendLayout();
            // 
            // vButton16
            // 
            this.vButton16.AllowAnimations = true;
            this.vButton16.BackColor = System.Drawing.Color.Transparent;
            this.vButton16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vButton16.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vButton16.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.vButton16.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton16.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.vButton16.Location = new System.Drawing.Point(50, 24);
            this.vButton16.Name = "vButton16";
            this.vButton16.RoundedCornersMask = ((byte)(15));
            this.vButton16.RoundedCornersRadius = 8;
            this.vButton16.Size = new System.Drawing.Size(126, 49);
            this.vButton16.TabIndex = 21;
            this.vButton16.Text = "Rename";
            this.vButton16.TextAbsolutePosition = new System.Drawing.Point(10, 5);
            this.vButton16.UseVisualStyleBackColor = false;
            this.vButton16.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.ULTRABLUE;
            this.vButton16.Click += new System.EventHandler(this.vButton16_Click);
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.vButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton1.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.vButton1.Location = new System.Drawing.Point(250, 213);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.RoundedCornersRadius = 8;
            this.vButton1.Size = new System.Drawing.Size(187, 93);
            this.vButton1.TabIndex = 22;
            this.vButton1.Text = "Exit";
            this.vButton1.TextAbsolutePosition = new System.Drawing.Point(10, 5);
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.ULTRABLUE;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // vButton2
            // 
            this.vButton2.AllowAnimations = true;
            this.vButton2.BackColor = System.Drawing.Color.Transparent;
            this.vButton2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.vButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.vButton2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.vButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.vButton2.Location = new System.Drawing.Point(199, 24);
            this.vButton2.Name = "vButton2";
            this.vButton2.RoundedCornersMask = ((byte)(15));
            this.vButton2.RoundedCornersRadius = 8;
            this.vButton2.Size = new System.Drawing.Size(126, 49);
            this.vButton2.TabIndex = 22;
            this.vButton2.Text = "ReOpen";
            this.vButton2.TextAbsolutePosition = new System.Drawing.Point(10, 5);
            this.vButton2.UseVisualStyleBackColor = false;
            this.vButton2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.ULTRABLUE;
            this.vButton2.Click += new System.EventHandler(this.vButton2_Click);
            // 
            // changebill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(731, 316);
            this.Controls.Add(this.vButton2);
            this.Controls.Add(this.vButton1);
            this.Controls.Add(this.vButton16);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "changebill";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.changebill_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private VIBlend.WinForms.Controls.vButton vButton16;
        private VIBlend.WinForms.Controls.vButton vButton1;
        private VIBlend.WinForms.Controls.vButton vButton2;
    }
}