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
            this.pnlFooter.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClose.Location = new System.Drawing.Point(1049, 11);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClose.TabIndex = 15;
            // 
            // cmbPicMode
            // 
            this.cmbPicMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbPicMode.Location = new System.Drawing.Point(945, 15);
            this.cmbPicMode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.cmbPicMode.Size = new System.Drawing.Size(98, 23);
            this.cmbPicMode.TabIndex = 14;
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomOut.Location = new System.Drawing.Point(730, 11);
            this.btnZoomOut.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnZoomOut.TabIndex = 12;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZoomIn.Location = new System.Drawing.Point(693, 11);
            this.btnZoomIn.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnZoomIn.TabIndex = 11;
            // 
            // picViewer
            // 
            this.picViewer.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.picViewer.Size = new System.Drawing.Size(1134, 370);
            // 
            // btnTagPanelToggle
            // 
            this.btnTagPanelToggle.FlatAppearance.BorderColor = System.Drawing.SystemColors.Info;
            this.btnTagPanelToggle.Location = new System.Drawing.Point(1094, 0);
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.chkSimilar);
            this.pnlFooter.Controls.Add(this.cmbSort);
            this.pnlFooter.Controls.Add(this.btnGroup);
            this.pnlFooter.Controls.Add(this.btnBookmark);
            this.pnlFooter.Controls.Add(this.btnNext);
            this.pnlFooter.Controls.Add(this.txtIndex);
            this.pnlFooter.Controls.Add(this.lblCount);
            this.pnlFooter.Controls.Add(this.btnPrevious);
            this.pnlFooter.Controls.Add(this.lblCountDelim);
            this.pnlFooter.Location = new System.Drawing.Point(0, 424);
            this.pnlFooter.Size = new System.Drawing.Size(1134, 50);
            this.pnlFooter.Controls.SetChildIndex(this.lblCountDelim, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnPrevious, 0);
            this.pnlFooter.Controls.SetChildIndex(this.lblCount, 0);
            this.pnlFooter.Controls.SetChildIndex(this.txtIndex, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnNext, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnBookmark, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnGroup, 0);
            this.pnlFooter.Controls.SetChildIndex(this.cmbSort, 0);
            this.pnlFooter.Controls.SetChildIndex(this.chkSimilar, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnClose, 0);
            this.pnlFooter.Controls.SetChildIndex(this.cmbPicMode, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnDelete, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnZoomIn, 0);
            this.pnlFooter.Controls.SetChildIndex(this.btnZoomOut, 0);
            // 
            // pnlHeader
            // 
            this.pnlHeader.Size = new System.Drawing.Size(1134, 30);
            // 
            // pnlContent
            // 
            this.pnlContent.Size = new System.Drawing.Size(1134, 370);
            // 
            // btnPrevious
            // 
            this.btnPrevious.Location = new System.Drawing.Point(516, 11);
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
            this.btnNext.Location = new System.Drawing.Point(658, 11);
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
            this.lblCount.Location = new System.Drawing.Point(611, 15);
            this.lblCount.Name = "lblCount";
            this.lblCount.Size = new System.Drawing.Size(41, 20);
            this.lblCount.TabIndex = 9;
            this.lblCount.Text = "9999";
            this.lblCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCountDelim
            // 
            this.lblCountDelim.Location = new System.Drawing.Point(597, 15);
            this.lblCountDelim.Name = "lblCountDelim";
            this.lblCountDelim.Size = new System.Drawing.Size(13, 20);
            this.lblCountDelim.TabIndex = 8;
            this.lblCountDelim.Text = "/";
            this.lblCountDelim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtIndex
            // 
            this.txtIndex.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.txtIndex.Location = new System.Drawing.Point(551, 15);
            this.txtIndex.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtIndex.Name = "txtIndex";
            this.txtIndex.Size = new System.Drawing.Size(40, 23);
            this.txtIndex.TabIndex = 7;
            this.txtIndex.Text = "9999";
            this.txtIndex.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtIndex.TextChanged += new System.EventHandler(this.txtIndex_TextChanged);
            this.txtIndex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtIndex_KeyDown);
            // 
            // chkSimilar
            // 
            this.chkSimilar.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSimilar.Location = new System.Drawing.Point(312, 11);
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
            this.cmbSort.Location = new System.Drawing.Point(774, 15);
            this.cmbSort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(165, 23);
            this.cmbSort.TabIndex = 13;
            this.cmbSort.SelectedIndexChanged += new System.EventHandler(this.cmbSort_SelectedIndexChanged);
            // 
            // btnBookmark
            // 
            this.btnBookmark.Location = new System.Drawing.Point(224, 11);
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
            this.btnGroup.Location = new System.Drawing.Point(391, 11);
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
            this.Resize += new System.EventHandler(this.Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picViewer)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.pnlFooter.PerformLayout();
            this.pnlHeader.ResumeLayout(false);
            this.pnlContent.ResumeLayout(false);
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