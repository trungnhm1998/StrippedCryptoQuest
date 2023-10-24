using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.BlackSmith
{
    public class BlackSmithInputManager : MonoBehaviour, InputActions.IBlackSmithActions
    {
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        public event UnityAction CancelEvent;
        public event UnityAction SubmitEvent;
        public event UnityAction<Vector2> NavigateEvent;

        private void OnEnable()
        {
            _inputMediatorSO.InputActions.BlackSmith.SetCallbacks(this);
        }

        private void OnDisable()
        {
            _inputMediatorSO.InputActions.BlackSmith.RemoveCallbacks(this);
        }

        public void EnableInput()
        {
            _inputMediatorSO.DisableAllInput();
            _inputMediatorSO.InputActions.BlackSmith.Enable();
        }

        public void DisableInput()
        {
            _inputMediatorSO.InputActions.BlackSmith.Disable();
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
        #endregion
    }
}