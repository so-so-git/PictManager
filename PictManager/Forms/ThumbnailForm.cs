using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;

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

        /// <summary>画像の順番を手動で変更出来るかのフラグ</summary>
        private bool _canChangeOrder;

        /// <summary>サムネイルドラッグ中フラグ</summary>
        private bool _isDraging;

        /// <summary>サムネイルドラッグ開始位置</summary>
        private Point? _dragStartPoint;

        /// <summary>ドラッグしているサムネイル画像のゴースト</summary>
        private Form _dragGhost;

        /// <summary>ドラッグしているサムネイル</summary>
        private ThumbnailUnit _dragThumbnail;

        #endregion

        #region プロパティ

        /// <summary>
        /// タイトルバーのテキストを取得または設定します。
        /// </summary>
        public string TitleBarText
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        /// <summary>
        /// ステータスバーのテキストを取得または設定します。
        /// </summary>
        public string StatusBarText
        {
            get { return lblStatus.Text; }
            set { lblStatus.Text = value; }
        }

        /// <summary>
        /// 表示している画像グループのIDを取得または設定します。
        /// </summary>
        public int? GroupId { get; set; }

        #endregion

        #region イベント

        /// <summary>
        /// 登録ボタン押下時に発生するイベントです。
        /// </summary>
        public event EventHandler EntryButtonClick
        {
            add { btnEntry.Click += value; }
            remove { btnEntry.Click -= value; }
        }

        /// <summary>
        /// 削除ボタン押下時に発生するイベントです。
        /// </summary>
        public event EventHandler DeleteButtonClick
        {
            add { btnDelete.Click += value; }
            remove { btnDelete.Click -= value; }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// ファイルモード用のコンストラクタです。
        /// </summary>
        /// <param name="targetPath">対象ディレクトリパス</param>
        /// <param name="includeSubFlg">サブディレクトリ以下を含むかを示すフラグ</param>
        public ThumbnailForm(string targetPath, bool includeSubFlg)
            : base(targetPath, includeSubFlg)
        {
            // コンポーネント初期化
            InitializeComponent();

            // 共通構築処理
            CommonConstruction();

            // ステータスバー更新
            lblStatus.Text = ImageCount > 0
                ? Path.GetDirectoryName(ImageList.First().Key) + string.Format(" - {0}ファイル", ImageCount)
                : NO_IMAGE_LABEL;
        }

        /// <summary>
        /// データベースモード用のコンストラクタです。
        /// </summary>
        /// <param name="category">対象カテゴリー</param>
        public ThumbnailForm(MstCategory category)
            : base(category)
        {
            // コンポーネント初期化
            InitializeComponent();

            // 共通構築処理
            CommonConstruction();

            // ステータスバー更新
            lblStatus.Text = ImageCount > 0
                ? category.CategoryName + string.Format(" - {0}ファイル", ImageCount)
                : NO_IMAGE_LABEL;
        }

        /// <summary>
        /// 任意の画像リスト用のコンストラクタです。
        /// </summary>
        /// <param name="imageList">画像データリスト</param>
        /// <param name="imageMode">画像モード</param>
        /// <param name="groupId">表示する画像グループのID</param>
        /// <param name="canChangeOrder">画像の順番を手動で変更出来るかのフラグ</param>
        public ThumbnailForm(List<IImage> imageList, ConfigInfo.ImageDataMode imageMode,
                             int? groupId = null, bool canChangeOrder = false)
            : base(imageMode)
        {
            // コンポーネント初期化
            InitializeComponent();

            // 渡された画像リストを操作対象とする
            ImageList = imageList;
            GroupId = groupId;

            _canChangeOrder = canChangeOrder;

            // 削除ボタンイベントを除去
            btnDelete.Click -= btnDelete_Click;

            // 共通構築処理
            CommonConstruction();
        }

        #endregion

        #region SetMouseEventHandlers - コントロールにマウスイベントを設定

        /// <summary>
        /// 指定されたコントロールと、その全ての子コントロールに
        /// サムネイル移動用のマウスイベントハンドラを設定します。
        /// </summary>
        /// <param name="control">対象のコントロール</param>
        private void SetMouseEventHandlers(Control control)
        {
            control.MouseDown += ThumbnailUnit_MouseDown;
            control.MouseMove += ThumbnailUnit_MouseMove;
            control.MouseUp += ThumbnailUnit_MouseUp;

            foreach (Control child in control.Controls)
            {
                SetMouseEventHandlers(child);
            }
        }

        #endregion

        #region RefreshAll - 対象ファイルリスト最新化(外部からの呼出用)

        /// <summary>
        /// 表示対象画像リストを最新の内容に更新します。
        /// 画像リストを対象に操作を行っている場合は何も行ないません。
        /// </summary>
        public void RefreshAll()
        {
            RefreshImageList();
        }

        #endregion

        #region AddImage - 表示対象画像を追加

        /// <summary>
        /// 指定された画像データを表示対象リストの末尾に追加します。
        /// </summary>
        /// <param name="imageData">追加する画像データ</param>
        public void AddImage(IImage imageData)
        {
            ImageList.Add(imageData);
            RefreshThumbnails();
        }

        #endregion

        #region RemoveSelectedImage - 選択されている画像を表示対象リストから除去

        /// <summary>
        /// 選択されている画像を表示対象リストから除去します。
        /// </summary>
        public void RemoveSelectedImage()
        {
            var selectedList = from t in _thumbnails
                               where t.BorderStyle == BorderStyle.FixedSingle
                               join i in ImageList
                               on t.ImageKey equals i.Key
                               select i;

            if (!selectedList.Any())
            {
                return;
            }

            foreach (var selected in selectedList)
            {
                ImageList.Remove(selected);
            }

            RefreshThumbnails();
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
            RefreshImageList();

            // ページ表示初期化
            txtPage.Text = _currentPage.ToString();
            lblPageMax.Text = ImageCount % _maxDispNum == 0
                ? (ImageCount / _maxDispNum).ToString()
                : (ImageCount / _maxDispNum + 1).ToString();

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
            ToolStripMenuItem menuTemp;
            if (ImageMode == Info.ConfigInfo.ImageDataMode.File)
            {
                // ファイル
                menuTemp = new ToolStripMenuItem("ファイル(&F)", null, null, "menuFile");
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
            }
            else
            {
                // データ
                menuTemp = new ToolStripMenuItem("データ(&D)", null, null, "menuData");
                menuTemp.ShortcutKeys = Keys.Alt | Keys.D;
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("戻る", null, btnClose_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("ページ再表示", null, (s, e) => RefreshThumbnails()));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("終了", null,
                    (s, e) => Form_FormClosing(s, new FormClosingEventArgs(CloseReason.UserClosing, false))));
                barMenu.Items.Add(menuTemp);
            }

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
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択画像表示", null, menuViewImage_Click, "menuViewImage"));
            barMenu.Items.Add(menuTemp);
        }

        #endregion

        #region RefreshImageList - 対象ファイルリスト最新化

        /// <summary>
        /// 表示対象画像リストを最新の内容に更新します。
        /// 画像リストを対象に操作を行っている場合は何も行ないません。
        /// (Refreshメソッドは本メソッドと同じ処理を行います)
        /// </summary>
        protected override void RefreshImageList()
        {
            if ((ImageMode == ConfigInfo.ImageDataMode.File && TargetDirectory == null)
                || (ImageMode == ConfigInfo.ImageDataMode.Database && TargetCategory == null))
            {
                return;
            }

            base.RefreshImageList();

            _currentPage = 1;
            RefreshThumbnails();
        }

        #endregion

        #region RefreshThumbnails - サムネイル表示更新

        /// <summary>
        /// 現在のページの表示サムネイルを、表示対象の最新状態に合わせて更新します。
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
                {
                    thumbnail.Dispose();
                }

                // 選択画像表示メニューを非活性化
                FormUtilities.GetMenuItem<ToolStripMenuItem>(barMenu.Items, "menuView/menuViewImage").Enabled = false;

                // プログレスダイアログ表示
                using (var progDlg = new ProgressDialog(this))
                {
                    progDlg.StartProgress(string.Format("ページ{0}表示中...", _currentPage.ToString()), string.Empty, 0,
                        _currentPage == int.Parse(lblPageMax.Text) ? ImageCount % _maxDispNum : _maxDispNum);
                    Update();

                    for (int i = 0; i < _maxDispNum && (_currentPage - 1) * _maxDispNum + i < ImageCount; i++)
                    {
                        // 対象ファイルインデックス算出
                        int idx = (_currentPage - 1) * _maxDispNum + i;

                        // プログレスメッセージ更新
                        progDlg.Message = ImageList[idx].Key;

                        ThumbnailUnit addThumbnail;
                        if (ImageList[idx].IsDeleted)
                        {
                            // 削除済み画像用のサムネイルユニットを作成
                            addThumbnail = new ThumbnailUnit();
                            addThumbnail.ImageKey = ThumbnailUnit.DELETED_NAME_LABEL;
                        }
                        else
                        {
                            // 指定パスを表示するサムネイルユニットを作成
                            addThumbnail = new ThumbnailUnit(ImageList[idx]);
                        }

                        // イベント定義
                        addThumbnail.UnitClick += ThumbnailUnit_Click;
                        addThumbnail.UnitDoubleClick += ThumbnailUnit_DoubleClick;

                        if (_canChangeOrder)
                        {
                            SetMouseEventHandlers(addThumbnail);
                        }

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
            if (string.IsNullOrEmpty(txtPage.Text.Trim()))
            {
                // 未入力時は変更前のページを復元
                txtPage.Text = _currentPage.ToString();
                txtPage.Focus();
                txtPage.SelectAll();
                return;
            }

            txtPage.Focus();
            txtPage.SelectAll();

            // 数値チェック、最小チェック
            int page;
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
            if (_currentPage == page)
            {
                return;
            }

            // 現在ページ表示、サムネイル表示更新
            _currentPage = page;
            RefreshThumbnails();
        }

        #endregion

        //*** イベントハンドラ ***

        #region Form_Shown - フォーム表示時

        /// <summary>
        /// フォームが表示された際に実行される処理です。
        /// 最初のページのサムネイル表示を初期化します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
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
                        if (Owner != null)
                        {
                            Owner.Dispose();
                        }
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Delta == 0)
                {
                    return;
                }

                if (pnlThumbnail.VerticalScroll.Visible)
                {
                    int delta = e.Delta / Constants.WHEEL_DELTA * pnlThumbnail.VerticalScroll.SmallChange * -1;
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
                    int delta = e.Delta / Constants.WHEEL_DELTA * pnlThumbnail.HorizontalScroll.SmallChange * -1;
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuMoveSelected_Click(object sender, EventArgs e)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            var status = ResultStatus.Empty;
            string target = null;   // 処理対象ファイルパス
            try
            {
                // マウスカーソル変更(待機)
                Cursor.Current = Cursors.WaitCursor;

                // 移動先ディレクトリを取得
                string destDir = null;
                using (var dlg = new FolderBrowserDialog())
                {
                    dlg.RootFolder = Environment.SpecialFolder.Desktop;
                    dlg.Description = "ファイルの移動先フォルダを指定して下さい。";

                    if (dlg.ShowDialog(this) != DialogResult.OK)
                    {
                        return;
                    }
                    destDir = dlg.SelectedPath;
                }

                // 指定ディレクトリへファイルを移動
                foreach (var srcPath in _thumbnails.Where(
                    t => t.BorderStyle == BorderStyle.FixedSingle).Select(t => t.ImageData.Key))
                {
                    target = srcPath;   // エラー処理用に対象ファイル名を保存
                    File.Move(srcPath, Path.Combine(destDir, Path.GetFileName(srcPath)));
                }

                RefreshImageList();

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
                RefreshImageList();

                // マウスカーソル変更(通常)
                Cursor.Current = Cursors.Default;

                if (status == ResultStatus.OK)
                {
                    // 終了通知
                    FormUtilities.ShowMessage("I004");
                }
            }
        }

        #endregion

        #region menuViewImage_Click - 選択画像表示メニュー押下時

        /// <summary>
        /// 選択画像表示メニューがクリックされた際に実行される処理です。
        /// 選択されたサムネイルをViewImageFormで表示します。
        /// ViewImageForm側で削除された場合は、サムネイルを削除済み状態に設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuViewImage_Click(object sender, EventArgs e)
        {
            try
            {
                // 選択中の画像をViewImageFormで表示
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ThumbnailUnit_Click(object sender, EventArgs e)
        {
            var clicked = sender as ThumbnailUnit;
            if (clicked.ImageKey != ThumbnailUnit.DELETED_NAME_LABEL)
            {
                var menuChkMulti = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                    barMenu.Items, "menuOpe/menuChkMulti");

                if (menuChkMulti.Checked)
                {
                    // 複数選択モード時
                    if (clicked.BorderStyle == BorderStyle.None)
                    {
                        // 未選択時はボーダー設定
                        clicked.BorderStyle = BorderStyle.FixedSingle;
                    }
                    else
                    {
                        // 選択済時はボーダー消去
                        clicked.BorderStyle = BorderStyle.None;
                    }
                }
                else
                {
                    // 単一選択モード時
                    foreach (var thumbnail in _thumbnails)
                    {
                        // 全サムネイルのボーダーを消去
                        thumbnail.BorderStyle = BorderStyle.None;
                    }

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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ThumbnailUnit_DoubleClick(object sender, EventArgs e)
        {
            var clicked = sender as ThumbnailUnit;
            if (clicked.ImageKey != ThumbnailUnit.DELETED_NAME_LABEL)
            {
                var menuChkMulti = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                    barMenu.Items, "menuOpe/menuChkMulti");

                if (menuChkMulti.Checked)
                {
                    // 複数選択モード時、シングルクリックでボーダーが消える可能性が有るので再設定
                    clicked.BorderStyle = BorderStyle.FixedSingle;
                }

                new ViewImageForm(this, clicked.ImageData).ShowDialog(this);

                // 子フォーム側でファイルが削除された場合
                if (clicked.ImageData.IsDeleted)
                {
                    // 名称ラベル設定、選択マーク解除
                    clicked.ImageKey = ThumbnailUnit.DELETED_NAME_LABEL;
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // 全サムネイルのリソース破棄
            foreach (var thumbnail in _thumbnails)
            {
                if (thumbnail != null)
                {
                    thumbnail.Dispose();
                }
            }

            // 自フォームを破棄し親フォームを表示
            this.BackToOwner();
        }

        #endregion

        #region btnPrevious_Click - 前ページボタン押下時

        /// <summary>
        /// 前ページボタンがクリックされた際に実行される処理です。
        /// 前のページを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            // ページ更新
            if (_currentPage == 1)
            {
                _currentPage = int.Parse(lblPageMax.Text);
            }
            else
            {
                _currentPage--;
            }

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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            // ページ更新
            if (_currentPage == int.Parse(lblPageMax.Text))
            {
                _currentPage = 1;
            }
            else
            {
                _currentPage++;
            }

            txtPage.Text = _currentPage.ToString();

            // サムネイル表示更新
            RefreshThumbnails();
        }

        #endregion

        #region btnDelete_Click - 削除ボタン押下時

        /// <summary>
        /// 削除ボタンが押下された際に実行される処理です。
        /// 選択状態のサムネイルの画像を削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // 全ての選択中ユニットを取得
                var selectedList = from s in _thumbnails
                                   where s.BorderStyle == BorderStyle.FixedSingle
                                   select s;

                // 画像が一つも選択されていない場合
                if (!selectedList.Any())
                {
                    FormUtilities.ShowMessage("W009");
                    return;
                }

                // 削除確認
                var menuChkConfirm = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                    barMenu.Items, "menuOpe/menuChkConfirm");

                if (!menuChkConfirm.Checked)
                {
                    if (FormUtilities.ShowMessage("Q004") == DialogResult.No)
                    {
                        return;
                    }
                }

                foreach (var selected in selectedList)
                {
                    // 対象画像を削除
                    selected.ImageData.Delete();

                    // 画像キーラベルを設定
                    selected.ImageKey = ThumbnailUnit.DELETED_NAME_LABEL;

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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtPage_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // 修飾キーが付加されている場合は通常処理
                if ((e.KeyCode & Keys.Alt) != Keys.Alt
                    && (e.KeyCode & Keys.Control) != Keys.Control
                    && (e.KeyCode & Keys.Shift) != Keys.Shift)
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
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtPage_TextChanged(object sender, EventArgs e)
        {
            // メニューとメイン画面の同期を取る
            ToolStripTextBox menuTxtPage =
                FormUtilities.GetMenuItem<ToolStripTextBox>(barMenu.Items, "menuOpe/menuTxtPage");

            if (sender == txtPage)
            {
                menuTxtPage.Text = txtPage.Text;
            }
            else
            {
                txtPage.Text = menuTxtPage.Text;
            }
        }

        #endregion

        #region ThumbnailUnit_MouseDown - サムネイル上でのマウスボタン押下時

        /// <summary>
        /// サムネイル上でマウスのボタンが押下された際に実行される処理です。
        /// マウスの左ボタンが押下された場合、ドラッグ処理に関するパラメータを保管します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ThumbnailUnit_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Left)
                {
                    return;
                }

                // ドラッグしているコントロールの親をたどり、サムネイルを特定
                var control = sender as Control;
                do
                {
                    if (control is ThumbnailUnit)
                    {
                        _dragThumbnail = control as ThumbnailUnit;
                        break;
                    }

                    control = control.Parent;

                } while (control.Parent != null);

                if (_dragThumbnail == null)
                {
                    return;
                }

                // ドラッグ開始地点保管
                _dragStartPoint = e.Location;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region ThumbnailUnit_MouseMove - サムネイル上でのマウス移動時

        /// <summary>
        /// サムネイル上でマウスが移動された際に実行される処理です。
        /// 一定以上の距離をドラッグした場合、サムネイル移動状態に入ります。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ThumbnailUnit_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (_dragThumbnail == null)
                {
                    return;
                }

                if (!_isDraging)
                {
                    // ドラッグ開始地点からの移動距離が一定以下の場合は何もしない
                    int moveX = e.Location.X - _dragStartPoint.Value.X;
                    int moveY = e.Location.Y - _dragStartPoint.Value.Y;

                    if (Math.Abs(moveX) < 10 && Math.Abs(moveY) < 10)
                    {
                        return;
                    }

                    // 移動距離が一定以上になったら、ゴーストを表示してサムネイル移動状態に入る
                    _isDraging = true;

                    _dragGhost = new Form();
                    _dragGhost.FormBorderStyle = FormBorderStyle.None;
                    _dragGhost.MinimumSize = new Size(10, 10);
                    _dragGhost.Size = _dragThumbnail.PictureBox.Size;
                    _dragGhost.BackColor = _dragThumbnail.BackColor;
                    _dragGhost.BackgroundImage = _dragThumbnail.PictureBox.Image;
                    _dragGhost.BackgroundImageLayout = ImageLayout.Zoom;
                    _dragGhost.Opacity = 0.5;
                    _dragGhost.ShowInTaskbar = false;
                    _dragGhost.Visible = true;
                }

                if (_isDraging)
                {
                    // ゴーストをマウスに追随して移動
                    _dragGhost.Location = new Point(
                        MousePosition.X - _dragStartPoint.Value.X,
                        MousePosition.Y - _dragStartPoint.Value.Y);

                    // 領域交差判定用にゴーストの領域を保管
                    var ghostRect = new Rectangle(
                        pnlThumbnail.PointToClient(_dragGhost.Location), _dragGhost.Size);

                    bool isAlreadyHit = false;
                    foreach (var other in _thumbnails)
                    {
                        if (other == _dragThumbnail)
                        {
                            continue;
                        }

                        // ドラッグ中以外のサムネイルの表示領域と交差判定を行う
                        var otherRect = new Rectangle(other.Location, other.Size);

                        if (isAlreadyHit)
                        {
                            // 交差しているが2つ目以降に検出された場合は無視
                            other.BorderStyle = BorderStyle.None;
                        }
                        else if (ghostRect.IntersectsWith(otherRect))
                        {
                            // 最初に検出された交差しているサムネイルの位置を移動先としてマーク
                            other.BorderStyle = BorderStyle.Fixed3D;
                            isAlreadyHit = true;
                        }
                        else
                        {
                            // 交差していないサムネイルは無視
                            other.BorderStyle = BorderStyle.None;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region ThumbnailUnit_MouseUp - サムネイル上でのマウスボタン解放時

        /// <summary>
        /// サムネイル上でマウスのボタンが離された際に実行される処理です。
        /// サムネイル移動状態でマウスの左ボタンが離された場合、
        /// ボタンを離した位置に応じてサムネイルの表示順を変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ThumbnailUnit_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button != MouseButtons.Left)
                {
                    return;
                }

                if (_isDraging)
                {
                    // 移動先位置としてマークされているサムネイルを取得
                    ThumbnailUnit dropedLocThumbnail = null;
                    foreach (var thumbnail in _thumbnails)
                    {
                        if (thumbnail.BorderStyle == BorderStyle.Fixed3D)
                        {
                            dropedLocThumbnail = thumbnail;
                            thumbnail.BorderStyle = BorderStyle.None;
                            break;
                        }
                    }

                    if (dropedLocThumbnail != null)
                    {
                        // 移動先の位置にドラッグしたサムネイルを移動
                        int dropedIndex = pnlThumbnail.Controls.GetChildIndex(dropedLocThumbnail);
                        pnlThumbnail.Controls.SetChildIndex(_dragThumbnail, dropedIndex);

                        // 内部のリストを表示に併せて更新
                        ImageList = pnlThumbnail.Controls.OfType<ThumbnailUnit>()
                            .Select(t => t.ImageData).ToList();
                    }

                    // ゴーストを破棄しサムネイル移動状態を解除
                    _dragGhost.Close();
                    _dragGhost = null;
                    _isDraging = false;
                }

                _dragStartPoint = null;
                _dragThumbnail = null;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion
    }
}
