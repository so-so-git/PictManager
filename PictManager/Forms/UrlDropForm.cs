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
            
            using (var entity = new PictManagerEntities())
            {
                // カテゴリコンボボックス内容設定
                RefreshCategoryComboBox(entity);
            }
        }

        #endregion

        #region RefreshCategoryComboBox - カテゴリコンボボックス最新化

        /// <summary>
        /// カテゴリコンボボックスを最新化します。
        /// </summary>
        private void RefreshCategoryComboBox(PictManagerEntities entity)
        {
            cmbCategory.DataSource = entity.MstCategories.ToList();
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

        #region btnAddCategory_Click - カテゴリ追加ボタン押下時

        /// <summary>
        /// カテゴリー追加ボタンをクリックした際に実行される処理です。
        /// カテゴリー名を入力し、新規登録します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                using (var entity = new PictManagerEntities())
                using (var dlg = new CommonInputDialog(
                    "新規カテゴリー追加", "追加するカテゴリー名を入力して下さい。", true, string.Empty))
                {
                    bool isRetry = true;
                    do
                    {
                        if (dlg.ShowDialog(this) != DialogResult.OK) return;

                        if (string.IsNullOrEmpty(dlg.InputString)) continue;

                        // カテゴリー名重複チェック
                        if (isRetry = entity.MstCategories.Any(c => c.CategoryName == dlg.InputString))
                        {
                            FormUtilities.ShowMessage("E009", dlg.InputString);
                        }
                    } while (isRetry);

                    // カテゴリ登録
                    DateTime now = DateTime.Now;
                    var dto = new MstCategory();
                    dto.CategoryName = dlg.InputString;
                    dto.InsertedDateTime = now;
                    dto.UpdatedDateTime = now;

                    entity.MstCategories.Add(dto);
                    entity.SaveChanges();

                    // カテゴリコンボボックスリフレッシュ
                    RefreshCategoryComboBox(entity);
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
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void lblDropArea_DragEnter(object sender, DragEventArgs e)
        {
            // URL形式でない場合はNG
            if (e.Data.GetDataPresent("UniformResourceLocator")
                || e.Data.GetDataPresent("UniformResourceLocatorW"))
            {
                string uri = e.Data.GetData(DataFormats.Text).ToString();
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
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        private void lblDropArea_DragDrop(object sender, DragEventArgs e)
        {
            string uri = e.Data.GetData(DataFormats.Text).ToString();

            using (var entity = new PictManagerEntities())
            {
                DateTime now = DateTime.Now;
                var dto = new TblImage();
                dto.ImageData = DownloadManager.DownloadData(uri);
                dto.CategoryId = (cmbCategory.SelectedItem as MstCategory).CategoryId;
                dto.InsertedDateTime = now;
                dto.UpdatedDateTime = now;

                entity.TblImages.Add(dto);
                entity.SaveChanges();
            }
        }

        #endregion
    }
}
