using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SO.PictManager.Common
{
    /// <summary>
    /// 定数定義クラス
    /// </summary>
    public static class Constants
    {
        /// <summary>カテゴリーID：未分類</summary>
        public const int UN_CLASSIFIED_CATEGORY_ID = 0;

        /// <summary>カテゴリー名：未分類</summary>
        public const string UN_CLASSIFIED_CATEGORY_NAME = "(未分類)";

        /// <summary>マウスホイールの1移動量に対するデルタ値の比率</summary>
        public const int WHEEL_DELTA = 120;
    }

    #region enum ResultStatus - 処理結果ステータス列挙体

    /// <summary>
    /// 子画面処理結果列挙体
    /// </summary>
    public enum ResultStatus
    {
        /// <summary>未設定状態</summary>
        Empty,
        /// <summary>処理キャンセル</summary>
        Cancel,
        /// <summary>正常終了</summary>
        OK,
        /// <summary>異常終了</summary>
        Error,
    }

    #endregion
}
