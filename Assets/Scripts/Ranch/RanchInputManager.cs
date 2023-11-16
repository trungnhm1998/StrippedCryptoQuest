using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Ranch
{
    public class RanchInputManager : MonoBehaviour, InputActions.IFarmActions
    {
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        public event UnityAction<Vector2> NavigateEvent;
        public event UnityAction<float> ChangeTabEvent;
        public event UnityAction SubmitEvent;
        public event UnityAction CancelEvent;
        public event UnityAction InteractEvent;
        public event UnityAction ExecuteEvent;
        public event UnityAction ResetEvent;
        public event UnityAction ExitEvent;
        public event UnityAction ShowDetailEvent;

        public void EnableInput()
        {
            _inputMediatorSO.InputActions.Farm.SetCallbacks(this);
            _inputMediatorSO.DisableAllInput();
            _inputMediatorSO.InputActions.Farm.Enable();
        }

        public void DisableInput()
        {
            _inputMediatorSO.InputActions.Farm.RemoveCallbacks(this);
            _inputMediatorSO.InputActions.Farm.Disable();
            _inputMediatorSO.EnableMapGameplayInput();
        }

        public void EnableDialogueInput()
        {
            _inputMediatorSO.InputActions.Farm.Disable();
            _inputMediatorSO.EnableDialogueInput();
        }

        #region Events

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed) NavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed) SubmitEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) CancelEvent?.Invoke();
        }

        public void OnChangeTab(InputAction.CallbackContext context)
        {
            if (context.performed) ChangeTabEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnShowDetail(InputAction.CallbackContext context)
        {
            if (context.performed) ShowDetailEvent?.Invoke();
        }

        public void OnReset(InputAction.CallbackContext context)
        {
            if (context.performed) ResetEvent?.Invoke();
        }

        public void OnExecute(InputAction.CallbackContext context)
        {
            if (context.performed) ExecuteEvent?.Invoke();
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed) InteractEvent?.Invoke();
        }

        public void OnExit(InputAction.CallbackContext context)
        {
            if (context.performed) ExitEvent?.Invoke();
        }

        #endregion
    }
}