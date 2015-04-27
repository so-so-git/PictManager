using System;
using System.Collections;
using System.ComponentModel;
using System.Collections.Generic;
using System.Windows.Forms;

using SO.PictManager.Common;

namespace SO.PictManager.Components
{
    /// <summary>
    /// 特定キーダウン時のプリプロセス無効化機能を備えたボタンクラス
    /// (矢印キーをプリプロセスに渡さず、独自のロジックで処理するよう指定)
    /// </summary>
    public class KeyPrevButton : Button
    {
        #region IsInputKey - プリプロセス対象キー識別
        /// <summary>
        /// 押下されたキーがプリプロセス対象かを判別します。
        /// </summary>
        /// <param name="keyData">押下されたキーの情報</param>
        /// <returns>プリプロセス対象の場合:true、プリプロセス対象外の場合:false</returns>
        protected override bool IsInputKey(Keys keyData)
        {
            // 修飾キーが付加されている場合は通常処理
            if ((keyData & Keys.Alt) != Keys.Alt &&
                    (keyData & Keys.Control) != Keys.Control &&
                    (keyData & Keys.Shift) != Keys.Shift)
            {
                // "←" or "→"キー押下時のみプリプロセス無効化
                Keys kcode = keyData & Keys.KeyCode;
                if (kcode == Keys.Left || kcode == Keys.Right ||
                        kcode == Keys.Up || kcode == Keys.Down) return true;
            }

            return base.IsInputKey(keyData);
        }
        #endregion
    }
}
