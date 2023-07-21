using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using NotImplementedException = System.NotImplementedException;

namespace CryptoQuest.Input
{
    // TODO: Move action map interfaces to separate scriptable objects
    public class InputMediatorSO : ScriptableObject, InputActions.IMapGameplayActions, InputActions.IMenusActions,
        InputActions.IDialoguesActions
    {
        #region Events

        #region Gameplay

        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction PauseEvent;
        public event UnityAction InteractEvent;
        public event UnityAction OpenMainMenuEvent;

        #endregion

        #region Menu

        public event UnityAction MenuConfirmedEvent;
        public event UnityAction MenuSubmitEvent;
        public event UnityAction MenuNavigateEvent;
        public event UnityAction MoveSelectionEvent;
        public event UnityAction MouseMoveEvent;
        public event UnityAction MenuTabPressed;
        public event UnityAction CancelEvent;

        #endregion

        #region Dialogue

        public event UnityAction NextDialoguePressed;

        #endregion

        #endregion

        private InputActions _inputActions;

        private void OnEnable()
        {
            CreateInputInstanceIfNeeded();

            _inputActions.Disable();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        #region Main

        public void DisableAllInput()
        {
            _inputActions.Disable();
            _inputActions.MapGameplay.Disable();
            _inputActions.Menus.Disable();
        }

        private void CreateInputInstanceIfNeeded()
        {
            if (_inputActions != null) return;

            _inputActions = new InputActions();
            _inputActions.Menus.SetCallbacks(this);
            _inputActions.MapGameplay.SetCallbacks(this);
            _inputActions.Dialogues.SetCallbacks(this);
        }

        public void EnableMenuInput()
        {
            _inputActions.Menus.Enable();
            _inputActions.MapGameplay.Disable();
            _inputActions.Dialogues.Disable();
        }

        public void EnableDialogueInput()
        {
            _inputActions.Dialogues.Enable();
            _inputActions.MapGameplay.Disable();
            _inputActions.Menus.Disable();
        }

        public void EnableMapGameplayInput()
        {
            _inputCached.Clear();
            _inputActions.Menus.Disable();
            _inputActions.MapGameplay.Enable();
            _inputActions.Dialogues.Disable();
        }

        #endregion

        #region MapGameplayActions

        private List<Vector2> _inputCached = new();

        private void GameplayRemoveMove(Vector2 direction)
        {
            _inputCached.Remove(direction);  
            
            if (_inputCached.Count <= 0)
            {
                MoveEvent?.Invoke(Vector3.zero);
                return;
            }
            MoveEvent?.Invoke(_inputCached[_inputCached.Count - 1]);
        }

        private void GameplayMove(Vector2 direction)
        {
            if (!_inputCached.Contains(direction))
            {
                _inputCached.Add(direction);
            }
            MoveEvent?.Invoke(_inputCached[_inputCached.Count - 1]);
        }

        public void OnMoveUp(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.up);
                return;
            }
            GameplayMove(Vector2.up);
        }
        public void OnMoveDown(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.down);
                return;
            }
            GameplayMove(Vector2.down);
        }
        public void OnMoveLeft(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.left);
                return;
            }
            GameplayMove(Vector2.left);
        }
        public void OnMoveRight(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                GameplayRemoveMove(Vector2.right);
                return;
            }
            GameplayMove(Vector2.right);
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed) InteractEvent?.Invoke();
        }

        public void OnMainMenu(InputAction.CallbackContext context)
        {
            if (context.performed) OpenMainMenuEvent?.Invoke();
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed) PauseEvent?.Invoke();
        }

        #endregion

        #region MenuActions

        public void OnMoveSelection(InputAction.CallbackContext context)
        {
            if (context.performed) MoveSelectionEvent?.Invoke();
        }

        public void OnNavigate(InputAction.CallbackContext context)
        {
            if (context.performed) MenuNavigateEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
                MenuConfirmedEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) CancelEvent?.Invoke();
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed) MenuSubmitEvent?.Invoke();
        }

        public bool LeftMouseDown() => Mouse.current.leftButton.isPressed;
        public void OnClick(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        public void OnNextSelection(InputAction.CallbackContext context)
        {
            if (context.performed) MenuTabPressed?.Invoke();
        }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed) MouseMoveEvent?.Invoke();
        }

        #endregion

        #region Dialogue

        public void OnNextDialogue(InputAction.CallbackContext context)
        {
            if (context.performed) NextDialoguePressed?.Invoke();
        }

        public void OnEscape(InputAction.CallbackContext context)
        {
            Debug.LogWarning("Escape pressed, but not implemented");
        }

        #endregion
    }
}