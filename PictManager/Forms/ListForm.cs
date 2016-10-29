using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.Forms.Extensions;
using SO.Library.IO;
using SO.PictManager.Common;
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 画像一覧表示フォームクラス
    /// </summary>
    public partial class ListForm : BaseForm
    {
        #region class FileColumnIndexes - ファイルモード用グリッド列インデックス定義クラス

        /// <summary>
        /// ファイルモード用グリッド列インデックス定義クラス
        /// </summary>
        private static class FileColumnIndice
        {
            /// <summary>選択チェックの列インデックス</summary>
            public const int SELECT_CHECK = 0;

            /// <summary>ファイル名の列インデックス</summary>
            public const int FILE_NAME = 1;

            /// <summary>フォルダパスの列インデックス</summary>
            public const int FOLDER_PATH = 2;

            /// <summary>フォルダ参照ボタンの列インデックス</summary>
            public const int REFERENCE_BUTTON = 3;

            /// <summary>MD5の列インデックス</summary>
            public const int MD5 = 4;

            /// <summary>類似画像検索ボタンの列インデックス</summary>
            public const int SIMILAR_BUTTON = 5;
        }

        #endregion

        #region class DatabaseColumnIndexes - データベースモード用グリッド列インデックス定義クラス

        /// <summary>
        /// データベースモード用グリッド列インデックス定義クラス
        /// </summary>
        private static class DatabaseColumnIndice
        {
            /// <summary>選択チェックの列インデックス</summary>
            public const int SELECT_CHECK = 0;

            /// <summary>画像IDの列インデックス</summary>
            public const int IMAGE_ID = 1;

            /// <summary>カテゴリーの列インデックス</summary>
            public const int CATEGORY = 2;

            /// <summary>説明文の列インデックス</summary>
            public const int DESCRIPTION = 3;

            /// <summary>MD5の列インデックス</summary>
            public const int MD5 = 4;

            /// <summary>類似画像検索ボタンの列インデックス</summary>
            public const int SIMILAR_BUTTON = 5;
        }

        #endregion

        #region class RowInfo - グリッド行情報クラス

        /// <summary>
        /// グリッド行情報クラス
        /// </summary>
        private class RowInfo
        {
            /// <summary>行の削除フラグ</summary>
            public bool IsDeleted { get; set; }

            /// <summary>行の通常時の背景色</summary>
            public Color RowColor { get; set; }
        }

        #endregion

        #region クラス定数

        #endregion

        #region インスタンス変数

        /// <summary>選択カウント</summary>
        private int _selectCount = 0;

        /// <summary>編集カウント</summary>
        private int _changeCount = 0;

        /// <summary>行の画像と類似画像のマッピング</summary>
        private Dictionary<int, List<IImage>> _similarMap;

        /// <summary>表示対象画像リスト取得デリゲート</summary>
        private Func<List<IImage>> _imageListGetFunction;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// ファイルモード用のコンストラクタです。
        /// </summary>
        /// <param name="targetPath">対象ディレクトリパス</param>
        /// <param name="includeSubFlg">処理対象にサブディレクトリ以下を含むかを示すフラグ</param>
        public ListForm(string targetPath, bool includeSubFlg)
            : base(targetPath, includeSubFlg)
        {
            try
            {
                // コンポーネント初期化
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
            }
        }

        /// <summary>
        /// データベースモード用のカコンストラクタです。
        /// </summary>
        /// <param name="category">対象カテゴリー</param>
        /// <param name="tag">タグ</param>
        public ListForm(MstCategory category, MstTag tag)
            : base(category, tag)
        {
            try
            {
                // コンポーネント初期化
                InitializeComponent();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
            }
        }

        /// <summary>
        /// 表示対象カスタマイズ用のコンストラクタです。
        /// </summary>
        /// <param name="function">表示対象画像リスト取得デリゲート</param>
        public ListForm(Func<List<IImage>> function)
            : base(null as MstCategory, null as MstTag)
        {
            try
            {
                // コンポーネント初期化
                InitializeComponent();

                // 表示対象画像リスト取得デリゲートを保管
                _imageListGetFunction = function;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
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
            if (ImageMode == ConfigInfo.ImageDataMode.File)
            {
                // ファイル
                menuTemp = new ToolStripMenuItem("ファイル(&F)", null, null, "menuFile");
                menuTemp.ShortcutKeys = Keys.Alt | Keys.F;
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("戻る", null, btnClose_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("対象画像再取得", null, menuRefresh_Click));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("一括ファイル名変更", null, menuRenameAll_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("一括ファイル移動", null, menuMoveAll_Click));
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("削除済画像確認", null,
                    (s, e) => Utilities.ViewDeletedFiles()));
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
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("対象画像再取得", null, menuRefresh_Click));
                menuTemp.DropDownItems.Add(new ToolStripSeparator());
                menuTemp.DropDownItems.Add(new ToolStripMenuItem("終了", null,
                    (s, e) => Form_FormClosing(s, new FormClosingEventArgs(CloseReason.UserClosing, false))));
                barMenu.Items.Add(menuTemp);
            }

            // 操作
            menuTemp = new ToolStripMenuItem("操作(&O)", null, null, "menuOpe");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.O;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("全ての変更を適用", null, btnApplyChanges_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("全ての変更を戻す", null, btnRevertSelection_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("重複している画像のみを抽出", null, menuFilterDuplicated_Click));
            menuTemp.DropDownItems.Add(new ToolStripSeparator());
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("全ての行の類似画像を検索", null, menuSearchSimilarAll_Click));
            barMenu.Items.Add(menuTemp);

            // 表示
            menuTemp = new ToolStripMenuItem("表示(&V)", null, null, "menuView");
            menuTemp.ShortcutKeys = Keys.Alt | Keys.V;
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択行の画像を表示", null, menuViewImage_Click));
            menuTemp.DropDownItems.Add(new ToolStripMenuItem("選択行の類似画像を表示", null, menuSimilar_Click));
            barMenu.Items.Add(menuTemp);
        }

        #endregion

        #region RefreshImageList - 表示対象画像リスト最新化

        /// <summary>
        /// (BaseForm.RefreshImageList()をオーバーライドします)
        /// 表示対象画像リストを最新の内容に更新します。
        /// </summary>
        protected override void RefreshImageList()
        {
            if (_imageListGetFunction != null)
            {
                // 表示対象画像リスト取得デリゲートが指定されている場合はそれを利用し対象を取得
                ImageList = _imageListGetFunction();
            }
            else
            {
                // 表示対象画像リスト取得デリゲートが指定されていない場合は既定の処理
                base.RefreshImageList();
            }
        }

        #endregion

        #region CreateColumns - グリッド列生成

        /// <summary>
        /// 画像一覧グリッドの列を生成します。
        /// </summary>
        protected virtual void CreateColumns()
        {
            // 選択チェックボックス列
            var colChk = new DataGridViewTextBoxColumn();
            colChk.HeaderText = "選択";
            colChk.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colChk.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdImages.Columns.Add(colChk);

            if (ImageMode == ConfigInfo.ImageDataMode.File)
            {
                // ファイル名列
                var colFile = new DataGridViewTextBoxColumn();
                colFile.HeaderText = "ファイル名";
                colFile.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grdImages.Columns.Add(colFile);

                // 親フォルダパス列
                var colDir = new DataGridViewTextBoxColumn();
                colDir.HeaderText = "フォルダ";
                colDir.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colDir.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grdImages.Columns.Add(colDir);

                // ディレクトリ参照ボタン列
                var colRef = new DataGridViewButtonColumn();
                colRef.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                colRef.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                grdImages.Columns.Add(colRef);
            }
            else
            {
                // 画像ID列
                var colImageId = new DataGridViewTextBoxColumn();
                colImageId.HeaderText = "画像ID";
                colImageId.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                colImageId.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                grdImages.Columns.Add(colImageId);

                // カテゴリー列
                var colCategory = new DataGridViewComboBoxColumn();
                colCategory.HeaderText = "カテゴリー";
                colCategory.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                colCategory.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grdImages.Columns.Add(colCategory);

                // 説明文列
                var colDescription = new DataGridViewTextBoxColumn();
                colDescription.HeaderText = "説明";
                colDescription.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                colDescription.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                grdImages.Columns.Add(colDescription);
            }

            // MD5列
            var colMD5 = new DataGridViewTextBoxColumn();
            colMD5.HeaderText = "MD5";
            colMD5.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colMD5.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            grdImages.Columns.Add(colMD5);

            // 類似画像検索ボタン列
            var colSimilar = new DataGridViewButtonColumn();
            colSimilar.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colSimilar.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            grdImages.Columns.Add(colSimilar);
        }

        #endregion

        #region CreateCells - グリッドセル生成

        /// <summary>
        /// 画像一覧グリッドのセルを生成します。
        /// </summary>
        protected virtual void CreateCells()
        {
            grdImages.SuspendLayout();

            List<MstCategory> categoryList;
            if (ImageMode == ConfigInfo.ImageDataMode.Database)
            {
                using (var entities = new PictManagerEntities())
                {
                    categoryList = (from c in entities.MstCategories
                                    orderby c.CategoryName
                                    select c).ToList();
                }
            }
            else
            {
                categoryList = null;
            }

            string lastMd5 = string.Empty;
            bool isOdd = true;
            int i = 0;
            var rowList = new List<DataGridViewRow>(ImageList.Count);
            foreach (var img in ImageList)
            {
                var row = new DataGridViewRow();

                var rowInfo = new RowInfo();
                row.Tag = rowInfo;

                row.HeaderCell.Value = (++i).ToString();
                row.HeaderCell.Style.Font = new Font(this.Font, FontStyle.Regular);

                // 選択チェックボックスセル
                DataGridViewCell celChk;
                if (img.IsDeleted)
                {
                    celChk = new DataGridViewTextBoxCell();
                    celChk.Value = string.Empty;
                }
                else
                {
                    celChk = new DataGridViewCheckBoxCell();
                    celChk.Value = false;
                }
                celChk.Tag = celChk.Value;
                row.Cells.Add(celChk);

                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    // ファイル名セル
                    var celFile = new DataGridViewTextBoxCell();
                    celFile.Value = Path.GetFileName(img.Key);
                    celFile.Tag = celFile.Value;
                    row.Cells.Add(celFile);

                    // 親フォルダパスセル
                    var celDir = new DataGridViewTextBoxCell();
                    celDir.Value = Path.GetDirectoryName(img.Key);
                    celDir.Tag = celDir.Value;
                    row.Cells.Add(celDir);

                    // フォルダ参照ボタンセル
                    DataGridViewCell celRef;
                    if (img.IsDeleted)
                    {
                        celRef = new DataGridViewTextBoxCell();
                        celRef.Value = string.Empty;
                    }
                    else
                    {
                        celRef = new DataGridViewButtonCell();
                        celRef.Value = "...";
                    }
                    row.Cells.Add(celRef);
                    celRef.ReadOnly = img.IsDeleted;
                }
                else
                {
                    var dataImg = img as DataImage;

                    // 画像IDセル
                    var celFile = new DataGridViewTextBoxCell();
                    celFile.Value = img.Key;
                    celFile.Tag = celFile.Value;
                    row.Cells.Add(celFile);
                    celFile.ReadOnly = true;

                    // カテゴリーセル
                    var celCategory = new DataGridViewComboBoxCell();
                    celCategory.DataSource = categoryList;
                    celCategory.DisplayMember = "CategoryName";
                    celCategory.ValueMember = "CategoryId";
                    celCategory.Value = dataImg.CategoryId;
                    celCategory.Tag = celCategory.Value;
                    celCategory.DisplayStyle = DataGridViewComboBoxDisplayStyle.Nothing;
                    row.Cells.Add(celCategory);

                    // 説明文セル
                    var celDescription = new DataGridViewTextBoxCell();
                    celDescription.Value = dataImg.Description ?? string.Empty; 
                    celDescription.Tag = celDescription.Value;
                    row.Cells.Add(celDescription);
                }

                DataGridViewCell celSimilar;
                DataGridViewCell celMD5 = new DataGridViewTextBoxCell();
                if (img.IsDeleted)
                {
                    // MD5セル
                    celMD5.Value = string.Empty;

                    // 類似画像検索ボタンセル
                    celSimilar = new DataGridViewTextBoxCell();
                    celSimilar.Value = string.Empty;
                }
                else
                {
                    // MD5セル
                    celMD5.Value = ImageMode == ConfigInfo.ImageDataMode.File
                        ? Cryptgrapher.GetFileMD5(img.Key)
                        : Cryptgrapher.GetBytesMD5((img as DataImage).ImageBytes);

                    // 類似画像検索ボタンセル
                    celSimilar = new DataGridViewButtonCell();
                    celSimilar.Value = "類似";
                }
                row.Cells.Add(celMD5);
                row.Cells.Add(celSimilar);

                // MD5が前の行から変わったかに応じて行の背景色を設定
                if (celMD5.Value.ToString() != lastMd5)
                {
                    isOdd = !isOdd;
                    lastMd5 = celMD5.Value.ToString();
                }
                rowInfo.RowColor = isOdd ? Color.LightCyan : Color.White;

                celMD5.ReadOnly = true;
                celSimilar.ReadOnly = img.IsDeleted;

                SetRowDeleted(row, img.IsDeleted);
                row.ReadOnly = img.IsDeleted;

                rowList.Add(row);
            }

            grdImages.Rows.AddRange(rowList.ToArray());
            grdImages.ResumeLayout();
        }

        #endregion

        #region GetImagePath - 画像ファイルパス取得

        /// <summary>
        /// 指定された行の画像ファイルのパスを取得します。
        /// (ファイルモード時のみ利用可)
        /// </summary>
        /// <param name="rowIdx">指定行</param>
        /// <returns>画像ファイルのパス</returns>
        private string GetImagePath(int rowIdx)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            string dirName = grdImages[FileColumnIndice.FOLDER_PATH, rowIdx].Tag.ToSafeString();
            string fileName = grdImages[FileColumnIndice.FILE_NAME, rowIdx].Tag.ToSafeString();

            return Path.Combine(dirName, fileName);
        }

        #endregion

        #region GetImageId - 画像ID取得

        /// <summary>
        /// 指定された行の画像IDを取得します。
        /// (データベースモード時のみ利用可)
        /// </summary>
        /// <param name="rowIdx">指定行</param>
        /// <returns>画像ID</returns>
        private int GetImageId(int rowIdx)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.Database);

            object imageId = grdImages[DatabaseColumnIndice.IMAGE_ID, rowIdx].Value;

            return Convert.ToInt32(imageId);
        }

        #endregion

        #region IsValidValueInFileMode - 入力内容検証(ファイルモード用)

        /// <summary>
        /// セルに入力された内容の妥当性を検証します。(ファイルモード用)
        /// </summary>
        /// <param name="cell">検証対象のDetaGridViewCell</param>
        /// <returns>検証結果OK:true / 検証結果NG:false</returns>
        protected virtual bool IsValidValueInFileMode(DataGridViewCell cell)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            if (cell.ColumnIndex != FileColumnIndice.FILE_NAME
                && cell.ColumnIndex != FileColumnIndice.FOLDER_PATH)
            {
                return true;
            }

            char[] allowChars;
            if (cell.ColumnIndex == FileColumnIndice.FILE_NAME)
            {
                allowChars = new char[] { };
            }
            else
            {
                // フォルダパスの場合はパス区切り文字とボリューム区切り文字は許容
                allowChars = new char[]
                    {
                        Path.DirectorySeparatorChar,
                        Path.VolumeSeparatorChar
                    };
            }

            Predicate<string> ErrHandling = (msg) =>
            {
                // 編集モードを継続、エラー通知表示
                foreach (DataGridViewCell selCell in grdImages.SelectedCells)
                    selCell.Selected = false;

                grdImages.CurrentCell = cell;
                grdImages.BeginEdit(true);
                FormUtilities.ShowMessage(msg);
                return false;
            };

            // 禁則文字チェック
            string editVal = cell.EditedFormattedValue.ToString();
            if (editVal.HasInvalidPathChar(allowChars))
            {
                return ErrHandling("W017");
            }

            if (cell.ColumnIndex == FileColumnIndice.FILE_NAME)
            {
                // 拡張子変更チェック
                if (Path.GetExtension(editVal) != Path.GetExtension(cell.Value.ToString()))
                {
                    return ErrHandling("W018");
                }
            }
            else
            {
                // ボリューム区切り文字複数入力チェック
                // (ネットワークパスはサポート外)
                int volCharCnt = (from c in editVal.ToCharArray()
                                  where c == Path.VolumeSeparatorChar
                                  select c).Count();

                if (volCharCnt == 0)
                {
                    return ErrHandling("W019");
                }
                if (volCharCnt > 1)
                {
                    return ErrHandling("W020");
                }

                // ボリューム区切り文字位置チェック
                if (editVal.IndexOf(Path.VolumeSeparatorChar) != 1)
                {
                    return ErrHandling("W021");
                }

                // 空のディレクトリパスチェック
                var twinSep = new string(Path.DirectorySeparatorChar, 2);
                if (editVal.IndexOf(twinSep) != -1)
                {
                    return ErrHandling("W022");
                }
            }

            return true;
        }

        #endregion

        #region IsValidValueInDatabaseMode - 入力内容検証(データベースモード用)

        /// <summary>
        /// セルに入力された内容の妥当性を検証します。(データベースモード用)
        /// </summary>
        /// <param name="cell">検証対象のDetaGridViewCell</param>
        /// <returns>検証結果OK:true / 検証結果NG:false</returns>
        protected virtual bool IsValidValueInDatabaseMode(DataGridViewCell cell)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.Database);

            // 現状、常にOK
            return true;
        }

        #endregion

        #region ApplyChangeInFileMode - 入力された変更を適用(ファイルモード用)

        /// <summary>
        /// 画像一覧グリッドセルで入力された変更内容を各ファイルに適用します。
        /// (ファイルモードの場合のみ使用可)
        /// </summary>
        /// <param name="cell">変更内容が入力されたDataGridViewCell</param>
        /// <param name="proceededRows">処理済の行インデックスが格納されたList</param>
        /// <returns>処理中止フラグ</returns>
        protected virtual bool ApplyChangesInFileMode(DataGridViewCell cell, List<int> proceededRows)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            bool ret = true;
            int listRow = int.Parse(grdImages.Rows[cell.RowIndex].HeaderCell.Value.ToString()) - 1;
            switch (cell.ColumnIndex)
            {
                case FileColumnIndice.FILE_NAME:
                case FileColumnIndice.FOLDER_PATH:
                    // 既に対象が処理済の場合はスキップ
                    if (proceededRows.Contains(listRow))
                    {
                        break;
                    }

                    // 対象ファイルのパスを変更
                    ret = ChangePath(listRow);
                    proceededRows.Add(listRow);
                    break;

                default:
                    break;
            }

            return ret;
        }

        #endregion

        #region ApplyChangeInDatabaseMode - 入力された変更を適用(データベースモード用)

        /// <summary>
        /// 画像一覧グリッドセルで入力された変更内容を各ファイルに適用します。
        /// (データベースモードの場合のみ使用可)
        /// </summary>
        /// <param name="row">変更内容が入力されたDataGridViewRow</param>
        /// <param name="entity">データベースエンティティ</param>
        /// <returns>処理中止フラグ</returns>
        protected virtual bool ApplyChangesInDatabaseMode(DataGridViewRow row, PictManagerEntities entity)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.Database);

            int imageId = Convert.ToInt32(row.Cells[DatabaseColumnIndice.IMAGE_ID].Value);

            // データベースに変更を反映
            var img = (from i in entity.TblImages
                       where i.ImageId == imageId
                       select i).FirstOrDefault();

            if (img == null)
            {
                return true;
            }

            img.CategoryId = Convert.ToInt32(row.Cells[DatabaseColumnIndice.CATEGORY].Value);
            img.Description = row.Cells[DatabaseColumnIndice.DESCRIPTION].Value.ToSafeString();
            img.UpdatedDateTime = DateTime.Now;

            // 保持しているリスト内の要素にも同様の内容を反映
            int index = ImageList.FindIndex(i => int.Parse(i.Key) == imageId);
            var imgInList = ImageList[index] as DataImage;

            imgInList.CategoryId = img.CategoryId;
            imgInList.Description = img.Description;

            return true;
        }

        #endregion

        #region ChangePath - 入力内容をファイルパスに反映

        /// <summary>
        /// 画像一覧グリッドのファイル名列、ディレクトリパス列で入力された変更をファイルに対して反映します。
        /// </summary>
        /// <param name="rowIndex">処理対象の行インデックス</param>
        /// <returns>処理中止フラグ</returns>
        protected virtual bool ChangePath(int rowIndex)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            // ファイル移動
            string dirPath = grdImages[FileColumnIndice.FOLDER_PATH, rowIndex].Value.ToString();
            if (!Directory.Exists(dirPath))
            {
                // フォルダ作成確認
                if (FormUtilities.ShowMessage("Q009", "移動先", dirPath) == DialogResult.No)
                {
                    FormUtilities.ShowMessage("I007");
                    return false;
                }
                Directory.CreateDirectory(dirPath);
            }

            string fileName = grdImages[FileColumnIndice.FILE_NAME, rowIndex].Value.ToString();
            string newPath = Path.Combine(dirPath, fileName);

            if (File.Exists(newPath))
            {
                // ファイル上書き確認
                if (FormUtilities.ShowMessage("Q015", fileName) == DialogResult.No)
                {
                    FormUtilities.ShowMessage("I007");
                    return false;
                }

                File.Delete(newPath);
            }

            File.Move(ImageList[rowIndex].Key, newPath);
            ImageList[rowIndex].Key = newPath;

            return true;
        }

        #endregion

        #region RevertEdit - グリッドを変更前の状態に戻す

        /// <summary>
        /// グリッドの内容を直近の確定状態に戻します。
        /// </summary>
        /// <param name="isAllRevert">true:全ての対象を戻す / false:選択された対象のみ戻す</param>
        private void RevertEdit(bool isAllRevert)
        {
            foreach (DataGridViewColumn col in grdImages.Columns)
            {
                col.HeaderCell.SortGlyphDirection = SortOrder.None;
            }

            if (isAllRevert)
            {
                // セルを再作成
                grdImages.Rows.Clear();
                CreateCells();

                _changeCount = 0;
            }
            else
            {
                int selChkColIdx;
                int revertEndColIdx;
                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    selChkColIdx = FileColumnIndice.SELECT_CHECK;
                    revertEndColIdx = FileColumnIndice.FOLDER_PATH;
                }
                else
                {
                    selChkColIdx = DatabaseColumnIndice.SELECT_CHECK;
                    revertEndColIdx = DatabaseColumnIndice.DESCRIPTION;
                }

                // 選択されている行を取得
                var selectedRows = from r in grdImages.Rows.Cast<DataGridViewRow>()
                                   where Convert.ToBoolean(r.Cells[selChkColIdx].Value)
                                   select r;

                for (int rowIndex = 0; rowIndex < grdImages.Rows.Count; rowIndex++)
                {
                    DataGridViewRow row = grdImages.Rows[rowIndex];

                    for (int colIndex = selChkColIdx; colIndex <= revertEndColIdx; colIndex++)
                    {
                        DataGridViewCell cell = row.Cells[colIndex];
                        if (!cell.Value.Equals(cell.Tag))    // セル内容が表示時と異なる場合
                        {
                            // 変更前の内容を復元
                            cell.Value = cell.Tag;

                            // 変更カウントをデクリメント、変更判別色クリア
                            if (cell.Style.BackColor == Color.Yellow)
                            {
                                _changeCount--;
                            }

                            cell.Style.BackColor =
                                grdImages.Columns[colIndex].DefaultCellStyle.BackColor;
                        }
                    }

                    // 行ヘッダの変更スタイルをクリア
                    DataGridViewRowHeaderCell rhCell = grdImages.Rows[row.Index].HeaderCell;
                    rhCell.Style.Font = new Font(rhCell.Style.Font, FontStyle.Regular);
                }
            }

            bool isButtonsEnabled = _changeCount > 0;
            btnApplyChanges.Enabled = isButtonsEnabled;
            btnDeleteSelection.Enabled = isButtonsEnabled;
            btnRevertSelection.Enabled = isButtonsEnabled;
            btnRevertAll.Enabled = isButtonsEnabled;
        }

        #endregion

        #region ShowHistoryByStatusBar - 変更前後の内容をステータスバーに表示

        /// <summary>
        /// 指定セルの変更前後の内容をステータスバーに表示します。
        /// </summary>
        /// <param name="cell">変更内容を表示するセル</param>
        private void ShowHistoryByStatusBar(DataGridViewCell cell)
        {
            string statusText;
            if (ImageMode == ConfigInfo.ImageDataMode.Database
                && cell.ColumnIndex == DatabaseColumnIndice.CATEGORY)
            {
                using (var entities = new PictManagerEntities())
                {
                    int oldId = Convert.ToInt32(cell.Tag);
                    statusText = (from c in entities.MstCategories
                                  where c.CategoryId == oldId
                                  select c).First().CategoryName;

                    if (cell.Style.BackColor == Color.Yellow)
                    {
                        int newId = Convert.ToInt32(cell.Value);

                        statusText += " -> "
                            + (from c in entities.MstCategories
                               where c.CategoryId == newId
                               select c).First().CategoryName;
                    }
                }
            }
            else
            {
                statusText = cell.Tag.ToString();

                if (cell.Style.BackColor == Color.Yellow)
                {
                    statusText += " -> " + cell.Value.ToString();
                }
            }

            lblStatus.Text = statusText;
        }

        #endregion

        #region SelectDirectoryForChange - 変更後ディレクトリ選択

        /// <summary>
        /// 指定行の画像の変更後のディレクトリを選択するダイアログを表示します。
        /// </summary>
        /// <param name="rowIdx">指定行</param>
        private void SelectDirectoryForChange(int rowIdx)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            if (!IsRowDeleted(grdImages.Rows[rowIdx]))
            {
                using (var dlg = new FolderBrowserDialog())
                {
                    DataGridViewCell cell = grdImages[FileColumnIndice.FOLDER_PATH, rowIdx];

                    dlg.Description = "変更後のディレクトリを選択して下さい。";
                    dlg.RootFolder = Environment.SpecialFolder.Desktop;
                    dlg.ShowNewFolderButton = true;
                    dlg.SelectedPath = cell.Value.ToString();

                    if (dlg.ShowDialog(this) == DialogResult.OK)
                    {
                        // OKを押下された場合はグリッドに選択ディレクトリを反映
                        cell.Value = dlg.SelectedPath;
                    }
                }
            }
        }

        #endregion

        #region ShowSimilarImages - 類似画像表示

        /// <summary>
        /// 指定行の画像の類似画像を検索し、サムネイルフォームで表示します。
        /// </summary>
        /// <param name="rowIdx">指定行</param>
        private void ShowSimilarImages(int rowIdx)
        {
            int colIdx = ImageMode == ConfigInfo.ImageDataMode.File
                ? FileColumnIndice.SIMILAR_BUTTON
                : DatabaseColumnIndice.SIMILAR_BUTTON;

            if (!(grdImages[colIdx, rowIdx] is DataGridViewButtonCell))
            {
                return;
            }

            string key = ImageMode == ConfigInfo.ImageDataMode.File
                ? GetImagePath(rowIdx) : GetImageId(rowIdx).ToSafeString();

            List<IImage> similarImages;
            if (_similarMap != null && _similarMap.ContainsKey(rowIdx))
            {
                similarImages = _similarMap[rowIdx];
            }
            else
            {
                IImage criterion;
                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    criterion = new FileImage(key);
                }
                else
                {
                    criterion = new DataImage(int.Parse(key));
                }

                similarImages = ImageController.GetSimilarImages(this, criterion);
            }

            if (!similarImages.Any())
            {
                FormUtilities.ShowMessage("I008");
            }
            else
            {
                var form = new ThumbnailForm(similarImages, ImageMode);
                form.Text = string.Format("PictManager - 類似画像検索結果 [{0}]", key);
                form.StatusBarText = string.Format("[{0}] の類似画像を表示中 - {1}件", key, similarImages.Count);

                form.Show(this);
                form.Activate();
            }
        }

        #endregion

        #region IsRowDeleted - グリッド行の削除フラグを取得

        /// <summary>
        /// グリッド行の画像が削除されているかのフラグを取得します。
        /// </summary>
        /// <param name="row">グリッド行</param>
        /// <returns>グリッド行の画像が削除されているかのフラグ</returns>
        private bool IsRowDeleted(DataGridViewRow row)
        {
            return (row.Tag as RowInfo).IsDeleted;
        }

        #endregion

        #region SetRowDeleted - グリッド行の削除フラグを設定、行変更破棄、書式設定

        /// <summary>
        /// グリッド行の画像が削除されているかのフラグを設定します。
        /// 削除ONの場合は、同時に行の変更内容を全て破棄し、行の書式を削除状態のものに設定します。
        /// </summary>
        /// <param name="row">グリッド行</param>
        /// <param name="deleteFlag">グリッド行の画像が削除されているかのフラグ</param>
        private void SetRowDeleted(DataGridViewRow row, bool deleteFlag)
        {
            grdImages.CancelEdit();

            var rowInfo = row.Tag as RowInfo;
            rowInfo.IsDeleted = deleteFlag;

            if (deleteFlag)
            {
                int selChkColIdx;
                int revertEndColIdx;
                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    selChkColIdx = FileColumnIndice.SELECT_CHECK;
                    revertEndColIdx = FileColumnIndice.FOLDER_PATH;
                }
                else
                {
                    selChkColIdx = DatabaseColumnIndice.SELECT_CHECK;
                    revertEndColIdx = DatabaseColumnIndice.DESCRIPTION;
                }

                for (int colIndex = selChkColIdx; colIndex <= revertEndColIdx; colIndex++)
                {
                    DataGridViewCell cell = row.Cells[colIndex];
                    if (!cell.Value.Equals(cell.Tag))    // セル内容が表示時と異なる場合
                    {
                        // 変更前の内容を復元
                        cell.Value = cell.Tag;

                        // 変更カウントをデクリメント
                        if (cell.Style.BackColor == Color.Yellow)
                        {
                            _changeCount--;
                        }
                    }

                    cell.ReadOnly = true;
                }

                var selChkNewCell = new DataGridViewTextBoxCell();
                row.Cells[selChkColIdx] = selChkNewCell;

                selChkNewCell.Value = string.Empty;
                selChkNewCell.Tag = selChkNewCell.Value;
                selChkNewCell.Style.BackColor = Color.LightGray;
                selChkNewCell.ReadOnly = true;

                row.DefaultCellStyle.BackColor = Color.LightGray;
                row.HeaderCell.Style.Font = new Font(row.HeaderCell.Style.Font, FontStyle.Regular);
            }
            else
            {
                row.DefaultCellStyle.BackColor = rowInfo.RowColor;
            }
        }

        #endregion

        #region FilterDuplicatedImages - MD5が重複している画像のみを抽出して表示

        /// <summary>
        /// MD5が重複している画像のみを抽出し、グリッドに表示します。
        /// </summary>
        private void FilterDuplicatedImages()
        {
            int md5ColIndex;
            int keyColIndex;
            if (ImageMode == ConfigInfo.ImageDataMode.File)
            {
                md5ColIndex = FileColumnIndice.MD5;
                keyColIndex = FileColumnIndice.FILE_NAME;
            }
            else
            {
                md5ColIndex = DatabaseColumnIndice.MD5;
                keyColIndex = DatabaseColumnIndice.IMAGE_ID;
            }

            // MD5が同じ画像をグルーピング
            var duplecatedGroups = from row in grdImages.Rows.Cast<DataGridViewRow>()
                                   where !IsRowDeleted(row)
                                   group row by row.Cells[md5ColIndex].Value
                                   into grp
                                   where grp.Count() > 1
                                   orderby grp.Key
                                   select grp;

            var viewRowList = new List<DataGridViewRow>();
            bool isEvenGroup = false;

            foreach (var grp in duplecatedGroups)
            {
                // 同MD5毎に背景色を交互に設定
                Color backColor = isEvenGroup ? Color.PaleTurquoise : Color.White;
                foreach (var row in grp)
                {
                    row.DefaultCellStyle.BackColor = backColor;
                }
                isEvenGroup = !isEvenGroup;

                // キーでソートして表示リストに追加
                viewRowList.AddRange(grp.OrderBy(r => r.Cells[keyColIndex].Value));
            }

            // MD5重複画像のみをグリッドに表示
            grdImages.Rows.Clear();
            grdImages.Rows.AddRange(viewRowList.ToArray());
        }

        #endregion

        //*** イベントハンドラ ***

        #region Form_Shown - フォーム表示時

        /// <summary>
        /// フォームが表示された際に実行される処理です。
        /// 画像一覧グリッドの各列幅を初期化します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Form_Shown(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                // 表示対象画像取得
                RefreshImageList();

                // グリッド列生成
                CreateColumns();

                // グリッドセル生成
                CreateCells();

                // ステータスバー更新
                lblStatus.Text = string.Empty;
                lblFileCount.Text = ImageList.Count + " 件";

                // 列幅設定
                grdImages.RowHeadersWidthSizeMode =
                    DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders |
                    DataGridViewRowHeadersWidthSizeMode.DisableResizing;

                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    grdImages.Columns[FileColumnIndice.FILE_NAME].Width =
                        (grdImages.Width - grdImages.RowHeadersWidth -
                         grdImages.Columns[FileColumnIndice.SELECT_CHECK].Width -
                         grdImages.Columns[FileColumnIndice.REFERENCE_BUTTON].Width -
                         grdImages.Columns[FileColumnIndice.SIMILAR_BUTTON].Width) / 3;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                Cursor = Cursors.Default;
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
                // 終了確認
                if (Utilities.Config.CommonInfo.IsConfirmQuit
                    && _changeCount > 0
                    && FormUtilities.ShowMessage("Q000") == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    this.BackToOwner();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                Application.Exit(new CancelEventArgs());
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
                // 修飾キーが付加されている場合は通常処理
                if ((e.KeyCode & Keys.Alt) != Keys.Alt
                    && (e.KeyCode & Keys.Control) != Keys.Control
                    && (e.KeyCode & Keys.Shift) != Keys.Shift)
                {
                    Keys kcode = e.KeyCode & Keys.KeyCode;
                    switch (kcode)
                    {
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

        #region grdImages_CellEnter - セルフォーカス取得時

        /// <summary>
        /// 画像一覧グリッドのセルがフォーカスを取得した際に実行される処理です。
        /// セルがコンボボックスタイプの場合、ドロップダウンを開きます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdImages_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                if (grdImages.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn)
                {
                    SendKeys.Send("{F4}");
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region grdImages_CellDoubleClick - セルダブルクリック時

        /// <summary>
        /// 画像一覧グリッドのセルがダブルクリックされた際に実行される処理です。
        /// その行に関連付けられた画像をViewImageFormで表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdImages_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex <= -1 || IsRowDeleted(grdImages.Rows[e.RowIndex]))
                {
                    return;
                }

                // 表示画像情報生成
                IImage img = null;
                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    if (e.ColumnIndex != FileColumnIndice.SELECT_CHECK
                        && e.ColumnIndex != FileColumnIndice.REFERENCE_BUTTON
                        && e.ColumnIndex != FileColumnIndice.SIMILAR_BUTTON)
                    {
                        img = new FileImage(GetImagePath(e.RowIndex));
                    }
                }
                else
                {
                    if (e.ColumnIndex != DatabaseColumnIndice.SELECT_CHECK
                        && e.ColumnIndex != DatabaseColumnIndice.SIMILAR_BUTTON)
                    {
                        img = new DataImage(GetImageId(e.RowIndex));
                    }
                }

                if (img != null)
                {
                    // 画像閲覧
                    new ViewImageForm(this, img, ImageMode).ShowDialog(this);

                    if (img.IsDeleted)
                    {
                        // 子画面で画像が削除された場合は行を削除状態に設定
                        DataGridViewRow row = grdImages.Rows[e.RowIndex];
                        SetRowDeleted(row, true);
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region grdImages_CellClick - セルクリック時

        /// <summary>
        /// 画像一覧グリッドのセルがクリックされた際に実行される処理です。
        /// ・ファイル名列、ディレクトリパス列、カテゴリー列がクリックされた場合：変更前後の内容をステータスバーに表示します。
        /// ・参照ボタン列がクリックされた場合：移動先ディレクトリを選択する為のダイアログを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdImages_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                lblStatus.Text = string.Empty;

                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    switch (e.ColumnIndex)
                    {
                        case FileColumnIndice.FILE_NAME:
                        case FileColumnIndice.FOLDER_PATH:
                            // 変更前後の内容をステータスバーに表示
                            ShowHistoryByStatusBar(grdImages[e.ColumnIndex, e.RowIndex]);
                            break;

                        case FileColumnIndice.REFERENCE_BUTTON:
                            // 変更後ディレクトリ選択
                            SelectDirectoryForChange(e.RowIndex);
                            break;

                        case FileColumnIndice.SIMILAR_BUTTON:
                            // 類似画像表示
                            ShowSimilarImages(e.RowIndex);
                            break;

                        default:
                            break;
                    }
                }
                else
                {
                    switch (e.ColumnIndex)
                    {
                        case DatabaseColumnIndice.CATEGORY:
                            // 変更前後の内容をステータスバーに表示
                            ShowHistoryByStatusBar(grdImages[e.ColumnIndex, e.RowIndex]);
                            break;

                        case DatabaseColumnIndice.SIMILAR_BUTTON:
                            // 類似画像表示
                            ShowSimilarImages(e.RowIndex);
                            break;

                        default:
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

        #region grdImages_CellValidating - セル入力内容検証時

        /// <summary>
        /// 画像一覧グリッドのセルの入力内容検証時に実行される処理です。
        /// IsValidInputの実装内容に基づき、セルの入力内容を検証します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdImages_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                DataGridViewCell cell = grdImages[e.ColumnIndex, e.RowIndex];
                bool isValid = ImageMode == ConfigInfo.ImageDataMode.File
                    ? IsValidValueInFileMode(cell)
                    : IsValidValueInDatabaseMode(cell);

                if (!isValid)
                {
                    e.Cancel = true;
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region grdImages_CellValueChanged - セル内容変更時

        /// <summary>
        /// 画像一覧グリッドのセルの内容が変更された際に実行される処理です。
        /// 変更された箇所がわかるように、セルの背景色や列ヘッダーのフォントを変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void grdImages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                {
                    return;
                }

                DataGridViewCell cell = grdImages[e.ColumnIndex, e.RowIndex];
                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    if (e.ColumnIndex == FileColumnIndice.REFERENCE_BUTTON
                        || e.ColumnIndex == FileColumnIndice.MD5)
                    {
                        return;
                    }

                    if (e.ColumnIndex == FileColumnIndice.SIMILAR_BUTTON)
                    {
                        // 類似画像検索ボタンの場合
                        cell.Style.BackColor = cell.Value.Equals(string.Empty)
                            ? Color.LightGray : Color.White;
                        return;
                    }
                }
                else
                {
                    if (e.ColumnIndex == DatabaseColumnIndice.IMAGE_ID
                        || e.ColumnIndex == FileColumnIndice.MD5)
                    {
                        return;
                    }

                    if (e.ColumnIndex == DatabaseColumnIndice.SIMILAR_BUTTON)
                    {
                        // 類似画像検索ボタンの場合
                        cell.Style.BackColor = cell.Value.Equals(string.Empty)
                            ? Color.LightGray : Color.White;
                        return;
                    }
                }

                if ((ImageMode == ConfigInfo.ImageDataMode.File && e.ColumnIndex == FileColumnIndice.SELECT_CHECK)
                    || (ImageMode == ConfigInfo.ImageDataMode.Database && e.ColumnIndex == DatabaseColumnIndice.SELECT_CHECK))
                {
                    // 選択チェックボックス変更時
                    if (Convert.ToBoolean(cell.Value))
                    {
                        _selectCount++;
                    }
                    else
                    {
                        _selectCount--;
                    }
                    btnDeleteSelection.Enabled = _selectCount > 0;
                }
                else
                {
                    DataGridViewRowHeaderCell rhCell = grdImages.Rows[e.RowIndex].HeaderCell;
                    if (cell.Value.Equals(cell.Tag))    // セル内容が表示時と同一の場合
                    {
                        // 変更カウントをデクリメント、変更判別色クリア
                        if (cell.Style.BackColor == Color.Yellow)
                        {
                            _changeCount--;
                        }
                        cell.Style.BackColor = grdImages.Columns[e.ColumnIndex].DefaultCellStyle.BackColor;

                        // 行の全ての内容が元に戻された場合は行ヘッダの変更マークをクリア
                        if (!grdImages.Rows[e.RowIndex].Cells.Cast<DataGridViewCell>().Any(
                            c => c.Style.BackColor == Color.Yellow))
                        {
                            if (rhCell.Style.Font.Bold)
                            {
                                rhCell.Style.Font = new Font(rhCell.Style.Font, FontStyle.Regular);
                            }
                        }
                    }
                    else    // セル内容が表示時から変更された場合
                    {
                        // 変更カウントをインクリメント、変更判別色セット
                        cell.Style.BackColor = Color.Yellow;
                        _changeCount++;

                        // 行ヘッダの変更マークが未設定の場合は設定
                        if (!rhCell.Style.Font.Bold)
                        {
                            rhCell.Style.Font = new Font(rhCell.Style.Font, FontStyle.Bold);
                        }
                    }

                    bool isButtonsEnabled = _changeCount > 0;
                    btnApplyChanges.Enabled = isButtonsEnabled;
                    btnRevertSelection.Enabled = isButtonsEnabled;
                    btnRevertAll.Enabled = isButtonsEnabled;
                }

                grdImages.UpdateCellValue(e.ColumnIndex, e.RowIndex);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnClose_Click - 終了ボタン押下時

        /// <summary>
        /// 終了ボタンがクリックされた際に実行される処理です。
        /// 自フォームを破棄し、親フォームを再表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            // 自フォームを破棄し親フォームを表示
            if (_changeCount > 0 && FormUtilities.ShowMessage("Q005") == DialogResult.No)
            {
                return;
            }

            this.BackToOwner();
        }

        #endregion

        #region btnApplyChanges_Click - 変更適用ボタン押下時

        /// <summary>
        /// 変更適用ボタンがクリックされた際に実行される処理です。
        /// 画像一覧グリッドに入力された内容を各ファイルに適用します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormUtilities.ShowMessage("Q008") == DialogResult.No)
                {
                    return;
                }

                Cursor.Current = Cursors.WaitCursor;

                if (ImageMode == ConfigInfo.ImageDataMode.File)
                {
                    // 編集済セルを含む行を抽出→編集済セルを抽出(行順、列順でソート)
                    var editedCells = from r in grdImages.Rows.Cast<DataGridViewRow>()
                                      where r.HeaderCell.Style.Font.Bold
                                      select r.Cells.Cast<DataGridViewCell>() into er
                                      from c in er
                                      where c.Style.BackColor == Color.Yellow
                                      orderby c.RowIndex, c.ColumnIndex
                                      select c;

                    var proceededRows = new List<int>();
                    foreach (var cell in editedCells)
                    {
                        // 変更を適用
                        if (!ApplyChangesInFileMode(cell, proceededRows))
                        {
                            RevertEdit(true);
                            Console.WriteLine("Apply break.");
                            return;
                        }
                    }
                }
                else
                {
                    // 編集済セルを含む行を抽出
                    var editedRows = from r in grdImages.Rows.Cast<DataGridViewRow>()
                                     where r.HeaderCell.Style.Font.Bold
                                     select r;

                    using (var entities = new PictManagerEntities())
                    {
                        // 変更を適用
                        foreach (var row in editedRows)
                        {
                            if (!ApplyChangesInDatabaseMode(row, entities))
                            {
                                RevertEdit(true);
                                Console.WriteLine("Apply break.");
                                return;
                            }
                        }

                        entities.SaveChanges();
                    }
                }

                // 表示を最新状態に更新
                RevertEdit(true);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region btnDeleteSelection_Click - 選択行の画像削除ボタン押下時

        /// <summary>
        /// 選択行の画像削除ボタンがクリックされた際に実行される処理です。
        /// 選択されている行の画像を論理削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnDeleteSelection_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                // 削除確認
                if (FormUtilities.ShowMessage("Q016") == DialogResult.No)
                {
                    return;
                }

                // 選択行抽出
                int selChkColIdx = ImageMode == ConfigInfo.ImageDataMode.File
                    ? FileColumnIndice.SELECT_CHECK : DatabaseColumnIndice.SELECT_CHECK;

                var selectedRows = from r in grdImages.Rows.Cast<DataGridViewRow>()
                                   where r.Cells[selChkColIdx] is DataGridViewCheckBoxCell
                                      && Convert.ToBoolean(r.Cells[selChkColIdx].Value)
                                   select r;

                // 選択行の画像を削除
                foreach (var row in selectedRows)
                {
                    int listIndex = int.Parse(row.HeaderCell.Value.ToString()) - 1;
                    ImageList[listIndex].Delete();
                }

                // 表示内容更新
                RevertEdit(true);

                FormUtilities.ShowMessage("I011", "削除");
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                this.BackToOwner();
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region btnRevertSelection_Click - 選択行の変更を取消ボタン押下時

        /// <summary>
        /// 選択行の変更を取消ボタンがクリックされた際に実行される処理です。
        /// 画像一覧グリッドの選択チェックボックスがONの行の内容を、
        /// 最後に適用が実行された状態に戻します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnRevertSelection_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormUtilities.ShowMessage("Q006") == DialogResult.Yes)
                {
                    RevertEdit(false);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region btnRevertAll_Click - 全ての変更を取消ボタン押下時

        /// <summary>
        /// 全ての変更を取消ボタンがクリックされた際に実行される処理です。
        /// 画像一覧グリッドの全てのの行の内容を、
        /// 最後に適用が実行された状態に戻します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnRevertAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (FormUtilities.ShowMessage("Q011") == DialogResult.Yes)
                {
                    RevertEdit(true);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuRefresh_Click - 対象画像再取得メニュー押下時

        /// <summary>
        /// 対象画像再取得メニューがクリックされた際に実行される処理です。
        /// 現在の情報で対象画像リストを更新します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuRefresh_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            try
            {
                RefreshImageList();
                RevertEdit(true);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                Cursor = Cursors.Default;
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
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            try
            {
                if (RenameAllFiles() == ResultStatus.OK)
                {
                    RevertEdit(true);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
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
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            try
            {
                // ファイル移動実行、正常終了時はディレクトリ選択フォームへ戻る
                RefreshImageList();

                if (MoveAllFiles() == ResultStatus.OK)
                {
                    this.BackToOwner();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuSearchSimilarAll_Click - 全ての行の類似画像を検索メニュー押下時

        /// <summary>
        /// 全ての行の類似画像を検索メニューがクリックされた際に実行される処理です。
        /// 全ての行の画像の類似画像を対象ディレクトリ内から検索し、
        /// 類似画像があった画像のSimilarボタンのみを活性化状態に設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuSearchSimilarAll_Click(object sender, EventArgs e)
        {
            try
            {
                if (_similarMap != null)
                {
                    _similarMap.Clear();
                }

                _similarMap = new Dictionary<int, List<IImage>>();

                Action<int, bool> actResetSimilarButton = (rowIdx, enable) =>
                {
                    DataGridViewCell cell;
                    if (enable)
                    {
                        cell = new DataGridViewButtonCell();
                        grdImages[FileColumnIndice.SIMILAR_BUTTON, rowIdx] = cell;
                        cell.Value = "Similar";
                    }
                    else
                    {
                        cell = new DataGridViewTextBoxCell();
                        grdImages[FileColumnIndice.SIMILAR_BUTTON, rowIdx] = cell;
                        cell.Value = string.Empty;
                        cell.ReadOnly = true;
                    }
                };

                foreach (DataGridViewRow row in grdImages.Rows)
                {
                    IImage criterion;
                    string target;
                    if (ImageMode == ConfigInfo.ImageDataMode.File)
                    {
                        string path = GetImagePath(row.Index);
                        if (!File.Exists(path))
                        {
                            // ファイルが既に存在しない場合は押下不可能に設定
                            actResetSimilarButton(row.Index, false);
                            continue;
                        }

                        criterion = new FileImage(path);
                        target = "ファイルパス:";
                    }
                    else
                    {
                        criterion = new DataImage(GetImageId(row.Index));
                        target = "画像ID:";
                    }
                    string msg = string.Format("[{0}行目] {1} {2} の類似画像を検索中...{3}{3}",
                        row.Index + 1, target, criterion.Key, Environment.NewLine);

                    List<IImage> similarImages =
                        ImageController.GetSimilarImages(this, criterion, msg);

                    if (!similarImages.Any())
                    {
                        // 類似画像が存在しない場合は押下不可能に設定
                        actResetSimilarButton(row.Index, false);
                    }
                    else
                    {
                        // 類似画像が存在する場合は押下可能に設定し、類似画像パスを保管
                        actResetSimilarButton(row.Index, true);
                        _similarMap[row.Index] = similarImages;
                    }
                }

                FormUtilities.ShowMessage("I009");
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region menuFilterDuplicated_Click - 重複している画像のみを抽出メニュー押下時

        /// <summary>
        /// 重複している画像のみを抽出メニューがクリックされた際に実行される処理です。
        /// 重複が発生している画像の行のみを表示し、それ以外の行を非表示に設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuFilterDuplicated_Click(object sender, EventArgs e)
        {
            try
            {
                this.Update();
                Cursor.Current = Cursors.WaitCursor;

                int md5ColIdx = ImageMode == ConfigInfo.ImageDataMode.File
                    ? FileColumnIndice.MD5 : DatabaseColumnIndice.MD5;

                using (var progressDlg = new CircleProgressDialog(this))
                {
                    // プログレス表示開始
                    progressDlg.StartProgress();

                    grdImages.CellValidating -= grdImages_CellValidating;

                    // MD5が重複している画像のみを抽出して表示
                    FilterDuplicatedImages();

                    grdImages.CellValidating += grdImages_CellValidating;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region menuViewImage_Click - 選択行の画像を表示メニュー押下時

        /// <summary>
        /// 選択行の画像を表示メニューがクリックされた際に実行される処理です。
        /// 選択中の行の画像を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuViewImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdImages.SelectedCells.Count > 0)
                {
                    int rowIndex = grdImages.SelectedCells[0].RowIndex;
                    if (rowIndex > -1)
                    {
                        DataGridViewRow row = grdImages.Rows[rowIndex];
                        if (IsRowDeleted(row))
                        {
                            // 削除済みの場合はメッセージで警告
                            FormUtilities.ShowMessage("W006");
                        }
                        else
                        {
                            // 画像閲覧
                            IImage img;
                            if (ImageMode == ConfigInfo.ImageDataMode.File)
                            {
                                img = new FileImage(GetImagePath(rowIndex));
                            }
                            else
                            {
                                img = new DataImage(GetImageId(rowIndex));
                            }

                            new ViewImageForm(this, img, ImageMode).Show(this);

                            if (img.IsDeleted)
                            {
                                // 子画面で画像が削除された場合は行を削除状態に設定
                                SetRowDeleted(row, true);
                            }
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

        #region menuSimilar_Click - 選択行の類似画像を表示メニュー押下時

        /// <summary>
        /// 選択行の類似画像を表示メニューがクリックされた際に実行される処理です。
        /// 選択中の行の画像の類似画像を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void menuSimilar_Click(object sender, EventArgs e)
        {
            try
            {
                if (grdImages.SelectedCells.Count > 0)
                {
                    int rowIndex = grdImages.SelectedCells[0].RowIndex;
                    if (rowIndex > -1)
                    {
                        if (IsRowDeleted(grdImages.Rows[rowIndex]))
                        {
                            // 削除済みの場合はメッセージで警告
                            FormUtilities.ShowMessage("W006");
                        }
                        else
                        {
                            // イメージ閲覧
                            ShowSimilarImages(rowIndex);
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
    }
}
