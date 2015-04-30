using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.PictManager.Common;
using SO.PictManager.Forms.Info;

using Config = System.Configuration.ConfigurationManager;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// ブックマーク管理フォームクラス
    /// </summary>
    public sealed partial class BookmarkForm : Form
    {
        #region メンバ変数

        /// <summary>ブックマーク情報保存XMLファイルパス</summary>
        private readonly string _bookmarkFilePath;

        /// <summary>有効なファイルソート順</summary>
        private ImageSortOrder _sortOrder;

        #endregion

        #region プロパティ

        /// <summary>
        /// 有効なファイルソート順を取得または設定します。
        /// </summary>
        public ImageSortOrder SortOrder
        {
            get { return _sortOrder; }
            set
            {
                _sortOrder = value;
                SetBookmarkEnabled();
            }
        }

        #endregion

        #region デリゲート・イベント

        /// <summary>
        /// ブックマークジャンプイベントのハンドラです。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        public delegate void BookmarkEventHandler(object sender, BookmarkEventArgs e);

        /// <summary>
        /// ブックマークジャンプイベントです。
        /// </summary>
        public event BookmarkEventHandler BookmarkJump;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 唯一のコンストラクタです。
        /// </summary>
        public BookmarkForm(ImageSortOrder order)
        {
            // コンポーネント初期化
            InitializeComponent();

            _bookmarkFilePath = Config.AppSettings[BookmarkInfo.SAVE_PATH_KEY];

            // ブックマーク読込
            RefreshBookmarks();

            // 有効なソート順を設定
            SortOrder = order;
        }

        #endregion

        #region RefreshBookmarks - 表示されているブックマークの内容を最新状態に更新
        /// <summary>
        /// 表示されているブックマークの内容を最新状態に更新します。
        /// </summary>
        public void RefreshBookmarks()
        {
            dgvBookmarks.Rows.Clear();

            if (!File.Exists(_bookmarkFilePath)) return;

            foreach (var xelm in XDocument.Load(_bookmarkFilePath).Root.Descendants())
            {
                string order = ImageSorter.GetSortOrderDisplayText(xelm.Attribute("SortOrder").Value);

                dgvBookmarks.Rows.Add(
                    xelm.Attribute("Id").Value,
                    xelm.Attribute("Name").Value,
                    xelm.Attribute("FileNo").Value,
                    xelm.Attribute("Timestamp").Value,
                    order);
            }

            SetBookmarkEnabled();
        }
        #endregion

        #region RaiseBookmarkJump - ブックマークジャンプイベント発生
        /// <summary>
        /// ブックマークジャンプイベントを発生させます。
        /// </summary>
        public void RaiseBookmarkJump()
        {
            if (BookmarkJump != null)
            {
                if (dgvBookmarks.SelectedRows.Count == 0) return;

                DataGridViewRow selectedRow;
                if (dgvBookmarks.SelectedRows.Count > 1)
                {
                    selectedRow = dgvBookmarks.SelectedRows.Cast<DataGridViewRow>()
                        .OrderBy(r => r.Index).First();
                    foreach (DataGridViewRow row in dgvBookmarks.Rows)
                    {
                        if (row.Index != selectedRow.Index)
                            row.Selected = false;
                    }
                }
                else
                {
                    selectedRow = dgvBookmarks.SelectedRows[0];
                }

                if (!IsBookmarkEnabled(selectedRow)) return;

                var info = new BookmarkInfo(
                    Convert.ToInt32(selectedRow.Cells["colId"].Value),
                    selectedRow.Cells["colName"].Value.ToString(),
                    Convert.ToInt32(selectedRow.Cells["colFileNo"].Value),
                    DateTime.ParseExact(selectedRow.Cells["colTimestamp"].Value.ToString(),
                        BookmarkInfo.TIMESTAMP_FORMAT, null),
                    ImageSorter.GetSortOrderByDisplayText(selectedRow.Cells["colSortOrder"].Value.ToString()));
                BookmarkJump(this, new BookmarkEventArgs(info));
            }
        }
        #endregion

        #region DeleteBookmark - 選択されているブックマークを削除
        /// <summary>
        /// 選択されているブックマークを削除します。
        /// </summary>
        private void DeleteBookmark()
        {
            if (dgvBookmarks.SelectedRows.Count == 0) return;

            var selectedRowIndexes = from r in dgvBookmarks.SelectedRows.Cast<DataGridViewRow>()
                                     orderby r.Index descending
                                     select r.Index;

            XDocument xdoc = XDocument.Load(_bookmarkFilePath);
            var xelms = xdoc.Root.Descendants();
            foreach (var i in selectedRowIndexes)
            {
                // XMLから情報を削除
                string id = dgvBookmarks.Rows[i].Cells["colId"].Value.ToString();
                xdoc.Root.Descendants().Where(elm => elm.Attribute("Id").Value == id).Single().Remove();

                // グリッドから情報を削除
                dgvBookmarks.Rows.RemoveAt(i);
            }
            
            xdoc.Save(_bookmarkFilePath);
        }
        #endregion

        #region ClearBookmarks - 全てのブックマークを削除
        /// <summary>
        /// 全てのブックマークを削除します。
        /// </summary>
        private void ClearBookmarks()
        {
            if (dgvBookmarks.RowCount == 0
                    || FormUtilities.ShowMessage("Q010") == DialogResult.No)
                return;

            // XMLから情報を削除
            XDocument xdoc = XDocument.Load(_bookmarkFilePath);
            xdoc.Root.RemoveNodes();

            xdoc.Save(_bookmarkFilePath);

            // グリッドから情報を削除
            dgvBookmarks.Rows.Clear();
        }
        #endregion

        #region SetBookmarkEnabled - ブックマークの行有効状態設定
        /// <summary>
        /// ブックマークの有効状態を設定します。
        /// </summary>
        private void SetBookmarkEnabled()
        {
            foreach (DataGridViewRow row in dgvBookmarks.Rows)
            {
                ImageSortOrder order =
                    ImageSorter.GetSortOrderByDisplayText(row.Cells["colSortOrder"].Value.ToString());
                if (order == _sortOrder)
                {
                    row.DefaultCellStyle.BackColor = SystemColors.Window;
                    row.DefaultCellStyle.SelectionBackColor = SystemColors.Highlight;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = SystemColors.ControlDark;
                    row.DefaultCellStyle.SelectionBackColor = SystemColors.HotTrack;
                }
            }
        }
        #endregion

        #region IsBookmarkEnabled - ブックマークの有効状態を取得
        /// <summary>
        /// ブックマークの有効状態を取得します。
        /// </summary>
        /// <param name="row">ブックマーク情報の表示行</param>
        /// <returns>ブックマークの有効状態</returns>
        private bool IsBookmarkEnabled(DataGridViewRow row)
        {
            ImageSortOrder order =
                ImageSorter.GetSortOrderByDisplayText(row.Cells["colSortOrder"].Value.ToString());
            return order == _sortOrder;
        }
        #endregion

        #region イベントハンドラ

        #region BookmarkForm_Load - フォームロード時
        /// <summary>
        /// フォームがロードされた際の処理です。
        /// フォームの表示関連の初期設定を行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void BookmarkForm_Load(object sender, EventArgs e)
        {
            // 設定情報から常に前面表示するかの設定を読込
            chkTopMost.Checked = Utilities.Config.SlideInfo.IsBookmarkTopMost;
        }
        #endregion

        #region btnJump_Click - ジャンプボタン押下時
        /// <summary>
        /// ジャンプボタンが押下された際の処理です。
        /// 選択されているブックマークのブックマークジャンプイベントを発生させます。
        /// 複数のブックマークが選択されている場合、最も上のものが対象となります。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnJump_Click(object sender, EventArgs e)
        {
            try
            {
                RaiseBookmarkJump();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region btnDelete_Click - 削除ボタン押下時
        /// <summary>
        /// 削除ボタンが押下された際の処理です。
        /// 選択されているブックマークを削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DeleteBookmark();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region btnClear_Click - クリアボタン押下時
        /// <summary>
        /// クリアボタンが押下された際の処理です。
        /// 全てのブックマークを削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearBookmarks();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region chkTopMost_CheckedChanged - 最前面表示チェックボックス変更時
        /// <summary>
        /// 最前面表示チェックボックス変更時のチェックが変更された際の処理です。
        /// チェックが付いている場合、常にウィンドウを最前面に表示するよう設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkTopMost_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                TopMost = chkTopMost.Checked;

                // 変更されている場合は設定情報に保存
                ConfigInfo.SlideConfig slideConf = Utilities.Config.SlideInfo;
                if (chkTopMost.Checked != slideConf.IsBookmarkTopMost)
                {
                    slideConf.IsBookmarkTopMost = chkTopMost.Checked;
                    Utilities.SaveConfigInfo();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region dgvBookmarks_CellDoubleClick - ブックマークグリッドセルダブルクリック時
        /// <summary>
        /// ブックマークグリッドのセルがダブルクリックされた際の処理です。
        /// 選択されたブックマークのブックマークジャンプイベントを発生させます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void dgvBookmarks_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                    RaiseBookmarkJump();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #region dgvBookmarks_CellEndEdit - ブックマークグリッドセル編集時
        /// <summary>
        /// ブックマークグリッドのセルが編集された際の処理です。
        /// 編集された内容をブックマーク情報に反映します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void dgvBookmarks_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvBookmarks.Columns[e.ColumnIndex].Name != "colName") return;

                DataGridViewRow row = dgvBookmarks.Rows[e.RowIndex];
                string id = row.Cells["colId"].Value.ToString();

                XDocument xdoc = XDocument.Load(_bookmarkFilePath);
                XElement xelm = xdoc.Root.Descendants().Where(
                    elm => elm.Attribute("Id").Value == id).Single();

                xelm.Attribute("Name").Value = row.Cells["colName"].Value.ToString();

                xdoc.Save(_bookmarkFilePath);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }
        #endregion

        #endregion
    }

    #region BookmarkEventArgs - ブックマークイベント引数クラス

    /// <summary>
    /// ブックマークイベント引数クラス
    /// </summary>
    /// <seealso cref="System.EventArgs"/>
    public class BookmarkEventArgs : EventArgs
    {
        #region プロパティ

        /// <summary>ブックマーク情報を取得します。</summary>
        public BookmarkInfo Bookmark { get; private set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// ブックマーク情報の各フィールドの値を指定してインスタンスを作成するコンストラクタです。
        /// </summary>
        /// <param name="id">ブックマークのID</param>
        /// <param name="name">ブックマーク名</param>
        /// <param name="fileNo">ブックマークしたファイルのNo</param>
        /// <param name="timeStamp">ブックマークした日時</param>
        /// <param name="sortOrder">ブックマーク時のファイルソート順</param>
        public BookmarkEventArgs(int id, string name, int fileNo, DateTime timeStamp, ImageSortOrder sortOrder)
        {
            Bookmark = new BookmarkInfo(id, name, fileNo, timeStamp, sortOrder);
        }

        /// <summary>
        /// ブックマーク情報自体を指定してインスタンスを作成するコンストラクタです。
        /// </summary>
        /// <param name="info">ブックマーク情報</param>
        public BookmarkEventArgs(BookmarkInfo info)
        {
            Bookmark = info;
        }

        #endregion
    }

    #endregion
}
