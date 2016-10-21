namespace SO.PictManager.Forms
{
    partial class ViewImageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewImageForm));
            this.cmbLupeMagnification = new System.Windows.Forms.ComboBox();
            this.chkLupe = new SO.PictManager.Components.KeyPrevCheckBox();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.cmbPicMode = new System.Windows.Forms.ComboBox();
            this.btnDelete = new SO.PictManager.Components.KeyPrevButton();
            this.btnClose = new SO.PictManager.Components.KeyPrevButton();
            this.barStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.timLupe = new System.Windows.Forms.Timer(this.components);
            this.pnlTags = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTagPanelToggle = new System.Windows.Forms.Button();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.picLupe = new System.Windows.Forms.PictureBox();
            this.picViewer = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.barStatus.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLupe)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbLupeMagnification
            // 
            this.cmbLupeMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLupeMagnification.FormattingEnabled = true;
            this.cmbLupeMagnification.Location = new System.Drawing.Point(170, 15);
            this.cmbLupeMagnification.Name = "cmbLupeMagnification";
            this.cmbLupeMagnification.Size = new System.Drawing.Size(42, 23);
            this.cmbLupeMagnification.TabIndex = 2;
            this.cmbLupeMagnification.SelectedIndexChanged += new System.EventHandler(this.cmbLupeMagnification_SelectedIndexChanged);
            // 
            // chkLupe
            // 
            this.chkLupe.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLupe.Location = new System.Drawing.Point(91, 11);
            this.chkLupe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkLupe.Name = "chkLupe";
            this.chkLupe.Size = new System.Drawing.Size(73, 29);
            this.chkLupe.TabIndex = 1;
            this.chkLupe.Text = "部分拡大";
            this.chkLupe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkLupe.UseVisualStyleBackColor = true;
            this.chkLupe.CheckedChanged += new System.EventHandler(this.chkLupe_CheckedChanged);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.Location = new System.Drawing.Point(511, 11);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(31, 29);
            this.btnZoomOut.TabIndex = 4;
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.Location = new System.Drawing.Point(474, 11);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(31, 29);
            this.btnZoomIn.TabIndex = 3;
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // cmbPicMode
            // 
            this.cmbPicMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPicMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPicMode.FormattingEnabled = true;
            this.cmbPicMode.Location = new System.Drawing.Point(548, 15);
            this.cmbPicMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbPicMode.Name = "cmbPicMode";
            this.cmbPicMode.Size = new System.Drawing.Size(98, 23);
            this.cmbPicMode.TabIndex = 5;
            this.cmbPicMode.SelectedIndexChanged += new System.EventHandler(this.cmbPicMode_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 11);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(73, 29);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(652, 11);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(73, 29);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // barStatus
            // 
            this.barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.barStatus.Location = new System.Drawing.Point(0, 536);
            this.barStatus.Name = "barStatus";
            this.barStatus.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.barStatus.Size = new System.Drawing.Size(737, 22);
            this.barStatus.TabIndex = 7;
            this.barStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(86, 17);
            this.lblStatus.Text = "[ステータス表示]";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // timLupe
            // 
            this.timLupe.Interval = 10;
            this.timLupe.Tick += new System.EventHandler(this.timLupe_Tick);
            // 
            // pnlTags
            // 
            this.pnlTags.BackColor = System.Drawing.SystemColors.Info;
            this.pnlTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTags.Location = new System.Drawing.Point(0, 0);
            this.pnlTags.Name = "pnlTags";
            this.pnlTags.Size = new System.Drawing.Size(697, 30);
            this.pnlTags.TabIndex = 1;
            // 
            // btnTagPanelToggle
            // 
            this.btnTagPanelToggle.BackColor = System.Drawing.SystemColors.Info;
            this.btnTagPanelToggle.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnTagPanelToggle.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.btnTagPanelToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTagPanelToggle.Image = global::SO.PictManager.Properties.Resources.arrow_down;
            this.btnTagPanelToggle.Location = new System.Drawing.Point(697, 0);
            this.btnTagPanelToggle.Name = "btnTagPanelToggle";
            this.btnTagPanelToggle.Size = new System.Drawing.Size(40, 30);
            this.btnTagPanelToggle.TabIndex = 0;
            this.btnTagPanelToggle.UseVisualStyleBackColor = false;
            this.btnTagPanelToggle.Visible = false;
            this.btnTagPanelToggle.Click += new System.EventHandler(this.btnTagPanelToggle_Click);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.cmbLupeMagnification);
            this.pnlFooter.Controls.Add(this.btnZoomOut);
            this.pnlFooter.Controls.Add(this.btnZoomIn);
            this.pnlFooter.Controls.Add(this.btnDelete);
            this.pnlFooter.Controls.Add(this.cmbPicMode);
            this.pnlFooter.Controls.Add(this.chkLupe);
            this.pnlFooter.Controls.Add(this.btnClose);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 486);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(737, 50);
            this.pnlFooter.TabIndex = 1;
            // 
            // picLupe
            // 
            this.picLupe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLupe.Location = new System.Drawing.Point(12, 7);
            this.picLupe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picLupe.Name = "picLupe";
            this.picLupe.Size = new System.Drawing.Size(200, 200);
            this.picLupe.TabIndex = 10;
            this.picLupe.TabStop = false;
            this.picLupe.Visible = false;
            // 
            // picViewer
            // 
            this.picViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picViewer.ErrorImage = null;
            this.picViewer.InitialImage = null;
            this.picViewer.Location = new System.Drawing.Point(0, 0);
            this.picViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picViewer.Name = "picViewer";
            this.picViewer.Size = new System.Drawing.Size(737, 432);
            this.picViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picViewer.TabIndex = 9;
            this.picViewer.TabStop = false;
            this.picViewer.WaitOnLoad = true;
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfo.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(737, 432);
            this.lblInfo.TabIndex = 8;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.SystemColors.Info;
            this.pnlHeader.Controls.Add(this.pnlTags);
            this.pnlHeader.Controls.Add(this.btnTagPanelToggle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 24);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(737, 30);
            this.pnlHeader.TabIndex = 11;
            // 
            // pnlContent
            // 
            this.pnlContent.AutoScroll = true;
            this.pnlContent.Controls.Add(this.picLupe);
            this.pnlContent.Controls.Add(this.picViewer);
            this.pnlContent.Controls.Add(this.lblInfo);
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContent.Location = new System.Drawing.Point(0, 54);
            this.pnlContent.Name = "pnlContent";
            this.pnlContent.Size = new System.Drawing.Size(737, 432);
            this.pnlContent.TabIndex = 7;
            // 
            // ViewImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 558);
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.barStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ViewImageForm";
            this.Text = "ViewImageForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.ResizeEnd += new System.EventHandler(this.Form_ResizeEnd);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form_MouseWheel);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.Controls.SetChildIndex(this.barStatus, 0);
            this.Controls.SetChildIndex(this.pnlFooter, 0);
            this.Controls.SetChildIndex(this.pnlHeader, 0);
            this.Controls.SetChildIndex(this.pnlContent, 0);
            this.barStatus.ResumeLayout(false);
            this.barStatus.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLupe)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
            this.pnlHeader.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        /// <summary>ステータスバー本体</summary>
        protected System.Windows.Forms.StatusStrip barStatus;
        /// <summary>ステータスバー内表示用ラベル</summary>
        protected System.Windows.Forms.ToolStripStatusLabel lblStatus;
        /// <summary>表示画像削除ボタン</summary>
        protected SO.PictManager.Components.KeyPrevButton btnDelete;
        /// <summary>フォームクローズボタン</summary>
        protected SO.PictManager.Components.KeyPrevButton btnClose;
        /// <summary>サイズモード指定コンボボックス</summary>
        protected System.Windows.Forms.ComboBox cmbPicMode;
        /// <summary>ズームアウトボタン</summary>
        protected System.Windows.Forms.Button btnZoomOut;
        /// <summary>ズームインボタン</summary>
        protected System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Timer timLupe;
        private Components.KeyPrevCheckBox chkLupe;
        private System.Windows.Forms.ComboBox cmbLupeMagnification;
        private System.Windows.Forms.PictureBox picLupe;
        protected System.Windows.Forms.PictureBox picViewer;
        protected System.Windows.Forms.Label lblInfo;
        protected System.Windows.Forms.Button btnTagPanelToggle;
        protected System.Windows.Forms.FlowLayoutPanel pnlTags;
        protected System.Windows.Forms.Panel pnlFooter;
        protected System.Windows.Forms.Panel pnlHeader;
        protected System.Windows.Forms.Panel pnlContent;
    }
}