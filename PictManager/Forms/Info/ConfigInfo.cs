using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SO.PictManager.Forms.Info
{
    /// <summary>
    /// システム設定保管クラス
    /// </summary>
    [Serializable()]
    public sealed class ConfigInfo : ICloneable
    {
        #region 定数定義

        /// <summary>ファイル保存先パス取得用のAppSettingsのキー</summary>
        public const string SAVE_PATH_KEY = "ConfigFilePath";

        #endregion

        #region enum ImageDataMode - 読込画像データモード

        /// <summary>
        /// 読込画像データモード列挙体
        /// </summary>
        public enum ImageDataMode
        {
            /// <summary>ファイル読込モード</summary>
            File,
            /// <summary>データベース読込モード</summary>
            Database,
        }

        #endregion

        #region class CommonConfig - 共通システム設定情報クラス

        /// <summary>
        /// 共通システム設定情報クラス
        /// </summary>
        public class CommonConfig : ICloneable
        {
            #region プロパティ

            /// <summary>対象ファイルの拡張子リストを取得・設定します。</summary>
            public List<string> TargetExtensions { get; set; }

            /// <summary>サブディレクトリを含むかのフラグを取得・設定します。</summary>
            public bool IsIncludeSubDirectory { get; set; }

            /// <summary>終了時に確認をするかのフラグを取得・設定します。</summary>
            public bool IsConfirmQuit { get; set; }

            /// <summary>読込画像データモードを取得・設定します。</summary>
            public ImageDataMode Mode { get; set; }

            /// <summary>コントローラを使用するかのフラグを取得・設定します。</summary>
            public bool IsUseJoystick { get; set; }

            #endregion

            #region コンストラクタ

            /// <summary>
            /// デフォルトのコンストラクタです。
            /// </summary>
            public CommonConfig()
            {
                TargetExtensions = new List<string>();
            }

            /// <summary>
            /// コピー元インスタンス指定付きのコンストラクタです。
            /// </summary>
            /// <param orderName="original">コピー元インスタンス</param>
            public CommonConfig(CommonConfig original)
		    {
                var newObj = new CommonConfig();
                original.TargetExtensions.AddRange(newObj.TargetExtensions);
                newObj.IsIncludeSubDirectory = original.IsIncludeSubDirectory;
                newObj.IsConfirmQuit = original.IsConfirmQuit;
                newObj.Mode = original.Mode;
                newObj.IsUseJoystick = original.IsUseJoystick;
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
                return new CommonConfig(this);
            }

            #endregion
        }

        #endregion

        #region class SlideConfig - スライド表示のシステム設定情報クラス

        /// <summary>
        /// スライド表示のシステム設定情報クラス
        /// </summary>
        public class SlideConfig : ICloneable
        {
            #region プロパティ

            /// <summary>ブックマークウィンドウを常に全面に表示するかのフラグを取得・設定します。</summary>
            public bool IsBookmarkTopMost { get; set; }

            #endregion

            #region コンストラクタ

            /// <summary>
            /// デフォルトのコンストラクタです。
            /// </summary>
            public SlideConfig() { }

            /// <summary>
            /// コピー元インスタンス指定付きのコンストラクタです。
            /// </summary>
            /// <param orderName="original">コピー元インスタンス</param>
            public SlideConfig(SlideConfig original)
            {
                var newObj = new SlideConfig();
                newObj.IsBookmarkTopMost = original.IsBookmarkTopMost;
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
                return new SlideConfig(this);
            }

            #endregion
        }

        #endregion

        #region class ListConfig - リスト表示のシステム設定情報クラス

        /// <summary>
        /// リスト表示のシステム設定情報クラス
        /// </summary>
        public class ListConfig : ICloneable
        {
            #region プロパティ

            #endregion

            #region コンストラクタ

            /// <summary>
            /// デフォルトのコンストラクタです。
            /// </summary>
            public ListConfig() { }

            /// <summary>
            /// コピー元インスタンス指定付きのコンストラクタです。
            /// </summary>
            /// <param orderName="original">コピー元インスタンス</param>
            public ListConfig(ListConfig original)
            {
                var newObj = new ListConfig();
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
                return new ListConfig(this);
            }

            #endregion
        }

        #endregion

        #region class ThumbnailConfig - サムネイル表示のシステム設定情報クラス

        /// <summary>
        /// サムネイル表示のシステム設定情報クラス
        /// </summary>
        public class ThumbnailConfig : ICloneable
        {
            #region プロパティ

            #endregion

            #region コンストラクタ

            /// <summary>
            /// デフォルトのコンストラクタです。
            /// </summary>
            public ThumbnailConfig() { }

            /// <summary>
            /// コピー元インスタンス指定付きのコンストラクタです。
            /// </summary>
            /// <param orderName="original">コピー元インスタンス</param>
            public ThumbnailConfig(ThumbnailConfig original)
            {
                var newObj = new ThumbnailConfig();
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
                return new ThumbnailConfig(this);
            }

            #endregion
        }

        #endregion

        #region プロパティ

        /// <summary>共通システム設定を取得・設定します。</summary>
        public CommonConfig CommonInfo { get; set; }

        /// <summary>スライド表示のシステム設定を取得・設定します。</summary>
        public SlideConfig SlideInfo { get; set; }

        /// <summary>リスト表示のシステム設定を取得・設定します。</summary>
        public ListConfig ListInfo { get; set; }

        /// <summary>サムネイル表示のシステム設定を取得・設定します。</summary>
        public ThumbnailConfig ThumbnailInfo { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public ConfigInfo()
        {
            CommonInfo = new CommonConfig();
            SlideInfo = new SlideConfig();
            ListInfo = new ListConfig();
            ThumbnailInfo = new ThumbnailConfig();
        }

        /// <summary>
        /// コピー元インスタンス指定付きのコンストラクタです。
        /// </summary>
        /// <param orderName="original">コピー元インスタンス</param>
        public ConfigInfo(ConfigInfo original)
		{
            var newObj = new ConfigInfo();
            newObj.CommonInfo = original.CommonInfo.Clone() as CommonConfig;
            newObj.SlideInfo = original.SlideInfo.Clone() as SlideConfig;
            newObj.ListInfo = original.ListInfo.Clone() as ListConfig;
            newObj.ThumbnailInfo = original.ThumbnailInfo.Clone() as ThumbnailConfig;
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
            return new ConfigInfo(this);
        }

        #endregion
    }
}
