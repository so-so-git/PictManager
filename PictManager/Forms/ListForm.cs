using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.Forms.Extensions;
using SO.Library.IO;
using SO.Library.Text;
using SO.PictManager.Common;
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;

using CursorFace = System.Windows.Forms.Cursor;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// ファイル一覧表示フォームクラス
    /// </summary>
    public partial class ListForm : BaseForm
    {
        #region クラス定数

        /// <summary>グリッド列インデックス：選択チェック</summary>
        private const int IDX_SEL_CHK = 0;
        /// <summary>グリッド列インデックス：ファイル名</summary>
        private const int IDX_FILE_NAME = 1;
        /// <summary>グリッド列インデックス：ディレクトリパス</summary>
        private const int IDX_DIR_PATH = 2;
        /// <summary>グリッド列インデックス：ディレクトリ参照ボタン</summary>
        private const int IDX_REF_BTN = 3;
        /// <summary>グリッド列インデックス：MD5</summary>
        private const int IDX_MD5 = 4;
        /// <summary>グリッド列インデックス：類似画像検索ボタン</summary>
        private const int IDX_SIMILAR_BTN = 5;

        #endregion

        #region インスタンス変数

        /// <summary>編集フラグ</summary>
        private int _changeCnt = 0;

        /// <summary>行の画像と類似画像のマッピング</summary>
        private Dictionary<int, List<IImage>> _similarMap;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// ファイルモード用のコンストラクタです。
        /// </summary>
        /// <param name="targetPath">対象ディレクトリパス</param>
        /// <param name="includeSubFlg">処理対象にサブディレクトリ以下を含むかを示すフラグ</param>
        public ListForm(string targetPath, bool includeSubFlg)
                : base(targetPath, includeSubFlg)
        {
            try
            {
                // コンポーネント初期化
                InitializeComponent();

                // 共通処理
                ConstructCommon();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
            }
        }

        /// <summary>
        /// データベースモード用のコンストラクタです。
        /// </summary>
        /// <param name="category">対象カテゴリー</param>
        public ListForm(MstCategory category)
            : base(category)
        {
            try
            {
                // コンポーネント初期化
                InitializeComponent();

                // 共通処理
                ConstructCommon();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
            }
        }

        #endregion

        #region ConstructCommon - 共通コンストラクション

        /// <summary>
        /// インスタンス構築時の共通処理を実行します。
        /// </summary>
        private void ConstructCommon()
        {
            // ファイル取得
            RefreshImageList();

            // グリッド列生成
            CreateColumns();

            // グリッドセル生成
            CreateCells();

            // ステータスバー更新
            lblStatus.Text = string.Empty;
            lblFileCount.Text = ImageList.Count + " files.";
        }

        #endregion

        #region CreateMenu - メニューバー作成
        /// <summary>
        /// (BaseForm.CreateMenu()をオーバーライドします)
        /// メニューバーを生成します。
        /// </summary>
        protected override void CreateMenu()
        {
            // ファイル
            var menuTemp = new ToolStripMenuItem("ファイル(&F)", null, null, "menuFile");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.F;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("戻る", null, btnClose_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("対象ファイル再取得", null, menuRefresh_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("一括ファイル名変更", null, menuRenameAll_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("一括ファイル移動", null, menuMoveAll_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("削除済画像確認", null,
                    (s, e) => ViewDeletedFiles()));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("ディレクトリを開く", null, (s, e) => Utilities.OpenExplorer(TargetDirectory.FullName)));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("終了", null,
                    (s, e) => Form_FormClosing(s, new FormClosingEventArgs(CloseReason.UserClosing, false))));
            barMenu.Items.Add(menuTemp);

            // 操作
            menuTemp = new ToolStripMenuItem("操作(&O)", null, null, "menuOpe");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.O;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("全ての変更を適用", null, btnApply_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("全ての変更を戻す", null, btnRevertSelection_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("重複しているファイルのみを抽出", null, menuFilterDuplicated_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("全ての行の類似画像を検索", null, menuSearchSimilarAll_Click));
            barMenu.Items.Add(menuTemp);

            // 表示
            menuTemp = new ToolStripMenuItem("表示(&V)", null, null, "menuView");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.V;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択行の画像を表示", null, menuViewImage_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択行の類似画像を表示", null, menuSimilar_Click));
            barMenu.Items.Add(menuTemp);
        }
        #endregion

        #region CreateColumns - グリッド列生成
        /// <summary>
        /// ファイル一覧グリッドの列を生成します。
        /// </summary>
        protected virtual void CreateColumns()
        {
            // 選択チェックボックス列
            var colChk = new DataGridViewCheckBoxColumn();
            colChk.HeaderText = "選択";
            colChk.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colChk.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdFiles.Columns.Add(colChk);

            // ファイル名列
            var colFile = new DataGridViewTextBoxColumn();
            colFile.HeaderText = "ファイル名";
            colFile.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdFiles.Columns.Add(colFile);

            // 親ディレクトリパス列
            var colDir = new DataGridViewTextBoxColumn();
            colDir.HeaderText = "ディレクトリ";
            colDir.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            colDir.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdFiles.Columns.Add(colDir);

            // ディレクトリ参照ボタン列
            var colRef = new DataGridViewButtonColumn();
            colRef.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colRef.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdFiles.Columns.Add(colRef);

            // MD5列
            var colMD5 = new DataGridViewTextBoxColumn();
            colMD5.HeaderText = "MD5";
            colMD5.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colMD5.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdFiles.Columns.Add(colMD5);

            // 類似画像検索ボタン列
            var colSimilar = new DataGridViewButtonColumn();
            colSimilar.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colSimilar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdFiles.Columns.Add(colSimilar);
        }
        #endregion

        #region CreateCells - グリッドセル生成

        /// <summary>
        /// ファイル一覧グリッドのセルを生成します。
        /// </summary>
        protected virtual void CreateCells()
        {
            grdFiles.SuspendLayout();

            int i = 0;
            var rowList = new List<DataGridViewRow>(ImageList.Count);
            foreach (var img in ImageList)
            {
                var row = new DataGridViewRow();
                var filePath = img.Key;
                bool delFlg = false;
                if (img.IsDeleted)
                {
                    row.DefaultCellStyle.BackColor = Color.LightGray;
                    delFlg = true;
                }
                row.Tag = delFlg;    // タグを削除済みフラグとして使用
                row.ReadOnly = delFlg;
                row.HeaderCell.Value = (++i).ToString();
                row.HeaderCell.Style.Font = new Font(this.Font, FontStyle.Regular);

                // 選択チェックボックスセル
                var celChk = new DataGridViewCheckBoxCell();
                celChk.Value = false;
                celChk.Tag = false;
                row.Cells.Add(celChk);

                // ファイル名セル
                var celFile = new DataGridViewTextBoxCell();
                celFile.Value = Path.GetFileName(filePath);
                celFile.Tag = celFile.Value;
                row.Cells.Add(celFile);

                // 親ディレクトリパスセル
                var celDir = new DataGridViewTextBoxCell();
                celDir.Value = Path.GetDirectoryName(filePath);
                celDir.Tag = celDir.Value;
                row.Cells.Add(celDir);

                DataGridViewCell celRef;
                DataGridViewCell celSimilar;
                DataGridViewCell celMD5 = new DataGridViewTextBoxCell();
                if (delFlg)
                {
                    // ディレクトリ参照ボタンセル
                    celRef = new DataGridViewTextBoxCell();
                    celRef.Value = string.Empty;

                    // MD5セル
                    celMD5.Value = string.Empty;

                    // 類似画像検索ボタンセル
                    celSimilar = new DataGridViewTextBoxCell();
                    celSimilar.Value = string.Empty;
                }
                else
                {
                    // ディレクトリ参照ボタンセル
                    celRef = new DataGridViewButtonCell();
                    celRef.Value = "...";

                    // MD5セル
                    celMD5.Value = Cryptgrapher.GetFileMD5(filePath);

                    // 類似画像検索ボタンセル
                    celSimilar = new DataGridViewButtonCell();
                    celSimilar.Value = "類似";
                }
                row.Cells.Add(celRef);
                row.Cells.Add(celMD5);
                row.Cells.Add(celSimilar);

                celMD5.ReadOnly = true;
                celRef.ReadOnly = delFlg;
                celSimilar.ReadOnly = delFlg;

                if (!delFlg)
                {
                    // MD5重複チェック
                    for (int j = 0; j < grdFiles.RowCount; ++j)
                    {
                        if (Convert.ToBoolean(grdFiles[IDX_SEL_CHK, j].Tag)) continue;

                        DataGridViewCell targetCell = grdFiles[IDX_MD5, j];
                        if (celMD5.Value.Equals(targetCell.Value))
                        {
                            celMD5.Style.BackColor = Color.Bisque;
                            targetCell.Style.BackColor = Color.Bisque;
                            break;
                        }
                    }
                }

                rowList.Add(row);
            }

            grdFiles.Rows.AddRange(rowList.ToArray());
            grdFiles.ResumeLayout();
        }

        #endregion

        #region GetImagePath - 画像ファイルパス取得

        /// <summary>
        /// 指定された行の画像ファイルのパスを取得します。
        /// </summary>
        /// <param name="rowIdx">指定行</param>
        /// <returns>画像ファイルのパス</returns>
        private string GetImagePath(int rowIdx)
        {
            string dirName = grdFiles[IDX_DIR_PATH, rowIdx].Tag.ToSafeString();
            string fileName = grdFiles[IDX_FILE_NAME, rowIdx].Tag.ToSafeString();

            return Path.Combine(dirName, fileName);
        }

        #endregion

        #region IsValidValue - 入力内容検証

        /// <summary>
        /// セルに入力された内容の妥当性を検証します。
        /// </summary>
        /// <param name="cell">検証対象のDetaGridViewCell</param>
        /// <returns>検証結果OK:true / 検証結果NG:false</returns>
        protected virtual bool IsValidValue(DataGridViewCell cell)
        {
            if (cell.ColumnIndex != IDX_FILE_NAME && cell.ColumnIndex != IDX_DIR_PATH)
                return true;

            char[] allowChars;
            if (cell.ColumnIndex == IDX_FILE_NAME)
                allowChars = new char[] { };
            else
                // ディレクトリパスの場合はパス区切り文字とボリューム区切り文字は許容
                allowChars = new char[]
                    {
                        Path.DirectorySeparatorChar,
                        Path.VolumeSeparatorChar
                    };

            Predicate<string> ErrHandling = (msg) =>
                {
                    // 編集モードを継続、エラー通知表示
                    foreach (DataGridViewCell selCell in grdFiles.SelectedCells)
                        selCell.Selected = false;

                    grdFiles.CurrentCell = cell;
                    grdFiles.BeginEdit(true);
                    FormUtilities.ShowMessage(msg);
                    return false;
                };

            // 禁則文字チェック
            string editVal = cell.EditedFormattedValue.ToString();
            if (editVal.HasInvalidPathChar(allowChars))
                return ErrHandling("W017");

            if (cell.ColumnIndex == IDX_FILE_NAME)
            {
                // 拡張子変更チェック
                if (Path.GetExtension(editVal) != Path.GetExtension(cell.Value.ToString()))
                    return ErrHandling("W018");
            }
            else
            {
                // ボリューム区切り文字複数入力チェック
                // (ネットワークパスはサポート外)
                int volCharCnt = (from c in editVal.ToCharArray()
                                  where c == Path.VolumeSeparatorChar
                                  select c).Count();
                if (volCharCnt == 0)
                    return ErrHandling("W019");
                if (volCharCnt > 1)
                    return ErrHandling("W020");

                // ボリューム区切り文字位置チェック
                if (editVal.IndexOf(Path.VolumeSeparatorChar) != 1)
                    return ErrHandling("W021");

                // 空のディレクトリパスチェック
                var twinSep = new string(Path.DirectorySeparatorChar, 2);
                if (editVal.IndexOf(twinSep) != -1)
                    return ErrHandling("W022");
            }

            return true;
        }

        #endregion

        #region ApplyChange - 入力された変更を適用

        /// <summary>
        /// ファイル一覧グリッドセルで入力された変更内容を各ファイルに適用します。
        /// </summary>
        /// <param name="cell">変更内容が入力されたDataGridViewCell</param>
        /// <param name="proceededRows">処理済の行インデックスが格納されたList</param>
        /// <returns>処理中止フラグ</returns>
        protected virtual bool ApplyChange(DataGridViewCell cell, List<int> proceededRows)
        {
            bool ret = true;
            int listRow = int.Parse(grdFiles.Rows[cell.RowIndex].HeaderCell.Value.ToString()) - 1;
            switch (cell.ColumnIndex)
            {
                case IDX_SEL_CHK:
                    // 対象削除
                    ImageList[listRow].Delete();
                    proceededRows.Add(listRow);
                    break;

                case IDX_FILE_NAME:
                case IDX_DIR_PATH:
                    // 既に対象が処理済の場合はスキップ
                    if (proceededRows.Contains(listRow)) break;

                    // 対象ファイルのパスを変更
                    ret = ChangePath(listRow);
                    proceededRows.Add(listRow);
                    break;

                default:
                    break;
            }

            return ret;
        }

        #endregion

        #region ChangePath - 入力内容をファイルパスに反映

        /// <summary>
        /// ファイル一覧グリッドのファイル名列、ディレクトリパス列で入力された変更をファイルに対して反映します。
        /// </summary>
        /// <param name="rowIndex">処理対象の行インデックス</param>
        /// <returns>処理中止フラグ</returns>
        protected virtual bool ChangePath(int rowIndex)
        {
            // ファイル移動
            string dirPath = grdFiles[IDX_DIR_PATH, rowIndex].Value.ToString();
            if (!Directory.Exists(dirPath))
            {
                if (FormUtilities.ShowMessage("Q009") == DialogResult.No)
                {
                    FormUtilities.ShowMessage("I007");
                    return false;
                }
                Directory.CreateDirectory(dirPath);
            }

            string fileName = grdFiles[IDX_FILE_NAME, rowIndex].Value.ToString();
            string newPath = Path.Combine(dirPath, fileName);

            File.Move(ImageList[rowIndex].Key, newPath);
            ImageList[rowIndex].Key = newPath;

            return true;
        }

        #endregion

        #region RevertEdit - 最新の確定状態に戻す

        /// <summary>
        /// グリッドの状態を直近の確定状態に戻します。
        /// </summary>
        /// <param name="isAllRevert">true:全ての対象を戻す / false:選択された対象のみ戻す</param>
        private void RevertEdit(bool isAllRevert)
        {
            foreach (DataGridViewColumn col in grdFiles.Columns)
                col.HeaderCell.SortGlyphDirection = SortOrder.None;

            if (isAllRevert)
            {
                // セルを再作成
                grdFiles.Rows.Clear();
                CreateCells();

                _changeCnt = 0;
            }
            else
            {
                // 選択されている行を取得
                var selectedRows = from r in grdFiles.Rows.Cast<DataGridViewRow>()
                                   where Convert.ToBoolean(r.Cells[IDX_SEL_CHK].Value)
                                   select r;

                for (int rowIndex = 0; rowIndex < grdFiles.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = grdFiles.Rows[rowIndex];

                    if (!Convert.ToBoolean(row.Cells[IDX_SEL_CHK].Value)) continue;

                    for (int colIndex = IDX_SEL_CHK; colIndex < IDX_REF_BTN; colIndex++)
                    {
                        DataGridViewCell cell = row.Cells[colIndex];
                        if (!cell.Value.Equals(cell.Tag))    // セル内容が表示時と異なる場合
                        {
                            // 変更前の内容を復元
                            cell.Value = cell.Tag;

                            // 変更カウントをデクリメント、変更判別色クリア
                            if (cell.Style.BackColor == Color.Yellow) --_changeCnt;
                            cell.Style.BackColor =
                                    grdFiles.Columns[colIndex].DefaultCellStyle.BackColor;
                        }
                    }

                    // 行ヘッダの変更スタイルをクリア
                    DataGridViewRowHeaderCell rhCell = grdFiles.Rows[rowIndex].HeaderCell;
                    rhCell.Style.Font = new Font(rhCell.Style.Font, FontStyle.Regular);
                }
            }

            btnApply.Enabled = btnRevertSelection.Enabled = btnRevertAll.Enabled = _changeCnt > 0;
        }

        #endregion

        #region ShowHistoryByStatusBar - 変更前後の内容をステータスバーに表示

        /// <summary>
        /// 指定セルの変更前後の内容をステータスバーに表示します。
        /// </summary>
        /// <param name="cell">変更内容を表示するセル</param>
        private void ShowHistoryByStatusBar(DataGridViewCell cell)
        {
            lblStatus.Text = cell.Tag.ToString();

            if (cell.Style.BackColor == Color.Yellow)
                lblStatus.Text += " -> " + cell.Value.ToString();
        }

        #endregion

        #region SelectDirectoryForChange - 変更後ディレクトリ選択
        /// <summary>
        /// 指定行の画像の変更後のディレクトリを選択するダイアログを表示します。
        /// </summary>
        /// <param name="rowIdx">指定行</param>
        private void SelectDirectoryForChange(int rowIdx)
        {
            if (!(bool)grdFiles.Rows[rowIdx].Tag)
            {
                using (var dlg = new FolderBrowserDialog())
                {
                    DataGridViewCell cell = grdFiles[IDX_DIR_PATH, rowIdx];

                    dlg.Description = "変更後のディレクトリを選択して下さい。";
                    dlg.RootFolder = Environment.SpecialFolder.Desktop;
                    dlg.ShowNewFolderButton = true;
                    dlg.SelectedPath = cell.Value.ToString();

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                        // OKを押下された場合はグリッドに選択ディレクトリを反映
                        cell.Value = dlg.SelectedPath;
                }
            }
        }
        #endregion

        #region ShowSimilarImages - 類似画像表示

        /// <summary>
        /// 指定行の画像の類似画像を検索し、サムネイルフォームで表示します。
        /// </summary>
        /// <param name="rowIdx">指定行</param>
        private void ShowSimilarImages(int rowIdx)
        {
            if (!(grdFiles[IDX_SIMILAR_BTN, rowIdx] is DataGridViewButtonCell)) return;

            string path = GetImagePath(rowIdx);
            List<IImage> similarImages = _similarMap != null && _similarMap.ContainsKey(rowIdx)
                    ? _similarMap[rowIdx]
                    : ImageController.GetSimilarImages(this, GetImagePath(rowIdx));

            if (!similarImages.Any())
            {
                FormUtilities.ShowMessage("I008");
            }
            else
            {
                var form = new ThumbnailForm(similarImages);
                form.Text = string.Format("PictManager - 類似画像検索結果 [{0}]", path);
                form.StatusBarText = string.Format("[{0}] の類似画像を表示中 - {1}件", path, similarImages.Count);

                form.Show(this);
                form.Activate();
            }
        }

        #endregion

        #region イベントハンドラ

        #region Form_Shown - フォーム表示時
        /// <summary>
        /// フォームが表示された際に実行される処理です。
        /// ファイル一覧グリッドの各列幅を初期化します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Form_Shown(object sender, EventArgs e)
        {
            try
            {
                // 列幅設定
                grdFiles.RowHeadersWidthSizeMode =
                        DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders |
                        DataGridViewRowHeadersWidthSizeMode.DisableResizing;
                grdFiles.Columns[IDX_FILE_NAME].Width =
                        (grdFiles.Width - grdFiles.RowHeadersWidth -
                         grdFiles.Columns[IDX_SEL_CHK].Width -
                         grdFiles.Columns[IDX_REF_BTN].Width -
                         grdFiles.Columns[IDX_SIMILAR_BTN].Width) / 3;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region Form_FormClosing - ×ボタン押下時
        /// <summary>
        /// ×ボタンがクリックされた際に実行される処理です。
        /// 終了確認後、アプリケーションを終了します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 終了確認
                if (Utilities.Config.CommonInfo.IsConfirmQuit
                    && _changeCnt > 0
                    && FormUtilities.ShowMessage("Q000") == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    if (Owner != null) Owner.Dispose();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                Application.Exit(new CancelEventArgs());
            }
        }
        #endregion

        #region Form_KeyDown - フォーム上でのキー押下時
        /// <summary>
        /// フォーム上でキーが押下された際に実行される処理です。
        /// 特殊なキーが押下された場合に固有の処理を実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void Form_KeyDown(object sender, KeyEventArgs e)
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
                        case Keys.Escape:
                            // 自フォームを破棄して親フォームを表示
                            this.BackToOwner();
                            e.Handled = true;
                            break;

                        default:
                            // 上記以外は処理無し
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

        #region grdFiles_CellDoubleClick - セルダブルクリック時

        /// <summary>
        /// ファイル一覧グリッドのセルがダブルクリックされた際に実行される処理です。
        /// その行に関連付けられたファイルをViewImageFormで表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdFiles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex > -1 
                    && !(bool)grdFiles.Rows[e.RowIndex].Tag
                    && e.ColumnIndex != IDX_REF_BTN
                    && e.ColumnIndex != IDX_SIMILAR_BTN)
                {
                    // イメージ閲覧
                    new ViewImageForm(this, new FileImage(GetImagePath(e.RowIndex))).Show(this);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region grdFiles_CellClick - セルクリック時

        /// <summary>
        /// ファイル一覧グリッドのセルがクリックされた際に実行される処理です。
        /// ・ファイル名列、ディレクトリパス列がクリックされた場合：変更前後の内容をステータスバーに表示します。
        /// ・参照ボタン列がクリックされた場合：移動先ディレクトリを選択する為の台ログを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdFiles_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                switch (e.ColumnIndex)
                {
                    case IDX_SEL_CHK:
                        // チェック切り替え
                        var delChk = grdFiles[e.ColumnIndex, e.RowIndex] as DataGridViewCheckBoxCell;
                        delChk.Value = !Convert.ToBoolean(delChk.Value);
                        grdFiles.CommitEdit(DataGridViewDataErrorContexts.Commit);
                        break;

                    case IDX_FILE_NAME:
                    case IDX_DIR_PATH:
                        // 変更前後の内容をステータスバーに表示
                        ShowHistoryByStatusBar(grdFiles[e.ColumnIndex, e.RowIndex]);
                        break;

                    case IDX_REF_BTN:
                        // 変更後ディレクトリ選択
                        SelectDirectoryForChange(e.RowIndex);
                        break;

                    case IDX_SIMILAR_BTN:
                        // 類似画像表示
                        ShowSimilarImages(e.RowIndex);
                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region grdFiles_CellValidating - セル入力内容検証時

        /// <summary>
        /// ファイル一覧グリッドのセルの入力内容検証時に実行される処理です。
        /// IsValidInputの実装内容に基づき、セルの入力内容を検証します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdFiles_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

                DataGridViewCell cell = grdFiles[e.ColumnIndex, e.RowIndex];
                if (!IsValidValue(cell))
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region grdFiles_CellValueChanged - セル内容変更時

        /// <summary>
        /// ファイル一覧グリッドのセルの内容が変更された際に実行される処理です。
        /// 
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdFiles_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.ColumnIndex == IDX_REF_BTN)
                    return;

                DataGridViewCell cell = grdFiles[e.ColumnIndex, e.RowIndex];
                if (e.ColumnIndex == IDX_SIMILAR_BTN)
                {
                    // 類似画像検索ボタンの場合
                    cell.Style.BackColor = cell.Value.Equals(string.Empty)
                            ? Color.Gray : Color.White;
                    return;
                }

                DataGridViewRowHeaderCell rhCell = grdFiles.Rows[e.RowIndex].HeaderCell;
                if (cell.Value.Equals(cell.Tag))    // セル内容が表示時と同一の場合
                {
                    // 変更カウントをデクリメント、変更判別色クリア
                    if (cell.Style.BackColor == Color.Yellow)
                    {
                        --_changeCnt;
                    }
                    cell.Style.BackColor = grdFiles.Columns[e.ColumnIndex].DefaultCellStyle.BackColor;

                    // 行の全ての内容が元に戻された場合は行ヘッダの変更マークをクリア
                    if (!grdFiles.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().Any(
                            c => c.Style.BackColor == Color.Yellow))
                    {
                        if (rhCell.Style.Font.Bold)
                            rhCell.Style.Font = new Font(rhCell.Style.Font, FontStyle.Regular);
                    }
                }
                else    // セル内容が表示時から変更された場合
                {
                    // 変更カウントをインクリメント、変更判別色セット
                    cell.Style.BackColor = Color.Yellow;
                    ++_changeCnt;

                    // 行ヘッダの変更マークが未設定の場合は設定
                    if (!rhCell.Style.Font.Bold)
                        rhCell.Style.Font = new Font(rhCell.Style.Font, FontStyle.Bold);
                }

                btnApply.Enabled = btnRevertSelection.Enabled = btnRevertAll.Enabled = _changeCnt > 0;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnClose_Click - 終了ボタン押下時
        /// <summary>
        /// 終了ボタンがクリックされた際に実行される処理です。
        /// 自フォームを破棄し、親フォームを再表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // 自フォームを破棄し親フォームを表示
            if (_changeCnt > 0 && FormUtilities.ShowMessage("Q005") == DialogResult.No)
                return;

            this.BackToOwner();
        }
        #endregion

        #region btnApply_Click - 適用ボタン押下時

        /// <summary>
        /// 適用ボタンがクリックされた際に実行される処理です。
        /// ファイル一覧グリッドに入力された内容を各ファイルに適用します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormUtilities.ShowMessage("Q008") == DialogResult.No) return;

                CursorFace.Current = Cursors.WaitCursor;

                // 変更を適用
                // 編集済セルを含む行を抽出→編集済セルを抽出(行順、列順でソート)
                Func<DataGridViewCell, bool> IsEdit = x => x.Style.BackColor == Color.Yellow;
                var editedCells = from r in grdFiles.Rows.Cast<DataGridViewRow>()
                                  where r.Cells.Cast<DataGridViewCell>().Any(IsEdit)
                                  select r.Cells.Cast<DataGridViewCell>() into er
                                  from c in er
                                  where IsEdit(c)
                                  orderby c.RowIndex, c.ColumnIndex
                                  select c;

                // 変更を適用
                var proceededRows = new List<int>();
                foreach (var cell in editedCells)
                {
                    if (!ApplyChange(cell, proceededRows))
                    {
                        RevertEdit(true);
                        Console.WriteLine("Apply break.");
                        return;
                    }
                }

                // 表示を最新状態に更新
                RevertEdit(true);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
            }
            finally
            {
                CursorFace.Current = Cursors.Default;
            }
        }

        #endregion

        #region btnRevertSelection_Click - 選択行の変更を取消ボタン押下時

        /// <summary>
        /// 選択行の変更を取消ボタンがクリックされた際に実行される処理です。
        /// ファイル一覧グリッドの選択チェックボックスがONの行の内容を、
        /// 最後に適用が実行された状態に戻します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnRevertSelection_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormUtilities.ShowMessage("Q006") == DialogResult.Yes)
                {
                    RevertEdit(false);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnRevertAll_Click - 全ての変更を取消ボタン押下時

        /// <summary>
        /// 全ての変更を取消ボタンがクリックされた際に実行される処理です。
        /// ファイル一覧グリッドの全てのの行の内容を、
        /// 最後に適用が実行された状態に戻します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnRevertAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormUtilities.ShowMessage("Q011") == DialogResult.Yes)
                {
                    RevertEdit(true);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuRefresh_Click - 対象ファイル再取得メニュー押下時

        /// <summary>
        /// 対象ファイル再取得メニューがクリックされた際に実行される処理です。
        /// 現在の情報で対象ファイルリストを更新します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                RevertEdit(true);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuRenameAll_Click - 一括ファイル名変更メニュー押下時

        /// <summary>
        /// 一括ファイル名変更メニューがクリックされた際に実行される処理です。
        /// ファイル名変更情報入力ダイアログを表示し、入力された内容に応じてファイル名を一括で変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuRenameAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (RenameAllFiles() == ResultStatus.OK)
                    RevertEdit(true);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuMoveAll_Click - 一括ファイル移動メニュー押下時
        /// <summary>
        /// 一括ファイル移動メニューがクリックされた際に実行される処理です。
        /// 移動先ディレクトリ指定ダイアログを表示し、入力された内容に応じてファイルを一括で移動します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuMoveAll_Click(object sender, EventArgs e)
        {
            try
            {
                // ファイル移動実行、正常終了時はディレクトリ選択フォームへ戻る
                RefreshImageList();
                if (MoveAllFiles() == ResultStatus.OK) this.BackToOwner();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region menuSearchSimilarAll_Click - 全ての行の類似画像を検索メニュー押下時

        /// <summary>
        /// 全ての行の類似画像を検索メニューがクリックされた際に実行される処理です。
        /// 全ての行の画像の類似画像を対象ディレクトリ内から検索し、
        /// 類似画像があった画像のSimilarボタンのみを活性化状態に設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuSearchSimilarAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (_similarMap != null) _similarMap.Clear();
                _similarMap = new Dictionary<int, List<IImage>>();

                Action<int, bool> actResetSimilarButton = (rowIdx, enable) =>
                    {
                        DataGridViewCell cell;
                        if (enable)
                        {
                            cell = new DataGridViewButtonCell();
                            grdFiles[IDX_SIMILAR_BTN, rowIdx] = cell;
                            cell.Value = "Similar";
                        }
                        else
                        {
                            cell = new DataGridViewTextBoxCell();
                            grdFiles[IDX_SIMILAR_BTN, rowIdx] = cell;
                            cell.Value = string.Empty;
                            cell.ReadOnly = true;
                        }
                    };

                foreach (DataGridViewRow row in grdFiles.Rows)
                {
                    string path = GetImagePath(row.Index);
                    if (!File.Exists(path))
                    {
                        // ファイルが既に存在しない場合は押下不可能に設定
                        //btnCell.ReadOnly = true;
                        actResetSimilarButton(row.Index, false);
                        continue;
                    }

                    string msg = string.Format("[{0}行目]{1} の類似画像を検索中...{2}{2}",
                            row.Index + 1, path, Environment.NewLine);
                    List<IImage> similarImages =
                            ImageController.GetSimilarImages(this, path, msg);
                    if (similarImages.Count == 0)
                    {
                        // 類似画像が存在しない場合は押下不可能に設定
                        actResetSimilarButton(row.Index, false);
                    }
                    else
                    {
                        // 類似画像が存在する場合は押下可能に設定し、類似画像パスを保管
                        actResetSimilarButton(row.Index, true);
                        _similarMap[row.Index] = similarImages;
                    }
                }

                FormUtilities.ShowMessage("I009");
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuFilterDuplicated_Click - 重複しているファイルのみを抽出メニュー押下時
        /// <summary>
        /// 重複しているファイルのみを抽出メニューがクリックされた際に実行される処理です。
        /// 重複が発生しているファイルの行のみを表示し、それ以外の行を非表示に設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuFilterDuplicated_Click(object sender, EventArgs e)
        {
            try
            {
                this.Update();
                CursorFace.Current = Cursors.WaitCursor;

                using (var progressDlg = new CircleProgressDialog(this))
                {
                    // プログレス表示開始
                    progressDlg.StartProgress();

                    grdFiles.CellValidating -= grdFiles_CellValidating;

                    var rows = new List<DataGridViewRow>(grdFiles.Rows.Count);
                    for (int i = grdFiles.RowCount - 1; i >= 0; --i)
                    {
                        // MD5重複が発生していない行は非表示に設定
                        if (grdFiles[IDX_MD5, i].Style.BackColor == Color.Bisque)
                            rows.Add(grdFiles.Rows[i]);
                    }
                    grdFiles.Rows.Clear();
                    grdFiles.Rows.AddRange(rows.ToArray());

                    grdFiles.Sort(grdFiles.Columns[IDX_MD5], ListSortDirection.Ascending);

                    grdFiles.CellValidating += grdFiles_CellValidating;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                CursorFace.Current = Cursors.Default;
            }
        }
        #endregion

        #region menuViewImage_Click - 選択行の画像を表示メニュー押下時

        /// <summary>
        /// 選択行の画像を表示メニューがクリックされた際に実行される処理です。
        /// 選択中の行の画像を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuViewImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdFiles.SelectedCells.Count > 0)
                {
                    int rowIndex = grdFiles.SelectedCells[0].RowIndex;
                    if (rowIndex > -1)
                    {
                        if ((bool)grdFiles.Rows[rowIndex].Tag)
                            // 削除済みの場合はメッセージで警告
                            FormUtilities.ShowMessage("W006");
                        else
                            // イメージ閲覧
                            new ViewImageForm(this, new FileImage(GetImagePath(rowIndex))).Show(this);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuSimilar_Click - 選択行の類似画像を表示メニュー押下時
        /// <summary>
        /// 選択行の類似画像を表示メニューがクリックされた際に実行される処理です。
        /// 選択中の行の画像の類似画像を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuSimilar_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdFiles.SelectedCells.Count > 0)
                {
                    int rowIndex = grdFiles.SelectedCells[0].RowIndex;
                    if (rowIndex > -1)
                    {
                        if ((bool)grdFiles.Rows[rowIndex].Tag)
                            // 削除済みの場合はメッセージで警告
                            FormUtilities.ShowMessage("W006");
                        else
                            // イメージ閲覧
                            ShowSimilarImages(rowIndex);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #endregion
    }
}
