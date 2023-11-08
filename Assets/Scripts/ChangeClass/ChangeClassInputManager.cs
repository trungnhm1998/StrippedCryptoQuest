using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.ChangeClass
{
    public class ChangeClassInputManager : MonoBehaviour, InputActions.IChangeClassActions
    {
        [SerializeField] private InputMediatorSO _inputMediatorSO;
        public event UnityAction CancelEvent;
        public event UnityAction SubmitEvent;
        public event UnityAction DetailEvent;
        public event UnityAction<Vector2> NavigateEvent;

        private void OnEnable()
        {
            _inputMediatorSO.InputActions.ChangeClass.SetCallbacks(this);
        }

        private void OnDisable()
        {
            _inputMediatorSO.InputActions.ChangeClass.RemoveCallbacks(this);
        }

        public void EnableInput()
        {
            _inputMediatorSO.DisableAllInput();
            _inputMediatorSO.InputActions.ChangeClass.Enable();
        }

        public void DisableInput()
        {
            _inputMediatorSO.InputActions.ChangeClass.Disable();
            _inputMediatorSO.EnableMapGameplayInput();
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

        public void OnShowDetail(InputAction.CallbackContext context)
        {
            if (context.performed) DetailEvent?.Invoke();
        }
        #endregion
    }
}
