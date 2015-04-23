using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SO.PictManager.Imaging
{
    /// <summary>
    /// 画像ファイルクラス
    /// </summary>
    public class FileImage : IImage
    {
        #region インスタンス変数

        /// <summary>画像ファイル情報</summary>
        private FileInfo _fileInfo;

        /// <summary>削除フラグ</summary>
        private bool _isDeleted = false;

        #endregion

        #region プロパティ

        /// <summary>
        /// 画像キーを取得・設定します。
        /// </summary>
        public string Key
        {
            get { return _fileInfo.FullName; }
            set
            {
                _fileInfo = new FileInfo(value);
                _isDeleted = false;
            }
        }

        /// <summary>
        /// 画像のタイムスタンプを取得します。
        /// </summary>
        public DateTime Timestamp
        {
            get { return _fileInfo.LastWriteTime; }
        }

        /// <summary>
        /// 画像のデータサイズを取得します。
        /// </summary>
        public long DataSize
        {
            get { return _fileInfo.Length; }
        }

        /// <summary>
        /// 画像が削除されているかのフラグを取得・設定します。
        /// </summary>
        public bool IsDeleted
        {
            get { return _isDeleted; }
            set { _isDeleted = value; }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        /// <param name="filePath">画像ファイルパス</param>
        public FileImage(string filePath)
        {
            _fileInfo = new FileInfo(filePath);
            _isDeleted = false;
        }

        #endregion

        #region GetImage - 画像オブジェクトを取得 

        /// <summary>
        /// 画像オブジェクトを取得します。
        /// </summary>
        /// <returns>画像オブジェクト</returns>
        public Image GetImage()
        {
            return Image.FromFile(Key);
        }

        #endregion

        #region Delete - 画像ファイルを削除

        /// <summary>
        /// 画像を削除します。
        /// </summary>
        public void Delete()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
