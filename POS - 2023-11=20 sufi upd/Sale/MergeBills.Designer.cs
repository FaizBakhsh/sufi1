namespace POSRestaurant.Sale
{
    partial class MergeBills
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.vButton3 = new VIBlend.WinForms.Controls.vButton();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Select});
            this.dataGridView1.Location = new System.Drawing.Point(8, 11);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(2);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(948, 663);
            this.dataGridView1.TabIndex = 1;
            // 
            // Select
            // 
            this.Select.HeaderText = "Select";
            this.Select.Name = "Select";
            // 
            // vButton3
            // 
            this.vButton3.AllowAnimations = true;
            this.vButton3.BackColor = System.Drawing.Color.Transparent;
            this.vButton3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.vButton3.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton3.Location = new System.Drawing.Point(12, 692);
            this.vButton3.Name = "vButton3";
            this.vButton3.Padding = new System.Windows.Forms.Padding(5);
            this.vButton3.RoundedCornersMask = ((byte)(15));
            this.vButton3.RoundedCornersRadius = 20;
            this.vButton3.Size = new System.Drawing.Size(422, 64);
            this.vButton3.TabIndex = 33;
            this.vButton3.Text = "Merge Selected Bills";
            this.vButton3.TextWrap = true;
            this.vButton3.UseVisualStyleBackColor = false;
            this.vButton3.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.METROBLUE;
            this.vButton3.Click += new System.EventHandler(this.vButton3_Click);
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.vButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton1.Location = new System.Drawing.Point(743, 692);
            this.vButton1.Name = "vButton1";
            this.vButton1.Padding = new System.Windows.Forms.Padding(5);
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.RoundedCornersRadius = 20;
            this.vButton1.Size = new System.Drawing.Size(203, 64);
            this.vButton1.TabIndex = 34;
            this.vButton1.Text = "Exit";
            this.vButton1.TextWrap = true;
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.METROBLUE;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // MergeBills
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(967, 768);
            this.Controls.Add(this.vButton1);
            this.Controls.Add(this.vButton3);
            this.Controls.Add(this.dataGridView1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "MergeBills";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Merge Bills";
            this.Load += new System.EventHandler(this.MergeBills_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Select;
        private VIBlend.WinForms.Controls.vButton vButton3;
        private VIBlend.WinForms.Controls.vButton vButton1;
    }
}