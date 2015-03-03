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

            if (Utilities.Config.CommonInfo.TargetExtensions.Any())
            {
                // カテゴリ読込
                using (var entity = new PictManagerEntities())
                {
                    cmbCategories.DataSource = entity.MstCategories.OrderBy(c => c.CategoryName);
                    cmbCategories.DisplayMember = "CategoryName";
                }

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

        #region ImportFileToDatabase - ファイルをデータベースにインポート

        /// <summary>
        /// 指定されたパスに存在するファイルをデータベースのTblImagesにインポートします。
        /// パスの存在チェックやファイル形式チェックは行われません。
        /// </summary>
        /// <param name="filePath">インポートするファイルのパス</param>
        private void ImportFileToDatabase(string filePath)
        {
            using (var entity = new PictManagerEntities())
            {
                var dto = new TblImages();

                // 画像データ取得
                using (var img = Image.FromFile(filePath))
                {
                    dto.ImageData = new ImageConverter().ConvertTo(img, typeof(byte[])) as byte[];
                }

                // その他のカラム
                dto.CategoryId = (cmbCategories.SelectedValue as MstCategories).CategoryId;
                DateTime now = DateTime.Now;
                dto.InsertedDateTime = now;
                dto.UpdatedDateTime = now;

                entity.AddToTblImages(dto);
                entity.SaveChanges();
            }
        }

        #endregion

        #region rdoImportKinds_CheckedChanged - インポート対象ラジオボタン変更時

        /// <summary>
        /// インポート対象ラジオボタンの状態が変更された際に実行される処理です。
        /// 選択されたインポート対象の種類に応じてフォームの状態を切り替えます。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
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
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
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
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
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

                    FormUtilities.ShowMessage("I011");
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
    }
}
