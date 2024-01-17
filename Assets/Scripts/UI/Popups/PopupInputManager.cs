using System;
using System.Linq;
using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace CryptoQuest.UI.Popups
{
    // Cache last action map and disable all input and after close popup enable cached action map
    public class PopupInputManager : MonoBehaviour, InputActions.IPopupActions
    {
        public static string POPUP_ACTION_MAP_NAME = "Popup";
        public event Action ClosePopupEvent;

        // Disable this to prevent user can still navigate or confirm while popup is showing
        [SerializeField] private InputSystemUIInputModule _inputSystemModule;
        [SerializeField] private InputMediatorSO _inputSO;
        [SerializeField] private string[] _ignoredActionMaps;
        private string _previouslyEnabledInputMap = "";

        private void OnEnable()
        {
            if (_inputSystemModule == null) _inputSystemModule = FindObjectOfType<InputSystemUIInputModule>();
        }

        private void EnableLastEnabledActionMap()
        {
            if (string.IsNullOrEmpty(_previouslyEnabledInputMap)) return;
            _inputSO.EnableInputMap(_previouslyEnabledInputMap);
        }

        private void CacheLastEnabledActionMap()
        {
            var assetActionMaps = _inputSO.InputActions.asset.actionMaps;
            foreach (var actionMap in assetActionMaps)
            {
                if (actionMap.enabled && !_ignoredActionMaps.Contains(actionMap.name))
                {
                    _previouslyEnabledInputMap = actionMap.name;
                    break;
                }
            }
        }

        public void EnableInput()
        {
            _inputSO.InputActions.Popup.SetCallbacks(this);
            CacheLastEnabledActionMap();
            _inputSO.DisableAllInput();
            _inputSO.InputActions.Popup.Enable();
            _inputSystemModule.enabled = false;
        }
        
        public void DisableInput()
        {
            _inputSO.InputActions.Popup.Disable();
            _inputSO.InputActions.Popup.RemoveCallbacks(this);
            _inputSystemModule.enabled = true;
            EnableLastEnabledActionMap();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) ClosePopupEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.performed) ClosePopupEvent?.Invoke();
        }
    }
}