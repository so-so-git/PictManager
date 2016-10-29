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
using SO.PictManager.Imaging;

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
                lblLogicalDeletedImages.Text = deletedCount.ToString("#,0");
                btnPhysicalDeleteImages.Enabled = deletedCount > 0;

                // 画像が紐付けられていないタグ数を取得
                var unusedTags = (from tag in entities.MstTags
                                  join tagging in entities.TblTaggings
                                    on tag.TagId equals tagging.TagId into tmpTagging
                                  from joinedTagging in tmpTagging.DefaultIfEmpty()
                                  where joinedTagging == null
                                  select tag
                                 ).Count();
                lblUnusedTags.Text = unusedTags.ToString("#,0");
                btnDeleteUnusedTags.Enabled = unusedTags > 0;

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
                grpImportExport.Enabled = false;
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
            bool isSucceeded;
            using (var progress = new CircleProgressDialog(this))
            {
                // プログレス表示開始
                progress.StartProgress();

                // 指定されたファイルをインポート
                isSucceeded = InsertImage(txtTargetPath.Text);
            }

            if (isSucceeded)
            {
                // 登録成功時
                if (FormUtilities.ShowMessage("Q018", string.Format("[{0}] ", txtTargetPath.Text)) == DialogResult.Yes)
                {
                    // インポートしたファイルを削除
                    Utilities.DeleteFile(txtTargetPath.Text, true);
                    btnViewDeletedFiles.Enabled = true;
                }
            }
            else
            {
                // 登録失敗時
                FormUtilities.ShowMessage("E012");
                txtTargetPath.Focus();
                txtTargetPath.SelectAll();
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
            var skipList = new List<string>();
            using (var progress = new ProgressDialog(this))
            {
                progress.StartProgress("画像ファイルインポート中...", string.Empty, 0, filePathList.Count);

                foreach (var path in filePathList)
                {
                    progress.Message = path;

                    if (!InsertImage(path))
                    {
                        // 重複によりインポートスキップした場合
                        skipList.Add(path);
                    }

                    progress.PerformStep();
                }
            }

            // スキップしたもの以外を抽出
            var importedList = filePathList.Except(skipList);

            if (!importedList.Any())
            {
                // 全重複エラーメッセージ
                FormUtilities.ShowMessage("E013");
                txtTargetPath.Focus();
                txtTargetPath.SelectAll();

                return;
            }
            else if (skipList.Any())
            {
                // 一部重複警告メッセージ
                FormUtilities.ShowMessage("W029", skipList.Count);
            }

            if (FormUtilities.ShowMessage("Q018", string.Format("{0} ファイル", importedList.Count())) == DialogResult.Yes)
            {
                // インポートしたファイルを削除
                using (var progress = new ProgressDialog(this))
                {
                    progress.StartProgress("インポート済みファイルの削除中...", string.Empty, 0, importedList.Count());

                    foreach (var path in importedList)
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
        /// <returns>true:登録正常終了 / false:重複画像の為、取込み中断</returns>
        private bool InsertImage(string filePath)
        {
            // バイナリデータ取得
            byte[] imageData;
            using (var img = Image.FromFile(filePath))
            {
                imageData = new ImageConverter().ConvertTo(img, typeof(byte[])) as byte[];
            }

            using (var entities = new PictManagerEntities())
            {
                // インポートする画像と同じMD5のものが登録済みかを確認
                string md5 = Cryptgrapher.GetBytesMD5(imageData);

                var query = from image in entities.TblImages
                            where image.Md5 == md5
                            select image;

                if (query.Any())
                {
                    // 重複画像
                    return false;
                }

                // エンティティ生成
                DateTime now = DateTime.Now;
                var entity = new TblImage();

                entity.ImageData = imageData;
                entity.ImageFormat = Path.GetExtension(filePath).Substring(1);
                entity.CategoryId = (cmbImportCategory.SelectedValue as MstCategory).CategoryId;
                entity.Md5 = Cryptgrapher.GetBytesMD5(entity.ImageData);
                entity.InsertedDateTime = now;
                entity.UpdatedDateTime = now;

                entities.TblImages.Add(entity);
                entities.SaveChanges();
            }

            return true;
        }

        #endregion

        #region ExportImages - ファイルエクスポート

        /// <summary>
        /// 画面で指定されたディレクトリに画像をエクスポートします。
        /// </summary>
        /// <param name="exportDirectory">エクスポート先ディレクトリ</param>
        private void ExportImages(DirectoryInfo exportDirectory)
        {
            using (var entities = new PictManagerEntities())
            {
                int categoryId = (cmbImportCategory.SelectedValue as MstCategory).CategoryId;

                // 指定されているカテゴリーの画像データを抽出
                var rows = from image in entities.TblImages
                           where image.CategoryId == categoryId
                              && !image.DeleteFlag
                           join category in entities.MstCategories
                             on image.CategoryId equals category.CategoryId
                           select new { Image = image, Category = category };

                // 画像データエクスポート実施
                using (var progress = new ProgressDialog(this))
                {
                    progress.StartProgress("画像データエクスポート中...", string.Empty, 0, rows.Count());

                    foreach (var row in rows)
                    {
                        // 画像データをファイルとして出力
                        string fileName = string.Format("{0}_{1:00000}.{2}",
                            row.Category.CategoryName, row.Image.ImageId, row.Image.ImageFormat);
                        string filePath = Path.Combine(exportDirectory.FullName, fileName);

                        progress.Message = filePath;

                        using (var img = (new ImageConverter().ConvertFrom(row.Image.ImageData) as Image))
                        {
                            img.Save(filePath);
                        }

                        progress.PerformStep();
                    }
                }

                // エクスポート済みファイルの削除確認
                if (FormUtilities.ShowMessage("Q019", string.Format("{0} 画像", rows.Count())) == DialogResult.Yes)
                {
                    using (var progress = new ProgressDialog(this, ProgressBarStyle.Marquee))
                    {
                        foreach (var row in rows)
                        {
                            row.Image.DeleteFlag = true;
                        }
                    }
                    entities.SaveChanges();
                }

                // 物理削除済み画像数取得
                var deletedCount = entities.TblImages.Where(i => i.DeleteFlag).Count();
                lblLogicalDeletedImages.Text = deletedCount.ToString("#,0");
                btnPhysicalDeleteImages.Enabled = deletedCount > 0;
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

        #region rdoImportTargets_CheckedChanged - インポート対象ラジオボタン変更時

        /// <summary>
        /// 実行処理ラジオボタンの状態が変更された際に実行される処理です。
        /// 選択された実行処理の種類に応じてフォームの状態を切り替えます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void rdoImportExportType_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                grpImportTarget.Enabled =
                    chkIncludeSubDirectory.Enabled = rdoTypeImport.Checked;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region rdoImportTargets_CheckedChanged - インポート対象ラジオボタン変更時

        /// <summary>
        /// インポート対象ラジオボタンの状態が変更された際に実行される処理です。
        /// 選択されたインポート対象の種類に応じてフォームの状態を切り替えます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void rdoImportTargets_CheckedChanged(object sender, EventArgs e)
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

        #region btnImportExport_Click - インポート・エクスポート実行ボタン押下時

        /// <summary>
        /// インポート・エクスポート実行ボタンがクリックされた際に実行される処理です。
        /// 指定された処理(インポート または エクスポート)を実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnImportExport_Click(object sender, EventArgs e)
        {
            try
            {
                // 入力チェック
                if (string.IsNullOrEmpty(txtTargetPath.Text))
                {
                    FormUtilities.ShowMessage("W000",
                        rdoTypeImport.Checked ? "インポート対象" : "エクスポート先");
                    return;
                }

                try
                {
                    if (rdoTypeImport.Checked)  // インポート
                    {
                        if (rdoImportDirectory.Checked)
                        {
                            // 指定されたディレクトリをインポート
                            ImportDirectory();
                        }
                        else
                        {
                            // 指定されたファイルをインポート
                            ImportFile();
                        }
                    }
                    else    // エクスポート
                    {
                        // エクスポート先確認
                        var exportDirectory = new DirectoryInfo(txtTargetPath.Text);
                        if (exportDirectory.Exists)
                        {
                            // エクスポート先フォルダ内のファイル存在確認
                            if (exportDirectory.GetFiles().Any())
                            {
                                // 継続確認
                                if (FormUtilities.ShowMessage("Q020") == DialogResult.No)
                                {
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // エクスポート先フォルダ作成確認
                            if (FormUtilities.ShowMessage("Q009", "エクスポート先", exportDirectory.FullName) == DialogResult.No)
                            {
                                return;
                            }

                            // エクスポート先フォルダ作成
                            try
                            {
                                exportDirectory.Create();
                            }
                            catch (IOException)
                            {
                                FormUtilities.ShowMessage("E011");
                                return;
                            }
                        }

                        // 指定されたディレクトリにエクスポート
                        ExportImages(exportDirectory);
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

        #region btnPhysicalDeleteImages_Click - 画像物理削除ボタンクリック

        /// <summary>
        /// 画像物理削除ボタンがクリックされた際に実行される処理です。
        /// 論理削除済みの画像データを物理削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnPhysicalDeleteImages_Click(object sender, EventArgs e)
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
                    // 論理削除されている画像と、それに関連するタグ付けを取得
                    var query = from image in entities.TblImages
                                where image.DeleteFlag
                                join tagging in entities.TblTaggings
                                  on image.ImageId equals tagging.ImageId
                                select new { Image = image, Tagging = tagging} into joined
                                group joined by joined.Image into groups
                                select groups;
                                

                    foreach (var group in query)
                    {
                        // 画像データを削除
                        entities.TblImages.Remove(group.Key);

                        // 関連するタグ付けを全て削除
                        foreach (var entity in group)
                        {
                            entities.TblTaggings.Remove(entity.Tagging);
                        }
                    }

                    entities.SaveChanges();
                }

                lblLogicalDeletedImages.Text = "0";
                btnPhysicalDeleteImages.Enabled = false;

                FormUtilities.ShowMessage("I011", "削除画像の完全消去");
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnDeleteUnusedTags_Click - 不要タグ削除ボタンクリック

        /// <summary>
        /// 不要タグ削除ボタンがクリックされた際に実行される処理です。
        /// 画像が紐付けられていないタグを削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnDeleteUnusedTags_Click(object sender, EventArgs e)
        {
            try
            {
                // 削除確認
                if (FormUtilities.ShowMessage("Q014") == DialogResult.No)
                {
                    return;
                }

                using (var entities = new PictManagerEntities())
                {
                    // 画像が紐付けられていないタグを削除
                    var unusedTags = from tag in entities.MstTags
                                     join tagging in entities.TblTaggings
                                       on tag.TagId equals tagging.TagId into tmpTagging
                                     from joinedTagging in tmpTagging.DefaultIfEmpty()
                                     where joinedTagging == null
                                     select tag;

                    foreach (var unusedTag in unusedTags)
                    {
                        entities.MstTags.Remove(unusedTag);
                    }

                    entities.SaveChanges();
                }

                lblUnusedTags.Text = "0";
                btnDeleteUnusedTags.Enabled = false;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnCheckDuplicate_Click - 重複画像を確認ボタンクリック時

        /// <summary>
        /// 重複画像を確認ボタンがクリックされた際に実行される処理です。
        /// MD5が重複している画像を画像一覧表示フォームで表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnCheckDuplicate_Click(object sender, EventArgs e)
        {
            try
            {
                // 一覧で表示する画像リストの取得デリゲートを定義
                Func<List<IImage>> imageListGetFunction = () =>
                {
                    var duplicatedList = new List<IImage>();

                    // MD5が重複しているものを抽出
                    using (var entities = new PictManagerEntities())
                    {
                        var query = from image in entities.TblImages
                                    where !image.DeleteFlag
                                    group image by image.Md5 into groups
                                    where groups.Count() > 1
                                    from dupli in groups
                                    orderby dupli.Md5, dupli.CategoryId, dupli.ImageId
                                    select dupli;

                        foreach (var row in query)
                        {
                            duplicatedList.Add(new DataImage(row.ImageId));
                        }
                    }

                    return duplicatedList;
                };

                using (var listForm = new ListForm(imageListGetFunction))
                {
                    listForm.ShowDialog(this);
                }

                using (var entities = new PictManagerEntities())
                {
                    // 物理削除済み画像数取得
                    var deletedCount = entities.TblImages.Where(i => i.DeleteFlag).Count();
                    lblLogicalDeletedImages.Text = deletedCount.ToString("#,0");
                    btnPhysicalDeleteImages.Enabled = deletedCount > 0;
                }
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
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion
    }
}
