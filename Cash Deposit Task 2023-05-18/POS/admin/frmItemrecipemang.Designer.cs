namespace POSRestaurant.admin
{
    partial class frmItemrecipemang
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
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lvList = new System.Windows.Forms.ListView();
            this.ItemCategory_Name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.pbxClose = new System.Windows.Forms.PictureBox();
            this.cmdRefresh = new System.Windows.Forms.PictureBox();
            this.pbxDelete = new System.Windows.Forms.PictureBox();
            this.pbxEdit = new System.Windows.Forms.PictureBox();
            this.cmdAdd = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbxClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRefresh)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDelete)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxEdit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdAdd)).BeginInit();
            this.SuspendLayout();
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "S.No";
            this.columnHeader1.Width = 82;
            // 
            // lvList
            // 
            this.lvList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.ItemCategory_Name,
            this.columnHeader2});
            this.lvList.FullRowSelect = true;
            this.lvList.GridLines = true;
            this.lvList.Location = new System.Drawing.Point(4, 3);
            this.lvList.Name = "lvList";
            this.lvList.Size = new System.Drawing.Size(513, 212);
            this.lvList.TabIndex = 6;
            this.lvList.UseCompatibleStateImageBehavior = false;
            this.lvList.View = System.Windows.Forms.View.Details;
            // 
            // ItemCategory_Name
            // 
            this.ItemCategory_Name.Text = "Item";
            this.ItemCategory_Name.Width = 158;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Quantity/Unit";
            this.columnHeader2.Width = 251;
            // 
            // pbxClose
            // 
            this.pbxClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbxClose.Image = global::POSRestaurant.Properties.Resources.Cancel_icon;
            this.pbxClose.Location = new System.Drawing.Point(538, 178);
            this.pbxClose.Name = "pbxClose";
            this.pbxClose.Size = new System.Drawing.Size(90, 37);
            this.pbxClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxClose.TabIndex = 17;
            this.pbxClose.TabStop = false;
            this.pbxClose.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdRefresh.Image = global::POSRestaurant.Properties.Resources.Window_Refresh_icon;
            this.cmdRefresh.Location = new System.Drawing.Point(538, 138);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(90, 37);
            this.cmdRefresh.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.cmdRefresh.TabIndex = 16;
            this.cmdRefresh.TabStop = false;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // pbxDelete
            // 
            this.pbxDelete.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbxDelete.Image = global::POSRestaurant.Properties.Resources.Actions_edit_delete_icon;
            this.pbxDelete.Location = new System.Drawing.Point(538, 98);
            this.pbxDelete.Name = "pbxDelete";
            this.pbxDelete.Size = new System.Drawing.Size(90, 37);
            this.pbxDelete.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxDelete.TabIndex = 15;
            this.pbxDelete.TabStop = false;
            this.pbxDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // pbxEdit
            // 
            this.pbxEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbxEdit.Image = global::POSRestaurant.Properties.Resources.Edit_icon;
            this.pbxEdit.Location = new System.Drawing.Point(538, 58);
            this.pbxEdit.Name = "pbxEdit";
            this.pbxEdit.Size = new System.Drawing.Size(90, 37);
            this.pbxEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbxEdit.TabIndex = 14;
            this.pbxEdit.TabStop = false;
            this.pbxEdit.Click += new System.EventHandler(this.pbxEdit_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cmdAdd.Image = global::POSRestaurant.Properties.Resources.add_1_icon;
            this.cmdAdd.Location = new System.Drawing.Point(538, 18);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(90, 37);
            this.cmdAdd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.cmdAdd.TabIndex = 13;
            this.cmdAdd.TabStop = false;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // frmItemrecipemang
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 219);
            this.Controls.Add(this.pbxClose);
            this.Controls.Add(this.cmdRefresh);
            this.Controls.Add(this.pbxDelete);
            this.Controls.Add(this.pbxEdit);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.lvList);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmItemrecipemang";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recipe Items";
            this.Activated += new System.EventHandler(this.frmItemCategory_Activated);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmRentalCategory_FormClosed);
            this.Load += new System.EventHandler(this.frmRentalCategory_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbxClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRefresh)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxDelete)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbxEdit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdAdd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader ItemCategory_Name;
        private System.Windows.Forms.PictureBox cmdAdd;
        private System.Windows.Forms.PictureBox pbxEdit;
        private System.Windows.Forms.PictureBox pbxDelete;
        private System.Windows.Forms.PictureBox cmdRefresh;
        private System.Windows.Forms.PictureBox pbxClose;
        public System.Windows.Forms.ListView lvList;
        private classes.Clsdbcon objCore;
        private bool activated;
        private System.Windows.Forms.ColumnHeader columnHeader2;
    }
}