namespace SO.PictManager.Forms
{
    partial class ConfigDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param orderName="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            this.tabConfig = new System.Windows.Forms.TabControl();
            this.tbpCommon = new System.Windows.Forms.TabPage();
            this.grpImageDataMode = new System.Windows.Forms.GroupBox();
            this.rdoModeDatabase = new System.Windows.Forms.RadioButton();
            this.rdoModeFile = new System.Windows.Forms.RadioButton();
            this.chkConfirmQuit = new System.Windows.Forms.CheckBox();
            this.chkIncludeSub = new System.Windows.Forms.CheckBox();
            this.grpFileFormat = new System.Windows.Forms.GroupBox();
            this.chkIco = new System.Windows.Forms.CheckBox();
            this.chkPng = new System.Windows.Forms.CheckBox();
            this.chkGif = new System.Windows.Forms.CheckBox();
            this.chkJpg = new System.Windows.Forms.CheckBox();
            this.chkBmp = new System.Windows.Forms.CheckBox();
            this.tbpSlide = new System.Windows.Forms.TabPage();
            this.chkBookmarkTopMost = new System.Windows.Forms.CheckBox();
            this.tbpList = new System.Windows.Forms.TabPage();
            this.tbpThumbnail = new System.Windows.Forms.TabPage();
            this.btnApply = new System.Windows.Forms.Button();
            this.tabConfig.SuspendLayout();
            this.tbpCommon.SuspendLayout();
            this.grpImageDataMode.SuspendLayout();
            this.grpFileFormat.SuspendLayout();
            this.tbpSlide.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(376, 249);
            this.btnOk.TabIndex = 3;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(304, 249);
            this.btnCancel.TabIndex = 2;
            // 
            // tabConfig
            // 
            this.tabConfig.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabConfig.Controls.Add(this.tbpCommon);
            this.tabConfig.Controls.Add(this.tbpSlide);
            this.tabConfig.Controls.Add(this.tbpList);
            this.tabConfig.Controls.Add(this.tbpThumbnail);
            this.tabConfig.Location = new System.Drawing.Point(12, 12);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.SelectedIndex = 0;
            this.tabConfig.Size = new System.Drawing.Size(436, 231);
            this.tabConfig.TabIndex = 0;
            // 
            // tbpCommon
            // 
            this.tbpCommon.Controls.Add(this.grpImageDataMode);
            this.tbpCommon.Controls.Add(this.chkConfirmQuit);
            this.tbpCommon.Controls.Add(this.chkIncludeSub);
            this.tbpCommon.Controls.Add(this.grpFileFormat);
            this.tbpCommon.Location = new System.Drawing.Point(4, 22);
            this.tbpCommon.Name = "tbpCommon";
            this.tbpCommon.Padding = new System.Windows.Forms.Padding(3);
            this.tbpCommon.Size = new System.Drawing.Size(428, 205);
            this.tbpCommon.TabIndex = 0;
            this.tbpCommon.Text = "共通";
            this.tbpCommon.UseVisualStyleBackColor = true;
            // 
            // grpImageDataMode
            // 
            this.grpImageDataMode.Controls.Add(this.rdoModeDatabase);
            this.grpImageDataMode.Controls.Add(this.rdoModeFile);
            this.grpImageDataMode.Location = new System.Drawing.Point(6, 7);
            this.grpImageDataMode.Name = "grpImageDataMode";
            this.grpImageDataMode.Size = new System.Drawing.Size(292, 48);
            this.grpImageDataMode.TabIndex = 0;
            this.grpImageDataMode.TabStop = false;
            this.grpImageDataMode.Text = "画像データモード";
            // 
            // rdoModeDatabase
            // 
            this.rdoModeDatabase.AutoSize = true;
            this.rdoModeDatabase.Location = new System.Drawing.Point(78, 20);
            this.rdoModeDatabase.Name = "rdoModeDatabase";
            this.rdoModeDatabase.Size = new System.Drawing.Size(80, 16);
            this.rdoModeDatabase.TabIndex = 1;
            this.rdoModeDatabase.TabStop = true;
            this.rdoModeDatabase.Text = "データベース";
            this.rdoModeDatabase.UseVisualStyleBackColor = true;
            // 
            // rdoModeFile
            // 
            this.rdoModeFile.AutoSize = true;
            this.rdoModeFile.Location = new System.Drawing.Point(12, 20);
            this.rdoModeFile.Name = "rdoModeFile";
            this.rdoModeFile.Size = new System.Drawing.Size(57, 16);
            this.rdoModeFile.TabIndex = 0;
            this.rdoModeFile.TabStop = true;
            this.rdoModeFile.Text = "ファイル";
            this.rdoModeFile.UseVisualStyleBackColor = true;
            // 
            // chkConfirmQuit
            // 
            this.chkConfirmQuit.AutoSize = true;
            this.chkConfirmQuit.Location = new System.Drawing.Point(6, 138);
            this.chkConfirmQuit.Name = "chkConfirmQuit";
            this.chkConfirmQuit.Size = new System.Drawing.Size(112, 16);
            this.chkConfirmQuit.TabIndex = 4;
            this.chkConfirmQuit.Text = "終了時に確認する";
            this.chkConfirmQuit.UseVisualStyleBackColor = true;
            // 
            // chkIncludeSub
            // 
            this.chkIncludeSub.AutoSize = true;
            this.chkIncludeSub.Location = new System.Drawing.Point(6, 116);
            this.chkIncludeSub.Name = "chkIncludeSub";
            this.chkIncludeSub.Size = new System.Drawing.Size(130, 16);
            this.chkIncludeSub.TabIndex = 3;
            this.chkIncludeSub.Text = "サブフォルダを検索する";
            this.chkIncludeSub.UseVisualStyleBackColor = true;
            // 
            // grpFileFormat
            // 
            this.grpFileFormat.Controls.Add(this.chkIco);
            this.grpFileFormat.Controls.Add(this.chkPng);
            this.grpFileFormat.Controls.Add(this.chkGif);
            this.grpFileFormat.Controls.Add(this.chkJpg);
            this.grpFileFormat.Controls.Add(this.chkBmp);
            this.grpFileFormat.Location = new System.Drawing.Point(6, 62);
            this.grpFileFormat.Name = "grpFileFormat";
            this.grpFileFormat.Size = new System.Drawing.Size(292, 48);
            this.grpFileFormat.TabIndex = 1;
            this.grpFileFormat.TabStop = false;
            this.grpFileFormat.Text = "対象ファイル形式";
            // 
            // chkIco
            // 
            this.chkIco.AutoSize = true;
            this.chkIco.Location = new System.Drawing.Point(238, 20);
            this.chkIco.Name = "chkIco";
            this.chkIco.Size = new System.Drawing.Size(45, 16);
            this.chkIco.TabIndex = 4;
            this.chkIco.Tag = "ico";
            this.chkIco.Text = "Icon";
            this.chkIco.UseVisualStyleBackColor = true;
            // 
            // chkPng
            // 
            this.chkPng.AutoSize = true;
            this.chkPng.Location = new System.Drawing.Point(185, 20);
            this.chkPng.Name = "chkPng";
            this.chkPng.Size = new System.Drawing.Size(47, 16);
            this.chkPng.TabIndex = 3;
            this.chkPng.Tag = "png";
            this.chkPng.Text = "PNG";
            this.chkPng.UseVisualStyleBackColor = true;
            // 
            // chkGif
            // 
            this.chkGif.AutoSize = true;
            this.chkGif.Location = new System.Drawing.Point(137, 20);
            this.chkGif.Name = "chkGif";
            this.chkGif.Size = new System.Drawing.Size(42, 16);
            this.chkGif.TabIndex = 2;
            this.chkGif.Tag = "gif";
            this.chkGif.Text = "GIF";
            this.chkGif.UseVisualStyleBackColor = true;
            // 
            // chkJpg
            // 
            this.chkJpg.AutoSize = true;
            this.chkJpg.Location = new System.Drawing.Point(78, 20);
            this.chkJpg.Name = "chkJpg";
            this.chkJpg.Size = new System.Drawing.Size(53, 16);
            this.chkJpg.TabIndex = 1;
            this.chkJpg.Tag = "jpg,jpeg";
            this.chkJpg.Text = "JPEG";
            this.chkJpg.UseVisualStyleBackColor = true;
            // 
            // chkBmp
            // 
            this.chkBmp.AutoSize = true;
            this.chkBmp.Location = new System.Drawing.Point(12, 20);
            this.chkBmp.Name = "chkBmp";
            this.chkBmp.Size = new System.Drawing.Size(60, 16);
            this.chkBmp.TabIndex = 0;
            this.chkBmp.Tag = "bmp";
            this.chkBmp.Text = "Bitmap";
            this.chkBmp.UseVisualStyleBackColor = true;
            // 
            // tbpSlide
            // 
            this.tbpSlide.Controls.Add(this.chkBookmarkTopMost);
            this.tbpSlide.Location = new System.Drawing.Point(4, 22);
            this.tbpSlide.Name = "tbpSlide";
            this.tbpSlide.Size = new System.Drawing.Size(428, 205);
            this.tbpSlide.TabIndex = 2;
            this.tbpSlide.Text = "スライド表示";
            this.tbpSlide.UseVisualStyleBackColor = true;
            // 
            // chkBookmarkTopMost
            // 
            this.chkBookmarkTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkBookmarkTopMost.AutoSize = true;
            this.chkBookmarkTopMost.Location = new System.Drawing.Point(12, 13);
            this.chkBookmarkTopMost.Name = "chkBookmarkTopMost";
            this.chkBookmarkTopMost.Size = new System.Drawing.Size(203, 16);
            this.chkBookmarkTopMost.TabIndex = 4;
            this.chkBookmarkTopMost.Text = "ブックマークウィンドウを前面に表示する";
            this.chkBookmarkTopMost.UseVisualStyleBackColor = true;
            // 
            // tbpList
            // 
            this.tbpList.Location = new System.Drawing.Point(4, 22);
            this.tbpList.Name = "tbpList";
            this.tbpList.Size = new System.Drawing.Size(428, 205);
            this.tbpList.TabIndex = 3;
            this.tbpList.Text = "リスト表示";
            this.tbpList.UseVisualStyleBackColor = true;
            // 
            // tbpThumbnail
            // 
            this.tbpThumbnail.Location = new System.Drawing.Point(4, 22);
            this.tbpThumbnail.Name = "tbpThumbnail";
            this.tbpThumbnail.Size = new System.Drawing.Size(428, 205);
            this.tbpThumbnail.TabIndex = 1;
            this.tbpThumbnail.Text = "サムネイル表示";
            this.tbpThumbnail.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Location = new System.Drawing.Point(234, 249);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(64, 24);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // ConfigDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(460, 287);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tabConfig);
            this.Name = "ConfigDialog";
            this.Text = "PictManager - システム設定";
            this.Controls.SetChildIndex(this.tabConfig, 0);
            this.Controls.SetChildIndex(this.btnApply, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.tabConfig.ResumeLayout(false);
            this.tbpCommon.ResumeLayout(false);
            this.tbpCommon.PerformLayout();
            this.grpImageDataMode.ResumeLayout(false);
            this.grpImageDataMode.PerformLayout();
            this.grpFileFormat.ResumeLayout(false);
            this.grpFileFormat.PerformLayout();
            this.tbpSlide.ResumeLayout(false);
            this.tbpSlide.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabConfig;
        private System.Windows.Forms.TabPage tbpCommon;
        private System.Windows.Forms.TabPage tbpSlide;
        private System.Windows.Forms.TabPage tbpList;
        private System.Windows.Forms.TabPage tbpThumbnail;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.GroupBox grpFileFormat;
        private System.Windows.Forms.CheckBox chkBmp;
        private System.Windows.Forms.CheckBox chkGif;
        private System.Windows.Forms.CheckBox chkJpg;
        private System.Windows.Forms.CheckBox chkIco;
        private System.Windows.Forms.CheckBox chkPng;
        private System.Windows.Forms.CheckBox chkIncludeSub;
        private System.Windows.Forms.CheckBox chkConfirmQuit;
        private System.Windows.Forms.CheckBox chkBookmarkTopMost;
        private System.Windows.Forms.GroupBox grpImageDataMode;
        private System.Windows.Forms.RadioButton rdoModeDatabase;
        private System.Windows.Forms.RadioButton rdoModeFile;
    }
}