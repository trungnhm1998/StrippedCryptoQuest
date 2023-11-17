using System.Collections.Generic;
using CryptoQuest.System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace CryptoQuest.Input
{
    // TODO: Move action map interfaces to separate scriptable objects
    public class InputMediatorSO : ScriptableObject,
        InputActions.IMapGameplayActions,
        InputActions.IMenusActions,
        InputActions.IDialoguesActions
    {
        #region Events

        #region Gameplay

        public event UnityAction<Vector2> MoveEvent;
        public event UnityAction InteractEvent;

        /// <summary>
        /// During field gameplay, this event is raised when the player presses the "Start"/"Tab"/"Escape" button.
        /// </summary>
        public event UnityAction ShowMainMenuEvent;

        #endregion

        #region Menu

        public event UnityAction CloseMainMenuEvent;
        public event UnityAction<float> TabChangeEvent;
        public event UnityAction<Vector2> MenuNavigateEvent;
        public event UnityAction<InputAction.CallbackContext> MenuNavigationContextEvent;
        public event UnityAction MenuConfirmedEvent;
        public event UnityAction MenuSubmitEvent;
        public event UnityAction MenuMouseMoveEvent;
        public event UnityAction MenuInteractEvent;
        public event UnityAction MenuCancelEvent;
        public event UnityAction MenuResetEvent;
        public event UnityAction MenuExecuteEvent;

        #endregion

        #region Dialogue

        public event UnityAction NextDialoguePressed;

        #endregion

        #endregion

        private InputActions _inputActions;

        public InputActions InputActions
        {
            get
            {
                CreateInputInstanceIfNeeded();
                return _inputActions;
            }
        }

        protected void OnEnable()
        {
            ServiceProvider.Provide(this);
            CreateInputInstanceIfNeeded();
        }

        private void OnDisable()
            => DisableAllInput();

        #region Main

        /// <summary>
        /// Disable all input in this mediator, not all input in action map
        /// </summary>
        public void DisableAllInput()
        {
            _inputCached.Clear();

            _inputActions.Menus.Disable();
            _inputActions.MapGameplay.Disable();
            _inputActions.Dialogues.Disable();
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
            DisableAllInput();
            _inputActions.Menus.Enable();
        }

        public void EnableDialogueInput()
        {
            DisableAllInput();
            _inputActions.Dialogues.Enable();
        }

        public void EnableMapGameplayInput()
        {
            DisableAllInput();
            _inputActions.MapGameplay.Enable();
        }

        public void EnableInputMap(string actionMapName)
        {
            DisableAllInput();
            var actionMap = _inputActions.asset.FindActionMap(actionMapName);
            actionMap.Enable();
        }

        public void DisableInputMap(string actionMapName)
        {
            var actionMap = _inputActions.asset.FindActionMap(actionMapName);
            actionMap.Disable();
        }

        #endregion

        #region MapGameplayActions

        private readonly List<Vector2> _inputCached = new();

        private void GameplayRemoveMove(Vector2 direction)
        {
            _inputCached.Remove(direction);

            if (_inputCached.Count <= 0)
            {
                MoveEvent?.Invoke(Vector3.zero);
                return;
            }

            // Index from end operator ^, this equal _inputCached.Count - 1
            MoveEvent?.Invoke(_inputCached[^1]);
        }

        private void GameplayMove(Vector2 direction)
        {
            if (!_inputCached.Contains(direction))
            {
                _inputCached.Add(direction);
            }

            MoveEvent?.Invoke(_inputCached[^1]);
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
            if (context.performed) ShowMainMenuEvent?.Invoke();
        }

        #endregion

        #region MenuActions

        public void OnNavigate(InputAction.CallbackContext context)
        {
            HandleMenuNavigate(context);
        }

        private void HandleMenuNavigate(InputAction.CallbackContext context)
        {
            if (!_inputActions.Menus.enabled) return;
            MenuNavigationContextEvent?.Invoke(context);
            if (context.performed) MenuNavigateEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnSubmit(InputAction.CallbackContext context)
        {
            if (context.performed) MenuSubmitEvent?.Invoke();
        }

        public void OnConfirm(InputAction.CallbackContext context)
        {
            if (context.performed) MenuConfirmedEvent?.Invoke();
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.performed) MenuCancelEvent?.Invoke();
        }

        public void OnClick(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        public void OnMouseMove(InputAction.CallbackContext context)
        {
            // if (context.performed) MenuMouseMoveEvent?.Invoke();
        }

        public void OnChangeTab(InputAction.CallbackContext context)
        {
            if (context.performed) TabChangeEvent?.Invoke(context.ReadValue<float>());
        }

        public void OnMenuInteract(InputAction.CallbackContext context)
        {
            if (context.performed) MenuInteractEvent?.Invoke();
        }

        public void OnMenuReset(InputAction.CallbackContext context)
        {
            if (context.performed) MenuResetEvent?.Invoke();
        }

        public void OnMenuExecute(InputAction.CallbackContext context)
        {
            if (context.performed) MenuExecuteEvent?.Invoke();
        }

        public void OnCloseMainMenu(InputAction.CallbackContext context)
        {
            if (context.performed) CloseMainMenuEvent?.Invoke();
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

        public bool LeftMouseDown()
            => Mouse.current.leftButton.isPressed;
    }
}