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
            this.picViewer = new System.Windows.Forms.PictureBox();
            this.pnlToolAreaSplit = new System.Windows.Forms.SplitContainer();
            this.picLupe = new System.Windows.Forms.PictureBox();
            this.lblInfo = new System.Windows.Forms.Label();
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
            this.pnlTagSplit = new System.Windows.Forms.SplitContainer();
            this.pnlTags = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTagPanelToggle = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlToolAreaSplit)).BeginInit();
            this.pnlToolAreaSplit.Panel1.SuspendLayout();
            this.pnlToolAreaSplit.Panel2.SuspendLayout();
            this.pnlToolAreaSplit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLupe)).BeginInit();
            this.barStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlTagSplit)).BeginInit();
            this.pnlTagSplit.Panel1.SuspendLayout();
            this.pnlTagSplit.Panel2.SuspendLayout();
            this.pnlTagSplit.SuspendLayout();
            this.SuspendLayout();
            // 
            // picViewer
            // 
            this.picViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picViewer.ErrorImage = null;
            this.picViewer.InitialImage = null;
            this.picViewer.Location = new System.Drawing.Point(0, 0);
            this.picViewer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picViewer.Name = "picViewer";
            this.picViewer.Size = new System.Drawing.Size(737, 378);
            this.picViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picViewer.TabIndex = 1;
            this.picViewer.TabStop = false;
            this.picViewer.WaitOnLoad = true;
            // 
            // pnlToolAreaSplit
            // 
            this.pnlToolAreaSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlToolAreaSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.pnlToolAreaSplit.IsSplitterFixed = true;
            this.pnlToolAreaSplit.Location = new System.Drawing.Point(0, 0);
            this.pnlToolAreaSplit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlToolAreaSplit.Name = "pnlToolAreaSplit";
            this.pnlToolAreaSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pnlToolAreaSplit.Panel1
            // 
            this.pnlToolAreaSplit.Panel1.AutoScroll = true;
            this.pnlToolAreaSplit.Panel1.Controls.Add(this.picLupe);
            this.pnlToolAreaSplit.Panel1.Controls.Add(this.picViewer);
            this.pnlToolAreaSplit.Panel1.Controls.Add(this.lblInfo);
            // 
            // pnlToolAreaSplit.Panel2
            // 
            this.pnlToolAreaSplit.Panel2.BackColor = System.Drawing.SystemColors.Control;
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.cmbLupeMagnification);
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.chkLupe);
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.btnZoomOut);
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.btnZoomIn);
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.cmbPicMode);
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.btnDelete);
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.btnClose);
            this.pnlToolAreaSplit.Panel2.Controls.Add(this.barStatus);
            this.pnlToolAreaSplit.Size = new System.Drawing.Size(737, 453);
            this.pnlToolAreaSplit.SplitterDistance = 378;
            this.pnlToolAreaSplit.SplitterWidth = 5;
            this.pnlToolAreaSplit.TabIndex = 0;
            this.pnlToolAreaSplit.TabStop = false;
            // 
            // picLupe
            // 
            this.picLupe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLupe.Location = new System.Drawing.Point(244, 94);
            this.picLupe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picLupe.Name = "picLupe";
            this.picLupe.Size = new System.Drawing.Size(200, 200);
            this.picLupe.TabIndex = 3;
            this.picLupe.TabStop = false;
            this.picLupe.Visible = false;
            // 
            // lblInfo
            // 
            this.lblInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblInfo.Font = new System.Drawing.Font("MS UI Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblInfo.Location = new System.Drawing.Point(0, 0);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(737, 378);
            this.lblInfo.TabIndex = 0;
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbLupeMagnification
            // 
            this.cmbLupeMagnification.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLupeMagnification.FormattingEnabled = true;
            this.cmbLupeMagnification.Location = new System.Drawing.Point(176, 12);
            this.cmbLupeMagnification.Name = "cmbLupeMagnification";
            this.cmbLupeMagnification.Size = new System.Drawing.Size(42, 23);
            this.cmbLupeMagnification.TabIndex = 2;
            this.cmbLupeMagnification.SelectedIndexChanged += new System.EventHandler(this.cmbLupeMagnification_SelectedIndexChanged);
            // 
            // chkLupe
            // 
            this.chkLupe.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLupe.Location = new System.Drawing.Point(97, 8);
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
            this.btnZoomOut.Location = new System.Drawing.Point(490, 8);
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
            this.btnZoomIn.Location = new System.Drawing.Point(453, 8);
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
            this.cmbPicMode.Location = new System.Drawing.Point(537, 12);
            this.cmbPicMode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbPicMode.Name = "cmbPicMode";
            this.cmbPicMode.Size = new System.Drawing.Size(98, 23);
            this.cmbPicMode.TabIndex = 5;
            this.cmbPicMode.SelectedIndexChanged += new System.EventHandler(this.cmbPicMode_SelectedIndexChanged);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(6, 8);
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
            this.btnClose.Location = new System.Drawing.Point(652, 8);
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
            this.barStatus.Location = new System.Drawing.Point(0, 48);
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
            // pnlTagSplit
            // 
            this.pnlTagSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTagSplit.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.pnlTagSplit.IsSplitterFixed = true;
            this.pnlTagSplit.Location = new System.Drawing.Point(0, 24);
            this.pnlTagSplit.Name = "pnlTagSplit";
            this.pnlTagSplit.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // pnlTagSplit.Panel1
            // 
            this.pnlTagSplit.Panel1.BackColor = System.Drawing.SystemColors.Info;
            this.pnlTagSplit.Panel1.Controls.Add(this.pnlTags);
            this.pnlTagSplit.Panel1.Controls.Add(this.btnTagPanelToggle);
            this.pnlTagSplit.Panel1MinSize = 30;
            // 
            // pnlTagSplit.Panel2
            // 
            this.pnlTagSplit.Panel2.Controls.Add(this.pnlToolAreaSplit);
            this.pnlTagSplit.Size = new System.Drawing.Size(737, 534);
            this.pnlTagSplit.SplitterDistance = 80;
            this.pnlTagSplit.SplitterWidth = 1;
            this.pnlTagSplit.TabIndex = 1;
            // 
            // pnlTags
            // 
            this.pnlTags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTags.Location = new System.Drawing.Point(0, 0);
            this.pnlTags.Name = "pnlTags";
            this.pnlTags.Size = new System.Drawing.Size(697, 80);
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
            this.btnTagPanelToggle.Size = new System.Drawing.Size(40, 80);
            this.btnTagPanelToggle.TabIndex = 0;
            this.btnTagPanelToggle.UseVisualStyleBackColor = false;
            this.btnTagPanelToggle.Visible = false;
            this.btnTagPanelToggle.Click += new System.EventHandler(this.btnTagPanelToggle_Click);
            // 
            // ViewImageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 558);
            this.Controls.Add(this.pnlTagSplit);
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
            this.Controls.SetChildIndex(this.pnlTagSplit, 0);
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
            this.pnlToolAreaSplit.Panel1.ResumeLayout(false);
            this.pnlToolAreaSplit.Panel2.ResumeLayout(false);
            this.pnlToolAreaSplit.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlToolAreaSplit)).EndInit();
            this.pnlToolAreaSplit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLupe)).EndInit();
            this.barStatus.ResumeLayout(false);
            this.barStatus.PerformLayout();
            this.pnlTagSplit.Panel1.ResumeLayout(false);
            this.pnlTagSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlTagSplit)).EndInit();
            this.pnlTagSplit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        /// <summary>指定画像表示用PictureBox</summary>
        protected System.Windows.Forms.PictureBox picViewer;
        /// <summary>画像表示部とコントロール部の分割を行なう為のSplitCotainer</summary>
        protected System.Windows.Forms.SplitContainer pnlToolAreaSplit;
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
        /// <summary>情報通知ラベル</summary>
        protected System.Windows.Forms.Label lblInfo;
        /// <summary>ズームアウトボタン</summary>
        protected System.Windows.Forms.Button btnZoomOut;
        /// <summary>ズームインボタン</summary>
        protected System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.PictureBox picLupe;
        private System.Windows.Forms.Timer timLupe;
        private Components.KeyPrevCheckBox chkLupe;
        private System.Windows.Forms.ComboBox cmbLupeMagnification;
        private System.Windows.Forms.Button btnTagPanelToggle;
        protected System.Windows.Forms.SplitContainer pnlTagSplit;
        private System.Windows.Forms.FlowLayoutPanel pnlTags;
    }
}