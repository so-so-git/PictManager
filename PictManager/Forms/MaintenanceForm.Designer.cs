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
            this.grpImport = new System.Windows.Forms.GroupBox();
            this.rdoImportDirectory = new System.Windows.Forms.RadioButton();
            this.rdoImportFile = new System.Windows.Forms.RadioButton();
            this.btnImport = new System.Windows.Forms.Button();
            this.chkIncludeSubDirectory = new System.Windows.Forms.CheckBox();
            this.btnRef = new System.Windows.Forms.Button();
            this.txtTargetPath = new System.Windows.Forms.TextBox();
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.lblTargetDirectory = new System.Windows.Forms.Label();
            this.lblCategories = new System.Windows.Forms.Label();
            this.dlgImportFile = new System.Windows.Forms.OpenFileDialog();
            this.dlgImportDirectory = new System.Windows.Forms.FolderBrowserDialog();
            this.grpImport.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpImport
            // 
            this.grpImport.Controls.Add(this.rdoImportDirectory);
            this.grpImport.Controls.Add(this.rdoImportFile);
            this.grpImport.Controls.Add(this.btnImport);
            this.grpImport.Controls.Add(this.chkIncludeSubDirectory);
            this.grpImport.Controls.Add(this.btnRef);
            this.grpImport.Controls.Add(this.txtTargetPath);
            this.grpImport.Controls.Add(this.cmbCategories);
            this.grpImport.Controls.Add(this.lblTargetDirectory);
            this.grpImport.Controls.Add(this.lblCategories);
            this.grpImport.Location = new System.Drawing.Point(15, 16);
            this.grpImport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpImport.Name = "grpImport";
            this.grpImport.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpImport.Size = new System.Drawing.Size(449, 159);
            this.grpImport.TabIndex = 0;
            this.grpImport.TabStop = false;
            this.grpImport.Text = "画像ファイルインポート";
            // 
            // rdoImportDirectory
            // 
            this.rdoImportDirectory.AutoSize = true;
            this.rdoImportDirectory.Checked = true;
            this.rdoImportDirectory.Location = new System.Drawing.Point(9, 24);
            this.rdoImportDirectory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoImportDirectory.Name = "rdoImportDirectory";
            this.rdoImportDirectory.Size = new System.Drawing.Size(74, 19);
            this.rdoImportDirectory.TabIndex = 0;
            this.rdoImportDirectory.TabStop = true;
            this.rdoImportDirectory.Text = "ディレクトリ";
            this.rdoImportDirectory.UseVisualStyleBackColor = true;
            this.rdoImportDirectory.CheckedChanged += new System.EventHandler(this.rdoImportKinds_CheckedChanged);
            // 
            // rdoImportFile
            // 
            this.rdoImportFile.AutoSize = true;
            this.rdoImportFile.Location = new System.Drawing.Point(100, 24);
            this.rdoImportFile.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoImportFile.Name = "rdoImportFile";
            this.rdoImportFile.Size = new System.Drawing.Size(59, 19);
            this.rdoImportFile.TabIndex = 1;
            this.rdoImportFile.Text = "ファイル";
            this.rdoImportFile.UseVisualStyleBackColor = true;
            this.rdoImportFile.CheckedChanged += new System.EventHandler(this.rdoImportKinds_CheckedChanged);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Location = new System.Drawing.Point(355, 121);
            this.btnImport.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(87, 29);
            this.btnImport.TabIndex = 8;
            this.btnImport.Text = "インポート";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // chkIncludeSubDirectory
            // 
            this.chkIncludeSubDirectory.AutoSize = true;
            this.chkIncludeSubDirectory.Location = new System.Drawing.Point(9, 119);
            this.chkIncludeSubDirectory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkIncludeSubDirectory.Name = "chkIncludeSubDirectory";
            this.chkIncludeSubDirectory.Size = new System.Drawing.Size(125, 19);
            this.chkIncludeSubDirectory.TabIndex = 7;
            this.chkIncludeSubDirectory.Text = "サブディレクトリも含む";
            this.chkIncludeSubDirectory.UseVisualStyleBackColor = true;
            // 
            // btnRef
            // 
            this.btnRef.Location = new System.Drawing.Point(419, 55);
            this.btnRef.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRef.Name = "btnRef";
            this.btnRef.Size = new System.Drawing.Size(23, 25);
            this.btnRef.TabIndex = 4;
            this.btnRef.Text = "...";
            this.btnRef.UseVisualStyleBackColor = true;
            this.btnRef.Click += new System.EventHandler(this.btnRef_Click);
            // 
            // txtTargetPath
            // 
            this.txtTargetPath.AllowDrop = true;
            this.txtTargetPath.Location = new System.Drawing.Point(66, 55);
            this.txtTargetPath.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.Size = new System.Drawing.Size(345, 23);
            this.txtTargetPath.TabIndex = 3;
            // 
            // cmbCategories
            // 
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(66, 86);
            this.cmbCategories.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(230, 23);
            this.cmbCategories.TabIndex = 6;
            // 
            // lblTargetDirectory
            // 
            this.lblTargetDirectory.AutoSize = true;
            this.lblTargetDirectory.Location = new System.Drawing.Point(7, 60);
            this.lblTargetDirectory.Name = "lblTargetDirectory";
            this.lblTargetDirectory.Size = new System.Drawing.Size(43, 15);
            this.lblTargetDirectory.TabIndex = 2;
            this.lblTargetDirectory.Text = "対象：";
            // 
            // lblCategories
            // 
            this.lblCategories.AutoSize = true;
            this.lblCategories.Location = new System.Drawing.Point(7, 90);
            this.lblCategories.Name = "lblCategories";
            this.lblCategories.Size = new System.Drawing.Size(53, 15);
            this.lblCategories.TabIndex = 5;
            this.lblCategories.Text = "カテゴリ：";
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
            // MaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(478, 190);
            this.Controls.Add(this.grpImport);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "MaintenanceForm";
            this.Text = "PictManager - データベースメンテナンス";
            this.grpImport.ResumeLayout(false);
            this.grpImport.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpImport;
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.Label lblCategories;
        private System.Windows.Forms.CheckBox chkIncludeSubDirectory;
        private System.Windows.Forms.Button btnRef;
        private System.Windows.Forms.TextBox txtTargetPath;
        private System.Windows.Forms.Label lblTargetDirectory;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.RadioButton rdoImportDirectory;
        private System.Windows.Forms.RadioButton rdoImportFile;
        private System.Windows.Forms.OpenFileDialog dlgImportFile;
        private System.Windows.Forms.FolderBrowserDialog dlgImportDirectory;
    }
}