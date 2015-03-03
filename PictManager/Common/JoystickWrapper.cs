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
    #region Ps2ControllerButtons - PlayStation2�p�R���g���[���̃{�^���C���f�b�N�X�񋓑�
    /// <summary>
    /// PlayStation2�p�R���g���[���̃{�^���C���f�b�N�X�񋓑�
    /// </summary>
    public enum Ps2ControllerButtons
    {
        /// <summary>�~�{�^��</summary>
        Cross = 0,
        /// <summary>���{�^��</summary>
        Circle,
        /// <summary>���{�^��</summary>
        Square,
        /// <summary>���{�^��</summary>
        Triangle,
        /// <summary>L1�{�^��</summary>
        L1,
        /// <summary>L2�{�^��</summary>
        L2,
        /// <summary>R1�{�^��</summary>
        R1,
        /// <summary>R2�{�^��</summary>
        R2,
        /// <summary>�X�^�[�g�{�^��</summary>
        Select,
        /// <summary>�Z���N�g�{�^��</summary>
        Start,
        /// <summary>���X�e�B�b�N�������݃{�^��</summary>
        LeftStick,
        /// <summary>�E�X�e�B�b�N�������݃{�^��</summary>
        RightStick,
        /// <summary>���{�^��</summary>
        Up,
        /// <summary>���{�^��</summary>
        Right,
        /// <summary>���{�^��</summary>
        Down,
        /// <summary>���{�^��</summary>
        Left,
    }
    #endregion

    /// <summary>
    /// �W���C�X�e�B�b�N���͂̃��b�p�[�N���X
    /// </summary>
    public class JoystickWrapper : IDisposable
    {
        #region �C���X�^���X�ϐ�

        /// <summary>�f�o�C�X�̃C���X�^���X</summary>
        private Device _device;
        /// <summary>�L�[�̃|�[�����O���s���X���b�h</summary>
        private Thread _poling;

        /// <summary>�{�^�����������u�Ԃ̃A�N�V�����̃��X�g</summary>
        private List<Action> _buttonDownActions;
        /// <summary>�{�^����������u�Ԃ̃A�N�V�����̃��X�g</summary>
        private List<Action> _buttonUpActions;
        /// <summary>�{�^�������������Ă���Ԃ̃A�N�V�����̃��X�g</summary>
        private List<Action> _buttonPressActions;

        /// <summary>���߂̃|�[�����O���ɍ��L�[��������Ă������̃t���O</summary>
        private bool _isPressedLeftKey;
        /// <summary>���߂̃|�[�����O���ɉE�L�[��������Ă������̃t���O</summary>
        private bool _isPressedRightKey;
        /// <summary>���߂̃|�[�����O���ɏ�L�[��������Ă������̃t���O</summary>
        private bool _isPressedUpKey;
        /// <summary>���߂̃|�[�����O���ɉ��L�[��������Ă������̃t���O</summary>
        private bool _isPressedDownKey;

        /// <summary>���߂̃|�[�����O���Ɋe�{�^����������Ă������̃t���O�̔z��</summary>
        private bool[] _isPressedButtons;

        #endregion

        #region �C�x���g

        /// <summary>���L�[���������u�Ԃɔ�������C�x���g</summary>
        public event Action LeftKeyDown;
        /// <summary>�E�L�[���������u�Ԃɔ�������C�x���g</summary>
        public event Action RightKeyDown;
        /// <summary>��L�[���������u�Ԃɔ�������C�x���g</summary>
        public event Action UpKeyDown;
        /// <summary>���L�[���������u�Ԃɔ�������C�x���g</summary>
        public event Action DownKeyDown;

        /// <summary>���L�[��������u�Ԃɔ�������C�x���g</summary>
        public event Action LeftKeyUp;
        /// <summary>�E�L�[��������u�Ԃɔ�������C�x���g</summary>
        public event Action RightKeyUp;
        /// <summary>��L�[��������u�Ԃɔ�������C�x���g</summary>
        public event Action UpKeyUp;
        /// <summary>���L�[��������u�Ԃɔ�������C�x���g</summary>
        public event Action DownKeyUp;

        /// <summary>���L�[�����������Ă���Ԃɔ�������C�x���g</summary>
        public event Action LeftKeyPress;
        /// <summary>�E�L�[�����������Ă���Ԃɔ�������C�x���g</summary>
        public event Action RightKeyPress;
        /// <summary>��L�[�����������Ă���Ԃɔ�������C�x���g</summary>
        public event Action UpKeyPress;
        /// <summary>���L�[�����������Ă���Ԃɔ�������C�x���g</summary>
        public event Action DownKeyPress;

        #endregion

        #region �v���p�e�B

        /// <summary>
        /// �W���C�X�e�B�b�N���g�p�\���ǂ������擾���܂��B
        /// </summary>
        public bool Enabled { get; private set; }

        #endregion

        #region �R���X�g���N�^
        /// <summary>
        /// �B��̃R���X�g���N�^�ł��B
        /// </summary>
        /// <param name="parent">�e�R���g���[��</param>
        public JoystickWrapper(Control parent)
        {
            // �ڑ�����Ă���f�o�C�X������
            DeviceList devList = Manager.GetDevices(DeviceClass.GameControl,
                EnumDevicesFlags.AttachedOnly);
            if (devList.Count < 1) return;

            // �ŏ��̃R���g���[�����擾���A�����ݒ�
            _device = new Device(devList.Cast<DeviceInstance>().First().InstanceGuid);
            _device.SetCooperativeLevel(parent,
                CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
            _device.SetDataFormat(DeviceDataFormat.Joystick);

            // �A�N�Z�X���擾
            _device.Acquire();

            // �e��C�x���g������
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

            // �L�[�|�[�����O�J�n
            _poling = new Thread(new ThreadStart(KeyPolling));
            _poling.Start();

            // �g�p��������
            Enabled = true;
        }
        #endregion

        #region KeyPolling - �L�[�|�[�����O�E�A�N�V�������s
        /// <summary>
        /// �L�[�̏�Ԃ��|�[�����O�Ŏ擾���A�Ή�����A�N�V�������s���܂��B
        /// </summary>
        private void KeyPolling()
        {
            while (true)
            {
                // �L�[�̏�Ԃ��擾
                _device.Poll();
                JoystickState state = _device.CurrentJoystickState;

                // �㉺�L�[
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

                // ���E�L�[
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

                // ����ȊO�̃{�^��
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

                // 60fps�����Ń��[�v
                Thread.Sleep(1000 / 60);
            }
        }
        #endregion

        #region AddButtonDownAction - �{�^�����������u�Ԃ̃A�N�V������ǉ�
        /// <summary>
        /// �{�^�����������u�Ԃ̃A�N�V������ǉ����܂��B
        /// </summary>
        /// <param name="act">�A�N�V����</param>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void AddButtonDownAction(Action act, int buttonIndex)
        {
            _buttonDownActions[buttonIndex] += act;
        }
        #endregion

        #region RemoveButtonDownAction - �{�^�����������u�Ԃ̃A�N�V����������
        /// <summary>
        /// �{�^�����������u�Ԃ̃A�N�V�������������܂��B
        /// </summary>
        /// <param name="act">�A�N�V����</param>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void RemoveButtonDownAction(Action act, int buttonIndex)
        {
            _buttonDownActions[buttonIndex] -= act;
        }
        #endregion

        #region ClearButtonDownAction - �{�^�����������u�Ԃ̃A�N�V������S����
        /// <summary>
        /// �{�^�����������u�Ԃ̃A�N�V������S�ď������܂��B
        /// </summary>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void ClearButtonDownAction(int buttonIndex)
        {
            _buttonDownActions[buttonIndex] = CreateEmptyAction();
        }
        #endregion

        #region AddButtonDownAction - �{�^����������u�Ԃ̃A�N�V������ǉ�
        /// <summary>
        /// �{�^����������u�Ԃ̃A�N�V������ǉ����܂��B
        /// </summary>
        /// <param name="act">�A�N�V����</param>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void AddButtonUpAction(Action act, int buttonIndex)
        {
            _buttonUpActions[buttonIndex] += act;
        }
        #endregion

        #region RemoveButtonDownAction - �{�^����������u�Ԃ̃A�N�V����������
        /// <summary>
        /// �{�^����������u�Ԃ̃A�N�V�������������܂��B
        /// </summary>
        /// <param name="act">�A�N�V����</param>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void RemoveButtonUpAction(Action act, int buttonIndex)
        {
            _buttonUpActions[buttonIndex] -= act;
        }
        #endregion

        #region ClearButtonDownAction - �{�^����������u�Ԃ̃A�N�V������S����
        /// <summary>
        /// �{�^����������u�Ԃ̃A�N�V������S�ď������܂��B
        /// </summary>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void ClearButtonUpAction(int buttonIndex)
        {
            _buttonUpActions[buttonIndex] = CreateEmptyAction();
        }
        #endregion

        #region AddButtonDownAction - �{�^�������������Ă���Ԃ̃A�N�V������ǉ�
        /// <summary>
        /// �{�^�������������Ă���Ԃ̃A�N�V������ǉ����܂��B
        /// </summary>
        /// <param name="act">�A�N�V����</param>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void AddButtonPressAction(Action act, int buttonIndex)
        {
            _buttonPressActions[buttonIndex] += act;
        }
        #endregion

        #region RemoveButtonDownAction - �{�^�������������Ă���Ԃ̃A�N�V����������
        /// <summary>
        /// �{�^�������������Ă���Ԃ̃A�N�V�������������܂��B
        /// </summary>
        /// <param name="act">�A�N�V����</param>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void RemoveButtonPressAction(Action act, int buttonIndex)
        {
            _buttonPressActions[buttonIndex] -= act;
        }
        #endregion

        #region ClearButtonDownAction - �{�^�������������Ă���Ԃ̃A�N�V������S����
        /// <summary>
        /// �{�^�������������Ă���Ԃ̃A�N�V������S�ď������܂��B
        /// </summary>
        /// <param name="buttonIndex">�{�^���̃C���f�b�N�X</param>
        public void ClearButtonPressAction(int buttonIndex)
        {
            _buttonPressActions[buttonIndex] = CreateEmptyAction();
        }
        #endregion

        #region Dispose - ���\�[�X�j��
        /// <summary>
        /// ���\�[�X��j�����܂��B
        /// </summary>
        public void Dispose()
        {
            _poling.Abort();
            _device.Dispose();
            _device = null;
        }
        #endregion

        #region CreateEmptyAction - ��̃A�N�V�����𐶐�
        /// <summary>
        /// ��̃A�N�V�����𐶐����܂��B
        /// </summary>
        /// <returns></returns>
        private Action CreateEmptyAction()
        {
            return new Action(() => { });
        }
        #endregion
    }
}
