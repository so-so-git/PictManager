using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.Net;
using SO.PictManager.Common;
using SO.PictManager.DataModel;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// ダウンロードするURLのドラッグ＆ドロップを受け入れるフォームクラス
    /// </summary>
    public partial class UrlDropForm : Form
    {
        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public UrlDropForm()
        {
            InitializeComponent();
            
            using (var entities = new PictManagerEntities())
            {
                // カテゴリコンボボックス内容設定
                RefreshCategoryComboBox(entities);
            }
        }

        #endregion

        #region RefreshCategoryComboBox - カテゴリコンボボックス最新化

        /// <summary>
        /// カテゴリコンボボックスを最新化します。
        /// </summary>
        /// <param name="entities">エンティティオブジェクト</param>
        private void RefreshCategoryComboBox(PictManagerEntities entities)
        {
            cmbCategory.DataSource = entities.MstCategories.ToList();
            cmbCategory.DisplayMember = "CategoryName";

            if (cmbCategory.Items.Count > 0)
            {
                cmbCategory.SelectedIndex = 0;
                lblDropArea.Visible = true;
            }
            else
            {
                // カテゴリが無い場合はドロップ不可
                lblDropArea.Visible = false;
            }
        }

        #endregion

        //*** イベントハンドラ ***

        #region btnAddCategory_Click - カテゴリー追加ボタン押下時

        /// <summary>
        /// カテゴリー追加ボタンをクリックした際に実行される処理です。
        /// カテゴリー名を入力し、新規登録します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new PictManagerEntities())
                using (var dlg = new CommonInputDialog(
                    "新規カテゴリー追加", "追加するカテゴリー名を入力して下さい。", true, string.Empty))
                {
                    bool isRetry = true;
                    do
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK)
                        {
                            return;
                        }

                        if (string.IsNullOrEmpty(dlg.InputString))
                        {
                            continue;
                        }

                        // カテゴリー名重複チェック
                        if (isRetry = entities.MstCategories.Any(c => c.CategoryName == dlg.InputString))
                        {
                            FormUtilities.ShowMessage("E009", dlg.InputString);
                        }
                    } while (isRetry);

                    // カテゴリー登録
                    DateTime now = DateTime.Now;
                    var category = new MstCategory();
                    category.CategoryName = dlg.InputString;
                    category.InsertedDateTime = now;
                    category.UpdatedDateTime = now;

                    entities.MstCategories.Add(category);
                    entities.SaveChanges();

                    // カテゴリーコンボボックスリフレッシュ
                    RefreshCategoryComboBox(entities);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region lblDropArea_DragEnter - ドロップエリアエンター時

        /// <summary>
        /// ドロップエリアにドラッグエンターした際の処理です。
        /// ドラッグされたデータが処理可能形式の画像URLかを判定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void lblDropArea_DragEnter(object sender, DragEventArgs e)
        {
            // URL形式でない場合はNG
            if (e.Data.GetDataPresent("UniformResourceLocator")
                || e.Data.GetDataPresent("UniformResourceLocatorW"))
            {
                string uri = e.Data.GetData(DataFormats.Text).ToSafeString();
                if (Utilities.IsAvailableFormat(uri, false))
                {
                    // 処理可能ファイル形式の場合のみドロップ許可
                    e.Effect = DragDropEffects.Link;
                }
            }
        }

        #endregion

        #region lblDropArea_DragDrop - ドロップエリアドロップ時

        /// <summary>
        /// ドロップエリアに画像URLをドロップした際の処理です。
        /// ドロップされたURLの画像をダウンロードし、TblImagesに登録します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void lblDropArea_DragDrop(object sender, DragEventArgs e)
        {
            // 画像データ取得
            string uri = e.Data.GetData(DataFormats.Text).ToSafeString();
            byte[] imageData = DownloadManager.DownloadData(uri);

            using (var entities = new PictManagerEntities())
            {
                DateTime now = DateTime.Now;
                var entity = new TblImage();

                entity.ImageData = imageData;
                entity.ImageFormat = Path.GetExtension(uri).Substring(1);
                entity.CategoryId = (cmbCategory.SelectedItem as MstCategory).CategoryId;
                entity.InsertedDateTime = now;
                entity.UpdatedDateTime = now;

                entities.TblImages.Add(entity);
                entities.SaveChanges();
            }
        }

        #endregion
    }
}
