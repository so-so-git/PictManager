namespace SO.PictManager.Components
{
    partial class ThumbnailUnit
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

        #region コンポーネント デザイナで生成されたコード

        /// <summary> 
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.picThumbnail = new System.Windows.Forms.PictureBox();
            this.lblImageKey = new System.Windows.Forms.Label();
            this.tipInfo = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // picThumbnail
            // 
            this.picThumbnail.Location = new System.Drawing.Point(12, 12);
            this.picThumbnail.Name = "picThumbnail";
            this.picThumbnail.Size = new System.Drawing.Size(80, 80);
            this.picThumbnail.TabIndex = 0;
            this.picThumbnail.TabStop = false;
            this.picThumbnail.Click += new System.EventHandler(this.picThumbnail_Click);
            this.picThumbnail.DoubleClick += new System.EventHandler(this.picThumbnail_DoubleClick);
            // 
            // lblImageKey
            // 
            this.lblImageKey.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblImageKey.Location = new System.Drawing.Point(0, 100);
            this.lblImageKey.Name = "lblImageKey";
            this.lblImageKey.Size = new System.Drawing.Size(104, 23);
            this.lblImageKey.TabIndex = 1;
            this.lblImageKey.Text = "[表示画像キー]";
            this.lblImageKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblImageKey.Click += new System.EventHandler(this.lblImageKey_Click);
            this.lblImageKey.DoubleClick += new System.EventHandler(this.lblImageKey_DoubleClick);
            // 
            // tipInfo
            // 
            this.tipInfo.AutoPopDelay = 5000;
            this.tipInfo.InitialDelay = 1000;
            this.tipInfo.ReshowDelay = 1000;
            this.tipInfo.ShowAlways = true;
            // 
            // ThumbnailUnit
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.lblImageKey);
            this.Controls.Add(this.picThumbnail);
            this.Name = "ThumbnailUnit";
            this.Size = new System.Drawing.Size(106, 127);
            this.Click += new System.EventHandler(this.ThumbnailUnit_Click);
            this.DoubleClick += new System.EventHandler(this.ThumbnailUnit_DoubleClick);
            ((System.ComponentModel.ISupportInitialize)(this.picThumbnail)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picThumbnail;
        private System.Windows.Forms.Label lblImageKey;
        private System.Windows.Forms.ToolTip tipInfo;
    }
}
