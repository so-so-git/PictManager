using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.IO;
using SO.Library.Text;
using SO.PictManager.Common;
using SO.PictManager.DataModel;
using SO.PictManager.Forms;
using SO.PictManager.Forms.FileSystem;
using SO.PictManager.Forms.Info;

namespace SO.PictManager
{
    /// <summary>
    /// アプリケーション開始クラス
    /// </summary>
    public static class EntryPoint
    {
        #region クラス変数宣言・定義

        /// <summary>オプション：暗号化モード</summary>
        private const string PARAM_CRYPT_MODE = "-c";

        /// <summary>オプション：指定ディレクトリリスト表示</summary>
        private const string PARAM_VIEW_LIST = "-dl";

        /// <summary>オプション：指定ディレクトリサムネイル表示</summary>
        private const string PARAM_VIEW_THUMBNAIL = "-dt";

        /// <summary>オプション：指定ディレクトリスライド表示</summary>
        private const string PARAM_VIEW_SLIDE = "-ds";

        /// <summary>オプション：指定単一画像表示</summary>
        private const string PARAM_VIEW_IMAGE = "-f";

        /// <summary>一時ディレクトリパス</summary>
        internal static readonly string _tmpDirPath;

        /// <summary>SQLServerサービス名</summary>
        private static readonly string _sqlServiceName;

        /// <summary>終了時にSQLServerサービスを停止するかのフラグ</summary>
        private static bool _isSqlServiceNonStop = false;

        #endregion

        #region 静的コンストラクタ

        /// <summary>
        /// 静的コンストラクタです。
        /// </summary>
        static EntryPoint()
        {
            // 削除ファイル一時ディレクトリパス生成
            _tmpDirPath = Path.Combine(
                    Path.GetTempPath(), typeof(EntryPoint).Assembly.GetName().Name);

            // SQLServerサービス名取得
            _sqlServiceName = ConfigurationManager.AppSettings["SqlServiceName"];

            // メッセージ定義ファイル初期化
            MessageXml.MessageFilePath = ConfigurationManager.AppSettings["MessageFilePath"];
        }

        #endregion

        #region Main - メインエントリポイント

        /// <summary>
        /// アプリケーションのメインエントリポイントです。
        /// </summary>
        /// <param orderName="args">コマンドライン引数</param>
        [STAThread]
        public static void Main(string[] args)
        {
            if (Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database)
            {
                // SQLServerサービス開始
                StartSQLServerService();

                // 未分類カテゴリの初期登録
                EntryUnClassifiedCategory();
            }

            // 概観設定
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // 多重起動抑制
            var mutex = new Mutex(false, Assembly.GetExecutingAssembly().GetName().Name);
            if (!mutex.WaitOne(0, false))
            {
                FormUtilities.ShowMessage("W023");
                return;
            }
#if DEBUG
            // デバッグモードでビルドされたモジュールの為、警告表示
            FormUtilities.ShowMessage("W026", "Debug");
#endif
            try
            {
                if (args.Any())
                {
                    // コマンドライン引数を解析して実行
                    ParseCommandLine(args);
                }
                else
                {
                    // 対象ディレクトリ選択フォームを表示
                    Application.Run(new StartForm());
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(typeof(EntryPoint).FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                try
                {
                    if (Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.File)
                    {
                        // 一時ディレクトリ削除
                        if (Directory.Exists(_tmpDirPath))
                        {
                            // 読み取り専用属性解除
                            (from f in Directory.GetFiles(_tmpDirPath)
                                where (File.GetAttributes(f) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly
                                select new FileInfo(f)
                            ).ToList().ForEach(f => f.IsReadOnly = false);

                            Directory.Delete(_tmpDirPath, true);
                        }
                    }
                    else
                    {
                        // SQLServerサービス停止
                        StopSQLServerService();
                    }

                    mutex.ReleaseMutex();
                }
                catch (Exception ex)
                {
                    ex.DoDefault(typeof(EntryPoint).FullName, MethodBase.GetCurrentMethod());
                }
            }
        }

        #endregion

        #region ParseCommandLine - コマンドライン解析

        /// <summary>
        /// コマンドラインの解析を行い、適切なモードでプログラムを起動します。
        /// </summary>
        /// <param orderName="args">コマンドライン引数</param>
        private static void ParseCommandLine(string[] args)
        {
            for (int i = 0; i < args.Length; ++i)
            {
                switch (args[i])
                {
                    case PARAM_CRYPT_MODE:      // 暗号化モードで起動
                        Application.Run(new StartForm());

                        return;

                    case PARAM_VIEW_LIST:       // 引数のディレクトリを一覧表示
                        if (i + 1 >= args.Length)
                        {
                            FormUtilities.ShowMessage("E005");
                            return;
                        }

                        string listPath = args[i + 1];
                        if (Directory.Exists(listPath))
                            Application.Run(new FileListForm(listPath, false));
                        else
                            FormUtilities.ShowMessage("E006");

                        return;

                    case PARAM_VIEW_THUMBNAIL:  // 引数のディレクトリをサムネイル表示
                        if (i + 1 >= args.Length)
                        {
                            FormUtilities.ShowMessage("E005");
                            return;
                        }

                        string thumbnailPath = args[i + 1];
                        if (Directory.Exists(thumbnailPath))
                            Application.Run(new FileThumbnailForm(thumbnailPath, false));
                        else
                            FormUtilities.ShowMessage("E006");

                        return;

                    case PARAM_VIEW_SLIDE:      // 引数のディレクトリをスライドショー表示
                        if (i + 1 >= args.Length)
                        {
                            FormUtilities.ShowMessage("E005");
                            return;
                        }

                        string slidePath = args[i + 1];
                        if (Directory.Exists(slidePath))
                            Application.Run(new FileSlideForm(slidePath, false));
                        else
                            FormUtilities.ShowMessage("E006");

                        return;

                    case PARAM_VIEW_IMAGE:      // 引数のファイルを単一画像表示
                        if (i + 1 >= args.Length)
                        {
                            FormUtilities.ShowMessage("E005");
                            return;
                        }

                        string viewFilePath = args[i + 1];
                        if (Utilities.IsAvailableFormat(viewFilePath, true))
                            Application.Run(new FileViewImageForm(null, viewFilePath));
                        else
                            FormUtilities.ShowMessage("E007", viewFilePath);

                        return;

                    default:
                        break;
                }
            }
        }

        #endregion

        #region StartSQLServerService - SQLServerサービス開始

        /// <summary>
        /// SQLServerのサービスを開始します。
        /// </summary>
        private static void StartSQLServerService()
        {
            var sc = new ServiceController(_sqlServiceName);
            if (sc.Status == ServiceControllerStatus.Stopped)
            {
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
            else if (sc.Status == ServiceControllerStatus.Paused)
            {
                sc.Continue();
                sc.WaitForStatus(ServiceControllerStatus.Running);
            }
            else if (sc.Status == ServiceControllerStatus.Running)
            {
                _isSqlServiceNonStop = true;
            }
        }

        #endregion

        #region StopSQLServerService - SQLServerサービス停止

        /// <summary>
        /// SQLServerのサービスを停止します。
        /// </summary>
        private static void StopSQLServerService()
        {
            if (!_isSqlServiceNonStop)
            {
                var sc = new ServiceController(_sqlServiceName);
                if (sc.CanStop)
                {
                    sc.Stop();
                    sc.WaitForStatus(ServiceControllerStatus.Stopped);
                }
            }
        }

        #endregion

        #region EntryUnClassifiedCategory - 未分類カテゴリー登録

        /// <summary>
        /// 未分類カテゴリーが登録されていない場合、それを登録します。
        /// </summary>
        private static void EntryUnClassifiedCategory()
        {
            using (var entity = new PictManagerEntities())
            {
                var isExistsUnClassified = (from c in entity.MstCategories
                                            where c.CategoryName == Constants.UN_CLASSIFIED_CATEGORY_NAME
                                            select c).Any();

                // 登録済みの場合は処理不要
                if (isExistsUnClassified) return;

                // 現在の最大IDを取得
                int maxId;
                if (entity.MstCategories.Any())
                {
                    maxId = (from c in entity.MstCategories
                             select c.CategoryId).Max();
                }
                else
                {
                    maxId = 0;
                }

                // ID振り直し
                entity.Database.ExecuteSqlCommand(
                    "DBCC CHECKIDENT('MstCategories', RESEED, "
                    + (Constants.UN_CLASSIFIED_CATEGORY_ID) + ");");
                entity.SaveChanges();

                // 未分類カテゴリを登録
                DateTime now = DateTime.Now;
                var dto = new MstCategory();
                dto.CategoryName = Constants.UN_CLASSIFIED_CATEGORY_NAME;
                dto.InsertedDateTime = now;
                dto.UpdatedDateTime = now;

                entity.MstCategories.Add(dto);
                entity.SaveChanges();

                // IDを戻す
                entity.Database.ExecuteSqlCommand(
                    "DBCC CHECKIDENT('MstCategories', RESEED, " + maxId + ");");
                entity.SaveChanges();
            }
        }

        #endregion
    }
}
