using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using SO.PictManager.Common;
using SO.PictManager.Forms.Info;
using SO.Library.IO;

namespace SO.PictManager.Common
{
    /// <summary>
    /// アプリケーション内汎用機能定義クラス
    /// </summary>
    internal static class Utilities
    {
        #region プロパティ

        /// <summary>
        /// システム設定情報を取得・設定します。
        /// </summary>
        internal static ConfigInfo Config { get; set; }

        /// <summary>
        /// 状態情報を取得・設定します。
        /// </summary>
        internal static StateInfo State { get; set; }

        /// <summary>
        /// ファイルリネーム情報を取得・設定します。
        /// </summary>
        internal static RenameInfo Rename { get; set; }

        /// <summary>
        /// アプリケーションの共通ロガーを取得・設定します。
        /// </summary>
        internal static Logger Logger { get; set; }

        #endregion

        #region 静的コンストラクタ
        /// <summary>
        /// 静的コンストラクタです。
        /// </summary>
        static Utilities()
        {
            RestoreConfigInfo();
            RestoreStateInfo();
            RestoreRenameInfo();

            // ロガー生成
            string logPath = ConfigurationManager.AppSettings["ExecuteLogPath"];
            if (!string.IsNullOrEmpty(logPath))
            {
                Logger = new Logger(logPath);
            }
        }
        #endregion

        #region IsAvailableFormat - 対応形式チェック
        /// <summary>
        /// 指定されたURIにあるファイルがアプリケーションで処理出来るファイルか確認します。
        /// </summary>
        /// <param name="uri">確認対象ファイルのURI</param>
        /// <param name="isCheckExists">trueの場合、ファイル存在チェックを行い存在しない場合は処理不可として扱います</param>
        /// <returns>処理可能ファイルの場合:true、処理不可能ファイルの場合:false</returns>
        internal static bool IsAvailableFormat(string uri, bool isCheckExists)
        {
            if (isCheckExists && !File.Exists(uri)) return false;

            return Utilities.Config.CommonInfo.TargetExtensions
                    .Any(x => Path.GetExtension(uri) == "." + x);
        }
        #endregion

        #region OpenExplorer - エクスプローラでパスを表示
        /// <summary>
        /// 指定されたパスをエクスプローラで開きます。
        /// </summary>
        /// <param orderName="path">ディレクトリパス</param>
        public static void OpenExplorer(string path)
        {
            var procInfo = new ProcessStartInfo("explorer", path);
            procInfo.CreateNoWindow = true;
            procInfo.UseShellExecute = false;
            procInfo.WindowStyle = ProcessWindowStyle.Normal;

            var p = new Process();
            p.StartInfo = procInfo;

            p.Start();
        }
        #endregion

        #region SaveConfigInfo - システム設定情報保存
        /// <summary>
        /// Utilitiesクラスが保持しているシステム設定情報をシリアライズしてXMLファイルとして保存します。
        /// 保持しているシステム設定情報が無い場合、このメソッドは何もしません。
        /// </summary>
        public static void SaveConfigInfo()
        {
            SaveConfigInfo(Config);
        }

        /// <summary>
        /// 渡されたシステム設定情報をシリアライズしてXMLファイルとして保存します。
        /// </summary>
        /// <param orderName="configInfo">保存するシステム設定情報</param>
        public static void SaveConfigInfo(ConfigInfo configInfo)
        {
            string path = ConfigurationManager.AppSettings[ConfigInfo.SAVE_PATH_KEY];
            XmlManager.Serialize<ConfigInfo>(path, configInfo);
        }
        #endregion

        #region RestoreConfigInfo - システム設定情報保存読込
        /// <summary>
        /// シリアライズされたシステム設定情報を読み込みます。
        /// 読み込んだ情報は、同時にConfigプロパティに設定されることに注意してください。
        /// </summary>
        public static ConfigInfo RestoreConfigInfo()
        {
            string path = ConfigurationManager.AppSettings[ConfigInfo.SAVE_PATH_KEY];
            if (!string.IsNullOrEmpty(path))
                Config = XmlManager.Deserialize<ConfigInfo>(path, true);
            else
                Config = new ConfigInfo();

            return Config;
        }
        #endregion

        #region SaveStateInfo - 状態情報保存
        /// <summary>
        /// Utilitiesクラスが保持している状態情報をシリアライズしてXMLファイルとして保存します。
        /// 保持している状態情報が無い場合、このメソッドは何もしません。
        /// </summary>
        public static void SaveStateInfo()
        {
            SaveStateInfo(State);
        }

        /// <summary>
        /// 渡された状態情報をシリアライズしてXMLファイルとして保存します。
        /// </summary>
        /// <param orderName="stateInfo">保存する状態情報</param>
        public static void SaveStateInfo(StateInfo stateInfo)
        {
            string path = ConfigurationManager.AppSettings[StateInfo.SAVE_PATH_KEY];
            XmlManager.Serialize<StateInfo>(path, stateInfo);
        }
        #endregion

        #region RestoreStateInfo - 状態情報保存読込
        /// <summary>
        /// シリアライズされた状態情報を読み込みます。
        /// 読み込んだ情報は、同時にStateプロパティに設定されることに注意してください。
        /// </summary>
        public static StateInfo RestoreStateInfo()
        {
            string path = ConfigurationManager.AppSettings[StateInfo.SAVE_PATH_KEY];
            if (!string.IsNullOrEmpty(path))
                State = XmlManager.Deserialize<StateInfo>(path, true);
            else
                State = new StateInfo();

            return State;
        }
        #endregion

        #region SaveRenameInfo - ファイルリネーム情報保存
        /// <summary>
        /// Utilitiesクラスが保持しているファイルリネーム情報をシリアライズしてXMLファイルとして保存します。
        /// 保持しているファイルリネーム情報が無い場合、このメソッドは何もしません。
        /// </summary>
        public static void SaveRenameInfo()
        {
            SaveRenameInfo(Rename);
        }

        /// <summary>
        /// 渡されたファイルリネーム情報をシリアライズしてXMLファイルとして保存します。
        /// </summary>
        /// <param orderName="renameInfo">保存するファイルリネーム情報</param>
        public static void SaveRenameInfo(RenameInfo renameInfo)
        {
            string path = ConfigurationManager.AppSettings[RenameInfo.SAVE_PATH_KEY];
            XmlManager.Serialize<RenameInfo>(path, renameInfo);
        }
        #endregion

        #region RestoreRenameInfo - ファイルリネーム情報保存読込
        /// <summary>
        /// シリアライズされたファイルリネーム情報を読み込みます。
        /// 読み込んだ情報は、同時にRenameプロパティに設定されることに注意してください。
        /// </summary>
        public static RenameInfo RestoreRenameInfo()
        {
            string path = ConfigurationManager.AppSettings[RenameInfo.SAVE_PATH_KEY];
            if (!string.IsNullOrEmpty(path))
                Rename = XmlManager.Deserialize<RenameInfo>(path, true);
            else
                Rename = new RenameInfo();

            return Rename;
        }
        #endregion
    }
}
