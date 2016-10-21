using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.PictManager.Common;
using SO.PictManager.DataModel;

namespace SO.PictManager.Components
{
    /// <summary>
    /// タグユニットクラス
    /// </summary>
    public partial class TagUnit : UserControl
    {
        #region インスタンス変数

        /// <summary>タグ削除時のイベントハンドラ</summary>
        private EventHandler _tagDeletedHandler;

        #endregion

        #region プロパティ

        /// <summary>
        /// 画像IDを取得します。
        /// </summary>
        public int ImageId { get; private set; }

        /// <summary>
        /// タグIDを取得します。
        /// </summary>
        public int TagId { get; private set; }

        /// <summary>
        /// タグ名を取得します。
        /// </summary>
        public string TagName
        {
            get { return lnkTagName.Text; }
        }

        #endregion

        #region イベント

        /// <summary>
        /// タグ名リンクがクリックされた際に発行されるイベントです。
        /// </summary>
        public event LinkLabelLinkClickedEventHandler TagNameLinkClicked
        {
            add { lnkTagName.LinkClicked += value; }
            remove { lnkTagName.LinkClicked -= value; }
        }

        /// <summary>
        /// タグ削除時に発行されるイベントです。
        /// </summary>
        public event EventHandler TagDeleted
        {
            add { _tagDeletedHandler += value; }
            remove { _tagDeletedHandler -= value; }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 既定のコンストラクタです。
        /// </summary>
        /// <param name="imageId">画像ID</param>
        /// <param name="tagId">タグID</param>
        public TagUnit(int imageId, int tagId)
        {
            InitializeComponent();

            ImageId = imageId;
            TagId = tagId;

            // タグ名を取得
            using (var entities = new PictManagerEntities())
            {
                lnkTagName.Text = (from row in entities.MstTags
                                   where row.TagId == TagId
                                   select row.TagName).Single();
            }
        }

        #endregion

        //*** イベントハンドラ ***

        #region btnDelete_Click - 削除ボタン押下時

        /// <summary>
        /// 削除ボタンがクリックされた際に実行される処理です。
        /// 画像からタグを除去します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (var entities = new PictManagerEntities())
                {
                    // タグ付けを解除
                    var tag = (from row in entities.TblTaggings
                               where row.TagId == this.TagId
                                  && row.ImageId == this.ImageId
                               select row).Single();

                    entities.TblTaggings.Remove(tag);
                    entities.SaveChanges();

                    // タグ削除時のアクションを呼出
                    if (_tagDeletedHandler != null)
                    {
                        _tagDeletedHandler(sender, e);
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
