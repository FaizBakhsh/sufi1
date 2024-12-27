namespace POSRestaurant.Reports
{
    partial class pra
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
            this.lbltotal = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbluploaded = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblpending = new System.Windows.Forms.Label();
            this.lblname = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(36, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(177, 31);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Orders";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // lbltotal
            // 
            this.lbltotal.AutoSize = true;
            this.lbltotal.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltotal.Location = new System.Drawing.Point(68, 140);
            this.lbltotal.Name = "lbltotal";
            this.lbltotal.Size = new System.Drawing.Size(23, 31);
            this.lbltotal.TabIndex = 1;
            this.lbltotal.Text = ".";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(274, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(235, 31);
            this.label3.TabIndex = 2;
            this.label3.Text = "Uploaded Orders";
            // 
            // lbluploaded
            // 
            this.lbluploaded.AutoSize = true;
            this.lbluploaded.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbluploaded.Location = new System.Drawing.Point(327, 140);
            this.lbluploaded.Name = "lbluploaded";
            this.lbluploaded.Size = new System.Drawing.Size(23, 31);
            this.lbluploaded.TabIndex = 3;
            this.lbluploaded.Text = ".";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(530, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(217, 31);
            this.label5.TabIndex = 4;
            this.label5.Text = "Pending Orders";
            // 
            // lblpending
            // 
            this.lblpending.AutoSize = true;
            this.lblpending.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblpending.Location = new System.Drawing.Point(580, 140);
            this.lblpending.Name = "lblpending";
            this.lblpending.Size = new System.Drawing.Size(23, 31);
            this.lblpending.TabIndex = 5;
            this.lblpending.Text = ".";
            // 
            // lblname
            // 
            this.lblname.AutoSize = true;
            this.lblname.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblname.Location = new System.Drawing.Point(36, 19);
            this.lblname.Name = "lblname";
            this.lblname.Size = new System.Drawing.Size(85, 31);
            this.lblname.TabIndex = 6;
            this.lblname.Text = "name";
            // 
            // pra
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 306);
            this.Controls.Add(this.lblname);
            this.Controls.Add(this.lblpending);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lbluploaded);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbltotal);
            this.Controls.Add(this.label1);
            this.Name = "pra";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Pra Orders";
            this.Load += new System.EventHandler(this.pra_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbltotal;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbluploaded;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblpending;
        private System.Windows.Forms.Label lblname;
    }
}