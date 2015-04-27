using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.IO;
using SO.Library.Text;
using SO.PictManager.Common;
using SO.PictManager.Forms;
using SO.PictManager.Forms.Info;
using SO.PictManager.DataModel;

using Config = System.Configuration.ConfigurationManager;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 対象選択フォームクラス
    /// </summary>
    public sealed partial class StartForm : Form
    {
        #region クラス定数

        /// <summary>フォームタイトル書式</summary>
        private const string FORM_TITLE_FORMAT = "PictManager - 対象{0}指定";

        #endregion

        #region インスタンス変数

        /// <summary>ファイル監視で最後に作成されたファイル名(5秒保持)</summary>
        private string _lastRenameByWatch;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public StartForm()
        {
            // コンポーネント初期化
            InitializeComponent();

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
                this.Text = string.Format(FORM_TITLE_FORMAT, "ディレクトリ");
                txtTargetDirectory.Visible = btnRef.Visible = true;
                cmbCategories.Visible = btnMaintenance.Enabled = false;

                // 保存モードボタン設定変更
                btnSaveMode.Text = "監視";
                btnSaveMode.Click -= btnOpenUrlDrop_Click;
                btnSaveMode.Click += btnDirectoryWatch_Click;

                // 状態情報読込
                StateInfo stateInfo = Utilities.State;
                if (!string.IsNullOrEmpty(stateInfo.LastPath))
                {
                    txtTargetDirectory.Text = stateInfo.LastPath;
                    dlgRef.SelectedPath = stateInfo.LastPath;
                }
            }
            else
            {
                // 表示状態切替
                this.Text = string.Format(FORM_TITLE_FORMAT, "カテゴリ");
                txtTargetDirectory.Visible = btnRef.Visible = false;
                cmbCategories.Visible = btnMaintenance.Enabled = true;

                // 保存モードボタン設定変更
                btnSaveMode.Text = "URL受付";
                btnSaveMode.Click -= btnDirectoryWatch_Click;
                btnSaveMode.Click += btnOpenUrlDrop_Click;

                // カテゴリ読込
                using (var entity = new PictManagerEntities())
                {
                    cmbCategories.DataSource = entity.MstCategories.OrderBy(c => c.CategoryName).ToList();
                    cmbCategories.DisplayMember = "CategoryName";
                }
            }

            this.Update();
        }

        #endregion

        #region ValidateTargetDirectory - 対象ディレクトリパスの有効性チェック

        /// <summary>
        /// 対象ディレクトリパス指定テキストボックスに入力されたパスの有効性をチェックし、
        /// 有効なパスであれば状態情報に保存します。
        /// </summary>
        /// <returns>true:有効なパス / false:無効なパス</returns>
        private bool ValidateTargetDirectory()
        {
            System.Diagnostics.Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.File);

            // 対象ディレクトリ入力チェック
            if (txtTargetDirectory.Text.Trim() == string.Empty)
            {
                FormUtilities.ShowMessage("E007", "対象ディレクトリ");
                return false;
            }

            // 対象ディレクトリ存在チェック
            if (!Directory.Exists(txtTargetDirectory.Text))
            {
                FormUtilities.ShowMessage("E006");
                return false;
            }

            // 状態情報更新
            Utilities.State.LastPath = txtTargetDirectory.Text;
            Utilities.SaveStateInfo();

            return true;
        }

        #endregion

        #region EndWatch - フォルダ監視終了

        /// <summary>
        /// フォルダ監視を終了し、ウィンドウを元の状態に戻します。
        /// </summary>
        private void EndWatch()
        {
            fileWatcher.EnableRaisingEvents = false;
            notifyIcon.Visible = false;
            WindowState = FormWindowState.Normal;
            ShowInTaskbar = true;
            Invalidate(true);
        }

        #endregion

        #region btnRef_Click - 参照ボタン押下時

        /// <summary>
        /// 参照ボタンをクリックした際に実行される処理です。
        /// 対象ディレクトリ選択ダイアログを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnRef_Click(object sender, EventArgs e)
        {
            try
            {
                if (Directory.Exists(txtTargetDirectory.Text))
                    dlgRef.SelectedPath = txtTargetDirectory.Text;

                // 対象ディレクトリを取得
                if (dlgRef.ShowDialog(this) == DialogResult.OK)
                    txtTargetDirectory.Text = dlgRef.SelectedPath;
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
            try
            {
                var mainteForm = new MaintenanceForm();
                mainteForm.FormClosed += (sender2, e2) =>
                {
                    // カテゴリ読込
                    using (var entity = new PictManagerEntities())
                    {
                        cmbCategories.DataSource = entity.MstCategories.OrderBy(c => c.CategoryName).ToList();
                        cmbCategories.DisplayMember = "CategoryName";
                    }

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

        #region btnDirectoryWatch_Click - ディレクトリ監視ボタン押下時

        /// <summary>
        /// ディレクトリ監視ボタンを押下した際に実行される処理です。
        /// ウィンドウをタスクトレイに格納し、指定フォルダの監視を開始します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnDirectoryWatch_Click(object sender, EventArgs e)
        {
            try
            {
                // 対象チェック、状態更新
                if (!ValidateTargetDirectory()) return;

                fileWatcher.Path = txtTargetDirectory.Text;
                fileWatcher.EnableRaisingEvents = true;

                WindowState = FormWindowState.Minimized;

                notifyIcon.Visible = true;
                notifyIcon.Text = "ディレクトリ監視中 - " + Environment.NewLine + fileWatcher.Path;
                notifyIcon.ShowBalloonTip(3000, "ディレクトリ監視開始",
                    fileWatcher.Path, ToolTipIcon.Info);

                ShowInTaskbar = false;
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
                Form frmViewer;
                if (Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.File)
                {
                    // 対象チェック、状態更新
                    if (!ValidateTargetDirectory()) return;

                    // 対象ディレクトリの内容を展開
                    bool includeSub = Utilities.Config.CommonInfo.IsIncludeSubDirectory;
                    if (rdoSlide.Checked)
                        frmViewer = new SlideForm(txtTargetDirectory.Text, includeSub);       // スライドショー表示
                    else if (rdoList.Checked)
                        frmViewer = new ListForm(txtTargetDirectory.Text, includeSub);        // 一覧表示
                    else
                        frmViewer = new ThumbnailForm(txtTargetDirectory.Text, includeSub);   // サムネール表示
                }
                else
                {
                    var category = cmbCategories.SelectedItem as MstCategory;
                    if (rdoSlide.Checked)
                        frmViewer = new SlideForm(category);        // スライドショー表示
                    else if (rdoList.Checked)
                        frmViewer = new ListForm(category);         // 一覧表示
                    else
                        frmViewer = new ThumbnailForm(category);    // サムネール表示
                }

                frmViewer.Show(this);
                Visible = false;
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
                // 終了確認を行う設定の場合かつディレクトリ指定がある場合(入力中含む)、終了確認を行う
                if (Utilities.Config.CommonInfo.IsConfirmQuit
                        && txtTargetDirectory.Text.Trim() != string.Empty)
                    if (FormUtilities.ShowMessage("Q000") == DialogResult.No) return;

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
                    // 終了確認を行う設定の場合かつディレクトリ指定がある場合(入力中含む)、終了確認を行う
                    if (Utilities.Config.CommonInfo.IsConfirmQuit
                        && txtTargetDirectory.Text.Trim() != string.Empty
                        && FormUtilities.ShowMessage("Q000") == DialogResult.No)
                    {
                        e.Cancel = true;
                        return;
                    }
                }

                if (notifyIcon.Visible) notifyIcon.Visible = false;
            }
            catch(Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region txtTargetFolder_DragDrop - 対象指定テキストボックスへのDrag&Drop時

        /// <summary>
        /// 対象ディレクトリ指定テキストボックスにファイルがドラッグアンドドロップされた際に実行される処理です。
        /// ドラッグアンドドロップされたファイルの絶対パスをTextBoxに表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtTargetFolder_DragDrop(object sender, DragEventArgs e)
        {
            // ファイルドロップのみ許可
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] dropFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                // 複数のファイルがドロップされた場合はエラーとする
                if (dropFiles.Length > 1)
                {
                    FormUtilities.ShowMessage("E010");
                    return;
                }
                // ドロップされたファイルのパスをテキストボックスへ設定
                txtTargetDirectory.Text = dropFiles[0];
            }
        }

        #endregion

        #region txtTargetDirectory_DragEnter - 対象指定テキストボックス領域へのカーソルEnter時

        /// <summary>
        /// 対象ディレクトリ指定テキストボックスにドラッグアンドドロップ時のマウスカーソルが入った際に実行される処理です。
        /// マウスカーソルをファイル追加時カーソルに変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtTargetDirectory_DragEnter(object sender, DragEventArgs e)
        {
            // ファイルドロップのみ許可
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Move;
        }

        #endregion

        #region txtTargetDirectory_Enter - 対象指定テキストボックスゲットフォーカス時

        /// <summary>
        /// 対象ディレクトリ指定テキストボックスにフォーカスが与えられた際に実行される処理です。
        /// TextBox内の全てのテキストを選択状態にします。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtTargetDirectory_Enter(object sender, EventArgs e)
        {
            // テキスト全選択
            txtTargetDirectory.SelectAll();
        }

        #endregion

        #region fileWatcher_Created - 監視フィルダへのファイル作成時

        /// <summary>
        /// 監視中のフォルダにファイルが作成された際に実行される処理です。
        /// 作成されたファイルに対して、自動的にリネームを行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void fileWatcher_Created(object sender, FileSystemEventArgs e)
        {
            try
            {
                // ディレクトリの場合は無視
                if (!File.Exists(e.FullPath)) return;

                Thread.Sleep(1000);

                // 5秒以内に連続して同じファイルが作成された場合は重複イベントとみなす
                if (e.FullPath == _lastRenameByWatch)
                    return;

                // 監視対象の拡張子かチェック
                List<string> extentions = Utilities.Config.CommonInfo.TargetExtensions;
                string ext = Path.GetExtension(e.Name);
                if (!extentions.Contains(ext.Substring(1))) return;

                // 対象ディレクトリ内のファイル数を取得
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
            catch (Exception ex)
            {
                EndWatch();
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
                EndWatch();
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
                Utilities.OpenExplorer(txtTargetDirectory.Text);
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
                EndWatch();
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
