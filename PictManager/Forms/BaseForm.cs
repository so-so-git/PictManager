using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.IO;
using SO.Library.Text;
using SO.PictManager.Common;
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;

using CursorFace = System.Windows.Forms.Cursor;
using Config = System.Configuration.ConfigurationManager;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 基底フォームクラス
    /// </summary>
    public partial class BaseForm : Form
    {
        #region クラス定数

        #endregion

        #region インスタンス変数

        /// <summary>表示対象画像のリスト</summary>
        private List<IImage> _imageList = new List<IImage>();

        /// <summary>現在選択しているファイルのインデックス</summary>
        private int _currentIdx = 0;

        /// <summary>表示対象ディレクトリのパス</summary>
        private DirectoryInfo _targetDir;

        /// <summary>表示対象カテゴリー</summary>
        private MstCategory _targetCategory;

        /// <summary>サブフォルダ包含フラグ</summary>
        private bool _includeSubFlg = false;

        #endregion

        #region プロパティ

        /// <summary>
        /// 画像モードを取得・設定します。
        /// </summary>
        internal ConfigInfo.ImageDataMode ImageMode { get; set; }

        /// <summary>
        /// 表示対象ディレクトリのパスを取得・設定します。
        /// </summary>
        [Browsable(false)]
        protected internal DirectoryInfo TargetDirectory
        {
            get { return _targetDir; }
            protected set { _targetDir = value; }
        }

        /// <summary>
        /// 表示対象カテゴリーを取得・設定します。
        /// </summary>
        [Browsable(false)]
        protected internal MstCategory TargetCategory
        {
            get { return _targetCategory; }
            protected set { _targetCategory = value; }
        }

        /// <summary>
        /// 表示対象画像のリストを取得します。
        /// </summary>
        [Browsable(false)]
        protected internal List<IImage> ImageList
        {
            get { return _imageList; }
            protected set { _imageList = value; }
        }

        /// <summary>
        /// 表示対象画像の数を取得します。
        /// </summary>
        [Browsable(false)]
        internal int ImageCount
        {
            get { return _imageList.Count; }
        }

        /// <summary>
        /// 現在表示中のファイルのインデックスを取得または設定します。
        /// 無効なインデックスを指定した場合はエラーメッセージが表示され、指定がキャンセルされます。
        /// </summary>
        [Browsable(false)]
        internal int CurrentIndex
        {
            get { return _currentIdx; }
            set
            {
                try
                {
                    if (value < 0)
                    {
                        FormUtilities.ShowMessage("W002");
                        return;
                    }
                    if (value > _imageList.Count - 1)
                    {
                        FormUtilities.ShowMessage("W003");
                        return;
                    }
                    _currentIdx = value;
                }
                catch (Exception ex)
                {
                    ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                }
            }
        }

        /// <summary>
        /// サブフォルダ包含フラグを取得・設定します。
        /// </summary>
        protected internal bool IncludeSubFlg
        {
            get { return _includeSubFlg; }
            protected set { _includeSubFlg = value; }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        protected BaseForm()
        {
            // コンポーネント初期化
            InitializeComponent();

            // メニューバー作成
            CreateMenu();
        }

        /// <summary>
        /// ファイルモード用のコンストラクタです。
        /// </summary>
        /// <param name="targetPath">対象ディレクトリパス</param>
        /// <param name="includeSubFlg">サブディレクトリ処理フラグ</param>
        public BaseForm(string targetPath, bool includeSubFlg)
        {
            // コンポーネント初期化
            InitializeComponent();

            // フィールド初期化
            ImageMode = ConfigInfo.ImageDataMode.File;
            if (targetPath != null)
                _targetDir = new DirectoryInfo(targetPath);

            _includeSubFlg = includeSubFlg;

            // メニューバー作成
            CreateMenu();
        }

        /// <summary>
        /// データベースモード用のコンストラクタです。
        /// </summary>
        /// <param name="category">対象カテゴリー</param>
        public BaseForm(MstCategory category)
        {
            // コンポーネント初期化
            InitializeComponent();

            // フィールド初期化
            ImageMode = ConfigInfo.ImageDataMode.Database;
            _targetCategory = category;

            // メニューバー作成
            CreateMenu();
        }

        #endregion

        #region CreateMenu - メニューバー作成

        /// <summary>
        /// 空のメニューバーを生成します。
        /// 継承フォームでメニューバーを実装する場合は、
        /// このメソッドをオーバーライドして下さい。
        /// </summary>
        protected virtual void CreateMenu() { }

        #endregion

        #region ViewDeletedFiles - 一時退避ディレクトリをエクスプローラで閲覧

        /// <summary>
        /// 削除ディレクトリの内容をエクスプローラで表示します。
        /// </summary>
        protected void ViewDeletedFiles()
        {
            try
            {
                // 一時退避ディレクトリ存在確認
                string storeDir = Path.Combine(EntryPoint.TmpDirPath, Constants.STORE_DIR_NAME);
                if (!Directory.Exists(storeDir))
                {
                    FormUtilities.ShowMessage("I006");
                    return;
                }

                // explorerで一時退避ディレクトリを開く
                var process = new Process();
                process.StartInfo.FileName = "explorer";
                process.StartInfo.Arguments = storeDir;
                process.StartInfo.UseShellExecute = false;
                process.Start();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region RenameFile - 対象ファイル名変更

        /// <summary>
        /// 表示中ファイルのファイル名を変更します。
        /// </summary>
        /// <returns>処理結果</returns>
        public ResultStatus RenameFile()
        {
            var status = ResultStatus.Empty;
            string oldFilePath = _imageList[CurrentIndex].Key;

            try
            {

                // 対象ファイル存在チェック
                if (!File.Exists(_imageList[CurrentIndex].Key))
                {
                    FormUtilities.ShowMessage("E003");
                    return status = ResultStatus.Error;
                }

                // 変更後ファイル名取得
                string newName = null;
                using (var inputDlg = new CommonInputDialog(
                    "変更後ファイル名入力", "変更後ファイル名：", true,
                    Path.GetFileName(oldFilePath)))
                {
                    if (inputDlg.ShowDialog(this) != DialogResult.OK)
                    {
                        status = ResultStatus.Cancel;
                        return status;
                    }
                    newName = inputDlg.InputString;
                }

                // 禁則文字チェック
                if (newName.HasInvalidPathChar())
                {
                    FormUtilities.ShowMessage("W005");
                    return status = ResultStatus.Error;
                }

                // パス区切文字チェック
                if (newName.IndexOf(Path.DirectorySeparatorChar) != -1)
                {
                    FormUtilities.ShowMessage("W016");
                    return status = ResultStatus.Error;
                }

                // 拡張子チェック
                string oldExt = Path.GetExtension(oldFilePath);
                if (Path.GetExtension(newName) != oldExt)
                    newName += oldExt;

                // 変更後ファイルパス組立
                string newPath = Path.Combine(
                        Path.GetDirectoryName(oldFilePath), newName);

                // ファイル名変更
                var newFile = new FileInfo(oldFilePath);
                newFile.MoveTo(newPath);
                _imageList[CurrentIndex].Key = newPath;

                // 処理ログ出力
                var sb = new StringBuilder();
                sb.Append("[MOVE]");
                sb.Append(_imageList[CurrentIndex]);
                sb.Append(" -> ");
                sb.Append(newPath);

                Utilities.Logger.WriteLog(GetType().FullName, MethodBase.GetCurrentMethod().Name, sb.ToString());

                status = ResultStatus.OK;
            }
            catch (Exception ex)
            {
                status = ResultStatus.Error;
                string optionMsg = ex is IOException ?
                        MessageXml.GetMessageInfo("E001", oldFilePath).message : null;
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod(), optionMsg);
            }
            finally
            {
                if (status == ResultStatus.OK)
                    // 終了通知
                    FormUtilities.ShowMessage("I001");
            }

            return status;
        }

        #endregion

        #region RenameAllFiles - 対象ファイルリスト一括ファイル名変更

        /// <summary>
        /// 表示中ディレクトリ内の全ファイルのファイル名を一括で変更します。
        /// </summary>
        /// <returns>処理結果</returns>
        protected ResultStatus RenameAllFiles()
        {
            var status = ResultStatus.Empty;
            int idx = 0;

            try
            {
                // 前回のリネーム情報を復元してリネームダイアログを表示
                var renameDlg = new RenameDialog(Utilities.RestoreRenameInfo());

                // リネーム情報取得
                RenameInfo renameInfo = null;
                try
                {
                    if (renameDlg.ShowDialog(this) != DialogResult.OK)
                    {
                        status = ResultStatus.Cancel;
                        return status;
                    }
                    renameInfo = renameDlg.GetRenameInfo();
                }
                finally
                {
                    if (renameDlg != null)
                    {
                        renameDlg.Dispose();
                        Update();
                    }
                }

                // ファイルリネーム情報保管
                Utilities.SaveRenameInfo(renameInfo);

                // マウスカーソル変更(待機)
                CursorFace.Current = Cursors.WaitCursor;

                // プログレスダイアログ表示
                using (var progDlg = new ProgressDialog(this))
                {
                    MessageInfo msgInfo = MessageXml.GetMessageInfo("I002", string.Empty);
                    progDlg.StartProgress(
                            msgInfo.caption, msgInfo.message, 0, _imageList.Count);

                    // シャッフルがON場合はシャッフル最新化、OFFの場合は通常の最新化
                    if (renameInfo.IsShuffle)
                    {
                        ShuffleFiles();
                    }
                    else
                    {
                        RefreshImageList();

                        // ファイル名ソート
                        if (renameInfo.SortOrder.HasValue)
                            _imageList = ImageSorter.Sort(_imageList, renameInfo.SortOrder.Value, ImageMode).ToList();
                    }

                    // 変換前後パス格納リストを確保
                    var pathPare = Enumerable.Range(0, 0).Select(x =>
                            new { OriginalPath = string.Empty, TempPath = string.Empty }).ToList();

                    // ディレクトリ単位のファイルカウントマップ確保
                    var countMap = new Dictionary<string, int>();

                    // ファイル名重複を避ける為、仮名称に一時一次変換
                    StringBuilder pathBuilder;
                    string parentBuf = _imageList.Count == 0 ? null : Path.GetDirectoryName(_imageList[0].Key);
                    int fileCnt = 0;
                    for (idx = 0; idx < _imageList.Count; ++idx)
                    {
                        string oldFilePath = _imageList[idx].Key;
                        var target = new FileInfo(oldFilePath);
                        string parentPath = Path.GetDirectoryName(oldFilePath);
                        progDlg.Message = MessageXml.GetMessageInfo("I002", parentPath).message;

                        pathBuilder = new StringBuilder(parentPath);
                        pathBuilder.Append(Path.DirectorySeparatorChar);
                        pathBuilder.Append("_");
                        pathBuilder.Append(Path.GetFileName(oldFilePath));
                        File.Move(oldFilePath, pathBuilder.ToString());

                        // 変換前後のパスを保存
                        pathPare.Add(new { OriginalPath = oldFilePath, TempPath = pathBuilder.ToString() });

                        // ディレクトリ単位のファイルカウント処理
                        if (parentPath == parentBuf)
                        {
                            ++fileCnt;
                        }
                        else
                        {
                            // 親ディレクトリ変化
                            countMap.Add(parentBuf, fileCnt);
                            parentBuf = parentPath;
                            fileCnt = 1;
                        }

                        // プログレスバー更新
                        progDlg.PerformStep();
                    }
                    // 最終ファイルの属するディレクトリのカウントをマップに追加
                    countMap.Add(parentBuf, fileCnt);
                    progDlg.CurrentValue = progDlg.ProgressMaximum;

                    // プログレスダイアログ初期化
                    progDlg.CurrentValue = progDlg.ProgressMinimun;
                    progDlg.InitializeProgressbar(0, pathPare.Count);

                    // ファイル名一括変更
                    parentBuf = string.Empty;
                    for (idx = 0; idx < pathPare.Count; ++idx)
                    {
                        // プログレスメッセージ更新
                        progDlg.Message = MessageXml.GetMessageInfo("I003", pathPare[idx].OriginalPath).message;

                        pathBuilder = new StringBuilder();

                        // 拡張子取得
                        string ext = Path.GetExtension(pathPare[idx].OriginalPath);

                        // 親ディレクトリパスを取得
                        string parentPath = Path.GetDirectoryName(pathPare[idx].OriginalPath);

                        // 通し番号付加がONの場合
                        if (renameInfo.IsAddSequential)
                        {
                            // 親ディレクトリ変化
                            if (parentPath != parentBuf)
                            {
                                parentBuf = parentPath;
                                fileCnt = 0;
                            }

                            // 総ファイル数に合わせて0埋めで桁揃えを行う
                            pathBuilder.Append(StringUtilities.PaddingByZero(
                                    fileCnt++ * renameInfo.IncrementStep.Value,
                                    ((countMap[parentPath] - 1) * renameInfo.IncrementStep).ToString().Length));
                        }

                        if (renameInfo.IsReserveOriginalName)
                        {
                            // 元ファイル名を含めるがONの場合
                            string withoutExt = Path.GetFileNameWithoutExtension(pathPare[idx].OriginalPath);
                            // 置換前文字がある場合は置換実施
                            if (renameInfo.ReplaceBefore.BlankToNull() != null)
                                withoutExt = withoutExt.Replace(renameInfo.ReplaceBefore, renameInfo.ReplaceAfter);

                            if (renameInfo.OriginalPosition == OriginalPosition.Before)
                            {
                                // 元ファイル名が通し番号の前の場合
                                pathBuilder.Insert(0, withoutExt);
                                pathBuilder.Insert(withoutExt.Length, renameInfo.SeqDelimiter ?? string.Empty);
                            }
                            else
                            {
                                // 元ファイル名が通し番号の後の場合
                                pathBuilder.Append(renameInfo.SeqDelimiter ?? string.Empty);
                                pathBuilder.Append(withoutExt);
                            }
                        }

                        // 接頭文字、接尾文字を追加
                        pathBuilder.Insert(0, renameInfo.Prefix ?? string.Empty);
                        pathBuilder.Append(renameInfo.Suffix ?? string.Empty);

                        if (renameInfo.IsAddParentDirName)
                        {
                            // 親ディレクトリ名を先頭に挿入するがONの場合
                            string parentName = new DirectoryInfo(parentPath).Name;
                            pathBuilder.Insert(0, parentName);
                            pathBuilder.Insert(parentName.Length, renameInfo.DirDelimiter);
                        }

                        // 拡張子を追加
                        pathBuilder.Append(ext);

                        // ディレクトリパスを追加
                        pathBuilder.Insert(0, parentPath);
                        pathBuilder.Insert(parentPath.Length, Path.DirectorySeparatorChar);

                        // ファイル名変更
                        File.Move(pathPare[idx].TempPath, pathBuilder.ToString());

                        // プログレスバー更新
                        progDlg.PerformStep();
                    }
                }

                status = ResultStatus.OK;
            }
            catch (Exception ex)
            {
                status = ResultStatus.Error;
                string optionMsg = ex is IOException ?
                        MessageXml.GetMessageInfo("E001", _imageList[idx].Key).message : null;
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod(), optionMsg);
            }
            finally
            {
                // 対象ファイルリストをリフレッシュ
                RefreshImageList();

                // マウスカーソル変更(通常)
                CursorFace.Current = Cursors.Default;

                if (status == ResultStatus.OK)
                    // 終了通知
                    FormUtilities.ShowMessage("I001");
            }

            return status;
        }

        #endregion

        #region MoveFile - 対象ファイル移動

        /// <summary>
        /// 表示中ファイルを指定したディレクトリへ移動します。
        /// </summary>
        /// <returns>処理結果</returns>
        public ResultStatus MoveFile()
        {
            var status = ResultStatus.Empty;
            string oldFilePath = _imageList[CurrentIndex].Key;

            try
            {

                // 対象ファイル存在チェック
                if (!File.Exists(oldFilePath))
                {
                    FormUtilities.ShowMessage("E003");
                    return status = ResultStatus.Error;
                }

                // 移動先ディレクトリ取得
                string newPath = null;
                using (var dlg = new FolderBrowserDialog())
                {
                    dlg.ShowNewFolderButton = true;
                    dlg.RootFolder = Environment.SpecialFolder.MyComputer;
                    dlg.SelectedPath = Path.GetDirectoryName(oldFilePath);
                    dlg.Description = "移動先のディレクトリを選択して下さい。";
                    if (dlg.ShowDialog(this) != DialogResult.OK) return ResultStatus.Cancel;

                    // 変更後ファイルパス組立
                    newPath = Path.Combine(
                            dlg.SelectedPath, Path.GetFileName(oldFilePath));
                }

                // ファイル名変更
                var newFile = new FileInfo(oldFilePath);
                newFile.MoveTo(newPath);
                _imageList.RemoveAt(CurrentIndex);
                if (CurrentIndex > _imageList.Count - 2) CurrentIndex = 0;

                status = ResultStatus.OK;
            }
            catch (Exception ex)
            {
                status = ResultStatus.Error;
                string optionMsg = ex is IOException ?
                        MessageXml.GetMessageInfo("E001", oldFilePath).message : null;
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod(), optionMsg);
            }
            finally
            {
                if (status == ResultStatus.OK)
                    // 終了通知
                    FormUtilities.ShowMessage("I004");
            }

            return status;
        }

        #endregion

        #region MoveAllFiles - 対象ファイルを指定ディレクトリに移動

        /// <summary>
        /// 表示中ディレクトリ内の全ファイルを一括で指定したディレクトリへ移動します。
        /// </summary>
        /// <returns>処理結果</returns>
        protected ResultStatus MoveAllFiles()
        {
            var status = ResultStatus.Empty;
            string target = null;   // 処理対象ファイルパス
            try
            {
                // マウスカーソル変更(待機)
                CursorFace.Current = Cursors.WaitCursor;

                // 移動先ディレクトリを取得
                string destDir = null;
                using (var dlg = new FolderBrowserDialog())
                {
                    dlg.RootFolder = Environment.SpecialFolder.Desktop;
                    dlg.Description = "ファイルの移動先ディレクトリを指定して下さい。";
                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return status = ResultStatus.Cancel;
                    }
                    destDir = dlg.SelectedPath;
                }

                // 指定ディレクトリへファイルを移動
                foreach (var img in _imageList)
                {
                    target = img.Key;   // エラー処理用に対象ファイル名を保存
                    File.Move(img.Key, Path.Combine(destDir, Path.GetFileName(img.Key)));
                }

                status = ResultStatus.OK;
            }
            catch (Exception ex)
            {
                status = ResultStatus.Error;
                string optionMsg = ex is IOException ?
                        MessageXml.GetMessageInfo("E001", target).message : null;
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod(), optionMsg);
            }
            finally
            {
                // 対象ファイルリストをリフレッシュ
                RefreshImageList();

                // マウスカーソル変更(通常)
                CursorFace.Current = Cursors.Default;

                if (status == ResultStatus.OK)
                    // 終了通知
                    FormUtilities.ShowMessage("I004");
            }

            return status;
        }

        #endregion

        #region RefreshImageList - 表示対象画像リスト最新化

        /// <summary>
        /// 表示対象画像リストを最新の内容に更新します。
        /// </summary>
        protected virtual void RefreshImageList()
        {
            try
            {
                var imageList = new List<IImage>();
                
                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    if (_targetDir != null)
                    {
                        // 表示対象拡張子リストを取得
                        List<string> extentions = Utilities.Config.CommonInfo.TargetExtensions;

                        // サーチオプション設定
                        SearchOption opt = _includeSubFlg ?
                            SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                        // 対象ディレクトリ内の画像ファイルパスを全取得
                        foreach (var ext in extentions)
                        {
                            foreach (var file in Directory.GetFiles(_targetDir.FullName, "*." + ext, opt))
                            {
                                imageList.Add(new FileImage(file));
                            }
                        }
                    }
                }
                else
                {
                    // 画像テーブルから対象画像データを取得
                    using (var entity = new PictManagerEntities())
                    {
                        var imageIds = from i in entity.TblImages
                                       where i.CategoryId == TargetCategory.CategoryId
                                       select i.ImageId;

                        foreach (var imageId in imageIds)
                        {
                            imageList.Add(new DataImage(imageId));
                        }
                    }
                }

                _imageList.Clear();
                _imageList = imageList.OrderBy(i => i.Key).ToList();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region DoButtonClickWhenEnter - Enterキー押下時、ボタンClickイベント実行

        /// <summary>
        /// Enterキーの押下を検出し、指定されたボタンのClickイベントを発行します。
        /// </summary>
        /// <param name="e">キーイベント引数</param>
        /// <param name="target">Clickイベント発行対象のボタンオブジェクト</param>
        protected void DoButtonClickWhenEnter(KeyEventArgs e, Button target)
        {
            try
            {
                // 修飾キーが付加されている場合は通常処理
                if ((e.KeyCode & Keys.Alt) != Keys.Alt &&
                        (e.KeyCode & Keys.Control) != Keys.Control &&
                        (e.KeyCode & Keys.Shift) != Keys.Shift)
                {
                    Keys kcode = e.KeyCode & Keys.KeyCode;
                    switch (kcode)
                    {
                        case Keys.Enter:
                            // ボタンクリック実行
                            target.Focus();
                            target.PerformClick();
                            break;

                        default:
                            // 押下されたのがEnterキー以外の場合は処理無し
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region ShuffleFiles - 対象ファイルのインデックスをシャッフル

        /// <summary>
        /// 表示中ディレクトリ内のファイルのインデックスをシャッフルします。
        /// </summary>
        protected void ShuffleFiles()
        {
            int idx = 0;
            try
            {
                var targetFiles = new List<List<string>>();

                if (_targetDir == null)
                    targetFiles.Add(_imageList.Select(i => i.Key).ToList());
                else
                    // 二重リストでディレクトリ単位に対象ファイル群を取得
                    GetChildFiles(targetFiles, _targetDir.FullName, true);

                // 乱数を生成して対象ファイルを選択、ファイルリスト再構築
                var swapFiles = new List<string>();
                var rand = new Random();
                for (int listIdx = 0; listIdx < targetFiles.Count; ++listIdx)
                {
                    for (int max = targetFiles[listIdx].Count; max > 0; --max)
                    {
                        // 乱数で指定されたファイルを新しいリストへ移す
                        idx = rand.Next(max);
                        swapFiles.Add(targetFiles[listIdx][idx]);

                        // 移したファイルを除去
                        targetFiles[listIdx].RemoveAt(idx);
                    }
                }

                // 新しいリストで既存リストを上書き
                _imageList.Clear();
                foreach (var file in swapFiles)
                {
                    _imageList.Add(new FileImage(file));
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region GetChildFiles - 指定ディレクトリ以下の全ファイルリストを引数のリストに追加

        /// <summary>
        /// 指定ディレクトリ以下の全処理対象ファイルをListに追加します。
        /// Listには、更に子としてディレクトリ単位でファイルを保持したListが格納されます。
        /// 再帰検索フラグがtrueの場合、サブディレクトリ内を再帰的に検索し、その配下のファイルも追加対象とします。
        /// </summary>
        /// <param name="fileList">取得したファイルを追加するList</param>
        /// <param name="dirPath">検索対象ディレクトリパス</param>
        /// <param name="reflexive">再帰検索フラグ</param>
        protected void GetChildFiles(List<List<string>> fileList, string dirPath, bool reflexive)
        {
            try
            {
                // 表示対象拡張子リストを取得
                List<string> extentions = Utilities.Config.CommonInfo.TargetExtensions;

                // 対象ディレクトリ内の画像ファイルパスを全取得
                var files = new List<string>();
                foreach (var ext in extentions)
                    files.AddRange(Directory.GetFiles(dirPath, "*." + ext));
                files.Sort();

                // 引数として渡されたリストに作成したリストを追加
                fileList.Add(files);

                if (reflexive)
                {
                    // 再帰探索フラグがONの場合はサブディレクトリ内を再探索
                    DirectoryInfo[] subDirs = new DirectoryInfo(dirPath).GetDirectories();
                    foreach (var subDir in subDirs)
                        GetChildFiles(fileList, subDir.FullName, reflexive);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region SearchFileIndex - 指定ファイルの、対象ファイルリスト内におけるインデックスを検索

        /// <summary>
        /// 指定されたファイルのインデックスを取得します。
        /// 指定されたファイルが存在しない場合や、処理中に例外が発生した場合は-1が返されます。
        /// </summary>
        /// <param name="searchPath">インデックス検索対象ファイルのパス</param>
        /// <returns>ファイルパスのインデックス</returns>
        protected int SearchFileIndex(string searchPath)
        {
            int idx = -1;
            try
            {
                idx = _imageList.FindIndex(x => x.Key == searchPath);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                // エラー時は検索結果無しと同様の扱いとする
                idx = -1;
            }

            return idx;
        }

        #endregion

        #region CloseForm - フォームをクローズ

        /// <summary>
        /// フォームをクローズします。
        /// </summary>
        protected virtual void CloseForm()
        {
            // 継承クラスでのオーバーライド用に用意
        }

        #endregion

        #region Form_KeyDown - フォーム上でのキー押下時

        /// <summary>
        /// フォーム上でキーが押下された際に実行される処理です。
        /// 特殊なキーが押下された場合に固有の処理を実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void Form_KeyDown(object sender, KeyEventArgs e)
        {
            // 継承クラスでのオーバーライド用に用意
        }

        #endregion
    }
}
