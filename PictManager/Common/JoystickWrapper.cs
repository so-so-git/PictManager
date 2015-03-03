using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Microsoft.DirectX.DirectInput;

using Action = System.Action;

namespace SO.PictManager.Common
{
    #region Ps2ControllerButtons - PlayStation2用コントローラのボタンインデックス列挙体
    /// <summary>
    /// PlayStation2用コントローラのボタンインデックス列挙体
    /// </summary>
    public enum Ps2ControllerButtons
    {
        /// <summary>×ボタン</summary>
        Cross = 0,
        /// <summary>○ボタン</summary>
        Circle,
        /// <summary>□ボタン</summary>
        Square,
        /// <summary>△ボタン</summary>
        Triangle,
        /// <summary>L1ボタン</summary>
        L1,
        /// <summary>L2ボタン</summary>
        L2,
        /// <summary>R1ボタン</summary>
        R1,
        /// <summary>R2ボタン</summary>
        R2,
        /// <summary>スタートボタン</summary>
        Select,
        /// <summary>セレクトボタン</summary>
        Start,
        /// <summary>左スティック押し込みボタン</summary>
        LeftStick,
        /// <summary>右スティック押し込みボタン</summary>
        RightStick,
        /// <summary>↑ボタン</summary>
        Up,
        /// <summary>→ボタン</summary>
        Right,
        /// <summary>↓ボタン</summary>
        Down,
        /// <summary>←ボタン</summary>
        Left,
    }
    #endregion

    /// <summary>
    /// ジョイスティック入力のラッパークラス
    /// </summary>
    public class JoystickWrapper : IDisposable
    {
        #region インスタンス変数

        /// <summary>デバイスのインスタンス</summary>
        private Device _device;
        /// <summary>キーのポーリングを行うスレッド</summary>
        private Thread _poling;

        /// <summary>ボタンを押した瞬間のアクションのリスト</summary>
        private List<Action> _buttonDownActions;
        /// <summary>ボタンを放した瞬間のアクションのリスト</summary>
        private List<Action> _buttonUpActions;
        /// <summary>ボタンを押し続けている間のアクションのリスト</summary>
        private List<Action> _buttonPressActions;

        /// <summary>直近のポーリング時に左キーが押されていたかのフラグ</summary>
        private bool _isPressedLeftKey;
        /// <summary>直近のポーリング時に右キーが押されていたかのフラグ</summary>
        private bool _isPressedRightKey;
        /// <summary>直近のポーリング時に上キーが押されていたかのフラグ</summary>
        private bool _isPressedUpKey;
        /// <summary>直近のポーリング時に下キーが押されていたかのフラグ</summary>
        private bool _isPressedDownKey;

        /// <summary>直近のポーリング時に各ボタンが押されていたかのフラグの配列</summary>
        private bool[] _isPressedButtons;

        #endregion

        #region イベント

        /// <summary>左キーを押した瞬間に発生するイベント</summary>
        public event Action LeftKeyDown;
        /// <summary>右キーを押した瞬間に発生するイベント</summary>
        public event Action RightKeyDown;
        /// <summary>上キーを押した瞬間に発生するイベント</summary>
        public event Action UpKeyDown;
        /// <summary>下キーを押した瞬間に発生するイベント</summary>
        public event Action DownKeyDown;

        /// <summary>左キーを放した瞬間に発生するイベント</summary>
        public event Action LeftKeyUp;
        /// <summary>右キーを放した瞬間に発生するイベント</summary>
        public event Action RightKeyUp;
        /// <summary>上キーを放した瞬間に発生するイベント</summary>
        public event Action UpKeyUp;
        /// <summary>下キーを放した瞬間に発生するイベント</summary>
        public event Action DownKeyUp;

        /// <summary>左キーを押し続けている間に発生するイベント</summary>
        public event Action LeftKeyPress;
        /// <summary>右キーを押し続けている間に発生するイベント</summary>
        public event Action RightKeyPress;
        /// <summary>上キーを押し続けている間に発生するイベント</summary>
        public event Action UpKeyPress;
        /// <summary>下キーを押し続けている間に発生するイベント</summary>
        public event Action DownKeyPress;

        #endregion

        #region プロパティ

        /// <summary>
        /// ジョイスティックが使用可能かどうかを取得します。
        /// </summary>
        public bool Enabled { get; private set; }

        #endregion

        #region コンストラクタ
        /// <summary>
        /// 唯一のコンストラクタです。
        /// </summary>
        /// <param name="parent">親コントロール</param>
        public JoystickWrapper(Control parent)
        {
            // 接続されているデバイスを検索
            DeviceList devList = Manager.GetDevices(DeviceClass.GameControl,
                EnumDevicesFlags.AttachedOnly);
            if (devList.Count < 1) return;

            // 最初のコントローラを取得し、初期設定
            _device = new Device(devList.Cast<DeviceInstance>().First().InstanceGuid);
            _device.SetCooperativeLevel(parent,
                CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            _device.SetDataFormat(DeviceDataFormat.Joystick);

            // アクセス権取得
            _device.Acquire();

            // 各種イベント初期化
            LeftKeyDown = CreateEmptyAction();
            RightKeyDown = CreateEmptyAction();
            UpKeyDown = CreateEmptyAction();
            DownKeyDown = CreateEmptyAction();

            LeftKeyUp = CreateEmptyAction();
            RightKeyUp = CreateEmptyAction();
            UpKeyUp = CreateEmptyAction();
            DownKeyUp = CreateEmptyAction();

            LeftKeyPress = CreateEmptyAction();
            RightKeyPress = CreateEmptyAction();
            UpKeyPress = CreateEmptyAction();
            DownKeyPress = CreateEmptyAction();

            _buttonDownActions = new List<Action>(_device.Caps.NumberButtons);
            _buttonUpActions = new List<Action>(_device.Caps.NumberButtons);
            _buttonPressActions = new List<Action>(_device.Caps.NumberButtons);
            _isPressedButtons = new bool[_device.Caps.NumberButtons];
            for (int i = 0; i < _device.Caps.NumberButtons; ++i)
            {
                _buttonDownActions.Add(CreateEmptyAction());
                _buttonUpActions.Add(CreateEmptyAction());
                _buttonPressActions.Add(CreateEmptyAction());
            }

            // キーポーリング開始
            _poling = new Thread(new ThreadStart(KeyPolling));
            _poling.Start();

            // 使用準備完了
            Enabled = true;
        }
        #endregion

        #region KeyPolling - キーポーリング・アクション実行
        /// <summary>
        /// キーの状態をポーリングで取得し、対応するアクションを行います。
        /// </summary>
        private void KeyPolling()
        {
            while (true)
            {
                // キーの状態を取得
                _device.Poll();
                JoystickState state = _device.CurrentJoystickState;

                // 上下キー
                if (state.X == 0)
                {
                    if (_isPressedLeftKey)
                    {
                        LeftKeyPress();
                    }
                    else
                    {
                        LeftKeyDown();
                        _isPressedLeftKey = true;
                    }

                    if (_isPressedRightKey)
                        RightKeyUp();

                    _isPressedRightKey = false;
                }
                else if (state.X == 65535)
                {
                    if (_isPressedRightKey)
                    {
                        RightKeyPress();
                    }
                    else
                    {
                        RightKeyDown();
                        _isPressedRightKey = true;
                    }

                    if (_isPressedLeftKey)
                        LeftKeyUp();

                    _isPressedLeftKey = false;
                }
                else
                {

                    if (_isPressedRightKey)
                        RightKeyUp();

                    if (_isPressedLeftKey)
                        LeftKeyUp();

                    _isPressedLeftKey = false;
                    _isPressedRightKey = false;
                }

                // 左右キー
                if (state.Y == 0)
                {
                    if (_isPressedDownKey)
                    {
                        DownKeyPress();
                    }
                    else
                    {
                        DownKeyDown();
                        _isPressedDownKey = true;
                    }

                    if (_isPressedUpKey)
                        UpKeyUp();

                    _isPressedUpKey = false;
                }
                else if (state.Y == 65535)
                {
                    if (_isPressedUpKey)
                    {
                        UpKeyPress();
                    }
                    else
                    {
                        UpKeyDown();
                        _isPressedUpKey = true;
                    }

                    if (_isPressedDownKey)
                        DownKeyUp();

                    _isPressedDownKey = false;
                }
                else
                {

                    if (_isPressedUpKey)
                        UpKeyUp();

                    if (_isPressedDownKey)
                        DownKeyUp();

                    _isPressedDownKey = false;
                    _isPressedUpKey = false;
                }

                // それ以外のボタン
                byte[] buttons = state.GetButtons();
                for (int i = 0; i < _device.Caps.NumberButtons; ++i)
                {
                    if (buttons[i] == 128)
                    {
                        if (_isPressedButtons[i])
                            _buttonPressActions[i]();
                        else
                        {
                            _buttonDownActions[i]();
                            _isPressedButtons[i] = true;
                            Console.WriteLine("down [" + i + "] key.");
                        }
                    }
                    else
                    {
                        if (_isPressedButtons[i])
                            _buttonUpActions[i]();

                        _isPressedButtons[i] = false;
                    }
                }

                // 60fps周期でループ
                Thread.Sleep(1000 / 60);
            }
        }
        #endregion

        #region AddButtonDownAction - ボタンを押した瞬間のアクションを追加
        /// <summary>
        /// ボタンを押した瞬間のアクションを追加します。
        /// </summary>
        /// <param name="act">アクション</param>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void AddButtonDownAction(Action act, int buttonIndex)
        {
            _buttonDownActions[buttonIndex] += act;
        }
        #endregion

        #region RemoveButtonDownAction - ボタンを押した瞬間のアクションを除去
        /// <summary>
        /// ボタンを押した瞬間のアクションを除去します。
        /// </summary>
        /// <param name="act">アクション</param>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void RemoveButtonDownAction(Action act, int buttonIndex)
        {
            _buttonDownActions[buttonIndex] -= act;
        }
        #endregion

        #region ClearButtonDownAction - ボタンを押した瞬間のアクションを全消去
        /// <summary>
        /// ボタンを押した瞬間のアクションを全て消去します。
        /// </summary>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void ClearButtonDownAction(int buttonIndex)
        {
            _buttonDownActions[buttonIndex] = CreateEmptyAction();
        }
        #endregion

        #region AddButtonDownAction - ボタンを放した瞬間のアクションを追加
        /// <summary>
        /// ボタンを放した瞬間のアクションを追加します。
        /// </summary>
        /// <param name="act">アクション</param>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void AddButtonUpAction(Action act, int buttonIndex)
        {
            _buttonUpActions[buttonIndex] += act;
        }
        #endregion

        #region RemoveButtonDownAction - ボタンを放した瞬間のアクションを除去
        /// <summary>
        /// ボタンを放した瞬間のアクションを除去します。
        /// </summary>
        /// <param name="act">アクション</param>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void RemoveButtonUpAction(Action act, int buttonIndex)
        {
            _buttonUpActions[buttonIndex] -= act;
        }
        #endregion

        #region ClearButtonDownAction - ボタンを放した瞬間のアクションを全消去
        /// <summary>
        /// ボタンを放した瞬間のアクションを全て消去します。
        /// </summary>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void ClearButtonUpAction(int buttonIndex)
        {
            _buttonUpActions[buttonIndex] = CreateEmptyAction();
        }
        #endregion

        #region AddButtonDownAction - ボタンを押し続けている間のアクションを追加
        /// <summary>
        /// ボタンを押し続けている間のアクションを追加します。
        /// </summary>
        /// <param name="act">アクション</param>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void AddButtonPressAction(Action act, int buttonIndex)
        {
            _buttonPressActions[buttonIndex] += act;
        }
        #endregion

        #region RemoveButtonDownAction - ボタンを押し続けている間のアクションを除去
        /// <summary>
        /// ボタンを押し続けている間のアクションを除去します。
        /// </summary>
        /// <param name="act">アクション</param>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void RemoveButtonPressAction(Action act, int buttonIndex)
        {
            _buttonPressActions[buttonIndex] -= act;
        }
        #endregion

        #region ClearButtonDownAction - ボタンを押し続けている間のアクションを全消去
        /// <summary>
        /// ボタンを押し続けている間のアクションを全て消去します。
        /// </summary>
        /// <param name="buttonIndex">ボタンのインデックス</param>
        public void ClearButtonPressAction(int buttonIndex)
        {
            _buttonPressActions[buttonIndex] = CreateEmptyAction();
        }
        #endregion

        #region Dispose - リソース破棄
        /// <summary>
        /// リソースを破棄します。
        /// </summary>
        public void Dispose()
        {
            _poling.Abort();
            _device.Dispose();
            _device = null;
        }
        #endregion

        #region CreateEmptyAction - 空のアクションを生成
        /// <summary>
        /// 空のアクションを生成します。
        /// </summary>
        /// <returns></returns>
        private Action CreateEmptyAction()
        {
            return new Action(() => { });
        }
        #endregion
    }
}
