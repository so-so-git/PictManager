namespace SO.PictManager.Forms.FileSystem
{
    public partial class FileSlideForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FileSlideForm));
            this.btnPrevious = new SO.PictManager.Components.KeyPrevButton();
            this.btnNext = new SO.PictManager.Components.KeyPrevButton();
            this.lblCount = new System.Windows.Forms.Label();
            this.lblCountDelim = new System.Windows.Forms.Label();
            this.txtIndex = new System.Windows.Forms.TextBox();
            this.chkSimilar = new SO.PictManager.Components.KeyPrevCheckBox();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.btnBookmark = new SO.PictManager.Components.KeyPrevButton();
            this.chkLupe = new SO.PictManager.Components.KeyPrevCheckBox();
            this.picLupe = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).BeginInit();
            this.pnlParent.Panel1.SuspendLayout();
            this.pnlParent.Panel2.SuspendLayout();
            this.pnlParent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLupe)).BeginInit();
            this.SuspendLayout();
            // 
            // picViewer
            // 
            this.picViewer.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.picViewer.Size = new System.Drawing.Size(737, 445);
            // 
            // pnlParent
            // 
            this.pnlParent.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            // 
            // pnlParent.Panel1
            // 
            this.pnlParent.Panel1.Controls.Add(this.picLupe);
            // 
            // pnlParent.Panel2
            // 
            this.pnlParent.Panel2.AutoScroll = true;
            this.pnlParent.Panel2.Controls.Add(this.chkSimilar);
            this.pnlParent.Panel2.Controls.Add(this.cmbSort);
            this.pnlParent.Panel2.Controls.Add(this.btnBookmark);
            this.pnlParent.Panel2.Controls.Add(this.btnNext);
            this.pnlParent.Panel2.Controls.Add(this.txtIndex);
            this.pnlParent.Panel2.Controls.Add(this.lblCount);
            this.pnlParent.Panel2.Controls.Add(this.chkLupe);
            this.pnlParent.Panel2.Controls.Add(this.btnPrevious);
            this.pnlParent.Panel2.Controls.Add(this.lblCountDelim);
            this.pnlParent.Size = new System.Drawing.Size(1134, 472);
            this.pnlParent.SplitterDistance = 383;
            this.pnlParent.SplitterWidth = 6;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(1049, 8);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClose.TabIndex = 13;
            // 
            // cmbPicMode
            // 
            this.cmbPicMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbPicMode.Location = new System.Drawing.Point(945, 13);
            this.cmbPicMode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cmbPicMode.Size = new System.Drawing.Size(98, 23);
            this.cmbPicMode.TabIndex = 12;
            // 
            // lblInfo
            // 
            this.lblInfo.Size = new System.Drawing.Size(1118, 445);
            this.lblInfo.TabIndex = 0;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomOut.Location = new System.Drawing.Point(737, 8);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnZoomOut.TabIndex = 10;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomIn.Location = new System.Drawing.Point(700, 8);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnZoomIn.TabIndex = 9;
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(476, 8);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.Size = new System.Drawing.Size(29, 29);
            this.btnPrevious.TabIndex = 4;
            this.btnPrevious.Text = "<";
            this.btnPrevious.UseVisualStyleBackColor = true;
            this.btnPrevious.Click += new System.EventHandler(this.btnPrevious_Click);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(618, 8);
            this.btnNext.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(29, 29);
            this.btnNext.TabIndex = 8;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblCount
            // 
            this.lblCount.Location = new System.Drawing.Point(571, 12);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(41, 20);
            this.lblCount.TabIndex = 7;
            this.lblCount.Text = "9999";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCountDelim
            // 
            this.lblCountDelim.Location = new System.Drawing.Point(555, 11);
            this.lblCountDelim.Name = "lblCountDelim";
            this.lblCountDelim.Size = new System.Drawing.Size(13, 20);
            this.lblCountDelim.TabIndex = 6;
            this.lblCountDelim.Text = "/";
            this.lblCountDelim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtIndex
            // 
            this.txtIndex.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtIndex.Location = new System.Drawing.Point(511, 12);
            this.txtIndex.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(40, 23);
            this.txtIndex.TabIndex = 5;
            this.txtIndex.Text = "9999";
            this.txtIndex.TextChanged += new System.EventHandler(this.txtIndex_TextChanged);
            this.txtIndex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIndex_KeyDown);
            // 
            // chkSimilar
            // 
            this.chkSimilar.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSimilar.Location = new System.Drawing.Point(183, 8);
            this.chkSimilar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkSimilar.Name = "chkSimilar";
            this.chkSimilar.Size = new System.Drawing.Size(73, 29);
            this.chkSimilar.TabIndex = 2;
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
            this.cmbSort.TabIndex = 11;
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbSort_SelectedIndexChanged);
            // 
            // btnBookmark
            // 
            this.btnBookmark.Location = new System.Drawing.Point(94, 8);
            this.btnBookmark.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnBookmark.Name = "btnBookmark";
            this.btnBookmark.Size = new System.Drawing.Size(82, 29);
            this.btnBookmark.TabIndex = 1;
            this.btnBookmark.Text = "ブックマーク";
            this.btnBookmark.UseVisualStyleBackColor = true;
            this.btnBookmark.Click += new System.EventHandler(this.btnBookmark_Clicked);
            // 
            // chkLupe
            // 
            this.chkLupe.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLupe.Location = new System.Drawing.Point(264, 8);
            this.chkLupe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkLupe.Name = "chkLupe";
            this.chkLupe.Size = new System.Drawing.Size(73, 29);
            this.chkLupe.TabIndex = 3;
            this.chkLupe.Text = "部分拡大";
            this.chkLupe.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkLupe.UseVisualStyleBackColor = true;
            this.chkLupe.CheckedChanged += new System.EventHandler(this.chkLupe_CheckedChanged);
            // 
            // picLupe
            // 
            this.picLupe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picLupe.Location = new System.Drawing.Point(31, 31);
            this.picLupe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.picLupe.Name = "picLupe";
            this.picLupe.Size = new System.Drawing.Size(233, 250);
            this.picLupe.TabIndex = 2;
            this.picLupe.TabStop = false;
            this.picLupe.Visible = false;
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
            this.pnlParent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picLupe)).EndInit();
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
        private SO.PictManager.Components.KeyPrevCheckBox chkLupe;
        private System.Windows.Forms.PictureBox picLupe;

    }
}