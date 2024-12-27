namespace POSRetail.ControlPanel
{
    partial class frmAddEditUser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddEditUser));
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Label48 = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.plRights = new System.Windows.Forms.Panel();
            this.dgRights = new System.Windows.Forms.DataGridView();
            this.FormId = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.IdRights = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CanView = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CanAdd = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CanUpdate = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CanDelete = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.dsRights1 = new POSRetail.ControlPanel.dsRights();
            this.label4 = new System.Windows.Forms.Label();
            this.rboInactive = new System.Windows.Forms.RadioButton();
            this.rboActive = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.panel13 = new System.Windows.Forms.Panel();
            this.sqlSelectCommand1 = new System.Data.SqlClient.SqlCommand();
            this.sqlConnection1 = new System.Data.SqlClient.SqlConnection();
            this.sqlInsertCommand1 = new System.Data.SqlClient.SqlCommand();
            this.daRights = new System.Data.SqlClient.SqlDataAdapter();
            this.sqlDeleteCommand = new System.Data.SqlClient.SqlCommand();
            this.sqlUpdateCommand = new System.Data.SqlClient.SqlCommand();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.plRights.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgRights)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRights1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.cmdSave.Location = new System.Drawing.Point(604, 7);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 25);
            this.cmdSave.TabIndex = 0;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.cmdCancel.Location = new System.Drawing.Point(685, 8);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 25);
            this.cmdCancel.TabIndex = 1;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.SteelBlue;
            this.panel3.Controls.Add(this.Label48);
            this.panel3.Controls.Add(this.cmdCancel);
            this.panel3.Controls.Add(this.cmdSave);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 517);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(784, 45);
            this.panel3.TabIndex = 46;
            // 
            // Label48
            // 
            this.Label48.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.Label48.AutoSize = true;
            this.Label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label48.Location = new System.Drawing.Point(17, 18);
            this.Label48.Name = "Label48";
            this.Label48.Size = new System.Drawing.Size(75, 13);
            this.Label48.TabIndex = 67;
            this.Label48.Text = "Please specify";
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Franklin Gothic Medium", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(784, 53);
            this.lblHeader.TabIndex = 47;
            this.lblHeader.Text = "Add/Edit User";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.plRights);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.rboInactive);
            this.panel2.Controls.Add(this.rboActive);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtCPassword);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtPassword);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtUserName);
            this.panel2.Controls.Add(this.label17);
            this.panel2.Controls.Add(this.txtUserId);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(784, 427);
            this.panel2.TabIndex = 49;
            // 
            // plRights
            // 
            this.plRights.Controls.Add(this.dgRights);
            this.plRights.Dock = System.Windows.Forms.DockStyle.Right;
            this.plRights.Location = new System.Drawing.Point(425, 0);
            this.plRights.Name = "plRights";
            this.plRights.Size = new System.Drawing.Size(359, 427);
            this.plRights.TabIndex = 140;
            // 
            // dgRights
            // 
            this.dgRights.AutoGenerateColumns = false;
            this.dgRights.BackgroundColor = System.Drawing.Color.White;
            this.dgRights.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgRights.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgRights.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FormId,
            this.IdRights,
            this.CanView,
            this.CanAdd,
            this.CanUpdate,
            this.CanDelete});
            this.dgRights.DataSource = this.bindingSource1;
            this.dgRights.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgRights.GridColor = System.Drawing.Color.White;
            this.dgRights.Location = new System.Drawing.Point(0, 0);
            this.dgRights.Name = "dgRights";
            this.dgRights.RowHeadersVisible = false;
            this.dgRights.Size = new System.Drawing.Size(359, 427);
            this.dgRights.TabIndex = 0;
            // 
            // FormId
            // 
            this.FormId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.FormId.DataPropertyName = "FormId";
            this.FormId.HeaderText = "Form";
            this.FormId.Name = "FormId";
            this.FormId.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.FormId.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // IdRights
            // 
            this.IdRights.DataPropertyName = "Id";
            this.IdRights.HeaderText = "Id";
            this.IdRights.Name = "IdRights";
            this.IdRights.ReadOnly = true;
            this.IdRights.Visible = false;
            // 
            // CanView
            // 
            this.CanView.DataPropertyName = "CanView";
            this.CanView.HeaderText = "View";
            this.CanView.Name = "CanView";
            this.CanView.Width = 45;
            // 
            // CanAdd
            // 
            this.CanAdd.DataPropertyName = "CanAdd";
            this.CanAdd.HeaderText = "Add";
            this.CanAdd.Name = "CanAdd";
            this.CanAdd.Width = 45;
            // 
            // CanUpdate
            // 
            this.CanUpdate.DataPropertyName = "CanUpdate";
            this.CanUpdate.HeaderText = "Update";
            this.CanUpdate.Name = "CanUpdate";
            this.CanUpdate.Width = 50;
            // 
            // CanDelete
            // 
            this.CanDelete.DataPropertyName = "CanDelete";
            this.CanDelete.HeaderText = "Delete";
            this.CanDelete.Name = "CanDelete";
            this.CanDelete.Width = 50;
            // 
            // bindingSource1
            // 
            this.bindingSource1.DataMember = "UserRights";
            this.bindingSource1.DataSource = this.dsRights1;
            // 
            // dsRights1
            // 
            this.dsRights1.DataSetName = "dsRights";
            this.dsRights1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(35, 262);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 17);
            this.label4.TabIndex = 139;
            this.label4.Text = "Status";
            // 
            // rboInactive
            // 
            this.rboInactive.AutoSize = true;
            this.rboInactive.Location = new System.Drawing.Point(180, 285);
            this.rboInactive.Name = "rboInactive";
            this.rboInactive.Size = new System.Drawing.Size(66, 19);
            this.rboInactive.TabIndex = 6;
            this.rboInactive.Text = "Inactive";
            this.rboInactive.UseVisualStyleBackColor = true;
            // 
            // rboActive
            // 
            this.rboActive.AutoSize = true;
            this.rboActive.Checked = true;
            this.rboActive.Location = new System.Drawing.Point(180, 260);
            this.rboActive.Name = "rboActive";
            this.rboActive.Size = new System.Drawing.Size(56, 19);
            this.rboActive.TabIndex = 5;
            this.rboActive.TabStop = true;
            this.rboActive.Text = "Active";
            this.rboActive.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(35, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 17);
            this.label3.TabIndex = 136;
            this.label3.Text = "Confirm Password";
            // 
            // txtCPassword
            // 
            this.txtCPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtCPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtCPassword.Location = new System.Drawing.Point(180, 201);
            this.txtCPassword.Name = "txtCPassword";
            this.txtCPassword.PasswordChar = '*';
            this.txtCPassword.Size = new System.Drawing.Size(215, 26);
            this.txtCPassword.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(35, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 17);
            this.label2.TabIndex = 134;
            this.label2.Text = "Password";
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtPassword.Location = new System.Drawing.Point(180, 146);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(215, 26);
            this.txtPassword.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(35, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 17);
            this.label1.TabIndex = 132;
            this.label1.Text = "Name";
            // 
            // txtUserName
            // 
            this.txtUserName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtUserName.Location = new System.Drawing.Point(180, 95);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(215, 26);
            this.txtUserName.TabIndex = 2;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.label17.Location = new System.Drawing.Point(35, 52);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(55, 17);
            this.label17.TabIndex = 130;
            this.label17.Text = "UserId";
            // 
            // txtUserId
            // 
            this.txtUserId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.txtUserId.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.txtUserId.Location = new System.Drawing.Point(180, 47);
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(215, 26);
            this.txtUserId.TabIndex = 1;
            // 
            // panel13
            // 
            this.panel13.BackgroundImage = global::POSRetail.Properties.Resources.top3;
            this.panel13.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel13.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel13.Location = new System.Drawing.Point(0, 53);
            this.panel13.Name = "panel13";
            this.panel13.Size = new System.Drawing.Size(784, 37);
            this.panel13.TabIndex = 48;
            // 
            // sqlSelectCommand1
            // 
            this.sqlSelectCommand1.CommandText = "SELECT        Id, FormId, CanAdd, CanDelete, CanUpdate, CanView\r\nFROM            " +
                "UserRights\r\nWHERE        (Id = @userId)";
            this.sqlSelectCommand1.Connection = this.sqlConnection1;
            this.sqlSelectCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@userId", System.Data.SqlDbType.Decimal, 9, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "Id", System.Data.DataRowVersion.Current, null)});
            // 
            // sqlConnection1
            // 
            this.sqlConnection1.ConnectionString = "Data Source=SUDOZAI-COMP\\sqlexpress;Initial Catalog=MBLdb;Integrated Security=Tru" +
                "e";
            this.sqlConnection1.FireInfoMessageEventOnUserErrors = false;
            // 
            // sqlInsertCommand1
            // 
            this.sqlInsertCommand1.CommandText = resources.GetString("sqlInsertCommand1.CommandText");
            this.sqlInsertCommand1.Connection = this.sqlConnection1;
            this.sqlInsertCommand1.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "Id", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@FormId", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "FormId", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@CanAdd", System.Data.SqlDbType.Bit, 0, "CanAdd"),
            new System.Data.SqlClient.SqlParameter("@CanDelete", System.Data.SqlDbType.Bit, 0, "CanDelete"),
            new System.Data.SqlClient.SqlParameter("@CanUpdate", System.Data.SqlDbType.Bit, 0, "CanUpdate"),
            new System.Data.SqlClient.SqlParameter("@CanView", System.Data.SqlDbType.Bit, 0, "CanView")});
            // 
            // daRights
            // 
            this.daRights.DeleteCommand = this.sqlDeleteCommand;
            this.daRights.InsertCommand = this.sqlInsertCommand1;
            this.daRights.SelectCommand = this.sqlSelectCommand1;
            this.daRights.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
            new System.Data.Common.DataTableMapping("Table", "UserRights", new System.Data.Common.DataColumnMapping[] {
                        new System.Data.Common.DataColumnMapping("Id", "Id"),
                        new System.Data.Common.DataColumnMapping("FormId", "FormId"),
                        new System.Data.Common.DataColumnMapping("CanAdd", "CanAdd"),
                        new System.Data.Common.DataColumnMapping("CanDelete", "CanDelete"),
                        new System.Data.Common.DataColumnMapping("CanUpdate", "CanUpdate"),
                        new System.Data.Common.DataColumnMapping("CanView", "CanView")})});
            this.daRights.UpdateCommand = this.sqlUpdateCommand;
            // 
            // sqlDeleteCommand
            // 
            this.sqlDeleteCommand.CommandText = resources.GetString("sqlDeleteCommand.CommandText");
            this.sqlDeleteCommand.Connection = this.sqlConnection1;
            this.sqlDeleteCommand.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Original_Id", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_FormId", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "FormId", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanAdd", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanAdd", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanAdd", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanAdd", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanDelete", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanDelete", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanDelete", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanDelete", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanUpdate", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanUpdate", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanUpdate", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanUpdate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanView", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanView", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanView", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanView", System.Data.DataRowVersion.Original, null)});
            // 
            // sqlUpdateCommand
            // 
            this.sqlUpdateCommand.CommandText = resources.GetString("sqlUpdateCommand.CommandText");
            this.sqlUpdateCommand.Connection = this.sqlConnection1;
            this.sqlUpdateCommand.Parameters.AddRange(new System.Data.SqlClient.SqlParameter[] {
            new System.Data.SqlClient.SqlParameter("@Id", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "Id", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@FormId", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "FormId", System.Data.DataRowVersion.Current, null),
            new System.Data.SqlClient.SqlParameter("@CanAdd", System.Data.SqlDbType.Bit, 0, "CanAdd"),
            new System.Data.SqlClient.SqlParameter("@CanDelete", System.Data.SqlDbType.Bit, 0, "CanDelete"),
            new System.Data.SqlClient.SqlParameter("@CanUpdate", System.Data.SqlDbType.Bit, 0, "CanUpdate"),
            new System.Data.SqlClient.SqlParameter("@CanView", System.Data.SqlDbType.Bit, 0, "CanView"),
            new System.Data.SqlClient.SqlParameter("@Original_Id", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "Id", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@Original_FormId", System.Data.SqlDbType.Decimal, 0, System.Data.ParameterDirection.Input, false, ((byte)(18)), ((byte)(0)), "FormId", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanAdd", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanAdd", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanAdd", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanAdd", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanDelete", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanDelete", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanDelete", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanDelete", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanUpdate", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanUpdate", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanUpdate", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanUpdate", System.Data.DataRowVersion.Original, null),
            new System.Data.SqlClient.SqlParameter("@IsNull_CanView", System.Data.SqlDbType.Int, 0, System.Data.ParameterDirection.Input, ((byte)(0)), ((byte)(0)), "CanView", System.Data.DataRowVersion.Original, true, null, "", "", ""),
            new System.Data.SqlClient.SqlParameter("@Original_CanView", System.Data.SqlDbType.Bit, 0, System.Data.ParameterDirection.Input, false, ((byte)(0)), ((byte)(0)), "CanView", System.Data.DataRowVersion.Original, null)});
            // 
            // frmAddEditUser
            // 
            this.AcceptButton = this.cmdSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel13);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmAddEditUser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add/Edit User";
            this.Load += new System.EventHandler(this.frmAddMovie_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.plRights.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgRights)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRights1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Panel panel3;
        internal System.Windows.Forms.Label Label48;
        public System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel panel13;
        private System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Button cmdSave;
        private POSRetail.classes.Clsdbcon objCore;
        public int userId;
        public int editMode;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rboInactive;
        private System.Windows.Forms.RadioButton rboActive;
        private System.Windows.Forms.Panel plRights;
        private System.Windows.Forms.DataGridView dgRights;
        private System.Data.SqlClient.SqlCommand sqlSelectCommand1;
        private System.Data.SqlClient.SqlConnection sqlConnection1;
        private System.Data.SqlClient.SqlCommand sqlInsertCommand1;
        private System.Data.SqlClient.SqlDataAdapter daRights;
        private dsRights dsRights1;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Data.SqlClient.SqlCommand sqlDeleteCommand;
        private System.Data.SqlClient.SqlCommand sqlUpdateCommand;
        private System.Windows.Forms.DataGridViewComboBoxColumn FormId;
        private System.Windows.Forms.DataGridViewTextBoxColumn IdRights;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanAdd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanUpdate;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanDelete;
    }
}