using CryptoQuest.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Shop.UI
{
    public class ShopInputManager : MonoBehaviour, InputActions.IShopActions
    {
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        public event UnityAction ExitEvent;
        public event UnityAction BackEvent;
        public event UnityAction SubmitEvent;
        public event UnityAction<float> ChangeTabEvent;

        private void OnEnable()
        {
            _inputMediatorSO.InputActions.Shop.SetCallbacks(this);
        }
        private void OnDisable()
        {
            _inputMediatorSO.InputActions.Shop.RemoveCallbacks(this);
        }

        #region Events
        public void OnBack(InputAction.CallbackContext context)
        {
            if(context.performed) BackEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) ExitEvent?.Invoke();
        }

        public void OnChangeTab(InputAction.CallbackContext context)
        {
            if(context.performed) ChangeTabEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if(context.performed) SubmitEvent?.Invoke();
        }
        #endregion

        public void EnableInput()
        {
            _inputMediatorSO.DisableAllInput();
            _inputMediatorSO.InputActions.Shop.Enable();
        }    

        public void DisableInput()
        {
            _inputMediatorSO.InputActions.Shop.Disable();
        }    
        public void EnableNextInput()
        {
            DisableInput();
            _inputMediatorSO.EnableDialogueInput();
        }    
    }
}
