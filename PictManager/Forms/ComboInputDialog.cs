using System;

using SO.Library.Forms;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 簡易コンボボックス入力ダイアログクラス
    /// </summary>
    public sealed partial class ComboInputDialog<T> : CommonInputDialog
    {
        #region プロパティ

        /// <summary>
        /// このクラスでは未サポートのプロパティです。
        /// System.NotSupportedExceptionをスローします。
        /// </summary>
        public override string InputString
        {
            get { throw new NotSupportedException(); }
        }

        /// <summary>
        /// コンボボックスで選択されている値を取得します。
        /// </summary>
        public T SelectedValue
        {
            get { return (T)cmbSelect.SelectedValue; }
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        /// <param name="title">ダイアログタイトル</param>
        /// <param name="labelText">説明文言</param>
        /// <param name="dataSource">コンボボックスのデータソース</param>
        /// <param name="displayMember">コンボボックスの表示メンバー</param>
        /// <param name="valueMember">コンボボックスの値メンバー</param>
        public ComboInputDialog(string title, string labelText, object dataSource,
                                string displayMember = null, 
                                string valueMember = null)
            : base(title, labelText, false)
        {
            InitializeComponent();

            cmbSelect.DataSource = dataSource;
            
            if (!string.IsNullOrEmpty(displayMember))
            {
                cmbSelect.DisplayMember = displayMember;
            }
            if (!string.IsNullOrEmpty(valueMember))
            {
                cmbSelect.ValueMember = valueMember;
            }

            if (cmbSelect.Items.Count > 0)
            {
                cmbSelect.SelectedIndex = 0;
            }
        }

        #endregion
    }
}
