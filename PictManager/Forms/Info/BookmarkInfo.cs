using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SO.PictManager.Common;

namespace SO.PictManager.Forms.Info
{
    /// <summary>
    /// ブックマーク情報クラス
    /// </summary>
    public sealed class BookmarkInfo : ICloneable
    {
        #region クラス定数

        /// <summary>タイムスタンプの書式</summary>
        public const string TIMESTAMP_FORMAT = "yyyy/MM/dd H:mm";

        /// <summary>ファイル保存先パス取得用のAppSettingsのキー</summary>
        public const string SAVE_PATH_KEY = "BookmarkFilePath";

        #endregion

        #region プロパティ

        /// <summary>ブックマークのIDを取得または設定します。</summary>
        public int Id { get; set; }

        /// <summary>ブックマーク名を取得または設定します。</summary>
        public string Name { get; set; }

        /// <summary>ブックマークしたファイルのNoを取得または設定します。</summary>
        public int FileNo { get; set; }

        /// <summary>ブックマークした日時を取得または設定します。</summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>ブックマーク時のファイルソート順を取得または設定します。</summary>
        public ImageSortOrder SortOrder { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 各フィールドの値を指定してインスタンスを作成するコンストラクタです。
        /// </summary>
        /// <param name="id">ブックマークのID</param>
        /// <param name="name">ブックマーク名</param>
        /// <param name="fileNo">ブックマークしたファイルのNo</param>
        /// <param name="timeStamp">ブックマークした日時</param>
        /// <param name="sortOrder">ブックマーク時のファイルソート順</param>
        public BookmarkInfo(int id, string name, int fileNo, DateTime timeStamp,
                            ImageSortOrder sortOrder = ImageSortOrder.KeyAsc)
        {
            Id = id;
            Name = name;
            FileNo = fileNo;
            TimeStamp = timeStamp;
            SortOrder = sortOrder;
        }

        /// <summary>
        /// コピー元のインスタンスを指定してインスタンスを作成するコンストラクタです。
        /// </summary>
        /// <param name="src">コピー元のインスタンス</param>
        public BookmarkInfo(BookmarkInfo src)
        {
            Id = src.Id;
            Name = src.Name;
            FileNo = src.FileNo;
            TimeStamp = src.TimeStamp;
            SortOrder = src.SortOrder;
        }

        #endregion

        #region Clone - インスタンスのクローンを作成

        /// <summary>
        /// (ICloneable.Clone()を実装します)
        /// インスタンスのクローンを作成します。
        /// </summary>
        /// <returns>インスタンスのクローン</returns>
        public object Clone()
        {
            return new BookmarkInfo(this);
        }

        #endregion
    }
}
