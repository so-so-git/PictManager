namespace SO.PictManager.Forms
{
    public partial class SlideForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlideForm));
            this.btnPrevious = new SO.PictManager.Components.KeyPrevButton();
            this.btnNext = new SO.PictManager.Components.KeyPrevButton();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblCountDelim = new System.Windows.Forms.Label();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.chkSimilar = new SO.PictManager.Components.KeyPrevCheckBox();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.btnBookmark = new SO.PictManager.Components.KeyPrevButton();
            this.btnGroup = new SO.PictManager.Components.KeyPrevButton();
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pnlParent)).BeginInit();
            this.pnlParent.Panel1.SuspendLayout();
            this.pnlParent.Panel2.SuspendLayout();
            this.pnlParent.SuspendLayout();
            this.SuspendLayout();
            // 
            // picViewer
            // 
            this.picViewer.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.picViewer.Size = new System.Drawing.Size(737, 432);
            // 
            // pnlParent
            // 
            this.pnlParent.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            // 
            // pnlParent.Panel2
            // 
            this.pnlParent.Panel2.AutoScroll = true;
            this.pnlParent.Panel2.Controls.Add(this.chkSimilar);
            this.pnlParent.Panel2.Controls.Add(this.cmbSort);
            this.pnlParent.Panel2.Controls.Add(this.btnGroup);
            this.pnlParent.Panel2.Controls.Add(this.btnBookmark);
            this.pnlParent.Panel2.Controls.Add(this.btnNext);
            this.pnlParent.Panel2.Controls.Add(this.txtIndex);
            this.pnlParent.Panel2.Controls.Add(this.lblCount);
            this.pnlParent.Panel2.Controls.Add(this.btnPrevious);
            this.pnlParent.Panel2.Controls.Add(this.lblCountDelim);
            this.pnlParent.Size = new System.Drawing.Size(1134, 470);
            this.pnlParent.SplitterDistance = 370;
            this.pnlParent.SplitterWidth = 6;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(1049, 8);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClose.TabIndex = 15;
            // 
            // cmbPicMode
            // 
            this.cmbPicMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbPicMode.Location = new System.Drawing.Point(945, 13);
            this.cmbPicMode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cmbPicMode.Size = new System.Drawing.Size(98, 23);
            this.cmbPicMode.TabIndex = 14;
            // 
            // lblInfo
            // 
            this.lblInfo.Size = new System.Drawing.Size(1117, 432);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomOut.Location = new System.Drawing.Point(737, 8);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnZoomOut.TabIndex = 12;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomIn.Location = new System.Drawing.Point(700, 8);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnZoomIn.TabIndex = 11;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(515, 8);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(29, 29);
            this.btnPrevious.TabIndex = 6;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(657, 8);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(29, 29);
            this.btnNext.TabIndex = 10;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(610, 12);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(41, 20);
            this.lblCount.TabIndex = 9;
            this.lblCount.Text = "9999";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCountDelim
            // 
            this.lblCountDelim.Location = new System.Drawing.Point(594, 11);
            this.lblCountDelim.Name = "lblCountDelim";
            this.lblCountDelim.Size = new System.Drawing.Size(13, 20);
            this.lblCountDelim.TabIndex = 8;
            this.lblCountDelim.Text = "/";
            this.lblCountDelim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtIndex
            // 
            this.txtIndex.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtIndex.Location = new System.Drawing.Point(550, 12);
            this.txtIndex.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(40, 23);
            this.txtIndex.TabIndex = 7;
            this.txtIndex.Text = "9999";
            this.txtIndex.TextChanged += new System.EventHandler(this.txtIndex_TextChanged);
            this.txtIndex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIndex_KeyDown);
            // 
            // chkSimilar
            // 
            this.chkSimilar.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSimilar.Location = new System.Drawing.Point(312, 8);
            this.chkSimilar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkSimilar.Name = "chkSimilar";
            this.chkSimilar.Size = new System.Drawing.Size(73, 29);
            this.chkSimilar.TabIndex = 4;
            this.chkSimilar.Text = "類似検索";
            this.chkSimilar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSimilar.UseVisualStyleBackColor = true;
            this.chkSimilar.CheckedChanged += new System.EventHandler(this.chkSimilar_CheckedChanged);
            // 
            // cmbSort
            // 
            this.cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Location = new System.Drawing.Point(774, 12);
            this.cmbSort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(165, 23);
            this.cmbSort.TabIndex = 13;
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbSort_SelectedIndexChanged);
            // 
            // btnBookmark
            // 
            this.btnBookmark.Location = new System.Drawing.Point(224, 8);
            this.btnBookmark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBookmark.Name = "btnBookmark";
            this.btnBookmark.Size = new System.Drawing.Size(82, 29);
            this.btnBookmark.TabIndex = 3;
            this.btnBookmark.Text = "ブックマーク";
            this.btnBookmark.UseVisualStyleBackColor = true;
            this.btnBookmark.Click += new System.EventHandler(this.btnBookmark_Clicked);
            // 
            // btnGroup
            // 
            this.btnGroup.Location = new System.Drawing.Point(391, 8);
            this.btnGroup.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGroup.Name = "btnGroup";
            this.btnGroup.Size = new System.Drawing.Size(91, 29);
            this.btnGroup.TabIndex = 5;
            this.btnGroup.Text = "グループ登録";
            this.btnGroup.UseVisualStyleBackColor = true;
            this.btnGroup.Click += new System.EventHandler(this.btnGroup_Click);
            // 
            // SlideForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 496);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.Name = "SlideForm";
            this.Text = "PictManager - スライドショー";
            this.Resize += new System.EventHandler(this.Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
            this.pnlParent.Panel1.ResumeLayout(false);
            this.pnlParent.Panel2.ResumeLayout(false);
            this.pnlParent.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlParent)).EndInit();
            this.pnlParent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private SO.PictManager.Components.KeyPrevButton btnPrevious;
		private SO.PictManager.Components.KeyPrevButton btnNext;
		private System.Windows.Forms.Label lblCountDelim;
		private System.Windows.Forms.TextBox txtIndex;
        private System.Windows.Forms.Label lblCount;
        private SO.PictManager.Components.KeyPrevCheckBox chkSimilar;
        private System.Windows.Forms.ComboBox cmbSort;
        private SO.PictManager.Components.KeyPrevButton btnBookmark;
        private Components.KeyPrevButton btnGroup;

    }
}