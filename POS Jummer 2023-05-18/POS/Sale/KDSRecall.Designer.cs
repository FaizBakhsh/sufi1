﻿namespace POSRestaurant.Sale
{
    partial class KDSRecall
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
            this.components = new System.ComponentModel.Container();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tblmain2 = new System.Windows.Forms.TableLayoutPanel();
            this.vButton1 = new VIBlend.WinForms.Controls.vButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.vButton1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 98F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1547, 638);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tblmain2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(4, 102);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1539, 532);
            this.panel2.TabIndex = 1;
            // 
            // tblmain2
            // 
            this.tblmain2.AutoScroll = true;
            this.tblmain2.ColumnCount = 1;
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 400F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tblmain2.Location = new System.Drawing.Point(9, 6);
            this.tblmain2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tblmain2.Name = "tblmain2";
            this.tblmain2.RowCount = 1;
            this.tblmain2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblmain2.Size = new System.Drawing.Size(199, 503);
            this.tblmain2.TabIndex = 1;
            // 
            // vButton1
            // 
            this.vButton1.AllowAnimations = true;
            this.vButton1.BackColor = System.Drawing.Color.Transparent;
            this.vButton1.Dock = System.Windows.Forms.DockStyle.Right;
            this.vButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.vButton1.Location = new System.Drawing.Point(733, 4);
            this.vButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.vButton1.Name = "vButton1";
            this.vButton1.RoundedCornersMask = ((byte)(15));
            this.vButton1.Size = new System.Drawing.Size(810, 90);
            this.vButton1.TabIndex = 2;
            this.vButton1.Text = "Back";
            this.vButton1.UseVisualStyleBackColor = false;
            this.vButton1.VIBlendTheme = VIBlend.Utilities.VIBLEND_THEME.METROORANGE;
            this.vButton1.Click += new System.EventHandler(this.vButton1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // KDSRecall
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Purple;
            this.ClientSize = new System.Drawing.Size(1547, 638);
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "KDSRecall";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "KDSNEW";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.KDSNEW_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tblmain2;
        private VIBlend.WinForms.Controls.vButton vButton1;
        private System.Windows.Forms.Timer timer1;
    }
}                      Transaction.resources
E:\POS\obj\Debug\POSRestaurant.Sale.TabltOrders.resources
E:\POS\obj\Debug\POSRestaurant.Reports.Members.frmMembers.resources
E:\POS\obj\Debug\POSRestaurant.Reports.Members.frmCustomerPoints.resources
E:\timmys\POS\bin\Debug\MBLFront.exe.config
E:\timmys\POS\bin\Debug\MBLFront.exe.manifest
E:\timmys\POS\bin\Debug\MBLFront.application
E:\timmys\POS\bin\Debug\MBLFront.exe
E:\timmys\POS\bin\Debug\MBLFront.pdb
E:\timmys\POS\bin\Debug\POSRetail.exe
E:\timmys\POS\bin\Debug\POSRetail.pdb
E:\timmys\POS\bin\Debug\Interop.VBA.dll
E:\timmys\POS\obj\Debug\POSRestaurant.csprojResolveAssemblyReference.cache
E:\timmys\POS\obj\Debug\Interop.VBA.dll
E:\timmys\POS\obj\Debug\POSRestaurant.csproj.ResolveComReference.cache
E:\timmys\POS\obj\Debug\POSRestaurant.Accounts.ChartofAccount.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.backup.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmadditems.resources
E:\timmys\POS\obj\Debug\POSRestaurant.admin.frmAddstaff.resources
E:\tim