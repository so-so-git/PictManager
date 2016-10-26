﻿namespace SO.PictManager.Forms
{
    partial class StartForm
    {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StartForm));
            this.txtTargetFolder = new System.Windows.Forms.TextBox();
            this.btnFolderBrowse = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.rdoThumbnail = new System.Windows.Forms.RadioButton();
            this.rdoList = new System.Windows.Forms.RadioButton();
            this.dlgFolderRef = new System.Windows.Forms.FolderBrowserDialog();
            this.rdoSlide = new System.Windows.Forms.RadioButton();
            this.btnConfig = new System.Windows.Forms.Button();
            this.fileWatcher = new System.IO.FileSystemWatcher();
            this.watchDupliTimer = new System.Windows.Forms.Timer(this.components);
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.tasktrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuOpenFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.menuEndWatch = new System.Windows.Forms.ToolStripMenuItem();
            this.menuQuit = new System.Windows.Forms.ToolStripMenuItem();
            this.btnSaveMode = new System.Windows.Forms.Button();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnMaintenance = new System.Windows.Forms.Button();
            this.btnOpenUrlDrop = new System.Windows.Forms.Button();
            this.grpViewMode = new System.Windows.Forms.GroupBox();
            this.pnlMain = new System.Windows.Forms.Panel();
            this.lblTargetFolder = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.pnlForDatabase = new System.Windows.Forms.Panel();
            this.grpTargetSelect = new System.Windows.Forms.GroupBox();
            this.rdoTargetTag = new System.Windows.Forms.RadioButton();
            this.rdoTargetCategory = new System.Windows.Forms.RadioButton();
            this.lstSearchedTags = new System.Windows.Forms.ListBox();
            this.lblTagSearch = new System.Windows.Forms.Label();
            this.txtTagSearch = new System.Windows.Forms.TextBox();
            this.btnTagSearch = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileWatcher)).BeginInit();
            this.tasktrayMenu.SuspendLayout();
            this.grpViewMode.SuspendLayout();
            this.pnlMain.SuspendLayout();
            this.pnlForDatabase.SuspendLayout();
            this.grpTargetSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTargetFolder
            // 
            this.txtTargetFolder.AllowDrop = true;
            this.txtTargetFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTargetFolder.Location = new System.Drawing.Point(76, 4);
            this.txtTargetFolder.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.Size = new System.Drawing.Size(273, 23);
            this.txtTargetFolder.TabIndex = 1;
            this.txtTargetFolder.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtTargetFolder_DragDrop);
            this.txtTargetFolder.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtTargetFolder_DragEnter);
            this.txtTargetFolder.Enter += new System.EventHandler(this.txtTargetFolder_Enter);
            // 
            // btnFolderBrowse
            // 
            this.btnFolderBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFolderBrowse.Location = new System.Drawing.Point(355, 4);
            this.btnFolderBrowse.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnFolderBrowse.Name = "btnFolderBrowse";
            this.btnFolderBrowse.Size = new System.Drawing.Size(29, 23);
            this.btnFolderBrowse.TabIndex = 2;
            this.btnFolderBrowse.Text = "...";
            this.btnFolderBrowse.UseVisualStyleBackColor = true;
            this.btnFolderBrowse.Click += new System.EventHandler(this.btnFolderBrowse_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.Location = new System.Drawing.Point(120, 98);
            this.btnView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(84, 30);
            this.btnView.TabIndex = 5;
            this.btnView.Text = "閲覧";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnQuit.Location = new System.Drawing.Point(300, 134);
            this.btnQuit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(84, 30);
            this.btnQuit.TabIndex = 9;
            this.btnQuit.Text = "終了";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // rdoThumbnail
            // 
            this.rdoThumbnail.AutoSize = true;
            this.rdoThumbnail.Location = new System.Drawing.Point(265, 23);
            this.rdoThumbnail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoThumbnail.Name = "rdoThumbnail";
            this.rdoThumbnail.Size = new System.Drawing.Size(98, 19);
            this.rdoThumbnail.TabIndex = 2;
            this.rdoThumbnail.Text = "サムネイル表示";
            this.rdoThumbnail.UseVisualStyleBackColor = true;
            // 
            // rdoList
            // 
            this.rdoList.AutoSize = true;
            this.rdoList.Location = new System.Drawing.Point(163, 23);
            this.rdoList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoList.Name = "rdoList";
            this.rdoList.Size = new System.Drawing.Size(73, 19);
            this.rdoList.TabIndex = 1;
            this.rdoList.Text = "リスト表示";
            this.rdoList.UseVisualStyleBackColor = true;
            // 
            // dlgFolderRef
            // 
            this.dlgFolderRef.Description = "表示対象ディレクトリを選択して下さい。";
            this.dlgFolderRef.ShowNewFolderButton = false;
            // 
            // rdoSlide
            // 
            this.rdoSlide.AutoSize = true;
            this.rdoSlide.Checked = true;
            this.rdoSlide.Location = new System.Drawing.Point(19, 23);
            this.rdoSlide.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoSlide.Name = "rdoSlide";
            this.rdoSlide.Size = new System.Drawing.Size(109, 19);
            this.rdoSlide.TabIndex = 0;
            this.rdoSlide.TabStop = true;
            this.rdoSlide.Text = "スライドショー表示";
            this.rdoSlide.UseVisualStyleBackColor = true;
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfig.Location = new System.Drawing.Point(7, 98);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(84, 30);
            this.btnConfig.TabIndex = 4;
            this.btnConfig.Text = "設定";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // fileWatcher
            // 
            this.fileWatcher.EnableRaisingEvents = true;
            this.fileWatcher.IncludeSubdirectories = true;
            this.fileWatcher.SynchronizingObject = this;
            // 
            // watchDupliTimer
            // 
            this.watchDupliTimer.Interval = 5000;
            this.watchDupliTimer.Tick += new System.EventHandler(this.watchDupliTimer_Tick);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.tasktrayMenu;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // tasktrayMenu
            // 
            this.tasktrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuOpenFolder,
            this.menuEndWatch,
            this.menuQuit});
            this.tasktrayMenu.Name = "tasktrayMenu";
            this.tasktrayMenu.Size = new System.Drawing.Size(195, 70);
            // 
            // menuOpenFolder
            // 
            this.menuOpenFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuOpenFolder.Name = "menuOpenFolder";
            this.menuOpenFolder.Size = new System.Drawing.Size(194, 22);
            this.menuOpenFolder.Text = "監視対象のフォルダを開く";
            this.menuOpenFolder.Click += new System.EventHandler(this.menuOpenFolder_Click);
            // 
            // menuEndWatch
            // 
            this.menuEndWatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuEndWatch.Name = "menuEndWatch";
            this.menuEndWatch.Size = new System.Drawing.Size(194, 22);
            this.menuEndWatch.Text = "監視終了";
            this.menuEndWatch.Click += new System.EventHandler(this.menuEndWatch_Click);
            // 
            // menuQuit
            // 
            this.menuQuit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuQuit.Name = "menuQuit";
            this.menuQuit.Size = new System.Drawing.Size(194, 22);
            this.menuQuit.Text = "アプリケーション終了";
            this.menuQuit.Click += new System.EventHandler(this.menuQuit_Click);
            // 
            // btnSaveMode
            // 
            this.btnSaveMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMode.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveMode.Location = new System.Drawing.Point(210, 98);
            this.btnSaveMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveMode.Name = "btnSaveMode";
            this.btnSaveMode.Size = new System.Drawing.Size(84, 30);
            this.btnSaveMode.TabIndex = 6;
            this.btnSaveMode.Text = "監視";
            this.btnSaveMode.UseVisualStyleBackColor = true;
            // 
            // cmbCategory
            // 
            this.cmbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategory.DisplayMember = "CategoryName";
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(76, 63);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(218, 23);
            this.cmbCategory.TabIndex = 1;
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMaintenance.Location = new System.Drawing.Point(7, 134);
            this.btnMaintenance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(84, 30);
            this.btnMaintenance.TabIndex = 8;
            this.btnMaintenance.Text = "DBメンテ";
            this.btnMaintenance.UseVisualStyleBackColor = true;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // btnOpenUrlDrop
            // 
            this.btnOpenUrlDrop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOpenUrlDrop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOpenUrlDrop.Location = new System.Drawing.Point(300, 98);
            this.btnOpenUrlDrop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOpenUrlDrop.Name = "btnOpenUrlDrop";
            this.btnOpenUrlDrop.Size = new System.Drawing.Size(84, 30);
            this.btnOpenUrlDrop.TabIndex = 7;
            this.btnOpenUrlDrop.Text = "URL受付";
            this.btnOpenUrlDrop.UseVisualStyleBackColor = true;
            this.btnOpenUrlDrop.Click += new System.EventHandler(this.btnOpenUrlDrop_Click);
            // 
            // grpViewMode
            // 
            this.grpViewMode.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpViewMode.Controls.Add(this.rdoSlide);
            this.grpViewMode.Controls.Add(this.rdoThumbnail);
            this.grpViewMode.Controls.Add(this.rdoList);
            this.grpViewMode.Location = new System.Drawing.Point(7, 34);
            this.grpViewMode.Name = "grpViewMode";
            this.grpViewMode.Size = new System.Drawing.Size(377, 55);
            this.grpViewMode.TabIndex = 3;
            this.grpViewMode.TabStop = false;
            this.grpViewMode.Text = "表示モード";
            // 
            // pnlMain
            // 
            this.pnlMain.Controls.Add(this.lblTargetFolder);
            this.pnlMain.Controls.Add(this.txtTargetFolder);
            this.pnlMain.Controls.Add(this.btnMaintenance);
            this.pnlMain.Controls.Add(this.btnQuit);
            this.pnlMain.Controls.Add(this.btnFolderBrowse);
            this.pnlMain.Controls.Add(this.btnOpenUrlDrop);
            this.pnlMain.Controls.Add(this.grpViewMode);
            this.pnlMain.Controls.Add(this.btnConfig);
            this.pnlMain.Controls.Add(this.btnSaveMode);
            this.pnlMain.Controls.Add(this.btnView);
            this.pnlMain.Location = new System.Drawing.Point(9, 237);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Size = new System.Drawing.Size(389, 170);
            this.pnlMain.TabIndex = 1;
            // 
            // lblTargetFolder
            // 
            this.lblTargetFolder.AutoSize = true;
            this.lblTargetFolder.Location = new System.Drawing.Point(4, 7);
            this.lblTargetFolder.Name = "lblTargetFolder";
            this.lblTargetFolder.Size = new System.Drawing.Size(66, 15);
            this.lblTargetFolder.TabIndex = 0;
            this.lblTargetFolder.Text = "対象フォルダ";
            // 
            // lblCategory
            // 
            this.lblCategory.AutoSize = true;
            this.lblCategory.Location = new System.Drawing.Point(4, 66);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(51, 15);
            this.lblCategory.TabIndex = 0;
            this.lblCategory.Text = "カテゴリー";
            // 
            // pnlForDatabase
            // 
            this.pnlForDatabase.Controls.Add(this.grpTargetSelect);
            this.pnlForDatabase.Controls.Add(this.lstSearchedTags);
            this.pnlForDatabase.Controls.Add(this.lblTagSearch);
            this.pnlForDatabase.Controls.Add(this.lblCategory);
            this.pnlForDatabase.Controls.Add(this.txtTagSearch);
            this.pnlForDatabase.Controls.Add(this.btnTagSearch);
            this.pnlForDatabase.Controls.Add(this.cmbCategory);
            this.pnlForDatabase.Location = new System.Drawing.Point(9, 3);
            this.pnlForDatabase.Name = "pnlForDatabase";
            this.pnlForDatabase.Size = new System.Drawing.Size(389, 228);
            this.pnlForDatabase.TabIndex = 0;
            // 
            // grpTargetSelect
            // 
            this.grpTargetSelect.Controls.Add(this.rdoTargetTag);
            this.grpTargetSelect.Controls.Add(this.rdoTargetCategory);
            this.grpTargetSelect.Location = new System.Drawing.Point(7, 3);
            this.grpTargetSelect.Name = "grpTargetSelect";
            this.grpTargetSelect.Size = new System.Drawing.Size(215, 53);
            this.grpTargetSelect.TabIndex = 6;
            this.grpTargetSelect.TabStop = false;
            this.grpTargetSelect.Text = "対象指定";
            // 
            // rdoTargetTag
            // 
            this.rdoTargetTag.AutoSize = true;
            this.rdoTargetTag.Location = new System.Drawing.Point(129, 22);
            this.rdoTargetTag.Name = "rdoTargetTag";
            this.rdoTargetTag.Size = new System.Drawing.Size(66, 19);
            this.rdoTargetTag.TabIndex = 0;
            this.rdoTargetTag.Text = "タグ指定";
            this.rdoTargetTag.UseVisualStyleBackColor = true;
            this.rdoTargetTag.CheckedChanged += new System.EventHandler(this.rdoTargetSelect_CheckedChanged);
            // 
            // rdoTargetCategory
            // 
            this.rdoTargetCategory.AutoSize = true;
            this.rdoTargetCategory.Checked = true;
            this.rdoTargetCategory.Location = new System.Drawing.Point(19, 22);
            this.rdoTargetCategory.Name = "rdoTargetCategory";
            this.rdoTargetCategory.Size = new System.Drawing.Size(93, 19);
            this.rdoTargetCategory.TabIndex = 0;
            this.rdoTargetCategory.TabStop = true;
            this.rdoTargetCategory.Text = "カテゴリー指定";
            this.rdoTargetCategory.UseVisualStyleBackColor = true;
            this.rdoTargetCategory.CheckedChanged += new System.EventHandler(this.rdoTargetSelect_CheckedChanged);
            // 
            // lstSearchedTags
            // 
            this.lstSearchedTags.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSearchedTags.DisplayMember = "TagName";
            this.lstSearchedTags.Enabled = false;
            this.lstSearchedTags.FormattingEnabled = true;
            this.lstSearchedTags.ItemHeight = 15;
            this.lstSearchedTags.Location = new System.Drawing.Point(76, 125);
            this.lstSearchedTags.Name = "lstSearchedTags";
            this.lstSearchedTags.Size = new System.Drawing.Size(237, 94);
            this.lstSearchedTags.TabIndex = 5;
            // 
            // lblTagSearch
            // 
            this.lblTagSearch.AutoSize = true;
            this.lblTagSearch.Enabled = false;
            this.lblTagSearch.Location = new System.Drawing.Point(4, 98);
            this.lblTagSearch.Name = "lblTagSearch";
            this.lblTagSearch.Size = new System.Drawing.Size(24, 15);
            this.lblTagSearch.TabIndex = 2;
            this.lblTagSearch.Text = "タグ";
            // 
            // txtTagSearch
            // 
            this.txtTagSearch.AllowDrop = true;
            this.txtTagSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTagSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtTagSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtTagSearch.Enabled = false;
            this.txtTagSearch.Location = new System.Drawing.Point(76, 94);
            this.txtTagSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTagSearch.Name = "txtTagSearch";
            this.txtTagSearch.Size = new System.Drawing.Size(237, 23);
            this.txtTagSearch.TabIndex = 3;
            this.txtTagSearch.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtTargetFolder_DragDrop);
            this.txtTagSearch.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtTargetFolder_DragEnter);
            this.txtTagSearch.Enter += new System.EventHandler(this.txtTargetFolder_Enter);
            // 
            // btnTagSearch
            // 
            this.btnTagSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTagSearch.Enabled = false;
            this.btnTagSearch.Location = new System.Drawing.Point(319, 94);
            this.btnTagSearch.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnTagSearch.Name = "btnTagSearch";
            this.btnTagSearch.Size = new System.Drawing.Size(65, 23);
            this.btnTagSearch.TabIndex = 4;
            this.btnTagSearch.Text = "タグ検索";
            this.btnTagSearch.UseVisualStyleBackColor = true;
            this.btnTagSearch.Click += new System.EventHandler(this.btnTagSearch_Click);
            // 
            // StartForm
            // 
            this.AcceptButton = this.btnView;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnQuit;
            this.ClientSize = new System.Drawing.Size(408, 418);
            this.Controls.Add(this.pnlForDatabase);
            this.Controls.Add(this.pnlMain);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "StartForm";
            this.Text = "PictManager - 対象ディレクトリ指定";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.fileWatcher)).EndInit();
            this.tasktrayMenu.ResumeLayout(false);
            this.grpViewMode.ResumeLayout(false);
            this.grpViewMode.PerformLayout();
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlForDatabase.ResumeLayout(false);
            this.pnlForDatabase.PerformLayout();
            this.grpTargetSelect.ResumeLayout(false);
            this.grpTargetSelect.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtTargetFolder;
        private System.Windows.Forms.Button btnFolderBrowse;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.RadioButton rdoThumbnail;
        private System.Windows.Forms.RadioButton rdoList;
        private System.Windows.Forms.FolderBrowserDialog dlgFolderRef;
        private System.Windows.Forms.RadioButton rdoSlide;
        private System.Windows.Forms.Button btnConfig;
        private System.IO.FileSystemWatcher fileWatcher;
        private System.Windows.Forms.Timer watchDupliTimer;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.Button btnSaveMode;
        private System.Windows.Forms.ContextMenuStrip tasktrayMenu;
        private System.Windows.Forms.ToolStripMenuItem menuEndWatch;
        private System.Windows.Forms.ToolStripMenuItem menuQuit;
        private System.Windows.Forms.ToolStripMenuItem menuOpenFolder;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnMaintenance;
        private System.Windows.Forms.Button btnOpenUrlDrop;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.GroupBox grpViewMode;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblTargetFolder;
        private System.Windows.Forms.Panel pnlForDatabase;
        private System.Windows.Forms.Label lblTagSearch;
        private System.Windows.Forms.TextBox txtTagSearch;
        private System.Windows.Forms.Button btnTagSearch;
        private System.Windows.Forms.ListBox lstSearchedTags;
        private System.Windows.Forms.GroupBox grpTargetSelect;
        private System.Windows.Forms.RadioButton rdoTargetCategory;
        private System.Windows.Forms.RadioButton rdoTargetTag;
    }
}

