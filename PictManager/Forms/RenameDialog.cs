using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

using SO.Library.Extensions;
using SO.Library.Forms;
using SO.Library.Text;
using SO.PictManager.Common;
using SO.PictManager.Forms.Info;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// ファイル名変更情報入力ダイアログクラス
    /// </summary>
    public sealed partial class RenameDialog : BaseDialog
    {
        #region メンバ変数

        /// <summary>アクセス制御変更中フラグ</summary>
        private bool _isAccessibleChanging;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// デフォルトのコンストラクタです。
        /// </summary>
        public RenameDialog()
        {
            // コンポーネント初期化
            InitializeComponent();

            // 共通初期化処理
            CommonConstruction(null);
        }

        /// <summary>
        /// 渡されたリネーム情報を設定して画面を構築するコンストラクタです。
        /// </summary>
        /// <param name="renameInfo">画面に設定するリネーム情報</param>
        public RenameDialog(RenameInfo renameInfo)
        {
            // コンポーネント初期化
            InitializeComponent();

            // 共通初期化処理
            CommonConstruction(renameInfo);
        }

        /// <summary>
        /// 各コンストラクタ共通の初期化処理を行います。
        /// </summary>
        /// <param name="renameInfo">画面に設定するリネーム情報</param>
        private void CommonConstruction(RenameInfo renameInfo)
        {
            // ソート順コンボボックス構築
            ImageSorter.BindSortOrderDataSource(cmbSort);

            // リネーム情報設定
            if (renameInfo != null)
            {
                SetRenameInfo(renameInfo);
            }

            // コントロールアクセス制御
            ChangeAccessibles();
        }

        #endregion

        #region IsValidInput - 入力チェック、相関チェック

        /// <summary>
        /// (BaseDialog.IsValidInput()をオーバーライドします)
        /// ダイアログに入力された内容の妥当性及び相関チェックを行ないます。
        /// </summary>
        /// <returns>チェックOK時:true / チェックNG時:false</returns>
        protected override bool IsValidInput()
        {
            // 元ファイル名保持チェック、通し番号付加チェック入力チェック
            if (!chkOriginal.Checked && !chkAddSeq.Checked)
            {
                chkOriginal.Focus();
                FormUtilities.ShowMessage("W011");
                return false;
            }

            // 元ファイル名を含むがチェックされている場合
            if (chkOriginal.Checked)
            {
                // 置換前文字入力チェック
                if (!string.IsNullOrEmpty(txtRepBefore.Text)
                    && !string.IsNullOrEmpty(txtRepAfter.Text))
                {
                    txtRepBefore.Focus();
                    FormUtilities.ShowMessage("W012");
                    return false;
                }
            }

            // 通し番号を付加するがチェックされている場合
            if (chkAddSeq.Checked)
            {
                // 通し番号間隔が入力チェック
                if (!string.IsNullOrEmpty(nudStep.Text))
                {
                    nudStep.Focus();
                    FormUtilities.ShowMessage("W013");
                    return false;
                }
            }

            // 禁止文字チェック
            var pathCheckList = new[]
            {
                new { TextBox = txtDirDelimiter, ItemName = "親ディレクトリ名区切り文字" },
                new { TextBox = txtSeqDelimiter, ItemName = "通し番号区切り文字" },
                new { TextBox = txtPrefix      , ItemName = "接頭文字" },
                new { TextBox = txtSuffix      , ItemName = "接尾文字" },
                new { TextBox = txtRepBefore   , ItemName = "置換前文字" },
                new { TextBox = txtRepAfter    , ItemName = "置換後文字" },
            };

            foreach (var chkTarget in pathCheckList)
            {
                if (!string.IsNullOrEmpty(chkTarget.TextBox.Text)
                    && chkTarget.TextBox.Text.HasInvalidPathChar())
                {
                    chkTarget.TextBox.Focus();
                    chkTarget.TextBox.SelectAll();
                    FormUtilities.ShowMessage("W015", chkTarget.ItemName);
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region SetRenameInfo - リネーム情報設定

        /// <summary>
        /// ファイル名変更情報をダイアログに設定します。
        /// </summary>
        /// <param name="renameInfo">ファイル名変更情報</param>
        public void SetRenameInfo(RenameInfo renameInfo)
        {
            chkAddDirName.Checked = renameInfo.IsAddParentDirName;
            txtDirDelimiter.Text = renameInfo.DirDelimiter ?? string.Empty;
            chkOriginal.Checked = renameInfo.IsReserveOriginalName;
            chkAddSeq.Checked = renameInfo.IsAddSequential;
            chkShuffle.Checked = renameInfo.IsShuffle;
            chkSort.Checked = renameInfo.SortOrder.HasValue;
            nudStep.Value = renameInfo.IncrementStep ?? nudStep.Minimum;
            renameInfo.SeqDelimiter = chkAddSeq.Checked && chkOriginal.Checked ? txtSeqDelimiter.Text : null;
            txtPrefix.Text = renameInfo.Prefix;
            txtSuffix.Text = renameInfo.Suffix;
            txtRepBefore.Text = renameInfo.ReplaceBefore ?? string.Empty;
            txtRepBefore.Text = renameInfo.ReplaceAfter ?? string.Empty;
            rdoBefore.Checked = renameInfo.OriginalPosition == OriginalPosition.Before;
            rdoAfter.Checked = renameInfo.OriginalPosition == OriginalPosition.After;
            if (renameInfo.IsShuffle || !renameInfo.SortOrder.HasValue)
            {
                cmbSort.SelectedValue = ImageSortOrder.KeyAsc;
            }
            else
            {
                cmbSort.SelectedValue = renameInfo.SortOrder.Value;
            }
        }

        #endregion

        #region GetRenameInfo - リネーム情報取得

        /// <summary>
        /// ダイアログに入力されたファイル名変更情報を取得します。
        /// </summary>
        /// <returns>ファイル名変更情報</returns>
        public RenameInfo GetRenameInfo()
        {
            // リネーム情報セット
            var renameInfo = new RenameInfo();
            renameInfo.IsAddParentDirName = chkAddDirName.Checked;
            renameInfo.DirDelimiter = chkAddDirName.Checked ? txtDirDelimiter.Text : null;
            renameInfo.IsReserveOriginalName = chkOriginal.Checked;
            renameInfo.IsAddSequential = chkAddSeq.Checked;
            renameInfo.IsShuffle = chkAddSeq.Checked ? chkShuffle.Checked : false;
            renameInfo.IncrementStep = chkAddSeq.Checked ? new Nullable<uint>(decimal.ToUInt32(nudStep.Value)) : null;
            renameInfo.SeqDelimiter = chkAddSeq.Checked && chkOriginal.Checked ? txtSeqDelimiter.Text : null;
            renameInfo.Prefix = txtPrefix.Text;
            renameInfo.Suffix = txtSuffix.Text;
            renameInfo.ReplaceBefore = chkOriginal.Checked ? txtRepBefore.Text : null;
            renameInfo.ReplaceAfter = chkOriginal.Checked && !string.IsNullOrEmpty(txtRepBefore.Text)
                ? txtRepAfter.Text : null;
            renameInfo.OriginalPosition = rdoBefore.Checked ? OriginalPosition.Before : OriginalPosition.After;
            if (renameInfo.IsShuffle || !chkSort.Checked)
            {
                renameInfo.SortOrder = null;
            }
            else
            {
                renameInfo.SortOrder = (ImageSortOrder)cmbSort.SelectedValue;
            }

            return renameInfo;
        }

        #endregion

        #region GenerateSample - サンプルファイル名表示内容生成

        /// <summary>
        /// ダイアログの入力内容を基にサンプルファイル名表示内容を生成します。
        /// </summary>
        private void GenerateSample()
        {
            var sb = new StringBuilder();

            // 元ファイル名を含める
            OriginalPosition? orgPos = null;
            string seqDelimiter = string.Empty;
            if (chkOriginal.Checked)
            {
                sb.Append("Image");
                orgPos = rdoBefore.Checked ? OriginalPosition.Before : OriginalPosition.After;
                seqDelimiter = txtSeqDelimiter.Text;
            }

            // 置換文字
            if (!string.IsNullOrEmpty(txtRepBefore.Text))
            {
                sb = new StringBuilder(sb.ToString().Replace(txtRepBefore.Text, txtRepAfter.Text));
            }

            // 通し番号付加
            if (chkAddSeq.Checked)
            {
                if (orgPos.HasValue && orgPos.Value == OriginalPosition.Before)
                {
                    sb.Append(seqDelimiter + "001");
                }
                else
                {
                    sb.Insert(0, "001" + seqDelimiter);
                }
            }

            // 接頭文字、接尾文字
            sb.Insert(0, txtPrefix.Text);
            sb.Append(txtSuffix.Text);

            // 親ディレクトリ名付加
            if (chkAddDirName.Checked)
            {
                sb.Insert(0, "parent" + txtDirDelimiter.Text);
            }

            sb.Insert(0, @"parent\");
            sb.Append(".jpg");

            lblSampleAfter.Text = sb.ToString();
        }

        #endregion

        #region ChangeAccessibles - コントロールアクセス制御

        /// <summary>
        /// 画面のコントロール状態に応じて、関連するコントロールのアクセス状態を制御します。
        /// </summary>
        private void ChangeAccessibles()
        {
            if (_isAccessibleChanging)
            {
                return;
            }

            try
            {
                _isAccessibleChanging = true;

                // シャッフルチェックボックス関連
                if (chkShuffle.Checked)
                {
                    chkSort.Enabled = false;
                    cmbSort.Enabled = false;
                }
                else
                {
                    // ソート順チェックボックス関連
                    chkSort.Enabled = true;
                    cmbSort.Enabled = chkSort.Checked;
                }

                // 親ディレクトリ名区切り文字
                bool isAddDirName = chkAddDirName.Checked;
                lblDirDelimiter.Enabled = isAddDirName;
                txtDirDelimiter.Enabled = isAddDirName;

                // 通し番号付与設定関連
                bool isAddSeq = chkAddSeq.Checked;
                lblStep.Enabled = isAddSeq;
                nudStep.Enabled = isAddSeq;

                // 通し番号区切り文字、元ファイル名位置
                bool isKeepOriginal = chkOriginal.Checked;
                bool isAddSeqAndOriginal = isAddSeq & isKeepOriginal;
                lblSeqDelimiter.Enabled = isAddSeqAndOriginal;
                txtSeqDelimiter.Enabled = isAddSeqAndOriginal;
                rdoBefore.Enabled = isAddSeqAndOriginal;
                rdoAfter.Enabled = isAddSeqAndOriginal;

                // 置換前後文字列
                if (isKeepOriginal)
                {
                    txtRepBefore.Enabled = true;

                    bool isInputRepBefore = !string.IsNullOrEmpty(txtRepBefore.Text);
                    lblArrow1.Enabled = isInputRepBefore;
                    txtRepAfter.Enabled = isInputRepBefore;
                }
                else
                {
                    txtRepBefore.Enabled = false;
                    lblArrow1.Enabled = false;
                    txtRepAfter.Enabled = false;
                }
            }
            finally
            {
                _isAccessibleChanging = false;
            }
        }

        #endregion

        //*** イベントハンドラ ***

        #region btnClear_Click - クリアボタン押下時

        /// <summary>
        /// クリアボタンがクリックされた際に実行される処理です。
        /// ダイアログに入力された内容を全て消去します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                cmbSort.SelectedIndex = 0;
                txtDirDelimiter.Text = string.Empty;
                txtPrefix.Text = string.Empty;
                txtSuffix.Text = string.Empty;
                txtRepBefore.Text = string.Empty;
                txtRepAfter.Text = string.Empty;
                nudStep.Value = nudStep.Minimum;
                txtSeqDelimiter.Text = string.Empty;
                chkSort.Checked = false;
                chkAddDirName.Checked = false;
                chkShuffle.Checked = false;
                chkOriginal.Checked = false; 
                chkAddSeq.Checked = false;
                rdoBefore.Checked = true;
                lblSampleAfter.Text = @"parent\.jpg";
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkShuffle_CheckedChanged - 順番シャッフルチェックボックスチェック変更時

        /// <summary>
        /// 順番シャッフルチェックボックスが変更された際に実行される処理です。
        /// チェックボックスの状態に応じて入力可能項目の変更を行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkShuffle_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAccessibles();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkSort_CheckedChanged - ソート順チェックボックスチェック変更時

        /// <summary>
        /// ソート順チェックボックスが変更された際に実行される処理です。
        /// チェックボックスの状態に応じて入力可能項目の変更を行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkSort_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAccessibles();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkAddDirName_CheckedChanged - 親ディレクトリ名挿入チェック変更時

        /// <summary>
        /// 親ディレクトリ名変更チェックボックスが変更された際に実行される処理です。
        /// チェックボックスの状態に応じて入力可能項目の変更を行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkAddDirName_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAccessibles();

                // サンプル更新
                GenerateSample();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkOriginal_CheckedChanged - 元ファイル名を含むチェック変更時

        /// <summary>
        /// 元ファイル名を含むチェックボックスが変更された際に実行される処理です。
        /// チェックボックスの状態に応じて入力可能項目の変更を行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkOriginal_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAccessibles();

                // サンプル更新
                GenerateSample();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region chkAddSeq_CheckedChanged - 通し番号付加チェック変更時

        /// <summary>
        /// 通し番号付加チェックボックスが変更された際に実行される処理です。
        /// チェックボックスの状態に応じて入力可能項目の変更を行います。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void chkAddSeq_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ChangeAccessibles();

                // サンプル更新
                GenerateSample();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region TextBoxes_TextChanged - TextBox入力内容変更時

        /// <summary>
        /// 各TextBoxの入力内容が変更された際に実行される処理です。
        /// 入力された内容に基づいてサンプルファイル名表示内容を更新します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void TextBoxes_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (sender == txtRepBefore)
                {
                    ChangeAccessibles();
                }

                GenerateSample();
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion

        #region OriginalPosRadio_CheckedChanged - 元ファイル名位置指定ラジオボタンチェック変更時

        /// <summary>
        /// 元ファイル名位置指定ラジオボタンのチェックが変更された際の処理です。
        /// チェックされた内容に基づいてサンプルファイル名表示内容を更新します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void OriginalPosRadio_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as RadioButton).Checked)
                {
                    GenerateSample();
                }
            }
            catch (Exception ex)
            {
                ex.DoDefault(GetType().FullName, MethodBase.GetCurrentMethod());
            }
        }

        #endregion
    }

    #region enum OriginalPosition - 元ファイル名位置指定列挙体

    /// <summary>
    /// 元ファイル名位置指定列挙体
    /// </summary>
    public enum OriginalPosition
    {
        /// <summary>元ファイル名を新ファイル名の前に配置</summary>
        Before,
        /// <summary>元ファイル名を新ファイル名の後に配置</summary>
        After,
    }

    #endregion
}
