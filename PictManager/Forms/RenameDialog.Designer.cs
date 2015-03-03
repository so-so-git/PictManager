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
            this.lblPrefix = new System.Windows.Forms.Label();
            this.lblSuffix = new System.Windows.Forms.Label();
            this.chkOriginal = new System.Windows.Forms.CheckBox();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.txtSuffix = new System.Windows.Forms.TextBox();
            this.rdoBefore = new System.Windows.Forms.RadioButton();
            this.chkAddSeq = new System.Windows.Forms.CheckBox();
            this.txtStep = new System.Windows.Forms.TextBox();
            this.lblStep = new System.Windows.Forms.Label();
            this.txtRepBefore = new System.Windows.Forms.TextBox();
            this.lblReplace = new System.Windows.Forms.Label();
            this.lblArrow1 = new System.Windows.Forms.Label();
            this.txtRepAfter = new System.Windows.Forms.TextBox();
            this.rdoAfter = new System.Windows.Forms.RadioButton();
            this.txtSeqDelimiter = new System.Windows.Forms.TextBox();
            this.lblDelimiter2 = new System.Windows.Forms.Label();
            this.chkShuffle = new System.Windows.Forms.CheckBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.chkAddDirName = new System.Windows.Forms.CheckBox();
            this.txtDirDelimiter = new System.Windows.Forms.TextBox();
            this.lblDelimiter1 = new System.Windows.Forms.Label();
            this.grpSample = new System.Windows.Forms.GroupBox();
            this.lblArrow2 = new System.Windows.Forms.Label();
            this.lblSampleAfter = new System.Windows.Forms.Label();
            this.lblSampleBefore = new System.Windows.Forms.Label();
            this.cmbSort = new System.Windows.Forms.ComboBox();
            this.chkSort = new System.Windows.Forms.CheckBox();
            this.grpSample.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(291, 484);
            this.btnOk.TabIndex = 26;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(219, 484);
            this.btnCancel.TabIndex = 25;
            // 
            // lblPrefix
            // 
            this.lblPrefix.Location = new System.Drawing.Point(10, 260);
            this.lblPrefix.Name = "lblPrefix";
            this.lblPrefix.Size = new System.Drawing.Size(64, 20);
            this.lblPrefix.TabIndex = 15;
            this.lblPrefix.Text = "接頭文字：";
            this.lblPrefix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSuffix
            // 
            this.lblSuffix.Location = new System.Drawing.Point(10, 288);
            this.lblSuffix.Name = "lblSuffix";
            this.lblSuffix.Size = new System.Drawing.Size(64, 20);
            this.lblSuffix.TabIndex = 17;
            this.lblSuffix.Text = "接尾文字：";
            this.lblSuffix.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkOriginal
            // 
            this.chkOriginal.AutoSize = true;
            this.chkOriginal.Location = new System.Drawing.Point(12, 178);
            this.chkOriginal.Name = "chkOriginal";
            this.chkOriginal.Size = new System.Drawing.Size(144, 16);
            this.chkOriginal.TabIndex = 10;
            this.chkOriginal.Text = "元のファイル名を保持する";
            this.chkOriginal.UseVisualStyleBackColor = true;
            this.chkOriginal.CheckedChanged += new System.EventHandler(this.chkOriginal_CheckedChanged);
            // 
            // txtPrefix
            // 
            this.txtPrefix.Location = new System.Drawing.Point(78, 260);
            this.txtPrefix.MaxLength = 200;
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(256, 19);
            this.txtPrefix.TabIndex = 16;
            this.txtPrefix.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // txtSuffix
            // 
            this.txtSuffix.Location = new System.Drawing.Point(78, 288);
            this.txtSuffix.MaxLength = 200;
            this.txtSuffix.Name = "txtSuffix";
            this.txtSuffix.Size = new System.Drawing.Size(256, 19);
            this.txtSuffix.TabIndex = 18;
            this.txtSuffix.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // rdoBefore
            // 
            this.rdoBefore.AutoSize = true;
            this.rdoBefore.Checked = true;
            this.rdoBefore.Location = new System.Drawing.Point(32, 202);
            this.rdoBefore.Name = "rdoBefore";
            this.rdoBefore.Size = new System.Drawing.Size(158, 16);
            this.rdoBefore.TabIndex = 11;
            this.rdoBefore.TabStop = true;
            this.rdoBefore.Text = "元ファイル名は通し番号の前";
            this.rdoBefore.UseVisualStyleBackColor = true;
            this.rdoBefore.CheckedChanged += new System.EventHandler(this.OriginalPosRadio_CheckedChanged);
            // 
            // chkAddSeq
            // 
            this.chkAddSeq.AutoSize = true;
            this.chkAddSeq.Location = new System.Drawing.Point(12, 124);
            this.chkAddSeq.Name = "chkAddSeq";
            this.chkAddSeq.Size = new System.Drawing.Size(121, 16);
            this.chkAddSeq.TabIndex = 7;
            this.chkAddSeq.Text = "通し番号を付加する";
            this.chkAddSeq.UseVisualStyleBackColor = true;
            this.chkAddSeq.CheckedChanged += new System.EventHandler(this.chkAddSeq_CheckedChanged);
            // 
            // txtStep
            // 
            this.txtStep.Location = new System.Drawing.Point(72, 143);
            this.txtStep.MaxLength = 3;
            this.txtStep.Name = "txtStep";
            this.txtStep.Size = new System.Drawing.Size(32, 19);
            this.txtStep.TabIndex = 9;
            this.txtStep.Text = "1";
            // 
            // lblStep
            // 
            this.lblStep.Location = new System.Drawing.Point(28, 143);
            this.lblStep.Name = "lblStep";
            this.lblStep.Size = new System.Drawing.Size(40, 20);
            this.lblStep.TabIndex = 8;
            this.lblStep.Text = "間隔：";
            this.lblStep.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRepBefore
            // 
            this.txtRepBefore.Location = new System.Drawing.Point(78, 316);
            this.txtRepBefore.MaxLength = 200;
            this.txtRepBefore.Name = "txtRepBefore";
            this.txtRepBefore.Size = new System.Drawing.Size(256, 19);
            this.txtRepBefore.TabIndex = 20;
            this.txtRepBefore.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // lblReplace
            // 
            this.lblReplace.Location = new System.Drawing.Point(10, 316);
            this.lblReplace.Name = "lblReplace";
            this.lblReplace.Size = new System.Drawing.Size(64, 20);
            this.lblReplace.TabIndex = 19;
            this.lblReplace.Text = "置換文字：";
            this.lblReplace.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblArrow1
            // 
            this.lblArrow1.Location = new System.Drawing.Point(78, 340);
            this.lblArrow1.Name = "lblArrow1";
            this.lblArrow1.Size = new System.Drawing.Size(16, 16);
            this.lblArrow1.TabIndex = 21;
            this.lblArrow1.Text = "↓";
            this.lblArrow1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtRepAfter
            // 
            this.txtRepAfter.Location = new System.Drawing.Point(78, 360);
            this.txtRepAfter.MaxLength = 200;
            this.txtRepAfter.Name = "txtRepAfter";
            this.txtRepAfter.Size = new System.Drawing.Size(256, 19);
            this.txtRepAfter.TabIndex = 22;
            this.txtRepAfter.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // rdoAfter
            // 
            this.rdoAfter.AutoSize = true;
            this.rdoAfter.Location = new System.Drawing.Point(196, 202);
            this.rdoAfter.Name = "rdoAfter";
            this.rdoAfter.Size = new System.Drawing.Size(158, 16);
            this.rdoAfter.TabIndex = 12;
            this.rdoAfter.Text = "元ファイル名は通し番号の後";
            this.rdoAfter.UseVisualStyleBackColor = true;
            this.rdoAfter.CheckedChanged += new System.EventHandler(this.OriginalPosRadio_CheckedChanged);
            // 
            // txtSeqDelimiter
            // 
            this.txtSeqDelimiter.Location = new System.Drawing.Point(98, 228);
            this.txtSeqDelimiter.MaxLength = 1;
            this.txtSeqDelimiter.Name = "txtSeqDelimiter";
            this.txtSeqDelimiter.Size = new System.Drawing.Size(24, 19);
            this.txtSeqDelimiter.TabIndex = 14;
            this.txtSeqDelimiter.Text = "_";
            this.txtSeqDelimiter.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // lblDelimiter2
            // 
            this.lblDelimiter2.Location = new System.Drawing.Point(30, 228);
            this.lblDelimiter2.Name = "lblDelimiter2";
            this.lblDelimiter2.Size = new System.Drawing.Size(64, 20);
            this.lblDelimiter2.TabIndex = 13;
            this.lblDelimiter2.Text = "区切文字：";
            this.lblDelimiter2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chkShuffle
            // 
            this.chkShuffle.AutoSize = true;
            this.chkShuffle.Location = new System.Drawing.Point(12, 12);
            this.chkShuffle.Name = "chkShuffle";
            this.chkShuffle.Size = new System.Drawing.Size(119, 16);
            this.chkShuffle.TabIndex = 0;
            this.chkShuffle.Text = "順番をシャッフルする";
            this.chkShuffle.UseVisualStyleBackColor = true;
            this.chkShuffle.CheckedChanged += new System.EventHandler(this.chkShuffle_CheckedChanged);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(147, 484);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(64, 24);
            this.btnClear.TabIndex = 24;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // chkAddDirName
            // 
            this.chkAddDirName.AutoSize = true;
            this.chkAddDirName.Location = new System.Drawing.Point(12, 68);
            this.chkAddDirName.Name = "chkAddDirName";
            this.chkAddDirName.Size = new System.Drawing.Size(182, 16);
            this.chkAddDirName.TabIndex = 4;
            this.chkAddDirName.Text = "先頭に親ディレクトリ名を挿入する";
            this.chkAddDirName.UseVisualStyleBackColor = true;
            this.chkAddDirName.CheckedChanged += new System.EventHandler(this.chkAddDirName_CheckedChanged);
            // 
            // txtDirDelimiter
            // 
            this.txtDirDelimiter.Location = new System.Drawing.Point(96, 92);
            this.txtDirDelimiter.MaxLength = 1;
            this.txtDirDelimiter.Name = "txtDirDelimiter";
            this.txtDirDelimiter.Size = new System.Drawing.Size(24, 19);
            this.txtDirDelimiter.TabIndex = 6;
            this.txtDirDelimiter.Text = "_";
            this.txtDirDelimiter.TextChanged += new System.EventHandler(this.TextBoxes_TextChanged);
            // 
            // lblDelimiter1
            // 
            this.lblDelimiter1.Location = new System.Drawing.Point(28, 92);
            this.lblDelimiter1.Name = "lblDelimiter1";
            this.lblDelimiter1.Size = new System.Drawing.Size(64, 20);
            this.lblDelimiter1.TabIndex = 5;
            this.lblDelimiter1.Text = "区切文字：";
            this.lblDelimiter1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grpSample
            // 
            this.grpSample.Controls.Add(this.lblArrow2);
            this.grpSample.Controls.Add(this.lblSampleAfter);
            this.grpSample.Controls.Add(this.lblSampleBefore);
            this.grpSample.Location = new System.Drawing.Point(15, 389);
            this.grpSample.Name = "grpSample";
            this.grpSample.Size = new System.Drawing.Size(319, 86);
            this.grpSample.TabIndex = 23;
            this.grpSample.TabStop = false;
            this.grpSample.Text = "サンプル";
            // 
            // lblArrow2
            // 
            this.lblArrow2.AutoSize = true;
            this.lblArrow2.Location = new System.Drawing.Point(7, 33);
            this.lblArrow2.Name = "lblArrow2";
            this.lblArrow2.Size = new System.Drawing.Size(17, 12);
            this.lblArrow2.TabIndex = 1;
            this.lblArrow2.Text = "↓";
            // 
            // lblSampleAfter
            // 
            this.lblSampleAfter.Location = new System.Drawing.Point(7, 49);
            this.lblSampleAfter.Name = "lblSampleAfter";
            this.lblSampleAfter.Size = new System.Drawing.Size(306, 32);
            this.lblSampleAfter.TabIndex = 2;
            this.lblSampleAfter.Text = "parent\\.jpg";
            // 
            // lblSampleBefore
            // 
            this.lblSampleBefore.Location = new System.Drawing.Point(7, 15);
            this.lblSampleBefore.Name = "lblSampleBefore";
            this.lblSampleBefore.Size = new System.Drawing.Size(306, 18);
            this.lblSampleBefore.TabIndex = 0;
            this.lblSampleBefore.Text = "parent\\Image.jpg";
            // 
            // cmbSort
            // 
            this.cmbSort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSort.FormattingEnabled = true;
            this.cmbSort.Location = new System.Drawing.Point(89, 38);
            this.cmbSort.Name = "cmbSort";
            this.cmbSort.Size = new System.Drawing.Size(142, 20);
            this.cmbSort.TabIndex = 3;
            // 
            // chkSort
            // 
            this.chkSort.AutoSize = true;
            this.chkSort.Location = new System.Drawing.Point(12, 41);
            this.chkSort.Name = "chkSort";
            this.chkSort.Size = new System.Drawing.Size(69, 16);
            this.chkSort.TabIndex = 1;
            this.chkSort.Text = "ソート順：";
            this.chkSort.UseVisualStyleBackColor = true;
            this.chkSort.CheckedChanged += new System.EventHandler(this.chkSort_CheckedChanged);
            // 
            // RenameDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(368, 523);
            this.Controls.Add(this.cmbSort);
            this.Controls.Add(this.grpSample);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtRepAfter);
            this.Controls.Add(this.txtRepBefore);
            this.Controls.Add(this.lblReplace);
            this.Controls.Add(this.lblArrow1);
            this.Controls.Add(this.lblDelimiter1);
            this.Controls.Add(this.txtDirDelimiter);
            this.Controls.Add(this.lblDelimiter2);
            this.Controls.Add(this.txtSeqDelimiter);
            this.Controls.Add(this.lblStep);
            this.Controls.Add(this.txtStep);
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
            this.Controls.SetChildIndex(this.txtStep, 0);
            this.Controls.SetChildIndex(this.lblStep, 0);
            this.Controls.SetChildIndex(this.txtSeqDelimiter, 0);
            this.Controls.SetChildIndex(this.lblDelimiter2, 0);
            this.Controls.SetChildIndex(this.txtDirDelimiter, 0);
            this.Controls.SetChildIndex(this.lblDelimiter1, 0);
            this.Controls.SetChildIndex(this.lblArrow1, 0);
            this.Controls.SetChildIndex(this.lblReplace, 0);
            this.Controls.SetChildIndex(this.txtRepBefore, 0);
            this.Controls.SetChildIndex(this.btnOk, 0);
            this.Controls.SetChildIndex(this.txtRepAfter, 0);
            this.Controls.SetChildIndex(this.btnCancel, 0);
            this.Controls.SetChildIndex(this.btnClear, 0);
            this.Controls.SetChildIndex(this.grpSample, 0);
            this.Controls.SetChildIndex(this.cmbSort, 0);
            this.grpSample.ResumeLayout(false);
            this.grpSample.PerformLayout();
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
        private System.Windows.Forms.TextBox txtStep;
        private System.Windows.Forms.Label lblStep;
        private System.Windows.Forms.TextBox txtRepBefore;
        private System.Windows.Forms.Label lblReplace;
        private System.Windows.Forms.Label lblArrow1;
        private System.Windows.Forms.TextBox txtRepAfter;
        private System.Windows.Forms.RadioButton rdoAfter;
        private System.Windows.Forms.TextBox txtSeqDelimiter;
        private System.Windows.Forms.Label lblDelimiter2;
        private System.Windows.Forms.CheckBox chkShuffle;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.CheckBox chkAddDirName;
        private System.Windows.Forms.TextBox txtDirDelimiter;
        private System.Windows.Forms.Label lblDelimiter1;
        private System.Windows.Forms.GroupBox grpSample;
        private System.Windows.Forms.Label lblSampleBefore;
        private System.Windows.Forms.Label lblArrow2;
        private System.Windows.Forms.Label lblSampleAfter;
        private System.Windows.Forms.ComboBox cmbSort;
        private System.Windows.Forms.CheckBox chkSort;
    }
}