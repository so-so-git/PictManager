using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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

        /// <summary>
        /// 画像テーブルデータ
        /// ※メモリ圧迫回避の為、画像のバイナリデータは保持していません。
        /// </summary>
        private TblImage _image;

        /// <summary>画像のデータサイズ</summary>
        private long _dataSize;

        #endregion

        #region プロパティ

        /// <summary>
        /// 画像キー(画像ID)を取得・設定します。
        /// </summary>
        public string Key
        {
            get { return _image.ImageId.ToString(); }
            set { GetTblImage(int.Parse(value)); }
        }

        /// <summary>
        /// 画像のタイムスタンプ(登録日時)を取得します。
        /// </summary>
        public DateTime Timestamp
        {
            get { return _image.InsertedDateTime; }
        }

        /// <summary>
        /// 画像のデータサイズを取得します。
        /// </summary>
        public long DataSize
        {
            get { return _dataSize; }
        }

        /// <summary>
        /// 画像が論理削除されているかのフラグを取得・設定します。
        /// </summary>
        public bool IsDeleted
        {
            get { return _image.DeleteFlag; }
            set { _image.DeleteFlag = value; }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        /// <param name="imageId">画像ID</param>
        public DataImage(int imageId)
        {
            GetTblImage(imageId);
        }

        #endregion

        #region GetImage - 画像オブジェクトを取得

        /// <summary>
        /// 画像オブジェクトを取得します。
        /// </summary>
        /// <returns>画像オブジェクト</returns>
        public Image GetImage()
        {
            using (var entity = new PictManagerEntities())
            {
                byte[] imageData = (from i in entity.TblImages
                                    where i.ImageId == _image.ImageId
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
            using (var entity = new PictManagerEntities())
            {
                var image = (from i in entity.TblImages
                             where i.ImageId == _image.ImageId
                             select i).First();

                image.DeleteFlag = true;

                entity.SaveChanges();
            }

            // ログ出力
            Utilities.Logger.WriteLog(GetType().FullName, MethodBase.GetCurrentMethod().Name,
                "[DELETE] 画像ID: " + _image.ImageId.ToString());
        }

        #endregion

        #region GetTblImage - 画像テーブルデータ取得

        /// <summary>
        /// 指定されたIDの画像データを画像テーブルから取得します。
        /// 但し、画像のバイナリデータは、画像データサイズのみ取得しクリアされます。
        /// </summary>
        /// <param name="imageId">画像ID</param>
        private void GetTblImage(int imageId)
        {
            using (var entity = new PictManagerEntities())
            {
                var image = (from i in entity.TblImages
                             where i.ImageId == imageId
                             select i).First();

                entity.Entry<TblImage>(image).State = EntityState.Detached;

                _image = image;

                // データサイズを取得し、バイナリデータを削除
                _dataSize = _image.ImageData.LongLength;
                _image.ImageData = null;
            }
        }

        #endregion
    }
}
