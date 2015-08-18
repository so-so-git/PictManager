namespace SO.PictManager.Forms
{
    partial class RenameDialog
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
            this.lblPrefix = new System.Windows.Forms.Label();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.chkOriginal = new System.Windows.Forms.CheckBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.rdoBefore = new System.Windows.Forms.RadioButton();
            this.chkAddSeq = new System.Windows.Forms.CheckBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.txtRepBefore = new System.Windows.Forms.TextBox();
            this.lblReplace = new System.Windows.Forms.Label();
            this.lblArrow1 = new System.Windows.Forms.Label();
            this.txtRepAfter = new System.Windows.Forms.TextBox();
            this.rdoAfter = new System.Windows.Forms.RadioButton();
            this.txtSeqDelimiter = new System.Windows.Forms.TextBox();
            this.lblSeqDelimiter = new System.Windows.Forms.Label();
            this.chkShuffle = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkAddDirName = new System.Windows.Forms.CheckBox();
            this.txtDirDelimiter = new System.Windows.Forms.TextBox();
            this.lblDirDelimiter = new System.Windows.Forms.Label();
            this.grpSample = new System.Windows.Forms.GroupBox();
            this.lblArrow2 = new System.Windows.Forms.Label();
            this.lblSampleAfter = new System.Windows.Forms.Label();
            this.lblSampleBefore = new System.Windows.Forms.Label();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.chkSort = new System.Windows.Forms.CheckBox();
            this.nudStep = new System.Windows.Forms.NumericUpDown();
            this.grpSample.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(339, 605);
            this.btnOk.TabIndex = 26;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(255, 605);
            this.btnCancel.TabIndex = 25;
            // 
            // lblPrefix
            // 
            this.lblPrefix.Location = new System.Drawing.Point(12, 325);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(75, 25);
            this.lblPrefix.TabIndex = 15;
            this.lblPrefix.Text = "接頭文字：";
            this.lblPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSuffix
            // 
            this.lblSuffix.Location = new System.Drawing.Point(12, 360);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(75, 25);
            this.lblSuffix.TabIndex = 17;
            this.lblSuffix.Text = "接尾文字：";
            this.lblSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkOriginal
            // 
            this.chkOriginal.AutoSize = true;
            this.chkOriginal.Location = new System.Drawing.Point(14, 222);
            this.chkOriginal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkOriginal.Name = "chkOriginal";
            this.chkOriginal.Size = new System.Drawing.Size(146, 19);
            this.chkOriginal.TabIndex = 10;
            this.chkOriginal.Text = "元のファイル名を保持する";
            this.chkOriginal.UseVisualStyleBackColor = true;
            this.chkOriginal.CheckedChanged += new System.EventHandler(this.chkOriginal_CheckedChanged);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(91, 325);
            this.txtPrefix.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPrefix.MaxLength = 200;
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(298, 23);
            this.txtPrefix.TabIndex = 16;
            this.txtPrefix.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(91, 360);
            this.txtSuffix.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSuffix.MaxLength = 200;
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(298, 23);
            this.txtSuffix.TabIndex = 18;
            this.txtSuffix.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // rdoBefore
            // 
            this.rdoBefore.AutoSize = true;
            this.rdoBefore.Checked = true;
            this.rdoBefore.Location = new System.Drawing.Point(37, 252);
            this.rdoBefore.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoBefore.Name = "rdoBefore";
            this.rdoBefore.Size = new System.Drawing.Size(160, 19);
            this.rdoBefore.TabIndex = 11;
            this.rdoBefore.TabStop = true;
            this.rdoBefore.Text = "元ファイル名は通し番号の前";
            this.rdoBefore.UseVisualStyleBackColor = true;
            this.rdoBefore.CheckedChanged += new System.EventHandler(this.OriginalPosRadio_CheckedChanged);
            // 
            // chkAddSeq
            // 
            this.chkAddSeq.AutoSize = true;
            this.chkAddSeq.Location = new System.Drawing.Point(14, 155);
            this.chkAddSeq.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAddSeq.Name = "chkAddSeq";
            this.chkAddSeq.Size = new System.Drawing.Size(123, 19);
            this.chkAddSeq.TabIndex = 7;
            this.chkAddSeq.Text = "通し番号を付加する";
            this.chkAddSeq.UseVisualStyleBackColor = true;
            this.chkAddSeq.CheckedChanged += new System.EventHandler(this.chkAddSeq_CheckedChanged);
            // 
            // lblStep
            // 
            this.lblStep.Location = new System.Drawing.Point(33, 179);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(47, 25);
            this.lblStep.TabIndex = 8;
            this.lblStep.Text = "間隔：";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRepBefore
            // 
            this.txtRepBefore.Location = new System.Drawing.Point(91, 395);
            this.txtRepBefore.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRepBefore.MaxLength = 200;
            this.txtRepBefore.Name = "txtRepBefore";
            this.txtRepBefore.Size = new System.Drawing.Size(298, 23);
            this.txtRepBefore.TabIndex = 20;
            this.txtRepBefore.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // lblReplace
            // 
            this.lblReplace.Location = new System.Drawing.Point(12, 395);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(75, 25);
            this.lblReplace.TabIndex = 19;
            this.lblReplace.Text = "置換文字：";
            this.lblReplace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArrow1
            // 
            this.lblArrow1.Location = new System.Drawing.Point(91, 425);
            this.lblArrow1.Name = "lblArrow1";
            this.lblArrow1.Size = new System.Drawing.Size(19, 20);
            this.lblArrow1.TabIndex = 21;
            this.lblArrow1.Text = "↓";
            this.lblArrow1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRepAfter
            // 
            this.txtRepAfter.Location = new System.Drawing.Point(91, 450);
            this.txtRepAfter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRepAfter.MaxLength = 200;
            this.txtRepAfter.Name = "txtRepAfter";
            this.txtRepAfter.Size = new System.Drawing.Size(298, 23);
            this.txtRepAfter.TabIndex = 22;
            this.txtRepAfter.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // rdoAfter
            // 
            this.rdoAfter.AutoSize = true;
            this.rdoAfter.Location = new System.Drawing.Point(229, 252);
            this.rdoAfter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.rdoAfter.Name = "rdoAfter";
            this.rdoAfter.Size = new System.Drawing.Size(160, 19);
            this.rdoAfter.TabIndex = 12;
            this.rdoAfter.Text = "元ファイル名は通し番号の後";
            this.rdoAfter.UseVisualStyleBackColor = true;
            this.rdoAfter.CheckedChanged += new System.EventHandler(this.OriginalPosRadio_CheckedChanged);
            // 
            // txtSeqDelimiter
            // 
            this.txtSeqDelimiter.Location = new System.Drawing.Point(114, 285);
            this.txtSeqDelimiter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSeqDelimiter.MaxLength = 1;
            this.txtSeqDelimiter.Name = "txtSeqDelimiter";
            this.txtSeqDelimiter.Size = new System.Drawing.Size(27, 23);
            this.txtSeqDelimiter.TabIndex = 14;
            this.txtSeqDelimiter.Text = "_";
            this.txtSeqDelimiter.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // lblSeqDelimiter
            // 
            this.lblSeqDelimiter.Location = new System.Drawing.Point(35, 285);
            this.lblSeqDelimiter.Name = "lblSeqDelimiter";
            this.lblSeqDelimiter.Size = new System.Drawing.Size(75, 25);
            this.lblSeqDelimiter.TabIndex = 13;
            this.lblSeqDelimiter.Text = "区切文字：";
            this.lblSeqDelimiter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkShuffle
            // 
            this.chkShuffle.AutoSize = true;
            this.chkShuffle.Location = new System.Drawing.Point(14, 15);
            this.chkShuffle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkShuffle.Name = "chkShuffle";
            this.chkShuffle.Size = new System.Drawing.Size(121, 19);
            this.chkShuffle.TabIndex = 0;
            this.chkShuffle.Text = "順番をシャッフルする";
            this.chkShuffle.UseVisualStyleBackColor = true;
            this.chkShuffle.CheckedChanged += new System.EventHandler(this.chkShuffle_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(171, 605);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 30);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "クリア";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chkAddDirName
            // 
            this.chkAddDirName.AutoSize = true;
            this.chkAddDirName.Location = new System.Drawing.Point(14, 85);
            this.chkAddDirName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkAddDirName.Name = "chkAddDirName";
            this.chkAddDirName.Size = new System.Drawing.Size(184, 19);
            this.chkAddDirName.TabIndex = 4;
            this.chkAddDirName.Text = "先頭に親ディレクトリ名を挿入する";
            this.chkAddDirName.UseVisualStyleBackColor = true;
            this.chkAddDirName.CheckedChanged += new System.EventHandler(this.chkAddDirName_CheckedChanged);
            // 
            // txtDirDelimiter
            // 
            this.txtDirDelimiter.Location = new System.Drawing.Point(112, 115);
            this.txtDirDelimiter.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDirDelimiter.MaxLength = 1;
            this.txtDirDelimiter.Name = "txtDirDelimiter";
            this.txtDirDelimiter.Size = new System.Drawing.Size(27, 23);
            this.txtDirDelimiter.TabIndex = 6;
            this.txtDirDelimiter.Text = "_";
            this.txtDirDelimiter.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // lblDirDelimiter
            // 
            this.lblDirDelimiter.Location = new System.Drawing.Point(33, 115);
            this.lblDirDelimiter.Name = "lblDirDelimiter";
            this.lblDirDelimiter.Size = new System.Drawing.Size(75, 25);
            this.lblDirDelimiter.TabIndex = 5;
            this.lblDirDelimiter.Text = "区切文字：";
            this.lblDirDelimiter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpSample
            // 
            this.grpSample.Controls.Add(this.lblArrow2);
            this.grpSample.Controls.Add(this.lblSampleAfter);
            this.grpSample.Controls.Add(this.lblSampleBefore);
            this.grpSample.Location = new System.Drawing.Point(17, 486);
            this.grpSample.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpSample.Name = "grpSample";
            this.grpSample.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpSample.Size = new System.Drawing.Size(372, 108);
            this.grpSample.TabIndex = 23;
            this.grpSample.TabStop = false;
            this.grpSample.Text = "サンプル";
            // 
            // lblArrow2
            // 
            this.lblArrow2.AutoSize = true;
            this.lblArrow2.Location = new System.Drawing.Point(8, 41);
            this.lblArrow2.Name = "lblArrow2";
            this.lblArrow2.Size = new System.Drawing.Size(19, 15);
            this.lblArrow2.TabIndex = 1;
            this.lblArrow2.Text = "↓";
            // 
            // lblSampleAfter
            // 
            this.lblSampleAfter.Location = new System.Drawing.Point(8, 61);
            this.lblSampleAfter.Name = "lblSampleAfter";
            this.lblSampleAfter.Size = new System.Drawing.Size(357, 40);
            this.lblSampleAfter.TabIndex = 2;
            this.lblSampleAfter.Text = "parent\\.jpg";
            // 
            // lblSampleBefore
            // 
            this.lblSampleBefore.Location = new System.Drawing.Point(8, 19);
            this.lblSampleBefore.Name = "lblSampleBefore";
            this.lblSampleBefore.Size = new System.Drawing.Size(357, 22);
            this.lblSampleBefore.TabIndex = 0;
            this.lblSampleBefore.Text = "parent\\Image.jpg";
            // 
            // cmbSort
            // 
            this.cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Location = new System.Drawing.Point(104, 48);
            this.cmbSort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(165, 23);
            this.cmbSort.TabIndex = 3;
            // 
            // chkSort
            // 
            this.chkSort.AutoSize = true;
            this.chkSort.Location = new System.Drawing.Point(14, 51);
            this.chkSort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.chkSort.Name = "chkSort";
            this.chkSort.Size = new System.Drawing.Size(77, 19);
            this.chkSort.TabIndex = 1;
            this.chkSort.Text = "ソート順：";
            this.chkSort.UseVisualStyleBackColor = true;
            this.chkSort.CheckedChanged += new System.EventHandler(this.chkSort_CheckedChanged);
            // 
            // nudStep
            // 
            this.nudStep.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.nudStep.Location = new System.Drawing.Point(86, 182);
            this.nudStep.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudStep.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStep.Name = "nudStep";
            this.nudStep.Size = new System.Drawing.Size(51, 23);
            this.nudStep.TabIndex = 9;
            this.nudStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // RenameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 654);
            this.Controls.Add(this.nudStep);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.grpSample);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtRepAfter);
            this.Controls.Add(this.txtRepBefore);
            this.Controls.Add(this.lblReplace);
            this.Controls.Add(this.lblArrow1);
            this.Controls.Add(this.lblDirDelimiter);
            this.Controls.Add(this.txtDirDelimiter);
            this.Controls.Add(this.lblSeqDelimiter);
            this.Controls.Add(this.txtSeqDelimiter);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.rdoAfter);
            this.Controls.Add(this.rdoBefore);
            this.Controls.Add(this.txtSuffix);
            this.Controls.Add(this.txtPrefix);
            this.Controls.Add(this.chkAddDirName);
            this.Controls.Add(this.chkAddSeq);
            this.Controls.Add(this.chkSort);
            this.Controls.Add(this.chkShuffle);
            this.Controls.Add(this.chkOriginal);
            this.Controls.Add(this.lblSuffix);
            this.Controls.Add(this.lblPrefix);
            this.Name = "RenameDialog";
            this.Text = "ファイル名変更形式指定";
            this.Controls.SetChildIndex(this.lblPrefix, 0);
            this.Controls.SetChildIndex(this.lblSuffix, 0);
            this.Controls.SetChildIndex(this.chkOriginal, 0);
            this.Controls.SetChildIndex(this.chkShuffle, 0);
            this.Controls.SetChildIndex(this.chkSort, 0);
            this.Controls.SetChildIndex(this.chkAddSeq, 0);
            this.Controls.SetChildIndex(this.chkAddDirName, 0);
            this.Controls.SetChildIndex(this.txtPrefix, 0);
            this.Controls.SetChildIndex(this.txtSuffix, 0);
            this.Controls.SetChildIndex(this.rdoBefore, 0);
            this.Controls.SetChildIndex(this.rdoAfter, 0);
            this.Controls.SetChildIndex(this.lblStep, 0);
            this.Controls.SetChildIndex(this.txtSeqDelimiter, 0);
            this.Controls.SetChildIndex(this.lblSeqDelimiter, 0);
            this.Controls.SetChildIndex(this.txtDirDelimiter, 0);
            this.Controls.SetChildIndex(this.lblDirDelimiter, 0);
            this.Controls.SetChildIndex(this.lblArrow1, 0);
            this.Controls.SetChildIndex(this.lblReplace, 0);
            this.Controls.SetChildIndex(this.txtRepBefore, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.txtRepAfter, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnClear, 0);
            this.Controls.SetChildIndex(this.grpSample, 0);
            this.Controls.SetChildIndex(this.cmbSort, 0);
            this.Controls.SetChildIndex(this.nudStep, 0);
            this.grpSample.ResumeLayout(false);
            this.grpSample.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStep)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblPrefix;
        private System.Windows.Forms.Label lblSuffix;
        private System.Windows.Forms.CheckBox chkOriginal;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.TextBox txtSuffix;
        private System.Windows.Forms.RadioButton rdoBefore;
        private System.Windows.Forms.CheckBox chkAddSeq;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.TextBox txtRepBefore;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.Label lblArrow1;
        private System.Windows.Forms.TextBox txtRepAfter;
        private System.Windows.Forms.RadioButton rdoAfter;
        private System.Windows.Forms.TextBox txtSeqDelimiter;
        private System.Windows.Forms.Label lblSeqDelimiter;
        private System.Windows.Forms.CheckBox chkShuffle;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkAddDirName;
        private System.Windows.Forms.TextBox txtDirDelimiter;
        private System.Windows.Forms.Label lblDirDelimiter;
        private System.Windows.Forms.GroupBox grpSample;
        private System.Windows.Forms.Label lblSampleBefore;
        private System.Windows.Forms.Label lblArrow2;
        private System.Windows.Forms.Label lblSampleAfter;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.CheckBox chkSort;
        private System.Windows.Forms.NumericUpDown nudStep;
    }
}