using System;
using System.Drawing;

namespace SO.PictManager.Imaging
{
    /// <summary>
    /// 画像インタフェース
    /// </summary>
    public interface IImage
    {
        /// <summary>
        /// 画像キーを取得または設定します。
        /// </summary>
        string Key { get; set; }

        /// <summary>
        /// 画像のタイムスタンプを取得します。
        /// </summary>
        DateTime Timestamp { get; }

        /// <summary>
        /// 画像のデータサイズを取得します。
        /// </summary>
        long DataSize { get; }

        /// <summary>
        /// 画像が削除されているかのフラグを取得または設定します。
        /// </summary>
        bool IsDeleted { get; set; }

        /// <summary>
        /// 画像オブジェクトを取得します。
        /// </summary>
        /// <returns>画像オブジェクト</returns>
        Image GetImage();

        /// <summary>
        /// 画像を削除します。
        /// </summary>
        void Delete();
    }
}
