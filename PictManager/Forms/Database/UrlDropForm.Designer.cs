namespace SO.PictManager.Forms.Database
{
    partial class UrlDropForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UrlDropForm));
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.btnAddCategory = new System.Windows.Forms.Button();
            this.lblDropArea = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(15, 16);
            this.cmbCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(140, 23);
            this.cmbCategory.TabIndex = 0;
            // 
            // btnAddCategory
            // 
            this.btnAddCategory.Location = new System.Drawing.Point(163, 14);
            this.btnAddCategory.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAddCategory.Name = "btnAddCategory";
            this.btnAddCategory.Size = new System.Drawing.Size(89, 29);
            this.btnAddCategory.TabIndex = 1;
            this.btnAddCategory.Text = "カテゴリ追加";
            this.btnAddCategory.UseVisualStyleBackColor = true;
            this.btnAddCategory.Click += new System.EventHandler(this.btnAddCategory_Click);
            // 
            // lblDropArea
            // 
            this.lblDropArea.AllowDrop = true;
            this.lblDropArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblDropArea.BackColor = System.Drawing.SystemColors.Control;
            this.lblDropArea.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblDropArea.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.lblDropArea.Image = ((System.Drawing.Image)(resources.GetObject("lblDropArea.Image")));
            this.lblDropArea.Location = new System.Drawing.Point(16, 58);
            this.lblDropArea.Name = "lblDropArea";
            this.lblDropArea.Size = new System.Drawing.Size(236, 131);
            this.lblDropArea.TabIndex = 2;
            this.lblDropArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDropArea.DragDrop += new System.Windows.Forms.DragEventHandler(this.lblDropArea_DragDrop);
            this.lblDropArea.DragEnter += new System.Windows.Forms.DragEventHandler(this.lblDropArea_DragEnter);
            // 
            // UrlDropForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(271, 209);
            this.Controls.Add(this.lblDropArea);
            this.Controls.Add(this.btnAddCategory);
            this.Controls.Add(this.cmbCategory);
            this.Font = new System.Drawing.Font("Meiryo UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "UrlDropForm";
            this.Text = "PictManager - URL受付";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Button btnAddCategory;
        private System.Windows.Forms.Label lblDropArea;
    }
}