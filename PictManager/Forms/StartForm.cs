using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.IO;
using SO.PictManager.Common;
using SO.PictManager.Forms.Info;
using SO.PictManager.DataModel;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 開始フォームクラス
    /// </summary>
    public sealed partial class StartForm : Form
    {
        #region クラス定数

        /// <summary>フォームタイトル書式</summary>
        private const string FORM_TITLE_FORMAT = "PictManager - {0}";

        /// <summary>カテゴリーIDとカテゴリー名の区切り文字</summary>
        private const char CATEGORY_SEPARATOR = '_';

        #endregion

        #region インスタンス変数

        /// <summary>ファイル監視で最後に作成されたファイル名(5秒保持)</summary>
        private string _lastRenameByWatch;

        /// <summary>パネル間の空白領域</summary>
        private readonly int _spacer;

        /// <summary>ファイルモード時の画面サイズ</summary>
        private readonly Size _formSizeInFileMode;

        /// <summary>データベースモード時の画面サイズ</summary>
        private readonly Size _formSizeInDatabaseMode;

        /// <summary>未選択用のブランクカテゴリー</summary>
        private readonly MstCategory _blankCategory;

        /// <summary>選択されているタグ検索結果リストのインデックス</summary>
        private int? _selectedTagIndex = null;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public StartForm()
        {
            // コンポーネント初期化
            InitializeComponent();

            // 画面サイズセット
            _spacer = 6;
            _formSizeInDatabaseMode = this.Size;
            _formSizeInFileMode = new Size(this.Width, this.Height - (pnlForDatabase.Height + _spacer));

            // 未選択用のカテゴリーを用意
            _blankCategory = new MstCategory()
            {
                CategoryId = int.MinValue,
                CategoryName = string.Empty,
            };

            // タグ検索テキストボックスにサジェストを設定
            var tagSuggest = new AutoCompleteStringCollection();

            // フォーム状態設定
            ChangeImageMode();
        }

        #endregion

        #region ChangeImageMode - 読込画像データモード切替

        /// <summary>
        /// 読込画像データモードに応じてフォームの状態を切り替えます。
        /// </summary>
        private void ChangeImageMode()
        {
            if (Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.File)
            {
                // 表示状態切替
                this.Text = string.Format(FORM_TITLE_FORMAT, "ファイルモード");
                lblTargetFolder.Text = "対象フォルダ";
                btnOpenUrlDrop.Visible = false;
                btnMaintenance.Visible = false;

                // パネル表示切替
                pnlForDatabase.Visible = false;
                pnlMain.Location = pnlForDatabase.Location;
                this.Size = _formSizeInFileMode;

                // 保存モードボタン設定変更
                btnSaveMode.Text = "監視";
                btnSaveMode.Click -= btnAutoImport_Click;
                btnSaveMode.Click += btnFolderWatch_Click;

                // フォルダ監視処理切替
                fileWatcher.Created -= fileWatcherInDatabaseMode_Created;
                fileWatcher.Created += fileWatcherInFileMode_Created;

                // 状態情報読込
                StateInfo stateInfo = Utilities.State;
                if (!string.IsNullOrEmpty(stateInfo.LastViewPath))
                {
                    txtTargetFolder.Text = stateInfo.LastViewPath;
                    dlgFolderRef.SelectedPath = stateInfo.LastViewPath;
                }
            }
            else
            {
                // 表示状態切替
                this.Text = string.Format(FORM_TITLE_FORMAT, "データベースモード");
                lblTargetFolder.Text = "監視フォルダ";
                btnOpenUrlDrop.Visible = true;
                btnMaintenance.Visible = true;

                // パネル表示切替
                this.Size = _formSizeInDatabaseMode;
                pnlMain.Location = new Point(pnlMain.Location.X, pnlForDatabase.Location.Y + pnlForDatabase.Height + _spacer);
                pnlForDatabase.Visible = true;

                // 保存モードボタン設定変更
                btnSaveMode.Text = "自動取込";
                btnSaveMode.Click -= btnFolderWatch_Click;
                btnSaveMode.Click += btnAutoImport_Click;

                // フォルダ監視処理切替
                fileWatcher.Created -= fileWatcherInFileMode_Created;
                fileWatcher.Created += fileWatcherInDatabaseMode_Created;

                // 有効なカテゴリーとタグを更新
                UpdateCategoriesAndTags();

                lstSearchedTags.Items.Clear();

                // 状態情報読込
                StateInfo stateInfo = Utilities.State;
                if (!string.IsNullOrEmpty(stateInfo.LastAutoImportPath))
                {
                    txtTargetFolder.Text = stateInfo.LastAutoImportPath;
                    dlgFolderRef.SelectedPath = stateInfo.LastAutoImportPath;
                }
            }

            this.Update();
        }

        #endregion

        #region UpdateCategoriesAndTags - 有効なカテゴリーとタグを更新

        /// <summary>
        /// 画面上で有効なカテゴリーとタグを更新します。
        /// </summary>
        private void UpdateCategoriesAndTags()
        {
            using (var entities = new PictManagerEntities())
            {
                // 最後に選択していたカテゴリーのIDを保管
                int lastSelectedCategoryId;
                if (cmbCategory.SelectedItem == null)
                {
                    lastSelectedCategoryId = _blankCategory.CategoryId;
                }
                else
                {
                    lastSelectedCategoryId = (cmbCategory.SelectedItem as MstCategory).CategoryId;
                }

                // カテゴリーをクリアし未選択用カテゴリーを追加
                cmbCategory.Items.Clear();
                cmbCategory.Items.Add(_blankCategory);
                int selectedIndex = 0;

                // カテゴリー読込
                var categories = entities.MstCategories.OrderBy(c => c.CategoryName).ToArray();
                for (int i = 0; i < categories.Count(); i++)
                {
                    MstCategory category = categories.ElementAt(i);
                    entities.Entry(category).State = EntityState.Detached;
                    cmbCategory.Items.Add(category);

                    // 最後に選択していたものを同じカテゴリーの場合は選択対象とする
                    if (category.CategoryId == lastSelectedCategoryId)
                    {
                        selectedIndex = i + 1;  // 未選択用カテゴリーの分があるので1プラス
                    }
                }
                cmbCategory.SelectedIndex = selectedIndex;

                // タグ検索のサジェストを更新
                txtTagSearch.AutoCompleteCustomSource.Clear();
                txtTagSearch.AutoCompleteCustomSource.AddRange(entities.MstTags.Select(t => t.TagName).ToArray());
            }
        }

        #endregion

        #region ValidateFolderForFileMode - 対象フォルダパスの有効性チェック(ファイルモード用)

        /// <summary>
        /// 対象フォルダパス指定テキストボックスに入力されたパスの有効性をチェックし、
        /// 有効なパスであれば状態情報に保存します。
        /// (ファイルモード用)
        /// </summary>
        /// <param name="isFolderWatch">フォルダ監視時の処理かどうかを示すフラグ</param>
        /// <returns>true:有効なパス / false:無効なパス</returns>
        private bool ValidateFolderForFileMode(bool isFolderWatch)
        {
            Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.File);

            string action = isFolderWatch ? "監視" : "閲覧";

            // 対象フォルダ入力チェック
            if (string.IsNullOrEmpty(txtTargetFolder.Text.Trim()))
            {
                FormUtilities.ShowMessage("E007", string.Format("{0}対象フォルダ", action));
                return false;
            }

            // 対象フォルダ存在チェック
            if (!Directory.Exists(txtTargetFolder.Text))
            {
                FormUtilities.ShowMessage("E006", action);
                return false;
            }

            // 状態情報更新
            Utilities.State.LastViewPath = txtTargetFolder.Text;
            Utilities.SaveStateInfo();

            return true;
        }

        #endregion

        #region ValidateFolderForDatabaseMode - 対象フォルダパスの有効性チェック(データベースモード用)

        /// <summary>
        /// 対象フォルダパス指定テキストボックスに入力されたパスの有効性をチェックし、
        /// 有効なパスであれば状態情報に保存します。
        /// (データベースモード用)
        /// </summary>
        /// <param name="isCreate">自動取込フォルダを作成するかのフラグ</param>
        /// <returns>true:有効なパス / false:無効なパス</returns>
        private bool ValidateFolderForDatabaseMode(out bool isCreate)
        {
            Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database);

            isCreate = false;

            // 対象フォルダ入力チェック
            if (string.IsNullOrEmpty(txtTargetFolder.Text.Trim()))
            {
                FormUtilities.ShowMessage("E007", string.Format("{0}対象フォルダ", "自動取込"));
                return false;
            }

            // 対象フォルダ存在チェック
            if (Directory.Exists(txtTargetFolder.Text))
            {
                if (FormUtilities.ShowMessage("Q017") == DialogResult.Yes)
                {
                    isCreate = true;
                }
            }
            else
            {
                isCreate = true;
            }

            // 状態情報更新
            Utilities.State.LastAutoImportPath = txtTargetFolder.Text;
            Utilities.SaveStateInfo();

            return true;
        }

        #endregion

        #region CreateAutoImportFolder - 自動取込フォルダ作成

        /// <summary>
        /// 自動取込フォルダを作成します。
        /// 既に同名フォルダが存在する場合、配下のフォルダとファイルも含め削除された後に再作成されます。
        /// </summary>
        private void CreateAutoImportFolder()
        {
            Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database);

            string path = txtTargetFolder.Text;
            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            Directory.CreateDirectory(path);

            using (var entities = new PictManagerEntities())
            {
                foreach (var category in entities.MstCategories)
                {
                    string childDirName = Path.Combine(
                        path, category.CategoryId.ToString() + CATEGORY_SEPARATOR + category.CategoryName);

                    Directory.CreateDirectory(childDirName);
                }
            }
        }

        #endregion

        #region BeginFolderWatch - フォルダ管理開始

        /// <summary>
        /// フォルダ監視を開始し、ウィンドウを最小化します。
        /// </summary>
        /// <param name="path">監視対象のフォルダパス</param>
        /// <param name="iconText">タスクトレイアイコンの表示文字列</param>
        /// <param name="baloonText">タスクトレイアイコンのバルーンチップの文字列</param>
        private void BeginFolderWatch(string path, string iconText, string baloonText)
        {
            fileWatcher.Path = path;
            fileWatcher.EnableRaisingEvents = true;

            WindowState = FormWindowState.Minimized;

            notifyIcon.Visible = true;
            notifyIcon.Text = iconText;
            notifyIcon.ShowBalloonTip(3000, baloonText, fileWatcher.Path, ToolTipIcon.Info);

            ShowInTaskbar = false;
        }

        #endregion

        #region EndFolderWatch - フォルダ監視終了

        /// <summary>
        /// フォルダ監視を終了し、ウィンドウを元の状態に戻します。
        /// </summary>
        private void EndFolderWatch()
        {
            fileWatcher.EnableRaisingEvents = false;
            notifyIcon.Visible = false;
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Invalidate(true);
        }

        #endregion

        //*** イベントハンドラ ***

        #region txtTagSearch_KeyDown - タグ検索テキストボックスキー押下時

        /// <summary>
        /// タグ検索テキストボックスでキーを押下した際に実行される処理です。
        /// Enterキー押下時にタグ検索ボタン押下時と同等の処理を行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtTagSearch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.KeyCode & Keys.Return) == Keys.Return)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;

                    btnTagSearch_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnTagSearch_Click - タグ検索ボタン押下時

        /// <summary>
        /// タグ検索ボタンをクリックした際に実行される処理です。
        /// テキストボックスに入力された内容に該当するタグを検索します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnTagSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtTagSearch.Text))
                {
                    return;
                }

                lstSearchedTags.Items.Clear();

                // 検索条件に該当するタグをリストボックスに表示
                string searchValue = txtTagSearch.Text.ToLower();
                using (var entities = new PictManagerEntities())
                {
                    var tags = from tag in entities.MstTags
                               where tag.TagName.ToLower().Contains(searchValue)
                               select tag;

                    foreach (var tag in tags)
                    {
                        entities.Entry(tag).State = EntityState.Detached;
                        lstSearchedTags.Items.Add(tag);
                    }
                }

                if (lstSearchedTags.Items.Count > 0)
                {
                    lstSearchedTags.Focus();
                    lstSearchedTags.SelectedIndex = 0;
                }
                else
                {
                    txtTagSearch.Focus();
                    txtTagSearch.SelectAll();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region lstSearchedTags_SelectedIndexChanged - タグ検索結果リストボックス選択インデックス変更時

        /// <summary>
        /// タグ検索結果リストボックスの選択インデックスを変更した際に実行される処理です。
        /// 選択済みのインデックスが再選択された場合、選択を解除します。
        /// 選択されていないインデックスが選択された場合、既に他に選択されているインデックスがある場合は
        /// その選択を解除し、最後に選択されたもののみを選択状態にします。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void lstSearchedTags_SelectedIndexChanged(object sender, EventArgs e)
        {
            lstSearchedTags.SelectedIndexChanged -= lstSearchedTags_SelectedIndexChanged;

            try
            {
                if (lstSearchedTags.SelectedIndices.Count == 0)
                {
                    // 選択が解除された場合
                    _selectedTagIndex = null;
                }
                else if (lstSearchedTags.SelectedIndices.Count == 1)
                {
                    // 単一の項目が選択されている場合
                    _selectedTagIndex = lstSearchedTags.SelectedIndices[0];
                }
                else
                {
                    // 複数の項目が選択されている場合、最後に選択したもののみを選択状態にする
                    _selectedTagIndex = lstSearchedTags.SelectedIndices
                        .Cast<int>().Except(new[] { _selectedTagIndex.Value }).First();
                    
                    while (lstSearchedTags.SelectedIndices.Count > 0)
                    {
                        lstSearchedTags.SetSelected(lstSearchedTags.SelectedIndices[0], false);
                    }

                    lstSearchedTags.SetSelected(_selectedTagIndex.Value, true);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                lstSearchedTags.SelectedIndexChanged += lstSearchedTags_SelectedIndexChanged;
            }
        }

        #endregion

        #region btnFolderBrowse_Click - フォルダー参照ボタン押下時

        /// <summary>
        /// フォルダー参照ボタンをクリックした際に実行される処理です。
        /// 対象フォルダー選択ダイアログを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnFolderBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(txtTargetFolder.Text))
                {
                    dlgFolderRef.SelectedPath = txtTargetFolder.Text;
                }

                // 対象フォルダーを取得
                if (dlgFolderRef.ShowDialog(this) == DialogResult.OK)
                {
                    txtTargetFolder.Text = dlgFolderRef.SelectedPath;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnMaintenance_Click - メンテナンスボタンクリック時

        /// <summary>
        /// メンテナンスボタンをクリックした際に実行される処理です。
        /// データベースメンテナンスフォームを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnMaintenance_Click(object sender, EventArgs e)
        {
            Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database);

            try
            {
                var mainteForm = new MaintenanceForm();
                mainteForm.FormClosed += (sender2, e2) =>
                {
                    // 有効なカテゴリーとタグを更新
                    UpdateCategoriesAndTags();

                    this.ShowInTaskbar = true;
                    this.Show();
                    this.Activate();
                };

                // 自フォーム非表示
                this.ShowInTaskbar = false;
                this.Hide();

                mainteForm.Show();
                mainteForm.Activate();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnConfig_Click - システム設定ボタン押下時

        /// <summary>
        /// システム設定ボタンをクリックした際に実行される処理です。
        /// システム設定ダイアログを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnConfig_Click(object sender, EventArgs e)
        {
            using (var dialog = new ConfigDialog())
            {
                dialog.Show(this);
            }

            // フォーム状態設定
            ChangeImageMode();
        }

        #endregion

        #region btnFolderWatch_Click - フォルダ監視ボタン押下時

        /// <summary>
        /// フォルダ監視ボタンを押下した際に実行される処理です。
        /// ウィンドウをタスクトレイに格納し、指定フォルダの監視を開始します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnFolderWatch_Click(object sender, EventArgs e)
        {
            try
            {
                // 対象チェック、状態更新
                if (!ValidateFolderForFileMode(true))
                {
                    return;
                }

                // フォルダ監視開始
                BeginFolderWatch(
                    txtTargetFolder.Text,
                    "フォルダ監視中 - " + Environment.NewLine + fileWatcher.Path,
                    "フォルダ監視開始");
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnAutoImport_Click - 自動取込ボタン押下時

        /// <summary>
        /// 自動取込ボタンを押下した際に実行される処理です。
        /// ウィンドウをタスクトレイに格納し、指定フォルダの監視を開始します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnAutoImport_Click(object sender, EventArgs e)
        {
            Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database);

            try
            {
                // 対象チェック、状態更新
                bool isCreate;
                if (!ValidateFolderForDatabaseMode(out isCreate))
                {
                    return;
                }

                // 自動取込フォルダ作成
                if (isCreate)
                {
                    CreateAutoImportFolder();
                }

                // 自動取込開始
                BeginFolderWatch(
                    txtTargetFolder.Text,
                    "自動取込中 - " + Environment.NewLine + fileWatcher.Path,
                    "自動取込開始");
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnOpenUrlDrop_Click - URL受付ボタン押下時

        /// <summary>
        /// URL受付ボタンをクリックした際に実行される処理です。
        /// URL受付フォームを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnOpenUrlDrop_Click(object sender, EventArgs e)
        {
            Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database);

            try
            {
                var urlDropForm = new UrlDropForm();
                urlDropForm.FormClosed += (sender2, e2) =>
                {
                    this.ShowInTaskbar = true;
                    this.Show();
                    this.Activate();
                };

                // 自フォーム非表示
                this.ShowInTaskbar = false;
                this.Hide();

                urlDropForm.Show();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnView_Click - 閲覧ボタン押下時

        /// <summary>
        /// 閲覧ボタンをクリックした際に実行される処理です。
        /// 選択したモードで対象ディレクトリ内のファイルを操作する為の子フォームを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                BaseForm frmViewer;
                if (Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.File)
                {
                    // 対象チェック、状態更新
                    if (!ValidateFolderForFileMode(false))
                    {
                        return;
                    }

                    // 対象ディレクトリの内容を展開
                    bool includeSub = Utilities.Config.CommonInfo.IsIncludeSubDirectory;
                    if (rdoSlide.Checked)
                    {
                        frmViewer = new SlideForm(txtTargetFolder.Text, includeSub);       // スライドショー表示
                    }
                    else if (rdoList.Checked)
                    {
                        frmViewer = new ListForm(txtTargetFolder.Text, includeSub);        // 一覧表示
                    }
                    else
                    {
                        frmViewer = new ThumbnailForm(txtTargetFolder.Text, includeSub);   // サムネイル表示
                    }
                }
                else
                {
                    var category = cmbCategory.SelectedItem as MstCategory;
                    if (category == _blankCategory)
                    {
                        // 未選択用カテゴリーが選択されている場合は対象カテゴリーをnullに読み替え
                        category = null;
                    }
                    var tag = lstSearchedTags.SelectedItem as MstTag;

                    // 閲覧対象未指定警告
                    if (category == null && tag == null)
                    {
                        FormUtilities.ShowMessage("W030");
                        return;
                    }

                    if (rdoSlide.Checked)
                    {
                        frmViewer = new SlideForm(category, tag);       // スライドショー表示
                    }
                    else if (rdoList.Checked)
                    {
                        frmViewer = new ListForm(category, tag);        // 一覧表示
                    }
                    else
                    {
                        frmViewer = new ThumbnailForm(category, tag);   // サムネイル表示
                    }
                }

                if (frmViewer.ImageCount == 0)
                {
                    FormUtilities.ShowMessage("W031");
                    return;
                }

                frmViewer.Disposed += (sender2, e2) =>
                {
                    // 有効なカテゴリーとタグを更新
                    UpdateCategoriesAndTags();
                };

                frmViewer.Show(this);
                this.Hide();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnQuit_Click - 終了ボタン押下時

        /// <summary>
        /// 終了ボタンをクリックした際に実行される処理です。
        /// 終了確認後、全ての実行中リソースを破棄してアプリケーションを終了します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnQuit_Click(object sender, EventArgs e)
        {
            try
            {
                // 終了確認を行う設定の場合かつフォルダ指定がある場合(入力中含む)、終了確認を行う
                if (Utilities.Config.CommonInfo.IsConfirmQuit
                    && !string.IsNullOrEmpty(txtTargetFolder.Text.Trim()))
                {
                    if (FormUtilities.ShowMessage("Q000") == DialogResult.No)
                    {
                        return;
                    }
                }

                // 自動取込フォルダが残っている場合は削除
                if (Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database
                    && Directory.Exists(Utilities.State.LastAutoImportPath))
                {
                    Directory.Delete(Utilities.State.LastAutoImportPath, true);
                }

                Dispose();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region Form_FormClosing - ×ボタン押下時

        /// <summary>
        /// ×ボタンをクリックした際に実行される処理です。
        /// 終了確認後、全ての実行中リソースを破棄してアプリケーションを終了します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                // 発生元が自フォームで無い場合はそのまま終了
                if (sender is StartForm)
                {
                    // 終了確認を行う設定の場合かつフォルダ指定がある場合(入力中含む)、終了確認を行う
                    if (Utilities.Config.CommonInfo.IsConfirmQuit
                        && !string.IsNullOrEmpty(txtTargetFolder.Text.Trim())
                        && FormUtilities.ShowMessage("Q000") == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                // 自動取込フォルダが残っている場合は削除
                if (Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database
                    && Directory.Exists(Utilities.State.LastAutoImportPath))
                {
                    Directory.Delete(Utilities.State.LastAutoImportPath, true);
                }

                if (notifyIcon.Visible)
                {
                    notifyIcon.Visible = false;
                }
            }
            catch(Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region txtTargetFolder_DragDrop - 対象フォルダ指定テキストボックスへのDrag&Drop時

        /// <summary>
        /// 対象フォルダ指定テキストボックスにファイルがドラッグアンドドロップされた際に実行される処理です。
        /// ドラッグアンドドロップされたファイルの絶対パスをテキストボックスに表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtTargetFolder_DragDrop(object sender, DragEventArgs e)
        {
            // ファイルドロップのみ許可
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                var dropFiles = e.Data.GetData(DataFormats.FileDrop) as string[];

                // 複数のファイルがドロップされた場合はエラーとする
                if (dropFiles.Length != 1)
                {
                    FormUtilities.ShowMessage("E010");
                    return;
                }

                // ドロップされたファイルのパスをテキストボックスへ設定
                txtTargetFolder.Text = dropFiles.Single();
            }
        }

        #endregion

        #region txtTargetFolder_DragEnter - 対象フォルダ指定テキストボックス領域へのカーソルEnter時

        /// <summary>
        /// 対象フォルダ指定テキストボックスにドラッグアンドドロップ時のマウスカーソルが入った際に実行される処理です。
        /// マウスカーソルをファイル追加時カーソルに変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtTargetFolder_DragEnter(object sender, DragEventArgs e)
        {
            // ファイルドロップのみ許可
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        #endregion

        #region txtTargetFolder_Enter - 対象フォルダ指定テキストボックスゲットフォーカス時

        /// <summary>
        /// 対象フォルダ指定テキストボックスにフォーカスが与えられた際に実行される処理です。
        /// テキストボックス内の全てのテキストを選択状態にします。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtTargetFolder_Enter(object sender, EventArgs e)
        {
            // テキスト全選択
            txtTargetFolder.SelectAll();
        }

        #endregion

        #region fileWatcherInFileMode_Created - 監視フォルダへのファイル作成時(ファイルモード用)

        /// <summary>
        /// ファイルモード時の監視中のフォルダにファイルが作成された際に実行される処理です。
        /// 作成されたファイルに対して、自動的にリネームを行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void fileWatcherInFileMode_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                lock (this)
                {
                    // フォルダの場合は無視
                    if (!File.Exists(e.FullPath))
                    {
                        return;
                    }
                    Thread.Sleep(1000);

                    // 指定秒数以内に連続して同じファイルが作成された場合は重複イベントとみなす
                    if (e.FullPath == _lastRenameByWatch)
                    {
                        return;
                    }

                    // 監視対象の拡張子かチェック
                    List<string> extentions = Utilities.Config.CommonInfo.TargetExtensions;
                    string ext = Path.GetExtension(e.Name);
                    if (!extentions.Contains(ext.Substring(1)))
                    {
                        return;
                    }

                    // 対象フォルダ内のファイル数を取得
                    var dir = new DirectoryInfo(Path.GetDirectoryName(e.FullPath));
                    int num = dir.GetFiles().Length - 1;
                    Func<string> createFilePath = () =>
                        Path.Combine(dir.FullName, string.Format("{0}_{1}{2}", dir.Name, (num++).ToString(), ext));

                    // ファイル名生成
                    const int RETRY_MAX = 30;
                    int retry = 0;
                    string newPath;
                    while (File.Exists(newPath = createFilePath()))
                    {
                        if (++retry > RETRY_MAX)
                        {
                            // エラーログ出力
                            Utilities.Logger.WriteLog(GetType().FullName, MethodBase.GetCurrentMethod().Name,
                                MessageXml.GetMessageInfo("E008", e.FullPath).message);
                            return;
                        }
                    }

                    // リネーム実行
                    var moveFile = new FileInfo(e.FullPath);
                    moveFile.MoveTo(newPath);

                    _lastRenameByWatch = e.FullPath;
                    watchDupliTimer.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                EndFolderWatch();
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region fileWatcherInDatabaseMode_Created - 監視フォルダへのファイル作成時(データベースモード用)

        /// <summary>
        /// データベースモード時の監視中のフォルダにファイルが作成された際に実行される処理です。
        /// 作成されたファイルに対して、自動的にリネームを行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void fileWatcherInDatabaseMode_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                lock (this)
                {
                    // フォルダの場合は無視
                    if (!File.Exists(e.FullPath))
                    {
                        return;
                    }
                    Thread.Sleep(3000);

                    // 取込対象の拡張子かチェック
                    List<string> extentions = Utilities.Config.CommonInfo.TargetExtensions;
                    string ext = Path.GetExtension(e.Name).Substring(1);
                    if (!extentions.Contains(ext))
                    {
                        return;
                    }

                    // カテゴリーID取得
                    string dirName = new DirectoryInfo(Path.GetDirectoryName(e.FullPath)).Name;
                    int categoryId = int.Parse(dirName.Split(CATEGORY_SEPARATOR)[0]);
                    
                    // エンティティ生成
                    DateTime now = DateTime.Now;
                    var entity = new TblImage();

                    using (Image img = Image.FromFile(e.FullPath))
                    {
                        entity.ImageData = new ImageConverter().ConvertTo(img, typeof(byte[])) as byte[];
                    }
                    entity.ImageFormat = ext;
                    entity.CategoryId = categoryId;
                    entity.InsertedDateTime = now;
                    entity.UpdatedDateTime = now;

                    // 画像データをデータベースに登録
                    using (var entities = new PictManagerEntities())
                    {
                        entities.TblImages.Add(entity);
                        entities.SaveChanges();
                    }

                    // 画像ファイルを削除
                    File.Delete(e.FullPath);
                }
            }
            catch (Exception ex)
            {
                EndFolderWatch();
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region watchDupliTimer_Tick - ファイル監視イベント重複発生監視タイマー処理

        /// <summary>
        /// ファイル監視イベントの重複発生監視用タイマーの処理です。
        /// 指定時間が経過した後、イベント重複チェックに使用するファイルパスをクリアします。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void watchDupliTimer_Tick(object sender, EventArgs e)
        {
            _lastRenameByWatch = null;
        }

        #endregion

        #region notifyIcon_MouseDoubleClick - タスクトレイアイコンダブルクリック時

        /// <summary>
        /// タスクトレイアイコンダブルクリック時の処理です。
        /// 監視を終了し、ウィンドウを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                EndFolderWatch();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuOpenFolder_Click - 監視対象のフォルダを開くメニューアイテム選択時

        /// <summary>
        /// 監視対象のフォルダを開くメニューアイテム選択時の処理です。
        /// 監視対象のフォルダを開きます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuOpenFolder_Click(object sender, EventArgs e)
        {
            try
            {
                Utilities.OpenExplorer(txtTargetFolder.Text);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuEndWatch_Click - 監視終了メニューアイテム選択時

        /// <summary>
        /// 監視終了メニューアイテム選択時の処理です。
        /// 監視を終了し、ウィンドウを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuEndWatch_Click(object sender, EventArgs e)
        {
            try
            {
                EndFolderWatch();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuQuit_Click - アプリケーション終了メニューアイテム選択時

        /// <summary>
        /// アプリケーション終了メニューアイテム選択時の処理です。
        /// アプリケーションを終了します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuQuit_Click(object sender, EventArgs e)
        {
            try
            {
                Close();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion
    }
}
