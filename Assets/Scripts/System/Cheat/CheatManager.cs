using System;
using CommandTerminal;
using CryptoQuest.Gameplay;
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
        public static string ACTION_MAP_NAME = "Terminal";
        [SerializeField] private Animator _stateMachine;
        [SerializeField] private Terminal _terminal;
        public Terminal Terminal => _terminal;
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        public InputMediatorSO Input => _inputMediatorSO;

        [SerializeField] private GameStateSO _gameStateSO;
        public GameStateSO GameState => _gameStateSO;
        
        [SerializeField] public BattleInput _battleInput;
        public BattleInput BattleInput => _battleInput;

        private void OnEnable()
        {
            _inputMediatorSO.InputActions.Terminal.SetCallbacks(this);
            EnableTerminalInput();
        }

        public void EnableTerminalInput()
        {
#if !PRODUCTION_BUILD || ENABLE_CHEAT
            _inputMediatorSO.InputActions.Terminal.Enable();
#endif
        }

        private void OnDisable()
        {
            _inputMediatorSO.InputActions.Terminal.RemoveCallbacks(this);
        }

        private string _previouslyEnabledInputMap = "";

        public void EnableLastEnabledActionMap()
        {
            if (string.IsNullOrEmpty(_previouslyEnabledInputMap) == false)
                _inputMediatorSO.EnableInputMap(_previouslyEnabledInputMap);
        }

        public void CacheLastEnabledActionMap()
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

        public void InitCheats() { }

        public Action OnOpenTerminalPressed;

        public void OnOpenTerminal(InputAction.CallbackContext context)
        {
            if (context.performed) OnOpenTerminalPressed?.Invoke();
        }

        public static readonly int FullSize = Animator.StringToHash("FullSize");

        public void OnFullSizeModifier(InputAction.CallbackContext context) =>
            _stateMachine.SetBool(FullSize, context.performed);

        public Action<InputAction.CallbackContext> OnCommandNavigatePressed { get; set; }

        public void OnCommandNavigate(InputAction.CallbackContext context) =>
            OnCommandNavigatePressed?.Invoke(context);

        public Action OnCloseTerminalPressed { get; set; }

        public void OnClose(InputAction.CallbackContext context)
        {
            if (context.performed) OnCloseTerminalPressed?.Invoke();
        }

        public Action OnEnterPressed { get; set; }

        public void OnExecute(InputAction.CallbackContext context)
        {
            if (context.performed) OnEnterPressed?.Invoke();
        }

        public Action OnTabPressed { get; set; }

        public void OnComplete(InputAction.CallbackContext context)
        {
            if (context.performed) OnTabPressed?.Invoke();
        }
    }
}