using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

using FileSortOrderMap = System.Collections.Generic.Dictionary<SO.PictManager.Common.FileSortOrder, string>;

namespace SO.PictManager.Common
{
    #region enum FileSortOrder - ファイルソート順列挙体

    /// <summary>
    /// ファイルソート順列挙体
    /// </summary>
    public enum FileSortOrder
    {
        /// <summary>ファイル名の昇順</summary>
        FileNameAsc,
        /// <summary>ファイル名の降順</summary>
        FileNameDesc,
        /// <summary>タイムスタンプの昇順</summary>
        TimestampAsc,
        /// <summary>タイムスタンプの降順</summary>
        TimestampDesc,
        /// <summary>ファイルサイズの昇順</summary>
        FileSizeAsc,
        /// <summary>ファイルサイズの降順</summary>
        FileSizeDesc,
        /// <summary>ランダム表示</summary>
        Random,
    }

    #endregion

    /// <summary>
    /// ファイル名ソート機能提供クラス
    /// </summary>
    public static class FileSorter
    {
        #region メンバ変数

        /// <summary>ソート順コンボアイテムマッピング</summary>
        private static readonly FileSortOrderMap _sortOrderMap = new FileSortOrderMap()
        {
            { FileSortOrder.FileNameAsc,   "ファイル名の昇順" },
            { FileSortOrder.FileNameDesc,  "ファイル名の降順" },
            { FileSortOrder.TimestampAsc,  "タイムスタンプの昇順" },
            { FileSortOrder.TimestampDesc, "タイムスタンプの降順" },
            { FileSortOrder.FileSizeAsc,   "ファイルサイズの昇順" },
            { FileSortOrder.FileSizeDesc,  "ファイルサイズの降順" },
            { FileSortOrder.Random,        "ランダム表示" },
        };

        #endregion

        #region Sort - ソート実施

        /// <summary>
        /// 渡された文字列群を、指定されたソート順でソートして返却します。
        /// </summary>
        /// <param name="source">ソート対象の文字列群</param>
        /// <param name="order">ソート順</param>
        /// <returns>ソートされた文字列群</returns>
        public static IEnumerable<string> Sort(IEnumerable<string> source, FileSortOrder order)
        {
            switch (order)
            {
                case FileSortOrder.FileNameAsc:
                    return source.OrderBy(p => Path.GetFileName(p));
                case FileSortOrder.FileNameDesc:
                    return source.OrderByDescending(p => Path.GetFileName(p));
                case FileSortOrder.TimestampAsc:
                    return source.OrderBy(p => File.GetLastWriteTime(p));
                case FileSortOrder.TimestampDesc:
                    return source.OrderByDescending(p => File.GetLastWriteTime(p));
                case FileSortOrder.FileSizeAsc:
                    return source.OrderBy(p => new FileInfo(p).Length);
                case FileSortOrder.FileSizeDesc:
                    return source.OrderByDescending(p => new FileInfo(p).Length);
                case FileSortOrder.Random:
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
        /// <param orderName="cmb">バインド先のコンボボックス</param>
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
        /// <param orderName="order">FileSortOrderの列挙値</param>
        /// <returns>FileSortOrderの表示用名称</returns>
        public static string GetSortOrderDisplayText(FileSortOrder order)
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
            FileSortOrder order = GetSortOrderByName(orderName);
            return GetSortOrderDisplayText(order);
        }

        #endregion

        #region GetSortOrderByName - FileSortOrderの定義名称を指定して列挙値を取得

        /// <summary>
        /// FileSortOrderの定義名称を指定して列挙値を取得します。
        /// </summary>
        /// <param name="orderName">FileSortOrderの定義名称</param>
        /// <returns>FileSortOrderの列挙値</returns>
        public static FileSortOrder GetSortOrderByName(string orderName)
        {
            FieldInfo field = typeof(FileSortOrder).GetField(orderName,
                BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return (FileSortOrder)field.GetValue(null);
        }

        #endregion

        #region GetSortOrderByDisplayText - FileSortOrderの表示用文字列を指定して列挙値を取得

        /// <summary>
        /// FileSortOrderの表示用文字列を指定して列挙値を取得します。
        /// </summary>
        /// <param orderName="displayText">FileSortOrderの表示用文字列</param>
        /// <returns>FileSortOrderの列挙値</returns>
        /// <exception cref="System.ArgumentException">
        /// 指定された表示用文字列に一致するFileSortOrderが存在しない場合
        /// </exception>
        public static FileSortOrder GetSortOrderByDisplayText(string displayText)
        {
            if (!_sortOrderMap.ContainsValue(displayText))
                throw new ArgumentException("無効なソート順名です。");

            return _sortOrderMap.Where(o => o.Value == displayText).Single().Key;
        }

        #endregion
    }
}
