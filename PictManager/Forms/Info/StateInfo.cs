using System;
using System.Collections.Generic;
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
			LastPath = null;
			SizeMode = PictureBoxSizeMode.Zoom;
            SortOrder = ImageSortOrder.KeyAsc;
        }

        /// <summary>
        /// コピー元インスタンス指定付きのコンストラクタです。
        /// </summary>
        /// <param orderName="original">コピー元インスタンス</param>
		public StateInfo(StateInfo original)
		{
			StateInfo newObj = new StateInfo();

			newObj.LastPath = LastPath;
			newObj.SizeMode = SizeMode;
            newObj.SortOrder = SortOrder;
		}
        #endregion

		#region プロパティ

        /// <summary>最後に表示したディレクトリのパスを取得または設定します。</summary>
		public string LastPath { get; set; }

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
