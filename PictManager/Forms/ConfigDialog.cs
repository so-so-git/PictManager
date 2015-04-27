using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using SO.PictManager.Common;
using SO.PictManager.Forms.Info;

namespace SO.PictManager.Forms
{
    /// <summary>
    /// システム設定ダイアログクラス
    /// </summary>
    public partial class ConfigDialog : BaseDialog
    {
        #region コンストラクタ

        /// <summary>
        /// 規定のコンストラクタです。
        /// </summary>
        public ConfigDialog()
        {
            InitializeComponent();

            ConfigInfo config = Utilities.Config;

            // 現在の共通情報の内容を画面にセット
            rdoModeFile.Checked = config.CommonInfo.Mode == ConfigInfo.ImageDataMode.File;
            rdoModeDatabase.Checked = !rdoModeFile.Checked;

            var formatMap = new Dictionary<string, CheckBox>();
            foreach (var format in grpFileFormat.Controls.OfType<CheckBox>())
            {
                string extension = format.Tag.ToString();
                if (extension.IndexOf(',') > -1)
                    foreach (var splitExt in extension.Split(new[] { ',' }))
                    {
                        formatMap[splitExt] = format;
                    }
                else
                    formatMap[extension] = format;
            }
            foreach (var target in config.CommonInfo.TargetExtensions)
                formatMap[target].Checked = true;

            chkIncludeSub.Checked = config.CommonInfo.IsIncludeSubDirectory;
            chkConfirmQuit.Checked = config.CommonInfo.IsConfirmQuit;
        }

        #endregion

        #region SaveConfigInfo - 入力したシステム設定情報を保存

        /// <summary>
        /// 入力したシステム設定情報を保存します。
        /// </summary>
        private void SaveConfigInfo()
        {
            //*** 共通情報をセット ***
            ConfigInfo.CommonConfig commonConf = Utilities.Config.CommonInfo;
            commonConf.Mode = rdoModeFile.Checked ? ConfigInfo.ImageDataMode.File : ConfigInfo.ImageDataMode.Database;
            commonConf.TargetExtensions.Clear();
            foreach (var format in grpFileFormat.Controls.OfType<CheckBox>())
            {
                if (format.Checked)
                {
                    string extension = format.Tag.ToString();
                    if (extension.IndexOf(',') > -1)
                        commonConf.TargetExtensions.AddRange(extension.Split(new[] { ',' }));
                    else
                        commonConf.TargetExtensions.Add(extension);
                }
            }
            commonConf.IsIncludeSubDirectory = chkIncludeSub.Checked;
            commonConf.IsConfirmQuit = chkConfirmQuit.Checked;

            //*** スライドフォーム情報をセット ***
            ConfigInfo.SlideConfig slideConf = Utilities.Config.SlideInfo;
            slideConf.IsBookmarkTopMost = chkBookmarkTopMost.Checked;

            // システム設定情報をファイルに保存
            Utilities.SaveConfigInfo();
        }

        #endregion

        #region btnApply_Click - 適用ボタン押下時

        /// <summary>
        /// 適用ボタンをクリックした際に実行される処理です。
        /// 入力したシステム設定情報を保存します。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveConfigInfo();
        }

        #endregion

        #region btnOk_Click - OKボタン押下時

        /// <summary>
        /// (BaseDialog.btnOk_Clickをオーバーライドします)
        /// OKボタンをクリックした際に実行される処理です。
        /// 入力したシステム設定情報を保存し、画面を閉じます。
        /// </summary>
        /// <param name="sender">イベント発生元オブジェクト</param>
        /// <param name="e">イベント引数</param>
        protected override void btnOk_Click(object sender, EventArgs e)
        {
            SaveConfigInfo();
            this.DialogResult = DialogResult.OK;
        }

        #endregion
    }
}
