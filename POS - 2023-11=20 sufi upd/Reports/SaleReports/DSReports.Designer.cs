namespace POSRestaurant.Reports.SaleReports
{
    partial class DSReports
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
            this.vButton3 = new VIBlend.WinForms.Controls.vButton();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.vButton2 = new VIBlend.WinForms.Controls.vButton();
            this.vButton4 = new VIBlend.WinForms.Controls.vButton();
            this.SuspendLayout();
            // 
            // vButton3
            // 
            this.vButton3.AllowAnimations = true;
            this.vButton3.BackColor = System.Drawing.Color.Transparent;
            this.vButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton3.Location = new System.Drawing.Point(22, 25);
            this.vButton3.Name = "vButton3";
            this.vButton3.RoundedCornersMask = ((byte)(15));
            this.vButton3.Size = new System.Drawing.Size(198, 52);
            this.vButton3.TabIndex = 4;
            this.vButton3.Text = "DS Group Wise Sales Report";
            this.vButton3.UseVisualStyleBackColor = false;
            this.vButton3.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton3.Click += new System.EventHandler(this.vButton3_Click);
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton1.Location = new System.Drawing.Point(240, 25);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.Size = new System.Drawing.Size(198, 52);
            this.vButton1.TabIndex = 5;
            this.vButton1.Text = "DS Invoice Wise Report";
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // vButton2
            // 
            this.vButton2.AllowAnimations = true;
            this.vButton2.BackColor = System.Drawing.Color.Transparent;
            this.vButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton2.Location = new System.Drawing.Point(457, 25);
            this.vButton2.Name = "vButton2";
            this.vButton2.RoundedCornersMask = ((byte)(15));
            this.vButton2.Size = new System.Drawing.Size(198, 52);
            this.vButton2.TabIndex = 6;
            this.vButton2.Text = "DS Item Wise Report";
            this.vButton2.UseVisualStyleBackColor = false;
            this.vButton2.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton2.Click += new System.EventHandler(this.vButton2_Click);
            // 
            // vButton4
            // 
            this.vButton4.AllowAnimations = true;
            this.vButton4.BackColor = System.Drawing.Color.Transparent;
            this.vButton4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton4.Location = new System.Drawing.Point(22, 83);
            this.vButton4.Name = "vButton4";
            this.vButton4.RoundedCornersMask = ((byte)(15));
            this.vButton4.Size = new System.Drawing.Size(198, 52);
            this.vButton4.TabIndex = 5;
            this.vButton4.Text = "Sales Report";
            this.vButton4.UseVisualStyleBackColor = false;
            this.vButton4.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.NERO;
            this.vButton4.Click += new System.EventHandler(this.vButton4_Click);
            // 
            // DSReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 244);
            this.Controls.Add(this.vButton4);
            this.Controls.Add(this.vButton2);
            this.Controls.Add(this.vButton1);
            this.Controls.Add(this.vButton3);
            this.Name = "DSReports";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DS Reports";
            this.ResumeLayout(false);

        }

        #endregion

        private VIBlend.WinForms.Controls.vButton vButton3;
        private VIBlend.WinForms.Controls.vButton vButton1;
        private VIBlend.WinForms.Controls.vButton vButton2;
        private VIBlend.WinForms.Controls.vButton vButton4;
    }
}