namespace POSRetail.forms
{
    partial class updatesales
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
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chksales = new System.Windows.Forms.CheckBox();
            this.chkinv = new System.Windows.Forms.CheckBox();
            this.chkclosing = new System.Windows.Forms.CheckBox();
            this.chktransfer = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkexpenses = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(149, 90);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(138, 20);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.CustomFormat = "yyyy-MM-dd";
            this.dateTimePicker2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePicker2.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker2.Location = new System.Drawing.Point(347, 90);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(138, 20);
            this.dateTimePicker2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(146, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 24);
            this.label1.TabIndex = 2;
            this.label1.Text = "Start";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(343, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 24);
            this.label2.TabIndex = 3;
            this.label2.Text = "End";
            // 
            // chksales
            // 
            this.chksales.AutoSize = true;
            this.chksales.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chksales.Location = new System.Drawing.Point(67, 138);
            this.chksales.Name = "chksales";
            this.chksales.Size = new System.Drawing.Size(73, 24);
            this.chksales.TabIndex = 4;
            this.chksales.Text = "Sales";
            this.chksales.UseVisualStyleBackColor = true;
            // 
            // chkinv
            // 
            this.chkinv.AutoSize = true;
            this.chkinv.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkinv.Location = new System.Drawing.Point(159, 138);
            this.chkinv.Name = "chkinv";
            this.chkinv.Size = new System.Drawing.Size(192, 24);
            this.chkinv.TabIndex = 5;
            this.chkinv.Text = "Inventory Consumed";
            this.chkinv.UseVisualStyleBackColor = true;
            // 
            // chkclosing
            // 
            this.chkclosing.AutoSize = true;
            this.chkclosing.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkclosing.Location = new System.Drawing.Point(374, 138);
            this.chkclosing.Name = "chkclosing";
            this.chkclosing.Size = new System.Drawing.Size(132, 24);
            this.chkclosing.TabIndex = 6;
            this.chkclosing.Text = "Closing/Staff";
            this.chkclosing.UseVisualStyleBackColor = true;
            // 
            // chktransfer
            // 
            this.chktransfer.AutoSize = true;
            this.chktransfer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chktransfer.Location = new System.Drawing.Point(521, 138);
            this.chktransfer.Name = "chktransfer";
            this.chktransfer.Size = new System.Drawing.Size(124, 24);
            this.chktransfer.TabIndex = 7;
            this.chktransfer.Text = "Inv Transfer";
            this.chktransfer.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(288, 191);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(197, 91);
            this.button1.TabIndex = 8;
            this.button1.Text = "Update";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkexpenses
            // 
            this.chkexpenses.AutoSize = true;
            this.chkexpenses.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkexpenses.Location = new System.Drawing.Point(651, 138);
            this.chkexpenses.Name = "chkexpenses";
            this.chkexpenses.Size = new System.Drawing.Size(106, 24);
            this.chkexpenses.TabIndex = 9;
            this.chkexpenses.Text = "Expenses";
            this.chkexpenses.UseVisualStyleBackColor = true;
            // 
            // updatesales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 424);
            this.Controls.Add(this.chkexpenses);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.chktransfer);
            this.Controls.Add(this.chkclosing);
            this.Controls.Add(this.chkinv);
            this.Controls.Add(this.chksales);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dateTimePicker2);
            this.Controls.Add(this.dateTimePicker1);
            this.Name = "updatesales";
            this.Text = "updatesales";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chksales;
        private System.Windows.Forms.CheckBox chkinv;
        private System.Windows.Forms.CheckBox chkclosing;
        private System.Windows.Forms.CheckBox chktransfer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkexpenses;
    }
}