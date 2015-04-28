using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using SO.Library.Drawing;
using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.Forms.Extensions;
using SO.Library.IO;
using SO.Library.Text;
using SO.PictManager.Components;
using SO.PictManager.Common;
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;

using Config = System.Configuration.ConfigurationManager;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 単体画像表示フォームクラス
    /// </summary>
    public partial class ViewImageForm : BaseForm
    {
        #region クラス定数

        /// <summary>表示エラー時の表示テキスト</summary>
        protected const string DISP_ERR_LABEL = "Error Occured\n when loading image.";

        /// <summary>倍率変更単位</summary>
        protected const int ZOOM_UNIT = 25;

        /// <summary>スクロール幅(小)の設定値</summary>
        protected const int SCROLL_CHANGE_SMALL = 50;

        /// <summary>スクロール幅(大)の設定値</summary>
        protected const int SCROLL_CHANGE_LARGE = 250;

        #endregion

        #region インスタンス変数

        /// <summary>ズーム倍率(%)</summary>
        private int _magnify = 100;

        /// <summary>倍率変更中フラグ</summary>
        private bool _zoomed = false;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// クラス継承時にのみ利用可能です。
        /// </summary>
        protected ViewImageForm()
        {
            // コンポーネント初期化
            InitializeComponent();

            // フィールド初期化
            ImageMode = ConfigInfo.ImageDataMode.File;

            // 共通コンストラクション
            ConstructCommon();
        }

        /// <summary>
        /// 親フォーム、画像情報、画像モードを指定可能なコンストラクタです。
        /// </summary>
        /// <param name="owner">親フォーム</param>
        /// <param name="imageData">画像情報</param>
        /// <param name="mode">画像モード(省略時：File)</param>
        public ViewImageForm(Form owner, IImage imageData,
            ConfigInfo.ImageDataMode mode = ConfigInfo.ImageDataMode.File)
        {
            // コンポーネント初期化
            InitializeComponent();

            // フィールド初期化
            Owner = owner;
            ImageMode = mode;
            ImageData = imageData;

            // UI制御
            InitializeAccessibility();

            // 共通コンストラクション
            ConstructCommon();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 画像情報を取得・設定します。
        /// </summary>
        protected IImage ImageData { get; set; }

        #endregion

        #region CreateMenu - メニューバー作成

        /// <summary>
        /// (BaseForm.CreateMenu()をオーバーライドします)
        /// メニューバーを生成します。
        /// </summary>
        protected override void CreateMenu()
        {
            if (ImageMode == ConfigInfo.ImageDataMode.File)
            {
                // ファイル
                var menuFile = new ToolStripMenuItem("ファイル(&F)", null, null, "menuFile");
                menuFile.ShortcutKeys = Keys.Alt | Keys.F;
                menuFile.DropDownItems.Add(new ToolStripMenuItem("戻る", null, btnClose_Click));
                menuFile.DropDownItems.Add(new ToolStripMenuItem("上書き保存", null, (s, e) => SaveImage()));
                menuFile.DropDownItems.Add(new ToolStripSeparator());
                menuFile.DropDownItems.Add(new ToolStripMenuItem("表示画像ファイル名変更", null, menuRename_Click));
                menuFile.DropDownItems.Add(new ToolStripMenuItem("表示画像移動", null, menuMove_Click));
                menuFile.DropDownItems.Add(new ToolStripMenuItem("表示画像削除", null, btnDelete_Click));
                menuFile.DropDownItems.Add(new ToolStripSeparator());
                menuFile.DropDownItems.Add(new ToolStripMenuItem("ディレクトリを開く", null,
                        (s, e) => Utilities.OpenExplorer(Path.GetDirectoryName(ImageData.Key))));
                menuFile.DropDownItems.Add(new ToolStripSeparator());
                menuFile.DropDownItems.Add(new ToolStripMenuItem("終了", null,
                        (s, e) => Form_FormClosing(s, new FormClosingEventArgs(CloseReason.UserClosing, false))));
                barMenu.Items.Add(menuFile);
            }

            // 表示
            var menuDisp = new ToolStripMenuItem("表示(&V)", null, null, "menuView");
            menuDisp.ShortcutKeys = Keys.Alt | Keys.V;
            var menuSizeMode = new ToolStripMenuItem("表示サイズモード", null, GetSizeModeMenuItems().ToArray());
            menuSizeMode.Name = "menuCmbSizeMode";
            menuDisp.DropDownItems.Add(menuSizeMode);
            menuDisp.DropDownItems.Add(new ToolStripSeparator());
            menuDisp.DropDownItems.Add(new ToolStripMenuItem("右に90°回転", null,
                    (s, e) => RotateImage(RotateFlipType.Rotate90FlipNone)));
            menuDisp.DropDownItems.Add(new ToolStripMenuItem("左に90°回転", null,
                    (s, e) => RotateImage(RotateFlipType.Rotate270FlipNone)));
            menuDisp.DropDownItems.Add(new ToolStripSeparator());
            menuDisp.DropDownItems.Add(new ToolStripMenuItem("グレースケール表示", null,
                    (s, e) => DisplayByGrayScale()));
            barMenu.Items.Add(menuDisp);
        }

        #endregion

        #region ShowImageInfoByStatusBar - ステータスバーに画像情報を表示

        /// <summary>
        /// ステータスバーに表示中の画像の情報を表示します。
        /// </summary>
        protected void ShowImageInfoByStatusBar()
        {
            if (ImageData != null)
            {
                using (Image img = ImageData.GetImage())
                {
                    lblStatus.Text = string.Format("パス：{0}    サイズ：{1}×{2}    更新日時：{3}",
                        ImageData.Key, img.Width, img.Height,
                        ImageData.Timestamp.ToString("yyyy/MM/dd HH:mm:ss"));
                }
            }
            else
            {
                lblStatus.Text = string.Empty;
            }
        }

        #endregion

        #region InitializeAccessibility - コンポーネントのアクセス制限初期化

        /// <summary>
        /// フォーム項目のアクセス可不可の初期設定を行ないます。
        /// </summary>
        protected virtual void InitializeAccessibility()
        {
            // 表示対象ファイルが無い場合は削除ボタン押下不可
            if (ImageData.Key.BlankToNull() == null) btnDelete.Enabled = false;
        }

        #endregion

        #region DisplayImage - 指定画像表示

        /// <summary>
        /// 指定された画像を表示します。
        /// </summary>
        protected virtual void DisplayImage()
        {
            try
            {
                // 現在表示中のイメージがある場合はそのリソースを解放
                if (picViewer.Image != null) picViewer.Image.Dispose();

                // ズーム中フラグ、倍率初期化
                _zoomed = false;
                _magnify = 100;

                // イメージ設定、サイズ再設定
                picViewer.Image = ImageData.GetImage();
                ResizeImageRect();

                lblInfo.Hide();
                picViewer.Show();

                // スクロール設定
                ResetScrollProperties();
            }
            catch (Exception ex)
            {
                // エラーラベル表示
                ShowInformationLabel(lblInfo.Text = DISP_ERR_LABEL);

                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());

                // エラー対象ファイル削除確認ダイアログ表示
                if (ImageMode == ConfigInfo.ImageDataMode.File
                    && DialogResult.Yes == FormUtilities.ShowMessage("I010"))
                {
                    // エラーファイル削除
                    btnDelete_Click(this, new EventArgs());
                }
            }
        }

        #endregion

        #region ResizeImageRect - 画像表示領域サイズ更新

        /// <summary>
        /// フォームのピクチャボックスのサイズを、フォームのサイズに応じて調整します。
        /// </summary>
        protected virtual void ResizeImageRect()
        {
            try
            {
                // PictureBoxSizeModeの型情報取得
                Type t = typeof(PictureBoxSizeMode);
                FieldInfo fieldInfo = t.GetField(cmbPicMode.SelectedItem.ToString());

                // コンボボックスの選択アイテムと同名のサイズモードを設定
                picViewer.SizeMode = (PictureBoxSizeMode)fieldInfo.GetValue(null);

                // Fill時のサイズを取得
                picViewer.Dock = DockStyle.Fill;
                System.Drawing.Size s = picViewer.Size;

                // Fillを解除し、取得しておいたサイズに変更
                // (AutoSizeではみ出す場合には再度サイズが自動拡張される)
                picViewer.Dock = DockStyle.None;
                picViewer.Size = s;

                // スクロール設定
                ResetScrollProperties();

                // メニューのチェック状態を更新
                var menuItem = FormUtilities.GetMenuItem<ToolStripMenuItem>(barMenu.Items, "menuView/menuCmbSizeMode");
                if (menuItem != null)
                    foreach (ToolStripMenuItem item in menuItem.DropDownItems)
                        item.Checked = item.Text == cmbPicMode.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region ShowInformationLabel - 情報ラベル表示

        /// <summary>
        /// 情報通知ラベルを表示します。
        /// </summary>
        /// <param name="text">ラベルに表示するメッセージ</param>
        protected void ShowInformationLabel(string text)
        {
            // 表示画像がnullの場合、PictureBoxのサイズ未確定エラー回避の為、
            // サイズモードをNormal、サイズを固定に強制変更
            if (picViewer.Image == null)
            {
                picViewer.SizeMode = PictureBoxSizeMode.Normal;
                picViewer.Size = new Size(1, 1);
            }

            // 情報ラベル設定・可視化、PictureBox不可視化
            lblInfo.Text = text;
            lblInfo.Show();
            picViewer.Hide();
        }

        #endregion

        #region SaveStateInfo - 状態情報保存

        /// <summary>
        /// 状態情報をシリアライズしてXMLファイルとして保存します。
        /// </summary>
        protected virtual void SaveStateInfo()
        {
            try
            {
                Utilities.State.SizeMode = picViewer.SizeMode;
                Utilities.SaveStateInfo();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region GetSizeModeMenuItems - 画像サイズモード選択メニュー取得

        /// <summary>
        /// ピクチャボックスのサイズモードを選択するメニューの項目を取得します。
        /// </summary>
        /// <returns>サイズモード選択メニュー項目</returns>
        protected IEnumerable<ToolStripMenuItem> GetSizeModeMenuItems()
        {
            foreach (var fld in typeof(PictureBoxSizeMode).GetFields(BindingFlags.Public | BindingFlags.Static))
            {
                string name = fld.GetValue(null).ToString();
                yield return new ToolStripMenuItem(name, null, (s, e) => cmbPicMode.SelectedItem = name, name);
            }
        }

        #endregion

        #region ZoomImage - 画像表示倍率変更

        /// <summary>
        /// 画像の表示倍率を変更します。
        /// </summary>
        /// <param name="magnify">画像表示倍率</param>
        /// <returns>倍率が変更された場合:true / 倍率が変更されなかった場合:false</returns>
        protected bool ZoomImage(int magnify)
        {
            try
            {
                // 0%以下もしくは最大値を超える場合は処理無し
                if (magnify <= 0 ||
                        picViewer.Image.Width * magnify / 100 > int.MaxValue ||
                        picViewer.Image.Height * magnify / 100 > int.MaxValue)
                    return false;

                // SizeModeを自動伸張に設定
                cmbPicMode.SelectedItem = PictureBoxSizeMode.StretchImage.ToString();

                // 画像表示領域伸縮
                if (picViewer.Image.Width * magnify / 100 <= int.MaxValue && magnify > 0)
                    picViewer.Width = picViewer.Image.Width * magnify / 100;
                if (picViewer.Image.Height * magnify / 100 <= int.MaxValue && magnify > 0)
                    picViewer.Height = picViewer.Image.Height * magnify / 100;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
                return false;
            }

            // ズーム中フラグON
            return _zoomed = true;
        }

        #endregion

        #region RotateImage - 表示画像回転

        /// <summary>
        /// 表示されている画像を回転させます。
        /// </summary>
        /// <param name="rotate">回転種類</param>
        protected void RotateImage(RotateFlipType rotate)
        {
            try
            {
                // 表示画像回転
                picViewer.Image.RotateFlip(rotate);
                picViewer.Refresh();

                // ズーム中の場合は幅と高さを入替
                if (_zoomed)
                {
                    int height = picViewer.Width;
                    int width = picViewer.Height;
                    picViewer.Height = height;
                    picViewer.Width = width;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region SaveImage - 画像ファイル上書き保存

        /// <summary>
        /// 表示中の画像に対して行なった変更を上書き保存します。
        /// </summary>
        protected void SaveImage()
        {
            try
            {
                if (ImageMode == ConfigInfo.ImageDataMode.File
                    && FormUtilities.ShowMessage("Q007") == DialogResult.Yes)
                {
                    picViewer.Image.Save(ImageList[CurrentIndex].Key);
                }
                else
                {
                    //TODO 画像保存(DBモード時)
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region DisplayByGrayScale - グレースケール表示

        /// <summary>
        /// 表示中の画像をグレースケール画像に変換して表示します。
        /// </summary>
        protected void DisplayByGrayScale()
        {
            if (picViewer.Image != null)
            {
                // 現在表示中のイメージのリソースを解放
                picViewer.Image.Dispose();

                // NTSC係数を用いた加重平均法でグレースケール化
                Bitmap bmp = new Bitmap(ImageData.GetImage());
                picViewer.Image = ImageUtilities.ToGrayScale(bmp, GrayScaleMethod.NTSC);
            }
        }

        #endregion

        #region CloseForm - フォームをクローズ

        /// <summary>
        /// フォームをクローズします。
        /// </summary>
        protected override void CloseForm()
        {
            // 自フォームを破棄し親フォームを表示
            SaveStateInfo();
            this.BackToOwner();
        }

        #endregion

        #region ResetScrollProperties - スクロール設定

        /// <summary>
        /// スクロール設定をリセットします。
        /// </summary>
        private void ResetScrollProperties()
        {
            // 垂直スクロールバー設定
            if (pnlParent.Panel1.VerticalScroll.Visible)
            {
                pnlParent.Panel1.AutoScrollPosition = new Point(0, pnlParent.Panel1.VerticalScroll.Minimum);
                pnlParent.Panel1.VerticalScroll.SmallChange = picViewer.Size.Height / 20;
                pnlParent.Panel1.VerticalScroll.LargeChange = picViewer.Size.Height / 4;
            }

            // 水平スクロールバー設定
            if (pnlParent.Panel1.HorizontalScroll.Visible)
            {
                pnlParent.Panel1.AutoScrollPosition = new Point(pnlParent.Panel1.HorizontalScroll.Minimum, 0);
                pnlParent.Panel1.HorizontalScroll.SmallChange = picViewer.Size.Width / 20;
                pnlParent.Panel1.HorizontalScroll.LargeChange = picViewer.Size.Width / 4;
            }
        }

        #endregion

        #region ConstructCommon - 共通コンストラクション

        /// <summary>
        /// インスタンス構築時の共通処理を実行します。
        /// </summary>
        private void ConstructCommon()
        {
            // 情報ラベル不可視化
            lblInfo.Hide();

            // タイトルバー表示
            this.Text = string.Format("PictManager - イメージビューア [{0}:{1}]",
                ImageMode == ConfigInfo.ImageDataMode.File ? "画像パス" : "画像ID",
                ImageData == null ? string.Empty : ImageData.Key);

            // ステータスバーに画像情報を表示
            ShowImageInfoByStatusBar();

            // コンボボックス初期化
            foreach (var item in GetSizeModeMenuItems())
                cmbPicMode.Items.Add(item.Text);

            // スクロール幅設定
            pnlParent.Panel1.VerticalScroll.SmallChange = SCROLL_CHANGE_SMALL;
            pnlParent.Panel1.HorizontalScroll.SmallChange = SCROLL_CHANGE_SMALL;
            pnlParent.Panel1.VerticalScroll.LargeChange = SCROLL_CHANGE_LARGE;
            pnlParent.Panel1.HorizontalScroll.LargeChange = SCROLL_CHANGE_LARGE;

            // サイズモード復元
            cmbPicMode.SelectedItem = Utilities.State.SizeMode.ToString();
        }

        #endregion

        #region イベントハンドラ

        #region Form_FormClosing - ×ボタン押下時

        /// <summary>
        /// ×ボタンがクリックされた際に実行される処理です。
        /// 自フォームを破棄し、親フォームを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            // 自フォームを破棄し親フォームをアクティブ化
            CloseForm();
        }

        #endregion

        #region Form_Shown - フォーム初回表示時

        /// <summary>
        /// フォームが表示された際に実行される処理です。
        /// コンストラクタで指定されたパスに存在する画像ファイルを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void Form_Shown(object sender, EventArgs e)
        {
            // picViewerの初期化後クライアントサイズが必要なのでコンストラクタではなくこっち
            if (ImageData != null) DisplayImage();
        }

        #endregion

        #region Form_ResizeEnd - フォームサイズ変更終了時

        /// <summary>
        /// フォームのサイズ変更が終了した際に実行される処理です。
        /// フォームのピクチャボックスのサイズを、フォームのサイズに合わせて調整します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected void Form_ResizeEnd(object sender, EventArgs e)
        {
            // PitureBoxのサイズを再設定(ズーム表示中は設定無し)
            if (!_zoomed) ResizeImageRect();
        }

        #endregion

        #region Form_Resize - フォームサイズ変更時

        /// <summary>
        /// フォームのサイズが変更された際に実行される処理です。
        /// ピクチャボックスのサイズモードを再設定します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void Form_Resize(object sender, EventArgs e)
        {
            // PitureBoxのサイズを再設定(ズーム表示中は設定無し)
            if (!_zoomed) ResizeImageRect();
        }

        #endregion

        #region Form_MouseWheel - フォーム上マウスホイール回転時

        /// <summary>
        /// フォーム上でマウスホイールが回された際に実行される処理です。
        /// 表示中の画像をスクロールします。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void Form_MouseWheel(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Delta == 0) return;

                if (pnlParent.Panel1.VerticalScroll.Visible)
                {
                    int delta = e.Delta / Constants.WHEEL_DELTA * pnlParent.Panel1.VerticalScroll.SmallChange * -1;
                    if (delta < 0)
                    {
                        if (-pnlParent.Panel1.AutoScrollPosition.Y + delta
                                < pnlParent.Panel1.VerticalScroll.Minimum)
                        {
                            pnlParent.Panel1.AutoScrollPosition =
                                    new Point(0, pnlParent.Panel1.VerticalScroll.Minimum);
                            return;
                        }
                    }
                    else
                    {
                        if (-pnlParent.Panel1.AutoScrollPosition.Y + delta
                                > pnlParent.Panel1.VerticalScroll.Maximum)
                        {
                            pnlParent.Panel1.AutoScrollPosition =
                                    new Point(0, pnlParent.Panel1.VerticalScroll.Maximum);
                            return;
                        }
                    }

                    pnlParent.Panel1.AutoScrollPosition =
                            new Point(0, -pnlParent.Panel1.AutoScrollPosition.Y + delta);
                }
                else if (pnlParent.Panel1.HorizontalScroll.Visible)
                {
                    int delta = e.Delta / Constants.WHEEL_DELTA * pnlParent.Panel1.HorizontalScroll.SmallChange * -1;
                    if (delta < 0)
                    {
                        if (-pnlParent.Panel1.AutoScrollPosition.X + delta
                                < pnlParent.Panel1.HorizontalScroll.Minimum)
                        {
                            pnlParent.Panel1.AutoScrollPosition =
                                    new Point(pnlParent.Panel1.HorizontalScroll.Minimum, 0);
                            return;
                        }
                    }
                    else
                    {
                        if (-pnlParent.Panel1.AutoScrollPosition.X + delta
                                < pnlParent.Panel1.HorizontalScroll.Maximum)
                        {
                            pnlParent.Panel1.AutoScrollPosition =
                                    new Point(pnlParent.Panel1.HorizontalScroll.Maximum, 0);
                            return;
                        }
                    }

                    pnlParent.Panel1.AutoScrollPosition =
                            new Point(-pnlParent.Panel1.AutoScrollPosition.X + delta, 0);
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
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
                if ((e.KeyCode & Keys.Alt) != Keys.Alt &&
                        (e.KeyCode & Keys.Control) != Keys.Control &&
                        (e.KeyCode & Keys.Shift) != Keys.Shift)
                {
                    Keys kcode = e.KeyCode & Keys.KeyCode;
                    switch (kcode)
                    {
                        case Keys.Escape:
                            // 自フォームを破棄して親フォームを表示
                            CloseForm();
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

        #region btnClose_Click - 閉じるボタン押下時

        /// <summary>
        /// 閉じるボタンがクリックされた際に実行される処理です。
        /// 自フォームを破棄し、親フォームを表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected void btnClose_Click(object sender, EventArgs e)
        {
            CloseForm();
        }

        #endregion

        #region btnDelete_Click - 削除ボタン押下時

        /// <summary>
        /// 削除ボタンがクリックされた際に実行される処理です。
        /// 表示中の画像を削除します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void btnDelete_Click(object sender, EventArgs e)
        {
            // PictureBoxリソース解放
            picViewer.Image.Dispose();

            // 表示中の画像を削除
            ImageData.Delete();
        }

        #endregion

        #region btnZoomIn_Click - ズームインボタン押下時

        /// <summary>
        /// ズームインボタンがクリックされた際に実行される処理です。
        /// 表示中の画像を25%拡大します。
        /// 拡大上限に達した場合は何もしません。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            // ズーム倍率25%増加
            if (ZoomImage(_magnify + ZOOM_UNIT)) _magnify += ZOOM_UNIT;
        }

        #endregion

        #region btnZoomOut_Click - ズームアウトボタン押下時

        /// <summary>
        /// ズームアウトボタンクリックされた際に実行される処理です。
        /// 表示中の画像を25%縮小します。
        /// 縮小後のサイズが0になる場合は何もしません。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            // ズーム倍率25%減少
            if (ZoomImage(_magnify - ZOOM_UNIT)) _magnify -= ZOOM_UNIT;
        }

        #endregion

        #region cmbPicMode_SelectedIndexChanged - イメージサイズモード選択コンボ変更時

        /// <summary>
        /// 画像サイズモード選択コンボの選択値が変更された場合に実行される処理です。
        /// 選択されたモードに応じて、画像の表示タイプを変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void cmbPicMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ズーム中フラグ、倍率初期化
            _zoomed = false;
            _magnify = 100;

            // PitureBoxのサイズを再設定
            ResizeImageRect();
        }

        #endregion

        #region menuRename_Click - 表示画像ファイル名変更メニュー押下時

        /// <summary>
        /// 表示画像ファイル名変更メニューがクリックされた際に実行される処理です。
        /// 表示中の画像のファイル名を、入力ダイアログで指定された内容に変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void menuRename_Click(object sender, EventArgs e)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            // ファイル名変更
            if (RenameFile() != ResultStatus.OK) return;

            // ステータスバー更新
            lblStatus.Text = ImageList[CurrentIndex].Key;
        }

        #endregion

        #region menuMove_Click - 表示画像移動メニュー押下時

        /// <summary>
        /// 表示画像移動メニューがクリックされた際に実行される処理です。
        /// 表示中の画像を指定ディレクトリに移動し、前画面へ戻ります。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void menuMove_Click(object sender, EventArgs e)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.File);

            // ファイル名変更
            MoveFile();

            // 前画面へ戻る
            this.BackToOwner();
        }

        #endregion

        #endregion
    }
}
