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
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) CancelEvent?.Invoke();
        }
    }
}