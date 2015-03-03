namespace SO.PictManager.Forms
{
    partial class ListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListForm));
            this.grdFiles = new System.Windows.Forms.DataGridView();
            this.pnlParent = new System.Windows.Forms.SplitContainer();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.btnRollback = new SO.PictManager.Components.KeyPrevButton();
            this.btnRevert = new SO.PictManager.Components.KeyPrevButton();
            this.btnApply = new SO.PictManager.Components.KeyPrevButton();
            this.btnClose = new SO.PictManager.Components.KeyPrevButton();
            this.barStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdFiles)).BeginInit();
            this.pnlParent.Panel1.SuspendLayout();
            this.pnlParent.Panel2.SuspendLayout();
            this.pnlParent.SuspendLayout();
            this.barStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdFiles
            // 
            this.grdFiles.AllowUserToAddRows = false;
            this.grdFiles.AllowUserToDeleteRows = false;
            this.grdFiles.AllowUserToResizeRows = false;
            this.grdFiles.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdFiles.Location = new System.Drawing.Point(0, 0);
            this.grdFiles.MultiSelect = false;
            this.grdFiles.Name = "grdFiles";
            this.grdFiles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdFiles.RowTemplate.Height = 21;
            this.grdFiles.Size = new System.Drawing.Size(632, 363);
            this.grdFiles.TabIndex = 0;
            this.grdFiles.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFiles_CellValueChanged);
            this.grdFiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFiles_CellDoubleClick);
            this.grdFiles.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdFiles_CellValidating);
            this.grdFiles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFiles_CellClick);
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
            this.pnlParent.Panel1.Controls.Add(this.grdFiles);
            // 
            // pnlParent.Panel2
            // 
            this.pnlParent.Panel2.Controls.Add(this.lblFileCount);
            this.pnlParent.Panel2.Controls.Add(this.btnRollback);
            this.pnlParent.Panel2.Controls.Add(this.btnRevert);
            this.pnlParent.Panel2.Controls.Add(this.btnApply);
            this.pnlParent.Panel2.Controls.Add(this.btnClose);
            this.pnlParent.Panel2.Controls.Add(this.barStatus);
            this.pnlParent.Size = new System.Drawing.Size(632, 422);
            this.pnlParent.SplitterDistance = 363;
            this.pnlParent.TabIndex = 2;
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Location = new System.Drawing.Point(234, 9);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(51, 12);
            this.lblFileCount.TabIndex = 5;
            this.lblFileCount.Text = "999 files.";
            // 
            // btnRollback
            // 
            this.btnRollback.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRollback.Enabled = false;
            this.btnRollback.Location = new System.Drawing.Point(153, 4);
            this.btnRollback.Name = "btnRollback";
            this.btnRollback.Size = new System.Drawing.Size(63, 23);
            this.btnRollback.TabIndex = 2;
            this.btnRollback.Text = "Rollback";
            this.btnRollback.UseVisualStyleBackColor = true;
            this.btnRollback.Click += new System.EventHandler(this.btnRollback_Click);
            // 
            // btnRevert
            // 
            this.btnRevert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRevert.Enabled = false;
            this.btnRevert.Location = new System.Drawing.Point(84, 4);
            this.btnRevert.Name = "btnRevert";
            this.btnRevert.Size = new System.Drawing.Size(63, 23);
            this.btnRevert.TabIndex = 1;
            this.btnRevert.Text = "Revert";
            this.btnRevert.UseVisualStyleBackColor = true;
            this.btnRevert.Click += new System.EventHandler(this.btnRevert_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(12, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(63, 23);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(556, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 23);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // barStatus
            // 
            this.barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.barStatus.Location = new System.Drawing.Point(0, 32);
            this.barStatus.Name = "barStatus";
            this.barStatus.Size = new System.Drawing.Size(632, 23);
            this.barStatus.TabIndex = 4;
            this.barStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(102, 18);
            this.lblStatus.Text = "[ステータス表示]";
            // 
            // ListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(632, 446);
            this.Controls.Add(this.pnlParent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "ListForm";
            this.Text = "PictManager - ファイル一覧";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Controls.SetChildIndex(this.pnlParent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grdFiles)).EndInit();
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

        private System.Windows.Forms.DataGridView grdFiles;
		private System.Windows.Forms.SplitContainer pnlParent;
		private System.Windows.Forms.StatusStrip barStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private SO.PictManager.Components.KeyPrevButton btnApply;
		private SO.PictManager.Components.KeyPrevButton btnClose;
		private SO.PictManager.Components.KeyPrevButton btnRevert;
        private SO.PictManager.Components.KeyPrevButton btnRollback;
        private System.Windows.Forms.Label lblFileCount;

    }
}