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
            this.grdImages = new System.Windows.Forms.DataGridView();
            this.pnlParent = new System.Windows.Forms.SplitContainer();
            this.lblFileCount = new System.Windows.Forms.Label();
            this.btnRevertAll = new SO.PictManager.Components.KeyPrevButton();
            this.btnDeleteSelection = new SO.PictManager.Components.KeyPrevButton();
            this.btnRevertSelection = new SO.PictManager.Components.KeyPrevButton();
            this.btnApplyChanges = new SO.PictManager.Components.KeyPrevButton();
            this.btnClose = new SO.PictManager.Components.KeyPrevButton();
            this.barStatus = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.grdImages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlParent)).BeginInit();
            this.pnlParent.Panel1.SuspendLayout();
            this.pnlParent.Panel2.SuspendLayout();
            this.pnlParent.SuspendLayout();
            this.barStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdImages
            // 
            this.grdImages.AllowUserToAddRows = false;
            this.grdImages.AllowUserToDeleteRows = false;
            this.grdImages.AllowUserToResizeRows = false;
            this.grdImages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdImages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdImages.Location = new System.Drawing.Point(0, 0);
            this.grdImages.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grdImages.MultiSelect = false;
            this.grdImages.Name = "grdImages";
            this.grdImages.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.grdImages.RowTemplate.Height = 21;
            this.grdImages.Size = new System.Drawing.Size(737, 459);
            this.grdImages.TabIndex = 0;
            this.grdImages.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdImages_CellClick);
            this.grdImages.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdImages_CellDoubleClick);
            this.grdImages.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdImages_CellEnter);
            this.grdImages.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.grdImages_CellValidating);
            this.grdImages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdImages_CellValueChanged);
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
            this.pnlParent.Panel1.Controls.Add(this.grdImages);
            // 
            // pnlParent.Panel2
            // 
            this.pnlParent.Panel2.Controls.Add(this.lblFileCount);
            this.pnlParent.Panel2.Controls.Add(this.btnRevertAll);
            this.pnlParent.Panel2.Controls.Add(this.btnDeleteSelection);
            this.pnlParent.Panel2.Controls.Add(this.btnRevertSelection);
            this.pnlParent.Panel2.Controls.Add(this.btnApplyChanges);
            this.pnlParent.Panel2.Controls.Add(this.btnClose);
            this.pnlParent.Panel2.Controls.Add(this.barStatus);
            this.pnlParent.Size = new System.Drawing.Size(737, 534);
            this.pnlParent.SplitterDistance = 459;
            this.pnlParent.SplitterWidth = 5;
            this.pnlParent.TabIndex = 2;
            // 
            // lblFileCount
            // 
            this.lblFileCount.AutoSize = true;
            this.lblFileCount.Location = new System.Drawing.Point(539, 12);
            this.lblFileCount.Name = "lblFileCount";
            this.lblFileCount.Size = new System.Drawing.Size(30, 15);
            this.lblFileCount.TabIndex = 5;
            this.lblFileCount.Text = "0 件";
            // 
            // btnRevertAll
            // 
            this.btnRevertAll.Enabled = false;
            this.btnRevertAll.Location = new System.Drawing.Point(421, 5);
            this.btnRevertAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRevertAll.Name = "btnRevertAll";
            this.btnRevertAll.Size = new System.Drawing.Size(112, 29);
            this.btnRevertAll.TabIndex = 2;
            this.btnRevertAll.Text = "全ての変更を取消";
            this.btnRevertAll.UseVisualStyleBackColor = true;
            this.btnRevertAll.Click += new System.EventHandler(this.btnRevertAll_Click);
            // 
            // btnDeleteSelection
            // 
            this.btnDeleteSelection.Enabled = false;
            this.btnDeleteSelection.Location = new System.Drawing.Point(128, 5);
            this.btnDeleteSelection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDeleteSelection.Name = "btnDeleteSelection";
            this.btnDeleteSelection.Size = new System.Drawing.Size(132, 29);
            this.btnDeleteSelection.TabIndex = 1;
            this.btnDeleteSelection.Text = "選択行の画像を削除";
            this.btnDeleteSelection.UseVisualStyleBackColor = true;
            this.btnDeleteSelection.Click += new System.EventHandler(this.btnDeleteSelection_Click);
            // 
            // btnRevertSelection
            // 
            this.btnRevertSelection.Enabled = false;
            this.btnRevertSelection.Location = new System.Drawing.Point(283, 5);
            this.btnRevertSelection.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnRevertSelection.Name = "btnRevertSelection";
            this.btnRevertSelection.Size = new System.Drawing.Size(132, 29);
            this.btnRevertSelection.TabIndex = 1;
            this.btnRevertSelection.Text = "選択行の変更を取消";
            this.btnRevertSelection.UseVisualStyleBackColor = true;
            this.btnRevertSelection.Click += new System.EventHandler(this.btnRevertSelection_Click);
            // 
            // btnApplyChanges
            // 
            this.btnApplyChanges.Enabled = false;
            this.btnApplyChanges.Location = new System.Drawing.Point(14, 5);
            this.btnApplyChanges.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnApplyChanges.Name = "btnApplyChanges";
            this.btnApplyChanges.Size = new System.Drawing.Size(87, 29);
            this.btnApplyChanges.TabIndex = 0;
            this.btnApplyChanges.Text = "変更を確定";
            this.btnApplyChanges.UseVisualStyleBackColor = true;
            this.btnApplyChanges.Click += new System.EventHandler(this.btnApplyChanges_Click);
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
            this.barStatus.Location = new System.Drawing.Point(0, 48);
            this.barStatus.Name = "barStatus";
            this.barStatus.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.barStatus.Size = new System.Drawing.Size(737, 22);
            this.barStatus.TabIndex = 4;
            this.barStatus.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(86, 17);
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
            this.Text = "PictManager - 画像一覧";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_FormClosing);
            this.Shown += new System.EventHandler(this.Form_Shown);
            this.Controls.SetChildIndex(this.pnlParent, 0);
            ((System.ComponentModel.ISupportInitialize)(this.grdImages)).EndInit();
            this.pnlParent.Panel1.ResumeLayout(false);
            this.pnlParent.Panel2.ResumeLayout(false);
            this.pnlParent.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlParent)).EndInit();
            this.pnlParent.ResumeLayout(false);
            this.barStatus.ResumeLayout(false);
            this.barStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grdImages;
		private System.Windows.Forms.SplitContainer pnlParent;
		private System.Windows.Forms.StatusStrip barStatus;
		private System.Windows.Forms.ToolStripStatusLabel lblStatus;
		private SO.PictManager.Components.KeyPrevButton btnApplyChanges;
		private SO.PictManager.Components.KeyPrevButton btnClose;
		private SO.PictManager.Components.KeyPrevButton btnRevertSelection;
        private SO.PictManager.Components.KeyPrevButton btnRevertAll;
        private System.Windows.Forms.Label lblFileCount;
        private Components.KeyPrevButton btnDeleteSelection;

    }
}