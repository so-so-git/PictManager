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
            this.txtEntryCategory = new System.Windows.Forms.TextBox();
            this.btnDeleteCategory = new System.Windows.Forms.Button();
            this.btnEntryCategory = new System.Windows.Forms.Button();
            this.cmbDeleteCategory = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.grpDeletedImages = new System.Windows.Forms.GroupBox();
            this.lblDeletedCount = new System.Windows.Forms.Label();
            this.btnApplyDelete = new System.Windows.Forms.Button();
            this.lblDeletedCountHeader = new System.Windows.Forms.Label();
            this.barStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnCheckDuplicate = new System.Windows.Forms.Button();
            this.grpImportExport.SuspendLayout();
            this.grpImportExportType.SuspendLayout();
            this.grpImportTarget.SuspendLayout();
            this.grpCategories.SuspendLayout();
            this.grpDeletedImages.SuspendLayout();
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
            this.grpImportExport.Location = new System.Drawing.Point(13, 110);
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
            this.grpImportExportType.TabIndex = 9;
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
            this.grpImportTarget.TabIndex = 9;
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
            this.btnViewDeletedFiles.TabIndex = 8;
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
            this.chkIncludeSubDirectory.TabIndex = 7;
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
            this.cmbImportCategory.TabIndex = 6;
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
            this.lblCategories.TabIndex = 5;
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
            this.grpCategories.Controls.Add(this.txtEntryCategory);
            this.grpCategories.Controls.Add(this.btnDeleteCategory);
            this.grpCategories.Controls.Add(this.btnEntryCategory);
            this.grpCategories.Controls.Add(this.cmbDeleteCategory);
            this.grpCategories.Location = new System.Drawing.Point(13, 13);
            this.grpCategories.Name = "grpCategories";
            this.grpCategories.Size = new System.Drawing.Size(307, 90);
            this.grpCategories.TabIndex = 0;
            this.grpCategories.TabStop = false;
            this.grpCategories.Text = "カテゴリー管理";
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
            // cmbDeleteCategory
            // 
            this.cmbDeleteCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeleteCategory.FormattingEnabled = true;
            this.cmbDeleteCategory.Location = new System.Drawing.Point(9, 54);
            this.cmbDeleteCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbDeleteCategory.Name = "cmbDeleteCategory";
            this.cmbDeleteCategory.Size = new System.Drawing.Size(230, 23);
            this.cmbDeleteCategory.TabIndex = 2;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(375, 405);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 29);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // grpDeletedImages
            // 
            this.grpDeletedImages.Controls.Add(this.lblDeletedCount);
            this.grpDeletedImages.Controls.Add(this.btnApplyDelete);
            this.grpDeletedImages.Controls.Add(this.lblDeletedCountHeader);
            this.grpDeletedImages.Location = new System.Drawing.Point(13, 326);
            this.grpDeletedImages.Name = "grpDeletedImages";
            this.grpDeletedImages.Size = new System.Drawing.Size(307, 55);
            this.grpDeletedImages.TabIndex = 2;
            this.grpDeletedImages.TabStop = false;
            this.grpDeletedImages.Text = "削除済み画像データ";
            // 
            // lblDeletedCount
            // 
            this.lblDeletedCount.AutoSize = true;
            this.lblDeletedCount.Location = new System.Drawing.Point(92, 23);
            this.lblDeletedCount.Name = "lblDeletedCount";
            this.lblDeletedCount.Size = new System.Drawing.Size(53, 15);
            this.lblDeletedCount.TabIndex = 1;
            this.lblDeletedCount.Text = "999,999";
            // 
            // btnApplyDelete
            // 
            this.btnApplyDelete.Location = new System.Drawing.Point(215, 18);
            this.btnApplyDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnApplyDelete.Name = "btnApplyDelete";
            this.btnApplyDelete.Size = new System.Drawing.Size(80, 25);
            this.btnApplyDelete.TabIndex = 2;
            this.btnApplyDelete.Text = "完全に削除";
            this.btnApplyDelete.UseVisualStyleBackColor = true;
            this.btnApplyDelete.Click += new System.EventHandler(this.btnApplyDelete_Click);
            // 
            // lblDeletedCountHeader
            // 
            this.lblDeletedCountHeader.AutoSize = true;
            this.lblDeletedCountHeader.Location = new System.Drawing.Point(7, 23);
            this.lblDeletedCountHeader.Name = "lblDeletedCountHeader";
            this.lblDeletedCountHeader.Size = new System.Drawing.Size(79, 15);
            this.lblDeletedCountHeader.TabIndex = 0;
            this.lblDeletedCountHeader.Text = "削除画像数：";
            // 
            // barStatus
            // 
            this.barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.barStatus.Location = new System.Drawing.Point(0, 438);
            this.barStatus.Name = "barStatus";
            this.barStatus.Size = new System.Drawing.Size(471, 22);
            this.barStatus.TabIndex = 4;
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
            this.btnCheckDuplicate.Location = new System.Drawing.Point(12, 388);
            this.btnCheckDuplicate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCheckDuplicate.Name = "btnCheckDuplicate";
            this.btnCheckDuplicate.Size = new System.Drawing.Size(109, 29);
            this.btnCheckDuplicate.TabIndex = 8;
            this.btnCheckDuplicate.Text = "重複画像を確認";
            this.btnCheckDuplicate.UseVisualStyleBackColor = true;
            this.btnCheckDuplicate.Click += new System.EventHandler(this.btnCheckDuplicate_Click);
            // 
            // MaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 460);
            this.Controls.Add(this.barStatus);
            this.Controls.Add(this.grpDeletedImages);
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
            this.grpDeletedImages.ResumeLayout(false);
            this.grpDeletedImages.PerformLayout();
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
        private System.Windows.Forms.ComboBox cmbDeleteCategory;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.GroupBox grpDeletedImages;
        private System.Windows.Forms.Label lblDeletedCount;
        private System.Windows.Forms.Button btnApplyDelete;
        private System.Windows.Forms.Label lblDeletedCountHeader;
        private System.Windows.Forms.StatusStrip barStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.Button btnViewDeletedFiles;
        private System.Windows.Forms.GroupBox grpImportExportType;
        private System.Windows.Forms.RadioButton rdoTypeImport;
        private System.Windows.Forms.RadioButton rdoTypeExport;
        private System.Windows.Forms.GroupBox grpImportTarget;
        private System.Windows.Forms.Button btnCheckDuplicate;
    }
}