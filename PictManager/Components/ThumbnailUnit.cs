using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

using SO.Library.Forms;
using SO.PictManager.Imaging;

namespace SO.PictManager.Components
{
    /// <summary>
    /// サムネイル表示機能提供クラス
    /// </summary>
    public partial class ThumbnailUnit : UserControl
    {
        #region クラス定数定義

        /// <summary>削除済名称ラベル</summary>
        public const string DELETED_NAME_LABEL = "Deleted.";

        #endregion

        #region インスタンス変数定義

        /// <summary>画像表示幅</summary>
        private int _picWidth;

        /// <summary>画像表示高さ</summary>
        private int _picHeight;

        #endregion

        #region プロパティ

        /// <summary>
        /// コンポーネントクリック時のアクションを取得または設定します。
        /// </summary>
        public Action<object, EventArgs> UnitClick { get; set; }

        /// <summary>
        /// コンポーネントダブルクリック時のアクションを取得または設定します。
        /// </summary>
        public Action<object, EventArgs> UnitDoubleClick { get; set; }

        /// <summary>
        /// 表示対象画像情報を取得または設定します。
        /// </summary>
        public IImage ImageData { get; set; }

        /// <summary>
        /// 画像キーを取得または設定します。
        /// </summary>
        public string ImageKey
        {
            get { return lblImageKey.Text; }
            set { lblImageKey.Text = value; }
        }

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
        /// 表示対象画像情報付きのコンストラクタです。
        /// </summary>
        /// <param name="imageData">表示対象画像情報</param>
        public ThumbnailUnit(IImage imageData)
        {
            // コンポーネント初期化
            InitializeComponent();

            // ファイルパス格納
            ImageData = imageData;

            // ピクチャボックスのサイズ初期化
            _picWidth = _picHeight = 80;

            // 画像キーをラベルに表示
            lblImageKey.Text = Path.GetFileName(ImageData.Key);

            // 指定パスの画像表示
            RefreshPicture();
        }

        #endregion

        #region RefreshPicture - 画像表示を更新

        /// <summary>
        /// 画像表示を更新します。
        /// </summary>
        public void RefreshPicture()
        {
            // 表示対象ファイル確認
            if (ImageData == null)
            {
                FormUtilities.ShowMessage("E005");
                return;
            }

            // 表示対象イメージをストリームから読み込み
            Image img;
            using (Image imgTemp = ImageData.GetImage())
            {
                // GDI+汎用エラー回避の為、ストリームの受け皿のImageから新規Imageのインスタンスを作成
                img = new Bitmap(imgTemp);
            }
            picThumbnail.SizeMode = PictureBoxSizeMode.Zoom;    // 常に縮小表示
            picThumbnail.Size = new Size(_picWidth, _picHeight);
            picThumbnail.Image = img;

            ResetToolTip();
        }

        #endregion

        #region ResetToolTip - ツールチップ表示再設定

        /// <summary>
        /// ツールチップに表示する内容を再設定します。
        /// </summary>
        private void ResetToolTip()
        {
            if (ImageData == null)
            {
                tipInfo.RemoveAll();
            }
            else
            {
                var caption = new StringBuilder();
                if (ImageData is FileImage)
                {
                    caption.Append("ファイルパス：");
                }
                else
                {
                    caption.Append("画像ID　　　：");
                }
                caption.Append(ImageData.Key);

                caption.AppendLine();

                caption.Append("サイズ　　　：");
                caption.Append(picThumbnail.Image.Width);
                caption.Append("×");
                caption.Append(picThumbnail.Image.Height);

                caption.AppendLine();

                caption.Append("更新日時　　：");
                caption.Append(ImageData.Timestamp.ToString("yyyy/MM/dd HH:mm:ss"));

                tipInfo.SetToolTip(picThumbnail, caption.ToString());
            }
        }

        #endregion

        //*** イベントハンドラ ***

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

        #region lblImageKey_Click - 表示画像キークリック時

        /// <summary>
        /// 画像の下の文字列をクリックした際に実行される処理です。
        /// UnitClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void lblImageKey_Click(object sender, EventArgs e)
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

        #region lblImageKey_DoubleClick - 表示画像キーダブルクリック時

        /// <summary>
        /// 画像の下の文字列をダブルクリックした際に実行される処理です。
        /// UnitDoubleClickアクションを実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void lblImageKey_DoubleClick(object sender, EventArgs e)
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
