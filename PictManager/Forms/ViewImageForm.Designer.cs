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
        /// <param orderName="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewImageForm));
            this.picViewer = new System.Windows.Forms.PictureBox();
            this.pnlParent = new System.Windows.Forms.SplitContainer();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.cmbPicMode = new System.Windows.Forms.ComboBox();
            this.btnDelete = new SO.PictManager.Components.KeyPrevButton();
            this.btnClose = new SO.PictManager.Components.KeyPrevButton();
            this.barStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
            this.pnlParent.Panel1.SuspendLayout();
            this.pnlParent.Panel2.SuspendLayout();
            this.pnlParent.SuspendLayout();
            this.barStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // picViewer
            // 
            this.picViewer.ErrorImage = null;
            this.picViewer.InitialImage = null;
            this.picViewer.Location = new System.Drawing.Point(0, 0);
            this.picViewer.Name = "picViewer";
            this.picViewer.Size = new System.Drawing.Size(632, 364);
            this.picViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picViewer.TabIndex = 1;
            this.picViewer.TabStop = false;
            this.picViewer.WaitOnLoad = true;
            // 
            // pnlParent
            // 
            this.pnlParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.pnlParent.IsSplitterFixed = true;
            this.pnlParent.Location = new System.Drawing.Point(0, 24);
            this.pnlParent.Name = "pnlParent";
            this.pnlParent.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pnlParent.Panel1
            // 
            this.pnlParent.Panel1.AutoScroll = true;
            this.pnlParent.Panel1.Controls.Add(this.picViewer);
            this.pnlParent.Panel1.Controls.Add(this.lblInfo);
            // 
            // pnlParent.Panel2
            // 
            this.pnlParent.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.pnlParent.Panel2.Controls.Add(this.btnZoomOut);
            this.pnlParent.Panel2.Controls.Add(this.btnZoomIn);
            this.pnlParent.Panel2.Controls.Add(this.cmbPicMode);
            this.pnlParent.Panel2.Controls.Add(this.btnDelete);
            this.pnlParent.Panel2.Controls.Add(this.btnClose);
            this.pnlParent.Panel2.Controls.Add(this.barStatus);
            this.pnlParent.Size = new System.Drawing.Size(632, 422);
            this.pnlParent.SplitterDistance = 364;
            this.pnlParent.TabIndex = 0;
            this.pnlParent.TabStop = false;
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfo.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(632, 364);
            this.lblInfo.TabIndex = 2;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomOut.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomOut.Image")));
            this.btnZoomOut.Location = new System.Drawing.Point(420, 4);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(27, 23);
            this.btnZoomOut.TabIndex = 3;
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomIn.Image = ((System.Drawing.Image)(resources.GetObject("btnZoomIn.Image")));
            this.btnZoomIn.Location = new System.Drawing.Point(388, 4);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(27, 23);
            this.btnZoomIn.TabIndex = 3;
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // cmbPicMode
            // 
            this.cmbPicMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPicMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPicMode.FormattingEnabled = true;
            this.cmbPicMode.Location = new System.Drawing.Point(460, 4);
            this.cmbPicMode.Name = "cmbPicMode";
            this.cmbPicMode.Size = new System.Drawing.Size(85, 20);
            this.cmbPicMode.TabIndex = 1;
            this.cmbPicMode.SelectedIndexChanged += new System.EventHandler(this.cmbPicMode_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(12, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(63, 23);
            this.btnDelete.TabIndex = 0;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(556, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // barStatus
            // 
            this.barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.barStatus.Location = new System.Drawing.Point(0, 31);
            this.barStatus.Name = "barStatus";
            this.barStatus.Size = new System.Drawing.Size(632, 23);
            this.barStatus.TabIndex = 0;
            this.barStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(102, 18);
            this.lblStatus.Text = "[ステータス表示]";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ViewImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.Controls.Add(this.pnlParent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ViewImageForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form_MouseWheel);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.ResizeEnd += new System.EventHandler(this.Form_ResizeEnd);
            this.Controls.SetChildIndex(this.pnlParent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
            this.pnlParent.Panel1.ResumeLayout(false);
            this.pnlParent.Panel2.ResumeLayout(false);
            this.pnlParent.Panel2.PerformLayout();
            this.pnlParent.ResumeLayout(false);
            this.barStatus.ResumeLayout(false);
            this.barStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>指定画像表示用PictureBox</summary>
        protected System.Windows.Forms.PictureBox picViewer;
        /// <summary>画像表示部とコントロール部の分割を行なう為のSplitCotainer</summary>
        protected System.Windows.Forms.SplitContainer pnlParent;
        /// <summary>ステータスバー本体</summary>
        protected System.Windows.Forms.StatusStrip barStatus;
        /// <summary>ステータスバー内表示用ラベル</summary>
        protected System.Windows.Forms.ToolStripStatusLabel lblStatus;
        /// <summary>表示画像ファイル削除ボタン</summary>
        protected SO.PictManager.Components.KeyPrevButton btnDelete;
        /// <summary>フォームクローズボタン</summary>
        protected SO.PictManager.Components.KeyPrevButton btnClose;
        /// <summary>サイズモード指定コンボボックス</summary>
        protected System.Windows.Forms.ComboBox cmbPicMode;
        /// <summary>情報通知ラベル</summary>
        protected System.Windows.Forms.Label lblInfo;
        /// <summary>ズームアウトボタン</summary>
        protected System.Windows.Forms.Button btnZoomOut;
        /// <summary>ズームインボタン</summary>
        protected System.Windows.Forms.Button btnZoomIn;
    }
}