using System;
using System.Windows.Forms;
using SO.PictManager.Common;

namespace SO.PictManager.Forms.Info
{
    /// <summary>
    /// 状態情報保管クラス
    /// </summary>
    [Serializable()]
    public sealed class StateInfo
    {
        #region 定数定義

        /// <summary>ファイル保存先パス取得用のAppSettingsのキー</summary>
        public const string SAVE_PATH_KEY = "StateFilePath";

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public StateInfo()
        {
			LastViewPath = null;
            LastAutoImportPath = null;
			SizeMode = PictureBoxSizeMode.Normal;
            SortOrder = ImageSortOrder.KeyAsc;
        }

        /// <summary>
        /// コピー元インスタンス指定付きのコンストラクタです。
        /// </summary>
        /// <param name="original">コピー元インスタンス</param>
		public StateInfo(StateInfo original)
		{
			var newObj = new StateInfo();

			newObj.LastViewPath = this.LastViewPath;
            newObj.LastAutoImportPath = this.LastAutoImportPath;
            newObj.SizeMode = this.SizeMode;
            newObj.SortOrder = this.SortOrder;
		}

        #endregion

		#region プロパティ

        /// <summary>最後に表示したフォルダのパスを取得または設定します。</summary>
		public string LastViewPath { get; set; }

        /// <summary>最後に指定した自動取込フォルダのパスを取得または設定します。</summary>
        public string LastAutoImportPath { get; set; }

        /// <summary>画像表示時のPictureBoxSizeModeを取得または設定します。</summary>
		public PictureBoxSizeMode SizeMode { get; set; }

        /// <summary>連続表示時のFileSortOrderを取得または設定します。</summary>
        public ImageSortOrder SortOrder { get; set; }

        #endregion

        #region Clone - インスタンスのクローンを作成

        /// <summary>
        /// (ICloneable.Clone()を実装します)
        /// インスタンスのクローンを作成します。
        /// </summary>
        /// <returns>インスタンスのクローン</returns>
        public object Clone()
        {
            return new StateInfo(this);
        }

        #endregion
    }
}
