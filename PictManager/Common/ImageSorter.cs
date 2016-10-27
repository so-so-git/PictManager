using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using SO.PictManager.Forms.Info;
using SO.PictManager.Imaging;

using SortOrderMap = System.Collections.Generic.Dictionary<SO.PictManager.Common.ImageSortOrder, string>;

namespace SO.PictManager.Common
{
    #region enum ImageSortOrder - 画像ソート順列挙体

    /// <summary>
    /// 画像ソート順列挙体
    /// </summary>
    public enum ImageSortOrder
    {
        /// <summary>画像キーの昇順</summary>
        KeyAsc,
        /// <summary>画像キーの降順</summary>
        KeyDesc,
        /// <summary>タイムスタンプの昇順</summary>
        TimestampAsc,
        /// <summary>タイムスタンプの降順</summary>
        TimestampDesc,
        /// <summary>画像データサイズの昇順</summary>
        DataSizeAsc,
        /// <summary>画像データサイズの降順</summary>
        DataSizeDesc,
        /// <summary>ランダム表示</summary>
        Random,
    }

    #endregion

    /// <summary>
    /// 画像ソート機能提供クラス
    /// </summary>
    public static class ImageSorter
    {
        #region メンバ変数

        /// <summary>ソート順コンボアイテムマッピング</summary>
        private static readonly SortOrderMap _sortOrderMap = new SortOrderMap()
        {
            { ImageSortOrder.KeyAsc,        "画像キーの昇順" },
            { ImageSortOrder.KeyDesc,       "画像キーの降順" },
            { ImageSortOrder.TimestampAsc,  "タイムスタンプの昇順" },
            { ImageSortOrder.TimestampDesc, "タイムスタンプの降順" },
            { ImageSortOrder.DataSizeAsc,   "画像データサイズの昇順" },
            { ImageSortOrder.DataSizeDesc,  "画像データサイズの降順" },
            { ImageSortOrder.Random,        "ランダム表示" },
        };

        #endregion

        #region Sort - ソート実施

        /// <summary>
        /// 渡された画像データのリストを、指定されたソート順でソートして返却します。
        /// </summary>
        /// <param name="source">ソート対象の文字列群</param>
        /// <param name="order">ソート順</param>
        /// <param name="mode">画像モード</param>
        /// <returns>ソートされた画像データのリスト</returns>
        public static IEnumerable<IImage> Sort(
            IEnumerable<IImage> source, ImageSortOrder order, ConfigInfo.ImageDataMode mode)
        {
            Func<string, string> keyGetter;
            if (mode == ConfigInfo.ImageDataMode.File)
            {
                keyGetter = Path.GetFileName;
            }
            else
            {
                keyGetter = k => k;
            }

            switch (order)
            {
                case ImageSortOrder.KeyAsc:
                    return source.OrderBy(p => keyGetter(p.Key));

                case ImageSortOrder.KeyDesc:
                    return source.OrderByDescending(p => keyGetter(p.Key));

                case ImageSortOrder.TimestampAsc:
                    return source.OrderBy(p => p.Timestamp);

                case ImageSortOrder.TimestampDesc:
                    return source.OrderByDescending(p => p.Timestamp);

                case ImageSortOrder.DataSizeAsc:
                    return source.OrderBy(p => p.DataSize);

                case ImageSortOrder.DataSizeDesc:
                    return source.OrderByDescending(p => p.DataSize);

                case ImageSortOrder.Random:
                    return source.OrderBy(p => Guid.NewGuid());

                default:
                    return null;
            }
        }

        #endregion

        #region BindSortOrderDataSource - ソート順マッピングをコンボボックスにバインド

        /// <summary>
        /// ソート順マッピングをコンボボックスにバインドします。
        /// </summary>
        /// <param name="cmb">バインド先のコンボボックス</param>
        public static void BindSortOrderDataSource(ComboBox cmb)
        {
            cmb.DataSource = _sortOrderMap.ToList();
            cmb.DisplayMember = "Value";
            cmb.ValueMember = "Key";
        }

        #endregion

        #region GetSortOrderDisplayText - FileSortOrderの表示用名称を取得

        /// <summary>
        /// FileSortOrderの列挙値を指定して表示用名称を取得します。
        /// </summary>
        /// <param name="order">FileSortOrderの列挙値</param>
        /// <returns>FileSortOrderの表示用名称</returns>
        public static string GetSortOrderDisplayText(ImageSortOrder order)
        {
            return _sortOrderMap[order];
        }

        /// <summary>
        /// FileSortOrderの定義名称を指定して表示用名称を取得します。
        /// </summary>
        /// <param name="orderName">FileSortOrderの定義名称</param>
        /// <returns>FileSortOrderの表示用名称</returns>
        /// <exception cref="System.ArgumentException">
        /// 指定された表示用文字列に一致するFileSortOrderが存在しない場合
        /// </exception>
        public static string GetSortOrderDisplayText(string orderName)
        {
            ImageSortOrder order = GetSortOrderByName(orderName);
            return GetSortOrderDisplayText(order);
        }

        #endregion

        #region GetSortOrderByName - FileSortOrderの定義名称を指定して列挙値を取得

        /// <summary>
        /// FileSortOrderの定義名称を指定して列挙値を取得します。
        /// </summary>
        /// <param name="orderName">FileSortOrderの定義名称</param>
        /// <returns>FileSortOrderの列挙値</returns>
        public static ImageSortOrder GetSortOrderByName(string orderName)
        {
            FieldInfo field = typeof(ImageSortOrder).GetField(orderName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

            return (ImageSortOrder)field.GetValue(null);
        }

        #endregion

        #region GetSortOrderByDisplayText - FileSortOrderの表示用文字列を指定して列挙値を取得

        /// <summary>
        /// FileSortOrderの表示用文字列を指定して列挙値を取得します。
        /// </summary>
        /// <param name="displayText">FileSortOrderの表示用文字列</param>
        /// <returns>FileSortOrderの列挙値</returns>
        /// <exception cref="System.ArgumentException">
        /// 指定された表示用文字列に一致するFileSortOrderが存在しない場合
        /// </exception>
        public static ImageSortOrder GetSortOrderByDisplayText(string displayText)
        {
            if (!_sortOrderMap.ContainsValue(displayText))
            {
                throw new ArgumentException("無効なソート順名です。");
            }

            return _sortOrderMap.Where(o => o.Value == displayText).Single().Key;
        }

        #endregion
    }
}
