using CommandTerminal;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CryptoQuest.System.Cheat
{
    /// <summary>
    /// To manage the in game terminal, currently I'm too lazy to implement state machine for this class
    /// </summary>
    public class CheatManager : MonoBehaviour, InputActions.ITerminalActions, ICheatInitializer
    {
        private const string ACTION_MAP_NAME = "Terminal";
        [SerializeField] private Terminal _terminal;
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        private void OnEnable()
        {
            _inputMediatorSO.InputActions.Terminal.SetCallbacks(this);
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            _inputMediatorSO.InputActions.Terminal.Enable();
#endif
        }

        private void OnDisable()
        {
            _inputMediatorSO.InputActions.Terminal.RemoveCallbacks(this);
        }

        private string _previouslyEnabledInputMap = "";

        private void ToggleTerminal()
        {
            UpdateLastEnabledActionMap();
            if (_terminal.State == TerminalState.Close)
            {
                _inputMediatorSO.DisableAllInput();
                if (_openingFullSizeTerminal)
                {
                    _terminal.OpenFull();
                }
                else
                {
                    _terminal.OpenSmall();
                }
            }
            else
            {
                CloseTerminal();
            }
        }

        private void EnableLastEnabledActionMap()
        {
            _inputMediatorSO.EnableInputMap(_previouslyEnabledInputMap);
        }

        private void UpdateLastEnabledActionMap()
        {
            var assetActionMaps = _inputMediatorSO.InputActions.asset.actionMaps;
            foreach (var actionMap in assetActionMaps)
            {
                if (actionMap.enabled && actionMap.name != ACTION_MAP_NAME)
                {
                    _previouslyEnabledInputMap = actionMap.name;
                    break;
                }
            }
        }

        public void InitCheats()
        {
            Terminal.Shell.AddCommand("hello", HelloWorld, 0, 0, "Says Hello world");
        }

        private void HelloWorld(CommandArg[] commandArgs)
        {
            Debug.Log("Hello world");
        }

        public void OnOpenTerminal(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                ToggleTerminal();
            }
        }

        private void CloseTerminal()
        {
            _terminal.Close();
            EnableLastEnabledActionMap();
        }

        private bool _openingFullSizeTerminal = false;

        public void OnFullSizeModifier(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _openingFullSizeTerminal = true;
            }
            else if (context.canceled)
            {
                _openingFullSizeTerminal = false;
            }
        }

        public void OnCommandNavigate(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var readValue = (int)context.ReadValue<float>();
                if (readValue == 1)
                {
                    _terminal.NextCommand();
                }
                else if (readValue == -1)
                {
                    _terminal.PreviousCommand();
                }
            }
        }

        public void OnClose(InputAction.CallbackContext context)
        {
            if (context.performed && _terminal.State != TerminalState.Close) CloseTerminal();
        }

        public void OnExecute(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _terminal.EnterPressed();
            }
        }

        public void OnComplete(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _terminal.TabPressed();
            }
        }
    }
}