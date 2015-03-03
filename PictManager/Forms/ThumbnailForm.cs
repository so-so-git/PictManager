using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.Forms.Extensions;
using SO.Library.IO;
using SO.Library.Text;
using SO.PictManager.Common;
using SO.PictManager.Components;

using CursorFace = System.Windows.Forms.Cursor;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// サムネイル表示フォームクラス
    /// </summary>
    public sealed partial class ThumbnailForm : BaseForm
    {
        #region クラス定数

        /// <summary>イメージファイル無し時の表示テキスト</summary>
        private const string NO_IMAGE_LABEL = "No image file in selected folder.";

        #endregion

        #region インスタンス変数

        /// <summary>サムネイルユニット配列</summary>
        private List<ThumbnailUnit> _thumbnails = new List<ThumbnailUnit>();

        /// <summary>現在表示中のページ</summary>
        private int _currentPage = 1;

        /// <summary>1ページ中の最大表示枚数</summary>
        private int _maxDispNum = 20;

        /// <summary>リサイズ変更前サイズ</summary>
        private Size _beforeResize;

        #endregion

        #region プロパティ

        #region StatusBarText - ステータスバーのテキスト取得・設定
        /// <summary>
        /// ステータスバーのテキストを取得または設定します。
        /// </summary>
        internal string StatusBarText
        {
            get { return lblStatus.Text; }
            set { lblStatus.Text = value; }
        }
        #endregion

        #endregion

        #region コンストラクタ
        /// <summary>
        /// 対象フォルダを指定して表示する際に使用するコンストラクタです。
        /// </summary>
        /// <param orderName="targetPath">対象ディレクトリパス</param>
        /// <param orderName="includeSubFlg">サブディレクトリ以下を含むかを示すフラグ</param>
        public ThumbnailForm(string targetPath, bool includeSubFlg)
                : base(targetPath, includeSubFlg)
        {
            // コンポーネント初期化
            InitializeComponent();

            // 共通構築処理
            CommonConstruction();

            // ステータスバー更新
            lblStatus.Text = FileCount > 0
                    ? Path.GetDirectoryName(FilePathes[0]) + string.Format(" - {0}ファイル", FileCount)
                    : NO_IMAGE_LABEL;
        }

        /// <summary>
        /// 対象ファイルパスのリストを指定して表示する際に使用するコンストラクタです。
        /// </summary>
        /// <param orderName="pathList">対象ディレクトリパス</param>
        public ThumbnailForm(List<string> pathList)
        {
            // コンポーネント初期化
            InitializeComponent();

            // 渡されたファイルパスリストを操作対象とする
            FilePathes = pathList;

            // 共通構築処理
            CommonConstruction();
        }

        #endregion

        #region CommonConstruction - 共通構築処理
        /// <summary>
        /// コンストラクタ共通の構築処理を行ないます。
        /// </summary>
        private void CommonConstruction()
        {
            // 定数定義
            const int unitInLine = 5;
            const int xMargin = 5;
            const int yMargin = 25;

            // サムネイルユニット配列初期化
            _thumbnails.Clear();

            // ファイル取得
            RefreshTargetFiles();

            // ページ表示初期化
            txtPage.Text = _currentPage.ToString();
            lblPageMax.Text = FileCount % _maxDispNum == 0
                    ? (FileCount / _maxDispNum).ToString()
                    : (FileCount / _maxDispNum + 1).ToString();

            // 初期表示時のコントロール配置更新用に最大化前のサイズを保管
            _beforeResize = Size;

            // フォームサイズ設定
            using (ThumbnailUnit sample = new ThumbnailUnit())
            {
                int formHeight = _maxDispNum % unitInLine == 0
                        ? _maxDispNum / unitInLine * sample.Height + (_maxDispNum / unitInLine + 1) * yMargin
                        : (_maxDispNum / unitInLine + 1) * sample.Height + (_maxDispNum / unitInLine + 2) * yMargin;
                formHeight += pnlParent.Panel2.Height;
                ClientSize = new Size(sample.Width * unitInLine + (unitInLine + 1) * xMargin, formHeight);
            }
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
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("ページ再表示", null, (s, e) => RefreshThumbnails()));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択ファイル移動", null, menuMoveSelected_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択ファイル削除", null, btnDelete_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("ディレクトリを開く", null, (s, e) => Utilities.OpenExplorer(TargetDirectory.FullName)));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("終了", null,
                    (s, e) => Form_FormClosing(s, new FormClosingEventArgs(CloseReason.UserClosing, false))));
            barMenu.Items.Add(menuTemp);

            // 操作
            menuTemp = new ToolStripMenuItem("操作(&O)", null, null, "menuOpe");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.O;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("次ページへ", null, btnNext_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("前ページへ", null, btnPrevious_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            var menuPage = new ToolStripTextBox("menuTxtPage");
            menuPage.KeyDown += txtPage_KeyDown;
            menuPage.TextChanged += txtPage_TextChanged;
            menuTemp.DropDownItems.Add(menuPage);
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("指定したページへジャンプ", null,
                    (s, e) => AcceptPageNumber()));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("削除確認しない", null,
                    (s, e) => ((ToolStripMenuItem)s).Checked = !((ToolStripMenuItem)s).Checked, "menuChkConfirm"));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("複数選択", null,
                    (s, e) => ((ToolStripMenuItem)s).Checked = !((ToolStripMenuItem)s).Checked, "menuChkMulti"));
            barMenu.Items.Add(menuTemp);

            // 表示
            menuTemp = new ToolStripMenuItem("表示(&V)", null, null, "menuView");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.V;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択画像表示", null, menuViewImage_Clicked, "menuViewImage"));
            barMenu.Items.Add(menuTemp);
        }
        #endregion

        #region RefreshTargetFiles - 対象ファイルリスト最新化
        /// <summary>
        /// ディレクトリを対象に操作を行っている場合はディレクトリの現在の状態を再取得します。
        /// ファイルパスリストを対象に操作を行っている場合は何も行ないません。
        /// </summary>
        protected override void  RefreshTargetFiles()
        {
            if (TargetDirectory != null)
            {
                base.RefreshTargetFiles();
                _currentPage = 1;
                RefreshThumbnails();
            }
        }
        #endregion

        #region RefreshThumbnails - サムネイル表示更新
        /// <summary>
        /// 現在のページの表示サムネイルを、対象ディレクトリの最新状態に合わせて更新します。
        /// </summary>
        private void RefreshThumbnails()
        {
            try
            {
                // サムネイルパネル表示更新抑制、表示中サムネイルクリア
                pnlThumbnail.SuspendLayout();

                // 表示中サムネイルの破棄
                _thumbnails.Clear();
                pnlThumbnail.Controls.Clear();
                foreach (var thumbnail in _thumbnails)
                    thumbnail.Dispose();

                // 選択画像表示メニューを非活性化
                FormUtilities.GetMenuItem<ToolStripMenuItem>(barMenu.Items, "menuView/menuViewImage").Enabled = false;

                // プログレスダイアログ表示
                using (var progDlg = new ProgressDialog(this))
                {
                    progDlg.StartProgress(string.Format("ページ{0}表示中...", _currentPage.ToString()), string.Empty, 0,
                            _currentPage == int.Parse(lblPageMax.Text) ? FileCount % _maxDispNum : _maxDispNum);
                    Update();

                    for (int i = 0; i < _maxDispNum && (_currentPage - 1) * _maxDispNum + i < FileCount; ++i)
                    {
                        // 対象ファイルインデックス算出
                        int idx = (_currentPage - 1) * _maxDispNum + i;

                        // プログレスメッセージ更新
                        progDlg.Message = FilePathes[idx];

                        ThumbnailUnit addThumbnail;
                        if (FilePathes[idx] == DELETED_MARK)
                        {
                            // 削除済み画像用のサムネイルユニットを作成
                            addThumbnail = new ThumbnailUnit();
                            addThumbnail.NameLabel = ThumbnailUnit.DELETED_NAME_LABEL;
                        }
                        else
                        {
                            // 指定パスを表示するサムネイルユニットを作成
                            addThumbnail = new ThumbnailUnit(FilePathes[idx]);
                        }

                        // イベント定義
                        addThumbnail.UnitClick += ThumbnailUnit_Click;
                        addThumbnail.UnitDoubleClick += ThumbnailUnit_DoubleClick;

                        // サムネイルをパネルと管理リストに追加
                        pnlThumbnail.Controls.Add(addThumbnail);
                        _thumbnails.Add(addThumbnail);

                        // プログレスバー更新
                        progDlg.PerformStep();
                    }
                }

                // サムネイルパネル表示更新再開
                pnlThumbnail.ResumeLayout();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region AcceptPageNumber - 現在ページ指定入力
        /// <summary>
        /// 指定されたページを表示します。
        /// </summary>
        private void AcceptPageNumber()
        {
            if (txtPage.Text.Trim().Length == 0)
            {
                // 未入力時は変更前のページを復元
                txtPage.Text = _currentPage.ToString();
                txtPage.Focus();
                txtPage.SelectAll();
                return;
            }

            txtPage.Focus();
            txtPage.SelectAll();
            int page;
            // 数値チェック、最小チェック
            if (!int.TryParse(txtPage.Text, out page) || page < 1)
            {
                FormUtilities.ShowMessage("W004");
                return;
            }
            // 最大チェック
            if (page > int.Parse(lblPageMax.Text))
            {
                FormUtilities.ShowMessage("W003");
                return;
            }

            // 指定ページと現在表示ページが同一の場合は処理無し
            if (_currentPage == page) return;

            // 現在ページ表示、サムネイル表示更新
            _currentPage = page;
            RefreshThumbnails();
        }
        #endregion

        #region イベントハンドラ

        #region Form_Shown - フォーム表示時
        /// <summary>
        /// フォームが表示された際に実行される処理です。
        /// 最初のページのサムネイル表示を初期化します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void Form_Shown(object sender, EventArgs e)
        {
            // サムネイル表示を初期化
            RefreshThumbnails();
        }
        #endregion

        #region Form_Resize - フォームサイズ変更時
        /// <summary>
        /// フォームのサイズが変更された際に実行される処理です。
        /// 各ボタン、コントロールの配置を再設定します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void Form_Resize(object sender, EventArgs e)
        {
            if (!Size.Equals(_beforeResize))
            {
                int gap = Size.Width - _beforeResize.Width;

                btnPrevious.Location = new Point(btnPrevious.Location.X + gap / 2, btnPrevious.Location.Y);
                txtPage.Location = new Point(txtPage.Location.X + gap / 2, txtPage.Location.Y);
                lblPageSlash.Location = new Point(lblPageSlash.Location.X + gap / 2, lblPageSlash.Location.Y);
                lblPageMax.Location = new Point(lblPageMax.Location.X + gap / 2, lblPageMax.Location.Y);
                btnNext.Location = new Point(btnNext.Location.X + gap / 2, btnNext.Location.Y);

                _beforeResize = Size;
            }
        }
        #endregion

        #region Form_FormClosing - ×ボタン押下時
        /// <summary>
        /// ×ボタンがクリックされた際に実行される処理です。
        /// 終了確認後、アプリケーションを終了します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (TargetDirectory == null)
                {
                    // 類似画像表示用に呼び出された場合は親フォーム表示
                    this.BackToOwner();
                }
                else
                {
                    // ディレクトリ表示用に呼び出された場合は終了確認
                    if (Utilities.Config.CommonInfo.IsConfirmQuit
                        && FormUtilities.ShowMessage("Q000") == DialogResult.No)
                    {
                        e.Cancel = true;
                    }
                    else
                    {
                        if (Owner != null) Owner.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region Form_MouseWheel - フォーム上マウスホイール回転時
        /// <summary>
        /// フォーム上でマウスホイールが回された際に実行される処理です。
        /// フォーム内容をスクロールします。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Delta == 0) return;

                if (pnlThumbnail.VerticalScroll.Visible)
                {
                    int delta = e.Delta / WHEEL_DELTA * pnlThumbnail.VerticalScroll.SmallChange * -1;
                    if (delta < 0)
                    {
                        if (-pnlThumbnail.AutoScrollPosition.Y + delta < pnlThumbnail.VerticalScroll.Minimum)
                        {
                            pnlThumbnail.AutoScrollPosition =
                                    new Point(0, pnlThumbnail.VerticalScroll.Minimum);
                            return;
                        }
                    }
                    else
                    {
                        if (-pnlThumbnail.AutoScrollPosition.Y + delta > pnlThumbnail.VerticalScroll.Maximum)
                        {
                            pnlThumbnail.AutoScrollPosition =
                                    new Point(0, pnlThumbnail.VerticalScroll.Maximum);
                            return;
                        }
                    }

                    pnlThumbnail.AutoScrollPosition =
                            new Point(0, -pnlThumbnail.AutoScrollPosition.Y + delta);
                }
                else if (pnlThumbnail.HorizontalScroll.Visible)
                {
                    int delta = e.Delta / WHEEL_DELTA * pnlThumbnail.HorizontalScroll.SmallChange * -1;
                    if (delta < 0)
                    {
                        if (-pnlThumbnail.AutoScrollPosition.X + delta < pnlThumbnail.HorizontalScroll.Minimum)
                        {
                            pnlThumbnail.AutoScrollPosition =
                                    new Point(pnlThumbnail.HorizontalScroll.Minimum, 0);
                            return;
                        }
                    }
                    else
                    {
                        if (-pnlThumbnail.AutoScrollPosition.X + delta < pnlThumbnail.HorizontalScroll.Maximum)
                        {
                            pnlThumbnail.AutoScrollPosition =
                                    new Point(pnlThumbnail.HorizontalScroll.Maximum, 0);
                            return;
                        }
                    }

                    pnlThumbnail.AutoScrollPosition =
                            new Point(-pnlThumbnail.AutoScrollPosition.X + delta, 0);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region menuMoveSelected_Click - 選択ファイル移動メニュー押下時
        /// <summary>
        /// 選択ファイル移動メニューがクリックされた際に実行される処理です。
        /// 選択されたサムネイルを指定されたフォルダに移動します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void menuMoveSelected_Click(object sender, EventArgs e)
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
                        return;
                    }
                    destDir = dlg.SelectedPath;
                }

                // 指定ディレクトリへファイルを移動
                foreach (var srcPath in _thumbnails.Where(
                    t => t.BorderStyle == BorderStyle.FixedSingle).Select(t => t.FilePath))
                {
                    target = srcPath;   // エラー処理用に対象ファイル名を保存
                    File.Move(srcPath, Path.Combine(destDir, Path.GetFileName(srcPath)));
                }

                RefreshTargetFiles();

                status = ResultStatus.OK;
            }
            catch (Exception ex)
            {
                status = ResultStatus.Error;
                string optionMsg = ex is IOException ?
                        MessageXml.GetMessageInfo("E001", target).message : null;
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                // 対象ファイルリストをリフレッシュ
                RefreshTargetFiles();

                // マウスカーソル変更(通常)
                CursorFace.Current = Cursors.Default;

                if (status == ResultStatus.OK)
                    // 終了通知
                    FormUtilities.ShowMessage("I004");
            }
        }
        #endregion

        #region menuViewImage_Clicked - 選択画像表示メニュー押下時
        /// <summary>
        /// 選択画像表示メニューがクリックされた際に実行される処理です。
        /// 選択されたサムネイルをViewImageFormで表示します。
        /// ViewImageForm側で削除された場合は、サムネイルを削除済み状態に設定します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void menuViewImage_Clicked(object sender, EventArgs e)
        {
            try
            {
                // 選択中の画像をViewImageFormdで表示
                ThumbnailUnit_DoubleClick(
                        _thumbnails.Single(t => t.BorderStyle == BorderStyle.FixedSingle),
                        new EventArgs());
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region ThumbnailUnit_Click - サムネイル表示コントロールクリック時
        /// <summary>
        /// サムネイルがクリックされた際に実行される処理です。
        /// クリックされたサムネイルを選択状態にします。
        /// 既に選択状態の場合は、選択を解除します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void ThumbnailUnit_Click(object sender, EventArgs e)
        {
            ThumbnailUnit clicked = (ThumbnailUnit)sender;
            if (clicked.NameLabel != ThumbnailUnit.DELETED_NAME_LABEL)
            {
                var menuChkMulti = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                        barMenu.Items, "menuOpe/menuChkMulti");
                if (menuChkMulti.Checked)
                {
                    // 複数選択モード時
                    if (clicked.BorderStyle == BorderStyle.None)
                        // 未選択時はボーダー設定
                        clicked.BorderStyle = BorderStyle.FixedSingle;
                    else
                        // 選択済時はボーダー消去
                        clicked.BorderStyle = BorderStyle.None;
                }
                else
                {
                    // 単一選択モード時
                    foreach (var thumbnail in _thumbnails)
                        // 全サムネイルのボーダーを消去
                        thumbnail.BorderStyle = BorderStyle.None;

                    // 選択されたサムネイルのボーダーを設定
                    clicked.BorderStyle = BorderStyle.FixedSingle;
                }
            }

            // 選択画像表示メニューを活性制御
            FormUtilities.GetMenuItem<ToolStripMenuItem>(barMenu.Items, "menuView/menuViewImage").Enabled =
                    _thumbnails.Count(t => t.BorderStyle == BorderStyle.FixedSingle) == 1;
        }
        #endregion

        #region ThumbnailUnit_DoubleClick - サムネイル表示コントロールダブルクリック時
        /// <summary>
        /// サムネイルがダブルクリックされた際に実行される処理です。
        /// ダブルクリックされたサムネイルをViewImageFormで表示します。
        /// ViewImageForm側で削除された場合は、サムネイルを削除済み状態に設定します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void ThumbnailUnit_DoubleClick(object sender, EventArgs e)
        {
            ThumbnailUnit clicked = (ThumbnailUnit)sender;
            if (clicked.NameLabel != ThumbnailUnit.DELETED_NAME_LABEL)
            {
                var menuChkMulti = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                        barMenu.Items, "menuOpe/menuChkMulti");
                if (menuChkMulti.Checked)
                    // 複数選択モード時、シングルクリックでボーダーが消える可能性が有るので再設定
                    clicked.BorderStyle = BorderStyle.FixedSingle;

                new ViewImageForm(this, clicked.FilePath).ShowDialog(this);

                // 子フォーム側でファイルが削除された場合
                if (!File.Exists(clicked.FilePath))
                {
                    // 削除済みマークを設定
                    int idx = SearchFileIndex(clicked.FilePath);
                    FilePathes[idx] = DELETED_MARK;

                    // 画像破棄、名称ラベル設定、選択マーク解除
                    if (clicked.PictureBox != null) clicked.PictureBox.Dispose();
                    clicked.NameLabel = ThumbnailUnit.DELETED_NAME_LABEL;
                    clicked.BorderStyle = BorderStyle.None;
                }
            }
        }
        #endregion

        #region btnClose_Click - 閉じるボタン押下時
        /// <summary>
        /// 閉じるボタンがクリックされた際に実行される処理です。
        /// 自フォームを破棄し、親フォームを再表示します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // 全サムネイルのリソース破棄
            foreach (var thumbnail in _thumbnails)
                if (thumbnail != null) thumbnail.Dispose();

            // 自フォームを破棄し親フォームを表示
            this.BackToOwner();
        }
        #endregion

        #region btnPrevious_Click - 前ページボタン押下時
        /// <summary>
        /// 前ページボタンがクリックされた際に実行される処理です。
        /// 前のページを表示します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // ページ更新
            if (_currentPage == 1)
                _currentPage = int.Parse(lblPageMax.Text);
            else
                --_currentPage;

            txtPage.Text = _currentPage.ToString();

            // サムネイル表示更新
            RefreshThumbnails();
        }
        #endregion

        #region btnNext_Click - 次ページボタン押下時
        /// <summary>
        /// 次ページボタンがクリックされた際に実行される処理です。
        /// 次のページを表示します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            // ページ更新
            if (_currentPage == int.Parse(lblPageMax.Text))
                _currentPage = 1;
            else
                ++_currentPage;

            txtPage.Text = _currentPage.ToString();

            // サムネイル表示更新
            RefreshThumbnails();
        }
        #endregion

        #region btnDelete_Click - 削除ボタン押下時
        /// <summary>
        /// 削除ボタンが押下された際に実行される処理です。
        /// 選択状態の全サムネイルの実ファイルを削除します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // 全ての選択中ユニットを取得
                var selectedList = (from s in _thumbnails
                                    where s.BorderStyle == BorderStyle.FixedSingle
                                    select s).ToList();


                // ファイルが一つも選択されていない場合
                if (selectedList.Count == 0)
                {
                    FormUtilities.ShowMessage("W009");
                    return;
                }

                // 削除確認
                var menuChkConfirm = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                        barMenu.Items, "menuOpe/menuChkConfirm");
                if (!menuChkConfirm.Checked)
                    if (FormUtilities.ShowMessage("Q004") == DialogResult.No)
                        return;

                foreach (var selected in selectedList)
                {
                    // 対象ファイルを削除
                    if (!DeleteFile(selected.FilePath)) return;

                    // 名称ラベルを設定
                    selected.NameLabel = ThumbnailUnit.DELETED_NAME_LABEL; ;

                    // 削除済みマークを設定
                    int idx = SearchFileIndex(selected.FilePath);
                    FilePathes[idx] = DELETED_MARK;

                    // 選択マークを解除
                    selected.BorderStyle = BorderStyle.None;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region txtPage_Leave - 現在ページ表示テキストボックスロストフォーカス時
        /// <summary>
        /// 現在ページ表示テキストボックスからフォーカスが外れた際に実行される処理です。
        /// テキストボックスに入力されている番号のページを表示します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void txtPage_Leave(object sender, EventArgs e)
        {
            // 指定されたページを表示
            AcceptPageNumber();
        }
        #endregion

        #region txtPage_KeyDown - 現在ページ表示テキストボックスでのキー押下時
        /// <summary>
        /// 現在ページ表示テキストボックスでキーが押下された際に実行される処理です。
        /// Enterキーが押下された場合、テキストボックスに入力されている番号のページを表示します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void txtPage_KeyDown(object sender, KeyEventArgs e)
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
                            // 現在ページ表示更新
                            AcceptPageNumber();
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

        #region txtPage_TextChanged - 現在ページ表示テキストボックス値変更時
        /// <summary>
        /// 現在ページ表示テキストボックスの内容が変更された際に実行される処理です。
        /// 現在ページ表示メニューと内容の同期します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            // メニューとメイン画面の同期を取る
            ToolStripTextBox menuTxtPage =
                    FormUtilities.GetMenuItem<ToolStripTextBox>(barMenu.Items, "menuOpe/menuTxtPage");
            if (sender == txtPage)
                menuTxtPage.Text = txtPage.Text;
            else
                txtPage.Text = menuTxtPage.Text;
        }
        #endregion

        #endregion
    }
}
