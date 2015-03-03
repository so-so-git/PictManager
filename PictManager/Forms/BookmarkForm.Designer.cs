namespace SO.PictManager.Forms
{
    partial class BookmarkForm
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
            this.dgvBookmarks = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFileNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTimestamp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSortOrder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnJump = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.chkTopMost = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookmarks)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBookmarks
            // 
            this.dgvBookmarks.AllowUserToAddRows = false;
            this.dgvBookmarks.AllowUserToDeleteRows = false;
            this.dgvBookmarks.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBookmarks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookmarks.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colName,
            this.colFileNo,
            this.colTimestamp,
            this.colSortOrder});
            this.dgvBookmarks.Location = new System.Drawing.Point(12, 12);
            this.dgvBookmarks.Name = "dgvBookmarks";
            this.dgvBookmarks.RowTemplate.Height = 21;
            this.dgvBookmarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvBookmarks.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookmarks.Size = new System.Drawing.Size(456, 120);
            this.dgvBookmarks.TabIndex = 0;
            this.dgvBookmarks.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookmarks_CellDoubleClick);
            this.dgvBookmarks.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBookmarks_CellEndEdit);
            // 
            // colId
            // 
            this.colId.HeaderText = "ID";
            this.colId.Name = "colId";
            this.colId.Visible = false;
            // 
            // colName
            // 
            this.colName.HeaderText = "ブックマーク名";
            this.colName.Name = "colName";
            // 
            // colFileNo
            // 
            this.colFileNo.HeaderText = "ファイル番号";
            this.colFileNo.Name = "colFileNo";
            this.colFileNo.ReadOnly = true;
            // 
            // colTimestamp
            // 
            this.colTimestamp.HeaderText = "作成日付";
            this.colTimestamp.Name = "colTimestamp";
            this.colTimestamp.ReadOnly = true;
            // 
            // colSortOrder
            // 
            this.colSortOrder.HeaderText = "ソート順";
            this.colSortOrder.Name = "colSortOrder";
            this.colSortOrder.ReadOnly = true;
            // 
            // btnJump
            // 
            this.btnJump.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnJump.Location = new System.Drawing.Point(231, 145);
            this.btnJump.Name = "btnJump";
            this.btnJump.Size = new System.Drawing.Size(75, 23);
            this.btnJump.TabIndex = 1;
            this.btnJump.Text = "Jump";
            this.btnJump.UseVisualStyleBackColor = true;
            this.btnJump.Click += new System.EventHandler(this.btnJump_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.Location = new System.Drawing.Point(312, 145);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // chkTopMost
            // 
            this.chkTopMost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTopMost.AutoSize = true;
            this.chkTopMost.Location = new System.Drawing.Point(335, 174);
            this.chkTopMost.Name = "chkTopMost";
            this.chkTopMost.Size = new System.Drawing.Size(133, 16);
            this.chkTopMost.TabIndex = 4;
            this.chkTopMost.Text = "常に最前面に表示する";
            this.chkTopMost.UseVisualStyleBackColor = true;
            this.chkTopMost.CheckedChanged += new System.EventHandler(this.chkTopMost_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(393, 145);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // BookmarkForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 202);
            this.Controls.Add(this.chkTopMost);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnJump);
            this.Controls.Add(this.dgvBookmarks);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "BookmarkForm";
            this.Text = "ブックマーク";
            this.Load += new System.EventHandler(this.BookmarkForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookmarks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBookmarks;
        private System.Windows.Forms.Button btnJump;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFileNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTimestamp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSortOrder;
        private System.Windows.Forms.CheckBox chkTopMost;
        private System.Windows.Forms.Button btnClear;
    }
}