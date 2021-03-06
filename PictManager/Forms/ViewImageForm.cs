﻿using System;
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
using SO.PictManager.Common;
using SO.PictManager.Components;
using SO.PictManager.DataModel;
using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;
using SO.PictManager.Properties;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 単一画像表示フォームクラス
    /// </summary>
    public partial class ViewImageForm : BaseForm
    {
        #region クラス定数

        /// <summary>倍率変更単位</summary>
        protected const int ZOOM_UNIT = 25;

        /// <summary>スクロール幅(小)の設定値</summary>
        protected const int SCROLL_CHANGE_SMALL = 50;

        /// <summary>スクロール幅(大)の設定値</summary>
        protected const int SCROLL_CHANGE_LARGE = 250;

        /// <summary>新規タグ入力用テキストボックスのコントロール名</summary>
        protected const string TEXT_BOX_NAME_NEW_TAG = "txtNewTag";

        /// <summary>タグパネルの最小の高さ</summary>
        protected const int TAG_PANEL_MIN_HEIGHT = 30;

        /// <summary>フッターエリアの高さ</summary>
        protected const int FOOTER_HEIGHT = 50;

        #endregion

        #region インスタンス変数

        /// <summary>ズーム倍率(%)</summary>
        private int _magnify = 100;

        /// <summary>倍率変更中フラグ</summary>
        private bool _zoomed = false;

        /// <summary>部分拡大位置</summary>
        private Point _lupePos;

        /// <summary>画像サイズモード保管</summary>
        private PictureBoxSizeMode _storeSizeMode;

        #endregion

        #region プロパティ

        /// <summary>
        /// 画像情報を取得または設定します。
        /// </summary>
        protected IImage ImageData { get; set; }

        /// <summary>
        /// タグパネルが開かれているかを示すフラグを取得または設定します。
        /// </summary>
        protected bool TagPanelExpanded { get; set; }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デザイン表示用の仮コンストラクタです。
        /// </summary>
        protected ViewImageForm()
        {
            // コンポーネント初期化
            InitializeComponent();

            // フィールド初期化
            ImageMode = ConfigInfo.ImageDataMode.Database;

            // 共通コンストラクション
            ConstructCommon();
        }

        /// <summary>
        /// 継承クラス用のコンストラクタです。
        /// </summary>
        /// <param name="imageMode">画像モード</param>
        protected ViewImageForm(ConfigInfo.ImageDataMode imageMode)
            : base(imageMode)
        {
            // コンポーネント初期化
            InitializeComponent();

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

        #region ConstructCommon - 共通コンストラクション

        /// <summary>
        /// インスタンス構築時の共通処理を実行します。
        /// </summary>
        private void ConstructCommon()
        {
            // 部分拡大倍率コンボボックス初期化
            for (int i = 2; i <= 5; i++)
            {
                cmbLupeMagnification.Items.Add(new KeyValuePair<string, int>(string.Format("x{0}", i), i));
            }
            cmbLupeMagnification.DisplayMember = "Key";
            cmbLupeMagnification.SelectedIndex = 0;
            cmbLupeMagnification.Enabled = false;

            // サイズモードコンボボックス初期化
            foreach (var item in GetSizeModeMenuItems())
            {
                cmbPicMode.Items.Add(item.Text);
            }

            // サイズモード復元
            cmbPicMode.SelectedItem = Utilities.State.SizeMode.ToString();
        }

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
            else
            {
                // データ
                var menuFile = new ToolStripMenuItem("データ(&D)", null, null, "menuData");
                menuFile.ShortcutKeys = Keys.Alt | Keys.D;
                menuFile.DropDownItems.Add(new ToolStripMenuItem("戻る", null, btnClose_Click));
                menuFile.DropDownItems.Add(new ToolStripMenuItem("上書き保存", null, (s, e) => SaveImage()));
                menuFile.DropDownItems.Add(new ToolStripMenuItem("ファイルとしてエクスポート", null, menuExportAsFile_Click));
                menuFile.DropDownItems.Add(new ToolStripSeparator());
                menuFile.DropDownItems.Add(new ToolStripMenuItem("表示画像カテゴリー変更", null, menuChangeCategory_Click));
                menuFile.DropDownItems.Add(new ToolStripMenuItem("表示画像削除", null, btnDelete_Click));
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
                    lblStatus.Text = string.Format("画像{0}：{1}    サイズ：{2}×{3}    更新日時：{4}",
                        ImageMode == ConfigInfo.ImageDataMode.File ? "パス" : "ID",
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
            if (ImageData == null)
            {
                btnDelete.Enabled = false;
            }
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
                if (picViewer.Image != null)
                {
                    picViewer.Image.Dispose();
                }

                // ズーム中フラグ、倍率初期化
                _zoomed = false;
                _magnify = 100;

                // イメージ設定、サイズ再設定
                picViewer.Image = ImageData.GetImage();
                ResizeImageRect();

                // スクロール設定
                ResetScrollProperties();

                // タグを表示
                ShowTags(false);
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());

                // エラー対象画像削除確認ダイアログ表示
                if (ImageMode == ConfigInfo.ImageDataMode.File
                    && DialogResult.Yes == FormUtilities.ShowMessage("I010"))
                {
                    // エラー画像削除
                    btnDelete_Click(this, new EventArgs());
                }
            }
        }

        #endregion

        #region ShowTags - タグ表示

        /// <summary>
        /// 表示中の画像のタグを表示します。
        /// </summary>
        /// <param name="isFocusNewTagText">新規タグ追加テキストボックスにフォーカスをセットするかを示すフラグ</param>
        private void ShowTags(bool isFocusNewTagText)
        {
            // 前の画像のタグ表示を消去
            pnlTags.Controls.Clear();

            int imageId = int.Parse(ImageData.Key);
            using (var entities = new PictManagerEntities())
            {
                // 画像のタグ付けを取得
                var taggings = from row in entities.TblTaggings
                               where row.ImageId == imageId
                               select row;

                foreach (var tagging in taggings)
                {
                    // タグユニットをパネルに追加
                    var tagUnit = new TagUnit(imageId, tagging.TagId);
                    tagUnit.TagDeleted += (sender, e) => ShowTags(false);

                    pnlTags.Controls.Add(tagUnit);
                }
            }

            if (pnlTags.Controls.Count < Constants.TAGGING_MAX_COUNT)
            {
                // タグ付けが最大数に満たない場合、タグ付け追加用のテキストボックスを配置
                var txtNewTag = new TextBox();
                txtNewTag.Name = TEXT_BOX_NAME_NEW_TAG;
                txtNewTag.MaxLength = Constants.TAG_NAME_MAX_LENGTH;

                txtNewTag.KeyDown += txtNewTag_KeyDown;

                pnlTags.Controls.Add(txtNewTag);

                if (isFocusNewTagText)
                {
                    txtNewTag.Focus();
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

                //// Fill時のサイズを取得
                //picViewer.Dock = DockStyle.Fill;
                //Size s = picViewer.Size;

                //// Fillを解除し、取得しておいたサイズに変更
                //// (AutoSizeではみ出す場合には再度サイズが自動拡張される)
                //picViewer.Dock = DockStyle.None;
                //picViewer.Size = s;

                // スクロール設定
                ResetScrollProperties();

                // メニューのチェック状態を更新
                var menuItem = FormUtilities.GetMenuItem<ToolStripMenuItem>(barMenu.Items, "menuView/menuCmbSizeMode");
                if (menuItem != null)
                {
                    foreach (ToolStripMenuItem item in menuItem.DropDownItems)
                    {
                        item.Checked = item.Text == cmbPicMode.SelectedItem.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
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
                if (chkLupe.Checked)
                {
                    Utilities.State.SizeMode = _storeSizeMode;
                }
                else
                {
                    Utilities.State.SizeMode = picViewer.SizeMode;
                }
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
                if (magnify <= 0
                    || picViewer.Image.Width * magnify / 100 > int.MaxValue
                    || picViewer.Image.Height * magnify / 100 > int.MaxValue)
                {
                    return false;
                }

                // SizeModeを自動伸張に設定
                cmbPicMode.SelectedItem = PictureBoxSizeMode.StretchImage.ToString();

                // 画像表示領域伸縮
                if (picViewer.Image.Width * magnify / 100 <= int.MaxValue && magnify > 0)
                {
                    picViewer.Width = picViewer.Image.Width * magnify / 100;
                }
                if (picViewer.Image.Height * magnify / 100 <= int.MaxValue && magnify > 0)
                {
                    picViewer.Height = picViewer.Image.Height * magnify / 100;
                }
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
                    picViewer.Image.Save(ImageData.Key);
                }
                else
                {
                    using (var entities = new PictManagerEntities())
                    {
                        int imageId = int.Parse(ImageData.Key);

                        var image = (from i in entities.TblImages
                                     where i.ImageId == imageId
                                     select i).First();

                        var converter = new ImageConverter();
                        image.ImageData = converter.ConvertTo(picViewer.Image, typeof(byte[])) as byte[];
                        image.UpdatedDateTime = DateTime.Now;

                        entities.SaveChanges();
                    }
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
                var bmp = new Bitmap(ImageData.GetImage());
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
            // リソース破棄
            timLupe.Stop();
            timLupe.Enabled = false;

            // 状態情報をセーブ
            SaveStateInfo();

            // 自フォームを破棄し親フォームを表示
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
            if (pnlContent.VerticalScroll.Visible)
            {
                pnlContent.AutoScrollPosition = new Point(0, pnlContent.VerticalScroll.Minimum);
                pnlContent.VerticalScroll.SmallChange = picViewer.Size.Height / 20;
                pnlContent.VerticalScroll.LargeChange = picViewer.Size.Height / 4;
            }

            // 水平スクロールバー設定
            if (pnlContent.HorizontalScroll.Visible)
            {
                pnlContent.AutoScrollPosition = new Point(pnlContent.HorizontalScroll.Minimum, 0);
                pnlContent.HorizontalScroll.SmallChange = picViewer.Size.Width / 20;
                pnlContent.HorizontalScroll.LargeChange = picViewer.Size.Width / 4;
            }
        }

        #endregion

        #region ChangeLupeModification - 部分拡大倍率変更

        /// <summary>
        /// 部分拡大倍率をコンボボックスで選択されている倍率に変更します。
        /// </summary>
        private void ChangeLupeModification()
        {
            var selectedMagni = (KeyValuePair<string, int>)cmbLupeMagnification.SelectedItem;
            int lupeSize = 100 * selectedMagni.Value;

            picLupe.Size = new Size(lupeSize, lupeSize);
        }

        #endregion

        #region UpdateLupeView - 部分拡大表示更新

        /// <summary>
        /// 指定された位置を中心として、部分拡大表示を更新します。
        /// </summary>
        /// <param name="pos">拡大位置</param>
        private void UpdateLupeView(Point pos)
        {
            _lupePos = pos;
            picLupe.Location = new Point(_lupePos.X - picLupe.Size.Width / 2, _lupePos.Y - picLupe.Size.Height / 2);

            using (var g = picLupe.CreateGraphics())
            {
                g.Clear(picLupe.BackColor);
                g.DrawImage(
                    picViewer.Image,
                    new Rectangle(new Point(0, 0), picLupe.Size),
                    new Rectangle(
                        new Point(_lupePos.X - 50, _lupePos.Y - 50),
                        new Size(100, 100)),
                    GraphicsUnit.Pixel);
            }
        }

        #endregion

        //*** イベントハンドラ ***

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
            // ステータスバーに画像情報を表示
            ShowImageInfoByStatusBar();

            // スクロール幅設定
            pnlContent.VerticalScroll.SmallChange = SCROLL_CHANGE_SMALL;
            pnlContent.HorizontalScroll.SmallChange = SCROLL_CHANGE_SMALL;
            pnlContent.VerticalScroll.LargeChange = SCROLL_CHANGE_LARGE;
            pnlContent.HorizontalScroll.LargeChange = SCROLL_CHANGE_LARGE;

            // タグパネル設定
            if (ImageMode == ConfigInfo.ImageDataMode.Database)
            {
                pnlHeader.Height = TAG_PANEL_MIN_HEIGHT;
                TagPanelExpanded = false;
            }
            else
            {
                pnlHeader.Hide();
            }

            if (ImageData != null)
            {
                // 画像データが有る場合は表示
                DisplayImage();
            }
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
            if (!_zoomed)
            {
                ResizeImageRect();
            }
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
            if (!_zoomed)
            {
                ResizeImageRect();
            }
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
                if (e.Delta == 0)
                {
                    return;
                }

                if (pnlContent.VerticalScroll.Visible)
                {
                    int delta = e.Delta / Constants.WHEEL_DELTA * pnlContent.VerticalScroll.SmallChange * -1;
                    if (delta < 0)
                    {
                        if (-pnlContent.AutoScrollPosition.Y + delta < pnlContent.VerticalScroll.Minimum)
                        {
                            pnlContent.AutoScrollPosition = new Point(0, pnlContent.VerticalScroll.Minimum);
                            return;
                        }
                    }
                    else
                    {
                        if (-pnlContent.AutoScrollPosition.Y + delta > pnlContent.VerticalScroll.Maximum)
                        {
                            pnlContent.AutoScrollPosition =
                                new Point(0, pnlContent.VerticalScroll.Maximum);
                            return;
                        }
                    }

                    pnlContent.AutoScrollPosition = new Point(0, -pnlContent.AutoScrollPosition.Y + delta);
                }
                else if (pnlContent.HorizontalScroll.Visible)
                {
                    int delta = e.Delta / Constants.WHEEL_DELTA * pnlContent.HorizontalScroll.SmallChange * -1;
                    if (delta < 0)
                    {
                        if (-pnlContent.AutoScrollPosition.X + delta < pnlContent.HorizontalScroll.Minimum)
                        {
                            pnlContent.AutoScrollPosition = new Point(pnlContent.HorizontalScroll.Minimum, 0);
                            return;
                        }
                    }
                    else
                    {
                        if (-pnlContent.AutoScrollPosition.X + delta < pnlContent.HorizontalScroll.Maximum)
                        {
                            pnlContent.AutoScrollPosition =
                                new Point(pnlContent.HorizontalScroll.Maximum, 0);
                            return;
                        }
                    }

                    pnlContent.AutoScrollPosition = new Point(-pnlContent.AutoScrollPosition.X + delta, 0);
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
                if ((e.KeyCode & Keys.Alt) != Keys.Alt
                    && (e.KeyCode & Keys.Control) != Keys.Control
                    && (e.KeyCode & Keys.Shift) != Keys.Shift)
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
            try
            {
                if (FormUtilities.ShowMessage("Q002") == DialogResult.No)
                {
                    return;
                }

                // PictureBoxリソース解放
                picViewer.Image.Dispose();

                // 表示中の画像を削除
                ImageData.Delete();

                CloseForm();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkLupe_CheckedChanged - ルーペ表示チェックボックス変更時

        /// <summary>
        /// ルーペ表示チェックボックスが変更された際に実行される処理です。
        /// 部分拡大鏡を表示します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkLupe_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkLupe.Checked)
                {
                    // 部分拡大モード開始時の各種値を保存
                    _lupePos = Cursor.Position;
                    _storeSizeMode = picViewer.SizeMode;

                    // コントロール値更新
                    picViewer.SizeMode = PictureBoxSizeMode.Normal;
                    cmbPicMode.Enabled = false;
                    cmbLupeMagnification.Enabled = true;

                    // 部分拡大倍率設定
                    ChangeLupeModification();

                    // 部分拡大窓表示
                    picLupe.Location = new Point(_lupePos.X - picLupe.Size.Width / 2, _lupePos.Y - picLupe.Size.Height / 2);
                    picLupe.Visible = true;
                    picLupe.BringToFront();

                    // 部分拡大タイマー開始
                    timLupe.Enabled = true;
                    timLupe.Start();
                }
                else
                {
                    // 部分拡大窓消去
                    cmbPicMode.Enabled = true;
                    cmbLupeMagnification.Enabled = false;
                    picViewer.SizeMode = _storeSizeMode;

                    picLupe.Visible = false;

                    // 部分拡大タイマー終了
                    timLupe.Stop();
                    timLupe.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region timLupe_Tick - 部分拡大表示タイマー時間経過時

        /// <summary>
        /// 部分拡大表示タイマーの時間経過時に実行される処理です。
        /// 部分拡大表示位置をマウスカーソルに追随させます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void timLupe_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_lupePos != Cursor.Position)
                {
                    UpdateLupeView(Cursor.Position);
                }
            }
            catch (Exception ex)
            {
                timLupe.Stop();
                timLupe.Enabled = false;

                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region cmbLupeMagnification_SelectedIndexChanged - 部分拡大倍率コンボボックス選択変更時

        /// <summary>
        /// 部分拡大倍率コンボボックスの選択変更時に実行される処理です。
        /// 部分拡大倍率を変更します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void cmbLupeMagnification_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (chkLupe.Checked)
                {
                    ChangeLupeModification();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
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
            if (ZoomImage(_magnify + ZOOM_UNIT))
            {
                _magnify += ZOOM_UNIT;
            }
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
            if (ZoomImage(_magnify - ZOOM_UNIT))
            {
                _magnify -= ZOOM_UNIT;
            }
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

        #region btnTagPanelToggle_Click - タグパネル切替ボタン押下時

        /// <summary>
        /// タグパネル切替ボタンがクリックされた際に実行される処理です。
        /// タグパネルの表示高さを切り替えます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnTagPanelToggle_Click(object sender, EventArgs e)
        {
            try
            {
                if (TagPanelExpanded)
                {
                    pnlHeader.Height = TAG_PANEL_MIN_HEIGHT;
                    btnTagPanelToggle.Image = Resources.arrow_down;
                }
                else
                {
                    pnlHeader.Height = TAG_PANEL_MIN_HEIGHT * 2;
                    btnTagPanelToggle.Image = Resources.arrow_up;
                }

                TagPanelExpanded = !TagPanelExpanded;
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region txtNewTag_KeyDown - 新規タグテキストボックスキー押下時

        /// <summary>
        /// フォーム上でキーが押下された際に実行される処理です。
        /// 特殊なキーが押下された場合に固有の処理を実行します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void txtNewTag_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.KeyCode & Keys.Return) == Keys.Return)   // リターンキー押下時
                {
                    try
                    {
                        string tagName = (sender as TextBox).Text.Trim();
                        if (string.IsNullOrEmpty(tagName))
                        {
                            return;
                        }

                        int imageId = int.Parse(ImageData.Key);
                        int tagNo = pnlTags.Controls.OfType<TagUnit>().Count() + 1;

                        using (var entities = new PictManagerEntities())
                        {
                            // 同じ名称のタグを取得
                            var tag = (from row in entities.MstTags
                                        where row.TagName == tagName
                                        select row).SingleOrDefault();

                            if (tag == null)
                            {
                                // タグが存在しない場合、新規登録
                                tag = new MstTag();
                                tag.TagName = tagName;
                                tag.InsertedDateTime = DateTime.Now;
                                tag.UpdatedDateTime = DateTime.Now;

                                entities.MstTags.Add(tag);
                                entities.SaveChanges();
                            }
                            else
                            {
                                // タグが存在する場合、既にそのタグが付けられているか確認
                                var query = from row in entities.TblTaggings
                                            where row.TagId == tag.TagId
                                                && row.ImageId == imageId
                                            select row;

                                if (query.Any())
                                {
                                    // 既に付けられている場合は何もしない
                                    return;
                                }
                            }

                            // タグ付けを作成
                            var tagging = new TblTagging();
                            tagging.TagId = tag.TagId;
                            tagging.ImageId = imageId;
                            tagging.InsertedDateTime = DateTime.Now;
                            tagging.UpdatedDateTime = DateTime.Now;

                            entities.TblTaggings.Add(tagging);
                            entities.SaveChanges();
                        }

                        // タグ表示を更新
                        ShowTags(true);
                    }
                    finally
                    {
                        e.Handled = true;
                        e.SuppressKeyPress = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
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
            if (RenameFile() != ResultStatus.OK)
            {
                return;
            }

            // ステータスバー更新
            lblStatus.Text = ImageData.Key;
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
            if (MoveFile() != ResultStatus.OK)
            {
                return;
            }

            // 前画面へ戻る
            this.BackToOwner();
        }

        #endregion

        #region menuChangeCategory_Click - 表示画像カテゴリー変更メニュー押下時

        /// <summary>
        /// 表示画像カテゴリー変更メニューがクリックされた際に実行される処理です。
        /// 表示中の画像を指定されたカテゴリーに変更し、前画面へ戻ります。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void menuChangeCategory_Click(object sender, EventArgs e)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.Database);

            // カテゴリー変更
            if (ChangeCategory() != ResultStatus.OK)
            {
                return;
            }

            // 前画面へ戻る
            this.BackToOwner();
        }

        #endregion

        #region menuExportAsFile_Click - ファイルとしてエクスポートメニュー押下時

        /// <summary>
        /// ファイルとしてエクスポートメニューがクリックされた際に実行される処理です。
        /// 表示中の画像を指定されたカテゴリーに変更し、前画面へ戻ります。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected virtual void menuExportAsFile_Click(object sender, EventArgs e)
        {
            Debug.Assert(ImageMode == ConfigInfo.ImageDataMode.Database);

            // 画像ファイルエクスポート
            ExportImageFile();
        }

        #endregion
    }
}
