using CryptoQuest.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.System.CutsceneSystem
{
    public class CutsceneInput : ScriptableObject, InputActions.ICutsceneActions
    {
        [SerializeField] private InputMediatorSO _inputMediatorSO;

        public event UnityAction<Vector2> NavigateEvent;

        public event UnityAction SubmitEvent;

        public void EnableInput()
        {
            _inputMediatorSO.InputActions.Cutscene.SetCallbacks(this);
            _inputMediatorSO.DisableAllInput();
            _inputMediatorSO.InputActions.Cutscene.Enable();
        }

        public void DisableInput()
        {
            _inputMediatorSO.InputActions.Cutscene.RemoveCallbacks(this);
            _inputMediatorSO.InputActions.Cutscene.Disable();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed) NavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed) SubmitEvent?.Invoke();
        }
    }
}