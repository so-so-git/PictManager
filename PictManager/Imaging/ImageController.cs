using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using SO.Library.Drawing;
using SO.Library.Forms;
using SO.PictManager.Forms;

namespace SO.PictManager.Imaging
{
    /// <summary>
    /// 共通画像処理クラス
    /// </summary>
    internal class ImageController
    {
        #region クラス定数

        /// <summary>比較時の水平方向の分割数</summary>
        private const int DIVIDE_HORISONTAL = 5;

        /// <summary>比較時の垂直方向の分割数</summary>
        private const int DIVIDE_VERTICAL = 5;

        /// <summary>比較時閾値</summary>
        private const int THRESHOLD = 16;

        #endregion

        #region GetSimilarImages - 類似画像リスト取得

        /// <summary>
        /// 指定されたフォームが持つ表示対象画像リスト内から、
        /// 基準となる画像と類似した画像を検索し、そのリストを取得します。
        /// </summary>
        /// <param name="form">フォーム</param>
        /// <param name="criterion">基準となる画像</param>
        /// <returns>類似画像のリスト</returns>
        internal static List<IImage> GetSimilarImages(BaseForm form, IImage criterion)
        {
            return GetSimilarImages(form, criterion, null);
        }

        /// <summary>
        /// 指定されたフォームが持つ表示対象画像リスト内から、
        /// 基準となる画像と類似した画像を検索し、そのリストを取得します。
        /// (プログレスダイアログに追加表示するメッセージを指定します)
        /// </summary>
        /// <param name="form">フォーム</param>
        /// <param name="criterion">基準となる画像</param>
        /// <param name="message">プログレスダイアログに追加表示するメッセージ</param>
        /// <returns>類似画像のリスト</returns>
        internal static List<IImage> GetSimilarImages(BaseForm form, IImage criterion, string message)
        {
            try
            {
                bool useGray = false;

                var similarImages = new List<IImage>();

                // マウスカーソル変更(待機)
                Cursor.Current = Cursors.WaitCursor;

                // プログレスダイアログ表示
                using (var progDlg = new ProgressDialog(form))
                {
                    progDlg.StartProgress("類似画像検索中...", string.Empty, 0, form.ImageCount);
                    form.Update();

                    int blockWidth;
                    int blockHeight;
                    int xPos;
                    int yPos = 0;
                    int[,] viewAvgs = new int[DIVIDE_HORISONTAL, DIVIDE_VERTICAL];
                    using (Image img = criterion.GetImage())
                    using (Bitmap colorBmp = new Bitmap(img))
                    using (Bitmap bmp = useGray ? ImageUtilities.ToGrayScale(colorBmp, GrayScaleMethod.NTSC) : colorBmp)
                    {
                        // 基準画像の各ブロックのピクセル深度の平均を取得
                        blockWidth = bmp.Width / DIVIDE_HORISONTAL;
                        blockHeight = bmp.Height / DIVIDE_VERTICAL;
                        for (int y = 1; y <= DIVIDE_VERTICAL; y++)
                        {
                            xPos = 0;
                            for (int x = 1; x <= DIVIDE_HORISONTAL; x++)
                            {
                                Rectangle rect = new Rectangle(
                                    xPos,
                                    yPos,
                                    x == DIVIDE_HORISONTAL ? blockWidth + bmp.Width % DIVIDE_HORISONTAL : blockWidth,
                                    y == DIVIDE_VERTICAL ? blockHeight + bmp.Height % DIVIDE_VERTICAL : blockHeight);

                                viewAvgs[x - 1, y - 1] = GetBlockColorAverage(bmp, rect);

                                xPos += blockWidth;
                            }
                            yPos += blockHeight;
                        }
                    }

                    int proceedCount = 0;
                    var similarList = new List<string>();
                    foreach (var compImage in form.ImageList)
                    {
                        // プログレスメッセージ更新
                        progDlg.Message = string.Format("{0}({1}/{2}) {3}",
                            message ?? string.Empty, ++proceedCount, form.ImageCount, compImage);

                        if (compImage.Key == criterion.Key)
                        {
                            continue;
                        }

                        using (Image img = compImage.GetImage())
                        using (Bitmap colorBmp = new Bitmap(img))
                        using (Bitmap bmp = useGray ? ImageUtilities.ToGrayScale(colorBmp, GrayScaleMethod.NTSC) : colorBmp)
                        {
                            // 比較先の画像の各ブロックのピクセル深度の平均を取得
                            blockWidth = bmp.Width / DIVIDE_HORISONTAL;
                            blockHeight = bmp.Height / DIVIDE_VERTICAL;
                            yPos = 0;
                            for (int y = 1; y <= DIVIDE_VERTICAL; y++)
                            {
                                xPos = 0;
                                for (int x = 1; x <= DIVIDE_HORISONTAL; x++)
                                {
                                    Rectangle rect = new Rectangle(
                                        xPos,
                                        yPos,
                                        x == DIVIDE_HORISONTAL ? blockWidth + bmp.Width % DIVIDE_HORISONTAL : blockWidth,
                                        y == DIVIDE_VERTICAL ? blockHeight + bmp.Height % DIVIDE_VERTICAL : blockHeight);

                                    int viewAvg = viewAvgs[x - 1, y - 1];
                                    int compAvg = GetBlockColorAverage(bmp, rect);
                                    int blockDist = Math.Abs(viewAvg - compAvg);

                                    if (blockDist > THRESHOLD)
                                    {
                                        goto NextImage;
                                    }

                                    xPos += blockWidth;
                                }
                                yPos += blockHeight;
                            }
                        }

                        similarImages.Add(compImage);

                    NextImage:
                        // プログレスバー更新
                        progDlg.PerformStep();
                    }
                }

                return similarImages;
            }
            finally
            {
                // マウスカーソル変更(通常)
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region GetBlockColorAverage - 指定ブロックのピクセル色値の平均値を取得

        /// <summary>
        /// 対象画像内の指定された矩形内のピクセル色値の平均値を取得します。
        /// </summary>
        /// <param name="bmp">対象画像</param>
        /// <param name="rect">対象画像内の処理対象矩形</param>
        /// <returns>指定されたブロックのピクセル色値の平均値</returns>
        private static int GetBlockColorAverage(Bitmap bmp, Rectangle rect)
        {
            BitmapData bmpData = null;
            try
            {
                // 指定矩形内のデータに対するポインタを取得
                bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

                // 指定矩形内のピクセルデータを取得
                var pixels = new byte[rect.Width * rect.Height * 4];
                Marshal.Copy(bmpData.Scan0, pixels, 0, pixels.Length);

                long total = 0;
                for (int i = 0; i < pixels.Length; i += 4)
                {
                    // ピクセルの各色のバイト値を合計に加算(4バイト目のAlphaは無視)
                    total += pixels[i];     // B
                    total += pixels[i + 2]; // G
                    total += pixels[i + 3]; // R
                }

                // 全ピクセルの平均を返却
                return (int)Math.Ceiling(total / (double)(rect.Width * rect.Height));
            }
            finally
            {
                if (bmpData != null)
                {
                    bmp.UnlockBits(bmpData);
                }
            }
        }

        #endregion
    }
}
