using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.IO;
using SO.PictManager.Common;
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// データベースメンテナンスフォーム
    /// </summary>
    public partial class MaintenanceForm : Form
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public MaintenanceForm()
        {
            Debug.Assert(Utilities.Config.CommonInfo.Mode == ConfigInfo.ImageDataMode.Database);

            InitializeComponent();

            using (var entities = new PictManagerEntities())
            {
                // カテゴリ読込
                RefreshCategoriesComboBox(entities);

                // 物理削除済み画像数取得
                var deletedCount = entities.TblImages.Where(i => i.DeleteFlag).Count();
                lblDeletedCount.Text = deletedCount.ToString("#,0");
                btnApplyDelete.Enabled = deletedCount > 0;
            }

            lblStatus.Text = string.Empty;

            if (Utilities.Config.CommonInfo.TargetExtensions.Any())
            {
                // 対象ファイルフィルタ作成
                var filterPattern = new StringBuilder();
                foreach (var ext in Utilities.Config.CommonInfo.TargetExtensions)
                {
                    if (filterPattern.Length > 0)
                    {
                        filterPattern.Append("|");
                    }

                    filterPattern.AppendFormat("{0}ファイル|*.{0}", ext);
                }
                dlgImportFile.Filter = filterPattern.ToString();

                // ファイル参照初期ディレクトリ設定
                dlgImportFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            }
            else
            {
                grpImport.Enabled = false;
            }

            btnViewDeletedFiles.Enabled = Utilities.IsExistsDeletedFile;
        }

        #endregion

        #region RefreshCategoriesComboBox - カテゴリー表示系コンボボックス内容更新

        /// <summary>
        /// カテゴリー表示系のコンボボックスの内容を最新の状態に更新します。
        /// </summary>
        /// <param name="entity">エンティティオブジェクト</param>
        private void RefreshCategoriesComboBox(PictManagerEntities entity)
        {
            // インポート先カテゴリ
            var allCategories = entity.MstCategories.OrderBy(c => c.CategoryName).ToList();

            cmbImportCategory.DataSource = allCategories;
            cmbImportCategory.DisplayMember = "CategoryName";
            cmbImportCategory.SelectedIndex = 0;

            // 削除カテゴリ
            var deletableCategories = allCategories.Where(c => c.CategoryId != Constants.UN_CLASSIFIED_CATEGORY_ID).ToList();

            cmbDeleteCategory.DataSource = deletableCategories;
            cmbDeleteCategory.DisplayMember = "CategoryName";

            if (deletableCategories.Any())
            {
                cmbDeleteCategory.SelectedIndex = 0;
                btnDeleteCategory.Enabled = true;
            }
            else
            {
                btnDeleteCategory.Enabled = false;
            }
        }

        #endregion

        #region EntryCategory - カテゴリー登録

        /// <summary>
        /// テキストボックスに入力されたカテゴリーをデータベースに登録します。
        /// </summary>
        private void EntryCategory()
        {
            lblStatus.Text = string.Empty;

            // カテゴリー名入力チェック
            string categoryName = txtEntryCategory.Text.Trim();
            if (string.IsNullOrEmpty(categoryName))
            {
                FormUtilities.ShowMessage("W000", "カテゴリー名");
                return;
            }

            using (var entities = new PictManagerEntities())
            {
                // カテゴリー名重複チェック
                if (entities.MstCategories.Any(c => c.CategoryName == categoryName))
                {
                    FormUtilities.ShowMessage("E009", categoryName);
                    return;
                }

                // カテゴリー登録
                var dto = new MstCategory();
                dto.CategoryName = categoryName;

                DateTime now = DateTime.Now;
                dto.InsertedDateTime = now;
                dto.UpdatedDateTime = now;

                entities.MstCategories.Add(dto);
                entities.SaveChanges();

                // コンボボックス内容を更新
                RefreshCategoriesComboBox(entities);
            }

            txtEntryCategory.Text = string.Empty;
            lblStatus.Text = MessageXml.GetMessageInfo("I011",
                string.Format("カテゴリー[{0}]の登録", categoryName)).message;
        }

        #endregion

        #region ImportFile - ファイルインポート

        /// <summary>
        /// 画面で指定されたファイルのインポートを行います。
        /// </summary>
        private void ImportFile()
        {
            // ファイル存在チェック
            if (!File.Exists(txtTargetPath.Text))
            {
                FormUtilities.ShowMessage("W024", "ファイル");
                return;
            }

            // ファイル形式チェック
            if (!Utilities.IsAvailableFormat(txtTargetPath.Text, false))
            {
                FormUtilities.ShowMessage("W025");
                return;
            }

            Cursor = Cursors.WaitCursor;
            using (var progress = new CircleProgressDialog(this))
            {
                // プログレス表示開始
                progress.StartProgress();

                // 指定されたファイルをインポート
                InsertImage(txtTargetPath.Text);
            }

            if (FormUtilities.ShowMessage("Q018", string.Format("[{0}] ", txtTargetPath.Text)) == DialogResult.Yes)
            {
                // インポートしたファイルを削除
                Utilities.DeleteFile(txtTargetPath.Text, true);
                btnViewDeletedFiles.Enabled = true;
            }
        }

        #endregion

        #region ImportDirectory - ディレクトリインポート

        /// <summary>
        /// 画面で指定されたディレクトリ配下の全ファイルのインポートを行います。
        /// </summary>
        private void ImportDirectory()
        {
            // ディレクトリ存在チェック
            if (!Directory.Exists(txtTargetPath.Text))
            {
                FormUtilities.ShowMessage("W024", "ディレクトリ");
                return;
            }

            Cursor = Cursors.WaitCursor;

            // サーチオプション設定
            SearchOption opt = chkIncludeSubDirectory.Checked ?
                SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            // 対象ディレクトリ内の画像ファイルパスを全取得
            var filePathList = new List<string>();
            using (var progress = new ProgressDialog(this))
            {
                progress.StartProgress("インポート対象ファイル検索中...",
                    string.Empty, 0, Utilities.Config.CommonInfo.TargetExtensions.Count);

                foreach (var ext in Utilities.Config.CommonInfo.TargetExtensions)
                {
                    progress.Message = ext + "ファイル";

                    filePathList.AddRange(Directory.GetFiles(txtTargetPath.Text, "*." + ext, opt));

                    progress.PerformStep();
                }
            }

            // 対象ファイルが存在確認
            if (!filePathList.Any())
            {
                FormUtilities.ShowMessage("W028");
                return;
            }

            // ファイルインポート実施
            using (var progress = new ProgressDialog(this))
            {
                progress.StartProgress("画像ファイルインポート中...", string.Empty, 0, filePathList.Count);

                foreach (var path in filePathList)
                {
                    progress.Message = path;

                    InsertImage(path);

                    progress.PerformStep();
                }
            }

            if (FormUtilities.ShowMessage("Q018", string.Format("{0} ファイル", filePathList.Count)) == DialogResult.Yes)
            {
                // インポートしたファイルを削除
                using (var progress = new ProgressDialog(this))
                {
                    progress.StartProgress("インポート済みファイルの削除中...", string.Empty, 0, filePathList.Count);

                    foreach (var path in filePathList)
                    {
                        progress.Message = path;

                        Utilities.DeleteFile(path, true);

                        progress.PerformStep();
                    }
                }

                btnViewDeletedFiles.Enabled = true;
            }
        }

        #endregion

        #region InsertImage - 画像データをデータベースに登録

        /// <summary>
        /// 指定されたパスに存在するファイルの画像データをデータベースのTblImagesに登録します。
        /// パスの存在チェックやファイル形式チェックは行われません。
        /// </summary>
        /// <param name="filePath">インポートするファイルのパス</param>
        private void InsertImage(string filePath)
        {
            DateTime now = DateTime.Now;
            var entity = new TblImage();

            // エンティティ生成
            using (var img = Image.FromFile(filePath))
            {
                entity.ImageData = new ImageConverter().ConvertTo(img, typeof(byte[])) as byte[];
            }
            entity.ImageFormat = Path.GetExtension(filePath).Substring(1);
            entity.CategoryId = (cmbImportCategory.SelectedValue as MstCategory).CategoryId;
            entity.Md5 = Cryptgrapher.GetBytesMD5(entity.ImageData);
            entity.InsertedDateTime = now;
            entity.UpdatedDateTime = now;

            using (var entities = new PictManagerEntities())
            {
                entities.TblImages.Add(entity);
                entities.SaveChanges();
            }
        }

        #endregion

        //*** イベントハンドラ ***

        #region txtEntryCategory_KeyDown - 登録カテゴリーテキストボックスキーダウン時

        /// <summary>
        /// 登録カテゴリーテキストボックスでキーが押下された際に実行される処理です。
        /// テキストボックスに入力されたカテゴリーをデータベースに登録します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtEntryCategory_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.KeyCode & Keys.Return) == Keys.Return)
                {
                    // リターンキー押下時、カテゴリーを登録
                    EntryCategory();

                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }

        }

        #endregion

        #region btnEntryCategory_Click - カテゴリー登録ボタンクリック時

        /// <summary>
        /// カテゴリー登録ボタンがクリックされた際に実行される処理です。
        /// テキストボックスに入力されたカテゴリーをデータベースに登録します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnEntryCategory_Click(object sender, EventArgs e)
        {
            try
            {
                EntryCategory();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnDeleteCategory_Click - カテゴリー削除ボタンクリック時

        /// <summary>
        /// カテゴリー削除ボタンがクリックされた際に実行される処理です。
        /// 選択されたカテゴリーをデータベースから削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedCategory = cmbDeleteCategory.SelectedItem as MstCategory;

                using (var entities = new PictManagerEntities())
                {
                    // 削除対象のカテゴリーに属している画像を検索
                    var relationImages = from i in entities.TblImages
                                         where i.CategoryId == selectedCategory.CategoryId
                                         select i;

                    // 削除確認
                    if (relationImages.Any())
                    {
                        if (FormUtilities.ShowMessage("Q013", selectedCategory.CategoryName) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else if (FormUtilities.ShowMessage("Q012", selectedCategory.CategoryName) == DialogResult.No)
                    {
                        return;
                    }

                    // 選択されたカテゴリーを削除
                    var deleteObj = (from c in entities.MstCategories
                                     where c.CategoryId == selectedCategory.CategoryId
                                     select c).First();

                    entities.MstCategories.Remove(deleteObj);

                    // 削除したカテゴリーに属していた画像を未分類に更新
                    foreach (var img in relationImages)
                    {
                        img.CategoryId = Constants.UN_CLASSIFIED_CATEGORY_ID;
                    }

                    entities.SaveChanges();

                    // コンボボックス内容を更新
                    RefreshCategoriesComboBox(entities);
                }

                lblStatus.Text = MessageXml.GetMessageInfo("I011", "カテゴリーの削除").message;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }

        }

        #endregion

        #region rdoImportKinds_CheckedChanged - インポート対象ラジオボタン変更時

        /// <summary>
        /// インポート対象ラジオボタンの状態が変更された際に実行される処理です。
        /// 選択されたインポート対象の種類に応じてフォームの状態を切り替えます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void rdoImportKinds_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                chkIncludeSubDirectory.Enabled = rdoImportDirectory.Checked;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnRef_Click - 参照ボタン押下時

        /// <summary>
        /// 参照ボタンをクリックした際に実行される処理です。
        /// インポート対象がディレクトリの場合はディレクトリ選択ダイアログを、
        /// インポート対象がファイルの場合はファイル選択ダイアログを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnRef_Click(object sender, EventArgs e)
        {
            try
            {
                if (rdoImportDirectory.Checked) // ディレクトリ
                {
                    if (Directory.Exists(txtTargetPath.Text))
                    {
                        dlgImportDirectory.SelectedPath = txtTargetPath.Text;
                    }

                    // 対象ディレクトリを取得
                    if (dlgImportDirectory.ShowDialog(this) == DialogResult.OK)
                    {
                        txtTargetPath.Text = dlgImportDirectory.SelectedPath;
                    }
                }
                else // ファイル
                {
                    if (File.Exists(txtTargetPath.Text))
                    {
                        dlgImportFile.FileName = txtTargetPath.Text;
                    }

                    // 対象ファイルを取得
                    if (dlgImportFile.ShowDialog(this) == DialogResult.OK)
                    {
                        txtTargetPath.Text = dlgImportFile.FileName;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnImport_Click - インポートボタン押下時

        /// <summary>
        /// インポートボタンがクリックされた際に実行される処理です。
        /// 指定された対象をデータベースにインポートします。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                // 入力チェック
                if (string.IsNullOrEmpty(txtTargetPath.Text))
                {
                    FormUtilities.ShowMessage("W000", "インポート対象");
                    return;
                }

                try
                {
                    if (rdoImportDirectory.Checked)
                    {
                        ImportDirectory();
                    }
                    else
                    {
                        ImportFile();
                    }
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnViewDeletedFiles_Click - 削除ファイル確認ボタン押下時

        /// <summary>
        /// 削除ファイル確認ボタンがクリックされた際に実行される処理です。
        /// 一時退避ディレクトリをエクスプローラで表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnViewDeletedFiles_Click(object sender, EventArgs e)
        {
            try
            {
                Utilities.ViewDeletedFiles();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnApplyDelete_Click - 完全に削除ボタンクリック

        /// <summary>
        /// 完全に削除ボタンがクリックされた際に実行される処理です。
        /// 論理削除済みの画像データを物理削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnApplyDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // 削除確認
                if (FormUtilities.ShowMessage("Q014") == DialogResult.No)
                {
                    return;
                }

                // 物理削除実行
                using (var entities = new PictManagerEntities())
                {
                    var deletedImages = entities.TblImages.Where(i => i.DeleteFlag);

                    foreach (var img in deletedImages)
                    {
                        entities.TblImages.Remove(img);
                    }

                    entities.SaveChanges();
                }

                lblDeletedCount.Text = "0";
                btnApplyDelete.Enabled = false;

                FormUtilities.ShowMessage("I011", "削除画像の完全消去");
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnClose_Click -  閉じるボタンクリック時

        /// <summary>
        /// 閉じるボタンがクリックされた際に実行される処理です。
        /// 画面を閉じます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
