using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

using SO.Library.Forms;
using SO.Library.Text;
using SO.PictManager.Common;

namespace SO.PictManager.Components
{
    /// <summary>
    /// サムネイル表示機能提供クラス
    /// </summary>
    public partial class ThumbnailUnit : UserControl
    {
        #region クラス変数宣言・定義

        /// <summary>画像表示幅</summary>
        private int _picWidth;

        /// <summary>画像表示高さ</summary>
        private int _picHeight;

        /// <summary>削除済名称ラベル</summary>
        public const string DELETED_NAME_LABEL = "Deleted.";

        /// <summary>仮削除ファイル格納フォルダ</summary>
        private const string STORE_DIR_NAME = "DeletedFiles";

        #endregion

        #region 自動定義プロパティ

        /// <summary>
        /// コンポーネントクリック時のアクションを取得または設定します。
        /// </summary>
        public Action<object, EventArgs> UnitClick { get; set; }

        /// <summary>
        /// コンポーネントダブルクリック時のアクションを取得または設定します。
        /// </summary>
        public Action<object, EventArgs> UnitDoubleClick { get; set; }

        /// <summary>
        /// 表示対象ファイルのパスを取得または設定します。
        /// </summary>
        public string FilePath { get; set; }

        #endregion

        #region NameLabel - 名称ラベル取得・設定プロパティ
        /// <summary>
        /// 画像の下に表示する文字列を取得または設定します。
        /// </summary>
        public string NameLabel
        {
            get { return lblFileName.Text; }
            set { lblFileName.Text = value; }
        }
        #endregion

        #region PictureBox - ファイル表示用PictureBox取得プロパティ
        /// <summary>
        /// 画像を表示するPictureBoxを取得します。
        /// </summary>
        public PictureBox PictureBox
        {
            get { return picThumbnail; }
        }
        #endregion

        #region コンストラクタ
        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public ThumbnailUnit()
        {
            // コンポーネント初期化
            InitializeComponent();

            // ピクチャボックスのサイズ初期化
            _picWidth = _picHeight = 80;
        }

        /// <summary>
        /// 表示対象ファイルパス指定付きのコンストラクタです。
        /// </summary>
        /// <param name="newPath">表示対象ファイルパス</param>
        public ThumbnailUnit(string filePath)
        {
            // コンポーネント初期化
            InitializeComponent();

            // ファイルパス格納
            FilePath = filePath;

            // ピクチャボックスのサイズ初期化
            _picWidth = _picHeight = 80;

            // ファイル名をラベルに表示
            lblFileName.Text = Path.GetFileName(filePath);

            // 指定パスの画像表示
            RefreshPicture();
        }
        #endregion

        #region RefreshPicture - 指定パスで画像表示を更新
        /// <summary>
        /// FilePathで指定された画像を表示します。
        /// </summary>
        public void RefreshPicture()
        {
            // 表示対象ファイル確認
            if (FilePath == null)
            {
                FormUtilities.ShowMessage("E005");
                return;
            }

            // 表示対象対象が読み取り専用の場合、それを一時的に解除
            var target = new FileInfo(FilePath);
            bool readOnlyFlg;
            if (readOnlyFlg = target.IsReadOnly)   // ←比較では無く代入
            {
                target.Attributes = target.Attributes ^ FileAttributes.ReadOnly;
            }

            // 表示対象イメージをストリームから読み込み
            Image img;
            using (var fs = new FileStream(FilePath, FileMode.Open))
            using (Image imgTemp = Image.FromStream(fs))
            {
                // GDI+汎用エラー回避の為、ストリームの受け皿のImageから新規Imageのインスタンスを作成
                img = new Bitmap(imgTemp);
            }
            picThumbnail.SizeMode = PictureBoxSizeMode.Zoom;    // 常に縮小表示
            picThumbnail.Size = new Size(_picWidth, _picHeight);
            picThumbnail.Image = img;

            // 読み取り専用を解除した場合、再設定を行う
            if (readOnlyFlg)
            {
                target.Attributes = target.Attributes | FileAttributes.ReadOnly;
                readOnlyFlg = false;
            }

            ResetToolTip();
        }
        #endregion

        #region ResetToolTip - ツールチップ表示再設定
        /// <summary>
        /// ツールチップに表示する内容を再設定します。
        /// </summary>
        private void ResetToolTip()
        {
            if (string.IsNullOrEmpty(FilePath))
            {
                tipInfo.RemoveAll();
            }
            else
            {
                StringBuilder caption = new StringBuilder();
                caption.Append("パス　　：");
                caption.Append(FilePath);

                caption.AppendLine();

                caption.Append("サイズ　：");
                caption.Append(picThumbnail.Image.Width);
                caption.Append("×");
                caption.Append(picThumbnail.Image.Height);

                caption.AppendLine();

                caption.Append("更新日時：");
                caption.Append(File.GetLastWriteTime(FilePath).ToString("yyyy/MM/dd HH:mm:ss"));

                tipInfo.SetToolTip(picThumbnail, caption.ToString());
            }
        }
        #endregion

        #region picThumbnail_Click - 表示画像クリック時
        /// <summary>
        /// 表示画像をクリックした際に実行される処理です。
        /// UnitClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void picThumbnail_Click(object sender, EventArgs e)
        {
            UnitClick(this, e);
        }
        #endregion

        #region lblFileName_Click - 表示画像名クリック時
        /// <summary>
        /// 画像の下の文字列をクリックした際に実行される処理です。
        /// UnitClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void lblFileName_Click(object sender, EventArgs e)
        {
            UnitClick(this, e);
        }
        #endregion

        #region ThumbnailUnit_Click - コントロール本体クリック時
        /// <summary>
        /// コントロール本体をクリックした際に実行される処理です。
        /// UnitClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ThumbnailUnit_Click(object sender, EventArgs e)
        {
            UnitClick(sender, e);
        }
        #endregion

        #region picThumbnail_DoubleClick - 表示画像ダブルクリック時
        /// <summary>
        /// 表示画像をダブルクリックした際に実行される処理です。
        /// UnitDoubleClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void picThumbnail_DoubleClick(object sender, EventArgs e)
        {
            UnitDoubleClick(this, e);
        }
        #endregion

        #region lblFileName_DoubleClick - 表示画像名ダブルクリック時
        /// <summary>
        /// 画像の下の文字列をダブルクリックした際に実行される処理です。
        /// UnitDoubleClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void lblFileName_DoubleClick(object sender, EventArgs e)
        {
            UnitDoubleClick(this, e);
        }
        #endregion

        #region ThumbnailUnit_DoubleClick - コントロール本体ダブルクリック時
        /// <summary>
        /// コントロール本体をダブルクリックした際に実行される処理です。
        /// UnitDoubleClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void ThumbnailUnit_DoubleClick(object sender, EventArgs e)
        {
            UnitDoubleClick(sender, e);
        }
        #endregion
    }
}
