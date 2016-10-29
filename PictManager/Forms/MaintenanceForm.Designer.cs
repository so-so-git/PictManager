namespace SO.PictManager.Forms
{
    partial class MaintenanceForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaintenanceForm));
            this.grpImportExport = new System.Windows.Forms.GroupBox();
            this.grpImportExportType = new System.Windows.Forms.GroupBox();
            this.rdoTypeImport = new System.Windows.Forms.RadioButton();
            this.rdoTypeExport = new System.Windows.Forms.RadioButton();
            this.grpImportTarget = new System.Windows.Forms.GroupBox();
            this.rdoImportDirectory = new System.Windows.Forms.RadioButton();
            this.rdoImportFile = new System.Windows.Forms.RadioButton();
            this.btnViewDeletedFiles = new System.Windows.Forms.Button();
            this.btnImportExport = new System.Windows.Forms.Button();
            this.chkIncludeSubDirectory = new System.Windows.Forms.CheckBox();
            this.btnRef = new System.Windows.Forms.Button();
            this.txtTargetPath = new System.Windows.Forms.TextBox();
            this.cmbImportCategory = new System.Windows.Forms.ComboBox();
            this.lblTargetDirectory = new System.Windows.Forms.Label();
            this.lblCategories = new System.Windows.Forms.Label();
            this.dlgImportFile = new System.Windows.Forms.OpenFileDialog();
            this.dlgImportDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.grpCategories = new System.Windows.Forms.GroupBox();
            this.lblUpdateCategory = new System.Windows.Forms.Label();
            this.txtUpdateCategory = new System.Windows.Forms.TextBox();
            this.txtEntryCategory = new System.Windows.Forms.TextBox();
            this.btnUpdateCategory = new System.Windows.Forms.Button();
            this.btnDeleteCategory = new System.Windows.Forms.Button();
            this.btnEntryCategory = new System.Windows.Forms.Button();
            this.cmbEditCategory = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpCleanup = new System.Windows.Forms.GroupBox();
            this.lblUnusedTags = new System.Windows.Forms.Label();
            this.lblLogicalDeletedImages = new System.Windows.Forms.Label();
            this.btnDeleteUnusedTags = new System.Windows.Forms.Button();
            this.btnPhysicalDeleteImages = new System.Windows.Forms.Button();
            this.lblUnusedTagsHeader = new System.Windows.Forms.Label();
            this.lblLogicalDeletedImagesHeader = new System.Windows.Forms.Label();
            this.barStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCheckDuplicate = new System.Windows.Forms.Button();
            this.grpImportExport.SuspendLayout();
            this.grpImportExportType.SuspendLayout();
            this.grpImportTarget.SuspendLayout();
            this.grpCategories.SuspendLayout();
            this.grpCleanup.SuspendLayout();
            this.barStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpImportExport
            // 
            this.grpImportExport.Controls.Add(this.grpImportExportType);
            this.grpImportExport.Controls.Add(this.grpImportTarget);
            this.grpImportExport.Controls.Add(this.btnViewDeletedFiles);
            this.grpImportExport.Controls.Add(this.btnImportExport);
            this.grpImportExport.Controls.Add(this.chkIncludeSubDirectory);
            this.grpImportExport.Controls.Add(this.btnRef);
            this.grpImportExport.Controls.Add(this.txtTargetPath);
            this.grpImportExport.Controls.Add(this.cmbImportCategory);
            this.grpImportExport.Controls.Add(this.lblTargetDirectory);
            this.grpImportExport.Controls.Add(this.lblCategories);
            this.grpImportExport.Location = new System.Drawing.Point(13, 139);
            this.grpImportExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpImportExport.Name = "grpImportExport";
            this.grpImportExport.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpImportExport.Size = new System.Drawing.Size(449, 209);
            this.grpImportExport.TabIndex = 1;
            this.grpImportExport.TabStop = false;
            this.grpImportExport.Text = "画像ファイルインポート・エクスポート";
            // 
            // grpImportExportType
            // 
            this.grpImportExportType.Controls.Add(this.rdoTypeImport);
            this.grpImportExportType.Controls.Add(this.rdoTypeExport);
            this.grpImportExportType.Location = new System.Drawing.Point(10, 23);
            this.grpImportExportType.Name = "grpImportExportType";
            this.grpImportExportType.Size = new System.Drawing.Size(175, 50);
            this.grpImportExportType.TabIndex = 0;
            this.grpImportExportType.TabStop = false;
            this.grpImportExportType.Text = "実行処理";
            // 
            // rdoTypeImport
            // 
            this.rdoTypeImport.AutoSize = true;
            this.rdoTypeImport.Checked = true;
            this.rdoTypeImport.Location = new System.Drawing.Point(10, 20);
            this.rdoTypeImport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoTypeImport.Name = "rdoTypeImport";
            this.rdoTypeImport.Size = new System.Drawing.Size(71, 19);
            this.rdoTypeImport.TabIndex = 0;
            this.rdoTypeImport.TabStop = true;
            this.rdoTypeImport.Text = "インポート";
            this.rdoTypeImport.UseVisualStyleBackColor = true;
            this.rdoTypeImport.CheckedChanged += new System.EventHandler(this.rdoImportExportType_CheckedChanged);
            // 
            // rdoTypeExport
            // 
            this.rdoTypeExport.AutoSize = true;
            this.rdoTypeExport.Location = new System.Drawing.Point(87, 20);
            this.rdoTypeExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoTypeExport.Name = "rdoTypeExport";
            this.rdoTypeExport.Size = new System.Drawing.Size(79, 19);
            this.rdoTypeExport.TabIndex = 1;
            this.rdoTypeExport.Text = "エクスポート";
            this.rdoTypeExport.UseVisualStyleBackColor = true;
            this.rdoTypeExport.CheckedChanged += new System.EventHandler(this.rdoImportExportType_CheckedChanged);
            // 
            // grpImportTarget
            // 
            this.grpImportTarget.Controls.Add(this.rdoImportDirectory);
            this.grpImportTarget.Controls.Add(this.rdoImportFile);
            this.grpImportTarget.Location = new System.Drawing.Point(191, 23);
            this.grpImportTarget.Name = "grpImportTarget";
            this.grpImportTarget.Size = new System.Drawing.Size(146, 50);
            this.grpImportTarget.TabIndex = 1;
            this.grpImportTarget.TabStop = false;
            this.grpImportTarget.Text = "インポート対象";
            // 
            // rdoImportDirectory
            // 
            this.rdoImportDirectory.AutoSize = true;
            this.rdoImportDirectory.Checked = true;
            this.rdoImportDirectory.Location = new System.Drawing.Point(10, 20);
            this.rdoImportDirectory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoImportDirectory.Name = "rdoImportDirectory";
            this.rdoImportDirectory.Size = new System.Drawing.Size(60, 19);
            this.rdoImportDirectory.TabIndex = 0;
            this.rdoImportDirectory.TabStop = true;
            this.rdoImportDirectory.Text = "フォルダ";
            this.rdoImportDirectory.UseVisualStyleBackColor = true;
            this.rdoImportDirectory.CheckedChanged += new System.EventHandler(this.rdoImportTargets_CheckedChanged);
            // 
            // rdoImportFile
            // 
            this.rdoImportFile.AutoSize = true;
            this.rdoImportFile.Location = new System.Drawing.Point(76, 20);
            this.rdoImportFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoImportFile.Name = "rdoImportFile";
            this.rdoImportFile.Size = new System.Drawing.Size(59, 19);
            this.rdoImportFile.TabIndex = 1;
            this.rdoImportFile.Text = "ファイル";
            this.rdoImportFile.UseVisualStyleBackColor = true;
            this.rdoImportFile.CheckedChanged += new System.EventHandler(this.rdoImportTargets_CheckedChanged);
            // 
            // btnViewDeletedFiles
            // 
            this.btnViewDeletedFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnViewDeletedFiles.Location = new System.Drawing.Point(342, 172);
            this.btnViewDeletedFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnViewDeletedFiles.Name = "btnViewDeletedFiles";
            this.btnViewDeletedFiles.Size = new System.Drawing.Size(100, 29);
            this.btnViewDeletedFiles.TabIndex = 9;
            this.btnViewDeletedFiles.Text = "削除ファイル確認";
            this.btnViewDeletedFiles.UseVisualStyleBackColor = true;
            this.btnViewDeletedFiles.Click += new System.EventHandler(this.btnViewDeletedFiles_Click);
            // 
            // btnImportExport
            // 
            this.btnImportExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImportExport.Location = new System.Drawing.Point(249, 172);
            this.btnImportExport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImportExport.Name = "btnImportExport";
            this.btnImportExport.Size = new System.Drawing.Size(87, 29);
            this.btnImportExport.TabIndex = 8;
            this.btnImportExport.Text = "実行";
            this.btnImportExport.UseVisualStyleBackColor = true;
            this.btnImportExport.Click += new System.EventHandler(this.btnImportExport_Click);
            // 
            // chkIncludeSubDirectory
            // 
            this.chkIncludeSubDirectory.AutoSize = true;
            this.chkIncludeSubDirectory.Location = new System.Drawing.Point(64, 111);
            this.chkIncludeSubDirectory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkIncludeSubDirectory.Name = "chkIncludeSubDirectory";
            this.chkIncludeSubDirectory.Size = new System.Drawing.Size(111, 19);
            this.chkIncludeSubDirectory.TabIndex = 5;
            this.chkIncludeSubDirectory.Text = "サブフォルダも含む";
            this.chkIncludeSubDirectory.UseVisualStyleBackColor = true;
            // 
            // btnRef
            // 
            this.btnRef.Location = new System.Drawing.Point(413, 80);
            this.btnRef.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRef.Name = "btnRef";
            this.btnRef.Size = new System.Drawing.Size(29, 23);
            this.btnRef.TabIndex = 4;
            this.btnRef.Text = "...";
            this.btnRef.UseVisualStyleBackColor = true;
            this.btnRef.Click += new System.EventHandler(this.btnRef_Click);
            // 
            // txtTargetPath
            // 
            this.txtTargetPath.AllowDrop = true;
            this.txtTargetPath.Location = new System.Drawing.Point(64, 80);
            this.txtTargetPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.Size = new System.Drawing.Size(343, 23);
            this.txtTargetPath.TabIndex = 3;
            // 
            // cmbImportCategory
            // 
            this.cmbImportCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbImportCategory.FormattingEnabled = true;
            this.cmbImportCategory.Location = new System.Drawing.Point(64, 138);
            this.cmbImportCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbImportCategory.Name = "cmbImportCategory";
            this.cmbImportCategory.Size = new System.Drawing.Size(378, 23);
            this.cmbImportCategory.TabIndex = 7;
            // 
            // lblTargetDirectory
            // 
            this.lblTargetDirectory.AutoSize = true;
            this.lblTargetDirectory.Location = new System.Drawing.Point(7, 85);
            this.lblTargetDirectory.Name = "lblTargetDirectory";
            this.lblTargetDirectory.Size = new System.Drawing.Size(31, 15);
            this.lblTargetDirectory.TabIndex = 2;
            this.lblTargetDirectory.Text = "対象";
            // 
            // lblCategories
            // 
            this.lblCategories.AutoSize = true;
            this.lblCategories.Location = new System.Drawing.Point(7, 142);
            this.lblCategories.Name = "lblCategories";
            this.lblCategories.Size = new System.Drawing.Size(51, 15);
            this.lblCategories.TabIndex = 6;
            this.lblCategories.Text = "カテゴリー";
            // 
            // dlgImportFile
            // 
            this.dlgImportFile.AddExtension = false;
            this.dlgImportFile.SupportMultiDottedExtensions = true;
            // 
            // dlgImportDirectory
            // 
            this.dlgImportDirectory.Description = "インポート対象ディレクトリを選択して下さい。";
            this.dlgImportDirectory.ShowNewFolderButton = false;
            // 
            // grpCategories
            // 
            this.grpCategories.Controls.Add(this.lblUpdateCategory);
            this.grpCategories.Controls.Add(this.txtUpdateCategory);
            this.grpCategories.Controls.Add(this.txtEntryCategory);
            this.grpCategories.Controls.Add(this.btnUpdateCategory);
            this.grpCategories.Controls.Add(this.btnDeleteCategory);
            this.grpCategories.Controls.Add(this.btnEntryCategory);
            this.grpCategories.Controls.Add(this.cmbEditCategory);
            this.grpCategories.Location = new System.Drawing.Point(13, 13);
            this.grpCategories.Name = "grpCategories";
            this.grpCategories.Size = new System.Drawing.Size(307, 119);
            this.grpCategories.TabIndex = 0;
            this.grpCategories.TabStop = false;
            this.grpCategories.Text = "カテゴリー管理";
            // 
            // lblUpdateCategory
            // 
            this.lblUpdateCategory.Font = new System.Drawing.Font("Meiryo UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblUpdateCategory.Location = new System.Drawing.Point(6, 81);
            this.lblUpdateCategory.Name = "lblUpdateCategory";
            this.lblUpdateCategory.Size = new System.Drawing.Size(18, 27);
            this.lblUpdateCategory.TabIndex = 4;
            this.lblUpdateCategory.Text = "└";
            this.lblUpdateCategory.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtUpdateCategory
            // 
            this.txtUpdateCategory.AllowDrop = true;
            this.txtUpdateCategory.Location = new System.Drawing.Point(30, 85);
            this.txtUpdateCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtUpdateCategory.Name = "txtUpdateCategory";
            this.txtUpdateCategory.Size = new System.Drawing.Size(209, 23);
            this.txtUpdateCategory.TabIndex = 5;
            // 
            // txtEntryCategory
            // 
            this.txtEntryCategory.AllowDrop = true;
            this.txtEntryCategory.Location = new System.Drawing.Point(9, 23);
            this.txtEntryCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEntryCategory.Name = "txtEntryCategory";
            this.txtEntryCategory.Size = new System.Drawing.Size(230, 23);
            this.txtEntryCategory.TabIndex = 0;
            this.txtEntryCategory.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtEntryCategory_KeyDown);
            // 
            // btnUpdateCategory
            // 
            this.btnUpdateCategory.Location = new System.Drawing.Point(245, 85);
            this.btnUpdateCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdateCategory.Name = "btnUpdateCategory";
            this.btnUpdateCategory.Size = new System.Drawing.Size(50, 23);
            this.btnUpdateCategory.TabIndex = 6;
            this.btnUpdateCategory.Text = "更新";
            this.btnUpdateCategory.UseVisualStyleBackColor = true;
            this.btnUpdateCategory.Click += new System.EventHandler(this.btnUpdateCategory_Click);
            // 
            // btnDeleteCategory
            // 
            this.btnDeleteCategory.Location = new System.Drawing.Point(245, 54);
            this.btnDeleteCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteCategory.Name = "btnDeleteCategory";
            this.btnDeleteCategory.Size = new System.Drawing.Size(50, 23);
            this.btnDeleteCategory.TabIndex = 3;
            this.btnDeleteCategory.Text = "削除";
            this.btnDeleteCategory.UseVisualStyleBackColor = true;
            this.btnDeleteCategory.Click += new System.EventHandler(this.btnDeleteCategory_Click);
            // 
            // btnEntryCategory
            // 
            this.btnEntryCategory.Location = new System.Drawing.Point(245, 23);
            this.btnEntryCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnEntryCategory.Name = "btnEntryCategory";
            this.btnEntryCategory.Size = new System.Drawing.Size(50, 23);
            this.btnEntryCategory.TabIndex = 1;
            this.btnEntryCategory.Text = "登録";
            this.btnEntryCategory.UseVisualStyleBackColor = true;
            this.btnEntryCategory.Click += new System.EventHandler(this.btnEntryCategory_Click);
            // 
            // cmbEditCategory
            // 
            this.cmbEditCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEditCategory.FormattingEnabled = true;
            this.cmbEditCategory.Location = new System.Drawing.Point(9, 54);
            this.cmbEditCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbEditCategory.Name = "cmbEditCategory";
            this.cmbEditCategory.Size = new System.Drawing.Size(230, 23);
            this.cmbEditCategory.TabIndex = 2;
            this.cmbEditCategory.SelectedIndexChanged += new System.EventHandler(this.cmbEditCategory_SelectedIndexChanged);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(375, 459);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 29);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpCleanup
            // 
            this.grpCleanup.Controls.Add(this.lblUnusedTags);
            this.grpCleanup.Controls.Add(this.lblLogicalDeletedImages);
            this.grpCleanup.Controls.Add(this.btnDeleteUnusedTags);
            this.grpCleanup.Controls.Add(this.btnPhysicalDeleteImages);
            this.grpCleanup.Controls.Add(this.lblUnusedTagsHeader);
            this.grpCleanup.Controls.Add(this.lblLogicalDeletedImagesHeader);
            this.grpCleanup.Location = new System.Drawing.Point(13, 355);
            this.grpCleanup.Name = "grpCleanup";
            this.grpCleanup.Size = new System.Drawing.Size(307, 90);
            this.grpCleanup.TabIndex = 2;
            this.grpCleanup.TabStop = false;
            this.grpCleanup.Text = "クリーンアップ";
            // 
            // lblUnusedTags
            // 
            this.lblUnusedTags.AutoSize = true;
            this.lblUnusedTags.Location = new System.Drawing.Point(95, 56);
            this.lblUnusedTags.Name = "lblUnusedTags";
            this.lblUnusedTags.Size = new System.Drawing.Size(53, 15);
            this.lblUnusedTags.TabIndex = 4;
            this.lblUnusedTags.Text = "999,999";
            // 
            // lblLogicalDeletedImages
            // 
            this.lblLogicalDeletedImages.AutoSize = true;
            this.lblLogicalDeletedImages.Location = new System.Drawing.Point(95, 23);
            this.lblLogicalDeletedImages.Name = "lblLogicalDeletedImages";
            this.lblLogicalDeletedImages.Size = new System.Drawing.Size(53, 15);
            this.lblLogicalDeletedImages.TabIndex = 1;
            this.lblLogicalDeletedImages.Text = "999,999";
            // 
            // btnDeleteUnusedTags
            // 
            this.btnDeleteUnusedTags.Location = new System.Drawing.Point(215, 51);
            this.btnDeleteUnusedTags.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteUnusedTags.Name = "btnDeleteUnusedTags";
            this.btnDeleteUnusedTags.Size = new System.Drawing.Size(80, 25);
            this.btnDeleteUnusedTags.TabIndex = 5;
            this.btnDeleteUnusedTags.Text = "削除";
            this.btnDeleteUnusedTags.UseVisualStyleBackColor = true;
            this.btnDeleteUnusedTags.Click += new System.EventHandler(this.btnDeleteUnusedTags_Click);
            // 
            // btnPhysicalDeleteImages
            // 
            this.btnPhysicalDeleteImages.Location = new System.Drawing.Point(215, 18);
            this.btnPhysicalDeleteImages.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPhysicalDeleteImages.Name = "btnPhysicalDeleteImages";
            this.btnPhysicalDeleteImages.Size = new System.Drawing.Size(80, 25);
            this.btnPhysicalDeleteImages.TabIndex = 2;
            this.btnPhysicalDeleteImages.Text = "完全に削除";
            this.btnPhysicalDeleteImages.UseVisualStyleBackColor = true;
            this.btnPhysicalDeleteImages.Click += new System.EventHandler(this.btnPhysicalDeleteImages_Click);
            // 
            // lblUnusedTagsHeader
            // 
            this.lblUnusedTagsHeader.AutoSize = true;
            this.lblUnusedTagsHeader.Location = new System.Drawing.Point(7, 56);
            this.lblUnusedTagsHeader.Name = "lblUnusedTagsHeader";
            this.lblUnusedTagsHeader.Size = new System.Drawing.Size(82, 15);
            this.lblUnusedTagsHeader.TabIndex = 3;
            this.lblUnusedTagsHeader.Text = "不要なタグ数：";
            // 
            // lblLogicalDeletedImagesHeader
            // 
            this.lblLogicalDeletedImagesHeader.AutoSize = true;
            this.lblLogicalDeletedImagesHeader.Location = new System.Drawing.Point(7, 23);
            this.lblLogicalDeletedImagesHeader.Name = "lblLogicalDeletedImagesHeader";
            this.lblLogicalDeletedImagesHeader.Size = new System.Drawing.Size(79, 15);
            this.lblLogicalDeletedImagesHeader.TabIndex = 0;
            this.lblLogicalDeletedImagesHeader.Text = "削除画像数：";
            // 
            // barStatus
            // 
            this.barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.barStatus.Location = new System.Drawing.Point(0, 492);
            this.barStatus.Name = "barStatus";
            this.barStatus.Size = new System.Drawing.Size(471, 22);
            this.barStatus.TabIndex = 5;
            this.barStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(76, 17);
            this.lblStatus.Text = "ステータス表示";
            // 
            // btnCheckDuplicate
            // 
            this.btnCheckDuplicate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheckDuplicate.Location = new System.Drawing.Point(13, 459);
            this.btnCheckDuplicate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckDuplicate.Name = "btnCheckDuplicate";
            this.btnCheckDuplicate.Size = new System.Drawing.Size(109, 29);
            this.btnCheckDuplicate.TabIndex = 3;
            this.btnCheckDuplicate.Text = "重複画像を確認";
            this.btnCheckDuplicate.UseVisualStyleBackColor = true;
            this.btnCheckDuplicate.Click += new System.EventHandler(this.btnCheckDuplicate_Click);
            // 
            // MaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 514);
            this.Controls.Add(this.barStatus);
            this.Controls.Add(this.grpCleanup);
            this.Controls.Add(this.grpCategories);
            this.Controls.Add(this.btnCheckDuplicate);
            this.Controls.Add(this.grpImportExport);
            this.Controls.Add(this.btnClose);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MaintenanceForm";
            this.Text = "PictManager - データベースメンテナンス";
            this.grpImportExport.ResumeLayout(false);
            this.grpImportExport.PerformLayout();
            this.grpImportExportType.ResumeLayout(false);
            this.grpImportExportType.PerformLayout();
            this.grpImportTarget.ResumeLayout(false);
            this.grpImportTarget.PerformLayout();
            this.grpCategories.ResumeLayout(false);
            this.grpCategories.PerformLayout();
            this.grpCleanup.ResumeLayout(false);
            this.grpCleanup.PerformLayout();
            this.barStatus.ResumeLayout(false);
            this.barStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpImportExport;
        private System.Windows.Forms.ComboBox cmbImportCategory;
        private System.Windows.Forms.Label lblCategories;
        private System.Windows.Forms.CheckBox chkIncludeSubDirectory;
        private System.Windows.Forms.Button btnRef;
        private System.Windows.Forms.TextBox txtTargetPath;
        private System.Windows.Forms.Label lblTargetDirectory;
        private System.Windows.Forms.Button btnImportExport;
        private System.Windows.Forms.RadioButton rdoImportDirectory;
        private System.Windows.Forms.RadioButton rdoImportFile;
        private System.Windows.Forms.OpenFileDialog dlgImportFile;
        private System.Windows.Forms.FolderBrowserDialog dlgImportDirectory;
        private System.Windows.Forms.GroupBox grpCategories;
        private System.Windows.Forms.TextBox txtEntryCategory;
        private System.Windows.Forms.Button btnEntryCategory;
        private System.Windows.Forms.Button btnDeleteCategory;
        private System.Windows.Forms.ComboBox cmbEditCategory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox grpCleanup;
        private System.Windows.Forms.Label lblLogicalDeletedImages;
        private System.Windows.Forms.Button btnPhysicalDeleteImages;
        private System.Windows.Forms.Label lblLogicalDeletedImagesHeader;
        private System.Windows.Forms.StatusStrip barStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnViewDeletedFiles;
        private System.Windows.Forms.GroupBox grpImportExportType;
        private System.Windows.Forms.RadioButton rdoTypeImport;
        private System.Windows.Forms.RadioButton rdoTypeExport;
        private System.Windows.Forms.GroupBox grpImportTarget;
        private System.Windows.Forms.Button btnCheckDuplicate;
        private System.Windows.Forms.Label lblUnusedTags;
        private System.Windows.Forms.Button btnDeleteUnusedTags;
        private System.Windows.Forms.Label lblUnusedTagsHeader;
        private System.Windows.Forms.Label lblUpdateCategory;
        private System.Windows.Forms.TextBox txtUpdateCategory;
        private System.Windows.Forms.Button btnUpdateCategory;
    }
}