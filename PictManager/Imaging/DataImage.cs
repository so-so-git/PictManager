using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

using SO.PictManager.Common;
using SO.PictManager.DataModel;

namespace SO.PictManager.Imaging
{
    /// <summary>
    /// 画像データクラス
    /// </summary>
    public class DataImage : IImage
    {
        #region インスタンス変数

        /// <summary>画像データ概要</summary>
        private VImageOverview _overview;

        #endregion

        #region プロパティ

        /// <summary>
        /// 画像キー(画像ID)を取得または設定します。
        /// </summary>
        public string Key
        {
            get { return _overview.ImageId.ToString(); }
            set { GetImageOverview(int.Parse(value)); }
        }

        /// <summary>
        /// 画像のタイムスタンプ(更新日時)を取得または設定します。
        /// </summary>
        public DateTime Timestamp
        {
            get { return _overview.UpdatedDateTime; }
            set { _overview.UpdatedDateTime = value; }
        }

        /// <summary>
        /// 画像のデータサイズを取得します。
        /// </summary>
        public long DataSize
        {
            get { return _overview.DataSize; }
        }

        /// <summary>
        /// 画像が論理削除されているかのフラグを取得または設定します。
        /// </summary>
        public bool IsDeleted
        {
            get { return _overview.DeleteFlag; }
            set { _overview.DeleteFlag = value; }
        }

        /// <summary>
        /// カテゴリーIDを取得または設定します。
        /// </summary>
        public int CategoryId
        {
            get { return _overview.CategoryId; }
            set { _overview.CategoryId = value; }
        }

        /// <summary>
        /// 画像の説明を取得または設定します。
        /// </summary>
        public string Description
        {
            get { return _overview.Description; }
            set { _overview.Description = value; }
        }

        /// <summary>
        /// 画像グループのIDを取得または設定します。
        /// </summary>
        public int? GroupId
        {
            get { return _overview.GroupId; }
            set { _overview.GroupId = value; }
        }

        /// <summary>
        /// 画像グループの表示順を取得または設定します。
        /// </summary>
        public int? GroupOrder
        {
            get { return _overview.GroupOrder; }
            set { _overview.GroupOrder = value; }
        }

        /// <summary>
        /// 画像のバイトデータを取得します。
        /// </summary>
        public byte[] ImageBytes
        {
            get
            {
                using (var entities = new PictManagerEntities())
                {
                    return (from i in entities.TblImages
                            where i.ImageId == _overview.ImageId
                            select i.ImageData).First();
                }
            }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        /// <param name="imageId">画像ID</param>
        public DataImage(int imageId)
        {
            GetImageOverview(imageId);
        }

        #endregion

        #region GetImage - 画像オブジェクトを取得

        /// <summary>
        /// 画像オブジェクトを取得します。
        /// </summary>
        /// <returns>画像オブジェクト</returns>
        public Image GetImage()
        {
            using (var entities = new PictManagerEntities())
            {
                byte[] imageData = (from i in entities.TblImages
                                    where i.ImageId == _overview.ImageId
                                    select i.ImageData).First();

                var converter = new ImageConverter();
                var img = converter.ConvertFrom(imageData) as Image;

                return img;
            }
        }

        #endregion

        #region Delete - 画像データ論理削除

        /// <summary>
        /// 画像データを論理削除します。
        /// </summary>
        public void Delete()
        {
            using (var entities = new PictManagerEntities())
            {
                // 画像データを論理削除
                var image = (from i in entities.TblImages
                             where i.ImageId == _overview.ImageId
                             select i).First();

                image.DeleteFlag = true;
                image.UpdatedDateTime = DateTime.Now;

                entities.SaveChanges();

                GetImageOverview(_overview.ImageId);
            }

            // ログ出力
            Utilities.Logger.WriteLog(GetType().FullName, MethodBase.GetCurrentMethod().Name,
                "[DELETE] 画像ID: " + _overview.ImageId.ToString());
        }

        #endregion

        #region GetTblImage - 画像テーブルデータ取得

        /// <summary>
        /// 指定されたIDの画像データの概要をビューから取得します。
        /// </summary>
        /// <param name="imageId">画像ID</param>
        private void GetImageOverview(int imageId)
        {
            using (var entities = new PictManagerEntities())
            {
                _overview = (from overview in entities.VImageOverviews
                             where overview.ImageId == imageId
                             select overview
                            ).First();
            }
        }

        #endregion
    }
}
