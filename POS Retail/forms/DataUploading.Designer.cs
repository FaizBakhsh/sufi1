namespace POSRetail.forms
{
    partial class DataUploading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataUploading));
            this.vLabel1 = new VIBlend.WinForms.Controls.vLabel();
            this.vLabel2 = new VIBlend.WinForms.Controls.vLabel();
            this.vProgressBar1 = new VIBlend.WinForms.Controls.vProgressBar();
            this.SuspendLayout();
            // 
            // vLabel1
            // 
            this.vLabel1.BackColor = System.Drawing.Color.Transparent;
            this.vLabel1.DisplayStyle = VIBlend.WinForms.Controls.LabelItemStyle.TextOnly;
            this.vLabel1.Ellipsis = false;
            this.vLabel1.Font = new System.Drawing.Font("Times New Roman", 17.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vLabel1.ImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel1.Location = new System.Drawing.Point(222, 6);
            this.vLabel1.Multiline = true;
            this.vLabel1.Name = "vLabel1";
            this.vLabel1.Size = new System.Drawing.Size(249, 32);
            this.vLabel1.TabIndex = 0;
            this.vLabel1.Text = "Uploading Data Online";
            this.vLabel1.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel1.UseMnemonics = true;
            this.vLabel1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.ECOGREEN;
            // 
            // vLabel2
            // 
            this.vLabel2.BackColor = System.Drawing.Color.Transparent;
            this.vLabel2.DisplayStyle = VIBlend.WinForms.Controls.LabelItemStyle.TextOnly;
            this.vLabel2.Ellipsis = false;
            this.vLabel2.Font = new System.Drawing.Font("Bell MT", 17.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vLabel2.ImageAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel2.Location = new System.Drawing.Point(183, 181);
            this.vLabel2.Multiline = true;
            this.vLabel2.Name = "vLabel2";
            this.vLabel2.Size = new System.Drawing.Size(322, 32);
            this.vLabel2.TabIndex = 1;
            this.vLabel2.Text = "Please Wait While Uploading";
            this.vLabel2.TextAlignment = System.Drawing.ContentAlignment.TopLeft;
            this.vLabel2.UseMnemonics = true;
            this.vLabel2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.ECOGREEN;
            // 
            // vProgressBar1
            // 
            this.vProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.vProgressBar1.Location = new System.Drawing.Point(29, 91);
            this.vProgressBar1.Name = "vProgressBar1";
            this.vProgressBar1.RoundedCornersMask = ((byte)(15));
            this.vProgressBar1.Size = new System.Drawing.Size(643, 31);
            this.vProgressBar1.TabIndex = 2;
            this.vProgressBar1.Text = "vProgressBar1";
            this.vProgressBar1.Value = 0;
            this.vProgressBar1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            // 
            // DataUploading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(697, 239);
            this.Controls.Add(this.vProgressBar1);
            this.Controls.Add(this.vLabel2);
            this.Controls.Add(this.vLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "DataUploading";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Data Uploading";
            this.Load += new System.EventHandler(this.DataUploading_Load);
            this.Shown += new System.EventHandler(this.DataUploading_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private VIBlend.WinForms.Controls.vLabel vLabel1;
        private VIBlend.WinForms.Controls.vLabel vLabel2;
        private VIBlend.WinForms.Controls.vProgressBar vProgressBar1;
    }
}