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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ListForm));
            this.grdFiles = new System.Windows.Forms.DataGridView();
            this.pnlParent = new System.Windows.Forms.SplitContainer();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.btnRevertAll = new SO.PictManager.Components.KeyPrevButton();
            this.btnRevertSelection = new SO.PictManager.Components.KeyPrevButton();
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
            this.grdFiles.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdFiles.MultiSelect = false;
            this.grdFiles.Name = "grdFiles";
            this.grdFiles.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdFiles.RowTemplate.Height = 21;
            this.grdFiles.Size = new System.Drawing.Size(737, 463);
            this.grdFiles.TabIndex = 0;
            this.grdFiles.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFiles_CellClick);
            this.grdFiles.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFiles_CellDoubleClick);
            this.grdFiles.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdFiles_CellValidating);
            this.grdFiles.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdFiles_CellValueChanged);
            // 
            // pnlParent
            // 
            this.pnlParent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlParent.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.pnlParent.IsSplitterFixed = true;
            this.pnlParent.Location = new System.Drawing.Point(0, 24);
            this.pnlParent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
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
            this.pnlParent.Panel2.Controls.Add(this.btnRevertAll);
            this.pnlParent.Panel2.Controls.Add(this.btnRevertSelection);
            this.pnlParent.Panel2.Controls.Add(this.btnApply);
            this.pnlParent.Panel2.Controls.Add(this.btnClose);
            this.pnlParent.Panel2.Controls.Add(this.barStatus);
            this.pnlParent.Size = new System.Drawing.Size(737, 534);
            this.pnlParent.SplitterDistance = 463;
            this.pnlParent.SplitterWidth = 5;
            this.pnlParent.TabIndex = 2;
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Location = new System.Drawing.Point(349, 12);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(59, 15);
            this.lblFileCount.TabIndex = 5;
            this.lblFileCount.Text = "999 files.";
            // 
            // btnRevertAll
            // 
            this.btnRevertAll.Enabled = false;
            this.btnRevertAll.Location = new System.Drawing.Point(231, 5);
            this.btnRevertAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRevertAll.Name = "btnRevertAll";
            this.btnRevertAll.Size = new System.Drawing.Size(112, 29);
            this.btnRevertAll.TabIndex = 2;
            this.btnRevertAll.Text = "全ての変更を取消";
            this.btnRevertAll.UseVisualStyleBackColor = true;
            this.btnRevertAll.Click += new System.EventHandler(this.btnRevertAll_Click);
            // 
            // btnRevertSelection
            // 
            this.btnRevertSelection.Enabled = false;
            this.btnRevertSelection.Location = new System.Drawing.Point(93, 5);
            this.btnRevertSelection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRevertSelection.Name = "btnRevertSelection";
            this.btnRevertSelection.Size = new System.Drawing.Size(132, 29);
            this.btnRevertSelection.TabIndex = 1;
            this.btnRevertSelection.Text = "選択行の変更を取消";
            this.btnRevertSelection.UseVisualStyleBackColor = true;
            this.btnRevertSelection.Click += new System.EventHandler(this.btnRevertSelection_Click);
            // 
            // btnApply
            // 
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(14, 5);
            this.btnApply.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(73, 29);
            this.btnApply.TabIndex = 0;
            this.btnApply.Text = "適用";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(649, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(73, 29);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "閉じる";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // barStatus
            // 
            this.barStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.barStatus.Location = new System.Drawing.Point(0, 43);
            this.barStatus.Name = "barStatus";
            this.barStatus.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.barStatus.Size = new System.Drawing.Size(737, 23);
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
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(737, 558);
            this.Controls.Add(this.pnlParent);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "ListForm";
            this.Text = "PictManager - ファイル一覧";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Shown += new System.EventHandler(this.Form_Shown);
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
		private SO.PictManager.Components.KeyPrevButton btnRevertSelection;
        private SO.PictManager.Components.KeyPrevButton btnRevertAll;
        private System.Windows.Forms.Label lblFileCount;

    }
}