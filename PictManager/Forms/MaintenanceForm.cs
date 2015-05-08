using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.PictManager.Common;
using SO.PictManager.DataModel;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// データベースメンテナンスフォーム
    /// </summary>
    public partial class MaintenanceForm : Form
    {
        #region コンストラクタ

        /// <summary>
        /// 唯一のコンストラクタです。
        /// </summary>
        public MaintenanceForm()
        {
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

            if (Utilities.Config.CommonInfo.TargetExtensions.Any())
            {
                // 対象ファイルフィルタ作成
                var filterPattern = new StringBuilder();
                foreach (var ext in Utilities.Config.CommonInfo.TargetExtensions)
                {
                    if (filterPattern.Length > 0)
                        filterPattern.Append("|");

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

        #region ImportFileToDatabase - ファイルをデータベースにインポート

        /// <summary>
        /// 指定されたパスに存在するファイルをデータベースのTblImagesにインポートします。
        /// パスの存在チェックやファイル形式チェックは行われません。
        /// </summary>
        /// <param name="filePath">インポートするファイルのパス</param>
        private void ImportFileToDatabase(string filePath)
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

                FormUtilities.ShowMessage("I011", "カテゴリーの登録");
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

                FormUtilities.ShowMessage("I011", "カテゴリーの削除");
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
                        dlgImportDirectory.SelectedPath = txtTargetPath.Text;

                    // 対象ディレクトリを取得
                    if (dlgImportDirectory.ShowDialog(this) == DialogResult.OK)
                        txtTargetPath.Text = dlgImportDirectory.SelectedPath;
                }
                else // ファイル
                {
                    if (File.Exists(txtTargetPath.Text))
                        dlgImportFile.FileName = txtTargetPath.Text;

                    // 対象ファイルを取得
                    if (dlgImportFile.ShowDialog(this) == DialogResult.OK)
                        txtTargetPath.Text = dlgImportFile.FileName;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnImport_Click - インポートボタンクリック時

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
                    Cursor = Cursors.WaitCursor;

                    using (var progress = new CircleProgressDialog(this))
                    {
                        // プログレス表示開始
                        progress.StartProgress();

                        if (rdoImportDirectory.Checked) // ディレクトリ
                        {
                            // ディレクトリ存在チェック
                            if (!Directory.Exists(txtTargetPath.Text))
                            {
                                FormUtilities.ShowMessage("W024", "ディレクトリ");
                                return;
                            }

                            // サーチオプション設定
                            SearchOption opt = chkIncludeSubDirectory.Checked ?
                                SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                            // 対象ディレクトリ内の画像ファイルパスを全取得
                            foreach (var ext in Utilities.Config.CommonInfo.TargetExtensions)
                            {
                                var filePathList = Directory.GetFiles(txtTargetPath.Text, "*." + ext, opt).ToList();
                                filePathList.ForEach(p => ImportFileToDatabase(p));
                            }
                        }
                        else // ファイル
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

                            // 指定されたファイルをインポート
                            ImportFileToDatabase(txtTargetPath.Text);
                        }
                    }

                    FormUtilities.ShowMessage("I011", "インポート");
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
