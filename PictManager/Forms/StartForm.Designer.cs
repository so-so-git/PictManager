namespace SO.PictManager.Forms
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
            this.txtTargetDirectory = new System.Windows.Forms.TextBox();
            this.btnRef = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.rdoThumbnail = new System.Windows.Forms.RadioButton();
            this.rdoList = new System.Windows.Forms.RadioButton();
            this.dlgRef = new System.Windows.Forms.FolderBrowserDialog();
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
            this.cmbCategories = new System.Windows.Forms.ComboBox();
            this.btnMaintenance = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.fileWatcher)).BeginInit();
            this.tasktrayMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtTargetDirectory
            // 
            this.txtTargetDirectory.AllowDrop = true;
            this.txtTargetDirectory.Location = new System.Drawing.Point(19, 20);
            this.txtTargetDirectory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTargetDirectory.Name = "txtTargetDirectory";
            this.txtTargetDirectory.Size = new System.Drawing.Size(342, 23);
            this.txtTargetDirectory.TabIndex = 0;
            this.txtTargetDirectory.DragDrop += new System.Windows.Forms.DragEventHandler(this.txtTargetFolder_DragDrop);
            this.txtTargetDirectory.DragEnter += new System.Windows.Forms.DragEventHandler(this.txtTargetDirectory_DragEnter);
            this.txtTargetDirectory.Enter += new System.EventHandler(this.txtTargetDirectory_Enter);
            // 
            // btnRef
            // 
            this.btnRef.Location = new System.Drawing.Point(367, 20);
            this.btnRef.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRef.Name = "btnRef";
            this.btnRef.Size = new System.Drawing.Size(29, 25);
            this.btnRef.TabIndex = 1;
            this.btnRef.Text = "...";
            this.btnRef.UseVisualStyleBackColor = true;
            this.btnRef.Click += new System.EventHandler(this.btnRef_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.Location = new System.Drawing.Point(131, 135);
            this.btnView.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(84, 30);
            this.btnView.TabIndex = 7;
            this.btnView.Text = "閲覧";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQuit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnQuit.Location = new System.Drawing.Point(313, 135);
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
            this.rdoThumbnail.Location = new System.Drawing.Point(269, 60);
            this.rdoThumbnail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoThumbnail.Name = "rdoThumbnail";
            this.rdoThumbnail.Size = new System.Drawing.Size(98, 19);
            this.rdoThumbnail.TabIndex = 4;
            this.rdoThumbnail.Text = "サムネイル表示";
            this.rdoThumbnail.UseVisualStyleBackColor = true;
            // 
            // rdoList
            // 
            this.rdoList.AutoSize = true;
            this.rdoList.Location = new System.Drawing.Point(167, 60);
            this.rdoList.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoList.Name = "rdoList";
            this.rdoList.Size = new System.Drawing.Size(73, 19);
            this.rdoList.TabIndex = 3;
            this.rdoList.Text = "リスト表示";
            this.rdoList.UseVisualStyleBackColor = true;
            // 
            // dlgRef
            // 
            this.dlgRef.Description = "表示対象ディレクトリを選択して下さい。";
            this.dlgRef.ShowNewFolderButton = false;
            // 
            // rdoSlide
            // 
            this.rdoSlide.AutoSize = true;
            this.rdoSlide.Checked = true;
            this.rdoSlide.Location = new System.Drawing.Point(23, 60);
            this.rdoSlide.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoSlide.Name = "rdoSlide";
            this.rdoSlide.Size = new System.Drawing.Size(109, 19);
            this.rdoSlide.TabIndex = 2;
            this.rdoSlide.TabStop = true;
            this.rdoSlide.Text = "スライドショー表示";
            this.rdoSlide.UseVisualStyleBackColor = true;
            // 
            // btnConfig
            // 
            this.btnConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConfig.Location = new System.Drawing.Point(14, 135);
            this.btnConfig.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnConfig.Name = "btnConfig";
            this.btnConfig.Size = new System.Drawing.Size(84, 30);
            this.btnConfig.TabIndex = 6;
            this.btnConfig.Text = "設定";
            this.btnConfig.UseVisualStyleBackColor = true;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // fileWatcher
            // 
            this.fileWatcher.EnableRaisingEvents = true;
            this.fileWatcher.IncludeSubdirectories = true;
            this.fileWatcher.SynchronizingObject = this;
            this.fileWatcher.Created += new System.IO.FileSystemEventHandler(this.fileWatcher_Created);
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
            this.tasktrayMenu.Size = new System.Drawing.Size(221, 70);
            // 
            // menuOpenFolder
            // 
            this.menuOpenFolder.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuOpenFolder.Name = "menuOpenFolder";
            this.menuOpenFolder.Size = new System.Drawing.Size(220, 22);
            this.menuOpenFolder.Text = "監視対象のフォルダを開く";
            this.menuOpenFolder.Click += new System.EventHandler(this.menuOpenFolder_Click);
            // 
            // menuEndWatch
            // 
            this.menuEndWatch.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuEndWatch.Name = "menuEndWatch";
            this.menuEndWatch.Size = new System.Drawing.Size(220, 22);
            this.menuEndWatch.Text = "監視終了";
            this.menuEndWatch.Click += new System.EventHandler(this.menuEndWatch_Click);
            // 
            // menuQuit
            // 
            this.menuQuit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.menuQuit.Name = "menuQuit";
            this.menuQuit.Size = new System.Drawing.Size(220, 22);
            this.menuQuit.Text = "アプリケーション終了";
            this.menuQuit.Click += new System.EventHandler(this.menuQuit_Click);
            // 
            // btnSaveMode
            // 
            this.btnSaveMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMode.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSaveMode.Location = new System.Drawing.Point(222, 135);
            this.btnSaveMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSaveMode.Name = "btnSaveMode";
            this.btnSaveMode.Size = new System.Drawing.Size(84, 30);
            this.btnSaveMode.TabIndex = 8;
            this.btnSaveMode.Text = "監視";
            this.btnSaveMode.UseVisualStyleBackColor = true;
            // 
            // cmbCategories
            // 
            this.cmbCategories.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategories.FormattingEnabled = true;
            this.cmbCategories.Location = new System.Drawing.Point(19, 20);
            this.cmbCategories.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbCategories.Name = "cmbCategories";
            this.cmbCategories.Size = new System.Drawing.Size(230, 23);
            this.cmbCategories.TabIndex = 0;
            // 
            // btnMaintenance
            // 
            this.btnMaintenance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaintenance.Location = new System.Drawing.Point(14, 98);
            this.btnMaintenance.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnMaintenance.Name = "btnMaintenance";
            this.btnMaintenance.Size = new System.Drawing.Size(84, 30);
            this.btnMaintenance.TabIndex = 5;
            this.btnMaintenance.Text = "DBメンテ";
            this.btnMaintenance.UseVisualStyleBackColor = true;
            this.btnMaintenance.Click += new System.EventHandler(this.btnMaintenance_Click);
            // 
            // StartForm
            // 
            this.AcceptButton = this.btnView;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnQuit;
            this.ClientSize = new System.Drawing.Size(411, 182);
            this.Controls.Add(this.cmbCategories);
            this.Controls.Add(this.btnConfig);
            this.Controls.Add(this.rdoList);
            this.Controls.Add(this.rdoSlide);
            this.Controls.Add(this.rdoThumbnail);
            this.Controls.Add(this.btnSaveMode);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnView);
            this.Controls.Add(this.btnMaintenance);
            this.Controls.Add(this.btnRef);
            this.Controls.Add(this.txtTargetDirectory);
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
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtTargetDirectory;
        private System.Windows.Forms.Button btnRef;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnQuit;
        private System.Windows.Forms.RadioButton rdoThumbnail;
        private System.Windows.Forms.RadioButton rdoList;
        private System.Windows.Forms.FolderBrowserDialog dlgRef;
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
        private System.Windows.Forms.ComboBox cmbCategories;
        private System.Windows.Forms.Button btnMaintenance;
    }
}

