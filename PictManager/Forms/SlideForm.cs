using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;

using SO.Library.Drawing;
using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.Forms.Extensions;
using SO.Library.IO;
using SO.Library.Text;
using SO.PictManager.Components;
using SO.PictManager.Common;
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;

using CursorFace = System.Windows.Forms.Cursor;
using Config = System.Configuration.ConfigurationManager;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// スライドショー表示フォームクラス
    /// </summary>
    public sealed partial class SlideForm : ViewImageForm
    {
        #region クラス定数

        /// <summary>イメージファイル無し時の表示テキスト</summary>
        private const string NO_IMAGE_LABEL = "No image file in \nselected folder.";
        /// <summary>削除済時の表示テキスト</summary>
        private const string DELETED_IMAGE_LABEL = "This image is deleted.";

        #endregion 

        #region インスタンス変数

        /// <summary>スライド表示制御用スレッド</summary>
        private Thread _slideThread;

        /// <summary>スライドショー実行中フラグ</summary>
        private bool _slideFlg = false;

        /// <summary>スライド表示の切り替え間隔</summary>
        private int _slideInterval;

        /// <summary>類似画像表示用サムネイルフォーム</summary>
        private ThumbnailForm _thumbnailForm;

        /// <summary>ブックマークフォーム</summary>
        private BookmarkForm _bookmarkForm;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// ファイルモード用のコンストラクタです。
        /// </summary>
        /// <param name="targetPath">対象ディレクトリパス</param>
        /// <param name="includeSubFlg">サブディレクトリ以下を含むかを示すフラグ</param>
        public SlideForm(string targetPath, bool includeSubFlg)
        {
            // コンポーネント初期化
            InitializeComponent();

            // フィールド初期化
            ImageMode = ConfigInfo.ImageDataMode.File;
            TargetDirectory = new DirectoryInfo(targetPath);
            IncludeSubFlg = includeSubFlg;

            // 共通処理
            ConstructCommon();
        }

        /// <summary>
        /// データベースモード用のコンストラクタです。
        /// </summary>
        /// <param name="category">対象カテゴリー</param>
        public SlideForm(MstCategory category)
        {
            // コンポーネント初期化
            InitializeComponent();

            // フィールド初期化
            ImageMode = ConfigInfo.ImageDataMode.Database;
            TargetCategory = category;

            // 共通処理
            ConstructCommon();
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
            lblCount.Text = ImageCount.ToString();
            txtIndex.MaxLength = lblCount.Text.Length;

            // ソート順コンボボックス構築
            cmbSort.SelectedIndexChanged -= cmbSort_SelectedIndexChanged;
            ImageSorter.BindSortOrderDataSource(cmbSort);
            cmbSort.SelectedValue = Utilities.State.SortOrder;
            ImageList = ImageSorter.Sort(ImageList, Utilities.State.SortOrder, ImageMode).ToList();
            cmbSort.SelectedIndexChanged += cmbSort_SelectedIndexChanged;

            // 最初の画像を基底クラスの表示対象ファイルプロパティに設定
            if (ImageCount > 0)
            {
                ImageData = ImageList.First();
            }
            else
            {
                txtIndex.Text = string.Empty;
                txtIndex.Enabled = false;
                lblStatus.Text = string.Empty;
                ShowInformationLabel(NO_IMAGE_LABEL);
            }

            // UI制御
            InitializeAccessibility();
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
            if (ImageMode == ConfigInfo.ImageDataMode.File)
            {
                // ファイル
                menuTemp = new ToolStripMenuItem("ファイル(&F)", null, null, "menuFile");
                menuTemp.ShortcutKeys = Keys.Alt | Keys.F;
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("戻る", null, btnClose_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("上書き保存", null, (s, e) => SaveImage()));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("対象ファイル再取得", null, menuRefresh_Click));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("ディレクトリを開く", null, (s, e) => Utilities.OpenExplorer(TargetDirectory.FullName)));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("表示画像ファイル名変更", null, menuRename_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("一括ファイル名変更", null, menuRenameAll_Click));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("表示画像移動", null, menuMove_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("一括ファイル移動", null, menuMoveAll_Click));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("表示画像削除", null, btnDelete_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("削除済画像確認", null,
                        (s, e) => ViewDeletedFiles()));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("終了", null,
                        (s, e) => Form_FormClosing(s, new FormClosingEventArgs(CloseReason.UserClosing, false))));
                barMenu.Items.Add(menuTemp);
            }

            // 操作
            menuTemp = new ToolStripMenuItem("操作(&O)", null, null, "menuOpe");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.O;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("次へ", null, btnNext_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("前へ", null, btnPrevious_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            var menuIndex = new ToolStripTextBox("menuTxtIndex");
            menuIndex.KeyDown += txtIndex_KeyDown;
            menuIndex.TextChanged += txtIndex_TextChanged;
            menuTemp.DropDownItems.Add(menuIndex);
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("指定インデックスの画像へジャンプ", null,
                    (s, e) => DisplayPictureByTextBoxValue()));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("削除確認しない", null,
                    (s, e) => ((ToolStripMenuItem)s).Checked = !((ToolStripMenuItem)s).Checked, "menuChkConfirm"));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("ブックマークウィンドウを開く", null,
                    (s, e) => ShowBookmarkForm()));
            barMenu.Items.Add(menuTemp);

            // 表示
            menuTemp = new ToolStripMenuItem("表示(&V)", null, null, "menuView");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.V;
            var menuSizeMode = new ToolStripMenuItem("表示サイズモード", null, GetSizeModeMenuItems().ToArray());
            menuSizeMode.Name = "menuCmbSizeMode";
            menuTemp.DropDownItems.Add(menuSizeMode);
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("右に90°回転", null,
                    (s, e) => RotateImage(RotateFlipType.Rotate90FlipNone)));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("左に90°回転", null,
                    (s, e) => RotateImage(RotateFlipType.Rotate270FlipNone)));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            var menuInterval = new ToolStripTextBox("menuTxtInterval");
            menuInterval.KeyDown += menuTxtInterval_KeyDown;
            menuTemp.DropDownItems.Add(menuInterval);
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("スライド表示", null, menuChkSlide_Click, "menuChkSlide"));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("グレースケール表示", null,
                    (s, e) => DisplayByGrayScale()));
            barMenu.Items.Add(menuTemp);
        }

        #endregion

        #region InitializeAccessibility - コンポーネントのアクセス制限初期化

        /// <summary>
        /// (ViewImageForm.InitializeAccessibility()をオーバーライドします)
        /// フォーム項目のアクセス可不可の初期設定を行ないます。
        /// </summary>
        protected override void InitializeAccessibility()
        {
            if (ImageCount == 0)
            {
                btnDelete.Enabled = false;
                btnNext.Enabled = false;
                btnPrevious.Enabled = false;
                txtIndex.Enabled = false;
                cmbPicMode.Enabled = false;
            }
        }

        #endregion

        #region RefreshTargetFiles - 対象ファイルリスト最新化

        /// <summary>
        /// 表示中ディレクトリの現在の状態を再取得します。
        /// </summary>
        protected override void RefreshImageList()
        {
            base.RefreshImageList();

            // ファイル総数を更新
            lblCount.Text = ImageCount.ToString();
        }

        #endregion

        #region ChangeAccessibility - コンポーネントのアクセス制御

        /// <summary>
        /// フォーム項目のアクセス可不可を設定します。
        /// </summary>
        /// <param name="accessible">アクセス可不可を示すフラグ</param>
        private void ChangeAccessibility(bool accessible)
        {
            try
            {
                btnClose.Enabled = accessible;
                btnDelete.Enabled = accessible;
                btnNext.Enabled = accessible;
                btnPrevious.Enabled = accessible;
                txtIndex.Enabled = accessible;
                cmbPicMode.Enabled = accessible;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region DisplayPicture - 指定画像表示

        /// <summary>
        /// (ViewImageForm.DisplayPicture()をオーバーライドします)
        /// 現在のインデックスが指し示す画像を表示します。
        /// ファイルが削除済みの場合は、それを示すメッセージを表示します。
        /// </summary>
        protected override void DisplayImage()
        {
            try
            {
                // スクロールバー位置初期化
                if (pnlParent.Panel1.VerticalScroll.Visible)
                    pnlParent.Panel1.VerticalScroll.Value = 0;
                if (pnlParent.Panel1.HorizontalScroll.Visible)
                    pnlParent.Panel1.HorizontalScroll.Value = 0;

                // イメージファイルがあるか確認
                if (ImageCount == 0)
                {
                    lblStatus.Text = string.Empty;
                    ShowInformationLabel(NO_IMAGE_LABEL);
                    return;
                }

                // PictureBox更新
                if (ImageList[CurrentIndex].IsDeleted)
                {
                    // 既に対象イメージが削除されている場合は非表示
                    ShowInformationLabel(DELETED_IMAGE_LABEL);
                }
                else
                {
                    // イメージ表示
                    ImageData = ImageList[CurrentIndex];
                    base.DisplayImage();
                }

                // 表示中情報更新
                txtIndex.Text = (CurrentIndex + 1).ToString();
                ShowImageInfoByStatusBar();

            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region DisplayPictureByIndex - 指定されたインデックスの画像を表示

        /// <summary>
        /// インデックスを直接指定し、画像を表示します。
        /// </summary>
        private void DisplayPictureByIndex(int idx)
        {
            try
            {
                int factIdx;
                if (idx < 0)
                    factIdx = 0;
                else if (idx > ImageCount - 1)
                    factIdx = ImageCount - 1;
                else
                    factIdx = idx;

                // 表示更新
                CurrentIndex = factIdx;
                ImageData = ImageList[CurrentIndex];
                DisplayImage();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region DisplayPictureByTextBoxValue - インデックス直接指定ボックスで指定された画像を表示

        /// <summary>
        /// インデックス直接指定ボックスで指定されたインデックスの画像を表示します。
        /// </summary>
        private void DisplayPictureByTextBoxValue()
        {
            try
            {
                int idx;
                if (!int.TryParse(txtIndex.Text, out idx))
                {
                    FormUtilities.ShowMessage("W004");
                    txtIndex.Text = CurrentIndex.ToString();
                    return;
                }
                --idx;

                DisplayPictureByIndex(idx);
                txtIndex.Text = (CurrentIndex + 1).ToString();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region SearchNextValidIndex - 次の未削除画像のインデックス取得

        /// <summary>
        /// 次の有効な(削除されていない)画像のインデックスを検索します。
        /// </summary>
        /// <returns>次の画像のインデックス</returns>
        private int SearchNextValidIndex()
        {
            int ret = -1;   // 戻り値、有効なイメージが無い場合は-1のまま
            try
            {
                // 現在のインデックスから末尾までを検索
                int i;
                for (i = CurrentIndex; i < ImageCount; ++i)
                {
                    if (!ImageList[i].IsDeleted)
                    {
                        ret = i;
                        break;
                    }
                }

                // 末尾まで検索して見つからなかった場合は先頭から再検索
                if (i >= ImageCount)
                {
                    int j;
                    for (j = 0; j < CurrentIndex; ++j)
                    {
                        if (!ImageList[j].IsDeleted)
                        {
                            ret = j;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                // 念の為、戻り値を-1(無効値)に再設定
                ret = -1;
            }

            return ret;
        }

        #endregion

        #region SaveBookmark - ブックマーク保存

        /// <summary>
        /// 現在表示している画像をブックマークとして保存します。
        /// </summary>
        private void SaveBookmark()
        {
            var bookmarkFile = new FileInfo(Config.AppSettings[BookmarkInfo.SAVE_PATH_KEY]);
            XDocument xdoc;
            if (bookmarkFile.Exists)
            {
                xdoc = XDocument.Load(bookmarkFile.FullName);
            }
            else
            {
                xdoc = new XDocument(new XElement("Bookmarks"));
            }

            // ブックマーク追加
            int id = xdoc.Root.HasElements
                ? xdoc.Root.Descendants().Max(elm => int.Parse(elm.Attribute("Id").Value)) + 1
                : 0;

            xdoc.Root.Add(
                new XElement("Bookmark",
                    new XAttribute("Id", id.ToString()),
                    new XAttribute("Name", string.Format("{0} - {1}", id.ToString(), TargetDirectory.Name)),
                    new XAttribute("FileNo", txtIndex.Text),
                    new XAttribute("Timestamp", DateTime.Now.ToString(BookmarkInfo.TIMESTAMP_FORMAT)),
                    new XAttribute("SortOrder", cmbSort.SelectedValue.ToString())));

            xdoc.Save(bookmarkFile.FullName);

            // ブックマークウィンドウを開いている場合は内容を更新
            if (_bookmarkForm != null)
                _bookmarkForm.RefreshBookmarks();
        }

        #endregion

        #region ShowBookmarkForm - ブックマークウィンドウを開く

        /// <summary>
        /// ブックマークウィンドウを開きます。
        /// 既に開かれている場合、ブックマークウィンドウをアクティブに設定します。
        /// </summary>
        private void ShowBookmarkForm()
        {
            try
            {
                if (_bookmarkForm != null)
                {
                    _bookmarkForm.Activate();
                    return;
                }

                try
                {
                    _bookmarkForm = new BookmarkForm((ImageSortOrder)cmbSort.SelectedValue);
                    _bookmarkForm.BookmarkJump += (sender, e) => DisplayPictureByIndex(e.Bookmark.FileNo - 1);
                    _bookmarkForm.FormClosed += (sender, e) => _bookmarkForm = null;

                    _bookmarkForm.Show();
                }
                catch
                {
                    if (_bookmarkForm != null)
                    {
                        _bookmarkForm.Dispose();
                        _bookmarkForm = null;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region SaveStateInfo - 状態情報保存

        /// <summary>
        /// 状態情報をシリアライズしてXMLファイルとして保存します。
        /// </summary>
        protected override void SaveStateInfo()
        {
            try
            {
                Utilities.State.SortOrder = (ImageSortOrder)cmbSort.SelectedValue;
                base.SaveStateInfo();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region CloseForm - フォームをクローズ

        /// <summary>
        /// フォームをクローズします。
        /// </summary>
        protected override void CloseForm()
        {
            // 各リソースを破棄
            if (_thumbnailForm != null)
                _thumbnailForm.Dispose();
            if (_bookmarkForm != null)
                _bookmarkForm.Dispose();
            if (_slideThread != null)
                _slideThread.Abort();

            base.CloseForm();
        }

        #endregion

        #region イベントハンドラ

        #region Form_Shown - フォーム初期表示時

        /// <summary>
        /// (ViewImageForm.Form_Shown(sender, EventArgs)をオーバーライドします)
        /// フォームが表示された際に実行される処理です。
        /// 画面レイアウトを初期化します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void Form_Shown(object sender, EventArgs e)
        {
            base.Form_Shown(sender, e);
            Form_ResizeEnd(sender, e);
        }

        #endregion

        #region Form_FormClosing - ×ボタン押下時

        /// <summary>
        /// (ViewImageForm.Form_FormClosing(sender, EventArgs)をオーバーライドします)
        /// ×ボタンがクリックされた際に実行される処理です。
        /// 終了確認後、アプリケーションを終了します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 終了確認
                if (Utilities.Config.CommonInfo.IsConfirmQuit
                    && FormUtilities.ShowMessage("Q000") == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                // 各リソースを破棄
                if (_thumbnailForm != null)
                    _thumbnailForm.Dispose();
                if (_bookmarkForm != null)
                    _bookmarkForm.Dispose();
                if (_slideThread != null)
                    _slideThread.Abort();

                if (Owner != null) Owner.Dispose();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
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
                // スライド表示中はキー入力無効
                if (_slideFlg)
                {
                    e.Handled = true;
                    return;
                }

                // 修飾キーが付加されている場合は通常処理
                if ((e.KeyCode & Keys.Alt) != Keys.Alt &&
                        (e.KeyCode & Keys.Control) != Keys.Control &&
                        (e.KeyCode & Keys.Shift) != Keys.Shift)
                {
                    Keys kcode = e.KeyCode & Keys.KeyCode;
                    switch (kcode)
                    {
                        case Keys.Left:
                        case Keys.Up:
                            if (txtIndex.Focused) break;

                            // 前のファイルへ
                            btnPrevious_Click(sender, e);
                            e.Handled = true;
                            break;

                        case Keys.Right:
                        case Keys.Down:
                            if (txtIndex.Focused) break;

                            // 次のファイルへ
                            btnNext_Click(sender, e);
                            e.Handled = true;
                            break;

                        case Keys.Delete:
                            if (txtIndex.Focused) break;

                            // 表示中のファイルを削除
                            btnDelete_Click(sender, e);
                            e.Handled = true;
                            break;

                        case Keys.F5:
                            // 対象ファイルリフレッシュ
                            menuRefresh_Click(sender, e);
                            e.Handled = true;
                            break;

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

        #region Form_Resize - フォームサイズ変更時

        /// <summary>
        /// フォームのサイズが変更された際に実行される処理です。
        /// 各ボタン、コントロールの配置を再設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void Form_Resize(object sender, EventArgs e)
        {
            base.Form_Resize(sender, e);

            const int SPACE_WIDTH = 6;

            // 中央のコントロール群の位置調整
            int x = pnlParent.Width / 2 - lblCountDelim.Width / 2 - txtIndex.Width - btnPrevious.Width - SPACE_WIDTH * 2;
            int leftLimit = chkLupe.Location.X + chkLupe.Width + SPACE_WIDTH;
            if (x < leftLimit)
            {
                x = leftLimit;
            }

            btnPrevious.Location = new Point(x, btnPrevious.Location.Y);
            x += btnPrevious.Width + SPACE_WIDTH;

            txtIndex.Location = new Point(x, txtIndex.Location.Y);
            x += txtIndex.Width + SPACE_WIDTH;

            lblCountDelim.Location = new Point(x, lblCountDelim.Location.Y);
            x += lblCountDelim.Width + SPACE_WIDTH;

            lblCount.Location = new Point(x, lblCount.Location.Y);
            x += lblCount.Width + SPACE_WIDTH;

            btnNext.Location = new Point(x, btnNext.Location.Y);

            // 右のコントロール群の位置調整
            x = pnlParent.Width - btnClose.Width - cmbPicMode.Width - cmbSort.Width
                - btnZoomOut.Width - btnZoomIn.Width - SPACE_WIDTH * 5;
            leftLimit = btnNext.Location.X + btnNext.Width + SPACE_WIDTH;
            if (x < leftLimit)
            {
                x = leftLimit;
            }

            btnZoomIn.Location = new Point(x, btnZoomIn.Location.Y);
            x += btnZoomIn.Width + SPACE_WIDTH;

            btnZoomOut.Location = new Point(x, btnZoomOut.Location.Y);
            x += btnZoomOut.Width + SPACE_WIDTH;

            cmbSort.Location = new Point(x, cmbSort.Location.Y);
            x += cmbSort.Width + SPACE_WIDTH;

            cmbPicMode.Location = new Point(x, cmbPicMode.Location.Y);
            x += cmbPicMode.Width + SPACE_WIDTH;

            btnClose.Location = new Point(x, btnClose.Location.Y);
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
                // 最後に表示していたファイルを取得
                // (削除されている場合は次の有効イメージを対象とする)
                int lastIdx;
                if ((lastIdx = SearchNextValidIndex()) == -1)
                {
                    // 有効なイメージが無い場合は対象ファイル無しのキャプションを表示
                    RefreshImageList();
                    DisplayImage();
                    return;
                }

                // 最終表示ファイルパスを検索用に保存
                string lastFile = ImageList[lastIdx].Key;

                // 対象ファイルを最新化
                RefreshImageList();

                // 最後に表示していたファイルを再表示
                int idx = SearchFileIndex(lastFile);
                CurrentIndex = idx == -1 ? 0 : idx;

                lblCount.Text = (ImageCount + 1).ToString();
                DisplayImage();
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
            // 最後に表示していたファイルを取得
            // (削除されている場合は次の有効イメージを対象とする)
            int lastIdx;
            if ((lastIdx = SearchNextValidIndex()) == -1)
            {
                // 有効なイメージが無い場合は対象ファイル無しのキャプションを表示
                RefreshImageList();
                DisplayImage();
                return;
            }

            // 最終表示ファイルパスを検索用に保存
            string lastFile = ImageList[lastIdx].Key;

            // ファイル名一括変更
            if (RenameAllFiles() == ResultStatus.OK)
            {
                // 表示初期化
                CurrentIndex = 0;
            }
            else
            {
                // 最後に表示していたファイルを再表示
                int idx = SearchFileIndex(lastFile);
                CurrentIndex = idx == -1 ? 0 : idx;
            }
            lblCount.Text = (ImageCount + 1).ToString();
            DisplayImage();
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
            // 対象ファイル最新化
            RefreshImageList();

            // ファイル移動実行、正常終了時はディレクトリ選択フォームへ戻る
            if (MoveAllFiles() == ResultStatus.OK) this.BackToOwner();
        }

        #endregion

        #region menuMove_Click - 表示画像移動メニュー押下時

        /// <summary>
        /// (ViewImageForm.menuMove_Click(object, EventArgs)をオーバーライドします)
        /// 表示画像移動メニューがクリックされた際に実行される処理です。
        /// 表示中の画像を指定ディレクトリに移動し、ファイルリストを最新化します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void menuMove_Click(object sender, EventArgs e)
        {
            // ファイル名変更
            if (MoveFile() != ResultStatus.OK) return;

            // 次の有効イメージを表示
            menuRefresh_Click(sender, e);
        }

        #endregion

        #region menuChkSlide_Click - スライド表示メニュー押下時

        /// <summary>
        /// スライド表示メニューがクリックされた際に実行される処理です。
        /// ファイルリストの全画像をスライドショーで表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuChkSlide_Click(object sender, EventArgs e)
        {
            var menuChklide = (ToolStripMenuItem)sender;
            try
            {
                menuChklide.Checked = !menuChklide.Checked;
                if (menuChklide.Checked)
                {
                    var menuTxt = FormUtilities.GetMenuItem<ToolStripTextBox>(
                            barMenu.Items, "menuView/menuTxtInterval");
                    Action<bool, string> ActCancel = (focusing, msgId) =>
                    {
                        if (focusing)
                        {
                            // 切替間隔指定ボックスにフォーカスセット
                            ((ToolStripMenuItem)menuTxt.OwnerItem).ShowDropDown();
                            menuTxt.Focus();
                            menuTxt.SelectAll();
                        }
                        menuChklide.Checked = false;
                        if (msgId != null)
                        {
                            // エラー通知
                            FormUtilities.ShowMessage(msgId);
                        }
                    };

                    // 入力チェック
                    if (menuTxt.Text == string.Empty)
                    {
                        ActCancel(true, "W007");
                        return;
                    }
                    // 数値チェック(フィールドへの入力値代入も含む)
                    if (!int.TryParse(menuTxt.Text, out _slideInterval))
                    {
                        ActCancel(true, "W008");
                        return;
                    }
                    // 表示間隔0以下は不可
                    if (_slideInterval < 1)
                    {
                        ActCancel(true, "W008");
                        return;
                    }

                    if (FormUtilities.ShowMessage("Q003") == DialogResult.Yes)
                    {
                        _slideFlg = true;   // フォームのキーイベント抑制用フラグON
                        _slideInterval *= 1000;
                        ChangeAccessibility(false);

                        // 画像自動切替スレッド作成
                        _slideThread = new Thread(new ThreadStart(() =>
                        {
                            while (true)
                            {
                                Thread.Sleep(_slideInterval);
                                if (this.IsHandleCreated && this.InvokeRequired)
                                    // フォームのハンドルが生きていてかつ、
                                    // フォームのスレッドに処理を委譲する必要がある場合
                                    this.Invoke(new Action(() => btnNext_Click(this, new EventArgs())));
                                else
                                    btnNext_Click(this, new EventArgs());
                            }
                        }
                        ));

                        _slideThread.IsBackground = true;
                        _slideThread.Start();
                    }
                    else
                    {
                        ActCancel(false, null);   // チェック状態をチェック無しに戻す
                    }
                }
                else
                {
                    if (_slideThread != null)
                    {
                        // スレッドを破棄し、破棄完了まで待機
                        _slideThread.Abort();
                        _slideThread.Join();
                        _slideThread = null;
                    }
                    ChangeAccessibility(true);
                    _slideFlg = false;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuTxtInterval_KeyDown - 画像切替間隔テキストボックスでのキー押下時

        /// <summary>
        /// 画像切替間隔メニューでキーが押下された際に実行される処理です。
        /// Enterキーが押下された場合、スライドショー表示を開始します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuTxtInterval_KeyDown(object sender, KeyEventArgs e)
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
                            // スライド表示チェックをONにし、スライド表示開始
                            var menuChkSlide = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                                    barMenu.Items, "menuView/menuChkSlide");
                            menuChkSlide.Checked = true;
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

        #region btnDelete_Click - 削除ボタン押下時

        /// <summary>
        /// 削除ボタンがクリックされた際に実行される処理です。
        /// 表示中の画像の削除を行ないます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // 既に削除済か確認
                if (ImageList[CurrentIndex].IsDeleted)
                {
                    FormUtilities.ShowMessage("E006");
                    return;
                }

                // 削除確認
                var menuChkConfirm = FormUtilities.GetMenuItem<ToolStripMenuItem>(
                        barMenu.Items, "menuOpe/menuChkConfirm");
                if (!menuChkConfirm.Checked)
                    if (FormUtilities.ShowMessage("Q002") == DialogResult.No) return;

                // PictureBoxリソース解放
                picViewer.Image.Dispose();

                // 対象ファイルを削除
                ImageList[CurrentIndex].Delete();

                // 次の有効イメージを表示
                if ((CurrentIndex = SearchNextValidIndex()) == -1)
                {
                    FormUtilities.ShowMessage("I005");
                    this.BackToOwner();
                }
                else
                {
                    if (CurrentIndex == 0)
                        FormUtilities.ShowMessage("I000");
                    DisplayImage();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnBookmark_Clicked - ブックマークボタン押下時

        /// <summary>
        /// ブックマークボタンが押下された際に実行される処理です。
        /// 表示中の画像をブックマークに追加します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnBookmark_Clicked(object sender, EventArgs e)
        {
            try
            {
                SaveBookmark();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkSimilar_CheckedChanged - 類似画像表示チェックボックス変更時

        /// <summary>
        /// 類似画像表示チェックボックスが変更された際に実行される処理です。
        /// 表示中の画像と類似したものをパスリストから検索してサムネイルフォームで表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkSimilar_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkSimilar.Checked)
                {
                    // 類似画像を検索しサムネイルリストで表示
                    List<IImage> similarImages =
                        ImageController.GetSimilarImages(this, ImageList[CurrentIndex]);
                    if (!similarImages.Any())
                    {
                        FormUtilities.ShowMessage("I008");
                        chkSimilar.Checked = false;
                    }
                    else
                    {
                        _thumbnailForm = new ThumbnailForm(similarImages);
                        _thumbnailForm.Text = string.Format("PictManager - 類似画像検索結果 [{0}]",
                                ImageList[CurrentIndex]);
                        _thumbnailForm.StatusBarText = string.Format("[{0}] の類似画像を表示中 - {1}件",
                                ImageList[CurrentIndex], similarImages.Count);
                        _thumbnailForm.Disposed += new EventHandler(
                            (obj, fce) =>
                            {
                                chkSimilar.Checked = false;
                                RefreshImageList();
                            });

                        _thumbnailForm.Show(this);
                        _thumbnailForm.Activate();
                    }
                }
                else
                {
                    // 表示しているサムネイルフォームを破棄
                    if (_thumbnailForm != null)
                        _thumbnailForm.Dispose();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkLupe_CheckedChanged - ルーペ表示チェックボックス変更時

        /// <summary>
        /// ルーペ表示チェックボックスが変更された際に実行される処理です。
        /// 部分拡大鏡を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkLupe_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (chkLupe.Checked)
                //{
                //    picLupe.Visible = true;
                //    picLupe.BringToFront();
                //    picViewer.MouseMove += new MouseEventHandler(picViewer_MouseMove);
                //}
                //else
                //{
                //    picLupe.Visible = false;
                //    picViewer.MouseMove -= new MouseEventHandler(picViewer_MouseMove);
                //}
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        void picViewer_MouseMove(object sender, MouseEventArgs e)
        {
            picLupe.Location = e.Location;
            using (var g = picLupe.CreateGraphics())
            {
                g.DrawImage(picViewer.Image, new Rectangle(new Point(0, 0), picLupe.Size), new Rectangle(new Point(0, 0), new Size(50, 50)), GraphicsUnit.Pixel);
            }
        }

        #endregion

        #region btnPrevious_Click - 前へボタン押下時

        /// <summary>
        /// 前へボタンがクリックされた際に実行される処理です。
        /// 前のインデックスの画像を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                // 類似画像が表示されている場合は表示フォームを破棄
                if (_thumbnailForm != null)
                    _thumbnailForm.Dispose();

                // 一つ前のインデックスの画像を表示(最初の場合は末尾を表示)
                if (CurrentIndex == 0) CurrentIndex = ImageList.Count - 1;
                else --CurrentIndex;
                DisplayImage();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnNext_Click - 次へボタン押下時

        /// <summary>
        /// 次へボタンがクリックされた際に実行される処理です。
        /// 次のインデックスの画像を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                // 類似画像が表示されている場合は表示フォームを破棄
                if (_thumbnailForm != null)
                    _thumbnailForm.Dispose();

                // 一つ後のインデックスの画像を表示(末尾の場合は最初を表示)
                if (CurrentIndex == ImageCount - 1)
                {
                    CurrentIndex = 0;
                    if (!_slideFlg)
                        // スライド表示中は通知ダイアログは表示しない
                        FormUtilities.ShowMessage("I000");
                }
                else ++CurrentIndex;

                DisplayImage();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region txtIndex_TextChanged - インデックス直接指定ボックス値変更時

        /// <summary>
        /// 画像インデックス指定テキストボックスの内容が変更された際に実行される処理です。
        /// 画像切替間隔メニューとの間で入力された内容の同期を取ります。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtIndex_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // メニューとメイン画面の同期を取る
                ToolStripTextBox menuTxtIndex =
                        FormUtilities.GetMenuItem<ToolStripTextBox>(barMenu.Items, "menuOpe/menuTxtIndex");
                if (sender == txtIndex)
                    menuTxtIndex.Text = txtIndex.Text;
                else
                    txtIndex.Text = menuTxtIndex.Text;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region txtIndex_KeyDown - インデックス直接指定ボックス上でのキー押下時

        /// <summary>
        /// 画像インデックス指定テキストボックスでキーが押下された際に実行される処理です。
        /// Enterキーが押下された場合、入力されたインデックスの画像を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtIndex_KeyDown(object sender, KeyEventArgs e)
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
                            // 指定インデックスの画像を表示
                            DisplayPictureByTextBoxValue();
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

        #region cmbSort_SelectedIndexChanged - ソート順コンボボックス変更時

        /// <summary>
        /// ソート順コンボボックスの選択内容が変更された際に実行される処理です。
        /// ファイルの表示順を選択内容に応じてソートし、先頭から再表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void cmbSort_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // マウスカーソル変更(待機)
                CursorFace.Current = Cursors.WaitCursor;

                // ソート用メソッドセット
                ImageSortOrder order = (ImageSortOrder)cmbSort.SelectedValue;

                // プログレスダイアログ表示
                using (var progDlg = new ProgressDialog(this))
                {
                    MessageInfo msgInfo = MessageXml.GetMessageInfo("I010");
                    progDlg.StartProgressWithMarquee(msgInfo.caption, msgInfo.message);

                    // ファイル名ソート
                    ImageList = ImageSorter.Sort(ImageList, order, ImageMode).ToList();

                    // 先頭の画像を再表示
                    DisplayPictureByIndex(0);

                    // ブックマークウィンドウが開いている場合は有効なソート順を設定
                    if (_bookmarkForm != null)
                        _bookmarkForm.SortOrder = order;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                // マウスカーソル変更(通常)
                CursorFace.Current = Cursors.Default;
            }
        }

        #endregion

        #endregion
    }
}
