namespace SO.PictManager.Forms
{
    partial class ThumbnailForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ThumbnailForm));
            this.barStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.pnlParent = new System.Windows.Forms.SplitContainer();
            this.pnlThumbnail = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPrevious = new System.Windows.Forms.Button();
            this.lblPageMax = new System.Windows.Forms.Label();
            this.lblPageSlash = new System.Windows.Forms.Label();
            this.txtPage = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.barStatus.SuspendLayout();
            this.pnlParent.Panel1.SuspendLayout();
            this.pnlParent.Panel2.SuspendLayout();
            this.pnlParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // barStatus
            // 
            this.barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.barStatus.Location = new System.Drawing.Point(0, 424);
            this.barStatus.Name = "barStatus";
            this.barStatus.Size = new System.Drawing.Size(632, 22);
            this.barStatus.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(82, 17);
            this.lblStatus.Text = "[ステータス表示]";
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
            this.pnlParent.Panel1.Controls.Add(this.pnlThumbnail);
            // 
            // pnlParent.Panel2
            // 
            this.pnlParent.Panel2.Controls.Add(this.btnNext);
            this.pnlParent.Panel2.Controls.Add(this.btnDelete);
            this.pnlParent.Panel2.Controls.Add(this.btnPrevious);
            this.pnlParent.Panel2.Controls.Add(this.lblPageMax);
            this.pnlParent.Panel2.Controls.Add(this.lblPageSlash);
            this.pnlParent.Panel2.Controls.Add(this.txtPage);
            this.pnlParent.Panel2.Controls.Add(this.btnClose);
            this.pnlParent.Size = new System.Drawing.Size(632, 400);
            this.pnlParent.SplitterDistance = 361;
            this.pnlParent.TabIndex = 1;
            this.pnlParent.TabStop = false;
            // 
            // pnlThumbnail
            // 
            this.pnlThumbnail.AutoScroll = true;
            this.pnlThumbnail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlThumbnail.Location = new System.Drawing.Point(0, 0);
            this.pnlThumbnail.Name = "pnlThumbnail";
            this.pnlThumbnail.Size = new System.Drawing.Size(632, 361);
            this.pnlThumbnail.TabIndex = 0;
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(358, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(35, 23);
            this.btnNext.TabIndex = 5;
            this.btnNext.Text = ">>";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(8, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(64, 23);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(242, 4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(35, 23);
            this.btnPrevious.TabIndex = 3;
            this.btnPrevious.Text = "<<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // lblPageMax
            // 
            this.lblPageMax.AutoSize = true;
            this.lblPageMax.Location = new System.Drawing.Point(326, 10);
            this.lblPageMax.Name = "lblPageMax";
            this.lblPageMax.Size = new System.Drawing.Size(23, 12);
            this.lblPageMax.TabIndex = 3;
            this.lblPageMax.Text = "999";
            // 
            // lblPageSlash
            // 
            this.lblPageSlash.AutoSize = true;
            this.lblPageSlash.Location = new System.Drawing.Point(314, 10);
            this.lblPageSlash.Name = "lblPageSlash";
            this.lblPageSlash.Size = new System.Drawing.Size(11, 12);
            this.lblPageSlash.TabIndex = 2;
            this.lblPageSlash.Text = "/";
            // 
            // txtPage
            // 
            this.txtPage.Location = new System.Drawing.Point(286, 7);
            this.txtPage.Name = "txtPage";
            this.txtPage.Size = new System.Drawing.Size(28, 19);
            this.txtPage.TabIndex = 4;
            this.txtPage.Text = "999";
            this.txtPage.TextChanged += new System.EventHandler(this.txtPage_TextChanged);
            this.txtPage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtPage_KeyDown);
            this.txtPage.Leave += new System.EventHandler(this.txtPage_Leave);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(548, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ThumbnailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.Controls.Add(this.pnlParent);
            this.Controls.Add(this.barStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ThumbnailForm";
            this.Text = "PictManager - サムネイル";
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.Form_MouseWheel);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Resize += new System.EventHandler(this.Form_Resize);
            this.Controls.SetChildIndex(this.barStatus, 0);
            this.Controls.SetChildIndex(this.pnlParent, 0);
            this.barStatus.ResumeLayout(false);
            this.barStatus.PerformLayout();
            this.pnlParent.Panel1.ResumeLayout(false);
            this.pnlParent.Panel2.ResumeLayout(false);
            this.pnlParent.Panel2.PerformLayout();
            this.pnlParent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip barStatus;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.SplitContainer pnlParent;
        private System.Windows.Forms.FlowLayoutPanel pnlThumbnail;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnPrevious;
        private System.Windows.Forms.Label lblPageMax;
        private System.Windows.Forms.Label lblPageSlash;
        private System.Windows.Forms.TextBox txtPage;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnDelete;
    }
}