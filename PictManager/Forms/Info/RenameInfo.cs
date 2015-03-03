using System;

using SO.PictManager.Common;

using EnumOrignalPosition = SO.PictManager.Forms.OriginalPosition;

namespace SO.PictManager.Forms.Info
{
    /// <summary>
    /// ファイルリネーム情報保管用クラス
    /// </summary>
    [Serializable()]
    public sealed class RenameInfo : ICloneable
    {
        #region 定数定義

        /// <summary>ファイル保存先パス取得用のAppSettingsのキー</summary>
        public const string SAVE_PATH_KEY = "RenameFilePath";

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public RenameInfo()
        {
            // クラス変数初期化
            SortOrder = null;
			IsAddParentDirName = false;
			DirDelimiter = null;
			IsShuffle = false;
			IsReserveOriginalName = false;
			IsAddSequential = false;
			IncrementStep = null;
			SeqDelimiter = null;
			Prefix = null;
			Suffix = null;
			ReplaceBefore = null;
			ReplaceAfter = null;
			OriginalPosition = EnumOrignalPosition.Before;
        }

        #endregion

		#region プロパティ

        /// <summary>ソート順を取得・設定します。</summary>
        public FileSortOrder? SortOrder { get; set; }

        /// <summary>親ディレクトリ名をファイル名に含めるかを取得・設定します。</summary>
		public bool IsAddParentDirName { get; set; }

        /// <summary>親ディレクトリ名の区切り文字を取得・設定します。</summary>
		public string DirDelimiter { get; set; }

        /// <summary>ファイル順をシャッフルするかを取得・設定します。</summary>
		public bool IsShuffle { get; set; }

        /// <summary>元ファイル名を変更後のファイル名に含めるかを取得・設定します。</summary>
		public bool IsReserveOriginalName { get; set; }

        /// <summary>通り番号をファイル名に含めるかを取得・設定します。</summary>
		public bool IsAddSequential { get; set; }

        /// <summary>通し番号の間隔を取得・設定します。</summary>
		public int? IncrementStep { get; set; }

        /// <summary>通し番号と元ファイル名の区切り文字を取得・設定します。</summary>
		public string SeqDelimiter { get; set; }

        /// <summary>ファイル名に付加する接頭文字列を取得・設定します。</summary>
		public string Prefix { get; set; }

        /// <summary>ファイル名に付加する接尾文字列を取得・設定します。</summary>
		public string Suffix { get; set; }

        /// <summary>置換前文字列を取得・設定します。</summary>
		public string ReplaceBefore { get; set; }

        /// <summary>置換後文字列を取得・設定します。</summary>
		public string ReplaceAfter { get; set; }

        /// <summary>元ファイル名の挿入位置を取得・設定します。</summary>
		public EnumOrignalPosition OriginalPosition { get; set; }

		#endregion

        #region Clone - インスタンスのクローンを作成

        /// <summary>
        /// (ICloneable.Clone()を実装します)
        /// インスタンスのクローンを作成します。
        /// </summary>
        /// <returns>インスタンスのクローン</returns>
        public object Clone()
        {
            RenameInfo newObj = new RenameInfo();

            newObj.SortOrder = SortOrder;
			newObj.IsAddParentDirName = IsAddParentDirName;
			newObj.DirDelimiter = DirDelimiter;
			newObj.IsShuffle = IsShuffle;
			newObj.IsReserveOriginalName = IsReserveOriginalName;
			newObj.IsAddSequential = IsAddSequential;
			newObj.IncrementStep = IncrementStep;
			newObj.SeqDelimiter = SeqDelimiter;
			newObj.Prefix = Prefix;
			newObj.Suffix = Suffix;
			newObj.ReplaceBefore = ReplaceBefore;
			newObj.ReplaceAfter = ReplaceAfter;
			newObj.OriginalPosition = OriginalPosition;

            return newObj;
        }

        #endregion
    }
}
