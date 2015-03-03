using System;
using System.Windows.Forms;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// 基底ダイアログクラス
    /// </summary>
    public partial class BaseDialog : Form
    {
        #region コンストラクタ
        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public BaseDialog()
        {
            // コンポーネント初期化
            InitializeComponent();
        }
        #endregion

        #region btnOk_Click - OKボタン押下時
        /// <summary>
        /// OKボタン押下時の処理です。
        /// 入力チェックを実施し、OKなら呼出元へ制御を返します。
        /// </summary>
        /// <param orderName="sender">イベント発生元オブジェクト</param>
        /// <param orderName="e">イベント引数</param>
        protected virtual void btnOk_Click(object sender, EventArgs e)
        {
            // 入力チェック、相関チェックOKなら呼出元へOKを返す
            if (IsValidInput()) this.DialogResult = DialogResult.OK;
        }
        #endregion

        #region IsValidInput - 入力チェック、相関チェック
        /// <summary>
        /// 入力チェックを実施します。
        /// 継承先ダイアログで固有の入力チェックを行なう場合は本メソッドをオーバーライドして下さい。
        /// オーバーライド前は必ずtrueを返します。
        /// </summary>
        /// <returns>チェックOK時:true、チェックNG時:false</returns>
        protected virtual bool IsValidInput() { return true; }
        #endregion

        #region Show - モーダルダイアログとして表示
        /// <summary>
        /// (Form.Show()を隠蔽します)
        /// ダイアログをモーダル状態で表示します。
        /// </summary>
        /// <returns>ダイアログ処理結果</returns>
        public new virtual DialogResult Show()
        {
            // 必ずモーダルダイアログとして表示
            return ShowDialog();
        }

        /// <summary>
        /// (Form.Show(IWin32Window)を隠蔽します)
        /// オーナーウィンドウを指定し、ダイアログをモーダル状態で表示します。
        /// </summary>
        /// <param orderName="owner">オーナーウィンドウ</param>
        /// <returns>ダイアログ処理結果</returns>
        public new virtual DialogResult Show(IWin32Window owner)
        {
            // 必ずモーダルダイアログとして表示
            return ShowDialog(owner);
        }
        #endregion
    }
}
